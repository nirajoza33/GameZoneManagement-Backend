using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GameZoneManagementApi.Models
{
    public class TblPayment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentId { get; set; }

        [Required]
        public string TransactionId { get; set; }

        [Required]
        [ForeignKey("TblUsers")]
        public int UserId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        [ForeignKey("TblGame")]
        public int GameId { get; set; }

        [Required]
        public PaymentStatus PaymentStatus { get; set; }

        public Tblusers TblUsers { get; set; }
        public TblGame TblGame { get; set; }
    }

    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed
    }
}
