using System.Security.Claims;
using System.Security.Cryptography;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieTheaterCore.Interfaces;

namespace AdminAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IRepository<User> _userRepository;
        private readonly TokenCreationService _tokenCreationService;

        public AuthController(IRepository<User> userRepository, TokenCreationService tokenCreationService)
        {
            _userRepository = userRepository;
            _tokenCreationService = tokenCreationService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDTO userDto)
        {
            var takenName = await _userRepository.ListAsync(u => u.Username == userDto.UserName);
            if (takenName.Count() > 0) return BadRequest("Username is unavailable");

            CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var newUser = new User
            {
                Username = userDto.UserName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await _userRepository.InsertAsync(newUser);
            return Ok(newUser);
        }

        [HttpPost("login")]
        public ActionResult<User> Login(UserDTO user)
        {
            var userFromDb = _userRepository.ListAsync(u => u.Username == user.UserName).Result.FirstOrDefault();

            if (userFromDb == null)
            {
                return Unauthorized();
            }

            if (!VerifyPasswordHash(user.Password, userFromDb.PasswordHash, userFromDb.PasswordSalt))
            {
                return Unauthorized();
            }

            var token = _tokenCreationService.CreateToken(userFromDb);
            return Ok(token);
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}