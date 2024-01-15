using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igni.SDK
{
    public abstract class IgniPlugin
    {
        public abstract bool IsEnabled { get; set; }
        public abstract string Name { get; set; }


        protected IgniPlugin()
        {
            IsEnabled = true;

        }

        public abstract void Initialize();
        public abstract void SingleExcecute();
        public abstract string ContinuesExcecute();

    }
}
