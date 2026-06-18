Imports System.Web.Configuration
Imports Nexus.Library
Imports CMS.Library
Imports Nexus.Utils
Imports System.Web.HttpContext
Imports Nexus.Constants
Imports Nexus.Constants.Session


Namespace Nexus
    Partial Class modal_ClaimPaymentSummary
        Inherits System.Web.UI.Page

        ''' <summary>
        ''' Handle Page_Load Event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                'fetch data from session
                Dim oSettleAllClaimPayments As NexusProvider.SettleAllClaimPaymentsResults = Session(CNClaimPaymentSummary)
                'bind the grid
                grdvSearchResults.DataSource = oSettleAllClaimPayments.Summary
                grdvSearchResults.DataBind()
            End If
        End Sub

        ''' <summary>
        ''' Handle Page_PreInit Event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            'change the theme here
            CMS.Library.Frontend.Functions.SetTheme(Page, ConfigurationManager.AppSettings("ModalPageTemplate"))
        End Sub

        ''' <summary>
        ''' Fill the grid and Populate MediaTypeDescription from MediaTypeCode
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvSearchResults_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdvSearchResults.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                'populate media type description from code
                CType(e.Row.FindControl("lblMediaType"), Label).Text = GetDescriptionForCode(NexusProvider.ListType.PMLookup, CType(e.Row.DataItem, NexusProvider.ClaimPaymentsSummary).MediaTypeCode, "MediaType")
            End If
        End Sub

        ''' <summary>
        ''' Handle Ok event and close the page
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
            'close the modal page
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
        End Sub
    End Class

End Namespace