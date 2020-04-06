namespace DogCarePlatform.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using DogCarePlatform.Data.Common.Models;

    public class Comment : BaseDeletableModel<int>
    { 
        public string Content { get; set; }

        [Required]
        public string DogsitterId { get; set; }

        public virtual Dogsitter Dogsitter { get; set; }

        [Required]
        public string OwnerId { get; set; }

        public virtual Owner Owner { get; set; }
    }
}
