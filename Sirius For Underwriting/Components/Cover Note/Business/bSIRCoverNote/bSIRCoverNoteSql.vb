Option Strict Off
Option Explicit On
Imports System
Module BusinessSql
	
	' ***************************************************************** '
	' Class Name: bSIRCoverNote
	'
	' Date: 02 Aug 2007
	'
	' Description: Contains the SQL Stored Procedures Statements
	'
	' ***************************************************************** '
	
	'SQL Statements
	
	'Add Cover Note Book SQL
	Public Const ACAddCoverNoteBookStored As Boolean = True
    Public Const ACAddCoverNoteBookName As String = "AddCoverNoteBook"
    'Developer Guide No. 39
    Public Const ACAddCoverNoteBookSQL As String = "spu_SIR_Cover_Note_Book_Add"
	
	'Find Cover Note Book SQL
	Public Const ACFindCoverNoteBookStored As Boolean = True
    Public Const ACFindCoverNoteBookName As String = "FindCoverNoteBook"
    'Developer Guide No. 39
    Public Const ACFindCoverNoteBookSQL As String = "spu_SIR_Cover_Note_Book_Find"
	
	'Update Cover Note Book SQL
	Public Const ACUpdCoverNoteBookStored As Boolean = True
	Public Const ACUpdCoverNoteBookName As String = "UpdCoverNoteBook"
    'Developer Guide No. 39
    Public Const ACUpdCoverNoteBookSQL As String = "spu_SIR_Cover_Note_Book_Upd"
	
	'Select Cover Note Book for cover note book id SQL
	Public Const ACSelCoverNoteBookStored As Boolean = True
	Public Const ACSelCoverNoteBookName As String = "SelCoverNoteBook"
    'Developer Guide No. 39
    Public Const ACSelCoverNoteBookSQL As String = "spu_SIR_Cover_Note_Book_Sel"
	
	'Get Cover Note Sheets SQL
	Public Const ACGetCoverNoteSheetsStored As Boolean = True
    Public Const ACGetCoverNoteSheetsName As String = "GetCoverNoteSheets"
    'Developer Guide No. 39
    Public Const ACGetCoverNoteSheetsSQL As String = "spu_SIR_Cover_Note_Sheet_Get"
	
	'Select Cover Note Sheet SQL
	Public Const ACSelCoverNoteSheetsStored As Boolean = True
	Public Const ACSelCoverNoteSheetsName As String = "SelCoverNoteSheets"
    'Developer Guide No. 39
    Public Const ACSelCoverNoteSheetsSQL As String = "spu_SIR_Cover_Note_Sheet_Sel"
	
	'Update Cover Note Sheet SQL
	Public Const ACUpdCoverNoteSheetStored As Boolean = True

    Public Const ACUpdCoverNoteSheetName As String = "UpdateCoverNoteSheet"
    'Developer Guide No. 39
    Public Const ACUpdCoverNoteSheetSQL As String = "spu_SIR_Cover_Note_Sheet_Upd"
	
	'Assign Cover Note Sheet SQL
	Public Const ACAssignCoverNoteSheetStored As Boolean = True
	Public Const ACAssignCoverNoteSheetName As String = "AssignCoverNoteSheet"

    'Developer Guide No. 39
    Public Const ACAssignCoverNoteSheetSQL As String = "spu_SIR_Cover_Note_Sheet_Assign"
	
	'Validate Cover Note Sheet SQL
	Public Const ACValidateCoverNoteSheetStored As Boolean = True
	Public Const ACValidateCoverNoteSheetName As String = "ValidateCoverNoteSheet"
    'Developer Guide No. 39
    Public Const ACValidateCoverNoteSheetSQL As String = "spu_SIR_Cover_Note_Sheet_Validate"
	
	'Get Cover Note Sheet By Policy SQL
	Public Const ACGetCoverNoteSheetByPolStored As Boolean = True
	Public Const ACGetCoverNoteSheetByPolName As String = "GetCoverNoteSheetByPolicy"
    'Developer Guide No. 39
    Public Const ACGetCoverNoteSheetByPolSQL As String = "spu_SIR_Get_Cover_Note_Sheet_By_Policy"
	
	'Add Cover Note Sheet SQL
	Public Const ACAddCoverNoteSheetStored As Boolean = True
	Public Const ACAddCoverNoteSheetName As String = "AddCoverNoteSheets"
    'Developer Guide No. 39
    Public Const ACAddCoverNoteSheetSQL As String = "spu_SIR_Cover_Note_Sheet_Add"

	
	'Delete Cover Note Sheet SQL
	Public Const ACDelCoverNoteSheetStored As Boolean = True
	Public Const ACDelCoverNoteSheetName As String = "DelCoverNoteSheets"
    Public Const ACDelCoverNoteSheetSQL As String = "spu_SIR_Cover_Note_Sheet_Del"
	
	'Get Available Branches
	Public Const ACGetBranchesStored As Boolean = True
	Public Const ACGetBranchesName As String = "GetBranches"
    Public Const ACGetBranchesSQL As String = "spu_pm_get_user_sources"
End Module