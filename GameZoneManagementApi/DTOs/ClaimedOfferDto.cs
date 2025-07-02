using System;

namespace GameZoneManagementApi.DTOs
{
    public class ClaimedOfferDto
    {
        public int ClaimedOfferId { get; set; }
        public int OfferId { get; set; }
        public int UserId { get; set; }
        public DateTime ClaimedDate { get; set; }
        public DateTime? UsedDate { get; set; }
        public bool IsUsed { get; set; }
        public bool IsActive { get; set; }
        public string OfferCode { get; set; }

        // Offer details
        public string OfferTitle { get; set; }
        public string Description { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal DiscountedPrice { get; set; }
        public string Category { get; set; }
        public DateTime ValidUntil { get; set; }
        public string ImageUrl { get; set; }
        public string[] GamesIncluded { get; set; }
        public bool IsExpired { get; set; }
        public string TimeLeft { get; set; }
    }
}
