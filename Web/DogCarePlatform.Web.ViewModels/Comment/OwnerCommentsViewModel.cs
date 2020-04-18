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

        public int Total { get; set; }

        public int FiveStars { get; set; }

        public int FourStars { get; set; }

        public int ThreeStars { get; set; }

        public int TwoStars { get; set; }

        public int OneStar { get; set; }

        public decimal FiveStarPercentage { get; set; }

        public decimal FourStarPercentage { get; set; }

        public decimal ThreeStarPercentage { get; set; }

        public decimal TwoStarPercentage { get; set; }

        public decimal OneStarPercentage { get; set; }

    }
}
