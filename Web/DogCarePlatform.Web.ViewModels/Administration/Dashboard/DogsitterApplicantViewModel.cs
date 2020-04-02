﻿namespace DogCarePlatform.Web.ViewModels.Administration.Dashboard
{
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Services.Mapping;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class DogsitterApplicantViewModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public ICollection<QuestionAnswer> QuestionsAnswers { get; set; }
    }
}