# AI-SDLC Working Guide: Construction and Delivery

> **When to use this guide:** You have completed INCEPTION (spec, ADO tickets, approval) and are
> ready to build. This guide covers the parallel execution protocol, branch strategy, PR reviews,
> integration testing, and close-out.
>
> For INCEPTION, use one of these guides first:
> - [fast-track-guide.md](./fast-track-guide.md) — simple features, clear requirements
> - [full-lifecycle-guide.md](./full-lifecycle-guide.md) — complex features, interactive requirements discovery
>
> For framework setup, use the setup scripts in `scripts/` or see [SETUP.md](../SETUP.md).

---

## My Feature Details

| Field | Value |
|---|---|
| Feature Name | |
| ADO Epic ID | |
| Integration Branch | `feature/ADO-[epic-id]-[feature-name]` |
| Tech Stack | |
| AI Agent | |

---

## How parallel execution works

Rather than working through tasks one at a time, the goal is for multiple agents to work
simultaneously on independent tasks while respecting dependencies. The mechanism for this is
`aidlc-state.md` — it acts as a shared coordination file that every agent reads before claiming
a task and writes to immediately after claiming it.

```
tasks.md          = the dependency graph and task definitions (read-only during execution)
aidlc-state.md    = live execution state — what is available, claimed, in progress, done
audit.md          = append-only log of every action taken by every agent
```

**Task status lifecycle:**

```
Not Started → Available → Claimed (Agent X) → In Progress (Agent X) → Done
                                                      ↓
                                              (on failure) → Failed — needs review
```

An agent's first action on every session is always: read `aidlc-state.md`, find the highest
priority `Available` task, claim it (write `Claimed: Agent [X]`), then start work. This prevents
two agents picking up the same task. When the task is done, the agent resolves dependencies —
any task whose blockers are now all `Done` gets promoted from `Blocked` to `Available`.

---

## Before you start

Complete INCEPTION before using this guide. INCEPTION produces the spec files and ADO tickets
that construction depends on. When inception is done you will have:

- `aidlc-state.md` with all tasks initialised (Available or Blocked)
- Every task in `tasks.md` tagged with an `[ADO#ID]`
- Integration branch `feature/ADO-[epic-id]-[feature-name]` pushed to origin
- INCEPTION approval recorded in `aidlc-state.md`

If you are not there yet, go to one of the INCEPTION guides:

- [fast-track-guide.md](./fast-track-guide.md) — simple features, clear requirements (15 min setup)
- [full-lifecycle-guide.md](./full-lifecycle-guide.md) — complex features, interactive discovery (2-4 hr setup)

---

## Phase 1 — Build with AI Agents (CONSTRUCTION)

### Step 1 — Branch strategy and what lives where

> The integration branch was already created in Step 1 as part of spec generation. This section explains the full structure for reference.

Parallel work requires a clear branch structure and a shared coordination file. Every agent or developer must understand this before starting.

```
main                                              ← protected, never commit here directly
 └── feature/ADO-[epic-id]-[feature-name]        ← INTEGRATION BRANCH (created in Step 1)
      │   Lives here: aidlc-state.md  ← shared coordination file
      │               audit.md        ← shared append-only log
      │               tasks.md        ← read-only during execution
      │
      ├── task/ADO-[id]-[task-name]               ← TASK BRANCH
      │    Created by: agent/developer when claiming a task
      │    Lives here: implementation code and tests only
      │
      ├── task/ADO-[id]-[task-name]
      └── task/ADO-[id]-[task-name]
```

**Key rule — where state updates happen:**

`aidlc-state.md` and `audit.md` are coordination files. They must always be updated on the **feature integration branch**, not on task branches. This ensures every agent and developer is reading the same live state.

| Action | Which branch |
|---|---|
| Read current task state | Feature integration branch |
| Claim a task (write `Status: Claimed`) | Feature integration branch |
| Update task to `In Progress` | Feature integration branch |
| Append to `audit.md` | Feature integration branch |
| Write implementation code | Task branch |
| Write tests | Task branch |
| Mark task `Done`, promote unblocked tasks | Feature integration branch |

- [ ] Move the ADO Epic to **"In Progress"**

_Notes:_

---

### Step 2 — Choose your execution mode

There are two modes of execution. Choose based on your maturity level and tooling.

---

#### Mode A — Guided execution (L1/L2)

Use this when agents are started manually per session, or when developers are working through tasks. `aidlc-state.md` is the coordination file — each participant reads it to decide what to work on.

**For AI agents** — give each agent session this prompt:

```
You are an autonomous agent working on feature: [feature-name]
Your agent ID is: Agent-[A/B/C/D — assign a unique letter per session]

Spec location: .aidlc/specs/[feature-name]/
Feature integration branch: feature/ADO-[epic-id]-[feature-name]

Branch rules:
- aidlc-state.md and audit.md are ONLY ever updated on the feature integration branch
- Implementation code is ONLY ever committed to your task branch
- Always pull the latest integration branch before reading or writing aidlc-state.md

Repeat until no Available tasks remain:

--- CLAIM (on integration branch) ---
1. Checkout feature/ADO-[epic-id]-[feature-name] and pull latest
2. Read aidlc-state.md — find the highest priority task with Status: Available
3. Write "Status: Claimed | Agent: Agent-[X]" to aidlc-state.md
4. Append claim entry to audit.md
5. Commit and push both files to the integration branch immediately
6. Update ADO task to "In Progress" via ADO MCP

--- IMPLEMENT (on task branch) ---
7. Create task branch: git checkout -b task/ADO-[id]-[task-name] feature/ADO-[epic-id]-[feature-name]
8. Implement the task per tasks.md and design.md
9. Write tests per .ai/rules/testing-requirements.md
10. Commit and push the task branch

--- COMPLETE (on integration branch) ---
11. Checkout feature/ADO-[epic-id]-[feature-name] and pull latest
12. Mark task "Status: Done" in aidlc-state.md
13. Promote any newly unblocked tasks from Blocked → Available in aidlc-state.md
14. Append completion entry to audit.md
15. Commit and push both files to the integration branch
16. Update ADO task to Done via ADO MCP
17. If this is the LAST task (all tasks Done): update .ai/memory/ files to reflect any new architecture, conventions, or key decisions introduced by this feature; commit to integration branch
18. Raise a PR from task/ADO-[id]-[task-name] → feature/ADO-[epic-id]-[feature-name]
19. Return to CLAIM
```

**For human developers** — same protocol:
- Claim means: write `Status: Claimed | Developer: [your name]` to `aidlc-state.md` on the integration branch **and** assign the ADO task to yourself
- ADO assignment is your source of truth — keep `aidlc-state.md` in sync with it

> **How many to start:** Count tasks with `Status: Available` at the start — that is the maximum useful parallelism.

---

#### Mode B — Autonomous execution (L3)

Use this when agents are assigned work directly via ADO work items or GitHub Issues — for example GitHub Copilot Coding Agent. In this mode, **ADO is the coordination layer**. `aidlc-state.md` is not used to decide what to work on — it is updated as an audit record only after the agent completes its assigned task.

> The race condition risk of Mode A (two agents reading the same Available task simultaneously) does not exist here because each agent already knows its task from the ADO assignment.

**Each agent is triggered by being assigned an ADO task. It runs this protocol once:**

```
You have been assigned ADO task: [ADO#ID] — [task name]
Feature: [feature-name]

Spec location: .aidlc/specs/[feature-name]/
Feature integration branch: feature/ADO-[epic-id]-[feature-name]

1. Read your assigned ADO task for full requirements
2. Read .aidlc/specs/[feature-name]/tasks.md — find your task by ADO ID
3. Read design.md and requirements.md for context
4. Create task branch: git checkout -b task/ADO-[id]-[task-name] feature/ADO-[epic-id]-[feature-name]
5. Implement the task per tasks.md and design.md
6. Write tests per .ai/rules/testing-requirements.md
7. Commit and push the task branch
8. Checkout the integration branch and pull latest
9. Mark your task "Status: Done" in aidlc-state.md
10. Promote any newly unblocked tasks from Blocked → Available in aidlc-state.md
11. Append completion entry to audit.md
12. Commit and push aidlc-state.md and audit.md to the integration branch
13. Update ADO task to Done via ADO MCP
14. If this is the LAST task (all tasks Done): update .ai/memory/ files to reflect any new architecture, conventions, or key decisions introduced by this feature; commit to integration branch
15. Raise a PR from task/ADO-[id]-[task-name] → feature/ADO-[epic-id]-[feature-name]
```

> **Note:** In Mode B, `aidlc-state.md` serves as an audit trail and dependency tracker, not a coordination file. Human review of `aidlc-state.md` still shows overall feature progress and unblocked tasks, but agents do not read it to decide what to work on.

> **Infrastructure requirement:** Mode B requires Tier 3 (GitHub Platform Layer) — specifically the GitHub Actions workflows and Copilot agent definitions that trigger agents from ADO task assignments. If Tier 3 is not set up yet, use Mode A. See [`guides/setup/tier-3-setup.md`](../guides/setup/tier-3-setup.md).

---

- [ ] Execution mode chosen: **Mode A — Guided** / **Mode B — Autonomous**

- [ ] Agents / developers assigned

_Notes:_

---

### Step 3 — Monitor execution state

Check progress at any time with:

```
Read .aidlc/specs/[feature-name]/aidlc-state.md and audit.md.
Summarise:
- How many tasks are Done / In Progress / Available / Blocked
- Which agents are active and what they are working on
- Which tasks are on the critical path and their current status
- Any tasks that have been In Progress for more than [X] hours (may be stuck)
- Which tasks became newly Available since the last check
```

**Live task board** (update as agents complete tasks):

| Task | ADO# | Depends on | Status | Agent | Branch |
|---|---|---|---|---|---|
| Task 1 | | none | ⬜ Available | | |
| Task 2 | | Task 1 | 🔒 Blocked | | |
| Task 3 | | Task 1 | 🔒 Blocked | | |
| Task 4 | | none | ⬜ Available | | |
| Task 5 | | Task 2, Task 3 | 🔒 Blocked | | |

_Status key: ⬜ Available · 🔒 Blocked · 🏷️ Claimed · 🔄 In Progress · ✅ Done · ❌ Failed_

_Notes:_

---

### Step 4 — Merge task branches into the feature branch

As each task branch is completed and reviewed:

- [ ] Task PR reviewed (AI pre-review first — see prompt below)

- [ ] Task branch merged into `feature/ADO-[epic-id]-[feature-name]`

- [ ] Downstream dependent tasks promoted to Available in `aidlc-state.md` (agents handle this automatically, but verify)

**AI pre-review prompt for each task PR:**

```
Review the changes on branch task/ADO-[id]-[task-name] against:
- Task definition:  .aidlc/specs/[feature-name]/tasks.md (Task N)
- Design:           .aidlc/specs/[feature-name]/design.md
- Requirements:     .aidlc/specs/[feature-name]/requirements.md
- Coding standards: .ai/rules/coding-standards.md
- Security rules:   .ai/rules/security-requirements.md

Flag: gaps vs the task definition, design deviations, missing tests, security issues.
Also confirm this task's completion correctly unblocks its downstream tasks.
```

_Notes:_

---

### Step 5 — Integration testing on the feature branch

Once all task branches are merged into the feature integration branch:

- [ ] Run integration tests across the full feature:

```
Generate integration tests for [feature-name] that verify:
- All acceptance criteria in .aidlc/specs/[feature-name]/requirements.md are met
- Tasks that depend on each other integrate correctly
- End-to-end flows work as specified in design.md
- Edge cases and error paths are covered

Base tests on the combined implementation in feature/ADO-[epic-id]-[feature-name].
```

- [ ] All tests pass

- [ ] No regressions against existing functionality

_Notes:_

---

## Phase 2 — Code Review and Merge

### Step 6 — Raise the feature PR

- [ ] Create PR: `feature/ADO-[epic-id]-[feature-name]` → `main`

- [ ] PR description includes:
  - Link to spec: `.aidlc/specs/[feature-name]/requirements.md`
  - ADO Epic reference: `AB#[epic-id]`
  - Summary of all tasks completed and any deviations from the original design
  - Integration test results

- [ ] Move ADO User Stories to **"In Review"**

_PR link:_

_Notes:_

---

### Step 7 — Human review and approval

- [ ] Reviewer(s) approve

- [ ] Branch policies satisfied

- [ ] PR merged to main

_Notes:_

---

## Phase 3 — Deploy and Close (OPERATIONS)

### Step 8 — Deploy

- [ ] Pipeline triggered by merge

- [ ] **Dev** — passed

- [ ] **Staging** — passed

- [ ] **Production** — passed

> Generate pipeline YAML if needed:
> ```
> Generate an Azure DevOps pipeline YAML for deploying [service-name].
> Environments: dev, staging, prod.
> Standards: .ai/workflows/feature-development.md
> ```

_Pipeline run link:_

---

### Step 9 — Close and record

- [ ] Update memory files:

```
Update .ai/memory/ files for [feature-name]

Review what changed and update:
- architecture.md if system structure, components, or integrations changed
- conventions.md if new patterns or conventions were established
- decisions.md if key technical decisions were made
```

- [ ] Mark spec complete:

```
Mark spec [feature-name] as COMPLETE in .aidlc/specs/[feature-name]/aidlc-state.md
Deployed: [date]
Append final entry to audit.md including:
- Total tasks completed
- Number of agents used
- Elapsed time vs estimate
- Any deviations from original design
```

- [ ] Close all ADO Tasks, User Stories, and Epic

_Notes:_

---

### Step 10 — Post-release monitoring

- [ ] Monitor for 24-48 hours

- [ ] Bugs → new `bugfix.md` spec in `.aidlc/specs/[bug-name]/` with same dependency-aware structure

---

## Quick Reference: The Full Parallel Loop

```
Spec + dependency graph created in .aidlc/specs/[feature]/
                    ↓
ADO tickets created with dependency links via ADO MCP
ADO IDs written back into tasks.md + aidlc-state.md initialised
                    ↓
Feature integration branch created
                    ↓
Multiple agents launched in parallel
Each agent: reads aidlc-state.md → claims available task → creates task branch
         → implements → tests → commits → updates aidlc-state.md
         → updates ADO → promotes unblocked tasks → claims next task
                    ↓
Task branches merged to feature branch as they complete
Newly unblocked tasks become available automatically
                    ↓
Integration tests on feature branch
                    ↓
Feature PR → human review → merge → pipeline → deploy
                    ↓
Spec marked complete · ADO closed · audit trail complete
```

---

## Dependency conflict resolution

**Two agents claim the same task:**
The agent whose claim was written last to `aidlc-state.md` keeps it. The other agent re-reads and claims a different Available task.

**A task fails mid-execution:**
Mark `Status: Failed | Agent: [X] | Reason: [brief description]` in `aidlc-state.md`. Do not promote downstream tasks. Investigate before re-assigning.

**A dependency turns out to be wrong:**
Update `tasks.md` and `aidlc-state.md` to correct the dependency, log the change to `audit.md`, update the ADO dependency links via ADO MCP, then re-evaluate which tasks are now Available.

**Agent goes silent / appears stuck:**
Run the monitoring prompt. If a task has been `In Progress` for significantly longer than its estimate, inspect `audit.md` for its last log entry and reassign if necessary.

---

## Troubleshooting

**Agent not following the parallel protocol**
→ Confirm the agent instruction file exists and contains the full AIDLC execution protocol (see `guides/setup/agent-configs/` in the ai-dlc repo)

**Two agents built the same thing**
→ Claim step was skipped or too slow. Enforce: claim in `aidlc-state.md` BEFORE writing any code

**Merge conflicts on task branches**
→ Tasks that touch the same files should have explicit dependencies. Update `tasks.md` and re-plan

**aidlc-state.md out of sync with reality**
→ Run: `Reconcile .aidlc/specs/[feature-name]/aidlc-state.md against the actual git branch state and ADO ticket statuses. Update aidlc-state.md to match reality and log the reconciliation to audit.md`
**ADO not reflecting task branch state**
→ Run: `Check all ADO task statuses for feature [feature-name] against aidlc-state.md and bring them into sync via the ADO MCP`

---

## Agent Configuration

Each agent needs its own instruction file in the repo so it knows how to follow the AIDLC protocol.
Templates are in `guides/setup/agent-configs/` in the ai-dlc repo. Copy the relevant template into your repo and customise it.

The instruction file must tell the agent:
1. Where specs live (`.aidlc/specs/`)
2. The claim-before-code protocol (`aidlc-state.md`)
3. How to update ADO via MCP on task completion
4. How to resolve dependency conflicts

See `guides/setup/agent-configs/` in the ai-dlc repo for per-agent templates.

---

_Last updated: 2026-04-29_
_Guide version: 4.0 — construction and delivery reference; INCEPTION covered by fast-track-guide.md and full-lifecycle-guide.md_
