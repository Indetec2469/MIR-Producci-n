using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RequisicionesAlmacenBL.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class CorreosAttribute : ValidationAttribute
    {
        private readonly string _correo1;

        private readonly string _correo2;

        public CorreosAttribute(string correo1, string correo2)
        {
            _correo1 = correo1;

            _correo2 = correo2;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property1 = validationContext.ObjectType.GetProperty(_correo1);

            var property2 = validationContext.ObjectType.GetProperty(_correo2);

            if (property1 == null)
            {
                throw new ArgumentException("Property with this name not found");
            }

            if (property2 == null)
            {
                throw new ArgumentException("Property with this name not found");
            }

            var correo1 = (string) property1.GetValue(validationContext.ObjectInstance);

            var correo2 = (string) property2.GetValue(validationContext.ObjectInstance);

            ErrorMessage = ErrorMessageString;

            if (string.IsNullOrEmpty(correo1) && string.IsNullOrEmpty(correo2))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            string errorMessage = ErrorMessageString;

            // The value we set here are needed by the jQuery adapter
            ModelClientValidationRule modelClientValidationRule = new ModelClientValidationRule();
            modelClientValidationRule.ErrorMessage = errorMessage;
            modelClientValidationRule.ValidationType = "dategreaterthan"; // This is the name the jQuery adapter will use
            
            //"otherpropertyname" is the name of the jQuery parameter for the adapter, must be LOWERCASE!
            modelClientValidationRule.ValidationParameters.Add("correo1", _correo1);
            modelClientValidationRule.ValidationParameters.Add("correo2", _correo2);

            yield return modelClientValidationRule;
        }
    }
}