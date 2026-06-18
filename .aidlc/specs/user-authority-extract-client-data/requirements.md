# Feature Specification: New User Authority to Extract Client Data

**Spec ID**: `SPEC-39906`
**Date**: 2026-06-01
**Status**: Draft
**Author**: AI Agent (Kiro)
**Source PBI**: ADO #39413
**Epic**: ADO #39906

---

## 1. Overview

Introduce a new user authority "Can Extract Client Data" that controls access to the EXTRACT CLIENT DATA button on the Client Details page in the Pure Portal. The authority is configured via a checkbox in User Maintenance (Admin Portal) under Tab 5 – Authorities, Policy sub-tab (Sub-Tab 3), Access section. When unchecked (default for all users), the button is hidden. When checked, the button is visible and the existing extraction workflow operates normally.

This is a security and access control enhancement restricting a sensitive GDPR Subject Access Request data extraction operation to only authorised users.

### Intent Analysis

- **Request Type**: Enhancement (Access control for existing functionality)
- **Scope**: Multiple Components — Admin Portal (User Maintenance checkbox), Pure Portal (Client Details button visibility), Database (authority storage), System Events (audit)
- **Complexity**: Moderate — new authority flag following existing patterns, conditional UI visibility, server-side protection, audit trail

---

## 2. Requirements (EARS Format)

### Functional Requirements

#### User Maintenance — Admin Portal (New Authority Checkbox)

| ID | Type | Requirement |
|----|------|-------------|
| FR-001 | Ubiquitous | The Admin Portal User Maintenance screen SHALL include a new checkbox labelled "Can Extract Client Data" on Tab 5 – Authorities, under the Policy sub-tab (Sub-Tab 3), within the Access section |
| FR-002 | Ubiquitous | The "Can Extract Client Data" checkbox SHALL be positioned in the Access section following the same pattern as the existing "User can view Batch Process Status" checkbox |
| FR-003 | Ubiquitous | The "Can Extract Client Data" checkbox SHALL be unchecked (disabled) by default for all new users |
| FR-004 | Ubiquitous | The application logic SHALL treat NULL or missing authority values as "unchecked" (no migration script required for existing users) |
| FR-005 | Event-driven | WHEN the System Administrator checks the "Can Extract Client Data" checkbox and saves the user record, the system SHALL persist the setting to the existing user authority configuration table |
| FR-006 | Event-driven | WHEN the System Administrator unchecks the "Can Extract Client Data" checkbox and saves the user record, the system SHALL persist the setting to the existing user authority configuration table |

#### Client Details Page — Pure Portal (Button Visibility)

| ID | Type | Requirement |
|----|------|-------------|
| FR-007 | State-driven | WHILE the "Can Extract Client Data" authority is unchecked for a user, WHEN that user navigates to the Client Details page in the Portal, the system SHALL hide the EXTRACT CLIENT DATA button completely (not rendered in the page) |
| FR-008 | State-driven | WHILE the "Can Extract Client Data" authority is checked for a user, WHEN that user navigates to the Client Details page in the Portal, the system SHALL display the EXTRACT CLIENT DATA button in its standard position |
| FR-009 | State-driven | WHILE the "Can Extract Client Data" authority is checked for a user, WHEN that user clicks the EXTRACT CLIENT DATA button, the system SHALL proceed with the existing extraction workflow (password prompt, zip file generation, download) without any change to current behaviour |
| FR-010 | Ubiquitous | The Portal SHALL check the user's "Can Extract Client Data" authority on page load of Client Details and cache the result for the session |

#### Server-Side Protection

| ID | Type | Requirement |
|----|------|-------------|
| FR-011 | State-driven | WHILE the "Can Extract Client Data" authority is unchecked for a user, IF that user's request reaches the server-side code-behind handler for client data extraction, the system SHALL reject the request |
| FR-012 | Event-driven | WHEN the server-side handler rejects an unauthorised extraction request, the system SHALL return the message: "You do not have permission to extract client data. Contact your System Administrator." |

#### Audit Trail

| ID | Type | Requirement |
|----|------|-------------|
| FR-013 | Event-driven | WHEN the "Can Extract Client Data" authority is changed for a user (checked or unchecked), the system SHALL generate a system event following the same pattern as other User Maintenance configuration changes |
| FR-014 | Ubiquitous | The system event for authority changes SHALL capture: the administrator who made the change, the affected user, the old value, the new value, and the timestamp |

#### Authority Scope

| ID | Type | Requirement |
|----|------|-------------|
| FR-015 | Ubiquitous | The "Can Extract Client Data" authority SHALL be a single global flag per user, applying across all branches and products |

### Non-Functional Requirements

| ID | Category | Requirement | Target |
|----|----------|-------------|--------|
| NFR-001 | Technology | Admin Portal User Maintenance changes must be implemented in the Admin Portal platform | Admin Portal |
| NFR-002 | Technology | Client Details page changes must use ASP.NET Web Forms (Pure Portal) | ASP.NET Web Forms |
| NFR-003 | Compatibility | Must target .NET Framework 4.8 for Portal components | .NET 4.8 |
| NFR-004 | Data Access | Authority storage must use the existing user authority configuration table | Existing schema pattern |
| NFR-005 | Backward Compatibility | Existing EXTRACT CLIENT DATA functionality must remain unchanged for authorised users | Zero regression |
| NFR-006 | Security | Server-side authority check must prevent bypass of UI-level hiding | Defence in depth |
| NFR-007 | Security | NULL/missing authority values must default to "denied" (secure by default) | Secure default |
| NFR-008 | Audit | Authority changes must generate system events consistent with existing patterns | Standard audit |
| NFR-009 | UX | Button visibility behaviour must follow existing Portal patterns for authority-controlled features | Consistent UX |
| NFR-010 | Investigation | Determine whether EXTRACT CLIENT DATA button is controlled via Navigator XM or hardcoded — implement authority check using the same mechanism | Match existing pattern |

### Acceptance Criteria

- [ ] AC-001: "Can Extract Client Data" checkbox appears on Admin Portal User Maintenance, Tab 5 – Authorities, Policy sub-tab, Access section
- [ ] AC-002: Checkbox follows same visual pattern and positioning as "User can view Batch Process Status"
- [ ] AC-003: New users have the checkbox unchecked by default
- [ ] AC-004: Existing users (with NULL value) are treated as unchecked — no migration required
- [ ] AC-005: Checking and saving persists the authority to the user authority configuration table
- [ ] AC-006: Unchecking and saving persists the authority to the user authority configuration table
- [ ] AC-007: User WITHOUT authority: EXTRACT CLIENT DATA button is completely hidden on Client Details page
- [ ] AC-008: User WITH authority: EXTRACT CLIENT DATA button is visible and functional (existing workflow unchanged)
- [ ] AC-009: Authority is checked on page load and cached for the session
- [ ] AC-010: Server-side handler rejects extraction requests from unauthorised users with specific error message
- [ ] AC-011: System event generated when authority is changed (captures admin, user, old value, new value, timestamp)
- [ ] AC-012: Authority is global per user (not branch/product specific)
- [ ] AC-013: Existing extraction workflow for authorised users is completely unaffected

---

## 3. Out of Scope

- Migration script for existing users (application treats NULL as unchecked)
- Changes to the extraction workflow itself (password prompt, zip generation, download)
- New roles or role-based access control changes (uses existing authority pattern)
- Back Office WinForms changes (User Maintenance is in Admin Portal)
- Reporting on authority usage
- Bulk authority assignment (individual user configuration only)

---

## 4. Dependencies

| Dependency | Type | Notes |
|------------|------|-------|
| Admin Portal User Maintenance | Prerequisite | User Maintenance task must be migrated to Admin Portal before this checkbox can be added |
| Existing user authority configuration table | Internal | Same table/pattern as "User can view Batch Process Status" |
| Pure Portal Client Details page | Internal | Existing page where EXTRACT CLIENT DATA button lives |
| System event generation mechanism | Internal | Same pattern as other User Maintenance configuration changes |
| "User can view Batch Process Status" pattern | Reference | Follow same implementation pattern for the new checkbox |

---

## 5. Risks & Assumptions

| # | Type | Description | Mitigation |
|---|------|-------------|-----------|
| 1 | Assumption | User Maintenance will be migrated to Admin Portal before this feature is implemented | Confirm migration timeline; this feature depends on it |
| 2 | Assumption | Existing user authority table can accommodate a new flag without schema changes | Investigate table structure — may need a new column or use existing extensible pattern |
| 3 | Assumption | NULL/missing values in authority table are safely treated as "unchecked" by application logic | Verify no existing code assumes non-NULL for authority fields |
| 4 | Risk | EXTRACT CLIENT DATA button mechanism (Navigator XM vs hardcoded) is unknown | Investigate existing implementation before design phase |
| 5 | Assumption | System event generation for authority changes follows an established pattern | Verify pattern exists for other authority checkboxes (e.g., Batch Process Status) |
| 6 | Assumption | Authority check cached per session is sufficient (no real-time revocation needed) | Confirm acceptable lag between admin change and Portal enforcement |
| 7 | Risk | Admin Portal technology stack may differ from Back Office WinForms patterns | Investigate Admin Portal technology and authority management patterns |

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

*Generated by AI-SDLC INCEPTION phase. Traceability: PBI #39413 → requirements.md → user-stories.md → design.md → tasks.md*
