namespace DogCarePlatform.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    using DogCarePlatform.Data.Common.Models;

    public class Owner : BaseDeletableModel<string>
    {
        public Owner()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Comments = new HashSet<Comment>();
            this.Dogs = new HashSet<Dog>();
            this.Appointments = new HashSet<Appointment>();
        }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string ImageUrl { get; set; }

        public decimal Rating { get; set; }

        public Gender Gender { get; set; }

        public string Address { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<Dog> Dogs { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }
}
