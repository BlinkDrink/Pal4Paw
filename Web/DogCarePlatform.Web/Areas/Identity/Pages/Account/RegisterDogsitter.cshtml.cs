namespace DogCarePlatform.Web.Areas.Identity.Pages.Account
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using DogCarePlatform.Common;
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Services.Data;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Logging;

    [AllowAnonymous]
    public class RegisterDogsitterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IUsersService usersService;

        public RegisterDogsitterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IUsersService usersService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            this.usersService = usersService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage ="Моля въведете електронна поща")]
            [EmailAddress(ErrorMessage = "Това не е валидна електронна поща")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Моля въведете парола")]
            [StringLength(100, ErrorMessage = "Полето {0} трябва да съдържа от {2} до {1} символа.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Парола")]
            public string Password { get; set; }

            [Required(ErrorMessage ="Моля повторете паролата")]
            [DataType(DataType.Password)]
            [Display(Name = "Потвърждаване на паролата")]
            [Compare("Password", ErrorMessage = "Паролите трябва да съвпадат.")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "Моля въведете телефонен номер")]
            [RegularExpression(@"^([+]?359)|0?(|-| )8[789]\d{1}(|-| )\d{3}(|-| )\d{3}$", ErrorMessage = "Невалиден български телефонен номер")]
            [Display(Name = "Телефонен номер")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "Моля попълнете полето")]
            [StringLength(500, MinimumLength = 50, ErrorMessage = "Полето трябва да съдържа между 50 до 500 символа")]
            [RegularExpression("^[а-я А-Я 0-9_.,-]*$", ErrorMessage = "Моля пишете на кирилица")]
            [Display(Name="Въпрос 1")]
            public string Question1 { get; set; }

            [Required(ErrorMessage = "Моля попълнете полето")]
            [StringLength(500, MinimumLength = 50, ErrorMessage = "Полето трябва да съдържа между 50 до 500 символа")]
            [RegularExpression("^[а-я А-Я 0-9_.,-]*$", ErrorMessage = "Моля пишете на кирилица")]
            [Display(Name = "Въпрос 2")]
            public string Question2 { get; set; }

            [Required(ErrorMessage = "Моля попълнете полето")]
            [StringLength(500, MinimumLength = 50, ErrorMessage = "Полето трябва да съдържа между 50 до 500 символа")]
            [RegularExpression("^[а-я А-Я 0-9_.,-]*$", ErrorMessage = "Моля пишете на кирилица")]
            [Display(Name = "Въпрос 3")]
            public string Question3 { get; set; }

            [Required(ErrorMessage = "Моля попълнете полето")]
            [StringLength(500, MinimumLength = 50, ErrorMessage = "Полето трябва да съдържа между 50 до 500 символа")]
            [RegularExpression("^[а-я А-Я 0-9_.,-]*$", ErrorMessage = "Моля пишете на кирилица")]
            [Display(Name = "Въпрос 4")]
            public string Question4 { get; set; }

            [Required(ErrorMessage = "Моля попълнете полето")]
            [StringLength(500, MinimumLength = 50, ErrorMessage = "Полето трябва да съдържа между 50 до 500 символа")]
            [RegularExpression("^[а-я А-Я 0-9_.,-]*$", ErrorMessage = "Моля пишете на кирилица")]
            [Display(Name = "Въпрос 5")]
            public string Question5 { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/Dogsitter/SuccessfullySentApplication");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email, PhoneNumber = Input.PhoneNumber};

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                         $"Моля потвърдете своя акаунт от тук <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'></a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
                    }
                    else
                    {
                        await this._userManager.AddToRoleAsync(user, GlobalConstants.UnapprovedUserRoleName);

                        Type clsType = typeof(InputModel);
                        PropertyInfo[] mInfo = clsType.GetProperties();

                        foreach (var property in mInfo)
                        {
                            var isDef = Attribute.IsDefined(property, typeof(DisplayAttribute));

                            if (isDef)
                            {
                                DisplayAttribute dispAttr =
                                 (DisplayAttribute)Attribute.GetCustomAttribute(
                                                    property, typeof(DisplayAttribute));

                                var propValue = Input.GetType().GetProperty(property.Name).GetValue(Input, null);

                                if (property.Name.StartsWith("Question"))
                                {
                                    await this.usersService.AddQuestionsAnswersToUser(new QuestionAnswer { Question = dispAttr.Name, Answer = propValue.ToString(), UserId = user.Id, User = user }, user);
                                }
                            }
                        }

                        return LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
