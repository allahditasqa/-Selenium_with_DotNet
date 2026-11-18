# Date Pickers Test Cases

**Automation:** `tests/Automation.Tests/Specs/` (DatePickerTests.cs)  |  **Report:** `Reports/date-pickers/Date Pickers test <DD-MM-YYYY>_<HH-mm-ss>.html` (all reports: `Reports/index.html`)
**Application:** https://testautomationpractice.blogspot.com/  |  **ID prefixes:** `TC_DATE_`  |  **Master CSV:** `docs/test-cases/date-pickers.csv`

## Overview
The three date widgets of the practice site: the calendar picker, the month/year dropdown picker and the date range picker with Submit.

## Scope and notes
- Test data is listed per case in the Test data column.

## Date pickers

| ID | Title | Priority | Type | Preconditions | Test data | Steps | Expected result | Automation |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| TC_DATE_001 | Verify Date Picker 1 (mm/dd/yyyy) selects a date from the calendar popup | P2 - High | Functional | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Day: 15 (current month) | 1. Click the 'Date Picker 1' field<br>2. Verify the calendar popup opens<br>3. Click day 15 in the calendar<br>4. Read the field value | Field shows the date in mm/dd/yyyy format with day 15 | Automated `tests/Automation.Tests/Specs/DatePickerTests.cs::DatePickerTests.TC_DATE_001_DatePicker1_Pick_From_Calendar` |
| TC_DATE_002 | Verify Date Picker 2 (dd/mm/yyyy) selects a date using the month and year dropdowns | P2 - High | Functional | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Month: Mar; Year: 2025; Day: 10 | 1. Click the 'Date Picker 2' field<br>2. Select month 'Mar' from the month dropdown<br>3. Select year '2025' from the year dropdown<br>4. Click day 10<br>5. Read the field value | Field shows 10/03/2025 (dd/mm/yyyy) | Automated `tests/Automation.Tests/Specs/DatePickerTests.cs::DatePickerTests.TC_DATE_002_DatePicker2_Select_Month_And_Year` |
| TC_DATE_003 | Verify Date Picker 3 calculates the number of days for a valid date range | P1 - Critical | Functional | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Start: 01-Jan-2026; End: 31-Jan-2026 | 1. Enter Start Date<br>2. Enter End Date after the start date<br>3. Click Submit<br>4. Read the result message | Message 'You selected a range of 30 days.' is displayed | Automated `tests/Automation.Tests/Specs/DatePickerTests.cs::DatePickerTests.TC_DATE_003_DatePicker3_Valid_Range` |
| TC_DATE_004 | Verify Date Picker 3 rejects an end date earlier than the start date | P2 - High | Negative | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Start: 10-Feb-2026; End: 01-Feb-2026 | 1. Enter Start Date<br>2. Enter an End Date earlier than the start date<br>3. Click Submit<br>4. Read the result message | Validation message 'End date must be after start date.' is displayed | Automated `tests/Automation.Tests/Specs/DatePickerTests.cs::DatePickerTests.TC_DATE_004_DatePicker3_End_Before_Start` |
| TC_DATE_005 | Verify Date Picker 3 requires both dates before submitting | P2 - High | Negative | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Start: (empty); End: (empty) | 1. Leave Start Date and End Date empty<br>2. Click Submit<br>3. Read the result message | Message 'Please select both start and end dates.' is displayed | Automated `tests/Automation.Tests/Specs/DatePickerTests.cs::DatePickerTests.TC_DATE_005_DatePicker3_Missing_Dates` |

## Summary

| Group | Cases | Automated |
| --- | --- | --- |
| Date pickers | 5 | 5 |
| **Total** | **5** | **5** |
