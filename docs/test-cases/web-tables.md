# Web Tables Test Cases

**Automation:** `tests/Automation.Tests/Specs/` (WebTableTests.cs)  |  **Report:** `Reports/web-tables/Web Tables test <DD-MM-YYYY>_<HH-mm-ss>.html` (all reports: `Reports/index.html`)
**Application:** https://testautomationpractice.blogspot.com/  |  **ID prefixes:** `TC_TABLE_`  |  **Master CSV:** `docs/test-cases/web-tables.csv`

## Overview
The static book table, the dynamic table (values change on every refresh) and the paginated product table.

## Scope and notes
- The dynamic table is read by header text because its column order changes.

## Web tables

| ID | Title | Priority | Type | Preconditions | Test data | Steps | Expected result | Automation |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| TC_TABLE_001 | Verify Static Web Table headers and number of rows | P2 - High | UI | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Expected headers: BookName, Author, Subject, Price | 1. Scroll to 'Static Web Table'<br>2. Read the column headers<br>3. Count the data rows | Headers are BookName, Author, Subject, Price; table has 6 data rows | Automated `tests/Automation.Tests/Specs/WebTableTests.cs::WebTableTests.TC_TABLE_001_Static_Table_Headers_And_Row_Count` |
| TC_TABLE_002 | Verify books can be filtered by author in the Static Web Table | P3 - Medium | Functional | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Author: Mukesh | 1. Read all rows of the Static Web Table<br>2. Filter rows where Author = 'Mukesh'<br>3. Collect book names | Books 'Learn Java' and 'Master In Selenium' are returned | Automated `tests/Automation.Tests/Specs/WebTableTests.cs::WebTableTests.TC_TABLE_002_Static_Table_Filter_By_Author` |
| TC_TABLE_003 | Verify the total price of all books in the Static Web Table | P3 - Medium | Validation | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Prices: 300, 500, 300, 3000, 2000, 1000 | 1. Read the Price column of all 6 rows<br>2. Add the prices | Total price is 7100 | Automated `tests/Automation.Tests/Specs/WebTableTests.cs::WebTableTests.TC_TABLE_003_Static_Table_Total_Price` |
| TC_TABLE_004 | Verify the Chrome CPU value in the Dynamic Web Table matches the value shown below the table | P1 - Critical | Validation | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Process: Chrome; Column: CPU (%) | 1. Locate the 'Chrome' row and the 'CPU (%)' column (column order changes on every load)<br>2. Read the CPU value from the table<br>3. Read 'CPU load of Chrome process' text below the table<br>4. Compare both | Both values are identical | Automated `tests/Automation.Tests/Specs/WebTableTests.cs::WebTableTests.TC_TABLE_004_Dynamic_Table_Chrome_Cpu_Matches_Label` |
| TC_TABLE_005 | Verify the Dynamic Web Table has 4 processes and every value carries the correct unit | P3 - Medium | Validation | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Units: MB, Mbps, MB/s, % | 1. Count the rows of the Dynamic Web Table<br>2. Read Firefox Memory, System Network, Internet Explorer Disk and Chrome CPU values | 4 rows; values end with MB, Mbps, MB/s and % respectively | Automated `tests/Automation.Tests/Specs/WebTableTests.cs::WebTableTests.TC_TABLE_005_Dynamic_Table_Rows_And_Units` |
| TC_TABLE_006 | Verify Pagination Web Table shows 5 products on each of its 4 pages | P1 - Critical | Functional | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Pages: 1 to 4 | 1. Count the pagination links<br>2. Click page 1, 2, 3 and 4 one by one<br>3. On each page count the product rows | There are 4 pages; each page shows 5 rows (20 products in total) | Automated `tests/Automation.Tests/Specs/WebTableTests.cs::WebTableTests.TC_TABLE_006_Pagination_Table_Pages_And_Rows` |
| TC_TABLE_007 | Verify the first row of the Pagination Web Table on page 1 | P3 - Medium | Validation | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Expected: 1, Smartphone, $10.99 | 1. Stay on page 1 of the Pagination Web Table<br>2. Read ID, Name and Price of the first row | First row is ID 1, Name 'Smartphone', Price '$10.99' | Automated `tests/Automation.Tests/Specs/WebTableTests.cs::WebTableTests.TC_TABLE_007_Pagination_Table_First_Row` |
| TC_TABLE_008 | Verify product checkboxes can be selected on different pages of the Pagination Web Table | P2 - High | Functional | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Product: Laptop; Page: 2 (all products) | 1. On page 1 tick the checkbox of 'Laptop'<br>2. Verify it is ticked<br>3. Go to page 2<br>4. Tick all checkboxes on page 2<br>5. Verify all are ticked | Selected checkboxes are ticked on each page | Automated `tests/Automation.Tests/Specs/WebTableTests.cs::WebTableTests.TC_TABLE_008_Pagination_Table_Select_Checkboxes` |

## Summary

| Group | Cases | Automated |
| --- | --- | --- |
| Web tables | 8 | 8 |
| **Total** | **8** | **8** |
