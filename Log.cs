using Microsoft.Extensions.Logging;
using Serilog;

namespace GrabDRCCData;

internal class Log
{
    private const string LOG_CATEGORY = "AimParser";

    private static Microsoft.Extensions.Logging.ILogger _logger;

    internal static Microsoft.Extensions.Logging.ILogger Logger
    {
        get
        {
            if (_logger == null)
            {

                var serilogLogger = new Serilog.LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Is(Serilog.Events.LogEventLevel.Debug)
                .WriteTo.Console()
                .CreateLogger();
                var factory = new Serilog.Extensions.Logging.SerilogLoggerFactory(serilogLogger);

                _logger = factory.CreateLogger<Program>();
            }

            return _logger;
        }
        set
        {
            _logger = value;
        }
    }

    /// <summary>
    /// Intended to be called if the application wide logging configuration changes. Will force
    /// the singleton logger to be re-created.
    /// </summary>
    internal static void Reset()
    {
        _logger = null;
    }
}