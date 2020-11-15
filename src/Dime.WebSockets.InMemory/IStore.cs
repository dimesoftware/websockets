using System.Collections.Generic;

namespace Dime.WebSockets.InMemory
{
    /// <summary>
    /// Represents a store.
    /// </summary>
    /// <typeparam name="T">The type to hold within this instance</typeparam>
    public interface IStore<T>
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> Find();

        /// <summary>
        ///
        /// </summary>
        void Purge();
    }
}