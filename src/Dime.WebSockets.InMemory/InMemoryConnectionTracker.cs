using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dime.WebSockets.InMemory
{
    public class InMemoryConnectionTracker<T> : IConnectionTracker<T>
        where T : IWebSocketConnection, new()
    {
        public InMemoryConnectionTracker(IKeyValueStore<T> store)
        {
            Store = store;
        }

        private IKeyValueStore<T> Store { get; }

        public Task<IEnumerable<T>> GetConnectionsAsync()
            => Task.FromResult(Store.Find());

        public Task<IEnumerable<T>> GetConnectionsAsync(Expression<Func<T, bool>> filter)
            => Task.FromResult(Store.Find().Where(x => filter.Compile().Invoke(x)));

        public async Task AddAsync(T connection)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            Store.Add(connection.ConnectionId, connection);
            await Task.FromResult(0);
        }

        public async Task UpdateAsync(T connection)
        {
            var item = Store.Find().FirstOrDefault(x => x.ConnectionId == connection.ConnectionId);
            if (item == null)
                await AddAsync(connection);
            else
                item = connection;

            throw new NotImplementedException();
        }

        public async Task RemoveAsync(T connection)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            Store.Remove(connection.ConnectionId);
            await Task.FromResult(0);
        }

        public async Task Clear()
        {
            Store.Purge();
            await Task.FromResult(0);
        }
    }
}