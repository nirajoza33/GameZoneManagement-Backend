using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GameZoneManagementApi.Models
{
    public class TblGameCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; } = null!;

        public string? Description { get; set; }

        public ICollection<TblGame>? Games { get; set; }
    }
}
