# File Upload Test Cases

**Automation:** `tests/Automation.Tests/Specs/` (FileUploadTests.cs)  |  **Report:** `Reports/file-upload/File Upload test <DD-MM-YYYY>_<HH-mm-ss>.html` (all reports: `Reports/index.html`)
**Application:** https://testautomationpractice.blogspot.com/  |  **ID prefixes:** `TC_UPLOAD_`  |  **Master CSV:** `docs/test-cases/file-upload.csv`

## Overview
Single and multiple file upload with the status message of the site.

## Scope and notes
- Small text files are created by the test in a temporary folder.

## File upload

| ID | Title | Priority | Type | Preconditions | Test data | Steps | Expected result | Automation |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| TC_UPLOAD_001 | Verify a single file can be uploaded | P1 - Critical | Functional | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | File: single-upload.txt (text file) | 1. Choose one file in 'Upload Single File'<br>2. Click the 'Upload Single File' button<br>3. Read the status message | Status message confirms the upload and contains the file name 'single-upload.txt' | Automated `tests/Automation.Tests/Specs/FileUploadTests.cs::FileUploadTests.TC_UPLOAD_001_Upload_Single_File` |
| TC_UPLOAD_002 | Verify multiple files can be uploaded together | P1 - Critical | Functional | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | Files: one.txt, two.txt, three.txt | 1. Choose three files in 'Upload Multiple Files'<br>2. Click the 'Upload Multiple Files' button<br>3. Read the status message | Status message lists all three file names | Automated `tests/Automation.Tests/Specs/FileUploadTests.cs::FileUploadTests.TC_UPLOAD_002_Upload_Multiple_Files` |
| TC_UPLOAD_003 | Verify upload without choosing a file shows a message | P2 - High | Negative | Browser is launched; practice site home page (https://testautomationpractice.blogspot.com/) is open | File: (none) | 1. Do not choose any file<br>2. Click the 'Upload Single File' button<br>3. Read the status message | A message asking the user to select a file is displayed; nothing is uploaded | Automated `tests/Automation.Tests/Specs/FileUploadTests.cs::FileUploadTests.TC_UPLOAD_003_Upload_Without_File` |

## Summary

| Group | Cases | Automated |
| --- | --- | --- |
| File upload | 3 | 3 |
| **Total** | **3** | **3** |
