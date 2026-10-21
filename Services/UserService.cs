using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using ticketManage.Models;
using ticketManage.Utils;

namespace ticketManage.Services
{
    public class UserService
    {
        private readonly UserManager<User> _userManager;
        private readonly CurrentUserService _currentUserService;

        public UserService(UserManager<User> userManager, CurrentUserService currentUserService)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        public async Task<User> GetCurrentUser()
        {
            var userId = this._currentUserService.GetCurrentUserIdChecked();
            return await this.GetUserById(userId);
        }

        public async Task<User> GetUserById(string userId)
        {
            if (Utility.IsNullOrEmpty(userId))
            {
                throw new BadHttpRequestException("User is not present");
            }
            return await _userManager.FindByIdAsync(userId)
                ?? throw new UnauthorizedAccessException("User is invalid");
        }
    }
}
