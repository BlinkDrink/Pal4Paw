namespace DogCarePlatform.Data.Models
{
    public class Rating
    {
        public string OwnerId { get; set; }

        public virtual Owner Owner { get; set; }

        public string DogsitterId { get; set; }

        public virtual Dogsitter Dogsitter { get; set; }

        public int Score { get; set; }
    }
}