# Feature Specification: Instalment for Claim Recovery - New Plan

**Spec ID**: `SPEC-39489`
**Date**: 2026-05-07
**Status**: Draft
**Author**: AI Agent (Kiro)
**Source PBI**: ADO #37524
**Epic**: ADO #39489
**Prerequisites**: Epic #39336 / PBI #37528 (Scheme Configuration), Epic #39472 / PBI #24690 (Portal Receipt Button)

---

## 1. Overview

Provide a standalone "New Plan" workflow accessible from the Pure Portal Finance Menu, enabling users to search for outstanding claim recovery transactions, select eligible transactions, choose an instalment scheme, configure the plan (with Summary, Instalments, Breakdown, Override, Finance Details, and Deposit tabs), capture bank details, and create the instalment plan with full accounting transactions (CLR → ICC/ICD).

This is a separate entry point from the recovery receipt page "Instalments" button (covered by SPEC-39472). This spec covers the complete "Finance Menu → New Plan" standalone workflow.

### Intent Analysis

- **Request Type**: New Feature (Standalone Portal page for claim recovery instalment plan creation)
- **Scope**: Multiple Components — Portal Web Forms (new page), Claims Management business logic, Premium Finance plan creation, Database (new transaction types ICC/ICD), Navigator XM
- **Complexity**: Complex — new standalone page with search, selection, multi-tab configuration, bank details, new transaction types, and plan maintenance awareness

---

## 2. Requirements (EARS Format)

### Functional Requirements

#### Finance Menu Entry Point

| ID | Type | Requirement |
|----|------|-------------|
| FR-001 | Ubiquitous | The system SHALL provide a "New Plan" option under the existing Finance Menu used for Premium Finance in Pure Portal (same menu location, additional option for claim recovery) |
| FR-002 | Event-driven | WHEN the user selects "Finance Menu → New Plan", the system SHALL open the Plan Transactions screen |

#### Plan Transactions Screen — Search

| ID | Type | Requirement |
|----|------|-------------|
| FR-003 | Ubiquitous | The Plan Transactions screen SHALL display a Party Code search field with a "Find Party" button that opens the existing Find Party search screen |
| FR-004 | Ubiquitous | The Plan Transactions screen SHALL display a Claim Number search field as an optional refinement filter |
| FR-005 | Ubiquitous | Party Code SHALL be mandatory for search execution |
| FR-006 | Event-driven | WHEN the user clicks the Party Code button, the system SHALL open the existing Find Party search screen and populate the Party Code field with the selected party |
| FR-007 | Event-driven | WHEN the user clicks FIND, the system SHALL retrieve recovery transactions matching the search criteria that are eligible for instalments |
| FR-008 | Event-driven | WHEN the user clicks CLEAR, the system SHALL clear all search criteria on the Plan Transactions screen |
| FR-009 | Ubiquitous | The system SHALL apply the following eligibility criteria for retrieved transactions: product has "Recovery Receipts on Instalments" enabled, outstanding amount > 0, transaction does not already have an active instalment plan |

#### Plan Transactions Screen — Results and Selection

| ID | Type | Requirement |
|----|------|-------------|
| FR-010 | Ubiquitous | Search results SHALL display a list of recovery transactions including: Document Reference, Claim Number, Party, Transaction Date, Transaction Amount, Outstanding Amount |
| FR-011 | Ubiquitous | Each row SHALL include a selection checkbox |
| FR-012 | Event-driven | WHEN a user selects a checkbox, IF multiple rows share the same `document_ref`, the system SHALL automatically select all transactions under that document reference |
| FR-013 | Ubiquitous | The system SHALL calculate and display the Total Selected Amount (sum of Outstanding Amount for all selected transactions) |
| FR-014 | Event-driven | WHEN the user clicks OK with transactions selected, the system SHALL open the Instalment Recovery Screen |

#### Instalment Scheme Selection

| ID | Type | Requirement |
|----|------|-------------|
| FR-015 | Ubiquitous | The Instalment Recovery Screen SHALL display all available instalment schemes WHERE Scheme Type = "Claim Recovery" AND Transaction Type matches the recovery type of the selected transactions AND the scheme's configured Product and Branch match the product and branch of the claim. If Product or Branch do not match, the scheme SHALL NOT be available for selection |
| FR-016 | Event-driven | WHEN the user selects a scheme, the system SHALL highlight the selected scheme and load the scheme configuration to generate the instalment plan |

#### Plan Configuration — Plan Summary

| ID | Type | Requirement |
|----|------|-------------|
| FR-017 | Ubiquitous | The system SHALL display Plan Summary fields: Preferred Day in Month, First Payment Date, Use Transaction Currency |
| FR-018 | Event-driven | WHEN "Use Transaction Currency" is selected, the system SHALL use the claim recovery transaction currency to calculate the financed amount |
| FR-019 | Event-driven | WHEN "Use Transaction Currency" is NOT selected, the system SHALL apply the same currency logic as Premium Finance (company base currency or user-selected) |

#### Plan Configuration — Tabs (Reuse Premium Finance)

| ID | Type | Requirement |
|----|------|-------------|
| FR-020 | Ubiquitous | The system SHALL display the following tabs using the same UI controls and logic as existing Premium Finance plan configuration: Summary, Instalments, Breakdown, Override, Finance Details, Deposit |
| FR-021 | Ubiquitous | The Summary tab SHALL display: Financed amount, Deposit, Interest, Total payable amount, Number of instalments |
| FR-022 | Ubiquitous | The Instalments tab SHALL display the generated schedule: Instalment number, Due date, Principal amount, Interest amount, Total instalment amount, Outstanding balance |
| FR-023 | Ubiquitous | The Breakdown tab SHALL display: Principal component, Interest component, Deposit allocation |
| FR-024 | Ubiquitous | The Override tab SHALL provide: Override Interest Rate checkbox (one-off plan override with new rate entry), Commission Override checkbox (same commission reference logic as Premium Finance), Deposit Override checkbox (amount or percentage against financed amount) |
| FR-025 | Ubiquitous | The Finance Details tab SHALL display: Financed amount, Interest rate, Interest amount, Total payable amount — updating dynamically when overrides are applied |
| FR-026 | Ubiquitous | The Deposit tab SHALL display: Deposit amount, Deposit percentage, Deposit date — deposit reduces the financed amount |

#### Bank Details

| ID | Type | Requirement |
|----|------|-------------|
| FR-027 | Ubiquitous | The system SHALL display a Bank Details section using the existing party bank details management (same screens/logic as Premium Finance) |
| FR-028 | Ubiquitous | The system SHALL display Account Types configured for the selected Party |
| FR-029 | Ubiquitous | The system SHALL provide Add Bank and Edit Bank functionality (reusing existing Premium Finance bank management) |
| FR-030 | Ubiquitous | Account Type in Bank Details SHALL be mandatory for plan creation |

#### Plan Creation — Save & Transact

| ID | Type | Requirement |
|----|------|-------------|
| FR-031 | Event-driven | WHEN the user clicks "Save & Transact", the system SHALL validate all mandatory fields |
| FR-032 | Event-driven | WHEN validation passes, the system SHALL generate the instalment payment schedule |
| FR-033 | Event-driven | WHEN the schedule is generated, the system SHALL create the instalment plan and generate a unique instalment plan reference number |
| FR-034 | Ubiquitous | The system SHALL create instalment transactions following the Premium Finance pattern: source transaction = CLR (Claim Recovery), instalment credits = ICC (Instalment Claim Credit — behaviour similar to INC), instalment debits = ICD (Instalment Claim Debit — behaviour similar to IND) |

#### New Transaction Types

| ID | Type | Requirement |
|----|------|-------------|
| FR-035 | Ubiquitous | The system SHALL create new transaction types: ICC (Instalment Claim Credit) with behaviour similar to INC (Instalment New Credit) in Premium Finance |
| FR-036 | Ubiquitous | The system SHALL create new transaction types: ICD (Instalment Claim Debit) with behaviour similar to IND (Instalment New Debit) in Premium Finance |

#### Plan Maintenance Integration

| ID | Type | Requirement |
|----|------|-------------|
| FR-037 | Ubiquitous | Created instalment plans SHALL be maintainable through the existing Plan Maintenance task in Pure Portal |
| FR-038 | Ubiquitous | The Plan Maintenance screens SHALL be aware of the claim recovery plan type and display/filter them appropriately |
| FR-039 | Ubiquitous | All existing Plan Maintenance functionality SHALL apply to claim recovery plans: Search, Edit, View History, Reverse, Cancel, Delete, Settlement, MTA, Save, Add Task |
| FR-040 | Ubiquitous | The Claim Instalment Plan SHALL be searchable using the claim number in Plan Maintenance |

#### Duplicate Prevention

| ID | Type | Requirement |
|----|------|-------------|
| FR-041 | Unwanted | IF a recovery transaction already has an active instalment plan, THEN the system SHALL NOT include it in the eligible transactions search results (FR-009) |

#### Audit

| ID | Type | Requirement |
|----|------|-------------|
| FR-042 | Event-driven | WHEN an instalment plan is created, the system SHALL log the action to event_log using standard audit patterns |

### Non-Functional Requirements

| ID | Category | Requirement | Target |
|----|----------|-------------|--------|
| NFR-001 | Technology | Portal pages must use ASP.NET Web Forms | ASP.NET Web Forms |
| NFR-002 | Compatibility | Must target .NET Framework 4.8 | .NET 4.8 |
| NFR-003 | Data Access | All database operations via stored procedures through dPMDAO | No inline SQL |
| NFR-004 | Prerequisite | Epic #39336 (Scheme Configuration) must be complete | Hard dependency |
| NFR-005 | Backward Compatibility | Existing Premium Finance instalment plan creation must remain unaffected | Zero regression |
| NFR-006 | Security | Use existing claim recovery role-based permissions — no new roles required | Existing RBAC |
| NFR-007 | Audit | All plan creation actions logged to event_log | Standard audit trail |
| NFR-008 | UX | Plan configuration tabs must match Premium Finance UX patterns | Consistent UX |
| NFR-009 | Performance | Plan Transactions search must return results within acceptable response time for Portal pages | < 5 seconds |
| NFR-010 | Data Integrity | Transaction selection with document reference grouping must be atomic (all-or-nothing per document ref) | Consistent selection |

### Acceptance Criteria

- [ ] AC-001: "New Plan" option is available under the existing Finance Menu (same menu used for Premium Finance) in Pure Portal
- [ ] AC-002: Plan Transactions screen opens with Party Code (mandatory) and Claim Number (optional) search fields
- [ ] AC-003: Party Code button opens Find Party search and populates field on selection
- [ ] AC-004: FIND retrieves only eligible recovery transactions (product enabled, outstanding > 0, no active plan)
- [ ] AC-005: CLEAR resets all search criteria
- [ ] AC-006: Results display Document Reference, Claim Number, Party, Transaction Date, Transaction Amount, Outstanding Amount with checkboxes
- [ ] AC-007: Selecting a checkbox auto-selects all rows sharing the same document_ref
- [ ] AC-008: Total Selected Amount updates correctly on selection changes
- [ ] AC-009: OK opens Instalment Recovery Screen with available Claim Recovery schemes filtered by recovery type, Product, and Branch
- [ ] AC-010: User can select a scheme and system loads configuration
- [ ] AC-011: Plan Summary fields (Preferred Day, First Payment Date, Use Transaction Currency) function correctly
- [ ] AC-012: All 6 tabs (Summary, Instalments, Breakdown, Override, Finance Details, Deposit) display correctly with same behaviour as Premium Finance
- [ ] AC-013: Override Interest Rate applies one-off override to this plan only
- [ ] AC-014: Commission Override uses same logic as Premium Finance
- [ ] AC-015: Deposit Override accepts amount or percentage and reduces financed amount
- [ ] AC-016: Bank Details section shows party's configured account types with Add/Edit functionality
- [ ] AC-017: Account Type is mandatory — validation blocks save without it
- [ ] AC-018: Save & Transact validates, generates schedule, creates plan with unique reference
- [ ] AC-019: ICC transaction type created and behaves like INC
- [ ] AC-020: ICD transaction type created and behaves like IND
- [ ] AC-021: Accounting transactions created correctly (CLR → ICC/ICD)
- [ ] AC-022: Created plans appear in Plan Maintenance and are searchable by claim number
- [ ] AC-023: All Plan Maintenance operations work for claim recovery plans (edit, history, reverse, cancel, delete, settlement, MTA, save, add task)
- [ ] AC-024: event_log entry created on plan creation
- [ ] AC-025: Existing Premium Finance functionality unaffected

---

## 3. Out of Scope

- Instalment Scheme Maintenance configuration — covered by SPEC-39336 / PBI #37528
- Product Risk Maintenance configuration — covered by SPEC-39336 / PBI #37528
- Recovery receipt page "Instalments" button — covered by SPEC-39472 / PBI #24690
- Payment collection/processing for recovery instalments
- Reporting on recovery instalment plans
- Automated plan creation (manual user-initiated only)
- Back Office (WinForms) plan creation — Portal only

---

## 4. Dependencies

| Dependency | Type | Notes |
|------------|------|-------|
| Epic #39336 / PBI #37528 | Hard prerequisite | Scheme configuration, Product Risk config, database schema (scheme_type, transaction_type, recovery_instalments_enabled) |
| Premium Finance plan creation logic | Reuse | Plan reference generation, plan summary, tabs, schedule generation, folder creation |
| Premium Finance bank details management | Reuse | Account type display, Add/Edit bank screens |
| Find Party search screen | Reuse | Existing party search functionality |
| Claims Management (gCLMLibrary) | Internal | Recovery type identification, eligibility checks |
| Portal Web Forms infrastructure | Internal | ASP.NET Web Forms for new Plan Transactions page |
| Transaction type configuration | New | ICC and ICD transaction types must be created |
| Plan Maintenance | Internal | Must be updated to recognise claim recovery plan type |

---

## 5. Risks & Assumptions

| # | Type | Description | Mitigation |
|---|------|-------------|-----------|
| 1 | Assumption | Portal uses ASP.NET Web Forms (confirmed) | N/A |
| 2 | Assumption | Premium Finance plan configuration tabs can be reused as-is for claim recovery | Verify PF tabs are sufficiently decoupled from PF-specific context |
| 3 | Assumption | `document_ref` field on transaction table is the grouping field for auto-selection | Confirmed by user (Q4) |
| 4 | Risk | Creating new transaction types ICC/ICD requires database changes and may affect reporting | Create migration scripts; verify no downstream reporting dependencies |
| 5 | Risk | Plan Maintenance awareness of new plan type may require filtering/display changes | Assess Plan Maintenance code for plan-type assumptions |
| 6 | Assumption | Bank details management reuses existing PF screens without modification | Verify PF bank screens are not PF-context-specific |
| 7 | Risk | Epic #39336 not yet complete — this feature cannot function without it | Sequence delivery; develop Portal UI in parallel if schema is available |
| 8 | Assumption | Override Interest Rate is plan-specific (one-off), not permanent scheme change | Confirmed by user (Q6) |
| 9 | Assumption | Commission Override follows same logic as Premium Finance | Confirmed by user (Q7) |
| 10 | Assumption | Currency logic when "Use Transaction Currency" is unchecked follows PF pattern | Confirmed by user (Q12) |

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

*Generated by AI-SDLC INCEPTION phase. Traceability: PBI #37524 → requirements.md → user-stories.md → design.md → tasks.md*
