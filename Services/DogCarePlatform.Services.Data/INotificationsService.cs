namespace DogCarePlatform.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using DogCarePlatform.Data.Models;

    public interface INotificationsService
    {
        T GetNotificationById<T>(string id);

        Notification GetNotificationById(string id);

        Task RemoveReviewedNotification(Notification notification);

        Task SendNotification(Notification notification);
    }
}
