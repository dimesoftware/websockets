using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dime.Repositories;
using Microsoft.Extensions.Logging;

namespace Dime.WebSockets.Sql
{
    public class SqlConnectionTracker<T> : IConnectionTracker<T> where T : class, IWebSocketConnection, new()
    {
        public SqlConnectionTracker(IRepositoryFactory factory, ILogger logger)
        {
            ConnectionRepositoryFactory = factory;
            Logger = logger;
        }

        private IRepositoryFactory ConnectionRepositoryFactory { get; }
        private ILogger Logger { get; }

        public async Task<IEnumerable<T>> GetConnectionsAsync()
        {
            try
            {
                IRepository<T> repository = ConnectionRepositoryFactory.Create<T>();
                return await repository.FindAllAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return [];
            }
        }

        public async Task<IEnumerable<T>> GetConnectionsAsync(Expression<Func<T, bool>> filter)
        {
            try
            {
                IRepository<T> repository = ConnectionRepositoryFactory.Create<T>();
                return await repository.FindAllAsync(filter);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return [];
            }
        }

        public async Task AddAsync(T key)
        {
            try
            {
                IRepository<T> repository = ConnectionRepositoryFactory.Create<T>();
                IEnumerable<T> connections = await repository.FindAllAsync(x => x.ConnectionId == key.ConnectionId);
                if (!connections.Any())
                    await repository.CreateAsync(key);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
            }
        }

        public async Task UpdateAsync(T connection)
        {
            try
            {
                IRepository<T> repository = ConnectionRepositoryFactory.Create<T>();
                await repository.UpdateAsync(connection);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
            }
        }

        public async Task RemoveAsync(T key)
        {
            try
            {
                IRepository<T> repository = ConnectionRepositoryFactory.Create<T>();
                await repository.DeleteAsync(x => x.ConnectionId == key.ConnectionId);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
            }
        }

        public async Task Clear()
        {
            try
            {
                IRepository<T> repository = ConnectionRepositoryFactory.Create<T>();
                await repository.DeleteAsync(await GetConnectionsAsync());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
            }
        }
    }
}