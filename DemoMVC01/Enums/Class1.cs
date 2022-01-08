using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace DemoMVC01.Enums
{
    public class Class1 : ValidationAttribute, IClientModelValidator
    {
        public Class1()
        {
            if (ErrorMessage==null)
            {
                ErrorMessage = "error message is null";
            }
        }
        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-name", ErrorMessage);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string s = (string)value;
            if (s == "aaaa")
                return new ValidationResult(null);

            return ValidationResult.Success;
        }
        
    }

}
