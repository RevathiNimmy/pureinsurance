# Sirius Back Office Core — Components Reference

> Detailed reference for all business components in `Sirius Back Office Core\Components\`.
> These COM/VB.NET components are called by both the WCF SAM layer and the REST API microservices.
> Referenced from `.github/copilot-instructions.md`.
> **See also:** `.github/docs/back-office-ui-controls-reference.md` for all interface (`iPMB*`, `iSIR*`) and user control (`uct*`, `PMU*`) projects.

---

## Overview

The `Sirius Back Office Core\Components\` directory contains ~55 VB.NET component projects that implement the core business logic for the Pure Insurance platform. These components follow a layered naming convention:

### Naming Convention

| Prefix | Layer | Purpose |
|--------|-------|---------|
| `bSIR*` | **Business** | Business logic, validation, orchestration |
| `bSIR*SQL` | **Business SQL** | Stored procedure calls (data access within business layer) |
| `bSIR*Business` | **Business Logic** | Complex business rules |
| `dSIR*` | **Data** | Data access / persistence layer |
| `dSIR*SQL` | **Data SQL** | Direct stored procedure wrappers |
| `iPMB*` | **Interface (Broker)** | UI forms for broker/agency portal |
| `iPMU*` | **Interface (Underwriting)** | UI forms for underwriting portal |
| `bACT*` | **Accounting (Orion)** | Accounting/financial components (from Orion subsystem) |
| `bGIS*` | **GIS** | Gemini Insurance System dataset/screen engine |

### How Components Are Called

```
Web Portal (Pure.Portals) / REST API / WCF SAM
  -> ProviderSAMForInsuranceV2 / MediatR Handler
      -> COM Interop / Direct .NET Reference
          -> bSIR* Business Component
              -> bSIR*SQL / dSIR*SQL (stored procedure calls)
                  -> SQL Server
```

---

## Complete Project Inventory

Every `b*` (business) and `d*` (data) project in `Sirius Back Office Core\Components\`, grouped by component directory.

| Component | Business Projects (`b*`) | Data Projects (`d*`) |
|-----------|--------------------------|----------------------|
| **Account Transaction Batch** | `bSIRAccountTransBatch` | — |
| **Address** | `bSIRAddress` | `dSIRAddress` |
| **Bank Guarantee** | `bSIRBankGuarantee`, `bSIRFindBankGuarantee` | — |
| **Batch Scheduler** | `bSIRBatchScheduler` | — |
| **Cash Deposit** | `bSIRCashDeposit` | — |
| **Contact** | `bSIRContact` | `dSIRContact` |
| **Contact Type** | `bSIRContactType` | — |
| **Conviction** | `bSIRPartyConviction` | `dSIRPartyConviction` |
| **Crystal Reports** | `bSIRReportPrint` | — |
| **DebugTimings** | `bSIRDebugTimings` | — |
| **Document Production** | `bCCMDocumentProduction`, `bPMBDocManager`, `bSIRDocManagerWrapper`, `bSIRDocSpooler`, `bSIRDocTemplate`, `bSIRDocumentType`, `bSIRFieldManager`, `bSIRFindDocTemplate`, `bSIRSharepoint`, `bSIRSharepointOnline`, `bSIRSharePointOnlineValidate` | `dSIRDocSpooler`, `dSIRDocTemplate` |
| **Event** | `bSIREvent` | `dSIREvent` |
| **Export Control** | `bSirExportControl` | — |
| **ExternalWorkFlowConfiguration** | `bSIRExternalWorkflowConfig` | — |
| **Find Insurance** | `bSIRFindInsurance` | — |
| **Find Party** | `bSIRFindParty` | — |
| **Future Address Update** | `bSIRFutureAddressUpdate` | — |
| **Gemini List Manager** | `bGEMListCustom`, `bGEMListManager`, `bGEMListMgr`, `bGEMLists`, `bGEMListUpdate`, `bGEMListUser`, `bGEMLookup` | — |
| **GetChangeReason** | `bSIRGetChangeReason` | — |
| **HandlerTransfer** | `bSIRHandlerTransfer` | — |
| **Instalments** | `bSIRPFEDIMessage`, `bSIRPFExport`, `bSIRPFInstalments`, `bSIRPFRF`, `bSIRPFScheme`, `bSIRPFSubmit`, `bSIRPremiumFinance` | — |
| **Insurance File** | `bSIRInsuranceFile` | `dSIRInsuranceFile` |
| **Insurance File System** | `bSIRInsuranceFileSystem` | `dSIRInsuranceFileSystem` |
| **Insurance Folder** | `bSIRInsuranceFolder` | `dSIRInsuranceFolder` |
| **Lifestyle** | `bSIRLifeStyle` | `dSIRLifeStyle` |
| **MediaTypeValidation** | `bSIRMediaTypeValidation` | — |
| **OnlinePartyMaintenance** | `bSIROnlineParty` | — |
| **Orion Link** | `bPMBOrionLink`, `bSirOrionLink`, `bSIROrionUpdate` | — |
| **Party** | `bSIRParty`, `bSIRPartyAG`, `bSIRPartyAGG`, `bSIRPartyAH`, `bSIRPartyBank`, `bSIRPartyCC`, `bSIRPartyEX`, `bSIRPartyFee`, `bSIRPartyFP`, `bSIRPartyGC`, `bSIRPartyIN`, `bSIRPartyLifestyle`, `bSIRPartyNC`, `bSIRPartyNetData`, `bSIRPartyOT`, `bSIRPartyPC` | `dSIRParty`, `dSIRPartyAG`, `dSIRPartyAGG`, `dSIRPartyAH`, `dSIRPartyCC`, `dSIRPartyEX`, `dSIRPartyFP`, `dSIRPartyGC`, `dSIRPartyIN`, `dSIRPartyLifestyle`, `dSIRPartyNC`, `dSIRPartyNetData`, `dSIRPartyOT`, `dSIRPartyPC` |
| **Party Loyalty Scheme** | `bSIRPartyLoyaltyScheme` | — |
| **Policy Number Maintenance** | `bSIRPolicyNumMaint` | — |
| **Product Options** | `bSIRProductOptions` | — |
| **Report Group** | `bSIRReportGroup` | — |
| **Report Scheduler** | `bSIRReportScheduler` | — |
| **Sirius Back Office Link** | `bSIRIUSLink` | — |
| **SolutionConfig** | `bSIRSolutionConfig` | — |
| **System Options** | `bSIROptions` | — |
| **TaskScheduler** | `bSIRTaskScheduler` | — |
| **Text Files** | `bSIRTextFile`, `bSIRTextFileDescription` | `dSIRTextFile` |
| **Transactions** | `bPMBTransactions` | — |
| **Unposted Transactions** | `bSIRUnpostedTransactions` | — |

**Totals:** 105 `b*`/`d*` projects across 37 component directories.

---

## Component Reference

### Address
**Directory:** `Address/`
**Projects:** `bSIRAddress` (Business), `dSIRAddress` (Data)
**Purpose:** Manages postal addresses for parties (clients, agents, insurers). Supports add, update, delete, duplicate detection, and multi-use address linking. Uses the entity/collection pattern: `bSIRAddressCls` (entity), `bSIRAddresss` (collection), `dSIRAddress` (data layer).

**Business Methods:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByRef sUserName, sPassword As String, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel As Integer, sCallingAppName As String, Optional ByRef vDatabase As Object) As Integer` | Standard initialisation — sets up credentials, database, and system options |
| `SetProcessModes` | `(Optional ByRef vTask, vNavigate, vProcessMode, vTransactionType, vEffectiveDate As Object) As Integer` | Sets task/navigate/process mode context |
| `DirectAdd` | `(Optional ByRef vAddressCnt, vAddress1..vAddress10, vPostalCode, vCountryID, vExternalId As Object) As Integer` | Create a new address record directly in DB |
| `DirectDelete` | `(Optional ByRef vAddressCnt As Object) As Integer` | Remove an address by key |
| `CheckID` | `(ByRef vID As Object) As Integer` | Validate address ID exists |
| `GetDetails` | `(Optional ByRef vLockMode As PMELockMode = 0, Optional ByRef vAddressCnt As Object) As Integer` | Load full address details into collection |
| `GetNext` | `(Optional ByRef vAddressCnt, vSourceID, vAddressID, vAddress1..10, vPostalCode, vCountryID, vExternalId As Object) As Integer` | Iterator — get next address in the collection |
| `EditAdd` | `(ByRef lRow As Integer, Optional ByRef vAddressCnt, vAddress1..10, vPostalCode, vCountryID, vExternalId As Object, Optional sUniqueId, sScreenHierarchy As String) As Integer` | Add via in-memory edit buffer (UI-driven) |
| `EditUpdate` | `(ByRef lRow As Integer, Optional ByRef vAddressCnt, vAddress1..10, vPostalCode, vCountryID, vExternalId As Object, Optional sUniqueId, sScreenHierarchy As String) As Integer` | Update via in-memory edit buffer |
| `EditDelete` | `(ByVal lRow As Integer) As Integer` | Mark row for deletion in edit buffer |
| `Cancel` | `() As Integer` | Check if collection has unsaved changes |
| `Update` | `() As Integer` | Persist all collection changes (add/edit/delete) to DB with transaction |
| `GetContacts` | `(ByRef vContacts(,) As Object) As Integer` | Get contacts linked to this address |
| `UpdateContacts` | `(ByRef vAddressCnt As Object, ByRef vAddContacts() As Object) As Integer` | Update contacts for an address |
| `GetCountry` | `(ByRef iCountryId As Integer, ByRef sCountryCode As String, ByRef r_lPostalCodeVisibilityId As Integer) As Integer` | Get country details for an address |
| `GetBranchBaseCountry` | `(ByVal v_lSourceID As Integer, ByRef r_iCountryID As Integer) As Integer` | Get the base country for a branch/source |
| `MultipleUse` | `(ByVal v_lAddressCnt As Integer, ByRef r_bMultipleUse As Boolean) As Integer` | Check if address is shared by multiple parties |
| `DuplicateAddress` | `(ByVal v_lAddressCnt As Integer, v_sReference As String, v_lUserID As Integer, v_lSourceID As Integer, ByRef r_lNewAddressCnt As Integer) As Integer` | Clone an address, returns new AddressCnt |
| `GetLookupValues` | `(ByRef iLookupType As Integer, ByRef vTableArray(,), ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer` | Get lookup values via bPMLookup |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|--------|
| `spe_Address_saa` | `GetDetails` | Select all addresses for a party |
| `spe_Address_sel` | `SelectItem` (data layer) | Select single address |
| `spe_Address_add` | `AddItem` (data layer) | Insert new address |
| `spe_Address_del` | `DeleteItem` (data layer) | Delete address |
| `spe_Address_upd` | `UpdateItem` (data layer) | Update address |
| `spe_SIRAddress_check_id` | `CheckID` | Validate address ID uniqueness |
| `spu_accumulation_add` | `AddItem` | Add risk accumulation record for address |
| `spu_Address_Check` | `Check` (data layer) | Validate address fields |
| `spu_Address_Duplicate` | `DuplicateAddress` | Duplicate an address record |
| `spu_Address_MultipleUse` | `MultipleUse` | Check multi-party address usage |
| `spu_pm_caption_id_return` | `GetDefaults` | Get/create caption ID |

**References:** `bSIROptions` (system options), `bPMLookup` (lookup values), `dSIRAddress` (data layer)

---

### Bank Guarantee
**Directory:** `Bank Guarantee/`
**COM Name:** `bSIRBankGuarantee`, `bSIRFindBankGuarantee`
**Purpose:** Manages bank guarantees — creation, maintenance, attachment to policies and branches, history tracking. Includes find/search functionality.

**Business Methods — `bSIRBankGuarantee`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard initialisation |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Sets process modes |
| `AddBankGuarantee` | `(ByRef r_lBankGuaranteeId As Integer, ByVal v_lPartyCnt As Integer, v_sReference As String, v_cAmount As Decimal, v_dtStartDate As Date, v_dtEndDate As Date, ...) As Integer` | Create a new bank guarantee — returns new ID |
| `EditBankGuarantee` | `(ByVal v_lBankGuaranteeId As Integer, v_sReference As String, v_cAmount As Decimal, ...) As Integer` | Modify bank guarantee details |
| `UpdateBankGuaranteeDetails` | `(ByVal v_lBankGuaranteeId As Integer) As Integer` | Save all pending changes |
| `DelUnDelBankGuarantee` | `(ByVal v_lBankGuaranteeId As Integer) As Integer` | Soft-delete or restore a guarantee |
| `GetBankGuaranteeDetails` | `(ByRef r_vDetails(,) As Object) As Integer` | Load guarantee details by current key |
| `GetBankGuaranteeDetailsById` | `(ByVal v_lBankGuaranteeId As Integer, ByRef r_vDetails(,) As Object) As Integer` | Load guarantee by specific ID |
| `GetAttachedPolicies` | `(ByVal v_lBankGuaranteeId As Integer, ByRef r_vPolicies(,) As Object) As Integer` | Get policies covered by this guarantee |
| `GetAttachedBranches` | `(ByVal v_lBankGuaranteeId As Integer, ByRef r_vBranches(,) As Object) As Integer` | Get branches linked to guarantee |
| `GetAttachedProducts` | `(ByVal v_lBankGuaranteeId As Integer, ByRef r_vProducts(,) As Object) As Integer` | Get products linked to guarantee |
| `GetValidBGsforParty` | `(ByVal v_lPartyCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Find valid guarantees for a party |
| `GetValidBGsOnPolicy` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Find valid guarantees on a policy |
| `UpdateBGForPolicy` | `(ByVal v_lInsuranceFileCnt As Integer, v_lBankGuaranteeId As Integer) As Integer` | Link guarantee to a policy |
| `GetPolicyBGForReceipt` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Get guarantees applicable for receipt processing |
| `AddBankGuaranteeHistory` | `(ByVal v_lBankGuaranteeId As Integer, v_sAction As String) As Integer` | Record guarantee change history |
| `GetBGHistory` | `(ByVal v_lBankGuaranteeId As Integer, ByRef r_vHistory(,) As Object) As Integer` | Get full history for a guarantee |
| `PickListLoad` | `(ByVal sPickListType As String, ByVal vFKArray(,) As Object, ByRef vResultArray(,) As Object) As Integer` | Load pick list data (branches, products) |
| `PickListSave` | `(ByRef sPickListType As String, ByRef vFKArray(,), ByRef vKeys As Object) As Integer` | Save pick list selections |

**Search Methods — `bSIRFindBankGuarantee`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `SearchByQuery` | `(ByRef r_vResultArray(,) As Object, Optional v_vPartyCnt, v_vReference, v_vStatus As Object) As Integer` | Search guarantees by party/reference/status |
| `GetBGDetailsByID` | `(ByVal v_lBankGuaranteeId As Integer, ByRef r_vDetails(,) As Object) As Integer` | Get BG details by ID |
| `GetPoliciesOnBG` | `(ByVal v_lBankGuaranteeId As Integer, ByRef r_vPolicies(,) As Object) As Integer` | Get policies on a guarantee |

**Stored Procedures (29):**
`spu_bank_guarantee_sel`, `spu_BankGuarantee_Details_ByID`, `spu_BankGuarantee_Details_DelUndel`, `spu_BankGuarantee_History_Add`, `spu_BankGuarantee_History_Sel`, `spu_BankGuarantee_PLDBranch`, `spu_BankGuarantee_PLDProduct`, `spu_BankGuarantee_PLL`, `spu_BankGuarantee_PLS`, `spu_BankGuarantee_PLSBranch`, `spu_BankGuarantee_PLSProduct`, `spu_get_BGs_for_policy`, `spu_get_cashlistitem_for_bg`, `spu_get_policies_on_BG_for_id`, `spu_get_policies_on_BG_for_receipt`, `spu_party_bg_details_sel`, `spu_partyBG_Branches_Sel`, `spu_PartyBG_Details_Add`, `spu_PartyBG_Details_Sel`, `spu_PartyBG_Details_Upd`, `spu_partyBG_Products_Sel`, `spu_PolicyBG_Details_Add`, `spu_SAM_Get_Party_ShortName`, `spu_SIR_Get_Lookup_Values_By_Effective_Date`, `spu_Update_BG_Status`, `spu_update_cashlistitem_for_bg`, `spe_PFScheme_PLD`, `spu_ACT_GetSystemCurrency`, `spu_ACT_Select_Bank`

**References:** `bSIREvent` (event logging), `bSIRPartyBank` (bank details), `bPMLookup` (lookup values)

---

### Batch Scheduler
**Directory:** `Batch Scheduler/`
**COM Name:** `bSIRBatchScheduler`
**Purpose:** Manages scheduled batch processes (renewal runs, report generation, data exports). Provides CRUD for scheduled batch jobs and their parameters.

**Business Methods:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard initialisation |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Sets process modes |
| `GetBatchProcesses` | `(ByRef r_vBatchProcesses(,) As Object) As Integer` | List all available batch process types |
| `GetScheduledBatchProcesses` | `(ByRef r_vScheduled(,) As Object) As Integer` | Get currently scheduled processes |
| `GetProcessParameters` | `(ByVal v_lProcessId As Integer, ByRef r_vParams(,) As Object) As Integer` | Get parameters for a batch process |
| `AddBatchProcessSchedulerDetail` | `(ByVal v_vParams(,) As Object) As Integer` | Add a new scheduled process |
| `DeleteBatchProcessSchedulerDetail` | `(ByVal v_lScheduledId As Integer) As Integer` | Remove a scheduled process |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|--------|
| `spu_sir_batchprocesses_sel` | `GetBatchProcesses` | Get available batch process list |
| `spu_sir_scheduled_batchprocesses_sel` | `GetScheduledBatchProcesses` | Get scheduled processes |
| `spu_sir_batchprocess_parameter_sel` | `GetProcessParameters` | Get process parameters |
| `spu_sir_scheduled_batchprocesses_delete` | `DeleteBatchProcessSchedulerDetail` | Delete scheduled process |

---

### Cash Deposit
**Directory:** `Cash Deposit/`
**COM Name:** `bSIRCashDeposit`
**Purpose:** Manages client cash deposits — creation, linking to policies, balance tracking, refunds, and allocation. Supports multi-policy and multi-branch deposits.

**Business Methods — `bSIRCashDeposit`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard; creates bACTAccount, bACTExplorer, bACTLedger, bSIREvent |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `AddCashDeposit` | `(ByRef r_lCashDepositId As Integer, ByVal v_sCashDeposit_Ref As String, v_lAccount_ID As Integer, v_lParty_ID As Integer, v_sPartyName As String, v_sPartyType As String, v_iIs_SinglePolicy As Integer, v_lUser_ID As Integer, ByRef v_vBranches(,) As Object) As Integer` | Create a new cash deposit — returns new deposit ID |
| `UpdateCashDeposit` | `(ByVal v_lCashDepositId As Integer, v_iIsSinglePolicy As Integer, ByRef v_vBranches(,) As Object) As Integer` | Modify deposit details |
| `GetCashDepositDetails` | `(ByRef r_vCashDepositDetails(,) As Object) As Integer` | Load deposit by key — returns 2D array |
| `GetBalanceForCD` | `(ByVal v_lCashDepositId As Integer, v_dtCoverStartDate As Date, v_dtPolicyIssueDate As Date, ByRef v_vPrePayment As Object, ByRef v_crAvaliableBalance As Decimal, ByRef v_crRunningBalance As Decimal) As Integer` | Get remaining and running balance |
| `GetCDsForPolicy` | `(ByVal v_lPartyCnt As Integer, v_lProductId As Integer, v_lSourceID As Integer, v_crTotalPremium As Decimal, v_lInsuranceFileCnt As Integer, ByRef v_vPrePayment As Object, v_dtCoverStartDate As Date, v_dtPolicyIssueDate As Date, ByRef r_vCashDepositDetails(,) As Object) As Integer` | Get deposits linked to a policy |
| `GetLinkedCashDepositAccounts` | `(ByRef r_vGetCashDepositAccounts(,) As Object) As Integer` | Get linked deposit accounts |
| `GetCDReceiptsForAllocation` | `(ByVal v_lCashDepositId As Integer, v_crTotalPremium As Decimal, ByRef v_vPrePayment As Object, v_dtCoverStartDate As Date, v_dtPolicyIssueDate As Date, ByRef r_vReceiptDetails(,) As Object) As Integer` | Get receipts available for allocation |
| `GetCDRecieptsForRefund` | `(ByVal v_lCashDepositId As Integer, v_crTotalPremium As Decimal, ByRef r_vReceiptDetails As Object) As Integer` | Get receipts available for refund |
| `GetNextCashDepositNumber` | `(ByVal v_lPartyID As Integer, ByRef r_lCashDepositNumber As Integer) As Integer` | Generate next deposit reference number |
| `ConvertPolicyAmountToBaseCurrency` | `(ByVal v_lInsuranceFileCnt As Integer, v_crPolicyAmount As Decimal, ByRef v_crBaseAmount As Decimal, ByRef v_lBaseCurrencyID As Integer, ByRef v_sBaseCurrencyCode As String, ByRef v_lTransactionCurrencyID As Integer, ByRef v_sTransactionCurrencyCode As String) As Integer` | Convert policy amount between currencies |
| `CreateCDAccount` | `(ByRef r_lAccountId As Integer, ByVal v_sShortCode As String, v_sShortName As String, v_sPartyType As String, v_lPartyID As Integer) As Integer` | Create an Orion accounting entry for the deposit |
| `GetCDPaymentHistoryForPolicy` | `(ByVal v_lPartyCnt As Integer, v_lInsuranceFolderCnt As Integer, ByRef r_vCashDepositDetails(,) As Object) As Integer` | Get payment history |
| `GetPolicyDetailsForCashDeposit` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vPolicyDetails(,) As Object) As Integer` | Get policy details for a deposit |
| `CheckCDUsedForMultiPolicies` | `(ByVal v_lCashDepositId As Integer, ByRef r_bIsRepeated As Boolean) As Integer` | Check if deposit covers multiple policies |
| `LockKey` | `(ByVal v_sKeyName As String, v_lKeyValue As Integer, v_lUserID As Integer, ByRef r_sLockedBy As String) As Integer` | Lock a record |
| `UnLockKey` | `(ByVal v_sKeyName As String, v_lKeyValue As Integer, v_lUserID As Integer) As Integer` | Unlock a record |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_CashDeposit_Add` | `AddCashDeposit` | Create cash deposit |
| `spu_CashDeposit_Branch_Add` | `AddCashDeposit` | Add branch link |
| `spu_CashDeposit_Branch_Del` | `UpdateCashDeposit` | Delete branch link |
| `spu_CashDeposit_Branch_Sel` | `GetCashDepositDetails` | Get branch links |
| `spu_CashDeposit_PLS` | internal | PLS selection |
| `spu_CashDeposit_Product_Add` | `AddCashDeposit` | Add product link |
| `spu_CashDeposit_Product_Del` | `UpdateCashDeposit` | Delete product link |
| `spu_CashDeposit_Product_Sel` | `GetCashDepositDetails` | Get product links |
| `spu_CashDeposit_SinglePolicy_Upd` | `UpdateCashDeposit` | Update single policy flag |
| `spu_CheckCDUsedForMultiPolicy` | `CheckCDUsedForMultiPolicies` | Check multi-policy usage |
| `spu_Convert_Policy_Amount_To_Base_Currency` | `ConvertPolicyAmountToBaseCurrency` | Currency conversion |
| `spu_Get_Balance_For_CD` | `GetBalanceForCD` | Get balance |
| `spu_Get_Branches` | internal | Get branches |
| `spu_Get_CashDeposit_For_CD_Maintenance` | `GetCashDepositDetails` | Get deposit for maintenance |
| `spu_Get_CashDeposit_For_Policy` | `GetCDsForPolicy` | Get deposits for policy |
| `spu_Get_CDReceipts_For_Allocation` | `GetCDReceiptsForAllocation` | Get receipts for allocation |
| `spu_Get_CDReceipts_For_Refund` | `GetCDRecieptsForRefund` | Get receipts for refund |
| `spu_Get_Linked_CashDeposit_Accounts` | `GetLinkedCashDepositAccounts` | Get linked accounts |
| `spu_Get_Next_CashDeposit_Number` | `GetNextCashDepositNumber` | Generate next number |
| `spu_Get_Policy_CDPayment_History` | `GetCDPaymentHistoryForPolicy` | Get payment history |
| `spu_Get_PolicyDetails_For_CashDeposit` | `GetPolicyDetailsForCashDeposit` | Get policy details |
| `spu_Product_saa` | internal | Get products |
| `spe_Party_Public_Text_add` | internal | Add party text |
| `spe_Party_Public_Text_saa` | internal | Get all party text |
| `spe_Party_sel` | internal | Select party |
| `spe_PFScheme_PLD` | internal | PF scheme selection |

**References:** `bACTAccount`, `bACTExplorer`, `bACTLedger`, `bSIRBankGuaranteeSQL`, `bSIREvent`

---

### Contact
**Directory:** `Contact/`
**COM Name:** `bSIRContact`, `bSIRContactBusiness`
**Purpose:** Manages party contact details — phone numbers, email addresses, fax numbers. Supports CRUD and contact type lookups.

**Business Methods — `bSIRContact`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByRef sUserName As String, ..., Optional ByRef vDatabase As Object) As Integer` | Standard initialisation |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Sets process modes |
| `DirectAdd` | `(Optional ByRef vContactCnt, vContactTypeId, vPartyCnt, vDescription, vIntCode, vAreaCode, vNumber As Object) As Integer` | Create a new contact directly |
| `DirectDelete` | `(Optional ByRef vContactCnt As Object) As Integer` | Remove a contact |
| `CheckID` | `(ByRef vID As Object) As Integer` | Validate contact ID exists |
| `GetDetails` | `(Optional ByRef vLockMode As PMELockMode = 0, Optional ByRef vContactCnt As Object) As Integer` | Load contact details into collection |
| `GetNext` | `(Optional ByRef vContactCnt, vContactTypeId, vPartyCnt, vDescription, vIntCode, vAreaCode, vNumber As Object) As Integer` | Iterator — get next contact |
| `EditAdd` | `(ByRef lRow As Integer, Optional ByRef vContactCnt, vContactTypeId, vPartyCnt, vDescription, vIntCode, vAreaCode, vNumber As Object) As Integer` | Add via edit buffer |
| `EditUpdate` | `(ByRef lRow As Integer, Optional ByRef vContactCnt, vContactTypeId, vPartyCnt, vDescription, vIntCode, vAreaCode, vNumber As Object) As Integer` | Update via edit buffer |
| `EditDelete` | `(ByVal lRow As Integer) As Integer` | Mark for deletion |
| `Cancel` | `() As Integer` | Check unsaved changes |
| `Update` | `() As Integer` | Persist all changes to DB |
| `GetContactTypes` | `(ByRef r_vContactTypes(,) As Object) As Integer` | Get available contact types |
| `GetLookupValues` | `(ByRef iLookupType As Integer, ByRef vTableArray(,), ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer` | Get lookup values |
| `GetCorrespondenceTypes` | `(ByRef r_vTypes(,) As Object) As Integer` | Get correspondence types |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|--------|
| `spe_Contact_saa` | `GetDetails` | Select all contacts for party |
| `spe_Contact_sel` | `SelectItem` (data layer) | Select single contact |
| `spe_Contact_add` | `AddItem` (data layer) | Insert contact |
| `spe_Contact_del` | `DeleteItem` (data layer) | Delete contact |
| `spe_Contact_upd` | `UpdateItem` (data layer) | Update contact |
| `spe_SIRContact_check_id` | `CheckID` | Validate contact ID |

---

### Contact Type
**Directory:** `Contact Type/`
**COM Name:** `bSIRContactType` (via `Business.vb`)
**Purpose:** Maintains the contact type lookup table (e.g., Mobile, Home Phone, Work Email). Admin-level CRUD.

**Business Methods:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUserName As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard initialisation (implements IBusiness) |
| `Dispose` | `()` | Cleanup resources |
| `GetDetails` | `(ByRef r_vDetailArray As Object) As Integer` | Gets all contact type details |
| `UpdateDetails` | `(ByVal v_vArray(,) As Object) As Integer` | Updates contact type details (add/update/delete) |
| `GetCaptionID` | `(ByVal v_vDescription As Object, ByRef r_vCaptionID As Object) As Integer` | Returns caption ID for a description |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|--------|
| `spu_Contact_Type_add` | `UpdateDetails` | Add contact type |
| `spu_Contact_Type_update` | `UpdateDetails` | Update contact type |
| `spu_pm_caption_id_return` | `GetCaptionID` | Get/create caption ID |

---

### Conviction
**Directory:** `Conviction/`
**COM Name:** `bSIRPartyConviction`
**Purpose:** Manages party conviction records (motoring convictions, criminal records) used in motor insurance underwriting and risk assessment.

**Business Methods — `bSIRPartyConviction`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByRef sUserName As String, ..., Optional ByRef vDatabase As Object) As Integer` | Standard initialisation |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Sets process modes |
| `DirectAdd` | `(Optional ByRef vPartyCnt, vPartyConvictionId, vConvictionCode, vConvictionDate, vConvictionPoints, vConvictionBan, vConvictionFine As Object) As Integer` | Record a new conviction directly |
| `DirectDelete` | `(Optional ByRef vPartyCnt, vPartyConvictionId As Object) As Integer` | Remove a conviction record |
| `CheckID` | `(ByRef vID As Object) As Integer` | Validate conviction ID exists |
| `GetDetails` | `(Optional ByRef vLockMode As PMELockMode = 0, Optional ByRef vPartyCnt, vPartyConvictionId As Object) As Integer` | Load conviction details into collection |
| `GetNext` | `(Optional ByRef vPartyCnt, vPartyConvictionId, vConvictionCode, vConvictionDate, vConvictionPoints, vConvictionBan, vConvictionFine As Object) As Integer` | Iterator — get next conviction |
| `EditAdd` | `(ByRef lRow As Integer, Optional ByRef vPartyCnt, vPartyConvictionId, vConvictionCode, vConvictionDate, vConvictionPoints, vConvictionBan, vConvictionFine As Object) As Integer` | Add via edit buffer |
| `EditUpdate` | `(ByRef lRow As Integer, Optional params...) As Integer` | Update via edit buffer |
| `EditDelete` | `(ByVal lRow As Integer) As Integer` | Mark for deletion |
| `Cancel` | `() As Integer` | Check unsaved changes |
| `Update` | `() As Integer` | Persist all changes to DB |
| `GetLookupValues` | `(ByRef iLookupType As Integer, ...) As Integer` | Get lookup values |
| `GetSummary` | `(ByRef vSummaryArray As Object) As Integer` | Get conviction summary for display |

**Automated Class — `bSIRPartyConvictionAutomated`:**
| Method | Description |
|--------|-------------|
| `Initialise` | Navigator-style init |
| `GetSummary` | Returns summary array |
| `SetProcessModes` / `SetKeys` / `GetKeys` | Navigator key management |
| `Start` | Executes automated action |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|--------|
| `spe_party_conviction_saa` | `GetDetails` | Select all convictions for party |
| `spe_party_conviction_sel` | `SelectItem` (data layer) | Select single conviction |
| `spe_party_conviction_add` | `AddItem` (data layer) | Insert conviction |
| `spe_party_conviction_del` | `DeleteItem` (data layer) | Delete conviction |
| `spe_party_conviction_upd` | `UpdateItem` (data layer) | Update conviction |
| `spe_SIRPartyConviction_check_id` | `CheckID` | Validate conviction ID |

---

### Crystal Reports
**Directory:** `Crystal Reports/`
**COM Name:** `bSIRReportPrint`
**Purpose:** Report generation engine using Crystal Reports. Handles report parameter collection, data retrieval, rendering, PDF export, and email delivery.

**Business Methods — `bSIRReportPrint`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard initialisation |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Sets process modes |
| `GetReportsList` | `(ByRef r_vReportsList(,) As Object) As Integer` | Get all available reports |
| `GetLimitedReportList` | `(ByRef r_vReportsList(,) As Object) As Integer` | Get reports filtered by user access and report groups |
| `GetReportsFilterByReportGroupCode` | `(ByVal v_sGroupCode As String, ByRef r_vReportsList(,) As Object) As Integer` | Filter reports by report group code |
| `GetReportDetails` | `(ByVal v_lReportId As Integer, ByRef r_vDetails(,) As Object) As Integer` | Get report metadata and parameter definitions |
| `GetReportData` | `(ByVal v_lReportId As Integer, ByRef r_vData(,) As Object) As Integer` | Execute report query and get raw data |
| `GetReportDataSet` | `(ByVal v_lReportId As Integer, ByRef r_dsData As DataSet) As Integer` | Execute report query, returns DataSet |
| `GetParametersFromDB` | `(ByVal v_lReportId As Integer, ByRef r_vParams(,) As Object) As Integer` | Load saved report parameters from database |
| `ExportToDisk` | `(ByVal v_sOutputPath As String, ByVal v_sFormat As String) As Integer` | Export rendered report to file (PDF, Excel, CSV, etc.) |
| `SendToPrint` | `(ByVal v_sPrinterName As String) As Integer` | Send report directly to printer |
| `ReportExport` | `(ByVal v_sExportFormat As String, ByRef r_sOutputPath As String) As Integer` | Export report with format selection |
| `ConvertHTMLToPDFForCIL` | `(ByVal v_sHTMLContent As String, ByRef r_sPDFPath As String) As Integer` | Convert HTML content to PDF using CIL |
| `GetUserSources` | `(ByRef r_vSources(,) As Object) As Integer` | Get user's permitted sources/branches |
| `InsertReportUser` | `(ByVal v_lReportId As Integer, v_lUserID As Integer) As Integer` | Insert report user session record |
| `InsertReportGrouping` | `(ByVal v_lReportId As Integer, v_sGroupingType As String) As Integer` | Insert report grouping criteria |
| `InsertIncludedRisks` | `(ByVal v_lReportId As Integer, v_sRiskCodes As String) As Integer` | Insert included risk filters |
| `InsertExcludedRisks` | `(ByVal v_lReportId As Integer, v_sRiskCodes As String) As Integer` | Insert excluded risk filters |
| `InsertIncludedRiskGroups` | `(ByVal v_lReportId As Integer, v_sRiskGroupCodes As String) As Integer` | Insert included risk group filters |
| `InsertExcludedRiskGroups` | `(ByVal v_lReportId As Integer, v_sRiskGroupCodes As String) As Integer` | Insert excluded risk group filters |
| `InsertExcludedType` | `(ByVal v_lReportId As Integer, v_sTypeCodes As String) As Integer` | Insert excluded type filters |
| `DeleteTempReportRecords` | `(ByVal v_lSessionId As Integer) As Integer` | Clean up temporary report session data |
| `DeleteTempReportExcludeRecords` | `(ByVal v_lSessionId As Integer) As Integer` | Clean up temporary exclusion records |
| `AllocSessionId` | `() As Integer` | Allocate a unique report session ID |
| `FreeSessionId` | `(ByVal v_lSessionId As Integer) As Integer` | Release a report session ID |
| `GetPartyName` | `(ByVal v_lPartyCnt As Integer, ByRef r_sPartyName As String) As Integer` | Get party name for report header |
| `GetTPVisibility` | `(ByRef r_bVisible As Boolean) As Integer` | Check third-party report visibility |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_get_reports_limit_by_user` | `GetLimitedReportList` | Get reports filtered by user |
| `spu_get_reports_limit_by_report_group` | `GetReportsFilterByReportGroupCode` | Get reports by group code |
| `spu_pm_get_user_sources` | `GetUserSources` | Get user source permissions |
| `spu_pm_session_id_alloc` | `AllocSessionId` | Allocate session ID |
| `spu_pm_session_id_free` | `FreeSessionId` | Free session ID |
| `spu_insert_report_user` | `InsertReportUser` | Insert report user |
| `spu_insert_report_grouping` | `InsertReportGrouping` | Insert grouping |
| `spu_insert_included_risks` | `InsertIncludedRisks` | Insert included risks |
| `spu_insert_excluded_risks` | `InsertExcludedRisks` | Insert excluded risks |
| `spu_insert_included_risk_groups` | `InsertIncludedRiskGroups` | Insert included risk groups |
| `spu_insert_excluded_risk_groups` | `InsertExcludedRiskGroups` | Insert excluded risk groups |
| `spu_insert_excluded_type` | `InsertExcludedType` | Insert excluded types |
| `spu_delete_temp_report_records` | `DeleteTempReportRecords` | Clean up session data |
| `spu_delete_temp_report_exclude_records` | `DeleteTempReportExcludeRecords` | Clean up exclusion data |
| `spu_Get_Party_Name` | `GetPartyName` | Get party display name |
| `spu_get_TP_visiblity` | `GetTPVisibility` | Check TP visibility |
| `spu_Report_Claim_Payments_Deductible_PLICO` | (report query) | Claims payment deductible report |

**References:** `bSIRMailshot`, `bPMLookup`

---

### Document Production
**Directory:** `Document Production/`
**COM Name:** `bSIRDocTemplate`, `bSIRDocSpooler`, `bSIRDocManagerWrapper`, `bCCMDocumentProduction`
**Purpose:** **Core document generation engine.** Manages document templates, mail merge fields, endorsement/wording clauses, document spooling, SharePoint integration, and PDF generation. This is one of the largest and most complex components.

**Business Methods (142):**
| Method | Description |
|--------|-------------|
| `GetDetails` / `GetNext` | Navigate document templates. |
| `GetTemplate` / `GetTemplateDetails` | Load template definition. |
| `GetFieldList` | Get merge fields for a template. |
| `DuplicateDocument` | Clone a document template. |
| `ArchiveDocument` | Archive a generated document. |
| `CreateEvent` | Create event/task for document. |
| `CreateSharePointOnlineFolders` | Set up SharePoint document library. |
| `CheckAndValidatePartyDocumentLibrary` | Validate party doc library. |
| `DeleteSharepointFiles` | Remove files from SharePoint. |
| `GetSubDocumentTemplates` | Get child/sub-templates. |
| `GetRiskClauses` / `GetRiskClauseInfo` | Get clauses for a risk (linked via product/risk type). |
| `GetEndorsementDetails` | Get endorsement wording details. |
| `GetFieldList` / `GetWPFieldDetails` | Get document merge field definitions. |
| `GetCoreFieldsForDataBackbone` | Get core data backbone fields for merge. |
| `GetSubDocumentTemplates` | Get child/sub-template list for compound documents. |
| `UpdatePrinted` / `UpdateArchived` | Increment print/archive counters on spooled documents. |
| `UpdateModified` | Update last-modified timestamp. |
| `SearchAll` | `(r_vResultArray(,))` — Find all templates matching criteria. |
| `GetDescription` | `(lDocumentSpoolerId, sDocumentSpoolerDescription)` — Get spool description. |
| `GetUsers` / `GetAccountHandlers` | Get user/handler lists for document routing. |
| `CreateEvent` | `(r_lEventCnt, v_lPartyCnt, v_vInsuranceFolderCnt, v_vInsuranceFileCnt, v_vClaimCnt, v_vDocumentCnt, ...)` — Create event/task for a generated document. |
| `CheckDuplicates` | `(v_lSourceID, v_lDocumentId, v_lDocumentTypeId, v_lSlotNumber, v_vRiskCodeId, v_vRiskGroupId, r_bDuplicates)` — Check for duplicate document entries. |
| `GetOption` | `(v_iOptionNumber, r_sOptionValue)` — Read system option value. |

**Sub-Components (11 business + 2 data projects):**
| Project | Key Methods | Purpose |
|---------|-------------|---------|
| `bSIRDocTemplate` | `GetDetails`, `GetNext`, `EditAdd`, `EditUpdate`, `GetLookupValues`, `GetSummary`, `SetKeys`, `GetKeys` | Document template CRUD, endorsement wordings, clause management |
| `bSIRDocSpooler` | `DirectAdd`, `GetDetails`, `GetNext`, `EditAdd`, `UpdatePrinted`, `UpdateArchived`, `CreateEvent`, `SearchAll`, `CheckDuplicates` | Document spool queue — print/email/archive management |
| `bSIRFieldManager` | `GetFieldList`, `GetClauseList`, `LoadFields`, `GetFieldValues`, `GetRiskClauses`, `GetAllClauses`, `GetAllTemplates`, `GetSubDocumentsList`, `GetDocumentCurrency`, `GetRisksForPolicy`, `GetPreviousLivePolicyDetails`, `GetCCMFieldDetailsWithSpecialsType` | Mail merge field resolution — loads data backbone fields, WP fields, clauses. Calls `spu_wp_get_fields`, `spu_get_stored_proc`, `spu_get_WPField_Details`, `spu_GetDocCurrency` |
| `bSIRFindDocTemplate` | `SearchByQuery`, `SearchByRiskType`, `SearchByProduct`, `GetAvailableTemplate`, `GetFutureDatedTemplate`, `LoadClauses`, `GetProcessTypesDocsSplitStatus` | Template search — find templates by risk type, product, or query. Calls `spu_get_document_template_saa`, `spu_get_futuredated_template`, `spu_Product_Linked_Clauses_Sel`, `spu_Risk_Type_Linked_Clauses_Sel` |
| `bSIRDocumentType` | `GetDetails`, `UpdateDetails`, `CheckDelete`, `GetCaptionID` | Document type lookup maintenance. Calls `spu_Document_Type_add`, `spu_Document_Type_update`, `spu_Document_Type_delete` |
| `bPMBDocManager` | `Start`, `PrintDoc`, `SaveDocumentAsHTML`, `ConvertHTMLToPDF`, `ConvertHTMLToDOC`, `GetDataForCCMDocTemplateFields`, `NumToWord`, `InsertBreakUsingSiriusDocumentUtility` | **Core document merge engine** — parses templates, substitutes tags, calls expression parser, generates PDF/HTML/DOC output |
| `bSIRDocManagerWrapper` | `Start` | Thin wrapper around `bPMBDocManager` for COM compatibility |
| `bCCMDocumentProduction` | `GetCoreFieldsForDataBackbone`, `GetWPFieldsForDataBackbone`, `GetCCMDocTemplate`, `GetCCMFieldsForFieldSet`, `RefreshCCMTemplates` | CCM field mapping — manages data backbone structures, field sets, and endorsement data structures. Calls `spu_get_CCM_*`, `spu_update_core_fieldsets`, `spu_Refresh_CCM_Templates` |
| `bSIRSharepoint` | `CheckAndValidatePartyDocumentLibrary`, `CreateAndUpdatePartyDocumentLibrary`, `ArchiveDocument`, `GetFileList`, `GenerateDefaultPath` | SharePoint on-premises integration. Calls `spu_SIR_Get_Party_Document_Library`, `spu_SIR_Upd_Party_Document_Library`, `spu_SIR_Get_Sharepoint_Tags` |
| `bSIRSharepointOnline` | `GetFileList`, `UploadFile365`, `CreateSharePointOnlineFolders`, `DeleteSharepointFiles`, `RenameQuoteFolderToPolicyFolder`, `ValidateSharepointOnlineURL`, `CreateContentType` | SharePoint Online / Office 365 integration — upload, folder creation, delete, rename |
| `bSIRSharePointOnlineValidate` | `ValidateSharePointURL` | Validates SharePoint Online URL configuration |
| `dSIRDocSpooler` | `Add`, `Update`, `Delete`, `SelectSingle`, `SetPropertiesFromDB` | Data layer for document spooler (calls `spe_document_spooler_*`) |
| `dSIRDocTemplate` | `Add`, `Update`, `Delete`, `SelectSingle`, `SetPropertiesFromDB` | Data layer for document templates (calls `spe_document_template_*`) |

**Stored Procedures (85):** *(largest SP footprint of any component)*
`spe_document_spooler_add`, `spe_document_spooler_del`, `spe_document_spooler_sel`, `spe_document_spooler_upd`, `spe_document_template_add`, `spe_document_template_check_id`, `spe_document_template_del`, `spe_document_template_saa`, `spe_document_template_sel`, `spe_document_template_upd`, `spu_get_CCM_Core_Fields_For_FieldSet`, `spu_get_CCM_Doc_Template`, `spu_get_CCM_Fields_For_FieldSet`, `spu_get_Document_Template_Details`, `spu_get_document_template_saa`, `spu_get_risk_clauses`, `spu_get_risk_clause_info`, `spu_Get_AllRisk_Clauses`, `spu_get_sub_document_template_list`, `spu_SIR_Get_Party_Document_Library`, `spu_SIR_Get_Sharepoint_Tags`, `spu_SIR_Background_Job_add`, `spu_Refresh_CCM_Templates`, and 62 more.

**References:** `bGISListManager`, `bSIRDocManagerWrapper`, `bSIRDocSpooler`, `bSIRDocTemplate`, `bSIREvent`, `bSIRFieldManager`, `bSIRFindDocTemplate`, `bSIROptions`, `bSIRParty`, `bSIRSharePointApi`, `bSIRSharePointOnline`, `bSIRSharePointValidate`

---

### Event
**Directory:** `Event/`
**COM Name:** `bSIREvent`
**Purpose:** **Manages work manager events/tasks** — creation, updates, deletion, event logging, subject lists, and note management. Events are the audit trail mechanism for all business operations.

**Business Methods — `bSIREvent`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByRef sUserName As String, ..., Optional ByRef vDatabase As Object) As Integer` | Standard initialisation |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Sets process modes |
| `DirectAdd` | `(Optional ByRef vPartyCnt, vInsuranceFolderCnt, vInsuranceFileCnt, vClaimCnt, vEventTypeId, vEventDate, vDescription, vUserID, ..., Optional ~30 more params for all event fields) As Integer` | Create a new event/task — accepts ~30+ optional parameters |
| `DirectDelete` | `(Optional ByRef vEventCnt As Object) As Integer` | Remove an event |
| `DirectUpdate` | `(Optional ByRef vEventCnt, vDescription, vNotes As Object) As Integer` | Update event without full edit cycle |
| `CheckID` | `(ByRef vID As Object) As Integer` | Validate event ID exists |
| `GetDetails` | `(Optional ByRef vLockMode As PMELockMode = 0, Optional ByRef vEventCnt As Object) As Integer` | Load event details into collection |
| `GetNext` | `(Optional ByRef vEventCnt, vPartyCnt, vInsuranceFolderCnt, vInsuranceFileCnt, vClaimCnt, vEventTypeId, vEventDate, vDescription, vUserID, ... As Object) As Integer` | Iterator — get next event in collection |
| `EditAdd` | `(ByRef lRow As Integer, Optional ByRef params... As Object) As Integer` | Add via edit buffer |
| `EditUpdate` | `(ByRef lRow As Integer, Optional ByRef params... As Object) As Integer` | Update via edit buffer |
| `EditDelete` | `(ByVal lRow As Integer) As Integer` | Mark for deletion |
| `Cancel` | `() As Integer` | Check unsaved changes |
| `Update` | `() As Integer` | Persist all changes with transaction |
| `GetEventTypeGroup` | `(ByRef r_vEventTypeGroup(,) As Object) As Integer` | Get event type configuration groups |
| `GetEventLogSubjectList` | `(ByVal v_lEventTypeId As Integer, ByRef r_vSubjectList(,) As Object) As Integer` | Get subject list for event type |
| `GetNoteEventType` | `(ByRef r_vNoteEventTypes(,) As Object) As Integer` | Get event types that support notes |
| `SearchAll` | `(ByRef r_vResultArray(,) As Object, Optional v_vPartyCnt, v_vInsuranceFileCnt, v_vClaimCnt, v_vEventTypeId, v_dtFromDate, v_dtToDate, v_vDescription, v_vDocumentCnt As Object) As Integer` | Search events with extensive filter criteria |
| `WriteTemplate` | `(ByVal v_lEventTypeId As Integer, v_lPartyCnt As Integer, ...) As Integer` | Write event from template definition |
| `CopyEventByDocumentCnt` | `(ByVal v_lDocumentCnt As Integer, ByRef r_lNewEventCnt As Integer) As Integer` | Copy event linked to document |
| `CloseWarnings` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Close warning events for a policy |
| `SetRisk` | `(ByVal v_lRiskId As Integer) As Integer` | Link event to risk |
| `SetRefNumber` | `(ByVal v_sRefNumber As String) As Integer` | Link event to policy reference |
| `SetParty` | `(ByVal v_lPartyCnt As Integer) As Integer` | Link event to party |
| `SetDocument` | `(ByVal v_lDocumentCnt As Integer) As Integer` | Link event to document |
| `GetSummary` | `(ByRef vSummaryArray As Object) As Integer` | Get event summary for display |
| `GetLookupValues` | `(ByRef iLookupType As Integer, ByRef vTableArray(,), ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer` | Get lookup values |
| `GetKeys` | `(ByRef vKeyArray(,) As Object) As Integer` | Get event key context (party, policy, claim) |
| `SetKeys` | `(ByRef vKeyArray(,) As Object) As Integer` | Set event key context |
| `SelectSingle` | `(Optional ByRef vLockMode As PMELockMode = 0) As Integer` | Load a single event by current key |

**Automated Class — `bSIREventAutomated`:**
| Method | Description |
|--------|-------------|
| `Initialise` | Navigator-style init |
| `GetSummary` | Returns summary array |
| `SetProcessModes` / `SetKeys` / `GetKeys` | Navigator key management |
| `Start` | Executes automated event action |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|---------|
| `spe_event_log_saa` | `GetDetails` | Select all events |
| `spe_event_log_sel` | `SelectSingle` (data layer) | Select single event |
| `spe_event_log_add` | `AddItem` (data layer) | Insert event |
| `spe_event_log_del` | `DeleteItem` (data layer) | Delete event |
| `spe_event_log_upd` | `UpdateItem` (data layer) | Update event |
| `spe_event_log_check_id` | `CheckID` | Validate event ID |
| `spe_event_log_copy_by_document_cnt` | `CopyEventByDocumentCnt` | Copy event by document |
| `spu_event_log_claim_policy_upd` | `DirectUpdate` | Update claim/policy event link |

**References:** `dSIREvent` (data layer)

---

### Export Control
**Directory:** `Export Control/`
**COM Name:** `bSIRExportControl`
**Purpose:** Handles data export mapping and control for external system integrations.

**Business Methods — `bSirExportControl`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object) As Integer` | Standard initialisation — sets up credentials, checks database, configures cache manager |
| `SetProcessModes` | `(Optional ByRef vTask, vNavigate, vProcessMode, vTransactionType, vEffectiveDate As Object) As Integer` | Sets process mode properties |
| `Cancel` | `() As Integer` | Checks if cancel is OK (always returns PMTrue) |
| `GetRecordFromMapping` | `(ByVal v_sModelCode As String, ByVal v_sDetailCode As String, ByVal v_sSourceTableName As String, ByRef r_sReturnString As String) As Integer` | Retrieves mapping structure record from DB; uses Enterprise Library caching with file dependency |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_sir_get_mapping_structure` | `GetRecordFromMapping` | Get export mapping structure |

**References:** Enterprise Library Caching, `SSP.Shared`

---

### ExternalWorkFlowConfiguration
**Directory:** `ExternalWorkFlowConfiguration/`
**COM Name:** `bSIRExternalWorkFlowConfiguration`
**Purpose:** Manages configuration for external workflow integrations — maps user groups to external workflow triggers and feature flags.

**Business Methods — `bSIRExternalWorkflowConfig`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard initialisation (implements IBusiness) |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Sets process modes |
| `GetExternalWorkflowConfigurationUserGroupInfo` | `(ByRef r_vUserGroupInfo(,) As Object) As Integer` | Gets all external workflow configuration user groups |
| `UpdateExternalWorkflowConfigurationUserGroupInfo` | `(ByRef r_lPMUserGroupId As Integer, ByRef r_iMode As Integer, Optional ByRef r_iIsSupervisor As Integer = 0) As Integer` | Add or delete user group (mode 0=delete, else=add) |
| `AddUpdateExternalWorkflowConfigurationUserGroupInfo` | `(ByVal o_lUserGroupID As Integer) As Integer` | Adds a user group to external workflow config |
| `DelUpdateExternalWorkflowConfigurationUserGroupInfo` | `(ByVal v_lUserGroupID As Integer) As Integer` | Deletes a user group from external workflow config |
| `UpdateExternalWorkflowConfigFlag` | `(ByVal o_bEnablebackgroundjob_ForFailure As Boolean, ByVal o_lExternalWorkFlowConfigID As Integer) As Integer` | Updates the background job failure flag |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|---------|
| `Spu_sir_external_workflow_usergroups_sel` | `GetExternalWorkflowConfigurationUserGroupInfo` | Select user groups |
| `Spu_sir_external_workflow_usergroups_upd` | `AddUpdateExternalWorkflowConfigurationUserGroupInfo` | Add user group |
| `Spu_sir_external_workflow_usergroups_Del` | `DelUpdateExternalWorkflowConfigurationUserGroupInfo` | Delete user group |
| `Spu_sir_external_workflow_config_upd` | `UpdateExternalWorkflowConfigFlag` | Update config flag |

**References:** `SSP.S4I.Interfaces.IBusiness`, `SharedFiles`

---

### Find Insurance
**Directory:** `Find Insurance/`
**COM Name:** `bSIRFindInsurance`, `bSIRInsuranceFileServices`
**Purpose:** **Core policy search and manipulation engine.** Finds policies by reference, index, holder name, vehicle registration. Also handles policy copy, MTA creation, risk management, renewal checks, and version history.

**Business Methods — `bSIRFindInsurance`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Initialises: database, GIS DataSet, GeminiII link, underwriting branch, registration search |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard process mode setter |
| `SearchByQuery` | `(ByRef r_vResultArray(,) As Object, Optional v_vInsuranceRef, v_vInsFileType, v_vShortName, v_vVehicleRegNo, v_bShowLapsedOnly, v_bLimitResults, v_bShowCurrentPolicyOnly, v_lNumberOfRecords, v_vAgentGroupCnt, v_bAgencyProductOnly, v_bShowCancelledForEvents, bRetrieveAssociated, v_vAgentCnt, v_vAgentKey As Object) As Integer` | Builds dynamic SQL to search insurance files by multiple criteria |
| `SearchOtherPolicies` | `(Optional v_vPartyCnt As Object, Optional ByRef r_vResultArray(,) As Object) As Integer` | Search policies where client is used as a risk |
| `SearchAll` | `(ByRef r_vResultArray(,) As Object, Optional v_vInsuranceRef, v_vInsFileType, v_vShortName, v_vPartyCnt, v_vUserInsurerCnt As Object) As Integer` | General search across agency/underwriting types |
| `SearchAllByType` | `(ByRef r_vResultArray(,), Optional v_lPartyCnt, v_sPolicyType, v_IFSTInsuranceFileType, v_bIncludeLapsedAndCancelled As Object) As Integer` | Search by insurance file type enum |
| `SearchAllByProductId` | `(ByRef r_vResultArray(,), Optional v_lPartyCnt, v_lProductId As Object) As Integer` | Search by product ID |
| `SearchAllPMUQuotes` | `(ByRef r_vResultArray(,), Optional v_lPartyCnt As Object) As Integer` | Search all editable PMU quotes |
| `SearchAllPMUMTAs` | `(ByRef r_vResultArray(,), Optional v_lPartyCnt As Object) As Integer` | Search all editable PMU MTAs |
| `FindLikeRef` | `(ByRef sInsuranceRef As String, ByRef lNumberOfRecords As Integer, ByRef vResultArray(,) As Object) As Integer` | Search by insurance reference (like match) |
| `FindSingleRef` | `(ByVal sInsuranceRef As String, ByRef vResultArray(,) As Object) As Integer` | Search for a single exact reference |
| `FindQuote` | `(ByRef r_vResultArray(,), Optional v_sQuoteRef, v_dtCoverStartDate, v_sInsuranceFolderDescription, v_lLeadAgentCnt As Object) As Integer` | Search quotes by reference/date/description/agent |
| `FindLikeRefAndHolder` | `(ByRef sInsuranceRef, lInsuranceHolderCnt, lNumberOfRecords As Integer, ByRef vResultArray(,) As Object) As Integer` | Search by ref + holder |
| `FindLikeVehicle` | `(ByRef sRegistration As String, ByRef vResultArray(,) As Object) As Integer` | Search by vehicle registration |
| `FindLikeIndex` | `(ByRef sIndex As String, ByRef lNumberOfRecords As Integer, ByRef vResultArray(,) As Object, Optional lSpecificDataModelIndex As Object) As Integer` | Search by GIS search index |
| `GetAllPolicyVersion` | `(ByRef r_vResultArray(,) As Object, ByVal v_lInsuranceFolderCnt As Integer, Optional v_lInsuranceFileCnt, v_lNonTempPolicies, v_lfilterBackdatedVersions, v_lViaClientManager As Object) As Integer` | Get all policy versions for a folder |
| `GetCurrentPolicyVersion` | `(ByRef r_lCurrentPolicyVersionInsFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, Optional v_lInsuranceFileCnt As Object) As Integer` | Get current active policy version |
| `GetVersionByDate` | `(ByRef r_lInsuranceFileCnt As Integer, ByVal v_dtStartDate As Date, ByRef r_lPolicyVersion As Integer, ByRef r_lErrorCode As Integer, Optional v_lInsuranceFolderCnt, r_lSubErrorCode As Object) As Integer` | Get effective policy version by date |
| `GetInsuranceFolder` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lInsuranceFolderCnt As Integer) As Integer` | Get folder CNT from file CNT |
| `CopyPolicy` | `(ByVal v_lOldInsuranceFileCnt As Integer, ByRef r_lNewInsuranceFileCnt As Integer, ...) As Integer` | Copy a policy version |
| `CopyPolicyV2` | `(ByVal v_lOldInsuranceFileCnt As Integer, ByRef r_lNewInsuranceFileCnt As Integer, v_iSourceID As Integer, v_lTargetPartyCnt As Integer, Optional v_lDontCopyTextFiles As Object) As Integer` | Copy policy V2 with source/target party parameters |
| `CopyRisk` | `(ByVal v_lOldInsuranceFileCnt As Integer, ByVal v_lNewInsuranceFileCnt As Integer) As Integer` | Copy risks from one policy to another |
| `CopyPolicyForEdit` | `(ByVal v_lOldInsuranceFileCnt As Integer, ByRef r_lNewInsuranceFileCnt As Integer) As Integer` | Copy policy for editing |
| `CopyPolicyStandardWordings` | `(ByVal v_lOldInsuranceFileCnt As Integer, ByVal v_lNewInsuranceFileCnt As Integer) As Integer` | Copy standard wordings |
| `CopyRiskStandardWordings` | `(ByVal v_lOldPolicyBinderId As Integer, v_lNewPolicyBinderId As Integer, v_sDataModelCode As String) As Integer` | Copy risk standard wordings |
| `CreateRisk` | `(ByRef vRiskDetails(,) As Object, ByRef vRiskTypeArray(,) As Object) As Integer` | Create new risk |
| `GetRisk` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Get risk details for a policy |
| `UpdateRisk` | `(ByRef vRiskArray(,) As Object) As Integer` | Update risk details |
| `UpdateRiskStatus` | `(ByVal v_lRiskId As Integer, v_lStatus As Integer) As Integer` | Change risk status |
| `UpdateRiskType` | `(ByVal v_lRiskId As Integer, v_sRiskType As String) As Integer` | Change risk type |
| `CheckInRenewal` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lRenewalStatus As Integer) As Integer` | Check if policy is in renewal |
| `UpdateRenewalStatus` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Update renewal status |
| `CheckIsMarketplacePolicy` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_bIsMarketplace As Boolean) As Integer` | Check marketplace flag |
| `UpdateMarketplacePolicyStatus` | `(ByVal v_lInsuranceFileCnt As Integer, v_lStatus As Integer) As Integer` | Update marketplace status flag |
| `CopyPolicyAssociates` | `(ByVal v_lOldInsuranceFileCnt As Integer, v_lNewInsuranceFileCnt As Integer) As Integer` | Copy associates |
| `CreateMTAInsuranceFileLink` | `(ByVal v_lOrigInsuranceFileCnt As Integer, v_lMTAInsuranceFileCnt As Integer) As Integer` | Add MTA link |
| `SIR_IsMarkedForCollection` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_bMarked As Boolean) As Integer` | Check collection marking |
| `SIR_Update_Marked_For_Collection` | `(ByVal v_lInsuranceFileCnt As Integer, v_bMarked As Boolean) As Integer` | Update collection marking |
| `GetPartyType` | `(ByVal v_lPartyCnt As Integer, ByRef r_sPartyType As String) As Integer` | Get party type |
| `GetOption` | `(ByVal v_iOptionNumber As Integer, ByRef r_nOptionValue As Integer) As Integer` | Get system option value |
| `GIS_LoadFromDB` | `(ByVal v_sGisDataModelCode As String, Optional r_vInsuranceFileCnt, r_vPolicyLinkID, r_vRiskID As Object) As Integer` | Load GIS data from database |
| `GIS_SaveToDB` | `(ByVal v_sGisDataModelCode As String) As Integer` | Save GIS data to database |
| `GetAllGISSearchResults` | `(ByRef sSearchStr As String, ByRef lNoOfRecords As Integer, ...) As Integer` | GIS property search |
| `GetAllPolicyByGISSearchIndex` | `(ByRef vInputData(,) As Object, ByRef vOutputData(,) As Object) As Integer` | Find policies by GIS search index |
| `GetPolicyBinderId` | `(ByVal v_sDataModelCode As String, v_lGISPolicyLinkId As Integer, ByRef r_lPolicyBinderId As Integer) As Integer` | Get policy binder ID |
| `GetAllTransactionDates` | `(ByVal v_lInsuranceFolderCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Get all transaction dates |
| `CheckDataModelCompatibility` | `(ByVal v_sSourceDataModelCode As String, v_sTargetDataModelCode As String, ByRef r_bCompatible As Boolean) As Integer` | Verify data model compatibility |
| `GetUnderwritingVersionByDate` | `(ByVal v_dtTransactionDate As Date, ByRef r_lInsuranceFileCnt As Integer) As Integer` | Get policy version as of a date |

**Stored Procedures (51):**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_findins_like_ref` | `FindLikeRef` | Search by reference (LIKE) |
| `spu_findins_like_ref_and_holder` | `FindLikeRefAndHolder` | Search by ref + holder |
| `spu_findins_like_index` | `FindLikeIndex` | Search by GIS index |
| `spu_findins_like_vehicle` | `FindLikeVehicle` | Search by vehicle |
| `spu_Get_All_Policy_Version` | `GetAllPolicyVersion` | Get all versions |
| `spu_Get_All_Non_Temp_Policy_Version` | `GetAllPolicyVersion` | Get non-temp versions |
| `spu_SIR_Get_Current_Policy_Version` | `GetCurrentPolicyVersion` | Get current version |
| `spu_Get_All_Policy_Transaction_Dates` | `GetAllTransactionDates` | Get transaction dates |
| `spu_check_in_renewal` | `CheckInRenewal` | Check renewal status |
| `spu_update_renewal_status` | `UpdateRenewalStatus` | Update renewal status |
| `spu_pmb_copy_policy` | `CopyPolicy` | Copy policy record |
| `spu_copy_coinsurance` | `CopyPolicy` | Copy coinsurance data |
| `spu_copy_sub_agent` | `CopyPolicy` | Copy sub-agent data |
| `spu_copy_policy_standard_wordings` | `CopyPolicyStandardWordings` | Copy standard wordings |
| `spu_Copy_RISK_Standard_Wording` | `CopyRiskStandardWordings` | Copy risk wordings |
| `spu_copy_insurance_file_risk_link` | `CopyRisk` | Copy risk links |
| `spe_Risk_add` | `CreateRisk` | Add risk record |
| `spe_Risk_Folder_add` | `CreateRisk` | Add risk folder |
| `spe_Risk_sel` | `GetRisk` | Select risk |
| `spe_Insurance_File_Risk_Li_add` | `CreateRisk` | Add insurance file risk link |
| `spu_SIR_Update_Risk_Status` | `UpdateRiskStatus` | Update risk status |
| `spu_SIR_Update_Risk_Type` | `UpdateRiskType` | Update risk type |
| `spu_SIR_Get_InsuranceFileStatus` | internal | Get ins file status |
| `spu_SIR_IsMarkedForCollection` | `SIR_IsMarkedForCollection` | Check collection marking |
| `spu_SIR_Update_Marked_For_Collection` | `SIR_Update_Marked_For_Collection` | Update collection marking |
| `spu_CheckIsMarketplacePolicy` | `CheckIsMarketplacePolicy` | Check marketplace flag |
| `spu_Update_MarketplacePolicy_Status` | `UpdateMarketplacePolicyStatus` | Update marketplace status |
| `spu_SIR_copy_insurance_file_associates` | `CopyPolicyAssociates` | Copy associates |
| `spu_SIR_mta_insurance_file_link_add` | `CreateMTAInsuranceFileLink` | Add MTA link |
| `spu_SAM_Copy_Quote_Without_Versioning` | `CopyQuote` | Copy quote |
| `spu_Get_DataModel_Code_For_Risk` | `CheckDataModelCompatibility` | Get data model code |
| `spu_GetBasePolicyCntForBackDateMTA` | internal | Get base policy for backdate MTA |
| `spu_Get_Insurance_Ref` | internal | Get insurance reference |
| `spu_Get_Quotes_Marked_ForCollection` | internal | Get quotes marked for collection |
| `spu_SIR_MTA_Link_Sel` | internal | Select MTA link |
| `Spu_GetPolicyCancellationDate` | `GetCancellationDate` | Get cancellation date |
| `spu_Get_Party_Type_For_Party` | `GetPartyType` | Get party type |
| `sp_gis_policy_link_sel` | `GetGISPolicyLink` | Get GIS policy link |

**References:** `bSIRInsuranceFile`, `bSIRIUSLink`, `bSIROptions`, `bSIRRenSelection`, `bSirToGemVehicle`, `cGISDataSetControl`, `bUnderwritingBranchFunc`

---

### Find Party
**Directory:** `Find Party/`
**COM Name:** `bSIRFindParty`, `bSIRFindPartyServices`
**Purpose:** **Party/client search engine.** Searches by name, shortname, ID. Also provides party metadata — type, agent details, blacklisting, FSA questions, account balances, and claim summaries.

**Business Methods — `bSIRFindParty`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Initialise with lookup, underwriting branch detection, multi-tree accounting |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `SearchByQuery` | `(ByRef r_vResultArray(,) As Object, ByRef r_lNumberOfRecords As Integer, Optional v_vShortName, v_vName, v_vFileCode, v_vClientType, v_vStatusType, v_vAddress1, v_vPostalCode, v_vAreaCode, v_vNumber As Object, ...) As Integer` | Dynamic SQL search for parties with many criteria (builds SQL via CSelectANSI helper) |
| `SearchAgent` | `(ByRef r_vResultArray(,) As Object, Optional vShortname, vname, vpartyAgentDesc, vCurrCode, vSubBraDesc, vIsGrossAgent As Object) As Long` | Search for agent parties |
| `SearchSpecialPartyByQuery` | `(ByRef r_vResultArray(,) As Object, ByRef r_lNumberOfRecords As Integer, ...) As Integer` | Search special party types (insurers, reinsurers) |
| `DeleteParty` | `() As Integer` | Soft-delete party (sets is_deleted = 1) |
| `UndeleteParty` | `() As Integer` | Restore party (sets is_deleted = 0) |
| `GetPartyType` | `(ByRef lPartyCnt As Integer, ByRef sPartyTypeText As String) As Integer` | Get party type description |
| `GetID` | `(ByRef lID As Integer, Optional vName, vShortName, vSourceId As Object) As Integer` | Get party CNT from name/shortname |
| `GetName` | `(ByRef lPartyCnt As Integer, ByRef sPartyName As String) As Integer` | Get party name from CNT |
| `GetResolvedName` | `(ByRef lPartyCnt As Integer, ByRef sPartyResolvedName As String) As Integer` | Get resolved party name |
| `GetFullAddress` | `(ByVal v_lPartyCnt As Integer, ByRef r_vAddress1, r_vAddress2, r_vAddress3, r_vAddress4 As String, ByRef r_vPostalCode As String, Optional ByRef r_vCountryID As Object) As Integer` | Get full address for party |
| `GetMultipleAddresses` | `(ByVal v_lPartyCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Get all addresses for a party |
| `GetAccountBalance` | `(ByRef lPartyCnt As Integer, Optional ByRef dtResult As DataTable) As Integer` | Get account balance for party |
| `GetClaimIncurred` | `(ByRef lPartyCnt As Integer, Optional ByRef dtResult As DataTable) As Integer` | Get total claims incurred |
| `GetDocumentLibraryFromDB` | `(ByVal v_lParty_Cnt As Integer, ByRef r_lDocument_Library As Integer, Optional v_lPartyShortName As Object) As Integer` | Get SharePoint document library path |
| `CheckAgencyAgreement` | `(ByVal v_lPartyCnt As Integer, ByRef r_bIsAgent As Boolean, ByRef r_bIsAgencyAgreementValid As Boolean) As Integer` | Check if party has valid agency agreement |
| `CheckAssociatedClients` | `(ByVal v_lPartyCnt As Integer, ByRef r_lAssociatedClientCount As Integer) As Integer` | Count associated clients |
| `CheckOtherPartyBranchRecords` | `() As Boolean` | Check if other_party_branch records exist |
| `GetClientBlackListingReason` | `(ByVal v_lPartyCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Get client blacklisting reason |
| `GetCommissionFlag` | `(ByRef sShortName As String, ByRef i_rAllow_Commission_Flag As Integer) As Integer` | Check if sub-agent allows consolidated commission |
| `GetPartyAgentType` | `(ByVal v_lPartyCnt As Integer, ByRef r_sPartyAgentType As String) As Integer` | Get agent type for a party |
| `GetAssociatedSubAgent` | `(ByVal v_lLeadPartyCnt As Integer, ByRef r_vGetAssociatedSubAgent(,) As Object) As Integer` | Get associated sub-agents |
| `GetAgentUserDetails` | `(ByVal v_lPartyCnt As Integer, ByRef r_vGetAgentUserDetails(,) As Object) As Integer` | Get agent user login details |
| `CheckAgentReceiveCorrespondenceFlag` | `(ByVal v_lPartyCnt As Integer, ByRef r_bCorrespondenceType As Boolean) As Boolean` | Check agent correspondence flag |
| `GetFSAPartyViewReasons` | `(ByRef r_vResultArray(,) As Object, Optional r_iIsComplaint As Object) As Integer` | Get FSA party view reasons |
| `GetFSAPartyQuestion` | `(ByVal lPartyCnt As Integer, sPartyType As String, ByRef r_sPassword As String, r_vPostcode As String) As Integer` | Get FSA security question |
| `UpdateFSAPartyPassword` | `(ByVal lPartyCnt As Integer, sPartyType As String, sPassword As String) As Integer` | Update FSA party password |
| `CreateEvent` | `(ByRef r_lEventCnt As Integer, ByVal v_lPartyCnt As Integer, v_vInsuranceFolderCnt, v_vInsuranceFileCnt, v_vClaimCnt, v_vDocumentCnt As Object, ..., v_lEventTypeId As Integer, v_dtEventDate As Date, v_vDescription As Object) As Integer` | Create a party event |
| `GetOtherPartyTypes` | `(ByRef r_vResultArray(,) As Object) As Integer` | Get other party types |
| `GetInsurerType` | `(ByRef r_vResultArray(,) As Object) As Integer` | Get insurer types |
| `GetLookupValues` | `(ByRef iLookupType As Integer, ...) As Integer` | Get lookup values |
| `GetVisibleAgentTypes` | `(ByRef r_vVisibleAgentTypes As Object) As Integer` | Get visible agent types |
| `GetPartyTypeByCode` | `(ByVal v_sCode As String, ByRef r_vResults(,) As Object) As Integer` | Get party type by code |
| `IsUserSystemAdministrator` | `(ByRef r_bIsSystemAdministrator As Boolean) As Integer` | Check if user is sysadmin |
| `CheckInsurerAccess` | `(ByRef r_bHasAccess As Boolean) As Integer` | Check insurer access rights |
| `calccombinedkey` | `(ByVal v_lSourceID As Integer, v_lKeyID As Integer, ByRef r_lCombinedKeyID As Integer) As Integer` | Calculate combined key from source+key |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_FindParty_like_shortname` | `SearchByQuery` | Search by shortname |
| `spu_select_id_from_shortname` | `GetID` | Get party from shortname |
| `spu_select_id_from_name` | `GetID` | Get party from name |
| `spu_select_name_from_id` | `GetName` | Get name from CNT |
| `spu_SAM_ACT_Select_AccountBal` | `GetAccountBalance` | Get account balance |
| `spu_Select_Incurred_on_all_claims` | `GetClaimIncurred` | Get claim incurred total |
| `spu_select_allow_commission_from_shortname` | `GetCommissionFlag` | Check commission flag |
| `spu_select_associated_client_count` | `CheckAssociatedClients` | Count associated clients |
| `spu_get_visible_agent_types` | `GetVisibleAgentTypes` | Get visible agent types |
| `spu_SIR_Get_Party_Type_By_Code` | `GetPartyTypeByCode` | Get party type by code |
| `spu_SIR_Party_BlackList_Reason_Sel` | `GetClientBlackListingReason` | Get blacklist reason |
| `spu_pmuser_insurer_access` | `CheckInsurerAccess` | Check insurer access |
| `spu_Get_Insurer_Type` | `GetInsurerType` | Get insurer types |

**References:** `bSIREvent` (event logging), `bPMLookup` (lookups), `bUnderwritingBranchFunc` (branch detection)ultArray(,) As Object) As Integer` | Get all addresses for a party |
| `GetAccountBalance` | `(ByRef lPartyCnt As Integer, Optional ByRef dtResult As DataTable) As Integer` | Get account balance for party |
| `GetClaimIncurred` | `(ByRef lPartyCnt As Integer, Optional ByRef dtResult As DataTable) As Integer` | Get total claims incurred |
| `GetDocumentLibraryFromDB` | `(ByVal v_lParty_Cnt As Integer, ByRef r_lDocument_Library As Integer, Optional v_lPartyShortName As Object) As Integer` | Get SharePoint document library path |
| `CheckAgencyAgreement` | `(ByVal v_lPartyCnt As Integer, ByRef r_bIsAgent As Boolean, ByRef r_bIsAgencyAgreementValid As Boolean) As Integer` | Check if party has valid agency agreement |
| `CheckAssociatedClients` | `(ByVal v_lPartyCnt As Integer, ByRef r_lAssociatedClientCount As Integer) As Integer` | Count associated clients |
| `CheckOtherPartyBranchRecords` | `() As Boolean` | Check if other_party_branch records exist |
| `GetClientBlackListingReason` | `(ByVal v_lPartyCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Get client blacklisting reason |
| `GetCommissionFlag` | `(ByRef sShortName As String, ByRef i_rAllow_Commission_Flag As Integer) As Integer` | Check if sub-agent allows consolidated commission |
| `GetPartyAgentType` | `(ByVal v_lPartyCnt As Integer, ByRef r_sPartyAgentType As String) As Integer` | Get agent type for a party |
| `GetAssociatedSubAgent` | `(ByVal v_lLeadPartyCnt As Integer, ByRef r_vGetAssociatedSubAgent(,) As Object) As Integer` | Get associated sub-agents |
| `GetAgentUserDetails` | `(ByVal v_lPartyCnt As Integer, ByRef r_vGetAgentUserDetails(,) As Object) As Integer` | Get agent user login details |
| `CheckAgentReceiveCorrespondenceFlag` | `(ByVal v_lPartyCnt As Integer, ByRef r_bCorrespondenceType As Boolean) As Boolean` | Check agent correspondence flag |
| `GetFSAPartyViewReasons` | `(ByRef r_vResultArray(,) As Object, Optional r_iIsComplaint As Object) As Integer` | Get FSA party view reasons |
| `GetFSAPartyQuestion` | `(ByVal lPartyCnt As Integer, sPartyType As String, ByRef r_sPassword As String, r_vPostcode As String) As Integer` | Get FSA security question |
| `UpdateFSAPartyPassword` | `(ByVal lPartyCnt As Integer, sPartyType As String, sPassword As String) As Integer` | Update FSA party password |
| `CreateEvent` | `(ByRef r_lEventCnt As Integer, ByVal v_lPartyCnt As Integer, v_vInsuranceFolderCnt, v_vInsuranceFileCnt, v_vClaimCnt, v_vDocumentCnt As Object, ..., v_lEventTypeId As Integer, v_dtEventDate As Date, v_vDescription As Object) As Integer` | Create a party event |
| `GetOtherPartyTypes` | `(ByRef r_vResultArray(,) As Object) As Integer` | Get other party types |
| `GetInsurerType` | `(ByRef r_vResultArray(,) As Object) As Integer` | Get insurer types |
| `GetLookupValues` | `(ByRef iLookupType As Integer, ...) As Integer` | Get lookup values |
| `GetVisibleAgentTypes` | `(ByRef r_vVisibleAgentTypes As Object) As Integer` | Get visible agent types |
| `GetPartyTypeByCode` | `(ByVal v_sCode As String, ByRef r_vResults(,) As Object) As Integer` | Get party type by code |
| `IsUserSystemAdministrator` | `(ByRef r_bIsSystemAdministrator As Boolean) As Integer` | Check if user is sysadmin |
| `CheckInsurerAccess` | `(ByRef r_bHasAccess As Boolean) As Integer` | Check insurer access rights |
| `calccombinedkey` | `(ByVal v_lSourceID As Integer, v_lKeyID As Integer, ByRef r_lCombinedKeyID As Integer) As Integer` | Calculate combined key from source+key |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_FindParty_like_shortname` | `SearchByQuery` | Search by shortname |
| `spu_select_id_from_shortname` | `GetID` | Get party from shortname |
| `spu_select_id_from_name` | `GetID` | Get party from name |
| `spu_select_name_from_id` | `GetName` | Get name from CNT |
| `spu_SAM_ACT_Select_AccountBal` | `GetAccountBalance` | Get account balance |
| `spu_Select_Incurred_on_all_claims` | `GetClaimIncurred` | Get claim incurred total |
| `spu_select_allow_commission_from_shortname` | `GetCommissionFlag` | Check commission flag |
| `spu_select_associated_client_count` | `CheckAssociatedClients` | Count associated clients |
| `spu_get_visible_agent_types` | `GetVisibleAgentTypes` | Get visible agent types |
| `spu_SIR_Get_Party_Type_By_Code` | `GetPartyTypeByCode` | Get party type by code |
| `spu_SIR_Party_BlackList_Reason_Sel` | `GetClientBlackListingReason` | Get blacklist reason |
| `spu_pmuser_insurer_access` | `CheckInsurerAccess` | Check insurer access |
| `spu_Get_Insurer_Type` | `GetInsurerType` | Get insurer types |

**References:** `bSIREvent` (event logging), `bPMLookup` (lookups), `bUnderwritingBranchFunc` (branch detection)

---

### GetChangeReason
**Directory:** `GetChangeReason/`
**COM Name:** `bSIRGetChangeReason`
**Purpose:** Captures change reason when modifying claims or performing MTAs. Presents a dialog for users to select/enter a reason code.

**Business Methods — `bSIRGetChangeReason`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard initialisation; creates bPMLookup, bACTExplorer |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `GetReasonData` | `(ByVal lproductID As Integer, ByVal sTransactionType As String, ByRef r_vReasonData As Object) As Integer` | Get event descriptions for MTA or Claims by product; dispatches to appropriate SP based on transaction type |
| `GetLookupValues` | `(ByRef iLookupType As Integer, ByRef vTableArray(,), ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer` | Get lookup values via bPMLookup |
| `SetKeys` | `(ByRef vKeyArray(,) As Object) As Integer` | Navigator SetKeys (stub) |
| `GetKeys` | `(ByRef vKeyArray(,) As String) As Integer` | Navigator GetKeys (stub) |
| `Start` | `() As Integer` | Navigator Start (stub) |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_SIR_MTAEventDescription_Sel` | `GetReasonData` (MTA) | Get MTA event descriptions |
| `spu_SIR_ClaimEventDescription_Sel` | `GetReasonData` (Claims) | Get claim event descriptions |

**References:** `bPMLookup` (lookup values), `bACTExplorer` (account explorer)

---

### HandlerTransfer
**Directory:** `HandlerTransfer/`
**COM Name:** `bSIRHandlerTransfer`
**Purpose:** Transfers policies and clients between account handlers/executives. Bulk reassignment tool with event logging.

**Business Methods — `bSIRHandlerTransfer`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Initialise with bSIREvent, bPMLookup; resolves CLICHANGE and POLCHANGE event type IDs |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `Start` | `() As Integer` | Entry point — calls UpdateExecutiveData and/or UpdateHandlerData based on set properties |
| `UpdateExecutiveData` | `() As Integer` | Updates account executive on all matching clients and their policies; writes events |
| `UpdateHandlerData` | `() As Integer` | Updates account handler on all matching policies; writes events |
| `GetClients` | `(ByRef vClients(,) As Object) As Integer` | Get all clients for the old executive |
| `GetPoliciesForExecutive` | `(ByRef vPolicies(,) As Object) As Integer` | Get policies linked to old executive |
| `GetPoliciesForHandler` | `(ByRef vPolicies(,) As Object) As Integer` | Get policies linked to old handler |
| `UpdateClientAccountExecutive` | `(ByVal lPartyCnt As Integer) As Integer` | Update single client's account executive |
| `UpdatePolicyAccountExecutive` | `(ByVal lInsuranceFileCnt As Integer) As Integer` | Update single policy's account executive |
| `UpdateAccountHandler` | `(ByVal lInsuranceFileCnt As Integer) As Integer` | Update single policy's account handler |
| `WriteClientEvent` | `(ByVal lPartyCnt As Integer) As Integer` | Write client change event via bSIREvent |
| `WritePolicyHandlerEvent` | `(ByVal lInsuranceFileCnt, lPartyCnt, lInsuranceFolderCnt As Integer) As Integer` | Write policy handler change event |
| `WritePolicyExecutiveEvent` | `(ByVal lInsuranceFileCnt, lPartyCnt, lInsuranceFolderCnt As Integer) As Integer` | Write policy executive change event |

**Properties:** `OldHandlerCnt`, `NewHandlerCnt`, `OldHandlerRef`, `NewHandlerRef`, `OldExecutiveCnt`, `NewExecutiveCnt`, `OldExecutiveRef`, `NewExecutiveRef`, `UniqueId`, `ScreenHierarchy`

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_Account_Executive_Handler_Transfer` | `Start` | Insert configuration audit details |
| (inline SQL) | `GetClients` | `SELECT party_cnt FROM Party WHERE consultant_cnt = {old}` |
| (inline SQL) | `UpdateClientAccountExecutive` | `UPDATE party SET consultant_cnt = {new}` |
| (inline SQL) | `UpdatePolicyAccountExecutive` | `UPDATE insurance_file SET account_executive_cnt = {new}` |
| (inline SQL) | `UpdateAccountHandler` | `UPDATE insurance_file SET account_handler_cnt = {new}` |

**References:** `bSIREvent` (event logging), `bPMLookup` (lookup values)

---

### Instalments
**Directory:** `Instalments/`
**COM Name:** `bSIRPFInstalmentBusiness`, `bSIRPremiumFinance`, `bSIRPFScheme`, `bSIRPFRF`
**Purpose:** **Largest component (262 methods, 279 SPs).** Complete premium finance and instalment management — scheme setup, plan creation, quote calculation, instalment scheduling, collection, cancellation, EDI messaging, third-party finance providers (SG, Close), and payment hub integration.

**Sub-Components (7 business projects):**

#### `bSIRPFInstalments` — Core Instalment Orchestrator
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard; creates child components bSIRPremiumFinance, bSIRPFScheme |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `Start` | `() As Integer` | Navigator entry point; dispatches by task name |
| `Calculate_Quotes` | `(ByRef r_vQuotes(,) As Object) As Integer` | Calculate all instalment plan quote options for the current policy |
| `CalculateSingleQuote` | `(ByRef r_vQuoteData(,) As Object) As Integer` | Calculate a single specific instalment quote |
| `AcceptQuote` | `(ByVal v_lQuoteID As Integer) As Integer` | Accept/confirm a quoted instalment plan |
| `CancelPlanInHouse` | `(ByVal v_lPlanID As Integer) As Integer` | Cancel an in-house instalment plan |
| `CancelPlanThirdParty` | `(ByVal v_lPlanID As Integer) As Integer` | Cancel a third-party instalment plan |
| `CancelPolicies` | `(ByRef v_vPolicies(,) As Object) As Integer` | Cancel policies on an instalment plan |
| `ChangeStatus` | `(ByVal v_lPlanID As Integer, v_lNewStatus As Integer) As Integer` | Update instalment plan status |
| `CheckDeposit` | `(ByRef r_bDepositRequired As Boolean, ByRef r_dDepositAmount As Decimal) As Integer` | Check if deposit is required and calculate amount |
| `AllocateDeposit` | `(ByRef v_vDepositData(,) As Object) As Integer` | Allocate deposit payment to plan |
| `GetPlanDetails` | `(ByVal v_lPlanID As Integer, ByRef r_vPlanData(,) As Object) As Integer` | Get full instalment plan details |
| `GetPlanSchedule` | `(ByVal v_lPlanID As Integer, ByRef r_vSchedule(,) As Object) As Integer` | Get instalment schedule (dates, amounts) |
| `GetPlanHistory` | `(ByVal v_lPlanID As Integer, ByRef r_vHistory(,) As Object) As Integer` | Get plan transaction history |
| `UpdatePlanDetails` | `(ByRef v_vPlanData(,) As Object) As Integer` | Update instalment plan details |
| `ReCalculate` | `(ByVal v_lPlanID As Integer) As Integer` | Recalculate instalments after MTA/amendment |
| `GenerateInstalments` | `(ByVal v_lPlanID As Integer) As Integer` | Generate instalment schedule entries |
| `SetCancellationRefund` | `(ByVal v_dRefundAmount As Decimal) As Integer` | Set cancellation refund amount |
| `GetDefaultPaymentMethod` | `(ByRef r_lPaymentMethodId As Integer) As Integer` | Get default payment method for plan |
| `GetCollectionHistory` | `(ByVal v_lPlanID As Integer, ByRef r_vHistory(,) As Object) As Integer` | Get payment collection history |
| `ProcessCollection` | `(ByVal v_lPlanID As Integer) As Integer` | Process a payment collection for plan |

**Key SPs (40+):** `spu_PF_CalculateQuote`, `spu_PF_AcceptQuote`, `spu_PF_GetPlanDetails`, `spu_PF_GetPlanSchedule`, `spu_PF_UpdatePlan`, `spu_PF_CancelPlan`, `spu_PF_GenerateInstalments`, `spu_PF_GetCollectionHistory`, `spu_PF_ProcessCollection`

#### `bSIRPremiumFinance` — Comprehensive Premium Finance Engine (150+ methods)
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Initialise; creates bSIRParty, bSIREvent, bACTExplorer, bSIROptions and many helper objects |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `Start` | `() As Integer` | Navigator entry point — dispatches by task name to specific handler |
| `CreateNewPlan` | `(...) As Integer` | Create a new premium finance plan |
| `GetPlan` | `(ByVal v_lPlanCnt As Integer, ByRef r_vPlanData(,) As Object) As Integer` | Get plan details by CNT |
| `GetPlanDetails` | `(ByRef r_vPlanDetails(,) As Object) As Integer` | Get detailed plan information |
| `UpdatePlan` | `(ByRef v_vPlanData(,) As Object) As Integer` | Update an existing plan |
| `DeletePlan` | `(ByVal v_lPlanCnt As Integer) As Integer` | Delete a plan |
| `CalculatePremium` | `(ByRef r_vPremiumData(,) As Object) As Integer` | Calculate premium amounts |
| `CalculateInstalments` | `(ByRef r_vInstalmentData(,) As Object) As Integer` | Calculate instalment schedule |
| `GetInstalmentSchedule` | `(ByRef r_vSchedule(,) As Object) As Integer` | Get full instalment schedule |
| `ProcessPayment` | `(ByRef v_vPaymentData(,) As Object) As Integer` | Process a payment on a plan |
| `ProcessRefund` | `(ByRef v_vRefundData(,) As Object) As Integer` | Process a refund on a plan |
| `CancelPlan` | `(ByVal v_lPlanCnt As Integer) As Integer` | Cancel a premium finance plan |
| `ReinstatePlan` | `(ByVal v_lPlanCnt As Integer) As Integer` | Reinstate a cancelled plan |
| `SuspendPlan` | `(ByVal v_lPlanCnt As Integer) As Integer` | Suspend a plan |
| `ActivatePlan` | `(ByVal v_lPlanCnt As Integer) As Integer` | Activate a suspended plan |
| `GetDirectDebitDetails` | `(ByRef r_vDDData(,) As Object) As Integer` | Get direct debit details for plan |
| `UpdateDirectDebitDetails` | `(ByRef v_vDDData(,) As Object) As Integer` | Update direct debit details |
| `GetPaymentHistory` | `(ByRef r_vHistory(,) As Object) As Integer` | Get payment history for plan |
| `GetStatementData` | `(ByRef r_vStatement(,) As Object) As Integer` | Get statement data for plan |
| `ProcessMTAAdjustment` | `(ByRef v_vAdjustmentData(,) As Object) As Integer` | Process MTA premium adjustment |
| `ProcessRenewalAdjustment` | `(ByRef v_vRenewalData(,) As Object) As Integer` | Process renewal premium adjustment |
| `GetCollectionSchedule` | `(ByRef r_vSchedule(,) As Object) As Integer` | Get future collection schedule |
| `ProcessCancellationRefund` | `(ByRef v_vRefundData(,) As Object) As Integer` | Process cancellation refund |
| `GetFinanceProviderDetails` | `(ByRef r_vProviderData(,) As Object) As Integer` | Get finance provider config |
| `SubmitToFinanceProvider` | `(ByVal v_lPlanCnt As Integer) As Integer` | Submit plan to third-party finance provider |
| `GetProviderResponse` | `(ByVal v_lPlanCnt As Integer, ByRef r_vResponse(,) As Object) As Integer` | Get response from finance provider |

**Key SPs (100+):** `spu_PF_*` pattern — `spu_PF_CreatePlan`, `spu_PF_GetPlan`, `spu_PF_UpdatePlan`, `spu_PF_DeletePlan`, `spu_PF_CalculateInstalments`, `spu_PF_ProcessPayment`, `spu_PF_ProcessRefund`, `spu_PF_CancelPlan`, `spu_PF_GetSchedule`, `spu_PF_GetPaymentHistory`, `spu_PF_GetDDDetails`, `spu_PF_SubmitProvider`, and ~90 more

#### `bSIRPFScheme` — Instalment Scheme Management
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `GetDetails` / `GetNext` | Standard entity iteration | Navigate scheme records |
| `EditAdd` / `EditUpdate` / `EditDelete` | Standard entity CRUD | Manage scheme records |
| `Update` | `() As Integer` | Persist changes |
| `SearchAll` | `(ByRef r_vResultArray(,) As Object) As Integer` | List all schemes |
| `GetSchemeProducts` | `(ByRef r_vProducts(,) As Object) As Integer` | Get products linked to scheme |
| `UpdateSchemeProducts` | `(ByRef v_vProducts(,) As Object) As Integer` | Update scheme product links |
| `GetSchemeRates` | `(ByRef r_vRates(,) As Object) As Integer` | Get scheme rate bands |
| `UpdateSchemeRates` | `(ByRef v_vRates(,) As Object) As Integer` | Update scheme rate bands |
| `GetOptions` | `(ByRef r_vOptions(,) As Object) As Integer` | Get scheme options |
| `GetLookupValues` | `(ByRef iLookupType, vTableArray(,), iLanguageID, r_vResultArray(,) As Object) As Integer` | Lookup values |

**Key SPs (7):** `spe_pf_scheme_*` (sel/add/upd/del/saa/check_id), `spu_PF_GetSchemeProducts`, `spu_PF_GetSchemeRates`

#### `bSIRPFEDIMessage` — EDI Messaging
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | Standard | Standard |
| `Start` | `() As Integer` | Navigator entry |
| `CreateEDIMessage` | `(ByVal v_lPlanCnt As Integer) As Integer` | Create EDI message for plan |
| `GetEDIMessages` | `(ByRef r_vMessages(,) As Object) As Integer` | Get EDI messages for plan |
| `ProcessEDIResponse` | `(ByRef v_vResponse(,) As Object) As Integer` | Process EDI response |
| `SendEDIMessage` | `(ByVal v_lMessageCnt As Integer) As Integer` | Send EDI message |
| `GetEDIStatus` | `(ByVal v_lMessageCnt As Integer, ByRef r_lStatus As Integer) As Integer` | Get EDI message status |

**Key SP (1):** `spu_PF_EDI_*`

#### `bSIRPFExport` — Export/Report Generation
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | Standard | Standard |
| `Start` | `() As Integer` | Navigator entry |
| `ExportToCSV` | `(ByVal v_sFilePath As String) As Integer` | Export plan data to CSV |
| `ExportToExcel` | `(ByVal v_sFilePath As String) As Integer` | Export plan data to Excel |
| `GenerateReport` | `(ByVal v_lReportType As Integer, ByRef r_vReportData(,) As Object) As Integer` | Generate PF report |

**Key SPs (2):** `spu_PF_Export_*`

#### `bSIRPFRF` — Renewal Finance
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | Standard | Standard |
| `SetProcessModes` | Standard | Standard |
| `Start` | `() As Integer` | Navigator entry |
| `ProcessRenewalFinance` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Process PF renewal |
| `CalculateRenewalQuote` | `(ByRef r_vQuoteData(,) As Object) As Integer` | Calculate renewal PF quote |
| `GetRenewalPlanDetails` | `(ByRef r_vDetails(,) As Object) As Integer` | Get renewal plan details |
| `ApplyRenewalFinance` | `(ByVal v_lOldPlanCnt As Integer, v_lNewPlanCnt As Integer) As Integer` | Apply renewal finance terms |

**Key SPs (6):** `spu_PF_Renewal_*` pattern

#### `bSIRPFSubmit` — Third Party Submission
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | Standard | Standard |
| `Start` | `() As Integer` | Navigator entry |
| `SubmitToSG` | `(ByVal v_lPlanCnt As Integer) As Integer` | Submit to SG finance provider |
| `SubmitToClose` | `(ByVal v_lPlanCnt As Integer) As Integer` | Submit to Close Brothers |
| `GetSubmissionStatus` | `(ByVal v_lPlanCnt As Integer, ByRef r_vStatus(,) As Object) As Integer` | Get submission status |
| `ProcessSubmissionResponse` | `(ByRef v_vResponse(,) As Object) As Integer` | Process submission response |
| `CancelSubmission` | `(ByVal v_lPlanCnt As Integer) As Integer` | Cancel a pending submission |

**Stored Procedures (279 total):** `spu_PF_*` pattern dominates — plan CRUD, schedule generation, collection processing, payment handling, scheme management, EDI messaging, provider integration, renewal processing, export operations

**References:** `bSIRInsuranceFile`, `bSIRParty`, `bSIREvent`, `bSIRPFScheme`, `bSIRPremiumFinance`, `bSIROptions`, `bACTExplorer`

---

### Insurance File
**Directory:** `Insurance File/`
**COM Name:** `bSIRInsuranceFile`, `bSIRInsuranceFileBusiness`
**Purpose:** **Core policy/quote data management.** Handles the insurance file record (header), policy clients, fees, narratives, standard wordings, sub-agents, coinsurance, discounts, cancellation, and lapse. The insurance file is the central entity linking risks, premiums, and transactions.

**Business Methods — `bSIRInsuranceFile`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Initialise with bSIREvent, bSIROptions, bPMLookup, bSIRSharepoint; detects underwriting branch |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `Start` | `() As Integer` | Navigator entry point; dispatches by task/process mode |
| `GetDetails` / `GetNext` | Standard entity iteration | Navigate insurance file records |
| `EditAdd` / `EditUpdate` / `EditDelete` | Standard entity CRUD | Manage insurance file records |
| `Update` | `() As Integer` | Persist all changes with transaction |
| `Cancel` | `() As Integer` | Check unsaved changes; handle locks |
| `CheckID` | `(ByRef vID As Object) As Integer` | Validate insurance file ID |
| `GetHeader` | `(ByVal v_lInsFileCnt As Integer, ByRef r_vHeaderData(,) As Object) As Integer` | Get policy header information |
| `GetSummary` | `(ByRef vSummaryArray As Object) As Integer` | Get insurance file summary |
| `GetInsuranceFileStatus` | `(ByVal v_lInsFileCnt As Integer, ByRef r_lStatus As Integer) As Integer` | Get current status of insurance file |
| `UpdateStatus` | `(ByVal v_lInsFileCnt As Integer, v_lNewStatus As Integer) As Integer` | Update insurance file status |
| `GetPolicyDetails` | `(ByRef r_vPolicyDetails(,) As Object) As Integer` | Get full policy details |
| `UpdatePolicyDetails` | `(ByRef v_vPolicyDetails(,) As Object) As Integer` | Update policy details |
| `GetSubAgents` | `(ByRef r_vSubAgents(,) As Object) As Integer` | Get sub-agents for policy |
| `UpdateSubAgents` | `(ByRef v_vSubAgents(,) As Object) As Integer` | Update sub-agent assignments |
| `GetCoinsurance` | `(ByRef r_vCoinsurance(,) As Object) As Integer` | Get coinsurance details |
| `UpdateCoinsurance` | `(ByRef v_vCoinsurance(,) As Object) As Integer` | Update coinsurance details |
| `GetPremiumSummary` | `(ByRef r_vPremium(,) As Object) As Integer` | Get premium summary breakdown |
| `GetTransactions` | `(ByRef r_vTransactions(,) As Object) As Integer` | Get policy transactions |
| `GetAssociates` | `(ByRef r_vAssociates(,) As Object) As Integer` | Get policy associates |
| `UpdateAssociates` | `(ByRef v_vAssociates(,) As Object) As Integer` | Update policy associates |
| `GetRisks` | `(ByRef r_vRisks(,) As Object) As Integer` | Get risks on policy |
| `GetDocumentLibrary` | `(ByRef r_sLibraryPath As String) As Integer` | Get SharePoint document library path |
| `CreateDocumentLibrary` | `(ByVal v_sLibraryPath As String) As Integer` | Create SharePoint document library |
| `GetStandardWordings` | `(ByRef r_vWordings(,) As Object) As Integer` | Get standard wordings |
| `UpdateStandardWordings` | `(ByRef v_vWordings(,) As Object) As Integer` | Update standard wordings |
| `GetPolicyVersionHistory` | `(ByRef r_vHistory(,) As Object) As Integer` | Get version history |
| `GetLookupValues` | `(ByRef iLookupType As Integer, ...) As Integer` | Get lookup values |
| `SetKeys` / `GetKeys` | Standard navigator keys | Navigator key management |
| `SearchAll` | `(ByRef r_vResultArray(,) As Object) As Integer` | Search all insurance files |
| `DirectAdd` | `(Optional params...) As Integer` | Direct add without edit cycle |
| `DirectUpdate` | `(Optional params...) As Integer` | Direct update without edit cycle |
| `DirectDelete` | `(Optional params...) As Integer` | Direct delete without edit cycle |
| `ValidatePolicyReference` | `(ByVal v_sPolicyRef As String, ByRef r_bValid As Boolean) As Integer` | Validate policy reference format |
| `GetOption` | `(ByVal v_iOptionNumber As Integer, ByRef r_nOptionValue As Integer) As Integer` | Get system option value |
| `CreateEvent` | `(ByRef r_lEventCnt As Integer, ...) As Integer` | Create event/task via bSIREvent |
| `GetInsurerDetails` | `(ByRef r_vInsurer(,) As Object) As Integer` | Get insurer details for policy |
| `GetProductDetails` | `(ByRef r_vProduct(,) As Object) As Integer` | Get product details for policy |

**Services Class — `bSIRInsuranceFileServices`:**
| Method | Description |
|--------|-------------|
| `GetInsuranceFileHeader` | Get insurance file header via service |
| `GetInsuranceFileDetails` | Get full details via service |
| `UpdateInsuranceFile` | Update insurance file via service |
| `GetPolicyTransactions` | Get transactions via service |
| `GetPolicySummary` | Get policy summary via service |

**Stored Procedures (89+):**
| SP | Called By | Purpose |
|----|----------|---------|
| `spe_Insurance_File_sel` | `GetDetails` (data) | Select insurance file |
| `spe_Insurance_File_add` | `EditAdd` (data) | Add insurance file |
| `spe_Insurance_File_upd` | `EditUpdate` (data) | Update insurance file |
| `spe_Insurance_File_del` | `EditDelete` (data) | Delete insurance file |
| `spe_Insurance_File_saa` | `GetDetails` (data) | Select all insurance files |
| `spe_Insurance_File_check_id` | `CheckID` (data) | Check insurance file ID |
| `spu_SIR_Get_InsuranceFileStatus` | `GetInsuranceFileStatus` | Get status |
| `spu_SIR_Update_Insurance_File_Status` | `UpdateStatus` | Update status |
| `spu_get_insurance_file_header` | `GetHeader` | Get header |
| `spu_Get_Sub_Agents` | `GetSubAgents` | Get sub-agents |
| `spu_Update_Sub_Agents` | `UpdateSubAgents` | Update sub-agents |
| `spu_Get_Coinsurance` | `GetCoinsurance` | Get coinsurance |
| `spu_Update_Coinsurance` | `UpdateCoinsurance` | Update coinsurance |
| `spu_Get_Premium_Summary` | `GetPremiumSummary` | Get premium breakdown |
| `spu_Get_Policy_Associates` | `GetAssociates` | Get associates |
| `spu_SIR_Get_Party_Document_Library` | `GetDocumentLibrary` | Get doc library |
| `spu_get_standard_wordings` | `GetStandardWordings` | Get standard wordings |
| `spu_update_standard_wordings` | `UpdateStandardWordings` | Update wordings |
| `spu_SIR_Get_Sharepoint_Tags` | `CreateDocumentLibrary` | Get SharePoint tags |

**References:** `bSIREvent`, `bSIROptions`, `bSIRSharepoint`, `bSIRSharepointOnline`, `bPMLookup`, `bUnderwritingBranchFunc`, `dSIRInsuranceFile`

---

### Insurance File System
**Directory:** `Insurance File System/`
**COM Name:** `bSIRInsuranceFileSystem`
**Purpose:** Manages system-level attributes on insurance files — custom system fields, lookups, and event-based system data.

**Business Methods — `bSIRInsuranceFileSystem`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `GetDetails` / `GetNext` | Standard entity iteration | Navigate insurance file system records |
| `EditAdd` / `EditUpdate` / `EditDelete` | Standard entity CRUD | Manage file system records |
| `Update` | `() As Integer` | Persist changes |

**Stored Procedures (6):**
| SP | Called By | Purpose |
|----|----------|---------|
| `spe_Insurance_File_System_sel` | `GetDetails` (data) | Select file system |
| `spe_Insurance_File_System_add` | `EditAdd` (data) | Add file system |
| `spe_Insurance_File_System_upd` | `EditUpdate` (data) | Update file system |
| `spe_Insurance_File_System_del` | `EditDelete` (data) | Delete file system |
| `spe_Insurance_File_System_saa` | `GetDetails` (data) | Select all |
| `spe_Insurance_File_System_check_id` | `CheckID` (data) | Check ID |

**References:** `dSIRInsuranceFileSystem` (data layer)

---

### Insurance Folder
**Directory:** `Insurance Folder/`
**COM Name:** `bSIRInsuranceFolder`
**Purpose:** Manages the insurance folder — the container for all versions of a policy (NB, MTA, Renewal). Handles folder creation, duplicate policy numbers, and version linking.

**Business Methods — `bSIRInsuranceFolder`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `GetDetails` / `GetNext` | Standard entity iteration | Navigate insurance folder records |
| `EditAdd` / `EditUpdate` / `EditDelete` | Standard entity CRUD | Manage folder records |
| `Update` | `() As Integer` | Persist changes |
| `RemoveDuplicatePolicy` | `(ByVal v_lInsuranceFolderCnt As Integer) As Integer` | Remove duplicate policy entries from folder |

**Stored Procedures (7):**
| SP | Called By | Purpose |
|----|----------|---------|
| `spe_Insurance_Folder_sel` | `GetDetails` (data) | Select insurance folder |
| `spe_Insurance_Folder_add` | `EditAdd` (data) | Add insurance folder |
| `spe_Insurance_Folder_upd` | `EditUpdate` (data) | Update insurance folder |
| `spe_Insurance_Folder_del` | `EditDelete` (data) | Delete insurance folder |
| `spe_Insurance_Folder_saa` | `GetDetails` (data) | Select all |
| `spe_Insurance_Folder_check_id` | `CheckID` (data) | Check ID |
| `spu_Remove_Duplicate_Policy` | `RemoveDuplicatePolicy` | Remove duplicates |

**References:** `dSIRInsuranceFolder` (data layer)

---

### Lifestyle
**Directory:** `Lifestyle/`
**COM Name:** `bSIRLifestyle` (via data layer `dSIRPartyLifestyle`)
**Purpose:** Manages party lifestyle information — hobbies, activities, health details used for personal lines underwriting.

**Business Methods — `bSIRLifestyle`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `Start` | `() As Integer` | Navigator entry point |
| `GetDetails` / `GetNext` | Standard entity iteration | Navigate lifestyle records |
| `EditAdd` / `EditUpdate` / `EditDelete` | Standard entity CRUD | Manage lifestyle records |
| `Update` | `() As Integer` | Persist changes |
| `GetLookupValues` | `(ByRef iLookupType As Integer, ...) As Integer` | Get lookup values |
| `SearchAll` | `(ByRef r_vResultArray(,) As Object) As Integer` | Search all lifestyle records |
| `GetLifestyleTypes` | `(ByRef r_vTypes(,) As Object) As Integer` | Get lifestyle type definitions |
| `GetLifestyleForParty` | `(ByVal v_lPartyCnt As Integer, ByRef r_vLifestyle(,) As Object) As Integer` | Get lifestyle data for a party |
| `UpdateLifestyleForParty` | `(ByVal v_lPartyCnt As Integer, ByRef v_vData(,) As Object) As Integer` | Update party lifestyle data |

**Automated Class — `bSIRLifestyleAutomated`:**
| Method | Description |
|--------|-------------|
| `Initialise` / `SetProcessModes` / `Start` | Navigator automation flow |
| `GetSummary` / `SetKeys` / `GetKeys` | Navigator key management |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|---------|
| `spe_lifestyle_data_sel` | `GetDetails` (data) | Select lifestyle data |
| `spe_lifestyle_data_add` | `EditAdd` (data) | Add lifestyle data |
| `spe_lifestyle_data_upd` | `EditUpdate` (data) | Update lifestyle data |
| `spe_lifestyle_data_del` | `EditDelete` (data) | Delete lifestyle data |
| `spu_sir_get_lifestyle_types` | `GetLifestyleTypes` | Get lifestyle types |
| `spu_sir_get_lifestyle_for_party` | `GetLifestyleForParty` | Get party lifestyle |

**References:** `dSIRLifestyleData` (data layer)

---

### MediaTypeValidation
**Directory:** `MediaTypeValidation/`
**COM Name:** `bSIRMediaTypeValidation`
**Purpose:** Validates payment media types — validates bank account numbers, sort codes, credit card numbers using modulus checking. Called during receipt/payment processing.

**Business Methods — `bSIRMediaTypeValidation`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `ValidateNumber` | `(ByVal v_sNumber As String, ByVal v_lMediaTypeId As Integer, ByRef r_bValid As Boolean) As Integer` | Validate a media number (phone/fax/email) against type rules |
| `RoundCurrency` | `(ByVal v_dAmount As Decimal, ByRef r_dRounded As Decimal) As Integer` | Round a currency amount to configured precision |
| `GetValidationCode` | `(ByVal v_lMediaTypeId As Integer, ByRef r_sCode As String) As Integer` | Get validation code/pattern for a media type |
| `GetMediaTypeIdForCode` | `(ByVal v_sCode As String, ByRef r_lMediaTypeId As Integer) As Integer` | Reverse lookup — get media type ID from code |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_sir_media_type_validation` | `ValidateNumber` | Validate media number |
| `spu_sir_round_currency` | `RoundCurrency` | Round currency |
| `spu_sir_get_validation_code` | `GetValidationCode` | Get validation code |
| `spu_sir_get_media_type_id` | `GetMediaTypeIdForCode` | Get media type ID |

**References:** SharedFiles

---

### OnlinePartyMaintenance
**Directory:** `OnlinePartyMaintenance/`
**COM Name:** `bSIROnlinePartyMaintenance`
**Purpose:** Updates online access status for parties (enable/disable portal access).

**Business Methods — `bSIROnlinePartyMaintenance`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `UpdateOnlineAccessStatus` | `(ByVal v_lPartyCnt As Integer, ByVal v_bEnabled As Boolean) As Integer` | Enable or disable online access for a party |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_sir_update_online_access` | `UpdateOnlineAccessStatus` | Update online access flag |

**References:** SharedFiles

---

### Orion Link
**Directory:** `Orion Link/`
**COM Name:** `bSIROrionLink`
**Purpose:** **Bridges Sirius and Orion accounting systems.** Creates and synchronizes accounts, ledgers, and party data between the insurance system and the Orion financial accounting system. Handles document posting and batch export.

**Sub-Components (3 business projects):**

#### `bSirOrionLink` — Core Orion Integration
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `PostDocument` | `(ByVal v_lDocumentCnt As Integer, ByRef r_lOrionDocRef As Integer) As Integer` | Post a document to Orion accounting |
| `GetAccountIDs` | `(ByVal v_lPartyCnt As Integer, ByRef r_vAccountIDs(,) As Object) As Integer` | Get Orion account IDs for a party |
| `GetOrionStatus` | `(ByRef r_bConnected As Boolean) As Integer` | Check Orion connectivity |

#### `bSIROrionUpdate` — Batch Orion Sync
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | Standard | Standard |
| `Start` | `() As Integer` | Navigator entry — runs batch sync |
| `SiriusToOrion` | `() As Integer` | Synchronise Sirius parties to Orion accounts |
| `GetPendingUpdates` | `(ByRef r_vPending(,) As Object) As Integer` | Get parties pending Orion sync |
| `ProcessUpdate` | `(ByVal v_lPartyCnt As Integer) As Integer` | Process single party Orion update |

#### `bPMBOrionLink` — Account Link Management
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | Standard | Standard |
| `GetAccountID` | `(ByVal v_lPartyCnt As Integer, ByRef r_lOrionAccountID As Integer) As Integer` | Get Orion account ID from party |
| `CreateAccount` | `(ByVal v_lPartyCnt As Integer, ByRef r_lOrionAccountID As Integer) As Integer` | Create Orion account for party |
| `UpdateAccount` | `(ByVal v_lPartyCnt As Integer) As Integer` | Update Orion account |
| `LinkAccount` | `(ByVal v_lPartyCnt As Integer, v_lOrionAccountID As Integer) As Integer` | Link party to Orion account |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_sir_orion_post_document` | `PostDocument` | Post document to Orion |
| `spu_sir_orion_get_account_ids` | `GetAccountIDs` | Get Orion account IDs |
| `spu_sir_orion_get_pending_updates` | `GetPendingUpdates` | Get pending sync |
| `spu_sir_orion_create_account` | `CreateAccount` | Create Orion account |
| `spu_sir_orion_update_account` | `UpdateAccount` | Update Orion account |
| `spu_sir_orion_link_account` | `LinkAccount` | Link account |

**References:** `bSIRParty` (party operations), Orion Accounting system

---

### Party
**Directory:** `Party/`
**COM Name:** `bSIRParty`, `bSIRPartyBusiness`
**Purpose:** **Core party/client management — the second largest component (217 methods, 242 SPs).** Full lifecycle for all party types (personal client, corporate client, agent, agent group, insurer, other party). Manages addresses, contacts, bank details, convictions, lifestyles, fee amounts, agent products, supplier details, GIS data, and variant addresses.

**Sub-Components (16 business projects):**

#### `bSIRParty` — Core Party Management (50+ methods)
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard; creates bSIREvent, bSIROptions, bPMLookup, bSIRSharepoint |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `Start` | `() As Integer` | Navigator entry point |
| `GetDetails` / `GetNext` | Standard entity iteration | Navigate party records |
| `EditAdd` / `EditUpdate` / `EditDelete` | Standard entity CRUD | Manage party records |
| `Update` | `() As Integer` | Persist all changes; creates events, updates SharePoint |
| `Cancel` | `() As Integer` | Check unsaved changes |
| `CheckID` | `(ByRef vID As Object) As Integer` | Validate party ID |
| `DirectAdd` | `(Optional ByRef ~40 params As Object) As Integer` | Direct add without edit cycle — accepts full party data |
| `DirectUpdate` | `(Optional ByRef ~40 params As Object) As Integer` | Direct update without edit cycle |
| `DirectDelete` | `(Optional ByRef vPartyCnt As Object) As Integer` | Direct delete |
| `GetSummary` | `(ByRef vSummaryArray As Object) As Integer` | Get party summary |
| `SearchAll` | `(ByRef r_vResultArray(,) As Object) As Integer` | Search all parties |
| `GetAddress` | `(ByRef r_vAddress(,) As Object) As Integer` | Get primary address |
| `GetAllAddresses` | `(ByRef r_vAddresses(,) As Object) As Integer` | Get all addresses for party |
| `UpdateAddress` | `(ByRef v_vAddress(,) As Object) As Integer` | Update address |
| `GetMediaContacts` | `(ByRef r_vContacts(,) As Object) As Integer` | Get phone/email/fax contacts |
| `UpdateMediaContacts` | `(ByRef v_vContacts(,) As Object) As Integer` | Update media contacts |
| `GetBankDetails` | `(ByRef r_vBank(,) As Object) As Integer` | Get bank account details |
| `UpdateBankDetails` | `(ByRef v_vBank(,) As Object) As Integer` | Update bank details |
| `GetAssociates` | `(ByRef r_vAssociates(,) As Object) As Integer` | Get party associates |
| `UpdateAssociates` | `(ByRef v_vAssociates(,) As Object) As Integer` | Update associates |
| `GetDocumentLibrary` | `(ByRef r_sLibraryPath As String) As Integer` | Get SharePoint document library |
| `CreateDocumentLibrary` | `() As Integer` | Create SharePoint document library |
| `GetPartyType` | `(ByRef r_sPartyType As String) As Integer` | Get party type description |
| `GetLookupValues` | `(ByRef iLookupType As Integer, ...) As Integer` | Get lookup values |
| `SetKeys` / `GetKeys` | Standard navigator keys | Navigator key management |
| `CreateEvent` | `(ByRef r_lEventCnt As Integer, ...) As Integer` | Create event/task via bSIREvent |
| `GetOption` | `(ByVal v_iOptionNumber As Integer, ByRef r_nOptionValue As Integer) As Integer` | Get system option |
| `Merge` | `(ByVal v_lSourcePartyCnt As Integer, v_lTargetPartyCnt As Integer) As Integer` | Merge two party records |
| `ValidatePartyData` | `(ByRef r_vErrors(,) As Object) As Integer` | Validate party data completeness |
| `GetPartyHistory` | `(ByRef r_vHistory(,) As Object) As Integer` | Get party change history |

**Stored Procedures (main — 50+):** `spe_Party_sel`, `spe_Party_add`, `spe_Party_upd`, `spe_Party_del`, `spe_Party_saa`, `spe_Party_check_id`, `spu_SIR_Get_Party_Address`, `spu_SIR_Update_Party_Address`, `spu_SIR_Get_All_Addresses`, `spu_SIR_Get_Media_Contacts`, `spu_SIR_Update_Media_Contacts`, `spu_SIR_Get_Bank_Details`, `spu_SIR_Update_Bank_Details`, `spu_SIR_Get_Party_Associates`, `spu_SIR_Get_Party_Document_Library`, `spu_SIR_Upd_Party_Document_Library`, and 34+ more

#### `bSIRPartyAG` — Agent Party Extension
| Method | Description |
|--------|-------------|
| `GetDetails` / `GetNext` | Navigate agent party records |
| `EditAdd` / `EditUpdate` | Agent-specific CRUD |
| `GetAgencyAgreement` | Get agency agreement details |
| `UpdateAgencyAgreement` | Update agency agreement |
| `GetSubBranches` | Get agent sub-branches |
| `GetCommissionStructure` | Get commission structure for agent |
| `UpdateCommissionStructure` | Update commission rates |

#### `bSIRPartyAGG` — Agent Group Extension
| Method | Description |
|--------|-------------|
| `GetDetails` / `GetNext` | Navigate agent group records |
| `EditAdd` / `EditUpdate` | Agent group CRUD |
| `GetGroupMembers` | Get members of agent group |

#### `bSIRPartyAH` — Account Holder Extension
| Method | Description |
|--------|-------------|
| `GetDetails` / `GetNext` | Navigate account holder records |
| `EditAdd` / `EditUpdate` | Account holder CRUD |

#### `bSIRPartyBank` — Bank Details (17 methods)
| Method | Signature | Description |
|--------|-----------|-------------|
| `GetDetails` / `GetNext` | Standard entity iteration | Navigate bank account records |
| `EditAdd` / `EditUpdate` / `EditDelete` | Standard entity CRUD | Manage bank accounts |
| `Update` | `() As Integer` | Persist changes |
| `ValidateSortCode` | `(ByVal v_sSortCode As String, ByRef r_bValid As Boolean) As Integer` | Validate sort code |
| `ValidateAccountNumber` | `(ByVal v_sAccountNumber As String, ByRef r_bValid As Boolean) As Integer` | Validate account number |
| `GetBankName` | `(ByVal v_sSortCode As String, ByRef r_sBankName As String) As Integer` | Look up bank name from sort code |
| `GetDirectDebitMandate` | `(ByRef r_vMandate(,) As Object) As Integer` | Get DD mandate details |
| `UpdateDirectDebitMandate` | `(ByRef v_vMandate(,) As Object) As Integer` | Update DD mandate |
| `SearchAll` | `(ByRef r_vResultArray(,) As Object) As Integer` | Search all bank records |
| `GetSummary` | `(ByRef vSummaryArray As Object) As Integer` | Get bank summary |
| `SetKeys` / `GetKeys` | Standard navigator keys | Key management |
| `GetLookupValues` | Standard | Lookup values |
| `CheckID` | `(ByRef vID As Object) As Integer` | Validate bank ID |

#### `bSIRPartyCC` — Credit Check Extension
| Method | Description |
|--------|-------------|
| `GetCreditCheckResult` | Get credit check result for party |
| `PerformCreditCheck` | Initiate credit check |
| `GetCreditHistory` | Get credit check history |

#### `bSIRPartyEX` — External Reference Extension
| Method | Description |
|--------|-------------|
| `GetDetails` / `GetNext` | Navigate external reference records |
| `EditAdd` / `EditUpdate` / `EditDelete` | External reference CRUD |

#### `bSIRPartyFee` — Fee Management
| Method | Description |
|--------|-------------|
| `GetDetails` / `GetNext` | Navigate fee records |
| `EditAdd` / `EditUpdate` / `EditDelete` | Fee CRUD |
| `Update` | Persist fee changes |
| `GetFeeTypes` | Get fee type definitions |
| `CalculateFee` | Calculate fee amount |
| `SearchAll` | Search all fees |

#### `bSIRPartyFP` — Financial Profile Extension
| Method | Description |
|--------|-------------|
| `GetDetails` / `GetNext` | Navigate financial profile records |
| `EditAdd` / `EditUpdate` | Financial profile CRUD |

#### `bSIRPartyGC` — Group Company Extension
| Method | Description |
|--------|-------------|
| `GetDetails` / `GetNext` | Navigate group company records |
| `EditAdd` / `EditUpdate` | Group company CRUD |
| `GetGroupMembers` | Get group company members |

#### `bSIRPartyIN` — Insurer Extension
| Method | Description |
|--------|-------------|
| `GetDetails` / `GetNext` | Navigate insurer records |
| `EditAdd` / `EditUpdate` | Insurer CRUD |
| `GetInsurerProducts` | Get products for insurer |

#### `bSIRPartyLifestyle` — Party Lifestyle Links
| Method | Description |
|--------|-------------|
| `GetDetails` / `GetNext` | Navigate lifestyle data |
| `EditAdd` / `EditUpdate` / `EditDelete` | Lifestyle CRUD |
| `GetLifestyleForParty` | Get lifestyle entries for party |

#### `bSIRPartyNC` — Named Contact Extension
| Method | Description |
|--------|-------------|
| `GetDetails` / `GetNext` | Navigate named contacts |
| `EditAdd` / `EditUpdate` / `EditDelete` | Named contact CRUD |

#### `bSIRPartyNetData` — Network Data Extension
| Method | Description |
|--------|-------------|
| `GetDetails` / `GetNext` | Navigate network data records |
| `EditAdd` / `EditUpdate` | Network data CRUD |

#### `bSIRPartyOT` — Other Party Type Extension
| Method | Description |
|--------|-------------|
| `GetDetails` / `GetNext` | Navigate other party type records |
| `EditAdd` / `EditUpdate` | Other party type CRUD |

#### `bSIRPartyPC` — Postal Code Extension
| Method | Description |
|--------|-------------|
| `GetDetails` / `GetNext` | Navigate postal code records |
| `LookupPostcode` | Lookup address from postcode |
| `ValidatePostcode` | Validate postcode format |

**References:** `bSIREvent`, `bSIROptions`, `bSIRSharepoint`, `bSIRSharepointOnline`, `bPMLookup`, `dSIRParty*` (data layers)

---

### Party Loyalty Scheme
**Directory:** `Party Loyalty Scheme/`
**COM Name:** `bSIRPartyLoyaltyScheme`
**Purpose:** Manages party loyalty scheme memberships and points.

**Business Methods — `bSIRPartyLoyaltyScheme`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `GetSchemeDetails` | `(ByVal v_lPartyCnt As Integer, ByRef r_vSchemeData(,) As Object) As Integer` | Get loyalty scheme details for a party |
| `UpdateSchemeDetails` | `(ByRef v_vSchemeData(,) As Object) As Integer` | Update loyalty scheme for party |
| `GetSchemeTypes` | `(ByRef r_vTypes(,) As Object) As Integer` | Get available loyalty scheme types |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|--------|
| `spu_sir_party_loyalty_sel` | `GetSchemeDetails` | Select loyalty scheme |
| `spu_sir_party_loyalty_upd` | `UpdateSchemeDetails` | Update loyalty scheme |
| `spu_sir_party_loyalty_types_sel` | `GetSchemeTypes` | Get scheme types |
| `spu_sir_party_loyalty_del` | internal | Delete loyalty scheme |

**References:** SharedFiles

---

### Policy Number Maintenance
**Directory:** `Policy Number Maintenance/`
**COM Name:** `bSIRPolicyNumMaintenance`
**Purpose:** Generates policy numbers, client codes, case codes, cover note numbers, and media references. Manages numbering schemes (sequential, period-based) and renewal policy number generation.

**Business Methods — `bSIRPolicyNumberMaintenance`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `Start` | `() As Integer` | Navigator entry point |
| `GeneratePolicyNumber` | `(ByVal v_lProductId As Integer, ByRef r_sPolicyNumber As String) As Integer` | Generate next sequential policy number for product |
| `GenerateClientCode` | `(ByRef r_sClientCode As String) As Integer` | Generate next client code |
| `GenerateCaseCode` | `(ByRef r_sCaseCode As String) As Integer` | Generate next case code |
| `GetCurrentSequence` | `(ByVal v_lProductId As Integer, ByRef r_lSequence As Integer) As Integer` | Get current sequence number |
| `UpdateSequence` | `(ByVal v_lProductId As Integer, v_lNewSequence As Integer) As Integer` | Update/reset sequence |
| `GetNumberFormat` | `(ByVal v_lProductId As Integer, ByRef r_sFormat As String) As Integer` | Get number format pattern |
| `UpdateNumberFormat` | `(ByVal v_lProductId As Integer, v_sFormat As String) As Integer` | Update number format |
| `GetAllFormats` | `(ByRef r_vFormats(,) As Object) As Integer` | Get all number format configurations |
| `ValidatePolicyNumber` | `(ByVal v_sPolicyNumber As String, ByRef r_bValid As Boolean) As Integer` | Validate policy number format |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|--------|
| `spu_sir_generate_policy_number` | `GeneratePolicyNumber` | Generate policy number |
| `spu_sir_generate_client_code` | `GenerateClientCode` | Generate client code |
| `spu_sir_generate_case_code` | `GenerateCaseCode` | Generate case code |
| `spu_sir_get_policy_number_sequence` | `GetCurrentSequence` | Get sequence |
| `spu_sir_update_policy_number_sequence` | `UpdateSequence` | Update sequence |
| `spu_sir_get_number_format` | `GetNumberFormat` | Get format |
| `spu_sir_update_number_format` | `UpdateNumberFormat` | Update format |
| `spu_sir_get_all_number_formats` | `GetAllFormats` | Get all formats |
| `spu_sir_validate_policy_number` | `ValidatePolicyNumber` | Validate number |

**References:** SharedFiles

---

### Product Options
**Directory:** `Product Options/`
**COM Name:** `bSIRProductOptions`
**Purpose:** Manages product-level configuration options — hidden options, master options, branch-product mappings, and ICCS code lookups.

**Business Methods — `bSIRProductOptions`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `GetProductOptions` | `(ByVal v_lProductId As Integer, ByRef r_vOptions(,) As Object) As Integer` | Get all options for a product |
| `UpdateProductOptions` | `(ByVal v_lProductId As Integer, ByRef v_vOptions(,) As Object) As Integer` | Update product options |
| `GetOptionValue` | `(ByVal v_lProductId As Integer, v_lOptionId As Integer, ByRef r_vValue As Object) As Integer` | Get single option value |
| `SetOptionValue` | `(ByVal v_lProductId As Integer, v_lOptionId As Integer, v_vValue As Object) As Integer` | Set single option value |
| `GetDefaultOptions` | `(ByRef r_vDefaults(,) As Object) As Integer` | Get default option values |
| `CopyProductOptions` | `(ByVal v_lSourceProductId As Integer, v_lTargetProductId As Integer) As Integer` | Copy options from one product to another |
| `GetOptionDefinitions` | `(ByRef r_vDefinitions(,) As Object) As Integer` | Get option type definitions |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|--------|
| `spu_sir_product_options_sel` | `GetProductOptions` | Select product options |
| `spu_sir_product_options_upd` | `UpdateProductOptions` | Update product options |
| `spu_sir_product_option_value_sel` | `GetOptionValue` | Get option value |
| `spu_sir_product_option_value_upd` | `SetOptionValue` | Set option value |
| `spu_sir_product_options_defaults` | `GetDefaultOptions` | Get defaults |
| `spu_sir_product_options_copy` | `CopyProductOptions` | Copy options |
| `spu_sir_product_option_definitions` | `GetOptionDefinitions` | Get definitions |

**References:** SharedFiles

---

### Report Group
**Directory:** `Report Group/`
**COM Name:** `bSIRReportGroup`
**Purpose:** Manages report access groups — controls which user groups can see which reports.

**Business Methods — `bSIRReportGroup`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `GetDetails` / `GetNext` | Standard entity iteration | Navigate report group records |
| `EditAdd` / `EditUpdate` / `EditDelete` | Standard entity CRUD | Manage report groups |
| `Update` | `() As Integer` | Persist changes |
| `SearchAll` | `(ByRef r_vResultArray(,) As Object) As Integer` | Search all report groups |
| `GetReportsInGroup` | `(ByVal v_lGroupId As Integer, ByRef r_vReports(,) As Object) As Integer` | Get reports assigned to group |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|--------|
| `spe_report_group_sel` | `GetDetails` (data) | Select report group |
| `spe_report_group_add` | `EditAdd` (data) | Add report group |
| `spe_report_group_upd` | `EditUpdate` (data) | Update report group |
| `spe_report_group_del` | `EditDelete` (data) | Delete report group |
| `spe_report_group_saa` | `GetDetails` (data) | Select all |
| `spe_report_group_check_id` | `CheckID` (data) | Check ID |
| `spu_sir_get_reports_in_group` | `GetReportsInGroup` | Get reports in group |
| `spu_sir_report_group_search` | `SearchAll` | Search groups |

**References:** `dSIRReportGroup` (data layer)

---

### Report Scheduler
**Directory:** `Report Scheduler/`
**COM Name:** `bSIRReportScheduler`
**Purpose:** Schedules automated report generation. Manages report schedules, parameters, and execution triggers.

**Business Methods — `bSIRReportScheduler`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `Start` | `() As Integer` | Navigator entry point |
| `GetScheduledReports` | `(ByRef r_vReports(,) As Object) As Integer` | Get all scheduled reports |
| `AddScheduledReport` | `(ByRef v_vReportData(,) As Object) As Integer` | Add a new scheduled report |
| `UpdateScheduledReport` | `(ByRef v_vReportData(,) As Object) As Integer` | Update scheduled report |
| `DeleteScheduledReport` | `(ByVal v_lScheduleId As Integer) As Integer` | Delete a scheduled report |
| `ExecuteScheduledReport` | `(ByVal v_lScheduleId As Integer) As Integer` | Execute a scheduled report immediately |
| `GetScheduleHistory` | `(ByVal v_lScheduleId As Integer, ByRef r_vHistory(,) As Object) As Integer` | Get execution history |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|--------|
| `spu_sir_report_scheduler_sel` | `GetScheduledReports` | Select scheduled reports |
| `spu_sir_report_scheduler_add` | `AddScheduledReport` | Add schedule |
| `spu_sir_report_scheduler_upd` | `UpdateScheduledReport` | Update schedule |
| `spu_sir_report_scheduler_del` | `DeleteScheduledReport` | Delete schedule |
| `spu_sir_report_scheduler_execute` | `ExecuteScheduledReport` | Execute report |
| `spu_sir_report_scheduler_history` | `GetScheduleHistory` | Get history |
| `spu_sir_report_scheduler_status_upd` | internal | Update status |
| `spu_sir_report_scheduler_next_run` | internal | Calculate next run |

**References:** Crystal Reports, `bSIRReportGroup`

---

### SolutionConfig
**Directory:** `SolutionConfig/`
**COM Name:** `bSIRSolutionConfig`
**Purpose:** Retrieves solution-level configuration data for the deployment.

**Business Methods — `bSIRSolutionConfig`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `GetSolutionConfig` | `(ByRef r_vConfig(,) As Object) As Integer` | Retrieves solution configuration data using embedded SQL (not a stored procedure). Returns configuration key-value pairs for the deployment |

**Stored Procedures:** None — uses embedded/inline SQL queries

**References:** SharedFiles

---

### System Options
**Directory:** `System Options/`
**COM Name:** `bSIROptions`, `bSIRSystemOptions`
**Purpose:** Manages system-wide configuration options, currency settings, GIS screens, numbering schemes, user groups, accounting periods, tax groups, and document master settings.

**Business Methods — `bSIROptions`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `Start` | `() As Integer` | Navigator entry point |
| `GetDetails` / `GetNext` | Standard entity iteration | Navigate system option records |
| `EditAdd` / `EditUpdate` / `EditDelete` | Standard entity CRUD | Manage system option records |
| `Update` | `() As Integer` | Persist changes |
| `GetOptionValue` | `(ByVal v_iOptionNumber As Integer, ByRef r_vValue As Object) As Integer` | Get a system option value by number |
| `GetOptionValueAsString` | `(ByVal v_iOptionNumber As Integer, ByRef r_sValue As String) As Integer` | Get option value as string |
| `GetOptionValueAsInteger` | `(ByVal v_iOptionNumber As Integer, ByRef r_iValue As Integer) As Integer` | Get option value as integer |
| `GetOptionValueAsBoolean` | `(ByVal v_iOptionNumber As Integer, ByRef r_bValue As Boolean) As Integer` | Get option value as boolean |
| `SetOptionValue` | `(ByVal v_iOptionNumber As Integer, v_vValue As Object) As Integer` | Set a system option value |
| `GetAllOptions` | `(ByRef r_vOptions(,) As Object) As Integer` | Get all system options |
| `GetOptionsByCategory` | `(ByVal v_sCategory As String, ByRef r_vOptions(,) As Object) As Integer` | Get options filtered by category |
| `GetOptionDefinition` | `(ByVal v_iOptionNumber As Integer, ByRef r_vDefinition(,) As Object) As Integer` | Get option metadata (name, type, category, description) |
| `RefreshCache` | `() As Integer` | Refresh in-memory option cache |
| `GetBranchOptions` | `(ByVal v_sBranchCode As String, ByRef r_vOptions(,) As Object) As Integer` | Get branch-specific option overrides |
| `SetBranchOption` | `(ByVal v_sBranchCode As String, v_iOptionNumber As Integer, v_vValue As Object) As Integer` | Set branch-specific option |
| `GetProductOptions` | `(ByVal v_lProductId As Integer, ByRef r_vOptions(,) As Object) As Integer` | Get product-specific options |
| `GetLookupValues` | `(ByRef iLookupType As Integer, ...) As Integer` | Get lookup values |
| `SearchAll` | `(ByRef r_vResultArray(,) As Object) As Integer` | Search all options |
| `GetSummary` | `(ByRef vSummaryArray As Object) As Integer` | Get option summary |
| `SetKeys` / `GetKeys` | Standard navigator keys | Key management |
| `CheckID` | `(ByRef vID As Object) As Integer` | Validate option ID |
| `Cancel` | `() As Integer` | Cancel changes |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|--------|
| `spe_system_options_sel` | `GetDetails` (data) | Select system option |
| `spe_system_options_add` | `EditAdd` (data) | Add system option |
| `spe_system_options_upd` | `EditUpdate` (data) | Update system option |
| `spe_system_options_del` | `EditDelete` (data) | Delete system option |
| `spe_system_options_saa` | `GetDetails` (data) | Select all |
| `spe_system_options_check_id` | `CheckID` (data) | Check ID |
| `spu_sir_get_option_value` | `GetOptionValue` | Get option value |

**Note:** Many option reads use embedded/inline SQL (`SELECT option_value FROM system_options WHERE option_number = @n`) for performance rather than stored procedures.

**References:** `dSIROptions` (data layer), SharedFiles

---

### TaskScheduler
**Directory:** `TaskScheduler/`
**COM Name:** `bSIRTaskScheduler`
**Purpose:** Manages scheduled task configuration — batch processes, frequency parameters, and execution scheduling.

**Business Methods — `bSIRTaskScheduler`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `Start` | `() As Integer` | Navigator entry point — dispatches scheduled task execution |
| `GetScheduledTasks` | `(ByRef r_vTasks(,) As Object) As Integer` | Get all scheduled tasks |
| `AddScheduledTask` | `(ByRef v_vTaskData(,) As Object) As Integer` | Add a new scheduled task |
| `UpdateScheduledTask` | `(ByRef v_vTaskData(,) As Object) As Integer` | Update scheduled task configuration |
| `DeleteScheduledTask` | `(ByVal v_lTaskId As Integer) As Integer` | Delete a scheduled task |
| `ExecuteTask` | `(ByVal v_lTaskId As Integer) As Integer` | Execute a task immediately |
| `GetTaskHistory` | `(ByVal v_lTaskId As Integer, ByRef r_vHistory(,) As Object) As Integer` | Get task execution history |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|--------|
| `spu_sir_task_scheduler_sel` | `GetScheduledTasks` | Select tasks |
| `spu_sir_task_scheduler_add` | `AddScheduledTask` | Add task |
| `spu_sir_task_scheduler_upd` | `UpdateScheduledTask` | Update task |
| `spu_sir_task_scheduler_del` | `DeleteScheduledTask` | Delete task |
| `spu_sir_task_scheduler_execute` | `ExecuteTask` | Execute task |
| `spu_sir_task_scheduler_history` | `GetTaskHistory` | Get history |
| `spu_sir_task_scheduler_status_upd` | internal | Update task status |
| `spu_sir_task_scheduler_next_run` | internal | Calculate next run |

**References:** `bNavigatorV3` (workflow engine)

---

### Text Files
**Directory:** `Text Files/`
**COM Name:** `bSIRTextFile`
**Purpose:** Manages text file attachments (notes, memos, free-text documents) linked to parties, policies, and claims. Supports create, copy, move between branches, and event logging.

**Business Methods — `bSIRTextFile`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard; creates bSIREvent |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `Start` | `() As Integer` | Navigator entry point |
| `GetDetails` / `GetNext` | Standard entity iteration | Navigate text file records |
| `EditAdd` / `EditUpdate` / `EditDelete` | Standard entity CRUD | Manage text file records |
| `Update` | `() As Integer` | Persist changes with event logging |
| `Cancel` | `() As Integer` | Cancel changes |
| `CheckID` | `(ByRef vID As Object) As Integer` | Validate text file ID |
| `DirectAdd` | `(Optional params... As Object) As Integer` | Direct add without edit cycle |
| `DirectUpdate` | `(Optional params... As Object) As Integer` | Direct update |
| `DirectDelete` | `(Optional params... As Object) As Integer` | Direct delete |
| `GetText` | `(ByVal v_lTextFileId As Integer, ByRef r_sText As String) As Integer` | Get text content by ID |
| `UpdateText` | `(ByVal v_lTextFileId As Integer, v_sText As String) As Integer` | Update text content |
| `SearchAll` | `(ByRef r_vResultArray(,) As Object) As Integer` | Search all text files |
| `GetTextFileTypes` | `(ByRef r_vTypes(,) As Object) As Integer` | Get text file type definitions |
| `GetTextFilesForPolicy` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vTextFiles(,) As Object) As Integer` | Get text files linked to a policy |
| `GetTextFilesForClaim` | `(ByVal v_lClaimCnt As Integer, ByRef r_vTextFiles(,) As Object) As Integer` | Get text files linked to a claim |
| `CopyTextFiles` | `(ByVal v_lSourceInsFileCnt As Integer, v_lTargetInsFileCnt As Integer) As Integer` | Copy text files from one policy to another |
| `CreateEvent` | `(ByRef r_lEventCnt As Integer, ...) As Integer` | Create event via bSIREvent |
| `GetLookupValues` | Standard | Get lookup values |
| `SetKeys` / `GetKeys` | Standard navigator keys | Key management |
| `GetSummary` | `(ByRef vSummaryArray As Object) As Integer` | Get summary |

**Sub-Component — `bSIRTextFileDescription`:**
| Method | Description |
|--------|-------------|
| `GetDetails` / `GetNext` | Navigate text file description records |
| `EditAdd` / `EditUpdate` / `EditDelete` | Text file description CRUD |
| `Update` | Persist changes |
| `SearchAll` | Search descriptions |

**Automated Class — `bSIRTextFileAutomated`:**
| Method | Description |
|--------|-------------|
| `Initialise` / `SetProcessModes` / `Start` | Navigator automation |
| `GetSummary` / `SetKeys` / `GetKeys` | Key management |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|--------|
| `spe_text_file_sel` | `GetDetails` (data) | Select text file |
| `spe_text_file_add` | `EditAdd` (data) | Add text file |
| `spe_text_file_upd` | `EditUpdate` (data) | Update text file |
| `spe_text_file_del` | `EditDelete` (data) | Delete text file |
| `spe_text_file_saa` | `GetDetails` (data) | Select all |
| `spe_text_file_check_id` | `CheckID` (data) | Check ID |
| `spu_sir_get_text_files_for_policy` | `GetTextFilesForPolicy` | Get policy text files |
| `spu_sir_get_text_files_for_claim` | `GetTextFilesForClaim` | Get claim text files |
| `spu_sir_copy_text_files` | `CopyTextFiles` | Copy text files |

**References:** `bSIREvent` (event logging), `dSIRTextFile` (data layer), `bSIRTextFileDescription` (descriptions)

---

### Transactions
**Directory:** `Transactions/`
**COM Name:** `bSIRTransactions`
**Purpose:** Creates financial transactions (premium postings, credit control items) when policies are bound or adjusted. Handles nominal ledger posting, introducer commissions, export folder creation, and cash deposit linking. Called internally by `bSIRInsuranceFile` during bind/MTA/renewal.

**Business Methods — `bPMBTransactions`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `Start` | `() As Integer` | Navigator entry point |
| `GetDetails` / `GetNext` | Standard entity iteration | Navigate transaction records |
| `EditAdd` / `EditUpdate` / `EditDelete` | Standard entity CRUD | Manage transactions |
| `Update` | `() As Integer` | Persist changes |
| `PostTransaction` | `(ByRef v_vTransData(,) As Object) As Integer` | Post a financial transaction |
| `ReverseTransaction` | `(ByVal v_lTransactionCnt As Integer) As Integer` | Reverse a posted transaction |
| `GetTransactionHistory` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vHistory(,) As Object) As Integer` | Get transaction history for policy |
| `GetTransactionTypes` | `(ByRef r_vTypes(,) As Object) As Integer` | Get available transaction types |
| `ValidateTransaction` | `(ByRef v_vTransData(,) As Object, ByRef r_vErrors(,) As Object) As Integer` | Validate transaction data |
| `GetAccountBalance` | `(ByVal v_lAccountCnt As Integer, ByRef r_dBalance As Decimal) As Integer` | Get account balance |
| `SearchAll` | `(ByRef r_vResultArray(,) As Object) As Integer` | Search all transactions |
| `GetLookupValues` | Standard | Get lookup values |
| `SetKeys` / `GetKeys` | Standard navigator keys | Key management |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|--------|
| `spe_transaction_sel` | `GetDetails` (data) | Select transaction |
| `spe_transaction_add` | `EditAdd` (data) | Add transaction |
| `spe_transaction_upd` | `EditUpdate` (data) | Update transaction |
| `spe_transaction_del` | `EditDelete` (data) | Delete transaction |
| `spe_transaction_saa` | `GetDetails` (data) | Select all |
| `spe_transaction_check_id` | `CheckID` (data) | Check ID |
| `spu_pmb_post_transaction` | `PostTransaction` | Post transaction |
| `spu_pmb_reverse_transaction` | `ReverseTransaction` | Reverse transaction |
| `spu_pmb_get_transaction_history` | `GetTransactionHistory` | Get history |
| `spu_pmb_get_transaction_types` | `GetTransactionTypes` | Get types |
| `spu_pmb_validate_transaction` | `ValidateTransaction` | Validate |
| `spu_pmb_get_account_balance` | `GetAccountBalance` | Get balance |

**References:** `dPMBTransactions` (data layer), Orion accounting

---

### Unposted Transactions
**Directory:** `Unposted Transactions/`
**COM Name:** `bSIRUnpostedTransactions`
**Purpose:** Manages the export of unposted transactions to the Orion accounting system. Gets pending transactions and marks them as exported.

**Business Methods — `bPMBUnpostedTransactions`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `GetUnpostedTransactions` | `(ByRef r_vTransactions(,) As Object) As Integer` | Retrieve all unposted transactions pending processing |
| `PostAllPending` | `() As Integer` | Batch post all pending unposted transactions |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|--------|
| `spu_pmb_get_unposted_transactions` | `GetUnpostedTransactions` | Get unposted |
| `spu_pmb_post_pending_transactions` | `PostAllPending` | Post pending |

**References:** `bPMBTransactions` (transaction posting)

---

## Additional Business Components

### Sirius Back Office Link
**Directory:** `Sirius Back Office Link/`
**Project:** `bSIRIUSLink`
**Purpose:** Links the GIS/Gemini back-office system with the Sirius insurance processing layer. Used during renewals to retrieve claims data and create insurance file versions.

**Sub-Components (3 classes in single project):**

#### `bSIRIUSLink` — Core Sirius Integration
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `GetInsuranceFileDetails` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vDetails(,) As Object) As Integer` | Get insurance file details for linking |
| `GetPartyDetails` | `(ByVal v_lPartyCnt As Integer, ByRef r_vDetails(,) As Object) As Integer` | Get party details for linking |
| `UpdateLink` | `(ByRef v_vLinkData(,) As Object) As Integer` | Update cross-system link |
| `GetLinkedRecords` | `(ByRef r_vLinked(,) As Object) As Integer` | Get all linked records |
| `CreateLink` | `(ByRef v_vLinkData(,) As Object) As Integer` | Create new cross-system link |
| `DeleteLink` | `(ByVal v_lLinkId As Integer) As Integer` | Remove a link |
| `ValidateLink` | `(ByRef v_vLinkData(,) As Object, ByRef r_bValid As Boolean) As Integer` | Validate link data |

#### `bSIRIUSLinkClaims` — Claims Integration
| Method | Description |
|--------|-------------|
| `GetClaimDetails` | Get claim details for linking |
| `GetClaimPerils` | Get claim perils for linking |
| `GetClaimReserves` | Get claim reserve data |
| `UpdateClaimLink` | Update claim cross-system link |
| `GetLinkedClaims` | Get all linked claims |

#### `bSIRIUSLinkRenewals` — Renewals Integration
| Method | Description |
|--------|-------------|
| `GetRenewalDetails` | Get renewal details for linking |
| `UpdateRenewalLink` | Update renewal cross-system link |
| `GetLinkedRenewals` | Get all linked renewals |
| `ProcessRenewalSync` | Synchronise renewal data across systems |

**Stored Procedures (30+):**
| SP | Called By | Purpose |
|----|----------|--------|
| `spu_sir_link_get_insurance_file` | `GetInsuranceFileDetails` | Get insurance file for link |
| `spu_sir_link_get_party` | `GetPartyDetails` | Get party for link |
| `spu_sir_link_create` | `CreateLink` | Create link |
| `spu_sir_link_update` | `UpdateLink` | Update link |
| `spu_sir_link_delete` | `DeleteLink` | Delete link |
| `spu_sir_link_get_linked_records` | `GetLinkedRecords` | Get linked records |
| `spu_sir_link_claims_get` | `GetClaimDetails` | Get claim details |
| `spu_sir_link_claims_perils` | `GetClaimPerils` | Get claim perils |
| `spu_sir_link_renewals_get` | `GetRenewalDetails` | Get renewal details |
| `spu_sir_link_renewals_sync` | `ProcessRenewalSync` | Sync renewals |

**References:** `bSIRInsuranceFile`, `bSIRFindInsurance`, `bSIRParty`

---

### Future Address Update
**Directory:** `Future Address Update/`
**Project:** `bSIRFutureAddressUpdate`
**Purpose:** Batch job that processes scheduled future address changes — updates party addresses when their effective date is reached.

**Business Methods — `bSIRFutureAddressUpdate`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `Start` | `() As Integer` | Navigator entry — runs batch update of future-dated addresses |
| `ProcessFutureAddresses` | `() As Integer` | Find addresses with effective dates <= today and apply them via `bSIRParty` |

**Stored Procedures:** Delegates to `bSIRParty` stored procedures for actual address updates

**References:** `bSIRParty` (address update), `bNavigatorV3` (batch scheduling)

---

### Gemini List Manager
**Directory:** `Gemini List Manager/`
**Projects:** `bGEMListCustom`, `bGEMListManager`, `bGEMListMgr`, `bGEMLists`, `bGEMListUpdate`, `bGEMListUser`, `bGEMLookup`
**Purpose:** Manages GIS/Gemini lookup lists — custom lists, standard lists, user-defined lists, and list-to-list synchronisation. Used by the Product Builder and GIS screens.

**Sub-Components (7 projects):**

#### `bGEMListMgr` — Core List Manager
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `GetListDefinitions` | `(ByRef r_vLists(,) As Object) As Integer` | Get all list definitions |
| `GetListData` | `(ByVal v_lListId As Integer, ByRef r_vData(,) As Object) As Integer` | Get data for a specific list |
| `UpdateListData` | `(ByVal v_lListId As Integer, ByRef v_vData(,) As Object) As Integer` | Update list data |
| `RefreshList` | `(ByVal v_lListId As Integer) As Integer` | Refresh a cached list |

#### `bGEMListCustom` — Custom List Implementation
| Method | Description |
|--------|-------------|
| `GetCustomList` | Get custom-defined list data |
| `UpdateCustomList` | Update custom list |
| `CreateCustomList` | Create new custom list definition |

#### `bGEMLists` — List Collection Manager
| Method | Description |
|--------|-------------|
| `GetAllLists` | Get all available lists |
| `GetListByName` | Get list by name |
| `GetListByCategory` | Get lists by category |

#### `bGEMListManager` — Manager Façade
| Method | Description |
|--------|-------------|
| `Initialise` | Standard — delegates to child components |
| `GetList` | Get list by ID — dispatches to appropriate list type handler |
| `UpdateList` | Update list — dispatches to handler |

#### `bGEMListUpdate` — List Update Processing
| Method | Description |
|--------|-------------|
| `Start` | Navigator entry point for batch list updates |
| `ProcessUpdates` | Process pending list updates |
| `ValidateUpdate` | Validate list update data |

#### `bGEMListUser` — User-Specific Lists
| Method | Description |
|--------|-------------|
| `GetUserLists` | Get lists for current user |
| `GetUserListPreferences` | Get user list display preferences |
| `UpdateUserListPreferences` | Update user preferences |

#### `bGEMLookup` — List Lookup Helper
| Method | Description |
|--------|-------------|
| `GetLookupValue` | Get lookup value by key |
| `GetLookupList` | Get full lookup list |
| `RefreshLookupCache` | Refresh lookup cache |

**References:** GIS Combined, `bGISListManager`

---

### Account Transaction Batch
**Directory:** `Account Transaction Batch/`
**Project:** `bSIRAccountTransBatch`
**Purpose:** Batch processing wrapper for accounting transaction generation. Entry point for scheduled batch runs.

**Business Methods — `bACTTransactionBatch`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `Start` | `() As Integer` | Navigator entry — stub implementation (placeholder for batch transaction processing) |

**Stored Procedures:** None (stub component)

**References:** Orion accounting framework

---

### DebugTimings
**Directory:** `DebugTimings/`
**Project:** `bSIRDebugTimings`
**Purpose:** Performance timing/profiling utility for measuring component execution times. Wraps timing data in a collection for reporting.

**Business Methods — `bSIRDebugTimings`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `StartTiming` | `(ByVal v_sLabel As String) As Integer` | Start a named timing block for performance diagnostics |
| `StopTiming` | `(ByVal v_sLabel As String) As Integer` | Stop a timing block and record elapsed time |
| `GetTimings` | `(ByRef r_vTimings(,) As Object) As Integer` | Get all recorded timings |
| `ResetTimings` | `() As Integer` | Clear all timing data |

**Stored Procedures:** None — diagnostics only, in-memory timing

**References:** Development/diagnostics utility

---

## Utility / UI Components

> **For detailed interface and user control reference**, see `.github/docs/back-office-ui-controls-reference.md`.
> That file documents all ~90 `iPMB*`, `iPMU*`, `iSIR*`, `iGEM*`, `uct*`, and `PMU*` projects with methods, business component cross-references, and hosting relationships.

Interface and user control projects provide the back-office desktop UI layer. Key highlights:

| Category | Count | Examples |
|----------|-------|---------|
| **Broker interfaces (`iPMB*`)** | ~45 | `iPMBFindInsurance`, `iPMBFindParty`, `iPMBPartyPC`, `iPMBDocManager`, `iPMBFinancePlanMaint` |
| **Underwriting interfaces (`iPMU*`)** | ~5 | `iPMUCopyFile`, `iPMUDocConversion`, `iPMUReportGroup` |
| **Sirius interfaces (`iSIR*`)** | ~15 | `iSIRBankGuarantee`, `iSIRCashDeposit`, `iSIRPolicySummary`, `iSIRPartySummary` |
| **GIS interfaces (`iGEM*`)** | ~7 | `iGEMListManager`, `iGEMListMgr`, `iGEMLookup` |
| **User controls (`uct*`, `PMU*`)** | ~30 | `uctPartyPCControl`, `uctListPolicyControl`, `uctListClaimControl`, `PMUPolicyControl1` |

---

## Component Size Summary

| Component | Methods | Stored Procedures | References |
|-----------|---------|-------------------|------------|
| **Instalments** | 262 | 279 | 24 |
| **Party** | 217 | 242 | 10 |
| **Document Production** | 142 | 85 | 15 |
| **Insurance File** | 143 | 89 | 9 |
| **Find Insurance** | 83 | 51 | 4 |
| **Event** | 48 | 8 | 1 |
| **Crystal Reports** | 45 | 18 | 2 |
| **Find Party** | 42 | 18 | 2 |
| **Text Files** | 42 | 9 | 3 |
| **Contact** | 31 | 6 | 0 |
| **Conviction** | 34 | 6 | 0 |
| **Lifestyle** | 33 | 6 | 0 |
| **Bank Guarantee** | 32 | 29 | 2 |
| **Insurance Folder** | 32 | 9 | 0 |
| **Insurance File System** | 30 | 8 | 0 |
| **Cash Deposit** | 29 | 27 | 5 |
| **System Options** | 28 | 8 | 3 |
| **Orion Link** | 27 | 5 | 7 |
| **Policy Number Maintenance** | 23 | 21 | 3 |
| **Address** | 36 | 11 | 1 |
| **HandlerTransfer** | 15 | 1 | 3 |
| **Product Options** | 13 | 7 | 0 |
| **TaskScheduler** | 12 | 8 | 0 |
| **Report Scheduler** | 12 | 8 | 0 |
| **Report Group** | 10 | 8 | 0 |
| **GetChangeReason** | 10 | 3 | 1 |
| **MediaTypeValidation** | 9 | 4 | 1 |
| **ExternalWorkFlowConfiguration** | 9 | 0 | 1 |
| **Batch Scheduler** | 7 | 4 | 0 |
| **Export Control** | 7 | 1 | 0 |
| **Unposted Transactions** | 7 | 2 | 0 |
| **Contact Type** | 5 | 3 | 0 |
| **OnlinePartyMaintenance** | 4 | 1 | 0 |
| **Party Loyalty Scheme** | 11 | 4 | 0 |
| **Transactions** | 0 | 16 | 0 |

---

## Cross-Component Dependency Map

Most-referenced components (called by other Back Office Core components):

| Component | Referenced By |
|-----------|---------------|
| `bSIREvent` | Address, Bank Guarantee, Cash Deposit, Document Production, Find Party, HandlerTransfer, Instalments, Insurance File, Party, Text Files |
| `bSIROptions` | Address, Document Production, Find Insurance, Insurance File, Party, System Options |
| `bSIRInsuranceFile` | Find Insurance, Instalments, Insurance File (self), Party |
| `bSIRParty` | Document Production, Instalments, Orion Link, Party (self) |
| `bSIRAddress` / `bSIRContact` | Party |
| `bSIRRiskScreen` | Insurance File, Party |
| `bSIRSharepoint` / `bSIRDOCAPI` | Insurance File, Party, Document Production |
| `bACTAccount` / `bACTLedger` / `bACTExplorer` | Cash Deposit, Orion Link |
| `bACTCurrencyConvert` | Instalments, Party |
| `bACTUserAuthorities` | Instalments, MediaTypeValidation |

### External Component Dependencies (outside Back Office Core)

These components from other subsystems are called by Back Office Core:

| External Component | Subsystem | Called By |
|-------------------|-----------|----------|
| `bSIRAutoMTA` | Sirius For Underwriting | Instalments |
| `bSIRRenSelection` | Sirius For Underwriting | Find Insurance, Insurance File |
| `bSIRProduct` | Sirius For Underwriting | Insurance File |
| `bSIRRITax` | Sirius For Underwriting | Instalments, Insurance File |
| `bSIRIUSLink` | Sirius For Underwriting | Find Insurance |
| `bGISListManager` | GIS Combined | Document Production |
| `bACTPeriod` | Orion | Policy Number Maintenance |
| `bACTFinanceSpoke` | Orion | Instalments |
| `bACTDocument` / `bACTTransdetail` | Orion | Instalments |
