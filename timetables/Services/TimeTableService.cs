using Microsoft.EntityFrameworkCore;

using Timetables.Core.Context;
using Timetables.Core.Entity;

namespace Timetables.Services;


public class TimeTableService : ITimeTableService
{

    private readonly PostgresDbContext _context;

    public TimeTableService(PostgresDbContext context)
    {
        _context = context;
    }

    public async Task<List<TimeTable>> GetTimeTables()
    {
        return await _context.TimeTables.ToListAsync();
    }

    public async Task<TimeTable?> GetTimeTableById(int id)
    {
        return await _context.TimeTables.FindAsync(id);
    }

    public async Task<TimeTable> CreateTimeTable(TimeTable timeTable)
    {
        await _context.TimeTables.AddAsync(timeTable);
        await _context.SaveChangesAsync();
        return timeTable;
    }

    public async Task<TimeTable> UpdateTimeTable(TimeTable timeTable)
    {
        _context.TimeTables.Update(timeTable);
        await _context.SaveChangesAsync();
        return timeTable;
    }

    public async Task DeleteTimeTable(int id)
    {
        await _context.TimeTables.Where(x => x.Id == id).ExecuteDeleteAsync();
    }

    public async Task<List<TimeTable>> GetDoctorTimeTables(int doctorId)
    {
        return await _context.TimeTables.Where(x => x.DoctorId == doctorId).ToListAsync();
    }

    public async Task DeleteDoctorTimeTables(int doctorId)
    {
        await _context.TimeTables.Where(x => x.DoctorId == doctorId).ExecuteDeleteAsync();
    }

    public async Task<List<TimeTable>> GetHospitalTimeTables(int hospitalId)
    {
        return await _context.TimeTables.Where(x => x.HospitalId == hospitalId).ToListAsync();
    }

    public async Task DeleteHospitalTimeTables(int hospitalId)
    {
        await _context.TimeTables.Where(x => x.HospitalId == hospitalId).ExecuteDeleteAsync();
    }

    public async Task<List<TimeTable>> GetRoomsTimeTable(int hospitalId, string roomName)
    {
        return await _context.TimeTables.Where(x => x.HospitalId == hospitalId && x.RoomName == roomName).ToListAsync();
    }

    public async Task<bool> IsTimeSlotAvailable(int doctorId, DateTime from, DateTime to)
    {
        // Check if the doctor has any overlapping appointments
        var existingAppointments = await _context.TimeTables
            .Where(t => t.DoctorId == doctorId)
            .Where(t => (t.From <= from && t.To > from) ||
                    (t.From < to && t.To >= to) ||
                    (t.From >= from && t.To <= to))
            .AnyAsync();
        return !existingAppointments;
    }
}
