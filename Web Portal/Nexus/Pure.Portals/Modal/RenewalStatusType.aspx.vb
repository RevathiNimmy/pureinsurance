Imports CMS.library
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session
Namespace Nexus
    Partial Class Modal_RenewalStatusType : Inherits Frontend.clsCMSPage

        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            'To set the Focus
            Page.SetFocus(RenewalStatusType)

            btnUpdateStatus.Attributes.Add("onclick", "javascript:return UpdateConfirmation();")
            If Not IsPostBack Then
                Dim olist As NexusProvider.LookupListCollection
                Dim oListWithoutAwaitingBrokerTransfer As New NexusProvider.LookupListCollection

                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                olist = oWebService.GetList(NexusProvider.ListType.PMLookup, "Renewal_Status_Type", True, False)
                For iCount As Integer = 0 To olist.Count - 1
                    If olist(iCount).Code.Trim <> "BROKERXFER" Then
                        oListWithoutAwaitingBrokerTransfer.Add(olist(iCount))
                    End If
                Next
                RenewalStatusType.DataSource = oListWithoutAwaitingBrokerTransfer
                RenewalStatusType.DataTextField = "Description"
                RenewalStatusType.DataValueField = "Code"
                RenewalStatusType.DataBind()
            End If
        End Sub
        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub
        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "UpdateConfirmation", _
                "<script language=""JavaScript"" type=""text/javascript"">function UpdateConfirmation(){return confirm('" & GetLocalResourceObject("msg_UpdateStatus").ToString() & "');}</script>")

        End Sub

        Protected Sub btnUpdateStatus_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdateStatus.Click

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote
            Dim oPolicyCollection As NexusProvider.PolicyCollection = Session(CNSearchResults)

            If oPolicyCollection IsNot Nothing AndAlso oPolicyCollection.Count > 0 Then
                For iCount As Integer = 0 To oPolicyCollection.Count - 1
                    If oPolicyCollection(iCount).IsSelected = True Then
                        oQuote = oWebService.GetHeaderAndSummariesByKey(oPolicyCollection(iCount).InsuranceFileKey)
                        If oPolicyCollection(iCount).RenewalStatusTypeCode = "BROKERXFER" Then
                            Response.Write("<script type=""text/javascript"">alert(""Can't renew the policy in Awaiting Broker Transfer Mode"");</script")
                            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
                        Else
                            oWebService.UpdateRenewalStatus(oQuote, RenewalStatusType.SelectedValue, oQuote.BranchCode)
                        End If
                    End If
                Next
            End If
            
            'Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_updated('" & Request.QueryString("PostbackTo") & "','RefreshPolicy');", True)
            Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "RefreshPolicy") & ";"
            Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)

        End Sub
    End Class
End Namespace



