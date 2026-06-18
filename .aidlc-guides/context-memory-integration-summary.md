# Context Memory Integration - Summary

**Created**: 2026-05-08  
**Epic**: #39472 (Claim Recovery Instalment Portal)  
**Purpose**: Establish mandatory `.ai/memory/` context usage in AIDLC workflow

---

## What Was Created

### 1. **New Guide**: `.aidlc-guides/context-memory-guide.md`

Comprehensive guide covering:
- ✅ Memory folder structure and purpose
- ✅ Mandatory "Read Before You Code" protocol
- ✅ Integration with AIDLC Mode A (Guided) and Mode B (Autonomous)
- ✅ Rules for when to UPDATE memory files
- ✅ Quality gates and enforcement
- ✅ Examples of good memory updates
- ✅ FAQ and troubleshooting

### 2. **Updated**: `.github/copilot-instructions.md`

Added mandatory memory check to execution protocol:
```markdown
**CRITICAL**: Before starting ANY task, you MUST read `.ai/memory/` context files.  
See [Context Memory Guide](.aidlc-guides/context-memory-guide.md) for the complete protocol.

**Quick Memory Checklist:**
1. Read `.ai/memory/architecture.md` — Understand system layers
2. Read `.ai/memory/conventions.md` — Learn coding standards
3. Read task-specific files (database/API/Portal/etc. based on your work)
4. Check `.ai/memory/decisions.md` for relevant ADRs
5. Review `.ai/memory/known-issues.md` for pitfalls
```

### 3. **Updated**: `.aidlc-guides/README.md`

Added reference to Context Memory Guide in Policies section:
```markdown
### Policies & Protocols
- [Context Memory Guide](./context-memory-guide.md) - **MANDATORY**: How to use `.ai/memory/` folder
```

---

## How It Works

### For Mode A (Guided - Manual Sessions)

**BEFORE (Old Workflow):**
```
1. Claim task from aidlc-state.md
2. Create task branch
3. Start coding (might miss existing patterns)
```

**AFTER (New Workflow):**
```
1. Claim task from aidlc-state.md
2. ✅ READ .ai/memory/ files (architecture, conventions, task-specific)
3. Create task branch
4. Code using knowledge from .ai/memory/
5. ✅ UPDATE .ai/memory/ files if epic introduces new patterns
```

### For Mode B (Autonomous - ADO Direct Assignment)

**BEFORE (Old Workflow):**
```
1. Read ADO task
2. Create task branch
3. Start coding (might miss existing patterns)
```

**AFTER (New Workflow):**
```
1. Read ADO task
2. ✅ READ .ai/memory/ files (architecture, conventions, task-specific)
3. Create task branch
4. Code using knowledge from .ai/memory/
5. ✅ UPDATE .ai/memory/ files if task introduces new patterns
```

---

## Task-Specific Memory File Mapping

| If your task involves... | Read these memory files |
|-------------------------|-------------------------|
| **Database changes** | `data-models.md`, `docs/database_knowledge.md`, `docs/database_rules.md` |
| **REST API** | `api-documentation.md`, `docs/rest-api-reference.md` |
| **Portal (Web Forms)** | `docs/web-portal-reference.md`, `patterns.md` |
| **Back Office (WinForms)** | `docs/back-office-components-reference.md` |
| **Claims** | `docs/claims-components-reference.md`, `glossary.md` |
| **Orion (Accounts)** | `docs/orion-components-reference.md` |
| **Testing** | `test-patterns.md` |
| **New dependencies** | `dependencies.md` |

---

## When to Update .ai/memory/ Files

### ✅ YES - Update immediately when:
- You introduce a **new architectural pattern**
- You make a **significant architectural decision** (add to `decisions.md`)
- You create **new API endpoints** (update `api-documentation.md`)
- You add **new database tables/columns** (update `data-models.md`)
- You discover a **new convention** that should be standardized
- You complete the **LAST task in an epic** and feature introduces systemic changes

### ❌ NO - Don't update for:
- Trivial bug fixes that don't change patterns
- Refactoring that follows existing patterns
- Individual task completion within an epic (wait for epic completion)

---

## Example: How Epic #39472 Would Update Memory Files

**When Epic #39472 is COMPLETE**, update:

### 1. `.ai/memory/patterns.md`
Add section: "Reusing Premium Finance UI for Alternative Contexts"
- How to reuse `Controls/Instalments.ascx` for non-PF contexts
- ProcessPFMode parameter pattern
- Session-based context switching

### 2. `.ai/memory/decisions.md`
Add ADR: "ProcessPFMode Parameter for Premium Finance Context Switching"
- Why we chose conditional mapping over code duplication
- Why ProcessPFMode flows from Portal → API → Business layer
- Consequences and mitigation strategies

### 3. `.ai/memory/api-documentation.md`
Update `SavePremiumFinanceDetails` endpoint:
- Add ProcessPFMode parameter documentation
- Add ClaimRecoveryTransactionId parameter
- Document SR/TPR TransType values

### 4. `.ai/memory/docs/web-portal-reference.md`
Add "Recovery Instalment Plan Creation Pattern"
- How Portal wires recovery context to existing PF controls
- Reflection-based bindInstalments() call pattern
- Session variable usage (CNMode, CNAmountToPay, PFProcessMode)

### 5. `.ai/memory/glossary.md`
Add terms:
- **ICC**: Instalment Claim Credit
- **ICD**: Instalment Claim Debit
- **ProcessPFMode**: Context parameter for Premium Finance operations (SR/TPR/PF)
- **TransType**: Plan transaction type stored in PFPremiumFinance table

---

## Enforcement

### Pre-Implementation Quality Gate
**Before writing ANY code, confirm:**
- [ ] I have read `architecture.md`
- [ ] I have read `conventions.md` for my language
- [ ] I have searched `patterns.md` for similar features
- [ ] I have checked task-specific memory files
- [ ] I have reviewed `decisions.md` for relevant ADRs

### Post-Implementation Quality Gate
**Before marking epic Done:**
- [ ] I have updated `.ai/memory/` files with new patterns/decisions/conventions
- [ ] My updates follow the standard format (Added date, Epic ID, Context)
- [ ] I have verified my changes don't contradict existing memory files

### PR Review Checklist
PRs must include:
- [ ] Confirmation that memory files were read before implementation
- [ ] Memory file updates (if applicable)
- [ ] Reference to relevant memory file sections in PR description

---

## Benefits

### For AI Agents
- ✅ Prevents re-learning of established patterns
- ✅ Reduces trial-and-error coding
- ✅ Faster onboarding to unfamiliar components
- ✅ Consistent architecture across features

### For Human Developers
- ✅ Living documentation that evolves with codebase
- ✅ Searchable knowledge base (no tribal knowledge)
- ✅ Onboarding new team members faster
- ✅ Reduced code review churn

### For Project Quality
- ✅ Architectural consistency
- ✅ Reduced technical debt
- ✅ Fewer refactoring cycles
- ✅ Better maintainability

---

## Next Steps

1. ✅ **Guide created** — `.aidlc-guides/context-memory-guide.md`
2. ✅ **Copilot instructions updated** — `.github/copilot-instructions.md`
3. ✅ **README updated** — `.aidlc-guides/README.md`
4. ⏭️ **Train team** — Share guide with all developers
5. ⏭️ **Populate memory files** — Complete Epic #39472 updates to `.ai/memory/`
6. ⏭️ **Enforce in PRs** — Add memory file checks to PR template

---

## Quick Reference Card

**Before Starting Task:**
```
1. What am I working on? (Portal/API/DB/Claims/etc.)
2. Read .ai/memory/architecture.md + conventions.md
3. Read task-specific memory files from table
4. Check decisions.md + known-issues.md
5. Start coding
```

**After Completing Epic:**
```
1. What new patterns did this introduce?
2. What decisions did we make?
3. Update patterns.md + decisions.md + api-documentation.md
4. Commit to integration branch
5. Mark epic Done
```

---

**Document Owner**: Architecture Team + AIDLC Governance  
**Last Updated**: 2026-05-08  
**Next Review**: 2026-08-08
