Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports Nexus.Utils
Imports CMS.Library
Imports Nexus.Library
Imports System.Web.Configuration.WebConfigurationManager
Namespace Nexus

    Partial Class Controls_RiskTax
        Inherits System.Web.UI.UserControl

        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim oPolicytax As NexusProvider.Tax

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim iInsuranceFilekey As Integer
            Dim oQuote As NexusProvider.Quote
            Dim sRisk As String

            Try
                oQuote = Session(CNQuote)
                iInsuranceFilekey = oQuote.InsuranceFileKey

                sRisk = Request.QueryString("Riskkey")
                If sRisk Is Nothing AndAlso oQuote.Risks.Count > 0 AndAlso Session(CNCurrentRiskKey) IsNot Nothing Then
                    sRisk = oQuote.Risks(Session(CNCurrentRiskKey)).Key
                End If

                If sRisk IsNot Nothing AndAlso oQuote.Risks.Count > 0 Then
                    oQuote = Nothing
                    oQuote = oWebService.GetHeaderAndRiskTaxByKey(iInsuranceFilekey, CType(sRisk, Integer))

                    If oQuote.RiskTaxes IsNot Nothing And oQuote.RiskTaxes.Count > 0 Then
                        grdvRiskTax.DataSource = oQuote.RiskTaxes
                        grdvRiskTax.DataBind()
                    Else
                        grdvRiskTax.DataSource = Nothing
                        grdvRiskTax.DataBind()
                    End If
                Else
                    grdvRiskTax.DataSource = Nothing
                    grdvRiskTax.DataBind()
                End If
                'Catch ex As Exception
            Finally
                oQuote = Nothing
                oWebService = Nothing
                iInsuranceFilekey = Nothing
                sRisk = Nothing
            End Try
        End Sub

        Protected Sub grdvRiskTax_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvRiskTax.Load
            If grdvRiskTax.PageCount = 1 Then
                grdvRiskTax.AllowPaging = False
            End If
        End Sub

        Protected Sub grdvRiskTax_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvRiskTax.RowDataBound

            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim chkIsNotAppliedToClient As CheckBox = CType(e.Row.FindControl("IsNotAppliedToClient"), CheckBox)
                Dim chkIncludeInInstallment As CheckBox = CType(e.Row.FindControl("IncludeInInstallment"), CheckBox)
                Dim chkSpreadAcrossInstallment As CheckBox = CType(e.Row.FindControl("SpreadAcrossInstallment"), CheckBox)
                oPolicytax = CType(e.Row.DataItem, NexusProvider.Tax)
                chkIsNotAppliedToClient.Checked = oPolicytax.IsNotAppliedToClient
                chkIncludeInInstallment.Checked = oPolicytax.IncludeinInstallment
                chkSpreadAcrossInstallment.Checked = oPolicytax.SpreadAcrossInstallment
                Dim oFormatStringPercentage As String = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).FormatStrings.FormatString("Percentage").DataFormatString

                If oPolicytax.IsValue = True Then
                    e.Row.Cells(5).Text = New Money(oPolicytax.Rate, oPolicytax.CurrencyCode).Formatted
                Else
                    e.Row.Cells(5).Text = String.Format(oFormatStringPercentage, oPolicytax.Rate) 'awaiting for a generic solution so left 
                End If
                oPolicytax = Nothing
            End If


        End Sub
    End Class

End Namespace
