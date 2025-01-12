

using Microsoft.EntityFrameworkCore;

namespace Timetables.Core.Entity;

[Index(nameof(Id), IsUnique = true)]
[Index(nameof(HospitalId))]
[Index(nameof(DoctorId))]
public class TimeTable
{
    public required int Id {get; set;}
    public required int HospitalId {get; set;}
    public required int DoctorId {get; set;}
    public required DateTime From {get; set;} 
    public required DateTime To {get; set;} 
    public required string RoomName {get; set;}
}