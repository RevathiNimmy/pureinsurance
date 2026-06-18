Option Strict Off
Option Explicit On
Imports System
Module bSIRSelectClauseSQL
	
	'***********************************************************************************************
	'Created By     :   Arul Stephen
	'Date           :   02-Sep-2008
	'History        :   It is an New File
	'***********************************************************************************************
	'Start-(Arul Stephen)-(TechSpec WR6ClauseGrouping.doc)
	
	
	Public Const ACSELAllBranchesSQL As String = "spu_Get_Branches"
	Public Const ACSELAllBranchesSQLSQLName As String = "Select all the branches details"
	
	Public Const ACSELRiskTypeClausesSQL As String = "spu_Risk_Type_Clauses_Sel"
	Public Const ACSELRiskTypeClausesSQLName As String = "Select the Risk Type details"
	
	Public Const ACSELProductTypeClausesSQL As String = "spu_Product_Clauses_Sel"
	Public Const ACSELProductTypeClausesSQLName As String = "Select the  Product Type details"
	
	
	Public Const ACAddRisktypeClausesSQL As String = "spu_Risk_Type_Linked_Clauses_add"
	Public Const ACADDRiskTypeClausesSQLName As String = "Add the Risk Type details"
	
	
	Public Const ACAddProductTypeClausesSQL As String = "spu_Product_Linked_Clauses_add"
	Public Const ACADDProductTypeClausesSQLName As String = "Add the ProductType details"
	
	
	Public Const ACDELRisktypeClausesSQL As String = "spu_Risk_Type_Linked_Clauses_del"
	Public Const ACDELRiskTypeClausesSQLName As String = "Del the Risk Type details"
	
	
	Public Const ACDELProductTypeClausesSQL As String = "spu_Product_Linked_Clauses_del"
	Public Const ACDELProductTypeClausesSQLName As String = "Del the Product Type details"
	
	Public Const ACSELRiskTypeLinkedClausesSelSQL As String = "spu_Risk_Type_Linked_Clauses_upd"
	Public Const ACSELRiskTypeLinkedClausesSelSQLName As String = "Sel the Risk Type with Linked Clauses details"
	
	Public Const ACSELProductTypeLinkedClausesSelSQL As String = "spu_Product_Linked_Clauses_upd"
	Public Const ACSELProductTypeLinkedClausesSelSQLName As String = "Sel the Product Type with Linked Clauses details"
	
	'End-(Arul Stephen)-(TechSpec WR6ClauseGrouping.doc)
End Module