using System.ComponentModel.DataAnnotations;

namespace GameZoneManagementApi.DTOs
{
    public class RegisterUserDto
    {

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required,MinLength(6)]
        public string Password { get; set; }

        [Required,Phone]
        public string phone { get; set; }

        public string? Bio { get; set; }

        [Required]
        public int RoleId { get; set; }
    }
}
