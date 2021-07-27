using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YouBikeAPI.Data;
using YouBikeAPI.Dtos;
using YouBikeAPI.Models;
using YouBikeAPI.Services;

namespace YouBikeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BikeStationsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IBikeStationRepository _bikeStationRepository;
        private readonly IMapper _mapper;

        public BikeStationsController(
            AppDbContext context,
            IBikeStationRepository bikeStationRepository,
            IMapper mapper)
        {
            _context = context;
            _bikeStationRepository = bikeStationRepository;
            _mapper = mapper;
        }

        // GET: api/BikeStations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BikeStation>>> GetBikeStations()
        {
            var bikeStations = await _bikeStationRepository.GetBikeStations();

            if (bikeStations == null)
            {
                return NotFound();
            }

            return Ok(bikeStations);
        }

        // GET: api/BikeStations/5
        [HttpGet("{id:Guid}", Name = "GetBikeStation")]
        public async Task<ActionResult<BikeStation>> GetBikeStation(Guid id)
        {
            var bikeStation = await _bikeStationRepository.GetBikeStationById(id);

            if (bikeStation == null)
            {
                return NotFound();
            }

            return Ok(bikeStation);
        }

        // PUT: api/BikeStations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutBikeStation(BikeStationForManipulationDto bikeStation)
        {
            if (!await _bikeStationRepository.BikeStationExists(bikeStation.Id))
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

        // POST: api/BikeStations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BikeStation>> PostBikeStation(BikeStationForCreationDto station)
        {
            if (station == null) return NotFound();

            var newStation = _bikeStationRepository.CreateBikeStation(station);

            if (await _bikeStationRepository.SaveAllAsync())
            {
                var result = _mapper.Map<BikeStation>(newStation);
                return CreatedAtAction("GetBikeStation", new { id = result.Id }, result);
            }

            return BadRequest("新增失敗");
        }

        // DELETE: api/BikeStations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBikeStation(Guid id)
        {
            if (!await _bikeStationRepository.BikeStationExists(id))
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
