using Automation.Framework.Pages;
using OpenQA.Selenium;

namespace Automation.Tests.Pages;

/// <summary>The three "Form" sections at the bottom of the home page.</summary>
public class FormSectionsPage : BasePage
{
    public FormSectionsPage(IWebDriver driver) : base(driver) { }

    public string Heading(int n) => GetText(By.CssSelector($"#section{n} h4"));
    public string Paragraph(int n) => GetText(By.Id($"para{n}"));

    public string FillAndSubmit(int n, string text)
    {
        Type(By.Id($"input{n}"), text);
        Click(By.Id($"btn{n}"));
        return GetValue(By.Id($"input{n}"));
    }
}
