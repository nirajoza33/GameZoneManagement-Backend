//using System;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace GameZoneManagementApi.Models
//{
//    [Table("Bookings")]
//    public class TblBooking
//    {
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int BookingId { get; set; }

//        [Required]
//        public int GameId { get; set; }

//        [Required]
//        public int UserId { get; set; }

//        [Required]
//        [DataType(DataType.Date)]
//        public DateTime BookingDate { get; set; }

//        [Required]
//        [DataType(DataType.Time)]
//        public TimeSpan StartTime { get; set; }

//        [Required]
//        [DataType(DataType.Time)]
//        public TimeSpan EndTime { get; set; }

//        [Column(TypeName = "decimal(18,2)")]
//        [Required]
//        public decimal Price { get; set; }

//        [Required]
//        public BookingStatus Status { get; set; } = BookingStatus.Pending;

//        [ForeignKey("GameId")]
//        public TblGame Game { get; set; }

//        [ForeignKey("UserId")]
//        public Tblusers User { get; set; }
//    }

//    public enum BookingStatus
//    {
//        Pending,
//        Confirmed,
//        Cancelled,
//        Refunded
//    }
//}


using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameZoneManagementApi.Models
{
    [Table("Bookings")]
    public class TblBooking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookingId { get; set; }

        [Required]
        public int GameId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

        // Original price before any discounts
        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal OriginalPrice { get; set; }

        // Final price after discounts (what customer actually pays)
        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal Price { get; set; }

        // Offer tracking fields
        public bool HasOfferApplied { get; set; } = false;

        [ForeignKey("AppliedOffer")]
        public int? AppliedOfferId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? DiscountAmount { get; set; }

        [StringLength(20)]
        public string? OfferCode { get; set; }

        [Required]
        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        [ForeignKey("GameId")]
        public TblGame Game { get; set; }

        [ForeignKey("UserId")]
        public Tblusers User { get; set; }

        [ForeignKey("AppliedOfferId")]
        public TblOffer? AppliedOffer { get; set; }

        // Calculated property for savings
        [NotMapped]
        public decimal SavingsAmount => OriginalPrice - Price;

        [NotMapped]
        public decimal SavingsPercentage => OriginalPrice > 0 ? (SavingsAmount / OriginalPrice) * 100 : 0;
    }

    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Cancelled,
        Refunded
    }
}
