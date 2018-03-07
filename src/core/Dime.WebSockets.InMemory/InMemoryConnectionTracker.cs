using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dime.WebSockets.InMemory
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InMemoryConnectionTracker<T> : IConnectionTracker<T> where T : IWebSocketsConnection, new()
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public InMemoryConnectionTracker()
        {
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetConnectionsAsync()
        {
            await Task.FromResult(0);
            return Connections<T>.Instance().Users.Select(x => x.Value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetConnectionsAsync(Expression<Func<T, bool>> filter)
        {
            await Task.FromResult(0);
            return Connections<T>.Instance().Users.Select(x => x.Value).Where(x => filter.Compile().Invoke(x));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <param name="connection"></param>
        public async Task AddAsync(T connection)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            Connections<T>.Instance().Users.AddOrUpdate(connection.ConnectionId, connection, (key, old) => { return connection; });
            await Task.FromResult(0);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <param name="connection"></param>
        public async Task RemoveAsync(T connection)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            T metadata;
            if (Connections<T>.Instance().Users.TryRemove(connection.ConnectionId, out metadata))
            {
            }

            await Task.FromResult(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task Clear()
        {
            Connections<T>.Instance().Users.Clear();
            await Task.FromResult(0);
        }

        #endregion Methods
    }
}