using FluentValidation;
using FluentValidation.AspNetCore;

using Timetables.Core.Context;
using Timetables.Core.Entity;
using Timetables.Services;
using Timetables.Utils.Mappers;
using Timetables.Validators;

namespace Timetables.Core.Settings;
public static class ServiceConfiguration
{
    public static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddAutoMapper(typeof(TimeTableMappingProfile));
        builder.Services.AddControllers();
        builder.Services.AddDbContext<PostgresDbContext>();
        builder.Services.AddScoped<ITimeTableService, TimeTableService>();
        builder.Services.AddScoped<IValidator<TimeTable>, TimeTableValidator>();
        builder.Services.AddScoped<MicroserviceClient>();
    }
}
