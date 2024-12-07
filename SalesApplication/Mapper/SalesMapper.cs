using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Dto;
using SalesApplication.Models;


namespace SalesApplication.Mapper
{
    public class SalesMapper : Profile
    {
        public SalesMapper()
        {

            CreateMap<Employee, EmployeeResponseDto>().ReverseMap();
            CreateMap<Employee, EmployeeCompanyResponseDto>().ReverseMap();


            CreateMap<Order, OrdersShipDetailsDto>().ReverseMap();
            CreateMap<Employee, OrdersByEmployeeDto>().ReverseMap();

            CreateMap<TerritoryCreateDto, Territory>().ReverseMap();
            CreateMap<Territory, TerritoryResponseDto>().ReverseMap();

            CreateMap<Shipper, ShipperEarningsResponseDto>().ReverseMap();
            //Externally added 

            CreateMap<ShipperDto, ShipperRegistrationDto>().ReverseMap();

            
        }
    }
}

