using Riok.Mapperly.Abstractions;
using ticketManage.DTOs.Auth;
using ticketManage.Models;
using ticketManage.Utils;

namespace ticketManage.Mappers
{
    [Mapper]
    public static partial class AuthMapper
    {
        [MapProperty(
            nameof(User.AccountType),
            nameof(LoginUserResponse.AccountType),
            Use = nameof(AccountTypeToString)
        )]
        public static partial LoginUserResponse ToLoginUserResponse(User user);

        [MapProperty(nameof(user), nameof(LoginResponse.User))]
        public static partial LoginResponse ToLoginResponse(
            LoginUserResponse user,
            string message,
            string accessToken,
            string refreshToken
        );

        [MapProperty(nameof(RegisterRequest.Email), nameof(User.UserName))] //username is required in asp identity hence we will use email as username
        public static partial User ToUser(RegisterRequest registerRequest);

        private static string AccountTypeToString(AccountType accountType) =>
            accountType.ToString();

        public static partial LoginApiResponse ToLoginApiResponse(LoginResponse loginResponse);

        [MapProperty(nameof(accessToken), nameof(RefreshResponse.AccessToken))]
        [MapProperty(nameof(refreshToken), nameof(RefreshResponse.RefreshToken))]
        public static partial RefreshResponse ToRefreshResponse(
            string accessToken,
            string refreshToken
        );
    }
}
