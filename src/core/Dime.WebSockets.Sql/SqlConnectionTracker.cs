using Dime.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dime.WebSockets.Sql
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SqlConnectionTracker<T> : IConnectionTracker<T>
        where T : class, IWebSocketsConnection, new()
    {
        #region Constructor

        /// <summary>
        ///
        /// </summary>
        /// <param name="factory"></param>
        public SqlConnectionTracker(IMultiTenantRepositoryFactory factory)
        {
            this.ConnectionRepositoryFactory = factory;
        }

        #endregion Constructor

        #region Properties

        private IMultiTenantRepositoryFactory ConnectionRepositoryFactory { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Adds connection to the data store if the unique connection ID doesn't exist yet
        /// </summary>
        /// <param name="key"></param>
        /// <param name="connection"></param>
        /// <history>
        /// [HB] 31/12/2015 - Item existence check
        /// </history>
        public async Task AddAsync(T key)
        {
            IRepository<T> repository = this.ConnectionRepositoryFactory.Create<T>();
            IEnumerable<T> connections = await repository.FindAllAsync(x => x.ConnectionId == key.ConnectionId);
            if (connections.Count() == 0)
            {
                await repository.CreateAsync(key);
            }
            else
            {
                // Record already exists, do nothing
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <history>
        /// [HB] 31/12/2015 - Review
        /// </history>
        public async Task<IEnumerable<T>> GetConnectionsAsync()
        {
            IRepository<T> repository = this.ConnectionRepositoryFactory.Create<T>();
            return await repository.FindAllAsync(null);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        ///  <history>
        /// [HB] 31/12/2015 - Review
        /// </history>
        public async Task<IEnumerable<T>> GetConnectionsAsync(Expression<Func<T, bool>> filter)
        {
            IRepository<T> repository = this.ConnectionRepositoryFactory.Create<T>();
            return await repository.FindAllAsync(filter);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <param name="connection"></param>
        ///  <history>
        /// [HB] 31/12/2015 - Review
        /// </history>
        public async Task RemoveAsync(T key)
        {
            IRepository<T> repository = this.ConnectionRepositoryFactory.Create<T>();
            await repository.DeleteAsync(int.Parse(key.ToString()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task Clear()
        {
            IRepository<T> repository = this.ConnectionRepositoryFactory.Create<T>();
            await repository.DeleteAsync(await repository.FindAllAsync());
        }

        #endregion Methods
    }
}