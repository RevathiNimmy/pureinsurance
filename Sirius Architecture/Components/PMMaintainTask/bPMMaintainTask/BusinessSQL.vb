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
	
	
	' Returns all the existing PMWrk_Task rows that are not is_deleted = 0
	Public Const ACGetMaintainDataSQL As String = "{call spu_PMwrk_Task_NotDeleted_Select}"
	Public Const ACGetMaintainDataName As String = "Select all not deleted PMWrk_Task rows"
	
	' Returns all the existing PMWrk_Task rows that are not is_deleted = 0
	Public Const ACGetTaskOutcomesSQL As String = "{call spu_PMwrk_Task_Outcomes_Select}"
	Public Const ACGetTaskOutcomesName As String = "Select all not deleted Task_Outcome rows"
	
	' Updates the specified PMwrk_Tasks default outcomes, action required indicator
	Public Const ACUpdateTaskSQL As String = "{call spu_PMwrk_Task_Default_Outcomes_Update(?,?,?,?,?)}"
	Public Const ACUpdateTaskName As String = "updates the specified task default outcomes"
End Module