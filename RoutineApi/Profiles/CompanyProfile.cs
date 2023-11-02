using AutoMapper;
using RoutineApi.Entities;
using RoutineApi.Models;

namespace RoutineApi.Profiles
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<Company, CompanyDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(origin => origin.Name));

            CreateMap<CompanyAddDto, Company>();
        }
    }
}
