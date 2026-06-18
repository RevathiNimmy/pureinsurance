# QA Verification Rules

## SESSION START — REST API WORKSPACE DETECTION (Automatic)

> **This runs automatically at the start of every session** via
> `.ai/memory/workspace-init.md`. The workspace initialization file handles
> sibling repo detection, user confirmation, and branch alignment for ALL
> Pure Insurance workspaces.
>
> **You MUST read and execute `.ai/memory/workspace-init.md` BEFORE using any
> section in this document.** The session state it produces (especially
> `SESSION_SIBLING_PATH` and `SESSION_SIBLING_INCLUDED`) is required by the
> Pre-PR API Gate, Phase 2 API Analysis, and QA Full Review.

### What workspace-init.md Does (summary)
1. **Fingerprints** the current workspace using git remote URL, directory structure
   markers, or solution file patterns — does NOT rely on the folder name
2. Scans sibling directories for the other Pure Insurance repo using the same
   fingerprint approach
3. If the sibling cannot be found automatically, **asks the user** for the path
4. **Asks the user for confirmation** before including the sibling in scope
5. Verifies branch alignment between both repos
6. Stores session state (`SESSION_REST_REPO_PATH`, `SESSION_BACKOFFICE_REPO_PATH`,
   `SESSION_SIBLING_INCLUDED`, etc.) for all downstream operations

### If workspace-init.md Was NOT Run
If you reach any API-related operation (Pre-PR Gate, Phase 2 Step 6, QA Full Review)
and workspace initialization has not been performed:
1. **STOP** — do not proceed with the API operation
2. Run `.ai/memory/workspace-init.md` first
3. Resume the operation after session state is established

---

## Trigger Keywords
- **"QA Analyse"** or **"QA Analyze"** → Run PHASE 1 (Comprehensive Analysis)
- **"QA Verify"** or **"Verify Development"** → Run PHASE 2 (Development Verification)
- **"QA Full Review"** → Run PHASE 1 + PHASE 2 together

## Automatic Triggers (No keyword needed)
- **PR creation from feature branch to `main`** with API changes → Run PRE-PR API GATE automatically (see below)

---

## PRE-PR API GATE — AUTOMATIC BEFORE FEATURE → MAIN PR

> **This gate runs automatically when merging a completed feature into `main`.**
> Whenever an AI agent is about to raise a PR from a **feature/integration branch**
> (`feature/ADO-[epic-id]-[feature-name]`) **to `main`** (Mode A step 24 or
> Mode B step 20) and the feature branch contains changes to ANY file in the
> `PureInsurance.REST` repository or touches any `Controller.cs`,
> `QueryHandler.cs`, `CommandHandler.cs`, `Repository.cs`, `Validator.cs`,
> `Query.cs`, `Command.cs`, `QueryResponse.cs`, or `CommandResponse.cs` file,
> the agent MUST execute this gate **before** creating the PR to `main`.
>
> **This gate does NOT run for task → feature PRs.** Those are lightweight merges
> within the feature; the full inspection happens once before the feature reaches `main`.
>
> **Multi-repo awareness**: If the REST repo was confirmed at session start,
> the gate MUST scan changed files in BOTH the `PureInsurance` workspace AND
> the `PureInsurance.REST` sibling repo. Use the resolved REST path from the
> SESSION START detection — never hardcode it.

### When to Trigger
The agent MUST run this gate when **ALL** of the following are true:
1. The agent is creating a PR from `feature/ADO-*` → `main`
2. **Any** of the following file patterns are detected in the feature branch diff vs `main`:
   - Files under a `Controllers/` directory
   - `*Query.cs`, `*Command.cs`, `*QueryHandler.cs`, `*CommandHandler.cs`
   - `*Validator.cs`, `*Repository.cs`, `*IRepository.cs`
   - `*QueryResponse.cs`, `*CommandResponse.cs`
   - `StoredProcedures.cs`
   - Any file in a project whose name contains `.REST` or `.Api`

### Gate Process — Execute in Exact Order

#### Gate Step 1: Identify Changed Files
Run on the **feature/integration branch** to list all files changed vs `main`:
```powershell
git diff --name-only origin/main...HEAD
```

**Multi-repo scan**: If `SESSION_SIBLING_INCLUDED = true`, ALSO run:
```powershell
git -C "{SESSION_REST_REPO_PATH}" diff --name-only origin/main...HEAD
```
Use `SESSION_REST_REPO_PATH` and `SESSION_BACKOFFICE_REPO_PATH` from session state —
never hardcode paths or assume folder names.

Combine the results from both repos. Prefix each file with its repo type for clarity
(e.g., `[BackOffice] path/to/file.cs`, `[REST] path/to/file.cs`).

Categorise each file as: Controller / Handler / Query-Command Contract / Response Contract / Validator / Repository / SP Reference / Other.

#### Gate Step 2: Scan for API Surface Changes
For every changed file:
- **Controllers**: Detect new or modified `[HttpGet]`, `[HttpPost]`, `[HttpPut]`, `[HttpDelete]` action methods
- **Query/Command contracts**: Detect added, removed, or renamed properties
- **Response contracts**: Detect added, removed, or renamed properties
- **Validators**: Detect new or changed validation rules
- **Handlers**: Detect logic changes in `Handle` method
- **Repository/IRepository**: Detect new or modified data access methods
- **StoredProcedures.cs**: Detect new SP constant entries

Classify each detected change as **NEW** (endpoint/file did not exist before) or
**MODIFIED** (endpoint/file existed and was changed).

#### Gate Step 2b: New API Creation Compliance (MANDATORY for NEW endpoints)

> **If Gate Step 2 identified ANY new API endpoint**, the agent MUST read and
> verify against `.ai/memory/docs/new-api-creation-guide.md` before proceeding.
> This guide defines the canonical layered architecture, file structure, naming,
> and registration rules for every new endpoint.

**When to run**: If ANY of the following are true:
- A new `[HttpGet]`, `[HttpPost]`, `[HttpPut]`, or `[HttpDelete]` action was added to a Controller
- A new `*Command.cs` or `*Query.cs` file was created
- A new `*CommandHandler.cs` or `*QueryHandler.cs` file was created

**Checklist — verify ALL of the following for every NEW endpoint:**

| # | Check | What to verify (per new-api-creation-guide.md) | Severity |
|---|-------|------------------------------------------------|----------|
| 1 | Command vs Query classification | Commands (POST/PUT/DELETE) are in `Commands/{Feature}/` folder. Queries (GET) are in `Queries/{Feature}/` folder. Not mixed | Critical |
| 2 | All required files exist | For a Command: `{Name}Command.cs`, `{Name}CommandBase.cs`, `{Name}CommandResponse.cs`, `{Name}CommandBaseResponse.cs`, `{Name}CommandValidator.cs`, `{Name}CommandHandler.cs`. For a Query: equivalent `Query` files. Flag any missing file | Critical |
| 3 | Base class inheritance | Command/Query class inherits from `{Name}Base` and implements `IRequest<{Name}Response>`. Base class inherits from `REST.Common.Domain.Models.BaseRequestType` | Critical |
| 4 | Response class structure | Response class inherits from `{Name}BaseResponse`. BaseResponse contains actual properties. Response class is the MediatR return type | Medium |
| 5 | Handler follows correct order | Handler `Handle` method follows: (1) Validate → (2) CheckIdentityAndAuthority → (3) Delegate to Service → (4) Return. No business logic in the handler itself | Critical |
| 6 | Service layer exists | Service interface in `Contracts/I{Name}Service.cs`. Implementation in `Services/{Name}Service.cs`. Business logic lives here, NOT in the handler | Critical |
| 7 | DI registration | New service is registered in `DependencyInjection.cs` via `services.AddScoped<I{Name}Service, {Name}Service>()`. Handlers and validators are auto-registered (do NOT manually register them) | Critical |
| 8 | Controller endpoint pattern | Controller action sets `request.LoginUserName` and `request.Route` before `Mediator.Send()`. Uses `[FromBody]` for POST/PUT/DELETE, `[FromQuery]` for GET | Critical |
| 9 | Controller attributes | `[ProducesResponseType(typeof({Response}), 200)]`, `[Produces("application/json", "application/xml")]`, `[ApiConventionMethod(...)]` all present | Medium |
| 10 | Ocelot gateway routes | Route entry exists in BOTH `ocelot.json` (https/localhost) AND `ocelot.installer.json` (http/placeholders). Correct port for the microservice. Correct HTTP method. Correct path template | Critical |
| 11 | Port mapping correct | Microservice port matches: Claims=7087, Policy=7056, Account=7035, Core=7121, Party=7103, Security=7107, Messaging=7123 | Critical |
| 12 | Route casing | Route path uses lowercase camelCase (e.g., `/claims/paymentTaxGroups`). No PascalCase or UPPERCASE in routes | Medium |
| 13 | Unit test files exist | Test folder `{Domain}.Tests/{Feature}/` exists with: `{Name}HandlerTests.cs`, `{Name}ValidatorTests.cs`, `{Name}ServiceTests.cs` | Medium |
| 14 | Using statement added | Controller file has `using` statement for the new Command/Query namespace | Low |
| 15 | `[JsonIgnore]` on internal props | Properties not intended for API consumers (internal flags, derived values) have `[JsonIgnore]` attribute in the Base class | Medium |

**If any Critical check fails**: The gate verdict MUST be **BLOCKED ❌**.
Common failures include:
- Missing Ocelot route (new endpoint returns 404 from gateway)
- Missing DI registration (runtime DI exception)
- Business logic in handler instead of service
- Missing `request.LoginUserName` / `request.Route` in controller

#### Gate Step 3: Run API Quality Checklist
For **every** new or modified API endpoint found in Gate Step 2, verify ALL of the following:

| # | Check | What to verify | Auto-fix? |
|---|-------|---------------|-----------|
| 1 | HTTP verb correct | GET = read/query, POST = create/action, PUT = update, DELETE = remove. State-checking that passes IDs should be POST not GET | Suggest |
| 2 | Route naming convention | Lowercase, hyphenated, consistent with existing routes in same controller | Suggest |
| 3 | Sensitive IDs not in query string | IDs, transaction keys, claim keys should be in `[FromBody]` not `[FromQuery]` for POST/PUT | Suggest |
| 4 | Authority check present | `CheckIdentityAndAuthority` called with correct SAM authority string | Suggest |
| 5 | Validator exists and is registered | `FluentValidation` validator class exists; all mandatory fields validated; correct error codes | Suggest + create stub |
| 6 | Error codes correct | `MandatoryInputMissing` for absent fields, `InvalidValue` for wrong values — not mixed | Suggest |
| 7 | Response types declared | `[ProducesResponseType(200)]`, `[ProducesResponseType(400)]`, `[ProducesResponseType(401)]`, `[ProducesResponseType(403)]` all declared | Auto-fix |
| 8 | XML documentation | `<summary>` and `<param>` and `<returns>` present on controller action | Auto-fix |
| 9 | Null guards in handler | Handler checks for null repository results before accessing properties | Suggest |
| 10 | Empty result vs error | For optional lookups (e.g., no schemes found), return empty result not 400 | Suggest |
| 11 | New fields have validator rules | Any new mandatory input field has a corresponding FluentValidation rule | Suggest + create stub |
| 12 | Backward compatibility | New fields on existing contracts have sensible defaults (0, null, false) so existing callers are not broken | Suggest |
| 13 | Unit tests cover new logic | At least one unit test per new handler; validator tests for each new validation rule | Suggest + create stub |
| 14 | SP registered in StoredProcedures.cs | New SP name added to the constants class | Auto-fix |
| 15 | No hardcoded values | Magic strings, numbers, or dates must be constants or config | Suggest |
| 16 | Null/empty input validation | All string inputs checked for null/empty; numeric IDs checked for > 0 | Suggest |
| 17 | Exception handling | Try-catch in handler where needed; no swallowed exceptions; meaningful error messages | Suggest |
| 18 | Consistent naming | Method names, route names, and class names follow existing patterns in the same module | Suggest |

#### Gate Step 4: Produce Suggestions
For every issue found, the agent MUST:
1. **Report** the issue in the standardised tabular format below
2. **Apply auto-fixable issues** directly to the task branch (items marked `Auto-fix` in Gate Step 3)
3. **List all issues** — both auto-fixed and suggestions — in the report tables

> **MANDATORY OUTPUT FORMAT**: All suggestions MUST be presented in tabular format.
> Never use free-text paragraphs or bullet lists for issues. Every issue gets a row
> in the appropriate table with all columns populated.

Format the output as:
```
## 🔍 Pre-PR API Inspection Report
**Task Branch**: task/ADO-[id]-[task-name]
**Inspection Date**: [date]
**API Files Changed**: [count]
**New Endpoints Created**: [count] (checked against new-api-creation-guide.md)

### Summary
| Metric | Count |
|--------|-------|
| Total issues found | [n] |
| Critical | [n] |
| Medium | [n] |
| Low | [n] |
| Auto-fixed | [n] |
| Require developer review | [n] |

### New API Creation Compliance (if any new endpoints)
| # | Endpoint | What is Missed | Where (File / Location) | Potential Fix |
|---|----------|---------------|-------------------------|---------------|
| 1 | [POST /domain/route] | Ocelot route not registered | `ApiGateway/.../ocelot.installer.json` | Add route entry: `{ "DownstreamPathTemplate": "/domain/route", ... }` |
| 2 | [POST /domain/route] | DI registration missing for service | `{Domain}.Application/DependencyInjection.cs` | Add `services.AddScoped<I{Name}Service, {Name}Service>();` |
| 3 | [GET /domain/route] | Command used instead of Query | `{Domain}.Application/.../Commands/{Name}/` | Move to `Queries/{Name}/` and rename `*Command*` → `*Query*` |

### Auto-Fixed Issues (applied to branch)
| # | What was Missed | Where (File : Line) | Fix Applied |
|---|----------------|---------------------|-------------|
| 1 | `[ProducesResponseType(400)]` not declared | `Controllers/{Name}Controller.cs:142` | Added missing response type attributes |
| 2 | XML `<summary>` documentation missing | `Controllers/{Name}Controller.cs:138` | Added XML doc block |

### Suggested Changes (require developer review)
| # | What is Missed | Where (File : Line) | Severity | Potential Fix |
|---|---------------|---------------------|----------|---------------|
| 1 | Authority check not called in handler | `Commands/{Name}/{Name}Handler.cs:45` | Critical | Add `await _identityService.CheckIdentityAndAuthority(...)` after validation |
| 2 | Sensitive key exposed in query string | `Controllers/{Name}Controller.cs:98` | Critical | Change `[FromQuery]` to `[FromBody]` and switch to POST |
| 3 | Null guard missing for repository result | `Commands/{Name}/{Name}Handler.cs:52` | Medium | Add `if (result == null) throw new NotFoundException(...)` |
| 4 | Hardcoded magic string | `Services/{Name}Service.cs:88` | Medium | Replace with constant from shared constants class |

### API Contract Summary
| API | Endpoint | Type | Issues | Status |
|-----|----------|------|--------|--------|
| [name] | [route] | NEW/MODIFIED | [n] Critical, [n] Medium, [n] Low | ✅ Clean / ⚠️ Has suggestions |

### Gate Verdict: **[PASS ✅ / PASS WITH SUGGESTIONS ⚠️ / BLOCKED ❌]**
```

#### Gate Step 5: Gate Verdict Rules
- **PASS ✅** — No issues found. Proceed to create the PR.
- **PASS WITH SUGGESTIONS ⚠️** — Only Low/Medium severity suggestions remain (auto-fixes already applied). Proceed to create the PR but include the suggestions in the PR description.
- **BLOCKED ❌** — Critical severity issues found. Do NOT create the PR. Report all critical issues to the developer and wait for instructions.

**Blocking conditions (always BLOCKED ❌):**
- Missing authority check, sensitive IDs in query string, no validator for mandatory fields
- Breaking backward compatibility without justification
- **New API creation guide violations**: missing required files (Command/Query, Base, Response, Validator, Handler, Service), missing Ocelot routes, missing DI registration, business logic in handler, missing `request.LoginUserName`/`request.Route` in controller

#### Gate Step 6: PR Description Enrichment
When the gate passes (✅ or ⚠️), the agent MUST include the following in the PR description:
- List of API endpoints added or modified
- Summary of auto-fixes applied
- Any remaining suggestions for reviewer attention
- Link to the work item

---

## PHASE 1 — COMPREHENSIVE ANALYSIS

### Process — Execute in Exact Order

#### Step 1: Retrieve Work Item
- Use `wit_get_work_item` with `expand=all` — get title, description, ACs, repro steps, state, severity, priority, all relations

#### Step 2: Retrieve Linked Items
- Parent, children (`wit_get_work_items_batch_by_ids`), "Tested By" test cases, related bugs, PR/commit artifact links

#### Step 3: Get Comments
- Use `wit_list_work_item_comments` for additional context

#### Step 4: Knowledge Base Consultation
Follow priority order and code search process in `knowledge-base-integration.md`.

#### Step 5: Code Search & Analysis
Search and read related code files. For Bugs: trace code path from repro steps to identify root cause.

#### Step 6: Produce Analysis Report

**For Stories/PBIs:**
```
## QA Analysis Report: [Title]
**Work Item**: #[ID] | **Type**: [Story/PBI] | **State**: [state]

### 1. Executive Summary
### 2. Acceptance Criteria Review
| AC# | Description | Clear? | Testable? | Complete? | Gaps |
### 3. Functional Flow
### 4. Business Rules (from Knowledge Base + Code)
### 5. Code Analysis Summary (Files, Methods, Validations, DB Tables)
### 6. Impacted Areas
### 7. Gaps & Risks
### 8. Linked Items Summary (Children, Test Cases, Bugs)
### 9. PR/Commit Links (for Phase 2)
### Knowledge Base Sources Referenced
```

**For Bugs/Defects:**
```
## QA Defect Analysis Report: [Title]
**Work Item**: #[ID] | **Severity**: [severity] | **State**: [state]

### 1. Defect Summary (Module, Build, Application Area, What's Broken)
### 2. Issues Identified (Expected vs Actual for each issue)
### 3. Root Cause Analysis (Code File, Method, Line, Issue, Why)
### 4. Code Path Trace (API → Business Layer → SP → Table)
### 5. Impact Assessment (Business, Data, Premium, Reinsurance, Regulatory, Scope)
### 6. Suggested Fix
### 7. Regression Risk
### 8. Related Items
### 9. PR/Commit Links (for Phase 2)
### Knowledge Base Sources Referenced
```

---

## PHASE 2 — DEVELOPMENT VERIFICATION

### Pre-requisite
Phase 1 should already be completed. If not, run lightweight Phase 1 first (work item + ACs only).

### Process — Execute in Exact Order

#### Step 1: Get Artifact Links
Extract ALL PR links (`vstfs:///Git/PullRequestId/`) and commit links from the work item AND all child tasks.

**CRITICAL — Always check child tasks for additional PRs:**
- The PBI/Story artifact links may only show a subset of PRs
- Child Dev tasks frequently have PRs linked directly to them that are not on the parent
- ALWAYS call `wit_get_work_item` with `expand=relations` on EACH child task to retrieve all linked PRs
- A Dev task can have PRs across MULTIPLE repositories (e.g., PureInsurance repo AND PureInsurance.REST repo)
- Collect ALL unique PR IDs across the PBI and all child tasks before proceeding
- Fetch ALL PRs in parallel before reading any changed files

#### Step 2: Retrieve PRs
Use `repo_get_pull_request_by_id` for details + `repo_list_pull_request_threads` for review comments.

#### Step 3: Retrieve Commits
Use `pipelines_get_build_changes` or `repo_search_commits` to find related commits.

#### Step 4: Read Changed Files
Use `fsRead` on changed files. Focus on: VB.NET/C# business logic, SQL stored procedures, REST API layer, ASPX/UI, configuration.

#### Step 5: Verify Against Requirements
**For Stories**: For each AC — is there code? Is it correct? Edge cases handled? Error handling? Audit trail? Lock mechanism?
**For Bugs**: Does fix address root cause? Correct layer? All repro scenarios? Edge cases? Follows patterns? Minimal and focused?

#### Step 6: API Analysis — MANDATORY when REST repo changes are present

If any PR touches `PureInsurance.REST` repository or any `Controller.cs`, `QueryHandler.cs`, `CommandHandler.cs`, or `Repository.cs` file — execute ALL steps below.

**Step 6a: Identify all API changes across ALL PRs**
For every changed file in every PR:
- Scan `Controllers/*.cs` for new `[HttpGet]`, `[HttpPost]`, `[HttpPut]`, `[HttpDelete]` methods
- Scan `Queries/*/Query.cs` and `Commands/*/Command.cs` for new or modified input contracts
- Scan `Queries/*/QueryResponse.cs` and `Commands/*/CommandResponse.cs` for new or modified output contracts
- Scan `QueryHandler.cs` and `CommandHandler.cs` for handler logic changes
- Scan `Repository.cs` and `IRepository.cs` for new or modified data access methods
- Scan `Validator.cs` for new or modified validation rules
- Scan `StoredProcedures.cs` for new SP references

**Step 6b: For each NEW API endpoint — document the full contract:**

> **MANDATORY**: For every new endpoint, also verify compliance against
> `.ai/memory/docs/new-api-creation-guide.md`. Check that all required files
> exist (Command/Query, Base, Response, BaseResponse, Validator, Handler,
> Service interface, Service implementation), DI registration is present,
> Ocelot routes are registered in both files, and the handler follows the
> correct Validate → Authorize → Delegate → Return order.

```
### NEW API: [HTTP Method] [Route]
- Controller: [ControllerName]
- Handler: [QueryHandler/CommandHandler class]
- Authority Check: [SAM authority string + entity type]
- Creation Guide Compliance: [✅ All files present / ❌ Missing: list]
- Ocelot Registration: [✅ Both files / ❌ Missing: which file]
- DI Registration: [✅ Registered / ❌ Missing]
- Input Contract:
  | Parameter | Type | Mandatory | Validation Rule | Description |
  |-----------|------|-----------|-----------------|-------------|
  | [param]   | [T]  | ✅/❌     | [rule]          | [what it does] |
- Output Contract:
  | Field | Type | Description |
  |-------|------|-------------|
  | [field] | [T] | [meaning]  |
- HTTP Responses:
  | Code | When |
  |------|------|
  | 200  | Success |
  | 400  | Validation failure |
  | 401  | Unauthenticated |
  | 403  | Unauthorized |
- Repository/SP flow:
  [Method] → [SP name] → [Tables queried/updated]
- Unit Tests: [Test file name — what is covered]
```

**Step 6c: For each MODIFIED API endpoint — document what changed:**
```
### MODIFIED API: [HTTP Method] [Route]
- What changed: [new fields / changed behaviour / removed validation]
- New input fields:
  | Field | Type | Description | Default | Impact on existing callers |
  |-------|------|-------------|---------|---------------------------|
- Behaviour changes:
  | Scenario | Before | After |
  |----------|--------|-------|
- Backward compatibility: [breaking / non-breaking — explain]
- Callers impacted: [Portal files / other services that call this API]
```

**Step 6d: API Quality Checks — verify ALL of the following for every new/changed API:**

| Check | What to verify |
|-------|---------------|
| HTTP verb correct | GET = read/query, POST = create/action, PUT = update, DELETE = remove. State-checking that passes IDs should be POST not GET |
| Route follows naming convention | Lowercase, hyphenated, consistent with existing routes in same controller |
| Sensitive IDs not in query string | IDs, transaction keys, claim keys should be in `[FromBody]` not `[FromQuery]` for POST/PUT |
| Authority check present | `CheckIdentityAndAuthority` called with correct SAM authority string |
| Validator exists and is registered | `FluentValidation` validator class exists; all mandatory fields validated; correct error codes |
| Error codes correct | `MandatoryInputMissing` for absent fields, `InvalidValue` for wrong values — not mixed |
| Response types declared | `[ProducesResponseType(200)]`, `[ProducesResponseType(400)]`, `[ProducesResponseType(401)]`, `[ProducesResponseType(403)]` all declared |
| XML documentation | `<summary>` and `<param>` and `<returns>` present on controller action |
| Null guards in handler | Handler checks for null repository results before accessing properties |
| Empty result vs error | For optional lookups (e.g., no schemes found), return empty result not 400 |
| New fields have validator rules | Any new mandatory input field has a corresponding FluentValidation rule |
| Backward compatibility | New fields on existing contracts have sensible defaults (0, null, false) so existing callers are not broken |
| Unit tests cover new logic | At least one unit test per new handler; validator tests for each new validation rule |
| SP registered in StoredProcedures.cs | New SP name added to the constants class |

**Step 6e: Produce API Analysis Section in Verification Report:**
```
## API Analysis

### New APIs Added
[Document each new endpoint using the contract template from Step 6b]

### Existing APIs Modified
[Document each changed endpoint using the template from Step 6c]

### API Issues Found
| # | Endpoint | Issue | Severity | Suggested Fix |
|---|----------|-------|----------|---------------|
| 1 | [route]  | [desc]| Critical/Medium/Low | [fix] |

### API Changes Summary Table
| API | Endpoint | Type | Change Description | PR |
|-----|----------|------|-------------------|----|
| [name] | [route] | NEW/MODIFIED/REMOVED | [what changed] | #[PR] |
```

#### Step 7: Check Code Quality
Review for: hardcoded values, null/empty validation, date calculations, premium/treaty rounding, lock mechanism, DB transactions, exception handling, audit trail, SQL injection, thread safety.
Reference checklist in `code-review.md`.

#### Step 8: Check Regression Risk
Shared methods affected? Existing validations preserved? Portal ↔ Back Office sync? Backward-compatible SP changes? Premium/treaty/policy impact? **Existing API callers broken by new required fields?**

#### Step 9: Produce Verification Report

**For Stories:**
```
## QA Development Verification Report: [Title]
**Work Item**: #[ID] | **Verification Date**: [date]

### 1. PRs Reviewed
| PR# | Title | Status | Branch | Reviewers | Merged? |
|-----|-------|--------|--------|-----------|---------|
| [n] | [title] | [status] | [branch] | [names] | ✅/❌ |

### 2. Commits Summary
| Commit | Author | Message | Files Changed |
|--------|--------|---------|---------------|
| [sha] | [name] | [msg] | [n] |

### 3. Files Changed Summary
| Category | Path | Change Type | Purpose |
|----------|------|-------------|----------|
| [cat] | [path] | Added/Modified/Deleted | [why] |

### 4. AC Coverage Matrix
| AC# | Description | Implemented? | Where (File / Method) | Correct? | Edge Cases? | Verdict |
|-----|-------------|-------------|----------------------|----------|-------------|----------|
| 1 | [desc] | ✅/❌ | `[file:method]` | ✅/❌ | ✅/❌/N/A | PASS/FAIL |

### 5. API Analysis
#### 5a. New APIs Added
| Endpoint | Method | Handler | Service | SP |
|----------|--------|---------|---------|----|
| [route] | POST/GET | [handler] | [service] | [sp name] |

#### 5b. Existing APIs Modified
| Endpoint | What Changed | Where (File : Line) | Impact |
|----------|-------------|---------------------|--------|
| [route] | [change desc] | `[file:line]` | Breaking/Non-breaking |

#### 5c. API Issues Found
| # | What is Missed | Where (File : Line) | Severity | Potential Fix |
|---|---------------|---------------------|----------|---------------|
| 1 | [desc] | `[file:line]` | Critical/Medium/Low | [fix] |

#### 5d. API Changes Summary Table
| API | Endpoint | Type | Issues | Status |
|-----|----------|------|--------|--------|
| [name] | [route] | NEW/MODIFIED | [count by severity] | ✅/⚠️ |

### 6. Code Quality Issues
| # | What is Missed | Where (File : Line) | Severity | Potential Fix |
|---|---------------|---------------------|----------|---------------|
| 1 | [desc] | `[file:line]` | Critical/Medium/Low | [fix] |

### 7. Missing Implementation
| # | What is Missed | Where (Expected File / Location) | Potential Fix |
|---|---------------|----------------------------------|---------------|
| 1 | [missing feature/AC] | `[expected file or module]` | [what to implement] |

### 8. Regression Concerns
| # | Concern | Where (File / Area) | Risk Level | Mitigation |
|---|---------|---------------------|------------|------------|
| 1 | [desc] | `[file or area]` | High/Medium/Low | [suggestion] |

### 9. PR Review Comments Summary
| PR# | Comment | Author | Status | Action Needed |
|-----|---------|--------|--------|---------------|
| [n] | [comment] | [name] | Resolved/Open | [action] |

### 10. Test Coverage Assessment
| Area | Tests Exist? | Test File | Coverage Notes |
|------|-------------|-----------|----------------|
| [feature] | ✅/❌ | `[test file]` | [notes] |

### 11. Overall Verdict: **[PASS ✅ / PASS WITH CONCERNS ⚠️ / FAIL ❌]**
```

**For Bugs:**
```
## QA Fix Verification Report: [Title]
**Work Item**: #[ID] | **Verification Date**: [date]

### 1. PRs Reviewed
| PR# | Title | Status | Branch | Merged? |
|-----|-------|--------|--------|---------|
| [n] | [title] | [status] | [branch] | ✅/❌ |

### 2. Files Changed
| Category | Path | Change Type | Purpose |
|----------|------|-------------|----------|
| [cat] | [path] | Added/Modified/Deleted | [why] |

### 3. Fix Verification
| Check | Status | Where (File : Line) | Notes |
|-------|--------|---------------------|-------|
| Root cause addressed? | ✅/❌ | `[file:line]` | [explanation] |
| All scenarios covered? | ✅/❌ | `[file:line]` | [explanation] |
| Edge cases handled? | ✅/❌ | `[file:line]` | [explanation] |
| Error handling correct? | ✅/❌ | `[file:line]` | [explanation] |
| Pattern consistent? | ✅/❌ | `[file:line]` | [explanation] |
| Audit trail present? | ✅/❌ | `[file:line]` | [explanation] |

### 4. API Issues (if any API changes involved)
| # | What is Missed | Where (File : Line) | Severity | Potential Fix |
|---|---------------|---------------------|----------|---------------|
| 1 | [desc] | `[file:line]` | Critical/Medium/Low | [fix] |

### 5. Code Quality Issues
| # | What is Missed | Where (File : Line) | Severity | Potential Fix |
|---|---------------|---------------------|----------|---------------|
| 1 | [desc] | `[file:line]` | Critical/Medium/Low | [fix] |

### 6. Regression Risk
| # | Concern | Where (File / Area) | Risk Level | Mitigation |
|---|---------|---------------------|------------|------------|
| 1 | [desc] | `[file or area]` | High/Medium/Low | [suggestion] |

### 7. Overall Verdict: **[PASS ✅ / PASS WITH CONCERNS ⚠️ / FAIL ❌]**
```

---

## DEFECTS FOUND — MANDATORY SECTION

After Phase 2 or QA Full Review, if ANY defects are identified, list them in Azure DevOps Bug-ready format:

```
### Defect [N]: [Module] - [Clear title]
**Priority**: [1-4] | **Severity**: [1-4] | **Application Area**: [area]
**Found In**: Code Review of PR #[number] for Work Item #[ID]

**Repro Steps**: (functional user steps, not code-level)
**Expected Result**: (reference AC or documentation)
**Actual Result**: (what the code does wrong)
**Root Cause**: File, Method/Line, Issue description
**Impact**: Business impact description
```

### Priority Assignment
- **P1**: Incorrect premium/treaty, data corruption, security, wrong dates, financial errors
- **P2**: Missing validation, missing audit trail, broken business rule, AC not implemented, regression
- **P3**: Hardcoded values, missing null checks (non-critical), code quality issues
- **P4**: Naming conventions, minor style, missing comments

### Defect Summary Table
| # | Title | Priority | Severity | Type |

---

## QA FULL REVIEW
Run Phase 1 → Phase 2 → List Defects → Combined Verdict:
```
## Combined QA Verdict
### Analysis Summary | Development Verification Summary
### 🐛 Defects Found (full format above)
### Final Verdict: **[PASS ✅ / PASS WITH CONCERNS ⚠️ / FAIL ❌]**
### Action Items (item, owner, priority)
```

## Quick Reference
| Prompt | Action |
|--------|--------|
| `QA Analyse #38005` | Phase 1 |
| `QA Verify #38005` | Phase 2 |
| `QA Full Review #34892` | Phase 1 + Phase 2 |
| `API Test for #38005` | Phase 2 (if not done) → then update ReadyAPI pack per `readyapi-pack-update.md` |
| `Update ReadyAPI Pack for #38005` | Use Phase 2 API Analysis output → update 6.4 pack per `readyapi-pack-update.md` |
| *(automatic)* PR creation with API changes | Pre-PR API Gate runs automatically — no prompt needed |

---

## Companion Gate — Back Office / Portal / Database

> **If the PR also contains Back Office, Portal, Database, or Navigator XM
> changes**, you MUST additionally run the gate defined in
> `.ai/memory/backoffice_portal_inspection.md`. Both gates must pass before
> creating the PR. See that file for the full checklist.

---

## Integration with AIDLC Workflow

> **For AI agents following the AIDLC execution protocol (Mode A or Mode B):**
>
> **On session start**: Read and execute `.ai/memory/workspace-init.md` FIRST.
> This detects sibling repos, confirms with the user, and establishes session
> state. Without this, API inspection operations will not have access to the
> REST repo.
>
> When you reach the "Raise a PR" step (Mode A step 21 / Mode B step 18):
> 1. **Check** whether changed files match the PRE-PR API GATE triggers
>    (this file). If yes, run this gate.
> 2. **Check** whether changed files match the PRE-PR CODE GATE triggers
>    (`backoffice_portal_inspection.md`). If yes, run that gate too.
> 3. **Both gates must pass** before creating the PR.
> 4. If either gate verdict is **BLOCKED ❌**, do NOT create the PR —
>    report all issues and wait for developer instructions.
>
> This is a mandatory step. Skipping either gate is a workflow violation.
