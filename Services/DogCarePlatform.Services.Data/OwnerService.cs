namespace DogCarePlatform.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using DogCarePlatform.Data.Common.Repositories;
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Web.ViewModels.Owner;

    public class OwnerService : IOwnerService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IDeletableEntityRepository<Owner> ownersRepository;


        public OwnerService(IDeletableEntityRepository<ApplicationUser> userRepository, IDeletableEntityRepository<Owner> ownersRepository)
        {
            this.usersRepository = userRepository;
            this.ownersRepository = ownersRepository;
        }

        public async Task AddPersonalInfoAsync(string address, string firstName, string middleName, string lastName, Gender gender, string imageUrl, string phoneNumber, ICollection<Dog> dogs, string userId)
        {
            var owner = new Owner
            {
                Address = address,
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                Gender = gender,
                ImageUrl = imageUrl,
                Dogs = dogs,
                UserId = userId,
            };

            await this.ownersRepository.AddAsync(owner);
        }
    }
}
