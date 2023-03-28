namespace Dime.WebSockets
{
    public interface IConnectionTracker<T, TContext> : IConnectionTracker<T> where T : IWebSocketConnection
    {
        void SetContext(TContext context);
    }
}