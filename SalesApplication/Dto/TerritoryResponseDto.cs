namespace SalesApplication.Dto
{
    public class TerritoryResponseDto
    {
        
        public string TerritoryId { get; set; } = null!; // Territory ID
        public string TerritoryDescription { get; set; } = null!; // Territory Description
        public int RegionId { get; set; } // Region ID
        public string RegionName { get; set; } = null!;
    }
}
