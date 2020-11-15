namespace Dime.WebSockets.InMemory
{
    /// <summary>
    /// Represents a store.
    /// </summary>
    /// <typeparam name="T">The type to hold within this instance</typeparam>
    public interface IKeyValueStore<T> : IStore<T>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        void Add(string key, T item);

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);
    }
}