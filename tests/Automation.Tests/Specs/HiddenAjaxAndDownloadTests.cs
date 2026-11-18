using Automation.Framework.Core;
using Automation.Framework.Reporting;
using Automation.Tests.Pages;
using Automation.Tests.TestData;

namespace Automation.Tests.Specs;

[TestFixture, Module("Hidden Elements and AJAX"), Category("Hidden")]
public class HiddenAjaxTests : BaseTest
{
    private HiddenAjaxPage _page = null!;

    [SetUp]
    public void Open() => _page = new HiddenAjaxPage(Driver).Open();

    [Test]
    [TestSpec("TC_HIDDEN_001", "Verify Input Box 2 is hidden initially and can be shown and hidden with the toggle button", Priority.P2_High, TestType.Functional,
        Pre.Ajax,
        "Verify Input Box 2 is not visible|Click 'Toggle Input Box 2'|Verify it is visible and status says 'Input Box 2 shown'|Click the toggle again|Verify it is hidden and status says 'Input Box 2 hidden'",
        "-",
        "Visibility switches hidden -> shown -> hidden with matching status messages")]
    public void TC_HIDDEN_001_Toggle_Input_Box_2()
    {
        Assert.That(_page.Input2Visible, Is.False);
        _page.ToggleInput2();
        Assert.That(_page.Input2Visible, Is.True);
        Assert.That(_page.Status, Is.EqualTo("Input Box 2 shown"));
        _page.ToggleInput2();
        Assert.That(_page.Input2Visible, Is.False);
        Assert.That(_page.Status, Is.EqualTo("Input Box 2 hidden"));
    }

    [Test]
    [TestSpec("TC_HIDDEN_002", "Verify Checkbox 2 is hidden initially and becomes visible with the toggle button", Priority.P2_High, TestType.Functional,
        Pre.Ajax,
        "Verify Checkbox 2 is not visible|Click 'Toggle Checkbox 2'|Verify it is visible and status says 'Checkbox 2 shown'",
        "-",
        "Checkbox 2 becomes visible and the status message is 'Checkbox 2 shown'")]
    public void TC_HIDDEN_002_Toggle_Checkbox_2()
    {
        Assert.That(_page.Checkbox2Visible, Is.False);
        _page.ToggleCheckbox2();
        Assert.That(_page.Checkbox2Visible, Is.True);
        Assert.That(_page.Status, Is.EqualTo("Checkbox 2 shown"));
    }

    [Test]
    [TestSpec("TC_HIDDEN_003", "Verify AJAX content loads after clicking 'Load AJAX Content'", Priority.P1_Critical, TestType.Functional,
        Pre.Ajax,
        "Click 'Load AJAX Content'|Wait for the content to appear (asynchronous)|Read the loaded content",
        "-",
        "Heading 'AJAX Content Loaded' is displayed after the delay")]
    public void TC_HIDDEN_003_Load_Ajax_Content()
    {
        var text = _page.LoadAjaxContent();
        Assert.That(text, Does.Contain("AJAX Content Loaded"));
    }

    [Test]
    [TestSpec("TC_HIDDEN_004", "Verify the visible Input Box 1 accepts text", Priority.P3_Medium, TestType.Functional,
        Pre.Ajax,
        "Type text in Input Box 1|Read the field value",
        "Text: visible input",
        "Field shows 'visible input'")]
    public void TC_HIDDEN_004_Type_In_Visible_Input()
    {
        _page.TypeInVisibleInput("visible input");
        Assert.That(_page.VisibleInputValue, Is.EqualTo("visible input"));
    }
}

[TestFixture, Module("Download Files"), Category("Download")]
public class DownloadFilesTests : BaseTest
{
    private DownloadFilesPage _page = null!;

    [SetUp]
    public void Open() => _page = new DownloadFilesPage(Driver).Open();

    [Test]
    [TestSpec("TC_DOWNLOAD_001", "Verify a text file generated from entered text can be downloaded with correct content", Priority.P1_Critical, TestType.Functional,
        Pre.Download,
        "Enter text in the text area|Click 'Generate and Download Text File'|Click 'Download Text File'|Wait for info.txt in the download folder|Read the file content",
        "Text: Hello from Selenium .NET",
        "info.txt is downloaded and its content equals the entered text")]
    public void TC_DOWNLOAD_001_Download_Text_File()
    {
        _page.EnterText("Hello from Selenium .NET");
        var path = _page.GenerateAndDownloadText(DownloadDir);
        Assert.That(File.Exists(path), Is.True, "info.txt must be downloaded");
        Assert.That(File.ReadAllText(path), Is.EqualTo("Hello from Selenium .NET"));
    }

    [Test]
    [TestSpec("TC_DOWNLOAD_002", "Verify a PDF file generated from entered text can be downloaded", Priority.P2_High, TestType.Functional,
        Pre.Download,
        "Enter text in the text area|Click 'Generate and Download PDF File'|Click 'Download PDF File'|Wait for info.pdf in the download folder|Check the file size",
        "Text: PDF content",
        "info.pdf is downloaded and is not empty")]
    public void TC_DOWNLOAD_002_Download_Pdf_File()
    {
        _page.EnterText("PDF content");
        var path = _page.GenerateAndDownloadPdf(DownloadDir);
        Assert.That(File.Exists(path), Is.True, "info.pdf must be downloaded");
        Assert.That(new FileInfo(path).Length, Is.GreaterThan(0));
    }
}
