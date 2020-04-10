namespace DogCarePlatform.Services.Data
{
    using DogCarePlatform.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface INotificationsService
    {
        T GetNotificationById<T>(string id);

        Notification GetNotificationById(string id);

        Task RemoveReviewedNotification(Notification notification);
    }
}
