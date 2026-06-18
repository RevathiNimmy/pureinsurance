Option Strict Off
Option Explicit On
Imports System
Module FormSQL
	' ***************************************************************** '
	' Class Name: FormSQL
	'
	' Date: 22-07-1997
	'
	' Description: Contains the SQL Statements required by the
	'              bACTExplorer.Form class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Select  Account SQL
	Public Const ACGetAccountDetailsStored As Boolean = True
	Public Const ACGetAccountDetailsName As String = "SelectAllAccount"
    Public Const ACGetAccountDetailsSQL As String = "spu_ACT_select_Account"
	
	'******************************************************
	' ELEMENT Table
	'******************************************************
	
	' Select All Element SQL
	Public Const ACGetAllElementDetailsStored As Boolean = True
	Public Const ACGetAllElementDetailsName As String = "SelectAllElement"
    Public Const ACGetAllElementDetailsSQL As String = "spu_ACT_selall_Element"
	
	' Select Element Record
	Public Const ACGetElementDetailsStored As Boolean = True
	Public Const ACGetElementDetailsName As String = "SelectElement"
    Public Const ACGetElementDetailsSQL As String = "spu_ACT_select_Element"
	' Update Element SQL
	Public Const ACUpdateElementStored As Boolean = True
	Public Const ACUpdateElementName As String = "UpdateElement"
    Public Const ACUpdateElementSQL As String = "spu_ACT_update_Element"
	
	' Add Element SQL
	Public Const ACAddElementStored As Boolean = True
	Public Const ACAddElementName As String = "AddElement"
    Public Const ACAddElementSQL As String = "spu_ACT_add_Element"
	
	' Delete Element SQL
	Public Const ACDeleteElementStored As Boolean = True
	Public Const ACDeleteElementName As String = "DeleteElement"
    Public Const ACDeleteElementSQL As String = "spu_ACT_delete_Element"
	
	' IsUsed? Element SQL
	Public Const ACIsUsedElementStored As Boolean = True
	Public Const ACIsUsedElementName As String = "IsUsedElement"
    Public Const ACIsUsedElementSQL As String = "spu_ACT_Select_IsUsed_Element"
	
	' IsDuplicate Element SQL
	Public Const ACIsDuplicateElementStored As Boolean = True
	Public Const ACIsDuplicateElementName As String = "IsDuplicateElement"
    Public Const ACIsDuplicateElementSQL As String = "spu_ACT_Do_IsDuplicate_Element"
	
	'******************************************************
	' STRUCTURE TREE Table
	'******************************************************
	
	' Select StructureTree SQL
	Public Const ACGetStructureTreeDetailsStored As Boolean = True
	Public Const ACGetStructureTreeDetailsName As String = "SelectStructureTree"
    Public Const ACGetStructureTreeDetailsSQL As String = "spu_ACT_Select_StructureTree"
	
	' Add StructureTree SQL
	Public Const ACAddStructureTreeStored As Boolean = True
	Public Const ACAddStructureTreeName As String = "AddStructureTree"
    Public Const ACAddStructureTreeSQL As String = "spu_ACT_add_StructureTree"
	
	' Delete StructureTree SQL
	Public Const ACDeleteStructureTreeStored As Boolean = True
	Public Const ACDeleteStructureTreeName As String = "DeleteStructureTree"
    Public Const ACDeleteStructureTreeSQL As String = "spu_ACT_delete_StructureTree"
	
	' Update StructureTree MapId SQL
	Public Const ACUpdateStructureTreeMapIDStored As Boolean = True
	Public Const ACUpdateStructureTreeMapIDName As String = "UpdateStructMapID"
    Public Const ACUpdateStructureTreeMapIDSQL As String = "spu_ACT_Update_StructMapID"
	
	' Update StructureTree ParentID SQL
	Public Const ACUpdateStructureTreeParentIDStored As Boolean = True
	Public Const ACUpdateStructureTreeParentIDName As String = "UpdateStructParentID"
    Public Const ACUpdateStructureTreeParentIDSQL As String = "spu_ACT_Update_StructParentID"
	
	' Update StructureTree AccountID SQL
	Public Const ACUpdateStructureTreeAccountIdStored As Boolean = True
	Public Const ACUpdateStructureTreeAccountIdName As String = "UpdateStructAccountID"
    Public Const ACUpdateStructureTreeAccountIdSQL As String = "spu_ACT_Update_StructAccountID"
	
	' Select StructureTree Node Details SQL
	Public Const ACGetStructNodeDetailsStored As Boolean = True
	Public Const ACGetStructNodeDetailsName As String = "GetStructNodeDetails"
    Public Const ACGetStructNodeDetailsSQL As String = "spu_ACT_Select_StructNode"
	
	' Select StructureTree Children Details SQL
	Public Const ACGetStructChildrenDetailsStored As Boolean = True
	Public Const ACGetStructChildrenDetailsName As String = "GetStructChildrenDetails"
    Public Const ACGetStructChildrenDetailsSQL As String = "spu_ACT_Select_StructChildren"
	
	' Select StructureTree Children Details For Client SQL
	Public Const ACGetStructClientDetailsStored As Boolean = True
	Public Const ACGetStructClientDetailsName As String = "GetStructClientsDetails"
    Public Const ACGetStructClientDetailsSQL As String = "spu_ACT_Select_StructClients"
	
	'******************************************************
	' MAPPING Table
	'******************************************************
	
	' Add Mapping SQL
	Public Const ACAddMappingStored As Boolean = True
	Public Const ACAddMappingName As String = "AddMapping"
    Public Const ACAddMappingSQL As String = "spu_ACT_add_Mapping"
	
	' Update Mapping SQL
	Public Const ACUpdateMappingStored As Boolean = True
	Public Const ACUpdateMappingName As String = "UpdateMapping"
    Public Const ACUpdateMappingSQL As String = "spu_ACT_update_Mapping"
	
	' Delete Mapping SQL
	Public Const ACDeleteMappingStored As Boolean = True
	Public Const ACDeleteMappingName As String = "DeleteMapping"
    Public Const ACDeleteMappingSQL As String = "spu_ACT_delete_Mapping"
	
	' Select Node Id From Mapping SQL
	Public Const ACSelectNodeFromMappingStored As Boolean = True
	Public Const ACSelectNodeFromMappingName As String = "SelectNodeFromMapping"
	'Sankar - (WPR85_Cash_Deposit_Process) - Paralleling
    Public Const ACSelectNodeFromMappingSQL As String = "spu_ACT_Do_Get_Node_From_Map"
	
	'******************************************************
	' ACCOUNT Table
	'******************************************************
	
	' Select AccountId From type SQL
	Public Const ACSelectAccountByTypeStored As Boolean = True
	Public Const ACSelectAccountByTypeName As String = "SelectAccountByType"
    Public Const ACSelectAccountByTypeSQL As String = "spu_ACT_Select_AccountType"
	
	' Select AccountId From ShortCode
	Public Const ACSelectAccountByShortStored As Boolean = True
	Public Const ACSelectAccountByShortName As String = "SelectAccountByShort"
	'eck PN6169
    Public Const ACSelectAccountByShortSQL As String = "spu_ACT_Select_AccountShort"
	
	'DJM 18/12/2003 : Changed to use stored procedure.
	' Select AccountId From Account Key
	Public Const ACSelectAccountByKeyStored As Boolean = True
	Public Const ACSelectAccountByKeyName As String = "SelectAccountByKey"
    Public Const ACSelectAccountByKeySQL As String = "spu_ACT_Select_AccountKey"
	
	'DN 06/12/02
	' Select branch From AccountId
	Public Const ACSelectBranchByAccountStored As Boolean = True
	Public Const ACSelectBranchByAccountName As String = "SelectBranchByAccount"
    Public Const ACSelectBranchByAccountSQL As String = "spu_ACT_Select_AccountBranch"
	
	'KB PN 7526 15/10/2003
	' Select branchid From AccountId
	Public Const ACSelectBranchIdByAccountStored As Boolean = True
	Public Const ACSelectBranchIdByAccountName As String = "SelectBranchIdByAccount"
    Public Const ACSelectBranchIdByAccountSQL As String = "spu_ACT_Select_AccountBranchId"
	
	
	'******************************************************
	' Element Extra Table
	'******************************************************
	
	' Select ElementExtras Record
	Public Const ACGetElementExtrasDetailsStored As Boolean = True
	Public Const ACGetElementExtrasDetailsName As String = "SelectElementExtras"
    Public Const ACGetElementExtrasDetailsSQL As String = "spe_ElementExtras_sel"
	' Update ElementExtras SQL
	Public Const ACUpdateElementExtrasStored As Boolean = True
	Public Const ACUpdateElementExtrasName As String = "UpdateElementExtras"
	'sj 21/11/2002 - start
	'PS700
    Public Const ACUpdateElementExtrasSQL As String = "spe_ElementExtras_upd"
	'sj 21/11/2002 - end
	
	' Add ElementExtras SQL
	Public Const ACAddElementExtrasStored As Boolean = True
	Public Const ACAddElementExtrasName As String = "AddElementExtras"
    Public Const ACAddElementExtrasSQL As String = "spe_ElementExtras_add"
	
	' Delete ElementExtras SQL
	Public Const ACDeleteElementExtrasStored As Boolean = True
	Public Const ACDeleteElementExtrasName As String = "DeleteElementExtras"
    Public Const ACDeleteElementExtrasSQL As String = "spe_ElementExtras_del"
	
	'eck PN5946 110803
	' Select All Account SQL
	Public Const ACGetLedgerDetailsStored As Boolean = True
    Public Const ACGetLedgerDetailsName As String = "SelectAllLedgers"
    Public Const ACGetLedgerDetailsSQL As String = "spu_ACT_selall_Ledger"
End Module