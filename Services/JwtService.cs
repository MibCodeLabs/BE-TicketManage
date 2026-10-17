using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ticketManage.Models;
using ticketManage.Utils;

namespace ticketManage.Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;
        private readonly RefreshTokenService _refreshTokenService;

        public JwtService(IConfiguration configuration, RefreshTokenService refreshTokenService)
        {
            _configuration = configuration;
            _refreshTokenService = refreshTokenService;
        }

        public string GenerateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim(Constants.JWT_CLAIM_ACCOUNT_TYPE, user.AccountType.ToString()),
                new Claim(Constants.JWT_CLAIM_TOKEN_TYPE, TokenType.Access.ToString()),
            };

            return this.CreateToken(
                claims,
                int.Parse(_configuration[Constants.JWT_ACCESS_EXPIRATION_MINUTES]!),
                this.GetCredentials()
            );
        }

        private SymmetricSecurityKey GetKey()
        {
            return new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration[Constants.JWT_KEY]!)
            );
        }

        private SigningCredentials GetCredentials()
        {
            return new SigningCredentials(this.GetKey(), SecurityAlgorithms.HmacSha256);
        }

        private string CreateToken(
            List<Claim> claims,
            int expirationMinutes,
            SigningCredentials credentials
        )
        {
            return new JwtSecurityTokenHandler().WriteToken(
                new JwtSecurityToken(
                    issuer: _configuration[Constants.JWT_ISSUER],
                    audience: _configuration[Constants.JWT_AUDIENCE],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                    signingCredentials: credentials
                )
            );
        }

        private string GenerateRefreshToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim(Constants.JWT_CLAIM_TOKEN_TYPE, TokenType.Refresh.ToString()),
            };

            var refreshToken = this.CreateToken(
                claims,
                int.Parse(_configuration[Constants.JWT_REFRESH_EXPIRATION_MINUTES]!),
                this.GetCredentials()
            );

            return refreshToken;
        }

        public async Task<string> GenerateAndSaveRefreshToken(User user)
        {
            var refreshToken = this.GenerateRefreshToken(user);
            await _refreshTokenService.RotateRefreshTokenAsync(
                RefreshToken.Create(refreshToken, user)
            );
            return refreshToken;
        }
    }
}
