Option Strict Off
Option Explicit On
Imports System
Module AllocateSQL
	
    ' Select Allocation SQL
    Public Const ACGetDetailsStored As Boolean = True
	Public Const ACGetDetailsName As String = "SelectAllocation"
    Public Const ACGetDetailsSQL As String = "spu_ACT_Select_Allocation"
	
	' Select All Allocation SQL
	Public Const ACGetAllDetailsStored As Boolean = True
	Public Const ACGetAllDetailsName As String = "SelectAllAllocation"
    Public Const ACGetAllDetailsSQL As String = "spu_ACT_selall_Allocation"
	
	' Check ID SQL
	Public Const ACCheckIDStored As Boolean = True
	Public Const ACCheckIDName As String = "CheckAllocationID"
    Public Const ACCheckIDSQL As String = "spu_ACT_check_Allocation"
	
	' Add Allocation SQL
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "AddAllocation"
    Public Const ACAddSQL As String = "spu_ACT_add_Allocation"
	
	' Delete Allocation SQL
	Public Const ACDeleteStored As Boolean = True
	Public Const ACDeleteName As String = "DeleteAllocation"
    Public Const ACDeleteSQL As String = "spu_ACT_delete_Allocation"
	
	' Update Allocation SQL
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "UpdateAllocation"
    Public Const ACUpdateSQL As String = "spu_ACT_update_Allocation"
	
    Public Const ACGetAllocationTotalSQL As String = "spu_ACT_Select_AllocationTotal_By_Allocation_ID"

    Public Const kAddAllocationBatchStored As Boolean = True
    Public Const kAddAllocationBatchName As String = "AddAllocationBatch"
    Public Const kAddAllocationBatchSQL As String = "spu_ACT_Add_AllocationBatch"

End Module