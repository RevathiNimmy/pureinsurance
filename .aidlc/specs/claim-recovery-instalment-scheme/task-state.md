# Task State Tracking — Claim Recovery Instalment Scheme

**Feature**: Instalment for Claim Recovery - Scheme Configuration  
**Epic**: ADO #39336  
**Integration Branch**: `feature/ADO-39336-claim-recovery-instalment-scheme`  
**Last Updated**: 2026-05-06

---

## Task Status Summary

| Task ID | Title | Status | Assigned To | Priority | Estimate |
|---------|-------|--------|-------------|----------|----------|
| T1 | PFScheme Table — Add scheme_type Column | Claimed | Amazon Q (Session 1) | P0 | 2h |
| T2 | RiskType Table — Add recovery_instalments_enabled Column | Available | — | P0 | 2h |
| T3 | PFRate Table — Add transaction_type Column | Available | — | P0 | 2h |
| T4 | Modify Existing spu_PF* Stored Procedures | Blocked | — | P1 | 4h |
| T5 | Create New Stored Procedures | Blocked | — | P1 | 4h |
| T6 | Product Risk Maintenance — UI and Business Logic | Blocked | — | P1 | 4h |
| T7 | Instalment Scheme Maintenance — Scheme Type Dropdown | Blocked | — | P1 | 4h |
| T8 | Instalment Rates — Transaction Type Configuration | Blocked | — | P1 | 4h |
| T9 | Claims Recovery Instalment Logic | Blocked | — | P2 | 8h |
| T10 | Navigator XM Roadmap Update | Blocked | — | P2 | 3h |
| T11 | Duplicate Instalment Plan Prevention | Blocked | — | P2 | 2h |
| T12 | Regression Testing — Premium Finance | Blocked | — | P2 | 4h |
| T13 | Integration Testing — End-to-End | Blocked | — | P3 | 6h |
| T14 | Documentation and Audit Completion | Blocked | — | P3 | 2h |

---

## Dependency Graph

```
T1 (DB Schema) ──────┬──► T3 (Stored Procs)
                      │         │
                      │         ├──► T5 (Scheme Maint UI)
                      │         │         │
                      │         ├──► T6 (Rates UI)
                      │         │         │
T2 (RiskType Schema)──┤         └──► T7 (Claims Recovery Logic)
                      │                   │
                      ├──► T4 (Product Risk UI)    │
                      │         │         │
                      │         └─────────┼──► T8 (Navigator XM)
                      │                   │
                      │                   └──► T9 (Duplicate Prevention)
                      │
                      └──► T10 (Regression Tests)
                                          │
T7 + T8 + T9 ────────────────────────────► T11 (Integration Testing)
                                          │
T11 ──────────────────────────────────────► T12 (Documentation)
```

---

## Execution Log

### Session 1 — 2026-05-06

**Agent**: Amazon Q  
**Start Time**: 2026-05-06T14:30:00Z  
**Status**: In Progress

#### Actions Taken
- [x] Checked out integration branch `feature/ADO-39336-claim-recovery-instalment-scheme`
- [x] Read spec files (requirements, design, tasks)
- [x] Reviewed architecture and conventions
- [x] Created task-state.md for tracking

#### Next Steps
1. Claim T1 (PFScheme schema change)
2. Implement database migration script
3. Commit to integration branch
4. Move to T2

---

## Notes

- **Critical Path**: T1 → T4 → T9 → T13 → T14 (24 hours)
- **Max Parallelism**: 6 tasks can run simultaneously after T4/T5 complete
- **Database Changes**: All in `Databases/After Change/` directory
- **Backward Compatibility**: All changes must preserve existing Premium Finance functionality
