namespace DogCarePlatform.Services.Data.Tests
{
    using DogCarePlatform.Data;
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Data.Repositories;
    using DogCarePlatform.Web.ViewModels.Notification;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System;
    using Xunit;

    public class NotificationsServiceTests
    {
        [Fact]
        public void GetAppointmentFromNotificationByIdShouldReturnProperNotification()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("Notifications_Database_GetAppointmentFromNotificationById");
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
                .UseInMemoryDatabase("Notifications_Database_GetAppointmentFromNotificationThrowException");
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
        public void GetNotificationByIdTemplateShouldReturnProperType()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("Notification_Database_GetNotificationByIdTemplate");
            var notificationRepository = new EfDeletableEntityRepository<Notification>(new ApplicationDbContext(options.Options));
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

            var notificationsService = new NotificationsService(notificationRepository, userRepository);

            var notification = new Notification
            {
                OwnerId = Guid.NewGuid().ToString(),
                Dogsitter = new Dogsitter(),
                Content = "Content",
            };

            notificationRepository.AddAsync(notification);
            notificationRepository.SaveChangesAsync();

            var notificationFromDb = notificationsService.GetNotificationById<NotificationViewModel>(notification.Id);

            Assert.IsType<NotificationViewModel>(notificationFromDb);
        }
    }
}
