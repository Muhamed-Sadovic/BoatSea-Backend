using AutoMapper;
using BoatSea.DTOs;
using BoatSea.Interfaces;
using BoatSea.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Stripe;

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

        [HttpPut("UpdateBoat/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromForm] BoatRequestDTO request, [FromRoute] int id)
        {
            var boat = await _boatService.GetByIdAsync(id);

            if (boat == null)
            {
                return NotFound();
            }
            Stream fileStream = new FileStream(_webHostEnvironment.WebRootPath + "\\Images\\" + request.ImageName, FileMode.Create);
            if (!Directory.Exists(_webHostEnvironment.WebRootPath + "\\Images"))
            {
                Directory.CreateDirectory(_webHostEnvironment.WebRootPath + "\\Images");
            }
            request.Image.CopyTo(fileStream);
            fileStream.Flush();

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

        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] CheckoutRequest request)
        {
            StripeConfiguration.ApiKey = "sk_test_51ObHPNFPaUUOIdBrtFfurojg4ymmPY7kBL6PKg0pguNpI2NWUzhLjLg8YMUviXcCp9pUca1v9I01qXUUzxtzUpiE000DTdOq3w";

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = request.Price * 100,
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "BoatSea",
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = request.SuccessUrl,
                CancelUrl = request.CancelUrl,
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options);

            return Ok(new { sessionId = session.Id });
        }

        [HttpPut("updateAvailable/{id}")]
        public async Task<IActionResult> UpdateAvailable([FromRoute] int id)
        {
            await _boatService.UpdateAvailable(id);
            return Ok();
        }

        [HttpPut("updateAvailableTrue/{id}")]
        public async Task<IActionResult> UpdateAvailableTrue([FromRoute] int id)
        {
            await _boatService.UpdateAvailableTrue(id);
            return Ok();
        }
        //[HttpGet("GetBoatsByType/{type}")]
        //public async Task<IActionResult> GetBoatByType([FromRoute] string type)
        //{
        //    var boats = await _boatService.GetByType(type);

        //    return Ok(_mapper.Map<List<BoatResponseDTO>>(boats));
        //}

    }
}
