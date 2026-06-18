Imports NexusProvider.SAMForInsurance
Imports Nexus
Imports Nexus.Utils
Imports System.Data
Imports Nexus.Constants
Imports Nexus.Constants.Session
Namespace Nexus

    Partial Class Claims_ClaimReinsurance
        Inherits CMS.Library.Frontend.clsCMSPage

        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'set the relevant tab styles
            ucProgressBar.ReinsuranceStyle = "in-progress"
            ucProgressBar.OverviewStyle = "complete"

            Select Case CType(Session(CNMode), Mode)
                Case Mode.NewClaim, Mode.EditClaim, Mode.PayClaim, Mode.SalvageClaim, Mode.TPRecovery
                    ucProgressBar.PerilsStyle = "complete"
                    ucProgressBar.SummaryStyle = "complete"
                    ucProgressBar.CompleteStyle = "incomplete"
            End Select
            'set nav links
            'Check for RI2007 product hidden option
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oRI2007 As NexusProvider.OptionTypeSetting = Nothing
            oRI2007 = oWebservice.GetOptionSetting(NexusProvider.OptionType.ProductOption, 88)

            If oRI2007.OptionValue = "1" Then
                'if RI2007 is ON
                ctrl_RI2007.visible = True
                ctrl_RI.visible = False
            Else
                'if RI2007 is OFF
                ctrl_RI.visible = True
                ctrl_RI2007.visible = False
            End If
        End Sub
    End Class
End Namespace
