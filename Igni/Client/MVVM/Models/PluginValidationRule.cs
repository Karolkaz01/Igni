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
    public class PluginValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            PluginInfoMVVM plugin = (value as BindingGroup).Items[0] as PluginInfoMVVM;
            if (!string.IsNullOrEmpty(plugin.PluginName) &&
                !string.IsNullOrEmpty(plugin.FileName) &&
                !string.IsNullOrEmpty(plugin.DirectoryName))
            {
                return ValidationResult.ValidResult;
            }
            return new ValidationResult(false, "Can't be empty");
        }
    }
}
