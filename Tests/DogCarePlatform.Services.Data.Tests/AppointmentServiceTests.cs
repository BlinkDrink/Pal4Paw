namespace DogCarePlatform.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DogCarePlatform.Data.Common.Repositories;
    using DogCarePlatform.Data.Models;
    using Moq;
    using Xunit;

    public class AppointmentServiceTests
    {
        public List<Appointment> GetAppointments()
        {
            var user = new ApplicationUser
            {
                UserName = "user@user.bg",
                PhoneNumber = "0882321232",
                Email = "user@user.bg",
                PasswordHash = "AQAAAAEAACcQAAAAEAAStsPPGUaY7E/QduXwLIu85to991hKIV66qLKQDJZHu9yCPiiCplmkGpO0lNWvjw==",
                NormalizedUserName = "USER@USER.BG",
            };

            var owner = new Owner
            {
                FirstName = "Гошко",
                MiddleName = "Гергинов",
                LastName = "Маринов",
                UserId = user.Id,
                Address = "бул. Пенчо",
                DogsDescription = "Три сладки коргита.",
                Gender = Gender.Male,
                ImageUrl = "https://media.mnn.com/assets/images/2019/05/koala.jpg.653x0_q80_crop-smart.jpg",
                PhoneNumber = user.PhoneNumber,
            };

            var dogsitter = new Dogsitter
            {
                FirstName = "Иван",
                MiddleName = "Граматиков",
                LastName = "Иванов",
                UserId = user.Id,
                Address = "бул. Гео Милев",
                Description = "Добре се справям с кучета.",
                Gender = Gender.Male,
                DateOfBirth = new DateTime(1990, 1, 1),
                WageRate = 10,
                ImageUrl = "https://media.mnn.com/assets/images/2019/05/koala.jpg.653x0_q80_crop-smart.jpg",
                PhoneNumber = user.PhoneNumber,
            };

            user.Owners.Add(owner);
            user.Dogsitters.Add(dogsitter);

            return new List<Appointment>
            {
                new Appointment
                {
                    Status = AppointmentStatus.Unprocessed,
                    Date = DateTime.UtcNow,
                    StartTime = DateTime.UtcNow.AddHours(1),
                    EndTime = DateTime.UtcNow.AddHours(2),
                    OwnerId = owner.Id,
                    DogsitterId = dogsitter.Id,
                },
                new Appointment
                {
                    Status = AppointmentStatus.Happening,
                    Date = DateTime.UtcNow,
                    StartTime = DateTime.UtcNow.AddHours(1),
                    EndTime = DateTime.UtcNow.AddHours(2),
                    OwnerId = owner.Id,
                    DogsitterId = dogsitter.Id,
                },
            };
        }

        [Fact]
        public void GetCountShouldReturnCorrectNumber()
        {
            var repository = new Mock<IDeletableEntityRepository<Setting>>();
            repository.Setup(r => r.All()).Returns(new List<Setting>
                                                        {
                                                            new Setting(),
                                                            new Setting(),
                                                            new Setting(),
                                                        }.AsQueryable());

            var service = new SettingsService(repository.Object);
            Assert.Equal(3, service.GetCount());
            repository.Verify(x => x.All(), Times.Once);
        }
    }
}
