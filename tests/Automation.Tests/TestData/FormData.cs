namespace Automation.Tests.TestData;

/// <summary>Central place for test data so tests stay free of magic strings.</summary>
public static class FormData
{
    public const string Name = "Allah Ditta";
    public const string Email = "allah.ditta@example.com";
    public const string Phone = "3001234567";   // the Phone field accepts at most 10 digits
    public const string Address = "House 10, Street 5, Islamabad, Pakistan";

    public static readonly string[] AllDays = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
    public static readonly string[] Countries =
        { "United States", "Canada", "United Kingdom", "Germany", "France", "Australia", "Japan", "China", "Brazil", "India" };
    public static readonly string[] Animals =
        { "Cat", "Cheetah", "Deer", "Dog", "Elephant", "Fox", "Giraffe", "Lion", "Rabbit", "Zebra" };
}
