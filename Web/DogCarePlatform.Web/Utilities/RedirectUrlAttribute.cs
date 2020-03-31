using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogCarePlatform.Web.Utilities
{
    public class RedirectUrlAttribute : ActionFilterAttribute
    {
        private readonly string _action;
        private readonly string _controller;
        private readonly string _role;

        public RedirectUrlAttribute(string action, string controller, string role)
        {
            _action = action;
            _controller = controller;
            _role = role;
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.User.IsInRole(_role))
            {
                filterContext.Result = new RedirectToRouteResult(new { action = _action, controller = _controller });
            }
            else
            {
                base.OnResultExecuting(filterContext);
            }
        }
    }
}
