using SalesApplication.Dto;

namespace SalesApplication.IServices
{
    public interface IShipperService
    {
        Task<ResponseShipperDto> GetShipperByCompanyNameAsync(string companyName);
        
        Task<List<ResponseShipperDto>> GetTotalAmountEarnedByShipperAsync(int year);
    }
}

