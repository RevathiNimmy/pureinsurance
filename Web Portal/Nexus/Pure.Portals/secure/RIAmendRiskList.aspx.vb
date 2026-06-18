Imports CMS.Library
Imports Nexus.Utils
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class secure_RIAmendRiskList : Inherits Frontend.clsCMSPage
#Region "Protected"
        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            Dim oQuote As NexusProvider.Quote = CType(Session(CNQuote), NexusProvider.Quote)

            If oQuote IsNot Nothing Then
                Dim sRoundOff As String = String.Empty
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                sRoundOff = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.RoundOffToZero, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing, oQuote.BranchCode).Trim()
                oWebService = Nothing
                If sRoundOff.Equals("1") Then
                    litPremiumDisplay.Text = Convert.ToString(GetLocalResourceObject("lbl_PremiumDisplayRoundoff"))
                Else
                    litPremiumDisplay.Text = Convert.ToString(GetLocalResourceObject("lbl_PremiumDisplay"))
                End If

                Dim dTatalPremium As Decimal
                If oQuote.Risks.Count > 0 Then
                    dTatalPremium = Convert.ToDecimal(oQuote.GrossTotal)
                End If
                lblPremiumValue.Text = New Money(dTatalPremium, Convert.ToString(Session(CNCurrenyCode))).Formatted
                lblPageheader.Text = Replace(lblPageheader.Text, "#PolicyNo", oQuote.InsuranceFileRef)
            End If
        End Sub

        Protected Sub btnAccept_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAccept.Click
            'If all risks are quoted then call CreatePostingForReinsurance and redirect to Transaction confirmation page
            Dim oQuote As NexusProvider.Quote = CType(Session(CNQuote), NexusProvider.Quote)
            If Page.IsValid Then
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                If Session(CNMode) = Mode.PortFolioTransferAmendment Then
                    oWebService.CreatePostingsForReinsurance(oQuote.InsuranceFileKey, PortfolioTransfer, oQuote.TimeStamp, oQuote.BranchCode)
                    Response.Redirect("RIAmendManager.aspx")
                ElseIf Session(CNMode) = Mode.ClonedTransferAmendment Then
                    oWebService.CreatePostingsForReinsurance(oQuote.InsuranceFileKey, Cloned, oQuote.TimeStamp, oQuote.BranchCode)
                    Response.Redirect("RIAmendManager.aspx?ClonedTransfer=True")
                End If
                oWebService = Nothing
            End If
        End Sub

        Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
            'Update Status if all risks are quoted
            Dim oQuote As NexusProvider.Quote = CType(Session(CNQuote), NexusProvider.Quote)
            Dim bAllRisksQuoted As Boolean = True

            For Each oRisk As NexusProvider.Risk In oQuote.Risks
                If oRisk.StatusCode.Trim().ToUpper() <> "QUOTED" AndAlso oRisk.StatusCode.Trim().ToUpper() <> "DELETED" Then
                    bAllRisksQuoted = False
                    Exit For
                End If
            Next

            If bAllRisksQuoted = True Then
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                If Session(CNMode) = Mode.PortFolioTransferAmendment Then
                    oWebService.UpdateRIAmendmentStatus(oQuote, PortfolioTransfer, Convert.ToString(PortfolioStatus.RICalculated), oQuote.BranchCode)
                    Response.Redirect("RIAmendManager.aspx")
                ElseIf Session(CNMode) = Mode.ClonedTransferAmendment Then
                    oWebService.UpdateRIAmendmentStatus(oQuote, Cloned, Convert.ToString(PortfolioStatus.RICalculated), oQuote.BranchCode)
                    Response.Redirect("RIAmendManager.aspx?ClonedTransfer=True")
                End If
                oWebService = Nothing
            Else
                If Session(CNMode) = Mode.PortFolioTransferAmendment Then
                    Response.Redirect("RIAmendManager.aspx")
                ElseIf Session(CNMode) = Mode.ClonedTransferAmendment Then
                    Response.Redirect("RIAmendManager.aspx?ClonedTransfer=True")
                End If
            End If
        End Sub

        Protected Sub vldChkStatus_ServerValidate(ByVal source As Object, ByVal args As ServerValidateEventArgs)
            Dim oQuote As NexusProvider.Quote = CType(Session(CNQuote), NexusProvider.Quote)
            Dim bAllRisksQuoted As Boolean = True

            For Each oRisk As NexusProvider.Risk In oQuote.Risks
                If oRisk.StatusCode.Trim() <> "QUOTED" AndAlso oRisk.StatusCode.Trim() <> "DELETED" Then
                    bAllRisksQuoted = False
                    Exit For
                End If
            Next

            If bAllRisksQuoted = False Then
                args.IsValid = False
            End If
        End Sub
#End Region
    End Class

End Namespace
