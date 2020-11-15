using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dime.Logging;
using Dime.Repositories;

namespace Dime.WebSockets.Sql
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SqlConnectionTracker<T> : IConnectionTracker<T> where T : class, IWebSocketConnection, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlConnectionTracker{T}"/> class
        /// </summary>
        /// <param name="factory"></param>
        public SqlConnectionTracker(IRepositoryFactory factory, ILogger logger)
        {
            ConnectionRepositoryFactory = factory;
            Logger = logger;
        }

        private IRepositoryFactory ConnectionRepositoryFactory { get; }
        private ILogger Logger { get; }

        /// <summary>
        /// Adds connection to the data store if the unique connection ID doesn't exist yet
        /// </summary>
        /// <param name="key"></param>
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

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetConnectionsAsync()
        {
            try
            {
                IRepository<T> repository = ConnectionRepositoryFactory.Create<T>();
                return await repository.FindAllAsync(null).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.Exception(ex.Message, ex);
                return new List<T>();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        public async Task RemoveAsync(T key)
        {
            try
            {
                IRepository<T> repository = ConnectionRepositoryFactory.Create<T>();
                await repository.DeleteAsync(int.Parse(key.ToString())).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.Exception(ex.Message, ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
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