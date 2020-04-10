namespace DogCarePlatform.Web.ViewModels.Owner
{
    using System.ComponentModel.DataAnnotations;

    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Services.Mapping;

    public class AddInfoInputModel : IMapFrom<Owner>
    {
        [Required(ErrorMessage = "Моля въведете име")]
        [RegularExpression(@"^[\p{L} \.'\-]+$")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Моля въведете презиме")]
        [RegularExpression(@"^[\p{L} \.'\-]+$")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Моля въведете фамилия")]
        public string LastName { get; set; }

        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Моля въведете телефонен номер")]
        [RegularExpression(@"^([+]?359)|0?(|-| )8[789]\d{1}(|-| )\d{3}(|-| )\d{3}$", ErrorMessage ="Невалиден български телефонен номер")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Моля изберете профилна снимка")]
        [Display(Name = "Профилна снимка")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Въведете адрес на улица")]
        public string Address { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        public string UserId { get; set; }
    }
}
