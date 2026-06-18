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
	Public Const ACApp As String = "iPMUMaintainAuthority"
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As Object
	'Public g_oBusiness As bSIRMaintainAuthority.Business
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACLblUser As Integer = 102
	Public Const ACLblProduct As Integer = 103
	Public Const ACLblAuthorityLevel As Integer = 104
	
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
	Public Const ACGetUsersFail As Integer = 304
	Public Const ACGetProductsFail As Integer = 305
	Public Const ACGetAuthorityLevelsFail As Integer = 306
	Public Const ACGetAuthorityLevelsNotSetUp As Integer = 307
	Public Const ACGetSelectFromList As Integer = 308
	
	' Menus
	
	
	' {* USER DEFINED CODE (End) *}
	
	Public Const ACActionAdd As gPMConstants.PMEComponentAction = gPMConstants.PMEComponentAction.PMAdd
	Public Const ACActionUpdate As gPMConstants.PMEComponentAction = gPMConstants.PMEComponentAction.PMEdit
	Public Const ACActionDelete As gPMConstants.PMEComponentAction = gPMConstants.PMEComponentAction.PMDelete
	
	'Array constants for PMUser.
	Public Const ACPMUserId As Integer = 0
	Public Const ACPMUserName As Integer = 1
	
    'Array constants for User Authority Levels.
    'ACUserAuthMaxIndex Increased to 4 because the SP returns description too.
    Public Const ACUserAuthMaxIndex As Integer = 4
	Public Const ACUserAuthProductId As Integer = 0
	Public Const ACUserAuthProductDesc As Integer = 1
	Public Const ACUserAuthLevelTypeId As Integer = 2
	Public Const ACUserAuthLevelTypeDesc As Integer = 3
	
	'Array constants for Product.
	Public Const ACProductId As Integer = 0
	Public Const ACProductCaptionId As Integer = 1
	Public Const ACProductCode As Integer = 2
	Public Const ACProductDesc As Integer = 3
	Public Const ACProductEffDate As Integer = 4
	Public Const ACProductIsDeleted As Integer = 5
	Public Const ACProductSchemeAgencyRef As Integer = 6
	Public Const ACProductBlockNo As Integer = 7
	Public Const ACProductTaxSuppressed As Integer = 8
	Public Const ACProductQuoteNumberingId As Integer = 9
	Public Const ACProductShortPeriodRated As Integer = 10
	Public Const ACProductMidnightRenewal As Integer = 11
	Public Const ACProductAutoRenewable As Integer = 12
	Public Const ACProductRenewalWeeks As Integer = 13
	Public Const ACProductPolicyNumberingId As Integer = 14
	Public Const ACProductProvNumberingId As Integer = 15
	Public Const ACProductFullNumberingId As Integer = 16
	Public Const ACProductAccumulation As Integer = 17
	Public Const ACProductRiPointer As Integer = 18
	Public Const ACProductReportPointer As Integer = 19
	Public Const ACProductClaimYearToCheck As Integer = 20
	Public Const ACProductMaxSingleClaim As Integer = 21
	Public Const ACProductMaxClaims As Integer = 22
	Public Const ACProductTotalOfClaims As Integer = 23
	
	'Array constants for Authority Level Type.
	Public Const ACALTId As Integer = 0
	Public Const ACALTCaptionId As Integer = 1
	Public Const ACALTCode As Integer = 2
	Public Const ACALTDesc As Integer = 3
	Public Const ACALTIsDeleted As Integer = 4
	Public Const ACALTEffDate As Integer = 5
	
	
	Public Const ACOpenFolder As String = "Open"
	Public Const ACClosedFolder As String = "Closed"
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	
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