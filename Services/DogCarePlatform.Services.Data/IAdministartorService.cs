namespace DogCarePlatform.Services.Data
{
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Web.ViewModels.Administration.Dashboard;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IAdministartorService
    {
        Task<ICollection<ApplicationUser>> GetApplicants();

        Т ApplicantDetailsById<Т>(string id);

        Task ApproveApplicant(string id);

        Task RejectApplicant(string id);
    }
}
