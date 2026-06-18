Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Web.Configuration.WebConfigurationManager

Namespace Nexus


    Partial Class Modal_PFPlanSettle
        Inherits Frontend.clsCMSPage
        Dim nFinancePlanKey As Integer
        Dim nFinancePlanVersion As Integer
        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                Dim oFinancePlanDetails As New NexusProvider.FinancePlanDetails
                If Session(CNFinancePlanDetails) IsNot Nothing Then
                    oFinancePlanDetails = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).PremiumFinanceDetails
                End If
                lblMessage.Text = GetLocalResourceObject("lbl_Message") & " " & oFinancePlanDetails.SettlementAmount & "?"
            End If
            btnCancel.Attributes.Add("onclick", "self.parent.tb_remove();return false;")
        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            'setting the default master page
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub
        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            'btnOk.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/Modal/PremiumFinanceQuote.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700' , null);return false;"

        End Sub

        Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
            Dim nFinancePlanKey As Integer
            Dim nFinancePlanVersion As Integer
            If Request.QueryString("FinancePlanKey") IsNot Nothing AndAlso _
                Request.QueryString("FinancePlanKey") <> "" AndAlso _
                Request.QueryString("FinancePlanVersion") IsNot Nothing AndAlso Request.QueryString("FinancePlanVersion") <> "" Then
                nFinancePlanKey = Request.QueryString("FinancePlanKey")
                nFinancePlanVersion = Request.QueryString("FinancePlanVersion")

                Dim oInstalment As New BaseInstalment
                Dim nDebitTransdetailKey As Integer = 0
                oInstalment.SettlePremiumFinancePlan(nFinancePlanKey, nFinancePlanVersion, nDebitTransdetailKey)
                Session(CNDebitTransDetailkey) = nDebitTransdetailKey
                ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "closeThickBox", "self.parent.RedirectToCashList();", True)
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowFinancePlanAlert", "alert('" + GetLocalResourceObject("msg_FinancePlanInvalid") + "');", True)
            End If
        End Sub
    End Class
End Namespace
