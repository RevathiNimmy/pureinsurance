
Imports System.Data
Imports CMS.Library
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports Nexus.Utils
Imports System.Exception
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session

Partial Class secure_payment_MarkForCollection
    Inherits Frontend.clsCMSPage

    ''' <summary>
    ''' Update quote to mark for collection, hide initial message and buttons and show confirmation message
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        'TODO - Call UpdateQuote to set quote as marked for collection
        Dim oQuote As NexusProvider.Quote
        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        oQuote = Session(CNQuote)
        oQuote.MarkedQuoteForCollection = True
        oQuote.MarkedDateforCollection = Date.Now.Date
        litMessage.Text = GetLocalResourceObject("litMessageConfirm").ToString()
        pnlSubmitArea.Visible = False
        oWebService.UpdateQuotev2(oQuote, oQuote.BranchCode)
    End Sub

End Class
