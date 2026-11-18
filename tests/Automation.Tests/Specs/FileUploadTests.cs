using Automation.Framework.Core;
using Automation.Framework.Reporting;
using Automation.Tests.Pages;
using Automation.Tests.TestData;

namespace Automation.Tests.Specs;

[TestFixture, Module("File Upload"), Category("Upload")]
public class FileUploadTests : BaseTest
{
    private FileUploadPage _upload = null!;

    [SetUp]
    public void Open()
    {
        new HomePage(Driver).Open();
        _upload = new FileUploadPage(Driver);
    }

    [Test]
    [TestSpec("TC_UPLOAD_001", "Verify a single file can be uploaded", Priority.P1_Critical, TestType.Functional,
        Pre.Home,
        "Choose one file in 'Upload Single File'|Click the 'Upload Single File' button|Read the status message",
        "File: single-upload.txt (text file)",
        "Status message confirms the upload and contains the file name 'single-upload.txt'")]
    public void TC_UPLOAD_001_Upload_Single_File()
    {
        var file = TempFiles.Create("single-upload.txt");
        var status = _upload.UploadSingle(file);
        Assert.That(status, Does.Contain("single-upload.txt"));
    }

    [Test]
    [TestSpec("TC_UPLOAD_002", "Verify multiple files can be uploaded together", Priority.P1_Critical, TestType.Functional,
        Pre.Home,
        "Choose three files in 'Upload Multiple Files'|Click the 'Upload Multiple Files' button|Read the status message",
        "Files: one.txt, two.txt, three.txt",
        "Status message lists all three file names")]
    public void TC_UPLOAD_002_Upload_Multiple_Files()
    {
        var files = new[] { TempFiles.Create("one.txt"), TempFiles.Create("two.txt"), TempFiles.Create("three.txt") };
        var status = _upload.UploadMultiple(files);
        Assert.Multiple(() =>
        {
            Assert.That(status, Does.Contain("one.txt"));
            Assert.That(status, Does.Contain("two.txt"));
            Assert.That(status, Does.Contain("three.txt"));
        });
    }

    [Test]
    [TestSpec("TC_UPLOAD_003", "Verify upload without choosing a file shows a message", Priority.P2_High, TestType.Negative,
        Pre.Home,
        "Do not choose any file|Click the 'Upload Single File' button|Read the status message",
        "File: (none)",
        "A message asking the user to select a file is displayed; nothing is uploaded")]
    public void TC_UPLOAD_003_Upload_Without_File()
    {
        var status = _upload.SubmitSingleWithoutFile();
        Assert.That(status, Is.Not.Empty);
        Assert.That(status.ToLowerInvariant(), Does.Contain("no file").Or.Contain("select").Or.Contain("choose"));
    }
}
