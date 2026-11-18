# Alerts and Windows Test Cases

**Automation:** `tests/Automation.Tests/Specs/` (AlertsAndWindowsTests.cs)  |  **Report:** `Reports/alerts-and-windows/Alerts and Windows test <DD-MM-YYYY>_<HH-mm-ss>.html` (all reports: `Reports/index.html`)
**Application:** https://testautomationpractice.blogspot.com/  |  **ID prefixes:** `TC_ALERT_`, `TC_WINDOW_`  |  **Master CSV:** `docs/test-cases/alerts-and-windows.csv`

## Overview
JavaScript alerts (simple, confirm, prompt) and the buttons that open a new tab or popup windows.

## Scope and notes
- New tab and popup cases need internet access to third-party sites (Pavan blog, selenium.dev, playwright.dev).

## A. JavaScript alerts

| ID | Title | Priority | Type | Preconditions | Test data | Steps | Expected result | Automation |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| TC_ALERT_001 | Verify the Simple Alert shows its message and closes on OK | P2 - High | Functional | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | - | 1. Click 'Simple Alert'<br>2. Read the alert text<br>3. Click OK | Alert text is 'I am an alert box!'; the alert closes after OK | Automated `tests/Automation.Tests/Specs/AlertsAndWindowsTests.cs::AlertsAndWindowsTests.TC_ALERT_001_Simple_Alert` |
| TC_ALERT_002 | Verify Confirmation Alert - OK shows 'You pressed OK!' | P2 - High | Functional | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Action: OK | 1. Click 'Confirmation Alert'<br>2. Click OK in the alert<br>3. Read the result text on the page | Page shows 'You pressed OK!' | Automated `tests/Automation.Tests/Specs/AlertsAndWindowsTests.cs::AlertsAndWindowsTests.TC_ALERT_002_Confirm_Alert_Accept` |
| TC_ALERT_003 | Verify Confirmation Alert - Cancel shows 'You pressed Cancel!' | P2 - High | Functional | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Action: Cancel | 1. Click 'Confirmation Alert'<br>2. Click Cancel in the alert<br>3. Read the result text on the page | Page shows 'You pressed Cancel!' | Automated `tests/Automation.Tests/Specs/AlertsAndWindowsTests.cs::AlertsAndWindowsTests.TC_ALERT_003_Confirm_Alert_Dismiss` |
| TC_ALERT_004 | Verify Prompt Alert greets the user with the entered name | P2 - High | Functional | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Name: Allah Ditta | 1. Click 'Prompt Alert'<br>2. Type a name in the prompt<br>3. Click OK<br>4. Read the result text on the page | Result text contains 'Allah Ditta' | Automated `tests/Automation.Tests/Specs/AlertsAndWindowsTests.cs::AlertsAndWindowsTests.TC_ALERT_004_Prompt_Alert_Accept_With_Text` |
| TC_ALERT_005 | Verify Prompt Alert - Cancel does not show a greeting | P3 - Medium | Negative | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Action: Cancel | 1. Click 'Prompt Alert'<br>2. Click Cancel in the prompt<br>3. Read the result text on the page | No 'Hello ...' greeting is displayed | Automated `tests/Automation.Tests/Specs/AlertsAndWindowsTests.cs::AlertsAndWindowsTests.TC_ALERT_005_Prompt_Alert_Dismiss` |

## B. New tab and popup windows

| ID | Title | Priority | Type | Preconditions | Test data | Steps | Expected result | Automation |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| TC_WINDOW_001 | Verify the 'New Tab' button opens the target site in a new browser tab | P2 - High | Functional | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Expected site: pavantestingtools.com | 1. Click 'New Tab'<br>2. Switch to the newly opened tab<br>3. Read its URL<br>4. Close the new tab and return to the original tab | A new tab opens with pavantestingtools.com; after closing it only the original tab remains | Automated `tests/Automation.Tests/Specs/AlertsAndWindowsTests.cs::AlertsAndWindowsTests.TC_WINDOW_001_New_Tab` |
| TC_WINDOW_002 | Verify the 'Popup Windows' button opens additional browser windows | P3 - Medium | Functional | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | - | 1. Click 'Popup Windows'<br>2. Count open windows<br>3. Close all popup windows | At least one extra window opens; after closing them only the original window remains | Automated `tests/Automation.Tests/Specs/AlertsAndWindowsTests.cs::AlertsAndWindowsTests.TC_WINDOW_002_Popup_Windows` |

## Summary

| Group | Cases | Automated |
| --- | --- | --- |
| A. JavaScript alerts | 5 | 5 |
| B. New tab and popup windows | 2 | 2 |
| **Total** | **7** | **7** |
