Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Web.Configuration.WebConfigurationManager

Namespace Nexus

    Partial Class Modal_PlanMTA
        Inherits Frontend.clsCMSPage

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            btnCancel.Attributes.Add("onclick", "self.parent.tb_remove();return false;")
        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            'setting the default master page
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
            If Request.QueryString("FinancePlanKey") IsNot Nothing AndAlso Request.QueryString("FinancePlanKey") <> "" AndAlso Request.QueryString("FinancePlanVersion") IsNot Nothing AndAlso Request.QueryString("FinancePlanVersion") <> "" Then
                Dim nFinancePlanKey As Integer
                Dim nFinancePlanVersion As Integer
                nFinancePlanKey = Request.QueryString("FinancePlanKey")
                nFinancePlanVersion = Request.QueryString("FinancePlanVersion")

                If chkAddTransaction.Checked = True AndAlso chkChangeFrequency.Checked = False Then
                    Session(CNInstalmentPlanMode) = InstalmentPlanType.edit
                    Response.Redirect("~/Modal/PlanTransactions.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700&Type=MTA&FinancePlanKey=" & nFinancePlanKey & "&FinancePlanVersion=" & nFinancePlanVersion & "")
                ElseIf chkChangeFrequency.Checked = True AndAlso chkAddTransaction.Checked = False Then
                    Dim objBaseInstalment As New BaseInstalment()
                    objBaseInstalment.FillQuoteSession()
                    Session(CNInstalmentPlanMode) = InstalmentPlanType.edit
                    Response.Redirect("~/Modal/PremiumFinanceQuote.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700&Type=MTA&FinancePlanKey=" & nFinancePlanKey & "&FinancePlanVersion=" & nFinancePlanVersion & "&ShowPlan=False")
                End If
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowFinancePlanAlert", "alert('" + GetLocalResourceObject("msg_FinancePlanInvalid") + "');", True)
            End If
        End Sub
    End Class
End Namespace
