Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Configuration
Imports System.IO
Imports System.Linq
Imports System.Web
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Web.HttpContext
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Xml.Linq
Imports Aspose.Words.Tables
Imports CMS.Library
Imports Microsoft.Reporting.WebForms
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports Nexus.Library
Imports Nexus.Library.ReportParameterDataSets
Imports Nexus.Utils
Imports LocalReport = Microsoft.Reporting.WebForms.LocalReport
Imports ReportDataSource = Microsoft.Reporting.WebForms.ReportDataSource
Imports ReportParameter = Microsoft.Reporting.WebForms.ReportParameter
Imports ReportParameterInfo = Microsoft.Reporting.WebForms.ReportParameterInfo


Namespace Nexus

	Partial Class secure_InsurerPayments
		Inherits Frontend.clsCMSPage

		Private oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
		Private sFileReportName As String
		Dim oWebService As NexusProvider.ProviderBase
		Dim dTotalMarkedAmount As Double
		Dim oUserDetails As NexusProvider.UserDetails
		Public Const CNFilteredCollection As String = "FilteredCollection"
		Dim oUserAuthority As New NexusProvider.UserAuthority
		Dim oWriteOffInterMediateAccount As NexusProvider.OptionTypeSetting
		Private sReportFolderName As String
		Private sReportPath As String

		''' <summary>
		''' Set the Back office System option.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
			Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
			Dim oOptionSettings As NexusProvider.OptionTypeSetting

			oUserAuthority.UserCode = Session(CNLoginName)
			'set the authority options for reverse allocation
			oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.HasWriteOffAuthority
			'initiate the GetUserAuthority method
			oWebService.GetUserAuthorityValue(oUserAuthority)

			oWriteOffInterMediateAccount = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.WriteOffInterMediateAccount)

			'Check Multi Step Approval system option
			Dim oMultiStepApprovalOption As NexusProvider.OptionTypeSetting
			oMultiStepApprovalOption = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, NexusProvider.ProductOptions.MultiStepApproval)
			If oMultiStepApprovalOption.OptionValue = "" OrElse oMultiStepApprovalOption.OptionValue = "0" Then
				divTransactionsBy.Visible = False
			End If

			'If System Option for "Single Cash List receipt/payment per allocation" is ON then we will not allow with multiple SPR/SPY
			oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.SingleCashListItemPerAllocation)
			'Save this system option in cache. So that it can be used furter
			If oOptionSettings.OptionValue = "" Then
				oOptionSettings.OptionValue = 0
			End If
			ViewState.Add("IsSingleCashListPaymentOrReciept", oOptionSettings.OptionValue.ToString())

			'Page.ClientScript.RegisterStartupScript()
		End Sub

		Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
			Dim aAllocationPeriods As String() = MultiSelectDD1.Text.Split(",")
			Dim sCurrentPeriod As String = "Period" & Date.Today.Month.ToString() & " | " & Date.Today.Year.ToString()
			MultiSelectDD1.Text = sCurrentPeriod
			hypWriteOff.Enabled = False

			If Not IsPostBack Then
				'Cleaning of the session values

				ClearQuote()
				ClearClaims()
				ClearHeader()


				Dim dtMinDate As DateTime = DateTime.MinValue
				rngDateTo.MinimumValue = dtMinDate.AddYears(1752).ToString("dd/MM/yyyy")
				rngDateTo.MaximumValue = Date.MaxValue.ToShortDateString()

				rngDateFrom.MinimumValue = dtMinDate.AddYears(1752).ToString("dd/MM/yyyy")
				rngDateFrom.MaximumValue = Date.MaxValue.ToShortDateString()

				'WPR48
				Session(CNTransInMultiCurr) = "No"
				Session(CNReciptAmountEntered) = "0"
				If rblViewby.SelectedValue.ToString = "TC" Then
					PopulateCurrencyByBranch() ' Fill the currency as per Branch Selection
				End If

				'create a unique key and add this to viewstate
				'this will be used to cache the results of the SAM call
				Dim AccountResultpageCacheID As Guid
				AccountResultpageCacheID = Guid.NewGuid
				ViewState.Add("AccountResultpageCacheID", AccountResultpageCacheID.ToString)

				Dim ManupulatedGridResultpageCacheID As Guid
				ManupulatedGridResultpageCacheID = Guid.NewGuid
				ViewState.Add("ManupulatedGridResultpageCacheID", ManupulatedGridResultpageCacheID.ToString)

				Dim OutStandingGridResultpageCacheID As Guid
				OutStandingGridResultpageCacheID = Guid.NewGuid
				ViewState.Add("OutStandingGridResultpageCacheID", OutStandingGridResultpageCacheID.ToString)

				'To set the Focus
				Page.SetFocus(btnAccountCode)

				'Make all the assosiated branches available for logged in Agent
				FillPaymentGroup()

				txtTotalMarked.Text = "0.00"
				txtTotalWriteOff.Text = "0.00"
				txtReceiptAmount.Text = "0.00"
				txtDateTo.Text = Date.Now.ToShortDateString
				txtDateFrom.Text = Date.Now.ToShortDateString
				'MultiSelectDD1.Text = Date.Now.ToShortDateString

				'We need to retain the searches if user is redirected from cashlistitems controls
				If Request.QueryString("Mode") Is Nothing Then
					Session.Remove(CNDocumentRef)
					Session.Remove(CNInsSearchCriteria)
					oUserDetails = oWebService.GetUserDetails(HttpContext.Current.User.Identity.Name)
					If Not String.IsNullOrEmpty(oUserDetails.PartyCode) AndAlso Not oUserDetails.PartyCode Is Nothing Then
						txtAccountCode.Text = CStr(oUserDetails.PartyCode)
						txtAccountCode.ReadOnly = True
						btnAccountCode.Enabled = False
					End If
				ElseIf Request.QueryString("Mode") = "IP" Then
					Dim oAccountDetails As NexusProvider.AccountDetails = Session(CNInsSearchCriteria)
					Dim oAccountSearchCriteria As New NexusProvider.AccountSearchCriteria
					Dim oAccountSearchResultColl As NexusProvider.AccountSearchResultCollection

					hiddenAccountCode.Value = Session(CNAccountkey)
					If Not Session(CNPartyKey) Is Nothing Then
						hPartyKey.Value = Session(CNPartyKey)
					Else
						hPartyKey.Value = Request.QueryString("PartyKey")
					End If
					txtAccountCode.Text = Session(CNAccountName)

					oAccountSearchCriteria.ShortCode = txtAccountCode.Text.Trim
					oAccountSearchCriteria.IncludeInsurerAgents = True
					oAccountSearchCriteria.ExcludeInsurerAgents = False

					oWebService = New NexusProvider.ProviderManager().Provider
					oAccountSearchResultColl = oWebService.FindAccounts(oAccountSearchCriteria)

					If oAccountSearchResultColl IsNot Nothing AndAlso oAccountSearchResultColl.Count > 0 Then
						rblViewby.Items.FindByValue("AC").Text = GetLocalResourceObject("lbl_AccountCurrency")
						rblViewby.Items.FindByValue("AC").Text &= "(" & oAccountSearchResultColl(0).CurrencyCode & ")"
					End If
					If oAccountDetails IsNot Nothing AndAlso oAccountDetails.DateTo <> Date.MinValue Then
						txtDateTo.Text = oAccountDetails.DateTo
						chkDateTo.Checked = True

						If oAccountDetails.DateByTransaction = NexusProvider.InsurerPaymentsDateByType.EffectiveDate Then
							oAccountDetails.DateByTransaction = NexusProvider.InsurerPaymentsDateByType.EffectiveDate
						Else
							oAccountDetails.DateByTransaction = NexusProvider.InsurerPaymentsDateByType.TransactionDate
						End If
					End If

					If oAccountDetails.InsurerPaymentBranchCode IsNot Nothing AndAlso oAccountDetails.InsurerPaymentBranchCode.Length > 0 Then
						ddlPaymentGroup.SelectedValue = oAccountDetails.InsurerPaymentBranchCode
					End If

					If oAccountDetails.AlternateReference IsNot Nothing AndAlso oAccountDetails.AlternateReference.Length > 0 Then
						txtAlternateRef.Text = oAccountDetails.AlternateReference
					End If

					If oAccountDetails.PolicyNumber IsNot Nothing AndAlso oAccountDetails.PolicyNumber.Length > 0 Then
						'txtPolicyNumber.Text = oAccountDetails.PolicyNumber
					End If

					'WPR48
					If oAccountDetails.CurrencyCode IsNot Nothing AndAlso oAccountDetails.CurrencyCode.Length > 0 Then
						ddlCurrency.SelectedValue = oAccountDetails.CurrencyCode
					End If

					'Preserve Transactions By filter if applicable
					If divTransactionsBy.Visible Then
						If oAccountDetails.OnlyPendingAuth Then
							rblTransactionsBy.SelectedValue = "1"
						Else
							rblTransactionsBy.SelectedValue = "0"
						End If
					End If

					ddlMarkedStatus.SelectedValue = oAccountDetails.MarkedStatus
					ddlMonth.SelectedValue = oAccountDetails.Month
					' refreshes the grid after updating for PartNow 
					GridDataBind()
					PopulateOutstanding()
					PopulateOutstandingTransaction()
				End If

				If Request.QueryString("Query") = "Report" Then
					'Generate the report
					GenerateReport()
				End If

			End If

			If (Session(CNWriteOffAmount) IsNot Nothing) Then
				AddWriteOffAmount()
				Session.Remove(CNWriteOffAmount)
			End If

			If Request("__EVENTARGUMENT") = "WRITEOFF" Then
				Page.ClientScript.GetPostBackEventReference(Me, "")
				RemoveWriteOff()
				hdnWriteOff.Value = ""
			End If

			If Request("__EVENTARGUMENT") = "RefreshGrid" Then
				Page.ClientScript.GetPostBackEventReference(Me, "")
				oWebService = New NexusProvider.ProviderManager().Provider
				' refreshes the grid after updating for PartNow 
				GridDataBind()
				PopulateOutstanding()
				PopulateOutstandingTransaction()

			ElseIf Request("__EVENTARGUMENT") = "RefreshIP" Then
				Page.ClientScript.GetPostBackEventReference(Me, "")
				rblViewby.Items.FindByValue("AC").Text = GetLocalResourceObject("lbl_AccountCurrency")
				rblViewby.Items.FindByValue("AC").Text &= "(" & hCurrencyCode.Value & ")"
				FillPaymentGroup()
				ddlPaymentGroup.Enabled = True
				Me.txtAccountCode.Focus()
			End If

			'WPR48
			PopulatePeriodTable()

		End Sub
		''' <summary>
		''' Make all the assosiated branches available for logged in Agent
		''' </summary>
		''' <remarks></remarks>
		Sub FillPaymentGroup()
			If Session(CNAgentDetails) IsNot Nothing Then
				Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
				If oUserDetails IsNot Nothing Then
					ddlPaymentGroup.DataSource = oUserDetails.ListOfBranches
					ddlPaymentGroup.DataTextField = "Description"
					ddlPaymentGroup.DataValueField = "Code"
					ddlPaymentGroup.DataBind()
					ddlPaymentGroup.Items.Insert(0, New ListItem(GetLocalResourceObject("ddl_PaymentGroup_defaulttext"), String.Empty))
				End If
			End If
		End Sub
		Private Sub SetControlsDefaultValues()

			grdvResultInsurerPayments.DataSource = Nothing
			grdvResultInsurerPayments.DataBind()
			grdvResultInsurerPayments.Visible = False

			grdvOutstandingTransaction.DataSource = Nothing
			grdvOutstandingTransaction.DataBind()
			grdvOutstandingTransaction.Visible = False
			divOutstandingTransDetails.Visible = False

			ddlPaymentGroup.Items(0).Text = String.Empty
			ddlPaymentGroup.SelectedIndex = 0

			txtTotalMarked.Text = "0.00"
			btnAllocate.Enabled = False
			btnPay.Visible = False
			btnDrill.Visible = False
			upInsurerPayment_UI.Update()
		End Sub
		Sub ResetValues()
			SetControlsDefaultValues()
			txtAccountCode.Text = String.Empty
			hiddenAccountCode.Value = String.Empty
			txtDateTo.Text = Date.Now.ToShortDateString
			txtDateTo.Enabled = False
			chkDateTo.Checked = False
			txtAlternateRef.Text = String.Empty
			'txtPolicyNumber.Text = String.Empty           
			ddlMarkedStatus.SelectedValue = 2
			ddlMonth.SelectedValue = 0
			rblDateOption.SelectedValue = 0
			rblViewby.SelectedValue = "TC"
			If divTransactionsBy.Visible Then
				rblTransactionsBy.SelectedValue = "0"
			End If
			txtTotalWriteOff.Text = String.Empty
			rblViewby.Items.FindByValue("AC").Text = GetLocalResourceObject("lbl_AccountCurrency")
			ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "EnableCalenderControl", "EnableCalender(false);", True)
			ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "ResetVal", "ResetValues(false);", True)
		End Sub

		Protected Sub btnFindNow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
			Cache.Remove(ViewState("AccountResultpageCacheID"))
			Cache.Remove(ViewState("ManupulatedGridResultpageCacheID"))
			Cache.Remove(ViewState("OutStandingGridResultpageCacheID"))
			Session("pageindex") = Nothing
			' create and initalising objects
			oWebService = New NexusProvider.ProviderManager().Provider
			Dim oAccountDetails As New NexusProvider.AccountDetails

			' obtaining data from controls if exists
			If Not hiddenAccountCode.Value.Trim().Length = 0 Then
				oAccountDetails.AccountKey = hiddenAccountCode.Value
				Session(CNAccountkey) = hiddenAccountCode.Value 'txtAccountCode.Text.Trim()
				Session(CNPartyKey) = hPartyKey.Value
				Session(CNAccountName) = txtAccountCode.Text.Trim() 'hiddenAccountCode.Value

				If String.IsNullOrEmpty(hCurrencyCode.Value) = False AndAlso hCurrencyCode.Value <> "0" Then
					Session(CNCurrency) = hCurrencyCode.Value.Trim
				End If

			ElseIf hiddenAccountCode.Value.Trim().Length = 0 Or hiddenAccountCode.Value.Trim = "0" Then
				Dim oAccountSearchCriteria As New NexusProvider.AccountSearchCriteria
				Dim oAccountSearchResultColl As NexusProvider.AccountSearchResultCollection

				''   FillPaymentGroup() 'Reset The Payment Group
				If txtAccountCode.Text.Trim.Contains("%") Then
					IsFound.IsValid = False
					Exit Sub
				End If
				oAccountSearchCriteria.ShortCode = txtAccountCode.Text.Trim
				oAccountSearchCriteria.IncludeInsurerAgents = False
				oAccountSearchCriteria.ExcludeInsurerAgents = False
				oAccountSearchResultColl = oWebService.FindAccounts(oAccountSearchCriteria)
				If oAccountSearchResultColl IsNot Nothing Then
					If oAccountSearchResultColl.Count > 0 Then
						hiddenAccountCode.Value = oAccountSearchResultColl(0).AccountKey
						oAccountDetails.AccountKey = hiddenAccountCode.Value
						rblViewby.Items.FindByValue("AC").Text = GetLocalResourceObject("lbl_AccountCurrency")
						rblViewby.Items.FindByValue("AC").Text &= "(" & oAccountSearchResultColl(0).CurrencyCode & ")"
						Session(CNAccountkey) = hiddenAccountCode.Value
						hPartyKey.Value = oAccountSearchResultColl(0).PartyKey
						Session(CNPartyKey) = oAccountSearchResultColl(0).PartyKey
						Session(CNAccountName) = txtAccountCode.Text.Trim()
						Session(CNCurrency) = oAccountSearchResultColl(0).CurrencyCode.Trim
					Else
						IsFound.IsValid = False
						SetControlsDefaultValues()
						Exit Sub
					End If
				Else
					SetControlsDefaultValues()
					IsFound.IsValid = False
					Exit Sub
				End If
			End If
			Session.Remove(CNDocumentRef)
			If chkDateTo.Checked = True Then
				oAccountDetails.DateTo = CType(txtDateTo.Text, Date)

				If Not rblDateOption.SelectedValue.Trim.Length = 0 Then
					If rblDateOption.SelectedValue.Trim = NexusProvider.InsurerPaymentsDateByType.EffectiveDate Then
						oAccountDetails.DateByTransaction = NexusProvider.InsurerPaymentsDateByType.EffectiveDate

					Else
						oAccountDetails.DateByTransaction = NexusProvider.InsurerPaymentsDateByType.TransactionDate

					End If
				End If
			End If

			If Not ddlPaymentGroup.Text.Trim().Length = 0 Then
				oAccountDetails.InsurerPaymentBranchCode = ddlPaymentGroup.SelectedValue.Trim()
			End If

			If Not txtAlternateRef.Text.Trim.Length = 0 Then
				oAccountDetails.AlternateReference = txtAlternateRef.Text.Trim()

			End If



			If Not ddlMarkedStatus.SelectedValue = Nothing Then
				oAccountDetails.MarkedStatus = ddlMarkedStatus.SelectedValue
				oAccountDetails.MarkedStatusSpecified = True
			Else
				oAccountDetails.MarkedStatusSpecified = False
			End If

			If ddlMonth.SelectedValue IsNot Nothing AndAlso ddlMonth.SelectedValue.Trim.Length <> 0 Then
				oAccountDetails.Month = ddlMonth.SelectedValue
			End If

			'WPR48
			If ddlCurrency.SelectedValue IsNot Nothing AndAlso ddlCurrency.SelectedValue.Trim.Length <> 0 Then
				oAccountDetails.CurrencyCode = ddlCurrency.SelectedValue.ToString.Trim
			End If

			If MultiSelectDD1.Text IsNot Nothing AndAlso MultiSelectDD1.Text <> "" Then
				Dim aAllocationPeriods As String() = MultiSelectDD1.Text.Split(",")
				Dim sPeriodName As String = ""
				Dim sYearName As String = ""

				For iCount As Integer = 0 To aAllocationPeriods.Length - 1
					sYearName = aAllocationPeriods(iCount).Substring(aAllocationPeriods(iCount).Length - 4, 4)
					sPeriodName = aAllocationPeriods(iCount).Substring(0, aAllocationPeriods(iCount).Length - 4).Trim()
					If iCount > 0 Then
						oAccountDetails.PeriodName = sPeriodName & "," & oAccountDetails.PeriodName
						oAccountDetails.YearName = sYearName & "," & oAccountDetails.YearName
					Else
						oAccountDetails.PeriodName = sPeriodName
						oAccountDetails.YearName = sYearName
					End If
				Next


			End If

			If chkDatefrom.Checked = True Then
				oAccountDetails.DateFrom = CType(txtDateFrom.Text, Date)
				If Not rblDateOption.SelectedValue.Trim.Length = 0 Then
					If rblDateOption.SelectedValue.Trim = NexusProvider.InsurerPaymentsDateByType.EffectiveDate Then
						oAccountDetails.DateByTransaction = NexusProvider.InsurerPaymentsDateByType.EffectiveDate
					Else
						oAccountDetails.DateByTransaction = NexusProvider.InsurerPaymentsDateByType.TransactionDate
					End If
				End If
			End If
			If Not txtReference.Text.Trim.Length = 0 Then
				oAccountDetails.Reference = txtReference.Text.Trim()
			End If
			If Not ddlMediaType.Text.Trim().Length = 0 Then
				oAccountDetails.MediaType = ddlPaymentGroup.SelectedValue.Trim()
			End If

			If ddlMediaType.Text.Trim() <> "" Then
				oAccountDetails.MediaType = ddlMediaType.Items.FindItemByDescription(ddlMediaType.Text.Trim()).Key
			End If

			If Not rblCommission.SelectedValue.Trim.Length = 0 Then

				If rblCommission.SelectedValue = "1" Then
					oAccountDetails.GrossAgent = True
				Else
					oAccountDetails.GrossAgent = False
				End If

			End If

			'Set pending authorization filter
			If divTransactionsBy.Visible Then
				If rblTransactionsBy.SelectedValue = "0" Then
					oAccountDetails.ExcludePendingAuth = True
				Else
					oAccountDetails.OnlyPendingAuth = True
				End If
			End If

			'Transaction Currency or Account Currency need to be implemented.

			' storing search criteria in session
			Session(CNInsSearchCriteria) = oAccountDetails

			' populating Transaction grid
			GridDataBind()

			' Populating Outstanding transaction grid
			PopulateOutstandingTransaction()

		End Sub

		Protected Sub GridDataBind()

			' create and initalising objects
			oWebService = New NexusProvider.ProviderManager().Provider
			Dim oGridDataCollection As NexusProvider.AccountDetailsCollection
			Dim dTotalMarkedAmount As Double
			Dim dTotalWriteOff As Double = 0
			' obtaining data from SAM
			oGridDataCollection = oWebService.GetInsurerPayments(CType(Session(CNInsSearchCriteria), NexusProvider.AccountDetails))

			'ViewState.Add(CNFilteredCollection, oGridDataCollection)
			'put the data in cache
			Cache.Insert(ViewState("AccountResultpageCacheID"), oGridDataCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

			'if allocation period selected then auto mark all trasactions
			If Not String.IsNullOrEmpty(CType(Session(CNInsSearchCriteria), NexusProvider.AccountDetails).PeriodName) Then
				ddlMarkedStatus.SelectedValue = NexusProvider.InsurerPaymentsMarkedStatus.Yes
				ddlMarkedStatus.Enabled = False
				CType(Session(CNInsSearchCriteria), NexusProvider.AccountDetails).MarkedStatus = NexusProvider.InsurerPaymentsMarkedStatus.Yes
				MarkTransactions(oGridDataCollection)
			Else
				ddlMarkedStatus.Enabled = True
			End If

			'if selected user has some transactions, enable "Allocate" button else disable it again
			If oGridDataCollection IsNot Nothing AndAlso Not oGridDataCollection.Count = 0 Then
				btnAllocate.Enabled = True
			Else
				btnAllocate.Enabled = False
			End If

			' obtains the collection of all the marked transactions and stores in 
			' session for later use.
			CollectAllMarkedTransaction(oGridDataCollection)


			Dim iMainGridCount As Integer = oGridDataCollection.NumberOfRows()
			Dim oResultsInsurerPayments As New NexusProvider.AccountDetailsCollection
			Dim iMainLoopCount, iSumLoopCount As Integer
			Dim dPaidAmount, dCurrencyAmount, dMarkedAmount, dAccountAmount, dTotalMarked As Double

			' looping through rows in grid and adding CurrencyAmount, PaidAmount and MarkedAmount
			For iMainLoopCount = 0 To iMainGridCount - 1
				'Check that if already Document is attached or not
				Dim bMatched As Boolean = False
				For iCount As Integer = 0 To oResultsInsurerPayments.Count - 1
					If oResultsInsurerPayments(iCount).DocumentRef.Trim.ToUpper = oGridDataCollection.Item(iMainLoopCount).DocumentRef.Trim.ToUpper Then
						bMatched = True
						Exit For
					End If
				Next

				If bMatched = False Then
					Dim oAccountDetails As New NexusProvider.AccountDetails
					oAccountDetails = oGridDataCollection.Item(iMainLoopCount)
					oResultsInsurerPayments.Add(oAccountDetails)
				End If
			Next

			Dim oFilterResultsInsurer As New NexusProvider.AccountDetailsCollection

			If ddlTransactionType.SelectedValue <> 0 Then
				Dim sTransTp As String
				For iCount As Integer = 0 To oResultsInsurerPayments.Count - 1
					sTransTp = oResultsInsurerPayments.Item(iCount).DocumentRef.Substring(0, 3)
					If ddlTransactionType.SelectedValue = 1 Then
						If sTransTp = "CLR" OrElse sTransTp = "CLP" Then
							Dim oAccountDetails As New NexusProvider.AccountDetails
							oAccountDetails = oResultsInsurerPayments.Item(iCount)
							oFilterResultsInsurer.Add(oAccountDetails)
						End If
					End If

					If ddlTransactionType.SelectedValue = 2 Then
						If sTransTp <> "CLR" OrElse sTransTp <> "CLP" Then
							Dim oAccountDetails As New NexusProvider.AccountDetails
							oAccountDetails = oResultsInsurerPayments.Item(iCount)
							oFilterResultsInsurer.Add(oAccountDetails)
						End If
					End If

				Next
				oResultsInsurerPayments = oFilterResultsInsurer
			End If

			For iCount As Integer = 0 To oResultsInsurerPayments.Count - 1
				dCurrencyAmount = 0
				dPaidAmount = 0
				dMarkedAmount = 0
				dAccountAmount = 0
				For iSumLoopCount = 0 To oGridDataCollection.Count - 1
					If Not oResultsInsurerPayments.Item(iCount).DocumentRef = Nothing AndAlso Not oResultsInsurerPayments.Item(iCount).DocumentRef.Trim.Trim.Length = 0 Then
						If oResultsInsurerPayments.Item(iCount).DocumentRef = oGridDataCollection.Item(iSumLoopCount).DocumentRef Then
							If rblViewby.Items.FindByValue("TC").Selected = True Then
								dAccountAmount += oGridDataCollection.Item(iSumLoopCount).CurrencyAmount
								dPaidAmount += oGridDataCollection.Item(iSumLoopCount).PaidAmount
								dMarkedAmount += oGridDataCollection.Item(iSumLoopCount).MarkedAmount
							ElseIf rblViewby.Items.FindByValue("AC").Selected = True Then
								dAccountAmount += oGridDataCollection.Item(iSumLoopCount).AccountAmount
								dPaidAmount += oGridDataCollection.Item(iSumLoopCount).PaidAccountAmount
								dMarkedAmount += oGridDataCollection.Item(iSumLoopCount).MarkedAccountAmount
							End If
							End If
						End If
                Next
				If dMarkedAmount = dPaidAmount Then
					dMarkedAmount = 0
				End If
				If dMarkedAmount <> 0 Then
					oResultsInsurerPayments.Item(iCount).IsSelected = True
				Else
					oResultsInsurerPayments.Item(iCount).IsSelected = False
				End If
				' assigning the summation of CurrencyAmount, PaidAmount, MarkedAmount to respective feilds
				If rblViewby.Items.FindByValue("TC").Selected = True Then

					oResultsInsurerPayments.Item(iCount).TotalPaidAmount = dPaidAmount + oResultsInsurerPayments.Item(iCount).FullyPaidAmount
					oResultsInsurerPayments.Item(iCount).TotalAmount = dAccountAmount - (oResultsInsurerPayments.Item(iCount).FullyPaidAmount * -1)
					oResultsInsurerPayments.Item(iCount).TotalOutstandingAmount = oResultsInsurerPayments.Item(iCount).TotalAmount - oResultsInsurerPayments.Item(iCount).TotalPaidAmount
					oResultsInsurerPayments.Item(iCount).TotalClientOutstandingAmount = oResultsInsurerPayments.Item(iCount).ClientOutstanding
				ElseIf rblViewby.Items.FindByValue("AC").Selected = True Then

					oResultsInsurerPayments.Item(iCount).TotalPaidAmount = dPaidAmount + oResultsInsurerPayments.Item(iCount).FullyPaidAccountAmount
					oResultsInsurerPayments.Item(iCount).TotalAmount = dAccountAmount - (oResultsInsurerPayments.Item(iCount).FullyPaidAccountAmount * -1)
					oResultsInsurerPayments.Item(iCount).TotalOutstandingAmount = oResultsInsurerPayments.Item(iCount).TotalAmount - oResultsInsurerPayments.Item(iCount).TotalPaidAmount
					oResultsInsurerPayments.Item(iCount).TotalClientOutstandingAmount = oResultsInsurerPayments.Item(iCount).ClientOutstandingAccountAmount
				End If
				oResultsInsurerPayments.Item(iCount).TotalMarkedAmount = dMarkedAmount
				dTotalMarkedAmount = Math.Round(dTotalMarkedAmount + dMarkedAmount, 2)
				If dMarkedAmount <> 0 Then
					dTotalMarked += 1
				End If
			Next

			'Make Show/Hide the Pay button
			For iCount As Integer = 0 To oResultsInsurerPayments.Count - 1
				If oResultsInsurerPayments.Item(iCount).MarkedAmount <> 0 _
				And oResultsInsurerPayments.Item(iCount).IsSelected = True Then
					'Disable Pay button if Only Pending for Authorisation is selected
					If divTransactionsBy.Visible Then
						btnPay.Visible = (rblTransactionsBy.SelectedValue = "0")
						hypWriteOff.Enabled = (rblTransactionsBy.SelectedValue = "0")
					End If

					Exit For
   				 Else
       				 btnPay.Visible = False
    		End If
		Next

			'WPR48
			Session(CNTransInMultiCurr) = "No"
			For iCount As Integer = 1 To oResultsInsurerPayments.Count - 1
				If oResultsInsurerPayments.Item(iCount).IsSelected = True Then
					If oResultsInsurerPayments.Item(iCount - 1).CurrencyCode.ToString.Trim <> oResultsInsurerPayments.Item(iCount).CurrencyCode.ToString.Trim Then
						Session(CNTransInMultiCurr) = "Yes"
						Exit For
					End If
				End If
			Next

			'put the data into cache
			Cache.Insert(ViewState("ManupulatedGridResultpageCacheID"), oResultsInsurerPayments, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

			For icount As Integer = 0 To oGridDataCollection.Count - 1
				If (oGridDataCollection.Item(icount).Spare = "WRITEOFF") Then
					dTotalWriteOff += oGridDataCollection.Item(icount).AccountAmount
				End If
			Next
			'Session(CNManupulatedGridResult) = oResultsInsurerPayments

			ViewState.Add(CNFilteredCollection, oResultsInsurerPayments)

			'enabling and disabling grid
			If oResultsInsurerPayments IsNot Nothing AndAlso Not oResultsInsurerPayments.NumberOfRows = 0 Then
				If rblViewby.Items.FindByValue("TC").Selected = True Then
					txtTotalWriteOff.Text = dTotalWriteOff
					If txtReceiptAmount.Text <> "" AndAlso Convert.ToDecimal(txtReceiptAmount.Text) <> 0 AndAlso (txtReceiptAmount.Text) <> (txtTotalMarked.Text) Then
						Session(CNReciptAmountEntered) = "1"
					Else
						txtReceiptAmount.Text = dTotalMarkedAmount
					End If
					If Session(CNReciptAmountEntered) = "0" Then
						txtReceiptAmount.Text = dTotalMarkedAmount
					End If
					txtTotalMarked.Text = dTotalMarkedAmount
					RefreshUnallocatedAmount()
					'txtTotalMarked.Text = New Money(txtTotalMarked.Text.Trim, oResultsInsurerPayments(0).CurrencyCode).Formatted
				ElseIf rblViewby.Items.FindByValue("AC").Selected = True Then
					txtTotalWriteOff.Text = dTotalWriteOff
					If txtReceiptAmount.Text <> "" AndAlso Convert.ToDecimal(txtReceiptAmount.Text) <> 0 AndAlso (txtReceiptAmount.Text) <> (txtTotalMarked.Text) Then
						Session(CNReciptAmountEntered) = "1"
					Else
						txtReceiptAmount.Text = dTotalMarkedAmount
					End If
					If Session(CNReciptAmountEntered) = "0" Then
						txtReceiptAmount.Text = dTotalMarkedAmount
					End If
					txtTotalMarked.Text = dTotalMarkedAmount
					RefreshUnallocatedAmount()
					'txtTotalMarked.Text = New Money(txtTotalMarked.Text.Trim, oResultsInsurerPayments(0).AccountCurrencyCode).Formatted
				Else
					Session(CNTotalAmount) = dTotalMarkedAmount + Convert.ToDecimal(txtUnallocatedAmount.Text)
				End If

				' btn Receipt for Debit transactions and Pay for credit
				If txtTotalMarked.Text > "0" And (txtReceiptAmount.Text <> "0" OrElse txtReceiptAmount.Text <> "") Then
					If txtTotalMarked.Text = 0 AndAlso txtReceiptAmount.Text = "0" Then
						btnPay.Visible = False
					Else
						btnPay.Text = "Receipt"
						btnPay.Visible = True
					End If

				ElseIf txtTotalMarked.Text < "0" Then
					'Disable Pay button if Only Pending for Authorisation is selected
					If divTransactionsBy.Visible AndAlso rblTransactionsBy.SelectedValue = "1" Then
						btnPay.Visible = False
					Else
						btnPay.Text = "PAY"
						btnPay.Visible = True
					End If
				Else
					btnPay.Visible = False
				End If

				'take the total marked amount(without formatting) in hidden text box to validate
				hiddentxtTotalMarked.Value = dTotalMarkedAmount.ToString("F2")
				Session(CNMarkedAmountSignForCashList) = dTotalMarkedAmount
				'Session(CNTotalAmount) = dTotalMarkedAmount + Convert.ToDecimal(txtUnallocatedAmount.Text)
				Session(CNTotalWriteOffAmount) = dTotalWriteOff

				If Not btnDrill.Visible Then
					btnDrill.Visible = True
				End If
				'if client has given any default value make it default
				ddlPaymentGroup.Enabled = True
			Else
				txtTotalMarked.Text = "0.00"
				btnDrill.Visible = False
			End If

			If dTotalMarkedAmount = 0 And dTotalMarked <> 0 Then
				btnAllocate.Enabled = True
			Else
				btnAllocate.Enabled = False
			End If
			If (hdnLedgerCode.Value.Trim.ToUpper <> "AG") Then
				Dim sValue As String = hdnLedgerCode.Value.Trim.ToUpper
				ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "RemoveComission('" & sValue & "');", True)
			End If

			'Bind The Grid
			If (Session("pageindex") IsNot Nothing) Then
				grdvResultInsurerPayments.PageIndex = Convert.ToInt64(Session("pageindex"))
			End If
			grdvResultInsurerPayments.DataSource = oResultsInsurerPayments
			If (oNexusConfig.FinanceGridSize) = "" Then
				grdvResultInsurerPayments.PageSize = 10
			Else
				grdvResultInsurerPayments.PageSize = CStr(oNexusConfig.FinanceGridSize)
			End If
		    If ViewState("SortExpression") IsNot Nothing Then
				Dim _sortDirection As New SortDirection
				If ViewState("SortDirection") IsNot Nothing Then
					_sortDirection = ViewState("SortDirection")

				End If
				oResultsInsurerPayments.SortColumn = ViewState("SortExpression")
				oResultsInsurerPayments.SortingOrder = _sortDirection
				oResultsInsurerPayments.Sort()
			End If
			grdvResultInsurerPayments.DataBind()
			grdvResultInsurerPayments.Visible = True
			upUpdateAmount.Update()
			upInsurerPayment_UI.Update()
		End Sub

		Protected Sub grdvResultInsurerPayments_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvResultInsurerPayments.PageIndexChanging

			grdvResultInsurerPayments.PageIndex = e.NewPageIndex
			Session("pageindex") = grdvResultInsurerPayments.PageIndex
			If (oNexusConfig.FinanceGridSize) = "" Then
				grdvResultInsurerPayments.PageSize = 10
			Else
				grdvResultInsurerPayments.PageSize = CStr(oNexusConfig.FinanceGridSize)
			End If
			If (Session("pageindex") IsNot Nothing) Then
				grdvResultInsurerPayments.PageIndex = Convert.ToInt64(Session("pageindex"))
			End If

			grdvResultInsurerPayments.DataSource = CType(Cache.Item(ViewState("ManupulatedGridResultpageCacheID")), NexusProvider.AccountDetailsCollection)
			grdvResultInsurerPayments.DataBind()

		End Sub

		Protected Sub grdvResultInsurerPayments_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdvResultInsurerPayments.RowCommand
			If e.CommandName = "select" Then
				' Dim test As GridViewRow
				Dim lnkbutton As LinkButton = e.CommandSource

				' obtains the document reference if rowcommand is select
				If lnkbutton.Text.ToUpper.Contains("SELECT") Then
					btnDrill.Enabled = True
					If Not String.IsNullOrEmpty(e.CommandArgument) Then
						Session(CNDocumentRef) = Nothing
						Session(CNDocumentRef) = e.CommandArgument
						Cache.Remove(ViewState("AccountResultpageCacheID"))
						Cache.Remove(ViewState("OutStandingGridResultpageCacheID"))
						PopulateOutstanding()
						PopulateOutstandingTransaction()
					End If
				End If
			End If

		End Sub

		Protected Sub grdvResultInsurerPayments_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvResultInsurerPayments.RowCreated
			If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
				e.Row.Cells(13).Visible = False 'Hide the "HidTransID" column
			End If
		End Sub

		Protected Sub grdvResultInsurerPayments_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvResultInsurerPayments.RowDataBound
			If e.Row.RowType = DataControlRowType.DataRow Then
				'NOTE - this will need to be changed to give each row a unique id
				'this needs to be matched in markup for the menu (id="Menu_<%# Eval("DocumentKey") %>")
				e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.AccountDetails).DocumentKey)

				Dim lnkbtnSelect As LinkButton = e.Row.FindControl("lnkbutSelect")
				Dim oItem As NexusProvider.AccountDetails = CType(e.Row.DataItem, NexusProvider.AccountDetails)
				Dim sDocumentRef As String = oItem.DocumentRef.Trim()

				lnkbtnSelect.CommandArgument = CType(e.Row.DataItem, NexusProvider.AccountDetails).DocumentRef.Trim
				lnkbtnSelect.CommandName = "select"

				Dim lnkbutQuery As LinkButton = e.Row.FindControl("lnkbutQuery")

				If HttpContext.Current.Session.IsCookieless Then

					lnkbutQuery.OnClientClick = "tb_show(null ,'../Modal/WrmTask.aspx?mode=add&modal=true&FromPage=WM&KeepThis=true&TB_iframe=true&height=500&width=750&onQueryClick=true&DocumentRef=" + sDocumentRef + "&AccountCode=" + txtAccountCode.Text + "&CallingApp=" + "InsurerPayment" + "' , null);return false;"

				Else

					lnkbutQuery.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "Modal/WrmTask.aspx?mode=add&modal=true&FromPage=WM&KeepThis=true&TB_iframe=true&height=500&width=750&onQueryClick=true&DocumentRef=" + sDocumentRef + "&AccountCode=" + txtAccountCode.Text + "&CallingApp=" + "InsurerPayment" + "' , null);return false;"

				End If

				Dim chkMarker As CheckBox = e.Row.Cells(15).FindControl("chkMarked")

				'Marked Record Need to be Checked
				If CType(e.Row.DataItem, NexusProvider.AccountDetails).IsSelected = True Then
					chkMarker.Checked = True
				Else
					chkMarker.Checked = False
				End If

				' iOutstandingAmount = iAccountNumber - iPaidAmount

				Dim hidTrans As HiddenField = e.Row.FindControl("HidTransID")
				hidTrans.Value = oItem.TransdetailId

				Dim TempGridRow As GridViewRow = e.Row
				If rblViewby.Items.FindByValue("TC").Selected = True Then
					TempGridRow.Cells(8).Text = New Money(oItem.TotalAmount, oItem.CurrencyCode.Trim).Formatted
					TempGridRow.Cells(9).Text = New Money(oItem.TotalPaidAmount, oItem.CurrencyCode.Trim).Formatted
					TempGridRow.Cells(10).Text = New Money(oItem.TotalOutstandingAmount, oItem.CurrencyCode.Trim).Formatted
					TempGridRow.Cells(11).Text = New Money(oItem.TotalMarkedAmount, oItem.CurrencyCode.Trim).Formatted
					TempGridRow.Cells(12).Text = New Money(oItem.TotalClientOutstandingAmount, oItem.CurrencyCode.Trim).Formatted
				ElseIf rblViewby.Items.FindByValue("AC").Selected = True Then
					TempGridRow.Cells(8).Text = New Money(oItem.TotalAmount, oItem.AccountCurrencyCode.Trim).Formatted
					TempGridRow.Cells(9).Text = New Money(oItem.TotalPaidAmount, oItem.AccountCurrencyCode.Trim).Formatted
					TempGridRow.Cells(10).Text = New Money(oItem.TotalOutstandingAmount, oItem.AccountCurrencyCode.Trim).Formatted
					TempGridRow.Cells(11).Text = New Money(oItem.TotalMarkedAmount, oItem.AccountCurrencyCode.Trim).Formatted
					TempGridRow.Cells(12).Text = New Money(oItem.TotalClientOutstandingAmount, oItem.AccountCurrencyCode.Trim).Formatted
				End If

				'if effective date is invalid
				If CType(e.Row.DataItem, NexusProvider.AccountDetails).EffectiveDate = Date.MinValue Or
			  CType(e.Row.DataItem, NexusProvider.AccountDetails).EffectiveDate.ToShortDateString = "01/01/0001" Or
			   CType(e.Row.DataItem, NexusProvider.AccountDetails).EffectiveDate < "01/01/1900" Then
					e.Row.Cells(5).Text = String.Empty
				End If

				'if transaction date date is invalid
				If CType(e.Row.DataItem, NexusProvider.AccountDetails).AccountingDate = Date.MinValue Or
			 CType(e.Row.DataItem, NexusProvider.AccountDetails).AccountingDate.ToShortDateString = "01/01/0001" Or
			  CType(e.Row.DataItem, NexusProvider.AccountDetails).AccountingDate < "01/01/1900" Then
					e.Row.Cells(6).Text = String.Empty
				End If

				'if dur date is invalid
				If CType(e.Row.DataItem, NexusProvider.AccountDetails).DueDate = Date.MinValue Or
		   CType(e.Row.DataItem, NexusProvider.AccountDetails).DueDate = "01/01/0001" Or
			CType(e.Row.DataItem, NexusProvider.AccountDetails).DueDate < "01/01/1900" Then
					e.Row.Cells(7).Text = String.Empty
				End If

			ElseIf e.Row.RowType = DataControlRowType.Header Then
				Dim chkIPselectall As CheckBox = e.Row.FindControl("chkSelectAll")
				Dim oAccountDetailsCollection As NexusProvider.AccountDetailsCollection
				Dim bFound As Boolean = False

				oAccountDetailsCollection = CType(Cache.Item(ViewState("ManupulatedGridResultpageCacheID")), NexusProvider.AccountDetailsCollection)

				If Not oAccountDetailsCollection Is Nothing Then
					For i As Integer = 0 To oAccountDetailsCollection.Count - 1
						If oAccountDetailsCollection(i).TotalMarkedAmount = 0 AndAlso oAccountDetailsCollection(i).TotalOutstandingAmount <> 0 Then
							bFound = True
							Exit For
						End If
					Next
				End If
				If bFound = False Then
					chkIPselectall.Checked = True
				Else
					chkIPselectall.Checked = False
				End If
			End If
		End Sub

		Sub PopulateOutstanding()
			' create and initalising objects
			Dim oOutstandingTransaction As New NexusProvider.AccountDetailsCollection
			Dim iForCounter As Integer
			Dim sDocumentRef As String = Nothing
			oWebService = New NexusProvider.ProviderManager().Provider
			Dim oGridDataCollection As NexusProvider.AccountDetailsCollection

			'try to get the search results from the cache
			oGridDataCollection =
				CType(Cache.Item(ViewState("AccountResultpageCacheID")), NexusProvider.AccountDetailsCollection)

			If oGridDataCollection Is Nothing Then
				oGridDataCollection = oWebService.GetInsurerPayments(CType(Session(CNInsSearchCriteria), NexusProvider.AccountDetails))
				Cache.Insert(ViewState("AccountResultpageCacheID"), oGridDataCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
			End If

			'obtaining data from SAM
			'oGridDataCollection = oWebService.GetInsurerPayments(CType(Session(CNInsSearchCriteria), NexusProvider.AccountDetails))

			If Session(CNDocumentRef) IsNot Nothing Then
				sDocumentRef = Session(CNDocumentRef)
			Else
				If oGridDataCollection IsNot Nothing AndAlso Not oGridDataCollection.NumberOfRows = 0 Then
					sDocumentRef = oGridDataCollection.Item(0).DocumentRef.Trim()
				End If
			End If

			'adding the records related to the DocumentRef in the Outstanding Transaction grid
			If sDocumentRef IsNot Nothing AndAlso Not sDocumentRef.Trim = "" Then
				For iForCounter = 0 To oGridDataCollection.Count - 1
					If oGridDataCollection.Item(iForCounter).DocumentRef.Trim().ToUpper = sDocumentRef.Trim().ToUpper Then
						oOutstandingTransaction.Add(oGridDataCollection.Item(iForCounter))
					End If
				Next
			End If
			'Check for Marked record
			For iCount As Integer = 0 To oOutstandingTransaction.Count - 1
				If oOutstandingTransaction(iCount).MarkedAmount <> 0 Then
					oOutstandingTransaction(iCount).IsSelected = True
				Else
					oOutstandingTransaction(iCount).IsSelected = False
				End If

			Next

			For iCount As Integer = 0 To oOutstandingTransaction.Count - 1
				If oOutstandingTransaction(iCount).MarkedAmount <> 0 Then
					oOutstandingTransaction(iCount).IsSelected = True
				Else
					oOutstandingTransaction(iCount).IsSelected = False
				End If


			Next

			Cache.Insert(ViewState("OutStandingGridResultpageCacheID"), oOutstandingTransaction, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
			' Session(CNOutStandingGridResult) = oOutstandingTransaction
		End Sub

		Protected Sub PopulateOutstandingTransaction()

			' create and initalising objects
			Dim oOutstandingTransaction As New NexusProvider.AccountDetailsCollection
			Dim iForCounter As Integer
			Dim sDocumentRef As String = Nothing
			oWebService = New NexusProvider.ProviderManager().Provider
			Dim oGridDataCollection As NexusProvider.AccountDetailsCollection
			Dim oAccountDetailsCollection As NexusProvider.AccountDetailsCollection

			'try to get the search results from the cache
			oGridDataCollection =
				CType(Cache.Item(ViewState("AccountResultpageCacheID")), NexusProvider.AccountDetailsCollection)

			oAccountDetailsCollection = CType(Cache.Item(ViewState("ManupulatedGridResultpageCacheID")), NexusProvider.AccountDetailsCollection)

			If oGridDataCollection Is Nothing Then
				oGridDataCollection = oWebService.GetInsurerPayments(CType(Session(CNInsSearchCriteria), NexusProvider.AccountDetails))
				Cache.Insert(ViewState("AccountResultpageCacheID"), oGridDataCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
			End If

			If Session(CNDocumentRef) IsNot Nothing Then
				sDocumentRef = Session(CNDocumentRef)
			Else
				If oGridDataCollection IsNot Nothing AndAlso Not oGridDataCollection.NumberOfRows = 0 Then
					sDocumentRef = oGridDataCollection.Item(0).DocumentRef.Trim()
					Session(CNDocumentRef) = sDocumentRef
				End If
			End If

			'adding the records related to the DocumentRef in the Outstanding Transaction grid
			If oAccountDetailsCollection IsNot Nothing AndAlso oAccountDetailsCollection.Count > 0 Then
				If sDocumentRef IsNot Nothing AndAlso Not sDocumentRef.Trim = "" Then
					For iForCounter = 0 To oGridDataCollection.Count - 1
						If oGridDataCollection.Item(iForCounter).DocumentRef.Trim().ToUpper = sDocumentRef.Trim().ToUpper Then
							oOutstandingTransaction.Add(oGridDataCollection.Item(iForCounter))
						End If
					Next
				End If
			End If
			'Check for Marked record
			For iCount As Integer = 0 To oOutstandingTransaction.Count - 1
				If oOutstandingTransaction(iCount).MarkedAmount <> 0 Then
					oOutstandingTransaction(iCount).IsSelected = True
				Else
					oOutstandingTransaction(iCount).IsSelected = False
				End If
			Next

			'put the data into cache
			Cache.Insert(ViewState("OutStandingGridResultpageCacheID"), oOutstandingTransaction, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))


			grdvOutstandingTransaction.DataSource = oOutstandingTransaction
			grdvOutstandingTransaction.DataBind()
			grdvOutstandingTransaction.Visible = True
			divOutstandingTransDetails.Visible = True

			For iCount As Integer = 0 To oOutstandingTransaction.Count - 1
				If oOutstandingTransaction(iCount).Spare = "WRITEOFF" Then
					hypWriteOff.Enabled = False
				End If
			Next

		End Sub

		Protected Sub grdvOutstandingTransaction_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvOutstandingTransaction.PageIndexChanging

			grdvOutstandingTransaction.PageIndex = e.NewPageIndex
			grdvOutstandingTransaction.DataSource = CType(Cache.Item(ViewState("OutStandingGridResultpageCacheID")), NexusProvider.AccountDetailsCollection)
			grdvOutstandingTransaction.DataBind()

		End Sub

		Protected Sub grdvOutstandingTransaction_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvOutstandingTransaction.RowCreated
			If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
				e.Row.Cells(9).Visible = False 'Hide the "HidTransID" column
			End If
		End Sub

		Protected Sub grdvOutstandingTransaction_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvOutstandingTransaction.RowDataBound

			If e.Row.RowType = DataControlRowType.DataRow Then
				'NOTE - this will need to be changed to give each row a unique id
				'this needs to be matched in markup for the menu (id="Menu_<%# Eval("TransdetailId") %>")
				e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.AccountDetails).TransdetailId)

				Dim chkMarker As CheckBox = e.Row.FindControl("chkMarkedOutTran")
				Dim oItem As NexusProvider.AccountDetails = CType(e.Row.DataItem, NexusProvider.AccountDetails)
				Dim hypPartPay As LinkButton = e.Row.Cells(11).FindControl("hypPartypay")

				'Marked Record Need to be Checked
				If CType(e.Row.DataItem, NexusProvider.AccountDetails).IsSelected = True Then
					chkMarker.Checked = True
				Else
					chkMarker.Checked = False
				End If
				'has writeoff authority
				If (CType(e.Row.DataItem, NexusProvider.AccountDetails).IsSelected AndAlso oUserAuthority.UserAuthorityValue = 1 AndAlso oWriteOffInterMediateAccount.OptionValue <> "") Then
					hypWriteOff.Enabled = True
					hypWriteOff.OnClientClick = "tb_show(null ,'../Modal/WriteOffPayment.aspx?modal=true&KeepThis=true&TB_iframe=true&height=100&width=150 ' , null);return false;"
				End If

				If (oItem.Spare = "WRITEOFF") Then
					hypPartPay.Enabled = False
				End If
				If divTransactionsBy.Visible Then
					hypPartPay.Enabled = (rblTransactionsBy.SelectedValue = "0")
					hypWriteOff.Enabled = (rblTransactionsBy.SelectedValue = "0")
				End If
				'iOutstandingAmount = iAccountNumber - iPaidAmount

				Dim hidTrans As HiddenField = e.Row.FindControl("HidTransID")
				hidTrans.Value = oItem.TransdetailId

				Dim TempGridRow As GridViewRow = e.Row
				If rblViewby.Items.FindByValue("TC").Selected = True Then
					If HttpContext.Current.Session.IsCookieless Then
						hypPartPay.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/modal/Payment.aspx?TK=" + oItem.TransdetailId.ToString() + "&CC=" + oItem.CurrencyCode.Trim() + "&PAY=" + (oItem.CurrencyAmount - oItem.PaidAmount).ToString() + "&Mark=" + chkMarker.Checked.ToString + "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=650' , null);return false;"
					Else
						hypPartPay.OnClientClick = "tb_show(null , '../modal/Payment.aspx?TK=" + oItem.TransdetailId.ToString() + "&CC=" + oItem.CurrencyCode.Trim() + "&PAY=" + (oItem.CurrencyAmount - oItem.PaidAmount).ToString() + "&Mark=" + chkMarker.Checked.ToString + "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=650' , null);return false;"
					End If
					TempGridRow.Cells(3).Text = New Money(oItem.CurrencyAmount, oItem.CurrencyCode.Trim).Formatted
					TempGridRow.Cells(4).Text = New Money(oItem.PaidAmount, oItem.CurrencyCode.Trim).Formatted
					TempGridRow.Cells(5).Text = New Money((oItem.CurrencyAmount - oItem.PaidAmount), oItem.CurrencyCode.Trim).Formatted
					TempGridRow.Cells(6).Text = New Money(oItem.MarkedAmount, oItem.CurrencyCode.Trim).Formatted
				ElseIf rblViewby.Items.FindByValue("AC").Selected = True Then
					If HttpContext.Current.Session.IsCookieless Then
						hypPartPay.OnClientClick = "tb_show(null ," & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/modal/Payment.aspx?TK=" + oItem.TransdetailId.ToString() + "&CC=" + oItem.AccountCurrencyCode.Trim() + "&PAY=" + (oItem.AccountAmount - oItem.PaidAccountAmount).ToString() + "&Mark=" + chkMarker.Checked.ToString + "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=650' , null);return false;"
					Else
						hypPartPay.OnClientClick = "tb_show(null , '../modal/Payment.aspx?TK=" + oItem.TransdetailId.ToString() + "&CC=" + oItem.AccountCurrencyCode.Trim() + "&PAY=" + (oItem.AccountAmount - oItem.PaidAccountAmount).ToString() + "&Mark=" + chkMarker.Checked.ToString + "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=650' , null);return false;"
					End If
					TempGridRow.Cells(3).Text = New Money(oItem.AccountAmount, oItem.AccountCurrencyCode.Trim).Formatted
					TempGridRow.Cells(4).Text = New Money(oItem.PaidAccountAmount, oItem.AccountCurrencyCode.Trim).Formatted
					TempGridRow.Cells(5).Text = New Money((oItem.AccountAmount - oItem.PaidAccountAmount), oItem.AccountCurrencyCode.Trim).Formatted
					TempGridRow.Cells(6).Text = New Money(oItem.MarkedAccountAmount, oItem.AccountCurrencyCode.Trim).Formatted
				End If
			ElseIf e.Row.RowType = DataControlRowType.Header Then
				Dim chkOTselectall As CheckBox = e.Row.FindControl("chkOTSelectAll")
				Dim oAccountDetailsCollection As NexusProvider.AccountDetailsCollection
				Dim bFound As Boolean = False
				Dim sDocumentRef As String = Session(CNDocumentRef)

				oAccountDetailsCollection = CType(Cache.Item(ViewState("OutStandingGridResultpageCacheID")), NexusProvider.AccountDetailsCollection)
				'Session(CNSearchAccountResult)

				For i As Integer = 0 To oAccountDetailsCollection.Count - 1
					If oAccountDetailsCollection(i).MarkedAmount = 0 Then
						bFound = True
						Exit For
					End If
				Next

				If bFound = False Then
					chkOTselectall.Checked = True
				Else
					chkOTselectall.Checked = False
				End If
			End If
		End Sub

		Protected Sub chkMarked_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles chkMarked.CheckedChanged  'chkselectedTransaction.CheckedChanged

			Dim chkTempCheckBox As CheckBox = sender
			Dim oAccountDetailCollection As NexusProvider.AccountDetailsCollection = CType(Cache.Item(ViewState("AccountResultpageCacheID")), NexusProvider.AccountDetailsCollection)
			Dim oOutstandingAccountDetails As NexusProvider.AccountDetailsCollection
			Dim oMarkUnmark As NexusProvider.MarkUnmarkTransaction
			Dim gvTempGridView As GridViewRow = CType(chkTempCheckBox.NamingContainer, GridViewRow)
			Dim sDocumentRef = gvTempGridView.Cells(3).Text.Trim()
			Dim currencyID As Integer = 0
			Dim preSelectedCurrencyID As Integer = -1
			Dim bFoundSelectedCurrency As Boolean = False
			Dim bCurrentCurrencyID As Boolean = False
			oWebService = New NexusProvider.ProviderManager().Provider

			'has writeoff authority
			If (chkTempCheckBox.Checked AndAlso oUserAuthority.UserAuthorityValue = 1 AndAlso oWriteOffInterMediateAccount.OptionValue <> "") Then
				hypWriteOff.Enabled = True
				hypWriteOff.OnClientClick = "tb_show(null ,'../Modal/WriteOffPayment.aspx?modal=true&KeepThis=true&TB_iframe=true&height=100&width=150 ' , null);return false;"
			End If

			' Mark or Unmark a record when checkbox checked or unchecked

			If Not IsNothing(oAccountDetailCollection) Then
				For Each oAccountDetails As NexusProvider.AccountDetails In oAccountDetailCollection
					If oAccountDetails.IsSelected AndAlso Not bFoundSelectedCurrency Then
						preSelectedCurrencyID = oAccountDetails.CurrencyId
						bFoundSelectedCurrency = True
					End If

					If oAccountDetails.DocumentRef.Trim = sDocumentRef AndAlso Not bCurrentCurrencyID Then
						currencyID = oAccountDetails.CurrencyId
						bCurrentCurrencyID = True
					End If
				Next
			End If

			If currencyID = preSelectedCurrencyID OrElse preSelectedCurrencyID = -1 OrElse Not chkTempCheckBox.Checked Then
				Session.Remove(CNDocumentRef)
				Session(CNDocumentRef) = sDocumentRef

				'Select the Child Record and Refresh The Data Stored in Session
				PopulateOutstanding()
				oOutstandingAccountDetails = CType(Cache.Item(ViewState("OutStandingGridResultpageCacheID")), NexusProvider.AccountDetailsCollection)

				If oAccountDetailCollection IsNot Nothing Then
					If oAccountDetailCollection.Count > 0 Then
						If chkTempCheckBox.Checked Then
							'Mark
							For iCount As Integer = 0 To oAccountDetailCollection.Count - 1
								If oAccountDetailCollection(iCount).DocumentRef.Trim().ToUpper = sDocumentRef.ToString.Trim.ToUpper Then

									'Mark All Child Record
									For jCount As Integer = 0 To oOutstandingAccountDetails.Count - 1
										If oAccountDetailCollection(iCount).DocumentRef.Trim().ToUpper = oOutstandingAccountDetails(jCount).DocumentRef.Trim().ToUpper Then
											oMarkUnmark = New NexusProvider.MarkUnmarkTransaction
											oMarkUnmark.CurrencyCode = oOutstandingAccountDetails(jCount).CurrencyCode.Trim()
											oMarkUnmark.TransactionKey = oOutstandingAccountDetails(jCount).TransdetailId
											If oMarkUnmark.CurrencyCode <> "" AndAlso Session(CNTransCurr) Is Nothing Then
												Session(CNTransCurr) = oMarkUnmark.CurrencyCode
											End If
											If rblViewby.Items.FindByValue("TC").Selected = True Then
												oMarkUnmark.PaymentAmount = oOutstandingAccountDetails.Item(jCount).CurrencyAmount - oOutstandingAccountDetails.Item(jCount).PaidAmount
											ElseIf rblViewby.Items.FindByValue("AC").Selected = True Then
												oMarkUnmark.PaymentAmount = oOutstandingAccountDetails.Item(jCount).AccountAmount - oOutstandingAccountDetails.Item(jCount).PaidAccountAmount
											End If

											oMarkUnmark.MarkStatus = NexusProvider.MarkStatusType.Mark
											oWebService.MarkUnmarkTransaction(oMarkUnmark)
										End If
									Next
									Exit For
								End If
							Next
						Else
							'Un Mark
							For iCount As Integer = 0 To oAccountDetailCollection.Count - 1
								If oAccountDetailCollection(iCount).DocumentRef.Trim().ToUpper = sDocumentRef.ToString.Trim.ToUpper Then

									'Un Mark All Child Record
									For jCount As Integer = 0 To oOutstandingAccountDetails.Count - 1
										If oAccountDetailCollection(iCount).DocumentRef.Trim().ToUpper = oOutstandingAccountDetails(jCount).DocumentRef.Trim().ToUpper Then

											If oOutstandingAccountDetails(jCount).IsSelected = True AndAlso oOutstandingAccountDetails(jCount).Spare <> "WRITEOFF" Then
												oMarkUnmark = New NexusProvider.MarkUnmarkTransaction
												oMarkUnmark.CurrencyCode = oOutstandingAccountDetails(jCount).CurrencyCode.Trim()
												oMarkUnmark.MarkStatus = NexusProvider.MarkStatusType.UnMark
												oMarkUnmark.PaymentAmount = CType("0.00", Decimal)
												oMarkUnmark.TransactionKey = oOutstandingAccountDetails(jCount).TransdetailId
												oWebService.MarkUnmarkTransaction(oMarkUnmark)
											End If

											If (oOutstandingAccountDetails(jCount).Spare = "WRITEOFF") Then
												Dim sWriteOffMessage As String = GetLocalResourceObject("RemoveWriteOffMessage").ToString
												ViewState("WriteOffTransactionGridName") = "chkMarked"
												ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "RemoveWriteOff('" & sWriteOffMessage & "');", True)
												Exit Sub
											End If
										End If
									Next
									Exit For
								End If
							Next
						End If

					End If
				End If
			Else
				ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "Alert", "showAlert('Entries with different currencies cannot be selected')", True)
			End If

			'Refresh the grid
			Cache.Remove(ViewState("AccountResultpageCacheID"))
			Cache.Remove(ViewState("ManupulatedGridResultpageCacheID"))
			Cache.Remove(ViewState("OutStandingGridResultpageCacheID"))
			GridDataBind()
			PopulateOutstandingTransaction()
		End Sub

		Protected Sub chkMarkedOutTran_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

			Dim chkTempCheckBox As CheckBox = sender
			Dim oAccountDetailCollection As NexusProvider.AccountDetailsCollection = CType(Cache.Item(ViewState("OutStandingGridResultpageCacheID")), NexusProvider.AccountDetailsCollection)
			Dim oMarkUnmark As New NexusProvider.MarkUnmarkTransaction
			Dim gvTempGridView As GridViewRow = CType(chkTempCheckBox.NamingContainer, GridViewRow)
			Dim sDocumentRef = gvTempGridView.Cells(2).Text.Trim()
			Dim hidTrans As HiddenField = gvTempGridView.FindControl("HidTransID")
			Dim iTransactionID As Integer = hidTrans.Value
			Session(CNDocumentRef) = sDocumentRef

			oWebService = New NexusProvider.ProviderManager().Provider

			If oAccountDetailCollection IsNot Nothing Then
				If oAccountDetailCollection.Count > 0 Then
					If chkTempCheckBox.Checked Then
						'Mark
						For Each oAccountdetails As NexusProvider.AccountDetails In oAccountDetailCollection
							If oAccountdetails.TransdetailId = iTransactionID Then
								oMarkUnmark.CurrencyCode = oAccountdetails.CurrencyCode.Trim()
								oMarkUnmark.TransactionKey = oAccountdetails.TransdetailId

								If rblViewby.Items.FindByValue("TC").Selected = True Then
									oMarkUnmark.PaymentAmount = oAccountdetails.CurrencyAmount - oAccountdetails.PaidAmount
								ElseIf rblViewby.Items.FindByValue("AC").Selected = True Then
									oMarkUnmark.PaymentAmount = oAccountdetails.AccountAmount - oAccountdetails.PaidAccountAmount
								End If

								oMarkUnmark.MarkStatus = NexusProvider.MarkStatusType.Mark
								oWebService.MarkUnmarkTransaction(oMarkUnmark)
								Exit For
							End If
						Next
					Else
						'Un Mark

						For Each oAccountdetails As NexusProvider.AccountDetails In oAccountDetailCollection
							If oAccountdetails.TransdetailId = iTransactionID AndAlso oAccountdetails.Spare <> "WRITEOFF" Then
								oMarkUnmark.CurrencyCode = oAccountdetails.CurrencyCode.Trim()
								oMarkUnmark.MarkStatus = NexusProvider.MarkStatusType.UnMark
								oMarkUnmark.PaymentAmount = CType("0.00", Decimal)
								oMarkUnmark.TransactionKey = oAccountdetails.TransdetailId
								oWebService.MarkUnmarkTransaction(oMarkUnmark)
								Exit For
							ElseIf oAccountdetails.TransdetailId = iTransactionID AndAlso oAccountdetails.Spare = "WRITEOFF" Then
								Dim sWriteOffMessage As String = GetLocalResourceObject("RemoveWriteOffMessage").ToString
								ViewState("WriteOffTransactionGridName") = "chkMarkedOutTran"
								ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "RemoveWriteOff('" & sWriteOffMessage & "');", True)
								Exit Sub
							End If
						Next
					End If

				End If
			End If

			'Refresh the grid
			Cache.Remove(ViewState("AccountResultpageCacheID"))
			Cache.Remove(ViewState("ManupulatedGridResultpageCacheID"))
			Cache.Remove(ViewState("OutStandingGridResultpageCacheID"))
			GridDataBind()
			PopulateOutstandingTransaction()

		End Sub

		Protected Sub btnDrill_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDrill.Click
			Response.Redirect("~/Secure/SearchTransactions.aspx?Mode=IP&AllocationAccountkey=" & hiddenAccountCode.Value, False)
		End Sub

		Protected Sub CollectAllMarkedTransaction(ByVal oAccountDetailCollection As NexusProvider.AccountDetailsCollection)
			Dim iMarkedAmount As Decimal
			Dim TransdetailList As New ArrayList
			If oAccountDetailCollection IsNot Nothing Then
				If oAccountDetailCollection.Count > 0 Then
					For Each oAccountDetail As NexusProvider.AccountDetails In oAccountDetailCollection
						iMarkedAmount = oAccountDetail.MarkedAmount

						If iMarkedAmount <> 0 Then
							TransdetailList.Add(New Pair(oAccountDetail.TransdetailId, oAccountDetail.MarkedAmount))
						End If

					Next

					Session.Remove(CNMarkedTransDetailList)
					Session(CNMarkedTransDetailList) = TransdetailList
					'hold the value in hidden text box to validate minimum selection of single transaction
					hiddentxtTotalTransactionSelected.Value = TransdetailList.Count
				End If
			End If
		End Sub

		Protected Sub chkOTSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
			Dim chkOTselectall As CheckBox = grdvOutstandingTransaction.HeaderRow.FindControl("chkOTSelectAll")
			Dim oAccountDetailsCollection As NexusProvider.AccountDetailsCollection
			Dim oMarkUnMarkTransaction As NexusProvider.MarkUnmarkTransaction
			Dim sDocumentRef As String = Session(CNDocumentRef)
			oWebService = New NexusProvider.ProviderManager().Provider
			oAccountDetailsCollection = CType(Cache.Item(ViewState("OutStandingGridResultpageCacheID")), NexusProvider.AccountDetailsCollection)
			'Session(CNOutStandingGridResult)

			If chkOTselectall.Checked = True Then
				'for select all
				For i As Integer = 0 To oAccountDetailsCollection.Count - 1
					If oAccountDetailsCollection(i).DocumentRef.Trim.ToUpper = sDocumentRef.Trim.ToUpper AndAlso oAccountDetailsCollection(i).IsSelected <> True Then
						oMarkUnMarkTransaction = New NexusProvider.MarkUnmarkTransaction
						oMarkUnMarkTransaction.CurrencyCode = oAccountDetailsCollection(i).CurrencyCode.Trim()
						oMarkUnMarkTransaction.TransactionKey = oAccountDetailsCollection(i).TransdetailId
						oMarkUnMarkTransaction.MarkStatus = NexusProvider.MarkStatusType.Mark

						If rblViewby.Items.FindByValue("TC").Selected = True Then
							oMarkUnMarkTransaction.PaymentAmount = oAccountDetailsCollection.Item(i).CurrencyAmount - oAccountDetailsCollection.Item(i).PaidAmount
						ElseIf rblViewby.Items.FindByValue("AC").Selected = True Then
							oMarkUnMarkTransaction.PaymentAmount = oAccountDetailsCollection.Item(i).AccountAmount - oAccountDetailsCollection.Item(i).PaidAccountAmount
						End If

						oWebService.MarkUnmarkTransaction(oMarkUnMarkTransaction)
					End If
				Next
			Else
				'for Clear all
				For i As Integer = 0 To oAccountDetailsCollection.Count - 1
					If oAccountDetailsCollection(i).MarkedAmount <> 0 And oAccountDetailsCollection(i).DocumentRef.Trim.ToUpper = sDocumentRef.Trim.ToUpper And (oAccountDetailsCollection(i).Spare <> "WRITEOFF") Then
						oMarkUnMarkTransaction = New NexusProvider.MarkUnmarkTransaction
						oMarkUnMarkTransaction.CurrencyCode = oAccountDetailsCollection(i).CurrencyCode.Trim()
						oMarkUnMarkTransaction.TransactionKey = oAccountDetailsCollection(i).TransdetailId
						oMarkUnMarkTransaction.MarkStatus = NexusProvider.MarkStatusType.UnMark
						oMarkUnMarkTransaction.PaymentAmount = CType("0.00", Decimal)
						oWebService.MarkUnmarkTransaction(oMarkUnMarkTransaction)
					End If

					If (oAccountDetailsCollection(i).Spare = "WRITEOFF") Then
						Dim sWriteOffMessage As String = GetLocalResourceObject("RemoveWriteOffMessage").ToString
						ViewState("WriteOffTransactionGridName") = "chkOTSelectAll"
						ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "RemoveWriteOff('" & sWriteOffMessage & "');", True)
						Exit Sub
					End If
				Next
			End If

			'Refresh the grid
			Cache.Remove(ViewState("AccountResultpageCacheID"))
			Cache.Remove(ViewState("ManupulatedGridResultpageCacheID"))
			Cache.Remove(ViewState("OutStandingGridResultpageCacheID"))
			GridDataBind()
			PopulateOutstandingTransaction()
		End Sub

		Protected Sub chkInsurerPaymentsSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
			Dim chkIPselectall As CheckBox = grdvResultInsurerPayments.HeaderRow.FindControl("chkSelectAll")
			Dim oAllAccountDetailsCollection As NexusProvider.AccountDetailsCollection
			Dim oUpperGridAccountDetailsCollection As NexusProvider.AccountDetailsCollection
			Dim oMarkUnMarkTransaction As NexusProvider.MarkUnmarkTransaction
			Dim sDocumentRef As String = ""

			oWebService = New NexusProvider.ProviderManager().Provider
			oAllAccountDetailsCollection = CType(Cache.Item(ViewState("AccountResultpageCacheID")), NexusProvider.AccountDetailsCollection)

			'Upper Grid Rows
			oUpperGridAccountDetailsCollection = CType(Cache.Item(ViewState("ManupulatedGridResultpageCacheID")), NexusProvider.AccountDetailsCollection)

			If chkIPselectall.Checked = True Then
				'for select all
				For iCount As Integer = 0 To oUpperGridAccountDetailsCollection.Count - 1
					'Mark All Child Record
					If oUpperGridAccountDetailsCollection(iCount).TotalOutstandingAmount <> 0 Then
						For jCount As Integer = 0 To oAllAccountDetailsCollection.Count - 1
							If oUpperGridAccountDetailsCollection(iCount).DocumentRef.Trim.ToUpper = oAllAccountDetailsCollection(jCount).DocumentRef.Trim.ToUpper Then
								oMarkUnMarkTransaction = New NexusProvider.MarkUnmarkTransaction
								oMarkUnMarkTransaction.CurrencyCode = oAllAccountDetailsCollection(jCount).CurrencyCode
								oMarkUnMarkTransaction.TransactionKey = oAllAccountDetailsCollection(jCount).TransdetailId
								oMarkUnMarkTransaction.MarkStatus = NexusProvider.MarkStatusType.Mark

								If rblViewby.Items.FindByValue("TC").Selected = True Then
									oMarkUnMarkTransaction.PaymentAmount = oAllAccountDetailsCollection.Item(jCount).CurrencyAmount - oAllAccountDetailsCollection.Item(jCount).PaidAmount
								ElseIf rblViewby.Items.FindByValue("AC").Selected = True Then
									oMarkUnMarkTransaction.PaymentAmount = oAllAccountDetailsCollection.Item(jCount).AccountAmount - oAllAccountDetailsCollection.Item(jCount).PaidAccountAmount
								End If

								oWebService = New NexusProvider.ProviderManager().Provider
								oWebService.MarkUnmarkTransaction(oMarkUnMarkTransaction)
							End If
						Next
					End If
				Next
			Else
				'for Clear all
				For iCount As Integer = 0 To oUpperGridAccountDetailsCollection.Count - 1
					'Un Mark All Child Record
					If oUpperGridAccountDetailsCollection(iCount).TotalMarkedAmount <> 0 Then
						For jCount As Integer = 0 To oAllAccountDetailsCollection.Count - 1
							If oUpperGridAccountDetailsCollection(iCount).DocumentRef.Trim.ToUpper = oAllAccountDetailsCollection(jCount).DocumentRef.Trim.ToUpper AndAlso oAllAccountDetailsCollection(jCount).Spare <> "WRITEOFF" Then
								oMarkUnMarkTransaction = New NexusProvider.MarkUnmarkTransaction
								oMarkUnMarkTransaction.CurrencyCode = oAllAccountDetailsCollection(jCount).CurrencyCode.Trim()
								oMarkUnMarkTransaction.TransactionKey = oAllAccountDetailsCollection(jCount).TransdetailId
								oMarkUnMarkTransaction.MarkStatus = NexusProvider.MarkStatusType.UnMark
								oMarkUnMarkTransaction.PaymentAmount = CType("0.00", Decimal)
								oWebService.MarkUnmarkTransaction(oMarkUnMarkTransaction)
							End If
						Next
					End If
				Next

				For iCount As Integer = 0 To oAllAccountDetailsCollection.Count - 1
					If (oAllAccountDetailsCollection(iCount).Spare = "WRITEOFF") Then
						sDocumentRef = sDocumentRef & "  " & oAllAccountDetailsCollection(iCount).DocumentRef.Trim()
					End If
				Next

				If sDocumentRef <> "" Then
					Dim sWriteOffMessage As String = GetLocalResourceObject("RemoveWriteOffMessage").ToString
					sWriteOffMessage = sWriteOffMessage & sDocumentRef
					ViewState("WriteOffTransactionGridName") = "chkInsurerPaymentsSelectAll"
					ScriptManager.RegisterStartupScript(Me, Page.GetType, "script", "RemoveWriteOff('" & sWriteOffMessage & "');", True)
					Exit Sub
				End If
			End If

			'Refresh the grid
			Cache.Remove(ViewState("AccountResultpageCacheID"))
			Cache.Remove(ViewState("ManupulatedGridResultpageCacheID"))
			Cache.Remove(ViewState("OutStandingGridResultpageCacheID"))
			GridDataBind()
			PopulateOutstandingTransaction()
		End Sub

		Protected Sub btnNewsearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewsearch.Click
			Response.Redirect("~/secure/InsurerPayments.aspx", False)
			'ResetValues()
			'Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
			'oUserDetails = oWebService.GetUserDetails(HttpContext.Current.User.Identity.Name)
			'If Not String.IsNullOrEmpty(oUserDetails.PartyCode) AndAlso Not oUserDetails.PartyCode Is Nothing Then
			'    txtAccountCode.Text = CStr(oUserDetails.PartyCode)
			'    txtAccountCode.ReadOnly = True
			'    btnAccountCode.Enabled = False
			'End If
		End Sub

		''' <summary>
		''' The process to make an allocation should be streamlined to allow the user to make allocations with a single button click
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub btnAllocate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAllocate.Click

			If Page.IsValid Then

				' Capture payment amount and transaction count BEFORE allocation resets them
				Session("AccountCode") = txtAccountCode.Text.Trim()
				Session("PaymentDate") = DateTime.Now.ToString("dd/MM/yyyy")
				Session("PaymentAmount") = If(txtTotalMarked.Text <> "", txtTotalMarked.Text, "0.00")
				Session("TransactionCount") = If(hiddentxtTotalTransactionSelected.Value <> "", hiddentxtTotalTransactionSelected.Value, "0")

				Dim oAllocation As NexusProvider.Allocation
				Dim oAllocationDetails As New NexusProvider.AllocationDetails
				Dim oAllocationDetailsCollection As New NexusProvider.AllocationDetailsCollections
				Dim oTransAllocationDetails As New NexusProvider.AllocationDetails

				Dim alTransDetailCollection As ArrayList = CType(Session(CNMarkedTransDetailList), ArrayList)
				Dim iTransDetailCount As Integer = alTransDetailCollection.Count
				Dim iCountVar As Integer = 0
				Dim sTransdetailKey As Integer = 0
				Dim dAmount As Double = 0

				'take the selected transactions from session and make the request ready for GetTransactionDetails
				If iTransDetailCount > 0 Then
					For Each oPair As Pair In alTransDetailCollection
						oAllocationDetails = New NexusProvider.AllocationDetails
						oAllocationDetails.TransdetailKey = oPair.First
						oAllocationDetailsCollection.Add(oAllocationDetails)
						oAllocationDetails = Nothing
					Next
				End If

				oWebService = New NexusProvider.ProviderManager().Provider
				oAllocationDetailsCollection = oWebService.GetTransactionDetails(Session(CNAccountkey), oAllocationDetailsCollection)

				For Each oTempAllocationDetails As NexusProvider.AllocationDetails In oAllocationDetailsCollection

					If iCountVar = 0 Then
						'first allocation will not go in collection, it will directly in TransdetailKey and Amount
						sTransdetailKey = oTempAllocationDetails.TransdetailKey
						For Each oPair As Pair In alTransDetailCollection
							If sTransdetailKey = CInt(oPair.First) Then
								dAmount = oTempAllocationDetails.Amount
								Exit For
							End If
						Next
					Else
						'make a collection of rest selected transactions
						oAllocation = New NexusProvider.Allocation
						oAllocation.AllocationTransdetailKey = oTempAllocationDetails.TransdetailKey
						oAllocation.AllocationTimeStamp = oTempAllocationDetails.AllocationTimeStamp

						For Each oPair As Pair In alTransDetailCollection
							If oAllocation.AllocationTransdetailKey = CInt(oPair.First) Then
								oAllocation.AllocationAmount = oTempAllocationDetails.Amount
								Exit For
							End If
						Next

						oTransAllocationDetails.Allocation.Add(oAllocation)
					End If

					iCountVar = iCountVar + 1
					oAllocation = Nothing

				Next

				'Account Key of the selected account (use text box hiddenAccountCode for AccountKey).
				If Not IsNothing(hiddenAccountCode.Value) AndAlso Not String.IsNullOrEmpty(hiddenAccountCode.Value) Then
					oTransAllocationDetails.AccountKey = hiddenAccountCode.Value
				Else
					oTransAllocationDetails.AccountKey = Convert.ToInt32(Session(CNAccountkey))
				End If

				'CashListItemKey should be always zero/any negative value for the Auto Allocation process
				oTransAllocationDetails.CashListItemKey = 0

				'Transdetailkey and  Amount should be the TransdetailID of the first selected transaction using checkbox for Auto Allocation process.
				oTransAllocationDetails.TransdetailKey = sTransdetailKey
				oTransAllocationDetails.Amount = dAmount

				'call SAM method for Auto Allocation Process
				oWebService.UpdateAllocation(oTransAllocationDetails)

				'cleaning up
				oWebService = Nothing
				oAllocation = Nothing
				oAllocationDetails = Nothing
				oAllocationDetailsCollection = Nothing
				oTransAllocationDetails = Nothing
				alTransDetailCollection = Nothing
				iTransDetailCount = Nothing
				iCountVar = Nothing
				sTransdetailKey = Nothing
				dAmount = Nothing

				' populating Transaction grid
				GridDataBind()

				' Populating Outstanding transaction grid
				PopulateOutstandingTransaction()

				'Generate the report
				GenerateReport()
			End If

		End Sub

		Sub PopulateCurrencyByBranch()
			'Fill Currency
			Dim oCurrencyCollection As NexusProvider.CurrencyCollection
			oWebService = New NexusProvider.ProviderManager().Provider
			Dim sBranchCode As String

			If Request.QueryString("Mode") Is Nothing And CType(Session(CNMode), Mode) = Mode.PayClaim And Session(CNUnAllocatedClaimPayment) Is Nothing Then
				Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
				sBranchCode = oQuote.BranchCode
			Else
				sBranchCode = Session(CNBranchCode)
			End If
			hdnBranch.Value = sBranchCode
			oCurrencyCollection = oWebService.GetCurrenciesByBranch(sBranchCode)
			oCurrencyCollection.SortColumn = "Description"
			oCurrencyCollection.SortingOrder = NexusProvider.GenericComparer.SortOrder.Ascending
			oCurrencyCollection.Sort()

			ddlCurrency.Items.Clear()
			For i As Integer = 0 To oCurrencyCollection.Count - 1
				Dim lstCurrency As New ListItem
				lstCurrency.Text = oCurrencyCollection.Item(i).Description.ToString
				lstCurrency.Value = Trim(oCurrencyCollection.Item(i).CurrencyCode.ToString)
				ddlCurrency.Items.Add(lstCurrency)
			Next
			ddlCurrency.DataBind()

			'set the dropdown value by default as that of system currency otherwise empty
			If Session(CNCurrency) IsNot Nothing And ddlCurrency.Items.Count > 0 Then
				For i As Integer = 0 To oCurrencyCollection.Count - 1
					If oCurrencyCollection(i).CurrencyCode.Trim = Session(CNCurrency).ToString.Trim Then
						ddlCurrency.SelectedValue = oCurrencyCollection(i).CurrencyCode.Trim
						Exit For
					Else
						ddlCurrency.Items.Remove("")
						ddlCurrency.SelectedIndex = 0
					End If
				Next
			End If

			ddlCurrency.Items.Insert(0, New ListItem("(select all)", ""))

		End Sub

		Protected Sub rblViewby_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblViewby.SelectedIndexChanged
			If rblViewby.SelectedValue.ToString = "TC" Then
				PopulateCurrencyByBranch()
				lblCurrency.Visible = True
				ddlCurrency.Visible = True
			Else
				ddlCurrency.Items.Remove("")
				ddlCurrency.Items.Insert(0, "")
				ddlCurrency.SelectedIndex = 0
				lblCurrency.Visible = False
				ddlCurrency.Visible = False
			End If
			If (hdnLedgerCode.Value.Trim.ToUpper <> "AG") Then
				Dim sValue As String = hdnLedgerCode.Value.Trim.ToUpper
				ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "RemoveComission('" & sValue & "');", True)
			End If
		End Sub

		Sub PopulatePeriodTable()
			Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
			Dim oAllocationPeriod As NexusProvider.PeriodCollection

			Dim sBranchCode As String
			Dim PeriodArray As New ArrayList

			If Request.QueryString("Mode") Is Nothing And CType(Session(CNMode), Mode) = Mode.PayClaim And Session(CNUnAllocatedClaimPayment) Is Nothing Then
				Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
				sBranchCode = oQuote.BranchCode
			Else
				sBranchCode = Session(CNBranchCode)
			End If

			oAllocationPeriod = oWebService.GetPeriod(False, sBranchCode)

			For i As Integer = 0 To oAllocationPeriod.Count - 1
				PeriodArray.Add(Trim(oAllocationPeriod.Item(i).PeriodName.ToString) & " " & Trim(oAllocationPeriod.Item(i).YearName.ToString))
			Next

			MultiSelectDD1.AddItems(PeriodArray)
		End Sub

		Protected Sub btnPay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPay.Click
			If Trim(txtReceiptAmount.Text) <> "" Then
				If Convert.ToDouble(txtTotalMarked.Text) > Convert.ToDouble(txtReceiptAmount.Text) Then
					Dim sMessage As String = "The Total Marked exceeds the value entered in respect of Receipt Amount. Please revise the Receipt Amount or change the marked transactions so that the Total Marked is less than or equal to the Receipt Amount."
					ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "AlertMessage('" & sMessage & "');", True)
					Exit Sub
				End If
			End If

			Dim oAccountDetailCollection As NexusProvider.AccountDetailsCollection = CType(Cache.Item(ViewState("AccountResultpageCacheID")), NexusProvider.AccountDetailsCollection)
			Dim sCurrencyCode As String = ""
			Dim dTotalAmount As Decimal
			Dim oCurrency As New NexusProvider.Currency
			Dim oWebservice As NexusProvider.ProviderBase

			If Not IsNothing(oAccountDetailCollection) Then
				For Each oAccountDetails As NexusProvider.AccountDetails In oAccountDetailCollection
					If oAccountDetails.IsSelected Then
						sCurrencyCode = oAccountDetails.CurrencyCode
						If sCurrencyCode <> "" AndAlso Session(CNTransCurr) Is Nothing Then
							Session(CNTransCurr) = sCurrencyCode
						End If
						Exit For
					End If
				Next
			End If
			Session("AccountCode") = txtAccountCode.Text.Trim()
			Session("PaymentDate") = DateTime.Now.ToString("dd/MM/yyyy")
			Session("PaymentAmount") = If(txtTotalMarked.Text <> "", txtTotalMarked.Text, "0.00")
			Session("TransactionCount") = If(hiddentxtTotalTransactionSelected.Value <> "", hiddentxtTotalTransactionSelected.Value, "0")

			If Not Session(CNCurrency) Is Nothing AndAlso (sCurrencyCode <> "" AndAlso Session(CNCurrency) <> sCurrencyCode) Then
				dTotalAmount = Session(CNTotalAmount)
				oWebservice = New NexusProvider.ProviderManager().Provider
				oCurrency.AccountCode = Session(CNAccountName)
				oCurrency.TransactionCurrencyCode = sCurrencyCode
				oCurrency.Mode = "ALL"
				oCurrency = oWebservice.GetCurrencyExchangeRates(oCurrency, Session(CNTransBranchCode))
				'Calculate the New Total Amount as per the choice
				dTotalAmount = Math.Round((dTotalAmount * oCurrency.TransactionCurrencyRate), 2)
				Session(CNTotalAmount) = dTotalAmount
			End If


			If Page.IsValid Then
				If Session(CNCurrency) Is Nothing OrElse Session(CNPartyKey) Is Nothing Then
					CheckForSession()
				End If
				Session("pageindex") = grdvResultInsurerPayments.PageIndex
				Response.Redirect("~/secure/payment/CashListNew.aspx?Mode=IP&PartyKey=" + hPartyKey.Value, False)
			End If
		End Sub

		Private Sub MarkTransactions(ByRef oGridDataCollection As NexusProvider.AccountDetailsCollection)
			Dim oMarkUnmark As NexusProvider.MarkUnmarkTransaction
			If oGridDataCollection IsNot Nothing Then
				For Each item As NexusProvider.AccountDetails In oGridDataCollection
					oMarkUnmark = New NexusProvider.MarkUnmarkTransaction
					oMarkUnmark.CurrencyCode = item.CurrencyCode
					oMarkUnmark.TransactionKey = item.TransdetailId
					If rblViewby.Items.FindByValue("TC").Selected = True Then
						oMarkUnmark.PaymentAmount = item.CurrencyAmount - item.PaidAmount
					ElseIf rblViewby.Items.FindByValue("AC").Selected = True Then
						oMarkUnmark.PaymentAmount = item.AccountAmount - item.PaidAccountAmount
					End If
					oMarkUnmark.MarkStatus = NexusProvider.MarkStatusType.UnMark
					oWebService.MarkUnmarkTransaction(oMarkUnmark)
				Next
				For Each item As NexusProvider.AccountDetails In oGridDataCollection
					oMarkUnmark = New NexusProvider.MarkUnmarkTransaction
					oMarkUnmark.CurrencyCode = item.CurrencyCode
					oMarkUnmark.TransactionKey = item.TransdetailId
					If rblViewby.Items.FindByValue("TC").Selected = True Then
						oMarkUnmark.PaymentAmount = item.CurrencyAmount - item.PaidAmount
					ElseIf rblViewby.Items.FindByValue("AC").Selected = True Then
						oMarkUnmark.PaymentAmount = item.AccountAmount - item.PaidAccountAmount
					End If
					oMarkUnmark.MarkStatus = NexusProvider.MarkStatusType.Mark
					oWebService.MarkUnmarkTransaction(oMarkUnmark)
				Next
				' obtaining data from SAM
				oGridDataCollection = oWebService.GetInsurerPayments(CType(Session(CNInsSearchCriteria), NexusProvider.AccountDetails))
				'put the data in cache
				Cache.Insert(ViewState("AccountResultpageCacheID"), oGridDataCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
			End If
		End Sub

		Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
			'This will populate search account modal 
			If HttpContext.Current.Session.IsCookieless Then
				btnAccountCode.OnClientClick = "tb_show(null , '" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/FindAccount.aspx?modal=true&Page=IP&KeepThis=true&TB_iframe=true&height=500&width=650' , null);return false;"
			Else
				btnAccountCode.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/Modal/FindAccount.aspx?modal=true&Page=IP&KeepThis=true&TB_iframe=true&height=500&width=650' , null);return false;"
			End If
		End Sub
		''' <summary>
		''' Validation for Single Cash List receipt/payment per allocation
		''' </summary>
		''' <param name="source"></param>
		''' <param name="args"></param>
		''' <remarks></remarks>
		Protected Sub ValidateSelectedTransaction(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
			Dim bIsSingleCashListItemAllocation As Boolean = CType(ViewState("IsSingleCashListPaymentOrReciept"), Boolean)
			Dim cvSingleSRPnSPY As CustomValidator = New CustomValidator
			cvSingleSRPnSPY = CType(source, CustomValidator)

			If bIsSingleCashListItemAllocation = True Then
				Dim iSelectedCashPayment As Integer
				Dim iOtherPayment As Integer
				Dim cPayAmount As Decimal = Session(CNTotalAmount)
				Dim oAllocationDetails As NexusProvider.AccountDetailsCollection = CType(Cache.Item(ViewState("ManupulatedGridResultpageCacheID")), NexusProvider.AccountDetailsCollection)
				For Each oAllocationDetail As NexusProvider.AccountDetails In oAllocationDetails
					If oAllocationDetail.IsSelected = True Then
						If Left(oAllocationDetail.DocumentRef.ToUpper, 3) = "SRP" Or Left(oAllocationDetail.DocumentRef.ToUpper, 3) = "SPY" Then
							iSelectedCashPayment = iSelectedCashPayment + 1
						Else
							iOtherPayment = iOtherPayment + 1
						End If
					End If
				Next

				If ((iSelectedCashPayment = 1 And cPayAmount <> 0) Or (iSelectedCashPayment > 1)) And iOtherPayment >= 1 Then
					If iSelectedCashPayment > 1 Then
						cvSingleSRPnSPY.ErrorMessage = GetLocalResourceObject("lbl_SingleSRPSPYError")
						args.IsValid = False
					ElseIf iSelectedCashPayment = 1 And cPayAmount <> 0 Then
						cvSingleSRPnSPY.ErrorMessage = GetLocalResourceObject("lbl_DupSRPSPYError")
						args.IsValid = False
					End If
				End If
			End If
		End Sub

		Protected Sub grdvResultInsurerPayments_Sorting(sender As Object, e As GridViewSortEventArgs) Handles grdvResultInsurerPayments.Sorting
			'sort the Quote & Policy according to the column clicked
			'we need to store the current sort order in viewstate, and reverse it each time
			Dim oCollection As NexusProvider.AccountDetailsCollection = ViewState(CNFilteredCollection)
			oCollection.SortColumn = e.SortExpression
			If (Session("pageindex") IsNot Nothing) Then
				grdvResultInsurerPayments.PageIndex = Convert.ToInt64(Session("pageindex"))
			End If
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
			oCollection.SortingOrder = _sortDirection
			oCollection.Sort()
			CType(sender, GridView).DataSource = oCollection
			CType(sender, GridView).DataBind()
		End Sub

		Public Sub GenerateReport(Optional ByVal bArchiveReport As Boolean = False)
			Dim oParametersCollection As New NexusProvider.ParametersCollection
			Dim sPlaceHolderControlID As String = "plcReportForm"
			Dim sUrl As String = String.Empty
			Dim sReportsTypeControlID As String = Nothing
			Dim sSelectedReportsType As String = Nothing
			Dim sCustomValidator As String = "cusReportForm"
			Dim oUserDetails As NexusProvider.UserDetails
			Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)

			'get the name of the selected Report to be generated
			sSelectedReportsType = GetLocalResourceObject("lblReport")

			'Executed Function from Dataset function
			Try

				If Session(CNAgentDetails) IsNot Nothing AndAlso CType(Session(CNAgentDetails), NexusProvider.UserDetails).UserId <> 0 Then
					oUserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
				Else
					If oWebService IsNot Nothing Then
						oUserDetails = oWebService.GetUserDetails(HttpContext.Current.User.Identity.Name)
					End If
				End If
				Dim oParameters As NexusProvider.Parameters
				oParameters = New NexusProvider.Parameters
				oParameters.ParamNameField = "user_id"
				oParameters.ParamValueField = oUserDetails.UserId

				'add the param into the collection
				oParametersCollection.Add(oParameters)

				Session("Parameters") = oParametersCollection
				sUrl = GetReportUrl(sSelectedReportsType, oParametersCollection, bArchiveReport)
				ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "openReport", "openReport('" & sUrl & "');", True)
			Catch ex As NexusProvider.NexusException
				'Checking  (bSIRReportPrint.Business.SendToPrint Failed : Failed : Return Value = PMNotFound) Error code , then display a message saying no record found 
				If ex.Errors(0).Code = "1000019" Then
					ex.Errors(0).Code = "88"
				End If
				Throw
			End Try
		End Sub
		''' <summary>
		''' This method retreives the report
		''' </summary>
		''' <param name="sReportName"></param>
		''' <param name="oParametersCollection"></param>
		''' <remarks></remarks>
		Public Function GetReportUrl(ByVal sReportName As String, ByVal oParametersCollection As NexusProvider.ParametersCollection, Optional ByVal bArchiveReport As Boolean = False) As String
			Dim url As String = String.Empty

			sReportPath = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork) _
				  .Portals.Portal(CMS.Library.Portal.GetPortalID()).Reports.Location

			If HttpContext.Current.Session.IsCookieless Then
				url = AppSettings("webroot") & "(S(" & Current.Session.SessionID.ToString() + "))" & "/secure/reportviewer.aspx?reportname=" & HttpUtility.UrlEncode(sReportName) & "&Mode=IP&PartyKey=" + Request.QueryString("PartyKey")
			Else
				url = AppSettings("webroot") & "secure/reportviewer.aspx?reportname=" & HttpUtility.UrlEncode(sReportName) & "&Mode=IP&PartyKey=" + Request.QueryString("PartyKey")
			End If

			If bArchiveReport = False Then
				ArchiveReportToSharePoint()
			Else
				ReportExport("pdf", sReportName, oParametersCollection)
			End If
			Return url
		End Function

		Public Sub ArchiveReportToSharePoint()
			'Dim oQuote As NexusProvider.Quote
			'oQuote = Session(CNQuote)
			GenerateReport(True)
			Dim sPartyKey = Request.QueryString("PartyKey")
			Dim xlJob As XElement =
		  <BACKGROUND_JOB>
			  <JOB jobtype="DOCUPACK">
				  <PARAMETERS>
					  <PARAMETER name="destination" value="archive"/>
					  <PARAMETER name="archive" value="True"/>
					  <PARAMETER name="PartyCnt" value=<%= sPartyKey %>/>
					  <PARAMETER name="type" value="report"/> **Added parameter for Archiving Reports
              </PARAMETERS>
			  </JOB>
		  </BACKGROUND_JOB>

			Dim xlPath As XElement = <PARAMETER name="Path" value=<%= sFileReportName %>/>
			xlJob.Element("JOB").Element("PARAMETERS").Add(xlPath)
			'we need to specify format
			Dim sFileType As String = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).DocumentFormat.ToLower()
			xlJob.Element("JOB").Element("PARAMETERS").Add(New XElement(<PARAMETER name="OutputFormat" value=<%= sFileType.ToUpper %>/>))
			'documents to generate so specify the document template code
			Dim sOutputFileName As String = Right(sFileReportName.ToString(), Len(sFileReportName.ToString()) - InStrRev(sFileReportName.ToString(), "\"))
			sOutputFileName = Left(sOutputFileName, sOutputFileName.LastIndexOf("."))
			Dim xlDestinationFileName As XElement = <PARAMETER name="DestinationFilename" value=<%= ReplaceSplCharacters(sOutputFileName) %>/>
			xlJob.Element("JOB").Element("PARAMETERS").Add(xlDestinationFileName)

			Dim strJob As String = xlJob.ToString 'this will be used as input to the SAM call
			Dim sDescription As String = "Archive report"
			Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
			'call SAM to queue the docs for Archiving
			Dim iBackgroundJobID As Integer = oWebService.CreateBackgroundJob(sDescription, strJob, Now.Date)
			If Request.QueryString("PostBack") IsNot Nothing Then
				If Request.QueryString("PostBack").ToUpper = "True" Then
					Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "RefreshGrid") & ";"
					'refresh the parent page on postback with event argument RefreshGrid  
					Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
				End If
			End If

		End Sub
		Public Sub ReportExport(format As String, reportName As String, ByVal oParametersCollection As NexusProvider.ParametersCollection)

			Dim viewer = New ReportViewer() With {
				  .ProcessingMode = ProcessingMode.Local
				}
			Dim devInfo = "<DeviceInfo><Toolbar>False</Toolbar></DeviceInfo>"

			Dim localReport As LocalReport = viewer.LocalReport
			If Not sReportPath.EndsWith("\") Then
				localReport.ReportPath = sReportPath & "\" & reportName & ".rdl"
			Else
				localReport.ReportPath = sReportPath & reportName & ".rdl"
			End If

			Dim arrReportName As String() = reportName.Split("\")
			If arrReportName.Length > 1 Then
				sReportFolderName = arrReportName(0)
			End If
			Dim addBranchInLocalParameter As Boolean = False

			For Each rp As ReportParameterInfo In localReport.GetParameters()
				If rp.Name.ToLower() = "branch" Then
					addBranchInLocalParameter = True
					Exit For
				End If
			Next

			Dim iCount As Integer = 0
			Dim oParameter(oParametersCollection.Count - 1, 1) As Object
			For Each param As NexusProvider.Parameters In oParametersCollection
				oParameter(iCount, 0) = param.ParamNameField
				oParameter(iCount, 1) = param.ParamValueField
				iCount = iCount + 1
			Next

			Dim reportDataSets As ReportDataSets = GetReportDataSet(sReportPath, reportName, oParameter)

			localReport.DataSources.Clear()
			Dim localReportParameter As List(Of ReportParameter) = New List(Of ReportParameter)()
			Dim dsReport As Data.DataSet = GetReportData(reportDataSets.ReportDataSet(0), oParametersCollection, localReportParameter, addBranchInLocalParameter, localReport.GetParameters())

			localReport.DataSources.Add(New ReportDataSource() With {
			  .Name = "DataSet1",
			  .Value = dsReport.Tables(0)
			})
			' Add a handler for SubreportProcessing.
			AddHandler localReport.SubreportProcessing, AddressOf Me.SubreportProcessingEventHandler

			Dim warningslocal As Warning() = Nothing
			Dim encoding As String = String.Empty
			Dim streamIds As String() = Nothing
			Dim mimeType As String = String.Empty
			Dim extension As String = String.Empty
			localReport.SetParameters(localReportParameter)
			Dim renderingExtension As RenderingExtension() = localReport.ListRenderingExtensions()
			localReport.Refresh()
			Dim bytes = localReport.Render(format, devInfo, mimeType, encoding, extension, streamIds, warningslocal)

			Dim sUniqueDirectory As String = Guid.NewGuid.ToString
			Dim reportExportLocation As String = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork) _
				  .Portals.Portal(CMS.Library.Portal.GetPortalID()).Reports.ExportLocation
			Dim compileReportPath As String
			If arrReportName.Length > 1 Then
				If Not reportExportLocation.EndsWith("\") Then
					compileReportPath = reportExportLocation & "\" & sReportFolderName & "\" & sUniqueDirectory & "\" & arrReportName(1) & "." & format
				Else
					compileReportPath = reportExportLocation & sReportFolderName & "\" & sUniqueDirectory & "\" & arrReportName(1) & "." & format
				End If
			Else
				If Not reportExportLocation.EndsWith("\") Then
					compileReportPath = reportExportLocation & "\" & sUniqueDirectory & "\" & reportName & "." & format
				Else
					compileReportPath = reportExportLocation & sUniqueDirectory & "\" & reportName & "." & format
				End If
			End If
			sFileReportName = compileReportPath
			If Not Directory.Exists(Path.GetDirectoryName(compileReportPath)) Then
				Directory.CreateDirectory(Path.GetDirectoryName(compileReportPath))
			End If

			Dim strings = compileReportPath.Split(New Char() {"."c})
			Dim lenght = strings.Length
			Dim extensionToReplace = strings(lenght - 1)
			compileReportPath = compileReportPath.Replace("." & extensionToReplace, "." & format)
			File.WriteAllBytes(compileReportPath, bytes)

		End Sub
		Public Function GetReportData(ByRef reportDataSet As ReportDataSet, ByRef reportParameters As NexusProvider.ParametersCollection, ByRef localReportParameter As List(Of Microsoft.Reporting.WebForms.ReportParameter), ByVal addBranchInLocalParameter As Boolean, ByVal orgReportParameters As ReportParameterInfoCollection) As Data.DataSet
			Dim dsReport As Data.DataSet = New Data.DataSet()
			Dim storedProcedureName As String = reportDataSet.SqlCommandText
			Dim sqlCommandType As String = reportDataSet.SqlCommandType
			Dim queryParameters = New Dictionary(Of String, Object)()
			localReportParameter = New List(Of Microsoft.Reporting.WebForms.ReportParameter)()

			Dim operatorParamExists As Boolean = orgReportParameters.Any(Function(p) p.Name = "Operator")
			If operatorParamExists Then
				localReportParameter.Add(New Microsoft.Reporting.WebForms.ReportParameter("Operator", Session(Nexus.Constants.CNLoginName).ToString()))
			End If

			Dim oQueryParameterCollection As New NexusProvider.ParametersCollection
			Dim i As Integer = 0
			For Each param As NexusProvider.Parameters In reportParameters
				If (Not Equals(param.ParamNameField.ToString().ToLower(), "branch") Or addBranchInLocalParameter) Then
					Dim value As String = String.Empty
					If param.ParamValueField Is Nothing Then
						If (param.ParamNameField = "TPACode") Then
							value = "null"
						Else
							value = 0
						End If
					Else
						value = param.ParamValueField.ToString()
					End If
					Dim key As String = param.ParamNameField.ToString()
					If key = "BRANCH_ID" Then
						key = "branch_id"
						If value.ToLower() = "all" Then
							value = 0
						End If
					End If
					For Each p As ReportParameterInfo In orgReportParameters
						If p.Name.ToLower() = key.ToLower() Then
							localReportParameter.Add(New Microsoft.Reporting.WebForms.ReportParameter(p.Name, value))
							Exit For
						End If
					Next
				End If

				If (reportDataSet.ReportQueryParameters IsNot Nothing) Then

					For Each keyValue As KeyValuePair(Of String, Object) In reportDataSet.ReportQueryParameters
						If keyValue.Key.ToString().StartsWith("@") Then
							Dim key = keyValue.Key.Replace("@", "").Trim()
							If Equals(key.ToLower(), param.ParamNameField.ToString().ToLower()) Then
								Dim oParam As New NexusProvider.Parameters
								oParam.ParamNameField = param.ParamNameField.ToString()
								oParam.ParamValueField = param.ParamValueField
								oQueryParameterCollection.Add(oParam)
								Exit For
							End If
						End If
					Next
				End If
			Next

			Dim objDataSet As New System.Data.DataSet

			'make SAM call with request parameters, sFileName will contain the name of the file we need to display
			objDataSet = oWebService.CallNamedStoredProcedure(reportDataSet.SqlCommandText, oQueryParameterCollection, True)
			Return objDataSet
		End Function
		Public Function GetReportDataSet(reportPath As String, reportName As String, reportParameters As Object(,)) As ReportDataSets

			Dim report As RdlReportSchema.Report = New RdlReportSchema.Report()
			Dim reportDataSets As ReportDataSets = New ReportDataSets()
			Dim reportDataSet As List(Of ReportDataSet) = New List(Of ReportDataSet)()
			If reportPath.EndsWith("\") = False Then
				reportPath = reportPath & "\"
			End If

			report = GetReportDetails(reportPath, reportName, False)
			If report.DataSets IsNot Nothing AndAlso report.DataSets.DataSet IsNot Nothing Then
				'foreach (DataSet ds in report.DataSets.DataSet)
				If True Then
					If report.DataSets.DataSet IsNot Nothing AndAlso report.DataSets.DataSet.Query IsNot Nothing Then
						Dim reportDataSet1 As ReportDataSet = New ReportDataSet()
						reportDataSet1.DataSetName = report.DataSets.DataSet.Name
						reportDataSet1.SqlCommandType = report.DataSets.DataSet.Query.CommandType
						reportDataSet1.SqlCommandText = report.DataSets.DataSet.Query.CommandText
						If report.DataSets.DataSet.Query.QueryParameters IsNot Nothing AndAlso report.DataSets.DataSet.Query.QueryParameters.QueryParameter.Count > 0 Then
							reportDataSet1.ReportQueryParameters = New Dictionary(Of String, Object)()

							For Each queryParameter As RdlReportSchema.QueryParameter In report.DataSets.DataSet.Query.QueryParameters.QueryParameter
								reportDataSet1.ReportQueryParameters.Add(queryParameter.Name, queryParameter.Value)
							Next
						End If
						reportDataSet.Add(reportDataSet1)
					End If
				End If
			End If
			reportDataSets.ReportDataSet = New List(Of ReportDataSet)()
			reportDataSets.ReportDataSet = reportDataSet
			Return reportDataSets

		End Function
		Private Sub SubreportProcessingEventHandler(ByVal sender As Object,
											   ByVal e As Microsoft.Reporting.WebForms.SubreportProcessingEventArgs)
			Dim oReportDataSets As ReportDataSets
			Dim query As New Nexus.Utils.SubReportQuery
			If Not sReportPath.EndsWith("\") Then
				query = GetSubReportDetails(sReportPath + "\" + sReportFolderName, e.ReportPath)
			Else
				query = GetSubReportDetails(sReportPath + sReportFolderName, e.ReportPath)

			End If

			Dim storedProcedureName As String = query.CommandText.ToString()
			Dim sqlCommandType As String = query.CommandType.ToString()
			Dim dataSourceName As String = query.DataSourceName.ToString()
			Dim oParametersCollection As New NexusProvider.ParametersCollection
			Dim oParameters As Object(,)
			Dim iCount As Integer = 0
			Dim oParameter(oParametersCollection.Count - 1, 1) As Object
			If e.Parameters IsNot Nothing AndAlso e.Parameters.Count > 0 Then
				For Each param As Microsoft.Reporting.WebForms.ReportParameterInfo In e.Parameters
					If param.Name.ToLower().StartsWith("pm_sp") = False Then
						Dim oParam As New NexusProvider.Parameters
						oParam.ParamNameField = param.Name
						oParam.ParamValueField = param.Values(0)
						oParametersCollection.Add(oParam)
					End If
				Next
			End If

			Dim dsSubReport As New System.Data.DataSet
			dsSubReport = oWebService.CallNamedStoredProcedure(storedProcedureName, oParametersCollection, True)
			e.DataSources.Add(New ReportDataSource("DataSet1", dsSubReport.Tables(0)))
		End Sub
		Private Shared Function ReplaceSplCharacters(ByRef str As String) As String
			Dim illegalChars As Char() = ":~""#%&*<>?/\{}|.".ToCharArray()
			Dim ext As String
			Dim fName As String
			If str.LastIndexOf(".") = -1 Then
				fName = str
				ext = ""
			Else
				ext = str.Substring(str.LastIndexOf("."))
				fName = str.Substring(0, str.LastIndexOf("."))
			End If


			Dim sb As New System.Text.StringBuilder

			For Each ch As Char In fName
				If Array.IndexOf(illegalChars, ch) = -1 Then
					sb.Append(ch)
				End If
			Next
			Return sb.ToString() & IIf(ext.Length > 1, ext, "")
		End Function

		Private Sub CheckForSession()
			oWebService = New NexusProvider.ProviderManager().Provider
			Dim oAccountDetails As New NexusProvider.AccountDetails
			Dim oAccountSearchCriteria As New NexusProvider.AccountSearchCriteria
			Dim oAccountSearchResultColl As NexusProvider.AccountSearchResultCollection

			''   FillPaymentGroup() 'Reset The Payment Group
			If txtAccountCode.Text.Trim.Contains("%") Then
				IsFound.IsValid = False
				Exit Sub
			End If
			oAccountSearchCriteria.ShortCode = txtAccountCode.Text.Trim
			oAccountSearchCriteria.IncludeInsurerAgents = False
			oAccountSearchCriteria.ExcludeInsurerAgents = False
			oAccountSearchResultColl = oWebService.FindAccounts(oAccountSearchCriteria)
			If oAccountSearchResultColl IsNot Nothing Then
				If oAccountSearchResultColl.Count > 0 Then
					hiddenAccountCode.Value = oAccountSearchResultColl(0).AccountKey
					oAccountDetails.AccountKey = hiddenAccountCode.Value
					rblViewby.Items.FindByValue("AC").Text = GetLocalResourceObject("lbl_AccountCurrency")
					rblViewby.Items.FindByValue("AC").Text &= "(" & oAccountSearchResultColl(0).CurrencyCode & ")"
					Session(CNAccountkey) = hiddenAccountCode.Value
					hPartyKey.Value = oAccountSearchResultColl(0).PartyKey
					Session(CNPartyKey) = oAccountSearchResultColl(0).PartyKey
					Session(CNAccountName) = txtAccountCode.Text.Trim()
					Session(CNCurrency) = oAccountSearchResultColl(0).CurrencyCode.Trim
				Else
					IsFound.IsValid = False
					SetControlsDefaultValues()
					Exit Sub
				End If
			End If
		End Sub
		Protected Sub btnWriteOff_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hypWriteOff.Click
		End Sub

		Protected Sub AddWriteOffAmount()
			Dim WriteOffAmount As Decimal = CDec(Session(CNWriteOffAmount))
			Dim sDocumentRef As String = Session(CNDocumentRef)
			Dim iTransDetailId As Integer
			Dim oAccountDetail As NexusProvider.AccountDetails
			Dim oAccountCollection As NexusProvider.AccountDetailsCollection = CType(Cache.Item(ViewState("ManupulatedGridResultpageCacheID")), NexusProvider.AccountDetailsCollection)

			If (oAccountCollection IsNot Nothing) Then
				For iCount As Integer = 0 To oAccountCollection.Count - 1
					If (oAccountCollection(iCount).DocumentRef.Trim.ToUpper = sDocumentRef.Trim.ToUpper) Then
						oAccountDetail = oAccountCollection.Item(iCount)
					End If
				Next

				Dim oWriteOffRequest As NexusProvider.Writeoff = New NexusProvider.Writeoff
				oWriteOffRequest.AccountKey = Session(CNAccountkey)
				oWriteOffRequest.DocumentKey = oAccountDetail.DocumentId
				oWriteOffRequest.WriteOffAmount = WriteOffAmount
				Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
				iTransDetailId = oWebService.AddWriteoff(oWriteOffRequest, oAccountDetail.TransdetailId, HttpContext.Current.Session(CNBranchCode))
			End If

			Cache.Remove(ViewState("AccountResultpageCacheID"))
			Cache.Remove(ViewState("ManupulatedGridResultpageCacheID"))
			Cache.Remove(ViewState("OutStandingGridResultpageCacheID"))
		End Sub

		Protected Sub RemoveWriteOff()
			Dim oAccountDetailCollection As NexusProvider.AccountDetailsCollection = CType(Cache.Item(ViewState("AccountResultpageCacheID")), NexusProvider.AccountDetailsCollection)
			Dim oOutstandingAccountDetails As NexusProvider.AccountDetailsCollection = CType(Cache.Item(ViewState("OutStandingGridResultpageCacheID")), NexusProvider.AccountDetailsCollection)
			Dim oUpperGridAccountDetailsCollection As NexusProvider.AccountDetailsCollection = CType(Cache.Item(ViewState("ManupulatedGridResultpageCacheID")), NexusProvider.AccountDetailsCollection)
			Dim oMarkUnmark As NexusProvider.MarkUnmarkTransaction
			Dim sDocumentRef As String = Session(CNDocumentRef)
			Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

			If ((ViewState("WriteOffTransactionGridName") = "chkMarked" OrElse ViewState("WriteOffTransactionGridName") = "chkOTSelectAll" OrElse ViewState("WriteOffTransactionGridName") = "chkMarkedOutTran") AndAlso hdnWriteOff.Value.Trim.ToUpper = "TRUE") Then
				For iCount As Integer = 0 To oAccountDetailCollection.Count - 1
					If oAccountDetailCollection(iCount).DocumentRef.Trim().ToUpper = sDocumentRef.ToString.Trim.ToUpper Then

						'Un Mark Child Record
						For jCount As Integer = 0 To oOutstandingAccountDetails.Count - 1
							If oAccountDetailCollection(iCount).DocumentRef.Trim().ToUpper = oOutstandingAccountDetails(jCount).DocumentRef.Trim().ToUpper Then

								If oOutstandingAccountDetails(jCount).IsSelected = True And oOutstandingAccountDetails(jCount).Spare = "WRITEOFF" Then
									oMarkUnmark = New NexusProvider.MarkUnmarkTransaction
									oMarkUnmark.CurrencyCode = oOutstandingAccountDetails(jCount).CurrencyCode.Trim()
									oMarkUnmark.MarkStatus = NexusProvider.MarkStatusType.UnMark
									oMarkUnmark.PaymentAmount = CType("0.00", Decimal)
									oMarkUnmark.TransactionKey = oOutstandingAccountDetails(jCount).TransdetailId
									oWebService.MarkUnmarkTransaction(oMarkUnmark)
								End If
							End If
						Next
						Exit For
					End If
				Next
			ElseIf (ViewState("WriteOffTransactionGridName") = "chkInsurerPaymentsSelectAll") Then
				If (hdnWriteOff.Value.Trim.ToUpper = "TRUE") Then
					For icount As Integer = 0 To oAccountDetailCollection.Count - 1
						If (oAccountDetailCollection(icount).Spare = "WRITEOFF") Then
							oMarkUnmark = New NexusProvider.MarkUnmarkTransaction
							oMarkUnmark.CurrencyCode = oAccountDetailCollection(icount).CurrencyCode.Trim()
							oMarkUnmark.TransactionKey = oAccountDetailCollection(icount).TransdetailId
							oMarkUnmark.MarkStatus = NexusProvider.MarkStatusType.UnMark
							oMarkUnmark.PaymentAmount = CType("0.00", Decimal)
							oWebService.MarkUnmarkTransaction(oMarkUnmark)
						End If
					Next
				End If
			End If

			Cache.Remove(ViewState("AccountResultpageCacheID"))
			Cache.Remove(ViewState("ManupulatedGridResultpageCacheID"))
			Cache.Remove(ViewState("OutStandingGridResultpageCacheID"))
			GridDataBind()
			PopulateOutstandingTransaction()
		End Sub

		Protected Sub txtAccountCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
			oWebService = New NexusProvider.ProviderManager().Provider
			Dim oAccountSearchCriteria As New NexusProvider.AccountSearchCriteria
			Dim oAccountSearchResultColl As NexusProvider.AccountSearchResultCollection

			''   FillPaymentGroup() 'Reset The Payment Group
			If txtAccountCode.Text.Trim.Contains("%") Then
				IsFound.IsValid = False
				Exit Sub
			End If
			If txtDateTo.Text.Trim = "" Then
				txtDateTo.Text = Date.Now.ToShortDateString
			End If
			oAccountSearchCriteria.ShortCode = txtAccountCode.Text.Trim
			oAccountSearchCriteria.IncludeInsurerAgents = False
			oAccountSearchCriteria.ExcludeInsurerAgents = False
			oAccountSearchResultColl = oWebService.FindAccounts(oAccountSearchCriteria)
			If oAccountSearchResultColl IsNot Nothing Then
				If oAccountSearchResultColl.Count > 0 Then
					hiddenAccountCode.Value = oAccountSearchResultColl(0).AccountKey
					hdnGrossAgent.Value = oAccountSearchResultColl(0).IsGrossAgent
					hdnLedgerCode.Value = oAccountSearchResultColl(0).LedgerCode
					rblViewby.Items.FindByValue("AC").Text = GetLocalResourceObject("lbl_AccountCurrency")
					rblViewby.Items.FindByValue("AC").Text &= "(" & oAccountSearchResultColl(0).CurrencyCode & ")"
					Session(CNAccountkey) = hiddenAccountCode.Value
					hPartyKey.Value = oAccountSearchResultColl(0).PartyKey
					Session(CNPartyKey) = oAccountSearchResultColl(0).PartyKey
					Session(CNAccountName) = txtAccountCode.Text.Trim()
					Session(CNCurrency) = oAccountSearchResultColl(0).CurrencyCode.Trim
				Else
					IsFound.IsValid = False
					SetControlsDefaultValues()
					Exit Sub
				End If
			Else
				SetControlsDefaultValues()
				IsFound.IsValid = False
				Exit Sub
			End If

			If (hdnLedgerCode.Value.Trim.ToUpper = "AG") Then
				If (hdnGrossAgent.Value.Trim.ToUpper = "0") Then
					rblCommission.Items(0).Selected = False
					rblCommission.Items(1).Selected = True
				Else
					rblCommission.Items(0).Selected = True
					rblCommission.Items(1).Selected = False
				End If
			ElseIf (txtAccountCode.Text.Trim <> "") Then
				Dim sValue As String = hdnLedgerCode.Value.Trim.ToUpper
				ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "RemoveComission('" & sValue & "');", True)
			End If
		End Sub

		Protected Sub rblCommission_SelectedIndexChanged(sender As Object, e As EventArgs)
			Dim oUpperGridAccountDetailsCollection As NexusProvider.AccountDetailsCollection = CType(Cache.Item(ViewState("ManupulatedGridResultpageCacheID")), NexusProvider.AccountDetailsCollection)
			Dim oAllAccountDetailsCollection As NexusProvider.AccountDetailsCollection = CType(Cache.Item(ViewState("AccountResultpageCacheID")), NexusProvider.AccountDetailsCollection)
			Dim oMarkUnMarkTransaction As NexusProvider.MarkUnmarkTransaction
			Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
			Dim sDocumentRef As String = ""
			Dim iWriteOffCount As Integer = 0

			If (oAllAccountDetailsCollection IsNot Nothing AndAlso oUpperGridAccountDetailsCollection IsNot Nothing) Then
				For iCount As Integer = 0 To oUpperGridAccountDetailsCollection.Count - 1
					'Un Mark All Child Record
					If oUpperGridAccountDetailsCollection(iCount).TotalMarkedAmount <> 0 Then
						For jCount As Integer = 0 To oAllAccountDetailsCollection.Count - 1
							If oUpperGridAccountDetailsCollection(iCount).DocumentRef.Trim.ToUpper = oAllAccountDetailsCollection(jCount).DocumentRef.Trim.ToUpper AndAlso oAllAccountDetailsCollection(jCount).Spare <> "WRITEOFF" Then
								oMarkUnMarkTransaction = New NexusProvider.MarkUnmarkTransaction
								oMarkUnMarkTransaction.CurrencyCode = oAllAccountDetailsCollection(jCount).CurrencyCode.Trim()
								oMarkUnMarkTransaction.TransactionKey = oAllAccountDetailsCollection(jCount).TransdetailId
								oMarkUnMarkTransaction.MarkStatus = NexusProvider.MarkStatusType.UnMark
								oMarkUnMarkTransaction.PaymentAmount = CType("0.00", Decimal)
								oWebService.MarkUnmarkTransaction(oMarkUnMarkTransaction)
							End If
						Next
					End If
				Next

				For iCount As Integer = 0 To oAllAccountDetailsCollection.Count - 1
					If (oAllAccountDetailsCollection(iCount).Spare = "WRITEOFF") Then
						iWriteOffCount = iWriteOffCount + 1
						sDocumentRef = sDocumentRef & "  " & oAllAccountDetailsCollection(iCount).DocumentRef.Trim()
					End If
				Next

				If iWriteOffCount = 1 Then
					Dim sWriteOffMessage As String = GetLocalResourceObject("RemoveWriteOffMessage").ToString
					ViewState("WriteOffTransactionGridName") = "chkInsurerPaymentsSelectAll"
					ScriptManager.RegisterStartupScript(Me, Page.GetType, "script", "WriteOff('" & sWriteOffMessage & "');", True)
					Exit Sub
				ElseIf iWriteOffCount > 1 Then
					Dim sWriteOffMessage As String = GetLocalResourceObject("RemoveWriteOffMessage").ToString
					sWriteOffMessage = sWriteOffMessage & sDocumentRef
					ViewState("WriteOffTransactionGridName") = "chkInsurerPaymentsSelectAll"
					ScriptManager.RegisterStartupScript(Me, Page.GetType, "script", "WriteOff('" & sWriteOffMessage & "');", True)
					Exit Sub
				End If

				'Refresh the grid
				Cache.Remove(ViewState("AccountResultpageCacheID"))
				Cache.Remove(ViewState("ManupulatedGridResultpageCacheID"))
				Cache.Remove(ViewState("OutStandingGridResultpageCacheID"))
				GridDataBind()
				PopulateOutstandingTransaction()
			End If

		End Sub
		Private Sub RefreshUnallocatedAmount()
			Dim cUnAllocatedAmount As Decimal

			Dim cReceiptAmount As Decimal = 0
			If Trim(txtReceiptAmount.Text) <> "" Then
				cReceiptAmount = Convert.ToDecimal(txtReceiptAmount.Text)
			End If

			If cReceiptAmount < 0 Then
				txtReceiptAmount.Text = ""
			End If

			If Trim(txtReceiptAmount.Text) = "" Then
				cUnAllocatedAmount = -1 * Convert.ToDecimal(txtTotalMarked.Text)
			Else
				cUnAllocatedAmount = (Convert.ToDecimal(txtReceiptAmount.Text) - Convert.ToDecimal(txtTotalMarked.Text))
			End If

			If cUnAllocatedAmount <> 0 Then
				txtUnallocatedAmount.Text = cUnAllocatedAmount
			Else
				txtUnallocatedAmount.Text = "0.00"
			End If

			If Trim(txtReceiptAmount.Text) <> "" Then
				Session(CNTotalAmount) = Convert.ToDecimal(txtTotalMarked.Text) + Convert.ToDecimal(txtUnallocatedAmount.Text)
			Else
				Session(CNTotalAmount) = Convert.ToDecimal(txtTotalMarked.Text)
			End If
		End Sub
		Private Sub txtReceiptAmount_TextChanged(sender As Object, e As EventArgs) Handles txtReceiptAmount.TextChanged

			RefreshUnallocatedAmount()
		End Sub
	End Class
End Namespace
