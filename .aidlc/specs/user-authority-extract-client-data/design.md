# Application Design — New User Authority to Extract Client Data

**Feature**: New User Authority to Extract Client Data
**Spec ID**: SPEC-39906
**Source PBI**: ADO #39413
**Date**: 2026-06-01
**Last Updated**: 2026-06-08 (Corrected with codebase verification)

---

## 1. Architecture Overview

```
+------------------------------------------------------------------+
|           BACK OFFICE (User Maintenance — WinForms)               |
|                                                                  |
|  Tab 5 – Authorities > Policy Sub-Tab > Access Section           |
|  [NEW] chkCanExtractClientData checkbox                          |
|                                                                  |
|  Save → calls stored procedure to persist column value           |
|       → triggers audit trail (tr_PMUser_Authority_Level_audit_log)|
+------------------------------------------------------------------+
              |
              v
+------------------------------------------------------------------+
|                    DATABASE                                       |
|                                                                  |
|  Table: User_Authorities                                         |
|  [NEW COLUMN]: can_extract_client_data  TINYINT NULL             |
|  Pattern: Column-based (each authority = separate column)        |
|  NULL = unchecked/denied                                         |
|                                                                  |
|  Stored Procedure: spu_Specific_User_Authority_Sel               |
|  SELECT @Authority FROM User_Authorities WHERE user_id=@user_id  |
|  (dynamically selects column by name)                            |
|                                                                  |
|  Audit: tr_PMUser_Authority_Level_audit_log (trigger)            |
+------------------------------------------------------------------+
              ^
              |
+------------------------------------------------------------------+
|              REST API LAYER (PureInsurance.REST)                  |
|                                                                  |
|  Endpoint: GET /core/users/authorityValue                        |
|  Request:  GetUserAuthorityValueQuery                            |
|            { UserAuthorityOption: enum, UserCode: string }       |
|  Response: GetUserAuthorityValueQueryResponse                    |
|            { UserAuthorityValue: string }                        |
|                                                                  |
|  Enum: SSP.PureInsuranceRestAPIHandler.Enums.UserAuthorityOptions|
|  [NEEDS ADDING]: CanExtractClientData = 187                      |
+------------------------------------------------------------------+
              ^
              |
+------------------------------------------------------------------+
|     PORTAL PROVIDER (ProviderSAMForInsuranceV2.Core.vb)          |
|                                                                  |
|  Method: GetUserAuthorityValue(ByRef r_oUserAuthority)           |
|  Maps NexusProvider.UserAuthority.UserAuthorityOptionType enum   |
|  → SSP.PureInsuranceRestAPIHandler.Enums.UserAuthorityOptions    |
|  Calls: ApiClient.Get(ApiMethods.GetUserAuthorityValue, request) |
|  Endpoint: /core/users/authorityValue                            |
+------------------------------------------------------------------+
              ^
              |
+------------------------------------------------------------------+
|           PURE PORTAL (BaseClient.vb — Page_PreRender)           |
|                                                                  |
|  Button: cmdExtractClientData (LinkButton, hardcoded in ASPX)    |
|  Check: UserAuthorityOptionType.CanExtractClientData             |
|  If value = "1" → show button + attach ThickBox modal            |
|  Modal: ExtractFilePassword.aspx                                 |
+------------------------------------------------------------------+
```

---

## 2. Database Layer

### Table: `User_Authorities`

**Pattern**: Column-based — each authority is a separate column (TINYINT).

**Existing columns** (same pattern):
- `is_view_only_client_manager` TINYINT NOT NULL DEFAULT 0
- `has_ViewBatchProcessStatus` TINYINT NULL
- `display_reinsurance` TINYINT NOT NULL DEFAULT 1
- `allow_receipt_reversal` TINYINT NULL
- `has_ManualJournal_authority` TINYINT NULL

**New column required**:
```sql
EXEC DDLAddColumn 'User_Authorities', 'can_extract_client_data', 'TINYINT NULL'
```

NULL = denied (no migration script needed for existing users).

### Stored Procedure: `spu_Specific_User_Authority_Sel`

Already handles the new column dynamically — it takes `@Authority VARCHAR(50)` and builds:
```sql
SELECT @Authority FROM User_Authorities WHERE user_id=@user_id
```

Called with `@Authority = 'can_extract_client_data'`.

### Audit Trigger: `tr_PMUser_Authority_Level_audit_log`

Existing trigger on `User_Authorities` table automatically logs all column changes. No additional audit code needed for the database layer.

### CRUD Procedures:
- `spe_PMUser_Authority_Level_sel` — reads authority record
- `spe_PMUser_Authority_Level_upd` — updates authority record (needs new column in parameter list)
- `spe_PMUser_Authority_Level_add` — adds authority record (needs new column in parameter list)

---

## 3. REST API Layer

### Endpoint: `GET /core/users/authorityValue`

**Request class**: `GetUserAuthorityValueQueryBase` (C#)
```csharp
public class GetUserAuthorityValueQueryBase : BaseRequestType
{
    public UserAuthorityOptions UserAuthorityOption { get; set; }
    public string UserCode { get; set; }
    public int AgentKey { get; set; }
}
```

**Response class**: `BaseGetUserAuthorityValueResponseType` (C#)
```csharp
public class BaseGetUserAuthorityValueResponseType : BaseResponseType
{
    public string UserAuthorityValue { get; set; }
    public int UserAuthorityOptionalValue1 { get; set; }
    public double UserAuthorityOptionalValue2 { get; set; }
    public string UserAuthorityOptionalValue3 { get; set; }
}
```

### ⚠️ CRITICAL GAP: REST API Handler Enum

**File**: `SSP.PureInsuranceRestAPIHandler\Enums\UserAuthorityOptions.cs`

`CanExtractClientData` is **NOT YET ADDED** to this enum. Current last value is `HasManualJournalAuthority = 186`.

**Required change**:
```csharp
HasManualJournalAuthority = 186,
CanExtractClientData = 187  // NEW — PBI 39413
```

Without this, the REST API cannot deserialise the enum value from the portal request and the authority check will fail.

### REST API Microservice (PureInsurance.REST)

The Core API microservice receives the `GET /core/users/authorityValue` request, maps the enum to the column name `can_extract_client_data`, and calls `spu_Specific_User_Authority_Sel`. The microservice enum must also include the new value.

---

## 4. Portal Provider Layer

### File: `NexusProvider.SAMForInsurance\ProviderSAMForInsuranceV2.Core.vb`

**Method**: `GetUserAuthorityValue` (line ~4213)

Call chain:
1. Receives `UserAuthority` DTO with `.UserAuthorityOption = CanExtractClientData`
2. Creates `GetUserAuthorityValueQuery` request
3. Sets `.UserAuthorityOption = r_oUserAuthority.UserAuthorityOption` (passes enum integer value)
4. Calls `ApiClient.Get(ApiMethods.GetUserAuthorityValue, request)` → `GET /core/users/authorityValue`
5. Deserialises response → sets `r_oUserAuthority.UserAuthorityValue`
6. Caches result in `HttpContext.Cache` with key `"UserAuthority_{UserCode}_{EnumName}"`

**No code changes needed** in this file — it's generic and handles any `UserAuthorityOptionType` via enum value passthrough.

### File: `NexusProvider.SAMForInsurance\ApiMethods.vb`

**Constant**: `GetUserAuthorityValue = "/core/users/authorityValue"` — already exists, no change needed.

---

## 5. Portal UI Layer

### File: `Web Portal\Nexus\NexusProvider\Objects\UserAuthority.vb`

**Change** (already done): Added `CanExtractClientData` to `UserAuthorityOptionType` enum:
```vb
'''<summary>
'''User is allowed to extract client data (GDPR SAR)
CanExtractClientData
```

### File: `Web Portal\Nexus\Pure.Portals\App_Code\Nexus\BaseClient.vb`

**Location**: `Page_PreRender` → `Case Mode.View` section

**Implementation** (already done):
```vb
If oMaster.FindControl("cmdExtractClientData") IsNot Nothing Then
    Dim oExtractAuthority As New NexusProvider.UserAuthority
    oExtractAuthority.UserCode = Session(CNLoginName)
    oExtractAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.CanExtractClientData
    Dim oExtractWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
    oExtractWebService.GetUserAuthorityValue(oExtractAuthority)
    If oExtractAuthority.UserAuthorityValue = "1" Then
        oMaster.FindControl("cmdExtractClientData").Visible = True
        DirectCast(oMaster.FindControl("cmdExtractClientData"), LinkButton).Attributes.Add(
            "OnClick", String.Format("javascript: tb_show(null , '{0}/Modal/ExtractFilePassword.aspx?...', null);return false;", AppSettings("WebRoot")))
    End If
End If
```

**Button visibility defaults**:
- Edit mode: `cmdExtractClientData` → `Visible = False`
- Add mode: `cmdExtractClientData` → `Visible = False`
- View mode: Hidden unless authority = "1"

---

## 6. Back Office User Maintenance

### Stored Procedures (CRUD for User_Authorities):
- `spe_PMUser_Authority_Level_sel` — must return new column
- `spe_PMUser_Authority_Level_upd` — must accept and persist new column
- `spe_PMUser_Authority_Level_add` — must accept and persist new column

### UI (WinForms):
- Add checkbox to Tab 5 → Policy sub-tab → Access section
- Bind to `can_extract_client_data` column
- Follow same pattern as `has_ViewBatchProcessStatus` checkbox

---

## 7. Complete File Change List

| Layer | File | Change | Status |
|-------|------|--------|--------|
| **Database** | `Databases\Pure\Structure\PURE_STRUCTURE.sql` | Add column `can_extract_client_data TINYINT NULL` | ❌ Not done |
| **Database** | `Databases\Pure\Procedures\spe\spe_PMUser_Authority_Level_upd.sql` | Add parameter + column | ❌ Not done |
| **Database** | `Databases\Pure\Procedures\spe\spe_PMUser_Authority_Level_sel.sql` | Add column to SELECT | ❌ Not done |
| **REST API Handler** | `SSP.PureInsuranceRestAPIHandler\Enums\UserAuthorityOptions.cs` | Add `CanExtractClientData = 187` | ❌ **CRITICAL — Not done** |
| **REST Microservice** | PureInsurance.REST Core API | Map enum 187 → column `can_extract_client_data` | ❌ Not done |
| **NexusProvider** | `NexusProvider\Objects\UserAuthority.vb` | Add `CanExtractClientData` to enum | ✅ Done |
| **Portal** | `Pure.Portals\App_Code\Nexus\BaseClient.vb` | Authority check + button visibility | ✅ Done |
| **Back Office** | User Maintenance WinForms (Tab 5) | Add checkbox | ❌ Not done |

---

## 8. Security Considerations

| Rule | Implementation |
|------|----------------|
| Defence in depth | UI hiding + server-side validation in ExtractFilePassword.aspx |
| Secure default | NULL in `User_Authorities` = denied; button hidden by default |
| Audit trail | `tr_PMUser_Authority_Level_audit_log` trigger fires on column update |
| No SQL injection | Parameterised stored procedure (spu_Specific_User_Authority_Sel) |
| Fail closed | If API call fails or returns non-"1", button stays hidden |

---

## 9. Identified Issues in Current Implementation

1. **BLOCKER**: `CanExtractClientData` not in `SSP.PureInsuranceRestAPIHandler\Enums\UserAuthorityOptions.cs` — API will fail to map the enum value
2. **BLOCKER**: No `can_extract_client_data` column exists in `User_Authorities` table — stored procedure will fail
3. **Missing**: Back Office User Maintenance checkbox not implemented
4. **Missing**: Server-side protection in `ExtractFilePassword.aspx` modal (re-validate authority before extraction)

---
.aidlc/specs/user-authority-extract-client-data/
├── requirements.md
├── design.md
├── tasks.md
├── aidlc-state.md
├── audit.md
├── user-stories.md
├── requirement-verification-questions.md

*Updated by AI-SDLC — Design verified against actual codebase.*
*Traceability: PBI #39413 → requirements.md → user-stories.md → design.md → tasks.md*
