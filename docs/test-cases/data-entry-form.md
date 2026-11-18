# Data Entry Form Test Cases

**Automation:** `tests/Automation.Tests/Specs/` (DataEntryFormTests.cs)  |  **Report:** `Reports/data-entry-form/Data Entry Form test <DD-MM-YYYY>_<HH-mm-ss>.html` (all reports: `Reports/index.html`)
**Application:** https://testautomationpractice.blogspot.com/  |  **ID prefixes:** `TC_FORM_`  |  **Master CSV:** `docs/test-cases/data-entry-form.csv`

## Overview
The Data Entry Form section: text fields, gender radio buttons, day checkboxes, dropdowns and the complete form flow.

## Scope and notes
- Field values come from `TestData/FormData.cs`.
- The Colors list has duplicate options (Red, Green) on the site; the case handles this on purpose.

## Data entry form

| ID | Title | Priority | Type | Preconditions | Test data | Steps | Expected result | Automation |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| TC_FORM_001 | Verify text fields accept valid Name, Email, Phone and Address | P1 - Critical | Functional | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Name: Allah Ditta; Email: allah.ditta@example.com; Phone: 3001234567; Address: House 10, Street 5, Islamabad, Pakistan | 1. Enter a valid name in the Name field<br>2. Enter a valid email in the Email field<br>3. Enter a phone number in the Phone field<br>4. Enter an address in the Address field<br>5. Read back the value of every field | Every field shows exactly the value that was entered | Automated `tests/Automation.Tests/Specs/DataEntryFormTests.cs::DataEntryFormTests.TC_FORM_001_Fill_Text_Fields` |
| TC_FORM_002 | Verify Gender radio buttons are mutually exclusive | P2 - High | Functional | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Gender: Male, then Female | 1. Select the 'Male' radio button<br>2. Verify Male is selected and Female is not<br>3. Select the 'Female' radio button<br>4. Verify Female is selected and Male is not | Only one radio button is selected at any time; selecting Female deselects Male | Automated `tests/Automation.Tests/Specs/DataEntryFormTests.cs::DataEntryFormTests.TC_FORM_002_Gender_Radio_Buttons_Are_Mutually_Exclusive` |
| TC_FORM_003 | Verify all Days checkboxes can be selected and unselected | P3 - Medium | Functional | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Days: Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday | 1. Tick all 7 day checkboxes (Sunday to Saturday)<br>2. Verify all 7 are selected<br>3. Untick all 7 checkboxes<br>4. Verify none is selected | All checkboxes toggle independently; after unticking, no day remains selected | Automated `tests/Automation.Tests/Specs/DataEntryFormTests.cs::DataEntryFormTests.TC_FORM_003_Days_Checkboxes_Select_And_Unselect_All` |
| TC_FORM_004 | Verify only the selected Days checkboxes are checked | P3 - Medium | Functional | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Days: Monday, Friday | 1. Tick 'Monday'<br>2. Tick 'Friday'<br>3. Read the state of all 7 day checkboxes | Only Monday and Friday are selected; the other 5 days stay unselected | Automated `tests/Automation.Tests/Specs/DataEntryFormTests.cs::DataEntryFormTests.TC_FORM_004_Select_Specific_Days` |
| TC_FORM_005 | Verify Country dropdown lists 10 countries and allows selection | P2 - High | Functional | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Country: India | 1. Read all options of the Country dropdown<br>2. Select 'India'<br>3. Read the selected option | Dropdown lists United States, Canada, United Kingdom, Germany, France, Australia, Japan, China, Brazil, India; 'India' is shown as selected | Automated `tests/Automation.Tests/Specs/DataEntryFormTests.cs::DataEntryFormTests.TC_FORM_005_Country_Dropdown` |
| TC_FORM_006 | Verify Colors multi-select list supports selecting multiple options | P3 - Medium | Functional | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Colors: Red, Green | 1. Count the options of the Colors list<br>2. Select 'Red'<br>3. Select 'Green' while keeping Red selected<br>4. Read the selected options | List has 7 options; exactly Red and Green are selected | Automated `tests/Automation.Tests/Specs/DataEntryFormTests.cs::DataEntryFormTests.TC_FORM_006_Colors_Multi_Select` |
| TC_FORM_007 | Verify Sorted List dropdown is in alphabetical order and allows selection | P3 - Medium | Validation | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Animal: Lion | 1. Read all options of the Sorted List dropdown<br>2. Verify they are in ascending alphabetical order<br>3. Select 'Lion'<br>4. Read the selected option | 10 animals (Cat ... Zebra) are listed in alphabetical order; 'Lion' is selected | Automated `tests/Automation.Tests/Specs/DataEntryFormTests.cs::DataEntryFormTests.TC_FORM_007_Animals_Sorted_List` |
| TC_FORM_008 | Verify the complete Data Entry Form can be filled end to end | P1 - Critical | Smoke | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Name: Allah Ditta; Gender: Male; Day: Saturday; Country: Japan; Color: Blue; Animal: Zebra | 1. Fill Name, Email, Phone and Address<br>2. Select gender Male<br>3. Tick Saturday<br>4. Select country Japan<br>5. Select color Blue<br>6. Select animal Zebra<br>7. Verify every control holds the chosen value | All fields and selections retain the entered values | Automated `tests/Automation.Tests/Specs/DataEntryFormTests.cs::DataEntryFormTests.TC_FORM_008_Complete_Form_End_To_End` |

## Summary

| Group | Cases | Automated |
| --- | --- | --- |
| Data entry form | 8 | 8 |
| **Total** | **8** | **8** |
