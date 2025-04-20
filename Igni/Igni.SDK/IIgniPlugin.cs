namespace Igni.SDK
{
    public interface IIgniPlugin
    {
        IIgniContext Context { get; set; }

        void Initialize(CancellationToken? cancellationTokens);
        void ExecuteAsync(CancellationToken? cancellationToken, string speech);
    }
}
