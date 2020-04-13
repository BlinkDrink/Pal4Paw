namespace DogCarePlatform.Web.ViewModels.Comment
{
    using System.ComponentModel.DataAnnotations;

    public class RateAndCommentInputModel
    {
        [Required]
        [StringLength(250, MinimumLength = 50, ErrorMessage = "Полето трябва да съдържа поне 50 символа.")]
        [Display(Name = "Напишете коментар")]
        public string Content { get; set; }

        public int Score { get; set; }

        public string OwnerId { get; set; }

        public string DogsitterId { get; set; }

        public string SentBy { get; set; }
    }
}
