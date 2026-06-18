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
	
	
	Public Const ACCreateWorkStepGroupUserSQL As String = "{call spu_pmwrk_work_stepgroupuser_insert (?,?,?,?,?,?,?)}"
	Public Const ACCreateWorkStepGroupUserName As String = "Create an entry in the work_pmwrk_workflow_stepgroupuser table"
	
	Public Const ACCreateWorkStepInstanceSQL As String = "{call spu_pmwrk_work_step_instance_insert (?,?,?,?)}"
	Public Const ACCreateWorkStepInstanceName As String = "Create an entry in the work_pmwrk_worklow_step_instance table"
	
	Public Const ACCreateWorkStepDataSQL As String = "{call spu_pmwrk_work_stepdata_insert (?,?,?,?,?,?,?,?)}"
	Public Const ACCreateWorkStepDataName As String = "Create an entry in the work pmwrk_workflow_stepdata table"
	
	' Removes all entries from the work workflow tables for the specified calling process ids
	Public Const ACDeleteWorkWorkflowEntriesSQL As String = "{call spu_pmwrk_work_workflow_delete (?,?)}"
	Public Const ACDeleteWorkWorkflowEntriesName As String = "Removes all entries from the work workflow tables for process ids"
	
	' copies all entries from work workflow tables into their live equivalents
	Public Const ACCopyWorkToLiveSQL As String = "{call spu_pmwrk_work_copy_to_live (?,?)}"
	Public Const ACCopyWorkToLiveName As String = "copy all entries in work workflow tables to live equivalents"
	
	' Returns the data for the specified step id
	Public Const ACGetStepDetailsSQL As String = "{call spu_PMwrk_Workflow_Step_Details_Select(?)}"
	Public Const ACGetStepDetailsName As String = "Returns the data for the specified step id "
	
	' returns the keys for the specified task instance.
	Public Const ACGetTaskInstKeysSQL As String = "{call spu_pmwrk_task_inst_key_select(?)}"
	Public Const ACGetTaskInstKeysName As String = "Returns the task instance keys for the specified task instance"
	
	Public Const ACCopyTaskInstKeysSQL As String = "{call spu_pmwrk_task_inst_copy_keys(?,?)}"
	Public Const ACCopyTaskInstKeysName As String = "Copies Task Instance Keys to the specified task"
	
	Public Const ACUpdateStepInstanceSQL As String = "{call spu_pmwrk_workflow_step_instance_update(?,?,?)}"
	Public Const ACUpdateStepInstanceName As String = "Updates step instance with the next step - task"
	
	Public Const ACCompleteTaskSQL As String = "{call spu_pmwrk_task_inst_outcome_update(?,?,?)}"
	Public Const ACCompleteTaskName As String = "Updates the specified task instances outcome and status"
	
	Public Const ACDeleteStepInstanceName As String = "RemoveWorkflowInstance"
	Public Const ACDeleteStepInstanceSQL As String = "{call spu_PMWrk_Workflow_Step_Instance_del (?)}"
End Module