using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatDomain;

namespace ChatWPF
{
    public class SignalRChatService
    {
        private readonly HubConnection _connection;

        public event Action<string, string, ChatMessage> MessageRecived;

        public event Action<string> LoggedOut;

        public event Action<User> LoggedIn; 

        public SignalRChatService(HubConnection connection)
        {
            _connection = connection;

            _connection.On<string, string, ChatMessage>("ReceiveMessage", (name, sender, message) => MessageRecived?.Invoke(name, sender, message));

            _connection.On<User>("AddUser", (user) => LoggedIn?.Invoke(user));

            _connection.On<string>("RemoveUser", (name) => LoggedOut?.Invoke(name));
        }

        public async Task Connect()
        {
            await _connection.StartAsync();
        }

        public async Task SendMessage(string recepient, string sender, ChatMessage message)
        {
            await _connection.SendAsync("SendMessage", recepient, sender, message);
        }

        public async Task<List<User>> LoginAsync(string name)
        {
            return await _connection.InvokeAsync<List<User>>("Login", name);
        }

        public async Task LogoutAsync(string name)
        {
            await _connection.SendAsync("Logout", name);
        }

    }
}
