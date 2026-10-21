namespace ticketManage.Utils
{
    public static class Constants
    {
        public const string JWT_ISSUER = "Jwt:Issuer";
        public const string JWT_AUDIENCE = "Jwt:Audience";
        public const string JWT_KEY = "Jwt:Key";
        public const string JWT_ACCESS_EXPIRATION_MINUTES = "Jwt:AccessExpirationMinutes";
        public const string JWT_REFRESH_EXPIRATION_MINUTES = "Jwt:RefreshExpirationMinutes";
        public const string JWT_CLAIM_ACCOUNT_TYPE = "accountType";
        public const string JWT_CLAIM_TOKEN_TYPE = "type";
        public const string DB_CONNECTION_STRING = "DefaultConnection";
        public const SameSiteMode SECURE_COOKIE_SAMESITE = SameSiteMode.Strict;
        public const bool SECURE_COOKIE_SECURE_FLAG = true;
        public const bool SECURE_COOKIE_HTTP_ONLY_FLAG = true;
        public const string SECURE_COOKIE_PATH = "/";
        public const string DEFAULT_SYSTEM = "system";
        public const int DEFAULT_PAGE_SIZE = 10;
        public const int DEFAULT_MAX_PAGE_SIZE = 50;
        public const int DEFAULT_START_PAGE_NUMBER = 0;
    }
}
