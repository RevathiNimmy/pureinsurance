Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
	' ***************************************************************** '
	' Class Name: FormSQL
	'
	' Date: 31/07/1997
	'
	' Description: Contains the SQL Statements required by the
	'              bPMSource.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select Source SQL
	Public Const ACGetDetailsStored As Boolean = True
	Public Const ACGetDetailsName As String = "SelectSource"
    'Developer Guide No. 39
    Public Const ACGetDetailsSQL As String = "spu_PM_Select_Source"

    ' Select All Source SQL
    Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllSourcer"
    'Developer Guide No. 39
    Public Const ACGetAllDetailsSQL As String = "spu_PM_SelAll_Source"

    ' Check ID SQL
    Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckSourceID"
    'Developer Guide No. 39
    Public Const ACCheckIDSQL As String = "spu_PM_Check_Source"

    ' Add Source SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddSource"
    'Public Const ACAddSQL = "{call spu_PM_Add_Source (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)}"
    'Developer Guide No. 39
    Public Const ACAddSQL As String = "spu_PM_Add_Source"

    ' Delete Source SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteSource"
    'Developer Guide No. 39
    Public Const ACDeleteSQL As String = "spu_PM_Delete_Source"

    ' Update Source SQL
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateSource"
    'Public Const ACUpdateSQL = "{call spu_PM_Update_Source (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)}"
    'Developer Guide No. 39
    Public Const ACUpdateSQL As String = "spu_PM_Update_Source"

    ' Add Product Source SQL
    Public Const ACAddProductSourceStored As Boolean = True
    Public Const ACAddProductSourceName As String = "AddSource"
    'Developer Guide No. 39
    Public Const ACAddProductSourceSQL As String = "spu_Add_Source_Product_Options"

    ' Check Underwriting Branch Policies
    Public Const ACCheckUnderwritingBranchPoliciesStored As Boolean = True
    Public Const ACCheckUnderwritingBranchPoliciesName As String = "CheckUnderwritingBranchPolicies"
    'Developer Guide No. 39
    Public Const ACCheckUnderwritingBranchPoliciesSQL As String = "spu_PM_CheckUnderwritingBranchPolicies"
    'Developer Guide No. 29
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module