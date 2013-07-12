using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Signatory.Framework
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SignatureAttribute : ValidationAttribute
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