using System;

namespace Dime.WebSockets
{
    /// <summary>
    ///
    /// </summary>
    public interface IWebSocketConnection
    {
        /// <summary>
        /// 
        /// </summary>
        string ConnectionId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string TimeZone { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string TenantId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        DateTime CreatedOn { get; set; }
    }
}