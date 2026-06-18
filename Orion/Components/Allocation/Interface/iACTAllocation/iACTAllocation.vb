Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 11th July 1997
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' RAW 12/03/2003 : ISS2893 : add new functions to display message
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iACTAllocation"
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	' Form Constants for Captions
	
	Public Const ACDetailsCaption As Integer = 100
	Public Const ACMainTabTitle0 As Integer = 101
	
	Public Const ACListCaption As Integer = 150
	Public Const ACListTabTitle0 As Integer = 151
	
	Public Const ACWriteOffCaption As Integer = 102
	Public Const ACBaseCaption As Integer = 103
	Public Const ACCCYCaption As Integer = 104
	Public Const ACNewOSAmountCaption As Integer = 105
	Public Const ACAllocAmountCaption As Integer = 106
	Public Const ACOSAmountCaption As Integer = 107
	Public Const ACOrigAmountCaption As Integer = 108
	Public Const ACCommentCaption As Integer = 109
	Public Const ACEffectiveXrateCaption As Integer = 110
	Public Const ACOrigXrateCaption As Integer = 111
	Public Const ACDocumentRefCaption As Integer = 112
	Public Const ACOriginalCurrencyCaption As Integer = 113
	Public Const ACDocumentTypeCaption As Integer = 114
	Public Const ACAccountingDateCaption As Integer = 115
	Public Const ACStatusCaption As Integer = 116
	Public Const ACAllocBalanceCaption As Integer = 117
	Public Const ACWriteOffReasonCaption As Integer = 118
	
	' Form
	
	' Details Form
	Public Const ACDetailsTabTitle1 As Integer = 120
	
	' Button/label Constants for Captions
	
	Public Const ACCommitCaption As Integer = 200
	Public Const ACNavigateCaption As Integer = 201
	Public Const ACHelpCaption As Integer = 202
	Public Const ACCancelCaption As Integer = 203
	Public Const ACOKCaption As Integer = 204
	Public Const ACFindCaption As Integer = 205
	Public Const ACAddCaption As Integer = 206
	Public Const ACRemoveCaption As Integer = 207
	Public Const ACEditCaption As Integer = 208
	Public Const ACAllocateAllCaption As Integer = 209
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	Public Const ACShortNameErrorTitle As Integer = 304
	Public Const ACShortNameError As Integer = 305
	Public Const ACImbalanceTitle As Integer = 306
	Public Const ACImbalanceMessage As Integer = 307
	Public Const ACWriteOffMessage As Integer = 308
	Public Const ACWriteOffNotAllowed As Integer = 309
	Public Const ACExcessiveAllocation As Integer = 310
	Public Const ACWriteOffTitle As Integer = 311 ' RAW 12/03/2003 : ISS2893 : added
	
	' Transaction ColumnHeaders
	Public Const ACTransColumnHeads As Integer = 160
	Public Const ACTransColumnHeadCount As Integer = 14
	
	' Menus
	
	' Constants for the List data array subscripts.
	
	Public Const ACSubAllocationDetailID As Integer = 0
	Public Const ACSubCashlistitemID As Integer = 1
	Public Const ACSubAllocationID As Integer = 2
	Public Const ACSubOriginalCurrency As Integer = 3
	Public Const ACSubTransdetailID As Integer = 4
	Public Const ACSubDocumenttypeID As Integer = 5
	Public Const ACSubAccountingDate As Integer = 6
	Public Const ACSubOriginalDate As Integer = 7
	Public Const ACSubAllocateToBase As Integer = 8
	Public Const ACSubOrigBaseAmount As Integer = 9
	Public Const ACSubOrigCcyAmount As Integer = 10
	Public Const ACSubOrigXrate As Integer = 11
	Public Const ACSubEffectiveXrate As Integer = 12
	Public Const ACSubOsBaseAmount As Integer = 13
	Public Const ACSubOsCcyAmount As Integer = 14
	Public Const ACSubAllocBaseAmount As Integer = 15
	Public Const ACSubAllocCcyAmount As Integer = 16
	Public Const ACSubFullyMatched As Integer = 17
	Public Const ACSubWriteOff As Integer = 18
	Public Const ACSubNewOsCcyAmount As Integer = 19
	Public Const ACSubNewOsBaseAmount As Integer = 20
	Public Const ACSubLossGainAmount As Integer = 21
	Public Const ACSubIsPrimary As Integer = 22
	Public Const ACSubDocumentRef As Integer = 23
	Public Const ACSubAllocBaseAmountUnrounded As Integer = 24
	Public Const ACSubWriteOffReasonID As Integer = 25
	Public Const ACSubWriteOffCode As Integer = 26
	Public Const ACSubWriteOffReason As Integer = 27
	Public Const ACSubWriteOffAmount As Integer = 28
	'EK 100100 New Contstants for extra Euro Fields
	Public Const ACSubCCyAmountUnRounded As Integer = 29
	Public Const ACSubBaseAmountUnRounded As Integer = 30
	Public Const ACSubEuro As Integer = 31
	Public Const ACSubEuroAmount As Integer = 32
	Public Const ACSubEuroBaseXrate As Integer = 33
	Public Const ACSubEuroCCyXrate As Integer = 34
	'eck040601
	Public Const ACSubOriginalOSBaseAmount As Integer = 35
	Public Const ACSubOriginalOSCCyAmount As Integer = 36
	
	' Columns in the Transaction ListView
	Public Const ACAccountingDateColumn As Integer = 1
	Public Const ACDocumentTypeColumn As Integer = 2
	Public Const ACDocumentRefColumn As Integer = 3
	Public Const ACOSCCYColumn As Integer = 4
	Public Const ACOSBaseColumn As Integer = 5
	Public Const ACAllocCCYColumn As Integer = 6
	Public Const ACAllocBaseColumn As Integer = 7
	Public Const ACNewOSCCYColumn As Integer = 8
	Public Const ACNewOSBaseColumn As Integer = 9
	Public Const ACWriteOffColumn As Integer = 10
	Public Const ACWriteOffReasonColumn As Integer = 11
	Public Const ACOriginalCurrencyColumn As Integer = 12
	Public Const ACOrigXrateColumn As Integer = 13
	Public Const ACEffectiveXrateColumn As Integer = 14
	Public Const ACCommentColumn As Integer = 15
	Public Const ACOrigCCYColumn As Integer = 16
	Public Const ACOrigBaseColumn As Integer = 17
	
	'end of rejiggery
	
	
	Public Const ACIAccountingDate As Integer = 0
	Public Const ACIDocumentTypeId As Integer = 1
	Public Const ACIDocumentRef As Integer = 2
	'Public Const ACIPeriodName = 2
	Public Const ACICurrency As Integer = 3
	Public Const ACICurrencyAmount As Integer = 4
	Public Const ACIBaseAmount As Integer = 5
	'Public Const ACIDocTypeGroupId = 5
	Public Const ACIProject As Integer = 6
	Public Const ACIContract As Integer = 7
	Public Const ACIProduct As Integer = 8
	Public Const ACIDepartment As Integer = 9
	Public Const ACIAgent As Integer = 10
	Public Const ACIClient As Integer = 11
	Public Const ACIAccountShortCode As Integer = 12
	Public Const ACIAccountId As Integer = 13
	Public Const ACICurrencyId As Integer = 14
	Public Const ACITransDetailId As Integer = 15
	
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
	' Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	
	Public g_sUsername As String = ""
	Public g_sPassword As String = ""
	Public g_iCurrencyID As Integer
	Public g_iLogLevel As Integer
	
	' Company ID
	Public g_iCompanyID As Integer
	
	' User ID
	Public g_iUserID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	'Account object
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oAccount As bACTAccount.Form
	
	'Transaction Lock object
    'developer guide no. 107
    <ThreadStatic()> _
 Private m_oPMLock As bPMLock.User
	
    ' Declare an instance of the Write off object
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oWriteOffReason As Object
	
    'eck040601
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_bFirst As Boolean
	
	'Product Family Name for Help
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFOrion
	
	Public Const ScreenHelpID1 As Integer = 30000
	Public Const ScreenHelpID2 As Integer = 17000
	
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	'Constants for ledgertype.description
	Public Const ACCOUNT_LEDGER_CLIENT As String = "Client"
	Public Const ACCOUNT_LEDGER_SUBAGENT As String = "Sub Agent"
	Public Const ACCOUNT_LEDGER_PURCHASE As String = "Purchase"
	Public Const ACCOUNT_LEDGER_AGENT As String = "Agent"
    Public Const ACCOUNT_LEDGER_INSURER As String = "Insurer"
    Public Const kSysOptSingleCashReceipt = 5087
	
	

	Public Sub Main()
		
	End Sub
	
	
	' RAW 12/03/2003 : ISS2893 : added
	Public Function DisplayMessage(ByVal v_lTitleId As Integer, ByVal v_lMessageId As Integer, ByVal v_lOptions As Integer, ByVal v_vTokens As Object) As Integer
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: DisplayMessage
		' PURPOSE: Displays a message based on passed resource file Ids
		' AUTHOR: Sirius Financial Systems Plc
		' DATE: 09 October 2002, 16:03:54
		' RETURNS: PMTrue for success
		' CHANGES: PWC 16/10/2002 - Added param array to enable substition of tokens
		' ---------------------------------------------------------------------------
		
		Dim result As Integer = 0
		Dim sTitle, sMessage As String
		
		
		Try
		
		'Get the title from the res file

		sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=v_lTitleId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
		
		'Get the message from the res file

		sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=v_lMessageId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
		
		'Replace Tokens in the message
		ReplaceTokens(sMessage, v_vTokens)
		
		'Now display the message to the user
		result = Interaction.MsgBox(sMessage, v_lOptions, sTitle)
		
		
		'----------------------------------------------------------------------------------------
		'Only for Debugging, the code will never execute this line
		'----------------------------------------------------------------------------------------
	
		Catch ex As Exception
		Select Case Information.Err().Number
			Case Else
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayMessage", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
				
				result = gPMConstants.PMEReturnCode.PMError
				
		End Select
		
		Finally
	
		
		End Try
		Return result
	End Function
	
	' RAW 12/03/2003 : ISS2893 : added
	Private Function ReplaceTokens(ByRef r_sMessage As String, ParamArray ByVal r_vTokens() As Object) As Boolean
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: ReplaceTokens
		' PURPOSE: Replace place holders with values
		'          e.g. convert |Username| to "Fred Bloggs"
		' AUTHOR: Paul Cunnigham
		' DATE: 21 October 2002, 10:04:52
		' RETURNS: True for success
		' CHANGES:
		' ---------------------------------------------------------------------------
		
		Dim result As Boolean = False
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
			

			
			'----------------------------------------------------------------------------------------
			'Only for Debugging, the code will never execute this line
			'----------------------------------------------------------------------------------------


			
			Return result
		
		
	End Function
	
	
	' ***************************************************************** '
	'
	' Name: LockTransdetailId
	'
	' Description:
	'
	' History: 13/05/2003 sj - Created.
	'
	' ***************************************************************** '
	Public Function LockTransdetailId(ByVal v_lTransDetailId As Integer, ByRef r_bAlreadyLocked As Boolean) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			Dim sCurrentlyLockedBy As String = ""
			
			'We need to lock this transdetail_id
			If m_oPMLock Is Nothing Then
				Dim temp_m_oPMLock As Object
				m_lReturn = g_oObjectManager.GetInstance(temp_m_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
				m_oPMLock = temp_m_oPMLock
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					result = gPMConstants.PMEReturnCode.PMFalse
					iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Failed to create instance of bPMLock.User", vApp:=ACApp, vClass:=ACClass, vMethod:="LockTransdetailId")
					Return result
				End If
			End If
			

			m_lReturn = m_oPMLock.LockKey(sKeyName:="ALLOCATION", vKeyValue:=v_lTransDetailId, iUserID:=g_iUserID, sCurrentlyLockedBy:=sCurrentlyLockedBy)
			If m_lReturn = gPMConstants.PMEReturnCode.PMError Then
				result = gPMConstants.PMEReturnCode.PMFalse
				iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="bPMLock.User.LockKey Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LockTransdetailId")
				Return result
			End If
			If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
				MessageBox.Show("One of the Transactions is already being allocated by " & sCurrentlyLockedBy, "Find Transaction", MessageBoxButtons.OK)
				r_bAlreadyLocked = True
			Else
				r_bAlreadyLocked = False
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LockTransdetailId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LockTransdetailId", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: UnLockTransdetailId
	'
	' Description:
	'
	' History: 13/05/2003 sj - Created.
	'
	' ***************************************************************** '
	Public Function UnLockTransdetailId(ByVal v_vLockedTransDetailIds As Object) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If Not Information.IsArray(v_vLockedTransDetailIds) Then
				Return result
			End If
			
			If g_oAccount Is Nothing Then
				Dim temp_g_oAccount As Object
				m_lReturn = g_oObjectManager.GetInstance(temp_g_oAccount, "bACTAccount.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
				g_oAccount = temp_g_oAccount
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					result = gPMConstants.PMEReturnCode.PMFalse
					iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Failed to create instance of bACTAccount.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="UnLockTransdetailId")
					Return result
				End If
			End If
			

			m_lReturn = g_oAccount.DeleteAllocationLocks(v_vOSTransactions:=v_vLockedTransDetailIds)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="bACTAccount.Form.DeleteAllocationLocks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnLockTransdetailId")
				Return result
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnLockTransdetailId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnLockTransdetailId", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ChkDocTypeIsInstalments
    '
    ' Description: PM011373
    '
    ' History: 01/06/2011 JP - Created.
    '
    ' ***************************************************************** '
    Public Function ChkDocTypeIsInstalments(ByVal v_sDocType As String) As Boolean

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            Select Case v_sDocType
                Case gACTLibrary.ACTAutoNumberRangeCodeIdr, _
                            gACTLibrary.ACTAutoNumberRangeCodeIcr, _
                            gACTLibrary.ACTAutoNumberRangeCodeIca, _
                            ACTAutoNumberRangeCodeIND, ACTAutoNumberRangeCodeINC, _
                            ACTAutoNumberRangeCodeIED, ACTAutoNumberRangeCodeIEC, _
                            ACTAutoNumberRangeCodeIRD, ACTAutoNumberRangeCodeIRC
                    result = True
            End Select

            Return result

        Catch ex As Exception


            ' This will cause calling app to fail safe
            result = True

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error determining status of Document Type", vApp:=ACApp, vClass:=ACClass, vMethod:="ChkDocTypeIsInstalments", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function
        End Try

    End Function

    Sub New()
        Main()
    End Sub
	Sub JustForInvokeMain()
    End Sub
    'Defect TFS 6444
    Public Function CheckIsLinkedToThirdPartyScheme(ByVal sDocumentRef As String) As Boolean
        Dim oBAllocation As Object = Nothing
        Dim result As Boolean = False

        Try
            m_lReturn = g_oObjectManager.GetInstance(oObject:=oBAllocation, sClassName:="bACTAllocation.Form", vInstanceManager:=PMGetViaClientManager)

            result = oBAllocation.IsLinkedToThirdPartyScheme(DocumentRef:=sDocumentRef)

        Catch excep As System.Exception

            ' This will cause calling app to fail safe
            result = True
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error determining Is Linked To Third Party Scheme", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckIsLinkedToThirdPartyScheme", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        Finally
            oBAllocation = Nothing
        End Try

        Return result

    End Function
End Module
