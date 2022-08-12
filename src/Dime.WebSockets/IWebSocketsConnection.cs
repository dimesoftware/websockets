using System;

namespace Dime.WebSockets
{
    public interface IWebSocketConnection
    {
        string ConnectionId { get; set; }

        int UserId { get; set; }

        string TimeZone { get; set; }

        string TenantId { get; set; }

        DateTime CreatedOn { get; set; }
    }
}