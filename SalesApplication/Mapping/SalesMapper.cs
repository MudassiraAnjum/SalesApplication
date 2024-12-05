using AutoMapper;
using SalesApplication.Dto;
using SalesApplication.Models;


namespace SalesApplication.Mapping
{
    public class SalesMapper : Profile
    {
        public SalesMapper()
        {
            CreateMap<Employee, CreateEmployeeDto>().ReverseMap();
            CreateMap<ResponseEmployeeDto, Employee>().ReverseMap();

            CreateMap<CreateTerritoryDto, Territory>().ReverseMap();
            CreateMap<Territory, ResponseTerritoryDto>().ReverseMap();

            CreateMap<ResponseShipperDto, Shipper>().ReverseMap();

            CreateMap<ResponseOrderDto, Order>().ReverseMap();

            CreateMap<OrderDetail, ResponseOrderDetailsDto>().ReverseMap();

        }
    }
}
