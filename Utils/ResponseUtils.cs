using Microsoft.AspNetCore.Http;

namespace ticketManage.Utils
{
    public class ResponseUtils
    {
        public static void AddCookie(
            HttpResponse response,
            string key,
            string value,
            CookieOptions cookieOptions
        )
        {
            response.Cookies.Append(key, value, cookieOptions);
        }

        public static CookieOptions GenerateSecureCookieOptions(int expirationMinutes)
        {
            return new CookieOptions
            {
                HttpOnly = Constants.SECURE_COOKIE_HTTP_ONLY_FLAG,
                Secure = Constants.SECURE_COOKIE_SECURE_FLAG,
                SameSite = Constants.SECURE_COOKIE_SAMESITE,
                Expires = DateTimeOffset.UtcNow.AddMinutes(expirationMinutes),
                Path = Constants.SECURE_COOKIE_PATH,
            };
        }

        public static void AddHttpOnlyCookie(
            HttpResponse response,
            string key,
            string value,
            int expirationMinutes
        )
        {
            AddCookie(response, key, value, GenerateSecureCookieOptions(expirationMinutes));
        }
    }
}
