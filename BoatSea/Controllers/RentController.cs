using AutoMapper;
using BoatSea.DTOs;
using BoatSea.Interfaces;
using BoatSea.Models;
using BoatSea.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace BoatSea.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class RentController : Controller
    {
        private readonly IRentService _rentService;
        private readonly IMapper _mapper;
        public RentController(IRentService rentService, IMapper mapper) 
        {
            _rentService = rentService;
            _mapper = mapper;
        }

        [HttpPost("RentBoat")]
        public async Task<IActionResult> RentBoat([FromBody] RentRequestDTO request)
        {
            var rent = _mapper.Map<Rent>(request);

            await _rentService.RentABoat(rent);

            return Ok(_mapper.Map<RentResponseDTO>(rent));
        }

        [HttpGet("GetRentsByUser/{id}")]

        public async Task<IActionResult> GetRentsByUser([FromRoute] int id)
        {
            var rents = await _rentService.GetAllRentsByUser(id);
            return Ok(rents);
        }

        [HttpDelete("CancelRent/{id}")]
        public async Task<IActionResult> CancelRent([FromRoute] int id)
        {
            try
            {
                await _rentService.CancelRent(id);
                return Ok("Rent successfully cancelled.");
            }
            catch (Exception ex)
            {
                // Logujte ex za dalju analizu
                return StatusCode(500, "Internal Server Error");
            }
        }

    }
}
