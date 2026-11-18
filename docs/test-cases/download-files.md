# Download Files Test Cases

**Automation:** `tests/Automation.Tests/Specs/` (HiddenAjaxAndDownloadTests.cs)  |  **Report:** `Reports/download-files/Download Files test <DD-MM-YYYY>_<HH-mm-ss>.html` (all reports: `Reports/index.html`)
**Application:** https://testautomationpractice.blogspot.com/  |  **ID prefixes:** `TC_DOWNLOAD_`  |  **Master CSV:** `docs/test-cases/download-files.csv`

## Overview
The Download Files page generates a text file and a PDF file in the browser from typed text.

## Scope and notes
- The browser downloads into a dedicated folder and the cases read the file from disk.
- Site quirk: the "PDF" holds the typed text only; it is not a real PDF document.

## Downloads

| ID | Title | Priority | Type | Preconditions | Test data | Steps | Expected result | Automation |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| TC_DOWNLOAD_001 | Verify a text file generated from entered text can be downloaded with correct content | P1 - Critical | Functional | Browser is launched; 'Download Files' page is open; browser download folder is empty | Text: Hello from Selenium .NET | 1. Enter text in the text area<br>2. Click 'Generate and Download Text File'<br>3. Click 'Download Text File'<br>4. Wait for info.txt in the download folder<br>5. Read the file content | info.txt is downloaded and its content equals the entered text | Automated `tests/Automation.Tests/Specs/HiddenAjaxAndDownloadTests.cs::DownloadFilesTests.TC_DOWNLOAD_001_Download_Text_File` |
| TC_DOWNLOAD_002 | Verify a PDF file generated from entered text can be downloaded | P2 - High | Functional | Browser is launched; 'Download Files' page is open; browser download folder is empty | Text: PDF content | 1. Enter text in the text area<br>2. Click 'Generate and Download PDF File'<br>3. Click 'Download PDF File'<br>4. Wait for info.pdf in the download folder<br>5. Check the file size | info.pdf is downloaded and is not empty | Automated `tests/Automation.Tests/Specs/HiddenAjaxAndDownloadTests.cs::DownloadFilesTests.TC_DOWNLOAD_002_Download_Pdf_File` |

## Summary

| Group | Cases | Automated |
| --- | --- | --- |
| Downloads | 2 | 2 |
| **Total** | **2** | **2** |
