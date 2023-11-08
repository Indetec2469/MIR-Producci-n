using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace RequisicionesAlmacenBL.Models.Attributes
{
    public class ManualStringLengthAttribute : ValidationAttribute, IClientValidatable
    {
        //private readonly int? _minimumLength;
        //private readonly int? _maximumLength;
        public int _minimumLength { get; private set; }
        public int _maximumLength { get; private set; }
        public ManualStringLengthAttribute(int minimumLength,int maximumLength)
        {
            _minimumLength = minimumLength;
            _maximumLength = maximumLength;
        }

        //public override bool IsValid(object value)
        //{
        //    //string val = (String)value;
        //    //Console.WriteLine(val);
        //    //if (val.Length < _minimumLength)
        //    //{
        //    //    base.ErrorMessage = "La Mínima de " + _minimumLength + " caracteres";
        //    //    return false;
        //    //} 
        //    //if (val.Length > _maximumLength)
        //    //{
        //    //    base.ErrorMessage = "La Máxima de " + _maximumLength + " caracteres";
        //    //    return false;
        //    //}
        //    //return false; 
        //    return false;
        //}

        //protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        //{
        //    return base.IsValid(value, validationContext);
        //}
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string val = (String)value;
            if (val.Length < _minimumLength)
            {
                //base.ErrorMessage = "La Mínima de " + _minimumLength + " caracteres";
                return new ValidationResult("La Mínima de " + _minimumLength + " caracteres");
            }
            if (val.Length > _maximumLength)
            {
                //base.ErrorMessage = "La Máxima de " + _maximumLength + " caracteres";
                return new ValidationResult("La Mínima de " + _maximumLength + " caracteres");
            }
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = ErrorMessage;
            //rule.ValidationParameters.Add("max", _maximumLength);
            //rule.ValidationParameters.Add("min", _minimumLength);
            rule.ValidationParameters.Add(
                "validationcallback",
                $@"function(options) {{
                    return options.value && options.value.length < {_minimumLength} || options.value.length > {_maximumLength};
                }}");
            rule.ValidationType = "custom";
            yield return rule;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name, "hola mundo");
        }
    }
}
