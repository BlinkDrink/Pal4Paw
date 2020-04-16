namespace DogCarePlatform.Web.ViewModels.Notification
{
    using System.ComponentModel.DataAnnotations;

    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Services.Mapping;

    public class NotificationAfterAppointmentViewModel : IMapFrom<Notification>
    {
        [Required]
        [StringLength(250, MinimumLength = 50, ErrorMessage = "Полето трябва да съдържа поне 50 символа.")]
        [Display(Name = "Напишете коментар")]
        public string Content { get; set; }

        public string DogsitterId { get; set; }

        public string OwnerId { get; set; }

        public string SentBy { get; set; }

        public int Score { get; set; }
    }
}
