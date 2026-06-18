Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Utils
Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class ReInsurance : Inherits Frontend.clsCMSPage


        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                SetPageProgress(4)
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim sDisplayReinsurance As String
                sDisplayReinsurance = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.RiskTypeMaintenance, NexusProvider.ProductRiskOptions.Description, NexusProvider.RiskTypeOptions.DisplayReinsurance, CType(Session(CNQuote), NexusProvider.Quote).ProductCode, CType(Session(CNQuote), NexusProvider.Quote).ProductCode)

                If sDisplayReinsurance = False Then
                    ReInsuranceCntrl.Visible = False
                End If
            End If

        End Sub

        Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
            Response.Redirect("~/secure/PremiumDisplay.aspx", False)
        End Sub

    End Class

End Namespace
