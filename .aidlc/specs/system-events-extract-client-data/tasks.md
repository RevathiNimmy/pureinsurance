# Tasks — System Events on Extracting Client Data

**Feature**: System Events on Extracting Client Data
**Source PBI**: ADO #39544
**Related Epic**: ADO #39586
**Date**: 2026-06-08 (revised v2 — System Events view path tasks added)

---

## Task 1: Seed Data Migration Script

**Priority**: 1
**Status**: Available
**Blocked By**: None
**Repo**: `PureInsurance` — `Databases/After Change/`
**Estimated Effort**: Small (< 1 hour)

**Description**:  
Create a single idempotent migration script that seeds all required lookup data:
1. Insert new row into `event_type`: `event_type_code = 'CLIEXTRACT'`, `description = 'Client Data Extracted'`
2. Insert new row into `Audit_Trail_Modules`: `ModuleName = 'Extract Client Data'` — note the `Modules_id` that is auto-assigned (needed for Task 2)

**Acceptance**:
- Both rows inserted if not already present (idempotent guards)
- `event_type_id` and `Modules_id` are identity columns — no need to specify them
- Confirm all required NOT NULL columns by checking existing rows in each table

**Files**:
- `Databases/After Change/PBI39544_Add_EventType_And_AuditModule.sql`

---

## Task 2: New Stored Procedure — spu_add_client_data_extract_audit_trail

**Priority**: 2
**Status**: Blocked (by Task 1 — needs `Modules_id` from `Audit_Trail_Modules`)
**Blocked By**: Task 1
**Repo**: `PureInsurance` — `Databases/Pure/Procedures/`
**Estimated Effort**: Small (1-2 hours)

**Description**:  
Create new stored procedure `spu_add_client_data_extract_audit_trail` that inserts one row into `configuration_audit_master` and one row into `configuration_audit_details`.

Inputs: `@UserId INT`, `@ClientCode VARCHAR(100)`, `@ModuleId INT`

The proc inserts:
- `configuration_audit_master`: `UniqueId=NEWID()`, `Module_Id=@ModuleId`, `ModuleName='Extract Client Data'`, `UpdateDate=GETDATE()`, `UserId=@UserId`
- `configuration_audit_details`: `key_field_desc='Extract Client Data'`, `FieldDisplayName='Extract Client Data / ' + @ClientCode`, `OldValue=''`, `NewValue=''`, `Type='U'`

**Note**: Verify the `diff_count = 1` CTE condition in `spu_get_audit_trail_details` does not filter out rows where OldValue = NewValue = empty — adjust the proc's `FieldName`/`OldValue` values if needed so the record is returned by the read query.

**Files**:
- `Databases/Pure/Procedures/A-B/spu_add_client_data_extract_audit_trail.sql`

---

## Task 3: REST API — AddClientDataExtractAuditTrail Command

**Priority**: 3
**Status**: Blocked (by Task 2)
**Blocked By**: Task 2
**Repo**: `PureInsurance.REST`
**Estimated Effort**: Medium (2-3 hours)

**Description**:  
Add new CQRS command following the `AddEvent` pattern exactly.

**Files to create**:
- `MicroServices/Core/PureInsurance.REST.Core.Application/Core/Commands/AddClientDataExtractAuditTrail/AddClientDataExtractAuditTrailCommand.cs`
- `...AddClientDataExtractAuditTrailCommandBase.cs` — fields: `PartyKey (int)`, `ClientCode (string)`, `LoginUserName`, `BranchCode`
- `...AddClientDataExtractAuditTrailCommandHandler.cs`
- `...AddClientDataExtractAuditTrailCommandValidator.cs`
- `...AddClientDataExtractAuditTrailCommandResponse.cs`
- `MicroServices/Core/PureInsurance.REST.Core.Application/Contracts/IAddClientDataExtractAuditTrailService.cs`
- `MicroServices/Core/PureInsurance.REST.Core.Application/Services/AddClientDataExtractAuditTrailService.cs`

**Files to modify**:
- `MicroServices/Core/PureInsurance.REST.Core.API/Controllers/CoreController.cs` — add `POST /core/clientDataExtractAuditTrail` route
- `PureInsurance.REST.Common.Repositories/Contracts/IAuditTrailRepository.cs` — add `AddClientDataExtractAuditTrail()` method
- `PureInsurance.REST.Common.Repositories/AuditTrailRepository.cs` — implement the method calling `spu_add_client_data_extract_audit_trail`
- `PureInsurance.REST.Common.Repositories/StoredProcedures.cs` — add constant `AddClientDataExtractAuditTrail = "spu_add_client_data_extract_audit_trail"`

**Acceptance**:
- `POST /core/clientDataExtractAuditTrail` returns 200
- Row appears in `configuration_audit_master` and `configuration_audit_details`

---

## Task 4: Portal — NexusProvider Method + ApiMethods entry

**Priority**: 4
**Status**: Blocked (by Task 3)
**Blocked By**: Task 3
**Repo**: `PureInsurance`
**Estimated Effort**: Small (1 hour)

**Description**:  
Add NexusProvider support for the new REST endpoint, following the `AddEvent` pattern in `ProviderSAMForInsuranceV2.Core.vb`.

**Files to modify**:
- `Web Portal/Nexus/NexusProvider.SAMForInsurance/ApiMethods.vb` — add:
  ```vbnet
  Friend Shared ReadOnly AddClientDataExtractAuditTrail As String = $"/core/clientDataExtractAuditTrail"
  ```
- `Web Portal/Nexus/NexusProvider.SAMForInsurance/ProviderSAMForInsuranceV2.Core.vb` — add new `AddClientDataExtractAuditTrail` method override calling the new endpoint

**Acceptance**:
- Method compiles and is callable from `ExtractFilePassword.aspx.vb`

---

## Task 5: Portal Code-Behind — ExtractFilePassword.aspx.vb

**Priority**: 5
**Status**: Blocked (by Task 1, Task 4)
**Blocked By**: Task 1 (event type), Task 4 (NexusProvider method)
**Repo**: `PureInsurance`
**Estimated Effort**: Small (1-2 hours)

**Description**:  
Modify `btnOK_Click` in `Modal/ExtractFilePassword.aspx.vb` to call both event logging methods after a successful extraction, BEFORE `Response.End()`.

**Implementation**:
1. After `abClientDataExtract` non-null/non-empty guard, BEFORE `Response.Clear()`:
2. **Path A — Client Events tab**:
   - Look up `EventTypeKey` for `"CLIEXTRACT"` via `oWebservice.GetList(NexusProvider.ListType.PMLookup, "Event_type", False, False)`
   - Call `oWebservice.AddEvent(oEventDetails)` with `.RtfText = "Client Data Extracted"`, `.EventTypeKey = key`, `.PartyKey = party.Key`, `.UserName = Session(CNLoginName)`, `.EventDate = Now()`
3. **Path B — System Events page**:
   - Get `ClientCode` from `DirectCast(Session(CNParty), NexusProvider.BaseParty).UserName`
   - Get `UserId` by looking up `Session(CNLoginName)` in PMUser (or pass username and resolve server-side in SP/service)
   - Call `oWebservice.AddClientDataExtractAuditTrail(partyKey, clientCode)`
4. Wrap BOTH calls in a single Try/Catch — never block the download

**Files**:
- `Web Portal/Nexus/Pure.Portals/Modal/ExtractFilePassword.aspx.vb`

**Acceptance**:
- After extracting client data:
  - New event appears on the client's Events tab with type "Client Data Extracted"
  - New row appears in `secure/SystemEvents.aspx` grid showing "Extract Client Data" / "Extract Client Data / {ClientCode}"
  - File download is not affected if either event call throws
