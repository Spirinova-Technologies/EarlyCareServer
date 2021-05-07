using AutoMapper;
using EarlyCare.Core.Models;
using EarlyCare.Infrastructure.SharedModels;
using EarlyCare.WebApi.Models;

namespace EarlyCare.WebApi.AutoMapperConfiguration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<CreateUserRequestModel, User>()
               .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<User, UserResponseModel>()
              .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<GoogleSheet, GoogleSheetResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}