////using System.ComponentModel.DataAnnotations.Schema;
////using System.ComponentModel.DataAnnotations;

////namespace GameZoneManagementApi.Models
////{
////    public class TblReview
////    {
////        [Key]
////        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
////        public int ReviewId { get; set; }

////        [Required]
////        public int GameId { get; set; }

////        [ForeignKey("GameId")]
////        public TblGame Game { get; set; }

////        [Required]
////        public int UserId { get; set; }

////        [ForeignKey("UserId")]
////        public Tblusers User { get; set; }

////        [Range(1, 5)]
////        public int Rating { get; set; }

////        public string ReviewText { get; set; }

////        [Required]
////        [StringLength(100)]
////        public string Title { get; set; }

////        public DateTime CreatedDate { get; set; } = DateTime.Now;

////        public int Likes { get; set; } = 0;

////        public int Replies { get; set; } = 0;

////        // Optional: Add sentiment analysis result
////        [StringLength(20)]
////        public string Sentiment { get; set; }


////    }
////}

//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace GameZoneManagementApi.Models
//{
//    [Table("TblReviews")]
//    public class TblReview
//    {
//        [Key]
//        public int ReviewId { get; set; }

//        [Required]
//        public int GameId { get; set; }

//        [Required]
//        public int UserId { get; set; }

//        [Required]
//        [Range(1, 5)]
//        public int Rating { get; set; }

//        [StringLength(200)]
//        public string Title { get; set; }

//        [Required]
//        [StringLength(2000)]
//        public string ReviewText { get; set; }

//        [Required]
//        public DateTime CreatedDate { get; set; }

//        public DateTime? UpdatedDate { get; set; }

//        // NEW: Additional properties for likes and replies functionality
//        public int Likes { get; set; } = 0;

//        public int Replies { get; set; } = 0;

//        [StringLength(20)]
//        public string Sentiment { get; set; }

//        public bool IsDeleted { get; set; } = false;

//        // Existing navigation properties
//        [ForeignKey("GameId")]
//        public virtual TblGame Game { get; set; }

//        [ForeignKey("UserId")]
//        public virtual Tblusers User { get; set; }

//        // NEW: Collection navigation properties for likes and replies
//        public virtual ICollection<TblReviewReply> ReviewReplies { get; set; } = new List<TblReviewReply>();

//        public virtual ICollection<TblReviewLike> ReviewLikes { get; set; } = new List<TblReviewLike>();


//    }
//}




using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameZoneManagementApi.Models
{
    [Table("TblReviews")]
    public class TblReview
    {
        [Key]
        public int ReviewId { get; set; }

        [Required]
        public int GameId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(2000)]
        public string ReviewText { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int Likes { get; set; } = 0;

        public int Replies { get; set; } = 0;

        [StringLength(20)]
        public string Sentiment { get; set; }

        public bool IsDeleted { get; set; } = false;

        // NEW: Support for multiple images (stored as JSON string)
        [StringLength(2000)]
        public string ImageUrls { get; set; }

        // Navigation properties
        [ForeignKey("GameId")]
        public virtual TblGame Game { get; set; }

        [ForeignKey("UserId")]
        public virtual Tblusers User { get; set; }

        public virtual ICollection<TblReviewReply> ReviewReplies { get; set; } = new List<TblReviewReply>();

        public virtual ICollection<TblReviewLike> ReviewLikes { get; set; } = new List<TblReviewLike>();
    }

    

    
}