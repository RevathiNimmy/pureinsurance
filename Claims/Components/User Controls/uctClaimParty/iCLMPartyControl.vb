Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 8/8/2000
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	Public Const ACApp As String = "uctClaimPartyControl"
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
	' Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	Public g_iUserId As Integer
	
    'Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As Object
	
	'Constants for Set Key rows
	Public Const ACOperateModeRowSetKeys As Integer = 0
	Public Const ACPartyTypeIDRowSetKeys As Integer = 1
	Public Const ACClaimCntRowSetKeys As Integer = 2
	Public Const ACPartyIDRowSetKeys As Integer = 3
	
	'Constants used for resizing & positioning the user control
	Public Const ACListViewTop As Integer = 8
	Public Const ACcmdAddPartyTop As Integer = 30
	Public Const ACCommandButtonWidth As Integer = 1095
	Public Const ACCommandButtonHeight As Integer = 330
	Public Const ACVerticalGapBetweenTwoButtons As Integer = 45
	
	' Constants for key names
	Public Const PMKeyNamePartyClaimid As String = "party_claim_id"
	Public Const PMKeyNameClaimPartyTypeID As String = "claim_party_type_id"
	Public Const PMKeyNamePartyName As String = "claim_party_name"
	Public Const PMKeyNamePartyCnt As String = "party_cnt"
	Public Const PMKeyNamePartyOther As String = "party_other"
	
	'DC050601 -start -cater for more than 1 address line
	Public Const PMKeyNameAddress1 As String = "claim_party_address1"
	Public Const PMKeyNameAddress2 As String = "claim_party_address2"
	Public Const PMKeyNameAddress3 As String = "claim_party_address3"
	Public Const PMKeyNameAddress4 As String = "claim_party_address4"
	Public Const PMKeyNamePostCode As String = "claim_party_postcode"
	'DC050601 -end
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
	Public Const ACFirstSubItem As Integer = 1
	Public Const ACSecondSubItem As Integer = 2
	Public Const ACThirdSubItem As Integer = 3
	Public Const ACFourthSubItem As Integer = 4
	Public Const PMKeyNameReg_Number As String = "Reg_Number"
	Public Const ACFirstRow As Integer = 0
	Public Const ACSecondRow As Integer = 1
	
	
	' Resource string constants
	Public Const ACAddParty As Integer = 100
	Public Const ACDeleteParty As Integer = 101
	Public Const ACEditParty As Integer = 102
	Public Const ACNameParty As Integer = 103
	Public Const ACAddressParty As Integer = 104
	Public Const ACPhoneParty As Integer = 105
	Public Const ACSelectPartyForModification As Integer = 106
	Public Const ACSelectPartyForDeletion As Integer = 107
	Public Const ACDeletePartyMessage As Integer = 108
	Public Const ACDeletePartyConfirmation As Integer = 109
	Public Const ACAddPartyTitle As Integer = 111
	Public Const ACPartyAlreadyPresentMessage As Integer = 112
	Public Const ACLicenseNumber As Integer = 113
	Public Const ACDOB As Integer = 114
	Public Const ACSex As Integer = 115
	Public Const ACStatus As Integer = 116
	Public Const ACMale As Integer = 117
	Public Const ACFemale As Integer = 118
	Public Const ACRES_CONTACTNAME As Integer = 119
	Public Const ACRES_CONTACTNUMBER As Integer = 120
	Public Const ACRES_PARTYTYPE As Integer = 121
	
	Public Const ACColPartyCnt As Integer = 0
	Public Const ACColName As Integer = 1
	Public Const ACColAddressLine1 As Integer = 2
	Public Const ACColPhone As Integer = 3
	Public Const ACColLicenseNumber As Integer = 4
	Public Const ACColDOB As Integer = 5
	Public Const ACColGender As Integer = 6
	Public Const ACColStatus As Integer = 7
	'S4B Claim Enhancements R&D 2005
	Public Const ACColContactName As Integer = 8
	Public Const ACColContactTelephone As Integer = 9
	Public Const ACColPartyTypeCode As Integer = 10
	Public Const ACColPartyTypeDescription As Integer = 11
	Public Const ACCOL_INDEX_LOWER As Integer = 0
	Public Const ACCOL_INDEX_UPPER_A As Integer = 11
	Public Const ACCOL_INDEX_UPPER_U As Integer = 7
	
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
End Module