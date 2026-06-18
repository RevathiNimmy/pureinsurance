Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class controls_ProgressBar : Inherits System.Web.UI.UserControl

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

            'SINCE USER CAN'T NAVIGATE BACK AND FORWARD, THIS LOGIC SHOULD WORK. 
            'SETTING THE PREVIOUS PAGES FROM "CURRENTPAGE" TO DONE.
            'SETTING CURRENT PAGE TO INPROGRESS.
            If Session(CNLoginType) = LoginType.Customer Then
                lblClientDetails.Text = GetLocalResourceObject("lbl_CustDetails") 'Customer
            Else
                lblClientDetails.Text = GetLocalResourceObject("lbl_ClientDetails") 'Client 
            End If

            If Session.Item(CNQuoteMode) = QuoteMode.QuickQuote Then
                pnlProgress1.Visible = True
            End If

            Dim oPortalConfig As Config.Portal = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID())
            If oPortalConfig.ShowStatements Then
                'Need to Important Statement if ShowStatements =true in the Config
                pnlProgress5.Visible = True
            End If

            For i As Integer = 1 To Session(CNCurrentPageNumber) - 1
                If Me.FindControl("pnlProgress" & i) IsNot Nothing Then
                    CType(Me.FindControl("pnlProgress" & i), HtmlControl).Attributes("class") += " complete"
                End If
            Next

            If Me.FindControl("pnlProgress" & Session(CNCurrentPageNumber)) IsNot Nothing Then
                CType(Me.FindControl("pnlProgress" & Session(CNCurrentPageNumber)), HtmlControl).Attributes("class") += " in-progress"
            End If

            For i As Integer = Session(CNCurrentPageNumber) + 1 To 7
                If Me.FindControl("pnlProgress" & i) IsNot Nothing Then
                    CType(Me.FindControl("pnlProgress" & i), HtmlControl).Attributes("class") += " incomplete"
                End If
            Next

        End Sub
    End Class

End Namespace
