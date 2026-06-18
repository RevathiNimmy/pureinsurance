# Back Office & Portal Code Inspection Rules

> **Companion to `API_inspection.md`.** That file covers REST API (PureInsurance.REST)
> inspection. This file covers Back Office (WinForms), Portal (Nexus), Database (SPs),
> and Navigator XM changes — all within the `PureInsurance` repository.

**Last Updated**: 2025-07-18
**Owned By**: Pure Insurance Team

---

## Automatic Triggers (No keyword needed)
- **PR creation from feature branch to `main`** with Back Office / Portal / DB changes → Run PRE-PR CODE GATE automatically

## Keyword Triggers
- **"BO Inspect"** → Run the PRE-PR CODE GATE manually on the current branch
- **"Portal Inspect"** → Run only the Portal-specific checks (STEP 3 subset)
- **"DB Inspect"** → Run only the Database-specific checks (STEP 4 subset)

---

## PRE-PR CODE GATE — AUTOMATIC BEFORE FEATURE → MAIN PR

> **This gate runs automatically when merging a completed feature into `main`.**
> Whenever an AI agent is about to raise a PR from a **feature/integration branch**
> (`feature/ADO-[epic-id]-[feature-name]`) **to `main`** (Mode A step 24 or
> Mode B step 20) and the feature branch contains changes to ANY file in the
> `PureInsurance` repository (Back Office, Portal, Database, Navigator XM),
> the agent MUST execute this gate **before** creating the PR to `main`.
>
> **This gate does NOT run for task → feature PRs.** Those are lightweight merges
> within the feature; the full inspection happens once before the feature reaches `main`.
>
> This gate runs **in addition to** the API gate in `API_inspection.md` when
> both repos have changes. They do not replace each other.

### When to Trigger
The agent MUST run this gate when **ALL** of the following are true:
1. The agent is creating a PR from `feature/ADO-*` → `main`
2. **Any** of the following file patterns are detected in the feature branch diff vs `main`:
   - Files under a `b*` or `i*` component directory
   - `.aspx`, `.aspx.vb`, or `.aspx.cs` files
   - `.sql` files under `Databases/`
   - `.XML` files under `Navigator XM Roadmaps/`
   - Files in `Shared Files/`, `gPMLibrary/`, or any `g*Library`
   - Files in `Web Portal/Nexus/Pure.Portals/`
   - `app.config`, `web.config`, or `App.config` files
   - `dPMDAO` or `dPMDAOBridge` files

### Prerequisites
- `workspace-init.md` must have been executed for this session
- If the task involves both repos, `SESSION_REST_REPO_PATH` must be set

---

## Gate Process — Execute in Exact Order

### STEP 1: Identify and Categorise Changed Files

Run on the **feature/integration branch** to list all files changed vs `main`:
```powershell
git diff --name-only origin/main...HEAD
```

Categorise each changed file into one of:

| Category | File patterns |
|----------|--------------|
| **Back Office Business** | `*/Components/*/Business/b*/*.vb`, `*/Components/*/b*/*.vb` |
| **Back Office UI** | `*/Components/*/Interface/i*/*.vb`, `*/Components/*/i*/*.vb` |
| **Portal Page** | `Web Portal/Nexus/Pure.Portals/**/*.aspx`, `*.aspx.vb`, `*.aspx.cs` |
| **Portal App_Code** | `Web Portal/Nexus/Pure.Portals/App_Code/**` |
| **Database SP** | `Databases/Pure/Procedures/*.sql`, `Databases/*/Procedures/*.sql` |
| **Database Migration** | `Databases/After Change/*.sql`, `Databases/*/After Change/*.sql` |
| **Navigator XM** | `Navigator XM Roadmaps/*.XML` |
| **Shared Library** | `Shared Files/**`, `gPMLibrary/**`, `g*Library/**` |
| **Configuration** | `*.config`, `web.config`, `app.config` |
| **Data Access** | `dPMDAO/**`, `dPMDAOBridge/**` |
| **Other** | Everything else |

Count files per category and report summary.

---

### STEP 2: Back Office Component Checks (b*/i* Projects)

Run for **every** changed `.vb` file in a `b*` or `i*` project.

| # | Check | What to verify | Severity |
|---|-------|---------------|----------|
| 1 | ACApp / ACClass constants exist | Every `b*` and `i*` class MUST declare `Private Const ACApp` and `Private Const ACClass` for logging. Values must match the project/class name | Medium |
| 2 | Error handling pattern | All public methods MUST have `Try/Catch` with `PMFunctions.LogMessageToFile` in the `Catch` block. Exception must NOT be silently swallowed — either return `PMEReturnCode.PMError` or re-throw | Critical |
| 3 | Return codes used correctly | Methods that return status MUST use `PMConstants.PMEReturnCode` enum (`PMSuccess`, `PMError`). Do not use raw integers or Boolean where return codes are expected | Medium |
| 4 | dPMDAO for data access | All database calls MUST go through `dPMDAO` using stored procedures. No inline SQL, no `SqlCommand` with raw SQL text, no `ExecuteNonQuery` with concatenated strings | Critical |
| 5 | Parameterised queries | All SP parameters MUST be passed via parameterised ADO.NET commands. Never concatenate user input into SQL strings | Critical |
| 6 | No hardcoded credentials | No passwords, connection strings, client secrets, or tokens in source code. Use `app.config`, environment variables, or registry | Critical |
| 7 | No hardcoded magic values | Business-critical numbers (tax rates, percentages, date offsets, currency codes) must be constants, config, or database-driven — not inline literals | Medium |
| 8 | COM object cleanup | If `Set obj = New ...` pattern is used, ensure cleanup in `Finally` block (`obj = Nothing`) | Low |
| 9 | Logging includes context | Log messages MUST include entity IDs (policy number, claim ID, party ID). Must NOT include PII (national insurance numbers, bank details, passwords) | Medium |
| 10 | Business/UI layer separation | `b*` projects must NOT reference WinForms UI types (`Form`, `MessageBox`, `TextBox`). UI logic belongs in `i*` projects only | Medium |
| 11 | Correct module-level structure | File follows: Imports → Class → Constants → Private fields → Constructor → Public methods → Private methods | Low |
| 12 | Option Strict awareness | Even though `Option Strict Off` is set, new code should use explicit types — avoid `Object` where specific types are available | Low |
| 13 | Decimal for financial values | All premium, payment, tax, and monetary amounts MUST use `Decimal` type — never `Double` or `Single` for financial calculations | Critical |
| 14 | Transaction management | Database operations that modify multiple rows/tables MUST use explicit transactions with commit/rollback | Medium |
| 15 | Audit trail entries | State changes (status updates, approvals, cancellations) MUST write to `event_log` table via the audit trail pattern | Medium |

---

### STEP 3: Portal Page Checks (ASP.NET Web Forms)

Run for **every** changed `.aspx`, `.aspx.vb`, or `.aspx.cs` file.

| # | Check | What to verify | Severity |
|---|-------|---------------|----------|
| 1 | REST API calls use correct handler | New Portal pages MUST call REST API via `SSP.PureInsuranceRestAPIHandler.ApiClient.CallApi()`. Do NOT use `NexusProvider` / `ProviderBase` / `SAMForInsurance` — these are legacy | Critical |
| 2 | No NexusProvider in new code | New code must NOT add references to `NexusProvider`, `ProviderManager`, or `SAMForInsurance`. These are legacy WCF/SAM providers | Critical |
| 3 | Null-check API responses | Every `CallApi()` result MUST be null-checked before accessing properties. The REST client returns null on failure — not exceptions | Critical |
| 4 | XSS prevention | User inputs displayed on page MUST be HTML-encoded. Use `Server.HtmlEncode()` or `HttpUtility.HtmlEncode()`. Do not write raw user input into `innerHTML`, `Literal.Text`, or `Label.Text` without encoding | Critical |
| 5 | Input validation server-side | All form inputs MUST be validated server-side — do not rely on client-side JavaScript validation alone. Check for null, empty, length, format | Medium |
| 6 | ViewState not storing sensitive data | Do not store passwords, tokens, connection strings, or PII in `ViewState`. ViewState is base64-encoded, not encrypted by default | Critical |
| 7 | Session state usage | Sensitive data in `Session` must be removed when no longer needed. Check for session timeout handling | Medium |
| 8 | Authentication check | Protected pages must verify user authentication. Check for `IsAuthenticated` or equivalent Keycloak token validation | Medium |
| 9 | Error handling in code-behind | Page events (`Page_Load`, button clicks, etc.) must have Try/Catch with appropriate error display to user — not raw exception details | Medium |
| 10 | No inline SQL in code-behind | Portal code-behind must NEVER have inline SQL or direct `SqlConnection` usage. All data access goes through REST API | Critical |
| 11 | GridView / Repeater binding | Data binding to grid controls must handle empty result sets gracefully — show "no records found" not an empty grid or error | Low |
| 12 | Base class inheritance | New Portal pages should inherit from appropriate base class (`BaseInstalment`, `BaseClaim`, `BaseClient`, etc.) where one exists for that area | Medium |
| 13 | Consistent UI patterns | New pages should follow existing layout patterns in the same area (same master page, same CSS classes, same button naming) | Low |
| 14 | JavaScript dependencies | If new JavaScript is added, verify it does not conflict with existing jQuery / Bootstrap versions loaded by the master page | Medium |

---

### STEP 4: Database Checks (Stored Procedures & Migrations)

Run for **every** changed `.sql` file.

| # | Check | What to verify | Severity |
|---|-------|---------------|----------|
| 1 | SP naming convention | Stored procedure names must follow existing convention in `Databases/Pure/Procedures/`. Typically `usp_[table]_[action]` or descriptive | Medium |
| 2 | Parameterised inputs | All SP parameters must be explicitly typed. No `varchar` without length. No `sql_variant` unless justified | Medium |
| 3 | No dynamic SQL with concatenation | If `EXEC(@sql)` or `sp_executesql` is used, parameters must be passed via `@params` — never concatenated into the SQL string | Critical |
| 4 | Transaction handling | Multi-statement SPs that modify data MUST use `BEGIN TRAN / COMMIT / ROLLBACK` with `TRY/CATCH` | Medium |
| 5 | Error handling | SPs must have `BEGIN TRY / BEGIN CATCH` with `RAISERROR` or `THROW` in the catch block. Do not silently swallow errors | Medium |
| 6 | Table/column naming | New tables use `snake_case`. New columns use `snake_case`. Must be consistent with existing schema | Medium |
| 7 | No `SELECT *` | All queries must explicitly list columns — never use `SELECT *` | Medium |
| 8 | Index consideration | If a new SP queries a table with a `WHERE` clause on non-indexed columns, flag as performance concern | Low |
| 9 | Migration script safety | `ALTER TABLE` scripts must check if column exists before adding (`IF NOT EXISTS`). `CREATE` scripts must check if object exists | Medium |
| 10 | Backward compatibility | SP signature changes must not break existing callers. If adding parameters, provide defaults. If removing, verify no callers remain | Critical |
| 11 | Data type consistency | New columns must use types consistent with existing patterns: `datetime` for dates (not `datetime2` unless justified), `decimal(18,2)` for monetary values, `int` for IDs | Medium |
| 12 | Audit columns | New tables that track business data should include `created_date`, `created_by`, `modified_date`, `modified_by` columns | Medium |
| 13 | No hardcoded environment values | Connection strings, server names, file paths must not appear in SP scripts | Critical |
| 14 | Permission grants | If a new SP is created, verify that appropriate `GRANT EXECUTE` is included or handled by the migration framework | Low |

---

### STEP 5: Navigator XM Checks (XML Roadmaps)

Run for **every** changed `.XML` file under `Navigator XM Roadmaps/`.

| # | Check | What to verify | Severity |
|---|-------|---------------|----------|
| 1 | Valid XML | File must be well-formed XML. Must reference correct DTD (`navigatorxm.dtd` or `navigatorxmv2.dtd`) | Critical |
| 2 | Component ProgID exists | Every `<STEP>` `Component` attribute must reference a valid, existing component ProgID (e.g., `iPMBFinanceTransactions.NavigatorV3`). Verify the referenced `i*` project exists | Critical |
| 3 | Element IDs unique | Every `ElementID` attribute must be unique within the roadmap file | Medium |
| 4 | Step flow logical | `OKAction` / `CancelAction` values must produce a valid navigation flow. No orphan steps that cannot be reached | Medium |
| 5 | TransactionType correct | `TransactionType` attribute must match a valid business transaction code (NB, REN, MTA, CAN, etc.) | Medium |
| 6 | Core attribute set | `Core` attribute should be `1` for mandatory steps, `0` for optional. Verify against requirements | Low |

---

### STEP 6: Shared Library & Configuration Checks

Run for changed files in `Shared Files/`, `g*Library/`, `dPMDAO/`, or config files.

| # | Check | What to verify | Severity |
|---|-------|---------------|----------|
| 1 | Backward compatibility | Changes to shared libraries (`gPMLibrary`, `gSIRLibrary`, etc.) must not break existing callers. Method signatures changes need default parameter values | Critical |
| 2 | No removed public members | Do not remove or rename public methods/properties/constants that other projects reference. Mark as `<Obsolete>` first | Critical |
| 3 | Config changes documented | Changes to `app.config` / `web.config` must be noted — new keys, changed connection strings, new sections. Flag if a key is removed | Medium |
| 4 | dPMDAO changes minimal | Changes to `dPMDAO` affect ALL data access across the entire system. Any change here requires extra scrutiny and must be reviewed | Critical |
| 5 | .NET Standard compatibility | Shared libraries targeting `.NET Standard 2.0` must not reference `.NET Framework 4.8`-only APIs | Medium |

---

### STEP 7: Cross-Cutting Quality Checks

Run for **all** changed files regardless of category.

| # | Check | What to verify | Severity |
|---|-------|---------------|----------|
| 1 | No hardcoded credentials | Scan all changed files for patterns: `Password=`, `pwd=`, `client_secret`, `apikey`, `token`, `connectionString` with literal values | Critical |
| 2 | No commented-out code | Remove commented-out code blocks before PR. Commented code is not a backup — that's what source control is for | Low |
| 3 | No `Debug.Print` or `Console.WriteLine` | Development debug statements must not reach PR. These are not production-appropriate | Medium |
| 4 | Consistent naming | New classes, methods, and variables follow conventions in `conventions.md`. VB.NET: PascalCase methods, camelCase locals. C#: same | Low |
| 5 | No PII in log statements | Log messages must not contain national insurance numbers, bank account numbers, full names alongside identifiers, or other PII | Critical |
| 6 | File encoding | VB.NET files must be ANSI or UTF-8 with BOM. SQL files must be consistent with existing files in same directory | Low |
| 7 | Build output path | New projects must output to `C:\Pure\Application\` — verify `OutputPath` in `.vbproj` or `.csproj` | Medium |
| 8 | No new NuGet packages without justification | If a new NuGet package is added, flag it — verify it is necessary and compatible with `.NET Framework 4.8` / `.NET Standard 2.0` | Medium |

---

### STEP 8: Requirement Verification

For **every** acceptance criterion (AC) or bug repro step in the work item:

**For Features / Stories:**

| # | Check | What to verify | Severity |
|---|-------|---------------|----------|
| 1 | AC is implemented | For each AC, identify at least one changed file that implements it. If an AC has no corresponding code change, flag as missing | Critical |
| 2 | AC is complete | The implementation must cover the full AC — not just the happy path. Check error paths, edge cases, boundary values | Medium |
| 3 | Business rules match | Compare the code logic against business rules in the AC description and knowledge base. Flag discrepancies | Critical |
| 4 | Data flow is end-to-end | If the AC involves user action → API call → database change, verify all layers are present (Portal page → REST API endpoint → SP) | Critical |
| 5 | Lock mechanism | If the AC involves editing a record, verify optimistic/pessimistic locking is implemented to prevent concurrent edit conflicts | Medium |
| 6 | Audit trail | If the AC involves a state change (approve, cancel, modify), verify an audit trail entry is created in `event_log` | Medium |

**For Bugs / Defects:**

| # | Check | What to verify | Severity |
|---|-------|---------------|----------|
| 1 | Root cause addressed | The fix must address the actual root cause — not just suppress the symptom | Critical |
| 2 | Correct layer | The fix must be in the correct architectural layer (don't fix a business logic bug in the UI layer) | Medium |
| 3 | All repro scenarios | The fix must handle all repro steps described in the bug, including edge cases mentioned in comments | Critical |
| 4 | Minimal and focused | The fix should change only what is necessary. Unrelated refactoring should be in a separate PR | Medium |
| 5 | No regression | The fix must not break existing functionality. Verify shared methods still behave correctly for other callers | Critical |

---

### STEP 9: Produce Pre-PR Code Inspection Report

> **MANDATORY OUTPUT FORMAT**: All suggestions MUST be presented in tabular format.
> Never use free-text paragraphs or bullet lists for issues. Every issue gets a row
> in the appropriate table with all columns populated.

Format the output as:

```
## 🔍 Pre-PR Code Inspection Report (Back Office / Portal / DB)
**Task Branch**: task/ADO-[id]-[task-name]
**Inspection Date**: [date]
**Files Changed**: [count] | **Categories**: [list of categories with counts]

### Summary
| Metric | Count |
|--------|-------|
| Total issues found | [n] |
| Critical | [n] |
| Medium | [n] |
| Low | [n] |

### Files Changed Summary
| Category | Count | Files |
|----------|-------|-------|
| Back Office Business (b*) | [n] | [file list] |
| Back Office UI (i*) | [n] | [file list] |
| Portal Page (.aspx) | [n] | [file list] |
| Portal Code-Behind (.aspx.vb) | [n] | [file list] |
| Database SP (spu_/spe_) | [n] | [file list] |
| Navigator XM Roadmap (.xml) | [n] | [file list] |
| Shared Library (g*/Shared) | [n] | [file list] |
| Configuration | [n] | [file list] |

### Issues Found
| # | What is Missed | Where (File : Line) | Severity | Potential Fix |
|---|---------------|---------------------|----------|---------------|
| 1 | Error handling missing in public method | `bSIRContact.vb:245` | Critical | Add Try-Catch block with `SiriusCommand.HandleError()` pattern |
| 2 | Inline SQL concatenation | `dSIRParty.vb:130` | Critical | Use `SiriusCommand.AddParameter()` for parameterised query |
| 3 | NexusProvider used in new code | `QuotePage.aspx.vb:88` | Critical | Use `SSP.PureInsuranceRestAPIHandler` via REST API instead |
| 4 | Missing null check on API response | `PolicyView.aspx.vb:156` | Medium | Add `If response IsNot Nothing AndAlso response.Success Then` guard |
| 5 | Hardcoded string instead of shared constant | `bSIRRenewal.vb:310` | Medium | Use `bSIRConst.STATUS_ACTIVE` from shared library |
| 6 | SP missing NOCOUNT/transaction | `spu_SIR_Contact_add.sql:1` | Medium | Add `SET NOCOUNT ON` and wrap in `BEGIN TRY / BEGIN TRAN` |

### Requirement Coverage
| AC# / Repro Step | Description | Implemented? | Where (File / Method) | Verdict |
|------------------|-------------|-------------|----------------------|---------|
| AC1 | [desc] | ✅/❌ | `[file:method]` | PASS/FAIL/PARTIAL |
| AC2 | [desc] | ✅/❌ | `[file:method]` | PASS/FAIL/PARTIAL |
| BUG Repro Step 1 | [desc] | ✅/❌ | `[file:method]` | PASS/FAIL |

### Missing Implementation
| # | What is Missed | Where (Expected File / Location) | Potential Fix |
|---|---------------|----------------------------------|---------------|
| 1 | [AC or requirement not implemented] | `[expected component or file]` | [what to implement and where] |

### Gate Verdict: **[PASS ✅ / PASS WITH SUGGESTIONS ⚠️ / BLOCKED ❌]**
```

---

### STEP 10: Gate Verdict Rules

- **PASS ✅** — No issues found. Proceed to create the PR.
- **PASS WITH SUGGESTIONS ⚠️** — Only Low/Medium severity issues. Proceed to create the PR but include suggestions in the PR description for reviewer attention.
- **BLOCKED ❌** — Critical severity issues found. Do NOT create the PR. Report issues and wait for developer instructions.

**Blocking issues (always BLOCKED ❌):**
- Missing error handling in public methods
- Inline SQL or SQL concatenation
- Hardcoded credentials or PII in logs
- NexusProvider usage in new code
- Missing null checks on API responses in Portal
- XSS vulnerabilities (unencoded user input)
- Acceptance criteria with no corresponding implementation
- Root cause not addressed (for bugs)

---

### STEP 11: PR Description Enrichment

When the gate passes (✅ or ⚠️), the agent MUST include the following in the PR description:

```
## Changes Summary
- **Back Office**: [list of b*/i* components changed and why]
- **Portal**: [list of .aspx pages changed and why]
- **Database**: [list of SPs changed/added and why]
- **Navigator XM**: [roadmap changes and why]
- **Config**: [any config changes]

## Acceptance Criteria Coverage
| AC# | Status |
|-----|--------|
| AC1 | ✅ Implemented |
| AC2 | ✅ Implemented |

## Code Inspection
- Issues found: [count]
- Critical: [count] | Medium: [count] | Low: [count]
- Suggestions for reviewer: [list or "None"]

## Linked Work Item
AB#[work-item-id]
```

---

## Integration with API Inspection

When a task involves changes in **both** repositories:

1. Run **this gate** for all `PureInsurance` repo changes
2. Run **API gate** (from `API_inspection.md`) for all `PureInsurance.REST` repo changes
3. Both gates must pass before any PR is created
4. If the task has PRs in both repos, include cross-references in both PR descriptions

### Cross-Repo Consistency Checks

When both repos are changed, additionally verify:

| # | Check | What to verify | Severity |
|---|-------|---------------|----------|
| 1 | Portal calls match API contract | If a Portal page calls a REST API endpoint that was also changed, verify the request parameters and response handling match the new API contract | Critical |
| 2 | SP matches repository | If a new SP is added in `PureInsurance`, and a new Repository method is added in `PureInsurance.REST` that calls it, verify the SP name and parameter list match | Critical |
| 3 | Error handling aligned | If the API returns new error codes or response structures, verify the Portal handles them correctly (null checks, error display) | Medium |
| 4 | Feature flag / config sync | If the feature requires configuration in both repos, verify both are present | Medium |

---

## Integration with AIDLC Workflow

> **For AI agents following the AIDLC execution protocol (Mode A or Mode B):**
>
> When you reach the "Raise a PR" step (Mode A step 21 / Mode B step 18),
> check whether the task branch contains Back Office, Portal, Database, or
> Navigator XM changes. If it does, run this gate **before** creating the PR.
>
> If the task also has REST API changes, run the API gate from
> `API_inspection.md` as well. Both gates must pass.
>
> If either gate verdict is **BLOCKED ❌**, do NOT create the PR — report
> the issues and wait for developer instructions.

---

## Quick Reference

| Prompt | Action |
|--------|--------|
| *(automatic)* PR with Back Office/Portal/DB changes | This gate runs automatically |
| `BO Inspect` | Run this gate manually on current branch |
| `Portal Inspect` | Run STEP 3 only |
| `DB Inspect` | Run STEP 4 only |
