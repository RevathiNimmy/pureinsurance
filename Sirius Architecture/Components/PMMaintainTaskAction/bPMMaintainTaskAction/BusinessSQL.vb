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
	Public Const ACGetDocumentTemplatesSQL As String = "{call spu_PMwrk_Get_Document_Templates}"
	Public Const ACGetDocumentTemplatesName As String = "returns all the document templates"
	
	' Adds a new PMWrk_Task_Action_Type
	Public Const ACAddTaskActionTypeSQL As String = "{call spu_PMwrk_Task_Action_Type_Add(?,?,?,?,?,?,?,?)}"
	Public Const ACAddTaskActionTypeName As String = "adds a new task action type"
	
	' Updates an existing PMWrk_Task_Action_Type
	Public Const ACUpdateTaskActionTypeSQL As String = "{call spu_PMwrk_Task_Action_Type_Update(?,?,?,?,?,?,?)}"
	Public Const ACUpdateTaskActionTypeName As String = "Updates an existing PMWrk_Task_Action_Type"
	
	' Returns all the existing PMWrk_Task_Action_Type rows
	Public Const ACGetMaintainDataSQL As String = "{call spu_PMwrk_Task_Action_Type_Select}"
	Public Const ACGetMaintainDataName As String = "Select all PMWrk_Task_Action_Type rows"
	
	' Returns all the existing Task_Outcome rows
	Public Const ACGetTaskOutcomesSQL As String = "{call spu_PMwrk_Task_Outcomes_Select}"
	Public Const ACGetTaskOutcomesName As String = "Select all Task_Outcomes rows"
	
	' Returns all the existing PMWrk_Task_Action_Type rows
	Public Const ACGetTaskActionTypeOutcomesSQL As String = "{call spu_PMwrk_Task_Action_Type_Outcome_Select}"
	Public Const ACGetTaskActionTypeOutcomesName As String = "Select all PMwrk_Task_Action_Type_Outcome rows"
	
	
	' Deletes all task action type outcomes for the specified task_action_type_id
	' and then creates all required outcomes
	Public Const ACUpdateTaskActionTypeOutcomesSQL As String = "{call spu_PMwrk_Task_Action_Type_Outcome_Update(?,?)}"
	Public Const ACUpdateTaskActionTypeOutcomesName As String = "Select all PMwrk_Task_Action_Type_Outcome rows"
End Module