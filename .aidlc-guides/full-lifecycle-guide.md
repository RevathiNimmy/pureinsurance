# AI-SDLC Full Lifecycle Guide

> **When to use this guide:** Complex features, unclear requirements, architectural decisions, cross-team coordination

> **When NOT to use:** Simple features with obvious requirements (use [fast-track-guide.md](./fast-track-guide.md) instead)

---

## Before You Start

✅ Feature needs requirements discovery  
✅ Multiple stakeholders or technical unknowns  
✅ Architectural or security impact  
✅ Time to do it right (2-4 hours for inception phase)

---

## Phase 1: INCEPTION — Discover & Design

### Step 1.0: ADO Pre-flight — Create Epic First (2 minutes)

> **Required before anything else.** The integration branch name includes the Epic ID. No files or branches can be created without it.

```
Read .aidlc/config.json to confirm organisation, project, and workItemTypes.

Create an Epic in ADO for feature: [BRIEF-NAME]

Report the Epic ID before we proceed. If ADO MCP is unavailable or the Epic
cannot be created, stop here and do not create any files or branches until
the issue is resolved.
```

**You confirm:** Epic ID: `ADO#_____`

---

### Step 1.1: Initialize the Spec (5 minutes)

**Start with this prompt:**

```
I want to create a new feature spec using the interactive AIDLC inception phase.

Feature: [BRIEF-NAME]
ADO Epic ID: [epic-id from Step 1.0]
Problem: [WHAT-PROBLEM-DOES-IT-SOLVE]

Before generating anything, guide me through the inception workflow:

1. Read .aidlc-rule-details/inception/ for the structured process
2. Read .ai/memory/architecture.md for system context
3. Read .ai/memory/conventions.md for project patterns

Let's work through requirements gathering interactively.
Don't generate any files yet - we'll do that together.

Start by asking me questions to understand:
- Who are the users?
- What are they trying to accomplish?
- What are the constraints?
- What does success look like?
```

**The agent should now ask you clarifying questions.** Answer them thoughtfully.

---

### Step 1.2: Requirements Discovery (30-60 minutes)

The agent will guide you through gathering requirements. Expect questions like:

**User & Problem Space:**
- Who will use this feature?
- What problem are they trying to solve?
- How do they solve it today?
- What pain points exist?

**Functional Requirements:**
- What must the system do?
- What are the happy path scenarios?
- What are the error/edge cases?
- What data is involved?

**Non-Functional Requirements:**
- Performance expectations? (response time, throughput)
- Security requirements? (auth, data protection)
- Scalability needs? (concurrent users, data volume)
- Compliance constraints? (GDPR, HIPAA, SOC2)

**Integration Points:**
- What systems does this interact with?
- What APIs/databases are involved?
- Any external dependencies?

**Success Criteria:**
- How do we know it works?
- What metrics matter?
- What would make you confident to ship this?

---

### Step 1.3: Create User Stories (20 minutes)

**After requirements discussion, prompt:**

```
Based on what we discussed, create user stories with acceptance criteria.

Format each story as:
---
User Story: [TITLE]

As a [USER-TYPE]
I want to [ACTION]
So that [BENEFIT]

Acceptance Criteria:
- [ ] [TESTABLE-CRITERION-1]
- [ ] [TESTABLE-CRITERION-2]
- [ ] [TESTABLE-CRITERION-3]

Priority: [High/Medium/Low]
Estimate: [Story points or hours]
---

Don't generate files yet - show me the stories for review.
```

**Review the stories:**
- [ ] Each story is independently valuable
- [ ] Acceptance criteria are testable
- [ ] Stories are sized appropriately (< 3 days)
- [ ] No missing scenarios

**If adjustments needed:**
```
Update story [N]: [WHAT-TO-CHANGE]
```

---

### Step 1.4: Technical Design Discussion (30-60 minutes)

**Once stories are approved, prompt:**

```
Now let's discuss the technical design.

Based on the requirements and user stories we defined, propose:

1. High-level architecture
   - What components are needed?
   - How do they interact?
   - What's the data flow?

2. Data models
   - What entities exist?
   - What are their relationships?
   - Any schema changes needed?

3. API contracts (if applicable)
   - What endpoints are needed?
   - Request/response formats?
   - Authentication/authorization?

4. Integration approach
   - How do we integrate with [SYSTEM-X]?
   - Any new dependencies?

5. Security considerations
   - What are the security risks?
   - How do we mitigate them?
   - Any sensitive data involved?

6. Testing strategy
   - What needs unit tests?
   - What needs integration tests?
   - Any E2E scenarios?

Don't generate design.md yet - let's discuss these points first.
```

**Discuss each area.** The agent should propose approaches and you refine them.

**Key discussions:**
- Architecture patterns that fit your system
- Reusing existing components vs new code
- Performance implications
- Security controls needed
- Rollback strategy if things go wrong

---

### Step 1.5: Generate Requirements Document (5 minutes)

**When design discussion is complete:**

```
Generate requirements.md at .aidlc/specs/[FEATURE-NAME]/requirements.md

Include:
- Feature overview and problem statement
- All user stories with acceptance criteria (from our discussion)
- Functional requirements (what it must do)
- Non-functional requirements (performance, security, scalability)
- Integration requirements (systems, APIs, dependencies)
- Success metrics (how we measure success)
- Constraints and assumptions

Make it production-ready based on everything we discussed.
```

- [ ] Review `requirements.md` for completeness

---

### Step 1.6: Generate Technical Design (5 minutes)

```
Generate design.md at .aidlc/specs/[FEATURE-NAME]/design.md

Include:
- Architecture overview (components, data flow, diagrams if helpful)
- Data models and schema changes
- API contracts (endpoints, request/response formats)
- Integration points and dependencies
- Security design (auth, data protection, threat mitigations)
- Performance considerations (caching, optimization, scalability)
- Error handling approach
- Testing strategy (unit, integration, E2E)
- Deployment considerations (migrations, config, rollback)
- Open questions or risks (if any remain)

Base it on our technical design discussion.
```

- [ ] Review `design.md` for completeness and accuracy

---

### Step 1.7: Break Down into Tasks (20 minutes)

```
Generate tasks.md at .aidlc/specs/[FEATURE-NAME]/tasks.md

Based on requirements.md and design.md, create granular implementation tasks.

For each task:
- Title and clear description
- Depends on: [task IDs or "none"]
- Blocks: [task IDs or "none"]
- Parallelisable: [yes/no]
- Estimate: [hours - aim for 2-8 hour tasks]

Organize into layers:
1. Foundation (database, models, infrastructure)
2. Core Logic (business logic, services)
3. Integration (APIs, external systems)
4. Testing (unit, integration, E2E)
5. Documentation & Deployment

Show me the task list with dependency graph before saving.
```

**Review the task breakdown:**
- [ ] Tasks are granular enough (2-8 hours each)
- [ ] Dependencies make sense
- [ ] Foundation tasks come first
- [ ] Testing tasks exist for everything
- [ ] Critical path is reasonable

**If adjustments needed:**
```
Split task [N] into [N.1] and [N.2] because [REASON]
OR
Merge tasks [N] and [M] into a single task because [REASON]
OR
Add dependency: Task [X] depends on Task [Y] because [REASON]
```

---

### Step 1.8: Execution State Files (automatic)

After tasks.md is approved and saved, the agent will automatically generate:

`aidlc-state.md` — mirrors every task from tasks.md, sets tasks with no unmet dependencies to `Status: Available` and tasks with unmet dependencies to `Status: Blocked`, and includes a dependency graph at the top.

`audit.md` — logs spec creation with timestamp, feature name, and the list of files generated during this inception session.

**Verify:**

- [ ] `.aidlc/specs/[FEATURE-NAME]/aidlc-state.md` exists
- [ ] `.aidlc/specs/[FEATURE-NAME]/audit.md` exists
- [ ] Available/Blocked mix in `aidlc-state.md` reflects the dependency graph in tasks.md

**Stop if:** either file is missing after the agent confirms tasks.md is saved. Ask the agent to generate the missing files before continuing to Step 1.9.

---

### Step 1.9: Critical Path Analysis (10 minutes)

```
Analyze .aidlc/specs/[FEATURE-NAME]/tasks.md dependency graph.

Identify:
1. The critical path (longest chain of dependent tasks)
2. Tasks that can run in parallel
3. Bottleneck tasks that block many others
4. Minimum elapsed time with unlimited parallel agents

Show me:
- Critical path: Task X → Task Y → Task Z (Total: N hours)
- Maximum parallelism: M tasks can run simultaneously
- Bottlenecks: Task A blocks [count] tasks
```

**Critical path:** `________________________ (Total: ____ hours)`  
**Max parallelism:** `____ tasks`

---

### Step 1.10: Create Integration Branch (automatic)

After all spec files are approved, the agent will automatically:

- Create branch `feature/ADO-[epic-id]-[feature-name]` from main
- Commit all five spec files: requirements.md, design.md, tasks.md, aidlc-state.md, audit.md
- Push the branch to origin
- Report the branch name

**Verify:**

- [ ] Branch `feature/ADO-[epic-id]-[feature-name]` exists in origin
- [ ] All five spec files are present on the branch

**Stop if:** the branch is not visible in origin after the agent reports completion. Confirm the Epic ID from Step 1.0 was used in the branch name and that git credentials allow pushing. Resolve before proceeding to Step 1.11.

**Your record:** Branch: `feature/ADO-_____-[feature-name]`

---

### Step 1.11: Request INCEPTION Approval (Phase Gate)

> **Required before moving to CONSTRUCTION.** Do not start any implementation until this approval is given.

```
INCEPTION phase is complete for [feature-name].

Please:
1. Update .aidlc/specs/[feature-name]/aidlc-state.md:
   - phase: INCEPTION
   - completion_percentage: 100
   - approvals.inception_requested_at: [now]
2. Append to audit.md: "[timestamp] — INCEPTION — [agent] — Requesting human approval to proceed to CONSTRUCTION"
3. Summarise:
   - What requirements were gathered
   - Key design decisions made
   - Task count and critical path
   - Any open questions requiring human input
```

- [ ] Human reviews and approves `requirements.md`, `design.md`, `tasks.md`
- [ ] Human updates `aidlc-state.md` with approval details
- [ ] **Only then:** proceed to Phase 2

---

### Inception Phase Complete ✓

**You now have:**
- ✅ `requirements.md` — What to build (based on discovery)
- ✅ `design.md` — How to build it (based on discussion)
- ✅ `tasks.md` — Work breakdown with dependencies
- ✅ `aidlc-state.md` — Execution state tracker
- ✅ `audit.md` — Audit trail
- ✅ **Epic created in ADO** (Step 1.0)
- ✅ **Integration branch created with spec committed**
- ✅ **INCEPTION approval obtained**

**Time invested:** 2-4 hours  
**Value:** Clear, validated spec with stakeholder buy-in

---

## Phase 2: CONSTRUCTION — Build It

### Step 2.1: Create ADO Work Items

```
Create ADO tickets for [feature-name]
```

**The agent will automatically:**
- Create User Stories from requirements.md (under existing Epic)
- Create Tasks from tasks.md with estimates
- Link predecessor/successor dependencies
- Update tasks.md with [ADO#ID] for each task
- Update aidlc-state.md with task IDs
- Commit spec updates to integration branch
- Report when complete

**You verify:** All tasks now have `[ADO#ID]` in tasks.md

---

### Step 2.2: Execute Tasks

```
Work on [feature-name]
```

**The agent will autonomously:**
1. **For each available task (until all done):**
   - Claim task in aidlc-state.md
   - Update ADO task to "In Progress"
   - Create task branch: `task/ADO-[id]-[task-name]`
   - Implement per design.md and tasks.md
   - Write tests per .ai/rules/testing-requirements.md
   - **Run tests locally and verify they pass**
   - **Build and run the application locally**
   - **Verify the changes work as expected**
   - Commit and push code
   - Mark task Done in aidlc-state.md
   - Update ADO task to Done
   - Create PR: task branch → feature branch
   - Promote newly unblocked tasks from Blocked → Available
   - **If last task: update `.ai/memory/` files (architecture, conventions, decisions) to reflect what changed; commit to integration branch**
   - Report completion and move to next task

**You review:** Merge each task PR (human review recommended for complex features)

---

### Step 2.3: Monitor Progress (Optional)

**Check status anytime:**

```
Status of [feature-name]
```

The agent reports:
- Tasks: Done / In Progress / Available / Blocked
- Current work and critical path status
- Newly unblocked tasks

---

### Step 2.4: Code Review (Per Task)

For each task PR:

```
Review PR: task/ADO-[id]-[task-name]

Check against spec and standards. Flag issues.
```

- [ ] PR approved
- [ ] Merged to feature branch
- [ ] Downstream tasks promoted

---

### Step 2.5: Integration Testing

When all tasks are complete:

```
Generate integration tests for [feature-name]

Verify all acceptance criteria are met.
```

- [ ] All tests pass
- [ ] No regressions
- [ ] Performance meets NFRs

---

## Phase 3: OPERATIONS — Ship It

### Step 3.1: Create Feature PR

```
Create PR for feature/ADO-[epic-id]-[feature-name] → main

Include:
- Feature overview and problem statement
- User stories completed
- Testing evidence (unit, integration, manual)
- Design deviations (if any)
- Security review checklist
- Rollback plan
```

- [ ] Get approval
- [ ] Merge to main

---

### Step 3.2: Deploy

```
Deploy [feature-name] to [environment]
```

- [ ] Dev — passed
- [ ] Staging — passed
- [ ] Production — passed

**Monitor deployment:**

```
Check deployment status for [feature-name]
```

---

### Step 3.3: Close Out

```
Close out [feature-name]

Mark spec COMPLETE, close all ADO items, capture lessons learned.
```

- [ ] `.ai/memory/` files updated (architecture, conventions, decisions)
- [ ] All ADO items closed
- [ ] Spec marked complete
- [ ] Lessons learned captured

---

## Full Lifecycle Timeline

| Phase | Time | Notes |
|-------|------|-------|
| **Inception** (Interactive) | 2-4 hours | Requirements discovery + design discussion |
| Setup (ADO tickets) | Agent handles | Automatic |
| **Construction** (Automated) | Varies | Agent executes all tasks autonomously |
| Task PR reviews | 5-10 min per PR | Human review |
| Integration testing | 30-60 min | — |
| **Operations** | Varies | Deploy + monitor |
| Close out | Agent handles | Automatic |

**Inception investment:** 2-4 hours (interactive discovery and design)  
**Construction:** Agent-driven (runs autonomously with human PR review)  
**Value:** Validated requirements, stakeholder alignment, clear technical design

---

## When to Use Full Lifecycle

✅ **Use when:**
- Requirements are unclear or complex
- Multiple stakeholders need alignment
- Architectural decisions needed
- Security or compliance implications
- Cross-team coordination required
- High risk or business impact

❌ **Don't use when:**
- Requirements are obvious
- Feature is simple and isolated
- Team is experienced with similar work
- Time pressure is extreme

→ For simple features, use [fast-track-guide.md](./fast-track-guide.md)

---

## Troubleshooting

**Requirements keep changing during inception:**
→ Normal! Refine them interactively until stable. Don't move to design until requirements feel solid.

**Design discussion reveals missing requirements:**
→ Go back to Step 1.2. Update requirements, then continue design discussion.

**Task breakdown feels wrong:**
→ Refine it in Step 1.7 before moving to construction.

**Agent needs guidance during construction:**
```
Continue [feature-name]

[Provide specific guidance for the current task]
```

**Need to update spec mid-construction:**
```
Update .aidlc/specs/[feature-name]/[file].md to [change]

Adjust affected tasks in tasks.md and aidlc-state.md if needed.
```

---

## What's Automated vs Manual

### ✅ Agent Handles Automatically

**Step 1.0 — pre-flight (separate prompt):**
- Create ADO Epic, report the Epic ID

**Steps 1.5–1.7 — file generation (each triggered by you after discussion):**
- Write requirements.md, design.md, tasks.md based on the interactive discussion

**Steps 1.8 and 1.10 — state and branch (automatic after tasks.md is approved):**
- Generate aidlc-state.md and audit.md
- Create feature integration branch, commit spec files, push to origin

**Step 2.1 — ADO work items (separate prompt):**
- Create User Stories and Tasks under the Epic, link dependencies, write IDs back into tasks.md

**Phase 2 — construction (single prompt per feature):**
- Create task branches per task
- Implement code per spec
- Write tests per standards
- Update ADO task status (In Progress → Done)
- Create PRs (task → feature, feature → main)
- Promote unblocked tasks (Blocked → Available)
- Update audit log
- Close out ADO work items

### 👤 You Lead Interactively
- Inception phase (requirements discovery and design discussion)
- Spec review and approval at each step
- INCEPTION approval gate (Step 1.11)
- Task PR reviews (human judgment recommended)
- Feature PR approval (required)
- Deployment decisions
- Lessons learned capture

---

**Next Steps:**
- See [working-guide.md](./working-guide.md) for detailed execution protocols
- See [fast-track-guide.md](./fast-track-guide.md) for simpler features
