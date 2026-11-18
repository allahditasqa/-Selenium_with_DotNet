# Test Strategy: Practice Site UI Automation (Selenium + C# / .NET 8 + NUnit)

**Version:** 1.0  |  **Prepared by:** QA (SDET)

## 1. Objective
A complete, repeatable UI suite for all control types of a real web application (forms, date pickers, tables, alerts and windows, mouse actions, dynamic content, hidden elements and AJAX, uploads and downloads), with module-wise evidence for every run.

## 2. Application under test
https://testautomationpractice.blogspot.com/ (public practice site, main page + "Hidden Elements & AJAX" + "Download Files").

## 3. Scope

| Module | Cases | Type |
| --- | --- | --- |
| Data Entry Form | 8 | Functional, Validation, Smoke |
| Date Pickers | 5 | Functional, Negative |
| File Upload | 3 | Functional, Negative |
| Web Tables | 8 | UI, Validation, Functional |
| Alerts and Windows | 7 | Functional, Negative |
| Mouse and Keyboard Actions | 5 | Functional |
| Dynamic Elements | 6 | Functional, UI, Integration |
| Links and Navigation | 10 (12 tests: one case is data-driven with 3 data sets) | UI, Functional, Integration |
| Hidden Elements and AJAX | 4 | Functional |
| Download Files | 2 | Functional |
| **Total** | **58 cases / 60 tests** | |

Out of scope: performance and load, visual regression, accessibility audit, mobile browsers.

## 4. Approach
Page Object Model (framework in `src/`, page objects in `tests/`), a fresh browser per test, explicit waits only, test classes in parallel (4 at a time), written test case on every test (`[TestSpec]`), reports and CSV generated from it. Positive, negative and validation cases; data-driven where the same steps apply to several data sets. External-site cases (Wikipedia, broken links, new windows) need internet access.

## 5. Environment
.NET SDK 8, Selenium 4.27, NUnit 4, Chrome (headless by default), Windows or Linux. Settings in `appsettings.json`, overridable with `AUTOMATION_<KEY>` environment variables.

## 6. Entry and exit criteria
Entry: build succeeds and `dotnet test --filter "Category=Docs"` passes. Exit: all P1 and P2 cases pass; every failure is fixed or logged in `docs/DEFECTS.md`; final run done with no retries.

## 7. Risks

| Risk | Mitigation |
| --- | --- |
| Public site slow or down | Timeouts in settings; failures are reported honestly, not retried away |
| Third-party sites change or block | Few, independent external cases |
| Driver download race in parallel start | Browser start is serialised and retried |
| Locators change | Locators only in page objects |

## 8. Deliverables
Test case documents and master CSV, automation code, module-wise HTML and CSV reports with history (`Reports/`), traceability test, CI pipeline.
