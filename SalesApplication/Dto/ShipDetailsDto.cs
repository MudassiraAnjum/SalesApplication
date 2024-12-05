namespace SalesApplication.Dto
{
    public class ShipDetailsDto
    {
        public int OrderId { get; set; }
        public DateTime ShippedDate { get; set; }

        public string ShipVia { get; set; } = null!;

        public string Freight { get; set; } = null!;

        public string ShipName { get; set; } = null!;

        public string ShipAddress { get; set; } = null!;

        public string ShipCity { get; set; } = null!;

        public string ShipRegion { get; set; } = null!;

        public string ShipPostalCode { get; set; } = null!;

        public string ShipCountry { get; set; } = null!;

        public int ShipperId { get; set; }
    }
}
