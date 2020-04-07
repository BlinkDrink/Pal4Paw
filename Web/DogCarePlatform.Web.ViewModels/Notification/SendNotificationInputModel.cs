namespace DogCarePlatform.Web.ViewModels.Notification
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using DogCarePlatform.Data.Models;

    public class SendNotificationInputModel
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public TimeSpan TimeSpan => this.EndTime - this.StartTime;

        public DateTime WholeTime => this.Date.Date + this.TimeSpan;

        public DateTime ReceivedOn => DateTime.UtcNow;

    }
}
