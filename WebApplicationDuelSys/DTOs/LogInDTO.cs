using System.ComponentModel.DataAnnotations;

namespace WebApplicationDuelSys.DTOs
{
    public class LogInDTO
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
