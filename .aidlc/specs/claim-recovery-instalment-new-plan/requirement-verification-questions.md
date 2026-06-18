# Requirement Verification Questions — Instalment for Claim Recovery - New Plan

**Feature**: Instalment for Claim Recovery - New Plan
**Spec ID**: SPEC-39489
**Source PBI**: ADO #37524
**Date**: 2026-05-07

---

## Context

PBI #37524 describes the full end-to-end Portal workflow for creating instalment plans for claim recovery transactions. This includes the Plan Transactions screen (search/select), scheme selection, plan configuration with multiple tabs, bank details, and plan creation with accounting transactions.

Two related specs already exist:
- **SPEC-39336** (PBI #37528): Back-office scheme configuration (Scheme Type, Rates, Product Risk config)
- **SPEC-39472** (PBI #24690): Portal basic instalment button/tab on recovery receipt page

PBI #37524 appears to be the overarching feature covering the full "Finance Menu → New Plan" workflow which is a separate entry point from the recovery receipt page.

---

## Questions

### Q1: Entry Point Clarification
The PBI describes access via "Finance Menu → New Plan" which opens a "Plan Transactions" screen. The existing portal spec (SPEC-39472/PBI #24690) covers an "Instalments" button on the recovery receipt page.

Are these two separate entry points to the same underlying functionality, or is "Finance Menu → New Plan" a completely different screen/workflow from the receipt-page approach?

[Answer]:

---

### Q2: Technology Platform
The PBI states "Portal only development". Is this the same ASP.NET Web Forms Portal referenced in the existing portal spec, or is there a different Portal technology (e.g., React, Angular, MVC)?

A) ASP.NET Web Forms (same as existing Pure Portal)
B) ASP.NET MVC
C) React/Angular SPA
D) Other (please describe)

[Answer]:

---

### Q3: Plan Transactions Screen — Scope
The Plan Transactions screen allows searching by Party Code and Claim Number. Is this screen:

A) A brand new standalone page accessible from Finance Menu → New Plan (independent of the recovery receipt workflow)
B) An enhancement to the existing recovery receipt page
C) Both — accessible from Finance Menu AND from the recovery receipt page

[Answer]:

---

### Q4: Transaction Selection — Document Reference Grouping
The PBI states: "If multiple rows belong to the same document reference, selecting a single checkbox must automatically select all transactions under that document reference number."

Is the "document reference" the same as the existing `document_ref` field on the transaction table, or is it a different field?

[Answer]:

---

### Q5: Instalment Plan Configuration Tabs
The PBI describes 6 tabs: Summary, Instalments, Breakdown, Override, Finance Details, Deposit. 

Are these tabs identical to the existing Premium Finance plan configuration tabs (reuse existing UI), or do they need to be built from scratch for the claim recovery context?

A) Reuse existing Premium Finance plan configuration tabs as-is
B) Reuse with minor modifications (please describe differences)
C) Build new tabs specific to claim recovery
D) Mix — some reused, some new (please specify)

[Answer]:

---

### Q6: Override Tab — Interest Rate Override
The PBI mentions "Override Interest Rate" with a checkbox and new rate entry. Does this override:

A) Apply only to this specific plan (one-off override)
B) Override the scheme rate permanently for this party
C) Other (please describe)

[Answer]:

---

### Q7: Override Tab — Commission Override
The PBI mentions "Commission Override" with a "commission reference". What is a commission reference in this context?

[Answer]:

---

### Q8: Bank Details — Account Types
The PBI states "Account Type - those configured for that Party" with Add/Edit bank functionality. Is this:

A) Reusing the existing party bank details management (same screens/logic as Premium Finance)
B) A new bank details section specific to claim recovery
C) A simplified version of existing bank details

[Answer]:

---

### Q9: Accounting Transactions — ICC/ICD
The PBI states instalment credits = ICC and debits = ICD (similar to INC/IND in Premium Finance). Do ICC and ICD transaction types already exist in the system, or do they need to be created?

A) Already exist — just need to be used in this context
B) Need to be created as new transaction types
C) Not sure — needs investigation

[Answer]:

---

### Q10: Plan Maintenance
The PBI states "no separate development required for maintaining the plan" and existing Plan Maintenance should work for claim recovery plans. Does this mean:

A) Zero code changes needed — existing Plan Maintenance already handles any plan type
B) Minor configuration/filtering changes needed so Plan Maintenance can find/display claim recovery plans
C) The Plan Maintenance screens need to be aware of the new plan type but logic is the same

[Answer]:

---

### Q11: Relationship to Existing Specs
Given that SPEC-39336 (scheme config) and SPEC-39472 (portal receipt button) already exist, should this spec (PBI #37524) cover:

A) Only the "Finance Menu → New Plan" workflow (Plan Transactions screen, search, selection, full configuration) — treating it as a separate entry point
B) The complete feature including what's in SPEC-39472 (superseding it)
C) Only the gaps not covered by the other two specs (plan configuration tabs, bank details, accounting transactions, plan maintenance integration)

[Answer]:

---

### Q12: "Use Transaction Currency" Field
The Plan Summary includes "Use Transaction Currency" — when selected, the claim recovery transaction currency is used. What happens when it's NOT selected?

A) System uses a default currency (e.g., company base currency)
B) User must manually select a currency
C) Other (please describe)

[Answer]:

---

### Q13: Claim Number Search
The PBI says "Claim No should refine the search for only the Recovery transaction related to that claim." Can a user search by Claim Number alone (without Party Code), or is Party Code always required?

A) Party Code is mandatory; Claim Number is optional refinement
B) Either Party Code OR Claim Number can be used independently
C) At least one must be provided, but either works alone

[Answer]:

---

### Q14: Eligible Transactions
The PBI states "System should read the Product Risk Maintenance configuration" to determine eligible transactions. Beyond the "Recovery Receipts on Instalments" flag, are there other eligibility criteria?

A) Only the product-level flag determines eligibility
B) Additional criteria exist (please describe — e.g., outstanding amount > 0, transaction status, age)
C) Not sure — needs investigation

[Answer]:

---

### Q15: Security Extension
Should the Security Baseline extension be enabled for this spec (audit logging, input validation, RBAC checks)?

A) Yes — enable Security Baseline
B) No — not needed for this feature

[Answer]:

---

*Please fill in all [Answer]: tags and return this document for requirements generation.*
