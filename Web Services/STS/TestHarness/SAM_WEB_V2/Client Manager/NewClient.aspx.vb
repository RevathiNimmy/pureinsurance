
Partial Class New_Business_NewClient
    Inherits System.Web.UI.Page

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If ddlParty.SelectedItem.Text = "Personal Client" Then
            Session("CLIENTTYPE") = "PC"
        ElseIf ddlParty.SelectedItem.Text = "Corporate Client" Then
            Session("CLIENTTYPE") = "CC"
        Else
            Session("CLIENTTYPE") = "GC"
        End If
        Response.Redirect("AmendClient1.aspx")

    End Sub
End Class
