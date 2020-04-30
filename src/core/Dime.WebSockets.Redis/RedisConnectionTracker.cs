using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dime.WebSockets.Redis
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RedisConnectionTracker<T> : IConnectionTracker<T>
        where T : class, IWebSocketsConnection, new()
    {
        #region Constructor

        /// <summary>
        ///
        /// </summary>
        /// <param name="factory"></param>
        public RedisConnectionTracker(IRedisClientsManager redisManager)
        {
            RedisManager = redisManager;
        }

        #endregion Constructor

        #region Properties

        private IRedisClientsManager RedisManager { get; set; }
        public string Key { get; set; } = "dimescheduler:connections";

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets all connections
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetConnectionsAsync()
        {
            using (IRedisClient redisClient = RedisManager.GetClient())
            {
                IRedisTypedClient<T> redisTypedClient = redisClient.As<T>();
                IRedisSet<T> items = redisTypedClient.Sets[Key];
                return items.AsQueryable();
            }
        }

        /// <summary>
        /// Gets all connections that match the filter
        /// </summary>
        /// <param name="filter">The filter to apply on the data set</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetConnectionsAsync(Expression<Func<T, bool>> filter)
        {
            using (IRedisClient redisClient = RedisManager.GetClient())
            {
                IRedisTypedClient<T> redisTypedClient = redisClient.As<T>();
                IRedisSet<T> items = redisTypedClient.Sets[Key];
                return filter != null ? items.AsQueryable().Where(filter) : items.AsQueryable();
            }
        }

        /// <summary>
        /// Adds connection to the data store if the unique connection ID doesn't exist yet
        /// </summary>
        /// <param name="connection">The connection to add</param>
        public async Task AddAsync(T connection)
        {
            using (IRedisClient redisClient = RedisManager.GetClient())
            {
                IRedisTypedClient<T> redisTypedClient = redisClient.As<T>();
                IRedisSet<T> items = redisTypedClient.Sets[Key];

                if (items.Count(x => x.ConnectionId == connection.ConnectionId) == 0)
                    items.Add(connection);
            }
        }

        /// <summary>
        /// Removes the connection from the connection list
        /// </summary>
        /// <param name="connection">The connection to remove</param>
        public async Task RemoveAsync(T connection)
        {
            using (IRedisClient redisClient = RedisManager.GetClient())
            {
                IRedisTypedClient<T> redisTypedClient = redisClient.As<T>();
                IRedisSet<T> items = redisTypedClient.Sets[Key];
                items.Remove(connection);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task Clear()
        {
            using (IRedisClient redisClient = RedisManager.GetClient())
            {
                IRedisTypedClient<T> redisTypedClient = redisClient.As<T>();
                IRedisSet<T> items = redisTypedClient.Sets[Key];

                IEnumerable<T> connections = items.AsQueryable();
                foreach (T connection in connections)
                {
                    items.Remove(connection);
                }
            }
        }

        #endregion Methods
    }
}