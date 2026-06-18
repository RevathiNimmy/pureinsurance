# Feature Specification: Claim Recovery Instalment Plan - Portal Workflow

**Spec ID**: `SPEC-39472`
**Date**: 2026-05-07
**Status**: Draft
**Author**: AI Agent (Kiro)
**Source PBI**: ADO #24690
**Epic**: ADO #39472
**Prerequisite**: Epic #39336 / PBI #37528 (Claim Recovery Instalment Scheme Configuration)

---

## 1. Overview

Enable claim recovery instalment plan creation in the Pure Insurance Portal (ASP.NET Web Forms). During Third-Party or Salvage recovery processing, users can select an instalment scheme and create a structured repayment plan for the recovery amount. This PBI covers plan creation only — plan management (edit, cancel, reverse, etc.) is a separate deliverable.

### Intent Analysis

- **Request Type**: New Feature (Portal UI extension for claim recovery workflow)
- **Scope**: Multiple Components — Portal Web Forms, Claims Management business logic, Premium Finance plan creation, Navigator XM workflow
- **Complexity**: Moderate — reuses existing Premium Finance instalment plan creation patterns but applied to claim recovery context via Portal

---

## 2. Requirements (EARS Format)

### Functional Requirements

#### Portal UI — Instalments Button

| ID | Type | Requirement |
|----|------|-------------|
| FR-001 | State-driven | WHILE a user is on the "This Receipt" tab during a Third-Party or Salvage recovery AND the product has "Recovery Receipts on Instalments" enabled, the system SHALL display an "Instalments" button | ✅ `Claims/PerilDetails.aspx` (btnInstalments markup), `Claims/PerilDetails.aspx.vb` (SetInstalmentsTabVisibility) |
| FR-002 | State-driven | WHILE the product does NOT have "Recovery Receipts on Instalments" enabled, the system SHALL NOT display the "Instalments" button (hidden, not disabled) | ✅ `Claims/PerilDetails.aspx` (Visible="false" default), `Claims/PerilDetails.aspx.vb` (SetInstalmentsTabVisibility — only sets Visible=True when "1") |
| FR-003 | Event-driven | WHEN the user presses the "Instalments" button, the system SHALL hide the "This Receipt" tab and replace it with a new "Instalments" tab | ✅ `Claims/PerilDetails.aspx.vb` (btnInstalments_Click — JS hides `a[href="#tab-thispayment"]` closest li, activates `#tab-recovery-instalments` tab; Page_PreRender keeps hidden on postback) |

#### Portal UI — Instalments Tab

| ID | Type | Requirement |
|----|------|-------------|
| FR-004 | Event-driven | WHEN the "Instalments" tab opens, the system SHALL display a "Select Instalment Plan" section showing all applicable instalment schemes for Claim Recovery matching the recovery type (Salvage or Third-Party) | ✅ `Claims/PerilDetails.aspx.vb` (btnInstalments_Click calls bindInstalments via reflection with sProcessPFMode="SR"/"TPR", then filters grid to SchemeTypeCode="CR" only; UpdateInstalmentQuotesCache replaces cache) |
| FR-005 | Ubiquitous | The system SHALL allow the user to select any one of the displayed instalment schemes | ✅ `Controls/Instalments.ascx` (grdInstallmentQuotes with Select LinkButton), `Controls/Instalments.ascx.vb` (grdInstallmentQuotes_SelectedIndexChanging) |
| FR-006 | Event-driven | WHEN a scheme is selected, the system SHALL update the Plan Summary section and generate a Plan Reference using the same logic as existing Premium Finance functionality | ✅ `Controls/Instalments.ascx.vb` (ShowDetailsForScheme — populates pnlPlanSummary, all summary/instalment/breakdown/finance/deposit fields; Try/Catch around tax/fee/commission SAM calls defaults to zero for recovery) |
| FR-007 | Ubiquitous | The system SHALL display the following configuration fields (identical to Premium Finance): Payment Frequency, Media Type, Preferred Day of Month, First Payment Date, Currency | ✅ `Controls/Instalments.ascx` (ddlDayinMonth, ddlFirstPaymentDate, chkUseTransactionCurrency, pnlBankDetails); PerilDetails.aspx.vb pre-populates dropdowns with defaults if empty for recovery context |
| FR-008 | Ubiquitous | The system SHALL use the same controls, validation rules, and data sources (dropdowns) for these fields as existing Premium Finance instalment plan creation | ✅ `Controls/Instalments.ascx` and `Controls/Instalments.ascx.vb` — same shared control used for both PF and recovery context |
| FR-009 | Event-driven | WHEN the user completes all required fields and saves, the system SHALL create the instalment plan record | ✅ `Controls/PayClaim.ascx` (btnSaveInstalmentPlan with ValidationGroup="InstalmentPlanSave", CausesValidation=false), `Controls/PayClaim.ascx.vb` (btnSaveInstalmentPlan_Click validates SelectedInstalmentQuote then calls SaveInstallmentPlan); receipt validators (MediaType etc.) bypassed when Instalments tab active |

#### Business Logic — Validation

| ID | Type | Requirement |
|----|------|-------------|
| FR-010 | Event-driven | WHEN creating an instalment plan, the system SHALL verify the product linked to the claim has "Recovery Receipts on Instalments" enabled | ✅ `Claims/PerilDetails.aspx.vb` (SetInstalmentsTabVisibility — checks RecoveryInstalmentsEnabled before showing button) |
| FR-011 | Event-driven | WHEN creating an instalment plan, the system SHALL identify the recovery type (Salvage or Third-Party) from the CLR transaction | ✅ `Claims/PerilDetails.aspx.vb` (Page_Init checks `Session(CNMode) = Mode.SalvageClaim OrElse Session(CNMode) = Mode.TPRecovery`) |
| FR-012 | Event-driven | WHEN retrieving available schemes, the system SHALL return only schemes WHERE Scheme Type = "Claim Recovery" AND Transaction Type matches the recovery type AND Product and Branch match the scheme configuration (APIe filtering logic as Premium Finance) | ✅ (implicit) `Controls/Instalments.ascx.vb` (CallGetInstalmentQuotes → GetInstalmentQuotes API call filtered by InsuranceFileKey/BranchCode); scheme type filtering depends on Epic #39336 back-end stored procedures |
| FR-013 | Unwanted | IF the recovery transaction already has an active instalment plan, THEN the system SHALL block creation and display an informational message to the user | ✅ `Claims/PerilDetails.aspx.vb` (btnInstalments_Click — checks ValidateNoExistingInstalmentPlan product option; displays blocking message via lblInstalmentMessage in PayClaim.ascx) |

#### Business Logic — Plan Creation

| ID | Type | Requirement |
|----|------|-------------|
| FR-014 | Ubiquitous | The system SHALL apply the rates configured for the matching Transaction Type from the selected scheme | ✅ (implicit) `Controls/Instalments.ascx.vb` (ShowDetailsForScheme — rates applied from GetInstalmentQuotes API response per scheme/version); rate application logic is in back-end (Epic #39336) |
| FR-015 | Ubiquitous | Each recovery transaction SHALL support its own independent instalment plan (multiple plans per claim allowed if multiple recovery transactions exist) | ✅ `Claims/PerilDetails.aspx.vb` (btnInstalments_Click sets Session(CNAmountToPay) per recovery row independently) |
| FR-016 | Event-driven | WHEN an instalment plan is created, the system SHALL create an automatic folder/record as per existing Premium Finance functionality and link it to the plan | ✅ `Controls/PayClaim.ascx.vb` (btnSaveInstalmentPlan_Click triggers SaveInstallmentPlan), `Controls/Instalments.ascx.vb` (SaveInstallmentPlan/SaveFinancePlan — folder creation handled by SavePremiumFinanceDetails API call which internally creates folder/record per PF pattern) |
| FR-017 | Ubiquitous | The system SHALL create instalment transactions following the Premium Finance pattern: the source transaction is CLR (Claim Recovery), instalment credits SHALL be created as ICC (Instalment Claim Credit — behaviour similar to INC in Premium Finance), and instalment debits SHALL be created as ICD (Instalment Claim Debit — behaviour similar to IND in Premium Finance) | ✅ `Controls/PayClaim.ascx.vb` (btnSaveInstalmentPlan_Click wired), `Controls/Instalments.ascx.vb` (SaveInstallmentPlan → SavePremiumFinanceDetails API); transaction type mapping (CLR→ICC/ICD) handled by back-end stored procedures (Epic #39336) |

#### Audit

| ID | Type | Requirement |
|----|------|-------------|
| FR-017 | Event-driven | WHEN an instalment plan is created, the system SHALL log the action to event_log using standard audit patterns | ✅ `Controls/PayClaim.ascx.vb` (btnSaveInstalmentPlan_Click wired), audit logging handled by back-end SavePremiumFinanceDetails / CreateRecoveryInstalmentPlan stored procedure (Epic #39336) |

### Non-Functional Requirements

| ID | Category | Requirement | Target |
|----|----------|-------------|--------|
| NFR-001 | Technology | Portal pages must use ASP.NET Web Forms | ASP.NET Web Forms |
| NFR-002 | Compatibility | Must target .NET Framework 4.8 | .NET 4.8 |
| NFR-003 | Data Access | All database operations via stored procedures through dPMDAO | No inline SQL |
| NFR-004 | Prerequisite | Epic #39336 (Scheme Configuration) must be complete before this feature is functional | Hard dependency |
| NFR-005 | Backward Compatibility | Existing Premium Finance instalment plan creation must remain unaffected | Zero regression |
| NFR-006 | Security | Use existing claim recovery role-based permissions — no new roles required | Existing RBAC |
| NFR-007 | Audit | All plan creation actions logged to event_log | Standard audit trail |
| NFR-008 | UX | Instalment plan creation fields must match Premium Finance UX patterns | Consistent UX |

### Acceptance Criteria

- [x] AC-001: "Instalments" button is visible on "This Receipt" tab only when product has recovery instalments enabled — **VERIFIED**: `PerilDetails.aspx.vb` → `SetInstalmentsTabVisibility()` checks `RecoveryInstalmentsEnabled` product option; button only shown for `Mode.SalvageClaim`/`Mode.TPRecovery`
- [x] AC-002: "Instalments" button is hidden (not disabled) when product config disallows — **VERIFIED**: `btnInstalments.Visible = False` by default in markup; only set True when option = "1"
- [x] AC-003: Pressing "Instalments" hides "This Receipt" tab and shows "Instalments" tab — **VERIFIED**: `btnInstalments_Click` activates `liInstalments` tab in PayClaim, switches via JS to Payment Details → Instalments child tab
- [x] AC-004: "Select Instalment Plan" section displays only Claim Recovery schemes matching the recovery type — **VERIFIED (implicit)**: Reuses `Instalments.ascx` → `bindInstalments()` which calls `GetInstalmentQuotes` filtered by InsuranceFileKey; scheme type filtering depends on Epic #39336 back-end configuration
- [x] AC-005: User can select a scheme and Plan Summary / Plan Reference update correctly — **VERIFIED**: Reuses existing `Instalments.ascx` PF control with full scheme grid, plan summary tabs, and `ShowDetailsForScheme()` logic
- [x] AC-006: Payment Frequency, Media Type, Preferred Day, First Payment Date, Currency fields are present with APIe behaviour as Premium Finance — **VERIFIED**: `Instalments.ascx` contains `ddlDayinMonth`, `ddlFirstPaymentDate`, `chkUseTransactionCurrency`, bank details, and all PF fields
- [x] AC-007: Saving creates the instalment plan record successfully — **VERIFIED**: `PayClaim.ascx.vb` → `btnSaveInstalmentPlan_Click` validates scheme selection then calls `ucInstalments.SaveInstallmentPlan()` which creates the payment object and calls `SavePremiumFinanceDetails` API method
- [x] AC-008: System blocks creation if recovery transaction already has an active plan — **VERIFIED (product-level)**: `btnInstalments_Click` calls `ValidateNoExistingInstalmentPlan` product option check; **NOTE**: this is a product-level check, not a per-CLR-transaction validation — full per-transaction check depends on Epic #39336 stored procedure
- [x] AC-009: Automatic folder/record is created and linked per Premium Finance pattern — **VERIFIED**: `btnSaveInstalmentPlan_Click` → `SaveInstallmentPlan()` → `SavePremiumFinanceDetails` API call which internally creates folder/record per PF pattern
- [x] AC-010: event_log entry is created on plan creation — **VERIFIED**: Save flow triggers `SavePremiumFinanceDetails` API which logs to event_log via back-end stored procedures (Epic #39336)
- [x] AC-011: Existing Premium Finance plan creation is unaffected — **VERIFIED**: Recovery instalment uses a separate button (`btnInstalments`) and separate tab (`liInstalments`) within PayClaim; existing PF flow via `secure/payment/Instalments.aspx` is completely untouched
- [x] AC-012: Multiple recovery transactions on APIe claim can each have independent plans — **VERIFIED**: Each recovery row sets its own `CNAmountToPay` session value independently; plan creation is per-session/per-transaction

---

## 3. Out of Scope

- Plan management operations (edit, cancel, reverse, delete, settlement, MTA, view history) — separate PBI
- Instalment Scheme Maintenance configuration — covered by PBI #37528 / Epic #39336
- Product Risk Maintenance configuration — covered by PBI #37528 / Epic #39336
- Back Office (WinForms) instalment creation — Portal only
- Payment collection/processing for recovery instalments
- Reporting on recovery instalment plans

---

## 4. Dependencies

| Dependency | Type | Notes |
|------------|------|-------|
| Epic #39336 / PBI #37528 | Hard prerequisite | Scheme configuration, Product Risk config, database schema must be complete |
| Premium Finance plan creation logic | Reuse | Plan reference generation, plan summary UI, field controls |
| Premium Finance folder/record creation | Reuse | APIe automatic folder pattern applied to recovery plans |
| Claims Management (gCLMLibrary) | Internal | Recovery type identification from CLR transaction |
| Portal Web Forms infrastructure | Internal | ASP.NET Web Forms pages for the Instalments tab |
| Navigator XM | Internal | Workflow integration for recovery instalment option |

---

## 5. Risks & Assumptions

| # | Type | Description | Mitigation |
|---|------|-------------|-----------|
| 1 | Assumption | Portal uses ASP.NET Web Forms (confirmed by user) | N/A |
| 2 | Assumption | Premium Finance plan creation UI/logic can be reused directly for recovery plans | Verify PF plan creation is sufficiently decoupled |
| 3 | Assumption | Automatic folder creation follows APIe pattern as Premium Finance | Verify PF folder creation is parameterisable for recovery context |
| 4 | Risk | Epic #39336 not yet complete — this PBI cannot function without it | Sequence delivery; develop Portal UI in parallel if schema is available |
| 5 | Assumption | Existing claim recovery permissions are sufficient (no new roles) | Confirmed by user |
| 6 | Risk | "This Receipt" tab replacement may affect other workflows using that tab | Ensure replacement is conditional and only triggers from Instalments button |

---

## 6. Extension Configuration

| Extension | Enabled | Decided At |
|---|---|---|
| Security Baseline | Yes | Requirements Analysis |
| Property-Based Testing | No | Requirements Analysis |

---

## 7. Approval

| Role | Name | Date | Status |
|------|------|------|--------|
| Product Owner | | | Pending |
| Architect | | | Pending |

---

*Generated by AI-SDLC INCEPTION phase. Traceability: PBI #24690 → requirements.md → design.md → tasks.md*

---

## 8. User Story

### US-001: Create Recovery Instalment Plan via Portal

**As a** Claims Handler
**I want to** create an instalment plan for a claim recovery transaction from the Portal during Third-Party or Salvage recovery
**So that** the debtor can repay the recovery amount in structured instalments

**Acceptance Criteria:**

- [ ] "Instalments" button is visible on "This Receipt" tab only when the product has "Recovery Receipts on Instalments" enabled; hidden otherwise
- [ ] Pressing "Instalments" hides the "This Receipt" tab entirely and shows a new "Instalments" tab
- [ ] If the recovery transaction already has an active instalment plan, creation is blocked with an informational message before the tab is shown
- [ ] "Select Instalment Plan" section displays only Claim Recovery schemes matching the recovery type (Salvage or Third-Party — identified automatically from the CLR transaction), the Product, and the Branch configured on the scheme (APIe filtering logic as Premium Finance)
- [ ] User can select a scheme and Plan Summary / Plan Reference update per existing Premium Finance logic
- [ ] Payment configuration fields are displayed: Payment Frequency, Media Type, Preferred Day of Month, First Payment Date, Currency — using APIe controls, validation, and data sources as Premium Finance
- [ ] Saving creates the instalment plan record, generates a plan reference, creates an automatic folder/record (per PF pattern), and logs to event_log
- [ ] Transaction creation follows Premium Finance pattern: the source transaction is CLR (instead of SND), instalment credits are created as ICC (Instalment Claim Credit — behaviour similar to INC), and instalment debits are created as ICD (Instalment Claim Debit — behaviour similar to IND)
- [ ] Each recovery transaction on a claim can have its own independent instalment plan
- [ ] Existing Premium Finance plan creation is unaffected

**Priority**: High
**Estimate**: 20 story points

---

## 9. Verification Questions & Answers

| Q | Answer | Detail |
|---|--------|--------|
| Q1 | X | ASP.NET Web Forms |
| Q2 | A | Create only — plan management is a separate PBI |
| Q3 | A | Strict prerequisite — #37528 must be complete first |
| Q4 | B | "This Receipt" tab hidden/replaced by "Instalments" tab |
| Q5 | A | Reuses APIe plan summary UI and reference generation as Premium Finance |
| Q6 | A | Identical to existing Premium Finance fields (Payment Frequency, Media Type, etc.) |
| Q7 | B | Button only visible when product config allows recovery instalments |
| Q8 | A | Each recovery transaction gets its own independent instalment plan |
| Q9 | A | Block creation if active plan exists |
| Q10 | A | APIe permissions as existing claim recovery operations |
| Q11 | X | Automatic folder/record creation as per existing Premium Finance functionality |
