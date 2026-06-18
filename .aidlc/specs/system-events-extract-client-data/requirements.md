# Requirements — System Events on Extracting Client Data

**Feature**: System Events on Extracting Client Data
**Source PBI**: ADO #39544
**Related Epic**: ADO #39586
**Parent**: ADO #14752
**Date**: 2026-06-08 (revised v2 — System Events view and Client Events both covered)
**Priority**: 2

---

## 1. User Story

As a Pure Insurance user having authority to view Clients, I want a system event added whenever client data is extracted, so that a proper audit log is maintained for audit purposes.

---

## 2. Acceptance Criteria

### AC-1: System Events View Page Record (`secure/SystemEvents.aspx`)

**When** a user successfully extracts client data  
**Then** a record appears in `secure/SystemEvents.aspx` (the System Events view page shown in the PBI screenshot) with the following grid columns populated:

| Grid Column | Grid Header Resource | Data Source (AuditTrailRepository) | Expected Value |
|---|---|---|---|
| Level | `gv_level` | `key_field_desc` from `configuration_audit_details` | `"Extract Client Data"` |
| Property | `gv_Property` | `FieldDisplayName` from `configuration_audit_details` | `"Extract Client Data / {Client Code}"` |
| Old Value | `gv_OldVal` | `OldValue` from `configuration_audit_details` | blank |
| New Value | `gv_NewVal` | `NewValue` from `configuration_audit_details` | blank |
| Modified By | `gv_ModifiedBy` | `username` from `PMUser` (joined via `configuration_audit_master.UserId`) | Logged-in user |
| Modified On | `gv_ModifiedOn` | `UpdateDate` from `configuration_audit_master` | Date/time of extraction |

**Tables involved**:
- `configuration_audit_master` — one row per extract event (holds `UserId`, `UpdateDate`, `Module_Id`)
- `configuration_audit_details` — one row per extract event (holds `key_field_desc`, `FieldDisplayName`, `OldValue`, `NewValue`)
- `Audit_Trail_Modules` — must have a module entry for "Extract Client Data" so the filter dropdown shows it

**Implementation**: Requires a new stored procedure `spu_add_client_data_extract_audit_trail` (or equivalent) that inserts into both `configuration_audit_master` and `configuration_audit_details`. Called from the Portal after successful extraction.

---

### AC-2: Client Events Tab Record (Client Events panel)

**When** client data is extracted  
**Then** a client event appears on the client's Events tab with:

| Field | Expected Value |
|---|---|
| Event Date | Current date/time |
| Event Type | "Client Data Extracted" |
| Description | "Client Data Extracted" |
| User Name | Logged-in user |
| Context (Detail view `lblContextData`) | "Client data extracted" |
| Details (Detail view `lblSubjectData`) | "Client data extracted" |
| All other fields | blank |

**Tables involved**: `event_log` + `party_public_text`  
**Stored procedures**: `spe_event_log_add` + `spu_SAM_Party_Public_Text_add`  
**Portal call**: `NexusProvider.ProviderBase.AddEvent(oEventDetails)` → REST `POST /core/event`

---

### AC-3: New Event Type for Client Events Tab

A new row in the `event_type` table:
- **event_type_code**: `"CLIEXTRACT"` (max 10 chars, follows `"CLVIEW"` pattern)
- **description**: `"Client Data Extracted"`

Required because `AddEventService` validates `EventTypeKey` against `event_type` table before writing.

---

### AC-4: New Audit Trail Module for System Events Filter

A new row in the `Audit_Trail_Modules` table:
- **ModuleName**: `"Extract Client Data"`

Required so that the Event Type dropdown in `secure/SystemEvents.aspx` includes "Extract Client Data" as a filter option (populated by `spu_get_audit_trail_moduleList`).

---

## 3. Constraints

- Both event records (AC-1 and AC-2) must fire BEFORE `Response.End()` in `btnOK_Click`
- Both must fire only after a successful extraction (non-null, non-empty byte array from `GetClientDataExtract`)
- Both must fail silently — never block the file download
- No changes to the extraction logic or password validation

---

## 4. Out of Scope

- Changes to the existing extraction logic or password handling
- Authority check changes (covered by PBI 39413)
- Back-office WinForms event generation (Portal only)
