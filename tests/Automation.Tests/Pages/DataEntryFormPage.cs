using Automation.Framework.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Automation.Tests.Pages;

/// <summary>"Data Entry Form" section: text fields, radio, checkboxes, dropdowns, and the 3 date pickers.</summary>
public class DataEntryFormPage : BasePage
{
    private static readonly By Name = By.Id("name");
    private static readonly By Email = By.Id("email");
    private static readonly By Phone = By.Id("phone");
    private static readonly By Address = By.Id("textarea");
    private static readonly By Country = By.Id("country");
    private static readonly By Colors = By.Id("colors");
    private static readonly By Animals = By.Id("animals");

    public DataEntryFormPage(IWebDriver driver) : base(driver) { }

    public void FillBasicDetails(string name, string email, string phone, string address)
    {
        Type(Name, name); Type(Email, email); Type(Phone, phone); Type(Address, address);
    }

    public (string Name, string Email, string Phone, string Address) ReadBasicDetails() =>
        (GetValue(Name), GetValue(Email), GetValue(Phone), GetValue(Address));

    public void SelectGender(string gender) => Click(By.Id(gender.ToLowerInvariant()));
    public bool IsGenderSelected(string gender) => Find(By.Id(gender.ToLowerInvariant())).Selected;

    public void SelectDay(string day) { if (!IsDaySelected(day)) Click(By.Id(day.ToLowerInvariant())); }
    public void UnselectDay(string day) { if (IsDaySelected(day)) Click(By.Id(day.ToLowerInvariant())); }
    public bool IsDaySelected(string day) => Find(By.Id(day.ToLowerInvariant())).Selected;

    public void SelectCountry(string country) => SelectByText(Country, country);
    public string SelectedCountry() => SelectedText(Country);
    public List<string> CountryOptions() => new SelectElement(Find(Country)).Options.Select(o => o.Text).ToList();

    public void SelectColors(params string[] colors)
    {
        var select = new SelectElement(Find(Colors));
        select.DeselectAll();
        // The site lists "Red" and "Green" twice, and SelectByText would pick every duplicate. Pick the first match only.
        foreach (var c in colors) select.Options.First(o => o.Text.Trim() == c).Click();
    }
    public List<string> SelectedColors() => new SelectElement(Find(Colors)).AllSelectedOptions.Select(o => o.Text).ToList();
    public List<string> ColorOptions() => new SelectElement(Find(Colors)).Options.Select(o => o.Text).ToList();

    public void SelectAnimal(string animal) => SelectByText(Animals, animal);
    public string SelectedAnimal() => SelectedText(Animals);
    public List<string> AnimalOptions() => new SelectElement(Find(Animals)).Options.Select(o => o.Text).ToList();

    // ---------- date pickers ----------
    /// <summary>Date Picker 1 (mm/dd/yyyy) - pick a day from the calendar popup.</summary>
    public string PickDateFromCalendar(int day)
    {
        Click(By.Id("datepicker"));
        Find(By.Id("ui-datepicker-div"));
        Click(By.XPath($"//div[@id='ui-datepicker-div']//a[normalize-space()='{day}']"));
        return GetValue(By.Id("datepicker"));
    }

    /// <summary>Date Picker 2 (dd/mm/yyyy) - choose month and year with the dropdowns, then the day.</summary>
    public string PickDateWithMonthYear(string month, string year, int day)
    {
        Click(By.Id("txtDate"));
        Find(By.Id("ui-datepicker-div"));
        new SelectElement(Find(By.CssSelector(".ui-datepicker-month"))).SelectByText(month);
        new SelectElement(Find(By.CssSelector(".ui-datepicker-year"))).SelectByText(year);
        Click(By.XPath($"//div[@id='ui-datepicker-div']//td[not(contains(@class,'ui-datepicker-other-month'))]/a[normalize-space()='{day}']"));
        return GetValue(By.Id("txtDate"));
    }

    /// <summary>Date Picker 3 - native date inputs. Value must be yyyy-MM-dd.</summary>
    public string SelectDateRange(string startIso, string endIso)
    {
        SetDateInput("start-date", startIso);
        SetDateInput("end-date", endIso);
        Click(By.CssSelector(".submit-btn"));
        WaitUntil(d => d.FindElement(By.Id("result")).Text.Length > 0, 5);
        return GetText(By.Id("result"));
    }

    public string SubmitRangeWithoutDates()
    {
        Click(By.CssSelector(".submit-btn"));
        WaitUntil(d => d.FindElement(By.Id("result")).Text.Length > 0, 5);
        return GetText(By.Id("result"));
    }

    private void SetDateInput(string id, string iso) =>
        Js("var e=document.getElementById(arguments[0]);e.value=arguments[1];" +
           "e.dispatchEvent(new Event('input',{bubbles:true}));e.dispatchEvent(new Event('change',{bubbles:true}));", id, iso);
}
