# Orion Accounting Module — UI Interface & User Controls Reference

> **Related documentation:** `orion-components-reference.md` — full `bACT*` business component reference (methods, stored procedures, business logic).

## 1. Architecture Overview

The Orion accounting module follows the same **Interface_Renamed pattern** used throughout the Pure Insurance platform. Each functional area exposes a `ProgId("Interface_Renamed_NET.Interface_Renamed")` class as the public API surface. The Navigator roadmap calls `Initialise()` then reads/writes properties on the interface class to pass context in and out.

### Standard Interface Lifecycle

```
Navigator Roadmap
  → Interface_Renamed.Initialise()   — creates ObjectManager, gets bACT*.Form via GetInstance()
  → Set input properties              — context passed IN (AccountID, CashListID, etc.)
  → Interface_Renamed.Start()         — launches the form (ShowDialog)
  → Read output properties            — context passed OUT (Status, navigate keys, result IDs)
```

### Key Patterns

| Pattern | Detail |
|---|---|
| `ILocalInterface` | Implemented by most interfaces (exposes `Initialise()` to roadmap host) |
| `PMProductFamily` | Returns `pmePFOrion` for Orion; returns `pmePFSiriusSolutions` for shared components |
| `GetInstance()` | Called in `Initialise()`: `g_oObjectManager.GetInstance(o, "bACTXxx.Form", "ClientManager")` |
| `Status` | Exit code: `PMTrue` = OK/Save, `PMCancel` = user cancelled, `PMFalse`/`PMError` = failure |
| `StepStatus` | Used by roadmap-driven workflows to indicate which step completed |
| `Task` | Reflects the action taken (`PMView`, `PMEdit`, `PMAdd`, `PMDelete`) |
| `Navigate` | Navigate button result (`PMNavigateNotRequired`, `PMNavigatePrevious`, `PMNavigateNext`) |

---

## 2. Interface Component Index

| # | Component | Entry Class File | Form File(s) | Business Component(s) | PMProductFamily |
|---|---|---|---|---|---|
| 1 | Account | `iACTAccount.vb` | `iACTAccountFrm.vb` | `bACTAccount.Form`, `bACTExplorer.Form` | Orion |
| 2 | AccountExplorer | `iACTExplorer.vb` | `iACTExplorerFrm.vb` | `bACTExplorer.Form` | Orion |
| 3 | AgentSelect | `iACTAgentSelectCls.vb` | `iACTAgentSelect.vb` | (Sirius party lookup) | SiriusSolutions |
| 4 | AgentSummary/CommissionPayments | `Interface.vb` | `frmInterface.vb` | `bACTCommissionPayments.Business` | SiriusSolutions |
| 5 | Allocate | `iACTAllocateCls.vb` | `iACTAllocateFrm.vb` | `bACTCurrencyConvert.Form`, `bACTPeriod.Form`, `bACTUserAuthorities.Business` | Orion |
| 6 | Allocation | `iACTAllocationCls.vb` | `frmInterface.vb` | `bACTAllocation` | Orion |
| 7 | AllocationDecision | `iACTAllocationDecisionCls.vb` | (no form) | — | Orion |
| 8 | Bank | `iACTBankCls.vb` | `iACTBankFrm.vb` | `bACTBank.Form` | Orion |
| 9 | BankAccount | `iACTBankAccountInterface.vb` | `iACTBankAccountFrm.vb` | `bACTBankAccount.Form` | Orion |
| 10 | BankReconciliation | `iACTBankReconciliation.vb` | `iACTBankReconciliationFrm.vb` | `bACTBankReconciliation` | Orion |
| 11 | Budget | `iACTBudgetCls.vb` | `iACTBudget.vb` | `bACTBudget` | Orion |
| 12 | BudgetDetail | `iACTBudgetDetailCls.vb` | `iACTBudgetDetail.vb` | `bACTBudgetDetail` | Orion |
| 13 | CashList | `iACTCashListCls.vb` | `iACTCashListFrm.vb`, `iACTBanking.vb`, `iACTAdjustment.vb` | `bACTCashList.Form` | Orion |
| 14 | CashListItem | `iACTCashListItemCls.vb` | `iACTCashListItemDetails.vb`, `iACTCashListItemList.vb` | `bACTCashlistitem.Form`, `bACTCashListPost.Automated`, `bACTAllocate.Business`, `bACTInsurerPaymentAllocate.Business`, `bACTDocumentPost.Form`, `bACTDocumentReversal.Business`, `bACTCashList.Form`, `bACTAccount.Form`, `bACTAutoNumber.Business`, `bACTCashListDrawer.Business`, `bACTUserAuthorities.Business` | Orion |
| 15 | CashReceipt | `Interface.vb` | (no modal form) | `bACTAccount.Form` | Orion |
| 16 | ChequeProduction | `iACTChequeProduction.vb` | `iACTChequeProductionFrm.vb` | `bACTChequeProduction` | Orion |
| 17 | ClaimPaymentProcessing | `Interface.vb` | `frmInterface.vb`, `frmPaymentProcessed.vb` | `bACTCashListPost.Automated` | Orion |
| 18 | CLIRepeatDecision | `iACTCLIRepeatDecisionCls.vb` | (no form) | — | Orion |
| 19 | CommissionMovement | `iACTCommissionMovementCls.vb` | `iACTCommissionMovement.vb` | `bACTCommissionPost.Business` | Orion |
| 20 | Company | `iACTCompany.vb` | `iACTCompanyDetails.vb`, `iACTCompanyList.vb` | `bACTCompany.Form` | Orion |
| 21 | CreditControl | `iACTCreditControlMaintCls.vb` | `frmDetails.vb`, `frmInterface.vb`, `frmStep.vb` | `bACTCreditControl` | Orion |
| 22 | CreditControlProcessing | `iACTCreditControlProcessing.vb` | `frmInterface.vb` | `bACTCreditControl` | Orion |
| 23 | CurrencyRate | `iACTCurrencyRateCls.vb` | `iACTCurrencyRateFrm.vb` | `bACTCompany.Form`, `bACTCompanyCurrency.Form`, `bACTCurrency.Form`, `bACTCurrencyRate.Form` | Orion |
| 24 | Document | `iACTDocument.vb` | `iACTDocumentFrm.vb` | `bACTDocument.Form`, `bACTDocumentPost.Form`, `bACTAuditSet.Form`, `bACTPeriod.Form`, `bACTUserAuthorities.Business` | Orion |
| 25 | ExportCashListItems | `Interface.vb` | `frmExportCashBook.vb` | `bACTExportCashListItems.Business` | Orion |
| 26 | FindAccount | `iACTFindAccountCls.vb` | `iACTFindAccount.vb` | `bACTAccount.Form` | Orion |
| 27 | FindBank | `iACTFindBankCls.vb` | `iACTFindBank.vb` | `bACTBank.Form` | Orion |
| 28 | FindBudget | `iACTFindBudgetCls.vb` | `iACTFindBudget.vb` | `bACTBudget` | Orion |
| 29 | FindCashList | `iACTFindCashList.vb` | `iACTFindCashListFrm.vb` | `bACTCashList.Form` | Orion |
| 30 | FindCashListItem | `iACTFindCashListItemCls.vb` | `iACTFindCashListItemFrm.vb` | `bACTCashlistitem.Form` | Orion |
| 31 | FindDocument | `iACTFindDocument.vb` | `iACTFindDocumentFrm.vb` | `bACTDocument.Form` | Orion |
| 32 | FindInvoice | `iACTFindInvoice.vb` | `iACTFindInvoiceFrm.vb` | `bACTInvoice.Form` | Orion |
| 33 | FindTransaction | `iACTFindTransaction.vb` | `iACTFindTransactionFrm.vb` | `bACTTransDetail` | Orion |
| 34 | ImportExport | `iInterface.vb` | `iACTInterface.vb`, `frmReceiptImport.vb`, `frmViewGL.vb` | `bACTExportCashListItems.Business`, exchange/import classes | SiriusSolutions |
| 35 | InsurerPayment | `iACTInsurerPayment.vb` | `iACTInsurerPaymentFrm.vb` | `bACTInsurerPayment`, `bACTInsurerPaymentAllocate.Business` | Orion |
| 36 | InsurerPaymentGroup | `iACTInsurerPaymentGroupCls.vb` | `iACTInsurerPaymentGroup.vb` | `bACTInsurerPayment` | Orion |
| 37 | InsurerPaymentGroups | `iACTInsurerPaymentGroupsInterface.vb` | `iACTInsurerPaymentGroups.vb` | `bACTInsurerPayment` | Orion |
| 38 | Ledger | `iACTLedgerCls.vb` | `iACTLedgerDetails.vb`, `iACTLedgerList.vb` | `bACTLedger` | Orion |
| 39 | MaintainMediaTypeStatus | `iACTMaintainMediaTypeStatus.vb` | `iACTMaintainMediaTypeStatusFrm.vb` | `bACTMediaType` | Orion |
| 40 | PaymentMaintenance | `iACTPaymentMaintenance.vb` | `iACTPaymentMaintenanceFrm.vb` | `bACTCashlistitem.Form`, `bACTCashListPost.Automated` | Orion |
| 41 | Period | `iACTPeriodCls.vb` | `iACTPeriodAdd.vb`, `iACTPeriodDetails.vb`, `iACTPeriodList.vb` | `bACTPeriod.Form` | Orion |
| 42 | PeriodEnd | `iACTPeriodEnd.vb` | `iACTPeriodEndFrm.vb` | `bACTPeriodEnd`, `bACTAuditSet.Form` | Orion |
| 43 | PurchaseInvoice | `iACTInvoiceInterface.vb` | `iACTInvoiceFrm.vb` | `bACTInvoice.Form`, `bACTDocument.Form`, `bACTDocumentPost.Form` | Orion |
| 44 | PurchaseInvoiceCreditNote | `Interface.vb` | (no form) | `bACTInvoice.Form` | Orion |
| 45 | Transaction | `iACTTransaction.vb` | `iACTTransactionFrm.vb` | `bACTAccount.Form`, `bACTDocument.Form`, `bACTDocumentPost.Form`, `bACTAuditSet.Form`, `bACTCompanyCurrency.Form`, `bACTCurrencyConvert.Form`, `bACTPeriod.Form`, `bACTUserAuthorities.Business` | Orion |
| 46 | UserAuthorities | `iACTUserAuthoritiesInterface.vb` | `iACTUserAuthoritiesFrm.vb`, `frmAuthorities.vb` | `bACTUserAuthorities.Business` | Orion |

---

## 3. Core Accounting Interfaces

### 3.1 Account

**File:** `Orion\Components\Account\Interface\iACTAccount\iACTAccount.vb` (21.8 KB)  
**Form:** `iACTAccountFrm.vb` (167.3 KB)  
**Purpose:** Add, edit, view, and navigate ledger account records. Supports multi-currency accounts, debit/credit restrictions, and ledger type assignment.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `CompanyID` | ReadWrite | Integer | Company/source context (defaults to `g_iSourceID`) |
| `AccountID` | ReadWrite | Integer | Primary key of the account to open or returned after add |
| `Task` | ReadOnly | Integer | Action taken (`PMView`, `PMEdit`, `PMAdd`) |
| `Navigate` | ReadOnly | Integer | Navigator button used |
| `ProcessMode` | ReadOnly | Integer | Process mode |
| `TransactionType` | ReadOnly | String | Transaction type |
| `EffectiveDate` | ReadOnly | Date | Effective date |
| `PMAuthorityLevel` | ReadWrite | Integer | Authority level for restricted actions |
| `CallingAppName` | WriteOnly | String | Calling application identifier |

#### Form Methods (`iACTAccountFrm.vb`)

| Method | Description |
|---|---|
| `GetBusiness()` | Instantiates `bACTAccount.Form` and `bACTExplorer.Form` via object manager |
| `BusinessToInterface()` | Loads property values from business object into form controls |
| `InterfaceToBusiness()` | Saves form control values back into the business object |
| `BusinessToData()` | Persists business object data to the database (calls `bACTAccount`) |
| `InterfaceToData()` | Combined UI-to-DB write operation |
| `SetFieldValidation()` | Applies validation rules and field enable/disable based on task mode |
| `SetInterfaceDefaults()` | Sets default control values for an Add operation |
| `SetFirstLastControls()` | Enables/disables Previous/Next navigator buttons |
| `SetTabDefaults()` | Configures tab visibility and ordering |
| `DisplayCaptions()` | Sets localised label captions from resource strings |
| `GetLookupValues()` | Populates combo boxes from lookup tables (currencies, ledger types, etc.) |
| `GetLedgerDetails()` | Retrieves and displays the associated ledger record |
| `GetLookupDetails()` | Refreshes a specific lookup field value |
| `DisplayLookupDetails()` | Shows result returned from a child find screen |
| `CreateWorkManagerMemo()` | Records a WorkManager audit entry when account record is modified |
| `cmdOK_Click()` | Saves and closes; sets `Status = PMTrue` |
| `cmdCancel_Click()` | Closes without saving; sets `Status = PMCancel` |
| `cmdNavigate_Click()` | Handles Previous/Next navigation between records |
| `cmdPrevious_Click()` | Navigate to previous account in the browse list |
| `cmdNext_Click()` | Navigate to next account in the browse list |

**Business Components Used:** `bACTAccount.Form`, `bACTExplorer.Form`

---

### 3.2 AccountExplorer

**File:** `Orion\Components\AccountExplorer\Interface\iACTExplorer\iACTExplorer.vb` (28.6 KB)  
**Forms:** `iACTExplorerFrm.vb` (124.1 KB), `iACTExplorerCreateAccount.vb` (31.9 KB), `iACTExplorerMapFolder.vb` (3.1 KB), `iACTExplorerSecurity.vb` (2.7 KB)  
**Purpose:** Chart-of-accounts tree navigator. Allows browsing, mapping, and creating accounts in a hierarchical structure.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `CompanyID` | ReadWrite | Integer | Company context |
| `AccountID` | ReadWrite | Integer | Selected/created account ID |
| `LedgerID` | ReadWrite | Integer | Filter to specific ledger |
| `LedgerTypeID` | ReadWrite | Integer | Ledger type filter |
| `StartKey` | ReadWrite | String | Initial tree node to expand to |
| `ShortCode` | ReadWrite | String | Account short code |
| `FullKey` | ReadWrite | String | Full account tree key |
| `AccountName` | ReadOnly | String | Name of selected account |
| `MappingID` | ReadWrite | Integer | Mapping ID for account linking |
| `ReadOnly_Renamed` | ReadWrite | Integer | When non-zero, disables editing |
| `PMAuthorityLevel` | ReadWrite | Integer | Authority level |
| `Status` | ReadOnly | Integer | Exit status |

**Business Components Used:** `bACTExplorer.Form`

---

### 3.3 CashList

**File:** `Orion\Components\CashList\Interface\iACTCashList\iACTCashListCls.vb` (54 KB)  
**Forms:** `iACTCashListFrm.vb` (85.2 KB), `iACTBanking.vb` (140.4 KB), `iACTAdjustment.vb` (40.8 KB), `frmList.vb`, `frmListAdjusts.vb`, `frmSimpleBanking.vb`  
**Purpose:** Cash list header management — creates and manages receipt/payment cash lists. Supports banking workflow, adjustments, and multi-currency.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `ScreenMode` | ReadWrite | String | Start mode: `""` default, `"start_banking"`, `"start_list"` |
| `MulitCurrencyFlag` | ReadWrite | Boolean | Whether multi-currency mode is active |
| `PMAuthorityLevel` | ReadWrite | Integer | Authority level |
| `Task` | ReadOnly | Integer | Task performed |
| `Navigate` | ReadOnly | Integer | Navigate exit |
| `ProcessMode` | ReadOnly | Integer | Process mode |
| `TransactionType` | ReadOnly | String | Transaction type |
| `EffectiveDate` | ReadOnly | Date | Effective date |
| `StepStatus` | ReadOnly | String | Roadmap step status |

#### Key Private Fields

| Field | Description |
|---|---|
| `m_lCashlistID` | Cash list primary key |
| `m_lCashlistTypeID` | Type: receipt list, payment list, banking list |
| `m_iCurrencyID` / `m_iDepositCurrencyID` | Currency IDs |
| `m_sCashListRef` | Reference string |
| `m_sDebitCredit` | "D" or "C" |
| `m_lAccountId` | Associated account |
| `m_cAmount` | Total amount |
| `m_sDocumentRef` | Document reference |
| `m_lMediaTypeId` | Media type (cheque, BACS, etc.) |
| `m_iCompanyID` | Company |

#### Form Methods (`iACTCashListFrm.vb`)

| Method | Description |
|---|---|
| `GetBusiness()` | Gets `bACTCashList.Form` instance |
| `BusinessToInterface()` | Loads cash list from business object into form |
| `InterfaceToBusiness()` | Saves form changes to business object |
| `SetStatus(sProcessStatus, sMapStatus, sStepStatus)` | Sets roadmap step statuses |
| `GetCompany(m_iCompanyID)` | Retrieves and validates the company record |
| `SetFieldValidation()` | Enables/disables controls based on task and cash list type |
| `CheckCurrencyExistsInBranch()` | Validates the selected currency is configured for the branch |
| `ActionCashListTypeSelect()` | Handles logic when cash list type is changed |
| `GetConvertedPaymentAmount(v_iCurrencyIdTO)` | Calculates currency-converted payment total |
| `CheckCurrencyRatesExists()` | Validates that exchange rates exist for the currency pair |

**Business Components Used:** `bACTCashList.Form`

---

### 3.4 CashListItem

**File:** `Orion\Components\CashListItem\Interface\iACTCashListItem\iACTCashListItemCls.vb` (48.6 KB)  
**Forms:** `iACTCashListItemDetails.vb` (546 KB), `iACTCashListItemList.vb` (446 KB), `frmWriteOffReason.vb`, `frmDifference.vb`, `frmConversions.vb`  
**Purpose:** Cash list item (individual payment/receipt line) management. The most complex interface in Orion. Handles payment entry, allocation, write-off, insurer payments, instalment plans, claim payments, letter printing, multi-currency conversions, BACS/cheque media, and credit card processing.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `Status` | ReadOnly | Integer | Exit status |
| `Task` | ReadOnly | Integer | Action taken |
| `Navigate` | ReadOnly | Integer | Navigator exit |
| `ProcessMode` | ReadOnly | Integer | Process mode |
| `TransactionType` | ReadOnly | String | Transaction type |
| `EffectiveDate` | ReadOnly | Date | Effective date |
| `PMAuthorityLevel` | ReadWrite | Integer | Authority level |
| `PartyBankId` | ReadWrite | Integer | Party bank ID for BACS/EFT payment routing |
| `Letters` | ReadWrite | Object | Array of letter type IDs to print on completion |
| `LetterPrint` | ReadWrite | Boolean | Whether letters should be printed |

#### Key Private Fields

| Field | Description |
|---|---|
| `m_lCashListItemID` | Item primary key |
| `m_lCashlistID` | Parent cash list |
| `m_lCashlistItemMode` | Entry mode (new, edit, view) |
| `m_lCashListTypeID` | Receipt or payment |
| `m_lAccountID` | Posting account |
| `m_iCurrencyID` | Currency |
| `m_lAllocationID` | Pre-set allocation |
| `m_bAllowAllocateButton` | Show/hide allocate button |
| `m_sActionkey` | Roadmap action key |
| `m_vAllocationIDs` | Array of allocation IDs |
| `m_cAmount` / `m_cWriteOffAmount` | Amounts |
| `m_bISWOFF` | Write-off flag |
| `m_bViaInsurerPayment` | Entry via insurer payment workflow |
| `m_bViaFinancePlan` | Entry via instalment plan |
| `m_bViaClaimPayment` | Entry via claims payment |
| `m_sPayeeName` / `m_sPayeeAccountCode` / `m_sPayeeSortCode` | Payee banking details |
| `m_lClaimPaymentId` | Associated claim payment |
| `m_vClaimPaymentIDs` | Array of claim payment IDs |
| `m_iCashListItemPaymentTypeID` | Payment type |
| `m_iCashListStatusId` | Status ID |
| `m_lApprovalType` | Approval workflow type |
| `m_sMediaRef` / `m_sDocumentRef` | Reference strings |
| `m_bdisplayCashPaymentProcess` | Display claims payment processing button |

**Business Components Used:** `bACTCashlistitem.Form`, `bACTCashList.Form`, `bACTCashListPost.Automated`, `bACTAllocate.Business`, `bACTInsurerPaymentAllocate.Business`, `bACTDocumentPost.Form`, `bACTDocumentReversal.Business`, `bACTAutoNumber.Business`, `bACTCashListDrawer.Business`, `bACTAccount.Form`, `bACTUserAuthorities.Business`

---

### 3.5 Transaction (Journal Document)

**File:** `Orion\Components\Transaction\Interface\iACTTransaction\iACTTransaction.vb` (27.5 KB)  
**Form:** `iACTTransactionFrm.vb` (143.5 KB)  
**Purpose:** General ledger journal entry — creates, posts, reverses, and manages recurring journal documents. Supports reversing entries and recurring document schedules.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `DocumentID` | WriteOnly | Integer | Document to open (pass IN) |
| `AccountingDate` | WriteOnly | Date | Accounting date for the entry |
| `ReversingDocument` | ReadWrite | Boolean | Whether this is a reversing entry |
| `ReversingDocumentID` | ReadWrite | Integer | ID of the document being reversed |
| `ReverseDate` | ReadWrite | Date | Date for reversal posting |
| `RecurringDocument` | ReadWrite | Boolean | Whether this is a recurring document |
| `Occurances` | ReadWrite | Integer | Number of recurring occurrences |
| `RecurringDocumentIDs` | ReadWrite | Object | Array of generated recurring doc IDs |
| `DocumentRef` | ReadWrite | String | Document reference string |

#### Form Methods (`iACTTransactionFrm.vb`)

| Method | Description |
|---|---|
| `Initialise()` | Initialises form, gets all business object instances |
| `Load_Renamed()` | Loads existing document data from business object into form |
| `ShowForm(lDisplayState)` | Displays the form in the specified display state |
| `InterfaceToBusiness()` | Writes form data to business objects before saving |

**Business Components Used:** `bACTAccount.Form`, `bACTDocument.Form`, `bACTDocumentPost.Form`, `bACTAuditSet.Form`, `bACTCompanyCurrency.Form`, `bACTCurrencyConvert.Form`, `bACTPeriod.Form`, `bACTUserAuthorities.Business`

---

### 3.6 Document

**File:** `Orion\Components\Document\Interface\iACTDocument\iACTDocument.vb` (32.2 KB)  
**Form:** `iACTDocumentFrm.vb` (111.6 KB)  
**Purpose:** Accounting document header management — records document metadata, posting status, and supports recurring and reversing document patterns.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `DocumentId` | ReadWrite | Integer | Document primary key |
| `DocumenttypeID` | WriteOnly | Integer | Document type filter |
| `DocumentDate` | ReadWrite | Date | Document date |
| `DocumentRef` | ReadWrite | String | Document reference |
| `Comment` | ReadWrite | String | Narrative comment |
| `Postingstatus` | ReadWrite | Integer | Posting status ID |
| `ReversingDocument` | ReadWrite | Boolean | Is a reversing document |
| `ReversingDocumentID` | ReadWrite | Integer | Document being reversed |
| `ReverseDate` | ReadWrite | Date | Reversal date |
| `RecurringDocument` | ReadWrite | Boolean | Is a recurring document |
| `Occurances` | ReadWrite | Integer | Number of recurrences |
| `RecurringDocumentIDs` | ReadWrite | Object | Generated recurring document IDs |
| `RecurringDocumentDates` | ReadWrite | Object | Dates for recurring documents |
| `CompanyID` | ReadWrite | Integer | Company context |

**Business Components Used:** `bACTDocument.Form`, `bACTDocumentPost.Form`, `bACTAuditSet.Form`, `bACTPeriod.Form`, `bACTUserAuthorities.Business`

---

### 3.7 Allocate

**File:** `Orion\Components\Allocate\Interface\iACTAllocate\iACTAllocateCls.vb` (32.6 KB)  
**Form:** `iACTAllocateFrm.vb` (141.7 KB)  
**Purpose:** Allocate payments/receipts against outstanding transactions (debits against credits). Full allocation, partial allocation, and write-off support.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `TransdetailID` | ReadOnly | Integer | Selected transaction detail ID result |
| `AccountID` | ReadWrite | Integer | Account to allocate against |
| `DocumentRef` | ReadWrite | String | Document reference filter |
| `InsuranceRef` | ReadWrite | String | Insurance/policy reference filter |
| `AllocationTransType` | ReadWrite | String | Transaction type filter for allocation |
| `AllocationID` | ReadWrite | Integer | Pre-selected allocation ID |
| `BatchID` | ReadWrite | Integer | Cash list batch context |
| `CashListTypeID` | ReadWrite | Integer | Cash list type context |
| `BranchID` | ReadWrite | Integer | Branch filter |
| `OutstandingOnly` | ReadWrite | Boolean | Show only unallocated items |
| `CompanyID` | WriteOnly | Integer | Company context |
| `TypeOfBusiness` | ReadOnly | String | Returns business type of selected transaction |

#### Form Methods (`iACTAllocateFrm.vb`)

| Method | Description |
|---|---|
| `GetBusiness()` | Gets required business object instances |
| `DataToInterface()` | Loads outstanding transaction list from data into the grid |
| `DataToProperties()` | Copies selected grid row values to form property fields |
| `DisplayLookupDetails()` | Shows detail of a lookup result (account, etc.) |
| `PopulateAccountCode([r_lAccountID])` | Resolves and displays the account code/name |
| `GetAccountID(r_lAccountID)` | Returns the account ID from the currently selected row |

**Business Components Used:** `bACTCurrencyConvert.Form`, `bACTPeriod.Form`, `bACTUserAuthorities.Business`

---

### 3.8 Allocation

**File:** `Orion\Components\Allocation\Interface\iACTAllocation\iACTAllocationCls.vb` (20.1 KB)  
**Form:** `frmInterface.vb` (allocation)  
**Purpose:** Manages allocation records — displays and edits the allocation between transactions. Used to view all allocations on a posting.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `AllocationArray(,)` | ReadWrite | Object | 2D array of allocated transaction pairs |
| `CompanyID` | ReadWrite | Integer | Company context |
| `AccountID` | ReadWrite | Integer | Account context |
| `CashListTypeID` | ReadWrite | Integer | Cash list type |
| `CashListItemID` | ReadWrite | Integer | Cash list item context |
| `SelectedCurrencyId` | WriteOnly | Integer | Currency filter |
| `SelectedSourceId` | WriteOnly | Integer | Source ID filter |
| `Task` | ReadOnly | Integer | Task |
| `Navigate` | ReadOnly | Integer | Navigate exit |

**Business Components Used:** `bACTAllocation`

---

## 4. Banking Interfaces

### 4.1 Bank

**File:** `Orion\Components\Bank\Interface\iACTBank\iACTBankCls.vb` (22.7 KB)  
**Form:** `iACTBankFrm.vb` (97.2 KB)  
**Purpose:** Maintain bank records — name, branch, sort code, and account details. Supports add, edit, view, and delete (with cascade to bank accounts).

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `BankCode` | ReadOnly | String | Bank code after selection |
| `BankID` | ReadWrite | Integer | Bank primary key |
| `CompanyID` | WriteOnly | Integer | Company context |
| `Task` | ReadWrite | Integer | Requested task |
| `PMProductFamily` | ReadOnly | Integer | Returns `pmePFOrion` |

#### Form Methods (`iACTBankFrm.vb`)

| Method | Description |
|---|---|
| `GetBusiness()` | Gets `bACTBank.Form` instance |
| `SetFieldValidation()` | Enables/disables fields based on task |
| `BusinessToInterface()` | Loads bank data from business object to form |
| `InterfaceToBusiness()` | Saves form to business object |
| `DisplayLookupDetails()` | Shows lookup result in form |
| `DeleteBankAccounts()` | Cascade-deletes all bank accounts for this bank before bank deletion |

**Business Components Used:** `bACTBank.Form`

---

### 4.2 BankAccount

**File:** `Orion\Components\BankAccount\Interface\iACTBankAccount\iACTBankAccountInterface.vb` (22.6 KB)  
**Form:** `iACTBankAccountFrm.vb` (117.7 KB)  
**Purpose:** Maintain individual bank account records including account number, sort code, IBAN, account type, and company mappings.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `BankID` | ReadWrite | Integer | Parent bank ID |
| `BankAccountId` | ReadWrite | Integer | Bank account primary key |
| `BankAccountNo` | ReadOnly | String | Account number (returned after save) |
| `Code` | ReadOnly | String | Short code |
| `BankAccountName` | ReadOnly | String | Account name |
| `Description` | ReadOnly | String | Description |
| `BankAccountType` | ReadWrite | Integer | Type ID (current, savings, etc.) |
| `UniqueId` | ReadWrite | Integer | Unique identifier for account |
| `ScreenHierarchy` | ReadWrite | String | Navigation hierarchy |
| `BankCode` | ReadWrite | String | Parent bank code |
| `CompanyID` | WriteOnly | Integer | Company context |
| `PMProductFamily` | ReadOnly | Integer | Returns `pmePFOrion` |

#### Form Methods (`iACTBankAccountFrm.vb`)

| Method | Description |
|---|---|
| `GetBusiness()` | Gets `bACTBankAccount.Form` instance |
| `SetFieldValidation()` | Enables/disables fields |
| `BusinessToInterface()` | Loads bank account data into form |
| `InterfaceToBusiness()` | Saves form to business object |
| `DisplayLookupDetails()` | Shows lookup result |
| `GetAndDisplayRules()` | Retrieves and applies validation rules for this account type |
| `FindAccount(r_lAccountId, r_sAccountName)` | Opens account lookup and returns selected account ID |

**Business Components Used:** `bACTBankAccount.Form`

---

### 4.3 BankReconciliation

**File:** `Orion\Components\BankReconciliation\Interface\iACTBankReconciliation\iACTBankReconciliation.vb` (27.8 KB)  
**Form:** `iACTBankReconciliationFrm.vb` (138 KB)  
**Purpose:** Bank statement reconciliation — matches cash list items against bank statement entries to identify discrepancies.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `SourceArray` | ReadWrite | Object | Input array of transactions to reconcile |
| `StepStatus` | ReadOnly | String | Roadmap step completion status |

---

### 4.4 ChequeProduction

**File:** `Orion\Components\ChequeProduction\Interface\iACTChequeProduction\iACTChequeProduction.vb` (33.6 KB)  
**Form:** `iACTChequeProductionFrm.vb` (113.9 KB)  
**Purpose:** Batch cheque production — generates, numbers, and prints cheques from a prepared payment list.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `SourceArray` | ReadWrite | Object | Array of payment items selected for cheque printing |
| `ChequeArray` | ReadWrite | Object | Output array of produced cheques with numbers |
| `StepStatus` | ReadOnly | String | Step completion status |
| `iBranchID` | ReadWrite | Integer | Branch context |
| `AccountID` | ReadWrite | Integer | Bank account to draw from |
| `AccountCode` | ReadOnly | String | Account code after selection |
| `PartyCnt` | ReadWrite | Integer | Number of parties in the batch |

---

## 5. Insurer Payment Interfaces

### 5.1 InsurerPayment

**File:** `Orion\Components\InsurerPayment\Interface\iACTInsurerPaymentSFU\iACTInsurerPayment.vb` (31.5 KB)  
**Form:** `iACTInsurerPaymentFrm.vb` (418.1 KB)  
**Purpose:** Insurer payment processing — creates and manages premium payments to insurers. Handles batch payment grouping, allocation to policies, and payment file generation. Largest form in Orion at 418KB.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `PMAuthorityLevel` | ReadWrite | Integer | Authority level |
| `Task` | ReadWrite | Integer | Task to perform |
| `Navigate` | ReadOnly | Integer | Navigator exit |
| `ProcessMode` | ReadOnly | Integer | Process mode |
| `TransactionType` | ReadOnly | String | Transaction type |
| `EffectiveDate` | ReadOnly | Date | Effective date |
| `StepStatus` | ReadOnly | String | Step completion status |
| `SourceArray` | ReadOnly | Object | Output array of processed payment items |

**Business Components Used:** `bACTInsurerPayment`, `bACTInsurerPaymentAllocate.Business`

---

### 5.2 InsurerPaymentGroups

**File:** `Orion\Components\InsurerPaymentGroups\Interface\iACTInsurerPaymentGroups\iACTInsurerPaymentGroupsInterface.vb` (19.7 KB)  
**Form:** `iACTInsurerPaymentGroups.vb` (51.1 KB)  
**Purpose:** Container for insurer payment group management — displays a list of insurer payment groups and allows drill-down to individual groups.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `PartyCnt` | ReadWrite | Integer | Number of parties/groups |
| `AccountId` | ReadWrite | Integer | Account context |
| `StepStatus` | ReadWrite | String | Roadmap step status |
| `PMProductFamily` | ReadOnly | Integer | Returns `pmePFOrion` |
| `PMAuthorityLevel` | WriteOnly | Integer | Authority level |

---

### 5.3 InsurerPaymentGroup

**File:** `Orion\Components\InsurerPaymentGroups\Interface\iACTInsurerPaymentGroup\iACTInsurerPaymentGroupCls.vb` (20.7 KB)  
**Form:** `iACTInsurerPaymentGroup.vb` (45.9 KB)  
**Purpose:** Individual insurer payment group record — maintains group configuration for batching insurer payments.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `GroupID` | ReadWrite | Integer | Group primary key |
| `CompanyID` | ReadWrite | Integer | Company |
| `GroupDesc` | ReadWrite | String | Group description |
| `CompanyDesc` | ReadWrite | String | Company description |
| `Business` | WriteOnly | Object | Pre-initialised business object to inject |
| `StepStatus` | ReadWrite | String | Step status |
| `PMProductFamily` | ReadOnly | Integer | Returns `pmePFOrion` |
| `PMAuthorityLevel` | WriteOnly | Integer | Authority level |

---

### 5.4 PaymentMaintenance

**File:** `Orion\Components\Payment Maintenance\Interface\iACTPaymentMaintenance\iACTPaymentMaintenance.vb` (33.1 KB)  
**Form:** `iACTPaymentMaintenanceFrm.vb` (107.5 KB), `iACTCancelPayment.vb` (22.1 KB)  
**Purpose:** View and maintain existing payment records — cancel, reverse, or amend payment details. Supports both regular and claim payments.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `Task` | ReadOnly | Integer | Task |
| `Navigate` | ReadOnly | Integer | Navigate exit |
| `ProcessMode` | ReadOnly | Integer | Process mode |
| `TransactionType` | ReadOnly | String | Transaction type |
| `EffectiveDate` | ReadOnly | Date | Effective date |
| `StepStatus` | ReadOnly | String | Step status |
| `SourceArray` | ReadOnly | Object | Array of processed payment records |

**Business Components Used:** `bACTCashlistitem.Form`, `bACTCashListPost.Automated`

---

## 6. Search/Find Interfaces

### 6.1 FindAccount

**File:** `Orion\Components\FindAccount\Interface\iACTFindAccount\iACTFindAccountCls.vb` (45.8 KB)  
**Form:** `iACTFindAccount.vb` (109.3 KB)  
**Purpose:** Account search and selection screen. Supports full/short code lookup, wildcard search, and filtering by ledger, type, branch, and company.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `AccountID` | ReadWrite | Integer | Pre-select or returned account ID |
| `LedgerID` | ReadWrite | Integer | Filter to ledger |
| `LedgerTypeID` | ReadWrite | Integer | Filter to ledger type |
| `ShortCode` | ReadWrite | String | Short code search/result |
| `FullKey` | ReadWrite | String | Full key search/result |
| `AccountName` | ReadWrite | String | Account name search/result |
| `AccountUIK` | ReadWrite | Integer | Account unique invariant key |
| `NominalAccountID` | ReadWrite | Integer | Nominal account link |
| `AllowStoppedAccounts` | ReadWrite | Boolean | Include stopped accounts in results |
| `SourceArray(,)` | ReadWrite | Object | Source array for bulk pre-filtering |
| `BranchID` | ReadWrite | Integer | Branch filter |
| `AgentCnt` | ReadWrite | Integer | Agent count filter |
| `AppName` | ReadWrite | String | Application name for context |
| `InsurersAgents` | ReadWrite | Boolean | Only show insurer/agent accounts |
| `ExcludeInsurersAgents` | ReadWrite | Boolean | Exclude insurer/agent accounts |
| `OnlyUpdatableAccounts` | ReadWrite | Boolean | Only editable accounts |
| `NotEditable` | ReadWrite | Integer | Prevent editing from this screen |
| `AccountCompanyId` | ReadWrite | Integer | Company-specific account filter |
| `DisableWildcardSearchOption` | ReadWrite | Boolean | Hide wildcard toggle |
| `EnablePartialWildcardSearchOption` | ReadWrite | Boolean | Enable partial wildcard mode |
| `PMProductFamily` | ReadOnly | Integer | Returns `pmePFOrion` |
| `PMAuthorityLevel` | WriteOnly | Integer | Authority level |

**Business Components Used:** `bACTAccount.Form`

---

### 6.2 FindBank

**File:** `Orion\Components\FindBank\Interface\iACTFindBank\iACTFindBankCls.vb` (28.3 KB)  
**Form:** `iACTFindBank.vb` (70.6 KB)  
**Purpose:** Bank lookup and selection screen. Returns selected bank ID and short code.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `OmitBankID` | WriteOnly | Integer | Bank ID to exclude from results |
| `BankID` | ReadWrite | Integer | Pre-select or returned bank ID |
| `ShortCode` | ReadWrite | String | Short code search/result |
| `AccountName` | ReadOnly | String | Bank name of selected bank |
| `CompanyID` | WriteOnly | Integer | Company filter |
| `PMAuthorityLevel` | WriteOnly | Integer | Authority level |
| `PMProductFamily` | ReadOnly | Integer | Returns `pmePFOrion` |

**Business Components Used:** `bACTBank.Form`

---

### 6.3 FindBudget

**File:** `Orion\Components\FindBudget\Interface\iACTFindBudget\iACTFindBudgetCls.vb` (26.1 KB)  
**Form:** `iACTFindBudget.vb` (80.2 KB)  
**Purpose:** Budget lookup and selection screen.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `Reference` | ReadWrite | String | Budget reference search/result |
| `BudgetId` | ReadWrite | Integer | Pre-select or returned budget ID |
| `Revising` | ReadWrite | Boolean | Filter to revision budgets only |
| `PMAuthorityLevel` | ReadWrite | Integer | Authority level |

---

### 6.4 FindCashList

**File:** `Orion\Components\FindCashList\Interface\iACTFindCashList\iACTFindCashList.vb` (30.9 KB)  
**Form:** `iACTFindCashListFrm.vb` (70.8 KB)  
**Purpose:** Cash list search screen — find an existing cash list by type, reference, or date.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `CashListID` | ReadWrite | Integer | Pre-select or returned cash list ID |
| `CashListTypeID` | ReadWrite | Integer | Filter to receipt or payment lists |
| `SourceArray` | ReadOnly | Object | Result array of matching cash lists |
| `CompanyID` | WriteOnly | Integer | Company context |
| `StepStatus` | ReadOnly | String | Step status |

---

### 6.5 FindCashListItem

**File:** `Orion\Components\FindCashListItem\Interface\iACTFindCashListItem\iACTFindCashListItemCls.vb` (34.3 KB)  
**Form:** `iACTFindCashListItemFrm.vb` (107.3 KB)  
**Purpose:** Cash list item search — find specific payment/receipt items within a batch.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `CashlistTypeID` | ReadWrite | Integer | Filter to receipt or payment items |
| `BatchReference` | ReadWrite | String | Batch reference filter |
| `BatchID` | ReadWrite | Integer | Specific batch to search within |
| `CashListID` | ReadWrite | Integer | Cash list to search within |
| `CashListItemId` | ReadWrite | Integer | Pre-select or returned item ID |
| `SourceArray` | ReadOnly | Object | Result array |
| `StepStatus` | ReadOnly | String | Step status |
| `ClaimMode` | ReadWrite | Boolean | Search within claim payment context |

---

### 6.6 FindDocument

**File:** `Orion\Components\FindDocument\Interface\iACTFindDocument\iACTFindDocument.vb` (26.3 KB)  
**Form:** `iACTFindDocumentFrm.vb` (93.7 KB)  
**Purpose:** Document/journal search screen — find GL documents by reference, type, or date.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `DocumentId` | ReadWrite | Integer | Pre-select or returned document ID |
| `DocumentRef` | ReadWrite | String | Reference search/result |
| `PMAuthorityLevel` | ReadWrite | Integer | Authority level |

---

### 6.7 FindInvoice

**File:** `Orion\Components\FindInvoice\Interface\iACTFindInvoice\iACTFindInvoice.vb` (26.4 KB)  
**Form:** `iACTFindInvoiceFrm.vb` (73.2 KB)  
**Purpose:** Purchase invoice search screen.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `InvoiceType` | ReadWrite | String | Invoice type filter (Purchase, Credit Note) |
| `Reference` | ReadWrite | String | Invoice reference search/result |
| `InvoiceId` | ReadWrite | Integer | Pre-select or returned invoice ID |
| `PMAuthorityLevel` | ReadWrite | Integer | Authority level |

---

### 6.8 FindTransaction

**File:** `Orion\Components\FindTransaction\Interface\iACTFindTransaction\iACTFindTransaction.vb` (50.8 KB)  
**Form:** `iACTFindTransactionFrm.vb` (546.2 KB — largest form in Orion)  
**Purpose:** General ledger transaction/drill-down screen. Supports full GL inquiry with account, period, allocation, and batch filtering. The most feature-rich find screen in Orion.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `DataChanged` | ReadWrite | Boolean | Set to True if user changed data |
| `ActionKey` | ReadWrite | String | Action key returned from screen |
| `Rollup` | ReadWrite | Integer | Rollup level for account hierarchy view |
| `PMAuthorityLevel` | ReadWrite | Integer | Authority level |
| `CompanyID` | WriteOnly | Integer | Company context |
| `TransDetailId` | ReadOnly | Integer | Selected transaction detail ID |
| `AccountID` | ReadWrite | Integer | Filter to account |
| `DocumentRef` | ReadWrite | String | Document reference filter |
| `DocumentId` | ReadWrite | Integer | Specific document filter |
| `InsuranceRef` | ReadWrite | String | Policy/insurance reference filter |
| `DrillLevel` | ReadWrite | Integer | Drill-down level in account tree |
| `AllocationTransType` | ReadWrite | String | Allocation transaction type filter |
| `AllocationID` | ReadWrite | Integer | Specific allocation filter |
| `BatchID` | ReadWrite | Integer | Batch filter |
| `CashListTypeID` | ReadWrite | Integer | Cash list type filter |
| `SourceArray` | ReadWrite | Object | Input/output array |
| `BranchID` | ReadWrite | Integer | Branch filter |
| `OutstandingOnly` | ReadWrite | Boolean | Show only unallocated transactions |
| `CashListId` | ReadWrite | Integer | Cash list context |
| `ExcludeTransDetailID` | ReadWrite | Integer | Exclude specific transaction from results |
| `SelectedSourceId` | ReadWrite | Integer | Source/company of selected item |
| `SelectedCurrencyId` | ReadWrite | Integer | Currency of selected item |
| `InsuredAccountID` | ReadWrite | Integer | Insured party's account ID |
| `InsuredAccountView` | ReadWrite | Boolean | Show insured account view |
| `UserPartyArray` | ReadWrite | Object | User party context array |

---

## 7. Budget & Period Interfaces

### 7.1 Budget

**File:** `Orion\Components\Budget\Interface\iACTBudget\iACTBudgetCls.vb` (26.1 KB)  
**Form:** `iACTBudget.vb` (67.3 KB)  
**Purpose:** Budget maintenance — creates and maintains accounting period budgets, revisions, and copies.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `BudgetID` | ReadWrite | Integer | Budget primary key |
| `PeriodID` | ReadWrite | Integer | Accounting period |
| `BudgetRef` | ReadWrite | String | Budget reference string |
| `Description` | ReadWrite | String | Budget description |
| `PeriodYearName` | ReadWrite | String | Period year label |
| `RevisesBudgetID` | ReadWrite | Integer | Budget this is a revision of |
| `BasedOnBudgetID` | ReadWrite | Integer | Budget this was copied from |
| `BudgetStatusID` | ReadWrite | Integer | Current status (draft, approved, etc.) |
| `StepStatus` | ReadOnly | String | Roadmap step status |

**Business Components Used:** `bACTBudget`

---

### 7.2 BudgetDetail

**File:** `Orion\Components\BudgetDetail\Interface\iACTBudgetDetail\iACTBudgetDetailCls.vb` (24.4 KB)  
**Form:** `iACTBudgetDetail.vb` (98.4 KB)  
**Purpose:** Budget detail entries — maintains the period-by-account breakdown of budget figures.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `BudgetID` | ReadWrite | Integer | Parent budget |
| `RevisesBudgetID` | ReadWrite | Integer | Budget being revised |
| `BasedOnBudgetID` | ReadWrite | Integer | Budget copied from |
| `PeriodYearName` | ReadWrite | String | Period year label |
| `PMAuthorityLevel` | ReadWrite | Integer | Authority level |
| `StepStatus` | ReadOnly | String | Step status |

**Business Components Used:** `bACTBudgetDetail`

---

### 7.3 Period

**File:** `Orion\Components\Period\Interface\iACTPeriod\iACTPeriodCls.vb` (18.7 KB)  
**Forms:** `iACTPeriodAdd.vb` (62.5 KB), `iACTPeriodDetails.vb` (75.1 KB), `iACTPeriodList.vb` (82.2 KB)  
**Purpose:** Accounting period administration — add new periods, view and edit period dates, and list all periods.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `PMAuthorityLevel` | ReadWrite | Integer | Authority level |
| `Task` | ReadOnly | Integer | Task |
| `Navigate` | ReadOnly | Integer | Navigate exit |
| `ProcessMode` | ReadOnly | Integer | Process mode |
| `TransactionType` | ReadOnly | String | Transaction type |
| `EffectiveDate` | ReadOnly | Date | Effective date |

**Business Components Used:** `bACTPeriod.Form`

---

### 7.4 PeriodEnd

**File:** `Orion\Components\PeriodEnd\Interface\iACTPeriodEnd\iACTPeriodEnd.vb` (23.5 KB)  
**Form:** `iACTPeriodEndFrm.vb` (125.3 KB)  
**Purpose:** Period end closing process — runs the period-close routine, validates no open transactions exist, and locks the period. Supports background/scheduled execution.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `PMAuthorityLevel` | ReadWrite | Integer | Authority level |
| `Task` | ReadWrite | Integer | Task |
| `PMProductFamily` | ReadOnly | Integer | Returns `pmePFOrion` |
| `AttachToScheduler` | ReadWrite | Boolean | Run via scheduler |
| `BatchProcessId` | ReadWrite | Integer | Scheduler batch process ID |
| `BatchProcessName` | ReadWrite | String | Scheduler batch process name |
| `BatchSchedulerId` | ReadWrite | Integer | Scheduler ID |
| `BatchParameters` | ReadWrite | Object(,) | Parameters array for scheduler |

**Business Components Used:** `bACTPeriodEnd`, `bACTAuditSet.Form`

---

### 7.5 Ledger

**File:** `Orion\Components\Ledger\Interface\iACTLedger\iACTLedgerCls.vb` (18.6 KB)  
**Forms:** `iACTLedgerDetails.vb` (62 KB), `iACTLedgerList.vb` (71.2 KB)  
**Purpose:** Ledger maintenance — maintain ledger definitions that group accounts.  
No domain-specific input/output properties; navigation controlled via standard `Task`/`Status` pattern. Holds form reference `frmList`.

**Business Components Used:** `bACTLedger`

---

## 8. Invoice & Import/Export Interfaces

### 8.1 PurchaseInvoice

**File:** `Orion\Components\PurchaseInvoice\Interface\iACTInvoice\iACTInvoiceInterface.vb` (24.6 KB)  
**Form:** `iACTInvoiceFrm.vb` (119.3 KB)  
**Purpose:** Purchase invoice entry — records creditor invoices for payment. Creates the invoice document in the GL.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `InvoiceID` | WriteOnly | Integer | Invoice to open (pass IN for edit) |
| `Task` | ReadWrite | Integer | Task |
| `Navigate` | ReadOnly | Integer | Navigate exit |
| `ProcessMode` | ReadOnly | Integer | Process mode |
| `TransactionType` | ReadOnly | String | Transaction type |
| `EffectiveDate` | ReadOnly | Date | Effective date |
| `StepStatus` | ReadOnly | String | Step status |
| `PMProductFamily` | ReadOnly | Integer | Returns `pmePFOrion` |
| `PMAuthorityLevel` | WriteOnly | Integer | Authority level |

**Business Components Used:** `bACTInvoice.Form`, `bACTDocument.Form`, `bACTDocumentPost.Form`

---

### 8.2 PurchaseInvoiceCreditNote

**File:** `Orion\Components\PurchaseInvoiceCreditNote\Interface\iACTPurchaseCredit\Interface.vb` (10.1 KB)  
**Purpose:** Credit note against a purchase invoice. Minimal interface — delegates to invoice business logic.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `CallingAppName` | ReadWrite | String | Calling application name |
| `PMAuthorityLevel` | ReadWrite | Integer | Authority level |
| `Status` | ReadWrite | Integer | Exit status |

**Business Components Used:** `bACTInvoice.Form`

---

### 8.3 ImportExport

**File:** `Orion\Components\ImportExport\Interface\iACTImportExport\iInterface.vb` (18.6 KB)  
**Form:** `iACTInterface.vb` (105.3 KB), `frmReceiptImport.vb` (31.2 KB), `frmViewGL.vb` (9 KB)  
**Purpose:** Bulk data import and export. Supports export of GL, policy, claim, commission, MID, instalment, and payment data. Imports bank reconciliation, cash allocation, agent reconciliation, policy BDX, claim BDX, and premium BDX files.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `PMProductFamily` | ReadOnly | Integer | Returns `pmePFSiriusSolutions` |
| `PMAuthorityLevel` | WriteOnly | Integer | Authority level |
| `CallingAppName` | WriteOnly | String | Calling application |
| `Status` | ReadOnly | Integer | Exit status |
| `SpecialParty` | WriteOnly | String | Special party context |
| `AttachToScheduler` | ReadWrite | Boolean | Execute via scheduler |
| `BatchProcessId` | ReadWrite | Integer | Scheduler batch process ID |
| `BatchProcessName` | ReadWrite | String | Scheduler process name |
| `BatchParameters` | ReadWrite | Object(,) | Scheduler parameters |
| `BatchSchedulerId` | ReadWrite | Integer | Scheduler ID |
| `Task` | ReadWrite | Integer | Task mode |

#### Export Classes

| Class | Description |
|---|---|
| `GL_Export.vb` | General ledger transaction export |
| `Policy_Export.vb` / `Policy_Batch_Export.vb` | Policy data export |
| `Claims_Export.vb` | Claims data export |
| `Commission_Export.vb` | Commission transaction export |
| `MID_Export.vb` / `MID2_Export.vb` | Motor Insurance Database export |
| `Payment_Export.vb` | Payment data export |
| `Receipt_Export.vb` | Receipt data export |
| `Instalment_Export.vb` / `Instalment_Plan_Export.vb` | Instalment plan export |
| `Document_Export.vb` | Document/attachment export |

#### Import Classes

| Class | Description |
|---|---|
| `Bank_Reconciliation_Import.vb` | Bank statement import for reconciliation |
| `Cash_Allocation_Import.vb` | Cash allocation import |
| `Agent_Reconciliation_Import.vb` | Agent reconciliation import |
| `Policy_BDX_Import.vb` | Policy bordereau import |
| `Claim_BDX_Import.vb` | Claims bordereau import |
| `Premium_BDX_Import.vb` | Premium bordereau import |
| `Payment_Import.vb` | Payment import |
| `Receipt_Import.vb` | Receipt import |
| `Instalment_Import.vb` | Instalment plan import |
| `Cover_Note_Import.vb` | Cover note import |
| `Exchanges_Rates_Import.vb` | Exchange rate import |
| `Reference_Import.vb` | Reference data import |

---

## 9. Agent & Commission Interfaces

### 9.1 AgentSummary (CommissionPayments)

**File:** `Orion\Components\Agent Summary\Interface\iACTCommissionPayments\Interface.vb` (23.5 KB)  
**Form:** `frmInterface.vb` (131.6 KB)  
**Purpose:** Agent commission payment summary — displays commission position by agent with filtering by date range, currency, product, and branch.

**PMProductFamily:** `pmePFSiriusSolutions`

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `PMProductFamily` | ReadOnly | Integer | Returns `pmePFSiriusSolutions` |
| `PMAuthorityLevel` | WriteOnly | Integer | Authority level |
| `CallingAppName` | WriteOnly | String | Calling app |
| `Status` | ReadOnly | Integer | Exit status |

#### Interface Methods

| Method | Description |
|---|---|
| `Initialise()` | Gets `bACTCommissionPayments.Business` instance |
| `Start()` | Launches the commission summary form |
| `GetKeys(vKeyArray)` | Returns `risk_id` and `payment_terms` as key array to navigator |
| `SetKeys(vKeyArray)` | Accepts keys including: `AgentSelect` (agent array), `SearchResults`, `AutoSearch`, `StatementDate`, `TransDateFrom`, `TransDateTo`, `CurrencyItemID`, `ProductItemID`, `BranchItemID`, `TransAuthLimit` |

**Business Components Used:** `bACTCommissionPayments.Business`

---

### 9.2 AgentSelect

**File:** `Orion\Components\Agent Summary\Interface\iACTAgentSelect\iACTAgentSelectCls.vb` (54.1 KB)  
**Form:** `iACTAgentSelect.vb` (109.6 KB)  
**Purpose:** Agent/party selection screen shared across Sirius — finds parties by name, code, or agent type with filtering options.

**PMProductFamily:** `pmePFSiriusSolutions`

#### Key Interface Properties (selected)

| Property | Access | Type | Description |
|---|---|---|---|
| `PartyCnt` | ReadWrite | Integer | Number of selected parties |
| `ShortName` | ReadWrite | String | Short name search filter |
| `LongName` | ReadWrite | String | Long name search string |
| `AgentOnly` | ReadWrite | Integer | Restrict to agents only |
| `IntroducerOnly` | ReadWrite | Boolean | Restrict to introducers |
| `SpecialParty` | ReadWrite | String | Special party type code |
| `PartyUIK` | ReadWrite | Integer | Universal invariant key |
| `InsuranceRef` | ReadWrite | String | Insurance ref context |
| `SelectedPartyType` | ReadWrite | String | Returned party type |
| `IncludeClosedBranches` | ReadWrite | Boolean | Include closed branches in results |
| `SuppressCancelledAgents` | WriteOnly | Boolean | Hide cancelled agents |
| `IsComplaint` | WriteOnly | Integer | Complaint search mode |
| `ValidPartyTypesArray` | WriteOnly | Object() | Restrict to specific party types |
| `SuppressSubAgents` | WriteOnly | Boolean | Hide sub-agents |
| `AllowAddressSelection` | WriteOnly | Boolean | Enable address drill-down |
| `AllowAgentSearch` | WriteOnly | Boolean | Enable agent-specific search |
| `DisableWildcardSearchOption` | ReadWrite | Boolean | Disable wildcard toggle |
| `EnablePartialWildcardSearchOption` | ReadWrite | Boolean | Enable partial wildcard |
| `CommissionLevel` | WriteOnly | Integer | Filter by commission level |

---

### 9.3 CommissionMovement

**File:** `Orion\Components\CommissionMovement\Interface\iACTCommissionMovement\iACTCommissionMovementCls.vb` (22.8 KB)  
**Form:** `iACTCommissionMovement.vb` (26.2 KB)  
**Related:** `iACTAutoCommissionMovement.vb` (24.3 KB) — auto-commission movement form  
**Purpose:** Commission movement posting — creates journal documents to record commission earned/due movements.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `DocumentId` | ReadWrite | Integer | Document primary key |
| `DocumentTypeID` | WriteOnly | Integer | Document type |
| `DocumentDate` | ReadWrite | Date | Document date |
| `DocumentRef` | ReadWrite | String | Document reference |
| `CompanyID` | ReadWrite | Integer | Company context |

**Business Components Used:** `bACTCommissionPost.Business`

---

## 10. Configuration & Reference Interfaces

### 10.1 CurrencyRate

**File:** `Orion\Components\CurrencyRate\Interface\iACTCurrencyRate\iACTCurrencyRateCls.vb` (21.8 KB)  
**Form:** `iACTCurrencyRateFrm.vb` (72.3 KB)  
**Purpose:** Currency exchange rate maintenance — add and update exchange rates between currency pairs for a company.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `CurrencyId` | ReadWrite | Integer | Currency being rated |
| `CompanyId` | ReadWrite | Integer | Company context |
| `PMAuthorityLevel` | WriteOnly | Integer | Authority level |

**Business Components Used:** `bACTCompany.Form`, `bACTCompanyCurrency.Form`, `bACTCurrency.Form`, `bACTCurrencyRate.Form`

---

### 10.2 Company

**File:** `Orion\Components\Company\Interface\iACTCompany\iACTCompany.vb` (19 KB)  
**Forms:** `iACTCompanyDetails.vb` (84.3 KB), `iACTCompanyList.vb` (76 KB)  
**Purpose:** Company maintenance and selection — view company list, edit company details, and return selected company ID to the caller.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `SelectedCompanyID` | ReadOnly | Integer | Company ID selected by the user |
| `PMAuthorityLevel` | ReadWrite | Integer | Authority level |

**Business Components Used:** `bACTCompany.Form`

---

### 10.3 MaintainMediaTypeStatus

**File:** `Orion\Components\MaintainMediaTypeStatus\Interface\iACTMaintainMediaTypeStatus\iACTMaintainMediaTypeStatus.vb` (20.7 KB)  
**Form:** `iACTMaintainMediaTypeStatusFrm.vb` (84.4 KB), `iACTUpdateMediaTypeStatus.vb` (8.3 KB)  
**Purpose:** Maintain media type statuses (BACS, cheque, direct debit statuses) — update the status of payment media in bulk or individually.

Standard properties only (`Status`, `Task`, `Navigate`, `ProcessMode`, `TransactionType`, `EffectiveDate`, `CallingAppName`).

**Business Components Used:** `bACTMediaType`

---

### 10.4 CreditControl

**File:** `Orion\Components\CreditControl\Interface\iACTCreditControlMaint\iACTCreditControlMaintCls.vb` (23.5 KB)  
**Forms:** `frmDetails.vb` (118.6 KB), `frmInterface.vb` (73.2 KB), `frmStep.vb` (114.3 KB)  
**Purpose:** Credit control maintenance — manages credit control configuration, thresholds, and rule sets per policy or account.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `CashListDrawerID` | Public | Integer | Cash list drawer context |
| `StartMode` | Public | Integer | Initial screen mode |
| `PMAuthorityLevel` | ReadWrite | Integer | Authority level |
| `StartupMode` | ReadWrite | Integer | Startup mode (add/edit/view) |

**Business Components Used:** `bACTCreditControl`

---

### 10.5 CreditControlProcessing

**File:** `Orion\Components\CreditControl\Interface\iACTCreditControlProcessing\iACTCreditControlProcessing.vb` (23.4 KB)  
**Form:** `frmInterface.vb` (44.9 KB)  
**Purpose:** Runs the credit control processing step — applies credit control rules and generates notifications/actions for overdue accounts.

Standard properties only. **Business Components Used:** `bACTCreditControl`

---

### 10.6 UserAuthorities

**File:** `Orion\Components\UserAuthorities\Interface\iACTUserAuthorities\iACTUserAuthoritiesInterface.vb` (18.9 KB)  
**Forms:** `iACTUserAuthoritiesFrm.vb` (62.2 KB), `frmAuthorities.vb` (36.6 KB)  
**Purpose:** View and maintain Orion-specific user authority settings — which users have access to which accounting functions.

**PMProductFamily:** `pmePFOrion`

**Business Components Used:** `bACTUserAuthorities.Business`

---

## 11. Workflow Decision Interfaces

### 11.1 AllocationDecision

**File:** `Orion\Components\AllocationDecision\Interface\iACTAllocationDecision\iACTAllocationDecisionCls.vb` (16.2 KB)  
**Purpose:** Roadmap decision node — presents a decision dialog about allocation options (allocate now, allocate later, write off). Returns `StepStatus` to indicate the chosen path.

**Properties:** Standard navigator properties only (`Status`, `Task`, `Navigate`, `ProcessMode`, `TransactionType`, `EffectiveDate`, `StepStatus`).

---

### 11.2 CLIRepeatDecision

**File:** `Orion\Components\CLIRepeatDecision\Interface\iACTCLIRepeatDecision\iACTCLIRepeatDecisionCls.vb` (16.3 KB)  
**Purpose:** Roadmap decision node — asks whether to repeat a cash list item entry (add another item to the same batch). Returns `StepStatus`.

**Properties:** Standard navigator properties only (`Status`, `Task`, `Navigate`, `ProcessMode`, `TransactionType`, `EffectiveDate`, `StepStatus`).

---

### 11.3 CashReceipt

**File:** `Orion\Components\CashReceipt\Interface\iACTCashReceipt\Interface.vb` (23.9 KB)  
**Purpose:** Cash receipt coordination — coordinates the receipt entry workflow between CashList, CashListItem, and Allocation components.

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `AccountID` | ReadWrite | Integer | Posting account |
| `CashListTypeID` | ReadWrite | Integer | Cash list type |
| `CashListID` | ReadWrite | Integer | Cash list context |
| `CashListItemID` | ReadWrite | Integer | Cash list item context |
| `BatchID` | ReadWrite | Integer | Batch context |
| `AllocationID` | ReadWrite | Integer | Allocation context |

**Business Components Used:** `bACTAccount.Form`

---

### 11.4 ClaimPaymentProcessing

**File:** `Orion\Components\ClaimPaymentProcessing\Interface\iACTClaimPaymentProcessing\Interface.vb` (18.8 KB)  
**Forms:** `frmInterface.vb` (138.9 KB), `frmPaymentProcessed.vb` (1.9 KB)  
**Purpose:** Claims payment processing workflow — handles the payment processing steps for claim payments (authorisation, posting, media file generation).

#### Interface Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `CallingAppName` | WriteOnly | String | Calling application |
| `PMAuthorityLevel` | WriteOnly | Integer | Authority level |
| `Status` | ReadOnly | Integer | Exit status |

**Business Components Used:** `bACTCashListPost.Automated`

---

### 11.5 ExportCashListItems

**File:** `Orion\Components\ExportCashListItems\Interface\iACTExportCashListItems\Interface.vb` (14.8 KB)  
**Form:** `frmExportCashBook.vb` (24.9 KB)  
**Purpose:** Export cash list items to the SBO (back-office) database or external file format.

**Properties:** `Status`, `CallingAppName`, `PMAuthorityLevel` only.

**Business Components Used:** `bACTExportCashListItems.Business`

---

## 12. User Controls

All user controls reside in `Orion\Components\User Controls\`.

---

### 12.1 iACTAccountLookup

**File:** `iACTUserControls\iACTAccountLookup.vb` (50.4 KB)  
**Purpose:** Reusable account lookup/picker control — text box with lookup button that invokes `FindAccount`. Returns the selected account ID plus metadata.

#### Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `AccountId` | ReadWrite | Integer | Selected account ID |
| `AccountType(vAccountId?)` | ReadOnly | Integer | Account type of given/selected account |
| `AccountName(vAccountId?)` | ReadOnly | String | Account name |
| `AccountCode(vAccountId?)` | ReadOnly | String | Account code |
| `AccountShortCode(vAccountId?)` | ReadOnly | String | Account short code |
| `AccountCompanyId(vAccountId?)` | ReadOnly | Integer | Company ID of account |
| `NominalAccountID` | ReadOnly | Integer | Nominal account link |
| `CompanyId` | ReadWrite | Integer | Company filter |
| `AllowStoppedAccounts` | ReadWrite | Boolean | Include stopped accounts |
| `OnlyUpdatableAccounts` | ReadWrite | Boolean | Only editable accounts |
| `Default_Renamed` | ReadWrite | Boolean | Use as default |
| `ShowEditOnFindAccount` | ReadWrite | Boolean | Show edit button on find screen |
| `LookupCaption` | ReadWrite | String | Button/label caption |
| `LookupLeft`, `LookupHeight`, `LookupWidth` | ReadWrite | Integer | Layout dimensions |
| `LookupTextLeft`, `LookupTextWidth` | ReadWrite | Integer | Text box layout |
| `BackStyle`, `SelLength`, `SelStart`, `SelText`, `ToolTipText` | ReadWrite | Various | Appearance/selection |

#### Methods

| Method | Description |
|---|---|
| `CheckAccountActive(v_lAccountId)` | Validates whether the given account is active/not stopped |
| `GetAccountBusiness()` | Initialises the `bACTAccount.Form` business object |
| `GetAccountFromShort(v_sShortCode, v_bOnlyUpdatable, r_lAccountId, [v_vCompanyId])` | Resolves an account ID from its short code |
| `GetAccountFromFull(v_sFullCode, r_lAccountId)` | Resolves an account ID from its full key |
| `GetAccount(v_lAccountId, r_sAccountName, r_sAccountShortCode, r_lAccountType, r_sAccountCode, r_lNominalAccountID, r_iCompanyId)` | Retrieves all account attributes for the given account ID |

---

### 12.2 iACTBankAccount

**File:** `iACTUserControls\iACTBankAccount.vb` (27 KB)  
**Companion DTO:** `iACTBankAccountDetail.vb` (2.3 KB) — simple data class returned by `GetBanks()`. Properties: `Id`, `Description`, `Code`, `AccountId`, `BankAccountName`, `BankAccountNo`, `CurrencyId`, `IsCashReceiveInThisCurrencyOnly`.  
**Purpose:** Bank account picker control — combo-box style control for selecting a party's bank account. Populates from `bACTBankAccount` data.

#### Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `Id` | ReadWrite | Integer | Selected bank account item ID |
| `Description(v_vItemId?)` | ReadOnly | String | Account description |
| `Code(v_vItemId?)` | ReadOnly | String | Account code |
| `AccountId(v_vItemId?)` | ReadOnly | String | GL account ID |
| `BankAccountName(v_vItemId?)` | ReadOnly | String | Bank account name |
| `BankAccountNo(v_vItemId?)` | ReadOnly | String | Bank account number |
| `CurrencyId(v_vItemId?)` | ReadOnly | String | Currency of the account |
| `IsCashReceiveInThisCurrencyOnly(v_vItemId?)` | ReadOnly | String | Currency restriction flag |
| `DefaultId` | ReadWrite | String | Default selection ID |
| `FirstItem` | ReadWrite | String | First item text (blank/all option) |
| `CompanyId` | WriteOnly | Integer | Company filter |
| `List(Index)` | ReadWrite | String | Item text at index |
| `ItemData(Index)` | ReadWrite | Integer | Item data at index |
| `ListCount` | ReadOnly | Integer | Number of items |
| `ListIndex` | ReadWrite | Integer | Current selected index |
| `NewIndex` | ReadOnly | Integer | Index of most-recently added item |
| `ToolTipText`, `WhatsThisHelpID` | ReadWrite | Various | Appearance |

#### Methods

| Method | Description |
|---|---|
| `AddItem(Item, [vItemId])` | Adds an item to the combo list |
| `RemoveItem(Index)` | Removes item at the given index |
| `RefreshList()` | Reloads the bank account list from database |
| `GetBanks(colBankAccountDetails)` | Populates the control from a collection of bank account details |

---

### 12.3 iACTCurrencyLookup

**File:** `iACTUserControls\iACTCurrencyLookup.vb` (28.2 KB)  
**Purpose:** Currency picker control — combo-box for selecting a currency, with restriction filters.

#### Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `CurrencyId` | ReadWrite | Integer | Selected currency ID |
| `CurrencyCode(v_vCurrencyId?)` | ReadOnly | String | ISO currency code |
| `CurrencyName(v_vCurrencyId?)` | ReadOnly | String | Currency name |
| `RestrictTo` | ReadWrite | RestrictToCurrency | Enum filter (AllCurrencies, CompanyCurrencies, etc.) |
| `CompanyId` | ReadWrite | Integer | Company context for filtering |
| `DefaultCurrencyId` | ReadWrite | Integer | Pre-selected default |
| `FirstItem` | ReadWrite | String | Optional blank/all entry |
| `List(Index)` | ReadWrite | String | Item text |
| `ItemData(Index)` | ReadWrite | Integer | Item data |
| `ListCount` | ReadOnly | Integer | Number of currencies listed |
| `ListIndex` | ReadWrite | Integer | Selected index |
| `Sorted` | ReadOnly | Boolean | Whether list is sorted |
| `ToolTipText`, `WhatsThisHelpID` | ReadWrite | Various | Appearance |

#### Methods

| Method | Description |
|---|---|
| `AddItem(Item, [Index])` | Manually adds a currency item |
| `RemoveItem(Index)` | Removes a currency item |
| `RefreshList()` | Reloads currencies from database based on `RestrictTo` and `CompanyId` |

---

### 12.4 iACTTypeTable

**File:** `iACTUserControls\iACTTypeTable.vb` (34.9 KB)  
**Purpose:** Generic type/lookup table combo picker. Used to display items from any configured lookup table (payment types, account types, document types, etc.).

#### Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `Table` | ReadWrite | actTable | Selects which lookup table to load |
| `TableName` | ReadWrite | String | Alternative table selection by name |
| `ItemId` | ReadWrite | Integer | Selected item ID |
| `ItemCode` | ReadWrite | String | Selected item code |
| `ItemDescription(v_vItemId?)` | ReadOnly | String | Description of given/selected item |
| `DefaultItemId` | ReadWrite | Integer | Pre-selected default item |
| `FirstItem` | ReadWrite | String | Optional blank/all entry |
| `Sorted` | ReadWrite | Boolean | Auto-sort list |
| `List(Index)` | ReadWrite | String | Item text |
| `ItemData(Index)` | ReadWrite | Integer | Item data |
| `ListCount` | ReadOnly | Integer | Number of items |
| `ListIndex` | ReadWrite | Integer | Selected index |
| `NewIndex` | ReadOnly | Integer | Index of last added item |
| `BackStyle`, `ToolTipText`, `WhatsThisHelpID` | ReadWrite | Various | Appearance |

#### Methods

| Method | Description |
|---|---|
| `AddItem(Item, [Index])` | Manually adds an item |
| `RemoveItem(Index)` | Removes an item |
| `RefreshList()` | Reloads items from the configured lookup table |

---

### 12.5 uctACTCreditCard

**File:** `uctACTCreditCard\uctACTCreditCard.vb` (97.4 KB)  
**Purpose:** Credit card processing control — full credit card data entry with secure handling (encrypt flag), card type management, authorisation code capture, and integration with external card processing systems.

#### Properties

| Property | Access | Type | Description |
|---|---|---|---|
| `DefaultBankPaymentType` | ReadWrite | String | Default payment type code |
| `DefaultAccountType` | ReadWrite | String | Default account type |
| `IsExternalCreditCardProcessing` | ReadWrite | Boolean | Whether external card processor is in use |
| `ViewOnlyMode` | ReadWrite | Boolean | Display only, no editing |
| `IsPayment` | WriteOnly | Boolean | Payment (vs receipt) mode |
| `IsClaimPaymentType` | WriteOnly | Boolean | Claim payment context |
| `Encrypt` | WriteOnly | Boolean | Enable card number encryption |
| `ControlInitialisedFlag` | WriteOnly | Boolean | Signal control is ready |
| `MediaTypeIssuerID` | WriteOnly | Integer | Card issuer media type ID |
| `MediaTypeID` | WriteOnly | Integer | Media type ID |
| `AccountID` | WriteOnly | Integer | GL account ID |
| `InsuranceFileCnt` | WriteOnly | Integer | Insurance file count for context |
| `CCAmount` | WriteOnly | Decimal | Transaction amount |
| `CCCurrencyID` | WriteOnly | Integer | Transaction currency |
| `CCNumber` | ReadWrite | String | Card number (masked/encrypted) |
| `CCNumber1` | WriteOnly | String | Card number part 1 |
| `CCName` | ReadWrite | String | Cardholder name |
| `CCExpiry` | ReadWrite | String | Expiry date |
| `CCStart` | ReadWrite | String | Start date |
| `CCIssue` | ReadWrite | String | Issue number |
| `CCPIN` | ReadWrite | String | PIN/CV2 number |
| `CCAddress1` | WriteOnly | String | Cardholder address line 1 |
| `CCPostcode` | WriteOnly | String | Postcode for AVS check |
| `CCAutoAuthCode` | ReadWrite | String | Automated authorisation code |
| `CCManualAuthCode` | ReadWrite | String | Manual authorisation code |
| `CCCustomerFlag` | ReadWrite | String | Customer present flag |
| `CCStatusText` | ReadOnly | String | Processing status description |
| `CCReturnStatus` | ReadOnly | String | Processing return code |
| `CCTransactionCode` | ReadWrite | String | Transaction code |
| `CCIsDefault` | ReadWrite | Integer | Whether this is the default card |
| `IsAdditionalDetailOption` | ReadWrite | Boolean | Show additional options |
| `CaptionNameOnCardAdditionalOption` | WriteOnly | Boolean | Show name-on-card field |
| `CaptionExpiryDateAdditionalOption` | WriteOnly | Boolean | Show expiry date field |
| `CaptionCVSPINAdditionalOption` | WriteOnly | Boolean | Show CVS/PIN field |
| `CCBankId` | ReadWrite | Integer | Bank ID |
| `CardTypeId` | ReadWrite | Integer | Card type (Visa, MC, Amex, etc.) |
| `CardTransSlipNo` | ReadWrite | String | Transaction slip number |
| `IsDefault` | ReadWrite | Boolean | Is the default card |
| `ResetPreviousOne` | ReadWrite | Boolean | Clear previous card on initialise |

#### Methods

| Method | Description |
|---|---|
| `Initialise()` | Sets up the control — initialises business objects, loads card types, applies encrypt flag |
| `ClearControls()` | Clears all card data fields |
| `ShowPartyCreditCardScreen()` | Opens the party credit card management screen |
| `AdditionalDetailOptions()` | Shows/hides additional input fields |
| `SetSplitReceiptDefaults()` | Sets control defaults for split-receipt card entry |
| `Dispose()` | Releases all business objects |

---

## 13. Complete File Inventory

| Component Folder | Interface .vb | Size (KB) | Form .vb | Size (KB) |
|---|---|---|---|---|
| Account | `iACTAccount.vb` | 21.8 | `iACTAccountFrm.vb` | 167.3 |
| AccountExplorer | `iACTExplorer.vb` | 28.6 | `iACTExplorerFrm.vb` | 124.1 |
| Agent Summary / AgentSelect | `iACTAgentSelectCls.vb` | 54.1 | `iACTAgentSelect.vb` | 109.6 |
| Agent Summary / CommissionPayments | `Interface.vb` | 23.5 | `frmInterface.vb` | 131.6 |
| Allocate | `iACTAllocateCls.vb` | 32.6 | `iACTAllocateFrm.vb` | 141.7 |
| Allocation | `iACTAllocationCls.vb` | 20.1 | `frmInterface.vb` | — |
| AllocationDecision | `iACTAllocationDecisionCls.vb` | 16.2 | (none) | — |
| Bank | `iACTBankCls.vb` | 22.7 | `iACTBankFrm.vb` | 97.2 |
| BankAccount | `iACTBankAccountInterface.vb` | 22.6 | `iACTBankAccountFrm.vb` | 117.7 |
| BankReconciliation | `iACTBankReconciliation.vb` | 27.8 | `iACTBankReconciliationFrm.vb` | 138.0 |
| Budget | `iACTBudgetCls.vb` | 26.1 | `iACTBudget.vb` | 67.3 |
| BudgetDetail | `iACTBudgetDetailCls.vb` | 24.4 | `iACTBudgetDetail.vb` | 98.4 |
| CashList | `iACTCashListCls.vb` | 54.0 | `iACTCashListFrm.vb` | 85.2 |
| CashList | — | — | `iACTBanking.vb` | 140.4 |
| CashList | — | — | `iACTAdjustment.vb` | 40.8 |
| CashListItem | `iACTCashListItemCls.vb` | 48.6 | `iACTCashListItemDetails.vb` | 546.2 |
| CashListItem | — | — | `iACTCashListItemList.vb` | 446.6 |
| CashReceipt | `Interface.vb` | 23.9 | (none) | — |
| ChequeProduction | `iACTChequeProduction.vb` | 33.6 | `iACTChequeProductionFrm.vb` | 113.9 |
| ClaimPaymentProcessing | `Interface.vb` | 18.8 | `frmInterface.vb` | 138.9 |
| CLIRepeatDecision | `iACTCLIRepeatDecisionCls.vb` | 16.3 | (none) | — |
| CommissionMovement | `iACTCommissionMovementCls.vb` | 22.8 | `iACTCommissionMovement.vb` | 26.2 |
| CommissionMovement (Auto) | `iACTAutoCommissionMovement.vb` | 24.3 | `iACTAutoCommissionMovementFrm.vb` | 20.0 |
| Company | `iACTCompany.vb` | 19.0 | `iACTCompanyDetails.vb` | 84.3 |
| Company | — | — | `iACTCompanyList.vb` | 76.0 |
| CreditControl (Maint) | `iACTCreditControlMaintCls.vb` | 23.5 | `frmDetails.vb` | 118.6 |
| CreditControl (Processing) | `iACTCreditControlProcessing.vb` | 23.4 | `frmInterface.vb` | 44.9 |
| CurrencyRate | `iACTCurrencyRateCls.vb` | 21.8 | `iACTCurrencyRateFrm.vb` | 72.3 |
| Document | `iACTDocument.vb` | 32.2 | `iACTDocumentFrm.vb` | 111.6 |
| ExportCashListItems | `Interface.vb` | 14.8 | `frmExportCashBook.vb` | 24.9 |
| FindAccount | `iACTFindAccountCls.vb` | 45.8 | `iACTFindAccount.vb` | 109.3 |
| FindBank | `iACTFindBankCls.vb` | 28.3 | `iACTFindBank.vb` | 70.6 |
| FindBudget | `iACTFindBudgetCls.vb` | 26.1 | `iACTFindBudget.vb` | 80.2 |
| FindCashList | `iACTFindCashList.vb` | 30.9 | `iACTFindCashListFrm.vb` | 70.8 |
| FindCashListItem | `iACTFindCashListItemCls.vb` | 34.3 | `iACTFindCashListItemFrm.vb` | 107.3 |
| FindDocument | `iACTFindDocument.vb` | 26.3 | `iACTFindDocumentFrm.vb` | 93.7 |
| FindInvoice | `iACTFindInvoice.vb` | 26.4 | `iACTFindInvoiceFrm.vb` | 73.2 |
| FindTransaction | `iACTFindTransaction.vb` | 50.8 | `iACTFindTransactionFrm.vb` | 546.2 |
| ImportExport | `iInterface.vb` | 18.6 | `iACTInterface.vb` | 105.3 |
| InsurerPayment | `iACTInsurerPayment.vb` | 31.5 | `iACTInsurerPaymentFrm.vb` | 418.1 |
| InsurerPaymentGroup | `iACTInsurerPaymentGroupCls.vb` | 20.7 | `iACTInsurerPaymentGroup.vb` | 45.9 |
| InsurerPaymentGroups | `iACTInsurerPaymentGroupsInterface.vb` | 19.7 | `iACTInsurerPaymentGroups.vb` | 51.1 |
| Ledger | `iACTLedgerCls.vb` | 18.6 | `iACTLedgerDetails.vb` | 62.0 |
| Ledger | — | — | `iACTLedgerList.vb` | 71.2 |
| MaintainMediaTypeStatus | `iACTMaintainMediaTypeStatus.vb` | 20.7 | `iACTMaintainMediaTypeStatusFrm.vb` | 84.4 |
| Payment Maintenance | `iACTPaymentMaintenance.vb` | 33.1 | `iACTPaymentMaintenanceFrm.vb` | 107.5 |
| Period | `iACTPeriodCls.vb` | 18.7 | `iACTPeriodAdd.vb` | 62.5 |
| Period | — | — | `iACTPeriodDetails.vb` | 75.1 |
| Period | — | — | `iACTPeriodList.vb` | 82.2 |
| PeriodEnd | `iACTPeriodEnd.vb` | 23.5 | `iACTPeriodEndFrm.vb` | 125.3 |
| PurchaseInvoice | `iACTInvoiceInterface.vb` | 24.6 | `iACTInvoiceFrm.vb` | 119.3 |
| PurchaseInvoiceCreditNote | `Interface.vb` | 10.1 | (none) | — |
| Transaction | `iACTTransaction.vb` | 27.5 | `iACTTransactionFrm.vb` | 143.5 |
| UserAuthorities | `iACTUserAuthoritiesInterface.vb` | 18.9 | `iACTUserAuthoritiesFrm.vb` | 62.2 |

### User Controls

| File | Size (KB) | Location |
|---|---|---|
| `iACTAccountLookup.vb` | 50.4 | `User Controls\iACTUserControls\` |
| `iACTBankAccount.vb` | 27.0 | `User Controls\iACTUserControls\` |
| `iACTBankAccountDetail.vb` | 2.3 | `User Controls\iACTUserControls\` |
| `iACTCurrencyLookup.vb` | 28.2 | `User Controls\iACTUserControls\` |
| `iACTTypeTable.vb` | 34.9 | `User Controls\iACTUserControls\` |
| `iACTUserControls.vb` | 3.0 | `User Controls\iACTUserControls\` |
| `uctACTCreditCard.vb` | 97.4 | `User Controls\uctACTCreditCard\` |

> **Note — ManageDebtors:** A `ManageDebtors\` folder exists in `Orion\Components\` with `Business\bACTManageDebtors` and `Interface\iACTManageDebtors` project subdirectories, but contains **no `.vb` source files** (only `.vbproj.user` settings files). This component has no implementation to document.
