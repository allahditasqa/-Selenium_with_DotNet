using Automation.Framework.Pages;
using OpenQA.Selenium;

namespace Automation.Tests.Pages;

/// <summary>Static table, dynamic (shuffling) table and the paginated product table.</summary>
public class WebTablesPage : BasePage
{
    private static readonly By StaticRows = By.XPath("//table[@name='BookTable']//tr[td]");
    private static readonly By DynamicHeaders = By.CssSelector("#taskTable thead th");
    private static readonly By DynamicRows = By.CssSelector("#taskTable tbody tr");
    private static readonly By ProductRows = By.CssSelector("#productTable tbody tr");

    public WebTablesPage(IWebDriver driver) : base(driver) { }

    // ---------- static table ----------
    public record Book(string Name, string Author, string Subject, int Price);

    public List<Book> StaticBooks() => FindAll(StaticRows).Select(r =>
    {
        var c = r.FindElements(By.TagName("td")).Select(x => x.Text.Trim()).ToList();
        return new Book(c[0], c[1], c[2], int.Parse(c[3]));
    }).ToList();

    public List<string> StaticHeaders() =>
        FindAll(By.XPath("//table[@name='BookTable']//th")).Select(h => h.Text.Trim()).ToList();

    // ---------- dynamic table ----------
    /// <summary>Reads a cell by process name + column header. Column order changes on every refresh, so never use fixed indexes.</summary>
    public string DynamicCell(string process, string column)
    {
        var headers = FindAll(DynamicHeaders).Select(h => h.Text.Trim()).ToList();
        int col = headers.FindIndex(h => h.StartsWith(column, StringComparison.OrdinalIgnoreCase));
        foreach (var row in FindAll(DynamicRows))
        {
            var cells = row.FindElements(By.TagName("td"));
            if (cells[0].Text.Trim().Equals(process, StringComparison.OrdinalIgnoreCase)) return cells[col].Text.Trim();
        }
        throw new NoSuchElementException($"Process '{process}' not found in dynamic table.");
    }

    public string ChromeCpuFromLabel() => GetText(By.CssSelector("#displayValues")).Split('\n')
        .First(l => l.StartsWith("CPU load of Chrome")).Split(':')[1].Trim();

    public string DisplayValuesText() => GetText(By.Id("displayValues"));
    public int DynamicRowCount() => FindAll(DynamicRows).Count;

    // ---------- pagination table ----------
    public int PageCount() => FindAll(By.CssSelector("#pagination li a")).Count;

    public void GoToPage(int page)
    {
        Click(By.XPath($"//ul[@id='pagination']//a[normalize-space()='{page}']"));
        WaitUntil(d => d.FindElement(By.XPath($"//ul[@id='pagination']//a[normalize-space()='{page}']"))
            .GetDomAttribute("class")?.Contains("active") == true);
    }

    public List<(string Id, string Name, string Price)> ProductsOnPage() => FindAll(ProductRows).Select(r =>
    {
        var c = r.FindElements(By.TagName("td"));
        return (c[0].Text.Trim(), c[1].Text.Trim(), c[2].Text.Trim());
    }).ToList();

    public void SelectProduct(string name)
    {
        var row = FindAll(ProductRows).First(r => r.FindElements(By.TagName("td"))[1].Text.Trim() == name);
        var box = row.FindElement(By.CssSelector("input[type='checkbox']"));
        ScrollTo(box);
        if (!box.Selected) JsClick(box);
    }

    public bool IsProductSelected(string name) =>
        FindAll(ProductRows).First(r => r.FindElements(By.TagName("td"))[1].Text.Trim() == name)
            .FindElement(By.CssSelector("input[type='checkbox']")).Selected;

    public int SelectAllProductsOnPage()
    {
        int n = 0;
        foreach (var box in FindAll(By.CssSelector("#productTable tbody input[type='checkbox']")))
        {
            ScrollTo(box); if (!box.Selected) JsClick(box); n++;
        }
        return n;
    }

    public int CheckedProductsOnPage() =>
        FindAll(By.CssSelector("#productTable tbody input[type='checkbox']")).Count(b => b.Selected);
}
