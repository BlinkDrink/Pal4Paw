using System.Reflection;
using DogCarePlatform.Services.Mapping;
using DogCarePlatform.Web.ViewModels;
using DogCarePlatform.Web.ViewModels.Owner;

namespace DogCarePlatform.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using DogCarePlatform.Data;
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Data.Repositories;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class OwnersServiceTests
    {
        private const string Info = "Information";

        [Fact]
        public void OwnersRepositoryShouldBeEmptyUponInitialization()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));
            var dogsitterRepository = new EfDeletableEntityRepository<Dogsitter>(new ApplicationDbContext(options.Options));
            var ownerRepository = new EfDeletableEntityRepository<Owner>(new ApplicationDbContext(options.Options));

            var ownersService = new OwnersService(userRepository, ownerRepository, dogsitterRepository);

            Assert.Empty(ownerRepository.All());
        }

        [Fact]
        public async void CreateOwnerAsyncShouldCreateTheOwner()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));
            var dogsitterRepository = new EfDeletableEntityRepository<Dogsitter>(new ApplicationDbContext(options.Options));
            var ownerRepository = new EfDeletableEntityRepository<Owner>(new ApplicationDbContext(options.Options));

            var ownersService = new OwnersService(userRepository, ownerRepository, dogsitterRepository);

            var user = new ApplicationUser();

            await ownersService.CreateOwnerAsync(user, Info, Info, Info, Info, Gender.Male, Info, Info, user.Id, Info);

            Assert.Single(ownerRepository.All());
        }

        [Fact]
        public async void CreateOwnerAsyncShouldAddProperInformationToTheEntity()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));
            var dogsitterRepository = new EfDeletableEntityRepository<Dogsitter>(new ApplicationDbContext(options.Options));
            var ownerRepository = new EfDeletableEntityRepository<Owner>(new ApplicationDbContext(options.Options));

            var ownersService = new OwnersService(userRepository, ownerRepository, dogsitterRepository);

            var user = new ApplicationUser();

            await ownersService.CreateOwnerAsync(user, Info, Info, Info, Info, Gender.Male, Info, Info, user.Id, Info);

            var owner = ownerRepository.All().FirstOrDefault();

            Assert.Equal(Info, owner.FirstName);
        }

        [Fact]
        public async void GetDogsittersAsyncShouldReturnAllDogsitters()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));
            var dogsitterRepository = new EfDeletableEntityRepository<Dogsitter>(new ApplicationDbContext(options.Options));
            var ownerRepository = new EfDeletableEntityRepository<Owner>(new ApplicationDbContext(options.Options));

            var ownersService = new OwnersService(userRepository, ownerRepository, dogsitterRepository);

            var user = new ApplicationUser();
            var user2 = new ApplicationUser();

            var role = new ApplicationRole { Name = "Dogsitter", NormalizedName = "DOGSITTER", };

            user.Roles.Add(new IdentityUserRole<string> { UserId = user.Id, RoleId = role.Id });
            user2.Roles.Add(new IdentityUserRole<string> { UserId = user.Id, RoleId = role.Id });

            var dogsitters = ownersService.GetDogsittersAsync(new List<ApplicationUser> { user, user2 });

            Assert.Equal(2, dogsitters.Count);
        }

        [Fact]
        public async void GetOwnerByIdShouldReturnCorrectOwner()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));
            var dogsitterRepository = new EfDeletableEntityRepository<Dogsitter>(new ApplicationDbContext(options.Options));
            var ownerRepository = new EfDeletableEntityRepository<Owner>(new ApplicationDbContext(options.Options));

            var ownersService = new OwnersService(userRepository, ownerRepository, dogsitterRepository);

            var user = new ApplicationUser();
            var user2 = new ApplicationUser();

            await ownersService.CreateOwnerAsync(user, Info, Info, Info, Info, Gender.Male, Info, Info, user.Id, Info);
            await ownersService.CreateOwnerAsync(user2, Info, Info, Info, Info, Gender.Male, Info, Info, user2.Id, Info);

            var owner = ownersService.GetOwnerById(user.Id);

            Assert.Equal(user.Id, owner.UserId);
        }

        [Fact]
        public async void GetOwnerByIdShouldThrowNullReferenceExceptionWhenWrongId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));
            var dogsitterRepository = new EfDeletableEntityRepository<Dogsitter>(new ApplicationDbContext(options.Options));
            var ownerRepository = new EfDeletableEntityRepository<Owner>(new ApplicationDbContext(options.Options));

            var ownersService = new OwnersService(userRepository, ownerRepository, dogsitterRepository);

            var user = new ApplicationUser();

            await ownersService.CreateOwnerAsync(user, Info, Info, Info, Info, Gender.Male, Info, Info, user.Id, Info);

            var owner = ownersService.GetOwnerById("");

            Assert.Throws<NullReferenceException>(() => owner.Id);
        }

        [Fact]
        public async void GetOwnerApplicationUserShouldReturnCorrectUser()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));
            var dogsitterRepository = new EfDeletableEntityRepository<Dogsitter>(new ApplicationDbContext(options.Options));
            var ownerRepository = new EfDeletableEntityRepository<Owner>(new ApplicationDbContext(options.Options));

            var ownersService = new OwnersService(userRepository, ownerRepository, dogsitterRepository);

            var user = new ApplicationUser();
            var owner = new Owner
            {
                User = user,
                UserId = user.Id,
            };

            user.Owner = owner;
            await userRepository.AddAsync(user);
            await userRepository.SaveChangesAsync();
            var appUser = ownersService.GetOwnerApplicationUser(owner.Id);

            Assert.Equal(user.Id, appUser.Id);
        }

        [Fact]
        public async void UpdateCurrentLoggedInUserInfoAsyncShouldUpdateValuesAccordingly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));
            var dogsitterRepository = new EfDeletableEntityRepository<Dogsitter>(new ApplicationDbContext(options.Options));
            var ownerRepository = new EfDeletableEntityRepository<Owner>(new ApplicationDbContext(options.Options));

            var ownersService = new OwnersService(userRepository, ownerRepository, dogsitterRepository);

            var user = new ApplicationUser();

            await ownersService.CreateOwnerAsync(user, Info, Info, Info, Info, Gender.Male, Info, Info, user.Id, Info);

            await ownersService.UpdateCurrentLoggedInUserInfoAsync(user.Id, "NewName", Info, Info, Info, Info, Info);

            var owner = ownersService.GetOwnerById(user.Id);

            Assert.Equal("NewName", owner.FirstName);
        }

        [Fact]
        public async void SendNotificationShouldSaveNotificationAccordingly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));
            var dogsitterRepository = new EfDeletableEntityRepository<Dogsitter>(new ApplicationDbContext(options.Options));
            var ownerRepository = new EfDeletableEntityRepository<Owner>(new ApplicationDbContext(options.Options));

            var ownersService = new OwnersService(userRepository, ownerRepository, dogsitterRepository);

            var dogsitter = new Dogsitter();
            var owner = new Owner();

            await dogsitterRepository.AddAsync(dogsitter);
            await dogsitterRepository.SaveChangesAsync();

            await ownersService.SendNotification(dogsitter.Id, owner, DateTime.UtcNow, DateTime.UtcNow, DateTime.UtcNow);

            Assert.Equal(1, dogsitter.Notifications.Count);
        }

        [Fact]
        public async void DogsitterDetailsByIdTemplateShouldReturnProperType()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));
            var dogsitterRepository = new EfDeletableEntityRepository<Dogsitter>(new ApplicationDbContext(options.Options));
            var ownerRepository = new EfDeletableEntityRepository<Owner>(new ApplicationDbContext(options.Options));

            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var ownersService = new OwnersService(userRepository, ownerRepository, dogsitterRepository);

            var dogsitter = new Dogsitter();

            await dogsitterRepository.AddAsync(dogsitter);
            await dogsitterRepository.SaveChangesAsync();

            var dogsitterDb = ownersService.DogsitterDetailsById<DogsitterInfoViewModel>(dogsitter.Id);

            Assert.IsType<DogsitterInfoViewModel>(dogsitterDb);
        }

        [Fact]
        public async void SendNotificationShouldHaveCorrectOwnerAndDogsitter()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));
            var dogsitterRepository = new EfDeletableEntityRepository<Dogsitter>(new ApplicationDbContext(options.Options));
            var ownerRepository = new EfDeletableEntityRepository<Owner>(new ApplicationDbContext(options.Options));

            var ownersService = new OwnersService(userRepository, ownerRepository, dogsitterRepository);

            var dogsitter = new Dogsitter();
            var owner = new Owner();

            await dogsitterRepository.AddAsync(dogsitter);
            await dogsitterRepository.SaveChangesAsync();

            await ownersService.SendNotification(dogsitter.Id, owner, DateTime.UtcNow, DateTime.UtcNow, DateTime.UtcNow);

            var trueOwnerDogsitter = dogsitter.Notifications.FirstOrDefault().OwnerId == owner.Id;

            Assert.True(trueOwnerDogsitter);
        }
    }
}
