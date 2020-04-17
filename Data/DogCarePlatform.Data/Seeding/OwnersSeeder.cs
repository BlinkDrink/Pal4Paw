namespace DogCarePlatform.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    using DogCarePlatform.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public class OwnersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await SeedOwnerAsync(userManager, "owner@abv.bg");
        }

        private static async Task SeedOwnerAsync(UserManager<ApplicationUser> userManager, string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user != null)
            {
                if (user.Owners.Count == 0)
                {
                    var owner = new Owner
                    {
                        UserId = user.Id,
                        DogsDescription = "Моите кучета са 3. Голдън ретрийвър, корги и лабрадор. И трите са на по 4 години.",
                        Address = "Дианабад, ул. Тинтява 15-17",
                        Gender = Gender.Male,
                        FirstName = "Майкъл",
                        MiddleName = "Минков",
                        LastName = "Димитров",
                        ImageUrl = "https://res.cloudinary.com/add cloud name here/image/upload/v1586865183/xecbsn573zxmnpf9hmox.jpg",
                        PhoneNumber = user.PhoneNumber,
                    };

                    user.Owners.Add(owner);
                }
            }
        }
    }
}
