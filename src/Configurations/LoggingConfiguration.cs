

using Serilog;
using Serilog.Events;

namespace InsuraNova.Configurations
{
    public static class LoggingConfiguration
    {
       
        public static void ConfigureSerilog()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()

                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)


                .WriteTo.File(
                    path: "logs/success/success_log_.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 10,
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message:lj}{NewLine}{Exception}"
                )

                .WriteTo.File(
                    path: "logs/errors/error_log_.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 10,
                    restrictedToMinimumLevel: LogEventLevel.Error, 
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message:lj}{NewLine}{Exception}"
                )

                .WriteTo.Console()

                .CreateLogger();
        }
    }
}
