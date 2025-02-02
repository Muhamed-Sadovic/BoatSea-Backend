using AutoMapper;
using BoatSea.Data;
using BoatSea.DTOs;
using BoatSea.Interfaces;
using BoatSea.Models;
using BoatSea.Services;
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

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id) => Ok(_mapper.Map<UserResponseDTO>(await _userService.GetByIdAsync(id)));

        [HttpPut("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserDTO request, [FromRoute] int id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
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

            _mapper.Map(request, user);

            await _userService.UpdateUserAsync(user);

            return Ok(_mapper.Map<UserResponseDTO>(user));
        }

        [HttpDelete("deleteUser/{id}")]
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
            if (userExist is not null)
                return BadRequest(new ErrorResponseDTO
                {
                    Message = "User already exist"
                });

            Stream fileStream = new FileStream(_webHostEnvironment.WebRootPath + "\\Images\\" + request.ImageName, FileMode.Create);
            if (!Directory.Exists(_webHostEnvironment.WebRootPath + "\\Images"))
            {
                Directory.CreateDirectory(_webHostEnvironment.WebRootPath + "\\Images");
            }
            request.Image.CopyTo(fileStream);
            fileStream.Flush();
            var user = _mapper.Map<User>(request);
            user.Password = _userService.HashPassword(request.Password);

            var verificationCode = _userService.CreateRandomToken();
            user.VerificationCode = verificationCode;

            if (request.Role == "Admin")
            {
                user.IsVerified = true;
            }

            await _userService.RegisterUser(user);
            await _userService.SendEmailAsync(request.Email, "Verification Code", $"Your verification code is: {verificationCode}");

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
        [HttpPost("verifyAccount/{id}")]
        public async Task<IActionResult> VerifyAccount([FromRoute] int id, [FromBody] VerificationRequest request)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (user.VerificationCode != request.Code)
            {
                return BadRequest("Invalid or expired verification code.");
            }
            var token = _userService.GenerateToken(user);

            user.IsVerified = true;
            await _userService.UpdateUserAsync(user);

            return Ok(new AuthResponseDTO
            {
                Token = token,
                User = _mapper.Map<UserResponseDTO>(user)
            });
        }   

        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var user = await _userService.GetUserByEmail(request.Email);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }
            var resetToken = _userService.GenerateResetToken(user);

            await _userService.SendPasswordResetEmail(user.Email, resetToken);

            return Ok(new { resetToken });
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var user = await _userService.GetUserByResetToken(request.Token);
            if (user == null)
            {
                return BadRequest("Invalid or expired password reset token.");
            }

            await _userService.ResetPassword(user, request.NewPassword);

            return Ok("Password successfully changed");
        }

        [HttpPost("getGrad")]

        public async Task<IActionResult> GetGrad([FromForm] string drzava)
        {
            var gr = await _userService.GetGrad(drzava);
            return Ok(gr);
        }
    }
}
