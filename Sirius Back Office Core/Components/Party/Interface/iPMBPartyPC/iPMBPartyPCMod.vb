Option Strict Off
Option Explicit On
Imports System
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 23/06/1998
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	'sj 21/08/2002 - start
	Public Structure QASNamesData
		Dim Title As String
		Dim Forename As String
		Dim Surname As String
		Dim Initial As String
		Dim Postcode As String
		Dim OrgName As String
		Dim Add1 As String
		Dim Add2 As String
		Dim Add3 As String
		Dim Add4 As String
		Dim IsOrg As Boolean
		Public Shared Function CreateInstance() As QASNamesData
			Dim result As New QASNamesData
			result.Title = String.Empty
			result.Forename = String.Empty
			result.Surname = String.Empty
			result.Initial = String.Empty
			result.Postcode = String.Empty
			result.OrgName = String.Empty
			result.Add1 = String.Empty
			result.Add2 = String.Empty
			result.Add3 = String.Empty
			result.Add4 = String.Empty
			Return result
		End Function
	End Structure
	'sj 21/08/2002 - end
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "uctPartyPCControl"
	Public Const ACStatusActWebLoading As String = "Loading..........."
	Public Const AddressImage As String = "AddressImage"
	Public Const ContactImage As String = "ContactImage"
	Public Const LifestyleImage As String = "LifestyleImage"
	Public Const ConvictionImage As String = "ConvictionImage"
	Public Const CampaignImage As String = "CampaignImage"
	Public Const PolicyImage As String = "PolicyImage"
	Public Const CreditCard As String = "Credit Card"
	Public Const DebitCard As String = "Debit Card"
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACTabTitle2 As Integer = 102
	Public Const ACTabTitle3 As Integer = 103
	Public Const ACTabTitle4 As Integer = 104
	Public Const ACTabTitle5 As Integer = 105
	
	Public Const ACReference As Integer = 106
	Public Const ACSurname As Integer = 107
	Public Const ACForename As Integer = 108
	Public Const ACTitle As Integer = 109
	Public Const ACInitials As Integer = 110
	Public Const ACIsAgent As Integer = 111
	Public Const ACAgent As Integer = 112
	Public Const ACName As Integer = 113
	Public Const ACAssociate As Integer = 114
	Public Const ACAssociateCode As Integer = 115
	Public Const ACArea As Integer = 116
	Public Const ACFileCode As Integer = 117
	Public Const ACConsultant As Integer = 118
	Public Const ACContacts As Integer = 119
	Public Const ACPaymentDetails As Integer = 120
	Public Const ACCurrency As Integer = 121
	Public Const ACPaymentMethod As Integer = 122
	Public Const ACReminderType As Integer = 123
	Public Const ACServiceLevel As Integer = 124
	Public Const ACCreditCard As Integer = 125
	Public Const ACTermsOfPayment As Integer = 126
	Public Const ACEmploymentDetails As Integer = 127
	Public Const ACOccupation As Integer = 128
	Public Const ACEmployer As Integer = 129
	Public Const ACStatus As Integer = 130
	Public Const ACCCJ As Integer = 131
	Public Const ACLifestyle As Integer = 132
	Public Const ACDOB As Integer = 133
	Public Const ACMaritalStatus As Integer = 134
	Public Const ACSeasonalGift As Integer = 135
	Public Const ACGender As Integer = 136
	Public Const ACNationality As Integer = 137
	Public Const ACOrigin As Integer = 138
	Public Const ACMailshot As Integer = 139
	Public Const ACPets As Integer = 140
	Public Const ACSmoker As Integer = 141
	Public Const ACAccommodation As Integer = 142
	Public Const ACDependants As Integer = 143
	Public Const ACProspecting As Integer = 144
	Public Const ACAgentReference As Integer = 145
	Public Const ACProspectStatus As Integer = 146
	Public Const ACCurrentAgent As Integer = 147
	Public Const ACTargetPremium As Integer = 148
	Public Const ACCampaigns As Integer = 149
	Public Const ACPolicies As Integer = 150
	
	' TF031298
	'Public Const ACFinancial = 150
	'Public Const ACPolicy = 151
	'Public Const ACClaim = 152
	'Public Const ACNotes = 153
	'Public Const ACLetter = 154
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	Public Const ACAddButton As Integer = 204
	Public Const ACEditButton As Integer = 205
	Public Const ACDeleteButton As Integer = 206
	Public Const ACLookupButton As Integer = 207
	Public Const ACProspectButton As Integer = 208
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACEmployerMissing As Integer = 304
	Public Const ACAgentMissing As Integer = 305
	Public Const ACRefExists As Integer = 306
	Public Const ACConsultantMissing As Integer = 307
	Public Const ACAssociateMissing As Integer = 308
	' Menus
	
	
	' {* USER DEFINED CODE (End) *}
	
    'JDW for transferring QAS Names data to frmInterface
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public m_sQASSurname As String = ""

    'Developer Guide No. 107
    <ThreadStatic()> _
 Public m_sQASForename As String = ""
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public m_sQASInitial As String = ""
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public m_sQASTitle As String = ""
    Public m_oQASData As QASNamesData = QASNamesData.CreateInstance() 'defined in uctpartypc

    ' Public contants used for the start
    ' and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Public source and language ID's from the
    ' Object Manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iCurrencyId As Integer

    ' Public instance of the object manager.
    'Developer Guide No. 107(Guide)
    <ThreadStatic()>
    Public g_oObjectManager As bObjectManager.ObjectManager
    'Global reference to ListManager
    'eck120500 in PMBGeneralFunc
    'Public g_oListManager As Object
    'eck150500
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oGIS As Object
	Public Const ScreenHelpID As Integer = 3
	

	Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
	
	' Constants for the Policies array
	Public Const PMBPolicyPolicyID As Integer = 0
	Public Const PMBPolicyPolicyTypeID As Integer = 1
	Public Const PMBPolicyRenewalDate As Integer = 2
	Public Const PMBPolicyNoOfTimesQuoted As Integer = 3
	Public Const PMBPolicyTypeDescription As Integer = 4
	Public Const PMBPolicyTargetPremium As Integer = 5
	
	' Constants for the Campaigns array
	Public Const PMBCampaignRecordNo As Integer = 0
	Public Const PMBCampaignCampaignID As Integer = 1
	Public Const PMBCampaignCampaignDate As Integer = 2
	Public Const PMBCampaignDescription As Integer = 3
	
	Sub Main_Renamed()
		
		'Dim o As iPMBPartyPC.Interface
		'Dim lReturn As Long
		'Dim vKeyArray As Variant
		'Dim s As String
		'
		'    Set o = New Interface
		'
		'    ReDim vKeyArray(1, 0)
		'
		'    s = MsgBox("yes to add, on to edit", vbYesNo)
		'
		'    If s = vbYes Then
		'
		'        'New ***************************************
		'        lReturn = o.Initialise()
		'
		'        o.CallingAppName = "TEST"
		'
		'        lReturn = o.SetProcessModes( _
		''            vTask:=PMAdd)
		'
		'        'New ***************************************
		'
		'    Else
		'
		'    'Edit ***************************************
		'        s = InputBox("Enter Party_cnt ", , 554)
		'        vKeyArray(0, 0) = PMKeyNamePartyCnt
		'                vKeyArray(1, 0) = CLng(s)
		'
		'        lReturn = o.SetKeys(vKeyArray)
		'        lReturn = o.Initialise()
		'
		'        o.CallingAppName = "TEST"
		'
		'        lReturn = o.SetProcessModes( _
		''            vTask:=PMEdit)
		'
		'    'Edit ***************************************
		'
		'    End If
		'
		'    lReturn = o.Start()
		'
		'    lReturn = o.GetKeys(vKeyArray)
		'
		'    'MsgBox "Are we done?..." & o.StepStatus
		'
		''    If o.Status = PMOK Then
		''        ' must change to InsuranceFileCnt! - JW
		''        MsgBox "SELECTED: " & o.PartyCnt & ", " & _
		'''        o.PartyCnt
		''    End If
		'
		'    lReturn = o.Terminate()
		'
		'    Set o = Nothing
		
	End Sub
End Module