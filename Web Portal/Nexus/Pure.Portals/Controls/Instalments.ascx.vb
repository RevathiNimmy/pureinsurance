Imports CMS.Library
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports Nexus.Utils
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports System.Data
Imports System.Drawing
Imports System.Linq

Namespace Nexus
	Partial Class Controls_Instalments
		Inherits System.Web.UI.UserControl

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
		Dim objBasePayment As Nexus.BasePayment
		Dim Partybankidcache As Guid
		Dim dOverrideInterestRate As Double
		Dim dOverrideRate As Double
		Dim bPaymentProtection As Boolean
		Dim dOverrideDepositAmount As Double = Nothing
		Dim bOverrideCommission As Boolean
		Dim oFinancePlanInformation As New NexusProvider.FinancePlanInformation
		Dim sCurrISOCode As String = String.Empty
		Dim bHasPlanSelectionChanged As Boolean = False
		Dim iFinanceToNet As Int16

		Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
			If Not Page.IsPostBack OrElse ViewState("dOverrideInterestRate") Is Nothing Then
				Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
				Dim oPaymentHubEnabled As NexusProvider.OptionTypeSetting
				Dim oPaymentHubConfig As NexusProvider.PaymentHubConfig = GetPaymentHubConfig()
				Try
					oWebService = New NexusProvider.ProviderManager().Provider
					ViewState("dOverrideInterestRate") = -1
					ViewState("dOverrideRate") = 0
					ViewState("bPaymentProtection") = True
					ViewState("dOverrideDepositAmount") = -1
					ViewState("bOverrideCommission") = False
					oPaymentHubEnabled = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.SystemOptionPaymentHubEnabled)
					ViewState("PaymentHubEnabled") = oPaymentHubEnabled.OptionValue
					ViewState("MarkDefaultCard") = oPaymentHubConfig.MarkDefaultCreditCard
					ViewState("DoNotUseForSubsequentPayment") = oPaymentHubConfig.Donotuseoldcarddetailsforsubsequentpayments
					If ViewState("PaymentHubEnabled") = "1" AndAlso ViewState("DoNotUseForSubsequentPayment") <> "1" Then
						FillCardDetails()
					End If
				Catch ex As Exception
				Finally
					oWebService = Nothing
					oPaymentHubEnabled = Nothing
					oPaymentHubConfig = Nothing
				End Try

			End If
		End Sub
		Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
			If Request.UrlReferrer IsNot Nothing Then
				Dim sParentPage As String = Request.UrlReferrer.AbsoluteUri
				If sParentPage.Contains("PremiumDisplay.aspx") Then
					custValAccountType.ValidationGroup = ""
					rfvBankName.ValidationGroup = ""
					rfvAddress1.ValidationGroup = ""
					rfvBranch.ValidationGroup = ""
					rfvAccountType.ValidationGroup = ""
					rfvBranchCode.ValidationGroup = ""
					cvValidateToken.ValidationGroup = ""
				End If
			End If

			If Session(CNMTAType) IsNot Nothing AndAlso Session(CNMTAType) <> MTAType.REINSTATEMENT Then
				If rbInstalmentType.SelectedValue = 2 Then
					divChangeDate.Attributes.Add("style", "display;")
				Else
					divChangeDate.Attributes.Add("style", "display:none;")
				End If
			End If

			'setting the below hidden field so that we can hide the override tab when not required, using javascript.
			If Request.QueryString("Type") IsNot Nothing AndAlso (Request.QueryString("Type") = "NewPlan" OrElse Request.QueryString("Type") = "MTA" OrElse Request.QueryString("Type") = "NewPlanSED") AndAlso UserCanDoTask("OverrideInstalment") Then
				Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ShowHideOverrideTab", "<script type='text/javascript' language='javascript'> $(document).ready(function() { if (document.getElementById('tabOverride') != null) { document.getElementById('tabOverride').style.display = 'block'; } });</script>")
			ElseIf (Session(CNMTAType) IsNot Nothing OrElse Session(CNRenewal) IsNot Nothing) AndAlso UserCanDoTask("OverrideInstalment") Then
				Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ShowHideOverrideTab", "<script type='text/javascript' language='javascript'> $(document).ready(function() { if (document.getElementById('tabOverride') != null) { document.getElementById('tabOverride').style.display = 'block'; } });</script>")
			Else
				Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ShowHideOverrideTab", "<script type='text/javascript' language='javascript'> $(document).ready(function() { if (document.getElementById('tabOverride') != null) {  document.getElementById('tabOverride').style.display = 'none'; } });</script>")
			End If

			If Not IsPostBack Then
				Session(CNPFUseTransCurrency) = 0
				Session(CNPFUserAuthorityValue) = 0
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
						.BIC = sBankData(15)
						.IBAN = sBankData(16)

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
					End With

					oParty.BankDetails.Add(oNewBank)
					Session(CNParty) = oParty

				ElseIf sBankData(0).ToUpper = "UPDATE" Then
					Dim oUpdateBankCollection As NexusProvider.BankCollection = oParty.BankDetails
					Dim oUpdateBanks As NexusProvider.Bank = oParty.BankDetails.Item(CType(sBankData(17), Integer))

					With oUpdateBanks
						.BankPaymentTypeCode = sBankData(1)
						.AccountHolderName = sBankData(2)
						.AccountType = sBankData(3)
						.AccountNumber = sBankData(4)
						.AccountCode = sBankData(4)
						.BranchCode = sBankData(5)
						.BankBranch = sBankData(6)
						.BIC = sBankData(15)
						.IBAN = sBankData(16)

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
					End With

					oUpdateBankCollection.Update(oUpdateBanks)
					Session(CNParty) = oParty
				End If
				Dim objBaseClient As New Nexus.BaseClient
				objBaseClient.UpdatePartyBank()


				hypBankEdit.Visible = False
				Dim oQuote As NexusProvider.Quote
				'Create oQuote object from session
				oQuote = Session(CNQuote)
				Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
				Dim oPartyBankDetails As NexusProvider.BankCollection
				Dim oPartyBankDetailsForInstalment As New NexusProvider.BankCollection
				Try


					oPartyBankDetails = oWebService.GetPartyBankDetails(oQuote.PartyKey)

					'Populate Party bank Details
					oParty.BankDetails = oPartyBankDetails
					Session(CNParty) = oParty

					'Filter Valid Accounts for Instalment (should be for ANY or INS type)

					For Each oPartyBankDetail In oPartyBankDetails
						If (oPartyBankDetail.BankPaymentTypeCode.ToUpper() = "INS" Or oPartyBankDetail.BankPaymentTypeCode.ToUpper() = "ANY") AndAlso
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
					HttpContext.Current.Session.Remove(CNFinancePlan)
				End Try

			End If

			If hdnIsSuppressDecimals.Value Is Nothing OrElse Trim(hdnIsSuppressDecimals.Value) = "" Then
				Dim oWebService As NexusProvider.ProviderBase = Nothing
				Dim oSuppressDecimalOptionType As New NexusProvider.OptionTypeSetting
				oWebService = New NexusProvider.ProviderManager().Provider
				oSuppressDecimalOptionType = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, NexusProvider.ProductOptions.SuppressDecimalValues)
				If oSuppressDecimalOptionType IsNot Nothing Then
					hdnIsSuppressDecimals.Value = oSuppressDecimalOptionType.OptionValue
					If Trim(hdnIsSuppressDecimals.Value) = "1" Then
						txtOverrideDeposit.Attributes.Add("onpaste", "javascript:return false;")
					End If


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
		Private Sub ShowDetailsForScheme(ByVal iSchemeId As Integer, ByVal iVersionId As Integer, ByVal iCompanyId As Integer, Optional ByVal iFrequencyId As Integer = 0)
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
			Dim nPartyBankId As Integer = 0
			Dim dGrossDue As Double
			'To get exact tax and fees, wee need to call below given SAM functions
			oQuoteForTax = Cache.Item(ViewState("QuoteForTaxCacheId"))
			If oQuoteForTax Is Nothing Then
				Try
					oQuoteForTax = oWebService.GetHeaderAndPolicyTaxByKey(oQuote.InsuranceFileKey, oQuote.BranchCode)
				Catch
					oQuoteForTax = New NexusProvider.Quote()
				End Try
				Cache.Insert(ViewState("QuoteForTaxCacheId"), oQuoteForTax, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
			End If

			oQuoteForFees = Cache.Item(ViewState("QuoteForFeesCacheId"))
			If oQuoteForFees Is Nothing Then
				Try
					oQuoteForFees = oWebService.GetHeaderAndPolicyFeesByKey(oQuote.InsuranceFileKey, oQuote.BranchCode)
				Catch
					oQuoteForFees = New NexusProvider.Quote()
				End Try
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
				Try
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
				Catch
					' For recovery claims, agent commission is not applicable — default to zero
					dAgentCommission = 0
					dTaxOnAgentCommission = 0
				End Try
				Cache.Insert(ViewState("AgentCommissionCacheId"), dAgentCommission, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
				Cache.Insert(ViewState("TaxOnAgentCommissionCacheId"), dTaxOnAgentCommission, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
			End If

			Dim sAgentType As String = String.Empty
			If Session(CNAgentType) IsNot Nothing Then
				sAgentType = Session(CNAgentType).ToString.Trim.ToUpper
			End If
			If Cache.Item(ViewState("InstalmentQuotesCacheID")) IsNot Nothing Then
				oInstalmentQuotes = Cache.Item(ViewState("InstalmentQuotesCacheID"))
				For Each oInstalmentQuote In oInstalmentQuotes
					If oInstalmentQuote.SchemeNo = iSchemeId AndAlso oInstalmentQuote.SchemeVersion = iVersionId AndAlso oInstalmentQuote.CompanyNo = iCompanyId And oInstalmentQuote.FrequencyID = iFrequencyId Then
						iFinanceToNet = oInstalmentQuote.FinanceToNet
						Exit For
					End If
				Next
			End If

			If sAgentType = "BROKER" Then
				dAmountToFinance = CType(Session(CNAmountToPay), Double) - (oQuoteForTax.TotalPolicyTaxExcludedFromFinancing + oQuoteForFees.TotalPolicyFeesExcludedFromFinancing + dTotalRiskTaxExcludedFromInstalment + dTotalFeeTaxExcludedFromInstalment + dTotalRiskFeeExcludedFromInstalment)
				If iFinanceToNet = 1 Then
					dAmountToFinance = dAmountToFinance - (dAgentCommission + dTaxOnAgentCommission)
				Else
					dAmountToFinance = dAmountToFinance + dAgentCommission
				End If
			Else
				dAmountToFinance = CType(Session(CNAmountToPay), Double) - (oQuoteForTax.TotalPolicyTaxExcludedFromFinancing + oQuoteForFees.TotalPolicyFeesExcludedFromFinancing + dTotalRiskTaxExcludedFromInstalment + dTotalFeeTaxExcludedFromInstalment + dTotalRiskFeeExcludedFromInstalment)
			End If

			dOverrideInterestRate = ViewState("dOverrideInterestRate")
			dOverrideRate = ViewState("dOverrideRate")
			bPaymentProtection = ViewState("bPaymentProtection")
			dOverrideDepositAmount = ViewState("dOverrideDepositAmount")
			bOverrideCommission = ViewState("bOverrideCommission")

			If Cache.Item(ViewState("InstalmentQuotesCacheID")) IsNot Nothing Then
				oInstalmentQuotes = Cache.Item(ViewState("InstalmentQuotesCacheID"))
				For Each oInstalmentQuote In oInstalmentQuotes
					If oInstalmentQuote.SchemeNo = iSchemeId AndAlso oInstalmentQuote.SchemeVersion = iVersionId AndAlso oInstalmentQuote.CompanyNo = iCompanyId And oInstalmentQuote.FrequencyID = iFrequencyId Then
						iFinanceToNet = oInstalmentQuote.FinanceToNet
						Exit For
					End If
				Next
			Else
				'If process is MTA then we need to display instalment type option 
				If Not Session(CNMTAType) Is Nothing Then
					'Selection of instalment type will be visible only for MTA
					pnlInstalmentType.Visible = True
					'By default First option(AddAndSpread) will be selected 
					rbInstalmentType.SelectedValue = "0"

					'In Nexus, we does not have input field for selecting weekday and month day
					'By default first value is selected in BO.So passing value 1 for these parameters
					oInstalmentQuotes = oWebService.GetInstalmentQuotes(v_dAmountToFinance:=dAmountToFinance,
												v_dtStartDate:=oQuote.CoverStartDate, v_dtEndDate:=oQuote.CoverEndDate, v_dtPreferredDate:=Date.Today,
												v_dtQuoteDate:=DateTime.Now, v_iWeekDay:=1, v_iMonthDay:=1, v_iInsuranceFileKey:=oQuote.InsuranceFileKey,
												v_dOverrideInterestRate:=dOverrideInterestRate, v_dOverrideRate:=dOverrideRate, v_bPaymentProtection:=bPaymentProtection, v_sBranchCode:=oQuote.BranchCode,
												v_iInstallmentType:=rbInstalmentType.SelectedValue, r_nPartyBankId:=nPartyBankId, bIsUseTransactionCurrency:=chkUseTransactionCurrency.Checked,
												v_OverrideDepositAmount:=dOverrideDepositAmount)

				Else
					'In Nexus, we does not have input field for selecting weekday and month day
					'By default first value is selected in BO.So passing value 1 for these parameters
					oInstalmentQuotes = oWebService.GetInstalmentQuotes(v_dAmountToFinance:=dAmountToFinance,
												v_dtStartDate:=oQuote.CoverStartDate, v_dtEndDate:=oQuote.CoverEndDate, v_dtPreferredDate:=CDate(ddlFirstPaymentDate.SelectedValue),
												v_dtQuoteDate:=DateTime.Now, v_iWeekDay:=1, v_iMonthDay:=CInt(ddlDayinMonth.SelectedValue), v_iInsuranceFileKey:=oQuote.InsuranceFileKey,
												v_dOverrideInterestRate:=dOverrideInterestRate, v_dOverrideRate:=dOverrideRate, v_bPaymentProtection:=bPaymentProtection,
												bIsUseTransactionCurrency:=chkUseTransactionCurrency.Checked, v_OverrideDepositAmount:=dOverrideDepositAmount)
				End If
				'Add the retrived quotes in cache.So that they can be used throughout the page
				Cache.Insert(ViewState("InstalmentQuotesCacheID"), oInstalmentQuotes, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
				Cache.Insert(ViewState("Partybankidcache"), nPartyBankId, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
			End If

			sCurrISOCode = Session(CNCurrenyCode)

			If chkUseTransactionCurrency.Checked Then
				sCurrISOCode = oQuote.CurrencyCode
			End If

			For Each oInstalmentQuote In oInstalmentQuotes



				If oInstalmentQuote.SchemeNo = iSchemeId AndAlso oInstalmentQuote.SchemeVersion = iVersionId AndAlso oInstalmentQuote.CompanyNo = iCompanyId And oInstalmentQuote.FrequencyID = iFrequencyId Then
					Cache.Insert(ViewState("SelectedInstalmentQuoteCacheId"), oInstalmentQuote, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

					If Cache.Item(ViewState("Partybankidcache")) IsNot Nothing Then
						nPartyBankId = Cache.Item(ViewState("Partybankidcache"))  ' CType(Cache.Item(ViewState("Partybankidcache")), Integer)
					End If
					iFinanceToNet = oInstalmentQuote.FinanceToNet
					txtFinancedAmount.Text = oInstalmentQuote.TotalAmountInput.ToString(Format("0.00"))
					If iFinanceToNet = 1 Then
						txtFinancedAmount.Text = (oInstalmentQuote.TotalAmountInput - (dAgentCommission + dTaxOnAgentCommission)).ToString(Format("0.00"))
					End If

					If (oInstalmentQuote.TotalAmountInput.ToString(Format("0.00")) <= 0) Then
						chkOverrideDeposit.Enabled = False
					Else
						chkOverrideDeposit.Enabled = True
					End If
					txtTotalPayable.Text = oInstalmentQuote.TotalInstalmentsAmount.ToString(Format("0.00"))
					txtTransactions.Text = 1 'As per discussion with Subhankar this will be 1 in Nexus
					txtInstallements.Text = oInstalmentQuote.InstalmentsToPay
					txtRate.Text = oInstalmentQuote.InterestRate.ToString(Format("0.00"))
					txtAPR.Text = oInstalmentQuote.AprRate.ToString(Format("0.00"))

					txtDeposit.Text = oInstalmentQuote.DepositAmount.ToString(Format("0.00"))
					txtAdminCharge.Text = oInstalmentQuote.FinanceCharge.ToString(Format("0.00"))
					txtProtectionCharge.Text = oInstalmentQuote.ProtectionAmount.ToString(Format("0.00"))
					txtInterest.Text = oInstalmentQuote.InterestAmount.ToString(Format("0.00"))

					'Changes for WPR005 - FirstInstalmentdate must always be equal to FirstPaymentDate. Original NB case has been left unchanged
					If ddlFirstPaymentDate.SelectedValue <> "" AndAlso Session(CNMode) <> Mode.View AndAlso Session(CNMTAType) Is Nothing Then
						txtFirstInstalmentDate.Text = ddlFirstPaymentDate.SelectedValue
					Else
						txtFirstInstalmentDate.Text = oInstalmentQuote.FirstInstalmentDate
						'ddlFirstPaymentDate.SelectedValue = txtFirstInstalmentDate.Text
					End If
					If Session(CNMTAType) IsNot Nothing AndAlso (rbInstalmentType.SelectedValue = "0" Or rbInstalmentType.SelectedValue = "1") Then
						ddlFirstPaymentDate.Items.Insert(ddlFirstPaymentDate.Items.Count, New ListItem(oInstalmentQuote.FirstInstalmentDate, oInstalmentQuote.FirstInstalmentDate))
						txtFirstInstalmentDate.Text = oInstalmentQuote.FirstInstalmentDate
						ddlFirstPaymentDate.SelectedValue = txtFirstInstalmentDate.Text
					End If
					txtFirstInstalment.Text = oInstalmentQuote.FirstInstalmentAmount.ToString(Format("0.00"))
					txtNextInstalment.Text = oInstalmentQuote.NextInstalmentDate
					txtLastInstalment.Text = oInstalmentQuote.LastInstalmentDate
					txtOtherInstalment.Text = oInstalmentQuote.OtherInstalmentAmount.ToString(Format("0.00"))
					txtTaxes.Text = oInstalmentQuote.TaxAmount.ToString(Format("0.00"))
					If rbInstalmentType.SelectedValue = 0 Or rbInstalmentType.SelectedValue = 1 Then
						If Convert.ToDecimal(oInstalmentQuote.DepositAmount) > 0 Then ' AndAlso chkOverrideDeposit.Checked Then
							rfvOverrideDepositRange.MaximumValue = oInstalmentQuote.TotalAmountInput.ToString(Format("0.00"))
						Else
							rfvOverrideDepositRange.MaximumValue = oInstalmentQuote.TotalInstalmentsAmount.ToString(Format("0.00"))
						End If
					Else
						rfvOverrideDepositRange.MaximumValue = oInstalmentQuote.TotalAmountInput.ToString(Format("0.00"))
					End If

					'THIS IS IN CASE OF ANNUAL PLAN -SPECIAL CASE
					If oInstalmentQuote.NextInstalmentDate = oInstalmentQuote.LastInstalmentDate AndAlso oInstalmentQuote.InstalmentsToPay = 1 AndAlso
					  ddlDayinMonth.SelectedValue <> CDate(txtFirstInstalmentDate.Text).Day AndAlso Session(CNIsTrueMonthlyPolicy) <> True Then

						txtNextInstalment.Text = CDate(txtFirstInstalmentDate.Text)
						txtLastInstalment.Text = CDate(txtFirstInstalmentDate.Text)


					End If

					If oInstalmentQuote.FirstInstalmentAlignWithDayInMonth = 0 AndAlso ddlDayinMonth.Text = "" Then
						ddlDayinMonth.Text = Microsoft.VisualBasic.DateAndTime.Day(CDate(oInstalmentQuote.NextInstalmentDate))
					End If

					If Session(CNPFUseTransCurrency) = 0 Then
						CheckUseTransCurrency(oQuote.BaseCurrencyID, oQuote.TransCurrencyID, oInstalmentQuote.UseTransCurrncy)
						Session(CNPFUseTransCurrency) = 1
					End If

					If ddlFirstPaymentDate.Items.Count = 0 Then
						PopulateInstalmentDates(0, oInstalmentQuote.FirstInstalmentDate)
					End If

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

					If Session(CNAgentType) IsNot Nothing AndAlso Session(CNAgentType).ToString.Trim.ToUpper = "BROKER" Then
						dGrossDue = CType(Session(CNAmountToPay), Double) + dAgentCommission
					Else
						dGrossDue = CType(Session(CNAmountToPay), Double)
					End If
					Dim dTotalFees = (oQuoteForFees.TotalPolicyFeesExcludedFromFinancing + oQuoteForFees.TotalRiskFeesExcludedFromFinancing).ToString(Format("0.00"))
					Dim dTotalTaxes = (oQuoteForTax.TotalPolicyTaxExcludedFromFinancing + dTotalRiskTaxExcludedFromInstalment + dTotalFeeTaxExcludedFromInstalment).ToString(Format("0.00"))
					Dim dTotalFeesCollect As Double = oQuoteForFees.TotalFeesOnDeposit

					Double.TryParse(dTotalFeesCollect, oQuoteForFees.TotalFeesOnDeposit)
					txtGrossDue.Text = dGrossDue
					txtTotalFees.Text = dTotalFees
					txtTotalTaxes.Text = dTotalTaxes
					Dim dTotalFinanceAmount As Double = CDec(dGrossDue)
					If iFinanceToNet = 1 Then
						dTotalFinanceAmount = (dTotalFinanceAmount - (dAgentCommission + dTaxOnAgentCommission))
					End If
					txtTotalAmount.Text = (CDec(dGrossDue) - (CDec(dTotalFees) + CDec(dTotalTaxes))).ToString(Format("0.00"))

					txtTotalFeesCollect.Text = (CDbl(dTotalFeesCollect)).ToString(Format("0.00"))
					txtTotalTaxesCollect.Text = (CDec(oQuoteForTax.TotalTaxOnDeposit)).ToString(Format("0.00"))
					txtMinimumDeposit.Text = (oQuoteForTax.TotalTaxOnDeposit + oQuoteForFees.TotalFeesOnDeposit).ToString(Format("0.00"))

					If Session(CNFinancePlan) IsNot Nothing AndAlso Not Page.IsPostBack Then
						Dim oFinancePlan As New NexusProvider.FinancePlan
						oFinancePlan = CType(Session(CNFinancePlan), NexusProvider.FinancePlan)
						ddlDayinMonth.SelectedValue = oFinancePlan.DayOfWeekOrMonth
						If Session(CNMTAType) IsNot Nothing AndAlso (rbInstalmentType.SelectedValue = "0" Or rbInstalmentType.SelectedValue = "1") Then
						ElseIf oInstalmentQuote.FirstInstalmentDate > oFinancePlan.FirstInstalmentDate Then
							ddlFirstPaymentDate.SelectedValue = Convert.ToDateTime(oInstalmentQuote.FirstInstalmentDate).ToString("dd/MM/yyyy")
							txtFirstInstalmentDate.Text = Convert.ToDateTime(oInstalmentQuote.FirstInstalmentDate).ToString("dd/MM/yyyy")
							If oInstalmentQuote.NextInstalmentDate = oInstalmentQuote.LastInstalmentDate AndAlso oInstalmentQuote.InstalmentsToPay = 1 AndAlso
										ddlDayinMonth.SelectedValue <> CDate(txtFirstInstalmentDate.Text).Day Then
								ddlDayinMonth.SelectedValue = CDate(txtFirstInstalmentDate.Text).Day
							End If
						Else
							ddlFirstPaymentDate.SelectedValue = Convert.ToDateTime(oFinancePlan.FirstInstalmentDate).ToString("dd/MM/yyyy")
							ddlFirstPaymentDate.Focus()
							txtFirstInstalmentDate.Text = oFinancePlan.FirstInstalmentDate
							txtNextInstalment.Text = oFinancePlan.NextInstalmentDate
							txtLastInstalment.Text = oFinancePlan.LastInstalmentDate
						End If

					End If

					'If Media Type for selected instalment quote is Direct Debit then we need to display BankDetails sestion
					'And populate ddlAccountType dropdown with the corresponding party bank details for a party
					'For all other media type bank details are not required. So bank details section will be invisible

					If oInstalmentQuote.MediaTypeDescription = "Direct Debit" OrElse
						oInstalmentQuote.MediaTypeDescription.Trim.ToUpper = "EFT" Then
						pnlBankDetails.Visible = True
						rfvAccountType.Enabled = True
						custValAccountType.Enabled = True
						If ddlAccountType.SelectedValue <> "" AndAlso nPartyBankId = 0 Then 'This gets executed from ddlAccountType_SelectedIndexChanged and was resetting the values
							nPartyBankId = ddlAccountType.SelectedValue
						End If
						PopulateAccountType(nPartyBankId)
						'WPR005
						pnlCreditCardDetails.Visible = False
					ElseIf oInstalmentQuote.MediaTypeDescription.Contains("Credit Card") AndAlso (Request.QueryString("Type") IsNot Nothing) Then
						'WPR005
						If ViewState("PaymentHubEnabled") = "1" Then
							Dim lnkButtonSave As System.Web.UI.WebControls.LinkButton
							If grdCard.Rows.Count = 0 Then
								lnkButtonSave = Me.Parent.FindControl("btnSave")
								If lnkButtonSave IsNot Nothing Then
									lnkButtonSave.Enabled = False
								End If
							ElseIf ViewState("MarkDefaultCard") <> "1" Then
								lnkButtonSave = Me.Parent.FindControl("btnSave")
								If lnkButtonSave IsNot Nothing Then
									lnkButtonSave.Attributes.Add("onclick", "javascript:return ValidateSelectedCard();")
								End If
							End If
							pnlCreditCard.Visible = True
							pnlCreditCardDetails.Visible = False
						Else
							Dim oCreditCard As New NexusProvider.CreditCard
							Session(CNCardDetails) = oCreditCard
							pnlCreditCardDetails.Visible = True
							pnlCreditCard.Visible = False
							PopulateTokenNumber()
							hdnMediaType.Value = "Credit Card"
						End If

						pnlBankDetails.Visible = False
						ddlAccountType.Items.Clear()
						rfvAccountType.Enabled = False
						custValAccountType.Enabled = False

					Else
						pnlBankDetails.Visible = False
						ddlAccountType.Items.Clear()
						rfvAccountType.Enabled = False
						custValAccountType.Enabled = False
						'WPR005
						pnlCreditCardDetails.Visible = False
					End If
					If oInstalmentQuote.SchemeTypeCode.Trim.ToUpper() = "TP" Then
						txtFirstInstalmentDate.Text = String.Empty
						txtNextInstalment.Text = String.Empty
						txtLastInstalment.Text = String.Empty
					End If
					Exit For
				End If
			Next
		End Sub


		Public Sub PopulateTokenNumber()
			ddlExistingTokens.Items.Clear()

			'the first index should be none item.
			ddlExistingTokens.Items.Add("none")
			Dim oFinancePlanDetails As New NexusProvider.FinancePlanDetails
			Dim oParty As NexusProvider.BaseParty = Session(CNParty)
			Dim oPartyBankDetails As NexusProvider.BankCollection
			Dim oPartyBankDetail As NexusProvider.Bank
			Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
			Dim sTokenNumber As String = String.Empty
			Dim sPartyBankKey As String = String.Empty
			If oParty IsNot Nothing Then
				'Base on the session value is personal / corporate client is loaded
				Select Case True
					Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
						oParty = CType(Session(CNParty), NexusProvider.PersonalParty)
					Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
						oParty = CType(Session(CNParty), NexusProvider.CorporateParty)
				End Select
				'Populate Party bank Details
				oPartyBankDetails = oWebService.GetPartyBankDetails(oParty.Key)
				If oPartyBankDetails IsNot Nothing Then
					For Each oPartyBankDetail In oPartyBankDetails
						If oPartyBankDetail.CreditCard IsNot Nothing Then
							sTokenNumber = oPartyBankDetail.CreditCard.ManualAuthCode
							sPartyBankKey = oPartyBankDetail.PartyBankKey.ToString()
							If Not String.IsNullOrEmpty(sTokenNumber) Then
								Dim lstTokenNumber As New ListItem(sTokenNumber, sPartyBankKey)
								If ddlExistingTokens IsNot Nothing AndAlso ddlExistingTokens.Items IsNot Nothing AndAlso ddlExistingTokens.Items.Count > 0 AndAlso ddlExistingTokens.Items.FindByText(sTokenNumber) Is Nothing Then
									ddlExistingTokens.Items.Add(lstTokenNumber)
								End If
							End If
						End If
					Next
				End If

				If Session(CNFinancePlanDetails) IsNot Nothing Then
					oFinancePlanDetails = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).PremiumFinanceDetails
					If oFinancePlanDetails IsNot Nothing AndAlso Not String.IsNullOrEmpty(oFinancePlanDetails.Authcode) AndAlso ddlExistingTokens.Items.IndexOf(ddlExistingTokens.Items.FindByValue(GetPartyBankKeyOnToken(oFinancePlanDetails.Authcode))) > 0 Then
						ddlExistingTokens.SelectedValue = GetPartyBankKeyOnToken(oFinancePlanDetails.Authcode)
					End If
				End If

				If ddlExistingTokens.SelectedItem.Text <> "none" Then
					RqdTokenNo.Enabled = False
				End If
			End If
		End Sub

		''' <summary>
		''' Populate Instalment Dates
		''' </summary>
		''' <param name="iSelectedPlan"></param>
		''' <remarks></remarks>
		Private Sub PopulateInstalmentDates(Optional ByVal iSelectedPlan As Integer = 0, Optional v_sFirstInstalmentDate As String = "")
			hdnFirstPaymentDateSelected.Value = ddlFirstPaymentDate.SelectedValue
			ddlFirstPaymentDate.Items.Clear()
			If Cache.Item(ViewState("InstalmentQuotesCacheID")) Is Nothing Then
				CallGetInstalmentQuotes()
			End If
			Dim oInstalmentQuotes As NexusProvider.InstallmentQuoteCollection = Cache.Item(ViewState("InstalmentQuotesCacheID"))
			Dim oQuote As NexusProvider.Quote = Nothing

			If oInstalmentQuotes.Count = 1 Then
				iSelectedPlan = 0
			End If
			If hdnFirstPaymentDateSelected.Value = "" Then
				hdnFirstPaymentDateSelected.Value = oInstalmentQuotes(iSelectedPlan).FirstInstalmentDate
			End If
			If oInstalmentQuotes(iSelectedPlan).AlignTo = 1 Then
				ddlDayinMonth.Enabled = True
			End If
			If oInstalmentQuotes(iSelectedPlan).AlignTo = 0 Then
				ddlDayinMonth.Enabled = False
			Else
				If oInstalmentQuotes(iSelectedPlan).FirstInstalmentAlignWithDayInMonth = 1 Then
					ddlFirstPaymentDate.Items.Insert(0, New ListItem(System.DateTime.Today.ToShortDateString, System.DateTime.Today.ToShortDateString))
					ddlFirstPaymentDate.Enabled = False
					Exit Sub
				End If
			End If
			Dim iDaysDelay As Integer = oInstalmentQuotes(iSelectedPlan).DaysDelay
			Dim iStartLimit As Integer = oInstalmentQuotes(iSelectedPlan).StartLimit

			oQuote = Session(CNQuote)

			'Changes for WPR005, left the existing code unchanged.
			If Request.QueryString("Type") IsNot Nothing AndAlso hvActualIndex.Value <> "" Then
				iDaysDelay = oInstalmentQuotes(CInt(hvActualIndex.Value)).DaysDelay
				iStartLimit = oInstalmentQuotes(CInt(hvActualIndex.Value)).StartLimit
			End If

			ddlFirstPaymentDate.Enabled = True
			'Changes for WPR005, left the existing code unchanged.
			If Request.QueryString("Type") IsNot Nothing AndAlso hvActualIndex.Value <> "" Then
				iDaysDelay = oInstalmentQuotes(CInt(hvActualIndex.Value)).DaysDelay
				iStartLimit = oInstalmentQuotes(CInt(hvActualIndex.Value)).StartLimit
			End If

			If Request.QueryString("Type") = "NewPlan" AndAlso oQuote.CoverStartDate < System.DateTime.Today Then
				For icount = iDaysDelay To iStartLimit
					ddlFirstPaymentDate.Items.Insert(icount - iDaysDelay, New ListItem(System.DateTime.Today.AddDays(icount).ToShortDateString, System.DateTime.Today.AddDays(icount).ToShortDateString))
				Next

			ElseIf Request.QueryString("Type") Is Nothing AndAlso oQuote.CoverStartDate < System.DateTime.Today Then
				For icount = iDaysDelay To iStartLimit
					ddlFirstPaymentDate.Items.Insert(icount - iDaysDelay, New ListItem(System.DateTime.Today.AddDays(icount).ToShortDateString, System.DateTime.Today.AddDays(icount).ToShortDateString))
				Next

			Else
				PopulateInstalmentDatesForPlanMTA(iDaysDelay, iStartLimit, oQuote)
			End If
			If ddlFirstPaymentDate.Items.Count = 0 Then
				'Making changes as per WPR005 requirement, not changing the existing code. Defect 8803
				If Request.QueryString("Type") IsNot Nothing AndAlso Request.QueryString("Type") = "NewPlanSED" Then
					ddlFirstPaymentDate.Items.Insert(0, New ListItem(oQuote.CoverStartDate.AddDays(iDaysDelay).ToShortDateString, oQuote.CoverStartDate.AddDays(iDaysDelay).ToShortDateString))
				Else
					ddlFirstPaymentDate.Items.Insert(0, New ListItem(oQuote.CoverStartDate.AddDays(iDaysDelay).ToShortDateString, oQuote.CoverStartDate.AddDays(iDaysDelay).ToShortDateString))
				End If
			End If
		End Sub

		''' <summary>
		''' This Function will Execute for the Plan MTA to calculate the dates by taking the next instalment due date of previous Plan.
		''' </summary>
		''' <param name="nDaysDelay"></param>
		''' <param name="nStartLimit"></param>
		''' <param name="oQuote"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function PopulateInstalmentDatesForPlanMTA(ByVal nDaysDelay As Integer,
														   ByVal nStartLimit As Integer,
														   ByVal oQuote As NexusProvider.Quote,
														   Optional ByVal bIsdateHasChanged As Boolean = False) As Boolean
			Dim bReturn As Boolean = False
			Dim nDefaultIndex As Integer

			If Not Request.QueryString("Type") Is Nothing AndAlso IsDate(hdnNextInstalmentDueDate.Value) Then
				'This condition will execute to add the default previous date in Dropdown list
				'once user has changed the date from the dropdown list it will be removed .
				If Not bIsdateHasChanged AndAlso Not UCase(Request.QueryString("Type")).ToString.StartsWith("NEWPLAN") Then
					ddlFirstPaymentDate.Items.Insert(0,
													 New ListItem(Convert.ToDateTime(hdnNextInstalmentDueDate.Value).AddDays(0).ToShortDateString,
																  Convert.ToDateTime(hdnNextInstalmentDueDate.Value).AddDays(0).ToShortDateString))
					nDefaultIndex = 1
				End If
				For icount = nDaysDelay To nStartLimit
					ddlFirstPaymentDate.Items.Insert(icount - (nDaysDelay - nDefaultIndex),
													 New ListItem(Convert.ToDateTime(hdnNextInstalmentDueDate.Value).AddDays(icount).ToShortDateString,
																  Convert.ToDateTime(hdnNextInstalmentDueDate.Value).AddDays(icount).ToShortDateString)
																 )
				Next
			Else
				'This case will run incase hdnNextInstalmentDueDate.Value is nothing as run previous.
				For icount = nDaysDelay To nStartLimit
					ddlFirstPaymentDate.Items.Insert(icount - nDaysDelay,
													 New ListItem(oQuote.CoverStartDate.AddDays(icount).ToShortDateString,
																  oQuote.CoverStartDate.AddDays(icount).ToShortDateString))
					''in case of new plan set selected value for first payment date and avoid the value from rebinding again
					If Session(CNRITransactionType) = "NB" And Not String.IsNullOrEmpty(hdnFirstPaymentDateSelected.Value) And Not bHasPlanSelectionChanged Then
						If oQuote.CoverStartDate.AddDays(icount).ToShortDateString = hdnFirstPaymentDateSelected.Value Then
							ddlFirstPaymentDate.SelectedValue = hdnFirstPaymentDateSelected.Value
						End If
					End If
				Next
			End If
			Return bReturn
		End Function

		''' <summary>
		''' To retrive the instalment quote related information
		''' </summary>
		''' <remarks></remarks>
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

				Dim sAgentType As String = String.Empty
				If Session(CNAgentType) IsNot Nothing Then
					sAgentType = Session(CNAgentType).ToString.Trim.ToUpper
				End If

				If sAgentType = "BROKER" Then
					dAmountToFinance = CType(Session(CNAmountToPay), Double) - (oQuoteForTax.TotalPolicyTaxExcludedFromFinancing + oQuoteForFees.TotalPolicyFeesExcludedFromFinancing + dTotalRiskTaxExcludedFromInstalment + dTotalFeeTaxExcludedFromInstalment + dTotalRiskFeeExcludedFromInstalment)
					dAmountToFinance = dAmountToFinance + dAgentCommission
				Else
					dAmountToFinance = CType(Session(CNAmountToPay), Double) - (oQuoteForTax.TotalPolicyTaxExcludedFromFinancing + oQuoteForFees.TotalPolicyFeesExcludedFromFinancing + dTotalRiskTaxExcludedFromInstalment + dTotalFeeTaxExcludedFromInstalment + dTotalRiskFeeExcludedFromInstalment)
				End If

				dOverrideInterestRate = ViewState("dOverrideInterestRate")
				dOverrideRate = ViewState("dOverrideRate")
				bPaymentProtection = ViewState("bPaymentProtection")
				dOverrideDepositAmount = ViewState("dOverrideDepositAmount")
				bOverrideCommission = ViewState("bOverrideCommission")

				'Get installment Quotes for selected instalment type
				'Pass monthday and weekday value as 1 because we does not have corresponding input controls to get values
				'Pass override interest rate and override rate values as 0 because we does not have these properties exposed by SAM
				Dim dtPreferredDate As DateTime = Date.Today
				Dim iMonthDay As Integer = 1
				Dim nWeekDay As Integer = 1
				If ddlFirstPaymentDate.SelectedValue <> "" Then
					dtPreferredDate = CDate(ddlFirstPaymentDate.SelectedValue)
				End If
				If ddlDayinMonth.SelectedValue <> "" Then
					iMonthDay = CInt(ddlDayinMonth.SelectedValue)
				Else
					iMonthDay = 1
				End If
				If UCase(oQuote.Frequency) = "W" Then
					nWeekDay = oQuote.SavedDayInMonth
				End If
				'WPR005 - Do not Show Plans if user lands with this querystring request 
				'Call on Instalment Plan Maintenance MTA
				Dim iPFPremFinanceKey As Integer = 0
				Dim iPFPremFinanceVersion As Integer = 0
				If Request.QueryString("FinancePlanKey") IsNot Nothing AndAlso Request.QueryString("FinancePlanKey") <> "" AndAlso Request.QueryString("FinancePlanVersion") IsNot Nothing AndAlso Request.QueryString("FinancePlanVersion") <> "" Then
					iPFPremFinanceKey = Request.QueryString("FinancePlanKey")
					iPFPremFinanceVersion = Request.QueryString("FinancePlanVersion")
				End If

				If Request.QueryString("ShowPlan") IsNot Nothing AndAlso Request.QueryString("ShowPlan") = "False" Then
					'Selection of instalment type will be visible only for MTA
					pnlInstalmentType.Visible = False

					'In Nexus, we does not have input field for selecting weekday and month day
					'By default first value is selected in BO.So passing value 1 for these parameters
					oInstalmentQuotes = oWebService.GetInstalmentQuotes(v_dAmountToFinance:=0,
												v_dtStartDate:=oQuote.CoverStartDate, v_dtEndDate:=oQuote.CoverEndDate, v_dtPreferredDate:=Date.Today,
												v_dtQuoteDate:=DateTime.Now, v_iWeekDay:=nWeekDay, v_iMonthDay:=iMonthDay, v_iInsuranceFileKey:=oQuote.InsuranceFileKey,
												v_dOverrideInterestRate:=dOverrideInterestRate, v_dOverrideRate:=dOverrideRate, v_bPaymentProtection:=bPaymentProtection,
												v_sBranchCode:=oQuote.BranchCode, v_iInstallmentType:=NexusProvider.InstalmentType.NoAmountChange,
												sProcessPFMode:="MTA", v_OverrideDepositAmount:=dOverrideDepositAmount, bOverrideCommission:=bOverrideCommission, iPremiumFinancekey:=iPFPremFinanceKey,
												iPremiumFinanceVersionKey:=iPFPremFinanceVersion, bIsUseTransactionCurrency:=chkUseTransactionCurrency.Checked)

				ElseIf Request.QueryString("ShowPlan") Is Nothing AndAlso Request.QueryString("Type") IsNot Nothing AndAlso Request.QueryString("Type") = "MTA" Then
					If (Session(CNTransactionDetails)) IsNot Nothing Then
						Dim oFinancePlanTransactionCollection As New NexusProvider.FinancePlanTransactionsCollection
						oFinancePlanTransactionCollection = Session(CNTransactionDetails)
						oInstalmentQuotes = oWebService.GetInstalmentQuotes(v_dAmountToFinance:=0,
												v_dtStartDate:=oQuote.CoverStartDate, v_dtEndDate:=oQuote.CoverEndDate, v_dtPreferredDate:=Date.Today,
												 v_dtQuoteDate:=DateTime.Now, v_iWeekDay:=nWeekDay, v_iMonthDay:=iMonthDay, v_iInsuranceFileKey:=oQuote.InsuranceFileKey,
												 v_dOverrideInterestRate:=dOverrideInterestRate, v_dOverrideRate:=dOverrideRate, v_bPaymentProtection:=bPaymentProtection,
												 v_sBranchCode:=oQuote.BranchCode, v_iInstallmentType:=rbInstalmentType.SelectedValue, sProcessPFMode:="MTA",
												 v_OverrideDepositAmount:=dOverrideDepositAmount, bOverrideCommission:=bOverrideCommission,
												 oPFTransactionCollection:=oFinancePlanTransactionCollection, iPremiumFinancekey:=iPFPremFinanceKey,
												iPremiumFinanceVersionKey:=iPFPremFinanceVersion, bIsUseTransactionCurrency:=chkUseTransactionCurrency.Checked)
						If oQuote IsNot Nothing AndAlso IsNumeric(oQuote.OriginalInsuranceFileKey) AndAlso oQuote.OriginalInsuranceFileKey <> 0 AndAlso rbInstalmentType.SelectedValue <> "2" Then
							Dim oFinancePlan As New NexusProvider.FinancePlan
							oFinancePlan = oWebService.GetFinancePlanDetails(oQuote.OriginalInsuranceFileKey)
							If oFinancePlan IsNot Nothing AndAlso String.IsNullOrEmpty(oFinancePlan.DayOfWeekOrMonth) = False AndAlso oFinancePlan.DayOfWeekOrMonth <> 0 Then
								ddlDayinMonth.SelectedValue = oFinancePlan.DayOfWeekOrMonth
							End If
						End If
					End If
				ElseIf Request.QueryString("Type") = "NewPlanSED" Then
					Dim nInsuranceFileKey As Integer = oQuote.InsuranceFileKey
					oFinancePlanInformation = oWebService.GetFinancePlanInformation(oQuote.InsuranceFileKey)
					Session("PFProductCode") = oFinancePlanInformation.ProductCode
					'In Nexus, we does not have input field for selecting weekday and month day
					'By default first value is selected in BO.So passing value 1 for these parameters
					If oFinancePlanInformation.ProductCode.ToUpper() = "MTA" OrElse oFinancePlanInformation.ProductCode.ToUpper() = "REN" Then
						'Selection of instalment type will be visible only for MTA
						pnlInstalmentType.Visible = True
						If oFinancePlanInformation.ProductCode.ToUpper() = "MTA" Then
							nInsuranceFileKey = oFinancePlanInformation.OriginalInsuranceFileKey
						End If
						' Fill previous preferred day at IPM MTA with option Add transaction to current plan and from New Instalment plan for attached SED on current plan
						If oQuote IsNot Nothing AndAlso IsNumeric(oQuote.OriginalInsuranceFileKey) AndAlso oQuote.OriginalInsuranceFileKey <> 0 AndAlso rbInstalmentType.SelectedValue <> "2" Then
							Dim oFinancePlan As New NexusProvider.FinancePlan
							oFinancePlan = oWebService.GetFinancePlanDetails(oQuote.OriginalInsuranceFileKey)
							If oFinancePlan IsNot Nothing AndAlso String.IsNullOrEmpty(oFinancePlan.DayOfWeekOrMonth) = False AndAlso oFinancePlan.DayOfWeekOrMonth <> 0 Then
								ddlDayinMonth.SelectedValue = oFinancePlan.DayOfWeekOrMonth
							End If
						End If
					ElseIf oFinancePlanInformation.ProductCode.ToUpper() = "NB" Then
						rbInstalmentType.SelectedValue = "2"
					End If
					'On load of Plan NB
					Dim oFinancePlanTransactionCollection As New NexusProvider.FinancePlanTransactionsCollection
					If (Session(CNTransactionDetails)) IsNot Nothing Then
						oFinancePlanTransactionCollection = Session(CNTransactionDetails)
					End If

					oInstalmentQuotes = oWebService.GetInstalmentQuotes(v_dAmountToFinance:=dAmountToFinance,
													v_dtStartDate:=oQuote.CoverStartDate, v_dtEndDate:=oQuote.CoverEndDate, v_dtPreferredDate:=Date.Today,
													v_dtQuoteDate:=DateTime.Now, v_iWeekDay:=nWeekDay, v_iMonthDay:=iMonthDay, v_iInsuranceFileKey:=nInsuranceFileKey,
													v_dOverrideInterestRate:=dOverrideInterestRate, v_dOverrideRate:=dOverrideRate, v_bPaymentProtection:=bPaymentProtection,
													v_iInstallmentType:=rbInstalmentType.SelectedValue, v_OverrideDepositAmount:=dOverrideDepositAmount,
													bOverrideCommission:=bOverrideCommission, bIsUseTransactionCurrency:=chkUseTransactionCurrency.Checked,
													sProcessPFMode:=oFinancePlanInformation.ProductCode.ToUpper(), oPFTransactionCollection:=oFinancePlanTransactionCollection)

					' Use this case for View mode , no need to pass the finance amount while calling the GetInstalmentQuotes method.
				ElseIf Not Session(CNMode) Is Nothing AndAlso Session(CNMode) = Mode.View Then
					oInstalmentQuotes = oWebService.GetInstalmentQuotes(v_dAmountToFinance:=0,
												v_dtStartDate:=oQuote.CoverStartDate, v_dtEndDate:=oQuote.CoverEndDate, v_dtPreferredDate:=Date.Today,
												v_dtQuoteDate:=DateTime.Now, v_iWeekDay:=nWeekDay, v_iMonthDay:=1, v_iInsuranceFileKey:=oQuote.InsuranceFileKey, v_dOverrideInterestRate:=dOverrideInterestRate,
												v_dOverrideRate:=dOverrideRate, v_bPaymentProtection:=bPaymentProtection, v_iInstallmentType:=NexusProvider.InstalmentType.NoAmountChange,
												sProcessPFMode:="MTA", v_OverrideDepositAmount:=dOverrideDepositAmount, bOverrideCommission:=bOverrideCommission,
												bIsUseTransactionCurrency:=chkUseTransactionCurrency.Checked)

					Dim oInstalmentQuotesModified As New NexusProvider.InstallmentQuoteCollection
					For Each instalmentQuote As NexusProvider.InstalmentQuote In oInstalmentQuotes
						If instalmentQuote.SchemeName = CType(Session(CNFinancePlan), NexusProvider.FinancePlan).SchemeName Then
							oInstalmentQuotesModified.Add(instalmentQuote)
						End If
					Next
					oInstalmentQuotes = oInstalmentQuotesModified

				Else
					'On selection of instalment plan during NB
					oInstalmentQuotes = oWebService.GetInstalmentQuotes(v_dAmountToFinance:=dAmountToFinance,
										v_dtStartDate:=oQuote.CoverStartDate, v_dtEndDate:=oQuote.CoverEndDate, v_dtPreferredDate:=dtPreferredDate,
										v_dtQuoteDate:=DateTime.Now, v_iWeekDay:=nWeekDay, v_iMonthDay:=iMonthDay, v_iInsuranceFileKey:=oQuote.InsuranceFileKey,
										v_dOverrideInterestRate:=dOverrideInterestRate, v_dOverrideRate:=dOverrideRate, v_bPaymentProtection:=bPaymentProtection,
										v_sBranchCode:=oQuote.BranchCode, v_iInstallmentType:=rbInstalmentType.SelectedValue, bOverrideCommission:=bOverrideCommission,
										v_OverrideDepositAmount:=dOverrideDepositAmount, bIsUseTransactionCurrency:=chkUseTransactionCurrency.Checked,
										iPremiumFinancekey:=oQuote.DefaultInstalmentPlan, iPremiumFinanceVersionKey:=oQuote.DefaultInstalmentPlanVersion)
				End If

				If (oInstalmentQuotes IsNot Nothing AndAlso oInstalmentQuotes.Count > 0) Then
					If oInstalmentQuotes(0).UseTransCurrncy = 1 AndAlso oInstalmentQuotes(0).ProductCode = "MTA" AndAlso chkUseTransactionCurrency.Checked Then
						chkUseTransactionCurrency.Checked = True
						chkUseTransactionCurrency.Enabled = True
					End If

					If oInstalmentQuotes(0).ProductCode = "MTA" AndAlso rbInstalmentType.SelectedValue <> "2" Then
						If oInstalmentQuotes(0).UseTransCurrncy = 1 Then
							chkUseTransactionCurrency.Checked = True
						Else
							chkUseTransactionCurrency.Checked = False
						End If

					End If
				End If
				For nInstalmentQuoteIndex As Integer = 0 To oInstalmentQuotes.Count - 1
					If nInstalmentQuoteIndex >= oInstalmentQuotes.Count Then
						Exit For
					End If
					If oInstalmentQuotes(nInstalmentQuoteIndex).MediaTypeDescription = "Credit Card" AndAlso ViewState("PaymentHubEnabled") = "1" AndAlso ViewState("DoNotUseForSubsequentPayment") = "1" Then
						oInstalmentQuotes.RemoveAt(nInstalmentQuoteIndex)
						nInstalmentQuoteIndex = nInstalmentQuoteIndex - 1
					End If
					If oInstalmentQuotes.Count = 0 Then
						Exit For
					End If
				Next
				If ViewState("PaymentHubEnabled") = "1" AndAlso ViewState("DoNotUseForSubsequentPayment") = "1" Then
					oInstalmentQuotes.SortColumn = "SchemeName"
					oInstalmentQuotes.SortingOrder = NexusProvider.GenericComparer.SortOrder.Ascending
					oInstalmentQuotes.Sort()
					grdInstallmentQuotes.DataSource = oInstalmentQuotes
					grdInstallmentQuotes.DataBind()
				End If
				'Remove previous cache
				Cache.Remove(ViewState("InstalmentQuotesCacheID"))

				'Add the retrived quotes in cache.So that they can be used throughout the page
				Cache.Insert(ViewState("InstalmentQuotesCacheID"), oInstalmentQuotes, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

				pnlPlanSummary.Visible = True
			Catch ex As NexusProvider.NexusException
				If ex.Errors(0).Code = "0" OrElse ex.Errors(0).Code = 272 Then
					pnlPlanSummary.Visible = False
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
		Private Sub PopulateAccountType(Optional ByVal nPartyBankId As Integer = 0)
			Dim sSelectDefaultText As String = GetLocalResourceObject("lbl_Select_DefaultText")
			'Create PartyBankDetail object from cache
			If Cache.Item(ViewState("PartyBankdetailsCacheID")) Is Nothing Then
				GetPartyBankDetails()
			End If
			Dim oPartyBankDetail As NexusProvider.BankCollection = Cache.Item(ViewState("PartyBankdetailsCacheID"))

			Dim oTempBankDetailsCollection As New NexusProvider.BankCollection

			ddlAccountType.Items.Clear()
			ddlAccountType.SelectedValue = Nothing
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
				'WPR005 - Issue Raised by client - SSP ref - SSP441 - TFS Item -9019
			ElseIf Session(CNRenewal) IsNot Nothing AndAlso Session(CNFinancePlan) IsNot Nothing Then
				Dim oFinancePlan As New NexusProvider.FinancePlan
				oFinancePlan = CType(Session(CNFinancePlan), NexusProvider.FinancePlan)
				If oFinancePlan IsNot Nothing Then
					'Set selected item in dropdown
					If ddlAccountType.Items.FindByValue(oFinancePlan.PartyBankKey.ToString()) IsNot Nothing Then
						ddlAccountType.Items.FindByValue(oFinancePlan.PartyBankKey.ToString()).Selected = True
					End If
					'Show party bank details for selected account type
					ddlAccountType_SelectedIndexChanged(Me, Nothing)
				End If
			ElseIf oPartyBankDetail.Count > 0 AndAlso Session(CNFinancePlanDetails) IsNot Nothing Then
				Dim oFinancePlanDetails As New NexusProvider.FinancePlanDetails
				oFinancePlanDetails = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).PremiumFinanceDetails
				If oFinancePlanDetails IsNot Nothing Then
					'Set selected item in dropdown
					If ddlAccountType.Items.FindByValue(oFinancePlanDetails.PartyBankKey.ToString()) IsNot Nothing Then
						ddlAccountType.Items.FindByValue(oFinancePlanDetails.PartyBankKey.ToString()).Selected = False
					Else
						If nPartyBankId <> 0 Then
							ddlAccountType.Items.FindByValue(nPartyBankId).Selected = True
						End If
					End If
					'Show party bank details for selected account type
					ddlAccountType_SelectedIndexChanged(Me, Nothing)
				End If
			ElseIf oPartyBankDetail.Count > 0 AndAlso Session(CNFinancePlan) IsNot Nothing Then
				Dim oFinancePlan As New NexusProvider.FinancePlan
				oFinancePlan = CType(Session(CNFinancePlan), NexusProvider.FinancePlan)
				If oFinancePlan.BankDetails IsNot Nothing Then
					'Set selected item in dropdown
					If ddlAccountType.Items.FindByValue(oFinancePlan.BankDetails.BankKey.ToString()) IsNot Nothing Then
						ddlAccountType.Items.FindByValue(oFinancePlan.BankDetails.BankKey.ToString()).Selected = True
					Else
						If nPartyBankId <> 0 Then
							ddlAccountType.Items.FindByValue(nPartyBankId).Selected = True
						End If
					End If
					'Show party bank details for selected account type
					ddlAccountType_SelectedIndexChanged(Me, Nothing)
				End If
			Else
				If nPartyBankId <> 0 Then
					ddlAccountType.SelectedValue = nPartyBankId
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
					txtBIC.Text = String.Empty
					txtIBAN.Text = String.Empty
				End If



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
			If Cache.Item(ViewState("PartyBankdetailsCacheID")) Is Nothing Then
				GetPartyBankDetails()
			End If
			Dim oPartyBankDetails As NexusProvider.BankCollection = Cache.Item(ViewState("PartyBankdetailsCacheID"))
			Dim oPartyBankDetail As NexusProvider.Bank
			Dim bMaskBankAccountNumber As Boolean =
					CType(GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID().ToString()).MaskBankAccountNumber
			Dim sFirstStr As String = String.Empty
			Dim sLastStr As String = String.Empty

			If ddlAccountType.SelectedValue <> "" Then
				For Each oPartyBankDetail In oPartyBankDetails
					If oPartyBankDetail.PartyBankKey = ddlAccountType.SelectedValue Then
						txtBankName.Text = oPartyBankDetail.BankName
						txtAddress1.Text = oPartyBankDetail.PartyBankAddress.Address1
						txtBranch.Text = oPartyBankDetail.BankBranch
						txtBranchCode.Text = oPartyBankDetail.BranchCode
						txtAccountName.Text = oPartyBankDetail.AccountHolderName
						txtAccountNumber.Text = oPartyBankDetail.AccountNumber
						txtBIC.Text = oPartyBankDetail.BIC
						txtIBAN.Text = oPartyBankDetail.IBAN


						'Putting the Mask
						If bMaskBankAccountNumber And txtAccountNumber.Text.Length > 4 Then
							sFirstStr = Mid(txtAccountNumber.Text, 1, txtAccountNumber.Text.Length - 4)
							sLastStr = Mid(txtAccountNumber.Text, sFirstStr.Length + 1)
							For icount As Integer = 0 To sFirstStr.Length - 1
								sFirstStr = sFirstStr.Replace(sFirstStr.Chars(icount), "*")
							Next
							txtAccountNumber.Text = sFirstStr & sLastStr
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
					End If
				Next
				oParty = Nothing
			Else
				'Clear and enable all party bank related controls
				hypBankEdit.Visible = False
				txtBankName.Text = String.Empty
				txtAddress1.Text = String.Empty
				txtBranch.Text = String.Empty
				txtBranchCode.Text = String.Empty
				txtAccountName.Text = String.Empty
				txtAccountNumber.Text = String.Empty
				txtBIC.Text = String.Empty
				txtIBAN.Text = String.Empty
			End If

		End Sub

		''' <summary>
		''' Get party bank details
		''' </summary>
		''' <remarks></remarks>
		Private Sub GetPartyBankDetails()
			Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
			Dim oPartyBankDetails As NexusProvider.BankCollection
			Dim oPartyBankDetail As NexusProvider.Bank
			Dim oPartyBankDetailsForInstalment As New NexusProvider.BankCollection
			Dim oQuote As NexusProvider.Quote = Session(CNQuote)
			Dim oParty As NexusProvider.BaseParty = CType(Session(CNParty), NexusProvider.BaseParty)
			'Find all the account types for party/agent using sam method "GetPartyBankDetails"
			'Filter all the accounts for instalment payment method and save the details in cache
			'So that party bank details can be used througout the page

			oPartyBankDetails = oWebService.GetPartyBankDetails(oQuote.PartyKey)


			'Populate Party bank Details
			oParty.BankDetails = oPartyBankDetails
			Session(CNParty) = oParty

			For Each oPartyBankDetail In oPartyBankDetails
				If (oPartyBankDetail.BankPaymentTypeCode.ToUpper() = "INS" Or oPartyBankDetail.BankPaymentTypeCode.ToUpper() = "ANY") AndAlso
					oPartyBankDetail.AccountType.Trim() <> "" AndAlso oPartyBankDetail.IsDeleted = False Then
					oPartyBankDetailsForInstalment.Add(oPartyBankDetail)
				End If
			Next

			'Add filtered party bank details to cache
			Cache.Insert(ViewState("PartyBankdetailsCacheID"), oPartyBankDetailsForInstalment, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
		End Sub

		Protected Sub grdInstallmentQuotes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdInstallmentQuotes.Load
			If grdInstallmentQuotes.PageCount = 1 Then
				grdInstallmentQuotes.AllowPaging = False
			End If
		End Sub

		Protected Sub grdInstallmentQuotes_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdInstallmentQuotes.PageIndexChanging
			If Cache.Item(ViewState("InstalmentQuotesCacheID")) Is Nothing Then
				CallGetInstalmentQuotes()
			End If
			Dim oInstalmentQuotes As NexusProvider.InstallmentQuoteCollection = Cache.Item(ViewState("InstalmentQuotesCacheID"))
			grdInstallmentQuotes.DataSource = oInstalmentQuotes
			grdInstallmentQuotes.PageIndex = e.NewPageIndex
			'grdInstallmentQuotes.SelectedIndex = 0 ''select first scheme on page change
			grdInstallmentQuotes.DataBind()
		End Sub

		Protected Sub grdInstallmentQuotes_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdInstallmentQuotes.RowCommand
			hdnMediaType.Value = e.CommandArgument.ToString()
		End Sub

		Protected Sub grdInstallmentQuotes_SelectedIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSelectEventArgs) Handles grdInstallmentQuotes.SelectedIndexChanging
			bHasPlanSelectionChanged = True

			Dim oInstalmentQuotes As NexusProvider.InstallmentQuoteCollection
			Dim iCount As Integer = 0
			'Find key values for selected instalment quote
			Dim iSelectedSchemeNumber As Integer = grdInstallmentQuotes.DataKeys(e.NewSelectedIndex).Values("SchemeNo")
			Dim iSelectedSchemeVersion As Integer = grdInstallmentQuotes.DataKeys(e.NewSelectedIndex).Values("SchemeVersion")
			Dim iSelectedCompanyNumber As Integer = grdInstallmentQuotes.DataKeys(e.NewSelectedIndex).Values("CompanyNo")
			Dim iSelectedFrequencyId As Integer = grdInstallmentQuotes.DataKeys(e.NewSelectedIndex).Values("FrequencyID")

			'Changes for WPR005, maitaining selected grid values
			hvSchemeNo.Value = iSelectedSchemeNumber
			hvSchemeVersion.Value = iSelectedSchemeVersion
			hvCompanyNo.Value = iSelectedCompanyNumber
			hvActualIndex.Value = CInt(CInt(grdInstallmentQuotes.PageIndex * grdInstallmentQuotes.PageSize) + e.NewSelectedIndex)
			hvFrequencyId.Value = iSelectedFrequencyId
			txtFirstInstalmentDate.Text = ""
			If sender IsNot Nothing Then ''to be set when scheme selected in gridview
				'Show instalment details for selected instalment quote
				If ddlDayinMonth.SelectedIndex = -1 Then
					ddlDayinMonth.SelectedIndex = 0
				End If
			End If
			Dim lnkButtonSave As System.Web.UI.WebControls.LinkButton
			lnkButtonSave = Me.Parent.FindControl("btnSave")
			If lnkButtonSave IsNot Nothing Then
				lnkButtonSave.Attributes.Add("onclick", "javascript:return true;")
			End If

			pnlCreditCard.Visible = False
			pnlCreditCardDetails.Visible = False
			''reset firstpayment date on selecting new instalment scheme
			If ddlFirstPaymentDate.Items.Count > 0 AndAlso pnlInstalmentType.Visible Then
				ddlFirstPaymentDate.SelectedIndex = 0
			End If

			If Cache.Item(ViewState("InstalmentQuotesCacheID")) Is Nothing Then
				CallGetInstalmentQuotes()
			End If

			If Cache.Item(ViewState("InstalmentQuotesCacheID")) IsNot Nothing Then
				oInstalmentQuotes = Cache.Item(ViewState("InstalmentQuotesCacheID"))
				For iCount = 0 To oInstalmentQuotes.Count - 1
					If iSelectedSchemeNumber = oInstalmentQuotes(iCount).SchemeNo AndAlso iSelectedSchemeVersion = oInstalmentQuotes(iCount).SchemeVersion _
						AndAlso iSelectedCompanyNumber = oInstalmentQuotes(iCount).CompanyNo AndAlso iSelectedFrequencyId = oInstalmentQuotes(iCount).FrequencyID Then
						hvActualIndex.Value = iCount
						Exit For
					End If
				Next
			End If

			PopulateInstalmentDates(hvActualIndex.Value)
			CallGetInstalmentQuotes()
			ShowDetailsForScheme(iSelectedSchemeNumber, iSelectedSchemeVersion, iSelectedCompanyNumber, iSelectedFrequencyId)
			grdInstallmentQuotes.Rows(e.NewSelectedIndex).Cells(0).Style.Add(HtmlTextWriterStyle.FontWeight, "Bold")
			For Each row As GridViewRow In grdInstallmentQuotes.Rows
				If (e.NewSelectedIndex).ToString <> row.RowIndex.ToString Then
					row.Cells(0).Style.Add(HtmlTextWriterStyle.FontWeight, "Regular")
				End If
			Next
			If HttpContext.Current.Session(CNRenewal) IsNot Nothing AndAlso SelectedInstalmentQuote IsNot Nothing Then
				Session(CNInstalmentDatesUpdated) = "1"
				SaveInstallmentPlan()
			End If

		End Sub

		Protected Sub grdInstallmentQuotes_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdInstallmentQuotes.Sorting
			'sort the instalment quotes according to the column clicked
			'we need to store the current sort order in viewstate, and reverse it each time
			If Cache.Item(ViewState("InstalmentQuotesCacheID")) Is Nothing Then
				CallGetInstalmentQuotes()
			End If
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
				grdInstallmentQuotes.SelectedIndex = 0
				'Show instalment details for selected instalment quote
				ShowDetailsForScheme(oInstalmentQuotes(0).SchemeNo, oInstalmentQuotes(0).SchemeVersion, oInstalmentQuotes(0).CompanyNo, oInstalmentQuotes(0).FrequencyID)
				pnlPlanSummary.Visible = True
			Else
				grdInstallmentQuotes.DataSource = Nothing
				grdInstallmentQuotes.DataBind()
			End If

			If rbInstalmentType.SelectedValue = 0 AndAlso grdInstallmentQuotes.Rows.Count > 0 Then
				Dim eGridViewSelectEvent As System.Web.UI.WebControls.GridViewSelectEventArgs = Nothing
				grdInstallmentQuotes.Rows(0).RowState = DataControlRowState.Selected
				grdInstallmentQuotes.SelectedIndex = 0
				eGridViewSelectEvent = New System.Web.UI.WebControls.GridViewSelectEventArgs(0)
				grdInstallmentQuotes_SelectedIndexChanging(Nothing, eGridViewSelectEvent)
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
			'PopulateInstalmentDates()
			CallGetInstalmentQuotes()
			Dim nSelectedItem As Integer = -1
			Dim oInstalmentQuotes As NexusProvider.InstallmentQuoteCollection = Cache.Item(ViewState("InstalmentQuotesCacheID"))
			For Each oInstalmentQuote In oInstalmentQuotes
				nSelectedItem = nSelectedItem + 1
				If oInstalmentQuote.SchemeNo = SelectedInstalmentQuote.SchemeNo Then
					Exit For
				End If
			Next
			If Request.QueryString("Type") IsNot Nothing AndAlso hvSchemeNo.Value <> "" AndAlso hvSchemeVersion.Value <> "" AndAlso hvCompanyNo.Value <> "" AndAlso hvFrequencyId.Value <> "" Then 'Changes for WPR005, sending selected grid values, not impacting the exiting code
				ShowDetailsForScheme(Convert.ToInt32(hvSchemeNo.Value), Convert.ToInt32(hvSchemeVersion.Value), Convert.ToInt32(hvCompanyNo.Value), Convert.ToInt32(hvFrequencyId.Value))
			ElseIf grdInstallmentQuotes.SelectedIndex = -1 Then
				ShowDetailsForScheme(oInstalmentQuotes(0).SchemeNo, oInstalmentQuotes(0).SchemeVersion, oInstalmentQuotes(0).CompanyNo, oInstalmentQuotes(0).FrequencyID)

			Else
				ShowDetailsForScheme(oInstalmentQuotes(nSelectedItem).SchemeNo, oInstalmentQuotes(nSelectedItem).SchemeVersion, oInstalmentQuotes(grdInstallmentQuotes.SelectedIndex).CompanyNo, oInstalmentQuotes(grdInstallmentQuotes.SelectedIndex).FrequencyID)
			End If
			If oInstalmentQuotes(nSelectedItem).FirstInstalmentAlignWithDayInMonth = "1" Then
				Dim newAlignedDate As Date = Convert.ToDateTime(ddlFirstPaymentDate.SelectedValue)
				newAlignedDate = New Date(newAlignedDate.Year, newAlignedDate.Month, ddlDayinMonth.SelectedValue)
				ddlFirstPaymentDate.Items.Clear()
				ddlFirstPaymentDate.Items.Insert(0, New ListItem(newAlignedDate, newAlignedDate))
				ddlFirstPaymentDate.SelectedIndex = 0
				txtFirstInstalmentDate.Text = newAlignedDate
			End If
			If oInstalmentQuotes(nSelectedItem).SchemeTypeCode.Trim.ToUpper = "TP" Then
				txtFirstInstalmentDate.Text = String.Empty
			End If
			If HttpContext.Current.Session(CNRenewal) IsNot Nothing Then
				Session(CNInstalmentDatesUpdated) = "1"
				SaveInstallmentPlan()
			End If
		End Sub

		Protected Sub ddlFirstPaymentDate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFirstPaymentDate.SelectedIndexChanged
			CallGetInstalmentQuotes()
			Dim oInstalmentQuotes As NexusProvider.InstallmentQuoteCollection = Cache.Item(ViewState("InstalmentQuotesCacheID"))
			Dim selectedGridItemIndex As Integer = grdInstallmentQuotes.SelectedIndex + (grdInstallmentQuotes.PageIndex * grdInstallmentQuotes.PageSize)
			If Request.QueryString("Type") IsNot Nothing AndAlso hvSchemeNo.Value <> "" AndAlso hvSchemeVersion.Value <> "" AndAlso hvCompanyNo.Value <> "" AndAlso hvFrequencyId.Value <> "" Then 'Changes for WPR005, sending selected grid values, not impacting the exiting code
				ShowDetailsForScheme(Convert.ToInt32(hvSchemeNo.Value), Convert.ToInt32(hvSchemeVersion.Value), Convert.ToInt32(hvCompanyNo.Value), Convert.ToInt32(hvFrequencyId.Value))
			ElseIf grdInstallmentQuotes.SelectedIndex = -1 Then
				ShowDetailsForScheme(oInstalmentQuotes(0).SchemeNo, oInstalmentQuotes(0).SchemeVersion, oInstalmentQuotes(0).CompanyNo)
			Else
				ShowDetailsForScheme(oInstalmentQuotes(selectedGridItemIndex).SchemeNo, oInstalmentQuotes(selectedGridItemIndex).SchemeVersion, oInstalmentQuotes(selectedGridItemIndex).CompanyNo, oInstalmentQuotes(selectedGridItemIndex).FrequencyID)
			End If
			'Removing the Default date from the Dropdown down List once user has tried to change the date.
			If Not ddlFirstPaymentDate Is Nothing AndAlso ddlFirstPaymentDate.Items(0).Value = hdnNextInstalmentDueDate.Value Then
				ddlFirstPaymentDate.Items.RemoveAt(0)
			End If
			If (grdInstallmentQuotes.SelectedIndex = -1 AndAlso oInstalmentQuotes(0).SchemeTypeCode.Trim.ToUpper = "TP") OrElse
				oInstalmentQuotes(selectedGridItemIndex).SchemeTypeCode.Trim.ToUpper = "TP" Then
				txtFirstInstalmentDate.Text = String.Empty
			Else
				txtFirstInstalmentDate.Text = ddlFirstPaymentDate.SelectedValue
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
			Dim oQuote As NexusProvider.Quote
			Try
				'Get the system Option "Enable Editing in Client Manager"
				sReturnCode = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5000)

				'Get the system Option "Payment Type can only be edited on Party"
				sReturnCodePaymentType = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5062)

				'Checking of User Authority for Editing the Client using client manager
				Dim oUserAuthorityIsClientManagerViewonly As New NexusProvider.UserAuthority


				'Get the user name from session
				oUserAuthorityIsClientManagerViewonly.UserCode = Session(CNLoginName)
				'set the authority options for reverse allocation
				oUserAuthorityIsClientManagerViewonly.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.IsClientManagerViewonly
				oWebService = New NexusProvider.ProviderManager().Provider
				'initiate the GetUserAuthority method
				oWebService.GetUserAuthorityValue(oUserAuthorityIsClientManagerViewonly)
				If oUserAuthorityIsClientManagerViewonly.UserAuthorityValue = "1" Then
					bIsClientManagerViewOnly = True
				Else
					bIsClientManagerViewOnly = False
				End If

				'wpr10
				Dim oUserAuthorityCanChangeInstalmentDefaultCurrency As New NexusProvider.UserAuthority
				oUserAuthorityCanChangeInstalmentDefaultCurrency.UserCode = Session(CNLoginName)
				oUserAuthorityCanChangeInstalmentDefaultCurrency.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.CanChangeInstalmentDefaultCurrency
				oWebService.GetUserAuthorityValue(oUserAuthorityCanChangeInstalmentDefaultCurrency)

				'Create oQuote object from session
				oQuote = Session(CNQuote)
				If oQuote IsNot Nothing AndAlso oQuote.GrossTotal < 0 Then
					rbInstalmentType.Items(1).Enabled = True
					rbInstalmentType.Items(2).Enabled = True
				End If
				Session(CNPFUserAuthorityValue) = oUserAuthorityCanChangeInstalmentDefaultCurrency.UserAuthorityValue

				If Not String.IsNullOrEmpty(oUserAuthorityCanChangeInstalmentDefaultCurrency.UserAuthorityValue) Then
					Session(CNPFUserAuthorityValue) = oUserAuthorityCanChangeInstalmentDefaultCurrency.UserAuthorityValue
				Else
					Session(CNPFUserAuthorityValue) = "0"
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

			'Make a Call to Get the Next instalment due date as for defaulting purpose
			Dim iPFPremFinanceKey As Integer = 0
			Dim iPFPremFinanceVersion As Integer = 0
			Dim oInstalmentQuotes As NexusProvider.InstallmentQuoteCollection

			'Create oQuote object from session
			oQuote = Session(CNQuote)
			If Request.QueryString("FinancePlanKey") IsNot Nothing AndAlso Request.QueryString("FinancePlanKey") <> "" _
																   AndAlso Request.QueryString("FinancePlanVersion") IsNot Nothing _
																   AndAlso Request.QueryString("FinancePlanVersion") <> "" Then
				iPFPremFinanceKey = Request.QueryString("FinancePlanKey")
				iPFPremFinanceVersion = Request.QueryString("FinancePlanVersion")

				oInstalmentQuotes = oWebService.GetInstalmentQuotes(v_dAmountToFinance:=0,
											  v_dtStartDate:=oQuote.CoverStartDate, v_dtEndDate:=oQuote.CoverEndDate, v_dtPreferredDate:=Date.Today,
											   v_dtQuoteDate:=DateTime.Now, v_iWeekDay:=1, v_iMonthDay:=1, v_iInsuranceFileKey:=oQuote.InsuranceFileKey,
											   v_dOverrideInterestRate:=dOverrideInterestRate, v_dOverrideRate:=dOverrideRate, v_bPaymentProtection:=bPaymentProtection,
											   v_sBranchCode:=oQuote.BranchCode, v_iInstallmentType:=NexusProvider.InstalmentType.NoAmountChange, sProcessPFMode:="MTA",
											   v_OverrideDepositAmount:=dOverrideDepositAmount, bOverrideCommission:=bOverrideCommission,
											   iPremiumFinancekey:=iPFPremFinanceKey, bPreferredInstalmentDueDateonly:=True,
											   iPremiumFinanceVersionKey:=iPFPremFinanceVersion)
				'Assign the Value and it will used while populationg the First instalment combo.
				hdnNextInstalmentDueDate.Value = oInstalmentQuotes.Item(0).NextInstalmentDueDate

				'This condition has appllied to create the installment date after the current system date instead of from coverstart date
				'when System date  > coverstart date
			ElseIf UCase(Request.QueryString("Type")).ToString.StartsWith("NEWPLAN") Then
				If CDate(oQuote.CoverStartDate.ToString("d")) < CDate(DateTime.Now.ToString("d")) Then
					' start date is in the past so start date calculations from today
					hdnNextInstalmentDueDate.Value = CDate(DateTime.Now.ToString("d"))
				Else
					hdnNextInstalmentDueDate.Value = CDate(oQuote.CoverStartDate.ToString("d"))
				End If

			End If

			If Session(CNHasPremiumUpdated) = True Then
				bindInstalments()
				Session(CNHasPremiumUpdated) = Nothing
			End If
			If Session(CNMode) IsNot Nothing And Session(CNMode) = Mode.View Then
				ddlDayinMonth.Enabled = False
				ddlFirstPaymentDate.Enabled = False
				hypBank.Enabled = False
				hypBankEdit.Enabled = False
				ddlAccountType.Enabled = False
			End If


			If ddlExistingTokens.SelectedIndex > 0 Then
				RqdTokenNo.Enabled = False
				txtTokenNo.Text = String.Empty
				txtTokenNo.Enabled = False
			Else
				RqdTokenNo.Enabled = True
				txtTokenNo.Enabled = True
			End If
		End Sub

		Private Sub CheckUseTransCurrency(ByVal iBaseCurrencyID As Integer, ByVal iTransCurrencyID As Integer, Optional ByVal iTranCurrency As Integer = 0)

			If iBaseCurrencyID <> iTransCurrencyID AndAlso Session(CNPFUserAuthorityValue) = 1 Then
				chkUseTransactionCurrency.Checked = True
			Else
				chkUseTransactionCurrency.Checked = False
			End If

			If iBaseCurrencyID <> iTransCurrencyID AndAlso Session(CNPFUserAuthorityValue) = 1 Then
				chkUseTransactionCurrency.Enabled = True
			Else
				chkUseTransactionCurrency.Enabled = False
			End If

		End Sub

		Protected Sub SetMandatory()
			Dim oInstalmentQuotes As NexusProvider.InstallmentQuoteCollection = Cache.Item(ViewState("InstalmentQuotesCacheID"))
			Dim oPartyBankDetail As NexusProvider.BankCollection = Cache.Item(ViewState("PartyBankdetailsCacheID"))
			Dim iSelectedPlan As Integer

			If oInstalmentQuotes IsNot Nothing AndAlso oInstalmentQuotes.Count > 0 Then
				If grdInstallmentQuotes.Rows.Count = 1 Then
					iSelectedPlan = 0
				Else
					If grdInstallmentQuotes.SelectedIndex = -1 Then
						iSelectedPlan = 0
					Else
						iSelectedPlan = grdInstallmentQuotes.SelectedIndex
					End If
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

		''' <summary>
		''' To retun the selected instalment plan
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property SelectedInstalmentQuote() As NexusProvider.InstalmentQuote
			Get
				Dim oSelectedInstalmentQuote As NexusProvider.InstalmentQuote = Nothing
				If ViewState("SelectedInstalmentQuoteCacheId") IsNot Nothing AndAlso Cache.Item(ViewState("SelectedInstalmentQuoteCacheId")) IsNot Nothing Then
					oSelectedInstalmentQuote = Cache.Item(ViewState("SelectedInstalmentQuoteCacheId"))
				End If
				Return oSelectedInstalmentQuote
			End Get
		End Property

		''' <summary>
		''' To return the selected bank details
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property SelectedAccountType() As NexusProvider.Bank
			Get
				Dim oSelectedPartyBankDetail As NexusProvider.Bank = Nothing
				If ViewState("SelectedAccountTypeCacheId") IsNot Nothing AndAlso Cache.Item(ViewState("SelectedAccountTypeCacheId")) IsNot Nothing Then
					oSelectedPartyBankDetail = Cache.Item(ViewState("SelectedAccountTypeCacheId"))
				End If
				Return oSelectedPartyBankDetail
			End Get
		End Property

		''' <summary>
		''' THIS SECTION IS FOR UPDATING THE INCEPTION DATE FROM THE TXTINCEPTIONDATE
		''' SIMILARLY ALSO UPDATING THE BANK DETAILS AND THEN UPDATING THE CNPAYMENT AND CNQUOTE
		''' </summary>
		''' <remarks></remarks>
		Public Sub SaveInstallmentPlan(Optional ByVal bPutOnNextInstalmentRenewal As Boolean = False)
			Dim oQuote As NexusProvider.Quote = Session(CNQuote)
			Dim oSelectedPartyBankDetail As NexusProvider.Bank = Cache.Item(ViewState("SelectedAccountTypeCacheId"))
			Dim oSelectedInstalmentQuote As NexusProvider.InstalmentQuote
			Dim oPayment As NexusProvider.Payment = Nothing
			oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.None, CDec(Session(CNAmountToPay)))
			'This is the case when user clicks on Buy button without selecting any scheme.
			If ViewState("SelectedInstalmentQuoteCacheId") IsNot Nothing Then
				oSelectedInstalmentQuote = Cache.Item(ViewState("SelectedInstalmentQuoteCacheId"))
				If oSelectedInstalmentQuote IsNot Nothing Then
					Session(CNSelectedSchemeNo) = oSelectedInstalmentQuote.SchemeNo
					oPayment.AmountToFinance = oSelectedInstalmentQuote.TotalAmountInput
					oPayment.SelectedSchemeNo = oSelectedInstalmentQuote.SchemeNo
					oPayment.SelectedSchemeVersion = oSelectedInstalmentQuote.SchemeVersion
					oPayment.Pref_ID = oSelectedInstalmentQuote.PFRF_ID
					oPayment.DaysDelay = oSelectedInstalmentQuote.DaysDelay

					If oSelectedInstalmentQuote.FirstInstalmentAlignWithDayInMonth = 1 Then
						oPayment.MonthDay = ddlDayinMonth.SelectedValue
					Else
						If oSelectedInstalmentQuote.AlignTo = 1 Then
							oPayment.MonthDay = ddlDayinMonth.SelectedValue
						Else
							oPayment.MonthDay = 1
						End If
					End If
					If (Session(CNMTAType) IsNot Nothing OrElse Session(CNInstalmentPlanMode) = InstalmentPlanType.edit) AndAlso Request.QueryString("Type") IsNot Nothing AndAlso Request.QueryString("Type") = "MTA" OrElse Request.QueryString("Type") = "NewPlanSED" Then
						oPayment.InstallmentType = rbInstalmentType.SelectedValue
						oPayment.InstallmentTypeSpecified = True
					ElseIf Session(CNMTAType) IsNot Nothing Then
						oPayment.InstallmentType = rbInstalmentType.SelectedValue
						oPayment.InstallmentTypeSpecified = True
					Else
						oPayment.InstallmentTypeSpecified = False
					End If

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
							oPayment.BIC = txtBIC.Text
							oPayment.IBAN = txtIBAN.Text
						ElseIf Not oSelectedPartyBankDetail Is Nothing Then
							oPayment.PartyBankKey = oSelectedPartyBankDetail.PartyBankKey
							oPayment.BankAccountName = oSelectedPartyBankDetail.AccountHolderName
							oPayment.BankAccountNo = oSelectedPartyBankDetail.AccountNumber
							oPayment.BankAddress = oSelectedPartyBankDetail.PartyBankAddress
							oPayment.BankBranch = oSelectedPartyBankDetail.BankBranch
							oPayment.BankName = oSelectedPartyBankDetail.BankName
							oPayment.BankSortCode = oSelectedPartyBankDetail.BranchCode
							oPayment.BIC = oSelectedPartyBankDetail.BIC
							oPayment.IBAN = oSelectedPartyBankDetail.IBAN
						End If
						'WPR005
					ElseIf oSelectedInstalmentQuote.MediaTypeDescription = "Credit Card" AndAlso (Request.QueryString("Type") IsNot Nothing Or HttpContext.Current.Session(CNRenewal) IsNot Nothing) Then
						oPayment.CreditCard = New NexusProvider.CreditCardType
						Dim oCreditCard As New NexusProvider.CreditCardType
						If Not String.IsNullOrEmpty(ViewState("PaymentHubEnabled")) AndAlso ViewState("PaymentHubEnabled") Then
							Dim grd_Card As System.Web.UI.WebControls.GridView
							grd_Card = grdCard

							For CardIndex As Integer = 0 To grd_Card.Rows.Count - 1
								If CType(grd_Card.Rows(CardIndex).FindControl("rdDefaultCard"), System.Web.UI.WebControls.RadioButton).Checked Then

									oCreditCard.Number = grd_Card.Rows(CardIndex).Cells(0).Text
									oCreditCard.AuthCode = CType(grd_Card.Rows(CardIndex).FindControl("hdnAuthCode"), System.Web.UI.WebControls.HiddenField).Value
									oCreditCard.ExpiryDate = grd_Card.Rows(CardIndex).Cells(1).Text

									oCreditCard.PartyBankKey = grd_Card.DataKeys(CardIndex).Value
									oCreditCard.NameOnCreditCard = grd_Card.Rows(CardIndex).Cells(2).Text
									oCreditCard.TrackingNumber = CType(grd_Card.Rows(CardIndex).FindControl("hdnTokenNo"), System.Web.UI.WebControls.HiddenField).Value
								End If
							Next
							oPayment.CreditCard = oCreditCard
						End If
						If ddlExistingTokens.SelectedIndex > 0 And (txtTokenNo.Text = "" Or txtTokenNo.Text = ddlExistingTokens.SelectedItem.Text) Then
							oPayment.CreditCard.AuthCode = ddlExistingTokens.SelectedItem.ToString().Trim()
							oPayment.CreditCard.PartyBankKey = CInt(ddlExistingTokens.SelectedValue)
						Else
							If Not String.IsNullOrEmpty(ViewState("PaymentHubEnabled")) AndAlso ViewState("PaymentHubEnabled") Then
								oCreditCard.VIAPaymentHub = True
								oPayment.CreditCard = oCreditCard
							Else
								oPayment.CreditCard.AuthCode = txtTokenNo.Text
								oPayment.CreditCard.PartyBankKey = Nothing
							End If
							If Session(CNFinancePlanDetails) IsNot Nothing Then
								Dim oProcessPfPlan As New NexusProvider.PremiumFinancePlan
								oProcessPfPlan = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan)
								oProcessPfPlan.PremiumFinanceDetails.PartyBankKey = Nothing
								Session(CNFinancePlanDetails) = oProcessPfPlan
							End If
						End If
					End If
					If rbInstalmentType.SelectedIndex = "1" AndAlso bPutOnNextInstalmentRenewal = True Then
						oPayment.PayTrueMonthlyPolicyMTAPremiumOnRenewal = True
					End If
					Session(CNInstalmentMediaType) = oSelectedInstalmentQuote.MediaTypeDescription
					oPayment.PreferredDate = CType(ddlFirstPaymentDate.SelectedValue, Date)
					oPayment.InstDepositAmount = oSelectedInstalmentQuote.DepositAmount
					oQuote.DepositTransactasInstalment = oSelectedInstalmentQuote.DepositAsInstalment
					oQuote.InstDepositAmount = oSelectedInstalmentQuote.DepositAmount
				End If

			End If


			Session(CNPartyBankDetail) = oSelectedPartyBankDetail


			'Set all required properties for payment object


			If Not chkOverrideInterest.Checked Then
				oPayment.OverrideInterestRate = -1
			Else
				oPayment.OverrideInterestRate = IIf(txtNewRate.Text.Trim = "", 0, txtNewRate.Text.Trim)
			End If
			oPayment.PaymentProtection = True
			oPayment.QuoteDate = DateTime.Now


			If UCase(oQuote.Frequency) = "W" Or UCase(oQuote.Frequency) = "F" Then
				oPayment.WeekDay = oQuote.SavedDayInMonth
			Else
				oPayment.WeekDay = 1 'Default to 1 as no input field available in nexus
			End If


			'If PreferredDate logic is changed here, make sure to change in GetMatchingQuotes SAM method

			oPayment.StartDate = oQuote.CoverStartDate
			oPayment.EndDate = oQuote.CoverEndDate

			oPayment.IsUseTransactionCurrency = chkUseTransactionCurrency.Checked


			If (Session(CNMTAType) IsNot Nothing OrElse Session(CNInstalmentPlanMode) = InstalmentPlanType.edit) AndAlso Request.QueryString("Type") IsNot Nothing AndAlso Request.QueryString("Type") = "MTA" OrElse Request.QueryString("Type") = "NewPlanSED" Then
				oPayment.InstallmentType = rbInstalmentType.SelectedValue
				oPayment.InstallmentTypeSpecified = True
			ElseIf Session(CNMTAType) IsNot Nothing Then
				oPayment.InstallmentType = rbInstalmentType.SelectedValue
				oPayment.InstallmentTypeSpecified = True
			Else
				oPayment.InstallmentTypeSpecified = False
			End If
			'We need to set Bank details for selected account type. 

			'Save payment object in session.So that it can be used in BindQuote method
			Session(CNPayment) = oPayment
			oQuote.DepositTransactasInstalment = oSelectedInstalmentQuote.DepositAsInstalment
			oQuote.InstDepositAmount = oSelectedInstalmentQuote.DepositAmount
			Session(CNQuote) = oQuote
		End Sub
		''' <summary>
		''' Checks Party Bank Key of Token
		''' </summary>
		''' <param name="sTokenNumber"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function GetPartyBankKeyOnToken(ByVal sTokenNumber As String) As Integer
			Dim nPartyBankId As Integer = 0
			For Each oToken As ListItem In ddlExistingTokens.Items
				If oToken.Text = sTokenNumber Then
					nPartyBankId = CInt(oToken.Value)
					Exit For
				End If
			Next
			Return nPartyBankId
		End Function
		''' bind Instalments quote to grid
		''' </summary>
		''' <remarks></remarks>
		Public Sub bindInstalments()
			Dim dTotalRiskTaxExcludedFromInstalment As Decimal
			Dim dTotalFeeTaxExcludedFromInstalment As Decimal
			Dim dTotalRiskFeeExcludedFromInstalment As Double
			Dim dAmountToFinance As Double
			Dim dAgentCommission As Double
			Dim dTaxOnAgentCommission As Double
			Dim nPartyBankId As Integer = 0
			Dim nDayInMonth As Integer = 1
			Dim nWeekDay As Integer = 1
			Dim oPaymentOptions As Config.PaymentTypes = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).PaymentTypes

			If (Session(CNSelectedPaymentIndex) IsNot Nothing) AndAlso oPaymentOptions.PaymentType(Session(CNSelectedPaymentIndex)).Type = "PremiumFinance" OrElse (Session(CNAmountToPay) IsNot Nothing AndAlso CType(Session(CNAmountToPay), Double) <> 0.0) Then
				Dim oInstalmentQuotes As NexusProvider.InstallmentQuoteCollection
				'Dim oPartyBankDetails As NexusProvider.BankCollection
				'Dim oPartyBankDetail As NexusProvider.Bank
				Dim oParty As NexusProvider.BaseParty = CType(Session(CNParty), NexusProvider.BaseParty)
				Dim oPartyBankDetailsForInstalment As New NexusProvider.BankCollection
				Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
				Dim oQuote As NexusProvider.Quote
				Dim oQuoteForTax As NexusProvider.Quote
				Dim oQuoteForFees As NexusProvider.Quote
				Dim paymentOptions As New Config.PaymentTypes
				'Dim oPayment As New NexusProvider.Payment(NexusProvider.PaymentTypes.None)

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
				Partybankidcache = New Guid()

				ViewState.Add("Partybankidcache", Partybankidcache.ToString)
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
					'oQuote.InceptionDate = Date.Today

					If Session(CNPFUseTransCurrency) = 0 Then
						CheckUseTransCurrency(oQuote.BaseCurrencyID, oQuote.TransCurrencyID)
						Session(CNPFUseTransCurrency) = 1
					End If

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

					Dim sAgentType As String = String.Empty
					If Session(CNAgentType) IsNot Nothing Then
						sAgentType = Session(CNAgentType).ToString.Trim.ToUpper
					End If

					If sAgentType = "BROKER" Then
						dAmountToFinance = CType(Session(CNAmountToPay), Double) - (oQuoteForTax.TotalPolicyTaxExcludedFromFinancing + oQuoteForFees.TotalPolicyFeesExcludedFromFinancing + dTotalRiskTaxExcludedFromInstalment + dTotalFeeTaxExcludedFromInstalment + dTotalRiskFeeExcludedFromInstalment)
						dAmountToFinance = dAmountToFinance + dAgentCommission
					Else
						dAmountToFinance = CType(Session(CNAmountToPay), Double) - (oQuoteForTax.TotalPolicyTaxExcludedFromFinancing + oQuoteForFees.TotalPolicyFeesExcludedFromFinancing + dTotalRiskTaxExcludedFromInstalment + dTotalFeeTaxExcludedFromInstalment + dTotalRiskFeeExcludedFromInstalment)
					End If

					dOverrideInterestRate = ViewState("dOverrideInterestRate")
					dOverrideRate = ViewState("dOverrideRate")
					bPaymentProtection = ViewState("bPaymentProtection")
					dOverrideDepositAmount = ViewState("dOverrideDepositAmount")
					bOverrideCommission = ViewState("bOverrideCommission")

					If UCase(oQuote.Frequency) = "W" Then
						nWeekDay = oQuote.SavedDayInMonth
					Else
						nWeekDay = 1  'Default to 1 as no input field available in nexus
					End If

					'If process is MTA then we need to display instalment type option 
					If (Not Session(CNMTAType) Is Nothing AndAlso Session(CNMode) <> Mode.View AndAlso Session(CNMTAType) <> MTAType.REINSTATEMENT) OrElse (Request.QueryString("Type") IsNot Nothing AndAlso Request.QueryString("Type") = "MTA") Then

						'WPR005 - Do not Show Plans if user lands with this querystring request 
						If Request.QueryString("ShowPlan") IsNot Nothing AndAlso Request.QueryString("ShowPlan") = "False" Then
							'Selection of instalment type will be visible only for MTA
							pnlInstalmentType.Visible = False
							'Call on Instalment Plan Maintenance MTA
							Dim iPFPremFinanceKey As Integer = 0
							Dim iPFPremFinanceVersion As Integer = 0
							If Request.QueryString("FinancePlanKey") IsNot Nothing AndAlso Request.QueryString("FinancePlanKey") <> "" AndAlso Request.QueryString("FinancePlanVersion") IsNot Nothing AndAlso Request.QueryString("FinancePlanVersion") <> "" Then
								iPFPremFinanceKey = Request.QueryString("FinancePlanKey")
								iPFPremFinanceVersion = Request.QueryString("FinancePlanVersion")
							End If
							oInstalmentQuotes = oWebService.GetInstalmentQuotes(v_dAmountToFinance:=0,
											 v_dtStartDate:=oQuote.CoverStartDate, v_dtEndDate:=oQuote.CoverEndDate, v_dtPreferredDate:=Date.Today,
											  v_dtQuoteDate:=DateTime.Now, v_iWeekDay:=nWeekDay, v_iMonthDay:=1, v_iInsuranceFileKey:=oQuote.InsuranceFileKey,
											  v_dOverrideInterestRate:=dOverrideInterestRate, v_dOverrideRate:=dOverrideRate, v_bPaymentProtection:=bPaymentProtection,
											  v_sBranchCode:=oQuote.BranchCode, v_iInstallmentType:=NexusProvider.InstalmentType.NoAmountChange, sProcessPFMode:="MTA",
											  v_OverrideDepositAmount:=dOverrideDepositAmount, bOverrideCommission:=bOverrideCommission,
											  iPremiumFinancekey:=iPFPremFinanceKey, iPremiumFinanceVersionKey:=iPFPremFinanceVersion)
							Dim oFinancePlan As New NexusProvider.FinancePlan
							oFinancePlan = oWebService.GetFinancePlanDetails(oQuote.InsuranceFileKey)
							If oFinancePlan IsNot Nothing Then
								Session(CNFinancePlan) = oFinancePlan
								ddlDayinMonth.SelectedValue = oFinancePlan.DayOfWeekOrMonth
							End If
						Else
							'Selection of instalment type will be visible only for MTA
							pnlInstalmentType.Visible = True

							'check if existing plan is completed or cancelled
							'if yes then disable first 2 options and 3rd should be default selected
							If oQuote.OriginalInsuranceFileKey <> 0 Then
								Dim oFinancePlanDetail As New NexusProvider.FinancePlan
								oFinancePlanDetail = oWebService.GetFinancePlanDetails(oQuote.OriginalInsuranceFileKey, nPremiumFinanceCnt:=oQuote.DefaultInstalmentPlan, nPremiumFinanceVersion:=oQuote.DefaultInstalmentPlanVersion)
								If oFinancePlanDetail IsNot Nothing Then
									Session(CNFinancePlan) = oFinancePlanDetail
									Dim oSysOptionNextInstalmentRenewal As NexusProvider.OptionTypeSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5075)
									If Session(CNIsTrueMonthlyPolicy) = True AndAlso oSysOptionNextInstalmentRenewal.OptionValue = "1" AndAlso Session(CNMTAType) IsNot Nothing AndAlso
										 oQuote.GrossTotal < 0 Then
										rbInstalmentType.Items(0).Enabled = False
										rbInstalmentType.Items(2).Enabled = False
										rbInstalmentType.SelectedValue = "1"
									Else
										If oFinancePlanDetail.StatusDescription = "Cancelled" Or oFinancePlanDetail.StatusDescription = "Completed" Then
											rbInstalmentType.Items(0).Enabled = False
											rbInstalmentType.Items(1).Enabled = False
											rbInstalmentType.SelectedValue = "2"
										Else
											'By default First option(AddAndSpread) will be selected 
											rbInstalmentType.SelectedValue = "0"
										End If
									End If


								End If
							Else
								Dim rblSelectedVal As String = rbInstalmentType.SelectedValue

								'By default First option(AddAndSpread) will be selected 
								rbInstalmentType.SelectedValue = "0"

								''when policy changed to instalment policy from invoice during MTA
								If rblSelectedVal <> "0" AndAlso CInt(Request.QueryString("FinancePlanKey")) = 0 _
									AndAlso (Session(CNMTAType) = MTAType.PERMANENT OrElse Session(CNMTAType) = MTAType.TEMPORARY) Then
									rbInstalmentType.SelectedValue = rblSelectedVal
								End If
							End If


							If Request.QueryString("Type") IsNot Nothing AndAlso Request.QueryString("Type") = "MTA" Then
								'Call on Instalment Plan Maintenance MTA
								Dim iPFPremFinanceKey As Integer = 0
								Dim iPFPremFinanceVersion As Integer = 0
								If Request.QueryString("FinancePlanKey") IsNot Nothing AndAlso Request.QueryString("FinancePlanKey") <> "" AndAlso Request.QueryString("FinancePlanVersion") IsNot Nothing AndAlso Request.QueryString("FinancePlanVersion") <> "" Then
									iPFPremFinanceKey = Request.QueryString("FinancePlanKey")
									iPFPremFinanceVersion = Request.QueryString("FinancePlanVersion")

								End If
								If (Session(CNTransactionDetails)) IsNot Nothing Then
									Dim oFinancePlanTransactionCollection As New NexusProvider.FinancePlanTransactionsCollection
									oFinancePlanTransactionCollection = Session(CNTransactionDetails)

									oInstalmentQuotes = oWebService.GetInstalmentQuotes(v_dAmountToFinance:=0,
																 v_dtStartDate:=oQuote.CoverStartDate, v_dtEndDate:=oQuote.CoverEndDate, v_dtPreferredDate:=Date.Today,
																  v_dtQuoteDate:=DateTime.Now, v_iWeekDay:=nWeekDay, v_iMonthDay:=1, v_iInsuranceFileKey:=oQuote.InsuranceFileKey,
																  v_dOverrideInterestRate:=dOverrideInterestRate, v_dOverrideRate:=dOverrideRate, v_bPaymentProtection:=bPaymentProtection,
																  v_sBranchCode:=oQuote.BranchCode, v_iInstallmentType:=rbInstalmentType.SelectedValue, sProcessPFMode:="MTA",
																  v_OverrideDepositAmount:=dOverrideDepositAmount, bOverrideCommission:=bOverrideCommission,
																  iPremiumFinancekey:=iPFPremFinanceKey, iPremiumFinanceVersionKey:=iPFPremFinanceVersion,
																  oPFTransactionCollection:=oFinancePlanTransactionCollection)

								End If
							Else
								'Call on Policy MTA
								oInstalmentQuotes = oWebService.GetInstalmentQuotes(v_dAmountToFinance:=dAmountToFinance,
												 v_dtStartDate:=oQuote.CoverStartDate, v_dtEndDate:=oQuote.CoverEndDate, v_dtPreferredDate:=Date.Today,
												 v_dtQuoteDate:=DateTime.Now, v_iWeekDay:=1, v_iMonthDay:=1, v_iInsuranceFileKey:=oQuote.InsuranceFileKey,
												 v_dOverrideInterestRate:=-1, v_dOverrideRate:=0, v_bPaymentProtection:=True, v_sBranchCode:=oQuote.BranchCode,
												 v_iInstallmentType:=rbInstalmentType.SelectedValue, iPremiumFinancekey:=oQuote.DefaultInstalmentPlan, iPremiumFinanceVersionKey:=oQuote.DefaultInstalmentPlanVersion,
												 v_OverrideDepositAmount:=dOverrideDepositAmount)
							End If
						End If
					Else

						'Incase of SED and SRD plans selected
						If Request.QueryString("Type") = "NewPlanSED" Then
							Dim nInsuranceFileKey As Integer = oQuote.InsuranceFileKey
							oFinancePlanInformation = oWebService.GetFinancePlanInformation(oQuote.InsuranceFileKey)
							Session("PFProductCode") = oFinancePlanInformation.ProductCode
							'In Nexus, we does not have input field for selecting weekday and month day
							'By default first value is selected in BO.So passing value 1 for these parameters
							If oFinancePlanInformation.ProductCode.ToUpper() = "MTA" OrElse oFinancePlanInformation.ProductCode.ToUpper() = "REN" Then
								'Selection of instalment type will be visible only for MTA
								pnlInstalmentType.Visible = True
								'TFS Defect #9179
								'By default First option(AddAndSpread) will be selected 
								'rbInstalmentType.SelectedValue = "0"
								rbInstalmentType.Visible = False

								If oFinancePlanInformation.ProductCode.ToUpper() = "MTA" Then
									nInsuranceFileKey = oFinancePlanInformation.OriginalInsuranceFileKey
								End If
								If oQuote.InsuranceFileKey <> 0 Then
									Dim oFinancePlan As New NexusProvider.FinancePlan
									If oFinancePlanInformation.ProductCode.ToUpper() = "MTA" Then
										oFinancePlan = oWebService.GetFinancePlanDetails(oFinancePlanInformation.OriginalInsuranceFileKey)
									Else
										oFinancePlan = oWebService.GetFinancePlanDetails(oQuote.InsuranceFileKey)
									End If
									If oFinancePlan IsNot Nothing Then
										Session(CNFinancePlan) = oFinancePlan
									End If
								End If
							End If
							'On load of Plan NB
							Dim oFinancePlanTransactionCollection As New NexusProvider.FinancePlanTransactionsCollection
							If (Session(CNTransactionDetails)) IsNot Nothing Then
								oFinancePlanTransactionCollection = Session(CNTransactionDetails)
							End If

							oInstalmentQuotes = oWebService.GetInstalmentQuotes(v_dAmountToFinance:=dAmountToFinance,
														v_dtStartDate:=oQuote.CoverStartDate, v_dtEndDate:=oQuote.CoverEndDate, v_dtPreferredDate:=Date.Today,
														v_dtQuoteDate:=DateTime.Now, v_iWeekDay:=1, v_iMonthDay:=1, v_iInsuranceFileKey:=nInsuranceFileKey,
														v_dOverrideInterestRate:=dOverrideInterestRate, v_dOverrideRate:=dOverrideRate, v_bPaymentProtection:=bPaymentProtection,
														v_iInstallmentType:=rbInstalmentType.SelectedValue, r_nPartyBankId:=nPartyBankId,
														bIsUseTransactionCurrency:=chkUseTransactionCurrency.Checked, v_OverrideDepositAmount:=dOverrideDepositAmount,
														bOverrideCommission:=bOverrideCommission, sProcessPFMode:=oFinancePlanInformation.ProductCode.ToUpper(),
																				oPFTransactionCollection:=oFinancePlanTransactionCollection)
						ElseIf Not Session(CNMode) Is Nothing AndAlso Session(CNMode) = Mode.View Then
							Dim nInsuranceFileKey As Integer = oQuote.InsuranceFileKey
							If oQuote.InsuranceFileKey <> 0 Then
								Dim oFinancePlan As New NexusProvider.FinancePlan
								oFinancePlan = oWebService.GetFinancePlanDetails(oQuote.InsuranceFileKey)
								If oFinancePlan IsNot Nothing Then
									Session(CNFinancePlan) = oFinancePlan
								End If
								lbl_Plan_ref.Text = oFinancePlan.PlanReference
							End If
							oFinancePlanInformation = oWebService.GetFinancePlanInformation(oQuote.InsuranceFileKey)
							If oFinancePlanInformation.ProductCode.ToUpper() = "REN" Then
								oInstalmentQuotes = oWebService.GetInstalmentQuotes(v_dAmountToFinance:=dAmountToFinance,
														v_dtStartDate:=oQuote.CoverStartDate, v_dtEndDate:=oQuote.CoverEndDate, v_dtPreferredDate:=Date.Today,
														v_dtQuoteDate:=DateTime.Now, v_iWeekDay:=1, v_iMonthDay:=1, v_iInsuranceFileKey:=nInsuranceFileKey,
														v_dOverrideInterestRate:=dOverrideInterestRate, v_dOverrideRate:=dOverrideRate, v_bPaymentProtection:=bPaymentProtection,
														v_iInstallmentType:=NexusProvider.InstalmentType.NoAmountChange, v_OverrideDepositAmount:=dOverrideDepositAmount,
														bOverrideCommission:=bOverrideCommission)
							Else
								oInstalmentQuotes = oWebService.GetInstalmentQuotes(v_dAmountToFinance:=0,
														v_dtStartDate:=oQuote.CoverStartDate, v_dtEndDate:=oQuote.CoverEndDate, v_dtPreferredDate:=Date.Today,
														v_dtQuoteDate:=DateTime.Now, v_iWeekDay:=1, v_iMonthDay:=1, v_iInsuranceFileKey:=nInsuranceFileKey,
														v_dOverrideInterestRate:=dOverrideInterestRate, v_dOverrideRate:=dOverrideRate, v_bPaymentProtection:=bPaymentProtection,
														v_iInstallmentType:=NexusProvider.InstalmentType.NoAmountChange, sProcessPFMode:="MTA", v_OverrideDepositAmount:=dOverrideDepositAmount,
														bOverrideCommission:=bOverrideCommission)
							End If


							Dim oInstalmentQuotesModified As New NexusProvider.InstallmentQuoteCollection
							For Each instalmentQuote As NexusProvider.InstalmentQuote In oInstalmentQuotes
								If instalmentQuote.SchemeName = CType(Session(CNFinancePlan), NexusProvider.FinancePlan).SchemeName Then
									oInstalmentQuotesModified.Add(instalmentQuote)
								End If
							Next
							oInstalmentQuotes = oInstalmentQuotesModified
						Else
							If Session(CNRenewal) IsNot Nothing AndAlso Convert.ToBoolean(Session(CNRenewal)) Then
								Dim oFinancePlanDetail As New NexusProvider.FinancePlan
								oFinancePlanDetail = oWebService.GetFinancePlanDetails(oQuote.InsuranceFileKey, nPremiumFinanceCnt:=oQuote.DefaultInstalmentPlan, nPremiumFinanceVersion:=oQuote.DefaultInstalmentPlanVersion)
								If oFinancePlanDetail IsNot Nothing Then
									Session(CNFinancePlan) = oFinancePlanDetail
								End If
								oFinancePlanDetail = CType(Session(CNFinancePlan), NexusProvider.FinancePlan)
								hdnMediaType.Value = oFinancePlanDetail.PaymentMethod
							End If
							'In Nexus, we does not have input field for selecting weekday and month day
							'By default first value is selected in BO.So passing value 1 for these parameters
							'On load of Plan NB
							If oQuote.InsuranceFileKey <> 0 Then
								Dim oFinancePlan As New NexusProvider.FinancePlan
								oFinancePlan = oWebService.GetFinancePlanDetails(oQuote.InsuranceFileKey)
								If oFinancePlan IsNot Nothing Then
									Session(CNFinancePlan) = oFinancePlan
								End If
							End If
							oInstalmentQuotes = oWebService.GetInstalmentQuotes(v_dAmountToFinance:=dAmountToFinance,
													 v_dtStartDate:=oQuote.CoverStartDate, v_dtEndDate:=oQuote.CoverEndDate, v_dtPreferredDate:=Date.Today,
													 v_dtQuoteDate:=DateTime.Now, v_iWeekDay:=1, v_iMonthDay:=1, v_iInsuranceFileKey:=oQuote.InsuranceFileKey,
													 v_dOverrideInterestRate:=dOverrideInterestRate, v_dOverrideRate:=dOverrideRate, v_bPaymentProtection:=bPaymentProtection,
													 v_sBranchCode:=oQuote.BranchCode, v_iInstallmentType:=NexusProvider.InstalmentType.AddToNewPlan, r_nPartyBankId:=nPartyBankId,
													 bIsUseTransactionCurrency:=chkUseTransactionCurrency.Checked, v_OverrideDepositAmount:=dOverrideDepositAmount, bOverrideCommission:=bOverrideCommission,
													 sProcessPFMode:=If(Session(Nexus.Constants.Session.CNMode) = Nexus.Constants.Mode.SalvageClaim, "SR", If(Session(Nexus.Constants.Session.CNMode) = Nexus.Constants.Mode.TPRecovery, "TPR", Nothing)))

						End If
					End If

					If (oInstalmentQuotes IsNot Nothing AndAlso oInstalmentQuotes.Count > 0) Then
						If oInstalmentQuotes(0).UseTransCurrncy = 1 And oInstalmentQuotes(0).ProductCode = "MTA" AndAlso chkUseTransactionCurrency.Checked Then
							chkUseTransactionCurrency.Checked = True
							chkUseTransactionCurrency.Enabled = True
						End If
						If oInstalmentQuotes(0).ProductCode = "MTA" AndAlso rbInstalmentType.SelectedValue <> "2" Then
							If oInstalmentQuotes(0).UseTransCurrncy = 1 Then
								chkUseTransactionCurrency.Checked = True
							Else
								chkUseTransactionCurrency.Checked = False
							End If

						End If
						oInstalmentQuotes.SortColumn = "SchemeName"
						oInstalmentQuotes.SortingOrder = NexusProvider.GenericComparer.SortOrder.Descending
						oInstalmentQuotes.Sort()
					End If
					'Add the retrived quotes in cache.So that they can be used throughout the page
					Cache.Insert(ViewState("InstalmentQuotesCacheID"), oInstalmentQuotes, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
					Cache.Insert(ViewState("Partybankidcache"), nPartyBankId, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
					pnlPlanSummary.Visible = True
				Catch ex As NexusProvider.NexusException
					If ex.Errors(0).Code = "0" Then
						pnlPlanSummary.Visible = False
					Else
						Throw
					End If
				End Try


				'Bind the grid with retrieved quotes
				grdInstallmentQuotes.DataSource = oInstalmentQuotes
				grdInstallmentQuotes.DataBind()

				GetPartyBankDetails()

				If grdInstallmentQuotes.SelectedIndex <> -1 AndAlso grdInstallmentQuotes.Rows.Count > 0 Then
					grdInstallmentQuotes.Rows(grdInstallmentQuotes.SelectedIndex).Cells(0).Style.Add(HtmlTextWriterStyle.FontWeight, "Bold")
				End If

				'Details for selected instalment quote should be visible in pnlPlanSummary
				If grdInstallmentQuotes.Rows.Count > 0 Then
					If grdInstallmentQuotes.SelectedIndex = -1 Then
						Dim bUsePriorTermSchemeAtRenewal As Boolean = False
						Dim eGridViewSelectEvent As System.Web.UI.WebControls.GridViewSelectEventArgs = Nothing
						Dim eGridViewPageEvent As System.Web.UI.WebControls.GridViewPageEventArgs = Nothing
						If Session(CNInstalmentsPlan) IsNot Nothing AndAlso Session(CNMTAType) Is Nothing Then
							Dim sInstalmentsPlan() As String = Session(CNInstalmentsPlan).ToString().Split(",")
							Session(CNInstalmentsPlan) = Nothing
							Dim nPageIndex As Integer = -1
							Dim nSelectedItem As Integer = -1
							For iCount As Integer = 0 To oInstalmentQuotes.Count - 1
								If CInt(sInstalmentsPlan(0)) = oInstalmentQuotes(iCount).SchemeNo AndAlso CInt(sInstalmentsPlan(1)) = oInstalmentQuotes(iCount).SchemeVersion Then
									nPageIndex = CInt(Math.Ceiling((iCount + 1) / grdInstallmentQuotes.PageSize) - 1)
									nSelectedItem = CInt(iCount - (grdInstallmentQuotes.PageSize * nPageIndex))
									eGridViewSelectEvent = New System.Web.UI.WebControls.GridViewSelectEventArgs(nSelectedItem)
									eGridViewPageEvent = New System.Web.UI.WebControls.GridViewPageEventArgs(nPageIndex)
									grdInstallmentQuotes_PageIndexChanging(Nothing, eGridViewPageEvent)
									grdInstallmentQuotes_SelectedIndexChanging(Nothing, eGridViewSelectEvent)
									bUsePriorTermSchemeAtRenewal = True
									Exit For
								End If
							Next
							'If some quotes retrieved then first instalment quote should be selectes and btNext should be visible.
							If Not bUsePriorTermSchemeAtRenewal Then
								'Select first row
								eGridViewSelectEvent = New System.Web.UI.WebControls.GridViewSelectEventArgs(0)
								grdInstallmentQuotes_SelectedIndexChanging(Nothing, eGridViewSelectEvent)
								'Show instalment details for selecetd instalment quote
								ShowDetailsForScheme(oInstalmentQuotes(0).SchemeNo, oInstalmentQuotes(0).SchemeVersion, oInstalmentQuotes(0).CompanyNo)
							End If

							''Set the condition for View mode so that default scheme can be appear in selected mode.
						ElseIf (Session(CNMode) = Mode.View) _
						OrElse (Not Session(CNMTAType) Is Nothing OrElse (Request.QueryString("Type") IsNot Nothing AndAlso Request.QueryString("Type") = "MTA")) _
						AndAlso Not oQuote Is Nothing Then


							Dim nPageIndex As Integer = -1
							Dim nSelectedItem As Integer = -1

							For iCount As Integer = 0 To oInstalmentQuotes.Count - 1
								If CInt(oQuote.DefaultSchemeNumber) = oInstalmentQuotes(iCount).SchemeNo AndAlso CInt(oQuote.DefaultSchemeVersion) = oInstalmentQuotes(iCount).SchemeVersion Then
									nPageIndex = CInt(Math.Ceiling((iCount + 1) / grdInstallmentQuotes.PageSize) - 1)
									nSelectedItem = CInt(iCount - (grdInstallmentQuotes.PageSize * nPageIndex))
									eGridViewSelectEvent = New System.Web.UI.WebControls.GridViewSelectEventArgs(nSelectedItem)
									eGridViewPageEvent = New System.Web.UI.WebControls.GridViewPageEventArgs(nPageIndex)
									grdInstallmentQuotes_PageIndexChanging(Nothing, eGridViewPageEvent)
									grdInstallmentQuotes_SelectedIndexChanging(Nothing, eGridViewSelectEvent)
									bUsePriorTermSchemeAtRenewal = True
									Exit For
								End If
							Next

						Else
							'If some quotes retrieved then first instalment quote should be selectes and btNext should be visible.
							'Select first row
							eGridViewSelectEvent = New System.Web.UI.WebControls.GridViewSelectEventArgs(0)
							grdInstallmentQuotes_SelectedIndexChanging(Nothing, eGridViewSelectEvent)
							'Show instalment details for selecetd instalment quote
							ShowDetailsForScheme(oInstalmentQuotes(0).SchemeNo, oInstalmentQuotes(0).SchemeVersion, oInstalmentQuotes(0).CompanyNo)
						End If
					Else
						ShowDetailsForScheme(oInstalmentQuotes(grdInstallmentQuotes.SelectedIndex).SchemeNo, oInstalmentQuotes(grdInstallmentQuotes.SelectedIndex).SchemeVersion, oInstalmentQuotes(grdInstallmentQuotes.SelectedIndex).CompanyNo, oInstalmentQuotes(grdInstallmentQuotes.SelectedIndex).FrequencyID)
					End If
					'Make visible tyo plan summary and btnNext
					pnlPlanSummary.Visible = True
				Else
					If grdInstallmentQuotes.SelectedIndex <> -1 AndAlso grdInstallmentQuotes.Rows.Count > 0 Then
						Dim iSelectedSchemeNumber As Integer = grdInstallmentQuotes.DataKeys(grdInstallmentQuotes.SelectedIndex).Values("SchemeNo")
						Dim iSelectedSchemeVersion As Integer = grdInstallmentQuotes.DataKeys(grdInstallmentQuotes.SelectedIndex).Values("SchemeVersion")
						Dim iSelectedCompanyNumber As Integer = grdInstallmentQuotes.DataKeys(grdInstallmentQuotes.SelectedIndex).Values("CompanyNo")
						Dim iSelectedFrequencyId As Integer = grdInstallmentQuotes.DataKeys(grdInstallmentQuotes.SelectedIndex).Values("FrequencyID")

						'Show instalment details for selected instalment quote
						If hvActualIndex.Value <> "" Then
							PopulateInstalmentDates(hvActualIndex.Value)
						Else
							PopulateInstalmentDates(grdInstallmentQuotes.SelectedIndex)
						End If

						CallGetInstalmentQuotes()
						ShowDetailsForScheme(iSelectedSchemeNumber, iSelectedSchemeVersion, iSelectedCompanyNumber)
						Exit Sub
					End If
					If grdInstallmentQuotes.Rows.Count > 0 Then
						PopulateInstalmentDates()
					End If
				End If

				If oQuote IsNot Nothing AndAlso oQuote.ActivePlan > 0 Then
					Dim oPayment As New NexusProvider.Payment(NexusProvider.PaymentTypes.None, CDec(HttpContext.Current.Session(CNAmountToPay)))
					oPayment = HttpContext.Current.Session(CNPayment)
					oWebService = New NexusProvider.ProviderManager().Provider
					oWebService.SavePremiumFinanceDetails(oPayment, oQuote.InsuranceFileKey, "REN")
				End If

				If oQuote IsNot Nothing AndAlso grdInstallmentQuotes.SelectedIndex < 0 Then
					For Each row As GridViewRow In grdInstallmentQuotes.Rows
						If (grdInstallmentQuotes.DataKeys(row.RowIndex).Values("SchemeNo").ToString().Trim() = oQuote.DefaultSchemeNumber.ToString().Trim()) Then
							grdInstallmentQuotes.SelectedIndex = row.RowIndex
							Exit For
						End If
					Next
				End If
				If Session(CNRenewal) IsNot Nothing AndAlso Convert.ToBoolean(Session(CNRenewal)) AndAlso
					Not IsPostBack AndAlso Session(CNFinancePlan) IsNot Nothing Then
					Dim oFinancePlan As New NexusProvider.FinancePlan
					oFinancePlan = CType(Session(CNFinancePlan), NexusProvider.FinancePlan)
					ddlDayinMonth.SelectedValue = DateAndTime.Day(CDate(oFinancePlan.NextInstalmentDate))
					ddlFirstPaymentDate.SelectedValue = oFinancePlan.FirstInstalmentDate.ToShortDateString
				End If

				''handle special case when MTA done from Invoice, cash etc. to Instalment and rbInstalmentType is 0 or 1
				If grdInstallmentQuotes.Rows.Count = 0 Then
					txtFinancedAmount.Text = String.Empty
					txtTotalPayable.Text = String.Empty
					txtTransactions.Text = String.Empty
					txtInstallements.Text = String.Empty
					txtRate.Text = String.Empty
					txtAPR.Text = String.Empty
					txtDeposit.Text = String.Empty
					txtAdminCharge.Text = String.Empty
					txtProtectionCharge.Text = String.Empty
					txtInterest.Text = String.Empty
					txtFirstInstalment.Text = String.Empty
					txtNextInstalment.Text = String.Empty
					txtLastInstalment.Text = String.Empty
					txtOtherInstalment.Text = String.Empty
					txtTaxes.Text = String.Empty
					txtGrossDue.Text = String.Empty
					txtTotalFees.Text = String.Empty
					txtTotalTaxes.Text = String.Empty
					txtTotalAmount.Text = String.Empty
					txtTotalFeesCollect.Text = String.Empty
					txtTotalTaxesCollect.Text = String.Empty
					txtMinimumDeposit.Text = String.Empty
					txtFirstInstalmentDate.Text = String.Empty
					pnlBankDetails.Visible = False
					ddlAccountType.Items.Clear()
					rfvAccountType.Enabled = False
					custValAccountType.Enabled = False
					pnlCreditCardDetails.Visible = False
					rfvOverrideDepositRange.Enabled = False
				End If
			End If
		End Sub

		''' <summary>
		''' 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub chkUseTransactionCurrency_CheckedChanged(sender As Object, e As EventArgs) Handles chkUseTransactionCurrency.CheckedChanged
			bindInstalments()
		End Sub

		Protected Sub btnOverride_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOverride.Click
			If hdnRate.Value = "1" Then
				If chkOverrideInterest.Checked Then
					ViewState("dOverrideInterestRate") = CDbl(txtNewRate.Text)
					ViewState("dOverrideRate") = 1
				Else
					ViewState("dOverrideInterestRate") = -1
					ViewState("dOverrideRate") = 0
				End If
			End If

			If hdnProtection.Value = "1" Then
				If chkPaymentProtection.Checked Then
					ViewState("bPaymentProtection") = True
				Else
					ViewState("bPaymentProtection") = False
				End If
			End If

			If hdnCommission.Value = "1" Then
				If chkOverrideCommission.Checked Then
					ViewState("dOverrideCommission") = 1
				Else
					ViewState("dOverrideCommission") = -1
				End If
			End If

			If hdnDeposit.Value = "1" Then
				If chkOverrideDeposit.Checked Then
					If optOverrideDepositType.SelectedValue = "P" Then
						If hdnIsSuppressDecimals IsNot Nothing AndAlso hdnIsSuppressDecimals.Value = "1" Then
							ViewState("dOverrideDepositAmount") = Math.Round((CDbl(txtOverrideDeposit.Text) * CDbl(txtFinancedAmount.Text)) / 100, 0, MidpointRounding.AwayFromZero)
						Else
							ViewState("dOverrideDepositAmount") = CDbl((CDbl(txtOverrideDeposit.Text) * CDbl(txtFinancedAmount.Text)) / 100)
						End If


					Else
						ViewState("dOverrideDepositAmount") = CDbl(txtOverrideDeposit.Text)
					End If
				Else
					ViewState("dOverrideDepositAmount") = -1
				End If
			End If
			bindInstalments()
		End Sub

		Protected Sub cvValidateToken_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvValidateToken.ServerValidate
			cvValidateToken.ErrorMessage = GetLocalResourceObject("cv_ValidateToken").ToString()
			If ddlExistingTokens.SelectedIndex > 0 Then
				args.IsValid = True
			Else
				If String.IsNullOrEmpty(txtTokenNo.Text) Then
					args.IsValid = False
				Else
					args.IsValid = True
				End If
			End If

			If ddlExistingTokens.SelectedIndex > 0 Then
				RqdTokenNo.Enabled = False
			Else
				RqdTokenNo.Enabled = True
			End If

			If ddlExistingTokens.Items.Count > 0 Then
				If Not String.IsNullOrEmpty(txtTokenNo.Text) Then
					For iCount As Integer = 1 To ddlExistingTokens.Items.Count - 1
						If ddlExistingTokens.Items(iCount).Text.Trim() = txtTokenNo.Text.Trim() Then
							args.IsValid = False
							cvValidateToken.ErrorMessage = GetLocalResourceObject("cv_DuplicateTokenNumberMessage").ToString()
							Exit For
						Else
							args.IsValid = True
						End If
					Next
				End If
			End If
		End Sub

		Protected Sub cvZeroPremium_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvZeroPremium.ServerValidate
			If (Session(CNMTAType) Is Nothing OrElse (Session(CNMTAType) <> MTAType.CANCELLATION AndAlso Session(CNMTAType) <> MTAType.PERMANENT)) AndAlso
				Session(CNAmountToPay) IsNot Nothing AndAlso CType(Session(CNAmountToPay), Double) = 0.0 Then
				args.IsValid = False
			Else
				args.IsValid = True
			End If
		End Sub

		''' <summary>
		''' Validate Selected instalment plan
		''' </summary>
		''' <param name="source"></param>
		''' <param name="args"></param>
		''' <remarks></remarks>
		Protected Sub cvInstalmentPlans_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
			Dim oPaymentOptions As Config.PaymentTypes = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).PaymentTypes
			If Session(CNMTAType) IsNot Nothing AndAlso Session(CNMTAType) <> MTAType.CANCELLATION AndAlso Session(CNMTAType) <> MTAType.PERMANENT Then
				If oPaymentOptions.PaymentType(Session(CNSelectedPaymentIndex)).Type = "PremiumFinance" Then
					If Me.SelectedInstalmentQuote Is Nothing Then
						cvInstalmentPlans.ErrorMessage = GetLocalResourceObject("msg_NoLivePlan")
						args.IsValid = False
					Else
						If Me.SelectedInstalmentQuote.FirstInstalmentAmount < 0 Then
							cvInstalmentPlans.ErrorMessage = GetLocalResourceObject("msg_NegativeInstalment")
							args.IsValid = False
						End If
					End If
				End If
			End If

			oPaymentOptions = Nothing
		End Sub

		Protected Sub grdInstallmentQuotes_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdInstallmentQuotes.RowDataBound
			Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
			If e.Row.RowType = DataControlRowType.DataRow Then
				If (Session(CNMode) = Mode.View) Then
					Dim lnkSelect As LinkButton = e.Row.Cells(6).FindControl("lnkbtnSelect")
					If (lnkSelect IsNot Nothing) Then
						lnkSelect.Enabled = False
					End If
				End If
				If ViewState("PaymentHubEnabled") = "1" AndAlso ViewState("DoNotUseForSubsequentPayment") = "1" Then
					If e.Row.Cells(2).Text.ToString().Contains("Credit Card") Then
						e.Row.Visible = False
					End If
				End If
			End If
			If (e.Row.RowType = ListItemType.Header) Then
				Dim sCurrencyBaseSymbol As String
				Dim oCurrencies As Config.Currencies
				Dim oCurrencyColl As NexusProvider.CurrencyCollection
				Dim oQuote As NexusProvider.Quote
				If Not chkUseTransactionCurrency.Checked Then
					oQuote = Session(CNQuote)
					oCurrencyColl = oWebService.GetCurrenciesByBranch(oQuote.BranchCode)
					sCurrISOCode = oCurrencyColl(0).BaseCurrencyCode
					oCurrencies = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Currencies
					sCurrencyBaseSymbol = oCurrencies.Currency(sCurrISOCode).Symbol
					grdInstallmentQuotes.Columns(3).HeaderText = Replace(GetLocalResourceObject("lbl_Header_Deposit_Amt"), "[!Currency!]", sCurrencyBaseSymbol)
					grdInstallmentQuotes.Columns(4).HeaderText = Replace(GetLocalResourceObject("lbl_Header_Amount"), "[!Currency!]", sCurrencyBaseSymbol)
				Else
					grdInstallmentQuotes.Columns(3).HeaderText = Replace(GetLocalResourceObject("lbl_Header_Deposit_Amt"), "[!Currency!]", TransactionCurrency.Symbol)
					grdInstallmentQuotes.Columns(4).HeaderText = Replace(GetLocalResourceObject("lbl_Header_Amount"), "[!Currency!]", TransactionCurrency.Symbol)
				End If
			End If
			oWebService = Nothing
		End Sub

		''' <summary>
		''' Updates the internal instalment quotes cache with a pre-filtered collection.
		''' Called externally after filtering quotes (e.g. Claim Recovery CR filter).
		''' </summary>
		Public Sub UpdateInstalmentQuotesCache(ByVal oFilteredQuotes As NexusProvider.InstallmentQuoteCollection)
			If ViewState("InstalmentQuotesCacheID") IsNot Nothing Then
				Cache.Remove(ViewState("InstalmentQuotesCacheID").ToString())
				Cache.Insert(ViewState("InstalmentQuotesCacheID").ToString(), oFilteredQuotes, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
			End If
		End Sub

		''' <summary>
		''' Public wrapper to show details for a specific scheme (used after external filtering).
		''' </summary>
		Public Sub ShowDetailsForSelectedScheme(ByVal iSchemeId As Integer, ByVal iVersionId As Integer, ByVal iCompanyId As Integer, Optional ByVal iFrequencyId As Integer = 0)
			ShowDetailsForScheme(iSchemeId, iVersionId, iCompanyId, iFrequencyId)
		End Sub

		Public Sub SaveFinancePlan()
			Dim oPremiumFinancePlan As New NexusProvider.PremiumFinancePlan
			Dim oPayment As NexusProvider.Payment = Nothing
			Dim oPaymentOptions As Nexus.Library.Config.PaymentTypes = Nothing
			Dim oQuote As NexusProvider.Quote
			Dim oInstalmentQuote As NexusProvider.InstalmentQuote
			Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
			Dim nInsuranceFileKey As Integer = 0

			oQuote = Session(CNQuote)
			If Session(CNRenewal) IsNot Nothing AndAlso oQuote IsNot Nothing Then
				nInsuranceFileKey = oQuote.InsuranceFileKey
				Dim sPaymentMethod As String = Trim(oQuote.PaymentMethod.ToUpper())
				If sPaymentMethod = "INSTALMENT" OrElse sPaymentMethod = "PREMIUMFINANCE" OrElse sPaymentMethod = "INSTALMENTS" OrElse sPaymentMethod = "DIRECT DEBIT" Then
					SaveInstallmentPlan()
					'Allow Installment saving for Renewal only
					oPaymentOptions = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID()).PaymentTypes
					'If ViewState("SelectedInstalmentQuoteCacheId") IsNot Nothing AndAlso Cache.Item(ViewState("SelectedInstalmentQuoteCacheId")) IsNot Nothing Then
					oInstalmentQuote = Cache.Item(ViewState("SelectedInstalmentQuoteCacheId"))
					oPremiumFinancePlan.PFPremFinanceKey = oInstalmentQuote.SchemeNo
					oPremiumFinancePlan.PFPremFinanceVersion = oInstalmentQuote.SchemeVersion
					'End If
					If Session(CNPayment) IsNot Nothing Then
						oPayment = Session(CNPayment)
					End If
				End If
				oWebService.SavePremiumFinanceDetails(oPayment, nInsuranceFileKey, "REN")
			End If
		End Sub

		Sub FillCardDetails()
			Dim oCreditCardCollection As Object
			Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
			Dim oQuote As NexusProvider.Quote = Session(CNQuote)

			Dim oPartyBankDetails As NexusProvider.BankCollection
			Try
				oPartyBankDetails = oWebService.GetPartyBankDetails(oQuote.PartyKey, Session(CNTransBranchCode))
				oCreditCardCollection = (From r In oPartyBankDetails Where r.CreditCard IsNot Nothing AndAlso r.CreditCard.TrackingNumber IsNot Nothing AndAlso r.CreditCard.TrackingNumber <> "" _
				 AndAlso (Convert.ToInt32(Regex.Split(r.CreditCard.ExpiryDate, "/")(1)) > Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2, 2)) _
				 OrElse (Convert.ToInt32(Regex.Split(r.CreditCard.ExpiryDate, "/")(1)) = Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2, 2)) AndAlso Convert.ToInt32(Regex.Split(r.CreditCard.ExpiryDate, "/")(0)) >= Convert.ToInt32(DateTime.Now.Month.ToString())))
										 Select r.CreditCard.Number, r.CreditCard.NameOnCreditCard, r.PartyBankKey, r.CreditCard.ManualAuthCode, r.CreditCard.TrackingNumber, r.CreditCard.ExpiryDate, r.CreditCard.IsDefaultCreditCard).ToList()
				grdCard.DataSource = oCreditCardCollection
				grdCard.DataBind()
			Catch ex As Exception

			Finally
				oCreditCardCollection = Nothing
				oWebService = Nothing
				oPartyBankDetails = Nothing
				oQuote = Nothing
			End Try

		End Sub
		Protected Sub grdCard_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdCard.RowDataBound
			If e.Row.RowType = DataControlRowType.DataRow AndAlso ViewState("MarkDefaultCard") <> "1" Then
				CType(e.Row.FindControl("rdDefaultCard"), System.Web.UI.WebControls.RadioButton).Checked = False

			End If
		End Sub

		Private Sub ddlExistingTokens_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlExistingTokens.SelectedIndexChanged
			If ddlExistingTokens.SelectedIndex > 0 Then
				RqdTokenNo.Enabled = False
				txtTokenNo.Text = String.Empty
				txtTokenNo.Enabled = False
			Else
				RqdTokenNo.Enabled = True
				txtTokenNo.Text = String.Empty
				txtTokenNo.Enabled = True
			End If
		End Sub
	End Class
End Namespace
