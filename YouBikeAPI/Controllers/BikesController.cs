using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using YouBikeAPI.Dtos;
using YouBikeAPI.Helper;
using YouBikeAPI.Models;
using YouBikeAPI.Parameters;
using YouBikeAPI.Services;

namespace YouBikeAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BikesController : ControllerBase
	{
		private readonly IBikeRepository _bikeRepository;

		private readonly IBikeStationRepository _bikeStationRepository;

		private readonly IHttpContextAccessor _httpContextAccessor;

		private readonly IUrlHelper _urlHelper;

		private readonly IMapper _mapper;

		public BikesController(IBikeRepository bikeRepository, IBikeStationRepository bikeStationRepository, IHttpContextAccessor httpContextAccessor, IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor, IMapper mapper)
		{
			_bikeRepository = bikeRepository;
			_bikeStationRepository = bikeStationRepository;
			_httpContextAccessor = httpContextAccessor;
			_urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
			_mapper = mapper;
		}

		private string GenerateGetBikesURL(PageParameters pageParameters, ResourceUrlType resourceUrlType)
		{
			if (1 == 0)
			{
			}
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
			if (1 == 0)
			{
			}
			return result;
		}

		[HttpGet(Name = "GetBikes")]
		public async Task<IActionResult> GetBikes([FromQuery] PageParameters pageParameters)
		{
			PaginationList<Bike, BikeDto> bikes = await _bikeRepository.GetBikes(pageParameters.PageNum, pageParameters.PageSize);
			if (bikes == null)
			{
				return NotFound();
			}
			string previousPageLink = (bikes.HasPrevious ? GenerateGetBikesURL(pageParameters, ResourceUrlType.PreviousPage) : null);
			string nextPageLink = (bikes.HasNext ? GenerateGetBikesURL(pageParameters, ResourceUrlType.NextPage) : null);
			var paginationMetadata = new
			{
				previousPageLink = previousPageLink,
				nextPageLink = nextPageLink,
				totalCount = bikes.TotalCount,
				pageSize = bikes.PageSize,
				currentPage = bikes.CurrPage,
				totalPages = bikes.TotalPages
			};
			base.Response.Headers.Add("x-pagination", JsonConvert.SerializeObject(paginationMetadata));
			return Ok(bikes);
		}

		[HttpGet("{id}", Name = "GetBike")]
		public async Task<IActionResult> GetBike(int id)
		{
			BikeDto bike = await _bikeRepository.GetBikeForUser(id);
			if (bike == null)
			{
				return NotFound();
			}
			return Ok(bike);
		}

		[HttpPut("{id}")]
		[Authorize(Roles = "Admin")]
		[Authorize(AuthenticationSchemes = "Bearer")]
		public async Task<IActionResult> PutBike([FromRoute] int id, BikeForManipulationDto bike)
		{
			if (!(await _bikeRepository.BikeExists(id)))
			{
				return NotFound();
			}
			await _bikeRepository.UpdateBike(id, bike);
			if (await _bikeRepository.SaveAllAsync())
			{
				return NoContent();
			}
			return BadRequest("新增單車失敗");
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[Authorize(AuthenticationSchemes = "Bearer")]
		public async Task<ActionResult<BikeDto>> PostBike(BikeForManipulationDto bike)
		{
			if (bike == null)
			{
				return NotFound();
			}
			bool hasValue = bike.BikeStationId.HasValue;
			bool flag = hasValue;
			if (flag)
			{
				flag = await _bikeStationRepository.ValidateModel(this, bike.BikeStationId);
			}
			if (flag)
			{
				return ValidationProblem(base.ModelState);
			}
			Bike newBike = await _bikeRepository.CreateBike(bike);
			if (await _bikeRepository.SaveAllAsync())
			{
				BikeDto result = _mapper.Map<BikeDto>(newBike);
				return CreatedAtAction("GetBike", new
				{
					id = result.Id
				}, result);
			}
			return BadRequest("新增失敗");
		}

		public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
		{
			IOptions<ApiBehaviorOptions> options = base.HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
			return (ActionResult)options.Value.InvalidModelStateResponseFactory(base.ControllerContext);
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]
		[Authorize(AuthenticationSchemes = "Bearer")]
		public async Task<IActionResult> DeleteBike(int id)
		{
			if (!(await _bikeRepository.BikeExists(id)))
			{
				return NotFound();
			}
			await _bikeRepository.DeleteBike(id);
			if (await _bikeRepository.SaveAllAsync())
			{
				return NoContent();
			}
			return BadRequest("刪除失敗");
		}

		[HttpPost("transfer/{id}")]
		[Authorize(Roles = "Admin")]
		[Authorize(AuthenticationSchemes = "Bearer")]
		public async Task<IActionResult> TransferBike(int id, [FromQuery] Guid stationId)
		{
			if (!(await _bikeRepository.BikeExists(id)))
			{
				return NotFound();
			}
			await _bikeRepository.TransferBike(id, stationId);
			if (await _bikeRepository.SaveAllAsync())
			{
				return NoContent();
			}
			return BadRequest("移轉失敗");
		}

		[HttpPost("rent/{id}")]
		[Authorize(AuthenticationSchemes = "Bearer")]
		public async Task<IActionResult> RentABike(int id)
		{
			string userId = _httpContextAccessor.HttpContext!.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")!.Value;
			bool flag = await _bikeRepository.RentBike(id, userId);
			if (flag)
			{
				flag = await _bikeRepository.SaveAllAsync();
			}
			if (flag)
			{
				return NoContent();
			}
			return BadRequest("租借失敗");
		}

		[HttpPost("return/{id}")]
		[Authorize(AuthenticationSchemes = "Bearer")]
		public async Task<IActionResult> ReturnABike(int id, [FromQuery] Guid stationId)
		{
			string userId = _httpContextAccessor.HttpContext!.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")!.Value;
			(bool, string) res = await _bikeRepository.ReturnBike(id, stationId, userId);
			bool item = res.Item1;
			bool flag = item;
			if (flag)
			{
				flag = await _bikeRepository.SaveAllAsync();
			}
			IActionResult result;
			if (!flag)
			{
				IActionResult actionResult = BadRequest(res.Item2);
				result = actionResult;
			}
			else
			{
				IActionResult actionResult = Ok(res.Item2);
				result = actionResult;
			}
			return result;
		}
	}
}
