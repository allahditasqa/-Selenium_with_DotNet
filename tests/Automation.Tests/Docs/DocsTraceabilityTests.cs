using System.Reflection;
using System.Text.RegularExpressions;
using Automation.Framework.Reporting;

namespace Automation.Tests.Docs;

/// <summary>
/// Traceability check (no browser): the automated tests and the written test cases in
/// docs/test-cases must match 1:1 - same ID, title, priority, type and test method.
/// If a test is added, renamed or removed without updating the documents, this fixture fails.
/// Run alone with: dotnet test --filter "Category=Docs"
/// </summary>
[TestFixture, Category("Docs")]
public class DocsTraceabilityTests
{
    private static readonly Regex DocRow = new(@"^\| (TC_[A-Z0-9]+_\d+) \|", RegexOptions.Compiled);

    private static string RepoRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null && !File.Exists(Path.Combine(dir.FullName, "SeleniumDotNet.sln"))) dir = dir.Parent;
        return dir?.FullName ?? throw new DirectoryNotFoundException("SeleniumDotNet.sln not found above " + AppContext.BaseDirectory);
    }

    /// <summary>Minimal CSV reader (quoted fields, doubled quotes); the master CSV files are written by scripts/Build-CaseCsv.ps1.</summary>
    private static List<List<string>> ReadCsv(string path)
    {
        var rows = new List<List<string>>();
        var row = new List<string>();
        var cell = new System.Text.StringBuilder();
        var text = File.ReadAllText(path).TrimStart('\uFEFF');
        bool quoted = false;
        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];
            if (quoted)
            {
                if (c == '"' && i + 1 < text.Length && text[i + 1] == '"') { cell.Append('"'); i++; }
                else if (c == '"') quoted = false;
                else cell.Append(c);
            }
            else if (c == '"') quoted = true;
            else if (c == ',') { row.Add(cell.ToString()); cell.Clear(); }
            else if (c == '\n') { row.Add(cell.ToString().TrimEnd('\r')); cell.Clear(); rows.Add(row); row = new List<string>(); }
            else cell.Append(c);
        }
        if (cell.Length > 0 || row.Count > 0) { row.Add(cell.ToString()); rows.Add(row); }
        return rows;
    }

    [Test]
    public void Automated_Tests_And_Documented_Test_Cases_Match()
    {
        var root = RepoRoot();
        var problems = new List<string>();

        // ---- documented cases (master CSV)
        var docs = new Dictionary<string, Dictionary<string, string>>();
        foreach (var file in Directory.GetFiles(Path.Combine(root, "docs", "test-cases"), "*.csv"))
        {
            var rows = ReadCsv(file);
            var header = rows[0];
            foreach (var r in rows.Skip(1).Where(r => r.Count == header.Count))
            {
                var map = header.Zip(r, (h, v) => (h, v)).ToDictionary(x => x.h, x => x.v);
                map["_doc"] = Path.GetFileName(file);
                if (!docs.TryAdd(map["ID"], map)) problems.Add($"{map["ID"]}: documented twice");
            }
        }

        // ---- every .md document row must be in the master CSV (and vice versa)
        var mdIds = Directory.GetFiles(Path.Combine(root, "docs", "test-cases"), "*.md")
            .SelectMany(File.ReadAllLines).Select(l => DocRow.Match(l)).Where(m => m.Success).Select(m => m.Groups[1].Value).ToList();
        foreach (var id in mdIds.Except(docs.Keys)) problems.Add($"{id}: in a .md document but not in the master CSV (run scripts/Build-CaseCsv.ps1)");
        foreach (var id in docs.Keys.Except(mdIds)) problems.Add($"{id}: in the master CSV but not in a .md document");

        // ---- automated tests
        var code = new Dictionary<string, (TestSpecAttribute Spec, MethodInfo Method)>();
        foreach (var method in typeof(DocsTraceabilityTests).Assembly.GetTypes().SelectMany(t => t.GetMethods()))
        {
            var spec = method.GetCustomAttribute<TestSpecAttribute>();
            if (spec == null) continue;
            if (!code.TryAdd(spec.Id, (spec, method))) problems.Add($"{spec.Id}: used by more than one test method");
            if (!method.Name.StartsWith(spec.Id + "_")) problems.Add($"{spec.Id}: method name '{method.Name}' must start with '{spec.Id}_'");
        }

        foreach (var id in code.Keys.Except(docs.Keys).OrderBy(x => x)) problems.Add($"{id}: automated in {code[id].Method.Name} but NOT documented");
        foreach (var id in docs.Keys.Except(code.Keys).OrderBy(x => x)) problems.Add($"{id}: documented in {docs[id]["_doc"]} but NOT automated");
        foreach (var id in code.Keys.Intersect(docs.Keys).OrderBy(x => x))
        {
            var (spec, method) = code[id];
            var doc = docs[id];
            var methodRef = $"{method.DeclaringType!.Name}.{method.Name}";
            if (spec.Title != doc["Title"]) problems.Add($"{id}: title differs (code '{spec.Title}' / doc '{doc["Title"]}')");
            if (spec.PriorityLabel != doc["Priority"]) problems.Add($"{id}: priority differs (code '{spec.PriorityLabel}' / doc '{doc["Priority"]}')");
            if (spec.Type.ToString() != doc["Type"]) problems.Add($"{id}: type differs (code '{spec.Type}' / doc '{doc["Type"]}')");
            if (spec.NumberedSteps.Replace("\n", " ") != doc["Steps"]) problems.Add($"{id}: steps differ between code and document");
            if (spec.ExpectedResult != doc["Expected Result"]) problems.Add($"{id}: expected result differs between code and document");
            if (methodRef != doc["Test Method"]) problems.Add($"{id}: test method differs (code '{methodRef}' / doc '{doc["Test Method"]}')");
            if (!File.Exists(Path.Combine(root, doc["Test File"]))) problems.Add($"{id}: test file '{doc["Test File"]}' does not exist");
        }

        TestContext.Out.WriteLine($"tests in code: {code.Count} | cases in docs: {docs.Count}");
        Assert.That(problems, Is.Empty, "Traceability problems:\n - " + string.Join("\n - ", problems));
    }
}
