namespace DogCarePlatform.Data.Models
{
    using System;
    using System.Diagnostics;

    public class Appointment
    {
        public Appointment()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public bool IsHappening { get; set; }

        public int Timer { get; set; }

        public decimal TaxSoFar { get; set; }

        public DateTime Date { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string DogsitterId { get; set; }

        public Dogsitter Dogsitter { get; set; }

        public string OwnerId { get; set; }

        public Owner Owner { get; set; }
    }
}
