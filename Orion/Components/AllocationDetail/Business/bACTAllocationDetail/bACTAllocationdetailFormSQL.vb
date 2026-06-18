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
	
	' Description: Contains the SQL Statements required by the
	'              Form class.
	'
	' Edit History: TF191198 - amendments for EMU database changes
	' ***************************************************************** '
	
	'SQL Statements
	
    'Devloper Guide No 39
    'Starts
	' Select Allocationdetail SQL
	Public Const ACGetDetailsStored As Boolean = True
	Public Const ACGetDetailsName As String = "SelectAllocationdetail"
    Public Const ACGetDetailsSQL As String = "spu_ACT_select_AllocationDetail"
	
	' Select All Allocationdetail SQL
	Public Const ACGetAllDetailsStored As Boolean = True
	Public Const ACGetAllDetailsName As String = "SelectAllAllocationdetail"
    Public Const ACGetAllDetailsSQL As String = "spu_ACT_selall_AllocationDetail"
	
	' Check ID SQL
	Public Const ACCheckIDStored As Boolean = True
	Public Const ACCheckIDName As String = "CheckAllocationdetailID"
    Public Const ACCheckIDSQL As String = "spu_ACT_check_AllocationDetail"
	
	' Add Allocationdetail SQL
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "AddAllocationdetail"
    Public Const ACAddSQL As String = "spu_ACT_add_AllocationDetail"
	
	' Delete Allocationdetail SQL
	Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteAllocationdetail"
    Public Const ACDeleteSQL As String = "spu_ACT_delete_AllocationDetail"
	
	' Update Allocationdetail SQL
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "UpdateAllocationdetail"
    Public Const ACUpdateSQL As String = "spu_ACT_update_AllocationDetail"
	
	'EK 230200
	' Select Allocationdetail SQL
	Public Const ACSelectStored As Boolean = True
    Public Const ACSelectName As String = "SelectDetailAllocation"
    Public Const ACSelectSQL As String = "spu_ACT_select_DetailAllocation"
	
	' Delete Allocation SQL
	Public Const ACDeleteAllocationStored As Boolean = True
    Public Const ACDeleteAllocationName As String = "DeleteAllocation"
    Public Const ACDeleteAllocationSQL As String = "spu_ACT_delete_Allocation"
    'Ends
    
End Module