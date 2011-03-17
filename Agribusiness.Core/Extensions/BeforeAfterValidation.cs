using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace DataAnnotationsExtensions
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class BeforeAttribute : DateComparisonBaseAttribute
    {
        public BeforeAttribute(string otherProperty)
            : base(otherProperty, DateComparisonType.Before)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AfterAttribute : DateComparisonBaseAttribute
    {
        public AfterAttribute(string otherProperty)
            : base(otherProperty, DateComparisonType.After)
        {
        }
    }

    public abstract class DateComparisonBaseAttribute : ValidationAttribute
    {
        private readonly DateComparisonType _dateComparisonType;

        protected DateComparisonBaseAttribute(string otherProperty, DateComparisonType dateComparisonType)
        {
            _dateComparisonType = dateComparisonType;
            if (otherProperty == null)
            {
                throw new ArgumentNullException("otherProperty");
            }
            OtherProperty = otherProperty;
        }

        public string OtherProperty { get; private set; }

        public override string FormatErrorMessage(string name)
        {
            if (ErrorMessage == null && ErrorMessageResourceName == null)
            {
                ErrorMessage = "The field {0} must occur {1} the field {2}.";
            }

            return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, _dateComparisonType, OtherProperty);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var memberNames = new[] {validationContext.MemberName};

            PropertyInfo otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
            if (otherPropertyInfo == null)
            {
                return new ValidationResult(String.Format(CultureInfo.CurrentCulture, "Could not find a property named {0}.", OtherProperty), memberNames);
            }

            object otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);

            if (otherPropertyValue == null || value == null)
            {
                return null;
            }

            DateTime valueDate;
            DateTime otherDate;

            DateTime.TryParse(Convert.ToString(value), DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out valueDate);
            DateTime.TryParse(Convert.ToString(otherPropertyValue), DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out otherDate);

            var comparison = DateTime.Compare(valueDate, otherDate);

            switch (_dateComparisonType)
            {
                case DateComparisonType.Before:
                    if (comparison >= 0)
                    {
                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName), memberNames);
                    }
                    break;
                case DateComparisonType.After:
                    if (comparison <= 0)
                    {
                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName), memberNames);
                    }
                    break;
            }

            return null;
        }

        protected enum DateComparisonType
        {
            Before,
            After
        }
    }
}