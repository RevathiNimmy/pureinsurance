Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmPostingPeriod
    Inherits System.Windows.Forms.Form
    'Developer Guide No. 7
    Private Const vbFormCode As Integer = 0
    Public m_vOpenPeriodArray(,) As Object
    Public m_lPostingPeriod As Integer
    Public Status As gPMConstants.PMEReturnCode

    Public WriteOnly Property OpenPeriodArray() As Object(,)
        Set(ByVal Value(,) As Object)
            m_vOpenPeriodArray = Value
        End Set
    End Property
    Public ReadOnly Property PostingPeriod() As Integer
        Get
            Return m_lPostingPeriod
        End Get
    End Property


    ' ***************************************************************** '
    '                             EVENTS
    ' ***************************************************************** '
    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        m_lPostingPeriod = 0
        Status = gPMConstants.PMEReturnCode.PMCancel
        Me.Hide()
    End Sub

    Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
        If cboPostingPeriod.SelectedIndex < 0 Then
            MessageBox.Show("Please Select Posting Period", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        If cboPostingPeriod.Items.Count > 0 Then
            m_lPostingPeriod = VB6.GetItemData(cboPostingPeriod, cboPostingPeriod.SelectedIndex)
            Status = gPMConstants.PMEReturnCode.PMOK
        End If
        Me.Hide()
    End Sub

    Private Sub cboPostingPeriod_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPostingPeriod.DoubleClick
        cmdOk_Click(cmdOk, New EventArgs())
    End Sub


    ' ***************************************************************** '
    '                           FORM EVENTS
    ' ***************************************************************** '

    Private Sub frmPostingPeriod_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load


        m_lPostingPeriod = 0
        Status = gPMConstants.PMEReturnCode.PMCancel
        Dim cboPostingPeriod_NewIndex As Integer = -1
        If Information.IsArray(m_vOpenPeriodArray) Then

            cboPostingPeriod.Items.Clear()
            For vItem As Integer = 0 To m_vOpenPeriodArray.GetUpperBound(1)
                cboPostingPeriod_NewIndex = cboPostingPeriod.Items.Add(CDate(m_vOpenPeriodArray(2, vItem)).ToString("MMMM yyyy"))
                VB6.SetItemData(cboPostingPeriod, cboPostingPeriod_NewIndex, CInt(m_vOpenPeriodArray(0, vItem)))
            Next
            cboPostingPeriod.SelectedIndex = cboPostingPeriod_NewIndex

        End If

    End Sub

    Private Sub frmPostingPeriod_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        If UnloadMode <> vbFormCode Then
            Status = gPMConstants.PMEReturnCode.PMCancel
        End If
        eventArgs.Cancel = Cancel <> 0
    End Sub
End Class