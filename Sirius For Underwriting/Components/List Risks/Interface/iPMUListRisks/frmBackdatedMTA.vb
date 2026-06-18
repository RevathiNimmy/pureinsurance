Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmBackdatedMTA
    Inherits System.Windows.Forms.Form

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmBackdatedMTA"
    Public Const ACApplyMTATaxesOnOOSRenewalySystemOptionNumber As Integer = 5262

    Private m_lInsuranceFileCnt As Integer

    Private m_oAutoMTA As bSIRAutoMTA.Business
    Private m_cTotalPremium As Decimal
    Private m_crTotalCommission As Decimal
    Private m_crTotalFees As Decimal
    Private m_vBackdatedMTAVersions(,) As Object
    Private m_lItemsFound As Integer

    'WPR 33-75 added
    Private m_sTransactionType As String
    Private m_lProductID As Integer
    Private m_lInsuranceFolderCnt As Integer
    Private m_lPartyCnt As Integer
    Private m_sShortName As String
    Private m_oBusiness As Object
    Private m_frmParent As frmInterface
    Private m_iSourceID As Integer

    'Business objects
    Private m_oRenSelection As Object
    Private m_oRiskData As Object
    Private m_oFindRisk As Object
    Private m_oRisk As Object
    Private m_oChangePolicyStatus As Object

    Private m_bAllQuoted As Boolean
    'WPR 33-75 Ends

    Public WriteOnly Property BackdatedMTAVersions() As Object
        Set(ByVal vBackdatedMTAVersions As Object)
            m_vBackdatedMTAVersions = vBackdatedMTAVersions
        End Set
    End Property

    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property


    Public Property TotalPremium() As Decimal
        Get
            Return m_cTotalPremium
        End Get
        Set(ByVal Value As Decimal)
            m_cTotalPremium = Value
        End Set
    End Property

    Public WriteOnly Property oAutoMTA() As bSIRAutoMTA.Business
        Set(ByVal Value As bSIRAutoMTA.Business)
            m_oAutoMTA = Value
        End Set
    End Property

    Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
        If Not m_bAllQuoted Then
            If MsgBox("Not all Risks have been quoted. You cannot make this MTA live until all Risks are quoted." & vbCrLf & vbCrLf &
                "Are you sure you want to close this window?", vbExclamation + vbYesNo + vbDefaultButton2, "Risk Unquoted") = vbNo Then
                Exit Sub
            End If
        End If
        Me.Close()
    End Sub
    'WPR 33-75 added
    Public ReadOnly Property AllQuoted() As Boolean
        Get
            Return m_bAllQuoted
        End Get
    End Property
    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property
    Public WriteOnly Property ProductID() As Integer
        Set(ByVal Value As Integer)
            m_lProductID = Value
        End Set
    End Property

    Public WriteOnly Property PartyCnt() As Integer
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property

    Public WriteOnly Property SourceID() As Integer
        Set(ByVal Value As Integer)
            m_iSourceID = Value
        End Set
    End Property
    Public WriteOnly Property Shortname() As String
        Set(ByVal Value As String)
            m_sShortName = Value
        End Set
    End Property

    ' Public WriteOnly Property Business() As bSIRFindRisk.Form
    Public WriteOnly Property Business() As Object
        Set(ByVal Value As Object)
            m_oBusiness = Value
        End Set
    End Property
    Public WriteOnly Property ParentForm() As Form
        Set(ByVal Value As Form)
            m_frmParent = Value
        End Set
    End Property


    Public WriteOnly Property InsuranceFolderCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFolderCnt = Value
        End Set
    End Property

    Private Sub frmBackdatedMTA_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
        Const kMethodName As String = "Form_Terminate"

        Dim lReturn As Integer

        If Not m_oChangePolicyStatus Is Nothing Then
            m_oChangePolicyStatus.Dispose()
            m_oChangePolicyStatus = Nothing
        End If
    End Sub
    'WPR 33-75 ends
    ''' <summary>
    ''' frmBackdatedMTA_Load
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub frmBackdatedMTA_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        Const kMethodName As String = "Form_Load"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lSubValue As Integer

        Try
            If lvwBackdatePolicies.SelectedItems.Count = 0 Then
                cmdEdit.Enabled = False
            End If
            lvwBackdatePolicies.Columns.Insert(0, "", "Policy Type", CInt(VB6.TwipsToPixelsX(700)), HorizontalAlignment.Left, -1)
            lvwBackdatePolicies.Columns.Insert(1, "", "Cover Start Date", CInt(VB6.TwipsToPixelsX(2100)), HorizontalAlignment.Left, -1)
            lvwBackdatePolicies.Columns.Insert(2, "", "Cover End Date", CInt(VB6.TwipsToPixelsX(1300)), HorizontalAlignment.Center, -1)
            lvwBackdatePolicies.Columns.Insert(3, "", "Status", CInt(VB6.TwipsToPixelsX(1300)), HorizontalAlignment.Center, -1)
            lvwBackdatePolicies.Columns.Insert(4, "", "Original Premium", CInt(VB6.TwipsToPixelsX(1200)), HorizontalAlignment.Right, -1)
            lvwBackdatePolicies.Columns.Insert(5, "", "Endorsement Premium", CInt(VB6.TwipsToPixelsX(1600)), HorizontalAlignment.Right, -1)
            lvwBackdatePolicies.Columns.Insert(6, "", "Original Commission", CInt(VB6.TwipsToPixelsX(1500)), HorizontalAlignment.Right, -1)
            lvwBackdatePolicies.Columns.Insert(7, "", "Endorsement Commission", CInt(VB6.TwipsToPixelsX(1500)), HorizontalAlignment.Right, -1)
            lvwBackdatePolicies.Columns.Insert(8, "", "Original Fees", CInt(VB6.TwipsToPixelsX(1500)), HorizontalAlignment.Right, -1)
            lvwBackdatePolicies.Columns.Insert(9, "", "Endorsement Fees", CInt(VB6.TwipsToPixelsX(1500)), HorizontalAlignment.Right, -1)

            'Hidden columns
            lvwBackdatePolicies.Columns.Insert(10, "", "Reapplied_insurance_file_cnt", CInt(VB6.TwipsToPixelsX(0)), HorizontalAlignment.Left, -1)
            lvwBackdatePolicies.Columns.Insert(11, "", "Reversed_insurance_file_cnt", CInt(VB6.TwipsToPixelsX(0)), HorizontalAlignment.Left, -1)

            ''lReturn = SetExtraListViewProperties(v_hWndList:=lvwBackdatePolicies.hwnd, _
            ''                             v_vShowRowSelect:=True)

            If m_sTransactionType = "MTC" Or m_sTransactionType = "MTR" Then
                cmdEdit.Visible = False
                cmdView.Visible = False
            End If

            RefreshForm(False)

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
    End Sub

    Private Sub Form_Terminate_Renamed()

        oAutoMTA = Nothing
    End Sub

    ''' <summary>
    ''' GetBackdatedPolicyVersions
    ''' </summary>
    ''' <param name="v_bReload"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetBackdatedPolicyVersions(ByVal v_bReload As Boolean) As Integer

        Dim nResult As Integer = 0
        Const kMethodName As String = "GetBackdatedPolicyVersions"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oListItem As ListViewItem
        Dim sInsuranceFileCnt As String = ""

        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue
            m_bAllQuoted = True
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            sInsuranceFileCnt = "Key" & m_lInsuranceFileCnt.ToString()
            If Not IsArray(m_vBackdatedMTAVersions) OrElse v_bReload Then
                lReturn = m_oAutoMTA.GetBackdatedPolicyVersions(m_lInsuranceFileCnt, m_vBackdatedMTAVersions)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(m_vBackdatedMTAVersions) Then
                    gPMFunctions.RaiseError(kMethodName, "GetBackdatedPolicyVersions Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If
            CalculateTotalPremium()
            '' MergePremium
            lvwBackdatePolicies.Items.Clear()

            'Load list
            If Information.IsArray(m_vBackdatedMTAVersions) Then
                m_lItemsFound = m_vBackdatedMTAVersions.GetUpperBound(1) + 1

                For lCount As Integer = 0 To m_vBackdatedMTAVersions.GetUpperBound(1)

                    'Policy Type
                    oListItem = lvwBackdatePolicies.Items.Add(sInsuranceFileCnt, m_vBackdatedMTAVersions(ACIPolicyType, lCount), "")
                    'Cover Start Date
                    If CStr(m_vBackdatedMTAVersions(ACICoverStartDate, lCount)) <> "" Then
                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vBackdatedMTAVersions(ACICoverStartDate, lCount))
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = ""
                    End If

                    'Cover end date
                    If CStr(m_vBackdatedMTAVersions(ACICoverEndDate, lCount)) <> "TOTAL" Then
                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vBackdatedMTAVersions(ACICoverEndDate, lCount))
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vBackdatedMTAVersions(ACICoverEndDate, lCount))
                    End If

                    'Status

                    If m_vBackdatedMTAVersions(ACIStatus, lCount) <> "" Then
                        ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vBackdatedMTAVersions(ACIQuoteStatus, lCount))
                    End If
                    'MTA reversed Premium  4
                    ListViewHelper.GetListViewSubItem(oListItem, 4).Text = Math.Round(ToSafeCurrency(m_vBackdatedMTAVersions(ACIReversedPremium, lCount)), 2)
                    'MTA regenerated Premium
                    ListViewHelper.GetListViewSubItem(oListItem, 5).Text = Math.Round(ToSafeCurrency(m_vBackdatedMTAVersions(ACIReappliedPremium, lCount)), 2)
                    'MTA regenerated Premium
                    'MTA reversed Comm
                    ListViewHelper.GetListViewSubItem(oListItem, 6).Text = Math.Round(ToSafeCurrency(m_vBackdatedMTAVersions(ACIReversedComm, lCount)), 2)
                    'MTA regenerated Comm
                    ListViewHelper.GetListViewSubItem(oListItem, 7).Text = Math.Round(ToSafeCurrency(m_vBackdatedMTAVersions(ACIReappliedComm, lCount)), 2)
                    'MTA reversed Fee
                    ListViewHelper.GetListViewSubItem(oListItem, 8).Text = Math.Round(ToSafeCurrency(m_vBackdatedMTAVersions(ACIReversedFee, lCount)), 2)
                    'MTA regenerated Fee
                    ListViewHelper.GetListViewSubItem(oListItem, 9).Text = Math.Round(ToSafeCurrency(m_vBackdatedMTAVersions(ACIReappliedFee, lCount)), 2)
                    ListViewHelper.GetListViewSubItem(oListItem, 10).Text = ToSafeLong(m_vBackdatedMTAVersions(ACIReappliedIFileCnt, lCount))
                    ListViewHelper.GetListViewSubItem(oListItem, 11).Text = ToSafeLong(m_vBackdatedMTAVersions(ACIReversedIFileCnt, lCount))

                    If UCase(m_vBackdatedMTAVersions(ACIQuoteStatus, lCount)) <> "QUOTED" Then
                        m_bAllQuoted = False
                    End If

                    oListItem.Tag = lCount

                Next

                txtPremiumTotal.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency,
                                                   vFieldValue:=TotalPremium)
                txtCommTotal.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency,
                                                   vFieldValue:=m_crTotalCommission)

                txtFeeTotal.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency,
                                                   vFieldValue:=m_crTotalFees)
            End If
        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=excep)

            Return nResult

        End Try

        Return nResult
    End Function

    ''' <summary>
    ''' CalculateTotalPremium
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CalculateTotalPremium()

        Dim crAmount As Decimal

        crAmount = 0
        For i As Integer = 0 To UBound(m_vBackdatedMTAVersions, 2)
            crAmount = crAmount + ToSafeCurrency(m_vBackdatedMTAVersions(ACIReversedPremium, i)) + ToSafeCurrency(m_vBackdatedMTAVersions(ACIReappliedPremium, i))
        Next

        TotalPremium = crAmount

        crAmount = 0
        For i As Integer = 0 To UBound(m_vBackdatedMTAVersions, 2)
            crAmount = crAmount + ToSafeCurrency(m_vBackdatedMTAVersions(ACIReversedComm, i)) + ToSafeCurrency(m_vBackdatedMTAVersions(ACIReappliedComm, i))
        Next

        m_crTotalCommission = crAmount

        crAmount = 0
        For i As Integer = 0 To UBound(m_vBackdatedMTAVersions, 2)
            crAmount = crAmount + ToSafeCurrency(m_vBackdatedMTAVersions(ACIReversedFee, i)) + ToSafeCurrency(m_vBackdatedMTAVersions(ACIReappliedFee, i))
        Next

        m_crTotalFees = crAmount
    End Sub


    'WPR 33-75 added
    Private Function ProcessRisk(ByVal iTask As Integer, ByVal lInsuranceFileCnt As Integer, Optional ByVal lRiskID As Integer = 0, Optional ByVal lScreenID As Integer = 0, Optional ByVal lRiskTypeID As Integer = 0, Optional ByVal lRiskFolderID As Integer = 0) As Integer

        Const kMethodName As String = "ProcessRisk"

        Dim lReturn As Integer
        Dim lStatus As Integer
        Dim lRiskNumber As Integer
        Dim lTempInsuranceFileCnt As Integer
        Dim lTempRiskID As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Save these so that they can be restored
            lTempRiskID = m_frmParent.RiskId
            lTempInsuranceFileCnt = m_frmParent.InsuranceFileCnt
            m_frmParent.Status = gPMConstants.PMEReturnCode.PMOK

            If iTask = gPMConstants.PMEComponentAction.PMAdd Then
                ' get risk type to add from user input
                lReturn = m_frmParent.GetRiskType()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "GetRiskType Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            ElseIf iTask = gPMConstants.PMEComponentAction.PMDelete Then
                'Attempt to delete the Risk
                If m_oFindRisk Is Nothing Then
                    lReturn = g_oObjectManager.GetInstance(
                              oObject:=m_oFindRisk,
                              sClassName:="bSIRFindRisk.Form",
                              vInstanceManager:=PMGetViaClientManager)
                    If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                        RaiseError(kMethodName, "Failed to instance of bSIRFindRisk.Form", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If

                lReturn = m_oFindRisk.DeleteRisk(lInsuranceFileCnt:=lInsuranceFileCnt,
                                                   lRiskID:=lRiskID)

                If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    RaiseError(kMethodName, "bSIRFindRisk  DeleteRisk Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                If lRiskID = 0 Then
                    'This risk was not an original one that needed to be marked as deleted
                    'so it's now really gone
                    Return result
                End If

                'This Risk cannot be deleted because it was originally on the Policy before the MTA
                'started so perform a full Risk Delete
                m_oFindRisk.Dispose()
                m_oFindRisk = Nothing

            End If

            ' if the user has chosen to cancel on the get risk type interface
            ' then quit the add process without error
            If (m_frmParent.Status = gPMConstants.PMEReturnCode.PMCancel) Then
                Return result
            End If

            ' get instance of risk interface
            If m_oRisk Is Nothing Then
                lReturn = g_oObjectManager.GetInstance(
                          oObject:=m_oRisk,
                          sClassName:="iPMURisk.Interface_Renamed",
                          vInstanceManager:=PMGetLocalInterface)
                If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    RaiseError(kMethodName, "Failed to get instance of iPMURisk.Interface", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            ' set properties
            m_oRisk.PartyCnt = m_lPartyCnt
            m_oRisk.Shortname = m_sShortName
            m_oRisk.InsuranceFolderCnt = m_lInsuranceFolderCnt
            m_oRisk.InsuranceFileCnt = lInsuranceFileCnt
            m_oRisk.ProductID = m_lProductID
            m_oRisk.SourceID = m_iSourceID
            m_oRisk.CopyRisk = False

            If iTask = gPMConstants.PMEComponentAction.PMAdd Then
                'The values come from the parent GetRiskType call
                m_oRisk.RiskId = 0
                m_oRisk.ScreenID = m_frmParent.ScreenID
                m_oRisk.RiskTypeID = m_frmParent.RiskTypeID
            Else
                m_oRisk.RiskId = lRiskID
                m_oRisk.ScreenID = lScreenID
                m_oRisk.RiskTypeID = lRiskTypeID
            End If

            ' set process modes (use NB because we don't want full rollback)
            lReturn = m_oRisk.SetProcessModes(vTask:=iTask,
                                              vTransactionType:="NB")
            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "iPMURisk.Interface.SetProcessModes Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_oRisk.Task = iTask

            ' start the interface
            lReturn = m_oRisk.Start
            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "iPMURisk.Interface.Start Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get return details from the interface
            lStatus = m_oRisk.Status
            m_frmParent.RiskId = m_oRisk.RiskId
            lRiskID = m_oRisk.RiskId

            If iTask = gPMConstants.PMEComponentAction.PMAdd Then
                ' Find out the next risk number for this policy
                lReturn = m_oBusiness.GetNextRiskNo(v_lInsuranceFileCnt:=lInsuranceFileCnt,
                                                      r_lRiskNumber:=lRiskNumber)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Unable to retrieve Next Risk Number", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' Save the risk number to the risk record
                lReturn = m_oBusiness.UpdateRiskNo(v_lRiskCnt:=lRiskID,
                                                     v_lRiskNumber:=lRiskNumber)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Unable to update the Next Risk Number on the Risk record", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            If (lStatus <> gPMConstants.PMEReturnCode.PMCancel) Then

                ' get the risk ratings
                m_frmParent.InsuranceFileCnt = lInsuranceFileCnt
                lReturn = m_frmParent.GetRiskRating(iTask:=iTask)
                If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    RaiseError(kMethodName, "GetRiskRating Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                If (m_frmParent.Status <> gPMConstants.PMEReturnCode.PMCancel) Then

                    ' get the risk reinsurance
                    lReturn = m_frmParent.GetRiskReinsurance()
                    If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                        RaiseError(kMethodName, "GetRiskReinsurance Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    If (m_frmParent.Status <> gPMConstants.PMEReturnCode.PMCancel) Then
                        lReturn = m_oBusiness.UpdateIFRLInkRisk(v_lInsuranceFileCnt:=lInsuranceFileCnt, v_lRiskID:=lRiskID)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            RaiseError(kMethodName, "GetRiskReinsurance Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        'lReturn = m_oBusiness.UnquoteMandatoryRisk(v_lInsuranceFolderCnt:=lInsuranceFileCnt, _
                        '                                             v_lRiskID:=lRiskID)
                        'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        '    RaiseError(kMethodName, "UnquoteMandatoryRisk Failed", gPMConstants.PMELogLevel.PMLogError)
                        'End If

                        If iTask = gPMConstants.PMEComponentAction.PMAdd Then
                            'Copy Risks forward
                            CopyRisksForward(lInsuranceFileCnt, lRiskID)
                        ElseIf iTask = gPMConstants.PMEComponentAction.PMEdit Then
                            UnquoteRisksForward(lInsuranceFileCnt, lRiskID, lRiskFolderID)
                        End If

                        lReturn = ProcessRITax(
                                    v_lInsuranceFileCnt:=lInsuranceFileCnt,
                                    v_lRiskCnt:=0)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            RaiseError(kMethodName, "ProcessAgentCommission Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        lReturn = ProcessAgentCommission(
                                    v_lInsuranceFileCnt:=lInsuranceFileCnt)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            RaiseError(kMethodName, "ProcessAgentCommission Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        If m_oChangePolicyStatus Is Nothing Then
                            lReturn = g_oObjectManager.GetInstance(
                                      oObject:=m_oChangePolicyStatus,
                                      sClassName:="bSIRChangePolicyStatus.Business",
                                      vInstanceManager:=PMGetViaClientManager)
                            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                RaiseError(kMethodName, "Failed to get instance of bSIRChangePolicyStatus.Business", gPMConstants.PMELogLevel.PMLogError)
                            End If
                        End If

                        lReturn = m_oChangePolicyStatus.UpdatePolicyPremium(
                                                    v_lInsuranceFileCnt:=lInsuranceFileCnt)
                    Else
                        lReturn = m_oBusiness.UpdateRiskStatus(v_lRiskCnt:=lRiskID,
                                        v_sRiskStatusCode:="UNQUOTED")
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            RaiseError(kMethodName, "UpdateRiskStatus Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If
                Else
                    lReturn = m_oBusiness.UpdateRiskStatus(v_lRiskCnt:=lRiskID,
                                    v_sRiskStatusCode:="UNQUOTED")
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "UpdateRiskStatus Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If

            ElseIf iTask = gPMConstants.PMEComponentAction.PMEdit Then
                lReturn = m_oBusiness.UpdateRiskStatus(v_lRiskCnt:=lRiskID,
                                v_sRiskStatusCode:="UNQUOTED")
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "UpdateRiskStatus Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' DO Not Call any functions before here or the error will be lost

            ' If you want to rollback a transaction or something, do it here

        Finally

            m_frmParent.RiskId = lTempRiskID
            m_frmParent.InsuranceFileCnt = lTempInsuranceFileCnt
            RefreshForm(True)
        End Try
        Return result
    End Function

    ''' <summary>
    ''' CopyRiskData
    ''' </summary>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <param name="v_lNewInsuranceFileCnt"></param>
    ''' <param name="v_lInsuranceFolderCnt"></param>
    ''' <param name="v_lRiskCnt"></param>
    ''' <param name="r_lNewRiskCnt"></param>
    ''' <param name="r_sFailureReason"></param>
    ''' <param name="v_dtExpiryDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CopyRiskData(
        ByVal v_lInsuranceFileCnt As Integer,
        ByVal v_lNewInsuranceFileCnt As Integer,
        ByVal v_lInsuranceFolderCnt As Integer,
        ByVal v_lRiskCnt As Integer,
        ByRef r_lNewRiskCnt As Integer,
        ByRef r_sFailureReason As String,
        ByVal v_dtExpiryDate As Object) As Integer

        Const kMethodName As String = "CopyRiskData"

        Dim bFound As Boolean
        Dim lNewGisPolicyLinkID As Integer
        Dim vGisPolicyLinkArray(,) As Object
        Dim vRiskArray(,) As Object
        Dim lCount As Integer
        Dim sXMLDataSetDef As String
        Dim sXMLDataSet As String
        Dim lReturn As Integer
        Dim result As Integer = 0
        Try

            CopyRiskData = gPMConstants.PMEReturnCode.PMTrue

            If m_oRiskData Is Nothing Then
                lReturn = g_oObjectManager.GetInstance(
                          oObject:=m_oRiskData,
                          sClassName:="bSIRRiskData.Business",
                          vInstanceManager:=PMGetViaClientManager)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError("CopyRiskData", "Failed to get Instance of bSIRRiskData", lReturn)
                End If
            End If

            If m_oRenSelection Is Nothing Then
                lReturn = g_oObjectManager.GetInstance(
                          oObject:=m_oRenSelection,
                          sClassName:="bSIRRenSelection.Business",
                          vInstanceManager:=PMGetViaClientManager)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError("CopyRiskData", "Failed to get Instance of bSIRRenSelection", lReturn)
                End If
            End If

            ' Get all risks associate with the InsuranceFileCnt
            lReturn = m_oRiskData.GetRiskAllStatuses(
                      v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                      r_vResultArray:=vRiskArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sFailureReason = "Getting Risk"
                gPMFunctions.RaiseError(kMethodName, r_sFailureReason, gPMConstants.PMELogLevel.PMLogOnError)
            End If

            ' Check if we have any risks
            If Not IsArray(vRiskArray) Then
                r_sFailureReason = "No risks found"
                gPMFunctions.RaiseError(kMethodName, r_sFailureReason, gPMConstants.PMELogLevel.PMLogOnError)
            End If

            ' Find the risk that matches the passed risk count, i.e. the one we want
            ' to copy
            bFound = False
            For lCount = 0 To UBound(vRiskArray, 2)
                If vRiskArray(0, lCount) = v_lRiskCnt Then
                    bFound = True
                    Exit For
                End If
            Next

            ' Check if we have found the risk to copy
            If Not bFound Then
                r_sFailureReason = "Cannot find risk to copy"
                gPMFunctions.RaiseError(kMethodName, r_sFailureReason, gPMConstants.PMELogLevel.PMLogOnError)
            End If

            ' Copy risk with same insurance file cnt
            lReturn = m_oRiskData.CopyRisk(
                      v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt,
                      v_vRiskDetail:=vRiskArray,
                      v_lPosNo:=lCount,
                      r_lRiskCnt:=r_lNewRiskCnt,
                      v_lResetStatus:=gPMConstants.PMEReturnCode.PMTrue,
                      v_bAutoCancellation:=False,
                      v_dtExpiryDate:=v_dtExpiryDate)

            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                r_sFailureReason = "Copy Risk"
                gPMFunctions.RaiseError(kMethodName, r_sFailureReason, gPMConstants.PMELogLevel.PMLogOnError)
            End If

            ' Prepare details to copy GIS Stuff attached to current risk

            ' Get policy link detail
            lReturn = m_oRiskData.GetGISPolicyLink(
                      v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt,
                      v_lRiskID:=vRiskArray(ACRiskPosCnt, lCount),
                      r_vResultArray:=vGisPolicyLinkArray)
            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                r_sFailureReason = "GetGISPolicyLink"
                gPMFunctions.RaiseError(kMethodName, r_sFailureReason, gPMConstants.PMELogLevel.PMLogOnError)
            End If

            ' Do we have any data?
            If (Not (Convert.IsDBNull(vGisPolicyLinkArray(0, 0)))) Then
                ' Make sure GIS object present.

                lReturn = m_oRenSelection.GIS_LoadFromDB(
                          Trim$(vGisPolicyLinkArray(4, 0)),
                          v_lInsuranceFolderCnt,
                          vGisPolicyLinkArray(0, 0),
                          vRiskArray(0, lCount))
                If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    r_sFailureReason = "LoadFromDB"
                    gPMFunctions.RaiseError(kMethodName, r_sFailureReason, gPMConstants.PMELogLevel.PMLogOnError)
                End If

                ' REMEMBER we are storing folder_cnt in file_cnt field now !!!!!
                ' So we pass existing folder_cnt in for old and new file_cnt.
                'sj 12/02/2003 - start
                'PS104
                lReturn = m_oRenSelection.CopyDataSet(
                          v_sDataModelCode:=Trim$(vGisPolicyLinkArray(4, 0)),
                          r_lNewGISPolicyLinkId:=lNewGisPolicyLinkID,
                          r_sXMLDataSetDef:=sXMLDataSetDef,
                          r_sXMLDataset:=sXMLDataSet,
                          v_vOldGISPolicyLinkId:=vGisPolicyLinkArray(0, 0),
                          v_vOldInsuranceFileCnt:=v_lInsuranceFolderCnt,
                          v_vOldRiskID:=vRiskArray(0, lCount),
                          v_vNewInsuranceFileCnt:=v_lInsuranceFolderCnt,
                          v_vNewRiskID:=r_lNewRiskCnt)

                If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    r_sFailureReason = "CopyDataSet"
                    gPMFunctions.RaiseError(kMethodName, r_sFailureReason, gPMConstants.PMELogLevel.PMLogOnError)
                End If

                ' Initialise the Data Set with the Object/Properties
                lReturn = m_oRenSelection.LoadFromXML(
                          v_sXMLDataSetDef:=sXMLDataSetDef,
                          v_sXMLDataSet:=sXMLDataSet)
                If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    r_sFailureReason = "LoadFromXML"
                    gPMFunctions.RaiseError(kMethodName, r_sFailureReason, gPMConstants.PMELogLevel.PMLogOnError)
                End If

                'RWH(28/02/2001)
                lReturn = m_oRenSelection.GIS_SaveToDB(
                          v_sGisDataModelCode:=Trim$(vGisPolicyLinkArray(4, 0)))
                If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    r_sFailureReason = "SaveToDB"
                    gPMFunctions.RaiseError(kMethodName, r_sFailureReason, gPMConstants.PMELogLevel.PMLogOnError)
                End If

                lReturn = m_oAutoMTA.MarkRiskAsUnquoted(v_lNewInsuranceFileCnt, r_lNewRiskCnt)
                If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    r_sFailureReason = "MarkRiskAsUnquoted"
                    gPMFunctions.RaiseError(kMethodName, r_sFailureReason, gPMConstants.PMELogLevel.PMLogOnError)
                End If

            End If


        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=CopyRiskData, excep:=excep)

            Return result

        End Try
    End Function
    'WPR 33-75 added
    Private Function CopyRisksForward(ByVal lFromInsuranceFileCnt As Integer, ByVal lRiskID As Integer) As Integer
        Dim lLine As Integer
        Dim lStartLine As Integer
        Dim lCurrentInsuranceFileCnt As Integer
        Dim lReturn As Integer
        Dim sFailureReason As String

        lCurrentInsuranceFileCnt = lFromInsuranceFileCnt
        'lStartLine = lvwBackdatePolicies.SelectedItem.Index + 1
        lStartLine = lvwBackdatePolicies.SelectedItems(0).Index + 1

        For lLine = lStartLine To lvwBackdatePolicies.Items.Count
            If lvwBackdatePolicies.Items(lLine).SubItems(8).Text <> "REPLACED" Then
                If Convert.ToInt32(lvwBackdatePolicies.Items(lLine).SubItems(9).Text) <> lCurrentInsuranceFileCnt Then
                    lCurrentInsuranceFileCnt = Convert.ToInt32(lvwBackdatePolicies.Items(lLine).SubItems(9).Text)

                    If lCurrentInsuranceFileCnt <> 0 Then
                        lReturn = CopyRiskData(lFromInsuranceFileCnt, lCurrentInsuranceFileCnt,
                            m_lInsuranceFolderCnt, lRiskID, 0, sFailureReason, ToSafeDate(lvwBackdatePolicies.Items(lLine).SubItems(3).Text))

                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            MsgBox("Copy Risk Failure: " & sFailureReason, vbCritical, "Copy Risks Forward")
                            Exit For
                        End If
                    End If
                End If
            End If
        Next lLine
    End Function
    'WPR 33-75 added
    Private Function UnquoteRisksForward(ByVal lFromInsuranceFileCnt As Integer, ByVal lRiskID As Integer, ByVal lRiskFolderID As Integer) As Integer
        Dim lLine As Integer
        Dim lStartLine As Integer
        Dim lCurrentInsuranceFileCnt As Integer
        Dim lCurrentRiskCnt As Integer
        Dim lReturn As Integer

        lCurrentInsuranceFileCnt = lFromInsuranceFileCnt
        'changed for WPR 33-75 
        lStartLine = lvwBackdatePolicies.SelectedItems.Item(0).Index + 1

        For lLine = lStartLine To lvwBackdatePolicies.Items.Count - 1
            If lvwBackdatePolicies.Items(lLine).SubItems.Item(8).Text <> "REPLACED" Then

                If Convert.ToInt32(lvwBackdatePolicies.Items(lLine).SubItems.Item(13).Text) = lRiskFolderID _
                    And Convert.ToInt32(lvwBackdatePolicies.Items(lLine).SubItems.Item(10).Text) > lRiskID Then
                    lCurrentRiskCnt = ToSafeLong(lvwBackdatePolicies.Items(lLine).SubItems.Item(10).Text)

                    If lCurrentRiskCnt <> 0 Then
                        lReturn = m_oBusiness.UpdateRiskStatus(v_lRiskCnt:=lCurrentRiskCnt, v_sRiskStatusCode:="UNQUOTED")

                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            'Exit here
                            Exit For
                        End If
                    End If
                End If
            End If
        Next lLine
    End Function
    'WPR 33-75 added
    Private Function ProcessRITax(
           ByVal v_lInsuranceFileCnt As Integer,
           ByVal v_lRiskCnt As Integer) As Integer

        Dim result As Integer
        Const kMethodName As String = "ProcessRITax"

        Try
            result = gPMConstants.PMEReturnCode.PMTrue


            Dim bApplyTaxes As Boolean
            Dim vRITax As Object
            Dim sDesc As String
            Dim bTaxesSwitchedOff As Boolean
            Dim m_oRITax As Object
            Dim m_lReturn As Integer


            m_lReturn = g_oObjectManager.GetInstance(
                      oObject:=m_oRITax,
                      sClassName:="bSIRRITax.Business",
                      vInstanceManager:=PMGetViaClientManager)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to get instance of bSirAgentCommission.Business", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Do we need to apply taxes?
            m_lReturn = m_oRITax.ApplyTaxes(
                        v_lInsuranceFileCnt,
                        v_lRiskCnt,
                        bApplyTaxes, bTaxesSwitchedOff)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, kMethodName & "Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If

            If bApplyTaxes = False Then
                'Nothing to do
                Return result
            End If

            m_lReturn = m_oRITax.SetProcessModes(
                         vTask:=gPMConstants.PMEComponentAction.PMEdit, vTransactionType:=m_sTransactionType)

            With m_oRITax
                .InsuranceFileCnt = v_lInsuranceFileCnt
                .RiskCnt = v_lRiskCnt
            End With

            If v_lRiskCnt > 0 Then
                m_oRITax.RiskCnt = v_lRiskCnt
                m_lReturn = m_oRITax.GetRiskTax(
                             r_vRiskTax:=vRITax,
                             r_sDescription:=sDesc,
                             iTask:=gPMConstants.PMEComponentAction.PMEdit)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    RaiseError(kMethodName, kMethodName & "Failed.", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' update risk tax to actually set the tax amount
                m_lReturn = m_oRITax.UpdateRiskTax(v_vRiskTax:=vRITax)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, kMethodName & "Failed.", gPMConstants.PMELogLevel.PMLogError)
                End If

            Else
                m_oRITax.InsuranceFileCnt = v_lInsuranceFileCnt
                m_lReturn = m_oRITax.GetInsuranceFileTax(
                             r_vInsuranceFileTax:=vRITax,
                             r_sDescription:=sDesc,
                             iTask:=gPMConstants.PMEComponentAction.PMEdit)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    RaiseError(kMethodName, kMethodName & "Failed.", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If


        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=ProcessRITax, excep:=excep)

            Return result

        End Try
    End Function

    'WPR 33-75 added
    Private Function ProcessAgentCommission(
           ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim bCommissionRequired As Boolean
        Dim vAgentCommission As Object
        Dim m_oAgentCommission As Object
        Dim m_lReturn As Integer
        Dim result As Integer
        Const kMethodName As String = "ProcessAgentCommission"

        result = gPMConstants.PMEReturnCode.PMTrue
        ProcessAgentCommission = gPMConstants.PMEReturnCode.PMTrue
        Try

            m_lReturn = g_oObjectManager.GetInstance(
                      oObject:=m_oAgentCommission,
                      sClassName:="bSirAgentCommission.Business",
                      vInstanceManager:=PMGetViaClientManager)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to get instance of bSirAgentCommission.Business", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_oAgentCommission.InsuranceFileCnt = v_lInsuranceFileCnt

            'Do we require agent commission
            m_lReturn = m_oAgentCommission.CheckDisplayCommission(
                        r_bDisplayScreen:=bCommissionRequired)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, kMethodName & "Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If

            If bCommissionRequired = False Then
                'No processing required
                Return result

            End If

            'Calculate agent commission
            m_lReturn = m_oAgentCommission.CalculateAgentCommission(
                        v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                        v_sTransactionType:=m_sTransactionType,
                        r_vntResult:=vAgentCommission)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, kMethodName & "Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Calculate lead commission
            m_lReturn = m_oAgentCommission.UpdateLeadCommission(
                        v_lInsuranceFileCnt:=v_lInsuranceFileCnt)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, kMethodName & "Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=ProcessAgentCommission, excep:=excep)
            Return result

        End Try

    End Function

    ''' <summary>
    ''' RefreshForm
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RefreshForm(ByVal v_bReload As Boolean)
        Const kMethodName As String = "RefreshForm"
        Dim nReturn As Integer
        Dim nSubValue As Integer

        Try
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            nReturn = GetBackdatedPolicyVersions(v_bReload)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "RefreshForm Failed", gPMConstants.PMEReturnCode.PMError)
            End If

        Catch excep As System.Exception

            nReturn = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nSubValue, excep:=excep)

        End Try

    End Sub
    Private Sub lvwBackdatePolicies_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwBackdatePolicies.SelectedIndexChanged
        If lvwBackdatePolicies.SelectedItems.Count > 0 Then
            cmdEdit.Enabled = True
        End If
    End Sub

    ''' <summary>
    ''' Run Process
    ''' </summary>
    ''' <param name="sComponentName"></param>
    ''' <param name="oKeys"></param>
    ''' <param name="nStatus"></param>
    ''' <param name="nTask"></param>
    ''' <param name="sTransactionType"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Private Function RunProcess(ByVal sComponentName As String,
            ByRef oKeys As Object,
            ByRef nStatus As Integer,
            ByRef nTask As Integer,
            ByRef sTransactionType As String) As Integer
        Dim oComponent As Object
        Dim sClass As String
        Dim nReturn As Integer

        Const kMethodName As String = "RunProcess"

        Try
            RunProcess = gPMConstants.PMEReturnCode.PMTrue

            sClass = sComponentName

            ' Create Policy interface object
            nReturn = g_oObjectManager.GetInstance(
                    oObject:=oComponent,
                    sClassName:=sClass,
                    vInstanceManager:=PMGetLocalInterface)

            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If

            nReturn = oComponent.NavigatorV3_SetKeys(oKeys)

            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If

            nReturn = oComponent.NavigatorV3_SetProcessModes(vTask:=nTask,
                        vTransactionType:=sTransactionType)

            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If

            nReturn = oComponent.NavigatorV3_Start
            If (nReturn = gPMConstants.PMEReturnCode.PMTrue) Then
                nStatus = oComponent.NavigatorV3_Status
            Else
                RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If

            oComponent.Dispose()

            oComponent = Nothing

        Catch excep As System.Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=RunProcess, excep:=excep)
        End Try
        If Not (oComponent Is Nothing) Then
            oComponent.Dispose()
            oComponent = Nothing
        End If

    End Function


    Private Sub cmdView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdView.Click
        Dim nStatus As Integer
        Dim nReturn As Integer
        Dim oKeys(,) As Object

        If lvwBackdatePolicies.Items(lvwBackdatePolicies.SelectedItems.Item(0).Index).SubItems(10).Text = m_lInsuranceFileCnt Then
            MsgBox("You cannot view this policy. " & vbCrLf & vbCrLf & "This is the master Policy that has been edited on the List Risks screen.", vbExclamation, "Edit Policy")
            Exit Sub
        End If

        ' set initial status
        nStatus = gPMConstants.PMEReturnCode.PMOK

        While nStatus = gPMConstants.PMEReturnCode.PMOK Or nStatus = 2200

            If nStatus = gPMConstants.PMEReturnCode.PMOK Then
                ReDim oKeys(1, 4)
                oKeys(0, 0) = "insurance_file_cnt"
                oKeys(1, 0) = ToSafeLong(lvwBackdatePolicies.Items(lvwBackdatePolicies.SelectedItems.Item(0).Index).SubItems(10).Text)
                oKeys(0, 1) = "insurance_folder_cnt"
                oKeys(1, 1) = m_lInsuranceFolderCnt
                oKeys(0, 2) = "shortname"
                oKeys(1, 2) = m_sShortName
                oKeys(0, 2) = "is_out_of_sequence_editing"
                oKeys(1, 2) = True

                If (ToSafeString(lvwBackdatePolicies.SelectedItems.Item(0).Text).ToString.Substring(0, 3) = "MTA") Then
                    nReturn = RunProcess("iPMUListRisks.NavigatorV3", oKeys, nStatus, gPMConstants.PMEComponentAction.PMView, "MTA")
                Else
                    nReturn = RunProcess("iPMUListRisks.NavigatorV3", oKeys, nStatus, gPMConstants.PMEComponentAction.PMView, "REN")
                End If
            Else
                ReDim oKeys(1, 5)
                oKeys(0, 0) = "party_cnt"
                oKeys(1, 0) = m_lPartyCnt
                oKeys(0, 1) = "insurance_file_cnt"
                oKeys(1, 1) = ToSafeLong(lvwBackdatePolicies.Items(lvwBackdatePolicies.SelectedItems.Item(0).Index).SubItems(10).Text)
                oKeys(0, 2) = "insurance_folder_cnt"
                oKeys(1, 2) = m_lInsuranceFolderCnt
                oKeys(0, 3) = "shortname"
                oKeys(1, 3) = m_sShortName
                oKeys(0, 4) = "Product_id"
                oKeys(1, 4) = m_lProductID
                oKeys(0, 5) = "is_out_of_sequence_editing"
                oKeys(1, 5) = True

                nReturn = RunProcess("iPMUPolicy.NavigatorV3", oKeys, nStatus, gPMConstants.PMEComponentAction.PMView, "NB")
            End If

            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Exit Sub
            End If
            If (nStatus = gPMConstants.PMEReturnCode.PMCancel) Or (nStatus = gPMConstants.PMEReturnCode.PMError) Then
                Exit Sub
            ElseIf nStatus = 2200 Then
                'Requote

            End If
        End While

    End Sub
    ''' <summary>
    ''' cmdEdit_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click

        Dim nStatus As Integer
        Dim nReturn As Integer
        Dim oKeys(,) As Object

        If lvwBackdatePolicies.Items(lvwBackdatePolicies.SelectedItems.Item(0).Index).SubItems(10).Text = m_lInsuranceFileCnt Then
            MsgBox("You cannot edit this policy. " & vbCrLf & vbCrLf & "This is the master Policy that has been edited on the List Risks screen.", vbExclamation, "Edit Policy")
            Exit Sub
        Else
            ' establish if any previous version is still unquoted
            For lCnt As Integer = 0 To lvwBackdatePolicies.SelectedItems.Item(0).Index - 1
                If UCase(lvwBackdatePolicies.Items(lCnt).SubItems(3).Text()) = "UNQUOTED" Then
                    MsgBox("Please select Policy Versions in sequence of Cover Start Date.", vbExclamation, "Edit Policy")
                    Exit Sub
                End If
            Next

            ' Process reversal\reversed version here for editing
            nReturn = m_oAutoMTA.AddQuotes(ToSafeLong(lvwBackdatePolicies.Items(lvwBackdatePolicies.SelectedItems.Item(0).Index).SubItems(10).Text), ToSafeLong(lvwBackdatePolicies.Items(lvwBackdatePolicies.SelectedItems.Item(0).Index - 1).SubItems(10).Text))
            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Exit Sub
            End If
        End If

        ' set initial status
        nStatus = gPMConstants.PMEReturnCode.PMOK

            While nStatus = gPMConstants.PMEReturnCode.PMOK Or nStatus = 2200

                If nStatus = gPMConstants.PMEReturnCode.PMOK Then
                    ReDim oKeys(1, 4)
                    oKeys(0, 0) = "insurance_file_cnt"
                    oKeys(1, 0) = ToSafeLong(lvwBackdatePolicies.Items(lvwBackdatePolicies.SelectedItems.Item(0).Index).SubItems(10).Text)
                    oKeys(0, 1) = "insurance_folder_cnt"
                    oKeys(1, 1) = m_lInsuranceFolderCnt
                    oKeys(0, 2) = "shortname"
                    oKeys(1, 2) = m_sShortName
                    oKeys(0, 2) = "is_out_of_sequence_editing"
                    oKeys(1, 2) = True

                    If ToSafeString(lvwBackdatePolicies.SelectedItems.Item(0).Text).ToString.Substring(0, 3) = "MTA" Then
                        nReturn = RunProcess("iPMUListRisks.NavigatorV3", oKeys, nStatus, gPMConstants.PMEComponentAction.PMEdit, "MTA")
                    Else
                        Dim sSysOptionValue As String = "0"
                        ' 5262 - This option will apply MTA Rates on OOS Renewal
                        nReturn = CType(iPMFunc.GetSystemOption(v_iOptionNumber:=ACApplyMTATaxesOnOOSRenewalySystemOptionNumber, r_sOptionValue:=sSysOptionValue, v_iSourceID:=g_iSourceID), PMEReturnCode)

                        oKeys(0, 3) = "ApplyMTATaxRatesonRen"
                        oKeys(1, 3) = sSysOptionValue
                        nReturn = RunProcess("iPMUListRisks.NavigatorV3", oKeys, nStatus, gPMConstants.PMEComponentAction.PMEdit, "REN")
                    End If
                Else
                    ReDim oKeys(1, 5)
                    oKeys(0, 0) = "party_cnt"
                    oKeys(1, 0) = m_lPartyCnt
                    oKeys(0, 1) = "insurance_file_cnt"
                    oKeys(1, 1) = ToSafeLong(lvwBackdatePolicies.Items(lvwBackdatePolicies.SelectedItems.Item(0).Index).SubItems(10).Text)
                    oKeys(0, 2) = "insurance_folder_cnt"
                    oKeys(1, 2) = m_lInsuranceFolderCnt
                    oKeys(0, 3) = "shortname"
                    oKeys(1, 3) = m_sShortName
                    oKeys(0, 4) = "Product_id"
                    oKeys(1, 4) = m_lProductID
                    oKeys(0, 5) = "is_out_of_sequence_editing"
                    oKeys(1, 5) = True

                    nReturn = RunProcess("iPMUPolicy.NavigatorV3", oKeys, nStatus, gPMConstants.PMEComponentAction.PMView, "NB")
                End If

                If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    Exit Sub
                End If
                If (nStatus = gPMConstants.PMEReturnCode.PMCancel) Or (nStatus = gPMConstants.PMEReturnCode.PMError) Then
                    Exit Sub
                ElseIf nStatus = 2200 Then
                    'Requote

                End If
            End While

            RefreshForm(True)
    End Sub

End Class
