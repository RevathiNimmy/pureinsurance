# AI-SDLC Rules for Amazon Q Developer

## Project Context

**Project**: [Your Project Name]  
**Type**: [web app / API / library / service]  
**Stack**: [languages, frameworks, cloud platform]  
**Architecture**: [monolith / microservices / serverless]

## Repository Structure — Read Before Starting Work

This repository uses the AI-SDLC framework for structured feature development:
- `.ai/memory/workspace-init.md` — **Run workspace initialization FIRST** (detect sibling repos, confirm with user)
- `.ai/memory/` — Architecture, conventions, decisions, dependencies, glossary
- `.ai/workflows/` — Feature development, bug fixing, code review, deployment processes
- `.ai/rules/` — Coding standards, security requirements, testing requirements
- `.ai/context/` — Business domain knowledge, user personas
- `.aidlc-rule-details/` — SDLC workflow phases (inception → construction → operations)
- `.aidlc-templates/` — Reference architecture patterns, audit templates, compliance frameworks
- `.aidlc/specs/{feature-name}/` — Feature specifications with requirements, design, tasks, state, audit

**Always read** `.ai/memory/architecture.md` and `.ai/memory/conventions.md` before generating code.

> **Brownfield verification rule (Rule 13)**: For brownfield features, verify the key claims in `.ai/memory/architecture.md` against actual source files before generating any spec artefacts. If a claim is wrong, correct the memory file first and log the correction in `audit.md`. See `.aidlc-rule-details/common/core-workflow.md` Rule 13 and `.aidlc-rule-details/inception/workspace-detection.md` Step 3a.

## Repository Routing — MAIN Code Sets

Pure Insurance uses **two repositories**, both cloned side-by-side in the same parent folder on the developer's machine (e.g. `source\repos\` or equivalent). Resolve the actual local path from the IDE workspace at session start — do not hardcode any user-specific path.

| Repo | Folder name | Scope |
|------|-------------|-------|
| `PureInsurance` | `PureInsurance` | **Back Office** (WinForms `b*`/`i*`/`g*Library` VB.NET), **Portal** (ASP.NET Web Forms `.aspx`), **Business Components**, Navigator XM roadmap XML, **Database** (stored procedures, migration scripts), spec/context files (`.aidlc/`, `.ai/`, `.amazonq/`) |
| `PureInsurance.REST` | `PureInsurance.REST` | **REST API** microservices — C# ASP.NET Core, CQRS Controllers, QueryHandlers, CommandHandlers, Services, Repositories, Domain models, API unit tests |

> **Path resolution**: Both repos share the same parent directory. If the current workspace is `{parent}\PureInsurance`, the REST repo is at `{parent}\PureInsurance.REST`. Always derive the sibling path at runtime — never hardcode a username or machine-specific path.

**Decision rule — which repo?**
- New/changed REST endpoint, Controller, Handler, Service, Repository, Domain model, or API test → `PureInsurance.REST`
- Everything else (Portal page, back-office component, stored procedure, XML roadmap, migration script) → `PureInsurance`
- Most Portal features require changes in **both** repos — always list both in the task/plan before starting

**Wrong-repo rule**: If a plan step targets the wrong repo, STOP and correct the plan before writing any code.

## AIDLC Path Configuration

Spec files are always at:
- Requirements: `.aidlc/specs/{feature-name}/requirements.md`
- Design:        `.aidlc/specs/{feature-name}/design.md`
- Tasks:         `.aidlc/specs/{feature-name}/tasks.md`
- State:         `.aidlc/specs/{feature-name}/aidlc-state.md`
- Audit:         `.aidlc/specs/{feature-name}/audit.md`

Never use `.amazonq/specs/` — always use `.aidlc/specs/`.

## Work Item Types Configuration

**Always read `.aidlc/config.json`** before creating ADO work items. The `workItemTypes` section defines
the exact type names for this project's ADO process template:

- `workItemTypes.epic` — type to use for the feature Epic
- `workItemTypes.story` — type to use for user stories (varies: "User Story", "Product Backlog Item", "Requirement", or custom)
- `workItemTypes.task` — type to use for implementation tasks
- `workItemTypes.bug` — type to use for bugs
- `workItemTypes.issue` — type to use for issues/impediments (Agile process; may not exist in all templates)

Never assume "User Story" or "Task" — always use the names from config.json.

## Branch Rules — Read Before Starting Any Work

Two types of branch are always in use during feature development:

- `feature/ADO-[epic-id]-[feature-name]` — the **INTEGRATION BRANCH**
  - `aidlc-state.md` and `audit.md` live here
  - All state updates (claim, in-progress, done, promote) are committed here
  - Never commit implementation code directly to this branch

- `task/ADO-[id]-[task-name]` — the **TASK BRANCH**
  - Implementation code and tests live here
  - Created from the integration branch when claiming a task
  - Merged back to the integration branch via PR on completion

**Always pull the latest integration branch before reading or writing `aidlc-state.md`.**

## ADO Pre-flight Check — Required Before Spec Creation

> **If ADO MCP is unavailable when asked to create a spec: STOP. Do not create files or branches.**

The Epic ID from ADO is required to name the integration branch (`feature/ADO-[epic-id]-[feature-name]`). Without it, branches cannot be correctly named.

**When asked to "Create AIDLC spec for [feature-name]":**
1. Read `.aidlc/config.json` — confirm organisation, project, and `workItemTypes`
2. Attempt to create the Epic in ADO via MCP
3. **If ADO MCP is unavailable or Epic creation fails:**
   - STOP — do NOT create spec files, do NOT create branches
   - Tell the user: *"ADO MCP is unavailable. The Epic cannot be created, so no integration branch or spec files have been created. Please ensure the ADO MCP server is running and configured, then retry."*
4. Only proceed after the Epic ID is successfully obtained

## Execution Protocol — Choose One Mode

### Mode A: Guided (L1/L2)

Use when agents are started manually per session, or developers work through tasks.
`aidlc-state.md` is the coordination file — read it to decide what to work on.
Repeat until no Available tasks remain or instructed to stop.

#### CLAIM (on integration branch)
1. Checkout `feature/ADO-[epic-id]-[feature-name]` and pull latest
2. Read `aidlc-state.md` — find the highest priority task with `Status: Available`
3. Write `Status: Claimed | Agent: [your session ID]` to `aidlc-state.md`
4. Append a claim entry to `audit.md`
5. Commit and push `aidlc-state.md` and `audit.md` to the integration branch immediately
6. Update the ADO task to "In Progress" via ADO MCP (if available)

#### IMPLEMENT (on task branch)
7. **Branch safety guard**: Run `git branch --show-current` — if NOT `task/ADO-*`, STOP and tell the user: *"Current branch is '[name]'. Code must only be written on a task branch. Spec creation must complete and task branch must be created before implementation begins."*
8. Create a task branch: `git checkout -b task/ADO-[id]-[task-name] feature/ADO-[epic-id]-[feature-name]`
9. Implement the task per `tasks.md` and `design.md`
10. Write tests per `.ai/rules/testing-requirements.md`
11. Run tests locally and verify they pass
12. Build and run the application locally to verify changes work as expected
13. Commit and push the task branch

#### COMPLETE (on integration branch)
14. Checkout `feature/ADO-[epic-id]-[feature-name]` and pull latest
15. Mark `Status: Done` for the task in `aidlc-state.md`
16. Promote any newly unblocked tasks from `Blocked` → `Available` in `aidlc-state.md`
17. Append a completion entry to `audit.md`
18. Commit and push `aidlc-state.md` and `audit.md` to the integration branch
19. Update the ADO task to Done via ADO MCP (if available)
20. If this is the LAST task (all tasks Done): update `.ai/memory/` files to reflect any new architecture, conventions, or key decisions introduced by this feature; commit to integration branch
21. Raise a PR from `task/ADO-[id]-[task-name]` → `feature/ADO-[epic-id]-[feature-name]`
22. Return to CLAIM

**Conflict rule**: If two agents claim the same task simultaneously, the claim written
LAST wins. The other agent must pull latest, re-read `aidlc-state.md`, and claim a
different Available task.

#### PROMOTE (feature → main — after ALL tasks are Done)
When every task in the feature is Done and merged into the integration branch:
23. Checkout `feature/ADO-[epic-id]-[feature-name]` and pull latest
24. **Pre-PR Inspection Gates** — before raising the PR to `main`, run the applicable code inspection gates:
    - If the feature branch has REST API changes → run Pre-PR API Gate (`.ai/memory/API_inspection.md`)
    - If the feature branch has Back Office / Portal / DB / Navigator XM changes → run Pre-PR Code Gate (`.ai/memory/backoffice_portal_inspection.md`)
    - Both gates run if both repos are changed. Both must pass before creating the PR.
    - If either gate verdict is **BLOCKED ❌**, do NOT create the PR — fix the issues first.
25. Raise a PR from `feature/ADO-[epic-id]-[feature-name]` → `main`

---

### Mode B: Autonomous (L3)

Use when agents are assigned individual ADO tasks directly.
ADO is the coordination layer — do NOT read `aidlc-state.md` to decide what to work on.
Update `aidlc-state.md` as an audit record only after completing your assigned task.
Run this protocol once per assigned task.

1. Read your assigned ADO task for full requirements
2. Read `.aidlc/specs/[feature-name]/tasks.md` — find your task by ADO ID
3. Read `design.md` and `requirements.md` for context
4. Create task branch: `git checkout -b task/ADO-[id]-[task-name] feature/ADO-[epic-id]-[feature-name]`
5. **Branch safety guard**: Run `git branch --show-current` — if NOT `task/ADO-*`, STOP and alert the user
6. Implement the task per `tasks.md` and `design.md`
7. Write tests per `.ai/rules/testing-requirements.md`
8. Run tests locally and verify they pass
9. Build and run the application locally to verify changes work as expected
10. Commit and push the task branch
11. Checkout integration branch and pull latest
12. Mark `Status: Done` for your task in `aidlc-state.md`
13. Promote any newly unblocked tasks from `Blocked` → `Available` in `aidlc-state.md`
14. Append a completion entry to `audit.md`
15. Commit and push `aidlc-state.md` and `audit.md` to the integration branch
16. Update ADO task to Done via ADO MCP (if available)
17. If this is the LAST task (all tasks Done): update `.ai/memory/` files to reflect any new architecture, conventions, or key decisions introduced by this feature; commit to integration branch
18. Raise a PR from `task/ADO-[id]-[task-name]` → `feature/ADO-[epic-id]-[feature-name]`

#### PROMOTE (feature → main — after ALL tasks are Done)
When every task in the feature is Done and merged into the integration branch:
19. Checkout `feature/ADO-[epic-id]-[feature-name]` and pull latest
20. **Pre-PR Inspection Gates** — before raising the PR to `main`, run the applicable code inspection gates:
    - If the feature branch has REST API changes → run Pre-PR API Gate (`.ai/memory/API_inspection.md`)
    - If the feature branch has Back Office / Portal / DB / Navigator XM changes → run Pre-PR Code Gate (`.ai/memory/backoffice_portal_inspection.md`)
    - Both gates run if both repos are changed. Both must pass before creating the PR.
    - If either gate verdict is **BLOCKED ❌**, do NOT create the PR — fix the issues first.
21. Raise a PR from `feature/ADO-[epic-id]-[feature-name]` → `main`

## Coding Standards

Follow `.ai/rules/coding-standards.md` for project-specific standards. Key requirements:

- **All code changes MUST include tests**
- **No hardcoded secrets** in code (use Key Vault, environment variables, or secret management)
- **Security**: input validation, parameterised queries, XSS prevention, auth checks
- **Error handling**: proper try-catch with logging, user-friendly error messages
- **Code style**: [Add project-specific style guide reference]

## Testing Requirements

Follow `.ai/rules/testing-requirements.md` for project-specific testing standards.

- Unit tests required for all new functions/methods
- Integration tests for API endpoints and database operations
- Test coverage minimum: [specify percentage, e.g., 80%]
- Test naming: [specify convention, e.g., `test_<method>_<scenario>_<expected>`]

## Architecture Context

Read `.ai/memory/architecture.md` for full architecture documentation. Key points:

- [Add 3-5 bullet points about your architecture]
- [e.g., "Microservices architecture with API Gateway"]
- [e.g., "PostgreSQL database with Redis caching layer"]
- [e.g., "Event-driven with Azure Service Bus"]

## Conventions

Read `.ai/memory/conventions.md` for full conventions. Key points:

- [Add project-specific naming conventions]
- [Add file structure conventions]
- [Add error handling patterns]
- [Add logging patterns]
