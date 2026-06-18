# Tasks — Claim Recovery Instalment Plan - Portal Workflow

**Feature**: Claim Recovery Instalment Plan - Portal Workflow
**Spec ID**: SPEC-39472
**Epic**: ADO #39472
**Source PBI**: ADO #24690
**Date**: 2026-05-07

---

## Dependency Graph

```
T1 (Portal Page Setup) ──► T2 (Instalments Button)
                                    │
                                    ├──► T3 (Instalments Tab UI)
                                    │         │
                                    │         ├──► T4 (Scheme Selection)
                                    │         │         │
                                    │         │         └──► T5 (Plan Config & Save)
                                    │         │                   │
                                    │         └──► T6 (Duplicate Prevention)
                                    │                             │
                                    └─────────────────────────────┼──► T7 (Integration Testing)
                                                                  │
                                                                  └──► T8 (Documentation)
```

---

## Task 1: Portal Page Setup and Infrastructure

**Description**: Set up the ASP.NET Web Forms page/control infrastructure for the Instalments functionality on the Recovery Receipt page. Identify the existing recovery receipt page, add the new Panel controls (`pnlInstalments`), and wire up the code-behind structure.

- **Depends on**: Epic #39336 complete
- **Blocks**: T2, T3
- **Estimate**: 3 hours
- **Story**: US-001, US-002

---

## Task 2: Instalments Button — Visibility Logic

**Description**: Add the "Instalments" button (`btnInstalments`) to the "This Receipt" tab. Implement visibility logic in Page_Load:
- Call `IsRecoveryInstalmentEnabled(claimId)` via gCLMLibrary
- If enabled: render button visible
- If not enabled: hide button entirely (not rendered)
- Button only appears during Third-Party or Salvage recovery

- **Depends on**: T1
- **Blocks**: T3, T4, T6
- **Estimate**: 3 hours
- **Story**: US-001

---

## Task 3: Instalments Tab UI — Layout and Controls

**Description**: Build the "Instalments" tab panel with all UI controls:
- `pnlInstalments` container panel
- "Select Instalment Plan" section with `ddlScheme` dropdown
- Plan Summary panel (`pnlPlanSummary`) and Plan Reference label
- Payment configuration fields: `ddlPaymentFrequency`, `ddlMediaType`, `txtPreferredDay`, `txtFirstPaymentDate` (with calendar), `ddlCurrency`
- Save button (`btnSave`)
- Wire `btnInstalments_Click` to hide "This Receipt" panel and show Instalments panel

Controls should match existing Premium Finance UI patterns (same CSS classes, layout conventions).

- **Depends on**: T1, T2
- **Blocks**: T4, T5
- **Estimate**: 5 hours
- **Story**: US-002

---

## Task 4: Scheme Selection and Plan Summary

**Description**: Implement scheme selection logic:
- On Instalments tab load: call `GetRecoveryType(clrTransactionId)` then `GetAvailableRecoverySchemes(companyNo, recoveryType)`
- Bind results to `ddlScheme`
- On `ddlScheme_SelectedIndexChanged`: call `GetPlanSummary()` and `GeneratePlanReference()` (reuse PF logic)
- Update Plan Summary panel and Plan Reference label

- **Depends on**: T3
- **Blocks**: T5
- **Estimate**: 4 hours
- **Story**: US-003

---

## Task 5: Plan Configuration, Save, and Folder Creation

**Description**: Implement the save flow:
- Validate all required fields (Payment Frequency, Media Type, Preferred Day, First Payment Date, Currency)
- Call `CreateRecoveryInstalmentPlan(clrTransactionId, schemeNo, schemeVersion, params)`
- Call folder/record creation logic (per PF pattern)
- Log to event_log via standard audit pattern
- Display success confirmation to user

Field validation should match Premium Finance validation rules.

- **Depends on**: T4
- **Blocks**: T7
- **Estimate**: 5 hours
- **Story**: US-004, US-006

---

## Task 6: Duplicate Plan Prevention

**Description**: Implement duplicate plan check:
- On `btnInstalments_Click` (before showing Instalments tab): call `ValidateNoExistingInstalmentPlan(clrTransactionId)`
- If active plan exists: display blocking message, do NOT show Instalments tab
- Message: "An active instalment plan already exists for this recovery transaction."

- **Depends on**: T2
- **Blocks**: T7
- **Estimate**: 2 hours
- **Story**: US-005

---

## Task 7: Integration Testing

**Description**: End-to-end testing of the Portal instalment creation workflow:
- Test button visibility with enabled/disabled product config
- Test tab switching (This Receipt hidden, Instalments shown)
- Test scheme selection and plan summary update
- Test save with valid data — verify plan record created in DB
- Test duplicate prevention — verify blocking message
- Test folder/record creation
- Test audit log entry
- Regression: verify existing PF plan creation unaffected
- Regression: verify existing recovery workflow unaffected

- **Depends on**: T5, T6
- **Blocks**: T8
- **Estimate**: 6 hours
- **Story**: All

---

## Task 8: Documentation and Completion

**Description**:
- Update audit.md with completion records
- Verify all acceptance criteria (AC-001 through AC-012) are met
- Update aidlc-state.md to mark all tasks Done

- **Depends on**: T7
- **Blocks**: None
- **Estimate**: 1 hour
- **Story**: All

---

## Summary

| Metric | Value |
|--------|-------|
| Total Tasks | 8 |
| Total Estimate | 29 hours |
| Critical Path | T1 → T2 → T3 → T4 → T5 → T7 → T8 (27 hours) |
| Max Parallelism | T6 can run parallel with T3–T5 |
| Prerequisite | Epic #39336 must be complete |

---

*Generated by AI-SDLC INCEPTION phase. Traceability: requirements.md → user-stories.md → design.md → tasks.md*
