using System.Reflection;
using Automation.Framework.Config;
using Automation.Framework.Drivers;
using Automation.Framework.Logging;
using Automation.Framework.Reporting;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;

namespace Automation.Framework.Core;

/// <summary>
/// Parent of every test class. Creates one browser per test, quits it afterwards and
/// records the result (+ screenshot) for the HTML report.
/// </summary>
public abstract class BaseTest
{
    protected IWebDriver Driver { get; private set; } = null!;
    protected FrameworkSettings Settings => FrameworkSettings.Current;
    protected string DownloadDir { get; private set; } = "";

    private readonly List<(string, string)> _extraShots = new();

    [SetUp]
    public void BaseSetUp()
    {
        var name = TestContext.CurrentContext.Test.Name;
        TestLog.Info($"START  {GetType().Name}.{name}");
        _timer.Restart();
        DownloadDir = Path.Combine(Settings.DownloadsPath, Guid.NewGuid().ToString("N"));
        Driver = DriverFactory.Create(Settings, DownloadDir);
    }

    [TearDown]
    public void BaseTearDown()
    {
        var ctx = TestContext.CurrentContext;
        var status = ctx.Result.Outcome.Status switch
        {
            TestStatus.Passed => "passed",
            TestStatus.Failed => "failed",
            _ => "skipped"
        };

        var spec = ctx.Test.Method?.MethodInfo.GetCustomAttribute<TestSpecAttribute>();
        var record = new TestRecord
        {
            Module = GetType().GetCustomAttribute<ModuleAttribute>()?.Name ?? GetType().Name,
            ClassName = GetType().Name,
            TestName = ctx.Test.Name,   // includes TestCase arguments, e.g. Form_Sections(2)
            Title = spec != null ? $"{spec.Id} - {spec.Title}" : TitleOf(ctx),
            Spec = spec,
            Status = status,
            DurationSeconds = _timer.Elapsed.TotalSeconds,
            Message = status == "passed" ? null : ctx.Result.Message,
            StackTrace = status == "failed" ? ctx.Result.StackTrace : null
        };

        record.Screenshots.AddRange(_extraShots);
        if (status != "passed" || Settings.ScreenshotOnPass)
        {
            var shot = TakeScreenshot();
            if (shot != null) record.Screenshots.Add((status == "failed" ? "failure screenshot" : "final screenshot", shot));
        }

        ReportSession.Add(record);
        TestLog.Info($"{status.ToUpperInvariant(),-7}{record.ClassName}.{record.TestName}");
        if (status == "failed") TestLog.Error(ctx.Result.Message ?? "failed");

        _extraShots.Clear();
        try { Driver?.Quit(); } catch { /* browser already gone */ }
        try { Driver?.Dispose(); } catch { }
    }

    /// <summary>Add a named screenshot to the report in the middle of a test.</summary>
    protected void Snap(string caption)
    {
        var shot = TakeScreenshot();
        if (shot != null) _extraShots.Add((caption, shot));
    }

    private string? TakeScreenshot()
    {
        if (Driver == null) return null;   // browser never started
        for (int attempt = 0; attempt < 2; attempt++)
        {
            try { return ((ITakesScreenshot)Driver).GetScreenshot().AsBase64EncodedString; }
            catch (UnhandledAlertException)
            {
                try { Driver.SwitchTo().Alert().Dismiss(); } catch { }
            }
            catch (Exception ex)
            {
                TestLog.Warn($"Screenshot failed: {ex.Message}");
                return null;
            }
        }
        return null;
    }

    private static string TitleOf(TestContext ctx)
    {
        var desc = ctx.Test.Properties.Get("Description") as string;
        return string.IsNullOrWhiteSpace(desc) ? ctx.Test.Name : desc;
    }

    private readonly System.Diagnostics.Stopwatch _timer = new();
}
