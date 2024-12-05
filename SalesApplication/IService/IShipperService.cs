using SalesApplication.Dto;

namespace SalesApplication.IServices
{
    public interface IShipperService
    {
        Task<List<ShipperEarningsResponseDto>> GetTotalAmountEarnedByShipperAsync(DateTime date);
        Task<List<ShipperEarningsResponseDto>> GetTotalAmountEarnedByShipperAsync(int year);
    }
}
