Imports Nexus.Constants
Partial Class PremiumFinance_PlanTransactions
    Inherits CMS.Library.Frontend.clsCMSPage

    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            'Cleaning of the session values
            ClearClaims()
            ClearHeader()
            ClearInstalment()
            ClearQuote()
            Session("PFProductCode") = Nothing
            Session(CNInstalmentPlanMode) = InstalmentPlanType.Add
        End If
        If Request("__EVENTTARGET") IsNot Nothing AndAlso Request("__EVENTTARGET") = "RedirectFinancePlanDetails" Then
            Session(CNInstalmentPlanMode) = InstalmentPlanType.edit
            Response.Redirect(Request("__EVENTARGUMENT"))

        End If
    End Sub
End Class
