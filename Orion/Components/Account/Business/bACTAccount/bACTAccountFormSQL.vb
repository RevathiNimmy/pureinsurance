Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Module FormSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: FormSQL
	'
	' Date: 22-07-1997
	'
	' Description: Contains the SQL Statements required by the
	'              bAccount.Form class.
	'
	' Edit History:
	' RAW 17/12/2002 : PS187 : Added ProofListReportID and BordereauReportID
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select Account SQL
	Public Const ACGetDetailsStored As Boolean = True
	Public Const ACGetDetailsName As String = "SelectAllAccount"
	Public Const ACGetDetailsSQL As String = "spu_ACT_select_Account"
	
	' Select All Account SQL
	Public Const ACGetAllDetailsStored As Boolean = True
	Public Const ACGetAllDetailsName As String = "SelectAllAccount"
	Public Const ACGetAllDetailsSQL As String = "spu_ACT_selall_Account"
	
	' Select All Account SQL
	Public Const ACGetLedgerDetailsStored As Boolean = True
	Public Const ACGetLedgerDetailsName As String = "SelectAllLedgers"
	Public Const ACGetLedgerDetailsSQL As String = "spu_ACT_selall_Ledger"
	
	' Check ID SQL
	Public Const ACCheckIDStored As Boolean = True
	Public Const ACCheckIDName As String = "CheckAccountID"
	Public Const ACCheckIDSQL As String = "spu_ACT_check_Account"
	
	'Check ShortCode Sql
	Public Const ACCheckCodeStored As Boolean = True
	Public Const ACCheckCodeName As String = "CheckAccountCode"
	Public Const ACCheckCodeSQL As String = "spu_ACT_Add_WriteOffAccount"
	
	' Add Account SQL
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "AddAccount"
	' RAW 17/12/2002 : PS187 : increase number of params by 2
	' RDC 12112003 added parm for AllowElectronicPayment
	Public Const ACAddSQL As String = "spu_ACT_add_Account"
	'                                                 0102030405060708091001020304050607080910010203040506070809100102030405060708091001020304050607080910
	' Delete Account SQL
	Public Const ACDeleteStored As Boolean = True
	Public Const ACDeleteName As String = "DeleteAccount"
	Public Const ACDeleteSQL As String = "spu_ACT_delete_Account"
	
	' Update Account SQL
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "UpdateAccount"
	' RAW 17/12/2002 : PS187 : increase number of params by 2
	' RDC 12112003 added parm for AllowElectronicPayment
	Public Const ACUpdateSQL As String = "spu_ACT_update_Account"
	
	' Select Account Balance SQL
	Public Const ACSelectBalanceStored As Boolean = True
	Public Const ACSelectBalanceName As String = "UpdateAccount"
	Public Const ACSelectBalanceSQL As String = "spu_ACT_Select_AccountBal"
	
	'EK 130300
	' Select AccountLedger SQL
	Public Const ACGetAccountLedgerStored As Boolean = True
	Public Const ACGetAccountLedgerName As String = "SelectAccountLedger"
	Public Const ACGetAccountLedgerSQL As String = "spu_ACT_get_Account_Ledger"
	
	'eck230501
	' Select AccountLedger SQL
	Public Const ACSelectTransForAllocationStored As Boolean = True
	Public Const ACSelectTransForAllocationName As String = "SelectTransForAllocation"
    Public Const ACSelectTransForAllocationSQL As String = "spu_ACT_Select_trans_for_allocation"
    Public Const ACSelectTransForReceiptAllocationSQL As String = "spu_ACT_Select_Trans_For_Receipt_Allocation"
	
	'sw 28/01/2003
	Public Const ACAccountOSTransForClaimPaymentName As String = "SelectTransForAllocation"
	Public Const ACSelectAccountOSTransForClaimPaymentSQL As String = "spu_ACT_Select_Trans_For_Allocation_For_Claim_Payment"
	
	
	'TN20010629 - start
	Public Const ACSelIsPostCodeStored As Boolean = False
	Public Const ACSelIsPostCodeName As String = "Select Is PostCode Required"
	Public Const ACSelIsPostCodeSQL As String = "SELECT c.iso_code" & Strings.ChrW(13) & Strings.ChrW(10) &
												"FROM account a," & Strings.ChrW(13) & Strings.ChrW(10) &
												"country c" & Strings.ChrW(13) & Strings.ChrW(10) &
												"WHERE account_id = {account_id}" & Strings.ChrW(13) & Strings.ChrW(10) &
												"AND a.address_country = c.country_id"
	'TN20010629 - end

	'DD 15/07/2002: Select Account Security
	Public Const ACGetAccountSecurityStored As Boolean = True
	Public Const ACGetAccountSecurityName As String = "SelectAccountSecurity"
	Public Const ACGetAccountSecuritySQL As String = "spu_ACTSecurity_AccountRights"

	'SJ 13/05/2003 - start
	Public Const ACDeleteAllocationLocksStored As Boolean = True
	Public Const ACDeleteAllocationLocksName As String = "DeleteAllocationLocks"
	Public Const ACDeleteAllocationLocksSQL As String = "spu_ACT_Delete_Allocation_Locks"
	'SJ 13/05/2003 - end


	' KG 03/07/03
	Public Const ACSelectInstalmentDebtstored As Boolean = True
	Public Const ACSelectInstalmentDebtName As String = "SelectInstalmentDebtName"
	Public Const ACSelectInstalmentDebtSQL As String = "spu_ACT_Select_InstalmentDebt"
	' KG 03 /07/03

	'Tracy Richards 08/09/03 Select Account Balance Lite SQL
	Public Const ACSelectBalanceLiteStored As Boolean = True
	Public Const ACSelectBalanceLiteName As String = "UpdateAccount"
	Public Const ACSelectBalanceLiteSQL As String = "spu_ACT_Select_AccountBal_ByAccount"

	Public Const ACIsDeletedStored As Boolean = False
	Public Const ACIsDeletedName As String = "IsDeleted"
	Public Const ACIsDeletedSQL As String = "SELECT" & Strings.ChrW(13) & Strings.ChrW(10) &
											"   ISNULL(p.is_deleted,0)" & Strings.ChrW(13) & Strings.ChrW(10) &
											"FROM account a" & Strings.ChrW(13) & Strings.ChrW(10) &
											"LEFT JOIN party p" & Strings.ChrW(13) & Strings.ChrW(10) &
											"    ON p.party_cnt = a.account_key" & Strings.ChrW(13) & Strings.ChrW(10) &
											"WHERE a.account_id = {account_id}" & Strings.ChrW(13) & Strings.ChrW(10)

	'For 2005 Client View
	Public Const ACSelectClientAccountDetailsStored As Boolean = True
	Public Const ACSelectClientAccountDetailsName As String = "SelectTurnover"
	Public Const ACSelectClientAccountDetailsSQL As String = "spu_ACT_Select_Client_Account_Details"
	
	Public Const kGetAccountOSTransForDocumentName As String = "Returns outstanding transactions for specified account and document"
	Public Const kGetAccountOSTransForDocumentSQL As String = "spu_ACT_Select_Trans_For_Allocation_For_Document"
	
	Public Const kGetAccountDetailsFromPartyCntName As String = "Returns account details for the specified party"
	Public Const kGetAccountDetailsFromPartyCntSQL As String = "spu_CLM_Get_Party_Account_Details"
	
	Public Const kGetUnallocatedClaimPaymentsName As String = "returns details of any unallocated claim payments for the specified account"
	Public Const kGetUnallocatedClaimPaymentsSQL As String = "spu_ACT_Get_Unallocated_Claim_Payments"
	
	Public Const kGetUnallocatedClaimPaymentsForPaymentDateName As String = "returns details of any unallocated claim payments made on the specified payment date"
	Public Const kGetUnallocatedClaimPaymentsForPaymentDateSQL As String = "spu_ACT_Get_Unallocated_Claim_Payments_By_PaymentDate"
	
	
	Public Const ACGetBaseCountryStored As Boolean = True
	Public Const ACGetBaseCountryName As String = "SelectBaseCountry"
	Public Const ACGetBaseCountrySQL As String = "spu_Get_Account_Base_Country"
	
	
	''For checking Existance of Ledger in Related Table
	Public Const ACIsledgerExist As Boolean = True
	Public Const ACIsLedgerExistName As String = "Is Ledger Exists"
    Public Const ACIsLedgerExistSQL As String = "spu_ACT_isLedgerExists"

    Public Const kGetAccountOSCommForDocumentName As String = "Returns outstanding Commission transactions for specified account and document"
    Public Const kGetAccountOSCommForDocumentSQL As String = "spu_ACT_Select_Comm_For_Allocation_For_Document"
End Module