# Dynamic Elements Test Cases

**Automation:** `tests/Automation.Tests/Specs/` (MouseAndDynamicTests.cs)  |  **Report:** `Reports/dynamic-elements/Dynamic Elements test <DD-MM-YYYY>_<HH-mm-ss>.html` (all reports: `Reports/index.html`)
**Application:** https://testautomationpractice.blogspot.com/  |  **ID prefixes:** `TC_DYN_`  |  **Master CSV:** `docs/test-cases/dynamic-elements.csv`

## Overview
Dynamic and special widgets: the START/STOP button, the scrolling dropdown, SVG shapes, Shadow DOM and the Wikipedia search widget.

## Scope and notes
- The Wikipedia case needs internet access.

## Dynamic elements

| ID | Title | Priority | Type | Preconditions | Test data | Steps | Expected result | Automation |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| TC_DYN_001 | Verify the Dynamic Button toggles between START and STOP | P2 - High | Functional | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | - | 1. Read the button text (START)<br>2. Click the button<br>3. Verify it shows STOP<br>4. Click again<br>5. Verify it shows START | Button text alternates START -> STOP -> START | Automated `tests/Automation.Tests/Specs/MouseAndDynamicTests.cs::DynamicElementTests.TC_DYN_001_Dynamic_Button_Toggle` |
| TC_DYN_002 | Verify an item can be chosen from the Scrolling DropDown | P3 - Medium | Functional | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Item: Item 25 | 1. Click the Scrolling DropDown field<br>2. Scroll the list to 'Item 25'<br>3. Click 'Item 25'<br>4. Read the field value | Field shows 'Item 25' and the list closes | Automated `tests/Automation.Tests/Specs/MouseAndDynamicTests.cs::DynamicElementTests.TC_DYN_002_Scrolling_Dropdown_Item_25` |
| TC_DYN_003 | Verify lazy-loaded items appear while scrolling the Scrolling DropDown | P3 - Medium | Functional | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Item: Item 150 (not in the first 100 loaded items) | 1. Click the Scrolling DropDown field<br>2. Scroll to the bottom repeatedly until 'Item 150' is loaded<br>3. Click 'Item 150'<br>4. Read the field value | More items load on scroll; field shows 'Item 150' | Automated `tests/Automation.Tests/Specs/MouseAndDynamicTests.cs::DynamicElementTests.TC_DYN_003_Scrolling_Dropdown_Lazy_Loaded_Item` |
| TC_DYN_004 | Verify SVG shapes are present on the page | P4 - Low | UI | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Expected shapes: circle, rect, ... | 1. Scroll to 'SVG Elements'<br>2. Count the SVG graphics<br>3. Read the shape types | At least 3 SVG graphics exist including a circle and a rectangle | Automated `tests/Automation.Tests/Specs/MouseAndDynamicTests.cs::DynamicElementTests.TC_DYN_004_Svg_Elements` |
| TC_DYN_005 | Verify elements inside the Shadow DOM can be read and operated | P2 - High | Functional | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Text: shadow text | 1. Open the shadow root of the shadow host<br>2. Read the info text<br>3. Type text in the shadow text box<br>4. Tick the shadow checkbox<br>5. Read the shadow link | Info text is 'Mobiles'; typed text is retained; checkbox is ticked; link points to pavantestingtools.com | Automated `tests/Automation.Tests/Specs/MouseAndDynamicTests.cs::DynamicElementTests.TC_DYN_005_Shadow_Dom` |
| TC_DYN_006 | Verify the Wikipedia search widget returns results | P3 - Medium | Integration | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open; internet access to Wikipedia is available | Search term: Selenium | 1. Scroll to the 'Tabs' (Wikipedia) widget<br>2. Type a search term<br>3. Click the search button<br>4. Wait for the results<br>5. Count the result links | At least one search result is displayed | Automated `tests/Automation.Tests/Specs/MouseAndDynamicTests.cs::DynamicElementTests.TC_DYN_006_Wikipedia_Search` |

## Summary

| Group | Cases | Automated |
| --- | --- | --- |
| Dynamic elements | 6 | 6 |
| **Total** | **6** | **6** |
