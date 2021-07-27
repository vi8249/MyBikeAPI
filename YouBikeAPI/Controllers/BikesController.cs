using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using YouBikeAPI.Data;
using YouBikeAPI.Dtos;
using YouBikeAPI.Extensions;
using YouBikeAPI.Models;
using YouBikeAPI.Services;

namespace YouBikeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BikesController : ControllerBase
    {
        private readonly IBikeRepository _bikeRepository;
        private readonly IBikeStationRepository _bikeStationRepository;
        private readonly IMapper _mapper;

        public BikesController(
            IBikeRepository bikeRepository,
            IBikeStationRepository bikeStationRepository,
            IMapper mapper)
        {
            _bikeRepository = bikeRepository;
            _bikeStationRepository = bikeStationRepository;
            _mapper = mapper;
        }

        // GET: api/Bikes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bike>>> GetBikes()
        {
            var bikes = await _bikeRepository.GetBikes();
            return Ok(bikes);
        }

        // GET: api/Bikes/5
        [HttpGet("{id}", Name = "GetBike")]
        public async Task<ActionResult<Bike>> GetBike(int id)
        {
            var bike = await _bikeRepository.GetBikeForUser(id);

            if (bike == null)
            {
                return NotFound();
            }

            return Ok(bike);
        }

        // PUT: api/Bikes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBike([FromRoute] int id, BikeForManipulationDto bike)
        {
            if (!await _bikeRepository.BikeExists(id))
            {
                return NotFound();
            }

            await _bikeRepository.UpdateBike(id, bike);

            if (await _bikeRepository.SaveAllAsync()) return NoContent();

            return BadRequest("新增單車失敗");
        }

        // POST: api/Bikes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult<BikeDto>> PostBike(BikeForManipulationDto bike)
        {
            if (bike == null) return NotFound();

            if (bike.BikeStationId != null && await _bikeStationRepository.ValidateModel(this, bike.BikeStationId))
            {
                return ValidationProblem(ModelState);
            }

            var newBike = await _bikeRepository.CreateBike(bike);

            if (await _bikeRepository.SaveAllAsync())
            {
                var result = _mapper.Map<BikeDto>(newBike);
                return CreatedAtAction("GetBike", new { id = result.Id }, result);
            }

           return BadRequest("新增失敗");
        }

        public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }

        // DELETE: api/Bikes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBike(int id)
        {
            if (!await _bikeRepository.BikeExists(id))
            {
                return NotFound();
            }

            await _bikeRepository.DeleteBike(id);

            if (await _bikeRepository.SaveAllAsync())
                return NoContent();

            return BadRequest("刪除失敗");
        }

        // DELETE: api/Bikes/transfer/5
        [HttpPost("transfer/{id}")]
        public async Task<IActionResult> TransferBike(int id, [FromQuery] Guid stationId)
        {
            if (!await _bikeRepository.BikeExists(id))
            {
                return NotFound();
            }

            await _bikeRepository.TransferBike(id, stationId);

            if (await _bikeRepository.SaveAllAsync())
                return NoContent();

            return BadRequest("刪除失敗");
        }
    }
}
