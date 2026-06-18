Imports System.ComponentModel
Imports SharedFiles
Friend Class TaskLogin
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private Const vbFormCode As Integer = 0
    Public Property Status() As Integer
        Get
            ' Standard Property.
            ' Return the interface exit status.
            Return m_lStatus
        End Get
        Set(value As Integer)
            m_lStatus = value
        End Set
    End Property
    Private Sub LoginDialogForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        txtUserName.Text = Username

    End Sub
    Private _Username As String
    Public Property Username() As String
        Get
            Return _Username
        End Get
        Set(value As String)
            _Username = value
        End Set
    End Property
    Private _Password As String
    Public ReadOnly Property Password() As String
        Get
            Return _Password
        End Get
    End Property



    Private Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        m_lStatus = gPMConstants.PMEReturnCode.PMOK
        _Username = txtUserName.Text
        _Password = txtPassword.Text


        Me.Hide()

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        Me.Close()
    End Sub


    Private Sub TaskLogin_Closing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
        If UnloadMode <> vbFormCode Then
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        End If

    End Sub
End Class