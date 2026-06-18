Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Windows.Forms
Imports SharedFiles
Imports System.Data

Partial Public Class frmRenewalProcess
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmRenewalProcess
    '
    ' Date:
    '
    ' Description: Interface for Renewals processing.
    '
    ' ***************************************************************** '

    Public m_ofrmLapseRenewal As frmLapseRenewal

    Public m_ofrmChangeStatus As frmChangeStatus

    Public m_ofrmChangePolicyDetails As frmChangePolicyDetails
    Private Const ACClass As String = "frmRenewalProcess"

    'system option number for generating report in the renewal process
    Private Const ACGenerateRenewalStatusReport As Integer = 1012
    Private Const ACGenerateRenewalAgentList As Integer = 1013
    Private Const ACRenSchedulePrinting As Integer = 1036
    Private Const ACRenCertificatePrinting As Integer = 1037
    Private Const ACRenDebitNotePrinting As Integer = 1038
    Private Const ACCreditControlEnabled As Integer = 5001

    'default renewal lock name
    Private Const ACLockName As String = "Insurance_folder_cnt"

    Private Const ACIconManual As Integer = 1
    Private Const ACIconAccept As Integer = 2
    Private Const ACIconInvite As Integer = 3
    Private Const ACIconWrite As Integer = 4

    Private m_sCallingAppName As String = ""
    Private m_lErrorNumber As Integer
    Private m_lReturn As Integer
    Private m_lStatus As gPMConstants.PMEReturnCode

    Private m_vRenewalPolicy(,) As Object  'list of policies which are in renewal and match selected criteria
    Private m_lFormActivate As gPMConstants.PMEReturnCode

    'filter variables
    Private m_sInsuranceRef As String = ""
    Private m_dRenewalDate As Date
    Private m_lProductID As Integer
    Private m_lBranchID As Integer
    Private m_lRenewalType As Integer  'Acceptance, Amendment, Invite or All
    Private m_lLeadAgentCnt As Integer
    Private m_lAgentcode As Integer
    'store system option value
    Private m_sGenerateReport As String = ""
    Private m_sGenerateAgentList As String = ""  'use when do renewal invite

    Private m_oReportPrint As Object
    Private m_oFindDocTemplate As Object
    Private m_oDocTemplate As Object

    Private m_lRenewalMode As Integer
    Private m_lOriginalRenewalMode As Integer
    Private m_bCanTransferBroker As Boolean

    Private m_sRenSchedulePrinting As String = ""  'option number 1036
    Private m_sRenCertificatePrinting As String = ""  'option number 1037
    Private m_sRenDebitNotePrinting As String = ""  'option number 1038

    ' Last size variables for screen resizing
    Private m_lWidth As Integer
    Private m_lHeight As Integer

    Private m_lPaymentAccountID As Integer
    Private m_iDebitAgainst As Integer
    Private m_vCreditTransactions As Object
    Private m_lCashListID As Integer
    Private m_lCashListItemID As Integer
    Private m_lTransactionID As Integer
    Private m_cTransactionAmount As Decimal
    Private m_lWrittenUsed As Long
    ' Tech  Written Status.doc
    Private m_bIsAmendedPolicyWritten As Boolean
    Dim m_iPolicyMakeLiveStatus As Integer
    Dim m_lPrepayment As Object
    Private m_lCount As Integer

    Private m_crRoundOffAmount As Decimal = 0
    Private m_bRoundOff As Boolean = False
    Private m_bProcessWthAmend As Boolean = False
    Private m_bCancelEvent As Boolean = False

    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property

    Public Property InsuranceRef() As String
        Get
            Return m_sInsuranceRef
        End Get
        Set(ByVal Value As String)
            m_sInsuranceRef = Value
        End Set
    End Property

    Public Property RenewalDate() As Date
        Get
            Return m_dRenewalDate
        End Get
        Set(ByVal Value As Date)
            m_dRenewalDate = Value
        End Set
    End Property

    Public Property ProductID() As Integer
        Get
            Return m_lProductID
        End Get
        Set(ByVal Value As Integer)
            m_lProductID = Value
        End Set
    End Property

    Public Property BranchID() As Integer
        Get
            Return m_lBranchID
        End Get
        Set(ByVal Value As Integer)
            m_lBranchID = Value
        End Set
    End Property

    Public Property RenewalType() As Integer
        Get
            Return m_lRenewalType
        End Get
        Set(ByVal Value As Integer)
            m_lRenewalType = Value
        End Set
    End Property

    Public WriteOnly Property RenewalMode() As Integer
        Set(ByVal Value As Integer)
            m_lRenewalMode = Value
        End Set
    End Property

    Private Sub cmdAccept_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAccept.Click
        cmdAccept_Mode(v_bLocked:=False)
        DisplayListViewCount()
    End Sub

    Private Sub cmdAmend_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAmend.Click
        'Note: this is GJW mode - there will only be one record
        ' after amendment go directly to renewal invite and change status to awaiting update regardless of renewal invite status

        Dim lIndex As Integer
        Dim sLockedBy, sFailureMessage As String
        Dim bLocked As Boolean
        Dim vListViewUpdate(0, 1) As Object

        Dim sRenStatusDesc As String = ""
        Dim lRenStatusTypeID As Integer

        Try

            If lvwRenewalProcess.Items.Count = 0 Then
                Exit Sub
            End If

            If Not lvwRenewalProcess.Items.Item(0).Checked Then
                MessageBox.Show("Warning! Please select an item from the list", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            'get array position

            lIndex = Convert.ToString(lvwRenewalProcess.Items.Item(0).Tag)

            If gPMFunctions.ToSafeLong(CStr(m_vRenewalPolicy(ACIRenewalStatusTypeId, lIndex)), 0) = gPMConstants.PMBRenewalStatusTypeAwaitBrokerTransfer Then
                MessageBox.Show("Agent/Broker for this policy " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, lIndex)) & " is in transfer mode." & Strings.Chr(13) & Strings.Chr(10) &
                                "Please contact the System Administrator.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            Me.stbMain.Items.Item("Message").Text = "Locking policy please wait"
            Me.stbMain.Refresh()
            'lock this renewal status count to stop others from processing it

            m_lReturn = g_oBusiness.LockKey(v_sKeyName:=ACLockName, v_lKeyValue:=CInt(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)),
                                            v_lUserID:=g_oObjectManager.UserID, r_sLockedBy:=sLockedBy)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                bLocked = True

                Me.stbMain.Items.Item("Message").Text = "Amending policy please wait"
                Me.stbMain.Refresh()
                m_lReturn = ProcessAmendment(v_vPolicy:=m_vRenewalPolicy, v_lIndex:=lIndex, r_sFailureMessage:=sFailureMessage)
                If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                    Exit Sub
                End If
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Or gPMConstants.PMEReturnCode.PMNotFound Then

                    Me.stbMain.Items.Item("Message").Text = "Updating listview with new data please wait"
                    Me.stbMain.Refresh()
                    m_lReturn = RePopulatePolicy(1, lIndex)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show(sFailureMessage, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit Sub
                    End If

                    Me.stbMain.Items.Item("Message").Text = "Ready"
                    Me.stbMain.Refresh()

                    'did we come from renewal invites task?
                    If m_lRenewalMode = ACRenModeRI Then
                        If MessageBox.Show("Generate Renewal Invite?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then

                            Me.stbMain.Items.Item("Message").Text = "Preparing report table policy please wait"
                            Me.stbMain.Refresh()
                            'delete last print run for this user

                            m_lReturn = g_oBusiness.DeleteLastPrintRun(v_lUserID:=g_oObjectManager.UserID, v_lRenewalStatusCnt:=0)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                MessageBox.Show("Warning! Failed to delete last print run" & Strings.Chr(13) & Strings.Chr(10) & "Please contact support", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Exit Sub
                            End If

                            Me.stbMain.Items.Item("Message").Text = "Generating reports please wait"
                            Me.stbMain.Refresh()
                            'don't care if this failed - GJW specific
                            m_lReturn = ProduceNoticePrint(v_lRenewalStatusCnt:=CInt(m_vRenewalPolicy(ACIRenewalStatusCnt, lIndex)), v_lRenewalInsuranceFileCnt:=CInt(m_vRenewalPolicy(ACIRenewalPolicyCnt, lIndex)), v_lInsuranceFolderCnt:=CInt(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)), v_lPartyCnt:=CInt(m_vRenewalPolicy(ACIRenewalInsuranceHolder, lIndex)), v_lProcessType:=ACDocTypeNoticePrint, v_lMode:=ACSpoolSilentMode, v_sSpoolDesc:="Renewal Invite", v_lRenewalStatusTypeID:=0, v_lIsInvitePrinted:=1, r_sFailureMessage:=sFailureMessage)
                        End If

                        Me.stbMain.Items.Item("Message").Text = "Getting renewal status details please wait"
                        Me.stbMain.Refresh()
                        'get renewal status type details for (awaiting update)
                        m_lReturn = GetRenewalStatusType(v_sRenStatusCode:="Update", r_sDesc:=sRenStatusDesc, r_lRenewalStatusTypeID:=lRenStatusTypeID)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Exit Sub
                        End If

                        Me.stbMain.Items.Item("Message").Text = "Updating renewal status please wait"
                        Me.stbMain.Refresh()
                        'change renewal status

                        If g_oBusiness.SetRenewalStatusTypeID(v_lRenewalInsuranceFileCnt:=CInt(m_vRenewalPolicy(ACIRenewalPolicyCnt, lIndex)), v_lRenewalStatusTypeID:=lRenStatusTypeID) = gPMConstants.PMEReturnCode.PMTrue Then

                            Me.stbMain.Items.Item("Message").Text = "Updating listview with new status please wait"
                            Me.stbMain.Refresh()
                            'update listview with new status
                            m_lReturn = SetListViewRenewalStatus(v_lArrayIndex:=lIndex, v_lSelectedIndex:=1, r_sFailureMessage:=sFailureMessage, v_sRenStatusDesc:=sRenStatusDesc, v_lRenStatusTypeID:=lRenStatusTypeID)

                            Me.stbMain.Items.Item("Message").Text = "Ready"
                            Me.stbMain.Refresh()
                        Else
                            MessageBox.Show("Failed to change renewal status", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Exit Sub
                        End If

                        MessageBox.Show("Renewal Invite Completed Successfully", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    ElseIf m_lRenewalMode <> ACRenModeStandard AndAlso m_iPolicyMakeLiveStatus = 1 Then
                        'must have come from renewal acceptance with amendment task
                        cmdAccept_Mode(v_bLocked:=True)
                    End If
                ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                    lvwRenewalProcess.Focus()
                Else
                    lvwRenewalProcess.Focus()
                End If
            Else
                If sLockedBy = "ERROR" Then
                    MessageBox.Show("Failed to lock policy for, Insurance Folder count : " &
                                CStr(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)) & Strings.Chr(13) & Strings.Chr(10) &
                                "Process terminate.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    MessageBox.Show("Current policy " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, lIndex)) & " is being locked by " & sLockedBy & Strings.Chr(13) & Strings.Chr(10) & "Process terminate.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If



        Catch ex As Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed Amendment process", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAmend_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
            If bLocked Then
                Me.stbMain.Items.Item("Message").Text = "Unlocking policy please wait"
                Me.stbMain.Refresh()
                If g_oBusiness.UnLockKey(v_sKeyName:=ACLockName, v_lKeyValue:=m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex),
                                            v_lUserID:=g_oObjectManager.UserID) <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to unlock KeyName: " & ACLockName & Strings.Chr(13) & Strings.Chr(10) &
                                "KeyValue: " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)) & Strings.Chr(13) & Strings.Chr(10) &
                                "Process terminate.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If

            Me.stbMain.Items.Item("Message").Text = "Ready"
            Me.stbMain.Refresh()
            DisplayListViewCount()
        End Try
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        Me.Hide()
    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click
        If lvwRenewalProcess.Items.Count > 0 Then
            mnuRenewalProcessDelete_Click(mnuRenewalProcessDelete, New EventArgs())
        End If
    End Sub

    Private Sub cmdLapse_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdLapse.Click
        If lvwRenewalProcess.Items.Count <> 0 Then
            mnuRenewalProcessLapse_Click(mnuRenewalProcessLapse, New EventArgs())
        End If
    End Sub

    Private Sub cmdStatus_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdStatus.Click
        If lvwRenewalProcess.Items.Count <> 0 Then
            mnuRenewalProcessSetStatus_Click(mnuRenewalProcessSetStatus, New EventArgs())
        End If
    End Sub

    Private Sub frmRenewalProcess_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            If m_lFormActivate <> gPMConstants.PMEReturnCode.PMTrue Then

                m_lFormActivate = gPMConstants.PMEReturnCode.PMTrue

                Select Case m_lRenewalMode
                    Case ACRenModeRA
                        Me.Text = "Renewal Acceptance without Amendment"
                    Case ACRenModeRAA
                        Me.Text = "Renewal Acceptance with Amendment"
                    Case ACRenModeRI
                        Me.Text = "Renewal Invite"
                    Case ACRenModeStandard
                        Me.Text = "Renewal Process"
                End Select

                'hide and show controls according to renewal mode
                ' show amend button for acceptance with amendment
                ' show accept button for acceptance without amendment
                mnuRenewalProcess.Available = (m_lRenewalMode = ACRenModeStandard)
                mnuRenewalProcessTransfer.Enabled = (m_lRenewalMode = ACRenModeStandard And m_bCanTransferBroker)

                cmdAmend.Visible = (m_lRenewalMode <> ACRenModeRA And m_lRenewalMode <> ACRenModeStandard)
                cmdAccept.Visible = (m_lRenewalMode = ACRenModeRA And m_lRenewalMode <> ACRenModeStandard)
                cmdLapse.Visible = (m_lRenewalMode <> ACRenModeStandard)
                cmdDelete.Visible = (m_lRenewalMode <> ACRenModeStandard)
                cmdStatus.Visible = (m_lRenewalMode <> ACRenModeStandard)

                If cmdAccept.Visible And Not cmdAmend.Visible Then
                    cmdAccept.Left = cmdAmend.Left
                End If

                ShowFilterCriteria()

            End If

        End If
    End Sub

    Private Sub Form_Initialize_Renamed()

        Try
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            iPMFunc.ShowFormInTaskBar_Attach()

            m_lFormActivate = gPMConstants.PMEReturnCode.PMFalse
            m_lStatus = gPMConstants.PMEReturnCode.PMTrue

            If CreateRequireObject() <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to create instances of required objects", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                m_lStatus = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            'do we need to generate status report in renewal?
            If iPMFunc.GetSystemOption(v_iOptionNumber:=ACGenerateRenewalStatusReport, r_sOptionValue:=m_sGenerateReport) <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to get (Generate Renewal Status Report) system option", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                m_lStatus = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            'do we need to generate agent list when process renewal invite?
            If iPMFunc.GetSystemOption(v_iOptionNumber:=ACGenerateRenewalAgentList, r_sOptionValue:=m_sGenerateAgentList) <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to get (Generate Renewal Agent List) system option", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                m_lStatus = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            'do we need to print schedule when accepting renewal?
            If iPMFunc.GetSystemOption(v_iOptionNumber:=ACRenSchedulePrinting, r_sOptionValue:=m_sRenSchedulePrinting) <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to get option number 1036 - schedule printing", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                m_lStatus = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            'do we need to print certificate when accepting renewal?
            If iPMFunc.GetSystemOption(v_iOptionNumber:=ACRenCertificatePrinting, r_sOptionValue:=m_sRenCertificatePrinting) <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to get option number 1037 - certificate printing", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                m_lStatus = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            'do we need to print debit note when accepting renewal?
            If iPMFunc.GetSystemOption(v_iOptionNumber:=ACRenDebitNotePrinting, r_sOptionValue:=m_sRenDebitNotePrinting) <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to get option number 1038 - debit note printing", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                m_lStatus = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            'can this user do broker transfer?
            If GetBrokerTransferAuthority() <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to get broker transfer authority for this user", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                m_lStatus = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        Catch ex As Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise frmRenewalProcess", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

        End Try
        Exit Sub
    End Sub

    Private Sub frmRenewalProcess_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        iPMFunc.ShowFormInTaskBar_Detach()
        m_lOriginalRenewalMode = m_lRenewalMode
        lvwRenewalProcess.FullRowSelect = True
        stbMain.Width = Me.Width
        Message.Width = Me.Width * (85 / 100)
        Counter.Width = Me.Width * (15 / 100)

    End Sub

    Private Sub frmRenewalProcess_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        CloseRequireObject()

        eventArgs.Cancel = Cancel <> 0
    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmRenewalProcess_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        ' Enforce minimum sizes
        If Me.WindowState = FormWindowState.Normal Then
            If VB6.PixelsToTwipsX(Width) < 12765 Then Width = VB6.TwipsToPixelsX(12765)
            If VB6.PixelsToTwipsY(Height) < 8400 Then Height = VB6.TwipsToPixelsY(8400)
        End If

        If Me.WindowState <> FormWindowState.Minimized Then
            ' Resize the screen
            uctAnchor.Resize_Renamed(m_lWidth, m_lHeight, CInt(VB6.PixelsToTwipsX(ClientRectangle.Width)), CInt(VB6.PixelsToTwipsY(ClientRectangle.Height)))

            ' Store last sizes
            m_lWidth = CInt(VB6.PixelsToTwipsX(ClientRectangle.Width))
            m_lHeight = CInt(VB6.PixelsToTwipsY(ClientRectangle.Height))
        End If

    End Sub

    Private Sub lvwRenewalProcess_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwRenewalProcess.ColumnClick


        Try

            m_bCancelEvent = True
            ListViewFunc.SortListView(lvwRenewalProcess, eventArgs)
            m_bCancelEvent = False

        Catch excep As System.Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwRenewalProcess_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        Finally
            m_bCancelEvent = False
        End Try

    End Sub
    'Fixed:5264 Defect Mountain (Pure 3.1) Priya

    Private Sub lvwRenewalProcess_ItemChecked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvwRenewalProcess.ItemChecked
        If m_bCancelEvent = False Then
            RemoveHandler lvwRenewalProcess.ItemChecked, AddressOf Me.lvwRenewalProcess_ItemChecked
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            Dim Item As ListViewItem = lvwRenewalProcess.Items(e.Item.Index)
            Dim bSelectedRow As Boolean
            Dim bCheck As Boolean = Item.Checked
            Dim lCurrentRow As Integer = Item.Index + 1

            For lCount As Integer = 1 To lvwRenewalProcess.Items.Count
                If Not IsNothing(lvwRenewalProcess.FocusedItem) Then
                    If (lvwRenewalProcess.Items.Item(lCount - 1).Selected) Then
                        If (lCurrentRow = lvwRenewalProcess.Items.Item(lCount - 1).Index) Then
                            bSelectedRow = True
                            Exit For
                        End If
                    End If
                End If
            Next

            If bSelectedRow Then
                RemoveHandler lvwRenewalProcess.ItemChecked, AddressOf lvwRenewalProcess_ItemChecked
                For lCount As Integer = 1 To lvwRenewalProcess.Items.Count
                    If lvwRenewalProcess.Items.Item(lCount - 1).Selected Then
                        lvwRenewalProcess.Items.Item(lCount - 1).Checked = bCheck
                    End If
                Next lCount
                AddHandler lvwRenewalProcess.ItemChecked, AddressOf lvwRenewalProcess_ItemChecked
            End If

            DisplayListViewCount()
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            AddHandler lvwRenewalProcess.ItemChecked, AddressOf Me.lvwRenewalProcess_ItemChecked
        End If
    End Sub

    'Private Sub lvwRenewalProcess_ItemChecked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvwRenewalProcess.ItemChecked

    'iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
    'If (e.Item.Checked And e.Item.Selected) Then
    '    e.Item.Checked = True
    'End If
    'DisplayListViewCount()
    'iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

    'Dim Item As ListViewItem = lvwRenewalProcess.Items(e.Item.Index)
    'Dim bSelectedRow As Boolean
    'Dim vSelectedArray As Object

    'iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

    'ReDim vSelectedArray(0)

    'Dim bCheck As Boolean = Item.Checked
    'Dim lCurrentRow As Integer = Item.Index + 1

    'Dim lListCount As Integer = lvwRenewalProcess.Items.Count + 1 'added 1

    ''store selected rows in arrays
    ''For lCount As Integer = 1 To lListCount
    ''    If lvwRenewalProcess.Items.Item(lCount - 1).Selected Then

    ''        ReDim Preserve vSelectedArray(vSelectedArray.GetUpperBound(0) + 1)

    ''        vSelectedArray(vSelectedArray.GetUpperBound(0)) = lCount
    ''    End If
    ''Next lCount

    ''did we check one of the selected rows?

    'Dim lSelectedCount As Integer = vSelectedArray.GetUpperBound(0)
    'For lCount As Integer = 1 To lSelectedCount

    '    If lCurrentRow = CDbl(vSelectedArray(lCount)) Then
    '        bSelectedRow = True
    '        Exit For
    '    End If

    'Next lCount

    ''apply to all selected rowss
    'If bSelectedRow Then
    '    For lCount As Integer = 1 To lListCount
    '        If lvwRenewalProcess.Items.Item(lCount - 1).Selected Then
    '            lvwRenewalProcess.Items.Item(lCount - 1).Checked = bCheck
    '        End If
    '    Next lCount
    'End If

    'DisplayListViewCount()

    'iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

    'End Sub

    Private Sub lvwRenewalProcess_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwRenewalProcess.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000

        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        If m_lRenewalMode = ACRenModeStandard Then
            If lvwRenewalProcess.Items.Count <> 0 Then
                'If Button = MouseButtonConstants.RightButton Then
                If eventArgs.Button = MouseButtons.Right Then
                    Ctx_mnuRenewalProcess.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
                End If
            End If
        End If
    End Sub

    Public Sub mnuRenewalProcessAccept_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuRenewalProcessAccept.Click

        Dim sNewPolicyRef As String = ""
        Dim dNewStartDate, dNewEndDate As Date
        Dim bChanged As Boolean
        Dim bContinue As Boolean
        Dim lIsQuoted As gPMConstants.PMEReturnCode
        Dim sLockedBy As String = ""
        Dim bLocked As Boolean
        Dim lIndex, lListCount As Integer
        Dim sFailureMessage, sOptionValue As String
        Dim sReportText As New StringBuilder
        Dim sRenStatusDesc As String = ""
        Dim lRenStatusTypeID As Integer
        Dim sMsgBox As String = ""
        Dim lYesNo As DialogResult
        Dim lNumberTick, lInvalidTMPCount As Integer
        Dim bIsCashDeposit, bIsCashDepositCancel As Boolean
        'End - Prakash - WPR85_Paralleling
        Try
            m_lCount = 0
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'disable menu
            mnuRenewalProcess.Enabled = False

            'check to see if we have any ticked
            m_lReturn = ListViewIsTick(lvwRenewalProcess, lNumberTick)

            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMError
                    MessageBox.Show("Warning! An error has occurred whilst trying to check for selected items in list", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                Case gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Warning! Please select an item from the list", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
            End Select

            'Start - Prakash - WPR85_Paralleling
            m_lReturn = CheckForCashDepositPaymentMethod(bIsCashDeposit)
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                If bIsCashDeposit Then
                    MessageBox.Show("Batch renewal is not supported for Cash Deposit." &
                                    " Choose Another Batch.", "Renewals", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
            Else
                MessageBox.Show("Failed to  check for CashDeposit PaymentMethod", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            'End - Prakash - WPR85_Paralleling

            stbMain.Items.Item("Message").Text = "Processing renewal acceptance please wait..."
            Me.stbMain.Refresh()
            lListCount = lvwRenewalProcess.Items.Count
            'step backwards so we can remove processed items from list
            For lCount As Integer = lListCount To 1 Step -1
                If lvwRenewalProcess.Items.Item(lCount - 1).Checked Then
                    'Start - Prakash - WPR85_Paralleling
                    bIsCashDepositCancel = False
                    'End - Prakash - WPR85_Paralleling
                    bLocked = False
                    bContinue = False
                    sMsgBox = ""
                    lYesNo = System.Windows.Forms.DialogResult.No

                    'get array position

                    lIndex = Convert.ToString(lvwRenewalProcess.Items.Item(lCount - 1).Tag)
                    m_lCount = lCount
                    'check to see if current renewal status is ok for acceptance
                    m_lReturn = CheckRenewalStatus(lIndex, "ACCEPT", sMsgBox, lYesNo, lNumberTick)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("CheckRenewalStatus() error", ACApp, MessageBoxButtons.OK)
                        Exit For
                    Else
                        If sMsgBox <> "" Then
                            If lYesNo = System.Windows.Forms.DialogResult.Yes Then
                                If MessageBox.Show(sMsgBox, ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                                    Exit For
                                End If
                            Else
                                MessageBox.Show(sMsgBox, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                            End If
                        Else
                            bContinue = True
                        End If
                    End If

                    If bContinue Then
                        'lock this renewal status count to stop others from processing it

                        m_lReturn = g_oBusiness.LockKey(v_sKeyName:=ACLockName, v_lKeyValue:=m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex), v_lUserID:=g_oObjectManager.UserID, r_sLockedBy:=sLockedBy)

                        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                            bLocked = True

                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                            m_lReturn = ChangePolicyDetail(v_vPolicy:=m_vRenewalPolicy, v_lIndex:=lIndex, r_sNewPolicyRef:=sNewPolicyRef, r_dNewStartDate:=dNewStartDate, r_dNewExpiryDate:=dNewEndDate, r_bchanged:=bChanged)

                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                                    If MessageBox.Show("Cancel was selected in Change Policy screen." & Strings.Chr(13) & Strings.Chr(10) & "Do you want to continue with next selected policy?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                                        Exit For
                                    End If
                                Else
                                    If MessageBox.Show("Failed to change policy details." & Strings.Chr(13) & Strings.Chr(10) & "Do you want to continue with next selected policy?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                                        Exit For
                                    End If
                                End If
                            Else

                                If g_oBusiness.IsQuoted(v_lInsuranceFileCnt:=CInt(m_vRenewalPolicy(ACIRenewalPolicyCnt, lIndex)), r_lResult:=lIsQuoted) = gPMConstants.PMEReturnCode.PMTrue Then
                                    If lIsQuoted = gPMConstants.PMEReturnCode.PMTrue Then
                                        m_lReturn = ProcessAccept(v_vPolicy:=m_vRenewalPolicy, v_lIndex:=lIndex, v_bPolicyChanged:=bChanged, v_sNewPolicyNumber:=sNewPolicyRef, v_dNewStartDate:=dNewStartDate, v_dNewEndDate:=dNewEndDate, r_sFailureMessage:=sFailureMessage)

                                        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                            'did we fail in producing any of the document or creating work task
                                            sReportText.Append(Strings.Chr(13) & Strings.Chr(10) & CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, lIndex)) & " - Successful" & Strings.Chr(13) & Strings.Chr(10) & sFailureMessage & Strings.Chr(13) & Strings.Chr(10))
                                        Else
                                            'Start - Prakash - WPR85_Paralleling
                                            If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then bIsCashDepositCancel = True
                                            'End - Prakash - WPR85_Paralleling
                                            sReportText.Append(Strings.Chr(13) & Strings.Chr(10) & CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, lIndex)) & " - " & sFailureMessage & Strings.Chr(13) & Strings.Chr(10))
                                            If sReportText.ToString.Contains("credit") Then
                                                If MessageBox.Show(sReportText.ToString() & Strings.Chr(13) & Strings.Chr(10) & "Do you want to continue with next selected policy?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                                                    Exit For
                                                End If
                                            End If
                                            If (gPMFunctions.ToSafeLong(CStr(m_vRenewalPolicy(ACIRenewalIsTrueMonthlyPolicy, lIndex))) = 1 AndAlso gPMFunctions.ToSafeLong(CStr(m_vRenewalPolicy(ACIRenewalAnniversaryCopy, lIndex))) = 1) Then
                                                lInvalidTMPCount += 1
                                            End If
                                        End If
                                    Else
                                        sReportText.Append(Strings.Chr(13) & Strings.Chr(10) & CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, lIndex)) & " - Unquoted" & Strings.Chr(13) & Strings.Chr(10))

                                        m_lReturn = GetRenewalStatusType(v_sRenStatusCode:="ManReview", r_sDesc:=sRenStatusDesc, r_lRenewalStatusTypeID:=lRenStatusTypeID)

                                        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                            'reset status to awaiting manual review

                                            If g_oBusiness.SetRenewalStatusTypeID(v_lRenewalInsuranceFileCnt:=CInt(m_vRenewalPolicy(ACIRenewalPolicyCnt, lIndex)), v_lRenewalStatusTypeID:=lRenStatusTypeID) <> gPMConstants.PMEReturnCode.PMTrue Then
                                                If MessageBox.Show("Failed to set renewal status to awaiting manual review." & Strings.Chr(13) & Strings.Chr(10) & "Do you want to continue with next selected policy?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                                                    Exit For
                                                End If
                                            End If
                                        Else
                                            If MessageBox.Show("Failed to get renewal status type details." & Strings.Chr(13) & Strings.Chr(10) & "Do you want to continue with next selected policy?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                                                Exit For
                                            End If
                                        End If
                                    End If
                                Else
                                    If MessageBox.Show("Failed to check quote status." & Strings.Chr(13) & Strings.Chr(10) & "Do you want to continue with next selected policy?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                                        Exit For
                                    End If
                                End If
                                'Start - Prakash - WPR85_Paralleling
                                If Not bIsCashDepositCancel Then
                                    'mark this as deleted from listview
                                    m_vRenewalPolicy(ACRenewalDeleteFromListView, lIndex) = "1"

                                    'remove from list to stop user from selecting this again
                                    lvwRenewalProcess.Items.RemoveAt(lCount - 1)
                                End If
                                'End - Prakash - WPR85_Paralleling

                                'unlock renewal policy

                                If g_oBusiness.UnLockKey(v_sKeyName:=ACLockName, v_lKeyValue:=CInt(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)), v_lUserID:=g_oObjectManager.UserID) <> gPMConstants.PMEReturnCode.PMTrue Then
                                    MessageBox.Show("Failed to unlock KeyName: " & ACLockName & Strings.Chr(13) & Strings.Chr(10) & "KeyValue: " & CStr(m_vRenewalPolicy(ACIRenewalStatusCnt, lIndex)) & Strings.Chr(13) & Strings.Chr(10) & "Process terminate.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    Exit For
                                End If

                                bLocked = False 'so we won't try to unlock it later

                            End If 'ChangePolicyDetail
                        Else
                            If sLockedBy = "ERROR" Then
                                If MessageBox.Show("Failed to lock policy for, Insurance Folder count : " &
                                                       CStr(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)) & Strings.Chr(13) & Strings.Chr(10) &
                                                       "Do you want to process next selected policy?", ACApp,
                                                       MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                                    Exit For
                                End If
                            Else
                                If MessageBox.Show("Current policy " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, lIndex)) & " is being locked by " & sLockedBy & Strings.Chr(13) & Strings.Chr(10) & "Do you want to process next selected policy?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                                    Exit For
                                End If
                            End If
                        End If 'LockKey()
                    End If 'CheckRenewalStatus()
                End If 'lvwRenewalProcess.ListItems(lCount).Checked = True
            Next lCount

            If lInvalidTMPCount > 0 Then
                MessageBox.Show(CStr(lInvalidTMPCount) & " anniversary renewal/s could not be processed." &
                                " Anniversary Renewals cannot be accepted until " &
                                " the last monthly cycle has been accepted", "True Monthly Policy Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            If sReportText.ToString() <> "" Then
                If m_sGenerateReport = "1" Then
                    If RenewalReport(v_sReportTitle:="Renewal Acceptance", v_sReportText:=sReportText.ToString()) <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to do Renewal report", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            End If

            lvwRenewalProcess.Refresh()
            lvwRenewalProcess.FullRowSelect = True

        Catch ex As Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process renewal acceptance", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuRenewalProcessAccept_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)


        Finally

            'unlock current policy
            If bLocked Then

                If g_oBusiness.UnLockKey(v_sKeyName:=ACLockName, v_lKeyValue:=CInt(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)), v_lUserID:=g_oObjectManager.UserID) <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to unlock KeyName: " & ACLockName & Strings.Chr(13) & Strings.Chr(10) & "KeyValue: " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)) & Strings.Chr(13) & Strings.Chr(10) & "Process terminate.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If

            lvwRenewalProcess.Refresh()
            stbMain.Items.Item("Message").Text = "Ready"
            Me.stbMain.Refresh()

            DisplayListViewCount()

            mnuRenewalProcess.Enabled = True

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
    End Sub

    Public Sub mnuRenewalProcessAmend_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuRenewalProcessAmend.Click

        Dim bProcessAmendment As Boolean
        Dim sLockedBy As String = ""
        Dim bLocked As Boolean
        Dim lIndex, lListCount As Integer

        Dim sFailureMessage, sOptionValue As String
        Dim sReportText As New StringBuilder

        Dim sMsgBox As String = ""
        Dim lYesNo As DialogResult
        Dim lNumberTick As Integer

        'Start (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx)
        ' If policies has been amended and accepted during this process,
        ' maintain a variable to keep track the true monthly prolicy validation status
        Dim bContinue As Boolean
        Dim lInvalidTMPCount As Integer = 0
        'End (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx)

        Try

            'disable menu
            mnuRenewalProcess.Enabled = False

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'check to see if we have any ticked
            m_lReturn = ListViewIsTick(lvwRenewalProcess, lNumberTick)

            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMError
                    MessageBox.Show("Warning! An error has occurred whilst trying to check for selected items in list", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                Case gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Warning! Please select an item from the list", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
            End Select

            stbMain.Items.Item("Message").Text = "Processing renewal amendment please wait..."
            Me.stbMain.Refresh()
            lListCount = lvwRenewalProcess.Items.Count
            'loop backwards and process checked items and remove from list
            For lCount As Integer = lListCount To 1 Step -1
                If lvwRenewalProcess.Items.Item(lCount - 1).Checked Then
                    bProcessAmendment = False
                    bLocked = False
                    sMsgBox = ""
                    lYesNo = System.Windows.Forms.DialogResult.No

                    'get array position

                    lIndex = Convert.ToString(lvwRenewalProcess.Items.Item(lCount - 1).Tag)
                    lvwRenewalProcess.TopItem = lvwRenewalProcess.Items(lCount - 1)


                    'lock this renewal status count to stop others from processing it

                    m_lReturn = g_oBusiness.LockKey(v_sKeyName:=ACLockName, v_lKeyValue:=CInt(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)), v_lUserID:=g_oObjectManager.UserID, r_sLockedBy:=sLockedBy)

                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        bLocked = True

                        'check to see if current renewal status is ok for amending
                        m_lReturn = CheckRenewalStatus(lIndex, "AMEND", sMsgBox, lYesNo, lNumberTick)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            MessageBox.Show("CheckRenewalStatus() error", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Exit For
                        Else
                            If sMsgBox <> "" Then
                                If lYesNo = System.Windows.Forms.DialogResult.Yes Then
                                    If MessageBox.Show(sMsgBox, ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                                        Exit For
                                    End If
                                Else
                                    MessageBox.Show(sMsgBox, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                                End If
                            Else
                                bProcessAmendment = True
                            End If
                        End If

                        If bProcessAmendment Then
                            If ProcessAmendment(v_vPolicy:=m_vRenewalPolicy, v_lIndex:=lIndex, r_sFailureMessage:=sFailureMessage) = gPMConstants.PMEReturnCode.PMTrue Then

                                sReportText.Append(Strings.Chr(13) & Strings.Chr(10) & CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, lIndex)) & " - Amendment Successful" & Strings.Chr(13) & Strings.Chr(10))

                                m_lReturn = RePopulatePolicy(lCount, lIndex)

                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    sFailureMessage = "Failed to repopulate renewal policy"

                                    'mark this as deleted from listview
                                    m_vRenewalPolicy(ACRenewalDeleteFromListView, lIndex) = "1"

                                    'remove from list to stop user from selecting this again
                                    lvwRenewalProcess.Items.RemoveAt(lCount - 1)

                                    sReportText.Append(" - " & sFailureMessage)
                                    MessageBox.Show(sFailureMessage, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

                                    Exit For
                                End If
                            Else
                                sReportText.Append(Strings.Chr(13) & Strings.Chr(10) & CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, lIndex)) & " - " & sFailureMessage & Strings.Chr(13) & Strings.Chr(10))
                            End If

                            'Start (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx) - (6.1.1.1)
                            ' If the policy is amended before accept, make sure that renewal acceptance is done before unlocking the policy
                            If CDbl(m_vRenewalPolicy(ACIRenewalStatusTypeId, lIndex)) = gPMConstants.PMBRenewalStatusTypeAwaitUpdate And m_iPolicyMakeLiveStatus = MainModule.EPolicyMakeLiveStatus.PolicyMadeLive Then
                                RunRenewalAcceptance(r_sReportText:=sReportText.ToString(), r_lInvalidTMPcount:=lInvalidTMPCount, r_bContinue:=bContinue, lListIndex:=lCount, v_bLocked:=True)

                                'If user has opted not to continue, exit the loop
                                If Not bContinue Then
                                    Exit For
                                End If
                            End If
                            'End (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx) - (6.1.1.1)

                            'unlock current renewal policy

                            If g_oBusiness.UnLockKey(v_sKeyName:=ACLockName, v_lKeyValue:=CInt(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)), v_lUserID:=g_oObjectManager.UserID) <> gPMConstants.PMEReturnCode.PMTrue Then
                                MessageBox.Show("Failed to unlock KeyName: " & ACLockName & Strings.Chr(13) & Strings.Chr(10) & "KeyValue: " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)) & Strings.Chr(13) & Strings.Chr(10) & "Process terminate.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Exit For
                            End If

                            bLocked = False
                        End If
                    Else
                        If sLockedBy = "ERROR" Then
                            If MessageBox.Show("Failed to lock policy for, Insurance Folder Count count : " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)) & Strings.Chr(13) & Strings.Chr(10) & "Do you want to process next selected policy?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                                Exit For
                            End If
                        Else
                            If MessageBox.Show("Current policy " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, lIndex)) & " is being locked by " & sLockedBy & Strings.Chr(13) & Strings.Chr(10) & "Do you want to process next selected policy?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                                Exit For
                            End If
                        End If
                    End If
                End If
            Next lCount

            'Start (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx)
            ' If policies has been amended and accepted during this process,
            ' and if true monthly policy validation on any of those policies fails, display proper errof message
            If lInvalidTMPCount > 0 Then
                MessageBox.Show(CStr(lInvalidTMPCount) & " anniversary renewals could not be processed." &
                                " Anniversary Renewals cannot be accepted until " &
                                " the last monthly cycle has been accepted", "True Monthly Policy Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            'End (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx)

            If sReportText.ToString() <> "" Then
                If m_sGenerateReport = "1" Then
                    If RenewalReport(v_sReportTitle:="Renewal Amendment", v_sReportText:=sReportText.ToString()) <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to do Renewal report", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            End If

            lvwRenewalProcess.Refresh()
            lvwRenewalProcess.FullRowSelect = True

        Catch ex As Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process renewal amendment", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuRenewalProcessAmend_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)


        Finally

            'unlock current renewal policy
            If bLocked Then

                If g_oBusiness.UnLockKey(v_sKeyName:=ACLockName, v_lKeyValue:=m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex), v_lUserID:=g_oObjectManager.UserID) <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to unlock KeyName: " & ACLockName & Strings.Chr(13) & Strings.Chr(10) & "KeyValue: " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)) & Strings.Chr(13) & Strings.Chr(10) & "Process terminate.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If

            lvwRenewalProcess.Refresh()
            lvwRenewalProcess.FullRowSelect = True
            stbMain.Items.Item("Message").Text = "Ready"
            Me.stbMain.Refresh()
            DisplayListViewCount()

            mnuRenewalProcess.Enabled = True

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
    End Sub

    Public Sub mnuRenewalProcessDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuRenewalProcessDelete.Click

        Dim sLockedBy As String = ""
        Dim bLocked As Boolean
        Dim lIndex, lListCount As Integer

        Dim sFailureMessage, sOptionValue As String
        Dim sReportText As New StringBuilder

        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'disable menu
            mnuRenewalProcess.Enabled = False

            'check to see if we have any ticked
            m_lReturn = ListViewIsTick(v_oListView:=lvwRenewalProcess)

            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMError
                    MessageBox.Show("Warning! An error has occurred whilst trying to check for selected items in list", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                Case gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Warning! Please select an item from the list", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
            End Select

            If MessageBox.Show("Are you sure you want to delete selected policies from renewal?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                Exit Sub
            End If

            stbMain.Items.Item("Message").Text = "Deleting selected policies from renewal please wait..."
            Me.stbMain.Refresh()
            lListCount = lvwRenewalProcess.Items.Count
            'loop backwards and process checked items and remove from list
            For lCount As Integer = lListCount To 1 Step -1
                If lvwRenewalProcess.Items.Item(lCount - 1).Checked Then

                    bLocked = False

                    'get array posittion

                    lIndex = Convert.ToString(lvwRenewalProcess.Items.Item(lCount - 1).Tag)

                    Me.stbMain.Items.Item("Message").Text = "Locking policy please wait"
                    Me.stbMain.Refresh()
                    m_lReturn = g_oBusiness.LockKey(v_sKeyName:=ACLockName, v_lKeyValue:=CInt(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)), v_lUserID:=g_oObjectManager.UserID, r_sLockedBy:=sLockedBy)

                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        bLocked = True

                        Me.stbMain.Items.Item("Message").Text = "Deleting renewal please wait"
                        Me.stbMain.Refresh()
                        m_lReturn = g_oBusiness.DeleteRenewal(v_lRenewalInsuranceFileCnt:=CInt(m_vRenewalPolicy(ACIRenewalPolicyCnt, lIndex)), v_lInsuranceFileCnt:=CInt(m_vRenewalPolicy(ACIRenewalLivePolicyCnt, lIndex)), v_lRenewalStatusCnt:=CInt(m_vRenewalPolicy(ACIRenewalStatusCnt, lIndex)), v_lInsuranceFolderCnt:=CInt(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)), v_lPartyCnt:=CInt(m_vRenewalPolicy(ACIRenewalInsuranceHolder, lIndex)), v_sEventDesc:="Delete Renewal - " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, lIndex)), r_sFailureMessage:=sFailureMessage)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            sReportText.Append(Strings.Chr(13) & Strings.Chr(10) & CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, lIndex)) & " - " & sFailureMessage & Strings.Chr(13) & Strings.Chr(10))
                        Else
                            sReportText.Append(Strings.Chr(13) & Strings.Chr(10) & CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, lIndex)) & " - Successful" & Strings.Chr(13) & Strings.Chr(10))
                        End If

                        'mark this as deleted from listview
                        m_vRenewalPolicy(ACRenewalDeleteFromListView, lIndex) = "1"

                        'remove from list (even if it failed to stop user from picking it again)
                        lvwRenewalProcess.Items.RemoveAt(lCount - 1)

                        Me.stbMain.Items.Item("Message").Text = "Unlocking policy please wait"
                        Me.stbMain.Refresh()
                        'unlock record

                        If g_oBusiness.UnLockKey(v_sKeyName:=ACLockName, v_lKeyValue:=CInt(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)), v_lUserID:=g_oObjectManager.UserID) <> gPMConstants.PMEReturnCode.PMTrue Then
                            MessageBox.Show("Failed to unlock KeyName: " & ACLockName & Strings.Chr(13) & Strings.Chr(10) & "KeyValue: " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)) & Strings.Chr(13) & Strings.Chr(10) & "Process terminate.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Exit For
                        End If

                        bLocked = False
                    Else
                        If sLockedBy = "ERROR" Then
                            If MessageBox.Show("Failed to lock policy for, Insurance Folder count : " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)) & Strings.Chr(13) & Strings.Chr(10) & "Do you want to process next selected policy?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                                Exit For
                            End If
                        Else
                            If MessageBox.Show("Current policy " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, lIndex)) & " is being locked by " & sLockedBy & Strings.Chr(13) & Strings.Chr(10) & "Do you want to process next selected policy?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                                Exit For
                            End If
                        End If
                    End If
                End If
            Next lCount

            If sReportText.ToString() <> "" Then
                If m_sGenerateReport = "1" Then
                    Me.stbMain.Items.Item("Message").Text = "Generating status report please wait"
                    Me.stbMain.Refresh()
                    If RenewalReport(v_sReportTitle:="Renewal Deletion", v_sReportText:=sReportText.ToString()) <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to do Renewal Deletion report", "Renewal Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            End If


        Catch ex As Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete policy from renewal", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuRenewalProcessDelete_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)


        Finally

            If bLocked Then
                Me.stbMain.Items.Item("Message").Text = "Unlocking policy please wait"
                Me.stbMain.Refresh()
                If g_oBusiness.UnLockKey(v_sKeyName:=ACLockName, v_lKeyValue:=CInt(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)), v_lUserID:=g_oObjectManager.UserID) <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to unlock KeyName: " & ACLockName & Strings.Chr(13) & Strings.Chr(10) & "KeyValue: " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)) & Strings.Chr(13) & Strings.Chr(10) & "Process terminate.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If

            lvwRenewalProcess.Refresh()
            lvwRenewalProcess.FullRowSelect = True
            stbMain.Items.Item("Message").Text = "Ready"
            Me.stbMain.Refresh()
            DisplayListViewCount()

            mnuRenewalProcess.Enabled = True

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
    End Sub

    Public Sub mnuRenewalProcessInvite_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuRenewalProcessInvite.Click

        Dim sLockedBy As String = ""
        Dim bLocked As Boolean
        Dim lIndex As Integer
        Dim bContinue As Boolean
        Dim lListCount As Integer
        Dim vPolicyRenewalStatus As Object
        Dim vListViewUpdate(0, 1) As Object
        Dim lIcon As Integer

        Dim sFailureMessage, sOptionValue As String
        Dim sReportText As New StringBuilder

        Dim sMsgBox As String = ""
        Dim lYesNo As DialogResult
        Dim lNumberTick As Integer

        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'disable menu
            mnuRenewalProcess.Enabled = False

            'check to see if we have any ticked
            m_lReturn = ListViewIsTick(lvwRenewalProcess, lNumberTick)

            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMError
                    MessageBox.Show("Warning! An error has occurred whilst trying to check for selected items in list", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                Case gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Warning! Please select an item from the list", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                Case gPMConstants.PMEReturnCode.PMTrue
                    'delete last print run for this user

                    m_lReturn = g_oBusiness.DeleteLastPrintRun(v_lUserID:=g_oObjectManager.UserID, v_lRenewalStatusCnt:=0)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Warning! Failed to delete last print run" & Strings.Chr(13) & Strings.Chr(10) & "Please contact support", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit Sub
                    End If
            End Select

            If MessageBox.Show("Are you sure you want to produce renewal notice for selected policies?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                Exit Sub
            End If

            stbMain.Items.Item("Message").Text = "Processing renewal invite please wait..."
            Me.stbMain.Refresh()
            lListCount = lvwRenewalProcess.Items.Count
            'loop backwards and process checked items and remove from list
            For lCount As Integer = lListCount To 1 Step -1
                If lvwRenewalProcess.Items.Item(lCount - 1).Checked Then

                    bLocked = False
                    bContinue = False
                    sMsgBox = ""
                    lYesNo = System.Windows.Forms.DialogResult.No

                    'get array posittion

                    lIndex = Convert.ToString(lvwRenewalProcess.Items.Item(lCount - 1).Tag)

                    Me.stbMain.Items.Item("Message").Text = "Checking renewal status please wait"
                    Me.stbMain.Refresh()
                    'check to see if current renewal status is ok for notice print
                    m_lReturn = CheckRenewalStatus(lIndex, "INVITE", sMsgBox, lYesNo, lNumberTick)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("CheckRenewalStatus() error", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit For
                    Else
                        If sMsgBox <> "" Then
                            If lYesNo = System.Windows.Forms.DialogResult.Yes Then
                                If MessageBox.Show(sMsgBox, ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                                    Exit For
                                End If
                            Else
                                MessageBox.Show(sMsgBox, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                            End If
                        Else
                            bContinue = True
                        End If
                    End If

                    If bContinue Then
                        Me.stbMain.Items.Item("Message").Text = "Locking policy please wait"
                        Me.stbMain.Refresh()
                        'lock this renewal status count to stop others from processing it

                        m_lReturn = g_oBusiness.LockKey(v_sKeyName:=ACLockName, v_lKeyValue:=CInt(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)), v_lUserID:=g_oObjectManager.UserID, r_sLockedBy:=sLockedBy)

                        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                            bLocked = True

                            Me.stbMain.Items.Item("Message").Text = "Generating reports please wait"
                            Me.stbMain.Refresh()
                            m_lReturn = ProduceNoticePrint(v_lRenewalStatusCnt:=CInt(m_vRenewalPolicy(ACIRenewalStatusCnt, lIndex)), v_lRenewalInsuranceFileCnt:=CInt(m_vRenewalPolicy(ACIRenewalPolicyCnt, lIndex)), v_lInsuranceFolderCnt:=CInt(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)), v_lPartyCnt:=CInt(m_vRenewalPolicy(ACIRenewalInsuranceHolder, lIndex)), v_lProcessType:=ACDocTypeNoticePrint, v_lMode:=ACSpoolSilentMode, v_sSpoolDesc:="Renewal Invite", v_lRenewalStatusTypeID:=5, v_lIsInvitePrinted:=1, r_sFailureMessage:=sFailureMessage)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                sReportText.Append(Strings.Chr(13) & Strings.Chr(10) & CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, lIndex)) & " - " & sFailureMessage & Strings.Chr(13) & Strings.Chr(10))
                            Else
                                sReportText.Append(Strings.Chr(13) & Strings.Chr(10) & CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, lIndex)) & " - Successful" & Strings.Chr(13) & Strings.Chr(10))
                            End If

                            Me.stbMain.Items.Item("Message").Text = "Updating listview with new status please wait"
                            Me.stbMain.Refresh()
                            'get current renewal status and update listview
                            m_lReturn = SetListViewRenewalStatus(v_lArrayIndex:=lIndex, v_lSelectedIndex:=lCount, r_sFailureMessage:=sFailureMessage, v_lRenewalStatusCnt:=CInt(m_vRenewalPolicy(ACIRenewalStatusCnt, lIndex)))

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                'mark this as deleted from listview
                                m_vRenewalPolicy(ACRenewalDeleteFromListView, lIndex) = "1"

                                'remove from list to stop user from selecting this again
                                lvwRenewalProcess.Items.RemoveAt(lCount - 1)

                                sReportText.Append(" - " & sFailureMessage)
                                MessageBox.Show(sFailureMessage, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

                                Exit For
                            End If

                            Me.stbMain.Items.Item("Message").Text = "Unlocking policy please wait"
                            Me.stbMain.Refresh()
                            'unlock record

                            If g_oBusiness.UnLockKey(v_sKeyName:=ACLockName, v_lKeyValue:=CInt(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)), v_lUserID:=g_oObjectManager.UserID) <> gPMConstants.PMEReturnCode.PMTrue Then
                                MessageBox.Show("Failed to unlock KeyName: " & ACLockName & Strings.Chr(13) & Strings.Chr(10) & "KeyValue: " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)) & Strings.Chr(13) & Strings.Chr(10) & "Process terminate.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Exit For
                            End If

                            bLocked = False
                        Else
                            If sLockedBy = "ERROR" Then
                                If MessageBox.Show("Failed to lock policy for, Insurance Folder count : " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)) & Strings.Chr(13) & Strings.Chr(10) & "Do you want to process next selected policy?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                                    Exit For
                                End If
                            Else
                                If MessageBox.Show("Current policy " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, lIndex)) & " is being locked by " & sLockedBy & Strings.Chr(13) & Strings.Chr(10) & "Do you want to process next selected policy?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                                    Exit For
                                End If
                            End If
                        End If 'LockKey()
                    End If 'CheckRenewalStatus()
                End If 'lvwRenewalProcess.ListItems(lCount).Checked = True
            Next lCount

            'produce agent list
            If m_sGenerateAgentList = "1" Then
                Me.stbMain.Items.Item("Message").Text = "Generating Agent List report please wait"
                Me.stbMain.Refresh()
                If RenewalAgentList(v_iUserID:=g_oObjectManager.UserID) <> gPMConstants.PMEReturnCode.PMTrue Then
                    sReportText.Append(Strings.Chr(13) & Strings.Chr(10) & " Failed to produce Renewal Agent List")
                    MessageBox.Show("Failed to produce Renewal Agent List", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

            End If

            If sReportText.ToString() <> "" Then
                If m_sGenerateReport = "1" Then
                    Me.stbMain.Items.Item("Message").Text = "Generating status report please wait"
                    Me.stbMain.Refresh()
                    If RenewalReport(v_sReportTitle:="Renewal Invite", v_sReportText:=sReportText.ToString()) <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to do Renewal Invite report", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            End If


        Catch ex As Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lapse policy", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuRenewalProcessLapse_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)


        Finally

            If bLocked Then
                Me.stbMain.Items.Item("Message").Text = "Unlocking policy please wait"
                Me.stbMain.Refresh()

                If g_oBusiness.UnLockKey(v_sKeyName:=ACLockName, v_lKeyValue:=CInt(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)), v_lUserID:=g_oObjectManager.UserID) <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to unlock KeyName: " & ACLockName & Strings.Chr(13) & Strings.Chr(10) & "KeyValue: " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)) & Strings.Chr(13) & Strings.Chr(10) & "Process terminate.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If

            lvwRenewalProcess.Refresh()
            lvwRenewalProcess.FullRowSelect = True
            stbMain.Items.Item("Message").Text = "Ready"
            Me.stbMain.Refresh()
            DisplayListViewCount()

            mnuRenewalProcess.Enabled = True

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
    End Sub
    ''' <summary>
    ''' mnuRenewalProcessLapse_Click
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Public Function mnuRenewalProcessLapse_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuRenewalProcessLapse.Click

        Dim nResult As Integer = 0
        Dim sLockedBy As String = ""
        Dim bLocked As Boolean = False
        Dim lIndex, lListCount As Integer
        Dim nIndex As Integer = 0
        Dim nListCount As Integer = 0
        Dim sFailureMessage As String = ""
        Dim sOptionValue As String = ""
        Dim dtRenPolDetails As New DataTable
        Dim sReportText As New StringBuilder
        m_ofrmLapseRenewal = New frmLapseRenewal
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'disable menu
            mnuRenewalProcess.Enabled = False

            'check to see if we have any ticked
            nResult = ListViewIsTick(v_oListView:=lvwRenewalProcess)


            Select Case nResult
                Case gPMConstants.PMEReturnCode.PMError
                    MessageBox.Show("Warning! An error has occurred whilst trying to check for selected items in list", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return nResult
                Case gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Warning! Please select an item from the list", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return nResult
            End Select

            If MessageBox.Show("Are you sure you want to Lapse selected policies?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                Return nResult
            End If

            m_ofrmLapseRenewal.ShowDialog()

            If m_ofrmLapseRenewal.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Return nResult
            End If

            stbMain.Items.Item("Message").Text = "Lapsing selected policies please wait..."
            Me.stbMain.Refresh()
            nListCount = lvwRenewalProcess.Items.Count
            'loop backwards and process checked items and remove from list
            For lCount As Integer = nListCount To 1 Step -1
                If lvwRenewalProcess.Items.Item(lCount - 1).Checked Then

                    bLocked = False

                    'get array posittion

                    nIndex = Convert.ToString(lvwRenewalProcess.Items.Item(lCount - 1).Tag)

                    Me.stbMain.Items.Item("Message").Text = "Locking policy please wait"
                    Me.stbMain.Refresh()
                    'lock this renewal status count to stop others from processing it

                    nResult = g_oBusiness.LockKey(v_sKeyName:=ACLockName, v_lKeyValue:=CInt(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)), v_lUserID:=g_oObjectManager.UserID, r_sLockedBy:=sLockedBy)

                    If nResult = gPMConstants.PMEReturnCode.PMTrue Then
                        bLocked = True

                        Me.stbMain.Items.Item("Message").Text = "Lapsing renewal please wait"
                        Me.stbMain.Refresh()
                        'delete policy from renewal and set live version to lapse
                        nResult = g_oBusiness.GetRenewalPolicyDetails(v_lInsuranceFileCnt:=CLng(m_vRenewalPolicy(ACIRenewalPolicyCnt, nIndex)), dtResult:=dtRenPolDetails)
                        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return nResult
                        End If

                        If (dtRenPolDetails IsNot Nothing AndAlso dtRenPolDetails.Rows.Count > 0) Then
                            If MsgBox("Please confirm you wish to lapse this policy. You will need to recapture or re-migrate it if required later.",
                              vbOKCancel, "Lapsed Policy") = vbCancel Then

                                Return nResult
                            End If
                        End If
                        nResult = g_oBusiness.LapseRenewal(v_lRenewalInsuranceFileCnt:=CInt(m_vRenewalPolicy(ACIRenewalPolicyCnt, nIndex)), v_lInsuranceFileCnt:=CInt(m_vRenewalPolicy(ACIRenewalLivePolicyCnt, nIndex)), v_lRenewalStatusCnt:=CInt(m_vRenewalPolicy(ACIRenewalStatusCnt, nIndex)), v_lLapseReasonID:=m_ofrmLapseRenewal.LapseReasonID, v_sLapseReasonDesc:=m_ofrmLapseRenewal.LapseReasonDesc, v_lPartyCnt:=CInt(m_vRenewalPolicy(ACIRenewalInsuranceHolder, nIndex)), v_lInsuranceFolderCnt:=CInt(m_vRenewalPolicy(ACIRenewalInsuranceFolder, nIndex)), r_sFailureMessage:=sFailureMessage)

                        If nResult = gPMConstants.PMEReturnCode.PMTrue Then
                            sReportText.Append(CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, nIndex)) & " - Successfull" & Strings.Chr(13) & Strings.Chr(10))
                            nResult = GenerateDocument(v_lProcessType:=ACDocTypeLapse, v_lMode:=ACSpoolSilentMode, v_lInsuranceFileCnt:=CInt(m_vRenewalPolicy(ACIRenewalLivePolicyCnt, nIndex)), v_lInsuranceFolderCnt:=CInt(m_vRenewalPolicy(ACIRenewalInsuranceFolder, nIndex)), v_lPartyCnt:=CInt(m_vRenewalPolicy(ACIRenewalInsuranceHolder, nIndex)), v_sSpoolDesc:="Lapse Renewal", r_sFailureMessage:="")
                        Else
                            sReportText.Append(CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, nIndex)) & " - " & sFailureMessage & Strings.Chr(13) & Strings.Chr(10))
                        End If

                        'mark this as deleted from listview
                        m_vRenewalPolicy(ACRenewalDeleteFromListView, nIndex) = "1"

                        'remove from list (even if it failed to stop user from picking it again)
                        lvwRenewalProcess.Items.RemoveAt(lCount - 1)

                        Me.stbMain.Items.Item("Message").Text = "Unlocking policy please wait"
                        Me.stbMain.Refresh()
                        'unlock record

                        If g_oBusiness.UnLockKey(v_sKeyName:=ACLockName, v_lKeyValue:=CInt(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)), v_lUserID:=g_oObjectManager.UserID) <> gPMConstants.PMEReturnCode.PMTrue Then
                            MessageBox.Show("Failed to unlock KeyName: " & ACLockName & Strings.Chr(13) & Strings.Chr(10) & "KeyValue: " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)) & Strings.Chr(13) & Strings.Chr(10) & "Process terminate.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Exit For
                        End If

                        bLocked = False

                    Else
                        If sLockedBy = "ERROR" Then
                            If MessageBox.Show("Failed to lock policy for, Insurance Folder count : " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)) & Strings.Chr(13) & Strings.Chr(10) & "Do you want to process next selected policy?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                                Exit For
                            End If
                        Else
                            If MessageBox.Show("Current policy " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, nIndex)) & " is being locked by " & sLockedBy & Strings.Chr(13) & Strings.Chr(10) & "Do you want to process next selected policy?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                                Exit For
                            End If
                        End If
                    End If
                End If
            Next lCount

            If sReportText.ToString() <> "" Then
                If m_sGenerateReport = "1" Then
                    Me.stbMain.Items.Item("Message").Text = "Generating status report please wait"
                    Me.stbMain.Refresh()
                    If RenewalReport(v_sReportTitle:="Renewal Lapse", v_sReportText:=sReportText.ToString()) <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to do Renewal Lapse report", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            End If

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lapse policy", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuRenewalProcessLapse_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return nResult

        Finally

            If bLocked Then
                Me.stbMain.Items.Item("Message").Text = "Unlocking policy please wait"
                Me.stbMain.Refresh()

                If g_oBusiness.UnLockKey(v_sKeyName:=ACLockName, v_lKeyValue:=CInt(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)), v_lUserID:=g_oObjectManager.UserID) <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to unlock KeyName: " & ACLockName & Strings.Chr(13) & Strings.Chr(10) & "KeyValue: " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)) & Strings.Chr(13) & Strings.Chr(10) & "Process terminate.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If

            m_ofrmLapseRenewal = Nothing

            lvwRenewalProcess.Refresh()
            lvwRenewalProcess.FullRowSelect = True
            stbMain.Items.Item("Message").Text = "Ready"
            Me.stbMain.Refresh()
            DisplayListViewCount()

            mnuRenewalProcess.Enabled = True

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
    End Function

    Public Sub mnuRenewalProcessSelectAll_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuRenewalProcessSelectAll.Click
        If lvwRenewalProcess.Items.Count > 0 Then
            CheckUnCheckListView(True)
        End If
    End Sub

    Public Sub mnuRenewalProcessSetStatus_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuRenewalProcessSetStatus.Click

        Dim sLockedBy As String = ""
        Dim bLocked As Boolean
        Dim lIndex As Integer
        Dim vListViewUpdate(0, 1) As Object
        Dim bReDisplay As Boolean
        Dim lListCount, lIcon As Integer

        Dim sFailureMessage, sOptionValue As String
        Dim sReportText As New StringBuilder

        Dim sRenStatusTypeDesc As String = ""
        Dim lRenStatusTypeID, lNumberTick As Integer

        Dim sCreditControlEnabled As String = ""

        m_ofrmChangeStatus = New frmChangeStatus
        Try
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'disable menu
            mnuRenewalProcess.Enabled = False

            'check to see if we have any ticked
            m_lReturn = ListViewIsTick(lvwRenewalProcess, lNumberTick)

            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMError
                    MessageBox.Show("Warning! An error has occurred whilst trying to check for selected items in list", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                Case gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Warning! Please select an item from the list", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
            End Select

            If MessageBox.Show("Are you sure you want to change renewal status of selected policies?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                Exit Sub
            End If

            stbMain.Items.Item("Message").Text = "Please select renewal status"
            Me.stbMain.Refresh()

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            'get renewal status

            m_ofrmChangeStatus.cboRenewalStatusType.FirstItem = ""
            m_ofrmChangeStatus.ShowDialog()

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            If m_ofrmChangeStatus.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            'stored new renewal status type

            sRenStatusTypeDesc = m_ofrmChangeStatus.RenewalStatusType

            lRenStatusTypeID = m_ofrmChangeStatus.RenewalStatusTypeID

            stbMain.Items.Item("Message").Text = "Setting renewal status for selected policies please wait..."
            Me.stbMain.Refresh()
            lListCount = lvwRenewalProcess.Items.Count
            'loop backwards and process checked items and remove from list
            For lCount As Integer = lListCount To 1 Step -1
                If lvwRenewalProcess.Items.Item(lCount - 1).Checked Then

                    bLocked = False

                    'get array posittion

                    lIndex = Convert.ToString(lvwRenewalProcess.Items.Item(lCount - 1).Tag)

                    If gPMFunctions.ToSafeLong(CStr(m_vRenewalPolicy(ACIRenewalStatusTypeId, lIndex)), 0) = gPMConstants.PMBRenewalStatusTypeAwaitBrokerTransfer And Not m_bCanTransferBroker Then
                        If lNumberTick > 1 Then
                            If MessageBox.Show("Agent/Broker for this policy " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, lIndex)).Trim() &
                                               " is in transfer mode." & Strings.Chr(13) & Strings.Chr(10) &
                                               "Please contact the System Administrator." & Strings.Chr(13) & Strings.Chr(10) &
                                               "Do you want to continue with next selected policy?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                                Exit For
                            End If
                        Else
                            MessageBox.Show("Agent/Broker for this policy " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, lIndex)).Trim() &
                                            " is in transfer mode." & Strings.Chr(13) & Strings.Chr(10) &
                                            "Please contact the System Administrator.", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                        End If
                    Else

                        Me.stbMain.Items.Item("Message").Text = "Locking policy please wait"
                        Me.stbMain.Refresh()
                        'lock this renewal status count to stop others from processing it

                        m_lReturn = g_oBusiness.LockKey(v_sKeyName:=ACLockName, v_lKeyValue:=CInt(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)),
                                                            v_lUserID:=g_oObjectManager.UserID, r_sLockedBy:=sLockedBy)

                        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                            bLocked = True

                            'change renewal status

                            If g_oBusiness.SetRenewalStatusTypeID(v_lRenewalInsuranceFileCnt:=CInt(m_vRenewalPolicy(ACIRenewalPolicyCnt, lIndex)), v_lRenewalStatusTypeID:=lRenStatusTypeID) = gPMConstants.PMEReturnCode.PMTrue Then
                                sReportText.Append(CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, lIndex)) & " - Successfull" & Strings.Chr(13) & Strings.Chr(10))

                                Me.stbMain.Items.Item("Message").Text = "Updating listview with new status please wait"
                                Me.stbMain.Refresh()
                                'update listview with new status
                                m_lReturn = SetListViewRenewalStatus(v_lArrayIndex:=lIndex, v_lSelectedIndex:=lCount, r_sFailureMessage:=sFailureMessage, v_sRenStatusDesc:=sRenStatusTypeDesc, v_lRenStatusTypeID:=lRenStatusTypeID)

                                'if we failed to change listview value directly then set flag to redisplay whole list
                                If Not bReDisplay And m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    bReDisplay = True
                                End If

                                m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=ACCreditControlEnabled, r_sOptionValue:=sCreditControlEnabled)

                                If sCreditControlEnabled = "1" Then
                                    'If credit control enabled
                                    If gPMFunctions.ToSafeLong(CStr(m_vRenewalPolicy(ACIRenewalStatusTypeId, lIndex)), 0) = gPMConstants.PMBRenewalStatusTypeAwaitUpdate And lRenStatusTypeID <> gPMConstants.PMBRenewalStatusTypeAwaitUpdate Then

                                        If g_oBusiness.DeleteCreditControlItem(v_lInsuranceFileCnt:=gPMFunctions.ToSafeLong(CStr(m_vRenewalPolicy(ACIRenewalPolicyCnt, lIndex)))) <> gPMConstants.PMEReturnCode.PMTrue Then
                                            sFailureMessage = ""
                                        End If
                                    End If
                                End If

                            Else
                                sFailureMessage = " - Failed to change renewal status"
                                sReportText.Append(CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, lIndex)) & sFailureMessage & Strings.Chr(13) & Strings.Chr(10))
                            End If

                            Me.stbMain.Items.Item("Message").Text = "Unlocking policy please wait"
                            Me.stbMain.Refresh()
                            'unlock record

                            If g_oBusiness.UnLockKey(v_sKeyName:=ACLockName,
                                                         v_lKeyValue:=CInt(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)),
                                                         v_lUserID:=g_oObjectManager.UserID) <> gPMConstants.PMEReturnCode.PMTrue Then
                                MessageBox.Show("Failed to unlock KeyName: " & ACLockName & Strings.Chr(13) & Strings.Chr(10) &
                                                    "KeyValue: " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)) & Strings.Chr(13) & Strings.Chr(10) &
                                                    "Process terminate.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Exit For
                            End If

                            bLocked = False

                        Else
                            If sLockedBy = "ERROR" Then
                                If MessageBox.Show("Failed to lock policy for, Insurance Folder count : " &
                                                       CStr(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)) & Strings.Chr(13) &
                                                       Strings.Chr(10) & "Do you want to process next selected policy?", ACApp,
                                                       MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                                    Exit For
                                End If
                            Else
                                If MessageBox.Show("Current policy " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, lIndex)) & " is being locked by " & sLockedBy & Strings.Chr(13) & Strings.Chr(10) & "Do you want to process next selected policy?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                                    Exit For
                                End If
                            End If
                        End If 'lock
                    End If 'agent transfer
                End If 'selected
            Next lCount

            If sReportText.ToString() <> "" Then
                If m_sGenerateReport = "1" Then
                    Me.stbMain.Items.Item("Message").Text = "Generating status report please wait"
                    Me.stbMain.Refresh()
                    If RenewalReport(v_sReportTitle:="Renewal Change Status", v_sReportText:=sReportText.ToString()) <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to do Renewal Change Status report", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            End If

            If bReDisplay Then
                m_lReturn = BusinessToInterface()
            End If


        Catch ex As Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set renewal status", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuRenewalProcessSetStatus_Click()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)


        Finally

            If bLocked Then
                Me.stbMain.Items.Item("Message").Text = "Unlocking policy please wait"
                Me.stbMain.Refresh()
                If g_oBusiness.UnLockKey(v_sKeyName:=ACLockName, v_lKeyValue:=CInt(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)),
                                         v_lUserID:=g_oObjectManager.UserID) <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to unlock KeyName: " & ACLockName & Strings.Chr(13) & Strings.Chr(10) & "KeyValue: " &
                                    CStr(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)) & Strings.Chr(13) & Strings.Chr(10) & "Process terminate.",
                                    ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If

            m_ofrmChangeStatus = Nothing

            stbMain.Items.Item("Message").Text = "Ready"
            Me.stbMain.Refresh()
            DisplayListViewCount()

            mnuRenewalProcess.Enabled = True

            lvwRenewalProcess.Refresh()
            lvwRenewalProcess.FullRowSelect = True
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
    End Sub

    Public Sub mnuRenewalProcessTransfer_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuRenewalProcessTransfer.Click

        Dim lNumberTick, lTransfer, lIndex As Integer
        Dim bContinue As Boolean
        Dim sMsgBox As String = ""
        Dim lYesNo As DialogResult
        Dim bLocked As Boolean
        Dim sLockedBy As String = ""
        Dim sReportText As New StringBuilder
        Dim vTransferSuc(,) As Object
        Dim lMax As Integer

        Try

            If ListViewIsTick(lvwRenewalProcess, lNumberTick) <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Please select at least one record from the list", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            sReportText = New StringBuilder("")
            lTransfer = 0
            lMax = -1

            stbMain.Items.Item("Message").Text = "Transferring selected " & (IIf(lNumberTick > 1, "policies ", "policy ")) & "please wait"
            Me.stbMain.Refresh()
            For lCount As Integer = 1 To lvwRenewalProcess.Items.Count
                If lvwRenewalProcess.Items.Item(lCount - 1).Checked Then
                    lTransfer += 1
                    bContinue = False
                    lIndex = Convert.ToString(lvwRenewalProcess.Items.Item(lCount - 1).Tag)

                    stbMain.Items.Item("Counter").Text = CStr(lTransfer) & "\" & CStr(lNumberTick)
                    Me.stbMain.Refresh()
                    m_lReturn = CheckRenewalStatus(lIndex, "TRANSFER", sMsgBox, lYesNo, lNumberTick)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("CheckRenewalStatus() error", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit For
                    Else
                        If sMsgBox <> "" Then
                            If lYesNo = System.Windows.Forms.DialogResult.Yes Then
                                If MessageBox.Show(sMsgBox, ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                                    Exit For
                                End If
                            Else
                                MessageBox.Show(sMsgBox, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                            End If
                        Else
                            bContinue = True
                        End If
                    End If

                    If bContinue Then
                        'lock this renewal status count to stop others from processing it

                        m_lReturn = g_oBusiness.LockKey(v_sKeyName:=ACLockName, v_lKeyValue:=m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex), v_lUserID:=g_oObjectManager.UserID, r_sLockedBy:=sLockedBy)

                        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                            bLocked = True

                            If g_oBusiness.TransferBroker(v_lRenewalInsuranceFileCnt:=m_vRenewalPolicy(ACIRenewalPolicyCnt, lIndex), v_lTransferToPartyCnt:=m_vRenewalPolicy(ACIRenewalTransferToPartyCnt, lIndex)) = gPMConstants.PMEReturnCode.PMTrue Then
                                sReportText.Append(CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, lIndex)) & " - Successful" & Strings.Chr(13) & Strings.Chr(10))

                                If Not Information.IsArray(vTransferSuc) Then
                                    ReDim vTransferSuc(1, 0)
                                Else

                                    ReDim Preserve vTransferSuc(1, vTransferSuc.GetUpperBound(1) + 1)
                                End If

                                lMax = vTransferSuc.GetUpperBound(1)

                                vTransferSuc(0, lMax) = lCount

                                vTransferSuc(1, lMax) = lIndex
                            Else
                                sReportText.Append(CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, lIndex)) & " - Failed" & Strings.Chr(13) & Strings.Chr(10))
                            End If

                            'unlock renewal policy

                            If g_oBusiness.UnLockKey(v_sKeyName:=ACLockName, v_lKeyValue:=CInt(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)), v_lUserID:=g_oObjectManager.UserID) <> gPMConstants.PMEReturnCode.PMTrue Then
                                MessageBox.Show("Failed to unlock KeyName: " & ACLockName & Strings.Chr(13) & Strings.Chr(10) & "KeyValue: " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)) & Strings.Chr(13) & Strings.Chr(10) & "Process terminate.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Exit For
                            End If

                            bLocked = False
                        Else
                            If sLockedBy = "ERROR" Then
                                If MessageBox.Show("Failed to lock policy for,Insurance Folder count : " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)) & Strings.Chr(13) & Strings.Chr(10) & "Do you want to process next selected policy?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                                    Exit For
                                End If
                            Else
                                If MessageBox.Show("Current policy " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, lIndex)) & " is being locked by " & sLockedBy & Strings.Chr(13) & Strings.Chr(10) & "Do you want to process next selected policy?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                                    Exit For
                                End If
                            End If
                        End If
                    End If
                End If
            Next

            If lMax <> -1 Then
                stbMain.Items.Item("Message").Text = "Updating listview with new data please wait"
                Me.stbMain.Refresh()
                For lCount As Integer = 0 To lMax

                    If RePopulatePolicy(CInt(vTransferSuc(0, lCount)), CInt(vTransferSuc(1, lCount))) <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to update listview with new data", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit For
                    End If
                Next
            End If

            If sReportText.ToString() <> "" Then
                If m_sGenerateReport = "1" Then
                    stbMain.Items.Item("Message").Text = "Generating status report please wait"
                    Me.stbMain.Refresh()

                    If RenewalReport(v_sReportTitle:="Renewal Transfer", v_sReportText:=sReportText.ToString()) <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to do renewal status report", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            End If

            If lNumberTick > 0 Then
                MessageBox.Show("Successfully transferred " & (IIf(lMax = -1, CStr(0), CStr(lMax + 1))) & "/" & CStr(lNumberTick) & (IIf(lMax > 0, " policies ", " policy ")), ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            lvwRenewalProcess.Refresh()
            lvwRenewalProcess.FullRowSelect = True

        Catch ex As Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to launch listview search", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuRenewalSearchPolicyList_Click()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
            'unlock current policy
            If bLocked Then

                If g_oBusiness.UnLockKey(v_sKeyName:=ACLockName, v_lKeyValue:=CInt(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)), v_lUserID:=g_oObjectManager.UserID) <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to unlock KeyName: " & ACLockName & Strings.Chr(13) & Strings.Chr(10) & "KeyValue: " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)) & Strings.Chr(13) & Strings.Chr(10) & "Process terminate.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If

            stbMain.Items.Item("Message").Text = "Ready"
            Me.stbMain.Refresh()
            DisplayListViewCount()

        End Try
    End Sub

    Public Sub mnuRenewalProcessUnselectAll_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuRenewalProcessUnselectAll.Click
        If lvwRenewalProcess.Items.Count > 0 Then
            CheckUnCheckListView(False)
        End If
    End Sub

    Public Sub mnuRenewalSearchPolicyList_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuRenewalSearchPolicyList.Click
        Dim oSearch As iPMListViewSearch6.Interface_Renamed
        Try

            oSearch = New iPMListViewSearch6.Interface_Renamed()

            If Not Information.IsReference(oSearch) Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create search object", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuRenewalSearchPolicyList_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Exit Sub
            End If

            m_lReturn = oSearch.LvwSearch(oSearchList:=Me.lvwRenewalProcess)
            lvwRenewalProcess.Focus()


        Catch ex As Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to launch listview search", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuRenewalSearchPolicyList_Click()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

            oSearch = Nothing

        End Try
    End Sub

    Public Sub mnuRenewalSearchRenewalPolicies_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuRenewalSearchRenewalPolicies.Click

        ShowFilterCriteria()

    End Sub

    '**********************************************************************************************************
    ' Desc: get all policies in renewal process which match selected criteria
    ' v_lAgentCode Added to Filter By Agent
    '**********************************************************************************************************
    Private Function GetBusiness(ByRef r_vResultArray(,) As Object, ByVal v_lInsuranceFileCnt As Integer, ByVal v_sInsuranceRef As String, ByVal v_dRenewalDate As Date, ByVal v_lProductID As Integer, ByVal v_lBranchID As Integer, ByVal v_lRenewalType As Integer, ByVal v_lLeadAGentCnt As Integer, Optional ByVal v_lAgentCode As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            stbMain.Items.Item("Message").Text = "Selecting renewal please wait..."
            Me.stbMain.Refresh()

            result = g_oBusiness.GetRenewalPolicy(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_sInsuranceRef:=v_sInsuranceRef, v_dRenewalDate:=v_dRenewalDate, v_lProductID:=v_lProductID, v_lBranchID:=v_lBranchID, v_lRenewalType:=v_lRenewalType, v_lLeadAGentCnt:=v_lLeadAGentCnt, v_lAgentCode:=v_lAgentCode, r_vResult:=r_vResultArray)



        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get policies in renewal", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    '**********************************************************************************************************
    ' Desc: display policies in renewal process which match selected criteria
    '**********************************************************************************************************
    Private Function BusinessToInterface() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Dim lIcon, lListCount As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            lvwRenewalProcess.Items.Clear()
            lvwRenewalProcess.Refresh()
            lvwRenewalProcess.FullRowSelect = True
            If Not Information.IsArray(m_vRenewalPolicy) Then
                result = gPMConstants.PMEReturnCode.PMNotFound

                If m_sInsuranceRef <> "" Then
                    MessageBox.Show("Policy Number not found - please try again.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("No policies found - Please try again.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

                Return result
            End If

            stbMain.Items.Item("Message").Text = "Populating renewal list please wait..."
            Me.stbMain.Refresh()

            lListCount = m_vRenewalPolicy.GetUpperBound(1)
            For lCount As Integer = 0 To lListCount
                If CStr(m_vRenewalPolicy(ACRenewalDeleteFromListView, lCount)) <> "1" Then

                    'work out which icon to use
                    Select Case m_vRenewalPolicy(ACIRenewalStatusTypeId, lCount)
                        Case 1, 3, 4, 6  'manual update/review
                            lIcon = 0
                        Case 5  'await update (acceptance)
                            lIcon = 1
                        Case 2  'notice print
                            lIcon = 2
                        Case 7  'broker/agent transfer
                            lIcon = 3
                            'Start Written Status
                        Case PMBRenewalStatusTypeWrittenAwaitUpdate
                            lIcon = 5
                            'End   Written Status
                    End Select

                    'Col 1 branch

                    oListItem = lvwRenewalProcess.Items.Add(CStr(m_vRenewalPolicy(ACIRenewalSourceCode, lCount)).Trim(), lIcon)

                    ListViewHelper.SetListItemSmallIconProperty(oListItem, lIcon)
                    'col 2 client
                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vRenewalPolicy(ACIRenewalShortname, lCount)).Trim()

                    'col 3 client name
                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vRenewalPolicy(ACIRenewalResolvedName, lCount)).Trim()

                    'col 3 policy no
                    ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, lCount)).Trim()

                    'col 4 agent
                    ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vRenewalPolicy(ACIRenewalLeadAgentCode, lCount)).Trim()

                    'col 5 account handler
                    ListViewHelper.GetListViewSubItem(oListItem, 5).Text = CStr(m_vRenewalPolicy(ACIRenewalLeadAgentDescription, lCount)).Trim()

                    'col 5 account handler
                    ListViewHelper.GetListViewSubItem(oListItem, 6).Text = CStr(m_vRenewalPolicy(ACIRenewalAccHandlerCode, lCount)).Trim()

                    'col 6 renewal date (of the live policy not the renewal version)
                    ListViewHelper.GetListViewSubItem(oListItem, 7).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, CStr(m_vRenewalPolicy(ACIRenewalCoverStartDate, lCount)).Trim())

                    'col 7 status
                    ListViewHelper.GetListViewSubItem(oListItem, 8).Text = CStr(m_vRenewalPolicy(ACIRenewalStatusType, lCount)).Trim()

                    'col 8 product
                    ListViewHelper.GetListViewSubItem(oListItem, 9).Text = CStr(m_vRenewalPolicy(ACIRenewalProduct, lCount)).Trim()

                    'col 9 claim indicator
                    ListViewHelper.GetListViewSubItem(oListItem, 10).Text = CStr(m_vRenewalPolicy(ACIRenewalClaimsIndicator, lCount)).Trim()

                    'col 10 transfer broker to
                    If gPMFunctions.ToSafeLong(CStr(m_vRenewalPolicy(ACIRenewalStatusTypeId, lCount)), 0) = gPMConstants.PMBRenewalStatusTypeAwaitBrokerTransfer Then
                        If gPMFunctions.ToSafeLong(CStr(m_vRenewalPolicy(ACIRenewalTransferToPartyCnt, lCount)), 0) = 0 Then
                            ListViewHelper.GetListViewSubItem(oListItem, 11).Text = "Direct"
                        Else
                            ListViewHelper.GetListViewSubItem(oListItem, 11).Text = CStr(m_vRenewalPolicy(ACIRenewalTransferToPartyShortName, lCount)).Trim()
                        End If
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, 11).Text = ""
                    End If

                    oListItem.Tag = CStr(lCount)
                End If
            Next



        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display data to interface", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

            If lvwRenewalProcess.Items.Count = 0 Then
                mnuRenewalProcess.Enabled = False
                mnuRenewalSearchPolicyList.Enabled = False
            Else
                mnuRenewalProcess.Enabled = True
                mnuRenewalSearchPolicyList.Enabled = True

                'checked record if we come from one of GJW's tasks
                If m_lOriginalRenewalMode <> ACRenModeStandard Then
                    lvwRenewalProcess.Items.Item(0).Checked = True
                End If
            End If

            lvwRenewalProcess.Refresh()
            lvwRenewalProcess.FullRowSelect = True

            stbMain.Items.Item("Message").Text = "Ready"
            Me.stbMain.Refresh()

            DisplayListViewCount()



        End Try
        Return result
    End Function

    '****************************************************************************
    'Desc: check to see if selected record is eligible for Amendment/Acceptance
    ' return PMTrue if this record is ok for Amendment/Acceptance, PMFalse otherwise
    'Also checks that the policy is not still against a closed branch - user MUST
    'amend this to be against an open branch before it is accepted
    '****************************************************************************
    Private Function CheckRenewalStatus(ByVal v_lIndex As Integer, ByVal v_sStatusType As String, ByRef r_sMessage As String, ByRef r_lYesNo As Integer, Optional ByVal v_lNumberTick As Integer = 0) As Integer

        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            r_lYesNo = System.Windows.Forms.DialogResult.No
            r_sMessage = ""

            If Not Information.IsArray(m_vRenewalPolicy) Then
                r_sMessage = "Policy list is empty"
                Return result
            End If

            If v_sStatusType.ToUpper() <> "TRANSFER" Then
                If gPMFunctions.ToSafeLong(CStr(m_vRenewalPolicy(ACIRenewalStatusTypeId, v_lIndex)), 0) = gPMConstants.PMBRenewalStatusTypeAwaitBrokerTransfer Then
                    r_sMessage = "Agent/Broker for this policy is in transfer mode." & Strings.Chr(13) & Strings.Chr(10) & "Please contact the System Administrator."
                Else
                    Select Case v_sStatusType.ToUpper()
                        Case "AMEND"
                            'Start (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx) - (6.1.1.1)
                            ' Removing the Restrictions here to let the policies in PMBRenewalStatusTypeAwaitUpdate to proceed for Amendment
                            If CDbl(m_vRenewalPolicy(ACIRenewalStatusTypeId, v_lIndex)) = gPMConstants.PMBRenewalStatusTypePolicyChanged Then
                                'Or m_vRenewalPolicy(ACIRenewalStatusTypeId, v_lIndex) = PMBRenewalStatusTypeAwaitUpdate Then

                                r_sMessage = "Renewal status of selected policy is set for acceptance."
                            End If
                            'End (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx) - (6.1.1.1)
                        Case "ACCEPT"
                            'First check is this policy belongs to a Closed Branch - if it
                            'does then stop the accept and advise the user to amend this first
                            If gPMFunctions.ToSafeLong(CStr(m_vRenewalPolicy(ACIRenewalIsBranchDeleted, v_lIndex)), 0) = 1 Then
                                r_sMessage = "Unable to proceed - this Policy is attached to a branch that is closed. Please Amend " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, v_lIndex))
                                'Here check that the status type isn't a failure type.
                                'PN74111 : Priya
                            ElseIf CDbl(m_vRenewalPolicy(ACIRenewalStatusTypeId, v_lIndex)) = gPMConstants.PMBRenewalStatusTypeAwaitManualRating Or CDbl(m_vRenewalPolicy(ACIRenewalStatusTypeId, v_lIndex)) = gPMConstants.PMBRenewalStatusTypeAutoRatedFailed Or CDbl(m_vRenewalPolicy(ACIRenewalStatusTypeId, v_lIndex)) = gPMConstants.PMBRenewalStatusTypeManualReview Or CDbl(m_vRenewalPolicy(ACIRenewalStatusTypeId, v_lIndex)) = gPMConstants.PMBRenewalStatusTypeAutoRated Then

                                r_sMessage = "Renewal status of selected policy is not set for acceptance."
                                'PN62891 - Only able to accept policies via the 'Renewal Acceptance without Amendment' task that have a status of 'Awaiting Update'
                            ElseIf m_vRenewalPolicy(ACIRenewalStatusTypeId, v_lIndex) <> PMBRenewalStatusTypeAwaitUpdate _
                            And m_lRenewalMode = ACRenModeRA Then
                                r_sMessage = "Renewal status of selected policy is not set for acceptance."

                            End If
                            'Start  Written Status
                        Case "WRITE"
                            ' Check if the product allows write status
                            m_lReturn = g_oBusiness.IsWrittenUsed(ToSafeInteger(m_vRenewalPolicy(ACIRenewalProductId, v_lIndex)))
                            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                                'Sankar - PN 71391
                                r_sMessage = "Warning: Product Risk Maintenance option not set for renewal record(s) selected. " &
                                                 "Please re-select"
                            ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                If ToSafeLong(m_vRenewalPolicy(ACIRenewalIsBranchDeleted, v_lIndex), 0) = 1 Then
                                    r_sMessage = "Unable to proceed - this Policy is attached to a branch that is closed. Please Amend " _
                                                & m_vRenewalPolicy(ACIRenewalInsuranceRef, v_lIndex)
                                ElseIf m_vRenewalPolicy(ACIRenewalStatusTypeId, v_lIndex) = PMBRenewalStatusTypeAwaitManualRating _
                                    Or m_vRenewalPolicy(ACIRenewalStatusTypeId, v_lIndex) = PMBRenewalStatusTypeAutoRatedFailed _
                                    Or m_vRenewalPolicy(ACIRenewalStatusTypeId, v_lIndex) = PMBRenewalStatusTypeManualReview _
                                    Or m_vRenewalPolicy(ACIRenewalStatusTypeId, v_lIndex) = PMBRenewalStatusTypeAwaitBrokerTransfer _
                                    Or m_vRenewalPolicy(ACIRenewalStatusTypeId, v_lIndex) = PMBRenewalStatusTypeAutoRated _
                                    Or m_vRenewalPolicy(ACIRenewalStatusTypeId, v_lIndex) = PMBRenewalStatusTypePolicyChanged Then
                                    'Sankar - PN 71392
                                    r_sMessage = "Written Status is only available for renewal records with a status of " & """Awaiting Update"""
                                End If
                            Else
                                RaiseError("CheckRenewalStatus", "IsWrittenUsed Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                            'End  Written Status
                        Case "INVITE"
                            If CDbl(m_vRenewalPolicy(ACIRenewalStatusTypeId, v_lIndex)) <> gPMConstants.PMBRenewalStatusTypeAutoRated Then
                                r_sMessage = "Renewal status of policy (" & Strings.Chr(13) & Strings.Chr(10) &
                                             CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, v_lIndex)) & ") " & Strings.Chr(13) & Strings.Chr(10) &
                                             " is not set for notice print."
                            End If
                    End Select
                End If
            Else
                If gPMFunctions.ToSafeLong(CStr(m_vRenewalPolicy(ACIRenewalStatusTypeId, v_lIndex)), 0) <> gPMConstants.PMBRenewalStatusTypeAwaitBrokerTransfer Then
                    r_sMessage = "Renewal status on selected policy is not set to transfer."
                End If
            End If

            If r_sMessage <> "" Then
                If v_lNumberTick > 1 Then
                    r_sMessage = r_sMessage & Strings.Chr(13) & Strings.Chr(10) & "Do you want to continue processing next policy?"
                    r_lYesNo = System.Windows.Forms.DialogResult.Yes
                End If
            End If



        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check renewal status", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckRenewalStatus()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    '*******************************************************************************************************
    'Desc: process renewal amendment for selected policy
    '*******************************************************************************************************

    Private Function ProcessAmendment(ByRef v_vPolicy(,) As Object, ByVal v_lIndex As Object, ByRef r_sFailureMessage As String) As Integer

        Dim result As Integer = 0
        Dim vKeyArray(,) As Object
        Dim vGetKeyArray(,) As Object
        Dim lBusinessTypeID As Integer
        Dim vPlanArray As Object
        Dim sValue As String = ""
        Dim vValue As Byte
        Dim vResults(,) As Object
        Dim sPaymentMethod As String = ""
        Dim lProductID As Integer
        Dim iStatus As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_iPolicyMakeLiveStatus = 0  'Sankar - PN 66941

            'run policy screen
            ReDim vKeyArray(1, 5)

            vKeyArray(0, 0) = "party_cnt"

            vKeyArray(1, 0) = v_vPolicy(ACIRenewalInsuranceHolder, v_lIndex)

            vKeyArray(0, 1) = "insurance_file_cnt"

            vKeyArray(1, 1) = v_vPolicy(ACIRenewalPolicyCnt, v_lIndex)

            vKeyArray(0, 2) = "insurance_folder_cnt"

            vKeyArray(1, 2) = v_vPolicy(ACIRenewalInsuranceFolder, v_lIndex)

            vKeyArray(0, 3) = "shortname"

            vKeyArray(1, 3) = v_vPolicy(ACIRenewalShortname, v_lIndex)

            vKeyArray(0, 4) = "Product_id"

            vKeyArray(1, 4) = v_vPolicy(ACIRenewalProductId, v_lIndex)

            vKeyArray(0, 5) = "renewals"

            vKeyArray(1, 5) = True

            result = RunProcess(v_sComponent:="iPMUPolicy.NavigatorV3", v_vKeyArray:=vKeyArray, r_sFailureMessage:=r_sFailureMessage, r_vGetKeyArray:=vGetKeyArray)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            'get back BusinessTypeID
            If Information.IsArray(vGetKeyArray) Then

                For lCount As Integer = 0 To vGetKeyArray.GetUpperBound(1)

                    Select Case vGetKeyArray(0, lCount)
                        Case PMNavKeyConst.PMKeyNameBusinessTypeId

                            lBusinessTypeID = CInt(vGetKeyArray(1, lCount))
                    End Select
                Next lCount
            End If

            'policy details might have been changed...we need to update Renewal Status table with these info

            result = g_oBusiness.UpdateRenewalStatus(v_lRenewalStatusCnt:=CInt(v_vPolicy(ACIRenewalStatusCnt, v_lIndex)), r_sMessage:=r_sFailureMessage)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If



            'run coinsurance
            ReDim vKeyArray(1, 3)

            vKeyArray(0, 0) = "insurance_file_cnt"

            vKeyArray(1, 0) = v_vPolicy(ACIRenewalPolicyCnt, v_lIndex)

            vKeyArray(0, 1) = "insurance_folder_cnt"

            vKeyArray(1, 1) = v_vPolicy(ACIRenewalInsuranceFolder, v_lIndex)

            vKeyArray(0, 2) = "insurance_ref"

            vKeyArray(1, 2) = v_vPolicy(ACIRenewalInsuranceRef, v_lIndex)

            vKeyArray(0, 3) = "business_type_id"

            vKeyArray(1, 3) = lBusinessTypeID

            result = RunProcess(v_sComponent:="iPMUCoinsurance.NavigatorV3", r_sFailureMessage:=r_sFailureMessage, v_vKeyArray:=vKeyArray)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            'run list risks
            'Start (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx) - (6.1.1.1)
            'Increasing the array size to pass the renewal status
            ReDim vKeyArray(1, 4)
            'End (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx) - (6.1.1.1)

            vKeyArray(0, 0) = "insurance_file_cnt"

            vKeyArray(1, 0) = v_vPolicy(ACIRenewalPolicyCnt, v_lIndex)

            vKeyArray(0, 1) = "insurance_folder_cnt"

            vKeyArray(1, 1) = v_vPolicy(ACIRenewalInsuranceFolder, v_lIndex)

            vKeyArray(0, 2) = "shortname"

            vKeyArray(1, 2) = v_vPolicy(ACIRenewalShortname, v_lIndex)
            'Start (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx) - (6.1.1.1)
            'Passing Renewal Status of the policy to list risk

            vKeyArray(0, 3) = PMNavKeyConst.PMKeyNamePolicyRenewalStatus

            vKeyArray(1, 3) = v_vPolicy(ACIRenewalStatusTypeId, v_lIndex)
            'Declaring an array to get back the result from iPMUListRisks

            vKeyArray(0, 4) = PMNavKeyConst.PMKeyNameRenewalProcessMode

            vKeyArray(1, 4) = m_lOriginalRenewalMode

            'set initial status
            iStatus = gPMConstants.PMEReturnCode.PMOK

            Dim vResultArray(,) As Object

            Do While iStatus = gPMConstants.PMEReturnCode.PMOK Or iStatus = gPMConstants.PMEReturnCode.PMNavAction1
                If iStatus = gPMConstants.PMEReturnCode.PMOK Then
                    'run list risks
                    'Start (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx) - (6.1.1.1)
                    'Increasing the array size to pass the renewal status
                    ReDim vKeyArray(1, 5)
                    'End (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx) - (6.1.1.1)

                    vKeyArray(0, 0) = "insurance_file_cnt"

                    vKeyArray(1, 0) = v_vPolicy(ACIRenewalPolicyCnt, v_lIndex)

                    vKeyArray(0, 1) = "insurance_folder_cnt"

                    vKeyArray(1, 1) = v_vPolicy(ACIRenewalInsuranceFolder, v_lIndex)

                    vKeyArray(0, 2) = "shortname"

                    vKeyArray(1, 2) = v_vPolicy(ACIRenewalShortname, v_lIndex)
                    'Start (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx) - (6.1.1.1)
                    'Passing Renewal Status of the policy to list risk

                    vKeyArray(0, 3) = PMNavKeyConst.PMKeyNamePolicyRenewalStatus

                    vKeyArray(1, 3) = v_vPolicy(ACIRenewalStatusTypeId, v_lIndex)
                    'Declaring an array to get back the result from iPMUListRisks

                    vKeyArray(0, 4) = PMNavKeyConst.PMKeyNameRenewalProcessMode

                    vKeyArray(1, 4) = m_lOriginalRenewalMode

                    vKeyArray(0, 5) = "processwithamend"
                    vKeyArray(1, 5) = m_bProcessWthAmend
                    vResultArray = Nothing

                    'Prakash - Passing the vResultArray to Run process to get back the policy make live status from ListRisks
                    result = RunProcess(v_sComponent:="iPMUListRisks.NavigatorV3", r_sFailureMessage:=r_sFailureMessage, v_vKeyArray:=vKeyArray, r_vGetKeyArray:=vResultArray, r_iStatus:=iStatus)
                    'End (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx) - (6.1.1.1)

                    If result <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return result
                    End If

                    v_vPolicy(ACIPaymentMethod, v_lIndex) = vResultArray(1, 1)

                    If iStatus = gPMConstants.PMEReturnCode.PMOK Then
                        Exit Do
                    End If
                Else
                    'run policy screen
                    ReDim vKeyArray(1, 5)
                    vKeyArray(0, 0) = "party_cnt"
                    vKeyArray(1, 0) = v_vPolicy(ACIRenewalInsuranceHolder, v_lIndex)
                    vKeyArray(0, 1) = "insurance_file_cnt"
                    vKeyArray(1, 1) = v_vPolicy(ACIRenewalPolicyCnt, v_lIndex)
                    vKeyArray(0, 2) = "insurance_folder_cnt"
                    vKeyArray(1, 2) = v_vPolicy(ACIRenewalInsuranceFolder, v_lIndex)
                    vKeyArray(0, 3) = "shortname"
                    vKeyArray(1, 3) = v_vPolicy(ACIRenewalShortname, v_lIndex)
                    vKeyArray(0, 4) = "Product_id"
                    vKeyArray(1, 4) = v_vPolicy(ACIRenewalProductId, v_lIndex)
                    vKeyArray(0, 5) = "renewals"
                    vKeyArray(1, 5) = True

                    ProcessAmendment = RunProcess(v_sComponent:="iPMUPolicy.NavigatorV3", v_vKeyArray:=vKeyArray, r_sFailureMessage:=r_sFailureMessage, r_vGetKeyArray:=vGetKeyArray, r_iStatus:=iStatus)
                    If ProcessAmendment <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return result
                    End If

                    Dim lCount As Integer
                    'get back BusinessTypeID
                    If (IsArray(vGetKeyArray)) Then
                        For lCount = 0 To UBound(vGetKeyArray, 2)

                            Select Case vGetKeyArray(0, lCount)
                                Case PMNavKeyConst.PMKeyNameBusinessTypeId
                                    lBusinessTypeID = vGetKeyArray(1, lCount)
                            End Select
                        Next lCount
                    End If

                    'policy details might have been changed...we need to update Renewal Status table with these info
                    ProcessAmendment = g_oBusiness.UpdateRenewalStatus(v_lRenewalStatusCnt:=CLng(v_vPolicy(ACIRenewalStatusCnt, v_lIndex)), r_sMessage:=r_sFailureMessage)

                    If ProcessAmendment <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return result
                    End If
                End If
            Loop
            'Start (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx) - (6.1.1.1)
            'Get the policy make live status

            If Information.IsArray(vGetKeyArray) Then

                For iLoop As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                    If CStr(vResultArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop)) = PMNavKeyConst.PMKeyNamePolicyMakeLiveStaus Then

                        m_iPolicyMakeLiveStatus = gPMFunctions.ToSafeInteger(CStr(vResultArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop)), 0)
                    End If
                Next iLoop
            End If
            'End (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx) - (6.1.1.1)

            'm_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnablePayNowOptions, v_vBranch:=g_iSourceID, r_vUnderwriting:=sValue)
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    GoTo Finally_Renamed
            'End If
            lProductID = gPMFunctions.ToSafeLong(CStr(v_vPolicy(ACIRenewalProductId, v_lIndex)), 0)

            m_lReturn = g_oBusiness.GetPrePaymentOptionValue(lProductID, m_lPrepayment)

            m_lReturn = g_oBusiness.GetPaymentMethod(v_lInsuranceFileCnt:=m_vRenewalPolicy(ACIRenewalPolicyCnt, 0), r_vResults:=vResults)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            If Information.IsArray(vResults) Then

                sPaymentMethod = CStr(vResults(0, 0))
            End If

            'Start - Prakash - WPR85_Paralleling
            If m_lRenewalMode = ACRenModeRAA Then
                If sPaymentMethod = "CashDeposit" Then
                    m_lReturn = ProcessCashDeposit()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn
                        stbMain.Items.Item(0).Text = ""
                        Return result
                    End If
                ElseIf m_lPrepayment(0, 0) = "1" AndAlso (LCase(ToSafeString(v_vPolicy(ACIPaymentMethod, v_lIndex))) = "paynow" OrElse LCase(ToSafeString(v_vPolicy(ACIPaymentMethod, v_lIndex))) = "invoice") Then

                    ' ElseIf (vValue = 1 Or sPaymentMethod.ToLower() = "paynow") Then
                    m_lReturn = ShowPayNow(sPaymentMethod)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn
                        stbMain.Items.Item(0).Text = ""
                        Return result
                    End If
                End If
            End If
            'End - Prakash - WPR85_Paralleling
            'run change policy status
            'ProcessAmendment = RunProcess(v_sComponent:="iPMUChangePolicyStatus.NavigatorV3", r_sFailureMessage:=r_sFailureMessage, v_vKeyArray:=vKeyArray)

            'If ProcessAmendment <> PMTrue Then
            '    GoTo Finally
            'End If

            'quote version should have been create at renewal selection, but just in case something went wrong
            'do we have an instalment plan on current policy version

            result = g_oBusiness.IsInstalment(v_lInsuranceFileCnt:=v_vPolicy(ACIRenewalPolicyCnt, v_lIndex))
            If m_iPolicyMakeLiveStatus = 0 Then  'PM033055

                If result = gPMConstants.PMEReturnCode.PMTrue And (sPaymentMethod.ToLower = "instalments" Or sPaymentMethod.ToLower = "direct debit") Then
                    'create quote plan for renewal version ( if its not already there) now that we have plan on current version

                   ' result = g_oBusiness.GetSingleFinancePlanFromInsFileCnt(v_lInsuranceFileCnt:=v_vPolicy(ACIRenewalLivePolicyCnt, v_lIndex), r_vPFPremiumFinance:=vPlanArray)
                    result = g_oBusiness.CreateInstalmentQuote(v_lOriginalInsuranceFileCnt:=v_vPolicy(ACIRenewalLivePolicyCnt, v_lIndex), v_lRenewalInsuranceFileCnt:=CInt(v_vPolicy(ACIRenewalPolicyCnt, v_lIndex)), v_lPartyCnt:=CInt(v_vPolicy(ACIRenewalInsuranceHolder, v_lIndex)), r_vPlanArray:=vPlanArray, r_sFailureMessage:=r_sFailureMessage)
                    
                    If result = gPMConstants.PMEReturnCode.PMTrue Then
                        'get bank details if its missing

                        If CStr(vPlanArray(43, 0)).Trim() = "" Then
                            'launch plan maintenance to get bank details (Transaction button is disable - can only save details)
                            'run agent commission - (use same keyarray() as change policy status)
                            'ReDim vKeyArray(1, 2)
                            ReDim vKeyArray(1, 3)

                            vKeyArray(0, 0) = "FinancePlanCnt"

                            vKeyArray(1, 0) = vPlanArray(0, 0)

                            vKeyArray(0, 1) = "FinancePlanVersion"

                            vKeyArray(1, 1) = vPlanArray(1, 0)

                            vKeyArray(0, 2) = "Spawned"

                            vKeyArray(1, 2) = "True"

                            'PN 74070
                            'UPGRADE_WARNING: (1037) Couldn't resolve default property of object vKeyArray(). More Information: http://www.vbtonet.com/ewis/ewi1037.aspx
                            vKeyArray(0, 3) = "DontDeleteScheme"
                            'UPGRADE_TODO: (1067) Member DontDeleteScheme is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                            'UPGRADE_WARNING: (1037) Couldn't resolve default property of object vKeyArray(). More Information: http://www.vbtonet.com/ewis/ewi1037.aspx
                            vKeyArray(1, 3) = g_oBusiness.DontDeleteScheme
                            result = RunProcess(v_sComponent:="iPMBFinancePlanMaint.Interface_Renamed", r_sFailureMessage:=r_sFailureMessage, v_sTransactionType:="NB", v_vSetProperty:=vKeyArray)

                            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return result
                            End If
                        End If
                    Else
                        MessageBox.Show(r_sFailureMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return result
                    End If

                ElseIf result <> gPMConstants.PMEReturnCode.PMTrue And result <> gPMConstants.PMEReturnCode.PMNotFound Then

                    r_sFailureMessage = "Failed to check for instalment plan for policy ID " & CStr(v_vPolicy(ACIRenewalLivePolicyCnt, v_lIndex))
                    MessageBox.Show(r_sFailureMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return result
                End If
            End If
            'Start  Written Status.doc

            If ToSafeInteger(v_vPolicy(ACIRenewalStatusTypeId, v_lIndex)) = PMBRenewalStatusTypeWrittenAwaitUpdate Then
                m_bIsAmendedPolicyWritten = True
            Else
                m_bIsAmendedPolicyWritten = False
            End If
            'End  Written Status.doc
            'Start (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx) - (6.1.1.1)
            If m_iPolicyMakeLiveStatus = MainModule.EPolicyMakeLiveStatus.PolicyMadeLive Then

                ' No need to change the status if current status is awaiting update

                If CDbl(v_vPolicy(ACIRenewalStatusTypeId, v_lIndex)) <> gPMConstants.PMBRenewalStatusTypeAwaitUpdate Then
                    'Set the renewal status to await notice print only if PolicyMakeLiveStatus is PolicyMadeLive
                    'Niit 22 oct 2012
                    Dim vBindRenewalWithoutInvitation As String = ""

                    m_lReturn = g_oBusiness.GetValueFromTable(v_sTableName:="Product", v_vReturnColumn:="bind_renewal_without_invitation", v_sKeyColumn:="product_id", v_sKeyValue:=v_vPolicy(ACIRenewalProductId, v_lIndex), v_iDataType:=gPMConstants.PMEDataType.PMLong, r_vResult:=vBindRenewalWithoutInvitation)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        MessageBox.Show("Failed to get (bind_renewal_without_invitation) value", "Renewal Amendment Process", MessageBoxButtons.OK)
                        Return result
                    End If
                    If vBindRenewalWithoutInvitation = "1" Then
                        result = g_oBusiness.SetRenewalStatusTypeID(v_lRenewalInsuranceFileCnt:=CInt(v_vPolicy(ACIRenewalPolicyCnt, v_lIndex)), v_lRenewalStatusTypeID:=gPMConstants.PMBRenewalStatusTypeAwaitUpdate)
                    Else
                        'Niit 22 oct 2012 end
                        result = g_oBusiness.SetRenewalStatusTypeID(v_lRenewalInsuranceFileCnt:=CInt(v_vPolicy(ACIRenewalPolicyCnt, v_lIndex)), v_lRenewalStatusTypeID:=gPMConstants.PMBRenewalStatusTypeAutoRated)
                        'Niit 22 oct 2012
                    End If
                    'Niit 22 oct 2012 End
                    If result <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sFailureMessage = "Failed to set renewal status to Awaiting Update"
                        MessageBox.Show(r_sFailureMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return result
                    End If
                Else
                    'If Make live button has been actioned from the risk screen then it will be have 1 for successfull status.
                    If m_lRenewalMode <> ACRenModeRI Then
                        RenewalMode = ACRenModeRAA
                        result = gPMConstants.PMEReturnCode.PMTrue
                    End If
                End If

            ElseIf m_iPolicyMakeLiveStatus = EPolicyMakeLiveStatus.PolicyQuoted Then
                If v_vPolicy(ACIRenewalStatusTypeId, v_lIndex) = PMBRenewalStatusTypeAwaitUpdate Then
                    Dim lIsQuoted As Long
                    If g_oBusiness.IsQuoted(v_lInsuranceFileCnt:=CLng(v_vPolicy(ACIRenewalPolicyCnt, v_lIndex)), r_lResult:=lIsQuoted) = gPMConstants.PMEReturnCode.PMTrue Then
                        If lIsQuoted <> gPMConstants.PMEReturnCode.PMTrue Then
                            ProcessAmendment = g_oBusiness.SetRenewalStatusTypeID(v_lRenewalInsuranceFileCnt:=CLng(v_vPolicy(ACIRenewalPolicyCnt, v_lIndex)), v_lRenewalStatusTypeID:=PMBRenewalStatusTypeManualReview)
                            If ProcessAmendment <> gPMConstants.PMEReturnCode.PMTrue Then
                                r_sFailureMessage = "Failed to set renewal status to Awaiting Manual Review"
                                MessageBox.Show(r_sFailureMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Return result
                            End If
                        End If
                    End If
                End If
            End If
            'End (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx) - (6.1.1.1)



        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            r_sFailureMessage = "Failed to process renewal amendment."

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sFailureMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAmendment()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

        End Try
        Return result
    End Function

    '*******************************************************************************************************
    'Desc: allow user option to change policy number, start and or end dates
    '*******************************************************************************************************

    Private Function ChangePolicyDetail(ByVal v_vPolicy As Object, ByVal v_lIndex As Object, ByRef r_sNewPolicyRef As Object, ByRef r_dNewStartDate As Object, ByRef r_dNewExpiryDate As Object, ByRef r_bchanged As Object) As Integer

        Dim result As Integer = 0
        Dim vResult As String = ""

        m_ofrmChangePolicyDetails = New frmChangePolicyDetails
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            r_bchanged = False

            m_lReturn = g_oBusiness.GetValueFromTable(v_sTableName:="Product", v_vReturnColumn:="hide_summary_at_renewal_acceptance", v_sKeyColumn:="product_id", v_sKeyValue:=v_vPolicy(ACIRenewalProductId, v_lIndex), v_iDataType:=gPMConstants.PMEDataType.PMLong, r_vResult:=vResult)

            If vResult = "1" Then
                Return result
            End If

            'Start Written Status.doc

            If v_vPolicy(ACIRenewalStatusTypeId, v_lIndex) = gPMConstants.PMBRenewalStatusTypeWrittenAwaitUpdate _
                   Or m_bIsAmendedPolicyWritten Then
                Return result
            End If
            'End  Written Status.doc
            r_dNewStartDate = CDate(v_vPolicy(ACIRenewalCoverStartDate, v_lIndex))

            r_dNewExpiryDate = CDate(v_vPolicy(ACIRenewalExpiryDate, v_lIndex))

            With m_ofrmChangePolicyDetails

                .PolicyNumber = CStr(v_vPolicy(ACIRenewalInsuranceRef, v_lIndex))
                .CoverStartDate = r_dNewStartDate
                .CoverExpiryDate = r_dNewExpiryDate

                .ProductID = CInt(v_vPolicy(ACIRenewalProductId, v_lIndex))
                .BusinessType = ACBusinessTypePolicy
                .PartyCnt = ToSafeInteger(v_vPolicy(ACIRenewalInsuranceHolder, v_lIndex))
                .AgentId = 0

                Dim dbNumericTemp As Double
                If Double.TryParse(CStr(v_vPolicy(ACIRenewalAgentCnt, v_lIndex)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                    .AgentId = CInt(v_vPolicy(ACIRenewalAgentCnt, v_lIndex))
                End If

                .BranchID = g_iSourceID
            End With

            m_ofrmChangePolicyDetails.ShowDialog()

            With m_ofrmChangePolicyDetails
                If .Status = gPMConstants.PMEReturnCode.PMOK Then

                    If CStr(v_vPolicy(ACIRenewalInsuranceRef, v_lIndex)).Trim() <> .PolicyNumber Then
                        r_sNewPolicyRef = .PolicyNumber
                        r_bchanged = True
                    End If

                    If DateAndTime.DateDiff("d", r_dNewStartDate, .CoverStartDate, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) <> 0 Then
                        r_dNewStartDate = .CoverStartDate
                        r_bchanged = True
                    End If

                    If DateAndTime.DateDiff("d", r_dNewExpiryDate, .CoverExpiryDate, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) <> 0 Then
                        r_dNewExpiryDate = .CoverExpiryDate
                        r_bchanged = True
                    End If
                Else
                    result = .Status
                End If
            End With



        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process renewal amendment", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAmendment()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

            If Not (m_ofrmChangePolicyDetails Is Nothing) Then

                m_ofrmChangePolicyDetails.Close()

                m_ofrmChangePolicyDetails = Nothing
            End If



        End Try
        Return result
    End Function

    '*******************************************************************************************************
    'Desc: run relevant component with provided keys and get back required keys from component if required
    '*******************************************************************************************************

    Private Function RunProcess(ByVal v_sComponent As String, ByRef r_sFailureMessage As String, Optional ByVal v_vKeyArray(,) As Object = Nothing, Optional ByRef r_vGetKeyArray(,) As Object = Nothing, Optional ByVal v_bDisplayMessage As Boolean = True, Optional ByVal v_lProcessMode As Integer = gPMConstants.PMEComponentAction.PMEdit, Optional ByVal v_sTransactionType As String = "REN", Optional ByVal v_vSetProperty(,) As Object = Nothing, Optional ByRef r_iStatus As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim oObject As Object
        Dim bNavigatorV3 As Boolean

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            'are we using NavigatorV3 or interface class?
            bNavigatorV3 = (v_sComponent.ToUpper().IndexOf(".NAVIGATORV3") >= 0)

            'create an instance of required object
            m_lReturn = g_oObjectManager.GetInstance(oObject:=oObject, sClassName:=v_sComponent, vInstanceManager:=gPMConstants.PMGetLocalInterface)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                r_sFailureMessage = "Failed to instantiate object " & v_sComponent
                If v_bDisplayMessage Then
                    MessageBox.Show(r_sFailureMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
                Return result
            End If

            If bNavigatorV3 Then

                If Not Information.IsNothing(v_vKeyArray) Then
                    'pass in relevant keys

                    If oObject.NavigatorV3_SetKeys(v_vKeyArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        r_sFailureMessage = "Failed to set relevant Keys to " & v_sComponent
                        If v_bDisplayMessage Then
                            MessageBox.Show(r_sFailureMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                        Return result
                    End If
                End If

                'set process mode

                If oObject.NavigatorV3_SetProcessModes(vTask:=v_lProcessMode, vTransactionType:=v_sTransactionType) <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    r_sFailureMessage = "Failed to set process mode to " & v_sComponent
                    If v_bDisplayMessage Then
                        MessageBox.Show(r_sFailureMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                    Return result
                End If

                'set required properties

                If Not Information.IsNothing(v_vSetProperty) Then
                    If Information.IsArray(v_vSetProperty) Then
                        For lCount As Integer = 0 To v_vSetProperty.GetUpperBound(1)
                            Select Case v_vSetProperty(0, lCount)
                                Case "FinancePlanCnt"

                                    oObject.FinancePlanCnt = CInt(v_vSetProperty(1, lCount))
                                Case "FinancePlanVersion"

                                    oObject.FinancePlanVersion = CInt(v_vSetProperty(1, lCount))
                                Case "Spawned"

                                    oObject.Spawned = (CStr(v_vSetProperty(1, lCount)) = "True")
                                Case "DontDeleteScheme"
                                    'UPGRADE_TODO: (1067) Member DontDeleteScheme is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                                    'UPGRADE_WARNING: (1068) v_vSetProperty(1, lCount) of type Variant is being forced to Scalar. More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                                    oObject.DontDeleteScheme = v_vSetProperty(1, lCount)
                            End Select
                        Next lCount
                    End If
                End If

                'start component

                If oObject.NavigatorV3_Start() <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    r_sFailureMessage = "Failed to start" & v_sComponent
                    If v_bDisplayMessage Then
                        MessageBox.Show(r_sFailureMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                    Return result
                End If

                'get keys back if required

                'If Not Information.IsNothing(r_vGetKeyArray) Then
                If Not Information.IsArray(r_vGetKeyArray) Then

                    If oObject.NavigatorV3_GetKeys(vKeyArray:=r_vGetKeyArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        r_sFailureMessage = "Failed to get keys from " & v_sComponent
                        If v_bDisplayMessage Then
                            MessageBox.Show(r_sFailureMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                        Return result
                    End If
                End If

                'did we cancel or error?

                If oObject.NavigatorV3_Status = gPMConstants.PMEReturnCode.PMError Then
                    r_sFailureMessage = "Error in " & v_sComponent
                    result = gPMConstants.PMEReturnCode.PMError
                ElseIf oObject.NavigatorV3_Status = gPMConstants.PMEReturnCode.PMCancel Then
                    r_sFailureMessage = "Process was cancelled"
                    result = gPMConstants.PMEReturnCode.PMCancel
                End If
                r_iStatus = oObject.NavigatorV3_Status
            Else
                'we are using interface class

                If Not Information.IsNothing(v_vKeyArray) Then
                    'pass in relevant keys

                    If oObject.SetKeys(v_vKeyArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        r_sFailureMessage = "Failed to set relevant Keys to " & v_sComponent
                        If v_bDisplayMessage Then
                            MessageBox.Show(r_sFailureMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                        Return result
                    End If
                End If

                'set process mode

                If oObject.SetProcessModes(vTask:=v_lProcessMode, vTransactionType:=v_sTransactionType) <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    r_sFailureMessage = "Failed to set process mode to " & v_sComponent
                    If v_bDisplayMessage Then
                        MessageBox.Show(r_sFailureMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                    Return result
                End If

                'set required properties

                If Not Information.IsNothing(v_vSetProperty) Then
                    If Information.IsArray(v_vSetProperty) Then
                        For lCount As Integer = 0 To v_vSetProperty.GetUpperBound(1)
                            Select Case v_vSetProperty(0, lCount)
                                Case "FinancePlanCnt"

                                    oObject.FinancePlanCnt = CInt(v_vSetProperty(1, lCount))
                                Case "FinancePlanVersion"

                                    oObject.FinancePlanVersion = CInt(v_vSetProperty(1, lCount))
                                Case "Spawned"

                                    oObject.Spawned = (CStr(v_vSetProperty(1, lCount)) = "True")
                                Case "DontDeleteScheme"
                                    'UPGRADE_TODO: (1067) Member DontDeleteScheme is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                                    'UPGRADE_WARNING: (1068) v_vSetProperty(1, lCount) of type Variant is being forced to Scalar. More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                                    oObject.DontDeleteScheme = v_vSetProperty(1, lCount)
                            End Select

                        Next lCount
                    End If
                End If

                'start component

                If oObject.Start() <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    r_sFailureMessage = "Failed to start" & v_sComponent
                    If v_bDisplayMessage Then
                        MessageBox.Show(r_sFailureMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                    Return result
                End If

                'get keys back if required

                If Not Information.IsNothing(r_vGetKeyArray) Then

                    If oObject.GetKeys(vKeyArray:=r_vGetKeyArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        r_sFailureMessage = "Failed to get keys from " & v_sComponent
                        If v_bDisplayMessage Then
                            MessageBox.Show(r_sFailureMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                        Return result
                    End If
                End If

                'did we cancel or error?

                If oObject.Status = gPMConstants.PMEReturnCode.PMError Then
                    r_sFailureMessage = "Error in " & v_sComponent
                    result = gPMConstants.PMEReturnCode.PMError
                ElseIf oObject.Status = gPMConstants.PMEReturnCode.PMCancel Then
                    r_sFailureMessage = "Process was cancelled"
                    result = gPMConstants.PMEReturnCode.PMCancel
                End If


            End If  'NavigatorV3



        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            r_sFailureMessage = "Failed to run component " & v_sComponent

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sFailureMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="RunProcess()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

            If Not (oObject Is Nothing) Then

                oObject.Dispose()
                oObject = Nothing
            End If



        End Try
        Return result
    End Function

    ''' <summary>
    ''' accept policy, reset status of unquoted policy back to awaiting manual review
    ''' </summary>
    ''' <param name="v_vPolicy"></param>
    ''' <param name="v_lIndex"></param>
    ''' <param name="v_bPolicyChanged"></param>
    ''' <param name="v_sNewPolicyNumber"></param>
    ''' <param name="v_dNewStartDate"></param>
    ''' <param name="v_dNewEndDate"></param>
    ''' <param name="r_sFailureMessage"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessAccept(ByVal v_vPolicy As Object, ByVal v_lIndex As Object, ByVal v_bPolicyChanged As Object, ByVal v_sNewPolicyNumber As Object, ByVal v_dNewStartDate As Date, ByVal v_dNewEndDate As Date, ByRef r_sFailureMessage As String) As Integer

        Dim nResult As Integer = 0
        Dim nRenewalPolicyCnt As Integer = 0
        Dim nOldPolicyCnt As Integer = 0
        Dim nRenewalStatusCnt As Integer = 0
        Dim nInsuranceFolder As Integer = 0
        Dim nPartyCnt As Integer = 0
        Dim nAnniversaryCopy As Integer = 0
        Dim bIsTrueMonthlyPolicy As Boolean = False
        Dim oKeyArray(,) As Object = Nothing
        Dim bGenerateDocs As Boolean = False
        Dim oValidationResults(,) As Object = Nothing
        Dim bAcceptIsValid As Boolean = False
        Dim bProduceSchedule As Boolean = False
        Dim bProduceDebitNote As Boolean = False
        Dim bProduceCertificate As Boolean = False
        Dim nProductId As Integer = 0
        ' Dim sValue As String = ""
        Dim oResultArray(,) As Object = Nothing
        Dim crGrossTotal As Decimal = 0
        Dim crNetTotal As Decimal = 0
        Dim oProductBusiness As bSIRProduct.Business
        Dim oPrintOptions(,) As Object = Nothing
        Dim oFileCntArray(,) As Object = Nothing
        Dim sPaymentMethod As String = String.Empty
        Dim dtCoverStartDate As Date
        Dim nRenewalStatusTypeId As Integer
        Dim sRenewalLeadAgentCode As String = String.Empty

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            nOldPolicyCnt = CInt(v_vPolicy(ACIRenewalLivePolicyCnt, v_lIndex))

            nRenewalStatusCnt = CInt(v_vPolicy(ACIRenewalStatusCnt, v_lIndex))

            nRenewalPolicyCnt = CInt(v_vPolicy(ACIRenewalPolicyCnt, v_lIndex))

            nInsuranceFolder = CInt(v_vPolicy(ACIRenewalInsuranceFolder, v_lIndex))

            nRenewalStatusTypeId = CInt(v_vPolicy(ACIRenewalStatusTypeId, v_lIndex))

            nPartyCnt = CInt(v_vPolicy(ACIRenewalInsuranceHolder, v_lIndex))
            sPaymentMethod = LCase(ToSafeString(v_vPolicy(ACIPaymentMethod, v_lIndex)))

            nAnniversaryCopy = gPMFunctions.ToSafeInteger(CStr(v_vPolicy(ACIRenewalAnniversaryCopy, v_lIndex)))

            bIsTrueMonthlyPolicy = gPMFunctions.ToSafeBoolean(gPMFunctions.ToSafeInteger(v_vPolicy(ACIRenewalIsTrueMonthlyPolicy, v_lIndex)) = 1)

            'nResult = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnablePayNowOptions, v_vBranch:=g_iSourceID, r_vUnderwriting:=sValue)

            nProductId = gPMFunctions.ToSafeLong(CStr(v_vPolicy(ACIRenewalProductId, v_lIndex)), 0)
            sRenewalLeadAgentCode = CStr(v_vPolicy(ACIRenewalAgentCnt, v_lIndex))

            dtCoverStartDate = CDate(v_vPolicy(ACIRenewalCoverStartDate, v_lIndex))
            If bIsTrueMonthlyPolicy = True And nAnniversaryCopy = 1 Then
                m_lReturn = g_oBusiness.GetAnnivPriorVersionInsFileCnt(nFolderCnt:=v_vPolicy(ACIRenewalInsuranceFolder, v_lIndex),
                            nPolicyCnt:=nRenewalPolicyCnt,
                            r_oFileCntArray:=oFileCntArray)

                If nResult = gPMConstants.PMEReturnCode.PMTrue Then
                    If IsArray(oFileCntArray) Then nOldPolicyCnt = ToSafeLong(oFileCntArray(0, 0))
                Else
                    gPMFunctions.RaiseError("ProcessAccept", "ProcessAccept Failed", gPMConstants.PMELogLevel.PMLogError)
                    Return nResult
                End If
            End If

            nResult = g_oBusiness.GetPolicyGrossTotal(v_lInsuranceFileCnt:=nRenewalPolicyCnt,
                                                                 r_vResults:=oResultArray)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("ProcessAccept", "Failed to Get GetPolicyGrossTotal", gPMConstants.PMELogLevel.PMLogError)
                Return nResult
            End If
            If oResultArray IsNot Nothing AndAlso TypeOf (oResultArray) Is Object(,) Then
                crNetTotal = crNetTotal + gPMFunctions.ToSafeDecimal(oResultArray(1, 0), 0)
                crGrossTotal = crGrossTotal + gPMFunctions.ToSafeDecimal(oResultArray(4, 0), 0)
            End If

            If crNetTotal < 0 Then
                r_sFailureMessage = "Premium cannot be credit type in Renewals"
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            nResult = g_oObjectManager.GetInstance(oProductBusiness, "bSIRProduct.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)

            nResult = oProductBusiness.GetProductValue(v_lProductId:=nProductId,
                                                 v_sColumnName:="is_roundoff_to_zero",
                                                  r_vProductArray:=oResultArray)

            oProductBusiness = Nothing

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("ProcessAccept", "ProcessAccept Failed", gPMConstants.PMELogLevel.PMLogError)
                Return nResult
            Else
                If oResultArray IsNot Nothing AndAlso TypeOf (oResultArray) Is Object(,) Then
                    m_bRoundOff = IIf(oResultArray(0, 0) = 1, 1, 0)
                    If m_bRoundOff Then
                        m_crRoundOffAmount = PMRoundupValueCurrency(crGrossTotal, PMECurrencyNoOfDP.pmeCurDPZero, PMERoundupFactor.pmeRFactor50Up) - crGrossTotal
                    End If
                End If

            End If

            nResult = g_oBusiness.GetPrePaymentOptionValue(nProductId, m_lPrepayment)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("ProcessAccept", "ProcessAccept Failed", gPMConstants.PMELogLevel.PMLogError)
                Return nResult
            End If

            If nAnniversaryCopy = gPMConstants.PMEReturnCode.PMTrue Then

                nResult = g_oBusiness.ValidateAcceptTMPIsValidAction(v_lInsuranceFileCnt:=nRenewalPolicyCnt, v_sInsuranceRef:=m_sInsuranceRef, r_vResults:=oValidationResults)

                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("ProcessAccept", "ProcessAccept Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            ElseIf LCase(ToSafeString(v_vPolicy(ACIPaymentMethod, v_lIndex))) = "paynow" OrElse (m_lPrepayment(0, 0) = "1" AndAlso LCase(ToSafeString(v_vPolicy(ACIPaymentMethod, v_lIndex))) = "invoice") Then

                If Information.IsArray(oValidationResults) Then
                    bAcceptIsValid = Not (gPMFunctions.ToSafeInteger(CStr(oValidationResults(0, 0)), 0) = 0)
                Else
                    bAcceptIsValid = True
                End If

                If Not bAcceptIsValid Then
                    r_sFailureMessage = "Anniversary renewal can not be accepted until the last monthly cycle has been accepted."
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    Return nResult
                End If
            Else
                nResult = gPMConstants.PMEReturnCode.PMTrue
            End If

            'Call Pay Now Process
            If m_lRenewalMode = ACRenModeRAA Or m_lRenewalMode = ACRenModeStandard Then
                'Start - Prakash - WPR85_Paralleling

                If gPMFunctions.ToSafeString(CStr(v_vPolicy(ACIPaymentMethod, v_lIndex))) = "CashDeposit" Then
                    nResult = ProcessCashDeposit()

                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = m_lReturn
                        stbMain.Items.Item(0).Text = ""
                        Return nResult
                    End If

                ElseIf LCase(ToSafeString(v_vPolicy(ACIPaymentMethod, v_lIndex))) = "paynow" OrElse (m_lPrepayment(0, 0) = "1" And LCase(ToSafeString(v_vPolicy(ACIPaymentMethod, v_lIndex))) = "invoice") Then

                    nResult = ShowPayNow(CStr(m_vRenewalPolicy(ACIPaymentMethod, v_lIndex)))

                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        stbMain.Items.Item(0).Text = ""
                        Return nResult
                    End If
                End If
            End If
            If v_bPolicyChanged Then
                nResult = g_oBusiness.AcceptRenewal(v_lOldInsuranceFileCnt:=nOldPolicyCnt, v_lNewInsuranceFileCnt:=nRenewalPolicyCnt, v_lRenewalStatusCnt:=nRenewalStatusCnt, v_sNewPolicyRef:=v_sNewPolicyNumber, v_dNewStartDate:=v_dNewStartDate, v_dNewExpiryDate:=v_dNewEndDate, r_sFailureMessage:=r_sFailureMessage, v_lAccountId:=m_lPaymentAccountID)
            Else
                nResult = g_oBusiness.AcceptRenewal(v_lOldInsuranceFileCnt:=nOldPolicyCnt, v_lNewInsuranceFileCnt:=nRenewalPolicyCnt, v_lRenewalStatusCnt:=nRenewalStatusCnt, r_sFailureMessage:=r_sFailureMessage, v_lAccountId:=m_lPaymentAccountID)
            End If

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If

            'do stats
            ReDim oKeyArray(1, 10)

            oKeyArray(0, 0) = "insurance_file_cnt"

            oKeyArray(1, 0) = nRenewalPolicyCnt

            oKeyArray(0, 1) = PMNavKeyConst.PMKeyNameIsTrueMonthlyPolicy

            oKeyArray(1, 1) = bIsTrueMonthlyPolicy

            'Float Balance and Pre-Payment

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = "Payment Account ID"

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_lPaymentAccountID

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = "Debit Against"

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_iDebitAgainst

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = "Credit Transactions"

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_vCreditTransactions

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = "Cash List ID"

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = m_lCashListID

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = "Cash ListItem ID"

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = m_lCashListItemID

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 7) = "TransactionID"

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = m_lTransactionID

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 8) = "TransactionAmount"

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 8) = m_cTransactionAmount

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = "round_off_amount"

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = m_crRoundOffAmount

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 10) = gSIRLibrary.SIRLookupPaymentMethod

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 10) = sPaymentMethod

            If RunProcess(v_sComponent:="iPMUStats.Interface_Renamed", v_vKeyArray:=oKeyArray, r_sFailureMessage:=r_sFailureMessage, v_lProcessMode:=gPMConstants.PMEComponentAction.PMAdd) <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                g_oBusiness.RollBackPolicyToPreviousStatus(v_lProductId:=nProductId, v_lRenewalStatusTypeID:=nRenewalStatusTypeId, v_lInsuranceHolderCnt:=nPartyCnt, v_lInsuranceFileCnt:=nOldPolicyCnt, v_vLeadAgentCnt:=sRenewalLeadAgentCode, v_lRenewalInsuranceFileCnt:=nRenewalPolicyCnt)
                Return nResult
            End If

            'do accumulation using the same keyarray() as (do stats)
            If RunProcess(v_sComponent:="iPMUAccumulationValues.Interface_Renamed", v_vKeyArray:=oKeyArray, r_sFailureMessage:=r_sFailureMessage, v_lProcessMode:=gPMConstants.PMEComponentAction.PMAdd) <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If

            'create renewal acceptance event

            nResult = g_oBusiness.CreateEvent(v_vEventCnt:=0, v_vPartyCnt:=nPartyCnt, v_vInsuranceFolderCnt:=nInsuranceFolder, v_vInsuranceFileCnt:=nRenewalPolicyCnt, v_vEventType:=5, v_vUserId:=g_oObjectManager.UserID, v_vEventDate:=DateTime.Today, v_vDescription:="Accept Renewal - " & CStr(v_vPolicy(ACIRenewalInsuranceRef, v_lIndex)))

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sFailureMessage = "Failed to create an event for renewal acceptance"
                Return nResult
            End If

            'create broker/agent transfer event

            If gPMFunctions.ToSafeInteger(CStr(v_vPolicy(ACIRenewalIsInTransferMode, v_lIndex)), 0) <> 0 Then

                nResult = g_oBusiness.CreateEvent(v_vEventCnt:=0, v_vPartyCnt:=nPartyCnt, v_vInsuranceFolderCnt:=nInsuranceFolder, v_vInsuranceFileCnt:=nRenewalPolicyCnt, v_vEventType:=5, v_vUserId:=g_oObjectManager.UserID, v_vEventDate:=DateTime.Today, v_vDescription:="Renewal Accepted - Broker Transfer From " & CStr(v_vPolicy(ACIRenewalLivePolicyAgentCode, v_lIndex)).Trim() &
                            " to " & CStr(v_vPolicy(ACIRenewalLeadAgentCode, v_lIndex)).Trim())

                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureMessage = "Failed to create an event for broker/agent transfer"
                    Return nResult
                End If
            End If

            '*****************************************************
            ' DONT DO ANY DOCUMENT PRODUCTION UNLESS THIS IS THE
            ' ANNIVERSARY VERSION OF THE TRUE MONTHLY POLICY
            '*****************************************************

            ' by default generate documents
            bGenerateDocs = True

            ' if the renewal policy is based on a "true monthly policy" product
            If bIsTrueMonthlyPolicy Then
                ' if this version of the policy is not flagged as the anniversary copy
                If nAnniversaryCopy <> 1 Then
                    ' do not produce any documents
                    bGenerateDocs = False
                End If
            End If

            If bGenerateDocs Then

                nResult = g_oBusiness.GetProdPrintOptions(nProductId, oPrintOptions)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Information.IsArray(oPrintOptions) Then
                    bProduceSchedule = gPMFunctions.ToSafeBoolean(oPrintOptions(0, 0))
                    bProduceCertificate = gPMFunctions.ToSafeBoolean(oPrintOptions(1, 0))
                    bProduceDebitNote = gPMFunctions.ToSafeBoolean(oPrintOptions(2, 0))
                End If

                If m_sRenSchedulePrinting = "1" And bProduceSchedule Then
                    'Generate schedule document.
                    nResult = GenerateDocument(v_lProcessType:=ACDocTypeSchedule, v_lMode:=ACSpoolSilentMode, v_lInsuranceFileCnt:=nRenewalPolicyCnt, v_lInsuranceFolderCnt:=nInsuranceFolder, v_lPartyCnt:=nPartyCnt, v_sSpoolDesc:="Accept Renewal - Schedule Document", r_sFailureMessage:=r_sFailureMessage)
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sFailureMessage = "Failed to generate schedule document"
                        Return nResult
                    End If
                End If

                If m_sRenCertificatePrinting = "1" And bProduceCertificate Then

                    'Generate certificate document.
                    nResult = GenerateDocument(v_lProcessType:=ACDocTypeCertificate, v_lMode:=ACSpoolSilentMode, v_lInsuranceFileCnt:=nRenewalPolicyCnt, v_lInsuranceFolderCnt:=nInsuranceFolder, v_lPartyCnt:=nPartyCnt, v_sSpoolDesc:="Accept Renewal -  Certificate Document", r_sFailureMessage:=r_sFailureMessage)
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sFailureMessage = "Failed to generate certificate document"
                        Return nResult
                    End If
                End If

                If m_sRenDebitNotePrinting = "1" And bProduceDebitNote Then

                    'Generate debit note
                    nResult = GenerateDocument(v_lProcessType:=ACDOCTypeDebitNote, v_lMode:=ACSpoolSilentMode, v_lInsuranceFileCnt:=nRenewalPolicyCnt, v_lInsuranceFolderCnt:=nInsuranceFolder, v_lPartyCnt:=nPartyCnt, v_sSpoolDesc:="Accept Renewal -  Debit Note Document", r_sFailureMessage:=r_sFailureMessage)
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sFailureMessage = "Failed to generate debit note document"
                        Return nResult
                    End If

                End If

            End If

            nResult = CancelMTAQuotes(nRenewalPolicyCnt, nInsuranceFolder, nPartyCnt)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                'Business must have logged it.
            End If

        Catch ex As Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to accept renewal", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAccept()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
            If oProductBusiness IsNot Nothing Then
                oProductBusiness = Nothing
            End If
        End Try
        Return nResult

    End Function

    '*******************************************************************************************************
    'Desc: produce a text file and spool it
    '*******************************************************************************************************
    Private Function RenewalReport(ByVal v_sReportTitle As String, ByVal v_sReportText As String, Optional ByVal v_sFileName As String = "", Optional ByVal v_sPath As String = "", Optional ByVal v_bDeleteFile As Boolean = True) As Integer

        Dim result As Integer = 0
        Dim sRegPath As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            'get path from registry if its not passed in
            If v_sPath = "" Then
                m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="PrntFileDir", r_sSettingValue:=v_sPath)
            End If

            'make sure we have a backslash at the end
            If Not v_sPath.EndsWith("\") Then
                v_sPath = v_sPath & "\"
            End If

            If v_sFileName = "" Then
                v_sFileName = "Renewal_" & g_oObjectManager.UserName & "_" & DateTime.Now.ToString("yyyyMMddHHMMss") & ".log"
            End If

            If AppendText(v_sFile:=v_sPath & v_sFileName, v_sTextLine:=v_sReportTitle & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & v_sReportText) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            If SpoolDoc(v_sFileName:=v_sPath & v_sFileName, v_sSpoolDesc:=v_sReportTitle) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            'delete original file
            Dim sOptionValueisSharePointOnline As String = ""
            'For SharePoint Online donot delete the file
            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=kSystemOptionIsSharePointOnline, r_sOptionValue:=sOptionValueisSharePointOnline)

            ' RAM20040209 : Bug fix for PN Issue 10231
            '               1. Changed the Dir Command to IsFileExists command
            '               2. Use Delete File Function to delete file
            If sOptionValueisSharePointOnline <> "1" Then
                If v_bDeleteFile Then
                    File.Delete(v_sPath & v_sFileName)
                End If
            End If
        Catch ex As Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to spool renewal report", vApp:=ACApp, vClass:=ACClass, vMethod:="RenewalReport", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
        Finally
        End Try
        Return result
    End Function

    '*******************************************************************************************************
    'Desc: write to text file depend on mode.
    '*******************************************************************************************************
    Private Function AppendText(ByVal v_sFile As String, ByVal v_sTextLine As String, Optional ByVal v_sMode As String = "Output") As Integer

        Dim result As Integer = 0
        Dim lFileNo As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'get free handle
            lFileNo = FileSystem.FreeFile()

            Select Case v_sMode.ToUpper()
                Case "APPEND"
                    FileSystem.FileOpen(lFileNo, v_sFile, OpenMode.Append)
                Case "BINARY"
                    FileSystem.FileOpen(lFileNo, v_sFile, OpenMode.Binary)
                Case "INPUT"
                    FileSystem.FileOpen(lFileNo, v_sFile, OpenMode.Input)
                Case "OUTPUT"
                    FileSystem.FileOpen(lFileNo, v_sFile, OpenMode.Output)
                Case "RANDOM"
                    FileSystem.FileOpen(lFileNo, v_sFile, OpenMode.Random)
            End Select

            FileSystem.PrintLine(lFileNo, v_sTextLine)

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to write to " & v_sFile, vApp:=ACApp, vClass:=ACClass, vMethod:="AppendText", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

            FileSystem.FileClose(lFileNo)

        End Try
        Return result
    End Function

    '*****************************************************************
    ' Desc : send document to document spooler (just a text file not a normal merge doc)
    '*****************************************************************
    Private Function SpoolDoc(ByVal v_sFileName As String, ByVal v_sSpoolDesc As String) As Integer

        Dim result As Integer = 0
        Dim sDocTypeID As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = g_oBusiness.GetValueFromTable(v_sTableName:="Document_Type", v_vReturnColumn:="document_type_id", v_sKeyColumn:="Code", v_sKeyValue:="LETTER", v_iDataType:=gPMConstants.PMEDataType.PMString, r_vResult:=sDocTypeID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            m_oDocTemplate.DocName = v_sFileName

            m_oDocTemplate.SpoolDesc = v_sSpoolDesc

            m_oDocTemplate.DocumentTypeId = CInt(sDocTypeID)

            m_oDocTemplate.Mode = 5 'spool report

            result = m_oDocTemplate.Start()

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SpoolDoc Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SpoolDoc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

        End Try
        Return result
    End Function

    '*****************************************************************
    ' Desc : Retrieves document id from FindDocTemplate component for
    '               given type and Policy Id and generates document via
    '               Document Template component.
    '*****************************************************************
    Private Function GenerateDocument(ByVal v_lProcessType As Integer, ByVal v_lMode As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_sSpoolDesc As String, ByRef r_sFailureMessage As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GenerateDocument"

        Dim oGetDocument As iPMUGetDocument.Interface_Renamed
        Dim vKeyArray(,) As Object
        Dim bPMBDocLink As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim vKeyArray(1, 4)
            Dim obPMBDocLink As bPMBDocLink.Business
            Dim oResultArray(,) As Object
            Dim temp_obPMBDocLink As Object
            Dim m_iFuntionalArea As Integer

            'Generate document.
            oGetDocument = New iPMUGetDocument.Interface_Renamed()
            ReDim vKeyArray(1, 5)
            If oGetDocument Is Nothing Then

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create iPMUGetDocument object", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = g_oObjectManager.GetInstance(temp_obPMBDocLink, "bPMBDocLink.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            obPMBDocLink = temp_obPMBDocLink


            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Doc Link object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTheTemplate", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            oGetDocument.Initialise()

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameInsFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = v_lInsuranceFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameDocumentID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = v_lProcessType

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameInsFolderCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = v_lInsuranceFolderCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNamePartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = v_lPartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameProductID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_lProductID

            vKeyArray.SetValue(PMNavKeyConst.PMKeynameFormlessInterface, gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5)

            vKeyArray.SetValue(True, gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5)

            m_lReturn = oGetDocument.SetKeys(vKeyArray:=vKeyArray)

            oGetDocument.FunctionalArea = 1

            If v_lProcessType = 6 Then  'This renewal notice print
                oGetDocument.TransactionType = "RNI"
            Else
                oGetDocument.TransactionType = "RN"
            End If

            oGetDocument.IsNonBatchProcess = True

            m_lReturn = oGetDocument.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Get Values from
            'For time being funtional area is set to 1 i.e. document linking for policy
            m_iFuntionalArea = 1

            m_lReturn = obPMBDocLink.GetSFIDocumentTemplatesForProcessType(v_iFunctionalArea:=m_iFuntionalArea, v_lInsurance_File_Cnt:=v_lInsuranceFileCnt, v_lProcessType_Docs_ID:=v_lProcessType, v_lProcess_Type_Code:="RN", v_dtEffectiveDate:=DateTime.Now, r_vResultarray:=oResultArray, v_bCalledFromSAM:=False)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(oResultArray) Then
                If (oResultArray(10, 0) = "Lapse") Then
                    MessageBox.Show("Lapse Document(s) spooled." & Strings.Chr(13) & Strings.Chr(10) & "Process complete", "Lapse Renewal", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If
            oGetDocument.Dispose()
            oGetDocument = Nothing

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            r_sFailureMessage = "GenerateDocument() - " & Information.Err().Description

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to generate document", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    '******************************************************************************************************
    ' product renewal notice print, change status to await update and add to last print run table
    ' don't change status if v_lRenewalStatusTypeID = 0
    '******************************************************************************************************
    Private Function ProduceNoticePrint(ByVal v_lRenewalStatusCnt As Integer, ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lProcessType As Integer, ByVal v_lMode As Integer, ByVal v_sSpoolDesc As String, ByVal v_lRenewalStatusTypeID As Integer, ByVal v_lIsInvitePrinted As Integer, ByRef r_sFailureMessage As String) As Integer

        Dim result As Integer = 0
        Dim sCreditControlEnabled As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'spool notice print
            m_lReturn = GenerateDocument(v_lProcessType:=v_lProcessType, v_lMode:=v_lMode, v_lInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_sSpoolDesc:=v_sSpoolDesc, r_sFailureMessage:=r_sFailureMessage)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                Return result
            End If

            'start transaction so we can rollback if one of the steps failed

            If g_oBusiness.BeginTransaction() <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sFailureMessage = "Failed to start transaction"

                result = gPMConstants.PMEReturnCode.PMFalse

                Return result
            End If

            If v_lRenewalStatusTypeID <> 0 Then
                'update renewal status to await update
                m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=ACCreditControlEnabled, r_sOptionValue:=sCreditControlEnabled)
                If sCreditControlEnabled = "1" Then
                    m_lReturn = g_oBusiness.SetRenewalStatusTypeID(v_lRenewalInsuranceFileCnt:=v_lRenewalInsuranceFileCnt,
                                                                    v_lRenewalStatusTypeID:=v_lRenewalStatusTypeID,
                                                                    v_lIsInvitePrinted:=v_lIsInvitePrinted,
                                                                    sCreditControlEnabled:=sCreditControlEnabled)
                Else
                    m_lReturn = g_oBusiness.SetRenewalStatusTypeID(v_lRenewalInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, v_lRenewalStatusTypeID:=v_lRenewalStatusTypeID, v_lIsInvitePrinted:=v_lIsInvitePrinted)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    r_sFailureMessage = "Failed to set renewal status to await update"

                    Return result
                End If
            End If

            'add to last print run table

            m_lReturn = g_oBusiness.AddLastPrintRun(v_lRenewalStatusCnt:=v_lRenewalStatusCnt, v_iUserID:=g_oObjectManager.UserID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                r_sFailureMessage = "Failed to add to last_print_run table"

                Return result
            End If

            If g_oBusiness.CommitTransaction() <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                r_sFailureMessage = "Failed saving to database"

                Return result
            End If

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            r_sFailureMessage = "ProduceNoticePrint() - " & Information.Err().Description

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to product renewal notice print", vApp:=ACApp, vClass:=ACClass, vMethod:="ProduceNoticePrint", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
            If result <> gPMConstants.PMEReturnCode.PMTrue Then

                m_lReturn = g_oBusiness.RollBackTransaction()
            End If

        End Try
        Return result
    End Function

    Private Function RenewalAgentList(ByVal v_iUserID As Integer) As Integer

        Dim result As Integer = 0
        Dim sExportFile, sReportOutput, sUserReportName, sReportOutputLocation, sError As String
        Dim vParam, vDefaultValue As Object

        Const ACReportAgentRenewalList As String = "AgentRenewalList"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            stbMain.Items.Item("Message").Text = "Preparing Renewal Agent List"

            'assign report name and get output path
            m_oReportPrint.reportname = ACReportAgentRenewalList
            sReportOutput = m_oReportPrint.ReportOutputLocation

            'get user report name - this is unique per user per session
            sUserReportName = m_oReportPrint.UserReportName

            If sReportOutput.Length > 1 Then
                If Not sReportOutput.EndsWith("\") Then
                    sReportOutput = sReportOutput & "\"
                End If
            End If

            'delete old version of output file (just in case)
            'If FileSystem.Dir(sReportOutput & sUserReportName & ".*", FileAttribute.Normal) <> "" Then

            For Each fileName As String In Directory.GetFiles(sReportOutput, sUserReportName & ".*")
                File.Delete(fileName)
            Next
            'End If

            stbMain.Items.Item("Message").Text = "Exporting Renewal Agent List"

            'get param from report
            'UPGRADE_TODO: (1067) Member GetParameters is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
            m_lReturn = m_oReportPrint.GetParameters(r_vParameters:=vParam, r_vDefaultValues:=vDefaultValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sError = "Failed to get parameter list"
                Throw New Exception(sError)
            End If

            'find user_id param and assign our user_id to it
            'UPGRADE_WARNING: (1068) vParam of type Variant is being forced to Array(Object). More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
            For lCount As Integer = vParam.GetLowerBound(0) To vParam.GetUpperBound(0)
                'UPGRADE_WARNING: (1068) vParam() of type Variant is being forced to String. More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                Select Case CStr(vParam(lCount, 0)).ToUpper()
                    Case "USERID"
                        'UPGRADE_WARNING: (1037) Couldn't resolve default property of object vParam(). More Information: http://www.vbtonet.com/ewis/ewi1037.aspx
                        vParam(lCount, 1) = v_iUserID
                        Exit For
                End Select
            Next lCount

            'export to word format
            'UPGRADE_TODO: (1067) Member ExportToDisk is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
            m_lReturn = m_oReportPrint.ExportToDisk(r_ExportFile:=sExportFile, v_vParameters:=vParam, v_iFormatType:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sError = "Failed to export agent list to word format"
                Throw New Exception(sError)
            End If

            stbMain.Items.Item("Message").Text = "Get Document Type ID"

            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="PrntFileDir", r_sSettingValue:=sReportOutputLocation)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sError = "Failed to get report export path"
                Throw New Exception(sError)
            End If

            sExportFile = sReportOutputLocation & sUserReportName & ".word"

            If SpoolDoc(v_sFileName:=sExportFile, v_sSpoolDesc:="Renewal Invite Agent List") <> gPMConstants.PMEReturnCode.PMTrue Then
                sError = "Failed to spool renewal agent list"
                Throw New Exception(sError)
            End If

            For Each fileName As String In Directory.GetFiles(sReportOutput, sUserReportName & ".*")
                File.Delete(fileName)
            Next
            'delete file after spooling
            'File.Delete(sReportOutput & sUserReportName & ".*")



        Catch ex As Exception

            If sError = "" Then
                result = gPMConstants.PMEReturnCode.PMError
                sError = "Failed To Print Renewal Invite Agent List"
            End If

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sError, vApp:=ACApp, vClass:=ACClass, vMethod:="RenewalAgentList", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    '*************************************************************************
    ' create all required objects
    '*************************************************************************
    Private Function CreateRequireObject() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_m_oReportPrint As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oReportPrint, "bSIRReportPrint.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            'm_lReturn = g_oObjectManager.GetInstance(temp_m_oReportPrint, "iPMBReportPrint.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oReportPrint = temp_m_oReportPrint
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            Dim temp_m_oFindDocTemplate As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oFindDocTemplate, sClassName:="iPMBFindDocTemplate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oFindDocTemplate = temp_m_oFindDocTemplate

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            Dim temp_m_oDocTemplate As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oDocTemplate, sClassName:="iPMBDocTemplate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oDocTemplate = temp_m_oDocTemplate

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create relevant objects", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateRequireObject", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                CloseRequireObject()
            End If

        End Try
        Return result
    End Function

    '*************************************************************************
    ' close and destroy all required objects
    '*************************************************************************
    Private Sub CloseRequireObject()

        Try

            If Not (m_oReportPrint Is Nothing) Then

                m_oReportPrint.Dispose()
                m_oReportPrint = Nothing
            End If

            If Not (m_oFindDocTemplate Is Nothing) Then

                m_oFindDocTemplate.Dispose()
                m_oFindDocTemplate = Nothing
            End If

            If Not (m_oDocTemplate Is Nothing) Then

                m_oDocTemplate.Dispose()
                m_oDocTemplate = Nothing
            End If

            If Not (lvwRenewalProcess Is Nothing) Then
                lvwRenewalProcess = Nothing
            End If
        Catch excep As System.Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to close relevant objects", vApp:=ACApp, vClass:=ACClass, vMethod:="CloseRequireObject", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    ' ***********************************************************
    ' Set the resizing anchors
    ' ***********************************************************
    Private Sub SetResize()

        Try

            ' Set start dimensions
            m_lWidth = CInt(VB6.PixelsToTwipsX(ClientRectangle.Width))
            m_lHeight = CInt(VB6.PixelsToTwipsY(ClientRectangle.Height))

            ' listview
            uctAnchor.Add(lvwRenewalProcess, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)

            ' Control Buttons
            uctAnchor.Add(cmdCancel, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
            uctAnchor.Add(cmdAmend, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
            uctAnchor.Add(cmdLapse, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
            uctAnchor.Add(cmdDelete, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
            uctAnchor.Add(cmdStatus, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
            uctAnchor.Add(cmdAccept, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
            uctAnchor.Add(cmdWrite, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)  ' Written Status.doc
        Catch
        End Try

    End Sub

    ''' <summary>
    ''' show renewal criteria form and return selected criteria to FrmRenewalProcess
    ''' </summary>
    Private Sub ShowFilterCriteria()

        Dim oFilterForm As frmRenewalFilter
        Dim bDone As Boolean

        Try

            oFilterForm = New frmRenewalFilter()

            'set renewal mode
            oFilterForm.RenewalMode = m_lOriginalRenewalMode

            'set broker transfer authority
            oFilterForm.CanTransferBroker = m_bCanTransferBroker

            Do Until bDone

                oFilterForm.ShowDialog()

                If oFilterForm.Status = gPMConstants.PMEReturnCode.PMOK Then
                    m_sInsuranceRef = oFilterForm.InsuranceRef
                    m_dRenewalDate = oFilterForm.RenewalDate
                    m_lProductID = oFilterForm.ProductID
                    m_lBranchID = oFilterForm.BranchID
                    m_lRenewalType = oFilterForm.RenewalType
                    m_lLeadAgentCnt = oFilterForm.LeadAgentCnt
                    m_lAgentcode = oFilterForm.AgentCode

                    'get if written is used
                    m_lWrittenUsed = g_oBusiness.IsWrittenUsed()
                    If m_lWrittenUsed = gPMConstants.PMEReturnCode.PMTrue Or m_lWrittenUsed = gPMConstants.PMEReturnCode.PMNotFound Then
                        If m_lWrittenUsed = gPMConstants.PMEReturnCode.PMTrue Then
                            mnuRenewalProcessWrite.Visible = True
                        Else
                            cmdWrite.Visible = False
                            mnuRenewalProcessWrite.Visible = False
                        End If
                    Else
                        RaiseError("ShowFilterCriteria", "IsWrittenUsed failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                    'End Written Status.doc
                    'get renewal list
                    If GetBusiness(m_vRenewalPolicy, 0, m_sInsuranceRef, m_dRenewalDate, m_lProductID, m_lBranchID, m_lRenewalType, m_lLeadAgentCnt, m_lAgentcode) = gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = BusinessToInterface()
                        Select Case m_lReturn
                            Case gPMConstants.PMEReturnCode.PMTrue
                                ' Set resize details for form controls and exit loop
                                SetResize()
                                Me.BringToFront()
                                bDone = True

                                If m_lOriginalRenewalMode = ACRenModeRI Or m_lOriginalRenewalMode = ACRenModeRAA Then
                                    'launch amendment process
                                    cmdAmend_Click(cmdAmend, New EventArgs())
                                ElseIf m_lOriginalRenewalMode = ACRenModeRA Then
                                    cmdAccept_Mode(v_bLocked:=False)
                                End If
                            Case gPMConstants.PMEReturnCode.PMNotFound
                                ' Do nothing, loop again
                                ' oFilterForm = New frmRenewalFilter()

                            Case Else
                                'error
                                bDone = True
                        End Select
                    Else
                        bDone = True
                    End If

                Else
                    bDone = True
                End If
            Loop
        Catch ex As Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get criteria for renewal", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowFilterCriteria()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
            oFilterForm = Nothing

            'Me.BringToFront()

        End Try
        Exit Sub
    End Sub

    '*********************************************************************************
    ' Get renewal status type details
    '*********************************************************************************
    Private Function GetRenewalStatusType(ByVal v_sRenStatusCode As String, ByRef r_sDesc As String, ByRef r_lRenewalStatusTypeID As Integer) As Integer

        Dim result As Integer = 0
        Dim vReturnColumn As Object
        Dim vResult(,) As Object
        Dim sMessage As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim vReturnColumn(1)

            vReturnColumn(0) = "renewal_status_type_id"

            vReturnColumn(1) = "description"

            If g_oBusiness.GetValueFromTable(v_sTableName:="Renewal_Status_Type", v_vReturnColumn:=vReturnColumn, v_sKeyColumn:="Code", v_sKeyValue:=v_sRenStatusCode, v_iDataType:=gPMConstants.PMEDataType.PMString, r_vResult:=vResult) <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to get Renewal Status Type details"
                Throw New Exception()
            End If

            If Not Information.IsArray(vResult) Then
                sMessage = "No details found for Renewal Status Type (" & v_sRenStatusCode & ")"
                Throw New Exception()
            End If

            r_lRenewalStatusTypeID = CInt(vResult(0, 0))

            r_sDesc = CStr(vResult(1, 0))

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            If sMessage = "" Then
                sMessage = "Failed to get renewal status type details"
            End If

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="GetRenewalStatusType()", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '**************************************************************************************************
    ' Desc :Accepting renewal
    '       this function is called from cmdAccept_Click() and cmdAmend_Clicked()
    'Note:  this is GJW mode - there will only be one record
    '**************************************************************************************************
    Private Sub cmdAccept_Mode(Optional ByVal v_bLocked As Boolean = False)

        Dim lIndex As Integer
        Dim bLocked As Boolean
        Dim sNewPolicyRef As String = ""
        Dim dNewStartDate, dNewEndDate As Date
        Dim bChanged As Boolean
        Dim sFailureMessage, sLockedBy As String
        Dim lIsQuoted As gPMConstants.PMEReturnCode
        Dim vListViewUpdate(0, 1) As Object

        Dim sRenStatusDesc As String = ""
        Dim lRenStatusTypeID As Integer

        Dim sMsgBox As String = ""
        Dim lYesNo As DialogResult

        Try

            If lvwRenewalProcess.Items.Count = 0 Then
                Exit Sub
            End If

            If Not lvwRenewalProcess.Items.Item(0).Checked Then
                MessageBox.Show("Warning! Please select an item from the list", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            'get array position

            lIndex = Convert.ToString(lvwRenewalProcess.Items.Item(0).Tag)

            'this will be True if we come from cmdAmend()
            If Not v_bLocked Then

                'check to see if we can accept this policy
                m_lReturn = CheckRenewalStatus(lIndex, "ACCEPT", sMsgBox, lYesNo)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("CheckRenewalStatus() error", Application.ProductName)
                    Exit Sub
                Else
                    If sMsgBox <> "" Then
                        MessageBox.Show(sMsgBox, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit Sub
                    End If
                End If

                Me.stbMain.Items.Item("Message").Text = "Locking policy please wait"
                Me.stbMain.Refresh()
                'lock this renewal status count to stop others from processing it

                m_lReturn = g_oBusiness.LockKey(v_sKeyName:=ACLockName,
                                                v_lKeyValue:=CInt(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)),
                                                v_lUserID:=g_oObjectManager.UserID, r_sLockedBy:=sLockedBy)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If sLockedBy = "ERROR" Then
                        MessageBox.Show("Failed to lock policy for, Insurance Folder count : " &
                                        CStr(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)) &
                                        Strings.Chr(13) & Strings.Chr(10) & "Process terminate.",
                                        Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Else
                        MessageBox.Show("Current policy " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, lIndex)) & " is being locked by " & sLockedBy & Strings.Chr(13) & Strings.Chr(10) & "Process terminate.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If

                    Exit Sub
                End If
                bLocked = True
            End If

            Me.stbMain.Items.Item("Message").Text = "Change policy details please wait"
            Me.stbMain.Refresh()
            m_lReturn = ChangePolicyDetail(v_vPolicy:=m_vRenewalPolicy, v_lIndex:=lIndex, r_sNewPolicyRef:=sNewPolicyRef, r_dNewStartDate:=dNewStartDate, r_dNewExpiryDate:=dNewEndDate, r_bchanged:=bChanged)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
                    MessageBox.Show("Failed to change policy details", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

                Exit Sub
            End If

            Me.stbMain.Items.Item("Message").Text = "Checking to see if policy is quoted please wait"
            Me.stbMain.Refresh()
            If g_oBusiness.IsQuoted(v_lInsuranceFileCnt:=CInt(m_vRenewalPolicy(ACIRenewalPolicyCnt, lIndex)), r_lResult:=lIsQuoted) = gPMConstants.PMEReturnCode.PMTrue Then
                If lIsQuoted = gPMConstants.PMEReturnCode.PMTrue Then
                    Me.stbMain.Items.Item("Message").Text = "Accepting policy please wait"
                    Me.stbMain.Refresh()
                    m_lReturn = ProcessAccept(v_vPolicy:=m_vRenewalPolicy, v_lIndex:=lIndex, v_bPolicyChanged:=bChanged, v_sNewPolicyNumber:=sNewPolicyRef, v_dNewStartDate:=dNewStartDate, v_dNewEndDate:=dNewEndDate, r_sFailureMessage:=sFailureMessage)
                    If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                        Exit Sub
                    End If
                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        'did we failed on doing any of the document or creating work task?
                        MessageBox.Show("Successfully accepted policy." & Strings.Chr(13) & Strings.Chr(10) & sFailureMessage, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        MessageBox.Show(sFailureMessage, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit Sub
                    End If
                Else
                    Me.stbMain.Items.Item("Message").Text = "Getting renewal status type please wait"
                    Me.stbMain.Refresh()
                    'get renewal status type details for (awaiting manual review)
                    m_lReturn = GetRenewalStatusType(v_sRenStatusCode:="ManReview", r_sDesc:=sRenStatusDesc, r_lRenewalStatusTypeID:=lRenStatusTypeID)

                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Policy is not quoted." & Strings.Chr(13) & Strings.Chr(10) & "Renewal status will be set to awaiting manual review", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

                        Me.stbMain.Items.Item("Message").Text = "Updating renewal status please wait"
                        Me.stbMain.Refresh()
                        'reset status to awaiting manual review

                        If g_oBusiness.SetRenewalStatusTypeID(v_lRenewalInsuranceFileCnt:=CInt(m_vRenewalPolicy(ACIRenewalPolicyCnt, lIndex)), v_lRenewalStatusTypeID:=lRenStatusTypeID) = gPMConstants.PMEReturnCode.PMTrue Then
                            Me.stbMain.Items.Item("Message").Text = "Updating Listview with new status please wait"
                            Me.stbMain.Refresh()
                            'update listview with new status
                            m_lReturn = SetListViewRenewalStatus(v_lArrayIndex:=lIndex, v_lSelectedIndex:=1, r_sFailureMessage:=sFailureMessage, v_sRenStatusDesc:=sRenStatusDesc, v_lRenStatusTypeID:=lRenStatusTypeID)
                        Else
                            MessageBox.Show("Policy is not quoted" & Strings.Chr(13) & Strings.Chr(10) & "Failed to set renewal status to awaiting manual review.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    Else
                        MessageBox.Show("Failed to get renewal status type details." & Strings.Chr(13) & Strings.Chr(10) & "Do you want to continue with next selected policy?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    End If

                    Exit Sub
                End If
            Else
                MessageBox.Show("Failed to check quote status." & Strings.Chr(13) & Strings.Chr(10) & "Process terminate.?", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            'mark this as deleted from listview
            m_vRenewalPolicy(ACRenewalDeleteFromListView, lIndex) = "1"

            'remove from list to stop user from selecting this again
            lvwRenewalProcess.Items.RemoveAt(0)



        Catch ex As Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed Amendment process", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAmend_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
            If Not v_bLocked Then
                If bLocked Then
                    Me.stbMain.Items.Item("Message").Text = "Unlocking policy please wait"
                    Me.stbMain.Refresh()
                    If g_oBusiness.UnLockKey(v_sKeyName:=ACLockName, v_lKeyValue:=m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex),
                                             v_lUserID:=g_oObjectManager.UserID) <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to unlock KeyName: " & ACLockName & Strings.Chr(13) & Strings.Chr(10) &
                                        "KeyValue: " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)) &
                                        Strings.Chr(13) & Strings.Chr(10) & "Process terminate.", Me.Text, MessageBoxButtons.OK,
                                        MessageBoxIcon.Information)
                    End If
                End If
            End If

            Me.stbMain.Items.Item("Message").Text = "Ready"
            Me.stbMain.Refresh()
        End Try
    End Sub

    '**************************************************************************************************
    ' Desc : get renewal status type details for policy or for renewal status type code
    '        and update list view with it
    '
    '**************************************************************************************************
    Private Function SetListViewRenewalStatus(ByVal v_lArrayIndex As Integer, ByVal v_lSelectedIndex As Integer, ByRef r_sFailureMessage As String, Optional ByVal v_sRenStatusTypeCode As String = "", Optional ByVal v_lRenewalStatusCnt As Integer = 0, Optional ByVal v_sRenStatusDesc As String = "", Optional ByVal v_lRenStatusTypeID As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim vPolicyRenewalStatus As Object
        Dim vListViewUpdate(0, 1) As Object
        Dim lIcon As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            'did we pass in renewal status type desc and renewal status type id?
            If v_sRenStatusDesc = "" And v_lRenStatusTypeID = 0 Then
                'we must have either renewal status type code or renewal status count
                If v_sRenStatusTypeCode = "" And v_lRenewalStatusCnt = 0 Then
                    Return result
                End If

                'are we getting renewal status type detail via renewal status code?
                If v_sRenStatusTypeCode = "" Then
                    'get current renewal status for this policy

                    If g_oBusiness.GetPolicyRenewalStatus(v_lRenewalStatusCnt:=v_lRenewalStatusCnt, r_vResultArray:=vPolicyRenewalStatus) = gPMConstants.PMEReturnCode.PMTrue Then
                        If Information.IsArray(vPolicyRenewalStatus) Then

                            v_sRenStatusDesc = CStr(vPolicyRenewalStatus(2, 0))

                            v_lRenStatusTypeID = CInt(vPolicyRenewalStatus(0, 0))
                        Else
                            r_sFailureMessage = "No renewal status found for renewal_status_cnt = " & v_lRenewalStatusCnt
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Else
                        r_sFailureMessage = "Failed to get renewal status type (renewal_stauts_cnt = " & v_lRenewalStatusCnt
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Else
                    ' via renewal status code
                    If GetRenewalStatusType(v_sRenStatusCode:=v_sRenStatusTypeCode, r_sDesc:=v_sRenStatusDesc, r_lRenewalStatusTypeID:=v_lRenStatusTypeID) <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sFailureMessage = "Failed to get renewal status type detail for renewal status code (" & v_sRenStatusTypeCode & ")"
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            'work out which icon to use
            Select Case v_lRenStatusTypeID
                Case 1, 3, 4, 6  'manual update/review
                    lIcon = ACIconManual
                Case 5  'await update (acceptance)
                    lIcon = ACIconAccept
                Case 2  'notice print
                    lIcon = ACIconInvite
                    'Start Written Status.doc

                Case gPMConstants.PMBRenewalStatusTypeWrittenAwaitUpdate
                    lIcon = ACIconWrite
                    'End  Written Status.doc
            End Select

            'update array with new status details and redisplay
            m_vRenewalPolicy(ACIRenewalStatusType, v_lArrayIndex) = v_sRenStatusDesc
            m_vRenewalPolicy(ACIRenewalStatusTypeId, v_lArrayIndex) = v_lRenStatusTypeID

            'update listview with new status
            vListViewUpdate(0, 0) = 8

            vListViewUpdate(0, 1) = v_sRenStatusDesc

            If UpdateListView(v_oListView:=lvwRenewalProcess, v_vColumnIndex:=vListViewUpdate, v_lSelectedIndex:=v_lSelectedIndex, v_lIcon:=lIcon) <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sFailureMessage = "Failed to update list view with new renewal status"
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            r_sFailureMessage = "Failed to update renewal status on list view"

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sFailureMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="SetListViewRenewalStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ''' <summary>
    ''' get details for this renwal policy and redisplay list view with new data
    ''' </summary>
    ''' <param name="vLListIndex"></param>
    ''' <param name="vLArrayIndex"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function RePopulatePolicy(ByVal vLListIndex As Integer, ByVal vLArrayIndex As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object
        Dim lIcon As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            'get new data for this policy version

            If GetBusiness(vResultArray, CInt(m_vRenewalPolicy(ACIRenewalPolicyCnt, vLArrayIndex)), "", DateTime.Today, 0, 0, 0, 0) <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to get details for this policy - " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, vLArrayIndex)).Trim(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            'update renewal array with new data
            For lCount As Integer = 0 To m_vRenewalPolicy.GetUpperBound(0)
                If Not vResultArray Is Nothing Then
                    m_vRenewalPolicy(lCount, vLArrayIndex) = vResultArray(lCount, 0)
                End If
            Next lCount

            'work out which icon to use
            Select Case m_vRenewalPolicy(ACIRenewalStatusTypeId, vLArrayIndex)
                Case 1, 3, 4, 6  'manual update/review
                    lIcon = 0
                Case 5  'await update (acceptance)
                    lIcon = 1
                Case 2  'notice print
                    lIcon = 2
                Case 7  'broker/agent transfer
                    lIcon = 3
            End Select

            'now update list view with new data
            'Col 1 branch           
            lvwRenewalProcess.Items.Item(vLListIndex - 1).Text = CStr(m_vRenewalPolicy(ACIRenewalSourceCode, vLArrayIndex)).Trim()
            lvwRenewalProcess.Items(vLListIndex - 1).ImageIndex = lIcon

            lvwRenewalProcess.Items.Item(vLListIndex - 1).SubItems.Item(1).Text = CStr(m_vRenewalPolicy(ACIRenewalShortname, vLArrayIndex)).Trim()

            'col 3 Clien Name
            lvwRenewalProcess.Items.Item(vLListIndex - 1).SubItems.Item(2).Text = CStr(m_vRenewalPolicy(ACIRenewalResolvedName, vLArrayIndex)).Trim()

            'col 3 policy no
            lvwRenewalProcess.Items.Item(vLListIndex - 1).SubItems.Item(3).Text = CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, vLArrayIndex)).Trim()

            'col 4 agent
            lvwRenewalProcess.Items.Item(vLListIndex - 1).SubItems.Item(4).Text = CStr(m_vRenewalPolicy(ACIRenewalLeadAgentCode, vLArrayIndex)).Trim()

            'col 5 Agent Name
            lvwRenewalProcess.Items.Item(vLListIndex - 1).SubItems.Item(5).Text = CStr(m_vRenewalPolicy(ACIRenewalLeadAgentDescription, vLArrayIndex)).Trim()

            'col 5 account handler
            lvwRenewalProcess.Items.Item(vLListIndex - 1).SubItems.Item(6).Text = CStr(m_vRenewalPolicy(ACIRenewalAccHandlerCode, vLArrayIndex)).Trim()

            'col 6 renewal date (of the live policy not the renewal version)
            lvwRenewalProcess.Items.Item(vLListIndex - 1).SubItems.Item(7).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, CStr(m_vRenewalPolicy(ACIRenewalDate, vLArrayIndex)).Trim())

            'col 7 status
            lvwRenewalProcess.Items.Item(vLListIndex - 1).SubItems.Item(8).Text = CStr(m_vRenewalPolicy(ACIRenewalStatusType, vLArrayIndex)).Trim()

            'col 8 product
            lvwRenewalProcess.Items.Item(vLListIndex - 1).SubItems.Item(9).Text = CStr(m_vRenewalPolicy(ACIRenewalProduct, vLArrayIndex)).Trim()

            'col 9 claim indicator
            lvwRenewalProcess.Items.Item(vLListIndex - 1).SubItems.Item(10).Text = CStr(m_vRenewalPolicy(ACIRenewalClaimsIndicator, vLArrayIndex)).Trim()

            'col 10 transfer broker to
            If gPMFunctions.ToSafeInteger(CStr(m_vRenewalPolicy(ACIRenewalStatusTypeId, vLArrayIndex)), 0) = gPMConstants.PMBRenewalStatusTypeAwaitBrokerTransfer Then
                If gPMFunctions.ToSafeInteger(CStr(m_vRenewalPolicy(ACIRenewalTransferToPartyCnt, vLArrayIndex)), 0) = 0 Then
                    lvwRenewalProcess.Items.Item(vLListIndex - 1).SubItems.Item(11).Text = "Direct"
                Else
                    lvwRenewalProcess.Items.Item(vLListIndex - 1).SubItems.Item(11).Text = CStr(m_vRenewalPolicy(ACIRenewalTransferToPartyShortName, vLArrayIndex)).Trim()
                End If
            Else
                lvwRenewalProcess.Items.Item(vLListIndex - 1).SubItems.Item(11).Text = ""
            End If

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to repopulate details for this policy", vApp:=ACApp, vClass:=ACClass, vMethod:="RePopulatePolicy", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
        Finally

        End Try
        Return result
    End Function

    '*****************************************************************************
    ' get broker transfer authority for this user and assign it to the module level variable
    '*****************************************************************************
    Private Function GetBrokerTransferAuthority() As Integer

        Dim result As Integer = 0
        Dim vResult As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            result = g_oBusiness.GetValueFromTable(v_sTableName:="User_Authorities", v_vReturnColumn:="can_perform_broker_transfer", v_sKeyColumn:="user_id", v_sKeyValue:=g_oObjectManager.UserID, v_iDataType:=gPMConstants.PMEDataType.PMInteger, r_vResult:=vResult)

            If result = gPMConstants.PMEReturnCode.PMTrue Then
                m_bCanTransferBroker = (gPMFunctions.ToSafeInteger(vResult, 0) = 1)
            End If



        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get user authority for broker transfer portfolio", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBrokerTransferAuthority()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

        End Try
        Return result
    End Function

    Private Sub CheckUnCheckListView(Optional ByVal v_bChecked As Boolean = True)

        Try

            For lCount As Integer = 1 To lvwRenewalProcess.Items.Count
                lvwRenewalProcess.Items.Item(lCount - 1).Checked = v_bChecked
            Next

            DisplayListViewCount()

        Catch ex As Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check/uncheck list", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckUnCheckListView()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
        End Try
        Exit Sub
    End Sub

    Private Sub DisplayListViewCount()

        Dim lCount As Integer

        ListViewIsTick(lvwRenewalProcess, lCount)

        If lCount > 0 Then
            stbMain.Items.Item("Counter").Text = CStr(lCount) & "/" & CStr(lvwRenewalProcess.Items.Count)
            Me.stbMain.Refresh()
        Else
            stbMain.Items.Item("Counter").Text = CStr(lvwRenewalProcess.Items.Count)
            Me.stbMain.Refresh()
        End If

    End Sub
    Private Function ShowPayNow(ByVal sPaymentMethod As String) As Integer
        Dim result As Integer = 0
        Dim iPMUPaynowOptions As Object

        Dim oPayNow As iPMUPayNowOptions.Interface_Renamed
        Dim sErrMsg As String = ""
        Dim lGrossTotal As Decimal

        'Find item position in array
        Dim iPosInArray As Integer
        Dim bFlag As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            Dim temp_oPayNow As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPayNow, sClassName:="iPMUPayNowOptions.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oPayNow = temp_oPayNow
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sErrMsg = "Failed to get PayNow Instance"
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            bFlag = False
            If m_lCount = 0 Then
                m_lCount = lvwRenewalProcess.Items.Count
            End If
            For iListCount As Integer = lvwRenewalProcess.Items.Count To 1 Step -1
                If lvwRenewalProcess.Items.Item(iListCount - 1).Checked And iListCount <= m_lCount Then
                    For iVar As Integer = 0 To m_vRenewalPolicy.GetUpperBound(1)
                        If ListViewHelper.GetListViewSubItem(lvwRenewalProcess.Items.Item(iListCount - 1), 3).Text = CStr(m_vRenewalPolicy(5, iVar)).Trim() Then
                            iPosInArray = iVar
                            bFlag = True
                            m_lCount = iPosInArray
                            Exit For
                        End If
                    Next
                End If
                If bFlag Then Exit For
            Next

            oPayNow.PaymentOption = sPaymentMethod

            m_lReturn = GetGrossTotal(lGrossTotal, CInt(m_vRenewalPolicy(ACIRenewalPolicyCnt, iPosInArray)))

            oPayNow.InsuranceFileCnt = m_vRenewalPolicy(ACIRenewalPolicyCnt, iPosInArray)  'deepak

            oPayNow.AmountDue = lGrossTotal
            m_cTransactionAmount = lGrossTotal
            'Ashwani - (RFC_Enable_PrePayment_functionality)
            oPayNow.PrePayment = m_lPrepayment(0, 0)

            m_lReturn = oPayNow.Start

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sErrMsg = "PayNow.Start Failed"
                Return m_lReturn
            End If

            If oPayNow.OKClick Then
                'Get Values from iPMUPaynowOptions for GetKeys()

                m_lPaymentAccountID = oPayNow.PaymentAccountID

                m_iDebitAgainst = oPayNow.DebitAgainst

                m_vCreditTransactions = oPayNow.CreditTransactions

                m_lCashListID = oPayNow.CashListID

                m_lCashListItemID = oPayNow.CashListItemID

                m_lTransactionID = oPayNow.CashTransDetailID
            Else
                result = gPMConstants.PMEReturnCode.PMCancel
            End If

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sErrMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPayNow", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    'Gives the grossTotal of all the Selected Policies
    Private Function GetGrossTotal(ByRef value As Decimal, ByVal lInsuranceFileCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vResult(,) As Object
        Dim vGrosstotal As Decimal
        Dim sErrMsg As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim g_oRenewal As bSIRRenewalProcess.Business
            Dim sAgentType As String = ""

            Dim temp_g_oRenewal As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oRenewal, "bSIRRenewalProcess.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            g_oRenewal = temp_g_oRenewal

            lReturn = g_oRenewal.GetPolicyGrossTotal(v_lInsuranceFileCnt:=lInsuranceFileCnt, r_vResults:=vResult)
            'Deepak
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sErrMsg = "Failed to Get GetPolicyGrossTotal"
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
            If Information.IsArray(vResult) Then
                vGrosstotal += CDec(vResult(4, 0))
            End If

            If m_bRoundOff Then
                m_crRoundOffAmount = PMRoundupValueCurrency(vGrosstotal, PMECurrencyNoOfDP.pmeCurDPZero, PMERoundupFactor.pmeRFactor50Up) - vGrosstotal
            End If

            vResult = Nothing
            Dim cAgentcommission As Decimal

            'lReturn = g_oRenewal.GetAgentCommission(m_vRenewalPolicy(ACIRenewalPolicyCnt, iPosInArray), vResult)
            lReturn = g_oRenewal.GetAgentCommission(lInsuranceFileCnt, vResult)

            If Information.IsArray(vResult) Then

                sAgentType = CStr(vResult(1, 0)).Trim()
                If sAgentType = "Broker" Then

                    cAgentcommission = gPMFunctions.ToSafeCurrency(CStr(vResult(6, 0))) + gPMFunctions.ToSafeCurrency(CStr(vResult(16, 0)))
                    vGrosstotal -= cAgentcommission
                End If
            End If

            value = vGrosstotal + m_crRoundOffAmount

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sErrMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="GetGrossTotal", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
        Finally

        End Try
        Return result
    End Function

    'Start (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx)
    '**************************************************************************************************
    ' Desc :Accepting renewal
    '       this function is called from mnuRenewalProcessAmend_Click iteratively if the current policy
    '       has a renewal status as PMBRenewalStatusTypeAwaitUpdate
    'Note: This method is a replica of cmdAccept_Mode with the ability accept a policy from the list
    '      containing multiple policies
    '**************************************************************************************************
    Private Sub RunRenewalAcceptance(ByRef r_sReportText As String, ByRef r_lInvalidTMPcount As Integer, ByRef r_bContinue As Boolean, ByVal lListIndex As Integer, Optional ByVal v_bLocked As Boolean = False)

        Const kMethodName As String = "RunRenewalAcceptance"

        Dim lArrayIndex As Integer
        Dim bLocked As Boolean
        Dim sNewPolicyRef As String = ""
        Dim dNewStartDate, dNewEndDate As Date
        Dim bChanged As Boolean
        Dim sFailureMessage, sLockedBy As String
        Dim lIsQuoted As gPMConstants.PMEReturnCode

        Dim sRenStatusDesc As String = ""
        Dim lRenStatusTypeID As Integer

        Dim sMsgBox As String = ""
        Dim lYesNo As DialogResult

        Try

            r_bContinue = True

            If Not lvwRenewalProcess.Items.Item(lListIndex - 1).Checked Then
                MessageBox.Show("Warning! Please select an item from the list", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            'get array position

            lArrayIndex = Convert.ToString(lvwRenewalProcess.Items.Item(lListIndex - 1).Tag)

            If Not v_bLocked Then

                'check to see if we can accept this policy
                m_lReturn = CheckRenewalStatus(lArrayIndex, "ACCEPT", sMsgBox, lYesNo)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("CheckRenewalStatus() error", Application.ProductName)

                    Exit Sub
                Else
                    If sMsgBox <> "" Then
                        MessageBox.Show(sMsgBox, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)

                        Exit Sub
                    End If
                End If

                Me.stbMain.Items.Item("Message").Text = "Locking policy please wait"
                Me.stbMain.Refresh()
                'lock this renewal status count to stop others from processing it

                m_lReturn = g_oBusiness.LockKey(v_sKeyName:=ACLockName, v_lKeyValue:=CInt(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lArrayIndex)), v_lUserID:=g_oObjectManager.UserID, r_sLockedBy:=sLockedBy)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If sLockedBy = "ERROR" Then
                        MessageBox.Show("Failed to lock policy for, Insurance Folder count : " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lArrayIndex)) & Strings.Chr(13) & Strings.Chr(10) & "Process terminate.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Else
                        MessageBox.Show("Current policy " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, lArrayIndex)) & " is being locked by " & sLockedBy & Strings.Chr(13) & Strings.Chr(10) & "Process terminate.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If

                    Exit Sub
                End If
            End If

            Me.stbMain.Items.Item("Message").Text = "Change policy details please wait"
            Me.stbMain.Refresh()
            m_lReturn = ChangePolicyDetail(v_vPolicy:=m_vRenewalPolicy, v_lIndex:=lArrayIndex, r_sNewPolicyRef:=sNewPolicyRef, r_dNewStartDate:=dNewStartDate, r_dNewExpiryDate:=dNewEndDate, r_bchanged:=bChanged)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
                    MessageBox.Show("Failed to change policy details", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

                Exit Sub
            End If

            Me.stbMain.Items.Item("Message").Text = "Checking to see if policy is quoted please wait"
            Me.stbMain.Refresh()

            If g_oBusiness.IsQuoted(v_lInsuranceFileCnt:=CInt(m_vRenewalPolicy(ACIRenewalPolicyCnt, lArrayIndex)), r_lResult:=lIsQuoted) = gPMConstants.PMEReturnCode.PMTrue Then
                If lIsQuoted = gPMConstants.PMEReturnCode.PMTrue Then
                    Me.stbMain.Items.Item("Message").Text = "Accepting policy please wait"
                    Me.stbMain.Refresh()

                    m_lReturn = ProcessAccept(v_vPolicy:=m_vRenewalPolicy, v_lIndex:=lArrayIndex, v_bPolicyChanged:=bChanged, v_sNewPolicyNumber:=sNewPolicyRef, v_dNewStartDate:=dNewStartDate, v_dNewEndDate:=dNewEndDate, r_sFailureMessage:=sFailureMessage)
                    If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                        Exit Sub
                    End If

                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        'Pass the status to report text
                        r_sReportText = r_sReportText & Strings.Chr(13) & Strings.Chr(10) & CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, lArrayIndex)) & " - Successful" & Strings.Chr(13) & Strings.Chr(10) & sFailureMessage & Strings.Chr(13) & Strings.Chr(10)
                    Else
                        r_sReportText = r_sReportText & Strings.Chr(13) & Strings.Chr(10) & CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, lArrayIndex)) & " - " & sFailureMessage & Strings.Chr(13) & Strings.Chr(10)
                        If gPMFunctions.ToSafeInteger(CStr(m_vRenewalPolicy(ACIRenewalIsTrueMonthlyPolicy, lArrayIndex))) = 1 Then
                            r_lInvalidTMPcount += 1
                        End If

                        Exit Sub
                    End If
                Else

                    r_sReportText = r_sReportText & Strings.Chr(13) & Strings.Chr(10) & CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, lArrayIndex)) & " - Unquoted" & Strings.Chr(13) & Strings.Chr(10)

                    Me.stbMain.Items.Item("Message").Text = "Getting renewal status type please wait"
                    Me.stbMain.Refresh()
                    'get renewal status type details for (awaiting manual review)
                    m_lReturn = GetRenewalStatusType(v_sRenStatusCode:="ManReview", r_sDesc:=sRenStatusDesc, r_lRenewalStatusTypeID:=lRenStatusTypeID)

                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                        Me.stbMain.Items.Item("Message").Text = "Updating renewal status please wait"
                        Me.stbMain.Refresh()
                        'reset status to awaiting manual review

                        If g_oBusiness.SetRenewalStatusTypeID(v_lRenewalInsuranceFileCnt:=CInt(m_vRenewalPolicy(ACIRenewalPolicyCnt, lArrayIndex)), v_lRenewalStatusTypeID:=lRenStatusTypeID) = gPMConstants.PMEReturnCode.PMTrue Then
                            Me.stbMain.Items.Item("Message").Text = "Updating Listview with new status please wait"
                            Me.stbMain.Refresh()
                            'update listview with new status
                            m_lReturn = SetListViewRenewalStatus(v_lArrayIndex:=lArrayIndex, v_lSelectedIndex:=lListIndex, r_sFailureMessage:=sFailureMessage, v_sRenStatusDesc:=sRenStatusDesc, v_lRenStatusTypeID:=lRenStatusTypeID)
                        Else
                            If MessageBox.Show("Failed to set renewal status to awaiting manual review." & Strings.Chr(13) & Strings.Chr(10) & "Do you want to continue with next selected policy?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                                r_bContinue = False
                            End If

                            Exit Sub
                        End If
                    Else
                        If MessageBox.Show("Failed to get renewal status type details." & Strings.Chr(13) & Strings.Chr(10) & "Do you want to continue with next selected policy?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                            r_bContinue = False
                        End If

                        Exit Sub
                    End If
                End If
            Else
                If MessageBox.Show("Failed to check quote status." & Strings.Chr(13) & Strings.Chr(10) & "Do you want to continue with next selected policy?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                    r_bContinue = False
                End If

                Exit Sub
            End If

            'mark this as deleted from listview
            m_vRenewalPolicy(ACRenewalDeleteFromListView, lArrayIndex) = "1"

            'remove from list to stop user from selecting this again
            lvwRenewalProcess.Items.RemoveAt(lListIndex - 1)



        Catch ex As Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Acceptance process Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

            ' If an exception occurs, and this function is called from an loop,
            ' setting this flag will allow the loop to exit.
            r_bContinue = False
        Finally
            If Not v_bLocked Then
                If bLocked Then
                    Me.stbMain.Items.Item("Message").Text = "Unlocking policy please wait"
                    Me.stbMain.Refresh()
                    If g_oBusiness.UnLockKey(v_sKeyName:=ACLockName, v_lKeyValue:=m_vRenewalPolicy(ACIRenewalInsuranceFolder, lArrayIndex), v_lUserID:=g_oObjectManager.UserID) <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to unlock KeyName: " & ACLockName & Strings.Chr(13) & Strings.Chr(10) & "KeyValue: " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lArrayIndex)) & Strings.Chr(13) & Strings.Chr(10) & "Process terminate.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            End If

            Me.stbMain.Items.Item("Message").Text = "Ready"
            Me.stbMain.Refresh()
        End Try
    End Sub
    'End (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx)
    'Start - Prakash - WPR85_Paralleling
    Private Function ProcessCashDeposit() As Integer
        Dim result As Integer = 0
        Dim iSIRPolicyCashDeposit As Object
        Dim oCashDeposit As Object
        Dim sErrMsg As String = ""
        'Start - PN 65531
        Dim crGrossTotal, crLeadAgentCommission, crLeadAgentTax As Decimal
        'End - PN 65531
        Dim vAllowPayNowOption As Object

        'Find item position in array
        Dim iPosInArray As Integer
        Dim bFlag As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnablePayNowOptions, v_vBranch:=g_iSourceID, r_vUnderwriting:=CStr(vAllowPayNowOption))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sErrMsg = "Failed to get Product Option"
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Dim temp_oCashDeposit As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oCashDeposit, sClassName:="iSIRPolicyCashDeposit.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oCashDeposit = temp_oCashDeposit
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sErrMsg = "Failed to get CashDeposit Instance"
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            bFlag = False
            For iListCount As Integer = lvwRenewalProcess.Items.Count To 1 Step -1
                If lvwRenewalProcess.Items.Item(iListCount - 1).Checked Then
                    For iVar As Integer = 0 To m_vRenewalPolicy.GetUpperBound(1)
                        If ListViewHelper.GetListViewSubItem(lvwRenewalProcess.Items.Item(iListCount - 1), 3).Text = CStr(m_vRenewalPolicy(5, iVar)).Trim() Then
                            iPosInArray = iVar
                            bFlag = True
                            Exit For
                        End If
                    Next
                End If
                If bFlag Then Exit For
            Next

            oCashDeposit.InsuranceFileCnt = gPMFunctions.ToSafeInteger(CStr(m_vRenewalPolicy(ACIRenewalPolicyCnt, iPosInArray)), 0)

            'oCashDeposit.PolicyIssueDate = DateTime.Parse(DateTimeHelper.ToString(DateTime.Now))
            oCashDeposit.PolicyIssueDate = DateTime.Now

            oCashDeposit.PrePayment = vAllowPayNowOption

            oCashDeposit.CoverFromDate = gPMFunctions.ToSafeDate(CStr(m_vRenewalPolicy(ACIRenewalCoverStartDate, iPosInArray)), #12/30/1899#)

            'Start - PN 65531
            m_lReturn = GetPremiumDetails(crGrossTotal:=crGrossTotal, crLeadAgentCommission:=crLeadAgentCommission, crLeadAgentTax:=crLeadAgentTax, lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(CStr(m_vRenewalPolicy(ACIRenewalPolicyCnt, iPosInArray)), 0))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sErrMsg = "GetPremiumDetails Failed"
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oCashDeposit.LeadAgentCommission = crLeadAgentCommission

            oCashDeposit.LeadAgentTax = crLeadAgentTax

            oCashDeposit.TotalPremium = crGrossTotal
            'End - PN 65531

            m_lReturn = oCashDeposit.Start

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sErrMsg = "CashDeposit.Start Failed"
                Return m_lReturn
            End If

            If oCashDeposit.OKClick Then

                m_lPaymentAccountID = oCashDeposit.PaymentAccountID

                m_iDebitAgainst = oCashDeposit.DebitAgainst

                m_vCreditTransactions = oCashDeposit.CreditTransactions
                'Start - PN 65531
                If m_lPaymentAccountID <= 0 Or (Not Information.IsArray(m_vCreditTransactions)) Or gPMFunctions.IsArrayEmpty(m_vCreditTransactions) Then
                    MessageBox.Show("Payment via Cash Deposit Failed", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    result = gPMConstants.PMEReturnCode.PMCancel
                End If
                'End - PN 65531
            Else
                result = gPMConstants.PMEReturnCode.PMCancel
            End If
            'Start - PN 65531

            oCashDeposit.Dispose()
            oCashDeposit = Nothing
            'End - PN 65531


        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sErrMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCashDeposit", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
        End Try
        Return result
    End Function
    Private Function CheckForCashDepositPaymentMethod(ByRef bCashDeposit As Boolean) As Integer
        'This method will check if any of the selected policy has Cash Depsoit as payment method.
        Dim result As Integer = 0
        Dim lPolicycnt As Integer
        Dim sPaymentMethod As String = ""
        Dim vResults As Object
        Dim iPosInArray, iSelectedItemCount As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            bCashDeposit = False
            iSelectedItemCount = 0
            For iListCount As Integer = lvwRenewalProcess.Items.Count To 1 Step -1
                If lvwRenewalProcess.Items.Item(iListCount - 1).Checked Then
                    iSelectedItemCount += 1
                    'Find item position in array
                    For iVar As Integer = 0 To m_vRenewalPolicy.GetUpperBound(1)
                        If ListViewHelper.GetListViewSubItem(lvwRenewalProcess.Items.Item(iListCount - 1), 3).Text = CStr(m_vRenewalPolicy(7, iVar)).Trim() Then
                            iPosInArray = iVar
                            Exit For
                        End If
                    Next
                    lPolicycnt = CInt(m_vRenewalPolicy(ACIRenewalPolicyCnt, iPosInArray))

                    m_lReturn = g_oBusiness.GetPaymentMethod(v_lInsuranceFileCnt:=lPolicycnt, r_vResults:=vResults)

                    sPaymentMethod = CStr(vResults(0, 0))
                    If sPaymentMethod = "CashDeposit" Then
                        bCashDeposit = True
                    End If
                End If
            Next

            If (iSelectedItemCount > 1) And bCashDeposit Then
                bCashDeposit = True
            ElseIf (iSelectedItemCount = 1) And bCashDeposit Then
                bCashDeposit = False
            ElseIf Not bCashDeposit Then
                bCashDeposit = False
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckForCashDepositPaymentMethod Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckForCashDepositPaymentMethod", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    'Start - PN 61554
    Private Function GetPremiumDetails(ByRef crGrossTotal As Decimal, ByRef crLeadAgentCommission As Decimal, ByRef crLeadAgentTax As Decimal, ByVal lInsuranceFileCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vResult(,) As Object
        Dim sErrMsg As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim g_oRenewal As bSIRRenewalProcess.Business
            Dim sAgentType As String = ""

            Dim temp_g_oRenewal As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oRenewal, "bSIRRenewalProcess.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            g_oRenewal = temp_g_oRenewal

            lReturn = g_oRenewal.GetPolicyGrossTotal(v_lInsuranceFileCnt:=lInsuranceFileCnt, r_vResults:=vResult)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sErrMsg = "Failed to Get GetPolicyGrossTotal"
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If Information.IsArray(vResult) Then

                crGrossTotal = gPMFunctions.ToSafeCurrency(CStr(vResult(4, 0)), 0)
            End If

            vResult = Nothing

            lReturn = g_oRenewal.GetAgentCommission(lInsuranceFileCnt, vResult)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sErrMsg = "Failed to Get GetAgentCommission"
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If Information.IsArray(vResult) Then

                crLeadAgentCommission = gPMFunctions.ToSafeCurrency(CStr(vResult(6, 0)))

                crLeadAgentTax = gPMFunctions.ToSafeCurrency(CStr(vResult(16, 0)))
            End If

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sErrMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="GetPremiumDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
        Finally

        End Try
        Return result
    End Function

    'End - Prakash - PN 61554
    'End - Prakash - WPR85_Paralleling

    Private Sub lvwRenewalProcess_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lvwRenewalProcess.MouseUp
        If e.Button <> MouseButtons.Right Then
            Exit Sub
        End If
        '  Ctx_mnuRenewalProcess.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
    End Sub
    Private Function CancelMTAQuotes(ByVal v_lInsuranceFileCnt As Long,
                                                ByVal v_lInsuranceFolderCnt As Long,
                                                            ByVal v_lPartyCnt As Long) As Long
        Const kMethodName As String = "CancelMTAQuotes"

        Dim lReturn As Long
        Dim oRenewal As Object
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = g_oObjectManager.GetInstance(
                oObject:=oRenewal,
                sClassName:="bSIRRenewal.Business",
                vInstanceManager:=PMGetViaClientManager)

            ' apply policy discount
            lReturn = oRenewal.CancelMTAQuotes(v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                                                                   v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt,
                                                                   v_lPartyCnt:=v_lPartyCnt)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, " Failed", gPMConstants.PMELogLevel.PMLogError)
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=CancelMTAQuotes, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            Return result

        Finally

            If Not oRenewal Is Nothing Then
                oRenewal.Dispose()
                oRenewal = Nothing
            End If

        End Try
    End Function
    'Start   Written Status.doc
    Private Sub cmdWrite_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdWrite.Click
        If lvwRenewalProcess.Items.Count > 0 Then
            Call mnuRenewalProcessWrite_Click(sender, e)
        End If
    End Sub
    'End   Written Status.doc

    Private Sub mnuRenewalProcessWrite_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRenewalProcessWrite.Click
        Const kColInsuranceRef As Integer = 3
        Dim lIcon As Integer
        Dim bChanged As Boolean
        Dim lCount As Integer
        Dim bContinue As Boolean
        Dim bReDisplay As Boolean
        Dim lIsQuoted As Integer
        Dim sLockedBy As String
        Dim bLocked As Boolean
        Dim lIndex As Integer
        Dim lListCount As Integer

        Dim sFailureMessage As String
        Dim sReportText As String

        Dim sRenStatusDesc As String
        Dim lRenStatusTypeID As Integer

        Dim sMsgBox As String
        Dim lYesNo As Integer
        Dim lNumberTick As Integer

        Dim sNewPolicyRef As String
        Dim dtNewStartDate As Date
        Dim dtNewEndDate As Date
        Dim vListViewUpdate(0, 1) As Object

        Dim lInvalidTMPCount As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'disable menu
            mnuRenewalProcess.Enabled = False

            'check to see if we have any ticked
            m_lReturn = ListViewIsTick(lvwRenewalProcess, lNumberTick)

            Select Case m_lReturn
                Case gPMConstants.PMELogLevel.PMLogError
                    MsgBox("Warning! An error has occurred whilst trying to check for selected items in list", vbInformation + vbOKOnly, Me.Text)

                Case gPMConstants.PMEReturnCode.PMFalse
                    MsgBox("Warning! Please select an item from the list", vbInformation + vbOKOnly, Me.Text)

            End Select

            Me.stbMain.Items.Item("Message").Text = "Processing renewal write please wait..."
            'Me.stbMain.Refresh()
            lListCount = lvwRenewalProcess.Items.Count
            'step backwards so we can remove processed items from list
            'For lCount = lListCount To 1 Step -1
            Dim ilvCount As Integer
            For ilvCount = 0 To lvwRenewalProcess.Items.Count - 1
                If lvwRenewalProcess.Items(ilvCount).Checked = True Then
                    bLocked = False
                    bContinue = False
                    sMsgBox = ""
                    lYesNo = vbNo

                    'get array position
                    lIndex = lvwRenewalProcess.Items(ilvCount).Tag

                    'check to see if current renewal status is ok for Written
                    m_lReturn = CheckRenewalStatus(lIndex, "WRITE", sMsgBox, lYesNo, lNumberTick)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MsgBox("CheckRenewalStatus() error", vbOKOnly, ACApp)
                        Exit For
                    Else
                        If sMsgBox <> "" Then
                            If lYesNo = vbYes Then
                                If MsgBox(sMsgBox, vbQuestion + vbYesNo, ACApp) = vbNo Then
                                    Exit For
                                End If
                            Else
                                MsgBox(sMsgBox, vbOKOnly + vbInformation, ACApp)
                            End If
                        Else
                            bContinue = True
                        End If
                    End If

                    If bContinue Then
                        'lock this renewal status count to stop others from processing it
                        m_lReturn = g_oBusiness.LockKey(v_sKeyName:=ACLockName,
                                    v_lKeyValue:=m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex),
                                    v_lUserID:=g_oObjectManager.UserID,
                                    r_sLockedBy:=sLockedBy)

                        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                            bLocked = True

                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                            m_lReturn = ChangePolicyDetail(v_vPolicy:=m_vRenewalPolicy,
                                                            v_lIndex:=lIndex,
                                                            r_sNewPolicyRef:=sNewPolicyRef,
                                                            r_dNewStartDate:=dtNewStartDate,
                                                            r_dNewExpiryDate:=dtNewEndDate,
                                                            r_bchanged:=bChanged)

                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                                    If MsgBox("Cancel was selected in Change Policy screen." & vbCrLf & "Do you want to continue with next selected policy?", vbQuestion + vbYesNo, ACApp) = vbNo Then
                                        Exit For
                                    End If
                                Else
                                    If MsgBox("Failed to change policy details." & vbCrLf & "Do you want to continue with next selected policy?", vbQuestion + vbYesNo, ACApp) = vbNo Then
                                        Exit For
                                    End If
                                End If
                            Else
                                If g_oBusiness.IsQuoted(v_lInsuranceFileCnt:=CLng(m_vRenewalPolicy(ACIRenewalPolicyCnt, lIndex)), r_lResult:=lIsQuoted) = gPMConstants.PMEReturnCode.PMTrue Then
                                    If lIsQuoted = gPMConstants.PMEReturnCode.PMTrue Then
                                        m_lReturn = ProcessWrite(v_vPolicy:=m_vRenewalPolicy,
                                                                    v_lIndex:=lIndex,
                                                                    v_bPolicyChanged:=bChanged,
                                                                    v_sNewPolicyNumber:=sNewPolicyRef,
                                                                    v_dNewStartDate:=dtNewStartDate,
                                                                    v_dNewEndDate:=dtNewEndDate,
                                                                    r_sFailureMessage:=sFailureMessage)
                                        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                            'did we fail in producing any of the document or creating work task
                                            sReportText = sReportText & vbCrLf & m_vRenewalPolicy(ACIRenewalInsuranceRef, lIndex) & " - Successful" & vbCrLf & sFailureMessage & vbCrLf

                                            'Set Status to written
                                            m_lReturn = g_oBusiness.SetRenewalStatusTypeID(v_lRenewalInsuranceFileCnt:=CLng(m_vRenewalPolicy(ACIRenewalPolicyCnt, lIndex)),
                                                                                v_lRenewalStatusTypeID:=PMBRenewalStatusTypeWrittenAwaitUpdate)

                                            'update listview with new status
                                            m_lReturn = SetListViewRenewalStatus(v_lArrayIndex:=lIndex,
                                                                                v_lSelectedIndex:=lCount,
                                                                                r_sFailureMessage:=sFailureMessage,
                                                                                v_sRenStatusDesc:="Written - Awaiting Update",
                                                                                v_lRenStatusTypeID:=PMBRenewalStatusTypeWrittenAwaitUpdate)

                                            'if we failed to change listview value directly then set flag to redisplay whole list
                                            If Not bReDisplay And m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                bReDisplay = True
                                            End If

                                            If bChanged Then
                                                vListViewUpdate(0, 0) = kColInsuranceRef
                                                vListViewUpdate(0, 1) = sNewPolicyRef
                                                m_lReturn = UpdateListView(v_oListView:=lvwRenewalProcess,
                                                                            v_vColumnIndex:=vListViewUpdate,
                                                                            v_lSelectedIndex:=lCount,
                                                                            v_lIcon:=lIcon)

                                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                    MsgBox("Failed to set the new Policy Number", vbOKOnly + vbCritical, Me.Text)
                                                End If
                                                m_vRenewalPolicy(ACIRenewalInsuranceRef, lIndex) = sNewPolicyRef
                                            End If
                                        Else
                                            sReportText = sReportText & vbCrLf & m_vRenewalPolicy(ACIRenewalInsuranceRef, lIndex) & " - " & sFailureMessage & vbCrLf
                                            If ToSafeInteger(m_vRenewalPolicy(ACIRenewalIsTrueMonthlyPolicy, lIndex)) = 1 Then
                                                lInvalidTMPCount = lInvalidTMPCount + 1
                                            End If
                                        End If  'ProcessWrite
                                    Else
                                        sReportText = sReportText & vbCrLf & m_vRenewalPolicy(ACIRenewalInsuranceRef, lIndex) & " - Unquoted" & vbCrLf

                                        m_lReturn = GetRenewalStatusType(v_sRenStatusCode:="ManReview", r_sDesc:=sRenStatusDesc, r_lRenewalStatusTypeID:=lRenStatusTypeID)
                                        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                            'reset status to awaiting manual review
                                            If g_oBusiness.SetRenewalStatusTypeID(v_lRenewalInsuranceFileCnt:=CLng(m_vRenewalPolicy(ACIRenewalPolicyCnt, lIndex)), v_lRenewalStatusTypeID:=lRenStatusTypeID) <> gPMConstants.PMEReturnCode.PMTrue Then
                                                If MsgBox("Failed to set renewal status to awaiting manual review." & vbCrLf & "Do you want to continue with next selected policy?", vbQuestion + vbYesNo, Me.Text) = vbNo Then
                                                    Exit For
                                                End If
                                            End If
                                        Else
                                            If MsgBox("Failed to get renewal status type details." & vbCrLf & "Do you want to continue with next selected policy?", vbQuestion + vbYesNo, Me.Text) = vbNo Then
                                                Exit For
                                            End If
                                        End If  'GetRenewalStatusType
                                    End If  'lIsQuoted
                                Else
                                    If MsgBox("Failed to check quote status." & vbCrLf & "Do you want to continue with next selected policy?", vbQuestion + vbYesNo, ACApp) = vbNo Then
                                        Exit For
                                    End If
                                End If  'IsQuoted

                                'unlock renewal policy
                                If g_oBusiness.UnLockKey(v_sKeyName:=ACLockName, v_lKeyValue:=CLng(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)), v_lUserID:=g_oObjectManager.UserID) <> gPMConstants.PMEReturnCode.PMTrue Then
                                    MsgBox("Failed to unlock KeyName: " & ACLockName & vbCrLf & "KeyValue: " & m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex) & vbCrLf & "Process terminate.", vbInformation + vbOKOnly, ACApp)
                                    Exit For
                                End If

                                bLocked = False  'so we won't try to unlock it later

                            End If  'ChangePolicyDetail
                        Else
                            If sLockedBy = "ERROR" Then
                                If MsgBox("Failed to lock policy for, Insurance Folder count : " & m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex) & vbCrLf & "Do you want to process next selected policy?", vbQuestion + vbYesNo, ACApp) = vbNo Then
                                    Exit For
                                End If
                            Else
                                If MsgBox("Current policy " & m_vRenewalPolicy(ACIRenewalInsuranceRef, lIndex) & " is being locked by " & sLockedBy & vbCrLf & "Do you want to process next selected policy?", vbQuestion + vbYesNo, ACApp) = vbNo Then
                                    Exit For
                                End If
                            End If
                        End If   'LockKey()
                    End If   'CheckRenewalStatus()
                End If   'lvwRenewalProcess.ListItems(lCount).Checked = True
            Next ilvCount

            If lInvalidTMPCount > 0 Then
                MsgBox(lInvalidTMPCount & " anniversary renewal/s could not be processed." &
                                        " Anniversary Renewals cannot be accepted until " &
                                        " the last monthly cycle has been accepted",
                                            vbInformation, "True Monthly Policy Validation")
            End If

            If sReportText <> "" Then
                If m_sGenerateReport = "1" Then
                    If RenewalReport(v_sReportTitle:="Renewal Write", v_sReportText:=sReportText) <> gPMConstants.PMEReturnCode.PMTrue Then
                        MsgBox("Failed to do Renewal report", vbInformation + vbOKOnly, ACApp)
                    End If
                End If
            End If

            If bReDisplay Then
                m_lReturn = BusinessToInterface()
            End If

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="mnuRenewalProcess_Click", r_lFunctionReturn:=m_lReturn, excep:=excep)

        Finally

            'unlock current policy
            If bLocked Then
                If g_oBusiness.UnLockKey(v_sKeyName:=ACLockName, v_lKeyValue:=CLng(m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex)), v_lUserID:=g_oObjectManager.UserID) <> gPMConstants.PMEReturnCode.PMTrue Then
                    MsgBox("Failed to unlock KeyName: " & ACLockName & vbCrLf & "KeyValue: " & m_vRenewalPolicy(ACIRenewalInsuranceFolder, lIndex) & vbCrLf & "Process terminate.", vbInformation + vbOKOnly, ACApp)
                End If
            End If

            lvwRenewalProcess.Refresh()
            Me.stbMain.Items.Item("Message").Text = "Ready"
            Call DisplayListViewCount()

            mnuRenewalProcess.Enabled = True

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
    End Sub

    'Start - Sankar - Tech Spec -   - Written Status.doc
    Public Function ProcessWrite(ByVal v_vPolicy(,) As Object,
                                    ByVal v_lIndex As Long,
                                    ByVal v_bPolicyChanged As Boolean,
                                    ByVal v_sNewPolicyNumber As String,
                                    ByVal v_dNewStartDate As Date,
                                    ByVal v_dNewEndDate As Date,
                                    ByRef r_sFailureMessage As String) As Long

        'Error return for renewal that has already been accepted
        Const kMethodName As String = "ProcessWrite"
        'Sankar - PN 71528
        Const kPROCESSTYPESDOCSDEBITNOTE As Integer = 3
        Const kPROCESSTYPESDOCSSCHEDULE As Integer = 4
        Const kPROCESSTYPESDOCSCERTIFICATE As Integer = 5

        Dim lOldPolicyCnt As Integer
        Dim lRenewalPolicyCnt As Integer
        Dim lRenewalStatusCnt As Integer
        Dim lInsuranceFolder As Integer
        Dim lPartyCnt As Integer
        Dim lAnniversaryCopy As Integer
        Dim bIsTrueMonthlyPolicy As Boolean
        Dim vValidationResults(,) As Object
        Dim bAcceptIsValid As Boolean

        Dim bProduceSchedule As Boolean
        Dim bProduceDebitNote As Boolean
        Dim bProduceCertificate As Boolean
        Dim vPrintOptions(,) As Object
        Dim lProductID As Integer
        Dim lTaskDays As Integer
        Dim lUserGroupId As Integer
        Dim lTaskGroupId As Integer
        Dim vResult As Object
        Dim dtDueDate As Date
        Dim lTaskInstanceCnt As Integer
        Dim lRenProcessTaskID As Integer
        'Sankar - PN 71528
        Dim oGetDocument As Object
        Dim vKeyArray(0 To 1, 0 To 5)

        Dim result As Integer = 0

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ProcessWrite = gPMConstants.PMEReturnCode.PMTrue
            lOldPolicyCnt = v_vPolicy(ACIRenewalLivePolicyCnt, v_lIndex)
            lRenewalStatusCnt = v_vPolicy(ACIRenewalStatusCnt, v_lIndex)
            lRenewalPolicyCnt = v_vPolicy(ACIRenewalPolicyCnt, v_lIndex)
            lInsuranceFolder = v_vPolicy(ACIRenewalInsuranceFolder, v_lIndex)
            lPartyCnt = v_vPolicy(ACIRenewalInsuranceHolder, v_lIndex)
            lAnniversaryCopy = ToSafeInteger(v_vPolicy(ACIRenewalAnniversaryCopy, v_lIndex))
            bIsTrueMonthlyPolicy = (ToSafeInteger(v_vPolicy(ACIRenewalIsTrueMonthlyPolicy, v_lIndex) = 1))
            lProductID = ToSafeInteger(v_vPolicy(ACIRenewalProductId, v_lIndex), 0)

            If lAnniversaryCopy = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = g_oBusiness.ValidateAcceptTMPIsValidAction(
                                        v_lInsuranceFileCnt:=lRenewalPolicyCnt,
                                        v_sInsuranceRef:=m_sInsuranceRef,
                                        r_vResults:=vValidationResults)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "ValidateAcceptTMPIsValidAction Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                If IsArray(vValidationResults) Then
                    If ToSafeInteger(vValidationResults(0, 0), 0) = 0 Then
                        bAcceptIsValid = False
                    Else
                        bAcceptIsValid = True
                    End If
                Else
                    bAcceptIsValid = False
                End If

                If bAcceptIsValid = False Then
                    r_sFailureMessage = "Anniversary renewal can not be written until the last monthly cycle has been accepted."
                    ProcessWrite = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If
            Else
                ProcessWrite = gPMConstants.PMEReturnCode.PMTrue
            End If

            If v_bPolicyChanged Then
                m_lReturn = g_oBusiness.WriteRenewal(v_lOldInsuranceFileCnt:=lOldPolicyCnt,
                                                    v_lNewInsuranceFileCnt:=lRenewalPolicyCnt,
                                                    v_lRenewalStatusCnt:=lRenewalStatusCnt,
                                                    v_sNewPolicyRef:=v_sNewPolicyNumber,
                                                    v_dNewStartDate:=v_dNewStartDate,
                                                    v_dNewExpiryDate:=v_dNewEndDate,
                                                    r_sFailureMessage:=r_sFailureMessage)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "WriteRenewal Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                v_vPolicy(ACIRenewalInsuranceRef, v_lIndex) = v_sNewPolicyNumber

            Else
                m_lReturn = g_oBusiness.WriteRenewal(v_lOldInsuranceFileCnt:=lOldPolicyCnt,
                                                v_lNewInsuranceFileCnt:=lRenewalPolicyCnt,
                                                v_lRenewalStatusCnt:=lRenewalStatusCnt,
                                                r_sFailureMessage:=r_sFailureMessage)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "WriteRenewal Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            'create renewal acceptance event
            m_lReturn = g_oBusiness.CreateEvent(v_vEventCnt:=0,
                                                v_vPartyCnt:=lPartyCnt,
                                                v_vInsuranceFolderCnt:=lInsuranceFolder,
                                                v_vInsuranceFileCnt:=lRenewalPolicyCnt,
                                                v_vEventType:=5,
                                                v_vUserId:=g_oObjectManager.UserID,
                                                v_vEventDate:=DateTime.Today,
                                                v_vDescription:="Written Renewal - " & v_vPolicy(ACIRenewalInsuranceRef, v_lIndex))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sFailureMessage = "Failed to create an event for renewal written"
                RaiseError(kMethodName, "CreateEvent Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Create WM task as-per NB
            'TODO: Get Product options for Written WM task
            'TODO: Create WM task x number of days after cover start date of policy depending on product option

            m_lReturn = g_oBusiness.GetValueFromTable(v_sTableName:="Product",
                                            v_vReturnColumn:="written_task_manager_days",
                                            v_sKeyColumn:="product_id",
                                            v_sKeyValue:=lProductID,
                                            v_iDataType:=gPMConstants.PMEDataType.PMInteger,
                                            r_vResult:=vResult)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetValueFromTable Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lTaskDays = ToSafeInteger(vResult)

            'Start - Sankar - PN 71562
            '    dtDueDate = ToSafeDate(v_vPolicy(ACIRenewalCoverStartDate, v_lIndex))
            '    dtDueDate = DateAdd("d", lTaskDays, dtDueDate)
            If ToSafeDate(v_vPolicy(ACIRenewalCoverStartDate, v_lIndex)) < Now Then
                dtDueDate = DateAdd("d", lTaskDays, Now)
            Else
                dtDueDate = DateAdd("d", lTaskDays, ToSafeDate(v_vPolicy(ACIRenewalCoverStartDate, v_lIndex)))
            End If
            'End - Sankar - PN 71562

            m_lReturn = g_oBusiness.GetValueFromTable(v_sTableName:="Product",
                                            v_vReturnColumn:="written_rem_user_group",
                                            v_sKeyColumn:="product_id",
                                            v_sKeyValue:=lProductID,
                                            v_iDataType:=gPMConstants.PMEDataType.PMInteger,
                                            r_vResult:=vResult)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetValueFromTable Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            lUserGroupId = ToSafeInteger(vResult)

            If lUserGroupId = 0 Then
                m_lReturn = g_oBusiness.GetValueFromTable(v_sTableName:="PMUser_Group",
                                            v_vReturnColumn:="pmuser_group_id",
                                            v_sKeyColumn:="code",
                                            v_sKeyValue:="SYSADMIN",
                                            v_iDataType:=gPMConstants.PMEDataType.PMString,
                                            r_vResult:=vResult)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "GetValueFromTable Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                lUserGroupId = ToSafeInteger(vResult)
            End If

            m_lReturn = g_oBusiness.GetValueFromTable(v_sTableName:="Product",
                                            v_vReturnColumn:="written_rem_task_group",
                                            v_sKeyColumn:="product_id",
                                            v_sKeyValue:=lProductID,
                                            v_iDataType:=gPMConstants.PMEDataType.PMInteger,
                                            r_vResult:=vResult)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetValueFromTable Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            lTaskGroupId = ToSafeInteger(vResult)

            If lTaskGroupId = 0 Then
                m_lReturn = g_oBusiness.GetValueFromTable(v_sTableName:="PMWrk_Task_Group",
                                            v_vReturnColumn:="pmwrk_task_group_id",
                                            v_sKeyColumn:="code",
                                            v_sKeyValue:="UWRENEWAL",
                                            v_iDataType:=gPMConstants.PMEDataType.PMString,
                                            r_vResult:=vResult)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "GetValueFromTable Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                lTaskGroupId = ToSafeInteger(vResult)
            End If

            m_lReturn = g_oBusiness.GetValueFromTable(v_sTableName:="PMWrk_Task",
                                            v_vReturnColumn:="pmwrk_task_id",
                                            v_sKeyColumn:="code",
                                            v_sKeyValue:="RENPROCESS",
                                            v_iDataType:=gPMConstants.PMEDataType.PMString,
                                            r_vResult:=vResult)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetValueFromTable Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            lRenProcessTaskID = ToSafeInteger(vResult)

            m_lReturn = CreateWorkManagerTask(v_lPMWrkTaskGroupID:=lTaskGroupId,
                                         v_lPMWrkTaskID:=lRenProcessTaskID,
                                         v_sCustomer:=v_vPolicy(ACIRenewalShortname, v_lIndex),
                                         v_dtTaskDueDate:=dtDueDate,
                                         v_lPMUserGroupID:=lUserGroupId,
                                         v_sDescription:="Written Renewal - " & v_vPolicy(ACIRenewalInsuranceRef, v_lIndex),
                                         v_iTaskStatus:=0,
                                         v_iIsUrgent:=1,
                                         r_lPMWrkTaskInstanceCnt:=lTaskInstanceCnt,
                                         v_iIsVisible:=gPMConstants.PMEReturnCode.PMTrue)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                r_sFailureMessage = "Failed to create work manager task"
                RaiseError(kMethodName, "CreateWorkManagerTask Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = g_oBusiness.GetProdPrintOptions(lProductID, vPrintOptions)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "GetProdPrintOptions Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If IsArray(vPrintOptions) Then
                bProduceSchedule = ToSafeBoolean(vPrintOptions(0, 0))
                bProduceCertificate = ToSafeBoolean(vPrintOptions(1, 0))
                bProduceDebitNote = ToSafeBoolean(vPrintOptions(2, 0))
            End If

            'Start - Sankar - PN 71528
            If bProduceCertificate Or bProduceSchedule Or bProduceDebitNote Then
                m_lReturn = g_oObjectManager.GetInstance(
                                oObject:=oGetDocument,
                                sClassName:="iPMUGetDocument.Interface_Renamed",
                                vInstanceManager:=gPMConstants.PMGetLocalInterface)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "", gPMConstants.PMELogLevel.PMLogError)
                End If

                m_lReturn = oGetDocument.SetProcessModes(
                                            vProcessMode:=gPMConstants.PMEComponentAction.PMEdit,
                                            vTransactionType:="WRN",
                                            vEffectiveDate:=v_vPolicy(ACIRenewalCoverStartDate, v_lIndex))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureMessage = "Failed to generate document"
                    Return result
                End If

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameInsFolderCnt
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = lInsuranceFolder

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNamePartyCnt
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = lPartyCnt

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameInsFileCnt
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = lRenewalPolicyCnt

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = "Product_id"
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = lProductID
            End If

            If bProduceSchedule Then
                'Generate schedule document.
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = "document_id"
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = kPROCESSTYPESDOCSSCHEDULE

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = "short_code"
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = "schedule"

                m_lReturn = oGetDocument.SetKeys(vKeyArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureMessage = "Failed to generate schedule document"
                    Return result
                End If

                m_lReturn = oGetDocument.Start
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureMessage = "Failed to generate schedule document"
                    Return result
                End If
            End If

            If bProduceCertificate Then
                'Generate certificate document.
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = "document_id"
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = kPROCESSTYPESDOCSCERTIFICATE

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = "short_code"
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = "certificate"

                m_lReturn = oGetDocument.SetKeys(vKeyArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureMessage = "Failed to generate certificate document"
                    Return result
                End If

                m_lReturn = oGetDocument.Start
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureMessage = "Failed to generate certificate document"
                    Return result
                End If
            End If

            If bProduceDebitNote Then
                'Generate debit note
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = "document_id"
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = kPROCESSTYPESDOCSDEBITNOTE

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = "short_code"
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = "debitnote"

                m_lReturn = oGetDocument.SetKeys(vKeyArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureMessage = "Failed to generate debitnote document"
                    Return result
                End If

                m_lReturn = oGetDocument.Start
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureMessage = "Failed to generate debitnote document"
                    Return result
                End If
            End If
            'End - Sankar - PN 71528

            'GoTo Final

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=ProcessWrite, excep:=excep)

            ' If you want to rollback a transaction or something, do it here
            Return result

        End Try
    End Function
    Private Function CreateWorkManagerTask(ByVal v_lPMWrkTaskGroupID As Long,
                                            ByVal v_lPMWrkTaskID As Long,
                                            ByVal v_sCustomer As String,
                                            ByVal v_dtTaskDueDate As Date,
                                            ByVal v_lPMUserGroupID As Long,
                                            ByVal v_sDescription As String,
                                            ByVal v_iTaskStatus As Integer,
                                            ByVal v_iIsUrgent As Integer,
                                            ByRef r_lPMWrkTaskInstanceCnt As Long,
                                            ByVal v_iIsVisible As Integer) As Long

        Const kMethodName As String = "CreateWorkManagerTask"
        Dim oTaskInstance As Object

        CreateWorkManagerTask = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = g_oObjectManager.GetInstance(
                                        oObject:=oTaskInstance,
                                        sClassName:="bPMWrkTaskInstance.TaskControl",
                                        vInstanceManager:=PMGetLocalBusiness)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "Failed to get instance of bPMWrkTaskInstance.TaskControl", gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lReturn = oTaskInstance.CreateNew(
                                     v_lPMWrkTaskGroupID:=v_lPMWrkTaskGroupID,
                                     v_lPMWrkTaskID:=v_lPMWrkTaskID,
                                     v_sCustomer:=v_sCustomer,
                                     v_dtTaskDueDate:=v_dtTaskDueDate,
                                     v_lPMUserGroupID:=v_lPMUserGroupID,
                                     v_sDescription:=v_sDescription,
                                     v_iTaskStatus:=v_iTaskStatus,
                                     v_iIsUrgent:=v_iIsUrgent,
                                     r_lPMWrkTaskInstanceCnt:=r_lPMWrkTaskInstanceCnt,
                                     v_iIsVisible:=v_iIsVisible)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "Failed To create the Written task", gPMConstants.PMELogLevel.PMLogError)
        End If

        GoTo Finally_Renamed

Catch_Renamed:
        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=CreateWorkManagerTask)
Finally_Renamed:

        If Not (oTaskInstance Is Nothing) Then
            oTaskInstance.Dispose()
            oTaskInstance = Nothing
        End If

    End Function
    Private Function CheckJobBatchRenewalInProcess(ByVal v_sKey As String) As Integer

        Const kMethodName As String = "CheckJobBatchRenewalInProcess"
        Dim iReturn As Integer
        Dim oRenewal As Object
        Dim bIsJobBatchRenewalInProcess As Boolean

        Try

            CheckJobBatchRenewalInProcess = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oRenewal As Object
            m_lReturn = g_oObjectManager.GetInstance(
                oObject:=temp_oRenewal,
                sClassName:="bSIRRenewal.Business",
                vInstanceManager:=PMGetViaClientManager)
            oRenewal = temp_oRenewal

            ' apply policy discount
            iReturn = oRenewal.CheckJobBatchRenewalInProcess(v_sKey:=v_sKey,
                                                        r_bIsJobBatchRenewalInProcess:=bIsJobBatchRenewalInProcess)
            If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If bIsJobBatchRenewalInProcess Then
                CheckJobBatchRenewalInProcess = gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(
             v_sClass:=ACClass,
             v_sMethod:=kMethodName,
             r_lFunctionReturn:=CheckJobBatchRenewalInProcess)

            ' If you want to rollback a transaction or something, do it here
        Finally
            If Not oRenewal Is Nothing Then
                m_lReturn = oRenewal.Terminate
                oRenewal = Nothing
            End If

        End Try

    End Function
End Class
