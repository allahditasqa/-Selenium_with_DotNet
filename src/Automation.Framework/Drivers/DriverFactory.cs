using Automation.Framework.Config;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace Automation.Framework.Drivers;

/// <summary>Creates a fresh WebDriver for the browser configured in appsettings.json.
/// Selenium Manager (built into Selenium 4) downloads the matching driver automatically.</summary>
public static class DriverFactory
{
    public static IWebDriver Create(FrameworkSettings s, string downloadDir)
    {
        Directory.CreateDirectory(downloadDir);
        IWebDriver driver = StartBrowser(s, downloadDir);

        driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(s.PageLoadTimeoutSeconds);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero; // explicit waits only (no mixing)
        if (!s.Headless) driver.Manage().Window.Maximize();
        return driver;
    }

    private static readonly object StartLock = new();

    /// <summary>Browsers are started one at a time (Selenium Manager may download a driver on the first start)
    /// and a failed start is retried once, so parallel runs stay stable.</summary>
    private static IWebDriver StartBrowser(FrameworkSettings s, string downloadDir)
    {
        for (int attempt = 1; ; attempt++)
        {
            try
            {
                lock (StartLock)
                {
                    return s.Browser switch
                    {
                        "chrome" => new ChromeDriver(ChromeOpts(s, downloadDir)),
                        "edge" => new EdgeDriver(EdgeOpts(s, downloadDir)),
                        "firefox" => new FirefoxDriver(FirefoxOpts(s, downloadDir)),
                        _ => throw new NotSupportedException($"Browser '{s.Browser}' is not supported. Use chrome, edge or firefox.")
                    };
                }
            }
            catch (NotSupportedException) { throw; }
            catch (Exception ex) when (attempt < 3)
            {
                Logging.TestLog.Warn($"Browser start failed (attempt {attempt}): {ex.Message}");
                Thread.Sleep(2000);
            }
        }
    }

    private static ChromeOptions ChromeOpts(FrameworkSettings s, string dl)
    {
        var o = new ChromeOptions();
        if (s.Headless) o.AddArgument("--headless=new");
        o.AddArguments($"--window-size={s.WindowWidth},{s.WindowHeight}", "--disable-notifications",
            "--disable-popup-blocking", "--disable-infobars", "--no-sandbox", "--disable-dev-shm-usage");
        o.AddUserProfilePreference("download.default_directory", dl);
        o.AddUserProfilePreference("download.prompt_for_download", false);
        o.AddUserProfilePreference("safebrowsing.enabled", true);
        o.PageLoadStrategy = PageLoadStrategy.Normal;
        return o;
    }

    private static EdgeOptions EdgeOpts(FrameworkSettings s, string dl)
    {
        var o = new EdgeOptions();
        if (s.Headless) o.AddArgument("--headless=new");
        o.AddArguments($"--window-size={s.WindowWidth},{s.WindowHeight}", "--disable-notifications", "--disable-popup-blocking");
        o.AddUserProfilePreference("download.default_directory", dl);
        return o;
    }

    private static FirefoxOptions FirefoxOpts(FrameworkSettings s, string dl)
    {
        var o = new FirefoxOptions();
        if (s.Headless) o.AddArgument("-headless");
        o.AddArguments($"--width={s.WindowWidth}", $"--height={s.WindowHeight}");
        o.SetPreference("browser.download.folderList", 2);
        o.SetPreference("browser.download.dir", dl);
        o.SetPreference("browser.helperApps.neverAsk.saveToDisk", "text/plain,application/pdf");
        return o;
    }
}
