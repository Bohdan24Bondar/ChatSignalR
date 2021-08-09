using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatDomain;
using System.Collections.Concurrent;

namespace ChatSignalR
{
    public class ChatHub : Hub
    {
        private static ConcurrentDictionary<string, User> ChatClients = new ConcurrentDictionary<string, User>();

        //public async Task SendMessage(ChatMessage message)
        //{
        //    await Clients.All.SendAsync("ReceiveMessage", message);
        //}

        public async Task SendMessage(string recepient, string sender, ChatMessage message)
        {
            User client = new User();
            ChatClients.TryGetValue(recepient, out client);

            //await Clients.Caller.SendAsync("ReceiveMessage", client.Name, message);

            await Clients.Client(client.ID).SendAsync("ReceiveMessage", client.Name, sender, message);
        }

        public List<User> Login(string name)
        {
            
            if (!ChatClients.ContainsKey(name))
            {
                Console.WriteLine($"++ {name} logged in");
                List<User> users = new List<User>(ChatClients.Values);
                User currentUser = new User { Name = name, ID = Context.ConnectionId };
                bool added = ChatClients.TryAdd(name, currentUser);

                if (!added)
                {
                    return null;
                }

                //Clients.CallerState.UserName = name;
                //Clients.Others.ParticipantLogin(newUser);
                Clients.Others.SendAsync("AddUser", currentUser);
                return users;
            }

            return null;
        }
    }
}
