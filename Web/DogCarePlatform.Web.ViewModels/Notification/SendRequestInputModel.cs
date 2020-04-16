namespace DogCarePlatform.Web.ViewModels.Notification
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using DogCarePlatform.Web.Utilities;

    public class SendRequestInputModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage ="Моля въведете дата")]
        [DateValidation]
        public DateTime Date { get; set; }

        public DateTime StartTime { get; set; }

        [Compare(StartTime)]
        public DateTime EndTime { get; set; }

        public TimeSpan TimeSpan => this.EndTime - this.StartTime;

        public DateTime WholeTime => this.Date.Date + this.TimeSpan;

        public DateTime ReceivedOn => DateTime.UtcNow;

    }
}
