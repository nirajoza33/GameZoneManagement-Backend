using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameZoneManagementApi.Models
{
    public class Tblusers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [StringLength(255)]
        [Column]
        public string UserName { get; set; } = null!;

        [Required]
        [StringLength (255)]
        [Column]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength (255)]
        [Column]
        public string Password { get; set; } = null!;

        [Required]
        [StringLength (255)]
        [Column]
        public string phone { get; set; } = null!;

        [Column]
        public string? Bio {  get; set; }

        [Column]
        public int RoleId { get; set; }

        [Required]
        [StringLength(50)]
        [Column]
        public string Status { get; set; } = "Active"; // default value can be "Active"


        [ForeignKey("RoleId")]            
        public Tblroles Tblrole { get; set; }

    }
}
