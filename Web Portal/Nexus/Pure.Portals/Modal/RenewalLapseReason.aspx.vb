
Imports CMS.library
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session
Namespace Nexus
    Partial Class Modal_RenewalLapseReason : Inherits Frontend.clsCMSPage
        Protected sLapseConfirmation As String
        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub
        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "LapseConfirmation", _
                "<script language=""JavaScript"" type=""text/javascript"">function LapseConfirmation(){Page_ClientValidate();  if (Page_IsValid == true) {return confirm('" & GetLocalResourceObject("msg_LapseStatus").ToString() & "');}}</script>")

        End Sub
        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            sLapseConfirmation = GetLocalResourceObject("msg_LapseStatus")
            'To set the Focus
            Page.SetFocus(RenewalReasonDescription)

            Page.ClientScript.RegisterOnSubmitStatement(Me.GetType(), "OnSubmitScript", "beforeSubmit();")


            btnLapse.Attributes.Add("onclick", "javascript:return LapseConfirmation();")
        End Sub

        Protected Sub btnLapse_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLapse.Click
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote
            Dim oParty As NexusProvider.BaseParty
            Dim oPolicyCollection As NexusProvider.PolicyCollection = Session(CNSearchResults)
            btnLapse.Attributes.Add("onclick", "")
            If oPolicyCollection IsNot Nothing AndAlso oPolicyCollection.Count > 0 Then
                For iCount As Integer = 0 To oPolicyCollection.Count - 1
                    If oPolicyCollection(iCount).IsSelected = True Then
                        oQuote = oWebService.GetHeaderAndSummariesByKey(oPolicyCollection(iCount).InsuranceFileKey)
                        oParty = oWebService.GetParty(oQuote.PartyKey)
                        Session(CNParty) = oParty
                        Session(CNQuote) = oQuote
                        
                        oWebService.LapseRenewal(oQuote, RenewalReasonDescription.Value, oQuote.BranchCode)
                    End If
                Next
            End If

            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_updated('" & Request.QueryString("PostbackTo") & "','RefreshPolicy_lps');", True)
        End Sub
    End Class
End Namespace




