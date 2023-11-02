using AutoMapper;
using Microsoft.OpenApi.Extensions;
using RoutineApi.Entities;
using RoutineApi.Models;

namespace RoutineApi.Profiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(origin => $"{origin.FirestName} {origin.LastName}"))
                .ForMember(dest => dest.GenderDisplay, opt => opt.MapFrom(origin => origin.Gender.ToString()))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(origin => DateTime.Now.Year - origin.DateOfBirth.Year));

            CreateMap<EmployeeAddDto, Employee>();
            CreateMap<EmployeeUpdateDto, Employee>();
            CreateMap<Employee, EmployeeUpdateDto>();
        }
    }
}
