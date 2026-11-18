using Automation.Framework.Pages;
using OpenQA.Selenium;

namespace Automation.Tests.Pages;

/// <summary>Simple / confirm / prompt alerts, new tab and popup windows.</summary>
public class AlertsAndWindowsPage : BasePage
{
    public AlertsAndWindowsPage(IWebDriver driver) : base(driver) { }

    public string ResultText => GetText(By.Id("demo"));

    public string OpenSimpleAlertAndAccept()
    {
        Click(By.Id("alertBtn"));
        var alert = WaitForAlert();
        var text = alert.Text;
        alert.Accept();
        return text;
    }

    public string ConfirmAlert(bool accept)
    {
        Click(By.Id("confirmBtn"));
        var alert = WaitForAlert();
        var text = alert.Text;
        if (accept) alert.Accept(); else alert.Dismiss();
        return text;
    }

    public string PromptAlert(string? input, bool accept)
    {
        Click(By.Id("promptBtn"));
        var alert = WaitForAlert();
        var text = alert.Text;
        if (input != null) alert.SendKeys(input);
        if (accept) alert.Accept(); else alert.Dismiss();
        return text;
    }

    public string OpenNewTab()
    {
        var original = Driver.CurrentWindowHandle;
        Click(By.XPath("//button[normalize-space()='New Tab']"));
        SwitchToNewWindow(original);
        return original;
    }

    public (string Original, int Count) OpenPopupWindows()
    {
        var original = Driver.CurrentWindowHandle;
        Click(By.Id("PopUp"));
        WaitUntil(d => d.WindowHandles.Count > 1, 15);
        return (original, Driver.WindowHandles.Count);
    }
}
