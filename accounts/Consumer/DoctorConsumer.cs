
using Contracts.Models;
using Accounts.Services;
using MassTransit;

namespace Accounts.Consumer;
public class DoctorConsumer : IConsumer<DoctorRequest>
{
    private readonly ILogger<DoctorConsumer> _logger;
    private readonly AccountService _accountService;

    public DoctorConsumer(ILogger<DoctorConsumer> logger, AccountService accountService)
    {
        _logger = logger;
        _accountService = accountService;
    }

    public async Task Consume(ConsumeContext<DoctorRequest> context)
    {
        _logger.LogInformation("Received DoctorRequest for ID: {Id}", context.Message.Id);

        try
        {
            var accountExists = await _accountService.AccountWithRoleExists(context.Message.Id, "doctor");
            await context.RespondAsync(new DoctorResponse { Exists = accountExists });
            _logger.LogInformation("Sent DoctorResponse for ID: {Id}, Exists: {Exists}", context.Message.Id, accountExists);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing DoctorRequest for ID: {Id}", context.Message.Id);
            throw;
        }
    }
}