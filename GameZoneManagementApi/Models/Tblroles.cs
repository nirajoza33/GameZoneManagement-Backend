using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameZoneManagementApi.Models
{
    public class Tblroles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }

        [Column]
        [Required]
        [StringLength(50)]
        public string RoleName { get; set; } = null!;

        public ICollection<Tblusers> Tblusers { get; set; }


    }
}
