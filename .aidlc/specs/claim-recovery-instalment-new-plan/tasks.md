# Tasks — Instalment for Claim Recovery - New Plan

**Feature**: Instalment for Claim Recovery - New Plan
**Spec ID**: SPEC-39489
**Epic**: ADO #39489
**Source PBI**: ADO #37524
**Date**: 2026-05-07
**Revised**: 2026-05-07 — Corrected to reflect actual REST API microservices architecture (PureInsurance.REST repo, CQRS + MediatR pattern). Previous version incorrectly assumed WCF/SAM as Portal backend.
**Revised**: 2026-06-04 — Added T5 (API: Claim Recovery Schemes endpoint) to close FR-015 gap. Codebase inspection confirmed no `GET /claims/recovery/schemes` exists in PureInsurance.REST. Renumbered: old T5->T6, T6->T7, T7->T8; Portal old T8->T9, T9->T10, T10->T11, T11->T12, T12->T13; old T13->T14, T14->T15. Total tasks: 15.

---

## Dependency Graph

```
T1 (DB: ICC/ICD migration) ──────────────────────────────────────────────┐
                                                                          │
T2 (DB: Eligible Transactions SP) ───────────────────────────────────────┤
                                                                          │
T3 (DB: DocRef Grouping SP) ─────────────────────────────────────────────┤
                                                                          ▼
T4 (DB: Plan Maintenance SP) ───────────────────────────────► T6 (API: Eligible Transactions endpoint)
                                                                          │
                                              T5 (API: Schemes endpoint)──┤
                                                                          │
                                                               T7 (API: Create Instalment Plan endpoint)
                                                                          │
                                                               T8 (API: Plan Maintenance endpoint)
                                                                          │
                                              ┌───────────────────────────┤
                                              ▼                           ▼
                                   T10 (Portal: PlanTransactions page)  T11 (Portal: InstalmentRecovery – Scheme)
                                              │                           │
                                              └───────────┬───────────────┘
                                                          ▼
                                             T12 (Portal: InstalmentRecovery – Tabs + Bank + Save)
                                                          │
                                             T13 (Portal: Plan Maintenance UI)
                                                          │
                                                          ▼
                                                   T14 (Integration Testing)
                                                          │
                                                          ▼
                                                   T15 (Documentation)
```

Note: T9 (Portal: Finance Menu "New Plan" Entry) depends on nothing and blocks T10.

---

## Layer 1: Database

### Task 1: Migration Script — ICC and ICD Transaction Types
**Description**: Create migration script to add ICC (Instalment Claim Credit) and ICD (Instalment Claim Debit) transaction types to the `transaction_type` table. Use existing INC and IND records as column templates. Place in `Databases/After Change/`.

- **FR coverage**: FR-035, FR-036
- **Depends on**: None
- **Blocks**: T7
- **Parallelisable**: Yes (with T2, T3, T4)
- **Estimate**: 2 hours
- **Story**: US-005

---

### Task 2: Stored Procedure — Eligible Recovery Transactions Search
**Description**: Create `spu_CLR_EligibleRecoveryTransactions_Sel`:
- Parameters: `@PartyCode varchar`, `@ClaimNo varchar = NULL`
- Joins: transaction → claim → policy → risk_type
- Filters: `recovery_instalments_enabled = 1`, `outstanding_amount > 0`, no active instalment plan
- Returns: document_ref, claim_number, party, transaction_date, transaction_amount, outstanding_amount, transaction_id, recovery_type, product_code, branch_code

- **FR coverage**: FR-007, FR-009, FR-041
- **Depends on**: None (uses schema from Epic #39336)
- **Blocks**: T6
- **Parallelisable**: Yes (with T1, T3, T4)
- **Estimate**: 3 hours
- **Story**: US-001

---

### Task 3: Stored Procedure — Transactions by Document Reference
**Description**: Create `spu_CLR_TransactionsByDocRef_Sel`:
- Parameters: `@DocumentRef varchar`, `@PartyCode varchar`
- Returns: all transaction_ids sharing the same document_ref for the party

- **FR coverage**: FR-012
- **Depends on**: None
- **Blocks**: T6
- **Parallelisable**: Yes (with T1, T2, T4)
- **Estimate**: 1 hour
- **Story**: US-002

---

### Task 4: Stored Procedure — Plan Maintenance Claim Number Search
**Description**: Extend existing Plan Maintenance search stored procedure (or create `spu_PlanMaintenance_SelByClaimNo`) to accept optional `@ClaimNo` parameter, joining instalment plan → source transaction → claim.

- **FR coverage**: FR-040
- **Depends on**: None
- **Blocks**: T8
- **Parallelisable**: Yes (with T1, T2, T3)
- **Estimate**: 2 hours
- **Story**: US-006

---

## Layer 2: REST API — Claims Microservice (`PureInsurance.REST`)

All API tasks follow the CQRS + MediatR pattern in `MicroServices/Claims/`. Each task requires: Domain model, Repository, Query/Command + Handler + Validator, Service, Controller endpoint, and Tests.

### Task 5: API — Claim Recovery Schemes Endpoint
**Description**: Implement `GET /claims/recovery/schemes` in Claims microservice. Codebase inspection confirmed this endpoint does NOT exist — it must be created here (not inherited from Epic #39472).

- Domain: `ClaimRecoverySchemeModel.cs` in `Common.Domain` — scheme_no, scheme_name, scheme_version, recovery_type, product_code, branch_code
- Repository: `IClaimRecoverySchemeRepository` → calls `spu_CLR_RecoverySchemes_Sel` (new SP, place in `Databases/Pure/Procedures/`; parameters: `@CompanyNo`, `@RecoveryType`, `@ProductCode`, `@BranchCode`; filters: scheme_type = 'Claim Recovery', active schemes matching product + branch)
- Query: `GetClaimRecoverySchemesQuery` (CompanyNo, RecoveryType, ProductCode, BranchCode)
- Handler: `GetClaimRecoverySchemesQueryHandler`
- Validator: `GetClaimRecoverySchemesQueryValidator` — CompanyNo mandatory
- Service: `GetClaimRecoverySchemesService` — maps DataSet rows to list of `ClaimRecoverySchemeModel`
- Controller: `GET /claims/recovery/schemes` in `ClaimsController`
- Tests: Handler, Validator, Service unit tests

**Additional DB artefact**: Create `spu_CLR_RecoverySchemes_Sel` stored procedure in `Databases/Pure/Procedures/` (place file in PureInsurance repo).

- **FR coverage**: FR-015, FR-016
- **Depends on**: None (uses schema from Epic #39336 — scheme_type column on PFScheme table)
- **Blocks**: T11
- **Parallelisable**: Yes (with T6, T7, T8)
- **Estimate**: 4 hours
- **Story**: US-003

---

### Task 6: API — Eligible Recovery Transactions Endpoint
**Description**: Implement `GET /claims/recovery/eligibleTransactions` in Claims microservice:
- Domain: `EligibleRecoveryTransactionModel.cs` in `Common.Domain`
- Repository: `IClaimRecoveryRepository` → calls `spu_CLR_EligibleRecoveryTransactions_Sel` + `spu_CLR_TransactionsByDocRef_Sel`
- Query: `GetEligibleRecoveryTransactionsQuery` (PartyCode, ClaimNo)
- Service: maps DataSet rows → response DTOs including document_ref grouping metadata
- Controller: `GET /claims/recovery/eligibleTransactions`
- Tests: Handler, Validator, Service unit tests

- **FR coverage**: FR-003–FR-014, FR-041
- **Depends on**: T2, T3
- **Blocks**: T10
- **Parallelisable**: Yes (with T5, T7, T8)
- **Estimate**: 5 hours
- **Story**: US-001, US-002

---

### Task 7: API — Create Claim Recovery Instalment Plan Endpoint
**Description**: Implement `POST /claims/recovery/instalmentPlan` in Claims microservice:
- Domain: `CreateClaimRecoveryInstalmentPlanModel.cs` — includes selected transactions, scheme, plan config (preferred day, first payment date, currency flag), bank details, overrides
- Repository: calls existing PF plan creation SPs + creates ICC (credit) and ICD (debit) transactions (using T1 ICC/ICD types), logs to event_log
- Command: `CreateClaimRecoveryInstalmentPlanCommand`
- Service: mirrors existing PF `CreateInstalmentPlan` flow — CLR as source, ICC for credits, ICD for debits; generates unique plan reference; creates folder record
- Controller: `POST /claims/recovery/instalmentPlan`
- Tests: Handler, Validator, Service unit tests

- **FR coverage**: FR-017–FR-034, FR-042
- **Depends on**: T1
- **Blocks**: T12
- **Parallelisable**: Yes (with T5, T6, T8)
- **Estimate**: 8 hours
- **Story**: US-004, US-005

---

### Task 8: API — Plan Maintenance Claim Recovery Endpoint
**Description**: Implement `GET /claims/recovery/instalmentPlans` in Claims microservice:
- Query: `GetClaimRecoveryInstalmentPlansQuery` (ClaimNo optional, existing PF params)
- Service: calls `spu_PlanMaintenance_SelByClaimNo`, maps plan type indicator for claim recovery plans
- Controller: `GET /claims/recovery/instalmentPlans`
- Tests: Handler, Validator, Service unit tests

- **FR coverage**: FR-037, FR-038, FR-040
- **Depends on**: T4
- **Blocks**: T13
- **Parallelisable**: Yes (with T5, T6, T7)
- **Estimate**: 3 hours
- **Story**: US-006

---

## Layer 3: Portal UI (ASP.NET Web Forms — `Web Portal/Nexus/Pure.Portals/`)

All Portal tasks call the REST API via `SSP.PureInsuranceRestAPIHandler.ApiClient`. Pages go in `Web Portal/Nexus/Pure.Portals/Claims/` (or a new `ClaimRecoveryInstalment/` subfolder).

### Task 9: Portal — Finance Menu "New Plan" Entry
**Description**: Add "New Plan" entry to Finance Menu in Navigator XM roadmap XML (`Navigator XM Roadmaps/`). This is the entry point (FR-001, FR-002).

- **FR coverage**: FR-001, FR-002
- **Depends on**: None
- **Blocks**: T10
- **Parallelisable**: Yes
- **Estimate**: 1 hour
- **Story**: US-001

---

### Task 10: Portal — Plan Transactions Page (Search + Selection)
**Description**: Create `ClaimRecoveryPlanTransactions.aspx` / `.aspx.vb`:
- Party Code field + Find Party button (reuse existing Find Party popup pattern)
- Claim Number optional field
- FIND button: validates Party Code mandatory, calls `GET /claims/recovery/eligibleTransactions` via ApiClient
- CLEAR button: resets form
- GridView: Document Ref, Claim No, Party, Tx Date, Tx Amount, Outstanding Amount + checkbox per row
- Client-side checkbox change: auto-select all rows with same document_ref (using document_ref grouping metadata from API), recalculate Total Selected Amount label
- OK button: stores selected transaction IDs in session, redirects to InstalmentRecovery page

- **FR coverage**: FR-003–FR-014
- **Depends on**: T6, T9
- **Blocks**: T11
- **Parallelisable**: No
- **Estimate**: 6 hours
- **Story**: US-001, US-002

---

### Task 11: Portal — Instalment Recovery Screen: Scheme Selection
**Description**: Create `ClaimRecoveryInstalmentRecovery.aspx` / `.aspx.vb` — scheme selection panel:
- Retrieve selected transactions from session
- Determine recovery type from transactions
- Load available schemes: call `GET /claims/recovery/schemes` (T5 — new endpoint in this spec) filtered by Scheme Type = Claim Recovery, matching recovery type + Product + Branch
- Display schemes in selectable list; highlight on selection

- **FR coverage**: FR-015, FR-016
- **Depends on**: T5, T10
- **Blocks**: T12
- **Parallelisable**: No
- **Estimate**: 3 hours
- **Story**: US-003

---

### Task 12: Portal — Instalment Recovery Screen: Plan Config Tabs + Bank Details + Save
**Description**: Complete `ClaimRecoveryInstalmentRecovery.aspx` with:
- Plan Summary fields: Preferred Day in Month, First Payment Date, Use Transaction Currency checkbox (FR-017–FR-019)
- 6 tabs reusing existing PF plan configuration controls: Summary, Instalments, Breakdown, Override, Finance Details, Deposit (FR-020–FR-026)
  - Call existing instalment quote API on scheme selection to populate tabs
  - Override tab: Interest Rate override, Commission override, Deposit override — recalculate on change
- Bank Details section: Account Type dropdown (mandatory), Add/Edit bank (reuse PF bank management) (FR-027–FR-030)
- Save & Transact button:
  - Validate mandatory fields (Account Type, Preferred Day, First Payment Date, scheme)
  - Call `POST /claims/recovery/instalmentPlan` via ApiClient
  - On success: display plan reference number

- **FR coverage**: FR-017–FR-034, FR-042
- **Depends on**: T7, T11
- **Blocks**: T14
- **Parallelisable**: No
- **Estimate**: 8 hours
- **Story**: US-004, US-005

---

### Task 13: Portal — Plan Maintenance UI Updates
**Description**: Update existing Plan Maintenance page:
- Add Claim Number search field calling `GET /claims/recovery/instalmentPlans`
- Ensure plan type indicator displays correctly for claim recovery plans
- Verify all existing operations (Edit, History, Reverse, Cancel, Delete, Settlement, MTA, Save, Add Task) work without modification

- **FR coverage**: FR-037–FR-040
- **Depends on**: T8
- **Blocks**: T14
- **Parallelisable**: Yes (with T12)
- **Estimate**: 3 hours
- **Story**: US-006

---

## Layer 4: Testing & Documentation

### Task 14: Integration Testing
**Description**: End-to-end testing:
- Finance Menu → New Plan navigation
- Search: Party Code mandatory, Claim No optional, eligibility filtering
- Document ref auto-grouping and Total Selected Amount
- Scheme selection filtered by recovery type + Product + Branch
- All 6 plan config tabs with correct data
- Override application and dynamic Finance Details updates
- Bank Details: Account Type mandatory validation
- Save & Transact: plan creation, ICC/ICD transactions, folder, event_log, plan reference returned
- Plan Maintenance: claim number search, plan type display, all operations functional
- Regression: existing PF plan creation unaffected

- **Depends on**: T12, T13
- **Blocks**: T15
- **Estimate**: 8 hours
- **Story**: All

---

### Task 15: Documentation and Completion
**Description**:
- Update `audit.md` with completion records
- Verify all acceptance criteria AC-001 through AC-025
- Security baseline compliance check (SECURITY-03, -05, -08, -09, -10, -15)
- Update `aidlc-state.md` to mark all tasks Done
- Update `.ai/memory/` files if new architecture/conventions introduced

- **Depends on**: T14
- **Blocks**: None
- **Estimate**: 2 hours
- **Story**: All

---

## Summary

| Metric | Value |
|--------|-------|
| Total Tasks | 15 |
| Total Estimate | 59 hours |
| Critical Path | T2 → T6 → T10 → T11 → T12 → T14 → T15 (33 hours) |
| Max Parallelism | 4 tasks (T1, T2, T3, T4 simultaneously) |
| Database Tasks | 4 (T1–T4) |
| REST API Tasks | 4 (T5–T8) |
| Portal UI Tasks | 5 (T9–T13) |
| Testing & Docs | 2 (T14–T15) |

## FR Traceability

| FR Range | Covered By |
|----------|-----------|
| FR-001–FR-002 (Finance Menu) | T9 |
| FR-003–FR-014 (Plan Transactions Search/Selection) | T2, T3, T6, T10 |
| FR-015–FR-016 (Scheme Selection) | T5, T11 |
| FR-017–FR-019 (Plan Summary) | T12 |
| FR-020–FR-026 (6 Tabs) | T12 |
| FR-027–FR-030 (Bank Details) | T12 |
| FR-031–FR-034 (Save & Transact) | T7, T12 |
| FR-035–FR-036 (ICC/ICD types) | T1 |
| FR-037–FR-040 (Plan Maintenance) | T4, T8, T13 |
| FR-041 (Duplicate prevention) | T2 (SP filter) |
| FR-042 (Audit log) | T7 |

---

*Revised by AI agent 2026-06-04. FR-015 gap closed: added T5 (GET /claims/recovery/schemes) confirmed absent from codebase. All 42 FRs now covered. Traceability: requirements.md -> user-stories.md -> design.md -> tasks.md*
