namespace DogCarePlatform.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class DogsitterController : Controller
    {
        [AllowAnonymous]
        public IActionResult SuccessfullySentApplication()
        {
            return this.View();
        }
    }
}
