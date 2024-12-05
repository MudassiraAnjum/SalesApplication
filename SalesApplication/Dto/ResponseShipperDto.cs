namespace SalesApplication.Dto
{
    public class ResponseShipperDto
    {
        
        public decimal TotalAmount { get; internal set; }
        public int ShipperId { get; set; }

        public string CompanyName { get; set; } = null!;

        public string? Phone { get; set; }

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
       

    }
}
