using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JWTLibrary.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JWTLibrary
{
  public class JwtAuthManager
  {
    private readonly IConfiguration _configuration;
    private readonly JwtOptions _jwtOptions;

    public JwtAuthManager(IConfiguration configuration)
    {
      _configuration = configuration.GetSection(nameof(JwtOptions));
      
      // int.TryParse(_configuration["ExpirationHrs"], out int expirationTime);

      _jwtOptions = new JwtOptions
      {
        Key = _configuration["Key"],
        Issuer = _configuration["Issuer"],
        Audience = _configuration["Audience"],
        ExpirationHrs =  int.TryParse(_configuration["ExpirationHrs"], out int expirationTime) ? expirationTime: 1,
      };
    }

    /*
      ## Subject 
      We recive the claims outside of manager, because we want to allow user to pass it directly from auth system
    */

    public string GenerateToken(Claim[] claimsAuth)
    {
      var key = Encoding.ASCII.GetBytes(_jwtOptions.Key); // Codificación de la clave secreta en bytes

      // Descripción del token JWT con los claims (reclamaciones) del usuario
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claimsAuth),
        Expires = DateTime.UtcNow.AddHours(_jwtOptions.ExpirationHrs),

        // Don't forget to add Issuer and Audience
        Issuer = _jwtOptions.Issuer,
        Audience = _jwtOptions.Audience,

        // Sign token with secret key
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) 
      };

      var tokenHandler = new JwtSecurityTokenHandler();
      var token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token); // Parse token to string
    }
  }
}