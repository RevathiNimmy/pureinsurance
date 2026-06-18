Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: BusinessSQL
	'
	' Date: 05/05/1999
	'
	' Description: Contains the SQL Statements required by the
	'              bSIRNarr.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	' Select Account ID
	Public Const ACGetAccountIDStored As Boolean = False
	Public Const ACGetAccountIDName As String = "GetAccountID"
	Public Const ACGetAccountIDSQL As String = "SELECT account_id " &  _
	                                           "FROM account " &  _
	                                           "WHERE " &  _
	                                           "account_key = {account_key} "
	
	
	' Select All Payment Groups SQL
	Public Const ACGetPaymentGroupsDetailsStored As Boolean = False
	Public Const ACGetPaymentGroupsDetailsName As String = "SelectPaymentGroups"
	Public Const ACGetPaymentGroupsDetailsSQL As String = "SELECT G.description,C.description, G.Paymentgroup_Id, C.company_id  " &  _
	                                                      "FROM InsurerPayment I, Company C, PaymentGroup G " &  _
	                                                      "WHERE " &  _
	                                                      "G.paymentgroup_id = I.paymentgroup_id " &  _
	                                                      "AND I.company_id = C.company_id " &  _
	                                                      "AND I.account_id = {account_id} "
	
	' Delete Payment Groups SQL
	Public Const ACDeletePaymentGroupsStored As Boolean = False
	Public Const ACDeletePaymentGroupsName As String = "DeletePaymentGroups"
	Public Const ACDeletePaymentGroupsSQL As String = "DELETE from InsurerPayment " &  _
	                                                  "WHERE account_id = {account_id}"
	
	' Insert Payment Groups SQL
	Public Const ACInsertPaymentGroupsStored As Boolean = False
	Public Const ACInsertPaymentGroupsName As String = "InsertPaymentGroups"
	Public Const ACInsertPaymentGroupsSQL As String = "INSERT INTO InsurerPayment (paymentgroup_id, account_id, company_id) " &  _
	                                                  "VALUES ({paymentgroup_id}, {account_id}, {company_id})"
	
	' Select Party & Source IDs from PartyCnt
	Public Const ACGetPartyKeyFromPartyCntStored As Boolean = False
	Public Const ACGetPartyKeyFromPartyCntName As String = "PartyKeyFromPartyCnt"
	Public Const ACGetPartyKeyFromPartyCntSQL As String = "SELECT party_id, source_id FROM Party WHERE party_cnt = {party_cnt}"
	
	'TO BE REPLACED BY SOURCES ASAP
	' Select Companies
	' KB PN 6372 26082003 only if not deleted
	Public Const ACGetCompaniesStored As Boolean = False
	Public Const ACGetCompaniesName As String = "GetCompanies"
	Public Const ACGetCompaniesSQL As String = "SELECT company_id, description " &  _
	                                           "FROM company WHERE is_deleted = 0"
	
	''Devlopment work for Insurer Payment Locking
	' Select Payment Locking Type SQL
	Public Const ACGetPaymentLockingTypeStored As Boolean = True
	Public Const ACGetPaymentLockingTypeName As String = "SelectPaymentLockingType"
	Public Const ACGetPaymentLockingTypeSQL As String = "{call spe_Insurer_Payment_Type_sel (?)}"
End Module