using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dime.Logging;
using Dime.Repositories;

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

        public async Task AddAsync(T key)
        {
            try
            {
                IRepository<T> repository = ConnectionRepositoryFactory.Create<T>();
                IEnumerable<T> connections = await repository.FindAllAsync(x => x.ConnectionId == key.ConnectionId).ConfigureAwait(false);
                if (!connections.Any())
                    await repository.CreateAsync(key).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<T>> GetConnectionsAsync()
        {
            try
            {
                IRepository<T> repository = ConnectionRepositoryFactory.Create<T>();
                return await repository.FindAllAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.Exception(ex.Message, ex);
                return new List<T>();
            }
        }

        public async Task<IEnumerable<T>> GetConnectionsAsync(Expression<Func<T, bool>> filter)
        {
            try
            {
                IRepository<T> repository = ConnectionRepositoryFactory.Create<T>();
                return await repository.FindAllAsync(filter).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.Exception(ex.Message, ex);
                return new List<T>();
            }
        }

        public async Task RemoveAsync(T key)
        {
            try
            {
                IRepository<T> repository = ConnectionRepositoryFactory.Create<T>();
                await repository.DeleteAsync(x => x.ConnectionId == key.ConnectionId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.Exception(ex.Message, ex);
            }
        }

        public async Task Clear()
        {
            try
            {
                IRepository<T> repository = ConnectionRepositoryFactory.Create<T>();
                await repository.DeleteAsync(await GetConnectionsAsync().ConfigureAwait(false)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.Exception(ex.Message, ex);
            }
        }
    }
}