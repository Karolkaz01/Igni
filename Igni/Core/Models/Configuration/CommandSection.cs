using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Configuration
{
    public class CommandSection
    {
        public string? name { get; set; }
        public string? description { get; set; }
        public IEnumerable<Command>? values { get; set; }
    }
}
