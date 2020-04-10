namespace DogCarePlatform.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DogCarePlatform.Web.ViewModels.Dogsitter;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class DogsitterController : Controller
    {
        // TODO: Style the view
        [AllowAnonymous]
        public IActionResult SuccessfullySentApplication()
        {
            return View();
        }
    }
}
