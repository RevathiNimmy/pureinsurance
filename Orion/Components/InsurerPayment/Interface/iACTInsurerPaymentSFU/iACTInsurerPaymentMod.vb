Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Xml
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 02/09/1999
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' RAW 09/01/2003 : catchup : added code from old sourcesafe
	' ***************************************************************** '
	
	
	' Main public constant for all functions to identify which application this is.
	Public Const ACApp As String = "iACTInsurerPayment"
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public interface constants used when retrieving data from the resource file.
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACViewByCaption As Integer = 123
	Public Const ACTransactionCurrencyCaption As Integer = 124
	Public Const ACAccountCurrencyCaption As Integer = 125
	
    ' Base resource ids for listview captions
	Public Const ACTransactionListBase As Integer = 400
	Public Const ACEntryListBase As Integer = 500
    Public Const ACInstalmentEntryListBase = 600
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	Public Const ACPayButton As Integer = 204
	Public Const ACMarkButton As Integer = 205
	Public Const ACDrillButton As Integer = 206
	Public Const ACReportButton As Integer = 207
	Public Const ACAllocateButton As Integer = 208
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACClearDetailsTitle As Integer = 304
	Public Const ACClearDetails As Integer = 305
	Public Const ACStatusSearching As Integer = 306
	Public Const ACStatusFound As Integer = 307
	Public Const ACNoMarked As Integer = 308
	

	Public Const ACInsurerPaymentJournal As String = "JN"
	Public Const ACInsurerPaymentJournalEmpty As String = ""

	' Menus
	
	' Icons
	Public Const ACIconCheck As String = "check"
	Public Const ACIconPart As String = "part"
	Public Const ACIconBlank As String = "blank"
	
	Public Const ACIconComment As String = "Comment"
	Public Const ACIconNoComment As String = "NoComment"
	Public Const ACTransactionComment As String = "Transaction Comment -"
	Public Const ACTRANSENTRY As String = "TRANSENTRY"
	Public Const ACTRANSACTION As String = "TRANSACTION"
	Public Const ACForTransaction As String = "for Transaction -"
    Public Const ACTKeyNameUnallocatedAmountForPost As String = "Unallocated_Amount_For_Post"
    Public Const ACTKeyNameIsUnallocatedAmountForPost As String = "Is_Unallocated_Amount_For_Post"
	' Search Array Columns
	Public Enum SearchArrayEnum
        ACSADocumentID
        ACSADocumentRef
        ACSAInsuranceRef
        ACSAFullyPaidAmount
        ACSAClientOSAmount
        ACSAIsConsolidatedBinder
        ACSAHolderCode
        ACSAHolderName
        ACSADetailId
        ACSACompanyID
        ACSAAccountingDate
        ACSACurrencyAmount
        ACSACurrencyID
        ACSACurrencyCode
        ACSACurrencyRate
        ACSAMarkedAmount
        ACSAPaidAmount
        ACSASpare
        ACSAPeriodName
        ACSAMonth
        ACSAAccountCurrencyID
        ACSAAccountCurrencyCode
        ACSAAccountCurrencyRate
        ACSAFullyPaidAccountAmount
        ACSAClientOSAccountAmount
        ACSACurrencyAccountAmount
        ACSAMarkedAccountAmount
        ACSAPaidAccountAmount
        ACSAAlternateRef
        ACSAEffectiveDate
        ACSAComment
        ACSAnewDueDate
        ' End - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc) - (4.2.2)
        ACSAClientAccountCurrencyID
        ACSAYearName
        ACSAdueDate
        ACSAInstalmentNumber
        ACSAInstDueDate
        ACSAInstAmount
        ACSAInstPremiumFinaceCnt
        ACSAInstPremiumFinaceVersion
        ACSAMediaType
        ACSAInstPfInstalmentsId
        ACSAInstAccountAmount 'PN : 68972
        ACSAInstCurrencyAmount
        ACSASystemCurrencyID
        ACSASystemCurrencyRate
        ACSASystemCurrencyCode
        kSAIsDebitOrderTransDetail
	End Enum
	
	Public Enum ListViewTransactionEnum
        ACLTIconColumn
        ACLTHolderName
        ACLTInsuranceRef
        ACLTDocumentRef
        ACLTAlternateRef
        ACLTEffectiveDate
        ACLTAccountingDate
        ACLTDueDate
        ACLTCurrencyTotal
        ACLTPaidTotal
        ACLTNetTotal
        ACLTMarkedTotal
        ACLTClientOS
        ACLTHolderCode
        ACLTAllocationPeriod
        ACLTMediaType
    End Enum
	
	Public Enum ListViewEntryEnum
        ACLEIconColumn
        ACLECompany
		ACLEPeriod
		ACLEDocumentRef
		ACLECurrencyAmount
		ACLEPaidAmount
		ACLENetAmount
		ACLEMarkedAmount
		ACLESpare
		ACLEAltRef
        ACLEAllocationPeriod
	End Enum
    Public Enum ListViewEntryInstEnum
        ACLIIconColumn
        ACLIInstalmentNumber
        ACLIDueDate
        ACLIDocumentRef
        ACLICurrencyAmount
        ACLIPaidAmount
        ACLINetAmount
        ACLIMarkedAmount
        ACLISpare
        ACLIAltRef
    End Enum
	
	' Constants for the search data array indexes.
	Public Const ACClientCode As Integer = 0
	Public Const ACPolicyNumber As Integer = 1
	Public Const ACTransRef As Integer = 2
	Public Const ACGrossTransId As Integer = 3
	Public Const ACGrossAmt As Integer = 4
	Public Const ACCommTransId As Integer = 5
	Public Const ACCommAmt As Integer = 6
	Public Const ACCommAdjTransId As Integer = 7
	Public Const ACCommAdjAmt As Integer = 8
	Public Const ACAmtSettled As Integer = 9
	Public Const ACDocumentID As Integer = 10
	Public Const ACAccountingDate As Integer = 11
	Public Const ACCurrencyID As Integer = 12
	Public Const ACMarkedStatus As Integer = 13
	Public Const ACMarkedDate As Integer = 14
	Public Const ACTransType As Integer = 15
	Public Const ACPayAmt As Integer = 16
	Public Const ACSourceID As Integer = 17
	Public Const ACClientTransId As Integer = 19
	Public Const ACClientAmt As Integer = 20
	Public Const ACClientSettled As Integer = 21
	Public Const ACPeriodName As Integer = 22
	Public Const ACByInstalments As Integer = 23
	Public Const ACTaxAmnt As Integer = 24
	Public Const ACTaxTransId As Integer = 25
	Public Const ACMediaRef As Integer = 26
    Public Const ACAllocationPeriod As Integer = 27
	' Batch code
	Public Const ACBatchCode As String = "ACTINSPAY"
	' Process code
	Public Const ACProcessCode As String = "ACTIPAY"
	
	Public Const ACInsurerLedger As String = "Insurer"
	Public Const ACAgentLedger As String = "Agent"
	Public Const ACCommissionLedger As String = "Commission"
	Public Const ACOtherPartyLedger As String = "Other Party R''able"
    Public Const ACSubAgentLedger As String = "Sub Agent"
    Public Const ACClient = "Client"
	' Public contants used for the start and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the maxiumum search details.
	Public Const ACMaxSearchDetails As Integer = 500
	
	' Constant for the miniumum search length.
	Public Const ACMinSearchLength As Integer = 3
	
	'DC210801 Binder Indicator Options
	Public Const ACBIAllOutstanding As Integer = 0
	Public Const ACBIPaidByClient As Integer = 1
	
	' Public source and language ID's from the Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	Public g_iUserID As Integer
	'
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	' Public instance of the business object.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As bACTInsurerPaymentSFU.Business
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oPaymentGroups As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oCurrencyConvert As Object
	
	' Public instance of account business object.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oAccount As bACTAccount.Form
	
	'WR08
	Public Const kSysOptPaymentRefCheck As Integer = 5040
    Public Const kSysOptSingleCashReceipt As Integer = 5087

	' Start - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc) - (4.4)
	Public Const ACNoSelectionTitle As Integer = 713
	Public Const ACNoSelectionDetails As Integer = 714
	' End - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc) - (4.4)
    Public Const ACInstalmentsID As Short = 0
    Public Const ACInstalmentsTransPartPayAmount As Short = 17
    'Start - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.6.1)
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oDOMRootForInterfaceDisplay As XmlDocument
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sUserConfigXMLDataset As String = ""
	'End - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.6.1)
	
	' Start - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc) - (4.2.4.1.10)
	Public Function DisplayMessage(ByRef r_lTitleId As Integer, ByRef r_lMessageId As Integer, ByRef r_lOptions As Integer, ParamArray ByVal r_vTokens() As Object) As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "DisplayMessage"
		
		Dim sTitle, sMessage As String
		
		Try
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		'Get the title from the res file

        'developer guide no.243
        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=r_lTitleId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
		
		'Get the message from the res file

        'developer guide no.243
        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=r_lMessageId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
		
		'Replace Tokens in the title
		ReplaceTokens(sTitle, New Object(){r_vTokens})
		
		'Replace Tokens in the message
		ReplaceTokens(sMessage, New Object(){r_vTokens})
		
		'Now display the message to the user
		result = Interaction.MsgBox(sMessage, r_lOptions, sTitle)
		
		'----------------------------------------------------------------------------------------
		'Only for Debugging, the code will never execute this line
		'----------------------------------------------------------------------------------------
		  
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		' If you want to rollback a transaction or something, do it here
		
		Finally
		
		
		End Try
		Return result
	End Function
	' End - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc) - (4.2.4.1.10)
	
	' Start - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc) - (4.2.4.1.10)
	Public Function ReplaceTokens(ByRef r_sMessage As String, ParamArray ByVal r_vTokens() As Object) As Boolean
		
		Dim result As Boolean = False
		Const kMethodName As String = "ReplaceTokens"
		
		Dim lUpper As Integer
		Dim vToken, vValue As Object
		
		
		
		'This routine could be called directly like...
		'    ReplaceTokens sMessage, "Usename", m sUserName
		'With the params explicitly listed
		
		'OR by a routine that itself accepts a ParamArray.
		'    ReplaceTokens sMessage, r_vParams
		
		'We need to ensure that we find the 'root' ParamArray as the second
		'calling method would pass Variant(0)(0), Variant(0)(1) into this routine
		'and we need Variant(0), Variant(1)
		

		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' Find the 'root' paramarray
			'(i.e. convert  Variant(0)(0) to Variant(0))
			Dim vParams As Object = r_vTokens

            While vParams(0).GetType().Name = "Variant()"
                If Information.Err().Number <> 0 Then
                    'No params passed at all
                    Information.Err().Clear()
                    Return result
                End If


                vParams = vParams(0)

                If vParams.GetUpperBound(0) = -1 Then ' no params
                    Return result
                End If
            End While
			


			
			'Any params actually passed?
			'We could just have a blank paramarray
			lUpper = r_vTokens.GetUpperBound(0)
			If lUpper <> -1 Then
				'Loop through the param array
				For iItem As Integer = 0 To lUpper \ 2
					'Get the token and its replacement value


                    vToken = vParams(iItem * 2)
					'This will bomb if developer has passed an odd number of params


                    vValue = vParams(iItem * 2 + 1)
					
					'Replace the token with the value


					r_sMessage = r_sMessage.Replace("|" & CStr(vToken) & "|", CStr(vValue))
				Next 
			End If
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			GoTo Finally_Renamed
			'----------------------------------------------------------------------------------------
			'Only for Debugging, the code will never execute this line
			'----------------------------------------------------------------------------------------


			
Catch_Renamed: 
			' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse)
			' If you want to rollback a transaction or something, do it here
			
Finally_Renamed: 
			Return result
		
		Catch exc As System.Exception

        End Try
		
	End Function
	' End - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc) - (4.2.4.1.10)
End Module
