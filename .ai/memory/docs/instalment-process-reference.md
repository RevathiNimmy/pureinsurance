# Instalment Process — Complete Technical Reference

> End-to-end reference for how instalments work in Pure Insurance: portal UI, provider objects,
> SAM service layer, bind/live flow, statistics/export posting, Orion accounting documents,
> and premium finance collection.
>
> **Use this file** when implementing instalment-related features in new flows (e.g. claim recovery,
> new transaction types, or extending premium finance to other business areas).
>
> **Last Updated**: 2026-06-26
> **Owned By**: Pure Insurance Team

---

## Table of Contents

1. [Architecture Overview](#1-architecture-overview)
2. [Portal UI Layer](#2-portal-ui-layer)
3. [Provider Objects Layer](#3-provider-objects-layer)
4. [SAM Service Layer](#4-sam-service-layer)
5. [Bind / Go-Live Flow](#5-bind--go-live-flow)
6. [Statistics & Export Layer](#6-statistics--export-layer)
7. [Document Reference Generation](#7-document-reference-generation)
8. [Orion Accounting — INC/IND Documents](#8-orion-accounting--incinddocuments)
9. [Premium Finance Tables](#9-premium-finance-tables)
10. [Instalment Transaction Codes](#10-instalment-transaction-codes)
11. [Stored Procedures Reference](#11-stored-procedures-reference)
12. [Key Constants & Enums](#12-key-constants--enums)
13. [Implementation Checklist for New Flows](#13-implementation-checklist-for-new-flows)

---

## 1. Architecture Overview

The instalment process spans four architectural tiers:

```
Portal (ASP.NET Web Forms)
  → NexusProvider (ProviderBase / ProviderSAMForInsuranceV2)
	→ SAM Service Agent → CoreSAMBusiness (WCF/SOAP)
	  → SQL Server Stored Procedures

Orion Accounting (VB.NET back-office components)
  → bACTInstalmentsCls / bACTPremiumFinanceBusiness
	→ bACTDocumentPost → Document table
```

### Flow Summary

| Phase | What Happens | Key Components |
|-------|-------------|----------------|
| **Quote** | User selects instalment plan on portal | `Instalments.aspx`, `GetInstalmentQuotes` |
| **Bind/Live** | Quote committed as live policy | `TransactionConfirmation.aspx.vb`, `CoreSamBusiness-Quote.vb` |
| **Statistics** | Stats folder + export folder created | `bControlTransAutomated`, `spu_add_stats_folder` |
| **Import** | Stats exported to Orion as Documents | `bACTImportSiriusTrans` |
| **Collection** | Instalment debits/credits posted | `bACTInstalmentsCls`, `bACTPremiumFinanceBusiness` |

---

## 2. Portal UI Layer

### Pages & Controls

| File | Purpose |
|------|---------|
| `Web Portal\Nexus\Pure.Portals\secure\payment\Instalments.aspx` | Main NB instalment selection page — plan grid, bank details, date selection |
| `Web Portal\Nexus\Pure.Portals\secure\payment\Instalments.aspx.vb` | Code-behind — loads taxes/fees, computes amount to finance, calls `GetInstalmentQuotes`, handles `btnNext_Click` |
| `Web Portal\Nexus\Pure.Portals\Controls\Instalments.ascx` | Reusable instalment control for MTA/renewal flows |
| `Web Portal\Nexus\Pure.Portals\secure\TransactionConfirmation.aspx.vb` | Final page that calls `BindQuote` to commit the policy as live |
| `Web Portal\Nexus\Pure.Portals\App_Code\Nexus\BasePayment.vb` | Base class — `SetPaymentTakenAndRedirect()` sets `Session(CNPaid)=True` and redirects to confirmation |

### Key Session Variables

| Session Key | Type | Purpose |
|-------------|------|---------|
| `CNQuote` | Object | The active quote being processed |
| `CNAmountToPay` | Decimal | Total premium amount |
| `CNPayment` | `NexusProvider.Payment` | Payment object with instalment details |
| `CNSelectedPaymentIndex` | Integer | Selected instalment scheme index |
| `CNPaid` | Boolean | Set `True` after payment taken |
| `CNIsTransactionConfirmationVisited` | Boolean | Guards against re-processing |

### Portal Instalment Flow (NB)

```
1. Instalments.aspx.vb Page_Load:
   - Calls GetHeaderAndPolicyTaxByKey, GetHeaderAndPolicyFeesByKey,
	 GetHeaderAndRiskFeesByKey, GetHeaderAndRiskTaxByKey
   - Computes AmountToFinance = TotalPremium - Taxes/Fees (non-financed)
   - Calls Provider.GetInstalmentQuotes(AmountToFinance, ...)
   - Populates grdInstallmentQuotes GridView with scheme options

2. User selects a scheme → ShowDetailsForScheme() displays:
   - Monthly amount, deposit, APR, interest, total cost
   - PopulateInstalmentDates() for payment date selection

3. btnNext_Click:
   - Builds Payment object with selected scheme details
   - Sets Session(CNPayment) with all instalment/bank details
   - Calls BasePayment.SetPaymentTakenAndRedirect()
   - Redirects to ~/secure/TransactionConfirmation.aspx

4. TransactionConfirmation.aspx.vb Page_Load:
   - Calls BindQuote(transactionType, payment, ...)
   - For PremiumFinance: displays instalment plan reference
   - Calls DoInstalmentDeposit() / DoInstalmentDepositVersion2()
```

---

## 3. Provider Objects Layer

### Key Objects

| File | Class | Purpose |
|------|-------|---------|
| `NexusProvider\Objects\Payment.vb` | `Payment` | Carries all instalment and banking details across portal and SAM calls |
| `NexusProvider\Objects\InstallmentQuote.vb` | `InstalmentQuote` / `InstalmentQuoteCollection` | Represents a scheme quote returned from SAM |
| `NexusProvider\Objects\Instalment.vb` | `Instalment` / `InstalmentsCollection` | Individual instalment line item (number, amount, due date, status) |

### Payment Object — Key Instalment Properties

```vbnet
' Payment.vb key properties for instalments:
AmountToFinance         ' Total amount being financed
SelectedSchemeNo        ' Selected scheme number
SelectedSchemeVersion   ' Selected scheme version
PreferredDate           ' User's preferred payment date
StartDate               ' Plan start date
EndDate                 ' Plan end date
MonthDay                ' Day of month for collection
WeekDay                 ' Day of week (if weekly)
InstallmentType         ' Monthly/Weekly/etc
InstDepositAmount       ' Deposit amount
' Bank detail fields for direct debit:
BankSortCode, BankAccountNo, BankAccountName, etc.
```

### InstalmentQuote Object — Key Properties

```vbnet
' InstalmentQuote.vb:
SchemeNo, SchemeVersion, CompanyNo
MediaTypeDescription    ' e.g. "Direct Debit"
DepositAmount, AprRate, InterestRate
DaysDelay, AlignTo, StartLimit
DepositAsInstalment     ' Whether deposit counts as first instalment
FinanceToNet            ' Whether to finance net or gross
```

### Provider Method Chain

```
Portal → NexusProvider.ProviderManager().Provider
	   → ProviderSAMForInsuranceV2.Policy.vb (adapter)
	   → PureService.vb (SOAP proxy)
	   → SAM WCF endpoint
```

File: `Web Portal\Nexus\NexusProvider.SAMForInsurance\ProviderSAMForInsuranceV2.Policy.vb`

---

## 4. SAM Service Layer

### Service Agent

File: `Web Services\STS\SAM Solution\SiriusFS.SAM.ServiceAgent\Local_Code\ServiceImplementationLayer\SAMForInsuranceBusiness.vb`

Key method: `GetInstalmentQuotes(ByVal oRequest As GetInstalmentQuotesRequestType)`
- Forwards to `CoreSAMBusiness.GetInstalmentQuotes(...)`

### Core Business

File: `Web Services\STS\SAM Solution\SiriusFS.SAM.CoreImplementation\CoreSamBusiness-Instalments.vb`
- Contains premium finance / instalment-specific business logic
- Methods like `GetHeaderAndSummariesPFPlanByKey`

File: `Web Services\STS\SAM Solution\SiriusFS.SAM.CoreImplementation\CoreSamBusiness-Quote.vb`
- Contains `BindQuote(...)`, `ProcessBindQuote(...)`, `ProcessNewBusinessBindQuote(...)`
- Contains `ProcessAccounts(...)` which triggers statistics/export creation

---

## 5. Bind / Go-Live Flow

### Call Chain

```
TransactionConfirmation.aspx.vb
  → Provider.BindQuote(transactionType, quoteRef, ...)
	→ SAMForInsuranceBusiness.BindQuote(BindQuoteRequest)
	  → CoreSAMBusiness.BindQuote(...)
		→ ProcessBindQuote(...)
		  → ProcessNewBusinessBindQuote(...)    [for NB]
			→ ProcessAccounts(...)
			  → bControlTransAutomated.Start(...)
				→ GetNextOrionDocRef()          [range code "SND"]
				→ ProcessTransactions(...)
				  → CreateStats(...)
					→ CreateStatsFolder(...)     [→ spu_add_stats_folder]
					→ CreateStatsDetails(...)    [→ spu_add_stats_details]
				  → CreateExport(...)
					→ CreateExportFolder(...)    [→ spu_add_trans_export_folder]
					→ CreateExportDetails(...)
				  → PostDocument(...)
			  → ProcessInstalmentsDeposit(...)   [if PF deposit needed]
				→ CreateDepositStats(...)
				  → CreateDepositStatsFolder(...)  [→ spu_add_stats_folder_Deposit]
				  → CreateDepositStatsDetails(...)
				→ CreateDepositExport(...)
				→ PostDocument(...)
```

### Transaction Types

| Code | Description | Used In |
|------|-------------|---------|
| NB | New Business | `ProcessNewBusinessBindQuote` |
| MTA | Mid-Term Adjustment | `ProcessMTABindQuote` |
| REN | Renewal | `ProcessRenewalBindQuote` |
| MTC | Mid-Term Cancellation | `ProcessMTCBindQuote` |
| MTR | Policy Reinstatement | `ProcessMTRBindQuote` |

---

## 6. Statistics & Export Layer

### Component

File: `Sirius For Underwriting\Components\Statistics\Business\bControlTrans\bControlTransAutomated.vb`
SQL constants: `Sirius For Underwriting\Components\Statistics\Business\bControlTrans\bControlTransSQL.vb`

### Key Methods

| Method | Line (approx) | Purpose |
|--------|--------------|---------|
| `Start(...)` | 627 | Entry point — generates doc ref if needed, calls `ProcessTransactions` |
| `ProcessTransactions(...)` | 964 | Orchestrates stats + export creation |
| `CreateStats(...)` | — | Calls `CreateStatsFolder` then `CreateStatsDetails` |
| `CreateStatsFolder(...)` | 1874 | Passes `m_sNextOrionDocRef` to `spu_add_stats_folder` |
| `CreateStatsDetails(...)` | — | Inserts breakdown rows into `Stats_Details` |
| `ProcessInstalmentsDeposit(...)` | 2791 | Creates deposit-specific stats and export records |
| `CreateDepositStatsFolder(...)` | — | Calls `spu_add_stats_folder_Deposit` with `m_sNextOrionDocRefForInstalment` |
| `GetNextOrionDocRef()` | 3152 | Generates standard doc ref via `bACTAutoNumber` with range code `"SND"` |
| `GetNextOrionDocRefForInstalment()` | 3197 | Generates instalment doc ref via `bACTAutoNumber` with range code `"JN"` |
| `CheckIfInstalmentDepositRequired(...)` | 3232 | Checks if a deposit posting is needed for PF plan |

### SQL Constant Mapping (bControlTransSQL.vb)

```vbnet
ACAddStatsFolderSQL     = "spu_add_stats_folder"
ACAddExportFolderSQL    = "spu_add_trans_export_folder"
' Deposit variant called directly as "spu_add_stats_folder_Deposit"
```

### Member Variables

```vbnet
Private m_sNextOrionDocRef As String = ""               ' Standard doc ref (SND range)
Private m_sNextOrionDocRefForInstalment As String = ""   ' Instalment doc ref (JN range)
Private Const ACPaymentOptionInstalments As Integer = 3  ' Payment method ID for instalments
```

---

## 7. Document Reference Generation

### Two Distinct Numbering Systems

The system uses **two separate doc-ref generation paths** that produce different document reference formats:

#### A) Statistics Layer (Bind/Live Time)

Generated by `bControlTransAutomated` → stored in `Stats_Folder` → copied to `Transaction_Export_Folder`.

| Range Code | Generated By | Prefix Built In SQL | Used For |
|-----------|-------------|-------------------|----------|
| `SND` | `GetNextOrionDocRef()` | `S` + trans-type + debit/credit | Standard postings |
| `JN` | `GetNextOrionDocRefForInstalment()` | `JN` | Instalment deposit journals |

**How `spu_add_stats_folder` builds the `document_ref`:**

```sql
-- @IsInstalments is always passed as 'S' from bControlTransAutomated
SELECT @IsInstalments = 'S'
SELECT @debit_credit = CASE WHEN this_premium >= 0 THEN 'D' ELSE 'C' END
	FROM insurance_file WHERE insurance_file_cnt = @insurance_file_cnt

SELECT @document_prefix = CASE @transaction_type_code
	WHEN 'MTA' THEN @IsInstalments + 'E' + @debit_credit   -- SED or SEC
	WHEN 'MTC' THEN @IsInstalments + 'E' + @debit_credit   -- SEC or SED
	WHEN 'MTR' THEN @IsInstalments + 'I' + @debit_credit   -- SID
	WHEN 'REN' THEN @IsInstalments + 'R' + @debit_credit   -- SRD or SRC
	ELSE @IsInstalments + 'N' + @debit_credit               -- SND or SNC
END

SELECT @document_ref = @document_prefix + @next_orion_doc_ref
-- Result e.g.: "SND0000001234"
```

**How `spu_add_stats_folder_Deposit` builds it:**

```sql
@document_prefix = 'JN'
@document_ref = @document_prefix + @next_orion_doc_ref
-- Result e.g.: "JN0000001234"
@document_comment = 'Instalment NB Deposit' (or MTA/MTC/Renewal variant)
```

#### B) Orion Accounting Layer (Instalment Collection Time)

Generated by `bACTInstalmentsCls` / `bACTPremiumFinanceBusiness` → stored directly in `Document` table.

| Range Code | Doc Type ID Constant | Meaning |
|-----------|---------------------|---------|
| `IND` | `ACTDocTypeInstalmentDebit` | Instalment NB Debit |
| `INC` | `ACTDocTypeInstalmentNBCredit` | Instalment NB Credit |
| `IED` | `ACTDocTypeInstalmentEndorsementDebit` | Instalment Endorsement Debit |
| `IEC` | `ACTDocTypeInstalmentEndorsementCredit` | Instalment Endorsement Credit |
| `IRD` | `ACTDocTypeInstalmentRenewalDebit` | Instalment Renewal Debit |
| `IRC` | `ACTDocTypeInstalmentRenewalCredit` | Instalment Renewal Credit |

**Generation pattern (from `bACTInstalmentsCls.PostInstalmentDebit`):**

```vbnet
' 1. Get number range
m_oAutoNumber.GetNumberRange(v_sGroupCode:=GroupCodeDocumentRef42,
							 v_sRangeCode:="IND",
							 r_lNumberRangeID:=lNumberRangeID)

' 2. Generate next number
m_oAutoNumber.GenerateDocumentReferenceNumber(
	v_lNumberRangeID:=lNumberRangeID,
	v_iUserID:=m_iUserID,
	v_iCompanyID:=m_lCompanyID,
	r_sDocumentRef:=sDocumentRef,
	v_sRangeCode:="IND")

' 3. Format: prefix + padded number
sDocumentRef = "IND" & sDocumentRef   ' e.g. "IND0000001234"

' 4. Create document in Orion
m_oDocumentPost.AddDocument(
	v_lDocumentTypeId:=ACTDocTypeInstalmentDebit,
	v_sDocumentRef:=sDocumentRef, ...)
```

### Auto-Number Infrastructure

The `bACTAutoNumber` component (`Orion\Components\AutoNumber\Business\bACTAutoNumber\`) manages
sequential number pools per range code. Each range code has:
- An `ACTNumber_Range` row (defines the code like "IND", "INC", "SND")
- An `ACTNumber_Pool` per company (tracks next available number)
- Numbers are allocated atomically to prevent duplicates

---

## 8. Orion Accounting — INC/IND Documents

### Component Files

| File | Purpose |
|------|---------|
| `Orion\Components\Instalments\Business\bACTInstalments\bACTInstalmentsCls.vb` | Primary instalment posting — creates IND/INC/IRD/IRC documents |
| `Orion\Components\PremiumFinance\Business\bACTPremiumFinance\bACTPremiumFinanceBusiness.vb` | Premium finance collection — also creates IND/INC documents |
| `Orion\Components\ImportSiriusTrans\Business\bACTImportSiriusTrans\bACTImportSiriusTrans.vb` | Imports `Transaction_Export_Folder` rows into Orion `Document` table |

### bACTInstalmentsCls — Key Methods

| Method | Purpose | Doc Type Created |
|--------|---------|-----------------|
| `PostInstalmentDebit(...)` (line ~3401) | Posts debit side of instalment | `IND` (NB) or `IRD` (renewal) |
| Instalment credit posting (line ~2580) | Posts credit side when payment received | `INC` (NB) or `IRC` (renewal) |
| `PostDeposit(...)` | Posts deposit portion | Uses `JN` journal |

### PostInstalmentDebit — Default Parameters

```vbnet
Optional v_lDocumentTypeID As Integer = gACTLibrary.ACTDocTypeInstalmentDebit
Optional v_sRangeCode As String = gACTLibrary.ACTAutoNumberRangeCodeIND   ' "IND"
Optional v_sDocRef As Object = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef42
```

### Renewal vs NB Logic

```vbnet
' Callers set the doc type based on whether the plan is for a renewal:
If sDocumentTypeCode = kDocumentTypeCodeRenewalInvoice OrElse bResetToIRD Then
	lDocumentTypeID = gACTLibrary.ACTDocTypeInstalmentRenewalDebit
	sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeIRD   ' "IRD"
Else
	lDocumentTypeID = gACTLibrary.ACTDocTypeInstalmentDebit
	sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeIND   ' "IND"
End If
```

### bACTPremiumFinanceBusiness — Debit/Credit Selection

```vbnet
If v_sTransType = "D" Then
	sACTAutoNumberRangeCode = gACTLibrary.ACTAutoNumberRangeCodeIND          ' "IND"
	sACTAutoNumberGroupCodeDocument = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef42
	iACTDocType = gACTLibrary.ACTDocTypeInstalmentNBDebit
ElseIf v_sTransType = "C" Then
	sACTAutoNumberRangeCode = gACTLibrary.ACTAutoNumberRangeCodeINC          ' "INC"
	sACTAutoNumberGroupCodeDocument = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef43
	iACTDocType = gACTLibrary.ACTDocTypeInstalmentNBCredit
End If
```

### bACTImportSiriusTrans — Document Type Resolution

When importing from `Transaction_Export_Folder`, the document type is extracted from the 3-char prefix
of `document_ref`:

```vbnet
' PostDocument method (line ~870):
sDocumentType.Value = sDocumentRef.Value.Substring(0, 3)  ' e.g. "IND", "SND", "JN"

' Then looked up from DocumentType table:
sSQL = "SELECT documenttype_id from documenttype WHERE code = {code}"
```

Special handling for JN (journal / 2-char prefix):
```vbnet
If sDocumentRef.Value.Substring(0, 2) = gACTLibrary.ACTAutoNumberRangeCodeJn Then
	' Journal export folder path
End If
```

### How Document Table Gets Populated

```
spu_ACT_Add_Document:
  INSERT INTO Document (
	company_id, sub_branch_id, postingstatus_id, documenttype_id,
	auditset_id, batch_id, document_ref, document_date,
	created_date, authorised_date, comment, ...
  )
```

Called by `m_oDocumentPost.AddDocument(...)` in both `bACTInstalmentsCls` and `bACTImportSiriusTrans`.

---

## 9. Premium Finance Tables

### Core Tables

| Table | Purpose |
|-------|---------|
| `PFPremiumFinance` | Plan header — `pfprem_finance_cnt`, `pfprem_finance_version`, `transtype` |
| `pfinstalments` | Individual instalment rows — number, amount, due date, status, `transactioncode` |
| `pfinstalments_transaction` | Reference table — maps codes like `INC`, `IND`, `ICC`, `ICD` to IDs |
| `pfinstalments_status` | Instalment status reference — codes: `U` (Unpaid), `R` (Requested), `H` (Held), etc. |

### Premium Finance Plan Types (transtype)

| transtype | Description | Instalment Codes Used |
|-----------|-------------|----------------------|
| NB | New Business | INC, IND |
| MTA | Mid-Term Adjustment | IEC, IED |
| REN | Renewal | IRC, IRD |
| SR | Claim Recovery (Subrogation) | ICC, ICD |
| TPR | Third Party Recovery | ICC, ICD |

---

## 10. Instalment Transaction Codes

### Full Code Map

| Code | Meaning | When Used |
|------|---------|-----------|
| **IND** | Instalment New Business Debit | Plan created for NB — debit to client account |
| **INC** | Instalment New Business Credit | Payment received on NB instalment |
| **IED** | Instalment Endorsement Debit | Plan created for MTA |
| **IEC** | Instalment Endorsement Credit | Payment received on MTA instalment |
| **IRD** | Instalment Renewal Debit | Plan created for renewal |
| **IRC** | Instalment Renewal Credit | Payment received on renewal instalment |
| **ICC** | Instalment Claim Credit | Payment on claim recovery plan (maps from INC) |
| **ICD** | Instalment Claim Debit | Plan created for claim recovery (maps from IND) |
| **JN** | Journal | Deposit posting at bind/live time |

### Claim Recovery Mapping

`spu_ACT_Import_Update_Instalment_Transaction_Code` and `spu_PF_GetInstalmentTransactionCodeId`
handle the mapping of INC/IND → ICC/ICD for claim recovery plans:

```sql
-- spu_ACT_Import_Update_Instalment_Transaction_Code:
IF @plan_transtype IN ('SR', 'TPR')   -- Claim Recovery
BEGIN
	SET @resolved_code = CASE @pfinstalments_transaction_code
		WHEN 'INC' THEN 'ICC'    -- Instalment Claim Credit
		WHEN 'IND' THEN 'ICD'    -- Instalment Claim Debit
		ELSE @pfinstalments_transaction_code
	END
END

-- Then updates pfinstalments.transactioncode with the resolved ID
UPDATE pfinstalments SET transactioncode = @transaction_code_id
WHERE pfinstalments_id IN (
	SELECT MIN(pfinstalments_id) FROM pfinstalments
	WHERE status IN (SELECT pfinstalments_status_id FROM pfinstalments_status WHERE code IN ('U','R','H'))
	AND pfprem_finance_cnt = @pfprem_finance_cnt
	AND pfprem_finance_version = @pfprem_finance_version
	AND instalmentNumber <> 0
)
```

### REST API DocumentTypeType Enum

File: `SSP.PureInsuranceRestAPIHandler\SSP.PureInsuranceRestAPIHandler\Enums\DocumentTypeType.cs`

```csharp
public enum DocumentTypeType {
	JN  = 0,
	IND = 1,
	INC = 2,
	IED = 3,
	IEC = 4,
	IRD = 5,
	IRC = 6,
	IID = 7,
	IIC = 8
}
```

---

## 11. Stored Procedures Reference

### Statistics / Export Creation (Bind/Live Time)

| Procedure | File | Purpose |
|-----------|------|---------|
| `spu_add_stats_folder` | `Databases\Pure\Procedures\A-B\spu_add_stats_folder.sql` | Creates `Stats_Folder` row; builds `document_ref` from prefix + `@next_orion_doc_ref` |
| `spu_add_stats_folder_Deposit` | `Databases\Pure\Procedures\A-B\spu_add_stats_folder_Deposit.sql` | Creates deposit-specific `Stats_Folder` with `JN` prefix |
| `spu_add_stats_folder_reverse` | `Databases\Pure\Procedures\A-B\spu_add_stats_folder_reverse.sql` | Creates reversal stats folder |
| `spu_add_trans_export_folder` | `Databases\Pure\Procedures\A-B\spu_add_trans_export_folder.sql` | Copies `Stats_Folder` data into `Transaction_Export_Folder` |

### Transaction Export (Back Office)

| Procedure | File | Purpose |
|-----------|------|---------|
| `spu_pmb_trans_folder_add` | `Databases\Pure\Procedures\P-R\spu_pmb_trans_folder_add.sql` | Creates `Transaction_Export_Folder` row with `is_payable_by_instalments` flag |

### Orion Document Creation

| Procedure | File | Purpose |
|-----------|------|---------|
| `spu_ACT_Add_Document` | `Databases\Pure\Procedures\A-B\spu_ACT_Add_Document.sql` | Inserts into `Document` table with `documenttype_id` and `document_ref` |
| `spu_ACT_Generate_DocumentReference` | `Databases\Pure\Procedures\A-B\spu_ACT_Generate_DocumentReference.sql` | Alternative doc-ref generation (used by suspended agent commission flow only) |

### Premium Finance / Instalment

| Procedure | File | Purpose |
|-----------|------|---------|
| `spu_ACT_Import_Update_Instalment_Transaction_Code` | `Databases\Pure\Procedures\A-B\spu_ACT_Import_Update_Instalment_Transaction_Code.sql` | Maps INC/IND → ICC/ICD for claim recovery plans; updates `pfinstalments.transactioncode` |
| `spu_PF_GetInstalmentTransactionCodeId` | `Databases\Pure\Procedures\P-R\spu_PF_GetInstalmentTransactionCodeId.sql` | Resolves correct `pfinstalments_transaction_id` based on plan type |

### Key Tables Involved

| Table | Key Columns | Purpose |
|-------|-------------|---------|
| `Stats_Folder` | `stats_folder_cnt`, `insurance_file_cnt`, `document_ref`, `document_comment` | Staging for financial postings |
| `Transaction_Export_Folder` | `transaction_export_folder_cnt`, `document_ref`, `is_payable_by_instalments` | Export queue for Orion import |
| `Document` | `document_id`, `documenttype_id`, `document_ref`, `insurance_file_cnt` | Orion accounting documents |
| `DocumentType` | `documenttype_id`, `code` | Reference — maps INC/IND/SND etc. to IDs |
| `ACTNumber_Range` | `ACTnumber_range_id`, `code` | Auto-number range definitions |
| `ACTNumber_Pool` | `actnumber_pool_id`, `actnumber_range_id`, `company_id` | Per-company number sequences |

---

## 12. Key Constants & Enums

### gACTLibrary.vb Constants

File: `Shared Files\gACTLibrary\gACTLibrary.vb` (also mirrored in `SSP.Shared\gACTLibrary\`)

```vbnet
' === Auto-Number Range Codes (document_ref prefixes) ===

' Standard posting ranges (Statistics layer)
Public Const ACTAutoNumberRangeCodeSnd As String = "SND"   ' Standard NB Debit
Public Const ACTAutoNumberRangeCodeSnc As String = "SNC"   ' Standard NB Credit
Public Const ACTAutoNumberRangeCodeSed As String = "SED"   ' Standard Endorsement Debit
Public Const ACTAutoNumberRangeCodeSec As String = "SEC"   ' Standard Endorsement Credit
Public Const ACTAutoNumberRangeCodeSrd As String = "SRD"   ' Standard Renewal Debit
Public Const ACTAutoNumberRangeCodeSrc As String = "SRC"   ' Standard Renewal Credit
Public Const ACTAutoNumberRangeCodeShd As String = "SHD"   ' Standard Cancellation Debit
Public Const ACTAutoNumberRangeCodeShc As String = "SHC"   ' Standard Cancellation Credit

' Instalment posting ranges (Orion accounting layer)
Public Const ACTAutoNumberRangeCodeIND As String = "IND"   ' Instalment NB Debit
Public Const ACTAutoNumberRangeCodeINC As String = "INC"   ' Instalment NB Credit
Public Const ACTAutoNumberRangeCodeIED As String = "IED"   ' Instalment Endorsement Debit
Public Const ACTAutoNumberRangeCodeIEC As String = "IEC"   ' Instalment Endorsement Credit
Public Const ACTAutoNumberRangeCodeIRD As String = "IRD"   ' Instalment Renewal Debit
Public Const ACTAutoNumberRangeCodeIRC As String = "IRC"   ' Instalment Renewal Credit

' Other ranges
Public Const ACTAutoNumberRangeCodeJn As String = "JN"     ' Journal (deposit)
Public Const ACTAutoNumberRangeCodeFee As String = "FEE"   ' Client fees
Public Const ACTAutoNumberRangeCodeCla As String = "CLA"   ' Claim accrual
Public Const ACTAutoNumberRangeCodeClo As String = "CLO"   ' Claim payment
Public Const ACTAutoNumberRangeCodeSCD As String = "SCD"   ' Standard Claim Debit

' === Auto-Number Group Codes (reference group for number pools) ===
' Standard postings
Public Const ACTAutoNumberGroupCodeDocumentRef4 As String = "..."   ' SND group
Public Const ACTAutoNumberGroupCodeDocumentRef5 As String = "..."   ' SNC group
Public Const ACTAutoNumberGroupCodeDocumentRef15 As String = "..."  ' SRD group
Public Const ACTAutoNumberGroupCodeDocumentRef16 As String = "..."  ' SRC group
Public Const ACTAutoNumberGroupCodeDocumentRef17 As String = "..."  ' SED group
Public Const ACTAutoNumberGroupCodeDocumentRef18 As String = "..."  ' SEC group

' Instalment postings
Public Const ACTAutoNumberGroupCodeDocumentRef42 As String = "..."  ' IND group
Public Const ACTAutoNumberGroupCodeDocumentRef43 As String = "..."  ' INC group
Public Const ACTAutoNumberGroupCodeDocumentRef46 As String = "..."  ' IRD group

' === Document Type IDs ===
Public Const ACTDocTypeInstalmentDebit As Integer = ...
Public Const ACTDocTypeInstalmentNBCredit As Integer = ...
Public Const ACTDocTypeInstalmentRenewalDebit As Integer = ...
Public Const ACTDocTypeInstalmentRenewalCredit As Integer = ...
```

### bPMBTransactionsCls — Range Code Selection

File: `Sirius Back Office Core\Components\Transactions\Business\bPMBTransactions\bPMBTransactionsCls.vb`

```vbnet
' CreateExportFolder method (line ~1736):
' Maps transaction type + debit/credit to range code:
Select Case m_sDebitCredit
	Case "D"
		Select Case m_vLastTransType
			Case 1: m_sRangeCode = "SND"   ' NB Debit
			Case 2: m_sRangeCode = "SRD"   ' Renewal Debit
			Case 3: m_sRangeCode = "SED"   ' Endorsement Debit
			Case 4: m_sRangeCode = "SHD"   ' Cancellation Debit
			Case 5: m_sRangeCode = "TRD"   ' Transfer Debit
		End Select
	Case "C"
		Select Case m_vLastTransType
			Case 1: m_sRangeCode = "SNC"   ' NB Credit
			Case 2: m_sRangeCode = "SRC"   ' Renewal Credit
			Case 3: m_sRangeCode = "SEC"   ' Endorsement Credit
			Case 4: m_sRangeCode = "SHC"   ' Cancellation Credit
			Case 5: m_sRangeCode = "TRC"   ' Transfer Credit
		End Select
End Select
m_sDocumentRef = m_sRangeCode & m_sReference
```

---

## 13. Implementation Checklist for New Flows

When implementing instalments in a new flow (e.g. claim recovery), follow this checklist:

### A) Database Layer

- [ ] **Add new transaction codes** to `pfinstalments_transaction` table if needed (e.g. `ICC`, `ICD`)
- [ ] **Add new document type codes** to `DocumentType` table if needed
- [ ] **Add auto-number ranges** to `ACTNumber_Range` table for new codes
- [ ] **Create/update mapping procs** like `spu_ACT_Import_Update_Instalment_Transaction_Code`
  to resolve standard INC/IND to new codes based on plan `transtype`
- [ ] **Create/update code-resolution procs** like `spu_PF_GetInstalmentTransactionCodeId`
  for runtime lookup of the correct transaction code ID
- [ ] **Update `spu_pmb_trans_folder_add`** if new posting types need `is_payable_by_instalments` handling

### B) Constants Layer

- [ ] **Add range code constants** to `gACTLibrary.vb` (e.g. `ACTAutoNumberRangeCodeICC`)
- [ ] **Add group code constants** if new number pools are needed
- [ ] **Add document type ID constants** if new document types are created
- [ ] **Update REST API enum** `DocumentTypeType.cs` if the REST layer needs to reference new types

### C) Orion Accounting Layer

- [ ] **Update `bACTInstalmentsCls.vb`** — add new `PostInstalment*` method or extend existing
  ones to handle new transaction type with correct range code / doc type
- [ ] **Update `bACTPremiumFinanceBusiness.vb`** if the new flow uses PF collection
- [ ] **Update `bACTImportSiriusTrans.vb`** if new doc-type prefixes need special handling
  during import (the 3-char prefix extraction at line ~870 should work automatically if
  the `DocumentType` table has the new code)

### D) Statistics / Export Layer

- [ ] **Update `bControlTransAutomated.vb`** if the new flow needs deposit posting
  (`ProcessInstalmentsDeposit` / `CreateDepositStatsFolder`)
- [ ] **Update `spu_add_stats_folder.sql`** if new transaction type codes need new
  `@document_comment` or `@document_prefix` mappings
- [ ] **Update `spu_add_stats_folder_Deposit.sql`** if deposit handling differs

### E) Portal / Provider Layer (if user-facing)

- [ ] **Create or extend payment pages** following `Instalments.aspx` / `Instalments.aspx.vb` pattern
- [ ] **Extend `Payment.vb`** object if new properties are needed
- [ ] **Update `TransactionConfirmation.aspx.vb`** bind flow if new transaction type
- [ ] **Update provider / SAM methods** for new quote/bind operations

### F) Testing

- [ ] Verify auto-number sequence generates without gaps
- [ ] Verify `Stats_Folder` rows have correct `document_ref` prefix
- [ ] Verify `Transaction_Export_Folder` rows have correct `is_payable_by_instalments`
- [ ] Verify `Document` table rows have correct `documenttype_id` and `document_ref`
- [ ] Verify `pfinstalments.transactioncode` resolves to correct code
- [ ] Test renewal vs NB path to confirm IRD/IRC vs IND/INC selection
- [ ] Test claim recovery mapping (INC→ICC, IND→ICD) if applicable

---

## Appendix: File Index

### Portal Layer
| File | Path |
|------|------|
| Instalment Selection Page | `Web Portal\Nexus\Pure.Portals\secure\payment\Instalments.aspx(.vb)` |
| Instalment Control | `Web Portal\Nexus\Pure.Portals\Controls\Instalments.ascx` |
| Transaction Confirmation | `Web Portal\Nexus\Pure.Portals\secure\TransactionConfirmation.aspx.vb` |
| Base Payment | `Web Portal\Nexus\Pure.Portals\App_Code\Nexus\BasePayment.vb` |

### Provider Layer
| File | Path |
|------|------|
| Payment Object | `Web Portal\Nexus\NexusProvider\Objects\Payment.vb` |
| Instalment Quote Object | `Web Portal\Nexus\NexusProvider\Objects\InstallmentQuote.vb` |
| Instalment Object | `Web Portal\Nexus\NexusProvider\Objects\Instalment.vb` |
| Provider Base | `Web Portal\Nexus\NexusProvider\ProviderBase.vb` |
| SAM Provider (Policy) | `Web Portal\Nexus\NexusProvider.SAMForInsurance\ProviderSAMForInsuranceV2.Policy.vb` |

### SAM / Core Business Layer
| File | Path |
|------|------|
| SAM Service Agent | `Web Services\STS\SAM Solution\SiriusFS.SAM.ServiceAgent\Local_Code\ServiceImplementationLayer\SAMForInsuranceBusiness.vb` |
| Core - Instalments | `Web Services\STS\SAM Solution\SiriusFS.SAM.CoreImplementation\CoreSamBusiness-Instalments.vb` |
| Core - Quote/Bind | `Web Services\STS\SAM Solution\SiriusFS.SAM.CoreImplementation\CoreSamBusiness-Quote.vb` |

### Statistics / Export Layer
| File | Path |
|------|------|
| Control Trans Automated | `Sirius For Underwriting\Components\Statistics\Business\bControlTrans\bControlTransAutomated.vb` |
| Control Trans SQL | `Sirius For Underwriting\Components\Statistics\Business\bControlTrans\bControlTransSQL.vb` |

### Orion Accounting Layer
| File | Path |
|------|------|
| Instalments Component | `Orion\Components\Instalments\Business\bACTInstalments\bACTInstalmentsCls.vb` |
| Premium Finance Component | `Orion\Components\PremiumFinance\Business\bACTPremiumFinance\bACTPremiumFinanceBusiness.vb` |
| Import Sirius Trans | `Orion\Components\ImportSiriusTrans\Business\bACTImportSiriusTrans\bACTImportSiriusTrans.vb` |
| Shared Constants | `Shared Files\gACTLibrary\gACTLibrary.vb` |

### Back Office Core Layer
| File | Path |
|------|------|
| Transactions Component | `Sirius Back Office Core\Components\Transactions\Business\bPMBTransactions\bPMBTransactionsCls.vb` |
| Transactions SQL | `Sirius Back Office Core\Components\Transactions\Business\bPMBTransactions\bPMBTransactionsSQL.vb` |

### Stored Procedures
| File | Path |
|------|------|
| Add Stats Folder | `Databases\Pure\Procedures\A-B\spu_add_stats_folder.sql` |
| Add Stats Folder Deposit | `Databases\Pure\Procedures\A-B\spu_add_stats_folder_Deposit.sql` |
| Add Stats Folder Reverse | `Databases\Pure\Procedures\A-B\spu_add_stats_folder_reverse.sql` |
| Add Trans Export Folder | `Databases\Pure\Procedures\A-B\spu_add_trans_export_folder.sql` |
| Add Document | `Databases\Pure\Procedures\A-B\spu_ACT_Add_Document.sql` |
| Generate Document Reference | `Databases\Pure\Procedures\A-B\spu_ACT_Generate_DocumentReference.sql` |
| Update Instalment Transaction Code | `Databases\Pure\Procedures\A-B\spu_ACT_Import_Update_Instalment_Transaction_Code.sql` |
| Get Instalment Transaction Code ID | `Databases\Pure\Procedures\P-R\spu_PF_GetInstalmentTransactionCodeId.sql` |
| PMB Trans Folder Add | `Databases\Pure\Procedures\P-R\spu_pmb_trans_folder_add.sql` |

### REST API
| File | Path |
|------|------|
| Document Type Enum | `SSP.PureInsuranceRestAPIHandler\SSP.PureInsuranceRestAPIHandler\Enums\DocumentTypeType.cs` |
