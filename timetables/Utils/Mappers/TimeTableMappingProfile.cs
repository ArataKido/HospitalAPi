
using AutoMapper;
using Timetables.DTO;
using Timetables.Core.Entity;

namespace Timetables.Utils.Mappers;
public class TimeTableMappingProfile : Profile
{

    public TimeTableMappingProfile()
    {
        CreateMap<TimeTableCreateUpdateDTO, TimeTable>();


        // CreateMap<AccountUpdateDTO, Account>()
        //     .ForMember(dest => dest.FullName, opt => opt.Ignore())
        //     .AfterMap((src, dest) => dest.UpdateFullName())
        //     .ForAllMembers(opts =>
        //     {
        //         opts.Condition((src, dest, srcMember) => srcMember != null);
        //     });

        // CreateMap<AccountPrivateUpdateDTO, Account>()
        //     .ForMember(dest => dest.FullName, opt => opt.Ignore())
        //     .AfterMap((src, dest) => dest.UpdateFullName())
        //     .ForAllMembers(opts =>
        //     {
        //         opts.Condition((src, dest, srcMember) => srcMember != null);
        //     });

        CreateMap<TimeTable, TimeTableDTO>();
            // .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.T.Select(ar => ar.Role.Name)));
    }

}