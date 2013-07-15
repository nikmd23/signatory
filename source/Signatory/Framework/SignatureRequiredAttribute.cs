using System;
using System.ComponentModel.DataAnnotations;

namespace Signatory.Framework
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SignatureRequiredAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var text = value as string;

            if (text == null || text.Equals("[\"image/jsignature;base30\",\"\"]"))
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));

            return null;
        }
    }
}