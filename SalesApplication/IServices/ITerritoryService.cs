using SalesApplication.Dto;

namespace SalesApplication.IServices
{
    public interface ITerritoryService
    {
        Task<ResponseTerritoryDto> UpdateTerritoryAsync(string territoryId, TerritoryDto territoryDto);
    }
}
