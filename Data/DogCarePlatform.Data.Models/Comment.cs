namespace DogCarePlatform.Data.Models
{
    using System;

    public class Comment
    {
        public Comment()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public string Content { get; set; }

        public string DogsitterId { get; set; }

        public Dogsitter Dogsitter { get; set; }

        public string OwnerId { get; set; }

        public Owner Owner { get; set; }
    }
}
