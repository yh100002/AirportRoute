using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using LinkitAir.Service.Interface;
using LinkitAir.Service.Configurations;
using LinkitAir.Service.Model;
using LinkitAir.Service.Dto;
using LinkitAir.Service.ExceptionHandler;

namespace LinkitAir.API.Controllers
{
    //Attribute routing
    //string is case insensitive
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository authRepository;
        private readonly IConfiguration configuration;
        public AuthController(IAuthRepository authRepository, IConfiguration configuration)
        {
            this.authRepository = authRepository;
            this.configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(PassengerForRegisterDto passengerForRegisterDto)
        {
            passengerForRegisterDto.PassengerName = passengerForRegisterDto.PassengerName.ToLower();

            if (await this.authRepository.PassengerExists(passengerForRegisterDto.PassengerName))
                return StatusCode(400);
                //return BadRequest("Passenger Name already exists");                

            var passengerToCreate = new Passenger
            {
                PassengerName = passengerForRegisterDto.PassengerName
            };

            var createdPassenger = await this.authRepository.Register(passengerToCreate, passengerForRegisterDto.Password);

            return StatusCode(201); //Created Status
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login(PassengerForLoginDto passengerForLoginDto)
        {
            var passengerFromRepository = await this.authRepository.Login(passengerForLoginDto.PassengerName.ToLower(), passengerForLoginDto.Password);

            if (passengerFromRepository == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, passengerFromRepository.PassengerId.ToString()),
                new Claim(ClaimTypes.Name, passengerFromRepository.PassengerName)
            };

            //Passing the super key :) as byte array
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            //Reading super key this is just to show how it works
            var span = Int64.Parse(this.configuration.GetSection("AppSettings:TokenExpireSpan").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(span),//Valid for one day
                SigningCredentials = creds
            };

            /*
            On successful authentication the Authenticate method generates a JWT (JSON Web Token) using the JwtSecurityTokenHandler class 
            that generates a token that is digitally signed using a secret key stored in appsettings.json. 
            The JWT token is returned to the client application 
            which then must include it in the HTTP Authorization header of subsequent web api requests for authentication.
             */

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new {token = tokenHandler.WriteToken(token)});
        }
    }
}