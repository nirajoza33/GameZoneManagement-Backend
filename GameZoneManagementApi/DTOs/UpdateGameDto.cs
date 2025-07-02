using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GameZoneManagementApi.DTOs
{
    public class UpdateGameDto
    {

        public int GameId { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Pricing { get; set; }

        public int CategoryId { get; set; }
        public int UserId { get; set; }

        public IFormFile? ImageFile { get; set; }

        public bool Status { get; set; } = true;
    }
}

