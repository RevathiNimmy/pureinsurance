# Tasks — Instalment for Claim Recovery - Scheme Configuration

**Feature**: Instalment for Claim Recovery - Scheme Configuration
**Spec ID**: SPEC-39336
**Epic**: ADO #39336
**Date**: 2026-05-06

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

## Layer 1: Foundation (Database)

### Task 1: PFScheme Table — Add scheme_type Column
**Description**: Add `scheme_type tinyint NOT NULL DEFAULT 1` column to PFScheme table with CHECK constraint `(scheme_type IN (1, 2))`. Value 1 = Premium Finance, 2 = Claim Recovery. Create migration script in `Databases/After Change/`.

- **Depends on**: None
- **Blocks**: T3, T5, T6, T7, T10
- **Parallelisable**: Yes (with T2)
- **Estimate**: 2 hours
- **Story**: US-002

---

### Task 2: Risk_Type Table — Add recovery_instalments_enabled Column
**Description**: Add `recovery_instalments_enabled tinyint NOT NULL DEFAULT 0` column to `Risk_Type` table. Create migration script in `Databases/Pure/Structure/PURE_STRUCTURE.sql`.

- **Depends on**: None
- **Blocks**: T3, T4, T7, T10
- **Parallelisable**: Yes (with T1)
- **Estimate**: 2 hours
- **Story**: US-001

---

### Task 3: PFRF Table — Add transaction_type Column
**Description**: Add `transaction_type tinyint NULL` column to PFRF table (rates table) with CHECK constraint `(transaction_type IS NULL OR transaction_type IN (1, 2))`. NULL = Premium Finance, 1 = Salvage Recovery, 2 = Third-Party Recovery. Create migration script.

- **Depends on**: None
- **Blocks**: T4, T6, T7, T10
- **Parallelisable**: Yes (with T1, T2)
- **Estimate**: 2 hours
- **Story**: US-003

---

### Task 3a: PFPremiumFinance Table — Add claim_recovery_transaction_id Column
**Description**: Add `claim_recovery_transaction_id INT NULL` column to PFPremiumFinance table to link instalment plans to CLR recovery transactions. NULL = Premium Finance instalment (backward compatible). Create migration script.

- **Depends on**: None
- **Blocks**: T5, T9, T11
- **Parallelisable**: Yes (with T1, T2, T3)
- **Estimate**: 2 hours
- **Story**: US-004

---

## Layer 2: Data Access (Stored Procedures)

### Task 4: Modify Existing spu_PF* Stored Procedures
**Description**: Extend existing Premium Finance stored procedures with `@SchemeType tinyint = 1` parameter:
- `spu_PF_Scheme_Sel` — add WHERE `scheme_type = @SchemeType`
- `spu_PF_Scheme_Add` — include `@SchemeType` in INSERT
- `spu_PF_Scheme_Upd` — include `@SchemeType` in UPDATE
- `spu_PF_Rate_Sel` — add optional `@TransactionType tinyint = NULL` filter

Default values ensure backward compatibility.

- **Depends on**: T1, T2, T3, T3a
- **Blocks**: T5, T6, T7, T10
- **Parallelisable**: No
- **Estimate**: 4 hours
- **Story**: US-006

---

### Task 5: Create New Stored Procedures
**Description**: Create new stored procedures:
- `spu_PF_Scheme_SelByType` — select schemes by company_no + scheme_type
- `spu_PF_Rate_SelByTransType` — select rates by scheme + transaction_type
- `spu_CLR_RecoveryInstalmentPlan_Validate` — check active instalment plan exists for CLR transaction
- `spu_RiskType_GetRecoveryInstalmentFlag` — get recovery_instalments_enabled for a risk_type_id

Follow existing procedure conventions (DDLDropProcedure, SET QUOTED_IDENTIFIER OFF, PascalCase params).

- **Depends on**: T1, T2, T3, T3a
- **Blocks**: T6, T7, T9, T11
- **Parallelisable**: Yes (with T4)
- **Estimate**: 4 hours
- **Story**: US-002, US-004, US-005

---

## Layer 3: Business Logic & UI

### Task 6: Product Risk Maintenance — UI and Business Logic
**Description**: Add "Recovery Receipts on Instalments" checkbox to the "2-Claim" tab (`_tabMainTab_TabPage1`) of Product Risk Maintenance, inside `Frame4` ("Claim Payment" GroupBox):
- **Form**: `Sirius For Underwriting\Components\Product\Interface\iPMUProduct\iPMUProductFrm.vb`
- **Designer**: `Sirius For Underwriting\Components\Product\Interface\iPMUProduct\iPMUProductFrm.Designer.vb`
- **Constants**: `Sirius For Underwriting\Components\Product\Interface\iPMUProduct\iPMUProductMod.vb`
- UI: Add `chkRecoveryInstalmentsEnabled` checkbox control following `chkPaymentCannotExceedReserve` pattern
- Business: Add `m_vRecoveryInstalmentsEnabled` field, load/save via `ACICPSel`/`ACICPUpd` constants
- Data access: Extend existing product select/update stored procedures with `recovery_instalments_enabled` parameter
- Audit: Log changes to event_log

- **Depends on**: T4, T5
- **Blocks**: T8, T11
- **Parallelisable**: Yes (with T7, T8)
- **Estimate**: 4 hours
- **Story**: US-001

---

### Task 7: Instalment Scheme Maintenance — Scheme Type Dropdown
**Description**: Add "Scheme Type" dropdown to Tab 1 (Scheme) of Instalment Scheme Maintenance:
- UI: Add dropdown with options "Premium Finance" (1), "Claim Recovery" (2)
- Business: `LoadSchemeType()`, `SaveSchemeType()`
- Data access: Use modified `spu_PF_Scheme_Add/Upd` with @SchemeType parameter
- Ensure existing schemes display as "Premium Finance"

- **Depends on**: T4, T5
- **Blocks**: T8, T11
- **Parallelisable**: Yes (with T6, T8)
- **Estimate**: 4 hours
- **Story**: US-002

---

### Task 8: Instalment Rates — Transaction Type Configuration
**Description**: When Scheme Type = "Claim Recovery", show Transaction Type selector on Rates tab:
- UI: Add Transaction Type dropdown ("Salvage Recovery", "Third-Party Recovery")
- Business: `LoadTransactionTypes()`, `SaveRateByTransactionType()`, `GetRateByTransactionType()`
- Data access: Use `spu_PF_Rate_SelByTransType` and modified rate save procedures
- Rates use same fields as Premium Finance (interest rate, admin fee, etc.)

- **Depends on**: T4, T5
- **Blocks**: T9, T11
- **Parallelisable**: Yes (with T6, T7)
- **Estimate**: 4 hours
- **Story**: US-003

---

### Task 9: Claims Recovery Instalment Logic
**Description**: Implement the core recovery instalment creation logic in Claims Management (gCLMLibrary / bCLMRecovery):
- `IsRecoveryInstalmentEnabled(claimId)` — check product config via claim → policy → product → RiskType
- `GetRecoveryType(clrTransactionId)` — read `recovery_type` from CLR transaction
- `GetAvailableRecoverySchemes(companyNo, recoveryType)` — retrieve matching schemes
- `CreateRecoveryInstalmentPlan(clrTransactionId, schemeNo, schemeVersion)` — create plan with correct rates
- Orchestration flow per design.md section 2.3

- **Depends on**: T4, T5
- **Blocks**: T10, T11
- **Parallelisable**: Yes (with T6, T7, T8)
- **Estimate**: 8 hours
- **Story**: US-004

---

### Task 10: Navigator XM Roadmap Update
**Description**: Modify existing claims recovery Navigator XM roadmap XML to include instalment plan creation step:
- Add new step/option in the roadmap for recovery instalment creation
- Conditional visibility based on product config (recovery_instalments_enabled)
- Link to the recovery instalment creation screen/workflow

- **Depends on**: T6, T7
- **Blocks**: T11
- **Parallelisable**: Yes (with T8, T9)
- **Estimate**: 3 hours
- **Story**: US-007

---

### Task 11: Duplicate Instalment Plan Prevention
**Description**: Implement validation to block creation of a second instalment plan for a recovery that already has an active plan:
- Call `spu_CLR_RecoveryInstalmentPlan_Validate` before scheme selection
- If active plan exists, display blocking message to user
- Inform user they can modify existing plan via Instalment Plan Maintenance

- **Depends on**: T5, T9
- **Blocks**: T11
- **Parallelisable**: Yes (with T10)
- **Estimate**: 2 hours
- **Story**: US-005

---

## Layer 4: Testing

### Task 12: Regression Testing — Premium Finance
**Description**: Verify existing Premium Finance functionality is unaffected:
- Test scheme selection in PF workflows (only PF schemes appear)
- Test rate lookup in PF workflows (unchanged behaviour)
- Test instalment creation in PF workflows (unchanged behaviour)
- Verify extended stored procedures with default params return same results as before

- **Depends on**: T4, T5
- **Blocks**: T13
- **Parallelisable**: Yes (with T6–T11)
- **Estimate**: 4 hours
- **Story**: US-006

---

### Task 13: Integration Testing — End-to-End
**Description**: Full end-to-end testing of the Claim Recovery instalment workflow:
- Enable product → create Claim Recovery scheme → configure rates → create recovery instalment plan
- Test Salvage recovery type flow
- Test Third-Party recovery type flow
- Test duplicate prevention
- Test scheme type separation (no cross-contamination)
- Verify audit trail entries

- **Depends on**: T6, T7, T8, T9, T10, T11, T12
- **Blocks**: T14
- **Parallelisable**: No
- **Estimate**: 6 hours
- **Story**: All

---

## Layer 5: Documentation & Completion

### Task 14: Documentation and Audit Completion
**Description**: 
- Update audit.md with completion records
- Verify all acceptance criteria (AC-001 through AC-017) are met
- Security baseline compliance check (SECURITY rules verification)
- Update aidlc-state.md to mark all tasks Done

- **Depends on**: T13
- **Blocks**: None
- **Parallelisable**: No
- **Estimate**: 2 hours
- **Story**: US-008

---

## Summary

| Metric | Value |
|--------|-------|
| Total Tasks | 15 |
| Total Estimate | 53 hours |
| Critical Path | T1 → T3a → T4 → T9 → T13 → T14 (26 hours) |
| Max Parallelism | 6 tasks (T6, T7, T8, T9, T10, T12 can run simultaneously after T4/T5) |
| Foundation Tasks | 4 (T1, T2, T3, T3a) |
| Data Access Tasks | 2 (T4, T5) |
| Business/UI Tasks | 6 (T6–T11) |
| Testing Tasks | 2 (T12, T13) |
| Documentation | 1 (T14) |
