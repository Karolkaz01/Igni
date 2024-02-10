using Core.Enums;
using Core.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace Client.MVVM.Models
{
    public class CommandValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            Command command = (value as BindingGroup).Items[0] as Command;
            if (!string.IsNullOrEmpty(command.Value) &&
                !string.IsNullOrEmpty(command.ActivationCommand) &&
                command.CommandType != null)
            {
                return ValidationResult.ValidResult;
            }
            return new ValidationResult(false, "Can't be empty");
        }
    }
}
