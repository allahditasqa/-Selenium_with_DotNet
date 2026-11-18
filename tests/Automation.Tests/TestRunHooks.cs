using Automation.Framework.Reporting;

[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: LevelOfParallelism(4)]

namespace Automation.Tests;

/// <summary>Runs once per `dotnet test`: starts result collection and writes the reports when everything is done.</summary>
[SetUpFixture]
public class TestRunHooks
{
    [OneTimeSetUp]
    public void RunStarted() => ReportSession.Start();

    [OneTimeTearDown]
    public void RunFinished() => ReportSession.Finish();
}
