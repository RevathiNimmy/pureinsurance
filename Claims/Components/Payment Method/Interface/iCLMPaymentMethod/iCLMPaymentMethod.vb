Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 30/08/2000
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:Pandu
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iCLMPaymentMethod"
	
	' General Icons
	'RESOURCE FILE CONSTANTS
	
	Public Const PMBPartyNone As Integer = 0
	
	Public Const PMBPartyTypePersonalClientText As Integer = 1
	
	Public Const PMBPartyTypeCorporateClientText As Integer = 2
	
	Public Const PMBPartyTypeGroupClientText As Integer = 3
	
	
	Public Const PMBPartyTypeAgentText As Integer = 4
	
	
	
	
	' Form
	Public Const ACInterfaceTitlePayment As Integer = 100 'Payment Method
	Public Const ACTabTitle1 As Integer = 101 '&1-General
	Public Const ACInterfaceTitleReceipt As Integer = 102 'Receipt Method
	
	Public Const ACClaimPaymentAccount As Integer = 103
	Public Const ACClientPayableAccount As Integer = 104
	Public Const ACAgentPayableAccount As Integer = 105
	'DC110105
	Public Const ACThirdPartyPayableAccount As Integer = 124
	Public Const ACPartyPayableAccount As Integer = 106
	
	Public Const ACClaimReceivableAccount As Integer = 107
	Public Const ACClientReceivableAccount As Integer = 108
	Public Const ACAgentReceivableAccount As Integer = 109
	'DC110105
	Public Const ACThirdPartyReceivableAccount As Integer = 125
	Public Const ACPartyReceivableAccount As Integer = 110
	
	Public Const ACSelectPaymentMethod As Integer = 111
	Public Const ACSelectReceiptMethod As Integer = 112
	Public Const ACParty As Integer = 113
	Public Const ACAmount As Integer = 114
	Public Const ACComments As Integer = 115
	
	'DJM 22/03/2004
	Public Const ACTabTitle2 As Integer = 116 '&2-Payee
	Public Const ACPaymentInformation As Integer = 117
	Public Const ACPayee As Integer = 118
	Public Const ACPayeeName As Integer = 119
	Public Const ACBankName As Integer = 120
	Public Const ACSortCode As Integer = 121
	Public Const ACAccountNo As Integer = 122
	Public Const ACCountry As Integer = 123
	
	Public Const ACPaymentMethod As Integer = 0
	Public Const ACReceiptMethod As Integer = 1
	
	' AJM 12/03/01 - correct values of constants
	Public Const ACOptionValueSuspense As Integer = 0
	Public Const ACOptionValueThirdParty As Integer = 1
	Public Const ACOptionValueNominal As Integer = 2
	Public Const ACOptionValueClient As Integer = 3
	Public Const ACOptionNumber As Integer = 2002
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACDirectBusiness As Integer = 304
	Public Const ACAgentCancelled As Integer = 305
	
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
	Public g_lCountryID As Integer
	' RDC 16062004
	Public g_iUserID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
    ' Public instance of the business object.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As Object
	
	
	Public Const ACPaymentDetailsAmount As Integer = 0
	Public Const ACPaymentDetailsPartyCode As Integer = 1
	Public Const ACPaymentDetailsComments As Integer = 2
	Public Const ACPaymentDetailsPayeeMediaType As Integer = 3
	Public Const ACPaymentDetailsPayeeName As Integer = 4
	Public Const ACPaymentDetailsPayeeBankName As Integer = 5
	Public Const ACPaymentDetailsPayeeSortCode As Integer = 6
	Public Const ACPaymentDetailsPayeeAccountNo As Integer = 7
	Public Const ACPaymentDetailsPayeeCountry As Integer = 8
	Public Const ACPaymentDetailsPayeeComments As Integer = 9
	Public Const ACPaymentDetailsReserveID As Integer = 10 'eck 11/2005
	
	Public Const ACOptionNumberAltPayeeTab As Integer = 2020
	
	
	Sub Main_Renamed()
		
		'Dim o As iCLMFindClaim.Interface
		'Dim lReturn As Long
		'
		'    Set o = New Interface
		'    lReturn = o.Initialise()
		'    lReturn = o.SetProcessModes(vTask:=PMView)
		'    lReturn = o.Start
		'    lReturn = o.Terminate
		'
		'    Set o = Nothing
	End Sub
End Module