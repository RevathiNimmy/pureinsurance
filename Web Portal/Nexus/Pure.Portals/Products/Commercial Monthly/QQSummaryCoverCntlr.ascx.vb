Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports Nexus.Utils
Partial Class Products_TestMOTOR_TestMOTOR_QQSummaryOfCover
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim oQuote As NexusProvider.Quote = Session(CNQuote)
        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        oWebService.GetHeaderAndRisksByKey(oQuote)

        lblPremiumIndication.Text = New Money(oQuote.GrossTotal, Session(CNCurrenyCode)).Formatted
    End Sub
End Class
