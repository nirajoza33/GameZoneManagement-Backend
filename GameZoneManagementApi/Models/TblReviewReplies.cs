using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameZoneManagementApi.Models
{
    [Table("TblReviewReplies")]
    public class TblReviewReply
    {
        [Key]
        public int ReplyId { get; set; }

        [Required]
        public int ReviewId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(1000)]
        public string ReplyText { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool IsDeleted { get; set; } = false;

        // Navigation properties
        [ForeignKey("ReviewId")]
        public virtual TblReview Review { get; set; }

        [ForeignKey("UserId")]
        public virtual Tblusers User { get; set; }
    }
}