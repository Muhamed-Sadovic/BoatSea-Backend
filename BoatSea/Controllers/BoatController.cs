using AutoMapper;
using BoatSea.DTOs;
using BoatSea.Interfaces;
using BoatSea.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace BoatSea.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoatController : Controller
    {
        private readonly IBoatService _boatService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BoatController(IBoatService boatService, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _boatService = boatService;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("GetAllBoats")] //Svi brodovi
        public async Task<IActionResult> GetAll() => Ok(_mapper.Map<List<BoatResponseDTO>>(await _boatService.GetAllBoatsAsync()));


        [HttpGet("GetBoatById/{id}")] //brod po id-u
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var boat = await _boatService.GetByIdAsync(id);
            if (boat == null) return NotFound(new ErrorResponseDTO
            {
                Message = $"Boat with id={id} not found"
            });

            return Ok(_mapper.Map<BoatResponseDTO>(boat));
        }

        [HttpPost("CreateBoat")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] BoatRequestDTO request)
        {
            var boat = _mapper.Map<Boat>(request);
            Stream fileStream = new FileStream(_webHostEnvironment.WebRootPath + "\\Images\\" + request.ImageName, FileMode.Create);
            if (!Directory.Exists(_webHostEnvironment.WebRootPath + "\\Images"))
            {
                Directory.CreateDirectory(_webHostEnvironment.WebRootPath + "\\Images");
            }
            request.Image.CopyTo(fileStream);
            fileStream.Flush();
            await _boatService.CreateBoat(boat);

            return Ok(_mapper.Map<BoatResponseDTO>(boat));

            //return Created("http://localhost:7087/api/Boat/createBoat", _mapper.Map<BoatRequestDTO>(boat));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] BoatRequestDTO request, [FromRoute] int id)
        {
            var boat = await _boatService.GetByIdAsync(id);

            if (boat == null)
            {
                return NotFound();
            }

            _mapper.Map(request, boat);

            await _boatService.UpdateBoatAsync(boat);

            return Ok(_mapper.Map<BoatResponseDTO>(boat));
        }

        [HttpDelete("DeleteBoat/{id}")]
        [Authorize(Roles = "Admin")]
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
            return Ok(_mapper.Map<List<BoatRequestDTO>>(await _boatService.GetByAvailable()));
        }

        [HttpPost("/Tip/{type}")]
        public async Task<IActionResult> GetByType([FromRoute] string type)
        {
            var boats = await _boatService.GetByType(type);

            if (boats.Count == 0) return NotFound(new ErrorResponseDTO
            {
                Message = $"Boats with type {type} not found"
            });

            return Ok(_mapper.Map<List<BoatRequestDTO>>(boats));
        }

    }
}
