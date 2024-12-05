using SalesApplication.Dto;

namespace SalesApplication.IServices
{
    public interface ITerritoryService
    {
        Task<TerritoryResponseDto> UpdateTerritoryAsync(string territoryId, TerritoryUpdateDto territoryDto);
    }
}

