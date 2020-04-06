using DogCarePlatform.Data.Common.Models;
using System;

namespace DogCarePlatform.Data.Models
{
    public class Notification : BaseDeletableModel<string>
    {
        public Notification()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public DateTime ReceivedOn { get; set; }

        public string OwnerId { get; set; }

        public Owner Owner { get; set; }

        public string DogsitterId { get; set; }

        public Dogsitter Dogsitter { get; set; }
    }
}