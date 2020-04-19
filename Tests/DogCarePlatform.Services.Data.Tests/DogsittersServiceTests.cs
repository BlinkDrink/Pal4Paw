namespace DogCarePlatform.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using DogCarePlatform.Data;
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Data.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class DogsittersServiceTests
    {
        private const string FillInfo = "Информация";

        [Fact]
        public async void CurrentUserAddInfoShouldAddDetailsAccordingly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var dogsitterRepository = new EfDeletableEntityRepository<Dogsitter>(new ApplicationDbContext(options.Options));
            var dogsittersService = new DogsittersService(dogsitterRepository);

            var dogsitterBase = new Dogsitter
            {
                UserId = Guid.NewGuid().ToString(),
            };

            await dogsitterRepository.AddAsync(dogsitterBase);
            await dogsitterRepository.SaveChangesAsync();

            await dogsittersService.CurrentUserAddInfo(dogsitterBase.UserId, FillInfo, FillInfo, FillInfo, FillInfo, FillInfo, FillInfo, 5);

            var dogsitter = await dogsitterRepository.All().FirstOrDefaultAsync();

            Assert.Equal(FillInfo, dogsitter.FirstName);
        }

        [Fact]
        public async void GetDogsitterByUserIdShouldReturnProperValue()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var dogsitterRepository = new EfDeletableEntityRepository<Dogsitter>(new ApplicationDbContext(options.Options));
            var dogsittersService = new DogsittersService(dogsitterRepository);

            var dogsitterBase = new Dogsitter
            {
                UserId = Guid.NewGuid().ToString(),
            };

            await dogsitterRepository.AddAsync(dogsitterBase);
            await dogsitterRepository.SaveChangesAsync();

            await dogsittersService.CurrentUserAddInfo(dogsitterBase.UserId, FillInfo, FillInfo, FillInfo, FillInfo, FillInfo, FillInfo, 5);

            var dogsitter = dogsittersService.GetDogsitterByUserId(dogsitterBase.UserId);

            Assert.Equal(dogsitterBase.Id, dogsitter.Id);
        }

        [Fact]
        public async void GetDogsitterByUserIdShouldThrowNullReferenceExceptionWhenWrongId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var dogsitterRepository = new EfDeletableEntityRepository<Dogsitter>(new ApplicationDbContext(options.Options));
            var dogsittersService = new DogsittersService(dogsitterRepository);

            var dogsitterBase = new Dogsitter
            {
                UserId = Guid.NewGuid().ToString(),
            };

            await dogsitterRepository.AddAsync(dogsitterBase);
            await dogsitterRepository.SaveChangesAsync();

            await dogsittersService.CurrentUserAddInfo(dogsitterBase.UserId, FillInfo, FillInfo, FillInfo, FillInfo, FillInfo, FillInfo, 5);

            var dogsitter = dogsittersService.GetDogsitterByUserId("");

            Assert.Throws<NullReferenceException>(() => dogsitter.Id);
        }

        [Fact]
        public async void GetDogsitterByDogsitterIdShouldThrowNullReferenceExceptionWhenWrongId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var dogsitterRepository = new EfDeletableEntityRepository<Dogsitter>(new ApplicationDbContext(options.Options));
            var dogsittersService = new DogsittersService(dogsitterRepository);

            var dogsitterBase = new Dogsitter
            {
                UserId = Guid.NewGuid().ToString(),
            };

            await dogsitterRepository.AddAsync(dogsitterBase);
            await dogsitterRepository.SaveChangesAsync();

            await dogsittersService.CurrentUserAddInfo(dogsitterBase.UserId, FillInfo, FillInfo, FillInfo, FillInfo, FillInfo, FillInfo, 5);

            var dogsitter = dogsittersService.GetDogsitterByDogsitterId("");

            Assert.Throws<NullReferenceException>(() => dogsitter.Id);
        }

        [Fact]
        public async void GetDogsitterByDogsitterIdShouldReturnProperValue()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var dogsitterRepository = new EfDeletableEntityRepository<Dogsitter>(new ApplicationDbContext(options.Options));
            var dogsittersService = new DogsittersService(dogsitterRepository);

            var dogsitterBase = new Dogsitter
            {
                UserId = Guid.NewGuid().ToString(),
            };

            await dogsitterRepository.AddAsync(dogsitterBase);
            await dogsitterRepository.SaveChangesAsync();

            await dogsittersService.CurrentUserAddInfo(dogsitterBase.UserId, FillInfo, FillInfo, FillInfo, FillInfo, FillInfo, FillInfo, 5);

            var dogsitter = dogsittersService.GetDogsitterByDogsitterId(dogsitterBase.Id);

            Assert.Equal(dogsitterBase.UserId, dogsitter.UserId);
        }
    }
}
