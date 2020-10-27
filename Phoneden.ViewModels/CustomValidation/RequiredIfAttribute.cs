namespace Phoneden.ViewModels.CustomValidation
{
  using System;
  using System.ComponentModel.DataAnnotations;

  public class RequiredIfAttribute : RequiredAttribute
  {
    private string PropertyName { get; set; }

    private object DesiredValue { get; set; }

    public RequiredIfAttribute(string propertyName, object desiredValue, string customErrorMessage)
    {
      PropertyName = propertyName;

      DesiredValue = desiredValue;

      ErrorMessage = customErrorMessage;
    }

    protected override ValidationResult IsValid(object value, ValidationContext context)
    {
      object instance = context.ObjectInstance;

      Type type = instance.GetType();

      object propertyValue = type
        .GetProperty(PropertyName)
        ?.GetValue(instance, null);

      if (propertyValue == null || propertyValue.ToString() != DesiredValue.ToString())
      {
        return ValidationResult.Success;
      }

      ValidationResult result = base.IsValid(value, context);

      return result;
    }
  }
}
