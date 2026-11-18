using Automation.Framework.Config;
using Serilog;

namespace Automation.Framework.Logging;

/// <summary>Thin wrapper over Serilog. One log file per test run (never overwritten).</summary>
public static class TestLog
{
    private static readonly object Gate = new();
    private static ILogger? _logger;

    private static ILogger Logger
    {
        get
        {
            if (_logger != null) return _logger;
            lock (Gate)
            {
                if (_logger != null) return _logger;
                var file = Path.Combine(FrameworkSettings.Current.LogsPath,
                    $"test-run {DateTime.Now:dd-MM-yyyy_HH-mm-ss}.log");
                _logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                    .WriteTo.File(file, shared: true,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                    .CreateLogger();
                return _logger;
            }
        }
    }

    public static void Info(string message) => Logger.Information(message);
    public static void Debug(string message) => Logger.Debug(message);
    public static void Warn(string message) => Logger.Warning(message);
    public static void Error(string message, Exception? ex = null) => Logger.Error(ex, message);
}
