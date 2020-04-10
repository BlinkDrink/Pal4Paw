namespace DogCarePlatform.Web.ViewModels.Administration.Dashboard
{
    using System;
    using System.Collections.Generic;

    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Services.Mapping;

    public class ApplicantViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public ICollection<QuestionAnswer> QuestionsAnswers { get; set; }

        public DateTime CreatedOn { get; set; }

    }
}
