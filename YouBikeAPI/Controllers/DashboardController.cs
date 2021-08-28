using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YouBikeAPI.Models;
using YouBikeAPI.Services;

namespace YouBikeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDashboardInfo _dashboardInfo;

        public DashboardController(
            UserManager<ApplicationUser> userManager,
            IDashboardInfo dashboardInfo)
        {
            _userManager = userManager;
            _dashboardInfo = dashboardInfo;
        }

        [HttpGet("info")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetDashboardInfo()
        {
            var dashboardInfo = await _dashboardInfo.GetDashboardInfo();
            return Ok(dashboardInfo);
        }
    }
}