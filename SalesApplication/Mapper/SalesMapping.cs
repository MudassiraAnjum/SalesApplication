using AutoMapper;
using SalesApplication.Dto;
using SalesApplication.Models;

namespace SalesApplication.Mapper
{
    public class SalesMapping:Profile
    {
        public SalesMapping()
        {
            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<Employee, ResponseEmployeeDto>().ReverseMap();
            CreateMap<Territory, TerritoryDto>().ReverseMap();
            CreateMap<Shipper, ShipperDto>().ReverseMap();
            CreateMap<Shipper, ShipperUpdateDto>().ReverseMap();
            CreateMap<ResponseShipperDto, Shipper>().ReverseMap();
            CreateMap<Order,OrderDto>().ReverseMap();
        }
    }
}
