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
            CreateMap<Shipper, ShipperDto>().ReverseMap();
            CreateMap<ResponseShipperDto, Shipper>().ReverseMap();
            CreateMap<Shipper, ShipperUpdateDto>().ReverseMap();
            //CreateMap<Order,OrderDto>().ReverseMap();
            CreateMap<ResponseEmployeeDto, Employee>().ReverseMap(); 
            CreateMap<Territory, TerritoryDto>().ReverseMap();
            CreateMap<Employee, EmployeeSalesDto>().ReverseMap();
            CreateMap<Employee, EmployeeDto>().ReverseMap();

        }
    }
}
