using Microsoft.AspNetCore.JsonPatch;
using SalesApplication.Dtos;

namespace SalesApplication.IServices
{
    public interface IShipperService
    {
        Task<IEnumerable<ResponseShipperDto>> GetAllShippersByCompanyNameAsync(string companyName);
        Task UpdateShipperAsync(int id, JsonPatchDocument<ResponseShipperDto> patchDoc);
    }
}
