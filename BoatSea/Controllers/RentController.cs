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

    }
}
