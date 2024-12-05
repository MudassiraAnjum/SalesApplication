using System.ComponentModel.DataAnnotations;

namespace SalesApplication.Dto
{
    public class OrdersShipDetailsDto
    {
        [Required(ErrorMessage = "Ship name is required.")]
        [StringLength(100, ErrorMessage = "Ship name cannot exceed 100 characters.")]
        public string ShipName { get; set; }

        [Required(ErrorMessage = "Ship address is required.")]
        [StringLength(200, ErrorMessage = "Ship address cannot exceed 200 characters.")]
        public string ShipAddress { get; set; }

        public string ShipRegion { get; set; }
        public string ShipPostalCode { get; set; }


    }
}
