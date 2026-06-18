Imports Nexus.Utils
Imports NexusProvider.SAMForInsurance
Imports System.Data
Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Constants
Imports Nexus.Constants.Session

Partial Class Modal_CopyRiskTypeSelect
    Inherits System.Web.UI.Page

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim oQuote As NexusProvider.Quote = Session(CNQuote)
        Dim nRiskKey As Integer = 0
        Dim nRiskIndex As Integer = 0
        Dim nRiskNumber As Integer = 0

        If Request.QueryString("RiskIndex") IsNot Nothing Then
            nRiskIndex = Convert.ToInt32(Request.QueryString("RiskIndex"))
            Session(CNCurrentRiskKey) = nRiskIndex
        End If

        If Request.QueryString("RiskNumber") IsNot Nothing Then
            nRiskNumber = Convert.ToInt32(Request.QueryString("RiskNumber"))
        End If

        If Request.QueryString("RiskKey") IsNot Nothing Then
            nRiskKey = Convert.ToInt32(Request.QueryString("RiskKey"))
        End If

        If ddlCopyRiskType.SelectedValue = "C" Then
            oWebService.CopyRisk(oQuote, nRiskNumber, nRiskIndex, NexusProvider.CopyRiskTypes.Comparative, oQuote.BranchCode, nRiskKey)
        Else
            oWebService.CopyRisk(oQuote, nRiskNumber, nRiskIndex, NexusProvider.CopyRiskTypes.Duplicate, oQuote.BranchCode, nRiskKey)
        End If
        oWebService.GetRisk(oQuote.Risks(oQuote.Risks.Count - 1).Key, oQuote.Risks.Count - 1, oQuote, oQuote.BranchCode)
        oWebService.GetHeaderAndRisksByKey(oQuote)
        Session(CNCurrentRiskKey) = oQuote.Risks.Count - 1
        'Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "CopyRiskTypeSelected") & ";"
        'Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
        Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_updated('" & Request.QueryString("PostbackTo") & "','CopyRiskTypeSelected');", True)
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
    End Sub
End Class
