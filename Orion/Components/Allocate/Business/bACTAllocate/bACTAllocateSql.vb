Option Strict Off
Option Explicit On
Imports System
Module AllocateSql
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	' ***************************************************************** '
	' Class Name: FindTransSQL
	'
	' Date: 28 August 1997
	'
	' Description: Contains the SQL Statements to (Stored Procedures
	'              and Embedded SQL)
	'
	' ***************************************************************** '
	' Edit History :
	'
	' RAW 13/01/2003 : PS187 : replaced hard-coded sql that deleted from
	'                          TransMatch with stored procedure
	'****************************************************************** '
	
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectEvent"
	' Public Const ACSelectSQL = "SELECT * FROM Event WHERE event_id = {event_id}"
	
	'select Trans from full query
	Public Const ACTransFromQueryStored As Boolean = True
    Public Const ACTransFromQueryName As String = "SelectTransQuery"
    'Developer Guide No 85
    'Public Const ACTransFromQuerySQL As String = "{call sp_ACT_Do_FindTrans (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)}"
    Public Const ACTransFromQuerySQL As String = "sp_ACT_Do_FindTrans"
	
	'get TransID from parameters SQL
	Public Const ACGetTransIDStored As Boolean = True
    Public Const ACGetTransIDName As String = "GetTransID"
    'Developer Guide No 85
    'Public Const ACGetTransIDSQL As String = "{call sp_ACT_Do_GetTransId (?, ?, ?, ?, ?)}"
    Public Const ACGetTransIDSQL As String = "sp_ACT_Do_GetTransId"
	
	'select marked transmatch records
	Public Const ACGetMarkedDetailsStored As Boolean = True
	Public Const ACGetMarkedDetailsName As String = "GetMarkedTransmatch"
    'Developer Guide No 85
    'Public Const ACGetMarkedDetailsSQL As String = "{call spu_ACT_Select_Marked_Details (?,?,?,?,?,?,?,?,?,?,?)}"
    Public Const ACGetMarkedDetailsSQL As String = "spu_ACT_Select_Marked_Details"
	'delete marked transmatch record
	Public Const ACDeleteMarkedDetailsStored As Boolean = True
	Public Const ACDeleteMarkedDetailsName As String = "DeleteMarkedTransmatch"
    'Developer Guide No 85
    Public Const ACDeleteMarkedDetailsSQL As String = "spu_ACT_Delete_Transmatch" ' RAW 13/01/2003 : PS187 : replaced spu_ with spu_
	
	' RAW 13/01/2003 : PS187 : added
	Public Const ACUnMarkTransMatchStored As Boolean = True
	Public Const ACUnMarkTransMatchName As String = "UnMarkTransaction"
    'Developer Guide No 85
    'Public Const ACUnMarkTransMatchSQL As String = "{call spu_ACT_UnMark_TransMatch (?)}"
    Public Const ACUnMarkTransMatchSQL As String = "spu_ACT_UnMark_TransMatch"
	' RAW 13/01/2003 : PS187 : end
	
	'Get small amount write off details for a branch
	Public Const ACGetSmallAmountWriteOffStored As Boolean = True
	Public Const ACGetSmallAmountWriteOffName As String = "GetSmallAmountWriteOff"
    'Developer Guide No 85
    'Public Const ACGetSmallAmountWriteOffSQL As String = "{call spu_ACT_Select_Small_Amount_Write_Off (?)}"
    Public Const ACGetSmallAmountWriteOffSQL As String = "spu_ACT_Select_Small_Amount_Write_Off"
	
	'Get the outstanding transactions filtered by insurance_file.policy ref
	Public Const ACGetTransFilterByPolicyRefStored As Boolean = True
	Public Const ACGetTransFilterByPolicyRefName As String = "GetTransFilterByPolicyRef"
    'Developer Guide No 85
    'Public Const ACGetTransFilterByPolicyRefSQL As String = "{call spu_ACT_Select_Trans_For_Allocation_FilterBy_PolicyRef (?)}"
    Public Const ACGetTransFilterByPolicyRefSQL As String = "spu_ACT_Select_Trans_For_Allocation_FilterBy_PolicyRef"
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module