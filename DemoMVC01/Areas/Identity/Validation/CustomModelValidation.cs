using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace DemoMVC01.Areas.Identity.Validation
{
    public class CheckUsernameAttribute : ValidationAttribute, IClientModelValidator
    {
        public CheckUsernameAttribute()
        {
            if (string.IsNullOrEmpty(ErrorMessage))
            {
                ErrorMessage = "Username: Username is used. Please try a orther username!";
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
            if (s == "aaaa1")
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
    public class CheckEmailAttribute : ValidationAttribute, IClientModelValidator
    {
        public CheckEmailAttribute()
        {
            if (string.IsNullOrEmpty(ErrorMessage))
            {
                ErrorMessage = "Email: Email is used. Please try a orther Email!";
            }
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-tea", ErrorMessage);
        }
        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            string s = (string)value;
            if (s == "aaa1")
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
    /*public class CheckUsernameAttributeAdapter : AttributeAdapterBase<CheckUsernameAttribute>
    {
        public CheckUsernameAttributeAdapter(CheckUsernameAttribute attribute,
            IStringLocalizer stringLocalizer)
            : base(attribute, stringLocalizer)
        {

        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-name2", GetErrorMessage(context));

        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext) =>
            Attribute.ErrorMessage;
    }
    
    public class CheckEmailAttributeAdapter : AttributeAdapterBase<CheckEmailAttribute>
    {
        public CheckEmailAttributeAdapter(CheckEmailAttribute attribute,
            IStringLocalizer stringLocalizer)
            : base(attribute, stringLocalizer)
        {

        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-email2", GetErrorMessage(context));

        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext) =>
            Attribute.ErrorMessage;
    }
    public class CustomValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
    {
        private readonly IValidationAttributeAdapterProvider baseProvider =
            new ValidationAttributeAdapterProvider();

        public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute,
            IStringLocalizer stringLocalizer)
        {
            if (attribute is CheckEmailAttribute checkEmailAttribute)
            {
                return new CheckEmailAttributeAdapter(checkEmailAttribute, stringLocalizer);
            }
            if (attribute is CheckUsernameAttribute checkUsernameAttribute)
            {
                return new CheckUsernameAttributeAdapter(checkUsernameAttribute, stringLocalizer);
            }

            return baseProvider.GetAttributeAdapter(attribute, stringLocalizer);
        }
    }*/
}
