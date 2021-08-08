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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using YouBikeAPI.Data;
using YouBikeAPI.Dtos;
using YouBikeAPI.Models;

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

		private readonly AppDbContext _context;

		private readonly IMapper _mapper;

		public UsersController(IConfiguration configuration, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor, AppDbContext context, IMapper mapper)
		{
			_configuration = configuration;
			_userManager = userManager;
			_signInManager = signInManager;
			_httpContextAccessor = httpContextAccessor;
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
			return Ok(tokenStr);
		}

		[AllowAnonymous]
		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
		{
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
			return Ok();
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
			ApplicationUser user = await _userManager.FindByIdAsync(userId);
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

		[HttpGet("history")]
		[Authorize(AuthenticationSchemes = "Bearer")]
		public async Task<IActionResult> GetHistoryRoutes()
		{
			string userId = _httpContextAccessor.HttpContext!.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")!.Value;
			if (userId == string.Empty)
			{
				return BadRequest("error!");
			}
			if (await _userManager.FindByIdAsync(userId) == null)
			{
				return BadRequest("error!");
			}
			List<HistoryRoute> histories = await _context.HistoryRoutes.Where((HistoryRoute h) => h.ApplicationUserId == userId).Include((HistoryRoute h) => h.Bike).ToListAsync();
			return Ok(_mapper.Map<IEnumerable<HistoryRouteDto>>(histories));
		}
	}
}
