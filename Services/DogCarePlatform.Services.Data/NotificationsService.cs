namespace DogCarePlatform.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using DogCarePlatform.Data.Common.Repositories;
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Services.Mapping;
    using Microsoft.AspNetCore.Identity;

    public class NotificationsService : INotificationsService
    {
        private readonly IDeletableEntityRepository<Notification> notificationsRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public NotificationsService(IDeletableEntityRepository<Notification> notificationsRepository, IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.notificationsRepository = notificationsRepository;
            this.usersRepository = usersRepository;
        }

        public Notification GetAppointmentFromNotificationById(string id)
        {
            return this.notificationsRepository.All().FirstOrDefault(n => n.Id == id);
        }

        public T GetNotificationById<T>(string id)
        {
            return this.notificationsRepository.All().Where(n => n.Id == id).To<T>().FirstOrDefault();
        }

        public Notification GetNotificationById(string id)
        {
            return this.notificationsRepository.All().FirstOrDefault(n => n.Id == id);
        }

        public async Task RemoveCommentNotification(string id)
        {
            var notification = this.notificationsRepository.All().Where(n => n.Id == id).FirstOrDefault();

            this.notificationsRepository.Delete(notification);
            await this.notificationsRepository.SaveChangesAsync();
        }

        public async Task SendNotification(Notification notification)
        {
            await this.notificationsRepository.AddAsync(notification);
            await this.notificationsRepository.SaveChangesAsync();
        }
    }
}
