using System.Security.Claims;

namespace ticketManage.Services
{
    public class CurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUserId()
        {
            var principal = _httpContextAccessor.HttpContext?.User;
            return principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public string GetCurrentUserIdChecked()
        {
            var principal = _httpContextAccessor.HttpContext?.User;
            return principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new UnauthorizedAccessException("User is not authenticated.");
        }
    }
}
