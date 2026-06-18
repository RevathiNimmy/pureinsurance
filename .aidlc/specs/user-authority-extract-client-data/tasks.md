# Tasks — New User Authority to Extract Client Data

**Feature**: New User Authority to Extract Client Data
**Spec ID**: SPEC-39906
**Source PBI**: ADO #39413
**Epic**: ADO #39906
**Date**: 2026-06-01

---

## Task Dependency Graph

```
T1 (Investigation) ──────────────────────────────┐
                                                  │
T2 (Database Schema) ─────────────────────────────┤
                                                  │
T3 (Admin Portal Checkbox) ──── depends on T1,T2  │
                                                  │
T4 (Portal Button Visibility) ── depends on T1,T2 │
                                                  │
T5 (Server-Side Protection) ──── depends on T4    │
                                                  │
T6 (Audit/System Events) ─────── depends on T3    │
                                                  │
T7 (Integration Testing) ─────── depends on T3,T4,T5,T6
```

---

## Tasks

### T1: Investigation — Existing Authority Pattern & Button Mechanism

**Story**: US-001, US-002
**Priority**: 1 (Blocker — all other tasks depend on findings)
**Estimate**: 2 story points

**Description**:
Investigate the existing implementation patterns before any code changes:

1. **Authority storage pattern**: Examine how "User can view Batch Process Status" is stored
   - Identify the table name and column/row pattern
   - Determine if it's column-based (one column per authority) or row-based (key-value)
   - Document the data type and nullable behaviour

2. **Admin Portal technology**: Examine the Admin Portal User Maintenance screen
   - Identify the technology stack (React, Angular, ASP.NET, etc.)
   - Locate the code for Tab 5 – Authorities, Policy sub-tab, Access section
   - Identify how "User can view Batch Process Status" checkbox is implemented

3. **EXTRACT CLIENT DATA button mechanism**: Examine the Client Details page
   - Determine if the button is controlled via Navigator XM roadmap or hardcoded
   - Locate the existing button code/configuration
   - Identify the server-side handler for the button click

4. **System event pattern**: Examine how authority changes generate system events
   - Identify the event generation mechanism for other authority changes
   - Document the event structure (table, fields, format)

**Deliverables**:
- Investigation findings document with table names, file paths, and patterns
- Confirmed approach for T2–T6

**Dependencies**: None

---

### T2: Database Schema — Add Authority Flag

**Story**: US-001
**Priority**: 2
**Estimate**: 1 story point

**Description**:
Add the "Can Extract Client Data" authority flag to the database, following the pattern identified in T1.

**If column-based pattern**:
- Add `can_extract_client_data BIT NULL` column to the user authority table
- No data migration needed (NULL = unchecked by application logic)

**If row-based pattern**:
- Define the authority key/identifier for the new flag
- No schema change needed — rows created on first grant

**Deliverables**:
- Database change script (if column-based)
- Authority key definition (if row-based)

**Dependencies**: T1

---

### T3: Admin Portal — "Can Extract Client Data" Checkbox

**Story**: US-001, US-004
**Priority**: 3
**Estimate**: 5 story points

**Description**:
Add the "Can Extract Client Data" checkbox to the User Maintenance screen in the Admin Portal.

1. Add checkbox to Tab 5 – Authorities, Policy sub-tab (Sub-Tab 3), Access section
2. Position after/alongside "User can view Batch Process Status" (same pattern)
3. Load authority value on page load (NULL → unchecked)
4. Save authority value on user record save
5. Detect value change and generate system event on change (T6 integration)

**Acceptance Criteria**:
- Checkbox appears in correct location
- Default state is unchecked for new users
- NULL values display as unchecked
- Save persists the value correctly
- Follows same visual and code pattern as "User can view Batch Process Status"

**Dependencies**: T1, T2

---

### T4: Pure Portal — Button Visibility on Client Details Page

**Story**: US-002, US-003
**Priority**: 3
**Estimate**: 3 story points

**Description**:
Add authority check to the Client Details page to conditionally show/hide the EXTRACT CLIENT DATA button.

1. On page load, check user's "Can Extract Client Data" authority
2. Cache the authority value in session
3. Set button visibility based on authority (Visible = true/false)
4. If Navigator XM controlled: add authority condition to roadmap XML
5. If hardcoded: add server-side Visible property binding

**Acceptance Criteria**:
- Button hidden when authority is unchecked/NULL
- Button visible when authority is checked
- Authority cached in session (not re-queried on every page load)
- Existing extraction workflow unchanged for authorised users

**Dependencies**: T1, T2

---

### T5: Pure Portal — Server-Side Protection

**Story**: US-002
**Priority**: 4
**Estimate**: 2 story points

**Description**:
Add server-side authority validation to the extraction handler to prevent bypass of UI hiding.

1. In the button click handler / code-behind, re-validate authority before processing
2. If authority is not granted, return error message: "You do not have permission to extract client data. Contact your System Administrator."
3. Do NOT process the extraction request
4. If authority is granted, proceed with existing workflow (no changes)

**Acceptance Criteria**:
- Unauthorised requests rejected with specific error message
- Authorised requests processed normally (zero regression)
- Server-side check is independent of UI visibility (defence in depth)

**Dependencies**: T4

---

### T6: Audit — System Event Generation for Authority Changes

**Story**: US-004
**Priority**: 4
**Estimate**: 2 story points

**Description**:
Ensure system events are generated when the "Can Extract Client Data" authority is changed.

1. Detect when authority value changes (old value ≠ new value) during User Maintenance save
2. Generate system event following the same pattern as other authority changes
3. Event must capture: administrator, affected user, old value, new value, timestamp

**Acceptance Criteria**:
- System event generated on authority grant (unchecked → checked)
- System event generated on authority revoke (checked → unchecked)
- Event contains all required fields
- Follows same mechanism as other User Maintenance configuration changes

**Dependencies**: T3

---

### T7: Integration Testing & Verification

**Story**: All
**Priority**: 5
**Estimate**: 3 story points

**Description**:
End-to-end verification of the complete feature.

1. Set authority in Admin Portal → verify button visible in Portal
2. Remove authority in Admin Portal → verify button hidden in Portal
3. Attempt server-side bypass without authority → verify rejection
4. Verify system events generated for authority changes
5. Verify existing extraction workflow unchanged for authorised users
6. Verify new users default to unchecked (button hidden)
7. Verify existing users with NULL value treated as unchecked

**Acceptance Criteria**:
- All acceptance criteria from requirements.md verified
- No regression to existing functionality
- Audit trail complete and accurate

**Dependencies**: T3, T4, T5, T6

---

## Summary

| Task | Story | Priority | Estimate | Dependencies |
|------|-------|----------|----------|--------------|
| T1: Investigation | US-001, US-002 | 1 | 2 SP | None |
| T2: Database Schema | US-001 | 2 | 1 SP | T1 |
| T3: Admin Portal Checkbox | US-001, US-004 | 3 | 5 SP | T1, T2 |
| T4: Portal Button Visibility | US-002, US-003 | 3 | 3 SP | T1, T2 |
| T5: Server-Side Protection | US-002 | 4 | 2 SP | T4 |
| T6: Audit/System Events | US-004 | 4 | 2 SP | T3 |
| T7: Integration Testing | All | 5 | 3 SP | T3, T4, T5, T6 |
| **Total** | | | **18 SP** | |

---

*Generated by AI-SDLC INCEPTION phase. Traceability: requirements.md → user-stories.md → design.md → tasks.md*
