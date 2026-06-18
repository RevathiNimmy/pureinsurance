# AI-SDLC Steering Rules

## Project Context

**Project**: [Your Project Name]  
**Type**: [web app / API / library / service]  
**Stack**: [languages, frameworks, cloud platform]  
**Architecture**: [monolith / microservices / serverless]  
**Team**: [team name or size]

## Repository Structure

This repository uses the AI-SDLC framework for structured, traceable feature development:

### Context Directories (Read Before Starting Work)
- `ai/memory/workspace-init.md` — **Run workspace initialization FIRST** (detect sibling repos, confirm with user)
- `.ai/memory/` — Architecture, conventions, decisions, dependencies, glossary
- `.ai/workflows/` — Feature development, bug fixing, code review, deployment
- `.ai/rules/` — Coding standards, security requirements, testing requirements
- `.ai/context/` — Business domain, user personas, integration points
- `.aidlc-rule-details/` — SDLC workflow phases (inception → construction → operations)
- `.aidlc-templates/` — Reference patterns, audit templates, compliance frameworks

### Spec Workspace (Work Artifacts)
- `.aidlc/specs/{feature-name}/requirements.md` — What to build
- `.aidlc/specs/{feature-name}/design.md` — How to build it
- `.aidlc/specs/{feature-name}/tasks.md` — Work breakdown with dependencies
- `.aidlc/specs/{feature-name}/aidlc-state.md` — Live execution state (Available/Claimed/Done)
- `.aidlc/specs/{feature-name}/audit.md` — Append-only action log

**Before generating code**: Always read `.ai/memory/architecture.md` and `.ai/memory/conventions.md`

## Work Item Types Configuration

**Always read `.aidlc/config.json`** before creating ADO work items. The `workItemTypes` section defines
the exact type names for this project's ADO process template:

- `workItemTypes.epic` — type to use for the feature Epic
- `workItemTypes.story` — type to use for user stories (varies: "User Story", "Product Backlog Item", "Requirement", or custom)
- `workItemTypes.task` — type to use for implementation tasks
- `workItemTypes.bug` — type to use for bugs
- `workItemTypes.issue` — type to use for issues/impediments (Agile process; may not exist in all templates)

Never assume "User Story" or "Task" — always use the names from config.json.

## Branch Strategy

> **⚠️ CRITICAL: Branch Separation Rule**
> 
> **NEVER commit implementation code to the integration branch.**
> 
> - Integration branch = coordination ONLY (aidlc-state.md, audit.md)
> - Task branch = implementation (code, tests, config)
> 
> You MUST create a task branch before implementing. Committing code to the integration branch violates the workflow.

Two branch types are used for every feature:

### Integration Branch: `feature/ADO-[epic-id]-[feature-name]`
- **Home for coordination files**: `aidlc-state.md`, `audit.md`
- **All state updates committed here**: claim, in-progress, done, dependency promotion
- **No implementation code**: Never commit code directly to this branch

### Task Branch: `task/ADO-[id]-[task-name]`
- **Home for implementation**: Code, tests, configuration
- **Created from integration branch** when claiming a task
- **Merged back via PR** on completion

**Critical**: Always pull the latest integration branch before reading or writing `aidlc-state.md`

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

## Execution Protocol

### Mode A: Guided Execution (L1/L2)

Use when working through tasks from `aidlc-state.md`. Suitable for manual agent sessions
or developer-driven work. Repeat cycle until no Available tasks remain.

#### Phase 1: CLAIM (integration branch)
1. `git checkout feature/ADO-[epic-id]-[feature-name] && git pull`
2. Read `aidlc-state.md` — locate highest priority task where `Status: Available`
3. Update task to `Status: Claimed | Agent: [session-id]` in `aidlc-state.md`
4. Append claim record to `audit.md` with timestamp and agent ID
5. `git commit -m "claim: [task-name]" && git push` (immediate)
6. Update ADO task to "In Progress" via ADO MCP if available

#### Phase 2: IMPLEMENT (task branch)
7. **MUST create task branch first**: `git checkout -b task/ADO-[id]-[task-name] feature/ADO-[epic-id]-[feature-name]`
8. **Branch safety guard**: Run `git branch --show-current` — if NOT `task/ADO-*`, STOP immediately and tell the user: *"Current branch is '[name]'. Code must only be written on a task branch. Spec creation must complete and task branch must be created before implementation begins."* Do NOT write any code.
9. Implement per `tasks.md` and `design.md`
10. Write tests per `.ai/rules/testing-requirements.md`
11. Run tests locally and verify they pass
12. Build and run the application locally to verify changes work as expected
13. `git commit && git push` task branch

#### Phase 3: COMPLETE (integration branch)
14. `git checkout feature/ADO-[epic-id]-[feature-name] && git pull`
15. Update task to `Status: Done` in `aidlc-state.md`
16. Promote unblocked tasks: `Blocked` → `Available` (check dependency graph)
17. Append completion record to `audit.md`
18. `git commit -m "complete: [task-name]" && git push`
19. Update ADO task to Done via ADO MCP if available
20. If this is the LAST task (all tasks Done): update `.ai/memory/` files to reflect any new architecture, conventions, or key decisions introduced by this feature; commit to integration branch
21. Raise PR: `task/ADO-[id]-[task-name]` → `feature/ADO-[epic-id]-[feature-name]`
22. Return to Phase 1 (CLAIM next task)

**Conflict handling**: If simultaneous claims occur, the last-written claim wins.
Losing agent must `git pull`, re-read `aidlc-state.md`, and claim a different task.

#### Phase 4: PROMOTE (feature → main — after ALL tasks are Done)
When every task in the feature is Done and merged into the integration branch:
23. `git checkout feature/ADO-[epic-id]-[feature-name] && git pull`
24. **Pre-PR Inspection Gates** — before raising the PR to `main`, run the applicable code inspection gates:
    - If the feature branch has REST API changes → run Pre-PR API Gate (`.ai/memory/API_inspection.md`)
    - If the feature branch has Back Office / Portal / DB / Navigator XM changes → run Pre-PR Code Gate (`.ai/memory/backoffice_portal_inspection.md`)
    - Both gates run if both repos are changed. Both must pass before creating the PR.
    - If either gate verdict is **BLOCKED ❌**, do NOT create the PR — fix the issues first.
25. Raise PR: `feature/ADO-[epic-id]-[feature-name]` → `main`

---

### Mode B: Autonomous Execution (L3)

Use when assigned an ADO task directly (no manual task selection needed).
Do NOT read `aidlc-state.md` to choose work — ADO is the coordination layer.
Update `aidlc-state.md` only as an audit record after completion.

1. Read assigned ADO task for requirements
2. Locate task in `.aidlc/specs/[feature-name]/tasks.md` by ADO ID
3. Read `design.md` and `requirements.md` for context
4. **MUST create task branch first**: `git checkout -b task/ADO-[id]-[task-name] feature/ADO-[epic-id]-[feature-name]`
5. **Branch safety guard**: Run `git branch --show-current` — if NOT `task/ADO-*`, STOP immediately and alert the user. Do NOT write any code.
6. Implement per `tasks.md` and `design.md`
7. Write tests per `.ai/rules/testing-requirements.md`
8. Run tests locally and verify they pass
9. Build and run the application locally to verify changes work as expected
10. `git commit && git push` task branch
11. `git checkout feature/ADO-[epic-id]-[feature-name] && git pull`
12. Update task to `Status: Done` in `aidlc-state.md`
13. Promote unblocked tasks: `Blocked` → `Available`
14. Append completion record to `audit.md`
15. `git commit -m "complete: [task-name]" && git push`
16. Update ADO task to Done via ADO MCP if available
17. If this is the LAST task (all tasks Done): update `.ai/memory/` files to reflect any new architecture, conventions, or key decisions introduced by this feature; commit to integration branch
18. Raise PR: `task/ADO-[id]-[task-name]` → `feature/ADO-[epic-id]-[feature-name]`

#### PROMOTE (feature → main — after ALL tasks are Done)
When every task in the feature is Done and merged into the integration branch:
19. `git checkout feature/ADO-[epic-id]-[feature-name] && git pull`
20. **Pre-PR Inspection Gates** — before raising the PR to `main`, run the applicable code inspection gates:
    - If the feature branch has REST API changes → run Pre-PR API Gate (`.ai/memory/API_inspection.md`)
    - If the feature branch has Back Office / Portal / DB / Navigator XM changes → run Pre-PR Code Gate (`.ai/memory/backoffice_portal_inspection.md`)
    - Both gates run if both repos are changed. Both must pass before creating the PR.
    - If either gate verdict is **BLOCKED ❌**, do NOT create the PR — fix the issues first.
21. Raise PR: `feature/ADO-[epic-id]-[feature-name]` → `main`

## Project-Specific Rules

### Coding Standards
Follow `.ai/rules/coding-standards.md`. Key requirements:
- All code changes MUST include tests
- No hardcoded secrets (use environment variables, Key Vault, or secret management)
- Input validation on all external inputs (API, user forms, file uploads)
- Parameterised queries for database access (prevent SQL injection)
- Proper error handling with logging (no silent failures)

### Testing Requirements
Follow `.ai/rules/testing-requirements.md`. Minimum standards:
- Unit tests for all functions/methods
- Integration tests for API endpoints and database operations
- Test coverage: [specify minimum, e.g., 80%]
- Test file naming: [specify convention]
- Mocking strategy: [specify when to mock vs. integration test]

### Security Requirements
Follow `.ai/rules/security-requirements.md`. Key points:
- Authentication: [describe approach, e.g., OAuth2, JWT]
- Authorization: [describe approach, e.g., RBAC, claims-based]
- Data protection: [encryption at rest, in transit]
- Secret management: [Azure Key Vault, AWS Secrets Manager, etc.]
- Dependency scanning: [automated vulnerability checks]

### Architecture Constraints
Read `.ai/memory/architecture.md` for full documentation. Key constraints:
- [Add 3-5 architectural constraints]
- [e.g., "All services must communicate via API Gateway"]
- [e.g., "Database access only through repository pattern"]
- [e.g., "Async operations use message queue (Service Bus/SQS)"]

### Conventions
Read `.ai/memory/conventions.md` for full conventions. Quick reference:
- Naming: [specify conventions for classes, functions, variables]
- File structure: [specify directory organization]
- Error handling: [specify patterns]
- Logging: [specify format, levels, structured logging]
- Configuration: [environment-specific, how to add new settings]

## Reference Materials

When designing new features:
- Architecture patterns: `.aidlc-templates/architecture-patterns/[aws|azure|hybrid].md`
- CI/CD patterns: `.aidlc-templates/pipeline-patterns/`
- Security audits: `.aidlc-templates/audit-templates/security/`
- Compliance: `.aidlc-templates/compliance/[HIPAA|PCI-DSS|SOC2]/` (if applicable)
