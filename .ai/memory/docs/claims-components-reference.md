# Claims Business Components Reference

> **Sirius For Insurance — Claims Module**
> Source: `Claims\Components\` — 35 business components across 34 directories.
> Each component resides under `Claims\Components\{Folder}\Business\{ComponentName}\`.

---

## Overview

The Claims module manages the full claims lifecycle: opening claims, managing perils/reserves/payments/receipts/recoveries, reinsurance, coinsurance, status changes, document production, and financial summaries.

### Architecture

```
SAM WCF Service → Claims Business Component (bCLM*/bOpen*/bSIR*)
                    → Data Component (dCLM*) → SQL Server Stored Procedures
                    → Back Office Link (bBOLink) → Policy Data
                    → Orion Accounting (bACT*) → Financial Posting
```

### Common Patterns
- All components implement `SSP.S4I.Interfaces.IBusiness` with `Initialise`/`SetProcessModes`/`Dispose`
- Entity/Collection pattern: `*Cls.vb` (entity) + `*s.vb` (collection) + `*Business.vb` (facade)
- Automated/NavigatorV3 classes handle batch/navigator integration
- SQL constants in `*SQL.vb` or `*BusinessSQL.vb` files
- Hungarian notation: `v_` parameters, `r_` return refs, `s` strings, `l`/`i`/`n` integers

---

## Project Inventory

| # | Directory | Project | Purpose |
|---|-----------|---------|---------|
| 1 | Authorise Payments | `bCLMAuthorisePayments` | Payment authorisation workflow with step-based approval |
| 2 | Back Office Link | `bBackOfficeLink` | Policy lookup bridge to underwriting back office |
| 3 | Case | `bCLMCase` | Case management — group claims, versioning, GIS links |
| 4 | Change Claim Status | `bCLMChangeClaimStatus` | Claim status transitions, transaction raising, cash list |
| 5 | Check Deferred RI | `bCLMCheckDeferredRI` | Check deferred RI status on claim risks |
| 6 | Check Unpaid Premium | `bCLMCheckUnpaidPremium` | Verify premium payments for claim policy |
| 7 | Claim Address | `bCLMAddress` | Claim address CRUD with contacts |
| 8 | Claim Letter | `bCLMGetClaimLetter` | Claim letter merge data retrieval |
| 9 | Claim Party | `bCLMClaimParty` | Claim-to-party link management |
| 10 | Claim Party Link Maintenance | `bCLMClaimPartyLink` | Add/delete claim party links |
| 11 | Close Claim | `bCLMCloseClaim` | Claim closure — status check, reserve/recovery validation |
| 12 | Coinsurance Recoveries | `bCLMCoinsuranceRecoveries` | Coinsurance share management for claims |
| 13 | Define Fields | `bCLMDefnFlds` | User-defined risk/peril data fields |
| 14 | Document Production | `bCLMGetClaimDocument` | Claim document data for merge/production |
| 15 | Financial Summary | `bCLMFinSumm` | Claim financial summary — reserves, perils, recoveries |
| 16 | Find Claim | `bCLMFindClaim` | Claim search, copy, versioning, GIS integration |
| 17 | Find Party | `bCLMFindParty` | Party search for claims |
| 18 | Generic Peril | `bCLMPeril` | **Core peril engine** — reserves, payments, receipts, recoveries, tax, Orion posting |
| 19 | Information Checklist | `bCLMInfoChklst` | Expert services/information checklist CRUD |
| 20 | Loss Schedule | `bCLMLossSchedule` | Loss schedule types and item details |
| 21 | Open Claim | `bOpenClaim` | **Claim creation** — policy lookup, address, risk, numbering, duplicate check |
| 22 | Payment Method | `bCLMPaymentMethod` | Payment media types, currency, rejection |
| 23 | Peril Type | `bCLMPerilType` | Peril type definitions |
| 24 | Peril Type Reserve Type | `bCLMPerilReserveType` | Peril-to-reserve-type mappings |
| 25 | Recovery | `bCLMRecovery` | Recovery management — salvage/TP, co/reinsurance, tax |
| 26 | Reinsurance Recoveries | `bCLMReinsurance` | Claim RI arrangement management |
| 27 | Reinsurance Recoveries | `bCLMReinsuranceRI2007` | RI 2007 model — enhanced with broker participants |
| 28 | Reserve Definition | `bCLMResvDefn` | Reserve type definitions CRUD |
| 29 | Risk Details | `bCLMRiskDetails` | **Claim risk management** — perils, parties, GIS, close/reopen |
| 30 | Risk Type | `bCLMRiskType` | Risk type lookups and screen assignment |
| 31 | Risk Type Information Checklist | `bCLMRiskTypeInfoChecklist` | Risk type → experience series mapping |
| 32 | Salvage Recovery | `bCLMSalvageRecovery` | Salvage recovery CRUD with co/reinsurance splits |
| 33 | Third Party Recovery | `bCLMThirdPartyRecovery` | Third party recovery CRUD with co/reinsurance splits |
| 34 | User Authority Levels | `bSIRCheckAuthorityLevel` | VBScript rule-based authority checking |

---

## Component Reference

### 1. bCLMAddress
**Directory:** `Claim Address/`
**Project:** `bCLMAddress`
**Purpose:** Manages claim address records — add, delete, check, retrieve and navigate address data for claims.

**Business Methods — Business (bSIRAddressBusiness.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises the Business object, sets up database connection and creates SIRAddresss collection |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes the Business object and closes database if owned |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process mode properties (task, navigate, process mode, transaction type, effective date) |
| `DirectAdd` | `Public Function DirectAdd(Optional ByRef vAddressCnt As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing) As Integer` | Adds a single SIRAddress directly into the database (not to collection) |
| `DirectDelete` | `Public Function DirectDelete(Optional ByRef vAddressCnt As Object = Nothing) As Integer` | Deletes a single SIRAddress directly from the database (not from collection) |
| `CheckID` | `Public Function CheckID(ByRef vID As Object) As Integer` | Checks if the supplied ID is a valid address record |
| `GetDetails` | `Public Function GetDetails(Optional ByRef vLockMode As gPMConstants.PMELockMode = 0, Optional ByRef vAddressCnt As Integer = 0) As Integer` | Gets required SIRAddresss and populates the collection |
| `GetNext` | `Public Function GetNext(Optional ByRef vAddressCnt As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vAddressID As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vModifiedByID As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vAddressUsageTypeID As Object = Nothing) As Integer` | Gets nextSIRAddress from collection and returns properties via ByRef params |

**Entity Methods — SIRAddress (bSIRAddressCls.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal v_sUsername As String, ByVal v_sPassword As String, ByVal v_iUserID As Integer, ByVal v_iSourceID As Integer, ByVal v_iLanguageID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_iLogLevel As Integer, ByVal v_sCallingAppName As String, Optional ByVal v_vDatabase As dPMDAO.Database = Nothing) As Integer` | Initialises entity class, creates data component instance |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes the entity and data classes |
| `GetDefaults` | `Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vAddressCnt As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vAddressID As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vModifiedByID As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vAddressUsageTypeID As Object = Nothing) As Integer` | Returns default values for SIRAddress attributes |
| `SetProperties` | `Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vAddressCnt As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vAddressID As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vModifiedByID As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vAddressUsageTypeID As Object = Nothing, Optional ByRef vUpdateGlobalAddress As Object = Nothing) As Integer` | Sets SIRAddress property values, validates, and tracks data changes |
| `GetProperties` | `Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vAddressCnt As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vAddressID As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vModifiedByID As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vAddressUsageTypeID As Object = Nothing) As Integer` | Returns SIRAddress property values via ByRef params |
| `SelectItem` | `Public Function SelectItem() As Integer` | Reads base details from the database for AddressCnt |

**Collection Methods — SIRAddresss (bSIRAddresss.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `New` | `Public Sub New()` | Constructor, initialises internal VB6 Collection |
| `Initialise` | `Public Function Initialise() As Integer` | Initialisation entry point |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes collection resources |
| `Add` | `Public Function Add(ByRef oNewSIRAddress As bCLMAddress.SIRAddress) As Integer` | Adds a single SIRAddress into the collection |
| `Count` | `Public Function Count() As Integer` | Returns number of SIRAddresss in the collection |
| `Delete` | `Public Sub Delete(ByRef vKey As Integer)` | Deletes a SIRAddress from the collection by key |
| `Item` | `Public Function Item(ByRef vKey As Integer) As bCLMAddress.SIRAddress` | Returns the selected SIRAddress from the collection |
| `DeleteAll` | `Public Sub DeleteAll()` | Deletes all SIRAddresss from the collection |
| `Clear` | `Public Sub Clear()` | Clears the SIRAddress collection |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spe_Address_saa` | `GetDetails` | Select all SIRAddress records |
| `spe_SIRAddress_check_id` | `CheckID` | Check if an address ID is valid |
| `sp_get_party_address` | `GetPartyAddress` | Get party address by address usage type |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `dCLMAddress.SIRAddress` | `SIRAddress` (entity) | Data layer for address persistence |

---

### 2. bCLMAuthorisePayments
**Directory:** `Authorise Payments/`
**Project:** `bCLMAuthorisePayments`
**Purpose:** Manages claim payment authorisation workflow — referred payment listing, multi-step approval/decline processing, user authority checks, and work task integration.

**Business Methods — Business (bCLMAuthorisePaymentsCls.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises business object, database, and BPMLOOKUP |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes lookup and database |
| `New` | `Public Sub New()` | Constructor |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process mode properties |
| `BeginTrans` | `Public Function BeginTrans() As Integer` | Begins a database transaction |
| `CommitTrans` | `Public Function CommitTrans() As Integer` | Commits a database transaction |
| `RollbackTrans` | `Public Function RollbackTrans() As Integer` | Rolls back a database transaction |
| `getUnderwritingOrAgency` | `Public Function getUnderwritingOrAgency() As Integer` | Determines if business is underwriting or agency |
| `GetReferredList` | `Public Function GetReferredList(ByRef r_vResultArray(,) As Object, Optional ByRef other_party_id As Integer = 0) As Integer` | Gets list of all referred claim payments |
| `CreateEvent` | `Public Function CreateEvent(ByRef v_lEventType As Integer, ByRef v_sDescription As String, ByRef v_lClaimId As Integer, ByRef v_sOriginalUser As String, ByRef v_sMode As String) As Integer` | Creates an authorisation/decline event record |
| `ProcessDecline` | `Public Function ProcessDecline(ByRef v_lClaimId As Integer, ByVal v_lPaymentId As Integer) As gPMConstants.PMEReturnCode` | Sets payment status to declined |
| `ProcessAuthorise` | `Public Function ProcessAuthorise(ByRef v_lClaimId As Integer) As gPMConstants.PMEReturnCode` | Sets payment status to authorised |
| `CheckUserGroup` | `Public Function CheckUserGroup() As Integer` | Checks if current user is in CLMSUPER or SYSADMIN group |
| `ProcessWTM` | `Public Function ProcessWTM(ByVal v_lClaimId As Integer) As Integer` | Removes work task manager entries for authorised claims |
| `AddTaskToWorkManager` | `Public Function AddTaskToWorkManager(ByRef r_lPMWrkTaskInstanceCnt As Integer, ByVal v_sCustomer As String, ByVal v_sDescription As String, ByVal v_dtTaskDueDate As Date, Optional ByVal v_lPMWrkTaskID As Integer = 0, Optional ByVal v_sTaskCode As String = "", Optional ByVal v_lPMWrkTaskGroupID As Integer = 0, Optional ByVal v_sTaskGroupCode As String = "", Optional ByVal v_iUserID As Integer = 0, Optional ByVal v_sUserGroupCode As String = "PURCLDGR", Optional ByVal v_vKeyArray As Object = Nothing, Optional ByVal v_iIsUrgent As Integer = 0, Optional ByVal v_iIsVisible As Integer = gPMConstants.PMEReturnCode.PMTrue, Optional ByVal v_iTaskStatus As Integer = gPMConstants.PMEWrkManTaskStatus.pmeWMTSNew) As Integer` | Adds a task to the work task manager |
| `GetClaimPaymentAccountsDetails` | `Public Function GetClaimPaymentAccountsDetails(ByVal v_lClaimPaymentId As Integer, ByRef r_vResults(,) As Object) As Integer` | Returns claim payment accounts details for session |
| `GetClaimVersionDescription` | `Public Function GetClaimVersionDescription(ByVal v_lClaimId As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets claim version description |
| `AddInputParameter` | `Public Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer` | Adds a database input parameter |
| `GetClaimStatus` | `Public Function GetClaimStatus(ByVal v_lClaimId As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets claim status |
| `GetReferredClaimStatus` | `Public Function GetReferredClaimStatus(ByVal v_lClaimPaymentId As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets referred claim payment status |
| `Update_Claim_Status` | `Public Function Update_Claim_Status(ByVal iClaimid As Integer) As Integer` | Updates claim status |
| `GetCashListItemClaimLinkDetails` | `Public Function GetCashListItemClaimLinkDetails(ByVal v_lClaimPaymentId As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets cash list item claim link details |
| `SetClaimPaymentRecommendStatus` | `Public Function SetClaimPaymentRecommendStatus(ByVal v_lClaimId As Integer, ByVal v_iStatus As Integer, ByVal v_iUserID As Integer) As Integer` | Sets claim payment recommendation status |
| `GetReserveTotalForClaimPayment` | `Public Function GetReserveTotalForClaimPayment(ByVal v_lClaimPaymentId As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets reserve total for a claim payment |
| `GetTransDetailFromCashListItem` | `Public Function GetTransDetailFromCashListItem(ByVal v_lCashListItemId As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets transaction detail from cash list item |
| `GetAlreadyReferredClaimStatus` | `Public Function GetAlreadyReferredClaimStatus(ByVal v_lClaimPaymentId As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets already-referred claim payment status |
| `GetUserOtherParty` | `Public Function GetUserOtherParty(ByVal iUserID As Integer, ByRef r_vResultArray(,) As Object) As Long` | Gets user other party ID |

**StepAuthorization Methods — StepAuthorization (bStepAuthorization.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises step authorization object with Orion product family database |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes lookup and database |
| `New` | `Public Sub New()` | Constructor |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process mode properties |
| `BeginTrans` | `Public Function BeginTrans() As Integer` | Begins a database transaction |
| `CommitTrans` | `Public Function CommitTrans() As Integer` | Commits a database transaction |
| `RollbackTrans` | `Public Function RollbackTrans() As Integer` | Rolls back a database transaction |
| `getUnderwritingOrAgency` | `Public Function getUnderwritingOrAgency() As Integer` | Determines if business is underwriting or agency |
| `CheckUserGroup` | `Public Function CheckUserGroup(ByRef r_bUserInGroup As Boolean) As Integer` | Checks if current user belongs to the approval group |
| `ProcessApproval` | `Public Function ProcessApproval() As Integer` | Processes approval step (approved=1) |
| `ProcessDecline` | `Public Function ProcessDecline() As Integer` | Processes decline step (approved=0) |
| `GetStepGroupCode` | `Public Function GetStepGroupCode(ByRef r_sGroupCode As String, Optional ByRef r_sErrorMessage As String = "") As Integer` | Gets the user group code for the current approval step |
| `GetStepDetails` | `Public Function GetStepDetails(ByVal v_lApprovalStep As Integer, ByRef r_vStepDetails(,) As Object) As Integer` | Gets step details from debtor groups for a given step number |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_clm_get_referred_payments` | `GetReferredList` | Get list of all referred claim payments |
| `spu_get_claim_cnts` | `CreateEvent` | Get party/folder/file counts for event creation |
| `spu_clm_Process_Decline` | `ProcessDecline` | Set claim payment status to declined |
| `spu_clm_Process_Authorise` | `ProcessAuthorise` | Set claim payment status to authorised |
| `spu_pmuser_is_name_member` | `CheckUserGroup` | Check if user is a member of a named group |
| `spu_Get_Debtor_User_Groups` | `CheckDebtorGroups` | Get debtor user groups for a group type |
| `spu_Approval_Records_Sel` | `GetApprovalRecords` | Get approval records for a payment |
| `spu_Get_Approval_Step_Details` | `GetStepDetails` | Get approval user group and code for a step |
| `spu_Check_Is_User_Unique` | `IsUserUnique` | Check if user has approved a previous step |
| `spu_Get_User_Authority_Limit` | `CheckUserAuthorityLimit` | Get user claim & regular payment authority limits |
| `spu_Payment_Approval_Add` | `CreateApprovalRecord` | Add a record to Payment_Approval table |
| `spu_clm_remove_authorisation_tasks` | `RemoveAuthTasks` | Remove authorisation tasks |
| `spu_get_pmwrk_task_instance_cnt` | `ProcessWTM` | Get work task instance cnt by key name/value |
| `spu_CLM_Get_Claim_Payment_Accounts_Details` | `GetClaimPaymentAccountsDetails` | Get claim payment accounts details |
| `spu_CLM_GetClaimVersionDescription` | `GetClaimVersionDescription` | Get claim version description |
| `Spu_get_claim_status` | `GetClaimStatus` | Get claim status |
| `spu_CLM_Get_ReferredClaim_status` | `GetReferredClaimStatus` | Get referred claim payment status |
| `spu_Claim_Upd_Status` | `Update_Claim_Status` | Update claim status |
| `spu_cashlistitem_claim_link_Sel` | `GetCashListItemClaimLinkDetails` | Get cash list item claim link |
| `spu_CLM_Set_Payment_for_Recommendation` | `SetClaimPaymentRecommendStatus` | Set payment for recommendation |
| `spu_Get_Reserve_For_Claim_Payment` | `GetReserveTotalForClaimPayment` | Get reserve total for claim payment |
| `spu_ACT_Select_CashListItem` | `GetTransDetailFromCashListItem` | Select cash list item transaction detail |
| `spu_CLM_Get_Already_ReferredClaim_status` | `GetAlreadyReferredClaimStatus` | Get already-referred claim status |
| `spu_Get_User_OtherPartyID` | `GetUserOtherParty` | Get user other party ID |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `BPMLOOKUP.Business` | `Business`, `StepAuthorization` | Lookup operations |
| `bSIREvent.Business` | `Business.CreateEvent` | Create audit trail events |
| `bPMWrkTaskInstance.TaskControl` | `Business.ProcessWTM`, `Business.AddTaskToWorkManager` | Work task manager operations |
| `bACTCurrencyConvert.Form` | `StepAuthorization.CheckUserAuthorityLimit` | Currency conversion for payment limit comparison |

---

### 3. bBackOfficeLink
**Directory:** `Back Office Link/`
**Project:** `bBackOfficeLink`
**Purpose:** Provides the link between claims and the back-office policy system — policy search (UW and broking), client/insurer/risk detail retrieval, GIS index search, and claim date policy matching.

**Business Methods — bBOLink (bBOLink.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceId As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises the BOLink object with database and Sirius product context |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes database connection |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process mode properties |
| `GetUWPolicyList` | `Public Function GetUWPolicyList(ByRef r_vResultArray As Object, Optional ByVal v_vPolicyNo As Object = "", Optional ByVal v_vPartyShortName As Object = "", Optional ByVal v_vPostCode As Object = "", Optional ByVal v_vPolicyStartDate As Object = "", Optional ByVal v_vPolicyEndDate As Object = "", Optional ByVal v_vClaimDate As Object = "", Optional ByVal v_bLimitResults As Boolean = False, Optional ByVal v_lCoverNoteSheetNumber As Integer = 0, Optional ByVal v_vAgentGroupCnt As Object = 0, Optional ByVal v_lNumberofRecords As Integer = -1, Optional ByVal nAgentKey As Integer = 0, Optional ByVal bRetrieveAssociated As Boolean = False) As Integer` | Gets underwriting policy list (live, renewal, MTA) |
| `FindLikeIndex` (overload 1) | `Public Function FindLikeIndex(ByRef sIndex As String, ByRef lNumberOfRecords As Integer, ByRef vResultArray(,) As Object, Optional ByRef sDataModelType As String = "RISK", Optional ByVal v_vAgentGroupCnt As Object = 0) As Integer` | Finds policies by GIS search index (no insurance ref) |
| `FindLikeIndex` (overload 2) | `Public Function FindLikeIndex(ByRef sIndex As String, ByRef lNumberOfRecords As Integer, ByRef vResultArray(,) As Object, ByRef sInsuranceRef As String, Optional ByRef sDataModelType As String = "RISK", Optional ByVal v_vAgentGroupCnt As Object = 0) As Integer` | Finds policies by GIS search index with insurance ref |
| `GetAllGISSearchResults` | `Public Function GetAllGISSearchResults(ByRef sSearchStr As String, ByRef lNoOfRecords As Integer, ByRef vDataModelsArray(,) As Object, ByRef vResultArray As Object, ByRef sInsuranceRef As String, Optional ByVal v_vAgentGroupCnt As Object = Nothing) As Integer` | Gets all GIS search results across data models |
| `GetUWPolicyByGISSearchIndex` | `Public Function GetUWPolicyByGISSearchIndex(ByRef vInputData As Object, ByRef vOutputData(,) As Object, Optional ByVal v_vPolicyNo As Object = "", Optional ByVal v_vPartyShortName As Object = "", Optional ByVal v_vPostCode As Object = "", Optional ByVal v_vPolicyStartDate As Object = "", Optional ByVal v_vPolicyEndDate As Object = "", Optional ByVal v_vClaimDate As Object = "", Optional ByVal v_lCoverNoteSheetNumber As Integer = 0, Optional ByVal v_lNumbersOfRecords As Integer = -1, Optional ByVal bRetrieveAssociates As Boolean = False) As Integer` | Gets UW policies by GIS search index for risk details |
| `GetPolicyList` | `Public Function GetPolicyList(ByRef r_vResultArray(,) As Object, Optional ByVal v_vpol_no As Object = Nothing, Optional ByVal v_vpol_code As Object = Nothing, Optional ByVal v_vrisk_code As Object = Nothing, Optional ByVal v_vclm_dt As Object = Nothing, Optional ByVal v_vprty_shrt_nm As Object = Nothing, Optional ByVal v_vpost_code As Object = Nothing, Optional ByVal v_vfrm_dt As Object = Nothing, Optional ByVal v_vto_dt As Object = Nothing, Optional ByVal v_vexclude_lapsed As Object = Nothing, Optional ByVal v_vValidSourceArray As Object = Nothing, Optional ByVal v_bIncludeDeleted As Boolean = True, Optional ByVal v_vClientName As Object = Nothing) As Integer` | Gets broking policy list using dynamic SQL query |
| `GetPolicyDetails` | `Public Function GetPolicyDetails(ByRef r_vResultArray(,) As Object, ByVal v_lpol_id As Integer, Optional ByVal v_vclm_dt As String = "") As Integer` | Gets policy start/end dates and currency for a policy |
| `GetClientDetails` | `Public Function GetClientDetails(ByRef r_vResultArray() As Object, ByVal v_lpol_id As Integer, Optional ByVal v_vclm_dt As String = "") As Integer` | Gets client name, address, contacts for a policy |
| `GetInsurerDetails` | `Public Function GetInsurerDetails(ByRef r_vResultArray() As Object, ByVal v_lpol_id As Integer, Optional ByVal v_vclm_dt As Object = Nothing, Optional ByVal lTransactionMode As Integer = gPMConstants.PMEComponentAction.PMView) As Integer` | Gets insurer name, address, contacts for a policy |
| `GetRiskDetails` | `Public Function GetRiskDetails(ByRef r_vResultArray(,) As Object, ByVal v_lpol_id As Integer, Optional ByVal v_vclm_dt As String = "", Optional ByVal v_vclm_no As Object = Nothing) As Integer` | Gets risk code ID and description for a policy |
| `GetRiskDesc` | `Public Function GetRiskDesc(ByRef r_vResultArray(,) As Object, ByVal v_irisk_code_id As Integer) As Integer` | Gets risk description by risk code ID |
| `GetPolicyForClaimDate` | `Public Function GetPolicyForClaimDate(ByVal v_dtClaimDate As Date, ByRef r_lInsuranceFileCnt As Integer, ByRef r_sPolicyNumber As String, ByRef r_dtStartDate As Date, Optional ByRef r_lReturnCode As Integer = 0) As Integer` | Gets policy that covers a given claim date |
| `Apostrophes` | `Public Function Apostrophes(ByRef sString As String) As Integer` | Doubles up apostrophes in a string for SQL safety |
| `CheckInRenewal` | `Public Function CheckInRenewal(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lRenewalStatus As Integer) As Integer` | Checks if a policy is in renewal |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_get_sirius_prod` | `GetSiriusProd` | Get Sirius product information |
| `spu_get_party_adds` | (GetPartyAdds) | Get party addresses |
| `spu_get_party_contacts` | (GetPartyContacts) | Get party contacts |
| `spu_get_client_details` | (GetClientDetails - broking) | Get client details for broking |
| `spu_get_client_details_U` | `GetClientDetails` | Get client details for underwriting |
| `spu_get_insurer_details` | `GetInsurerDetails` | Get insurer details |
| `spu_get_risk_details` | `GetRiskDetails` (broking) | Get risk details for broking |
| `spu_get_risk_details_U` | `GetRiskDetails` (UW) | Get risk details for underwriting |
| `spu_get_risk_details_open_claim_U` | `GetRiskDetails` (open claim UW) | Get risk details for open claim underwriting |
| `spu_get_risk_desc` | `GetRiskDesc` | Get risk description |
| `spu_get_policy_details` | `GetPolicyDetails` | Get policy dates and currency |
| `spu_GetPolicy_U` | `GetUWPolicyList`, `GetUWPolicyByGISSearchIndex` | Get latest version of UW policy |
| `spu_gis_search_property_find` | `GetAllGISSearchResults` | GIS property search |
| `spu_gis_search_property_risk` | `GetAllGISSearchResults` | GIS property search for risk |
| `spu_Get_Policy_For_Claim_Date` | `GetPolicyForClaimDate` | Get policy covering a claim date |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| _(none — standalone policy lookup component)_ | | |

---

### 4. bCLMCase
**Directory:** `Case/`
**Project:** `bCLMCase`
**Purpose:** Manages claim case lifecycle — create, save, load, copy, close, link/unlink claims to cases, case history, search, and GIS data model integration.

**Business Methods — Business (bCLMCase.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises Business object and database connection |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes database connection |
| `New` | `Public Sub New()` | Constructor, creates database instance |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process mode properties |
| `LoadCase` | `Public Function LoadCase(ByVal v_lCaseId As Integer, ByRef r_sCaseNumber As String, ByRef r_dtCaseOpenedDate As Date, ByRef r_lCaseProgressStatusID As Integer, ByRef r_lCaseAnalystID As Integer, ByRef r_lCaseAssistantID As Integer, ByRef r_lCaseVersion As Integer, ByRef r_lBaseCaseID As Integer) As Integer` | Loads case details by case ID |
| `SaveCase` | `Public Function SaveCase(ByVal v_lCaseId As Integer, ByVal v_sCaseNumber As String, ByVal v_dtCaseOpenedDate As Date, ByVal v_lCaseProgressStatusID As Integer, ByVal v_lCaseAnalystID As Integer, ByVal v_lCaseAssistantID As Integer, ByVal v_lCaseVersion As Integer, ByRef r_lNewCaseID As Integer, ByRef r_lBaseCaseID As Integer) As Integer` | Saves/adds case details, returns new case ID and base case ID |
| `GenerateCaseCode` | `Public Function GenerateCaseCode(ByRef r_sCaseCode As String, Optional ByVal v_iclaimid As Integer = 0, Optional ByVal v_lReturnError As Integer = 0, Optional ByVal v_sFailureReason As String = "") As Integer` | Generates a new case code using numbering scheme |
| `GetLinks` | `Public Function GetLinks(ByVal v_lBaseCaseID As Integer, ByRef r_vLinks(,) As Object) As Integer` | Gets claim-to-case links for a base case ID |
| `LinkClaims` | `Public Function LinkClaims(ByVal v_lBaseCaseID As Integer, ByVal v_vLinkArray As Object) As Integer` | Links claims to a case by updating base_case_id |
| `UnlinkClaims` | `Public Function UnlinkClaims(ByVal v_vUnlinkArray() As Object) As Integer` | Unlinks claims from a case (sets base_case_id to null) |
| `GetClaimDetail` | `Public Function GetClaimDetail(ByVal v_lClaimID As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Gets claim details for a given claim ID |
| `FindCase` | `Public Function FindCase(ByVal v_sSQL As String, ByRef r_vResultArray(,) As Object, Optional ByVal v_lNumberRecords As Long = -1) As Integer` | Executes a case search using provided SQL |
| `BeginTrans` | `Public Function BeginTrans() As Integer` | Begins a database transaction |
| `RollbackTrans` | `Public Function RollbackTrans() As Integer` | Rolls back a database transaction |
| `CommitTrans` | `Public Function CommitTrans() As Integer` | Commits a database transaction |
| `GetCaseHistory` | `Public Function GetCaseHistory(ByVal v_lBaseCaseID As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Gets case version history for a base case ID |
| `CloseCase` | `Public Function CloseCase(ByVal v_lCaseId As Integer) As Integer` | Closes a case by updating progress status |
| `CreateEvent` | `Public Function CreateEvent(ByVal v_lCaseId As Integer, ByVal v_sEventTypeCode As String, ByVal v_dtEventDate As Date, ByVal v_vDescription As Object) As Integer` | Creates an event record for a case |
| `GenerateSQL` | `Public Function GenerateSQL(ByRef r_sSQL As String, Optional ByVal v_sClaimNumber As String = "", Optional ByVal v_sCaseNumber As String = "", Optional ByVal v_lRiskTypeID As Integer = 0, Optional ByVal v_lProgressStatusID As Integer = 0, Optional ByVal v_vCaseOpenDate As Object = "", Optional ByVal v_vSearchFields As Object = Nothing, Optional ByVal v_vAgentKey As Object = 0) As Integer` | Generates dynamic SQL for case search with optional GIS fields |
| `GetPolicyCase` | `Public Function GetPolicyCase(ByRef vInputData(,) As Object, ByRef vOutputData(,) As Object, ByRef v_vSiriusProduct As Object, Optional ByVal v_vClaimNumber As Object = Nothing, Optional ByVal v_vClientName As Object = Nothing, Optional ByVal v_vPolicyNumber As Object = Nothing, Optional ByVal v_vRegNumber As Object = Nothing, Optional ByVal v_vLossFromdate As Object = Nothing, Optional ByVal v_vLossToDate As Object = Nothing, Optional ByVal v_vClaimStatus As Object = Nothing) As Integer` | Gets policy data for case search criteria |
| `GetPreviousDataModel` | `Public Function GetPreviousDataModel(ByVal v_lCaseId As Integer, ByRef r_lPreviousDataModelId As Integer, ByRef r_lGISPolicyLinkID As Integer) As Integer` | Gets previously attached case builder data model if changed |
| `DeleteCustomData` | `Public Function DeleteCustomData(ByVal v_lGisPolicyLinkId As Integer) As Integer` | Deletes all GIS data for a GIS policy link ID |
| `GetCaseDetails` | `Public Function GetCaseDetails(ByVal v_lCaseId As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Gets case details |
| `CleanUpDirtyCase` | `Public Function CleanUpDirtyCase(ByVal v_lCaseId As Integer) As Integer` | Cleans up dirty (unsaved) cases |
| `ProcessCopyCase` | `Public Function ProcessCopyCase(ByVal v_lCaseId As Integer, ByRef r_lCopyCaseId As Integer, Optional ByVal iUserId As Integer = 0) As Integer` | Processes copying a case with GIS data |
| `CopyCase` | `Public Function CopyCase(ByVal v_lCaseId As Integer, ByRef r_lCopyCaseId As Integer) As Integer` | Copies case details and returns new case ID |
| `GetGisPolicyLinkDetails` | `Public Function GetGisPolicyLinkDetails(ByVal v_lCaseId As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets GIS policy link details for a case |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_CLM_Case_Select` | `LoadCase` | Select case details by case ID |
| `spu_CLM_Case_Add` | `SaveCase` | Add/save case details |
| `spu_Get_Case_Claim_Link` | `GetLinks` | Get case-claim links for a base case |
| `spu_Case_Update_Claim_CaseLink` | `LinkClaims`, `UnlinkClaims` | Update claim base_case_id link |
| `spu_Get_Case_History` | `GetCaseHistory` | Get case version history |
| `spu_Update_Case_Progress_Status` | `CloseCase` | Update case progress status (close) |
| `spu_ACT_Spoke_Get_EventTypeIDFromCode` | `CreateEvent` | Get event type ID from event type code |
| `spu_Get_Case_Claim_Details` | `GetClaimDetail` | Get claim details for case |
| `spu_SIR_Is_Case_Screen_Data_Model_Changed` | `GetPreviousDataModel` | Check if case screen data model changed |
| `spu_SIR_Delete_GIS_Data` | `DeleteCustomData` | Delete all GIS data for a GIS policy link ID |
| `spu_Get_Case_Details` | `GetCaseDetails` | Get case details |
| `spu_CLM_Clean_Up_Dirty_Cases` | `CleanUpDirtyCase` | Clean up dirty/unsaved cases |
| `spu_CLM_Copy_Case` | `CopyCase` | Copy case details and return new case ID |
| `spu_CLM_Get_DataModel_Code_for_CASE` | `ProcessCopyCase` | Get data model code for case |
| `spu_CLM_Get_GIS_Policy_Link_Details_for_CASE` | `GetGisPolicyLinkDetails` | Get GIS policy link details for case |
| `spu_CLM_Update_GIS_Policy_Link_Details_for_CASE` | (UpdateGisPolicyLinkDetails) | Update GIS policy link details for case |
| `spg_*_copy_dataset` | `ProcessCopyCase` | Product-specific GIS data copy (dynamic SP name) |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bSIRPolicyNumMaint.Business` | `GenerateCaseCode` | Case numbering scheme generation |
| `bSIREvent.Business` | `CreateEvent` | Create audit trail events |
| `bGISMaintainDataDictionary.Business` | `GenerateSQL` | GIS search field SQL generation |

---

### 5. bCLMChangeClaimStatus
**Directory:** `Change Claim Status/`
**Project:** `bCLMChangeClaimStatus`
**Purpose:** Manages claim status transitions including raising financial transactions (payments, reserves, recoveries), creating accounting entries (stats folders/details), generating cash lists, managing DME/SharePoint document folders, handling claim reversals, reinsurance repost, and coordinating claim version finalisation.

**Business Methods — Business (bCLMChangeClaimStatusBusiness.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceId As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises the component with credentials, database connection, and default process modes |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process modes (task, navigate, process mode, transaction type, effective date) |
| `DeleteClaim` | `Public Function DeleteClaim(ByVal v_lClaimId As Integer) As Integer` | Deletes a claim record via `spu_delete_claim` within a transaction |
| `RaiseTransactions` | `Public Function RaiseTransactions(ByVal v_lClaimId As Integer) As Integer` | Overload — delegates to full signature with defaults |
| `RaiseTransactions` | `Public Function RaiseTransactions(ByVal v_lClaimId As Integer, ByRef r_lDocumentId As Integer) As Integer` | Overload — delegates with document ID output |
| `RaiseTransactions` | `Public Function RaiseTransactions(ByVal v_lClaimId As Integer, ByRef r_lDocumentId As Integer, ByVal PerilId As Integer) As Integer` | Overload — delegates with document ID and peril ID |
| `RaiseTransactions` | `Public Function RaiseTransactions(ByVal v_lClaimId As Integer, ByVal v_bSavedStats As Boolean) As Integer` | Overload — delegates with saved stats flag |
| `RaiseTransactions` | `Public Function RaiseTransactions(ByVal v_lClaimId As Integer, ByVal v_bSavedStats As Boolean, ByRef r_lDocumentId As Integer) As Integer` | Overload — delegates with saved stats and document ID |
| `RaiseTransactions` | `Public Function RaiseTransactions(ByVal v_lClaimId As Integer, ByVal v_bSavedStats As Boolean, ByRef r_lDocumentId As Integer, ByRef r_bFromSAM As Boolean, ByRef PerilId As Integer) As Integer` | Core implementation — creates stats folders/details, raises coinsurance/reinsurance transactions, processes tax entries, generates cash lists for receipts, updates payment/receipt document details |
| `GetReceiptFromStatsFolder` | `Public Function GetReceiptFromStatsFolder(ByVal v_nStatsFolderCnt As Integer, ByRef r_oReceipt(,) As Object, ByRef r_oReceiptItems(,) As Object) As Integer` | Overload — delegates to full signature with default source ID |
| `GetReceiptFromStatsFolder` | `Public Function GetReceiptFromStatsFolder(ByVal v_nStatsFolderCnt As Integer, ByRef r_oReceipt(,) As Object, ByRef r_oReceiptItems(,) As Object, ByRef r_vSource_Id As Integer) As Integer` | Retrieves receipt and receipt items for a stats folder, including source ID |
| `GetDocumentRefFromDocumentId` | `Public Function GetDocumentRefFromDocumentId(ByVal v_nDocumentId As Integer, ByRef r_oDocumentRef(,) As Object) As Integer` | Gets document_ref from Document table for a given document_id |
| `GetDefaultCashListItemReceiptType` | `Public Function GetDefaultCashListItemReceiptType(ByRef r_oResults(,) As Object) As Integer` | Retrieves the default CashListItem_Receipt_Type for 'Claim Receipt' |
| `GenerateClaimCashList` | `Public Function GenerateClaimCashList(ByVal v_nClaimId As Integer) As Integer` | Generates automated cash lists for claim receipts/recoveries if system option 5117 is enabled |
| `CreateEvent` | `Public Function CreateEvent(ByVal v_lClaimId As Integer, ByVal v_lEventType As Integer, ByVal v_sDescription As String) As Integer` | Creates an event record for the claim using bSIREvent.Business |
| `IsClaimDateChanged` | `Public Function IsClaimDateChanged(ByVal v_lClaimId As Integer, ByRef r_lChanged As Integer) As Integer` | Checks if claim loss dates have changed from previous version |
| `IsAddTask` | `Public Function IsAddTask(ByVal v_lClaimId As Integer, ByVal v_sTransactionType As String, ByRef r_lAddTask As Integer) As Integer` | Determines if external handler tasks should be added based on transaction type, info-only status, and claim status |
| `GetClaimDetails` | `Public Function GetClaimDetails(ByVal v_lClaimId As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Retrieves claim details including insurance file info and post-claims-tax flag |
| `BeginTrans` | `Public Function BeginTrans() As Integer` | Begins a database transaction |
| `CommitTrans` | `Public Function CommitTrans() As Integer` | Commits a database transaction |
| `RollbackTrans` | `Public Function RollbackTrans() As Integer` | Rolls back a database transaction |
| `UpdateInsuranceFileSystem` | `Public Function UpdateInsuranceFileSystem(ByVal v_lClaimId As Integer) As Integer` | Updates Insurance_File_System with last transaction date, type, and description for the claim's policy |
| `GetInsFileCntProductId` | `Public Function GetInsFileCntProductId(ByVal v_lInsuranceFilecnt As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Gets the product ID for a given insurance file cnt |
| `SetPaymentReferred` | `Public Function SetPaymentReferred(ByVal v_lClaimId As Integer, ByVal v_iStatus As Integer) As Object` | Sets pending claim payments to Referred or Processed status |
| `SetPaymentRecommendation` | `Public Function SetPaymentRecommendation(ByVal v_lClaimId As Integer, ByVal v_iStatus As Integer) As Integer` | Sets claim payment recommendation status |
| `GetTotalPayment` | `Public Function GetTotalPayment(ByVal v_lClaimId As Integer, ByRef r_cAmount As Decimal) As Object` | Gets total payment amount for a claim |
| `GetClaimAdminGroup` | `Public Function GetClaimAdminGroup(ByRef r_lGroupId As Integer) As Object` | Overload — gets CLMSUPER group ID (default) |
| `GetClaimAdminGroup` | `Public Function GetClaimAdminGroup(ByRef r_lGroupId As Integer, ByVal v_sGroupCode As String) As Object` | Gets user group ID for a specified group code |
| `GetClaimNumber` | `Public Function GetClaimNumber(ByVal v_lClaimId As Integer, ByRef r_sClaimNumber As String) As Integer` | Gets the claim number string for a claim ID |
| `GetStatsFolderForClaim` | `Public Function GetStatsFolderForClaim(ByVal v_lClaimId As Integer, ByRef r_vStatsFolder(,) As Object) As Integer` | Overload — gets all stats folders for a claim |
| `GetStatsFolderForClaim` | `Public Function GetStatsFolderForClaim(ByVal v_lClaimId As Integer, ByRef r_vStatsFolder(,) As Object, ByRef r_sTransactionType As String) As Integer` | Gets stats folders for a claim, optionally filtered by transaction type |
| `RemoveAuthTasks` | `Public Function RemoveAuthTasks(ByRef v_sDescription As Object) As Integer` | Removes existing authorisation task instances by description |
| `GetPaymentAmount` | `Public Function GetPaymentAmount(ByVal v_lClaimId As Integer, ByRef r_crPaymentAmount As Decimal, ByRef r_crThisPaymentAmount As Decimal, ByRef r_lCurrencyId As Integer, Optional ByRef r_lOriginalPaymentAmount As Decimal = 0, Optional ByRef r_lOriginalCurrencyId As Integer = 0) As Integer` | Gets payment amounts (total, this session, original) and currency IDs for a claim |
| `GetReserveRecoveryOS` | `Public Function GetReserveRecoveryOS(ByVal v_lClaimId As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Gets outstanding reserve and recovery amounts for a claim |
| `UpdateClaimStatus` | `Public Function UpdateClaimStatus(ByVal v_lClaimId As Integer) As Integer` | Overload — updates claim status to Live (status ID 2) |
| `UpdateClaimStatus` | `Public Function UpdateClaimStatus(ByVal v_lClaimId As Integer, ByVal v_lClaimStatusID As Integer) As Integer` | Updates claim status to the specified status ID |
| `UpdateClaimDesc` | `Public Function UpdateClaimDesc(ByVal v_lClaimId As Integer, ByVal v_sClaimVersionDescription As String) As Integer` | Updates the claim version description |
| `CreateDMEClaimFolder` | `Public Function CreateDMEClaimFolder(ByVal v_lClaimId As Integer) As Integer` | Creates a DME or SharePoint folder for the claim based on system option 10 |
| `UpdatePaymentDocumentDetails` | `Public Function UpdatePaymentDocumentDetails(ByVal v_lStatsFolderCnt As Integer) As Integer` | Updates the stats folder's associated payment with document details |
| `UpdateReceiptDocumentDetails` | `Public Function UpdateReceiptDocumentDetails(ByVal v_lStatsFolderCnt As Integer) As Integer` | Updates the stats folder's associated receipt document link |
| `GetClaimPaymentAccountsDetails` | `Public Function GetClaimPaymentAccountsDetails(ByVal v_lClaimId As Integer, ByRef r_vResults(,) As Object) As Integer` | Returns payment accounts details for the claim (cheque production workflow) |
| `UpdateClaimIsDirty` | `Public Function UpdateClaimIsDirty(ByVal v_lClaimId As Integer, ByVal v_lIsDirty As Integer) As Integer` | Updates the is_dirty flag on the claim |
| `FinaliseClaimDetails` | `Public Function FinaliseClaimDetails(ByVal v_lClaimId As Integer, ByVal v_sClaimVersionDescription As String) As Integer` | Completes database processing for claim versioning |
| `GetOriginalClaimId` | `Public Function GetOriginalClaimId(ByVal v_lClaimId As Integer, ByRef r_lOriginalClaimId As Integer) As Integer` | Returns the base/original claim ID for a given claim |
| `UpdateClaimSuppression` | `Public Function UpdateClaimSuppression(ByVal lClaimID As Integer, ByVal lSuppressReserves As Integer, ByVal lSuppressPayments As Integer, ByVal lSuppressRecoveries As Integer) As Integer` | Overload — updates suppression flags (pass -1 to leave as-is) |
| `UpdateClaimSuppression` | `Public Function UpdateClaimSuppression(ByVal lClaimID As Integer, ByVal lSuppressReserves As Integer, ByVal lSuppressPayments As Integer, ByVal lSuppressRecoveries As Integer, ByRef lOriginalSuppressReserves As Integer) As Integer` | Overload — with original reserves output |
| `UpdateClaimSuppression` | `Public Function UpdateClaimSuppression(ByVal lClaimID As Integer, ByVal lSuppressReserves As Integer, ByVal lSuppressPayments As Integer, ByVal lSuppressRecoveries As Integer, ByRef lOriginalSuppressReserves As Integer, ByRef lOriginalSuppressPayments As Integer) As Integer` | Overload — with original reserves and payments output |
| `UpdateClaimSuppression` | `Public Function UpdateClaimSuppression(ByVal lClaimID As Integer, ByVal lSuppressReserves As Integer, ByVal lSuppressPayments As Integer, ByVal lSuppressRecoveries As Integer, ByRef lOriginalSuppressReserves As Integer, ByRef lOriginalSuppressPayments As Integer, ByRef lOriginalSuppressRecoveries As Integer) As Integer` | Updates claim suppression fields (0/1 or -1 for no change) and returns original values |
| `CreateReverseTransactions` | `Public Function CreateReverseTransactions(ByRef lClaim_id As Integer) As Integer` | Creates reversal stats transactions for a claim via RI reverse and creates accounting documents |
| `RepostClaimTransactions` | `Public Function RepostClaimTransactions(ByRef lClaim_id As Integer) As Integer` | Reposts claim reinsurance stats details and creates transactions for each stats folder |
| `IsClaimReversalRequired` | `Public Function IsClaimReversalRequired(ByVal v_lClaimId As Integer, ByRef r_bClaimReversalRequired As Boolean) As Integer` | Determines if a claim reversal is required |
| `GetClaimRIArrangementDetails` | `Public Function GetClaimRIArrangementDetails(ByVal v_lClaimId As Integer, ByRef r_lVersionId As Integer) As Integer` | Gets RI arrangement details and version ID for a claim |
| `UpdateClaimEvents` | `Public Function UpdateClaimEvents(ByVal v_lClaimId As Integer) As Integer` | Updates event records for a claim using bSIREvent.Business |
| `GetClaimOldPolicy` | `Public Function GetClaimOldPolicy(ByVal v_lClaimId As Integer, ByRef r_sClaimOldPolicyRef As String) As Integer` | Gets the old policy reference attached to a claim |
| `UpdatePaymentReference` | `Public Function UpdatePaymentReference(ByVal v_lDocument_Id As Integer) As Integer` | Records panel solicitors reference against a payment (WR08) |
| `ChangeClaimStatusForSAM` | `Public Function ChangeClaimStatusForSAM() As Integer` | Orchestrates claim status change when called from SAM — updates insurance file system, raises transactions with saved stats, resets referred payments |
| `GetProductDetails` | `Public Function GetProductDetails(ByVal v_lProductId As Integer, ByRef r_bPaymentRefCheck As Boolean) As Integer` | Gets product-level options for a claim including payment reference check flag |
| `GetClaimStatus` | `Public Function GetClaimStatus(ByVal v_lClaimId As Integer, ByRef r_sStatus As String) As Integer` | Gets the current claim status string |
| `CreateStatsFolder` | `Public Function CreateStatsFolder(ByVal v_sTransactionTypeCode As String, ByVal v_lClaimId As Integer, ByRef r_lStatsFolderCnt As Integer) As Integer` | Creates a new stats folder record for the claim and transaction type |
| `CreateStatsDetails` | `Public Function CreateStatsDetails(ByVal v_lStatsFolderCnt As Long, ByVal v_lClaimId As Long, ByVal v_sStatsDetailType As String, ByVal sCreditAccountCode As String, ByRef lClaimPaymentID As Long, ByRef r_bThisRevesionPresent As Boolean, Optional ByVal dTaxamount As Double = 0, Optional ByVal v_sTransactionTypeCode As String = "", Optional ByVal PerilId As Integer = 0) As Long` | Creates stats detail entries (GRS/TAG/TAN) for claims transactions |
| `GetClaimTransactionType` | `Public Function GetClaimTransactionType(ByVal v_lClaimId As Long, ByRef r_vResults As Object) As Long` | Gets the claim transaction type details |
| `RaiseClonedTransactions` | `Public Function RaiseClonedTransactions(ByVal nClaimId As Integer, ByVal nStatsFolderCnt As Integer) As Integer` | Raises transactions for cloned claims — finalises stats, creates transactions, and auto-allocates debit/credit entries |
| `GetInsuranceFileDetails` | `Function GetInsuranceFileDetails(ByVal nClaimId As Integer, ByRef r_nInsurance_file_cnt As Integer, ByRef r_nInsurance_folder_cnt As Integer, ByRef r_nParty_cnt As Integer) As Integer` | Gets insurance file cnt, folder cnt, and party cnt for a claim |
| `GetUserIDForTaskCompleteIntimation` | `Public Function GetUserIDForTaskCompleteIntimation(ByVal v_lClaimID As Integer, ByRef v_lPreviousUserID As Integer, ByRef v_sUserName As String, ByRef v_dReserveEntered As Decimal) As Integer` | Gets previous user ID, username, and reserve entered amount for task completion intimation |

**Properties — Business:**

| Property | Type | Access | Description |
|----------|------|--------|-------------|
| `CallingAppName` | `String` | WriteOnly | Sets the calling application name |
| `PMProductFamily` | `Integer` | ReadOnly | Returns `PMEProductFamily.pmePFSiriusSolutions` |
| `PMAuthorityLevel` | `Integer` | WriteOnly | Sets PM authority level (no-op in current implementation) |
| `NewClaimNumber` | `String` | ReadOnly | Returns the new claim number after generation |
| `ClaimId` | `Integer` | Read/Write | Gets or sets the current claim ID |
| `IsCloned` | `Integer` | Read/Write | Gets or sets whether the claim is cloned (1 = cloned) |
| `IsCloneReversal` | `Boolean` | Read/Write | Gets or sets whether this is a clone reversal operation |

**Module Constants — MainModule (bCLMChangeClaimStatus.vb):**

| Constant | Value | Description |
|----------|-------|-------------|
| `ACApp` | `"bCLMChangeClaimStatus"` | Application identifier |
| `kSysOptDMEInstalled` | `10` | System option: DME installed |
| `kSysOptPaymentRefCheck` | `5040` | System option: Payment reference check |
| `kTaxTypeArrayPosCode` | `1` | Tax type array position for code |
| `kTaxTypeArrayPosTaxAmount` | `2` | Tax type array position for amount |
| `kClaimDetailPostClaimsTaxes` | `11` | Claim detail array position for post-claims taxes flag |

**Stored Procedures (bCLMChangeClaimStatusBusinessSQL.vb):**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_Upd_Policy_Premium` | — | Update policy premium |
| `spu_delete_claim` | `DeleteClaim` | Delete a claim record |
| `spu_get_claim_cnts` | `CreateEvent`, `UpdateClaimEvents`, `GetClaimOldPolicy`, `GetInsuranceFileDetails` | Get party cnt, insurance folder cnt, insurance file cnt for a claim |
| `spu_CLM_Get_Transaction_Type_Details` | `RaiseTransactions`, `GenerateClaimCashList`, `RaiseClonedTransactions` | Get transaction type ID from transaction code |
| `spu_CLM_Get_Claim_Loss_Dates` | `IsClaimDateChanged` | Get loss dates from claim and work claim for comparison |
| `spu_CLM_Get_Add_Task_Details` | `IsAddTask` | Get claim flags (info_only, claim_status, previous info_only) |
| `spu_CLM_Get_Claim_Details` | `GetClaimDetails` | Get claim details including insurance file and tax settings |
| `spu_Get_DataModel_Code_For_Claim` | — | Get data model code for a claim |
| `spu_get_claiminsfilecnt_productid` | `GetInsFileCntProductId` | Get product ID for an insurance file cnt |
| `spu_CLM_Set_Referred_Payments` | `SetPaymentReferred` | Set pending payments to Referred/Processed status |
| `spu_CLM_Get_Total_Payment` | `GetTotalPayment` | Get total payment amount for a claim |
| `spu_Get_User_Group_Id` | `GetClaimAdminGroup` | Get user group ID for a group code |
| `spu_clm_remove_authorisation_tasks` | `RemoveAuthTasks` | Remove existing authorisation task instances |
| `spu_clm_get_payment_amount` | `GetPaymentAmount` | Get payment amounts for a claim in the current session |
| `spu_GetCurrentReserveRecovery` | `GetReserveRecoveryOS` | Get outstanding reserve and recovery amounts |
| `spu_CLM_Update_Claim_status` | `UpdateClaimStatus` | Update the claim status |
| `spu_CLM_Get_Claim_DME_Details` | `CreateDMEClaimFolder` | Get claim details needed to create DME folder |
| `spu_CLM_Payment_Document_Update` | `UpdatePaymentDocumentDetails` | Update stats folder's associated payment document |
| `spu_CLM_Receipt_Document_Update` | `UpdateReceiptDocumentDetails` | Update stats folder's associated receipt document |
| `spu_CLM_Get_Claim_Payment_Accounts_Details` | `GetClaimPaymentAccountsDetails` | Get claim payment accounts details for cheque production |
| `spu_CLM_Claim_Is_Dirty_Update` | `UpdateClaimIsDirty` | Update the is_dirty flag on a claim |
| `spu_CLM_Get_Claims_Stats_Folders` | `GetStatsFolderForClaim` | Get stats folders for a claim |
| `spu_CLM_Finalise_Claim_Details` | `FinaliseClaimDetails` | Complete database processing for claim versioning |
| `spu_CLM_Get_Base_Claim` | `GetOriginalClaimId` | Get the base/original claim ID |
| `spu_CLM_UpdateClaimDescription` | `UpdateClaimDesc` | Update claim version description |
| `spu_Claim_RI_ReverseTransaction_Stats` | `CreateReverseTransactions` | Reverse RI stats transactions for a claim |
| `spu_add_claims_stats_details_reins_process` | `RepostClaimTransactions` | Repost claims stats details for reinsurance processing |
| `spu_is_claim_reversal_required` | `IsClaimReversalRequired` | Check if claim reversal is required |
| `spu_Claim_RI_Arrangement_saa` | `GetClaimRIArrangementDetails` | Get RI arrangement details for a claim |
| `spu_get_claim_old_policy` | `GetClaimOldPolicy` | Get old policy reference attached to a claim |
| `spu_CLM_Update_Payment_Reference` | `UpdatePaymentReference` | Update claim payment reference (panel solicitors ref, WR08) |
| `spu_CLM_Set_Payment_for_Recommendation` | `SetPaymentRecommendation` | Set payment for recommendation status |
| `spu_CLM_Get_Claim_Status` | `GetClaimStatus` | Get current claim status |
| `spu_CLM_Get_transaction_type` | `GetClaimTransactionType` | Get claim transaction type details |
| `spu_clm_add_stats_folder` | `CreateStatsFolder` | Add a new claims stats folder |
| `spu_clm_add_stats_details_GRS` | `CreateStatsDetails` | Add claims stats detail entries (GRS/TAG/TAN) |
| `spu_CLM_Get_Claim_Tax_Amounts_By_Tax_Type` | `RaiseTransactions` (via private `GetClaimTaxAmountsByTaxType`) | Get tax entries by tax type for a payment or receipt |
| `spu_ACT_Do_Currency_Conversion` | `GenerateCashList` (private) | Perform currency conversion for cash list items |
| `spu_clm_check_reserve_amount` | `RaiseTransactions` | Check reserve amount for a claim and transaction type |
| `spu_clm_get_transdetails` | `RaiseClonedTransactions` | Get transaction details for a document ID |
| `spu_clm_get_transdetails_RI` | `RaiseClonedTransactions` | Get RI transaction details for allocation |
| `spu_CLM_Get_ThisReceipt_Item` | `GetReceiptFromStatsFolder` | Get receipt items for a claim receipt |
| `spu_CLM_Get_Stats_Folder_For_Claim` | `RaiseTransactions` | Get stats folder for claim (checks reversal presence) |
| `spu_CLM_Update_Suppression` | `UpdateClaimSuppression` | Update claim suppression fields (reserves/payments/recoveries) |
| `spu_CLM_Get_Stats_Folder` | `GetStatsFolderForClaim` | Get stats folder filtered by transaction type code |
| `spu_Get_Details_For_Completion_Intimation` | `GetUserIDForTaskCompleteIntimation` | Get details for task completion intimation |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bControlTransClaims.Automated` | `RaiseTransactions`, `GenerateClaimCashList`, `CreateReverseTransactions`, `RepostClaimTransactions`, `RaiseClonedTransactions` | Handles coinsurance/reinsurance stats creation, stats finalisation, and accounting document/transaction creation |
| `bSIREvent.Business` | `CreateEvent`, `UpdateClaimEvents` | Creates and updates event records for claims |
| `bACTCashList.Form` | `GenerateCashList` (private) | Creates cash list records for automated receipt generation |
| `bACTCashListItem.Form` | `GenerateCashList` (private) | Creates individual cash list item records |
| `bACTCashListPost.Automated` | `GenerateCashList` (private) | Posts and allocates cash list items |
| `bACTAllocationManual.Business` | `RaiseClonedTransactions` | Performs manual allocation of debit/credit transaction pairs for cloned claims |
| `bSIRDOCAPI.Form` | `CreateDMEClaimFolder` | Creates DME (Documaster) claim folders |
| `bSIRSharepoint.Business` | `CreateDMEClaimFolder` | Generates default SharePoint folder paths for claims |
| `bSIRProduct.Business` | `GetProductDetails`, `GetIsPaymentsReadOnly`, `GetIsReservesReadOnly` | Gets product-level options and script configuration (payment ref check, read-only flags) |

---

### 6. bCLMCheckDeferredRI
**Directory:** `Check Deferred RI/`
**Project:** `bCLMCheckDeferredRI`
**Purpose:** Checks deferred reinsurance (RI) status for claims, including retrieving claim risk status and adding tasks to the work manager for deferred RI processing.

**Business Methods — Business (bCLMCheckDeferredRICls.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard initialisation entry point; sets up database, credentials, and Lookup component |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process mode parameters |
| `New` | `Public Sub New()` | Constructor |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes resources and closes database |
| `GetClaimRiskStatus` | `Public Function GetClaimRiskStatus(ByVal v_lClaimId As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Gets the deferred RI risk status for a given claim |
| `AddTaskToWorkManager` | `Public Function AddTaskToWorkManager(ByVal v_sClientName As String, ByVal v_sDescription As String, ByVal v_dtDueDate As Date) As Integer` | Adds a deferred RI task to the work manager for claim payment follow-up |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_CLM_deferred_ri_status_sel` | `GetClaimRiskStatus` | Selects deferred RI status for a claim |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `BPMLOOKUP.Business` | `Initialise` | Lookup business component for reference data |
| `bPMWrkTaskInstance.TaskControl` | `AddTaskToWorkManager` | Creates work manager task instances |

---

### 7. bCLMCheckUnpaidPremium
**Directory:** `Check Unpaid Premium/`
**Project:** `bCLMCheckUnpaidPremium`
**Purpose:** Checks unpaid premium status for claims, retrieves premium payment information, transaction details for policies, and manages work claims for unpaid premium processing.

**Business Methods — Business (bSIRCheckUnpaidPremium.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard initialisation; implements `SSP.S4I.Interfaces.IBusiness.Initialise` |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process mode parameters |
| `New` | `Public Sub New()` | Constructor |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes resources and closes database |
| `getUnderwritingOrAgency` | `Public Function getUnderwritingOrAgency() As Integer` | Determines if business is underwriting or agency |
| `GetOriginalClaimNo` | `Public Function GetOriginalClaimNo(ByRef r_lOriginalClaimID As Integer) As Integer` | Gets the original claim ID from the work table |
| `DeleteClaim` | `Public Function DeleteClaim() As Integer` | Deletes work claim records |
| `GetPremiumPaymentsStatus` | `Public Function GetPremiumPaymentsStatus(ByVal v_vClaimRef As Object, ByRef r_vPremiumPayments(,) As Object) As Integer` | Gets premium payment status information for a claim |
| `GetTransactionsForPolicy` | `Public Function GetTransactionsForPolicy(ByVal v_vPolicyRef As Object, ByRef r_vTransactionsForPolicy(,) As Object) As Integer` | Gets all transactions for a given policy reference |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_get_Premium_Payment_Status` | `GetPremiumPaymentsStatus` | Gets premium payment status for a claim |
| `spu_get_Transactions_For_Policy` | `GetTransactionsForPolicy` | Gets transactions for a policy |
| `spu_CLM_Get_Base_Claim` | `GetOriginalClaimNo` | Gets the original/base claim ID |
| `spu_delete_claim` | `DeleteClaim` | Deletes a work claim record |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| *None* | — | No external business components referenced |

---

### 8. bCLMClaimParty
**Directory:** `Claim Party/`
**Project:** `bCLMClaimParty`
**Purpose:** Manages claim party (third-party, claimant) associations on claims, including retrieving, adding, deleting and updating claim party links in work tables.

**Business Methods — Business (bCLMClaimPartyBusiness.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard initialisation; implements `SSP.S4I.Interfaces.IBusiness.Initialise` |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process mode parameters |
| `New` | `Public Sub New()` | Constructor |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes resources and closes database |
| `getUnderwritingOrAgency` | `Public Function getUnderwritingOrAgency() As Integer` | Determines if business is underwriting or agency |
| `GetDetails` | `Public Function GetDetails() As Integer` | Gets claim party links from work table for current ClaimId and PartyTypeCode |
| `GetSingleParty` | `Public Function GetSingleParty(ByVal v_lParty As Integer, ByRef r_vDetails(,) As Object) As Integer` | Gets details for a single party from the work claim party table |
| `Update` | `Public Function Update() As Integer` | Deletes existing and re-inserts claim party links within a transaction |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_claim_party_link_saa` | *(defined, ACGetClaimPartyClaimSQL)* | Selects all claim party links for a claim |
| `spu_claim_party_link_dar` | *(defined, ACDeleteClaimPartyClaimSQL)* | Deletes all claim party links for a claim |
| `{spu_claim_party_link_add}` | *(defined, ACInsertClaimPartyClaimSQL)* | Inserts a claim party link |
| `{call spu_work_claim_party_link_saa (?,?)}` | `GetDetails` | Selects work claim party links by claim_id and code |
| `{call spu_work_claim_party_link_dar (?,?)}` | `Update` | Deletes work claim party links by claim_id and code |
| `{call spu_work_claim_party_link_add (?,?)}` | `Update` | Inserts work claim party link (claim_id + party_cnt) |
| `{call spu_work_single_party_claim_sel (?)}` | `GetSingleParty` | Selects single party details from work table |
| `{call spu_single_party_claim_sel (?)}` | *(defined, ACGetSinglePartyClaimSQL)* | Selects single party details from main table |
| `spu_get_client_policy_details` | *(defined, ACGetClientPolicyDetailsSQL)* | Gets client policy details |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| *None* | — | No external business components referenced |

---

### 9. bCLMClaimPartyLink
**Directory:** `Claim Party Link Maintenance/`
**Project:** `bCLMClaimPartyLink`
**Purpose:** Maintains claim-to-party link records — adds and deletes individual claim party link associations on the main claim table (as opposed to work tables).

**Business Methods — Business (Business.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Standard initialisation; sets up database and credentials |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process mode parameters |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes resources and closes database |
| `DeleteClaimPartyLink` | `Public Function DeleteClaimPartyLink(ByVal v_lClaimPartyId As Integer, ByVal v_lClaimId As Integer) As Integer` | Deletes a claim party link for the specified claim_id and party_cnt |
| `AddClaimPartyLink` | `Public Function AddClaimPartyLink(ByVal v_lClaimPartyId As Integer, ByVal v_lClaimId As Integer) As Integer` | Adds a claim party link for the specified claim_id and party_cnt |
| `BeginTrans` | `Public Function BeginTrans() As Integer` | Begins a database transaction |
| `CommitTrans` | `Public Function CommitTrans() As Integer` | Commits the current database transaction |
| `RollbackTrans` | `Public Function RollbackTrans() As Integer` | Rolls back the current database transaction |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_CLM_Claim_Party_Link_Add` | `AddClaimPartyLink` | Adds a claim party link record |
| `spu_CLM_Claim_Party_Link_Delete` | `DeleteClaimPartyLink` | Deletes a claim party link record |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| *None* | — | No external business components referenced |

---

### 10. bCLMCloseClaim
**Directory:** `Close Claim/`
**Project:** `bCLMCloseClaim`
**Purpose:** Handles claim closure operations — retrieves claim status, gets reserve/recovery balances, deletes work claims, and sets claim status to closed/re-closed.

**Business Methods — Business (bCLMCloseClaimBusiness.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard initialisation; implements `SSP.S4I.Interfaces.IBusiness.Initialise` |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process mode parameters |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Stores parameter members from the key array |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Returns parameter members in a key array |
| `New` | `Public Sub New()` | Constructor |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes resources and closes database |
| `GetClaimStatus` | `Public Function GetClaimStatus(ByVal v_lClaimID As Integer, ByRef r_sClaimStatus As String) As Integer` | Gets the current status for a claim |
| `GetOriginalClaimID` | `Public Function GetOriginalClaimID(ByVal v_lClaimID As Integer, ByRef r_lOriginalClaimID As Integer) As Integer` | Gets the original claim ID from the work_claim table |
| `DeleteWorkClaim` | `Public Function DeleteWorkClaim(ByRef lWorkClaimId As Integer) As Integer` | Deletes a work claim record within a transaction |
| `GetReserveRecoverySFU` | `Public Function GetReserveRecoverySFU(ByVal v_lClaimID As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Gets claim status, current reserve and current recovery amounts |
| `GetReserveFromRISFU` | `Public Function GetReserveFromRISFU(ByVal v_lClaimID As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Gets claim RI arrangement line balances |
| `SetClaimStatus` | `Public Function SetClaimStatus(ByVal v_lClaimID As Integer, Optional ByVal v_bIsWorkClaim As Boolean = True, Optional ByVal v_lClaimStatusID As Integer = CLMClosed) As Integer` | Updates the claim status (default to Closed) |

**Module Constants (bCLMCloseClaim.vb):**

| Constant | Value | Description |
|----------|-------|-------------|
| `CLMProvisionalOpenClaim` | `1` | Provisional open claim status |
| `CLMLiveOpenClaim` | `2` | Live open claim status |
| `CLMClosed` | `3` | Closed claim status |
| `CLMReOpened` | `4` | Re-opened claim status |
| `CLMReClosed` | `5` | Re-closed claim status |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_get_claim_status` | `GetClaimStatus` | Gets the claim status for a claim_id |
| `spu_delete_work_claim` | `DeleteWorkClaim` | Deletes a work claim record |
| `spu_GetCurrentReserveRecovery_SFU` | `GetReserveRecoverySFU` | Gets current reserve and recovery amounts |
| `spu_GetCurrentReserveFromRI_SFU` | `GetReserveFromRISFU` | Gets RI arrangement balance |
| `spu_CLM_Update_Claim_status` | `SetClaimStatus` | Updates claim status on the claim/work_claim table |

**Embedded SQL (bCLMCloseClaimSQL.vb):**

| SQL Name | SQL Text | Called By |
|----------|----------|-----------|
| `ACGetOriginalClaimIDSQL` | `SELECT Original_Claim_id FROM work_claim WHERE claim_id = {claim_id}` | `GetOriginalClaimID` |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| *None* | — | No external business components referenced |

---

### 11. bCLMCoinsuranceRecoveries
**Directory:** `Coinsurance Recoveries/`
**Project:** `bCLMCoinsuranceRecoveries`
**Purpose:** Manages coinsurance recovery operations on claims — retrieves, adds, edits, deletes coinsurer shares, manages treatment values, and handles claim party coinsurance links. Uses a collection-based CRUD pattern with Business/Recoveries/Recoveriess classes.

**Business Methods — Business (bCLMCoinsuranceRecoveriesBusiness.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard initialisation; implements `SSP.S4I.Interfaces.IBusiness.Initialise`; creates Recoveriess collection, Lookup component, and data component |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process mode parameters; also syncs InsuranceFileCnt/ClaimID to data component |
| `New` | `Public Sub New()` | Constructor |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes Lookup, collection, and database resources |
| `GetNext` | `Public Function GetNext(Optional ByRef vPartyID As Object = Nothing, Optional ByRef vPartyName As Object = Nothing, Optional ByRef vShare As Object = Nothing, Optional ByRef vShareValue As Object = Nothing) As Integer` | Iterates through the collection returning the next coinsurer record |
| `Cancel` | `Public Function Cancel() As Integer` | Checks if any items in collection have unsaved changes |
| `Update` | `Public Function Update() As Integer` | Loops through collection performing Add/Edit/Delete operations within a transaction |
| `GetDetails` | `Public Function GetDetails(Optional ByRef vLockMode As gPMConstants.PMELockMode = 0, Optional ByRef vClaimID As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing) As Integer` | Populates the collection with coinsurer records for a claim |
| `EditAdd` | `Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vPartyName As Object = Nothing, Optional ByRef vShare As Object = Nothing, Optional ByRef vShareValue As Object = Nothing) As Integer` | Adds a new coinsurer entry to the collection |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vPartyName As Object = Nothing, Optional ByRef vShare As Object = Nothing, Optional ByRef vShareValue As Object = Nothing) As Integer` | Updates an existing coinsurer entry in the collection |
| `EditDelete` | `Public Function EditDelete(ByVal lRow As Integer) As Integer` | Marks a coinsurer entry for deletion in the collection |
| `GetParty` | `Public Function GetParty(ByVal v_lClaimId As Integer, ByRef r_vPartyName As Object) As Integer` | Gets the list of party names for a claim |
| `GetDetailsShare` | `Public Function GetDetailsShare(ByVal v_lClaimId As Integer, ByVal v_lPartyID As Integer, ByRef r_vShare As Object) As Integer` | Gets share value/percent details for a specific party |
| `GetMainShare` | `Public Function GetMainShare(ByVal v_lClaimId As Integer, ByRef r_vShare(,) As Object) As Integer` | Gets coinsurance share summary for the main screen |
| `GetTreatment_Values` | `Public Function GetTreatment_Values(ByRef r_vArray As Object) As Integer` | Gets coinsurance treatment description lookup values |
| `UpdateTreatment` | `Public Function UpdateTreatment(ByVal v_lClaimId As Integer, ByVal v_sDescription As String) As Integer` | Updates the coinsurance treatment value on a claim |
| `GetTreatmentValue` | `Public Function GetTreatmentValue(ByVal v_lClaimId As Object, ByRef vArray(,) As Object) As Integer` | Gets the current treatment value for a claim |
| `GetClaimNumber` | `Public Function GetClaimNumber(ByRef r_vArray As Object) As Integer` | Gets the claim number from the current ClaimID |
| `GetLookupValues` | `Public Function GetLookupValues(ByVal iLookupType As Integer, ByRef vTableArray(,) As Object, ByVal iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer` | Gets CoInsurance_Treatment lookup values |
| `GetBusinessType` | `Public Function GetBusinessType(ByVal v_lInsFileCnt As Integer, ByRef r_vResArray(,) As Object) As Integer` | Gets the business type for an insurance file |
| `getUnderwritingOrAgency` | `Public Function getUnderwritingOrAgency() As Integer` | Determines if business is underwriting or agency |
| `DeleteClaim` | `Public Function DeleteClaim() As Integer` | Deletes a claim record within a transaction |
| `GetSystemOption` | `Public Function GetSystemOption(ByVal v_lOptionNumber As Integer, ByRef r_vResult As Object) As Integer` | Gets a system option value by option number |
| `GetInfoOnlyStatus` | `Public Function GetInfoOnlyStatus(ByVal v_lClaim_Id As Integer, ByRef r_bInfoStatus As Boolean) As Integer` | Checks if claim was previously in Info Only status |
| `GetOriginalClaimID` | `Public Function GetOriginalClaimID(ByVal v_lClaimId As Integer, ByRef r_lOriginalClaimID As Integer) As Integer` | Gets the original claim ID from the base claim |
| `DeleteCoinsurance` | `Public Function DeleteCoinsurance() As Integer` | Deletes all coinsurer entries for the current claim |
| `TidyUpAfterCancel` | `Public Function TidyUpAfterCancel(ByVal v_lClaimId As Integer) As Integer` | Cleans up work table data after a cancel operation |

**Recoveries Class Methods (CLMCoinsuranceRecoveries.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal v_sUsername As String, ByVal v_sPassword As String, ByVal v_iUserID As Integer, ByVal v_iSourceID As Integer, ByVal v_iLanguageID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_iLogLevel As Integer, ByVal v_sCallingAppName As String, Optional ByVal v_vDatabase As dPMDAO.Database = Nothing) As Integer` | Initialises recovery item and creates data component |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes data component |
| `GetDefaults` | `Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vPartyID As Object = Nothing, Optional ByRef vPartyName As Object = Nothing, Optional ByRef vShare As Object = Nothing, Optional ByRef vShareValue As Object = Nothing) As Integer` | Returns default values for a recovery item |
| `SetProperties` | `Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vPartyID As Object = Nothing, Optional ByRef vPartyName As Object = Nothing, Optional ByRef vShare As Object = Nothing, Optional ByRef vShareValue As Object = Nothing) As Integer` | Sets property values with validation |
| `GetProperties` | `Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vPartyID As Object = Nothing, Optional ByRef vPartyName As Object = Nothing, Optional ByRef vShare As Object = Nothing, Optional ByRef vShareValue As Object = Nothing) As Integer` | Returns current property values |
| `SelectItem` | `Public Function SelectItem(ByVal icount As Integer) As Integer` | Reads base details from the database |
| `AddItem` | `Public Function AddItem() As Integer` | Adds a record to the database |
| `DeleteItem` | `Public Function DeleteItem() As Integer` | Deletes a record from the database |
| `UpdateItem` | `Public Function UpdateItem() As Integer` | Updates a record in the database |
| `GetDetails` | *(via data component delegate)* | Gets details from data component |

**Recoveriess Collection Class Methods (CLMCoinsuranceRecoveriess.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `New` | `Public Sub New()` | Constructor; initialises the internal Collection |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes collection |
| `Initialise` | `Public Function Initialise(ByVal v_sUsername As String, ByVal v_sPassword As String, ByVal v_iUserID As Integer, ByVal v_iSourceID As Integer, ByVal v_iLanguageID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_iLogLevel As Integer, ByVal v_sCallingAppName As String, Optional ByVal v_vDatabase As dPMDAO.Database = Nothing) As Integer` | Initialises the collection with credentials |
| `Add` | `Public Function Add(ByRef oNewCLMCoinsuranceRecoveries As bCLMCoinsuranceRecoveries.Recoveries) As Integer` | Adds a Recoveries item to the collection |
| `Count` | `Public Function Count() As Integer` | Returns the number of items |
| `Delete` | `Public Sub Delete(ByRef vKey As Object)` | Removes an item by key |
| `Item` | `Public Function Item(ByRef vKey As Object) As bCLMCoinsuranceRecoveries.Recoveries` | Returns item by key |
| `DeleteAll` | `Public Sub DeleteAll()` | Removes all items from collection |
| `Clear` | `Public Sub Clear()` | Resets the collection |

**Automated Class Methods (bCLMCoinsuranceRecoveriesAutomated.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As dPMDAO.Database = Nothing) As Integer` | Initialises the Automated class |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes and closes database |
| `New` | `Public Sub New()` | Constructor |
| `GetSummary` | `Public Function GetSummary(ByRef vSummaryArray As Object) As Integer` | Gets summary (stub — returns PMTrue) |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process mode parameters |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Stores key values (stub) |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Returns key values (stub) |
| `Start` | `Public Function Start() As Integer` | Performs the automated action (stub — returns PMTrue) |

**NavigatorV3 Class Methods (bNavigatorV3.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises NavigatorV3 and creates Automated instance |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes Automated component |
| `NavigatorV3_SetKeys` | `Public Function NavigatorV3_SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Delegates SetKeys to Automated; implements `aPMNav.NavigatorV3.SetKeys` |
| `NavigatorV3_GetKeys` | `Public Function NavigatorV3_GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Delegates GetKeys to Automated; implements `aPMNav.NavigatorV3.GetKeys` |
| `NavigatorV3_GetSummary` | `Public Function NavigatorV3_GetSummary(ByRef vSummaryArray As Object) As Integer` | Delegates GetSummary to Automated; implements `aPMNav.NavigatorV3.GetSummary` |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_delete_claim` | `DeleteClaim` | Deletes a claim record |
| `spu_get_claim_info_only_status` | `GetInfoOnlyStatus` | Gets info-only status for a claim |
| `spu_CLM_Get_Base_Claim` | `GetOriginalClaimID` | Gets the original/base claim ID |
| `spu_delete_coinsurance` | `DeleteCoinsurance` | Deletes all coinsurer entries for a claim |

**Embedded SQL (bCLMCoinsuranceRecoveriesBusinessSQL.vb):**

| SQL Name | SQL Text | Called By |
|----------|----------|-----------|
| `ACGetSystemOptionSQL` | `SELECT value FROM system_options WHERE option_number = {option_number}` | `GetSystemOption` |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bPMLookup.Business` | `Initialise`, `GetLookupValues` | Lookup component for CoInsurance_Treatment values |
| `dCLMCoinsuranceRecoveries.Data` | `Initialise`, various methods | Data access component for coinsurance CRUD operations |
| `bCLMRiskDetails.Business` | `TidyUpAfterCancel` | Risk details component for cleanup operations |

---

### 12. bCLMDefnFlds
**Directory:** `Define Fields/`
**Project:** `bCLMDefnFlds`
**Purpose:** Manages claim data definition fields for risk types and peril types — custom field definitions (captions, types, display order, read-only flags, lookup associations, party type assignments) attached to risk and peril configurations.

**Business Methods — Business (bCLMBusiness.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard initialisation; creates database, collection, and lookup objects |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes lookup, collection, and database resources |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process mode properties |
| `EditAdd` | `Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vDataDefnID As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vMandatory As Object = Nothing, Optional ByRef vCaption As Object = Nothing, Optional ByRef vRiskTypeID As Object = Nothing, Optional ByRef vType As Object = Nothing, Optional ByRef vDispOrd As Object = Nothing, Optional ByRef vReadOnly As Object = Nothing, Optional ByRef vClmPrtyTypeID As Object = Nothing, Optional ByRef vClmLookupID As Object = Nothing, Optional ByRef vTabID As Object = Nothing, Optional ByRef vTabCaption As Object = Nothing, Optional ByRef vMode As Object = Nothing) As gPMConstants.PMEReturnCode` | Adds a new data definition field to the collection |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vDataDefnID As Integer = 0, Optional ByRef vDescription As Object = Nothing, Optional ByRef vMandatory As Object = Nothing, Optional ByRef vCaption As Object = Nothing, Optional ByRef vRiskTypeID As Object = Nothing, Optional ByRef vType As Object = Nothing, Optional ByRef vDispOrd As Object = Nothing, Optional ByRef vReadOnly As Object = Nothing, Optional ByRef vClmPrtyTypeID As Object = Nothing, Optional ByRef vClmLookupID As Object = Nothing, Optional ByRef vTabID As Object = Nothing, Optional ByRef vTabCaption As Object = Nothing, Optional ByRef vMode As Object = Nothing) As gPMConstants.PMEReturnCode` | Updates an existing data definition field in the collection |
| `EditDelete` | `Public Function EditDelete(ByVal lRow As Integer, ByVal vDataDefnID As Integer, ByVal vMode As Object) As Integer` | Marks a data definition field for deletion |
| `ClearColl` | `Public Sub ClearColl()` | Clears the internal collection |
| `Cancel` | `Public Function Cancel() As Integer` | Checks if collection has unsaved changes |
| `Update` | `Public Function Update() As Integer` | Loops collection, performing Add/Update/Delete on database within a transaction |
| `ApplyMergeCodes` | `Public Function ApplyMergeCodes(ByRef lMode As Integer) As Integer` | Generates stored procedure and wp_fields entries for risk/peril merge codes |
| `New` | `Public Sub New()` | Constructor |
| `GetRiskDataDefn` | `Public Function GetRiskDataDefn(ByRef r_vResultArray(,) As Object, ByVal v_lRiskTypeId As Integer, ByRef r_lRecordsFound As Integer) As Integer` | Retrieves all risk data definition fields for a risk type |
| `GetPerilDataDefn` | `Public Function GetPerilDataDefn(ByRef r_vResultArray(,) As Object, ByVal v_lPerilTypeId As Integer, ByRef r_lRecordsFound As Integer) As Integer` | Retrieves all peril data definition fields for a peril type |
| `ChkCaptionExists` | `Public Function ChkCaptionExists(ByRef r_lRecordCount As Integer, ByVal v_sCaption As String, ByVal v_iTypeId As Integer, ByVal iMode As Integer, ByVal v_iType As Integer) As Integer` | Checks if a caption already exists for the given risk/peril type |
| `ChkDispOrdExists` | `Public Function ChkDispOrdExists(ByRef r_lRecordCount As Integer, ByVal v_sDispOrd As String, ByVal v_iTypeId As Integer, ByVal iMode As Integer) As Integer` | Checks if a display order already exists for the given type |
| `SelLookupTables` | `Public Function SelLookupTables(ByRef rvResultArray(,) As Object) As Integer` | Selects all claim lookup table entries |
| `SelPartyTypes` | `Public Function SelPartyTypes(ByRef rvResultArray(,) As Object) As Integer` | Selects all claim party types |
| `ChkDataDefnIDExists` | `Public Function ChkDataDefnIDExists(ByRef r_lRecordCount As Integer, ByVal v_lDataDefnID As Integer, ByVal iMode As Integer) As Integer` | Checks if a data definition ID is in use before deletion |
| `ChkDataDefnIDForPartyExists` | `Public Function ChkDataDefnIDForPartyExists(ByRef r_lRecordCount As Integer, ByVal v_lTypeID As Integer, ByVal v_lPrtyTypeID As Integer, ByVal iMode As Integer) As Integer` | Checks if a data definition is associated with a party type |
| `ChkLookupExists` | `Public Function ChkLookupExists(ByRef r_lRecordCount As Integer, ByVal v_iLookup As Integer, ByVal v_iTypeId As Integer, ByVal iMode As Integer) As Integer` | Checks if a lookup exists (not currently used) |

**Business Methods — CLMDefnFlds (bCLMDefnFldsCls.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal v_sUsername As String, ByVal v_sPassword As String, ByVal v_iUserID As Integer, ByVal v_iSourceID As Integer, ByVal v_iLanguageID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_iLogLevel As Integer, ByVal v_sCallingAppName As String, Optional ByVal v_vDatabase As dPMDAO.Database = Nothing) As Integer` | Initialises single field instance and data component |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes data component |
| `GetDefaults` | `Public Function GetDefaults(Optional ByRef vDataDefnID As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vMandatory As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vIntialReserve As Object = Nothing, Optional ByRef vRevisedReserve As Object = Nothing, Optional ByRef vReceivedToDate As Object = Nothing, Optional ByRef vRevisionCount As Object = Nothing, Optional ByRef vReceiptId As Object = Nothing, Optional ByRef vPartyClaimID As Object = Nothing, Optional ByRef vReceiptAmount As Object = Nothing, Optional ByRef vDateofReceipt As Object = Nothing, Optional ByRef vPaymentId As Object = Nothing, Optional ByRef vClaimID As Object = Nothing, Optional ByRef vPaymentAmount As Object = Nothing, Optional ByRef vDateofPayment As Object = Nothing, Optional ByRef vComments As Object = Nothing, Optional ByRef vTable As Object = Nothing) As Integer` | Returns default values for a field definition |
| `SetProperties` | `Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vDataDefnID As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vMandatory As Object = Nothing, Optional ByRef vCaption As Object = Nothing, Optional ByRef vRiskTypeID As Object = Nothing, Optional ByRef vType As Object = Nothing, Optional ByRef vDispOrd As Object = Nothing, Optional ByRef vReadOnly As Object = Nothing, Optional ByRef vClmPrtyTypeID As Object = Nothing, Optional ByRef vClmLookupID As Object = Nothing, Optional ByRef vTabID As Object = Nothing, Optional ByRef vTabCaption As Object = Nothing, Optional ByRef vMode As Object = Nothing) As Integer` | Sets property values on the data object |
| `GetProperties` | `Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vDataDefnID As Object = Nothing, Optional ByRef vDescription As Object = Nothing, ...) As Integer` | Returns property values from the data object |
| `AddItem` | `Public Function AddItem() As Integer` | Adds a record to the database |
| `DeleteItem` | `Public Function DeleteItem() As Integer` | Deletes a record from the database |
| `UpdateItem` | `Public Function UpdateItem() As Integer` | Updates a record in the database |

**Business Methods — CLMDefnFldss (bCLMDefnFldss.vb) — Collection class:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Add` | `Public Function Add(ByRef oNewCLMDefnFlds As bCLMDefnFlds.CLMDefnFlds) As Integer` | Adds a field definition to the collection |
| `Count` | `Public Function Count() As Integer` | Returns number of items in collection |
| `Delete` | `Public Sub Delete(ByRef vKey As Integer)` | Removes an item by key |
| `Item` | `Public Function Item(ByRef vKey As Integer) As bCLMDefnFlds.CLMDefnFlds` | Returns an item by key |
| `DeleteAll` | `Public Sub DeleteAll()` | Removes all items from collection |
| `Clear` | `Public Sub Clear()` | Resets the collection |
| `Initialise` | `Public Function Initialise(ByVal v_sUsername As String, ByVal v_sPassword As String, ByVal v_iUserID As Integer, ByVal v_iSourceID As Integer, ByVal v_iLanguageID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_iLogLevel As Integer, ByVal v_sCallingAppName As String, Optional ByVal v_vDatabase As dPMDAO.Database = Nothing) As Integer` | Initialises the collection |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes collection resources |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_get_risk_data_defn` | `GetRiskDataDefn` | Gets all risk data definition fields for a risk type |
| `spu_get_peril_data_defn` | `GetPerilDataDefn` | Gets all peril data definition fields for a peril type |
| `spu_sel_lookup_tables` | `SelLookupTables` | Selects all claim lookup tables |
| `spu_sel_party_types` | `SelPartyTypes` | Selects all claim party types |
| `spu_chk_risk_caption_exists` | `ChkCaptionExists` | Checks risk caption uniqueness |
| `spu_chk_peril_caption_exists` | `ChkCaptionExists` | Checks peril caption uniqueness |
| `spu_chk_data_defn_id_exists` | `ChkDataDefnIDExists` | Checks if data definition ID is in use |
| `spu_chk_data_defn_prty_exists` | `ChkDataDefnIDForPartyExists` | Checks if data definition has party type association |
| `spu_chk_risk_disp_ord_exists` | `ChkDispOrdExists` | Checks risk display order uniqueness |
| `spu_chk_peril_disp_ord_exists` | `ChkDispOrdExists` | Checks peril display order uniqueness |
| `spu_get_clm_for_resv_type` | *(commented out in Update)* | Gets claims for a reserve type |
| `spe_wp_fields_add` | `GenerateStoredProcedure` | Inserts user-defined merge codes into wp_fields table |
| `spu_claimriskdetails` | `GenerateStoredProcedure` | Selects claim risk details for merge code generation |
| `spu_claimperildetails` | `GenerateStoredProcedure` | Selects claim peril details for merge code generation |
| `spu_Claim_Tab_List` | *(available)* | Returns claim tab list |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bPMLookup` | `Business.Initialise` | Lookup values provider |
| `dCLMDefnFlds` | `CLMDefnFlds.Initialise` | Data access layer for field definitions |

---

### 13. bCLMFinSumm
**Directory:** `Financial Summary/`
**Project:** `bCLMFinSumm`
**Purpose:** Retrieves financial summary data for claims — reserve type counts, peril-level reserve/payment details, and recovery/salvage information per claim.

**Business Methods — Business (bCLMFinSumm.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard initialisation |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes database resources |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process mode properties |
| `New` | `Public Sub New()` | Constructor; initialises database |
| `GetResvTypCount` | `Public Function GetResvTypCount(ByRef r_vResultArray(,) As Object, ByVal v_lclm_id As Integer) As Integer` | Gets distinct reserve types for a claim (used for tab count) |
| `GetPerilsForReserve` | `Public Function GetPerilsForReserve(ByRef r_vResultArray(,) As Object, ByVal v_lclaim_id As Integer, ByVal v_lReserve_type_id As Integer) As Integer` | Gets peril details (initial/revised reserve, paid to date, sum insured) for a claim and reserve type |
| `GetPerilTotals` | `Public Function GetPerilTotals(ByRef r_vResultArray(,) As Object, ByVal v_lclaim_id As Integer) As Integer` | Gets totals across all perils for each reserve type of a claim |
| `GetPerilsForRecovery` | `Public Function GetPerilsForRecovery(ByRef r_vResultArray(,) As Object, ByVal v_lclaim_id As Integer, ByVal v_lIs_Salvage As Integer) As Integer` | Gets peril recovery/salvage details for a claim |
| `getUnderwritingOrAgency` | `Public Function getUnderwritingOrAgency() As Integer` | Determines if underwriting or agency business |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_get_resv_typ_count` | `GetResvTypCount` | Gets distinct reserve types for a claim |
| `spu_get_perils_for_reserve` | `GetPerilsForReserve` | Gets peril-level reserve details for a claim and reserve type |
| `spu_get_perils_for_recovery` | `GetPerilsForRecovery` | Gets peril-level recovery/salvage details for a claim |
| `spu_get_peril_totals` | `GetPerilTotals` | Gets totals across all perils per reserve type |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bPMFunc` | `getUnderwritingOrAgency` | Shared utility for underwriting/agency determination |

---

### 14. bCLMFindClaim
**Directory:** `Find Claim/`
**Project:** `bCLMFindClaim`
**Purpose:** Searches for claims using multiple criteria (claim number, client, policy number, dates, status) and manages claim versioning operations (copy, clean up, GIS data synchronisation).

**Business Methods — Business (bCLMFindClaim.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard initialisation |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes database resources |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process mode properties |
| `GetClaimDetails` | `Public Function GetClaimDetails(ByRef r_vResultArray(,) As Object, ByVal v_vSiriusProduct As String, Optional ByVal v_vClaimNumber As Object = Nothing, Optional ByVal v_vClientName As Object = Nothing, Optional ByVal v_vPolicyNumber As Object = Nothing, Optional ByVal v_vRegNumber As Object = Nothing, Optional ByVal v_vLossFromdate As Object = Nothing, Optional ByVal v_vLossToDate As Object = Nothing, Optional ByVal v_vClaimStatus As Object = Nothing, Optional ByVal v_vValidSourceArray As Object = Nothing, Optional ByVal v_vClientResolvedName As Object = Nothing, Optional ByVal v_vInsurer As Object = Nothing, Optional ByVal v_vAccountExecutive As Object = Nothing, Optional ByVal v_vVehicleRegistration As Object = Nothing) As Integer` | Builds and executes dynamic SQL to search claims (broking mode) |
| `GetClaimDetailsUW` | `Public Function GetClaimDetailsUW(ByRef r_vResultArray(,) As Object, ByVal v_vSiriusProduct As String, Optional ByVal v_vClaimNumber As Object = Nothing, Optional ByVal v_vPolicyNumber As Object = Nothing, Optional ByVal v_vClientName As Object = Nothing, Optional ByVal v_vRegNumber As Object = Nothing, Optional ByVal v_vLossFromdate As Object = Nothing, Optional ByVal v_vLossToDate As Object = Nothing, Optional ByVal v_vClaimStatus As Object = Nothing, Optional ByVal v_lCaseID As Integer = 0) As Integer` | Searches claims for underwriting mode via stored procedure |
| `GetClaimDetailsSFU` | `Public Function GetClaimDetailsSFU(ByRef r_vResultArray(,) As Object, Optional ByVal v_vClaimNumber As Object = Nothing, Optional ByVal v_vClientName As Object = Nothing, Optional ByVal v_vPolicyNumber As Object = Nothing, Optional ByVal v_vRegNumber As Object = Nothing, Optional ByVal v_vLossFromdate As Object = Nothing, Optional ByVal v_vLossToDate As Object = Nothing, Optional ByVal v_vClaimStatus As Boolean = False, Optional ByVal v_lCaseID As Integer = 0, Optional ByVal v_sOtherParty As String = "") As Integer` | Searches claims for Sirius For Underwriting mode |
| `DeleteClaim` | `Public Function DeleteClaim(ByVal v_lClaimId As Integer) As Integer` | Deletes work claim records |
| `GetMultiPolicyClaims` | `Public Function GetMultiPolicyClaims(ByRef vInputData As Object, ByRef vOutputData As Object, ByRef v_vSiriusProduct As Object, Optional ByVal v_vClaimNumber As Object = Nothing, Optional ByVal v_vClientName As Object = Nothing, Optional ByVal v_vPolicyNumber As Object = Nothing, Optional ByVal v_vRegNumber As Object = Nothing, Optional ByVal v_vLossFromdate As Object = Nothing, Optional ByVal v_vLossToDate As Object = Nothing, Optional ByVal v_vClaimStatus As Object = Nothing) As Integer` | Gets claims for multiple policies from GIS search results |
| `Apostrophes` | `Public Function Apostrophes(ByRef sString As String) As Integer` | Doubles up apostrophes in search strings for SQL safety |
| `GetGisPolicyLinkDetails` | `Public Function GetGisPolicyLinkDetails(ByVal v_lClaimId As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets GIS policy link details for a claim |
| `ProcessCopyClaim` | `Public Function ProcessCopyClaim(ByVal v_lClaimId As Integer, ByRef r_lCopyClaimId As Integer) As Integer` | Copies a claim with GIS data in a transaction (versioning) |
| `CopyClaim` | `Public Function CopyClaim(ByVal v_lClaimId As Integer, ByRef r_lCopyClaimId As Integer) As Integer` | Copies claim details and returns new claim ID |
| `GetClaimVersions` | `Public Function GetClaimVersions(ByVal v_lClaimId As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets all version details for a claim |
| `IsInfoOnlyClaim` | `Public Function IsInfoOnlyClaim(ByVal v_lClaimId As Integer) As Integer` | Checks if a claim version is information-only |
| `FindClaim` | `Public Function FindClaim(ByVal v_sShortname As String, ByVal v_sInsuranceRef As String, ByVal v_lClaimId As Integer, ByRef r_vResults(,) As Object, Optional ByVal v_bViaClaimVersionList As Boolean = False) As Integer` | Finds claims matching shortname, insurance ref, or claim ID |
| `AddInputParameter` | `Public Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer` | Helper to add a database input parameter |
| `GetOriginalClaimId` | `Public Function GetOriginalClaimId(ByVal v_lClaimId As Integer, ByRef r_lOriginalClaimId As Integer) As Integer` | Returns the base/original claim ID for a versioned claim |
| `CleanUpDirtyClaims` | `Public Function CleanUpDirtyClaims(ByVal v_lClaimId As Integer) As Integer` | Cleans up dirty (uncommitted) claim versions |
| `CheckReferredPayment` | `Public Function CheckReferredPayment(ByVal v_lClaimId As Integer, ByRef r_bStatus As Boolean, Optional ByRef r_iNoofReferredPayments As Integer = 0, Optional ByRef r_cSumofReferredPayments As Decimal = 0) As Integer` | Checks for referred payments on a claim |
| `FindOtherClaims` | `Public Function FindOtherClaims(ByVal v_lPartyCnt As Integer, ByRef r_vOtherClaimDetails(,) As Object) As Integer` | Finds other claims for a party (risk-related) |
| `ProcessPolicyReceiptMediaTypeStatus` | `Public Function ProcessPolicyReceiptMediaTypeStatus(ByVal v_lInsuranceFileId As Integer, ByVal v_dtLossDate As Date, ByRef r_bProceed As Boolean) As Integer` | Checks policy receipt media type status for claim payment |
| `GetUserOtherParty` | `Public Function GetUserOtherParty(ByVal iUserID As Integer, ByRef r_vResultArray(,) As Object) As Long` | Gets the other party ID for the current user (TPA) |
| `GetLatestClaimId` | `Public Function GetLatestClaimId(ByVal v_sClaimRef As String, ByRef r_lLatestClaimId As Integer) As Integer` | Returns latest claim ID for a claim reference |
| `UnLockKey` | `Public Function UnLockKey(ByVal v_sKeyName As String, ByVal v_nKeyValue As Integer, ByVal v_nUserID As Integer) As Integer` | Unlocks a specified key |
| `New` | `Public Sub New()` | Constructor; initialises database |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_find_claim_details_u` | `GetClaimDetailsUW` | Finds claim details for underwriting mode |
| `spu_find_claim_details_sfu` | `GetClaimDetailsSFU` | Finds claim details for Sirius For Underwriting |
| `spu_delete_claims` | `DeleteClaim` | Deletes work claim records |
| `spu_Get_DataModel_Code_For_Claim` | `GetClaimDataModelCode` | Gets datamodel code for a claim |
| `spu_CLM_Get_GIS_Policy_Link_Details` | `GetGisPolicyLinkDetails` | Gets GIS policy link details for a claim |
| `spu_CLM_Update_GIS_Policy_Link_Details` | `UpdateGisPolicyLinkDetails` | Updates GIS policy link with work claim ID |
| `spg_{code}_copy_claim` | `CopyGISClaim` | Copies GIS claim data (dynamic: `spg_` + datamodel code + `_copy_claim`) |
| `spg_{code}_copy_dataset` | `CopyGISDataSet` | Copies GIS dataset (dynamic: `spg_` + datamodel code + `_copy_dataset`) |
| `spu_CLM_Copy_Claim` | `CopyClaim` | Copies claim details and returns new claim ID |
| `spu_CLM_Get_Claim_Version_Details` | `GetClaimVersions` | Gets all claim version details |
| `spu_CLM_Find_Claim` | `FindClaim` | Finds claims matching specified parameters |
| `spu_CLM_Get_Base_Claim` | `GetOriginalClaimId` | Returns base claim ID for a versioned claim |
| `spu_CLM_Clean_Up_Dirty_Claims` | `CleanUpDirtyClaims` | Cleans up dirty claims |
| `spu_CLM_Is_InfoOnly_Version` | `IsInfoOnlyClaim` | Checks if claim version is info-only |
| `spu_claim_GetOtherClaims` | `FindOtherClaims` | Gets other claims for a party |
| `spu_CLM_Get_Referred_Payment_Count` | `CheckReferredPayment` | Gets referred payment count and sum |
| `spu_CLM_Check_Policy_Receipt_MediaType_Status` | `ProcessPolicyReceiptMediaTypeStatus` | Checks policy receipt media type status |
| `spu_get_otherparty_details` | *(available)* | Gets other party details |
| `spu_Get_User_OtherPartyID` | `GetUserOtherParty` | Gets the other party ID for a user |
| `spu_CLM_Get_Latest_ClaimID` | `GetLatestClaimId` | Gets latest claim ID for a claim reference |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bPMLock` | `UnLockKey` | Record locking/unlocking |
| `bGIS` | `UpdateGisPolicyLinkQuoteRef` | GIS Application for quote reference generation |

---

### 15. bCLMFindParty
**Directory:** `Find Party/`
**Project:** `bCLMFindParty`
**Purpose:** Searches for claim parties (claimants, third parties, witnesses, etc.) by name, address, phone number, and party type using dynamic SQL queries.

**Business Methods — Business (bCLMFindPartyCls.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard initialisation |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes event object, database resources, and collections |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process mode properties |
| `New` | `Public Sub New()` | Constructor |
| `SearchByQuery` | `Public Function SearchByQuery(ByRef r_vRefArray(,) As Object, Optional ByRef vName As String = "", Optional ByRef vAddress As String = "", Optional ByRef vPhoneNumber As String = "", Optional ByRef vClaimPartyType As String = "", Optional ByRef vBrokingOrUnderwriting As Object = Nothing) As Integer` | Searches claim parties by name, address, phone, and party type using dynamic SQL |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| *(embedded SQL, not stored procedures)* | `SearchByQuery` | Uses inline SQL to query `party_claim` table with dynamic WHERE clauses. The SQL module defines constants `ACFindPartyName = "spu_find_party_claim"` and `ACFindTypeName = "spu_find_Party_type"` but `ACFindPartyStored = False` and `ACFindTypeStored = False`, meaning these are used as SQL statement names for logging only, not actual stored procedure calls. |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| *(none)* | — | No external business components referenced |

---

### 16. bCLMGetClaimDocument
**Directory:** `Document Production/`
**Project:** `bCLMGetClaimDocument`
**Purpose:** Supports claim document production — retrieves client/policy information for claims, gets large loss advice limits, determines effective dates for document generation, and retrieves spooled document descriptions.

**Business Methods — Business (bCLMClaimBusiness.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard initialisation with lookup component |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes lookup and database resources |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process mode properties |
| `Cancel` | `Public Function Cancel() As Integer` | Always returns PMTrue (no collection to check) |
| `New` | `Public Sub New()` | Constructor |
| `getUnderwritingOrAgency` | `Public Function getUnderwritingOrAgency() As Integer` | Determines if underwriting or agency business |
| `GetClientAndPolicyID` | `Public Function GetClientAndPolicyID(ByVal v_lClaimID As Object, ByRef r_vResultArray(,) As Object) As Integer` | Gets client name and policy ID for a claim |
| `GetLargeLossAdviceLimit` | `Public Function GetLargeLossAdviceLimit(ByRef r_vClaimLimit As String) As Integer` | Gets system option 1014 — the claim value threshold for large loss advice |
| `GetEffectiveDate` | `Public Function GetEffectiveDate(ByVal v_lClaimID As Integer, ByRef r_dtEffectiveDate As Date) As Integer` | Gets the effective date for document production |
| `GetClaimSpooledDesc` | `Public Function GetClaimSpooledDesc(ByVal v_lClaimID As Object, ByRef r_vResultArray(,) As Object) As Integer` | Gets spooled document description for a claim |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_get_claim_clipol_id` | `GetClientAndPolicyID` | Gets client name and policy ID for a claim |
| `spu_CLM_GetDocumentEffectiveDate` | `GetEffectiveDate` | Gets effective date for document production |
| `spu_get_claim_spooled_desc` | `GetClaimSpooledDesc` | Gets spooled document description |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bPMLookup` | `Business.Initialise` | Lookup values provider |
| `bSIROptions` | `GetLargeLossAdviceLimit` | System options retrieval (option 1014) |
| `bPMFunc` | `getUnderwritingOrAgency` | Underwriting/agency determination |

---

### 17. bCLMGetClaimLetter
**Directory:** `Claim Letter/`
**Project:** `bCLMGetClaimLetter`
**Purpose:** Supports claim letter production — retrieves client/policy information for letters, gets large loss advice limits, and determines underwriting/agency context for letter generation.

**Business Methods — Business (bCLMGetClaimLetterCls.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard initialisation with lookup component |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes lookup and database resources |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process mode properties |
| `Cancel` | `Public Function Cancel() As Integer` | Always returns PMTrue (no collection to check) |
| `New` | `Public Sub New()` | Constructor |
| `getUnderwritingOrAgency` | `Public Function getUnderwritingOrAgency() As Integer` | Determines if underwriting or agency business |
| `GetClientAndPolicyID` | `Public Function GetClientAndPolicyID(ByVal v_lClaimID As Object, ByRef r_vResultArray(,) As Object) As Integer` | Gets client name and policy ID for a claim |
| `GetLargeLossAdviceLimit` | `Public Function GetLargeLossAdviceLimit(ByRef r_vClaimLimit As String) As Integer` | Gets system option 1014 — the claim value threshold for large loss advice |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_get_claim_clipol_id` | `GetClientAndPolicyID` | Gets client name and policy ID for a claim (note: SQL constant uses `{call spu_get_claim_clipol_id(?)}` syntax) |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bPMLookup` | `Business.Initialise` | Lookup values provider |
| `bSIROptions` | `GetLargeLossAdviceLimit` | System options retrieval (option 1014) |
| `bPMFunc` | `getUnderwritingOrAgency` | Underwriting/agency determination |

---

### 18. bCLMInfoChklst
**Directory:** `Information Checklist/`
**Project:** `bCLMInfoChklst`
**Purpose:** Manages Information Checklist items (expected services) for claims — tracking external services required, dates requested/critical/received per claim.

**Business Methods — Business class (bCLMBusiness.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises the business component |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes resources |
| `New` | `Public Sub New()` | Constructor |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets process mode properties |
| `GetNext` | `Public Function GetNext(ByVal v_lCurrentRecord As Integer, Optional ByRef vClmExpServId As Object = Nothing, Optional ByRef vClaim_Id As Object = Nothing, Optional ByRef vExpServId As Object = Nothing, Optional ByRef vPrtyClmId As Object = Nothing, Optional ByRef vServTypeId As Object = Nothing, Optional ByRef vService As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vReference As Object = Nothing, Optional ByRef vContact As Object = Nothing, Optional ByRef vDateReq As Object = Nothing, Optional ByRef vDateCrit As Object = Nothing, Optional ByRef vDateRecv As Object = Nothing) As Integer` | Gets the next record from collection |
| `EditAdd` | `Public Function EditAdd(ByRef lRow As Integer, ByRef iStatus As Integer, Optional ByRef vClmExpServId As Object = Nothing, Optional ByRef vClaim_Id As Object = Nothing, Optional ByRef vExpServId As Object = Nothing, Optional ByRef vPrtyClmId As Object = Nothing, Optional ByRef vServTypeId As Object = Nothing, Optional ByRef vService As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vReference As Object = Nothing, Optional ByRef vContact As Object = Nothing, Optional ByRef vDateReq As Object = Nothing, Optional ByRef vDateCrit As Object = Nothing, Optional ByRef vDateRecv As Object = Nothing, Optional ByRef vInfoStatus As Object = Nothing, Optional ByRef vUnderwritingOrAgency As Object = Nothing) As gPMConstants.PMEReturnCode` | Adds a checklist item to collection |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vClmExpServId As Object = Nothing, Optional ByRef vClaim_Id As Object = Nothing, Optional ByRef vExpServId As Object = Nothing, Optional ByRef vPrtyClmId As Object = Nothing, Optional ByRef vServTypeId As Object = Nothing, Optional ByRef vService As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vReference As Object = Nothing, Optional ByRef vContact As Object = Nothing, Optional ByRef vDateReq As Object = Nothing, Optional ByRef vDateCrit As Object = Nothing, Optional ByRef vDateRecv As Object = Nothing, Optional ByRef vInfoStatus As Object = Nothing, Optional ByRef vUnderwritingOrAgency As Object = Nothing) As gPMConstants.PMEReturnCode` | Updates a checklist item in collection |
| `EditDelete` | `Public Function EditDelete(ByVal lRow As Integer, Optional ByRef vInfoStatus As Integer = 0, Optional ByRef vUnderwritingOrAgency As Object = Nothing) As Integer` | Marks a checklist item for deletion |
| `Cancel` | `Public Function Cancel() As Integer` | Checks if collection has unsaved changes |
| `Update` | `Public Function Update() As Integer` | Commits all adds/edits/deletes to database |
| `DeleteClaim` | `Public Function DeleteClaim() As Integer` | Deletes claim records |
| `GetLookupValues` | `Public Function GetLookupValues(ByVal iLookupType As Integer, ByRef vTableArray(,) As Object, ByVal iLanguageID As Integer, ByRef vResultarray(,) As Object) As Integer` | Gets lookup values for info checklist |
| `GetCoinsuranceRecoveries` | `Public Function GetCoinsuranceRecoveries(ByRef r_vResultArray(,) As Object, ByVal v_lClaim_Id As Integer) As Integer` | Gets coinsurance recoveries for a claim |
| `GetReinsuranceRecoveries` | `Public Function GetReinsuranceRecoveries(ByRef r_vResultArray(,) As Object, ByVal v_lClaim_Id As Integer) As Integer` | Gets reinsurance recoveries for a claim |
| `GetPerilDetails` | `Public Function GetPerilDetails(ByRef r_vResultArray(,) As Object, ByVal v_lClaim_Id As Integer) As Integer` | Gets peril details for a claim |
| `GetDefaultCurrencyID` | `Public Function GetDefaultCurrencyID(ByRef r_vResultArray(,) As Object, ByVal v_lClaim_Id As Integer) As Integer` | Gets default currency ID for a claim |
| `GetExpServsAdd` | `Public Function GetExpServsAdd(ByRef r_vResultArray(,) As Object, ByVal v_lRisk_Type_Id As Integer) As Integer` | Gets expected services for add mode |
| `GetExpServsEdit` | `Public Function GetExpServsEdit(ByRef r_vResultArray(,) As Object, ByVal v_lClaim_Id As Integer) As Integer` | Gets expected services for edit mode |
| `CollCount` | `Public Function CollCount() As Integer` | Returns count of items in collection |
| `getUnderwritingOrAgency` | `Public Function getUnderwritingOrAgency() As Integer` | Determines if underwriting or agency business |
| `GetInfoOnlyStatus` | `Public Function GetInfoOnlyStatus(ByVal v_lClaim_Id As Integer, ByRef r_bInfoStatus As Boolean) As Integer` | Checks if claim was previously info-only |
| `CreateClaimHandlerTasks` | `Public Function CreateClaimHandlerTasks(ByVal v_sClaimNumber As String) As Integer` | Creates work manager tasks for external claim handler |
| `AddTaskToWorkManager` | `Public Function AddTaskToWorkManager(ByVal v_sClientName As String, ByVal v_sDescription As String, ByVal v_dtDueDate As Date) As Integer` | Adds a task to the work manager |

**Business Methods — CLMInfoChklst entity class (bCLMInfoChklst.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises entity instance |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes resources |
| `SetProperties` | `Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vClmExpServId As Object = Nothing, Optional ByRef vClaim_Id As Object = Nothing, Optional ByRef vExpServId As Object = Nothing, Optional ByRef vPrtyClmId As Object = Nothing, Optional ByRef vServTypeId As Object = Nothing, Optional ByRef vService As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vReference As Object = Nothing, Optional ByRef vContact As Object = Nothing, Optional ByRef vDateReq As Object = Nothing, Optional ByRef vDateCrit As Object = Nothing, Optional ByRef vDateRecv As Object = Nothing, Optional ByRef vInfoStatus As Object = Nothing, Optional ByRef vUnderwritingOrAgency As Object = Nothing) As Integer` | Sets entity property values |
| `GetProperties` | `Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vClmExpServId As Object = Nothing, Optional ByRef vClaim_Id As Object = Nothing, Optional ByRef vExpServId As Object = Nothing, Optional ByRef vPrtyClmId As Object = Nothing, Optional ByRef vServTypeId As Object = Nothing, Optional ByRef vService As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vReference As Object = Nothing, Optional ByRef vContact As Object = Nothing, Optional ByRef vDateReq As Object = Nothing, Optional ByRef vDateCrit As Object = Nothing, Optional ByRef vDateRecv As Object = Nothing) As Integer` | Gets entity property values |

**Business Methods — CLMInfoChklsts collection class (bCLMInfoChklsts.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Add` | `Public Function Add(ByRef oNewCLMInfoChklst As bCLMInfoChklst.CLMInfoChklst) As Integer` | Adds entity to collection |
| `Count` | `Public Function Count() As Integer` | Returns collection count |
| `Delete` | `Public Sub Delete(ByRef vKey As Integer)` | Removes entity from collection by index |
| `Item` | `Public Function Item(ByRef vKey As Integer) As bCLMInfoChklst.CLMInfoChklst` | Returns entity at index |
| `DeleteAll` | `Public Sub DeleteAll()` | Removes all entities from collection |
| `Clear` | `Public Sub Clear()` | Clears the collection |
| `Initialise` | `Public Function Initialise() As Integer` | Initialises the collection |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_get_cli_name_claim_chklist` | `GetClientNameForTask` | Get client name from claim for work manager tasks |
| `spu_get_exp_servs_add` | `GetExpServsAdd` | Get expected services for add mode |
| `spu_get_exp_servs_Edit` | `GetExpServsEdit` | Get expected services for edit mode |
| `spu_get_sal_coins_details` | `GetCoinsuranceRecoveries` | Get coinsurance recovery details |
| `spu_get_sal_reins_details` | `GetReinsuranceRecoveries` | Get reinsurance recovery details |
| `spu_get_Peril_details` | `GetPerilDetails` | Get peril details for claim |
| `spu_get_CurrencyID` | `GetDefaultCurrencyID` | Get default currency ID |
| `spu_get_claim_clipol_id` | `GetClientAndPolicyID` | Get client and policy ID from claim |
| `spu_delete_claim` | `DeleteClaim` | Delete claim records |
| `spu_get_claim_info_only_status` | `GetInfoOnlyStatus` | Check if claim was previously info-only |
| `spu_CLM_Get_This_Claim_Info_Only_Status` | internal | Get info-only flag from claim table |
| `spu_CLM_Get_Base_Claim` | internal | Get original claim ID |
| `spu_CLM_Show_InfoCheckList` | internal | Auto-show info checklist |
| `spu_get_client_policy_details` | internal | Get client policy details |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bPMLookup` | `Initialise`, `GetLookupValues` | Lookup value retrieval |
| `bSIROptions` | `Business` (internal) | System options |
| `bPMWrkTaskInstance` | `AddTaskToWorkManager` | Work manager task creation |
| `dCLMInfoChklst` | `CLMInfoChklst` entity | Data layer access |

---

### 19. bCLMLossSchedule
**Directory:** `Loss Schedule/`
**Project:** `bCLMLossSchedule`
**Purpose:** Manages loss schedule data for claims — retrieving loss schedule types (peril types with loss schedule type IDs) and item details.

**Business Methods — Business class (bCLMLossScheduleBusiness.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises the business component |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes resources |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets process mode properties |
| `GetLookupValues` | `Public Function GetLookupValues(ByVal iLookupType As Integer, ByRef vTableArray(,) As Object, ByVal iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer` | Gets lookup values for loss schedule |
| `GetSiriusProduct` | `Public Function GetSiriusProduct(ByRef r_sSiriusProduct As String) As Integer` | Gets Sirius product type |
| `getUnderwritingOrAgency` | `Public Function getUnderwritingOrAgency() As Integer` | Determines if underwriting or agency business |
| `GetLossScheduleTypes` | `Public Function GetLossScheduleTypes(ByRef r_vResultArray(,) As Object) As Integer` | Gets undeleted peril types with loss schedule type IDs |
| `GetItemDetails` | `Public Function GetItemDetails(ByVal v_lItemDetailId As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets data from item detail table |
| `New` | `Public Sub New()` | Constructor |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_Get_Loss_Schedule_Type` | `GetLossScheduleTypes` | Get loss schedule types (peril types) |
| `spu_get_item_details` | `GetItemDetails` | Get item details by ID |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bPMLookup` | `Initialise`, `GetLookupValues` | Lookup value retrieval |
| `bBackOfficeLink` | `GetSiriusProduct` | Back office product identification |

---

### 20. bCLMPaymentMethod
**Directory:** `Payment Method/`
**Project:** `bCLMPaymentMethod`
**Purpose:** Manages claim payment method operations — payment details retrieval, rejection, currency overrides, co-insurer details, media types, and agent validation for claims payment processing.

**Business Methods — Business class (bCLMBusiness.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises the business component |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes resources |
| `New` | `Public Sub New()` | Constructor |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets process mode properties |
| `GetMediaTypeId` | `Public Function GetMediaTypeId(ByVal sMediaTypeCode As String, ByRef r_lMediaTypeId As Integer) As Integer` | Returns the ID of a media type given its code |
| `GetPaymentDetails` | `Public Function GetPaymentDetails(ByVal v_lClaimId As Integer, ByVal v_lClaimPerilId As Integer, ByVal v_lSequenceNo As Integer, ByVal v_lClaimPaymentId As Integer, ByRef r_vPaymentDetailsArray(,) As Object) As Integer` | Gets payment details for a claim |
| `GetInsurerDetails` | `Public Function GetInsurerDetails(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lClaimId As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets co-insurer breakdown details |
| `UpdateClaimPayment` | `Public Function UpdateClaimPayment(ByVal v_lDocumentId As Integer, ByVal v_lClaimPaymentId As Integer) As Integer` | Updates claim payment document ID |
| `RejectPayment` | `Public Function RejectPayment(ByVal v_lClaimPerilId As Integer, ByVal v_lSequenceNo As Integer, ByVal v_lClaimPaymentId As Integer) As Integer` | Rejects a claim payment |
| `GetOption` | `Public Function GetOption(ByVal v_iOptionNumber As Integer, ByRef r_sOptionValue As String) As Integer` | Gets a system option value |
| `getUnderwritingOrAgency` | `Public Function getUnderwritingOrAgency() As Integer` | Determines if underwriting or agency business |
| `IsAgentCancelled` | `Public Function IsAgentCancelled(ByVal v_lAgentID As Integer, ByRef r_bIsCancelled As Boolean) As Integer` | Checks if an agent is cancelled |
| `GetOptionAgent` | `Public Function GetOptionAgent(ByVal v_lProductID As Integer, ByRef r_bValue As Boolean) As Integer` | Gets agent option from product |
| `GetOptionMediaTypeMandatory` | `Public Function GetOptionMediaTypeMandatory(ByVal v_lProductID As Integer, ByRef r_bValue As Boolean) As Integer` | Gets media type mandatory option from product |
| `GetBaseCurrencyID` | `Public Function GetBaseCurrencyID(ByVal v_lSourceID As Integer, ByRef r_iCurrencyID As Integer) As Integer` | Gets branch base currency ID |
| `GetClaimCurrencyByRefs` | `Public Function GetClaimCurrencyByRefs(ByVal v_sClaimRef As String, ByVal v_sPolicyNumber As String, ByRef r_iCurrencyID As Integer) As Integer` | Gets claim currency from policy and claim number |
| `GetUserCurrencyAuthorities` | `Public Function GetUserCurrencyAuthorities(ByVal v_iUserID As Integer, ByRef r_bChangeDate As Boolean, ByRef r_bChangeRate As Boolean) As Integer` | Gets user currency authorities |
| `UpdateOverrideRates` | `Public Function UpdateOverrideRates(ByVal v_lScreenMethod As Integer, ByVal v_lClaimId As Integer, ByVal v_lOverrideID As Integer, ByVal v_dtRateDate As Date, ByVal v_dCurrencyBaseRate As Double, ByVal v_dAccountBaseRate As Double, ByVal v_dSystemBaseRate As Double) As Integer` | Updates override exchange rates for payment/receipt |
| `GetClaimPayableAccountID` | `Public Function GetClaimPayableAccountID(ByRef r_lAccountID As Integer) As Integer` | Gets the CLMPAYABLE account ID |
| `GetClaimBaseCurrencyDetails` | `Public Function GetClaimBaseCurrencyDetails(ByVal v_lClaimId As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets base currency details for a claim |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_check_agent_cancelled` | `IsAgentCancelled` | Check if agent is cancelled |
| `spe_Product_Sel` | `GetOptionAgent`, `GetOptionMediaTypeMandatory` | Select product options |
| `spu_get_claim_currency_by_refs` | `GetClaimCurrencyByRefs` | Get claim currency by policy/claim refs |
| `spu_update_payment_rates` | `UpdateOverrideRates` | Update payment exchange rates |
| `spu_update_receipt_rates` | `UpdateOverrideRates` | Update receipt exchange rates |
| `spu_reject_payment` | `RejectPayment` | Reject a claim payment |
| `spu_select_payment_details` | `GetPaymentDetails` | Select payment details |
| `spu_ACT_Select_MediaType_ByCode` | `GetMediaTypeId` | Select media type by code |
| `spu_CLM_Get_Claim_Base_Currency_Details` | `GetClaimBaseCurrencyDetails` | Get claim base currency details |
| `spu_CLM_Get_CoInsurer_Split` | `GetInsurerDetails` | Get co-insurer breakdown |
| `Update claim_payment SET document_id...` | `UpdateClaimPayment` | Update payment document ID (embedded SQL) |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bSIROptions` | `GetOption` | System options retrieval |

---

### 21. bCLMPeril
**Directory:** `Generic Peril/`
**Project:** `bCLMPeril`
**Purpose:** The largest claims component — manages all claim peril operations including reserves, payments, receipts, recoveries, tax calculations, Orion posting, coinsurance/reinsurance, claim numbering, currency handling, and GIS rule execution.

**Business Methods — Business class (bCLMPerilBusiness.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises the business component |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes resources |
| `New` | `Public Sub New()` | Constructor |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets process mode properties |
| `BeginTrans` | `Public Function BeginTrans() As Integer` | Begins a database transaction |
| `CommitTrans` | `Public Function CommitTrans() As Integer` | Commits a database transaction |
| `RollbackTrans` | `Public Function RollbackTrans() As Integer` | Rolls back a database transaction |
| `GetControls` | `Public Function GetControls(ByRef r_vControlsArray As Object) As Integer` | Gets user-defined controls for peril type |
| `GetReserveType` | `Public Function GetReserveType(ByRef r_vReserveTypeArray As Object) As Integer` | Gets all reserve types |
| `GetReserveDetails` | `Public Function GetReserveDetails(ByVal v_vPolicyID As Object, ByVal v_vRiskID As Object, ByRef r_vReserveDetailsArray(,) As Object) As Integer` | Gets reserve details for a policy/risk |
| `GetPaymentList` | `Public Function GetPaymentList(ByVal lClaimID As Integer, ByVal lReserveID As Integer, ByRef r_vPaymentList As Object) As Integer` | Gets payment list for claim/reserve |
| `GetPaymentDetails` | `Public Function GetPaymentDetails(ByRef r_vPaymentDetailsArray(,) As Object) As Integer` | Gets payment details |
| `GetPartyDetails` | `Public Function GetPartyDetails(ByRef r_vPartyDetailsArray As Object) As Integer` | Gets party details |
| `AddParty` | `Public Function AddParty(ByVal v_vPartyIDArray As Object) As Integer` | Adds a party to claim |
| `DeleteParty` | `Public Function DeleteParty(ByVal v_vPartyIDArray As Object) As Integer` | Deletes a party from claim |
| `UpdateReserveDetails` | `Public Function UpdateReserveDetails(ByVal v_vReserveDetailsArray(,) As Object) As Integer` | Updates reserve details |
| `UpdateGeneral` | `Public Function UpdateGeneral(ByVal v_vGeneralDetailsArray(,) As Object) As Integer` | Updates user-defined field details |
| `GetClaimLookup` | `Public Function GetClaimLookup(ByVal v_vclaimlookupid As Object, ByRef r_vLookupArray(,) As Object) As Integer` | Gets claim lookup values |
| `GetRecoveryDetails` | `Public Function GetRecoveryDetails(ByVal v_vRecoveryType As Object, ByRef r_vRecoveryDetailsArray(,) As Object) As Integer` | Gets recovery details by type |
| `AddComments` | `Public Function AddComments(ByVal v_vComments As String) As Integer` | Adds comments to claim peril |
| `AddCommentsUW` | `Public Function AddCommentsUW(ByVal v_vComments As Object) As Integer` | Adds underwriting comments |
| `GetComments` | `Public Function GetComments(ByRef r_vComments As Object) As Integer` | Gets comments for claim peril |
| `GetCommentsUW` | `Public Function GetCommentsUW(ByRef r_vComments As Object) As Integer` | Gets underwriting comments |
| `CreateEvent` | `Public Function CreateEvent(ByVal v_lEventTypeId As Integer, ByVal v_sDescription As String, Optional ByRef v_lEventCnt As Integer = 0) As Integer` | Creates an event log entry |
| `GetClaimPerilDetails` | `Public Function GetClaimPerilDetails(ByRef r_vClaimPerilDetails As Object) As Integer` | Gets claim peril details |
| `GetOption` | `Public Function GetOption(ByVal v_iOptionNumber As Integer, ByRef r_sOptionValue As Integer) As Integer` | Gets a system option value |
| `GetPartyName` | `Public Function GetPartyName(ByVal v_lPartyCnt As Integer, ByVal v_sFieldName As String, ByRef r_sResult As String) As Integer` | Gets party name by party count |
| `GetClassOfBusiness` | `Public Function GetClassOfBusiness(ByRef r_lId As Integer, ByRef r_sCode As String, Optional ByVal v_lPerilTypeID As Integer = 0, Optional ByVal v_lClaimPerilId As Integer = 0) As Integer` | Gets class of business for peril |
| `GetSystemOption` | `Public Function GetSystemOption(ByVal v_lOptionNumber As Integer, ByRef r_sReturn As String) As Integer` | Gets system option value |
| `GetClaimNumber` | `Public Function GetClaimNumber(ByVal v_lClaimId As Integer, ByRef r_sClaimRef As String) As Integer` | Gets claim number by ID |
| `GetClaimNumberFromClaim` | `Public Function GetClaimNumberFromClaim(ByVal v_lClaimId As Integer, ByRef r_sClaimRef As String) As Integer` | Gets claim number from claim table |
| `GetClaimClientAndAgent` | `Public Function GetClaimClientAndAgent(ByVal v_lClaimId As Integer, ByRef r_vClaimClientAndAgent As Object) As Integer` | Gets client and agent details for claim |
| `GetTaskGroupCode` | `Public Function GetTaskGroupCode(ByVal v_sTaskCode As String, ByRef r_sTaskGroupCode As String) As Integer` | Gets task group code |
| `GetOriginalClaimID` | `Public Function GetOriginalClaimID(ByVal v_lClaimId As Integer, ByRef r_lOriginalClaimID As Integer) As Integer` | Gets original claim ID from work table |
| `GetClientPolicyDetails` | `Public Function GetClientPolicyDetails(ByVal v_lInsuranceFileCnt As Integer, Optional ByRef r_lPartyCnt As Integer = 0, Optional ByRef r_sPartyShortName As String = "", Optional ByRef r_lInsuranceFolderCnt As Integer = 0, Optional ByRef r_sInsuranceRef As String = "") As Integer` | Gets client and policy details |
| `GetRiskDetails` | `Public Function GetRiskDetails(ByVal v_lClaimId As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Gets risk details for claim |
| `CheckReferredPayment` | `Public Function CheckReferredPayment(ByVal v_lClaimId As Integer, ByRef r_bStatus As Boolean, Optional ByRef r_iNoofReferredPayments As Integer = 0, Optional ByRef r_cSumofReferredPayments As Decimal = 0) As Integer` | Checks for referred payments on claim |
| `GetTaxTypesTaxBands` | `Public Function GetTaxTypesTaxBands(ByRef r_vResultArray(,) As Object) As Integer` | Gets tax types and associated tax bands |
| `GetPolicyType` | `Public Function GetPolicyType(ByVal v_lPolicyId As Integer, ByRef r_sType As String) As Integer` | Gets policy type (e.g. GII) |
| `GetClaimCurrency` | `Public Function GetClaimCurrency(ByVal v_lClaimId As Integer, ByRef r_lCurrencyID As Integer, ByRef r_sCurrencyDesc As String) As Integer` | Gets claim currency ID and description |
| `RetrieveCurrenciesForBranch` | `Public Function RetrieveCurrenciesForBranch(ByRef iSourceID As Integer, ByRef vReturnArray(,) As Object) As Integer` | Gets currencies for branch |
| `RetrieveCurrenciesForClaimBranch` | `Public Function RetrieveCurrenciesForClaimBranch(ByVal v_lClaimId As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets currencies for claim's branch |
| `GetSafeHarbourDetails` | `Public Function GetSafeHarbourDetails(ByRef r_vResults(,) As Object) As Integer` | Gets safe harbour details |
| `GetClaimPaymentToDetails` | `Public Function GetClaimPaymentToDetails(ByRef r_vResults(,) As Object) As Integer` | Gets claim payment-to details |
| `GetLookupsByEffectiveDate` | `Public Function GetLookupsByEffectiveDate(ByVal v_sTableName As String, ByRef r_vResults(,) As Object) As Integer` | Gets lookups by effective date |
| `GetClaimPaymentDetails` | `Public Function GetClaimPaymentDetails(ByVal v_lClaimId As Integer, ByVal v_lClaimPaymentId As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets claim payment details |
| `GetCoInsurerDetails` | `Public Function GetCoInsurerDetails(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lClaimId As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets co-insurer breakdown |
| `GenerateClaimNumber` | `Public Function GenerateClaimNumber(ByVal v_lBusinessType As Integer, ByVal v_iBranchId As Integer, ByVal v_lProductId As Integer, ByVal v_lAgentId As Integer, ByRef r_sGeneratedClaimNumber As String, ByVal v_sLossYear As String, ByVal v_sReportedYear As String) As Integer` | Generates a claim number |
| `GetOriginalClaimNo` | `Public Function GetOriginalClaimNo(ByVal v_lClaimId As Integer, ByRef r_lOriginalClaimID As Integer) As Integer` | Gets original claim number |
| `UpdateCoInsurerDetails` | `Public Function UpdateCoInsurerDetails(ByVal v_lReserveId As Integer, ByVal v_lClaimPerilId As Integer, ByVal r_vCoInsurers(,) As Object) As Integer` | Updates co-insurer details |
| `CreateReserveEntries` | `Public Function CreateReserveEntries(ByVal v_lClaimPerilId As Integer) As Integer` | Creates reserve entries for claim peril |
| `GetClaimDetails` | `Public Function GetClaimDetails(ByVal v_lClaimId As Integer, ByVal v_lClaimPerilId As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets claim details for payment |
| `GetCurrentClaimPaymentReserveDetails` | `Public Function GetCurrentClaimPaymentReserveDetails(ByVal v_lClaimPerilId As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets current reserves and associated payments |
| `GetOtherPartyDetails` | `Public Function GetOtherPartyDetails(ByVal v_lPartyCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets other party account details |
| `GetAccountDetailsByShortCode` | `Public Function GetAccountDetailsByShortCode(ByVal v_sShortCode As String, ByRef r_vResults(,) As Object) As Integer` | Gets account details by short code |
| `GetClaimPaymentItemDetails` | `Public Function GetClaimPaymentItemDetails(ByVal v_lClaimPaymentId As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets claim payment item details |
| `GetTaxGroupDetails` | `Public Function GetTaxGroupDetails(ByVal v_vIsWithHoldingTax As Object, ByRef r_vResults(,) As Object) As Integer` | Gets tax group details |
| `CalculateTaxAmounts` | `Public Function CalculateTaxAmounts(ByVal v_lCompanyId As Integer, ByVal v_lTaxGroupId As Integer, ByVal v_sTranstype As String, ByVal v_lCurrencyId As Integer, ByVal v_lLossCurrencyId As Integer, ByVal v_crAmount As Decimal, ByRef r_crTaxCurrencyAmount As Decimal, ByRef r_crTaxLossAmount As Decimal, ByRef r_crTaxBaseAmount As Decimal, ByVal v_lClaimPerilId As Integer, ByVal v_lClaimPaymentId As Integer, ByVal v_lClaimReceiptId As Integer, ByVal v_lClaimPaymentItemId As Integer, ByVal v_lClaimReceiptItemId As Integer) As Integer` | Calculates tax amounts for payment/receipt |
| `ExecuteAdvancedTaxScript` | `Public Function ExecuteAdvancedTaxScript(ByVal v_lTaxScriptMode As Integer, ByVal v_sTaxScriptName As String, ByVal v_vTaxParameters() As Object, ByRef r_vUpdatedTaxParameters() As Object, ...) As Integer` | Executes GIS advanced tax script |
| `GetTaxGroupTaxBandDetails` | `Public Function GetTaxGroupTaxBandDetails(ByRef r_vResults(,) As Object) As Integer` | Gets tax group tax band links |
| `SaveClaimPayment` | `Public Function SaveClaimPayment(ByVal v_vClaimPayment() As Object, ByRef r_lClaimPaymentId As Integer) As Integer` | Saves claim payment to database |
| `SaveClaimPaymentItem` | `Public Function SaveClaimPaymentItem(ByVal v_vClaimPaymentItem As Object, ByRef r_lClaimPaymentItemId As Integer) As Integer` | Saves claim payment item |
| `SaveTaxCalculationItem` | `Public Function SaveTaxCalculationItem(ByVal v_vTaxCalculation As Object, ByRef r_lTaxCalculationCnt As Integer) As Integer` | Saves tax calculation item |
| `GetClaimPaymentItemTax` | `Public Function GetClaimPaymentItemTax(ByVal v_lClaimPaymentItemId As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets tax details for claim payment |
| `UpdateClaimPaymentItemReserve` | `Public Function UpdateClaimPaymentItemReserve(ByVal v_lReserveId As Integer, ByVal v_crThisRevision As Decimal, ByVal v_crThisPayment As Decimal) As Integer` | Updates reserve based on payment amount |
| `PostPaymentToOrion` | `Public Function PostPaymentToOrion(ByVal v_lClaimPaymentId As Integer, ByVal v_sClaimNumber As String, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lClaimId As Integer, ByVal v_lClaimPerilId As Integer, ByVal v_cPayAmount As Decimal, ByVal v_sCreditAccountCode As String, ByVal v_sCOBCode As String, ByVal v_lCOBId As Integer, ByVal v_bPostClaimTax As Boolean, Optional ByVal v_lPartyCnt As Integer = 0) As Integer` | Posts payment to Orion accounting |
| `PostReserveAdjustmentToOrion` | `Public Function PostReserveAdjustmentToOrion(ByVal v_crRevisionAmount As Decimal, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lClaimId As Integer, ByVal v_sClaimNo As String, ByVal v_lPerilID As Integer, ByVal v_sCOBCode As String, ByVal v_lCOBId As Integer) As Integer` | Posts reserve adjustment to Orion |
| `PostBrokingPaymentToOrion` | `Public Function PostBrokingPaymentToOrion(ByVal v_lClaimPaymentId As Integer, ByVal v_sClaimNumber As String, ByVal v_lInsuranceFileCnt As Integer, ByVal v_cPayAmount As Decimal, ByVal v_sCreditAccountCode As String, ByVal v_bPostClaimTax As Boolean, ByVal v_lMediaType As Integer, ByVal v_sComments As String, ByVal v_lPartyCnt As Integer, ByRef r_lClientAccountId As Integer) As Integer` | Posts broking payment to Orion |
| `PostBrokingReceiptToOrion` | `Public Function PostBrokingReceiptToOrion(ByVal v_lClaimReceiptId As Integer, ByVal v_lClaimId As Integer, ByVal v_sClaimNumber As String, ByVal v_lInsuranceFileCnt As Integer, ByVal v_cReceiptAmount As Decimal, ByVal v_sDebitAccountCode As String, ByVal v_bPostClaimTax As Boolean, ByVal v_lMediaType As Integer, ByVal v_sComments As String, ByVal v_lPartyCnt As Integer, ByVal v_vInsurerDetails(,) As Object, ByRef r_lClientAccountId As Integer) As Integer` | Posts broking receipt to Orion |
| `GetClaimPerilRecoveryDetails` | `Public Function GetClaimPerilRecoveryDetails(ByVal v_lClaimPerilId As Integer, ByVal v_bIsSalvage As Boolean, ByRef r_vResults(,) As Object, Optional ByRef v_lClaimReceiptId As Integer = 0) As Integer` | Gets claim peril recovery details |
| `SaveClaimReceipt` | `Public Function SaveClaimReceipt(ByVal v_vClaimReceiptDetails As Object, ByRef r_lClaimReceiptId As Integer) As Integer` | Saves claim receipt |
| `SaveClaimReceiptItem` | `Public Function SaveClaimReceiptItem(ByVal v_vClaimReceiptItem As Object, ByRef r_lClaimReceiptItemId As Integer) As Integer` | Saves claim receipt item |
| `UpdateClaimReceiptItemRecovery` | `Public Function UpdateClaimReceiptItemRecovery(ByVal v_lRecoveryId As Integer, ByVal v_crThisRevision As Decimal, ByVal v_crThisReceipt As Decimal, ByVal v_crTaxAmount As Decimal) As Integer` | Updates recovery with receipt details |
| `PostReceiptToOrion` | `Public Function PostReceiptToOrion(ByVal v_bIsSalvage As Integer, ByVal v_lClaimReceiptId As Integer, ByVal v_sClaimNumber As String, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lClaimId As Integer, ByVal v_lClaimPerilId As Integer, ByVal v_crGrossReceiptAmount As Decimal, ByVal v_crNetReceiptAmount As Decimal, ByVal v_crTaxAmount As Decimal, ByVal v_sDebitAccountCode As String, ByVal v_sCOBCode As String, ByVal v_lCOBId As Integer, ByVal v_bPostClaimTax As Boolean, Optional ByVal v_lPartyCnt As Integer = 0, ...) As Integer` | Posts receipt to Orion accounting |
| `CreateClaimReserves` | `Public Function CreateClaimReserves(ByVal v_lClaimPerilId As Integer, ByVal v_lRiskId As Integer, ByVal v_lInsuranceFileCnt As Integer) As Integer` | Creates claim reserves |
| `GetCoinsurance` | `Public Function GetCoinsurance(ByVal v_lClaimPerilId As Integer, ByVal v_bIsSalvage As Boolean, ByRef r_vResults(,) As Object) As Integer` | Gets coinsurance details |
| `GetReinsurance` | `Public Function GetReinsurance(ByVal v_lClaimPerilId As Integer, ByVal v_bIsSalvage As Boolean, ByRef r_vResults(,) As Object) As Integer` | Gets reinsurance details |
| `UpdateClaimReinsurance` | `Public Function UpdateClaimReinsurance(ByVal v_lClaimPerilId As Integer) As Integer` | Updates reinsurance allocations |
| `GetMediaTypes` | `Public Function GetMediaTypes(ByRef r_vResults(,) As Object, Optional ByVal iPaymentsOnly As Integer = 0) As Integer` | Gets media type lookup details |
| `GetClaimBranchCurrencies` | `Public Function GetClaimBranchCurrencies(ByVal v_lSourceID As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets branch currencies for claims |
| `GetUserCanChangeReserves` | `Public Function GetUserCanChangeReserves(ByVal v_lUserID As Integer, ByRef r_bCanChangeReserves As Boolean) As Integer` | Checks if user can change reserves |
| `GetClaimXOLineCount` | `Public Function GetClaimXOLineCount(ByVal v_lClaimId As Integer, ByRef r_bHaveXOLLines As Boolean) As Integer` | Gets excess of loss line count |
| `GetReceiptList` | `Public Function GetReceiptList(ByVal lClaimID As Integer, ByVal vRecoveryType As gPMConstants.PMEReturnCode, ByRef lRecoveryID As Integer, ByRef r_vReceiptList As Object, ByVal nSalvageAndTPRecoveryReceipts As Integer) As Integer` | Gets receipt list for claim (overload 1) |
| `GetReceiptList` | `Public Function GetReceiptList(ByVal lClaimID As Integer, ByVal vRecoveryType As gPMConstants.PMEReturnCode, ByRef lRecoveryID As Integer, ByRef r_vReceiptList As Object) As Integer` | Gets receipt list for claim (overload 2) |
| `GetClaimReceiptDetails` | `Public Function GetClaimReceiptDetails(ByVal v_lclaim_Receipt_id As Integer, ByRef r_vResultArray(,) As Object, ByRef v_lClaimId As Integer) As Integer` | Gets claim receipt details |
| `GetClaimReceiptItemTaxDetails` | `Public Function GetClaimReceiptItemTaxDetails(ByVal v_lclaim_Receipt_Item_id As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Gets receipt item tax details |
| `GetClaimReceiptItemDetails` | `Public Function GetClaimReceiptItemDetails(ByVal v_lClaimReceiptId As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets claim receipt item details |
| `UpdateClaimTransactionType` | `Public Function UpdateClaimTransactionType(...) As Integer` | Updates claim transaction type |
| `IsAccountExists` | `Public Function IsAccountExists(ByVal AccountCode As String, ByRef IsExists As Boolean) As Integer` | Checks if account exists |
| `GetDefaultBankAccount` | `Public Function GetDefaultBankAccount(v_nSourceID As Integer, v_nMediaType As Integer, v_nProductId As Integer, ByRef r_oResults(,) As Object) As Integer` | Gets default bank account |
| `GetCurrencyFromBankAccount` | `Public Function GetCurrencyFromBankAccount(ByVal v_nBankAccountId As Integer, ByRef r_oResults(,) As Object) As Integer` | Gets currency from bank account |
| `GetDefaultCashListItemReceiptType` | `Public Function GetDefaultCashListItemReceiptType(ByRef r_oResults(,) As Object) As Integer` | Gets default cash list receipt type |
| `GetMediaTypeLookUpDetails` | `Public Function GetMediaTypeLookUpDetails(ByRef r_oResults(,) As Object) As Integer` | Gets media type lookup details |
| `GetClaimPaymentTotal` | `Public Function GetClaimPaymentTotal(ByVal nClaimId As Integer, ByRef r_oResults(,) As Object) As Integer` | Gets claim payment total |
| `GetClaimRecoveries` | `Public Function GetClaimRecoveries(ByVal nClaimPerilID As Integer, ByRef r_oResultArray(,) As Object) As Integer` | Gets recoveries by peril |
| `SaveClaimRecoveries` | `Public Function SaveClaimRecoveries(...) As Integer` | Saves claim recovery |
| `UpdateThisClaimPaymentDetails` | `Public Function UpdateThisClaimPaymentDetails(ByVal nClaimID As Integer, ...) As Integer` | Updates claim payment details |
| `GetCurrencyRatesToOverride` | `Public Function GetCurrencyRatesToOverride(ByVal v_nClaimId As Long, ...) As Integer` | Gets currency rates for override |
| `OverrideClaimCurrencyRate` | `Public Function OverrideClaimCurrencyRate(ByVal v_nClaimId As Long, ...) As Integer` | Overrides claim currency rate |
| `GetUsersReserveLimit` | `Public Function GetUsersReserveLimit(ByRef dReserveLimit As Decimal) As Integer` | Gets user's reserve limit |

**Business Methods — Automated class (bCLMPeril Automated.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises automated class |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes resources |
| `New` | `Public Sub New()` | Constructor |
| `GetSummary` | `Public Function GetSummary(ByRef vSummaryArray As Object) As Integer` | Gets summary array |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets process modes |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Stores key array parameters |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Gets key array |
| `Start` | `Public Function Start() As Integer` | Performs automated action |

**Business Methods — NavigatorV3 class (bNavigatorV3.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises NavigatorV3 |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes resources |
| `New` | `Public Sub New()` | Constructor |
| `NavigatorV3_SetKeys` | `Public Function NavigatorV3_SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Sets keys via Navigator interface |
| `NavigatorV3_GetKeys` | `Public Function NavigatorV3_GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Gets keys via Navigator interface |
| `NavigatorV3_GetSummary` | `Public Function NavigatorV3_GetSummary(ByRef vSummaryArray As Object) As Integer` | Gets summary via Navigator interface |
| `NavigatorV3_SetProcessModes` | `Public Function NavigatorV3_SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets process modes via Navigator |
| `NavigatorV3_Start` | `Public Function NavigatorV3_Start() As Integer` | Starts processing via Navigator |

**Business Methods — cPaymentTaxParameters class (cPaymentTaxParameters.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `ArrayToData` | `Public Function ArrayToData(ByVal v_vDataArray() As Object) As Integer` | Populates properties from array |
| `DataToArray` | `Public Function DataToArray(ByRef v_vDataArray() As Object) As Integer` | Exports properties to array |

**Business Methods — cReceiptTaxParameters class (cReceiptTaxParameters.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `ArrayToData` | `Public Function ArrayToData(ByVal v_vDataArray() As Object) As Integer` | Populates properties from array |
| `DataToArray` | `Public Function DataToArray(ByRef v_vDataArray() As Object) As Integer` | Exports properties to array |

**Stored Procedures (from bCLMPeril AutomatedSQL.vb):**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_get_claimnumber` | `GetClaimNumber`, `GetClaimNumberFromClaim` | Get claim number |
| `spu_CLM_Get_Client_And_Agent_Details` | `GetClaimClientAndAgent` | Get claim client and agent details |
| `spu_CLM_Get_Base_Claim` | `GetOriginalClaimID`, `GetOriginalClaimNo` | Get original claim ID |
| `spu_get_client_policy_details` | `GetClientPolicyDetails` | Get client policy details |
| `spu_CLM_Get_Referred_Payment_Count` | `CheckReferredPayment` | Get referred payment count |
| `spu_Get_Tax_Types_and_Bands` | `GetTaxTypesTaxBands` | Get tax types and bands |
| `spu_Get_Policy_Type` | `GetPolicyType` | Get policy type |
| `spu_Get_Claim_Currency` | `GetClaimCurrency` | Get claim currency |
| `spu_Get_Claim_Branch_Currency` | `RetrieveCurrenciesForClaimBranch` | Get claim branch currencies |
| `spu_CLM_Get_Safe_Harbour_Details` | `GetSafeHarbourDetails` | Get safe harbour details |
| `spu_CLM_Get_Claim_Payment_To` | `GetClaimPaymentToDetails` | Get claim payment-to details |
| `spu_CLM_Get_Claim_Payment_Details` | `GetClaimPaymentDetails` | Get claim payment details |
| `spu_CLM_Get_Claim_Details_For_Payment` | `GetClaimDetails` | Get claim details for payment |
| `spu_SIR_Get_Lookup_Values_By_Effective_Date` | `GetLookupsByEffectiveDate` | Get lookups by effective date |
| `spu_CLM_Get_Current_Claim_Payments_By_Reserve` | `GetCurrentClaimPaymentReserveDetails` | Get current reserves and payments |
| `spu_CLM_Get_Party_Account_Details` | `GetOtherPartyDetails` | Get other party account details |
| `spu_CLM_Get_Account_Details_by_Short_Code` | `GetAccountDetailsByShortCode` | Get account details by short code |
| `spu_CLM_Get_Payment_Item_Details` | `GetClaimPaymentItemDetails` | Get payment item details |
| `spu_SIR_Get_Tax_Group_Details` | `GetTaxGroupDetails` | Get tax group details |
| `spu_CLM_Calculate_Tax_Amounts` | `CalculateTaxAmounts` | Calculate tax amounts |
| `spu_CLM_Tax_Group_Tax_Band_Select` | `GetTaxGroupTaxBandDetails` | Get tax group tax band links |
| `spu_CLM_Claim_Payment_Add` | `SaveClaimPayment` | Save claim payment |
| `spu_CLM_Claim_Payment_Item_Add` | `SaveClaimPaymentItem` | Save claim payment item |
| `spu_CLM_Tax_Calculation_Add` | `SaveTaxCalculationItem` | Save tax calculation item |
| `spu_CLM_Tax_Calculation_Select` | `GetClaimPaymentItemTax` | Get tax details for payment |
| `spu_CLM_Claim_Payment_Item_Reserve_Update` | `UpdateClaimPaymentItemReserve` | Update reserve based on payment |
| `spu_recovery_saa` | `GetClaimPerilRecoveryDetails` | Get claim peril recovery details |
| `spu_CLM_Claim_Receipt_Add` | `SaveClaimReceipt` | Save claim receipt |
| `spu_CLM_Claim_Receipt_Item_Add` | `SaveClaimReceiptItem` | Save claim receipt item |
| `spu_CLM_Claim_Receipt_Item_Recovery_Update` | `UpdateClaimReceiptItemRecovery` | Update recovery with receipt |
| `spu_CLM_Get_Claim_Tax_Amounts_By_Tax_Type` | internal | Get tax amounts by tax type |
| `spu_get_reserve_details` | `CreateClaimReserves` | Create claim reserves |
| `spu_claims_recovery_coins_select` | `GetCoinsurance` | Get coinsurance details |
| `spu_claims_recovery_reins_select` | `GetReinsurance` | Get reinsurance details |
| `spu_claims_recovery_reins_allocate` | `UpdateClaimReinsurance` | Allocate reinsurance amounts |
| `spu_CLM_Get_MediaTypes` | `GetMediaTypes` | Get media types |
| `spu_CLM_Get_CoInsurer_Split` | `GetCoInsurerDetails` | Get co-insurer split |
| `spu_CLM_Update_CoInsurer_Split` | `UpdateCoInsurerDetails` | Update co-insurer split |
| `spu_CLM_Get_Branch_Currencies` | `GetClaimBranchCurrencies` | Get branch currencies |
| `spu_CLM_Get_Claim_Peril_Class_Of_Business` | `GetClassOfBusiness` | Get class of business for peril |
| `spu_CLM_Get_Risk_Details` | `GetRiskDetails` | Get risk details |
| `spu_GetRiskDetailsForBroking` | internal | Get risk details for broking |
| `spu_SIR_Get_UserCanChange_Reserves` | `GetUserCanChangeReserves` | Check user reserve permissions |
| `spu_CLM_Get_XOL_Count` | `GetClaimXOLineCount` | Get XOL line count |
| `spu_get_all_receipts_for_claim` | `GetReceiptList` | Get all receipts for claim |
| `spu_CLM_Tax_Calculation_Select_For_Receipt` | `GetClaimReceiptItemTaxDetails` | Get tax calculation for receipt |
| `spu_CLM_Get_Claim_Receipt_Details` | `GetClaimReceiptDetails` | Get claim receipt details |
| `spu_CLM_Get_Receipt_Item_Details` | `GetClaimReceiptItemDetails` | Get receipt item details |
| `spu_CLM_Update_transaction_type` | `UpdateClaimTransactionType` | Update claim transaction type |
| `spu_Get_AccountIdFromShortCode` | `IsAccountExists` | Get account ID from short code |
| `spu_CLM_Get_Claim_Payment_Total` | `GetClaimPaymentTotal` | Get claim payment total |
| `spu_Get_Rule_Type_Values` | `ExecuteAdvancedTaxScript` | Get GIS rule type and file value |
| `spu_get_recoveries_by_peril` | `GetClaimRecoveries` | Get recoveries by peril |
| `spu_CLM_save_recovery` | `SaveClaimRecoveries` | Save claim recovery |
| `spu_update_claim_this_payment_Details` | `UpdateThisClaimPaymentDetails` | Update claim payment details |
| `spu_CLM_GetCurrencyRatesToOverride` | `GetCurrencyRatesToOverride` | Get currency rates for override |
| `spu_CLM_OverrideClaimCurrencyRate` | `OverrideClaimCurrencyRate` | Override claim currency rate |
| `spu_Get_User_Reserve_Limit_sel` | `GetUsersReserveLimit` | Get user's reserve limit |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bPMLookup` | `Initialise` | Lookup value retrieval |
| `bSIROptions` | `GetOption` | System options retrieval |
| `bSIREvent` | `CreateEvent` | Event log creation |
| `bSIRPolicyNumMaint` | `GenerateClaimNumber` | Claim number generation |
| `bBackOfficeLink` | `GetSiriusProduct` | Product identification |
| `dCLMPeril` | `Business` (throughout) | Data layer access |

---

### 22. bCLMPerilType
**Directory:** `Peril Type/`
**Project:** `bCLMPerilType`
**Purpose:** Manages claim peril type lookups — retrieving available peril types, checking for duplicate names, and getting claims for a given reserve type.

**Business Methods — Business class (bCLMBusiness.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises the business component |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes resources |
| `New` | `Public Sub New()` | Constructor |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets process mode properties |
| `GetPerilTypes` | `Public Function GetPerilTypes(ByRef r_vResultArray(,) As Object) As Integer` | Gets all undeleted peril types (ID, name, description) |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_get_clm_for_resv_type` | internal | Get claims for a reserve type |
| `spu_get_Peril_types` | `GetPerilTypes` | Get all peril types |
| `spu_chk_Peril_type_name_exists` | internal | Check if peril type name already exists |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| (none) | — | Standalone component with direct database access |

---

### 23. bCLMPerilReserveType
**Directory:** `Peril Type Reserve Type/`
**Project:** `bCLMPerilReserveType`
**Purpose:** Manages the association between peril types and reserve types, allowing configuration of which reserve types apply to which peril types and designating a main reserve.

**Business Methods — Business (bCLMBusiness.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(sUserName, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, Optional bStandAlone, Optional vDatabase) As Long` | Initialises the business component with credentials and database |
| `SetProcessModes` | `Public Function SetProcessModes(Optional vTask, Optional vNavigate, Optional vProcessMode, Optional vTransactionType, Optional vEffectiveDate) As Integer` | Sets optional process mode properties (task, navigate, transaction type) |
| `EditAdd` | `Public Function EditAdd(ByRef lRow, Optional vPerilTypeReserveTypeId, Optional vReserveTypeId, Optional vPerilTypeId, Optional vMainReserve, Optional vMode) As PMEReturnCode` | Adds a peril type/reserve type association into the collection |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow, Optional vPerilTypeReserveTypeId, Optional vReserveTypeId, Optional vPerilTypeId, Optional vMainReserve, Optional vMode) As PMEReturnCode` | Updates an existing peril type/reserve type association in the collection |
| `EditDelete` | `Public Function EditDelete(lRow, vPerilTypeReserveTypeId, vMode) As Integer` | Marks a peril type/reserve type association for deletion |
| `ClearColl` | `Public Sub ClearColl()` | Clears the collection and resets to zero |
| `Cancel` | `Public Function Cancel() As Integer` | Checks collection for pending changes; returns PMDataChanged if dirty |
| `Update` | `Public Function Update() As Integer` | Loops collection performing adds, deletes, and updates within a transaction |

**Business Methods — CLMPerilRsrvType (bCLMPerilReserveType.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(v_sUsername, v_sPassword, v_iUserID, v_iSourceID, v_iLanguageID, v_iCurrencyID, v_iLogLevel, v_sCallingAppName, Optional v_vDatabase) As Integer` | Initialises the single peril reserve type entity and its data component |
| `GetDefaults` | `Public Function GetDefaults(Optional vPerilTypeReserveType, Optional vDescription, ...) As Integer` | Returns default values for all peril type reserve type properties |
| `SetProperties` | `Public Function SetProperties(ByRef iStatus, Optional vPerilTypeReserveTypeId, Optional vReserveTypeId, Optional vPerilTypeId, Optional vMainReserve) As Integer` | Sets property values on the data component and tracks change status |
| `AddItem` | `Public Function AddItem() As Integer` | Adds the record to the database from the data component |
| `DeleteItem` | `Public Function DeleteItem() As Integer` | Deletes a single record from the database |
| `UpdateItem` | `Public Function UpdateItem() As Integer` | Updates a single record in the database from the data component |

**Business Methods — CLMPerilRsrvTypes (bCLMPerilReserveTypes.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Add` | `Public Function Add(ByRef oNewCLMPerilTypeReserveType As CLMPerilRsrvType) As Integer` | Adds a single peril type reserve type into the collection |
| `Count` | `Public Function Count() As Integer` | Returns the number of items in the collection |
| `Delete` | `Public Sub Delete(ByRef vKey As Object)` | Removes a peril type reserve type from the collection by key |
| `Item` | `Public Function Item(ByRef vKey As Object) As CLMPerilRsrvType` | Returns the selected item from the collection |
| `DeleteAll` | `Public Sub DeleteAll()` | Deletes all items from the collection |
| `Clear` | `Public Sub Clear()` | Clears and reinitialises the collection |
| `Initialise` | `Public Function Initialise(v_sUsername, v_sPassword, v_iUserID, v_iSourceID, v_iLanguageID, v_iCurrencyID, v_iLogLevel, v_sCallingAppName, Optional v_vDatabase) As Integer` | Initialises the collection with credentials |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_get_all_reserve_types` | `Business` | Get all reserve types |
| `spu_ChckRsrvTypExstInPrlRsrTyp` | `Business` | Check if reserve type exists for a peril type |
| `spu_GetRsrvTypForPrlTyp` | `Business` | Get reserve types for peril types |
| `spu_ChckDelForPrlRskTyp` | `Business` | Check if reserves exist for a peril type & reserve type before deletion |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `dCLMPerilReserveType` | `CLMPerilRsrvType` | Data access layer for peril reserve type database operations |
| `bPMLookup` | `Business` | Lookup values for peril reserve type forms |

---

### 24. bCLMRecovery
**Directory:** `Recovery/`
**Project:** `bCLMRecovery`
**Purpose:** Manages salvage and recovery processing for claims including recovery reserves, receipts, payments, tax calculations, and coinsurance/reinsurance recovery allocations.

**Business Methods — Business (bCLMBusiness.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, Optional bStandAlone, Optional vDatabase) As Long` | Initialises the business component with credentials and database |
| `SetProcessModes` | `Public Function SetProcessModes(Optional vTask, Optional vNavigate, Optional vProcessMode, Optional vTransactionType, Optional vEffectiveDate) As Integer` | Sets optional process mode properties |
| `AddReceiptAndPayments` | `Public Function AddReceiptAndPayments() As Integer` | Adds receipt and payment details within a transaction |
| `BalanceRecovery` | `Public Function BalanceRecovery(Optional vClaimId, Optional vIsSalvage) As Integer` | Balances recoveries on the specified claim |
| `CheckCancel` | `Public Function CheckCancel() As Integer` | Checks collection for pending changes; returns PMDataChanged if dirty |
| `CloseClaim` | `Public Function CloseClaim(v_lClaimID As Integer) As Integer` | Closes a claim from salvage/recovery |
| `DeleteReceiptAndPayments` | `Public Function DeleteReceiptAndPayments() As Integer` | Deletes receipt and payment details within a transaction |
| `DeleteClaim` | `Public Function DeleteClaim(v_lClaimID As Integer) As Integer` | Deletes claim records |
| `EditAdd` | `Public Function EditAdd(vClaimId, vPerilId, vRecoveryType, vRecoveryTypeID, vLossCurrency, vLossCurrencyID, vInitialReserve, ByRef rUniqueId, Optional v_lRecoveryPartyTypeId, Optional v_lRecoveryPartyCnt, Optional v_sRecoveryParty, Optional v_sRecoveryPartyDesc) As Integer` | Creates a new recovery and adds it to the collection |
| `EditDelete` | `Public Function EditDelete(vUniqueId As String) As Integer` | Marks a recovery for deletion or removes new ones |
| `EditUpdate` | `Public Function EditUpdate(vUniqueId, Optional vRecoveryId, Optional vThisReserve, Optional vThisReceipt, Optional vTaxType, Optional vTaxTypeID, Optional vTaxTypeCode, Optional vTaxBand, Optional vTaxBandID, Optional vTaxAmount, Optional v_lRecoveryPartyTypeId, Optional v_lRecoveryPartyCnt, Optional v_sRecoveryParty, Optional v_sRecoveryPartyDesc) As Integer` | Updates recovery values including reserve, receipt, tax, and party link |
| `GetClientAgentID` | `Public Function GetClientAgentID(v_lClaimID As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Gets client and agent details for a claim |
| `GetDetails` | `Public Function GetDetails(Optional vPerilId, Optional vIsSalvage) As Integer` | Loads all recovery details including coinsurance/reinsurance into the collection |
| `GetCurrentReserveRecovery` | `Public Function GetCurrentReserveRecovery(v_lClaimID As Integer, ByRef r_vDataArray(,) As Object) As Integer` | Gets outstanding reserve and recovery amounts for close claim |
| `GetNext` | `Public Function GetNext(Optional vGetUniqueID, Optional vUniqueId, Optional vRecoveryId, ...) As Integer` | Returns data from the next or a specific recovery record |
| `GetOriginalClaimID` | `Public Function GetOriginalClaimID(v_lClaimID As Integer, ByRef r_lOriginalClaimID As Integer) As Integer` | Gets the original claim ID from work claim |
| `GetPerilDetails` | `Public Function GetPerilDetails(ByRef r_vResultArray(,) As Object, v_lClaim_Id As Integer) As Integer` | Gets peril details for a claim |
| `GetReceiptTotal` | `Public Function GetReceiptTotal(ByRef r_cReceiptTotal As Decimal) As Integer` | Returns the total receipt amount across all recoveries |
| `GetRecoveryTypes` | `Public Function GetRecoveryTypes(v_bIsSalvage As Boolean, ByRef r_vResultArray(,) As Object) As Integer` | Gets available recovery types (salvage or subrogation) |
| `GetTaxTypesTaxBands` | `Public Function GetTaxTypesTaxBands(ByRef r_vResultArray(,) As Object) As Integer` | Loads tax types and bands for recovery tax calculation |
| `PostReceipt` | `Public Function PostReceipt(bIsSalvage, ByRef lInsuranceFileCnt, ByRef lClaimID, ByRef lPerilID, ByRef lReceiptPartyCnt, ByRef sAccountCode, ByRef sMappingCode, ByRef sReceiptComments, ByRef lCOBID, ByRef sCOBCode) As Integer` | Posts a receipt for recovery processing |
| `SetReceiptCurrency` | `Public Function SetReceiptCurrency(vReceiptCurrency As String, vReceiptCurrencyID As Integer, vCurrencyRate As Double) As Integer` | Sets receipt currency and exchange rate on all recoveries |
| `Update` | `Public Function Update() As Integer` | Loops collection performing adds, deletes, updates within transaction |
| `GetAttachedParties` | `Public Function GetAttachedParties(ByRef r_lAgentCnt, ByRef r_sAgentCode, ByRef r_sAgentName, ByRef r_lClientCnt, ByRef r_sClientCode, ByRef r_sClientName, v_lClaim_Id) As Integer` | Gets attached parties (agent/client) on a claim |
| `UpdateRecoveryPartyLink` | `Public Function UpdateRecoveryPartyLink(lRecoveryId, lRecoveryPartyTypeId, lRecoveryPartyCnt) As Integer` | Updates recovery party link association |

**Business Methods — CLMRecovery (bCLMRecoveryCls.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, Optional vDatabase) As Integer` | Initialises the recovery entity with credentials and database |
| `Add` | `Public Function Add() As Integer` | Adds a new recovery record to the database |
| `Delete` | `Public Function Delete() As Integer` | Deletes a recovery record from the database |
| `Update` | `Public Function Update() As Integer` | Updates a recovery record in the database |
| `RecalcCoReinsurance` | `Public Function RecalcCoReinsurance() As Integer` | Recalculates coinsurance and reinsurance split percentages |

**Business Methods — CLMRecoveryTaxes (bCLMRecoveryTaxes.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Add` | `Public Function Add(TaxTypeCode As String, TaxAmount As Decimal) As Integer` | Adds or accumulates a tax amount by tax type code |
| `Count` | `Public Function Count() As Integer` | Returns number of tax types in the collection |
| `Remove` | `Public Sub Remove(vKey As String)` | Removes a tax type from the collection |
| `Item` | `Public Function Item(ByRef vKey As String) As CLMRecoveryTax` | Returns the specified tax type from the collection |
| `Clear` | `Public Sub Clear()` | Clears and reinitialises the tax collection |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_recovery_saa` | `Business.GetDetails` | Select all recoveries for a peril |
| `spu_recovery_add` | `CLMRecovery.Add` | Add a new recovery record |
| `spu_recovery_del` | `CLMRecovery.Delete` | Delete a recovery record |
| `spu_recovery_upd` | `CLMRecovery.Update` | Update a recovery record |
| `spu_recovery_balance` | `Business.BalanceRecovery` | Balance all recoveries on a claim |
| `spu_Recovery_Party_Link_upd` | `Business.UpdateRecoveryPartyLink` | Update recovery party link association |
| `spu_payment_add` | `Business` (private) | Add a payment record |
| `spu_payment_del` | `Business` (private) | Delete a payment record |
| `spu_receipt_add` | `Business` (private) | Add a receipt record |
| `spu_receipt_del` | `Business` (private) | Delete a receipt record |
| `spu_receipt_upd` | `Business` (private) | Update a receipt record |
| `spu_claims_recovery_reins_allocate` | `Business` (private) | Allocate reinsurance recovery values |
| `spu_claims_recovery_coins_saa` | `Business` (private) | Select all coinsurance recovery details |
| `spu_claims_recovery_reins_saa` | `Business` (private) | Select all reinsurance recovery details |
| `spu_CloseClaim` | `Business.CloseClaim` | Close a claim |
| `spu_delete_claim` | `Business.DeleteClaim` | Delete claim records |
| `spu_claim_get_clientagent` | `Business.GetClientAgentID` | Get client/agent for a claim |
| `spu_GetCurrentReserveRecovery` | `Business.GetCurrentReserveRecovery` | Get current outstanding reserve and recovery amounts |
| `spu_CLM_Get_Base_Claim` | `Business.GetOriginalClaimID` | Get original claim ID from work claim |
| `spu_get_Peril_details` | `Business.GetPerilDetails` | Get peril details for a claim |
| `spu_recovery_type_saa` | `Business.GetRecoveryTypes` | Get available recovery types |
| `spu_Get_Tax_Types_and_Bands` | `Business.GetTaxTypesTaxBands` | Load tax types and bands |
| `spu_Get_Attached_Parties_On_Claim` | `Business.GetAttachedParties` | Get parties attached to a claim |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bPMLookup` | `Business` | Lookup values for recovery forms |

---

### 25. bCLMReinsurance
**Directory:** `Reinsurance Recoveries/`
**Project:** `bCLMReinsurance`
**Purpose:** Manages claims reinsurance recovery processing including RI arrangement calculations, band allocation, treaty lookups, and XOL processing for the pre-2007 reinsurance model.

**Business Methods — Form (bCLMReinsuranceForm.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, Optional bStandAlone, Optional vDatabase) As Long` | Initialises the form component with credentials and database |
| `SetProcessModes` | `Public Function SetProcessModes(Optional vTask, Optional vNavigate, Optional vProcessMode, Optional vTransactionType, Optional vEffectiveDate) As Integer` | Sets optional process mode properties |
| `SetStatus` | `Public Function SetStatus(ByRef sProcessStatus, ByRef sMapStatus, ByRef sStepStatus) As Integer` | Sets the process, map, and step status values |
| `ApplyReinsurance` | `Public Function ApplyReinsurance(ByRef r_bApplyReinsurance As Boolean) As Integer` | Returns true; retained for class interface compatibility |
| `CalculateRI` | `Public Function CalculateRI() As Integer` | Creates or refreshes RI arrangement for the claim by calling copy SP |
| `Cancel` | `Public Function Cancel() As Integer` | Checks collection for pending changes; returns PMDataChanged if dirty |
| `DeleteClaim` | `Public Function DeleteClaim() As Integer` | Deletes the work claim |
| `EditUpdate` | `Public Function EditUpdate(lRIBandID As Integer, ByRef vRILines(,) As Object) As Integer` | Updates RI band details and detects changes |
| `GetBandValues` | `Public Function GetBandValues(lRIBandID, ByRef cSumInsured, ByRef cReserveToDate, ByRef cPaymentToDate, ByRef cThisReserve, ByRef cThisPayment, ByRef lRIModelID, ByRef lCatastropheCodeID, ByRef lXolClmModelID, ByRef cXolClmLimit, ByRef lXolCatModelID, ByRef cXolCatLimit, ByRef lXolCatReinstatements, ByRef vRILines) As Integer` | Gets reinsurance details for a specific RI band |
| `GetClaimRiskStatus` | `Public Function GetClaimRiskStatus(v_lClaimId As Integer, ByRef r_bIsDeferred As Boolean) As Integer` | Returns TRUE if the associated risk is 'Reinsurance Deferred' |
| `GetDetails` | `Public Function GetDetails() As Integer` | Gets all reinsurance arrangement details, recalculates RI first |
| `GetInfoOnlyStatus` | `Public Function GetInfoOnlyStatus(ByRef bInfoStatus As Boolean) As Integer` | Checks if claim was previously Info Only |
| `GetOriginalClaimID` | `Public Function GetOriginalClaimID(v_lClaimId As Integer, ByRef r_lOriginalClaimID As Integer) As Integer` | Gets the original claim ID from work table |
| `GetRIBands` | `Public Function GetRIBands(ByRef vBands(,) As Object) As Integer` | Gets reinsurance arrangement band details |
| `GetTreatyInfo` | `Public Function GetTreatyInfo(lTreatyId As Integer, ByRef sCode As String, ByRef sAgreementCode As String, ByRef bIsRetained As Boolean) As Integer` | Retrieves treaty details (code, agreement code, retained flag) |
| `TidyUpAfterCancel` | `Public Function TidyUpAfterCancel(v_lClaimId As Integer) As Integer` | Cleans up after a cancel operation on a claim |
| `Update` | `Public Function Update() As Integer` | Loops collection performing adds/updates within a transaction |
| `ValidateBands` | `Public Function ValidateBands(ByRef r_lValid As Integer, ByRef r_lBand As Integer) As Integer` | Validates all RI bands for consistency |

**Business Methods — Reinsurance (bCLMReinsuranceCls.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Round` | `Public Function Round() As Integer` | Rounds minor allocation discrepancies to allow 100% RI allocation |

**Business Methods — Reinsurances (bCLMReinsurances.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Add` | `Public Function Add(ByRef oNewReinsurance As Reinsurance) As Integer` | Adds a reinsurance item to the collection |
| `Clear` | `Public Sub Clear()` | Clears and reinitialises the collection |
| `Count` | `Public Function Count() As Integer` | Returns number of items in the collection |
| `Item` | `Public Function Item(Index As Object) As Reinsurance` | Returns a reinsurance item by index |
| `Remove` | `Public Sub Remove(Index As Object)` | Removes a reinsurance item from the collection |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_copy_reinsurance_details_to_claim` | `Form.CalculateRI` | Create/refresh RI arrangement for claim |
| `spu_Claim_RI_Arrangement_saa` | `Form.GetDetails` | Select all RI arrangement details for a claim |
| `spu_Claim_RI_Arrangement_sel_bands` | `Form.GetRIBands` | Select RI arrangement bands for a claim |
| `spu_Claim_RI_Arrangement_upd` | `Form.Update` | Update RI arrangement record |
| `spu_Claim_RI_Arrangement_Line_add` | `Form.Update` | Insert RI arrangement line (FAC) |
| `spu_Claim_RI_Arrangement_Line_saa` | `Form` (private) | Select all RI arrangement lines |
| `spu_Claim_RI_Arrangement_Line_upd` | `Form.Update` | Update RI arrangement line |
| `spu_Treaty_sel` | `Form.GetTreatyInfo` | Select treaty details |
| `spu_delete_claim` | `Form.DeleteClaim` | Delete work claim |
| `spu_CLM_risk_status_sel2` | `Form.GetClaimRiskStatus` | Check RI deferred status on claim risk |
| `spu_get_claim_info_only_status` | `Form.GetInfoOnlyStatus` | Check if claim was info-only |
| `spu_CLM_Get_Base_Claim` | `Form.GetOriginalClaimID` | Get original claim ID |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bPMLookup` | `Form` | Lookup values for reinsurance forms |

---

### 26. bCLMReinsuranceRI2007
**Directory:** `Reinsurance Recoveries/`
**Project:** `bCLMReinsuranceRI2007`
**Purpose:** Enhanced version of claims reinsurance recovery for the RI2007 model, adding support for arrangement versioning, broker participants, placement details, grouping, recovered-to-date tracking, and arrangement line deletion.

**Business Methods — Form (bCLMReinsuranceForm.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, Optional bStandAlone, Optional vDatabase) As Long` | Initialises the form component with credentials and database |
| `SetProcessModes` | `Public Function SetProcessModes(Optional vTask, Optional vNavigate, Optional vProcessMode, Optional vTransactionType, Optional vEffectiveDate, Optional bOpenClaimNoTrans) As Integer` | Sets process modes including open-claim-no-transaction flag |
| `SetStatus` | `Public Function SetStatus(ByRef sProcessStatus, ByRef sMapStatus, ByRef sStepStatus) As Integer` | Sets the process, map, and step status values |
| `ApplyReinsurance` | `Public Function ApplyReinsurance(ByRef r_bApplyReinsurance As Boolean) As Integer` | Returns true; retained for class interface compatibility |
| `CalculateRI` | `Public Function CalculateRI() As Integer` | Creates/refreshes RI2007 arrangement including recovery and open-claim flags |
| `Cancel` | `Public Function Cancel() As Integer` | Checks collection for pending changes; returns PMDataChanged if dirty |
| `DeleteClaim` | `Public Function DeleteClaim() As Integer` | Deletes the work claim |
| `EditUpdate` | `Public Function EditUpdate(lRIBandID As Integer, ByRef vRILines(,) As Object) As Integer` | Updates RI band details using RI2007 arrangement line enumerators |
| `GetBandValues` | `Public Function GetBandValues(lRIBandID, ByRef cSumInsured, ByRef cReserveToDate, ByRef cPaymentToDate, ByRef cThisReserve, ByRef cThisPayment, ByRef lRIModelID, ByRef lCatastropheCodeID, ByRef lXolClmModelID, ByRef cXolClmLimit, ByRef lXolCatModelID, ByRef cXolCatLimit, ByRef lXolCatReinstatements, ByRef vRILines, Optional cRecoveredToDate, Optional lXOLRIModelId, Optional dIncurredToDate) As Integer` | Gets RI band values including recovered-to-date and incurred-to-date |
| `GetClaimRiskStatus` | `Public Function GetClaimRiskStatus(v_lClaimId As Integer, ByRef r_bIsDeferred As Boolean) As Integer` | Returns TRUE if the associated risk is 'Reinsurance Deferred' |
| `GetClaimTransType` | `Public Function GetClaimTransType(ByRef vResultArray(,) As Object) As Integer` | Gets claim transaction type |
| `GetDetails` | `Public Function GetDetails() As Integer` | Gets all RI2007 arrangement details, recalculates RI first |
| `GetInfoOnlyStatus` | `Public Function GetInfoOnlyStatus(ByRef bInfoStatus As Boolean) As Integer` | Checks if claim was previously Info Only |
| `GetOriginalClaimID` | `Public Function GetOriginalClaimID(v_lClaimId As Integer, ByRef r_lOriginalClaimID As Integer) As Integer` | Gets the original claim ID from work table |
| `GetRIBands` | `Public Function GetRIBands(ByRef vBands(,) As Object) As Integer` | Gets RI arrangement band details |
| `GetTreatyInfo` | `Public Function GetTreatyInfo(lTreatyId As Integer, ByRef sCode As String, ByRef sAgreementCode As String, ByRef bIsRetained As Boolean) As Integer` | Retrieves treaty details |
| `TidyUpAfterCancel` | `Public Function TidyUpAfterCancel(v_lClaimId As Integer) As Integer` | Cleans up after a cancel operation |
| `Update` | `Public Function Update() As Integer` | Loops collection performing adds/updates with broker participants and versioning |
| `ValidateBands` | `Public Function ValidateBands(ByRef r_lValid As Integer, ByRef r_lBand As Integer) As Integer` | Validates all RI bands for consistency |

**Business Methods — Reinsurance (bCLMReinsuranceCls.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Round` | `Public Function Round() As Integer` | Rounds minor allocation discrepancies to allow 100% RI allocation |

**Business Methods — Reinsurances (bCLMReinsurances.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Add` | `Public Function Add(ByRef oNewReinsurance As Reinsurance) As Integer` | Adds a reinsurance item to the collection |
| `Clear` | `Public Sub Clear()` | Clears and reinitialises the collection |
| `Count` | `Public Function Count() As Integer` | Returns number of items in the collection |
| `Item` | `Public Function Item(Index As Object) As Reinsurance` | Returns a reinsurance item by index |
| `Remove` | `Public Sub Remove(Index As Object)` | Removes a reinsurance item from the collection |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_copy_reinsurance_details_to_claim_RI2007` | `Form.CalculateRI` | Create/refresh RI2007 arrangement for claim |
| `spu_Claim_RI_Arrangement_saa` | `Form.GetDetails` | Select all RI arrangement details |
| `spu_Claim_RI_Arrangement_sel_bands` | `Form.GetRIBands` | Select RI arrangement bands |
| `spu_Claim_RI_Arrangement_upd` | `Form.Update` | Update RI arrangement record |
| `spu_Claim_RI_Arrangement_Line_add` | `Form.Update` | Insert RI arrangement line |
| `spu_Claim_RI_Arrangement_Line_saa_RI2007` | `Form` (private) | Select all RI2007 arrangement lines |
| `spu_Claim_RI_Arrangement_Line_upd` | `Form.Update` | Update RI arrangement line |
| `spu_Claim_RI_Arrangement_Line_Del_RI2007` | `Form.Update` | Delete RI2007 arrangement lines |
| `spu_Claim_RI_Arrangement_Version_upd` | `Form.Update` | Update RI arrangement version |
| `Spu_Sir_AddBrokerParticipants` | `Form.Update` | Add broker participants to arrangement |
| `spu_Treaty_sel` | `Form.GetTreatyInfo` | Select treaty details |
| `spu_delete_claim` | `Form.DeleteClaim` | Delete work claim |
| `spu_CLM_risk_status_sel2` | `Form.GetClaimRiskStatus` | Check RI deferred status |
| `spu_SAM_CLM_Get_Claim_Transaction_Type` | `Form.GetClaimTransType` | Get claim transaction type |
| `spu_get_claim_info_only_status` | `Form.GetInfoOnlyStatus` | Check if claim was info-only |
| `spu_CLM_Get_Base_Claim` | `Form.GetOriginalClaimID` | Get original claim ID |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bCLMReinsurance` | `bCLMReinsuranceRI2007` | Shares Reinsurance/Reinsurances classes and enumerators |

---

### 27. bCLMResvDefn
**Directory:** `Reserve Definition/`
**Project:** `bCLMResvDefn`
**Purpose:** Manages reserve type definitions for claims, allowing configuration of reserve types with description, name, include-in-total flag, excess flag, and indemnity/expense classification.

**Business Methods — Business (bCLMBusiness.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, Optional bStandAlone, Optional vDatabase) As Long` | Initialises the business component with credentials and database |
| `SetProcessModes` | `Public Function SetProcessModes(Optional vTask, Optional vNavigate, Optional vProcessMode, Optional vTransactionType, Optional vEffectiveDate) As Integer` | Sets optional process mode properties |
| `EditAdd` | `Public Function EditAdd(ByRef lRow, Optional vReserveTypeID, Optional vDescription, Optional vIncludeInTotal, Optional vName, Optional vIsExcess, Optional vIs_Indemnity, Optional vIs_Expense) As PMEReturnCode` | Adds a reserve type definition into the collection |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow, Optional vReserveTypeID, Optional vDescription, Optional vIncludeInTotal, Optional vName, Optional vIsExcess, Optional vIs_Indemnity, Optional vIs_Expense) As PMEReturnCode` | Updates an existing reserve type definition in the collection |
| `EditDelete` | `Public Function EditDelete(lRow As Integer, Optional vReserveTypeID As Integer = 0) As Integer` | Marks a reserve type definition for deletion |
| `ClearColl` | `Public Sub ClearColl()` | Clears the collection and resets to zero |
| `Cancel` | `Public Function Cancel() As Integer` | Checks collection for pending changes; returns PMDataChanged if dirty |
| `Update` | `Public Function Update() As Integer` | Loops collection performing adds, deletes, and updates within a transaction |
| `GetClmForResvType` | `Public Function GetClmForResvType(ByRef r_lRecordCount As Integer, ByRef r_vResultArray(,) As Object, v_lReserveTypeID As Integer) As Integer` | Checks if the reserve type is used by any claim |
| `GetReserveTypes` | `Public Function GetReserveTypes(ByRef r_vResultArray(,) As Object) As Integer` | Gets all reserve type definitions |
| `ChkResvTypeNameExists` | `Public Function ChkResvTypeNameExists(ByRef r_lRecordCount As Integer, v_sName As String) As Integer` | Checks if a reserve type name already exists |

**Business Methods — CLMResvDefn (bCLMResvDefnCls.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, Optional vDatabase) As Integer` | Initialises the reserve definition entity and its data component |
| `GetDefaults` | `Public Function GetDefaults(Optional vReserveTypeID, Optional vDescription, Optional vIncludeInTotal, ..., Optional IsExcess) As Integer` | Returns default values for reserve type properties |
| `SetProperties` | `Public Function SetProperties(ByRef iStatus, Optional vReserveTypeID, Optional vDescription, Optional vIncludeInTotal, Optional vName, Optional vIsExcess, Optional vIs_Indemnity, Optional vIs_Expense) As Integer` | Sets property values on the data component and tracks change status |
| `GetProperties` | `Public Function GetProperties(ByRef iStatus, Optional vReserveTypeID, Optional vDescription, Optional vIncludeInTotal, Optional vCurrencyID, Optional vName, ..., Optional vIsExcess) As Integer` | Returns the current property values from the data component |
| `AddItem` | `Public Function AddItem() As Integer` | Adds the record to the database *(inherited from data component)* |
| `DeleteItem` | `Public Function DeleteItem() As Integer` | Deletes a single record from the database |
| `UpdateItem` | `Public Function UpdateItem() As Integer` | Updates a single record in the database |

**Business Methods — CLMResvDefns (bCLMResvDefns.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Add` | `Public Function Add(ByRef oNewCLMResvDefn As CLMResvDefn) As Integer` | Adds a single reserve definition into the collection |
| `Count` | `Public Function Count() As Integer` | Returns the number of items in the collection |
| `Delete` | `Public Sub Delete(ByRef vKey As Integer)` | Removes a reserve definition from the collection by key |
| `Item` | `Public Function Item(ByRef vKey As Integer) As CLMResvDefn` | Returns the selected item from the collection |
| `DeleteAll` | `Public Sub DeleteAll()` | Deletes all items from the collection |
| `Clear` | `Public Sub Clear()` | Clears and reinitialises the collection |
| `Initialise` | `Public Function Initialise(ByRef sUserName, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, Optional vDatabase) As Integer` | Initialises the collection with credentials |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_get_clm_for_resv_type` | `Business.GetClmForResvType` | Check if reserve type is used by any claim |
| `spu_get_resv_types` | `Business.GetReserveTypes` | Get all reserve type definitions |
| `spu_chk_resv_type_name_exists` | `Business.ChkResvTypeNameExists` | Check if reserve type name already exists |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `dCLMResvDefn` | `CLMResvDefn` | Data access layer for reserve type database operations |
| `bPMLookup` | `Business` | Lookup values for reserve definition forms |

---

### 28. bCLMRiskDetails
**Directory:** `Risk Details/`
**Project:** `bCLMRiskDetails`
**Purpose:** Manages claim risk details including claim creation, peril management, risk data definitions, party associations, GIS integration, claim comments, close/reopen/re-close operations, and claim builder workflow.

**Business Methods — Business (bCLMRiskDetailsBusiness.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, Optional bStandAlone, Optional vDatabase) As Long` | Initialises the business component with credentials and database |
| `SetProcessModes` | `Public Function SetProcessModes(Optional vTask, Optional vNavigate, Optional vProcessMode, Optional vTransactionType, Optional vEffectiveDate) As Integer` | Sets optional process mode properties |
| `GetLookupValues` | `Public Function GetLookupValues(iLookupType As Integer, ByRef vTableArray(,), iLanguageID As Integer, ByRef vResultArray(,)) As Integer` | Gets lookup values for claim risk details forms |
| `DirectAdd` | `Public Function DirectAdd(Optional vProgressStatusID, Optional vClaimStatusID, Optional vClaimDescription, Optional vPrimaryCauseID, Optional vSecondaryCauseID, Optional vPerilTypeID, Optional vPerilDescription, Optional vClaimNumber, Optional vSumInsured, Optional vCurrentReserve, Optional vComments) As Integer` | Adds a claim risk detail directly to the database (not collection) |
| `DirectDelete` | `Public Function DirectDelete(Optional vClaimID, Optional vRiskTypeID, Optional vRiskDataDefnID) As Integer` | Deletes a claim risk detail directly from the database |
| `CheckID` | `Public Function CheckID(ByRef vID As Object) As Integer` | Checks if a claim ID exists |
| `GetDetails` | `Public Function GetDetails(Optional vLockMode, Optional vClaimID, Optional vRiskTypeID, Optional vRiskDataDefnID) As Integer` | Gets risk details and populates the collection |
| `GetNext` | `Public Function GetNext(Optional vProgressStatusID, Optional vClaimStatusID, Optional vClaimDescription, ...) As Integer` | Returns data from the next record in the collection |
| `EditAdd` | `Public Function EditAdd(ByRef lRow, Optional vProgressStatusID, Optional vClaimStatusID, Optional vClaimDescription, ...) As Integer` | Adds a claim risk detail into the collection |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow, Optional vProgressStatusID, Optional vClaimStatusID, Optional vClaimDescription, ...) As Integer` | Updates an existing claim risk detail in the collection |
| `EditDelete` | `Public Function EditDelete(lRow As Integer) As Integer` | Marks a claim risk detail for deletion |
| `Cancel` | `Public Function Cancel() As Integer` | Checks collection for pending changes |
| `Update` | `Public Function Update() As Integer` | Loops collection performing adds, deletes, and updates |
| `DeleteClaim` | `Public Function DeleteClaim() As Integer` | Deletes work claim records |
| `GetFieldsForRiskDataDefn` | `Public Function GetFieldsForRiskDataDefn(v_lRiskTypeId, v_lClaimId, ByRef r_vResultArray, Optional ByRef v_iMandatory) As Integer` | Gets fields and values for a risk data definition (2 overloads) |
| `GetDataForRiskDataDefn` | `Public Function GetDataForRiskDataDefn(v_lRiskDataDefn, v_lClaimId, ByRef r_vResultArray(,)) As Integer` | Gets data values for a risk data definition |
| `GetPartyTypesforRiskType` | `Public Function GetPartyTypesforRiskType(v_lRiskTypeId As Integer, ByRef r_vResultArray(,)) As Integer` | Gets all party types for a risk type |
| `GetPartyDetailsForClaim` | `Public Function GetPartyDetailsForClaim(v_lClaimId, v_lPartyTypeId, ByRef r_vResultArray(,)) As Integer` | Gets party details for a claim and party type |
| `GetCommentsForClaim` | `Public Function GetCommentsForClaim(v_lClaimId, v_lRiskTypeId, ByRef v_vDescription, ByRef r_vResultArray(,)) As Integer` | Gets comments for a claim risk |
| `GetBasicClaimDetails` | `Public Function GetBasicClaimDetails(v_lClaimId As Integer, ByRef r_vResultArray(,)) As Integer` | Gets basic claim details |
| `CheckForExistenceinClaimRisk` | `Public Function CheckForExistenceinClaimRisk(v_lClaimId, v_lRiskTypeId, ByRef r_bExists) As Integer` | Checks if a risk type exists on a claim |
| `AddClaimRisk` | `Public Function AddClaimRisk(v_lClaimId, v_lRiskTypeId, v_sDescription, v_sComments) As Integer` | Adds a claim risk record |
| `AddClaimPartyClaim` | `Public Function AddClaimPartyClaim(v_lClaimId As Integer, v_lPartyId As Integer) As Integer` | Adds a claim party claim link |
| `DeleteClaimPartyClaim` | `Public Function DeleteClaimPartyClaim(v_lClaimId As Integer, v_lPartyId As Integer) As Integer` | Deletes a claim party claim link |
| `GetLookupTables` | `Public Function GetLookupTables(v_sLookupIDs As String, ByRef r_vResultArrray As Object) As Integer` | Gets lookup tables by IDs |
| `AddPeril` | `Public Function AddPeril(ClaimId As Integer, PerilTypeId As Integer, ByRef PerilID As Integer) As Integer` | Adds a peril to a claim |
| `AddGeneralDetail` | `Public Function AddGeneralDetail(v_lClaimId, v_lRiskDataDefn, v_sValue As String) As Integer` | Adds a general detail value for a claim |
| `GetSiriusProduct` | `Public Function GetSiriusProduct(ByRef r_sSiriusProduct As String) As Integer` | Gets the Sirius product type |
| `GetPerilTypeForRisk` | `Public Function GetPerilTypeForRisk(v_lClaimId, v_lRisk, v_lPolicy, ByRef r_vResultArray(,), Optional bClaimsBuilder) As Integer` | Gets peril types for a risk (2 overloads) |
| `AddClaimPeril` | `Public Function AddClaimPeril(v_lClaimId, v_lPerilTypeID, ByRef r_lClaimPerilId, v_lRiskId, v_sDescription) As Integer` | Adds a claim peril record |
| `GetRiskDetails` | `Public Function GetRiskDetails(v_lRisk, v_lPolicyId, ByRef r_vDataArray(,)) As Integer` | Gets risk details for a policy risk |
| `GetPerilForClaimRisk` | `Public Function GetPerilForClaimRisk(v_lClaimId, v_lRiskTypeId, ByRef r_vDataArray(,)) As Integer` | Gets perils for a claim risk |
| `CheckDeletionForPeril` | `Public Function CheckDeletionForPeril(v_lClaimPerilId, ByRef r_bCanDelete) As Integer` | Checks if a peril can be safely deleted |
| `DeletePeril` | `Public Function DeletePeril(v_lClaimPerilId As Integer) As Integer` | Deletes a peril from a claim |
| `AddPerilForClaimRisk` | `Public Function AddPerilForClaimRisk(v_lPolicyId, v_lRiskId, v_lClaimId) As Integer` | Adds peril for claim risk from policy |
| `GetShowRiskDetails` | `Public Function GetShowRiskDetails(v_lClaimId As Integer, ByRef r_vDataArray(,)) As Integer` | Gets display risk details for a claim |
| `GetPolicynumber` | `Public Function GetPolicynumber(v_lEventCnt As Integer, ByRef r_vDataArray(,)) As Integer` | Gets policy number for an event |
| `GetCurrentReserveRecovery` | `Public Function GetCurrentReserveRecovery(v_lClaimId As Integer, ByRef r_vDataArray(,)) As Integer` | Gets current outstanding reserve and recovery amounts |
| `CloseClaim` | `Public Function CloseClaim(v_lClaimId As Integer) As Integer` | Closes a claim |
| `GetRiskDetails_U` | `Public Function GetRiskDetails_U(v_lClaimId As Integer, ByRef r_vResultArray(,)) As Integer` | Gets risk details for underwriting view |
| `GetOriginalClaimID` | `Public Function GetOriginalClaimID(v_lClaimId As Integer, ByRef r_lOriginalClaimID As Integer) As Integer` | Gets the original claim ID from work table |
| `GetInfoOnlyStatus` | `Public Function GetInfoOnlyStatus(v_lClaim_Id As Integer, ByRef r_bInfoStatus As Boolean) As Integer` | Checks if claim was previously Info Only |
| `GetClientPolicyDetails` | `Public Function GetClientPolicyDetails(v_lInsuranceFileCnt, ByRef r_lPartyCnt, Optional ByRef r_sPartyShortName, Optional ByRef r_lInsuranceFolderCnt, Optional ByRef r_sInsuranceRef) As Integer` | Gets client/policy details (2 overloads) |
| `GetGISScreenID` | `Public Function GetGISScreenID(lClaimID, ByRef r_lScreenID, bPerilLevel) As Integer` | Gets GIS screen ID for a claim |
| `SaveGISScreenID` | `Public Function SaveGISScreenID(lClaimID, lScreenID, bPerilLevel) As Integer` | Saves GIS screen ID for a claim |
| `GetPolicyType` | `Public Function GetPolicyType(v_lPolicyId As Integer, ByRef r_sType As String) As Integer` | Gets policy type |
| `GetDrivers` | `Public Function GetDrivers(v_lInsurance_File_Cnt As Integer, ByRef r_vDrivers(,)) As Integer` | Gets drivers from policy |
| `ReCloseClaim` | `Public Function ReCloseClaim(v_lClaimId As Integer) As Integer` | Re-closes a previously reopened claim |
| `ReOpenClaim` | `Public Function ReOpenClaim(v_lClaimId As Integer) As Integer` | Reopens a closed claim |
| `GetRiskTypeScreenID` | `Public Function GetRiskTypeScreenID(v_lRiskTypeId As Integer, ByRef r_lScreenID As Integer) As Integer` | Gets screen ID for a risk type |
| `GetProgressStatusCode` | `Public Function GetProgressStatusCode(iProgressStatus As Integer, ByRef sCode As String) As PMEReturnCode` | Gets progress status code by ID |
| `GetRiskType` | `Public Function GetRiskType(ByRef r_vResults(,), v_lRiskId As Integer) As Integer` | Gets risk type for a risk ID |
| `AddInputParameter` | `Public Function AddInputParameter(v_sName As String, v_vValue As Object, v_iType As Integer) As Integer` | Adds a database input parameter |
| `DeleteGISDetails` | `Public Function DeleteGISDetails(v_lClaimId As Integer) As Integer` | Deletes GIS dataset and policy link for a work claim |
| `TidyUpAfterCancel` | `Public Function TidyUpAfterCancel(v_lClaimId As Integer, Optional v_lClaimMode As Integer) As Integer` | Cleans up after cancel (2 overloads) |
| `GetGisPolicyLinkDetails` | `Public Function GetGisPolicyLinkDetails(v_lClaimId As Integer, ByRef r_vResults(,)) As Integer` | Gets GIS policy link details for claim |

**Business Methods — Automated (bCLMRiskDetailsAutomated.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUserName, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, Optional vDatabase) As Integer` | Initialises the automated class |
| `GetSummary` | `Public Function GetSummary(ByRef vSummaryArray As Object) As Integer` | Returns summary array (stub) |
| `SetProcessModes` | `Public Function SetProcessModes(Optional vTask, Optional vNavigate, Optional vProcessMode, Optional vTransactionType, Optional vEffectiveDate) As Integer` | Sets process mode properties |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Stores parameter members from key array |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Returns key array with parameter members |
| `Start` | `Public Function Start() As Integer` | Performs automated action based on task/process mode |

**Navigator Methods — NavigatorV3 (bNavigatorV3.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Navigator initialization |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes navigator resources |
| `NavigatorV3_SetKeys` | `Public Function NavigatorV3_SetKeys(ByRef vKeyArray(,) As Object) As Integer Implements aPMNav.NavigatorV3.SetKeys` | Sets navigation keys (claim_id, risk_type_id) |
| `NavigatorV3_GetKeys` | `Public Function NavigatorV3_GetKeys(ByRef vKeyArray(,) As Object) As Integer Implements aPMNav.NavigatorV3.GetKeys` | Gets current navigation keys |
| `NavigatorV3_GetSummary` | `Public Function NavigatorV3_GetSummary(ByRef vSummaryArray As Object) As Integer Implements aPMNav.NavigatorV3.GetSummary` | Gets summary data for navigator display |
| `NavigatorV3_SetProcessModes` | `Public Function NavigatorV3_SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer Implements aPMNav.NavigatorV3.SetProcessModes` | Sets navigator process modes |
| `NavigatorV3_Start` | `Public Function NavigatorV3_Start() As Integer Implements aPMNav.NavigatorV3.Start` | Starts navigator processing |
| `New` | `Public Sub New()` | Constructor |

**Entity Methods — RiskDetails (bCLMRiskDetailsCls.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Entity initialization with data component |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes entity and data resources |
| `GetDefaults` | `Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vProgressStatusID As Object = Nothing, Optional ByRef vClaimStatusID As Object = Nothing, Optional ByRef vClaimDescription As Object = Nothing, Optional ByRef vPrimaryCauseID As Object = Nothing, Optional ByRef vSecondaryCauseID As Object = Nothing, ...) As Integer` | Gets default property values for a new risk detail |
| `SetProperties` | `Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vProgressStatusID As Object = Nothing, Optional ByRef vClaimStatusID As Object = Nothing, Optional ByRef vClaimDescription As Object = Nothing, ...) As Integer` | Sets entity properties with validation |
| `GetProperties` | `Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vProgressStatusID As Object = Nothing, Optional ByRef vClaimStatusID As Object = Nothing, Optional ByRef vClaimDescription As Object = Nothing, ...) As Integer` | Gets entity properties |
| `SelectItem` | `Public Function SelectItem(ByRef iIndex As Integer) As Integer` | Selects a risk detail record by index from the data component |
| `AddItem` | `Public Function AddItem() As Integer` | Adds a new risk detail record to the database |
| `DeleteItem` | `Public Function DeleteItem() As Integer` | Deletes a risk detail record from the database |
| `UpdateItem` | `Public Function UpdateItem() As Integer` | Updates a risk detail record in the database |
| `New` | `Public Sub New()` | Constructor |

**Collection Methods — RiskDetailss (bCLMRiskDetailss.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Add` | `Public Function Add(ByRef oNewCLMRiskDetails As bCLMRiskDetails.RiskDetails) As Integer` | Adds a risk detail entity to the collection |
| `Count` | `Public Function Count() As Integer` | Returns the number of items in the collection |
| `Delete` | `Public Sub Delete(ByRef vKey As Integer)` | Removes an item from the collection by key |
| `Item` | `Public Function Item(ByRef vKey As Integer) As bCLMRiskDetails.RiskDetails` | Gets a risk detail entity by key |
| `DeleteAll` | `Public Sub DeleteAll()` | Deletes all items from the collection |
| `Clear` | `Public Sub Clear()` | Clears the collection |
| `Initialise` | `Public Function Initialise() As Integer` | Initialises the collection |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes collection resources |
| `New` | `Public Sub New()` | Constructor |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_get_basic_claim_details` | `Business.GetBasicClaimDetails` | Get basic claim details |
| `spu_Get_ClaimRisk_Comments` | `Business.GetCommentsForClaim` | Get comments for claim risk |
| `spu_Claim_Comments_Sel` | `Business.GetCommentsForClaim` (BR) | Get claim comments (broker version) |
| `spu_Get_ClaimRisk_Desc` | `Business` | Get description for claim risk |
| `spu_getfields_for_rskdatadefn` | `Business.GetFieldsForRiskDataDefn` | Get fields for risk data definition |
| `spu_getdata_for_rskdatadefn` | `Business.GetDataForRiskDataDefn` | Get data values for risk data definition |
| `spu_get_partytypes_for_risk` | `Business.GetPartyTypesforRiskType` | Get party types for a risk type |
| `spu_partydet_for_party_type` | `Business.GetPartyDetailsForClaim` | Get party details for a party type |
| `spu_check_existence_in_clmrsk` | `Business.CheckForExistenceinClaimRisk` | Check existence in claim risk |
| `spu_add_claim_risk` | `Business.AddClaimRisk` | Add to claim_risk table |
| `spu_add_claim_party_claim` | `Business.AddClaimPartyClaim` | Add claim party claim link |
| `spu_delete_claim_party_claim` | `Business.DeleteClaimPartyClaim` | Delete claim party claim link |
| `spu_Get_Peril_type_For_Risk` | `Business.GetPerilTypeForRisk` | Get peril types for a risk |
| `spu_Add_Claim_Peril` | `Business.AddClaimPeril` | Add a claim peril record |
| `spu_Get_Lookup_Tables` | `Business.GetLookupTables` | Get lookup tables |
| `spu_Add_General_Details` | `Business.AddGeneralDetail` | Add general detail value |
| `spu_GetRiskDetails` | `Business.GetRiskDetails` | Get risk details for policy risk |
| `spu_Get_Peril_For_claim_Risk` | `Business.GetPerilForClaimRisk` | Get perils for a claim risk |
| `spu_check_deletion_for_peril` | `Business.CheckDeletionForPeril` | Check if peril can be deleted |
| `spu_deleteperil` | `Business.DeletePeril` | Delete a peril |
| `spu_add_peril_claim_risk` | `Business.AddPerilForClaimRisk` | Add peril for claim risk from policy |
| `spu_GetShowRiskDetails` | `Business.GetShowRiskDetails` | Get display risk details |
| `spu_GetPolicynumber` | `Business.GetPolicynumber` | Get policy number |
| `spu_CloseClaim` | `Business.CloseClaim` | Close a claim |
| `spu_GetCurrentReserveRecovery` | `Business.GetCurrentReserveRecovery` | Get current reserve/recovery amounts |
| `spu_CLM_Get_Claim_Link_Details` | `Business.GetRiskDetails_U` | Get claim link details for underwriting |
| `spu_delete_claim` | `Business.DeleteClaim` | Delete work claim |
| `spu_CLM_Get_Base_Claim` | `Business.GetOriginalClaimID` | Get original claim ID |
| `spu_get_claim_info_only_status` | `Business.GetInfoOnlyStatus` | Check if claim was info-only |
| `spu_Get_Policy_Type` | `Business.GetPolicyType` | Get policy type |
| `spu_SIRRen_Get_Drivers` | `Business.GetDrivers` | Get drivers from policy |
| `spu_ReCloseClaim` | `Business.ReCloseClaim` | Re-close a reopened claim |
| `spu_ReOpenClaim` | `Business.ReOpenClaim` | Reopen a closed claim |
| `spu_get_risk_type_screenID` | `Business.GetRiskTypeScreenID` | Get screen ID for risk type |
| `spu_CLM_Get_Progress_Status_Code` | `Business.GetProgressStatusCode` | Get progress status code |
| `spu_CLM_Get_Risk_Type` | `Business.GetRiskType` | Get risk type for a risk ID |
| `spu_CLM_Get_Gis_Data` | `Business` | Get GIS data for claim |
| `spu_claim_get_screen_id` | `Business.GetGISScreenID` | Get GIS screen ID |
| `spu_claim_upd_screen_id` | `Business.SaveGISScreenID` | Save GIS screen ID |
| `spu_CLM_Delete_GIS_DataSet` | `Business.DeleteGISDetails` | Delete GIS dataset |
| `spu_CLM_Get_GIS_Policy_Link_Details` | `Business.GetGisPolicyLinkDetails` | Get GIS policy link details |
| `spu_reserve_balance` | `Business.Update` | Balance reserve amounts |
| `spu_CLM_Get_Claim_Peril_id` | `Business` | Get claim peril ID |
| `spu_CLM_Get_Claim_Peril_Class_Of_Business` | `Business` | Get class of business for peril |
| `spu_get_client_policy_details` | `Business.GetClientPolicyDetails` | Get client/policy details |
| `spu_claim_comments_sel` | `Business` | Select claim comments |
| `spu_claim_comments_del` | `Business` | Delete claim comments |
| `spu_claim_comments_add` | `Business` | Add claim comments |
| `spu_PMWrk_Task_Instance_upd_For_Reserve_Limit` | `Business` | Update work manager for reserve limit |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `dCLMRiskDetails` | `RiskDetails` | Data access layer for risk details database operations |
| `bPMLookup` | `Business` | Lookup values for risk details forms |
| `bCLMAuthorisePayments` | `Business` | Authorise payments integration |

---

### 29. bCLMRiskType
**Directory:** `Risk Type/`
**Project:** `bCLMRiskType`
**Purpose:** Manages risk type lookups for claims — retrieves underwriting and broking risk types, GIS screen mappings, and updates risk type claim screen assignments.

**Business Methods — Business (bCLMBusiness.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises database and components |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes database resources |
| `New` | `Public Sub New()` | Constructor |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process modes |
| `GetRiskTypesUnderWriting` | `Public Function GetRiskTypesUnderWriting(ByRef r_vResultArray(,) As Object) As Integer` | Gets all risk types for underwriting |
| `GetRiskTypesBroking` | `Public Function GetRiskTypesBroking(ByRef r_vResultArray(,) As Object) As Integer` | Gets all risk types for broking |
| `GetGISScreensList` | `Public Function GetGISScreensList(ByRef r_vResultArray(,) As Object) As Integer` | Gets all claim GIS screens |
| `UpdateRiskType` | `Public Function UpdateRiskType(ByVal v_lRiskTypeID As Integer, ByVal v_lScreenID As Integer) As Integer` | Updates claims_gis_screen_id for a risk type |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_get_risk_types_UW` | `GetRiskTypesUnderWriting` | Select all underwriting risk types |
| `spu_get_risk_types_BRK` | `GetRiskTypesBroking` | Select all broking risk types |
| `spu_get_GIS_screens` | `GetGISScreensList` | Select all GIS screens |
| `spu_Risk_Type_ClaimScreen_upd` | `UpdateRiskType` | Update risk type selected screen |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| *None* | — | Standalone component with no b* dependencies |

---

### 30. bCLMRiskTypeInfoChecklist
**Directory:** `Risk Type Information Checklist/`
**Project:** `bCLMRiskTypeInfoChecklist`
**Purpose:** Manages the association between risk types and expert services (information checklist items). Supports adding, deleting, updating, and batch-saving risk type expert service assignments.

**Business Methods — Business (bCLMBusiness.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise` | Initialises database, lookup, and back office link |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes resources |
| `New` | `Public Sub New()` | Constructor |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process modes |
| `ClearColl` | `Public Sub ClearColl()` | Clears the collection |
| `CollCount` | `Public Function CollCount() As Integer` | Gets count of items in collection |
| `SelectRiskType` | `Public Function SelectRiskType(ByRef rvResultArray(,) As Object) As Integer` | Selects all risk type records |
| `SelRiskTypeExpSer` | `Public Function SelRiskTypeExpSer(ByRef rvResultArray(,) As Object, ByVal v_lrsk_type_id As Integer) As Integer` | Selects expert services for a risk type |
| `SelExpSer` | `Public Function SelExpSer(ByRef rvResultArray(,) As Object, ByVal v_lrsk_type_id As Integer) As Integer` | Selects all expert services excluding those already in risk type |
| `EditAdd` | `Public Function EditAdd(ByVal lRow As Integer, Optional ByRef bLoad As Boolean = False, Optional ByRef bCboChange As Boolean = False, Optional ByRef vRisk_type_Exp_ser_id As Object = Nothing, Optional ByRef vExp_ser_id As Object = Nothing, Optional ByRef vRisk_type_id As Object = Nothing) As Integer` | Adds a checklist item to collection |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vExp_ser_id As Object = Nothing, Optional ByRef vRisk_type_id As Object = Nothing, Optional ByRef vRecoveryTypeID As Object = Nothing, Optional ByRef vIntialReserve As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vPerilID As Object = Nothing) As gPMConstants.PMEReturnCode` | Updates a checklist item (stub/not implemented) |
| `EditDelete` | `Public Function EditDelete(ByVal lRow As Integer) As Integer` | Marks a checklist item for deletion |
| `Cancel` | `Public Function Cancel() As Integer` | Checks collection for unsaved changes |

**Entity Methods — CLMRTInfoChklst (bCLMRiskTypeInfoChecklistCls.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises entity with data component |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes data component |
| `New` | `Public Sub New()` | Constructor |
| `SetProperties` | `Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vRisk_type_Exp_ser_id As Object = Nothing, Optional ByRef vExp_ser_id As Object = Nothing, Optional ByRef vRisk_type_id As Object = Nothing) As Integer` | Sets entity properties |
| `AddItem` | `Public Function AddItem() As Integer` | Adds record to database |
| `DeleteItem` | `Public Function DeleteItem() As Integer` | Deletes record from database |

**Collection Methods — CLMRTInfoChklsts (bCLMRiskTypeInfoChecklists.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Add` | `Public Function Add(ByRef oNewCLMRiskTypeInfoChecklist As bCLMRTInfoChkLst.CLMRTInfoChklst) As Integer` | Adds entity to collection |
| `Count` | `Public Function Count() As Integer` | Returns collection count |
| `Delete` | `Public Sub Delete(ByRef vKey As Integer)` | Removes from collection by key |
| `Item` | `Public Function Item(ByRef vKey As Integer) As bCLMRTInfoChkLst.CLMRTInfoChklst` | Gets entity by key |
| `DeleteAll` | `Public Sub DeleteAll()` | Removes all items from collection |
| `Clear` | `Public Sub Clear()` | Resets collection |
| `Initialise` | `Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises collection |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes collection |
| `New` | `Public Sub New()` | Constructor |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_Rsk_Type_sel` | `SelectRiskType` | Select all risk types |
| `spu_Rsk_code_sel` | *(defined but not directly called in Business)* | Select all risk codes |
| `spu_Get_RskType_ExpSer` | `SelRiskTypeExpSer` | Select expert services for a risk type |
| `spu_Get_ExpSer` | `SelExpSer` | Select expert services excluding existing |
| `Spu_Info_Checklist_upd` | *(defined for update of show information checklist)* | Update show information checklist flag |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bBackOfficeLink` | `Initialise` | Back office link for system options |
| `bPMLookup` | `Initialise` | PM lookup business component |

---

### 31. bCLMSalvageRecovery
**Directory:** `Salvage Recovery/`
**Project:** `bCLMSalvageRecovery`
**Purpose:** Manages salvage recovery records for claims — handles CRUD operations for recovery details including receipts, payments, coinsurance/reinsurance breakdowns, tax, and currency.

**Business Methods — Business (bCLMBusiness.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises database and lookup |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes resources |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets process modes |
| `DirectAdd` | `Public Function DirectAdd(Optional ByRef vRecoveryId As Object = Nothing, Optional ByRef vRecoveryTypeID As Object = Nothing, Optional ByRef vPerilId As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vinitialReserve As Object = Nothing, Optional ByRef vRevisedReserve As Object = Nothing, Optional ByRef vReceivedToDate As Object = Nothing, Optional ByRef vRevisionCount As Object = Nothing, Optional ByRef vReceiptId As Object = Nothing, Optional ByRef vPartyClaimID As Object = Nothing, Optional ByRef vReceiptAmount As Object = Nothing, Optional ByRef vDateofReceipt As Object = Nothing, Optional ByRef vPaymentId As Object = Nothing, Optional ByRef vClaimID As Object = Nothing, Optional ByRef vPaymentAmount As Object = Nothing, Optional ByRef vDateofPayment As Object = Nothing, Optional ByRef vComments As Object = Nothing, Optional ByRef vTable As Object = Nothing) As gPMConstants.PMEReturnCode` | Adds recovery directly to database |
| `DirectDelete` | `Public Function DirectDelete(Optional ByRef vRecoveryId As Object = Nothing) As Integer` | Deletes recovery directly from database |
| `CheckID` | `Public Function CheckID(ByRef vID As Object) As Integer` | Checks if recovery ID is valid |
| `GetDetails` | `Public Function GetDetails(Optional ByRef vLockMode As Object = Nothing, Optional ByRef vPerilId As Object = Nothing) As Integer` | Gets recoveries and populates collection |
| `GetNext` | `Public Function GetNext(Optional ByRef vRecoveryId As Object = Nothing, ..., Optional ByRef vTable As Object = Nothing) As Integer` | Gets next recovery from collection |
| `EditAdd` | `Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vRecoveryId As Object = Nothing, ..., Optional ByRef vReceiptToLossRate As Object = Nothing) As gPMConstants.PMEReturnCode` | Adds recovery to collection |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vRecoveryId As Object = Nothing, ..., Optional ByRef vReceiptToLossRate As Object = Nothing) As gPMConstants.PMEReturnCode` | Updates recovery in collection |
| `EditDelete` | `Public Function EditDelete(ByVal lRow As Integer) As Integer` | Marks recovery for deletion |
| `Cancel` | `Public Function Cancel() As Integer` | Checks for unsaved changes |
| `Update` | `Public Function Update() As Integer` | Saves all collection changes to DB |

**Entity Methods — CLMSalvageRecovery (bCLMSalvageRecoveryCls.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises with data component |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes data component |
| `New` | `Public Sub New()` | Constructor |
| `GetDefaults` | `Public Function GetDefaults(Optional ByRef vRecoveryId As Object = Nothing, ..., Optional ByRef vTable As Object = Nothing) As Integer` | Returns default values |
| `SetProperties` | `Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vRecoveryId As Integer = 0, Optional ByRef vRecoveryTypeID As Integer = 0, Optional ByRef vPerilId As Integer = 0, Optional ByRef vCurrencyID As Integer = 0, Optional ByRef vinitialReserve As Decimal = 0, Optional ByRef vRevisedReserve As Decimal = 0, Optional ByRef vReceivedToDate As Decimal = 0, Optional ByRef vRevisionCount As Integer = 0, Optional ByRef vReceiptId As Integer = 0, Optional ByRef vPartyClaimID As Integer = 0, Optional ByRef vReceiptAmount As Decimal = 0, Optional ByRef vDateofReceipt As Object = Nothing, Optional ByRef vPaymentId As Integer = 0, Optional ByRef vClaimID As Integer = 0, Optional ByRef vPaymentAmount As Decimal = 0, Optional ByRef vDateofPayment As Object = Nothing, Optional ByRef vComments As String = "", Optional ByRef vTable As Integer = 0, Optional ByRef vTaxAmount As Decimal = 0, Optional ByRef vReceiptToLossRate As Double = 0) As Integer` | Sets recovery properties |
| `GetProperties` | `Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vRecoveryId As Integer = 0, Optional ByRef vRecoveryTypeID As String = "", ..., Optional ByRef vTable As String = "") As Integer` | Gets recovery properties |

**Collection Methods — CLMSalvageRecoverys (bCLMSalvageRecoverys.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Add` | `Public Function Add(ByRef oNewCLMSalvageRecovery As bCLMSalvageRecovery.CLMSalvageRecovery) As Integer` | Adds to collection |
| `Count` | `Public Function Count() As Integer` | Returns count |
| `Delete` | `Public Sub Delete(ByRef vKey As Integer)` | Removes by key |
| `Item` | `Public Function Item(ByRef vKey As Integer) As bCLMSalvageRecovery.CLMSalvageRecovery` | Gets by key |
| `DeleteAll` | `Public Sub DeleteAll()` | Removes all |
| `Clear` | `Public Sub Clear()` | Resets collection |
| `Initialise` | `Public Function Initialise(ByRef sUserName As String, ...) As Integer` | Initialises |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes |
| `New` | `Public Sub New()` | Constructor |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_GetAllSalvageDetails` | `GetDetails` | Select all salvage recovery details by peril |
| `spu_Salvage_Recovery_Type` | *(lookup)* | Select salvage recovery types |
| `spu_Sal_chk_del_id` | `CheckID` | Check if recovery ID can be deleted |
| `spu_get_sal_coins_details` | *(lookup)* | Select coinsurance recovery details |
| `spu_get_sal_reins_details` | *(lookup)* | Select reinsurance recovery details |
| `spu_get_Peril_details` | *(lookup)* | Select peril details |
| `spu_get_CurrencyID` | *(lookup)* | Get default currency ID |
| `spu_Check_Recovery_Type` | *(validation)* | Check recovery type ID validity |
| `spu_delete_work_claim` | *(cleanup)* | Delete work claim records |
| `spu_get_claim_info_only_status` | *(status)* | Get claim info-only status |
| `spu_CloseClaim` | *(close)* | Close a claim |
| `spu_GetCurrentReserveRecovery` | *(reserve check)* | Get current reserve/recovery amounts |
| `spu_Get_Tax_Types_and_Bands` | *(tax)* | Get tax types and bands |
| `spu_get_client_policy_details` | *(policy)* | Get client policy details |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bPMLookup` | `Initialise` | PM lookup business component |

---

### 32. bCLMThirdPartyRecovery
**Directory:** `Third Party Recovery/`
**Project:** `bCLMThirdPartyRecovery`
**Purpose:** Manages third-party recovery records for claims — structurally identical to bCLMSalvageRecovery but for third-party recoveries.

**Business Methods — Business (bCLMBusiness.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises database and lookup |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes resources |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets process modes |
| `DirectAdd` | `Public Function DirectAdd(Optional ByRef vRecoveryId As Object = Nothing, Optional ByRef vRecoveryTypeID As Object = Nothing, ..., Optional ByRef vTable As Object = Nothing) As gPMConstants.PMEReturnCode` | Adds TP recovery directly to database |
| `DirectDelete` | `Public Function DirectDelete(Optional ByRef vRecoveryId As Object = Nothing) As Integer` | Deletes TP recovery directly |
| `CheckID` | `Public Function CheckID(ByRef vID As Object) As Integer` | Checks ID validity |
| `GetDetails` | `Public Function GetDetails(Optional ByRef vLockMode As Object = Nothing, Optional ByRef vPerilId As Object = Nothing) As Integer` | Populates collection |
| `GetNext` | `Public Function GetNext(Optional ByRef vRecoveryId As Object = Nothing, ...) As Integer` | Gets next from collection |
| `EditAdd` | `Public Function EditAdd(ByRef lRow As Integer, ..., Optional ByRef vReceiptToLossRate As Object = Nothing) As gPMConstants.PMEReturnCode` | Adds to collection |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow As Integer, ..., Optional ByRef vReceiptToLossRate As Object = Nothing) As gPMConstants.PMEReturnCode` | Updates in collection |
| `EditDelete` | `Public Function EditDelete(ByVal lRow As Integer) As Integer` | Marks for deletion |
| `Cancel` | `Public Function Cancel() As Integer` | Checks for unsaved changes |
| `Update` | `Public Function Update() As Integer` | Saves all changes to DB |

**Entity Methods — CLMThirdPartyRecovery (bCLMThirdPartyRecovery.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUserName As String, ..., Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises with data component |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes data component |
| `GetDefaults` | `Public Function GetDefaults(Optional ByRef vRecoveryId As Object = Nothing, ..., Optional ByRef vTable As Object = Nothing) As Integer` | Returns defaults |
| `SetProperties` | `Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vRecoveryId As Integer = 0, Optional ByRef vRecoveryTypeID As Integer = 0, Optional ByRef vPerilId As Integer = 0, Optional ByRef vCurrencyID As Integer = 0, Optional ByRef vInitialReserve As Decimal = 0, Optional ByRef vRevisedReserve As Decimal = 0, Optional ByRef vReceivedToDate As Decimal = 0, Optional ByRef vRevisionCount As Integer = 0, Optional ByRef vReceiptId As Integer = 0, Optional ByRef vPartyClaimID As Integer = 0, Optional ByRef vReceiptAmount As Decimal = 0, Optional ByRef vDateofReceipt As Object = Nothing, Optional ByRef vPaymentId As Integer = 0, Optional ByRef vClaimID As Integer = 0, Optional ByRef vPaymentAmount As Decimal = 0, Optional ByRef vDateofPayment As Object = Nothing, Optional ByRef vComments As String = "", Optional ByRef vTable As Integer = 0, Optional ByRef vTaxAmount As Decimal = 0, Optional ByRef vReceiptToLossRate As Double = 0) As Integer` | Sets TP recovery properties |
| `GetProperties` | `Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vRecoveryId As Integer = 0, ..., Optional ByRef vTable As String = "") As Integer` | Gets TP recovery properties |

**Collection Methods — CLMThirdPartyRecoverys (bCLMThirdPartyRecoverys.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Add` | `Public Function Add(ByRef oNewCLMThirdPartyRecovery As bCLMThirdParty.CLMThirdPartyRecovery) As Integer` | Adds to collection |
| `Count` | `Public Function Count() As Integer` | Returns count |
| `Delete` | `Public Sub Delete(ByRef vKey As Integer)` | Removes by key |
| `Item` | `Public Function Item(ByRef vKey As Integer) As bCLMThirdParty.CLMThirdPartyRecovery` | Gets by key |
| `DeleteAll` | `Public Sub DeleteAll()` | Removes all |
| `Clear` | `Public Sub Clear()` | Resets collection |
| `Initialise` | `Public Function Initialise(ByRef sUserName As String, ...) As Integer` | Initialises |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes |
| `New` | `Public Sub New()` | Constructor |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_GetAllTPDetails` | `GetDetails` | Select all third-party recovery details |
| `spu_TP_Recovery_Type` | *(lookup)* | Select TP recovery types |
| `spu_TP_chk_del_id` | `CheckID` | Check if TP recovery ID can be deleted |
| `spu_get_TP_coins_details` | *(lookup)* | Select TP coinsurance recovery details |
| `spu_get_TP_reins_details` | *(lookup)* | Select TP reinsurance recovery details |
| `spu_get_TP_Peril_details` | *(lookup)* | Select TP peril details |
| `spu_get_tp_CurrencyID` | *(lookup)* | Get default TP currency ID |
| `spu_TPRecovery_Type` | *(validation)* | Check TP recovery type validity |
| `spu_delete_work_claim` | *(cleanup)* | Delete work claim records |
| `spu_get_claim_info_only_status` | *(status)* | Get claim info-only status |
| `spu_CloseClaim` | *(close)* | Close a claim |
| `spu_GetCurrentReserveRecovery` | *(reserve check)* | Get current reserve/recovery amounts |
| `spu_Get_Tax_Types_and_Bands` | *(tax)* | Get tax types and bands |
| `spu_get_client_policy_details` | *(policy)* | Get client policy details |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bPMLookup` | `Initialise` | PM lookup business component |

---

### 33. bOpenClaim
**Directory:** `Open Claim/`
**Project:** `bOpenClaim`
**Purpose:** Core claim creation and management component — handles opening claims, setting/getting claim properties (including additional S4B fields), address management, risk details, policy lookups, duplicate detection, claim numbering, events, renewal checks, claim deletion, reserve management, and progress status.

**Business Methods — Business (bOpenClaimBusiness.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises database, lookup, event, BOLink, data component |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes all sub-components |
| `New` | `Public Sub New()` | Constructor |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets process modes (also delegates to BOLink) |
| `GetLookupValues` | `Public Function GetLookupValues(ByVal iLookupType As Integer, ByRef vTableArray(,) As Object, ByVal iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer` | Gets lookup values for claim dropdowns |
| `GetCountryName` | `Public Function GetCountryName(ByRef v_sCountryName As String, ByVal v_CountryID As Integer) As Integer` | Gets country name by ID |
| `SelectSecondaryCause` | `Public Function SelectSecondaryCause(ByRef rvResultArray(,) As Object) As Integer` | Selects secondary cause for primary cause |
| `Add` | `Public Function Add() As Integer` | Adds claim to database |
| `Update` | `Public Function Update() As Integer` | Updates claim in database |
| `AddClaimComments` | `Public Function AddClaimComments(ByRef vClaimComments As Object) As Integer` | Adds claim comments |
| `UpdateClaimComments` | `Public Function UpdateClaimComments(ByRef vClaimComments As Object) As Integer` | Updates claim comments |
| `GetClaimComments` | `Public Function GetClaimComments(ByRef m_vClaimComments As Object) As Integer` | Gets claim comments |
| `SetKeyID` | `Public Function SetKeyID(ByVal vvntClaimNo As Object) As Integer` | Sets claim key/ID |
| `SelectSingle` | `Public Function SelectSingle() As Integer` | Selects single claim record |
| `GetClientDetails` | `Public Function GetClientDetails(ByVal vvntPolicyNo As Integer, ByVal vvntClaimDate As String, ByRef rvntResultArray() As Object) As Integer` | Gets client details for policy |
| `GetInsurerDetails` | `Public Function GetInsurerDetails(ByVal vvntPolicyNo As Integer, ByVal vvntClaimDate As String, ByRef rvntResultArray() As Object, Optional ByVal lTransactionMode As Integer = gPMConstants.PMEComponentAction.PMView) As Integer` | Gets insurer details |
| `GetPartyDetails` | `Public Function GetPartyDetails(ByVal v_vShortname As Object, ByVal v_iAddressType As Integer, ByRef r_vResultArray() As Object) As Integer` | Gets party details by shortname |
| `DeleteClaim` | `Public Function DeleteClaim() As Integer` | Deletes claim record |
| `SetProperties` | `Public Function SetProperties(ByVal PMMode As Integer, ByVal vvntClaimNo As Object, ByVal vvntPolicyNo As Object, ByVal vvntPolicyID As Object, ByVal vvntDescription As Object, ByVal vvntClaimStatusID As Object, ByVal vvntProgressStatusID As Object, ByVal vvntPrimaryCauseID As Object, ByVal vvntSecondaryCauseID As Object, ByVal vvntCatastropheCodeID As Object, ByVal vvntLossFromDate As Object, ByRef vvntLossToDate As Object, ByVal vvntReportedDate As Object, ByVal vvntReportedToDate As Object, ByVal vvntLastModifiedDate As Object, ByVal vvntHandlerID As Object, ByVal vvntCurrencyID As Object, ByVal vvntInfoOnly As Object, ByVal vvntLikelyClaim As Object, ByVal vvntLocation As Object, ByVal vvntTown As Object, ByVal vvntRiskTypeID As Object, ByVal vvntClientName As Object, ByVal vvntClientAddress As Object, ByVal vvntClientTelNo As Object, ByVal vvntClientFaxNo As Object, ByVal vvntClientMobileNo As Object, ByVal vvntClientEmail As Object, ByVal vvntClientClaimNo As Object, ByVal vvntInsurerName As Object, ByVal vvntInsurerAddress As Integer, ByVal vvntInsurerTelNo As Object, ByVal vvntInsurerFaxNo As Object, ByVal vvntInsurerEmail As Object, ByVal vvntInsurerClaimNo As Object, ByVal vvntInsurerContact As Object, ByVal vvntVATRegistered As Object, ByVal vvntVATRegisteredNo As Object, ByVal vvntComments As Object, ByVal vvntClaimsStatusDate As Object, ByVal vvntClientShortName As Object, ByVal vvntInsurerShortName As Object, ByVal vvntClientTelNooff As Object, ByVal vvntClaimID As Object, ByVal vvntUserDefFldA As Object, ByVal vvntUserDefFldB As Object, ByVal vvntUserDefFldC As Object, ByVal vvntUserDefFldD As Object, ByVal vvntUserDefFldE As Object, ByVal vvntSourceID As Object, ByVal vvntLanguageID As Object, ByVal vvntUnderwritingYearID As Object, ByVal vvntClaimHandled As Object, ByVal v_iUserOtherPartyID As Object, Optional ByVal v_vBaseCaseID As Object = Nothing) As Object` | Sets all claim properties |
| `SetAdditionalProperties` | `Public Function SetAdditionalProperties(ByVal vvDriverTitle As Object, ByVal vvDriverForename As Object, ByVal vvDriverSurname As Object, ByVal vvDatePassedTest As Object, ByVal vvEmployeeTitle As Object, ByVal vvEmployeeForename As Object, ByVal vvEmployeeSurname As Object, ByVal vvEmployeeLengthOfService As Object, ByVal vvEmployeePreviousClaim As Object, ByVal vvEmployeePreviousClaimDetails As Object, ByVal vvULR As Object, ByVal vvRecoveryAgent As Object, ByVal vvSolicitorAppointed As Object, ByVal vvSolicitorName As Object, ByVal vvULRLossDetails As Object, ByVal vvClaimAtFaultId As Object, ByVal vvBonusAffected As Object, ByVal vvPolicyDeductibleId As Object, ByVal vvNonStandardExcess As Object, ByVal vvSubsidiaryCompanyName As Object) As Integer` | Sets S4B additional claim properties |
| `GetProperties` | `Public Function GetProperties(ByVal PMMode As Integer, ByRef rsClaimNo As String, ByRef rsPolicyNo As String, ByRef rlPolicyID As Integer, ByRef rsDescription As String, ByRef rlClaimStatusID As Integer, ByRef rlProgressStatusID As Integer, ..., ByRef rlVersionId As Integer, ByRef rvClaimHandled As Object, Optional ByRef r_sCaseNumber As String = "", Optional ByRef r_lCaseID As Integer = 0, Optional ByRef otherpartyID As Object = Nothing, Optional ByRef otherpartyName As Object = Nothing) As Object` | Gets all claim properties |
| `GetAdditionalProperties` | `Public Function GetAdditionalProperties(ByRef rsDriverTitle As String, ByRef rsDriverForename As String, ByRef rsDriverSurname As String, ByRef rvDatePassedTest As Object, ByRef rsEmployeeTitle As String, ByRef rsEmployeeForename As String, ByRef rsEmployeeSurname As String, ByRef rlEmployeeLengthOfService As Integer, ByRef rbEmployeePreviousClaim As Boolean, ByRef rsEmployeePreviousClaimDetails As String, ByRef rbULR As Boolean, ByRef rsRecoveryAgent As String, ByRef rbSolicitorAppointed As Boolean, ByRef rsSolicitorName As String, ByRef rsULRLossDetails As String, ByRef rlClaimAtFaultId As Integer, ByRef rbBonusAffected As Boolean, ByRef rlPolicyDeductibleId As Integer, ByRef rdNonStandardExcess As Double, ByRef rsSubsidiaryCompanyName As String) As Integer` | Gets S4B additional properties |
| `GetRiskDetails` | `Public Function GetRiskDetails(ByVal vvntPolicyNo As Integer, ByVal vvntClaimDate As String, ByRef rvntResultArray(,) As Object, Optional ByVal vvntClaimID As Object = Nothing) As Integer` | Gets risk details via BOLink |
| `GetClmAdd` | `Public Function GetClmAdd(ByRef r_vResultArray(,) As Object, ByVal v_lAdd_cnt As Integer) As Integer` | Gets claim address |
| `GetRiskDesc` | `Public Function GetRiskDesc(ByRef rvntResultArray(,) As Object, ByVal vvntRiskId As Integer) As Integer` | Gets risk description |
| `UpdateAddress` | `Public Function UpdateAddress(ByVal v_lAddress_Cnt As Integer, ByVal v_sAddress1 As String, ByVal v_sAddress2 As String, ByVal v_sAddress3 As String, ByVal v_sAddress4 As String, ByVal v_sPostalCode As String, ByVal v_lAddressUsage As Integer, ByVal v_lAddressId As Integer, Optional ByVal v_lCountryID As Integer = 0) As Integer` | Updates claim address |
| `AddAddress` | `Public Function AddAddress(ByVal v_sAddress1 As String, ByVal v_sAddress2 As String, ByVal v_sAddress3 As String, ByVal v_sAddress4 As String, ByVal v_sPostalCode As String, ByVal v_lAddressUsage As Integer, ByVal v_lAddressId As Integer, ByRef r_iAddCnt As Integer, ByVal v_bUpdateAddress As Boolean, Optional ByVal v_lCountryID As Integer = 0) As Integer` | Adds claim address |
| `GetPolicyDetails` | `Public Function GetPolicyDetails(ByRef r_vResultArray(,) As Object, ByVal v_lpol_id As Integer, Optional ByVal v_vclm_dt As String = "") As Integer` | Gets policy details |
| `CreateEvent` | `Public Function CreateEvent(ByRef r_lEventCnt As Integer, ByVal v_vClaimCnt As Object, ByVal v_vDescription As Object, ByVal v_lEventTypeId As Object, ByVal v_lPartyid As Integer) As Integer` | Creates event for claim |
| `GetOption` | `Public Function GetOption(ByVal v_iOptionNumber As Integer, ByRef r_nOptionValue As Integer) As Integer` | Gets system option value |
| `GetUserDefinedCaption` | `Public Function GetUserDefinedCaption(ByRef r_vResultArray(,) As Object, ByVal Tableid As Integer) As Integer` | Gets user-defined captions |
| `getUnderwritingOrAgency` | `Public Function getUnderwritingOrAgency() As Integer` | Determines underwriting or agency type |
| `GetInsuranceFolderCnt` | `Public Function GetInsuranceFolderCnt(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets insurance folder count |
| `GetInsuranceFile` | `Public Function GetInsuranceFile(ByVal v_lEventCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Gets insurance file details |
| `GetCoInsurerDetails` | `Public Function GetCoInsurerDetails(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lClaimId As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets co-insurer split |
| `GenerateClaimNumber` | `Public Function GenerateClaimNumber(ByVal v_lBusinessType As Integer, ByVal v_iBranchId As Integer, ByVal v_lProductID As Integer, ByVal v_lAgentId As Integer, ByRef r_sGeneratedClaimNumber As String, ByVal v_sLossYear As String, ByVal v_sReportedYear As String, Optional ByVal v_nPartyCnt As Integer = 0) As Integer` | Auto-generates claim number |
| `GetOriginalClaimNo` | `Public Function GetOriginalClaimNo(ByVal v_lClaimId As Integer, ByRef r_lOriginalClaimID As Integer) As Integer` | Gets original/base claim ID |
| `CheckClaimNumber` | `Public Function CheckClaimNumber(ByVal v_sClaimNumber As String, ByRef r_lClaimID As Integer) As Integer` | Validates claim number exists |
| `GetValidPrimaryCauses` | `Public Function GetValidPrimaryCauses(ByVal v_lInsFileCnt As Integer, ByRef r_vValidPrimaryCauses As Object) As Integer` | Gets valid primary causes for product |
| `GetPolicyStatus` | `Public Function GetPolicyStatus(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lPolicyStatus As Integer) As Integer` | Gets policy status |
| `GetPolicyType` | `Public Function GetPolicyType(ByVal v_lPolicyId As Integer, ByRef r_sType As String) As Integer` | Gets policy type |
| `GetClaimRiskStatus` | `Public Function GetClaimRiskStatus(ByVal v_lRiskCnt As Integer, ByRef r_bIsDeferred As Boolean) As Integer` | Checks if risk is deferred for RI |
| `CheckRenewal` | `Public Function CheckRenewal(ByVal v_lInsurance_File_Cnt As Integer, ByRef r_lInsurance_Folder_Cnt As Integer, Optional ByRef r_lRenewal_Frequency_Id As Integer = 0) As Integer` | Checks renewal status |
| `SetExistingRenQuotesToReplaced` | `Public Function SetExistingRenQuotesToReplaced(ByRef v_lInsuranceFolderCnt As Integer) As Integer` | Sets existing renewal quotes to replaced |
| `UpdateRenewalStatusType` | `Public Function UpdateRenewalStatusType(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_sRenewalStatusTypeCode As String) As Integer` | Updates renewal status type |
| `GetShowBrokingRiskDetails` | `Public Function GetShowBrokingRiskDetails(ByVal v_lClaimId As Integer, ByRef r_vDataArray(,) As Object) As Integer` | Gets broking risk details |
| `GetClientPolicyDetails` | `Public Function GetClientPolicyDetails(ByVal v_lInsuranceFileCnt As Integer, Optional ByRef r_lPartyCnt As Integer = 0, Optional ByRef r_lInsuranceFolderCnt As Integer = 0, Optional ByRef r_sInsuranceRef As String = "", Optional ByRef r_vRenewalDate As String = "", Optional ByRef r_lPolicyTypeId As Integer = 0, Optional ByRef r_sPartyShortName As String = "") As Integer` | Gets client and policy details |
| `GetDefaultContacts` | `Public Function GetDefaultContacts(ByVal v_lPolicyId As Integer, ByRef r_vResults() As Object, ByVal v_bIsClient As Boolean) As Integer` | Gets default contact details |
| `GetPolicyForClaimDate` | `Public Function GetPolicyForClaimDate(ByVal v_dtClaimDate As Date, ByRef r_lInsuranceFileCnt As Integer, ByRef r_sPolicyNumber As String, ByRef r_dtStartDate As Date, ByRef r_dtEndDate As Date, Optional ByRef r_lReturnCode As Integer = 0, Optional ByRef r_dtInceptionDate As Date = #12:00:00 PM#) As Integer` | Gets policy for a claim date |
| `GetClaimTaskUserGroup` | `Public Function GetClaimTaskUserGroup(ByRef r_lTaskGroupID As Integer, ByRef r_lUserGroupID As Integer) As Integer` | Gets claim task and user group |
| `GetDefaultUnderwritingYear` | `Public Function GetDefaultUnderwritingYear(ByVal v_lPolicyId As Integer, ByRef r_vUnderwritingYearID As Object) As Integer` | Gets default underwriting year |
| `RetrieveCurrenciesForBranch` | `Public Function RetrieveCurrenciesForBranch(ByRef iSourceID As Integer, ByRef vReturnArray(,) As Object) As Integer` | Gets currencies for branch |
| `GetClaimNumber` | `Public Function GetClaimNumber(ByVal v_lClaimId As Integer, ByRef r_sClaimNumber As String) As Integer` | Gets claim number for claim ID |
| `GetCancellationDate` | `Public Function GetCancellationDate(ByVal v_lInsuranceFileCnt As Integer, ByRef r_dtCancellationDate As Date) As Integer` | Gets policy cancellation date |
| `GetDuplicateClaims` | `Public Function GetDuplicateClaims(ByVal v_sPolicyNumber As String, ByVal v_dtLossDate As Date, ByVal v_lRiskTypeId As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets potential duplicate claims |
| `GetDuplicateClaimOverrideUsers` | `Public Function GetDuplicateClaimOverrideUsers(ByRef r_vResults(,) As Object) As Integer` | Gets users with duplicate override authority |
| `AddClaimLink` | `Public Function AddClaimLink(ByVal v_lClaimId As Integer, ByVal v_lLinkTypeId As Integer, ByVal v_lLinkId As Integer) As Integer` | Adds claim link entry |
| `GetClaimTransactionSuppressionInd` | `Public Function GetClaimTransactionSuppressionInd(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lClaimId As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets transaction suppression indicators |
| `IsProgressStatusClosed` | `Public Function IsProgressStatusClosed(ByVal v_lClaimStatusId As Integer, ByRef r_bIsClosed As Boolean) As Integer` | Checks if progress status is closed |
| `GetClaimReserves` | `Public Function GetClaimReserves(ByVal v_lClaimId As Integer, ByVal v_lRiskTypeId As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Gets claim reserve amounts |
| `ResetClaimReserves` | `Public Function ResetClaimReserves(ByVal v_lClaimId As Integer) As Integer` | Resets reserves to zero |
| `GetCurrentReserveRecovery` | `Public Function GetCurrentReserveRecovery(ByVal v_lClaimId As Integer, ByRef r_vDataArray(,) As Object) As Integer` | Gets current reserve/recovery |
| `GetPolicyAccountHandlers` | `Public Function GetPolicyAccountHandlers(ByVal v_lPolicyId As Integer, ByRef r_sAccountHandler As String, ByRef r_sAccountExecutive As String) As Integer` | Gets policy account handlers |
| `ValidateClaimNumber` | `Public Function ValidateClaimNumber(ByVal sEnteredNumber As String, ByVal v_lBusinessType As Integer, ByVal v_lProductID As Integer, ByRef sFailureReason As String) As Integer` | Validates claim number format |
| `GetFSAComplianceValue` | `Public Function GetFSAComplianceValue(ByRef r_vValue As Object) As Integer` | Gets FSA compliance value |
| `SelectRenewalStatusType` | `Public Function SelectRenewalStatusType(ByVal v_lInsuranceFolderCnt As Integer, ByRef r_sRenewalStatusTypeCode As String) As Integer` | Selects renewal status type |
| `UpdateClaimPolicyDetails` | `Public Function UpdateClaimPolicyDetails() As Integer` | Updates claim policy details |
| `LogMessageToPMMessageTable` | `Public Sub LogMessageToPMMessageTable(ByVal v_iType As Integer, ByVal v_sMsg As String, ByVal v_sCallingAppName As String, Optional ByRef v_vApp As Object = Nothing, Optional ByRef v_vClass As Object = Nothing, Optional ByRef v_vMethod As Object = Nothing, Optional ByRef v_vErrNo As Object = Nothing, Optional ByRef v_vErrDesc As Object = Nothing)` | Logs message to PM messages table |
| `GetClaimTypeAndCover` | `Public Function GetClaimTypeAndCover(ByVal v_lRiskTypeID As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets claim type and cover |
| `GetGISRetroactiveDate` | `Public Function GetGISRetroactiveDate(ByVal v_lInsurancefileID As Integer, ByRef r_vResults(,) As Object, Optional ByVal v_lRiskCnt As Integer = 0) As Integer` | Gets GIS retroactive date |
| `GetProgressStatus` | `Public Function GetProgressStatus(ByVal sTransaction_Type As String, ByRef r_vDataArray(,) As Object) As Integer` | Gets progress status options |
| `GetClaimHandler` | `Public Function GetClaimHandler(ByRef r_vDataArray(,) As Object) As Integer` | Gets claim handler |
| `GetProgressStatusDetails` | `Public Function GetProgressStatusDetails(ByVal iProgressStatusID As Integer, ByRef r_vDataArray(,) As Object) As Integer` | Gets progress status details |
| `GetShowRiskDetails_U` | `Public Function GetShowRiskDetails_U(ByVal v_lClaimId As Integer, ByRef r_vDataArray(,) As Object) As Integer` | Gets underwriting risk details for claims |
| `GetSpecificUserAuthority` | `Public Function GetSpecificUserAuthority(ByVal v_vAuthority As Object, ByRef r_vAuthorityValue As Object) As Integer` | Gets specific user authority level |
| `GetRiskDetailsForClaim` | `Public Function GetRiskDetailsForClaim(ByRef rvntResultArray(,) As Object, ByVal vvntInsuranceFileCnt As Integer) As Integer` | Gets risk details for existing claim |
| `GetUserOtherParty` | `Public Function GetUserOtherParty(ByVal iUserID As Integer, ByRef r_vResultArray(,) As Object) As Long` | Gets user's other party ID |

**Automated Methods — Automated (bOpenClaimAutomated.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises automated component |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes resources |
| `New` | `Public Sub New()` | Constructor |
| `GetSummary` | `Public Function GetSummary(ByRef vSummaryArray As Object) As Integer` | Gets summary array |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets process modes |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Sets key array parameters |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Gets key array parameters |
| `Start` | `Public Function Start() As Integer` | Starts automated processing |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_get_clm_add` | `GetClmAdd` | Get claim address |
| `spu_User_Def_Caption` | `GetUserDefinedCaption` | Get user-defined captions |
| `spe_{SQLTableName}_saa` | `GetDetails` | Select all claim records (dynamic) |
| `spu_Claim_Check_No` | `CheckClaimNumber` | Check claim number exists |
| `Spe_Secondary_Cause_Sel` | `SelectSecondaryCause` | Select secondary causes for primary cause |
| `spu_Claim_Address_add` | `AddAddress` | Add claim address |
| `spu_Claim_Address_upd` | `UpdateAddress` | Update claim address |
| `spe_Insurance_File_sel` | `GetInsuranceFile` | Select insurance file details |
| `spu_delete_claim` | `DeleteClaim` | Delete claim records |
| `spu_CLM_Get_Base_Claim` | `GetOriginalClaimNo` | Get original/base claim ID |
| `spu_Get_Policy_Type` | `GetPolicyType` | Get policy type |
| `spu_SIRRen_CheckRenewals` | `CheckRenewal` | Check renewal status |
| `spu_renewal_control_Update` | `UpdateRenewalStatusType` | Update renewal control |
| `spu_Renewal_Control_Sel` | `SelectRenewalStatusType` | Select renewal control |
| `spu_SIR_set_exist_ren_to_rep` | `SetExistingRenQuotesToReplaced` | Set existing renewals to replaced |
| `spu_get_clm_party_dtls` | `GetPartyDetails` | Get claim party details |
| `spu_GetShowBrokingRiskDetails` | `GetShowBrokingRiskDetails` | Get broking risk details |
| `spu_get_client_policy_details` | `GetClientPolicyDetails` | Get client policy details |
| `spu_CLM_Get_Valid_Primary_Causes` | `GetValidPrimaryCauses` | Get valid primary causes |
| `spu_CLM_risk_status_sel` | `GetClaimRiskStatus` | Get claim risk status (deferred check) |
| `spu_Get_Policy_For_Claim_Date` | `GetPolicyForClaimDate` | Get policy for a claim date |
| `spu_get_claim_user_task_group` | `GetClaimTaskUserGroup` | Get claim user and task group |
| `spu_Get_ClaimNumber` | `GetClaimNumber` | Get claim number for claim ID |
| `spu_clm_get_all_policy_versions` | *(policy versions)* | Get all policy versions |
| `spu_get_Insurance_Folder` | `GetInsuranceFolderCnt` | Get insurance folder count |
| `spu_clm_get_duplicate_claim` | `GetDuplicateClaims` | Get duplicate claims |
| `spu_clm_get_duplicate_claim_override_users` | `GetDuplicateClaimOverrideUsers` | Get duplicate override users |
| `spu_clm_claim_link_add` | `AddClaimLink` | Add claim link |
| `spu_clm_transaction_suppression_ind_sel` | `GetClaimTransactionSuppressionInd` | Get transaction suppression indicators |
| `spu_clm_is_progress_status_closed` | `IsProgressStatusClosed` | Check if progress status is closed |
| `spu_clm_reset_reserves` | `ResetClaimReserves` | Reset outstanding reserves to zero |
| `spu_clm_get_policy_account_handlers` | `GetPolicyAccountHandlers` | Get policy account handlers |
| `spu_CLM_Get_CoInsurer_Split` | `GetCoInsurerDetails` | Get co-insurer split |
| `spu_GetCurrentReserveRecovery` | `GetCurrentReserveRecovery` | Get current reserve/recovery amounts |
| `spu_Add_PMMessage` | `LogMessageToPMMessageTable` | Add PM message |
| `spu_Get_Claim_Type_And_Cover` | `GetClaimTypeAndCover` | Get claim type and cover |
| `spu_gis_get_retroactivedate_property` | `GetGISRetroactiveDate` | Get GIS retroactive date property |
| `spu_CLM_Get_Progress_Status` | `GetProgressStatus` | Get progress status list |
| `spu_CLM_Get_Progress_Status_details` | `GetProgressStatusDetails` | Get progress status details |
| `spu_GetShowRiskDetails_U` | `GetShowRiskDetails_U` | Get underwriting risk details |
| `spu_Specific_User_Authority_Sel` | `GetSpecificUserAuthority` | Select specific user authority |
| `spu_CLM_Get_Claim_Handler` | `GetClaimHandler` | Get claim handler details |
| `spu_get_risk_details_for_claim` | `GetRiskDetailsForClaim` | Get risk details for claim |
| `spu_get_otherparty_details` | *(other party)* | Get other party details |
| `spu_Get_User_OtherPartyID` | `GetUserOtherParty` | Get user other party ID |
| `spu_get_prod_auto_num_ids` | `GenerateClaimNumber` | Get numbering scheme IDs from product |
| `spu_numbering_scheme_saa` | `GenerateClaimNumber` | Get numbering scheme |
| `spu_abandoned_numbers_saa` | `GenerateClaimNumber` | Get abandoned numbers |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bSIREvent` | `CreateEvent` | Event creation for claims |
| `bBackOfficeLink` | `Initialise`, `GetRiskDetails`, `SetProcessModes` | Back office link for risk details and system options |
| `bSIRPolicyNumMaint` | `GenerateClaimNumber` | Policy/claim number generation |
| `bPMLookup` | `Initialise`, `GetLookupValues` | Lookup values |
| `dOpenClaim` | `Initialise`, `SetProperties`, `GetProperties`, etc. | Data access layer |

---

### 34. bSIRCheckAuthorityLevel
**Directory:** `User Authority Levels/`
**Project:** `bSIRCheckAuthorityLevel`
**Purpose:** Authority level checking component — loads and executes rule scripts (or compiled rules) to determine if a user is authorised to perform payment actions based on product, amount, currency, and transaction type.

**Business Methods — Business (Business.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceId As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises database and rule engine |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Disposes resources |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets process modes |
| `LoadRule` | `Public Function LoadRule() As Integer` | Locates and loads the applicable authority rule file |
| `LoadRuleFile` | `Public Function LoadRuleFile(ByVal v_sRuleFileName As String, Optional ByVal v_sRuleFilePath As String = "") As Integer` | Loads a specified rule file |
| `ExecuteRule` | `Public Function ExecuteRule(ByRef r_bAuthorised As Boolean) As Integer` | Executes the loaded rule and returns authorisation result |

**Data Class — cAuthLevelData (cAuthLevelData.vb):**

| Property | Type | Description |
|----------|------|-------------|
| `AuthError` | `Integer` | Script error value |
| `AuthUserID` | `Integer` | User who performed original action |
| `CurrentUserID` | `Integer` | User performing operation |
| `IsAuthorised` | `Boolean` | Whether user action is authorised |
| `PaymentAmount` | `Double` | Amount of payment to be authorised |
| `PaymentCurrencyAmount` | `Double` | Amount in payment currency |
| `PaymentType` | `Integer` | Type of payment |
| `ProductID` | `Integer` | Product ID |
| `Reference` | `Integer` | Reference ID (claim ID) |
| `TransType` | `Integer` | Transaction type |
| `CurrencyCode` | `String` | Currency ISO code |
| `PaymentCurrencyCode` | `String` | Payment currency ISO code |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_SIR_Get_Rule_File_Name` | `LoadRule` (via `GetRuleFileName`) | Gets the authority rule script filename for user/product |
| `spu_SIR_Get_Risk_Type_Rule_Set_Type` | `LoadRule` (via `GetRuleCompiled`) | Gets whether rules are compiled or script-based |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bGISPMUExtras` | `ExecuteRuleScript` | Extras object passed to rule script engine |
| `bACTCurrency` | `SetUpAuthLevelData` | Currency ISO code lookup from ID |
| `SharedQuoteEngine` | `ExecuteRuleScript` | VBQuoteEngine for executing authority rule scripts |
