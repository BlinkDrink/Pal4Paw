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
        Т ApplicantDetailsById<Т>(string id);
        Task RemoveQuestionsAnswersFromUserAsync(string userId);
        Task AddDogsitterAsync(string id);
    }
}
