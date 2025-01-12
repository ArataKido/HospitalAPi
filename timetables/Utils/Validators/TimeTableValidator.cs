
using FluentValidation;

using Timetables.Core.Entity;

namespace Timetables.Validators;

public class TimeTableValidator : AbstractValidator<TimeTable>
{
    public TimeTableValidator()
    {
        RuleFor(x => x.From)
            .Must(BeValidTimeInterval)
            .WithMessage("From time must be in 30-minute intervals with seconds set to 0");
        RuleFor(x => x.To)
            .Must(BeValidTimeFormat)
            .WithMessage("From time must be in 30-minute intervals with seconds set to 0");

        RuleFor(x => x.From)
            .Must(BeValidTimeFormat)
            .WithMessage("Only timestamp with UTC time zone is supported.");
        RuleFor(x => x.To)
            .Must(BeValidTimeInterval)
            .WithMessage("Only timestamp with UTC time zone is supported.");

        RuleFor(x => new { x.From, x.To })
            .Must(x => x.To > x.From)
            .WithMessage("To time must be after From time")
            .Must(x => (x.To - x.From).TotalHours <= 12)
            .WithMessage("Time difference cannot exceed 12 hours");
    }

    private bool BeValidTimeInterval(DateTime time)
    {
        return time.Second == 0 && time.Minute % 30 == 0;
    }
    private bool BeValidTimeFormat(DateTime time)
    {
        return time.Kind == DateTimeKind.Utc;
    }
}
