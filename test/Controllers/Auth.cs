using System.Security.Claims;
using JWTLibrary;
using JwtAuthWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthWeb.Controllers
{
  [Route("api/auth")]
  [ApiController]
  public class AuthController: ControllerBase
  {
    private readonly JwtAuthManager _jwtAuthManager;
    public AuthController (IConfiguration configuration)
    {
      _jwtAuthManager = new JwtAuthManager(configuration);
    }

    [HttpPost]
    public string Login ([FromBody] LoginDTO login)
    {
      int userId = 2;
      var claims = new Claim[]
      {
        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
        new Claim(ClaimTypes.Role, "Admin")
      };

      return _jwtAuthManager.GenerateToken(claims);
    }
  }
}