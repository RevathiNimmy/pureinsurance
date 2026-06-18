
Partial Class UIIC_demo_Login
    Inherits System.Web.UI.Page
    Protected Sub LoginButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If Login1.UserName = "sirius" AndAlso Login1.Password = "sirius" Then
            Response.Redirect("homepage.aspx")
        Else

        End If

    End Sub
End Class
