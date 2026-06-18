
Partial Class MTR_Reinstatement
    Inherits System.Web.UI.Page

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("ListPolicyVersions.aspx")
    End Sub

    Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If (IsDate(txtMTADate.Text)) Then
            Session("MTADATE") = txtMTADate.Text
            Session("MTAREASON") = "OTHER"
            Session("MTAISPERMANENT") = "PERMANENT"
            Response.Redirect("PolicyHeader.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            txtMTADate.Text = Today.Date
        End If
    End Sub
End Class
