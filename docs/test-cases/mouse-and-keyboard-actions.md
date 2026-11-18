# Mouse and Keyboard Actions Test Cases

**Automation:** `tests/Automation.Tests/Specs/` (MouseAndDynamicTests.cs)  |  **Report:** `Reports/mouse-and-keyboard-actions/Mouse and Keyboard Actions test <DD-MM-YYYY>_<HH-mm-ss>.html` (all reports: `Reports/index.html`)
**Application:** https://testautomationpractice.blogspot.com/  |  **ID prefixes:** `TC_MOUSE_`  |  **Master CSV:** `docs/test-cases/mouse-and-keyboard-actions.csv`

## Overview
Mouse hover menu, double click, drag and drop and the two-handle range slider.

## Scope and notes
- The slider has two handles; each handle has its own case.

## Mouse and keyboard actions

| ID | Title | Priority | Type | Preconditions | Test data | Steps | Expected result | Automation |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| TC_MOUSE_001 | Verify hovering over 'Point Me' reveals the Mobiles and Laptops menu | P2 - High | Functional | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | - | 1. Scroll to 'Mouse Hover'<br>2. Move the mouse over the 'Point Me' button<br>3. Read the dropdown menu items | Dropdown opens and shows 'Mobiles' and 'Laptops' | Automated `tests/Automation.Tests/Specs/MouseAndDynamicTests.cs::MouseActionTests.TC_MOUSE_001_Hover_Menu` |
| TC_MOUSE_002 | Verify double-clicking 'Copy Text' copies Field1 into Field2 | P2 - High | Functional | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Field1: Automation with Selenium .NET | 1. Enter text in Field1<br>2. Double-click the 'Copy Text' button<br>3. Read Field2 | Field2 contains exactly the Field1 text | Automated `tests/Automation.Tests/Specs/MouseAndDynamicTests.cs::MouseActionTests.TC_MOUSE_002_Double_Click_Copies_Text` |
| TC_MOUSE_003 | Verify drag and drop of 'Drag me to my target' onto the drop box | P1 - Critical | Functional | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | - | 1. Scroll to 'Drag and Drop'<br>2. Drag the 'Drag me to my target' box<br>3. Drop it on the 'Drop here' box<br>4. Read the target text | Target box text changes to 'Dropped!' | Automated `tests/Automation.Tests/Specs/MouseAndDynamicTests.cs::MouseActionTests.TC_MOUSE_003_Drag_And_Drop` |
| TC_MOUSE_004 | Verify the slider minimum handle moves with the keyboard and updates the price range | P3 - Medium | Functional | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Handle: minimum; Key: Right Arrow x 25 | 1. Scroll to 'Slider'<br>2. Read the initial price range<br>3. Focus the left handle and press Right Arrow 25 times<br>4. Read the price range | Range minimum increases from $75 to $100; range text changes | Automated `tests/Automation.Tests/Specs/MouseAndDynamicTests.cs::MouseActionTests.TC_MOUSE_004_Slider_Min_Handle` |
| TC_MOUSE_005 | Verify the slider maximum handle moves with the keyboard and updates the price range | P3 - Medium | Functional | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Handle: maximum; Key: Left Arrow x 50 | 1. Scroll to 'Slider'<br>2. Focus the right handle and press Left Arrow 50 times<br>3. Read the price range | Range maximum decreases from $300 to $250 | Automated `tests/Automation.Tests/Specs/MouseAndDynamicTests.cs::MouseActionTests.TC_MOUSE_005_Slider_Max_Handle` |

## Summary

| Group | Cases | Automated |
| --- | --- | --- |
| Mouse and keyboard actions | 5 | 5 |
| **Total** | **5** | **5** |
