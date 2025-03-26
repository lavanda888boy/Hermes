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
        [HttpGet]
        public IActionResult GetAdminJwtAccessToken()
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(DotNetEnv.Env.GetString("JWT_SECRET")));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
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
    }
}
