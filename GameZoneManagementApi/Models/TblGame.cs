using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GameZoneManagementApi.Models
{
    public class TblGame
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GameId { get; set; }

        [Required, StringLength(255)]
        public string Title { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal Pricing { get; set; }

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public Tblusers? User { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public TblGameCategory? Category { get; set; }

        [Required]
        public bool Status { get; set; } = true;
    }
}




