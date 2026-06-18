Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Public Module MainModule

    Public Const ACApp As String = "iPMUBatchRenewalJobs"

    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 101

    Public Const ACListTitle1 As Integer = 102
    Public Const ACListTitle2 As Integer = 103

    Public Const ACPostCode As Integer = 121
    Public Const ACAddressLine1 As Integer = 123
    Public Const ACAddressLine2 As Integer = 124

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

    Public Const ACClearDetailsTitle As Integer = 304
    Public Const ACClearDetails As Integer = 305
    Public Const ACDeleteDetails As Integer = 306
    Public Const ACDeleteDetailsTitle As Integer = 307
    Public Const ACSuspendButton As Integer = 308
    Public Const ACCloseButton As Integer = 309
    Public Const ACAgentCode As Integer = 310
    Public Const ACAgentName As Integer = 311
    Public Const ACPartyCnt As Integer = 312
    Public Const ACDeleteAgent As Integer = 313
    Public Const ACDuplicateTitle As Integer = 314
    Public Const ACDuplicateMsg As Integer = 315

    Public Const ACSuspendTitle As Integer = 323
    Public Const ACSuspendMsg As Integer = 324
    Public Const ACMultiSelect As Integer = 325
    Public Const ACMultiSelectDelete As Integer = 326
    Public Const ACMultiSelectCalculate As Integer = 327

    ' ListView
    Public Const ACBatchRenewalJobId As Integer = 0
    Public Const ACBatchRenewalCode As Integer = 1
    Public Const ACBatchRenewalDescription As Integer = 2
    Public Const ACBatchRenewalSAMServer As Integer = 3
    Public Const ACBatchRenewalDaysBeforeRenewalDate As Integer = 4
    Public Const ACBatchRenewalIsActive As Integer = 5
    Public Const ACBatchRenewalJobTypeId As Integer = 6
    Public Const ACBatchRenewalDocsDescription As Integer = 7
    Public Const ACBatchRenewalReportSortOrder As Integer = 8
    Public Const ACBatchRenewalAllAgents As Integer = 9
    Public Const ACBatchRenewalUserId As Integer = 10
    Public Const ACBatchRenewalDateCreated As Integer = 11
    Public Const ACBatchRenewalDateUpdated As Integer = 12
    Public Const ACBatchRenewalUserName As Integer = 13
    Public Const ACBatchRenewalJobDescription As Integer = 14
    Public Const ACBatchRenewalJobIncludeDirectPolicies As Integer = 15
    Public Const ACBatchRenewalJobRunExtendedRule As Integer = 16

    Public Const kBatchRenewalColHIndexJobId As Integer = 0
    Public Const kBatchRenewalColHIndexDateCreated As Integer = 1
    Public Const kBatchRenewalColHIndexJobCode As Integer = 2
    Public Const kBatchRenewalColHIndexDesription As Integer = 3
    Public Const kBatchRenewalColHIndexStatus As Integer = 4
    Public Const kBatchRenewalColHIndexJobType As Integer = 5
    Public Const kBatchRenewalColHIndexUser As Integer = 6

    'Agent List View
    Public Const ACArPartyCnt As Integer = 0
    Public Const ACArAgentCode As Integer = 1
    Public Const ACArAgentName As Integer = 2
    Public Const ACArAgAddressLine1 As Integer = 3
    Public Const ACArAgAddressLine2 As Integer = 4
    Public Const ACArAgPostCode As Integer = 5
    Public Const ACArAddressLine1 As Integer = 14
    Public Const ACArAddressLine2 As Integer = 15
    Public Const ACArPostCode As Integer = 18

    Public Const ACSearchJobId As Integer = 316
    Public Const ACSearchCreated As Integer = 317
    Public Const ACSearchJobCode As Integer = 318
    Public Const ACSearchDescription As Integer = 319
    Public Const ACSearchStatus As Integer = 320
    Public Const ACSearchJobType As Integer = 321
    Public Const ACSearchUser As Integer = 322

    Public Const ACArJobCode As Integer = 0
    Public Const ACTabGeneral As Integer = 0

    'Constants Batch Renewal Jobs
    Public Const ACPaymentCashListItemID As Integer = 0

    Public Const kPaymentMaintColHIndexClientCode As Integer = 0

    'Associated Sub Agent Details
    Public Const kAssociatedSubAgentCnt As Integer = 0
    Public Const kAssociatedSubAgentShortName As Integer = 1
    Public Const kAssociatedSubAgentDateCancelled As Integer = 2
    Public Const kAssociatedSubAgentName As Integer = 3
    Public Const kAssociatedSubAgentResolvedName As Integer = 4

    Public Const kSelection As Integer = 1
    Public Const kInvitation As Integer = 2
    Public Const kAcceptance As Integer = 3

    Public Const kRenewalWithNoException As Integer = 0
    Public Const kRenewalExceptionPOLNUM As Integer = 1
    Public Const kRenewalExceptionINSTAL As Integer = 2
    Public Const kRenewalExceptionPERPAY As Integer = 3
    Public Const kRenewalExceptionTEMPLATE As Integer = 4
    Public Const kRenewalExceptionOTHER As Integer = 5

    ' Cancel Payment of Event Type
    Public Const ACEventCode As String = "BATCHREN"

    ' Constant for the maxiumum search details.
    Public Const ACMaxSearchDetails As Integer = 250

    ' Constant for the miniumum search length.
    Public Const ACMinSearchLength As Integer = 3

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
 Public g_iCompanyID As Integer

    ' Username.
    Public g_sUsername As New FixedLengthString(12)
    'Developer Guide No. 107
    <ThreadStatic()> _
    Public g_iUserID As Integer

    ' Public instance of the object manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager

    ' Public instance of the business object.
    'Developer Guide No. 107
    <ThreadStatic()> _
    Public g_oBusiness As Object
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oPMUser As bPMUser.Business
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iCurrencyID As Integer
    'Product Family Name for Help
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusUnderwriting

    Public Const ScreenHelpID As Integer = 51000

    '**************************************************************************
    '
    ' Name    : IsInListView
    ' Desc    : return PMTrue if in list view
    ' History : 18/08/2000 Tinny (Created)
    '
    '**************************************************************************
    Public Function IsInListView(ByVal v_vKeyID As Integer, ByRef r_oListView As ListView) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'is v_vKeyID in list view
            For lCount As Integer = 1 To r_oListView.Items.Count

                'If r_oListView.ListItems(lCount&).Tag = v_vKeyID Then

                ' Ram 10-01-2001
                ' Added the cLng Conversion, since the v_vKeyID is a numeric,
                ' when compared with Tag property so it always mismatches
                If Convert.ToString(r_oListView.Items.Item(lCount - 1).Tag) = v_vKeyID Then
                    result = gPMConstants.PMEReturnCode.PMTrue
                    Exit For
                End If
            Next

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="IsInListView", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function PadLeft(ByVal v_vValue As String, ByVal iMaxChar As Integer) As String

        Return New String(" "c, iMaxChar - v_vValue.Length) & v_vValue.Trim()

    End Function

    Public Function PadRight(ByVal v_vValue As String, ByVal iMaxChar As Integer) As String

        Return v_vValue.Trim() & New String(" "c, iMaxChar - v_vValue.Length)

    End Function



    Public Sub Main()

    End Sub
    Sub New()
        Main()
    End Sub
    Sub JustForInvokeMain()
    End Sub
End Module