using AutoMapper;
using BoatSea.Interfaces;
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
    }
}
