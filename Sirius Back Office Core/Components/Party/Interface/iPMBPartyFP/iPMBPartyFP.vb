Option Strict Off
Option Explicit On
Imports System
'Developer Guide No 129
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 17/10/2000
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History: TF171000 - Created
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMBPartyFP"
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACTabTitle2 As Integer = 102
	Public Const ACTabTitle3 As Integer = 103
	Public Const ACCode As Integer = 104
	Public Const ACName As Integer = 105
	Public Const ACNumber As Integer = 106
	Public Const ACAgencyNumber As Integer = 107
	Public Const ACMailboxNumber As Integer = 108
	Public Const ACCurrency As Integer = 109
	
	'TN07072000
	'PF23102001 - Updated to match resource file
	'Public Const ACTreatyNumber = 119
	Public Const ACTreatyNumber As Integer = 110
	
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
	
	Public Const ACRefExists As Integer = 304
	' Menus
	'Images
	Public Const AddressImage As String = "AddressImage"
	Public Const ContactImage As String = "ContactImage"
	
	
	' {* USER DEFINED CODE (End) *}
	
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
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iCurrencyID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	

	Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
	
	Public Const ScreenHelpID As Integer = 30
	
	'****************************************
	' Party Detail Array Position Constants
	Public Const kPartyDetailTaxNumber As Integer = 0
	Public Const kPartyDetailDomiciledForTax As Integer = 1
	Public Const kPartyDetailTaxExempt As Integer = 2
	Public Const kPartyDetailTaxPercentage As Integer = 3
	'****************************************
	
	
	Sub Main_Renamed()
		
		
	End Sub
End Module