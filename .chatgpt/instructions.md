# ChatGPT Project Instructions

## Project Overview

**Name**: [Your Project Name]  
**Type**: [web app / API / library / service]  
**Stack**: [languages, frameworks, cloud platform]  
**Architecture**: [monolith / microservices / serverless]

## AI-SDLC Framework

This repository uses the AI-SDLC framework for structured feature development. Before starting
any work, familiarise yourself with the repository structure:

### Context Directories
- `.ai/memory/` — Architecture, conventions, decisions, dependencies, glossary
- `.ai/workflows/` — Feature development, bug fixing, code review, deployment processes
- `.ai/rules/` — Coding standards, security requirements, testing requirements
- `.ai/context/` — Business domain, user personas
- `.aidlc-rule-details/` — SDLC workflow phases (inception → construction → operations)
- `.aidlc-templates/` — Reference patterns, audit templates, compliance frameworks

### Spec Workspace
- `.aidlc/specs/{feature-name}/requirements.md` — What to build
- `.aidlc/specs/{feature-name}/design.md` — How to build it
- `.aidlc/specs/{feature-name}/tasks.md` — Work breakdown with dependencies
- `.aidlc/specs/{feature-name}/aidlc-state.md` — Live task execution state
- `.aidlc/specs/{feature-name}/audit.md` — Append-only action log

**Before generating code**, always read:
1. `.ai/memory/architecture.md` — Understand the system architecture
2. `.ai/memory/conventions.md` — Follow project patterns and conventions
3. `.ai/rules/` — Apply coding standards, security requirements, testing requirements

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

This project uses two branch types for every feature:

### Integration Branch: `feature/ADO-[epic-id]-[feature-name]`
- **Purpose**: Coordination and state tracking
- **Contains**: `aidlc-state.md`, `audit.md` (coordination files only)
- **Rule**: All state updates committed here (claim, done, promote)
- **Rule**: NO implementation code on this branch

### Task Branch: `task/ADO-[id]-[task-name]`
- **Purpose**: Implementation work
- **Contains**: Code, tests, configuration
- **Created from**: Integration branch when claiming a task
- **Merged to**: Integration branch via PR on completion

**Critical rule**: Always pull the latest integration branch before reading or writing `aidlc-state.md`.

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

Choose the mode that matches your workflow:

### Mode A: Guided Execution (L1/L2)

Use when working through tasks from `aidlc-state.md`. Suitable for manual sessions where
you choose tasks from the available work. Repeat this cycle until no Available tasks remain
or you're instructed to stop.

#### Step 1: CLAIM (on integration branch)
1. `git checkout feature/ADO-[epic-id]-[feature-name] && git pull`
2. Read `aidlc-state.md` — find the highest priority task with `Status: Available`
3. Update task to `Status: Claimed | Agent: chatgpt-[session-id]` in `aidlc-state.md`
4. Append claim entry to `audit.md` with timestamp and agent identifier
5. Commit and push `aidlc-state.md` and `audit.md` to integration branch immediately
6. Update the ADO task to "In Progress" (if ADO integration is available)

#### Step 2: IMPLEMENT (on task branch)
7. Create task branch: `git checkout -b task/ADO-[id]-[task-name] feature/ADO-[epic-id]-[feature-name]`
8. **Branch safety guard**: Run `git branch --show-current` — if NOT `task/ADO-*`, STOP immediately and tell the user: *"Current branch is '[name]'. Code must only be written on a task branch. Spec creation must complete and task branch must be created before implementation begins."* Do NOT write any code.
9. Implement the task according to `tasks.md` and `design.md`
10. Write tests according to `.ai/rules/testing-requirements.md`
11. Run tests locally and verify they pass
12. Build and run the application locally to verify changes work as expected
13. Commit and push the task branch

**Note**: If using ChatGPT web interface (no local terminal access), provide the commands for the user to run manually.

#### Step 3: COMPLETE (on integration branch)
14. `git checkout feature/ADO-[epic-id]-[feature-name] && git pull`
15. Update task to `Status: Done` in `aidlc-state.md`
16. Promote newly unblocked tasks from `Blocked` → `Available` (check dependency graph in `tasks.md`)
17. Append completion entry to `audit.md`
18. Commit and push `aidlc-state.md` and `audit.md` to integration branch
19. Update ADO task to Done (if ADO integration is available)
20. If this is the LAST task (all tasks Done): update `.ai/memory/` files to reflect any new architecture, conventions, or key decisions introduced by this feature; commit to integration branch
21. Raise a PR from `task/ADO-[id]-[task-name]` → `feature/ADO-[epic-id]-[feature-name]`
22. Return to Step 1 (CLAIM next available task)

**Conflict handling**: If two agents claim the same task simultaneously, the claim written
LAST wins. The other agent must `git pull`, re-read `aidlc-state.md`, and claim a different
Available task.

---

### Mode B: Autonomous Execution (L3)

Use when you're assigned an individual ADO task directly (no manual task selection needed).
In this mode, ADO is the coordination layer — do NOT read `aidlc-state.md` to decide what
to work on. Only update `aidlc-state.md` as an audit record after completing your assigned task.

1. Read your assigned ADO task for complete requirements
2. Locate the task in `.aidlc/specs/[feature-name]/tasks.md` by ADO ID
3. Read `design.md` and `requirements.md` for context
4. Create task branch: `git checkout -b task/ADO-[id]-[task-name] feature/ADO-[epic-id]-[feature-name]`
5. **Branch safety guard**: Run `git branch --show-current` — if NOT `task/ADO-*`, STOP immediately and alert the user. Do NOT write any code.
6. Implement the task per `tasks.md` and `design.md`
7. Write tests per `.ai/rules/testing-requirements.md`
8. Run tests locally and verify they pass
9. Build and run the application locally to verify changes work as expected
10. Commit and push the task branch
11. `git checkout feature/ADO-[epic-id]-[feature-name] && git pull`
12. Update task to `Status: Done` in `aidlc-state.md`
13. Promote newly unblocked tasks from `Blocked` → `Available`
14. Append completion entry to `audit.md`
15. Commit and push `aidlc-state.md` and `audit.md` to integration branch
16. Update ADO task to Done (if ADO integration available)
17. If this is the LAST task (all tasks Done): update `.ai/memory/` files to reflect any new architecture, conventions, or key decisions introduced by this feature; commit to integration branch
18. Raise a PR from `task/ADO-[id]-[task-name]` → `feature/ADO-[epic-id]-[feature-name]`

**Note**: If using ChatGPT web interface (no local terminal access), provide the commands for the user to run manually.

## Coding Standards (Required)

Follow `.ai/rules/coding-standards.md` for complete standards. Key requirements:

### Must-Have for All Changes
- **Tests**: All code changes MUST include tests (unit + integration where applicable)
- **No secrets**: Never commit hardcoded secrets, API keys, passwords, or tokens
- **Security**: Input validation, parameterised queries, XSS prevention, auth checks
- **Error handling**: Proper try-catch with logging, user-friendly error messages
- **Code review**: All changes via PR, never commit directly to integration or main

### Testing Requirements
Follow `.ai/rules/testing-requirements.md`. Key points:
- Unit tests for all functions/methods
- Integration tests for APIs and database operations
- Test coverage minimum: [specify percentage]
- Test naming: [specify convention]
- Test file location: [specify pattern]

### Security Requirements
Follow `.ai/rules/security-requirements.md`. Key points:
- Authentication: [describe mechanism]
- Authorization: [describe approach]
- Data protection: [encryption requirements]
- Secret management: [Key Vault, environment variables, etc.]
- Input validation: All external inputs validated

## Project Conventions

Read `.ai/memory/conventions.md` for full conventions. Quick reference:

### Naming Conventions
- [Classes: PascalCase, CamelCase, etc.]
- [Functions: camelCase, snake_case, etc.]
- [Variables: camelCase, snake_case, etc.]
- [Files: kebab-case, PascalCase, etc.]

### File Structure
- [Where to put new features]
- [How to organise tests]
- [Configuration file locations]

### Error Handling
- [Pattern for exceptions]
- [Logging requirements]
- [User-facing error messages]

### Architecture Constraints
Read `.ai/memory/architecture.md` for complete architecture. Key points:
- [List 3-5 key architectural constraints]
- [e.g., "All services communicate via API Gateway"]
- [e.g., "Database access only through repository pattern"]

## Reference Materials

When designing features:
- Architecture patterns: `.aidlc-templates/architecture-patterns/[aws|azure|hybrid].md`
- CI/CD patterns: `.aidlc-templates/pipeline-patterns/`
- Security audits: `.aidlc-templates/audit-templates/security/`
- Compliance: `.aidlc-templates/compliance/` (if applicable)

## Additional Resources

- Working guide: `.aidlc-guides/working-guide.md`
- Quick reference: `.aidlc-guides/quick-reference.md`
- Role-specific guides: `.aidlc-guides/roles/`
- Prompt cheat sheet: `.aidlc-guides/prompt-cheat-sheet.md`
