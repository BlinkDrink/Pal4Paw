namespace DogCarePlatform.Web.ViewModels.Administration.Dashboard
{
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Services.Mapping;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class UnapprovedUsersViewModel
    {
        public ICollection<ApplicationUser> Applicants { get; set; }
    }
}
