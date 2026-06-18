# User Stories — Instalment for Claim Recovery - Scheme Configuration

**Feature**: Instalment for Claim Recovery - Scheme Configuration
**Spec ID**: SPEC-39336
**Date**: 2026-05-06

---

## Personas

### System Administrator (SA)
- Configures products, schemes, and system-level settings
- Responsible for enabling features at the product level
- Uses Product Risk Maintenance and Instalment Scheme Maintenance screens
- Needs clear separation between Premium Finance and Claim Recovery configuration

### Claims Handler (CH)
- Manages claim recoveries day-to-day
- Creates instalment plans for recovery transactions
- Needs to select appropriate schemes and see correct rates applied
- Works within the claims recovery workflow via Navigator XM

### Finance/Back-Office Staff (FO)
- Monitors instalment plans and financial transactions
- Needs confidence that correct rates are applied per recovery type
- Relies on audit trail for configuration changes

---

## User Stories

---

### US-001: Enable Instalment Recovery at Product Level

**As a** System Administrator
**I want to** enable or disable instalment-based recovery for a specific product risk
**So that** I can control which products allow claim recoveries to be placed on instalment plans

**Acceptance Criteria:**
- [ ] A "Recovery Receipts on Instalments" checkbox is visible on the 2-Claims tab of Product Risk Maintenance, in a section below Claim Payment
- [ ] The checkbox defaults to unchecked for new products
- [ ] When checked and saved, the product allows instalment recovery creation
- [ ] When unchecked and saved, instalment recovery options are hidden for that product's claims
- [ ] The change is logged to event_log with standard audit entry

**Priority**: High
**Estimate**: 3 story points

---

### US-002: Configure Claim Recovery Scheme Type

**As a** System Administrator
**I want to** create an instalment scheme with Scheme Type "Claim Recovery"
**So that** recovery-specific schemes are separated from Premium Finance schemes

**Acceptance Criteria:**
- [ ] Instalment Scheme Maintenance (Tab 1 - Scheme) shows "Claim Recovery" in the Scheme Type dropdown alongside "Premium Finance"
- [ ] When "Claim Recovery" is selected and saved, the scheme is stored with `scheme_type` = Claim Recovery in PFScheme
- [ ] A Claim Recovery scheme does not appear in any Premium Finance workflow
- [ ] A Premium Finance scheme does not appear in any recovery instalment workflow
- [ ] Database CHECK constraint prevents invalid `scheme_type` values
- [ ] All other scheme configuration fields (Tab 1 onwards) function identically to Premium Finance

**Priority**: High
**Estimate**: 5 story points

---

### US-003: Configure Instalment Rates by Recovery Transaction Type

**As a** System Administrator
**I want to** configure separate instalment rates for Salvage Recovery and Third-Party Recovery
**So that** different financial terms can be applied based on the type of recovery

**Acceptance Criteria:**
- [ ] When Scheme Type = "Claim Recovery", the Instalment Rates screen shows Transaction Type options: "Salvage Recovery" and "Third-Party Recovery"
- [ ] Rates (interest rate, admin fee, etc.) can be configured independently for each Transaction Type
- [ ] The rate fields are identical in structure to existing Premium Finance rates
- [ ] Saving rates for one Transaction Type does not affect the other
- [ ] Rate configuration changes are logged to event_log

**Priority**: High
**Estimate**: 3 story points

---

### US-004: Create Recovery Instalment Plan

**As a** Claims Handler
**I want to** create an instalment plan for a claim recovery transaction
**So that** the debtor can repay the recovery amount in structured instalments

**Acceptance Criteria:**
- [ ] The instalment plan creation option is available in the claims recovery workflow (Navigator XM roadmap)
- [ ] The system verifies "Recovery Receipts on Instalments" is enabled for the product linked to the claim
- [ ] If not enabled, the instalment option is not visible/available
- [ ] The system identifies the recovery type (Salvage or Third-Party) from the CLR transaction `recovery_type` field
- [ ] Only schemes matching Scheme Type = "Claim Recovery" AND Transaction Type = applicable recovery type are presented
- [ ] If multiple schemes match, all are presented for user selection
- [ ] The selected scheme's rates are applied to the instalment plan
- [ ] The instalment plan is created successfully with correct financial terms

**Priority**: High
**Estimate**: 8 story points

---

### US-005: Prevent Duplicate Instalment Plans

**As a** Claims Handler
**I want to** be prevented from creating a second instalment plan for a recovery that already has an active plan
**So that** financial integrity is maintained and duplicate plans are avoided

**Acceptance Criteria:**
- [ ] When attempting to create an instalment plan, the system checks if the recovery transaction already has an active instalment plan
- [ ] If an active plan exists, the system blocks creation and displays an appropriate message
- [ ] The user is informed they can modify the existing plan via Instalment Plan Maintenance (separate functionality)
- [ ] The validation occurs before scheme selection to avoid unnecessary user effort

**Priority**: Medium
**Estimate**: 2 story points

---

### US-006: Scheme Type Filtering in Existing Premium Finance Workflows

**As a** System Administrator
**I want to** ensure Premium Finance workflows only show Premium Finance schemes (not Claim Recovery schemes)
**So that** there is no cross-contamination between the two scheme types

**Acceptance Criteria:**
- [ ] All existing Premium Finance scheme selection screens filter by Scheme Type = "Premium Finance"
- [ ] Extended `spu_PF*` stored procedures accept a Scheme Type parameter and default to "Premium Finance" for backward compatibility
- [ ] Existing Premium Finance functionality is completely unaffected by the new Claim Recovery scheme type
- [ ] No regression in Premium Finance instalment creation, rate lookup, or scheme maintenance

**Priority**: High
**Estimate**: 5 story points

---

### US-007: Navigator XM Roadmap Update for Recovery Instalments

**As a** Claims Handler
**I want to** access the instalment plan creation option from within the existing claims recovery workflow
**So that** I don't need to navigate to a separate area to set up recovery instalments

**Acceptance Criteria:**
- [ ] The existing claims recovery Navigator XM roadmap includes a step/option for instalment plan creation
- [ ] The option is only visible when "Recovery Receipts on Instalments" is enabled for the product
- [ ] Selecting the option launches the recovery instalment creation workflow
- [ ] Existing claims recovery roadmap steps continue to function unchanged

**Priority**: Medium
**Estimate**: 3 story points

---

### US-008: Audit Trail for Scheme Configuration

**As a** Finance/Back-Office Staff member
**I want to** see audit entries for all Claim Recovery scheme configuration changes
**So that** I can track who changed what and when for compliance purposes

**Acceptance Criteria:**
- [ ] Creating a new Claim Recovery scheme generates a standard event_log entry
- [ ] Modifying scheme settings (rates, Transaction Type configuration) generates event_log entries
- [ ] Enabling/disabling "Recovery Receipts on Instalments" on a product generates an event_log entry
- [ ] Audit entries follow the same format and detail level as existing Premium Finance audit entries

**Priority**: Medium
**Estimate**: 2 story points

---

## Story Map Summary

| Story | Persona | Priority | Estimate | Dependencies |
|-------|---------|----------|----------|--------------|
| US-001 | System Administrator | High | 3 | None |
| US-002 | System Administrator | High | 5 | None |
| US-003 | System Administrator | High | 3 | US-002 |
| US-004 | Claims Handler | High | 8 | US-001, US-002, US-003, US-007 |
| US-005 | Claims Handler | Medium | 2 | US-004 |
| US-006 | System Administrator | High | 5 | US-002 |
| US-007 | Claims Handler | Medium | 3 | US-001 |
| US-008 | Finance/Back-Office | Medium | 2 | US-001, US-002 |

**Total Estimate**: 31 story points

---

*Generated by AI-SDLC INCEPTION phase. Traceability: requirements.md → user-stories.md → design.md → tasks.md*
