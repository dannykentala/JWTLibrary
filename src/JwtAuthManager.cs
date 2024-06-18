using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace JWTLibrary
{
  public class JwtAuthManager
  {
    private readonly string _key; // Clave secreta utilizada para firmar y validar el token

    public JwtAuthManager(string key)
    {
      _key = key; // Inicialización de la clave secreta en el constructor
    }

    public string GenerateToken(string userId, string email)
    {
      var tokenHandler = new JwtSecurityTokenHandler(); // Instancia del manejador de tokens JWT
      var key = Encoding.ASCII.GetBytes(_key); // Codificación de la clave secreta en bytes

      // Descripción del token JWT con los claims (reclamaciones) del usuario
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId), // Claim del ID del usuario
            new Claim(ClaimTypes.Email, email) // Claim del email del usuario
        }),
        Expires = DateTime.UtcNow.AddHours(1), // Tiempo de expiración del token (1 hora desde ahora)
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) // Firmado del token con la clave secreta
      };

      var token = tokenHandler.CreateToken(tokenDescriptor); // Creación del token
      return tokenHandler.WriteToken(token); // Escritura del token como una cadena
    }

    // public ClaimsPrincipal ValidateToken(string token)
    // {
    //   var tokenHandler = new JwtSecurityTokenHandler(); // Instancia del manejador de tokens JWT
    //   var key = Encoding.ASCII.GetBytes(_key); // Codificación de la clave secreta en bytes

    //   try
    //   {
    //     // Validación del token JWT
    //     var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
    //     {
    //       ValidateIssuerSigningKey = true, // Validar la firma del emisor
    //       IssuerSigningKey = new SymmetricSecurityKey(key), // Clave para validar la firma
    //       ValidateIssuer = false, // No validar el emisor (issuer)
    //       ValidateAudience = false, // No validar el receptor (audience)
    //       ClockSkew = TimeSpan.Zero // Sin margen de desfase en el tiempo
    //     }, out _);

    //     return claimsPrincipal; // Devuelve los claims del usuario validados
    //   }
    //   catch
    //   {
    //     // Manejo de errores de validación de tokens apropiadamente en tu aplicación
    //     throw new SecurityTokenException("Token validation failed."); // Lanza una excepción en caso de error de validación
    //   }
    // }
  }
}