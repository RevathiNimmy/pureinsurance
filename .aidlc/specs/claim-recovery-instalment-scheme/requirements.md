# Feature Specification: Instalment for Claim Recovery - Scheme Configuration

**Spec ID**: `SPEC-39336`
**Date**: 2026-05-04
**Status**: Draft
**Author**: AI Agent (Kiro)
**Source PBI**: ADO #37528
**Epic**: ADO #39336

---

## 1. Overview

Enable instalment-based recovery for insurance claims, allowing debtors to repay recovery amounts (Salvage or Third-Party) over structured instalments rather than a single payment. This requires product-level configuration, a new scheme type for claim recoveries, rate configuration per recovery transaction type, and scheme selection logic during recovery instalment creation.

### Intent Analysis

- **Request Type**: New Feature (Enhancement to existing Claims and Premium Finance modules)
- **Scope**: Multiple Components — Claims Management, Sirius Back Office Core (Instalment Scheme Maintenance), GIS/Product Builder (Product Risk Maintenance), Database (PFScheme), Navigator XM
- **Complexity**: Moderate — extends existing instalment infrastructure with new scheme type, requires cross-component coordination but reuses established patterns

---

## 2. Requirements (EARS Format)

### Functional Requirements

#### Product Risk Maintenance

| ID | Type | Requirement |
|----|------|-------------|
| FR-001 | Ubiquitous | The system SHALL provide a "Recovery Receipts on Instalments" checkbox field on the 2-Claims tab of Product Risk Maintenance, in a separate section below the Claim Payment section (fraSupression), with a default value of unchecked. The setting is stored in the `Risk_Type.recovery_instalments_enabled` column |
| FR-002 | Optional | WHERE "Recovery Receipts on Instalments" is enabled on a product risk, the system SHALL allow claim recoveries for that product to be placed on instalment plans |
| FR-003 | State-driven | WHILE "Recovery Receipts on Instalments" is unchecked for a product, the system SHALL NOT display instalment recovery options for claims under that product |

#### Instalment Scheme Maintenance

| ID | Type | Requirement |
|----|------|-------------|
| FR-004 | Ubiquitous | The system SHALL add "Claim Recovery" as a Scheme Type option in Instalment Scheme Maintenance (Tab 1 - Scheme) dropdown. This requires a new row in the `PFScheme_Type` lookup table with `code = 'CR'` and `description = 'Claim Recovery'` |
| FR-005 | Ubiquitous | The system SHALL use the existing `pfscheme_type_id` foreign key on the PFScheme table to distinguish between "Premium Finance" and "Claim Recovery" schemes (no additional `scheme_type` column needed) |
| FR-006 | State-driven | WHILE Scheme Type = "Claim Recovery", the scheme SHALL only be available in the recovery instalment workflow and SHALL NOT appear in Premium Finance workflows |
| FR-007 | State-driven | WHILE Scheme Type = "Premium Finance", the scheme SHALL only be available in Premium Finance workflows and SHALL NOT appear in recovery instalment workflows |
| FR-008 | Ubiquitous | The system SHALL enforce Scheme Type separation at the database level using CHECK constraints (with IF NOT EXISTS guard) to prevent cross-contamination |

#### Instalment Rates

| ID | Type | Requirement |
|----|------|-------------|
| FR-009 | Ubiquitous | The system SHALL provide Transaction Type options of "Salvage Recovery" and "Third-Party Recovery" for Instalment Rates configuration (PFRF table) when Scheme Type = "Claim Recovery". The rates form (`iPMBPFRF`) must populate `cboProductFamily` with these options via `BuildProductFamilyCombo()` when `m_sSchemeType = "CR"` |
| FR-010 | Ubiquitous | The system SHALL allow separate financial rate configuration for each Transaction Type (Salvage Recovery, Third-Party Recovery) using the same rate structure as existing Premium Finance rates (interest rate, admin fee, etc.) |
| FR-010a | Ubiquitous | The system SHALL persist the Transaction Type value (`transaction_type` column on PFRF table) when saving a rate record via `spu_PFRF_add` and `spu_PFRF_upd`, passing the value through the business layer (`bSIRPFRFBusiness.AddInputParam`) from the interface (`iPMBPFRFFrm.GetTransactionTypeValue()`) |

#### Scheme Selection During Recovery Instalment Creation

| ID | Type | Requirement |
|----|------|-------------|
| FR-011 | Event-driven | WHEN a user creates an instalment plan for a recovery transaction, the system SHALL identify the product linked to the claim and verify "Recovery Receipts on Instalments" is enabled |
| FR-012 | Event-driven | WHEN creating a recovery instalment plan, the system SHALL identify the recovery type (Salvage or Third-Party) from the existing `recovery_type` field on the CLR transaction |
| FR-013 | Event-driven | WHEN retrieving available schemes for a recovery instalment, the system SHALL return only schemes WHERE Scheme Type = "Claim Recovery" AND Transaction Type matches the applicable recovery type |
| FR-014 | Event-driven | WHEN multiple matching schemes exist, the system SHALL present all matching schemes to the user and allow them to choose |
| FR-015 | Event-driven | WHEN the applicable scheme rates are identified, the system SHALL apply the rates configured for the matching Transaction Type |

#### Duplicate Prevention

| ID | Type | Requirement |
|----|------|-------------|
| FR-016 | Unwanted | IF a recovery transaction already has an active instalment plan (linked via PFPremiumFinance.claim_recovery_transaction_id), THEN the system SHALL NOT allow creation of a second instalment plan for that recovery |

#### Database Schema

| ID | Type | Requirement |
|----|------|-------------|
| FR-017 | Ubiquitous | The system SHALL add a `claim_recovery_transaction_id` column to the PFPremiumFinance table to link instalment plans to CLR recovery transactions, with NULL values for Premium Finance instalments (backward compatibility) |

#### Stored Procedures

| ID | Type | Requirement |
|----|------|-------------|
| FR-018 | Ubiquitous | The system SHALL extend existing `spu_PF*` stored procedures with a Scheme Type filtering parameter to support both Premium Finance and Claim Recovery workflows |

#### Navigator XM

| ID | Type | Requirement |
|----|------|-------------|
| FR-019 | Ubiquitous | The system SHALL modify the existing claims recovery Navigator XM roadmap to include the instalment plan creation option |

### Non-Functional Requirements

| ID | Category | Requirement | Target |
|----|----------|-------------|--------|
| NFR-001 | Compatibility | All changes must target .NET Framework 4.8 (VB.NET WinForms) | .NET 4.8 |
| NFR-002 | Data Access | All database operations via stored procedures through dPMDAO | No inline SQL |
| NFR-003 | Backward Compatibility | Existing Premium Finance instalment schemes must continue to function unchanged | Zero regression |
| NFR-004 | Data Integrity | Scheme Type separation enforced at database level via CHECK constraints on PFScheme | No cross-contamination |
| NFR-005 | Audit | All configuration changes logged to event_log using standard patterns (same as existing Premium Finance) | Standard audit trail |
| NFR-006 | Security | All SECURITY extension rules enforced as blocking constraints | Full security baseline |

### Acceptance Criteria

- [ ] AC-001: Product Risk Maintenance 2-Claims tab displays "Recovery Receipts on Instalments" checkbox in a section below Claim Payment (default unchecked)
- [ ] AC-002: When checkbox is unchecked, no instalment recovery options appear for that product's claims
- [ ] AC-003: When checkbox is checked, instalment recovery plan creation is available for that product's claims
- [ ] AC-004: Instalment Scheme Maintenance shows "Claim Recovery" in Scheme Type dropdown
- [ ] AC-005: PFScheme table has a `scheme_type` column with database-level CHECK constraint (with IF NOT EXISTS guard)
- [ ] AC-006: Schemes with Type "Claim Recovery" do not appear in Premium Finance workflows
- [ ] AC-007: Schemes with Type "Premium Finance" do not appear in recovery instalment workflows
- [ ] AC-008: Instalment Rates screen (PFRF table) shows Transaction Type options (Salvage Recovery, Third-Party Recovery) for Claim Recovery schemes
- [ ] AC-009: Rates can be configured independently per Transaction Type using same fields as Premium Finance
- [ ] AC-009a: Transaction Type value is persisted to PFRF.transaction_type on save and correctly restored when reopening the rate record
- [ ] AC-010: During recovery instalment creation, system correctly identifies recovery type from CLR transaction `recovery_type` field
- [ ] AC-011: All matching schemes (correct Scheme Type + Transaction Type) are presented to the user for selection
- [ ] AC-012: Correct rates are applied based on recovery type
- [ ] AC-013: System blocks creation of a second instalment plan when recovery already has an active plan (via PFPremiumFinance.claim_recovery_transaction_id lookup)
- [ ] AC-014: PFPremiumFinance table has `claim_recovery_transaction_id` column (NULL for PF instalments)
- [ ] AC-015: Existing `spu_PF*` stored procedures accept Scheme Type parameter and filter correctly
- [ ] AC-016: Claims recovery Navigator XM roadmap includes instalment plan creation option
- [ ] AC-017: Existing Premium Finance functionality is unaffected
- [ ] AC-018: All configuration changes produce standard event_log entries

---

## 3. Out of Scope

- Actual instalment payment processing/collection for recoveries (separate PBI)
- Modification of existing Premium Finance scheme logic (beyond adding filtering parameter)
- Reporting on recovery instalments
- Automated recovery instalment creation (manual user-initiated only)
- Integration with external payment gateways for recovery collection
- Instalment plan maintenance (changing scheme on existing plan) — separate task

---

## 4. Dependencies

| Dependency | Type | Notes |
|------------|------|-------|
| Product Risk Maintenance (GIS/Product Builder) | Internal Component | New checkbox on 2-Claims tab, below Claim Payment section |
| Instalment Scheme Maintenance (Sirius Back Office Core) | Internal Component | Scheme Type dropdown modification |
| PFScheme table | Database | New `scheme_type` column + CHECK constraint (with IF NOT EXISTS guard) |
| PFRF table | Database | New `transaction_type` column for rate configuration per recovery type |
| PFPremiumFinance table | Database | New `claim_recovery_transaction_id` column to link instalment plans to CLR transactions |
| Risk_Type table | Database | New `recovery_instalments_enabled` column |
| Claims Management (gCLMLibrary) | Internal Component | Recovery type identification from CLR transaction `recovery_type` field |
| dPMDAO | Internal Component | Data access for extended stored procedures |
| Navigator XM | Internal Config | Modify existing claims recovery roadmap |
| Existing `spu_PF*` stored procedures | Database | Extend with Scheme Type filtering parameter |

---

## 5. Risks & Assumptions

| # | Type | Description | Mitigation |
|---|------|-------------|-----------|
| 1 | Risk | Extending existing `spu_PF*` procedures may introduce regression in Premium Finance | Default parameter value preserves existing behaviour; thorough regression testing |
| 2 | Risk | Adding `scheme_type` column to PFScheme requires migration script for existing data | Set default value for existing rows to "Premium Finance"; include in `Databases/After Change/` |
| 3 | Risk | Modifying claims recovery Navigator XM roadmap may affect existing recovery workflows | Test existing recovery flows after roadmap change |
| 4 | Assumption | CLR transactions have an existing `recovery_type` field that reliably identifies Salvage vs Third-Party | Confirmed by user (Q3 answer) |
| 5 | Assumption | "Recovery Receipts on Instalments" applies at the product risk level (not per-policy or per-claim) | Confirmed by PBI description |
| 6 | Assumption | Rate structure for Claim Recovery is identical to Premium Finance (same fields) | Confirmed by user (Q4 answer) |
| 7 | Assumption | Multiple schemes per Transaction Type per company are allowed (user selects) | Confirmed by user (Q5 answer) |
| 8 | Risk | Database CHECK constraint on `scheme_type` may need coordination with DBA team | Include constraint in migration script; test with existing data |

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

*Generated by AI-SDLC INCEPTION phase. Traceability: this spec → design.md → tasks.md*
*Source: PBI #37528 — Epic #39336*
