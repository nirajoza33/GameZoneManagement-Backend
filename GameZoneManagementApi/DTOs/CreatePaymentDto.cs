namespace GameZoneManagementApi.DTOs
{
    public class CreatePaymentDto
    {
        public string TransactionId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public int GameId { get; set; }
        public string? PaymentStatus { get; set; }
    }
}
