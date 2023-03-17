using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;

namespace Tachimi.Hubs
{
    public class ChatHub : Hub
    {
        private static ConcurrentDictionary<string, int> ConnectionsPerGroup = new ConcurrentDictionary<string, int>();
        private static ConcurrentDictionary<string, string> ConnectionIdToGroup = new ConcurrentDictionary<string, string>();

        public async Task SendMessage(string group, string user, string message)
        {
            var sanitizedMessage = SanitizeMessage(message);
            await Clients.Group(group).SendAsync("ReceiveMessage", user, sanitizedMessage);
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            ConnectionIdToGroup.TryAdd(Context.ConnectionId, groupName);
            ConnectionsPerGroup.AddOrUpdate(groupName, 1, (key, oldValue) => oldValue + 1);
            await UpdateConnections(groupName);
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (ConnectionIdToGroup.TryRemove(Context.ConnectionId, out string groupName))
            {
                ConnectionsPerGroup.AddOrUpdate(groupName, 0, (key, oldValue) => oldValue > 0 ? oldValue - 1 : 0);
                await UpdateConnections(groupName);
            }
            await base.OnDisconnectedAsync(exception);
        }

        private async Task UpdateConnections(string groupName)
        {
            if (ConnectionsPerGroup.TryGetValue(groupName, out int connections))
            {
                await Clients.Group(groupName).SendAsync("updateConnections", connections);
            }
        }

        private static string SanitizeMessage(string message)
        {
            // サニタイズ処理を実装
            return message;
        }
    }
}
