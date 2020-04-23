namespace DogCarePlatform.Web.ViewModels.Utilities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class EndTimeValidationAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        private const string DefaultErrorMessage =
    "Крайното време не трябва да бъде преди началното";

        public EndTimeValidationAttribute(string comparisonProperty)
        {
            this._comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //this.ErrorMessage = this.ErrorMessageString;
            var currentValue = (DateTime)value;

            var property = validationContext.ObjectType.GetProperty(this._comparisonProperty);

            if (property == null)
            {
                throw new ArgumentException("Property with this name not found");
            }

            var comparisonValue = (DateTime)property.GetValue(validationContext.ObjectInstance);

            if (currentValue <= comparisonValue)
            {
                return new ValidationResult(ErrorMessage ?? DefaultErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
