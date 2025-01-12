using MassTransit;
using Contracts.Models;

namespace Timetables.Core;

public class MicroserviceClient
{
    private readonly IRequestClient<DoctorRequest> _doctorClient;
    private readonly ILogger<MicroserviceClient> _logger;

    public MicroserviceClient(IRequestClient<DoctorRequest> doctorClient, ILogger<MicroserviceClient> logger)
    {
        _doctorClient = doctorClient;
        _logger = logger;
    }

    public async Task<bool> DoctorExists(int id)
    {
        try
        {
            _logger.LogInformation("Sending DoctorRequest for ID: {Id}", id);
            var response = await _doctorClient.GetResponse<DoctorResponse>(new DoctorRequest { Id = id });
            _logger.LogInformation("Received DoctorResponse for ID: {Id}, Exists: {Exists}", id, response.Message.Exists);
            return response.Message.Exists;
        }
        catch (RequestTimeoutException ex)
        {
            _logger.LogError(ex, "Timeout occurred while checking if doctor exists. ID: {Id}", id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while checking if doctor exists. ID: {Id}", id);
            throw;
        }
    }
}