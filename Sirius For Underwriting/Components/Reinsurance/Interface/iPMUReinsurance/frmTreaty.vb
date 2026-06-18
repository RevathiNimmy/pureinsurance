Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
'Developer Guide No 129. 
'Start
Imports SharedFiles
Imports Artinsoft.VB6.Utils
'End

Partial Friend Class frmTreaty
    Inherits System.Windows.Forms.Form

    Public Code As String = ""
    Public TreatyId As Integer
    'Developer Guide No.7
    Private Const vbFormCode As Integer = 0
    Public Status As gPMConstants.PMEReturnCode
    Private m_sTransactionType As String = ""

    ' ***************************************************************** '
    '                             EVENTS
    ' ***************************************************************** '
    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        Status = gPMConstants.PMEReturnCode.PMCancel
        'Developer Guide No. 231
        Me.Hide()
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        Status = gPMConstants.PMEReturnCode.PMOK

        TreatyId = cboTreaty.ItemId
        Code = cboTreaty.ItemCaption
        'Developer Guide No. 231
        Me.Hide()
    End Sub

    ' ***************************************************************** '
    '                           FORM EVENTS
    ' ***************************************************************** '

    Private Sub frmTreaty_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        'Developer Guide No. 220
        cboTreaty.FirstItem = ""
        ' Check how we should label the "treaty"
        If m_sTransactionType = "TX" Then
            cboTreaty.WhereClause = "reinsurance_type_id IN (Select reinsurance_type_id FROM Reinsurance_Type WHERE code = 'XOL')"
            cboTreaty.RefreshList()
        ElseIf m_sTransactionType = "T" Then
            cboTreaty.WhereClause = "reinsurance_type_id IN (Select reinsurance_type_id FROM Reinsurance_Type WHERE code='QUO' or Code='001' or code='002' or code='003')"
            cboTreaty.RefreshList()
        End If

    End Sub

    Private Sub frmTreaty_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        If UnloadMode <> vbFormCode Then
            Status = gPMConstants.PMEReturnCode.PMCancel
        End If
        eventArgs.Cancel = Cancel <> 0
    End Sub



    Public Property TransactionType() As String
        Get
            Return m_sTransactionType
        End Get
        Set(ByVal Value As String)

            m_sTransactionType = CStr(Value)
        End Set
    End Property
End Class