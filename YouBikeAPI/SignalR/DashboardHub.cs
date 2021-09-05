using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using YouBikeAPI.Extensions;
using YouBikeAPI.Models;
using YouBikeAPI.Services;

namespace YouBikeAPI.SignalR
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class DashboardHub : Hub
    {
        private readonly IDashboardInfo _dashBoardInfo;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardHub(IDashboardInfo dashBoardInfo,
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _dashBoardInfo = dashBoardInfo;
        }

        public override async Task OnConnectedAsync()
        {
            var user = await _userManager.FindByIdAsync(Context.User.GetUserId());
            var admin = await _userManager.FindByNameAsync("admin@xx.com");

            var info = await _dashBoardInfo.GetDashboardInfo();

            await Clients.Caller.SendAsync("GetDashboard", info);
        }

        public async Task UpdateDashboard()
        {
            var user = await _userManager.FindByIdAsync(Context.User.GetUserId());
            var admin = await _userManager.FindByNameAsync("admin@xx.com");

            var info = await _dashBoardInfo.GetDashboardInfo();

            await Clients.User(admin.Id).SendAsync("UpdateDashboard", info, user.UserName);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}