Option Strict Off
Option Explicit On
Imports System
Module MaintainConst
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Module Name: MaintainConst
	'
	' Date: 14/05/99
	'
	' Description: Constants used by both business and interface
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MaintainConst"
	
	Public Const ACColumnID As Integer = 0
	Public Const ACCaptionID As Integer = 1
	Public Const ACCode As Integer = 2
	Public Const ACDescription As Integer = 3
	Public Const ACIsDeleted As Integer = 4
	Public Const ACEffectiveDate As Integer = 5
	Public Const ACExtraStart As Integer = 6
	
	Public Const ACColumnNameCaptionID As String = "caption_id"
	Public Const ACColumnNameCode As String = "code"
	Public Const ACColumnNameDescription As String = "description"
	Public Const ACColumnNameIsDeleted As String = "is_deleted"
	Public Const ACColumnNameEffectiveDate As String = "effective_date"
	
	'Public Const ACColumnID = 0 'Already defined above with same value
	Public Const ACColumnName As Integer = 1
	Public Const ACColumnType As Integer = 2
	Public Const ACColumnLength As Integer = 3
	Public Const ACColumnOffset As Integer = 4
	Public Const ACColumnForeignKey As Integer = 5
	
	Public Const ACTagDelete As String = "Delete"
	Public Const ACTagUndelete As String = "Undelete"
	
	Public Const ACExtraCaption As Integer = 0
	Public Const ACExtraValue As Integer = 1
	Public Const ACExtraLength As Integer = 2
	Public Const ACExtraOffset As Integer = 3
	Public Const ACExtraType As Integer = 4
	Public Const ACExtraLookupTable As Integer = 5
	
	' Key Columns
	Public Const ACKeyColumnsIndex As Integer = 0
	Public Const ACKeyColumnsTable As Integer = 1
	
	' Extra types
	Public Const ACExtraTypeTextBox As Integer = 0
	Public Const ACExtraTypeLookup As Integer = 1
	Public Const ACExtraTypeCheckBox As Integer = 2
	
	'SD 09/08/2002
	Public Const sSingleQuote As String = "'"
End Module