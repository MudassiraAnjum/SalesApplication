namespace SalesApplication.Dto
{
    public class ResponseShipperDto
    {
        public string ShipperId { get; set; }
        public string CompanyName { get; set; } = null!;

        public string? Phone { get; set; }

        public string? Email { get; set; }

    }
}