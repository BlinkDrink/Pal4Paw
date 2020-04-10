namespace DogCarePlatform.Services.Data
{
    using DogCarePlatform.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface INotificationsService
    {
        Notification GetNotificationById(string id);
    }
}
