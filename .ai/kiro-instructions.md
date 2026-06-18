# Kiro Agent Instructions

**Agent**: Kiro (Windsurf IDE AI Agent)  
**Project**: Pure Insurance  
**Last Updated**: 2026-05-08

---

## Quick Start

**READ FIRST**: [`.ai/instructions.md`](.ai/instructions.md) — Universal AI agent instructions

This file contains **Kiro-specific** optimizations and workflow patterns.

---

## Kiro-Specific Capabilities

### Multi-File Cascade Mode

Kiro excels at **multi-file edits**. Use this for:
- Refactoring across multiple components
- Updating API contracts + implementations + tests simultaneously
- Propagating changes through Portal → API → Database layers

**Pattern:**
```
User: "Update SavePremiumFinanceDetails to include ProcessPFMode parameter across all layers"

Kiro response:
1. Identify all files to change:
   - SSP.PureInsuranceRestAPIHandler/BaseClasses/SavePremiumFinanceDetailsCommandBase.cs
   - Web Portal/Nexus/NexusProvider.SAMForInsurance/ProviderSAMForInsuranceV2.Policy.vb
   - PureInsurance.REST/.../SavePremiumFinanceDetailsService.cs

2. Read each file to understand current state
3. Apply changes to all files in single response
4. Verify compilation after changes
```

### Codebase-Wide Context

Kiro can search entire workspace efficiently. **Use this advantage:**

**Before making changes:**
```
1. Search for existing patterns:
   "Search workspace for ProcessPFMode usage"

2. Find all implementations:
   "Find all classes implementing ISavePremiumFinanceCommand"

3. Check for similar features:
   "Search for Recovery instalment patterns"
```

### Memory File Integration

**Kiro-optimized workflow:**

```
Step 1: Load context
@workspace Read .ai/memory/architecture.md
@workspace Read .ai/memory/conventions.md
@workspace Read .ai/memory/docs/web-portal-reference.md

Step 2: Understand task
Read .aidlc/specs/[feature-name]/tasks.md

Step 3: Execute with full context
Use loaded memory context for all suggestions
```

---

## AIDLC Workflow with Kiro

### Mode A: Guided (Recommended for Kiro)

**Claim Task:**
```
User: "Claim next available task"

Kiro:
1. git checkout feature/ADO-39472-claim-recovery-instalment-portal
2. git pull origin feature/ADO-39472-claim-recovery-instalment-portal
3. Read .aidlc/specs/claim-recovery-instalment-portal/aidlc-state.md
4. Identify highest priority Available task
5. Read relevant .ai/memory/ files based on task type
6. Update aidlc-state.md: Status: Claimed | Agent: Kiro-[session-id]
7. Commit and push state files
8. Report: "Claimed Task X: [task-name]. Memory context loaded."
```

**Implement Task:**
```
User: "Implement the claimed task"

Kiro:
1. Verify on task branch (task/ADO-[id]-[task-name])
2. If not, create from integration branch
3. Read requirements.md + design.md + tasks.md
4. Apply patterns from .ai/memory/patterns.md
5. Implement across all required files (multi-file cascade)
6. Generate tests per .ai/memory/test-patterns.md
7. Verify compilation
8. Commit with descriptive message
```

**Complete Task:**
```
User: "Mark task complete"

Kiro:
1. git checkout feature/ADO-39472-claim-recovery-instalment-portal
2. git pull
3. Update aidlc-state.md: Status: Done
4. Promote newly unblocked tasks
5. If LAST task: Update .ai/memory/ files with new learnings
6. Commit state + memory updates
7. Create PR from task branch to integration branch
8. Report: "Task complete. PR created: [PR-link]"
```

---

## Memory File Workflow

### Reading Memory Files (Kiro Optimized)

**Use @workspace mentions for efficient loading:**

```
User: "I need to work on Portal changes"

Kiro: 
@workspace Read .ai/memory/architecture.md
@workspace Read .ai/memory/conventions.md  
@workspace Read .ai/memory/docs/web-portal-reference.md
@workspace Read .ai/memory/patterns.md

[Summarize key patterns and conventions]
Ready to implement Portal changes following established patterns.
```

### Updating Memory Files

**When completing LAST task in epic:**

```
User: "Update memory files for Epic #39472"

Kiro:
1. Analyze changes introduced by epic
2. Identify new patterns: "Reusing PF UI for Recovery Context"
3. Identify decisions: "ProcessPFMode parameter pattern"
4. Update relevant files:
   - .ai/memory/patterns.md
   - .ai/memory/decisions.md
   - .ai/memory/api-documentation.md
   - .ai/memory/docs/web-portal-reference.md
   - .ai/memory/glossary.md
5. Use standard format (Added date, Epic ID, Context)
6. Commit with message: "docs: Update .ai/memory for Epic #39472"
```

---

## Best Practices for Kiro

### 1. Leverage Multi-File View

**Don't:**
```
User: "Update ProviderSAMForInsuranceV2.Policy.vb"
[Make change]
User: "Now update SavePremiumFinanceDetailsCommandBase.cs"
[Make change]
User: "Now update SavePremiumFinanceDetailsService.cs"
```

**Do:**
```
User: "Update all layers to support ProcessPFMode parameter"

Kiro: [Identifies and updates all 3 files in single response]
```

### 2. Read Before Edit

**Always:**
```
1. Read target file completely before editing
2. Search for similar patterns in codebase
3. Check .ai/memory/ for established conventions
4. Verify change doesn't contradict existing architecture
```

### 3. Explain Changes

**Pattern:**
```
User: "Add ProcessPFMode to SavePremiumFinanceDetails"

Kiro:
"I'll add ProcessPFMode parameter across the stack:

1. Command DTO (Portal Handler):
   - Add ProcessPFMode property
   - Maps from Portal session state

2. Portal Provider:
   - Read Session("PFProcessMode")
   - Assign to request.ProcessPFMode

3. REST API Service:
   - Accept ProcessPFMode in command
   - Pass to GetInstalmentQuotes
   - Set TransType on business object

Changes follow pattern from .ai/memory/patterns.md: 'Context Parameter Flow'

Proceeding with edits..."
```

### 4. Validate After Changes

**Always run:**
```
1. Compilation check (if Kiro has build access)
2. Syntax validation
3. Cross-reference with .ai/memory/conventions.md
4. Report any potential issues
```

---

## Kiro Workflow Shortcuts

### Quick Commands

```
// Claim next task
"Claim next available task from aidlc-state.md"

// Load context for task
"Load memory context for [Portal/API/Database] task"

// Implement with full context
"Implement Task X using established patterns from .ai/memory/"

// Update memory after epic
"Update .ai/memory/ files with learnings from Epic #[id]"

// Create PR
"Complete task and create PR"
```

### Multi-Step Workflows

**Full Task Execution:**
```
User: "Execute Task 5 from claim-recovery-instalment-portal spec"

Kiro:
[Executes full workflow]
1. Claim task from aidlc-state.md
2. Load memory context
3. Create task branch
4. Implement changes across all layers
5. Generate tests
6. Verify compilation
7. Commit and push
8. Update aidlc-state.md
9. Create PR
10. Report completion
```

---

## Common Patterns

### Pattern 1: Portal → API → Database

```
User: "Add new field to recovery instalment plan"

Kiro workflow:
1. Read .ai/memory/data-models.md → understand PFPremiumFinance table
2. Read .ai/memory/docs/database_rules.md → learn stored proc conventions
3. Update database schema (or note it's in Epic #39336)
4. Update REST API DTO
5. Update REST API service
6. Update Portal provider
7. Update Portal UI
8. Generate tests
9. Report changes
```

### Pattern 2: Reusing Existing Components

```
User: "Create recovery instalment UI using existing Premium Finance control"

Kiro workflow:
1. Read .ai/memory/patterns.md → find "Component Reuse" section
2. Search workspace for existing instalment controls
3. Read Controls/Instalments.ascx implementation
4. Understand how to invoke via reflection (from memory files)
5. Implement wiring in PerilDetails.aspx.vb
6. Add ProcessPFMode parameter flow
7. Report pattern reuse
```

### Pattern 3: Fixing Bugs

```
User: "Fix GetSingleFinancePlan failure when saving recovery plan"

Kiro workflow:
1. Read .ai/memory/known-issues.md → check if documented
2. Search codebase for GetSingleFinancePlan usage
3. Trace call stack through Portal → API → Business layer
4. Identify missing ProcessPFMode parameter
5. Apply fix across all layers (multi-file cascade)
6. Add to .ai/memory/known-issues.md if systemic
7. Update .ai/memory/decisions.md with ADR if needed
```

---

## Integration with Other Agents

### Coordinating with GitHub Copilot

**Scenario:** GitHub Copilot claims a task, Kiro needs to help

```
Kiro workflow:
1. Read aidlc-state.md → see task claimed by "GitHub Copilot-[session]"
2. DO NOT claim same task
3. Find next Available task
4. OR: Wait for task to be Done/Available
5. Never conflict on same task
```

### Coordinating with Amazon Q

**Scenario:** Amazon Q working on backend, Kiro on frontend

```
Kiro workflow:
1. Read aidlc-state.md → see backend tasks claimed by Amazon Q
2. Claim frontend task
3. Read shared memory files for API contract
4. Implement frontend using expected API signature
5. Sync via aidlc-state.md and audit.md
```

---

## Error Recovery

### If Compilation Fails

```
Kiro workflow:
1. Read error messages
2. Check .ai/memory/conventions.md for correct patterns
3. Search codebase for working examples
4. Apply fix
5. Re-verify compilation
6. If still failing: Report to user with context
```

### If Memory Files Outdated

```
Kiro workflow:
1. Detect contradiction between memory file and requirements
2. Assume requirements.md is correct (new direction)
3. Check .ai/memory/decisions.md for ADR explaining divergence
4. If no ADR: Flag to user
5. Suggest creating new ADR in decisions.md
6. Proceed with requirements.md guidance
```

---

## Quality Gates

### Before Submitting PR

**Kiro checklist:**
- [ ] All files compile successfully
- [ ] Tests generated and pass
- [ ] Changes follow .ai/memory/conventions.md
- [ ] No hardcoded secrets
- [ ] Security checks (input validation, SQL injection prevention)
- [ ] Memory files updated (if LAST task in epic)
- [ ] aidlc-state.md updated
- [ ] audit.md entry created
- [ ] PR description references memory file sections used

---

## Help Commands

```
// Show available memory files
"List all .ai/memory/ files relevant to my task"

// Summarize memory context
"Summarize architectural patterns for [Portal/API/Database] work"

// Check task status
"Show status of all tasks in current epic"

// Validate changes
"Verify my changes against .ai/memory/conventions.md"

// Create ADR
"Create Architectural Decision Record for [decision]"
```

---

## References

- **Universal Instructions**: `.ai/instructions.md`
- **Context Memory Guide**: `.aidlc-guides/context-memory-guide.md`
- **Working Guide**: `.aidlc-guides/working-guide.md`
- **All Memory Files**: `.ai/memory/` directory

---

**Maintained By**: Kiro Users + AIDLC Governance  
**Last Updated**: 2026-05-08  
**Next Review**: 2026-08-08
