using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace CodeWarriors.API.Hubs
{
    [HubName("friend")]
    public class FriendHub : Hub
    {
        public void Send() {
            Clients.All.acceptGreet("Successfull Web Socket");
        }
    }
}