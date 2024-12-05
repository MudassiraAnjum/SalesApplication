using SalesApplication.Dtos;

namespace SalesApplication.IServices
{
    public interface ITerritoryService
    {
        Task<ResponseTerritoryDto> AddTerritoryAsync(TerritoryDto territoryDto);
    }
}
