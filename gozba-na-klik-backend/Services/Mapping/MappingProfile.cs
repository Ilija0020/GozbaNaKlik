using AutoMapper;
using gozba_na_klik_backend.Services.DTOs;
using gozba_na_klik_backend.Domain.Entities;

namespace gozba_na_klik_backend.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserDTO>()
                .ForMember(
                    destination => destination.Username,
                    options => options.MapFrom(source => source.UserName))
                .ForMember(
                    destination => destination.Role,
                    options => options.Ignore());
            CreateMap<UserRegisterDTO, ApplicationUser>()
                .ForMember(
                    destination => destination.UserName,
                    options => options.MapFrom(source => source.Username));
            CreateMap<UserAdminRegisterDTO, ApplicationUser>()
                .ForMember(
                    destination => destination.UserName,
                    options => options.MapFrom(source => source.Username));

            CreateMap<Restaurant, RestaurantDTO>();
            CreateMap<RestaurantCreateDTO, Restaurant>();
            CreateMap<RestaurantUpdateDTO, Restaurant>();
            CreateMap<RestaurantOwnerUpdateDTO, Restaurant>();

            CreateMap<RestaurantWorkingHours, RestaurantWorkingHoursDTO>().ReverseMap();
            CreateMap<NonWorkingDay, NonWorkingDayDTO>().ReverseMap();

            CreateMap<Meal, MealDTO>();
            CreateMap<Allergen, AllergenDTO>();
        }
    }
}