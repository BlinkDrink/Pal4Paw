namespace DogCarePlatform.Services.Data.Tests
{
    using System;
    using System.Reflection;

    using DogCarePlatform.Data;
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Data.Repositories;
    using DogCarePlatform.Services.Mapping;
    using DogCarePlatform.Web.ViewModels;
    using DogCarePlatform.Web.ViewModels.Notification;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class NotificationsServiceTests
    {
        [Fact]
        public void NotificationRepositoryShouldHaveZeroItemsUponInitialization()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var notificationsRepository = new EfDeletableEntityRepository<Notification>(new ApplicationDbContext(options.Options));

            Assert.Empty(notificationsRepository.All());
        }

        [Fact]
        public void GetAppointmentFromNotificationByIdShouldReturnProperNotification()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var notificationRepository = new EfDeletableEntityRepository<Notification>(new ApplicationDbContext(options.Options));
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

            var notificationsService = new NotificationsService(notificationRepository, userRepository);

            var notification = new Notification
            {
                OwnerId = Guid.NewGuid().ToString(),
            };

            notificationRepository.AddAsync(notification);
            notificationRepository.SaveChangesAsync();

            var notificationFromDb = notificationsService.GetAppointmentFromNotificationById(notification.Id);

            Assert.Equal(notification.OwnerId, notificationFromDb.OwnerId);
        }

        [Fact]
        public void GetAppointmentFromNotificationByIdShouldThrowException()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var notificationRepository = new EfDeletableEntityRepository<Notification>(new ApplicationDbContext(options.Options));
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

            var notificationsService = new NotificationsService(notificationRepository, userRepository);

            var notification = new Notification
            {
                OwnerId = Guid.NewGuid().ToString(),
            };

            notificationRepository.AddAsync(notification);
            notificationRepository.SaveChangesAsync();

            var notificationFromDb = notificationsService.GetAppointmentFromNotificationById(Guid.NewGuid().ToString());

            Assert.Throws<NullReferenceException>(() => notificationFromDb.OwnerId);
        }

        [Fact]
        public void GetNotificationByIdShouldReturnCorrectNotification()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var notificationRepository = new EfDeletableEntityRepository<Notification>(new ApplicationDbContext(options.Options));
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

            var notificationsService = new NotificationsService(notificationRepository, userRepository);

            var notification = new Notification
            {
                OwnerId = Guid.NewGuid().ToString(),
            };

            notificationRepository.AddAsync(notification);
            notificationRepository.SaveChangesAsync();

            var notificationFromDb = notificationsService.GetNotificationById(notification.Id);

            Assert.Equal(notification, notificationFromDb);
        }

        [Fact]
        public void GetNotificationByIdShouldThrowException()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var notificationRepository = new EfDeletableEntityRepository<Notification>(new ApplicationDbContext(options.Options));
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

            var notificationsService = new NotificationsService(notificationRepository, userRepository);

            var notification = new Notification
            {
                OwnerId = Guid.NewGuid().ToString(),
            };

            notificationRepository.AddAsync(notification);
            notificationRepository.SaveChangesAsync();

            var notificationFromDb = notificationsService.GetNotificationById(Guid.NewGuid().ToString());

            Assert.Throws<NullReferenceException>(() => notificationFromDb.Id);
        }

        [Fact]
        public async void RemoveCommentNotificationShouldDeleteTheNotification()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var notificationRepository = new EfDeletableEntityRepository<Notification>(new ApplicationDbContext(options.Options));
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

            var notificationsService = new NotificationsService(notificationRepository, userRepository);

            var notification = new Notification();

            await notificationRepository.AddAsync(notification);
            await notificationRepository.SaveChangesAsync();

            await notificationsService.RemoveCommentNotification(notification.Id);

            Assert.Empty(notificationRepository.AllAsNoTracking());
        }

        [Fact]
        public async void RemoveCommentNotificationShouldReturnProperCount()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var notificationRepository = new EfDeletableEntityRepository<Notification>(new ApplicationDbContext(options.Options));
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

            var notificationsService = new NotificationsService(notificationRepository, userRepository);

            var notification = new Notification();
            var notification2 = new Notification();

            await notificationRepository.AddAsync(notification);
            await notificationRepository.AddAsync(notification2);
            await notificationRepository.SaveChangesAsync();

            await notificationsService.RemoveCommentNotification(notification.Id);

            Assert.Single(notificationRepository.AllAsNoTracking());
        }

        [Fact]
        public async void RemoveCommentNotificationShouldDeleteTheCorrectNotification()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var notificationRepository = new EfDeletableEntityRepository<Notification>(new ApplicationDbContext(options.Options));
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

            var notificationsService = new NotificationsService(notificationRepository, userRepository);

            var notification = new Notification();
            var notification2 = new Notification
            {
                Content = "True",
            };

            await notificationRepository.AddAsync(notification);
            await notificationRepository.AddAsync(notification2);
            await notificationRepository.SaveChangesAsync();

            await notificationsService.RemoveCommentNotification(notification.Id);
            var notifFromDb = await notificationRepository.AllAsNoTracking().FirstAsync();

            Assert.Equal(notification2.Content, notifFromDb.Content);
        }

        [Fact]
        public async void SendNotificationShouldSaveTheNotificationProperly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var notificationRepository = new EfDeletableEntityRepository<Notification>(new ApplicationDbContext(options.Options));
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

            var notificationsService = new NotificationsService(notificationRepository, userRepository);

            var notification = new Notification
            {
                DogsitterId = Guid.NewGuid().ToString(),
                OwnerId = Guid.NewGuid().ToString(),
                Content = "True",
            };

            await notificationRepository.SaveChangesAsync();

            await notificationsService.SendNotification(notification);

            Assert.Single(notificationRepository.All());
        }

        [Fact]
        public async void GetNotificationTemplateShouldRetunrProperType()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var notificationRepository = new EfDeletableEntityRepository<Notification>(new ApplicationDbContext(options.Options));
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var notificationsService = new NotificationsService(notificationRepository, userRepository);

            var notification = new Notification
            {
                DogsitterId = Guid.NewGuid().ToString(),
                OwnerId = Guid.NewGuid().ToString(),
                Content = "True",
            };

            await notificationsService.SendNotification(notification);

            var notificationBase = notificationsService.GetNotificationById<NotificationViewModel>(notification.Id);

            Assert.IsType<NotificationViewModel>(notificationBase);
        }

        [Fact]
        public async void SendNotificationShouldHaveTheSameValuesAsTheSentOne()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var notificationRepository = new EfDeletableEntityRepository<Notification>(new ApplicationDbContext(options.Options));
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

            var notificationsService = new NotificationsService(notificationRepository, userRepository);

            var notification = new Notification
            {
                DogsitterId = Guid.NewGuid().ToString(),
                OwnerId = Guid.NewGuid().ToString(),
                Content = "True",
            };

            await notificationRepository.SaveChangesAsync();

            await notificationsService.SendNotification(notification);
            var notifFromDb = await notificationRepository.All().FirstAsync();

            Assert.Equal(notification.Content, notifFromDb.Content);
        }
    }
}
