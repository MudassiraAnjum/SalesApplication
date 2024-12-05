namespace SalesApplication.Dtos
{
    public class TerritoryDto
    {
        public string TerritoryId { get; set; } = null!;
        public string TerritoryDescription { get; set; } = null!;
        public int RegionId { get; set; }
    }
}
