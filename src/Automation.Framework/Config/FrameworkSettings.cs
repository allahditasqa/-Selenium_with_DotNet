using Microsoft.Extensions.Configuration;

namespace Automation.Framework.Config;

/// <summary>
/// Central configuration. Values come from appsettings.json and can be overridden by
/// environment variables that start with AUTOMATION_ (e.g. AUTOMATION_BROWSER=firefox).
/// </summary>
public sealed class FrameworkSettings
{
    private static readonly Lazy<FrameworkSettings> Instance = new(Load);

    public static FrameworkSettings Current => Instance.Value;

    public string BaseUrl { get; private init; } = "https://testautomationpractice.blogspot.com/";
    public string Browser { get; private init; } = "chrome";
    public bool Headless { get; private init; } = true;
    public int WindowWidth { get; private init; } = 1920;
    public int WindowHeight { get; private init; } = 1080;
    public int ExplicitWaitSeconds { get; private init; } = 15;
    public int PageLoadTimeoutSeconds { get; private init; } = 60;
    public bool ScreenshotOnPass { get; private init; } = true;
    public string ReportsDirectory { get; private init; } = "Reports";
    public string LogsDirectory { get; private init; } = "Logs";
    public string DownloadsDirectory { get; private init; } = "Downloads";

    /// <summary>Folder that contains the .sln file. Reports and logs are created next to it.</summary>
    public string RepoRoot { get; private init; } = AppContext.BaseDirectory;

    public string ReportsPath => Path.Combine(RepoRoot, ReportsDirectory);
    public string LogsPath => Path.Combine(RepoRoot, LogsDirectory);
    public string DownloadsPath => Path.Combine(RepoRoot, DownloadsDirectory);

    private static FrameworkSettings Load()
    {
        var cfg = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables(prefix: "AUTOMATION_")
            .Build();

        string Str(string key, string def) => string.IsNullOrWhiteSpace(cfg[key]) ? def : cfg[key]!;
        int Int(string key, int def) => int.TryParse(cfg[key], out var v) ? v : def;
        bool Bool(string key, bool def) => bool.TryParse(cfg[key], out var v) ? v : def;

        return new FrameworkSettings
        {
            BaseUrl = Str("BaseUrl", "https://testautomationpractice.blogspot.com/"),
            Browser = Str("Browser", "chrome").ToLowerInvariant(),
            Headless = Bool("Headless", true),
            WindowWidth = Int("WindowWidth", 1920),
            WindowHeight = Int("WindowHeight", 1080),
            ExplicitWaitSeconds = Int("ExplicitWaitSeconds", 15),
            PageLoadTimeoutSeconds = Int("PageLoadTimeoutSeconds", 60),
            ScreenshotOnPass = Bool("ScreenshotOnPass", true),
            ReportsDirectory = Str("ReportsDirectory", "Reports"),
            LogsDirectory = Str("LogsDirectory", "Logs"),
            DownloadsDirectory = Str("DownloadsDirectory", "Downloads"),
            RepoRoot = FindRepoRoot()
        };
    }

    private static string FindRepoRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null)
        {
            if (dir.GetFiles("*.sln").Length > 0) return dir.FullName;
            dir = dir.Parent;
        }
        return AppContext.BaseDirectory;
    }
}
