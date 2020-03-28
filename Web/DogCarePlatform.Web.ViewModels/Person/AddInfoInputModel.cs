namespace DogCarePlatform.Web.ViewModels.Person
{
    using DogCarePlatform.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class AddInfoInputModel
    {
        [Required(ErrorMessage = "Моля въведете име")]
        [RegularExpression(@"^[\p{L} \.'\-]+$")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Моля въведете презиме")]
        [RegularExpression(@"^[\p{L} \.'\-]+$")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Моля въведете фамилия")]
        [RegularExpression(@"^[\p{L} \.'\-]+$")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Моля въведете дата на раждане")]
        public DateTime DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Моля въведете телефонен номер")]
        [RegularExpression(@"/ 08[789]\d{7}/")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Моля изберете профилна снимка")]
        [Display(Name = "Профилна снимка")]
        public string ImageUrl { get; set; }
    }
}
