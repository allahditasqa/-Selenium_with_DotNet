namespace Automation.Framework.Reporting;

public sealed class TestRecord
{
    public string Module { get; init; } = "";
    public string ClassName { get; init; } = "";
    public string TestName { get; init; } = "";
    public string Title { get; init; } = "";
    public string Status { get; init; } = "passed";   // passed | failed | skipped
    public double DurationSeconds { get; init; }
    public TestSpecAttribute? Spec { get; init; }
    public string? Message { get; init; }
    public string? StackTrace { get; init; }
    public List<(string Caption, string Base64Png)> Screenshots { get; } = new();
}

/// <summary>One row of history.json: a module report produced in a given run.</summary>
public sealed class ModuleRunEntry
{
    public string Module { get; set; } = "";
    public string ReportPath { get; set; } = "";
    public string CsvPath { get; set; } = "";
    public int Total { get; set; }
    public int Passed { get; set; }
    public int Failed { get; set; }
    public int Skipped { get; set; }
    public double DurationSeconds { get; set; }
}

/// <summary>One test run (one `dotnet test` execution) in history.json.</summary>
public sealed class RunEntry
{
    public string RunId { get; set; } = "";
    public DateTime StartedAt { get; set; }
    public double DurationSeconds { get; set; }
    public List<ModuleRunEntry> Modules { get; set; } = new();
}
