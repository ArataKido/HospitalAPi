using System;

namespace Timetables.DTO;

public class TimeTableCreateUpdateDTO
{
    public required int HospitalId {get; set;}
    public required int DoctorId {get; set;}
    public required DateTime From {get; set;} 
    public required DateTime To {get; set;} 
    public required string RoomName {get; set;}
}
