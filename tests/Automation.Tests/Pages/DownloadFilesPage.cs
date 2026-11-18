using Automation.Framework.Pages;
using OpenQA.Selenium;

namespace Automation.Tests.Pages;

/// <summary>"Download Files" page: generate a text/pdf file in the browser and download it.</summary>
public class DownloadFilesPage : BasePage
{
    public const string Url = "https://testautomationpractice.blogspot.com/p/download-files_25.html";

    public DownloadFilesPage(IWebDriver driver) : base(driver) { }

    public DownloadFilesPage Open()
    {
        Driver.Navigate().GoToUrl(Url);
        Find(By.Id("inputText"));
        return this;
    }

    public void EnterText(string text) => Type(By.Id("inputText"), text);

    public string GenerateAndDownloadText(string downloadDir) => Download("generateTxt", "txtDownloadLink", "info.txt", downloadDir);
    public string GenerateAndDownloadPdf(string downloadDir) => Download("generatePdf", "pdfDownloadLink", "info.pdf", downloadDir);

    private string Download(string generateButtonId, string linkId, string fileName, string downloadDir)
    {
        Click(By.Id(generateButtonId));
        Click(By.Id(linkId));
        var path = Path.Combine(downloadDir, fileName);
        WaitUntil(_ => File.Exists(path) && !File.Exists(path + ".crdownload"), 20);
        return path;
    }
}
