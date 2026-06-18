# Ticket Integration Policy: Specs ↔ Work Tracking

## The Problem

AIDLC specs (requirements.md, design.md, tasks.md) live in the repository. Work tracking (ADO work items or GitHub Issues) lives in the project management tool. Without a clear policy on what goes where and when, we get:

- Duplicate information that drifts out of sync
- Tickets with no context (just a title)
- Specs with no link to trackable work
- No visibility of AI-driven work in project boards

## The Policy

**Specs are the source of truth for detail. Tickets are the source of truth for status and assignment.**

| Information | Lives In | Why |
|-------------|---------|-----|
| Detailed requirements, acceptance criteria | Spec (requirements.md) | Co-located with code, AI can read it |
| Architecture, diagrams, API specs | Spec (design.md) | Co-located with code, AI can read it |
| Task breakdown with implementation detail | Spec (tasks.md) | Co-located with code, AI can read it |
| Status, assignment, priority, sprint | Ticket (ADO/GitHub) | Visible to project management |
| Comments, discussions, approvals | Ticket (ADO/GitHub) | Visible to stakeholders |

---

## When Tickets Are Created

| Event | Who Creates Ticket | What Goes In The Ticket |
|-------|-------------------|------------------------|
| New feature requested | Product Owner | Title, brief description, priority, link to spec |
| AIDLC spec completed (requirements approved) | Developer or AI (L2+) | One ticket per feature linking to spec, plus child tickets per task |
| Bug reported | Anyone | Bug description, steps to reproduce, link to bugfix spec (if using AIDLC) |
| Task breakdown generated | Developer or AI (L2+) | One child ticket per task from tasks.md |

---

## What Goes In Each Ticket Type

### Feature Ticket (Epic/User Story)

```
Title: [Feature name]
Description:
  Spec: .aidlc/specs/[feature-name]/requirements.md
  Design: .aidlc/specs/[feature-name]/design.md

  Summary: [2-3 sentence summary from requirements.md]

  Acceptance Criteria: [copied from requirements.md]

Labels: ai-sdlc, [product], [priority]
```

Keep the ticket lean. The detail is in the spec. The ticket provides:
- Visibility on the board
- Assignment and status tracking
- A link to the full spec

### Task Ticket (Child of Feature)

```
Title: Task [number]: [task title from tasks.md]
Description:
  Parent: [link to feature ticket]
  Spec: .aidlc/specs/[feature-name]/tasks.md#task-[number]

  What: [task description from tasks.md]
  Acceptance: [task acceptance criteria from tasks.md]

Labels: ai-sdlc, task
```

### Bug Ticket

```
Title: Bug: [brief description]
Description:
  Bug: [what's happening]
  Expected: [what should happen]
  Steps to reproduce: [steps]
  Spec: .aidlc/specs/[fix-name]/bugfix.md (if using AIDLC bugfix workflow)

Labels: bug, [severity]
```

---

## Flow by Maturity Level

### Volaris L1 (AI Assisted)

```
1. PO creates feature ticket manually
2. Developer creates AIDLC spec
3. Developer manually creates child task tickets from tasks.md
4. Developer works tasks, updates ticket status manually
5. Developer links PR to ticket
```

Tickets and specs are created separately. Developer keeps them in sync manually.

### Volaris L2 (AI Directed)

```
1. PO creates feature ticket (or developer creates from spec)
2. Developer creates AIDLC spec
3. AI generates task tickets from tasks.md (with prompt)
4. Developer works tasks, AI updates spec progress
5. Developer updates ticket status
6. AI generates PR description linking to ticket and spec
```

Prompt to create tickets from spec:
```
Create [ADO work items / GitHub issues] from .aidlc/specs/[feature-name]/tasks.md:
- One parent feature ticket with summary and acceptance criteria from requirements.md
- One child ticket per task with description from tasks.md
- Link all tickets to the spec
- Add labels: ai-sdlc, [product]
```

### Volaris L3 (AI Delegated)

```
1. PO creates feature ticket (or issue with ai-agent label)
2. Agent creates AIDLC spec from ticket
3. Agent creates child task tickets from tasks.md
4. Agent works tasks, updates ticket status
5. Agent creates PR linking to ticket and spec
6. Human reviews PR and approves
```

At L3, the agent manages the full ticket lifecycle. Human reviews the output.

---

## Keeping Specs and Tickets In Sync

### Important: Status Does NOT Sync Automatically

Completing a ticket in ADO/GitHub does NOT update tasks.md in the repo. Completing a task via AIDLC does NOT update the ticket. The sync is one-directional at creation time only:

```
Spec (tasks.md) ──creates──▶ Tickets (ADO/GitHub)
                  ◀── no automatic sync back ──
```

### Source of Truth

| Information | Source of Truth | Why |
|-------------|---------------|-----|
| **Status** (done, in progress, blocked) | Tickets (ADO/GitHub) | Visible to everyone, existing process |
| **Detail** (requirements, design, implementation) | Spec (tasks.md, requirements.md, design.md) | Co-located with code, AI can read it |

Accept that tasks.md status may lag behind tickets at L1-L2. This is fine - tickets are where people check status, not tasks.md.

### How Sync Works at Each Level

| Level | tasks.md updated by | Tickets updated by | In sync? |
|:---:|---------------------|-------------------|:---:|
| L1 | Developer manually | Developer manually | Depends on discipline |
| L2 | AIDLC workflow (when executing tasks) | Developer manually or sync prompt | tasks.md more current |
| L3 | Agent automatically | Agent automatically (project-manager) | Yes |

### Rule: Spec is the detail, ticket is the status

| When This Happens | Update Spec | Update Ticket |
|-------------------|:-----------:|:-------------:|
| Requirements change | ✅ Update requirements.md | ✅ Update acceptance criteria on ticket |
| Design changes | ✅ Update design.md | ❌ No change needed |
| Task completed | ✅ Mark task done in tasks.md | ✅ Move ticket to done |
| New task added | ✅ Add to tasks.md | ✅ Create new child ticket |
| Bug found during work | ✅ Create bugfix spec (if needed) | ✅ Create bug ticket |
| PR created | ❌ No change | ✅ Link PR to ticket |

### Sync Prompts

**After completing work (L1-L2) - sync spec → tickets:**
```
Sync tickets with spec .aidlc/specs/[feature-name]/:
- Check tasks.md for completed/new tasks
- Update [ADO/GitHub] tickets to match
- Flag any tickets that don't match the spec
```

**After tickets updated externally - sync tickets → spec:**
```
Sync spec with tickets for [feature-name]:
- Check [ADO/GitHub] ticket status for this feature
- Update tasks.md checkboxes to match ticket status
- Flag any mismatches
```

**Quick status check (no sync, just report):**
```
Show status of .aidlc/specs/[feature-name]/:
- tasks.md completion percentage
- Current phase from aidlc-state.md
- Last modified date
```

---

## Platform-Specific Notes

### Azure DevOps (ADO)

- Feature ticket = User Story or Product Backlog Item
- Task tickets = Tasks (children of User Story)
- Link to spec using the description field (ADO doesn't have native repo links)
- Use tags for ai-sdlc tracking

### GitHub Issues

- Feature ticket = Issue (with feature label)
- Task tickets = Issues (linked to feature via task list or sub-issues)
- Link to spec using relative path in description (GitHub renders repo links)
- Use labels for ai-sdlc tracking
- At L3: GitHub Copilot Coding Agent can be assigned issues directly

---

## What's NOT in Tickets

Keep tickets lean. Don't duplicate spec content:

| Don't Put In Ticket | Why | Where It Lives |
|---------------------|-----|---------------|
| Full requirements detail | Drifts out of sync | requirements.md |
| Architecture diagrams | Can't render in ticket | design.md |
| API specifications | Too detailed for ticket | design.md |
| Implementation notes | Changes during development | tasks.md |
| Test specifications | Too detailed for ticket | tasks.md |

**Exception**: Acceptance criteria should be in both the spec AND the ticket, because reviewers and POs check tickets for acceptance.

---

## Agent Rules Addition

Add to agent configuration (Tier 1 setup):

```markdown
## Ticket Integration Rules
- Specs (.aidlc/specs/) are the source of truth for detail
- Tickets (ADO/GitHub) are the source of truth for status and assignment
- When creating tickets from specs, keep tickets lean with links to spec
- Always copy acceptance criteria to tickets (exception to lean rule)
- Link PRs to both tickets and specs
- Don't duplicate full spec content in tickets
```
