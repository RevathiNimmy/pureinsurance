
Partial Class PolicyRenewal_wfrmRenewalAmendment
    Inherits System.Web.UI.Page

    Protected Sub btnAmend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAmend.Click
        Session("Process") = "Amend"
        Response.Redirect("wfrmFilterRenewals.aspx")
    End Sub

    Protected Sub btnAccept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        Session("Process") = "Accept"
        Response.Redirect("wfrmFilterRenewals.aspx")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
End Class
