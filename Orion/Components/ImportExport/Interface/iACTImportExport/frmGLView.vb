Public Class frmGLView
    Private sfilename As String
    Private sTitle As String
    Public WriteOnly Property FileName() As String
        Set(ByVal s_vfilename As String)
            sfilename = s_vfilename
        End Set
    End Property
    Public WriteOnly Property Title() As String
        Set(ByVal s_vsTitle As String)
            sTitle = s_vsTitle
        End Set
    End Property

    Private Sub cmdOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOk.Click
        Me.Close()
    End Sub

    Private Sub frmGLView_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = sTitle
        WbrowGL.Navigate(sfilename)
    End Sub


End Class