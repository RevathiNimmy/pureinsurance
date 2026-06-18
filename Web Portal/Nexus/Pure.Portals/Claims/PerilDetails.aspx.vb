Imports System.Configuration.ConfigurationManager
Imports NexusProvider.SAMForInsurance
Imports Nexus
Imports Nexus.Utils
Imports Nexus.Constants.Constant
Imports System.Linq
Imports NexusProvider
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Library
Imports CMS.Library
Imports System.Xml.XmlReader
Imports System.Xml.XPath
Imports System.Xml
Namespace Nexus
    Partial Class Claims_PerilDetails
        Inherits BasePeril
        Private sJScriptDisableCalculate As String
        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim m_sIsPaymentsReadOnly As String = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsPaymentsReadOnly, NexusProvider.RiskTypeOptions.None, Session(CNProductCode), Nothing)
            If Not IsPostBack Then
                If Session(CNMode) = Mode.PayClaim AndAlso m_sIsPaymentsReadOnly = "1" Then
                    hfRememberTabs.Value = 2
                Else
                    hfRememberTabs.Value = 1
                End If
            End If

            ' Clear CNQuote on non-instalment postbacks so WriteContainerToXML (Next button)
            ' enters the correct claims branch. CNQuote was set for the Instalments control.
            If IsPostBack AndAlso Session(CNQuote) IsNot Nothing AndAlso _
               (Session(CNMode) = Mode.SalvageClaim OrElse Session(CNMode) = Mode.TPRecovery) Then
                Dim sEventTarget As String = Page.Request("__EVENTTARGET")
                Dim bIsInstalmentPostback As Boolean = (sEventTarget IsNot Nothing AndAlso _
                   (sEventTarget.ToLower().Contains("instalment") OrElse _
                    sEventTarget.ToLower().Contains("installment") OrElse _
                    sEventTarget.Contains("ddlDayinMonth") OrElse _
                    sEventTarget.Contains("ddlFirstPaymentDate") OrElse _
                    sEventTarget.Contains("ddlAccountType") OrElse _
                    sEventTarget.Contains("chkUseTransactionCurrency")))
                If Not bIsInstalmentPostback Then
                    Session.Remove(CNQuote)
                End If
            End If

            ' Show Instalments panel in ReserveAndRecovery control if product config enabled
            If Session(CNMode) = Mode.SalvageClaim OrElse Session(CNMode) = Mode.TPRecovery Then
                SetInstalmentsTabVisibility(oWebservice)
            End If
        End Sub

        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                'if page is loaded first time then setting of the status of progres bar
                ucProgressBar.OverviewStyle = "complete"
                ucProgressBar.PerilsStyle = "in-progress"
                ucProgressBar.SummaryStyle = "incomplete"
                ucProgressBar.ReinsuranceStyle = "incomplete"
                ucProgressBar.CompleteStyle = "incomplete"

                If Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                    liTabPaymentDetails.Text = GetLocalResourceObject("liReceiptDetails")
                Else
                    liTabPaymentDetails.Text = GetLocalResourceObject("lbl_TabPaymentDetails")
                End If

                If CType(Session.Item(CNMode), Mode) = Mode.NewClaim Or CType(Session.Item(CNMode), Mode) = Mode.EditClaim Or CType(Session.Item(CNMode), Mode) = Mode.ViewClaim Then
                    hfRememberTabs.Value = "3"
                End If

                If Session(CNMode) = Mode.SalvageClaim OrElse Session(CNMode) = Mode.TPRecovery Then
                    liTabPaymentDetails.Text = GetLocalResourceObject("lbl_TabReceiptDetails")
                End If
            End If

            Dim rblPayee As RadioButtonList = CType(PayClaim_ctrl.FindControl("rblPayee"), RadioButtonList)
            Dim txtParty As TextBox = CType(PayClaim_ctrl.FindControl("txtParty"), TextBox)
            If Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Or Session(CNMode) = Mode.PayClaim Then
                If rblPayee.SelectedValue = "1" And txtParty.Text IsNot Nothing And txtParty.Text <> "" Then
                    hfRememberTabs.Value = "2"
                End If
            End If
        End Sub

        ''' <summary>
        ''' To Validate all preconditions for WPR85 - Automatic cashlist generation for Salavage and TP Recovery
        ''' </summary>
        Protected Sub cvMediaTypeAndDefaultBankAccountForReciept_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvMediaTypeAndDefaultBankAccountForReciept.ServerValidate

            ' Skip receipt validation when Instalments tab is active (This Receipt is hidden per FR-003)
            Dim liInst As Control = PayClaim_ctrl.FindControl("liInstalments")
            If liInst IsNot Nothing AndAlso liInst.Visible Then
                args.IsValid = True
                Exit Sub
            End If

            If Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim sReturnCode As NexusProvider.OptionTypeSetting
                Dim oQuote As NexusProvider.Quote = CType(Session(CNClaimQuote), NexusProvider.Quote)
                Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)

                ''Pass system option for "Automate receipt generation for Salvage/Third Party receipt"
                sReturnCode = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5117)

                If sReturnCode IsNot Nothing AndAlso sReturnCode.OptionValue IsNot Nothing Then
                    If sReturnCode.OptionValue = "1" Then
                        Dim sMediaTypeValue As String = CType(CType(CType(PayClaim_ctrl, Controls_PayClaim).FindControl("GISLookup_MediaType"), NexusProvider.LookupList), NexusProvider.LookupList).Value
                        If String.IsNullOrEmpty(sMediaTypeValue) Then
                            args.IsValid = False
                            cvMediaTypeAndDefaultBankAccountForReciept.ErrorMessage = GetLocalResourceObject("vld_MediaTypeRequired").ToString()
                            Exit Sub
                        End If

                        'Get MediaTypeKey from Code
                        Dim sMediaTypeKey As String = GetCodeForKey(NexusProvider.ListType.PMLookup, sMediaTypeValue, "MediaType", False)
                        'Find Bank Account Defaults for Reciepts
                        Dim oBankAccountDefaults As New NexusProvider.BankAccountDefaults
                        oBankAccountDefaults = oWebService.GetDefaultBankAccountWithCurrency(oQuote.ProductCode, Convert.ToInt32(sMediaTypeKey), 2, oQuote.BranchCode)
                        If oBankAccountDefaults.Count = 0 Then
                            args.IsValid = False
                            cvMediaTypeAndDefaultBankAccountForReciept.ErrorMessage = GetLocalResourceObject("vld_NoDefaultBankAccount").ToString()
                            Exit Sub
                        End If


                        Dim iCurrencyMatches As Integer = 0
                        For iCt As Integer = 0 To oBankAccountDefaults.Count - 1
                            If oBankAccountDefaults(iCt).CurrencyCode.Trim.ToUpper = oClaimOpen.CurrencyISOCode.Trim.ToUpper Then
                                iCurrencyMatches += 1
                            End If
                        Next

                        If iCurrencyMatches > 1 Then
                            args.IsValid = False
                            cvMediaTypeAndDefaultBankAccountForReciept.ErrorMessage = GetLocalResourceObject("vld_MoreThanOneDefaultBankAccountForCurrency").ToString()
                            Exit Sub
                        End If

                        If iCurrencyMatches = 0 Then
                            args.IsValid = False
                            cvMediaTypeAndDefaultBankAccountForReciept.ErrorMessage = GetLocalResourceObject("vld_NoDefaultBankAccountForCurrency").ToString()
                            Exit Sub
                        End If

                        Dim oCashListItemReceiptTypes As NexusProvider.LookupListCollection
                        Dim iCashListItemRecieptTypeForClaim As Integer
                        Dim v_sOptionList As System.Xml.XmlElement = Nothing
                        oCashListItemReceiptTypes = oWebService.GetList(NexusProvider.ListType.PMLookup, "CashListItem_Receipt_Type", True, False, , , , v_sOptionList)

                        If oCashListItemReceiptTypes IsNot Nothing Then
                            If oCashListItemReceiptTypes.Cast(Of LookupListItem)().Any(Function(oCashListItemRecieptType) oCashListItemRecieptType.Code.Trim.ToUpper() = "CLAIMRPT") Then
                                iCashListItemRecieptTypeForClaim = iCashListItemRecieptTypeForClaim + 1
                            End If
                        End If
                        If iCashListItemRecieptTypeForClaim = 0 Then
                            args.IsValid = False
                            cvMediaTypeAndDefaultBankAccountForReciept.ErrorMessage = GetLocalResourceObject("vld_NoClaimRecieptType").ToString()
                            Exit Sub
                        End If
                    End If
                End If
            End If
        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If IsPostBack AndAlso (Session(CNMode) = Mode.SalvageClaim OrElse Session(CNMode) = Mode.TPRecovery) Then
                hfRememberTabs.Value = "2"

                ' Keep Instalments child tab active on postback if it was already shown
                Dim liInst As Control = PayClaim_ctrl.FindControl("liInstalments")
                If liInst IsNot Nothing AndAlso liInst.Visible Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "KeepInstalmentsTab",
                        "$(document).ready(function(){ setTimeout(function(){ $('a[href=""#tab-thispayment""]').closest('li').hide(); $('#liPaymentDetail a').tab('show'); setTimeout(function(){ $('[href=""#tab-recovery-instalments""]').tab('show'); }, 200); }, 100); });", True)
                End If

                Dim hasValidationErrors As Boolean = Page.Validators.Cast(Of System.Web.UI.IValidator)().Any(Function(validator) Not validator.IsValid)

                If hasValidationErrors Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ScrollToReceipt", "$(document).ready(function() { setTimeout(function() { $('a[href=""#tab-thispayment""]').tab('show').addClass('active').attr('aria-selected', 'true'); $('a[href=""#tab-details""]').removeClass('active').attr('aria-selected', 'false'); $('#tab-thispayment').addClass('show active'); $('#tab-details').removeClass('show active'); }, 50); });", True)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Shows the Instalments button and tab inside PayClaim control if product config enabled.
        ''' </summary>
        Private Sub SetInstalmentsTabVisibility(ByVal oWebservice As NexusProvider.ProviderBase)
            Try
                Dim sEnabled As String = oWebservice.GetProductRiskOptionValue(
                    NexusProvider.ProductConfigActionType.ProductRiskMaintenance,
                    NexusProvider.ProductRiskOptions.RecoveryInstalmentsEnabled,
                    NexusProvider.RiskTypeOptions.None,
                    Session(CNProductCode), Nothing)
                If sEnabled = "1" Then
                    btnInstalments.Visible = True
                End If
            Catch
            End Try
        End Sub

        ''' <summary>
        ''' btnInstalments_Click — shows the Instalments tab in PayClaim and activates it.
        ''' </summary>
        Protected Sub btnInstalments_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInstalments.Click
            ' FR-013: Block if recovery transaction already has an active instalment plan
            ' Check via existing finance plan for the claim's insurance file key
            Try
                Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oClaimQuote As NexusProvider.Quote = CType(Session(CNClaimQuote), NexusProvider.Quote)
                If oClaimQuote IsNot Nothing AndAlso oClaimQuote.InsuranceFileKey > 0 Then
                    Dim oExistingPlan As NexusProvider.FinancePlan = oWebservice.GetFinancePlanDetails(oClaimQuote.InsuranceFileKey)
                    If oExistingPlan IsNot Nothing AndAlso Not String.IsNullOrEmpty(oExistingPlan.PlanReference) AndAlso
                       oExistingPlan.StatusDescription IsNot Nothing AndAlso
                       oExistingPlan.StatusDescription.Trim().ToUpper() <> "CANCELLED" AndAlso
                       oExistingPlan.StatusDescription.Trim().ToUpper() <> "COMPLETED" Then
                        Dim lblMsg As Label = CType(PayClaim_ctrl.FindControl("lblInstalmentMessage"), Label)
                        If lblMsg IsNot Nothing Then
                            lblMsg.Text = "An active instalment plan already exists for this recovery transaction."
                            lblMsg.Visible = True
                        End If
                        Exit Sub
                    End If
                End If
            Catch
            End Try

            ' Get the receipt amount - try txtGrossPayment first, then fall back to claim recovery reserve
            Dim dAmount As Double = 0
            Dim txtGrossPayment As TextBox = CType(PayClaim_ctrl.FindControl("txtGrossPayment"), TextBox)
            If txtGrossPayment IsNot Nothing AndAlso Not String.IsNullOrEmpty(txtGrossPayment.Text) Then
                Double.TryParse(txtGrossPayment.Text.Replace(",", ""), dAmount)
            End If

            ' If still 0, get from claim quote GrossTotal (same as PremiumDisplay pattern)
            If dAmount = 0 Then
                Dim oQuote As NexusProvider.Quote = CType(Session(CNClaimQuote), NexusProvider.Quote)
                If oQuote IsNot Nothing AndAlso oQuote.GrossTotal <> 0 Then
                    dAmount = Math.Abs(oQuote.GrossTotal)
                End If
            End If

            ' Block if receipt amount is zero — user must enter a receipt amount first
            If dAmount = 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "ZeroAmountAlert",
                    "alert('Please enter a receipt amount before creating an instalment plan.');", True)
                Exit Sub
            End If

            ' Set required session values (same pattern as PremiumDisplay line 1227)
            Session(CNAmountToPay) = dAmount
            ' Clear CNSelectedPaymentIndex to avoid null ref in bindInstalments payment type lookup
            ' bindInstalments will proceed via the CNAmountToPay <> 0 condition
            Session.Remove(CNSelectedPaymentIndex)

            ' Ensure Session(CNParty) is set — bindInstalments uses it for bank details
            If Session(CNParty) Is Nothing Then
                Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
                If oClaimOpen IsNot Nothing AndAlso oClaimOpen.Client IsNot Nothing AndAlso oClaimOpen.Client.PartyKey > 0 Then
                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Session(CNParty) = oWebService.GetParty(oClaimOpen.Client.PartyKey)
                End If
            End If

            ' Ensure CNQuote is set — bindInstalments reads quote for InsuranceFileKey/BranchCode.
            ' For recovery, we must ensure the Quote has valid Risks collection (even if empty)
            ' because ShowDetailsForScheme iterates Risks for tax/fee exclusion calculations.
            ' Also ensure CoverStartDate/CoverEndDate are set (used by GetInstalmentQuotes).
            If Session(CNQuote) Is Nothing AndAlso Session(CNClaimQuote) IsNot Nothing Then
                Session(CNQuote) = Session(CNClaimQuote)
            End If
            Dim oQuoteForSetup As NexusProvider.Quote = CType(Session(CNQuote), NexusProvider.Quote)
            If oQuoteForSetup IsNot Nothing Then
                If oQuoteForSetup.Risks Is Nothing Then
                    oQuoteForSetup.Risks = New NexusProvider.RiskCollection()
                End If
                If oQuoteForSetup.CoverStartDate = DateTime.MinValue Then
                    oQuoteForSetup.CoverStartDate = DateTime.Today
                End If
                If oQuoteForSetup.CoverEndDate = DateTime.MinValue Then
                    oQuoteForSetup.CoverEndDate = DateTime.Today.AddYears(1)
                End If
                Session(CNQuote) = oQuoteForSetup
            End If

            ' Set process mode for scheme filtering — SR for Salvage, TPR for Third Party Recovery
            ' This is read by Instalments.ascx.vb CallGetInstalmentQuotes to pass as sProcessPFMode
            If Session(CNMode) = Mode.SalvageClaim Then
                Session("PFProcessMode") = "SR"
            ElseIf Session(CNMode) = Mode.TPRecovery Then
                Session("PFProcessMode") = "TPR"
            End If

            ' Ensure CNAgentType is set — ShowDetailsForScheme uses it for amount calculation
            If Session(CNAgentType) Is Nothing Then
                Session(CNAgentType) = String.Empty
            End If

            ' Set ClaimId for the CalculateQuotes request
            Dim oClaimForId As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            If oClaimForId IsNot Nothing Then
                Session("PFClaimId") = oClaimForId.BaseClaimKey
            End If

            ' Show and bind instalments via PayClaim control
            Dim li As Control = PayClaim_ctrl.FindControl("liInstalments")
            If li IsNot Nothing Then li.Visible = True

            Dim uc As Control = PayClaim_ctrl.FindControl("ucInstalments")
            If uc IsNot Nothing Then
                Try
                    uc.GetType().GetMethod("bindInstalments").Invoke(uc, Nothing)

                    ' FR-012: Filter to only Claim Recovery schemes (SchemeTypeCode = "CR")
                    ' SA/TP rate type filtering is handled by the back-end within CR schemes
                    Dim oGrid As GridView = CType(uc.FindControl("grdInstallmentQuotes"), GridView)
                    If oGrid IsNot Nothing Then
                        Dim oQuotes As NexusProvider.InstallmentQuoteCollection = TryCast(oGrid.DataSource, NexusProvider.InstallmentQuoteCollection)
                        If oQuotes IsNot Nothing AndAlso oQuotes.Count > 0 Then
                            Dim oFilteredQuotes As New NexusProvider.InstallmentQuoteCollection
                            For i As Integer = 0 To oQuotes.Count - 1
                                If oQuotes(i).SchemeTypeCode IsNot Nothing AndAlso
                                   oQuotes(i).SchemeTypeCode.Trim().ToUpper() = "CR" Then
                                    oFilteredQuotes.Add(oQuotes(i))
                                End If
                            Next
                            oGrid.DataSource = oFilteredQuotes
                            oGrid.DataBind()

                            ' Update the cache with filtered quotes so ShowDetailsForScheme finds them on selection
                            uc.GetType().GetMethod("UpdateInstalmentQuotesCache").Invoke(uc, New Object() {oFilteredQuotes})

                            ' Auto-select first scheme and show details.
                            ' Ensure ddlDayinMonth and ddlFirstPaymentDate have default values
                            ' since ShowDetailsForScheme uses them if cache misses.
                            If oFilteredQuotes.Count > 0 Then
                                oGrid.SelectedIndex = 0

                                ' Populate ddlDayinMonth if empty (ShowDetailsForScheme reads it)
                                Dim ddlDay As DropDownList = CType(uc.FindControl("ddlDayinMonth"), DropDownList)
                                If ddlDay IsNot Nothing AndAlso ddlDay.Items.Count = 0 Then
                                    For iDay As Integer = 1 To 28
                                        ddlDay.Items.Add(New ListItem(iDay.ToString(), iDay.ToString()))
                                    Next
                                    ddlDay.SelectedIndex = 0
                                End If

                                ' Populate ddlFirstPaymentDate if empty
                                Dim ddlFirstPay As DropDownList = CType(uc.FindControl("ddlFirstPaymentDate"), DropDownList)
                                If ddlFirstPay IsNot Nothing AndAlso ddlFirstPay.Items.Count = 0 Then
                                    Dim dtStart As DateTime = DateTime.Today
                                    For iCount As Integer = 0 To 30
                                        ddlFirstPay.Items.Add(New ListItem(dtStart.AddDays(iCount).ToShortDateString(), dtStart.AddDays(iCount).ToShortDateString()))
                                    Next
                                    ddlFirstPay.SelectedIndex = 0
                                End If

                                uc.GetType().GetMethod("ShowDetailsForSelectedScheme").Invoke(uc, New Object() {oFilteredQuotes(0).SchemeNo, oFilteredQuotes(0).SchemeVersion, oFilteredQuotes(0).CompanyNo, oFilteredQuotes(0).FrequencyID})

                                ' Ensure plan summary panel is visible after showing details
                                Dim pnlSummary As Panel = CType(uc.FindControl("pnlPlanSummary"), Panel)
                                If pnlSummary IsNot Nothing Then pnlSummary.Visible = True

                                ' Ensure SelectedInstalmentQuote cache is populated.
                                ' If ShowDetailsForScheme failed to reach the cache insert line,
                                ' manually write the first filtered quote to the cache.
                                Dim oSelProp As System.Reflection.PropertyInfo = uc.GetType().GetProperty("SelectedInstalmentQuote")
                                If oSelProp IsNot Nothing Then
                                    Dim oCurrentSelection As NexusProvider.InstalmentQuote = TryCast(oSelProp.GetValue(uc, Nothing), NexusProvider.InstalmentQuote)
                                    If oCurrentSelection Is Nothing Then
                                        ' Manually insert into the control's cache via reflection on ViewState
                                        Dim vsProp As System.Reflection.PropertyInfo = uc.GetType().GetProperty("ViewState", System.Reflection.BindingFlags.NonPublic Or System.Reflection.BindingFlags.Instance)
                                        If vsProp IsNot Nothing Then
                                            Dim ucVS As System.Web.UI.StateBag = CType(vsProp.GetValue(uc, Nothing), System.Web.UI.StateBag)
                                            If ucVS IsNot Nothing AndAlso ucVS("SelectedInstalmentQuoteCacheId") IsNot Nothing Then
                                                HttpContext.Current.Cache.Insert(ucVS("SelectedInstalmentQuoteCacheId").ToString(), oFilteredQuotes(0), Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If

                Catch ex As System.Reflection.TargetInvocationException
                    Dim lblMsg As Label = CType(PayClaim_ctrl.FindControl("lblInstalmentMessage"), Label)
                    If lblMsg IsNot Nothing Then
                        lblMsg.Text = "Error loading instalment schemes: " & If(ex.InnerException IsNot Nothing, ex.InnerException.Message, ex.Message)
                        lblMsg.Visible = True
                    End If
                End Try
            End If

            ' Activate the Payment Details tab then Instalments child tab, hiding This Receipt tab
            hfRememberTabs.Value = "2"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "ActivateInstalmentsTab",
                "$(document).ready(function(){ setTimeout(function(){ $('#liPaymentDetail a').tab('show'); setTimeout(function(){ $('a[href=""#tab-thispayment""]').closest('li').hide(); $('[href=""#tab-recovery-instalments""]').tab('show'); }, 200); }, 100); });", True)
        End Sub

    End Class
End Namespace
