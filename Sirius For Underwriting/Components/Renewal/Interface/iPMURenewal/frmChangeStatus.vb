Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms

Imports SharedFiles
Partial Friend Class frmChangeStatus
    'Public Class frmChangeStatus
    Inherits System.Windows.Forms.Form

    Private Const ACClass As String = "frmFilterRenewal"

    Private m_lStatus As Integer

    Private m_lRenewalStatusTypeID As Integer

    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property

    Public ReadOnly Property RenewalStatusTypeID() As Integer
        Get
            Return m_lRenewalStatusTypeID
        End Get
    End Property

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        Me.Hide()

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        m_lRenewalStatusTypeID = cboRenewalStatusType.ItemId

        m_lStatus = gPMConstants.PMEReturnCode.PMOK

        Me.Hide()

    End Sub

    Private Sub Form_Initialize_Renamed()
        'Start Renuka PN 61436
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        'End Renuka PN 61436
        iPMFunc.ShowFormInTaskBar_Attach()

    End Sub

    Private Sub frmChangeStatus_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        'Start Renuka PN 61436
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        'End Renuka PN 61436

        cboRenewalStatusType.FirstItem = ""
        iPMFunc.ShowFormInTaskBar_Detach()
    End Sub
    'Start Renuka PN 61436
    Private Sub frmChangeStatus_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
    End Sub
    'End Renuka PN 61436
End Class