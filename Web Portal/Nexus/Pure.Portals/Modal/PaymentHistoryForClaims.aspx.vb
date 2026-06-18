Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Utils
Imports System.Linq
Imports System.Linq.Enumerable
Imports NexusProvider

Namespace Nexus
    Partial Class Modal_PaymentHistoryForClaims
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'parallel from 3.1SR1
            If Not Page.IsPostBack Then
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
                Dim oCurrencyColl As NexusProvider.CurrencyCollection
                oCurrencyColl = oWebService.GetCurrenciesByBranch(oQuote.BranchCode)
                ViewState("BaseCurerency") = oCurrencyColl(0).BaseCurrencyCode
                PopulatePayments(oCurrencyColl(0).BaseCurrencyCode)
                oWebService = Nothing
            End If

        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub drgPaymentHistory_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles drgPaymentHistory.RowDataBound
            Dim sLossCurrency As String = Session(CNCurrenyCode)
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.FindControl("lblPayeeName"), Literal).Text = CType(e.Row.DataItem, NexusProvider.ClaimPayment).Payee.Name

                Dim TempGridRow As GridViewRow = e.Row
                TempGridRow.Cells(4).Text = New Money(CType(e.Row.DataItem, NexusProvider.ClaimPayment).PaymentAmount, CType(e.Row.DataItem, NexusProvider.ClaimPayment).CurrencyCode).Formatted
                TempGridRow.Cells(5).Text = New Money(CType(e.Row.DataItem, NexusProvider.ClaimPayment).TaxAmount, CType(e.Row.DataItem, NexusProvider.ClaimPayment).CurrencyCode).Formatted

                If CType(e.Row.DataItem, NexusProvider.ClaimPayment).LossCurrencyCode IsNot Nothing AndAlso String.IsNullOrEmpty(CType(e.Row.DataItem, NexusProvider.ClaimPayment).LossCurrencyCode) = False Then
                    TempGridRow.Cells(7).Text = New Money(CType(e.Row.DataItem, NexusProvider.ClaimPayment).LossAmount, CType(e.Row.DataItem, NexusProvider.ClaimPayment).LossCurrencyCode).Formatted
                Else
                    TempGridRow.Cells(7).Text = New Money(CType(e.Row.DataItem, NexusProvider.ClaimPayment).LossAmount, CType(e.Row.DataItem, NexusProvider.ClaimPayment).CurrencyCode).Formatted
                End If
                TempGridRow.Cells(8).Text = New Money(CType(e.Row.DataItem, NexusProvider.ClaimPayment).BaseAmount, CType(e.Row.DataItem, NexusProvider.ClaimPayment).BaseCurrencyCode).Formatted
            End If
        End Sub

        Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
        End Sub

        Private Sub PopulatePayments(Optional ByVal sBaseCurrencyCode As String = "")
            Dim oOpenClaim As New NexusProvider.ClaimOpen
            Dim oClaimDetails As NexusProvider.ClaimDetails = Nothing
            Dim oClaimRisk As NexusProvider.ClaimRisk = Nothing
            Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
            Dim sCurrencyCode As String
            Dim bDocRefVisible As Boolean = False

            Try

                Dim sOption As String
                Dim sIsGrossClaimPaymentAmount As String
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                sOption = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsGrossClaimPaymentAmount, NexusProvider.RiskTypeOptions.None, Session(CNProductCode), Nothing)
                If String.IsNullOrEmpty(sOption) Then
                    sIsGrossClaimPaymentAmount = "0"
                Else
                    sIsGrossClaimPaymentAmount = sOption
                End If

                oClaimDetails = GetClaimDetailsCall(CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimKey, Nothing, 1)
                With oClaimDetails
                    oOpenClaim.ClaimPeril = .ClaimPeril
                    sCurrencyCode = .CurrencyCode
                End With
                If sBaseCurrencyCode = "" Then
                    sBaseCurrencyCode = sCurrencyCode
                End If
                Dim oClaimPayment As New NexusProvider.ClaimPaymentCollection
                'Calculate the Loss Amount and Base Amount
                Dim oCurrency As New NexusProvider.Currency
                For Each oClaimPeril As NexusProvider.PerilSummary In oOpenClaim.ClaimPeril
                    For Each oPayment As NexusProvider.ClaimPayment In oClaimPeril.ClaimPayment
                        For Each oPaymentItem As NexusProvider.ClaimPaymentItem In oPayment.PaymentItems
                            Dim oBasePayment As New NexusProvider.ClaimPayment
                            oBasePayment.PaymentDate = oPayment.PaymentDate
                            oBasePayment.PartyPaidName = oPayment.PartyPaidName
                            oBasePayment.Payee = oPayment.Payee
                            If sIsGrossClaimPaymentAmount <> "0" Then
                                oBasePayment.PaymentAmount = oPaymentItem.PaymentAmount
                            Else
                                oBasePayment.PaymentAmount = oPaymentItem.PaymentAmount + Convert.ToDecimal(oPaymentItem.TaxAmount)
                            End If
                            oBasePayment.TaxAmount = oPaymentItem.TaxAmount
                            oBasePayment.CurrencyDescription = oPayment.CurrencyDescription
                            oBasePayment.CurrencyCode = oPayment.CurrencyCode

                            oBasePayment.LossCurrencyCode = sCurrencyCode
                            oBasePayment.BaseCurrencyCode = sBaseCurrencyCode
                            oBasePayment.DocumentReference = oPayment.DocumentReference
                            oBasePayment.PaymentStatus = oPayment.PaymentStatus
                            oBasePayment.TheirReference = oPayment.TheirReference

                            'Calculate the Loss Amount and Base Amount same as Financial Details

                            Dim dPaymentAmount As Decimal = 0D
                            dPaymentAmount = oPaymentItem.PaymentAmount + oPaymentItem.TaxAmount

                            Dim dBaseAmount As Decimal
                            Dim dLossAmount As Decimal
                            oCurrency.AccountCode = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen).ClientShortName
                            oCurrency.TransactionCurrencyCode = oPayment.CurrencyCode
                            oCurrency.Mode = "ALL"
                            oCurrency = oWebService.GetCurrencyExchangeRates(oCurrency)
                            dBaseAmount = Math.Round((dPaymentAmount * oCurrency.BaseCurrencyRate), 2)
                            oCurrency.TransactionCurrencyCode = oPayment.LossCurrencyCode
                            oCurrency = oWebService.GetCurrencyExchangeRates(oCurrency)
                            dLossAmount = Math.Round((dPaymentAmount * oCurrency.BaseCurrencyRate), 2)
                            oBasePayment.LossAmount = dLossAmount
                            oBasePayment.BaseAmount = dBaseAmount

                            'hide doc ref column when 
                            If Not bDocRefVisible AndAlso Not String.IsNullOrEmpty(oBasePayment.DocumentReference) Then
                                bDocRefVisible = True
                            End If
                            oClaimPayment.Add(oBasePayment)
                        Next
                    Next
                Next

                If bDocRefVisible Then
                    drgPaymentHistory.Columns(1).Visible = True
                End If
                Dim oNonDeclinedPayments = From obj In oClaimPayment.OfType(Of ClaimPayment)() Where (obj.PaymentAmount <> 0) Select obj
                drgPaymentHistory.DataSource = oNonDeclinedPayments
                drgPaymentHistory.DataBind()
            Finally
                oClaimDetails = Nothing
                oUserDetails = Nothing
                oClaimRisk = Nothing
            End Try
        End Sub
    End Class
End Namespace
