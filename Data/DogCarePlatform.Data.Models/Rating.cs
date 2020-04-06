namespace DogCarePlatform.Data.Models
{
    using DogCarePlatform.Data.Common.Models;
    using System.ComponentModel.DataAnnotations;

    public class Rating : BaseModel<int>
    {
        public string OwnerId { get; set; }

        public virtual Owner Owner { get; set; }

        public string DogsitterId { get; set; }

        public virtual Dogsitter Dogsitter { get; set; }

        public int Score { get; set; }
    }
}