using System.ComponentModel.DataAnnotations;

namespace JwtAuthWeb.Models
{
  public class LoginDTO
  {
    [Required(ErrorMessage = $"username is required")]
    public string Username {get; set;}

    [Required(ErrorMessage = "password is required")]
    public string Password {get; set;}

  }
}