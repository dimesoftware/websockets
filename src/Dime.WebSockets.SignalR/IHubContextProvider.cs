using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Dime.WebSockets.SignalR
{
    /// <summary>
    /// Represents a SignalR Hub Context Provider
    /// </summary>
    public interface IHubContextProvider
    {
        /// <summary>
        /// Gets the context
        /// </summary>
        /// <typeparam name="T">The hub type</typeparam>
        /// <returns>The hub context</returns>
        IHubContext GetContext<T>() where T : IHub;
    }

}
