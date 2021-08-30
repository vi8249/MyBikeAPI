using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using YouBikeAPI.Data;
using YouBikeAPI.Dtos;
using YouBikeAPI.Helper;
using YouBikeAPI.Models;
using YouBikeAPI.Parameters;

namespace YouBikeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUrlHelper _urlHelper;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UsersController(
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IHttpContextAccessor httpContextAccessor,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor,
            AppDbContext context,
            IMapper mapper)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            _context = context;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!(await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, isPersistent: false, lockoutOnFailure: false)).Succeeded)
            {
                return BadRequest("login Failed!");
            }
            ApplicationUser user = await _userManager.FindByNameAsync(loginDto.Email);
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            string sigingAlgorithm = "HS256";
            List<Claim> claims = new List<Claim>
            {
                new Claim("sub", user.Id)
            };
            foreach (string role in await _userManager.GetRolesAsync(user))
            {
                Claim roleClaim = new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", role);
                claims.Add(roleClaim);
            }
            byte[] secretByte = Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]);
            SymmetricSecurityKey secretKey = new SymmetricSecurityKey(secretByte);
            JwtSecurityToken token = new JwtSecurityToken(signingCredentials: new SigningCredentials(secretKey, sigingAlgorithm), issuer: _configuration["Authentication:Issuer"], audience: _configuration["Authentication:Audience"], claims: claims, notBefore: DateTime.UtcNow, expires: DateTime.UtcNow.AddDays(1.0));
            string tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { email = user.Email, token = tokenStr, admin = admins?.Any(u => u.Id == user.Id) });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (registerDto.Email == null) return BadRequest("Email不得為空");
            if (registerDto.Password == null) return BadRequest("密碼不得為空");
            if (registerDto.Password != registerDto.ConfirmPassword)
                return BadRequest("密碼不一致");

            ApplicationUser user = new ApplicationUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                Money = new Money
                {
                    Value = registerDto.Money
                }
            };
            bool flag = !(await _userManager.CreateAsync(user, registerDto.Password)).Succeeded;
            bool flag2 = flag;
            if (flag2)
            {
                flag2 = await _context.SaveChangesAsync() > 0;
            }
            if (flag2)
            {
                return BadRequest("用戶創建失敗!");
            }

            //for login jwt
            List<Claim> claims = new List<Claim>
            {
                new Claim("sub", user.Id)
            };
            string sigingAlgorithm = "HS256";
            byte[] secretByte = Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]);
            SymmetricSecurityKey secretKey = new SymmetricSecurityKey(secretByte);
            JwtSecurityToken token = new JwtSecurityToken(signingCredentials: new SigningCredentials(secretKey, sigingAlgorithm), issuer: _configuration["Authentication:Issuer"], audience: _configuration["Authentication:Audience"], claims: claims, notBefore: DateTime.UtcNow, expires: DateTime.UtcNow.AddDays(1.0));
            string tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { email = user.Email, token = tokenStr });
        }

        [HttpPost("storage/{amount}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddValue([FromRoute] string amount)
        {
            string userId = _httpContextAccessor.HttpContext!.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")!.Value;
            if (userId == string.Empty)
            {
                return BadRequest("error!");
            }
            ApplicationUser user = await _userManager.Users
                .Include(u => u.Money)
                .SingleOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return BadRequest("error!");
            }
            if (user.Money == null)
            {
                user.Money = new Money
                {
                    Value = decimal.Parse(amount)
                };
            }
            else
            {
                user.Money.Value += decimal.Parse(amount);
            }
            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }
            return BadRequest("儲值失敗");
        }

        private string GenerateGetBikesURL(PageParameters pageParameters, ResourceUrlType resourceUrlType)
        {
            string result = resourceUrlType switch
            {
                ResourceUrlType.PreviousPage => _urlHelper.Link("GetBikes", new
                {
                    pageNumber = pageParameters.PageNum - 1,
                    pageSize = pageParameters.PageSize
                }),
                ResourceUrlType.NextPage => _urlHelper.Link("GetBikes", new
                {
                    pageNumber = pageParameters.PageNum + 1,
                    pageSize = pageParameters.PageSize
                }),
                _ => _urlHelper.Link("GetBikes", new
                {
                    pageNumber = pageParameters.PageNum,
                    pageSize = pageParameters.PageSize
                }),
            };
            return result;
        }

        [HttpGet("info")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetUserInfo([FromQuery] PageParameters pageParameters)
        {
            string userId = _httpContextAccessor.HttpContext!.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")!.Value;
            if (userId == null)
                return BadRequest("error!");

            var user = await _userManager.Users
                .Include(u => u.Money)
                .SingleOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return BadRequest("使用者不存在!");

            var bike = await _context.Bikes.SingleOrDefaultAsync(b => b.UserId == user.Id);

            var histories = _context.HistoryRoutes
                .Where((HistoryRoute h) => h.ApplicationUserId == userId)
                .OrderByDescending(h => h.CreationDate);

            var admin = await _userManager.GetUsersInRoleAsync("Admin");

            var userInfo = new
            {
                UserId = user.Id,
                Email = user.Email,
                Money = user?.Money.Value,
                Bike = bike?.Id,
                Usage = await _context.HistoryRoutes.Where(h => h.ApplicationUserId == userId).CountAsync(),
                HistoryRoute = await PaginationList<HistoryRoute, HistoryRouteDto>.CreateAsync(pageParameters.PageNum, pageParameters.PageSize, histories, _mapper),
                Admin = admin?.Any(u => u.Id == user.Id)
            };

            string previousPageLink = (userInfo.HistoryRoute.HasPrevious ? GenerateGetBikesURL(pageParameters, ResourceUrlType.PreviousPage) : null);
            string nextPageLink = (userInfo.HistoryRoute.HasNext ? GenerateGetBikesURL(pageParameters, ResourceUrlType.NextPage) : null);
            var paginationMetadata = new
            {
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink,
                totalCount = userInfo.HistoryRoute.TotalCount,
                pageSize = userInfo.HistoryRoute.PageSize,
                currentPage = userInfo.HistoryRoute.CurrPage,
                totalPages = userInfo.HistoryRoute.TotalPages
            };
            base.Response.Headers.Add("x-pagination", JsonConvert.SerializeObject(paginationMetadata));

            return Ok(userInfo);
        }
    }
}
