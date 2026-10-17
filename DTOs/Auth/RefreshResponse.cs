namespace ticketManage.DTOs.Auth
{
    public class RefreshResponse
    {
        public string AccessToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";

        public static RefreshResponse Create(string refreshToken, string accessToken)
        {
            return new RefreshResponse { AccessToken = accessToken, RefreshToken = refreshToken };
        }
    };
}
