namespace DogCarePlatform.Web.Utilities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class DateValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            value = (DateTime)value;

            // This assumes inclusivity, i.e. exactly six years ago is okay
            if (DateTime.UtcNow.AddDays(-1).CompareTo(value) <= 0 && DateTime.UtcNow.AddYears(1).CompareTo(value) >= 0)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Датата трябва да е минимум от днес до след 1 година!");
            }
        }
    }
}
