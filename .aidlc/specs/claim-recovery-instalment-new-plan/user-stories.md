# User Stories — Instalment for Claim Recovery - New Plan

**Feature**: Instalment for Claim Recovery - New Plan
**Spec ID**: SPEC-39489
**Source PBI**: ADO #37524
**Date**: 2026-05-07

---

## Personas

### Claims Handler (CH)
- Processes claim recoveries day-to-day via the Pure Portal
- Creates instalment plans for recovery transactions when debtors request structured repayment
- Accesses the "New Plan" workflow from the Finance Menu

### Finance Administrator (FA)
- Manages instalment plans, monitors payments, handles overrides
- Uses Plan Maintenance to search, edit, and manage existing plans
- Requires audit trail for compliance

---

## User Stories

### US-001: Search Recovery Transactions for Instalment Plan

**As a** Claims Handler
**I want to** search for outstanding claim recovery transactions by Party Code and optionally Claim Number
**So that** I can identify which recovery transactions are eligible for an instalment plan

**Acceptance Criteria:**
- [ ] "New Plan" option accessible from Finance Menu in Pure Portal
- [ ] Plan Transactions screen displays Party Code (mandatory) and Claim Number (optional) fields
- [ ] Party Code button opens Find Party search; selected party populates the field
- [ ] FIND retrieves only eligible transactions (product enabled, outstanding > 0, no active plan)
- [ ] CLEAR resets all search criteria
- [ ] Results display: Document Reference, Claim Number, Party, Transaction Date, Transaction Amount, Outstanding Amount

**Priority**: High
**Estimate**: 8 story points
**Requirements**: FR-001 to FR-010

---

### US-002: Select Recovery Transactions

**As a** Claims Handler
**I want to** select one or more recovery transactions from the search results
**So that** I can include them in an instalment plan

**Acceptance Criteria:**
- [ ] Each result row has a selection checkbox
- [ ] Selecting a checkbox auto-selects all rows sharing the same document_ref
- [ ] Total Selected Amount displays and updates on selection changes
- [ ] Clicking OK with selections opens the Instalment Recovery Screen

**Priority**: High
**Estimate**: 5 story points
**Requirements**: FR-011 to FR-014

---

### US-003: Select Instalment Scheme

**As a** Claims Handler
**I want to** select an instalment scheme from the available Claim Recovery schemes
**So that** the system can generate a repayment plan based on the scheme configuration

**Acceptance Criteria:**
- [ ] Instalment Recovery Screen displays all Claim Recovery schemes matching the recovery type
- [ ] User can select a scheme; selected scheme is highlighted
- [ ] System loads scheme configuration on selection

**Priority**: High
**Estimate**: 3 story points
**Requirements**: FR-015, FR-016

---

### US-004: Configure Instalment Plan

**As a** Claims Handler
**I want to** configure the instalment plan details (preferred day, first payment date, currency, overrides, deposit, bank details)
**So that** the plan matches the debtor's repayment arrangement

**Acceptance Criteria:**
- [ ] Plan Summary fields available: Preferred Day in Month, First Payment Date, Use Transaction Currency
- [ ] 6 tabs display with same behaviour as Premium Finance: Summary, Instalments, Breakdown, Override, Finance Details, Deposit
- [ ] Override Interest Rate applies one-off override to this plan only
- [ ] Commission Override uses same logic as Premium Finance
- [ ] Deposit Override accepts amount or percentage; reduces financed amount
- [ ] Finance Details tab updates dynamically when overrides are applied
- [ ] Bank Details section shows party's account types with Add/Edit bank functionality
- [ ] Account Type is mandatory

**Priority**: High
**Estimate**: 13 story points
**Requirements**: FR-017 to FR-030

---

### US-005: Create Instalment Plan with Accounting Transactions

**As a** Claims Handler
**I want to** save the configured plan and have the system generate the instalment schedule and accounting transactions
**So that** the recovery amount is formally structured into instalments with proper financial records

**Acceptance Criteria:**
- [ ] Save & Transact validates all mandatory fields
- [ ] System generates instalment payment schedule
- [ ] System creates instalment plan with unique reference number
- [ ] Accounting transactions created: CLR (source), ICC (credits), ICD (debits)
- [ ] ICC behaves like INC in Premium Finance
- [ ] ICD behaves like IND in Premium Finance
- [ ] event_log entry created on plan creation

**Priority**: High
**Estimate**: 13 story points
**Requirements**: FR-031 to FR-036, FR-042

---

### US-006: Maintain Claim Recovery Instalment Plans

**As a** Finance Administrator
**I want to** search, view, and manage claim recovery instalment plans through Plan Maintenance
**So that** I can monitor and adjust plans as needed

**Acceptance Criteria:**
- [ ] Claim recovery plans appear in Plan Maintenance search results
- [ ] Plans are searchable by claim number
- [ ] All existing Plan Maintenance operations work: Edit, View History, Reverse, Cancel, Delete, Settlement, MTA, Save, Add Task
- [ ] Plan Maintenance correctly identifies and displays claim recovery plan type

**Priority**: High
**Estimate**: 5 story points
**Requirements**: FR-037 to FR-040

---

## Summary

| Metric | Value |
|--------|-------|
| Total Stories | 6 |
| Total Estimate | 47 story points |
| Personas | 2 (Claims Handler, Finance Administrator) |
| Priority | All High |

---

*Generated by AI-SDLC INCEPTION phase. Traceability: requirements.md → user-stories.md → design.md → tasks.md*
