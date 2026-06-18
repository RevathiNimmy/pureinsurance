Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Module MainModule
	
	
	Public Const ACApp As String = "uctSIRSelectClauses"
	
	
	Private Const ACClass As String = "uctSIRSelectClauses"
	
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	
	Public Const kSelectClauseNewRiskTypeOrProductTypeAdded As Integer = 0
	Public Const kSelectClauseProductClauseId As Integer = 1
	Public Const kSelectClauseRiskClauseId As Integer = 2
	Public Const kDefaultRowIndex As Integer = 2
	Public Const kBranchesAccessDifferentCaption As String = "Branches Access(Different)"
	Public Const kBranchesAccessCaption As String = "Branches Access"
	Public Const kYesNoButton As Integer = 6
	
	'To set the columns
	Public Const kRegKeyConstLvwCode As Integer = 300
	Public Const kRegKeyConstLvwDescription As Integer = 301
	Public Const kRegKeyConstLvwSelected As Integer = 302
	Public Const kRegKeyConstLvwDefault As Integer = 303
	Public Const kRegKeyConstCancel As Integer = 304
	
	'**********************************************
	' Select Clasues list view constants
	'**********************************************
	Public Const kSelectClauseColHIndexCode As Integer = 0
	Public Const kSelectClauseColHIndexDescription As Integer = 1
	Public Const kSelectClauseColHIndexSelected As Integer = 2
	Public Const kSelectClauseColHIndexDefault As Integer = 3
	
	'**********************************************
	' Select Clasues listview width of the columns
	'**********************************************
	Public Const kSelectClauseWidthCode As Double = 1839.68
	Public Const kSelectClauseColWidthDescription As Double = 2860.15
	Public Const kSelectClauseColWidthSelected As Double = 1539.77
	Public Const kSelectClauseColWidthDefault As Double = 1539.77
	
	Public Const kSelectClauseCBNotCheckedIndex As CheckState = CheckState.Unchecked
	Public Const kSelectClauseCBCheckedIndex As CheckState = CheckState.Checked
	
	Public Const kSelectClauseDefaultIndex As Integer = 0
	Public Const kSelectClauseDescriptionIndex As Integer = 2
	
	Public Enum ENSelectClause
		id = 0
		code = 1
		Description = 2
		Selected = 3
		Default_Renamed = 4
		Branch = 5
	End Enum
	
	Public Enum ENClauseType
		ProductType = 1
		RiskType = 2
    End Enum
End Module