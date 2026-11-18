using System.Collections.Concurrent;
using Automation.Framework.Config;
using Automation.Framework.Logging;

namespace Automation.Framework.Reporting;

/// <summary>
/// Collects results during a run and, at the end, writes ONE html report + csv per module
/// plus the shared index.html. Every run gets new, timestamped files: nothing is overwritten.
/// </summary>
public static class ReportSession
{
    private static readonly ConcurrentBag<TestRecord> Records = new();
    private static DateTime _start = DateTime.Now;

    public static void Start()
    {
        _start = DateTime.Now;
        TestLog.Info($"=== Test run started {_start:dd-MM-yyyy HH:mm:ss} ===");
    }

    public static void Add(TestRecord record) => Records.Add(record);

    public static void Finish()
    {
        var settings = FrameworkSettings.Current;
        var all = Records.OrderBy(r => r.Module).ThenBy(r => r.Title).ToList();
        if (all.Count == 0) return;

        var run = new RunEntry
        {
            RunId = _start.ToString("yyyyMMdd-HHmmss"),
            StartedAt = _start,
            DurationSeconds = (DateTime.Now - _start).TotalSeconds
        };

        foreach (var group in all.GroupBy(r => r.Module))
        {
            var entry = HtmlReportWriter.WriteModuleReport(settings.ReportsPath, group.Key, _start, group.ToList());
            run.Modules.Add(entry);
        }

        HtmlReportWriter.AppendToHistoryAndWriteIndex(settings.ReportsPath, run);
        TestLog.Info($"=== Reports written to {settings.ReportsPath} ===");
    }
}
