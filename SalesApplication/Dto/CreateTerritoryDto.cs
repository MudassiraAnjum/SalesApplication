using SalesApplication.Models;
using System.ComponentModel.DataAnnotations;

namespace SalesApplication.Dto
{
    public class CreateTerritoryDto
    {
        [Required]
        public string TerritoryId { get; set; } = null!;
        [Required]
        public string TerritoryDescription { get; set; } = null!;
        [Required]
        public int RegionId { get; set; }

    }
}