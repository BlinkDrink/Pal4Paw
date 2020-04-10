namespace DogCarePlatform.Services.Data
{
    using DogCarePlatform.Data.Common.Repositories;
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Services.Mapping;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class NotificationsService : INotificationsService
    {
        private readonly IDeletableEntityRepository<Notification> notificationsRepository;

        public NotificationsService(IDeletableEntityRepository<Notification> notificationsRepository)
        {
            this.notificationsRepository = notificationsRepository;
        }

        public T GetNotificationById<T>(string id)
        {
            return this.notificationsRepository.All().Where(n => n.Id == id).To<T>().FirstOrDefault();
        }

        public async Task RemoveReviewedNotification(Notification notification)
        {
            this.notificationsRepository.Delete(notification);
            await this.notificationsRepository.SaveChangesAsync();
        }
    }
}
