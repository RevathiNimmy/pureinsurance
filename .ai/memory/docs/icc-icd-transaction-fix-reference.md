# ICC/ICD Claim Recovery Instalment Transactions — Technical Reference

> Complete reference for how ICC (Instalment Claim Credit) and ICD (Instalment Claim Debit)
> transactions are generated, the call chain involved, and the root-cause fix applied when
> these transaction types were not being created.
>
> **Use this file** when debugging instalment transaction generation issues, extending claim
> recovery instalment logic, adding new instalment transaction types, or tracing the
> IND/INC → ICD/ICC mapping flow.
>
> **Last Updated**: 2026-07-01
> **Owned By**: Pure Insurance Team
> **Related ADOs**: Epic #39472, Epic #39336, PBI #37524

---

## Table of Contents

1. [Transaction Type Overview](#1-transaction-type-overview)
2. [Call Chain Architecture](#2-call-chain-architecture)
3. [Root Cause — Why ICC/ICD Were Not Created](#3-root-cause--why-iccicd-were-not-created)
4. [Fix Applied — Layer by Layer](#4-fix-applied--layer-by-layer)
5. [Key Constants & Enum Values](#5-key-constants--enum-values)
6. [Database Prerequisites](#6-database-prerequisites)
7. [File Reference Index](#7-file-reference-index)
8. [Decision Log](#8-decision-log)
9. [Debugging Checklist](#9-debugging-checklist)

---

## 1. Transaction Type Overview

### Instalment Transaction Code Map

| Code | Meaning | Plan TransType | Layer | Purpose |
|------|---------|---------------|-------|---------|
| **IND** | Instalment NB Debit | NB | Orion Accounting | Debit to client account per instalment due |
| **INC** | Instalment NB Credit | NB | Orion Accounting | Credit when instalment payment received |
| **IED** | Instalment Endorsement Debit | MTA | Orion Accounting | Debit for MTA instalment |
| **IEC** | Instalment Endorsement Credit | MTA | Orion Accounting | Credit for MTA instalment payment |
| **IRD** | Instalment Renewal Debit | REN | Orion Accounting | Debit for renewal instalment |
| **IRC** | Instalment Renewal Credit | REN | Orion Accounting | Credit for renewal instalment payment |
| **ICD** | Instalment Claim Debit | SR, TPR | Orion Accounting | Debit for claim recovery instalment |
| **ICC** | Instalment Claim Credit | SR, TPR | Orion Accounting | Credit for claim recovery instalment payment |

### Plan TransType Values

| TransType | Description | Instalment Codes Used |
|-----------|-------------|----------------------|
| NB | New Business | IND, INC |
| MTA (or M) | Mid-Term Adjustment | IED, IEC |
| REN | Renewal | IRD, IRC |
| SR | Salvage Recovery (Claim Recovery) | ICD, ICC |
| TPR | Third Party Recovery (Claim Recovery) | ICD, ICC |

### ICC/ICD vs INC/IND Relationship

ICC and ICD are **behavioural clones** of INC and IND respectively. The only difference is
the document type and range code used for auto-numbering. They exist to distinguish claim
recovery instalment postings from standard premium finance postings in Orion accounting
and reporting.

---

## 2. Call Chain Architecture

### Two Posting Paths

ICC/ICD documents can be created through **two independent posting paths**:

#### Path A — In-House Instalments (bACTInstalmentsCls)

Used for in-house instalment plans. This path queries the plan transtype directly from the
database via `GetPlanTransType()`.

```
bACTInstalmentsCls.PostInstalmentDebit()
  → GetPlanTransType(pfprem_finance_cnt, pfprem_finance_version)
	→ SELECT transtype FROM PFPremiumFinance WHERE ...
  → If transtype = "SR" or "TPR":
	  lDocumentTypeID = ACTDocTypeInstalmentClaimDebit  (59)
	  sRangeCode = ACTAutoNumberRangeCodeICD            ("ICD")
	  sDocRef = ACTAutoNumberGroupCodeDocumentRef59      ("DOCREF59")
  → PostInstalmentDebit(... v_lDocumentTypeID, v_sRangeCode, v_sDocRef)

bACTInstalmentsCls.PostInstalmentCredit()
  → GetPlanTransType(pfprem_finance_cnt, pfprem_finance_version)
  → If transtype = "SR" or "TPR":
	  sRangeCode = ACTAutoNumberRangeCodeICC            ("ICC")
	  lDocumentTypeID = ACTDocTypeInstalmentClaimCredit  (60)
	  sGroupCode = ACTAutoNumberGroupCodeDocumentRef60   ("DOCREF60")
```

**Status**: ✅ Working — `GetPlanTransType` queries DB directly.

#### Path B — Third-Party Premium Finance (bACTPremiumFinanceBusiness)

Used for third-party (provider) instalment plans. This path receives the plan transtype
as a parameter propagated from the caller.

```
bSIRPremiumFinance.TransactPlanThirdParty(vPremiumFinance, ...)
  → Extracts: vPremiumFinance(k_PFPlanTransactionType, 0)  [e.g. "SR", "TPR"]
  → m_oPremiumFinance.TransactPremiumFinance(... v_sPlanTransType:="SR")
	→ PostTransactions(... v_sTransType:="C", v_sPlanTransType:="SR")
	  → If v_sPlanTransType = "SR" or "TPR":
		  sACTAutoNumberRangeCode = ACTAutoNumberRangeCodeICC  ("ICC")
		  iACTDocType = ACTDocTypeInstalmentClaimCredit         (60)
	→ PostTransactions(... v_sTransType:="D", v_sPlanTransType:="SR")
	  → If v_sPlanTransType = "SR" or "TPR":
		  sACTAutoNumberRangeCode = ACTAutoNumberRangeCodeICD  ("ICD")
		  iACTDocType = ACTDocTypeInstalmentClaimDebit          (59)
```

**Status**: ✅ Working after fix (was broken — see Section 3).

### Import Path — Transaction Code Mapping

When instalment records are imported into Orion from `Transaction_Export_Folder`, the
stored procedure `spu_ACT_Import_Update_Instalment_Transaction_Code` maps INC/IND
codes to ICC/ICD based on the plan's transtype:

```sql
IF @plan_transtype IN ('SR', 'TPR')
BEGIN
	SET @resolved_code = CASE @pfinstalments_transaction_code
		WHEN 'INC' THEN 'ICC'
		WHEN 'IND' THEN 'ICD'
		ELSE @pfinstalments_transaction_code
	END
END
```

This updates `pfinstalments.transactioncode` with the resolved `pfinstalments_transaction_id`.

---

## 3. Root Cause — Why ICC/ICD Were Not Created

### Problem

Claim recovery instalment plans (TransType = "SR" or "TPR") were generating IND/INC
documents instead of ICD/ICC. The override logic existed but was **dead code** in
Path B (third-party premium finance).

### Root Cause Diagram

```
bSIRPremiumFinance.TransactPlanThirdParty
  │
  │  vPremiumFinance(k_PFPlanTransactionType, 0) = "SR"   ← transtype IS available here
  │
  └──→ m_oPremiumFinance.TransactPremiumFinance(...)       ← BUT NO v_sPlanTransType param!
		 │
		 │  TransactPremiumFinance signature had NO v_sPlanTransType parameter
		 │
		 └──→ PostTransactions(... v_sPlanTransType:="")    ← always "" (empty string)
				│
				│  If v_sPlanTransType = "SR" OrElse "TPR"  ← NEVER matches
				│  ∴ ICC/ICD override logic = DEAD CODE
				│
				└──→ Always defaults to IND/INC
```

### Gap Summary (4 layers)

| # | Layer | File | Gap |
|---|-------|------|-----|
| 1 | **Orion PF Business** | `bACTPremiumFinanceBusiness.vb` | `TransactPremiumFinance` had no `v_sPlanTransType` parameter, so its 4 internal `PostTransactions` calls never passed the plan transtype |
| 2 | **Back Office Installer** | `bSIRPremiumFinance.vb` | `TransactPlanThirdParty` had access to `vPremiumFinance(k_PFPlanTransactionType, 0)` but did not pass it to `TransactPremiumFinance` |
| 3 | **Import Layer** | `bACTImportSiriusTrans.vb` | Line 1305 checked `StartsWith("IND" \| "IRD" \| "IED")` but omitted `"ICD"` — TAX/COMM suspended transactions for claim recovery debits would fail |
| 4 | **REST API** | `PureInsurance.REST DocumentTypeType.cs` | Missing `ICD = 9` and `ICC = 10` enum values — REST API could not recognize claim recovery document types |

---

## 4. Fix Applied — Layer by Layer

### Fix 1: bACTPremiumFinanceBusiness.vb

**File**: `Orion\Components\PremiumFinance\Business\bACTPremiumFinance\bACTPremiumFinanceBusiness.vb`

**Change**: Added `Optional ByVal v_sPlanTransType As String = ""` parameter to
`TransactPremiumFinance` (line 649) and passed it through to all 4 internal
`PostTransactions` calls.

```vbnet
' BEFORE (line 649):
Public Function TransactPremiumFinance(... Optional ByRef r_lProviderTransDetailID As Integer = 0) As Integer

' AFTER:
Public Function TransactPremiumFinance(... Optional ByRef r_lProviderTransDetailID As Integer = 0, Optional ByVal v_sPlanTransType As String = "") As Integer
```

Internal calls updated (4 total):

| Call Site | Line | Trans Type | Change |
|-----------|------|-----------|--------|
| Sub-agent posting | ~713 | (none) | Added `v_sPlanTransType:=v_sPlanTransType` |
| Credit to client | ~734 | "C" | Added `v_sPlanTransType:=v_sPlanTransType` |
| Deposit debit | ~754 | "D" | Added `v_sPlanTransType:=v_sPlanTransType` |
| Provider debit | ~772 | "D" | Added `v_sPlanTransType:=v_sPlanTransType` |

**Backward compatibility**: The parameter is `Optional` with default `""`, so existing
callers (e.g. `bPMBTransactionsCls.vb` lines 1281, 1461) continue to work unchanged
with standard IND/INC behaviour.

### Fix 2: bSIRPremiumFinance.vb

**File**: `Sirius Back Office Core\Components\Instalments\Business\bSIRPremiumFinance\bSIRPremiumFinance.vb`

**Change**: Updated `TransactPlanThirdParty` (line 8044) to extract and pass the plan
transtype:

```vbnet
' BEFORE (line 8044):
m_lReturn = m_oPremiumFinance.TransactPremiumFinance(
	... v_lAgentType:=nAgentType)

' AFTER:
m_lReturn = m_oPremiumFinance.TransactPremiumFinance(
	... v_lAgentType:=nAgentType,
	v_sPlanTransType:=gPMFunctions.ToSafeString(vPremiumFinance(bSIRPremFinConst.k_PFPlanTransactionType, 0)).Trim())
```

For claim recovery plans, `vPremiumFinance(k_PFPlanTransactionType, 0)` contains `"SR"` or
`"TPR"`. For standard PF plans it contains `"NB"`, `"REN"`, or `"MTA"`, which are handled
by the existing IND/INC/IRD/IRC logic in `PostTransactions`.

### Fix 3: bACTImportSiriusTrans.vb

**File**: `Orion\Components\ImportSiriusTrans\Business\bACTImportSiriusTrans\bACTImportSiriusTrans.vb`

**Change**: Added `ICD` to the debit document prefix check (line 1305):

```vbnet
' BEFORE:
And (v_sDocRef.ToUpper().StartsWith("IND") Or v_sDocRef.ToUpper().StartsWith("IRD") Or v_sDocRef.ToUpper().StartsWith("IED")) Then

' AFTER:
And (v_sDocRef.ToUpper().StartsWith("IND") Or v_sDocRef.ToUpper().StartsWith("IRD") Or v_sDocRef.ToUpper().StartsWith("IED") Or v_sDocRef.ToUpper().StartsWith("ICD")) Then
```

This ensures TAX and COMM suspended transactions on ICD documents are correctly processed
during import (retrieving the `PremiumFinanceCnt` and `PremiumFinanceVer` for suspended
transaction posting).

### Fix 4: PureInsurance.REST DocumentTypeType.cs

**File**: `PureInsurance.REST\PureInsurance.REST.Common.Domain\Enums\DocumentTypeType.cs`

**Change**: Added `ICD = 9` and `ICC = 10` enum values:

```csharp
// BEFORE:
public enum DocumentTypeType
{
	JN = 0, IND = 1, INC = 2, IED = 3, IEC = 4,
	IRD = 5, IRC = 6, IID = 7, IIC = 8
}

// AFTER:
public enum DocumentTypeType
{
	JN = 0, IND = 1, INC = 2, IED = 3, IEC = 4,
	IRD = 5, IRC = 6, IID = 7, IIC = 8,
	ICD = 9, ICC = 10
}
```

Values match the SSP handler version (`SSP.PureInsuranceRestAPIHandler`).

---

## 5. Key Constants & Enum Values

### gACTLibrary.vb Constants (both Shared Files and SSP.Shared copies)

```vbnet
' Range Codes (document_ref prefixes)
Public Const ACTAutoNumberRangeCodeICD As String = "ICD"
Public Const ACTAutoNumberRangeCodeICC As String = "ICC"

' Document Type IDs (maps to DocumentType table)
Public Const ACTDocTypeInstalmentClaimDebit As Integer = 59
Public Const ACTDocTypeInstalmentClaimCredit As Integer = 60

' Group Codes (auto-number pool identifiers)
Public Const ACTAutoNumberGroupCodeDocumentRef59 As String = "DOCREF59"   ' ICD pool
Public Const ACTAutoNumberGroupCodeDocumentRef60 As String = "DOCREF60"   ' ICC pool
```

### Comparison with Standard PF Constants

| Purpose | NB | Renewal | Endorsement | Claim Recovery |
|---------|-----|---------|-------------|---------------|
| **Debit Range Code** | IND | IRD | IED | **ICD** |
| **Credit Range Code** | INC | IRC | IEC | **ICC** |
| **Debit DocType ID** | 42 | 46 | 44 | **59** |
| **Credit DocType ID** | 43 | 47 | 45 | **60** |
| **Debit Group Code** | DOCREF42 | DOCREF46 | DOCREF44 | **DOCREF59** |
| **Credit Group Code** | DOCREF43 | DOCREF47 | DOCREF45 | **DOCREF60** |

### PFPremiumFinance Array Constants (bSIRPremFinConst)

| Constant | Purpose |
|----------|---------|
| `k_PFPlanTransactionType` | Holds the plan transtype (NB, REN, MTA, SR, TPR) |
| `k_PFPlanPFCnt` | Premium Finance count (plan ID) |
| `k_PFPlanPFVersion` | Premium Finance version |
| `k_PFPlanAmountToFinance` | Amount being financed |
| `k_PFPlanAutoGenPlanRef` | Auto-generated plan reference |

### REST API Enum Values

| Enum | Value | Both Repos |
|------|-------|-----------|
| `DocumentTypeType.ICD` | 9 | SSP Handler ✅, REST API ✅ (after fix) |
| `DocumentTypeType.ICC` | 10 | SSP Handler ✅, REST API ✅ (after fix) |

---

## 6. Database Prerequisites

### pfinstalments_transaction Table

ICC and ICD must exist in this reference table:

```sql
-- Verify:
SELECT pfinstalments_transaction_id, code, description
FROM pfinstalments_transaction
WHERE code IN ('ICC', 'ICD')
-- Expected:
-- 8 | ICC | Instalment Claim Credit
-- 9 | ICD | Instalment Claim Debit
```

### DocumentType Table

ICC and ICD **should** exist in the DocumentType table for document generation:

```sql
-- Check:
SELECT documenttype_id, code, description
FROM DocumentType
WHERE code IN ('ICC', 'ICD')
-- If missing, add:
-- INSERT INTO DocumentType (documenttype_id, code, description, ...)
-- VALUES (59, 'ICD', 'Instalment Claim Debit', ...)
-- VALUES (60, 'ICC', 'Instalment Claim Credit', ...)
```

**Note**: As of 2026-07-01, the FR-017 verification report flagged DocumentType entries for
ICC/ICD as an enhancement opportunity. These may not yet exist in all environments.

### ACTNumber_Range and ACTNumber_Pool Tables

Auto-number ranges for ICC and ICD must be configured:

```sql
-- Verify ranges exist:
SELECT * FROM ACTNumber_Range WHERE code IN ('ICC', 'ICD')

-- Verify pools exist per company:
SELECT r.code, p.*
FROM ACTNumber_Pool p
JOIN ACTNumber_Range r ON p.actnumber_range_id = r.actnumber_range_id
WHERE r.code IN ('ICC', 'ICD')
```

---

## 7. File Reference Index

### Modified Files (ICC/ICD Fix)

| File | Path | Change |
|------|------|--------|
| PF Business | `Orion\Components\PremiumFinance\Business\bACTPremiumFinance\bACTPremiumFinanceBusiness.vb` | Added `v_sPlanTransType` parameter to `TransactPremiumFinance`, threaded to `PostTransactions` calls |
| PF Installer | `Sirius Back Office Core\Components\Instalments\Business\bSIRPremiumFinance\bSIRPremiumFinance.vb` | `TransactPlanThirdParty` now passes `k_PFPlanTransactionType` |
| Import Trans | `Orion\Components\ImportSiriusTrans\Business\bACTImportSiriusTrans\bACTImportSiriusTrans.vb` | Added `ICD` to debit prefix check |
| REST Enum | `PureInsurance.REST\PureInsurance.REST.Common.Domain\Enums\DocumentTypeType.cs` | Added `ICD = 9, ICC = 10` |

### Already-Correct Files (No Changes Needed)

| File | Path | Why Correct |
|------|------|-------------|
| Constants | `Shared Files\gACTLibrary\gACTLibrary.vb` | ICD/ICC range codes, doc type IDs, group codes all present |
| Constants (copy) | `SSP.Shared\gACTLibrary\gACTLibrary.vb` | Mirror copy — same constants |
| In-house Instalments | `Orion\Components\Instalments\Business\bACTInstalments\bACTInstalmentsCls.vb` | Has `GetPlanTransType()` — queries DB directly for SR/TPR |
| SP Mapping | `Databases\Pure\Procedures\A-B\spu_ACT_Import_Update_Instalment_Transaction_Code.sql` | Correct INC→ICC, IND→ICD mapping for SR/TPR |
| SSP Handler Enum | `SSP.PureInsuranceRestAPIHandler\SSP.PureInsuranceRestAPIHandler\Enums\DocumentTypeType.cs` | Already had ICD=9, ICC=10 |

### Key Method Locations

| Method | File | Line (approx) | Purpose |
|--------|------|---------------|---------|
| `TransactPremiumFinance` | bACTPremiumFinanceBusiness.vb | 649 | Entry point for third-party PF posting — accepts `v_sPlanTransType` |
| `PostTransactions` | bACTPremiumFinanceBusiness.vb | 394 | Creates IND/INC or ICD/ICC documents based on `v_sPlanTransType` |
| `TransactPlanThirdParty` | bSIRPremiumFinance.vb | 7901 | Calls `TransactPremiumFinance` with plan transtype from array |
| `PostInstalmentDebit` | bACTInstalmentsCls.vb | 3425 | In-house debit posting — uses `GetPlanTransType` for SR/TPR override |
| `PostInstalmentCredit` | bACTInstalmentsCls.vb | 2732 | In-house credit posting — uses `GetPlanTransType` for SR/TPR override |
| `GetPlanTransType` | bACTInstalmentsCls.vb | 6830 | Queries `PFPremiumFinance.transtype` from DB |

---

## 8. Decision Log

| # | Decision | Rationale | Date |
|---|----------|-----------|------|
| 1 | Add `v_sPlanTransType` as `Optional` with default `""` to `TransactPremiumFinance` | Backward compatibility — existing callers (`bPMBTransactionsCls` lines 1281, 1461) continue unchanged with IND/INC default behaviour | 2026-07-01 |
| 2 | Do not add `GetPlanTransType` to `bACTPremiumFinanceBusiness` (unlike `bACTInstalmentsCls`) | `bACTPremiumFinanceBusiness` does not have access to `pfprem_finance_cnt` at the `TransactPremiumFinance` level; the caller already knows the plan transtype, so passing it as a parameter is the correct approach | 2026-07-01 |
| 3 | Only add `ICD` (not `ICC`) to import prefix check at line 1305 | The check is for debit documents with TAX/COMM suspended transactions — credits don't apply here. ICC documents are credit-side and handled separately | 2026-07-01 |
| 4 | REST enum values `ICD = 9, ICC = 10` to match SSP handler | Consistency — both REST API projects must use the same enum values | 2026-07-01 |

---

## 9. Debugging Checklist

When ICC/ICD transactions are not being generated, check the following in order:

### Step 1: Verify Plan TransType

```sql
SELECT pfprem_finance_cnt, pfprem_finance_version, transtype
FROM PFPremiumFinance
WHERE pfprem_finance_cnt = @your_plan_cnt
-- Expected: transtype = 'SR' or 'TPR' for claim recovery
```

### Step 2: Verify Auto-Number Configuration

```sql
-- Range must exist:
SELECT * FROM ACTNumber_Range WHERE code IN ('ICC', 'ICD')

-- Pool must exist per company:
SELECT r.code, p.* FROM ACTNumber_Pool p
JOIN ACTNumber_Range r ON p.actnumber_range_id = r.actnumber_range_id
WHERE r.code IN ('ICC', 'ICD') AND p.company_id = @your_company_id
```

### Step 3: Verify DocumentType Entries

```sql
SELECT documenttype_id, code, description FROM DocumentType
WHERE code IN ('ICC', 'ICD')
-- If missing, documents cannot be created
```

### Step 4: Verify pfinstalments_transaction Codes

```sql
SELECT pfinstalments_transaction_id, code, description
FROM pfinstalments_transaction
WHERE code IN ('ICC', 'ICD')
-- Expected: ICC (ID 8), ICD (ID 9)
```

### Step 5: Trace the Posting Path

**Which path is being used?**

- **In-house plan** → `bACTInstalmentsCls.PostInstalmentDebit/Credit` → uses `GetPlanTransType` (self-contained)
- **Third-party plan** → `bSIRPremiumFinance.TransactPlanThirdParty` → `bACTPremiumFinanceBusiness.TransactPremiumFinance` → `PostTransactions` (needs `v_sPlanTransType` parameter)

**For third-party path, verify the call chain:**

1. Does `TransactPlanThirdParty` pass `v_sPlanTransType`?
2. Does `TransactPremiumFinance` have the `v_sPlanTransType` parameter?
3. Do all 4 `PostTransactions` calls inside `TransactPremiumFinance` pass `v_sPlanTransType`?

### Step 6: Check Generated Documents

```sql
SELECT document_id, document_ref, documenttype_id
FROM Document
WHERE document_ref LIKE 'ICC%' OR document_ref LIKE 'ICD%'
ORDER BY document_id DESC
-- If empty, documents are not being generated
```

### Step 7: Check pfinstalments Transaction Codes

```sql
SELECT i.pfinstalments_id, i.transactioncode, t.code
FROM pfinstalments i
LEFT JOIN pfinstalments_transaction t ON i.transactioncode = t.pfinstalments_transaction_id
WHERE i.pfprem_finance_cnt = @your_plan_cnt
-- For claim recovery: code should be ICC or ICD, not INC or IND
```

---

## Appendix: Adding New Instalment Transaction Types

If you need to add a new instalment transaction type (e.g. for a new business flow), follow
this pattern based on the ICC/ICD implementation:

1. **Database**: Add codes to `pfinstalments_transaction`, `DocumentType`, `ACTNumber_Range`, `ACTNumber_Pool`
2. **Constants**: Add range code, doc type ID, and group code constants to both `gACTLibrary.vb` copies
3. **bACTInstalmentsCls.vb**: Add SR/TPR-style override in `PostInstalmentDebit` and `PostInstalmentCredit` using `GetPlanTransType`
4. **bACTPremiumFinanceBusiness.vb**: Add override logic in `PostTransactions` based on `v_sPlanTransType`
5. **bSIRPremiumFinance.vb**: Ensure caller passes the plan transtype via `v_sPlanTransType`
6. **bACTImportSiriusTrans.vb**: Add new debit prefix to the StartsWith check at line ~1305
7. **REST API**: Add enum values to both `DocumentTypeType.cs` files
8. **SP Mapping**: Update `spu_ACT_Import_Update_Instalment_Transaction_Code` with new mapping rules

---

*Generated from ICC/ICD fix analysis. Traceability: Epic #39472, Epic #39336, PBI #37524*
