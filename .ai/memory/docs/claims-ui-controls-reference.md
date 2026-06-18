# Claims UI Controls & Interfaces Reference

> **Claims Module — User Controls and Navigator Roadmaps**
> Source: `Claims\Components\User Controls\` (10 user controls) · `Claims\Components\Roadmaps\` (3 navigator roadmaps)
>
> This file documents all public API surface: properties, events, methods, and the business components each control delegates to. Direct stored procedure calls do not appear in the UI layer — all SPs are called by the business components listed in the cross-reference at the end.
>
> **See also:** `.github/docs/claims-components-reference.md` for the underlying `bCLM*` business component reference (methods and stored procedures).

---

## Architecture Overview

```
Calling Form / Navigator Step
  └─ User Control (uctCLM* / uctClaimParty)
       ├─ MainModule / iCLM*Control (constants, globals)
       ├─ [data model class: cPaymentItem / cReceiptItem / cClaimDetails]
       └─ Business Component (bCLM*.Business / bACT*.Form)
            └─ SQL Server Stored Procedures (spu_* / spg_*)
```

All user controls follow the same lifecycle:
1. `Initialise()` — creates `bObjectManager`, retrieves business object instance, sets language/source/user IDs
2. `SetProcessModes()` — sets Task (PMAdd/PMEdit/PMView), Navigate, ProcessMode, TransactionType, EffectiveDate
3. `LoadControl()` / `Load_Renamed()` — loads data from business into interface
4. `Save()` — pushes interface data back to business; business handles SP calls and transactions

---

## Component Index

| # | User Control | Class Name | ProgId | Business Components Used |
|---|---|---|---|---|
| 1 | `uctClaimParty` | `uctClaimParty` | `uctClaimParty_NET.uctClaimParty` | `bCLMClaimParty.Business` |
| 2 | `uctCLMCaseClaimList` | `uctCLMCaseClaim` | `uctCLMCaseClaim_NET.uctCLMCaseClaim` | `bCLMCase.Business` |
| 3 | `uctCLMCaseHeader` | `uctCLMCaseHeader` | `uctCLMCaseHeader_NET.uctCLMCaseHeader` | `bCLMCase.Business` |
| 4 | `uctCLMListPayments` | `uctCLMListPaymentsC` | `uctCLMListPaymentsC_NET.uctCLMListPaymentsC` | `bCLMPeril.Business`, `bACTCurrencyConvert.Form` |
| 5 | `uctCLMListReceipts` | `uctCLMListReceiptsC` | `uctCLMListReceiptsC_NET.uctCLMListReceiptsC` | `bCLMPeril.Business`, `bACTCurrencyConvert.Form` |
| 6 | `uctCLMListVersionControl` | `uctCLMVersions` | `uctCLMVersions_NET.uctCLMVersions` | `bCLMFindClaim.Business`, `bCLMCase.Business` |
| 7 | `uctCLMPayment` | `uctCLMPayment` | `uctCLMPayment_NET.uctCLMPayment` | Host-provided business object, `iPMFormControl.FormFields` |
| 8 | `uctCLMPerilRT` | `uctCLMPerilRT` | `uctCLMPerilRT_NET.uctCLMPerilRT` | `bCLMPeril.Business` |
| 9 | `uctCLMReceipt` | `uctCLMReceipt` | `uctCLMReceipt_NET.uctCLMReceipt` | `bCLMPeril.Business`, `bACTCurrencyConvert.Form`, `bCLMPaymentMethod.Business` |
| 10 | `uctCLMReserve` | `uctCLMReserve` | `uctCLMReserve_NET.uctCLMReserve` | Host-provided business object, `bControlTransClaims.Automated` |
| 11 | `iCLMMaintainClaim` *(roadmap)* | `Interface_Renamed` | `Interface_Renamed_NET.Interface_Renamed` | Navigator V3 framework (`frmMain`, XML roadmap `MAINCLM.XML`) |
| 12 | `iCLMPaymentOfClaim` *(roadmap)* | `Interface_Renamed` | `Interface_Renamed_NET.Interface_Renamed` | Navigator V3 framework (`frmMain`, XML roadmap `PAYCLM.XML`) |
| 13 | `iCLMSalvage` *(roadmap)* | `Interface_Renamed` | `Interface_Renamed_NET.Interface_Renamed` | Navigator V3 framework (`frmMain`, XML roadmap `SALVAGE.XML`) |

---

## User Controls — Detailed Reference

---

### 1. uctClaimParty — Claim Party

**Files:** `uctClaimParty\uctClaimParty.vb` · `uctClaimParty\iCLMPartyControl.vb`
**Class:** `Partial Public Class uctClaimParty` · Inherits `System.Windows.Forms.UserControl`
**Global state module (`iCLMPartyControl.vb`):** `g_oObjectManager As bObjectManager.ObjectManager`, `g_oBusiness As Object`, `g_iSourceID/LanguageID/UserId As Integer`, `ACApp="uctClaimPartyControl"`

Manages the list of parties associated with a claim (insured, third party, driver, etc.). Provides add/edit/delete of claim parties from a `ListView`. Delegates all data operations to `bCLMClaimParty.Business`.

#### Public Events

| Event | Raised When |
|---|---|
| `ClaimChange()` | `ClaimId` property set |
| `PartyTypeCodeChange()` | `PartyTypeCode` property set |
| `PartyTypeChange()` | `PartyType` property set |
| `EffectiveDateChange()` | `EffectiveDate` property set |
| `TransactionTypeChange()` | `TransactionType` property set |
| `ProcessModeChange()` | `ProcessMode` property set |
| `NavigateChange()` | `Navigate` property set |
| `TaskChange()` | `Task` property set |
| `CallingAppNameChange()` | `CallingAppName` property set |

#### Public Properties

| Property | Type | Access | Description |
|---|---|---|---|
| `ErrorNumber` | `Integer` | ReadOnly | Last error number from the interface |
| `CallingAppName` | `String` | WriteOnly | Identifies the calling application in logs |
| `Status` | `Integer` | ReadOnly | Exit status after `ProcessCommand()` (`PMOk`/`PMCancel`/`PMError`) |
| `Task` | `Integer` | Read/Write | Component action: `PMAdd`, `PMEdit`, or `PMView` |
| `Navigate` | `Integer` | WriteOnly | Navigate flag passed to business |
| `ProcessMode` | `Integer` | WriteOnly | Process mode (NB/MTA/Renewal/etc.) |
| `TransactionType` | `String` | WriteOnly | Transaction type code (e.g. `C_CO`, `C_CR`, `C_CP`) |
| `EffectiveDate` | `Date` | WriteOnly | Effective date for the transaction |
| `PartyType` | `Integer` | Read/Write | Party type ID |
| `PartyTypeCode` | `String` | Read/Write | Party type code string |
| `ClaimId` | `Integer` | Read/Write | The claim being worked on; setting raises `ClaimChange` |
| `RiskTypeId` | `Integer` | Read/Write | Risk type ID |
| `PerilTypeId` | `Integer` | Read/Write | Peril type ID |
| `PartyCount` | `Integer` | ReadOnly | Number of parties in `m_vPartyArray` |

#### Public Methods

| Method | Signature | Description |
|---|---|---|
| `Initialise` | `() As Integer` | Creates `bObjectManager`, retrieves `bCLMClaimParty.Business` via `g_oObjectManager.GetInstance`. Thread-safe (`Static bIsInitialised`). Returns `PMTrue`/`PMError`. |
| `SetProcessModes` | `(Optional vTask, vNavigate, vProcessMode, vTransactionType, vEffectiveDate) As Integer` | Stores all process-mode values into private members; raises corresponding change events |
| `LoadControl` | `() As Integer` | Calls `m_oBusiness.SetProcessModes(...)`, sets `ClaimId`, `RiskTypeId`, `PerilTypeId` on business; creates `iPMFormControl.FormFields`; calls `SetFieldValidation()`, `SetInterfaceDefaults()` |
| `GetParties` | `() As Integer` | Calls `GetBusiness()` to retrieve party array from business, then `BusinessToInterface()` to populate `ListView`. If `PMView` task, calls `DisableForm()`. |
| `Refresh` | `() (overrides UserControl.Refresh)` | Calls `GetParties()` — full refresh of the party list from business |
| `CancelClick` | `() As Integer` | Calls `CancelParty()`, sets status to `PMCancel`, calls `ProcessCommand()` |
| `UnLoadControl` | `(ByRef Cancel As Integer, ByRef UnloadMode As Integer) As Integer` | Query-unload — if not dismissed by form, calls `ProcessCommand()` |
| `Dispose` | `(implements IDisposable.Dispose)` | Disposes `g_oObjectManager`, `m_oBusiness`, `m_oFormFields`; clears `m_vPartyArray` |

#### Key Private Methods

| Method | Description |
|---|---|
| `SaveParty()` | Validates mandatory controls via `m_oFormFields.CheckMandatoryControls()`, calls `ValidateOK()`, then `ProcessCommand()` |
| `GetBusiness()` | Retrieves data from `m_oBusiness` into `m_vPartyArray` (calling business GetParties method) |
| `BusinessToInterface()` | Populates ListView columns: Name, Address, Phone, Licence Number, DOB, Gender, Status, Contact Name, Telephone, Party Type |
| `ProcessCommand()` | Routes by `m_lStatus`: `PMOK` → `SaveBusiness()`, `PMCancel` → cleanup |
| `DisableForm(lDisabled)` | Enables or disables all editable controls |

#### ListView Column Array Positions (iCLMPartyControl.vb constants)

`ACColPartyCnt=0`, `ACColFirstName=1`, `ACColLastName=2`, `ACColAddress1=3`, `ACColAddress2=4`, `ACColAddress3=5`, `ACColAddress4=6`, `ACColPostCode=7`, `ACColPhoneNumber=8`, `ACColLicenceNumber=9`, `ACColPartyTypeId=10`, `ACColPartyTypeDescription=11`

*Business Component:* **`bCLMClaimParty.Business`**

---

### 2. uctCLMCaseClaimList — Case/Claim List

**Files:** `uctCLMCaseClaimList\uctCLMCaseClaimList.vb` · `uctCLMCaseClaimList\iCLMCaseClaimList.vb`
**Class:** `Partial Public Class uctCLMCaseClaim` · Inherits `System.Windows.Forms.UserControl`

Displays a list of claims linked to a case. Provides buttons to open a claim, maintain a claim, pay a claim, salvage, and third party recovery — all via Navigator roadmaps. Also supports linking and unlinking claims to/from the case.

#### Public Events

| Event | Raised When |
|---|---|
| `EnabledChange()` | `Enabled` property is set |
| `LinkedOrUnlinked()` | After `LinkClaimToCase()` or `UnLinkClaimFromCase()` completes |
| `UnRecoverableError(Sender, EventArgs)` | An unrecoverable error occurs |

#### Public Properties

| Property | Type | Description |
|---|---|---|
| `CaseID` | `Integer` | Current case ID |
| `ClaimId` | `Integer` | Currently selected claim ID |
| `BaseCaseId` | `Integer` | Base case ID used for linking new claims |
| `PartyCnt` | `Integer` | WriteOnly — party count for key passing |
| `CaseNumber` | `String` | Case reference number string |
| `CaseProgressStatusCode` | `String` | Status code (e.g. `"CLOSED"`) used to lock UI |
| `Enabled` | `Boolean` | Shadows base; raises `EnabledChange()` event |
| `MinimumWidth` | `Integer` | Minimum control width (default 8250 twips) |
| `MinimumHeight` | `Integer` | Minimum control height (default 1995 twips) |

#### Public Methods

| Method | Signature | Description |
|---|---|---|
| `Initialise` | `() As Integer` | Creates `bObjectManager`, retrieves `bCLMCase.Business` instance, sets IDs. Thread-safe via `m_bIsInitialised`. |
| `SetProcessModes` | `(Optional vTask, vNavigate, vProcessMode, vTransactionType, vEffectiveDate) As Integer` | Sets all process mode properties |
| `Load_Renamed` | `() As Integer` | Configures ListView columns; calls `GetCaseClaimLink()`, `PopulateCaseClaimList()`, `GetCaseDetails()`, `SetInterfaceforCloseCase()` |
| `Save` | `() As Integer` | Opens transaction on `bCLMCase.Business`; calls `m_oBusiness.LinkClaims(v_lBaseCaseID, v_vLinkArray)` and `m_oBusiness.UnlinkClaims(v_vUnlinkArray)`; commits. Checks system options 5032/5033 for post-save document templates; calls `UseTheTemplate()` if required. |

#### Key Private Methods

| Method | Description |
|---|---|
| `OpenClaim()` | Starts `OPENCLM` navigator via `iPMNavStart.Interface_Renamed` passing key `base_case_id`; on `NavigatorClose` calls `LinkClaimToCase()` with returned `claim_cnt` |
| `MaintainClaim()` | Calls `CopyClaim(v_sTransactionType:="C_CR")`; starts `MAINCLM` navigator with keys: `claim_cnt`, `restart_step=1`, `insurancefile_cnt`, `claim_mode=2`, `claim_id` |
| `PayClaim()` | Calls `CopyClaim(v_sTransactionType:="C_CP")`; starts `PAYCLM` navigator |
| `OpenSalvage()` | Starts `SALVAGE` navigator with `claim_cnt` key |
| `OpenThirdPartyRecovery()` | Starts `TPRECOVER` navigator with `claim_cnt` key |
| `LinkClaimToCase()` | Calls `m_oBusiness.LinkClaims(...)` |
| `UnLinkClaimFromCase()` | Calls `m_oBusiness.UnlinkClaims(...)` |
| `GetCaseClaimLink()` | Retrieves case-claim linkage array from `m_oBusiness` |
| `PopulateCaseClaimList()` | Builds ListView from `m_vCaseClaimList` array |
| `CopyClaim(v_sTransactionType)` | Copies claim via `m_oBusiness` before edit/pay operations |
| `SetInterfaceforCloseCase()` | If `CaseProgressStatusCode = "CLOSED"`: disables all buttons |
| `GetTemplateType()` / `UseTheTemplate()` | Post-save document template handling (uses system options 5032/5033) |

#### Navigator Process Codes Launched (from iCLMCaseClaimList.vb)

| Constant | Value | Navigator Roadmap |
|---|---|---|
| `kRoadMapConstantOpenClaim` | `"OPENCLM"` | Open new claim linked to case |
| `kRoadMapConstantMaintainClaim` | `"MAINCLM"` | Edit existing claim |
| `kRoadMapConstantPayClaim` | `"PAYCLM"` | Pay a claim |
| `kRoadMapConstantSalvage` | `"SALVAGE"` | Salvage recovery |
| `kRoadMapConstantTPRecovery` | `"TPRECOVER"` | Third party recovery |

*Business Component:* **`bCLMCase.Business`**

---

### 3. uctCLMCaseHeader — Case Header

**Files:** `uctCLMCaseHeader\uctCLMCaseHeader.vb` · `uctCLMCaseHeader\uctCLMCaseHeaderMod.vb`
**Class:** `Public Partial Class uctCLMCaseHeader` · Inherits `System.Windows.Forms.UserControl`

Displays and edits the header fields of a case: case number, opened date, progress status, analyst, assistant, and version. Validates the case before saving.

#### Public Events

| Event | Raised When |
|---|---|
| `EnabledChange()` | `Enabled` shadow property is set |

#### Public Properties

| Property | Type | Description |
|---|---|---|
| `CaseID` | `Integer` | Sets internal `m_lCaseID`; resets `m_bHasChanged = False` |
| `CaseNumber` | `String` | Read/Write — also updates `txtCaseNumber.Text` |
| `CaseOpenedDate` | `Date` | Opened date |
| `CaseProgressStatusID` | `Integer` | Progress status dropdown selection ID |
| `CaseProgressStatusCode` | `String` | Progress status code (e.g. `"CLOSED"` locks analyst/date/assistant) |
| `CaseAssistantID` | `Integer` | Assistant combo selection ID |
| `CaseVersion` | `Integer` | Version number |
| `BaseCaseID` | `Integer` | Base case ID |
| `ClaimID` | `Integer` | Claim ID (used in `GenerateCaseCode`) |
| `Task` | `Integer` | `PMAdd` / `PMEdit` / `PMView` |
| `HasChanged` | `Boolean` | ReadOnly — `True` if user modified any field |
| `ValidCase` | `Boolean` | ReadOnly — calls `ValidateForm()`; `True` if all mandatory fields pass |
| `Enabled` | `Boolean` | Shadows base; raises `EnabledChange()` |
| `MinimumWidth` | `Integer` | Min control width |
| `MinimumHeight` | `Integer` | Min control height |

#### Public Methods

| Method | Signature | Description |
|---|---|---|
| `Initialise` | `() As Integer` | Creates `bObjectManager`, retrieves `bCLMCase.Business` instance; calls `DisplayCaptions()` and `SetupFormLayout()` |
| `Load_Renamed` | `() As Integer` | Calls `SetupFormLayout()` then `GetCaseDetails()` then `DataToInterface()` |
| `Save` | `() As Integer` | Calls `InterfaceToData()` to read form fields; calls `m_oBusiness.SaveCase(v_lCaseID, v_sCaseNumber, v_dtCaseOpenedDate, v_lCaseProgressStatusID, v_lCaseAnalystID, v_lCaseAssistantID, v_lCaseVersion, r_lNewCaseID, r_lBaseCaseID)`; then `m_oBusiness.CreateEvent(v_lCaseID, "CASES", DateTime.Today, sDescription)` |

#### Key Private Methods

| Method | Description |
|---|---|
| `GetCaseDetails()` | `PMAdd`: calls `m_oBusiness.GenerateCaseCode(r_sCaseNumber, m_lClaimID)`. Edit/View: calls `m_oBusiness.LoadCase(v_lCaseID, r_sCaseNumber, r_dtCaseOpenedDate, r_lCaseProgressStatusID, r_lCaseAnalystID, r_lCaseAssistantID, r_lCaseVersion, r_lBaseCaseID)` |
| `DataToInterface()` | Populates form fields from member variables. If `cboCaseProgressStatus.ItemCode = "CLOSED"`: disables `cboAnalyst`, `cboCaseOpenDate`, `cboAssistant` with warning message |
| `InterfaceToData()` | Reads form controls back into private member variables |
| `ValidateForm()` | Validates: `CaseNumber` not empty; `CaseOpenedDate` not in future; `CaseProgressStatus ≠ -1`; `CaseAssistant ≠ -1`; `CaseAnalyst ≠ -1` |
| `DisplayCaptions()` | Loads localised captions from resource file into all labels |
| `SetupFormLayout()` | Resets all controls to blank/default values |

*Business Component:* **`bCLMCase.Business`** — methods called: `GenerateCaseCode`, `LoadCase`, `SaveCase`, `CreateEvent`

---

### 4. uctCLMListPayments — Payments List View

**Files:** `uctCLMListPayments\uctCLMListPayments.vb` · `uctCLMListPayments\iCLMListPayments.vb`
**Class:** `Partial Public Class uctCLMListPaymentsC` · Inherits `System.Windows.Forms.UserControl`

Read-only view of all payments on a claim. Displays payment date, payee, amount (net + tax), currency, loss/base amounts, bank details, status and media reference. Optional "View" button to drill into a payment record.

#### Public Events

| Event | Raised When |
|---|---|
| `InitialisedChange()` | `Initialised` property changes |
| `ClaimIDChange()` | `ClaimId` or `SalvageAndTPRecoveryReceipts` is set |
| `ReserveIDChange()` | `ReserveID` is set |
| `visibleCmdViewChange()` | `visibleCmdView` is set |

#### Public Properties

| Property | Type | Description |
|---|---|---|
| `ClaimId` | `Integer` | Claim ID; triggers reload via `ClaimIDChange` event |
| `ReserveID` | `Integer` | WriteOnly — filters list to a specific reserve |
| `selectedItem` | `Integer` | Gets/sets the selected item (payment ID) |
| `CountColumn` | `Integer` | Number of ListView columns; resizes `m_iColumn` array |
| `ColumnCaption(index)` | `String` | Caption for a specific column header |
| `ShowPaymentView` | `Boolean` | Shows or hides the "View Payment" button |
| `visibleCmdView` | `Boolean` | Shows/hides `cmdViewPayment`; adjusts `lvwPayments` height |

#### Public Methods

| Method | Signature | Description |
|---|---|---|
| `GetBusiness` | `() As Integer` | Calls `Initialise()` if needed; calls `m_oBusiness.GetPaymentList(lClaimId, lReserveID, r_vPaymentList)`; calls `GetProductDetails()`; calls `BusinessToInterface()`; hides payment ID column (col 0) |
| `BusinessToInterface` | `() As Integer` | Clears `lvwPayments`; iterates `m_vPaymentList` array rows; for each row adds: date, resolved payee name, payee, net amount (Amount − TaxAmount), tax amount, currency, loss amount, base amount, bank name, account, sort code, BIC, IBAN, status, media ref |

#### Payment List Array Column Positions

| Pos | Column | Description |
|---|---|---|
| 0 | `PaymentID` | Hidden, used for selection |
| 1 | `Date` | Payment date |
| 2 | `ResolvedName` | Resolved payee name |
| 3 | `Payee` | Payee |
| 4 | `Amount` | Gross amount |
| 5 | `Currency` | Currency code |
| 6 | `LossAmount` | Amount in loss currency |
| 7 | `BaseAmount` | Amount in base currency |
| 8–10 | `*CurrencyID` | Payment/Loss/Base currency IDs |
| 11 | `TaxAmount` | Tax portion |
| 12 | `ClaimPerilId` | |
| 13 | `MediaRef` | Cheque/BACS reference |
| 14–17 | `BankName/AccountNo/Code/Status` | Bank details |
| 18 | `BIC` | SWIFT BIC code |
| 19 | `IBAN` | IBAN |

*Business Components:* **`bCLMPeril.Business`** (GetPaymentList), **`bACTCurrencyConvert.Form`** (currency formatting)

---

### 5. uctCLMListReceipts — Receipts List View

**Files:** `uctCLMListReceipts\uctCLMListReceipts.vb` · `uctCLMListReceipts\iCLMListReceipts.vb`
**Class:** `Public Partial Class uctCLMListReceiptsC` · Inherits `System.Windows.Forms.UserControl`

Read-only list of receipts (recoveries) on a claim. Mirrors the payments list structure. Supports filtering by recovery type and recovery ID for salvage/TP recovery views.

#### Public Events

| Event | Raised When |
|---|---|
| `InitialisedChange()` | `Initialised` changes |
| `RecoveryTypeChange()` | `RecoveryType` set |
| `RecoveryIDChange()` | `RecoveryID` set |
| `ClaimIDChange()` | `ClaimId` or `SalvageAndTPRecoveryReceipts` set |
| `visibleCmdViewChange()` | `visibleCmdView` set |

#### Public Properties

| Property | Type | Description |
|---|---|---|
| `ClaimId` | `Integer` | Claim ID for retrieval |
| `SalvageAndTPRecoveryReceipts` | `Integer` | Flag for recovery-type filtering (0 = all) |
| `RecoveryID` | `Integer` | WriteOnly — recovery ID filter |
| `RecoveryType` | `Integer` | Recovery type filter |
| `selectedItem` | `Integer` | Selected receipt ID |
| `CountColumn` | `Integer` | Number of list columns |
| `ColumnCaption(Index)` | `String` | Column header captions |
| `visibleCmdView` | `Boolean` | Shows/hides `cmdViewReceipts` button |

#### Public Methods

| Method | Signature | Description |
|---|---|---|
| `GetBusiness` | `() As Integer` | Calls `Initialise()` if needed; calls `m_oBusiness.GetReceiptList(lClaimId, vRecoveryType, r_vReceiptList, lRecoveryID, nSalvageAndTPRecoveryReceipts)`; calls `BusinessToInterface()`; hides receipt ID column |
| `BusinessToInterface` | `() As Integer` | Clears `lvwReceipts`; iterates receipt array; adds: date, resolved name, payee, formatted amount, tax amount, currency, loss amount, base amount |

#### Receipt List Array Column Positions

| Pos | Column |
|---|---|
| 0 | `ReceiptID` (hidden) |
| 1 | `Date` |
| 2 | `ResolvedName` |
| 3 | `Payee` |
| 4 | `Amount` |
| 5 | `Currency` |
| 6 | `LossAmount` |
| 7 | `BaseAmount` |
| 8–10 | `Receipt/Loss/BaseCurrencyID` |
| 11 | `TaxAmount` |

*Business Components:* **`bCLMPeril.Business`** (GetReceiptList), **`bACTCurrencyConvert.Form`** (currency formatting)

---
### 6. uctCLMListVersionControl — Claim Version History

**Files:** `uctCLMListVersionControl\uctCLMListVersionControl.vb` · `uctCLMListVersionControl\MainModule.vb` · `uctCLMListVersionControl\cClaimDetails.vb`
**Class:** `Partial Public Class uctCLMVersions` · Inherits `System.Windows.Forms.UserControl`

Displays a TreeView of claims (grouped by status: Open, Info-Only, Settled) with a ListView of all versions for the selected claim. Allows navigating claim versions, viewing case details, and switching to claims from other systems via the Client Manager.

#### Data Model Class: cClaimDetails

Simple container for one tree node's data. **Properties:** `ClaimVersionDetails As Object` (2D array of version rows), `ClaimDescription As String`, `InsuranceRef As String`, `CaseNumber As String`.

#### Public Events

| Event | Description |
|---|---|
| `DblClick(Sender, EventArgs)` | User double-clicks a claim version row in the ListView |

#### Public Properties

| Property | Type | Access | Description |
|---|---|---|---|
| `ClaimNumber` | `String` | WriteOnly | Sets claim number search filter |
| `ShortName` | `String` | WriteOnly | Client short name filter |
| `InsuranceRef` | `String` | WriteOnly | Insurance reference filter |
| `ClaimId` | `Integer` | WriteOnly | Claim ID to select in the tree |
| `SelectedClaimId` | `Integer` | Read/Write | Currently selected claim's claim ID |
| `PartyCnt` | `Integer` | WriteOnly | Party count (passed in key array for navigation) |

#### Public Methods

| Method | Signature | Description |
|---|---|---|
| `Initialise` | `() As Integer` | Creates `bObjectManager`, initialises `m_colClaimVersionDetails As Collection`, retrieves `bCLMFindClaim.Business` and `bCLMCase.Business` instances. Thread-safe via Static `bIsInitialised`. |
| `Load_Renamed` | `() As Integer` | Calls `SetUpUserControl()` which builds TreeView and loads claim list |
| `SetProcessModes` | `(Optional vTask, vNavigate, vProcessMode, vTransactionType, vEffectiveDate) As Integer` | Sets all process mode values |
| `LoadClaimDetails` | `() As Integer` | Calls `bCLMFindClaim.Business` to get paginated claim/version list; populates TreeView nodes and `m_colClaimVersionDetails` collection |
| `GetSelectedClaimsDetails` | `(Optional ByRef r_lClaimId, r_lInsuranceFileCnt, r_sClaimNumber, r_sInsuranceRef, r_lRiskCnt, r_sClientShortname, r_dtLossFromDate, r_sInsuranceHolderShortname, r_lInsuranceFolderCnt, r_bRecovery...) As Integer` | Returns all details of the selected version via `ByRef` parameters |
| `SetSelectedItemsDetails` | `(ByVal oItem As ListViewItem) As Integer` | Reads selected ListView row into member variables: `m_lSelectedClaimID`, `m_lSelectedInsuranceFileCnt`, etc. |
| `ClearSelectedItemsDetails` | `() As Integer` | Resets all selected-item member variables to defaults |

#### Key Private Methods

| Method | Description |
|---|---|
| `SetUpUserControl()` | Configures TreeView and ListView column widths; calls `LoadClaimDetails()` |
| `cmdViewCase_Click` | Gets case number from `txtCaseNumber`; calls `m_oBusinessCase.GenerateSQL(r_sSQL, v_sCaseNumber)` then `m_oBusinessCase.FindCase(v_sSQL, r_vResultArray)` then `m_oBusinessCase.GetPreviousDataModel(v_lCaseId, r_lPreviousDataModelId, r_lGISPolicyLinkID)`. If previous data model found, prompts user; on confirmation calls `m_oBusinessCase.DeleteCustomData(v_lGISPolicyLinkID)`. Then calls `ShowCaseScreen(v_lTask=PMView, "C_VC", v_lCaseID, v_lBaseCaseID)` |
| `tvwClaims_DoubleClick` | Detects if node is "other claim" (from another system); if so, calls `iPMBClientManagerWrapper.Interface_Renamed` to switch active client in Client Manager |
| `lvwClaimVersions_DoubleClick` | Calls `SetSelectedItemsDetails(oItem)` then raises `DblClick` event |
| `ResizeControl()` | Repositions TreeView, claim info frame, version list frame, and ListView on control resize |

#### TreeView Node Keys (MainModule.vb constants)

| Constant | Value | Meaning |
|---|---|---|
| `ktvwNodeKeyREALROOT` | `"REALROOT"` | Invisible root node |
| `ktvwNodeKeyALL` | `"ALL"` | All claims node |
| `ktvwNodeKeyOPEN` | `"OPEN"` | Open claims group |
| `ktvwNodeKeyINFOONLY` | `"INFOONLY"` | Info-only claims group |
| `ktvwNodeKeySETTLED` | `"SETTLED"` | Settled claims group |
| `ktvwNodeTagClaimNode` | `"CLAIM"` | Tag on individual claim nodes |
| `kClaimStatusIdClosed` | `3` | Status ID for closed claim |
| `kClaimStatusIdReClosed` | `5` | Status ID for re-closed claim |

#### Claim Version Details Array Positions

Positions 0–24: `ClaimId`, `VersionId`, `CreateDate`, `TransactionType`, `VersionDescription`, `TotalIncurred`, `TotalPaid`, `ThisReserveRevision`, `ThisReservePayment`, `ThisSalvageRecovery`, `ThisThirdPartyRecovery`, `CurrentReserve`, `InsuranceFileCurrency`, `ClaimCurrency`, `CreatedBy`, `ClaimDescription`, `InsuranceRef`, `InsuranceFileCnt`, `ClaimNumber`, `RiskCnt`, `ClientShortName`, `LossFromDate`, `InsuranceHolderShortname`, `InsuranceFolderCnt`, `TransactionTypeCode`

*Business Components:* **`bCLMFindClaim.Business`** (find/load claim list), **`bCLMCase.Business`** (GenerateSQL, FindCase, GetPreviousDataModel, DeleteCustomData, ShowCaseScreen)

---

### 7. uctCLMPayment — Payment Entry

**Files:** `uctCLMPayment\uctCLMPayment.vb` · `uctCLMPayment\iCLMPaymentControl.vb` · `uctCLMPayment\cPaymentItem.vb` · `uctCLMPayment\cTaxParameters.vb`
**Class:** `Partial Public Class uctCLMPayment` · Inherits `System.Windows.Forms.UserControl` · Implements `IDisposable`

The payment entry control. Displays a reserve-level payment grid with editable "This Payment" amounts, calculates tax automatically per reserve line, and delegates save operations to the host-provided business object. Used in the Maintain Claim and Pay Claim screens.

#### Data Model Class: cPaymentItem

Per-reserve payment item holding calculation state. Key properties: `ThisPayment As Decimal`, `TaxAmount As Decimal`, `TaxAmountWHT As Decimal`, `CurrencyId As Integer`, `TaxGroupId As Integer`, `ReserveId As Integer`, `IsWithHoldingTax As Boolean`, `IsExcess As Boolean`, `TotalReserve/PaidToDate/Balance As Decimal`, `ExchangeRateOverrideReasonId As Integer`, `CurrencyToBaseXRate/Date`, `AccountToBaseXRate/Date`, `SystemToBaseXRate/Date`, `PaymentToLossXRate As Double`, `TaxBandRateArray As Object`, `AdvancedTaxScript As String`, `ThisRevisionInLossCurrency As Decimal` (ReadOnly — calls `GetRevisionAmount()`).

#### Public Events

| Event | Description |
|---|---|
| `VisibleChange()` | `Visible_Renamed` property set |
| `ShowEditChange()` | `ShowEdit` property set |
| `EnabledChange()` | `Enabled` property set |
| `DataHasChanged(Sender, DataHasChangedEventArgs)` | Payment data in ListView changed |

#### Public Structures

```vb
Public Structure udtDetails
    lPaymentID As Integer
    cPaidToDate As Decimal
    cPaymentAmount As Decimal
    lReserveID As Integer
    sReserveTypeDesc As String
End Structure

Public Structure udtReserveDetails
    lReserveID As Integer
    lPaymentID As Integer
    cTotalReserve As Decimal
    cInitialReserve As Decimal
End Structure
```

#### Public Properties

| Property | Type | Description |
|---|---|---|
| `Enabled` | `Boolean` | Enables/disables `fraPaymentDetails` and `lstviewPayment` |
| `Text` | `Object` | ReadOnly — returns full 2D payment array from ListView (col 0 = payment ID, col 1 = description, cols 2+ = currency amounts) |
| `ShowEdit` | `Boolean` | Shows/hides the `cmdEdit` button |
| `Visible_Renamed` | `Boolean` | Shows/hides `fraPaymentDetails` |
| `Insurance_File_Cnt` | `Integer` | ReadOnly — from `g_lInsurance_file_cnt` |
| `PerilID` | `Integer` | ReadOnly |
| `ClaimId` | `Integer` | ReadOnly |
| `PerilTypeID` | `Integer` | ReadOnly |
| `RiskID` | `Integer` | ReadOnly |

#### Public Methods

| Method | Signature | Description |
|---|---|---|
| `Initialise` | `() As Integer` | Creates `bObjectManager`, completes object initialisation |
| `SetProcessModes` | `(Optional vTask, vNavigate, vProcessMode, vTransactionType, vEffectiveDate) As Integer` | Sets all process mode values |
| `LoadControl` | `() As Integer` | Called after `Initialise` + `SetProcessModes`; configures form layout per task |
| `GetDetails` | `(Optional lPerilID, lPerilTypeID, lClaimID, lRiskID, lInsurance_File_Cnt) As Integer` | Loads reserve details from business for the peril into `m_vReserveDetails` array |
| `GetPaymentDetails` | `() As Integer` | Retrieves existing payment detail array from business |
| `Save` | `() As Integer` | Validates; calls `GetPaymentPartyid()` to select payee party; calls `m_oBusiness.UpdateReserveDetails(vReserves)` then `m_oBusiness.SavePaymentDetails(vPayArray)` |
| `GetPaymentPartyid` | `(ByRef lPaymentPartyId, ByRef sOComments, ByRef lButtonClicked, Optional sIComments, Optional iCurrencyID) As Integer` | Shows party selection dialog; returns party ID and comments via `ByRef` |
| `UpdateReserveValue` | `(ByRef uNewValues As udtReserveDetails) As Integer` | Finds reserve in `m_vReserveDetails` by ID; calls `UpdateCurrentPayment(iTemp+3, cTotalReserve, cInitialReserve)` if amounts changed |
| `Dispose` | `() Sub (IDisposable)` | Disposes `g_oObjectManager`, `m_oBusiness`, `m_oPaymentDetailArray` |

#### Key Private Methods

| Method | Description |
|---|---|
| `SaveReserveDetails()` | Builds reserve save array from ListView. For `C_CO` posts initial reserve. For `C_CR` posts revision. For `C_CP` posts payment. Blocks if `m_lAllowNegativeReserve = False` and new reserve < 0. Calls `m_oBusiness.UpdateReserveDetails(vReserves)` |
| `SetPayeeDetails(lPartyCnt, sComments)` | Sets party ID and comments on every item in `m_vPaymentDetailArray` |
| `CalcAverage(iIndex)` | Calculates `(initial + revision) / sumInsured * 100` |
| `UpdateCurrentPayment(iCol, cTotal, cInitial)` | Updates a ListView row with new reserve calculation amounts |

#### Payment Details Array Positions (iCLMPaymentControl.vb, `kClaimPaytDet*`)

| Pos | Name | Pos | Name |
|---|---|---|---|
| 0 | `WorkClaimPaymentId` | 28 | `IsTaxExempt` |
| 1 | `ClaimPerilId` | 29 | `IsWHTExempt` |
| 2 | `DateOfPayment` | 30 | `IsSettlement` |
| 3 | `Amount` | 31 | `DocumentId` |
| 4 | `TaxAmount` | 32 | `PartyShortname` |
| 5 | `PartyCnt` | 33 | `ClaimPayable` |
| 6 | `Comments` | 34 | `Party` |
| 7 | `IsReferred` | 35 | `Agent` |
| 8 | `CreatedBy` | 36 | `Client` |
| 9 | `PayeeMediaType` | 37 | `MediaRef` |
| 10 | `PayeeName` | 38 | `ClaimPaymentToDesc` |
| 11 | `PayeeBankName` | 39 | `SafeHarbourDesc` |
| 12 | `PayeeSortCode` | 40 | `MediaTypeDesc` |
| 13 | `PayeeAccountNo` | 41 | `CountryDesc` |
| 14 | `PayeeCountry` | 42 | `ExcessAmount` |
| 15 | `PayeeComments` | 43–47 | Address lines 1–5 |
| 16 | `SequenceNo` | 48 | `ThirdPartyReference` |
| 17 | `TreatyId` | 49 | `ChequeDate` |
| 18 | `ClaimPaymentToId` | 50 | `BankPaymentTypeId` |
| 19 | `PaymentPartyTo` | 51 | `OurReference` |
| 20 | `InsuredDomiciled` | 52 | `IsExGratia` |
| 21 | `InsuredPercentage` | 53 | `BIC` |
| 22 | `InsuredTaxNumber` | 54 | `IBAN` |
| 26 | `SafeHarbourId` | | |
| 27 | `SafeHarbourPercentage` | | |

#### Account Code Constants

| Constant | Value | Purpose |
|---|---|---|
| `kAccountCLMPAYABLE` | `"CLMPAYABLE"` | Claims payable ledger account |
| `kAccountCLMRECEIVABLE` | `"CLMRECEIVABLE"` | Claims receivable ledger account |
| `kAccountCLMParty` | `"PARTY"` | Payment to parties |
| `kAccountCLMClient` | `"CLIENT"` | Payment to client |
| `kAccountCLMAgent` | `"AGENT"` | Payment to agent |
| `kAccountCLMInsurer` | `"INSURER"` | Payment from insurer |

#### System Option References

| Constant | Value | Purpose |
|---|---|---|
| `kSysOptionDefaultClaimPayment` | `2002` | Default claim payment party type |
| `kSysOptionATSSattlement` | `5071` | ATS/ATSSettlement system option |

*Business Component:* Host-provided via `g_oObjectManager.GetInstance` — typically `bCLMPeril.Business` or `bCLMPayment.Business`

---

### 8. uctCLMPerilRT — Peril / Risk Type

**Files:** `uctCLMPerilRT\uctCLMPerilRT.vb` · `uctCLMPerilRT\Main.vb`
**Class:** `Partial Public Class uctCLMPerilRT` · Inherits `System.Windows.Forms.UserControl`

Displays and manages the list of perils on a claim. Supports add/edit/delete of claim perils with different behaviour for Agency vs Underwriting entry paths. Raises events carrying key arrays to the hosting form for GIS screen navigation.

#### Public Events

| Event | Description |
|---|---|
| `PerilListChanged(Sender, EventArgs)` | After any peril add/edit/delete operation |
| `AddClick(Sender, AddClickEventArgs)` | Peril added; `AddClickEventArgs` carries key array |
| `EditClick(Sender, EditClickEventArgs)` | Peril edit initiated; carries key array with GIS screen info |
| `DeleteClick(Sender, DeleteClickEventArgs)` | Peril deleted; carries key array |
| `OnControlGotFocus(Sender, OnControlGotFocusEventArgs)` | Control receives focus |

#### Public Properties

| Property | Type | Description |
|---|---|---|
| `ScreenCaption` | `String` | Caption shown on LOA003 (insured name / claim number) |
| `Status` | `Integer` | Exit status after last operation |
| `PerilCount` | `Integer` | ReadOnly — `lvwPerils.Items.Count` |
| `Policy` | `Integer` | Policy (insurance file count) ID |
| `Risk` | `Integer` | Risk ID |
| `Claimid` | `Integer` | Claim ID |
| `ClaimMode` | `Integer` | WriteOnly — 1=OpenClaim, 2=EditMode |
| `ViewRiskFlag` | `Boolean` | If `True`, enables risk viewing button |
| `IsOpenClaimNoTrans` | `Boolean` | Open Claim with No Transaction flag |

#### Public Methods

| Method | Signature | Description |
|---|---|---|
| `Initialise` | `() As Integer` | Creates business objects, sets language/source/user IDs, calls `LoadPerilData()` |
| `LoadControl` | `() As Integer` | Called after `Initialise`; checks `CheckIsLegacyClaim()` (UW path only); configures ListView columns; calls `LoadPerilData()` |
| `SetProcessModes` | `(Optional vTask, vNavigate, vProcessMode, vTransactionType, vEffectiveDate) As Integer` | Sets all process mode values |

#### Key Private Methods

| Method | Description |
|---|---|
| `cmdPerilAdd_Click` | Calls `GetPerilTypes(vDataArray)` on business; shows `frmAddPeril` dialog with peril type dropdown. On confirm: calls `AddClaimPeril(v_iPerilTypeId, r_lPerilID, v_sDescription)` on business; calls `LoadPerilData(1)`. For Underwriting: builds key array with `GIS_Screen_id`, `WorkClaimPerilID`, `LossSchedule`, `PerilTypeId`, `LossScheduleTypeId`, `NoTransaction`; raises `PerilListChanged` + `AddClick`. |
| `cmdPerilEdit_Click` | Builds 18-element key array: `ClaimPerilID`, `RealClaimID`, `risk_type`, `InsFileCnt`, `risk_id`, `claim_mode`, `GIS_Screen_id`, `InsuranceFolderCnt`, `work_claim_peril_id`, `claim_transaction_type`, loss schedule, no-trans, screen caption. For Agency: raises `EditClick`. For Underwriting: opens GIS screen via key array navigation. |
| `cmdPerilDelete_Click` → `DeletePeril()` | Calls `m_oBusiness.DeleteClaimPeril(v_lClaimPerilId)` |
| `LoadPerilData(Optional iCalledFrom)` | Retrieves peril list from `m_oBusiness`; populates `lvwPerils` with: risk description, peril description, sum insured, incurred, paid, recoveries, salvage, current reserve, GIS screen ID, original peril ID, loss schedule type, peril type ID, policy currency, loss currency |
| `AddClaimPeril(v_iPerilTypeId, r_lPerilID, v_sDescription)` | Calls `m_oBusiness.AddClaimPeril(...)` on business object |
| `GetPerilTypes(vDataArray)` | Gets available peril types for the risk from business |
| `SetPerilButtons()` | Enables/disables Add/Edit/Delete buttons based on claim mode and selection state |
| `CheckIsLegacyClaim()` | Checks if claim pre-dates claims builder (UW entry path) — blocks edit of pre-builder claims |
| `GetIsPaymentsReadOnly()` | Reads product maintenance config for read-only payment flag |

#### ListView Column Positions (Main.vb constants)

| Constant | Value | ListView Column |
|---|---|---|
| `kColHeaderRiskDescription` | 1 | Risk type description |
| `kolHeaderPerilDescription` | 2 | Peril description |
| `kColHeaderSumInsured` | 3 | Sum insured |
| `kColHeaderIncurred` | 4 | Total incurred |
| `kColHeaderPaid` | 5 | Total paid |
| `kColHeaderRecoveries` | 6 | Total recoveries |
| `kColHeaderSalvage` | 7 | Salvage |
| `kColHeaderCurrentReserve` | 8 | Current reserve |
| `kColHeaderPolicyCurrency` | 14 | Policy currency |
| `kColHeaderLossCurrency` | 15 | Loss currency |

*Business Component:* **`bCLMPeril.Business`** — methods called: `GetPerilTypes`, `AddClaimPeril`, `DeleteClaimPeril`, `LoadPerilList`

---

### 9. uctCLMReceipt — Receipt / Recovery Entry

**Files:** `uctCLMReceipt\uctCLMReceipt.vb` · `uctCLMReceipt\iCLMReceiptControl.vb` · `uctCLMReceipt\cReceiptItem.vb` · `uctCLMReceipt\cTaxParameters.vb`
**Class:** `Partial Public Class uctCLMReceipt` · Inherits `System.Windows.Forms.UserControl`

The recovery/receipt entry control. Manages receipts (incoming money: salvage, third party recovery, coinsurance recovery, reinsurance recovery). Displays recovery lines per peril, handles tax calculations, coinsurer splits, reinsurance splits, and payee party selection. Posts cash list entries to Orion on save.

#### Data Model Class: cReceiptItem

Per-recovery receipt item. Key properties: `RecoveryTypeId As Integer`, `RecoveryPartyTypeId As Integer`, `RecoveryPartyCnt As Integer`, `WorkClaimReceiptItemId As Integer`, `ThisNet As Decimal` (ReadOnly — `m_crThisReceipt - m_crTaxAmount`), `ThisRevisionInLossCurrency As Decimal` (ReadOnly), `TaxAmount As Decimal`, `CurrencyId/Code/Description`, `TaxGroupId/Description`, `TotalReserve/ReceivedToDate/Balance As Decimal`, `AdvancedTaxScript As String`, `TaxBandRateArray As Object`, `ScriptedTaxAmount As Decimal`. Exchange rates: `CurrencyToBaseXRate/Date`, `AccountToBaseXRate/Date`, `SystemToBaseXRate/Date`, `ReceiptToLossXRate As Double`.

#### Public Events

| Event | Description |
|---|---|
| `UnRecoverableError(Sender, EventArgs)` | Fatal error in receipt processing |

#### Public Properties

| Property | Type | Description |
|---|---|---|
| `ClaimID` | `Integer` | Read/Write — claim being worked on |
| `ClaimPerilId` | `Integer` | Read/Write — peril for this receipt |
| `ClaimPaymentId` | `Integer` | WriteOnly |
| `ClaimReceiptId` | `Integer` | WriteOnly (for review mode) |
| `ClaimNumber` | `String` | ReadOnly |
| `RecoveryMode` | `Integer` | WriteOnly — if `kRecoveryModeSalvageReceipt` then `m_bIsSalvage=True` |
| `ReceiptMade` | `Boolean` | ReadOnly — True if a receipt was saved in this session |

#### Public Methods

| Method | Signature | Description |
|---|---|---|
| `Initialise` | `() As Integer` | Creates `bObjectManager`; retrieves `bCLMPeril.Business`, `bACTCurrencyConvert.Form`, `bCLMPaymentMethod.Business` instances |
| `SetProcessModes` | `(Optional vTask, vNavigate, vProcessMode, vTransactionType, vEffectiveDate) As Integer` | Sets all process mode values |
| `Load_Renamed` | `() As Integer` | Full load: `GetSystemOptions()`, `GetProductOptions()`, `RetrieveSingleSystemOption(kSIROPTReceiptExcludeTax)`, `GetClaimDetails()`, `PopulateClaimDetails()`, `SetupTaxesListView()`, `GetLookups()`, `PopulateLookups()`. For new receipt: `GetCurrentRecoveryDetails()` → `SetupReceiptDetailsListView()` → `PopulateReceiptDetailsListView()`. For review: `GetClaimReceiptItemDetails()` → `SetupReceiptItemDetailsListView()` → `PopulateReceiptItemDetailsListView()` → `GetClaimReceiptItemTaxDetails()` → `PopulateThisReceiptDetails()` → `GetClaimReceiptDetails()` → `PopulateClaimReceiptDetails()`. Then: `GetCoinsurance()`, `SetupInsurerListView()`, `PopulateInsurerCollection()`. For UW also `GetReinsurance()`. Finally `SetUpUserControl()`. |
| `GetLookups` | `() As Integer` | Gets: media type, country, currency, tax groups, tax bands, class of business, claim payment-to options, safe harbour |
| `Save` | `() As Integer` | Validates mandatory fields; creates `cReceiptItem` for each recovery line; calls `m_oBusiness.SaveReceipt(...)` within a transaction. Handles reinsurance/coinsurance splits. Creates WTA settlement if `kSysOptionATSSattlement` system option set. Creates cash list entries via Orion. |
| `GetCashListDetails` | `(ByRef r_lAccountId, ByRef r_lPartyId, ByRef r_crCashListAmount) As Integer` | Returns cash list account ID, party ID, and total amount via `ByRef` (for Orion cash list posting by the calling form) |
| `CheckChangeOfParty` | `(ByVal lPartyTypeId As Integer) As Integer` | If payee party type has changed, prompts user to clear existing selection |
| `RemoveAttachedParty` | `(ByVal lRecoveryId As Integer) As Integer` | Detaches party from a recovery via business object |
| `SetNewlyAttachedParty` | `(ByVal lPartyId, sShortName, sLongName, lRecoveryId, iPartyTypeId) As Integer` | Attaches a new party to a recovery via business object |
| `SetupDefaultInsuredTaxStatus` | `() As Integer` | Defaults insured tax domicile/percentage from claim details |
| `EnableDisablePayeeDetails` | `(ByVal v_bEnabled As Boolean) As Integer` | Enables/disables payee detail fields |
| `GetDefaultTaxItem` | `(ByRef r_oTaxItem As cTaxParameters) As Integer` | Returns default tax parameters for the current claim/peril |
| `ValidateReciept` | `() As Integer` | Validates mandatory receipt fields (note: method name has spelling error) |

#### Key Private Methods

| Method | Description |
|---|---|
| `CheckMandatory()` | Validates `txtParty.Text` (payee name); if advanced-tax scripting option is set, also validates insured percentage |
| `GetClaimDetails()` | Calls `m_oBusiness.GetClaimDetails(v_lClaimId, ...)` returning `m_vClaimDetails` 2D array |
| `GetCurrentRecoveryDetails()` | Calls `m_oBusiness.GetAllRecoveries(v_lClaimId, v_lClaimPerilId, r_vRecovery)` returning `m_vRecoveryDetails` |
| `GetCoinsurance()` | Gets coinsurer split array from business |
| `GetReinsurance()` | Gets reinsurer split array from business (UW path) |
| `PopulateReceiptDetailsListView()` | Builds recovery details ListView with: totals, balance, editable "this receipt" amount column |
| `PopulateInsurerCollection()` | Builds coinsurer/reinsurer collection used for tax sharing calculations |

#### Recovery Details Array Positions (iCLMReceiptControl.vb, `kRecoveryDetails*`)

| Pos | Column |
|---|---|
| 0 | `RecoveryId` |
| 1 | `ClaimPerilId` |
| 2 | `RecoveryTypeId` |
| 3 | `RecoveryTypeDescription` |
| 4 | `CurrencyId` |
| 5 | `CurrencyDescription` |
| 6 | `InitialReserve` |
| 7 | `RevisedReserve` |
| 8 | `ReceivedToDate` |
| 9 | `RevisionCount` |
| 10 | `TaxAmount` |
| 11 | `ClaimId` |
| 12 | `ClaimsIsPostTaxes` |

#### Tab Constants

| Constant | Value | Tab |
|---|---|---|
| `kTabRecovery` | 0 | Recovery details |
| `kTabCoinsurance` | 1 | Coinsurance |
| `kTabReinsurance` | 2 | Reinsurance |
| `kTabThisReceipt` | 3 | This receipt |

#### Payee Party Type Options

| Constant | Value |
|---|---|
| `kPayeeOptClaimReceivable` | 1 |
| `kPayeeOptParty` | 2 |
| `kPayeeOptAgent` | 4 |
| `kPayeeOptClient` | 8 |

*Business Components:* **`bCLMPeril.Business`** (GetClaimDetails, GetAllRecoveries, GetCoinsurance, GetReinsurance, SaveReceipt), **`bACTCurrencyConvert.Form`** (currency formatting), **`bCLMPaymentMethod.Business`** (payment method lookup)

---

### 10. uctCLMReserve — Reserve Management

**Files:** `uctCLMReserve\uctCLMReserve.vb` · `uctCLMReserve\iCLMReserveControl.vb`
**Class:** `Partial Public Class uctCLMReserve` · Inherits `System.Windows.Forms.UserControl` · Implements `IDisposable`

Displays and edits reserve amounts per reserve type on a claim peril. Supports initial reserve (C_CO), reserve revision (C_CR), and payment (C_CP) transaction types. Handles currency rate overrides. Optionally posts reserve transactions to Orion via `bControlTransClaims.Automated`.

**Utility Function (iCLMReserveControl.vb):**
- `IsValidCurrency(ByRef cValue As String) As Integer` — Validates a numeric string as currency using `Double.TryParse` then `CDec()`; returns `PMTrue`/`PMFalse`

#### Public Events

| Event | Description |
|---|---|
| `VisibleChange()` | `Visible_Renamed` property set |
| `EnabledChange()` | `Enabled` property set |
| `ShowCoInsurersChange()` | `ShowCoInsurers` property set |
| `ShowEditChange()` | `ShowEdit` property set |
| `DataHasChanged(Sender, DataHasChangedEventArgs)` | Reserve data in the grid changed |

#### Public Structures

```vb
Public Structure udtReserveDetails
    lReserveId As Integer
    cRevisedReserve As Decimal
    cInitialReserve As Decimal
    cPaidToDate As Decimal
    cSumInsured As Decimal
    cThisRevision As Decimal
    cRevisedEntered As Decimal
    sngAverage As Single
    lReserveTypeID As Integer
    sReserveTypeDesc As String
End Structure

Public Structure udtPaymentDetails
    lReserveId As Integer
    cTotalPayment As Decimal
    cReserveAdjustment As Decimal
End Structure
```

#### Public Properties

| Property | Type | Description |
|---|---|---|
| `ShowEdit` | `Boolean` | Shows/hides edit button; triggers control resize |
| `ShowCoInsurers` | `Boolean` | Shows/hides co-insurer tab |
| `Visible_Renamed` | `Boolean` | Shows/hides the reserve frame |
| `IsOpenClaimNoTrans` | `Boolean` | Open Claim no-transaction flag; sets read-only mode |

#### Public Methods

| Method | Signature | Description |
|---|---|---|
| `Initialise` | `() As Integer` | Creates `bObjectManager`, retrieves business object instance |
| `SetProcessModes` | `(Optional vTask, vNavigate, vProcessMode, vTransactionType, vEffectiveDate) As Integer` | Sets all process mode values |
| `LoadControl` | `() As Integer` | Configures form and reads system options per task |
| `GetDetails` | `(Optional lPerilID, lPerilTypeID, lClaimID, lRiskID, lInsurance_File_Cnt) As Integer` | Loads reserve detail array from business for the peril |
| `GetCoInsurerDetails` | `() As Integer` | Loads co-insurer split data from business |
| `GetPaymentDetails` | `() As Integer` | Retrieves payment details for reserve rows |
| `GetReserveGridInArray` | `(ByRef vReserveArray(,) As Object) As Integer` | Returns current reserve ListView data as a 2D object array |
| `SaveScriptArrayToReserve` | `(ByVal vReserveArray(,) As Object) As Integer` | Copies a scripted reserve array back into the control's ListView |
| `UpdatePaymentValue` | `(ByRef uNewValues As udtPaymentDetails) As Integer` | Updates the "this payment" value for a reserve row from payment control data |
| `Save` | `() As Integer` | Checks `m_vReserveDetailsArray` is present. For `C_CR`: calls `m_oBusiness.GetCurrencyRatesToOverride(v_nClaimID)` and if rates differ prompts user to override via `m_oBusiness.OverrideClaimCurrencyRate(v_nClaimID, r_oOverriddenCurrencyRate)`. Saves: calls `m_oBusiness.UpdateReserveDetails(m_vReserveDetailsArray)`. |
| `Dispose` | `() Sub (IDisposable)` | Disposes `g_oObjectManager`, `m_oBusiness`, releases reserves array |

#### Key Private Methods

| Method | Description |
|---|---|
| `FillGrid()` | Rebuilds `lstviewReserve` from `m_ListViewArray`; preserves selected row |
| `PostReserveToOrion(...)` | Gets `bControlTransClaims.Automated`; determines debit/credit accounts (`"CLMEXP"+COBCode` / `"CLMRES"+COBCode`); determines transaction type ID (26=C_CO, 28=C_CR/C_CP); calculates total reserve change from ListView column comparison; calls `g_oClaimTrans.CreateStatsFolder(r_lStatsFolderCnt, sTransactionTypeCode)` then `g_oClaimTrans.CreateStatsDetails(v_lStatsFolderCnt, "GRS", v_lClassOfBusId, v_sCOBCode, ...)` to post to Orion |
| `CalcAverage()` | Calculates reserve average as `(paymentsToDate / sumInsured) * 100` |
| `ResizeControl events` | Handles `Resize` to reposition frame, edit button, and ListView |

#### Reserve Details Array Positions (iCLMReserveControl.vb, `g_cIRDA*`)

| Pos | Column |
|---|---|
| 0 | `reserveid` |
| 1 | `initialreserve` |
| 2 | `paidtodate` |
| 3 | `revisedreserve` |
| 4 | `suminsured` |
| 5 | `average` |
| 6 | `revisioncount` |
| 7 | `reservetype` |
| 8 | `revisedentered` |
| 9 | `lastversionrevisedreserve` / `ThisPayment` |
| 10 | `ThisPaymentTax` |
| 11 | `TaxPaidToDate` |
| 12 | `Initialtax` |
| 13 | `TaxRevision` |
| 14 | `ThisReserveTax` |

*Business Components:* Host-provided business object (typically `bCLMPeril.Business`) + **`bControlTransClaims.Automated`** (Orion stats folder/detail creation for reserve posting)

---
---

## Navigator Roadmaps — Detailed Reference

All three roadmap projects (`iCLMMaintainClaim`, `iCLMPaymentOfClaim`, `iCLMSalvage`) follow an identical structure. Each contains:
- `Interface.vb` — Public facade class, entry point for the Navigator, raised `NavigatorClose` event
- `frmMain.vb` — WinForms container that hosts the navigator step UI
- `MainModule.vb` — Step array, key array, Navigator V2/V3 detection utilities

---

### Navigator Pattern Overview

```
Calling control (e.g. uctCLMCaseClaim)
  └─ Creates:  New iPMNavStart.Interface_Renamed()   [= roadmap Interface_Renamed class]
       ├─ Calls .SetKeys(vKeyArray)                   [stores keys + restart_step]
       ├─ Calls .SetProcessModes(Task, ...)
       ├─ Calls .Start()                              [creates frmMain, calls ShowDialog()]
       │    └─ frmMain loads XML roadmap file
       │         └─ Iterates steps: FindForm → DataForm → BusinessObject
       └─ Handles .NavigatorClose event               [raised after ShowDialog returns]
```

---

### 11. iCLMMaintainClaim — Maintain Claim Navigator

**XML Roadmap File:** `MAINCLM.XML`
**Entry from:** `uctCLMCaseClaim.MaintainClaim()` with keys: `claim_cnt`, `restart_step`, `insurancefile_cnt`, `claim_mode=2`, `claim_id`

#### Interface_Renamed Class

**ProgId:** `Interface_Renamed_NET.Interface_Renamed`

**Public Events:**

| Event | Description |
|---|---|
| `NavigatorClose()` | Raised after `frmMain.ShowDialog()` returns (roadmap completed or cancelled) |

**Public Properties:**

| Property | Type | Description |
|---|---|---|
| `PMProductFamily` | `Integer` | ReadOnly — returns `pmePFSiriusSolutions` |
| `CallingAppName` | `String` | Read/Write — calling application identifier shown in Work Manager |
| `NavigatorV3_PMAuthorityLevel` | `Integer` | Read/Write — authority level passed to Navigator V3 steps |
| `Status` | `Integer` | Read/Write — exit status from the roadmap (`PMOk`/`PMCancel`/`PMError`) |

**Public Methods:**

| Method | Signature | Description |
|---|---|---|
| `Initialise` | `() As Integer` | Stub — returns `PMTrue` immediately |
| `SetKeys` | `(ByRef vKeyArray(,) As Object) As Integer` | Stores all keys except `restart_step` into `m_vKeyArray`; extracts `restart_step` into `m_lRestartStep` |
| `GetKeys` | `(ByRef vKeyArray(,) As Object) As Integer` | Stub — returns `PMTrue` (keys not published from roadmaps) |
| `GetSummary` | `(ByRef vSummaryArray As Object) As Integer` | Stub — returns `PMTrue` |
| `SetProcessModes` | `(Optional vTask, vNavigate, vProcessMode, vTransactionType, vEffectiveDate) As Integer` | Stores process mode values into private members |
| `Start` | `() As Integer` | **Main entry:** Creates `New frmMain`; calls `oForm.SetKeys(m_vKeyArray)`; shows form via `oForm.ShowDialog()` (modal); reads `Status = oForm.Status`; closes form; raises `NavigatorClose()` event |
| `Dispose` | `() Sub (IDisposable)` | Disposes resources |

**MainModule.vb — Navigator Step Structure:**

```vb
Public Structure Step_Renamed
    Description As String
    Component As String         ' ProgId of the step component
    Type As String              ' PMNavComponentFindForm | PMNavComponentDataForm |
                                ' PMNavComponentDecisionForm | PMNavComponentBusinessObject | "PO"
    OKAction As String          ' "OK" | "CANCEL" | "SKIP" | step number
    CancelAction As String
    OKSteps As Integer          ' Steps to advance on OK
    CancelSteps As Integer      ' Steps to advance on Cancel
    ComponentAction As Integer  ' PMEComponentAction enum
    ServerSide As Boolean       ' True = run on server side
    DefaultKeys As Object       ' Default keys for this step
    CreateWorkManagerTask As Boolean
    ResumeStep As Integer       ' -1 = current step (ACResumeStepCurrent)
End Structure
```

**`<ThreadStatic>` Globals:** `g_oObjectManager As bObjectManager.ObjectManager`, `m_vSteps() As Step_Renamed`, `m_vKeyArray As Object`, `m_lCurrentStep As Integer`, `g_iSwitchedSteps As Integer`

**Public Function:**
- `CheckNav2or3(v_oObject, r_bNav2, r_bNav3) As Integer` — Tests if a component implements the `aPMNav.NavigatorV2` or `aPMNav.NavigatorV3` interface via reflection; returns `PMTrue` if either found

---

### 12. iCLMPaymentOfClaim — Pay Claim Navigator

**XML Roadmap File:** `PAYCLM.XML`
**Entry from:** `uctCLMCaseClaim.PayClaim()` after `CopyClaim(v_sTransactionType:="C_CP")`

Identical `Interface.vb`, `frmMain.vb`, and `MainModule.vb` structure to `iCLMMaintainClaim`. Steps defined in `PAYCLM.XML` drive the payment workflow: typically includes risk selection, peril selection, payment entry, cash list posting.

**Key passed:** `claim_cnt`, `claim_mode`, `insurancefile_cnt`, `claim_id`, `transaction_type="C_CP"`

---

### 13. iCLMSalvage — Salvage Navigator

**XML Roadmap File:** `SALVAGE.XML`
**Entry from:** `uctCLMCaseClaim.OpenSalvage()` with key `claim_cnt`

Identical structure to above. Steps in `SALVAGE.XML` drive the salvage recovery workflow.

---

## Business Component Dependencies

The following table shows which business components are called by each user control, and what key operations are delegated to them. Direct SP calls do not appear in user control code — see the Claims Components Reference for SP details of each business component.

| Business Component | Used By | Key Operations Delegated |
|---|---|---|
| `bCLMClaimParty.Business` | `uctClaimParty` | GetParties, SaveParty, SetProcessModes, party CRUD |
| `bCLMCase.Business` | `uctCLMCaseClaim`, `uctCLMCaseHeader`, `uctCLMVersions` | LoadCase, SaveCase, GenerateCaseCode, CreateEvent, LinkClaims, UnlinkClaims, CopyClaim, FindCase, GenerateSQL, GetPreviousDataModel, DeleteCustomData |
| `bCLMPeril.Business` | `uctCLMListPaymentsC`, `uctCLMListReceiptsC`, `uctCLMReceipt`, `uctCLMPerilRT` | GetPaymentList, GetReceiptList, GetAllRecoveries, GetClaimDetails, GetPerilTypes, AddClaimPeril, DeleteClaimPeril, GetCoinsurance, GetReinsurance, SaveReceipt |
| `bCLMFindClaim.Business` | `uctCLMVersions` | Paginated claim/version list retrieval |
| `bACTCurrencyConvert.Form` | `uctCLMListPaymentsC`, `uctCLMListReceiptsC`, `uctCLMReceipt` | Format currency amounts with exchange rates |
| `bCLMPaymentMethod.Business` | `uctCLMReceipt` | Payment method lookup (BACS, CHAPS, cheque etc.) |
| `bControlTransClaims.Automated` | `uctCLMReserve` | CreateStatsFolder, CreateStatsDetails — posts reserve transactions to Orion accounting |
| `bObjectManager.ObjectManager` | All controls | Object creation/retrieval, authority checks, language/source/user ID provision |
| `iPMFormControl.FormFields` | `uctClaimParty`, `uctCLMPayment` | Mandatory field validation (CheckMandatoryControls) |
| `iPMNavStart.Interface_Renamed` | `uctCLMCaseClaim` | Starts Navigator roadmaps: OPENCLM, MAINCLM, PAYCLM, SALVAGE, TPRECOVER |
| `iPMBClientManagerWrapper.Interface_Renamed` | `uctCLMVersions` | Switches active client for claims from external systems |

---

## Consolidated File Inventory

| File | Type | Class / Module | Notes |
|---|---|---|---|
| `uctClaimParty\iCLMPartyControl.vb` | Module | `MainModule` | Constants, globals |
| `uctClaimParty\uctClaimParty.vb` | Class | `uctClaimParty` | Party list control |
| `uctCLMCaseClaimList\iCLMCaseClaimList.vb` | Module | `MainModule` | Constants, roadmap process codes |
| `uctCLMCaseClaimList\uctCLMCaseClaimList.vb` | Class | `uctCLMCaseClaim` | Case claim list + navigator launchers |
| `uctCLMCaseHeader\uctCLMCaseHeaderMod.vb` | Module | (constants module) | Caption/resource constants |
| `uctCLMCaseHeader\uctCLMCaseHeader.vb` | Class | `uctCLMCaseHeader` | Case header fields control |
| `uctCLMListPayments\iCLMListPayments.vb` | Module | `MainModule` | Globals |
| `uctCLMListPayments\uctCLMListPayments.vb` | Class | `uctCLMListPaymentsC` | Read-only payment list |
| `uctCLMListReceipts\iCLMListReceipts.vb` | Module | `MainModule` | Globals |
| `uctCLMListReceipts\uctCLMListReceipts.vb` | Class | `uctCLMListReceiptsC` | Read-only receipt list |
| `uctCLMListVersionControl\MainModule.vb` | Module | `MainModule` | Constants, tree node keys, array positions |
| `uctCLMListVersionControl\cClaimDetails.vb` | Class | `cClaimDetails` | Tree node data container |
| `uctCLMListVersionControl\uctCLMListVersionControl.vb` | Class | `uctCLMVersions` | Claim version history tree + list |
| `uctCLMPayment\iCLMPaymentControl.vb` | Module | `MainModule` | Large constants: payment array, account codes, system options |
| `uctCLMPayment\cPaymentItem.vb` | Class | `cPaymentItem` | Per-reserve payment model |
| `uctCLMPayment\cTaxParameters.vb` | Class | `cTaxParameters` | Tax parameter model |
| `uctCLMPayment\uctCLMPayment.vb` | Class | `uctCLMPayment` | Payment entry grid control |
| `uctCLMPerilRT\Main.vb` | Module | `Main` | Peril ListView column constants |
| `uctCLMPerilRT\uctCLMPerilRT.vb` | Class | `uctCLMPerilRT` | Peril list + add/edit/delete |
| `uctCLMReceipt\iCLMReceiptControl.vb` | Module | `MainModule` | Recovery/receipt constants: array positions, tab IDs, payee options |
| `uctCLMReceipt\cReceiptItem.vb` | Class | `cReceiptItem` | Per-recovery receipt model |
| `uctCLMReceipt\cTaxParameters.vb` | Class | `cTaxParameters` | Tax parameter model |
| `uctCLMReceipt\uctCLMReceipt.vb` | Class | `uctCLMReceipt` | Receipt entry + coinsurer/reinsurer tabs |
| `uctCLMReserve\iCLMReserveControl.vb` | Module | `MainModule` | Reserve array constants, `IsValidCurrency()` |
| `uctCLMReserve\uctCLMReserve.vb` | Class | `uctCLMReserve` | Reserve grid + Orion posting |
| `Roadmaps\iCLMMaintainClaim\Interface.vb` | Class | `Interface_Renamed` | Maintain Claim navigator facade |
| `Roadmaps\iCLMMaintainClaim\frmMain.vb` | Class | `frmMain` *(Friend)* | Navigator container form |
| `Roadmaps\iCLMMaintainClaim\MainModule.vb` | Module | `MainModule` | Step array, key array, Nav2/3 detection |
| `Roadmaps\iCLMPaymentOfClaim\Interface.vb` | Class | `Interface_Renamed` | Pay Claim navigator facade |
| `Roadmaps\iCLMPaymentOfClaim\frmMain.vb` | Class | `frmMain` *(Friend)* | Navigator container form |
| `Roadmaps\iCLMPaymentOfClaim\MainModule.vb` | Module | `MainModule` | Step array, key array |
| `Roadmaps\iCLMSalvage\Interface.vb` | Class | `Interface_Renamed` | Salvage navigator facade |
| `Roadmaps\iCLMSalvage\frmMain.vb` | Class | `frmMain` *(Friend)* | Navigator container form |
| `Roadmaps\iCLMSalvage\MainModule.vb` | Module | `MainModule` | Step array, key array |

---

*End of Claims UI Controls & Interfaces Reference*

---

## Part 4 — Claims Interface Components (Interface_Renamed Classes)

> **Architecture:** Each Claims component that exposes a COM-visible UI entry point contains an `Interface_Renamed` class decorated with `[ProgId("Interface_Renamed_NET.Interface_Renamed")]`. This class is the single entry point invoked by the Navigator/Platform Manager. It implements `IDisposable` and optionally `SSP.S4I.Interfaces.ILocalInterface`.
>
> **Standard properties (all interfaces):**
> - `PMProductFamily` (ReadOnly Integer) — always returns `gPMConstants.PMEProductFamily.pmePFSiriusSolutions`
> - `CallingAppName` (WriteOnly String) — name of the host application
> - `PMAuthorityLevel` (WriteOnly Integer) — authority level from platform  
> - `Status` (ReadOnly Integer) — exit status `gPMConstants.PMEReturnCode`
> - `Initialise() As Integer` — entry point; creates ObjectManager, launches UI form
>
> **Standard private fields (all interfaces):**
> `m_iTask`, `m_lNavigate`, `m_lProcessMode`, `m_sTransactionType`, `m_dtEffectiveDate`, `m_sCallingAppName`, `m_lPMAuthorityLevel`, `m_lStatus`, `m_lReturn`

### Interface Component Index

| # | Interface Class | Component Folder | Class File | ILocalInterface | Business Component |
|---|---|---|---|---|---|
| 1 | `iCLMAuthorisePayments` | Authorise Payments | `iCLMAuthorisePaymentsCls.vb` | No | `bACTUserAuthorities.Business` |
| 2 | `iCLMCaseHistory` | Case | `iCLMCaseHistoryFrm.vb` | No | MainModule only |
| 3 | `iCLMChangeClaimStatus` | Change Claim Status | `iCLMChangeClaimStatusCls.vb` | Yes | `bCLMChangeClaimStatus.Business` |
| 4 | `iCLMCheckDeferredRI` | Check Deferred RI | `iCLMCheckDeferredRICls.vb` | No | `bCLMCheckDeferredRI.Business` |
| 5 | `iCLMCheckUnpaidPremium` | Check Unpaid Premium | `iCLMCheckUnpaidPremiumInterface.vb` | No | `bCLMCheckUnpaidPremium.Business` |
| 6 | `iCLMAddress` | Claim Address | `iCLMAddressInterface.vb` | Yes | `bSIRAddress.Business` (Nav V2) |
| 7 | `iCLMCreateClaimDiary` | Claim Diary | `iCLMCreateClaimDiaryCls.vb` | No | `bCLMDiary.Business` |
| 8 | `iCLMGetClaimLetter` | Claim Letter | `iCLMGetClaimLetterCls.vb` | No | `bCLMGetClaimLetter.Business` |
| 9 | `iCLMPaymentProcess` | Claim Payment Process | `iCLMPaymentProcess.vb` | No | `iPMNavStart.Interface_Renamed` |
| 10 | `iCLMCloseClaim` | Close Claim | `iCLMCloseClaimCls.vb` | No | `bCLMCloseClaim.Business` |
| 11 | `iCLMCoinsuranceRecoveries` | Coinsurance Recoveries | `iCLMCoinsuranceRecoveriesInterface.vb` | Yes | `bCLMCoinsuranceRecoveries.Business` |
| 12 | `iCLMDefnFlds` | Define Fields | `iCLMDefnFldsCls.vb` | Yes | `bCLMDefnFlds.Business` |
| 13 | `iCLMGetClaimDocuments` | Document Production | `iCLMGetClaimDocuments.vb` | Yes | `bCLMGetClaimDocument.Business` |
| 14 | `iCLMFinSumm` | Financial Summary | `iCLMFinSummCls.vb` | Yes | Direct form (`frmInterface`) |
| 15 | `iCLMFindCase` | Find Case | `iCLMFindCaseCls.vb` | No | Direct form (`frmInterface`) |
| 16 | `iCLMFindClaim` | Find Claim | `iCLMFindClaimCls.vb` | Yes | Direct form (`m_ofrmInterface`) |
| 17 | `iSIRFindInsurance` | Find Insurance | `iSIRFindInsurance.vb` | Yes | Direct form (`m_ofrmInterface`) |
| 18 | `iCLMFindParty` | Find Party | `iCLMFindPartyCls.vb` | No | Direct form (`frmInterface`) |
| 19 | `iCLMPeril` | Generic Peril | `iCLMPeril.vb` | No | `iPMURiskWrapper.Interface_Renamed` |
| 20 | `iCLMInfoChklst` | Information Checklist | `iCLMInfoChklst.vb` | Yes | Direct form (`frmInterface`) |
| 21 | `iCLMListPayments` | List Payments | `iCLMListPayments.vb` | Yes | `bCLMRecovery.Business` |
| 22 | `iCLMListReceipts` | List Receipts | `iCLMListReceipts.vb` | Yes | `bCLMRecovery.Business` |
| 23 | `iCLMLossSchedule` | Loss Schedule | `iCLMLossSchedule.vb` | No | `bCLMLossSchedule.Business` |
| 24 | `iCLMPaymentMethod` | Payment Method | `iCLMPaymentMethodCls.vb` | Yes | Direct form (`frmInterface`) |
| 25 | `iCLMPerilType` | Peril Type | `iCLMPeriltype.vb` | No | Direct form (`frmInterface`) |
| 26 | `iCLMPerilReserveType` | Peril Type Reserve Type | `iCLMPerilTypeReserveTypeCls.vb` | Yes | `bCLMDefnFlds.Business` |
| 27 | `iCLMRecovery` | Recovery | `iCLMRecoveryInterface.vb` | Yes | `bCLMRecovery.Business` |
| 28 | `iCLMReinsurance` | Reinsurance Recoveries | `iCLMReinsurance.vb` | No | Dynamic (`m_oFrmInterface As Object`) |
| 29 | `iCLMResvDefn` | Reserve Definition | `iCLMResvDefnCls.vb` | Yes | Direct form (`frmInterface`) |
| 30 | `iCLMRiskDetails` | Risk Details | `iCLMRiskDetailsInterface.vb` | Yes | `bCLMRiskDetails.Business` |
| 31 | `iCLMRiskType` | Risk Type | `iCLMRisktype.vb` | No | Direct form (`frmInterface`) |
| 32 | `iCLMRiskTypeInfoChecklist` | Risk Type Information Checklist | `iCLMRiskTypeInfoChecklistInterface.vb` | No | `bCLMRiskTypeInfoChecklist.Business` |
| 33 | `iCLMSalvageRecovery` | Salvage Recovery | `iCLMSalvageRecoveryInterface.vb` | No | `bCLMSalvageRecovery.Business` |
| 34 | `iCLMThirdPartyRecovery` | Third Party Recovery | `iCLMThirdPartyRecoveryInterface.vb` | No | `bCLMThirdPartyRecovery.Business` |
| 35 | `iOpenClaim` | Open Claim | `iOpenClaim.vb` | No | `iOpenClaim.General` |
| 36 | `iCLMUnlock` | Unlock | `iCLMUnlockCls.vb` | No | `bPMLock.User` |
| 37 | Navigator Roadmaps (×3) | Roadmaps | `iNavigatorV3.vb` / `iNavigatorV2.vb` | See Part 3 | — |


### 4.1 Case Management Interfaces

---

#### iCLMCaseHistory — Case History

**Source:** `Claims\Components\Case\Interface\iCLMCaseHistory\`
**Entry class file:** `iCLMCaseHistory.vb` (MainModule), main form: `iCLMCaseHistoryFrm.vb`
**Purpose:** Displays the history log for a claims case — date of change, description, progress status, and user.

**Module constants (`iCLMCaseHistory.vb`):**

| Constant | Value | Description |
|---|---|---|
| `kInterfaceTitle` | 100 | Form caption resource ID |
| `kLvwColNameDateOfChange` | 101 | List column header: Date of Change |
| `kLvwColNameDescription` | 102 | List column header: Description |
| `kLvwColNameProgressStatus` | 103 | List column header: Progress Status |
| `kLvwColNameUser` | 104 | List column header: User |
| `ACMaxSearchDetails` | 500 | Maximum records to retrieve |
| `ACDateConversion` | "dd/mm/yyyy" | Date format constant |

**Result array indexes:** `kICaseID`=0, `kIDateOfChange`=1, `kIDescription`=2, `kIProgressStatus`=3, `kIUser`=4

---

#### iCLMFindCase — Find Case

**Source:** `Claims\Components\Find Case\Interface\iCLMFindCase\`
**Entry class file:** `iCLMFindCaseCls.vb` (Interface_Renamed class), module: `iCLMFindCase.vb`
**ILocalInterface:** No

**Private domain fields:**

| Field | Type | Description |
|---|---|---|
| `m_sCaseNumber` | String | Case number search criteria |
| `m_lProgressStatusId` | Integer | Progress status filter |
| `m_dtCaseOpenDate` | Date | Case open date filter |
| `m_lClaimNumber` | Integer | Related claim number |
| `m_lRiskTypeId` | Integer | Risk type filter |
| `m_bDisableWildcardSearchOption` | Boolean | System option 5065 |
| `m_bEnablePartialWildcardSearchOption` | Boolean | System option 5066 |

**List view columns:** CaseNumber, CaseOpenDate, Analyst, Assistant, ProgressStatus, TotalIndemnity, TotalExpense, TotalExcess, CaseID

---

### 4.2 Claim Lifecycle Interfaces

---

#### iCLMChangeClaimStatus — Change Claim Status

**Source:** `Claims\Components\Change Claim Status\Interface\iCLMChangeClaimStatus\`
**Entry class file:** `iCLMChangeClaimStatusCls.vb`
**ILocalInterface:** Yes

**Module constants:** `ACModeAuthorise` = `PMEComponentAction.PMReverse`, `ACModeProcessed`=0, `ACModeReferred`=1, `ACModeRecommend`=1

**Private domain fields:**

| Field | Type | Description |
|---|---|---|
| `m_lClaimId` | Integer | The claim being processed |
| `m_lOriginalClaimID` | Integer | Original claim ID |
| `m_cAmount` | Decimal | Payment amount |
| `m_lCurrencyId` | Integer | Currency |
| `m_cThisPaymentAmount` | Decimal | This payment amount |
| `m_lThisPaymentCurrencyId` | Integer | This payment currency (PN45635) |
| `m_bBalanceAndCloseClaim` | Boolean | Balance and close mode flag |
| `m_lDocumentId` | Integer | Associated document |
| `m_lMediaTypeId` | Integer | Media type |
| `m_lAccountId` | Integer | Account |
| `m_lClaimPaymentId` | Integer | Claim payment ID |
| `m_crTotalClaimPaymentAmount` | Decimal | Total claim payment amount |
| `m_lSourceId` | Integer | Source |
| `m_bNoTransactions` | Boolean | No transactions flag |
| `m_iClaimWorkFlowID` | Integer | Workflow ID |
| `m_lInsuranceFileCnt` | Integer | Insurance file count |
| `m_bIs_Multiple_Claims_Payments` | Boolean | Multiple claims payments |
| `m_nAuthorisation_Threshold` | Decimal | Authorisation threshold |

**Business:** `bCLMChangeClaimStatus.Business`
**Task groups:** `ACTClaimAdminTaskGroupID`=10, `ACTBrokingTaskGroupID`=21

---

#### iCLMCloseClaim — Close Claim

**Source:** `Claims\Components\Close Claim\Interface\iCLMCloseClaim\`
**Entry class file:** `iCLMCloseClaimCls.vb`
**ILocalInterface:** No

**Private domain fields:**

| Field | Type | Description |
|---|---|---|
| `m_vKeyArray` | Object | Key array |
| `m_lInsuranceFileCnt` | Integer | Insurance file count |
| `m_lWorkClaimID` | Integer | Work claim ID |
| `m_lClaimID` | Integer | The claim to close |
| `m_sClaimStatus` | String | Current claim status |
| `m_sUnderwriting` | String | Underwriting/broking mode |
| `m_bClaimClosed` | Boolean | Set True when claim closed (PN61432) |

**Custom properties:** `ClaimClosed As Boolean` (ReadWrite), `InsuranceFileCnt As Integer` (ReadWrite)
**Business:** `bCLMCloseClaim.Business`

---

#### iCLMUnlock — Unlock

**Source:** `Claims\Components\Unlock\Interface\iCLMUnlock\`
**Entry class file:** `iCLMUnlockCls.vb`
**ILocalInterface:** No

**Private domain fields:**

| Field | Type | Description |
|---|---|---|
| `m_vKeyArray` | Object | Key array |
| `m_lInsuranceFileCnt` | Integer | Insurance file count |
| `m_lWorkClaimID` | Integer | Work claim ID |
| `m_lClaimID` | Integer | The claim to unlock |
| `m_sClaimStatus` | String | Current claim status |
| `m_sUnderwriting` | String | Underwriting/broking mode |

**Custom properties:** `InsuranceFileCnt As Integer` (ReadWrite)
**Business:** `bPMLock.User`

---

#### iCLMPaymentProcess — Claim Payment Process

**Source:** `Claims\Components\Claim Payment Process\Interface\iCLMPaymentProcess\`
**Entry class file:** `iCLMPaymentProcess.vb`
**ILocalInterface:** No (uses `iPMNavStart.Interface_Renamed` nested navigator)

**Private domain fields:**

| Field | Type | Description |
|---|---|---|
| `m_lClaimId` | Integer | The claim |
| `m_lOriginalClaimID` | Integer | Original claim ID |
| `m_sClaimReference` | String | Claim reference string |
| `m_lCurrencyId` | Integer | Currency |
| `m_bBalanceAndCloseClaim` | Boolean | Balance and close mode |
| `m_lSourceId` | Integer | Source |
| `m_lWorkflowId` | Integer | Workflow ID |
| `m_bClaim_Payment_Process` | Boolean | Payment process flag |
| `m_iClaimPaymentValid` | Integer | Claim payment validation |
| `m_bUserAuthRunClaimPayment` | Boolean | User auth to run payment |
| `m_oNavStart` | `iPMNavStart.Interface_Renamed` | Navigator with events |

**Navigator events handled:** `NavigatorClose`, `SetProcessStatus`


---

### 4.3 Claim Information & Checking Interfaces

---

#### iCLMCheckDeferredRI — Check Deferred RI

**Source:** `Claims\Components\Check Deferred RI\Interface\iCLMCheckDeferredRI\`
**Entry class file:** `iCLMCheckDeferredRICls.vb`
**ILocalInterface:** No

**Private domain fields:**

| Field | Type | Description |
|---|---|---|
| `m_lClaimID` | Integer | The claim |
| `m_sClaimRef` | String | Claim reference |
| `m_bCheckDeferredRI` | Boolean | Flag indicating deferred RI check needed |
| `m_iWorkFlowId` | Integer | Workflow ID |

**Business:** `bCLMCheckDeferredRI.Business`
**Module constant:** `ksDeferredRIRiskStatus = "RIDEFERRED"`

---

#### iCLMFinSumm — Financial Summary

**Source:** `Claims\Components\Financial Summary\Interface\iCLMFinSumm\`
**Entry class file:** `iCLMFinSummCls.vb`
**ILocalInterface:** Yes

**Private domain fields:**

| Field | Type | Description |
|---|---|---|
| `m_lClaimId` | String | Claim ID to summarise |
| `m_sClaimRef` | String | Claim reference |
| `m_lInsuranceFileCnt` | Integer | Policy count |
| `m_sPolicyRef` | String | Policy reference filter |
| `m_lPolicyHolderCnt` | Integer | Policy holder count |
| `m_sPolicyHolder` | String | Policy holder filter |
| `m_nFindClaimMode` | Integer | Find claim mode |

**Custom properties:** `ClaimRef As String` (ReadWrite), `ClaimId As String` (ReadWrite)

**Form tabs (resource IDs):** Total (101), Advanced (102), Salvage (204), TP Recovery (205), Payment (402), Receipt (403)

---

#### iCLMInfoChklst — Information Checklist

**Source:** `Claims\Components\Information Checklist\Interface\iCLMInfoChklst\`
**Entry class file:** `iCLMInfoChklst.vb`
**ILocalInterface:** Yes

Sub-forms in folder: `iCLMRequirement.vb`, `iCLMService.vb`

**Private domain fields:**

| Field | Type | Description |
|---|---|---|
| `m_lClaimID` | Integer | The claim |
| `m_lOriginalClaimID` | Integer | Original claim ID |
| `m_sClaimNumber` | String | Claim number for caption |
| `m_nFindClaimMode` | Integer | Find claim mode |
| `m_lRiskTypeID` | Integer | Risk type |
| `m_lDeleteWorkTableFlag` | Integer | Delete work table on cancel |
| `m_bBalanceAndCloseClaim` | Boolean | Balance and close mode |
| `m_bIsIAG` | Boolean | IAG-specific processing |

---

#### iCLMCheckUnpaidPremium — Check Unpaid Premium

**Source:** `Claims\Components\Check Unpaid Premium\Interface\iCLMCheckUnpaidPremium\`
**Entry class file:** `iCLMCheckUnpaidPremiumInterface.vb`
**ILocalInterface:** No

**Private domain fields:**

| Field | Type | Description |
|---|---|---|
| `m_lBusinessTypeId` | Integer | Business type |
| `m_bCheckUnpaidStatus` | Boolean | Unpaid status flag |
| `m_iClaimWorkFlowId` | Integer | Workflow ID |

**Business:** `bCLMCheckUnpaidPremium.Business`

---

### 4.4 Search / Find Interfaces

---

#### iCLMFindClaim — Find Claim

**Source:** `Claims\Components\Find Claim\Interface\iCLMFindClaim\`
**Entry class file:** `iCLMFindClaimCls.vb`
**ILocalInterface:** Yes

**Private domain fields:**

| Field | Type | Description |
|---|---|---|
| `m_lClaimCnt` | Integer | Count of matching claims |
| `m_sClaimRef` | String | Claim reference search |
| `m_lInsuranceFilecnt` | Integer | Policy count |
| `m_sPolicyRef` | String | Policy reference filter |
| `m_lPolicyHolderCnt` | Integer | Policy holder count |
| `m_sPolicyHolder` | String | Policy holder filter |
| `m_nFindClaimMode` | Integer | Find mode |
| `m_lRiskTypeId` | Integer | Risk type filter |
| `m_lPartyCnt` | Integer | Party count |
| `m_bAskDPAQuestions` | Boolean | DPA compliance questions |
| `m_lRealClaimID` | Integer | Resolved real claim ID |
| `m_bIncludeClosedClaims` | Boolean | Include closed claims in search |
| `m_iClaimWorkflowId` | Integer | Workflow ID |
| `m_IIsComplaint` | Integer | Complaint flag |
| `m_bSelectVersionEnabled` | Boolean | Version selection |

---

#### iSIRFindInsurance — Find Insurance

**Source:** `Claims\Components\Find Insurance\Interface\iSIRFindInsurance\`
**Entry class file:** `iSIRFindInsurance.vb`
**ILocalInterface:** Yes

**Private domain fields:**

| Field | Type | Description |
|---|---|---|
| `m_nInsuranceFilecnt` | Integer | Count of found policies |
| `m_sPolicyNumber` | String | Policy number search |
| `m_sPolicyCode` | String | Policy code search |
| `m_sRiskIndex` | String | Risk index search |
| `m_dtClaimDate` | Object | Claim date |
| `m_sShortName` | String | Short name search |
| `m_sPostCode` | String | Postcode search |
| `m_dtFromDate` | Object | Date from filter |
| `m_dtToDate` | Object | Date to filter |
| `m_sPolicyHolder` | String | Policy holder |
| `m_sProductCode` | String | Product code |
| `m_lSiriusUnderWritingBroking` | String | UW/broking mode |
| `m_vSourceArray` | Object | Source array |
| `m_bDisableWildcardSearchOption` | Boolean | System option 5065 |
| `m_bEnablePartialWildcardSearchOption` | Boolean | System option 5066 |

---

#### iCLMFindParty — Find Party

**Source:** `Claims\Components\Find Party\Interface\iCLMFindParty\`
**Entry class file:** `iCLMFindPartyCls.vb`
**ILocalInterface:** No

**Private domain fields:**

| Field | Type | Description |
|---|---|---|
| `m_iNotEditable` | Integer | Read-only mode |
| `m_vSourceArray` | Object | Source array |
| `m_bPartyTypeOnly` | Boolean | Show party type screen only |
| `m_sPartyType` | String | Party type to return |
| `m_bDeleteMode` | Boolean | Delete mode |
| `m_lInvariantKey` | Integer | Invariant key |
| `m_sSiriusUnderWritingBroking` | String | UW/broking mode |

**Custom properties:** `InvariantKey As Integer` (ReadWrite)

**Party type constants:** `ACIDriver`="Driver", `ACIRepairer`="Repairer", `ACIThirdParty`="Third Party", `ACIWitness`="Witness"


---

### 4.5 Document & Letter Interfaces

---

#### iCLMGetClaimLetter — Claim Letter

**Source:** `Claims\Components\Claim Letter\Interface\iCLMGetClaimLetter\`
**Entry class file:** `iCLMGetClaimLetterCls.vb`
**ILocalInterface:** No

**Private domain fields:**

| Field | Type | Description |
|---|---|---|
| `m_lClaimID` | Integer | The claim |
| `m_lPartyCnt` | Integer | Party count |
| `m_sPartyShortname` | String | Party short name |
| `m_sClaimNumber` | String | Claim number |
| `m_lProcessType` | Integer | Process type |
| `m_lPolicycnt` | Integer | Policy count |
| `m_sDocDescription` | String | Document description |
| `m_sTaskCode` | String | Task code |

**Business:** `bCLMGetClaimLetter.Business`

---

#### iCLMGetClaimDocuments — Document Production

**Source:** `Claims\Components\Document Production\Interface\iCLMGetClaimDocuments\`
**Entry class file:** `iCLMGetClaimDocuments.vb`
**ILocalInterface:** Yes

**Private domain fields:**

| Field | Type | Description |
|---|---|---|
| `m_lClaimID` | Integer | The claim |
| `m_lPartyCnt` | Integer | Party count |
| `m_lProcessType` | Integer | Process type |
| `m_lPolicycnt` | Integer | Policy count |
| `m_sDocDescription` | Object | Document description |
| `m_bGenerateClaimDocument` | Boolean | Generate document flag |
| `m_iClaimWorkFlowId` | Integer | Workflow ID |
| `m_sDocumentRef` | String | Document reference |
| `m_sProcessTypeDocuments` | String | Process type documents (PN56858) |
| `m_lUserChoice` | Integer | User choice |
| `m_lIsEditableMerging` | Integer | Editable after merging |

**Document type short codes:**

| Code | Expanded Name |
|---|---|
| `CJ` | Claim Jacket |
| `CC` | Claim notification to Client |
| `CA` | Claim notification to Agent |
| `CI` | Claim notification to Insurer |
| `EN` | External Handler Notification |
| `LL` | Large Loss Advice |
| `LA` | Loss Advice |
| `AS` | Claim Advice to Agent |
| `CQ` | Cheque Requisition |
| `AF` | Claim Acceptance Form |
| `NT` | Advice to Reinsurer |
| `CP` | Claim Payment Advice |

**Business:** `bCLMGetClaimDocument.Business`

---

#### iCLMCreateClaimDiary — Claim Diary

**Source:** `Claims\Components\Claim Diary\Interface\iCLMCreateClaimDiary\`
**Entry class file:** `iCLMCreateClaimDiaryCls.vb`
**ILocalInterface:** No

Standard interface members only; no custom domain fields in entry class.
**ACApp:** `"iCLMCreateClaimDiary"`

---

### 4.6 Reserve, Financial Definition & Configuration Interfaces

---

#### iCLMResvDefn — Reserve Definition

**Source:** `Claims\Components\Reserve Definition\Interface\iCLMResvDefn\`
**Entry class file:** `iCLMResvDefnCls.vb`
**ILocalInterface:** Yes

**Private domain fields:**

| Field | Type | Description |
|---|---|---|
| `m_lClaimId` | String | Claim ID |
| `m_sClaimRef` | String | Claim reference |
| `m_lInsuranceFileCnt` | Integer | Policy count |
| `m_sPolicyRef` | String | Policy reference |
| `m_lPolicyHolderCnt` | Integer | Policy holder count |
| `m_sPolicyHolder` | String | Policy holder |
| `m_nFindClaimMode` | Integer | Find claim mode |

**Custom properties:** `ClaimRef As String` (ReadWrite), `ClaimId As String` (ReadWrite)

---

#### iCLMDefnFlds — Define Fields

**Source:** `Claims\Components\Define Fields\Interface\iCLMDefnFlds\`
**Entry class file:** `iCLMDefnFldsCls.vb`
**ILocalInterface:** Yes

**Module constants:** `ACRiskMode=PMEComponentAction.PMView`, `ACPerilMode=PMEComponentAction.PMAdd`

Field type constants: `ACText`=1, `ACInteger`=2, `ACDate`=3, `ACYesNo`=4, `ACLookUp`=5, `ACParty`=6, `ACTabName`=7

**Private domain fields:**

| Field | Type | Description |
|---|---|---|
| `m_lTypeId` | String | Type ID (risk or peril type) |
| `m_lMode` | Integer | Mode (Risk=PMView, Peril=PMAdd) |
| `m_sTypeName` | String | Type name display |
| `m_lPerilTypeId` | Integer | Peril type ID |
| `m_sPerilTypeDescription` | String | Peril type description |

**Custom properties:** `TypeName As String` (ReadWrite), `TypeId As String` (ReadWrite)
**Business:** `bCLMDefnFlds.Business`

---

#### iCLMPerilReserveType — Peril Type Reserve Type

**Source:** `Claims\Components\Peril Type Reserve Type\Interface\iCLMPerilReserveType\`
**Entry class file:** `iCLMPerilTypeReserveTypeCls.vb`
**ILocalInterface:** Yes

**Private domain fields:**

| Field | Type | Description |
|---|---|---|
| `m_lTypeId` | String | Type ID |
| `m_lMode` | Integer | Mode |
| `m_sTypeName` | String | Type name |
| `m_lPerilTypeId` | Integer | Peril type ID |
| `m_sPerilTypeDescription` | String | Peril type description |

**Custom properties:** `TypeName As String` (ReadWrite), `TypeId As String` (ReadWrite)
**Business:** `bCLMDefnFlds.Business`

---

### 4.7 Peril & Risk Configuration Interfaces

---

#### iCLMPerilType — Peril Type

**Source:** `Claims\Components\Peril Type\Interface\iCLMPerilType\`
**Entry class file:** `iCLMPeriltype.vb`
**ILocalInterface:** No

**Private domain fields:** `m_lClaimId`, `m_lClaimCnt`, `m_sClaimRef`, `m_lInsuranceFileCnt`, `m_sPolicyRef`, `m_sPolicyHolder`, `m_nFindClaimMode`
**ACApp:** `"iCLMPerilType"`, sub-form `frmInterface`

---

#### iCLMRiskType — Risk Type

**Source:** `Claims\Components\Risk Type\Interface\iCLMRiskType\`
**Entry class file:** `iCLMRisktype.vb`
**ILocalInterface:** No

**Private domain fields:** `m_lClaimId`, `m_lClaimCnt`, `m_sClaimRef`, `m_lInsuranceFileCnt`, `m_sPolicyRef`, `m_sPolicyHolder`, `m_nFindClaimMode`, `m_lSiriusUnderWritingBroking`
**ACApp:** `"iCLMRiskType"`, sub-form `frmInterface`

---

#### iCLMPaymentMethod — Payment Method

**Source:** `Claims\Components\Payment Method\Interface\iCLMPaymentMethod\`
**Entry class file:** `iCLMPaymentMethodCls.vb`
**ILocalInterface:** Yes

**Private domain fields:**

| Field | Type | Description |
|---|---|---|
| `m_lPartyid` | Integer | Party ID |
| `m_lScreenMethod` | Integer | Screen method code |
| `m_sPartyName` | String | Party name |
| `m_sComments` | String | Comments |
| `m_lButtonClicked` | Integer | Which button was clicked |
| `m_cAmount` | Decimal | Amount |
| `m_lAgentID` | Integer | Agent ID |
| `m_sAgentName` | String | Agent name |
| `m_lClientID` | Integer | Client ID |
| `m_sClientName` | String | Client name |
| `m_lProductID` | Integer | Product ID |
| `m_lPayeeMediaType` | Integer | Payee media type |
| `m_sPayeeName` | String | Payee name |
| `m_sPayeeBankName` | String | Payee bank name |
| `m_sPayeeSortCode` | String | Sort code |
| `m_sPayeeAccountNo` | String | Account number |
| `m_lPayeeCountry` | Integer | Payee country |
| `m_sPayeeComments` | String | Payee comments |
| `m_lInsuranceFileCnt` | Integer | Policy count |
| `m_sClaimRef` | String | Claim reference |
| `m_iCurrencyID` | Integer | Currency |
| `m_lClaimID` | Integer | Claim ID |
| `m_iLossCurrencyID` | Integer | Loss currency |
| `m_cLossCurrencyAmount` | Decimal | Loss currency amount |
| `m_lClaimPerilID` | Integer | Claim peril |
| `m_lRiskTypeId` | Integer | Risk type |
| `m_bFromNavigator` | Boolean | Called from navigator |
| `m_bAuthoriseMode` | Boolean | Authorisation mode |
| `m_lClaimPaymentID` | Integer | Claim payment ID |
| `m_sScreenType` | String | Screen type |

**Custom properties:** `ProductID`, `AgentID`, `AgentName`, `ClientID`, `ClientName` (all ReadWrite Integer/String)

---

#### iCLMAddress — Claim Address

**Source:** `Claims\Components\Claim Address\Interface\iCLMAddress\`
**Entry class file:** `iCLMAddressInterface.vb`
**ILocalInterface:** Yes | **Navigator:** V2 (`iNavigatorV2.vb`)

**Private domain fields:**

| Field | Type | Description |
|---|---|---|
| `m_lAddressCnt` | Integer | Address count |
| `m_sAddress1..4` | String | Address lines 1-4 |
| `m_sPostalCode` | String | Postal code |
| `m_lCountryID` | Integer | Country ID |
| `m_lAddressId` | Integer | Address ID |
| `m_bAddressChanged` | Boolean | Address modified flag |
| `m_iQASType` | Integer | QAS address service type |

**Business:** `bSIROptions.Business`, `bSIRAddress.Business`


---

### 4.8 Recovery & Reinsurance Interfaces

---

#### iCLMRecovery — Recovery

**Source:** `Claims\Components\Recovery\Interface\iCLMRecovery\`
**Entry class file:** `iCLMRecoveryInterface.vb`
**ILocalInterface:** Yes

**Custom properties (in addition to standard):**

| Property | Type | Description |
|---|---|---|
| `Task` | `gPMConstants.PMEComponentAction` | Task action |
| `Navigate` | Integer | Navigate flag |
| `ProcessMode` | Integer | Process mode |
| `TransactionType` | String | Transaction type |
| `EffectiveDate` | Date | Effective date |

**Private domain fields:**

| Field | Type | Description |
|---|---|---|
| `m_lClaimId` | Integer | Claim ID |
| `m_lRecoveryID` | Integer | Recovery ID |
| `m_vRecoveryType` | String | Recovery type |
| `m_lClaimPerilId` | Integer | Claim peril |
| `m_bIsSalvage` | Boolean | Salvage recovery mode |
| `m_bBalanceAndCloseClaim` | Boolean | Balance and close mode |
| `m_bFurtherReceipts` | Boolean | Further receipts pending |

**Sub-forms:** `frmSelectPeril`, `frmRecovery`, `frmRecoveryReceipt`, `frmRecoveryReserve`, `frmRecoveryReceipting`
**Business:** `bCLMRecovery.Business`

---

#### iCLMSalvageRecovery — Salvage Recovery

**Source:** `Claims\Components\Salvage Recovery\Interface\iCLMSalvageRecovery\`
**Entry class file:** `iCLMSalvageRecoveryInterface.vb`
**ILocalInterface:** No

**Private domain fields:**

| Field | Type | Description |
|---|---|---|
| `m_lClaimID` | Integer | Claim ID |
| `m_lClaimMode` | Integer | Claim mode |
| `m_lInsuranceFileCnt` | Integer | Policy count |
| `m_bIsIAG` | Boolean | IAG-specific processing |

**Business:** `bCLMSalvageRecovery.Business`
**Main form:** `iCLMSalvageMain.vb` (189.5KB)

---

#### iCLMThirdPartyRecovery — Third Party Recovery

**Source:** `Claims\Components\Third Party Recovery\Interface\iCLMThirdPartyRecovery\`
**Entry class file:** `iCLMThirdPartyRecoveryInterface.vb`
**ILocalInterface:** No

**Private domain fields:**

| Field | Type | Description |
|---|---|---|
| `m_lClaimID` | Integer | Claim ID |
| `m_nFindClaimMode` | Integer | Find claim mode |
| `m_lInsuranceFileCnt` | Integer | Policy count |
| `m_bIsIAG` | Boolean | IAG-specific processing |

**Custom properties:** `ClaimNumber`, `ClaimId`, `ClaimMode` (ReadWrite)
**Business:** `bCLMThirdPartyRecovery.Business`
**Main form:** `iCLMThirdPartyMain.vb` (181.7KB)

---

#### iCLMCoinsuranceRecoveries — Coinsurance Recoveries

**Source:** `Claims\Components\Coinsurance Recoveries\Interface\iCLMCoinsuranceRecoveries\`
**Entry class file:** `iCLMCoinsuranceRecoveriesInterface.vb`
**ILocalInterface:** Yes

**Private domain fields:**

| Field | Type | Description |
|---|---|---|
| `m_bIsIAG` | Boolean | IAG-specific processing (`SIROPTIsNRMA`) |
| `m_bBalanceAndCloseClaim` | Boolean | Balance and close mode |

**Business:** `bCLMCoinsuranceRecoveries.Business`
**Sub-forms:** `iCLMCoInsuranceRecoveries.vb` (62.9KB), `iCLMCoInsuranceDetails.vb` (17.2KB)

---

#### iCLMReinsurance — Reinsurance Recoveries

**Source:** `Claims\Components\Reinsurance Recoveries\Interface\iCLMReinsurance\`
**Entry class file:** `iCLMReinsurance.vb`
**ILocalInterface:** No

**Private domain fields:**

| Field | Type | Description |
|---|---|---|
| `m_sClaimNumber` | String | Claim number |
| `m_lClaimID` | Integer | Claim ID |
| `m_lInsuranceFileCnt` | Integer | Policy count |
| `m_bDisplayClaimReinsurance` | Boolean | Show RI form flag |
| `m_bOpenClaimNoTrans` | Boolean | Open claim no-transaction mode |
| `m_bIsReserveUpdatednTaskCompleted` | Boolean | Reserve updated and task completed |
| `m_bRecovery` | Boolean | Recovery mode |
| `m_lRecovery` | Integer | Actual recovery amount |

**Custom write-only properties:** `Recovery As Boolean`, `ActualRecovery As Integer`

**Sub-forms:** `iCLMReinsuranceFrm.vb` (48.4KB), `iCLMReinsuranceRI2007.vb` (79.1KB)
**Note:** Form is created dynamically as `Object` to support both RI and RI2007 form variants.

---

#### iCLMListReceipts — List Receipts

**Source:** `Claims\Components\List Receipts\Interface\iCLMListReceipts\`
**Entry class file:** `iCLMListReceiptsInterface.vb`
**ILocalInterface:** Yes

**Private domain fields:**

| Field | Type | Description |
|---|---|---|
| `m_lClaimId` | Integer | Claim ID |
| `m_lRecoveryID` | Integer | Recovery ID |
| `m_lWorkClaimPerilId` | Integer | Work claim peril ID |
| `m_bShowPaymentView` | Boolean | Show payment view mode |

**Custom properties:** `ClaimPaymentId As Integer`, `ShowPaymentView As Boolean`, `WorkClaimPerilId As Integer` (all ReadWrite)
**Business:** `bCLMRecovery.Business`

---

#### iCLMListPayments — List Payments

**Source:** `Claims\Components\List Payments\Interface\iCLMListPayments\`
**Entry class file:** `iCLMListPayments.vb`
**ILocalInterface:** Yes

Same structure as `iCLMListReceipts`.
**Custom properties:** `ClaimPaymentId`, `ShowPaymentView`, `WorkClaimPerilId`
**Business:** `bCLMRecovery.Business`

---

### 4.9 Risk Details Interface

---

#### iCLMRiskDetails — Risk Details

**Source:** `Claims\Components\Risk Details\Interface\iCLMRiskDetails\`
**Entry class file:** `iCLMRiskDetailsInterface.vb`
**ILocalInterface:** Yes

**Private domain fields:**

| Field | Type | Description |
|---|---|---|
| `m_lclaimid` | Integer | Claim ID |
| `m_lRisk` | Integer | Risk number |
| `m_lPolicyId` | Integer | Policy ID |
| `m_bViewRiskFlag` | Boolean | View-only risk flag |
| `m_lClaimMode` | Integer | Claim mode |
| `m_bBalanceAndCloseClaim` | Boolean | Balance and close mode |
| `m_bOpenClaimNoTrans` | Boolean | Open claim no-transaction mode |
| `m_bReserveLimitExceeded` | Boolean | Reserve limit exceeded |
| `m_bFurtherPayments` | Boolean | Further payments pending |

**Business:** `bCLMRiskDetails.Business`
**Main form:** `iCLMRiskDetailsFrm.vb` (211.7KB — largest form in module)

---

#### iCLMRiskTypeInfoChecklist — Risk Type Information Checklist

**Source:** `Claims\Components\Risk Type Information Checklist\Interface\iCLMRiskTypeInfoChecklist\`
**Entry class file:** `iCLMRiskTypeInfoChecklistInterface.vb`
**ILocalInterface:** No

**Private domain fields:**

| Field | Type | Description |
|---|---|---|
| `m_lBusinessTypeId` | Integer | Business type |
| `m_bCheckUnpaidStatus` | Boolean | Unpaid status flag |
| `m_iClaimWorkFlowId` | Integer | Workflow ID |

**Business:** `bCLMRiskTypeInfoChecklist.Business`

---

### 4.10 Open Claim, Authorise Payments, Generic Peril, Loss Schedule (WinForms Variants)

These four interfaces use `frmInterface.vb` or a named form class as the main entry point rather than `Interface_Renamed` + `*Cls.vb`.

---

#### iOpenClaim — Open Claim

**Source:** `Claims\Components\Open Claim\Interface\Open Claim\`
**Entry class file:** `iOpenClaim.vb` (52.5KB)
**ILocalInterface:** No

**Business object:** `iOpenClaim.General`

**Private domain fields:**

| Field | Type | Description |
|---|---|---|
| `m_oGeneral` | `iOpenClaim.General` | Main business controller |
| `m_oFormFields` | Object | Form field definitions |
| `m_bFSAComplianceFlag` | Boolean | FSA compliance questions required |
| `m_vPrimaryCauseArray` | Object | Primary cause picklist data |
| `m_vSecondaryCauseArray` | Object | Secondary cause picklist data |

**Module constants (`iOpenClaimMod.vb`):** Toolbar image indexes 11, 15, 16, 17, 18

---

#### iCLMAuthorisePayments — Authorise Payments

**Source:** `Claims\Components\Authorise Payments\Interface\Authorise Payments\`
**Entry class file:** `iCLMAuthorisePaymentsCls.vb` (19.6KB)
**Entry module:** `iCLMAuthorisePayments.vb` (5.7KB), navigator: `iNavigatorV3.vb` (14.6KB)
**ILocalInterface:** No

**Private domain fields:**

| Field | Type | Description |
|---|---|---|
| `m_oGeneral` | `iCLMAuthorisePayments.General` | General business controller |
| `m_oUserAuthorities` | `bACTUserAuthorities.Business` | User authority checker |
| `m_bClaimPaymentWorkflowEnabled` | Boolean | Workflow enabled flag |
| `m_iIsReferredForRecommendation` | Integer | Referred for recommendation |

**Task group constants:** `ACTClaimAdminTaskGroupID`=10, `ACTBrokingTaskGroupID`=21, `ACClaimPaymentsType`=1, `ACPaymentsType`=2

---

#### iCLMPeril — Generic Peril

**Source:** `Claims\Components\Generic Peril\Interface\Generic Peril\`
**Entry class file:** `iCLMPeril.vb` (28.5KB)
**ILocalInterface:** No

**Private domain fields:**

| Field | Type | Description |
|---|---|---|
| `m_oPartySummary` | Object | Party summary data |
| `m_oPolicySummary` | Object | Policy summary data |
| `m_oRisk` | `iPMURiskWrapper.Interface_Renamed` | Risk wrapper interface |
| `m_bIsRI2007Enabled` | Boolean | RI 2007 enabled |
| `m_bOpenClaimNoTrans` | Boolean | Open claim no-transaction mode |
| `m_bShowCoinsurers` | Boolean | Show co-insurers tab |

**Embedded user controls:** `uctCLMReserve`, `uctCLMPayment`

---

#### iCLMLossSchedule — Loss Schedule

**Source:** `Claims\Components\Loss Schedule\Interface\Loss Schedule\`
**Entry class file:** `iCLMLossSchedule.vb` (5.1KB)
**ILocalInterface:** No

**Custom properties:**

| Property | Type | Description |
|---|---|---|
| `Status` | Integer | Return status |
| `LossSchedule` | Boolean | Loss schedule mode |
| `LossScheduleTypeId` | Integer | Loss schedule type ID |

**Private domain fields:**

| Field | Type | Description |
|---|---|---|
| `m_oBusiness` | `bCLMLossSchedule.Business` | Loss schedule business logic |

**Type constants:** `ACLSTypeMVPC` (motor vehicle property claim), `ACLSTypeGeneral` (general)
**Sub-forms:** `frmAdd`, `frmAssign`, `frmLossScheduleType`

---

### 4.11 Complete File Inventory — All Interface Folders

| Component Folder | Key .vb Files | Size (KB) |
|---|---|---|
| Authorise Payments | `iCLMAuthorisePaymentsCls.vb`, `iCLMAuthorisePayments.vb`, `iGeneral.vb`, `iNavigatorV3.vb` | 19.6, 5.7, 7.9, 14.6 |
| Case | `iCLMCaseHistoryFrm.vb`, `iCLMCaseHistoryCls.vb`, `iCLMCaseHistory.vb` | 33.2, 11.0, 2.5 |
| Change Claim Status | `iCLMChangeClaimStatusCls.vb`, `iNavigatorV3.vb` | 108.4, 13.3 |
| Check Deferred RI | `iCLMCheckDeferredRICls.vb`, `iNavigatorV3.vb` | 18.1, 13.3 |
| Check Unpaid Premium | `iCLMCheckUnpaidPremiumInterface.vb`, `iCLMCheckUnpaidPremiumFrm.vb`, `iGeneral.vb`, `iNavigatorV3.vb` | 24.2, 51.5, 11.4, 14.7 |
| Claim Address | `iCLMAddressInterface.vb`, `iCLMAddressFrm.vb`, `iCLMAddress.vb`, `iNavigatorV2.vb` | 32.2, 53.3, 9.1, 18.1 |
| Claim Diary | `iCLMCreateClaimDiaryCls.vb`, `iNavigatorV3.vb` | 21.3, 13.3 |
| Claim Letter | `iCLMGetClaimLetterCls.vb`, `iNavigatorV3.vb` | 20.8, 13.2 |
| Claim Payment Process | `iCLMPaymentProcess.vb`, `iCLMPaymentProcessMod.vb`, `iNavigatorV3.vb` | 24.2, 1.9, 14.7 |
| Close Claim | `iCLMCloseClaimCls.vb`, `iNavigatorV3.vb` | 20.6, 13.3 |
| Coinsurance Recoveries | `iCLMCoInsuranceRecoveries.vb`, `iCLMCoinsuranceRecoveriesInterface.vb`, `iCLMCoInsuranceDetails.vb`, `iCoinsuranceRecoveries.vb`, `iNavigatorV3.vb` | 62.9, 27.1, 17.2, 3.9, 14.6 |
| Define Fields | `iCLMDefnFldsFrm.vb`, `iCLMDefnFldsCls.vb`, `iCLMDefnFldsFrm.Designer.vb`, `iNavigatorV3.vb` | 137.2, 27.1, 28.6, 13.3 |
| Document Production | `iCLMGetClaimDocuments.vb`, `iNavigatorV3.vb` | 41.3, 13.3 |
| Financial Summary | `iCLMFinSummFrm.vb`, `iCLMFinSummCls.vb`, `iNavigatorV3.vb` | 71.1, 26.7, 13.2 |
| Find Case | `iCLMFindCaseFrm.vb`, `iCLMFindCaseCls.vb`, `iNavigatorV3.vb` | 104.3, 27.3, 14.8 |
| Find Claim | `iCLMFindClaimFrm.vb`, `iCLMFindClaimCls.vb`, `iNavigatorV3.vb` | 154.9, 33.9, 13.5 |
| Find Insurance | `iSIRFindInsuranceFrm.vb`, `iSIRFindInsurance.vb`, `iSIRFindInsuranceMod.vb`, `iNavigatorV3.vb` | 133.9, 32.6, 9.8, 13.3 |
| Find Party | `iCLMFindPartyFrm.vb`, `iCLMFindPartyCls.vb`, `iNavigatorV3.vb` | 85.4, 36.4, (none) |
| Generic Peril | `iCLMPeril.vb`, `iCLMPerilMod.vb`, `Igeneral.vb`, `iNavigatorV3.vb` | 28.5, 11.8, 73.5, 14.0 |
| Information Checklist | `iCLMInfoChklstFrm.vb`, `iCLMInfoChklst.vb`, `iCLMRequirement.vb`, `iCLMService.vb`, `iNavigatorV3.vb` | 87.1, 31.9, 45.1, 46.2, 14.6 |
| List Payments | `iCLMListPayments.vb`, `iCLMListReceiptsInterface.vb`, `iNavigatorV3.vb` | 22.6, 21.6, 14.7 |
| List Receipts | `iCLMListReceipts.vb`, `iCLMListReceiptsInterface.vb`, `iNavigatorV3.vb` | 22.6, 21.6, 14.7 |
| Loss Schedule | `iCLMLossSchedule.vb`, `iCLMLossScheduleCls.vb`, `iCLMItemDetails.vb` | 5.1, 7.4, 9.6 |
| Open Claim | `iOpenClaim.vb`, `iOpenClaimMod.vb`, `iNavigatorV3.vb` | 52.5, 32.1, 12.7 |
| Payment Method | `iCLMPaymentMethodFrm.vb`, `iCLMPaymentMethodCls.vb`, `iNavigatorV3.vb` | 112.4, 31.3, 13.3 |
| Peril Type | `iCLMPerilTypeFrm.vb`, `iCLMPeriltype.vb`, `iCLMPerilTypeMod.vb`, `iNavigatorV3.vb` | 48.1, 26.5, 3.5, 13.3 |
| Peril Type Reserve Type | `iCLMPerilTypeReserveTypeFrm.vb`, `iCLMPerilTypeReserveTypeCls.vb`, `iNavigatorV3.vb` | 74.5, 21.6, 13.3 |
| Recovery | `iCLMRecoveryInterface.vb`, `iCLMRecovery.vb`, `iNavigatorV3.vb` | 35.1, 12.1, 14.6 |
| Reinsurance Recoveries | `iCLMReinsuranceRI2007.vb`, `iCLMReinsuranceFrm.vb`, `iCLMReinsurance.vb`, `iCLMReinsuranceMod.vb`, `iNavigatorV3.vb` | 79.1, 48.4, 24.9, 2.4, 13.2 |
| Reserve Definition | `iCLMResvDefnFrm.vb`, `iCLMResvDefnCls.vb`, `iNavigatorV3.vb` | 80.8, 25.0, 13.3 |
| Risk Details | `iCLMRiskDetailsFrm.vb`, `iCLMRiskDetailsInterface.vb`, `iCLMRiskDetails.vb`, `iNavigatorV3.vb` | 211.7, 62.1, 9.0, 14.6 |
| Risk Type | `iCLMRiskTypeFrm.vb`, `iCLMRisktype.vb`, `iCLMRiskTypeMod.vb`, `iNavigatorV3.vb` | 56.8, 28.2, 3.1, 13.3 |
| Risk Type Info Checklist | `iCLMRiskTypeInfoChecklistFrm.vb`, `iCLMRiskTypeInfoChecklistInterface.vb`, `iCLMRiskTypeInfoChecklist.vb`, `iNavigatorV3.vb` | 73.1, 23.4, 4.8, 12.7 |
| Salvage Recovery | `iCLMSalvageMain.vb`, `iCLMSalvageRecoveryInterface.vb`, `iCLMSalvageRecovery.vb`, `iNavigatorV3.vb` | 189.5, 25.8, 7.5, 14.6 |
| Third Party Recovery | `iCLMThirdPartyMain.vb`, `iCLMThirdPartyRecoveryInterface.vb`, `iCLMThirdPartyRecovery.vb`, `iNavigatorV3.vb` | 181.7, 27.5, 7.9, 14.5 |
| Unlock | `iCLMUnlockCls.vb`, `iNavigatorV3.vb` | 15.9, 13.3 |

