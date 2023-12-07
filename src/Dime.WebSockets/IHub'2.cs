namespace Dime.WebSockets
{
    /// <summary>
    /// Contracts of a hub that is capable of broadcasting store items
    /// </summary>
    /// <typeparam name="T">The type that in scope</typeparam>
    public interface IHub<in T, in TContext> : IHub<T>
    {
        void SetContext(TContext context);
    }
}