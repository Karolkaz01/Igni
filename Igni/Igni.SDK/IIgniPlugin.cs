namespace Igni.SDK
{
    public interface IIgniPlugin
    {
        void InitializePlugin();
        void PerformPlugin();
        bool ExecuteContinues();

    }
}
