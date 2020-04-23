namespace DogCarePlatform.Web.ViewModels.Notification
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using DogCarePlatform.Web.Utilities;
    using DogCarePlatform.Web.ViewModels.Utilities;

    public class SendRequestInputModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage ="Моля въведете дата")]
        [DateValidation(ErrorMessage = "Датата трябва да е минимум от днес до след 1 година")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage ="Моля въведете начален час")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "Моля въведете краен час")]
        [EndTimeValidation("StartTime", ErrorMessage = "Крайното време трябва да бъде след началното време")]
        public DateTime EndTime { get; set; }

        public DateTime ReceivedOn => DateTime.UtcNow;

    }
}
