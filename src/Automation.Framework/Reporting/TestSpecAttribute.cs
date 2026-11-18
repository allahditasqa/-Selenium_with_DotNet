namespace Automation.Framework.Reporting;

public enum Priority { P1_Critical, P2_High, P3_Medium, P4_Low }

public enum TestType { Functional, Negative, Validation, UI, Integration, Smoke, Compatibility }

/// <summary>
/// The written test case (what a manual QA would put in a test-case sheet) kept right on the automated test.
/// Reports and CSV files are generated from it, so documentation and automation never drift apart.
/// Separate the steps with '|'.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class TestSpecAttribute : Attribute
{
    public string Id { get; }
    public string Title { get; }
    public Priority Priority { get; }
    public TestType Type { get; }
    public string Preconditions { get; }
    public string Steps { get; }
    public string TestData { get; }
    public string ExpectedResult { get; }

    public TestSpecAttribute(string id, string title, Priority priority, TestType type,
        string preconditions, string steps, string testData, string expectedResult)
    {
        Id = id; Title = title; Priority = priority; Type = type;
        Preconditions = preconditions; Steps = steps; TestData = testData; ExpectedResult = expectedResult;
    }

    public string PriorityLabel => Priority switch
    {
        Priority.P1_Critical => "P1 - Critical",
        Priority.P2_High => "P2 - High",
        Priority.P3_Medium => "P3 - Medium",
        _ => "P4 - Low"
    };

    public IEnumerable<string> StepList => Steps.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    public string NumberedSteps => string.Join("\n", StepList.Select((s, i) => $"{i + 1}. {s}"));
}
