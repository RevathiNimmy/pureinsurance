
Partial Class exceptionDummy
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Throw New System.Exception("This is a test error.")
    End Sub
End Class
