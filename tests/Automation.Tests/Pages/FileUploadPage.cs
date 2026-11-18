using Automation.Framework.Pages;
using OpenQA.Selenium;

namespace Automation.Tests.Pages;

public class FileUploadPage : BasePage
{
    public FileUploadPage(IWebDriver driver) : base(driver) { }

    public string UploadSingle(string filePath)
    {
        FindPresent(By.Id("singleFileInput")).SendKeys(filePath);
        Click(By.XPath("//form[@id='singleFileForm']//button"));
        WaitUntil(d => d.FindElement(By.Id("singleFileStatus")).Text.Length > 0);
        return GetText(By.Id("singleFileStatus"));
    }

    public string UploadMultiple(params string[] filePaths)
    {
        FindPresent(By.Id("multipleFilesInput")).SendKeys(string.Join("\n", filePaths));
        Click(By.XPath("//form[@id='multipleFilesForm']//button"));
        WaitUntil(d => d.FindElement(By.Id("multipleFilesStatus")).Text.Length > 0);
        return GetText(By.Id("multipleFilesStatus"));
    }

    public string SubmitSingleWithoutFile()
    {
        Click(By.XPath("//form[@id='singleFileForm']//button"));
        WaitUntil(d => d.FindElement(By.Id("singleFileStatus")).Text.Length > 0, 5);
        return GetText(By.Id("singleFileStatus"));
    }
}
