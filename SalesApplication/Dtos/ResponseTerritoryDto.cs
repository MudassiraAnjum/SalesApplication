namespace SalesApplication.Dtos
{
    public class ResponseTerritoryDto
    {
        public int TerritoryId { get; set; }
        public string TerritoryDescription { get; set; } = null!;
        public int RegionId { get; set; }
    }
}
