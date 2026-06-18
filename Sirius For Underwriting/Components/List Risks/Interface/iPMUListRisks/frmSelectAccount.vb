Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmSelectAccount
    Inherits System.Windows.Forms.Form

    Public SelectAgent As Boolean
    Public OK As Boolean

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        OK = False
        Me.Hide()
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        OK = True
        SelectAgent = optSelectAccount(1).Checked
        Me.Hide()
    End Sub


    Private Sub frmSelectAccount_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        optSelectAccount(0).Checked = True
        optSelectAccount(1).Checked = False
        iPMFunc.CenterForm(Me)
    End Sub
End Class