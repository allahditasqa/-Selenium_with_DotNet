using Automation.Framework.Core;
using Automation.Framework.Reporting;
using Automation.Tests.Pages;
using Automation.Tests.TestData;
using OpenQA.Selenium;

namespace Automation.Tests.Specs;

[TestFixture, Module("Mouse and Keyboard Actions"), Category("Actions")]
public class MouseActionTests : BaseTest
{
    private InteractionsPage _page = null!;

    [SetUp]
    public void Open()
    {
        new HomePage(Driver).Open();
        _page = new InteractionsPage(Driver);
    }

    [Test]
    [TestSpec("TC_MOUSE_001", "Verify hovering over 'Point Me' reveals the Mobiles and Laptops menu", Priority.P2_High, TestType.Functional,
        Pre.Home,
        "Scroll to 'Mouse Hover'|Move the mouse over the 'Point Me' button|Read the dropdown menu items",
        "-",
        "Dropdown opens and shows 'Mobiles' and 'Laptops'")]
    public void Hover_Menu()
    {
        Assert.That(_page.HoverAndReadMenu(), Is.EqualTo(new[] { "Mobiles", "Laptops" }));
        Snap("hover menu open");
    }

    [Test]
    [TestSpec("TC_MOUSE_002", "Verify double-clicking 'Copy Text' copies Field1 into Field2", Priority.P2_High, TestType.Functional,
        Pre.Home,
        "Enter text in Field1|Double-click the 'Copy Text' button|Read Field2",
        "Field1: Automation with Selenium .NET",
        "Field2 contains exactly the Field1 text")]
    public void Double_Click_Copies_Text()
    {
        _page.SetField1("Automation with Selenium .NET");
        _page.DoubleClickCopyText();
        Assert.That(_page.Field2Value, Is.EqualTo("Automation with Selenium .NET"));
    }

    [Test]
    [TestSpec("TC_MOUSE_003", "Verify drag and drop of 'Drag me to my target' onto the drop box", Priority.P1_Critical, TestType.Functional,
        Pre.Home,
        "Scroll to 'Drag and Drop'|Drag the 'Drag me to my target' box|Drop it on the 'Drop here' box|Read the target text",
        "-",
        "Target box text changes to 'Dropped!'")]
    public void Drag_And_Drop()
    {
        Assert.That(_page.DragAndDrop(), Does.Contain("Dropped"));
    }

    [Test]
    [TestSpec("TC_MOUSE_004", "Verify the slider minimum handle moves with the keyboard and updates the price range", Priority.P3_Medium, TestType.Functional,
        Pre.Home,
        "Scroll to 'Slider'|Read the initial price range|Focus the left handle and press Right Arrow 25 times|Read the price range",
        "Handle: minimum; Key: Right Arrow x 25",
        "Range minimum increases from $75 to $100; range text changes")]
    public void Slider_Min_Handle()
    {
        var before = _page.SliderRange;
        var after = _page.MoveSliderHandle(0, Keys.ArrowRight, 25);
        Assert.That(after, Is.Not.EqualTo(before));
        Assert.That(after, Does.StartWith("$100"));
    }

    [Test]
    [TestSpec("TC_MOUSE_005", "Verify the slider maximum handle moves with the keyboard and updates the price range", Priority.P3_Medium, TestType.Functional,
        Pre.Home,
        "Scroll to 'Slider'|Focus the right handle and press Left Arrow 50 times|Read the price range",
        "Handle: maximum; Key: Left Arrow x 50",
        "Range maximum decreases from $300 to $250")]
    public void Slider_Max_Handle()
    {
        var after = _page.MoveSliderHandle(1, Keys.ArrowLeft, 50);
        Assert.That(after, Does.EndWith("$250"));
    }
}

[TestFixture, Module("Dynamic Elements"), Category("Dynamic")]
public class DynamicElementTests : BaseTest
{
    private InteractionsPage _page = null!;

    [SetUp]
    public void Open()
    {
        new HomePage(Driver).Open();
        _page = new InteractionsPage(Driver);
    }

    [Test]
    [TestSpec("TC_DYN_001", "Verify the Dynamic Button toggles between START and STOP", Priority.P2_High, TestType.Functional,
        Pre.Home,
        "Read the button text (START)|Click the button|Verify it shows STOP|Click again|Verify it shows START",
        "-",
        "Button text alternates START -> STOP -> START")]
    public void Dynamic_Button_Toggle()
    {
        Assert.That(_page.DynamicButtonText, Is.EqualTo("START"));
        _page.ClickDynamicButton();
        Assert.That(_page.DynamicButtonText, Is.EqualTo("STOP"));
        _page.ClickDynamicButton();
        Assert.That(_page.DynamicButtonText, Is.EqualTo("START"));
    }

    [Test]
    [TestSpec("TC_DYN_002", "Verify an item can be chosen from the Scrolling DropDown", Priority.P3_Medium, TestType.Functional,
        Pre.Home,
        "Click the Scrolling DropDown field|Scroll the list to 'Item 25'|Click 'Item 25'|Read the field value",
        "Item: Item 25",
        "Field shows 'Item 25' and the list closes")]
    public void Scrolling_Dropdown_Item_25()
    {
        Assert.That(_page.ChooseFromScrollingDropdown("Item 25"), Is.EqualTo("Item 25"));
    }

    [Test]
    [TestSpec("TC_DYN_003", "Verify lazy-loaded items appear while scrolling the Scrolling DropDown", Priority.P3_Medium, TestType.Functional,
        Pre.Home,
        "Click the Scrolling DropDown field|Scroll to the bottom repeatedly until 'Item 150' is loaded|Click 'Item 150'|Read the field value",
        "Item: Item 150 (not in the first 100 loaded items)",
        "More items load on scroll; field shows 'Item 150'")]
    public void Scrolling_Dropdown_Lazy_Loaded_Item()
    {
        Assert.That(_page.ChooseFromScrollingDropdown("Item 150"), Is.EqualTo("Item 150"));
    }

    [Test]
    [TestSpec("TC_DYN_004", "Verify SVG shapes are present on the page", Priority.P4_Low, TestType.UI,
        Pre.Home,
        "Scroll to 'SVG Elements'|Count the SVG graphics|Read the shape types",
        "Expected shapes: circle, rect, ...",
        "At least 3 SVG graphics exist including a circle and a rectangle")]
    public void Svg_Elements()
    {
        Assert.That(_page.SvgShapeCount(), Is.GreaterThanOrEqualTo(3));
        Assert.That(_page.SvgShapeNames(), Does.Contain("circle").And.Contain("rect"));
    }

    [Test]
    [TestSpec("TC_DYN_005", "Verify elements inside the Shadow DOM can be read and operated", Priority.P2_High, TestType.Functional,
        Pre.Home,
        "Open the shadow root of the shadow host|Read the info text|Type text in the shadow text box|Tick the shadow checkbox|Read the shadow link",
        "Text: shadow text",
        "Info text is 'Mobiles'; typed text is retained; checkbox is ticked; link points to pavantestingtools.com")]
    public void Shadow_Dom()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_page.ShadowText(), Is.EqualTo("Mobiles"));
            Assert.That(_page.TypeInShadowInput("shadow text"), Is.EqualTo("shadow text"));
            Assert.That(_page.TickShadowCheckbox(), Is.True);
            Assert.That(_page.ShadowLinkHref(), Does.Contain("pavantestingtools.com"));
        });
    }

    [Test]
    [TestSpec("TC_DYN_006", "Verify the Wikipedia search widget returns results", Priority.P3_Medium, TestType.Integration,
        Pre.Home + "; internet access to Wikipedia is available",
        "Scroll to the 'Tabs' (Wikipedia) widget|Type a search term|Click the search button|Wait for the results|Count the result links",
        "Search term: Selenium",
        "At least one search result is displayed")]
    public void Wikipedia_Search()
    {
        Assert.That(_page.SearchWikipedia("Selenium"), Is.GreaterThan(0));
    }
}
