using Microsoft.Extensions.Logging;

namespace Timetables.Core.Settings;
public static class LoggingConfiguration
{
    public static void ConfigureLogging(ILoggingBuilder logging)
    {
        logging.ClearProviders();
        logging.AddConsole();
        logging.SetMinimumLevel(LogLevel.Debug);
    }
}
