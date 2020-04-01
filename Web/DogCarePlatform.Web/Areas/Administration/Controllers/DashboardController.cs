namespace DogCarePlatform.Web.Areas.Administration.Controllers
{
    using DogCarePlatform.Services.Data;
    using DogCarePlatform.Web.ViewModels.Administration.Dashboard;

    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    public class DashboardController : AdministrationController
    {
        private readonly ISettingsService settingsService;
        private readonly IAdministartorService administartorService;

        public DashboardController(ISettingsService settingsService, IAdministartorService administartorService)
        {
            this.settingsService = settingsService;
            this.administartorService = administartorService;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel { SettingsCount = this.settingsService.GetCount(), };
            return this.View(viewModel);
        }

        public IActionResult RegulationPage()
        {
            return this.View();
        }

        /// <summary>
        /// Lists all UnapprovedUsers of the system. The service checks if the user
        /// is in role of "UnapprovedUser".
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> RegulateApplicants()
        {
            var applicants = await this.administartorService.GetApplicants();

            var viewModel = new UnapprovedUsersViewModel
            {
                Applicants = applicants,
            };

            return this.View(viewModel);
        }

        public IActionResult RegulateComments()
        {
            return this.View();
        }

        public IActionResult RegulateAppointments()
        {
            return this.View();
        }

        public IActionResult ApplicantById(string id)
        {
            var applicantViewModel = this.administartorService.ApplicantDetailsById<ApplicantViewModel>(id);

            return this.View(applicantViewModel);
        }
    }
}
