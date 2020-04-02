namespace DogCarePlatform.Web.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DogCarePlatform.Common;
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Services.Data;
    using DogCarePlatform.Web.ViewModels.Administration.Dashboard;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : AdministrationController
    {
        private readonly ISettingsService settingsService;
        private readonly IAdministartorService administartorService;
        private readonly UserManager<ApplicationUser> userManager;

        public DashboardController(ISettingsService settingsService, IAdministartorService administartorService, UserManager<ApplicationUser> userManager)
        {
            this.settingsService = settingsService;
            this.administartorService = administartorService;
            this.userManager = userManager;
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
            var applicants = await this.userManager.GetUsersInRoleAsync(GlobalConstants.UnapprovedUserRoleName);
            applicants.OrderBy(a => a.CreatedOn);

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

        [HttpPost]
        public async Task<IActionResult> ApproveApplicant(string id)
        {
            var applicant = await this.userManager.FindByIdAsync(id);

            if (applicant == null)
            {
                return this.NotFound();
            }

            await this.userManager.RemoveFromRoleAsync(applicant, GlobalConstants.UnapprovedUserRoleName);
            await this.userManager.AddToRoleAsync(applicant, GlobalConstants.DogsitterRoleName);
            await this.administartorService.AddDogsitterAsync(id);

            this.TempData["isSuccessful"] = $"Успешно одобрихте кандидат {applicant.Email}";

            return this.RedirectToAction("RegulateApplicants");
        }

        [HttpPost]
        public async Task<IActionResult> RejectApplicant(string id)
        {
            var applicant = await this.userManager.FindByIdAsync(id);

            await this.userManager.RemoveFromRoleAsync(applicant, GlobalConstants.UnapprovedUserRoleName);
            await this.administartorService.RemoveQuestionsAnswersFromUserAsync(id);

            if (applicant == null)
            {
                return this.NotFound();
            }

            var result = await this.userManager.DeleteAsync(applicant);

            if (result.Succeeded)
            {
                this.TempData["isSuccessful"] = $"Успешно отхвърлихте кандидат {applicant.Email}";

                return this.RedirectToAction("RegulateApplicants");
            }

            return this.View("RegulateApplicants");
        }
    }
}
