namespace DogCarePlatform.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    using DogCarePlatform.Data.Common.Repositories;
    using DogCarePlatform.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public class DogsittersSeeder : ISeeder
    {
        private static readonly IDeletableEntityRepository<Dogsitter> dogsitterRepository;

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await SeedDogsitterAsync(userManager, "dogsitter@abv.bg");
        }

        private static async Task SeedDogsitterAsync(UserManager<ApplicationUser> userManager, string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user != null)
            {
                if (user.Dogsitters.Count == 0)
                {
                    var dogsitter = new Dogsitter
                    {
                        UserId = user.Id,
                        DateOfBirth = new DateTime(1990, 2, 10),
                        Description = "Харесвам кученцата адски много. Занимавам се от малък с тях и се хваля с това.",
                        Address = "Младост 3, ул. Радичков",
                        Gender = Gender.Male,
                        FirstName = "Георги",
                        MiddleName = "Петров",
                        LastName = "Христов",
                        ImageUrl = "https://res.cloudinary.com/add cloud name here/image/upload/v1586650442/gjrmxzlm7lcrd7b4ntgz.png",
                        PhoneNumber = user.PhoneNumber,
                        WageRate = 10,
                    };

                    user.Dogsitters.Add(dogsitter);
                }
            }
        }
    }
}
