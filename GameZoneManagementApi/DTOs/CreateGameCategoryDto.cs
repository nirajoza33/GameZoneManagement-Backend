namespace GameZoneManagementApi.DTOs
{
    public class CreateGameCategoryDto
    {
        public string CategoryName { get; set; } = null!;
        public string? Description { get; set; }
    }
}
