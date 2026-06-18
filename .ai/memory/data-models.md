---
title: Data Models
description: Database schema, key entities and relationships, stored procedure conventions, and data access patterns for Pure Insurance
ms.date: 2026-04-28
---

## Overview

Pure Insurance uses Microsoft SQL Server 2016+. All schema is defined in `Databases/Pure/Structure/PURE_STRUCTURE.sql`. Reference data is in `Databases/Pure/Data/PURE_DATA.sql`. There are 5000+ stored procedures in `Databases/Pure/Procedures/`. No ORM is used — all data access is via stored procedures through `dPMDAO`.

---

## Database Access Layer

### dPMDAO

**Location:** `Sirius Architecture/Components/dPMDAO/dPMDAO.vbproj`
**Target:** VB.NET / .NET Standard 2.0
**NuGet:** `System.Data.SqlClient` 4.8.6, `Microsoft.Practices.EnterpriseLibrary.Caching` 5.0.505, `Microsoft.Practices.EnterpriseLibrary.Logging` 5.0.505

`dPMDAO` is the sole data access component for all VB.NET code. It provides:

- Connection pooling and COM+ transaction support
- Stored procedure execution with typed parameter mapping
- Deadlock detection and retry logic
- Error logging with full context (username, app, class, method, SP params)
- Configurable query timeouts (default 30s, max 7200s)
- Password masking in error output

**Key constants:**

```vbnet
Public Const ACDefaultMaxRows As Integer = 500
Public Const ACMaxMaxRows As Integer = 2147483647

Public Const ACDefaultUser As String = "SIRIUS"
Public Const ACDefaultPassword As String = "<REDACTED>"    ' Default DB credentials — see known-issues.md

Public Const ACDefaultLoginTimeout As Integer = 15
Public Const ACDefaultQueryTimeout As Integer = 30
Public Const ACMaxQueryTimeout As Integer = 7200
Public Const ACMinQueryTimeout As Integer = 5

Public Const ACDefaultDateFormat As String = "{0: yyyy-MM-dd HH:mm:ss}"
Public Const ACDeadlock As Integer = -2147467259
```

### dPMDAOBridge

**Location:** `Sirius Architecture/Components/dPMDAOBridge/dPMDAOBridge.csproj`
**Target:** C# / .NET Standard 2.0

Thin C# wrapper exposing `dPMDAO` functionality to .NET Standard consumers (e.g., `Sirius.Achitecture.Data`, `SSP.PureInsuranceRestAPIHandler`).

---

## Schema Conventions

- **Column names:** `snake_case` (e.g., `insurance_file_cnt`, `policy_version`, `claim_ref`)
- **Primary keys:** `*_id` suffix (e.g., `account_id`, `party_id`) or `*_cnt` for integer counters (e.g., `insurance_file_cnt`, `pfprem_finance_cnt`)
- **Soft delete:** `is_deleted tinyint` column — records are not physically deleted
- **Dates:** `datetime` columns; `NULL` used for open-ended dates
- **Flags:** `tinyint` (0/1) for boolean-style flags

---

## Key Tables (from stored procedure and view analysis)

The full schema is in `Databases/Pure/Structure/PURE_STRUCTURE.sql`. The following tables are referenced most frequently in stored procedures and views.

### Core Insurance Entities

| Table | Key Columns | Purpose |
|-------|------------|---------|
| `InsuranceFile` | `insurance_file_cnt`, `party_id`, `insurance_file_type` | Top-level folder grouping all policies for an insured |
| `Policy` | `policy_id`, `insurance_file_cnt`, `product_id`, `policy_version`, `risk_status_id` | Insurance policy with version history |
| `Risk` | `risk_id`, `policy_id`, `risk_type_id` | Subject of insurance (vehicle, property, liability) |
| `RiskType` | `risk_type_id`, `risk_type_code`, `risk_type_desc` | Configuration for a type of risk |
| `Party` | `party_id`, `party_type_id`, `party_name`, `is_deleted` | Any person or organisation (insured, broker, insurer, TPA) |
| `Address` | `address_id`, `party_id`, `address_type_id` | Party addresses |
| `Product` | `product_id`, `product_code`, `product_desc`, `company_no` | Insurance product definition |

### Claims

| Table | Key Columns | Purpose |
|-------|------------|---------|
| `Claim` | `claim_id`, `claim_ref`, `policy_id`, `risk_id`, `claim_status_id` | Claim record |
| `ClaimReserve` | `reserve_id`, `claim_id`, `reserve_type_id`, `reserve_amount`, `is_deleted` | Outstanding reserve per claim |
| `ClaimPayment` | `payment_id`, `claim_id`, `payment_amount`, `payment_date` | Claim payment record |
| `ClaimDiary` | `diary_id`, `claim_id`, `diary_date`, `diary_text` | Claim diary entries |
| `ClaimParty` | `claim_party_id`, `claim_id`, `party_id`, `party_type_id` | Parties associated with a claim |

### Financial

| Table | Key Columns | Purpose |
|-------|------------|---------|
| `Transdetail` | `transdetail_id`, `insurance_file_cnt`, `policy_id`, `transdetail_type_id`, `amount` | Financial transaction line |
| `Account` | `account_id`, `account_type_id`, `company_no` | General ledger account |
| `Suspended_Accounts_Transactions` | `suspended_transdetail_id`, `linked_transdetail_id`, `insurance_file_cnt`, `is_deleted` | Deferred/suspended transactions |
| `PFPremiumFinance` | `pfprem_finance_cnt`, `pfprem_finance_version`, `scheme_no`, `company_no` | Premium finance plan |
| `PFScheme` | `company_no`, `scheme_no`, `scheme_version`, `spread_ri`, `ri_suspense_account_id` | Premium finance scheme |

### Reinsurance

| Table | Key Columns | Purpose |
|-------|------------|---------|
| `Reinsurance` | `ri_id`, `policy_id`, `treaty_id`, `cession_pct` | RI cession record |
| `Treaty` | `treaty_id`, `treaty_code`, `treaty_type_id` | RI treaty definition |

### System / Configuration

| Table | Key Columns | Purpose |
|-------|------------|---------|
| `SysOptConfig` | (see `SysOptConfig.sql`) | System-wide option configuration |
| `Company` | `company_no`, `company_name` | Insurer/company definitions |
| `event_log` | `log_id`, `log_date`, `log_message`, `log_level` | Application event log entries |

---

## Database Views

Views are in `Databases/Pure/Procedures/Views/`. Used primarily for reporting and complex queries.

| View | Purpose |
|------|---------|
| `TransactionDetails` | Full transaction detail with joins |
| `QRY_FSA` | FSA reporting query |
| `qryPolicyLineDetails` | Policy line breakdown |
| `qryPolicyAgents` | Policy-to-agent mapping |
| `qryPolicyAddOns` | Policy add-on covers |
| `qryPartyInsurerRisk` | Party-insurer-risk relationships |
| `qryAllTransactions` | All transactions |
| `qryAllPolicies` | All policy records |
| `qryAllPartyDetails` | All party details |
| `qryAllClaimDetails` | All claim details |
| `qryAllAccountDetails` | All account details |
| `Policies` | Current policy view |
| `Policy2Files` / `Policy2CurrentFiles` | Policy documents |
| `Claims` | Claims summary |
| `ClaimRiskQuestions` | Risk question responses for claims |
| `Clients` / `ClientsProspect` / `ClientsPersonal` / `ClientsCorporate` | Client views |
| `ClientDependants` / `ClientConvictions` / `ClientContacts` | Client sub-entity views |
| `ClientPolicyAccountTransactions` | Client-level financial view |
| `Diary` | Diary entries across all entities |
| `MediaType_Receipt` / `MediaType_Payment` | Payment/receipt type views |
| `Party_Consultant` / `Party_Account_Handler` | Party role views |

---

## Stored Procedure Conventions

### Naming Pattern

All stored procedures use the `spu_` prefix followed by a domain area code:

| Prefix pattern | Domain |
|---------------|--------|
| `spu_ACT_*` | Accounting / Orion |
| `spu_SIR_*` | Sirius core |
| `spu_PF*` / `spu_PF_*` | Premium Finance |
| `spu_SAM_*` | SAM (access manager) |
| `spu_Report_*` | Reporting procedures |
| `spe_*` | Special/utility procedures |

### Operation Suffix

| Suffix | Operation |
|--------|-----------|
| `_Add` | INSERT |
| `_Upd` / `_Update` | UPDATE |
| `_Sel` / `_Select` | SELECT / READ |
| `_Del` / `_Delete` | DELETE (or soft-delete) |
| `_Rewrite` | DELETE + re-INSERT |

### Procedure File Format

Every stored procedure file follows this template:

```sql
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO

Execute DDLDropProcedure 'spu_XYZ_OperationName'
Go

CREATE PROCEDURE spu_XYZ_OperationName
    @ParamOne     int,
    @ParamTwo     varchar(50),
    @IsDeleted    tinyint
AS

-- procedure body

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
```

**Notes:**
- `DDLDropProcedure` is a utility stored procedure that drops the procedure if it exists
- `SET QUOTED_IDENTIFIER OFF` / `SET ANSI_NULLS OFF` are the defaults — **inconsistent with best practice** (see [known-issues.md](known-issues.md))
- Parameters use PascalCase (unlike column names which are snake_case)
- No TRY/CATCH or explicit transaction management in the examples reviewed — transaction handling may be done in `dPMDAO` or calling code

### Real Example: `spu_ACT_SuspendedAccountsTransactions_Add`

```sql
CREATE PROCEDURE spu_ACT_SuspendedAccountsTransactions_Add
    @SuspendedTransdetailId      int,
    @LinkedTransdetailId         int,
    @LinkedPercentage            float,
    @PremiumFinanceCnt           int,
    @PremiumFinanceVersion       int,
    @InsuranceFileCnt            int,
    @DestinationAccountId        int,
    @DocumentTypeId              int,
    @TransdetailTypeId           int,
    @Spare                       varchar(50),
    @IsDeleted                   tinyint,
    @manually_released           tinyint,
    @released_on_full_settlement tinyint,
    @released_for_whole_posting  tinyint,
    @released_on_policy_effective tinyint
AS

-- Zero values treated as NULL for foreign keys
IF @PremiumFinanceCnt = 0
    SELECT @PremiumFinanceCnt = NULL
IF @PremiumFinanceVersion = 0
    SELECT @PremiumFinanceVersion = NULL

INSERT INTO Suspended_Accounts_Transactions (
    suspended_transdetail_id,
    linked_transdetail_id,
    linked_percentage,
    pfprem_finance_cnt,
    pfprem_finance_version,
    insurance_file_cnt,
    destination_account_id,
    documenttype_id,
    transdetail_type_id,
    spare,
    is_deleted,
    manually_released,
    released_on_full_settlement,
    released_for_whole_posting,
    released_on_policy_effective
) VALUES (
    @SuspendedTransdetailId, @LinkedTransdetailId, @LinkedPercentage,
    @PremiumFinanceCnt, @PremiumFinanceVersion, @InsuranceFileCnt,
    @DestinationAccountId, @DocumentTypeId, @TransdetailTypeId,
    @Spare, @IsDeleted, @manually_released, @released_on_full_settlement,
    @released_for_whole_posting, @released_on_policy_effective
)
```

### Real Example: `spu_PFGetRISuspenseInfo` (SELECT)

```sql
CREATE PROCEDURE spu_PFGetRISuspenseInfo
    @PremiumFinanceCnt    int,
    @PremiumFinanceVersion int
AS

SELECT   Account.account_id, PFScheme.spread_ri
FROM     PFScheme
    INNER JOIN PFPremiumFinance
        ON PFScheme.CompanyNo         = PFPremiumFinance.CompanyNo
        AND PFScheme.SchemeNo         = PFPremiumFinance.SchemeNo
        AND PFScheme.SchemeVersion    = PFPremiumFinance.SchemeVersion
    INNER JOIN Account
        ON PFScheme.ri_suspense_account_id = Account.account_id
WHERE (PFPremiumFinance.pfprem_finance_cnt     = @PremiumFinanceCnt)
  AND (PFPremiumFinance.pfprem_finance_version = @PremiumFinanceVersion)
```

---

## Data Validation Rules

Validation is performed at the application layer (business components), not enforced by database constraints in most cases.

- **Zero → NULL conversion:** Integer FK parameters with value 0 are converted to NULL before INSERT (see example above). This is a pervasive pattern.
- **Soft deletes:** Records are marked `is_deleted = 1` rather than physically deleted. All SELECT procedures should filter `WHERE is_deleted = 0`.
- **Date handling:** Dates are passed as `varchar` formatted `yyyy-MM-dd HH:mm:ss` by `dPMDAO` and converted by SQL Server.
- **`@Spare` parameters:** Many procedures have a `@Spare varchar(50)` parameter for future use — pass empty string.

---

## Installer and Migration Scripts

| Location | Purpose |
|----------|---------|
| `Databases/Installer/` | Full database installation scripts for fresh deployments |
| `Databases/After Change/` | Incremental migration scripts applied after code releases |
| `Databases/Utility Scripts/` | One-off utility scripts (not part of standard deployment) |
| `Databases/Pure/Data/SysOptConfig.sql` | System option configuration data |
| `Databases/Pure/Data/TScriptUpdateSysOption.sql` | Script-based system option updates |

---

## Inconsistencies and Notes

- Stored procedures use `SET QUOTED_IDENTIFIER OFF` and `SET ANSI_NULLS OFF` — this is inconsistent with SQL Server best practice (both should be `ON`). New procedures should still match this convention to avoid breaking existing code — raise a tech debt item if changing.
- No TRY/CATCH blocks in the stored procedures reviewed. Transaction management appears to be delegated to `dPMDAO` or the application layer.
- Parameter casing is inconsistent: some use PascalCase (`@SuspendedTransdetailId`), others use lower_snake_case (`@manually_released`) in the same procedure.
- 5000+ stored procedures make comprehensive documentation impractical — always explore `Databases/Pure/Procedures/` when working on data access changes.
