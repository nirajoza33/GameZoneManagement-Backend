//using System;
//using GameZoneManagementApi.Models;

//namespace GameZoneManagementApi.DTOs
//{
//    public class BookingWithOfferDto
//    {
//        public int BookingId { get; set; }
//        public int GameId { get; set; }
//        public string GameTitle { get; set; }
//        public string GameCategory { get; set; }
//        public int UserId { get; set; }
//        public string UserName { get; set; }

//        public decimal OriginalPrice { get; set; }
//        public decimal FinalPrice { get; set; }
//        public decimal DiscountAmount { get; set; }
//        public decimal SavingsPercentage { get; set; }

//        public DateTime BookingDate { get; set; }
//        public TimeSpan StartTime { get; set; }
//        public TimeSpan EndTime { get; set; }
//        public BookingStatus Status { get; set; }

//        public int? AppliedOfferId { get; set; }
//        public string OfferTitle { get; set; }
//        public string OfferCode { get; set; }
//        public string OfferCategory { get; set; }
//    }
//}

using System;
using GameZoneManagementApi.Models;

namespace GameZoneManagementApi.DTOs
{
    public class BookingWithOfferDto
    {
        public int BookingId { get; set; }
        public int GameId { get; set; }
        public string GameTitle { get; set; }

        // Changed from string to object to handle TblGameCategory
        public object GameCategory { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal FinalPrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal SavingsPercentage { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public BookingStatus Status { get; set; }
        public int? AppliedOfferId { get; set; }
        public string OfferCode { get; set; }
        public string OfferTitle { get; set; }
        public string OfferCategory { get; set; }
    }
}
