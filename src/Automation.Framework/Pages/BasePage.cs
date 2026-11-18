using Automation.Framework.Config;
using Automation.Framework.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace Automation.Framework.Pages;

/// <summary>Base class for every Page Object. Holds the driver and all reusable, wait-safe actions.</summary>
public abstract class BasePage
{
    protected readonly IWebDriver Driver;
    protected readonly WebDriverWait Wait;

    protected BasePage(IWebDriver driver)
    {
        Driver = driver;
        Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(FrameworkSettings.Current.ExplicitWaitSeconds))
        {
            PollingInterval = TimeSpan.FromMilliseconds(200)
        };
        Wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException),
            typeof(ElementNotInteractableException));
    }

    // ---------- finding ----------
    protected IWebElement Find(By by) => Wait.Until(d =>
    {
        var e = d.FindElement(by);
        return e.Displayed ? e : null;
    })!;

    protected IWebElement FindPresent(By by) => Wait.Until(d => d.FindElement(by));
    protected IReadOnlyList<IWebElement> FindAll(By by) => Driver.FindElements(by);

    protected IWebElement Clickable(By by) => Wait.Until(d =>
    {
        var e = d.FindElement(by);
        return e.Displayed && e.Enabled ? e : null;
    })!;

    // ---------- actions ----------
    protected void Click(By by)
    {
        var el = Clickable(by);
        ScrollTo(el);
        try { el.Click(); }
        catch (ElementClickInterceptedException) { JsClick(el); }
        TestLog.Debug($"Click {by}");
    }

    protected void Type(By by, string text)
    {
        var el = Find(by);
        ScrollTo(el);
        el.Clear();
        el.SendKeys(text);
        TestLog.Debug($"Type '{text}' into {by}");
    }

    protected string GetText(By by) => Find(by).Text.Trim();
    protected string GetValue(By by) => Find(by).GetDomProperty("value") ?? string.Empty;
    protected bool IsDisplayed(By by) => Driver.FindElements(by).Any(e => e.Displayed);

    protected void SelectByText(By by, string text) => new SelectElement(Find(by)).SelectByText(text);
    protected string SelectedText(By by) => new SelectElement(Find(by)).SelectedOption.Text;

    protected void ScrollTo(IWebElement el) =>
        ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", el);

    protected void JsClick(IWebElement el) =>
        ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", el);

    protected object? Js(string script, params object[] args) =>
        ((IJavaScriptExecutor)Driver).ExecuteScript(script, args);

    protected Actions Act() => new(Driver);

    // ---------- waits ----------
    protected bool WaitUntil(Func<IWebDriver, bool> condition, int seconds = 10)
    {
        try
        {
            new WebDriverWait(Driver, TimeSpan.FromSeconds(seconds)).Until(d => condition(d));
            return true;
        }
        catch (WebDriverTimeoutException) { return false; }
    }

    // ---------- alerts ----------
    public IAlert WaitForAlert() => Wait.Until(d =>
    {
        try { return d.SwitchTo().Alert(); }
        catch (NoAlertPresentException) { return null; }
    })!;

    // ---------- windows ----------
    public string SwitchToNewWindow(string originalHandle)
    {
        Wait.Until(d => d.WindowHandles.Count > 1);
        var newHandle = Driver.WindowHandles.First(h => h != originalHandle);
        Driver.SwitchTo().Window(newHandle);
        return newHandle;
    }

    public void CloseOtherWindows(string keepHandle)
    {
        foreach (var h in Driver.WindowHandles.Where(h => h != keepHandle).ToList())
        {
            Driver.SwitchTo().Window(h);
            Driver.Close();
        }
        Driver.SwitchTo().Window(keepHandle);
    }

    public string Title => Driver.Title;
    public string CurrentUrl => Driver.Url;
}
