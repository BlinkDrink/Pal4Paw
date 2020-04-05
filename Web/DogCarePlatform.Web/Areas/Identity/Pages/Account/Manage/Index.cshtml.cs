namespace DogCarePlatform.Web.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using DogCarePlatform.Common;
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Services.Data;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Configuration;

    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IDogsittersService dogsitterService;
        private readonly IOwnersService ownersService;
        private readonly IConfiguration configuration;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IDogsittersService dogsitterService,
            IOwnersService ownersService,
            IConfiguration configuration)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this.dogsitterService = dogsitterService;
            this.ownersService = ownersService;
            this.configuration = configuration;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Телефонен номер")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "Моля въведете име")]
            [RegularExpression("[А-я]+", ErrorMessage = "Името трябва да на кирилица")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Моля въведете презиме")]
            [RegularExpression("[А-я]+", ErrorMessage = "Презимето трябва да на кирилица")]
            public string MiddleName { get; set; }

            [Required(ErrorMessage = "Моля въведете фамилия")]
            [RegularExpression("[А-я]+", ErrorMessage = "Фамилията трябва да на кирилица")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Моля въведете дата на раждане")]
            [Display(Name = "Дата на раждане")]
            public DateTime DateOfBirth { get; set; }

            [Required(ErrorMessage = "Моля изберете профилна снимка")]
            [Display(Name = "Профилна снимка")]
            public string ImageUrl { get; set; }

            public IFormFile ImageFile { get; set; }

            [Required(ErrorMessage = "Въведете адрес на улица")]
            public string Address { get; set; }

            [Required(ErrorMessage = "Моля опишете себе си в няколко изречения")]
            [Display(Name = "Лично описание")]
            public string Description { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            var isDogsitter = await _userManager.IsInRoleAsync(user, GlobalConstants.DogsitterRoleName);
            var isOwner = await _userManager.IsInRoleAsync(user, GlobalConstants.OwnerRoleName);

            if (isDogsitter)
            {
                var dogsitter = this.dogsitterService.GetDogsitterById(user.Id);

                Input = new InputModel
                {
                    PhoneNumber = phoneNumber,
                    FirstName = dogsitter.FirstName,
                    MiddleName = dogsitter.MiddleName,
                    LastName = dogsitter.LastName,
                    DateOfBirth = dogsitter.DateOfBirth,
                    Address = dogsitter.Address,
                    Description = dogsitter.Description,
                    ImageUrl = dogsitter.ImageUrl,
                };

                return;
            }
            else if (isOwner)
            {
                var owner = this.ownersService.GetOwnerById(user.Id);

                Input = new InputModel
                {
                    PhoneNumber = phoneNumber,
                    FirstName = owner.FirstName,
                    MiddleName = owner.MiddleName,
                    LastName = owner.LastName,
                    Address = owner.Address,
                    Description = owner.DogsDescription,
                    ImageUrl = owner.ImageUrl,
                };

                return;
            }

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

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

            if (this.User.IsInRole(GlobalConstants.DogsitterRoleName))
            {
                await this.dogsitterService.CurrentUserAddInfo(user.Id, Input.FirstName, Input.MiddleName, Input.LastName, Input.DateOfBirth, Input.Address, Input.Description, imageUrl);
            }
            else if (this.User.IsInRole(GlobalConstants.OwnerRoleName))
            {
                await this.ownersService.UpdateCurrentLoggedInUserInfoAsync(user.Id, Input.FirstName, Input.MiddleName, Input.LastName, Input.Address, Input.Description, imageUrl);
            }


            this.TempData["SuccessfullyUpdated"] = "Успешно запазихте промените";
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
