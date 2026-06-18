Option Strict Off
Option Explicit On
Imports System
'developer guide no. 129
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 10/05/1999
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMUMaintainUARule"

    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As Object
    'Public g_oBusiness As bSIRMaintainAURule.Business
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sDataModelCode As String = ""
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACLblAuthorityLevel As Integer = 102
	Public Const ACLblProduct As Integer = 103
	Public Const ACLblTransactionType As Integer = 104
	Public Const ACLblIsUnderwriter As Integer = 105
	Public Const ACLblRuleSet As Integer = 106
	Public Const ACLblCode As Integer = 107
	Public Const ACLblDescription As Integer = 108
	Public Const ACLblEffectiveDate As Integer = 109
	Public Const ACLblRuleFile As Integer = 110
	Public Const ACLblLive As Integer = 111
	Public Const ACLinkTitle As Integer = 112
	Public Const ACRuleTitle As Integer = 113
	Public Const ACTabRuleDetails As Integer = 114
	Public Const ACAvailableRuleSetsTitle As Integer = 115
	Public Const ACSelectRuleSet As Integer = 116
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	Public Const ACInterfaceFailTitle As Integer = 304
	Public Const ACInterfaceFail As Integer = 305
	Public Const ACMandatoryFields As Integer = 306
	Public Const ACMandatoryFieldsTitle As Integer = 307
	
	' Menus
	
	
	' {* USER DEFINED CODE (End) *}
	
	
	'Array constants for Rule Links.
	Public Const ACRuleFieldMaxIndex As Integer = 13
	Public Const ACUARuleId As Integer = 0
	Public Const ACUARuleCaptionId As Integer = 1
	Public Const ACUARuleCode As Integer = 2
	Public Const ACUARuleDescription As Integer = 3
	Public Const ACUARuleEffectiveDate As Integer = 4
	Public Const ACUARuleFileName As Integer = 5
	Public Const ACUARuleLive As Integer = 6
	Public Const ACUARuleTypeId As Integer = 7
	Public Const ACUARuleIsUnderwriter As Integer = 8
	Public Const ACUARuleProductId As Integer = 9
	Public Const ACUARuleTransTypeId As Integer = 10
	Public Const ACUARuleAuthTypeDescription As Integer = 11
	Public Const ACUARuleProductDescription As Integer = 12
	'sj 01/04/2003 - start
	Public Const ACUARuleTransactionTypeDescription As Integer = 13
	'sj 01/04/2003 - end
	
	'Action Array constants.
	Public Const ACActionAdd As Integer = 1
	Public Const ACActionUpdate As Integer = 2
	Public Const ACActionDelete As Integer = 3
	
	Public Const ACOpenFolder As String = "Open"
	Public Const ACClosedFolder As String = "Closed"
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	
	'Public constants for rule sets
	Public Const ACRuleSetMaxIndex As Integer = 9
	Public Const ACRuleSetId As Integer = 0
	Public Const ACRuleSetCaptionId As Integer = 1
	Public Const ACRuleSetCode As Integer = 2
	Public Const ACRuleSetDescription As Integer = 3
	Public Const ACRuleSetIsDeleted As Integer = 4
	Public Const ACRuleSetEffectiveDate As Integer = 5
	Public Const ACRuleSetType As Integer = 6
	Public Const ACRuleSetFileName As Integer = 7
	Public Const ACRuleSetLive As Integer = 8
	Public Const ACRuleSetCaption As Integer = 9
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
    ' Object Manager.
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
	
	' KB 100801 required for help text, screenhelpid will need to be amended
	' when text available
	Public Const ScreenHelpID As Integer = 4001
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
	
	
	Sub Main_Renamed()
		
	End Sub
End Module