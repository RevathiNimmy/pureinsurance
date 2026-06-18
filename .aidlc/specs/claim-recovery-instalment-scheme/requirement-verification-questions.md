# Requirement Verification Questions

**Feature**: Instalment for Claim Recovery - Scheme Configuration
**Spec ID**: SPEC-39336
**Date**: 2026-05-04

---

## Question 1: Product Risk Maintenance — Field Placement

The PBI specifies a "Recovery Receipts on Instalments" checkbox in Product Risk Maintenance. Where exactly should this field be placed on the existing screen?

A) On the existing Product Risk Maintenance form alongside other risk-level configuration fields
B) On a new tab or section within Product Risk Maintenance dedicated to recovery settings
C) In a separate configuration screen accessible from Product Risk Maintenance
X) Other (please describe after [Answer]: tag below)

[Answer]: X This checkbox should be on the 2-Claims tab as a seperate section below Claim Payment section

---

## Question 2: Scheme Type — Database Implementation

The PFScheme table currently stores scheme data for Premium Finance. How should the new "Claim Recovery" Scheme Type be stored?

A) Add a new column `scheme_type` (e.g., tinyint or varchar) to the existing PFScheme table to distinguish between "Premium Finance" and "Claim Recovery"
B) Create a separate table for Claim Recovery schemes (e.g., `CLRScheme`) mirroring PFScheme structure
C) Use an existing column or lookup mechanism already in PFScheme to differentiate scheme types
X) Other (please describe after [Answer]: tag below)

[Answer]: A

---

## Question 3: Transaction Type — Data Source for Recovery Type Identification

The PBI states the system must identify recovery type (Salvage or Third-Party) from the CLR transaction. What field or mechanism in the existing claims data model identifies whether a recovery is Salvage or Third-Party?

A) There is an existing `recovery_type` or similar column on the claim recovery transaction
B) The recovery type is determined by the `transdetail_type_id` on the Transdetail record
C) The recovery type is inferred from the claim party type (e.g., Third-Party claimant vs salvage agent)
D) I'm not sure — this needs investigation in the existing codebase
X) Other (please describe after [Answer]: tag below)

[Answer]: A

---

## Question 4: Instalment Rates — Rate Structure

When configuring rates for Salvage Recovery and Third-Party Recovery under a Claim Recovery scheme, should the rate structure be identical to existing Premium Finance rates?

A) Yes — same rate fields (interest rate, admin fee, etc.) apply to both Salvage and Third-Party recovery types
B) No — Claim Recovery rates need different/additional fields compared to Premium Finance rates
C) Mostly the same, but with some differences (please describe after [Answer]: tag below)
X) Other (please describe after [Answer]: tag below)

[Answer]: A

---

## Question 5: Scheme Selection — Multiple Matching Schemes

When creating a recovery instalment plan, if multiple Claim Recovery schemes match the criteria (correct Scheme Type + Transaction Type), how should the system behave?

A) Present all matching schemes to the user and let them choose
B) Apply a default scheme based on priority/configuration and allow override
C) Only one scheme per Transaction Type should be allowed per company (enforce uniqueness)
X) Other (please describe after [Answer]: tag below)

[Answer]: A

---

## Question 6: Existing Premium Finance Stored Procedures

The existing Premium Finance workflow uses `spu_PF*` stored procedures. Should the new Claim Recovery functionality:

A) Extend existing `spu_PF*` procedures with Scheme Type filtering parameters
B) Create new `spu_CLR*` or `spu_PFCLR*` stored procedures specifically for Claim Recovery
C) A mix — reuse read procedures with filtering, but create new write procedures
X) Other (please describe after [Answer]: tag below)

[Answer]: A

---

## Question 7: Navigator XM — Workflow Changes

Does the recovery instalment creation need a new Navigator XM roadmap, or will it be integrated into the existing claims recovery workflow?

A) New roadmap XML file specifically for recovery instalment creation
B) Modify existing claims recovery roadmap to include instalment option
C) No roadmap changes needed — the instalment option will be triggered from within existing claims screens
X) Other (please describe after [Answer]: tag below)

[Answer]: B

---

## Question 8: Validation — Scheme Type Enforcement

Should the Scheme Type separation (Claim Recovery vs Premium Finance) be enforced at the database level (e.g., CHECK constraints, separate tables) or only at the application level?

A) Database level — add constraints to prevent cross-contamination
B) Application level only — business logic in VB.NET components handles filtering
C) Both — database constraints as a safety net plus application-level filtering
X) Other (please describe after [Answer]: tag below)

[Answer]: A

---

## Question 9: Audit Trail — Configuration Changes

What level of audit logging is required for Claim Recovery scheme configuration changes?

A) Standard event_log entries (same as existing Premium Finance scheme changes)
B) Enhanced logging with before/after values for all configuration changes
C) Standard logging is sufficient — no special audit requirements beyond existing patterns
X) Other (please describe after [Answer]: tag below)

[Answer]: A

---

## Question 10: Edge Case — Recovery Already on Instalment

What should happen if a user attempts to create a second instalment plan for a recovery that already has an active instalment plan?

A) Block it — only one active instalment plan per recovery transaction
B) Allow it — multiple instalment plans can exist (e.g., for partial recoveries)
C) Warn the user but allow them to proceed
X) Other (please describe after [Answer]: tag below)

[Answer]: the user can change the instalment scheme through instalment plan maintenance task which is not covered under this development

**Follow-up clarification**: Block — the system should not allow a second instalment plan for a recovery that already has an active one. Prevent creation entirely.


---

## Question 11: Security Extensions

Should security extension rules be enforced for this project?

A) Yes — enforce all SECURITY rules as blocking constraints (recommended for production-grade applications)
B) No — skip all SECURITY rules (suitable for PoCs, prototypes, and experimental projects)
X) Other (please describe after [Answer]: tag below)

[Answer]: B

---

## Question 12: Property-Based Testing Extension

Should property-based testing (PBT) rules be enforced for this project?

A) Yes — enforce all PBT rules as blocking constraints (recommended for projects with business logic, data transformations, serialization, or stateful components)
B) Partial — enforce PBT rules only for pure functions and serialization round-trips (suitable for projects with limited algorithmic complexity)
C) No — skip all PBT rules (suitable for simple CRUD applications, UI-only projects, or thin integration layers with no significant business logic)
X) Other (please describe after [Answer]: tag below)

[Answer]: B

---

## Question 13: User Stories Inclusion

This feature has multiple user personas (Claims Handler, System Administrator, Back-Office Staff) and complex business rules. Should we include a formal User Stories stage?

A) Yes — create detailed user stories with personas and acceptance criteria (recommended given the complexity)
B) No — the requirements document with acceptance criteria is sufficient
X) Other (please describe after [Answer]: tag below)

[Answer]: A

---

*Please fill in all [Answer]: tags above and save the file. I'll review your answers before proceeding to finalize the requirements document.*
