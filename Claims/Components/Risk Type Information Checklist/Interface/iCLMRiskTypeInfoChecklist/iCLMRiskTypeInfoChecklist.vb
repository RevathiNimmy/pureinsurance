
Option Strict Off
Option Explicit On
Imports System
'Developer Guide no. 129
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name:  MainModule
	' Description:  Main module containing public variable/constants.
	' Author:       SK
	' Date:         06/07/2000
	' ***************************************************************** '
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iCLMRiskTypeInfoChecklist"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	' Caption Constants for Tab on the Form
	Public Const ACInterfaceTitle As Integer = 100
	
	'Caption Constants for Labels on the form
	Public Const ACRiskType As Integer = 101
	Public Const ACDescription As Integer = 102
	Public Const ACInfoChkList As Integer = 103
	
	'Caption Constants for  Buttons
	Public Const ACAPPLYButton As Integer = 104
	Public Const ACOKButton As Integer = 105
	Public Const ACCancelButton As Integer = 106
	Public Const ACHelpButton As Integer = 107
	
	'Caption Constants for Messages to User
	Public Const ACSaveChangesTitle As Integer = 108
	Public Const ACSaveChangesMsg1 As Integer = 109
	Public Const ACSaveChangesMsg2 As Integer = 110
	Public Const ACExitSaveChangesMsg1 As Integer = 111
	Public Const ACExitSaveChangesMsg2 As Integer = 112
	Public Const ACTabCaption As Integer = 113
	
	'DC140201 added as they wern't here before
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	' Menus
	
	
	' {* USER DEFINED CODE (End) *}
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vLookupValues As Object
	
    ' Stores the search data from the lookup business object for currency,coinsurancetreatment.
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vLookupDetails As Object
	
	
	' Public source and language ID's from the
	' Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	
    ' Public instance of the business object.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As Object

    'Modified as per vb code
    'Public g_oBackOffice As bBackOfficeLink.bBOLink
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_oBackOffice As Object
	
	' Public variables redclared
	Public Const PMKeyNameClaimCnt As String = "claim_cnt" ' ClaimID
	Public Const PMKeyNameClaimReference As String = "claim_ref" ' Claim Number
	Public Const PMKeyNameFindClaimMode As String = "findclaim_mode" ' The PMMOde
	Public Const PMKeyNameInsFileCnt As String = "insurance_file_cnt" ' The PolicyID
	Public Const PMKeyNameInsReference As String = "insurance_file_ref" ' the policy ref
	
	' constants declaration for the controls
	Public Const g_cICLAIM_NUMBER As Integer = 0
	Public Const g_cITOTAL_SHARE_PERCENTAGE As Integer = 1
	Public Const g_cITOTAL_CURRENT_SHARE_VALUE As Integer = 2
	Public Const g_cITOTAL_NEW_SHARE_VALUE As Integer = 3
	Public Const g_cIADD As Integer = 3
	Public Const g_cIEDIT As Integer = 4
	Public Const g_cIDELETE As Integer = 5
	Public Const g_cIOK As Integer = 0
	Public Const g_cICANCEL As Integer = 1
	Public Const g_cIHELP As Integer = 2
	
	Public Const g_cbTRUE As Boolean = True
	Public Const g_cbFALSE As Boolean = False
	
	' constants declaration for frmDetails screen
	Public Const g_cISHARE_PERCENTAGE As Integer = 0
	Public Const g_cICURRENT_SHARE_VALUE As Integer = 1
	Public Const g_cINEW_SHARE_VALUE As Integer = 2
	
	' constants declared for FindPolicyMode
	'Public Const PMADD As Integer = 1
	'Public Const PMEDIT As Integer = 2
	'Public Const PMVIEW As Integer = 3
	
	'' temporary declaration for the variables
    'used to hold the value of the Risk Type specified in the combo
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_lrsk_type_id As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_bDataChanged As Boolean
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_bChecked As Boolean
    'developer guide no. 107
    <ThreadStatic()> _
    Public m_iRskTypeCboIndex As Integer


End Module