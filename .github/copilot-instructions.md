# GitHub Copilot Instructions

## AIDLC Path Configuration
Spec files are always at:
- Requirements: `.aidlc/specs/{feature-name}/requirements.md`
- Design:        `.aidlc/specs/{feature-name}/design.md`
- Tasks:         `.aidlc/specs/{feature-name}/tasks.md`
- State:         `.aidlc/specs/{feature-name}/aidlc-state.md`
- Audit:         `.aidlc/specs/{feature-name}/audit.md`

Never use `.github/specs/` — always use `.aidlc/specs/`.

## Branch rules — read before starting any work

Two types of branch are always in use during feature development:

- `feature/ADO-[epic-id]-[feature-name]` — the INTEGRATION BRANCH
  - `aidlc-state.md` and `audit.md` live here
  - All state updates (claim, in-progress, done, promote) are committed here
  - Never commit implementation code directly to this branch

- `task/ADO-[id]-[task-name]` — the TASK BRANCH
  - Implementation code and tests live here
  - Created from the integration branch when claiming a task
  - Merged back to the integration branch via PR on completion

Always pull the latest integration branch before reading or writing `aidlc-state.md`.

## ADO Pre-flight Check — Required Before Spec Creation

> **If ADO MCP is unavailable when asked to create a spec: STOP. Do not create files or branches.**

The Epic ID from ADO is required to name the integration branch (`feature/ADO-[epic-id]-[feature-name]`). Without it, the branch cannot be correctly named and the workflow is compromised.

**When asked to "Create AIDLC spec for [feature-name]":**
1. Read `.aidlc/config.json` — confirm organisation, project, and `workItemTypes`
2. Attempt to create the Epic in ADO via MCP
3. **If ADO MCP is unavailable or Epic creation fails:**
   - STOP — do NOT create spec files, do NOT create branches
   - Tell the user: *"ADO MCP is unavailable. The Epic cannot be created, so no integration branch or spec files have been created. Please ensure the ADO MCP server is running and configured, then retry."*
4. Only proceed after the Epic ID is successfully obtained

## Execution protocol — choose one mode

**CRITICAL**: Before starting ANY task, you MUST read `.ai/memory/` context files.  
See [Context Memory Guide](.aidlc-guides/context-memory-guide.md) for the complete protocol.

**Quick Memory Checklist:**
1. Read `.ai/memory/workspace-init.md` — **Run workspace initialization FIRST** (detect sibling repos, confirm with user)
2. Read `.ai/memory/architecture.md` — Understand system layers
3. Read `.ai/memory/conventions.md` — Learn coding standards
4. Read task-specific files (database/API/Portal/etc. based on your work)
5. Check `.ai/memory/decisions.md` for relevant ADRs
6. Review `.ai/memory/known-issues.md` for pitfalls

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
6. Update the ADO task to "In Progress" via ADO MCP

#### IMPLEMENT (on task branch)
7. **Branch safety guard**: Run `git branch --show-current` — if the result is NOT `task/ADO-*`, STOP and tell the user: *"Current branch is '[name]'. Code must only be written on a task branch. Spec creation must complete and task branch must be created before implementation begins."*
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
19. Update the ADO task to Done via ADO MCP
20. If this is the LAST task (all tasks Done): update `.ai/memory/` files to reflect any new architecture, conventions, or key decisions introduced by this feature; commit to integration branch
21. Raise a PR from `task/ADO-[id]-[task-name]` → `feature/ADO-[epic-id]-[feature-name]`
22. Return to CLAIM

#### PROMOTE (feature → main — after ALL tasks are Done)
When every task in the feature is Done and merged into the integration branch:
23. Checkout `feature/ADO-[epic-id]-[feature-name]` and pull latest
24. **Pre-PR Inspection Gates** — before raising the PR to `main`, run the applicable code inspection gates:
    - If the feature branch has REST API changes → run Pre-PR API Gate (`.ai/memory/API_inspection.md`)
    - If the feature branch has Back Office / Portal / DB / Navigator XM changes → run Pre-PR Code Gate (`.ai/memory/backoffice_portal_inspection.md`)
    - Both gates run if both repos are changed. Both must pass before creating the PR.
    - If either gate verdict is **BLOCKED ❌**, do NOT create the PR — fix the issues first.
25. Raise a PR from `feature/ADO-[epic-id]-[feature-name]` → `main`

**Conflict rule**: If two agents claim the same task simultaneously, the claim written
LAST wins. The other agent must pull latest, re-read `aidlc-state.md`, and claim a
different Available task.

---

### Mode B: Autonomous (L3)
Use when agents are assigned individual ADO tasks directly (e.g. Copilot Coding Agent).
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
16. Update ADO task to Done via ADO MCP
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

## Azure DevOps MCP Tool Usage

**CRITICAL**: Use the correct MCP tool for each ADO operation. Tool selection errors are common - be explicit.

### Work Item Types

**Always read work item type names from `.aidlc/config.json`** (`workItemTypes` section) before creating work items.
Do not assume "User Story" — the project's ADO process template may use different names.

```
config.json workItemTypes:
- epic   → the ADO type for the feature Epic
- story  → the ADO type for user stories (e.g., "User Story", "Product Backlog Item")
- task   → the ADO type for implementation tasks
- bug    → the ADO type for bugs
- issue  → the ADO type for issues/impediments (Agile process; may not exist in all templates)
```

### Tool Selection Rules

| Operation | Tool Name | Don't Use |
|-----------|-----------|-----------|
| Get single work item status | `mcp__ado__wit_get_work_item` | ❌ batch tools |
| Create Epic/Story/Task | `mcp__ado__wit_create_work_item` | ❌ update tools |
| Update work item state | `mcp__ado__wit_update_work_item` | ❌ create tools |
| Link work items | `mcp__ado__wit_work_items_link` | ❌ create tools |

### Common Mistakes to Avoid

❌ **WRONG**: Using `wit_create_work_item` when asked to "get status"  
✅ **RIGHT**: Use `wit_get_work_item` with id and project parameters

❌ **WRONG**: Using batch tools for single operations  
✅ **RIGHT**: Use singular tools unless operating on 2+ items

### If Tool Selection Fails

If you start calling wrong tools or ignoring parameters:
1. **Stop the current operation**
2. **Read** `.aidlc-guides/copilot-ado-tool-guide.md` for explicit tool examples
3. **Restart session** if context degradation occurs

### Multi-Step ADO Operations

For complex operations like "Create ADO tickets", break into explicit steps:

```
Step 0: Read .aidlc/config.json - get workItemTypes (epic, story, task names)
Step 1: Read requirements.md - extract user stories
Step 2: For each story - use wit_create_work_item (workItemType: config.workItemTypes.story)
Step 3: For each task - use wit_create_work_item (workItemType: config.workItemTypes.task)
Step 4: Use wit_work_items_link to link tasks to stories (type: "parent")
Step 5: Update tasks.md with ADO IDs
```

**Reference**: See `.aidlc-guides/copilot-ado-tool-guide.md` for complete tool selection guide with examples.

## Quality checks
- ALL code changes MUST include tests
- No hardcoded secrets in code
- Security: input validation, parameterised queries, XSS prevention, auth checks

## Conventions
[Add project-specific conventions here — coding standards, naming, file structure]
