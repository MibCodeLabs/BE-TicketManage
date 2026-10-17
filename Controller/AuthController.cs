using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ticketManage.DTOs.Auth;
using ticketManage.Mappers;
using ticketManage.Services;
using ticketManage.Utils;

namespace ticketManage.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly AuthService _authService;
        private readonly UserService _userService;

        private readonly IConfiguration _configuration;

        public AuthController(
            AuthService authService,
            IConfiguration configuration,
            ILogger<AuthController> logger,
            UserService userService
        )
        {
            _authService = authService;
            _configuration = configuration;
            _logger = logger;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            return Ok(await _authService.RegisterAsync(request));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var loginResponse = await _authService.LoginAsync(request);

            if (loginResponse == null)
            {
                _logger.LogWarning("Login failed: Email:{@email}", request.Email);
                return Unauthorized(new { message = "Invalid email or password." });
            }
            ResponseUtils.AddHttpOnlyCookie(
                Response,
                TokenType.Access.ToString(),
                loginResponse.AccessToken,
                int.Parse(_configuration[Constants.JWT_ACCESS_EXPIRATION_MINUTES]!)
            );
            ResponseUtils.AddHttpOnlyCookie(
                Response,
                TokenType.Refresh.ToString(),
                loginResponse.RefreshToken,
                int.Parse(_configuration[Constants.JWT_REFRESH_EXPIRATION_MINUTES]!)
            );

            return Ok(AuthMapper.ToLoginApiResponse(loginResponse));
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            return Ok(AuthMapper.ToLoggedInUserResponse(await this._userService.GetCurrentUser()));
        }

        [Authorize(AuthenticationSchemes = "RefreshToken")]
        [HttpGet("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var refreshResponse = await this._authService.Refresh();
            ResponseUtils.AddHttpOnlyCookie(
                Response,
                TokenType.Access.ToString(),
                refreshResponse.AccessToken,
                int.Parse(_configuration[Constants.JWT_ACCESS_EXPIRATION_MINUTES]!)
            );
            ResponseUtils.AddHttpOnlyCookie(
                Response,
                TokenType.Refresh.ToString(),
                refreshResponse.RefreshToken,
                int.Parse(_configuration[Constants.JWT_REFRESH_EXPIRATION_MINUTES]!)
            );

            return Ok();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();

            ResponseUtils.DeleteCookie(Response, TokenType.Access.ToString());
            ResponseUtils.DeleteCookie(Response, TokenType.Refresh.ToString());
            return Ok(new { message = "Logout successful." });
        }
    }
}
