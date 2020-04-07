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
    using DogCarePlatform.Services.Mapping;
    using DogCarePlatform.Web.ViewModels.Owner;

    public class OwnersService : IOwnersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IDeletableEntityRepository<Owner> ownersRepository;
        private readonly IDeletableEntityRepository<Dogsitter> dogsittersRepository;

        public OwnersService(IDeletableEntityRepository<ApplicationUser> userRepository, IDeletableEntityRepository<Owner> ownersRepository, IDeletableEntityRepository<Dogsitter> dogsittersRepository)
        {
            this.userRepository = userRepository;
            this.ownersRepository = ownersRepository;
            this.dogsittersRepository = dogsittersRepository;
        }

        public async Task CreateOwnerAsync(ApplicationUser user, string address, string firstName, string middleName, string lastName, Gender gender, string imageUrl, string phoneNumber, string userId, string dogsDescription)
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

            user.Owners.Add(owner);
            await this.ownersRepository.AddAsync(owner);
            await this.ownersRepository.SaveChangesAsync();
        }

        public T DogsitterDetailsById<T>(string id)
        {
            var dogsitter = this.dogsittersRepository.All().Where(d => d.Id == id).To<T>().FirstOrDefault();

            return dogsitter;
        }

        public ICollection<Dogsitter> GetDogsittersAsync(ICollection<ApplicationUser> applicationUsers)
        {
            var dogsitters = new List<Dogsitter>();

            foreach (var user in applicationUsers)
            {
                var dogsitter = this.dogsittersRepository.All().FirstOrDefault(d => d.UserId == user.Id);
                dogsitters.Add(dogsitter);
            }

            return dogsitters.ToList();
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

        public async Task SendNotification(string dogsitterId, Owner owner, DateTime date, DateTime startTime, DateTime endTime)
        {
            var dogsitter = this.dogsittersRepository.All().FirstOrDefault(d => d.Id == dogsitterId);     

            var notification = new Notification
            {
                DogsitterId = dogsitterId,
                OwnerId = owner.Id,
                Content = $"Получихте заявка от  <p class=\"orange-text\">{owner.FirstName}</p>",
                ReceivedOn = DateTime.UtcNow,
                Date = date,
                StartTime = startTime,
                EndTime = endTime,
            };

            dogsitter.Notifications.Add(notification);
            await dogsittersRepository.SaveChangesAsync();
        }
    }
}
