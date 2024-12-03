using SalesApplication.Dto;

namespace SalesApplication.IServices
{
    public interface ITerritoryService
    {
        Task<List<TerritoryDto>> GetTerritory();
    }
}
