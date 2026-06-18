Option Strict Off
Option Explicit On
Imports System
Module MainModule
	'***********************************************************************************************
	'Created By     :   Arul Stephen
	'Date           :   02-Sep-2008
	'History        :   It is an New File
	'***********************************************************************************************
	'Start-(Arul Stephen)-(TechSpec WR6ClauseGrouping.doc)
	
	Public Const ACApp As String = "bSIRSelectClauses.Business"
	
	Private Const ACClass As String = "MainModule"
	
	'Default Constants
	Public Const kIDefaultClauseNotExist As Integer = 0
	Public Const kIDefaultClauseExist As Integer = 1
	Public Const kIDefaultClauseExistInArray As Integer = 3
	
	'Clause Constants
	Public Const kIClauseId As Integer = 0
	Public Const kIClauseCode As Integer = 1
	
	'Branch Constants
	Public Const kIBranchId As Integer = 0
	Public Const kIBranchArrayCode As Integer = 4
	
	'Clause Index Constants
	Public Const kIClauseCountIndex As Integer = 0
	Public Const kISelectClauseRowIndex As Integer = 2
	Public Const kISelectClauseSortIndex As Integer = 4
	Public Const kISelectClauseBranchIndex As Integer = 4
	
	'Clause Enum
	Public Enum EnClauseType
		ProductType = 1
		RiskType = 2
	End Enum
	'End-(Arul Stephen)-(TechSpec WR6ClauseGrouping.doc)
End Module