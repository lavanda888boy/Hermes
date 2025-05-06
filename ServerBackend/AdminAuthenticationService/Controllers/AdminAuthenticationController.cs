using AdminAuthenticationService.Models;
using AdminAuthenticationService.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AdminAuthenticationService.Controllers
{
    [ApiController]
    [Route("/")]
    public class AdminAuthenticationController : ControllerBase
    {
        private readonly IRepository<User> _userRepository;

        public AdminAuthenticationController(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> LoginAdminUser([FromBody] User user)
        {
            var existingUser = await _userRepository.GetByUserNameAsync(user.UserName);

            if (existingUser == null)
            {
                return BadRequest($"Admin user with the given username: {user.UserName} does not exist");
            }

            bool passwordVerified = BCrypt.Net.BCrypt.Verify(user.Password, existingUser.Password);

            if (!passwordVerified)
            {
                return BadRequest($"Admin user password is incorrect");
            }

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(DotNetEnv.Env.GetString("JWT_SECRET")));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var jwtAccessToken = new JwtSecurityToken(
                issuer: DotNetEnv.Env.GetString("JWT_ISSUER"),
                audience: DotNetEnv.Env.GetString("JWT_AUDIENCE"),
                claims: claims,
                expires: DateTime.Now.AddMinutes(DotNetEnv.Env.GetDouble("JWT_LIFETIME_MINUTES")),
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var encodedToken = tokenHandler.WriteToken(jwtAccessToken);

            return Ok(encodedToken);
        }

        [HttpPost("/register")]
        public async Task<IActionResult> RegisterAdminUser([FromBody] User user)
        {
            var existingUser = await _userRepository.GetByUserNameAsync(user.UserName);

            if (existingUser != null)
            {
                return BadRequest($"Admin user with the given username: {user.UserName} already exist");
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Password = hashedPassword;

            await _userRepository.AddAsync(user);

            return Ok(user.UserName);
        }
    }
}
