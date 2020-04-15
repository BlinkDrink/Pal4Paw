namespace DogCarePlatform.Web.ViewModels.Comment
{
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Services.Mapping;
    using System;

    public class OwnerCommentsViewModel
    {
        public string Content { get; set; }

        public Dogsitter Dogsitter { get; set; }

        public Owner Owner { get; set; }

        public string SentBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int RatingScore { get; set; }
    }
}
