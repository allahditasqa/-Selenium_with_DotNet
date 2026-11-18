using Automation.Framework.Core;
using Automation.Framework.Reporting;
using Automation.Tests.Pages;
using Automation.Tests.TestData;

namespace Automation.Tests.Specs;

[TestFixture, Module("Web Tables"), Category("Table")]
public class WebTableTests : BaseTest
{
    private WebTablesPage _tables = null!;

    [SetUp]
    public void Open()
    {
        new HomePage(Driver).Open();
        _tables = new WebTablesPage(Driver);
    }

    [Test]
    [TestSpec("TC_TABLE_001", "Verify Static Web Table headers and number of rows", Priority.P2_High, TestType.UI,
        Pre.Home,
        "Scroll to 'Static Web Table'|Read the column headers|Count the data rows",
        "Expected headers: BookName, Author, Subject, Price",
        "Headers are BookName, Author, Subject, Price; table has 6 data rows")]
    public void Static_Table_Headers_And_Row_Count()
    {
        Assert.That(_tables.StaticHeaders(), Is.EqualTo(new[] { "BookName", "Author", "Subject", "Price" }));
        Assert.That(_tables.StaticBooks(), Has.Count.EqualTo(6));
    }

    [Test]
    [TestSpec("TC_TABLE_002", "Verify books can be filtered by author in the Static Web Table", Priority.P3_Medium, TestType.Functional,
        Pre.Home,
        "Read all rows of the Static Web Table|Filter rows where Author = 'Mukesh'|Collect book names",
        "Author: Mukesh",
        "Books 'Learn Java' and 'Master In Selenium' are returned")]
    public void Static_Table_Filter_By_Author()
    {
        var books = _tables.StaticBooks().Where(b => b.Author == "Mukesh").Select(b => b.Name).ToList();
        Assert.That(books, Is.EquivalentTo(new[] { "Learn Java", "Master In Selenium" }));
    }

    [Test]
    [TestSpec("TC_TABLE_003", "Verify the total price of all books in the Static Web Table", Priority.P3_Medium, TestType.Validation,
        Pre.Home,
        "Read the Price column of all 6 rows|Add the prices",
        "Prices: 300, 500, 300, 3000, 2000, 1000",
        "Total price is 7100")]
    public void Static_Table_Total_Price()
    {
        Assert.That(_tables.StaticBooks().Sum(b => b.Price), Is.EqualTo(7100));
    }

    [Test]
    [TestSpec("TC_TABLE_004", "Verify the Chrome CPU value in the Dynamic Web Table matches the value shown below the table", Priority.P1_Critical, TestType.Validation,
        Pre.Home,
        "Locate the 'Chrome' row and the 'CPU (%)' column (column order changes on every load)|Read the CPU value from the table|Read 'CPU load of Chrome process' text below the table|Compare both",
        "Process: Chrome; Column: CPU (%)",
        "Both values are identical")]
    public void Dynamic_Table_Chrome_Cpu_Matches_Label()
    {
        var fromTable = _tables.DynamicCell("Chrome", "CPU");
        Assert.That(fromTable, Is.EqualTo(_tables.ChromeCpuFromLabel()));
    }

    [Test]
    [TestSpec("TC_TABLE_005", "Verify the Dynamic Web Table has 4 processes and every value carries the correct unit", Priority.P3_Medium, TestType.Validation,
        Pre.Home,
        "Count the rows of the Dynamic Web Table|Read Firefox Memory, System Network, Internet Explorer Disk and Chrome CPU values",
        "Units: MB, Mbps, MB/s, %",
        "4 rows; values end with MB, Mbps, MB/s and % respectively")]
    public void Dynamic_Table_Rows_And_Units()
    {
        Assert.That(_tables.DynamicRowCount(), Is.EqualTo(4));
        Assert.Multiple(() =>
        {
            Assert.That(_tables.DynamicCell("Firefox", "Memory"), Does.EndWith("MB"));
            Assert.That(_tables.DynamicCell("System", "Network"), Does.EndWith("Mbps"));
            Assert.That(_tables.DynamicCell("Internet Explorer", "Disk"), Does.EndWith("MB/s"));
            Assert.That(_tables.DynamicCell("Chrome", "CPU"), Does.EndWith("%"));
        });
    }

    [Test]
    [TestSpec("TC_TABLE_006", "Verify Pagination Web Table shows 5 products on each of its 4 pages", Priority.P1_Critical, TestType.Functional,
        Pre.Home,
        "Count the pagination links|Click page 1, 2, 3 and 4 one by one|On each page count the product rows",
        "Pages: 1 to 4",
        "There are 4 pages; each page shows 5 rows (20 products in total)")]
    public void Pagination_Table_Pages_And_Rows()
    {
        Assert.That(_tables.PageCount(), Is.EqualTo(4));
        int total = 0;
        for (int p = 1; p <= 4; p++)
        {
            _tables.GoToPage(p);
            var rows = _tables.ProductsOnPage();
            Assert.That(rows, Has.Count.EqualTo(5), $"page {p}");
            total += rows.Count;
        }
        Assert.That(total, Is.EqualTo(20));
    }

    [Test]
    [TestSpec("TC_TABLE_007", "Verify the first row of the Pagination Web Table on page 1", Priority.P3_Medium, TestType.Validation,
        Pre.Home,
        "Stay on page 1 of the Pagination Web Table|Read ID, Name and Price of the first row",
        "Expected: 1, Smartphone, $10.99",
        "First row is ID 1, Name 'Smartphone', Price '$10.99'")]
    public void Pagination_Table_First_Row()
    {
        var first = _tables.ProductsOnPage().First();
        Assert.That(first, Is.EqualTo(("1", "Smartphone", "$10.99")));
    }

    [Test]
    [TestSpec("TC_TABLE_008", "Verify product checkboxes can be selected on different pages of the Pagination Web Table", Priority.P2_High, TestType.Functional,
        Pre.Home,
        "On page 1 tick the checkbox of 'Laptop'|Verify it is ticked|Go to page 2|Tick all checkboxes on page 2|Verify all are ticked",
        "Product: Laptop; Page: 2 (all products)",
        "Selected checkboxes are ticked on each page")]
    public void Pagination_Table_Select_Checkboxes()
    {
        _tables.SelectProduct("Laptop");
        Assert.That(_tables.IsProductSelected("Laptop"), Is.True);
        _tables.GoToPage(2);
        var count = _tables.SelectAllProductsOnPage();
        Assert.That(_tables.CheckedProductsOnPage(), Is.EqualTo(count));
        Snap("page 2 all selected");
    }
}
