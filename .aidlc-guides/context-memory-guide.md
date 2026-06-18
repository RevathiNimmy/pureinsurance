# AIDLC Context Memory Guide

**Version**: 1.0  
**Last Updated**: 2026-05-08  
**Applies To**: All AI agents working on Pure Insurance codebase

---

## Purpose

This guide establishes the **mandatory protocol** for AI agents to leverage the `.ai/memory` folder as the **primary source of architectural context, conventions, and domain knowledge** before starting any AIDLC task.

**Why this matters:**
- Prevents re-learning of established patterns
- Ensures consistency with existing architecture
- Reduces errors from incorrect assumptions
- Speeds up onboarding for new features
- Maintains institutional knowledge across sprints

---

## Memory Folder Structure

The `.ai/memory` folder contains **permanent project knowledge** organized as follows:

```
.ai/memory/
├── architecture.md              # System architecture, component layers
├── conventions.md               # Coding standards, naming conventions
├── patterns.md                  # Design patterns, reusable solutions
├── data-models.md              # Database schema, entity relationships
├── api-documentation.md        # REST API endpoints, contracts
├── dependencies.md             # External dependencies, NuGet packages
├── decisions.md                # Architectural Decision Records (ADRs)
├── known-issues.md             # Known limitations, workarounds
├── glossary.md                 # Domain terminology, acronyms
├── test-patterns.md            # Testing strategies, test patterns
├── codebase-map.md             # Directory structure, where to find things
└── docs/                       # Detailed component references
    ├── database_knowledge.md   # Database-specific knowledge
    ├── database_rules.md       # Database coding rules
    ├── rest-api-reference.md   # Complete API documentation
    ├── web-portal-reference.md # Portal patterns, controls
    ├── *-components-reference.md  # Per-component guides
    └── guidelines.md           # General development guidelines
```

---

## Mandatory Protocol: Read Before You Code

### ✅ ALWAYS Execute This Sequence BEFORE Starting Work

**When you receive a task (whether via ADO assignment or spec claim):**

1. **Read `.ai/memory/architecture.md`** — Understand system layers, component boundaries
2. **Read `.ai/memory/conventions.md`** — Learn coding standards for the language/framework you'll use
3. **Read `.ai/memory/patterns.md`** — Check if your task matches an existing pattern
4. **Read task-specific memory files** based on your work:

   | If your task involves... | Read these memory files |
   |-------------------------|-------------------------|
   | **Database changes** | `data-models.md`, `docs/database_knowledge.md`, `docs/database_rules.md` |
   | **REST API** | `api-documentation.md`, `docs/rest-api-reference.md` |
   | **Portal (Web Forms)** | `docs/web-portal-reference.md`, `patterns.md` (UI patterns section) |
   | **Back Office (WinForms)** | `docs/back-office-components-reference.md`, `docs/back-office-ui-controls-reference.md` |
   | **Claims** | `docs/claims-components-reference.md`, `glossary.md` (claims terms) |
   | **Orion (Accounts)** | `docs/orion-components-reference.md` |
   | **Underwriting** | `docs/underwriting-components-reference.md` |
   | **New dependencies** | `dependencies.md` (check before adding new NuGet packages) |
   | **Testing** | `test-patterns.md` |

5. **Search `.ai/memory/decisions.md`** — Check if your feature area has existing ADRs
6. **Check `.ai/memory/known-issues.md`** — Avoid known pitfalls
7. **Search `.ai/memory/glossary.md`** — Learn domain-specific terminology

---

## Integration with AIDLC Workflow

### Mode A: Guided (L1/L2) — Manual Agent Sessions

**CLAIM Phase** (on integration branch):
```
1. Checkout feature/ADO-[epic-id]-[feature-name]
2. Pull latest
3. Read aidlc-state.md → identify task to claim
4. ✅ READ .ai/memory/ FILES (per protocol above)  ← NEW STEP
5. Update aidlc-state.md (Status: Claimed)
6. Commit and push state files
7. Update ADO task to "In Progress"
```

**IMPLEMENT Phase** (on task branch):
```
8. Create task/ADO-[id]-[task-name] branch
9. ✅ RE-READ .ai/memory/ FILES IF NEEDED  ← NEW STEP
10. Implement using knowledge from .ai/memory/
11. Write tests per .ai/memory/test-patterns.md
12. Build and verify locally
13. Commit and push
```

**COMPLETE Phase** (on integration branch):
```
14. Update aidlc-state.md (Status: Done)
15. ✅ UPDATE .ai/memory/ FILES WITH NEW LEARNINGS  ← NEW STEP
    - If you introduced a NEW pattern → add to patterns.md
    - If you made an architectural decision → add to decisions.md
    - If you discovered a convention → add to conventions.md
    - If you created new API endpoints → update api-documentation.md
16. Commit state + memory updates
17. Raise PR
```

### Mode B: Autonomous (L3) — Direct ADO Task Assignment

**Execution Protocol**:
```
1. Read assigned ADO task
2. Read tasks.md for your task details
3. ✅ READ .ai/memory/ FILES (per protocol above)  ← NEW STEP
4. Create task branch
5. Implement using .ai/memory/ knowledge
6. Test per .ai/memory/test-patterns.md
7. ✅ UPDATE .ai/memory/ FILES WITH NEW LEARNINGS  ← NEW STEP
8. Update aidlc-state.md + commit
9. Raise PR
```

---

## Memory File Maintenance Rules

### When to UPDATE .ai/memory/ Files

✅ **YES - Update immediately when:**
- You introduce a **new architectural pattern** (add to `patterns.md`)
- You make a **significant architectural decision** (add to `decisions.md`)
- You create **new API endpoints** (update `api-documentation.md`)
- You add **new database tables/columns** (update `data-models.md`)
- You discover a **new convention** that should be standardized (add to `conventions.md`)
- You complete the **LAST task in an epic** and the feature introduces systemic changes

❌ **NO - Don't update for:**
- Trivial bug fixes that don't change patterns
- Refactoring that follows existing patterns
- Documentation-only changes (unless they reveal new patterns)
- Individual task completion within an epic (wait for epic completion)

### Update Format

When updating `.ai/memory/` files, use this format:

```markdown
## [Your New Section Title]

**Added**: 2026-05-08 (Epic #39472)  
**Author**: [Agent Name]  
**Context**: Claim Recovery Instalment Portal feature

[Your content here - pattern, decision, convention, etc.]

### Example
[Code example if applicable]

### When to Use
[Guidance on when this pattern/decision applies]

### Related
- See: [Link to related memory file section]
- ADR: [Link to decisions.md if applicable]
```

---

## Example: Applying Memory Context to Epic #39472

### Scenario
You're implementing Task 5 (Plan Config & Save) for Claim Recovery Instalment Portal.

### Memory Files You MUST Read
1. ✅ `architecture.md` → Understand Portal → REST API → Business Layer separation
2. ✅ `conventions.md` → Learn VB.NET code-behind naming conventions for Web Forms
3. ✅ `patterns.md` → Find "Reusing Premium Finance Patterns" section (if exists)
4. ✅ `docs/web-portal-reference.md` → Learn how Portal calls REST API via ProviderSAMForInsurance
5. ✅ `docs/rest-api-reference.md` → Find SavePremiumFinanceDetails API signature
6. ✅ `data-models.md` → Understand PFPremiumFinance table structure
7. ✅ `docs/database_rules.md` → Learn stored procedure naming conventions
8. ✅ `glossary.md` → Look up "ProcessPFMode", "TransType", "SR", "TPR"

### What You WOULD Learn
From these files you'd discover:
- **ProcessPFMode parameter** is required for recovery vs. standard PF (prevents the bug you had!)
- **TransType field** must be set to "SR"/"TPR" for recovery plans
- **Session("PFProcessMode")** is the standard way Portal passes context
- **Stored procedure naming**: `spu_*` prefix is mandatory
- **API pattern**: Always use MediatR commands, never direct DB calls

### What You WOULD Update After Task Completion
When Epic #39472 is COMPLETE (all tasks Done):
- ✅ `patterns.md` → Add new section: "Reusing Premium Finance UI for Claim Recovery Context"
- ✅ `decisions.md` → Add ADR: "Why we chose to bypass receipt validators for instalment plans"
- ✅ `api-documentation.md` → Document ProcessPFMode parameter in SavePremiumFinanceDetails
- ✅ `docs/web-portal-reference.md` → Add "Recovery Instalment Plan Creation Pattern"
- ✅ `glossary.md` → Add ICC, ICD, ProcessPFMode terms

---

## Quality Gates

### Pre-Implementation Checklist

Before writing ANY code, confirm:
- [ ] I have read `architecture.md` and understand the system layers
- [ ] I have read `conventions.md` for my programming language
- [ ] I have searched `patterns.md` for similar features
- [ ] I have checked task-specific memory files (database/API/Portal/etc.)
- [ ] I have reviewed `decisions.md` for relevant ADRs
- [ ] I have checked `known-issues.md` for pitfalls in my work area

### Post-Implementation Checklist

Before marking task/epic Done:
- [ ] I have updated `.ai/memory/` files with any new patterns/decisions/conventions
- [ ] My updates follow the standard format (Added date, Epic ID, Context)
- [ ] I have verified my changes don't contradict existing memory files
- [ ] If this is the LAST task in an epic, I have comprehensively updated all relevant memory files

---

## Conflict Resolution

### What if Memory Files Contradict Requirements?

**Priority Order** (highest to lowest):
1. **Epic/PBI Requirements** (requirements.md, design.md, tasks.md) — The current work
2. **Architectural Decision Records** (decisions.md) — Explicit past decisions
3. **Memory Files** (architecture.md, patterns.md, conventions.md) — General guidance
4. **Code Comments** — Localized knowledge

**If you find a conflict:**
1. Assume requirements.md is correct (it represents the new direction)
2. Check decisions.md to see if there's an ADR explaining why memory files are different
3. If no ADR exists and requirements contradict memory:
   - **Flag it in your PR description**
   - **Create a NEW ADR in decisions.md** explaining the divergence
   - **Update memory files** to reflect the new pattern

---

## Memory File Ownership

| File | Owner Role | Update Frequency |
|------|-----------|------------------|
| architecture.md | Architect | Per epic (if architecture changes) |
| conventions.md | Tech Lead | Per sprint (as conventions emerge) |
| patterns.md | All developers | Per epic (when new patterns introduced) |
| decisions.md | Architect + Senior Devs | Per significant decision |
| data-models.md | Database team | Per schema change |
| api-documentation.md | Backend team | Per API change |
| docs/*.md | Component owners | Per major feature |
| test-patterns.md | QA team | Per new test strategy |

**All agents** can propose updates via PR, but final approval rests with the file owner.

---

## Quick Start Command

**Before starting any task, run this mental checklist:**

```
1. What type of work am I doing? (Portal/API/Database/Claims/etc.)
2. Which .ai/memory/ files apply? (Check table in "Mandatory Protocol" section)
3. Read those files now
4. Proceed with implementation
```

**After completing the LAST task in an epic:**

```
1. What new patterns did this epic introduce?
2. What decisions did we make that others should know?
3. What conventions emerged?
4. Update .ai/memory/ files accordingly
5. Commit updates to integration branch
```

---

## Examples of Good Memory Updates

### Example 1: Adding a Pattern

**File**: `.ai/memory/patterns.md`

```markdown
## Reusing Premium Finance UI for Alternative Contexts

**Added**: 2026-05-08 (Epic #39472)  
**Author**: GitHub Copilot  
**Context**: Claim Recovery Instalment Portal

### Pattern
When implementing instalments for non-premium finance contexts (e.g., claim recovery), reuse the existing `Controls/Instalments.ascx` user control rather than creating a new UI.

### Implementation
1. Call `bindInstalments()` method via reflection with custom `sProcessPFMode` parameter
2. Filter scheme grid to appropriate SchemeTypeCode (e.g., "CR" for Claim Recovery)
3. Set `Session("PFProcessMode")` to context code ("SR"/"TPR") before calling control
4. Pass ProcessPFMode through to REST API in SavePremiumFinanceDetails command

### When to Use
- Any workflow that creates instalment plans outside standard Premium Finance
- Salvage recovery, Third-Party recovery, or future instalment contexts

### Benefits
- Zero UI duplication
- Consistent UX across all instalment creation workflows
- Automatic inheritance of Premium Finance validation rules
- Reduced maintenance burden

### Related
- See: `api-documentation.md` → SavePremiumFinanceDetails → ProcessPFMode parameter
- ADR: `decisions.md` → "Why ProcessPFMode is Required for Context Switching"
```

### Example 2: Adding a Decision

**File**: `.ai/memory/decisions.md`

```markdown
## ADR-042: ProcessPFMode Parameter for Premium Finance Context Switching

**Date**: 2026-05-08  
**Status**: Accepted  
**Epic**: #39472 (Claim Recovery Instalment Portal)  
**Deciders**: Architecture Team, Backend Team

### Context
The existing Premium Finance instalment system was designed for policy instalments only (TransType = SND/REN/MTA). We needed to extend it to support Claim Recovery instalments (TransType = SR/TPR) with different transaction codes (ICC/ICD instead of INC/IND).

### Decision
Introduce a **ProcessPFMode** parameter that flows from Portal → REST API → GetInstalmentQuotes to enable context-aware scheme filtering and rate calculation.

**Values**:
- `"SR"` = Salvage Recovery
- `"TPR"` = Third-Party Recovery
- `"PF"` = Standard Premium Finance (default if omitted)

### Consequences

**Positive**:
- Single codebase supports multiple instalment contexts
- No need to duplicate Premium Finance logic
- Scheme filtering happens automatically based on TransType
- Transaction code mapping (INC→ICC, IND→ICD) is conditional and automatic

**Negative**:
- Breaking change to GetInstalmentQuotes API signature (but internal only)
- Portal must always pass ProcessPFMode for recovery (failure = GetSingleFinancePlan crash)
- Requires migration of existing Portal code to pass ProcessPFMode

**Mitigation**:
- Default ProcessPFMode to "PF" in API layer if omitted (backward compatibility)
- Add validation in SavePremiumFinanceDetailsService to block save if ProcessPFMode is missing for recovery plans

### Related
- Pattern: `patterns.md` → "Reusing Premium Finance UI for Alternative Contexts"
- Implementation: Epic #39472 commits 4c7ba69d06, 312c7e38, b7f6b70004
```

---

## Enforcement

This protocol is **mandatory** and will be enforced via:

1. **PR Review Checklist**: PRs must confirm memory files were read and updated
2. **AIDLC State Audit**: `aidlc-state.md` completion requires memory file review
3. **Epic Completion Gate**: Epic cannot be closed until memory files are updated

**Non-compliance consequences**:
- PR will be rejected with request to read relevant memory files
- Task will be returned to "In Progress" until memory context is confirmed
- Repeated violations may result in agent re-training

---

## FAQ

**Q: Do I need to read ALL memory files for every task?**  
A: No. Use the task-specific table in the "Mandatory Protocol" section to identify which files apply to your work.

**Q: What if the memory files are outdated?**  
A: Update them! If you discover incorrect information, fix it as part of your task and note the correction in your PR.

**Q: How do I know if my task requires a memory update?**  
A: Ask: "Did I introduce a new pattern, make a significant decision, or discover a convention that future developers should know?" If yes, update.

**Q: Can I add new memory files?**  
A: Yes! If you're working on a new component area that doesn't have a reference doc, create one in `.ai/memory/docs/` and add a link to the main README.

**Q: What if reading memory files would delay my task?**  
A: Reading memory files **saves** time by preventing re-work. A 10-minute read prevents hours of refactoring when you discover you violated existing patterns.

---

## Maintenance

This guide should be reviewed and updated:
- **Quarterly**: By Tech Lead and Architecture team
- **Per major epic**: If the epic introduces new memory file categories
- **On request**: Any team member can propose improvements via PR

---

**Last Reviewed**: 2026-05-08  
**Next Review Due**: 2026-08-08  
**Owner**: Architecture Team + AIDLC Governance
