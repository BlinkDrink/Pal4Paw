namespace DogCarePlatform.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using DogCarePlatform.Common;
    using DogCarePlatform.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public class UsersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await SeedUserAsync(userManager, "admin@admin.com");
        }

        private static async Task SeedUserAsync(UserManager<ApplicationUser> userManager, string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                var appUser = new ApplicationUser();
                appUser.UserName = username;
                appUser.Email = username;

                var result = userManager.CreateAsync(appUser, "nsusb2020").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(appUser, GlobalConstants.AdministratorRoleName).Wait();
                }
            }
        }
    }
}
