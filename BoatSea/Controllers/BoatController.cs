using AutoMapper;
using BoatSea.DTOs;
using BoatSea.Interfaces;
using BoatSea.Models;
using Microsoft.AspNetCore.Mvc;

namespace BoatSea.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoatController : Controller
    {
        private readonly IBoatService _boatService;
        private readonly IMapper _mapper;
        public BoatController(IBoatService boatService, IMapper mapper)
        {
            _boatService = boatService;
            _mapper = mapper;
        }

        [HttpGet] //Svi brodovi
        public async Task<IActionResult> GetAll() => Ok(_mapper.Map<List<BoatDTO>>(await _boatService.GetAllBoatsAsync()));


        [HttpGet("{id}")] //brod po id-u
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var boat = await _boatService.GetByIdAsync(id);
            if (boat == null) return NotFound(new ErrorResponseDTO
            {
                Message = $"Boat with id={id} not found"
            });

            return Ok(_mapper.Map<BoatDTO>(boat));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BoatDTO boat)
        {
            var item = _mapper.Map<Boat>(boat);
            await _boatService.CreateBoat(item);

            return Created("http://localhost:7000/api/Boats", _mapper.Map<BoatDTO>(item));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] BoatDTO request, [FromRoute] int id)
        {
            var boat = await _boatService.GetByIdAsync(id);

            if (boat == null)
            {
                return NotFound();
            }

            _mapper.Map(request, boat);

            await _boatService.UpdateBoatAsync(boat);

            return Ok(_mapper.Map<BoatDTO>(boat));
        }

        [HttpDelete("{id}")]


        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var boat = await _boatService.GetByIdAsync(id);
            if (boat == null) return NotFound(new ErrorResponseDTO
            {
                Message = $"Boat with id={id} not found"
            });

            await _boatService.DeleteBoatAsync(boat);

            return NoContent();
        }

        [HttpGet("/Available")]

        public async Task<IActionResult> AvailableBoats()
        {
            return Ok(_mapper.Map<List<BoatDTO>>(await _boatService.GetByAvailable()));
        }

        [HttpPost("/Tip/{type}")]
        public async Task<IActionResult> GetByType([FromRoute] string type)
        {
            var boats = await _boatService.GetByType(type);

            if (boats.Count == 0) return NotFound(new ErrorResponseDTO
            {
                Message = $"Boats with type {type} not found"
            });

            return Ok(_mapper.Map<List<BoatDTO>>(boats));
        }

    }
}
