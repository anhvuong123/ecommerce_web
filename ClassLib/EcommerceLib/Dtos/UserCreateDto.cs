// Dtos/CategoryDto.cs
using System.ComponentModel.DataAnnotations;

namespace EcommerceLib.Dtos
{
    public class UserCreateDto
    {
        [Required(ErrorMessage = "Username is required")]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [EmailAddress]  // Email validation
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Phone]  // Phone Validation
        [DataType(DataType.PhoneNumber)] 
        public string? PhoneNumber { get; set; }

        // [Required(ErrorMessage = "Role is required")]
        // [DataType(DataType.Text)]
        public string? Role { get; set; }
    }
}
