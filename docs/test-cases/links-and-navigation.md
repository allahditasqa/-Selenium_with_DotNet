# Links and Navigation Test Cases

**Automation:** `tests/Automation.Tests/Specs/` (LinksAndNavigationTests.cs)  |  **Report:** `Reports/links-and-navigation/Links and Navigation test <DD-MM-YYYY>_<HH-mm-ss>.html` (all reports: `Reports/index.html`)
**Application:** https://testautomationpractice.blogspot.com/  |  **ID prefixes:** `TC_LINK_`, `TC_NAV_`  |  **Master CSV:** `docs/test-cases/links-and-navigation.csv`

## Overview
Labels, laptop links, broken links, header menu, footer links, visitors counter and the three form sections.

## Scope and notes
- Broken link and working link status cases call external sites and need internet access.

## A. Labels, links and broken links

| ID | Title | Priority | Type | Preconditions | Test data | Steps | Expected result | Automation |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| TC_LINK_001 | Verify Mobile Labels are displayed | P4 - Low | UI | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | - | 1. Scroll to 'Labels And Links'<br>2. Read the 'Mobile Labels' items | Labels 'Samsung', 'Real Me' and 'Moto' are displayed in this order | Automated `tests/Automation.Tests/Specs/LinksAndNavigationTests.cs::LinksAndNavigationTests.TC_LINK_001_Mobile_Labels` |
| TC_LINK_002 | Verify Laptop Links point to the correct sites | P3 - Medium | Functional | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | - | 1. Scroll to 'Laptop Links'<br>2. Read the href of Apple, Lenovo and Dell | Hrefs contain apple.com, lenovo.com and dell.com respectively | Automated `tests/Automation.Tests/Specs/LinksAndNavigationTests.cs::LinksAndNavigationTests.TC_LINK_002_Laptop_Links` |
| TC_LINK_003 | Verify all 8 Broken Links are listed with their error codes | P3 - Medium | UI | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Error codes: 400, 401, 403, 404, 408, 500, 502, 503 | 1. Scroll to 'Broken Links'<br>2. Count the links<br>3. Read the href of every link | 8 links exist and each href ends with its own error code | Automated `tests/Automation.Tests/Specs/LinksAndNavigationTests.cs::LinksAndNavigationTests.TC_LINK_003_Broken_Links_Are_Listed` |
| TC_LINK_004 | Verify the HTTP status of every Broken Link is checked | P3 - Medium | Integration | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open; internet access is available | 8 URLs from the Broken Links section | 1. Collect the href of all 8 Broken Links<br>2. Send an HTTP GET request to every URL<br>3. Record the status code | Every link is requested; each returns a status code or is reported as unreachable | Automated `tests/Automation.Tests/Specs/LinksAndNavigationTests.cs::LinksAndNavigationTests.TC_LINK_004_Broken_Links_Http_Status` |
| TC_LINK_005 | Verify Laptop Links respond without a server error | P3 - Medium | Integration | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open; internet access is available | URLs: apple.com, lenovo.com, dell.com | 1. Collect the href of Apple, Lenovo and Dell<br>2. Send an HTTP GET request to each<br>3. Read the status code | No link returns a 5xx status code | Automated `tests/Automation.Tests/Specs/LinksAndNavigationTests.cs::LinksAndNavigationTests.TC_LINK_005_Working_Links_Respond` |

## B. Navigation, footer and form sections

| ID | Title | Priority | Type | Preconditions | Test data | Steps | Expected result | Automation |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| TC_NAV_001 | Verify the header menu contains all navigation links | P2 - High | UI | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Expected menu: Home, Udemy Courses, Online Trainings, Blog, PlaywrightPractice | 1. Read all links of the top menu<br>2. Read the href of 'PlaywrightPractice' | All 5 links are present; PlaywrightPractice points to the playwrightpractice page | Automated `tests/Automation.Tests/Specs/LinksAndNavigationTests.cs::LinksAndNavigationTests.TC_NAV_001_Header_Menu_Links` |
| TC_NAV_002 | Verify the footer link 'Hidden Elements & AJAX' opens its page | P2 - High | Functional | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | - | 1. Scroll to the footer<br>2. Click 'Hidden Elements & AJAX'<br>3. Read the page URL | URL contains 'gui-elements-ajax-hidden' | Automated `tests/Automation.Tests/Specs/LinksAndNavigationTests.cs::LinksAndNavigationTests.TC_NAV_002_Footer_Link_Hidden_Elements` |
| TC_NAV_003 | Verify the footer link 'Download Files' opens its page | P2 - High | Functional | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | - | 1. Scroll to the footer<br>2. Click 'Download Files'<br>3. Read the page URL | URL contains 'download-files' | Automated `tests/Automation.Tests/Specs/LinksAndNavigationTests.cs::LinksAndNavigationTests.TC_NAV_003_Footer_Link_Download_Files` |
| TC_NAV_004 | Verify the Visitors counter displays a number | P4 - Low | UI | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | - | 1. Scroll to the 'Visitors' widget<br>2. Read the total count | Counter shows a numeric value (digits with optional thousand separators) | Automated `tests/Automation.Tests/Specs/LinksAndNavigationTests.cs::LinksAndNavigationTests.TC_NAV_004_Visitors_Counter` |
| TC_NAV_005 | Verify Form Sections 1, 2 and 3 show their heading and paragraph and accept input (data-driven) | P3 - Medium | Functional | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Section N = 1, 2, 3; Text: 'hello N' | 1. Scroll to the Form section N<br>2. Read the heading and the paragraph<br>3. Type text in the section input<br>4. Click the section Submit button<br>5. Read the input value | Heading is 'Section N'; paragraph is 'This is a paragraph in Section N.'; input keeps the typed text | Automated `tests/Automation.Tests/Specs/LinksAndNavigationTests.cs::LinksAndNavigationTests.TC_NAV_005_Form_Sections` |

## Summary

| Group | Cases | Automated |
| --- | --- | --- |
| A. Labels, links and broken links | 5 | 5 |
| B. Navigation, footer and form sections | 5 | 5 |
| **Total** | **10** | **10** |
