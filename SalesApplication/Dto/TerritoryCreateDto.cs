namespace SalesApplication.Dto
{
    public class TerritoryCreateDto
    {
        public string TerritoryId { get; set; } = null!; // Required Territory ID
        public string TerritoryDescription { get; set; } = null!; // Required Territory Description
        public int RegionId { get; set; } // ID of the associated region
    }

}
