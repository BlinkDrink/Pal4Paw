namespace DogCarePlatform.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Dogsitter : ApplicationUser
    {
        public Dogsitter()
        {
            this.Appointments = new HashSet<Appointment>();
            this.Comments = new HashSet<Comment>();
        }

        public string Name { get; set; }

        public int Age { get; set; }

        public Gender Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public decimal WageRate { get; set; }

        public string Address { get; set; }

        public decimal Rating { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }
}
