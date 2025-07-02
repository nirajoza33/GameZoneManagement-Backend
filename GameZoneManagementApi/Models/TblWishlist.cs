using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GameZoneManagementApi.Models
{
    public class TblWishlist
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WishlistId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public Tblusers User { get; set; }

        [Required]
        public int GameId { get; set; }

        [ForeignKey("GameId")]
        public TblGame Game { get; set; }
    }
}
