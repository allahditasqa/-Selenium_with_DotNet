using Automation.Framework.Core;
using Automation.Framework.Reporting;
using Automation.Tests.Pages;
using Automation.Tests.TestData;

namespace Automation.Tests.Specs;

[TestFixture, Module("Data Entry Form"), Category("Form")]
public class DataEntryFormTests : BaseTest
{
    private DataEntryFormPage _form = null!;

    [SetUp]
    public void Open()
    {
        new HomePage(Driver).Open();
        _form = new DataEntryFormPage(Driver);
    }

    [Test]
    [TestSpec("TC_FORM_001", "Verify text fields accept valid Name, Email, Phone and Address", Priority.P1_Critical, TestType.Functional,
        Pre.Home,
        "Enter a valid name in the Name field|Enter a valid email in the Email field|Enter a phone number in the Phone field|Enter an address in the Address field|Read back the value of every field",
        "Name: Allah Ditta; Email: allah.ditta@example.com; Phone: 3001234567; Address: House 10, Street 5, Islamabad, Pakistan",
        "Every field shows exactly the value that was entered")]
    public void TC_FORM_001_Fill_Text_Fields()
    {
        _form.FillBasicDetails(FormData.Name, FormData.Email, FormData.Phone, FormData.Address);
        var v = _form.ReadBasicDetails();
        Assert.Multiple(() =>
        {
            Assert.That(v.Name, Is.EqualTo(FormData.Name));
            Assert.That(v.Email, Is.EqualTo(FormData.Email));
            Assert.That(v.Phone, Is.EqualTo(FormData.Phone));
            Assert.That(v.Address, Is.EqualTo(FormData.Address));
        });
    }

    [Test]
    [TestSpec("TC_FORM_002", "Verify Gender radio buttons are mutually exclusive", Priority.P2_High, TestType.Functional,
        Pre.Home,
        "Select the 'Male' radio button|Verify Male is selected and Female is not|Select the 'Female' radio button|Verify Female is selected and Male is not",
        "Gender: Male, then Female",
        "Only one radio button is selected at any time; selecting Female deselects Male")]
    public void TC_FORM_002_Gender_Radio_Buttons_Are_Mutually_Exclusive()
    {
        _form.SelectGender("Male");
        Assert.Multiple(() =>
        {
            Assert.That(_form.IsGenderSelected("Male"), Is.True);
            Assert.That(_form.IsGenderSelected("Female"), Is.False);
        });
        _form.SelectGender("Female");
        Assert.Multiple(() =>
        {
            Assert.That(_form.IsGenderSelected("Female"), Is.True);
            Assert.That(_form.IsGenderSelected("Male"), Is.False);
        });
    }

    [Test]
    [TestSpec("TC_FORM_003", "Verify all Days checkboxes can be selected and unselected", Priority.P3_Medium, TestType.Functional,
        Pre.Home,
        "Tick all 7 day checkboxes (Sunday to Saturday)|Verify all 7 are selected|Untick all 7 checkboxes|Verify none is selected",
        "Days: Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday",
        "All checkboxes toggle independently; after unticking, no day remains selected")]
    public void TC_FORM_003_Days_Checkboxes_Select_And_Unselect_All()
    {
        foreach (var d in FormData.AllDays) _form.SelectDay(d);
        Assert.That(FormData.AllDays.All(_form.IsDaySelected), Is.True, "all days should be selected");
        Snap("all days selected");
        foreach (var d in FormData.AllDays) _form.UnselectDay(d);
        Assert.That(FormData.AllDays.Any(_form.IsDaySelected), Is.False, "no day should remain selected");
    }

    [Test]
    [TestSpec("TC_FORM_004", "Verify only the selected Days checkboxes are checked", Priority.P3_Medium, TestType.Functional,
        Pre.Home,
        "Tick 'Monday'|Tick 'Friday'|Read the state of all 7 day checkboxes",
        "Days: Monday, Friday",
        "Only Monday and Friday are selected; the other 5 days stay unselected")]
    public void TC_FORM_004_Select_Specific_Days()
    {
        _form.SelectDay("Monday"); _form.SelectDay("Friday");
        var selected = FormData.AllDays.Where(_form.IsDaySelected).ToList();
        Assert.That(selected, Is.EqualTo(new[] { "Monday", "Friday" }));
    }

    [Test]
    [TestSpec("TC_FORM_005", "Verify Country dropdown lists 10 countries and allows selection", Priority.P2_High, TestType.Functional,
        Pre.Home,
        "Read all options of the Country dropdown|Select 'India'|Read the selected option",
        "Country: India",
        "Dropdown lists United States, Canada, United Kingdom, Germany, France, Australia, Japan, China, Brazil, India; 'India' is shown as selected")]
    public void TC_FORM_005_Country_Dropdown()
    {
        Assert.That(_form.CountryOptions(), Is.EqualTo(FormData.Countries));
        _form.SelectCountry("India");
        Assert.That(_form.SelectedCountry(), Is.EqualTo("India"));
    }

    [Test]
    [TestSpec("TC_FORM_006", "Verify Colors multi-select list supports selecting multiple options", Priority.P3_Medium, TestType.Functional,
        Pre.Home,
        "Count the options of the Colors list|Select 'Red'|Select 'Green' while keeping Red selected|Read the selected options",
        "Colors: Red, Green",
        "List has 7 options; exactly Red and Green are selected")]
    public void TC_FORM_006_Colors_Multi_Select()
    {
        _form.SelectColors("Red", "Green");
        Assert.That(_form.SelectedColors(), Is.EquivalentTo(new[] { "Red", "Green" }));
        Assert.That(_form.ColorOptions(), Has.Count.EqualTo(7));
    }

    [Test]
    [TestSpec("TC_FORM_007", "Verify Sorted List dropdown is in alphabetical order and allows selection", Priority.P3_Medium, TestType.Validation,
        Pre.Home,
        "Read all options of the Sorted List dropdown|Verify they are in ascending alphabetical order|Select 'Lion'|Read the selected option",
        "Animal: Lion",
        "10 animals (Cat ... Zebra) are listed in alphabetical order; 'Lion' is selected")]
    public void TC_FORM_007_Animals_Sorted_List()
    {
        var options = _form.AnimalOptions();
        Assert.That(options, Is.EqualTo(FormData.Animals));
        Assert.That(options, Is.Ordered);
        _form.SelectAnimal("Lion");
        Assert.That(_form.SelectedAnimal(), Is.EqualTo("Lion"));
    }

    [Test]
    [TestSpec("TC_FORM_008", "Verify the complete Data Entry Form can be filled end to end", Priority.P1_Critical, TestType.Smoke,
        Pre.Home,
        "Fill Name, Email, Phone and Address|Select gender Male|Tick Saturday|Select country Japan|Select color Blue|Select animal Zebra|Verify every control holds the chosen value",
        "Name: Allah Ditta; Gender: Male; Day: Saturday; Country: Japan; Color: Blue; Animal: Zebra",
        "All fields and selections retain the entered values")]
    public void TC_FORM_008_Complete_Form_End_To_End()
    {
        _form.FillBasicDetails(FormData.Name, FormData.Email, FormData.Phone, FormData.Address);
        _form.SelectGender("Male");
        _form.SelectDay("Saturday");
        _form.SelectCountry("Japan");
        _form.SelectColors("Blue");
        _form.SelectAnimal("Zebra");
        Snap("form completed");
        Assert.Multiple(() =>
        {
            Assert.That(_form.ReadBasicDetails().Email, Is.EqualTo(FormData.Email));
            Assert.That(_form.IsGenderSelected("Male"), Is.True);
            Assert.That(_form.IsDaySelected("Saturday"), Is.True);
            Assert.That(_form.SelectedCountry(), Is.EqualTo("Japan"));
            Assert.That(_form.SelectedColors(), Is.EqualTo(new[] { "Blue" }));
            Assert.That(_form.SelectedAnimal(), Is.EqualTo("Zebra"));
        });
    }
}
