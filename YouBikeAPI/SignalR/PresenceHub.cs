using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using YouBikeAPI.Extensions;
using YouBikeAPI.Models;

namespace YouBikeAPI.SignalR
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PresenceHub : Hub
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PresenceTracker _tracker;

        public PresenceHub(UserManager<ApplicationUser> userManager,
            PresenceTracker tracker)
        {
            _userManager = userManager;
            _tracker = tracker;
        }

        public override async Task OnConnectedAsync()
        {
            var user = await _userManager.FindByIdAsync(Context.User.GetUserId());

            await _tracker.UserConnected(user.UserName, Context.ConnectionId);
            await Clients.Others.SendAsync("UserOnline", user.UserName);

            var currentUsers = await _tracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);

            //Console.WriteLine("on " + Context.User.GetUserId());
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = await _userManager.FindByIdAsync(Context.User.GetUserId());

            await _tracker.UserDisconnected(user.UserName, Context.ConnectionId);
            await Clients.Others.SendAsync("UserOffline", user.UserName);

            var currentUsers = await _tracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);

            //Console.WriteLine("off " + Context.User.GetUserId());

            await base.OnDisconnectedAsync(exception);
        }
    }
}