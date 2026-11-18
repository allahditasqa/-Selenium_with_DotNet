using Automation.Framework.Core;
using Automation.Framework.Reporting;
using Automation.Tests.Pages;
using Automation.Tests.TestData;

namespace Automation.Tests.Specs;

[TestFixture, Module("Date Pickers"), Category("DatePicker")]
public class DatePickerTests : BaseTest
{
    private DataEntryFormPage _page = null!;

    [SetUp]
    public void Open()
    {
        new HomePage(Driver).Open();
        _page = new DataEntryFormPage(Driver);
    }

    [Test]
    [TestSpec("TC_DATE_001", "Verify Date Picker 1 (mm/dd/yyyy) selects a date from the calendar popup", Priority.P2_High, TestType.Functional,
        Pre.Home,
        "Click the 'Date Picker 1' field|Verify the calendar popup opens|Click day 15 in the calendar|Read the field value",
        "Day: 15 (current month)",
        "Field shows the date in mm/dd/yyyy format with day 15")]
    public void DatePicker1_Pick_From_Calendar()
    {
        var value = _page.PickDateFromCalendar(15);
        Assert.That(value, Does.Match(@"^\d{2}/15/\d{4}$"), "format must be mm/15/yyyy");
    }

    [Test]
    [TestSpec("TC_DATE_002", "Verify Date Picker 2 (dd/mm/yyyy) selects a date using the month and year dropdowns", Priority.P2_High, TestType.Functional,
        Pre.Home,
        "Click the 'Date Picker 2' field|Select month 'Mar' from the month dropdown|Select year '2025' from the year dropdown|Click day 10|Read the field value",
        "Month: Mar; Year: 2025; Day: 10",
        "Field shows 10/03/2025 (dd/mm/yyyy)")]
    public void DatePicker2_Select_Month_And_Year()
    {
        var value = _page.PickDateWithMonthYear("Mar", "2025", 10);
        Assert.That(value, Is.EqualTo("10/03/2025"));
    }

    [Test]
    [TestSpec("TC_DATE_003", "Verify Date Picker 3 calculates the number of days for a valid date range", Priority.P1_Critical, TestType.Functional,
        Pre.Home,
        "Enter Start Date|Enter End Date after the start date|Click Submit|Read the result message",
        "Start: 01-Jan-2026; End: 31-Jan-2026",
        "Message 'You selected a range of 30 days.' is displayed")]
    public void DatePicker3_Valid_Range()
    {
        var result = _page.SelectDateRange("2026-01-01", "2026-01-31");
        Assert.That(result, Is.EqualTo("You selected a range of 30 days."));
    }

    [Test]
    [TestSpec("TC_DATE_004", "Verify Date Picker 3 rejects an end date earlier than the start date", Priority.P2_High, TestType.Negative,
        Pre.Home,
        "Enter Start Date|Enter an End Date earlier than the start date|Click Submit|Read the result message",
        "Start: 10-Feb-2026; End: 01-Feb-2026",
        "Validation message 'End date must be after start date.' is displayed")]
    public void DatePicker3_End_Before_Start()
    {
        var result = _page.SelectDateRange("2026-02-10", "2026-02-01");
        Assert.That(result, Is.EqualTo("End date must be after start date."));
    }

    [Test]
    [TestSpec("TC_DATE_005", "Verify Date Picker 3 requires both dates before submitting", Priority.P2_High, TestType.Negative,
        Pre.Home,
        "Leave Start Date and End Date empty|Click Submit|Read the result message",
        "Start: (empty); End: (empty)",
        "Message 'Please select both start and end dates.' is displayed")]
    public void DatePicker3_Missing_Dates()
    {
        Assert.That(_page.SubmitRangeWithoutDates(), Is.EqualTo("Please select both start and end dates."));
    }
}
