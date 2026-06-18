Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Utils
Partial Class Controls_FinancePlan
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dTotalRiskTaxExcludedFromInstalment As Decimal
        Dim dTotalFeeTaxExcludedFromInstalment As Decimal
        Dim dTotalRiskFeeExcludedFromInstalment As Double
        Dim dAmountToFinance As Double
        Dim dAgentCommission As Double
        Dim dTaxOnAgentCommission As Double

        Dim oFinancePlan As NexusProvider.FinancePlan
        Dim oPartyBankDetails As NexusProvider.BankCollection
        Dim oPartyBankDetail As NexusProvider.Bank
        Dim oParty As NexusProvider.BaseParty = CType(Session.Item(CNParty), NexusProvider.BaseParty)
        Dim oPartyBankDetailsForInstalment As New NexusProvider.BankCollection
        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim oQuote As NexusProvider.Quote
        Dim oQuoteForTax As NexusProvider.Quote
        Dim oQuoteForFees As NexusProvider.Quote
        Dim sCurrISOCode As String
        Try
            'Create oQuote object from session
            oQuote = Session.Item(CNQuote)

            If oQuote.PaymentMethod <> "PayNow" And oQuote.PaymentMethod <> "Invoice" And Session(CNMode) = Mode.View Then
                'To get exact tax and fees, wee need to call below given SAM functions
                oQuoteForTax = oWebService.GetHeaderAndPolicyTaxByKey(oQuote.InsuranceFileKey, oQuote.BranchCode)

                oQuoteForFees = oWebService.GetHeaderAndPolicyFeesByKey(oQuote.InsuranceFileKey, oQuote.BranchCode)

                'Get total tax excluded from instalment for all risks
                For iCt As Integer = 0 To oQuote.Risks.Count - 1
                    Dim oHeaderandRisk As NexusProvider.HeaderAndRisk
                    oHeaderandRisk = oWebService.GetHeaderAndRiskFeesByKey(oQuote.InsuranceFileKey, oQuote.Risks(iCt).Key)
                    For Each oRiskFee As NexusProvider.Fee In oHeaderandRisk.RiskFees
                        If oRiskFee.IncludeInInstallment = 0 Then
                            dTotalFeeTaxExcludedFromInstalment = dTotalFeeTaxExcludedFromInstalment + oRiskFee.TaxAmount
                            dTotalRiskFeeExcludedFromInstalment = dTotalRiskFeeExcludedFromInstalment + oRiskFee.FeeAmount
                        End If
                    Next
                    oHeaderandRisk = Nothing
                    Dim oQuoteForRiskTax As NexusProvider.Quote
                    oQuoteForRiskTax = oWebService.GetHeaderAndRiskTaxByKey(oQuote.InsuranceFileKey, oQuote.Risks(iCt).Key)

                    For Each oRiskTax As NexusProvider.Tax In oQuoteForRiskTax.RiskTaxes
                        If oRiskTax.IncludeinInstallment = 0 Then
                            dTotalRiskTaxExcludedFromInstalment = dTotalRiskTaxExcludedFromInstalment + oRiskTax.TaxAmount
                        End If
                    Next
                    oQuoteForRiskTax = Nothing
                Next


                Dim oAgentCommission As NexusProvider.EditAgentCommission
                'make SAM call to get the Agent Commission and save them in cache
                oAgentCommission = oWebService.GetAgentCommission(oQuote.InsuranceFileKey)

                If oAgentCommission IsNot Nothing Then
                    With oAgentCommission
                        For iCt As Integer = 0 To oAgentCommission.AgentCommission.Count - 1
                            Dim oSelectAgentCommission As NexusProvider.AgentCommission = .AgentCommission(iCt)
                            dAgentCommission = dAgentCommission + oSelectAgentCommission.CommissionValue
                            dTaxOnAgentCommission = dTaxOnAgentCommission + oSelectAgentCommission.TaxValue
                        Next
                    End With
                End If


                dAmountToFinance = CType(Session.Item(CNAmountToPay), Double) - (oQuoteForTax.TotalPolicyTaxExcludedFromFinancing + oQuoteForFees.TotalPolicyFeesExcludedFromFinancing + dTotalRiskTaxExcludedFromInstalment + dTotalFeeTaxExcludedFromInstalment + dTotalRiskFeeExcludedFromInstalment)
                dAmountToFinance = dAmountToFinance + dAgentCommission

                'Selection of instalment type will be visible only for MTA
                'pnlInstalmentType.Visible = True
                Try
                    oFinancePlan = oWebService.GetFinancePlanDetails(oQuote.InsuranceFileKey, oQuote.BranchCode)
                Catch
                Finally
                    oWebService = Nothing
                End Try
                If oFinancePlan IsNot Nothing Then

                    Dim dGrossDue As Decimal
                    Dim dTotalFees As Decimal
                    Dim dTotalTaxes As Decimal
                    'Bind the grid with retrieved quotes
                    If oFinancePlan.InstalmentDetails IsNot Nothing Then
                        grdInstallmentQuotes.DataSource = oFinancePlan.InstalmentDetails
                        grdInstallmentQuotes.DataBind()
                    End If
                    grdInstallmentQuotes.Font.Size = FontUnit.Smaller
                    sCurrISOCode = oQuote.CurrencyCode
                    Dim GrossTotal As Double
                    Dim TotalFees As Double
                    Dim TotalTaxes As Double
                    lbl_Plan_ref.Text = oFinancePlan.PlanReference
                    txtFinancedAmount.Text = New Money(oFinancePlan.TotalInstalmentAmount, Session(CNCurrenyCode)).Formatted
                    txtTotalPayable.Text = New Money(oFinancePlan.TotalInstalmentAmount, Session(CNCurrenyCode)).Formatted
                    txtTransactions.Text = 1 'As per discussion with Subhankar this will be 1 in Nexus
                    txtInstallements.Text = oFinancePlan.NoOfInstalments
                    txtRate.Text = oFinancePlan.InterestRate
                    txtAPR.Text = oFinancePlan.APRRate
                    txtStatus.Text = oFinancePlan.StatusDescription
                    txtDeposit.Text = New Money(oFinancePlan.Deposit, Session(CNCurrenyCode)).Formatted
                    txtAdminCharge.Text = New Money(oFinancePlan.AdminCharge, Session(CNCurrenyCode)).Formatted
                    txtProtectionCharge.Text = New Money(oFinancePlan.ProtectionCharge, Session(CNCurrenyCode)).Formatted
                    txtInterest.Text = New Money(oFinancePlan.InterestAmount, Session(CNCurrenyCode)).Formatted

                    txtFirstInstalmentDate.Text = oFinancePlan.FirstInstalmentDate
                    txtFirstInstalment.Text = New Money(oFinancePlan.FirstInstalmentAmount, Session(CNCurrenyCode)).Formatted
                    txtNextInstalment.Text = oFinancePlan.NextInstalmentDate
                    txtLastInstalment.Text = oFinancePlan.LastInstalmentDate
                    txtOtherInstalment.Text = New Money(oFinancePlan.OtherInstalmentAmount, Session(CNCurrenyCode)).Formatted
                    txtTaxes.Text = New Money(oFinancePlan.TaxAmount, Session(CNCurrenyCode)).Formatted
                    dGrossDue = oQuote.GrossTotal
                    txtGrossDue.Text = New Money(dGrossDue, Session(CNCurrenyCode)).Formatted
                    dTotalFees = oQuoteForFees.TotalPolicyFeesExcludedFromFinancing + oQuoteForFees.TotalRiskFeesExcludedFromFinancing
                    txtTotalFees.Text = New Money(dTotalFees, Session(CNCurrenyCode)).Formatted
                    dTotalTaxes = oQuoteForTax.TotalPolicyTaxExcludedFromFinancing + dTotalRiskTaxExcludedFromInstalment + dTotalFeeTaxExcludedFromInstalment
                    txtTotalTaxes.Text = New Money(dTotalTaxes, Session(CNCurrenyCode)).Formatted

                    txtTotalAmount.Text = New Money(oFinancePlan.TotalInstalmentAmount, Session(CNCurrenyCode)).Formatted
                    txtTotalFeesCollect.Text = New Money(oQuoteForFees.TotalFeesOnDeposit, Session(CNCurrenyCode)).Formatted
                    txtTotalTaxesCollect.Text = New Money(oQuoteForTax.TotalTaxOnDeposit, Session(CNCurrenyCode)).Formatted
                    txtMinimumDeposit.Text = New Money(oQuoteForTax.TotalTaxOnDeposit + oQuoteForFees.TotalFeesOnDeposit, Session(CNCurrenyCode)).Formatted
                    'Populate the Finance Charge and Deposit Tab’s fields.	
                    'Populate the Media Type and bank Details from the Collection.
                    If oFinancePlan.BankDetails IsNot Nothing Then
                        ddlAccountType.Items.Insert(0, oFinancePlan.BankDetails.AccountType)
                        txtBankName.Text = oFinancePlan.BankDetails.BankName
                        txtAddress1.Text = oFinancePlan.BankDetails.BankAddress
                        txtBranch.Text = oFinancePlan.BankDetails.BankBranch
                        txtBranchCode.Text = oFinancePlan.BankDetails.BranchCode
                        txtAccountName.Text = oFinancePlan.BankDetails.AccountHolderName
                        txtAccountNumber.Text = oFinancePlan.BankDetails.AccountNumber
                        txtBIC.Text = oFinancePlan.BankDetails.BIC
                        txtIBAN.Text = oFinancePlan.BankDetails.IBAN
                        'Putting the Mask
                        Dim bMaskBankAccountNumber As Boolean = CType(GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID().ToString()).MaskBankAccountNumber
                        If bMaskBankAccountNumber And txtAccountNumber.Text.Length > 4 Then
                            Dim sFirstStr As String = Mid(txtAccountNumber.Text, 1, txtAccountNumber.Text.Length - 4)
                            Dim sLastStr As String = Mid(txtAccountNumber.Text, sFirstStr.Length + 1)
                            For iCount As Integer = 0 To sFirstStr.Length - 1
                                sFirstStr = sFirstStr.Replace(sFirstStr.Chars(iCount), "*")
                            Next
                            txtAccountNumber.Text = sFirstStr & sLastStr
                        End If
                    End If

                    lblschemeinfo.Text = "(" + oFinancePlan.SchemeName + "," + oFinancePlan.Frequency + "," + oFinancePlan.PaymentMethod + ")"
                Else
                    Me.Visible = False

                End If
            End If
        Catch ex As NexusProvider.NexusException
            Throw
        End Try
    End Sub

    Protected Sub grdInstallmentQuotes_DataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdInstallmentQuotes.RowDataBound
        Dim oQuote As NexusProvider.Quote
        Dim sCurrISOCode As String
        oQuote = Session.Item(CNQuote)
        sCurrISOCode = oQuote.CurrencyCode
        If e.Row.RowType = DataControlRowType.DataRow Then
            If (e.Row.Cells(0).Text = "0" AndAlso e.Row.Cells(3).Text = "0") Then
                e.Row.Visible = False
            End If
            e.Row.Cells(3).Text = New Money(FormatNumber(e.Row.Cells(3).Text, 2), sCurrISOCode).Formatted
            e.Row.Cells(1).Text = FormatDateTime(e.Row.Cells(1).Text, DateFormat.ShortDate)
            e.Row.Cells(2).Text = FormatDateTime(e.Row.Cells(2).Text, DateFormat.ShortDate)
            If (e.Row.Cells(2).Text = "01/01/0001") Then
                e.Row.Cells(2).Text = ""
            End If

            If (e.Row.Cells(0).Text = "0") Then
                e.Row.Cells(0).Text = "Deposit"
            End If

        End If
    End Sub
End Class
