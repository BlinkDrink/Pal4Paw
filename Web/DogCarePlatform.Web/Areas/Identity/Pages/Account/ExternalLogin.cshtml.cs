namespace DogCarePlatform.Web.Areas.Identity.Pages.Account
{
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using DogCarePlatform.Common;
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Services.Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration configuration;
        private readonly IOwnersService ownersService;
        private readonly ILogger<ExternalLoginModel> _logger;

        public ExternalLoginModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender,
            IConfiguration configuration,
            IOwnersService ownersService)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._logger = logger;
            this._emailSender = emailSender;
            this.configuration = configuration;
            this.ownersService = ownersService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string LoginProvider { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Моля въведете електронна поща")]
            [EmailAddress(ErrorMessage = "Това не е валидна електронна поща")]
            [Display(Name = "Email")]
            public string Email { get; set; }

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

        public IActionResult OnGetAsync()
        {
            return this.RedirectToPage("./Login");
        }

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = this.Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = this._signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                this.ErrorMessage = $"Error from external provider: {remoteError}";
                return this.RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            var info = await this._signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                this.ErrorMessage = "Error loading external login information.";
                return this.RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await this._signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (result.Succeeded)
            {
                this._logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);

                return this.LocalRedirect(returnUrl);
            }

            if (result.IsLockedOut)
            {
                return this.RedirectToPage("./Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                this.ReturnUrl = returnUrl;
                this.LoginProvider = info.LoginProvider;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    this.Input = new InputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                    };
                }

                return this.Page();
            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            // Get the information about the user from the external login provider
            var info = await this._signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                this.ErrorMessage = "Error loading external login information during confirmation.";
                return this.RedirectToPage("./LoginOwner", new { ReturnUrl = returnUrl });
            }

            if (this.ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = this.Input.Email, Email = this.Input.Email, PhoneNumber = this.Input.PhoneNumber };
                var result = await this._userManager.CreateAsync(user);

                var cloudinaryAccount = this.configuration.GetSection("Cloudinary");

                Account account = new Account(
                    cloudinaryAccount["Cloud_Name"],
                    cloudinaryAccount["API_Key"],
                    cloudinaryAccount["API_Secret"]
                    );

                Cloudinary cloudinary = new Cloudinary(account);

                var file = this.Input.ImageFile;

                var uploadResult = new ImageUploadResult();

                var imageUrl = string.Empty;

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
                    imageUrl = this.Input.ImageUrl;
                }

                if (result.Succeeded)
                {
                    result = await this._userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        this._logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                        // If account confirmation is required, we need to show the link if we don't have a real email sender
                        if (this._userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return this.RedirectToPage("./RegisterConfirmation", new { Email = this.Input.Email });
                        }

                        await this._userManager.AddToRoleAsync(user, GlobalConstants.OwnerRoleName);
                        await this._signInManager.SignInAsync(user, isPersistent: false);
                        await this.ownersService.CreateOwnerAsync(user, this.Input.Address, this.Input.FirstName, this.Input.MiddleName, this.Input.LastName, this.Input.Gender, imageUrl, this.Input.PhoneNumber, user.Id, this.Input.Description);

                        var userId = await this._userManager.GetUserIdAsync(user);
                        string code = await this._userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = this.Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, code = code },
                            protocol: this.Request.Scheme);

                        await this._emailSender.SendEmailAsync(this.Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        return this.LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            this.LoginProvider = info.LoginProvider;
            this.ReturnUrl = returnUrl;
            return this.Page();
        }
    }
}
