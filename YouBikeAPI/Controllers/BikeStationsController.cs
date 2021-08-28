using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;
using YouBikeAPI.Data;
using YouBikeAPI.Dtos;
using YouBikeAPI.Helper;
using YouBikeAPI.Models;
using YouBikeAPI.Parameters;
using YouBikeAPI.Services;

namespace YouBikeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BikeStationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly IBikeStationRepository _bikeStationRepository;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IUrlHelper _urlHelper;

        private readonly IMapper _mapper;

        public BikeStationsController(AppDbContext context, IBikeStationRepository bikeStationRepository, IHttpContextAccessor httpContextAccessor, IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor, IMapper mapper)
        {
            _context = context;
            _bikeStationRepository = bikeStationRepository;
            _httpContextAccessor = httpContextAccessor;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            _mapper = mapper;
        }

        private string GenerateGetBikeStationsURL(PageParameters pageParameters, ResourceUrlType resourceUrlType)
        {
            if (1 == 0)
            {
            }
            string result = resourceUrlType switch
            {
                ResourceUrlType.PreviousPage => _urlHelper.Link("GetBikeStations", new
                {
                    pageNumber = pageParameters.PageNum - 1,
                    pageSize = pageParameters.PageSize
                }),
                ResourceUrlType.NextPage => _urlHelper.Link("GetBikeStations", new
                {
                    pageNumber = pageParameters.PageNum + 1,
                    pageSize = pageParameters.PageSize
                }),
                _ => _urlHelper.Link("GetBikeStations", new
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

        [HttpGet(Name = "GetBikeStations")]
        public async Task<IActionResult> GetBikeStations([FromQuery] PageParameters pageParameters, [FromQuery] string filter)
        {
            PaginationList<BikeStation, BikeStationDto> bikeStations = await _bikeStationRepository.GetBikeStations(pageParameters.PageNum, pageParameters.PageSize, filter);
            if (bikeStations == null)
            {
                return NotFound();
            }
            string previousPageLink = (bikeStations.HasPrevious ? GenerateGetBikeStationsURL(pageParameters, ResourceUrlType.PreviousPage) : null);
            string nextPageLink = (bikeStations.HasNext ? GenerateGetBikeStationsURL(pageParameters, ResourceUrlType.NextPage) : null);
            var paginationMetadata = new
            {
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink,
                totalCount = bikeStations.TotalCount,
                pageSize = bikeStations.PageSize,
                currentPage = bikeStations.CurrPage,
                totalPages = bikeStations.TotalPages
            };
            base.Response.Headers.Add("x-pagination", JsonConvert.SerializeObject(paginationMetadata));
            return Ok(bikeStations);
        }

        [HttpGet("{id}", Name = "GetBikeStation")]
        public async Task<IActionResult> GetBikeStation(Guid id)
        {
            BikeStationDto bikeStation = await _bikeStationRepository.GetBikeStationByIdDto(id);
            if (bikeStation == null)
            {
                return NotFound();
            }
            return Ok(bikeStation);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PutBikeStation(BikeStationForManipulationDto bikeStation)
        {
            if (!(await _bikeStationRepository.BikeStationExists(bikeStation.Id)))
            {
                return NotFound();
            }
            await _bikeStationRepository.UpdateBikeStation(bikeStation);
            if (await _bikeStationRepository.SaveAllAsync())
            {
                return NoContent();
            }
            return BadRequest("修改失敗");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<BikeStation>> PostBikeStation(BikeStationForCreationDto station)
        {
            if (station == null)
            {
                return NotFound();
            }
            BikeStation newStation = _bikeStationRepository.CreateBikeStation(station);
            if (await _bikeStationRepository.SaveAllAsync())
            {
                BikeStation result = _mapper.Map<BikeStation>(newStation);
                return CreatedAtAction("GetBikeStation", new
                {
                    id = result.Id
                }, result);
            }
            return BadRequest("新增失敗");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteBikeStation(Guid id)
        {
            if (!(await _bikeStationRepository.BikeStationExists(id)))
            {
                return NotFound();
            }
            await _bikeStationRepository.DeleteBikeStation(id);
            if (await _bikeStationRepository.SaveAllAsync())
            {
                return NoContent();
            }
            return BadRequest("新增失敗");
        }
    }
}
