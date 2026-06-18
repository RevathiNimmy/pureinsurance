Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	
	' ***************************************************************** '
	' Class Name: BusinessSQL
	'
	' Date:
	'
	' Description: Contains the SQL Statements required by the
	'              business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	Public Const kGetALLPMWrkTaskGroupTasksName As String = "Returns all effective PMWrkTasks for all effective PMWrkTaskGroups"
	Public Const kGetALLPMWrkTaskGroupTasksSQL As String = "spu_ACT_PMwrk_Task_Group_Tasks_Select"
	
	Public Const kGetALLPMWrkTaskGroupPMUserGroupsName As String = "Returns all effective PMUSerGroups for all effective PMWrkTaskGroups"
	Public Const kGetALLPMWrkTaskGroupPMUserGroupsSQL As String = "spu_ACT_PMwrk_Task_Group_PMUserGroup_Select"
	
    Public Const kGetInsuranceFileStatusSQL As String = "spu_SIR_GetInsuranceFileStatus"
	
    Public Const kGetAllUDLItemsName As String = "Returns all UDL Items"
    Public Const kGetAllUDLItemsSQL As String = "spu_SIR_Select_Chase_Cycle_UDL"

    Public Const kGetChaseCycleUDLStatusDescName As String = "Return UDL Status Description"
    Public Const kGetChaseCycleUDLStatusDesc As String = "spu_SIR_Sel_Chase_Cycle_Udl_Status_Description"

End Module