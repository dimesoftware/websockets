using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dime.WebSockets
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IConnectionTracker<T> where T : IWebSocketConnection
    {
        /// <summary>
        /// Gets all connections
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetConnectionsAsync();

        /// <summary>
        /// Gets all connections that match the filter
        /// </summary>
        /// <param name="filter">The filter to apply on the data set</param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetConnectionsAsync(Expression<Func<T, bool>> filter);

        /// <summary>
        /// Adds a connection to the connection list
        /// </summary>
        /// <param name="connection">The connection to add</param>
        /// <returns></returns>
        Task AddAsync(T connection);

        /// <summary>
        /// Adds a connection to the connection list
        /// </summary>
        /// <param name="connection">The connection to add</param>
        /// <returns></returns>
        Task UpdateAsync(T connection);

        /// <summary>
        /// Removes the connection from the connection list
        /// </summary>
        /// <param name="connection">The connection to remove</param>
        /// <returns></returns>
        Task RemoveAsync(T connection);

        /// <summary>
        /// Clears the underlying data store
        /// </summary>
        /// <returns></returns>
        Task Clear();
    }
}