using SalesApplication.Dto;

namespace SalesApplication.IService
{
    public interface ITerritoryService
    {
        Task<string> CreateTerritoryAsync(TerritoryCreateDto territoryCreateDto);
        Task<List<TerritoryResponseDto>> GetAllTerritoriesAsync();

    }
}
