using AutoMapper;
using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

using Contracts;
using Timetables.DTO;
using Timetables.Core.Entity;
using Timetables.Services;
using Timetables.Utils.Extensions;
using Timetables.Core;

namespace Timetables.Controllers;

[ApiController]
[Route("/api/Timetable")]
[Produces("application/json")]
public class TimeTableController:ControllerBase
{
    private readonly ITimeTableService _timeTableService;
    private readonly IMapper _mapper;
    private readonly IValidator<TimeTable> _validator;
    private readonly MicroserviceClient _microserviceClient;

    public TimeTableController(ITimeTableService timeTableService, IMapper mapper, IValidator<TimeTable> validator, MicroserviceClient microserviceClient)
    {
        _timeTableService = timeTableService;
        _mapper = mapper;
        _validator = validator;
        _microserviceClient = microserviceClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetTimeTabels()
    {
        return Ok(await _timeTableService.GetTimeTables());
    }

    [HttpGet("{id}/")]
    public async Task<IActionResult> GetTimeTableById(int id)
    {
        var timeTable = await _timeTableService.GetTimeTableById(id);

        if(timeTable is not null) return Ok(timeTable);
        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateTimeTable(TimeTableCreateUpdateDTO timeTableCreateUpdateDTO)
    {
        TimeTable timeTable = _mapper.Map<TimeTable>(timeTableCreateUpdateDTO);

        if(!await _microserviceClient.DoctorExists(timeTable.DoctorId)) 
        {
            return BadRequest($"Doctor with {timeTable.DoctorId} does not exits" );
        }


        var timeTableValidator = await _validator.ValidateAsync(timeTable);

        if (!timeTableValidator.IsValid)
        {
            timeTableValidator.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }
        await _timeTableService.CreateTimeTable(timeTable);

        TimeTableDTO timeTableDTO = _mapper.Map<TimeTableDTO>(timeTable);
        return CreatedAtAction(nameof(CreateTimeTable), timeTableDTO);
    }

    [HttpPut("{id}/")]
    public async Task<IActionResult> UpdateTimeTable(int id, TimeTableCreateUpdateDTO timeTableCreateUpdateDTO)
    {
        TimeTable timeTable = await _timeTableService.GetTimeTableById(id:id);
        _mapper.Map(timeTableCreateUpdateDTO, timeTable);

        var timeTableValidator = await _validator.ValidateAsync(timeTable);

        if (!timeTableValidator.IsValid)
        {
            timeTableValidator.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }
        await _timeTableService.UpdateTimeTable(timeTable);

        TimeTableDTO timeTableDTO = _mapper.Map<TimeTableDTO>(timeTable);
        return CreatedAtAction(nameof(CreateTimeTable), timeTableDTO);
    }

    [HttpDelete("{id}/")]
    public async Task<IActionResult> DeleteTimeTable(int id)
    {
        await _timeTableService.DeleteTimeTable(id);
        return Ok();
    }
    
    [HttpGet("Doctor/{doctorId}")]
    public async Task<IActionResult> GetDoctorTimeTable(int doctorId)
    {
        var timeTable = await _timeTableService.GetDoctorTimeTables(doctorId);
        if(timeTable.Count != 0) return Ok(timeTable);
        return NotFound();
    }
    
    [HttpDelete("Doctor/{doctorId}")]
    public async Task<IActionResult> DeleteDoctorTimeTable(int doctorId)
    {
        await _timeTableService.DeleteDoctorTimeTables(doctorId);
        return Ok();
    }

    [HttpGet("Hospital/{hospitalId}")]
    public async Task<IActionResult> GetHospitalTimeTable(int hospitalId)
    {
        var hospital = await _timeTableService.GetHospitalTimeTables(hospitalId);
        if(hospital.Count != 0) return Ok(hospital);
        return NotFound();
    }
    
    [HttpDelete("Hospital/{hospitalId}")]
    public async Task<IActionResult> DeleteHospitalTimeTable(int hospitalId)
    {
        await _timeTableService.DeleteHospitalTimeTables(hospitalId);
        return Ok();
    }

    [HttpGet("/Hospital/{id}/Room/{room}")]
    public async Task<IActionResult> GetRoomsTimeTable(int id, string room)
    {
        return Ok(await _timeTableService.GetRoomsTimeTable(id, room));
    }
}