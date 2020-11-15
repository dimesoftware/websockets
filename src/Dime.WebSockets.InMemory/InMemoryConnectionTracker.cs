using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dime.WebSockets.InMemory
{
    /// <summary>
    /// Represents an in-memory web sockets connection tracker
    /// </summary>
    /// <typeparam name="T">The connection type</typeparam>
    public class InMemoryConnectionTracker<T> : IConnectionTracker<T>
        where T : IWebSocketConnection, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryConnectionTracker{T}"/> class
        /// </summary>
        /// <param name="store">The in-memory store</param>
        public InMemoryConnectionTracker(IKeyValueStore<T> store)
        {
            Store = store;
        }

        private IKeyValueStore<T> Store { get; }

        /// <summary>
        /// Gets all connections
        /// </summary>
        /// <returns>A collection of connections</returns>
        public Task<IEnumerable<T>> GetConnectionsAsync()
            => Task.FromResult(Store.Find());

        /// <summary>
        /// Gets all connections that match the filter
        /// </summary>
        /// <param name="filter">The filter to apply on the data set</param>
        /// <returns>A collection of connections that matched the filter</returns>
        public Task<IEnumerable<T>> GetConnectionsAsync(Expression<Func<T, bool>> filter)
            => Task.FromResult(Store.Find().Where(x => filter.Compile().Invoke(x)));

        /// <summary>
        /// Adds a connection to the connection list
        /// </summary>
        /// <param name="connection">The connection to add</param>
        /// <returns></returns>
        public async Task AddAsync(T connection)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            Store.Add(connection.ConnectionId, connection);
            await Task.FromResult(0).ConfigureAwait(true);
        }

        /// <summary>
        /// Removes the connection from the connection list
        /// </summary>
        /// <param name="connection">The connection to remove</param>
        /// <returns></returns>
        public async Task RemoveAsync(T connection)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            Store.Remove(connection.ConnectionId);
            await Task.FromResult(0).ConfigureAwait(true);
        }

        /// <summary>
        /// Clears the underlying data store
        /// </summary>
        /// <returns></returns>
        public async Task Clear()
        {
            Store.Purge();
            await Task.FromResult(0).ConfigureAwait(true);
        }
    }
}