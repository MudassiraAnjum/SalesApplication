using SalesApplication.Models;

namespace SalesApplication.Dto
{
    public class ResponseOrderDto
    {
        public int OrderId { get; set; }
        public string? ShipRegion { get; set; }
        public string? ShipName { get; set; }
        public string? ShipAddress { get; set; }
        
    }
}