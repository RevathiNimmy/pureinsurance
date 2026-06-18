Option Strict Off
Option Explicit On
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
	' Date: 08/09/1997
	'
	' Description: Contains the SQL Statements required by the
	'              bACTBank.Form class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements

    'developer guide no. 39
    'start
	' Select Finance Ref Pos SQL
	Public Const ACGetFinanceDataPosStored As Boolean = True
	Public Const ACGetFinanceDataPosName As String = "GetFinanceRefPos"
    'Public Const ACGetFinanceDataPosSQL As String = "{call spu_get_finance_data_positions}"
    Public Const ACGetFinanceDataPosSQL As String = "spu_get_finance_data_positions"
	
	' Select Finance Details SQL
	Public Const ACGetFinanceDetailsStored As Boolean = True
	Public Const ACGetFinanceDetailsName As String = "GetFinanceDetails"
    'Public Const ACGetFinanceDetailsSQL As String = "{call spu_get_finance_plan_details (?)}"
    Public Const ACGetFinanceDetailsSQL As String = "spu_get_finance_plan_details"


	' Select Useretails
	Public Const ACSelectUserDetailsStored As Boolean = False
	Public Const ACSelectUserDetailsName As String = "SelectCommission"
    Public Const ACSelectUserDetailsSQL As String = "SELECT user_id,language_id FROM PMUser where username = {username} and password = {password}"

	' Select Next Batch Number
	Public Const ACGetNextEDIBatchNumberStored As Boolean = True
	Public Const ACGetNextEDIBatchNumberName As String = "GetnextBatchNumber"
    'Public Const ACGetNextEDIBatchNumberSQL As String = "{call spu_get_next_edi_batch_number (?)}"
    Public Const ACGetNextEDIBatchNumberSQL As String = "spu_get_next_edi_batch_number"

	' Update Next Batch Number
	Public Const ACUpdateNextEDIBatchNumberStored As Boolean = True
	Public Const ACUpdateNextEDIBatchNumberName As String = "UpdateNextBatchNumber"
    'Public Const ACUpdateNextEDIBatchNumberSQL As String = "{call spu_update_next_edi_batch_number (?)}"
    Public Const ACUpdateNextEDIBatchNumberSQL As String = "spu_update_next_edi_batch_number"
    'end

End Module