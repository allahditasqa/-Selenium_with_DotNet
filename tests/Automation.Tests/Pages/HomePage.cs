using Automation.Framework.Config;
using Automation.Framework.Pages;
using OpenQA.Selenium;

namespace Automation.Tests.Pages;

/// <summary>Top of the practice site: opening the page, header nav links, footer links, visitor stats.</summary>
public class HomePage : BasePage
{
    private static readonly By NavLinks = By.CssSelector("#PageList2 a");
    private static readonly By FooterLinks = By.CssSelector("#footer-2-2 a, #PageList1 a, #HTML16 a");
    private static readonly By TotalVisitors = By.Id("Stats1_totalCount");

    public HomePage(IWebDriver driver) : base(driver) { }

    public HomePage Open()
    {
        Driver.Navigate().GoToUrl(FrameworkSettings.Current.BaseUrl);
        Find(By.Id("name"));          // page is ready when the first form field is visible
        return this;
    }

    public void OpenUrl(string url) => Driver.Navigate().GoToUrl(url);

    public Dictionary<string, string> HeaderLinks() =>
        FindAll(NavLinks).Where(a => !string.IsNullOrWhiteSpace(a.Text))
            .ToDictionary(a => a.Text.Trim(), a => a.GetDomProperty("href") ?? "", StringComparer.OrdinalIgnoreCase);

    public Dictionary<string, string> FooterNavLinks() =>
        FindAll(By.CssSelector("a")).Where(a => new[] { "Hidden Elements & AJAX", "Download Files", "Youtube" }.Contains(a.Text.Trim()))
            .GroupBy(a => a.Text.Trim()).ToDictionary(g => g.Key, g => g.First().GetDomProperty("href") ?? "");

    public void ClickFooterLink(string text) => Click(By.XPath($"//a[normalize-space()='{text}']"));

    public string TotalVisitorCount()
    {
        WaitUntil(d => d.FindElement(TotalVisitors).Text.Trim().Length > 0, 15);
        return FindPresent(TotalVisitors).Text.Trim();
    }

    public int VisitorTableRows() => FindAll(By.CssSelector("#Stats1 table tr, .widget.Stats table tr")).Count;
}
