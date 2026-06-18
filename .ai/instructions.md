# AI Agent Instructions - Universal

**Applies To**: GitHub Copilot, Kiro, Amazon Q, and all AI coding assistants  
**Last Updated**: 2026-05-08  
**Project**: Pure Insurance

---

## Critical: Context Memory Protocol

**BEFORE starting ANY task, you MUST:**

1. **Read `.ai/memory/architecture.md`** — Understand system layers and component boundaries
2. **Read `.ai/memory/conventions.md`** — Learn coding standards for the language/framework
3. **Read task-specific memory files** based on your work area (see table below)
4. **Check `.ai/memory/decisions.md`** — Review Architectural Decision Records (ADRs)
5. **Review `.ai/memory/known-issues.md`** — Avoid known pitfalls and workarounds

**See**: [`.aidlc-guides/context-memory-guide.md`](.aidlc-guides/context-memory-guide.md) for complete protocol.

---

## Task-Specific Memory Files

| If your task involves... | Read these `.ai/memory/` files |
|-------------------------|-------------------------------|
| **Database changes** | `data-models.md`, `docs/database_knowledge.md`, `docs/database_rules.md` |
| **REST API** | `api-documentation.md`, `docs/rest-api-reference.md` |
| **Portal (Web Forms)** | `docs/web-portal-reference.md`, `patterns.md` (UI patterns) |
| **Back Office (WinForms)** | `docs/back-office-components-reference.md`, `docs/back-office-ui-controls-reference.md` |
| **Claims** | `docs/claims-components-reference.md`, `glossary.md` (claims terminology) |
| **Orion (Accounts)** | `docs/orion-components-reference.md` |
| **Underwriting** | `docs/underwriting-components-reference.md` |
| **Testing** | `test-patterns.md` |
| **New dependencies** | `dependencies.md` (check before adding NuGet packages) |

---

## AIDLC Path Configuration

Spec files are **always** at:
- Requirements: `.aidlc/specs/{feature-name}/requirements.md`
- Design: `.aidlc/specs/{feature-name}/design.md`
- Tasks: `.aidlc/specs/{feature-name}/tasks.md`
- State: `.aidlc/specs/{feature-name}/aidlc-state.md`
- Audit: `.aidlc/specs/{feature-name}/audit.md`

**Never** use `.github/specs/` — always use `.aidlc/specs/`.

---

## Branch Strategy

Two branch types are **always** used during feature development:

### Integration Branch
- **Name**: `feature/ADO-[epic-id]-[feature-name]`
- **Contains**: `aidlc-state.md`, `audit.md`
- **Purpose**: State coordination between tasks
- **Rule**: Never commit implementation code directly here

### Task Branch
- **Name**: `task/ADO-[task-id]-[task-name]`
- **Contains**: Implementation code, tests
- **Created from**: Integration branch
- **Merged to**: Integration branch via PR

**Always pull** the latest integration branch before reading/writing `aidlc-state.md`.

---

## ADO Pre-flight Check

**REQUIRED before spec creation:**

When asked to "Create AIDLC spec for [feature-name]":

1. Read `.aidlc/config.json` — Confirm organisation, project, `workItemTypes`
2. Attempt to create Epic in Azure DevOps via MCP
3. **If ADO MCP unavailable** → STOP. Do NOT create files or branches
4. Tell user: *"ADO MCP is unavailable. Cannot create Epic or integration branch. Please ensure ADO MCP server is running."*
5. Only proceed after Epic ID is obtained

---

## Execution Modes

### Mode A: Guided (L1/L2)

Used when agents are started manually per session. `aidlc-state.md` is the coordination file.

#### CLAIM (on integration branch)
1. Checkout `feature/ADO-[epic-id]-[feature-name]` and pull latest
2. Read `aidlc-state.md` → find highest priority task with `Status: Available`
3. **✅ READ `.ai/memory/` FILES** (per protocol above)
4. Update `aidlc-state.md`: `Status: Claimed | Agent: [session-id]`
5. Append claim entry to `audit.md`
6. Commit and push state files immediately
7. Update ADO task to "In Progress"

#### IMPLEMENT (on task branch)
8. **Branch safety guard**: Run `git branch --show-current`
9. If NOT `task/ADO-*` → STOP and alert user
10. Create task branch: `git checkout -b task/ADO-[id]-[task-name] feature/ADO-[epic-id]-[feature-name]`
11. **✅ RE-READ `.ai/memory/` FILES IF NEEDED**
12. Implement using knowledge from `.ai/memory/`
13. Write tests per `.ai/memory/test-patterns.md`
14. Run tests locally and verify they pass
15. Build and verify application locally
16. Commit and push task branch

#### COMPLETE (on integration branch)
17. Checkout integration branch and pull latest
18. Update `aidlc-state.md`: `Status: Done`
19. Promote newly unblocked tasks from `Blocked` → `Available`
20. **✅ UPDATE `.ai/memory/` FILES** (if this is LAST task in epic and introduces new patterns)
21. Append completion entry to `audit.md`
22. Commit and push state + memory updates
23. Update ADO task to "Done"
24. Raise PR: `task/ADO-[id]-[task-name]` → `feature/ADO-[epic-id]-[feature-name]`
25. Return to CLAIM

**Conflict Rule**: If two agents claim same task, the LAST claim wins. Loser must pull, re-read state, and claim different task.

---

### Mode B: Autonomous (L3)

Used when agents are assigned individual ADO tasks directly. ADO is the coordination layer.

1. Read assigned ADO task
2. Read `.aidlc/specs/[feature-name]/tasks.md` → find task by ADO ID
3. **✅ READ `.ai/memory/` FILES** (per protocol above)
4. Read `requirements.md` and `design.md` for context
5. Create task branch from integration branch
6. **Branch safety guard**: Verify on `task/ADO-*` branch
7. Implement using `.ai/memory/` knowledge
8. Write tests per `.ai/memory/test-patterns.md`
9. Run tests locally and verify they pass
10. Build and verify application locally
11. Commit and push task branch
12. Checkout integration branch and pull latest
13. Update `aidlc-state.md`: `Status: Done` for your task
14. Promote newly unblocked tasks from `Blocked` → `Available`
15. **✅ UPDATE `.ai/memory/` FILES** (if LAST task in epic and introduces new patterns)
16. Append completion entry to `audit.md`
17. Commit and push state + memory updates
18. Update ADO task to "Done"
19. Raise PR: `task/ADO-[id]-[task-name]` → `feature/ADO-[epic-id]-[feature-name]`

---

## Memory File Update Rules

### ✅ UPDATE `.ai/memory/` when:
- Introducing a **new architectural pattern** → add to `patterns.md`
- Making a **significant architectural decision** → add to `decisions.md`
- Creating **new API endpoints** → update `api-documentation.md`
- Adding **new database tables/columns** → update `data-models.md`
- Discovering a **new convention** to standardize → add to `conventions.md`
- Completing the **LAST task in an epic** with systemic changes

### ❌ DON'T UPDATE for:
- Trivial bug fixes (no pattern changes)
- Refactoring following existing patterns
- Individual task completion within an epic (wait for epic completion)
- Documentation-only changes (unless they reveal new patterns)

### Update Format:
```markdown
## [Your Section Title]

**Added**: 2026-05-08 (Epic #39472)  
**Author**: [Agent Name]  
**Context**: [Feature description]

[Your content - pattern, decision, convention, etc.]

### Example
[Code example if applicable]

### When to Use
[Guidance on when this applies]

### Related
- See: [Link to related section]
- ADR: [Link to decisions.md if applicable]
```

---

## Azure DevOps MCP Tool Usage

**CRITICAL**: Use correct MCP tool for each ADO operation.

### Work Item Types

**Always read** work item type names from `.aidlc/config.json` (`workItemTypes` section) before creating work items.

Example from Pure Insurance:
```json
"workItemTypes": {
  "epic": "Epic",
  "story": "Product Backlog Item",
  "task": "Task",
  "bug": "Bug",
  "issue": "Issue"
}
```

### Tool Selection

| Operation | Tool Name | Don't Use |
|-----------|-----------|-----------|
| Get single work item status | `mcp__ado__wit_get_work_item` | ❌ batch tools |
| Create Epic/Story/Task | `mcp__ado__wit_create_work_item` | ❌ update tools |
| Update work item state | `mcp__ado__wit_update_work_item` | ❌ create tools |
| Link work items | `mcp__ado__wit_work_items_link` | ❌ create tools |

**Reference**: See `.aidlc-guides/copilot-ado-tool-guide.md` for complete examples.

---

## Quality Checks

### Pre-Implementation Checklist

Before writing ANY code:
- [ ] Read `architecture.md` and understand system layers
- [ ] Read `conventions.md` for programming language
- [ ] Search `patterns.md` for similar features
- [ ] Check task-specific memory files (database/API/Portal/etc.)
- [ ] Review `decisions.md` for relevant ADRs
- [ ] Check `known-issues.md` for pitfalls in work area

### Code Quality

- **All code changes MUST include tests**
- No hardcoded secrets in code
- Security: input validation, parameterized queries, XSS prevention, auth checks
- Follow conventions from `.ai/memory/conventions.md`

### Post-Implementation Checklist

Before marking task/epic Done:
- [ ] Updated `.ai/memory/` files with new patterns/decisions/conventions
- [ ] Updates follow standard format (Added date, Epic ID, Context)
- [ ] Verified changes don't contradict existing memory files
- [ ] If LAST task in epic, comprehensively updated all relevant memory files

---

## Technology Stack

- **Portal**: ASP.NET Web Forms, .NET Framework 4.8
- **REST API**: .NET Core/C#, MediatR pattern
- **Database**: SQL Server, stored procedures only (no inline SQL)
- **Data Access**: dPMDAO (stored procedure wrapper)
- **Source Control**: Azure DevOps Git

---

## Conventions

See `.ai/memory/conventions.md` for complete coding standards.

**Key Rules:**
- VB.NET: PascalCase for all identifiers, prefix private fields with `m_`
- C#: Standard .NET conventions
- Stored Procedures: `spu_` prefix mandatory
- No inline SQL — all database operations via stored procedures
- API: Always use MediatR commands/queries
- Portal: Session variables follow `CN*` naming pattern for claims

---

## Agent-Specific Guidance

### For GitHub Copilot
- Use inline suggestions for small edits
- Use Copilot Chat for complex multi-file changes
- Always verify file paths before editing
- Read memory files using `@workspace` context

### For Kiro
- Leverage multi-file editing capabilities
- Use codebase-wide search before making assumptions
- Always confirm `.ai/memory/` files are up-to-date
- Coordinate with other agents via `aidlc-state.md`

### For Amazon Q
- Use `/dev` mode for larger feature implementations
- Read memory files explicitly before suggesting changes
- Confirm branch strategy before creating new branches
- Validate all changes against `.ai/memory/conventions.md`

---

## Enforcement

This protocol is **mandatory** and enforced via:

1. **PR Review**: PRs rejected if memory files not read/updated
2. **AIDLC State Audit**: Tasks returned to "In Progress" if memory context missing
3. **Epic Completion Gate**: Epic cannot close until memory files updated

---

## Quick Reference

**Starting a task?**
```
1. What am I working on? (Portal/API/DB/Claims/etc.)
2. Read .ai/memory/architecture.md + conventions.md
3. Read task-specific memory files from table above
4. Check decisions.md + known-issues.md
5. Create task branch and start coding
```

**Finishing an epic?**
```
1. What new patterns did this introduce?
2. What decisions did we make?
3. Update patterns.md + decisions.md + api-documentation.md
4. Commit to integration branch
5. Mark epic Done
```

---

## Help & References

- **Full Context Guide**: `.aidlc-guides/context-memory-guide.md`
- **Working Guide**: `.aidlc-guides/working-guide.md`
- **ADO Tool Guide**: `.aidlc-guides/copilot-ado-tool-guide.md`
- **Quick Reference**: `.aidlc-guides/quick-reference.md`
- **Memory Files**: `.ai/memory/` directory

---

**Last Updated**: 2026-05-08  
**Maintained By**: Architecture Team + AIDLC Governance
