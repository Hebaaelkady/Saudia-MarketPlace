using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kader.Middlewares.Hub
{
    public class OrderHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private static Dictionary<string, List<string>> userRoles = new Dictionary<string, List<string>>();
        private static HashSet<string> connectedUsers = new HashSet<string>(); // ✅ تتبع المستخدمين المتصلين فقط
        //private IKeys _keys;
        public OrderHub(/*IKeys keys*/)
        {
            //_keys = Keys;
        }

        public async Task JoinOrdersPage(string userId, List<string> roles)
        {
            if (!userRoles.ContainsKey(userId))
            {
                userRoles[userId] = roles;
            }

            connectedUsers.Add(userId); // ✅ إضافة المستخدم إلى قائمة المتصلين

            await Clients.Caller.SendAsync("ConnectedToOrders");
        }

        public async Task LeaveOrdersPage(string userId)
        {
            connectedUsers.Remove(userId); // ✅ إزالة المستخدم من قائمة المتصلين
        }

        public async Task NotifyNewOrder()
        {
            var authorizedUsers = connectedUsers.ToList();

            if (authorizedUsers.Any())
            {
                await Clients.Users(authorizedUsers).SendAsync("ReceiveNewOrderNotification");
            }
        }
    

}
}

