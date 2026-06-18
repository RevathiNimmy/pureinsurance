Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
 Public Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 23/06/1998
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMBPartyCC"
	
	'sj 27/08/2002 - start
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
	'sj 27/08/2002 - end
	
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
	
	' Tab 1
	Public Const ACClientCode As Integer = 106
	Public Const ACIsAgent As Integer = 107
	Public Const ACTradingName As Integer = 108
	Public Const ACProspect As Integer = 109
	Public Const ACBusinessDetails As Integer = 110
	Public Const ACBusiness As Integer = 111
	Public Const ACNoOfOffices As Integer = 112
	Public Const ACTrade As Integer = 113
	Public Const ACNoOfEmployees As Integer = 114
	Public Const ACCompanyReg As Integer = 115
	Public Const ACTradingSince As Integer = 116
	Public Const ACLeadAgent As Integer = 117
	Public Const ACConsultant As Integer = 136
	Public Const ACCode1 As Integer = 118
	Public Const ACCode2 As Integer = 119
	Public Const ACName1 As Integer = 120
	Public Const ACName2 As Integer = 121
	
	' Tab 2, 3, 4
	Public Const ACAdd As Integer = 122
	Public Const ACDelete As Integer = 123
	Public Const ACEdit As Integer = 124
	
	' Tab 4
	Public Const ACCCC As Integer = 125
	
	' Tab 5
	Public Const ACCurrency As Integer = 126
	Public Const ACServiceLevel As Integer = 127
	Public Const ACPaymentMethod As Integer = 128
	Public Const ACCreditCardType As Integer = 129
	Public Const ACReminderType As Integer = 130
	Public Const ACTermsOfPayment As Integer = 131
	Public Const ACFinancialYear As Integer = 132
	Public Const ACAssociates As Integer = 133
	Public Const ACArea As Integer = 134
	Public Const ACFileCode As Integer = 135
	
	
	' TF031298
	Public Const ACFinancial As Integer = 150
	Public Const ACNotes As Integer = 153
	Public Const ACLetter As Integer = 154
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	Public Const ACAddButton As Integer = 204
	Public Const ACEditButton As Integer = 205
	Public Const ACDeleteButton As Integer = 206
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACAgentMissing As Integer = 305
	Public Const ACRefExists As Integer = 306
	Public Const ACConsultantMissing As Integer = 307
	
	' Menus
	
	' {* USER DEFINED CODE (End) *}
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
    'JDW for transferring QAS Names data to frmInterface
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_sQASOrgName As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_sQASSurname As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_sQASForename As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_sQASInitial As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_sQASTitle As String = ""
	Public m_oQASData As QASNamesData = QASNamesData.CreateInstance() 'defined in uctpartycc
	
	' Public source and language ID's from the
    ' Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iCurrencyID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	'Global reference to ListManager
    'Global g_oListManager As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oGIS As Object
	' Constants for the Policies array
	Public Const PMBPolicyPolicyID As Integer = 0
	Public Const PMBPolicyPolicyTypeID As Integer = 1
	Public Const PMBPolicyRenewalDate As Integer = 2
	Public Const PMBPolicyNoOfTimesQuoted As Integer = 3
	Public Const PMBPolicyTypeDescription As Integer = 4
	
	' Constants for the Campaigns array
	Public Const PMBCampaignRecordNo As Integer = 0
	Public Const PMBCampaignCampaignID As Integer = 1
	Public Const PMBCampaignCampaignDate As Integer = 2
	Public Const PMBCampaignDescription As Integer = 3
	
	Public Const ScreenHelpID As Integer = 44000
	
	
	Sub Main_Renamed()
		
		Dim lReturn As Integer
        Dim vKeyArray(,) As Object
		
		Dim o As New Interface_Renamed
		
		ReDim vKeyArray(1, 0)
		
		Dim s As String = CStr(MessageBox.Show("yes to add, on to edit", Application.ProductName, MessageBoxButtons.YesNo))
		
		If s = System.Windows.Forms.DialogResult.Yes Then
			
			'New ***************************************
			lReturn = CType(o, SSP.S4I.Interfaces.ILocalInterface).Initialise()
			
			o.CallingAppName = "TEST"
			
			lReturn = o.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)
			
			'New ***************************************
			
		Else
			
			'Edit ***************************************
			s = Interaction.InputBox("Enter Party_cnt ",  , CStr(554))

			vKeyArray(0, 0) = PMNavKeyConst.PMKeyNamePartyCnt

			vKeyArray(1, 0) = CInt(s)
			

			lReturn = o.SetKeys(vKeyArray)
			lReturn = CType(o, SSP.S4I.Interfaces.ILocalInterface).Initialise()
			
			o.CallingAppName = "TEST"
			
			lReturn = o.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)
			
			'Edit ***************************************
			
		End If
		
		lReturn = o.Start()
		

		lReturn = o.GetKeys(vKeyArray)
		
		'MsgBox "Are we done?..." & o.StepStatus
		
		'    If o.Status = PMOK Then
		'        ' must change to InsuranceFileCnt! - JW
		'        MsgBox "SELECTED: " & o.PartyCnt & ", " & _
		''        o.PartyCnt
		'    End If
		
		o.Dispose()
		
		
	End Sub
End Module
