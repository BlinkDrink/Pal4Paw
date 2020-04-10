namespace DogCarePlatform.Web.ViewModels.Administration.Dashboard
{
    using System.Collections.Generic;

    using DogCarePlatform.Data.Models;

    public class UnapprovedUsersViewModel
    {
        public ICollection<ApplicationUser> Applicants { get; set; }
    }
}
