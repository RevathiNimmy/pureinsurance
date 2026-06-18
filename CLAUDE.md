# Claude Code Project Instructions

@AGENTS.md

## Project Context

**Project**: [Your Project Name]  
**Type**: [web app / API / library / service]  
**Stack**: [languages, frameworks, databases, cloud platform]  
**Architecture**: [monolith / microservices / serverless]

## Working with AI-SDLC

This repository uses the AI-SDLC framework. See AGENTS.md (imported above) for the framework
structure. The following instructions are specific to Claude Code's workflow.

### Work Tracking Configuration

Read `.aidlc/config.json` for work tracking system configuration:

- `workTracking.system`: "azuredevops" or "github" (which work tracking system to use)
- `workTracking.organization`: Azure DevOps organization name (e.g., "SSP-Insurer")
- `workTracking.project`: ADO project name or GitHub repository
- `workItemTypes.epic`: ADO work item type for the feature Epic (e.g., "Epic")
- `workItemTypes.story`: ADO work item type for user stories (e.g., "User Story", "Product Backlog Item", "Requirement")
- `workItemTypes.task`: ADO work item type for tasks (e.g., "Task")
- `workItemTypes.bug`: ADO work item type for bugs (e.g., "Bug")
- `workItemTypes.issue`: ADO work item type for issues/impediments (e.g., "Issue" — Agile process only; may not exist in all templates)

**When creating work items:**
- Always use the work item type names from `workItemTypes` in config.json — never assume "User Story" or other type names
- Use the organization and project from config.json
- Update work item status via MCP as you progress through tasks
- Work item IDs must be recorded in tasks.md and aidlc-state.md

### Before Starting Work

1. Read `.ai/memory/architecture.md` — understand the system architecture
2. Read `.ai/memory/conventions.md` — follow project coding patterns
3. Read `.ai/rules/` — apply coding standards, security requirements, testing requirements
4. If working on a spec: Read `.aidlc/specs/[feature-name]/requirements.md` and `design.md`

### ADO Pre-flight Check — Required Before Spec Creation

> **If the user asks you to create a spec and ADO MCP is unavailable: STOP. Do not create files or branches.**

Creating the Epic in ADO is **step 0** of spec creation — the Epic ID is required to correctly name the integration branch (`feature/ADO-[epic-id]-[feature-name]`).

**When asked to "Create AIDLC spec for [feature-name]":**
1. Read `.aidlc/config.json` — confirm organisation, project, and `workItemTypes`
2. Attempt to create the Epic in ADO via MCP
3. **If ADO MCP is unavailable or Epic creation fails:**
   - STOP immediately — do NOT create any spec files, do NOT create any branches
   - Tell the user: *"ADO MCP is unavailable. The Epic cannot be created, so no integration branch or spec files have been created. Please ensure the ADO MCP server is running and configured, then retry."*
   - Wait for the user to resolve the issue
4. Only proceed with spec generation after the Epic ID is successfully obtained

### Branch Strategy

Two branch types for every feature:

**Integration Branch**: `feature/ADO-[epic-id]-[feature-name]`
- Home for `aidlc-state.md` and `audit.md` (coordination files)
- All state updates committed here (claim, done, promote)
- NO implementation code on this branch

**Task Branch**: `task/ADO-[id]-[task-name]`
- Home for implementation code and tests
- Created from integration branch when claiming a task
- Merged back via PR on completion

**Always pull the latest integration branch before reading or writing `aidlc-state.md`.**

## Execution Protocol

### Mode A: Guided (L1/L2)

Use when working through tasks from `aidlc-state.md`. Repeat until no Available tasks remain.

#### CLAIM (integration branch)
1. Checkout `feature/ADO-[epic-id]-[feature-name]` and pull latest
2. Read `aidlc-state.md` — find highest priority task with `Status: Available`
3. Write `Status: Claimed | Agent: claude-code-[session-id]` to `aidlc-state.md`
4. Append claim entry to `audit.md` with timestamp
5. Commit and push `aidlc-state.md` + `audit.md` immediately
6. Update ADO task to "In Progress" via MCP

#### IMPLEMENT (task branch)
7. **Branch safety guard**: Run `git branch --show-current` — if the result is NOT `task/ADO-*`, STOP and tell the user: *"Current branch is '[name]'. Code must only be written on a task branch. Spec creation must complete and task branch must be created before implementation begins."*
8. Create task branch: `git checkout -b task/ADO-[id]-[task-name] feature/ADO-[epic-id]-[feature-name]`
9. Implement per `tasks.md` and `design.md`
10. Write tests per `.ai/rules/testing-requirements.md`
11. Run tests locally and verify they pass
12. Build and run the application locally to verify changes work as expected
13. Commit and push task branch

#### COMPLETE (integration branch)
14. Checkout `feature/ADO-[epic-id]-[feature-name]` and pull latest
15. Set task `Status: Done` in `aidlc-state.md`
16. Promote unblocked tasks from `Blocked` → `Available` (check dependencies)
17. Append completion entry to `audit.md`
18. Commit and push `aidlc-state.md` + `audit.md`
19. Update ADO task to Done via MCP
20. If this is the LAST task (all tasks Done): update `.ai/memory/` files to reflect any new architecture, conventions, or key decisions introduced by this feature; commit to integration branch
21. Raise PR: `task/ADO-[id]-[task-name]` → `feature/ADO-[epic-id]-[feature-name]`
22. Return to CLAIM

**Conflict rule**: If two agents claim simultaneously, last-written claim wins.
Other agent must pull, re-read `aidlc-state.md`, and claim a different task.

---

### Mode B: Autonomous (L3)

Use when assigned an ADO task directly (no manual task selection).
Do NOT read `aidlc-state.md` to choose work — ADO assigns the task.
Update `aidlc-state.md` only as an audit record after completion.

1. Read assigned ADO task
2. Find task in `.aidlc/specs/[feature-name]/tasks.md` by ADO ID
3. Read `design.md` and `requirements.md` for context
4. Create task branch from integration branch
5. **Branch safety guard**: Run `git branch --show-current` — if NOT `task/ADO-*`, STOP and alert the user
6. Implement per `tasks.md` and `design.md`
7. Write tests per `.ai/rules/testing-requirements.md`
8. Run tests locally and verify they pass
9. Build and run the application locally to verify changes work as expected
10. Commit and push task branch
11. Checkout integration branch and pull latest
12. Set task `Status: Done` in `aidlc-state.md`
13. Promote unblocked tasks from `Blocked` → `Available`
14. Append completion entry to `audit.md`
15. Commit and push `aidlc-state.md` + `audit.md`
16. Update ADO task to Done via MCP
17. If this is the LAST task (all tasks Done): update `.ai/memory/` files to reflect any new architecture, conventions, or key decisions introduced by this feature; commit to integration branch
18. Raise PR: `task/ADO-[id]-[task-name]` → `feature/ADO-[epic-id]-[feature-name]`

## Key Coding Rules

### Required for All Changes
- **Tests**: All code changes MUST include tests (unit + integration where applicable)
- **No secrets**: Never hardcode secrets, API keys, passwords, or tokens
- **Security**: Input validation, parameterised queries, XSS prevention, auth checks
- **Error handling**: Proper try-catch with logging, user-friendly error messages

### Project Standards
- Follow `.ai/rules/coding-standards.md` for style and conventions
- Follow `.ai/rules/security-requirements.md` for security patterns
- Follow `.ai/rules/testing-requirements.md` for test coverage and structure

### Architecture Patterns
- Read `.ai/memory/architecture.md` for system design
- Follow established patterns (don't introduce new patterns without discussion)
- Use `.aidlc-templates/architecture-patterns/` for reference implementations

## Reference

- **Daily prompts**: `.aidlc-guides/prompts/daily-prompts.md`
- **Prompt cheat sheet**: `.aidlc-guides/prompt-cheat-sheet.md`
- **Working guide**: `.aidlc-guides/working-guide.md`
- **Role guides**: `.aidlc-guides/roles/[developer|architect|qa|devops|product-owner]-guide.md`
