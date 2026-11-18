namespace Automation.Tests.TestData;

/// <summary>Creates small throw-away files for upload tests.</summary>
public static class TempFiles
{
    public static string Create(string fileName, string content = "automation test file")
    {
        var dir = Path.Combine(Path.GetTempPath(), "selenium-dotnet", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        var path = Path.Combine(dir, fileName);
        File.WriteAllText(path, content);
        return path;
    }
}
