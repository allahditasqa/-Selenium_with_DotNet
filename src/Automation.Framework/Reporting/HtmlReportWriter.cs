using System.Net;
using System.Text;
using System.Text.Json;

namespace Automation.Framework.Reporting;

public static class HtmlReportWriter
{
    private static readonly object IndexLock = new();
    private static string E(string? s) => WebUtility.HtmlEncode(s ?? "");

    // ------------------------------------------------------------------ module report
    public static ModuleRunEntry WriteModuleReport(string reportsRoot, string module, DateTime runStart, List<TestRecord> tests)
    {
        var slug = Slug(module);
        var folder = Path.Combine(reportsRoot, slug);
        Directory.CreateDirectory(folder);

        // Unique file names: "<module> test dd-MM-yyyy_HH-mm-ss". Never reuse an existing file.
        var stamp = runStart.ToString("dd-MM-yyyy_HH-mm-ss");
        var htmlPath = UniquePath(folder, $"{module} test {stamp}", ".html");
        var csvPath = Path.ChangeExtension(htmlPath, ".csv").Replace(" test ", " test cases ");
        csvPath = UniquePath(folder, Path.GetFileNameWithoutExtension(csvPath), ".csv");

        int passed = tests.Count(t => t.Status == "passed");
        int failed = tests.Count(t => t.Status == "failed");
        int skipped = tests.Count(t => t.Status == "skipped");
        double duration = tests.Sum(t => t.DurationSeconds);
        var overall = failed > 0 ? "failed" : "passed";

        var sb = new StringBuilder();
        sb.Append("<p class=\"meta\"><a href=\"../index.html\">&larr; All reports</a></p>");
        sb.Append($"<h1>{E(module)}</h1>");
        sb.Append($"<div class=\"meta\">Run: {runStart:dd-MM-yyyy HH:mm} · duration {duration:0}s · overall: {overall} · click a screenshot to enlarge</div>");
        sb.Append("<div class=\"sum\">")
          .Append($"<div><b>{tests.Count}</b>total</div>")
          .Append($"<div><b class=\"passed-t\">{passed}</b>passed</div>")
          .Append($"<div><b class=\"failed-t\">{failed}</b>failed</div>")
          .Append($"<div><b class=\"skipped-t\">{skipped}</b>skipped</div></div>");

        foreach (var t in tests)
        {
            sb.Append($"<section class=\"card {t.Status}\"><div class=\"head\">")
              .Append($"<span class=\"badge {t.Status}\">{t.Status}</span><b>{E(t.Title)}</b>")
              .Append($"<span class=\"meta\">{E(t.ClassName)} › {E(t.TestName)} · {t.DurationSeconds:0.0}s</span></div>");
            if (t.Spec is { } sp)
            {
                sb.Append($"<div class=\"meta\">{E(sp.PriorityLabel)} · {sp.Type}</div>");
                sb.Append("<details><summary class=\"meta\">Test case details</summary><table>")
                  .Append($"<tr><th>Preconditions</th><td>{E(sp.Preconditions)}</td></tr>")
                  .Append($"<tr><th>Test steps</th><td>{string.Join("<br>", sp.StepList.Select((x, i) => $"{i + 1}. {E(x)}"))}</td></tr>")
                  .Append($"<tr><th>Test data</th><td>{E(sp.TestData)}</td></tr>")
                  .Append($"<tr><th>Expected result</th><td>{E(sp.ExpectedResult)}</td></tr></table></details>");
            }
            if (!string.IsNullOrWhiteSpace(t.Message))
                sb.Append($"<pre>{E(t.Message)}{(string.IsNullOrWhiteSpace(t.StackTrace) ? "" : "\n\n" + E(t.StackTrace))}</pre>");
            if (t.Screenshots.Count > 0)
            {
                sb.Append("<div class=\"shots\">");
                foreach (var (caption, b64) in t.Screenshots)
                    sb.Append($"<figure><img src=\"data:image/png;base64,{b64}\" alt=\"{E(caption)}\" onclick=\"this.classList.toggle('big')\"><figcaption>{E(caption)}</figcaption></figure>");
                sb.Append("</div>");
            }
            sb.Append("</section>");
        }

        File.WriteAllText(htmlPath, ReportTemplate.Page($"{module} Test Report {runStart:dd-MM-yyyy HH:mm}", sb.ToString()), new UTF8Encoding(false));

        // CSV with the test cases of this module/run
        // Every executed test case of this module in this run (Title format: "TC_ID - description").
        // Standard test-case sheet: one row per executed test case.
        var settings = Config.FrameworkSettings.Current;
        var csv = new StringBuilder("Test Case ID,Module,Test Case Title,Priority,Test Type,Preconditions,Test Steps,Test Data," +
                                    "Expected Result,Actual Result,Execution Status,Execution Type,Automation Method,Duration (s)," +
                                    "Executed On,Browser,Environment (URL)\n");
        foreach (var t in tests)
        {
            var s = t.Spec;
            var actual = t.Status == "passed" ? "As expected (all assertions passed)"
                       : t.Status == "failed" ? t.Message : "Test was not executed: " + t.Message;
            csv.AppendLine(string.Join(",",
                Csv(s?.Id ?? t.TestName), Csv(module), Csv(s?.Title ?? t.Title),
                Csv(s?.PriorityLabel ?? ""), Csv(s?.Type.ToString() ?? ""),
                Csv(s?.Preconditions ?? ""), Csv(s?.NumberedSteps ?? ""), Csv(s?.TestData ?? ""),
                Csv(s?.ExpectedResult ?? ""), Csv(actual),
                Csv(t.Status switch { "passed" => "Pass", "failed" => "Fail", _ => "Skipped" }),
                Csv("Automated"), Csv($"{t.ClassName}.{t.TestName}"), t.DurationSeconds.ToString("0.0"),
                Csv(runStart.ToString("dd-MM-yyyy HH:mm:ss")), Csv(settings.Browser), Csv(settings.BaseUrl)));
        }
        File.WriteAllText(csvPath, csv.ToString(), new UTF8Encoding(true));

        return new ModuleRunEntry
        {
            Module = module,
            ReportPath = Rel(reportsRoot, htmlPath),
            CsvPath = Rel(reportsRoot, csvPath),
            Total = tests.Count, Passed = passed, Failed = failed, Skipped = skipped, DurationSeconds = duration
        };
    }

    // ------------------------------------------------------------------ history + index
    public static void AppendToHistoryAndWriteIndex(string reportsRoot, RunEntry run)
    {
        lock (IndexLock)
        {
            Directory.CreateDirectory(reportsRoot);
            var historyFile = Path.Combine(reportsRoot, "history.json");
            var history = new List<RunEntry>();
            if (File.Exists(historyFile))
            {
                try { history = JsonSerializer.Deserialize<List<RunEntry>>(File.ReadAllText(historyFile)) ?? new(); }
                catch (JsonException)
                {
                    // Corrupt file: keep it as a backup instead of losing anything.
                    File.Move(historyFile, UniquePath(reportsRoot, "history.corrupt", ".json"));
                }
            }
            history.Add(run);

            var tmp = historyFile + ".tmp";
            File.WriteAllText(tmp, JsonSerializer.Serialize(history, new JsonSerializerOptions { WriteIndented = true }));
            File.Move(tmp, historyFile, overwrite: true);   // history is append-only; the file is rewritten with all old runs kept

            File.WriteAllText(Path.Combine(reportsRoot, "index.html"), RenderIndex(history), new UTF8Encoding(false));
        }
    }

    private static string RenderIndex(List<RunEntry> history)
    {
        var sb = new StringBuilder();
        sb.Append("<h1>Selenium .NET - Test Reports</h1>");
        sb.Append("<div class=\"meta\">History: newest run on top. Under each date-time you see the module-wise reports. Old runs are never overwritten or deleted.</div>");

        foreach (var run in history.OrderByDescending(r => r.StartedAt))
        {
            int total = run.Modules.Sum(m => m.Total), p = run.Modules.Sum(m => m.Passed),
                f = run.Modules.Sum(m => m.Failed), s = run.Modules.Sum(m => m.Skipped);
            sb.Append($"<h2>{run.StartedAt:dd-MM-yyyy}  •  {run.StartedAt:HH:mm:ss} <span class=\"meta\">&nbsp;total {total} · " +
                      $"<span class=\"passed-t\">{p} pass</span> · <span class=\"{(f > 0 ? "failed-t" : "")}\">{f} fail</span> · " +
                      $"<span class=\"skipped-t\">{s} skip</span> · {run.DurationSeconds:0}s</span></h2>");
            sb.Append("<table><thead><tr><th>Module</th><th>Total</th><th>Pass</th><th>Fail</th><th>Skip</th><th>Result</th><th>Report</th><th>Test cases</th></tr></thead><tbody>");
            foreach (var m in run.Modules.OrderBy(m => m.Module))
            {
                var ok = m.Failed == 0;
                sb.Append($"<tr><td>{E(m.Module)}</td><td>{m.Total}</td><td class=\"passed-t\">{m.Passed}</td>" +
                          $"<td class=\"{(m.Failed > 0 ? "failed-t" : "")}\">{m.Failed}</td><td class=\"skipped-t\">{m.Skipped}</td>" +
                          $"<td class=\"{(ok ? "passed-t" : "failed-t")}\">{(ok ? "Passed" : "Failed")}</td>" +
                          $"<td><a href=\"{Uri.EscapeDataString(m.ReportPath).Replace("%2F", "/")}\">HTML</a></td>" +
                          $"<td><a href=\"{Uri.EscapeDataString(m.CsvPath).Replace("%2F", "/")}\">CSV</a></td></tr>");
            }
            sb.Append("</tbody></table>");
        }
        return ReportTemplate.Page("Selenium .NET - Test Reports", sb.ToString());
    }

    // ------------------------------------------------------------------ helpers
    private static string UniquePath(string folder, string baseName, string ext)
    {
        var path = Path.Combine(folder, baseName + ext);
        for (int i = 2; File.Exists(path); i++) path = Path.Combine(folder, $"{baseName} ({i}){ext}");
        return path;
    }

    private static string Rel(string root, string full) => Path.GetRelativePath(root, full).Replace('\\', '/');

    private static string Slug(string s) =>
        new string(s.ToLowerInvariant().Select(c => char.IsLetterOrDigit(c) ? c : '-').ToArray()).Trim('-');

    private static string Csv(string? v) => "\"" + (v ?? "").Replace("\"", "\"\"").Replace("\r", " ").Replace("\n", " ") + "\"";
}
