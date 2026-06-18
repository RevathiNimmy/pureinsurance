Option Strict Off
Option Explicit On
Imports System
<System.Runtime.InteropServices.ProgId("GISPromptConstants_NET.GISPromptConstants")> _
 Public Module GISPromptConstants
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	Public Const PromptBusStatNewBusiness As String = "NB"
	Public Const PromptBusStatMidTermAdjustment As String = "MTA"
	Public Const PromptBusStatMidTermCancellation As String = "MTC"
	Public Const PromptBusStatRenewal As String = "RN"
	
	
	Public Const PromptPaymentMethodDirectDebit As String = "DD"
	Public Const PromptPaymentMethodDebitCard As String = "DC"
	Public Const PromptPaymentMethodCreditCard As String = "CC"
	
	Public Const PromptCardDescDelta As String = "Delta"
	Public Const PromptCardDescEuroCard As String = "Eurocard"
	Public Const PromptCardDescMasterCard As String = "Mastercard"
	Public Const PromptCardDescSwitch As String = "Switch"
	Public Const PromptCardDescVisa As String = "Visa"
	Public Const PromptCardDescAmericanExpress As String = "American Express"
	Public Const PromptCardDescSolo As String = "Solo"
	
	Public Const PromptSenderIDXelector As String = "XEL"
	Public Const PromptSenderIDFTYM As String = "FTYM"
	Public Const PromptSenderIDITS4ME As String = "ITS4ME"
	
	'21/2/01 CJB Changed from "Motor" to "PC" (Private Car) - Prompt's request to send ABI code
	Public Const PromptCoverTypeMotor As String = "PC"
	
	'Status Codes that Prompt may return
	Public Const PromptSuccessfulCompletion As String = "00"
	Public Const PromptNoRateHeld As String = "01"
	Public Const PromptFeesDisagree As String = "02"
	Public Const PromptRatesDisagree As String = "03"
	Public Const PromptInstalmentsDisagree As String = "04"
	Public Const PromptAmountsDisagree As String = "05"
	Public Const PromptInvalidAccount As String = "06"
	Public Const PromptUnknownBusinessType As String = "13"
	Public Const PromptOracleApplicationError As String = "21"
	Public Const PromptInvalidSortCode As String = "22"
	
	'Status Codes that we may return from the Prompt processing
	Public Const PromptDisabled As String = "50"
	Public Const PromptDisabledMessage As String = "Premium Finance has been bypassed since the PromptInterfaceURL setting in the registry is turned off."
	
	'Prompt Error Return Codes
	Public Const PromptBankAccountError As String = "10500"
	Public Const PromptOtherError As String = "10501"
	Public Const PromptSortCodeError As String = "10502"
End Module