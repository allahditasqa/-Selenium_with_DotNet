# Test Case Standard (the same format in every module)

Every test case of this project is written in this format, so anyone can read, review and run a case without help.
Language: simple English, short sentences, exact UI text in quotes.

## 1. Folder structure

| Item | Location | Example |
| --- | --- | --- |
| Test case document (source of truth) | `docs/test-cases/<module-slug>.md` | `docs/test-cases/web-tables.md` |
| **Master CSV** (regression list, one fixed file per module) | `docs/test-cases/<module-slug>.csv` | `docs/test-cases/web-tables.csv` |
| Automation code | `tests/Automation.Tests/Specs/<Name>Tests.cs` | `WebTableTests.cs` |
| Page objects | `tests/Automation.Tests/Pages/` | `WebTablesPage.cs` |
| Run report (new file every run) | `Reports/<module-slug>/<Module> test <DD-MM-YYYY>_<HH-mm-ss>.html` | `Reports/web-tables/Web Tables test 18-11-2026_12-16-28.html` |
| Run CSV (cases that ran) | `Reports/<module-slug>/<Module> test cases <DD-MM-YYYY>_<HH-mm-ss>.csv` | |
| History of runs | `Reports/index.html`, `Reports/history.json` | |

## 2. Test case ID

`TC_<AREA>_<NNN>` (NNN = 001, 002 ...), for example `TC_TABLE_004`. An ID is never used twice or reused.
A data-driven test (`[TestCase]`) keeps one ID; the data sets are listed in the **Test data** column.

The ID is in the code in two places, and both must match the document:

| Place | Example |
| --- | --- |
| Test method name starts with the ID | `public void TC_TABLE_004_Dynamic_Table_Chrome_Cpu_Matches_Label()` |
| `[TestSpec("<ID>", "<title>", ...)]` on the method | `[TestSpec("TC_TABLE_004", "Verify ...", Priority.P1_Critical, TestType.Validation, ...)]` |

The `[TestSpec]` attribute holds the whole written case (title, priority, type, preconditions, steps separated by `|`, test data, expected result).
The report and the run CSV are generated from it. The document and master CSV must say the same (checked automatically).

## 3. Document format

1. `# <Module> Test Cases`
2. Header lines: automation files, report location, application URL, ID prefixes, master CSV
3. **Overview**, **Scope and notes**
4. One table per group (`## A. ...`) with the columns below
5. **Summary** (cases and automated per group, total)

| Column | Meaning |
| --- | --- |
| **ID** | Rule above |
| **Title** | One sentence starting with "Verify ..." |
| **Priority** | `P1 - Critical`, `P2 - High`, `P3 - Medium`, `P4 - Low` |
| **Type** | Functional, Negative, Validation, UI, Integration, Smoke |
| **Preconditions** | State before step 1 |
| **Test data** | Exact data, `-` if none |
| **Steps** | Numbered, action based |
| **Expected result** | Clear and checkable |
| **Automation** | `Automated` + `<file>::<Class>.<Method>` |

Master CSV columns: `ID, Module, Sub-module, Title, Priority, Type, Preconditions, Test Data, Steps, Expected Result, Automation Status, Test File, Test Method, Last Updated`.
The run CSV in `Reports/` has the 17 execution columns (status, actual result, duration, browser, URL ...).

## 4. Rules
1. One case checks one thing.
2. Only automated cases are documented, and every automated case is documented.
3. New case = new ID; never reuse an ID. Removing a case removes test and row together.
4. Genuine site defects are marked `[Ignore]`/expected-failure with a reason and listed in `docs/DEFECTS.md`.

## 5. Keeping documents, CSV and code in sync

| Command | Purpose |
| --- | --- |
| `powershell scripts/Build-CaseCsv.ps1` | Rebuild master CSV from the `.md` files (unchanged rows keep `Last Updated`) |
| `powershell scripts/Build-CaseCsv.ps1 -Check` | Fail if a CSV is older than its document |
| `dotnet test --filter "Category=Docs"` | `DocsTraceabilityTests` compares code (reflection over `[TestSpec]`) with the documents: ID, title, priority, type, steps, expected result, test method |

CI runs the CSV check and the traceability test on every push.

## 6. Checklist for a new case
- [ ] Row (new ID) added to `docs/test-cases/<module>.md`
- [ ] Test method `TC_<AREA>_<NNN>_<Name>` with `[TestSpec]` identical to the row
- [ ] `Build-CaseCsv.ps1` run, `dotnet test --filter "Category=Docs"` green
- [ ] Full run done, report checked, README counts updated
