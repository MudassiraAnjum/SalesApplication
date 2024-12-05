using AutoMapper;
using SalesApplication.Dtos;
using SalesApplication.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SalesApplication.Mapping
{
    public class SalesMapperProfile : Profile
    {
        public SalesMapperProfile()
        {
            CreateMap<Employee, ResponseEmployeeDto>().ReverseMap();
            CreateMap<Order, ResponseOrderDto>().ReverseMap();
            CreateMap<Territory, ResponseTerritoryDto>().ReverseMap();
            CreateMap<TerritoryDto, Territory>().ReverseMap();
            CreateMap<Shipper, ResponseShipperDto>().ReverseMap();
        }
    }
}
