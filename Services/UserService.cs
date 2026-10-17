using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using ticketManage.Models;

namespace ticketManage.Services
{
    public class UserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<User> GetCurrentUser()
        {
            var principal = _httpContextAccessor.HttpContext?.User;

            var userId =
                principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new UnauthorizedAccessException("User is not authenticated.");

            return await _userManager.FindByIdAsync(userId)
                ?? throw new UnauthorizedAccessException("User is not authenticated.Id invalid");
        }

        public async Task<User> GetUserById(string userId)
        {
            return await _userManager.FindByIdAsync(userId)
                ?? throw new UnauthorizedAccessException("User is not authenticated.Id invalid");
        }
    }
}
