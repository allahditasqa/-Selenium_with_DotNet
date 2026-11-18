# Hidden Elements and AJAX Test Cases

**Automation:** `tests/Automation.Tests/Specs/` (HiddenAjaxAndDownloadTests.cs)  |  **Report:** `Reports/hidden-elements-and-ajax/Hidden Elements and AJAX test <DD-MM-YYYY>_<HH-mm-ss>.html` (all reports: `Reports/index.html`)
**Application:** https://testautomationpractice.blogspot.com/  |  **ID prefixes:** `TC_HIDDEN_`  |  **Master CSV:** `docs/test-cases/hidden-elements-and-ajax.csv`

## Overview
The 'Hidden Elements & AJAX' page: show/hide controls and content loaded by AJAX after a delay.

## Scope and notes
- Locators are scoped to `#container` because the blog repeats element ids.

## Hidden elements and AJAX

| ID | Title | Priority | Type | Preconditions | Test data | Steps | Expected result | Automation |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| TC_HIDDEN_001 | Verify Input Box 2 is hidden initially and can be shown and hidden with the toggle button | P2 - High | Functional | Browser is launched; 'Hidden Elements & AJAX' page is open | - | 1. Verify Input Box 2 is not visible<br>2. Click 'Toggle Input Box 2'<br>3. Verify it is visible and status says 'Input Box 2 shown'<br>4. Click the toggle again<br>5. Verify it is hidden and status says 'Input Box 2 hidden' | Visibility switches hidden -> shown -> hidden with matching status messages | Automated `tests/Automation.Tests/Specs/HiddenAjaxAndDownloadTests.cs::HiddenAjaxTests.TC_HIDDEN_001_Toggle_Input_Box_2` |
| TC_HIDDEN_002 | Verify Checkbox 2 is hidden initially and becomes visible with the toggle button | P2 - High | Functional | Browser is launched; 'Hidden Elements & AJAX' page is open | - | 1. Verify Checkbox 2 is not visible<br>2. Click 'Toggle Checkbox 2'<br>3. Verify it is visible and status says 'Checkbox 2 shown' | Checkbox 2 becomes visible and the status message is 'Checkbox 2 shown' | Automated `tests/Automation.Tests/Specs/HiddenAjaxAndDownloadTests.cs::HiddenAjaxTests.TC_HIDDEN_002_Toggle_Checkbox_2` |
| TC_HIDDEN_003 | Verify AJAX content loads after clicking 'Load AJAX Content' | P1 - Critical | Functional | Browser is launched; 'Hidden Elements & AJAX' page is open | - | 1. Click 'Load AJAX Content'<br>2. Wait for the content to appear (asynchronous)<br>3. Read the loaded content | Heading 'AJAX Content Loaded' is displayed after the delay | Automated `tests/Automation.Tests/Specs/HiddenAjaxAndDownloadTests.cs::HiddenAjaxTests.TC_HIDDEN_003_Load_Ajax_Content` |
| TC_HIDDEN_004 | Verify the visible Input Box 1 accepts text | P3 - Medium | Functional | Browser is launched; 'Hidden Elements & AJAX' page is open | Text: visible input | 1. Type text in Input Box 1<br>2. Read the field value | Field shows 'visible input' | Automated `tests/Automation.Tests/Specs/HiddenAjaxAndDownloadTests.cs::HiddenAjaxTests.TC_HIDDEN_004_Type_In_Visible_Input` |

## Summary

| Group | Cases | Automated |
| --- | --- | --- |
| Hidden elements and AJAX | 4 | 4 |
| **Total** | **4** | **4** |
