using SalesApplication.Dto;
namespace SalesApplication.IServices
{
    public interface ITerritoryService
    {
        Task<ResponseTerritoryDto> AddTerritoryAsync(CreateTerritoryDto territoryDto);
    }
}
