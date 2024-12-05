using AutoMapper;
using SalesApplication.Dto;
using SalesApplication.Models;
namespace SalesApplication.Mapping
{
    public class Salesmapper:Profile
    {
        public Salesmapper()
        {
            CreateMap<ShipperDto, Shipper>().ReverseMap();
            CreateMap<Shipper, ResponseShipperDto>().ReverseMap();

            CreateMap<EmployeeDto, Employee>().ReverseMap();
            CreateMap<Employee, ResponseEmployeeDto>().ReverseMap();

            CreateMap<OrderDto, Order>().ReverseMap();
            CreateMap<Order, ResponseOrderDto>().ReverseMap();

            CreateMap<Order, ShipDetailsDto>().ReverseMap();
            CreateMap<OrderDetailDto, OrderDetail>().ReverseMap();
            CreateMap<OrderDetail,ResponseOrderDetailDto>().ReverseMap();
            CreateMap<TerritoryDto,Territory>().ReverseMap();
            CreateMap<Territory,ResponseTerritoryDto>().ReverseMap();

        }
    }
}
