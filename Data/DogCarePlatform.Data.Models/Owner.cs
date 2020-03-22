namespace DogCarePlatform.Data.Models
{
    using System.Collections.Generic;

    public class Owner : ApplicationUser
    {
        public Owner()
        {
            this.Comments = new HashSet<Comment>();
            this.Dogs = new HashSet<Dog>();
            this.Appointments = new HashSet<Appointment>();
        }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public decimal Rating { get; set; }

        public string Address { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<Dog> Dogs { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }
}