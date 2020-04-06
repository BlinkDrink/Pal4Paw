namespace DogCarePlatform.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics;

    using DogCarePlatform.Data.Common.Models;

    public class Appointment : BaseDeletableModel<string>
    {
        public Appointment()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public AppointmentStatus Status { get; set; }

        public int Timer { get; set; }

        public decimal TaxSoFar { get; set; }

        public DateTime Date { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string OwnerId { get; set; }

        public virtual Owner Owner { get; set; }

        public string DogsitterId { get; set; }

        public virtual Dogsitter Dogsitter { get; set; }
    }
}
