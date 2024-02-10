using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Configuration
{
    public class Command
    {
        public string ActivationCommand { get; set; }
        public CommandType CommandType { get; set; }
        public string Value { get; set; }
    }
}
