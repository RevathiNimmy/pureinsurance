Option Strict Off
Option Explicit On
Imports System
'refer developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 23/10/2000
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMBPFScheme"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
	Public Const ScreenHelpID As Integer = 1
	
	' General Icons
	
    Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	
	Public Const ACCompanyNo As Integer = 110
	Public Const ACSchemeNo As Integer = 111
	Public Const ACSchemeVersion As Integer = 112
	Public Const ACSchemeName As Integer = 113
	Public Const ACSchemeDescription As Integer = 114
	Public Const ACStartDate As Integer = 115
	Public Const ACEndDate As Integer = 116
	
	Public Const ACPartyCode As Integer = 117
	Public Const ACPartyName As Integer = 118
	
	Public Const ACPaymentMethod As Integer = 119
	Public Const ACNoOfInstallments As Integer = 120
	Public Const ACInsrMailboxNo As Integer = 121
	Public Const ACEdiMessageCount As Integer = 122
	
	Public Const ACBasisOfCalc As Integer = 123
	Public Const ACBasisOfCalcNew As Integer = 124
	Public Const ACBasisOfCalcMta As Integer = 125
	Public Const ACBasisOfCalcPP As Integer = 126
	Public Const ACBasisOfCalcRenewal As Integer = 127
	Public Const ACBasisOfCalcCancel As Integer = 128
	
	Public Const ACDocumentPath As Integer = 129
	Public Const ACDocumentName As Integer = 130
	Public Const ACDocQuote As Integer = 131
	Public Const ACDocBank As Integer = 132
	Public Const ACDocCreditDetail As Integer = 133
	
	Public Const ACQuoteableInd As Integer = 134
	Public Const ACProductFamily As Integer = 135
	Public Const ACSystemTag As Integer = 136
	Public Const ACDataModelCode As Integer = 137
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	Public Const ACRatesButton As Integer = 204
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	' Alix - 08/04/2003
	Public Const ACDocConfirmation As Integer = 305
	
	'PF Client Fee
	Public Const ACAllowClientFees As Integer = 306
	
	'(RC) PLICO 9-10
	Public Const ACRatesForInformationOnly As Integer = 307
	
	' Menus
	
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
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	' Media Type Validation Constants
	Public Const ACMediaTypeCash As String = "CASH"
	Public Const ACMediaTypeCreditCard As String = "CC"
	Public Const ACMediaTypeBank As String = "BANK"
	Public Const ACMediaTypeBasic As String = "BASIC"
	
	Sub Main_Renamed()
		
	End Sub
End Module