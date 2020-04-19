using System.Reflection;
using DogCarePlatform.Services.Mapping;
using DogCarePlatform.Web.ViewModels;

namespace DogCarePlatform.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using AutoMapper;
    using DogCarePlatform.Data;
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Data.Repositories;
    using DogCarePlatform.Web.ViewModels.Administration.Dashboard;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class AdministratorServiceTests
    {
        [Fact]
        public async void AddDogsitterAsyncShouldBindDogsitterToUser()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));
            var questionRepository = new EfDeletableEntityRepository<QuestionAnswer>(new ApplicationDbContext(options.Options));

            var administartorService = new AdministartorService(userRepository, questionRepository);

            var user = new ApplicationUser();
            await userRepository.AddAsync(user);
            await userRepository.SaveChangesAsync();

            await administartorService.AddDogsitterAsync(user.Id);

            Assert.IsType<Dogsitter>(user.Dogsitter);
        }

        [Fact]
        public async void ApplicantDetailsByIdShouldReturnProperType()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));
            var questionRepository = new EfDeletableEntityRepository<QuestionAnswer>(new ApplicationDbContext(options.Options));

            var administartorService = new AdministartorService(userRepository, questionRepository);
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var questionAnswer = new List<QuestionAnswer> { new QuestionAnswer(), };
            var user = new ApplicationUser
            {
                Email = "user@user.com",
                PhoneNumber = "0882312311",
                QuestionsAnswers = questionAnswer,
            };

            await userRepository.AddAsync(user);
            await userRepository.SaveChangesAsync();

            var userFromService = administartorService.ApplicantDetailsById<ApplicantViewModel>(user.Id);

            Assert.IsType<ApplicantViewModel>(userFromService);
        }

        [Fact]
        public async void RemoveQuestionsAnswersFromUserAsyncShouldRemoveDataAccordingly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));
            var questionRepository = new EfDeletableEntityRepository<QuestionAnswer>(new ApplicationDbContext(options.Options));

            var administartorService = new AdministartorService(userRepository, questionRepository);


            var user = new ApplicationUser();

            var questionAnswer = new List<QuestionAnswer>
            {
                new QuestionAnswer { UserId = user.Id },
                new QuestionAnswer { UserId = user.Id },
                new QuestionAnswer { UserId = user.Id },
                new QuestionAnswer { UserId = user.Id },
            };

            foreach (var qa in questionAnswer)
            {
                await questionRepository.AddAsync(qa);
            }

            await questionRepository.SaveChangesAsync();

            await administartorService.RemoveQuestionsAnswersFromUserAsync(user.Id);

            Assert.Empty(questionRepository.AllAsNoTracking());
        }
    }
}
