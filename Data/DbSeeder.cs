using Microsoft.AspNetCore.Identity;
using ticketManage.Models;
using ticketManage.Utils;

namespace ticketManage.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();

            const string adminEmail = "admin@main.com";
            const string adminPassword = "ChangeMe123!";

            var admin = await userManager.FindByEmailAsync(adminEmail);

            if (admin != null)
                return;

            admin = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,

                FullName = "System Administrator",
                AccountType = AccountType.Admin,

                ProfilePicture = "/images/default-avatar.svg",

                CreatedBy = "System",
                ModifiedBy = "System",
                IsDeleted = false,
            };

            var result = await userManager.CreateAsync(admin, adminPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    Environment.NewLine,
                    result.Errors.Select(e => e.Description)
                );

                throw new InvalidOperationException(
                    $"Failed to create admin user:{Environment.NewLine}{errors}"
                );
            }
        }
    }
}
