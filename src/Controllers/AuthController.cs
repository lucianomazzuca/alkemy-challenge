using AlkemyChallenge.Repositories;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using AlkemyChallenge.Models;
using AlkemyChallenge.DTOs.User;
using Microsoft.Extensions.Configuration;
using AlkemyChallenge.Services;

namespace AlkemyChallenge.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly TokenService _tokenService;

        public AuthController(UserRepository userRepository, IMapper mapper, TokenService tokenService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        // POST: api/Character
        [HttpPost("register")]
        public async Task<ActionResult> Register(UserCreateDto user)
        {
            var userExists = await _userRepository.GetByEmail(user.Email);
            if (userExists != null)
            {
                return BadRequest("Wrong Credentials");
            }

            var userDto = _mapper.Map<User>(user);
            userDto.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            await _userRepository.Add(userDto);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(User user)
        {
            var userFromDb = await _userRepository.GetByEmail(user.Email);
            if (userFromDb == null)
            {
                return BadRequest("Wrong Credentials");
            }

            var token = _tokenService.GenerateToken(user);

            return Ok(new { token });
        }
    }
}
