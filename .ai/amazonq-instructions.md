# Amazon Q Agent Instructions

**Agent**: Amazon Q Developer Agent  
**Project**: Pure Insurance  
**Last Updated**: 2026-05-08

---

## Quick Start

**READ FIRST**: [`.ai/instructions.md`](.ai/instructions.md) — Universal AI agent instructions

This file contains **Amazon Q-specific** optimizations and workflow patterns.

---

## Amazon Q-Specific Capabilities

### /dev Mode for Feature Implementation

Amazon Q's `/dev` mode excels at **autonomous feature implementation**. Use for:
- Complete task implementation from spec
- Multi-file feature development
- End-to-end workflow implementation (Portal → API → Database)

**Pattern:**
```
User: /dev Implement Task 5: Plan Configuration & Save

Amazon Q:
1. Read .aidlc/specs/claim-recovery-instalment-portal/tasks.md
2. Read .aidlc/specs/claim-recovery-instalment-portal/design.md
3. Load .ai/memory/ context files (architecture, conventions, patterns)
4. Generate implementation plan
5. Create all required files/edits
6. Generate tests
7. Verify compilation
8. Present plan for approval
9. Execute on approval
```

### Workspace-Wide Understanding

Amazon Q maintains **persistent workspace context**. Leverage this:

**Avoid re-explaining:**
- Project structure (Amazon Q learns it)
- Technology stack (.NET Framework 4.8, ASP.NET Web Forms, etc.)
- Common patterns (already indexed)

**Do provide:**
- Specific task requirements
- Links to relevant memory files
- Acceptance criteria

### Code Review & Security

Amazon Q has strong **security scanning**. Use this:

**After implementation:**
```
User: "Review changes for security issues"

Amazon Q:
- Scans for SQL injection vulnerabilities
- Checks for hardcoded credentials
- Identifies XSS risks
- Validates input sanitization
- Reports findings with remediation
```

---

## AIDLC Workflow with Amazon Q

### Mode B: Autonomous (Recommended for Amazon Q)

Amazon Q is optimized for **direct ADO task assignment** (Mode B).

**Task Execution:**
```
User: "Complete ADO Task #12345"

Amazon Q /dev mode:
1. Query ADO via MCP for task details
2. Read .aidlc/specs/[feature-name]/tasks.md
3. Find task by ADO ID
4. Load .ai/memory/ context:
   - architecture.md
   - conventions.md
   - task-specific files (API/Portal/Database)
   - decisions.md
   - known-issues.md
5. Read requirements.md + design.md
6. Generate implementation plan
7. Present plan for approval
8. On approval:
   - Create task branch
   - Implement across all files
   - Generate tests
   - Verify build
   - Update aidlc-state.md
   - Update ADO task status
   - Create PR
```

---

## Memory File Workflow

### Reading Memory Files (Amazon Q Optimized)

**Explicit loading for /dev mode:**

```
User: /dev I need to implement Portal changes for recovery instalments

Amazon Q:
Reading required context...

✓ .ai/memory/architecture.md
  - Portal layer: ASP.NET Web Forms
  - API layer: .NET Core REST with MediatR
  - Database layer: SQL Server stored procedures

✓ .ai/memory/conventions.md
  - VB.NET: PascalCase, m_ prefix for private fields
  - Session variables: CN* pattern for claims

✓ .ai/memory/docs/web-portal-reference.md
  - Portal → API pattern: ProviderSAMForInsurance
  - Session state management
  - Control reuse patterns

✓ .ai/memory/patterns.md
  - "Reusing Premium Finance UI" pattern found
  - ProcessPFMode parameter flow pattern

Context loaded. Ready to implement.
What specific task should I execute?
```

### Updating Memory Files

**Amazon Q autonomous update:**

```
User: /dev Update memory files for completed Epic #39472

Amazon Q:
Analyzing epic changes...

New patterns identified:
1. Reusing PF UI for non-PF contexts (recovery instalments)
2. ProcessPFMode parameter flow through stack
3. Session-based context switching

New decisions identified:
1. Why ProcessPFMode is required (ADR-042)
2. Why bypass receipt validators for instalments
3. Transaction code mapping (INC→ICC, IND→ICD)

Files to update:
- .ai/memory/patterns.md (add 3 new sections)
- .ai/memory/decisions.md (add 3 ADRs)
- .ai/memory/api-documentation.md (document ProcessPFMode)
- .ai/memory/docs/web-portal-reference.md (add Recovery Instalment pattern)
- .ai/memory/glossary.md (add ICC, ICD, ProcessPFMode terms)

Proceed with updates? [y/n]
```

---

## Best Practices for Amazon Q

### 1. Use /dev Mode for Complete Features

**Prefer:**
```
User: /dev Implement Task 5: Plan Configuration & Save from claim-recovery-instalment-portal spec

[Amazon Q handles entire workflow autonomously]
```

**Over:**
```
User: "Update ProviderSAMForInsuranceV2.Policy.vb"
User: "Now update the command class"
User: "Now update the service"
[Manual step-by-step]
```

### 2. Provide Memory Context Upfront

**Pattern:**
```
User: /dev Implement [task]

Context:
- Read .ai/memory/architecture.md
- Read .ai/memory/docs/web-portal-reference.md
- Follow pattern from .ai/memory/patterns.md: "Reusing PF UI"
- Check .ai/memory/decisions.md ADR-042 for ProcessPFMode guidance
```

### 3. Leverage Security Scanning

**Always request:**
```
User: /dev After implementation, scan for:
- SQL injection risks
- XSS vulnerabilities
- Hardcoded credentials
- Input validation gaps
```

### 4. Use ADO Integration

**Direct task assignment:**
```
User: /dev Complete ADO Task #12345

[Amazon Q queries ADO directly via MCP]
[Reads task details, acceptance criteria, links]
[Implements with full context]
```

---

## Amazon Q Workflow Patterns

### Pattern 1: Full Feature Implementation

```
User: /dev Implement Epic #39472 Task 5

Amazon Q:
[Step 1] Reading specs...
- .aidlc/specs/claim-recovery-instalment-portal/requirements.md
- .aidlc/specs/claim-recovery-instalment-portal/design.md
- .aidlc/specs/claim-recovery-instalment-portal/tasks.md

[Step 2] Loading memory context...
- .ai/memory/architecture.md (system layers)
- .ai/memory/conventions.md (coding standards)
- .ai/memory/docs/web-portal-reference.md (Portal patterns)
- .ai/memory/docs/rest-api-reference.md (API patterns)

[Step 3] Analyzing task...
Task: Plan Configuration & Save
- Requires: Payment validation, scheme selection, plan save
- Files to modify: PayClaim.ascx, PayClaim.ascx.vb, Instalments.ascx.vb
- Tests required: Per .ai/memory/test-patterns.md

[Step 4] Implementation plan...
1. Add btnSaveInstalmentPlan button to PayClaim.ascx
2. Wire btnSaveInstalmentPlan_Click in PayClaim.ascx.vb
3. Validate SelectedInstalmentQuote
4. Call ucInstalments.SaveInstallmentPlan()
5. Handle success/error states
6. Generate unit tests

Proceed? [y/n]

[On y: Executes full implementation]
```

### Pattern 2: API Contract Implementation

```
User: /dev Implement SavePremiumFinanceDetails with ProcessPFMode parameter

Amazon Q:
[Reads .ai/memory/api-documentation.md]
[Reads .ai/memory/patterns.md for "API Parameter Flow"]

Implementation plan:
1. Command DTO: Add ProcessPFMode property
2. Service: Accept and validate ProcessPFMode
3. Pass to GetInstalmentQuotes
4. Set TransType on business object
5. Update API docs in .ai/memory/

Tests:
- Unit test: ProcessPFMode = "SR" sets TransType = "SR"
- Unit test: ProcessPFMode = "TPR" sets TransType = "TPR"
- Integration test: End-to-end recovery plan creation

Proceed? [y/n]
```

### Pattern 3: Database Changes

```
User: /dev Verify database schema supports Epic #39472

Amazon Q:
[Reads .ai/memory/data-models.md]
[Reads .ai/memory/docs/database_knowledge.md]

Schema verification:
✓ PFPremiumFinance.TransType column exists (varchar)
✓ PFPremiumFinance.claim_recovery_transaction_id exists (int, nullable)
✓ pfinstalments_transaction table has ICC (ID 8), ICD (ID 9)
✓ spu_ACT_Import_Update_Instalment_Transaction_Code has SR/TPR logic

⚠ Enhancement opportunity:
- DocumentType table missing ICC/ICD codes (IDs 56, 57)
- Add if document generation is in scope

Report complete. Proceed with implementation? [y/n]
```

---

## Amazon Q + AIDLC Integration

### Task Claiming (Mode B)

```
User: /dev Claim and implement next available task

Amazon Q:
1. Read .aidlc/specs/[feature]/aidlc-state.md
2. Find highest priority Available task
3. Update Status: Claimed | Agent: Amazon Q
4. Commit and push state file
5. Load memory context for task type
6. Present implementation plan
7. On approval: Execute full workflow
```

### Task Completion

```
Amazon Q autonomous completion:
1. Verify all acceptance criteria met
2. Run tests (all pass)
3. Build verification (compiles successfully)
4. Update aidlc-state.md: Status: Done
5. Promote newly unblocked tasks
6. Update .ai/memory/ files (if LAST task)
7. Commit state + memory updates
8. Update ADO task to Done via MCP
9. Create PR with detailed description
10. Report: "Task complete. PR #123 created."
```

---

## Quality Automation

### Pre-Implementation Checks

Amazon Q automatically:
- [ ] Reads memory files for context
- [ ] Validates against conventions
- [ ] Checks for existing patterns
- [ ] Reviews ADRs for guidance
- [ ] Searches for similar implementations

### Post-Implementation Checks

Amazon Q automatically:
- [ ] Runs compilation verification
- [ ] Executes unit tests
- [ ] Scans for security vulnerabilities
- [ ] Validates against acceptance criteria
- [ ] Checks code coverage
- [ ] Updates memory files (if needed)

---

## Error Recovery

### Compilation Failures

```
Amazon Q:
Compilation failed:
- Error CS0246: Type 'ProcessPFMode' not found

Analysis:
- Checking .ai/memory/conventions.md for namespace conventions
- Searching workspace for ProcessPFMode definition
- Found: Should be string property, not separate type

Fix:
- Change ProcessPFMode declaration from custom type to string
- Re-compile

Status: ✓ Compilation successful
```

### Test Failures

```
Amazon Q:
Test failed: SavePremiumFinanceDetails_WithSR_SetsTransTypeSR

Analysis:
- Expected: TransType = "SR"
- Actual: TransType = "PF" (default)
- Root cause: ProcessPFMode not passed to business layer

Fix:
- Update SavePremiumFinanceDetailsService.cs line 246
- Add: _bSIRPremiumFinanceBusiness.TransType = request.ProcessPFMode

Status: ✓ All tests passing
```

---

## Coordination with Other Agents

### With GitHub Copilot

```
Scenario: Copilot started task, Amazon Q needs to complete

Amazon Q:
1. Read aidlc-state.md → see "Claimed | Agent: GitHub Copilot"
2. Wait for status change to Available or Done
3. Do NOT override Copilot's claim
4. Find alternative Available task
5. Or: Alert user to coordination issue
```

### With Kiro

```
Scenario: Kiro working on frontend, Amazon Q on backend

Amazon Q:
1. Claim backend task (API layer)
2. Read .ai/memory/api-documentation.md for contract
3. Implement backend with documented API signature
4. Update .ai/memory/api-documentation.md with new endpoints
5. Commit so Kiro can read updated API docs
6. Coordinate via aidlc-state.md and audit.md
```

---

## Advanced Features

### Chain of Thought

Amazon Q shows reasoning:
```
User: /dev Why did you choose this implementation?

Amazon Q:
Decision chain:
1. Read requirements.md → reuse Premium Finance UI
2. Check .ai/memory/patterns.md → found "Component Reuse" pattern
3. Search workspace → found Controls/Instalments.ascx
4. Read Instalments.ascx.vb → identified bindInstalments method
5. Check .ai/memory/decisions.md → ADR-040 recommends reflection for cross-context calls
6. Implemented: btnInstalments_Click calls bindInstalments via reflection

Rationale: Follows established pattern, zero UI duplication, consistent with ADR-040
```

### Diff Explanation

Amazon Q explains changes:
```
User: /dev Explain your changes to ProviderSAMForInsuranceV2.Policy.vb

Amazon Q:
Changes in ProviderSAMForInsuranceV2.Policy.vb (lines 18032-18046):

Added:
```vb
If Current.Session("PFProcessMode") IsNot Nothing Then
    .ProcessPFMode = CStr(Current.Session("PFProcessMode"))
End If
```

Rationale:
- Session("PFProcessMode") set by PerilDetails.aspx.vb (line 290: "SR" or line 292: "TPR")
- Must flow to REST API for scheme filtering and TransType mapping
- Follows pattern from .ai/memory/patterns.md: "Session-Based Context Flow"
- Without this: GetSingleFinancePlan fails (known issue from .ai/memory/known-issues.md)
```

---

## Help Commands

```
// Show task status
/dev Show status of Epic #39472 tasks

// Load memory context
/dev Load memory files for Portal development

// Validate changes
/dev Review my changes against .ai/memory/conventions.md

// Security scan
/dev Scan for security vulnerabilities

// Update memory
/dev Update .ai/memory/ files with learnings from Epic #39472

// Create ADR
/dev Create Architectural Decision Record for ProcessPFMode pattern
```

---

## References

- **Universal Instructions**: `.ai/instructions.md`
- **Context Memory Guide**: `.aidlc-guides/context-memory-guide.md`
- **Working Guide**: `.aidlc-guides/working-guide.md`
- **All Memory Files**: `.ai/memory/` directory
- **Amazon Q Docs**: https://aws.amazon.com/q/developer/

---

**Maintained By**: Amazon Q Users + AIDLC Governance  
**Last Updated**: 2026-05-08  
**Next Review**: 2026-08-08
