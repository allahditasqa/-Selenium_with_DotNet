using Automation.Framework.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Automation.Tests.Pages;

/// <summary>Mouse hover, double click, drag &amp; drop, slider, dynamic button, scrolling dropdown, SVG, shadow DOM, Wikipedia tabs.</summary>
public class InteractionsPage : BasePage
{
    public InteractionsPage(IWebDriver driver) : base(driver) { }

    // ---------- hover ----------
    public List<string> HoverAndReadMenu()
    {
        var btn = Find(By.CssSelector(".dropbtn"));
        ScrollTo(btn);
        Act().MoveToElement(btn).Perform();
        WaitUntil(d => d.FindElement(By.CssSelector(".dropdown-content")).Displayed, 5);
        return FindAll(By.CssSelector(".dropdown-content a")).Where(a => a.Displayed).Select(a => a.Text.Trim()).ToList();
    }

    // ---------- double click ----------
    public string Field1Value => GetValue(By.Id("field1"));
    public string Field2Value => GetValue(By.Id("field2"));

    public void DoubleClickCopyText()
    {
        var btn = Find(By.XPath("//button[normalize-space()='Copy Text']"));
        ScrollTo(btn);
        Act().DoubleClick(btn).Perform();
    }

    public void SetField1(string text) => Type(By.Id("field1"), text);

    // ---------- drag and drop ----------
    public string DragAndDrop()
    {
        var src = Find(By.Id("draggable"));
        var target = Find(By.Id("droppable"));
        ScrollTo(src);
        Act().ClickAndHold(src).MoveToElement(target).MoveByOffset(2, 2).Release().Perform();
        WaitUntil(d => d.FindElement(By.Id("droppable")).Text.Contains("Dropped"), 5);
        return GetText(By.Id("droppable"));
    }

    // ---------- slider ----------
    public string SliderRange => GetValue(By.Id("amount"));

    public string MoveSliderHandle(int handleIndex, string key, int times)
    {
        var handles = FindAll(By.CssSelector("#slider-range .ui-slider-handle"));
        var handle = handles[handleIndex];
        ScrollTo(handle);
        handle.Click();
        for (int i = 0; i < times; i++) handle.SendKeys(key);
        return SliderRange;
    }

    // ---------- dynamic button ----------
    public string DynamicButtonText => FindPresent(By.CssSelector(".widget-content button.start, .widget-content button.stop")).Text.Trim();

    public void ClickDynamicButton() => Click(By.CssSelector(".widget-content button.start, .widget-content button.stop"));

    // ---------- scrolling dropdown ----------
    public string ChooseFromScrollingDropdown(string item)
    {
        Click(By.Id("comboBox"));
        Find(By.Id("dropdown"));
        var option = Wait.Until(d =>
        {
            var dd = d.FindElement(By.Id("dropdown"));
            var found = dd.FindElements(By.XPath($".//div[@class='option'][normalize-space()='{item}']")).FirstOrDefault();
            if (found == null) Js("arguments[0].scrollTop = arguments[0].scrollHeight;", dd);   // load more items
            return found;
        })!;
        Js("arguments[0].scrollIntoView({block:'nearest'});", option);
        option.Click();
        return GetValue(By.Id("comboBox"));
    }

    // ---------- SVG ----------
    public int SvgShapeCount() => FindAll(By.CssSelector("#HTML14 svg")).Count;
    public List<string> SvgShapeNames() =>
        FindAll(By.CssSelector("#HTML14 svg > *")).Select(e => e.TagName.ToLowerInvariant()).ToList();

    // ---------- shadow DOM ----------
    public ISearchContext ShadowRoot => Driver.FindElement(By.Id("shadow_host")).GetShadowRoot();

    public string ShadowText() => ShadowRoot.FindElement(By.CssSelector(".info")).Text.Trim();

    public string TypeInShadowInput(string text)
    {
        var input = ShadowRoot.FindElement(By.CssSelector("input[type='text']"));
        input.SendKeys(text);
        return input.GetDomProperty("value") ?? "";
    }

    public bool TickShadowCheckbox()
    {
        var box = ShadowRoot.FindElement(By.CssSelector("input[type='checkbox']"));
        ScrollTo(box);
        box.Click();
        return box.Selected;
    }

    public string ShadowLinkHref() => ShadowRoot.FindElement(By.CssSelector("a")).GetDomProperty("href") ?? "";

    // ---------- Wikipedia search ("Tabs" widget) ----------
    public int SearchWikipedia(string term)
    {
        Type(By.Id("Wikipedia1_wikipedia-search-input"), term);
        Click(By.CssSelector(".wikipedia-search-button"));
        WaitUntil(d => d.FindElements(By.CssSelector("#Wikipedia1_wikipedia-search-results a")).Count > 0, 20);
        return FindAll(By.CssSelector("#Wikipedia1_wikipedia-search-results a")).Count;
    }
}
