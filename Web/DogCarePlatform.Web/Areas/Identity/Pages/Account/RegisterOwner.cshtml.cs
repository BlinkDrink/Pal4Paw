namespace DogCarePlatform.Web.Areas.Identity.Pages.Account
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
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
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.Extensions.Configuration;
    using System.Web;
    using Microsoft.AspNetCore.Http;

    [AllowAnonymous]
    public class RegisterOwnerModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IOwnersService ownerService;
        private readonly IConfiguration configuration;

        public RegisterOwnerModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IOwnersService ownerService,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            this.ownerService = ownerService;
            this.configuration = configuration;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Моля въведете електронна поща")]
            [EmailAddress(ErrorMessage = "Това не е валидна електронна поща")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Моля въведете парола")]
            [StringLength(100, ErrorMessage = "Полето {0} трябва да съдържа от {2} до {1} символа.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Парола")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Моля повторете паролата")]
            [DataType(DataType.Password)]
            [Display(Name = "Потвърждаване на паролата")]
            [Compare("Password", ErrorMessage = "Паролите трябва да съвпадат.")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "Моля въведете име")]
            [RegularExpression("[А-я]+")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Моля въведете презиме")]
            [RegularExpression("[А-я]+")]
            public string MiddleName { get; set; }

            [Required(ErrorMessage = "Моля въведете фамилия")]
            [RegularExpression("[А-я]+")]
            public string LastName { get; set; }

            public Gender Gender { get; set; }

            [Required(ErrorMessage = "Моля въведете телефонен номер")]
            [RegularExpression(@"^([+]?359)|0?(|-| )8[789]\d{1}(|-| )\d{3}(|-| )\d{3}$", ErrorMessage = "Невалиден български телефонен номер")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "Моля изберете профилна снимка")]
            [Display(Name = "Профилна снимка")]
            public string ImageUrl { get; set; }

            public IFormFile ImageFile { get; set; }

            [Required(ErrorMessage = "Въведете адрес на улица")]
            public string Address { get; set; }

            [Required(ErrorMessage = "Описанието е задъжително")]
            [StringLength(500)]
            public string Description { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email, PhoneNumber = Input.PhoneNumber };

                var result = await _userManager.CreateAsync(user, Input.Password);

                var cloudinaryAccount = this.configuration.GetSection("Cloudinary");

                Account account = new Account(
                    cloudinaryAccount["Cloud_Name"],
                    cloudinaryAccount["API_Key"],
                    cloudinaryAccount["API_Secret"]
                    );

                Cloudinary cloudinary = new Cloudinary(account);

                var file = Input.ImageFile;

                var uploadResult = new ImageUploadResult();

                var imageUrl = "";

                if (file != null)
                {
                    if (file.Length > 0)
                    {
                        using (var stream = file.OpenReadStream())
                        {
                            var uploadParams = new ImageUploadParams()
                            {
                                File = new FileDescription(file.Name, stream),
                                Transformation = new Transformation().Width(100).Height(100).Gravity("face").Radius("max").Border("2px_solid_white").Crop("thumb"),
                            };

                            uploadResult = cloudinary.Upload(uploadParams);
                        }
                    }

                    imageUrl = uploadResult.Uri.ToString();
                }
                else
                {
                    imageUrl = Input.ImageUrl;
                }


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
                        await this._userManager.AddToRoleAsync(user, GlobalConstants.OwnerRoleName);
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        await this.ownerService.CreateOwnerAsync(user, Input.Address, Input.FirstName, Input.MiddleName, Input.LastName, Input.Gender, imageUrl, Input.PhoneNumber, user.Id, Input.Description);

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
