using System.ComponentModel.DataAnnotations;

namespace GameZoneManagementApi.DTOs
{
    public class VerifyCaptchaDto
    {
        [Required]
        public string Captcha { get; set; }
    }
}
