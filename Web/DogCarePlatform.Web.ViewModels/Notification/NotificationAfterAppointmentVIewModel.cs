namespace DogCarePlatform.Web.ViewModels.Notification
{
    using System;

    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Services.Mapping;

    public class NotificationAfterAppointmentVIewModel : IMapFrom<Notification>
    {
        public string Content { get; set; }

        public string DogsitterId { get; set; }

        public string OwnerId { get; set; }

        public DateTime Date { get; set; }

        public DateTime ReceivedOn { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
