using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using ticketManage.Data;
using ticketManage.Models;
using ticketManage.Services;
using ticketManage.Utils;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var connectionString =
    configuration.GetConnectionString(Constants.DB_CONNECTION_STRING)
    ?? throw new InvalidOperationException("Database connection string is not configured.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<RefreshTokenService>();
builder.Services.AddScoped<JwtService>();

builder
    .Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

var jwtKey =
    configuration[Constants.JWT_KEY]
    ?? throw new InvalidOperationException("JWT key is not configured.");

builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = configuration[Constants.JWT_ISSUER],
            ValidAudience = configuration[Constants.JWT_AUDIENCE],

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),

            ClockSkew = TimeSpan.Zero,
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies[TokenType.Access.ToString()];

                return Task.CompletedTask;
            },

            OnAuthenticationFailed = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<
                    ILogger<Program>
                >();

                logger.LogWarning(context.Exception, "JWT authentication failed.");

                return Task.CompletedTask;
            },
        };
    });

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };

    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AngularFrontend",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }
    );
});

builder.Services.AddControllers();
builder.Services.AddHealthChecks().AddDbContextCheck<AppDbContext>();

var app = builder.Build();

var schemeProvider = app.Services.GetRequiredService<IAuthenticationSchemeProvider>();

var authenticateScheme = await schemeProvider.GetDefaultAuthenticateSchemeAsync();

var challengeScheme = await schemeProvider.GetDefaultChallengeSchemeAsync();

app.Logger.LogInformation(
    "Default authenticate scheme: {Scheme}",
    authenticateScheme?.Name ?? "None"
);

app.Logger.LogInformation("Default challenge scheme: {Scheme}", challengeScheme?.Name ?? "None");

await using (var scope = app.Services.CreateAsyncScope())
{
    await DbSeeder.SeedAsync(scope.ServiceProvider);
}

app.UseCors("AngularFrontend");
app.UseStaticFiles();

app.Use(
    async (context, next) =>
    {
        app.Logger.LogInformation(
            "Request: {Method} {Path}",
            context.Request.Method,
            context.Request.Path
        );

        await next();

        app.Logger.LogInformation("Response status: {StatusCode}", context.Response.StatusCode);
    }
);

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");

app.MapGet(
    "/healthz",
    async (IConfiguration config) =>
    {
        var connectionString = config.GetConnectionString(Constants.DB_CONNECTION_STRING);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return Results.Json(
                new { database = "DOWN", error = "Connection string is not configured." },
                statusCode: StatusCodes.Status503ServiceUnavailable
            );
        }

        try
        {
            await using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            return Results.Ok(new { database = "UP", state = connection.State.ToString() });
        }
        catch (MySqlException)
        {
            return Results.Json(
                new { database = "DOWN" },
                statusCode: StatusCodes.Status503ServiceUnavailable
            );
        }
    }
);

app.MapControllers();

app.Run();
