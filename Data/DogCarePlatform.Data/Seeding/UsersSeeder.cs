namespace DogCarePlatform.Data.Seeding
{
    using System;
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
            await SeedUserAsync(userManager, "dogsitter@abv.bg");
            await SeedUserAsync(userManager, "owner@abv.bg");

        }

        private static async Task SeedUserAsync(UserManager<ApplicationUser> userManager, string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                var appUser = new ApplicationUser();
                appUser.UserName = username;
                appUser.Email = username;
                appUser.PhoneNumber = "0883421321";

                IdentityResult result = new IdentityResult();

                if (username == "dogsitter@abv.bg")
                {
                    result = userManager.CreateAsync(appUser, "123456").Result;
                    appUser.PhoneNumber = "0883421321";
                }
                else if (username == "admin@admin.com")
                {
                    result = userManager.CreateAsync(appUser, "nsusb2020").Result;
                    appUser.PhoneNumber = "0883424444";
                }
                else
                {
                    result = userManager.CreateAsync(appUser, "123456").Result;
                    appUser.PhoneNumber = "0883421234";
                }

                if (result.Succeeded)
                {
                    if (username == "admin@admin.com")
                    {
                        userManager.AddToRoleAsync(appUser, GlobalConstants.AdministratorRoleName).Wait();
                    }
                    else if (username == "owner@abv.bg")
                    {
                        userManager.AddToRoleAsync(appUser, GlobalConstants.OwnerRoleName).Wait();
                    }
                    else
                    {
                        userManager.AddToRoleAsync(appUser, GlobalConstants.DogsitterRoleName).Wait();
                    }
                }
            }
        }
    }
}
