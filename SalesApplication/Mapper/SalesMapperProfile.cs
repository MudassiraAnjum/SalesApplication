using AutoMapper;
using SalesApplication.Dto;
using SalesApplication.Models;

namespace SalesApplication.Mapper
{
    public class SalesMapperProfile:Profile
    {
        public SalesMapperProfile()
        {
            CreateMap<ResponseOrderDto, Order>().ReverseMap();
            CreateMap<Order, ShipperDetailsDto>().ReverseMap();
            CreateMap<Order, OrdersShipDetailsDto>().ReverseMap();
            CreateMap<Employee, OrdersByEmployeeDto>().ReverseMap();

            CreateMap<ResponseOrderDetailsDto,OrderDetail>().ReverseMap();
        }
    }
}
