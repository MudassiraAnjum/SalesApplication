using System.ComponentModel.DataAnnotations;

namespace SalesApplication.Dto
{
    public class ResponseTerritoryDto
    {
        [Required]
        public string TerritoryId { get; set; }
        public string TerritoryDescription { get; set; } = null!;
        public int RegionId { get; set; }
    }
}
