namespace DogCarePlatform.Web.ViewModels.Administration.Dashboard
{
    using System.Collections.Generic;

    using DogCarePlatform.Data.Models;

    public class DogsitterApplicantViewModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public ICollection<QuestionAnswer> QuestionsAnswers { get; set; }
    }
}
