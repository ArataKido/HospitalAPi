
using AutoMapper;

using Timetables.Core.Entity;
using Timetables.DTO;

namespace Timetables.Utils.Mappers;
public class TimeTableMappingProfile : Profile
{

    public TimeTableMappingProfile()
    {
        CreateMap<TimeTableCreateUpdateDTO, TimeTable>();

        CreateMap<TimeTable, TimeTableDTO>();
    }

}
