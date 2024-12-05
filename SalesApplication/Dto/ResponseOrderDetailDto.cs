namespace SalesApplication.Dto
{
    public class ResponseOrderDetailDto
    {
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public decimal UnitPrice { get; set; }

        public short Quantity { get; set; }

        public float Discount { get; set; }
        public decimal BillAmount { get; set; }
    }
}
