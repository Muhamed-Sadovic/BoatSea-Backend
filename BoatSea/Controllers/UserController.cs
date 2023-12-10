using AutoMapper;
using BoatSea.Data;
using BoatSea.DTOs;
using BoatSea.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoatSea.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(_mapper.Map<List<UserDTO>>(await _userService.GetAllUsersAsync()));


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound(new ErrorResponseDTO
            {
                Message = $"Todo with id={id} not found"
            });

            await _userService.DeleteUser(user);
            return NoContent();
        }

    }
}
