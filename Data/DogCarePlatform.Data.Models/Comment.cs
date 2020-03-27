namespace DogCarePlatform.Data.Models
{
    using System;

    using DogCarePlatform.Data.Common.Models;

    public class Comment : BaseDeletableModel<int>
    { 
        public string Content { get; set; }

        public string DogsitterId { get; set; }

        public virtual Dogsitter Dogsitter { get; set; }

        public string OwnerId { get; set; }

        public virtual Owner Owner { get; set; }
    }
}
