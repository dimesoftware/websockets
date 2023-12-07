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
    [ExcludeFromCodeCoverage]
    public class RedisConnectionTracker<T> : IConnectionTracker<T>
        where T : class, IWebSocketConnection, new()
    {
        public RedisConnectionTracker(IRedisClientsManager redisManager)
        {
            RedisManager = redisManager;
        }

        public RedisConnectionTracker(IRedisClientsManager redisManager, string key)
            : this(redisManager)
        {
            Key = key;
        }

        private IRedisClientsManager RedisManager { get; }

        public string Key { get; set; } = "connections";

        public Task<IEnumerable<T>> GetConnectionsAsync()
        {
            using IRedisClient redisClient = RedisManager.GetClient();
            IRedisTypedClient<T> redisTypedClient = redisClient.As<T>();
            IRedisSortedSet<T> items = redisTypedClient.SortedSets[Key];
            return Task.FromResult(items.AsEnumerable());
        }

        public Task<IEnumerable<T>> GetConnectionsAsync(Expression<Func<T, bool>> filter)
        {
            using IRedisClient redisClient = RedisManager.GetClient();
            IRedisTypedClient<T> redisTypedClient = redisClient.As<T>();
            IRedisSortedSet<T> items = redisTypedClient.SortedSets[Key];
            return Task.FromResult(filter != null ? items.AsQueryable().Where(filter).AsEnumerable() : items.AsEnumerable());
        }

        public Task AddAsync(T connection)
        {
            using (IRedisClient redisClient = RedisManager.GetClient())
            {
                IRedisTypedClient<T> redisTypedClient = redisClient.As<T>();
                IRedisSortedSet<T> items = redisTypedClient.SortedSets[Key];

                if (!items.Any(x => x.ConnectionId == connection.ConnectionId))
                {
                    double score = new DateTimeOffset(connection.CreatedOn).ToUnixTimeSeconds();
                    items.Add(connection, score);
                }
            }

            return Task.FromResult(0);
        }

        public Task RemoveAsync(T connection)
        {
            using (IRedisClient redisClient = RedisManager.GetClient())
            {
                IRedisTypedClient<T> redisTypedClient = redisClient.As<T>();
                IRedisSortedSet<T> items = redisTypedClient.SortedSets[Key];

                double score = new DateTimeOffset(connection.CreatedOn).ToUnixTimeSeconds();
                items.RemoveRangeByScore(score - 1, score + 1);
            }

            return Task.FromResult(0);
        }


        public Task Clear()
        {
            using (IRedisClient redisClient = RedisManager.GetClient())
            {
                IRedisTypedClient<T> redisTypedClient = redisClient.As<T>();
                T[] keys = redisTypedClient.SearchKeys("*:" + Key);
                foreach (T key in keys)
                    redisTypedClient.Delete(key);
            }

            return Task.FromResult(0);
        }

        public async Task UpdateAsync(T connection)
        {
            using IRedisClient redisClient = RedisManager.GetClient();
            IRedisTypedClient<T> redisTypedClient = redisClient.As<T>();
            IRedisSortedSet<T> items = redisTypedClient.SortedSets[Key];

            // TODO
        }
    }
}