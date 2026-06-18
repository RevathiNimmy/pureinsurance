# AI-SDLC Fast Track Guide

> **When to use this guide:** Simple features, clear requirements, experienced users, tight deadlines

> **When NOT to use:** Complex features, unclear requirements, cross-team dependencies, architectural changes

---

## Before You Start

✅ Requirements are clear and documented  
✅ Feature is small-medium sized (< 10 tasks)  
✅ Technical approach is obvious  
✅ No major architectural decisions needed

---

## What's Automated vs Manual

### ✅ Agent Handles Automatically

**Step 1 — spec generation (single prompt):**
- Create ADO Epic
- Create feature integration branch
- Generate spec files: requirements, design, tasks, aidlc-state, audit
- Commit and push the integration branch

**Step 3 — ADO work items (separate prompt required):**
- Create User Stories under the Epic
- Create Tasks with dependency links
- Write ADO IDs back into tasks.md and aidlc-state.md

**Phase 2 — construction (single prompt per feature):**
- Create task branches per task
- Implement code per spec
- Write tests per standards
- Update ADO task status (In Progress → Done)
- Create PRs (task → feature, feature → main)
- Promote unblocked tasks (Blocked → Available)
- Update audit log

### 👤 You Review and Approve
- Spec quality (requirements, design, tasks)
- INCEPTION approval (Step 3b)
- Task PR merges (human review recommended)
- Feature PR approval (required)
- Deployment triggers

---

## Phase 1: Setup (15 minutes)

### Step 1: Generate Spec (5 min)

```
Create AIDLC spec for [feature-name]

[Brief description of the feature and key requirements]
```

**The agent will automatically:**
- Read .ai/ context (architecture, conventions, standards)
- **Create Epic in ADO** with feature name
- **Create integration branch:** `feature/ADO-[epic-id]-[feature-name]`
- Generate requirements.md (user stories, acceptance criteria, NFRs)
- Generate design.md (technical approach, data models, integration points)
- Generate tasks.md (granular tasks with dependencies and estimates)
- Generate aidlc-state.md (task status tracking: Available/Blocked)
- Generate audit.md (change log)
- **Commit spec files to integration branch**
- Push branch to origin
- Report Epic ID and branch name

---

### Step 2: Review Spec (10 min)

Checkout the integration branch to review:

- [ ] Read `requirements.md` — user stories complete?
- [ ] Read `design.md` — technical approach sound?
- [ ] Read `tasks.md` — task breakdown and dependencies logical?

**If changes needed:**

```
Update .aidlc/specs/[feature-name]/[file].md:
[Describe what needs to change]
```

---

### Step 3: Create ADO Tickets

```
Create ADO tickets for [feature-name]
```

**The agent will automatically:**
- Create User Stories from requirements.md (under existing Epic)
- Create Tasks from tasks.md with time estimates
- Link predecessor/successor dependencies
- Update tasks.md with [ADO#ID] for each task
- Update aidlc-state.md with task IDs
- Commit spec updates to integration branch
- Report when complete

**You verify:** All tasks now have `[ADO#ID]` in tasks.md

---

### Step 3b: Request INCEPTION Approval (Phase Gate)

> **Required before moving to Phase 2.** Do not start any implementation until approval is given.

```
INCEPTION phase is complete for [feature-name].

Please:
1. Update .aidlc/specs/[feature-name]/aidlc-state.md:
   - phase: INCEPTION
   - completion_percentage: 100
   - approvals.inception_requested_at: [now]
2. Append to audit.md: "[timestamp] — INCEPTION — [agent] — Requesting human approval to proceed to CONSTRUCTION"
3. Summarise: total tasks, critical path, any open questions.
```

- [ ] Human reviews `requirements.md`, `design.md`, `tasks.md`
- [ ] Human updates `aidlc-state.md` with approval details
- [ ] **Only then:** proceed to Phase 2

---

## Phase 2: Build (Varies - mostly automated)

### Step 4: Execute Tasks

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

**You review:** Merge each task PR after review (or delegate to AI pre-review)

---

### AI Pre-Review (Optional)

For each task PR:

```
Review PR: task/ADO-[id]-[task-name]

Check against spec and standards. Flag issues.
```

---

### Step 5: Integration Testing

When all tasks are complete:

```
Generate integration tests for [feature-name]

Verify all acceptance criteria are met.
```

- [ ] All tests pass
- [ ] No regressions

---

## Phase 3: Deploy & Close (30-60 minutes)

### Step 6: Feature PR

```
Create PR for feature/ADO-[epic-id]-[feature-name] → main

Include summary, user stories completed, testing evidence.
```

- [ ] Get approval
- [ ] Merge to main

---

### Step 7: Deploy

```
Deploy [feature-name] to [environment]
```

- [ ] Dev — passed
- [ ] Staging — passed
- [ ] Production — passed

**Monitor:**

```
Check deployment status for [feature-name]
```

---

### Step 8: Close Out

```
Close out [feature-name]

Mark spec COMPLETE, close all ADO items, summarize outcome.
```

- [ ] `.ai/memory/` files updated (architecture, conventions, decisions)
- [ ] All ADO items closed
- [ ] Spec marked complete

---

## Fast Track Timeline

| Phase | Estimated Time |
|-------|---------------|
| **Setup** | |
| Generate spec | 5 min |
| Review spec | 10 min |
| Create ADO tickets | Agent handles automatically |
| | |
| **Build** | |
| Implementation | Varies (agent-driven) |
| Task PR reviews | 5-10 min per PR |
| Integration testing | 30 min |
| | |
| **Deploy & Close** | |
| Feature PR | 10 min |
| Deploy | Varies by environment |
| Close out | Agent handles automatically |

**Total overhead: ~55 min** (setup + review + testing + PR)  
**Implementation: Agent-driven** (runs autonomously)

---

## When Things Go Wrong

**Spec has issues after review:**
```
Update .aidlc/specs/[feature-name]/[file].md to fix [issue]
```

**Need to add task mid-implementation:**
```
Add new task to .aidlc/specs/[feature-name]/tasks.md and aidlc-state.md

Create corresponding ADO task
```

**Task blocked unexpectedly:**
```
Update aidlc-state.md:
- Task [N]: Status: Blocked | Blocked by: [reason]
```

**Agent needs guidance:**
```
Continue [feature-name]

[Provide specific guidance for the current task]
```

---

## Tips for Fast Track Success

✅ **Use for:** CRUD operations, UI components, API endpoints, integrations  
✅ **Works best when:** Requirements are clear, team is experienced, feature is isolated  
✅ **Time saved:** ~70% automation — agent handles all mechanical work  

❌ **Don't use for:** New architectures, cross-team features, unclear requirements, risky changes  

---

**Next:** See [full-lifecycle-guide.md](./full-lifecycle-guide.md) for the interactive structured approach with discovery phase

---

## Fast Track Timeline

| Phase | Time | Cumulative |
|-------|------|------------|
| 1. Generate spec | 5 min | 5 min |
| 2. Review | 10 min | 15 min |
| 3. Create ADO items | 5 min | 20 min |
| 4. Create branch | 2 min | 22 min |
| 5. Implementation | Varies | — |
| 6. PR reviews | Ongoing | — |
| 7. Integration testing | 30 min | — |
| 8. Feature PR | 10 min | — |
| 9. Deploy | Varies | — |
| 10. Close out | 5 min | — |

**Setup overhead:** ~22 minutes  
**Teardown overhead:** ~45 minutes

---

## When Things Go Wrong

**Spec has gaps or errors:**
```
Update .aidlc/specs/[FEATURE-NAME]/[FILE].md to fix [ISSUE]
```

**Task breakdown is wrong:**
```
Update .aidlc/specs/[FEATURE-NAME]/tasks.md:
- Split Task X into Task X.1 and X.2
- Update dependencies
- Update aidlc-state.md to match
```

**Need to add a task mid-implementation:**
```
Add new task to .aidlc/specs/[FEATURE-NAME]/tasks.md:
- Task N+1: [DESCRIPTION]
- Depends on: [TASK-IDS]
- Estimate: [HOURS]

Create ADO task and link dependencies.
Update aidlc-state.md with new task (Status: Available or Blocked).
```

---

## Tips for Fast Track Success

✅ **Use for:** CRUD operations, UI components, API endpoints, integrations  
✅ **Works best when:** Requirements are clear, team is experienced, feature is isolated  
✅ **Time saved:** ~50% less ceremony than full lifecycle  

❌ **Don't use for:** New architectures, cross-team features, unclear requirements, risky changes  

---

**Next:** See [full-lifecycle-guide.md](./full-lifecycle-guide.md) for the interactive structured approach
