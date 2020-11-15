using System.Threading.Tasks;

namespace Dime.WebSockets
{
    /// <summary>
    /// Defines the capabilities of a notifier
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBroadcaster<T>
    {
        /// <summary>
        /// Notifies the messages
        /// </summary>
        /// <returns>Instance of <see cref="Task"/></returns>
        Task Publish();

        /// <summary>
        /// Notifies the messages
        /// </summary>
        /// <returns>Instance of <see cref="Task"/></returns>
        Task<Task> PublishAsync();
    }
}