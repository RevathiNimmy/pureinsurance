# Application Design — System Events on Extracting Client Data

**Feature**: System Events on Extracting Client Data
**Source PBI**: ADO #39544
**Related Epic**: ADO #39586
**Date**: 2026-06-08 (revised v2 — both System Events view and Client Events tab covered)

---

## 1. Architecture Overview

Two separate audit paths must be written when client data is extracted:

```
+---------------------------------------------------------------------+
|  PURE PORTAL                                                         |
|  Modal/ExtractFilePassword.aspx.vb  — btnOK_Click                   |
|                                                                      |
|  1. Authority check (existing — PBI 39413)                           |
|  2. GetClientDataExtract(partyKey, password) — existing              |
|  3. IF byte array non-null AND non-empty:                            |
|     a. [NEW] Call AddEvent(oEventDetails)                            |
|        → REST POST /core/event                                       |
|        → writes event_log + party_public_text                        |
|        (Client Events tab — AC-2)                                    |
|     b. [NEW] Call oWebservice.AddClientDataExtractAuditTrail(...)    |
|        → NEW REST endpoint OR direct stored proc call                |
|        → writes configuration_audit_master + configuration_audit_details |
|        (System Events page — AC-1)                                   |
|  4. Response.BinaryWrite / Response.End — existing                   |
+---------------------------------------------------------------------+
```

---

## 2. Component Design

### 2.1 Path A — Client Events Tab (AC-2)

**Existing infrastructure — minimal change to Portal only.**

| Component | File | Change |
|---|---|---|
| Portal code-behind | `Web Portal/Nexus/Pure.Portals/Modal/ExtractFilePassword.aspx.vb` | Add `AddEvent` call |
| NexusProvider | `NexusProvider.SAMForInsurance/ProviderSAMForInsuranceV2.Core.vb` | No change |
| REST API | `POST /core/event` — `AddEventCommand` / `AddEventService` / `EventUoWRepository` | No change |
| Database SPs | `spe_event_log_add`, `spu_SAM_Party_Public_Text_add` | No change |
| Database data | `event_type` table — new row | Migration script |

**Portal code pattern** (follows `BaseClient.LoadClient` CLVIEW pattern and `Modal/AddEvent.aspx.vb`):

```vbnet
Dim oEventService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
Dim oEventDetails As New NexusProvider.EventDetails
With oEventDetails
    .PartyKey  = DirectCast(Session(CNParty), NexusProvider.BaseParty).Key
    .EventDate = Now()
    .RtfText   = "Client Data Extracted"
    .UserName  = Session(CNLoginName)
    .EventTypeKey = <CLIEXTRACT key looked up via GetList(PMLookup, "Event_type")>
End With
oEventService.AddEvent(oEventDetails)
```

**EventDetails payload**:

| Field | Value |
|---|---|
| `PartyKey` | `DirectCast(Session(CNParty), NexusProvider.BaseParty).Key` |
| `EventTypeKey` | Key for `event_type_code = 'CLIEXTRACT'` (from `event_type` table) |
| `EventDate` | `Now()` |
| `RtfText` | `"Client Data Extracted"` |
| `UserName` | `Session(CNLoginName)` |
| `InsuranceFileKey` | 0 |
| `InsuranceFolderKey` | 0 |
| `ClaimKey` | 0 |

---

### 2.2 Path B — System Events View Page (AC-1)

**New stored procedure + new NexusProvider method + new REST endpoint required.**

The System Events page (`secure/SystemEvents.aspx`) reads from `configuration_audit_details` joined to `configuration_audit_master` via `spu_get_audit_trail_details`. There is **no existing generic write path** for this table from the Portal — the existing `spu_create_audit_trail_for_lookups` is a complex lookup-comparison proc, not suitable for this simple insert.

#### 2.2.1 New Stored Procedure: `spu_add_client_data_extract_audit_trail`

**Repo**: `PureInsurance` — `Databases/Pure/Procedures/`

Inserts one row into `configuration_audit_master` and one row into `configuration_audit_details`:

```sql
CREATE PROCEDURE spu_add_client_data_extract_audit_trail
    @UserId         INT,
    @ClientCode     VARCHAR(100),
    @ModuleId       INT          -- ID from Audit_Trail_Modules for 'Extract Client Data'
AS
BEGIN
    DECLARE @MasterId INT

    INSERT INTO configuration_audit_master (UniqueId, Module_Id, ModuleName, UpdateDate, UserId)
    VALUES (NEWID(), @ModuleId, 'Extract Client Data', GETDATE(), @UserId)

    SET @MasterId = SCOPE_IDENTITY()

    INSERT INTO configuration_audit_details (
        configuration_audit_master_id,
        Type,
        TableName,
        key_field_name,
        key_field_value,
        key_field_desc,
        FieldName,
        FieldDisplayName,
        OldValue,
        NewValue
    )
    VALUES (
        @MasterId,
        'U',
        'Party_cnt',
        'party_cnt',
        '',
        'Extract Client Data',
        'Extract Client Data / Client Code',
        'Extract Client Data / ' + @ClientCode,
        '',
        ''
    )
END
```

> **Note**: Confirm exact `key_field_value` (party key or empty), `TableName`, and `Type` values by checking existing rows in `configuration_audit_details` for context. Adjust to match what the `spu_get_audit_trail_details` query CTE filtering requires (the CTE `diff_count = 1` condition).

#### 2.2.2 New REST API Endpoint (PureInsurance.REST)

**Repo**: `PureInsurance.REST`

New CQRS command following the existing `AddEvent` pattern:

| Layer | File | Notes |
|---|---|---|
| Controller | `CoreController.cs` | New route: `POST /core/clientDataExtractAuditTrail` |
| Command | `Core/Commands/AddClientDataExtractAuditTrail/AddClientDataExtractAuditTrailCommand.cs` | Fields: `PartyKey`, `ClientCode`, `LoginUserName`, `BranchCode` |
| Handler | `AddClientDataExtractAuditTrailCommandHandler.cs` | Validates → calls Service |
| Service | `Services/AddClientDataExtractAuditTrailService.cs` | Calls repository |
| Repository | `PureInsurance.REST.Common.Repositories/AuditTrailRepository.cs` | Add `AddClientDataExtractAuditTrail()` method calling `spu_add_client_data_extract_audit_trail` |
| Interface | `Contracts/IAuditTrailRepository.cs` | Add new method signature |

#### 2.2.3 New ApiMethods entry (PureInsurance)

**File**: `Web Portal/Nexus/NexusProvider.SAMForInsurance/ApiMethods.vb`

```vbnet
Friend Shared ReadOnly AddClientDataExtractAuditTrail As String = $"/core/clientDataExtractAuditTrail"
```

#### 2.2.4 New NexusProvider Method (PureInsurance)

**File**: `Web Portal/Nexus/NexusProvider.SAMForInsurance/ProviderSAMForInsuranceV2.Core.vb`

New `AddClientDataExtractAuditTrail` override calling the new REST endpoint, following the same pattern as `AddEvent`.

---

## 3. Database Tables Affected

| Table | Change | For |
|---|---|---|
| `event_log` | New row per extraction | Client Events tab (AC-2) |
| `party_public_text` | New rows (header + description) | Client Events tab (AC-2) |
| `event_type` | **New row**: code `'CLIEXTRACT'`, desc `'Client Data Extracted'` | AC-3 |
| `configuration_audit_master` | **New row** per extraction | System Events page (AC-1) |
| `configuration_audit_details` | **New row** per extraction | System Events page (AC-1) |
| `Audit_Trail_Modules` | **New row**: `'Extract Client Data'` | System Events filter (AC-4) |

---

## 4. Stored Procedures

| SP | Change | Purpose |
|---|---|---|
| `spe_event_log_add` | No change | Client Events tab write |
| `spu_SAM_Party_Public_Text_add` | No change | Client Events tab text write |
| `spu_add_client_data_extract_audit_trail` | **New** | System Events page write |
| `spu_get_audit_trail_details` | No change | System Events page read |
| `spu_get_audit_trail_moduleList` | No change | System Events filter dropdown |

---

## 5. Files to Change

| Repo | File | Change |
|---|---|---|
| `PureInsurance` | `Web Portal/Nexus/Pure.Portals/Modal/ExtractFilePassword.aspx.vb` | Add both event calls |
| `PureInsurance` | `Web Portal/Nexus/NexusProvider.SAMForInsurance/ApiMethods.vb` | Add new API method constant |
| `PureInsurance` | `Web Portal/Nexus/NexusProvider.SAMForInsurance/ProviderSAMForInsuranceV2.Core.vb` | Add new provider method |
| `PureInsurance` | `Databases/Pure/Procedures/` | New stored procedure file |
| `PureInsurance` | `Databases/After Change/PBI39544_seed.sql` | Seed `event_type`, `Audit_Trail_Modules` rows |
| `PureInsurance.REST` | `MicroServices/Core/PureInsurance.REST.Core.API/Controllers/CoreController.cs` | New route |
| `PureInsurance.REST` | `MicroServices/Core/PureInsurance.REST.Core.Application/Core/Commands/AddClientDataExtractAuditTrail/` | New CQRS command (Command, Handler, Validator, Response) |
| `PureInsurance.REST` | `MicroServices/Core/PureInsurance.REST.Core.Application/Contracts/IAddClientDataExtractAuditTrailService.cs` | New service interface |
| `PureInsurance.REST` | `MicroServices/Core/PureInsurance.REST.Core.Application/Services/AddClientDataExtractAuditTrailService.cs` | New service |
| `PureInsurance.REST` | `PureInsurance.REST.Common.Repositories/Contracts/IAuditTrailRepository.cs` | Add method signature |
| `PureInsurance.REST` | `PureInsurance.REST.Common.Repositories/AuditTrailRepository.cs` | Add method implementation |
| `PureInsurance.REST` | `PureInsurance.REST.Common.Repositories/StoredProcedures.cs` | Add SP name constant |

---

## 6. Security

- Both event calls fire only after the existing PBI 39413 authority guard passes
- Both wrapped in Try/Catch — never block the file download
- No sensitive data (password, file bytes) logged — only client code and username
