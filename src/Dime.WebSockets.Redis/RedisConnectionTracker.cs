using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;

namespace Dime.WebSockets.Redis
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ExcludeFromCodeCoverage]
    public class RedisConnectionTracker<T> : IConnectionTracker<T>
        where T : class, IWebSocketConnection, new()
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="redisManager"></param>
        public RedisConnectionTracker(IRedisClientsManager redisManager)
        {
            RedisManager = redisManager;
        }

        private IRedisClientsManager RedisManager { get; }
        public string Key { get; set; } = "ws:connections";

        /// <summary>
        /// Gets all connections
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<T>> GetConnectionsAsync()
        {
            using IRedisClient redisClient = RedisManager.GetClient();
            IRedisTypedClient<T> redisTypedClient = redisClient.As<T>();
            IRedisSet<T> items = redisTypedClient.Sets[Key];
            return Task.FromResult(items.AsEnumerable());
        }

        /// <summary>
        /// Gets all connections that match the filter
        /// </summary>
        /// <param name="filter">The filter to apply on the data set</param>
        /// <returns></returns>
        public Task<IEnumerable<T>> GetConnectionsAsync(Expression<Func<T, bool>> filter)
        {
            using IRedisClient redisClient = RedisManager.GetClient();
            IRedisTypedClient<T> redisTypedClient = redisClient.As<T>();
            IRedisSet<T> items = redisTypedClient.Sets[Key];
            return Task.FromResult(filter != null ? items.AsQueryable().Where(filter).AsEnumerable() : items.AsEnumerable());
        }

        /// <summary>
        /// Adds connection to the data store if the unique connection ID doesn't exist yet
        /// </summary>
        /// <param name="connection">The connection to add</param>
        public Task AddAsync(T connection)
        {
            using (IRedisClient redisClient = RedisManager.GetClient())
            {
                IRedisTypedClient<T> redisTypedClient = redisClient.As<T>();
                IRedisSet<T> items = redisTypedClient.Sets[Key];

                if (items.Count(x => x.ConnectionId == connection.ConnectionId) == 0)
                    items.Add(connection);
            }

            return Task.FromResult(0);
        }

        /// <summary>
        /// Removes the connection from the connection list
        /// </summary>
        /// <param name="connection">The connection to remove</param>
        public Task RemoveAsync(T connection)
        {
            using (IRedisClient redisClient = RedisManager.GetClient())
            {
                IRedisTypedClient<T> redisTypedClient = redisClient.As<T>();
                IRedisSet<T> items = redisTypedClient.Sets[Key];
                items.Remove(connection);
            }

            return Task.FromResult(0);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Task Clear()
        {
            using (IRedisClient redisClient = RedisManager.GetClient())
            {
                IRedisTypedClient<T> redisTypedClient = redisClient.As<T>();
                IRedisSet<T> items = redisTypedClient.Sets[Key];

                IEnumerable<T> connections = items.AsQueryable();
                foreach (T connection in connections)
                    items.Remove(connection);
            }

            return Task.FromResult(0);
        }
    }
}