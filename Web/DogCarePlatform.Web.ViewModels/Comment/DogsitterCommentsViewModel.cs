namespace DogCarePlatform.Web.ViewModels.Comment
{
    using System;

    using DogCarePlatform.Data.Models;

    public class DogsitterCommentsViewModel
    {
        public string Content { get; set; }

        public Dogsitter Dogsitter { get; set; }

        public Owner Owner { get; set; }

        public string SentBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int RatingScore { get; set; }
    }
}
