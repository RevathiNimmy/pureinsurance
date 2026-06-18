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
	
	
	' Returns all the existing Pwmrk_workflow_step records for the specified workflow_id
	Public Const ACGetMaintainDataSQL As String = "{call spu_PMwrk_Workflow_Step_Select(?)}"
	Public Const ACGetMaintainDataName As String = "select workflow package steps"
	
	' add an additional workflow step for the specified workflow package
	Public Const ACAddPackageStepSQL As String = "{call spu_PMWrk_Workflow_Step_Insert(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)}"
	Public Const ACAddPackageStepName As String = "select workflow package steps"
	
	' updates an existing workflow step for the specified workflow package
	Public Const ACUpdatePackageStepSQL As String = "{call spu_PMWrk_Workflow_Step_Update(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)}"
	Public Const ACUpdatePackageStepName As String = "select workflow package steps"
	
	' returns all valid branch id for all user / usergroups
	Public Const ACGetValidUserBranchesSQL As String = "{call spu_pmuser_get_allowed_branches}"
	Public Const ACGetValidUserBranchesName As String = "ReturnWorkflowPackageInformationToUserSCript"
End Module