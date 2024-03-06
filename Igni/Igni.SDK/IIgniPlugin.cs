namespace Igni.SDK
{
    public interface IIgniPlugin
    {
        IIgniContext Context { get; set; }

        void Initialize(CancellationToken? cancellationTokens);
        void Execute(CancellationToken? cancellationToken, string speech);
    }
}
