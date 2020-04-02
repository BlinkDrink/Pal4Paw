namespace DogCarePlatform.Web.ViewModels.Administration.Dashboard
{
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Services.Mapping;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ApplicantViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public ICollection<QuestionAnswer> QuestionsAnswers { get; set; }

        public DateTime CreatedOn { get; set; }

    }
}
