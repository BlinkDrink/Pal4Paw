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

        Task SendNotification(Notification notification);

        Task RemoveCommentNotification(string id);

        Notification GetAppointmentFromNotificationById(string id);
    }
}
