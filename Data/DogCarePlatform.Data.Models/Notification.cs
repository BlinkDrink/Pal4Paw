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

        public string Content { get; set; }

        public DateTime Date { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public TimeSpan TimeSpan => this.EndTime - this.StartTime;

        public DateTime WholeTime => this.Date.Date + this.TimeSpan;

        public DateTime ReceivedOn { get; set; }

        public string OwnerId { get; set; }

        public virtual Owner Owner { get; set; }

        public string DogsitterId { get; set; }

        public virtual Dogsitter Dogsitter { get; set; }
    }
}