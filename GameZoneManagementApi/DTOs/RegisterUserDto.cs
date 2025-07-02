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

        public string Status { get; set; } = "Active"; // New field

        [Required]
        public int RoleId { get; set; }


        // if using google captcha then uncoment this field

        //[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        //public string CaptchaToken { get; set; }
    }
}
