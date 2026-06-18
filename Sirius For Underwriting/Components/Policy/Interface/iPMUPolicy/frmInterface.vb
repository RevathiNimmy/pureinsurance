Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form


    Private Const ACClass As String = "frmInterface"
    'Developer Guide No. 7
    Private Const vbFormCode As Integer = 0
    ' Private variables
    Private m_lPartyCnt As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_lInsuranceFolderCnt As Integer
    Private m_sInsuranceRef As String = ""
    Private m_lProductId As Integer
    Private m_lBusinessTypeId As Integer
    Private m_bIsRenewal As Boolean
    Private m_lPolicySourceID As Integer
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lReturn As Integer
    Private m_lErrorNumber As Integer
    ' SourceID
    Private m_iSourceID As Integer

    'TN20010126 Start
    Private m_sTransactionType As String = ""
    Private m_bIsSubAgentAdded As Boolean 'Subagent status
    Private m_bIsSingleInstalmentPlan As Boolean
    Private m_vLeadAgentCnt As Object
    'Kevin Renshaw (CMG) 08/04/2003
    Private m_bPolicyLapsed As Boolean
    'Start (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1)
    Private m_bIsTrueMonthlypolicyandNextInstalmentRenewal As Boolean
    'End (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1)
    'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
    Private m_bBackDatedMTAsAllowed As Boolean
    Private m_sSelectedPolicyStatus As String = ""
    Private m_bIsMTATemp As Boolean
    Private m_dtRenewaldate As Date
    Private m_bIsPriorDate As Boolean
    Private m_bIsRenewed As Boolean
    Private m_dtLapsedDate As Date

    Public Property BackDatedMTAsAllowed() As Boolean
        Get
            Return m_bBackDatedMTAsAllowed
        End Get
        Set(ByVal Value As Boolean)
            m_bBackDatedMTAsAllowed = Value
        End Set
    End Property
    Public Property SelectedPolicyStatus() As String
        Get
            Return m_sSelectedPolicyStatus
        End Get
        Set(ByVal Value As String)
            m_sSelectedPolicyStatus = Value
        End Set
    End Property
    Public Property IsMTATemp() As String
        Get
            Return CStr(m_bIsMTATemp)
        End Get
        Set(ByVal Value As String)
            m_bIsMTATemp = CBool(Value)
        End Set
    End Property
    Public Property Renewaldate() As Date
        Get
            Return m_dtRenewaldate
        End Get
        Set(ByVal Value As Date)
            m_dtRenewaldate = Value
        End Set
    End Property
    Public Property IsPriorDate() As Boolean
        Get
            Return m_bIsPriorDate
        End Get
        Set(ByVal Value As Boolean)
            m_bIsPriorDate = Value
        End Set
    End Property
    Public Property IsRenewed() As Boolean
        Get
            Return m_bIsRenewed
        End Get
        Set(ByVal Value As Boolean)
            m_bIsRenewed = Value
        End Set
    End Property
    Public Property LapsedDate() As Date
        Get
            Return m_dtLapsedDate
        End Get
        Set(ByVal Value As Date)
            m_dtLapsedDate = Value
        End Set
    End Property
    'End  (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
    'Start (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1)
    Public Property IsTrueMonthlypolicyandNextInstalmentRenewal() As Boolean
        Get
            Return m_bIsTrueMonthlypolicyandNextInstalmentRenewal
        End Get
        Set(ByVal Value As Boolean)
            m_bIsTrueMonthlypolicyandNextInstalmentRenewal = Value
        End Set
    End Property
    'End (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1)

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property
    'TN20010126 End

    Public WriteOnly Property SourceID() As Integer
        Set(ByVal Value As Integer)
            m_iSourceID = Value
        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get
            Return m_lStatus
        End Get
    End Property

    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property

    Public Property Task() As Integer
        Get

            ' Return the objects task.
            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the objects task.
            m_iTask = Value

        End Set
    End Property

    Public Property InsuranceFolderCnt() As Integer
        Get
            Return m_lInsuranceFolderCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFolderCnt = Value
        End Set
    End Property

    Public Property InsuranceFileCnt() As Integer
        Get
            Return m_lInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    Public Property PolicySourceID() As Integer
        Get
            Return m_lPolicySourceID
        End Get
        Set(ByVal Value As Integer)
            m_lPolicySourceID = Value
        End Set
    End Property

    Public ReadOnly Property InsuranceRef() As String
        Get
            Return m_sInsuranceRef
        End Get
    End Property

    Public Property ProductId() As Integer
        Get
            Return m_lProductId
        End Get
        Set(ByVal Value As Integer)
            m_lProductId = Value
        End Set
    End Property

    Public Property BusinessTypeId() As Integer
        Get
            Return m_lBusinessTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lBusinessTypeId = Value
        End Set
    End Property
    Public Property IsRenewal() As Boolean
        Get
            Return m_bIsRenewal
        End Get
        Set(ByVal Value As Boolean)
            m_bIsRenewal = Value
        End Set
    End Property

    Public ReadOnly Property IsSubAgentAdded() As Boolean
        Get
            Return m_bIsSubAgentAdded
        End Get
    End Property

    Public Property PolicyLapsed() As Boolean
        Get
            Return m_bPolicyLapsed
        End Get
        Set(ByVal Value As Boolean)
            m_bPolicyLapsed = Value
        End Set
    End Property

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        ' Click event of the Cancel button.
        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = uctPMUPolicyControl1.CancelClick()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                'Developer Guide No. 231
                Me.Hide()
            End If

            'Unlock Current Policy 'PN35753 --RC
            If m_sTransactionType = gSIRLibrary.SIRProcessCodeMTA Or m_sTransactionType = "NB" Or m_sTransactionType = "MTC" Or m_sTransactionType = "MTR" Then
                UNLOCKPOLICY()
                UnLockSingleInstalmentPlan()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdCommission_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCommission.Click
        Try

            m_lReturn = uctPMUPolicyControl1.ValidatePolicy()
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                m_lReturn = uctPMUPolicyControl1.GetCommissionDetail()
                If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get Commission detail.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCommissionDetail", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Sub
                End If
            End If

        Catch excep As System.Exception



            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get Comm Detail.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCommissionDetail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try
    End Sub

    'This button Doc Archive is newly added for implementing the document archive functionality
    Private Sub cmdDocArchive_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDocArchive.Click

        Dim oPMUPolicy As bPMUPolicy.Business

        Dim vClientCode As Object = Nothing
        Dim sClientCode As String = ""
        Dim sOption As String = ""
        Dim sSPUrl As String = ""
        Dim sDocLIB As String = ""
        m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=kSystemOptionDocumentArchive, r_sOptionValue:=sOption, v_iSourceID:=g_iSourceID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return
            End If

        If sOption = "2" Then
            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=kSystemOptionSharepointserverName, r_sOptionValue:=sSPUrl, v_iSourceID:=g_iSourceID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return
            End If


            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=5086, r_sOptionValue:=sDocLIB, v_iSourceID:=g_iSourceID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return
            End If
        End If

        Dim temp_oPMUPolicy As Object = Nothing
        m_lReturn = g_oObjectManager.GetInstance(temp_oPMUPolicy, "bPMUPolicy.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oPMUPolicy = temp_oPMUPolicy

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise bPMUPolicy.Form.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientCode", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Exit Sub

            End If


            m_lReturn = oPMUPolicy.GetClientCode(v_iPartyID:=uctPMUPolicyControl1.PartyCnt, r_vClientarray:=vClientCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise bPMUPolicy.Form.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientCode", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Exit Sub

            End If

        sClientCode = gPMFunctions.ToSafeString(vClientCode(0, 0))

        oPMUPolicy.Dispose()
        If m_lInsuranceFileCnt = 0 Then
            If sOption = "1" Then
                m_lReturn = iPMFunc.RunDocumaster(v_sLinkCode:=sClientCode.Trim() & "1")
            ElseIf sOption = "2" Then
                If sSPUrl.EndsWith("\") Then
                    System.Diagnostics.Process.Start(sSPUrl & sDocLIB & "\" & sClientCode.Trim())
                Else
                    System.Diagnostics.Process.Start(sSPUrl & "\" & sDocLIB & "\" & sClientCode.Trim())
                End If
            End If

        Else
            If sOption = "1" Then
                m_lReturn = iPMFunc.RunDocumaster(v_sLinkCode:=uctPMUPolicyControl1.InsuranceRef.Trim() & "2")
            ElseIf sOption = "2" Then
                If sSPUrl.EndsWith("\") Then
                    System.Diagnostics.Process.Start(sSPUrl & sDocLIB & "\" & sClientCode.Trim() & "\Policy\" & uctPMUPolicyControl1.InsuranceRef.Trim())
                Else
                    System.Diagnostics.Process.Start(sSPUrl & "\" & sDocLIB & "\" & sClientCode.Trim() & "\Policy\" & uctPMUPolicyControl1.InsuranceRef.Trim())
                End If
            End If

        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

    End Sub
    'End (Sriram P) Tech Spec - WR8 - Navigator DME Link.doc section(4.3.1)


    Private Sub cmdFee_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFee.Click
        Try

            m_lReturn = uctPMUPolicyControl1.GetFeeDetail()
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get Fee detail.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFeeDetail", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

        Catch excep As System.Exception



            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get Fee Detail.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFeeDetail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try
    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        ' Fire up the help screen
        'm_lReturn = PMHelpFunc.ShowHelp(dlgHelp, ScreenHelpID)
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID)

    End Sub

    Private Sub cmdInstalment_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdInstalment.Click
        Try

            m_lReturn = uctPMUPolicyControl1.GetInstalmentDetail()
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get Commission detail.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCommissionDetail", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

        Catch excep As System.Exception



            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get Comm Detail.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCommissionDetail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try
    End Sub

    Private Sub cmdLapseQuote_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdLapseQuote.Click

        If MessageBox.Show("Lapse this Quote", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
            m_lReturn = uctPMUPolicyControl1.LapseQuote()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '            MsgBox "Failed to lapse quote", vbInformation + vbOKOnly, ACApp
            Else
                m_bPolicyLapsed = True
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                'Developer Guide No. 231
                Me.Hide()
            End If
        End If

    End Sub

    Private Sub cmdMTACancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdMTACancel.Click

        If MessageBox.Show("Cancel this MTA", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
            m_lReturn = uctPMUPolicyControl1.MTACancellation(v_lInsuranceFileCnt:=uctPMUPolicyControl1.InsuranceFileCnt, v_lInsuranceFolderCnt:=uctPMUPolicyControl1.InsuranceFolderCnt, v_lPartyCnt:=uctPMUPolicyControl1.PartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to cancel MTA", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                UNLOCKPOLICY()
                UnLockSingleInstalmentPlan()
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                'Developer Guide No. 231
                Me.Hide()
            End If

        End If


    End Sub

    ''' <summary>
    ''' cmdOK_Click
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            'Check that Account for client exists if Not Create a Account
            CheckandCreateClientAccount()

            m_bIsSingleInstalmentPlan = uctPMUPolicyControl1.IsSingleInstalmentPlan

            m_lProductId = uctPMUPolicyControl1.ProductId
            m_vLeadAgentCnt = uctPMUPolicyControl1.LeadAgentCnt

            m_lReturn = LockSingleInstalmentPlan()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Process the OK in the control
            m_lReturn = uctPMUPolicyControl1.OKClick()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If
            If uctPMUPolicyControl1.IsExit Then
                Exit Sub
            End If

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Get the insurance file and folder count
                m_lInsuranceFileCnt = uctPMUPolicyControl1.InsuranceFileCnt
                m_lInsuranceFolderCnt = uctPMUPolicyControl1.InsuranceFolderCnt

                m_lBusinessTypeId = uctPMUPolicyControl1.BusinessTypeId
                m_sInsuranceRef = uctPMUPolicyControl1.InsuranceRef

                m_lPolicySourceID = uctPMUPolicyControl1.SourceId
                m_bIsSubAgentAdded = uctPMUPolicyControl1.IsSubAgentAdded


                ' when transaction type is new business and policy status is not "current" then
                ' user must have selected either 'Cancelled' or 'Lapsed' so
                ' we stop the road map by setting m_lStatus = PMCancel
                '
                If m_sTransactionType = "NB" Then

                    If Not (Convert.IsDBNull(uctPMUPolicyControl1.InsuranceFileStatusID) Or
                         IsNothing(uctPMUPolicyControl1.InsuranceFileStatusID)) Then
                        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                    End If

                    If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then _
                        '- Only Lock policy in Add mode , IN other case it would be locked earlier from iPMUListPolicy
                        m_lReturn = LockPolicy()
                    End If
                End If

                ' Everything OK, so we can hide the interface.
                Me.Hide()
            ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMMandatoryMissing Then
                'Something is missing from the Policy form which means we cannot continue
                'Eg. Underwriting year is not available.
                'Pass on a cancel to stop things now
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Me.Hide()
            End If

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass,
                                vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Exit Sub

        End Try
    End Sub

    Private Sub cmdPolicyTax_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPolicyTax.Click
        Try

            m_lReturn = uctPMUPolicyControl1.GetPolicyTaxDetail()
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get Tax detail.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyTaxDetail", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

        Catch excep As System.Exception



            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get Tax Detail.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyTaxDetail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try
    End Sub

    Private Sub cmdReInsurance_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdReInsurance.Click
        Try

            m_lReturn = uctPMUPolicyControl1.GetRiskReinsurance()
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get RiskReinsurance.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskReinsurance", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

        Catch excep As System.Exception



            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get RiskReinsurance.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskReinsurance", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try
    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'TN20010419 Start
            If Len(m_sTransactionType) >= 3 Then
                If m_sTransactionType.Trim().Substring(0, 3) <> "MTA" AndAlso m_sTransactionType.Trim().Substring(0, 3) <> "MTC" Then
                    cmdMTACancel.Visible = False
                End If
                If m_sTransactionType.Trim().Substring(0, 3) = "MTR" Then
                    'Reverted Sumit Kumar changes as per suggestion regarding PM048960(Cancel MTA button should be visible during policy reinstatement.)
                    cmdMTACancel.Visible = True
                End If

                'TN20010419 Start

            ElseIf Len(m_sTransactionType) = 2 Then
                If m_sTransactionType.Trim().Substring(0, 2) = "NB" Then
                    If m_lInsuranceFileCnt <> 0 Then
                        'Kevin Renshaw (CMG) - Donot give option if policy already lapsed
                        If Not m_bPolicyLapsed Then
                            cmdLapseQuote.Top = cmdMTACancel.Top
                            cmdLapseQuote.Left = cmdMTACancel.Left
                            cmdLapseQuote.Visible = True
                        End If
                    End If
                End If
            End If
            'Set all buttons visible false
            'Added By Upendra.
            If m_sTransactionType.Trim().Substring(0, 2) = "NB" Then
                cmdMTACancel.Visible = False
            End If

            cmdFee.Visible = False
            cmdCommission.Visible = False
            cmdPolicyTax.Visible = False
            cmdReInsurance.Visible = False
            cmdInstalment.Visible = False

            'Set all buttons visible true except instalment
            If m_iTask <> gPMConstants.PMEComponentAction.PMAdd Then
                cmdFee.Visible = True
                cmdCommission.Visible = True
                cmdPolicyTax.Visible = True
                cmdReInsurance.Visible = False
            End If

            If (m_iTask = PMEComponentAction.PMView AndAlso (m_sTransactionType = "PT" OrElse m_sTransactionType = "DRI")) Then
                cmdFee.Enabled = False
                cmdCommission.Enabled = False
                cmdPolicyTax.Enabled = False
                cmdReInsurance.Enabled = False
                cmdMTACancel.Enabled = False
            End If


            With uctPMUPolicyControl1
                .Task = m_iTask
                ' Party Cnt
                .PartyCnt = m_lPartyCnt
                'DC 07/06/00
                'To Update Policy
                .InsuranceFileCnt = m_lInsuranceFileCnt
                .InsuranceFolderCnt = m_lInsuranceFolderCnt
                ' CTAF 300800 - Source ID

                .SourceId = m_iSourceID
                .ProductId = m_lProductId
                .IsRenewal = m_bIsRenewal
                .SetQuoteToLapsed = m_bPolicyLapsed
                'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
                .BackDatedMTAsAllowed = m_bBackDatedMTAsAllowed
                .SelectedPolicyStatus = m_sSelectedPolicyStatus
                .IsMTATemp = CStr(m_bIsMTATemp)
                .Renewaldate = m_dtRenewaldate
                .IsPriorDate = m_bIsPriorDate
                .IsRenewed = m_bIsRenewed
                .LapsedDate = m_dtLapsedDate
                'End  (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
            End With
            'TN20010126 Start
            m_lReturn = uctPMUPolicyControl1.SetProcessModes(vTransactionType:=m_sTransactionType)
            'TN20010126 End
            'Developer Guide No.9
            m_lReturn = uctPMUPolicyControl1.Initialise()
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the user control.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If
            If uctPMUPolicyControl1.HasInstalment(m_lInsuranceFileCnt) = gPMConstants.PMEReturnCode.PMTrue Then
                cmdInstalment.Visible = True
            End If

            m_lReturn = uctPMUPolicyControl1.LoadControl()
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the user control.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            m_lReturn = uctPMUPolicyControl1.GetPolicy()
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the policy.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            m_bIsSingleInstalmentPlan = uctPMUPolicyControl1.IsSingleInstalmentPlan


            m_vLeadAgentCnt = uctPMUPolicyControl1.LeadAgentCnt
            'Start (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1)
            m_bIsTrueMonthlypolicyandNextInstalmentRenewal = uctPMUPolicyControl1.IsTrueMonthlypolicyandNextInstalmentRenewal
            'End (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1)


            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Form_Load failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel

                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = uctPMUPolicyControl1.CancelClick()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Cancel = 1
                    'Developer Guide No. 7
                    eventArgs.Cancel = True
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If

                'Unlock Current Policy 'PN35753 --RC
                If m_sTransactionType = gSIRLibrary.SIRProcessCodeMTA Or m_sTransactionType = "NB" Or m_sTransactionType = "MTC" Or m_sTransactionType = "MTR" Then
                    UNLOCKPOLICY()
                    UnLockSingleInstalmentPlan()
                End If
            End If


            ' Terminate the control
            uctPMUPolicyControl1.Dispose()
            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            ' Error Section.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    'UPGRADE_NOTE: (7001) The following declaration (uctPMUPolicyControl1_BusinessTypeChange) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    Private Sub uctPMUPolicyControl1_BusinessTypeChange(ByVal Sender As Object, ByVal e As PMUPolicyControl.uctPMUPolicyControl.BusinessTypeChangeEventArgs) Handles uctPMUPolicyControl1.BusinessTypeChange
        'Commission button disabled for Direct Business
        If Not (m_iTask = PMEComponentAction.PMView AndAlso (m_sTransactionType = "PT" OrElse m_sTransactionType = "DRI")) Then
            cmdCommission.Enabled = Not (e.BusinessType = 1 And Not uctPMUPolicyControl1.IsSubAgentAdded)
            m_lBusinessTypeId = e.BusinessType
        End If


    End Sub

    Private Sub uctPMUPolicyControl1_SubAgentChange(ByVal Sender As Object, ByVal e As EventArgs) Handles uctPMUPolicyControl1.SubAgentChange
        If Not (m_iTask = PMEComponentAction.PMView AndAlso (m_sTransactionType = "PT" OrElse m_sTransactionType = "DRI")) Then
            cmdCommission.Enabled = Not (m_lBusinessTypeId = 1 And Not uctPMUPolicyControl1.IsSubAgentAdded)
        End If

    End Sub

    'PN 27351

    Public Sub CheckandCreateClientAccount()

        Dim oSIROrionUpdate As bSIROrionUpdate.Business

        Dim m_oParty As bSIRParty.Business
        Dim bExists As Boolean

        Dim temp_m_oParty As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_m_oParty, "bSIRParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        m_oParty = temp_m_oParty
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to get an instance of the business object.
            'Initialise = PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSIRParty", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Exit Sub
        End If

        m_lReturn = m_oParty.CheckIfPartyAccountExists(m_lPartyCnt, bExists)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to get an instance of the business object.
            '        Initialise = PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Execute CheckIfPartyAccountExists", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Exit Sub
        End If
        If Not bExists Then


            Dim temp_oSIROrionUpdate As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oSIROrionUpdate, "bSIROrionUpdate.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oSIROrionUpdate = temp_oSIROrionUpdate

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise bSIROrionUpdate.Form.", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateOrion", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Exit Sub
            End If


            m_lReturn = oSIROrionUpdate.SiriusToOrion(v_lPartyCnt:=m_lPartyCnt)

        End If


    End Sub

    ' ***************************************************************** '
    ' Name: UNLOCKPOLICY
    '
    ' Description: UnLock Policy  'PN35753 --RC
    '
    ' History:
    '           Created : Rajesh Choudhary : Date : 18 Jul 2007
    '' ***************************************************************** '
    Private Function UNLOCKPOLICY() As Integer
        Dim result As Integer = 0


        Dim oPMLock As bPMLock.User

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '   Find the Business Class
            Dim temp_oPMLock As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMLock = temp_oPMLock

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oPMLock.UnLockKey("insurance_folder_cnt", vKeyValue:=m_lInsuranceFolderCnt, iUserID:=g_oObjectManager.UserID)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMLogError1, sMsg:="Error trying to unlock the policy", vApp:="iPMUStats", vClass:="Intreface", vMethod:="UNLOCKPOLICY", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result

            End If

            'Terminate the business object

            oPMLock.Dispose()
            oPMLock = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMLogError1, sMsg:="UNLOCKPOLICY Failed", vApp:="iPMUStats", vClass:="Intreface", vMethod:="UNLOCKPOLICY", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function LockPolicy() As Integer
        Dim result As Integer = 0

        Dim sLockedBy As String = ""

        Dim oPMLock As bPMLock.User

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '   Find the Business Class
            Dim temp_oPMLock As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMLock = temp_oPMLock

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oPMLock.LockKey("insurance_folder_cnt", vKeyValue:=m_lInsuranceFolderCnt, iUserID:=g_oObjectManager.UserID, v_bOtherUserOnly:=False, sCurrentlyLockedBy:=sLockedBy)


            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMTrue
                    'OK

                Case gPMConstants.PMEReturnCode.PMFalse
                    'Locked or error
                    If sLockedBy = "ERROR" Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error trying to lock record", vApp:=ACApp, vClass:=ACClass, vMethod:="LockPolicy", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    Else
                        result = gPMConstants.PMEReturnCode.PMFalse
                        MessageBox.Show("Policy currently locked by " & sLockedBy &
                                        Strings.Chr(13) & Strings.Chr(10) & "Please try later", "Policy Lock")
                        Return result
                    End If


                Case Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lock the policy", vApp:=ACApp, vClass:=ACClass, vMethod:="LockPolicy", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result

            End Select

            'Terminate the business object

            oPMLock.Dispose()
            oPMLock = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LockPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LockPolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Function LockSingleInstalmentPlan() As Integer
        Dim result As Integer = 0

        Dim sLockedBy As String = ""

        Dim oPMLock As bPMLock.User

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '   Find the Business Class
            Dim temp_oPMLock As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMLock = temp_oPMLock

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_bIsSingleInstalmentPlan Then

                m_lReturn = oPMLock.LockKey("Single_Instalment_Plan", vKeyValue:=m_vLeadAgentCnt, iUserID:=g_oObjectManager.UserID, v_bOtherUserOnly:=True, sCurrentlyLockedBy:=sLockedBy, vKey2Value:=m_lProductId)


                Select Case m_lReturn
                    Case gPMConstants.PMEReturnCode.PMTrue
                        'OK

                    Case gPMConstants.PMEReturnCode.PMFalse
                        'Locked or error
                        If sLockedBy = "ERROR" Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error trying to lock record", vApp:=ACApp, vClass:=ACClass, vMethod:="LockSingleInstalmentPlan", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                            Return result
                        Else
                            result = gPMConstants.PMEReturnCode.PMFalse
                            MessageBox.Show("This Agent/Product is currently locked by " & sLockedBy &
                                            Strings.Chr(13) & Strings.Chr(10) & "Please try later", "Plan Lock")
                            Return result
                        End If


                    Case Else
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lock the policy", vApp:=ACApp, vClass:=ACClass, vMethod:="LockSingleInstalmentPlan", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return result

                End Select
            End If
            'Terminate the business object

            oPMLock.Dispose()
            oPMLock = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LockSingleInstalmentPlan Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LockSingleInstalmentPlan", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UnLockSingleInstalmentPlan
    '
    ' Description: UnLock SingleInstalmentPlan
    '' ***************************************************************** '
    Private Function UnLockSingleInstalmentPlan() As Integer
        Dim result As Integer = 0


        Dim oPMLock As bPMLock.User

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '   Find the Business Class
            Dim temp_oPMLock As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMLock = temp_oPMLock

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If m_bIsSingleInstalmentPlan Then

                m_lReturn = oPMLock.UnLockKey("Single_Instalment_Plan", vKeyValue:=m_vLeadAgentCnt, iUserID:=g_oObjectManager.UserID, vKey2Value:=m_lProductId)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to process the interface.
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMLogError1, sMsg:="Error trying to unlock the plan", vApp:="iPMUStats", vClass:="Intreface", vMethod:="UnLockSingleInstalmentPlan", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If
            End If
            'Terminate the business object

            oPMLock.Dispose()
            oPMLock = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMLogError1, sMsg:="UnLockSingleInstalmentPlan Failed", vApp:="iPMUStats", vClass:="Intreface", vMethod:="UnLockSingleInstalmentPlan", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub uctPMUPolicyControl1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uctPMUPolicyControl1.Load

    End Sub

    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293


        If e.Alt And e.KeyCode = Keys.D1 Then
            DirectCast(uctPMUPolicyControl1.Controls("tabMainTab"), TabControl).SelectedIndex = 0
        End If
        If e.Alt And e.KeyCode = Keys.D2 Then
            DirectCast(uctPMUPolicyControl1.Controls("tabMainTab"), TabControl).SelectedIndex = 1
        End If
        If e.Alt And e.KeyCode = Keys.D3 Then
            DirectCast(uctPMUPolicyControl1.Controls("tabMainTab"), TabControl).SelectedIndex = 2
        End If
        If e.Alt And e.KeyCode = Keys.D4 Then
            DirectCast(uctPMUPolicyControl1.Controls("tabMainTab"), TabControl).SelectedIndex = 3
        End If
        If e.Alt And e.KeyCode = Keys.D5 Then
            DirectCast(uctPMUPolicyControl1.Controls("tabMainTab"), TabControl).SelectedIndex = 4
        End If
    End Sub
End Class