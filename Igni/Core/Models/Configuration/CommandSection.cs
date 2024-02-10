using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Configuration
{
    public class CommandSection
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IEnumerable<Command>? Values { get; set; }
    }
}
