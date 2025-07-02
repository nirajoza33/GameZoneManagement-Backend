//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace GameZoneManagementApi.Models
//{
//    public class TblOffer
//    {
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int OfferId { get; set; }

//        [Required, StringLength(255)]
//        public string Title { get; set; } = null!;

//        [Required]
//        public string Description { get; set; } = null!;

//        [Required, StringLength(50)]
//        public string DiscountType { get; set; } = null!; // "percentage" or "fixed"

//        [Required, Column(TypeName = "decimal(10,2)")]
//        public decimal DiscountValue { get; set; } // 50 for 50% or 1000 for ₹1000 off

//        [Required, Column(TypeName = "decimal(10,2)")]
//        public decimal OriginalPrice { get; set; }

//        [Required, Column(TypeName = "decimal(10,2)")]
//        public decimal DiscountedPrice { get; set; }

//        [Required, StringLength(100)]
//        public string Category { get; set; } = null!; // weekend, student, birthday, etc.

//        [Required]
//        public DateTime ValidFrom { get; set; }

//        [Required]
//        public DateTime ValidUntil { get; set; }

//        [Required]
//        public bool IsFeatured { get; set; } = false;

//        [Required]
//        public bool IsTrending { get; set; } = false;

//        [StringLength(500)]
//        public string? ImageUrl { get; set; }

//        [StringLength(50)]
//        public string? Icon { get; set; } // emoji or icon identifier

//        [StringLength(100)]
//        public string? GradientClass { get; set; } // CSS gradient class

//        [StringLength(20)]
//        public string? AccentColor { get; set; } // hex color code

//        public string? GamesIncluded { get; set; } // JSON array of game names

//        public string? Terms { get; set; } // JSON array of terms and conditions

//        [Required]
//        public int UserId { get; set; } // GameZone Owner ID

//        [ForeignKey("UserId")]
//        public Tblusers? User { get; set; }

//        [Required]
//        public bool Status { get; set; } = true; // Active/Inactive

//        [Required]
//        public DateTime CreatedDate { get; set; } = DateTime.Now;

//        public DateTime? UpdatedDate { get; set; }
//    }
//}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameZoneManagementApi.Models
{
    public class TblOffer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OfferId { get; set; }

        [Required, StringLength(255)]
        public string Title { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required, StringLength(50)]
        public string DiscountType { get; set; } = null!; // "percentage" or "fixed"

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal DiscountValue { get; set; } // 50 for 50% or 1000 for ₹1000 off

        [Required, StringLength(100)]
        public string Category { get; set; } = null!; // weekend, student, birthday, etc.

        [Required]
        public DateTime ValidFrom { get; set; }

        [Required]
        public DateTime ValidUntil { get; set; }

        [Required]
        public bool IsFeatured { get; set; } = false;

        [Required]
        public bool IsTrending { get; set; } = false;

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [StringLength(50)]
        public string? Icon { get; set; } // emoji or icon identifier

        [StringLength(100)]
        public string? GradientClass { get; set; } // CSS gradient class

        [StringLength(20)]
        public string? AccentColor { get; set; } // hex color code

        public string? GamesIncluded { get; set; } // JSON array of game names

        public string? Terms { get; set; } // JSON array of terms and conditions

        [Required]
        public int UserId { get; set; } // GameZone Owner ID

        [ForeignKey("UserId")]
        public Tblusers? User { get; set; }

        [Required]
        public bool Status { get; set; } = true; // Active/Inactive

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; }
    }
}
