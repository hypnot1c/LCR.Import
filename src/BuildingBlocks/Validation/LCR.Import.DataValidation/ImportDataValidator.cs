using FluentValidation;
using FluentValidation.Validators;
using LCR.TPM.Model;
using System;
using System.Globalization;

namespace LCR.Import.DataValidation
{
  public class ImportDataValidator : AbstractValidator<ImportRawDataModel>
  {
    public ImportDataValidator()
    {
      var russianCulture = new CultureInfo("ru-RU");
      var usCulture = new CultureInfo("en-US");
      RuleFor(c => c.DataRowId).Cascade(CascadeMode.StopOnFirstFailure)
        .NotNull().WithErrorCode("2")
        //.Must(c => Int32.TryParse(c, out var res)).WithErrorCode("2")
        ;

      Func<ImportRawDataModel, string, PropertyValidatorContext, bool> validationFunc = (model, val, ctx) =>
      {
        if (String.IsNullOrEmpty(model.SwitchOperatorName) && String.IsNullOrEmpty(model.PairedSwitchOperatorFullName))
        {
          return false;
        }
        return true;
      };
      RuleFor(c => c.SwitchOperatorName).Must(validationFunc).WithErrorCode("4");
      RuleFor(c => c.PairedSwitchOperatorFullName).Must(validationFunc).WithErrorCode("4");

      RuleFor(c => c.Direction).Cascade(CascadeMode.StopOnFirstFailure)
        .NotNull().WithErrorCode("8")
        .NotEmpty().WithErrorCode("8")
        .Must(v => v.ToUpper() == "I" || v.ToUpper() == "O").WithErrorCode("8");

      RuleFor(c => c.DateOpen).Cascade(CascadeMode.StopOnFirstFailure)
        .NotNull().WithErrorCode("16")
        .Must(val =>
        {
          try
          {
            var date = DateTime.Parse(val, russianCulture);
            return true;
          }
          catch
          {
            try
            {
              var date = DateTime.Parse(val, usCulture);
              return true;
            }
            catch
            {
              return false;
            }
          }
        }).WithErrorCode("16");

      RuleFor(c => c.DateClose)
        .Must(val =>
        {
          if (!String.IsNullOrEmpty(val))
          {
            try
            {
              var date = DateTime.Parse(val, russianCulture);
              return true;
            }
            catch
            {
              try
              {
                var date = DateTime.Parse(val, usCulture);
                return true;
              }
              catch
              {
                return false;
              }
            }
          }
          return true;
        }).WithErrorCode("32");
    }
  }
}
