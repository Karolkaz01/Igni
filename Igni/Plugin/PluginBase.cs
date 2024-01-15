using System.ComponentModel.Composition;

namespace Plugin
{
    [Export(typeof(IPlugin))]
    public abstract class PluginBase : IPlugin
    {
        public virtual void InitializePluggin()
        {
            Console.WriteLine("Abstract base plugin initialize");
        }
        public abstract void PerformPluggin();
        public abstract bool RunContinues();
    }
}
