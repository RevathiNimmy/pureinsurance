
Partial Class Controls_PolicyHeader
    Inherits System.Web.UI.UserControl

    Protected sChangeMessage As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.Request.Url.ToString.ToLower.Contains("maindetails") Then
            'lbtnMainDetail.CssClass = "ActiveTab"
            'lbtnPolicySummary.CssClass = ""
            liMainDetails.Attributes.Add("class", "active")
            liPolicySummary.Attributes.Add("class", "")
        Else
            'lbtnMainDetail.CssClass = ""
            'lbtnPolicySummary.CssClass = "ActiveTab"
            liMainDetails.Attributes.Add("class", "")
            liPolicySummary.Attributes.Add("class", "active")
        End If
        sChangeMessage = GetLocalResourceObject("lbl_ChangeMessage")
    End Sub

End Class
