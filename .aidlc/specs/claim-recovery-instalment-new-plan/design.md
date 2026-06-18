# Application Design — Instalment for Claim Recovery - New Plan

**Feature**: Instalment for Claim Recovery - New Plan
**Spec ID**: SPEC-39489
**Source PBI**: ADO #37524
**Date**: 2026-05-07

---

## 1. Architecture Overview

This feature adds new endpoints to the **Claims REST API microservice** (`PureInsurance.REST`) and new ASP.NET Web Forms pages to the **Pure Portal** (Nexus). The Portal calls the REST API — not WCF/SAM.

**Architecture note**: `PureInsurance.REST` uses CQRS + MediatR pattern. Each operation has: Controller → QueryHandler/CommandHandler → Service → Repository → stored procedure via dPMDAO.

```
+------------------------------------------------------------------+
|              PORTAL UI LAYER (ASP.NET Web Forms)                  |
|         Web Portal/Nexus/Pure.Portals/Claims/                    |
|                                                                  |
|  [NEW] ClaimRecoveryPlanTransactions.aspx                        |
|    - Party Code + Find Party button                              |
|    - Claim Number (optional)                                     |
|    - FIND / CLEAR, results GridView with checkboxes             |
|    - document_ref auto-grouping, Total Selected Amount           |
|    - OK → session → redirect to InstalmentRecovery              |
|                                                                  |
|  [NEW] ClaimRecoveryInstalmentRecovery.aspx                      |
|    - Scheme selection panel                                      |
|    - Plan Summary (Preferred Day, First Payment, Currency)       |
|    - Tabs: Summary | Instalments | Breakdown | Override          |
|            | Finance Details | Deposit (reuse PF controls)       |
|    - Bank Details section (Account Type mandatory, Add/Edit)     |
|    - Save & Transact button                                      |
|                                                                  |
|  [UPDATED] Plan Maintenance page                                 |
|    - Add Claim Number search field                               |
|    - Claim recovery plan type display                            |
+------------------------------------------------------------------+
              |  SSP.PureInsuranceRestAPIHandler.ApiClient
              |  HTTPS + JWT Bearer (Keycloak)
              v
+------------------------------------------------------------------+
|         REST API — Claims Microservice (PureInsurance.REST)       |
|         MicroServices/Claims/                                    |
|                                                                  |
|  [NEW] GET  /claims/recovery/eligibleTransactions                |
|    → GetEligibleRecoveryTransactionsQueryHandler                 |
|    → GetEligibleRecoveryTransactionsService                      |
|                                                                  |
|  [NEW] POST /claims/recovery/instalmentPlan                      |
|    → CreateClaimRecoveryInstalmentPlanCommandHandler             |
|    → CreateClaimRecoveryInstalmentPlanService                    |
|      (CLR source → ICC credits, ICD debits, folder, event_log)  |
|                                                                  |
|  [NEW] GET  /claims/recovery/instalmentPlans                     |
|    → GetClaimRecoveryInstalmentPlansQueryHandler                 |
|    → GetClaimRecoveryInstalmentPlansService                      |
|                                                                  |
|  [NEW] GET  /claims/recovery/schemes                             |
|    -> GetClaimRecoverySchemesQueryHandler                        |
|    -> GetClaimRecoverySchemesService                             |
|      (spu_CLR_RecoverySchemes_Sel, filter scheme_type=CLR)      |
+------------------------------------------------------------------+
              |  dPMDAO / ADO.NET (stored procedures only)
              v
+------------------------------------------------------------------+
|                    DATABASE LAYER                                 |
|                                                                  |
|  [NEW SPs]                                                       |
|  spu_CLR_EligibleRecoveryTransactions_Sel                        |
|  spu_CLR_TransactionsByDocRef_Sel                                |
|  spu_PlanMaintenance_SelByClaimNo (or extend existing)           |
|                                                                  |
|  [NEW DATA — migration script]                                   |
|  transaction_type: ICC (Instalment Claim Credit)                 |
|  transaction_type: ICD (Instalment Claim Debit)                  |
|                                                                  |
|  [EXISTING from Epic #39336]                                     |
|  PFScheme.scheme_type, PFRate.transaction_type                   |
|  RiskType.recovery_instalments_enabled                           |
|                                                                  |
|  [EXISTING]                                                      |
|  pf_prem_finance_cnt (instalment plans), event_log, party_bank   |
+------------------------------------------------------------------+
```

---

## 2. Component Design

### 2.1 Plan Transactions Page (New ASP.NET Web Forms Page)

**Location**: New page in Portal — accessible from Finance Menu
**File**: e.g. `PlanTransactions.aspx` / `PlanTransactions.aspx.vb`

| Element | Type | Purpose |
|---------|------|---------|
| `txtPartyCode` | TextBox | Party code entry |
| `btnFindParty` | Button | Opens Find Party search screen |
| `txtClaimNumber` | TextBox | Optional claim number filter |
| `btnFind` | Button | Executes search |
| `btnClear` | Button | Clears search criteria |
| `grdTransactions` | GridView/Repeater | Displays eligible recovery transactions |
| `chkSelect` | CheckBox (per row) | Transaction selection |
| `lblTotalSelected` | Label | Total Selected Amount |
| `btnOK` | Button | Proceeds to Instalment Recovery Screen |

**Behaviour**:
```
Page_Load:
  1. Initialise empty search form

btnFindParty_Click:
  2. Open Find Party popup/modal
  3. On party selected: populate txtPartyCode

btnFind_Click:
  4. Validate: Party Code is mandatory
  5. Call SearchEligibleRecoveryTransactions(partyCode, claimNo)
  6. Bind results to grdTransactions

btnClear_Click:
  7. Clear txtPartyCode, txtClaimNumber, grdTransactions, lblTotalSelected

chkSelect_CheckedChanged:
  8. Get document_ref of selected row
  9. Auto-select all rows with same document_ref
  10. Recalculate and update lblTotalSelected

btnOK_Click:
  11. Validate: at least one transaction selected
  12. Store selected transaction IDs in session/viewstate
  13. Redirect/navigate to Instalment Recovery Screen
```

---

### 2.2 Instalment Recovery Screen (New ASP.NET Web Forms Page)

**Location**: New page in Portal — navigated from Plan Transactions
**File**: e.g. `InstalmentRecovery.aspx` / `InstalmentRecovery.aspx.vb`

| Element | Type | Purpose |
|---------|------|---------|
| `pnlSchemeSelection` | Panel | Displays available schemes |
| `lstSchemes` | ListBox/Repeater | Scheme list with selection |
| `txtPreferredDay` | TextBox | Preferred day of month (1-28) |
| `txtFirstPaymentDate` | TextBox + Calendar | First payment date |
| `chkUseTransCurrency` | CheckBox | Use transaction currency toggle |
| `tabSummary` | Tab/Panel | Summary tab (reuse PF) |
| `tabInstalments` | Tab/Panel | Instalments schedule tab (reuse PF) |
| `tabBreakdown` | Tab/Panel | Breakdown tab (reuse PF) |
| `tabOverride` | Tab/Panel | Override tab (reuse PF) |
| `tabFinanceDetails` | Tab/Panel | Finance Details tab (reuse PF) |
| `tabDeposit` | Tab/Panel | Deposit tab (reuse PF) |
| `pnlBankDetails` | Panel | Bank details section (reuse PF) |
| `btnSaveTransact` | Button | Save & Transact |

**Behaviour**:
```
Page_Load:
  1. Retrieve selected transactions from session
  2. Determine recovery type from transactions
  3. Load available schemes: GetAvailableRecoverySchemes(companyNo, recoveryType)
  4. Bind schemes to lstSchemes

lstSchemes_SelectedIndexChanged:
  5. Load scheme configuration
  6. Generate plan summary: GetPlanSummary(schemeNo, version, financedAmount)
  7. Generate plan reference: GeneratePlanReference(...)
  8. Populate all tabs with calculated values
  9. Load bank details for party

Override controls changed:
  10. Recalculate plan with overrides: ApplyOverrides(...)
  11. Update Finance Details tab dynamically
  12. Update Instalments tab with new schedule

btnSaveTransact_Click:
  13. Validate mandatory fields (Account Type, Preferred Day, First Payment Date, scheme selected)
  14. CreateRecoveryInstalmentPlan(transactions, schemeNo, version, params)
  15. Create ICC/ICD accounting transactions
  16. Create folder/record (per PF pattern)
  17. Log to event_log
  18. Display success with plan reference number
```

---

### 2.3 New REST API Components

All in `PureInsurance.REST / MicroServices/Claims/`:

#### New Domain Models (`PureInsurance.REST.Common.Domain/`)

| File | Purpose |
|------|---------|
| `EligibleRecoveryTransactionModel.cs` | Search result row (docRef, claimNo, party, txDate, txAmt, outstandingAmt, recoveryType) |
| `CreateClaimRecoveryInstalmentPlanModel.cs` | Plan creation request (transactions, scheme, preferred day, first payment date, currency flag, bank details, overrides) |
| `ClaimRecoveryInstalmentPlanModel.cs` | Plan search result row for Plan Maintenance |

#### New Repository Methods (`PureInsurance.REST.Common.Repositories/`)

| Method | SP Called |
|--------|-----------|
| `GetClaimRecoverySchemes(companyNo, recoveryType, productCode, branchCode)` | `spu_CLR_RecoverySchemes_Sel` |
| `GetEligibleRecoveryTransactions(partyCode, claimNo)` | `spu_CLR_EligibleRecoveryTransactions_Sel` |
| `GetTransactionsByDocRef(docRef, partyCode)` | `spu_CLR_TransactionsByDocRef_Sel` |
| `CreateClaimRecoveryInstalmentPlan(...)` | Existing PF plan creation SPs + ICC/ICD transaction inserts |
| `GetClaimRecoveryInstalmentPlans(claimNo, ...)` | `spu_PlanMaintenance_SelByClaimNo` |

#### New Service Methods (`Claims.Application/Services/`)

| Service | Purpose |
|---------|---------|
| `GetClaimRecoverySchemesService` | Calls spu_CLR_RecoverySchemes_Sel, maps rows to ClaimRecoverySchemeModel list filtered by scheme_type = Claim Recovery, recovery type, product, branch |
| `GetEligibleRecoveryTransactionsService` | Calls SP, maps rows, includes docRef grouping metadata |
| `CreateClaimRecoveryInstalmentPlanService` | CLR → ICC/ICD accounting, plan reference generation, folder creation, event_log |
| `GetClaimRecoveryInstalmentPlansService` | Plan Maintenance search with claim recovery plan type awareness |

---

### 2.4 New Stored Procedures

| Procedure | Purpose |
|-----------|---------|
| `spu_CLR_RecoverySchemes_Sel` | Select active Claim Recovery schemes filtered by scheme_type = 'Claim Recovery', recovery_type, product_code, branch_code (uses schema from Epic #39336) |
| `spu_CLR_EligibleRecoveryTransactions_Sel` | Select recovery transactions by party (+ optional claim_no) where product enabled, outstanding > 0, no active plan |
| `spu_CLR_TransactionsByDocRef_Sel` | Select all transaction IDs for a given document_ref |
| `spu_PlanMaintenance_SelByClaimNo` | Extend Plan Maintenance search to support claim number lookup (or add parameter to existing proc) |

---

### 2.5 Database Changes

#### New Transaction Types (Migration Script)

```sql
-- Databases/After Change/
-- Add ICC and ICD transaction types to transaction_type table
INSERT INTO transaction_type (type_code, description, ...)
VALUES ('ICC', 'Instalment Claim Credit', ...);

INSERT INTO transaction_type (type_code, description, ...)
VALUES ('ICD', 'Instalment Claim Debit', ...);
```

The exact column structure follows the existing INC/IND records as templates.

#### Plan Maintenance — Claim Number Search

Extend the existing Plan Maintenance search stored procedure to accept an optional `@ClaimNo` parameter, or create a new procedure that joins instalment plan → source transaction → claim to enable claim-based search.

---

### 2.6 Plan Maintenance Integration

The existing Plan Maintenance functionality needs minimal changes:

1. **Search**: Add claim number as a search parameter (new stored procedure or parameter extension)
2. **Display**: Plan Maintenance must recognise the claim recovery plan type and display it correctly (plan type indicator)
3. **Operations**: All existing operations (Edit, History, Reverse, Cancel, Delete, Settlement, MTA, Save, Add Task) work unchanged because the plan structure is identical to Premium Finance

No new UI screens needed for Plan Maintenance — only data access changes to support claim number search and plan type awareness.

---

## 3. Navigator XM Integration

A new Finance Menu entry must be added to the Navigator XM roadmap XML to provide the "New Plan" option:

```xml
<!-- Add to Finance Menu roadmap -->
<MenuItem>
  <Name>New Plan</Name>
  <Description>Create Instalment Plan for Claim Recovery</Description>
  <Target>PlanTransactions.aspx</Target>
  <!-- Visibility: always visible in Finance Menu -->
</MenuItem>
```

---

## 4. Security Considerations

| Rule | Applicability | Notes |
|------|--------------|-------|
| SECURITY-03 | Applicable | Plan creation logged to event_log; no PII in logs |
| SECURITY-05 | Applicable | All data access via stored procedures (no SQL injection) |
| SECURITY-08 | Applicable | Existing claim recovery RBAC applies — no new roles |
| SECURITY-09 | Applicable | Error messages generic to users; no internal details exposed |
| SECURITY-10 | Applicable | Input validation on Party Code, Claim Number, Preferred Day (1-28), dates |
| SECURITY-15 | Applicable | All dPMDAO calls wrapped in error handling; fail closed |

---

## 5. Testing Strategy

| Level | Scope |
|-------|-------|
| Unit Tests | Search eligibility logic, document_ref grouping, total calculation, validation rules |
| Integration Tests | End-to-end: search → select → scheme → configure → save → verify DB records + transactions |
| Regression Tests | Existing PF plan creation unaffected; existing Plan Maintenance unaffected |
| UI Tests | Page navigation, search behaviour, checkbox grouping, tab switching, save flow |

---

## 6. Reused Components Summary

| Component | What's Reused | Notes |
|-----------|---------------|-------|
| Find Party search | Entire screen/popup | No changes needed |
| Premium Finance plan tabs | All 6 tabs (Summary, Instalments, Breakdown, Override, Finance Details, Deposit) | Reused as-is |
| Premium Finance bank details | Account type display, Add/Edit bank | Reused as-is |
| bSIRInstalments | Plan creation, reference generation, schedule generation, folder creation, overrides | Core PF logic reused |
| gCLMLibrary | IsRecoveryInstalmentEnabled, GetRecoveryType, GetAvailableRecoverySchemes, ValidateNoExistingPlan | From Epic #39336 |
| Plan Maintenance | All operations (edit, history, reverse, cancel, delete, settlement, MTA) | Existing functionality |

---

*Generated by AI-SDLC INCEPTION phase. Traceability: requirements.md → user-stories.md → design.md → tasks.md*
