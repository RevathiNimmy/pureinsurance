# Architecture Decision Records (ADRs) — Pure Insurance

This file documents major architectural and technical decisions. Each record explains the context, decision, and consequences.

**Last Updated**: 2026-04-28
**Maintained By**: Pure Insurance Team

---

## Format for New ADRs

```
## ADR-NNN: [Decision Title]

**Date**: [YYYY-MM-DD]
**Status**: Proposed / Accepted / Deprecated
**Context**: [What problem or situation prompted this decision?]
**Decision**: [What decision was made?]
**Consequences**: [What are the positive and negative impacts?]
**Alternatives Considered**: [What other options were evaluated?]
**Related ADRs**: [Links to related decisions]
```

---

## ADR-001: Adopting AI-SDLC Framework

**Date**: 2026-04-28
**Status**: Accepted
**Context**:
The Pure Insurance team identified the need for a structured, traceable approach to AI-assisted software development. Ad-hoc use of AI tools was leading to inconsistent outputs, missing tests, and no audit trail of AI-driven changes. A framework was needed to enforce quality gates, maintain human oversight, and integrate with the existing Azure DevOps work tracking system.

**Decision**:
Adopt the AI-SDLC framework (via the `hve-core` extension) for all structured feature development. This introduces:

- `.aidlc/specs/{feature}/` — per-feature specs (requirements, design, tasks, state, audit)
- `.ai/` — persistent project context for AI agents (memory, rules, context)
- Integration branch strategy: `feature/ADO-[epic-id]-[feature-name]` for coordination, `task/ADO-[id]-[task-name]` for implementation
- ADO (Azure DevOps) as the work tracking system under organisation `SSP-Insurer`, project `Pure Insurance`
- Required approvals for architecture and security decisions before implementation

**Consequences**:

*Positive*:
- Consistent spec-driven development with traceability from ADO epic to code
- AI agents guided by stable project context (architecture, conventions, rules)
- Audit trail of all AI-driven changes via `audit.md`
- Human approval gates at architecture and security checkpoints

*Negative*:
- Overhead of spec creation before implementation begins
- Requires ADO MCP to be available to create integration branches
- Teams must learn the framework workflow

**Alternatives Considered**:
- Unstructured AI assistance: Lower overhead but no quality controls or traceability
- GitHub Issues-based workflow: Not aligned with existing ADO investment

**Related ADRs**: None

---

## ADR-002: Stored Procedures as the Sole Data Access Pattern

**Date**: (Existing — documented retrospectively)
**Status**: Accepted
**Context**:
Pure Insurance has a long-established policy of accessing the SQL Server database exclusively through stored procedures. This pre-dates modern ORM tooling and was adopted to centralise database logic, enforce security via SQL Server permissions, and enable DBA-controlled schema management.

**Decision**:
All database access from application code must go through SQL Server stored procedures via `dPMDAO` or `dPMDAOBridge`. Inline SQL strings are prohibited in application code.

**Consequences**:

*Positive*:
- SQL Server permissions can restrict direct table access
- Stored procedures are versioned and deployed as part of the database change process
- Consistent parameterised access prevents SQL injection at the data layer

*Negative*:
- Schema changes require coordination between application and database teams
- Cross-service queries and reporting require careful stored procedure design
- ORM tooling (Entity Framework, Dapper) is not used

**Alternatives Considered**:
- ORM (Entity Framework): Rejected due to existing codebase size and DBA oversight requirements

**Related ADRs**: None

---

## ADR-003: VB.NET as Primary Application Language

**Date**: (Existing — documented retrospectively)
**Status**: Accepted
**Context**:
Pure Insurance was originally built in VB.NET and has accumulated 996 VB projects over many years. The team has deep VB.NET expertise and the codebase is stable.

**Decision**:
VB.NET / .NET Framework 4.8 remains the primary language for WinForms client components. New shared libraries and integration components (REST API handler, AWS integration) use C# targeting .NET Standard 2.0 for cross-compatibility.

**Consequences**:

*Positive*:
- No language migration risk for the large existing codebase
- Team expertise is concentrated in VB.NET
- C# is used where modern library support requires it (AWS SDK, Azure Identity)

*Negative*:
- VB.NET has limited community investment compared to C#
- Some modern .NET features (records, nullable reference types) have limited or no VB.NET support
- AI tooling has stronger C# support

**Alternatives Considered**:
- Full migration to C#: Prohibitively expensive given 996 VB projects
- .NET 6+ migration: Deferred — WinForms .NET 4.8 is stable and supported until 2032

**Related ADRs**: ADR-001 (AI-SDLC adoption assumes C# preference; agents must respect VB.NET primary language)
