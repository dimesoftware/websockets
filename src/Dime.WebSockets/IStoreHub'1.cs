using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dime.WebSockets
{
    /// <summary>
    /// Contracts of a hub that is capable of broadcasting store items
    /// </summary>
    /// <typeparam name="T">The type that in scope</typeparam>
    public interface IStoreHub<in T>
    {
        /// <summary>
        /// Broadcasts the new record
        /// </summary>
        /// <param name="item">The new record to broadcast</param>
        /// <returns>Instance of <see cref="Task"/></returns>
        Task BroadcastNew(T item);

        /// <summary>
        /// Broadcasts the new records
        /// </summary>
        /// <param name="items">The new records to broadcast</param>
        /// <returns>Instance of <see cref="Task"/></returns>
        Task BroadcastNew(IEnumerable<T> items);

        /// <summary>
        /// Broadcasts the updated record
        /// </summary>
        /// <param name="item">The updated record to broadcast</param>
        /// <returns>Instance of <see cref="Task"/></returns>
        Task BroadcastUpdated(T item);

        /// <summary>
        /// Broadcasts the updated records
        /// </summary>
        /// <param name="items">The updated record to broadcast</param>
        /// <returns>Instance of <see cref="Task"/></returns>
        Task BroadcastUpdated(IEnumerable<T> items);

        /// <summary>
        /// Broadcasts the removed record
        /// </summary>
        /// <param name="item">The removed record to broadcast</param>
        /// <returns>Instance of <see cref="Task"/></returns>
        Task BroadcastDeleted(T item);

        /// <summary>
        /// Broadcasts the removed records
        /// </summary>
        /// <param name="items">The removed records to broadcast</param>
        /// <returns>Instance of <see cref="Task"/></returns>
        Task BroadcastDeleted(IEnumerable<T> items);
    }
}