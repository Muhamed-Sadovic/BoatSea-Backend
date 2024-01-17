using AutoMapper;
using BoatSea.Data;
using BoatSea.DTOs;
using BoatSea.Interfaces;
using BoatSea.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;


namespace BoatSea.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UserController(IUserService userService, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _userService = userService;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }


        [HttpGet("getAllUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll() => Ok(_mapper.Map<List<UserResponseDTO>>(await _userService.GetAllUsersAsync()));


        [HttpDelete("deleteUser/{id}")] //da se uradu authorize
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

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromForm] RegisterUserRequestDTO request)
        {
            var userExist = await _userService.GetUserByEmail(request.Email);
            Stream fileStream = new FileStream(_webHostEnvironment.WebRootPath + "\\Images\\" + request.ImageName, FileMode.Create);
            if (userExist is not null)
                return BadRequest(new ErrorResponseDTO
                {
                    Message = "User already exist"
                });
            var user = _mapper.Map<User>(request);
            user.Password = _userService.HashPassword(request.Password);
            if (!Directory.Exists(_webHostEnvironment.WebRootPath + "\\Images"))
            {
                Directory.CreateDirectory(_webHostEnvironment.WebRootPath + "\\Images");
            }
            request.Image.CopyTo(fileStream);
            fileStream.Flush();

            await _userService.RegisterUser(user);

            var token = _userService.GenerateToken(user);

            return Ok(new AuthResponseDTO
            {
                Token = token,
                User = _mapper.Map<UserResponseDTO>(user)
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserRequestDTO request)
        {
            var userExist = await _userService.GetUserByEmail(request.Email);

            if (userExist is null)
                return NotFound(new ErrorResponseDTO
                {
                    Message = "User not registered in app"
                });

            if (userExist.Password != _userService.HashPassword(request.Password))
                return BadRequest(new ErrorResponseDTO
                {
                    Message = "Wrong password!"
                });

            var token = _userService.GenerateToken(userExist);

            return Ok(new AuthResponseDTO
            {
                Token = token,
                User = _mapper.Map<UserResponseDTO>(userExist)
            });
        }

    }
}
