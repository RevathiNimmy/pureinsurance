# Application Design — Instalment for Claim Recovery - Scheme Configuration

**Feature**: Instalment for Claim Recovery - Scheme Configuration
**Spec ID**: SPEC-39336
**Date**: 2026-05-06

---

## 1. Architecture Overview

This feature extends the existing Pure Insurance instalment infrastructure to support claim recovery instalments. No new components are created — existing components are extended with new responsibilities while maintaining backward compatibility.

```
+------------------------------------------------------------------+
|                    CONFIGURATION LAYER                            |
|                                                                  |
|  +------------------------+    +-----------------------------+   |
|  | Product Risk Maint.    |    | Instalment Scheme Maint.    |   |
|  | (GIS/Product Builder)  |    | (Sirius Back Office Core)   |   |
|  |                        |    |                             |   |
|  | [NEW] Recovery         |    | [NEW] Scheme Type dropdown  |   |
|  | Receipts on            |    | [NEW] Transaction Type      |   |
|  | Instalments checkbox   |    |       (Salvage/Third-Party) |   |
|  | (2-Claims tab)         |    | [NEW] Rate config per type  |   |
|  +----------+-------------+    +-------------+---------------+   |
|             |                                |                   |
+------------------------------------------------------------------+
              |                                |
              v                                v
+------------------------------------------------------------------+
|                    BUSINESS LOGIC LAYER                           |
|                                                                  |
|  +------------------------+    +-----------------------------+   |
|  | Claims Management      |    | Premium Finance Business    |   |
|  | (gCLMLibrary)          |    | (bSIRInstalments)           |   |
|  |                        |    |                             |   |
|  | [NEW] Recovery type    |    | [MOD] Scheme selection      |   |
|  |       identification   |    |       with type filtering   |   |
|  | [NEW] Product config   |    | [MOD] Rate lookup with      |   |
|  |       validation       |    |       transaction type      |   |
|  | [NEW] Duplicate plan   |    |                             |   |
|  |       prevention       |    |                             |   |
|  +----------+-------------+    +-------------+---------------+   |
|             |                                |                   |
+------------------------------------------------------------------+
              |                                |
              v                                v
+------------------------------------------------------------------+
|                    DATA ACCESS LAYER                              |
|                                                                  |
|  +-----------------------------------------------------------+  |
|  | dPMDAO (stored procedure execution)                        |  |
|  |                                                           |  |
|  | [MOD] spu_PF* procedures — add @SchemeType parameter      |  |
|  | [NEW] spu_PF_Scheme_SelByType — scheme selection filter    |  |
|  | [NEW] spu_PF_Rate_SelByTransType — rate lookup by type    |  |
|  | [MOD] spu_PF_Scheme_Add/Upd — include scheme_type         |  |
|  +-----------------------------------------------------------+  |
|                                                                  |
+------------------------------------------------------------------+
              |
              v
+------------------------------------------------------------------+
|                    DATABASE LAYER                                 |
|                                                                  |
|  +-----------------------------------------------------------+  |
|  | PFScheme table                                             |  |
|  | [NEW] scheme_type tinyint NOT NULL DEFAULT 1               |  |
|  |       CHECK (scheme_type IN (1, 2))                        |  |
|  |       1 = Premium Finance, 2 = Claim Recovery              |  |
|  +-----------------------------------------------------------+  |
|  | PFRate table (or equivalent)                               |  |
|  | [NEW] transaction_type tinyint NULL                         |  |
|  |       1 = Salvage Recovery, 2 = Third-Party Recovery       |  |
|  |       (NULL for Premium Finance rates — backward compat)   |  |
|  +-----------------------------------------------------------+  |
|  | Product table                                              |  |
|  | [NEW] recovery_instalments_enabled tinyint DEFAULT 0       |  |
|  +-----------------------------------------------------------+  |
|                                                                  |
+------------------------------------------------------------------+
              |
              v
+------------------------------------------------------------------+
|                    WORKFLOW LAYER                                 |
|                                                                  |
|  +-----------------------------------------------------------+  |
|  | Navigator XM — Claims Recovery Roadmap                     |  |
|  | [MOD] Add instalment plan creation step                    |  |
|  |       (conditional on product config)                      |  |
|  +-----------------------------------------------------------+  |
|                                                                  |
+------------------------------------------------------------------+
```

---

## 2. Component Design

### 2.1 Product Risk Maintenance (GIS/Product Builder)

**Existing Component**: `iPMUProduct` / `bSIRProduct`
**Path**: `Sirius For Underwriting\Components\Product\Interface\iPMUProduct\iPMUProductFrm.vb`
**Designer**: `Sirius For Underwriting\Components\Product\Interface\iPMUProduct\iPMUProductFrm.Designer.vb`
**Constants**: `Sirius For Underwriting\Components\Product\Interface\iPMUProduct\iPMUProductMod.vb`
**Change Type**: Minor extension

| Method | Type | Purpose |
|--------|------|---------|
| `LoadRecoveryInstalmentConfig()` | NEW | Load `recovery_instalments_enabled` flag from Product table via `spe_Product_sel` |
| `SaveRecoveryInstalmentConfig()` | NEW | Persist checkbox value to Product table via `spe_Product_upd` |

**UI Change**: Add checkbox control to "2-Claim" tab (`_tabMainTab_TabPage1`), inside `Frame4` ("Claim Payment" GroupBox). Bind to `recovery_instalments_enabled` field on `Product` table.

**Data Flow Pattern** (follows existing `chkPaymentCannotExceedReserve` pattern):
1. Declare `m_vRecoveryInstalmentsEnabled As Object` field
2. Add `ACICPSelRecoveryInstalmentsEnabled = 194` / `ACICPUpdRecoveryInstalmentsEnabled = 171` constants in `iPMUProductMod.vb`
3. Load from `oResultArray(ACICPSelRecoveryInstalmentsEnabled, 0)` in data load — returned by `spe_Product_sel`
4. Set `chkRecoveryInstalmentsEnabled.CheckState` in `BusinessToInterface`
5. Read `chkRecoveryInstalmentsEnabled.CheckState` in `InterfaceToBusiness`
6. Write to `oParamArray(ACICPUpdRecoveryInstalmentsEnabled)` in save — passed to `spe_Product_upd`

**Stored Procedure Changes Required**:
- `spe_Product_sel`: Add `p.recovery_instalments_enabled` to SELECT list (position 194, after `delete_quote_after`)
- `spe_Product_upd`: Add `@recovery_instalments_enabled TINYINT = 0` parameter + `recovery_instalments_enabled = @recovery_instalments_enabled` in UPDATE SET clause

**Business Layer Changes Required** (`bSIRProduct`):
- **Constants file** (`bSIRProduct.vb`): Add `Public Const ACICPUpdRecoveryInstalmentsEnabled As Integer = 171` — the business layer has its own copy of update constants (separate from `iPMUProductMod.vb`)
- **UpdateProduct method** (`bSIRProductBusiness.vb`): Add `m_oDatabase.Parameters.Add(sName:="recovery_instalments_enabled", vValue:=gPMFunctions.ToSafeInteger(r_vParamArray(ACICPUpdRecoveryInstalmentsEnabled)), ...)` — this is what actually passes the value to `spe_Product_upd`. Without this line, the param array value is never sent to the stored procedure.

**Key Pattern**: Interface (`iPMUProductFrm`) populates `oParamArray` → Business (`bSIRProductBusiness.UpdateProduct`) maps each array element to a named SP parameter via `m_oDatabase.Parameters.Add` → dPMDAO executes `spe_Product_upd`. All three layers must be wired up for a value to persist.

---

### 2.2 Instalment Scheme Maintenance (Sirius Back Office Core)

**Existing Component**: `iPMBPFScheme` (Interface) / `bSIRPFScheme` (Business)
**Change Type**: Moderate extension

#### 2.2.1 Backend Module (DONE — modPFSchemeRecovery.vb)

| Method | Type | Status | Purpose |
|--------|------|--------|---------|
| `LoadSchemeType()` | NEW | ✅ Done | Load scheme_type from PFScheme for display in dropdown |
| `SaveSchemeType(schemeType)` | NEW | ✅ Done | Persist scheme_type value when saving scheme |
| `LoadTransactionTypes(schemeNo)` | NEW | ✅ Done | Load available transaction types for rate configuration |
| `SaveRateByTransactionType(schemeNo, transType, rates)` | NEW | ✅ Done | Save rate configuration per transaction type |
| `GetRateByTransactionType(schemeNo, transType)` | NEW | ✅ Done | Retrieve rates for a specific transaction type |

#### 2.2.2 UI Changes — T7: Scheme Type Dropdown (DONE)

**Form**: `Sirius Back Office Core\Components\Instalments\Interface\iPMBPFScheme\iPMBPFSchemeFrm.vb`
**Designer**: `Sirius Back Office Core\Components\Instalments\Interface\iPMBPFScheme\iPMBPFSchemeFrm.Designer.vb`

**Controls** (in `Frame4` GroupBox on Tab 1 — "Scheme Type"):
- `cboSchemeType` — PMLookupControl.cboPMLookup, TableName = "PFScheme_Type"
- `lblSchemeType` — Label, text "Scheme Type:"

**Behaviour**:
- Populated from `PFScheme_Type` lookup table via PMLookupControl
- Loaded from `m_vSchemeArray(bSIRPremFinConst.k_PFSchemePFSchemeTypeID, 0)` in `BusinessToInterface`
- Saved as `cboSchemeType.ItemId` to `m_vSchemeArray(bSIRPremFinConst.k_PFSchemePFSchemeTypeID, 0)` in `InterfaceToData`
- Disabled in Edit mode (scheme type cannot be changed after creation)
- Drives `FormLogic()` — controls visibility of Finance Provider, EDI, Connectivity tabs based on scheme type code

#### 2.2.3 UI Changes — T8: Transaction Type Dropdown (DONE)

**Form**: `Sirius Back Office Core\Components\Instalments\Interface\iPMBPFScheme\iPMBPFSchemeFrm.vb`
**Designer**: `Sirius Back Office Core\Components\Instalments\Interface\iPMBPFScheme\iPMBPFSchemeFrm.Designer.vb`

**Controls** (on `_tabMainTab_TabPage2` — Rates tab):
- `cboTransactionType` — ComboBox (DropDownList), items: "All", "Salvage Recovery", "Third-Party Recovery"
- `lblTransactionType` — Label, text "Transaction Type:"

**Behaviour**:
- Both controls default `Visible = False`
- `UpdateTransactionTypeVisibility()` shows them only when `cboSchemeType.ItemCode = "CR"` (Claim Recovery)
- Called from `cboSchemeType_Click` and `SetInterfaceDefaults`
- Default selection is "All" (index 0) when first shown
- Allows filtering rates display by transaction type for Claim Recovery schemes

**UI Summary**:
- "Scheme Type" dropdown on Tab 1 (Scheme) in `iPMBPFScheme` — populated from PFScheme_Type lookup
- When Scheme Type = "Claim Recovery" (code "CR"), Transaction Type selector appears on Rates tab
- Transaction Type options: "All", "Salvage Recovery", "Third-Party Recovery"

#### 2.2.4 Instalment Rates Form — Transaction Type (iPMBPFRF)

**Form**: `Sirius Back Office Core\Components\Instalments\Interface\iPMBPFRF\iPMBPFRFFrm.vb`

**Change**: `BuildProductFamilyCombo()` method — add `Case "CR"` to populate `cboProductFamily` with recovery transaction types.

**Existing behaviour**: The `cboProductFamily` combo is populated based on `m_sSchemeType`:
- `"TP"`, `"IH"` → "New Business", "MTA", "Renewal"
- `"TPSG"` → "Stargate"
- `"TPR"` → "TPR"

**New behaviour**: When `m_sSchemeType = "CR"`:
- Populate with "Salvage Recovery" (code `"SR"`) and "Third-Party Recovery" (code `"TPR"`)
- Without this, the combo is empty and `SelectedIndex = 0` throws `InvalidArgument` on form load

**Lookup Data Requirement** (`PFScheme_Type` table):
- A row with `code = 'CR'` and `description = 'Claim Recovery'` must exist in `PFScheme_Type`
- Added via `PURE_DATA.sql`: `INSERT INTO PFScheme_Type (pfscheme_type_id, code, description, caption_id, effective_date, is_deleted) VALUES (3, 'CR', 'Claim Recovery', 0, GETDATE(), 0)`
- Without this row, "Claim Recovery" does not appear in the Scheme Type dropdown

#### 2.2.5 Instalment Rates — Transaction Type Persistence (Bug #39993 Fix)

**Problem**: Transaction Type was displayed in the UI but never persisted to the database on save.

**Root Cause**: `spu_PFRF_add`/`spu_PFRF_upd` had no `@transaction_type` parameter, and `bSIRPFRFBusiness.AddInputParam` never passed the value.

**Fix — Full Data Flow**:

| Layer | Component | Change |
|-------|-----------|--------|
| Stored Proc | `spu_PFRF_add` | Added `@transaction_type TINYINT = NULL` parameter; included in INSERT column/values list |
| Stored Proc | `spu_PFRF_upd` | Added `@transaction_type TINYINT = NULL` parameter; included in UPDATE SET clause |
| Business | `bSIRPFRFBusiness.vb` → `AddInputParam()` | Added `v_vTransactionType As Object = Nothing` parameter; passes value to SP via `m_oDatabase.Parameters.Add(sName:="transaction_type", ...)` |
| Business | `bSIRPFRFBusiness.vb` → `DirectAdd()`/`DirectEdit()` | Added `v_vTransactionType` optional parameter; passes through to `AddInputParam` |
| Interface | `iPMBPFRFFrm.vb` → `GetTransactionTypeValue()` | New helper: maps `cboProductFamily.Text` to integer ("SR"→1, "TPR"→2) when `m_sSchemeType = "CR"`, returns `Nothing` for PF schemes |
| Interface | `iPMBPFRFFrm.vb` → `DirectAdd`/`DirectEdit` calls | Passes `v_vTransactionType:=GetTransactionTypeValue()` |

**Transaction Type Values**:
- `NULL` = Premium Finance rate (backward compatible, default)
- `1` = Salvage Recovery
- `2` = Third-Party Recovery

---

### 2.3 Claims Management — Recovery Instalment Logic

**Existing Component**: `bCLMRecovery` / `gCLMLibrary`
**Change Type**: New methods added

| Method | Type | Purpose | Error Scenarios |
|--------|------|---------|----------------|
| `IsRecoveryInstalmentEnabled(claimId)` | NEW | Check product config via claim → policy → product (`Product.recovery_instalments_enabled`) | Claim not found, Policy not found, Product not configured |
| `GetRecoveryType(clrTransactionId)` | NEW | Read `recovery_type` from CLR transaction record | CLR transaction not found, Invalid recovery type |
| `GetAvailableRecoverySchemes(companyNo, recoveryType)` | NEW | Retrieve matching Claim Recovery schemes for the recovery type | No schemes configured, Company not found |
| `ValidateNoExistingInstalmentPlan(clrTransactionId)` | NEW | Check no active instalment plan exists for this recovery | Active plan exists (blocking error) |
| `CreateRecoveryInstalmentPlan(clrTransactionId, schemeNo, schemeVersion)` | NEW | Create the instalment plan using selected scheme and applicable rates | Scheme not found, Rate configuration missing, Database error |

**Recovery Type Mapping**:
- Map existing CLR `recovery_type` values to transaction types:
  - Salvage recovery → Transaction Type 1
  - Third-Party recovery → Transaction Type 2
- Unknown recovery types should return error

**Rate Structure Details**:
The same rate fields as Premium Finance will be used for Claim Recovery rates:
- Interest rate (percentage)
- Administration fee (amount or percentage)
- Minimum instalment amount
- Maximum term (months)
- Payment frequency options (monthly, quarterly, etc.)
- Late payment fees/penalties
- Early settlement terms
- Processing fees

**Orchestration Flow** (CreateRecoveryInstalmentPlan):
1. Call `ValidateNoExistingInstalmentPlan` — block if active plan exists
2. Call `IsRecoveryInstalmentEnabled` — block if product not configured
3. Call `GetRecoveryType` — identify Salvage or Third-Party
4. Call `GetAvailableRecoverySchemes` — retrieve matching schemes
5. Present schemes to user for selection
6. Apply rates from selected scheme for the matching transaction type
7. Create instalment plan record
8. Log to event_log

---

### 2.4 Premium Finance Business Layer — Backward Compatibility

**Existing Component**: `bSIRInstalments`
**Change Type**: Filtering extension

| Method | Type | Purpose |
|--------|------|---------|
| `GetSchemes(companyNo)` | MOD | Add optional `schemeType` parameter (default = Premium Finance) |
| `GetSchemeRates(schemeNo, schemeVersion)` | MOD | Add optional `transactionType` parameter (default = NULL for PF) |

**Critical**: All existing callers continue to work unchanged due to default parameter values.

---

## 3. Data Model Changes

### 3.1 PFScheme Table — New Column

```sql
EXEC DDLAddColumn 'PFScheme', 'scheme_type', 'TINYINT NOT NULL DEFAULT 1'
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CH__PFScheme__scheme_type')
    EXEC DDLAddCheck 'PFScheme', 'scheme_type', '([scheme_type] IN (1, 2))'
-- 1 = Premium Finance (default — all existing rows)
-- 2 = Claim Recovery
```

### 3.2 PFRF Table — New Column

**NOTE**: The rates table is `PFRF` (not `PFRate`). The migration scripts committed as T3 incorrectly used `PFRate` — this must be corrected.

```sql
EXEC DDLAddColumn 'PFRF', 'transaction_type', 'TINYINT NULL'
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CH__PFRF__transaction_type')
    EXEC DDLAddCheck 'PFRF', 'transaction_type', '([transaction_type] IS NULL OR [transaction_type] IN (1, 2))'
-- NULL = Premium Finance rate (backward compatible)
-- 1 = Salvage Recovery
-- 2 = Third-Party Recovery
```

### 3.3 Product Table — New Column (recovery_instalments_enabled)

**NOTE**: This column lives on the `Product` table (not `Risk_Type`) because all claims tab settings (e.g. `Payment_Cannot_Exceed_Reserve`, `multiple_claims_payments`) are stored on `Product` and loaded/saved via `spe_Product_sel`/`spe_Product_upd`.

```sql
EXEC DDLAddColumn 'Product', 'recovery_instalments_enabled', 'TINYINT NOT NULL DEFAULT 0'
-- 0 = Disabled (default)
-- 1 = Enabled
```

### 3.4 PFPremiumFinance Table — New Column

```sql
EXEC DDLAddColumn 'PFPremiumFinance', 'claim_recovery_transaction_id', 'INT NULL'
-- Links instalment plans to CLR recovery transactions
-- NULL = Premium Finance instalment (backward compatible)
-- Non-NULL = CLR transaction ID for recovery instalment plans
-- Consider adding foreign key constraint to CLR transaction table if referential integrity is required
```

### 3.5 Database Schema Summary

| Table | Column | Type | Default | Description |
|-------|--------|------|---------|-------------|
| PFScheme | scheme_type | TINYINT | 1 | 1 = Premium Finance, 2 = Claim Recovery |
| PFRF | transaction_type | TINYINT | NULL | NULL = Premium Finance, 1 = Salvage Recovery, 2 = Third-Party Recovery |
| Product | recovery_instalments_enabled | TINYINT | 0 | 0 = Disabled, 1 = Enabled |
| PFPremiumFinance | claim_recovery_transaction_id | INT | NULL | Links to CLR transaction for recovery instalments |

**CHECK Constraints**:
- `PFScheme.scheme_type IN (1, 2)`
- `PFRF.transaction_type IS NULL OR PFRF.transaction_type IN (1, 2)`
- `Product.recovery_instalments_enabled IN (0, 1)`

### 3.5 Migration Strategy

- All changes go in `Databases/Pure/Structure/PURE_STRUCTURE.sql`
- Existing PFScheme rows get `scheme_type = 1` (Premium Finance) via DEFAULT
- Existing PFRF rows get `transaction_type = NULL` (Premium Finance) via DEFAULT
- Existing Product rows get `recovery_instalments_enabled = 0` (disabled) via DEFAULT
- Existing PFPremiumFinance rows get `claim_recovery_transaction_id = NULL` via DEFAULT
- CHECK constraints use IF NOT EXISTS guard to prevent duplicate creation
- No data loss, no breaking changes to existing records

---

## 4. Stored Procedure Changes

### 4.1 Modified Procedures

| Procedure | Change |
|-----------|--------|
| `spu_PF_Scheme_Sel` (or equivalent) | Add `@SchemeType tinyint = 1` parameter, add `WHERE scheme_type = @SchemeType` |
| `spu_PF_Scheme_Add` | Add `@SchemeType tinyint = 1` parameter, include in INSERT |
| `spu_PF_Scheme_Upd` | Add `@SchemeType tinyint` parameter, include in UPDATE |
| `spu_PF_Rate_Sel` (or equivalent) | Add `@TransactionType tinyint = NULL` parameter, filter when not NULL |

### 4.2 New Procedures

| Procedure | Purpose | Parameters | Returns |
|-----------|---------|------------|---------|
| `spu_PF_Scheme_SelByType` | Select schemes filtered by scheme_type AND company_no | `@CompanyNo INT, @SchemeType TINYINT = 1` | Scheme records matching criteria |
| `spu_PF_Rate_SelByTransType` | Select rates filtered by scheme + transaction_type | `@SchemeNo INT, @TransactionType TINYINT = NULL` | Rate configuration for specified transaction type |
| `spu_CLR_RecoveryInstalmentPlan_Validate` | Check if active instalment plan exists for a CLR transaction | `@CLRTransactionId INT` | 0 = No active plan, 1 = Active plan exists |


**Transaction Type Values**:
- `NULL` = Premium Finance rates (backward compatibility)
- `1` = Salvage Recovery
- `2` = Third-Party Recovery

### 4.3 Procedure Conventions

All new/modified procedures follow existing conventions:
- `SET QUOTED_IDENTIFIER OFF` / `SET ANSI_NULLS OFF`
- `Execute DDLDropProcedure` before CREATE
- PascalCase parameters
- Default parameter values for backward compatibility

---

## 5. Component Dependencies

```
Product Risk Maintenance ──────────────────────────────────────┐
  (reads/writes Product.recovery_instalments_enabled)          │
                                                               │
Instalment Scheme Maintenance ─────────────────────────────────┤
  (reads/writes PFScheme.scheme_type)                          │
  (reads/writes PFRate.transaction_type)                       │
                                                               ▼
Claims Recovery Instalment Logic ◄─── Navigator XM Roadmap
  │                                    (triggers workflow)
  │
  ├── calls → Product Risk config (IsRecoveryInstalmentEnabled)
  ├── calls → CLR transaction data (GetRecoveryType)
  ├── calls → Scheme selection (GetAvailableRecoverySchemes)
  ├── calls → Duplicate validation (ValidateNoExistingInstalmentPlan)
  └── calls → Instalment creation (existing PF instalment creation logic)
```

**Dependency Direction**: Claims Recovery logic depends on Product Config and Scheme Config. Configuration components are independent of each other.

---

## 6. Security Considerations (SECURITY Baseline)

| Rule | Applicability | Notes |
|------|--------------|-------|
| SECURITY-01 | N/A | No new data stores — existing SQL Server with encryption managed by infrastructure |
| SECURITY-03 | Applicable | All new methods must log via event_log; no PII in logs |
| SECURITY-05 | Applicable | All stored procedure parameters are parameterised via dPMDAO (no SQL injection risk) |
| SECURITY-08 | Applicable | Existing role-based access controls apply to new screens/functions |
| SECURITY-09 | Applicable | No default credentials; error messages must be generic to users |
| SECURITY-12 | N/A | No new authentication — uses existing STS/session management |
| SECURITY-15 | Applicable | All dPMDAO calls wrapped in Try/Catch; fail closed on errors |

---

## 7. Testing Strategy

| Level | Scope |
|-------|-------|
| Unit Tests | Business logic methods: recovery type identification, scheme filtering, duplicate validation |
| Integration Tests | Stored procedure execution via dPMDAO: scheme CRUD with type filtering, rate retrieval by transaction type |
| Regression Tests | Existing Premium Finance workflows unaffected: scheme selection, rate lookup, instalment creation |
| UI Tests | Product Risk Maintenance checkbox, Scheme Maintenance dropdown, Rates Transaction Type selector |
| End-to-End | Full flow: enable product → create scheme → configure rates → create recovery instalment plan |

---

*Generated by AI-SDLC INCEPTION phase. Traceability: requirements.md → user-stories.md → design.md → tasks.md*
