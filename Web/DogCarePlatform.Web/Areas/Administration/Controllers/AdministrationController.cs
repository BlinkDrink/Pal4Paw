namespace DogCarePlatform.Web.Areas.Administration.Controllers
{
    using DogCarePlatform.Common;
    using DogCarePlatform.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {

    }
}
