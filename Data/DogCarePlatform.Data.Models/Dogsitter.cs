namespace DogCarePlatform.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    using DogCarePlatform.Data.Common.Models;

    public class Dogsitter : BaseDeletableModel<string>
    {
        public Dogsitter()
        {

            this.Id = Guid.NewGuid().ToString();
            this.Appointments = new HashSet<Appointment>();
            this.Comments = new HashSet<Comment>();

            this.Rating = new HashSet<Rating>();
            this.Notifications = new HashSet<Notification>();
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

        public virtual ICollection<Rating> Rating { get; set; }

        public ApplicationStatus ApplicationStatus { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
