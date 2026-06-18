Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Imports Artinsoft.VB6.Gui

<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
 Public Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 02-May-2004
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '


    Public Const ACApp As String = "iPMURenewalProcess"
    Public Const ACDateColumn As Integer = 2
    ' and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Constants for the Renewals search data array indexes.
    Public Const ACIRenewalStatusCnt As Integer = 0
    Public Const ACIRenewalProduct As Integer = 1
    Public Const ACIRenewalInsuranceHolder As Integer = 2
    Public Const ACIRenewalShortname As Integer = 3
    Public Const ACIRenewalPartyType As Integer = 4
    Public Const ACIRenewalLiveInsuranceRef As Integer = 5
    Public Const ACIRenewalPolicyCnt As Integer = 6
    Public Const ACIRenewalInsuranceRef As Integer = 7
    Public Const ACIRenewalInsuranceFolder As Integer = 8
    Public Const ACIRenewalInsuranceStructID As Integer = 9
    Public Const ACIRenewalStatusTypeId As Integer = 10
    Public Const ACIRenewalStatusType As Integer = 11
    Public Const ACIRenewalCriticalDate As Integer = 12
    Public Const ACIRenewalLivePolicyCnt As Integer = 13
    Public Const ACIRenewalCoverStartDate As Integer = 14
    Public Const ACIRenewalExpiryDate As Integer = 15
    Public Const ACIRenewalAgentCnt As Integer = 16
    Public Const ACIRenewalProductId As Integer = 17
    Public Const ACIRenewalDate As Integer = 18
    Public Const ACIRenewalLeadAgentCode As Integer = 19
    Public Const ACIRenewalAccHandlerCode As Integer = 20
    Public Const ACIRenewalSourceCode As Integer = 21
    Public Const ACIRenewalClaimsIndicator As Integer = 22
    Public Const ACIRenewalSourceID As Integer = 23
    Public Const ACRenewalDeleteFromListView As Integer = 24 'set to 1 if this record is deleted from listview
    Public Const ACIRenewalIsBranchDeleted As Integer = 25
    Public Const ACIRenewalIsInTransferMode As Integer = 26
    Public Const ACIRenewalTransferToPartyCnt As Integer = 27
    Public Const ACIRenewalTransferToPartyShortName As Integer = 28
    Public Const ACIRenewalLivePolicyAgentCode As Integer = 29
    Public Const ACIRenewalIsTrueMonthlyPolicy As Integer = 30
    Public Const ACIRenewalAnniversaryCopy As Integer = 31
    Public Const ACIPaymentMethod As Integer = 32
    Public Const ACIRenewalResolvedName As Integer = 34
    Public Const ACIRenewalLeadAgentDescription As Integer = 35


    'Constants for Business Types
    Public Const ACBusinessTypeQuote As Integer = 1
    Public Const ACBusinessTypePolicy As Integer = 2
    Public Const ACBusinessTypeProvClaim As Integer = 3
    Public Const ACBusinessTypeFullClaim As Integer = 4

    'Constants for Lapse reasons search data array indexes.
    Public Const ACLapseReasonID As Integer = 0
    Public Const ACLapseReason As Integer = 1

    'Doc Generation modes.
    Public Const ACPrintMode As Integer = 2
    Public Const ACPrintSilentMode As Integer = 3
    Public Const ACSpoolSilentMode As Integer = 4

    Public Const ACDOCTypeDebitNote As Integer = 3
    Public Const ACDocTypeSchedule As Integer = 4
    Public Const ACDocTypeCertificate As Integer = 5
    Public Const ACDocTypeLapse As Integer = 8
    Public Const ACDocTypeRenewalDebitNote As Integer = 14
    Public Const ACDocTypeNoticePrint As Integer = 6


    Public Const ACRenModeStandard As Integer = 0
    Public Const ACRenModeRI As Integer = 1 'renewal invites
    Public Const ACRenModeRAA As Integer = 2 'renewal acceptance with amendment
    Public Const ACRenModeRA As Integer = 3 'renewal acceptance without amendment

    ' Public instance of the object manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
    Public g_oObjectManager As bObjectManager.ObjectManager
    'Developer Guide No. 107
    <ThreadStatic()> _
    Public g_oBusiness As Object

    ' Public source and language ID's from the
    ' Object Manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
    Public g_iSourceID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
    Public g_iLanguageID As Integer

    'Start (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx) - (6.1.1.1)
    'Adding constats to represent the policy make live status
    Public Enum EPolicyMakeLiveStatus
        PolicyQuoted = 0
        PolicyMadeLive = 1
    End Enum
    'End (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx) - (6.1.1.1)


    '*********************************************************************************************
    ' return PMTrue if one of the row is ticked in this listview
    ' return PMFalse if none are ticked and PMError if an error occurred
    'if r_lNumberTick passed in then return how many are ticked
    '*********************************************************************************************
    Public Function ListViewIsTick(ByVal v_oListView As ListView, Optional ByRef r_lNumberTick As Integer = 0) As Integer

        Dim result As Integer = 0

        Try

        result = gPMConstants.PMEReturnCode.PMFalse

        If v_oListView.Items.Count = 0 Then
		Return result
        End If

            If Not False Then
                r_lNumberTick = v_oListView.CheckedItems.Count
                If r_lNumberTick > 0 Then
                    result = gPMConstants.PMEReturnCode.PMTrue
                End If
            Else
                For lCount As Integer = 1 To v_oListView.Items.Count
                    If Not IsNothing(v_oListView.FocusedItem) Then
                        If v_oListView.Items.Item(lCount - 1).Checked Then
                            result = gPMConstants.PMEReturnCode.PMTrue
                            Exit For
                        End If
                    End If
                Next lCount
            End If


        Catch ex As Exception

        result = gPMConstants.PMEReturnCode.PMError

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check if an item in listview is ticked", vApp:=ACApp, vClass:=ACClass, vMethod:="ListViewIsTick()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

        End Try
	Return result	
    End Function

    '***********************************************************************
    ' Name : UpdateListView
    '
    ' Desc : update column on listview with new value
    '        v_vColumnIndex(0,0) = position
    '        v_vColumnIndex(0,1) = value
    '        v_vColumnIndex(0,2) = tag -- optional
    '***********************************************************************
    Public Function UpdateListView(ByVal v_oListView As ListView, ByVal v_vColumnIndex(,) As Object, Optional ByVal v_lSelectedIndex As Integer = -1, Optional ByVal v_lIcon As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lIcon As Integer

        Try

        result = gPMConstants.PMEReturnCode.PMTrue

        If Information.IsArray(v_vColumnIndex) Then
            For lCount As Integer = 0 To v_vColumnIndex.GetUpperBound(0)
                'get selected row
                If v_lSelectedIndex = -1 Or v_lSelectedIndex = 0 Then
                    v_lSelectedIndex = v_oListView.FocusedItem.Index + 1
                End If

                'apply changes to column

                If CDbl(v_vColumnIndex(lCount, 0)) = 0 Then

                    v_oListView.Items.Item(v_lSelectedIndex - 1).Text = CStr(v_vColumnIndex(lCount, 1))
                Else


                    v_oListView.Items.Item(v_lSelectedIndex - 1).SubItems.Item(CInt(v_vColumnIndex(lCount, 0))).Text = CStr(v_vColumnIndex(lCount, 1))

                    'update tag value
                    If v_vColumnIndex.GetUpperBound(1) = 2 Then



                        v_oListView.Items.Item(v_lSelectedIndex - 1).SubItems.Item(CInt(v_vColumnIndex(lCount, 0)) - 1).Tag = CStr(v_vColumnIndex(lCount, 2))
                    End If
                End If
            Next

            If v_lIcon <> 0 Then

                'Developer Guide No. 49
                v_oListView.Items(v_lSelectedIndex - 1).ImageIndex = v_lIcon - 1
            End If
        End If


        Catch ex As Exception

        result = gPMConstants.PMEReturnCode.PMError

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update listview with new value", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateListView()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

        End Try
	Return result	
    End Function
End Module
