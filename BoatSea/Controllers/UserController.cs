﻿using AutoMapper;
using BoatSea.Data;
using BoatSea.DTOs;
using BoatSea.Interfaces;
using BoatSea.Models;
using Microsoft.AspNetCore.Authorization;
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
        /*[Authorize]*/ //samo autorizovani mogu da koriste ovu f-ju
        public async Task<IActionResult> GetAll() => Ok(_mapper.Map<List<UserResponseDTO>>(await _userService.GetAllUsersAsync()));


        [HttpDelete("{id}")]
        [Authorize]
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
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequestDTO request)
        {
            var userExist = await _userService.GetUserByEmail(request.Email);

            if (userExist is not null)
                return BadRequest(new ErrorResponseDTO
                {
                    Message = "User already exist"
                });

            var user = _mapper.Map<User>(request);
            user.Password = _userService.HashPassword(request.Password);

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
