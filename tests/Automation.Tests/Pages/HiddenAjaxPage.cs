using Automation.Framework.Pages;
using OpenQA.Selenium;

namespace Automation.Tests.Pages;

/// <summary>"Hidden Elements &amp; AJAX" page.</summary>
public class HiddenAjaxPage : BasePage
{
    public const string Url = "https://testautomationpractice.blogspot.com/p/gui-elements-ajax-hidden.html";

    public HiddenAjaxPage(IWebDriver driver) : base(driver) { }

    public HiddenAjaxPage Open()
    {
        Driver.Navigate().GoToUrl(Url);
        Find(By.Id("toggleInput"));
        return this;
    }

    // The blog also renders the home-page form further down (duplicate ids), so scope locators to #container.
    public bool Input2Visible => IsDisplayed(By.CssSelector("#container #input2"));
    public bool Checkbox2Visible => IsDisplayed(By.CssSelector("#container #checkbox2"));
    public string Status => GetText(By.Id("statusLabel"));

    public void ToggleInput2() => Click(By.Id("toggleInput"));
    public void ToggleCheckbox2() => Click(By.Id("toggleCheckbox"));

    public string LoadAjaxContent()
    {
        Click(By.Id("loadContent"));
        WaitUntil(d => d.FindElement(By.CssSelector("#ajaxContent h2")).Displayed, 15);
        return GetText(By.Id("ajaxContent"));
    }

    public void TypeInVisibleInput(string text) => Type(By.CssSelector("#container #input1"), text);
    public string VisibleInputValue => GetValue(By.CssSelector("#container #input1"));
}
