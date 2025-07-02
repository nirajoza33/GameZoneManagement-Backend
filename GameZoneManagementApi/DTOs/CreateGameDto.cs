namespace GameZoneManagementApi.DTOs
{
    public class CreateGameDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Pricing { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }

        public IFormFile ImageFile { get; set; }

        public bool Status { get; set; } = true;
    }
}



