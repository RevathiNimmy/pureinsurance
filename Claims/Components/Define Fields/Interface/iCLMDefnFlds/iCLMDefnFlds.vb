Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
'Developer Guide No: 129
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 15/07/2000
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:SK
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iCLMDefnFlds"
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	Public Const ACRiskMode As gPMConstants.PMEComponentAction = gPMConstants.PMEComponentAction.PMView 'when screen is being called from Risk Type screen
	Public Const ACPerilMode As gPMConstants.PMEComponentAction = gPMConstants.PMEComponentAction.PMAdd 'when screen is being called from Peril Type screen
	
	
	Public Const ACAdd As Integer = 0 'Constant for ADD button index
	Public Const ACModify As Integer = 1 'Constant for MODIFY button index
	Public Const ACDelete As Integer = 2 'Constant for DELETE button index
	
	'Constant for TYPE index
	Public Const ACText As Integer = 1
	Public Const ACInteger As Integer = 2
	Public Const ACDate As Integer = 3
	Public Const ACYesNo As Integer = 4
	Public Const ACLookUp As Integer = 5
	Public Const ACParty As Integer = 6
	'DC140302
	Public Const ACTabName As Integer = 7
	
	
	' General Icons
	'RESOURCE FILE CONSTANTS
	' Form
	Public Const ACInterfaceTitle As Integer = 100 'Data Definition
	Public Const ACTabTitle1 As Integer = 101 '&1-General
	Public Const ACInterfaceTitle1 As Integer = 225 'Risk Type
	Public Const ACInterfaceTitle2 As Integer = 226 'Peril Type
	
	'Labels
	Public Const AClblCaption As Integer = 104 'Caption :
	Public Const AClblDesc As Integer = 105 'Description :
	Public Const AClblMandatory As Integer = 106 'Mandatory ?
	Public Const AClblType As Integer = 107 'Type :
	Public Const AClblDispOrd As Integer = 108 'Display Order :
	Public Const AClblParty As Integer = 109 'Party :
	Public Const AClblLookup As Integer = 110 'Lookup :
	Public Const AClblReadOnly As Integer = 111 'Read-only ?
	Public Const ACManualEntry As Integer = 123 'Manual Entry ?
	
	
	'List-View Column Headers
	Public Const ACCaption As Integer = 116 'Caption
	Public Const ACDesc As Integer = 117 'Description
	Public Const ACMandatory As Integer = 118 'Mandatory
	Public Const ACType As Integer = 119 'Type
	Public Const ACDispOrd As Integer = 120 'Display Order
	Public Const ACPartyLookup As Integer = 121 'Party/Lookup
	Public Const ACReadOnly As Integer = 122 'Read-only ?
	
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACAddButton As Integer = 203 '&Add
	Public Const ACModButton As Integer = 204 '&Modify
	Public Const ACDelButton As Integer = 205 '&Delete
	
	'Type combo values
	Public Const ACcboText As Integer = 231
	Public Const ACcboInteger As Integer = 232
	Public Const ACcboDate As Integer = 233
	Public Const ACcboYesNo As Integer = 234
	Public Const ACcboLookUp As Integer = 235
	Public Const ACcboParty As Integer = 236
	'DC140302
	Public Const ACcboTabName As Integer = 237
	
	
	'Messages
	
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACClearDetailsTitle As Integer = 304
	Public Const ACClearDetails As Integer = 305
	Public Const ACStatusSearching As Integer = 306
	
	Public Const ACResvTypeNameMsg1 As Integer = 307 'The caption
	Public Const ACResvTypeNameMsg2 As Integer = 308 'already exists
	Public Const ACResvTypeNameTitle As Integer = 309 'Invalid Action
	Public Const ACResvTypeNameMsg3 As Integer = 401 'The Display Order
	Public Const ACResvTypeNameMsg4 As Integer = 402 'Cannot Delete
	Public Const ACResvTypeNameMsg5 As Integer = 403 'because it is used in
	Public Const ACResvTypeNameMsg8 As Integer = 404 'Cannot Modify
	Public Const ACResvTypeNameMsg6 As Integer = 310 'Display Order cannot exceed
	Public Const ACResvTypeNameMsg7 As Integer = 307 'The caption
	Public Const ACDefFieldsZeroMsg As Integer = 312 'The Zero
	Public Const ACMandatoryFieldMsg As Integer = 313 'This is a Mandatory field. You must enter data in this field.
	
	
	Public Const ACInvalidDateMsg As Integer = 400 'Invalid Date Entered
	
	
	
	
	' Constants for the search data array indexes.
	Public Const ACIInsuranceid As Integer = 0
	Public Const ACIClaimCnt As Integer = 1
	Public Const ACIClaimType As Integer = 2
	Public Const ACIClaimRef As Integer = 3
	Public Const ACIInsuranceRef As Integer = 4
	
	
	
	
	' Constants for Underwriting
	Public Const ACIURiskIndex As Integer = 5
	Public Const ACIUProductCode As Integer = 6
	Public Const ACIULossDate As Integer = 7
	Public Const ACIUPolicyHolder As Integer = 8
	
	'Constants for Broking
	Public Const ACIBLossDate As Integer = 5
	Public Const ACIBPolicyHolder As Integer = 6
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the maxiumum search details.
	Public Const ACMaxSearchDetails As Integer = 500
	
	' Constant for the miniumum search length.
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
    'Public g_oBackofficelink As bBackOfficeLink.bBOLink
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBackofficelink As Object
	' Public instance of the business object.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As bCLMDefnFlds.Business
	
    'holds the Risk/Peril Data Definition ID globally
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lDataDefnID As Integer
	
	Public Const ACColTabID As Integer = 0
	Public Const ACColTabName As Integer = 1
	
	Sub ComboFindText(ByRef cmb As ComboBox, ByRef sText As String)
		
		'select the row of the combobox matching the given text
		
		Dim bFound As Boolean
		
		For l As Integer = 0 To cmb.Items.Count - 1
			cmb.SelectedIndex = l
			If cmb.Text = sText Then
				bFound = True
				Exit For
			End If
		Next 
		
		If Not bFound Then cmb.SelectedIndex = -1
		
    End Sub

End Module