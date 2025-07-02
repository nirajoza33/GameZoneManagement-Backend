//using Microsoft.AspNetCore.Http;
//using System.ComponentModel.DataAnnotations;

//namespace GameZoneManagementApi.DTOs
//{
//    public class CreateOfferDto
//    {
//        [Required(ErrorMessage = "Title is required")]
//        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
//        public string Title { get; set; } = string.Empty;

//        [Required(ErrorMessage = "Description is required")]
//        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
//        public string Description { get; set; } = string.Empty;

//        [Required(ErrorMessage = "Discount type is required")]
//        public string DiscountType { get; set; } = "percentage";

//        [Required(ErrorMessage = "Discount value is required")]
//        [Range(0.01, double.MaxValue, ErrorMessage = "Discount value must be greater than 0")]
//        public decimal DiscountValue { get; set; }

//        [Required(ErrorMessage = "Original price is required")]
//        [Range(0.01, double.MaxValue, ErrorMessage = "Original price must be greater than 0")]
//        public decimal OriginalPrice { get; set; }

//        [Range(0, double.MaxValue, ErrorMessage = "Discounted price must be 0 or greater")]
//        public decimal DiscountedPrice { get; set; }

//        [Required(ErrorMessage = "Category is required")]
//        public string Category { get; set; } = string.Empty;

//        [Required(ErrorMessage = "Valid from date is required")]
//        public DateTime ValidFrom { get; set; }

//        [Required(ErrorMessage = "Valid until date is required")]
//        public DateTime ValidUntil { get; set; }

//        public bool IsFeatured { get; set; } = false;

//        public bool IsTrending { get; set; } = false;

//        public string? Icon { get; set; }

//        public string? GradientClass { get; set; }

//        public string? AccentColor { get; set; }

//        public IFormFile? ImageFile { get; set; }

//        public List<string>? GamesIncluded { get; set; } = new List<string>();

//        public List<string>? Terms { get; set; } = new List<string>();

//        public bool Status { get; set; } = true;

//        [Required(ErrorMessage = "User ID is required")]
//        [Range(1, int.MaxValue, ErrorMessage = "User ID must be greater than 0")]
//        public int UserId { get; set; }
//    }

//    public class UpdateOfferDto : CreateOfferDto
//    {
//        public int OfferId { get; set; }
//    }

//    public class OfferResponseDto
//    {
//        public int OfferId { get; set; }
//        public string Title { get; set; } = string.Empty;
//        public string Description { get; set; } = string.Empty;
//        public string DiscountType { get; set; } = string.Empty;
//        public decimal DiscountValue { get; set; }
//        public decimal OriginalPrice { get; set; }
//        public decimal DiscountedPrice { get; set; }
//        public string Category { get; set; } = string.Empty;
//        public DateTime ValidFrom { get; set; }
//        public DateTime ValidUntil { get; set; }
//        public bool IsFeatured { get; set; }
//        public bool IsTrending { get; set; }
//        public string? Icon { get; set; }
//        public string? GradientClass { get; set; }
//        public string? AccentColor { get; set; }
//        public string? ImageUrl { get; set; }
//        public List<string> GamesIncluded { get; set; } = new List<string>();
//        public List<string> Terms { get; set; } = new List<string>();
//        public int UserId { get; set; }
//        public string? UserName { get; set; }
//        public bool Status { get; set; }
//        public DateTime CreatedDate { get; set; }
//        public DateTime? UpdatedDate { get; set; }
//        public string TimeLeft { get; set; } = string.Empty;
//        public decimal SavingsAmount { get; set; }
//        public bool IsExpired { get; set; }
//    }
//}

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GameZoneManagementApi.DTOs
{
    public class CreateOfferDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Discount type is required")]
        public string DiscountType { get; set; } = "percentage";

        [Required(ErrorMessage = "Discount value is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Discount value must be greater than 0")]
        public decimal DiscountValue { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Valid from date is required")]
        public DateTime ValidFrom { get; set; }

        [Required(ErrorMessage = "Valid until date is required")]
        public DateTime ValidUntil { get; set; }

        public bool IsFeatured { get; set; } = false;

        public bool IsTrending { get; set; } = false;

        public string? Icon { get; set; }

        public string? GradientClass { get; set; }

        public string? AccentColor { get; set; }

        public IFormFile? ImageFile { get; set; }

        public List<string>? GamesIncluded { get; set; } = new List<string>();

        public List<string>? Terms { get; set; } = new List<string>();

        public bool Status { get; set; } = true;

        [Required(ErrorMessage = "User ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "User ID must be greater than 0")]
        public int UserId { get; set; }
    }

    public class UpdateOfferDto : CreateOfferDto
    {
        public int OfferId { get; set; }
    }

    public class OfferResponseDto
    {
        public int OfferId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DiscountType { get; set; } = string.Empty;
        public decimal DiscountValue { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsTrending { get; set; }
        public string? Icon { get; set; }
        public string? GradientClass { get; set; }
        public string? AccentColor { get; set; }
        public string? ImageUrl { get; set; }
        public List<string> GamesIncluded { get; set; } = new List<string>();
        public List<string> Terms { get; set; } = new List<string>();
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string TimeLeft { get; set; } = string.Empty;
        public string DiscountDisplay { get; set; } = string.Empty; // "50%" or "₹100 OFF"
        public bool IsExpired { get; set; }
    }
}
