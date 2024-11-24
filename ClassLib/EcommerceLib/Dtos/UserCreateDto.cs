// Dtos/CategoryDto.cs
using System.ComponentModel.DataAnnotations;

namespace EcommerceLib.Dtos
{
    public class UserCreateDto
    {
        [Required]
        [DataType(DataType.Password)]
        public string Username { get; set; }
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Role { get; set; }
    }
}
