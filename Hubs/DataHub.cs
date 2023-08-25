using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace DotNetCoreSignalR.Hubs
{

    public class DataHub : Hub
    {
        private readonly IMemoryCache _memoryCache;
        public static int Count = 0;
        public DataHub(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public override async Task OnConnectedAsync()
        {
            //get the connection id
            string connectionId = Context.ConnectionId;
            Count++;

            await this.Clients.Caller.SendAsync("GetConnectionId", connectionId);
            await this.Clients.All.SendAsync("UpdateCount", Count);

            await base.OnConnectedAsync();

        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Count--;
            await this.Clients.All.SendAsync("UpdateCount", Count);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(Chat chat)
        {
            List<Chat> chats = _memoryCache.Get<List<Chat>>("GlobalChat");
            if (chats == null)
            {
                chats = new List<Chat>();
            }

            chats.Add(chat);

            _memoryCache.Set("GlobalChat", chats);
            await this.Clients.All.SendAsync("GlobalChat", chats);
        }

    }

    public class Chat
    {
        public string Username { get; set; }
        public string Message { get; set; }
    }
}
