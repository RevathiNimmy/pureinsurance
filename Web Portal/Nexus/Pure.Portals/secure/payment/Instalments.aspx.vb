Imports CMS.Library
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports Nexus.Utils
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class secure_payment_Instalments : Inherits BasePayment ' Frontend.clsCMSPage

        Dim InstalmentQuotesCacheID As Guid
        Dim PartyBankdetailsCacheID As Guid
        Dim SelectedAccountTypeCacheId As Guid
        Dim SelectedInstalmentQuoteCacheId As Guid
        Dim QuoteForTaxCacheId As Guid
        Dim QuoteForFeesCacheId As Guid
        Dim TotalRiskTaxExcludedFromInstalmentCacheId As Guid
        Dim TotalFeeTaxExcludedFromInstalmentCacheId As Guid
        Dim TotalRiskFeeExcludedFromInstalmentCacheId As Guid
        Dim AgentCommissionCacheId As Guid
        Dim TaxOnAgentCommissionCacheId As Guid


        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Dim dTotalRiskTaxExcludedFromInstalment As Decimal
            Dim dTotalFeeTaxExcludedFromInstalment As Decimal
            Dim dTotalRiskFeeExcludedFromInstalment As Double
            Dim dAmountToFinance As Double
            Dim dAgentCommission As Double
            Dim dTaxOnAgentCommission As Double

            'If payment made successfully the redirect to transaction confirmation page
            If Session(CNPaid) = True Then
                SetPaymentTakenAndRedirect()
            End If
            If Session(CNMTAType) IsNot Nothing Then
                divChangeDate.Attributes.Add("style", "display:none;")
            End If

            If Request("__EVENTARGUMENT") = "UpdateBank" Then
                Dim oParty As NexusProvider.BaseParty = CType(Session(CNParty), NexusProvider.BaseParty)
                Page.ClientScript.GetPostBackEventReference(Me, "")
                Dim sBankData() As String = txtBankDetailData.Value.Split(";")

                ''Need to Retreive the Data from Session
                'RetreiveData()

                If sBankData(0).ToUpper = "ADD" Then

                    Dim oNewBank As New NexusProvider.Bank

                    With oNewBank
                        .BankPaymentTypeCode = sBankData(1)
                        .AccountHolderName = sBankData(2)
                        .AccountType = sBankData(3)
                        .AccountNumber = sBankData(4)
                        .AccountCode = sBankData(4)
                        .BranchCode = sBankData(5)
                        .BankBranch = sBankData(6)

                        .BankCode = sBankData(7)
                        .BankName = sBankData(8)
                        .StreetName = sBankData(9)
                        .Locality = sBankData(10)
                        .PostTown = sBankData(11)
                        .County = sBankData(12)
                        .PostCode = sBankData(13)
                        .Country = sBankData(14)
                        .PartyBankAddress.Address1 = sBankData(9)
                        .PartyBankAddress.Address2 = sBankData(10)
                        .PartyBankAddress.Address3 = sBankData(11)
                        .PartyBankAddress.Address4 = sBankData(12)
                        .PartyBankAddress.PostCode = sBankData(13)
                        .PartyBankAddress.CountryCode = sBankData(14)
                        .TaskMode = NexusProvider.Bank.Mode.Add
                        '.BankKey = sBankData(14)
                    End With

                    oParty.BankDetails.Add(oNewBank)
                    Session(CNParty) = oParty

                ElseIf sBankData(0).ToUpper = "UPDATE" Then
                    Dim oUpdateBankCollection As NexusProvider.BankCollection = oParty.BankDetails
                    Dim oUpdateBanks As NexusProvider.Bank = oParty.BankDetails.Item(CType(sBankData(15), Integer))

                    With oUpdateBanks
                        .BankPaymentTypeCode = sBankData(1)
                        .AccountHolderName = sBankData(2)
                        .AccountType = sBankData(3)
                        .AccountNumber = sBankData(4)
                        .AccountCode = sBankData(4)
                        .BranchCode = sBankData(5)
                        .BankBranch = sBankData(6)

                        .BankCode = sBankData(7)
                        .BankName = sBankData(8)
                        .StreetName = sBankData(9)
                        .Locality = sBankData(10)
                        .PostTown = sBankData(11)
                        .County = sBankData(12)
                        .PostCode = sBankData(13)
                        .Country = sBankData(14)
                        .PartyBankAddress.Address1 = sBankData(9)
                        .PartyBankAddress.Address2 = sBankData(10)
                        .PartyBankAddress.Address3 = sBankData(11)
                        .PartyBankAddress.Address4 = sBankData(12)
                        .PartyBankAddress.PostCode = sBankData(13)
                        .PartyBankAddress.CountryCode = sBankData(14)
                        If String.IsNullOrEmpty(.PartyBankKey) = False AndAlso .PartyBankKey > 0 Then
                            .TaskMode = NexusProvider.Bank.Mode.Edit
                        Else
                            .TaskMode = NexusProvider.Bank.Mode.Add
                        End If
                        '.BankKey = sBankData(14)
                    End With

                    oUpdateBankCollection.Update(oUpdateBanks)
                    Session(CNParty) = oParty
                End If
                Dim objBaseClient As New Nexus.BaseClient
                objBaseClient.UpdatePartyBank()

                
                'hypBank.Visible = True
                hypBankEdit.Visible = False
                Dim oQuote As NexusProvider.Quote
                'Create oQuote object from session
                oQuote = Session(CNQuote)
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oPartyBankDetails As NexusProvider.BankCollection
                Dim oPartyBankDetailsForInstalment As New NexusProvider.BankCollection
                Try
                    
                    If (Session(CNAgentType) IsNot Nothing AndAlso Session(CNAgentType).Trim.ToUpper = "BROKER" AndAlso oQuote.BusinessTypeCode <> "DIRECT") Then
                        'If Session(CNAgentType) IsNot Nothing Then
                        'If agent type is broker then we need to get agent bank details 
                        'else party bank details would be required
                        Dim iAgentId As Integer = 0
                        Integer.TryParse(oQuote.Agent, iAgentId)
                        oPartyBankDetails = oWebService.GetPartyBankDetails(iAgentId)
                    Else
                        oPartyBankDetails = oWebService.GetPartyBankDetails(oQuote.PartyKey)
                    End If
                    'Populate Party bank Details
                    oParty.BankDetails = oPartyBankDetails
                    Session(CNParty) = oParty

                    'Filter Valid Accounts for Instalment (should be for ANY or INS type)
                    

                    For Each oPartyBankDetail In oPartyBankDetails
                        If (oPartyBankDetail.BankPaymentTypeCode.ToUpper() = "INS" Or oPartyBankDetail.BankPaymentTypeCode.ToUpper() = "ANY") AndAlso _
                            oPartyBankDetail.AccountType.Trim() <> "" AndAlso oPartyBankDetail.IsDeleted = False Then
                            oPartyBankDetailsForInstalment.Add(oPartyBankDetail)
                        End If
                    Next

                    'Add filtered party bank details to cache
                    Cache.Insert(ViewState("PartyBankdetailsCacheID"), oPartyBankDetailsForInstalment, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

                    PopulateAccountType()
                Catch
                Finally
                    oQuote = Nothing
                    oPartyBankDetails = Nothing
                    oPartyBankDetailsForInstalment = Nothing
                    oWebService = Nothing
                End Try
                
            End If

            If Not IsPostBack Then
                Dim oInstalmentQuotes As NexusProvider.InstallmentQuoteCollection
                Dim oPartyBankDetails As NexusProvider.BankCollection
                Dim oPartyBankDetail As NexusProvider.Bank
                Dim oParty As NexusProvider.BaseParty = CType(Session(CNParty), NexusProvider.BaseParty)
                Dim oPartyBankDetailsForInstalment As New NexusProvider.BankCollection
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oQuote As NexusProvider.Quote
                Dim oQuoteForTax As NexusProvider.Quote
                Dim oQuoteForFees As NexusProvider.Quote
                Dim paymentOptions As New Config.PaymentTypes
                Dim oPayment As New NexusProvider.Payment(NexusProvider.PaymentTypes.None)

                'Generata unique cache id for storing different values and collections in cache
                InstalmentQuotesCacheID = Guid.NewGuid()
                PartyBankdetailsCacheID = Guid.NewGuid()
                SelectedAccountTypeCacheId = Guid.NewGuid()
                SelectedInstalmentQuoteCacheId = Guid.NewGuid()
                QuoteForTaxCacheId = Guid.NewGuid()
                QuoteForFeesCacheId = Guid.NewGuid()
                TotalRiskTaxExcludedFromInstalmentCacheId = Guid.NewGuid()
                TotalFeeTaxExcludedFromInstalmentCacheId = Guid.NewGuid()
                TotalRiskFeeExcludedFromInstalmentCacheId = Guid.NewGuid()
                AgentCommissionCacheId = Guid.NewGuid()
                TaxOnAgentCommissionCacheId = Guid.NewGuid()

                ViewState.Add("InstalmentQuotesCacheID", InstalmentQuotesCacheID.ToString)
                ViewState.Add("PartyBankdetailsCacheID", PartyBankdetailsCacheID.ToString)
                ViewState.Add("SelectedAccountTypeCacheId", SelectedAccountTypeCacheId.ToString)
                ViewState.Add("SelectedInstalmentQuoteCacheId", SelectedInstalmentQuoteCacheId.ToString)

                ViewState.Add("QuoteForTaxCacheId", QuoteForTaxCacheId.ToString)
                ViewState.Add("QuoteForFeesCacheId", QuoteForFeesCacheId.ToString)
                ViewState.Add("TotalRiskTaxExcludedFromInstalmentCacheId", TotalRiskTaxExcludedFromInstalmentCacheId.ToString())
                ViewState.Add("TotalFeeTaxExcludedFromInstalmentCacheId", TotalFeeTaxExcludedFromInstalmentCacheId.ToString())
                ViewState.Add("TotalRiskFeeExcludedFromInstalmentCacheId", TotalRiskFeeExcludedFromInstalmentCacheId.ToString())

                ViewState.Add("AgentCommissionCacheId", AgentCommissionCacheId.ToString())
                ViewState.Add("TaxOnAgentCommissionCacheId", TaxOnAgentCommissionCacheId.ToString())

                Try
                    'Create oQuote object from session
                    oQuote = Session(CNQuote)

                    'HERE WE PASS THE INCEPTION DATE AS CURRENT DATE
                    oQuote.InceptionDate = Date.Today

                    'To get exact tax and fees, wee need to call below given SAM functions
                    oQuoteForTax = oWebService.GetHeaderAndPolicyTaxByKey(oQuote.InsuranceFileKey, oQuote.BranchCode)
                    Cache.Insert(ViewState("QuoteForTaxCacheId"), oQuoteForTax, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
                    oQuoteForFees = oWebService.GetHeaderAndPolicyFeesByKey(oQuote.InsuranceFileKey, oQuote.BranchCode)
                    Cache.Insert(ViewState("QuoteForFeesCacheId"), oQuoteForFees, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

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

                    Cache.Insert(ViewState("TotalRiskTaxExcludedFromInstalmentCacheId"), dTotalRiskTaxExcludedFromInstalment, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
                    Cache.Insert(ViewState("TotalFeeTaxExcludedFromInstalmentCacheId"), dTotalFeeTaxExcludedFromInstalment, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
                    Cache.Insert(ViewState("TotalRiskFeeExcludedFromInstalmentCacheId"), dTotalRiskFeeExcludedFromInstalment, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

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

                    Cache.Insert(ViewState("AgentCommissionCacheId"), dAgentCommission, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
                    Cache.Insert(ViewState("TaxOnAgentCommissionCacheId"), dTaxOnAgentCommission, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

                    dAmountToFinance = CType(Session(CNAmountToPay), Double) - (oQuoteForTax.TotalPolicyTaxExcludedFromFinancing + oQuoteForFees.TotalPolicyFeesExcludedFromFinancing + dTotalRiskTaxExcludedFromInstalment + dTotalFeeTaxExcludedFromInstalment + dTotalRiskFeeExcludedFromInstalment)
                    dAmountToFinance = dAmountToFinance + dAgentCommission + dTaxOnAgentCommission

                    'If process is MTA then we need to display instalment type option 
                    If Not Session(CNMTAType) Is Nothing Then
                        'Selection of instalment type will be visible only for MTA
                        pnlInstalmentType.Visible = True
                        'By default First option(AddAndSpread) will be selected 
                        rbInstalmentType.SelectedValue = "0"

                        If dAmountToFinance < 0 Then
                            rbInstalmentType.Items(2).Enabled = False
                        End If

                        'In Nexus, we does not have input field for selecting weekday and month day
                        'By default first value is selected in BO.So passing value 1 for these parameters
                        oInstalmentQuotes = oWebService.GetInstalmentQuotes(dAmountToFinance, _
                                                    oQuote.CoverStartDate, oQuote.CoverEndDate, oQuote.InceptionDate, _
                                                    DateTime.Now, 1, 1, oQuote.InsuranceFileKey, -1, 0, True, oQuote.BranchCode, rbInstalmentType.SelectedValue)

                    Else
                        'In Nexus, we does not have input field for selecting weekday and month day
                        'By default first value is selected in BO.So passing value 1 for these parameters
                        oInstalmentQuotes = oWebService.GetInstalmentQuotes(dAmountToFinance, _
                                                    oQuote.CoverStartDate, oQuote.CoverEndDate, oQuote.InceptionDate, _
                                                    DateTime.Now, 1, 1, oQuote.InsuranceFileKey, -1, 0, True, oQuote.BranchCode)
                    End If

                    'Add the retrived quotes in cache.So that they can be used throughout the page
                    Cache.Insert(ViewState("InstalmentQuotesCacheID"), oInstalmentQuotes, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

                    btnNext.Visible = True
                    pnlPlanSummary.Visible = True
                    grdInstallmentQuotes.Visible = True

                Catch ex As NexusProvider.NexusException
                    If ex.Errors(0).Code = "0" Then
                        '    lblErrorMsg.Visible = True
                        '   lblErrorMsg.Text = ex.Errors(0).Description
                        btnNext.Visible = False
                        pnlPlanSummary.Visible = False
                        grdInstallmentQuotes.Visible = False
                    Else
                        Throw
                    End If
                End Try


                'Bind the grid with retrieved quotes
                grdInstallmentQuotes.DataSource = oInstalmentQuotes
                grdInstallmentQuotes.DataBind()

                'Find all the account types for party/agent using sam method "GetPartyBankDetails"
                'Filter all the accounts for instalment payment method and save the details in cache
                'So that party bank details can be used througout the page
                If (Session(CNAgentType) IsNot Nothing AndAlso Session(CNAgentType).Trim.ToUpper = "BROKER" AndAlso oQuote.BusinessTypeCode <> "DIRECT") Then
                    'If Session(CNAgentType) IsNot Nothing Then
                    'If agent type is broker then we need to get agent bank details 
                    'else party bank details would be required
                    Dim iAgentId As Integer = 0
                    Integer.TryParse(oQuote.Agent, iAgentId)
                    oPartyBankDetails = oWebService.GetPartyBankDetails(iAgentId)
                Else
                    oPartyBankDetails = oWebService.GetPartyBankDetails(oQuote.PartyKey)
                End If

                'Populate Party bank Details
                oParty.BankDetails = oPartyBankDetails
                Session(CNParty) = oParty

                'Filter Valid Accounts for Instalment (should be for ANY or INS type)
                'For Each oPartyBankDetail In oPartyBankDetails
                '    If (oPartyBankDetail.BankPaymentTypeCode.ToUpper() = "INS" Or oPartyBankDetail.BankPaymentTypeCode.ToUpper() = "ANY") AndAlso _
                '        oPartyBankDetail.AccountType.Trim() <> "" AndAlso oPartyBankDetail.IsActive Then
                '        oPartyBankDetailsForInstalment.Add(oPartyBankDetail)
                '    End If
                'Next

                For Each oPartyBankDetail In oPartyBankDetails
                    If (oPartyBankDetail.BankPaymentTypeCode.ToUpper() = "INS" Or oPartyBankDetail.BankPaymentTypeCode.ToUpper() = "ANY") AndAlso _
                        oPartyBankDetail.AccountType.Trim() <> "" AndAlso oPartyBankDetail.IsDeleted = False Then
                        oPartyBankDetailsForInstalment.Add(oPartyBankDetail)
                    End If
                Next

                'Add filtered party bank details to cache
                Cache.Insert(ViewState("PartyBankdetailsCacheID"), oPartyBankDetailsForInstalment, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

                'If some quotes retrieved then first instalment quote should be selectes and btNext should be visible.
                'Details for selected instalment quote should be visible in pnlPlanSummary
                If grdInstallmentQuotes.Rows.Count > 0 And grdInstallmentQuotes.SelectedIndex = -1 Then
                    'Select first row
                    grdInstallmentQuotes.Rows(0).RowState = DataControlRowState.Selected
                    'Show instalment details for selecetd instalment quote
                    ShowDetailsForScheme(oInstalmentQuotes(0).SchemeNo, oInstalmentQuotes(0).SchemeVersion, oInstalmentQuotes(0).CompanyNo)
                    'Make visible tyo plan summary and btnNext
                    pnlPlanSummary.Visible = True
                    btnNext.Enabled = True
                    grdInstallmentQuotes.Visible = True
                End If
                If grdInstallmentQuotes.Rows.Count > 0 Then
                    PopulateInstalmentDates()
                End If

            End If
        End Sub

        Protected Sub btnNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNext.Click
            'THIS SECTION IS FOR UPDATING THE INCEPTION DATE FROM THE TXTINCEPTIONDATE
            'SIMILARLY ALSO UPDATING THE BANK DETAILS AND THEN UPDATING THE CNPAYMENT AND CNQUOTE
            If Page.IsValid Then
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                Dim oSelectedPartyBankDetail As NexusProvider.Bank = Cache.Item(ViewState("SelectedAccountTypeCacheId"))
                Dim oSelectedInstalmentQuote As NexusProvider.InstalmentQuote = Cache.Item(ViewState("SelectedInstalmentQuoteCacheId"))

                'Create partybankdetail object from Cache. This object contains selected party bank account from ddlAccount dropdown
                oSelectedPartyBankDetail = Cache.Item(ViewState("SelectedAccountTypeCacheId"))

                Dim oPayment As New NexusProvider.Payment(NexusProvider.PaymentTypes.None, CDec(Session(CNAmountToPay)))

                'Set all required properties for payment object

                oPayment.AmountToFinance = oSelectedInstalmentQuote.TotalAmountInput
                oPayment.OverrideInterestRate = -1
                oPayment.PaymentProtection = True
                oPayment.QuoteDate = DateTime.Now
                oPayment.SelectedSchemeNo = oSelectedInstalmentQuote.SchemeNo
                oPayment.SelectedSchemeVersion = oSelectedInstalmentQuote.SchemeVersion
                oPayment.WeekDay = 1 'Default to 1 as no input field available in nexus
                oPayment.Pref_ID = oSelectedInstalmentQuote.PFRF_ID

                If Session(CNMTAType) IsNot Nothing Then
                    oPayment.InstallmentType = rbInstalmentType.SelectedValue
                    oPayment.InstallmentTypeSpecified = True
                    oPayment.PreferredDate = oQuote.InceptionDate
                    oPayment.StartDate = oQuote.CoverStartDate 'oSelectedInstalmentQuote.FirstInstalmentDate
                    oPayment.EndDate = oQuote.CoverEndDate 'oSelectedInstalmentQuote.LastInstalmentDate
                    oPayment.MonthDay = 1 'Default to 1 as no input field available in nexus

                Else
                    oPayment.InstallmentTypeSpecified = False
                    oPayment.PreferredDate = CType(ddlFirstPaymentDate.SelectedValue, Date).AddDays(-oSelectedInstalmentQuote.DaysDelay) 'Correct 
                    oPayment.StartDate = oQuote.CoverStartDate ' CType(ddlFirstPaymentDate.SelectedValue, Date).AddDays(-oSelectedInstalmentQuote.DaysDelay)
                    'MOSS 1132
                    oPayment.EndDate = oQuote.CoverEndDate 'CType(txtLastInstalment.Text, Date)
                    If oSelectedInstalmentQuote.AlignTo = 1 Then
                        oPayment.MonthDay = ddlDayinMonth.SelectedValue
                    Else
                        oPayment.MonthDay = 1
                    End If
                End If
                'We need to set Bank details for selected account type. 
                'If no account type is selected then bank details will be blank
                If oSelectedInstalmentQuote.MediaTypeDescription = "Direct Debit" Then

                    If ddlAccountType.SelectedValue = "" Then
                        'oPayment.PartyBankKey = oSelectedPartyBankDetail.PartyBankKey
                        oPayment.BankAccountName = txtAccountName.Text
                        oPayment.BankAccountNo = txtAccountNumber.Text
                        If txtAddress1.Text.Trim() <> "" Then
                            Dim oAddress As New NexusProvider.Address
                            oAddress.Address1 = txtAddress1.Text
                            oPayment.BankAddress = oAddress
                        End If
                        oPayment.BankBranch = txtBranch.Text
                        oPayment.BankName = txtBankName.Text
                        oPayment.BankSortCode = txtBranchCode.Text
                    ElseIf Not oSelectedPartyBankDetail Is Nothing Then
                        oPayment.PartyBankKey = oSelectedPartyBankDetail.PartyBankKey
                        oPayment.BankAccountName = oSelectedPartyBankDetail.AccountHolderName
                        oPayment.BankAccountNo = oSelectedPartyBankDetail.AccountNumber
                        oPayment.BankAddress = oSelectedPartyBankDetail.PartyBankAddress
                        oPayment.BankBranch = oSelectedPartyBankDetail.BankBranch
                        oPayment.BankName = oSelectedPartyBankDetail.BankName
                        oPayment.BankSortCode = oSelectedPartyBankDetail.BranchCode

                    End If

                End If

                'Save payment object in session.So that it can be used in BindQuote method
                Session(CNPayment) = oPayment
                oQuote.DepositTransactasInstalment = oSelectedInstalmentQuote.DepositAsInstalment
                oQuote.InstDepositAmount = oSelectedInstalmentQuote.DepositAmount
                Session(CNQuote) = oQuote


                Dim paymentTypes As New Config.PaymentTypes

                paymentTypes = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID).PaymentTypes

                Dim PaymentCollectionUrl As String = paymentTypes.PaymentType(Session(CNSelectedPaymentIndex)).PaymentCollectionUrl

                'set appropriate session values here to indicate payment taken and then redirect to end page
                If PaymentCollectionUrl <> "" Then
                    'Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                    If oQuote.InstDepositAmount > 0 AndAlso oQuote.DepositTransactasInstalment = False Then
                        Response.Redirect(PaymentCollectionUrl, False)
                    Else
                        SetPaymentTakenAndRedirect()
                    End If
                Else
                    SetPaymentTakenAndRedirect()
                End If



            End If
        End Sub
        ''' <summary>
        ''' To show instalment details for selected instalment quote
        ''' </summary>
        ''' <param name="iSchemeId">Scheme Id for selected instalment quote</param>
        ''' <param name="iVersionId">Version Id for selected instalment quote</param>
        ''' <param name="iCompanyId">Company Id for selected instalment quote</param>
        ''' <remarks></remarks>
        Private Sub ShowDetailsForScheme(ByVal iSchemeId As Integer, ByVal iVersionId As Integer, ByVal iCompanyId As Integer)
            Dim dAmountToFinance As Double
            Dim oInstalmentQuotes As NexusProvider.InstallmentQuoteCollection
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oInstalmentQuote As NexusProvider.InstalmentQuote
            Dim oQuoteForTax As NexusProvider.Quote
            Dim oQuoteForFees As NexusProvider.Quote
            Dim dTotalRiskPremium As Double
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim dTotalRiskTaxExcludedFromInstalment As Double
            Dim dTotalFeeTaxExcludedFromInstalment As Double
            Dim dTotalRiskFeeExcludedFromInstalment As Double
            Dim dAgentCommission As Double
            Dim dTaxOnAgentCommission As Double

            'To get exact tax and fees, wee need to call below given SAM functions
            oQuoteForTax = Cache.Item(ViewState("QuoteForTaxCacheId"))
            If oQuoteForTax Is Nothing Then
                oQuoteForTax = oWebService.GetHeaderAndPolicyTaxByKey(oQuote.InsuranceFileKey, oQuote.BranchCode)
                Cache.Insert(ViewState("QuoteForTaxCacheId"), oQuoteForTax, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
            End If

            oQuoteForFees = Cache.Item(ViewState("QuoteForFeesCacheId"))
            If oQuoteForFees Is Nothing Then
                oQuoteForFees = oWebService.GetHeaderAndPolicyFeesByKey(oQuote.InsuranceFileKey, oQuote.BranchCode)
                Cache.Insert(ViewState("QuoteForFeesCacheId"), oQuoteForFees, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
            End If


            If Cache.Item(ViewState("TotalRiskTaxExcludedFromInstalmentCacheId")) IsNot Nothing _
                Or Cache.Item(ViewState("TotalFeeTaxExcludedFromInstalmentCacheId")) IsNot Nothing _
                Or Cache.Item(ViewState("TotalRiskFeeExcludedFromInstalmentCacheId")) IsNot Nothing Then

                dTotalRiskTaxExcludedFromInstalment = Cache.Item(ViewState("TotalRiskTaxExcludedFromInstalmentCacheId"))
                dTotalFeeTaxExcludedFromInstalment = Cache.Item(ViewState("TotalFeeTaxExcludedFromInstalmentCacheId"))
                dTotalRiskFeeExcludedFromInstalment = Cache.Item(ViewState("TotalRiskFeeExcludedFromInstalmentCacheId"))
            Else
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

                Cache.Insert(ViewState("TotalRiskTaxExcludedFromInstalmentCacheId"), dTotalRiskTaxExcludedFromInstalment, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
                Cache.Insert(ViewState("TotalFeeTaxExcludedFromInstalmentCacheId"), dTotalFeeTaxExcludedFromInstalment, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
                Cache.Insert(ViewState("TotalRiskFeeExcludedFromInstalmentCacheId"), dTotalRiskFeeExcludedFromInstalment, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
            End If

            If (Cache.Item(ViewState("AgentCommissionCacheId")) IsNot Nothing) Or (Cache.Item(ViewState("TaxOnAgentCommissionCacheId")) IsNot Nothing) Then
                dAgentCommission = CType(Cache.Item(ViewState("AgentCommissionCacheId")), Double)
                dTaxOnAgentCommission = CType(Cache.Item(ViewState("TaxOnAgentCommissionCacheId")), Double)
            Else
                Dim oAgentCommission As NexusProvider.EditAgentCommission
                'make SAM call to get the Agent Commission and save in cache for further use
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
                Cache.Insert(ViewState("AgentCommissionCacheId"), dAgentCommission, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
                Cache.Insert(ViewState("TaxOnAgentCommissionCacheId"), dTaxOnAgentCommission, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
            End If

            dAmountToFinance = CType(Session(CNAmountToPay), Double) - (oQuoteForTax.TotalPolicyTaxExcludedFromFinancing + oQuoteForFees.TotalPolicyFeesExcludedFromFinancing + dTotalRiskTaxExcludedFromInstalment + dTotalFeeTaxExcludedFromInstalment + dTotalRiskFeeExcludedFromInstalment)
            dAmountToFinance = dAmountToFinance + dAgentCommission + dTaxOnAgentCommission

            If Cache.Item(ViewState("InstalmentQuotesCacheID")) IsNot Nothing Then
                oInstalmentQuotes = Cache.Item(ViewState("InstalmentQuotesCacheID"))
            Else
                'If process is MTA then we need to display instalment type option 
                If Not Session(CNMTAType) Is Nothing Then
                    'Selection of instalment type will be visible only for MTA
                    pnlInstalmentType.Visible = True
                    'By default First option(AddAndSpread) will be selected 
                    rbInstalmentType.SelectedValue = "0"

                    'In Nexus, we does not have input field for selecting weekday and month day
                    'By default first value is selected in BO.So passing value 1 for these parameters
                    oInstalmentQuotes = oWebService.GetInstalmentQuotes(dAmountToFinance, _
                                                oQuote.CoverStartDate, oQuote.CoverEndDate, oQuote.InceptionDate, _
                                                DateTime.Now, 1, 1, oQuote.InsuranceFileKey, -1, 0, True, oQuote.BranchCode, rbInstalmentType.SelectedValue)

                Else
                    'In Nexus, we does not have input field for selecting weekday and month day
                    'By default first value is selected in BO.So passing value 1 for these parameters
                    oInstalmentQuotes = oWebService.GetInstalmentQuotes(dAmountToFinance, _
                                                oQuote.CoverStartDate, oQuote.CoverEndDate, CDate(ddlFirstPaymentDate.SelectedValue), _
                                                DateTime.Now, 1, CInt(ddlDayinMonth.SelectedValue), oQuote.InsuranceFileKey, -1, 0, True, oQuote.BranchCode)
                End If
                'Add the retrived quotes in cache.So that they can be used throughout the page
                Cache.Insert(ViewState("InstalmentQuotesCacheID"), oInstalmentQuotes, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
            End If


            For Each oInstalmentQuote In oInstalmentQuotes
                If oInstalmentQuote.SchemeNo = iSchemeId AndAlso oInstalmentQuote.SchemeVersion = iVersionId AndAlso oInstalmentQuote.CompanyNo = iCompanyId Then
                    Cache.Insert(ViewState("SelectedInstalmentQuoteCacheId"), oInstalmentQuote, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

                    txtFinancedAmount.Text = oInstalmentQuote.TotalAmountInput
                    txtTotalPayable.Text = oInstalmentQuote.TotalInstalmentsAmount
                    txtTransactions.Text = 1 'As per discussion with Subhankar this will be 1 in Nexus
                    txtInstallements.Text = oInstalmentQuote.InstalmentsToPay
                    txtRate.Text = oInstalmentQuote.InterestRate
                    txtAPR.Text = oInstalmentQuote.AprRate

                    txtDeposit.Text = oInstalmentQuote.DepositAmount
                    txtAdminCharge.Text = oInstalmentQuote.FinanceCharge
                    txtProtectionCharge.Text = oInstalmentQuote.ProtectionAmount
                    txtInterest.Text = oInstalmentQuote.InterestAmount

                    txtFirstInstalmentDate.Text = oInstalmentQuote.FirstInstalmentDate
                    txtFirstInstalment.Text = oInstalmentQuote.FirstInstalmentAmount
                    txtNextInstalment.Text = oInstalmentQuote.NextInstalmentDate
                    txtLastInstalment.Text = oInstalmentQuote.LastInstalmentDate
                    txtOtherInstalment.Text = oInstalmentQuote.OtherInstalmentAmount
                    txtTaxes.Text = oInstalmentQuote.TaxAmount

                    'As per shubhankar.This is the way to get risk premium
                    If oQuote IsNot Nothing AndAlso oQuote.Risks IsNot Nothing Then
                        If oQuote.Risks.Count > 0 Then
                            For Each oRisk As NexusProvider.Risk In oQuote.Risks
                                If oRisk.IsRiskSelected = True Then
                                    dTotalRiskPremium += dTotalRiskPremium + oRisk.Premium
                                End If
                            Next
                        End If
                    End If


                    txtGrossDue.Text = CType(Session(CNAmountToPay), Double) + dAgentCommission + dTaxOnAgentCommission
                    txtTotalFees.Text = oQuoteForFees.TotalPolicyFeesExcludedFromFinancing + oQuoteForFees.TotalRiskFeesExcludedFromFinancing
                    txtTotalTaxes.Text = oQuoteForTax.TotalPolicyTaxExcludedFromFinancing + dTotalRiskTaxExcludedFromInstalment + dTotalFeeTaxExcludedFromInstalment

                    txtTotalAmount.Text = CDec(txtGrossDue.Text) - (CDec(txtTotalFees.Text) + CDec(txtTotalTaxes.Text))

                    txtTotalFeesCollect.Text = oQuoteForFees.TotalFeesOnDeposit
                    txtTotalTaxesCollect.Text = oQuoteForTax.TotalTaxOnDeposit
                    txtMinimumDeposit.Text = oQuoteForTax.TotalTaxOnDeposit + oQuoteForFees.TotalFeesOnDeposit

                    'If Media Type for selected instalment quote is Direct Debit then we need to display BankDetails sestion
                    'And populate ddlAccountType dropdown with the corresponding party bank details for a party
                    'For all other media type bank details are not required. So bank details section will be invisible
                    If oInstalmentQuote.MediaTypeDescription = "Direct Debit" Then
                        pnlBankDetails.Visible = True
                        rfvAccountType.Enabled = True
                        custValAccountType.Enabled = True
                        PopulateAccountType()
                    Else
                        pnlBankDetails.Visible = False
                        ddlAccountType.Items.Clear()
                        rfvAccountType.Enabled = False
                        custValAccountType.Enabled = False
                    End If

                    Exit For
                End If
            Next
        End Sub
        
        Private Sub PopulateInstalmentDates(Optional ByVal iSelectedPlan As Integer = 0)
            ddlFirstPaymentDate.Items.Clear()
            Dim oInstalmentQuotes As NexusProvider.InstallmentQuoteCollection = Cache.Item(ViewState("InstalmentQuotesCacheID"))
            If oInstalmentQuotes(iSelectedPlan).AlignTo = 0 Then
                ddlDayinMonth.Enabled = False
            Else
                ddlDayinMonth.Enabled = True
            End If
            Dim iDaysDelay As Integer = oInstalmentQuotes(iSelectedPlan).DaysDelay
            Dim iStartLimit As Integer = oInstalmentQuotes(iSelectedPlan).StartLimit
            Dim FirstPaymentDateIndex As Integer

            For icount = iDaysDelay To iStartLimit
                ddlFirstPaymentDate.Items.Insert(icount - iDaysDelay, New ListItem(System.DateTime.Today.AddDays(icount).ToShortDateString, System.DateTime.Today.AddDays(icount).ToShortDateString))
            Next
            If ddlFirstPaymentDate.Items.Count = 0 Then
                ddlFirstPaymentDate.Items.Insert(0, New ListItem(System.DateTime.Today.ToShortDateString, System.DateTime.Today.ToShortDateString))
            End If

            Dim FirstDateListItem As ListItem = New ListItem(oInstalmentQuotes(iSelectedPlan).FirstInstalmentDate.ToShortDateString, oInstalmentQuotes(iSelectedPlan).FirstInstalmentDate.ToShortDateString)
            If ddlFirstPaymentDate.Items.Contains(FirstDateListItem) Then
                FirstPaymentDateIndex = ddlFirstPaymentDate.Items.IndexOf(FirstDateListItem)
                ddlFirstPaymentDate.SelectedIndex = FirstPaymentDateIndex
            Else
                ddlFirstPaymentDate.Items.Add(FirstDateListItem)
                FirstPaymentDateIndex = ddlFirstPaymentDate.Items.IndexOf(FirstDateListItem)
                ddlFirstPaymentDate.SelectedIndex = FirstPaymentDateIndex
            End If

        End Sub
        Protected Sub CallGetInstalmentQuotes()
            Dim oInstalmentQuotes As NexusProvider.InstallmentQuoteCollection
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote
            Dim oQuoteForTax As NexusProvider.Quote
            Dim oQuoteForFees As NexusProvider.Quote
            Dim dAmountToFinance As Double

            Dim dTotalRiskTaxExcludedFromInstalment As Decimal
            Dim dTotalFeeTaxExcludedFromInstalment As Decimal
            Dim dTotalRiskFeeExcludedFromInstalment As Double
            Dim dAgentCommission As Double
            Dim dTaxOnAgentCommission As Double
            Try
                'Create oQuote object from Session
                oQuote = Session(CNQuote)

                'To get exact tax and fees, wee need to call below given SAM functions
                oQuoteForTax = Cache.Item(ViewState("QuoteForTaxCacheId"))
                If oQuoteForTax Is Nothing Then
                    oQuoteForTax = oWebService.GetHeaderAndPolicyTaxByKey(oQuote.InsuranceFileKey, oQuote.BranchCode)
                    Cache.Insert(ViewState("QuoteForTaxCacheId"), oQuoteForTax, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
                End If

                oQuoteForFees = Cache.Item(ViewState("QuoteForFeesCacheId"))
                If oQuoteForFees Is Nothing Then
                    oQuoteForFees = oWebService.GetHeaderAndPolicyFeesByKey(oQuote.InsuranceFileKey, oQuote.BranchCode)
                    Cache.Insert(ViewState("QuoteForFeesCacheId"), oQuoteForTax, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
                End If

                dTotalRiskTaxExcludedFromInstalment = Cache.Item(ViewState("TotalRiskTaxExcludedFromInstalmentCacheId"))

                dTotalFeeTaxExcludedFromInstalment = Cache.Item(ViewState("TotalFeeTaxExcludedFromInstalmentCacheId"))

                dTotalRiskFeeExcludedFromInstalment = Cache.Item(ViewState("TotalRiskFeeExcludedFromInstalmentCacheId"))

                dAgentCommission = Cache.Item(ViewState("AgentCommissionCacheId"))

                dTaxOnAgentCommission = Cache.Item(ViewState("TaxOnAgentCommissionCacheId"))
                'dAmountToFinance = CType(Session(CNAmountToPay), Double) - (oQuoteForTax.TotalPolicyTaxExcludedFromFinancing + oQuoteForFees.TotalPolicyFeesExcludedFromFinancing)
                dAmountToFinance = CType(Session(CNAmountToPay), Double) - (oQuoteForTax.TotalPolicyTaxExcludedFromFinancing + oQuoteForFees.TotalPolicyFeesExcludedFromFinancing + dTotalRiskTaxExcludedFromInstalment + dTotalFeeTaxExcludedFromInstalment + dTotalRiskFeeExcludedFromInstalment)
                dAmountToFinance = dAmountToFinance + dAgentCommission + dTaxOnAgentCommission
                'Get installment Quotes for selected instalment type
                'Pass monthday and weekday value as 1 because we does not have corresponding input controls to get values
                'Pass override interest rate and override rate values as 0 because we does not have these properties exposed by SAM
                Dim dtPreferredDate As DateTime = oQuote.InceptionDate
                Dim iMonthDay As Integer = 1
                If ddlFirstPaymentDate.SelectedValue <> "" Then
                    dtPreferredDate = CDate(ddlFirstPaymentDate.SelectedValue)
                End If
                If ddlDayinMonth.SelectedValue <> "" Then
                    iMonthDay = CInt(ddlDayinMonth.SelectedValue)
                End If
                oInstalmentQuotes = oWebService.GetInstalmentQuotes(dAmountToFinance, _
                                                    oQuote.CoverStartDate, oQuote.CoverEndDate, dtPreferredDate, _
                                                    DateTime.Now, 1, iMonthDay, oQuote.InsuranceFileKey, -1, 0, True, oQuote.BranchCode, rbInstalmentType.SelectedValue)

                If Session(CNMTAType) IsNot Nothing AndAlso rbInstalmentType.SelectedIndex = 1 _
                AndAlso oInstalmentQuotes IsNot Nothing AndAlso oInstalmentQuotes.Item(0).FirstInstalmentAmount < 0 Then
                    rbInstalmentType.SelectedIndex = 0
                    Dim sMessage As String = "alert('" + Replace(GetLocalResourceObject("lbl_InstalmentTypeNextInstalment_error"), "{0}", oInstalmentQuotes.Item(0).FirstInstalmentAmount) + "')"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "InstalmentTypeNextInstalment ", sMessage, True)
                    Exit Sub
                End If

                'Remove previous cache
                Cache.Remove(ViewState("InstalmentQuotesCacheID"))

                'Add the retrived quotes in cache.So that they can be used throughout the page
                Cache.Insert(ViewState("InstalmentQuotesCacheID"), oInstalmentQuotes, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

                btnNext.Visible = True
                pnlPlanSummary.Visible = True
                grdInstallmentQuotes.Visible = True
            Catch ex As NexusProvider.NexusException
                If ex.Errors(0).Code = "0" Then
                    '    lblErrorMsg.Visible = True
                    '   lblErrorMsg.Text = ex.Errors(0).Description
                    Cache.Remove(ViewState("InstalmentQuotesCacheID"))
                    btnNext.Visible = False
                    pnlPlanSummary.Visible = False
                    grdInstallmentQuotes.Visible = False
                Else
                    Throw
                End If
            End Try
        End Sub
        ''' <summary>
        ''' Populate account type(ddlAccount) dropdown for party and set first account as selected
        ''' And show detail for selected account 
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub PopulateAccountType()
            Dim sSelectDefaultText As String = GetLocalResourceObject("lbl_Select_DefaultText")
            'Create PartyBankDetail object from cache
            Dim oPartyBankDetail As NexusProvider.BankCollection = Cache.Item(ViewState("PartyBankdetailsCacheID"))
            Dim oTempBankDetailsCollection As New NexusProvider.BankCollection

            ddlAccountType.Items.Clear()
            If oPartyBankDetail IsNot Nothing Then
                For icount = 0 To oPartyBankDetail.Count - 1
                    If oPartyBankDetail.Item(icount).IsDeleted = False And oPartyBankDetail.Item(icount).IsBankItem Then
                        oTempBankDetailsCollection.Add(oPartyBankDetail(icount))
                    End If
                Next
            End If

            oPartyBankDetail = oTempBankDetailsCollection

            If oPartyBankDetail.Count > 0 Then
                ddlAccountType.DataSource = oPartyBankDetail
                ddlAccountType.DataTextField = "AccountType"
                ddlAccountType.DataValueField = "PartyBankKey"
                ddlAccountType.DataBind()
            End If
            ddlAccountType.Items.Insert(0, New ListItem(sSelectDefaultText, ""))

            If oPartyBankDetail.Count = 1 Then
                'Set first item as selected from dropdown
                ddlAccountType.SelectedValue = oPartyBankDetail(0).PartyBankKey
                'Show party bank details for selected account type
                ddlAccountType_SelectedIndexChanged(Me, Nothing)
                
            Else
                ddlAccountType.SelectedIndex = 0
                'Clear and enable all party bank related controls
                txtBankName.Text = String.Empty
                txtAddress1.Text = String.Empty
                txtBranch.Text = String.Empty
                txtBranchCode.Text = String.Empty
                txtAccountName.Text = String.Empty
                txtAccountNumber.Text = String.Empty

            End If
            SetMandatory()
        End Sub

        ''' <summary>
        ''' Show party bank details for selected account from dropdown
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub ddlAccountType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAccountType.SelectedIndexChanged
            'Populate the detail for selected account type.
            'Details will be either for Credit card or Bank(Direct Debit)
            Dim oPartyBankDetails As NexusProvider.BankCollection = Cache.Item(ViewState("PartyBankdetailsCacheID"))
            Dim oPartyBankDetail As NexusProvider.Bank
            Dim sMaskBankAccountNumber As String = CType(GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID().ToString()).MaskBankAccountNumber


            If ddlAccountType.SelectedValue <> "" Then
                For Each oPartyBankDetail In oPartyBankDetails
                    If oPartyBankDetail.PartyBankKey = ddlAccountType.SelectedValue Then
                        txtBankName.Text = oPartyBankDetail.BankName
                        txtAddress1.Text = oPartyBankDetail.PartyBankAddress.Address1
                        txtBranch.Text = oPartyBankDetail.BankBranch
                        txtBranchCode.Text = oPartyBankDetail.BranchCode
                        txtAccountName.Text = oPartyBankDetail.AccountHolderName
                        txtAccountNumber.Text = oPartyBankDetail.AccountNumber


                        'Putting the Mask
                        If sMaskBankAccountNumber And txtAccountNumber.Text.Length > 4 Then
                            Dim sFirststr As String = Mid(txtAccountNumber.Text, 1, txtAccountNumber.Text.Length - 4)
                            Dim sLaststr As String = Mid(txtAccountNumber.Text, sFirststr.Length + 1)
                            For icount As Integer = 0 To sFirststr.Length - 1
                                sFirststr = sFirststr.Replace(sFirststr.Chars(icount), "*")
                            Next
                            txtAccountNumber.Text = sFirststr & sLaststr
                        End If

                        'Insert selected party bank account to cache.So that it can be used to create oPayment object at Next click
                        Cache.Insert(ViewState("SelectedAccountTypeCacheId"), oPartyBankDetail, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

                        Exit For
                    End If
                Next
                SetMandatory()
                Dim oParty As NexusProvider.BaseParty = Nothing

                'Need to Retreive the Data from Session
                If Session(CNParty) IsNot Nothing Then
                    Select Case True
                        Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                            oParty = CType(Session(CNParty), NexusProvider.CorporateParty)
                        Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                            oParty = CType(Session(CNParty), NexusProvider.PersonalParty)
                    End Select
                End If

                Dim BankKey As Integer = 0
                For BankKey = 0 To oParty.BankDetails.Count - 1
                    If oParty.BankDetails(BankKey).PartyBankKey = ddlAccountType.SelectedValue Then
                        hypBankEdit.Visible = True
                        'hypBank.Visible = False
                        If HttpContext.Current.Session.IsCookieless Then
                            If oParty.BankDetails(BankKey).IsPartyBankLinkedWithInst Then
                                hypBankEdit.OnClientClick = "if( confirm('" & GetLocalResourceObject("lbl_EditConfirmMsg").ToString() & "') == 1) {" & "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/BankDetail.aspx?PostbackTo=" & upBankDetails.ClientID.ToString & "&BankKey=" & BankKey & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;}else return false;"
                            Else
                                hypBankEdit.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/BankDetail.aspx?PostbackTo=" & upBankDetails.ClientID.ToString & "&BankKey=" & BankKey & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                            End If
                        Else
                            If oParty.BankDetails(BankKey).IsPartyBankLinkedWithInst Then
                                hypBankEdit.OnClientClick = "if( confirm('" & GetLocalResourceObject("lbl_EditConfirmMsg").ToString() & "') == 1) {" & "tb_show(null ,'" & AppSettings("WebRoot") & "/Modal/BankDetail.aspx?PostbackTo=" & upBankDetails.ClientID.ToString & "&BankKey=" & BankKey & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;}else return false;"
                            Else
                                hypBankEdit.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/Modal/BankDetail.aspx?PostbackTo=" & upBankDetails.ClientID.ToString & "&BankKey=" & BankKey & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                            End If
                        End If
                        Exit For
                        'hypBankEdit.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "Modal/BankDetail.aspx?PostbackTo=" & upBankDetails.ClientID.ToString & "&BankKey=" & BankKey & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                    End If
                Next
                oParty = Nothing
            Else
                'Clear and enable all party bank related controls
                hypBankEdit.Visible = False
                ' hypBank.Visible = True
                txtBankName.Text = String.Empty
                txtAddress1.Text = String.Empty
                txtBranch.Text = String.Empty
                txtBranchCode.Text = String.Empty
                txtAccountName.Text = String.Empty
                txtAccountNumber.Text = String.Empty
            End If

        End Sub

        Protected Sub grdInstallmentQuotes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdInstallmentQuotes.Load
            If grdInstallmentQuotes.PageCount = 1 Then
                grdInstallmentQuotes.AllowPaging = False
            End If
        End Sub



        Protected Sub grdInstallmentQuotes_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdInstallmentQuotes.PageIndexChanging
            Dim oInstalmentQuotes As NexusProvider.InstallmentQuoteCollection = Cache.Item(ViewState("InstalmentQuotesCacheID"))
            grdInstallmentQuotes.DataSource = oInstalmentQuotes
            grdInstallmentQuotes.PageIndex = e.NewPageIndex
            grdInstallmentQuotes.DataBind()
        End Sub

        Protected Sub grdInstallmentQuotes_SelectedIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSelectEventArgs) Handles grdInstallmentQuotes.SelectedIndexChanging
            'Find key values for selected instalment quote
            Dim iSelectedSchemeNumber As Integer = grdInstallmentQuotes.DataKeys(e.NewSelectedIndex).Values("SchemeNo")
            Dim iSelectedSchemeVersion As Integer = grdInstallmentQuotes.DataKeys(e.NewSelectedIndex).Values("SchemeVersion")
            Dim iSelectedCompanyNumber As Integer = grdInstallmentQuotes.DataKeys(e.NewSelectedIndex).Values("CompanyNo")

            'Show instalment details for selected instalment quote

            PopulateInstalmentDates(e.NewSelectedIndex)
            CallGetInstalmentQuotes()
            ShowDetailsForScheme(iSelectedSchemeNumber, iSelectedSchemeVersion, iSelectedCompanyNumber)

        End Sub

        Protected Sub grdInstallmentQuotes_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdInstallmentQuotes.Sorting
            'sort the instalment quotes according to the column clicked
            'we need to store the current sort order in viewstate, and reverse it each time
            Dim oInstalmentQuotes As NexusProvider.InstallmentQuoteCollection = Cache.Item(ViewState("InstalmentQuotesCacheID"))

            oInstalmentQuotes.SortColumn = e.SortExpression
            'check that the sort expression is the same as stored in viewstate as we should start again if reordering by a new column
            Dim _sortDirection As New SortDirection
            If ViewState("SortDirection") = SortDirection.Ascending And ViewState("SortExpression") = e.SortExpression Then
                _sortDirection = SortDirection.Descending
            Else
                _sortDirection = SortDirection.Ascending
            End If
            'store the current sortdirection for comparison on the next sort
            ViewState("SortDirection") = _sortDirection
            'store the SortExpression in viewstate so that we can check if we are sorting by a new column on the next sort
            ViewState("SortExpression") = e.SortExpression
            oInstalmentQuotes.SortingOrder = _sortDirection
            oInstalmentQuotes.Sort()
            CType(sender, GridView).DataSource = oInstalmentQuotes
            CType(sender, GridView).DataBind()
        End Sub

        Protected Sub rbInstalmentType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbInstalmentType.SelectedIndexChanged
            CallGetInstalmentQuotes()
            Dim oInstalmentQuotes As NexusProvider.InstallmentQuoteCollection = Cache.Item(ViewState("InstalmentQuotesCacheID"))

            If oInstalmentQuotes IsNot Nothing AndAlso oInstalmentQuotes.Count > 0 Then
                'Bind the grid with retrieved quotes
                grdInstallmentQuotes.DataSource = oInstalmentQuotes
                grdInstallmentQuotes.DataBind()
                'Set first row as selected from grid
                grdInstallmentQuotes.Rows(0).RowState = DataControlRowState.Selected
                'Show instalment details for selected instalment quote
                ShowDetailsForScheme(oInstalmentQuotes(0).SchemeNo, oInstalmentQuotes(0).SchemeVersion, oInstalmentQuotes(0).CompanyNo)
                pnlPlanSummary.Visible = True
                grdInstallmentQuotes.Visible = True
            End If
        End Sub

        Protected Sub custValAccountType_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles custValAccountType.ServerValidate
            If ddlAccountType.SelectedValue = "" Then
                custValAccountType.IsValid = False
                custValAccountType.ErrorMessage = GetLocalResourceObject("lbl_ErrorMsg_AccountType")
            Else
                custValAccountType.IsValid = True
            End If
        End Sub

        Protected Sub ddlDayinMonth_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDayinMonth.SelectedIndexChanged
            CallGetInstalmentQuotes()
            Dim oInstalmentQuotes As NexusProvider.InstallmentQuoteCollection = Cache.Item(ViewState("InstalmentQuotesCacheID"))
            If grdInstallmentQuotes.SelectedIndex = -1 Then
                ShowDetailsForScheme(oInstalmentQuotes(0).SchemeNo, oInstalmentQuotes(0).SchemeVersion, oInstalmentQuotes(0).CompanyNo)
            Else
                ShowDetailsForScheme(oInstalmentQuotes(grdInstallmentQuotes.SelectedIndex).SchemeNo, oInstalmentQuotes(grdInstallmentQuotes.SelectedIndex).SchemeVersion, oInstalmentQuotes(grdInstallmentQuotes.SelectedIndex).CompanyNo)
            End If

        End Sub

        Protected Sub ddlFirstPaymentDate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFirstPaymentDate.SelectedIndexChanged
            CallGetInstalmentQuotes()
            Dim oInstalmentQuotes As NexusProvider.InstallmentQuoteCollection = Cache.Item(ViewState("InstalmentQuotesCacheID"))
            If grdInstallmentQuotes.SelectedIndex = -1 Then
                ShowDetailsForScheme(oInstalmentQuotes(0).SchemeNo, oInstalmentQuotes(0).SchemeVersion, oInstalmentQuotes(0).CompanyNo)
            Else
                ShowDetailsForScheme(oInstalmentQuotes(grdInstallmentQuotes.SelectedIndex).SchemeNo, oInstalmentQuotes(grdInstallmentQuotes.SelectedIndex).SchemeVersion, oInstalmentQuotes(grdInstallmentQuotes.SelectedIndex).CompanyNo)
            End If
        End Sub
        

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            'Allow "Edit Client" as per BO and web.config file settings
            'If both are TRUE than only allow "Edit Client" otherwise "Edit" button will not be visible
            'Initalize the variables to get and set the editing fucntionality of the user. 
            Dim bEditClient As Boolean
            Dim bIsClientManagerViewOnly As Boolean
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim sReturnCode As NexusProvider.OptionTypeSetting
            Dim sReturnCodePaymentType As NexusProvider.OptionTypeSetting 'Payment Type can only be edited on Party
            Try
                'Get the system Option "Enable Editing in Client Manager"
                sReturnCode = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5000)

                'Get the system Option "Payment Type can only be edited on Party"
                sReturnCodePaymentType = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5062)

                'Checking of User Authority for Editing the Client using client manager
                Dim oUserAuthority As New NexusProvider.UserAuthority
                'Get the user name from session
                oUserAuthority.UserCode = Session(CNLoginName)
                'set the authority options for reverse allocation
                oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.IsClientManagerViewonly
                oWebService = New NexusProvider.ProviderManager().Provider
                'initiate the GetUserAuthority method
                oWebService.GetUserAuthorityValue(oUserAuthority)
                If oUserAuthority.UserAuthorityValue = "1" Then
                    bIsClientManagerViewOnly = True
                Else
                    bIsClientManagerViewOnly = False
                End If

                bEditClient = UserCanDoTask("EditClientDetails")
                'If bEditClientViaClientManager = True and User Can has authority to edit a client
                If sReturnCode IsNot Nothing AndAlso sReturnCode.OptionValue IsNot Nothing Then
                    If sReturnCode.OptionValue = "1" AndAlso bEditClient AndAlso bIsClientManagerViewOnly = False Then
                        If sReturnCodePaymentType IsNot Nothing AndAlso sReturnCodePaymentType.OptionValue IsNot Nothing AndAlso sReturnCodePaymentType.OptionValue <> "1" Then
                            liEditBank.Visible = True
                        Else
                            liEditBank.Visible = False
                        End If
                    ElseIf bIsClientManagerViewOnly Then
                        liEditBank.Visible = False
                    End If
                ElseIf bIsClientManagerViewOnly Then
                    liEditBank.Visible = False
                End If
            Catch ex As NexusProvider.NexusException
                sReturnCode = Nothing
                sReturnCodePaymentType = Nothing
                bEditClient = Nothing
            End Try

            If HttpContext.Current.Session.IsCookieless Then
                hypBank.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/BankDetail.aspx?PostbackTo=" & upBankDetails.ClientID.ToString & "&loc=Instalments&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            Else
                hypBank.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/Modal/BankDetail.aspx?PostbackTo=" & upBankDetails.ClientID.ToString & "&loc=Instalments&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            End If
        End Sub
        Protected Sub SetMandatory()
            Dim oInstalmentQuotes As NexusProvider.InstallmentQuoteCollection = Cache.Item(ViewState("InstalmentQuotesCacheID"))
            Dim oPartyBankDetail As NexusProvider.BankCollection = Cache.Item(ViewState("PartyBankdetailsCacheID"))
            Dim iSelectedPlan As Integer

            If oInstalmentQuotes IsNot Nothing AndAlso oInstalmentQuotes.Count > 0 Then


                If grdInstallmentQuotes.SelectedIndex = -1 Then
                    iSelectedPlan = 0
                Else
                    iSelectedPlan = grdInstallmentQuotes.SelectedIndex
                End If

                If ddlAccountType.SelectedValue <> "" Then

                    If oInstalmentQuotes(iSelectedPlan).BankNameMandatory Then
                        rfvBankName.Enabled = True
                    Else
                        rfvBankName.Enabled = False
                    End If

                    If oInstalmentQuotes(iSelectedPlan).BranchCodeMandatory Then
                        rfvBranchCode.Enabled = True
                    Else
                        rfvBranchCode.Enabled = False
                    End If
                    If oInstalmentQuotes(iSelectedPlan).BranchNameMandatory Then
                        rfvBranch.Enabled = True
                    Else
                        rfvBranch.Enabled = False
                    End If

                    If oInstalmentQuotes(iSelectedPlan).BankAddressMandatory Then
                        rfvAddress1.Enabled = True
                    Else
                        rfvAddress1.Enabled = False
                    End If
                Else
                    rfvAddress1.Enabled = False
                    rfvBranch.Enabled = False
                    rfvBranchCode.Enabled = False
                    rfvBankName.Enabled = False
                End If
            End If
        End Sub



       

    End Class

End Namespace
