using SalesApplication.Dto;

namespace SalesApplication.IServices
{
    public interface ITerritoryService
    {
        Task<List<ResponseTerritoryDto>> GetTerritory();
        Task<ResponseTerritoryDto> AddTerritoryAsync(TerritoryDto territoryDto);
    }
}
