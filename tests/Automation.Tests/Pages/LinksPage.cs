using System.Net;
using Automation.Framework.Pages;
using OpenQA.Selenium;

namespace Automation.Tests.Pages;

/// <summary>Labels, normal links and broken links.</summary>
public class LinksPage : BasePage
{
    private static readonly HttpClient Http = new(new HttpClientHandler { AllowAutoRedirect = true })
    { Timeout = TimeSpan.FromSeconds(20) };

    public LinksPage(IWebDriver driver) : base(driver) { }

    public List<string> MobileLabels() => FindAll(By.CssSelector("#mobiles label")).Select(l => l.Text.Trim()).ToList();

    public Dictionary<string, string> LaptopLinks() =>
        FindAll(By.CssSelector("#laptops a")).ToDictionary(a => a.Text.Trim(), a => a.GetDomProperty("href") ?? "");

    public Dictionary<string, string> BrokenLinks() =>
        FindAll(By.CssSelector("#broken-links a")).ToDictionary(a => a.Text.Trim(), a => a.GetDomProperty("href") ?? "");

    /// <summary>Returns the HTTP status code of a URL, or null when the server cannot be reached.</summary>
    public static async Task<HttpStatusCode?> StatusOf(string url)
    {
        try
        {
            using var req = new HttpRequestMessage(HttpMethod.Get, url);
            req.Headers.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AutomationFramework/1.0)");
            using var res = await Http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead);
            return res.StatusCode;
        }
        catch (Exception) { return null; }
    }
}
