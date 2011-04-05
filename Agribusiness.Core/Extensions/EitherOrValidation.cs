using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Agribusiness.Core.Extensions
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EitherOrAttribute : EitherOrValidation
    {
        public EitherOrAttribute(string[] otherProperties) : base (otherProperties)
        {
        }
    }

    public abstract class EitherOrValidation : ValidationAttribute
    {
        protected EitherOrValidation(string[] otherProperties)
        {
            if (otherProperties == null)
            {
                throw new ArgumentException("otherProperties");
            }

            OtherProperties = otherProperties;
        }

        public string[] OtherProperties { get; private set; }

        public override string FormatErrorMessage(string name)
        {
            if (ErrorMessage == null && ErrorMessageResourceName == null)
            {
                ErrorMessage = "Either {0} or {1} cannot be both null or both filled.";
            }

            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, string.Join(",", OtherProperties));
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var memberNames = new[] {validationContext.MemberName};

            List<object> values = new List<object>();

            foreach (var a in OtherProperties)
            {
                var propInfo = validationContext.ObjectType.GetProperty(a);

                if (propInfo == null)
                {
                    return new ValidationResult(string.Format(CultureInfo.CurrentCulture, "Could not find a property name {0}.", a), memberNames);
                }

                values.Add(propInfo.GetValue(validationContext.ObjectInstance, null));
            }

            // false if all null
            var anyOtherNonNull = values.Where(a => a != null).Any();

            // all values are null
            if (value == null && !anyOtherNonNull)
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName), memberNames);
            }

            // both have values filled in
            if (value != null && anyOtherNonNull)
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName), memberNames);
            }

            // only one is filled in.
            return null;

        }
    }
}
