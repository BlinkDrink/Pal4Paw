namespace DogCarePlatform.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using DogCarePlatform.Data.Common.Models;

    public class Dogsitter : BaseDeletableModel<string>
    {
        public Dogsitter()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Appointments = new HashSet<Appointment>();
            this.Comments = new HashSet<Comment>();
        }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        public string Address { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public string PhoneNumber { get; set; }

        public decimal WageRate { get; set; }

        public decimal Rating { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }
}
