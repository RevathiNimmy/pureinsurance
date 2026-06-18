# Core Workflow Rules

These rules apply across all three phases: INCEPTION, CONSTRUCTION, and OPERATIONS. Every AI agent must follow these rules when working on features using the AIDLC framework.

**Last Updated**: 2026-03-30
**Version**: 1.0

---

## Rule 1: Always Read Configuration First

Before starting any work on a feature, the agent must read `.aidlc/config.json` in the repo root.

This file contains:
- Project name
- Team name
- Required approvals
- Deployment targets
- File paths configuration

**Action**:
```
At session start:
1. Read .aidlc/config.json
2. Confirm the project name and paths
3. Proceed with phase-specific rules
```

**Why**: Configuration tells agents what project they're working on and what approvals are required. Prevents working on the wrong feature or skipping required reviews.

---

## Rule 2: Always Use .aidlc/specs/ for Specifications

All feature specifications, designs, and task breakdowns must be created in:
```
.aidlc/specs/{feature-name}/
```

Never scatter specs across the repo or in `.aidlc/` root directory.

**Required Structure**:
```
.aidlc/specs/user-auth-oauth2/
├── requirements.md           # INCEPTION output
├── design.md                 # INCEPTION output
├── tasks.md                  # INCEPTION output
├── aidlc-state.md           # Current phase & progress
├── audit.md                  # All work log
├── test-summary.md           # CONSTRUCTION output
├── code-review-findings.md   # CONSTRUCTION output
├── deployment.md             # OPERATIONS output
├── monitoring.md             # OPERATIONS output
└── incident-response.md      # OPERATIONS output
```

**Action**:
```
if feature-name not yet in .aidlc/specs/:
  mkdir .aidlc/specs/{feature-name}/
  create aidlc-state.md with initial phase and timestamp

create or update .aidlc/specs/{feature-name}/{filename}.md
```

**Why**: Centralizes all specs in one place, makes them discoverable, ensures nothing is lost, and allows easy tracing of feature development.

---

## Rule 3: Always Append to audit.md, Never Overwrite

The audit trail is a log of all work done on a feature. It must grow continuously and never be reset or overwritten.

**Format**:
```markdown
## [ISO 8601 Timestamp] — [Phase] — [Agent Name] — [Action]

Details:
- Status: [In Progress / Complete / Blocked]
- Task: [task ID if applicable]
- Notes: [What was accomplished or blocked?]
- Next: [What should happen next?]
```

**Example**:
```markdown
## 2026-03-30T14:23:45Z — INCEPTION — Claude Sonnet 3.5 — Requirements Gathering

Details:
- Status: Complete
- Task: requirements-draft-001
- Notes: Gathered user requirements from product spec. Identified 5 acceptance criteria.
- Next: Design phase, then request human approval

## 2026-03-30T15:45:12Z — INCEPTION — Claude Sonnet 3.5 — Architecture Design

Details:
- Status: Complete
- Task: design-draft-001
- Notes: Designed OAuth2 integration, JWT token storage, session management flow
- Next: Request INCEPTION phase approval before moving to CONSTRUCTION
```

**Action**:
```
when logging work:
  entry = "[timestamp] — [phase] — [agent name] — [action]"
  append(audit.md, entry)
  append(audit.md, details)
  # Never touch existing entries
```

**Why**: Maintains a complete audit trail. Required for traceability, debugging, and compliance. Allows resuming work without losing context.

---

## Rule 4: Always Preserve aidlc-state.md When Resuming

The `aidlc-state.md` file tracks the current phase and progress of a feature. When resuming work, read it first and update it only with current timestamp and status changes.

**Format**:
```yaml
feature: user-auth-oauth2
phase: INCEPTION
phase_start_timestamp: 2026-03-30T14:00:00Z
last_update: 2026-03-30T14:45:12Z
completion_percentage: 45
current_agent: Claude Sonnet 3.5
approvals:
  - inception_approved: false
    requested_at: null
    approved_by: null
    approved_at: null
    feedback: null
next_action: Request human approval for INCEPTION phase
```

**Action**:
```
when resuming work:
  1. Read aidlc-state.md
  2. Note the current phase
  3. Read audit.md to see what was done
  4. Continue from where work left off
  5. Update last_update timestamp
  6. Update completion_percentage
  7. Never reset phase or approvals
```

**Why**: State is the source of truth for resuming work. Preserving it ensures continuity across sessions and prevents losing work.

---

## Rule 4a: Task Status Lifecycle in aidlc-state.md

When using parallel execution (multiple agents working simultaneously on a feature), each task in `aidlc-state.md` follows a defined status lifecycle. Every agent must read and respect these statuses before claiming or starting any task.

**Task status lifecycle**:
```
Not Started → Available → Claimed → In Progress → Done
                                          ↓
                                    (on failure) → Failed
```

Tasks with unmet dependencies are held in `Blocked` until all their dependencies reach `Done`, at which point they are promoted to `Available`.

**Status definitions**:

| Status | Meaning | Who Sets It |
|--------|---------|-------------|
| `Not Started` | Task created but dependencies not yet evaluated | Agent during spec creation |
| `Available` | All dependencies met — ready to be claimed | Agent when promoting unblocked tasks |
| `Claimed` | An agent has reserved this task and will begin immediately | Agent before starting work |
| `In Progress` | Active implementation underway | Agent after claiming |
| `Done` | Task complete, PR raised, ADO updated | Agent on completion |
| `Failed` | Task could not be completed — needs review | Agent on failure |
| `Blocked` | One or more dependencies not yet `Done` | Agent during spec creation or when a blocker is discovered |

**Rules**:
- An agent **must claim a task before writing any code** — write `Status: Claimed | Agent: [ID]` to `aidlc-state.md` first
- If two agents claim the same task simultaneously, the agent whose claim was written **last** wins; the other re-reads and claims a different `Available` task
- When a task reaches `Done`, the agent must evaluate all downstream tasks — any whose dependencies are now all `Done` are promoted from `Blocked` to `Available`
- A `Failed` task must not have its downstream tasks promoted until the failure is resolved

**Note on audit.md vs aidlc-state.md statuses**: `audit.md` entries use a separate set of values — `In Progress / Complete / Blocked` — to describe the outcome of a work session or action. These are distinct from the task statuses above, which track the lifecycle of individual tasks in the dependency graph.

---

## Rule 4b: Branch Protocol for Parallel Execution

Parallel execution uses two branch types. Every agent and developer must follow this rule without exception.

**Branch structure**:
```
main                                              ← protected
 └── feature/ADO-[epic-id]-[feature-name]        ← INTEGRATION BRANCH
      │   aidlc-state.md  ← coordination file — updated here only
      │   audit.md        ← append-only log — updated here only
      │   tasks.md        ← read-only during execution
      │
      ├── task/ADO-[id]-[task-name]               ← TASK BRANCH (code only)
      ├── task/ADO-[id]-[task-name]
      └── task/ADO-[id]-[task-name]
```

**Rule**: `aidlc-state.md` and `audit.md` are **only ever updated on the integration branch**. Implementation code is **only ever committed to a task branch**. Never mix the two.

**Epic and Integration Branch Creation**: The Epic and integration branch are created during spec creation (not during ticket creation). When the user requests "Create AIDLC spec for [feature-name]", the agent must:
1. Create Epic in ADO with the feature name (to obtain Epic ID)
2. Create integration branch with Epic ID in name: `feature/ADO-[epic-id]-[feature-name]`
3. Generate all spec files (requirements.md, design.md, tasks.md, aidlc-state.md, audit.md)
4. Commit spec files to integration branch (NOT to main)
5. Push integration branch to origin

This ensures feature-specific content never touches main branch and the Epic ID is available for proper branch naming from the start.

There are two execution modes. Choose based on how agents are triggered.

---

**Mode A — Guided (L1/L2)**: Agents are started manually or developers work through tasks. `aidlc-state.md` is the coordination file — read it to find what to work on. Repeat until no Available tasks remain.

```
--- CLAIM (on integration branch) ---
1. Checkout integration branch and pull latest
2. Read aidlc-state.md — find highest priority task with Status: Available
3. Write "Status: Claimed | Agent: [id]" to aidlc-state.md
4. Append claim entry to audit.md
5. Commit and push aidlc-state.md + audit.md to integration branch immediately
6. Update ADO task to "In Progress" via ADO MCP

--- IMPLEMENT (on task branch) ---
7. Create task branch: git checkout -b task/ADO-[id]-[task-name] feature/ADO-[epic-id]-[feature-name]
8. Implement per tasks.md and design.md
9. Write tests per .ai/rules/testing-requirements.md
10. Commit and push task branch

--- COMPLETE (on integration branch) ---
11. Checkout integration branch and pull latest
12. Set task Status: Done in aidlc-state.md
13. Promote newly unblocked tasks from Blocked → Available in aidlc-state.md
14. Append completion entry to audit.md
15. Commit and push aidlc-state.md + audit.md to integration branch
16. Update ADO task to Done via ADO MCP
17. If this is the LAST task (all tasks Done): update `.ai/memory/` files to reflect any new architecture, conventions, or key decisions introduced by this feature
18. Raise PR: task branch → integration branch
19. Return to CLAIM
```

Conflict resolution: if two agents claim the same task simultaneously, the claim written **last** wins. The other agent pulls latest and claims a different Available task.

For human developers: "claim" means writing to `aidlc-state.md` on the integration branch AND assigning the ADO task to yourself. Keep both in sync.

---

**Mode B — Autonomous (L3)**: Agents are assigned individual ADO tasks directly (e.g. Copilot Coding Agent triggered by issue assignment). ADO is the coordination layer. Do NOT read `aidlc-state.md` to decide what to work on — update it as an audit record only after completing the assigned task. Run once per assigned task.

```
1. Read assigned ADO task for requirements
2. Read .aidlc/specs/[feature-name]/tasks.md — find task by ADO ID
3. Read design.md and requirements.md for context
4. Create task branch: git checkout -b task/ADO-[id]-[task-name] feature/ADO-[epic-id]-[feature-name]
5. Implement per tasks.md and design.md
6. Write tests per .ai/rules/testing-requirements.md
7. Commit and push task branch
8. Checkout integration branch and pull latest
9. Set task Status: Done in aidlc-state.md
10. Promote newly unblocked tasks from Blocked → Available in aidlc-state.md
11. Append completion entry to audit.md
12. Commit and push aidlc-state.md + audit.md to integration branch
13. Update ADO task to Done via ADO MCP
14. If this is the LAST task (all tasks Done): update `.ai/memory/` files to reflect any new architecture, conventions, or key decisions introduced by this feature
15. Raise PR: task branch → integration branch
```

---

## Rule 5: Three Phase Gates - Approval Required Between Phases

Work progresses through three gates. At each gate, the agent must request human approval before proceeding to the next phase.

### Gate 1: INCEPTION → CONSTRUCTION

**What must be complete**:
- [ ] requirements.md with clear acceptance criteria
- [ ] design.md with architecture and implementation plan
- [ ] tasks.md with all implementation tasks
- [ ] All tasks trace to at least one requirement
- [ ] Security and architecture review complete

**Agent action**:
```
When INCEPTION is complete:
1. Update aidlc-state.md:
   phase: INCEPTION
   completion_percentage: 100
   approvals.inception_requested_at: [now]
2. Append to audit.md: "[timestamp] — INCEPTION — [agent] — Requesting human approval"
3. Request human approval with:
   - Summary of what was designed
   - Link to .aidlc/specs/{feature-name}/
   - Specific questions requiring human input (if any)
4. Wait for approval before proceeding
```

**Human action**:
- Review specs
- Provide feedback or approval
- Update aidlc-state.md with approval details

### Gate 2: CONSTRUCTION → OPERATIONS

**What must be complete**:
- [ ] All tasks from tasks.md implemented
- [ ] Test coverage >= 80%
- [ ] Security review passed
- [ ] Code quality review passed
- [ ] Performance review passed
- [ ] test-summary.md and code-review-findings.md complete
- [ ] `.ai/memory/` files updated to reflect any new architectural decisions, conventions, or patterns introduced by this feature

**Agent action**:
```
When CONSTRUCTION is complete:
1. Update aidlc-state.md:
   phase: CONSTRUCTION
   completion_percentage: 100
   approvals.construction_requested_at: [now]
2. Append to audit.md: "[timestamp] — CONSTRUCTION — [agent] — Requesting human approval"
3. Request human approval with:
   - Test coverage report
   - Security findings and fixes
   - Code quality summary
   - Performance test results
4. Wait for approval before proceeding
```

**Human action**:
- Review test results and findings
- Provide feedback or approval
- Update aidlc-state.md with approval details

### Gate 3: OPERATIONS → COMPLETE

**What must be complete**:
- [ ] Deployment runbook prepared
- [ ] Pre-deployment checks passed
- [ ] Monitoring configured
- [ ] Alert thresholds set
- [ ] Incident response procedures documented

**Agent action**:
```
When OPERATIONS is ready:
1. Update aidlc-state.md:
   phase: OPERATIONS
   completion_percentage: 100
   approvals.operations_requested_at: [now]
2. Append to audit.md: "[timestamp] — OPERATIONS — [agent] — Requesting approval to deploy"
3. Request human approval with:
   - Deployment plan
   - Monitoring dashboard links
   - Incident runbook
   - Rollback procedure
4. Wait for approval before deploying
```

**Human action**:
- Review deployment plan
- Provide feedback or approval
- Update aidlc-state.md with approval details

---

## Rule 6: Traceability - Link Tasks to Requirements and Tests to Acceptance Criteria

Every task in tasks.md must link to at least one requirement in requirements.md. Every test must link to at least one acceptance criterion.

**In tasks.md**:
```markdown
## Task-001: Implement OAuth2 Authorization

Requirement: REQ-001 (User must be able to authenticate with Google)
Acceptance Criteria:
  - [ ] User can click "Login with Google"
  - [ ] User is redirected to Google consent screen
  - [ ] User is logged in after consent

Subtasks:
- [ ] Add Google OAuth configuration
- [ ] Implement authorization code flow
- [ ] Store refresh token securely
```

**In test files**:
```javascript
describe('OAuth2 Authentication', () => {
  // Links to REQ-001, Acceptance Criterion 1
  it('user can click Login with Google button', () => {
    // test code
  });

  // Links to REQ-001, Acceptance Criterion 2
  it('redirects to Google consent screen', () => {
    // test code
  });

  // Links to REQ-001, Acceptance Criterion 3
  it('user is logged in after consent', () => {
    // test code
  });
});
```

**In test-summary.md**:
```markdown
## Test Coverage by Requirement

| Requirement | Tests | Coverage |
|-------------|-------|----------|
| REQ-001 | oauth2-auth.test.js (3 tests) | 100% |
| REQ-002 | oauth2-token.test.js (5 tests) | 100% |
| REQ-003 | oauth2-revoke.test.js (2 tests) | 100% |
```

**Action**:
```
In INCEPTION:
  for each requirement:
    define acceptance criteria
    link each criterion to tasks that implement it

In CONSTRUCTION:
  for each task:
    write tests for each acceptance criterion
    document which test covers which criterion

In summary:
  create traceability matrix showing
  requirement → task → test coverage
```

**Why**: Traceability ensures:
- No requirements are forgotten
- Every test has a clear purpose
- Easy to verify that requirements are met
- Compliance auditors can trace requirements to code

---

## Rule 7: Timestamp Format - Always Use ISO 8601

All timestamps must be in ISO 8601 format with timezone offset.

**Format**: `YYYY-MM-DDTHH:mm:ssZ` (UTC/Zulu) or `YYYY-MM-DDTHH:mm:ss±HH:mm` (with offset)

**Examples**:
```
2026-03-30T14:23:45Z          (UTC)
2026-03-30T14:23:45-08:00     (Pacific time, March)
2026-03-30T14:23:45+05:30     (India time)
```

**Action**:
```
when logging anything with a timestamp:
  use new Date().toISOString()  // JavaScript
  or datetime.now().isoformat() // Python
  or similar in your language
```

**Why**: ISO 8601 is:
- Unambiguous (no "3/4 or 4/3" confusion)
- Sortable (lexicographic sort works)
- Machine-readable
- Compatible with databases and logs

---

## Rule 8: Agent Self-Identification

When appending to audit.md, the agent must identify itself. Include:
- Model name (e.g., "Claude Sonnet 3.5")
- Session ID if available
- Any relevant context

**Example**:
```markdown
## 2026-03-30T14:23:45Z — INCEPTION — Claude Sonnet 3.5 (session-abc-123) — Requirements Gathering
```

**Why**: Allows human reviewers to understand the agent's capabilities and reasoning. Useful for debugging if results are unsatisfactory.

---

## Rule 9: Context Preservation - Read .ai/ and Phase-Specific Rules

Before working on a feature, the agent must read:
1. `.ai/memory/` files (architecture, conventions, decisions)
2. `.ai/workflows/` for the current task type
3. `.ai/rules/` for applicable standards
4. Phase-specific rules in `.aidlc-rule-details/{phase}/`

**Action**:
```
At phase start:
1. Read .ai/memory/architecture.md
2. Read .ai/memory/conventions.md
3. Read .ai/workflows/{task-type}.md
4. Read .aidlc-rule-details/{phase}/*.md
5. Read existing spec files in .aidlc/specs/{feature-name}/
6. Proceed with phase work, applying all guidelines
```

**Why**: Ensures consistency with project standards and doesn't reinvent the wheel for decisions already made.

---

## Rule 10: Blocking Issues - Document and Request Help

If the agent encounters a blocker (missing dependency, unclear requirement, security concern), it must:
1. Document in audit.md with Status: "Blocked"
2. Clearly state what's blocking progress
3. Request human help with specific questions

**Example**:
```markdown
## 2026-03-30T15:00:00Z — INCEPTION — Claude Sonnet 3.5 — Blocked on Database Design

Details:
- Status: Blocked
- Task: design-database-schema
- Blocker: Unclear how to handle user data retention policy. Requirements mention GDPR compliance but don't specify retention period.
- Next: Need clarification on:
  1. How long should user data be retained after account deletion?
  2. Should we implement soft deletes or hard deletes?
  3. Any specific GDPR retention requirements from legal team?
```

**Why**: Prevents work from stalling silently. Clear blockers can be resolved quickly by humans.

---

## Rule 11: Memory Maintenance — Update .ai/memory/ After Feature Completion

The `.ai/memory/` files capture the living state of the project's architecture, conventions, and key decisions. They must be updated whenever a feature introduces changes to any of these.

**Trigger**: When the last task in a feature reaches `Done` (before raising the final PR to the integration branch).

**What to update**:

| File | Update when... |
|------|----------------|
| `.ai/memory/architecture.md` | New components added, integrations changed, data flows updated, deployment topology changed |
| `.ai/memory/conventions.md` | New naming patterns introduced, new file/folder structures, error handling conventions changed |
| `.ai/memory/decisions.md` | Key technical decisions made (library choices, architectural trade-offs, security approaches) |

**Action**:
```
When last task is Done:
1. Review what changed during the feature (diff, audit.md, design.md)
2. Update .ai/memory/architecture.md if system structure changed
3. Update .ai/memory/conventions.md if new patterns were established
4. Update .ai/memory/decisions.md if key decisions were made
5. Commit updated memory files to the integration branch
6. Note the update in audit.md
```

**Why**: `.ai/memory/` files are how future agents (and human developers) understand the current state of the system. If they fall out of sync with the codebase, agents will build on stale context and produce inconsistent output. Updating them at feature completion — not after merge to main — keeps them accurate and avoids the need for manual catch-up sessions.

---

## Rule 13: Architecture Memory Verification — Validate .ai/memory/ Against Actual Codebase

The `.ai/memory/` files (especially `architecture.md` and `api-documentation.md`) are only useful if they reflect reality. If they were created from partial context, outdated assumptions, or without inspecting the actual repositories, they will mislead every agent that reads them.

**Trigger**: At INCEPTION start for any brownfield feature, **before** generating requirements, design, or tasks.

**What to verify**:

| File | Key claims to verify against actual code |
|------|-------------------------------------------|
| `architecture.md` | How does the Portal actually call business logic? What repos are involved? Is the API layer WCF, REST, direct DLL? |
| `api-documentation.md` | Is the REST API hosted in this repo or a separate repo? What pattern does it use (CQRS, MVC, RPC)? |
| `conventions.md` | Do the naming conventions match what's actually in the codebase? |

**Action**:
```
At INCEPTION start (brownfield, .ai/memory/ exists):
1. Read .ai/memory/architecture.md — note any claims about how layers communicate
2. For each major claim, verify against actual code:
   a. If claim says "Portal calls WCF" → find an actual Portal .aspx.vb file and check what it calls
   b. If claim says "REST API is external" → check if a REST API repo/folder exists locally
   c. If claim says "b* libs are called directly" → find an actual .aspx.vb and confirm
3. If any claim is wrong or incomplete:
   a. Inspect actual code to determine correct pattern
   b. Update .ai/memory/architecture.md and api-documentation.md with correct information
   c. Note the correction in audit.md with what was wrong and what the codebase actually shows
4. Only proceed to requirements/design/tasks after memory files reflect reality
```

**Correction format for audit.md**:
```markdown
## [timestamp] — INCEPTION — [agent] — Architecture Memory Correction

Details:
- Status: Complete
- Claim in architecture.md: "Portal calls WCF/SAM via NexusProvider"
- Verified against: Web Portal/Nexus/Pure.Portals/PremiumFinance/PremiumFinancePlan.aspx.vb
- Actual pattern found: Portal calls REST API (PureInsurance.REST repo) via SSP.PureInsuranceRestAPIHandler
- Files updated: .ai/memory/architecture.md, .ai/memory/api-documentation.md
- Impact on spec: Tasks.md revised — removed WCF layer tasks, added CQRS REST API tasks
```

**Why this matters**: Architecture.md for Pure Insurance was previously created from partial analysis — it described the WCF/NexusProvider pattern (which exists but is legacy) as the current Portal-to-backend path. This caused tasks.md to be written against the wrong architecture. Verifying early prevents entire task breakdowns from being built on incorrect foundations.

---

## Rule 12: ADO Pre-flight Check and Branch Safety Guard

### Pre-flight Check — Required Before Spec Creation

Creating the Epic in ADO is the **mandatory first action** when a user requests spec creation. The integration branch name (`feature/ADO-[epic-id]-[feature-name]`) requires the Epic ID. Without it, branches cannot be correctly named and the entire workflow is compromised.

**Before generating any spec files or creating any branches:**

```
When asked to "Create AIDLC spec for [feature-name]":
1. Read .aidlc/config.json — confirm organisation, project, workItemTypes
2. Attempt to create Epic in ADO via ADO MCP
3. If ADO MCP is unavailable or Epic creation fails:
   → STOP immediately
   → Do NOT create any spec files
   → Do NOT create any branches
   → Tell the user: "ADO MCP is unavailable. The Epic cannot be created,
     so no integration branch or spec files have been created. Please
     ensure the ADO MCP server is running and configured, then retry."
   → Wait for the user to resolve the issue before retrying
4. Only proceed with spec generation after Epic ID is successfully obtained
```

**Why**: The Epic ID is structural — it names the integration branch. Proceeding without it causes agents to default to whatever branch they are currently on (often `main`), bypassing all branch discipline.

---

### Branch Safety Guard — Required Before Implementing Code

Before writing any implementation code (Mode A IMPLEMENT step 7, Mode B step 4), the agent must verify it is on a correctly named task branch.

```
Before writing any code:
1. Run: git branch --show-current
2. If branch does NOT match "task/ADO-*":
   → STOP immediately
   → Do NOT write any code
   → Tell the user: "Current branch is '[branch-name]'. Implementation
     code must only be written on a task branch (task/ADO-[id]-[name]).
     Spec creation must complete successfully and task branch must be
     created before implementation begins."
   → Do nothing else until on a correct task branch
3. If branch matches "task/ADO-[id]-[name]":
   → Proceed with implementation
```

**Why**: If the pre-flight check is somehow bypassed (e.g. spec files existed from a previous session), this guard prevents code from landing on `main` or the integration branch by catching the wrong branch at the last moment before any code is written.

---



| Rule | Core Principle |
|------|----------------|
| 1 | Read config.json first |
| 2 | Store specs in .aidlc/specs/{feature-name}/ |
| 3 | Append to audit.md, never overwrite |
| 4 | Preserve aidlc-state.md, read it when resuming |
| 4a | Task status lifecycle: Not Started → Available → Claimed → In Progress → Done / Failed |
| 4b | Branch protocol: state on integration branch, code on task branch — Mode A (guided/claim) or Mode B (autonomous/ADO-assigned) |
| 5 | Request human approval at phase gates |
| 6 | Trace requirements → tasks → tests |
| 7 | Use ISO 8601 timestamps |
| 8 | Identify the agent in audit logs |
| 9 | Read .ai/ and phase-specific rules |
| 10 | Document blockers and request help |
| 11 | Update .ai/memory/ files when the last task in a feature is Done |
| 12 | ADO pre-flight check before spec creation; branch safety guard before implementation |
| 13 | Verify .ai/memory/ claims against actual codebase at INCEPTION start — correct before generating specs |

---

## Checklist for Every Work Session

Before starting work:
- [ ] Read .aidlc/config.json
- [ ] Identify feature name and current phase
- [ ] **If creating a new spec: verify ADO MCP is available and create Epic FIRST — do not create files or branches if ADO MCP is unavailable (Rule 12)**
- [ ] **Brownfield: verify .ai/memory/architecture.md claims against actual code before generating any spec artefacts — correct if wrong (Rule 13)**
- [ ] Read .aidlc/specs/{feature-name}/aidlc-state.md
- [ ] Read .aidlc/specs/{feature-name}/audit.md to understand history
- [ ] Read .ai/memory/ files (architecture, conventions, decisions)
- [ ] Read .ai/workflows/{task-type}.md if applicable
- [ ] Read phase-specific rules in .aidlc-rule-details/{phase}/
- [ ] Confirm what work needs to be done
- [ ] **Before implementing: verify current branch matches `task/ADO-*` — STOP if on main or any other branch (Rule 12)**
- [ ] **Identify which repo(s) the change belongs to before writing any code — PureInsurance (Back Office / Portal / DB) or PureInsurance.REST (API). See `.ai/memory/architecture.md` — Repository Routing section.**

After completing work:
- [ ] Update .aidlc/specs/{feature-name}/aidlc-state.md
- [ ] Append completion entry to audit.md
- [ ] If this is the last task: update .ai/memory/ files (architecture, conventions, decisions) to reflect what changed
- [ ] If phase complete, request human approval
- [ ] If blocked, document blocker and request help

---

## Rule 14: ADO Work Item Attachment Retrieval

The `wit_get_work_item_attachment` tool returns binary files as base64-encoded content. Base64 inflates size by ~33% (e.g. a 342KB `.docx` becomes ~456K characters; a 1.4MB file becomes ~1.9M characters). Thresholds are based on **original file size in bytes**, not base64 output.

**Decision matrix**:

| Original file size | Action |
|--------------------|--------|
| ≤ 75 KB | Retrieve inline via `wit_get_work_item_attachment` (base64 output stays under 100K chars) |
| > 75 KB and ≤ 500 KB | Do NOT retrieve inline. Ask the user to open the attachment manually and paste relevant content, or summarise from the work item description/comments instead |
| > 500 KB | Return metadata only (file name, size, attachment URL). Instruct the user to download locally and provide content if needed |

**How to determine file size**: Read the work item with `expand: "relations"` — attachment relations include the file name and size in the URL/attributes. Check size BEFORE calling the download tool.

**For complex AIDLC features (e.g. RI tax, multi-component specs)**:
- Do NOT block INCEPTION on attachment content. If attachments exceed the inline threshold, proceed with requirements gathering from:
  1. The work item title, description, and comments
  2. Existing code analysis (Rule 13 verification)
  3. Ask the user targeted questions about anything unclear
- Log in `audit.md` which attachments could not be retrieved and what alternative source was used
- Never loop/retry attachment downloads that exceed the threshold — this wastes context and stalls progress

---

## Related Documents

- See `.aidlc-rule-details/inception/` for INCEPTION phase specifics
- See `.aidlc-rule-details/construction/` for CONSTRUCTION phase specifics
- See `.aidlc-rule-details/operations/` for OPERATIONS phase specifics
- See `.ai/` for repo context and conventions
