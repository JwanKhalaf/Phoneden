namespace Phoneden.ViewModels.Reports
{
  using System;
  using System.ComponentModel.DataAnnotations;
  using System.Reflection;

  [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
  public class DateGreaterThanAttribute : ValidationAttribute
  {
    private readonly string otherPropertyName;

    public DateGreaterThanAttribute(string otherPropertyName, string errorMessage)
      : base(errorMessage)
    {
      this.otherPropertyName = otherPropertyName;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      ValidationResult validationResult = ValidationResult.Success;

      // Using reflection we can get a reference to the other date property, in this example the project start date
      PropertyInfo otherPropertyInfo = validationContext.ObjectType.GetProperty(otherPropertyName);

      // Let's check that otherProperty is of type DateTime as we expect it to be
      if (otherPropertyInfo.PropertyType == default(DateTime).GetType())
      {
        DateTime toValidate = (DateTime)value;
        DateTime referenceProperty = (DateTime)otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);

        // if the end date is lower than the start date, than the validationResult will be set to false and return
        // a properly formatted error message
        if (toValidate.CompareTo(referenceProperty) <= -1)
        {
          validationResult = new ValidationResult(ErrorMessageString);
        }
      }
      else
      {
        validationResult = new ValidationResult("An error occurred while validating the property. OtherProperty is not of type DateTime");
      }

      return validationResult;
    }
  }
}
