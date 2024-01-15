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
        public string activationCommand { get; set; }
        public CommandType commandType { get; set; }
        public string value { get; set; }
    }
}
