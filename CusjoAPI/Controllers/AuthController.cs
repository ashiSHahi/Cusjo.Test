using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CusjoAPI.Data;
using CusjoAPI.Interfaces;
using CusjoAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using static CusjoAPI.Enumerators.Enumerators;

namespace CusjoAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationDto dto)
        {
            dto.UserName = dto.UserName.ToLower();

            if (await _authRepository.UserExists(dto.UserName))
                return BadRequest("Username already exists");

            if (await _authRepository.UserNameExists(dto.Email))
                return BadRequest("Email already exists");
            var userToCreate = new User
            {
                Username = dto.UserName,
                Email = dto.Email
            };

            var createdUser = await _authRepository.Register(userToCreate, dto.Password);

            return Ok(201);
        }

        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            var userFromRepo = await _authRepository.Login(dto.UserName.ToLower(), dto.Password);

            if (userFromRepo == null)
                return Unauthorized();

            var claims =  new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Username),
                new Claim(ClaimTypes.Name, userFromRepo.Username),
                new Claim(ClaimTypes.Email, userFromRepo.Email),
            };
            foreach(var claim in userFromRepo.Permissions)
            {
                claims.Add(new Claim(claim.Entities.Name, claim.HasAccess.ToString()));
            }


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}