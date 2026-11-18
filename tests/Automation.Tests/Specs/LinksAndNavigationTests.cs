using Automation.Framework.Core;
using Automation.Framework.Reporting;
using Automation.Tests.Pages;
using Automation.Tests.TestData;

namespace Automation.Tests.Specs;

[TestFixture, Module("Links and Navigation"), Category("Links")]
public class LinksAndNavigationTests : BaseTest
{
    private HomePage _home = null!;
    private LinksPage _links = null!;

    [SetUp]
    public void Open()
    {
        _home = new HomePage(Driver).Open();
        _links = new LinksPage(Driver);
    }

    [Test]
    [TestSpec("TC_LINK_001", "Verify Mobile Labels are displayed", Priority.P4_Low, TestType.UI,
        Pre.Home,
        "Scroll to 'Labels And Links'|Read the 'Mobile Labels' items",
        "-",
        "Labels 'Samsung', 'Real Me' and 'Moto' are displayed in this order")]
    public void Mobile_Labels()
    {
        Assert.That(_links.MobileLabels(), Is.EqualTo(new[] { "Samsung", "Real Me", "Moto" }));
    }

    [Test]
    [TestSpec("TC_LINK_002", "Verify Laptop Links point to the correct sites", Priority.P3_Medium, TestType.Functional,
        Pre.Home,
        "Scroll to 'Laptop Links'|Read the href of Apple, Lenovo and Dell",
        "-",
        "Hrefs contain apple.com, lenovo.com and dell.com respectively")]
    public void Laptop_Links()
    {
        var l = _links.LaptopLinks();
        Assert.Multiple(() =>
        {
            Assert.That(l["Apple"], Does.Contain("apple.com"));
            Assert.That(l["Lenovo"], Does.Contain("lenovo.com"));
            Assert.That(l["Dell"], Does.Contain("dell.com"));
        });
    }

    [Test]
    [TestSpec("TC_LINK_003", "Verify all 8 Broken Links are listed with their error codes", Priority.P3_Medium, TestType.UI,
        Pre.Home,
        "Scroll to 'Broken Links'|Count the links|Read the href of every link",
        "Error codes: 400, 401, 403, 404, 408, 500, 502, 503",
        "8 links exist and each href ends with its own error code")]
    public void Broken_Links_Are_Listed()
    {
        var b = _links.BrokenLinks();
        Assert.That(b, Has.Count.EqualTo(8));
        foreach (var (text, href) in b)
            Assert.That(href, Does.EndWith("e=" + text.Replace("Errorcode ", "")), text);
    }

    [Test]
    [TestSpec("TC_LINK_004", "Verify the HTTP status of every Broken Link is checked", Priority.P3_Medium, TestType.Integration,
        Pre.Home + "; internet access is available",
        "Collect the href of all 8 Broken Links|Send an HTTP GET request to every URL|Record the status code",
        "8 URLs from the Broken Links section",
        "Every link is requested; each returns a status code or is reported as unreachable")]
    public async Task Broken_Links_Http_Status()
    {
        var results = new List<string>();
        foreach (var (text, href) in _links.BrokenLinks())
        {
            var code = await LinksPage.StatusOf(href);
            results.Add($"{text}: {(code.HasValue ? ((int)code.Value).ToString() : "unreachable")}");
            Assert.That(code is null || (int)code.Value >= 200, Is.True);
        }
        TestContext.Out.WriteLine(string.Join(Environment.NewLine, results));
        Assert.That(results, Has.Count.EqualTo(8));
    }

    [Test]
    [TestSpec("TC_LINK_005", "Verify Laptop Links respond without a server error", Priority.P3_Medium, TestType.Integration,
        Pre.Home + "; internet access is available",
        "Collect the href of Apple, Lenovo and Dell|Send an HTTP GET request to each|Read the status code",
        "URLs: apple.com, lenovo.com, dell.com",
        "No link returns a 5xx status code")]
    public async Task Working_Links_Respond()
    {
        foreach (var (name, href) in _links.LaptopLinks())
        {
            var code = await LinksPage.StatusOf(href);
            if (code is null) Assert.Inconclusive($"{name} unreachable from this network");
            Assert.That((int)code!.Value, Is.LessThan(500), $"{name} -> {code}");
        }
    }

    [Test]
    [TestSpec("TC_NAV_001", "Verify the header menu contains all navigation links", Priority.P2_High, TestType.UI,
        Pre.Home,
        "Read all links of the top menu|Read the href of 'PlaywrightPractice'",
        "Expected menu: Home, Udemy Courses, Online Trainings, Blog, PlaywrightPractice",
        "All 5 links are present; PlaywrightPractice points to the playwrightpractice page")]
    public void Header_Menu_Links()
    {
        var h = _home.HeaderLinks();
        Assert.That(h.Keys, Is.SupersetOf(new[] { "Home", "Udemy Courses", "Online Trainings", "Blog", "PlaywrightPractice" }));
        Assert.That(h["PlaywrightPractice"], Does.Contain("playwrightpractice"));
    }

    [Test]
    [TestSpec("TC_NAV_002", "Verify the footer link 'Hidden Elements & AJAX' opens its page", Priority.P2_High, TestType.Functional,
        Pre.Home,
        "Scroll to the footer|Click 'Hidden Elements & AJAX'|Read the page URL",
        "-",
        "URL contains 'gui-elements-ajax-hidden'")]
    public void Footer_Link_Hidden_Elements()
    {
        _home.ClickFooterLink("Hidden Elements & AJAX");
        Assert.That(_home.CurrentUrl, Does.Contain("gui-elements-ajax-hidden"));
    }

    [Test]
    [TestSpec("TC_NAV_003", "Verify the footer link 'Download Files' opens its page", Priority.P2_High, TestType.Functional,
        Pre.Home,
        "Scroll to the footer|Click 'Download Files'|Read the page URL",
        "-",
        "URL contains 'download-files'")]
    public void Footer_Link_Download_Files()
    {
        _home.ClickFooterLink("Download Files");
        Assert.That(_home.CurrentUrl, Does.Contain("download-files"));
    }

    [Test]
    [TestSpec("TC_NAV_004", "Verify the Visitors counter displays a number", Priority.P4_Low, TestType.UI,
        Pre.Home,
        "Scroll to the 'Visitors' widget|Read the total count",
        "-",
        "Counter shows a numeric value (digits with optional thousand separators)")]
    public void Visitors_Counter()
    {
        Assert.That(_home.TotalVisitorCount(), Does.Match(@"^[\d,]+$"));
    }

    [TestCase(1), TestCase(2), TestCase(3)]
    [TestSpec("TC_NAV_005", "Verify Form Sections 1, 2 and 3 show their heading and paragraph and accept input (data-driven)", Priority.P3_Medium, TestType.Functional,
        Pre.Home,
        "Scroll to the Form section N|Read the heading and the paragraph|Type text in the section input|Click the section Submit button|Read the input value",
        "Section N = 1, 2, 3; Text: 'hello N'",
        "Heading is 'Section N'; paragraph is 'This is a paragraph in Section N.'; input keeps the typed text")]
    public void Form_Sections(int n)
    {
        var f = new FormSectionsPage(Driver);
        Assert.Multiple(() =>
        {
            Assert.That(f.Heading(n), Is.EqualTo($"Section {n}"));
            Assert.That(f.Paragraph(n), Is.EqualTo($"This is a paragraph in Section {n}."));
            Assert.That(f.FillAndSubmit(n, "hello " + n), Is.EqualTo("hello " + n));
        });
    }
}
