namespace DogCarePlatform.Web.ViewModels.Owner
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Services.Mapping;

    public class DogsitterInfoViewModel : IMapFrom<Dogsitter>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateOfBirth { get; set; }

        public string Address { get; set; }

        public Gender Gender { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public decimal WageRate { get; set; }

        public ICollection<Rating> Rating { get; set; }

        public ApplicationStatus ApplicationStatus { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }
}
