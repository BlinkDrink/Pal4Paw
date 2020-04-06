namespace DogCarePlatform.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using DogCarePlatform.Data.Common.Models;

    public class Owner : BaseDeletableModel<string>
    {
        public Owner()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Comments = new HashSet<Comment>();
            this.Appointments = new HashSet<Appointment>();
        }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string ImageUrl { get; set; }

        public Gender Gender { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string DogsDescription { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }

        public virtual ICollection<Rating> Rating { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
