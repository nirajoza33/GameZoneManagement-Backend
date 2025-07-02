//using System;

//namespace GameZoneManagementApi.DTOs
//{
//    public class CreateBookingDto
//    {
//        public int GameId { get; set; }
//        public int UserId { get; set; }
//        public decimal Price { get; set; }

//        public DateTime BookingDate { get; set; }
//        public TimeSpan StartTime { get; set; }
//        public TimeSpan EndTime { get; set; }
//    }
//}

using System;

namespace GameZoneManagementApi.DTOs
{
    public class CreateBookingDto
    {
        public int GameId { get; set; }
        public int UserId { get; set; }

        // Original price before any discounts
        public decimal OriginalPrice { get; set; }

        // Final price after discount (if any)
        public decimal Price { get; set; }

        public DateTime BookingDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        // Offer-related fields
        public bool HasOfferApplied { get; set; } = false;
        public int? AppliedOfferId { get; set; }
        public decimal? DiscountAmount { get; set; }
        public string? OfferCode { get; set; }
    }
}

