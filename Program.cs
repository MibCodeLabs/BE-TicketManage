using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using MySqlConnector;
using ticketManage.Data;
using ticketManage.DTOs.FileAsset;
using ticketManage.DTOs.Ticket;
using ticketManage.DTOs.User;
using ticketManage.Exceptions;
using ticketManage.Models;
using ticketManage.Services;
using ticketManage.Utils;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var connectionString =
    configuration.GetConnectionString(Constants.DB_CONNECTION_STRING)
    ?? throw new InvalidOperationException("Database connection string is not configured.");
builder.Services.AddScoped<AuditSaveChangesInterceptor>();
builder.Services.AddDbContext<AppDbContext>(
    (serviceProvider, options) =>
    {
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        options.AddInterceptors(serviceProvider.GetRequiredService<AuditSaveChangesInterceptor>());
    }
);
builder.Services.AddScoped<CurrentUserService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<FileAssetService>();
builder.Services.AddScoped<StorageObjectService>();
builder.Services.AddScoped<TicketService>();
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
    })
    .AddJwtBearer(
        "RefreshToken",
        options =>
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
                    context.Token = context.Request.Cookies[TokenType.Refresh.ToString()];
                    return Task.CompletedTask;
                },

                OnAuthenticationFailed = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetRequiredService<
                        ILogger<Program>
                    >();
                    logger.LogWarning(context.Exception, "Refresh token authentication failed.");
                    return Task.CompletedTask;
                },
            };
        }
    );

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

var odataBuilder = new ODataConventionModelBuilder();
odataBuilder.ComplexType<BasicUser>();
odataBuilder.ComplexType<BasicFileAsset>();
odataBuilder.EntitySet<TicketResponse>("Ticket");

odataBuilder.EnableLowerCamelCase();
builder
    .Services.AddControllers()
    .AddOData(options =>
        options
            .Select()
            .Filter()
            .OrderBy()
            .Count()
            .SetMaxTop(Constants.DEFAULT_MAX_PAGE_SIZE)
            .AddRouteComponents("q/api", odataBuilder.GetEdmModel())
    );
;
builder.Services.AddHealthChecks().AddDbContextCheck<AppDbContext>();

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context
            .Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()
            ?.Error;

        context.Response.ContentType = Constants.CONTENT_TYPE_JSON;
        context.Response.StatusCode = exception is AppException appException
            ? appException.StatusCode
            : Constants.INTERNAL_SERVER_ERROR_STATUS_CODE;

        await context.Response.WriteAsJsonAsync(
            new
            {
                success = false,
                message = exception is AppException
                    ? exception.Message
                    : Constants.INTERNAL_SERVER_ERROR_MESSAGE,
            }
        );
    });
});

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
