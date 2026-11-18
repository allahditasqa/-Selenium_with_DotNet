# Selenium with .NET – Test Automation Framework

A complete, production-style **UI test automation framework** built with **C# / .NET 8, Selenium WebDriver 4 and NUnit 4**.
It automates every practice scenario of <https://testautomationpractice.blogspot.com/> and produces **HTML reports** (module-wise, time-stamped, never overwritten).

> **60 automated tests (58 documented cases) · 10 modules · parallel execution · Page Object Model · HTML + CSV reports**

---

## Table of contents

1. [What is inside](#1-what-is-inside)
2. [Tech stack](#2-tech-stack)
3. [Project structure](#3-project-structure)
4. [Architecture (how it works)](#4-architecture-how-it-works)
5. [Setup (step by step)](#5-setup-step-by-step)
6. [Running the tests](#6-running-the-tests)
7. [Configuration](#7-configuration)
8. [Reports](#8-reports)
9. [Test coverage and test cases](#9-test-coverage)
10. [How to add a new test](#10-how-to-add-a-new-test)
11. [Design decisions and best practices](#11-design-decisions-and-best-practices)
12. [Troubleshooting](#12-troubleshooting)

---

## 1. What is inside

| Feature | Details |
|---|---|
| Page Object Model | One class per page/section, no locators inside tests |
| Reusable core | `BasePage` (waits, click, type, select, alerts, windows) and `BaseTest` (browser lifecycle, screenshots, result capture) |
| Multi-browser | Chrome, Edge, Firefox – change one setting |
| Headless / headed | One setting |
| Parallel execution | Test classes run in parallel (4 at a time by default) |
| Explicit waits only | No `Thread.Sleep`, no mixing implicit + explicit waits |
| Config | `appsettings.json` + environment variable overrides |
| Logging | Serilog – console + one log file per run |
| Reports | Self-contained HTML (screenshots embedded) + CSV per module, one shared template, history index |
| Test data | Central `TestData` classes, no magic strings in tests |

## 2. Tech stack

| Tool | Version | Purpose |
|---|---|---|
| .NET SDK | 8.0 | Language / runtime |
| Selenium.WebDriver + Selenium.Support | 4.27 | Browser automation (driver is downloaded automatically by Selenium Manager) |
| NUnit + NUnit3TestAdapter | 4.3 / 4.6 | Test runner and assertions |
| Serilog | 4 | Logging |
| Microsoft.Extensions.Configuration | 8 | Settings |

## 3. Project structure

```
Selenium_with_DotNet/
├── SeleniumDotNet.sln
├── README.md
├── .gitignore
│
├── src/Automation.Framework/            <- reusable framework (no site-specific code)
│   ├── Config/FrameworkSettings.cs      settings from appsettings.json + env variables
│   ├── Drivers/DriverFactory.cs         creates Chrome / Edge / Firefox drivers
│   ├── Pages/BasePage.cs                waits and common actions for every page object
│   ├── Core/BaseTest.cs                 SetUp / TearDown, screenshot, result capture
│   ├── Logging/TestLog.cs               Serilog wrapper
│   └── Reporting/
│       ├── ModuleAttribute.cs           [Module("Web Tables")] groups tests in reports
│       ├── ReportTemplate.cs            ONE shared HTML/CSS template for all reports
│       ├── HtmlReportWriter.cs          writes module reports, CSV, history.json and index.html
│       ├── TestSpecAttribute.cs         written test case (steps, data, expected) kept on each test
│       ├── ReportSession.cs             collects results, writes reports at the end of a run
│       └── TestRecord.cs                result models
│
├── tests/Automation.Tests/              <- the tests for the practice website
│   ├── appsettings.json                 browser, headless, timeouts, folders
│   ├── TestRunHooks.cs                  starts / finishes a run, parallel settings
│   ├── Pages/                           page objects (locators + actions)
│   ├── Specs/                           test classes (assertions only)
│   └── TestData/                        test data and temp-file helper
│
├── Reports/                             generated reports (committed as evidence)
│   ├── index.html                       history of all runs, newest first
│   ├── history.json                     machine-readable run history (append-only)
│   └── <module>/<Module> test dd-MM-yyyy_HH-mm-ss.html (+ .csv)
└── Logs/                                one log file per run (git-ignored)
```

## 4. Architecture (how it works)

```
 Test class (Specs)  ──uses──▶  Page Object (Pages)  ──extends──▶  BasePage  ──▶  Selenium WebDriver
        │                                                                              ▲
        └─ extends BaseTest ── creates driver with DriverFactory ◀── FrameworkSettings ┘
                   │
                   └─ after every test: result + screenshot ─▶ ReportSession ─▶ HtmlReportWriter ─▶ Reports/
```

1. **Test** says *what* to check (`Assert`).
2. **Page object** knows *how* to talk to the page (locators and actions).
3. **BasePage** holds the smart waits so tests are stable.
4. **BaseTest** opens a fresh browser for every test (tests never depend on each other) and quits it at the end.
5. **ReportSession** collects every result; when the run finishes it writes the reports.

## 5. Setup (step by step)

1. Install the **.NET 8 SDK**: <https://dotnet.microsoft.com/download/dotnet/8.0>
   (check with `dotnet --version`)
2. Install **Google Chrome** (or Edge / Firefox).
3. Clone and build:

```powershell
git clone https://github.com/allahditasqa/-Selenium_with_DotNet.git
cd -Selenium_with_DotNet
dotnet build
```

No driver download is needed – Selenium Manager fetches the right `chromedriver` automatically on the first run.

## 6. Running the tests

Run everything:

```powershell
dotnet test
```

Run one module / category:

```powershell
dotnet test --filter "Category=Table"
dotnet test --filter "Category=Alerts"
```

Run one test:

```powershell
dotnet test --filter "Name~TC_MOUSE_003"
```

Watch the browser (not headless) or use another browser – without editing any file:

```powershell
$env:AUTOMATION_HEADLESS = "false"
$env:AUTOMATION_BROWSER  = "firefox"
dotnet test
```

Categories: `Form`, `DatePicker`, `Upload`, `Table`, `Alerts`, `Actions`, `Dynamic`, `Links`, `Hidden`, `Download`.

## 7. Configuration

File: `tests/Automation.Tests/appsettings.json`

| Key | Default | Meaning |
|---|---|---|
| `BaseUrl` | practice site | Application under test |
| `Browser` | `chrome` | `chrome`, `edge` or `firefox` |
| `Headless` | `true` | `false` shows the browser |
| `WindowWidth` / `WindowHeight` | 1366 / 768 | Browser size |
| `ExplicitWaitSeconds` | 15 | Max wait for an element |
| `PageLoadTimeoutSeconds` | 60 | Max wait for a page load |
| `ScreenshotOnPass` | `true` | `false` = screenshots only for failures and `Snap()` calls (smaller reports) |
| `ReportsDirectory`, `LogsDirectory`, `DownloadsDirectory` | folders next to the `.sln` | Output folders |

Any key can be overridden with an environment variable named `AUTOMATION_<KEY>` (example: `AUTOMATION_BROWSER=edge`). This is how a CI pipeline changes settings.

## 8. Reports

After every run you get:

* **How to generate:** run `dotnet test` (the layout below is created automatically). **Latest committed runs:** 18-11-2026 14:35 and 14:27 (60 tests, 0 failed each; the two runs overlapped on the same machine) after the first run at 12:16; all are listed in `Reports/index.html` (newest on top). Reports come from real runs only.
* **`Reports/index.html`** – history page. Newest run on top, and under each date-time the module-wise results (total / pass / fail / skip) with links.
* **`Reports/<module>/<Module> test dd-MM-yyyy_HH-mm-ss.html`** – full module report: summary, every test case with status, duration, error message, stack trace and screenshots (click a screenshot to enlarge).
* **`Reports/<module>/<Module> test cases dd-MM-yyyy_HH-mm-ss.csv`** – the same test cases as a CSV.

### Test case format (CSV)

Every automated test carries its written test case in a `[TestSpec(...)]` attribute, so documentation and automation never drift apart. The CSV is a standard test-case sheet:

| Column | Meaning |
|---|---|
| Test Case ID | Unique id, e.g. `TC_TABLE_004` |
| Module | Functional area (report grouping) |
| Test Case Title | One-line objective starting with "Verify ..." |
| Priority | P1 Critical / P2 High / P3 Medium / P4 Low |
| Test Type | Functional, Negative, Validation, UI, Integration, Smoke |
| Preconditions | State required before the test starts |
| Test Steps | Numbered steps (1. 2. 3. ...) |
| Test Data | Exact data used |
| Expected Result | What must happen |
| Actual Result | "As expected" on pass, otherwise the failure message |
| Execution Status | Pass / Fail / Skipped |
| Execution Type / Automation Method | Automated, plus `Class.Method` that runs it |
| Duration, Executed On, Browser, Environment | Run details |

The HTML report shows the same details under "Test case details" for each test.

Important rules:

* **Every run creates new files.** The file name contains the run time (with seconds); if a name already exists a `(2)` suffix is added. Old reports are **never overwritten or deleted**.
* `history.json` only grows; `index.html` is rebuilt from it, so all previous runs stay listed.
* All reports use **one shared template** (`ReportTemplate.cs`). Change the look there once and every new report follows. Light and dark mode are supported.
* Reports are self-contained single files (screenshots are embedded) – you can email or archive them.

## 9. Test coverage

| Module (report) | Practice-site area | Test cases |
|---|---|---|
| Data Entry Form | Name / email / phone / address, gender radios, day checkboxes, country, multi-select colors, sorted animal list, full flow | 8 |
| Date Pickers | jQuery date picker, month/year dropdown picker, date-range picker (valid, reversed, empty) | 5 |
| File Upload | Single file, multiple files, upload without file | 3 |
| Web Tables | Static table, dynamic (shuffling) table, paginated table with checkboxes | 8 |
| Alerts and Windows | Simple / confirm / prompt alerts, new tab, popup windows | 7 |
| Mouse and Keyboard Actions | Hover menu, double click, drag and drop, range slider (2 handles) | 5 |
| Dynamic Elements | Dynamic START/STOP button, scrolling dropdown (lazy loaded), SVG, Shadow DOM, Wikipedia search widget | 6 |
| Links and Navigation | Labels, laptop links, broken links (href + HTTP status), header menu, footer links, visitors counter, form sections 1–3 (3 data-driven cases) | 12 |
| Hidden Elements and AJAX | Hidden input / checkbox toggles, AJAX content, visible input | 4 |
| Download Files | Generate and download a text file and a PDF file, verify the files | 2 |
| **Total** | | **60** |

### Test cases (documents)

Every automated test has a written test case in `docs/`, linked by the same ID (`TC_<AREA>_<NNN>`), which is also the start of the test method name.

| What | Where |
|---|---|
| Standard (format, ID rule, columns) | [`docs/TEST_CASE_STANDARD.md`](docs/TEST_CASE_STANDARD.md) |
| Test strategy | [`docs/TEST_STRATEGY.md`](docs/TEST_STRATEGY.md) |
| One document per module (10 files, **58 cases**, all automated; 60 tests because `TC_NAV_005` runs with 3 data sets) | [`docs/test-cases/*.md`](docs/test-cases/) |
| Master CSV per module | `docs/test-cases/*.csv` |

* `powershell scripts/Build-CaseCsv.ps1` builds the master CSV from the documents (`-Check` only verifies).
* `dotnet test --filter "Category=Docs"` (`DocsTraceabilityTests`) fails if code and documents differ in ID, title, priority, type, steps, expected result or test method. CI (`.github/workflows/ci.yml`) runs build + both checks.

## 10. How to add a new test

1. **Page object** – `tests/Automation.Tests/Pages/MyPage.cs`

```csharp
public class MyPage : BasePage
{
    public MyPage(IWebDriver driver) : base(driver) { }
    public void ClickSave() => Click(By.Id("save"));
    public string Message => GetText(By.Id("message"));
}
```

2. **Test class** – `tests/Automation.Tests/Specs/MyTests.cs`

```csharp
[TestFixture, Module("My Module"), Category("My")]
public class MyTests : BaseTest
{
    [Test]
    [TestSpec("TC_MY_001", "Verify Save shows a success message", Priority.P2_High, TestType.Functional,
        "Home page is open",                         // preconditions
        "Click Save|Read the message",               // steps separated by |
        "-",                                         // test data
        "Message 'Saved' is displayed")]             // expected result
    public void TC_MY_001_Save_Shows_Message()
    {
        new HomePage(Driver).Open();
        var page = new MyPage(Driver);
        page.ClickSave();
        Assert.That(page.Message, Is.EqualTo("Saved"));
    }
}
```

3. Add the row to `docs/test-cases/my-module.md`, run `scripts/Build-CaseCsv.ps1`, then `dotnet test`. A new "My Module" report appears automatically; `Category=Docs` checks the document matches.

Tip: call `Snap("caption")` inside a test to put an extra screenshot into the report.

## 11. Design decisions and best practices

* **Separation of concerns** – framework (`src`) is independent from the application tests (`tests`). Reuse `Automation.Framework` for any other website.
* **Test independence** – new browser per test, no shared state, so tests can run in parallel and in any order.
* **Stable waits** – every action waits for the element to be visible / clickable; stale and not-interactable exceptions are retried.
* **Robust locators** – IDs and scoped CSS first; the dynamic table is read by header text because its column order changes on every refresh; the hidden-elements page scopes locators to `#container` because the blog repeats element ids; duplicate options in the Colors list are handled explicitly.
* **Browser start-up is serialised and retried** – avoids driver-download races when running in parallel.
* **Evidence by default** – every test has a result, duration and screenshot; failures include message and stack trace.
* **No secrets in code** – configuration via file / environment variables.
* **Assertions** – `Assert.Multiple` reports all mismatches of a scenario at once.

## 12. Troubleshooting

| Problem | Fix |
|---|---|
| `dotnet` not found | Install the .NET 8 SDK and open a new terminal |
| Browser does not start | Update Chrome; make sure internet is available for the first driver download |
| Tests are slow | Keep `Headless` = `true`, or raise `LevelOfParallelism` in `TestRunHooks.cs` |
| `Wikipedia_Search` / link-status tests fail | These need internet access to external sites (Wikipedia, deadlinkcity.com, apple.com …) |
| Reports folder is large | Set `ScreenshotOnPass` to `false` |
| Want a clean start | Delete `Reports/` (history.json and index.html are rebuilt on the next run) |

---

**Author:** Allah Ditta · QA Automation Engineer / SDET · [GitHub](https://github.com/allahditasqa)
