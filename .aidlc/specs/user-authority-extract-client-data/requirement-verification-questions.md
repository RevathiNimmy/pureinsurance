# Requirement Verification Questions — New User Authority to Extract Client Data

**Feature**: New User Authority to Extract Client Data
**Spec ID**: `SPEC-39906`
**Source PBI**: ADO #39413
**Epic**: ADO #39906
**Date**: 2026-06-01

---

## Context

PBI #39413 describes a new user authority "Can Extract Client Data" that controls access to the Extract Client Data functionality on the Client Details page in the Pure Portal. The feature adds a checkbox to User Maintenance (Tab 5 – Authorities, Policy sub-tab, Access section) and conditionally hides/shows the EXTRACT CLIENT DATA button based on the authority setting.

This is a security/access control enhancement restricting a sensitive GDPR data extraction operation to authorised users only.

---

## Questions

### Q1: Technology Platform — User Maintenance Screen
The User Maintenance screen where the new checkbox is added — is this:

A) Back Office WinForms (VB.NET .NET 4.8)
B) Pure Portal (ASP.NET Web Forms)
C) Both — Back Office for admin, Portal for self-service
D) Other (please describe)

[Answer]: D) This should be done in the Admin Portal when the User Maintenance task is successfully migrated to Admin Portal.

---

### Q2: Technology Platform — Client Details Page
The Client Details page where the EXTRACT CLIENT DATA button is shown/hidden — is this:

A) Pure Portal (ASP.NET Web Forms)
B) Back Office WinForms
C) Both
D) Other (please describe)

[Answer]: A) Pure Portal (ASP.NET Web Forms)

---

### Q3: Database Storage — Authority Configuration
Where is the "Can Extract Client Data" authority value stored? Is there an existing authority/permissions table pattern used for other checkboxes on Tab 5?

A) Existing user authority table (same pattern as other authority checkboxes) — please provide table name if known
B) New table needs to be created
C) Not sure — needs investigation of existing authority storage pattern

[Answer]: A) Existing user authority table (same pattern as other authority checkboxes — same as "User can view Batch Process Status" checkbox)

---

### Q4: Default Value — Existing Users (Migration)
The PBI states the checkbox should be unchecked by default for all existing users after upgrade. Does this require:

A) A database migration script to explicitly set the value to "unchecked" for all existing users
B) The application logic treats NULL/missing value as "unchecked" (no migration needed)
C) Both — migration for data integrity plus application logic as fallback

[Answer]: B) The application logic treats NULL/missing value as "unchecked" (no migration needed)

---

### Q5: API Protection — ClientDataExtract API
The PBI states that if a user without authority calls the ClientDataExtract API directly, it should reject the request. Is there an existing API endpoint for this, or is the "API" referring to the server-side code behind the button?

A) There is a separate REST/Web API endpoint that needs protection
B) It's the server-side code-behind/handler for the button click (no separate API)
C) Both — button handler AND a separate API endpoint
D) Not sure — needs investigation

[Answer]: B) It's the server-side code-behind/handler for the button click (no separate API)

---

### Q6: Audit Trail Mechanism
The PBI states authority changes should be recorded in the audit trail using the same mechanism as other authority changes. Is this:

A) The standard `event_log` table used across the system
B) A specific user authority audit table
C) Both — event_log plus a dedicated authority change log
D) Not sure — needs investigation of existing authority audit pattern

[Answer]: A system event should be generated when the user-level configuration in User Maintenance is changed, following the same pattern as other configuration changes (system event generation).

---

### Q7: Authority Granularity — Per-Branch/Product or Global
Is the "Can Extract Client Data" authority:

A) A single global flag per user (applies across all branches/products)
B) Configurable per branch
C) Configurable per product
D) Configurable per branch AND product combination

[Answer]: A) A single global flag per user (applies across all branches/products)

---

### Q8: User Maintenance — Existing Authority Checkboxes
Are there existing authority checkboxes in the same section (Tab 5, Policy sub-tab, Access section) that this new checkbox should follow the same pattern as? If so, can you name one or two examples?

[Answer]: Yes — add this checkbox in the Access section similar to the "User can view Batch Process Status" checkbox. Follow the same pattern.

---

### Q9: Portal Session — Authority Check Timing
When should the Portal check the user's "Can Extract Client Data" authority?

A) On page load of Client Details (check once, cache for session)
B) On every page load of Client Details (real-time check)
C) On login (authority loaded into session/claims at authentication time)
D) Other (please describe)

[Answer]: A) On page load of Client Details (check once, cache for session)

---

### Q10: Navigator XM — Menu/Roadmap Changes
Does the EXTRACT CLIENT DATA button appear via Navigator XM roadmap configuration, or is it hardcoded in the Portal page?

A) Navigator XM roadmap controls button visibility (XML config change needed)
B) Hardcoded in the Portal page (code change needed to add authority check)
C) Not sure — needs investigation

[Answer]: C) Not sure — check the existing behaviour and work in a similar way.

---

### Q11: Security Extension
Should the Security Baseline extension be enabled for this spec (input validation, RBAC checks, secure defaults)?

A) Yes — enable Security Baseline
B) No — not needed for this feature

[Answer]: A) Yes — enable Security Baseline

---

### Q12: Scope Confirmation — Portal Only or Back Office Too?
The PBI mentions "User Maintenance screen" (typically Back Office) and "Client Details page" (typically Portal). Please confirm the scope:

A) Back Office: User Maintenance checkbox + Portal: Button visibility — both need changes
B) Portal only — User Maintenance is also in the Portal
C) Back Office only
D) Other (please describe)

[Answer]: D) The configuration in User Maintenance should be done in the Admin Portal, and the Client Details page is in the usual Portal. Both portals need changes.

---

### Q13: Error Response — Permission Denied
When a user without authority attempts to access the extraction (either via direct API call or any bypass), what should the error response look like?

A) Generic "Access Denied" / HTTP 403 with no detail
B) Specific message: "You do not have permission to extract client data. Contact your System Administrator."
C) Silent failure — no response, just don't process
D) Follow existing permission error pattern in the system (please describe if known)

[Answer]: B) Specific message: "You do not have permission to extract client data. Contact your System Administrator."

---

### Q14: Testing Scope
Are there existing automated tests (unit/integration) for the User Maintenance authority checkboxes or the Client Details page that this feature should follow?

A) Yes — existing test patterns to follow (please describe framework/location)
B) No existing automated tests — manual testing only
C) Not sure

[Answer]: C) Not sure

---

*Please fill in all [Answer]: tags and return this document for requirements generation.*
