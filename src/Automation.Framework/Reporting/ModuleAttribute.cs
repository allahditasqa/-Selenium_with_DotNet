namespace Automation.Framework.Reporting;

/// <summary>Tags a test class with the module it belongs to. Reports are grouped by this name.</summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class ModuleAttribute : Attribute
{
    public string Name { get; }
    public ModuleAttribute(string name) => Name = name;
}
