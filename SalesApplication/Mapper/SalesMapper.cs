using SalesApplication.Models;
using AutoMapper;
using SalesApplication.Dto;

namespace SalesApplication.Mapper
{
    public class SalesMapper : Profile
    {
        public SalesMapper()
        {
            CreateMap<Employee, ResponseEmployeeDto>().ReverseMap();
            CreateMap<Shipper,ResponseShipperDto>().ReverseMap();
            CreateMap<Order,ResponseOrderDto>().ReverseMap();
            CreateMap<Territory,TerritoryUpdateDto>().ReverseMap();
            CreateMap<TerritoryResponseDto,Territory>().ReverseMap();
            CreateMap<Order,OrderInfoDto>().ReverseMap();
           
        }
    }
}

