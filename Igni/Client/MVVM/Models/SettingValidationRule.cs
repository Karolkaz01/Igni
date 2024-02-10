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
    public class SettingValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            SettingMVVM plugin = (value as BindingGroup).Items[0] as SettingMVVM;
            if (!string.IsNullOrEmpty(plugin.Value) &&
                !string.IsNullOrEmpty(plugin.Name))
            {
                return ValidationResult.ValidResult;
            }
            return new ValidationResult(false, "Can't be empty");
        }
    }
}
