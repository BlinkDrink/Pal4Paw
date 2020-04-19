namespace DogCarePlatform.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using DogCarePlatform.Web.Utilities;
    using Xunit;

    public class ValidationAttributesTests
    {
        [Fact]
        public void DateValidationAttributeShouldReturnTrueOnProperDate()
        {
            var validator = new DateValidationAttribute();

            var date = DateTime.UtcNow;

            var result = validator.IsValid(date);

            Assert.True(result);
        }

        [Fact]
        public void DateValidationAttributeShouldReturnFalseOnWrongDate()
        {
            var validator = new DateValidationAttribute();

            var date = DateTime.UtcNow.AddDays(-5);

            var result = validator.IsValid(date);

            Assert.False(result);
        }

        [Fact]
        public void DateValidationAttributeShouldReturnProperMessageOnWrongDate()
        {
            var validator = new DateValidationAttribute();

            var date = DateTime.UtcNow.AddDays(-5);

            var result = validator.GetValidationResult(date, new ValidationContext(date));

            Assert.Equal("Датата трябва да е минимум от днес до след 1 година!", result.ErrorMessage);
        }
    }
}
