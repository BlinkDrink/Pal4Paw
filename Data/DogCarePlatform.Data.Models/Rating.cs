namespace DogCarePlatform.Data.Models
{
    using DogCarePlatform.Data.Common.Models;

    public class Rating : BaseDeletableModel<int>
    {
        public string OwnerId { get; set; }

        public virtual Owner Owner { get; set; }

        public string DogsitterId { get; set; }

        public virtual Dogsitter Dogsitter { get; set; }

        public int Score { get; set; }

        public string SentBy { get; set; }
    }
}