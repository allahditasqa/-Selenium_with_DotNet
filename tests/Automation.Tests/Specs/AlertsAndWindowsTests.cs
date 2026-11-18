using Automation.Framework.Core;
using Automation.Framework.Reporting;
using Automation.Tests.Pages;
using Automation.Tests.TestData;

namespace Automation.Tests.Specs;

[TestFixture, Module("Alerts and Windows"), Category("Alerts")]
public class AlertsAndWindowsTests : BaseTest
{
    private AlertsAndWindowsPage _page = null!;

    [SetUp]
    public void Open()
    {
        new HomePage(Driver).Open();
        _page = new AlertsAndWindowsPage(Driver);
    }

    [Test]
    [TestSpec("TC_ALERT_001", "Verify the Simple Alert shows its message and closes on OK", Priority.P2_High, TestType.Functional,
        Pre.Home,
        "Click 'Simple Alert'|Read the alert text|Click OK",
        "-",
        "Alert text is 'I am an alert box!'; the alert closes after OK")]
    public void Simple_Alert()
    {
        Assert.That(_page.OpenSimpleAlertAndAccept(), Is.EqualTo("I am an alert box!"));
    }

    [Test]
    [TestSpec("TC_ALERT_002", "Verify Confirmation Alert - OK shows 'You pressed OK!'", Priority.P2_High, TestType.Functional,
        Pre.Home,
        "Click 'Confirmation Alert'|Click OK in the alert|Read the result text on the page",
        "Action: OK",
        "Page shows 'You pressed OK!'")]
    public void Confirm_Alert_Accept()
    {
        Assert.That(_page.ConfirmAlert(accept: true), Is.EqualTo("Press a button!"));
        Assert.That(_page.ResultText, Is.EqualTo("You pressed OK!"));
    }

    [Test]
    [TestSpec("TC_ALERT_003", "Verify Confirmation Alert - Cancel shows 'You pressed Cancel!'", Priority.P2_High, TestType.Functional,
        Pre.Home,
        "Click 'Confirmation Alert'|Click Cancel in the alert|Read the result text on the page",
        "Action: Cancel",
        "Page shows 'You pressed Cancel!'")]
    public void Confirm_Alert_Dismiss()
    {
        _page.ConfirmAlert(accept: false);
        Assert.That(_page.ResultText, Is.EqualTo("You pressed Cancel!"));
    }

    [Test]
    [TestSpec("TC_ALERT_004", "Verify Prompt Alert greets the user with the entered name", Priority.P2_High, TestType.Functional,
        Pre.Home,
        "Click 'Prompt Alert'|Type a name in the prompt|Click OK|Read the result text on the page",
        "Name: Allah Ditta",
        "Result text contains 'Allah Ditta'")]
    public void Prompt_Alert_Accept_With_Text()
    {
        _page.PromptAlert("Allah Ditta", accept: true);
        Assert.That(_page.ResultText, Does.Contain("Allah Ditta"));
    }

    [Test]
    [TestSpec("TC_ALERT_005", "Verify Prompt Alert - Cancel does not show a greeting", Priority.P3_Medium, TestType.Negative,
        Pre.Home,
        "Click 'Prompt Alert'|Click Cancel in the prompt|Read the result text on the page",
        "Action: Cancel",
        "No 'Hello ...' greeting is displayed")]
    public void Prompt_Alert_Dismiss()
    {
        _page.PromptAlert(null, accept: false);
        Assert.That(_page.ResultText, Does.Not.Contain("Hello"));
    }

    [Test]
    [TestSpec("TC_WINDOW_001", "Verify the 'New Tab' button opens the target site in a new browser tab", Priority.P2_High, TestType.Functional,
        Pre.Home,
        "Click 'New Tab'|Switch to the newly opened tab|Read its URL|Close the new tab and return to the original tab",
        "Expected site: pavantestingtools.com",
        "A new tab opens with pavantestingtools.com; after closing it only the original tab remains")]
    public void New_Tab()
    {
        var original = _page.OpenNewTab();
        Assert.That(_page.CurrentUrl, Does.Contain("pavantestingtools.com"));
        Snap("new tab");
        _page.CloseOtherWindows(original);
        Assert.That(Driver.WindowHandles, Has.Count.EqualTo(1));
    }

    [Test]
    [TestSpec("TC_WINDOW_002", "Verify the 'Popup Windows' button opens additional browser windows", Priority.P3_Medium, TestType.Functional,
        Pre.Home,
        "Click 'Popup Windows'|Count open windows|Close all popup windows",
        "-",
        "At least one extra window opens; after closing them only the original window remains")]
    public void Popup_Windows()
    {
        var (original, count) = _page.OpenPopupWindows();
        Assert.That(count, Is.GreaterThan(1), "at least one popup window must open");
        _page.CloseOtherWindows(original);
        Assert.That(Driver.WindowHandles, Has.Count.EqualTo(1));
    }
}
