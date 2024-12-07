using AutoMapper;
using SalesApplication.Dto;
using SalesApplication.Models;

namespace SalesApplication.Mapper
{
    public class SalesMapperProfile:Profile
    {
        public SalesMapperProfile()
        {
            //CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<Territory, ResponseTerritoryDto>().ReverseMap();
            CreateMap<TerritoryDto, Territory>().ReverseMap();
            //CreateMap<Shipper, ShipperDto>().ReverseMap();
            //CreateMap<ResponseShipperDto, Shipper>().ReverseMap();
            //CreateMap<Order,OrderDto>().ReverseMap();
        }
    }
}
