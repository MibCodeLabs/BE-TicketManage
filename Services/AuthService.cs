using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ticketManage.DTOs.Auth;
using ticketManage.Mappers;
using ticketManage.Models;

namespace ticketManage.Services
{
    public class AuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly UserService _userService;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtService _jwtService;
        private readonly RefreshTokenService _refreshTokenService;

        public AuthService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            JwtService jwtService,
            IHttpContextAccessor httpContextAccessor,
            RefreshTokenService refreshTokenService,
            UserService userService
        )
        {
            _refreshTokenService = refreshTokenService;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }

        public async Task<LoginResponse?> LoginAsync(ticketManage.DTOs.Auth.LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null || user.IsDeleted)
            {
                return null;
            }

            if (
                !(
                    await _signInManager.CheckPasswordSignInAsync(
                        user,
                        request.Password,
                        lockoutOnFailure: true
                    )
                ).Succeeded
            )
            {
                return null;
            }

            string accessToken = _jwtService.GenerateAccessToken(user).ToString();
            string refreshToken = await _jwtService.GenerateAndSaveRefreshToken(user);

            if (refreshToken == null)
            {
                throw new InvalidOperationException("Login Error, Contact Support");
            }

            return AuthMapper.ToLoginResponse(
                AuthMapper.ToLoggedInUserResponse(user),
                "Login successful",
                accessToken,
                refreshToken.ToString()
            );
        }

        public async Task<IdentityResult> RegisterAsync(
            ticketManage.DTOs.Auth.RegisterRequest registerRequest
        )
        {
            var existingUser = await _userManager.FindByEmailAsync(registerRequest.Email);

            if (existingUser != null)
            {
                return IdentityResult.Failed(
                    new IdentityError
                    {
                        Code = "EmailAlreadyExists",
                        Description = "A user with this email already exists.",
                    }
                );
            }

            var user = AuthMapper.ToUser(registerRequest);

            return await _userManager.CreateAsync(user, registerRequest.Password);
        }

        public async Task LogoutAsync()
        {
            var principal = _httpContextAccessor.HttpContext?.User;
            var userId = principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                await _refreshTokenService.DeleteAllByUserId(userId);
            }
        }

        public async Task<RefreshResponse> Refresh()
        {
            var user = await _userService.GetCurrentUser();
            string accessToken = _jwtService.GenerateAccessToken(user).ToString();
            string refreshToken = await _jwtService.GenerateAndSaveRefreshToken(user);

            Console.WriteLine($"Tokens identical: {accessToken == refreshToken}");

            return RefreshResponse.Create(refreshToken, accessToken);
        }
    }
}
