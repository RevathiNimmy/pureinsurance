# Orion Business Components Reference

> **Orion Accounting Module**
> Source: `Orion\Components\` — 69 business components (bACT* prefix).
> Each component resides under `Orion\Components\{Folder}\Business\{ComponentName}\`.

---

## Overview

The Orion module is the accounting and financial management engine for Pure Insurance. It handles chart of accounts, cash lists, documents (invoices/credit notes), transactions, allocations, bank reconciliation, budgets, currency management, commission processing, premium finance, period-end processing, and financial posting.

### Architecture

```
SAM WCF Service / Claims / Underwriting
    → Orion Business Component (bACT*)
        → Data Component (dACT*) → SQL Server Stored Procedures
        → Other bACT* Components (cross-component calls)
```

### Common Patterns
- All components implement `SSP.S4I.Interfaces.IBusiness` with `Initialise`/`SetProcessModes`/`Dispose`
- Entity/Collection pattern: `*Cls.vb` (entity) + `*s.vb` (collection) + `*Business.vb` (facade)
- Form classes handle UI-driven workflows (edit/add/delete/update/cancel/GetNext/GetDetails)
- Automated classes handle batch processing
- SQL constants in `*SQL.vb` or `*BusinessSQL.vb` files
- Hungarian notation: `v_` parameters, `r_` return refs, `s` strings, `l`/`i`/`n` integers

---

## Project Inventory

| # | Directory | Project | Purpose |
|---|-----------|---------|---------|
| 1 | Account | bACTAccount | Chart of accounts — account CRUD with hierarchy |
| 2 | Allocate | bACTAllocate | Allocation engine — match payments to transactions |
| 3 | Allocation | bACTAllocation | Allocation management — view/edit allocations |
| 4 | AllocationCalculate | bACTAllocationCalculate | Allocation calculation logic |
| 5 | AllocationCreate | bACTAllocationCreate | Create new allocations |
| 6 | AllocationDetail | bACTAllocationDetail | Allocation detail records |
| 7 | AllocationManual | bACTAllocationManual | Manual allocation processing |
| 8 | AllocationPost | bACTAllocationPost | Post allocations to ledger |
| 9 | AuditSet | bACTAuditSet | Audit set definition and management |
| 10 | AutoNumber | bACTAutoNumber | Auto-numbering sequences |
| 11 | Bank | bACTBank | Bank entity management |
| 12 | BankAccount | bACTBankAccount | Bank account CRUD |
| 13 | BankReconciliation | bACTBankReconciliation | Bank reconciliation processing |
| 14 | Budget | bACTBudget | Budget header management |
| 15 | BudgetDetail | bACTBudgetDetail | Budget detail line items |
| 16 | CashList | bACTCashList | **Core** — cash list header management |
| 17 | CashListDrawer | bACTCashListDrawer | Cash list drawer assignment |
| 18 | CashListItem | bACTCashListItem | **Core** — cash list item CRUD |
| 19 | CashListPost | bACTCashListPost | Cash list posting to ledger |
| 20 | ChequeProduction | bACTChequeProduction | Cheque printing and production |
| 21 | Agent Summary | bACTCommissionPayments | Agent commission payment processing |
| 22 | CommissionPost | bACTCommissionPost | Commission posting to ledger |
| 23 | Company | bACTCompany | Company entity management |
| 24 | CompanyCurrency | bACTCompanyCurrency | Company-currency mappings |
| 25 | Credit Card | bACTCreditCard | Credit card payment handling |
| 26 | CreditControl | bACTCreditControl | Credit control management |
| 27 | CreditControlItem | bACTCreditControlItem | Credit control item details |
| 28 | CreditControl | bACTCreditControlProcessing | Credit control batch processing |
| 29 | Currency | bACTCurrency | Currency entity management |
| 30 | CurrencyConvert | bACTCurrencyConvert | Currency conversion engine |
| 31 | CurrencyRate | bACTCurrencyRate | Currency exchange rates |
| 32 | Document | bACTDocument | **Core** — accounting document management |
| 33 | DocumentPost | bACTDocumentPost | Document posting to ledger |
| 34 | DocumentReversal | bACTDocumentReversal | Document reversal processing |
| 35 | AccountExplorer | bACTExplorer | Account explorer / drill-down |
| 36 | ExportCashListItems | bACTExportCashListItems | Export cash list items |
| 37 | ExportPFTrans | bACTExportPFTrans | Export premium finance transactions |
| 38 | FinanceSpoke | bACTFinanceSpoke | **Core** — finance spoke integration hub |
| 39 | FindAccount | bACTFindAccount | Account search |
| 40 | FindBank | bACTFindBank | Bank search |
| 41 | FindBudget | bACTFindBudget | Budget search |
| 42 | FindCashList | bACTFindCashList | Cash list search |
| 43 | FindCashListItem | bACTFindCashListItem | Cash list item search |
| 44 | FindDocument | bACTFindDocument | Document search |
| 45 | FindInvoice | bACTFindInvoice | Invoice search |
| 46 | FindTransaction | bACTFindTransaction | Transaction search |
| 47 | ImportExport | bACTImportExport | Data import/export engine |
| 48 | ImportSiriusTrans | bACTImportSiriusTrans | Import Sirius transactions |
| 49 | Instalments | bACTInstalments | Instalment plan management |
| 50 | InsurerPaymentAllocate | bACTInsurerPaymentAllocate | Insurer payment allocation |
| 51 | InsurerPaymentGroups | bACTInsurerPaymentGroups | Insurer payment grouping |
| 52 | InsurerPayment | bACTInsurerPaymentSFU | Insurer payment SFU processing |
| 53 | PurchaseInvoice | bACTInvoice | Purchase invoice management |
| 54 | PurchaseInvoiceItem | bACTInvoiceItem | Purchase invoice line items |
| 55 | Ledger | bACTLedger | Ledger entity management |
| 56 | MaintainMediaTypeStatus | bACTMaintainMediaTypeStatus | Media type status maintenance |
| 57 | MatchGroup | bACTMatchgroup | Transaction match group management |
| 58 | MatchPost | bACTMatchPost | Match group posting |
| 59 | MisAllocationHelper | bACTMisAllocationHelper | Mis-allocation correction helper |
| 60 | Payment Maintenance | bACTPaymentMaintenance | Payment maintenance and updates |
| 61 | Period | bACTPeriod | Accounting period management |
| 62 | PeriodEnd | bACTPeriodEnd | Period-end close processing |
| 63 | PremiumFinance | bACTPremiumFinance | Premium finance calculations |
| 64 | SuspendedTransactions | bACTReleaseManualTransactions | Release suspended/manual transactions |
| 65 | TransDetail | bACTTransdetail | Transaction detail records |
| 66 | TransMatch | bACTTransmatch | Transaction matching engine |
| 67 | TypeTable | bACTTypeTable | Type table lookups |
| 68 | UserAuthorities | bACTUserAuthorities | User authority definitions |
| 69 | WriteOffReason | bACTWriteOffReason | Write-off reason codes |

---

## Component Reference

---

### 1. bACTAccount
**Directory:** `Orion/Components/Account/Business/bACTAccount/`
**Project:** `bACTAccount`
**Purpose:** Manages insurance accounting accounts — CRUD operations, balance queries, account security, ledger lookups, credit control, and account status management.

**Business Methods — Form (bACTAccountForm.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `GetAccountSecurity` | `Public Function GetAccountSecurity(ByVal v_lAccountId As Integer, ByRef r_bHasUnrestrictedEnquiry As Boolean, ByRef r_bHasUnrestrictedUpdate As Boolean) As Integer` | Returns enhanced security settings for an account |
| `GetAccountStatus` | `Public Function GetAccountStatus(ByVal v_lAccountId As Integer, ByRef r_iAccountStatus As Integer) As Integer` | Gets account status for a given account (4 overloads with optional r_bIsStopped, r_sAccountCode) |
| `GetAccountLedger` | `Public Function GetAccountLedger(ByVal v_lAccountId As Integer, ByRef v_lLedgerId As Integer, ByRef v_sLedgerCode As String) As Integer` | Gets ledger details for an account |
| `GetAccountDetails` | `Public Function GetAccountDetails(ByRef r_lAccountID As Integer, ByRef sAccountName As String, ByRef sContactName As String, ByRef sPhoneAreaCode As String, ByRef sPhoneNumber As String, ByRef sPhoneExtension As String, ByRef r_vdAccountBalance As Object, ByRef r_sAccountCode As String) As Integer` | Gets account details, balance, and status code |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Navigator SetKeys function |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray As String) As Integer` | Navigator GetKeys (string overload) |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Navigator GetKeys (array overload) |
| `GetSummary` | `Public Function GetSummary(ByRef vSummaryArray As Object) As Integer` | Navigator GetSummary function |
| `Start` | `Public Function Start() As Integer` | Navigator Start entry point |
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises Form class, creates database, lookup, and accounts collection |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Terminates and releases resources |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process mode properties |
| `GetMandatory` | `Public Function GetMandatory(Optional ByRef lAccountIDMandy As Integer = 0, ... Optional ByRef lAllowElectronicPayment As Integer = 0) As Integer` | Returns mandatory field status flags for all account fields (~50 optional params) |
| `GetLookupValues` | `Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer` | Gets lookup values for account type, purge frequency, country, payment type, status, reports |
| `DirectAdd` | `Public Function DirectAdd(Optional ByRef vAccountID As Integer = 0, ... Optional ByRef vParamArray() As Object = Nothing) As Integer` | Directly adds an account to the database (~55 optional params) |
| `DirectDelete` | `Public Function DirectDelete(ByRef vID As Object) As Integer` | Directly deletes an account from database |
| `GetDefaults` | `Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, ... Optional ByRef vClientBankAccType As Object = Nothing) As Integer` | Gets default values for an account (~52 optional params) |
| `CheckID` | `Public Function CheckID(ByRef vID As Object) As Integer` | Checks if an account ID exists |
| `GetLedgerDetails` | `Public Function GetLedgerDetails(ByRef vResultArray(,) As Object) As Integer` | Gets all ledger details |
| `GetCaptions` | `Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, Optional ByRef vTable As Object = Nothing) As Integer` | Gets field captions (2 overloads) |
| `GetDetails` | `Public Function GetDetails(Optional ByRef vAccountID As Object = Nothing, Optional ByRef vLockMode As Integer = 0) As Integer` | Selects/loads account details |
| `GetNext` | `Public Function GetNext(Optional ByRef vAccountID As Object = Nothing, ...) As Integer` | Returns next account from collection |
| `EditAdd` | `Public Function EditAdd(ByRef lRow As Integer, ... Optional ByRef vParamArray() As Object = Nothing) As Integer` | Adds account to edit collection |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow As Integer, ... Optional ByRef vParamArray() As Object = Nothing) As Integer` | Updates account in edit collection |
| `EditDelete` | `Public Function EditDelete(ByVal lRow As Integer) As Integer` | Marks account for delete in collection |
| `Cancel` | `Public Function Cancel() As Integer` | Cancels pending edits |
| `Update` | `Public Function Update() As Integer` | Persists all edits to database |
| `GetAccountBalance` | `Public Function GetAccountBalance(ByRef r_vdAccountBalance As Object, ByVal v_vAccountID As Object) As Integer` | Gets account balance (4 overloads with accounting date, currency, debt/float) |
| `GetInstalmentDebt` | `Public Function GetInstalmentDebt(ByVal lAccountID As Integer, ByRef r_vdInstalmentDebt As Double) As Integer` | Gets instalment debt for an account |
| `GetAccountOSTransactions` | `Public Function GetAccountOSTransactions(ByRef vAccount_id As Object, ByRef vOSTransactions(,) As Object) As Integer` | Gets outstanding transactions for allocation (4 overloads) |
| `GetAccountOSTransactionsForReceipt` | `Public Function GetAccountOSTransactionsForReceipt(ByRef vAccount_id As Object, ByRef vOSTransactions(,) As Object, ByRef r_cAccountBaseBalance As Decimal) As Integer` | Gets outstanding transactions for receipt allocation (6 overloads) |
| `DeleteAllocationLocks` | `Public Function DeleteAllocationLocks(ByVal v_vOSTransactions(,) As Object) As Integer` | Deletes allocation lock records |
| `IsPostCode` | `Public Function IsPostCode(ByVal v_lAccountId As Integer, ByRef r_lResult As Integer) As Integer` | Checks if postal code is required for account country |
| `CreateAccountForCompany` | `Public Function CreateAccountForCompany(ByRef vAccountID As Integer, ByRef vAccountName As String, ByRef vShortName As String, ByRef vLedgerId As Integer, ByRef vMappingId As Integer, ByVal vCompanyId As String) As Integer` | Creates new account for a company branch |
| `New` | `Public Sub New()` | Constructor |
| `GetAccountBalanceLite` | `Public Function GetAccountBalanceLite(ByVal v_lAccountId As Integer, ByVal v_vCompanyID As Object, ByRef r_curBalance As Decimal) As Integer` | Lightweight account balance query |
| `GetAccountID` | `Public Function GetAccountID(ByVal v_sShortCode As String, ByRef r_lAccountID As Integer) As Integer` | Gets account ID from short code (2 overloads) |
| `GetGLAccount` | `Public Function GetGLAccount(ByVal v_cAmount As Decimal, ByRef r_lGLAccountID As Integer) As Integer` | Gets GL account based on amount (credit/debit) |
| `IsDeleted` | `Public Function IsDeleted(ByVal v_lAccountId As Integer, ByRef r_bIsDeleted As Boolean) As Integer` | Checks if account's linked party is deleted |
| `GetClientAccountDetails` | `Public Function GetClientAccountDetails(ByVal v_lAccountKey As Integer, ByVal v_lCompanyID As Integer, ByRef r_curYearToDateTurnover As Decimal, ByRef r_curLastYearTurnover As Decimal, ByRef r_curClientBalance As Decimal) As Integer` | Gets client account YTD turnover and balance (2 overloads) |
| `GetAccountOSCommForDocuments` | `Public Function GetAccountOSCommForDocuments(ByVal v_lAccountId As Integer, ByVal v_vDocumentIds As Object, ByRef r_vOSTransactions As Object) As Integer` | Gets outstanding commission transactions for documents |
| `GetAccountOSTransForDocuments` | `Public Function GetAccountOSTransForDocuments(ByVal v_lAccountId As Integer, ByVal v_vDocumentIds() As Object, ByRef r_vOSTransactions As Array) As Integer` | Gets outstanding transactions for documents |
| `MergeArrays` | `Public Function MergeArrays(ByRef r_vMainArray As Array, ByVal v_vArrayToAdd As Array) As Integer` | Merges two result arrays |
| `ArrayBoundsMatch` | `Public Function ArrayBoundsMatch(ByVal v_vArray1(,) As Object, ByVal v_vArray2(,) As Object, ByRef r_lLBound As Integer, ByRef r_lUBound As Integer) As Integer` | Checks if array bounds match |
| `GetAccountDetailsFromPartyCnt` | `Public Function GetAccountDetailsFromPartyCnt(ByVal v_lPartyCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets account details from party count |
| `GetUnallocatedClaimPayments` | `Public Function GetUnallocatedClaimPayments(ByVal v_lAccountId As Integer, ByRef r_vResults(,) As Object) As Integer` | Returns unallocated claim payments for account |
| `GetUnallocatedClaimPaymentsForPaymentDate` | `Public Function GetUnallocatedClaimPaymentsForPaymentDate(ByVal v_dtPaymentDateFrom As Date, ByVal v_dtPaymentDateTo As Date, ByRef r_vResults(,) As Object) As Integer` | Returns unallocated claim payments by payment date range |
| `IsValidAccountCode` | `Public Function IsValidAccountCode(ByVal m_sAccountCode As String, ByRef IsAccountCode As Boolean, ByRef Account_id As Integer) As gPMConstants.PMEReturnCode` | Validates an account short code |
| `GetBaseCountry` | `Public Function GetBaseCountry(ByVal v_lAccountId As Integer, ByRef r_lCountryId As Integer) As Integer` | Gets base country for an account |
| `GetAccountKey` | `Public Function GetAccountKey(ByVal lAccountID As Integer, ByRef r_lAccountKey As Integer) As Integer` | Gets the account_key from account_id |

**Business Methods — Account (bACTAccountCls.vb):**

Data transfer class with public properties only (AccountID, PurgefrequencyID, CurrencyID, CompanyID, SubBranchID, AccounttypeID, LedgerID, PaymenttypeID, AccountName, ShortCode, RestrictEnquiry, RestrictUpdate, DeleteAtPurge, ContactName, Address1-4, PostalCode, AddressCountry, PhoneAreaCode, PhoneNumber, PhoneExtension, FaxAreaCode, FaxNumber, FaxExtension, PaymentName, PaymentAccountCode, PaymentBranchCode, PaymentExpiryDate, PaymentReference1-2, ProofListReportID, BordereauReportID, CreditLimit, DiscountPercentage, SettlementPeriod, BankName, BankAddress1-4, BankPostalCode, BankCountry, BankPhoneAreaCode, BankPhoneNumber, BankPhoneExtension, BankFaxAreaCode, BankFaxNumber, BankFaxExtension, Comments, BIC, IBAN, AllowElectronicPayment, AccountKey, NominalAccountID, AccountStatusID, IsTakenOffHold, MoneyCalcAccType, ClientBankAccType, MerchantId, DatabaseStatus, Username). No public methods.

**Business Methods — Accounts (bACTAccounts.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Add` | `Public Function Add(ByRef oNewAccount As bACTAccount.Account) As Integer` | Adds Account to collection |
| `Count` | `Public Function Count() As Integer` | Returns number of accounts in collection |
| `Delete` | `Public Sub Delete(ByRef vKey As Integer)` | Removes account from collection by key |
| `Item` | `Public Function Item(ByRef vKey As Integer) As bACTAccount.Account` | Returns account from collection |
| `DeleteAll` | `Public Sub DeleteAll()` | Removes all accounts from collection |
| `Clear` | `Public Sub Clear()` | Clears collection and reinitializes |
| `Initialise` | `Public Function Initialise() As Integer` | Initialises the collection |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Terminates and releases resources |
| `GetAccountOSTransForClaimPayment` | `Public Function GetAccountOSTransForClaimPayment(ByVal v_lAccount_id As Long, ByVal v_lDocumentId As Long, ByRef vOSTransactions As Variant) As Long` | Retrieves outstanding debit transactions for account filtered by document/claim ID — commented out in current implementation |

**Stored Procedures (bACTAccountFormSQL.vb):**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `spu_ACT_select_Account` | `ACGetDetailsSQL` | Select account by ID |
| `spu_ACT_selall_Account` | `ACGetAllDetailsSQL` | Select all accounts |
| `spu_ACT_selall_Ledger` | `ACGetLedgerDetailsSQL` | Select all ledgers |
| `spu_ACT_check_Account` | `ACCheckIDSQL` | Check account ID exists |
| `spu_ACT_Add_WriteOffAccount` | `ACCheckCodeSQL` | Check short code uniqueness |
| `spu_ACT_add_Account` | `ACAddSQL` | Add new account |
| `spu_ACT_delete_Account` | `ACDeleteSQL` | Delete account |
| `spu_ACT_update_Account` | `ACUpdateSQL` | Update account |
| `spu_ACT_Select_AccountBal` | `ACSelectBalanceSQL` | Select account balance |
| `spu_ACT_get_Account_Ledger` | `ACGetAccountLedgerSQL` | Get account ledger details |
| `spu_ACT_Select_trans_for_allocation` | `ACSelectTransForAllocationSQL` | Select transactions for allocation |
| `spu_ACT_Select_Trans_For_Receipt_Allocation` | `ACSelectTransForReceiptAllocationSQL` | Select transactions for receipt allocation |
| `spu_ACT_Select_Trans_For_Allocation_For_Claim_Payment` | `ACSelectAccountOSTransForClaimPaymentSQL` | Select transactions for claim payment allocation |
| `spu_ACTSecurity_AccountRights` | `ACGetAccountSecuritySQL` | Get account security rights |
| `spu_ACT_Delete_Allocation_Locks` | `ACDeleteAllocationLocksSQL` | Delete allocation locks |
| `spu_ACT_Select_InstalmentDebt` | `ACSelectInstalmentDebtSQL` | Select instalment debt |
| `spu_ACT_Select_AccountBal_ByAccount` | `ACSelectBalanceLiteSQL` | Lightweight account balance |
| `spu_ACT_Select_Client_Account_Details` | `ACSelectClientAccountDetailsSQL` | Select client account details |
| `spu_ACT_Select_Trans_For_Allocation_For_Document` | `kGetAccountOSTransForDocumentSQL` | OS transactions for document |
| `spu_CLM_Get_Party_Account_Details` | `kGetAccountDetailsFromPartyCntSQL` | Account details from party |
| `spu_ACT_Get_Unallocated_Claim_Payments` | `kGetUnallocatedClaimPaymentsSQL` | Unallocated claim payments |
| `spu_ACT_Get_Unallocated_Claim_Payments_By_PaymentDate` | `kGetUnallocatedClaimPaymentsForPaymentDateSQL` | Unallocated claim payments by date |
| `spu_Get_Account_Base_Country` | `ACGetBaseCountrySQL` | Get account base country |
| `spu_ACT_isLedgerExists` | `ACIsLedgerExistSQL` | Check if ledger exists in related tables |
| `spu_ACT_Select_Comm_For_Allocation_For_Document` | `kGetAccountOSCommForDocumentSQL` | OS commission for document |
| `spu_ACT_Credit_Control_Take_Off_Hold` | `TakeOffHold` | Processes credit control hold removal for specified account |
| `spu_ACT_Get_AccountID_From_ShortCode` | `GetAccountIDFromShortCode` | Retrieves account_id using account short code as lookup |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `BPMLOOKUP.Business` | `Form` | Lookup value resolution (account types, statuses, countries) |
| `bACTAccount.Accounts` | `Form` | Collection of Account objects |
| `bACTAccount.Account` | `Form`, `Accounts` | Account data transfer object |

---

### 2. bACTAllocate
**Directory:** `Orion/Components/Allocate/Business/bACTAllocate/`
**Project:** `bACTAllocate`
**Purpose:** Find transactions for allocation, mark/unmark transactions, perform auto-allocation and manual allocation, handle write-offs and exchange rate differences.

**Business Methods — Business (bACTAllocate.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Navigator SetKeys |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray As String) As Integer` | Navigator GetKeys (string) |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Navigator GetKeys (array) |
| `GetSummary` | `Public Function GetSummary(ByRef vSummaryArray As Object) As Integer` | Navigator GetSummary |
| `Start` | `Public Function Start() As Integer` | Navigator Start |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTypeOfBusiness As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets process mode properties |
| `GetUserAuthorities` | `Public Function GetUserAuthorities() As Integer` | Gets user authority flags |
| `GetLookupValues` | `Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer` | Gets lookup values |
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises component with all sub-components |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Terminates and releases |
| `SelectTransQuery` | `Public Function SelectTransQuery(ByRef r_lNumberOfRecords As Integer, ByRef r_vResultArray(,) As Object, ByVal v_iCompanyID As Integer, Optional ByVal v_vAccountID As String = "", Optional ByVal v_vDocumentRef As Integer = 0, Optional ByVal v_vCurrencyID As Byte = 0, Optional ByVal v_vCurrencyAmount As Double = 0, Optional ByVal v_vTolerance As Byte = 0, Optional ByVal v_vDocTypeGroupId As String = "", Optional ByVal v_vDocumentTypeID As String = "", Optional ByVal v_vPeriodID As Object = Nothing, Optional ByVal v_vDateFrom As Byte = 0, Optional ByVal v_vDateTo As Byte = 0, Optional ByVal v_vInsuranceRef As String = "", Optional ByVal v_vUsername As Integer = 0, Optional ByVal v_vPurchaseInvoiceNo As Integer = 0, Optional ByVal v_vPurchaseOrderNo As Integer = 0, Optional ByVal v_vDepartment As Integer = 0, Optional ByVal v_vSpare As Integer = 0, Optional ByVal v_bMultiTreeAccounting As Boolean = False) As Integer` | Executes find transactions query with multiple filters |
| `MarkTransaction` | `Public Function MarkTransaction(ByVal v_lTransactionId As Integer, ByVal v_iCurrencyID As Integer, ByVal v_lCompanyID As Integer, ByVal v_cPayment As Decimal) As Integer` | Marks a transaction for allocation |
| `UnMarkTransaction` | `Public Function UnMarkTransaction(ByVal v_lTransDetailId As Integer) As Integer` | Unmarks a transaction |
| `AutoAllocate` | `Public Function AutoAllocate(ByVal v_lTransDetailId As Integer, ByRef r_sStatusCode As String, Optional ByVal v_lCashListItemID As Integer = 0, Optional ByVal v_lTransAccountID As Object = Nothing) As Integer` | Performs automatic allocation for a transaction |
| `PerformAutoAllocation` | `Public Function PerformAutoAllocation(ByRef r_lAccountId As Integer, ByRef r_lTransDetailId As Integer, ByVal v_vOSTransactions(,) As Object, Optional ByVal v_lCashListItemID As Integer = 0, Optional ByVal lWriteOffReasonID As Integer = 0, Optional ByVal cCurrencyWriteOff As Decimal = 0, Optional ByVal v_lCurExchangeRateGainLossReasonID As Integer = 0, Optional ByVal v_cCurGainLossAutoAllocationLimitAmount As Decimal = 0) As Integer` | Core auto-allocation logic |
| `Allocate` | `Public Function Allocate(Optional ByRef v_bInsurerBinder As Object = Nothing, Optional ByRef v_lCompanyID As Integer = 0, Optional ByRef v_lAccountID As Object = Nothing, ... Optional ByRef v_vTransIDs() As Object = Nothing) As Integer` | Main allocation entry point (~13 optional params) |
| `GetLedgerForAccount` | `Public Function GetLedgerForAccount(ByVal v_lAccountID As Integer, ByRef r_lLedgerID As Integer, ByRef r_sLedgerTypeCode As String) As Integer` | Gets ledger type for account |
| `New` | `Public Sub New()` | Constructor |
| `GetSymbolForCurrency` | `Public Function GetSymbolForCurrency(ByVal v_iCurrencyID As Integer, ByRef r_sSymbol As String) As Integer` | Gets currency symbol |
| `GetMarkedTransactions` | `Public Function GetMarkedTransactions(ByRef v_vMarkedTransactions(,) As Object, Optional ByRef v_CompanyID As Object = Nothing, ... Optional ByRef v_MultiTreeAccounting As String = "") As Integer` | Gets marked/selected transactions for allocation |
| `GetAccountDetails` | `Public Function GetAccountDetails(ByRef r_lAccountId As Integer, ByRef sAccountName As String, ByRef sContactName As String, ByRef sPhoneAreaCode As String, ByRef sPhoneNumber As String, ByRef sPhoneExtension As String, ByRef r_vdAccountBalance As Object, ByRef r_sAccountCode As String, ByRef r_vlAccountCurrencyId As Integer, Optional ByVal v_dtAccountingDate As Date = #12/30/1899#, Optional ByRef r_lCompanyID As Integer = 0) As Integer` | Gets account details with balance |
| `GetAccountID` | `Public Function GetAccountID(ByVal v_sShortCode As String, ByRef r_lAccountId As Integer) As Integer` | Gets account ID from short code |
| `FormatCurrency` | `Public Function FormatCurrency(ByRef vCurrencyID As Object, ByRef vCurrencyAmount As Object, ByRef vFormattedCurrency As Object, ByRef vConversionDate As Object) As Integer` | Formats currency amount |
| `GetRegSettings` | `Public Function GetRegSettings(ByRef sResult As String, ByRef sAppName As String, ByRef sSection As String, ByRef sKey As String, ByRef vDefault As Object) As Integer` | Gets registry settings |
| `GetSmallAmountWriteOffDetails` | `Public Function GetSmallAmountWriteOffDetails(ByVal v_lSourceID As Integer, Optional ByRef r_lAccountId As Integer = 0, Optional ByRef r_crNegativeAmount As Decimal = 0, Optional ByRef r_crPositiveAmount As Decimal = 0) As Integer` | Gets small amount write-off configuration |

**Stored Procedures (bACTAllocateSql.vb):**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `sp_ACT_Do_FindTrans` | `ACTransFromQuerySQL` | Find transactions query |
| `sp_ACT_Do_GetTransId` | `ACGetTransIDSQL` | Get transaction ID from parameters |
| `spu_ACT_Select_Marked_Details` | `ACGetMarkedDetailsSQL` | Select marked transmatch records |
| `spu_ACT_Delete_Transmatch` | `ACDeleteMarkedDetailsSQL` | Delete marked transmatch record |
| `spu_ACT_UnMark_TransMatch` | `ACUnMarkTransMatchSQL` | Unmark a transmatch record |
| `spu_ACT_Select_Small_Amount_Write_Off` | `ACGetSmallAmountWriteOffSQL` | Get small amount write-off details |
| `spu_ACT_Select_Trans_For_Allocation_FilterBy_PolicyRef` | `ACGetTransFilterByPolicyRefSQL` | Filter OS transactions by policy ref |
| `spu_ACT_Get_LedgerType_Code` | `GetLedgerTypeCode` | Gets ledger type code for account allocation |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTAllocation.Form` | `Business` | Allocation header management |
| `bACTAllocationdetail.Form` | `Business` | Allocation detail records |
| `bACTMatchgroup.Form` | `Business` | Match group management |
| `bACTTransmatch.Form` | `Business` | Transaction match records |
| `bACTCurrencyConvert.Form` | `Business` | Currency conversion |
| `bACTPeriod.Form` | `Business` | Period management |
| `bACTAccount.Form` | `Business` | Account operations |
| `bACTMatchPost.Form` | `Business` | Match posting |
| `bACTTransdetail.Form` | `Business` | Transaction detail operations |
| `bSIROptions.Business` | `Business` | System option lookups |
| `BPMLOOKUP.Business` | `Business` | Lookup value resolution |

---

### 3. bACTAllocation
**Directory:** `Orion/Components/Allocation/Business/bACTAllocation/`
**Project:** `bACTAllocation`
**Purpose:** Manages allocation header records — CRUD operations for allocation headers, cash item/list sum calculations, account ID resolution, and third-party scheme integration.

**Business Methods — Form (bACTAllocationForm.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `GetCashItemSum` | `Public Function GetCashItemSum(ByVal v_lCashListItemID As Integer, ByRef r_cCashItemSum As Decimal) As Integer` | Gets allocation sum for a cash list item |
| `GetCashListSum` | `Public Function GetCashListSum(ByVal v_lCashListID As Integer, ByRef r_cCashListSum As Decimal) As Integer` | Gets allocation sum for a cash list |
| `GetCashListCurrency` | `Public Function GetCashListCurrency(ByVal v_lCashListID As Integer, ByRef r_lCashListCurrency As Integer) As Integer` | Gets currency for a cash list |
| `GetAccountID` | `Public Function GetAccountID(ByVal v_sShortCode As String, ByRef r_lAccountID As Integer) As Integer` | Gets account ID from short code |
| `MatchTransactions` | `Public Function MatchTransactions(ByVal v_vTransactions() As Object) As Integer` | Matches allocation transactions |
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises the component |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Terminates and releases |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets process modes |
| `IsLinkedToThirdPartyScheme` | `Public Function IsLinkedToThirdPartyScheme(ByVal DocumentRef As String) As Boolean` | Checks if document is linked to 3rd party scheme |
| `ThirdPartyScheme` | `Public Function ThirdPartyScheme(ByVal AccountId As Long) As Boolean` | Checks if account uses 3rd party scheme |
| `DirectAdd` | `Public Function DirectAdd(Optional ByRef vAllocationID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, ...) As Integer` | Adds allocation directly to DB |
| `DirectDelete` | `Public Function DirectDelete(Optional ByRef vAllocationID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vUserID As Object = Nothing, Optional ByRef vAllocationDate As Object = Nothing, Optional ByRef vAllocationstatusID As Object = Nothing) As Integer` | Deletes allocation directly |
| `GetDefaults` | `Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vAllocationID As Object = Nothing, ...) As Integer` | Gets default values |
| `CheckID` | `Public Function CheckID(ByRef vID As Object) As Integer` | Checks allocation ID exists |
| `GetCaptions` | `Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, Optional ByRef vTable As Object = Nothing) As Integer` | Gets field captions (2 overloads) |
| `GetDetails` | `Public Function GetDetails(Optional ByRef vAllocationID As Object = Nothing, Optional ByRef vLockMode As Integer = 0) As Integer` | Select allocation details |
| `GetNext` | `Public Function GetNext(Optional ByRef vAllocationID As Object = Nothing, ...) As Integer` | Returns next allocation from collection |
| `EditAdd` | `Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vAllocationID As Object = Nothing, ...) As Integer` | Add allocation to edit collection |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vAllocationID As Object = Nothing, ...) As Integer` | Update allocation in collection |
| `EditDelete` | `Public Function EditDelete(ByVal lRow As Integer) As Integer` | Delete allocation from collection |
| `Cancel` | `Public Function Cancel() As Integer` | Cancels pending edits |
| `Update` | `Public Function Update() As Integer` | Persists edits to database |
| `GetSubBranch` | `Public Function GetSubBranch(ByRef r_lSubBranchID As Integer, ByVal v_vAccountID As Object, ByVal v_vTransDetailID As Object, ByVal v_vPeriodID As Object, ByVal v_vBankAccountID As Object) As Integer` | Gets sub-branch ID |
| `New` | `Public Sub New()` | Constructor |
| `GetToAllocateAmount` | `Public Function GetToAllocateAmount(ByRef r_cToAllocateAmount As Decimal, ByVal v_lCashListItemID As Integer, ByVal v_lCashListID As Integer) As Integer` | Calculates remaining allocation amount |

**Business Methods — Allocation (bACTAllocation.vb — Allocation data class):**

Data transfer class with properties: AllocationID, CompanyID, AccountID, UserID, AllocationDate, AllocationstatusID, DatabaseStatus.

**Business Methods — Allocations (bACTAllocations.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Add` | `Public Function Add(ByRef oNewAllocation As bACTAllocation.Allocation) As Integer` | Adds to collection |
| `Count` | `Public Function Count() As Integer` | Collection count |
| `Delete` | `Public Sub Delete(ByRef vKey As Integer)` | Remove from collection |
| `Item` | `Public Function Item(ByRef vKey As Integer) As bACTAllocation.Allocation` | Get from collection |
| `DeleteAll` | `Public Sub DeleteAll()` | Remove all |
| `Clear` | `Public Sub Clear()` | Clear collection |
| `Initialise` | `Public Function Initialise() As Integer` | Initialise collection |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Terminate |

**Stored Procedures (bACTAllocationFormSQL.vb):**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `spu_ACT_Select_Allocation` | `ACGetDetailsSQL` | Select allocation by ID |
| `spu_ACT_selall_Allocation` | `ACGetAllDetailsSQL` | Select all allocations |
| `spu_ACT_check_Allocation` | `ACCheckIDSQL` | Check allocation ID exists |
| `spu_ACT_add_Allocation` | `ACAddSQL` | Add allocation |
| `spu_ACT_delete_Allocation` | `ACDeleteSQL` | Delete allocation |
| `spu_ACT_update_Allocation` | `ACUpdateSQL` | Update allocation |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTAllocation.Allocation` | `Form`, `Allocations` | Allocation data object |
| `bACTAllocation.Allocations` | `Form` | Allocation collection |
| `BPMLOOKUP.Business` | `Form` | Lookup values |

---

### 4. bACTAllocationCalculate
**Directory:** `Orion/Components/AllocationCalculate/Business/bACTAllocationCalculate/`
**Project:** `bACTAllocationCalculate`
**Purpose:** Calculates allocation values including base/currency amounts, loss/gain, outstanding amounts, and creates payments for allocations.

**Business Methods — Form (bACTAllocationCalculateForm.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `GetKeys` | `Public Function GetKeys(ByRef vkeyarray(,) As Object) As Integer` | Navigator GetKeys |
| `SetKeys` | `Public Function SetKeys(ByRef vkeyarray(,) As Object) As Integer` | Navigator SetKeys |
| `Initialise` | `Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserId As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyId As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises the component |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Terminates |
| `CalculateValues` | `Public Function CalculateValues(ByVal v_iOriginalCurrency As Integer, ByVal v_lCompanyID As Integer, ByVal v_iAllocateToBase As Integer, ByVal v_vdOrigXrate As Object, ByVal v_vdEffectiveXrate As Object, ByVal v_cOsBaseAmount As Decimal, ByVal v_cOsCcyAmount As Decimal, ByRef r_cAllocBaseAmount As Decimal, ByRef r_cAllocCcyAmount As Decimal, ByRef r_cNewOsCcyAmount As Decimal, ByRef r_cNewOsBaseAmount As Decimal, ByRef r_cLossGainAmount As Decimal, ByRef r_iFullyMatched As Integer, Optional ByRef r_vdAllocBase... ) As Integer` | Calculates allocation amounts, OS amounts, and loss/gain |
| `TotalOfAllocation` | `Public Function TotalOfAllocation(ByVal v_lAllocationId As Integer, ByRef r_cTotalCcyAmount As Decimal, ByRef r_cTotalBaseAmount As Decimal, ByRef r_iCurrencyId As Integer, ByRef r_bSameCurrency As Boolean) As Object` | Gets total amounts for an allocation |
| `GetAllocationDetails` | `Public Function GetAllocationDetails(ByVal v_lAllocationId As Integer) As Integer` | Gets allocation detail records |
| `IsTransInAllocation` | `Public Function IsTransInAllocation(ByVal v_lTransactionId As Integer) As Integer` | Checks if a transaction exists in the allocation |

**Business Methods — Business (bACTAllocationCalculateBusiness.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserId As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyId As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises automated business class |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Terminates |
| `CalculateRecordValues` | `Public Function CalculateRecordValues(ByVal v_iOriginalCurrency As Integer, ByVal v_lCompanyID As Integer, ByVal v_bAllocateToBase As Boolean, ByVal v_vdOrigXrate As Object, ByVal v_vdEffectiveXrate As Object, ByVal v_cOsBaseAmount As Decimal, ByVal v_cOsCcyAmount As Decimal, ByRef r_cAllocBaseAmount As Decimal, ByRef r_cAllocCcyAmount As Decimal, ByRef r_cNewOsCcyAmount As Decimal, ByRef r_cNewOsBaseAmount As Decimal, ByRef r_cLossGainAmount As Decimal, ByRef r_bFullyMatched As Boolean, ...) As Integer` | Calculates record-level allocation values |
| `CalculateAllocationSet` | `Public Function CalculateAllocationSet(ByRef r_cTotalBaseAmount As Decimal, ByRef r_cTotalCcyAmount As Decimal, ByRef r_iCurrencyId As Integer, ByRef r_bSameCurrency As Boolean) As Integer` | Calculates totals for allocation set |
| `CreatePayment` | `Public Function CreatePayment() As Integer` | Creates payment for allocation |
| `AllocateSet` | `Public Function AllocateSet() As Integer` | Allocates the detail set |
| `GetAllocationDetails` | `Public Function GetAllocationDetails() As Integer` | Gets allocation detail records |
| `IsTransInAllocation` | `Public Function IsTransInAllocation(ByVal v_lTransactionId As Integer) As Integer` | Checks if transaction in allocation |

**Stored Procedures (bACTAllocationCalculateSQL.vb):**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `spu_ACT_Select_Allocation` | `ACGetDetailsSQL` | Select allocation |
| `spu_ACT_selall_Allocation` | `ACGetAllDetailsSQL` | Select all allocations |
| `spu_ACT_check_Allocation` | `ACCheckIDSQL` | Check allocation ID |
| `spu_ACT_add_Allocation` | `ACAddSQL` | Add allocation |
| `spu_ACT_delete_Allocation` | `ACDeleteSQL` | Delete allocation |
| `spu_ACT_update_Allocation` | `ACUpdateSQL` | Update allocation |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTAllocationdetail.Form` | `Form`, `Business` | Allocation detail management |
| `bACTCurrencyConvert.Form` | `Business` | Currency conversion |
| `bACTAccount.Form` | `Business` | Account operations |
| `BPMLOOKUP.Business` | `Form` | Lookups |

---

### 5. bACTAllocationCreate
**Directory:** `Orion/Components/AllocationCreate/Business/bACTAllocationCreate/`
**Project:** `bACTAllocationCreate`
**Purpose:** Creates allocations and allocation details — automated batch allocation creation, allocation from cash lists.

**Business Methods — Automated (bACTAllocationCreateAutomated.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `GetAllocationAmounts` | `Public Function GetAllocationAmounts(ByVal v_lAllocationID As Integer, ByRef r_cAllocBaseAmount As Decimal, ByRef r_cAllocCcyAmount As Decimal) As Integer` | Gets total allocation amounts |
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserId As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyId As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises the component |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Terminates |
| `GetBatchDetails` | `Public Function GetBatchDetails(Optional ByVal lBatchID As Integer = 0) As Integer` | Gets allocation batch details |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets process modes |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Navigator SetKeys |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Navigator GetKeys |
| `Start` | `Public Function Start() As Integer` | Navigator Start |
| `CreateAllocation` | `Public Function CreateAllocation(Optional ByVal v_dtAccountingDate As Date = #12/30/1899#) As Integer` | Creates a new allocation header |
| `CreateAllocationDetail` | `Public Function CreateAllocationDetail(ByVal v_lTransDetailId As Object, ...) As Integer` | Creates allocation detail line |
| `CreateAllocationForCashlist` | `Public Function CreateAllocationForCashlist(ByVal v_vCashListID As Integer, ByRef r_lAllocationId As Integer, Optional ByRef v_vCashListItemId() As Object = Nothing) As Integer` | Creates allocation for a cash list |
| `GetTransDetail` | `Public Function GetTransDetail(ByVal v_lTransDetailId As Integer) As Integer` | Gets transaction detail |
| `GetAllocation` | `Public Function GetAllocation(ByVal v_lAllocationID As Integer) As Integer` | Gets allocation header |
| `New` | `Public Sub New()` | Constructor |

**Stored Procedures (bACTAllocationCreateSQL.vb):**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `spu_ACT_Select_Allocation` | `ACGetDetailsSQL` | Select allocation |
| `spu_ACT_selall_Allocation` | `ACGetAllDetailsSQL` | Select all allocations |
| `spu_ACT_check_Allocation` | `ACCheckIDSQL` | Check allocation ID |
| `spu_ACT_add_Allocation` | `ACAddSQL` | Add allocation |
| `spu_ACT_delete_Allocation` | `ACDeleteSQL` | Delete allocation |
| `spu_ACT_update_Allocation` | `ACUpdateSQL` | Update allocation |
| `spu_ACT_Select_AllocationTotal_By_Allocation_ID` | `ACGetAllocationTotalSQL` | Get allocation total |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTAllocation.Form` | `Automated` | Allocation header management |
| `bACTAllocationdetail.Form` | `Automated` | Allocation detail management |
| `bACTAllocationCalculate.Form` | `Automated` | Allocation calculations |
| `bACTTransdetail.Form` | `Automated` | Transaction detail access |
| `bACTAccount.Form` | `Automated` | Account operations |
| `bACTCurrencyConvert.Form` | `Automated` | Currency conversion |
| `BPMLOOKUP.Business` | `Automated` | Lookup values |

---

### 6. bACTAllocationDetail
**Directory:** `Orion/Components/AllocationDetail/Business/bACTAllocationdetail/`
**Project:** `bACTAllocationdetail`
**Purpose:** Manages allocation detail records — individual line items within an allocation, including write-off, round-off, and loss/gain amounts.

**Business Methods — Form (bACTAllocationdetailForm.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `SetWriteOff` | `Public Function SetWriteOff(ByVal v_lAllocationDetailID As Integer, ByVal v_cWriteOffAmount As Decimal, ByVal v_lWriteOffReasonID As Integer) As Integer` | Sets write-off amount on allocation detail |
| `SetRoundOff` | `Public Function SetRoundOff(ByVal v_lTransDetailId As Integer, ByVal v_cRoundOffAmount As Decimal) As Integer` | Sets round-off amount on allocation detail |
| `SetLossGain` | `Public Function SetLossGain(ByVal v_lAllocationDetailID As Integer, ByVal v_cLossGainAmount As Decimal) As Integer` | Sets loss/gain amount on allocation detail |
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises the component |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Terminates |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, ...) As Integer` | Sets process modes |
| `GetLookupValues` | `Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer` | Gets lookup values |
| `DirectAdd` | `Public Function DirectAdd(Optional ByRef vAllocationDetailID As Object = Nothing, ...) As Integer` | Adds allocation detail directly (~30+ optional params including OrigBaseAmount, OrigCcyAmount, AllocBaseAmount, AllocCcyAmount, etc.) |
| `DirectDelete` | `Public Function DirectDelete(Optional ByRef vAllocationDetailID As Object = Nothing, ...) As Integer` | Deletes allocation detail directly |
| `GetDefaults` | `Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, ...) As Integer` | Gets default values |
| `CheckID` | `Public Function CheckID(ByRef vID As Object) As Integer` | Checks allocation detail ID exists |
| `GetCaptions` | `Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, Optional ByRef vTable As Object = Nothing) As Integer` | Gets captions (2 overloads) |
| `GetDetails` | `Public Function GetDetails(ByRef vAllocationId As Object) As Integer` | Gets all details for allocation (3 overloads with optional vAllocationDetailID, vLockMode) |
| `GetNext` | `Public Function GetNext(Optional ByRef vAllocationDetailID As Object = Nothing, Optional ByRef vCashlistitemID As Object = Nothing, ...) As Integer` | Returns next detail from collection |
| `EditAdd` | `Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vAllocationDetailID As Object = Nothing, ...) As Integer` | Adds to edit collection |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vAllocationDetailID As Object = Nothing, ...) As Integer` | Updates in collection |
| `EditDelete` | `Public Function EditDelete(ByVal lRow As Integer) As Integer` | Deletes from collection |
| `Cancel` | `Public Function Cancel() As Integer` | Cancels edits |
| `Update` | `Public Function Update() As Integer` | Persists edits |
| `New` | `Public Sub New()` | Constructor |

**Stored Procedures (bACTAllocationdetailFormSQL.vb):**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `spu_ACT_select_AllocationDetail` | `ACGetDetailsSQL` | Select allocation detail by ID |
| `spu_ACT_selall_AllocationDetail` | `ACGetAllDetailsSQL` | Select all allocation details |
| `spu_ACT_check_AllocationDetail` | `ACCheckIDSQL` | Check allocation detail ID |
| `spu_ACT_add_AllocationDetail` | `ACAddSQL` | Add allocation detail |
| `spu_ACT_delete_AllocationDetail` | `ACDeleteSQL` | Delete allocation detail |
| `spu_ACT_update_AllocationDetail` | `ACUpdateSQL` | Update allocation detail |
| `spu_ACT_select_DetailAllocation` | `ACSelectSQL` | Select detail by allocation |
| `spu_ACT_delete_Allocation` | `ACDeleteAllocationSQL` | Delete parent allocation |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTAllocationdetail.Allocationdetail` | `Form`, `Allocationdetails` | Data object |
| `bACTAllocationdetail.Allocationdetails` | `Form` | Collection |
| `BPMLOOKUP.Business` | `Form` | Lookups |

---

### 7. bACTAllocationManual
**Directory:** `Orion/Components/AllocationManual/Business/bACTAllocationManual/`
**Project:** `bACTAllocationManual`
**Purpose:** Manual allocation processing — transaction data retrieval, allocation/match group creation, write-offs, round-offs, currency exchange differences, and DD reversal allocations.

**Business Methods — Business (bACTAllocationManualBusiness.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises the component |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTypeOfBusiness As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets process modes |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Terminates |
| `SetStatus` | `Public Function SetStatus(ByVal sProcessStatus As String, ByVal sMapStatus As String, ByVal sStepStatus As String) As Integer` | Sets process status |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Navigator SetKeys |
| `GetKeys` | `Public Function GetKeys(ByRef oKeyArray(,) As Object) As Integer` | Navigator GetKeys |
| `Start` | `Public Function Start() As Integer` | Navigator Start (parameterless) |
| `Start` | `Public Function Start(ByVal v_bDisableTransactions As Boolean) As Integer` | Navigator Start with transaction control |
| `GetTransactionData` | `Public Function GetTransactionData() As Integer` | Retrieves outstanding transaction data for allocation |
| `DDReversalAllocation` | `Public Function DDReversalAllocation(ByRef sDocumentRef As String, ByRef sReversalDocumentRef As String) As Integer` | Performs DD reversal allocation |
| `CreateAllocation` | `Public Function CreateAllocation() As Integer` | Creates allocation header and details |
| `CreateMatchGroup` | `Public Function CreateMatchGroup() As Integer` | Creates match group for matched transactions |
| `CreateTransMatchForRoundOff` | `Public Function CreateTransMatchForRoundOff(ByVal lTranDetailIdofRoundOff As Integer, ByVal lTransDetailId As Integer, ByVal cRoundOffAmount As Decimal) As Integer` | Creates transmatch for round-off |
| `CreateTransMatch` | `Public Function CreateTransMatch(ByVal lRow As Integer, ByVal lAllocationDetailId As Integer) As Integer` | Creates transmatch record |
| `GetSymbolForCurrency` | `Public Function GetSymbolForCurrency(ByVal v_lCurrencyID As Integer, ByRef r_sSymbol As String) As Integer` | Gets currency symbol |
| `GetMatchPayment` | `Public Function GetMatchPayment(ByVal v_lTransdetailID As Integer, ByRef v_cBaseAmount As Decimal, ByRef v_cCurrencyAmount As Decimal) As Integer` | Gets match payment amounts |
| `WriteOff` | `Public Function WriteOff(ByVal v_lAllocationDetailId As Integer, ByVal v_iCurrencyID As Integer, ByVal v_cBaseAmount As Decimal, ...) As Integer` | Processes write-off (2 overloads) |
| `RoundOff` | `Public Function RoundOff(ByVal v_lTransDetailId As Integer, ByVal v_iCurrencyID As Integer, ByVal v_cBaseAmount As Decimal, ByVal v_vAccountID As Object, ByVal v_vIsCurrencyDifference As Object, ByVal v_vCompanyID As Object) As Integer` | Processes round-off |
| `GetWriteOffAccount` | `Public Function GetWriteOffAccount(ByVal v_sLedgerTypeCode As String, ByRef r_lWOAccountID As Integer) As Integer` | Gets write-off account for ledger type |
| `GetExchangeDiffAccount` | `Public Function GetExchangeDiffAccount(ByVal v_cCurrencyDifference As Object, ByRef r_lAccountID As Integer) As Integer` | Gets exchange difference GL account |
| `GetWriteOffDiffAccount` | `Public Function GetWriteOffDiffAccount(ByVal crCurrencyDifference As Decimal, ByRef r_nAccountID As Integer) As Integer` | Gets write-off difference GL account |
| `UpdateWriteOffDocument_Id` | `Public Function UpdateWriteOffDocument_Id(ByVal lTransDetailId As Integer, ByVal lDocumentId As Integer) As Integer` | Updates write-off document ID |

**Stored Procedures (MainModule.vb):**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `spu_ACT_Sel_Client_Postings` | `ACSelClientPostingsSQL` | Select client postings |
| `spu_ACT_Sel_Reversal` | `ACSelReversalSQL` | Select reversal details |
| `spu_ACT_Select_Document` | `ACSelDocumentSQL` | Get sub-branch from document |
| `spu_ACT_Select_Reversal_Transaction` | `ACSelDDReversalTransactionSQL` | Select DD reversal transaction |
| `spu_ACT_Update_RoundOff_TransMatch` | `ACUpdateRoundOffTransMatchSQL` | Update round-off transmatch |
| `spu_ACT_Add_AllocationBatch` | `kAddAllocationBatchSQL` | Add allocation batch |
| `spu_ACT_Get_LedgerType_Code` | `GetLedgerTypeCode` | Gets ledger type code for allocation account |
| `spu_update_writeoff_documentId` | `UpdateWriteOffDocument_Id` | Updates write-off document ID on transaction |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTAllocation.Form` | `Business` | Allocation header management |
| `bACTAllocationdetail.Form` | `Business` | Allocation detail management |
| `bACTAllocationCalculate.Business` | `Business` | Allocation calculations |
| `bACTMatchgroup.Form` | `Business` | Match group management |
| `bACTTransmatch.Form` | `Business` | Transaction match records |
| `bACTTransdetail.Form` | `Business` | Transaction detail access |
| `bACTAccount.Form` | `Business` | Account operations |
| `bACTCurrencyConvert.Form` | `Business` | Currency conversion |
| `bACTMatchPost.Form` | `Business` | Match posting |
| `bSIROptions.Business` | `Business` | System option lookups |
| `BPMLOOKUP.Business` | `Business` | Lookup values |

---

### 8. bACTAllocationPost
**Directory:** `Orion/Components/AllocationPost/Business/bACTAllocationPost/`
**Project:** `bACTAllocationPost`
**Purpose:** Posts completed allocations, reverses allocations, checks for allocation detail pairs, manages credit control re-addition, and handles void reversal logging.

**Business Methods — Business (bACTAllocationPostCls.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises the component |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Terminates |
| `GetSummary` | `Public Function GetSummary(ByRef vSummaryArray(,) As Object) As Integer` | Gets allocation summary |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets process modes |
| `PostAllocation` | `Public Function PostAllocation(ByVal v_vAllocationID As Object) As Integer` | Posts a completed allocation |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Navigator SetKeys |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Navigator GetKeys |
| `Start` | `Public Function Start() As Integer` | Navigator Start |
| `SetStatus` | `Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer` | Sets process status |
| `New` | `Public Sub New()` | Constructor |
| `DoAllocationDetailPairsExist` | `Public Function DoAllocationDetailPairsExist(ByRef r_bDoAllocationDetailPairsExist As Boolean, Optional ByVal v_lTransDetailID As Integer = 0, Optional ByVal v_lCashListItemID As Integer = 0, Optional ByVal v_lAllocationID As Integer = 0) As Integer` | Checks if allocation detail pairs exist |
| `ReverseAllocation` | `Public Function ReverseAllocation(Optional ByVal v_lTransDetailID As Integer = 0, ...) As Integer` | Reverses an allocation |
| `AddReversalDetailLog` | `Public Function AddReversalDetailLog(ByVal v_lLogid As Integer, ByVal v_lallocation_id As Integer, ByVal v_laccount_id As Integer) As Integer` | Adds reversal detail log entry |

**Stored Procedures (bACTAllocationPost.vb — MainModule):**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `spu_ACT_Do_Allocation_Reversal` | `ACReverseAllocationSQL` | Performs allocation reversal |
| `spu_ACT_Select_ReverseAllocation` | `ACSelectReverseAllocationSQL` | Selects reversal allocation details |
| `spu_ACT_Get_DoAllocationDetailPairsExist` | `ACDoAllocationDetailPairsExistSQL` | Check for detail pairs |
| `spu_ACT_ReAdd_Credit_Control_Item` | `ACReAddCredControlItemSQL` | Re-add credit control item after reversal |
| `spu_ACT_Get_DR_Allocation_Detail_IDs` | `ACGetDebitAllocationIDSQL` | Get debit allocation detail IDs |
| `spu_ACT_Get_SWD_Documents_In_Allocation` | `ACGetSWDDocumentsInAllocationSQL` | Get SWD documents in allocation |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTAllocation.Form` | `Business` | Allocation header management |
| `bACTAllocationdetail.Form` | `Business` | Allocation detail management |
| `bACTAllocationCalculate.Business` | `Business` | Calculations |
| `bACTMatchgroup.Form` | `Business` | Match group |
| `bACTTransmatch.Form` | `Business` | Transaction match |
| `bACTTransdetail.Form` | `Business` | Transaction details |
| `bACTAccount.Form` | `Business` | Account operations |
| `bACTMatchPost.Form` | `Business` | Match posting |
| `bSIROptions.Business` | `Business` | System options |

---

### 9. bACTAuditSet
**Directory:** `Orion/Components/AuditSet/Business/bACTAuditset/`
**Project:** `bACTAuditset`
**Purpose:** Manages audit set records — CRUD operations for audit sets used to group and track posted accounting batches, approvals, and rejections.

**Business Methods — Form (bACTAuditsetForm.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Terminates |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, ...) As Integer` | Sets process modes |
| `GetLookupValues` | `Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer` | Gets lookup values |
| `DirectAdd` | `Public Function DirectAdd(Optional ByRef vAuditsetID As Integer = 0, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vUserID As Object = Nothing, Optional ByRef vPostedDate As Object = Nothing, Optional ByRef vComment As Object = Nothing, Optional ByRef vDocumentID As Object = Nothing, Optional ByRef vAuditSetTypeID As Object = Nothing, Optional ByRef vApprovedDate As Object = Nothing, Optional ByRef vApprovedUserID As Object = Nothing, Optional ByRef vRejected As Object = Nothing, Optional ByRef vRejectedUserID As Object = Nothing, Optional ByRef vCashListItemID As Object = Nothing) As Integer` | Directly adds audit set |
| `DirectDelete` | `Public Function DirectDelete(Optional ByRef vAuditsetID As Object = Nothing, ...) As Integer` | Directly deletes audit set |
| `GetDefaults` | `Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, ...) As Integer` | Gets defaults |
| `CheckID` | `Public Function CheckID(ByRef vID As Object) As Integer` | Checks ID exists |
| `GetCaptions` | `Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, Optional ByRef vTable As Object = Nothing) As Integer` | Gets field captions |
| `GetDetails` | `Public Function GetDetails(Optional ByRef vAuditsetID As Object = Nothing, Optional ByRef vLockMode As Integer = 0) As Integer` | Selects audit set details |
| `GetNext` | `Public Function GetNext(Optional ByRef vAuditsetID As Object = Nothing, ...) As Integer` | Gets next from collection |
| `EditAdd` | `Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vAuditsetID As Object = Nothing, ...) As Integer` | Add to edit collection |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vAuditsetID As Object = Nothing, ...) As Integer` | Update in collection |
| `EditDelete` | `Public Function EditDelete(ByVal lRow As Integer) As Integer` | Delete from collection |
| `Cancel` | `Public Function Cancel() As Integer` | Cancel edits |
| `Update` | `Public Function Update() As Integer` | Persist edits |
| `New` | `Public Sub New()` | Constructor |
| `GetAuditSetFromCashListItemID` | `Public Function GetAuditSetFromCashListItemID(ByVal v_lCashListItemID As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Gets audit sets for a cash list item |

**Business Methods — Auditset (bACTAuditset.vb):**

Data transfer class with properties: AuditsetID, CompanyID, PostedDate, Comment, DocumentID, AuditSetTypeID, ApprovedDate, ApprovedUserID, Rejected, RejectedUserID, CashListItemID, DatabaseStatus.

**Stored Procedures (bACTAuditsetFormSQL.vb):**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `spu_ACT_select_AuditSet` | `ACGetDetailsSQL` | Select audit set by ID |
| `spu_ACT_select_all_AuditSet` | `ACGetAllDetailsSQL` | Select all audit sets |
| `spu_ACT_check_AuditSet` | `ACCheckIDSQL` | Check audit set ID exists |
| `spu_ACT_add_AuditSet` | `ACAddSQL` | Add audit set |
| `spu_ACT_delete_AuditSet` | `ACDeleteSQL` | Delete audit set |
| `spu_ACT_update_AuditSet` | `ACUpdateSQL` | Update audit set |
| `spu_ACT_selall_AuditSet` | `ACGetAuditSetbyCashListSQL` | Select all audit sets (by cash list) |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTAuditset.Auditset` | `Form`, `Auditsets` | Audit set data object |
| `bACTAuditset.Auditsets` | `Form` | Audit set collection |
| `BPMLOOKUP.Business` | `Form` | Lookup values |

---

### 10. bACTAutoNumber
**Directory:** `Orion/Components/AutoNumber/Business/bACTAutoNumber/`
**Project:** `bACTAutoNumber`
**Purpose:** Generates unique sequential numbers — number ranges, pool management, document reference number generation, and Long-to-alphanumeric encoding.

**Business Methods — Business (bACTAutoNumberCls.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises the component |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Terminates |
| `GetNumberRange` | `Public Function GetNumberRange(ByVal v_sGroupCode As String, ByVal v_sRangeCode As String, ByRef r_lNumberRangeID As Integer) As Integer` | Gets number range ID from group/range codes |
| `GenerateNumber` | `Public Function GenerateNumber(ByVal v_lNumberRangeID As Integer, ByVal v_iUserID As Integer, ByVal v_iCompanyID As Integer, ByRef r_lNumber As Integer) As Integer` | Generates next sequential number |
| `PoolNumber` | `Public Function PoolNumber(ByVal v_lNumberRangeID As Integer, ByVal v_iUserID As Integer, ByVal v_iCompanyID As Integer, ByRef r_lNumber As Integer) As Integer` | Gets number from pool, or generates if pool empty |
| `New` | `Public Sub New()` | Constructor |
| `EncodeLong` | `Public Function EncodeLong(ByVal lNumber As Integer, ByRef r_sCode As String) As Integer` | Encodes a number to alphanumeric code |
| `EncodeLongV2` | `Public Function EncodeLongV2(ByVal v_lNumber As Integer, ByRef r_sCode As String) As Integer` | V2 encoding with different character set |
| `GenerateDocumentReferenceNumber` | `Public Function GenerateDocumentReferenceNumber(ByVal v_sRangeCode As String, ByVal v_iUserID As Integer, ByVal v_iCompanyID As Integer, ByRef r_sDocumentRef As String) As Integer` | Generates document reference number (2 overloads, one with v_lNumberRangeID) |

**Stored Procedures (bACTAutoNumberSql.vb):**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `spe_ACTNumber_pool_sel` | `ACSelectPoolNumberSQL` | Select pool number |
| `spe_ACTNumber_pool_del` | `ACDeletePoolNumberSQL` | Delete pool number |
| `spe_ACTnumber_upd` | `ACUpdateNumberSQL` | Update number user/date |
| `spe_ACTnumber_add` | `ACAllocateNumberSQL` | Allocate new number |
| `spe_ACTNumber_pool_add` | `ACAddPoolNumberSQL` | Add pool number |
| `spu_ACT_get_number_range` | `ACGetNumberRangeIDSQL` | Get number range ID |
| `spu_ACT_Generate_Next_Unique_Document_Reference` | `ACAllocateUniqueNumberSQL` | Generate unique document reference |
| `spu_ACT_Get_Number_Range_From_Code` | `ACGetNumberRangeFromCodeSQL` | Get number range from code |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| None | — | Standalone utility component |

---

### 11. bACTBank
**Directory:** `Orion/Components/Bank/Business/bACTBank/`
**Project:** `bACTBank`
**Purpose:** Manages bank records — CRUD operations for banks, head office relationships, bank account listings, and automated bank processing.

**Business Methods — Form (bACTBankForm.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise` | Initialises (implements IBusiness) |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Terminates |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, ...) As Integer` | Sets process modes |
| `DirectAdd` | `Public Function DirectAdd(Optional ByRef vBankId As Integer = 0, Optional ByRef vCode As Object = Nothing, Optional ByRef vBranchCode As Object = Nothing, Optional ByRef vBankName As Object = Nothing, Optional ByRef vHeadOffice As Object = Nothing, Optional ByRef vBankAddress1-4 As Object = Nothing, Optional ByRef vBankPostalCode As Object = Nothing, ...) As Integer` | Adds bank directly |
| `DirectDelete` | `Public Function DirectDelete(Optional ByRef vBankId As Object = Nothing, ...) As Integer` | Deletes bank directly |
| `GetDefaults` | `Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, ...) As Integer` | Gets defaults |
| `CheckID` | `Public Function CheckID(ByRef vID As Object) As Integer` | Checks bank ID |
| `GetCaptions` | `Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, Optional ByRef vTable As Object = Nothing) As Integer` | Gets field captions |
| `GetDetails` | `Public Function GetDetails(Optional ByRef vBankId As Object = Nothing, Optional ByRef vLockMode As gPMConstants.PMELockMode = 0) As Integer` | Select bank details |
| `GetNext` | `Public Function GetNext(Optional ByRef vBankId As Object = Nothing, ...) As Integer` | Gets next from collection |
| `GetOtherDetails` | `Public Function GetOtherDetails(Optional ByRef vHeadOfficeId As Object = Nothing, Optional ByRef vHeadOFficeName As String = "") As Integer` | Gets head office details |
| `GetLookupValues` | `Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer` | Gets lookup values |
| `GetAccountDetails` | `Public Function GetAccountDetails(ByRef vBankId As Object, ByRef vAccounts(,) As Object) As Integer` | Gets bank accounts for bank |
| `EditAdd` | `Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vBankId As Object = Nothing, ...) As Integer` | Add to collection |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow As Integer, ...) As Integer` | Update in collection |
| `EditDelete` | `Public Function EditDelete(ByVal lRow As Integer) As Integer` | Delete from collection |
| `Cancel` | `Public Function Cancel() As Integer` | Cancel edits |
| `UpdateBank` | `Public Function UpdateBank(ByVal v_iBankID As Integer, ByVal v_lBankAccountID As Integer, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer` | Updates bank with bank account link |
| `Update` | `Public Function Update() As Integer` | Persists edits |
| `GetBankId` | `Public Function GetBankId(Optional ByRef vBankRef As Object = Nothing, Optional ByRef vBankId As Integer = 0) As Integer` | Gets bank ID from reference |
| `GetBranchBaseCountry` | `Public Function GetBranchBaseCountry(ByVal v_lSourceID As Integer, ByRef r_iCountryID As Integer) As Integer` | Gets base country for branch |
| `DeleteBankAccount` | `Public Function DeleteBankAccount(ByVal v_lBankAccountID As Integer) As Integer` | Deletes a bank account |
| `New` | `Public Sub New()` | Constructor |

**Business Methods — Bank (bACTBankCls.vb):**

Data transfer class with properties (BankId, Code, BranchCode, BankName, HeadOffice, BankAddress1-4, BankPostalCode, CountryId, BankPhoneAreaCode, BankPhoneNumber, BankPhoneExtension, BankFaxAreaCode, BankFaxNumber, BankFaxExtension, CompanyID, DatabaseStatus). Includes `Initialise()` and `Dispose()`.

**Business Methods — Banks (bACTBanks.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Add` | `Public Function Add(ByRef oNewBank As bACTBank.Bank) As Integer` | Adds to collection |
| `Count` | `Public Function Count() As Integer` | Collection count |
| `Delete` | `Public Sub Delete(ByRef vKey As Integer)` | Remove from collection |
| `Item` | `Public Function Item(ByRef vKey As Integer) As bACTBank.Bank` | Get from collection |
| `DeleteAll` | `Public Sub DeleteAll()` | Remove all |
| `Clear` | `Public Sub Clear()` | Clear collection |
| `Initialise` | `Public Function Initialise() As Integer` | Initialise |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Terminate |

**Stored Procedures (bACTBankFormSQL.vb):**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `spu_ACT_Select_Bank` | `ACGetDetailsSQL` | Select bank by ID |
| `spu_ACT_SelAll_Bank` | `ACGetAllDetailsSQL` | Select all banks |
| `spu_ACT_Check_Bank` | `ACCheckIDSQL` | Check bank ID exists |
| `spu_ACT_Add_Bank` | `ACAddSQL` | Add bank |
| `spu_ACT_Delete_Bank` | `ACDeleteSQL` | Delete bank |
| `spu_ACT_Update_Bank` | `ACUpdateSQL` | Update bank |
| `spu_ACT_Get_BankAccount` | `ACGetBankAccountSQL` | Get bank accounts |
| `spu_ACT_GetBankByShortCode` | `ACGetBankIdSQL` | Get bank by short code |
| `spu_ACT_Delete_BankAccount` | `ACDeleteBankAccountSQL` | Delete bank account |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTBank.Bank` | `Form`, `Banks` | Bank data object |
| `bACTBank.Banks` | `Form` | Bank collection |
| `BPMLOOKUP.Business` | `Form` | Lookup values |

---

### 12. bACTBankAccount
**Directory:** `Orion/Components/BankAccount/Business/bACTBankAccount/`
**Project:** `bACTBankAccount`
**Purpose:** Manages bank account records — CRUD, bank account rules, statement balances, delays, source associations, cheque checking, and pick list management.

**Business Methods — Form (bACTBankAccountForm.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Terminates |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, ...) As Integer` | Sets process modes |
| `GetLookupValues` | `Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray As Object, ByRef iLanguageID As Integer, ByRef vResultArray As Object) As Integer` | Gets lookup values |
| `DirectAdd` | `Public Function DirectAdd(Optional ByRef vBankAccountId As Integer = 0, Optional ByRef vCurrencyId As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vAccountId As Object = Nothing, Optional ByRef vBankID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vBankAccountNo As Object = Nothing, Optional ByRef vBankAccountName As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vNextChequeNumber As Object = Nothing, ...) As Integer` | Adds bank account directly |
| `DirectDelete` | `Public Function DirectDelete(Optional ByRef vBankAccountId As Object = Nothing, ...) As Integer` | Deletes bank account directly |
| `GetDefaults` | `Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, ...) As Integer` | Gets defaults |
| `CheckID` | `Public Function CheckID(ByRef vID As Object) As Integer` | Checks bank account ID |
| `GetCaptions` | `Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, Optional ByRef vTable As Object = Nothing) As Integer` | Gets field captions |
| `GetDetails` | `Public Function GetDetails(Optional ByRef vBankAccountId As Object = Nothing, Optional ByRef vLockMode As Integer = 0, Optional ByRef v_iSourceID As Integer = 0) As Integer` | Select bank account details |
| `GetNext` | `Public Function GetNext(Optional ByRef vBankAccountId As Object = Nothing, ...) As Integer` | Gets next from collection |
| `GetOtherDetails` | `Public Function GetOtherDetails(Optional ByRef vAccountHolderId As Object = Nothing, Optional ByRef vAccountHolderName As String = "") As Integer` | Gets account holder details |
| `CheckAccount` | `Public Function CheckAccount(ByRef vAccountId As Object, Optional ByRef vBankAccountId As Integer = 0) As Integer` | Checks account mapping |
| `EditAdd` | `Public Function EditAdd(ByRef lRow As Integer, ...) As Integer` | Add to edit collection |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow As Integer, ...) As Integer` | Update in collection |
| `EditDelete` | `Public Function EditDelete(ByVal lRow As Integer, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "") As Integer` | Delete from collection |
| `Cancel` | `Public Function Cancel() As Integer` | Cancel edits |
| `Update` | `Public Function Update() As Integer` | Persist edits |
| `New` | `Public Sub New()` | Constructor |
| `GetBankAccountRules` | `Public Function GetBankAccountRules(ByVal v_lBankAccountID As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Gets reconciliation rules for bank account |
| `AddBankAccountRule` | `Public Function AddBankAccountRule(ByRef r_lBankAccountRuleID As Integer, ByRef v_lBankAccountID As Integer, ByRef v_lMediaTypeID As Integer, ByRef v_iMatchToTransdetail As Integer, ByRef v_iMatchAccountCode As Integer, ByRef v_iCodeIsMerchantNumber As Integer, ByRef v_iMatchBatchNumber As Integer, ByRef v_iBatchIsRemitCode As Integer, ByRef v_iMatchChequeNumber As Integer, ByRef v_iMatchAmount As Integer, ByRef v_iMatchDate As Integer, ByRef v_iSkipIfReasonNull As Integer, ByRef v_iActive As Integer) As Integer` | Adds reconciliation rule |
| `UpdateBankAccountRule` | `Public Function UpdateBankAccountRule(ByRef v_lBankAccountRuleID As Integer, ByRef v_lBankAccountID As Integer, ByRef v_lMediaTypeID As Integer, ...) As Integer` | Updates reconciliation rule |
| `DeleteBankAccountRule` | `Public Function DeleteBankAccountRule(ByVal v_lBankAccountRuleID As Integer) As Integer` | Deletes reconciliation rule |
| `GetBankStatementBalance` | `Public Function GetBankStatementBalance(ByVal lAccountID As Integer, ByRef r_cBalance As Decimal) As Integer` | Gets bank statement balance |
| `UpdateBankStatementBalance` | `Public Function UpdateBankStatementBalance(ByVal lAccountID As Integer, ByVal cBalance As Decimal) As Integer` | Updates bank statement balance |
| `SelectBankAccountDelay` | `Public Function SelectBankAccountDelay(ByVal lBankAccountID As Integer, ByVal lMediaTypeID As Integer, ByRef r_vBankAccountDelay As Object) As Integer` | Gets bank account delay settings |
| `AddBankAccountDelay` | `Public Function AddBankAccountDelay(ByVal lBankAccountID As Object, ByVal lMediaTypeID As Object, ByVal iDelay As Integer, Optional ByRef r_lBankAccountDelayID As Integer = 0, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer` | Adds bank account delay |
| `UpdateBankAccountDelay` | `Public Function UpdateBankAccountDelay(ByVal lBankAccountDelayID As Integer, ByVal lBankAccountID As Object, ByVal lMediaTypeID As Object, ByVal iDelay As Integer, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer` | Updates bank account delay |
| `DeleteBankAccountDelay` | `Public Function DeleteBankAccountDelay(ByVal lBankAccountDelayID As Integer, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer` | Deletes bank account delay |
| `PickListLoad` | `Public Function PickListLoad(ByVal sPickListType As String, ByVal vFKArray(,) As Object, ByRef vResultArray(,) As Object) As Integer` | Loads pick list data (source associations) |
| `PickListSave` | `Public Function PickListSave(ByRef sPickListType As String, ByRef vFKArray(,) As Object, ByRef vKeys() As Object) As Integer` | Saves pick list data (source associations) |
| `CheckSourceAssociatedWithDefaultBank` | `Public Function CheckSourceAssociatedWithDefaultBank(ByVal v_lBankAccountID As Integer, ByVal v_lSourceID As Integer, ByRef r_bIsSourceAssociatedWithDefaultBank As Boolean) As Integer` | Checks source/default bank association |
| `CheckSourceAssociatedWithBank` | `Public Function CheckSourceAssociatedWithBank(ByVal v_lBankAccountID As Integer, ByVal v_lSourceID As Integer, ByRef r_bIsSourceAssociatedWithBank As Boolean) As Integer` | Checks source/bank association |
| `CheckSourceAttachedWithBank` | `Public Function CheckSourceAttachedWithBank(ByVal v_lBankAccountID As Integer, ByRef r_bIsSourceAttachedWithBank As Boolean) As Integer` | Checks if any source attached to bank |
| `IsChequeExistForBankAccount` | `Public Function IsChequeExistForBankAccount(ByVal v_lBankAccountID As Integer, ByRef r_bExit As Boolean) As Integer` | Checks if cheques exist for bank account |

**Business Methods — BankAccount (bACTBankAccountCls.vb):**

Data transfer class with properties and `Initialise`/`Dispose`. Properties include: BankAccountId, CurrencyId, CompanyID, AccountId, BankID, Code, BankAccountNo, BankAccountName, Description, NextChequeNumber, Dormant, DefaultForPayments, DefaultForReceipts, AccountHolderID, BIC, IBAN, SubBranchID, DatabaseStatus.

**Business Methods — BankAccounts (bACTBankAccounts.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Add` | `Public Function Add(ByRef oNewBankAccount As bACTBankAccount.BankAccount) As Integer` | Adds to collection |
| `Count` | `Public Function Count() As Integer` | Collection count |
| `Delete` | `Public Sub Delete(ByRef vKey As Integer)` | Remove from collection |
| `Item` | `Public Function Item(ByRef vKey As Integer) As bACTBankAccount.BankAccount` | Get from collection |
| `DeleteAll` | `Public Sub DeleteAll()` | Remove all |
| `Clear` | `Public Sub Clear()` | Clear collection |
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ...) As Integer` | Initialise with credentials |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Terminate |

**Stored Procedures (bACTBankAccountFormSQL.vb):**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `spu_ACT_Select_BankAccount` | `ACGetDetailsSQL` | Select bank account by ID |
| `spu_ACT_SelAll_BankAccount` | `ACGetAllDetailsSQL` | Select all bank accounts |
| `spu_ACT_Check_BankAccount` | `ACCheckIDSQL` | Check bank account ID |
| `spu_ACT_Add_BankAccount` | `ACAddSQL` | Add bank account |
| `spu_ACT_Delete_BankAccount` | `ACDeleteSQL` | Delete bank account |
| `spu_ACT_Get_BankAccount_Cash` | `ACCheckDeleteOkSQL` | Check OK to delete bank account |
| `spu_ACT_Set_BankAccount_IsDeleted` | `ACSetBankAccountIsDeletedSQL` | Set bank account as deleted |
| `spu_ACT_Update_BankAccount` | `ACUpdateSQL` | Update bank account |
| `spu_ACT_Check_BankAccount_Mapping` | `ACCheckACcountSQL` | Check account mapping |
| `spu_ACT_Get_BankAccount_Rules` | `ACGetBankAccountRulesSQL` | Get reconciliation rules |
| `spu_ACT_Add_BankAccount_Rule` | `ACAddBankAccountRuleSQL` | Add reconciliation rule |
| `spu_ACT_Update_BankAccount_Rule` | `ACUpdateBankAccountRuleSQL` | Update reconciliation rule |
| `spu_ACT_Delete_BankAccount_Rule` | `ACDeleteBankAccountRuleSQL` | Delete reconciliation rule |
| `spu_ACT_DeleteAll_BankAccount_Rule` | `ACDeleteAllRulesForAccountSQL` | Delete all rules for account |
| `spu_ACT_Get_Bank_Statement_Balance` | `ACGetBankStatementBalanceSQL` | Get statement balance |
| `spu_ACT_Update_Bank_Statement_Balance` | `ACUpdateBankStatementBalanceSQL` | Update statement balance |
| `spu_ACT_Get_All_Sources_Linked_With_BankAc` | `ACGetAllSourcesLinkedWithBankAcSQL` | Get linked sources |
| `spu_ACT_Add_Sources_Linked_With_BankAc` | `ACAddSourcesLinkedWithBankAcSQL` | Add source link |
| `spu_ACT_Del_Sources_Linked_With_BankAc` | `ACDelSourcesLinkedWithBankAcSQL` | Delete source link |
| `spu_ACT_Get_All_Sources_Linked_With_BankAcDefault` | `ACGetAllSourcesLinkedWithBankAcDefaultSQL` | Get default source links |
| `spu_ACT_SelAll_BankAccountLinkedToSource` | `ACSelAllBankAccountLinkedToSourceSQL` | Select bank accounts linked to source |
| `spu_ACT_Get_Sources_List_For_PickList` | `ACGetSourcesListForPickListSQL` | Get sources for pick list |
| `spu_get_ChequeExistForBankAccount` | `ACGetChequeForBankAccountSQL` | Check cheques exist |
| `spu_ACT_Add_BankAccount_Delay` | `AddBankAccountDelay` | Inserts a new bank account delay record |
| `spu_ACT_Delete_BankAccount_Delay` | `DeleteBankAccountDelay` | Deletes a bank account delay record |
| `spu_ACT_Select_BankAccount_Delay` | `SelectBankAccountDelay` | Retrieves bank account delay records for a specific bank account and media type |
| `spu_ACT_Update_BankAccount_Delay` | `UpdateBankAccountDelay` | Updates an existing bank account delay record |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTBankAccount.BankAccount` | `Form`, `BankAccounts` | Bank account data object |
| `bACTBankAccount.BankAccounts` | `Form` | Bank account collection |
| `BPMLOOKUP.Business` | `Form` | Lookup values |

---

### 13. bACTBankReconciliation
**Directory:** `BankReconciliation/`
**Project:** `bACTBankReconciliation`
**Purpose:** Bank reconciliation — searching, marking/unmarking transactions for reconciliation, reconciling marked transactions against bank accounts, and managing account balance and currency formatting.

**Business Methods — Business (Business.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises the component with credentials, database, and child components (bPMLock, bACTFindTransaction) |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTypeOfBusiness As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process modes (no-op in this component) |
| `SetStatus` | `Public Function SetStatus(ByVal sProcessStatus As String, ByVal sMapStatus As String, ByVal sStepStatus As String) As Integer` | Sets status (no-op) |
| `SearchDetails` | `Public Function SearchDetails(ByVal v_lMarkedStatus As Integer, ByVal v_lMonth As Integer, Optional ByVal v_vAccountID As Object = Nothing, Optional ByVal v_vDateTo As String = "", Optional ByRef lNumberOfRecords As Integer = 0, Optional ByRef r_vResultArray(,) As Object = Nothing, Optional ByRef r_cTotalReconciled As Decimal = 0, Optional ByRef r_cTotalUnreconciled As Decimal = 0) As Integer` | Searches bank reconciliation records by marked status, month, account, and date; returns totals |
| `MarkTransaction` | `Public Function MarkTransaction(ByVal v_lTransactionID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_cPayment As Decimal) As Integer` | Creates a fake TransMatch entry to mark a transaction as ready to be paid |
| `UnMarkTransaction` | `Public Function UnMarkTransaction(ByVal v_lTransDetailID As Integer) As Integer` | Unmarks a previously marked transaction by deleting its temporary TransMatch entry |
| `Reconcile` | `Public Function Reconcile(ByVal vTransDetailIDs() As Object) As Integer` | Sets the spare field in bank transactions to "RECONCILED" for an array of transaction detail IDs |
| `GetSymbolForCurrency` | `Public Function GetSymbolForCurrency(ByVal v_lCurrencyID As Integer, ByRef r_sSymbol As String) As Integer` | Gets the currency symbol for a given currency ID |
| `LockBankAccount` | `Public Function LockBankAccount(ByVal v_lAccountID As Integer, ByRef r_sCurrentlyLockedBy As String) As Integer` | Acquires a pessimistic lock on a bank account for reconciliation |
| `UnLockBankAccount` | `Public Function UnLockBankAccount(ByVal v_lAccountID As Integer) As Integer` | Releases the lock on a bank account |
| `GetAccountBalance` | `Public Function GetAccountBalance(ByRef r_vAccountBalance As Double, ByVal v_vAccountID As Object, ByVal v_vAccountingDate As Object, ByRef r_sFormattedBalance As String) As Integer` | Gets the account balance for a bank account as of a date, with formatted currency |
| `CurrencyFormat` | `Public Function CurrencyFormat(ByVal v_cAmount As Decimal, ByVal v_iCurrencyID As Integer, ByRef r_sFormattedAmount As String) As Integer` | Formats a decimal amount using the specified currency |
| `GetBankReconciliationTotals` | `Public Function GetBankReconciliationTotals(ByVal v_lMonth As Long, ByVal v_lAccountID As Long, ByVal v_vDateTo As Variant, ByRef r_vTotalArray As Variant) As Long` | Returns reconciled and unreconciled totals for bank reconciliation by month and account — commented out in current implementation |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_ACT_Do_BankReconciliation` | `SearchDetails` | Search bank reconciliation records with output totals |
| `spu_ACT_Do_BankReconciliation_Totals` | *(commented out GetBankReconciliationTotals)* | Get reconciliation totals (defined but not currently called) |
| `spu_ACT_UnMark_TransMatch` | `UnMarkTransaction` | Delete temporary TransMatch entries for unmarking |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTMatchPost.Form` | `MarkTransaction` | Creates TransMatch entries when marking transactions |
| `bACTTransDetail.Form` | `Reconcile` | Gets/updates transaction detail spare field to "RECONCILED" |
| `bPMLock.User` | `LockBankAccount`, `UnLockBankAccount` | Pessimistic locking for bank account reconciliation |
| `bACTFindTransaction.Business` | `GetAccountBalance`, `CurrencyFormat` | Account detail retrieval and currency formatting |

---

### 14. bACTBudget
**Directory:** `Budget/`
**Project:** `bACTBudget`
**Purpose:** Budget management — CRUD operations for budget records, retrieving unique period years, budget status lookup, and updating actuals and variances. Standard Navigator-pattern Form/Automated/DTO/Collection classes.

**Business Methods — Form (bACTBudgetForm.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises component with credentials and database |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process modes |
| `DirectAdd` | `Public Function DirectAdd(Optional ByRef vBudgetID As Integer = 0, Optional ByRef vBudgetRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vBudgetDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesBudgetID As Object = Nothing, Optional ByRef vBudgetStatusID As Object = Nothing) As Integer` | Adds a budget directly to the database (bypasses collection) |
| `DirectDelete` | `Public Function DirectDelete(Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vBudgetDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesBudgetID As Object = Nothing, Optional ByRef vBudgetStatusID As Object = Nothing) As Integer` | Deletes a budget directly from the database |
| `GetDefaults` | `Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vBudgetDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesBudgetID As Object = Nothing, Optional ByRef vBudgetStatusID As Object = Nothing) As Integer` | Returns default values for a new budget |
| `CheckID` | `Public Function CheckID(ByRef vID As Object) As Integer` | Checks if a supplied budget ID is valid |
| `GetCaptions` | `Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, Optional ByRef vTable As Object = Nothing) As Integer` | Gets specified caption fields for a budget record |
| `GetDetails` | `Public Function GetDetails(Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vLockMode As gPMConstants.PMELockMode = 0) As Integer` | Loads budgets into collection; by ID or all records |
| `GetNext` | `Public Function GetNext(Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vBudgetDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesBudgetID As Object = Nothing, Optional ByRef vBudgetStatusID As Object = Nothing) As Integer` | Gets next budget from collection and returns its properties |
| `EditAdd` | `Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vBudgetDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesBudgetID As Object = Nothing, Optional ByRef vBudgetStatusID As Object = Nothing) As Integer` | Adds a budget to the in-memory collection |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vBudgetDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesBudgetID As Object = Nothing, Optional ByRef vBudgetStatusID As Object = Nothing) As Integer` | Updates a budget in the collection |
| `EditDelete` | `Public Function EditDelete(ByVal lRow As Integer) As Integer` | Deletes a budget from the collection |
| `Cancel` | `Public Function Cancel() As Integer` | Cancels pending collection changes |
| `Update` | `Public Function Update() As Integer` | Writes all collection changes to the database |
| `GetUniquePeriodYears` | `Public Function GetUniquePeriodYears(ByRef r_vYearArray(,) As Object) As Integer` | Gets unique list of period year names (delegates to bACTPeriod) |
| `GetDetailsForBudgetID` | `Public Function GetDetailsForBudgetID(ByVal v_lBudgetID As Integer, ByRef r_vBudgetDetails(,) As Object) As Integer` | Gets budget details for a specific budget ID |
| `GetBudgetStatus` | `Public Function GetBudgetStatus(ByVal v_lBudgetStatus As Integer, ByRef r_sDescription As String, ByRef r_vStatusList(,) As Object) As Integer` | Gets budget status description and full status list from PostingStatus table |
| `UpdateActualsAndVariances` | `Public Function UpdateActualsAndVariances(ByVal v_lPeriodID As Integer) As Integer` | Updates actuals and variances for a period |

**Business Methods — Automated (bACTBudgetAutomated.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As dPMDAO.Database = Nothing) As Integer` | Initialises automated class |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets process modes |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Stores key parameters (stub) |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Returns key parameters (stub) |
| `Start` | `Public Function Start() As Integer` | Performs the automated action (stub) |

**Business Methods — ACTBudget DTO (bACTBudget.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise() As Integer` | Initialises the DTO |

**Business Methods — ACTBudgets Collection (bACTBudgets.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Add` | `Public Function Add(ByRef oNewACTBudget As bACTBudget.ACTBudget) As Integer` | Adds a budget to the collection |
| `Count` | `Public Function Count() As Integer` | Returns number of items |
| `Delete` | `Public Sub Delete(ByRef vKey As Integer)` | Deletes a budget by index |
| `Item` | `Public Function Item(ByRef vKey As Integer) As bACTBudget.ACTBudget` | Returns a budget by index |
| `DeleteAll` | `Public Sub DeleteAll()` | Deletes all budgets |
| `Clear` | `Public Sub Clear()` | Clears the collection |
| `Initialise` | `Public Function Initialise() As Integer` | Initialises the collection |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_ACT_select_Budget` | `GetDetails` (by ID) | Select a single budget by ID |
| `spu_ACT_select_all_Budget` | `GetDetails` (all) | Select all budget records |
| `spu_ACT_check_Budget` | `CheckID` | Check if a budget ID exists |
| `spu_ACT_add_Budget` | `DirectAdd`, `Update` | Add a new budget record |
| `spu_ACT_delete_Budget` | `DirectDelete`, `Update` | Delete a budget record |
| `spu_ACT_update_Budget` | `Update` | Update a budget record |
| `spu_ACT_Do_GetDetailForBudgetId` | `GetDetailsForBudgetID` | Get details for a specific budget ID |
| `spu_ACT_Do_UpdateActVar` | `UpdateActualsAndVariances` | Update actuals and variances |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTPeriod.Form` | `GetUniquePeriodYears` | Gets unique period year names |

---

### 15. bACTBudgetDetail
**Directory:** `BudgetDetail/`
**Project:** `bACTBudgetDetail`
**Purpose:** Budget detail line-item management — CRUD for individual budget line items (budget detail records per account/period), lookup values, budget posting check, account name retrieval, period listing, and bulk detail deletion.

**Business Methods — Form (bACTBudgetDetailForm.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises with credentials and database |
| `GetBudgetRef` | `Public Function GetBudgetRef(ByVal v_vBudgetID As Object, ByRef r_sBudgetRef As String) As Integer` | Gets budget reference from a budget ID |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process modes |
| `GetLookupValues` | `Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer` | Gets lookup values for budget detail fields |
| `DirectAdd` | `Public Function DirectAdd(Optional ByRef vBudgetDetailID As Integer = 0, Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetSequence As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vBudgetAmount As Object = Nothing, Optional ByRef vActualAmount As Object = Nothing, Optional ByRef vVarianceAmount As Object = Nothing) As Integer` | Adds a budget detail directly to the database |
| `DirectDelete` | `Public Function DirectDelete(Optional ByRef vBudgetDetailID As Object = Nothing, Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetSequence As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vBudgetAmount As Object = Nothing, Optional ByRef vActualAmount As Object = Nothing, Optional ByRef vVarianceAmount As Object = Nothing) As Integer` | Deletes a budget detail directly |
| `GetDefaults` | `Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vBudgetDetailID As Object = Nothing, Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetSequence As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vBudgetAmount As Object = Nothing, Optional ByRef vActualAmount As Object = Nothing, Optional ByRef vVarianceAmount As Object = Nothing) As Integer` | Returns default values for a new budget detail |
| `CheckPosted` | `Public Function CheckPosted(ByRef r_bPosted As Boolean, ByVal v_lBudgetID As Integer) As Integer` | Checks if a budget has been posted |
| `CheckID` | `Public Function CheckID(ByRef vID As Object) As Integer` | Checks if a budget detail ID exists |
| `GetCaptions` | `Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, Optional ByRef vTable As Object = Nothing) As Integer` | Gets specified caption fields for a budget detail |
| `GetDetails` | `Public Function GetDetails(Optional ByRef vBudgetDetailID As Object = Nothing, Optional ByRef vLockMode As gPMConstants.PMELockMode = 0) As Integer` | Loads budget details into collection |
| `GetNext` | `Public Function GetNext(Optional ByRef vBudgetDetailID As Object = Nothing, Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetSequence As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vBudgetAmount As Object = Nothing, Optional ByRef vActualAmount As Object = Nothing, Optional ByRef vVarianceAmount As Object = Nothing) As Integer` | Gets next detail from collection |
| `EditAdd` | `Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vBudgetDetailID As Object = Nothing, Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetSequence As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vBudgetAmount As Object = Nothing, Optional ByRef vActualAmount As Object = Nothing, Optional ByRef vVarianceAmount As Object = Nothing) As Integer` | Adds detail to collection |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vBudgetDetailID As Object = Nothing, Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetSequence As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vBudgetAmount As Object = Nothing, Optional ByRef vActualAmount As Object = Nothing, Optional ByRef vVarianceAmount As Object = Nothing) As Integer` | Updates detail in collection |
| `EditDelete` | `Public Function EditDelete(ByVal lRow As Integer) As Integer` | Deletes detail from collection |
| `Cancel` | `Public Function Cancel() As Integer` | Cancels pending changes |
| `Update` | `Public Function Update() As Integer` | Writes collection changes to database |
| `GetDetailsForBudgetID` | `Public Function GetDetailsForBudgetID(ByVal v_lBudgetID As Integer, ByRef r_vBudgetDetails As Object, Optional ByRef r_sAccountName As String = "") As Integer` | Gets all details for a budget ID |
| `GetBudgetAccountName` | `Public Function GetBudgetAccountName(ByVal v_lAccountID As Object, ByRef r_sAccountName As String) As Integer` | Gets the account name for an account ID |
| `GetPeriodsForYear` | `Public Function GetPeriodsForYear(ByVal v_sPeriodYearName As String, ByRef r_vPeriods(,) As Object) As Integer` | Gets periods in a given year |
| `DeleteBudgetDetails` | `Public Function DeleteBudgetDetails(ByRef vBudgetID As Integer) As Integer` | Deletes all details for a budget ID |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_ACT_select_Budget_Detail` | `GetDetails` (by ID) | Select a single budget detail |
| `spu_ACT_select_all_Budget_Detail` | `GetDetails` (all) | Select all budget details |
| `spu_ACT_check_Budget_Detail` | `CheckID` | Check if a budget detail ID exists |
| `spu_ACT_add_Budget_Detail` | `DirectAdd`, `Update` | Add a new budget detail record |
| `spu_ACT_delete_Budget_Detail` | `DirectDelete`, `Update` | Delete a budget detail record |
| `spu_ACT_update_Budget_Detail` | `Update` | Update a budget detail record |
| `spu_ACT_Do_GetAccName` | `GetBudgetAccountName` | Get account name for an account ID |
| `spu_ACT_delete_Budget_Details` | `DeleteBudgetDetails` | Delete all details for a budget |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `BPMLOOKUP.Business` | `GetLookupValues` | Lookup value retrieval |

---

### 16. bACTCashList
**Directory:** `CashList/`
**Project:** `bACTCashList`
**Purpose:** Cash list management — CRUD operations for cash list records (receipts and payments collections), banking and receipt processing, currency denominations, adjustments, cash float management, auto-bank total calculation, payment/receipt media types, batch status, journal processing (binder), claim links, and currency conversion.

**Business Methods — Form (bACTCashListForm.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises with credentials and database |
| `SetProcessModes` | `Public Function SetProcessModes(...)` | Sets optional process modes |
| `GetLookupValues` | `Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer` | Gets lookup values for cash list fields |
| `DirectAdd` | `Public Function DirectAdd(Optional ByRef vCashListID As Integer = 0, Optional ByRef vCashListStatusID As Object = Nothing, ... Optional ByRef vSubBranchID As Object = Nothing) As Integer` | Adds a cash list directly to the database |
| `DirectDelete` | `Public Function DirectDelete(Optional ByRef vCashListID As Object = Nothing, ...) As Integer` | Deletes a cash list directly |
| `GetDefaults` | `Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, ...) As Integer` | Returns default values |
| `CheckID` | `Public Function CheckID(ByRef vID As Object) As Integer` | Checks if a cash list ID exists |
| `GetBankAccountDefault` | `Public Function GetBankAccountDefault(Optional ByRef vSourceID As Object = Nothing, Optional ByRef vCashListTypeID As Object = Nothing, Optional ByRef vDefaultAccountID As Object = Nothing, Optional ByRef vMediaTypeId As Object = Nothing) As Integer` | Gets the default bank account for a cash list type |
| `GetAllUserCashListDrawer` | `Public Function GetAllUserCashListDrawer(ByVal v_lUserId As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Gets all cash list drawers for a user |
| `GetUserBankingAuthorisation` | `Public Function GetUserBankingAuthorisation(ByVal v_lUserId As Integer, ByVal v_lCashDrawerId As Integer, ByRef r_bAuthorised As Boolean) As Integer` | Checks if user has banking authorisation for a drawer |
| `GetLocks` | `Public Function GetLocks(ByVal v_sLockName As String, ByVal v_lLockValue As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Gets lock records |
| `GetCaptions` | `Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, Optional ByRef vTable As Object = Nothing) As Integer` | Gets caption fields |
| `GetDetails` | `Public Function GetDetails(Optional ByRef vCashListID As Object = Nothing, Optional ByRef vLockMode As Integer = 0) As Integer` | Loads cash lists into collection |
| `GetNext` | `Public Function GetNext(Optional ByRef vCashListID As Object = Nothing, ...) As Integer` | Gets next cash list from collection |
| `EditAdd` | `Public Function EditAdd(ByRef lRow As Integer, ...) As Integer` | Adds to collection |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow As Integer, ...) As Integer` | Updates in collection |
| `EditDelete` | `Public Function EditDelete(ByVal lRow As Integer) As Integer` | Deletes from collection |
| `Cancel` | `Public Function Cancel() As Integer` | Cancels pending changes |
| `Update` | `Public Function Update() As Integer` | Writes to database |
| `BeginTrans` | `Public Function BeginTrans() As Integer` | Begins a database transaction |
| `CommitTrans` | `Public Function CommitTrans() As Integer` | Commits a database transaction |
| `RollbackTrans` | `Public Function RollbackTrans() As Integer` | Rolls back a database transaction |
| `GetCurrencyDenom` | `Public Function GetCurrencyDenom(ByRef vCurrencyDenom(,) As Object, ByRef lCurrencyId As Integer) As Integer` | Gets currency denominations for cash counting |
| `GetBankingItems` | `Public Function GetBankingItems(ByRef vBankingItems(,) As Object, ByRef lCashListID As Integer) As Integer` | Gets banking items for a cash list |
| `AddAdjustment` | `Public Function AddAdjustment(ByVal v_lCashListID As Integer, ByVal v_lPMUserID As Integer, ByVal v_cAmount As Decimal, ByVal v_lAdjustMethod As Integer, ByVal v_sReason As String) As Integer` | Adds an adjustment to a cash list |
| `CashFloat` | `Public Function CashFloat(ByRef blnCashFloat As Boolean, ByRef lCashListDrawerID As Integer) As Integer` | Checks cash float status for drawer |
| `GetAdjustmentMethods` | `Public Function GetAdjustmentMethods(ByRef vAdjustMethods(,) As Object) As Integer` | Gets available adjustment methods |
| `ListAdjustments` | `Public Function ListAdjustments(ByRef vAdjustments(,) As Object, ByRef lCashList_ID As Integer) As Integer` | Lists all adjustments for a cash list |
| `GetAdjustment` | `Public Function GetAdjustment(ByRef vAdjustments(,) As Object, ByRef lCashList_Adjustment_ID As Integer) As Integer` | Gets a specific adjustment |
| `SaveCash` | `Public Function SaveCash(ByVal v_lCashListID As Integer, ByVal v_lCurrencyDenomID As Integer, ByVal v_blnFloat As Boolean, ByVal v_lAmount As Integer) As Integer` | Saves cash denomination count |
| `LoadCash` | `Public Function LoadCash(ByRef vCashData(,) As Object, ByRef lCashListID As Integer) As Integer` | Loads cash denomination data for a cash list |
| `GetCashListStatus` | `Public Function GetCashListStatus(ByRef vCashlistStatusCode As Object, ByRef lCashlistStatueID As Integer) As Integer` | Gets cash list status code |
| `GetCashDrawerName` | `Public Function GetCashDrawerName(ByRef vCashListName As String, ByRef lCashDrawerID As Integer) As Integer` | Gets cash drawer name |
| `GetBankAccounts` | `Public Function GetBankAccounts(ByVal v_lCashListID As Integer, Optional ByRef r_vCollectionAccount As Integer = 0, Optional ByRef r_vBankAccount As Integer = 0, Optional ByRef r_vAdjustmentAccount As Integer = 0, Optional ByRef r_vSuspenseAccount As Integer = 0) As Integer` | Gets bank accounts associated with a cash list |
| `ProcessBinder` | `Public Function ProcessBinder(ByVal v_lCreditAccountId As Integer, ByVal v_lDebitAccountId As Integer, ByVal v_iCurrencyID As Integer, ByVal v_cPayment As Decimal, Optional ByRef r_vCreditTransDetailId As Integer = 0, Optional ByRef r_vDebitTransDetailId As Integer = 0, Optional ByVal v_lCashListID As Integer = 0, Optional ByVal v_lCompanyID As Integer = 0, Optional ByVal v_lSubBranchID As Integer = 0, Optional ByVal v_bSecondApprove As Boolean = False, Optional ByVal v_vJournalType As String = "", Optional ByVal v_vCashListDrawerID As Integer = 0) As Integer` | Processes a binder journal entry (credit/debit) |
| `GetCashListAutoBankTotal` | `Public Function GetCashListAutoBankTotal(ByRef r_cAutoBankTotal As Decimal, ByRef v_lCashListID As Integer) As Integer` | Gets auto-bank total for a cash list |
| `GetBatchStatusDetails` | `Public Function GetBatchStatusDetails(ByVal v_lBatchID As Integer, ByRef r_sBatchStatusDescription As String) As Integer` | Gets batch status description |
| `GetCashListStatusCode` | `Public Function GetCashListStatusCode(ByVal v_lCashListID As Integer, ByRef r_sStatusCode As String) As Integer` | Gets status code for a cash list |
| `GetMatchingDebits` | `Public Function GetMatchingDebits(ByVal v_lCashListID As Integer, ByRef r_vMatchingDebits(,) As Object, Optional ByVal v_vIsBanking As Object = Nothing) As Integer` | Gets matching debit transactions |
| `GetPaymentMediaTypeIDs` | `Public Function GetPaymentMediaTypeIDs(ByRef r_vResultArray(,) As Object) As Integer` | Gets payment media type IDs |
| `GetReceiptMediaTypeIDs` | `Public Function GetReceiptMediaTypeIDs(ByRef r_vResultArray(,) As Object) As Integer` | Gets receipt media type IDs |
| `GetAccountFromParty` | `Public Function GetAccountFromParty(ByVal v_lPartyCnt As Integer, ByVal v_lSourceID As Integer, ByRef r_lAccountID As Integer) As Integer` | Gets account ID from party key |
| `GetOSCashForDebit` | `Public Function GetOSCashForDebit(ByVal v_lAccountId As Integer, ByVal v_sDocumentRef As String, ByVal v_lSourceID As Integer, ByRef r_iCurrencyID As Integer, ByRef r_cCash As Decimal) As Integer` | Gets outstanding cash for a debit |
| `GetBranchBaseCurrency` | `Public Function GetBranchBaseCurrency(ByVal v_lSourceID As Integer, ByRef r_iCurrencyID As Integer) As Integer` | Gets base currency for a branch |
| `ConvertPaymentAmount` | `Public Function ConvertPaymentAmount(ByVal v_iCurrencyTo As Integer, ByVal v_vDocumentIds() As Object, ByVal v_lAccountId As Integer, ByRef r_crPaymentAmount As Decimal) As Integer` | Converts payment amount to target currency |
| `AddInputParameter` | `Public Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer` | Adds a database input parameter |
| `CheckCurrencyRate` | `Public Function CheckCurrencyRate(ByVal v_lCurrencyID As Integer, ByRef r_bCurrencyRateExist As Boolean) As Integer` | Checks if a currency rate exists |
| `CheckClaimLink` | `Public Function CheckClaimLink(ByVal v_lClaimPaymentId As Integer, ByRef r_bResults As Boolean) As Integer` | Checks if a claim link exists for a claim payment |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_ACT_Select_CashList` | `GetDetails` (by ID), `GetCashDrawerName` | Select a single cash list |
| `spu_ACT_SelAll_CashList` | `GetDetails` (all) | Select all cash lists |
| `spu_ACT_Check_CashList` | `CheckID` | Check if a cash list ID exists |
| `spu_ACT_Add_CashList` | `DirectAdd`, `Update` | Add a new cash list |
| `spu_ACT_Delete_CashList` | `DirectDelete`, `Update` | Delete a cash list |
| `spu_ACT_Update_CashList` | `Update` | Update a cash list |
| `spu_ACT_bank_default_Cashlist` | `GetBankAccountDefault` | Get default bank account for cash list type |
| `spu_ACT_Select_CashListByCashDrawerId` | `GetDetails` (by drawer) | Select cash lists by cash drawer ID |
| `spu_ACT_SelAll_User_CashListDrawer` | `GetAllUserCashListDrawer` | Get all user cash list drawers |
| `spu_ACT_Select_UserBankingAuthorisation` | `GetUserBankingAuthorisation` | Check user banking authorisation |
| `spu_ACT_Select_Locks` | `GetLocks` | Select locks |
| `spu_ACT_Get_Cur_Denominations` | `GetCurrencyDenom` | Get currency denominations |
| `spu_ACT_Get_Banking_Items` | `GetBankingItems` | Get banking items for a cash list |
| `spu_ACT_Add_Adjustment` | `AddAdjustment` | Add an adjustment to a cash list |
| `spu_ACT_Check_CashFloat` | `CashFloat` | Check cash float status |
| `spu_ACT_SelAll_AdjMethods` | `GetAdjustmentMethods` | Get all adjustment methods |
| `spu_ACT_GetAdjustments` | `ListAdjustments` | List adjustments for a cash list |
| `spu_ACT_GetAdjustment` | `GetAdjustment` | Get a single adjustment |
| `spu_ACT_Get_BankAccountIds` | `GetBankAccounts` | Get bank account IDs |
| `spu_ACT_Add_Cashlist_Cash` | `SaveCash` | Save cash denomination data |
| `spu_ACT_Get_Cashlist_Cash` | `LoadCash` | Load cash denomination data |
| `spu_ACT_Get_Cashlist_Status` | `GetCashListStatus` | Get cash list status |
| `spu_ACT_Select_CashListDrawer` | `GetCashDrawerName` | Select cash list drawer |
| `spu_ACT_Sel_Batch_Status_Details` | `GetBatchStatusDetails` | Get batch status details |
| `spu_ACT_Get_CashlistStatusCode` | `GetCashListStatusCode` | Get status code for a cash list |
| `spu_ACT_Select_Matching_CashList_Debits` | `GetMatchingDebits` | Get matching debit transactions |
| `spu_ACT_Select_MediaType_filtered` | `GetPaymentMediaTypeIDs`, `GetReceiptMediaTypeIDs` | Get filtered media types |
| `spu_ACT_Get_CashList_AutoBankTotal` | `GetCashListAutoBankTotal` | Get auto-bank total |
| `spu_ACT_Get_Document_Details_For_Account` | `GetOSCashForDebit`, `ConvertPaymentAmount` | Get document details for an account |
| `spu_ACT_Check_Currency_Rate` | `CheckCurrencyRate` | Check if currency rate exists |
| `spu_ACT_Check_Claim_link_For_ClaimPaymentId` | `CheckClaimLink` | Check claim link for a claim payment |
| `spu_ACT_Do_Get_OS_Cash_For_Client_Debit` | `GetOutstandingCashForClientDebit` | Gets outstanding cash for client debit with date and company ID parameters |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTAllocate.Business` | `ProcessBinder` | Allocation processing |
| `bACTCurrencyConvert.Form` | `ConvertPaymentAmount` | Currency conversion |
| `bACTDocument.Form` | `ProcessBinder` | Document operations |
| `bACTExplorer.Form` | Various | Transaction explorer operations |
| `bACTPeriod.Form` | `ProcessBinder` | Period lookups |
| `BPMLOOKUP.Business` | `GetLookupValues` | Lookup value retrieval |

---

### 17. bACTCashListDrawer
**Directory:** `CashListDrawer/`
**Project:** `bACTCashListDrawer`
**Purpose:** Cash list drawer configuration — manages cash drawers (physical or logical points of sale), including CRUD operations, security (user group access), banking authorisation (user-level), media type and receipt type associations, and user group/branch lookups.

**Business Methods — Business (bBusiness.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises with credentials and database |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process modes |
| `GetAllCashListDrawer` | `Public Function GetAllCashListDrawer(ByRef r_vResultArray(,) As Object) As Integer` | Selects all cash list drawer records |
| `GetDetails` | `Public Function GetDetails(ByVal v_lCashlistDrawerId As Integer, Optional ByRef r_vCompanyId As Object = Nothing, Optional ByRef r_vCode As Object = Nothing, Optional ByRef r_vDescription As Object = Nothing, Optional ByRef r_vMultiUser As Object = Nothing, Optional ByRef r_vBankAccountId As Object = Nothing, Optional ByRef r_vDepositBankAccountId As Object = Nothing, Optional ByRef r_vSuspenseAccountId As Object = Nothing, Optional ByRef r_vCollectionAccountId As Object = Nothing, Optional ByRef r_vAdjustmentAccountId As Object = Nothing, Optional ByRef r_vMediaTypeId As Object = Nothing, Optional ByRef r_vCashFloat As Object = Nothing, Optional ByRef r_vCashFloatAmount As Object = Nothing, Optional ByRef r_vGenerateTask As Object = Nothing, Optional ByRef r_vPMUserGroupId As Object = Nothing, Optional ByRef r_vTaskStatus As Object = Nothing, Optional ByRef r_vTaskIsUrgent As Object = Nothing, Optional ByRef r_vTaskDescription As Object = Nothing, Optional ByRef r_vTaskDueDays As Object = Nothing, Optional ByRef r_vFutureChequeDays As Object = Nothing, Optional ByRef r_vCashlistItemReceiptTypeId As Object = Nothing, Optional ByRef r_vMerchantNumber As Object = Nothing, Optional ByRef r_vAllowReversals As Object = Nothing, Optional ByRef r_vAutoClose As Object = Nothing, Optional ByRef r_vClosed As Object = Nothing, Optional ByRef r_vSubBranchID As Object = Nothing) As Integer` | Gets all details for a single drawer by ID |
| `GetAllCashListDrawerSecurity` | `Public Function GetAllCashListDrawerSecurity(ByVal v_lCashlistDrawerId As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Gets all security (user group) records for a drawer |
| `GetAllCashListDrawerBanking` | `Public Function GetAllCashListDrawerBanking(ByVal v_lCashlistDrawerId As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Gets all banking authorisation records for a drawer |
| `DirectAdd` | `Public Function DirectAdd(Optional ByRef r_lCashlistDrawerId As Integer = 0, Optional ByVal v_vCompanyId As Object = Nothing, Optional ByVal v_vCode As Object = Nothing, Optional ByVal v_vDescription As Object = Nothing, ... Optional ByVal v_vSubBranchId As Object = Nothing) As Integer` | Adds a new cash list drawer |
| `DirectEdit` | `Public Function DirectEdit(ByVal v_lCashlistDrawerId As Integer, ByVal v_vCompanyId As Object, ByVal v_vCode As Object, ByVal v_vDescription As Object, ... ByVal v_vSubBranchId As Object) As Integer` | Updates an existing drawer |
| `DirectDelete` | `Public Function DirectDelete(ByVal v_lCashlistDrawerId As Integer, ByRef r_lDrawerUsed As Integer) As Integer` | Deletes a drawer (returns whether it's in use) |
| `GetSubBranches` | `Public Function GetSubBranches(ByVal v_lSourceID As Integer, ByRef r_vSubBranchArray(,) As Object) As Integer` | Gets sub-branches for a source |
| `AddCashListDrawerSecurity` | `Public Function AddCashListDrawerSecurity(ByVal v_lCashlistDrawerId As Integer, ByVal v_lUserGroupId As Integer, ByVal v_lCompanyId As Integer) As Integer` | Adds a security (user group/company) record |
| `AddCashListDrawerBanking` | `Public Function AddCashListDrawerBanking(ByVal v_lCashlistDrawerId As Integer, ByVal v_lUserId As Integer) As Integer` | Adds a banking authorisation record for a user |
| `UpdateCashListDrawerSecurity` | `Public Function UpdateCashListDrawerSecurity(ByVal v_lCashlistDrawerId As Integer, ByVal v_lUserGroupId As Integer, ByVal v_lCompanyId As Integer, ByVal v_lNewUserGroupId As Integer, ByVal v_lNewCompanyId As Integer) As Integer` | Updates a security record |
| `UpdateCashListDrawerBanking` | `Public Function UpdateCashListDrawerBanking(ByVal v_lCashlistDrawerId As Integer, ByVal v_lUserId As Integer, ByVal v_lNewUserId As Integer) As Integer` | Updates a banking authorisation record |
| `DeleteCashListDrawerSecurity` | `Public Function DeleteCashListDrawerSecurity(ByVal v_lCashlistDrawerId As Integer, ByVal v_lUserGroupId As Integer, ByVal v_lCompanyId As Integer) As Integer` | Deletes a security record |
| `DeleteCashListDrawerBanking` | `Public Function DeleteCashListDrawerBanking(ByVal v_lCashlistDrawerId As Integer, ByVal v_lUserId As Object) As Integer` | Deletes a banking authorisation record |
| `PickListLoad` | `Public Function PickListLoad(ByRef sPickListType As String, ByRef vFKArray As Object, ByRef vResultArray As Object) As Integer` | Loads pick list values |
| `PickListSave` | `Public Function PickListSave(ByRef sPickListType As String, ByRef vFKArray(,) As Object, ByRef vKeys() As Object) As Integer` | Saves pick list selections |
| `GetCashDrawerMediaTypes` | `Public Function GetCashDrawerMediaTypes(ByVal v_lCashDrawerID As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Gets media type codes for a drawer |
| `GetCashDrawerReceiptTypes` | `Public Function GetCashDrawerReceiptTypes(ByVal v_lCashDrawerID As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Gets receipt type codes for a drawer |
| `GetUsersForUserGroupsAndBranches` | `Public Function GetUsersForUserGroupsAndBranches(ByVal v_lCashDrawerID As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Gets users in user groups and branches for a drawer |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_ACT_Add_CashListDrawer` | `DirectAdd` | Add a cash list drawer |
| `spu_ACT_Update_CashListDrawer` | `DirectEdit` | Update a cash list drawer |
| `spu_ACT_Delete_CashListDrawer` | `DirectDelete` | Delete a cash list drawer |
| `spu_ACT_SelAll_CashListDrawer` | `GetAllCashListDrawer` | Select all cash list drawers |
| `spu_ACT_Select_CashListDrawer` | `GetDetails` | Select a single cash list drawer |
| `spu_ACT_SelAll_CashListDrawer_Security` | `GetAllCashListDrawerSecurity` | Select security records for a drawer |
| `spu_ACT_Add_CashListDrawerSecurity` | `AddCashListDrawerSecurity` | Add a security record |
| `spu_ACT_Update_CashListDrawerSecurity` | `UpdateCashListDrawerSecurity` | Update a security record |
| `spu_ACT_Delete_CashListDrawerSecurity` | `DeleteCashListDrawerSecurity` | Delete a security record |
| `spu_ACT_SelAll_CashListDrawer_Banking` | `GetAllCashListDrawerBanking` | Select banking records for a drawer |
| `spu_ACT_Add_CashListDrawerBanking` | `AddCashListDrawerBanking` | Add a banking authorisation record |
| `spu_ACT_Update_CashListDrawerBanking` | `UpdateCashListDrawerBanking` | Update a banking record |
| `spu_ACT_Delete_CashListDrawerBanking` | `DeleteCashListDrawerBanking` | Delete a banking record |
| `spu_ACT_Sel_CashDrawer_Media_Codes` | `GetCashDrawerMediaTypes` | Get media type codes for a drawer |
| `spu_ACT_Sel_CashDrawer_Receipt_Codes` | `GetCashDrawerReceiptTypes` | Get receipt type codes for a drawer |
| `spu_ACT_Sel_UsersForUserGroupsAndBranches` | `GetUsersForUserGroupsAndBranches` | Get users for user groups and branches |
| `spu_ACT_CashListDrawer_PLD` | `PickListSave` | Deletes existing picklist security records for cash list drawer before reload |
| `spu_ACT_CashListDrawer_PLL` | `PickListLoad` | Loads picklist items for the cash list drawer (parameter-driven dynamic SQL) |
| `spu_ACT_CashListDrawer_PLS` | `PickListSave` | Saves new picklist items to the cash list drawer |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `BPMLOOKUP.Business` | `PickListLoad`, `PickListSave` | Lookup/pick list operations |

---

### 18. bACTCashListItem
**Directory:** `CashListItem/`
**Project:** `bACTCashListItem`
**Purpose:** Cash list item (line item) management — individual receipt and payment line items on cash lists. Supports CRUD, instalment processing, media reference maintenance, cheque stop/cancel, receipt reversals, document/letter generation, work task management, step-based payment authorisation, batch record management, and claim payment linking.

**Business Methods — Form (bACTCashlistitemForm.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises component |
| `SetProcessModes` | `Public Function SetProcessModes(...)` | Sets process modes |
| `GetLookupValues` | `Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray As Object, ByRef iLanguageID As Integer, ByRef vResultArray As Object) As Integer` | Gets lookup values |
| `GetMandatory` | `Public Function GetMandatory(Optional ByRef lContactNameMandy As Integer = 0, ... Optional ByRef lXML_ObjectMandy As Integer = 0) As Integer` | Gets mandatory field flags for cash list item fields |
| `DirectAdd` | `Public Function DirectAdd(ByRef r_vCashListItem() As Object) As Integer` | Adds a cash list item directly (array-based) |
| `DirectDelete` | `Public Function DirectDelete(ByVal v_vCashListItem() As Object) As Integer` | Deletes a cash list item directly |
| `CheckID` | `Public Function CheckID(ByRef vID As Object) As Integer` | Checks if a cash list item ID exists |
| `GetCaptions` | `Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object) As Integer` | Gets caption fields (overload 1) |
| `GetCaptions` | `Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, ByRef vTable As Object) As Integer` | Gets caption fields (overload 2 with table) |
| `GetDetails` | `Public Function GetDetails(Optional ByRef vCashlistitemID As Object = Nothing, Optional ByRef vCashListID As Object = Nothing, Optional ByRef vLockMode As Integer = 0) As Integer` | Loads cash list items into collection |
| `GetNext` | `Public Function GetNext(ByRef r_vCashListItem() As Object) As Integer` | Gets next item from collection (array-based) |
| `EditAdd` | `Public Function EditAdd(ByRef lRow As Integer, ByRef r_vCashListItem() As Object) As Integer` | Adds item to collection |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow As Integer, ByVal v_vCashListItem() As Object) As Integer` | Updates item in collection |
| `EditDelete` | `Public Function EditDelete(ByVal lRow As Integer) As Integer` | Deletes item from collection |
| `Cancel` | `Public Function Cancel() As Integer` | Cancels pending changes |
| `Update` | `Public Function Update() As Integer` | Writes changes to database |
| `GetDocumentIdOnInsuranceCnt` | `Public Function GetDocumentIdOnInsuranceCnt(ByRef lInsuranceCnt As Integer, ByRef ldocumentId As Integer) As Integer` | Gets document ID from insurance CNT |
| `DeleteUserProperty` | `Public Function DeleteUserProperty(ByVal v_sPropertyName As String, ByVal v_bDeleteAll As Boolean) As Integer` | Deletes a user property |
| `UpdateUserProperty` | `Public Function UpdateUserProperty(ByVal v_sPropertyName As String, ByVal v_vPropertyValue As Object) As Integer` | Updates a user property |
| `GetUserProperty` | `Public Function GetUserProperty(ByVal v_sPropertyName As String, ByRef r_vPropertyValue As Object) As Integer` | Gets a user property |
| `GetAllocationStatus` | `Public Function GetAllocationStatus(ByVal v_lAllocationID As Integer, ByVal v_cCashListAmt As Decimal, ByRef r_lAllocationStatus As Integer) As Integer` | Gets allocation status for a single allocation |
| `GetMultiAllocationStatus` | `Public Function GetMultiAllocationStatus(ByVal v_lCashListItemID As Integer, ByVal v_iCashListCurrency As Integer, ByVal v_cCashListAmt As Decimal, ByRef r_lAllocationStatus As Integer, ByRef r_cAllocAmt As Decimal) As Integer` | Gets multi-allocation status |
| `BeginTrans` | `Public Function BeginTrans() As Integer` | Begins a database transaction |
| `GetLetterDetails` | `Public Function GetLetterDetails(ByVal lAccountId As Integer, ByVal lTransdetailId As Integer, ByRef lPartyCnt As Integer, ByRef sShortName As String, ByRef sDocumentRef As String) As Integer` | Gets letter/party details for a receipt |
| `CommitTrans` | `Public Function CommitTrans() As Integer` | Commits a database transaction |
| `RollbackTrans` | `Public Function RollbackTrans() As Integer` | Rolls back a database transaction |
| `GetHiddenOption` | `Public Function GetHiddenOption(ByRef r_sResult As String) As Integer` | Gets a hidden option setting |
| `AddTaskToWorkManager` | `Public Function AddTaskToWorkManager(ByRef r_lPMWrkTaskInstanceCnt As Integer, ByVal v_sCustomer As String, ByVal v_sDescription As String, ByVal v_dtTaskDueDate As Date, Optional ByVal v_lPMWrkTaskID As Integer = 0, Optional ByVal v_sTaskCode As String = "", Optional ByVal v_lPMWrkTaskGroupID As Integer = 0, Optional ByVal v_sTaskGroupCode As String = "", Optional ByVal v_iUserID As Integer = 0, Optional ByVal v_sUserGroupCode As String = "PURCLDGR", Optional ByVal v_vKeyArray As Object = Nothing, Optional ByVal v_iIsUrgent As Integer = 0, Optional ByVal v_iIsVisible As Integer = gPMConstants.PMEReturnCode.PMTrue) As Integer` | Adds a task to the work manager |
| `ProcessWTM` | `Public Function ProcessWTM(ByVal v_lCashListItemID As Integer) As Integer` | Processes work task management for a cash list item |
| `GetCashListType` | `Public Function GetCashListType(ByVal v_lCashListTypeID As Integer, ByRef r_sCashListType As String) As Integer` | Gets cash list type description |
| `ValidateMediaReference` | `Public Function ValidateMediaReference(ByVal v_lCashListID As Integer, ByVal v_lCashListItemID As Integer, ByVal v_lMediaTypeID As Integer, ByVal v_sMediaRef As String, ByRef r_bValid As Boolean, ByRef r_iPeriodMonths As Integer, ByRef r_bValidateUI As Boolean) As Integer` | Validates a media reference (cheque number, etc.) |
| `GetInstalmentDetails` | `Public Function GetInstalmentDetails(ByVal v_lAccountID As Integer, ByRef r_vInstalArray(,) As Object, Optional ByVal v_sThirdPartyOnly As String = "") As Integer` | Gets instalment details for an account |
| `CreateCashlistItemInstalments` | `Public Function CreateCashlistItemInstalments(ByVal v_vInstalmentDetails As Object, ByVal v_lCashListItemID As Integer) As Integer` | Creates instalment records for a cash list item |
| `SelectCashlistItemInstalments` | `Public Function SelectCashlistItemInstalments(ByVal v_lCashListItemID As Integer, ByRef v_vResultArray(,) As Object) As Integer` | Selects instalments for a cash list item |
| `UpdateDBForMediaRefChange` | `Public Function UpdateDBForMediaRefChange(ByVal v_lCashListItemID As Integer, ByVal v_lPMUserID As Integer, ByVal v_sOldMediaRef As String, ByVal v_sNewMediaRef As String, ByVal v_sOurRef As String, ByVal v_sTheirRef As String) As Integer` | Updates database for media reference change |
| `UpdateDBForStopCheque` | `Public Function UpdateDBForStopCheque(ByVal v_lCashListItemID As Integer, ByVal v_sStopReason As String) As Integer` | Updates database for cheque stop request |
| `UpdateDBForStopChequeConfirm` | `Public Function UpdateDBForStopChequeConfirm(ByVal v_lCashListItemID As Integer, ByVal v_dBankConfirmDate As Date) As Integer` | Updates database for cheque stop confirmation |
| `UpdateDBForCancelCheque` | `Public Function UpdateDBForCancelCheque(ByVal v_lTransdetailId As Integer, ByVal v_lCashListItemID As Integer, ByVal v_sCancellationReason As Object) As Integer` | Updates database for cheque cancellation |
| `GetAccountAndUserGroupCode` | `Public Function GetAccountAndUserGroupCode(ByVal v_lAccountID As Integer, ByVal v_lUserGroupID As Integer, ByRef r_sAccountCode As String, ByRef r_sUsergroupCode As String) As Integer` | Gets account code and user group code |
| `GetUserGroupID` | `Public Function GetUserGroupID(ByVal v_lUserId As Integer, ByRef r_lUserGroupID As Integer) As Integer` | Gets user group ID for a user |
| `GetAdditionalFields` | `Public Function GetAdditionalFields(ByVal v_lCashListItemID As Integer, ByRef r_sXML As String) As Integer` | Gets additional XML fields |
| `GetPostedTransaction` | `Public Function GetPostedTransaction(ByVal lTransdetailId As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Gets posted transaction details |
| `GetPaymentStatusIDFromCode` | `Public Function GetPaymentStatusIDFromCode(ByVal v_sCode As String, ByRef r_lID As Integer) As Integer` | Gets payment status ID from code |
| `GetPartyCntFromAccountID` | `Public Function GetPartyCntFromAccountID(ByVal v_lAccountID As Integer, ByRef r_lPartyCnt As Integer) As Integer` | Gets party CNT from account ID |
| `ProcessSalvageAllocation` | `Public Function ProcessSalvageAllocation(ByVal lCashlistitemID As Integer, ByVal lTransdetailId As Integer, ByVal lAccountId As Integer) As Integer` | Processes salvage allocation |
| `GetLetterDetailsForInstalment` | `Public Function GetLetterDetailsForInstalment(ByVal lCashlistitemID As Integer, ByRef lPartyCnt As Integer, ByRef sShortName As String, ByRef sDocumentRef As String, Optional ByRef lInsuranceFileCnt As Integer = 0) As Integer` | Gets letter details for instalment receipt |
| `GetReceiptTypeCode` | `Public Function GetReceiptTypeCode(ByVal lReceiptTypeID As Integer, ByRef sReceiptTypeCode As String) As Integer` | Gets receipt type code |
| `SetLetterPrinted` | `Public Function SetLetterPrinted(ByVal lCashlistitemID As Integer) As Integer` | Sets a cash list item letter as printed |
| `UpdStatusOfReversedInstalment` | `Public Function UpdStatusOfReversedInstalment(ByVal lCashlistitemID As Integer, ByVal sReverseCode As String, ByRef r_sReverseReason As String) As Integer` | Updates status of reversed instalment |
| `GetReceiptTypeCodeAndTransDetailID` | `Public Function GetReceiptTypeCodeAndTransDetailID(ByVal v_lCashListItemID As Integer, ByRef r_vTransDetailID As Integer, ByRef r_sReceiptTypeCode As String, ByRef r_bIsInstalmentBased As Boolean) As Integer` | Gets receipt type code and transaction detail ID for reversal |
| `GetPaymentTypeCodeAndTransDetailID` | `Public Function GetPaymentTypeCodeAndTransDetailID(ByVal v_lCashListItemID As Integer, ByRef r_vTransDetailID As Integer, ByRef r_sPaymentTypeCode As String) As Integer` | Gets payment type code and transaction detail ID |
| `ReverseReceipt` | `Public Function ReverseReceipt(ByVal v_lCashListItemID As Integer, ByRef r_sFailureReason As Object, ByVal v_vCashListDrawerID As Object, ByVal sReverseCode As String) As Integer` | Reverses a receipt |
| `UnLockSalvageParty` | `Public Function UnLockSalvageParty(ByVal v_lPartyId As Integer) As Integer` | Unlocks a salvage party |
| `CreateBatchRecord` | `Public Function CreateBatchRecord(Optional ByRef r_lBatchID As Integer = 0, Optional ByVal v_lBatchStatusID As Integer = 0, Optional ByVal v_lCompanyID As Integer = 0, ... Optional ByVal v_iAutoClose As Integer = 0) As Integer` | Creates a batch record |
| `SelectBatchRecord` | `Public Function SelectBatchRecord(ByVal sBatchRef As String, ByRef r_lBatchID As Integer) As Integer` | Selects a batch record by reference |
| `GetMediaTypeIssuer` | `Public Function GetMediaTypeIssuer(ByVal lMediaTypeID As Integer, ByVal iIsClaimPayment As Integer, ByRef r_vOutputDetails(,) As Object) As Integer` | Gets media type issuer details |
| `GetClaimPaymentAccountsDetails` | `Public Function GetClaimPaymentAccountsDetails(ByVal v_lClaimPaymentId As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets claim payment account details |
| `AddInputParameter` | `Public Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer` | Adds a database input parameter |
| `GetReceiptTypeDetails` | `Public Function GetReceiptTypeDetails(ByVal v_lReceiptTypeId As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets receipt type details |
| `AddCashListItemClaimLink` | `Public Function AddCashListItemClaimLink(ByVal v_lClaim_payment_Id As Integer, ByVal v_lClaim_receipt_id As Integer, ByVal v_lCashListItem_id As Integer) As Integer` | Links a cash list item to a claim |
| `GetBranchBaseCurrency` | `Public Function GetBranchBaseCurrency(ByVal v_lSourceID As Integer, ByRef v_lBaseCurrencyID As Integer) As Integer` | Gets base currency for branch |
| `UpdateCLIPaymentStatus` | `Public Function UpdateCLIPaymentStatus(ByVal v_lCashListItem_id As Integer) As Integer` | Updates payment status to pending |
| `GetCollectionDateOverrideAuthority` | `Public Function GetCollectionDateOverrideAuthority(ByVal v_lUserId As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets collection date override authority for user |
| `GetCashListReceiptTypeFromID` | `Public Function GetCashListReceiptTypeFromID(ByRef lCashListReceiptTypeId As Integer, ByRef r_sCashListReceiptType As String) As Integer` | Gets receipt type name from ID |
| `GetDocumentFromTransdetail` | `Public Function GetDocumentFromTransdetail(ByVal v_lTransdetailId As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets document from transaction detail |
| `GetPolicyDetailsFromClaimPayment` | `Public Function GetPolicyDetailsFromClaimPayment(ByVal v_lClaimPaymentId As Integer, ByRef r_lInsuranceFileCnt As Integer, ByRef r_sDocumentRef As String) As Integer` | Gets policy details from claim payment |
| `GetTransDetailsFromBatch` | `Public Function GetTransDetailsFromBatch(ByVal v_lBatchID As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Gets transaction details from batch |
| `UpdateWriteOffDocumentRef` | `Public Function UpdateWriteOffDocumentRef(ByVal v_lOldDocumentId As Integer, ByVal v_lNewDocumentId As Integer) As Integer` | Updates write-off document reference |
| `UpdateCashListForSplitReceipt` | `Public Function UpdateCashListForSplitReceipt(ByVal v_iCashListId As Integer, ByVal v_bStatus As Boolean) As Integer` | Updates cash list for split receipt |
| `GetSchemeCurrency` | `Public Function GetSchemeCurrency(ByVal lPremiumFinanceCnt As Integer, ByVal lPremiumFinanceVersion As Integer, ...) As Integer` | Gets scheme currency for premium finance |
| `UpdateTransMatchCashListItemID` | `Public Function UpdateTransMatchCashListItemID(ByVal nCashListItemID As Integer, ...) As Integer` | Updates TransMatch with cash list item ID |
| `UpdateCashListBatchID` | `Public Function UpdateCashListBatchID(ByVal nBatchID As Integer, ...) As Integer` | Updates cash list batch ID |
| `GetCashListBatchID` | `Public Function GetCashListBatchID(ByVal nCashListID As Integer, ...) As Integer` | Gets cash list batch ID |
| `CheckInsurerPaymentRoadMap` | `Public Function CheckInsurerPaymentRoadMap(ByVal nCashListItemID As Integer, ...) As Integer` | Checks insurer payment roadmap |
| `GetandUpdateBatchTransDetailID` | `Public Function GetandUpdateBatchTransDetailID(ByVal nBatchID As Integer, ...) As Integer` | Gets and updates batch transaction detail ID |
| `GetAllocationDetailIDs` | `Public Function GetAllocationDetailIDs(ByVal nTransDetailID As Integer, ...) As Integer` | Gets allocation detail IDs |
| `GetReinsurerAndRIPaymentRecoveriesDetail` | `Public Function GetReinsurerAndRIPaymentRecoveriesDetail(ByVal nAccountID As Integer, ...) As Integer` | Gets reinsurer/RI payment recoveries detail |
| `GetTaxbandDetailForPaymentRecoveries` | `Public Function GetTaxbandDetailForPaymentRecoveries(ByRef r_oResultArray(,) As Object) As Integer` | Gets tax band detail for payment recoveries |
| `GetCashListDetails` | `Public Function GetCashListDetails(ByVal nCashListId As Integer, ...) As Integer` | Gets cash list details |
| `GetClaimPaymentDetailsByCashListItem` | `Public Function GetClaimPaymentDetailsByCashListItem(ByVal nCashListItemId As Integer, ...) As Integer` | Gets claim payment details by CLI ID |
| `CheckWriteOffReason` | `Public Function CheckWriteOffReason(ByRef drResultArray As DataRow()) As Integer` | Checks write-off reason |
| `GetPartyPolicies` | `Public Function GetPartyPolicies(ByVal v_lAccountID As Integer, ByRef r_vPolicyArray(,) As Object) As Integer` | Gets policies for a party account |

**Business Methods — bStepAuthorization (bStepAuthorization.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises step authorisation |
| `SetProcessModes` | `Public Function SetProcessModes(...)` | Sets process modes |
| `BeginTrans` | `Public Function BeginTrans() As Integer` | Begins a transaction |
| `CommitTrans` | `Public Function CommitTrans() As Integer` | Commits a transaction |
| `RollbackTrans` | `Public Function RollbackTrans() As Integer` | Rolls back a transaction |
| `getUnderwritingOrAgency` | `Public Function getUnderwritingOrAgency() As Integer` | Gets underwriting or agency setting |
| `CheckUserGroup` | `Public Function CheckUserGroup(ByRef r_bUserInGroup As Boolean) As Integer` | Checks if user is in the required group |
| `ProcessApproval` | `Public Function ProcessApproval() As Integer` | Processes payment approval for current step |
| `ProcessDecline` | `Public Function ProcessDecline() As Integer` | Processes payment decline |
| `GetStepGroupCode` | `Public Function GetStepGroupCode(ByRef r_sGroupCode As String) As Integer` | Gets step group code (overload 1) |
| `GetStepGroupCode` | `Public Function GetStepGroupCode(ByRef r_sGroupCode As String, ByRef r_sErrorMessage As String, Optional ByVal IsViaBulkClaimPayment As Boolean = False) As Integer` | Gets step group code with error message (overload 2) |
| `GetStepDetails` | `Public Function GetStepDetails(ByVal v_lApprovalStep As Integer, ByRef r_vStepDetails(,) As Object, Optional ByVal IsViaBulkClaimPayment As Boolean = False) As Integer` | Gets details for an approval step |
| `CheckPaymentStepStatus` | `Public Function CheckPaymentStepStatus(ByRef nApproved As Integer) As Integer` | Checks the current status of payment step approval |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_ACT_Select_CashListItem` | `GetDetails` (by ID) | Select a cash list item |
| `spu_ACT_SelAll_CashListItem` | `GetDetails` (all by cash list) | Select all items for a cash list |
| `spu_ACT_Check_CashListItem` | `CheckID` | Check if item ID exists |
| `spu_ACT_Add_CashListItem` | `DirectAdd`, `Update` | Add a cash list item |
| `spu_ACT_Delete_CashListItem` | `DirectDelete`, `Update` | Delete a cash list item |
| `spu_ACT_Update_CashListItem` | `Update` | Update a cash list item |
| `spu_ACT_Do_CashListItem_Validate` | `Update` (validation) | Validate a cash list item |
| `spu_ACT_Select_Instalments_For_Account` | `GetInstalmentDetails` | Get instalments for an account |
| `spu_ACT_Select_Instalments_For_CashListItem` | `SelectCashlistItemInstalments` | Get instalments for a CLI |
| `spu_ACT_Add_CashListItem_Instalment` | `CreateCashlistItemInstalments` | Add a CLI instalment |
| `spu_ACT_delete_cashlistitem_instalments` | `CreateCashlistItemInstalments` (cleanup) | Delete CLI instalments |
| `spu_ACT_Update_Media_Ref` | `UpdateDBForMediaRefChange` | Update media reference |
| `spu_ACT_update_stop_cheque` | `UpdateDBForStopCheque` | Update for stop cheque |
| `spu_ACT_update_stop_cheque_confirm` | `UpdateDBForStopChequeConfirm` | Update stop cheque confirmation |
| `spu_ACT_Update_Cancel_CashListItem` | `UpdateDBForCancelCheque` | Cancel a cash list item/cheque |
| `spu_ACT_Get_AccountAndUserGroupCode` | `GetAccountAndUserGroupCode` | Get account and user group codes |
| `spu_ACT_Get_Additional_Fields` | `GetAdditionalFields` | Get additional XML fields |
| `spu_ACT_Get_Payment_Status_ID` | `GetPaymentStatusIDFromCode` | Get payment status ID from code |
| `spu_ACT_GetLetterDetailsForInstalment` | `GetLetterDetailsForInstalment` | Get letter details for instalment |
| `spu_ACT_Get_CashListItem_ReceiptType` | `GetReceiptTypeCode` | Get receipt type code |
| `spu_ACT_CashListItem_SetPrinted` | `SetLetterPrinted` | Mark letter as printed |
| `spu_ACT_Update_StatusOfReversedInstalment` | `UpdStatusOfReversedInstalment` | Update reversed instalment status |
| `spu_ACT_GetInstalmentTransID_FromCLIID` | `ReverseReceipt` | Get instalment trans IDs |
| `spu_ACT_Get_Receipt_Reversal_Details` | `GetReceiptTypeCodeAndTransDetailID` | Get receipt reversal details |
| `spu_ACT_Get_Payment_Reversal_Details` | `GetPaymentTypeCodeAndTransDetailID` | Get payment reversal details |
| `spu_ACT_Add_Batch` | `CreateBatchRecord` | Create a batch record |
| `spu_ACT_Select_Batch_FromBatchRef` | `SelectBatchRecord` | Select batch by reference |
| `spu_pmuser_is_name_member` | `bStepAuthorization.CheckUserGroup` | Check user group membership |
| `spu_Get_Debtor_User_Groups` | `bStepAuthorization` | Get debtor user groups |
| `spu_Approval_Records_Sel` | `bStepAuthorization` | Get approval records |
| `spu_Get_Approval_Step_Details` | `bStepAuthorization.GetStepDetails` | Get approval step details |
| `spu_Check_Is_User_Unique` | `bStepAuthorization` | Check user uniqueness in approval |
| `spu_Get_User_Authority_Limit` | `bStepAuthorization` | Get user authority limits |
| `spu_Payment_Approval_Add` | `bStepAuthorization.ProcessApproval` | Add payment approval record |
| `spu_get_pmwrk_task_instance_cnt` | `ProcessWTM` | Get work task instance CNT |
| `spu_ACT_Select_MediaType_Issuer` | `GetMediaTypeIssuer` | Get media type issuer |
| `spu_CLM_Get_Claim_Payment_Accounts_Details` | `GetClaimPaymentAccountsDetails` | Get claim payment account details |
| `spu_ACT_Select_CashListItem_Receipt_Type_Details` | `GetReceiptTypeDetails` | Get receipt type details |
| `spu_CashListItem_claim_link_add` | `AddCashListItemClaimLink` | Add claim link |
| `spu_ACT_Get_Source_Base_Currency` | `GetBranchBaseCurrency` | Get source base currency |
| `spu_upd_CLI_PaymentStatus_Pending` | `UpdateCLIPaymentStatus` | Update CLI payment status to pending |
| `spu_Get_CollectionDate_Override_Authority` | `GetCollectionDateOverrideAuthority` | Get collection date override authority |
| `spu_SAM_Get_And_Validate_Field` | `GetCashListReceiptTypeFromID` | Get/validate field value |
| `spu_ACT_Get_Document_From_Transdetail` | `GetDocumentFromTransdetail` | Get document from transaction detail |
| `spu_CLM_Get_Policy_Details_From_Claim_Payment` | `GetPolicyDetailsFromClaimPayment` | Get policy details from claim payment |
| `spu_ACT_Update_TransMatch_CashListID` | `UpdateTransMatchCashListItemID` | Update TransMatch with CLI ID |
| `spu_ACT_Update_CashList_BatchId` | `UpdateCashListBatchID` | Update cash list batch ID |
| `spu_ACT_Update_Batch_Transdetail_ID` | `GetandUpdateBatchTransDetailID` | Update batch transaction detail ID |
| `spu_ACT_GetAllocationDetailIDs` | `GetAllocationDetailIDs` | Get allocation detail IDs |
| `spu_get_details_for_taxes_over_paymentrecoveries` | `GetReinsurerAndRIPaymentRecoveriesDetail` | Get details for taxes over payment recoveries |
| `spu_get_tax_band_detail_for_payment_recoveries` | `GetTaxbandDetailForPaymentRecoveries` | Get tax band detail for payment recoveries |
| `spu_ACT_Get_ClaimPaymentDetails_By_CashListItemId` | `GetClaimPaymentDetailsByCashListItem` | Get claim payment details by CLI ID |
| `spu_ACT_SelAll_Write_Off_Reason` | `CheckWriteOffReason` | Get all write-off reasons |
| `spu_ACT_Get_Policies_For_Account` | `GetPartyPolicies` | Get policies for a party account |
| `spu_ACT_Update_WritOff_Document` | `UpdateWriteOffDocumentRef` | Update write-off document |
| `spu_ACT_Update_CashList_For_SplitReceipt` | `UpdateCashListForSplitReceipt` | Update cash list for split receipt |
| `spu_Get_PMNav_Batch_Transaction_Details` | `GetTransDetailsFromBatch` | Get batch transaction details |
| `spu_get_document_id_from_insurance_file` | `GetDocumentIdOnInsuranceCnt` | Get document ID from insurance file |
| `spu_ACT_GetDocumentIDsByBatch` | `GetDocumentIDsByBatch` | Retrieves document IDs for a batch (defined constant, legacy/unused) |
| `spu_ACT_Select_CashList` | `GetCashListDetails` | Fetches cash list header details by cash list ID |
| `spu_ACT_Select_CashListItem_Posted_Transaction` | `GetPostedTransaction` | Retrieves posted transaction details for a transdetail record |
| `spu_PFScheme_GetCurrency` | `GetSchemeCurrency` | Retrieves currency and exchange rate information for a premium finance scheme |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `BPMLOOKUP.Business` | `GetLookupValues` | Lookup value retrieval |
| `bACTCashlistitem.Cashlistitems` | Collection management | Internal collection for cash list items |

---

### 19. bACTCashListPost
**Directory:** `CashListPost/`
**Project:** `bACTCashListPost`
**Purpose:** Posts cash list items to the accounting ledger — handles unallocated cash posting, allocated cash posting, document posting, match posting, commission transfer, cheque production, currency conversion, and premium finance instalment payments.

**Business Methods — Automated (bACTCashListPostCls.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises the component and all sub-components |
| `GetSummary` | `Public Function GetSummary(ByRef vSummaryArray As Object) As Integer` | Returns summary information to Navigator |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process modes |
| `GetMatchedTransDetailIDsForCashListItem` | `Public Function GetMatchedTransDetailIDsForCashListItem(ByVal v_lCashListItemID As Integer, ByVal v_lAllocationId As Integer, ByRef v_vMatchedTransDetailIds(,) As Object) As Integer` | Gets matched transaction detail IDs for a cash list item |
| `PostUnallocatedCash` | `Public Function PostUnallocatedCash(ByVal v_vCashListID As Object) As Integer` | Posts unallocated cash (simplest overload) |
| `PostUnallocatedCash` | `Public Function PostUnallocatedCash(ByVal v_vCashListID As Object, ByRef r_cBaseAmount As Decimal, ByVal v_vCashListItemID As Object) As Integer` | Posts unallocated cash with base amount return |
| `PostUnallocatedCash` | `Public Function PostUnallocatedCash(ByVal v_vCashListID As Object, ByRef r_cBaseAmount As Decimal, ByVal v_vCashListItemID As Object, ByVal v_dTransactionDate As Date) As Integer` | Posts unallocated cash with transaction date |
| `PostUnallocatedCash` | `Public Function PostUnallocatedCash(ByVal v_vCashListID As Object, ByRef sFailureReason As String) As Integer` | Posts unallocated cash with failure reason |
| `PostUnallocatedCash` | `Public Function PostUnallocatedCash(ByVal v_vCashListID As Object, ByRef sFailureReason As String, ByVal sInsurence_Ref As String, ByVal v_dTransactionDate As Date) As Integer` | Posts with insurance ref and transaction date |
| `PostUnallocatedCash` | `Public Function PostUnallocatedCash(ByVal v_vCashListID As Object, ByRef sFailureReason As String, ByVal sInsurence_Ref As String) As Integer` | Posts with insurance ref |
| `PostUnallocatedCash` | `Public Function PostUnallocatedCash(ByVal v_vCashListID As Object, ByRef sFailureReason As String, ByVal sInsurence_Ref As String, ByVal dtTransactionDate As Date, ByVal bThirdPartyOnly As Boolean, ByVal sPlanRef As String, ByVal dInsAmount As Decimal, ByRef dOutstandingAmount As Decimal, ByRef nPremiumFinanceVersion As Integer) As Integer` | Posts with premium finance plan details |
| `ValidateDocRef` | `Public Function ValidateDocRef(ByRef sDocRef As String, ByRef iCompanyId As Integer) As Integer` | Validates document reference for duplicates |
| `GetHiddenOption` | `Public Function GetHiddenOption(ByRef r_sResult As String) As Integer` | Gets hidden system option |
| `PostAllocatedCashListItem` | `Public Function PostAllocatedCashListItem(ByVal lCashListID As Integer, ByVal lCashListItemId As Integer, ByVal lInsuranceFileCnt As Integer, ByVal sDocumentRef As String, ByRef lWriteOffReasonID As Integer, ByRef cWriteOffAmount As Decimal, ByRef bCurrencyWriteOff As Boolean, ByRef r_iAllocationStatus As Integer) As Integer` | Posts an allocated cash list item |
| `PostAllocatedCashListItem` | `Public Function PostAllocatedCashListItem(ByVal lCashListID As Integer, ByVal lCashListItemId As Integer, ByVal lInsuranceFileCnt As Integer, ByVal sDocumentRef As String, ByRef lWriteOffReasonID As Integer, ByRef cWriteOffAmount As Decimal, ByRef bCurrencyWriteOff As Boolean, ByRef r_iAllocationStatus As Integer, ByVal bSpecificCashListItemId As Boolean) As Integer` | Posts allocated item with specific item flag |
| `PostAllocatedCashListItem` | `Public Function PostAllocatedCashListItem(ByVal lCashListID As Integer, ByVal lCashListItemId As Integer, ByVal lInsuranceFileCnt As Integer, ByVal sDocumentRef As String, ByRef lWriteOffReasonID As Integer, ByRef cWriteOffAmount As Decimal, ByRef bCurrencyWriteOff As Boolean, ByRef r_iAllocationStatus As Integer, ByVal bIsPosted As Boolean, ByVal cAmtTobePosted As Decimal, ByVal bSpecificCashListItemId As Boolean, ByVal lPostAccountId As Integer) As Integer` | Posts allocated item with amount/account override |
| `PostAllocatedCashListItemSAM` | `Public Function PostAllocatedCashListItemSAM(ByVal lCashListID As Integer, ByVal lCashListItemId As Integer, ByVal lInsuranceFileCnt As Integer, ByVal sDocumentRef As String, ByRef lWriteOffReasonID As Integer, ByRef cWriteOffAmount As Decimal, ByRef bCurrencyWriteOff As Boolean, ByRef r_iAllocationStatus As Integer, ByVal bIsPosted As Boolean, ByVal cAmtTobePosted As Decimal, ByVal bSpecificCashListItemId As Boolean, ByVal lPostAccountId As Integer) As Integer` | SAM variant of posted allocated item |
| `UpdateBGAvailableBalance` | `Public Function UpdateBGAvailableBalance(ByVal lBGID As Integer, ByVal lReceiptAmt As Decimal) As Integer` | Updates bank guarantee available balance |
| `PostAllocatedCash` | `Public Function PostAllocatedCash(ByVal v_vCashListID As Object) As Integer` | Posts all allocated cash for a cash list |
| `PostAllocatedCash` | `Public Function PostAllocatedCash(ByVal v_vCashListID As Object, ByVal v_vCashListItemID As Object) As Integer` | Posts allocated cash for a specific item |
| `PostCashlist` | `Public Function PostCashlist(ByVal v_vCashListID As Object) As Integer` | Posts entire cash list |
| `PostCashlist` | `Public Function PostCashlist(ByVal v_vCashListID As Object, ByVal v_vCashListItemID As Object) As Integer` | Posts a specific cash list item |
| `SetStatus` | `Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer` | Sets posting status |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Stores parameter members from key array |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Returns key array |
| `GetOption` | `Public Function GetOption(ByVal v_iOptionNumber As Integer, ByRef r_sOptionValue As String) As Integer` | Gets system option value |
| `GetOption` | `Public Function GetOption(ByVal v_iOptionNumber As Integer, ByRef r_sOptionValue As String, ByRef vDatabase As Object) As Integer` | Gets system option with explicit database |
| `Start` | `Public Function Start() As Integer` | Main entry point for posting process |
| `GetTransactionsForAllocatedCashListItem` | `Public Function GetTransactionsForAllocatedCashListItem(ByVal lAccountID As Integer, ByVal lInsuranceFileCnt As Integer, ByVal sDocumentRef As String, ByRef r_vResultArray As Object, ByVal v_bUseDocumentRef As Boolean) As Integer` | Gets transactions for allocated item |
| `GetTransactionsForAllocatedCashListItem` | `Public Function GetTransactionsForAllocatedCashListItem(ByVal lAccountID As Integer, ByVal lInsuranceFileCnt As Integer, ByVal sDocumentRef As String, ByRef r_vResultArray As Object, ByVal lTransDetailID As Integer, ByVal lCashListItemId As Integer, ByVal v_bUseDocumentRef As Boolean) As Integer` | Gets transactions with trans detail ID |
| `GetParentNodeIdForTax` | `Public Function GetParentNodeIdForTax(ByRef r_nParentNodeID As Integer) As Integer` | Gets parent node ID for tax account |
| `GetFinancerAccountID` | `Public Function GetFinancerAccountID(ByVal v_sPlanRef As String, ByRef r_nFinancerAccountId As Integer) As Integer` | Gets financer account ID for a plan |
| `GetPolicyTransDetail` | `Public Function GetPolicyTransDetail(ByVal v_sPlanRef As String, ByRef v_nTPTransdetailID As Integer, ByRef v_dbaseAmount As Double, ByRef v_nPremiumFinanceVersion As Integer) As Integer` | Gets policy transaction detail for plan |

**Business Methods — NavigatorV3 (bNavigatorV3.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises NavigatorV3 wrapper |
| `NavigatorV3_SetKeys` | `Public Function NavigatorV3_SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Sets keys via Navigator interface |
| `NavigatorV3_GetKeys` | `Public Function NavigatorV3_GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Gets keys via Navigator interface |
| `NavigatorV3_GetSummary` | `Public Function NavigatorV3_GetSummary(ByRef vSummaryArray As Object) As Integer` | Gets summary via Navigator interface |
| `NavigatorV3_SetProcessModes` | `Public Function NavigatorV3_SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets process modes via Navigator interface |
| `NavigatorV3_Start` | `Public Function NavigatorV3_Start() As Integer` | Starts process via Navigator interface |

**Stored Procedures:**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `spu_ACT_select_TransDetail_OS` | `ACGetOSDetailsSQL` | Select outstanding transaction details |
| `spu_ACT_Get_PFPremiumFinance` | `ACGetPlansSQL` | Get premium finance plans |
| `spe_PFInstalments_sel` | `SelPlanInstalmentsSQL` | Select plan instalments |
| `spu_Pay_PFInstalment` | `PayPlanInstalmentSQL` | Pay a premium finance instalment |
| `spu_ACT_Get_Payment_Status_Code` | `ACGetPaymentStatusCodeSQL` | Get payment status code |
| `spu_ACT_Select_AuditSetType` | `ACGetAuditSetTypeSQL` | Select audit set type |
| `spu_ACT_Do_Approve_AuditSet` | `ACUpdateApproveAuditSetSQL` | Approve audit set |
| `spu_ACT_Select_AllocationTotal_By_Allocation_ID` | `ACGetAllocationTotalSQL` | Select allocation total by allocation ID |
| `spu_SAM_Update_Available_Balance_with_BGKey` | `ACUpdateBGAvailableLimitSQL` | Update bank guarantee available balance |
| `spu_ACT_Get_PostingStatusForCashListItem` | `ACGetPostingStatusForCashListItemSQL` | Get posting status for cash list item |
| `spu_ACT_Get_Cashlist_Tax` | `kGetTaxTransdetailidSQL` | Get tax transaction for cash list |
| `spu_ACT_Get_PostingStatusForCashList` | `kGetPostingStatusForCashListSQL` | Get posting status for entire cash list |
| `spu_act_add_reisurer_payment` | `kAddRIPaymentRecieptSQL` | Add reinsurer payment receipt |
| `spu_Get_AccountId_Form_TaxBandId` | `kGetAccountIdFormTaxBandIDSQL` | Get account ID from tax band ID |
| `spu_Get_Parent_Node_ID_For_Tax` | `kGetParentNodeIdForTaxSQL` | Get parent node ID for tax |
| `spu_Get_AccountIdFromShortCode` | `kDoAccountExistsSQL` | Check if account exists by short code |
| `spu_Get_Financer_Account_ID` | `kGetFinancerAccountIdSQL` | Get premium financer account ID |
| `spu_Get_Policy_Transdetail` | `kGetPolicyTransdetailSQL` | Get policy transaction detail |
| `spu_get_plantransaction_outstanding_amount` | `ACGetPlanOutstandingAmountSQL` | Get plan transaction outstanding amount |
| `spu_ACT_Get_Preferred_Document_Date` | `GetPreferredDocumentDate` | Gets the preferred document date for a cash list item |
| `spu_ACT_Select_DebtForAllocatedCashListItem` | `GetTransactionsForAllocatedCashListItem` | Retrieves debt/credit transactions for cash list item allocation processing |
| `spu_ACT_Update_PolicyCashListItem` | `PostAllocatedCashListItem` | Links insurance file to allocated receipts/payments and cleans up unposted items |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTDocumentPost.Form` | `Automated` | Document posting operations |
| `bACTCashlistitem.Form` | `Automated` | Cash list item management (created via component services) |
| `bACTCashList.Form` | `Automated` | Cash list management |
| `bACTCashListDrawer.Business` | `Automated` | Cash list drawer operations |
| `bACTCurrencyConvert.Form` | `Automated` | Currency conversion |
| `bACTMatchPost.Form` | `Automated` | Match posting |
| `bACTAutoNumber.Business` | `Automated` | Auto-numbering (document refs) |
| `bACTChequeProduction` | `Automated` | Cheque production (via late binding) |
| `bACTAllocationDetail.Form` | `Automated` | Allocation detail management |
| `bACTTransDetail.Form` | `Automated` | Transaction detail management |
| `bACTAllocationManual.Business` | `Automated` | Manual allocation |
| `bACTCreditCard.Business` | `Automated` | Credit card payment processing |
| `bACTBankAccount.Form` | `Automated` | Bank account operations |
| `bPMLookup.Business` | `Automated` | Lookup operations |
| `bSIROptions.Business` | `Automated` | System options (commission option #16) |

---

### 20. bACTChequeProduction
**Directory:** `ChequeProduction/`
**Project:** `bACTChequeProduction`
**Purpose:** Manages cheque production for payment transactions — creates cheque records, assigns cheque numbers, validates duplicates, prints cheques via document templates, exports cheque data, and manages bank-specific cheque sequences.

**Business Methods — Business (bACTChequeProductionCls.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises the component |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process modes |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Stores parameter members from key array |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Returns key array |
| `Start` | `Public Function Start() As Integer` | Main entry point for cheque production |
| `AddCheque` | `Public Function AddCheque(ByRef lTransdetailId As Integer, ByRef lBankID As Integer) As Integer` | Adds a cheque record for a transaction |
| `UpdateCheque` | `Public Function UpdateCheque(ByRef lChequeId As Integer, ByRef sChequeNumber As String) As Integer` | Updates cheque number on a cheque |
| `GetCheques` | `Public Function GetCheques(ByRef r_vResultArray(,) As Object, ByVal r_dtTransactionDate As Date, Optional ByVal v_vBankAccountID As Object = Nothing, Optional ByVal v_iSourceID As Integer = 0) As Integer` | Retrieves cheque records for a date |
| `UpdateCashListItem` | `Public Function UpdateCashListItem(ByRef lTransdetailId As Integer, ByRef sChequeNumber As String) As Integer` | Updates cash list item with cheque number |
| `GetUserPrinter` | `Public Function GetUserPrinter() As String` | Gets the user's default printer |
| `ChequeMasterCheques` | `Public Function ChequeMasterCheques(ByVal v_sSourceDescription As String, ByRef r_vResultArray(,) As Object) As Integer` | Master cheque production with source filter |
| `ClearCheques` | `Public Function ClearCheques(ByRef r_vResultArray(,) As Object) As Integer` | Clears/deletes cheque records |
| `FormatCurrency` | `Public Function FormatCurrency(ByRef vCurrencyID As Object, ByRef vCurrencyAmount As Object, ByRef vFormattedCurrency As Object, ByRef vConversionDate As Object) As Integer` | Formats currency amount for display |
| `PrintCheques` | `Public Function PrintCheques(ByRef r_vResultArray(,) As Object, ByVal lDocumentTemplateID As Integer, ByVal lSpoolMode As Integer) As Integer` | Prints cheques using document template |
| `ExportCheques` | `Public Function ExportCheques(ByRef r_vResultArray(,) As Object, ByVal sExportPath As String, ByRef r_sExportFile As String) As Integer` | Exports cheques to file |
| `CheckDuplicateCheque` | `Public Function CheckDuplicateCheque(ByVal vBankAccoutID As Object, ByVal sChequeNumber As String, ByRef r_vDuplicateChequeFound(,) As Object) As Integer` | Checks for duplicate cheque numbers |
| `UpdateChequePrinted` | `Public Function UpdateChequePrinted(ByRef lChequeId As Integer, ByRef dtPrintedDate As Date, ByRef lPrintedByUserID As Integer) As Integer` | Marks cheque as printed |
| `GetBankStartChequeNumber` | `Public Function GetBankStartChequeNumber(ByVal v_lBankID As Integer, ByRef r_sStartChequeNumber As String) As Integer` | Gets starting cheque number for a bank |
| `GetBankHighestIssuedChequeNumber` | `Public Function GetBankHighestIssuedChequeNumber(ByVal v_lBankID As Integer, ByRef r_sHighestIssuedChequeNumber As String) As Integer` | Gets highest issued cheque number for a bank |
| `CanOverrideChequeNumber` | `Public Function CanOverrideChequeNumber(ByVal v_lUserId As Integer, ByRef r_bCanOverrideChequeNumber As Boolean) As Integer` | Checks if user has authority to override cheque numbers |
| `IsOutOfSequenceCheques` | `Public Function IsOutOfSequenceCheques(ByRef r_bIsOutofSequenceCheques As Boolean, ByVal v_sStartChequeNumber As String, ByVal v_vChequeData(,) As Object, ByVal v_lBankID As Integer) As Integer` | Checks if cheques are out of sequence |
| `GenerateDefaultChequeArrayForPrinting` | `Public Function GenerateDefaultChequeArrayForPrinting(ByVal v_vChequeData(,) As Object, ByRef r_vChequeArrayForPrinting(,) As Object) As Integer` | Generates default cheque array for printing |
| `GenerateChequeNumbersForBank` | `Public Function GenerateChequeNumbersForBank(ByVal v_lBankID As Integer, ByRef v_vChequeArrayForPrinting(,) As Object, ByVal v_lStartChequeNumber As Double) As Integer` | Generates sequential cheque numbers for a bank |

**Stored Procedures:**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `spu_ACT_select_Cheque` | `ACSelectAllSQL` | Select all cheques |
| `spu_ACT_select_Bank_Cheque` | `ACSelectBankSQL` | Select cheques by bank |
| `spu_ACT_add_Cheque` | `ACAddSQL` | Add a cheque record |
| `spu_ACT_delete_Cheque` | `ACDeleteSQL` | Delete a cheque record |
| `spu_ACT_update_Cheque` | `ACUpdateSQL` | Update a cheque record |
| `spu_ACT_update_CashlistItem_Cheque` | `ACUpdateCashListSQL` | Update cash list item with cheque info |
| `spu_ACT_Check_Duplicate_Cheque` | `ACCheckDuplicateChequeSQL` | Check for duplicate cheque number |
| `spu_ACT_Update_Cheque_Printed` | `ACUpdateChequePrintedSQL` | Update cheque printed status |
| `spu_get_bank_start_chequenumber` | `ACGetBankStartChequeNumberSQL` | Get bank starting cheque number |
| `spu_get_bank_highest_issued_chequenumber` | `ACGetBankHighestIssuedChequeNumberSQL` | Get bank highest issued cheque number |
| `spe_User_Authorities_Sel` | `ACCanOverrideChequeNumberSQL` | Select user authorities for cheque override |
| `spu_get_bank_cheque_sequence` | `ACSelBankChequeSequenceSQL` | Get bank cheque sequence info |
| `spu_Get_User_Printer` | `ACGetUserPrinterSQL` | Get user printer from PMUser |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTCurrencyConvert.Form` | `Business` | Currency formatting and conversion |

---

### 21. bACTCommissionPayments
**Directory:** `Agent Summary/`
**Project:** `bACTCommissionPayments`
**Purpose:** Manages commission payment processing for agents — prepares agent commission summaries, creates payment batches, marks commissions for payment, generates commission payment transactions, and retrieves agent/document details for payment runs.

**Business Methods — Business (bACTCommissionPaymentsBusiness.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Short, ByRef iSourceID As Short, ByRef iLanguageID As Short, ByRef iCurrencyID As Short, ByRef iLogLevel As Short, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises the component |
| `AddInputParameter` | `Public Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Short) As Integer` | Adds an input parameter to the database parameters collection |
| `PrepareAgentSummary` | `Public Function PrepareAgentSummary(ByVal v_dTransDateFrom As Date, ByVal v_dTransDateTo As Date, ByVal v_iCurrencyID As Short, ByVal v_lProductID As Integer, ByVal v_lCompanyId As Integer, ByVal v_lUserId As Integer, ByVal v_iOnlyAuthorityLimit As Short, ByVal v_sAgentId As String, ByRef r_sSessionGUID As String, ByRef r_vResultArray(,) As Object) As Integer` | Prepares agent commission summary for date range |
| `PrepareAgentSummaryForAllocatedTrans` | `Public Function PrepareAgentSummaryForAllocatedTrans(ByVal v_dTransDateFrom As Date, ByVal v_dTransDateTo As Date, ByVal v_iCurrencyID As Short, ByVal v_lProductID As Integer, ByVal v_lCompanyId As Integer, ByVal v_lUserId As Integer, ByVal v_iOnlyAuthorityLimit As Short, ByVal v_sAgentId As String, ByRef r_sSessionGUID As String, ByRef r_vResultArray(,) As Object) As Integer` | Prepares agent summary for allocated transactions |
| `MarkCommissionPayments` | `Public Function MarkCommissionPayments(ByVal v_lUserId As Integer, ByVal v_sSession_Guid As String, ByVal v_vSelectedAccounts As Object, ByVal v_dStatementDate As Date, ByRef r_lBatchId As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Creates batch and marks commission payments |
| `GenerateCommissionPayments` | `Public Function GenerateCommissionPayments(ByVal v_lBatchID As Integer, ByVal v_dStatementDate As Date) As Integer` | Generates commission payment transactions for a batch |
| `FormatCurrency_Renamed` | `Public Function FormatCurrency_Renamed(ByVal v_lCurrencyID As Integer, ByVal v_cCurrencyAmount As Decimal, Optional ByVal v_dtConversionDate As Date = #12:00:00 AM#) As String` | Formats currency amount for display |
| `GetDocumentsForAccountBatch` | `Public Function GetDocumentsForAccountBatch(ByVal v_lAccountId As Integer, ByVal v_lBatchID As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Gets documents for an account within a batch |
| `GetAgentDetailsforPayments` | `Public Function GetAgentDetailsforPayments(ByVal v_lAccountId As Integer, ByVal v_lSourceID As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Gets agent details for payments |
| `RemoveCommissionPaymentsBatch` | `Public Function RemoveCommissionPaymentsBatch(ByVal v_lBatchID As Integer) As Integer` | Removes a commission payments batch |
| `GetShortNameForParty` | `Public Function GetShortNameForParty(ByVal v_lPartyCnt As Integer, ByRef r_sPartyShortName As String) As Integer` | Gets party short name |
| `SaveSelection` | `Public Function SaveSelection(ByVal v_vCriteriaFields As Object) As Integer` | Saves selection criteria |

**Stored Procedures:**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `spu_ACT_PrepareAgentSummary` | `ACPrepareAgentSummaryCodeSQL` | Prepare agent commission summary |
| `spu_ACT_PrepareAgentSummaryAllocation` | `ACPrepareAgentSummaryAllocatedTransCodeSQL` | Prepare agent summary for allocated transactions |
| `spu_ACT_CreateCommissionPaymentsBatch` | `ACCreateCommPaymentsBatchSQL` | Create commission payments batch |
| `spu_ACT_MarkCommissionPaymentsInBatch` | `ACMarkCommissionPaymentsInBatchSQL` | Mark commission payments in batch |
| `spu_ACT_GetChosenCommissionPayments` | `ACGetChosenCommissionPaymentsSQL` | Get chosen commission payments |
| `spu_get_Agent_Details_for_Payments` | `ACGetAgentDetailsforPaymentsSQL` | Get agent details for payments |
| `spu_get_Documents_For_Account_Batch` | `ACGetDocumentsForAccountBatchSQL` | Get documents for account/batch |
| `spu_ACT_RemoveCommissionPaymentsBatch` | `ACRemoveCommissionPaymentsBatchSQL` | Remove commission payments batch |
| `spu_Get_Party_Shortname` | `ACGetPartyShortnameSQL` | Get party short name |
| `spu_SAM_get_wrk_task_id` | `ACGetWorkTaskIDSQL` | Get work task ID |
| `spu_Get_TaskGroup_For_WrkTaskID` | `ACGetWorkTaskGroupIDSQL` | Get task group for work task ID |
| `spu_ACT_Get_PartyCnt_From_AccountID` | `GetPartyCntFromAccountID` | Gets party count from account ID for commission posting |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTCurrencyConvert.Form` | `Business` | Currency formatting (via late binding GetBusinessObject) |
| `cGISDataSetControl.Application` | `Business` | Risk data dataset control |

---

### 22. bACTCommissionPost
**Directory:** `CommissionPost/`
**Project:** `bACTCommissionPost`
**Purpose:** Posts commission transactions — earns commission by moving from deferred to earned accounts, posts commission on effective/client/insurer/DID settlement options, handles tax movements, processes part-commission for instalment plans, manages suspended commission transactions, and handles auto batch payment runs.

**Business Methods — Business (bACTCommissionPostBusiness.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises the component |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process modes |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Stores parameter members from key array |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Returns key array |
| `EarnCommission` | `Public Function EarnCommission(ByVal v_lCommissionAccountID As Integer, ByVal v_lCommissionEarnedAccountID As Integer, ByVal v_dCommissionAmount As Double, ByVal v_sInsuranceRef As String, ByVal v_sDocumentRef As String, ByVal v_lReportMapId As Integer, Optional ByVal v_vDocumentDate As Date = #12/30/1899#) As Integer` | Posts commission from deferred to earned account |
| `Start` | `Public Function Start() As Integer` | Main entry point for commission posting |
| `MoveTax` | `Public Function MoveTax(ByVal v_lTaxAccountID As Integer, ByVal v_lTaxDueAccountID As Integer, ByVal v_dTaxAmount As Double, ByVal v_sInsuranceRef As String, ByVal v_sDocumentRef As String, ByVal v_lReportMapId As Integer, Optional ByVal v_vDocumentDate As Date = #12/30/1899#) As Integer` | Moves tax between accounts |
| `PostCommission` | `Public Function PostCommission(ByVal v_sCommissionOption As String, ByVal v_iCompanyID As Integer, ByVal v_lTransactionId As Integer, Optional ByVal v_bExcludeDID As Boolean = False) As Integer` | Posts commission based on settlement option |
| `PostCommissionTax` | `Public Function PostCommissionTax(ByVal v_iCompanyID As Integer, ByVal v_lTransactionId As Integer) As Integer` | Posts commission tax movements |
| `PostPartCommission` | `Public Function PostPartCommission(ByVal v_lCommissionSuspendedTransDetailId As Integer, ByVal v_dPercentage As Double, ByVal v_bLastInstalment As Boolean) As Integer` | Posts part commission for instalment plans |
| `PostEffectiveCommission` | `Public Function PostEffectiveCommission(ByVal v_sDocumentRef As String, ByVal v_lDocumentCompanyId As Integer) As Integer` | Posts effective (when-effective) commission |
| `GetCommissionEarnedAccount` | `Public Function GetCommissionEarnedAccount(ByVal v_lCommissionAccountID As Integer, ByRef v_lReportMapId As Integer, ByRef v_lCommissionEarnedAccountID As Object) As Integer` | Gets the commission earned account from commission account |
| `GetTaxDueAccount` | `Public Function GetTaxDueAccount(ByVal v_lTaxAccountID As Integer, ByRef v_lReportMapId As Integer, ByRef v_lTaxDueAccountID As Object) As Integer` | Gets the tax due account from tax account |
| `GetPartyCntFromAccountID` | `Public Function GetPartyCntFromAccountID(ByVal v_lAccountID As Integer, ByRef r_lPartyCnt As Integer) As Integer` | Gets party count from account ID |
| `PostSuspendedTransaction` | `Public Function PostSuspendedTransaction(ByVal v_lSuspendedTransDetailId As Integer, ByVal v_lSuspendedAccountID As Integer, ByVal v_dPercentage As Single, ByVal v_bIsLastInstalment As Boolean, ByVal v_lInsuranceFileCnt As Integer, ByVal v_sInsuranceRef As String, ByVal v_bPartialPayment As Boolean, ByVal v_lInstalmentID As Integer) As Integer` | Posts a suspended transaction (commission or other) |
| `GetTransAndAllocationAmounts` | `Public Function GetTransAndAllocationAmounts(ByVal v_lSuspendedTransDetailId As Integer, ByRef r_lSuspenseAccountID As Integer, ByRef r_cTransDetailAmount As Decimal, ByRef r_cAllocationDetailAmount As Decimal) As Integer` | Gets transaction and allocation amounts for suspended trans |

**Business Methods — PostPartCommission (PostPartCommission.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Start` | `Friend Function Start(ByRef r_lCommissionSuspendedTransDetailId As Integer, ByRef r_dPercentage As Double, ByRef r_bLastInstalment As Boolean) As Integer` | Entry point for part commission posting use case |

**Stored Procedures:**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `spu_ACT_Sel_Comm_for_effective` | `ACSelectCommissionForEffectiveSQL` | Select commission for effective settlement |
| `spu_ACT_Sel_Comm_for_client` | `ACSelectCommissionForClientSQL` | Select commission for client payment |
| `spu_ACT_Sel_Comm_for_client_did` | `ACSelectCommissionForClientDIDSQL` | Select commission for client payment including DID |
| `spu_ACT_Sel_Comm_Tax` | `ACSelectCommissionTaxSQL` | Select commission tax |
| `spu_ACT_Sel_Comm_for_insurer` | `ACSelectCommissionForInsurerSQL` | Select commission for insurer settlement |
| `spu_ACT_Sel_Comm_for_DID` | `ACSelectCommissionForDIDSQL` | Select commission for DID |
| `spu_ACT_Sel_Comm_Earned` | `ACSelectIsCommissionEarnedSQL` | Check if commission is earned |
| `spu_ACT_Sel_Commission_Moved` | `ACSelectHasCommissionMovedSQL` | Check if commission has been moved |
| `spu_ACT_Sel_Tax_Moved` | `ACSelectHasTaxMovedSQL` | Check if tax has been moved |
| `spu_ACT_Do_IsInsurer` | `ACSelectIsInsurerSQL` | Check if account is insurer |
| `spu_ACT_Sel_Paid_Brokerage_Trans` | `ACSelectBrokeragePaymentsSQL` | Select paid brokerage transactions |
| `spu_ACT_Select_TransDetail_Filter` | `ACSelectFilteredTransDetailsSQL` | Select filtered transaction details |
| `spu_ACT_Update_TransDetail_IsPaid_Only` | `ACUpdateTransDetailsIsPaidSQL` | Update transaction detail IsPaid status |
| `spu_ACT_Select_Account` | `ACGetAccountDetailsSQL` | Select account details |
| `spu_ACT_Get_CashListItem_PaymentType` | `ACGetCashListItemPayTypeSQL` | Get cash list item payment type |
| `spu_ACT_Select_AllocationStatusID` | `ACGetAllocationStatusSQL` | Get allocation status ID |
| `spu_ACT_Select_CashListItem_PaymentStatusID` | `ACGetCashListItemPayStatusSQL` | Get cash list item payment status ID |
| `spu_ACT_Get_Suspended_Trans_Info` | `ACGetSuspenseDetailsSQL` | Get suspended transaction info |
| `spu_ACT_PFSelect_Agent_Account` | `ACPFSelectAgentAccountSQL` | Get commission agent account (PostPartCommission) |
| `spu_ACT_Select_Commission_Amount_Remaining` | `ACSelectCommissionAmountRemainingSQL` | Get remaining commission amount (PostPartCommission) |
| `spu_ACT_Get_PartyCnt_From_AccountID` | `GetPartyCntFromAccountID` | Gets party count from account ID for commission posting |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTExplorer.Form` | `Business` | Account explorer/navigation |
| `bACTAllocationManual.Business` | `Business` | Manual allocation operations |
| `bACTCurrencyConvert.Form` | `Business` | Currency conversion |
| `bACTTransdetail.Form` | `Business` | Transaction detail operations |
| `bACTDocumentPost.Form` | `Business` | Document posting |
| `bACTAutoNumber.Business` | `Business` | Auto-numbering |
| `bACTPeriod.Form` | `Business` | Accounting period operations |
| `bACTTransdetail.Form` | `PostPartCommission` | Transaction detail retrieval for commission |

---

### 23. bACTCompany
**Directory:** `Company/`
**Project:** `bACTCompany`
**Purpose:** CRUD operations for company entities in the Orion accounting system — manages company master data including code, description, address, registration numbers, base currency, VAT, email, broker ABI, and user licence. Contains Form (interactive), Automated (batch), Company (entity), and Companys (collection) classes.

**Business Methods — Form (bACTCompanyForm.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises the Form class |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process modes |
| `GetLookupValues` | `Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer` | Gets lookup values |
| `DirectAdd` | `Public Function DirectAdd(Optional ByRef vCompanyID As Integer = 0, Optional ByRef vBaseCurrency As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, ...) As Integer` | Directly adds a company record |
| `DirectDelete` | `Public Function DirectDelete(Optional ByRef vCompanyID As Object = Nothing, ...) As Integer` | Directly deletes a company record |
| `GetDefaults` | `Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, ...) As Integer` | Gets default values for a new company |
| `CheckID` | `Public Function CheckID(ByRef vID As Object) As Integer` | Checks if a company ID exists |
| `GetCaptions` | `Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, Optional ByRef vTable As Object = Nothing) As Integer` | Gets field captions |
| `GetDetails` | `Public Function GetDetails(Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vLockMode As Integer = 0) As Integer` | Gets company details into collection |
| `GetNext` | `Public Function GetNext(Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vBaseCurrency As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, ...) As Integer` | Gets next company from collection |
| `EditAdd` | `Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vCompanyID As Object = Nothing, ...) As Integer` | Adds a company to the edit collection |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vCompanyID As Object = Nothing, ...) As Integer` | Updates company in the edit collection |
| `EditDelete` | `Public Function EditDelete(ByVal lRow As Integer) As Integer` | Deletes company from the edit collection |
| `Cancel` | `Public Function Cancel() As Integer` | Cancels pending edits |
| `Update` | `Public Function Update() As Integer` | Commits pending edits to database |

**Business Methods — Automated (bACTCompanyAutomated.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises the Automated class |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, ...) As Integer` | Sets process modes |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Sets keys |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Gets keys |
| `Start` | `Public Function Start() As Integer` | Starts automated company processing |

**Business Methods — Company (bACTCompanyCls.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise() As Integer` | Initialises entity class |

**Business Methods — Companys (bACTCompanys.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Add` | `Public Function Add(ByRef oNewCompany As bACTCompany.Company) As Integer` | Adds a Company to collection |
| `Count` | `Public Function Count() As Integer` | Returns count of companies |
| `Delete` | `Public Sub Delete(ByRef vKey As Integer)` | Deletes company from collection |
| `Item` | `Public Function Item(ByRef vKey As Integer) As bACTCompany.Company` | Returns company by key |
| `DeleteAll` | `Public Sub DeleteAll()` | Deletes all companies from collection |
| `Clear` | `Public Sub Clear()` | Clears the collection |
| `Initialise` | `Public Function Initialise() As Integer` | Initialises collection |

**Stored Procedures (FormSQL):**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `spu_ACT_Select_Company` | `ACGetDetailsSQL` | Select a company by ID |
| `spu_ACT_SelAll_Company` | `ACGetAllDetailsSQL` | Select all companies |
| `spu_ACT_Check_Company` | `ACCheckIDSQL` | Check if company ID exists |
| `spu_ACT_Add_Company` | `ACAddSQL` | Add a company |
| `spu_ACT_Delete_Company` | `ACDeleteSQL` | Delete a company |
| `spu_ACT_Update_Company` | `ACUpdateSQL` | Update a company |

**Stored Procedures (AutomatedSQL):**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `spu_ACT_Select_Company` | `ACAutoGetDetailsSQL` | Select a company (automated) |
| `spu_ACT_SelAll_Company` | `ACAutoGetAllDetailsSQL` | Select all companies (automated) |
| `spu_ACT_Check_Company` | `ACAutoCheckIDSQL` | Check company ID (automated) |
| `spu_ACT_Add_Company` | `ACAutoAddSQL` | Add a company (automated) |
| `spu_ACT_Delete_Company` | `ACAutoDeleteSQL` | Delete a company (automated) |
| `spu_ACT_Update_Company` | `ACAutoUpdateSQL` | Update a company (automated) |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bPMLookup.Business` | `Form` | Lookup operations |

---

### 24. bACTCompanyCurrency
**Directory:** `CompanyCurrency/`
**Project:** `bACTCompanyCurrency`
**Purpose:** CRUD operations for company-currency associations — manages which currencies are assigned to which companies, supports querying currencies in/not-in a company, base currencies, and bulk update of company currency assignments. Implements `SSP.S4I.Interfaces.IBusiness`.

**Business Methods — Form (bACTCompanyCurrencyForm.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises the Form class (implements IBusiness) |
| `GetCompanyCurrencies` | `Public Function GetCompanyCurrencies(ByRef lNumberOfRecords As Integer, ByRef vResultArray(,) As Object, ByVal vnMode As Integer) As Integer` | Gets company currencies by mode (in-company, not-in-company, all, base) |
| `GetCompanyCurrency` | `Public Function GetCompanyCurrency(ByRef r_lNumberOfRecords As Integer, ByVal v_iCurrencyID As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Gets a specific company currency |
| `UpdateCompanyCurrencies` | `Public Function UpdateCompanyCurrencies(ByRef vDataToUpdate() As Object) As Integer` | Bulk updates company currency assignments |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process modes |
| `DirectAdd` | `Public Function DirectAdd(Optional ByRef vCompanyCurrencyID As Integer = 0, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing) As Integer` | Directly adds a company-currency link |
| `DirectDelete` | `Public Function DirectDelete(Optional ByRef vCompanyCurrencyID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing) As Integer` | Directly deletes a company-currency link |
| `GetDefaults` | `Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vCompanyCurrencyID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing) As Integer` | Gets default values |
| `CheckID` | `Public Function CheckID(ByRef vID As Object) As Integer` | Checks if company-currency ID exists |
| `GetCaptions` | `Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, Optional ByRef vTable As String = "") As Integer` | Gets field captions |
| `GetDetails` | `Public Function GetDetails(Optional ByRef vCompanyCurrencyID As Object = Nothing, Optional ByRef vLockMode As gPMConstants.PMELockMode = 0) As Integer` | Gets company-currency details into collection |
| `GetNext` | `Public Function GetNext(Optional ByRef vCompanyCurrencyID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing) As Integer` | Gets next record from collection |
| `EditAdd` | `Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vCompanyCurrencyID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing) As Integer` | Adds to edit collection |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vCompanyCurrencyID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing) As Integer` | Updates in edit collection |
| `EditDelete` | `Public Function EditDelete(ByVal lRow As Integer) As Integer` | Deletes from edit collection |
| `Cancel` | `Public Function Cancel() As Integer` | Cancels pending edits |
| `Update` | `Public Function Update() As Integer` | Commits edits to database |

**Business Methods — Automated (bACTCompanyCurrencyAutomated.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises Automated class |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, ...) As Integer` | Sets process modes |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Sets keys |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Gets keys |
| `Start` | `Public Function Start() As Integer` | Starts automated processing |

**Business Methods — CompanyCurrency (bACTCompanyCurrency.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise() As Integer` | Initialises entity class |

**Business Methods — CompanyCurrencys (bACTCompanyCurrencys.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Add` | `Public Function Add(ByRef oNewACTCompanyCurrency As bACTCompanyCurrency.CompanyCurrency) As Integer` | Adds to collection |
| `Count` | `Public Function Count() As Integer` | Returns count |
| `Delete` | `Public Sub Delete(ByRef vKey As Integer)` | Deletes from collection |
| `Item` | `Public Function Item(ByRef vKey As Integer) As bACTCompanyCurrency.CompanyCurrency` | Returns item by key |
| `DeleteAll` | `Public Sub DeleteAll()` | Deletes all from collection |
| `Clear` | `Public Sub Clear()` | Clears collection |
| `Initialise` | `Public Function Initialise() As Integer` | Initialises collection |

**Stored Procedures (FormSQL):**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `spu_ACT_select_CompanyCurrency` | `ACGetDetailsSQL` | Select company-currency by ID |
| `spu_ACT_select_all_CompanyCurrency` | `ACGetAllDetailsSQL` | Select all company-currencies |
| `spu_ACT_check_CompanyCurrency_id` | `ACCheckIDSQL` | Check if company-currency ID exists |
| `spu_ACT_add_CompanyCurrency` | `ACAddSQL` | Add a company-currency record |
| `spu_ACT_delete_CompanyCurrency` | `ACDeleteSQL` | Delete a company-currency record |
| `spu_ACT_update_CompanyCurrency` | `ACUpdateSQL` | Update a company-currency record |
| `spu_ACT_Do_GetCompanyCurrency` | `ACGetCompanyCurrencySQL` | Get company currency details |
| `spu_ACT_Do_CurrencyInCompany` | `ACGetCurrenciesInCompanySQL` | Get currencies assigned to company |
| `spu_ACT_Do_AllCurrencyCompany` | `ACGetAllCurrenciesSQL` | Get all currencies for company |
| `spu_ACT_Do_CurrencyNotInCompany` | `ACGetCurrenciesNotInCompanySQL` | Get currencies not in company |
| `spu_ACT_Do_BaseCurrency` | `ACGetBaseCurrenciesSQL` | Get base currencies |

**Stored Procedures (AutomatedSQL):**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `spu_ACT_select_CompanyCurrency` | `ACAutoGetDetailsSQL` | Select company-currency (automated) |
| `spu_ACT_select_all_CompanyCurrency` | `ACAutoGetAllDetailsSQL` | Select all company-currencies (automated) |
| `spu_ACT_check_CompanyCurrency_id` | `ACAutoCheckIDSQL` | Check ID (automated) |
| `spu_ACT_add_CompanyCurrency` | `ACAutoAddSQL` | Add (automated) |
| `spu_ACT_delete_CompanyCurrency` | `ACAutoDeleteSQL` | Delete (automated) |
| `spu_ACT_update_CompanyCurrency` | `ACAutoUpdateSQL` | Update (automated) |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bPMLookup.Business` | `Form` | Lookup operations |

---

### 25. bACTCreditCard
**Directory:** `Orion/Components/Credit Card/Business/bACTCreditCard/`
**Project:** `bACTCreditCard`
**Purpose:** Credit card payment processing — authorisation, collection, and cancellation of credit card transactions via third-party connectors (Retail Logic SolveSE). Manages media type issuer/connector data and previously used card numbers.

**Business Methods — Business (Business.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard initialisation |
| `GetMediaTypeIssuerAndConnectorData` | `Public Function GetMediaTypeIssuerAndConnectorData(ByVal v_lMediaType_Issuer_ID As Integer, ByRef r_vOutputDetails(,) As Object) As Integer` | Gets media type issuer and connector configuration data |
| `GetPreviouslyUsedCCNumbers` | `Public Function GetPreviouslyUsedCCNumbers(ByVal v_lAccountID As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lMediatypeIssuerID As Integer, ByVal v_bIsClaimTypePayment As Boolean, ByRef r_vOutputDetails(,) As Object) As Integer` | Gets previously used credit card numbers for an account |
| `AuthorisePayment` | `Public Function AuthorisePayment(ByVal sMediaTypeConnector As String, ByVal bIsReceipt As Boolean, ByVal cCCAmount As Decimal, ByVal lCCCurrencyID As Integer, ByVal sCCNumber As String, ByVal sCCName As String, ByVal sCCExpiry As String, ByVal sCCStart As String, ByVal sCCIssue As String, ByVal sCCPin As String, ByVal sCCAddress1 As String, ByVal sCCPostcode As String, ByVal sCCCustomerFlag As String, ByRef r_sCCReturnStatus As String, ByRef r_sCCAutoAuthCode As String, ByRef r_sCCTransactionCode As String, Optional ByRef r_sResultXML As String = "", Optional ByRef bCancel As Boolean = False) As Integer` | Authorises a credit card payment via connector |
| `CollectPayment` | `Public Function CollectPayment(Optional ByVal lCashListItemID As Integer = 0, Optional ByVal sMediaTypeConnector As String = "", Optional ByVal sCCTransactionCode As String = "", Optional ByRef r_sCCReturnStatus As String = "", Optional ByRef r_sResultXML As String = "") As Integer` | Collects/settles an authorised credit card payment |
| `CancelPayment` | `Public Function CancelPayment(ByVal sMediaTypeConnector As String, ByVal sCCTransactionCode As String, Optional ByRef r_sCCReturnStatus As String = "", Optional ByRef r_sResultXML As String = "") As Integer` | Cancels an authorised credit card transaction |
| `GetDefaultCreditCardByAccount` | `Public Function GetDefaultCreditCardByAccount(ByVal v_nAccountID As Integer, ByRef r_vOutputDetails(,) As Object) As Integer` | Gets the default credit card for an account |

**Internal Class — SolveSE (SolveSE.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUserName As String, ..., ByRef vDatabase As dPMDAO.Database) As Integer` | Initialises SolveSE connector with Winsock form |
| `SolveSE` | `Public Function SolveSE(iSolveSEMessageID As MainModule.eSolveSEMessage, sHost As String, sPort As String, iTimeout As Integer, bIsReceipt As Boolean, sSourceID As String, sCCNumber As String, ...) As Integer` | Handles Retail Logic SolveSE payment gateway communication for card authorisation/settlement — commented out in current implementation |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_ACT_Select_MediaTypeIssuerAndConnectorData` | `GetMediaTypeIssuerAndConnectorData` | Get media type issuer and connector configuration |
| `spu_ACT_Select_PreviouslyUsedCCNumbers` | `GetPreviouslyUsedCCNumbers` | Get previously used credit card numbers |
| `spu_GetDefaultCreditcardByAccount` | `GetDefaultCreditCardByAccount` | Get default credit card for an account |
| `spu_ACT_Select_CollectCCPayment_For_CashListItem` | `CollectPayment` | Retrieves credit card payment details for collection processing |
| `spu_ACT_Select_MediaType_Connector_for_code` | `CollectPayment` | Gets media type connector configuration (host, port, timeout) for payment processing |
| `spu_pm_iccs` | `GetICCS` | Retrieves ICCS (payment source ID) value for credit card transactions |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| *(none — self-contained with SolveSE connector)* | | |

---

### 26. bACTCreditControl
**Directory:** `Orion/Components/CreditControl/Business/bACTCreditControl/`
**Project:** `bACTCreditControl`
**Purpose:** Credit control rule and step management — defines rules, steps, and processing logic for chasing overdue premium payments. Handles auto-cancellation, policy paid checks, letter generation for clients/OIPs/brokers, work manager task creation, and batch processing integration.

**Business Methods — Business (bBusiness.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard initialisation (Implements IBusiness) |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set processing mode options |
| `AutoCancel` | `Public Function AutoCancel(ByVal v_lCreditControlItemId As Integer, ByVal v_bCheckRulesOnly As Boolean, ByRef r_bAutoCancelResult As Boolean, Optional ByVal v_bArchiveDoc As Boolean = False, Optional ByVal v_bSpoolDoc As Boolean = False) As Integer` | Run auto-cancel rules for a credit control item |
| `AutoCancelReport` | `Public Function AutoCancelReport(ByVal v_vCreditControlItems As Object) As Integer` | Generate auto-cancel reports |
| `CancelPolicy` | `Public Function CancelPolicy(ByVal lCreditControlItemID As Object, ByVal dLapsedDate As Date) As Integer` | Cancel a policy via credit control |
| `CreateWorkManagerTask` | `Public Function CreateWorkManagerTask(ByVal v_lPMUserGroupID As Integer, ByVal v_lPMWrkTaskID As String, ByVal v_sCustomer As String, ByVal v_sDescription As String, Optional ByVal v_dtTaskDueDate As Date = ..., ...) As Integer` | Create a work manager task |
| `CreateTask` | `Public Function CreateTask(ByVal v_lPMWrkTaskID As Integer, ByVal v_lPMWrkTaskGroupID As Integer, ByVal v_sCustomer As String, ByVal v_dtTaskDueDate As Date, ByVal v_lPMUserGroupID As Integer, ByVal v_sDescription As String, ...) As Integer` | Create a task with full parameters |
| `ReleaseTempSession` | `Public Function ReleaseTempSession(ByVal v_lSessionID As Integer) As Integer` | Release a temporary session ID |
| `ProduceClientLetters` | `Public Function ProduceClientLetters(ByVal v_vCreditControlItems As Object, Optional ByVal v_bSpoolDocuments As Boolean = False, Optional ByVal v_bArchiveDocuments As Boolean = False) As Integer` | Generate and print/spool client letters |
| `ProduceOIPLetters` | `Public Function ProduceOIPLetters(ByVal v_vCreditControlItems As Object, Optional ByVal v_bSpoolDocuments As Boolean = False, Optional ByVal v_bArchiveDocuments As Boolean = False) As Integer` | Generate and print/spool Other Interested Party letters |
| `IsPolicyPaid` | `Public Function IsPolicyPaid(ByVal v_lInsuranceFileCnt As Integer, ByRef r_bIsPaidInFull As Boolean, ByRef r_bIsPartiallyPaid As Boolean, ByRef r_cAmountOwing As Decimal, Optional ByRef r_bHasLiveMTA As Boolean = False) As Integer` | Check if a policy has been paid in full |
| `IsPolicyPaidBroking` | `Public Function IsPolicyPaidBroking(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lDocumentId As Integer, ByRef r_bIsPaidInFull As Boolean, ByRef r_bIsPartiallyPaid As Boolean, ByRef r_cAmountOwing As Decimal, Optional ByRef r_bHasLiveMTA As Boolean = False) As Integer` | Check policy paid status (broking version) |
| `TakeOffHold` | `Public Function TakeOffHold(ByVal v_lAccountId As Integer) As Integer` | Take credit control records off hold for an account |
| `GetRuleList` | `Public Function GetRuleList(ByVal v_lSourceID As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Get list of credit control rules |
| `GetStepList` | `Public Function GetStepList(ByVal v_lCreditControlRuleId As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Get list of steps for a rule |
| `GetStepDetails` | `Public Function GetStepDetails(ByVal v_lCreditControlStepId As Object, Optional ByRef r_vCreditControlRuleID As Object = Nothing, ...) As Integer` | Get details of a credit control step |
| `GetRuleDetails` | `Public Function GetRuleDetails(ByVal v_lCreditControlRuleId As Integer, ...) As Integer` | Get details of a credit control rule |
| `GetDocTemplateList` | `Public Function GetDocTemplateList(ByRef r_vDocumentList(,) As Object) As Integer` | Get list of document templates |
| `DirectAddRule` | `Public Function DirectAddRule(ByRef r_vCreditControlRuleID As Object, ByVal v_vDescription As Object, ByVal v_vSourceID As Object, ...) As Integer` | Add a credit control rule |
| `DirectAddStep` | `Public Function DirectAddStep(ByRef r_vCreditControlStepID As Object, ByVal v_vCreditControlRuleID As Object, ByVal v_vStepNumber As Object, ...) As Integer` | Add a credit control step |
| `DirectEditRule` | `Public Function DirectEditRule(ByVal v_vCreditControlRuleID As Object, ByVal v_vDescription As Object, ByVal v_vSourceID As Object, ...) As Integer` | Edit a credit control rule |
| `DirectEditStep` | `Public Function DirectEditStep(ByVal v_vCreditControlStepID As Object, ByVal v_vCreditControlRuleID As Object, ...) As Integer` | Edit a credit control step |
| `DirectDeleteStep` | `Public Function DirectDeleteStep(ByVal v_lCreditControlStepId As Integer, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer` | Delete a credit control step |
| `DirectDeleteRule` | `Public Function DirectDeleteRule(ByVal v_lCreditControlRuleId As Integer, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer` | Delete a credit control rule |
| `GetALLPMWrkTaskGroupTasks` | `Public Function GetALLPMWrkTaskGroupTasks(ByRef r_vResults(,) As Object) As Integer` | Get all work tasks for task groups |
| `GetALLPMWrkTaskGroupPMUserGroups` | `Public Function GetALLPMWrkTaskGroupPMUserGroups(ByRef r_vResults(,) As Object) As Integer` | Get all user groups for task groups |
| `GetLookupValues` | `Public Function GetLookupValues(ByRef v_vLookupTables As Object, ByRef r_vLookupDetails As Object) As Integer` | Get credit control lookup values |
| `GetInstalmentImportInsuranceFileStatuses` | `Public Function GetInstalmentImportInsuranceFileStatuses(ByRef r_vResults(,) As Object) As Integer` | Get insurance file statuses for instalment imports |
| `AddCreditControlInsuranceFileStatuses` | `Public Function AddCreditControlInsuranceFileStatuses(ByVal v_lCreditControlRuleId As Integer, ByVal v_vInsuranceFileStatuses As Object, ...) As Integer` | Add insurance file status to credit control rule |
| `DeleteCreditControlRuleInsuranceFileStatus` | `Public Function DeleteCreditControlRuleInsuranceFileStatus(ByVal v_lCreditControlRuleId As Integer, ByVal v_sUniqueId As String, ByVal v_sScreenHierarchy As String) As Integer` | Delete insurance file statuses for a rule |
| `GetCreditControlRuleInsuranceFileStatuses` | `Public Function GetCreditControlRuleInsuranceFileStatuses(ByVal v_lCreditControlRuleId As Integer, ByRef r_vResults(,) As Object) As Integer` | Get insurance file statuses for a rule |
| `GetNextAvailableInstalmentFailureCount` | `Public Function GetNextAvailableInstalmentFailureCount(ByVal v_lCreditControlRuleId As Integer, ByRef r_lNextInstalmentFailureCount As Integer) As Integer` | Get next available instalment failure count |
| `CheckSingleInstalmentPlan` | `Public Function CheckSingleInstalmentPlan(ByVal nCreditControlItemID As Integer, ...) As Integer` | Check if item is a single instalment plan |
| `GetAutoCancelDetailForSingleInstalmentPlan` | `Public Function GetAutoCancelDetailForSingleInstalmentPlan(ByVal creditControlItemID As Integer, ...) As Integer` | Get auto-cancel details for single instalment plan |
| `GenerateLetterForBrokarDaysAndOutstandingAmount` | `Public Function GenerateLetterForBrokarDaysAndOutstandingAmount(ByVal nStepID As Integer, ...) As Integer` | Generate broker letters for outstanding amounts |
| `GetStepDetailsForSingleInstalmentPlan` | `Public Function GetStepDetailsForSingleInstalmentPlan(ByVal nStepID As Integer, ...) As Integer` | Get step details for single instalment plan |
| `GetClientIDForSingleInstalmentPlan` | `Public Function GetClientIDForSingleInstalmentPlan(ByVal nCreditControlItemID As Integer, ...) As Integer` | Get client ID for single instalment plan |
| `GetAutoCancelDetailForSingleInstalmentPlanForDocsGeneration` | `Public Function GetAutoCancelDetailForSingleInstalmentPlanForDocsGeneration(ByVal nCreditControlItemID As Integer, ...) As Integer` | Get auto-cancel details for document generation |
| `GetDetailsForAutoAllocate` | `Public Function GetDetailsForAutoAllocate(ByVal nCreditControlItemID As Integer, ...) As Integer` | Get details for auto-allocation |
| `CreditcontrolStepInUse` | `Public Function CreditcontrolStepInUse(ByVal nCreditControlStepID As Integer, ...) As Integer` | Check if a credit control step is in use |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_ACT_SelAll_Credit_Control_Rule` | `GetRuleList` | Select all credit control rules |
| `spu_ACT_SelAll_Credit_Control_Step` | `GetStepList` | Select all credit control steps |
| `spu_ACT_Select_Credit_Control_Rule` | `GetRuleDetails` | Select single credit control rule |
| `spu_ACT_Select_Credit_Control_Step` | `GetStepDetails` | Select single credit control step |
| `spu_ACT_Add_Credit_Control_Rule` | `DirectAddRule` | Add a credit control rule |
| `spu_ACT_Add_Credit_Control_Step` | `DirectAddStep` | Add a credit control step |
| `spu_ACT_Update_Credit_Control_Rule` | `DirectEditRule` | Update a credit control rule |
| `spu_ACT_Update_Credit_Control_Step` | `DirectEditStep` | Update a credit control step |
| `spu_ACT_Delete_Credit_Control_Rule` | `DirectDeleteRule` | Delete a credit control rule |
| `spu_ACT_Delete_Credit_Control_Step` | `DirectDeleteStep` | Delete a credit control step |
| `spu_ACT_Is_Policy_Paid` | `IsPolicyPaid` | Check if policy is fully paid |
| `spu_ACT_Is_Policy_Paid_B` | `IsPolicyPaidBroking` | Check policy paid status (broking) |
| `spu_ACT_Has_Policy_LiveMTA` | `AutoCancel` | Check if policy has live MTA |
| `spu_ACT_Credit_Control_Take_Off_Hold` | `TakeOffHold` | Take CC records off hold |
| `spu_ACT_Get_CC_Item_Insurance_File_Dets` | `AutoCancel` | Get insurance file details for auto-cancel script |
| `spu_ACT_Get_Party_Name_From_Account` | `AutoCancel` | Get party name from account |
| `spu_ACT_Get_Instalments_Paid_To_Date` | `AutoCancel` | Get instalment payments paid to date |
| `spu_ACT_Get_Credit_Control_Doc_IDs` | `ProduceClientLetters/ProduceOIPLetters` | Get document IDs for credit control letters |
| `spu_ACT_Get_Other_Interested_Parties` | `ProduceOIPLetters` | Get other interested parties |
| `spu_pm_session_id_alloc` | `CreateWorkManagerTask` | Get unique session ID |
| `spu_TempIDList_add` | `ProduceClientLetters` | Add to temp ID list |
| `spu_TempIDList_clear` | `ProduceClientLetters` | Clear temp ID list |
| `spu_pm_session_id_free` | `ReleaseTempSession` | Release session ID |
| `spu_ACT_select_Account` | `AutoCancel` | Get account details |
| `spu_Act_Check_AutoCancellation_Document` | `AutoCancel` | Check if cancellation document generated |
| `spu_email_contact_select` | `ProduceClientLetters` | Get email contact address |
| `spu_Get_Preferred_Correspondence_Address` | `ProduceClientLetters` | Get preferred correspondence address |
| `spu_ACT_PMwrk_Task_Group_Tasks_Select` | `GetALLPMWrkTaskGroupTasks` | Get all PMWrkTasks for groups |
| `spu_ACT_PMwrk_Task_Group_PMUserGroup_Select` | `GetALLPMWrkTaskGroupPMUserGroups` | Get all PMUserGroups for groups |
| `spu_ACT_Get_Account_UnallocatedCreditAmount` | `AutoCancel` | Get unallocated credit amount for account |
| `spu_ACT_Get_Instalment_Import_File_Insurance_File_Statuses` | `GetInstalmentImportInsuranceFileStatuses` | Get insurance file statuses |
| `spu_ACT_Credit_Control_Rule_Insurance_File_Status_Add` | `AddCreditControlInsuranceFileStatuses` | Add insurance file status to rule |
| `spu_ACT_Credit_Control_Rule_Insurance_File_Status_Delete` | `DeleteCreditControlRuleInsuranceFileStatus` | Delete insurance file status |
| `spu_ACT_Credit_Control_Rule_Insurance_File_Status_Select` | `GetCreditControlRuleInsuranceFileStatuses` | Select insurance file statuses |
| `spu_ACT_Credit_Control_Rule_Get_Selected_Instalment_Failure_Counts` | `GetNextAvailableInstalmentFailureCount` | Get selected instalment failure counts |
| `spu_SIR_GetInsuranceFileStatus` | `AutoCancel` | Get insurance file status |
| `spu_ACT_Get_Outstanding_Transactions_For_Insurance_Folder` | `AutoCancel` | Get outstanding transactions for insurance folder |
| `spu_SIR_CheckForRenewedPolicy` | `AutoCancel` | Check for renewed policy |
| `spu_ACT_Get_TransDetails_For_CreditControl` | `AutoCancel` | Get transaction details for credit control |
| `spu_ACT_CreditControlStepInUse` | `GetOriginalInsuranceFileCnt` | Validates if a Credit Control Step is in use for insurance file processing |
| `spu_ACT_GetDetailsForAutoAllocate_ForSIP` | `GetDetailsForAutoAllocate` | Retrieves details for automatic allocation on Single Instalment Plans |
| `spu_ACT_Select_Credit_Control_AutoCancel` | `GetAutoCancelDetail` | Retrieves Credit Control Auto Cancel details including party and insurance folder counts |
| `spu_ACT_Select_Credit_Control_AutoCancel_For_SingleInstalment` | `GetAutoCancelDetailForSingleInstalmentPlan` | Gets auto cancel details specific to Single Instalment Plan credit control items |
| `spu_Is_SingleInstallment_For_Credit_Control` | `CheckSingleInstalmentPlan` | Validates if a Credit Control Item is configured as a Single Instalment Plan |
| `spu_get_insurance_folder` | `GetInsuranceFolderCnt` | Gets insurance folder count for transaction processing by insurance file |
| `spu_ACT_Select_ClientID_For_Control_Control_Item` | `GetClientIDForSingleInstalmentPlan` | Gets ClientID associated with Credit Control Item for Single Instalment Plans |
| `spu_ACT_Select_Credit_Control_Details_For_DocsGeneration_For_SingleInstalment` | `GetAutoCancelDetailForSingleInstalmentPlanForDocsGeneration` | Retrieves auto cancel details for document generation on Single Instalment Plans |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTCreditControlItem.Business` | `AutoCancel` | Manage credit control items |
| `bACTAccount.Form` | `AutoCancel` | Account details and balance |
| `bSIREvent.Business` | `AutoCancel` | Event logging |
| `bSIRPremiumFinance.Business` | `AutoCancel` | Premium finance plan data |
| `bPMLookup.Business` | `Initialise` | Lookup operations |
| `bPMWrkTaskInstance.TaskControl` | `CreateWorkManagerTask/CreateTask` | Work manager task creation |

---

### 27. bACTCreditControlItem
**Directory:** `Orion/Components/CreditControlItem/Business/bACTCreditControlItem/`
**Project:** `bACTCreditControlItem`
**Purpose:** Individual credit control item management — CRUD operations for credit control items linked to insurance files, instalment plans, and claims. Supports setup of credit control items for failed instalments.

**Business Methods — Business (bBusiness.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Standard initialisation |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, ...) As Integer` | Set processing modes |
| `AddInstalment` | `Public Function AddInstalment(ByVal v_lPFInstalmentsID As Integer, ByVal v_sReason As String, ByVal v_cAmount As Decimal) As Integer` | Add instalment (retained for compatibility — returns error) |
| `GetList` | `Public Function GetList(ByRef r_vResultArray(,) As Object) As Integer` | Get list of all credit control items |
| `GetDetails` | `Public Function GetDetails(ByVal v_lCreditControlItemId As Integer, Optional ByRef r_vCreditControlReason As Object = Nothing, Optional ByRef r_vAccountID As Object = Nothing, ...) As Integer` | Get details of a credit control item |
| `DirectAdd` | `Public Function DirectAdd(ByRef r_vCreditControlItemID As Integer, ByVal v_vCreditControlReason As Object, ByVal v_vAccountID As Integer, ByVal v_vDocumentID As Object, ByVal v_vDocumentDate As Object, ByVal v_vInsuranceFileCnt As Integer, ByVal v_vPFPremFinanceCnt As Object, ByVal v_vPFPremFinanceVersion As Object, ByVal v_vAmount As Object, ByVal v_vCanAutoCancel As Object, ByVal v_vWillAutoCancel As Object, ByVal v_vCreditControlStepID As Object, ByVal v_vCreatedDate As Object, ByVal v_vDueDate As Object, ...) As Integer` | Add a credit control item |
| `DirectEdit` | `Public Function DirectEdit(ByVal v_vCreditControlItemID As Integer, ByVal v_vCreditControlReason As Object, ByVal v_vAccountID As Integer, ...) As Integer` | Edit a credit control item |
| `DirectDelete` | `Public Function DirectDelete(ByVal v_lCreditControlItemId As Integer) As Integer` | Delete a credit control item (soft) |
| `DirectDelete` | `Public Function DirectDelete(ByVal v_lCreditControlItemId As Integer, ByVal v_bDeletePermanent As Boolean, Optional ByVal v_iLetterSent As Integer = 0) As Integer` | Delete a credit control item (soft or permanent) |
| `AddInputParameter` | `Public Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer` | Add input parameter to database |
| `SetupCreditControlItemForInstalment` | `Public Function SetupCreditControlItemForInstalment(ByVal v_lInstalmentId As Integer, ByVal v_sDefaultCreditControlItemReason As String) As Integer` | Setup CC item for a failed instalment |
| `SetupCreditControlItemForInstalment` | `Public Function SetupCreditControlItemForInstalment(ByVal v_lInstalmentId As Integer, ByVal v_sDefaultCreditControlItemReason As String, ByVal v_lProcessMode As Integer) As Integer` | Setup CC item for a failed instalment (with process mode) |
| `GetCreditControlDetailsForInstalment` | `Public Function GetCreditControlDetailsForInstalment(ByVal v_lInstalmentId As Integer, ByRef r_vResults(,) As Object) As Integer` | Get CC details for an instalment |
| `GetCreditControlDetailsForInstalment` | `Public Function GetCreditControlDetailsForInstalment(ByVal v_lInstalmentId As Integer, ByRef r_vResults(,) As Object, ByVal v_lProcessMode As Integer) As Integer` | Get CC details for an instalment (with process mode) |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_ACT_SelAll_Credit_Control_Item` | `GetList` | Select all credit control items |
| `spu_ACT_Select_Credit_Control_Item` | `GetDetails` | Select a single credit control item |
| `spu_ACT_Add_Credit_Control_Item` | `DirectAdd` | Add a credit control item |
| `spu_ACT_Update_Credit_Control_Item` | `DirectEdit` | Update a credit control item |
| `spu_ACT_Delete_Credit_Control_Item` | `DirectDelete` | Delete a credit control item |
| `spu_ACT_Select_Credit_Control_Item_For_Plan` | `SetupCreditControlItemForInstalment` | Select CC item for instalment plan |
| `spu_ACT_Credit_Control_Item_Get_Plan_Details` | `SetupCreditControlItemForInstalment` | Get plan details for CC item |
| `spu_ACT_Get_Credit_Control_Details_For_Instalment` | `GetCreditControlDetailsForInstalment` | Get CC details for an instalment |
| `spu_ACT_Credit_Control_Item_Update` | `SetupCreditControlItemForInstalment` | Update CC item |
| `spu_ACT_Del_Credit_Control_Item_InsFile` | *(module-level constant)* | Delete CC item by insurance file cnt |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bPMLookup.Business` | `Initialise` | Lookup operations |

---

### 28. bACTCreditControlProcessing
**Directory:** `Orion/Components/CreditControl/Business/bACTCreditControlProcessing/`
**Project:** `bACTCreditControlProcessing`
**Purpose:** Batch credit control processing executable — command-line application that validates branch codes and triggers credit control processing via the Finance Spoke. Creates failure tasks when batch processing fails.

**Business Methods — Business (Business.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Standard initialisation |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, ...) As Integer` | Set processing modes |
| `IsBranchValid` | `Public Function IsBranchValid(ByVal v_sBranchCode As String) As Integer` | Validate that a branch code exists in the source table |
| `CreditControlProcessing` | `Public Function CreditControlProcessing(ByVal v_sBranchCode As String, ByVal v_dtDate As Date, ByVal v_bSpool As Boolean, ByVal v_bArchive As Boolean) As Integer` | Execute credit control processing via Finance Spoke |
| `CreateProcessFailedTask` | `Public Function CreateProcessFailedTask(ByVal v_sBranchCode As String, ByVal v_sDate As String, ByVal v_sDescription As String) As Integer` | Create a MEMO task indicating batch processing failure |
| `InitialiseBusiness` | `Public Function InitialiseBusiness() As Integer` | Creates business class instance with default SIRIUS/SIRIUS credentials and initialises database connection |
| `IsValidArchive` | `Public Function IsValidArchive(ByVal v_sCommandLineArg As String) As Integer` | Validates archive indicator command-line argument (must be "TRUE" or "FALSE") |
| `IsValidBranch` | `Public Function IsValidBranch(ByVal v_sCommandLineArg As String) As Integer` | Validates branch code is either "ALL" or exists in source table via IsBranchValid |
| `IsValidDate` | `Public Function IsValidDate(ByVal v_sCommandLineArg As String) As Integer` | Validates date argument (accepts date string, numeric delta days, or double-parseable format) |
| `IsValidSpool` | `Public Function IsValidSpool(ByVal v_sCommandLineArg As String) As Integer` | Validates spool indicator command-line argument (must be "TRUE" or "FALSE") |
| `ProcessCommandLineArgs` | `Public Function ProcessCommandLineArgs(ByRef r_sBranchCode As String, ByRef r_sDate As String, ByRef r_bSpool As Boolean, ByRef r_bArchive As Boolean) As Integer` | Parses and validates command-line arguments (BRANCH=, DATE=, SPOOL=, ARCHIVE=); applies defaults if not specified |
| `StartCreditControlProcessing` | `Public Function StartCreditControlProcessing(ByVal v_sBranchCode As String, ByVal v_sDate As String, ByVal v_bSpool As Boolean, ByVal v_bArchive As Boolean) As Integer` | Initiates credit control batch processing; converts date argument and calls Business.CreditControlProcessing |

**Main Module (MainModule.vb):**

| Routine | Description |
|---------|-------------|
| `Main()` | Entry point — initialises business, processes command-line args, starts credit control processing |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_PM_Select_Source_By_Code` | `IsBranchValid` | Validate branch code exists (via `{call ... (?)}` syntax) |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTFinanceSpoke.Business` | `CreditControlProcessing` | Execute credit control export via Finance Spoke |
| `bPMWrkTaskInstance.TaskControl` | `CreateProcessFailedTask` | Create failure notification task |

---

### 29. bACTCurrency
**Directory:** `Orion/Components/Currency/Business/bACTCurrency/`
**Project:** `bACTCurrency`
**Purpose:** Currency definition management — CRUD operations for currency records (ISO code, symbol, decimal places, format string, alignment). Provides both interactive Form class and Navigator-driven Automated class. Includes a Currencys collection class.

**Business Methods — Form (bACTCurrencyForm.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard initialisation |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, ...) As Integer` | Set processing modes |
| `DirectAdd` | `Public Function DirectAdd(Optional ByRef vCurrencyID As Integer = 0, Optional ByRef vCaptionID As Object = Nothing, ...) As Integer` | Add a currency record |
| `DirectDelete` | `Public Function DirectDelete(Optional ByRef vCurrencyID As Object = Nothing, ...) As Integer` | Delete a currency record |
| `GetDefaults` | `Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, ...) As Integer` | Get default values for a new currency |
| `CheckID` | `Public Function CheckID(ByRef vID As Object) As Integer` | Check if a currency ID exists |
| `GetCaptions` | `Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object) As Integer` | Get captions for currency fields |
| `GetCaptions` | `Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, ByRef vTable As Object) As Integer` | Get captions (with table) |
| `GetDetails` | `Public Function GetDetails(ByRef vCurrencyID As Object) As Integer` | Get details of a currency by ID |
| `GetDetails` | `Public Function GetDetails() As Integer` | Get all currency details |
| `GetDetails` | `Public Function GetDetails(ByRef vCurrencyID As Object, ByRef vLockMode As Integer) As Integer` | Get details with lock mode |
| `GetNext` | `Public Function GetNext(Optional ByRef vCurrencyID As Object = Nothing, ...) As Integer` | Get next currency from collection |
| `EditAdd` | `Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vCurrencyID As Object = Nothing, ...) As Integer` | Add a currency via edit mode |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vCurrencyID As Object = Nothing, ...) As Integer` | Update a currency via edit mode |
| `EditDelete` | `Public Function EditDelete(ByVal lRow As Integer) As Integer` | Delete a currency via edit mode |
| `Cancel` | `Public Function Cancel() As Integer` | Cancel pending changes |
| `Update` | `Public Function Update() As Integer` | Commit pending changes to database |
| `GetISOCodeFromCurrencyID` | `Public Function GetISOCodeFromCurrencyID(ByVal v_iCurrencyID As Integer, ByRef r_sISOCode As String) As Integer` | Get ISO code from currency ID |
| `GetCurrencyIdFromISO` | `Public Function GetCurrencyIdFromISO(ByVal v_sISOCode As String, ByRef r_iCurrencyId As Integer) As Integer` | Get currency ID from ISO code |
| `GetSystemCurrency` | `Public Function GetSystemCurrency(ByRef r_iCurrencyId As Integer) As Integer` | Get the system default currency |

**Business Methods — Automated (bACTCurrencyAutomated.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ..., Optional ByRef vDatabase As Object = Nothing) As Integer` | Standard initialisation |
| `SetProcessModes` | `Public Function SetProcessModes(...) As Integer` | Set processing modes |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Store parameter keys |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Return parameter keys |
| `Start` | `Public Function Start() As Integer` | Execute automated currency processing |

**Business Methods — Currency (bACTCurrency.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise() As Integer` | Initialise currency data object |

**Business Methods — Currencys (bACTCurrencys.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Add` | `Public Function Add(ByRef oNewCurrency As bACTCurrency.Currency) As Integer` | Add currency to collection |
| `Count` | `Public Function Count() As Integer` | Return number of currencies |
| `Delete` | `Public Sub Delete(ByRef vKey As Integer)` | Remove currency from collection by key |
| `Item` | `Public Function Item(ByRef vKey As Integer) As bACTCurrency.Currency` | Get currency by key |
| `DeleteAll` | `Public Sub DeleteAll()` | Remove all currencies |
| `Clear` | `Public Sub Clear()` | Clear currency collection |
| `Initialise` | `Public Function Initialise() As Integer` | Initialise collection |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_ACT_select_currency` | `GetDetails` (Form), `Start` (Automated) | Select a currency by ID |
| `spu_ACT_select_CurrencyCode` | `GetISOCodeFromCurrencyID`, `GetCurrencyIdFromISO` | Select a currency by ISO code |
| `spu_ACT_SelAll_currency` | `GetDetails` (all) | Select all currencies |
| `spu_ACT_check_currency` | `CheckID` | Check if currency ID exists |
| `spu_ACT_add_currency` | `DirectAdd`, `EditAdd`, `Update` | Add a currency record |
| `spu_ACT_delete_currency` | `DirectDelete`, `EditDelete`, `Update` | Delete a currency record |
| `spu_ACT_update_currency` | `EditUpdate`, `Update` | Update a currency record |
| `spu_ACT_GetSystemCurrency` | `GetSystemCurrency` | Get the system default currency |
| `spu_ACT_select_all_currency` | `Start` (Automated) | Select all currencies (automated) |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTCurrency.Currencys` | `Form.Initialise` | Internal collection of Currency objects |
| `bACTCurrency.Currency` | `Form/Currencys` | Individual currency data object |

---

### 30. bACTCurrencyConvert
**Directory:** `Orion/Components/CurrencyConvert/Business/bACTCurrencyConvert/`
**Project:** `bACTCurrencyConvert`
**Purpose:** Currency conversion and formatting — converts amounts between base, account, system, and transaction currencies. Provides formatting, amount-in-words, exchange rate lookups, insurance file and claim currency information retrieval, and currency-to-currency conversions.

**Business Methods — Form (bACTCurrencyConvertForm.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard initialisation — creates Currency, CurrencyRate, Company |
| `SetProcessModes` | `Public Function SetProcessModes(...) As Integer` | Set processing modes |
| `ConvertBaseToCurrency` | `Public Function ConvertBaseToCurrency(ByVal lCurrencyID As Integer, ByVal lCompanyID As Integer, ByVal cBaseAmount As Decimal, ByRef cCurrencyAmount As Decimal) As Integer` | Convert base amount to currency (minimal) |
| `ConvertBaseToCurrency` | `Public Function ConvertBaseToCurrency(..., ByRef vConversionDate As Object, ByRef vConversionRate As Object, ByRef vIsMultiplier As Object, ByRef vRounded As Object, ByRef vBaseRoundingDifference As Object, ByRef vCurrencyRoundingDifference As Object, ByRef vFormattedBase As Object, ByRef vFormattedCurrency As Object, ByRef lEuro As Integer, ByRef cEuroAmount As Decimal, ...) As Integer` | Convert base to currency (full parameters) |
| `ConvertCurrencytoBase` | `Public Function ConvertCurrencytoBase(ByVal lCurrencyID As Integer, ByVal lCompanyID As Integer, ByRef cBaseAmount As Decimal, ByVal cCurrencyAmount As Decimal, Optional ByRef vConversionDate As Object = Nothing, ...) As Integer` | Convert currency amount to base |
| `GetAmountInWords` | `Public Function GetAmountInWords(ByVal v_lCurrencyID As Integer, ByVal v_cCurrencyAmount As Decimal, ByRef r_sWords As String) As Integer` | Convert an amount to words with currency name |
| `FormatCurrency` | `Public Function FormatCurrency(ByVal vCurrencyID As Object, ByVal vCurrencyAmount As Object, ByRef vFormattedCurrency As String) As Integer` | Format amount per currency settings |
| `FormatCurrency` | `Public Function FormatCurrency(ByVal vCurrencyID As Object, ByVal vCurrencyAmount As Object, ByRef vFormattedCurrency As String, ByVal vConversionDate As Object) As Integer` | Format amount (with date) |
| `GetCurrencyDetails` | `Public Function GetCurrencyDetails(ByVal v_lCurrencyID As Integer) As Integer` | Get currency details (minimal) |
| `GetCurrencyDetails` | `Public Function GetCurrencyDetails(ByVal v_lCurrencyID As Integer, ByRef r_sFormatString As String) As Integer` | Get currency format string |
| `GetCurrencyDetails` | `Public Function GetCurrencyDetails(ByVal v_lCurrencyID As Integer, ByRef r_iDecimalPlaces As Integer) As Integer` | Get currency decimal places |
| `GetCurrencyDetails` | `Public Function GetCurrencyDetails(ByVal v_lCurrencyID As Integer, ByRef r_iDecimalPlaces As Integer, ByRef r_sFormatString As String, ByRef r_vMajorName As String, ByRef r_vMinorName As String) As Integer` | Get full currency details |
| `GetCurrencyRate` | `Public Function GetCurrencyRate(ByVal v_lCurrencyID As Integer, ByVal v_lCompanyId As Integer, ByVal v_dtConversionDate As Date, ByRef r_vConversionRate As Object) As Integer` | Get the exchange rate for a currency |
| `DoCurrencyConversion` | `Public Function DoCurrencyConversion(ByVal v_lAccountID As Integer, ByVal v_lCompanyId As Integer, ByVal v_iCurrencyID As Integer, ByVal v_cCurrencyAmountUnrounded As Decimal, ByRef r_iBaseCurrencyID As Integer, ByRef r_cBaseAmount As Decimal, ByRef r_iAccountCurrencyID As Integer, ByRef r_cAccountAmount As Decimal, ByRef r_iSystemCurrencyID As Integer, ByRef r_cSystemAmount As Decimal) As Integer` | Full multi-currency conversion (minimal) |
| `DoCurrencyConversion` | *(3 additional overloads with increasing rate/date return params)* | Full multi-currency conversion with exchange rates |
| `GetInsuranceFileInformation` | `Public Function GetInsuranceFileInformation(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Get insurance file currency info (minimal) |
| `GetInsuranceFileInformation` | *(3 additional overloads with increasing return params)* | Get insurance file currency/rate details |
| `UpdateInsuranceFile` | `Public Function UpdateInsuranceFile(ByVal v_lInsuranceFileCnt As Integer, ByVal v_dCurrencyBaseXrate As Double, ByVal v_dtCurrencyBaseDate As Object, ByVal v_dAccountBaseXrate As Double, ByVal v_dtAccountBaseDate As Object, ByVal v_dSystemBaseXrate As Double, ByVal v_dtSystemBaseDate As Object, ByVal v_lRateOverrideReasonID As Integer, ByVal v_iBaseCurrencyID As Integer, ByVal v_iAccountCurrencyID As Integer) As Integer` | Update insurance file exchange rates |
| `GetAccountIDFromPartyCnt` | `Public Function GetAccountIDFromPartyCnt(ByVal v_lPartyCnt As Integer, ByVal v_lCompanyId As Integer, ByRef r_lAccountID As Integer) As Integer` | Get account ID from party count and company |
| `Convert` | `Public Function Convert(ByVal v_bConvertToBase As Boolean, ByVal v_lCurrencyID As Integer, ByVal v_lCompanyId As Integer, ByRef r_cOriginalAmount As Decimal, ByRef r_cConvertedAmount As Decimal, ByRef r_vConversionRate As Object) As Integer` | Core conversion method (minimal) |
| `Convert` | `Public Function Convert(..., ByRef r_vConversionDate As Date, ByRef r_vConversionRate As Object, ByRef r_vConvertedAmountUnrounded As Object, ByRef r_vConvertedAmountRoundingDifference As Object, ByRef r_vFormattedOriginalAmount As Object, ByRef r_vFormattedConvertedAmount As Object) As Integer` | Core conversion method (full) |
| `GetBaseCurrency` | `Public Function GetBaseCurrency(ByVal v_lCompanyId As Integer, ByRef r_iBaseCurrencyID As Integer) As Integer` | Get base currency for a company |
| `GetClaimInformation` | `Public Function GetClaimInformation(ByVal sDocumentRef As String) As Integer` | Get claim currency info (minimal) |
| `GetClaimInformation` | `Public Function GetClaimInformation(ByVal sDocumentRef As String, ByRef r_lCompanyID As Integer, ByRef r_lAccountID As Integer, ByRef r_iCurrencyID As Integer, ...) As Integer` | Get claim currency information (full) |
| `GetClaimPaymentInformation` | `Public Function GetClaimPaymentInformation(ByVal sDocumentRef As String, ByRef r_lAccountID As Integer, ...) As Integer` | Get claim payment currency info |
| `GetClaimPaymentInformation` | *(additional overload with full params)* | Get claim payment currency info (full) |
| `GetClaimReceiptInformation` | `Public Function GetClaimReceiptInformation(ByVal sDocumentRef As String, ByRef r_lAccountID As Integer, ...) As Integer` | Get claim receipt currency info |
| `GetClaimReceiptInformation` | *(additional overload with full params)* | Get claim receipt currency info (full) |
| `CurrencyToCurrencyConversion` | `Public Function CurrencyToCurrencyConversion(ByVal v_lCurrencyIdFrom As Integer, ByVal v_crCurrencyAmountFrom As Decimal, ByVal v_lCompanyId As Integer, ByVal v_lCurrencyIdTo As Integer, ByRef r_crCurrencyAmountTo As Decimal) As Integer` | Convert between two non-base currencies |
| `CurrencyToCurrencyConversion` | `Public Function CurrencyToCurrencyConversion(..., ByVal dt_EffectiveDate As Object) As Integer` | Convert between currencies (with effective date) |
| `FindCurrencyBaseRateByAccount` | `Public Function FindCurrencyBaseRateByAccount(ByVal v_lAccountID As Integer, ByVal v_lCompanyId As Integer, ByRef r_dAccountBaseXrate As Double, ByVal r_dtAccountBaseDate As Date) As Integer` | Find currency base exchange rate by account |

**Module — ACTAmountInWords (ACTAmountInWords.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `AmountInWords` | `Public Function AmountInWords(ByVal v_vdAmount As Double, ByRef r_sMajor As String, ByRef r_sMinor As String) As Integer` | Convert numeric amount to English words |
| `GetNextEffectiveDate` | `Public Function GetNextEffectiveDate(ByVal v_bNext As Boolean, ByVal v_iCompanyID As Integer, ByRef r_dtEffectiveDate As Date) As Integer` | Gets next or previous effective date for currency rates based on company ID and direction flag |
| `GetTypeOfRates` | `Public Function GetTypeOfRates(ByRef r_iTypeOfRates As Integer) As Integer` | Retrieves the type of currency rates configured for the company |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_ACT_Do_Currency_Conversion` | `DoCurrencyConversion` | Perform multi-currency conversion |
| `spu_ACT_Get_Insurance_File_Information` | `GetInsuranceFileInformation` | Get insurance file currency/rate data |
| `spu_ACT_Update_Insurance_File` | `UpdateInsuranceFile` | Update insurance file exchange rates |
| `spu_ACT_Get_AccountID_From_partyCnt` | `GetAccountIDFromPartyCnt` | Get account ID from party and company |
| `spu_ACT_Get_Currency_Rate` | `GetCurrencyRate` | Get exchange rate for currency |
| `spu_ACT_Get_Claim_Information` | `GetClaimInformation` | Get claim currency information |
| `spu_ACT_Get_ClaimPayment_Information` | `GetClaimPaymentInformation` | Get claim payment currency information |
| `spu_ACT_Get_Claim_Receipt_Information` | `GetClaimReceiptInformation` | Get claim receipt currency information |
| `spu_ACT_Do_Currency_To_Currency_Conversion` | `CurrencyToCurrencyConversion` | Convert between two non-base currencies |
| `spu_ACT_CurrencyBaseRateByAccount` | `FindCurrencyBaseRateByAccount` | Get currency base rate by account |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTCurrency.Form` | `Initialise` | Currency details lookup |
| `bACTCurrencyRate.Form` | `Initialise` | Currency exchange rate lookup |
| `bACTCompany.Form` | `Initialise` | Company details (base currency) |

---

### 31. bACTCurrencyRate
**Directory:** `CurrencyRate/`
**Project:** `bACTCurrencyRate`
**Purpose:** Manages currency exchange rates against the base currency, including CRUD operations on rate records and applying rates across branches.

**Business Methods — Form (bACTCurrencyRateForm.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialise the Form object with credentials and database |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set optional process modes |
| `DirectAdd` | `Public Function DirectAdd(Optional ByRef vEffectiveFrom As Object = Nothing, Optional ByRef vRateAgainstBase As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing) As Integer` | Add a currency rate directly to database (not to collection) |
| `GetDefaults` | `Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vEffectiveFrom As Object = Nothing, Optional ByRef vRateAgainstBase As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing) As Integer` | Get default values for a currency rate |
| `GetDetails` | `Public Function GetDetails(ByVal v_lCompanyID As Integer, ByVal v_dtEffectiveFrom As Date) As Integer` | Get currency rates for a company/date and populate collection |
| `GetNext` | `Public Function GetNext(Optional ByRef vEffectiveFrom As Object = Nothing, Optional ByRef vRateAgainstBase As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing) As Integer` | Get next currency rate from collection |
| `GetCount` | `Public Function GetCount(ByRef v_lCount As Integer) As Integer` | Get the number of rows in the collection |
| `EditAdd` | `Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vEffectiveFrom As Object = Nothing, Optional ByRef vRateAgainstBase As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing) As Integer` | Add a currency rate to the collection |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vEffectiveFrom As Object = Nothing, Optional ByRef vRateAgainstBase As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing) As Integer` | Update a currency rate in the collection |
| `EditDelete` | `Public Function EditDelete(ByVal lRow As Integer) As Integer` | Mark a currency rate for deletion |
| `Cancel` | `Public Function Cancel() As Integer` | Check if collection has unsaved changes |
| `Update` | `Public Function Update() As Integer` | Persist all adds/updates/deletes to database |
| `GetRateForDate` | `Public Function GetRateForDate(ByVal lCurrencyID As Integer, ByVal lCompanyID As Integer, ByVal dtRateDate As Date, ByRef dRate As Double) As Integer` | Get exchange rate for a specific currency, company, and date |
| `ApplyToAllBranches` | `Public Function ApplyToAllBranches(ByVal lCurrencyID As Integer) As Integer` | Apply head office currency rates to all non-deleted branches |

**Stored Procedures (bACTCurrencyRateFormSQL.vb):**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `spu_ACT_selall_CurrencyRate` | `ACGetAllDetailsSQL` | Select all currency rates for company/date |
| `spu_ACT_add_CurrencyRate` | `ACAddSQL` | Add a new currency rate record |
| `spu_ACT_delete_CurrencyRate` | `ACDeleteSQL` | Delete a currency rate record |
| `spu_ACT_update_CurrencyRate` | `ACUpdateSQL` | Update a currency rate record |
| `spu_ACT_Get_Currency_Rate` | `ACGetRateForDateSQL` | Get rate for a specific currency/company/date |
| `spu_ACT_Apply_All_Currency_Rates` | `ACApplyCurrencyRateToAllBranchesSQL` | Apply currency rates to all branches |
| `spu_ACT_GetTypeOfRates` | `ACGetTypeOfRatesSQL` | Get type of rates |
| `spu_ACT_GetNextEffectiveDateForRates` | `ACGetNextEffectiveDateSQL` | Get next effective date for rates |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| (none) | — | Self-contained component |

---

### 32. bACTDocument
**Directory:** `Document/`
**Project:** `bACTDocument`
**Purpose:** Manages accounting documents (headers for batches of transactions) — CRUD operations, lookup values, auto-numbering, sub-branches, and complete document creation with suspense support.

**Business Methods — Form (bACTDocumentForm.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialise with credentials and database |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set optional process modes |
| `GetLookupValues` | `Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer` | Get lookup values for a document |
| `DirectAdd` | `Public Function DirectAdd(Optional ByRef vDocumentID As Integer = 0, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vPostingstatusID As Object = Nothing, Optional ByRef vDocumenttypeID As Object = Nothing, Optional ByRef vAuditsetID As Object = Nothing, Optional ByRef vBatchID As Object = Nothing, Optional ByRef vDocumentRef As Object = Nothing, Optional ByRef vDocumentDate As Object = Nothing, Optional ByRef vCreatedDate As Object = Nothing, Optional ByRef vAuthorisedDate As Object = Nothing, Optional ByRef vComment As Object = Nothing, Optional ByRef vWriteOffReasonID As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vReason As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vClaimID As Object = Nothing, Optional ByRef v_vTermsOfPaymentId As Object = Nothing, Optional ByRef v_vPaymentDueDate As Object = Nothing) As Integer` | Add a document directly to database |
| `DirectDelete` | `Public Function DirectDelete(Optional ByRef vDocumentID As Object = Nothing, ...) As Integer` | Delete a document directly from database |
| `GetDefaults` | `Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vDocumentID As Object = Nothing, ...) As Integer` | Get default values for a document |
| `CheckID` | `Public Function CheckID(ByRef vID As Object) As Integer` | Check if a document ID exists |
| `GetCaptions` | `Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object) As Integer` | Get caption fields for a record |
| `GetCaptions` (overload) | `Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, ByRef vTable As Object) As Integer` | Get caption fields with table parameter |
| `GetDetails` | `Public Function GetDetails(Optional ByRef vDocumentID As Object = Nothing, Optional ByRef vLockMode As Object = Nothing) As Integer` | Get documents and populate collection |
| `GetNext` | `Public Function GetNext(Optional ByRef vDocumentID As Object = Nothing, ...) As Integer` | Get next document from collection |
| `EditAdd` | `Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vDocumentID As Object = Nothing, ...) As Integer` | Add a document to collection |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vDocumentID As Object = Nothing, ...) As Integer` | Update a document in collection |
| `EditDelete` | `Public Function EditDelete(ByVal lRow As Integer) As Integer` | Mark document for deletion |
| `Cancel` | `Public Function Cancel() As Integer` | Check for unsaved changes |
| `Update` | `Public Function Update() As Integer` | Persist all changes to database |
| `GetSubBranches` | `Public Function GetSubBranches(ByVal v_lSourceID As Integer, ByRef r_vSubBranchArray(,) As Object) As Integer` | Get sub-branches for a source company |
| `GetDatePlusXMonths` | `Public Function GetDatePlusXMonths(ByVal v_vCurrentDate As Date, ByRef r_vNextDate As String, ByVal v_vOffset As Object, ByVal v_vMonths As Object) As Integer` | Calculate a date offset by months |
| `GetDateNext` | `Public Function GetDateNext(ByVal v_iNextType As Integer, ByVal v_vCurrentDate As Object, ByVal v_vOffset As Object, ByRef r_vNextDate As Object) As Integer` | Get next date based on type |
| `AddCompleteDocument` | `Public Function AddCompleteDocument(ByVal v_vInputs() As Object, ByRef r_vOutputs() As Object, ByRef r_vSuspenseArray(,) As Object, ByRef r_sAccountingBasis As String) As Integer` | Add a complete document with transactions and suspense |
| `GenerateNumber` | `Public Function GenerateNumber(ByRef v_sGroupCode As String, ByRef v_sRangeCode As String, ByRef v_iUserID As Integer, ByRef v_iCompanyID As Integer, ByRef r_lNumber As Integer) As Integer` | Generate sequential auto-number |
| `PoolNumber` | `Public Function PoolNumber(ByRef v_sRangeCode As String, ByRef v_sGroupCode As String, ByRef v_iUserID As Integer, ByRef v_iCompanyID As Integer, ByRef r_lNumber As Integer) As Integer` | Pool (release) an auto-number back |
| `GetGroupIDFromTypeID` | `Public Function GetGroupIDFromTypeID(ByVal v_lDocumentTypeID As Integer, ByRef r_lDocTypeGroupID As Integer) As Integer` | Get document type group from document type |
| `GetAutoNumValues` | `Public Function GetAutoNumValues(ByVal v_iDocumentTypeID As Integer, ByRef r_sGroupCode As String, ByRef r_sRangeCode As String) As Integer` | Get auto-numbering group/range codes for doc type |
| `GetOption` | `Public Function GetOption(ByVal v_iOptionNumber As Integer, ByRef r_sOptionValue As String) As Integer` | Get a system option value |
| `GetOption` (overload) | `Public Function GetOption(ByVal v_iOptionNumber As Integer, ByRef r_sOptionValue As String, ByRef vDatabase As Object) As Integer` | Get a system option with explicit database |
| `GenerateDocumentReferenceNumber` | `Public Function GenerateDocumentReferenceNumber(ByRef v_sGroupCode As String, ByRef v_sRangeCode As String, ByRef v_iUserID As Integer, ByRef v_iCompanyID As Integer, ByRef r_sDocumentRef As String) As Integer` | Generate a document reference number string |

**Business Methods — Automated (bACTDocumentAutomated.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialise automated process |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, ...) As Integer` | Set process modes |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Set keys from Navigator |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Get keys for Navigator |
| `Start` | `Public Function Start() As Integer` | Start the automated action |

**Stored Procedures (bACTDocumentFormSQL.vb + bACTDocumentAutomatedSQL.vb):**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `spu_ACT_select_Document` | `ACGetDetailsSQL` | Select a document by ID |
| `spu_ACT_select_all_Document` | `ACGetAllDetailsSQL` | Select all documents |
| `spu_ACT_check_Document` | `ACCheckIDSQL` | Check if document ID exists |
| `spu_ACT_add_Document` | `ACAddSQL` | Add a new document |
| `spu_ACT_delete_Document` | `ACDeleteSQL` | Delete a document |
| `spu_ACT_update_Document` | `ACUpdateSQL` | Update a document |
| `spe_ACTNumber_Group_sel` | `ACACTNumberGroupSelSQL` | Select auto-number group |
| `spe_ACTNumber_Range_sel` | `ACACTNumberRangeSelSQL` | Select auto-number range |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bPMLookup.Business` | `Form.Initialise` | Lookup values for document fields |
| `bSIROptions.Business` | `Form` (m_oSystemOption) | System option retrieval |

---

### 33. bACTDocumentPost
**Directory:** `DocumentPost/`
**Project:** `bACTDocumentPost`
**Purpose:** Posts accounting documents and their associated transactions — updates posting status, manages accounting dates/periods, adds documents with transactions, and handles batch/recurring document posting.

**Business Methods — Form (bACTDocumentPostCls.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialise with credentials and database |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, ...) As Integer` | Set optional process modes |
| `SetStatus` | `Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer` | Set process/map/step status |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Get keys for Navigator |
| `GetSummary` | `Public Function GetSummary(ByRef vSummaryArray As Object) As Integer` | Get summary for Navigator |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Set keys from Navigator (DocumentID, RecurringDocumentIDs, ReversingDocumentID) |
| `Start` | `Public Function Start() As Integer` | Entry point from Navigator — posts document(s) |
| `GetTransactionsForDoc` | `Public Function GetTransactionsForDoc(ByVal v_lDocumentID As Integer, ByRef r_vTransDetailIDs(,) As Object) As Integer` | Get transaction detail IDs for a document |
| `PostDocument` | `Public Function PostDocument() As Integer` | Post document using stored document ID |
| `PostDocument` (overload) | `Public Function PostDocument(ByVal v_lDocumentID As Integer) As Integer` | Post a specific document and its transactions |
| `AddDocument` | `Public Function AddDocument(ByVal v_lDocumentTypeId As Integer, ByVal v_sDocumentRef As String, ByVal v_dtDocumentDate As Date, ByVal v_sComment As String, Optional ByVal v_lInsuranceFileCnt As Integer = 0, Optional ByVal v_sReason As String = "", Optional ByRef r_vDocumentID As Object = Nothing, Optional ByVal r_vDocSourceID As Object = Nothing, Optional ByVal r_vSubBranchID As Object = Nothing, Optional ByVal v_vBatchID As Object = Nothing, Optional ByVal v_vClaimID As Object = Nothing, Optional ByVal v_vTermsOfPaymentId As Object = Nothing, Optional ByVal v_vPaymentDueDate As Object = Nothing) As Integer` | Add a document header record |
| `GetDocument` | `Public Function GetDocument(ByVal v_lDocumentID As Integer) As Integer` | Get an existing document |
| `GetLedgerIdForAccountId` | `Public Function GetLedgerIdForAccountId(ByVal v_vAccountID As Object, ByRef r_vLedgerID As Object) As Integer` | Get ledger ID from account ID |
| `AddTransaction` | `Public Function AddTransaction(ByVal v_lAccountID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_cAmount As Decimal, ByVal v_cCurrencyAmount As Decimal, ByVal v_vdCurrencyBaseXRate As Object, Optional ByRef r_vTransDetailId As Object = Nothing, Optional ByVal v_vDocumentSequence As Object = Nothing, Optional ByVal v_vComment As Object = Nothing, ...) As Integer` | Add a single transaction to the current document |
| `AddAdjustmentTransaction` | `Public Function AddAdjustmentTransaction(ByVal v_lAccountID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_cAmount As Decimal, ByVal v_cCurrencyAmount As Decimal, ByVal v_vdCurrencyBaseXRate As Object, ...) As Integer` | Add an adjustment transaction |
| `Commit` | `Public Function Commit() As Integer` | Commit the current transaction |
| `GetPeriodIdForDate` | `Public Function GetPeriodIdForDate(ByRef r_lPeriodId As Integer, ByVal v_dtAccountingDate As Date, ByRef lLedgerID As Integer) As Integer` | Get accounting period ID for a date |
| `AddDocumentTransactions` | `Public Function AddDocumentTransactions(ByRef r_vDocumentID As Object, ByVal v_lDocumentTypeId As Integer, ByVal v_sBranchID As String, ByVal v_sComment As String, ByVal v_dtDocumentDate As Date, ByVal v_vDocSourceID As Integer, ByRef v_sDocumentRef As String, ByVal v_vOperatorID As Object, ByVal v_vTransArray(,) As Object) As Integer` | Add document with multiple transactions in one call |
| `AddDocumentTransactions` (overload) | `Public Function AddDocumentTransactions(... ByRef v_lInsuranceFileCnt As Integer) As Integer` | Overload with insurance file count |
| `AddDocumentTransactions` (overload) | `Public Function AddDocumentTransactions(... Optional ByRef v_lInsuranceFileCnt As Integer = 0, Optional ByVal vTransdetailTypeID As Object = Nothing, Optional ByVal v_lSubBranchID As Object = Nothing) As Integer` | Full overload with sub-branch and trans type |

**Business Methods — NavigatorV3 (bNavigatorV3.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ...) As Integer` | Initialise NavigatorV3 wrapper |
| `NavigatorV3_SetKeys` | `Public Function NavigatorV3_SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Navigator SetKeys |
| `NavigatorV3_GetKeys` | `Public Function NavigatorV3_GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Navigator GetKeys |
| `NavigatorV3_GetSummary` | `Public Function NavigatorV3_GetSummary(ByRef vSummaryArray As Object) As Integer` | Navigator GetSummary |
| `NavigatorV3_SetProcessModes` | `Public Function NavigatorV3_SetProcessModes(Optional ByRef vTask As Object = Nothing, ...) As Integer` | Navigator SetProcessModes |
| `NavigatorV3_Start` | `Public Function NavigatorV3_Start() As Integer` | Navigator Start |

**Stored Procedures (bACTDocumentPost.vb — MainModule):**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `spu_ACT_Get_LedgerFromDocument` | `ACGetLedgerFromDocumentSQL` | Get ledger ID from document ID |
| `spu_ACT_Do_UpdateTransActDate` | `ACUpdateTransActDateSQL` | Update transdetail accounting date based on closed periods |
| `spu_update_ManualJournalDetail` | `UpdateManualJournalWithTransdetail` | Updates manual journal detail record with transaction detail |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTDocument.Form` | `Form.Initialise` | Document header CRUD |
| `bACTTransdetail.Form` | `Form.Initialise` | Transaction detail CRUD |
| `bACTPeriod.Form` | `UpdateAccountingDates` | Accounting period lookup |

---

### 34. bACTDocumentReversal
**Directory:** `DocumentReversal/`
**Project:** `bACTDocumentReversal`
**Purpose:** Reverses accounting documents and their transactions — handles insurer payments, allocations, cash list item reversals, FAP (Future Annual Premium) reversals, released accounts transactions recall, introducer/direct-to-insurer reversal, and credit control item cleanup.

**Business Methods — Business (bACTDocumentReversalCls.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialise with credentials and database |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, ...) As Integer` | Set process modes (no-op in this component) |
| `SetStatus` | `Public Function SetStatus(ByVal sProcessStatus As String, ByVal sMapStatus As String, ByVal sStepStatus As String) As Integer` | Set status values |
| `SearchDetails` | `Public Function SearchDetails(ByVal v_lMarkedStatus As Integer, ByVal v_lMonth As Integer) As Integer` | Search insurer payment details (simple overload) |
| `SearchDetails` (overload) | `Public Function SearchDetails(ByVal v_lMarkedStatus As Integer, ByVal v_lMonth As Integer, ByVal v_vAccountID As Object, ByVal v_vDateTo As String, ByRef lNumberOfRecords As Integer, r_vResultArray(,) As Object) As Integer` | Search insurer payment details with filters and return results |
| `GetFAPReversalInfo` | `Public Function GetFAPReversalInfo(ByVal v_lDocumentId As Integer) As Integer` | Get FAP reversal info (simple overload) |
| `GetFAPReversalInfo` (overload) | `Public Function GetFAPReversalInfo(ByVal v_lDocumentId As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Get Future Annual Premium reversal info |
| `CheckMatch` | `Public Function CheckMatch(ByVal v_lTransDetailID As Integer) As Integer` | Check if a transaction has been matched/allocated |
| `Start` | `Public Function Start(Optional ByRef r_vCreditTransDetailID() As Object = Nothing, Optional ByRef r_vDebitTransDetailID() As Object = Nothing, Optional ByRef r_sFailureReason As String = "", Optional ByVal v_bDisableTransactions As Boolean = False) As Integer` | Perform the full document reversal |
| `DoPartialReceiptReversal` | `Public Function DoPartialReceiptReversal(ByVal v_lReceiptDocumentId As Integer, ByVal v_sDoumentRef As String, ByVal v_dAmount As Double, ByVal v_dCurrencyAmount As Double, ByRef r_sDocumentRef As String) As Integer` | Reverse a partial receipt amount |
| `DoReceiptTransdetail` | `Public Function DoReceiptTransdetail(ByVal v_lReceiptDocumentId As Integer, ByVal v_sReceiptDocumentRef As String, ByVal v_dAmount As Double, ByVal v_dCurrencyAmount As Double, ByRef r_sDocumentRef As String) As Integer` | Process receipt transaction detail reversal |
| `RecallReleasedAccountsTransaction` | `Public Function RecallReleasedAccountsTransaction(ByVal vAssociatedDocuments(,) As Object) As Integer` | Recall released accounts transactions |
| `CheckTransactionForReversal` | `Public Function CheckTransactionForReversal(ByVal v_bOnlyCheckForInvalidTransaction As Boolean, ByRef r_vCheckResults As Object) As Integer` | Validate a transaction can be reversed |
| `BeginTrans` | `Public Function BeginTrans() As Integer` | Begin database transaction |
| `CommitTrans` | `Public Function CommitTrans() As Integer` | Commit database transaction |
| `RollbackTrans` | `Public Function RollbackTrans() As Integer` | Rollback database transaction |

**Stored Procedures (bACTDocumentReversal.vb — MainModule):**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `spu_ACT_Do_InsurerPayments_All` | `ACInsurerPaymentsSQL` | Search insurer payment details for reversal |
| `spu_ACT_Select_CashListItemID_From_TransDetailID` | `ACSelectCLIIDfromTDIDSQL` | Get cash list item ID from transaction detail ID |
| `spu_ACT_Check_DirectToInsurer` | `ACCheckDirectToInsurerSQL` | Check if transaction is direct to insurer |
| `spu_ACT_Check_Reconciled` | `ACCheckReconciledSQL` | Check if transaction has been reconciled |
| `spu_get_fap_reversal_info` | `ACGetFAPReversalInfoSQL` | Get Future Annual Premium reversal info |
| `spu_ACT_Get_ReleasedAccountsTransactions_ForReversal` | `ACGetReversedAccountsTransactionsforReversalSQL` | Get released accounts transactions for reversal |
| `spu_ACT_ReleasedAccountsTransactions_Recall` | `ACRecallReleasedAccountsTransactionsSQL` | Recall released accounts transactions |
| `spu_ACT_Get_Introducer_Trans_For_Reversal` | `ACGetIntroducerTransforReversalSQL` | Get introducer transactions for reversal |
| `spu_ACT_Get_Direct_To_Insurer_Trans_For_Reversal` | `ACGetDirectToInsurerTransforReversalSQL` | Get direct-to-insurer transactions for reversal |
| `spu_ACT_CashListItem_Mark_Reversed` | `ACCashListItemMarkReversedSQL` | Mark cash list item as reversed |
| `spu_ACT_Delete_Credit_Control_Item_DocId` | `ACReverseCreditControlItemSQL` | Delete credit control item by document ID |
| `spu_ACT_Select_Document` | `kGetDocumentDetailSQL` | Get document detail |
| `spu_ACT_Select_TransDetail` | `kSelectTransDetailSQL` | Get transaction detail |
| `spu_GetAccountIDfromInsuranceFileCnt` | `kGetAccountIDfromInsuranceFileCntSQL` | Get account ID from insurance file count |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTDocument.Form` | `Business.Initialise` | Document header operations |
| `bACTTransdetail.Form` | `Business.Initialise` | Transaction detail operations |
| `bACTAllocationManual.Business` | `Business.Initialise` | Reverse allocation matching |
| `bACTPeriod.Form` | `Business.Initialise` | Accounting period lookup |
| `bSIRInsuranceFile.Business` | `Business.Initialise` | Insurance file lookup (FAP reversal) |
| `bACTInsurerPaymentSFU.Business` | `Business.Initialise` | Insurer payment processing |

---

### 35. bACTExplorer
**Directory:** `AccountExplorer/`
**Project:** `bACTExplorer`
**Purpose:** Account Explorer — manages the chart of accounts structure tree (elements, nodes, mappings), account lookup by type/short code/key/full path, element extras, branch/sub-branch retrieval, and ledger details.

**Business Methods — Form (bACTExplorerForm.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialise with credentials and database |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, ...) As Integer` | Set optional process modes |
| `BeginTrans` | `Public Function BeginTrans() As Integer` | Begin database transaction |
| `CommitTrans` | `Public Function CommitTrans() As Integer` | Commit database transaction |
| `RollbackTrans` | `Public Function RollbackTrans() As Integer` | Rollback database transaction |
| `GetAccountDetails` | `Public Sub GetAccountDetails(ByRef lAccountId As Integer, Optional ByRef vAccountName As String = "", Optional ByRef vShortCode As String = "", Optional ByRef vAccountType As Object = Nothing, Optional ByRef vFullKey As Object = Nothing, Optional ByRef vBalance As Object = Nothing, Optional ByRef vNominalCode As Object = Nothing, Optional ByRef vCompanyId As Object = Nothing, Optional ByRef vLedgerID As Object = Nothing, Optional ByRef iSubBranchID As Integer = 0)` | Get details for an account by ID |
| `GetAccountIdFromType` | `Public Function GetAccountIdFromType(ByVal v_iAccountTypeId As Integer, ByRef r_vAccountIds(,) As Object, ByVal v_vLedgerID As String) As Integer` | Get account IDs by account type |
| `GetAccountIdFromShort` | `Public Function GetAccountIdFromShort(ByVal v_sShortCode As String, ByRef r_vAccountIds(,) As Object) As Integer` | Get account IDs by short code |
| `GetAccountIdFromShort` (overload) | `Public Function GetAccountIdFromShort(ByVal v_sShortCode As String, ByRef r_vAccountIds(,) As Object, ByVal v_vCompanyId As Object) As Integer` | Get account IDs by short code and company |
| `GetAccountIdFromKey` | `Public Function GetAccountIdFromKey(ByVal v_lKey As Integer, ByRef r_vAccountIds(,) As Object) As Integer` | Get account IDs by account key |
| `GetAccountIdFromKey` (overload) | `Public Function GetAccountIdFromKey(ByVal v_lKey As Integer, ByRef r_vAccountIds(,) As Object, ByVal v_iCompanyId As Integer) As Integer` | Get account IDs by key and company |
| `InsertAccount` | `Public Function InsertAccount(ByRef r_lAccountID As Integer, ByRef r_vAccountName As Object) As Integer` | Insert a new account (simple) |
| `InsertAccount` (overload) | `Public Function InsertAccount(ByRef r_lAccountID As Integer, ByRef r_vAccountName As Object, ByRef r_vShortCode As Object, ByRef r_vAccountType As Object, ByRef r_vLedgerId As Object, ByRef r_vCompanyID As Object) As Integer` | Insert account with full details |
| `InsertAccount` (overload) | `Public Function InsertAccount(ByRef r_lAccountID As Integer, ByRef r_vAccountName As Object, ByRef r_vShortCode As Object, ByRef r_vAccountType As Object, ByRef r_vLedgerId As Object, ByRef r_vCompanyID As Object, ByRef r_vSubBranchID As Object) As Integer` | Insert account with sub-branch |
| `DeleteAccount` | `Public Function DeleteAccount(ByVal lAccountId As Integer) As Boolean` | Delete an account |
| `GetElementFromAccountID` | `Public Function GetElementFromAccountID(ByVal v_lAccountId As Integer) As Integer` | Get element ID from account ID |
| `GetNodeFromAccountID` | `Public Function GetNodeFromAccountID(ByVal v_lAccountId As Integer) As Integer` | Get node ID from account ID |
| `GetNodeFromMappingID` | `Public Function GetNodeFromMappingID(ByVal v_lMappingId As Integer) As Integer` | Get node ID from mapping ID |
| `GetNodeFromMappingID` (overload) | `Public Function GetNodeFromMappingID(ByVal v_lMappingId As Integer, ByRef r_vParentNodeId As Integer) As Integer` | Get node and parent from mapping ID |
| `GetNodeFromMappingText` | `Public Function GetNodeFromMappingText(ByVal v_sMappingText As String, ByRef v_lNodeId As Integer) As Integer` | Get node from mapping text |
| `GetAccountIdFromFullPath` | `Public Sub GetAccountIdFromFullPath(ByVal v_sFullKey As String, ByRef r_lAccountID As Integer)` | Get account ID from full key path |
| `GetAccountIdFromFullPath` (overload) | `Public Sub GetAccountIdFromFullPath(ByVal v_sFullKey As String, ByRef r_lAccountID As Integer, ByVal v_vCompanyId As Object)` | With company filter |
| `GetNodeIdFromFullPath` | `Public Sub GetNodeIdFromFullPath(ByVal v_sFullKey As String, ByRef r_lNodeId As Integer, ByRef r_vElementId As Double, ByRef r_vParentNodeId As Double)` | Get node details from full path |
| `GetNodeIdFromFullPath` (overloads) | Multiple overloads with company and separator | Navigate account structure tree by path |
| `GetNode` | `Public Sub GetNode(ByVal lNodeId As Integer, ByRef vResultArray(,) As Object)` | Get node details |
| `GetAccountsOfNode` | `Public Sub GetAccountsOfNode(ByVal v_lParentNodeId As Integer, ByRef r_vResultArray() As Object)` | Get accounts under a node |
| `GetChildrenOfNode` | `Public Sub GetChildrenOfNode(ByVal lParentNodeId As Integer, ByRef lNumberRecords As Integer, ByRef vResultArray(,) As Object)` | Get child nodes (multiple overloads with code/company) |
| `FullKey` | `Public Function FullKey(ByVal lAccountId As Integer) As String` | Get full key path for account |
| `FullKeyExists` | `Public Function FullKeyExists(ByVal v_sFullKey As String) As Integer` | Check if full key path exists |
| `FullKeyExists` (overload) | `Public Function FullKeyExists(ByVal v_sFullKey As String, ByRef r_vAccountId As Integer, ByVal v_vCompanyId As Object) As Integer` | Check existence with return account ID |
| `GetBranches` | `Public Function GetBranches(ByRef r_vBranchArray(,) As Object) As Integer` | Get all branches |
| `GetSubBranches` | `Public Function GetSubBranches(ByRef r_vSubBranchArray(,) As Object) As Integer` | Get all sub-branches |
| `GetSubBranches` (overload) | `Public Function GetSubBranches(ByRef r_vSubBranchArray(,) As Object, ByVal v_vCompanyId As Integer) As Integer` | Get sub-branches for company |
| `GetFullPath` | `Public Sub GetFullPath(ByVal lNodeId As Integer, ByRef vFullPath As Object)` | Build full path from node (multiple overloads) |
| `GetLookupValues` | `Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray As Object) As Integer` | Get lookup values |
| `GetLookupValues` (overload) | `Public Function GetLookupValues(... ByRef vReportMapId As String) As Integer` | Get lookup values with report map |
| `GetCompanyDetails` | `Public Sub GetCompanyDetails(ByVal lAccountId As Integer, ByRef vCompany As Object, ByRef vSubBranch As String)` | Get company details for account |
| `GetCompanyIdDetails` | `Public Sub GetCompanyIdDetails(ByVal lAccountId As Integer)` | Get company ID for account |
| `GetCompanyIdDetails` (overload) | `Public Sub GetCompanyIdDetails(ByVal lAccountId As Integer, ByRef vCompanyId As Object)` | With return value |
| `GetElementExtras` | `Public Sub GetElementExtras(ByVal lElementId As Integer, ByRef vReportMapId As Object, ByRef vAccountMapID As Object)` | Get element extras (simple) |
| `GetElementExtras` (overload) | `Public Sub GetElementExtras(ByVal lElementId As Integer, ByRef vTotallingId As Object, ByRef vDescription As String, ByRef vReportMapId As Object, ByRef vAccountMapID As Object, ByRef vIsDeletable As Object, ByRef vGroupForGLExportInd As Object)` | Get element extras (full) |
| `GetLedgerDetails` | `Public Function GetLedgerDetails(ByRef vResultArray(,) As Object) As Integer` | Get all ledger details |
| `InsertElementExtras` | `Public Function InsertElementExtras(ByRef lElementId As Integer, ByRef vTotallingId As Object, ByRef vDescription As Object, ByRef vReportMapId As Object, ByRef vAccountMapID As Object, ByRef vIsDeletable As Object) As Integer` | Insert element extras record |
| `DeleteElement` | `Public Function DeleteElement(ByVal v_lElementId As Integer) As Boolean` | Deletes an element; fails if referenced in StructureTree or Element tables |
| `DeleteElementExtras` | `Public Function DeleteElementExtras(ByVal v_lElementId As Integer) As Boolean` | Deletes element extras (metadata) for specified element ID |
| `DeleteMapping` | `Public Function DeleteMapping(ByRef lMapID As Integer) As Boolean` | Deletes mapping; fails if referenced by StructureTree table |
| `DeleteNode` | `Public Function DeleteNode(ByRef lNodeId As Integer) As Boolean` | Deletes a node and its element if not referenced by other records |
| `GetElementRelationships` | `Public Sub GetElementRelationships(ByRef lNumberRecords As Integer, ByRef vResultArray(,) As Object)` | Retrieves element relationships defined in Elements table (parent-child) |
| `GetLedgerOfNode` | `Public Function GetLedgerOfNode(ByRef r_lNodeId As Integer, ByRef r_iLedgerId As Integer) As Integer` | Returns the ledger ID associated with specified node by traversing hierarchy |
| `GetStructureTree` | `Public Function GetStructureTree(ByRef r_vNodeId As Object, ByRef r_vMappingID As Object, ByRef r_vAccountId As Object, ByRef r_vElementId As Object, ByRef r_vParentNodeId As Object, ByRef r_vResultArray(,) As Object, ByRef r_vRecordCount As Object) As Integer` | Selects StructureTree records using up to 5 key parameters (NodeId, ElementId, ParentNodeId, AccountId, MappingId) |
| `InsertElement` | `Public Function InsertElement(ByRef sElementName As String, ...) As Integer` | Inserts element with required name and optional parent, totalling, description, report/account mapping, deletable flag (6 overloads) |
| `InsertMapping` | `Public Function InsertMapping(ByRef sMapName As String, ...) As Integer` | Inserts mapping with required name and optional mapping type (2 overloads) |
| `InsertNode` | `Public Function InsertNode(ByRef lParentNodeId As Integer, ByRef lElementId As Integer, ...) As Integer` | Inserts node with required parent node and element ID; optional account ID and error number (3 overloads) |
| `IsDuplicateError` | `Public Function IsDuplicateError(ByRef lParentNodeId As Integer, ByRef vElementName As Object, ByRef vElementID As Object, ByRef vErrorNum As Integer) As Boolean` | Checks if element name is duplicate of existing sibling under parent node |
| `IsNodeDeletable` | `Public Function IsNodeDeletable(ByRef lNodeId As Integer) As Boolean` | Validates that node can be deleted (no accounts attached, not creating circular path) |
| `IsNodeMovable` | `Public Function IsNodeMovable(ByRef lSrceNodeID As Integer, ByRef lDestNodeID As Integer, ByRef vErrorNum As Integer) As Boolean` | Validates node can be moved to destination; prevents circular paths and account-attached nodes |
| `LookupElementId` | `Public Function LookupElementId(ByVal sElementName As String, ...) As Integer` | Returns element ID matching given element name; optional company ID filtering (2 overloads) |
| `LookupElementName` | `Public Function LookupElementName(ByRef lElementId As Integer) As String` | Returns element name matching given element ID |
| `MapNode` | `Public Function MapNode(ByRef lNodeId As Integer, ByRef lMapID As Integer, ByRef vErrorNum As Integer) As Boolean` | Maps node to mapping ID; if MapId=0 unmaps the node |
| `MoveNode` | `Public Function MoveNode(ByRef lSrceNodeID As Integer, ByRef lDestNodeID As Integer, ByRef vErrorNum As Integer) As Boolean` | Moves node by updating its ParentNodeID after IsNodeMovable validation |
| `PickListLoad` | `Public Function PickListLoad(ByRef sPickListType As String, ByRef vFKArray(,) As Object, ByRef vResultArray As Object) As Integer` | Standard PickList control load method; calls spu_ACTSecurity_PLL for given type |
| `PickListSave` | `Public Function PickListSave(ByRef sPickListType As String, ByRef vFKArray(,) As Object, ByRef vKeys As Object) As Integer` | Saves PickList selections via spu_ACTSecurity_PLD (delete) and spu_ACTSecurity_PLS (save) |
| `UpdateElement` | `Public Function UpdateElement(ByRef vNodeId As Integer, ByRef vElementName As Object, ByRef vErrorNum As Integer, ...) As Boolean` | Updates element name/parent with duplicate checking (2 overloads) |
| `UpdateElementExtras` | `Public Function UpdateElementExtras(ByRef lElementId As Integer, ByRef vTotallingId As Object, ByRef vDescription As Object, ByRef vReportMapId As Object, ByRef vAccountMapID As Object, ByRef vIsDeletable As Object, ByRef vGroupForGLExportInd As Object) As Boolean` | Updates element metadata (totalling, description, report/account mapping, GL export flag) |
| `UpdateMapping` | `Public Function UpdateMapping(ByRef lMapID As Integer, ...) As Boolean` | Updates mapping with optional name and type (3 overloads) |

**Stored Procedures (bACTExplorerFormSQL.vb):**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `spu_ACT_select_Account` | `ACGetAccountDetailsSQL` | Select account details by ID |
| `spu_ACT_selall_Element` | `ACGetAllElementDetailsSQL` | Select all elements |
| `spu_ACT_select_Element` | `ACGetElementDetailsSQL` | Select element by ID |
| `spu_ACT_update_Element` | `ACUpdateElementSQL` | Update an element |
| `spu_ACT_add_Element` | `ACAddElementSQL` | Add an element |
| `spu_ACT_delete_Element` | `ACDeleteElementSQL` | Delete an element |
| `spu_ACT_Select_IsUsed_Element` | `ACIsUsedElementSQL` | Check if element is in use |
| `spu_ACT_Do_IsDuplicate_Element` | `ACIsDuplicateElementSQL` | Check for duplicate element |
| `spu_ACT_Select_StructureTree` | `ACGetStructureTreeDetailsSQL` | Select structure tree |
| `spu_ACT_add_StructureTree` | `ACAddStructureTreeSQL` | Add structure tree node |
| `spu_ACT_delete_StructureTree` | `ACDeleteStructureTreeSQL` | Delete structure tree node |
| `spu_ACT_Update_StructMapID` | `ACUpdateStructureTreeMapIDSQL` | Update structure tree map ID |
| `spu_ACT_Update_StructParentID` | `ACUpdateStructureTreeParentIDSQL` | Update structure tree parent ID |
| `spu_ACT_Update_StructAccountID` | `ACUpdateStructureTreeAccountIdSQL` | Update structure tree account ID |
| `spu_ACT_Select_StructNode` | `ACGetStructNodeDetailsSQL` | Select structure tree node details |
| `spu_ACT_Select_StructChildren` | `ACGetStructChildrenDetailsSQL` | Select children of structure tree node |
| `spu_ACT_Select_StructClients` | `ACGetStructClientDetailsSQL` | Select structure tree clients |
| `spu_ACT_add_Mapping` | `ACAddMappingSQL` | Add a mapping |
| `spu_ACT_update_Mapping` | `ACUpdateMappingSQL` | Update a mapping |
| `spu_ACT_delete_Mapping` | `ACDeleteMappingSQL` | Delete a mapping |
| `spu_ACT_Do_Get_Node_From_Map` | `ACSelectNodeFromMappingSQL` | Get node from mapping |
| `spu_ACT_Select_AccountType` | `ACSelectAccountByTypeSQL` | Select accounts by type |
| `spu_ACT_Select_AccountShort` | `ACSelectAccountByShortSQL` | Select account by short code |
| `spu_ACT_Select_AccountKey` | `ACSelectAccountByKeySQL` | Select account by key |
| `spu_ACT_Select_AccountBranch` | `ACSelectBranchByAccountSQL` | Select branch from account ID |
| `spu_ACT_Select_AccountBranchId` | `ACSelectBranchIdByAccountSQL` | Select branch ID from account ID |
| `spe_ElementExtras_sel` | `ACGetElementExtrasDetailsSQL` | Select element extras |
| `spe_ElementExtras_upd` | `ACUpdateElementExtrasSQL` | Update element extras |
| `spe_ElementExtras_add` | `ACAddElementExtrasSQL` | Add element extras |
| `spe_ElementExtras_del` | `ACDeleteElementExtrasSQL` | Delete element extras |
| `spu_ACT_selall_Ledger` | `ACGetLedgerDetailsSQL` | Select all ledgers |
| `spu_ACTSecurity_PLD` | `PickListSave` | Deletes existing picklist security records for account explorer before reload |
| `spu_ACTSecurity_PLL` | `PickListLoad` | Loads picklist data for security control based on FK array |
| `spu_ACTSecurity_PLS` | `PickListSave` | Saves picklist security records after validation |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bPMLookup.Business` | `Form.Initialise` | Lookup values for elements/accounts |
| `bACTAccount.Form` | `Form.Initialise` | Account CRUD operations |

---

### 36. bACTExportCashListItems
**Directory:** `ExportCashListItems/`
**Project:** `bACTExportCashListItems`
**Purpose:** Exports cash list item data to CSV — retrieves branch details, media types, export data from cash list items, and sets the export flag to prevent duplicate exports. Implements `SSP.S4I.Interfaces.IBusiness`.

**Business Methods — Business (Business.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceid As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialise with credentials and database (implements IBusiness) |
| `GetBranchDetails` | `Public Function GetBranchDetails(ByVal iSourceid As Integer, ByRef sBranchCode As String, ByRef sBranchDesc As String) As Integer` | Get branch code and description for a source ID |
| `GetAllBranches` | `Public Function GetAllBranches(ByRef vBranches(,) As Object) As Integer` | Get all branch source IDs |
| `MultiBranchCheck` | `Public Function MultiBranchCheck(ByRef lMulti As Integer) As Integer` | Check if multi-branch mode is enabled (hidden_options 16) |
| `GetMediaTypes` | `Public Function GetMediaTypes(ByRef vMediaTypes(,) As Object) As Integer` | Get all media types for dropdown list |
| `GetExportData` | `Public Function GetExportData(ByVal sMediaDesc As String, ByVal iSourceid As Integer, ByRef vExportData(,) As Object) As Integer` | Get cash list items data for CSV export |
| `SetExportFlag` | `Public Function SetExportFlag(ByVal iSourceid As Integer) As Integer` | Set exported flag on cash list items to prevent re-export |
| `GetNextEDIBatchNumber` | `Public Function GetNextEDIBatchNumber(ByVal v_sBatchType As String, ByRef lNextBatchNumber As Integer) As Integer` | Retrieves next EDI batch number from database for given batch type |
| `UpdateNextEDIBatchNumber` | `Public Function UpdateNextEDIBatchNumber(ByVal v_sBatchType As String) As Integer` | Increments EDI batch number in database for specified batch type |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_ACT_get_cashlist_export_data` | `GetExportData` (inline SQL exec) | Get cash list export data for a branch |
| `spu_get_finance_data_positions` | `GetFinanceDataPos` | Returns column positions for finance plan data used in EDI export file parsing |
| `spu_get_finance_plan_details` | `GetFinanceDetails` | Retrieves finance plan details for export processing |
| `spu_get_next_edi_batch_number` | `GetNextEDIBatchNumber` | Returns current EDI batch number for specified batch type |
| `spu_update_next_edi_batch_number` | `UpdateNextEDIBatchNumber` | Increments EDI batch number for specified batch type |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| (none) | — | Uses inline SQL; no other b* components |

---

### 37. bACTExportPFTrans
**Directory:** `ExportPFTrans/`
**Project:** `bACTExportPFTrans`
**Purpose:** Automated batch process that exports premium finance transaction data from EDI files. Reads EDI files from a configured directory, parses finance plan references and transaction details, enriches them with finance plan data from the database, and moves processed files to a dated archive directory.

**Business Methods — MainModule (bACTExportPFTrans.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Main` | `Public Sub Main()` | Entry point — calls Initialise, Process, then Terminate |
| `GetFinanceDataPos` | `Public Function GetFinanceDataPos(ByRef vFinanceDataPos(,) As Object) As Integer` | Gets finance data field position mappings from the database |
| `GetFinanceDetails` | `Public Function GetFinanceDetails(ByVal v_sFinancePlanRef As String, ByRef vFinanceDetails(,) As Object) As Integer` | Gets finance plan details for a given plan reference |

**Stored Procedures (bACTExportPFTransSQL.vb):**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_get_finance_data_positions` | `GetFinanceDataPos` | Gets field position mappings for finance data in EDI files |
| `spu_get_finance_plan_details` | `GetFinanceDetails` | Gets finance plan details by plan reference |
| `spu_get_next_edi_batch_number` | `Process` (internal) | Gets the next EDI batch sequence number |
| `spu_update_next_edi_batch_number` | `Process` (internal) | Updates the next EDI batch sequence number |
| *(inline SQL)* | `Initialise` | `SELECT user_id,language_id FROM PMUser where username = {username} and password = {password}` |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bPMLock` | `Initialise` | Locking mechanism to prevent concurrent Export runs |
| `bPMFunc` | `Initialise`, error handling | Logging and utility functions |

---

### 38. bACTFinanceSpoke
**Directory:** `FinanceSpoke/`
**Project:** `bACTFinanceSpoke`
**Purpose:** Central finance spoke component that manages all financial export and import operations via a HUB-spoke architecture. Routes export/re-export/import requests to specialized handler classes for auto-banking, credit control, electronic receipting, payment runs, recurring payments, rejections, general ledger, bank statements, cheque reminders, stale cheques, sweep balances, loyalty schemes, credit balances, chase cycles, 3rd party collections, and close batch operations.

**Business Methods — Business (bACTFinanceSpokeCls.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Export` | `Public Function Export(ByVal v_sInterfaceCode As String, ByRef r_sBatchRef As String, ByRef r_sStatusCode As String, ByRef r_sMessage As String, ByRef r_sHeaderXML As String, ByRef r_vHeaderData As Object, ByRef r_vDetailData As Object) As Integer` | Main export dispatcher — routes to the correct export handler based on interface code |
| `Export` | `Public Function Export(ByVal v_sInterfaceCode As String, ByRef r_sBatchRef As String, ByRef r_sStatusCode As String, ByRef r_sMessage As String, ByRef r_sHeaderXML As String, ByRef r_vHeaderData As Object, ByRef r_vDetailData As Object, ByVal bCreateBatch As Boolean) As Integer` | Overloaded export with batch creation control |
| `ReExport` | `Public Function ReExport(ByVal v_sInterfaceCode As String, ByVal v_sBatchRef As String, ByRef r_sStatusCode As String, ByRef r_sMessage As String, ByRef r_sHeaderXML As String, ByRef r_vHeaderData() As Object, ByRef r_vDetailData() As Object) As Integer` | Re-exports a previously exported batch |
| `Import` | `Public Function Import(ByVal v_sInterfaceCode As String, ByVal v_sBatchRef As String, ByRef r_sStatusCode As String, ByRef r_sMessage As String, ByVal v_sHeaderXML As String, ByRef r_vHeaderData() As Object, ByRef r_vDetailData() As Object) As Integer` | Main import dispatcher — routes to the correct import handler |
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard initialise with database setup |
| `CreateBatchRecord` | `Public Function CreateBatchRecord(Optional ByRef r_lBatchID As Integer = 0, Optional ByVal v_lBatchStatusID As Integer = 0, Optional ByVal v_lCompanyID As Integer = 0, Optional ByVal v_lUserID As Integer = 0, Optional ByVal v_sBatchRef As String = "", Optional ByVal v_dtCreatedDate As Date = #12/30/1899#, Optional ByVal v_dtAuthorisedDate As Date = #12/30/1899#, Optional ByVal v_dtAccountingDate As Date = #12/30/1899#, Optional ByVal v_sComment As String = "", Optional ByVal v_lBatchTypeID As Integer = 0, Optional ByVal v_lBatchSourceID As Integer = 0, Optional ByVal v_sXML As String = "", Optional ByVal v_dtExportDate As Date = #12/30/1899#, Optional ByVal v_dtReExportDate As Date = #12/30/1899#, Optional ByVal v_lMediaTypeID As Integer = 0, Optional ByVal v_cTotalAmount As Decimal = 0, Optional ByVal v_lTotalTransactions As Integer = 0, Optional ByVal v_dtImportedDate As Date = #12/30/1899#, Optional ByVal v_cRejectAmount As Decimal = 0, Optional ByVal v_lRejectTransactions As Integer = 0, Optional ByVal v_dtClosedDate As Date = #12/30/1899#, Optional ByVal v_sInterfaceCode As String = "", Optional ByVal v_iAutoClose As Integer = 0) As Integer` | Creates a batch record in the database |
| `UpdateBatchRef` | `Public Function UpdateBatchRef(ByVal v_lBatchID As Integer, ByVal v_sBatchRef As String) As Integer` | Updates the batch reference for a given batch ID |

**Business Methods — MainModule (bACTFinanceSpoke.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `AddHUBColumnsToDetailArray` | `Public Function AddHUBColumnsToDetailArray(ByVal v_sStatusCode As String, ByVal v_sStatusMsg As String, ByRef r_vResultArray As Array, ByVal v_sUsername As String) As Integer` | Adds 3 HUB-required columns (record no, status code, status msg) to the start of each row in a detail array |
| `AddResultArrayToDetailArray` | `Public Function AddResultArrayToDetailArray(ByRef v_vDetailArray As Object, ByRef r_vResultArray As Object, ByRef v_sUsername As String) As Integer` | Appends a result array as a new element in the detail array |

**Business Methods — Export Handlers:**

| Method | Class/File | Signature | Description |
|--------|------------|-----------|-------------|
| `Start` | ExportGeneralLedger.vb | `Public Function Start(ByVal v_sInterfaceCode As String, ByVal v_sBatchRef As String, ByRef r_sStatusCode As String, ByRef r_sMessage As String, ByVal v_sHeaderXML As String, ByRef r_vHeaderData As Object, ByRef r_vDetailData As Object) As Integer` | Executes the general ledger export process |
| `ProcessChaseCycleItem` | ExportChaseCycle.vb | `Public Function ProcessChaseCycleItem(ByRef r_vItemArray(,) As Object, ByVal i As Integer, ByRef r_vClientItemsArray() As Object, ByRef r_lClientItems As Integer, ByRef r_vAutoCancelItemsArray() As Object, ByRef r_lAutoCancelItems As Integer, ByRef r_vDeleteItemsArray() As Object, ByRef r_lDeleteItems As Integer) As Integer` | Processes a single chase cycle item |
| `UpdateChaseCycleItem` | ExportChaseCycle.vb | `Public Function UpdateChaseCycleItem(ByRef r_vItemArray(,) As Object, ByVal i As Integer, ByVal v_cAmountOwing As Decimal) As Integer` | Updates a chase cycle item with amount owing |
| `UpdateBatchTask` | ExportChaseCycle.vb | `Public Sub UpdateBatchTask(ByVal sBatchStatusCode As String, ByVal nBatchId As Integer, ByVal nTotal_Transactions As Integer, ByVal nReject_transactions As Integer)` | Updates batch task with status and transaction counts |
| `ProcessCreditControlItem` | ExportCreditControl.vb | `Public Function ProcessCreditControlItem(ByVal v_bInstalment As Boolean, ByRef r_vItemArray(,) As Object, ByVal i As Integer, ByRef r_vClientItemsArray() As Object, ByRef r_lClientItems As Integer, ByRef r_vOIPItemsArray() As Object, ByRef r_lOIPItems As Integer, ByRef r_vAutoCancelItemsArray() As Object, ByRef r_lAutoCancelItems As Integer, ByRef r_vDeleteItemsArray() As Object, ByRef r_lDeleteItems As Integer, ByRef r_vStopAccountArray() As Object, ByRef r_lStopAccountItems As Integer, ByRef r_vLapseRenewalArray() As Object, ByRef r_lLapseRenewalItems As Integer, Optional ByVal bAdvancedCCItemsForInstalments As Boolean = False) As Integer` | Processes a single credit control item |
| `UpdateCreditControlItem` | ExportCreditControl.vb | `Public Function UpdateCreditControlItem(ByRef r_vItemArray(,) As Object, ByVal i As Integer, ByVal v_cAmountOwing As Decimal) As Integer` | Updates a credit control item |
| `UpdateBatchTask` | ExportCreditControl.vb | `Public Sub UpdateBatchTask(ByVal sBatchStatusCode As String, ByVal nBatchId As Integer, ByVal nTotal_Transactions As Integer, ByVal nReject_transactions As Integer)` | Updates batch task for credit control |
| `RetrieveRecords` | ExportOneOffReceipts.vb | `Public Function RetrieveRecords(ByVal v_sWhereClause As String, ByRef r_vResults(,) As Object, ByVal v_bGroupRecords As Boolean) As Integer` | Retrieves one-off receipt records by where clause |
| `RetrieveRecords` | ExportRecurring.vb | `Public Function RetrieveRecords(ByVal v_sWhereClause As String, ByRef r_vResults(,) As Object, ByVal v_bGroupRecords As Boolean) As Integer` | Retrieves recurring transaction records by where clause |

**Business Methods — Import Handlers:**

| Method | Class/File | Signature | Description |
|--------|------------|-----------|-------------|
| `Start` | ImportElectronicReceipting.vb | `Public Function Start(ByVal v_sBatchRef As Integer, ByRef r_vHeaderData() As Object, ByRef r_vDetailData() As Object, ByVal v_sHeaderXML As String, ByRef r_sStatusCode As String, ByRef r_sMessage As String) As Integer` | Processes electronic receipting import |
| `Start` | ImportRejections.vb | `Public Function Start(ByRef r_vDetailData() As Object, ByRef r_vHeaderData() As Object, ByRef r_sStatusCode As String, ByRef r_sMessage As String, ByRef r_sBatchInterfaceCode As String, ByRef r_lBatchID As Integer, ByRef r_sInterfaceCode As String, ByRef r_sBatchRef As String) As Integer` | Processes import rejection records |

**Stored Procedures:**

| Stored Procedure | Called By / File | Purpose |
|-----------------|-----------|---------|
| `spu_ACT_Add_Batch` | `CreateBatchRecord` | Creates a new batch record |
| `spu_ACT_Update_Batch_Ref` | `UpdateBatchRef` | Updates batch reference |
| `spu_ACT_Spoke_Export3rdPartyTrans` | Export3rdPartyCollect.vb | Exports 3rd party collection transactions |
| `spu_ACT_Spoke_ExportAutoBank` | ExportAutoBank.vb, ImportBankStatement.vb | Exports auto-banking transactions |
| `spu_ACT_Spoke_ExportAutoBank_CloseCashLists` | ExportAutoBank.vb | Closes cash lists for auto-banking |
| `spu_ACT_Spoke_ExportChaseCycle` | ExportChaseCycle.vb | Exports chase cycle items |
| `spu_Create_Chase_Cycle_Batch` | ExportChaseCycle.vb | Creates a chase cycle batch |
| `spu_Update_BatchTask` | ExportChaseCycle.vb, ExportCreditControl.vb | Updates batch task status/counts |
| `spu_ACT_Spoke_Get_Cheque_Reminder` | ExportChequeReminder.vb | Gets cheque reminder data |
| `spu_ACT_Spoke_Update_Cheque_Reminder` | ExportChequeReminder.vb | Updates cheque reminder status |
| `spu_ACT_Spoke_Get_DocIDFromRef` | ExportChequeReminder.vb | Gets document ID from reference |
| `spu_ACT_Spoke_Get_DocTypeIDFromDocTypeRef` | ExportChequeReminder.vb | Gets document type ID from type reference |
| `spu_ACT_Spoke_Get_EventTypeIDFromCode` | ExportChequeReminder.vb | Gets event type ID from code |
| `spu_ACT_Spoke_ExportCreditBalance` | ExportCreditBalance.vb | Exports credit balance data |
| `spu_ACT_Spoke_CheckTaskExists` | ExportCreditBalance.vb | Checks if a task exists |
| `spu_ACT_Spoke_GetPMUserGroup` | ExportCreditBalance.vb | Gets PM user group info |
| `spu_ACT_Spoke_ExportCreditControl` | ExportCreditControl.vb | Exports credit control items |
| `spu_ACT_Get_Other_Interested_Parties` | ExportCreditControl.vb | Gets other interested parties for a policy |
| `spu_ACT_Get_PartyCnt_From_AccountID` | ExportCreditControl.vb | Gets party count from account ID |
| `spu_ACT_Update_Account_AccountStatus` | ExportCreditControl.vb | Updates account status |
| `spu_Create_CreditControl_Batch` | ExportCreditControl.vb | Creates credit control batch |
| `spu_Get_Renewal_Status_from_RenInsFileCnt` | ExportCreditControl.vb | Gets renewal status from renewal insurance file count |
| `spu_Lapsed_Reason_Sel_From_Code` | ExportCreditControl.vb | Selects lapse reason from code |
| `spu_ACT_Spoke_ExportExtractTrans` | ExportExtractTrans.vb, ExportOneOffReceipts.vb, ExportRecurring.vb, ImportCloseBatch.vb | Exports extracted transactions |
| `spu_ACT_general_ledger_sel` | ExportGeneralLedger.vb | Selects general ledger entries |
| `spu_ACT_general_ledger_group_sel` | ExportGeneralLedger.vb | Selects general ledger groups |
| `spu_ACT_general_ledger_group_add` | ExportGeneralLedger.vb | Adds general ledger group |
| `spu_ACT_general_ledger_group_del` | ExportGeneralLedger.vb | Deletes general ledger group |
| `spu_ACT_group_nodes_sel` | ExportGeneralLedger.vb | Selects group nodes |
| `spu_ACT_check_child_account_nodes_sel` | ExportGeneralLedger.vb | Checks child account nodes |
| `spu_ACT_max_min_document_id_sel` | ExportGeneralLedger.vb | Gets min/max document ID range |
| `spu_ACT_select_document` | ExportGeneralLedger.vb | Selects document details |
| `spu_ACT_Spoke_ExportPartyLoyaltyScheme` | ExportLoyaltyScheme.vb | Exports party loyalty scheme data |
| `spu_ACT_ProcessPaymentRun` | ExportPaymentRun.vb | Processes a payment run |
| `spu_ACT_PaymentRunType` | ExportPaymentRun.vb | Gets payment run type |
| `spu_ACT_Get_Document_Template_For_Payment` | ExportPaymentRun.vb | Gets document template for payment |
| `spu_ACT_Get_MediaID_From_code` | ExportPaymentRun.vb, ImportElectronicReceipting.vb | Gets media ID from media code |
| `spu_ACT_Select_MediaType` | ExportPaymentRun.vb | Selects media type |
| `spu_ACT_Select_Allocated_TransDetail_ID` | ExportPaymentRun.vb | Selects allocated transaction detail ID |
| `spu_ACT_Select_CashListItem` | ExportPaymentRun.vb, ImportRejections.vb | Selects cash list item details |
| `spu_ACT_Select_CashListItemID_From_TransDetailID` | ExportPaymentRun.vb | Gets cash list item ID from trans detail ID |
| `spu_report_select` | ExportPaymentRun.vb | Selects report data |
| `spu_ACT_Spoke_Get_Stale_Cheques` | ExportStaleCheques.vb | Gets stale cheque records |
| `spu_ACT_Spoke_Update_CashListItem_Stale` | ExportStaleCheques.vb | Updates cash list item as stale |
| `spu_ACT_Spoke_Get_AccountIDFromBankAccountID` | ExportStaleCheques.vb, ExportSweepBalances.vb | Gets account ID from bank account ID |
| `spu_ACT_Spoke_Get_Statement_Details` | ExportSweepBalances.vb | Gets bank statement details |
| `spu_ACT_Spoke_Insert_Bank_Rec_TD` | ExportSweepBalances.vb | Inserts bank reconciliation transaction detail |
| `spu_ACT_Spoke_UpdateMatchDate` | ExportSweepBalances.vb | Updates match date for bank reconciliation |
| `spu_ACT_Add_Bank_Statement_Header` | ImportBankStatement.vb | Adds bank statement header |
| `spu_ACT_Add_Bank_Statement_Detail` | ImportBankStatement.vb | Adds bank statement detail line |
| `spu_ACT_Get_Batch_Info` | ImportBankStatement.vb | Gets batch information |
| `spu_ACT_Select_Finance_Plan_From_Batch` | ImportCloseBatch.vb | Selects finance plans from a batch |
| `spu_ACT_Update_Instalment_Status` | ImportCloseBatch.vb | Updates instalment status |
| `spu_ACT_Check_Party_Exists` | ImportElectronicReceipting.vb | Checks if a party exists |
| `spu_ACT_Get_Account_Address` | ImportElectronicReceipting.vb | Gets account address |
| `spu_ACT_Get_AccountIDs_From_BatchSourceCode` | ImportElectronicReceipting.vb | Gets account IDs from batch source code |
| `spu_ACT_Get_Matching_Debits_For_Batch` | ImportElectronicReceipting.vb | Gets matching debits for a batch |
| `spu_ACT_Get_ReceiptID_From_Code` | ImportElectronicReceipting.vb | Gets receipt ID from code |
| `spu_ACT_Add_Batch_Rejection` | ImportRejections.vb | Adds a batch rejection record |
| `spu_ACT_Get_CashListItem_ReceiptType` | ImportRejections.vb | Gets cash list item receipt type |
| `spu_ACT_Get_Instalment_Collection_Details` | ImportRejections.vb | Gets instalment collection details |
| `spu_ACT_Get_TransDetailIDs_For_CashListItem` | ImportRejections.vb | Gets trans detail IDs for a cash list item |
| `spu_ACT_Update_Batch_For_Rejection` | ImportRejections.vb | Updates batch record for rejection |
| `spu_ACT_Update_Instalment_Due_Date` | ImportRejections.vb | Updates instalment due date |
| `spu_ACT_Update_Instalment_Failure_Reason` | ImportRejections.vb | Updates instalment failure reason |
| `spu_ACT_Update_Reverse_CashListItem` | ImportRejections.vb | Reverses a cash list item |
| `spu_PFPremiumFinance_UpdateStatus_By_TransDetail_ID` | ImportRejections.vb | Updates premium finance status by transaction detail ID |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTAccount` | Various export/import handlers | Account operations |
| `bACTAllocate` | ImportElectronicReceipting, ImportRejections | Allocation operations |
| `bACTAllocationManual` | ImportElectronicReceipting | Manual allocation |
| `bACTAllocationPost` | ImportElectronicReceipting, ImportRejections | Posting allocations |
| `bACTAutoNumber` | ExportAutoBank | Auto-numbering |
| `bACTBankAccountReconcile` | ExportSweepBalances | Bank account reconciliation |
| `bACTCashList` | Various export/import handlers | Cash list management |
| `bACTCashListItem` | Various export/import handlers | Cash list item operations |
| `bACTCashListPost` | ImportElectronicReceipting, ImportRejections | Posting cash list items |
| `bACTCreditControl` | ExportCreditControl | Credit control operations |
| `bACTCreditControlItem` | ExportCreditControl | Credit control item operations |
| `bACTCurrencyConvert` | ExportPaymentRun | Currency conversion |
| `bACTDocumentPost` | ExportPaymentRun | Document posting |
| `bACTDocumentReversal` | ImportRejections | Document reversals |
| `bACTFindTransaction` | ExportPaymentRun | Finding transactions |
| `bSIRChaseCycle` | ExportChaseCycle | Chase cycle management |
| `bSIRDocManagerWrapper` | ExportChequeReminder | Document manager wrapper |
| `bSIREvent` | ExportChequeReminder, ExportCreditControl | Event creation/management |
| `bSIROptions` | Various handlers | System option lookups |
| `bSIRRenewal` | ExportCreditControl | Renewal processing (lapse renewals) |
| `bSIRReportPrint` | ExportPaymentRun | Report printing |

---

### 39. bACTFindAccount
**Directory:** `FindAccount/`
**Project:** `bACTFindAccount`
**Purpose:** Search component for finding accounts in the Orion accounting system. Provides multiple search methods including standard query, group-level security-filtered query, ledger lookups, account ID resolution, full key path retrieval, and account status checking.

**Business Methods — Business (bACTFindAccountCls.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard initialise with database and component setup |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process modes |
| `GetLookupValues` | `Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vresultArray(,) As Object) As Integer` | Gets lookup values via bPMLookup |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Navigator SetKeys function |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray As String) As Integer` | Navigator GetKeys (string overload) |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Navigator GetKeys (array overload) |
| `GetSummary` | `Public Function GetSummary(ByRef vSummaryArray As Object) As Integer` | Navigator GetSummary function |
| `Start` | `Public Function Start() As Integer` | Navigator Start entry point |
| `SelectAccountQuery` | `Public Function SelectAccountQuery(ByRef lNumberOfRecords As Integer, ByRef vresultArray(,) As Object, ByVal iCompanyID As Integer, Optional ByVal vFullKey As Object = Nothing, Optional ByVal vLedgerID As Object = Nothing, Optional ByVal vAccountName As Object = Nothing, Optional ByVal vAccountType As Object = Nothing, Optional ByVal vShortCode As Object = Nothing, Optional ByVal vInsuranceRef As Object = Nothing, Optional ByVal vOperatorID As Object = Nothing, Optional ByVal vPurchaseOrderNo As Object = Nothing, Optional ByVal vPurchaseInvoiceNo As Object = Nothing, Optional ByVal vSpare As Object = Nothing, Optional ByVal vShowDeleted As Object = Nothing, Optional ByVal vPaymentLedgerIDs As Object = Nothing, Optional ByVal vExcludeLedgerIDs As Object = Nothing, Optional ByVal vShowBalance As Object = Nothing, Optional ByVal vBrokerCnt As Object = Nothing) As Integer` | Searches accounts by multiple criteria with inline SQL |
| `SelectAccountQueryFiltered` | `Public Function SelectAccountQueryFiltered(ByRef lNumberOfRecords As Integer, ByRef vresultArray(,) As Object, ByVal iCompanyID As Integer, Optional ByVal vFullKey As Object = Nothing, Optional ByVal vLedgerID As Object = Nothing, Optional ByVal vAccountName As Object = Nothing, Optional ByVal vAccountType As Object = Nothing, Optional ByVal vShortCode As Object = Nothing, Optional ByVal vInsuranceRef As Object = Nothing, Optional ByVal vOperatorID As Object = Nothing, Optional ByVal vPurchaseOrderNo As Object = Nothing, Optional ByVal vPurchaseInvoiceNo As Object = Nothing, Optional ByVal vSpare As Object = Nothing, Optional ByVal vShowDeleted As Object = Nothing, Optional ByVal v_bOnlyUpdatable As Boolean = False, Optional ByVal vPaymentLedgerIDs As Object = Nothing, Optional ByVal vExcludeLedgerIDs As Object = Nothing) As Integer` | Searches accounts with group-level security filtering |
| `GetUserAuthorities` | `Public Function GetUserAuthorities() As Integer` | Gets user's enquiry/update authority levels |
| `GetLedgersQuery` | `Public Function GetLedgersQuery(ByRef lNumberOfRecords As Integer, ByRef vresultArray(,) As Object, ByVal vCompanyId As Object) As Integer` | Gets all ledgers for a company |
| `GetFullKey` | `Public Function GetFullKey(ByVal v_lAccountID As Integer, ByRef r_sFullKey As String) As Integer` | Gets the full key path for an account ID |
| `GetID` | `Public Function GetID(ByRef lAccountID As Integer, ByVal iCompanyID As Integer, Optional ByVal vLedgerID As Object = Nothing, Optional ByVal vFullKey As Object = Nothing, Optional ByVal vShortCode As Object = Nothing) As Integer` | Gets account ID from given parameters |
| `GetAccountStatus` | `Public Function GetAccountStatus(ByVal v_lAccountID As Integer, ByRef r_iAccountStatus As Integer, ByRef r_bIsStopped As Boolean) As Integer` | Gets account status and stopped flag (delegates to bACTAccount) |
| `GetPaymentLedgers` | `Public Function GetPaymentLedgers(ByRef r_sPaymentLedgersString As String) As Integer` | Gets payment ledger IDs as comma-separated string |

**Business Methods — NavigatorV3 (bNavigatorV3.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ...) As Integer` | Initialises NavigatorV3 and creates Business instance |
| `NavigatorV3_SetKeys` | `Public Function NavigatorV3_SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Navigator SetKeys delegate |
| `NavigatorV3_GetKeys` | `Public Function NavigatorV3_GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Navigator GetKeys delegate |
| `NavigatorV3_GetSummary` | `Public Function NavigatorV3_GetSummary(ByRef vSummaryArray As Object) As Integer` | Navigator GetSummary delegate |
| `NavigatorV3_SetProcessModes` | `Public Function NavigatorV3_SetProcessModes(...) As Integer` | Navigator SetProcessModes delegate |
| `NavigatorV3_Start` | `Public Function NavigatorV3_Start() As Integer` | Navigator Start delegate |

**Stored Procedures (bACTFindAccountSql.vb):**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_ACT_Do_FindAccount` | `SelectAccountQuery` (referenced but inline SQL is used instead) | Finds accounts by query parameters |
| `spu_ACT_Do_GetAccountId` | `GetID` | Gets account ID from company/ledger/short code/full key |
| `spu_ACT_Do_SelectLedgers` | `GetLedgersQuery` | Selects all ledgers for a company |
| `spu_ACT_Select_Full_Path` | `GetFullKey` | Gets the full path for an account ID |
| `spu_ACT_PaymentLedgers` | `GetPaymentLedgers` | Gets payment ledger IDs for a source |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTAccount` | `Initialise`, `GetAccountStatus`, `SelectAccountQuery` | Account balance retrieval and status checking |
| `bPMLookup` | `Initialise`, `GetLookupValues` | Lookup value management |
| `bACTExplorer` | `Initialise` (FindBank variant) | Account explorer for tree navigation |

---

### 40. bACTFindBank
**Directory:** `FindBank/`
**Project:** `bACTFindBank`
**Purpose:** Search component for finding bank accounts in the Orion accounting system. Provides bank search by short code and bank name with wildcard support.

**Business Methods — Business (bACTFindBankCls.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard initialise with database and component setup |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process modes |
| `GetLookupValues` | `Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray As Object, ByRef iLanguageID As Integer, ByRef vResultArray As Object) As Integer` | Gets lookup values via bPMLookup |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Navigator SetKeys function |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray As String) As Integer` | Navigator GetKeys (string overload) |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Navigator GetKeys (array overload) |
| `Start` | `Public Function Start() As Integer` | Navigator Start entry point |
| `SelectBankQuery` | `Public Function SelectBankQuery(ByRef lNumberOfRecords As Integer, ByRef vResultArray(,) As Object, Optional ByVal vShortCode As String = "", Optional ByVal vBankName As String = "") As gPMConstants.PMEReturnCode` | Searches banks by short code and/or bank name with wildcard support |

**Business Methods — NavigatorV3 (bNavigatorV3.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ...) As Integer` | Initialises NavigatorV3 and creates Business instance |
| `NavigatorV3_SetKeys` | `Public Function NavigatorV3_SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Navigator SetKeys delegate |
| `NavigatorV3_GetKeys` | `Public Function NavigatorV3_GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Navigator GetKeys delegate |
| `NavigatorV3_GetSummary` | `Public Function NavigatorV3_GetSummary(ByRef vSummaryArray As Object) As Integer` | Navigator GetSummary delegate |
| `NavigatorV3_SetProcessModes` | `Public Function NavigatorV3_SetProcessModes(...) As Integer` | Navigator SetProcessModes delegate |
| `NavigatorV3_Start` | `Public Function NavigatorV3_Start() As Integer` | Navigator Start delegate |

**Stored Procedures (bACTFindBankSql.vb):**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_ACT_Do_FindBank` | `SelectBankQuery` | Finds bank accounts by short code, bank name, and company ID |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bPMLookup` | `Initialise`, `GetLookupValues` | Lookup value management |
| `bACTExplorer` | `Initialise` | Account explorer for navigation |

---

### 41. bACTFindBudget
**Directory:** `FindBudget/`
**Project:** `bACTFindBudget`
**Purpose:** Budget management component providing CRUD operations (add, update, delete) for budget records, search by query (reference, year, status), collection-based record navigation, and lookup value retrieval for posting statuses.

**Business Methods — Form (bACTFindBudgetForm.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard initialise with database and collection setup |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process modes |
| `DirectAdd` | `Public Function DirectAdd(Optional ByRef vBudgetID As Integer = 0, Optional ByRef vBudgetRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vBudgetDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesBudgetID As Object = Nothing, Optional ByRef vBudgetStatusID As Object = Nothing) As Integer` | Adds a budget record directly to the database |
| `DirectDelete` | `Public Function DirectDelete(Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vBudgetDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesBudgetID As Object = Nothing, Optional ByRef vBudgetStatusID As Object = Nothing) As Integer` | Deletes a budget record directly from the database |
| `GetDefaults` | `Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vBudgetDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesBudgetID As Object = Nothing, Optional ByRef vBudgetStatusID As Object = Nothing) As Integer` | Returns default values for budget fields |
| `CheckID` | `Public Function CheckID(ByRef vID As Object) As Integer` | Checks if a budget ID exists |
| `GetCaptions` | `Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, Optional ByRef vTable As Object = Nothing) As Integer` | Gets caption field values for a budget record |
| `GetDetails` | `Public Function GetDetails(Optional ByRef vACTFindBudgetID As Object = Nothing, Optional ByRef vLockMode As gPMConstants.PMELockMode = 0) As Integer` | Loads budget records into the collection (single or all) |
| `GetBudgetRef` | `Public Function GetBudgetRef(ByVal v_lBudgetID As Integer, ByRef r_sReference As String) As Integer` | Gets the reference string for a budget ID |
| `GetLookupValues` | `Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer` | Gets lookup values for posting status |
| `GetNext` | `Public Function GetNext(Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vBudgetDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesBudgetID As Object = Nothing, Optional ByRef vBudgetStatusID As Object = Nothing) As Integer` | Gets the next budget from the collection |
| `EditAdd` | `Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vBudgetDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesBudgetID As Object = Nothing, Optional ByRef vBudgetStatusID As Object = Nothing) As Integer` | Adds a budget to the collection |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vBudgetID As Object = Nothing, Optional ByRef vBudgetRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vBudgetDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesBudgetID As Object = Nothing, Optional ByRef vBudgetStatusID As Object = Nothing) As Integer` | Updates a budget in the collection |
| `EditDelete` | `Public Function EditDelete(ByVal lRow As Integer) As Integer` | Marks a budget in the collection for deletion |
| `Cancel` | `Public Function Cancel() As Integer` | Cancels pending changes |
| `Update` | `Public Function Update() As Integer` | Commits all collection changes to the database |
| `SearchByQuery` | `Public Function SearchByQuery(ByRef lNumberOfRecords As Integer, ByRef vResultArray(,) As Object, Optional ByVal v_vReference As String = "", Optional ByVal v_vYear As String = "", Optional ByVal v_vStatus As Double = 0) As gPMConstants.PMEReturnCode` | Searches budgets by reference, year, and/or status (inline SQL) |

**Business Methods — Automated (bACTFindBudgetAutomated.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises the automated class |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, ...) As Integer` | Sets optional process modes for automated processing |
| `Start` | `Public Function Start() As Integer` | Automated action entry point |

**Business Methods — ACTFindBudget (bACTFindBudget.vb — data class):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise() As Integer` | Initialises a single budget data object |

**Business Methods — ACTFindBudgets (bACTFindBudgets.vb — collection class):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Add` | `Public Function Add(ByRef oNewACTFindBudget As bACTFindBudget.ACTFindBudget) As Integer` | Adds a budget to the collection |
| `Count` | `Public Function Count() As Integer` | Returns collection count |
| `Delete` | `Public Sub Delete(ByRef vKey As Integer)` | Removes a budget from the collection by key |
| `Item` | `Public Function Item(ByRef vKey As Integer) As bACTFindBudget.ACTFindBudget` | Returns a budget from collection by key |
| `DeleteAll` | `Public Sub DeleteAll()` | Deletes all budgets from collection |

**Stored Procedures (bACTFindBudgetFormSQL.vb / bACTFindBudgetAutomatedSQL.vb):**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_ACT_select_{NewTable}` | `GetDetails` (Form), Automated | Selects a single budget record (template placeholder) |
| `spu_ACT_select_all_{NewTable}` | `GetDetails` (Form), Automated | Selects all budget records (template placeholder) |
| `spu_ACT_check_{NewTable}` | `CheckID` (Form), Automated | Checks if a budget ID exists (template placeholder) |
| `spu_ACT_add_{NewTable}` | `DirectAdd`/`Update` (Form), Automated | Adds a budget record (template placeholder) |
| `spu_ACT_delete_{NewTable}` | `DirectDelete`/`Update` (Form), Automated | Deletes a budget record (template placeholder) |
| `spu_ACT_update_{NewTable}` | `Update` (Form), Automated | Updates a budget record (template placeholder) |

> **Note:** The SP names contain `{NewTable}` placeholder — this appears to be a template component that was not fully customised. The `SearchByQuery` method uses inline SQL against the `Budget` table directly.

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bPMLookup` | `Initialise` (Form), `GetLookupValues` | Lookup value management (posting status) |

---

### 42. bACTFindCashList
**Directory:** `FindCashList/`
**Project:** `bACTFindCashList`
**Purpose:** Search component for finding cash lists in the Orion accounting system. Provides parameterised search by company, status, type, reference, bank account, currency, date range, control total, and item count.

**Business Methods — Form (bACTFindCashList.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard initialise with database and lookup setup |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTypeOfBusiness As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process modes |
| `GetLookupValues` | `Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer` | Gets lookup values via bPMLookup |
| `SearchDetails` | `Public Function SearchDetails(ByRef lNumberOfRecords As Integer, ByRef vResultArray(,) As Object, ByVal iCompanyID As Integer, Optional ByVal vCashListStatusID As Integer = 0, Optional ByVal vCashListTypeID As Integer = 0, Optional ByVal vCashListRef As String = "", Optional ByVal vBankAccountID As Integer = 0, Optional ByVal vCurrencyID As Integer = 0, Optional ByVal vStartDate As Byte = 0, Optional ByVal vEndDate As Byte = 0, Optional ByVal vControlTotal As Integer = 0, Optional ByVal vItemCount As Integer = 0) As Integer` | Searches cash lists by multiple criteria via stored procedure |
| `GetID` | `Public Function GetID(...) As Integer` | Returns cash list ID from search parameters |

**Stored Procedures (bACTFindCashListSql.vb):**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_ACT_Do_FindCashList` | `SearchDetails` | Finds cash lists by status, type, reference, bank account, currency, date range, control total, and item count |
| `spu_ACT_Do_GetCashListId` | `GetID` | Returns cash list ID from parameters — commented out |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bPMLookup` | `Initialise`, `GetLookupValues` | Lookup value management |

---

### 43. bACTFindCashListItem
**Directory:** `FindCashListItem/`
**Project:** `bACTFindCashListItem`
**Purpose:** Search and retrieve cash list items (receipts and payments) based on various filter criteria such as date range, account, media reference, amounts, and batch numbers.

**Business Methods — Form (bACTFindCashListItemCls.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialise component with credentials and database |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTypeOfBusiness As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set optional process modes (task, navigate, process mode, type of business, effective date) |
| `GetLookupValues` | `Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As String) As Integer` | Gets lookup values as defined by vTableArray via bPMLookup |
| `SearchReceiptDetails` | `Public Function SearchReceiptDetails(ByRef r_lNumberOfRecords As Integer, ByRef r_vResultArray(,) As Object, Optional ByVal v_vStartDate As Object = Nothing, Optional ByVal v_vEndDate As Object = Nothing, Optional ByVal v_vReceiptTypeId As Object = Nothing, Optional ByVal v_vAccount As Object = Nothing, Optional ByVal v_vMediaReference As Object = Nothing, Optional ByVal v_vTheirReference As Object = Nothing, Optional ByVal v_vAmount As Object = Nothing, Optional ByVal v_vBatchNumber As Object = Nothing, Optional ByVal v_vMediaTypeId As Object = Nothing, Optional ByVal v_vReceiptNumber As Object = Nothing, Optional ByVal v_vBatchReference As Object = Nothing) As Integer` | Search receipt cash list items by date range, receipt type, account, media reference, amount, batch, media type, receipt number, batch reference |
| `SearchPaymentDetails` | `Public Function SearchPaymentDetails(ByRef r_lNumberOfRecords As Object, ByRef r_vResultArray(,) As Object, Optional ByVal v_vPayeeName As Object = Nothing, Optional ByVal v_vAccountID As Object = Nothing, Optional ByVal v_vPaymentTypeID As Object = Nothing, Optional ByVal v_vPaymentMediaTypeID As Object = Nothing, Optional ByVal v_vChequeEFTNo As Object = Nothing, Optional ByVal v_vPaymentStatusID As Object = Nothing, Optional ByVal v_vAmount As Object = Nothing, Optional ByVal v_vBatchNumber As Object = Nothing, Optional ByVal v_vBatchReference As Object = Nothing, Optional ByVal v_vBranchID As Object = Nothing, Optional ByVal v_vBankAccountID As Object = Nothing, Optional ByVal v_vClientCode As Object = Nothing, Optional ByVal v_vClientAccountNumber As Object = Nothing, Optional ByVal v_vPolicyClaimNumber As Object = Nothing, Optional ByVal v_vMediaFrom As Object = Nothing, Optional ByVal v_vMediaTo As Object = Nothing, Optional ByVal v_vAmountFrom As Object = Nothing, Optional ByVal v_vAmountTo As Object = Nothing, Optional ByVal v_vStartDate As Object = Nothing, Optional ByVal v_vEndDate As Object = Nothing, Optional ByVal v_vShowOnlyOutStanding As Object = Nothing, Optional ByVal v_vUserID As Object = Nothing) As Integer` | Search payment cash list items by payee name, account, payment type, media type, cheque/EFT number, status, amount range, batch, branch, bank account, client code, policy/claim number, date range |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Clean up resources |
| `GetID` | `Public Function GetID(lID As Long, Optional ByVal vName As Variant, Optional ByVal vShortName As Variant, Optional ByVal vSourceId As Variant) As Long` | Retrieves document ID by name or short name — commented out in current implementation |
| `GetName` | `Public Function GetName(lDocumentCnt As Long, sDocumentName As String) As Long` | Retrieves document description/shortname using document_cnt — commented out in current implementation |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_ACT_Do_FindReceipt` | `SearchReceiptDetails` | Find receipt cash list items by search criteria |
| `spu_ACT_Do_FindPayment` | `SearchPaymentDetails` | Find payment cash list items by search criteria |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bPMLookup.Business` | `Initialise`, `GetLookupValues` | Lookup value retrieval |

---

### 44. bACTFindDocument
**Directory:** `FindDocument/`
**Project:** `bACTFindDocument`
**Purpose:** Search and retrieve accounting documents by document reference (wildcard), document type, posting status, date range, and source company. Provides lookup values for document types and posting statuses.

**Business Methods — Form (bACTFindDocumentCls.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialise component with credentials and database |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set optional process modes |
| `GetLookupValues` | `Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer` | Gets lookup values for DocumentType and PostingStatus via bPMLookup |
| `SearchLikeDocumentRef` | `Public Function SearchLikeDocumentRef(ByRef sDocumentRef As String, ByRef lNumberOfRecords As Integer, ByRef vResultArray(,) As Object) As Integer` | Search documents by document reference with wildcard matching |
| `SearchByQuery` | `Public Function SearchByQuery(ByRef lNumberOfRecords As Integer, ByRef vResultArray(,) As Object, Optional ByVal vDocumentRef As String = "", Optional ByVal vDateFrom As Integer = 0, Optional ByVal vDateTo As Integer = 0, Optional ByVal vDocumentType As Integer = 0, Optional ByVal vPostingStatus As Integer = 0, Optional ByVal vSourceArray As Object = Nothing) As Integer` | Search documents by query-by-example parameters (ref, date range, document type, posting status, source) |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Clean up resources |

**SQL Constants (bACTFindDocumentSql.vb):**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_ACT_Do_FindDocLikeDocRef` | `SearchLikeDocumentRef` | Find documents matching document reference pattern |
| *(embedded SQL)* | `SearchByQuery` | Dynamic SQL query built from filter parameters (not a stored procedure) |
| `spu_ACT_Do_GetDocRefFromId` | `GetName` | Returns document reference/shortname given document ID |
| `spu_ACT_GetIdFromDocRef` | `GetID` | Returns document ID given document reference or short name |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bPMLookup.Business` | `Initialise`, `GetLookupValues` | Lookup value retrieval for DocumentType and PostingStatus |

---

### 45. bACTFindInvoice
**Directory:** `FindInvoice/`
**Project:** `bACTFindInvoice`
**Purpose:** CRUD operations and search for accounting invoices. Provides full form management (add, update, delete, navigate), automated processing, and search-by-query capabilities for invoices by reference, year, and status.

**Business Methods — Form (bACTFindInvoiceForm.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialise component with credentials and database |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set optional process modes |
| `DirectAdd` | `Public Function DirectAdd(Optional ByRef vInvoiceID As Integer = 0, Optional ByRef vInvoiceRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vInvoiceDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesInvoiceID As Object = Nothing, Optional ByRef vInvoiceStatusID As Object = Nothing) As Integer` | Add a single invoice directly to the database |
| `DirectDelete` | `Public Function DirectDelete(Optional ByRef vInvoiceID As Object = Nothing, Optional ByRef vInvoiceRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vInvoiceDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesInvoiceID As Object = Nothing, Optional ByRef vInvoiceStatusID As Object = Nothing) As Integer` | Delete a single invoice directly from the database |
| `GetDefaults` | `Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vInvoiceID As Object = Nothing, Optional ByRef vInvoiceRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vInvoiceDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesInvoiceID As Object = Nothing, Optional ByRef vInvoiceStatusID As Object = Nothing) As Integer` | Get default values for invoice fields |
| `CheckID` | `Public Function CheckID(ByRef vID As Object) As Integer` | Check if an invoice ID exists |
| `GetCaptions` | `Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, Optional ByRef vTable As Object = Nothing) As Integer` | Get field captions for invoice form |
| `GetDetails` | `Public Function GetDetails(Optional ByRef vACTFindInvoiceID As Object = Nothing, Optional ByRef vLockMode As gPMConstants.PMELockMode = 0) As Integer` | Get invoice details by ID with optional lock |
| `GetInvoiceRef` | `Public Function GetInvoiceRef(ByVal v_lInvoiceID As Integer, ByRef r_sReference As String) As Integer` | Get invoice reference string by invoice ID |
| `GetLookupValues` | `Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer` | Gets lookup values for invoice types |
| `GetNext` | `Public Function GetNext(Optional ByRef vInvoiceID As Object = Nothing, Optional ByRef vInvoiceRef As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vInvoiceDescription As Object = Nothing, Optional ByRef vPeriodYearName As Object = Nothing, Optional ByRef vRevisesInvoiceID As Object = Nothing, Optional ByRef vInvoiceStatusID As Object = Nothing) As Integer` | Navigate to next invoice in collection |
| `EditAdd` | `Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vInvoiceID As Object = Nothing, ...) As Integer` | Add invoice to collection for batch editing |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vInvoiceID As Object = Nothing, ...) As Integer` | Update invoice in collection for batch editing |
| `EditDelete` | `Public Function EditDelete(ByVal lRow As Integer) As Integer` | Delete invoice from collection |
| `Cancel` | `Public Function Cancel() As Integer` | Cancel pending changes |
| `Update` | `Public Function Update() As Integer` | Commit pending changes to database |
| `SearchByQuery` | `Public Function SearchByQuery(ByRef lNumberOfRecords As Integer, ByRef vResultArray(,) As Object, Optional ByVal v_vReference As String = "", Optional ByVal v_vYear As Object = Nothing, Optional ByVal v_vStatus As Object = Nothing) As gPMConstants.PMEReturnCode` | Search invoices by reference, year, and status |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Clean up resources |

**Data Object — ACTFindInvoice (bACTFindInvoice.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUserName As String, ...) As Integer` | Initialise data object |

**Collection — ACTFindInvoices (bACTFindInvoices.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Add` | `Public Function Add(ByRef oNewACTFindInvoice As bACTFindInvoice.ACTFindInvoice) As Integer` | Add invoice to collection |
| `Count` | `Public Function Count() As Integer` | Return count of invoices |
| `Delete` | `Public Sub Delete(ByRef vKey As Integer)` | Delete invoice from collection by key |
| `Item` | `Public Function Item(ByRef vKey As Integer) As bACTFindInvoice.ACTFindInvoice` | Get invoice from collection by key |
| `DeleteAll` | `Public Sub DeleteAll()` | Delete all invoices from collection |

**Automated — Automated (bACTFindInvoiceAutomated.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ...) As Integer` | Initialise automated component |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, ...) As Integer` | Set process modes for automated actions |
| `Start` | `Public Function Start() As Integer` | Execute automated action based on task/process mode |

**Stored Procedures (bACTFindInvoiceFormSQL.vb / bACTFindInvoiceAutomatedSQL.vb):**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_ACT_select_{NewTable}` | `GetDetails` | Select invoice details (template SP name) |
| `spu_ACT_select_all_{NewTable}` | `GetAllDetails` | Select all invoices (template SP name) |
| `spu_ACT_check_{NewTable}` | `CheckID` | Check if invoice ID exists (template SP name) |
| `spu_ACT_add_{NewTable}` | `DirectAdd` | Add new invoice (template SP name) |
| `spu_ACT_delete_{NewTable}` | `DirectDelete` | Delete invoice (template SP name) |
| `spu_ACT_update_{NewTable}` | `Update` | Update invoice (template SP name) |

> **Note:** The SQL constants contain `{NewTable}` placeholders — these are template-generated SP names that should be replaced with the actual table name at deployment.

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bPMLookup.Business` | `Initialise`, `GetLookupValues` | Lookup value retrieval |
| `bACTFindInvoice.ACTFindInvoice` | `Form` | Invoice data transfer object |
| `bACTFindInvoice.ACTFindInvoices` | `Form` | Invoice collection management |

---

### 46. bACTFindTransaction
**Directory:** `FindTransaction/`
**Project:** `bACTFindTransaction`
**Purpose:** Comprehensive transaction enquiry component. Builds and executes complex dynamic SQL queries to search, filter, and display accounting transactions (bills). Supports transaction rollup, allocation viewing, account lookups, period navigation, policy/premium finance balance queries, audit sets, write-offs, transfers, and work manager integration.

**Business Methods — Business (bACTFindTransactionCls.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialise with credentials, database, and create dependent business objects |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Navigator SetKeys function |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Navigator GetKeys function |
| `GetSummary` | `Public Function GetSummary(ByRef vSummaryArray As Object) As Integer` | Navigator GetSummary function |
| `Start` | `Public Function Start() As Integer` | Navigator Start entry point |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTypeOfBusiness As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set optional process modes |
| `GetLookupValues` | `Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer` | Gets lookup values via bPMLookup |
| `SelectTransQuery` | `Public Function SelectTransQuery(ByRef r_lNumberOfRecords As Integer, ByRef r_vResultArray(,) As Object, ByVal v_iCompanyID As Integer, ...[36 optional params]) As Integer` | Main transaction query — builds dynamic SQL with extensive filtering (overload without agent) |
| `SelectTransQuery` | `Public Function SelectTransQuery(ByRef r_lNumberOfRecords As Integer, ByRef r_vResultArray(,) As Object, ByVal v_iCompanyID As Integer, ByVal v_iAgentCnt As Integer, ...[36 optional params]) As Integer` | Main transaction query — builds dynamic SQL with agent filtering |
| `SelectTransQueryFiltered` | `Public Function SelectTransQueryFiltered(ByRef r_lNumberOfRecords As Integer, ByRef r_vResultArray(,) As Object, ByVal v_iCompanyID As Integer, ...) As Integer` | Filtered transaction query (overload without agent) |
| `SelectTransQueryFiltered` | `Public Function SelectTransQueryFiltered(ByRef r_lNumberOfRecords As Integer, ByRef r_vResultArray(,) As Object, ByVal v_iCompanyID As Integer, ByVal v_iAgentCnt As Integer, ...) As Integer` | Filtered transaction query with agent |
| `GetAllocationDetails` | `Public Function GetAllocationDetails(ByVal v_lAllocationId As Integer) As Integer` | Get allocation details by allocation ID |
| `IsTransInAllocation` | `Public Function IsTransInAllocation(ByVal v_lTransactionId As Integer) As Integer` | Check if transaction is in an allocation |
| `GetPeriodForDate` | `Public Function GetPeriodForDate(ByRef dtDateInPeriod As Date, ByRef lPeriodId As Integer, ByRef vYearName As Object) As Integer` | Get accounting period for a given date |
| `GetDetails` | `Public Function GetDetails(ByRef vYearName As Object) As Integer` | Get period details |
| `GetNext` | `Public Function GetNext(ByRef vPeriodId As Object, ByRef vCompanyID As Object, ByRef vYearName As Object, ByRef vPeriodName As Object, ByRef vPeriodEndDate As Object) As Integer` | Navigate to next period |
| `GetAccountDetails` | `Public Function GetAccountDetails(ByRef r_lAccountID As Integer, ByRef sAccountName As String, ByRef sContactName As String, ByRef sPhoneAreaCode As String, ByRef sPhoneNumber As String, ByRef sPhoneExtension As String, ByRef r_vdAccountBalance As Object, ByRef r_sAccountCode As String, ByRef r_vlAccountCurrencyId As Integer, ...) As Integer` | Get full account details including balance, contact info, ledger |
| `GetAccountID` | `Public Function GetAccountID(ByVal v_sShortCode As String, ByRef r_lAccountID As Integer, Optional ByVal sLedgerCode As String = "") As Integer` | Get account ID from short code |
| `GetFinancePlanDetails` | `Public Function GetFinancePlanDetails(ByVal v_sDocumentReference As String, ByVal v_lCompanyID As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Get finance plan details for a document |
| `GetAccountAmounts` | `Public Function GetAccountAmounts(ByVal v_lAccountID As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Get credit amounts for an account (Manage Debtors) |
| `GetAccountCodeFromID` | `Public Function GetAccountCodeFromID(ByRef r_sShortCode As String, ByVal v_lAccountID As Integer) As Integer` | Get account short code from account ID |
| `GetAccountKeyFromId` | `Public Function GetAccountKeyFromId(ByRef r_lAccountKey As Integer, ByVal v_lAccountID As Integer) As Integer` | Get account key from account ID |
| `ViewAllocationDetails` | `Public Function ViewAllocationDetails(ByVal v_lTransDetailId As Integer, ByRef r_vResultArray(,) As Object) As Integer` | View allocation details for a transaction |
| `GetRollupTransactions` | `Public Function GetRollupTransactions(ByVal v_lDocumentID As Integer, ByVal v_lAccountID As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Get rolled-up transactions for document/account |
| `GetRegSettings` | `Public Function GetRegSettings(ByRef sResult As String, ByRef sAppName As String, ByRef sSection As String, ByRef sKey As String, ByRef vDefault As Object) As Integer` | Get registry settings |
| `AddAuditSet` | `Public Function AddAuditSet(ByVal v_lDocumentID As Integer, ByVal v_lAuditSetID As Integer, Optional ByVal v_lCashListItemID As Integer = 0, Optional ByRef r_lAuditSetID As Integer = 0) As Integer` | Add audit set to document |
| `AddTaskToWorkManager` | `Public Function AddTaskToWorkManager(ByRef r_lPMWrkTaskInstanceCnt As Integer, ByVal v_sCustomer As String, ByVal v_sDescription As String, ByVal v_dtTaskDueDate As Date, ...) As Integer` | Create work manager task |
| `AddTransferDocument` | `Public Function AddTransferDocument(ByVal v_crTransferAmount As Decimal, ByVal v_iCompanyID As Integer, ByVal v_lOriginatingAccount As Integer, ByRef r_lDocumentID As Integer) As Integer` | Add accounting transfer document |
| `DeleteTransferDocument` | `Public Function DeleteTransferDocument(ByVal v_lDocumentID As Integer) As Integer` | Delete transfer document |
| `UpdateUserProperty` | `Public Function UpdateUserProperty(ByVal v_sPropertyName As String, ByVal v_vPropertyValue As Object) As Integer` | Update user property setting |
| `IsInsurer` | `Public Function IsInsurer(ByVal v_lAccountID As Integer) As Boolean` | Check if account is insurer type |
| `IsAgent` | `Public Function IsAgent(ByVal v_lAccountID As Integer) As Boolean` | Check if account is agent type |
| `GetAllTransdetails` | `Public Function GetAllTransdetails(ByVal v_lTransDetailId As Integer, ByRef r_vTransdetails(,) As Object) As Integer` | Get all transaction details for a transaction |
| `UpdateComment` | `Public Function UpdateComment(ByVal v_lTransDetailId As Integer, ByVal v_sComment As String) As Integer` | Update transaction comment |
| `UpdateNotReported` | `Public Function UpdateNotReported(ByVal v_lTransDetailId As Integer, ByVal v_boNotReported As Boolean) As Integer` | Toggle not-reported flag on transaction |
| `FormatCurrency` | `Public Function FormatCurrency(ByRef vCurrencyID As Object, ByRef vCurrencyAmount As Object, ByRef vFormattedCurrency As Object, Optional ByVal vConversionDate As Object = Nothing, Optional ByVal vConvertToBase As Object = Nothing) As Integer` | Format currency amount |
| `GetWriteOffReasons` | `Public Function GetWriteOffReasons(ByRef v_vWriteOffReasons As Object) As Integer` | Get write-off reason codes |
| `GetInsuranceFolderDetails` | `Public Function GetInsuranceFolderDetails(ByVal v_lInsuranceFolderCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Get insurance folder details |
| `AddInputParameter` | `Public Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer` | Add input parameter to database parameters collection |
| `GetEventInsuranceFileForDocument` | `Public Function GetEventInsuranceFileForDocument(ByVal v_sDocumentRef As String, ByVal v_lSourceID As Integer, ByRef r_lInsuranceFileCnt As Integer) As Integer` | Get insurance file CNT for a document reference |
| `GetTransFromQueryAdditional` | `Public Function GetTransFromQueryAdditional() As String` | Get additional FROM clause for transaction query (claim joins) |
| `GetMarkTransdetails` | `Public Function GetMarkTransdetails(ByVal v_lDocumentID As Integer, ByVal v_lTransDetailId As Integer, ByVal v_lAccountID As Integer, ByRef r_bTransdetailIds As Boolean) As Integer` | Get marked transaction details |
| `GetCashListTypeCode` | `Public Function GetCashListTypeCode(ByVal v_iTransDetailId As Integer, ByRef vResultArray(,) As Object) As Integer` | Get cash list type code for a transaction |
| `GetAccountTypes` | `Public Function GetAccountTypes(ByRef r_vResultArray As Object) As Integer` | Get all account types |
| `GetPolicyBalance` | `Public Function GetPolicyBalance(ByVal v_lAccountID As Integer, ByVal v_dAccountingDate As Date, ByVal v_sInsurance_ref As String, ByRef r_vResultArray As Object, Optional ByVal v_lInsuranceFileCnt As Integer = 0) As Integer` | Get policy balance for account/insurance ref |
| `GetPremiumFinanceBalance` | `Public Function GetPremiumFinanceBalance(ByVal v_lAccountID As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_dAccountingDate As Date, ByRef r_vResultArray As Object, ByVal v_sInsurance_ref As String) As Integer` | Get premium finance balance |
| `GetSubAgentDetails` | `Public Function GetSubAgentDetails(ByVal v_iInsuranceFileCnt As Integer, ByRef vResultArray(,) As Object) As Integer` | Get sub-agent details for insurance file |
| `GetCaseNumber` | `Public Function GetCaseNumber(ByRef r_sCaseNumber As String) As Long` | Get case number |
| `GetReceiptReversalUserAuthority` | `Public Function GetReceiptReversalUserAuthority(ByVal nUserID As Integer, ByRef vResultArray(,) As Object) As Integer` | Check if user has receipt reversal authority |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Clean up resources and child components |
| `CheckIsThirdParty` | `Public Function CheckIsThirdParty(ByVal v_sPlanRef As String, ByRef r_bIsThirdParty As Boolean) As Integer` | Determines if a plan reference is associated with a third-party instalment arrangement |
| `GetPremiumFinanceBalance` | `Public Function GetPremiumFinanceBalance(ByVal v_lAccountID As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_dAccountingDate As Date, ByRef r_vResultArray As Object, ByVal v_sInsurance_ref As String) As Integer` | Retrieves Premium Finance balance for account with insurance file, date, and reference criteria |
| `PlanStatusUpdate` | `Public Function PlanStatusUpdate(ByVal v_sPlanRef As String, ByVal vStatusInd As String) As Integer` | Updates Premium Finance plan status indicator (e.g., to Live) |
| `SetCashListItemFlags` | `Public Function SetCashListItemFlags(ByVal nCashlistItemID As Integer, ByVal dtReversedDate As Date, ByVal nCashlistitemReversePMuserID As Integer, ByVal nCashlistitemReverseReasonID As Integer, ByVal nCashlistitemReversalTransdetailID As Integer, ByVal nIsReceiptReversal As Integer) As Integer` | Sets reversal flags on cash list items with user, reason, and reversal transaction details |
| `UpdateCashListItemForAllocationStatus` | `Public Function UpdateCashListItemForAllocationStatus(ByVal v_iTransDetailId As Integer) As Integer` | Updates CashListItem allocation status based on transaction detail ID |

**Stored Procedures (bACTFindTransactionSql.vb + bACTFindTransactionCls.vb):**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_ACT_Do_GetTransId` | `GetTransID` | Get transaction ID from parameters |
| `spu_ACT_Do_SelectLedgers` | `GetLedgersQuery` | Select all ledgers for company |
| `spu_ACT_get_fp_dets_from_doc_ref` | `GetFinancePlanDetails` | Get finance plan details from document reference |
| `spu_ACT_Select_Credit_Amounts` | `GetAccountAmounts` | Get credit amounts for manage debtors |
| `spu_ACT_Select_Account_Details` | `GetAccountDetails` | Select account details |
| `spu_ACT_Update_TransDetail_Comment` | `UpdateComment` | Update transaction detail comment |
| `spu_ACT_Update_TransDetail_NotReported` | `UpdateNotReported` | Toggle transaction not-reported flag |
| `spu_ACT_Get_Write_Off_Reasons` | `GetWriteOffReasons` | Get write-off reason codes |
| `spu_ACT_Get_Account_Types` | `GetAccountTypes` | Get account types |
| `spu_ACT_Select_View_Allocation` | `ViewAllocationDetails` | View allocation details for transaction |
| `spu_ACT_Select_RollupTransactions` | `GetRollupTransactions` | Get rolled-up transactions |
| `spu_ACT_Select_Mark_Transaction` | `GetMarkTransdetails` | Get marked transaction details |
| `spu_Get_Policy_Balance` | `GetPolicyBalance` | Get policy balance |
| `spu_Get_Premium_Finance_Balance` | `GetPremiumFinanceBalance` | Get premium finance balance |
| *(dynamic SQL)* | `SelectTransQuery` | Complex dynamic SQL with FROM/WHERE clauses built from constants |
| `SPU_ACT_UpdateCashListItem_For_AllocationStatus` | `UpdateCashListItemForAllocationStatus` | Updates CashListItem for allocation status tracking |
| `spu_ACT_Get_CashListPaymentType` | `GetCashListPaymentType` | Gets CashListItemType_Code for payment type classification |
| `spu_ACT_Get_Currecny_Not_Linked_With_Source` | `GetCurrenciesNotLinkedWithSource` | Gets currencies not linked with source for CCY cash allocation |
| `spu_ACT_Set_CashListItem_ReverseAllocation` | `SetCashListItemFlags` | Updates CashListItem reversal and allocation flags |
| `spu_Check_Third_Party_Instalment` | `CheckIsThirdParty` | Validates if a plan reference is a third-party instalment |
| `spu_Get_Linked_CashDeposit_Account_IDs` | `GetLinkedCashDepositAccountIDs` | Gets linked cash deposit account IDs for reconciliation (WPR85) |
| `spu_PFPremiumFinance_UpdateStatusLive` | `PlanStatusUpdate` | Updates Premium Finance plan status to Live |
| `spu_Select_SubAgents` | `GetSubAgents` | Retrieves list of sub-agents for transaction processing |
| `spu_TXN_event_insurance_file_sel` | `GetEventInsuranceFileDocs` | Retrieves event-based insurance file documents |
| `spu_Get_Premium_Finance_Balance` | `GetPremiumFinanceBalance` | Gets Premium Finance balance for account with insurance file, date, and reference criteria |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bPMLookup.Business` | `Initialise`, `GetLookupValues` | Lookup value retrieval |
| `bACTAllocationCalculate.Form` | `Initialise` | Allocation calculation operations |
| `bACTPeriod.Form` | `Initialise` | Accounting period operations |
| `bACTAccount.Form` | `Initialise` | Account operations |
| `bSIRSolutionConfig.Business` | `Initialise` | Solution configuration |
| `bACTAuditSet.Form` | `Initialise`, `AddAuditSet` | Audit set management |
| `bACTCurrencyConvert.Form` | `Initialise`, `FormatCurrency` | Currency conversion |

---

### 47. bACTImportExport
**Directory:** `ImportExport/`
**Project:** `bACTImportExport`
**Purpose:** Import and export of accounting data via XML files. Scans folders for XML import/export files, provides file summaries, preview of records before import, manual import/export processing, and period/branch data retrieval. Supports receipt import with account validation.

**Business Methods — Business (bACTImportExport.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialise with credentials and database, check cloud hosting option |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set optional process modes |
| `GetFileSummary` | `Public Function GetFileSummary(ByVal sPath As String, ByRef vResults As Object) As Integer` | Scan folder for XML files, return summary (filename, date, interface, reference, record count) |
| `GetRecordPreview` | `Public Function GetRecordPreview(ByVal v_sPath As String, ByVal v_sFilename As String, ByRef r_vHeader() As Object, ByRef r_vDetail(,) As Object) As Integer` | Get preview of records from an import/export XML file |
| `ProcessManualImport` | `Public Function ProcessManualImport(ByVal v_sFilename As String) As Integer` | Process manual import of an XML file |
| `ProcessManualExport` | `Public Function ProcessManualExport(ByVal v_iInterface As Integer, ByVal v_lBatchID As Integer, ByVal v_vParamArray(,) As Object) As Integer` | Process manual export to XML file |
| `UpdateRecordPreview` | `Public Function UpdateRecordPreview(ByVal v_sPath As String, ByVal v_sFilename As String, ByVal v_vHeader() As Object, ByVal v_vDetail(,) As Object) As Integer` | Update XML file with edited preview data |
| `GetAllPeriods` | `Public Function GetAllPeriods(ByRef r_vPeriodArray(,) As Object) As Integer` | Get all accounting periods |
| `GetPeriods` | `Public Function GetPeriods(ByRef r_vPeriod(,) As Object) As Integer` | Get periods for selection |
| `GetCurrentPeriods` | `Public Function GetCurrentPeriods(ByVal r_vPeriod As String, ByRef r_voCurrentPeriod(,) As Object) As Integer` | Get current active periods |
| `GetBranchDetails` | `Public Function GetBranchDetails(ByRef branchDetails(,) As Object) As Integer` | Get branch/source details |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Clean up resources |

**Interface — PreviewBase (PreviewBase.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `GetPreview` | `Function GetPreview(ByVal oRoot As XmlNode, ByRef vHeader() As Object, ByRef vDetail(,) As Object) As Integer` | Returns preview of supplied XML |
| `Initialise` | `Function Initialise(ByRef sUsername As String, ...) As Integer` | Initialise the preview class |
| `Update` | `Function Update(ByVal oRoot As XmlNode, ByRef vHeader() As Object, ByRef vDetail(,) As Object) As Integer` | Update XML with changes |

**ReceiptImport — ReceiptImport (ReceiptImport.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `PreviewBase_GetPreview` | *(Implements PreviewBase.GetPreview)* | Preview receipt import XML — extracts batch reference, bank account, date, currency, totals, record details with account validation |
| `PreviewBase_Initialise` | *(Implements PreviewBase.Initialise)* | Initialise receipt import |
| `PreviewBase_Update` | *(Implements PreviewBase.Update)* | Update receipt import XML with edited account codes |
| `TryGetAttribute` | `Public Function TryGetAttribute(ByVal oNode As XmlElement, ByVal sAttribute As String, Optional ByVal vDefault As String = "") As String` | Safely retrieves XML element attribute value; returns default if attribute missing or exception occurs |
| `TrySetAttribute` | `Public Sub TrySetAttribute(ByVal oNode As XmlElement, ByVal sAttribute As String, ByVal vValue As String)` | Safely sets XML element attribute value; silently fails if attribute cannot be set |

**Stored Procedures (bACTImportExportSQL.vb):**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_RI_Model_del` | RI Model operations | Delete RI Model |
| `spu_RI_Model_add` | RI Model operations | Insert RI Model |
| `spu_RI_Model_saa` | RI Model operations | Select RI Models |
| `spu_RI_Model_upd` | RI Model operations | Update RI Model |
| `spu_RI_Model_Line_del` | RI Model Line operations | Delete RI Model Line |
| `spu_RI_Model_Line_add` | RI Model Line operations | Insert RI Model Line |
| `spu_RI_Model_Line_saa` | RI Model Line operations | Select RI Model Lines |
| `Spu_Select_AllperiodEnddates` | `GetAllPeriods` | Select all period end dates |
| `spu_ACT_SelAll_Ledger` | `GetAllDetails` | Select all ledgers |
| `spu_ACT_Select_Period` | `GetPeriods` | Select periods |
| `spu_PM_Select_Source` | `GetBranchDetails` | Select branch/source details |
| `Spu_Select_AllperiodEnddates` | `GetDetails` | Retrieves all period end dates for account period lookups |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTAccount.Form` | `ReceiptImport.ValidateAccount` | Account validation for receipt imports |

---

### 48. bACTImportSiriusTrans
**Directory:** `ImportSiriusTrans/`
**Project:** `bACTImportSiriusTrans`
**Purpose:** Import and post Sirius transactions into the Orion accounting system. Posts documents with full transaction detail including currency conversion, commission processing, commission tax, suspended account handling, auto-allocation of cancelling transactions, risk transfer agreements, and account derivation. Core component for the Sirius-to-Orion accounting integration.

**Business Methods — Business (bACTImportSiriusTrans.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialise with credentials, database, and create all dependent business objects |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray As Object) As Integer` | Navigator SetKeys |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray As Object) As Integer` | Navigator GetKeys |
| `GetSummary` | `Public Function GetSummary(ByRef vSummaryArray As Object) As Integer` | Navigator GetSummary |
| `GetOption` | `Public Function GetOption(ByVal v_iOptionNumber As Integer, ByRef r_sOptionValue As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Get system option value |
| `Start` | `Public Function Start() As Integer` | Navigator Start function |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set process modes |
| `PostDocument` | `Public Function PostDocument(ByVal v_sDocRef As String, ByVal v_sDocDebitCredit As String, ByVal v_sDocTransactionTypeCode As String, ByVal v_dtDocDate As Date, ByVal v_dtDocAccountingDate As Date, ByVal v_sDocComments As String, ByVal v_sDocCurrencyCode As String, ByVal v_sDocBusinessTypeCode As String, ByVal v_sDocInsuranceRef As String, ByVal v_sDocProductCode As String, ByVal v_sDocBranchCode As String, ByVal v_sDocLeadAgentShortName As String, ByVal v_sDocInsuranceHolderShortName As String, ByVal v_dtDocInsuranceEffectiveDate As Date, ByVal v_iDocOperatorID As Object, ByVal v_vTransactionsArray(,) As Object, ByRef r_lDocPostedStatus As Integer, ByVal v_iDocSourceID As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_sReason As String, Optional ByRef r_vNewDocumentId As Object = Nothing, Optional ByRef v_vTermsOfPaymentId As Object = Nothing, Optional ByRef v_vPaymentDueDate As Object = Nothing, Optional ByRef r_sfailureReason As String = "") As Integer` | Post a full accounting document from Sirius transaction data with all transaction lines, currency conversion, commission, and period allocation |
| `PostCommission` | `Public Function PostCommission(ByVal v_sCommissionOption As String, ByVal v_iCompanyID As Integer, ByVal v_lTransactionId As Integer) As Integer` | Post commission transactions based on commission option |
| `PostCommissionTax` | `Public Function PostCommissionTax(ByVal v_iCompanyID As Integer, ByVal v_lTransactionId As Integer) As Integer` | Post commission tax transactions |
| `DeriveAccountID` | `Public Function DeriveAccountID(ByRef r_lAccountId As Integer, ByVal v_lParentNodeId As String, ByVal v_sRelativeCode As String, ByVal v_iAccountType As Integer, ByVal v_lLedgerId As Integer) As Integer` | Derive account ID from parent node, relative code, account type, and ledger |
| `AutoAllocateCancellingTransactions` | `Public Function AutoAllocateCancellingTransactions(ByVal vTransactions(,) As Object, Optional ByVal bAllowPartialAllocation As Boolean = False, Optional ByVal cWriteOffAmount As Decimal = 0, Optional ByVal lWriteOffReasonID As Integer = 0, Optional ByVal bCurrencyWriteOff As Boolean = False, Optional ByVal lCashListItemId As Integer = 0, Optional sDocBusinessTypeCode As String = "", Optional ByVal v_lPaymentAccount As Integer = 0) As Integer` | Auto-allocate cancelling transactions with optional partial allocation, write-off, and currency write-off |
| `WriteSuspendedAccountsTransactions` | `Public Function WriteSuspendedAccountsTransactions(ByVal vSuspenseArray(,) As Object, ByVal lDocumentId As Integer, Optional ByVal lLinkedTransactionID As Integer = 0) As Integer` | Write suspended accounts transactions for commission suspense handling |
| `GetInsurerRiskTransferAgreement` | `Public Function GetInsurerRiskTransferAgreement(ByVal lDocumentId As Integer, ByVal lAccountId As Integer) As Integer` | Get insurer risk transfer agreement for document/account |
| `DoAccountExists` | `Public Function DoAccountExists(ByVal sShortCode As String) As Integer` | Check if an account exists by short code |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Clean up resources and child components |
| `AutoAllocateCancellingTransactions` | `Public Function AutoAllocateCancellingTransactions(ByVal vTransactions(,) As Object, Optional ByVal bAllowPartialAllocation As Boolean = False, Optional ByVal cWriteOffAmount As Decimal = 0, Optional ByVal lWriteOffReasonID As Integer = 0, Optional ByVal bCurrencyWriteOff As Boolean = False, Optional ByVal lCashListItemId As Integer = 0, Optional sDocBusinessTypeCode As String = "", Optional ByVal v_lPaymentAccount As Integer = 0) As Integer` | Auto-allocates cancelling transactions against each other; supports partial allocation, write-offs, and currency write-offs |
| `DeriveAccountID` | `Public Function DeriveAccountID(ByRef r_lAccountId As Integer, ByVal v_lParentNodeId As String, ByVal v_sRelativeCode As String, ByVal v_iAccountType As Integer, ByVal v_lLedgerId As Integer) As Integer` | Derives account ID from parent node, relative code, account type, and ledger ID in the account hierarchy |
| `DoAccountExists` | `Public Function DoAccountExists(ByVal sShortCode As String) As Integer` | Checks if an account exists by short code; returns account ID if found |
| `GetInsurerRiskTransferAgreement` | `Public Function GetInsurerRiskTransferAgreement(ByVal lDocumentId As Integer, ByVal lAccountId As Integer) As Integer` | Determines if an insurer account has an active risk transfer agreement for the document |
| `GetOption` | `Public Function GetOption(ByVal v_iOptionNumber As Integer, ByRef r_sOptionValue As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Retrieves system option value by option number |
| `PostCommission` | `Public Function PostCommission(ByVal v_sCommissionOption As String, ByVal v_iCompanyID As Integer, ByVal v_lTransactionId As Integer) As Integer` | Posts commission entry for a transaction based on commission option and company |
| `PostCommissionTax` | `Public Function PostCommissionTax(ByVal v_iCompanyID As Integer, ByVal v_lTransactionId As Integer) As Integer` | Posts commission tax entry for a transaction |
| `PostDocument` | `Public Function PostDocument(ByVal v_sDocRef As String, ByVal v_sDocDebitCredit As String, ByVal v_sDocTransactionTypeCode As String, ByVal v_dtDocDate As Date, ByVal v_dtDocAccountingDate As Date, ByVal v_sDocComments As String, ByVal v_sDocCurrencyCode As String, ByVal v_sDocBusinessTypeCode As String, ByVal v_sDocInsuranceRef As String, ByVal v_sDocProductCode As String, ByVal v_sDocBranchCode As String, ByVal v_sDocLeadAgentShortName As String, ByVal v_sDocInsuranceHolderShortName As String, ByVal v_dtDocInsuranceEffectiveDate As Date, ByVal v_iDocOperatorID As Object, ByVal v_vTransactionsArray(,) As Object, ByRef r_lDocPostedStatus As Integer, ByVal v_iDocSourceID As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_sReason As String, Optional ByRef r_vNewDocumentId As Object = Nothing, Optional ByRef v_vTermsOfPaymentId As Object = Nothing, Optional ByRef v_vPaymentDueDate As Object = Nothing, Optional ByRef r_sfailureReason As String = "") As Integer` | Core method — posts a transaction document with full accounting details; returns PMTrue (1) on success |
| `WriteSuspendedAccountsTransactions` | `Public Function WriteSuspendedAccountsTransactions(ByVal vSuspenseArray(,) As Object, ByVal lDocumentId As Integer, Optional ByVal lLinkedTransactionID As Integer = 0) As Integer` | Writes suspended account transaction records after document posting |

**Stored Procedures (bACTImportSiriusTransBusinessSQL.vb + inline):**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_Get_AccountIdFromShortCode` | `DoAccountExists` | Check account exists by short code |
| `spu_GetStatsCurrencyXRate` | `PostDocument` | Get stats currency exchange rate for CLC documents |
| `spu_ACT_Trans_Detail_Rounding` | rounding operations | Transaction detail rounding |
| `spu_ACT_Get_Suspended_Commission` | `GetSuspendedCommissionTransactions` | Get suspended commission transactions for insurance file |
| `spe_Insurance_File_sel` | `PostDocument` | Get insurance file details (payment method, balance type) |
| `Spu_Update_Agent_Details_for_Suspended_account` | `PostDocument` | Updates agent details (Agent_Code, Insurance_ref) for suspended account policies |
| `spu_ACT_Get_SuspendedAccountsTransactions_Triggers` | `WriteSuspendedAccountsTransactions` | Retrieves trigger records for suspended accounts transactions — commented out |
| `spu_ACT_Get_SuspendedAgentTaxTransactions_Triggers` | `WriteSuspendedAccountsTransactions` | Retrieves trigger records for suspended agent tax transactions — commented out |
| `spu_ACT_Get_SuspendedFeeTransactions_Triggers` | `WriteSuspendedAccountsTransactions` | Retrieves trigger records for suspended fee transactions — commented out |
| `spu_ACT_Get_SuspendedTaxTransactions_Triggers` | `WriteSuspendedAccountsTransactions` | Retrieves trigger records for suspended tax transactions — commented out |
| `spu_Get_Due_Date_For_Transactions` | `GetDueDateForTransactions` | Gets payment due date for a transaction based on insurance file count and account ID |
| `spu_Get_TMP` | `PostDocument` | Alias for spu_get_true_monthly_policy_details — retrieves true monthly policy details |
| `spu_PFPremiumFinance_Sel_SingleFromInsuranceFileCount` | `GetPremiumFinanceRecord` | Retrieves premium finance plan record (cnt, version) from insurance file count |
| `spu_TRN_risk_transfer_status_select` | `GetInsurerRiskTransferAgreement` | Determines if an insurer account has an active risk transfer agreement |
| `spu_get_true_monthly_policy_details` | `PostDocument` | Retrieves true monthly policy details including lead/sub month cycles, consolidation flags, renewal count |
| `spu_transaction_export_complete_add` | `UpdateTransactionExportComplete` | Marks a transaction export folder as complete |
| `spu_transaction_export_complete_sel` | `CheckTransactionExportComplete` | Checks if a transaction export has already been completed |
| `spu_ACT_Trans_Detail_Rounding` | `ValidateAndFixRounding` | Validates and fixes rounding errors in transaction details |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTAccount.Form` | `Initialise` | Account operations |
| `bACTDocumentPost.Form` | `Initialise`, `PostDocument` | Document posting |
| `bACTCurrencyConvert.Form` | `Initialise`, `PostDocument` | Currency conversion |
| `bACTExplorer.Form` | `Initialise` | Explorer/tree navigation |
| `bACTPeriod.Form` | `Initialise` | Period management |
| `bACTCurrency.Form` | `Initialise` | Currency operations |
| `bACTTransdetail.Form` | `Initialise` | Transaction detail operations |
| `bSIROptions.Business` | `GetOption` | System options retrieval |
| `bACTCommissionPost.Business` | `PostCommission`, `PostCommissionTax` | Commission posting |

---

### 49. bACTInstalments
**Directory:** `Orion/Components/Instalments/Business/bACTInstalments/`
**Project:** `bACTInstalments`
**Purpose:** Manages premium finance instalment plans — posting plans, instalments, adjustments (MTA/UW), cancellations, settlements, commission suspense processing, and reinsurance dripping for instalment-based payment schemes.

**Business Methods — Business (bACTInstalmentsCls.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing, Optional ByRef vSIRDatabase As Object = Nothing) As Integer` | Initialises component with credentials and database |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets process modes |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Stores parameter members from key array |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Returns key array with parameter members |
| `PostPlan` | `Public Function PostPlan(ByVal v_lPartyCnt As Integer, ByVal v_sInstalmentRef As String, ...) As Integer` | Posts a new premium finance plan with accounting transactions |
| `PostInstalment` | `Public Function PostInstalment(ByVal v_lPlanTransDetailID As Integer, ...) As Integer` | Posts an individual instalment payment |
| `PostAdjustment` | `Public Function PostAdjustment(ByRef v_lPlanTransDetailID As Integer, ByVal v_vFinanceTransIds() As Object, ByVal v_cAmount As Decimal, ByVal v_lPremiumFinanceCnt As Integer, ByVal v_lPremiumFinanceVersion As Integer) As Integer` | Posts a plan adjustment |
| `PostMTAAdjustment` | `Public Function PostMTAAdjustment(ByRef r_lNewPlanTransID As Integer, ...) As Integer` | Posts a mid-term adjustment to a plan |
| `PostUWAdjustment` | `Public Function PostUWAdjustment(ByRef r_lNewPlanTransID As Integer, ByVal v_lPlanTransDetailID As Integer, ByVal v_vFinanceTrans As Object, ByVal v_cAdjustment As Decimal, ByVal v_cFee As Decimal, ByVal v_cOSAmount As Decimal, ByVal v_lPremiumFinanceCnt As Integer, ByVal v_lPremiumFinanceVersion As Integer, Optional ByVal v_lLeadAgentCnt As Integer = 0) As Integer` | Posts an underwriting adjustment to a plan |
| `CancelPlan` | `Public Function CancelPlan(ByVal v_lPlanTransDetailID As Integer, ByVal v_cAmount As Decimal, ByVal v_lPremiumFinanceCnt As Integer, ByVal v_lPremiumFinanceVersion As Integer, ByVal v_cTax As Decimal, Optional ByRef r_vCreditTransDetail As Object = Nothing, Optional ByRef r_vDebitTransDetail As Object = Nothing, Optional ByVal v_bPlanHasSinglePolicy As Boolean = False, Optional ByVal v_iIsThirdPartyPlan As Integer = 0, Optional ByVal v_lPartyCnt As Integer = 0) As Integer` | Cancels a premium finance plan |
| `SettlePlanCalculate` | `Public Function SettlePlanCalculate(ByVal v_lPremiumFinanceCnt As Integer, ByVal v_lPremiumFinanceVersion As Integer, ByRef r_crSettlement As Decimal, ByRef r_crRefundFee As Decimal, Optional ByRef r_vRefundTax As Decimal = Nothing) As Integer` | Calculates settlement amounts for a plan |
| `SettlePlanCalculateRefund` | `Public Function SettlePlanCalculateRefund(ByVal nPremiumFinanceCnt As Integer, ByVal nPremiumFinanceVersion As Integer, ByRef r_crSettlement As Decimal, ByRef r_crInterestRefund As Decimal) As Integer` | Calculates refund amount for plan settlement |
| `SettleCancelledSupersededPlan` | `Public Function SettleCancelledSupersededPlan(ByVal v_nPlanTransDetailID As Integer, ByVal v_nPremiumFinanceCnt As Integer, ByVal v_nPremiumFinanceVersion As Integer, ...) As Integer` | Settles a cancelled or superseded plan |
| `RecallInstalment` | `Public Function RecallInstalment(ByVal v_lPlanTransDetailID As Integer, ByVal v_lInstalmentTransdetailID As Integer) As Integer` | Recalls a previously posted instalment |
| `RecallInstalment` (overload) | `Public Function RecallInstalment(ByVal v_lPlanTransDetailID As Integer, ByVal v_lInstalmentTransdetailID As Integer, ...) As Integer` | Recalls instalment with additional parameters |
| `DeleteCancelledInstalments` | `Public Function DeleteCancelledInstalments(ByVal v_lFinancePlanCnt As Integer, ByVal v_lFinancePlanVersion As Integer) As Integer` | Deletes cancelled instalment records |
| `GetPFFromTransID` | `Public Function GetPFFromTransID(ByVal v_lTransactionId As Integer, ByRef r_lPremiumFinanceCnt As Integer, ByRef r_lPremiumFinanceVersion As Integer, ByRef r_nSpreadCommission As Integer, ByRef r_lPlanTransDetailID As Integer) As Integer` | Gets premium finance details from a transaction |
| `GetPlanOS` | `Public Function GetPlanOS(ByVal v_lPlanTransDetailID As Integer, ByRef r_cOS As Decimal) As Integer` | Gets outstanding amount for a plan |
| `GetCashListItem` | `Public Function GetCashListItem(ByVal v_lInstalmentID As Integer, ByRef r_lCashListItemID As Integer, ByRef r_lCashTransactionID As Integer) As Integer` | Gets cash list item for an instalment |
| `GetCollectionAccountID` | `Public Function GetCollectionAccountID(ByVal v_lCashDrawerID As Integer, ByRef r_lCollectionAccountID As Integer) As Integer` | Gets collection account from cash drawer |
| `GetCreditControlItemID` | `Public Function GetCreditControlItemID(ByVal lPFInstalmentsID As Integer, ByRef r_lCCIID As Integer) As Integer` | Gets credit control item for instalment |
| `GetCommissionDetails` | `Public Function GetCommissionDetails(ByVal v_lTransactionId As Integer, ByVal v_lPremiumFinanceCnt As Integer, ByVal v_lPremiumFinanceVersion As Integer, ByVal v_lInstalmentID As Integer, ByRef r_lCommissionSuspenseTransID As Integer, ByRef r_dPercentage As Double, ByRef r_bIsLastInstalment As Boolean, ByRef r_lFeeAccountID As Integer) As Integer` | Gets commission suspension details |
| `GetOutstandingCommission` | `Public Function GetOutstandingCommission(ByVal v_lFinancePlanCnt As Integer, ByVal v_lFinancePlanVersion As Integer, ByRef r_cCommission As Decimal, ByRef r_cOutstandingCommission As Decimal, ByRef r_lCommissionEarnedAccountId As Integer, ByRef r_lTransactionId As Integer) As Integer` | Gets outstanding commission for a plan |
| `GetOutstandingCommission_SFU` | `Public Function GetOutstandingCommission_SFU(ByVal v_lFinancePlanCnt As Integer, ByVal v_lFinancePlanVersion As Integer, ByRef r_cCommission As Decimal, ...) As Integer` | Gets outstanding commission (SFU-specific) |
| `GetSuspenseDetails` | `Public Function GetSuspenseDetails(ByVal v_lPremiumFinanceCnt As Integer, ByVal v_lPremiumFinanceVersion As Integer, ByVal v_lInstalmentID As Integer, ByRef r_vSuspenseTransDetails(,) As Object) As Integer` | Gets suspended transaction details |
| `GetFeeAccountFromTransID` | `Public Function GetFeeAccountFromTransID(ByVal v_lTransactionId As Integer, ByRef r_lFeeAccountID As Integer) As Integer` | Gets fee account from transaction |
| `GetPeriodFromInsuranceFile` | `Public Function GetPeriodFromInsuranceFile(v_lInsuranceFileCnt As Integer, ByRef nPeriodID As Integer)` | Gets posting period from insurance file |
| `GetValueFromTable` | `Public Function GetValueFromTable(ByVal v_sTableName As String, ByVal v_vReturnColumn As Object, ByVal v_sKeyColumn As String, ByVal v_sKeyValue As String, ByVal v_iDataType As Integer, ByRef r_vResult(,) As Object) As Integer` | Generic table value lookup |
| `AddInputParameter` | `Public Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer` | Adds a database input parameter |
| `ProcessSuspendedTransactions` | `Public Function ProcessSuspendedTransactions(ByVal v_lAllocationId As Integer, ...) As Integer` | Processes suspended (dripping) transactions for commission/RI |
| `UpdateSuspendedAccountsTransactions` | `Public Function UpdateSuspendedAccountsTransactions(ByVal v_vOldTriggerTransdetailIds As Object, Optional ByVal v_vNewTriggerTransdetailId As Object = Nothing, ...) As Integer` | Updates suspended account transaction linkages |
| `ChangeCommissionTransDetailsID` | `Public Function ChangeCommissionTransDetailsID(ByVal v_lFinancePlanCnt As Integer, ByVal v_lFinancePlanVersion As Integer, ByVal v_lCommissionAccountID As Integer, ByRef v_sDocumentRef As String, ByVal v_nDocSeq As Integer) As Integer` | Changes commission transaction detail IDs |
| `ChangePFTransactionDetailsID` | `Public Function ChangePFTransactionDetailsID(ByVal v_lFinancePlanCnt As Integer, ByVal v_lFinancePlanVersion As Integer, ByRef v_sDocumentRef As String) As Integer` | Changes premium finance transaction detail IDs |
| `CreatePFTransactions` | `Public Function CreatePFTransactions(ByVal v_lFinancePlanCnt As Long, ByVal v_lFinancePlanVersion As Long, ByVal v_lAccountId As Long, ByVal v_sSpare As String, ByVal v_lTransDetailID As Long, ByVal v_lTransactionType As Long) As Long` | Creates records in PF_Accounts_Transactions table — superseded (FSA Phase 3.2), currently commented out |

**Stored Procedures (bACTInstalmentsSQL.vb):**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `spu_ACT_Get_AccountID_From_PartyCnt` | `ACGetAccountFromPartySQL` | Get account ID from party count |
| `spu_ACT_Select_TransDetail` | `ACGetTransDetailSQL` | Select transaction detail |
| `spu_ACT_Get_AccountID_From_ShortCode` | `ACGetAccountFromShortCodeSQL` | Get account ID from short code |
| `spu_ACT_Select_Credit_Control_Item_For_Plan` | `ACGetCCIFromInstalmentSQL` | Get credit control item for plan |
| `spu_ACT_CancelPlan_InsFees_Split_By_COB` | `ACCancelPlanInsFeesSplitByCOBSQL` | Cancel plan when instalment fees split by COB |
| `spu_ACT_Copy_Stats_Details` | `ACCopyStatsFolderAndSettleSQL` | Copy stats folder and settle |
| `spu_ACT_Get_CR_TransDetail_For_Instalment_Settlement` | `ACSelectCreditTransDetailSQL` | Get credit transdetail for instalment settlement |
| `spu_PFGetRISuspenseInfo` | `ACGetRISuspenseInfoSQL` | Get reinsurance suspense flag and account |
| `spu_PFGetRIAccountInfo` | `ACGetSuspendedPartiesSQL` | Get RI/FAC parties for suspense |
| `spu_PFGetPartyTransactionsToSuspend` | `ACGetSuspendedTransactionsSQL` | Get RI suspended transactions for party |
| `spu_PFAccountsTransactions_Add` | `ACCreatePFAccountsTransactionsSQL` | Insert into PF_Accounts_Transactions |
| `spu_PFAccountsTransactions_Update` | `ACUpdatePFAccountsTransactionsSQL` | Update PF_Accounts_Transactions |
| `spu_ACT_GetPFFromInstalmentsID` | `ACGetPFFromInstalmentsIDSQL` | Get PF scheme properties from instalment ID |
| `spu_ACT_GetDetailsFor_PFTransactions` | `ACGetSuspenseDetailsSQL` | Get details for releasing suspended transactions |
| `spu_PFGetLastInstalmentID` | `ACGetLasInstalmentIDSQL` | Get last instalment ID for PF cnt/version |
| `spu_ACT_Get_TransDetail_Type_Id` | `ACGetRISUSPTransdetailTypeIDSQL` | Get transdetail type ID by code |
| `spu_ACT_Get_Period_Id_From_ledger` | `ACGetCurrentPeriodFromLegderSQL` | Get current period from ledger |
| `spu_Get_Account_Details_For_InsuranceFile` | `ACGetAccountDetailsForInsuranceFileSQL` | Get account details for insurance file |
| `spu_PFPremiumFinance_GetRefundAmount` | `ACSettlePlanCalculateRefundSQL` | Get plan outstanding/refund amount |
| `spu_PFGetInsuranceFile_TransactionType` | `KGetInsuranceFileTransactionTypeSQL` | Get insurance file transaction type |
| `spu_Get_Amount_Not_Included_In_Instalment` | `ACGetAmountNotIncludedInInstalmentSQL` | Get amount not included in instalment |
| `Spu_ACT_Get_Outstanding_Tax_Amount` | `ACGetOutstandingTaxAmount` | Get outstanding tax amount |
| `spu_ACT_Get_TransDetail_DocumentType` | `kGetTransDetailDocumentTypeSQL` | Get document type for transdetail ID |
| `spu_ACT_Select_CashListDrawer` | `ACSelectCashListDrawerSQL` | Select cash list drawer |
| `Spu_ACT_Get_Outstanding_Tax_Amount` | `GetOutstandingTaxAmount` | Gets outstanding tax amount for a document and account |
| `spu_ACT_ChangePFTransactionID` | `ChangePFTransactionDetailsID` | Updates premium finance transaction details ID after commission transfer |
| `spu_ACT_GetDetailsForCommissionPost` | `GetCommissionDetails` | Gets commission details (suspense ID, percentage, last instalment flag) for posting |
| `spu_ACT_GetPFFromTransID` | `GetPFFromTransID` | Gets premium finance record (cnt, version, spread flag) from transaction ID |
| `spu_ACT_Get_CD_CollectionAccountID` | `GetCollectionAccountID` | Gets collection account ID from cash drawer ID |
| `spu_ACT_Get_InsuranceFileFromPF` | `GetInsuranceFileCntFromPF` | Gets insurance file details (cnt, ref, renewal flag, cover date) from PF record |
| `spu_GetCashListBankAccount` | `GetSchemeAccount` | Gets bank account from premium finance scheme for cash posting |
| `spu_PFChangeCommisionTransID` | `ChangeCommissionTransDetailsID` | Updates commission transaction details ID after moving commission between accounts |
| `spu_PFGetCashListItemFromInstalment` | `GetCashListItem` | Gets associated cash list item and transaction ID from instalment ID |
| `spu_PFGetDepositInfo` | `SettlePlanCalculate` | Gets deposit information for premium finance plan settlement calculation |
| `spu_PFGetOutstandingCommission` | `GetOutstandingCommission` | Gets outstanding commission amount and account details for plan |
| `spu_PFGetOutstandingCommission_SFU` | `GetOutstandingCommission_SFU` | Gets outstanding commission for underwriting-specific operations with transaction array |
| `spu_PFInstalments_get_instalments_tax` | `PostInstalment` | Gets tax split by band from tax calculation table for instalment posting |
| `spu_PFPremiumFinance_settlement` | `SettlePlanCalculate` | Calculates settlement amount for premium finance plan |
| `spu_PFScheme_GetAccount` | `GetSchemeAccount` | Gets account from premium finance scheme (Fee, Tax, Suspense, Bank, Commission, Sub-Agent Commission) |
| `spu_PFScheme_GetCurrency` | `GetSchemeCurrency` | Gets base currency and exchange rate from premium finance scheme |
| `spu_PFgetFeeAccountFromTrans` | `GetFeeAccountFromTransID` | Gets fee/interest account ID from transaction ID |
| `spu_Scheme_GetAccount` | `GetSchemeAccount` | Gets account details from premium finance scheme by account type code |
| `spu_PFPremiumFinance_GetRefundAmount` | `SettlePlanCalculateRefund` | Gets refund calculation details (remaining instalments, interest, uncollected tax) for plan settlement |
| `spu_PFGetInsuranceFile_TransactionType` | `GetInsuranceFileTransactionType` | Gets transaction type code from insurance file to determine if reset to IRD is required |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTAllocationManual.Business` | `Business` | Manual allocation processing |
| `bACTDocumentPost.Form` | `Business` | Document posting for accounting transactions |
| `bACTAutoNumber.Business` | `Business` | Auto-number generation |
| `bACTTransDetail.Form` | `Business` | Transaction detail management and suspended transactions |
| `bACTCurrencyConvert.Form` | `Business` | Currency conversion |

---

### 50. bACTInsurerPaymentAllocate
**Directory:** `Orion/Components/InsurerPaymentAllocate/Business/bACTInsurerPaymentAllocate/`
**Project:** `bACTInsurerPaymentAllocate`
**Purpose:** Processes insurer payment allocation — creates allocation entries, matches payments against outstanding insurer transactions, and handles currency conversion for payment settlement.

**Business Methods — Business (Business.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises component with credentials |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTypeOfBusiness As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets process modes |
| `SetStatus` | `Public Function SetStatus(ByVal sProcessStatus As String, ByVal sMapStatus As String, ByVal sStepStatus As String) As Integer` | Sets process status |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Sets keys (BatchID, AccountID, CashListId, CashListItemId) |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Returns allocation ID key |
| `Start` | `Public Function Start() As Integer` | Entry point — runs allocation create then allocation process |
| `ProcessAllocationCreate` | `Public Function ProcessAllocationCreate() As Integer` | Creates allocation records |
| `ProcessAllocation` | `Public Function ProcessAllocation() As Integer` | Processes the allocation matching |
| `DeleteMatchPayment` | `Public Function DeleteMatchPayment(ByVal v_lTransDetailID As Integer) As Integer` | Deletes a match payment record |
| `GetMatchPayment` | `Public Function GetMatchPayment(ByVal v_lTransDetailID As Integer, ByRef v_cBaseAmount As Decimal, ByRef v_cCurrencyAmount As Decimal) As Integer` | Gets match payment amounts |
| `GetSymbolForCurrency` | `Public Function GetSymbolForCurrency(ByVal v_lCurrencyID As Integer, ByRef r_sSymbol As String) As Integer` | Gets currency symbol |

**Stored Procedures (MainModule.vb):**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `spu_ACT_Do_InsurerPayments` | `ACInsurerPaymentsSQL` | Execute insurer payment processing |
| `spu_ACT_Delete_TransMatch` | `ACDeleteMatchPaymentSQL` | Delete transaction match record |
| `spu_Act_UpdTransferTransDetail` | `UpdateTransferTransDetail` | Updates transfer transaction detail — commented out in current code |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTAllocationCreate.Automated` | `Business` | Creates allocation entries |
| `bACTAllocationDetail.Form` | `Business` | Allocation detail management |
| `bACTAllocation.Form` | `Business` | Allocation processing |
| `bACTMatchPost.Form` | `Business` | Match posting |
| `bACTDocument.Form` | `Business` | Document management |
| `bACTTransDetail.Form` | `Business` | Transaction detail operations |
| `bACTAutoNumber.Business` | `Business` | Auto-number generation |
| `bACTPeriod.Form` | `Business` | Period management |
| `bACTAccount.Form` | `Business` | Account operations |
| `bACTWriteOffReason.Form` | `Business` | Write-off reason lookups |

---

### 51. bACTInsurerPaymentGroups
**Directory:** `Orion/Components/InsurerPaymentGroups/Business/bACTInsurerPaymentGroups/`
**Project:** `bACTInsurerPaymentGroups`
**Purpose:** Manages insurer payment group assignments — associates insurer accounts with payment groups and companies, enabling grouped payment processing.

**Business Methods — Business (bACTInsurerPaymentGroups.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceId As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises component |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets process modes |
| `GetAccountID` | `Public Function GetAccountID(ByRef lPartyCnt As Integer, ByRef laccountID As Integer) As Integer` | Gets Orion account ID from SBO party count |
| `GetDetails` | `Public Function GetDetails(ByRef laccountID As Integer, ByRef vPaymentGroups(,) As Object) As Integer` | Gets payment group details for an account |
| `GetLookupValues` | `Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As String) As Integer` | Gets lookup values for PaymentGroup |
| `GetCompanyValues` | `Public Function GetCompanyValues(ByRef vResultArray(,) As String) As Integer` | Retrieves all non-deleted companies from database (company_id, description) into result array |
| `GetPaymentLockingType` | `Public Function GetPaymentLockingType(ByVal v_laccountID As Integer, ByRef r_lPaymentLockingType As Integer) As Integer` | Retrieves payment locking type for specified account from database |

**Stored Procedures (bACTInsurerPaymentGroupsSQL.vb):**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `{call spe_Insurer_Payment_Type_sel (?)}` | `ACGetPaymentLockingTypeSQL` | Select insurer payment locking type |

Note: Most queries in this component use inline SQL rather than stored procedures (e.g., SELECT from `account`, `InsurerPayment`, `PaymentGroup`, `company`, `Party` tables).

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bPMLookup.Business` | `Business` | Lookup value resolution |

---

### 52. bACTInsurerPaymentSFU
**Directory:** `Orion/Components/InsurerPayment/Business/bACTInsurerPaymentSFU/`
**Project:** `bACTInsurerPaymentSFU`
**Purpose:** Manages insurer/agent payment processing for Sirius For Underwriting (SFU) — handles searching, marking/unmarking transactions for payment, binder journal processing, write-off transactions, instalment-based payment views, and comment updates.

**Business Methods — Business (Business.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises component and sub-components |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, ...) As Integer` | Sets process modes |
| `SetStatus` | `Public Function SetStatus(ByVal sProcessStatus As String, ByVal sMapStatus As String, ByVal sStepStatus As String) As Integer` | Sets process status |
| `FormatCurrency` | `Public Function FormatCurrency(ByVal v_lCurrencyID As Integer, ByVal v_cCurrencyAmount As Decimal, Optional ByVal v_dtConversionDate As Date = #12/30/1899#) As String` | Formats a value as currency string |
| `GetAccountFromShortCode` | `Public Function GetAccountFromShortCode(ByVal v_sShortCode As String, ByRef r_lAccountId As Integer) As Integer` | Gets account ID from short code |
| `MarkTransaction` | `Public Function MarkTransaction(ByVal v_lTransactionID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_cPayment As Decimal, Optional ByVal v_iInstalmentNumber As Integer = 0) As Integer` | Creates a TransMatch entry marking transaction as ready for payment |
| `UnMarkTransaction` | `Public Function UnMarkTransaction(ByVal v_lTransDetailId As Integer) As Integer` | Removes mark from a transaction |
| `UnMarkInstTransaction` | `Public Function UnMarkInstTransaction(ByVal v_iTransdetailid As Integer, ByVal v_iInstalmentNumber As Integer) As Integer` | Removes instalment-specific mark |
| `ProcessBinder` | `Public Function ProcessBinder(ByVal v_lAccountId As Integer, ByVal v_iCurrencyID As Integer, ByVal v_cPayment As Decimal, ByRef v_vTransdetailIds() As Object) As Integer` | Creates binder journal and allocates |
| `SearchDetails` | `Public Function SearchDetails(ByVal v_vAccountID As Integer, Optional ByVal v_vDateTo As Object = Nothing, ...) As Integer` | Searches insurer payment details |
| `SearchDetailsForBatch` | `Public Function SearchDetailsForBatch(ByVal nBatchID As Integer, ...) As Integer` | Searches payment details for a batch |
| `GetTransDetailsFromBatch` | `Public Function GetTransDetailsFromBatch(ByVal v_lBatchID As Long, ...) As Integer` | Gets transaction details from a batch |
| `AddWriteOffTransaction` | `Public Function AddWriteOffTransaction(ByVal Docid As Integer, ByVal Account_id As Integer, ByVal WriteOffAccID As Integer, ByVal WriteOffamt As Decimal, Optional ByVal v_sAltReferance As String = "", Optional ByRef m_lTransDetailID As Integer = 0) As Integer` | Adds a write-off transaction |
| `DeleteWriteOffTransaction` | `Public Function DeleteWriteOffTransaction(ByVal Docid As Integer) As Integer` | Deletes write-off transactions |
| `UpdateWriteOffDocumentRef` | `Public Function UpdateWriteOffDocumentRef(ByVal v_lOldDocumentId As Integer, ...) As Integer` | Updates write-off document reference |
| `UpdateComment` | `Public Function UpdateComment(ByVal v_lTransDetailId As Integer, ByVal v_sComment As String) As Integer` | Updates transaction comment |
| `GetInstalmentDetailsForInsurerPayment` | `Public Function GetInstalmentDetailsForInsurerPayment(ByVal v_iAccountid As Integer, ByVal v_sViewType As String, ByRef r_vInstalArray(,) As Object) As Integer` | Gets instalment details for insurer payment view |
| `GetTranDetailContraEntriesForInstalments` | `Public Function GetTranDetailContraEntriesForInstalments(ByVal v_sParam As String, ByRef vResultArray(,) As Object) As Integer` | Gets contra entries for instalments |
| `GetTransDetailIdForSetteledPremium` | `Public Function GetTransDetailIdForSetteledPremium(ByVal v_iTransdetailid As Long, ByRef r_vResultsArray(,) As Object) As Integer` | Gets transdetail ID for settled premium |
| `GetAgentDetailForAccount` | `Public Function GetAgentDetailForAccount(ByVal v_lAccountid As Long, ByRef r_vResultsArray(,) As Object) As Long` | Gets agent details for account |
| `DeleteTransMatchInst` | `Public Function DeleteTransMatchInst(ByVal v_iTransdetailid As Integer) As Integer` | Deletes TransMatch instalment records |
| `LoadAllocationPeriod` | `Public Function LoadAllocationPeriod(ByRef r_vResultArray(,) As Object) As Integer` | Loads allocation period information |

**Stored Procedures (MainModule.vb):**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `spu_ACT_Do_InsurerPayments_SFU` | `ACInsurerPaymentsSQL` | Execute SFU insurer payments |
| `spu_Delete_WriteOff_Transactions` | `ACDeleteWOFFSQL` | Delete write-off transactions |
| `spu_ACT_Update_TransDetail_Comment` | `ACUpdateCommentSQL` | Update transdetail comment |
| `spu_Get_Agent_Detail_ForAccount` | `ACGetAgentDetailForAccountSQL` | Get agent detail for account |
| `spu_ACT_Update_TransMatch_InstalmentNumber` | `ACUpdateInstalmentNumberSQL` | Update TransMatch instalment number |
| `spu_ACT_Select_Instalments_For_PartySettelment` | `ACSelInstalmentsForPartySettelmentSQL` | Select instalments for party settlement |
| `spu_ACT_Get_Allocation_Period` | `ACLoadAllocationPeriodSQL` | Get allocation period |
| `spu_ACT_Do_GetPeriodForDate` | `ACGetPeriodIdSQL` | Get period ID for a date |
| `spu_Get_TranDetailContraEntriesForInstalments` | `ACGetTranDetailContraEntriesForInstalmentsSQL` | Get contra entries for instalments |
| `spu_ACT_GetTransDetailIdForSetteledPremium` | `ACGetTransDetailIdForSetteledPremiumSQL` | Get transdetail for settled premium |
| `spu_ACT_Find_InsurerPayments_ForBatch` | `kInsurerPaymentsForBatchSQL` | Find insurer payments for a batch |
| `spu_Get_PMNav_Batch_Transaction_Details` | `KGetPMNavXMBatchTransactionDetailSQL` | Get PMNav batch transaction details |
| `spu_ACT_Update_WritOff_Document` | `KTUpdateWriteOffDocumentSQL` | Update write-off document |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTAllocate.Business` | `Business` | Allocation processing |
| `bACTCurrencyConvert.Form` | `Business` | Currency conversion and formatting |
| `bACTDocument.Form` | `Business` | Document creation and reference generation |
| `bACTDocumentPost.Form` | `Business` | Document posting |
| `bACTExplorer.Form` | `Business` | Account explorer/lookup by short code |
| `bACTMatchPost.Form` | `Business` | Match posting (mark transactions) |
| `bACTPeriod.Form` | `Business` | Period management |
| `bACTTransdetail.Form` | `Business` | Transaction detail operations |

---

### 53. bACTInvoice
**Directory:** `Orion/Components/PurchaseInvoice/Business/bACTInvoice/`
**Project:** `bACTInvoice`
**Purpose:** Manages purchase invoice headers — CRUD operations for invoices including add, update, delete, checking duplicate invoice numbers, retrieving account names, and transdetail type lookups.

**Business Methods — Business (bACTInvoiceBusiness.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises component |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, ...) As Integer` | Sets process modes |
| `SetStatus` | `Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer` | Sets status |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Sets Nav keys |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray As String) As Integer` | Gets Nav keys (string) |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Gets Nav keys (array) |
| `GetSummary` | `Public Function GetSummary(ByRef vSummaryArray As Object) As Integer` | Gets summary array |
| `Start` | `Public Function Start() As Integer` | Navigator Start entry point |
| `GetAccountName` | `Public Function GetAccountName(ByRef lAccountID As Integer, ByRef sAccountName As String) As Integer` | Gets account short code from account ID |
| `GetNewID` | `Public Function GetNewID() As Integer` | Gets next available invoice ID |
| `GetAccountID` | `Public Function GetAccountID(ByRef r_lSupplierID As Integer, ByVal v_sShortCode As String) As Integer` | Gets account ID from short code (multi-branch aware) |
| `GetTransdetailTypeId` | `Public Function GetTransdetailTypeId(ByRef sCode As String, ByRef iTransdetailTypeID As Integer) As Integer` | Gets transdetail type ID from code |
| `GetDetails` | `Public Function GetDetails(Optional ByRef vLockMode As gPMConstants.PMELockMode = 0, Optional ByRef vInvoiceID As Object = Nothing) As Integer` | Gets invoice details from database |
| `GetNext` | `Public Function GetNext(Optional ByRef vInvoiceID As Object = Nothing, ...) As Integer` | Gets next invoice record |
| `GetDefaults` | `Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, ...) As Integer` | Gets default values |
| `DirectAdd` | `Public Function DirectAdd(Optional ByRef vInvoiceID As Object = Nothing, ...) As Integer` | Directly adds invoice to database |
| `DirectDelete` | `Public Function DirectDelete(ByRef vID As Object) As Integer` | Directly deletes invoice |
| `EditAdd` | `Public Function EditAdd(ByRef lRow As Integer, ...) As Integer` | Adds invoice to collection for batch processing |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow As Integer, ...) As Integer` | Updates invoice in collection |
| `EditDelete` | `Public Function EditDelete(ByVal lRow As Integer) As Integer` | Marks invoice for deletion in collection |
| `Update` | `Public Function Update() As Integer` | Commits all collection changes to database |
| `Cancel` | `Public Function Cancel() As Integer` | Cancels pending changes |
| `CheckID` | `Public Function CheckID(ByRef vID As Object) As Integer` | Checks if invoice ID exists |
| `CheckIfInvoiceNumberExists` | `Public Function CheckIfInvoiceNumberExists(ByRef r_vAccountID As String, ByVal v_vInvoiceNumber As Object) As Integer` | Checks for duplicate invoice number |

**Stored Procedures (bACTInvoiceBusinessSQL.vb):**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `spe_Invoice_saa` | `ACGetAllDetailsSQL` | Select all invoices |
| `spe_ACTInvoice_check_id` | `ACCheckIDSQL` | Check invoice ID exists |
| `spu_ACT_Get_Transdetail_Type_Id` | `ACGetTransdetailTypeIdSQL` | Get transdetail type ID |
| `spu_ACT_Select_Invoice` | `ACSelectSingleSQL` | Select single invoice |
| `spu_ACT_Add_Invoice` | `ACAddSQL` | Add invoice |
| `spu_ACT_Delete_Invoice` | `ACDeleteSQL` | Delete invoice |
| `spu_ACT_Update_Invoice` | `ACUpdateSQL` | Update invoice |
| `spu_ACT_Get_AccountID_From_ShortCode` | (inline call) | Get account ID from short code |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| None | — | Self-contained; uses internal collection (`ACTInvoices`) |

---

### 54. bACTInvoiceItem
**Directory:** `Orion/Components/PurchaseInvoiceItem/Business/bACTInvoiceItem/`
**Project:** `bACTInvoiceItem`
**Purpose:** Manages purchase invoice line items — CRUD operations for individual invoice items including descriptions, nominal codes, values, currency, department, and VAT rate.

**Business Methods — Business (bACTInvoiceItemBusiness.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises component |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, ...) As Integer` | Sets process modes |
| `SetStatus` | `Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer` | Sets status |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Sets Nav keys |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray As String) As Integer` | Gets Nav keys (string) |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Gets Nav keys (array) |
| `GetSummary` | `Public Function GetSummary(ByRef vSummaryArray As Object) As Integer` | Gets summary array |
| `Start` | `Public Function Start() As Integer` | Navigator Start entry point |
| `DeleteInvoiceItems` | `Public Function DeleteInvoiceItems(Optional ByRef vInvoiceID As Integer = 0) As Integer` | Deletes all invoice items for a given invoice |
| `Cancel` | `Public Function Cancel() As Integer` | Checks if any items in collection have pending changes (Add/Edit/Delete); returns PMDataChanged if changes pending |
| `GetDepartment` | `Public Function GetDepartment(ByVal v_iDepartmentID As Integer, ByRef r_vDepartment As Object) As Integer` | Retrieves department caption from CostCentre table for given department ID as of current date |
| `SelectItem` | `Public Function SelectItem(ByRef oACTInvoiceItem As Object) As Integer` | Populates ACTInvoiceItem object by selecting from database using invoice_id and invoice_item_no primary keys |

**Stored Procedures (bACTInvoiceItemBusinessSQL.vb):**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `spu_ACT_Select_InvoiceItemList` | `ACGetAllDetailsSQL` | Select all invoice items |
| `spe_ACTInvoiceItem_check_id` | `ACCheckIDSQL` | Check invoice item ID |
| `spu_ACT_Delete_Invoice_Item` | `ACDeleteInvoiceSQL` | Delete invoice items for an invoice |
| `spe_Invoice_Item_sel` | `ACSelectSingleSQL` | Select single invoice item |
| `spu_ACT_Add_Invoice_Item` | `ACAddSQL` | Add invoice item |
| `spu_ACT_Delete_Invoice_item` | `ACDeleteSQL` | Delete single invoice item |
| `spe_Invoice_Item_upd` | `ACUpdateSQL` | Update invoice item |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| None | — | Self-contained; uses internal collection (`ACTInvoiceItems`) |

---

### 55. bACTLedger
**Directory:** `Orion/Components/Ledger/Business/bACTLedger/`
**Project:** `bACTLedger`
**Purpose:** Manages accounting ledgers — CRUD operations for ledger definitions including ledger types, company/sub-branch assignments, period management, mapping configuration, node lookups, and period closure processing. Supports caching.

**Business Methods — Form (bACTLedgerForm.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises component with caching |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, ...) As Integer` | Sets process modes |
| `GetLookupValues` | `Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As String) As Integer` | Gets lookup values for ledger types |
| `GetDetails` | `Public Function GetDetails(Optional ByRef vLedgerID As Object = Nothing, Optional ByRef vLockMode As Integer = 0, Optional ByRef vSubBranchID As Object = Nothing) As Integer` | Gets ledger details from database |
| `GetNext` | `Public Function GetNext(Optional ByRef vLedgerID As Object = Nothing, ..., Optional ByRef vSequence As Object = Nothing) As Integer` | Gets next ledger record |
| `GetDefaults` | `Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, ...) As Integer` | Gets default ledger values |
| `DirectAdd` | `Public Function DirectAdd(Optional ByRef vLedgerID As Integer = 0, ..., Optional ByRef vSequence As Object = Nothing) As Integer` | Directly adds ledger |
| `DirectDelete` | `Public Function DirectDelete(Optional ByRef vLedgerID As Object = Nothing, ...) As Integer` | Directly deletes ledger |
| `EditAdd` | `Public Function EditAdd(ByRef lRow As Integer, ...) As Integer` | Adds ledger to collection |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow As Integer, ...) As Integer` | Updates ledger in collection |
| `EditDelete` | `Public Function EditDelete(ByVal lRow As Integer) As Integer` | Marks ledger for deletion |
| `Update` | `Public Function Update() As Integer` | Commits all changes to database |
| `Cancel` | `Public Function Cancel() As Integer` | Cancels pending changes |
| `CheckID` | `Public Function CheckID(ByRef vID As Object) As Integer` | Checks if ledger ID exists |
| `GetCaptions` | `Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object) As Integer` | Gets field captions |
| `GetCaptions` (overload) | `Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, ByRef vTable As Object) As Integer` | Gets field captions with table |
| `GetLedgerNodeId` | `Public Function GetLedgerNodeId(ByVal v_sLedgerName As String, ByRef v_lNodeId As Integer) As Integer` | Gets ledger node ID by name |
| `GetNodeFromLedger` | `Public Function GetNodeFromLedger(ByVal v_sLedgerName As String, ByVal v_lSubBranchID As Integer, ByRef r_lNodeId As Integer) As Integer` | Gets node from ledger by name and sub-branch |
| `GetMappingDetails` | `Public Function GetMappingDetails(ByRef lNumberOfRecords As Integer, ByRef vResultArray(,) As Object, ByVal iCompanyID As Integer) As Integer` | Gets all mapping details for company |
| `GetMappingDetails` (overload) | `Public Function GetMappingDetails(ByRef lNumberOfRecords As Integer, ByRef vResultArray(,) As Object, ByVal iCompanyID As Integer, ByVal vMapTypeID As Integer) As Integer` | Gets mapping details filtered by type |
| `GetClosures` | `Public Function GetClosures(ByRef vLedgerID As Object) As Integer` | Gets ledger closure details |
| `GetClosures` (overload) | `Public Function GetClosures(ByRef vLedgerID As Object, ByRef vLockMode As Integer) As Integer` | Gets closures with lock mode |

**Business Methods — Automated (bACTLedgerAutomated.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ..., Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises automated class |
| `SetProcessModes` | `Public Function SetProcessModes(...) As Integer` | Sets process modes |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Sets keys |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Gets keys |
| `Start` | `Public Function Start() As Integer` | Automated action entry point |

**Stored Procedures (bACTLedgerFormSQL.vb + bACTLedgerAutomatedSQL.vb):**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `spu_ACT_Select_Ledger` | `ACGetDetailsSQL` / `ACAutoGetDetailsSQL` | Select ledger |
| `spu_ACT_SelAll_Ledger` | `ACGetAllDetailsSQL` / `ACAutoGetAllDetailsSQL` | Select all ledgers |
| `spu_ACT_Check_Ledger` | `ACCheckIDSQL` / `ACAutoCheckIDSQL` | Check ledger ID |
| `spu_ACT_Add_Ledger` | `ACAddSQL` / `ACAutoAddSQL` | Add ledger |
| `spu_ACT_Delete_Ledger` | `ACDeleteSQL` / `ACAutoDeleteSQL` | Delete ledger |
| `spu_ACT_Update_Ledger` | `ACUpdateSQL` / `ACAutoUpdateSQL` | Update ledger |
| `spu_ACT_Select_LedgerNode` | `ACSelectLedgerNodeSQL` | Select ledger node ID |
| `spu_ACT_Select_NodeFromLedger` | `ACSelectNodeFromLedgerSQL` | Select node from ledger |
| `spu_ACT_SelAll_Mapping` | `ACGetAllMappingSQL` | Select all mappings |
| `spu_ACT_Select_Closures` | `ACGetClosuresSQL` | Select ledger closures |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bPMLookup.Business` | `Form` | Lookup value resolution |

---

### 56. bACTMaintainMediaTypeStatus
**Directory:** `Orion/Components/MaintainMediaTypeStatus/Business/bACTMaintainMediaTypeStatus/`
**Project:** `bACTMaintainMediaTypeStatus`
**Purpose:** Maintains media type status for policy receipts (cheques, direct debits, etc.) — enables searching, viewing, and updating the status of payment media types and creating tasks for status changes.

**Business Methods — Form (bACTMaintainMediaTypeStatusCls.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises component |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, ...) As Integer` | Sets process modes |
| `GetLookupValues` | `Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As String) As Integer` | Gets lookup values |
| `GetReceiptsForMediaTypeStatusMaintenance` | `Public Function GetReceiptsForMediaTypeStatusMaintenance(ByVal v_iUserID As Object, ByRef r_vResultArray(,) As Object, Optional ByVal v_vBranchID As Object = Nothing, Optional ByVal v_vBankAccountID As Object = Nothing, Optional ByVal v_vShortName As Object = Nothing, Optional ByVal v_vInsurance_Ref As Object = Nothing, Optional ByVal v_vCollectionDateFrom As Object = Nothing, Optional ByVal v_vCollectionDateTo As Object = Nothing, Optional ByVal v_vMediaReference As Object = Nothing, Optional ByVal v_vMediaTypeStatusID As Object = Nothing, Optional ByVal v_vDrawnBankID As Object = Nothing, Optional ByVal v_vDocumentRef As Object = Nothing, Optional ByVal v_vMaxRowsToFetch As Object = Nothing) As Integer` | Searches policy receipts for media type status maintenance |
| `GetPolicyStatusForMediaTypeStatusMaintenance` | `Public Function GetPolicyStatusForMediaTypeStatusMaintenance(ByVal v_lInsuranceFileID As Integer, ByRef r_bIsPolicyCancelled As Boolean, ByRef r_bIsClaimPaymentInitiated As Boolean) As Integer` | Gets policy status (cancelled/claim initiated) |
| `UpdateMediaTypeStatusForPolicyReciept` | `Public Function UpdateMediaTypeStatusForPolicyReciept(ByVal v_lCashListItemID As Integer, ByVal v_lMediaTypeID As Integer, ByVal v_lMediaTypeStatusID As Integer, ByVal v_sComments As String, ByVal v_iUserID As Integer, ByVal v_dtDateModified As Date, ByVal v_lInsuranceFileID As Integer, ByVal v_sDocumentRef As String) As Integer` | Updates media type status for a single receipt |
| `UpdateMediaTypeStatusForPolicyReciepts` | `Public Function UpdateMediaTypeStatusForPolicyReciepts(ByRef v_vUpdateArray(,) As Object) As Integer` | Batch updates media type status for multiple receipts |
| `CreateTask` | `Public Function CreateTask(ByVal v_lTaskGroupId As Integer, ByVal v_lTaskId As Integer, ByVal v_sCustomer As String, ByVal v_lUserGroupId As Integer, ByVal v_sDescription As String, ByVal v_iIsVisible As Integer) As Integer` | Creates a workflow task for status change |

**Stored Procedures (bACTMaintainMediaTypeStatusSql.vb):**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `spe_User_Authorities_Sel` | `ACPaymentMaintenanceGetUserReverseAllocationQuerySQL` | Select user reverse allocation authority |
| `spu_ACT_Find_Receipt` | `ACGetPolicyReceiptsForMediaTypeStatusMaintenanceSQL` | Find policy receipts for media type maintenance |
| `spu_ACT_Get_Policy_Status_For_MediaType_Status_Maintenance` | `ACGetPolicyStatusForMediaTypeStatusMaintenanceSQL` | Get policy status for media type maintenance |
| `spu_ACT_Update_Receipt_MediaType_Status` | `ACUpdateReceiptMediaTypeStatusSQL` | Update receipt media type status |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bPMLookup.Business` | `Form` | Lookup value resolution |

---

### 57. bACTMatchgroup
**Directory:** `Orion/Components/MatchGroup/Business/bACTMatchgroup/`
**Project:** `bACTMatchgroup`
**Purpose:** Manages match groups — tracks groups of matched/allocated transactions including period, company, and match date. Provides full CRUD plus collection-based editing for batch operations.

**Business Methods — Form (bACTMatchgroupForm.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ..., Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises component |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, ...) As Integer` | Sets process modes |
| `GetDetails` | `Public Function GetDetails(Optional ByRef vMatchID As Object = Nothing, Optional ByRef vLockMode As Integer = 0) As Integer` | Gets match group details |
| `GetNext` | `Public Function GetNext(Optional ByRef vMatchID As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vMatchDate As Object = Nothing) As Integer` | Gets next match group record |
| `GetDefaults` | `Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vMatchID As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vMatchDate As Object = Nothing) As Integer` | Gets default values |
| `DirectAdd` | `Public Function DirectAdd(Optional ByRef vMatchID As Integer = 0, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vMatchDate As Object = Nothing) As Integer` | Directly adds match group and returns MatchID |
| `DirectDelete` | `Public Function DirectDelete(Optional ByRef vMatchID As Object = Nothing, ...) As Integer` | Directly deletes match group |
| `EditAdd` | `Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vMatchID As Object = Nothing, ...) As Integer` | Adds match group to collection |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow As Integer, ...) As Integer` | Updates match group in collection |
| `EditDelete` | `Public Function EditDelete(ByVal lRow As Integer) As Integer` | Marks for deletion in collection |
| `Update` | `Public Function Update() As Integer` | Commits all changes to database |
| `Cancel` | `Public Function Cancel() As Integer` | Cancels pending changes |
| `CheckID` | `Public Function CheckID(ByRef vID As Object) As Integer` | Checks if match ID exists |
| `GetCaptions` | `Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, Optional ByRef vTable As Object = Nothing) As Integer` | Gets field captions |

**Business Methods — Automated (bACTMatchgroupAutomated.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ..., Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises automated class |
| `SetProcessModes` | `Public Function SetProcessModes(...) As Integer` | Sets process modes |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Sets keys |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Gets keys |
| `Start` | `Public Function Start() As Integer` | Automated action entry point |

**Stored Procedures (bACTMatchgroupFormSQL.vb + bACTMatchgroupAutomatedSQL.vb):**

| Stored Procedure | Constant | Purpose |
|-----------------|----------|---------|
| `spu_ACT_select_MatchGroup` | `ACGetDetailsSQL` / `ACAutoGetDetailsSQL` | Select match group |
| `spu_ACT_selall_MatchGroup` | `ACGetAllDetailsSQL` | Select all match groups (Form) |
| `spu_ACT_select_all_MatchGroup` | `ACAutoGetAllDetailsSQL` | Select all match groups (Automated) |
| `spu_ACT_check_MatchGroup` | `ACCheckIDSQL` / `ACAutoCheckIDSQL` | Check match group ID |
| `spu_ACT_add_MatchGroup` | `ACAddSQL` / `ACAutoAddSQL` | Add match group |
| `spu_ACT_delete_MatchGroup` | `ACDeleteSQL` / `ACAutoDeleteSQL` | Delete match group |
| `spu_ACT_update_MatchGroup` | `ACUpdateSQL` / `ACAutoUpdateSQL` | Update match group |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| None | — | Self-contained; uses internal collection (`Matchgroups`) |

---

### 58. bACTMatchPost
**Directory:** `MatchPost/`
**Project:** `bACTMatchPost`
**Purpose:** Manages the creation and processing of match postings — groups of matched allocation/transaction entries used during the cash matching/allocation process in accounting.

**Business Methods — Form (bACTMatchPostCls.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises the component, database connection, and child components (bACTMatchGroup, bACTTransMatch) |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process mode parameters |
| `SetStatus` | `Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer` | Sets Process, Map and Step status values |
| `AddMatchGroup` | `Public Function AddMatchGroup(ByVal v_dtMatchDate As Date, ByRef r_vMatchId As Integer) As Integer` | Creates a new match group for the given date (overload 1) |
| `AddMatchGroup` | `Public Function AddMatchGroup(ByVal v_dtMatchDate As Date, ByVal v_lSubBranchID As Integer, ByRef r_vMatchId As Integer) As Integer` | Creates a new match group with sub-branch (overload 2) |
| `AddMatchGroup` | `Public Function AddMatchGroup(ByVal v_dtMatchDate As Date, ByVal v_lSubBranchID As Integer, ByRef r_vMatchId As Integer, ByRef r_vMatchSourceId As Object) As Integer` | Creates a new match group with sub-branch and source (overload 3) |
| `AddMatchTrans` | `Public Function AddMatchTrans(ByVal v_lAllocationdetailID As Object, ByVal v_lTransDetailID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_cBaseMatchAmount As Decimal, ByVal v_cCurrencyMatchAmount As Decimal, ByVal v_vdCurrencyMatchXRate As Object) As Integer` | Adds a transaction to the current match group (overload 1) |
| `AddMatchTrans` | `Public Function AddMatchTrans(ByVal v_lAllocationdetailID As Object, ByVal v_lTransDetailID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_cBaseMatchAmount As Decimal, ByVal v_cCurrencyMatchAmount As Decimal, ByVal v_vdCurrencyMatchXRate As Object, ByRef r_vTransMatchId As Integer) As Integer` | Adds a transaction to the current match group and returns the transmatch ID (overload 2) |
| `Commit` | `Public Function Commit() As Integer` | Validates the match totals and commits by clearing the abort flag |

**Stored Procedures:**
| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| *(None — uses bACTMatchGroup.DirectAdd and bACTTransMatch.DirectAdd internally; GetPeriodIdForDate delegates to bACTPeriod.Form)* | | |
| `spu_ACT_Add_AllocationBatch` | `CreateAllocationBatch` | Creates a new allocation batch for year-end retained profit posting |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTMatchgroup.Form` | `Initialise`, `AddMatchGroup` | Creates and retrieves match group records |
| `bACTTransmatch.Form` | `Initialise`, `AddMatchTrans` | Creates transaction match records within a group |
| `bACTPeriod.Form` | `GetPeriodIdForDate` (private) | Resolves a date to an accounting period ID |

---

### 59. bACTMisAllocationHelper
**Directory:** `MisAllocationHelper/`
**Project:** `bACTMisallocationsHelper`
**Purpose:** A Windows Forms utility for diagnosing and correcting mis-allocated accounting transactions. Provides UI for browsing mis-allocated accounts, viewing allocations, editing amounts, and adding related/missing/other transaction lines to match groups.

**Business Methods — MainModule (MainModule.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `GeneratePassword` | `Public Function GeneratePassword(ByVal nSeed As Integer) As Integer` | Generates a numeric password from a seed value using date/time-based calculation |

**UI Forms — frmAccounts (frmAccounts.vb):**

| Member | Signature | Description |
|--------|-----------|-------------|
| `m_oConnection` | `Public m_oConnection As SqlConnection` | Shared SQL connection for all forms |
| `m_tsLog` | `Public m_tsLog As FileStream` | Log file stream for audit trail |
| `OpenLogFile` | `Public Sub OpenLogFile()` | Opens the log file for writing |

**UI Forms — frmAddRelated (frmAddRelated.vb):**

| Member | Signature | Description |
|--------|-----------|-------------|
| `MatchID` | `Public Property MatchID() As Integer` | Gets/sets the match group ID to work with |
| `AccountCode` | `Public Property AccountCode() As String` | Gets/sets the account code filter |
| `Transactions` | `Public ReadOnly Property Transactions() As Object` | Returns selected transaction IDs |
| `AddType` | `Public WriteOnly Property AddType() As Integer` | Sets the type of add operation (Related/Missing/Other) |

**UI Forms — frmEdit (frmEdit.vb):**

| Member | Signature | Description |
|--------|-----------|-------------|
| `OriginalAmount` | `Public Property OriginalAmount() As Decimal` | Gets/sets the original transaction amount |
| `AllocatedAmount` | `Public Property AllocatedAmount() As Decimal` | Gets/sets the allocated amount |
| `OSAmount` | `Public Property OSAmount() As Decimal` | Gets/sets the outstanding amount |

**Stored Procedures:**
| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| *(Direct inline SQL — no stored procedure constants. All SQL is dynamically built against `allocationdetail`, `transmatch`, `transdetail`, `account`, `document`, `currency`, `source` tables)* | `frmAccounts`, `frmAddRelated` | Insert/update allocationdetail and transmatch records, query mis-allocated transactions |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| *(None — standalone WinForms utility using direct SQL)* | | |

---

### 60. bACTPaymentMaintenance
**Directory:** `Payment Maintenance/`
**Project:** `bACTPaymentMaintenance`
**Purpose:** Provides business logic for payment maintenance operations including reverse allocation of cash list items, viewing cancel payment data, looking up event types, and media type validation.

**Business Methods — Form (bACTPaymentMaintenance.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises the component, database, lookup, and currency convert objects |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTypeOfBusiness As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process mode parameters |
| `GetLookupValues` | `Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As String) As Integer` | Gets lookup values from the PM Lookup business component |
| `GetUserReverseAllocation` | `Public Function GetUserReverseAllocation(ByVal v_UserID As Integer, ByRef vResultArray(,) As Object) As Integer` | Gets user authority for reverse allocation operations |
| `SetCashListItemFlags` | `Public Function SetCashListItemFlags(ByVal v_lCashlistitem_id As Integer, ByVal v_dtReversed_date As Date, ByVal v_iCashlistitem_reverse_pmuser_id As Integer, ByVal v_lCashlistitem_reverse_reason_id As Integer, ByVal v_lcashlistitem_reversal_transdetail_id As Long, Optional ByVal nIsReceiptReversal As Integer = 0) As Integer` | Sets reverse allocation flags on a cash list item |
| `FillCancelPaymentGrid` | `Public Function FillCancelPaymentGrid(ByVal v_lTransDetailID As Long, ByRef vResultArray(,) As Object) As Integer` | Gets allocation data for the cancel payment list grid |
| `GetEventTypeId` | `Public Function GetEventTypeId(ByVal v_sEventCode As String, ByRef vResultArray(,) As Object) As Integer` | Gets event type ID from an event code |
| `GetArrayForMediaTypeValidationId` | `Public Function GetArrayForMediaTypeValidationId(ByVal v_iMediaTypeValidationID As Integer, ByRef v_vMediaTypeArray(,) As Object) As Integer` | Gets media type array for a validation ID |

**Stored Procedures (bACTPaymentMaintenanceSql.vb):**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spe_User_Authorities_Sel` | `GetUserReverseAllocation` | Selects user reverse allocation authorities |
| `spu_ACT_Set_CashListItem_ReverseAllocation` | `SetCashListItemFlags` | Updates cash list item with reversal flags |
| `spu_ACT_Select_View_Allocation` | `FillCancelPaymentGrid` | Selects allocation view data for cancel payment |
| `spu_ACT_Get_Document_Details_For_Account` | *(kGetDocumentDetailsSQL constant)* | Returns document details for a specified document/account |
| `spu_ACT_Get_EventId_FromEventCode` | `GetEventTypeId` | Gets event type ID from event code string |
| `spu_ACT_Select_MediaType_Against_ValidationId` | `GetArrayForMediaTypeValidationId` | Selects media types for a validation ID |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `BPMLOOKUP.Business` | `Initialise`, `GetLookupValues` | Provides lookup value retrieval |
| `bACTCurrencyConvert.Form` | `Initialise` | Currency conversion for front office receipting |

---

### 61. bACTPeriod
**Directory:** `Period/`
**Project:** `bACTPeriod`
**Purpose:** Manages accounting periods — CRUD operations on period records, period lookups by date, period year navigation, sub-branch support, and automated period processing via Navigator.

**Business Methods — Form (bACTPeriodForm.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises the Form class with database connection and collections |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process mode parameters |
| `DirectAdd` | `Public Function DirectAdd(Optional ByVal vPeriodID As Integer = 0, Optional ByVal vCompanyID As Object = Nothing, Optional ByVal vSubBranchID As Object = Nothing, Optional ByVal vYearName As Object = Nothing, Optional ByVal vPeriodName As Object = Nothing, Optional ByVal vPeriodEndDate As Object = Nothing, Optional ByVal vPeriodEndComplete As Object = Nothing) As Integer` | Adds a period directly to database (not to collection) |
| `DirectDelete` | `Public Function DirectDelete(Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vYearName As Object = Nothing, Optional ByRef vPeriodName As Object = Nothing, Optional ByRef vPeriodEndDate As Object = Nothing, Optional ByRef vPeriodEndComplete As Object = Nothing) As Integer` | Deletes a period directly from database |
| `GetDefaults` | `Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vYearName As Object = Nothing, Optional ByRef vPeriodName As Object = Nothing, Optional ByRef vPeriodEndDate As Object = Nothing, Optional ByRef vPeriodEndComplete As Object = Nothing) As Integer` | Returns default values for a period |
| `CheckID` | `Public Function CheckID(ByRef vID As Object) As Integer` | Checks if a period ID exists |
| `GetCaptions` | `Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object) As Integer` | Gets caption field values for a period record (overload 1) |
| `GetCaptions` | `Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, ByRef vTable As Object) As Integer` | Gets caption field values for a period record (overload 2) |
| `GetDetails` | `Public Function GetDetails(Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vYearName As Object = Nothing, Optional ByRef vLockMode As Integer = 0, Optional ByRef vSubBranchID As Integer = 0) As Integer` | Gets period records into the collection |
| `GetNext` | `Public Function GetNext(Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vYearName As Object = Nothing, Optional ByRef vPeriodName As Object = Nothing, Optional ByRef vPeriodEndDate As Object = Nothing, Optional ByRef vPeriodEndComplete As Object = Nothing) As Integer` | Gets the next period from the collection |
| `GetPeriodYears` | `Public Function GetPeriodYears(ByRef vResultArray(,) As Object) As Integer` | Gets all period year names (4 overloads with optional LockMode, SubBranchID) |
| `GetPeriodLastDate` | `Public Function GetPeriodLastDate(ByRef vResultArray(,) As Object) As Integer` | Gets the latest period end date (4 overloads with optional LockMode, SubBranchID) |
| `GetPeriodForDate` | `Public Function GetPeriodForDate(ByVal dtDateInPeriod As Date, ByRef lPeriodID As Integer) As Integer` | Gets the period ID for a given date (7 overloads with optional YearName, SubBranchID, IncludeClosed, IsPeriodNotExist) |
| `GetPostingPeriodForDate` | `Public Function GetPostingPeriodForDate(ByRef dtDateInPeriod As Date, ByRef lPeriodID As Integer, ByVal lLedgerID As Integer) As Integer` | Gets posting period for a date and ledger (2 overloads) |
| `GetNextPeriodID` | `Public Function GetNextPeriodID(ByRef lPeriodID As Integer, ByRef lNextPeriodID As Integer) As Integer` | Gets the next period ID after the given one |
| `GetPreviousPeriodID` | `Public Function GetPreviousPeriodID(ByRef lPeriodID As Integer, ByRef lPreviousPeriodID As Integer) As Integer` | Gets the previous period ID |
| `GetFirstDayOfPeriod` | `Public Function GetFirstDayOfPeriod(ByVal v_lPeriodID As Integer, ByRef r_dtDateInPeriod As Date) As Integer` | Gets the first day of a period |
| `GetCurrentPeriodDetails` | `Public Function GetCurrentPeriodDetails(ByRef r_vDetails As Object) As Integer` | Gets current period details (2 overloads with optional SubBranchID) |
| `GetUniqueYears` | `Public Function GetUniqueYears(ByRef r_vYearArray(,) As Object) As Integer` | Gets unique year names (2 overloads with optional SubBranchID) |
| `GetSubBranches` | `Public Function GetSubBranches(ByRef r_vSubBranchArray(,) As Object) As Integer` | Gets all sub-branches |
| `GetLatestUsedPeriod` | `Public Function GetLatestUsedPeriod(ByVal v_lPeriodID As Integer, ByRef r_dtLatestUsedPeriod As Date) As Integer` | Gets the latest used period date |
| `EditAdd` | `Public Function EditAdd(ByRef lRow As Integer, Optional ByVal vPeriodID As Object = Nothing, ...) As Integer` | Adds a period to the edit collection |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow As Integer, Optional ByVal vPeriodID As Object = Nothing, ...) As Integer` | Updates a period in the edit collection |
| `EditDelete` | `Public Function EditDelete(ByVal lRow As Integer) As Integer` | Marks a period for deletion in the collection |
| `Update` | `Public Function Update() As Integer` | Commits all pending add/update/delete to the database |
| `Cancel` | `Public Function Cancel() As Integer` | Cancels all pending edits |
| `UpdatePeriodYearNames` | `Public Function UpdatePeriodYearNames(ByVal v_vPeriodArray(,) As Object, ByVal v_sYearName As String) As Integer` | Updates year names for a set of periods |

**Business Methods — Period (bACTPeriodCls.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUserName As String, ...) As Integer` | Initialises the Period data object |

**Business Methods — Periods (bACTPeriods.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Add` | `Public Function Add(ByRef oNewPeriod As bACTPeriod.Period) As Integer` | Adds a Period to the collection |
| `Count` | `Public Function Count() As Integer` | Returns number of periods in collection |
| `Delete` | `Public Sub Delete(ByRef vKey As Integer)` | Removes a period from the collection |
| `Item` | `Public Function Item(ByRef vKey As Integer) As bACTPeriod.Period` | Returns a period by index |
| `DeleteAll` | `Public Sub DeleteAll()` | Removes all periods from collection |
| `Clear` | `Public Sub Clear()` | Clears and reinitialises the collection |
| `Initialise` | `Public Function Initialise(ByRef sUserName As String, ...) As Integer` | Initialises the Periods collection |

**Business Methods — Automated (bACTPeriodAutomated.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ..., Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises the automated class |
| `SetProcessModes` | `Public Function SetProcessModes(...) As Integer` | Sets process mode parameters |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Stores parameter members from key array |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Gets parameter members into key array |
| `Start` | `Public Function Start() As Integer` | Performs the automated action (Navigator entry point) |

**Stored Procedures (bACTPeriodFormSQL.vb, bACTPeriodAutomatedSQL.vb):**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_ACT_Select_Period` | `GetDetails` (single), Automated | Selects a single period by ID |
| `spu_ACT_SelAll_Period` | `GetDetails` (all), Automated | Selects all periods for a company/year |
| `spu_ACT_SelAll_PeriodYear` | `GetPeriodYears` | Selects period year names |
| `spu_ACT_Select_Period_LastDate` | `GetPeriodLastDate` | Gets the latest period end date |
| `spu_ACT_Check_Period` | `CheckID`, Automated | Checks if a period ID exists |
| `spu_ACT_Add_Period` | `DirectAdd` (via AddItem), Automated | Adds a new period record |
| `spu_ACT_Delete_Period` | `DirectDelete` (via DeleteItem), Automated | Deletes a period record |
| `spu_ACT_Update_Period` | `EditUpdate` (via Update), Automated | Updates a period record |
| `spu_ACT_Do_GetPeriodForDate` | `GetPeriodForDate` | Gets period ID for a given date |
| `spu_ACT_Get_Next_Period_Id` | `GetNextPeriodID` | Gets the next period ID |
| `spu_ACT_Get_Previous_Period_Id` | `GetPreviousPeriodID` | Gets the previous period ID |
| `spu_ACT_Get_Period_Start_Date` | `GetFirstDayOfPeriod` | Gets the start date of a period |
| `spu_ACT_Get_Current_Period` | `GetCurrentPeriodDetails` | Gets current period details |
| `spu_ACT_Get_Unique_Years` | `GetUniqueYears` | Gets unique year names |
| `spu_sub_branch_sel` | `GetSubBranches` | Selects sub-branches |
| `spu_sub_branch_default` | *(constant defined)* | Gets default sub-branch |
| `spu_ACT_Get_Latest_Used_Period` | `GetLatestUsedPeriod` | Gets latest used period date |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTPeriod.Period` | `Form` (collection items) | Period data object with properties (PeriodID, CompanyID, etc.) |
| `bACTPeriod.Periods` | `Form` (collection manager) | Collection of Period objects |

---

### 62. bACTPeriodEnd
**Directory:** `PeriodEnd/`
**Project:** `bACTPeriodEnd`
**Purpose:** Processes accounting period end and year end operations — closes periods, processes retained profit journals, updates budget actuals/variances, and manages period end reports.

**Business Methods — Form (bACTPeriodEndForm.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises the component (implements IBusiness) |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process mode parameters |
| `GetPeriodEndDates` | `Public Function GetPeriodEndDates(ByVal v_vPeriodIDs As Object, ByRef r_vPeriodEndDates(,) As Object) As Integer` | Gets period end dates for three period IDs |
| `GetPreviousPeriodEndComplete` | `Public Function GetPreviousPeriodEndComplete(ByVal v_lCurrentPeriodID As Integer, ByRef r_lPreviousPeriodID As Integer, ByRef r_iPreviousPeriodEndComplete As Integer) As Integer` | Gets the previous period's end-complete status |
| `ProcessPeriodEnd` | `Public Function ProcessPeriodEnd(ByVal v_lCurrentPeriodID As Integer, ByRef v_lPreviousPeriodId As Integer) As Integer` | Processes a period end — updates budgets and marks period as ended |
| `ProcessYearEnd` | `Public Function ProcessYearEnd(ByVal v_lPeriodIDPeriodEnd As Integer, ByVal v_sPeriodYear As String) As Integer` | Processes year end — calls retained profit journal processing |
| `ProcessPeriodEndBudgets` | `Public Function ProcessPeriodEndBudgets(ByVal v_lPeriodIDForPeriodEnd As Integer, ByVal v_sPeriodYear As String) As Integer` | Calls bACTBudget to process end-of-period budget actuals/variances |
| `ProcessRetainedProfitJournal` | `Public Function ProcessRetainedProfitJournal(ByVal v_lPeriodID As Integer) As Integer` | Processes retained profit journal entries (V1 — multi-branch with manual allocation) |
| `ProcessRetainedProfitJournalV2` | `Public Function ProcessRetainedProfitJournalV2(ByVal v_lPeriodID As Integer) As Integer` | Processes retained profit journal entries (V2 — improved version) |
| `GetReports` | `Public Function GetReports(ByRef r_vReportArray As Object) As Integer` | Gets period end report configuration from PeriodEndReport table |
| `AllowYearEnd` | `Public Function AllowYearEnd(ByVal v_lPeriod_Id As Integer, ByRef r_bAllowYearEnd As Boolean) As Integer` | Checks whether a year end is allowed for the given period |

**Business Methods — Automated (bACTPeriodEndAutomated.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ..., Optional ByRef vDatabase As dPMDAO.Database = Nothing) As Integer` | Initialises the automated class |
| `SetProcessModes` | `Public Function SetProcessModes(...) As Integer` | Sets process mode parameters |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Stores parameter members from key array |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Gets parameter members into key array |
| `Start` | `Public Function Start() As Integer` | Performs the automated action (Navigator entry point) |

**Stored Procedures (bACTPeriodEndFormSQL.vb):**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_ACT_Get_Accounts_Journal` | `ProcessRetainedProfitJournal`, `ProcessRetainedProfitJournalV2` | Gets income/expense accounts for year-end journal |
| `spu_ACT_Select_trans_For_YearEnd` | `GetPeriodTotal` (private) | Gets transaction totals for year end processing |
| `spu_ACT_Get_Period_Dates` | `GetPeriodEndDates` | Gets period end dates for three period IDs |
| `spu_ACT_SelAll_Sub_Branch` | *(constant defined)* | Gets all sub-branches |
| `spu_ACT_Check_AllowYearEnd` | `AllowYearEnd` | Checks if year end is allowed for a period |
| `spu_ACT_Add_AllocationBatch` | `CreateAllocationBatch` | Creates a new allocation batch for year-end retained profit posting |
| `spu_ACT_DO_YearEND_Allocations` | `PostYearEndAllocations` | Posts allocation transactions for year-end financial closing |
| `spu_GetTransactions_summary_YearEnd` | `GetBranchIDsForYearEnd` | Retrieves transaction summary data by period and branch for year-end reconciliation |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTPeriod.Form` | `GetPreviousPeriodEndComplete`, `ProcessPeriodEnd` | Period CRUD, get previous period ID, read/update period end status |
| `bACTBudget.Form` | `ProcessPeriodEndBudgets` | Updates budget actuals and variances at period end |
| `bACTDocumentPost.Form` | `ProcessRetainedProfitJournal`, `ProcessRetainedProfitJournalV2` | Posts journal documents and transactions |
| `bACTAutoNumber.Business` | `ProcessRetainedProfitJournal`, `ProcessRetainedProfitJournalV2` | Generates sequential document reference numbers |
| `bACTAllocationManual.Business` | `ProcessRetainedProfitJournal` | Auto-allocates journal entries against outstanding transactions |
| `bPMLookup.Business` | `Initialise` (Form) | Lookup value retrieval |

---

### 63. bACTPremiumFinance
**Directory:** `PremiumFinance/`
**Project:** `bACTPremiumFinance`
**Purpose:** Handles premium finance operations — posting debit/credit transactions for instalment-based premium finance, processing deposits, cancelling premium finance, allocating transactions, and managing finance accounts.

**Business Methods — Business (bACTPremiumFinanceBusiness.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing, Optional ByRef vSIRDatabase As Object = Nothing) As Integer` | Initialises the component with database and currency converter |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets optional process mode parameters |
| `PostTransactions` | `Public Function PostTransactions(ByVal v_lClientAccountID As Integer, ByVal v_lPremFinanceAccountID As Integer, ByVal v_cFinanceAmount As Decimal, ByRef r_lTransDetailID As Integer, ByVal v_iDaysDelay As Integer, ByVal v_iInstallments As Integer, ByVal v_cDeposit As Decimal, Optional ByVal v_vInterestCost As Object = Nothing, Optional ByRef r_lTransDetailID2 As Integer = 0, Optional ByVal v_sPlanReference As String = "", Optional ByVal v_sTransType As String = "") As Integer` | Posts debit/credit transactions for premium finance transfer |
| `TransactPremiumFinance` | `Public Function TransactPremiumFinance(ByVal v_lClientAccount As Integer, ByVal v_lPremFinanceAccount As Integer, ByVal v_cFinanceAmount As Decimal, ByVal v_vTransactionIDs(,) As Object, ByVal v_iCompanyID As Integer, ByVal v_iDaysDelay As Integer, ByVal v_iInstallments As Integer, ByVal v_cDeposit As Decimal, ByVal v_lDepositTransId As Integer, Optional ByVal v_vInterestCost As Object = Nothing, Optional ByVal v_sPlanReference As String = "", Optional ByVal v_lAgentCnt As Integer = 0, Optional ByVal v_lAgentType As Integer = 0, Optional ByRef r_lProviderTransDetailID As Integer = 0) As Integer` | Main method to transact premium finance — posts, allocates, and processes agent commissions |
| `TransactIts4meDeposit` | `Public Function TransactIts4meDeposit(ByVal v_lClientAccount As Integer, ByVal v_lPremFinanceAccount As Integer, ByVal v_cFinanceAmount As Decimal, ByVal v_vTransactionIDs As Object, ByVal v_iCompanyID As Integer, ByVal v_iDaysDelay As Integer, ByVal v_iInstallments As Integer, ByVal v_cDeposit As Decimal, ByRef v_lDepositTransId As Integer, Optional ByRef v_vInterestCost As Object = Nothing) As Integer` | Processes an ITS4ME deposit transaction |
| `CancelPremiumFinance` | `Public Function CancelPremiumFinance(ByVal v_lClientAccount As Integer, ByVal v_lPremFinanceAccount As Integer, ByVal v_cFinanceAmount As Decimal, ByVal v_vTransactionIDs() As Object, ByVal v_iCompanyID As Integer, ByVal v_iDaysDelay As Integer, ByVal v_iInstallments As Integer, ByVal v_cDeposit As Decimal, ByVal v_lDepositTransId As Integer, Optional ByVal v_vInterestCost As Object = Nothing, Optional ByVal v_sPlanReference As String = "", Optional ByRef r_lProviderTransDetailID As Integer = 0) As Integer` | Cancels a premium finance plan and reverses transactions |
| `Allocate` | `Public Function Allocate(ByVal v_lClientAccount As Integer, ByVal v_vTransaction As Object, ByVal v_vTransactions As Object) As Integer` | Allocates premium finance transactions |
| `GetAccountID` | `Public Function GetAccountID(ByRef r_lSupplierID As Integer, ByVal v_sShortCode As String, ByVal v_iCompanyID As Integer) As Integer` | Gets the account ID for a given short code and company |
| `Start` | `Public Function Start() As Integer` | Entry point for automated processing |

**Stored Procedures:**
| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| *(No SQL constants defined in this component — delegates to bACTDocumentPost, bACTAutoNumber, bACTAllocationManual, bACTTransdetail for database operations)* | | |
| `SPU_ACT_GET_ACCOUNTID_FROM_SHORTCODE` | `GetAccountID` | Retrieves account ID from a short code for multi-branch account handling |
| `spu_ACT_Get_FinanceAmount` | `GetFinanceAmount` | Gets finance amount for client transactions in base currency |
| `spu_ACT_get_client_for_sub_agent` | `GetClientTransactionForSubAgent` | Gets client transaction details for sub-agent processing |
| `spu_ACT_get_sub_agent` | `GetSubAgentAccountId` | Retrieves sub-agent account if it exists for a transaction |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTDocumentPost.Form` | `PostTransactions`, `TransactPremiumFinance` | Posts documents and transaction lines |
| `bACTAutoNumber.Business` | `PostTransactions`, `TransactPremiumFinance` | Generates document reference numbers |
| `bACTAllocationManual.Business` | `Allocate`, `TransactPremiumFinance` | Manual allocation of transactions |
| `bACTCurrencyConvert.Form` | `Initialise` | Currency conversion operations |
| `bACTTransdetail.Form` | *(field declared)* | Transaction detail operations |

---

### 64. bACTReleaseManualTransactions
**Directory:** `SuspendedTransactions/`
**Project:** `bACTReleaseManualTransactions`
**Purpose:** Batch process that releases manually suspended accounting transactions by invoking the bACTTransDetail.Form component in view/generic mode and calling `ReleaseManualTransactions`.

**Business Methods — MainModule (bACTReleaseManualTransactions.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Main` | `Public Sub Main()` | Entry point; creates bACTTransDetail.Form, initialises it, sets process modes, and calls `ReleaseManualTransactions()` |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| *(none directly)* | — | All SP calls are delegated to `bACTTransDetail.Form.ReleaseManualTransactions()` |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTTransDetail` | `Main` | Creates `bACTTransDetail.Form` instance to initialise and execute the release process |

---

### 65. bACTTransdetail
**Directory:** `TransDetail/`
**Project:** `bACTTransdetail`
**Purpose:** Manages transaction detail records — the individual line items within accounting documents. Supports CRUD operations, collection-based editing, suspended/released transaction management, premium finance posting, and transaction cache.

**Business Methods — Form (bACTTransdetailForm.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises database, lookup, and collections |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set task, navigate, process mode, transaction type, effective date |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Navigator SetKeys function |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Navigator GetKeys function |
| `GetSummary` | `Public Function GetSummary(ByRef vSummaryArray As Object) As Integer` | Navigator GetSummary function |
| `Start` | `Public Function Start() As Integer` | Navigator Start entry point |
| `DirectAdd` | `Public Function DirectAdd(Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing, ...[42 optional params]..., Optional ByVal oFeeType As Object = Nothing) As Integer` | Adds a single transdetail directly to database (not to collection) |
| `DirectDelete` | `Public Function DirectDelete(ByRef vTransdetailID As Object) As Integer` | Deletes a single transdetail directly from database |
| `GetDefaults` | `Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, ...[35 optional params]...) As Integer` | Returns default values for transdetail fields |
| `CheckID` | `Public Function CheckID(ByRef vID As Object) As Integer` | Validates a transdetail ID exists |
| `GetCaptions` | `Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object) As Integer` | Gets caption field values for a record |
| `GetCaptions` | `Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, ByRef vTable As Object) As Integer` | Gets caption field values with table override |
| `GetDetails` | `Public Function GetDetails(Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vDocumentID As Object = Nothing, Optional ByRef vOSAmounts As Object = Nothing, Optional ByRef vLockMode As Object = Nothing) As Integer` | Loads transdetails into collection by ID or document |
| `GetNext` | `Public Function GetNext(Optional ByRef vTransdetailID As Object = Nothing, ...[45 optional params]...) As Integer` | Iterates collection returning next transdetail's properties |
| `GetLookupValues` | `Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer` | Gets lookup values (currency list) |
| `EditAdd` | `Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vTransdetailID As Object = Nothing, ...[38 optional params]...) As Integer` | Adds a transdetail to the collection |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vTransdetailID As Object = Nothing, ...[37 optional params]...) As Integer` | Updates a transdetail in the collection |
| `EditDelete` | `Public Function EditDelete(ByVal lRow As Integer) As Integer` | Marks a transdetail in collection for deletion |
| `Cancel` | `Public Function Cancel() As Integer` | Checks if collection has unsaved changes |
| `Update` | `Public Function Update() As Integer` | Commits all pending adds/updates/deletes in collection to database |
| `LoadTransCache` | `Public Function LoadTransCache(ByVal v_lUserID As Integer, ByVal v_lSourceID As Integer, ByRef r_lAccountID As Integer, ByRef r_lCurrencyID As Integer, ByRef r_dAmount As Double, ByRef r_sComment As String, ByRef r_dBaseAmount As Double, ByRef r_dExchangeRate As Double, ByRef r_dAmountInEuros As Double, ByRef r_lDepartmentID As Integer, ByRef r_sInsuranceRef As String, ByRef r_sPurchaseOrderNo As String, ByRef r_sPurchaseInvoiceNo As String) As Integer` | Loads cached transaction entry from database |
| `SaveTransCache` | `Public Function SaveTransCache(ByVal v_lUserID As Integer, ByVal v_lSourceID As Integer, ByVal v_lAccountID As Integer, ByVal v_lCurrencyID As Integer, ByVal v_dAmount As Double, ByVal v_sComment As String, ByVal v_dBaseAmount As Double, ByVal v_dExchangeRate As Double, ByVal v_dAmountInEuros As Double, ByVal v_lDepartmentID As Integer, ByVal v_sInsuranceRef As String, ByVal v_sPurchaseOrderNo As String, ByVal v_sPurchaseInvoiceNo As String) As Integer` | Saves cached transaction entry to database |
| `PostSuspendedTransaction` | `Public Function PostSuspendedTransaction(ByVal v_lPremiumFinanceCnt As Integer, ByVal v_lPremiumFinanceVersion As Integer, ByVal v_lAccountID As Integer, ByVal v_lTransdetailID As Integer, ByVal v_lTransactionType As Integer) As Integer` | Posts a transaction to PF_Accounts_transactions table |
| `PostSuspendedTransaction` | `Public Function PostSuspendedTransaction(ByVal v_lPremiumFinanceCnt As Integer, ByVal v_lPremiumFinanceVersion As Integer, ByVal v_lAccountID As Integer, ByVal v_lTransdetailID As Integer, ByVal v_lTransactionType As Integer, ByVal v_sSpare As String) As Integer` | Posts suspended transaction with spare field |
| `GetTransDetailTypeCode` | `Public Function GetTransDetailTypeCode(ByVal lTransdetailTypeID As Integer, ByRef sTransdetailTypeCode As String) As Integer` | Gets transdetail type code from transdetail_type table |
| `MoveTransactionToSuspense` | `Public Function MoveTransactionToSuspense(ByVal lTransdetailID As Integer, ByVal lDestinationAccountID As Integer, ByVal lLinkedTransdetailID As Integer, ByVal dLinkedPercentage As Double) As Integer` | Moves a transaction to suspense account via journal |
| `MoveTransactionToSuspense` | `Public Function MoveTransactionToSuspense(ByVal lTransdetailID As Integer, ByVal lDestinationAccountID As Integer, ByVal lLinkedTransdetailID As Integer, ByVal dLinkedPercentage As Double, ByVal vPFPremiumFinanceCnt As Object, ByVal vPFPremiumFinanceVersion As Object) As Integer` | Moves transaction to suspense with premium finance context |
| `ReleaseSuspendedTransactions` | `Public Function ReleaseSuspendedTransactions(ByVal lAllocationId As Integer) As Integer` | Releases suspended transactions (multiple overloads with optional params for linked ID, accounting date, percentage, amount, insurance file, instalment) |
| `CreateSuspendedTransaction` | `Public Function CreateSuspendedTransaction(ByVal lSuspendedTransdetailId As Integer, ByVal lLinkedTransdetailID As Integer, ByVal dLinkedPercentage As Double, ByVal vPFPremFinanceCnt As Object, ByVal vPFPremFinanceVersion As Object, ByVal lInsuranceFileCnt As Integer, ByVal lDestinationAccountID As Integer, ByVal lDocumentTypeId As Integer, ByVal lTransdetailTypeID As Integer, ByVal sSpare As String) As Integer` | Creates a suspended transaction record |
| `CreateReleasedTransaction` | `Public Function CreateReleasedTransaction(ByVal lSuspendedTransdetailId As Integer, ...) As Integer` | Creates a released transaction record |
| `RewriteSuspendedTransactions` | `Public Function RewriteSuspendedTransactions(ByVal lOldTriggerTransdetailID As Integer) As Integer` | Rewrites suspended transactions with new trigger |
| `FinanceSuspendedTransactions` | `Public Function FinanceSuspendedTransactions(ByVal lOldTriggerTransdetailID As Integer, ByVal vPlanTransdetailId As Integer) As Integer` | Finances suspended transactions (multiple overloads) |
| `RecallReleasedTransaction` | `Public Function RecallReleasedTransaction() As Integer` | Recalls a released transaction (multiple overloads) |
| `ReleaseManualTransactions` | `Public Function ReleaseManualTransactions() As Integer` | Releases manually suspended transactions |
| `CheckSuspendedAllocation` | `Public Function CheckSuspendedAllocation(ByVal lLinkedTransdetailID As Integer, ByRef lAllocationId As Integer) As Integer` | Checks if a suspended allocation exists |
| `GetRiskTransferStatus` | `Public Function GetRiskTransferStatus(ByVal lAccountID As Integer, ByVal lDocumentID As Integer, ByVal cAmount As Decimal) As gPMConstants.PMEReturnCode` | Gets risk transfer agreement status for an account |
| `GetAllocationbatch` | `Public Function GetAllocationbatch(ByVal v_nAllocationId As Integer, ...) As Integer` | Gets allocation batch by allocation ID |

**Business Methods — Transdetail (bACTTransdetailCls.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises the transdetail entity object |

**Business Methods — Transdetails Collection (bACTTransdetails.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Add` | `Public Function Add(ByRef oNewTransdetail As bACTTransdetail.Transdetail) As Integer` | Adds a transdetail to the collection |
| `Count` | `Public Function Count() As Integer` | Returns the number of transdetails in the collection |
| `Delete` | `Public Sub Delete(ByRef vKey As Integer)` | Deletes a transdetail from the collection by key |
| `Item` | `Public Function Item(ByRef vKey As Integer) As Transdetail` | Returns a transdetail by key |
| `DeleteAll` | `Public Sub DeleteAll()` | Deletes all transdetails from the collection |
| `Clear` | `Public Sub Clear()` | Clears and reinitialises the collection |
| `Initialise` | `Public Function Initialise(ByRef sUserName As String, ...) As Integer` | Initialises the collection |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_ACT_select_TransDetail` | `GetDetails` | Select a single transdetail by ID |
| `spu_ACT_selall_TransDetail_doc` | `GetDetails` | Select all transdetails by document ID |
| `spu_ACT_check_TransDetail` | `CheckID` | Check if a transdetail ID exists |
| `spu_ACT_add_TransDetail` | `DirectAdd`, `Update` | Add a transdetail record |
| `spu_ACT_delete_TransDetail` | `DirectDelete`, `Update` | Delete a transdetail record |
| `spu_ACT_update_TransDetail` | `Update` | Update a transdetail record |
| `spu_transdetail_cache_sel` | `LoadTransCache` | Select cached transaction entry |
| `spu_transdetail_cache_upd` | `SaveTransCache` | Update cached transaction entry |
| `spu_ACT_SuspendedAccountsTransactions_Add` | `CreateSuspendedTransaction` | Add suspended accounts transaction |
| `spu_ACT_SuspendedAccountsTransactions_Sel` | `ReleaseSuspendedTransactions` | Select suspended transactions |
| `spu_ACT_IsSuspendedTransactionPosted` | `CheckSuspendedAllocation` | Check if suspended transaction posted |
| `spu_ACT_SuspendedAccountsTransactions_Rewrite` | `RewriteSuspendedTransactions` | Rewrite suspended transactions |
| `spu_ACT_SuspendedAccountsTransactionsAllocation_Sel` | `ReleaseSuspendedTransactions` | Select suspended allocation transactions |
| `spu_ACT_ReleasedAccountsTransactions_Add` | `CreateReleasedTransaction` | Add released transaction record |
| `spu_ACT_ReleasedAccountsTransactions_Sel` | `RecallReleasedTransaction` | Select released transactions |
| `spu_ACT_GetAllocationPart` | `FinanceSuspendedTransactions` | Get allocation part for partial movement |
| `spu_ACT_SuspendedAccountsTransactions_Finance` | `FinanceSuspendedTransactions` | Finance suspended transactions |
| `spu_TRN_risk_transfer_status_select` | `GetRiskTransferStatus` | Get risk transfer status for account |
| `spu_Act_Get_InsuranceFileCnt` | Form internals | Get insurance file count |
| `spu_PFAccountsTransactions_Add` | `PostSuspendedTransaction` | Post to PF_Accounts_transactions table |
| `spu_ACT_GetSetReleaseManualTransProcessFlag` | `ReleaseManualTransactions` | Get/set release manual transactions process flag |
| `spu_ACT_ManualSuspendedAccountsTransactions_Sel` | `ReleaseManualTransactions` | Select manual suspended account transactions |
| `spu_act_get_allocation_batch_by_allocation` | `GetAllocationbatch` | Get allocation batch by allocation ID |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTDocument` | `MoveTransactionToSuspense` | Get document details for insurance file ref |
| `bACTDocumentPost` | `MoveTransactionToSuspense` | Create journal documents and add transactions |
| `bACTAllocationManual` | `MoveTransactionToSuspense` | Perform allocations between original and suspense transactions |
| `bACTAutoNumber` | Form class | Auto-number generation |
| `bACTCurrencyConvert` | Form class | Currency conversion operations |
| `bACTDocumentReversal` | Form class | Document reversal operations |
| `bACTAllocation` | Form class | Allocation operations |

---

### 66. bACTTransmatch
**Directory:** `TransMatch/`
**Project:** `bACTTransmatch`
**Purpose:** Manages transaction match records that link allocation details to transaction details with match amounts. Supports CRUD operations, collection-based editing, and matching amounts in base and currency.

**Business Methods — Form (bACTTransmatchForm.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises database connection and collection |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set task, navigate, process mode, transaction type, effective date |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Navigator SetKeys function |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray As String) As Integer` | Navigator GetKeys (string overload) |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Navigator GetKeys (array overload) |
| `GetSummary` | `Public Function GetSummary(ByRef vSummaryArray As Object) As Integer` | Navigator GetSummary function |
| `Start` | `Public Function Start() As Integer` | Navigator Start entry point |
| `DirectAdd` | `Public Function DirectAdd(Optional ByRef vTransmatchID As Integer = 0, Optional ByRef vAllocationdetailID As Object = Nothing, Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vMatchID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vBaseMatchAmount As Object = Nothing, Optional ByRef vCurrencyMatchAmount As Object = Nothing, Optional ByRef vCurrencyMatchXrate As Object = Nothing) As Integer` | Adds a transmatch directly to database |
| `DirectDelete` | `Public Function DirectDelete(Optional ByRef vTransmatchID As Object = Nothing, Optional ByRef vAllocationdetailID As Object = Nothing, Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vMatchID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vBaseMatchAmount As Object = Nothing, Optional ByRef vCurrencyMatchAmount As Object = Nothing, Optional ByRef vCurrencyMatchXrate As Object = Nothing) As Integer` | Deletes a transmatch directly from database |
| `GetDefaults` | `Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vTransmatchID As Object = Nothing, Optional ByRef vAllocationdetailID As Object = Nothing, Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vMatchID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vBaseMatchAmount As Object = Nothing, Optional ByRef vCurrencyMatchAmount As Object = Nothing, Optional ByRef vCurrencyMatchXrate As Object = Nothing) As Integer` | Returns default values for transmatch |
| `CheckID` | `Public Function CheckID(ByRef vID As Object) As Integer` | Validates a transmatch ID exists |
| `GetDetails` | `Public Function GetDetails(Optional ByRef vTransmatchID As Object = Nothing, Optional ByRef vLockMode As Integer = 0) As Integer` | Loads transmatches into collection |
| `GetNext` | `Public Function GetNext(Optional ByRef vTransmatchID As Object = Nothing, Optional ByRef vAllocationdetailID As Object = Nothing, Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vMatchID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vBaseMatchAmount As Object = Nothing, Optional ByRef vCurrencyMatchAmount As Object = Nothing, Optional ByRef vCurrencyMatchXrate As Object = Nothing) As Integer` | Iterates collection returning next transmatch's properties |
| `EditAdd` | `Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vTransmatchID As Object = Nothing, Optional ByRef vAllocationdetailID As Object = Nothing, Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vMatchID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vBaseMatchAmount As Object = Nothing, Optional ByRef vCurrencyMatchAmount As Object = Nothing, Optional ByRef vCurrencyMatchXrate As Object = Nothing) As Integer` | Adds a transmatch to the collection |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vTransmatchID As Object = Nothing, Optional ByRef vAllocationdetailID As Object = Nothing, Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vMatchID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vBaseMatchAmount As Object = Nothing, Optional ByRef vCurrencyMatchAmount As Object = Nothing, Optional ByRef vCurrencyMatchXrate As Object = Nothing) As Integer` | Updates a transmatch in the collection |
| `EditDelete` | `Public Function EditDelete(ByVal lRow As Integer) As Integer` | Marks a transmatch for deletion |
| `Cancel` | `Public Function Cancel() As Integer` | Checks if collection has unsaved changes |
| `Update` | `Public Function Update() As Integer` | Commits all pending changes to database |

**Business Methods — Transmatch (bACTTransmatch.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises the transmatch entity object |

**Business Methods — Transmatches Collection (bACTTransmatchs.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Add` | `Public Function Add(ByRef oNewTransmatch As bACTTransmatch.Transmatch) As Integer` | Adds a transmatch to the collection |
| `Count` | `Public Function Count() As Integer` | Returns the number of transmatches |
| `Delete` | `Public Sub Delete(ByRef vKey As Integer)` | Deletes a transmatch by key |
| `Item` | `Public Function Item(ByRef vKey As Integer) As bACTTransmatch.Transmatch` | Returns a transmatch by key |
| `DeleteAll` | `Public Sub DeleteAll()` | Deletes all transmatches |
| `Clear` | `Public Sub Clear()` | Clears and reinitialises the collection |

**Business Methods — Automated (bACTTransmatchAutomated.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises automated class with database |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets process modes |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Navigator SetKeys |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Navigator GetKeys |
| `Start` | `Public Function Start() As Integer` | Performs automated action |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_ACT_select_TransMatch` | `GetDetails` (Form & Automated) | Select a single transmatch by ID |
| `spu_ACT_select_all_TransMatch` | `GetDetails` (Form & Automated) | Select all transmatches |
| `spu_ACT_check_TransMatch` | `CheckID` (Form & Automated) | Check if a transmatch ID exists |
| `spu_ACT_add_TransMatch` | `DirectAdd`, `Update` (Form & Automated) | Add a transmatch record |
| `spu_ACT_delete_TransMatch` | `DirectDelete`, `Update` (Form & Automated) | Delete a transmatch record |
| `spu_ACT_update_TransMatch` | `Update` (Form & Automated) | Update a transmatch record |
| `spu_act_select_transdetail_OS` | Form internals | Select outstanding amount on transaction |
| `spu_ACT_select_transdetail_prm` | Form internals | Select the primary transaction of a set |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| *(none)* | — | Self-contained; references only its own Transmatch/Transmatches classes |

---

### 67. bACTTypeTable
**Directory:** `TypeTable/`
**Project:** `bACTTypeTable`
**Purpose:** Manages type table reference data records with code, description, effective date, and deletion status. Used for configurable lookup/classification types in the accounting system.

**Business Methods — Form (bACTTypeTableForm.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises database and collections |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets process modes |
| `DirectAdd` | `Public Function DirectAdd(Optional ByRef vTypeTableID As Integer = 0, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vCode As Object = Nothing) As Integer` | Adds a type table record directly to database |
| `DirectDelete` | `Public Function DirectDelete(Optional ByRef vTypeTableID As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vCode As Object = Nothing) As Integer` | Deletes a type table record directly |
| `GetDefaults` | `Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vTypeTableID As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vCode As Object = Nothing) As Integer` | Returns default values |
| `CheckID` | `Public Function CheckID(ByRef vID As Object) As Integer` | Validates a type table ID exists |
| `GetCaptions` | `Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, Optional ByRef vTable As Object = Nothing) As Integer` | Gets caption field values |
| `GetDetails` | `Public Function GetDetails(Optional ByRef vTypeTableID As Object = Nothing, Optional ByRef vLockMode As gPMConstants.PMELockMode = 0) As Integer` | Loads type tables into collection |
| `GetNext` | `Public Function GetNext(Optional ByRef vTypeTableID As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vCode As Object = Nothing) As Integer` | Iterates collection returning next type table |
| `GetLookupValues` | `Public Function GetLookupValues(ByRef dtEffectiveDate As Date, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer` | Gets lookup values for type table |
| `EditAdd` | `Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vTypeTableID As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vCode As Object = Nothing) As Integer` | Adds a type table to the collection |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vTypeTableID As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vCode As Object = Nothing) As Integer` | Updates a type table in the collection |
| `EditDelete` | `Public Function EditDelete(ByVal lRow As Integer) As Integer` | Marks a type table for deletion |
| `Cancel` | `Public Function Cancel() As Integer` | Checks for unsaved changes |
| `Update` | `Public Function Update() As Integer` | Commits all pending changes |
| `DeleteAllItems` | `Public Function DeleteAllItems() As Integer` | Deletes all type table items |

**Business Methods — TypeTable (bACTTypeTable.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises the type table entity |

**Business Methods — TypeTables Collection (bACTTypeTables.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Add` | `Public Function Add(ByRef oNewTypeTable As bACTTypeTable.TypeTable) As Integer` | Adds a type table to the collection |
| `Count` | `Public Function Count() As Integer` | Returns count |
| `Delete` | `Public Sub Delete(ByRef vKey As Integer)` | Deletes by key |
| `Item` | `Public Function Item(ByRef vKey As Integer) As bACTTypeTable.TypeTable` | Returns item by key |
| `DeleteAll` | `Public Sub DeleteAll()` | Deletes all items |
| `Clear` | `Public Sub Clear()` | Clears collection |

**Business Methods — Automated (bACTTypeTableAutomated.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises automated class |
| `SetProcessModes` | `Public Function SetProcessModes(...) As Integer` | Sets process modes |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Navigator SetKeys |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Navigator GetKeys |
| `Start` | `Public Function Start() As Integer` | Performs automated action |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_ACT_select_TypeTable` | `GetDetails` | Select a single type table by ID |
| `spu_ACT_SelAll_TypeTable` | `GetDetails` | Select all type table records |
| `spu_ACT_check_TypeTable` | `CheckID` | Check if a type table ID exists |
| `spu_ACT_add_TypeTable` | `DirectAdd`, `Update` | Add a type table record |
| `spu_ACT_delete_TypeTable` | `DirectDelete`, `Update` | Delete a type table record |
| `spu_ACT_delAll_TypeTable` | `DeleteAllItems` | Delete all type table records |
| `spu_ACT_update_TypeTable` | `Update` | Update a type table record |
| `spu_ACT_add_Write_Off_Reason` | `Add (Automated)` | Inserts a new write-off reason record (automated variant using lowercase-a naming) |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| *(none)* | — | Self-contained; references only its own TypeTable/TypeTables classes |

---

### 68. bACTUserAuthorities
**Directory:** `UserAuthorities/`
**Project:** `bACTUserAuthorities`
**Purpose:** Manages user-level authority settings for the accounting system — write-off limits, payment authorities, refund/transfer authorities, party view permissions, claim payment controls, policy edit permissions, posting period overrides, and various feature-level access controls.

**Business Methods — Business (bACTUserAuthoritiesBusiness.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises database and collections |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets process modes |
| `DirectAdd` | `Public Function DirectAdd(Optional ByRef vUserID As Object = Nothing, Optional ByRef vHasWriteOffAuthority As Object = Nothing, ...[25+ optional params]..., Optional ByRef vHasViewBatchProcessStatus As Object = Nothing) As Integer` | Adds user authorities record directly to database |
| `DirectDelete` | `Public Function DirectDelete() As Integer` | Deletes current user authorities |
| `DirectDelete` | `Public Function DirectDelete(ByRef vUserID As Object) As Integer` | Deletes user authorities by user ID |
| `CheckID` | `Public Function CheckID(ByRef vID As Object) As Integer` | Validates a user authorities record exists |
| `GetDetails` | `Public Function GetDetails(Optional ByRef vLockMode As Integer = 0, Optional ByRef vUserID As Integer = 0) As Integer` | Loads user authorities into collection |
| `GetNext` | `Public Function GetNext(Optional ByRef vUserID As Object = Nothing, Optional ByRef vHasWriteOffAuthority As Object = Nothing, ...[60+ optional params]..., Optional ByRef vVoidTransaction As String = "") As Integer` | Iterates collection returning next record's properties |
| `EditAdd` | `Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vUserID As Object = Nothing, ...[40+ optional params]..., Optional ByRef vVoidTransaction As String = "") As Integer` | Adds user authorities to collection |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vUserID As Object = Nothing, ...[60+ optional params]..., Optional ByRef vVoidTransaction As String = "") As Integer` | Updates user authorities in collection |
| `EditDelete` | `Public Function EditDelete(ByVal lRow As Integer) As Integer` | Marks user authorities for deletion |
| `Cancel` | `Public Function Cancel() As Integer` | Checks for unsaved changes |
| `Update` | `Public Function Update() As Integer` | Commits all pending changes |
| `Terminate` | `Public Function Terminate() As Integer` | Terminates and cleans up resources |
| `ValidateAmounts` | `Public Function ValidateAmounts(ByVal v_iCurrencyID As Integer, ByVal v_cAmount As Decimal, ByVal v_lCompanyID As Integer, ByRef r_vTransWriteOffValid As Object) As Integer` | Validates write-off/payment amounts against authority limits (3 overloads with increasing specificity) |
| `GetUserAuthoritiesDetails` | `Public Function GetUserAuthoritiesDetails(ByVal v_lUserId As Integer, ByRef r_vResults(,) As Object) As Integer` | Gets user authorities details for a specific user |
| `GetUserWriteOffDetails` | `Public Function GetUserWriteOffDetails(ByVal user_id As Integer, ByRef vResult As Object) As Integer` | Gets user write-off authority details |
| `GetPartyViewOptions` | `Public Function GetPartyViewOptions(ByVal v_lUserId As Integer, ByRef r_bIsViewOnlyClientManager As Boolean) As Integer` | Gets party view options (client manager only) |
| `GetPartyViewOptions` | `Public Function GetPartyViewOptions(ByVal v_lUserId As Integer, ByRef r_bIsViewOnlyClientManager As Boolean, ByRef r_bIsViewOnlyAgentMaintenance As Boolean, ByRef r_bIsViewOnlyAccountHandlerMaintenance As Boolean, ByRef r_bIsViewOnlyAccountExecutiveMaintenace As Boolean, ByRef r_bIsViewOnlyInsurerMaintenance As Boolean, ByRef r_bIsViewOnlyOtherPartyMaintenance As Boolean) As Integer` | Gets all party view options |
| `GetOption` | `Public Function GetOption(ByVal v_iOptionNumber As Integer, ByRef r_sOptionValue As String, ByVal v_iCompany As Integer) As Integer` | Gets a system option value |
| `GetValueFromTable` | `Public Function GetValueFromTable(ByVal v_sTableName As String, ByVal v_vReturnColumn As Object, ByVal v_sKeyColumn As String, ByVal v_sKeyValue As String, ByVal v_iDataType As Integer, ByRef r_vResult As Object) As Integer` | Generic lookup: gets a single value from any table |
| `CheckIsRecommendClaimPaymentEnabledatProduct` | `Public Function CheckIsRecommendClaimPaymentEnabledatProduct(ByRef r_vResults(,) As Object) As Integer` | Checks if claim payment recommendation is enabled at product level |

**Business Methods — ACTUserAuthorities Entity (bACTUserAuthoritiesCls.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ...) As Integer` | Initialises entity object |
| `GetDefaults` | `Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, ...[35+ optional params]...) As Integer` | Returns default values |
| `GetProperties` | `Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vUserID As Object = Nothing, ...) As Integer` | Gets all property values from entity |
| `SetProperties` | `Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vUserID As Object = Nothing, ...) As Integer` | Sets all property values on entity |
| `SetPropertiesFromDB` | `Public Function SetPropertiesFromDB(ByRef oFields As DataRow) As Integer` | Sets properties from database row |
| `SelectItem` | `Public Function SelectItem() As Integer` | Selects item from database |
| `AddItem` | `Public Function AddItem() As Integer` | Adds item to database |
| `UpdateItem` | `Public Function UpdateItem() As Integer` | Updates item in database |
| `DeleteItem` | `Public Function DeleteItem() As Integer` | Deletes item from database |

**Business Methods — ACTUserAuthoritiess Collection (bACTUserAuthoritiess.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Add` | `Public Function Add(ByRef oNewACTUserAuthorities As bACTUserAuthorities.ACTUserAuthorities) As Integer` | Adds to collection |
| `Count` | `Public Function Count() As Integer` | Returns count |
| `Delete` | `Public Sub Delete(ByRef vKey As Integer)` | Deletes by key |
| `Item` | `Public Function Item(ByRef vKey As Integer) As bACTUserAuthorities.ACTUserAuthorities` | Returns item by key |
| `DeleteAll` | `Public Sub DeleteAll()` | Deletes all |
| `Clear` | `Public Sub Clear()` | Clears collection |
| `Initialise` | `Public Function Initialise() As Integer` | Initialises collection |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_ACT_SelAll_UserAuthorities` | `GetDetails` | Select all user authorities records |
| `spe_ACTUserAuthorities_check_id` | `CheckID` | Check if a user authorities record exists |
| `spe_User_Authorities_sel` | `GetUserAuthoritiesDetails`, `SelectItem` | Select single user authority record |
| `spe_User_Authorities_add` | `DirectAdd`, `AddItem` | Add user authority record |
| `spe_User_Authorities_del` | `DirectDelete`, `DeleteItem` | Delete user authority record |
| `spe_User_Authorities_upd` | `Update`, `UpdateItem` | Update user authority record |
| `spu_ACT_GetPartyViewOptions` | `GetPartyViewOptions` | Get party view options for a user |
| `spu_Check_IsRecommendClaimPaymentEnabledatProduct` | `CheckIsRecommendClaimPaymentEnabledatProduct` | Check if claim payment recommendation enabled at product |

**Module Constants (bACTUserAuthorities.vb — MainModule):**

| Constant | Value | Description |
|----------|-------|-------------|
| `kParamsCount` | `26` | Total parameter array count |
| `ACIsRecommenderArrPos` | `0` | Is recommender array position |
| `ACRecommendationCurrencyArrPos` | `1` | Recommendation currency position |
| `ACRecommendationAmountArrPos` | `2` | Recommendation amount position |
| `ACCanReverseAllocationArrPos` | `3` | Can reverse allocation position |
| `ACTimePeriodForReversalArrPos` | `4` | Time period for reversal |
| `ACCanReverseReplaceArrPos` | `5` | Can reverse/replace position |
| `ACMTAAuthorityArrPos` | `6` | MTA authority position |
| `ACChequeNumberArrPos` | `7` | Cheque number position |
| `ACDisplayReinsuranceScreen` | `8` | Display reinsurance screen |
| `ACDisplayClaimReinsurance` | `9` | Display claim reinsurance |
| `ACMakeLiveBankGuarantee` | `10` | Make live bank guarantee |
| `ACCanBackdateCollectionDate` | `11` | Can backdate collection date |
| `ACEditDefaultCommission` | `12` | Edit default commission |
| `ACMakeLiveCashDeposit` | `13` | Make live cash deposit |
| `ACUserCanDebugDynamicLogicScripts` | `14` | User can debug dynamic logic |
| `ACUserServerScriptsRunInDebug` | `15` | Server scripts run in debug |
| `kEditDefaultCommissionNBRN` | `16` | Edit default commission NB/RN |
| `kEditDefaultCommissionMTA` | `17` | Edit default commission MTA |
| `kEditDefaultCommissionMTR` | `18` | Edit default commission MTR |
| `kEditDefaultCommissionMTC` | `19` | Edit default commission MTC |
| `kEditAgentDuringMTAMTC` | `20` | Edit agent during MTA/MTC |
| `ACCanReverseReceiptArrPos` | `21` | Can reverse receipt |
| `kACCViewBatchProcessStatus` | `22` | View batch process status |
| `ACUserCanChangeInstalmentDefaultCurrency` | `23` | Change instalment default currency |
| `ACInstalmentStatus` | `24` | Instalment status |
| `ACCanEditInstalmentDueDate` | `25` | Can edit instalment due date |
| `ACEditInstalmentDateByNoOfDays` | `26` | Edit instalment date by no of days |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| `bACTCurrencyConvert` | `ValidateAmounts` | Currency conversion for authority amount validation |
| `bACTCurrency` | Business class | Currency lookup operations |

---

### 69. bACTWriteOffReason
**Directory:** `WriteOffReason/`
**Project:** `bACTWriteOffReason`
**Purpose:** Manages write-off reason reference data with code, description, effective date, and deletion status. Provides lookup capabilities for write-off reason selection and ID/code matching.

**Business Methods — Form (bACTWriteOffReasonForm.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Initialises database and collections |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Sets process modes |
| `DirectAdd` | `Public Function DirectAdd(Optional ByRef vWriteOffReasonID As Integer = 0, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vCode As Object = Nothing) As Integer` | Adds a write-off reason directly to database |
| `DirectDelete` | `Public Function DirectDelete(Optional ByRef vWriteOffReasonID As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vCode As Object = Nothing) As Integer` | Deletes a write-off reason directly |
| `GetDefaults` | `Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vWriteOffReasonID As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vCode As Object = Nothing) As Integer` | Returns default values |
| `CheckID` | `Public Function CheckID(ByRef vID As Object) As Integer` | Validates a write-off reason ID exists |
| `GetCaptions` | `Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, Optional ByRef vTable As Object = Nothing) As Integer` | Gets caption field values |
| `GetDetails` | `Public Function GetDetails(Optional ByRef vWriteOffReasonID As Object = Nothing, Optional ByRef vLockMode As gPMConstants.PMELockMode = 0) As Integer` | Loads write-off reasons into collection |
| `GetNext` | `Public Function GetNext(Optional ByRef vWriteOffReasonID As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vCode As Object = Nothing) As Integer` | Iterates collection returning next record |
| `EditAdd` | `Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vWriteOffReasonID As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vCode As Object = Nothing) As Integer` | Adds a write-off reason to collection |
| `EditUpdate` | `Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vWriteOffReasonID As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vCode As Object = Nothing) As Integer` | Updates a write-off reason in collection |
| `EditDelete` | `Public Function EditDelete(ByVal lRow As Integer) As Integer` | Marks a write-off reason for deletion |
| `Cancel` | `Public Function Cancel() As Integer` | Checks for unsaved changes |
| `Update` | `Public Function Update() As Integer` | Commits all pending changes |
| `GetReasons` | `Public Function GetReasons(ByRef vReasonList(,) As Object) As Integer` | Gets all write-off reasons as a list |
| `GetReasonAndCode` | `Public Function GetReasonAndCode(ByRef vReasonID As Object, ByRef vReason As String, ByRef vCode As String) As gPMConstants.PMEReturnCode` | Gets reason description and code by ID |
| `MatchIDWithCode` | `Public Function MatchIDWithCode(ByRef vCode As Object, ByRef vID As Object) As Integer` | Matches a write-off reason ID with its code |

**Business Methods — WriteOffReason (bACTWriteOffReason.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Initialises entity object |

**Business Methods — WriteOffReasons Collection (bACTWriteOffReasons.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Add` | `Public Function Add(ByRef oNewWriteOffReason As bACTWriteOffReason.WriteOffReason) As Integer` | Adds a write-off reason to collection |
| `Count` | `Public Function Count() As Integer` | Returns count |
| `Delete` | `Public Sub Delete(ByRef vKey As Integer)` | Deletes by key |
| `Item` | `Public Function Item(ByRef vKey As Integer) As bACTWriteOffReason.WriteOffReason` | Returns item by key |
| `DeleteAll` | `Public Sub DeleteAll()` | Deletes all items |
| `Clear` | `Public Sub Clear()` | Clears collection |

**Business Methods — Automated (bACTWriteOffReasonAutomated.vb):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As dPMDAO.Database = Nothing) As Integer` | Initialises automated class |
| `SetProcessModes` | `Public Function SetProcessModes(...) As Integer` | Sets process modes |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Navigator SetKeys |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Navigator GetKeys |
| `Start` | `Public Function Start() As Integer` | Performs automated action |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_ACT_select_Write_Off_Reason` | `GetDetails` | Select a single write-off reason by ID |
| `spu_ACT_select_all_Write_Off_Reason` | `GetDetails`, `GetReasons` | Select all write-off reasons |
| `spu_ACT_check_Write_Off_Reason` | `CheckID` | Check if a write-off reason ID exists |
| `spu_ACT_Add_Write_Off_Reason` | `DirectAdd`, `Update` | Add a write-off reason record |
| `spu_ACT_delete_Write_Off_Reason` | `DirectDelete`, `Update` | Delete a write-off reason record |
| `spu_ACT_update_Write_Off_Reason` | `Update` | Update a write-off reason record |

**Component References:**

| Component | Used By | Purpose |
|-----------|---------|---------|
| *(none)* | — | Self-contained; references only its own WriteOffReason/WriteOffReasons classes |
