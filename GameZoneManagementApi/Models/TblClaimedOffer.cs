//using System;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace GameZoneManagementApi.Models
//{
//    public class TblClaimedOffer
//    {
//        [Key]
//        public int ClaimedOfferId { get; set; }

//        [Required]
//        public int OfferId { get; set; }

//        [Required]
//        public int UserId { get; set; }     

//        [Required]
//        public DateTime ClaimedDate { get; set; }

//        public DateTime? UsedDate { get; set; }

//        [Required]
//        public bool IsUsed { get; set; }

//        [Required]
//        public bool IsActive { get; set; }

//        // Unique code for this claimed offer
//        [StringLength(20)]
//        public string OfferCode { get; set; }

//        // Navigation properties
//        [ForeignKey("OfferId")]
//        public virtual TblOffer Offer { get; set; }

//        [ForeignKey("UserId")]
//        public virtual Tblusers User { get; set; }
//    }
//}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameZoneManagementApi.Models
{
    public class TblClaimedOffer
    {
        [Key]
        public int ClaimedOfferId { get; set; }

        [Required]
        public int OfferId { get; set; }

        [Required]
        public int UserId { get; set; }

        // Add BookingId property (nullable since it's only set when the offer is used)
        public int? BookingId { get; set; }

        [Required]
        public DateTime ClaimedDate { get; set; }

        public DateTime? UsedDate { get; set; }

        [Required]
        public bool IsUsed { get; set; }

        [Required]
        public bool IsActive { get; set; }

        // Unique code for this claimed offer
        [StringLength(20)]
        public string OfferCode { get; set; }

        // Add actual discount amount applied when the offer was used
        [Column(TypeName = "decimal(18,2)")]
        public decimal ActualDiscountApplied { get; set; } = 0;

        // Navigation properties
        [ForeignKey("OfferId")]
        public virtual TblOffer Offer { get; set; }

        [ForeignKey("UserId")]
        public virtual Tblusers User { get; set; }

        // Add navigation property to Booking
        [ForeignKey("BookingId")]
        public virtual TblBooking Booking { get; set; }
    }
}

