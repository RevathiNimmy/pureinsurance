Option Strict Off
Option Explicit On
Imports System
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 17/02/1997
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' SP130199 - Remove NavigatorV3 class an put in stub so can be called
	' iteratively.
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iCLMFindParty"
	'ECK 18/5/99
	Public Const ThisApp As String = "Client Manager" ' Registry App constant.
	Public Const ThisKey As String = "Recent Files" ' Registry Key constant.
	
	' Public constants declared for the claim_party_type
	Public Const ACIDriver As String = "Driver"
	Public Const ACIRepairer As String = "Repairer"
	Public Const ACIThirdParty As String = "Third Party"
	Public Const ACIWitness As String = "Witness"
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' General Icons
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACTabTitle2 As Integer = 102
	
	Public Const ACPartyClaimID As Integer = 104
	Public Const ACName As Integer = 105
	Public Const ACType As Integer = 106
	Public Const ACAddress As Integer = 108
	Public Const ACPhoneNumber As Integer = 110
	
	Public Const ACListTitle1 As Integer = 120
	Public Const ACListTitle2 As Integer = 121
	Public Const ACListTitle3 As Integer = 122
	Public Const ACListTitle5 As Integer = 124
	Public Const ACListTitle6 As Integer = 125
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	
	Public Const ACFindNowButton As Integer = 204
	Public Const ACNewSearchButton As Integer = 205
	Public Const ACNewButton As Integer = 206
	Public Const ACEditButton As Integer = 207
	
	Public Const ACDeleteButton As Integer = 208
	Public Const ACUndeleteButton As Integer = 209
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACClearDetailsTitle As Integer = 304
	Public Const ACClearDetails As Integer = 305
	Public Const ACStatusSearching As Integer = 306
	Public Const ACStatusFound As Integer = 307
	Public Const ACClientCodeTitle As Integer = 308
	Public Const ACClientCode As Integer = 309
	
	' Menus
	
	' Constants for the search data array indexes.
	Public Const ACIPartyType As Integer = 1
	Public Const ACIPartyClaimID As Integer = 0
	Public Const ACIName As Integer = 2
	Public Const ACIAddress As Integer = 3
	'DC120701 more than one line of address for Broking
	Public Const ACIAddress1 As Integer = 3
	Public Const ACIAddress2 As Integer = 4
	Public Const ACIAddress3 As Integer = 5
	Public Const ACIAddress4 As Integer = 6
	Public Const ACIPostcode As Integer = 7
	'DC120701
	Public Const ACILicenseType As Integer = 8
	Public Const ACILicenseTypeDescription As Integer = 9
	Public Const ACILicenseNumber As Integer = 10
	Public Const ACIDOB As Integer = 11
	Public Const ACISex As Integer = 12
	Public Const ACIPartyStatus As Integer = 13
	Public Const ACIPartyStatusDescription As Integer = 14
	'Public Const ACIPartyID = 7
	Public Const ACIPhoneNumber As Integer = 16
	Public Const ACIFaxNumber As Integer = 17
	Public Const ACIRegNumber As Integer = 18
	
	
	'sj 3/11/99 - start
	Public Const ACIInvariantKey As Integer = 11
	Public Const ACISource As Integer = 12
	Public Const ACIMax As Integer = 12
	
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the maxiumum search details.
	Public Const ACMaxSearchDetails As Integer = 500
	
	' Constant for the miniumum search length.
	'Modified by ECK 11/05/99
	Public Const ACMinSearchLength As Integer = 1
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
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
 Public g_oBusiness As bCLMFindParty.Business
    'eck160500
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oPMUser As Object

	Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
	'DC120701 for BackOfficeLink
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBOBusiness As bBackOfficeLink.bBOLink
	Public Const ScreenHelpID As Integer = 1
	
    'sj 3/11/99 - start
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_bGenericConnectionStatus As Boolean
	'sj 3/11/99 - end
	
	' constants declarations for the Claim Party Types
	Public Const g_iDriver As Integer = 1
	Public Const g_iThirdParty As Integer = 2
	Public Const g_iRepairer As Integer = 3
	Public Const g_iWitness As Integer = 4
	
    ' variable declarations for the properties
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_lPartyClaimID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_sName As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_sAddress As String = ""
    'DC120701 more than one line of address for Broking
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_sAddress1 As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_dAddress2 As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_sAddress3 As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_sAddress4 As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_sPostcode As String = ""
    'DC120701
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_sPhoneNumber As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_sClaimPartyType As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_lClaimPartyTypeID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_lClaimID As Integer
	' UNCOMMENT THIS PART FOR INTEGRATION*********************************
	'Public Const m_lClaimPartyTypeID As Long = 1
	'Public Const m_lClaimID As Long = 14
	'*********************************************************************
	
    ' variables declared for the Party Screens
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_sPartyName As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_sPartyAddress As String = ""
    'DC120701 more than one line of address for Broking
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_sPartyAddress1 As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_sPartyAddress2 As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_sPartyAddress3 As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_sPartyAddress4 As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_sPartyPostcode As String = ""
    'DC120701
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_sPartyPhoneNumber As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vLicensetype As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vLicenseDescription As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vLicenseNumber As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vDOB As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vSex As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vPartyStatus As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vDriverStatusDescription As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vPhoneNumber As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vFaxNumber As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vReferenceNumber As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vRegNumber As String = ""
	
	'#Public Const PMKeyNameInsFileCnt = "insurance_file_cnt"
	Public Const PMKeyNameOperateMode As String = "claim_mode"
	Public Const PMKeyNameClaimCnt As String = "claim_cnt"
	Public Const PMKeyNamePartyClaimID As String = "party_claim_id"
	Public Const PMKeyNameClaimPartyTypeID As String = "claim_party_type_id"
	Public Const PMKeyNamePartyName As String = "claim_party_name"
	Public Const PMKeyNameAddress As String = "claim_party_address"
	'DC120701 more than one line of address for Broking
	Public Const PMKeyNameAddress1 As String = "claim_party_address1"
	Public Const PMKeyNameAddress2 As String = "claim_party_address2"
	Public Const PMKeyNameAddress3 As String = "claim_party_address3"
	Public Const PMKeyNameAddress4 As String = "claim_party_address4"
	Public Const PMKeyNamePostcode As String = "claim_party_postcode"
	'DC120701
	Public Const PMKeyNameLicense_type As String = "License_type"
	Public Const PMKeyNameLicensetypedescription As String = "License_type_description"
	Public Const PMKeyNameLicense_Number As String = "License_Number"
	Public Const PMKeyNameDate_of_Birth As String = "Date_of_Birth"
	Public Const PMKeyNameSex As String = "Sex"
	Public Const PMKeyNameparty_status As String = "party_status"
	Public Const PMKeyNamedriver_status_description As String = "driver_status_description"
	Public Const PMKeyNamePhone_Number As String = "Phone_Number"
	Public Const PMKeyNameFax_Number As String = "Fax_Number"
	Public Const PMKeyNameReference_Number As String = "Reference_Number"
	Public Const PMKeyNameReg_Number As String = "Reg_Number"
End Module