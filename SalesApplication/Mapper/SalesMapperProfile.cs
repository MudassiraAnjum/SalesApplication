using AutoMapper;
using SalesApplication.Dto;
using SalesApplication.Models;

namespace SalesApplication.Mapper
{
    public class SalesMapperProfile:Profile
    {
        public SalesMapperProfile()
        {
            CreateMap<ResponseEmployeeDto, Employee>().ReverseMap(); 
            CreateMap<Territory, TerritoryDto>().ReverseMap();
            CreateMap<Employee, EmployeeSalesDto>().ReverseMap();
            CreateMap<Employee, EmployeeDto>().ReverseMap();

        }
    }
}
