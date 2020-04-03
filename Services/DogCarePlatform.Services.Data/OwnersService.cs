namespace DogCarePlatform.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AutoMapper;
    using DogCarePlatform.Data.Common.Repositories;
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Web.ViewModels.Owner;

    public class OwnersService : IOwnersService
    {
        private readonly IDeletableEntityRepository<Owner> ownersRepository;

        public OwnersService(IDeletableEntityRepository<ApplicationUser> userRepository, IDeletableEntityRepository<Owner> ownersRepository)
        {
            this.ownersRepository = ownersRepository;
        }

        public async Task AddPersonalInfoAsync(string address, string firstName, string middleName, string lastName, Gender gender, string imageUrl, string phoneNumber, string userId, string dogsDescription)
        {

            var owner = new Owner
            {
                Address = address,
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                Gender = gender,
                ImageUrl = imageUrl,
                PhoneNumber = phoneNumber,
                UserId = userId,
                DogsDescription = dogsDescription,
            };

            await this.ownersRepository.AddAsync(owner);
            await this.ownersRepository.SaveChangesAsync();
        }

        public Owner GetOwnerById(string id)
        {
            return this.ownersRepository.All().Where(o => o.UserId == id).FirstOrDefault();
        }

        public async Task UpdateCurrentLoggedInUserInfoAsync(string id, string firstName, string middleName, string lastName, string address, string description, string imageUrl)
        {
            var owner = this.ownersRepository.All().Where(o => o.UserId == id).FirstOrDefault();

            owner.FirstName = firstName;
            owner.MiddleName = middleName;
            owner.LastName = lastName;
            owner.Address = address;
            owner.DogsDescription = description;
            owner.ImageUrl = imageUrl;

            await this.ownersRepository.SaveChangesAsync();
        }
    }
}
