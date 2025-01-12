using Microsoft.AspNetCore.Mvc;

using Timetables.Core.Entity;

namespace Timetables.Services;

public interface ITimeTableService
{
    Task<List<TimeTable>> GetTimeTables();
    Task<TimeTable> GetTimeTableById(int id);
    Task<TimeTable> CreateTimeTable(TimeTable timeTable);
    Task<TimeTable> UpdateTimeTable(TimeTable timeTable);
    Task DeleteTimeTable(int id);
    Task<List<TimeTable>> GetDoctorTimeTables(int doctorId);
    Task DeleteDoctorTimeTables(int hospitalId);
    Task<List<TimeTable>> GetHospitalTimeTables(int hospitalId);
    Task DeleteHospitalTimeTables(int hospitalId);
    Task<List<TimeTable>> GetRoomsTimeTable(int hospitalId, string roomName);
}
