using Microsoft.AspNet.SignalR;
using System;

namespace JMangaHub.Models
{
    public class ChatHub : Hub
    {
        public void send(string name, string message)
        {
            Clients.All.broadcastMessage(name, message);
        }

        public void GetServerDateTime(string date)
        {
            Clients.Caller.DisplayDateTime(DateTime.Now);
        }
    }
}