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
	
	
	' returns all document templates
	Public Const ACGetTaskGroupTaskDetailsSQL As String = "{call spu_PMwrk_Get_Task_Group_Task_Details}"
	Public Const ACGetTaskGroupTaskDetailsName As String = "returns all the task / task group details"
	
	' returns all the pmwrk_task_action_type records which are not deleted
	Public Const ACGetTaskActionTypeSQL As String = "{call spu_PMwrk_Task_Action_Type_Select_NotDeleted}"
	Public Const ACGetTaskActionTypeName As String = "returns all the task / task group details"
	
	' returns all the pmwrk_task_group_task_action records
	Public Const ACGetTaskGroupActionTypesSQL As String = "{call spu_PMwrk_Get_Task_Group_Task_Action}"
	Public Const ACGetTaskGroupActionTypesName As String = "returns all the task / task group details"
	
	' deletes all appropriate pmwrk_Task_group_task_actions and replaces them with the latest selection
	Public Const ACUpdateTaskGroupTaskActionsSQL As String = "{call spu_PMwrk_Task_Group_Task_Action_Update(?,?,?)}"
	Public Const ACUpdateTaskGroupTaskActionsName As String = "deletes and then creates the appropriate action entries"
End Module