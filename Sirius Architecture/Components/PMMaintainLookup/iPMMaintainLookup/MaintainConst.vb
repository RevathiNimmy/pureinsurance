Option Strict Off
Option Explicit On
Imports System
Module MaintainConst
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
	Public Const ACIsEdit As Integer = 6 'PN45545
	Public Const ACExtraStart As Integer = 7
	
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
	'DJM 08/03/2004
	Public Const ACColumnPrecision As Integer = 5
	Public Const ACColumnScale As Integer = 6
	
	Public Const ACTagDelete As String = "Delete"
	Public Const ACTagUndelete As String = "Undelete"
	
	Public Const ACExtraCaption As Integer = 0
	Public Const ACExtraValue As Integer = 1
	Public Const ACExtraLength As Integer = 2
	Public Const ACExtraOffset As Integer = 3
	Public Const ACExtraType As Integer = 4
	Public Const ACExtraLookupTable As Integer = 5
	'DJM 08/03/2004
	Public Const ACExtraPrecision As Integer = 6
	Public Const ACExtraScale As Integer = 7
	
	' Key Columns
	Public Const ACKeyColumnsIndex As Integer = 0
    Public Const ACKeyColumnsTable As Integer = 1

    Public Const ACItemCode As Integer = 0
    Public Const ACItemDescription As Integer = 1
    Public Const ACItemEffectiveDate As Integer = 2
    Public Const ACItemMaximum As Integer = 2
	
	' Extra types
	Public Enum ACExtraControlType
		ACExtraTypeTextBox = 0
		ACExtraTypeLookup = 1
        ACExtraTypeCheckBox = 2
        ACExtraTypeButton = 3
	End Enum
End Module