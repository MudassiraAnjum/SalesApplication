using System.ComponentModel.DataAnnotations;

namespace SalesApplication.Dto
{
    public class TerritoryDto
    {
        [Required]
        public string TerritoryId { get; set; } = null!;
        public string TerritoryDescription { get; set; } = null!;
        public int RegionId { get; set; }
    }
}