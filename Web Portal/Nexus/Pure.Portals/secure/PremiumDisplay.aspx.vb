Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Utils
Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Xml.XPath
Imports System.Data
Imports System.Linq

Namespace Nexus

    Partial Class secure_PremiumDisplay : Inherits Frontend.clsCMSPage
        'This is the status of the renewal Waiting Status in Back office
        Const sAwaiting_Manual_Preview = "Awaiting Manual Review"
        Const sAwaiting_Renewal_Notice = "Awaiting Renewal Notice Print"
        Const sAwaiting_Update = "Awaiting Update"
        'Added code for   wpr35 Writte
        Const sWrittenAwaiting_Update = "Written - Awaiting Update"
        Protected sConfirmPolicyUnderRenewal As String

        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())

        ' declaring local variables for getting UserAuthority
        Dim bUserInvoice As Boolean = False
        Dim bUserPayNow As Boolean = False
        Dim bUserBankGuarantee As Boolean = False
        Dim bUserCashDeposit As Boolean = False
        Dim bUserDirectDebit As Boolean = False

        ' declaring local variables for getting Product Payment options
        Dim bProductInvoice As Boolean = False
        Dim bProductPayNow As Boolean = False
        Dim bProductBankGuarantee As Boolean = False
        Dim bProductCashDeposit As Boolean = False
        Dim bProductDirectDebit As Boolean = False

        ' declaring local variables for getting Agent Payment options
        Dim bAgentInvoice As Boolean = False
        Dim bAgentPayNow As Boolean = False
        Dim bAgentBankGuarantee As Boolean = False
        Dim bAgentCashDeposit As Boolean = False
        Dim bAgentDirectDebit As Boolean = False

        Dim sProductCode As String = Nothing
        Dim bCommAmendSessionHasValue As Boolean = False
        Dim oPaymentHubEnabled As NexusProvider.OptionTypeSetting
        Dim oRefundPremiumThroughInvoice As NexusProvider.OptionTypeSetting
        Dim bIsTrueMonthlypolicyandNextInstalmentRenewal As Boolean = False

        Dim oQuote As NexusProvider.Quote
        Protected Property SelectedPaymentOptionType As String = String.Empty

        Private Function GetPolicyDiscountCtrl() As Nexus.Controls_PolicyDiscount
            Return TryCast(ctrlPolicyDiscount, Nexus.Controls_PolicyDiscount)
        End Function

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "WriteConfirmation",
                               "<script language=""JavaScript"" type=""text/javascript"">function WriteConfirmation(){var IsConfirm; IsConfirm=confirm('" & GetLocalResourceObject("msg_WriteConfirmation").ToString() & "');document.getElementById('" & hdWriteChoice.ClientID & "').value=IsConfirm;if (IsConfirm == true) {  __doPostBack('', 'WritePolicy');} else {return false;};}</script>")


            If oPortal.UseCorePolicyHeader = False Then
                ucPolicyHeader.Visible = False
            Else
                btnRequote.Visible = False
            End If

            'Load the SummaryCoverControl on page init otherwise view state for Document manager will not get maintained
            Session(CNMtaRenSelResponse) = Nothing
            oQuote = CType(Session(CNQuote), NexusProvider.Quote)
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oProductConfig As Config.Product = oNexusConfig.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
            Dim WebControlPath As String
            WebControlPath = "~/Products/" & oProductConfig.Name & "/SummaryCoverCntlr.ascx"
            If (System.IO.File.Exists(Request.MapPath(WebControlPath))) Then
                ucPolicyDetails.Visible = False
                Dim tempControl As Control = LoadControl(WebControlPath)
                SummaryCoverCntrl.Controls.Clear()
                SummaryCoverCntrl.Controls.Add(tempControl)
                AddHandler GetPolicyDiscountCtrl().ApplyDiscountClicked, AddressOf OnApplyDiscountClicked
            Else
                ucPolicyDetails.Visible = True
            End If
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            oPaymentHubEnabled = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.SystemOptionPaymentHubEnabled)
            oRefundPremiumThroughInvoice = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.RefundPremiumThroughInvoice)
            ViewState("PaymentHubEnabled") = oPaymentHubEnabled.OptionValue
            ViewState("RefundPremiumToInvoice") = oRefundPremiumThroughInvoice.OptionValue
        End Sub

        Private Function GetDiscountTransactionType() As String
            If Session(CNMTAType) IsNot Nothing Then
                Return "MTA"
            ElseIf Session(CNRenewal) IsNot Nothing AndAlso CBool(Session(CNRenewal)) Then
                Return "REN"
            Else
                Return "NB"
            End If
        End Function

        ''' <summary>
        ''' AC: On clicking Apply Discount, apply discount to all risks, recalculate tax/RI.
        ''' AC: Iterative discount — if discount already applied, rollback previous then apply new.
        ''' AC: After success, Apply Discount disabled, Buy enabled, Payment Terms enabled.
        ''' </summary>
        Private Sub OnApplyDiscountClicked(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim oQuoteDiscount As NexusProvider.Quote = Session(CNQuote)
            Dim oWebServiceDiscount As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim sFailureReason As String = String.Empty
            Dim sTransactionType As String = GetDiscountTransactionType()

            ' AC: Iterative discount — rollback previous discount before applying new one.
            ' NB/REN: always rollback — backup values are always current.
            ' MTA: only rollback if POLICY_DISCOUNT_ORIGINAL_TOTAL is in session (no requote
            ' since last Apply). If requote happened, POLICY_DISCOUNT_ORIGINAL_TOTAL is cleared
            ' by btnRequote_Click — skip rollback and apply directly on fresh post-requote values.
            If CBool(If(Session("POLICY_DISCOUNT_APPLIED_TO_RISKS"), False)) Then
                If sTransactionType <> "MTA" OrElse
                   Session("POLICY_DISCOUNT_ORIGINAL_TOTAL") IsNot Nothing Then
                    oWebServiceDiscount.RollbackPolicyDiscount(oQuoteDiscount.InsuranceFileKey, 0, sTransactionType, 0)
                    Session("POLICY_DISCOUNT_APPLIED_TO_RISKS") = False
                    Session(CNPolicyDiscountApplied) = False
                End If
            End If

            Dim bSuccess As Boolean = oWebServiceDiscount.ApplyPolicyDiscount(
                oQuoteDiscount.InsuranceFileKey, 0, sTransactionType, 0, sFailureReason,
                CDec(If(Session(CNPolicyDiscountedPremium), 0D)),
                CDbl(If(Session(CNPolicyDiscountPercentage), 0.0)),
                0,
                CInt(If(Session(CNPolicyDiscountReasonId), 0)),
                CInt(If(Session(CNPolicyDiscountRecurringTypeId), 0)))

            If bSuccess Then
                If GetPolicyDiscountCtrl() IsNot Nothing Then GetPolicyDiscountCtrl().OnDiscountApplied()
                ' Mark that discount was actually applied to risks (for rollback logic)
                Session("POLICY_DISCOUNT_APPLIED_TO_RISKS") = True

                ' Refresh quote from server to get updated premium, taxes, fees after discount
                Dim oRefreshedQuote As NexusProvider.Quote = oWebServiceDiscount.GetHeaderAndSummariesByKey(oQuoteDiscount.InsuranceFileKey)
                For i As Integer = 0 To oRefreshedQuote.Risks.Count - 1
                    oWebServiceDiscount.GetRisk(oRefreshedQuote.Risks(i).Key, i, oRefreshedQuote, oRefreshedQuote.BranchCode)
                Next
                oWebServiceDiscount.GetHeaderAndRisksByKey(oRefreshedQuote)
                ' Preserve risk selection state
                If oQuoteDiscount.Risks IsNot Nothing Then
                    For i As Integer = 0 To Math.Min(oQuoteDiscount.Risks.Count, oRefreshedQuote.Risks.Count) - 1
                        oRefreshedQuote.Risks(i).IsRiskSelected = oQuoteDiscount.Risks(i).IsRiskSelected
                        oRefreshedQuote.Risks(i).IsRisk = oQuoteDiscount.Risks(i).IsRisk
                    Next
                End If
                Session(CNQuote) = oRefreshedQuote
                oQuote = oRefreshedQuote

                ' Redirect to self to refresh all controls with updated amounts
                Response.Redirect(Request.RawUrl, False)
            End If
        End Sub


        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Session(CNQuoteAllProcessComplete) IsNot Nothing AndAlso Session(CNQuoteAllProcessComplete) = True Then
                ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "ProcessComplete", "alert('" & GetLocalResourceObject("QuoteAllProcessComplete") & "');", True)
                Session(CNQuoteAllProcessComplete) = Nothing
            End If
            Session.Remove(CNSelectedAccount)
            Session.Remove(CNIsCancelMTA)
            If Request("__EVENTARGUMENT") = "GetAccount" Then
                Page.Validate()
                RedirectOnBuyNowClick()
            End If

            oQuote = CType(Session(CNQuote), NexusProvider.Quote)


            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oProductConfig As Config.Product = oNexusConfig.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim bDoNotDeleteRenewalQuoteOnMTA As Boolean
            Dim bDeleteRenQuoteReRunRenewal As Boolean


            Dim bIsBackDatedReinstatement = False
            Dim bIsInBackDatedMode As Boolean 'To identify if Editing/Viewing backdated policy version from backdated screen
            Dim bIsDuplicateRenewalExists As Boolean
            Session(CNIsTransactionConfirmationVisited) = Nothing


            bIsInBackDatedMode = IIf(Session(CNBaseInsuranceFileKey) IsNot Nothing AndAlso Session(CNBaseInsuranceFileKey) <> Session(CNInsuranceFileKey), True, False)
            Session.Remove(CNOnlyOriginalRating)
            'This section is used for Backdated MTA - Start

            If Request("__EVENTARGUMENT") = "Refresh" Then
                Page.ClientScript.GetPostBackEventReference(Me, "")
                FrameWorkFunctions.RedirectOnBuyNowClick()
            End If
            'Arch Adjust Premium Feature
            If Request("__EVENTARGUMENT") = "RefreshAdjustPremium" Then
                Page.ClientScript.GetPostBackEventReference(Me, "")
            End If
            If Request("__EVENTARGUMENT") = "RefreshAgentCommission" Then
                Page.ClientScript.GetPostBackEventReference(Me, "")
            End If

            sConfirmPolicyUnderRenewal = GetLocalResourceObject("msg_ConfirmPolicyUnderRenewal")
            bDoNotDeleteRenewalQuoteOnMTA = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.DoNotDeleteRenQuoteOnMTA, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
            bDeleteRenQuoteReRunRenewal = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.DeleteRenQuoteReRunRenewal, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
            If Session(CNBackDatedReinstatement) IsNot Nothing Then
                bIsBackDatedReinstatement = CBool(Session(CNBackDatedReinstatement))
            End If

            If (oQuote.IsPolicyInAnnualRenewal = True AndAlso bDoNotDeleteRenewalQuoteOnMTA = True) AndAlso Session(CNMTAType) IsNot Nothing AndAlso Session(CNMTAType) <> MTAType.CANCELLATION Then
                btnBuy.Attributes.Add("onclick", "RenewalWarning();")
                btnSaveQuote.Attributes.Add("onclick", "RenewalWarning();")
            ElseIf (oQuote.IsPolicyInAnnualRenewal = True AndAlso bDeleteRenQuoteReRunRenewal = True) AndAlso Session(CNMTAType) IsNot Nothing AndAlso Session(CNMTAType) <> MTAType.CANCELLATION Then
                btnBuy.Attributes.Add("onclick", "DeleteRenQuoteReRunRenewal();")
            End If

            'This section is used for Backdated MTA - End

            If Not IsPostBack Then
                Session(CNIsTransactionConfirmationVisited) = Nothing
                If Session(CNCommissionWarning) IsNot Nothing AndAlso Not bCommAmendSessionHasValue Then
                    Session.Remove(CNCommissionWarning)
                End If
                SetPageProgress(4)
                Session(CNIsSummaryVisited) = True 'to make the Finish Button visible
                'Make visible btnWrite button conditionally
                CheckWritePolicy()

                Dim sInsFileTypeCode As String = ""
                If (oQuote IsNot Nothing AndAlso oQuote.InsuranceFileTypeCode IsNot Nothing) Then
                    sInsFileTypeCode = oQuote.InsuranceFileTypeCode.Trim.ToUpper()
                    If Session(CNMode) = Mode.View Then
                        'User has select the view for policy
                        Dim dTotalPremium As Decimal
                        If oQuote.Risks.Count > 0 Then
                            dTotalPremium = oQuote.GrossTotal
                        End If

                        MultiRisk1.Visible = True
                        btnBuy.Visible = False
                        btnSaveQuote.Visible = False
                        btnAddTask.Visible = False

                        If Session(CNQuoteMode) = QuoteMode.MTAQuote AndAlso bIsInBackDatedMode = False Then
                            PanelButton.Visible = False
                        Else
                            If UserCanDoTask("MidTermAdjustment") Then
                                lblMessage.Visible = True
                                lblMessage.Text = GetLocalResourceObject("lbl_ViewPolicy").ToString()
                            End If
                        End If
                        pnlPaymentMethods.Visible = False

                        btnCancel.Visible = False

                    ElseIf Session(CNQuoteMode) = QuoteMode.FullQuote And Session(CNRenewal) Is Nothing Then 'for New Business

                        lblMessage.Visible = True
                        lblMessage.Text = GetLocalResourceObject("lbl_SaveBuyPolicy").ToString()
                        If Not UserCanDoTask("DisableBuyNow") Then 'EH023078
                            MakeLiveVisible()
                            btnBuy.Visible = True
                        Else
                            btnBuy.Visible = False
                        End If
                        btnSaveQuote.Visible = True
                        btnChangePolicy.Visible = False
                        btnCancel.Visible = False
                        'To be Modified
                        lblTotalPremium.Visible = False

                    ElseIf Session(CNQuoteMode) = QuoteMode.MTAQuote Then 'for New Business
                        lblMessage.Visible = True
                        lblMessage.Text = GetLocalResourceObject("lbl_SaveBuyPolicy").ToString()
                        If Not UserCanDoTask("DisableBuyNow") Then 'EH023078
                            btnBuy.Visible = True
                        Else
                            btnBuy.Visible = False
                        End If
                        btnSaveQuote.Visible = True
                        btnChangePolicy.Visible = False
                        btnCancel.Visible = False

                    End If
                End If

                If sInsFileTypeCode = "QUOTE" OrElse sInsFileTypeCode = "RENEWAL" OrElse sInsFileTypeCode = "MTAQUOTE" OrElse sInsFileTypeCode = "MTAQTETEMP" OrElse sInsFileTypeCode = "MTAQREINS" OrElse sInsFileTypeCode = "MTAQCAN" Then
                    lblPageheader.Text = GetLocalResourceObject("lbl_quote_Page_header")
                Else
                    lblPageheader.Text = GetLocalResourceObject("lbl_policy_Page_header")
                End If

                ''When multirisk is not used RiskType in session is nothing, get risk type from Session QUote to set the visibility of requote.
                Dim oRiskQuote As New NexusProvider.RiskType

                If Session(CNRiskType) Is Nothing Then
                    Dim oRiskType As Config.RiskType
                    If Not Session(CNCurrentRiskKey) Is Nothing Then
                        If oQuote.Risks.Count > 0 Then
                            If oQuote.Risks(Session(CNCurrentRiskKey)).RiskCode Is Nothing Then
                                oRiskType = oProductConfig.RiskTypes.RiskType(oQuote.Risks(Session(CNCurrentRiskKey)).RiskTypeCode.Trim)
                            Else
                                oRiskType = oProductConfig.RiskTypes.RiskType(oQuote.Risks(Session(CNCurrentRiskKey)).RiskCode.Trim)
                            End If
                            oRiskQuote.DataModelCode = oRiskType.DataModelCode
                            oRiskQuote.Name = oRiskType.Name
                            oRiskQuote.Path = oRiskType.Path
                            oRiskQuote.RiskCode = oRiskType.RiskCode
                            Session(CNRiskType) = oRiskQuote
                        End If

                    End If
                End If

                ValidateAgencyCancellation()

                If Session(CNRiskType) IsNot Nothing Then
                    oRiskQuote = Session(CNRiskType)
                    If CheckMainDetails("~/Products/" & oProductConfig.Name & "/" & oRiskQuote.Path & "/fullquote.config") And oPortal.UseCorePolicyHeader = False Then
                        btnRequote.Visible = True
                    Else
                        btnRequote.Visible = False
                    End If
                End If

                'WPR67 Begin:Tax Round-off
                Dim sRoundOff As String = String.Empty
                sRoundOff = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.RoundOffToZero, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing, oQuote.BranchCode).Trim()

                If Session(CNIsTrueMonthlyPolicy) Is Nothing Then
                    Session(CNIsTrueMonthlyPolicy) = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsTrueMonthlyPolicy, NexusProvider.RiskTypeOptions.Code, oQuote.ProductCode, Nothing)
                End If

                If sRoundOff.Equals("1") Then
                    litPremiumDisplay.Text = GetLocalResourceObject("lbl_PremiumDisplayRoundoff")
                Else
                    litPremiumDisplay.Text = GetLocalResourceObject("lbl_PremiumDisplay")
                End If
                'WPR67 End:Tax Round-off
                If Request.QueryString("ViewPolicy") IsNot Nothing Then
                    Session.Remove(CNOldPremium) 'Remove the old premium from session
                    Dim iInsuranceFileKey As Integer = Request.QueryString("ViewPolicy")
                    Dim oParty As NexusProvider.BaseParty

                    oQuote = oWebService.GetHeaderAndSummariesByKey(iInsuranceFileKey)
                    oParty = oWebService.GetParty(oQuote.PartyKey)
                    Session(CNParty) = oParty
                    'Put highest risk key into Session
                    For i As Integer = 0 To oQuote.Risks.Count - 1
                        oWebService.GetRisk(oQuote.Risks(i).Key, i, oQuote, oQuote.BranchCode)
                    Next

                    Session(CNQuote) = oQuote
                    Session(CNCurrenyCode) = oQuote.CurrencyCode
                    Session(CNMode) = Mode.View
                    Session.Remove(CNOI)
                    Session.Remove(CNQuoteMode)
                    Session(CNQuoteInSync) = False
                    Session(CNQuoteMode) = QuoteMode.FullQuote
                End If
                If oQuote.PaymentMethod IsNot Nothing AndAlso Trim(oQuote.PaymentMethod) <> "" AndAlso oQuote.PaymentMethod.ToUpper.Trim <> "PAYNOW" AndAlso oQuote.PaymentMethod.ToUpper.Trim <> "INVOICE" AndAlso Session(CNMode) = Mode.View Then
                    InstallementPlan1.Visible = True
                End If
                'This part will be executed if this page is called with arguments
                If Request.QueryString("ClientCode") IsNot Nothing And Request.QueryString("PolicyNo") IsNot Nothing Then
                    Dim sClientCode As String = Request.QueryString("ClientCode")
                    Dim sPolicyNo As String = Request.QueryString("PolicyNo")
                    Dim oSearchCriteria As NexusProvider.PartySearchCriteria
                    Dim oPartyCollection As NexusProvider.PartyCollection
                    Dim oParty As NexusProvider.BaseParty
                    Dim oPartySummary As NexusProvider.PartySummary
                    Dim iInsuranceFileKey As Integer
                    'Set the value to the search criteria
                    oSearchCriteria = New NexusProvider.PartySearchCriteria()
                    oSearchCriteria.PartyTypes.Add(NexusProvider.PartyType.Personal)
                    oSearchCriteria.PartyTypes.Add(NexusProvider.PartyType.Corporate)
                    oSearchCriteria.ShortName = sClientCode
                    Try
                        'calling the sam method to find the client
                        oPartyCollection = oWebService.FindParty(oSearchCriteria)
                        'if same clientcode is searched with different agent then value will be "Nothing" means "Not Found"
                        If oPartyCollection Is Nothing Then
                            Throw New System.Exception("Agent does not have permission to view this policy")
                        End If
                        'passed the find party to GetParty sam call to find client details
                        oParty = oWebService.GetParty(oPartyCollection.Item(0).Key)
                        Session(CNParty) = oParty ' Hold the value in session for later references
                    Finally

                    End Try

                    Try
                        'to get the all policy details of the find client
                        oPartySummary = oWebService.GetPartySummary(oPartyCollection.Item(0).Key)
                        For Each oPolicy As NexusProvider.Policy In oPartySummary.Policies
                            'TO validate the agent key so that other user can not view this policy
                            If oPolicy.LeadAgentKey = CType(Session(CNAgentDetails), NexusProvider.UserDetails).Key Then
                                If oPolicy.Reference.Trim = sPolicyNo.Trim And oPolicy.InsuranceFileTypeCode IsNot Nothing Then
                                    'To check for Renewal policy only
                                    If oPolicy.InsuranceFileTypeCode.Trim = "RENEWAL" Then
                                        iInsuranceFileKey = oPolicy.InsuranceFileKey
                                    End If
                                End If
                            Else
                                Throw New System.Exception("Agent does not have permission to view this policy")
                            End If
                        Next

                        For i As Integer = 0 To oQuote.Risks.Count - 1
                            oWebService.GetRisk(oQuote.Risks(i).Key, i, oQuote, oQuote.BranchCode)
                        Next

                        Session(CNQuote) = oQuote
                    Finally

                    End Try

                    'Reset the Session Variable
                    Session.Remove(CNMTAType)
                    Session.Remove(CNMTATypeDesc)
                    Session.Remove(CNRenewal)
                    Session.Remove(CNRenewalShowPremium)

                    Session(CNMode) = Mode.Buy
                    Session.Remove(CNOI)
                    Session(CNRenewal) = True
                    Session(CNQuoteInSync) = False
                    Session(CNQuoteMode) = QuoteMode.FullQuote

                End If

                'End Renewal Email
                If Not String.IsNullOrEmpty(oQuote.AgentCode) Then
                    Dim oAgentCommission As NexusProvider.EditAgentCommission
                    oAgentCommission = oWebService.GetAgentCommission(oQuote.InsuranceFileKey)
                    Dim oOriginalAgentCommcol As New NexusProvider.AgentCommissionsCollection
                    oOriginalAgentCommcol = CType(Session(CNAgentCommissions), NexusProvider.AgentCommissionsCollection)
                    If oAgentCommission IsNot Nothing And (Session(CNAgentCommissions) Is Nothing OrElse oOriginalAgentCommcol.Count = 0) Then
                        Dim oOriginalAgentCommissions As New NexusProvider.AgentCommissionsCollection
                        Dim oOriginalAgentCommission As NexusProvider.AgentCommissions
                        For icnt As Integer = 0 To oAgentCommission.AgentCommission.Count - 1
                            oOriginalAgentCommission = New NexusProvider.AgentCommissions
                            oOriginalAgentCommission.CommissionRate = oAgentCommission.AgentCommission(icnt).CommissionRate
                            oOriginalAgentCommission.CommissionValue = oAgentCommission.AgentCommission(icnt).CommissionValue
                            oOriginalAgentCommission.TaxGroup = oAgentCommission.AgentCommission(icnt).TaxGroup
                            oOriginalAgentCommission.TaxValue = oAgentCommission.AgentCommission(icnt).TaxValue
                            oOriginalAgentCommission.AgentCode = oAgentCommission.AgentCommission(icnt).Agent
                            oOriginalAgentCommissions.Add(oOriginalAgentCommission)
                        Next
                        Session(CNAgentCommissions) = oOriginalAgentCommissions
                    End If
                End If
                'Start Renewal Premium
                'if Process is renewal

                Dim oRenewalStatus As NexusProvider.RenewalStatus
                If oQuote.InsuranceFileKey <> 0 Then
                    oRenewalStatus = oWebService.GetRenewalStatus(oQuote.InsuranceFileKey)
                    If oRenewalStatus IsNot Nothing Then
                        If Trim(oRenewalStatus.RenewalStatusTypeCode) = "Written" And Trim(oQuote.InsuranceFileTypeCode) = "WRITTEN" Then
                            lblRenewalMessage.Visible = True
                            lblRenewalMessage.Text = Replace(lblRenewalMessage.Text, "#RenewalDate", oQuote.CoverEndDate.ToString("dd MMMMMMMM yyyy"))

                        End If
                        bIsDuplicateRenewalExists = oRenewalStatus.IsDuplicateRenewalExists
                    End If
                End If

                If Session(CNRenewal) Then
                    ' lblRenewalMessage.Visible = True
                    'lblRenewalMessage.Text = Replace(lblRenewalMessage.Text, "#RenewalDate", oQuote.CoverEndDate.ToString("dd MMMMMMMM yyyy"))
                    'Dim oRenewalStatus As NexusProvider.RenewalStatus
                    'oRenewalStatus = oWebService.GetRenewalStatus(oQuote.InsuranceFileKey)
                    oWebService.GetHeaderAndRisksByKey(oQuote)
                    'if Status is in "Awaiting Renewal Notice" then Premium is Visible to the user
                    ' and is ready to be sent to invitation
                    Dim bIsRiskQuoted As Boolean = True
                    If oRenewalStatus.RenewalStatusTypeDescription = sAwaiting_Renewal_Notice Then
                        For i As Integer = 0 To oQuote.Risks.Count - 1
                            If (oQuote.Risks(i).StatusCode.Trim().ToUpper() <> "QUOTED" And oQuote.Risks(i).StatusCode.Trim().ToUpper() <> "RIDEFERRED") Then
                                bIsRiskQuoted = False
                                oWebService.UpdateRenewalStatus(oQuote, "ManReview", Session(CNBranchCode))
                                Exit For
                            End If
                        Next
                    End If
                    If oRenewalStatus.RenewalStatusTypeDescription = sAwaiting_Renewal_Notice AndAlso bIsRiskQuoted Then
                        Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
                        Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
                        Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name
                        Dim oRiskType As New Config.RiskType

                        Session(CNRenewalShowPremium) = True
                        btnPrint.Visible = True
                        btnBuy.Visible = False
                        lblTotalPremium.Text = GetLocalResourceObject("lbl_Renewal_Premium").ToString()
                        Dim dTatalPremium As Decimal
                        If oQuote.Risks.Count > 0 Then
                            dTatalPremium = oQuote.GrossTotal
                        End If
                        hdnfRenewalStatus.Value = "Awaiting Renewal Notice Print"
                        lblTotalPremium.Text = Replace(lblTotalPremium.Text, "#TotalPremium", New Money(dTatalPremium, Session(CNCurrenyCode)).Formatted)
                        lblTotalPremium.Visible = True
                        lblPremium.Visible = True
                        'dont show the Buy Button  and Premium in case of Manual Review
                    ElseIf oRenewalStatus.RenewalStatusTypeDescription = sAwaiting_Manual_Preview Then
                        lblTotalPremium.Visible = False
                        lblPremium.Visible = False
                        lblMessage.Text = GetLocalResourceObject("lbl_RnlSavePolicy").ToString()
                        btnBuy.Visible = False

                        Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
                        Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
                        Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name
                        Dim oRiskType As New Config.RiskType

                        Session(CNRenewalShowPremium) = True

                        lblTotalPremium.Text = GetLocalResourceObject("lbl_Renewal_Premium").ToString()
                        Dim dTatalPremium As Decimal
                        If oQuote.Risks.Count > 0 Then
                            dTatalPremium = oQuote.GrossTotal
                        End If

                        lblTotalPremium.Text = Replace(lblTotalPremium.Text, "#TotalPremium", New Money(dTatalPremium, Session(CNCurrenyCode)).Formatted)
                        lblTotalPremium.Visible = True
                        lblPremium.Visible = True

                    ElseIf (oRenewalStatus.RenewalStatusTypeDescription = sAwaiting_Update Or oRenewalStatus.RenewalStatusTypeDescription = sWrittenAwaiting_Update) Then
                        If Not UserCanDoTask("DisableBuyNow") Then 'EH023078
                            MakeLiveVisible()
                            btnBuy.Visible = True
                            If oQuote.Risks.Count > 0 Then
                                If oQuote.GrossTotal = 0 Then
                                    btnBuy.Visible = False
                                End If
                            End If
                        Else
                            btnBuy.Visible = False
                        End If

                        lblTotalPremium.Text = GetLocalResourceObject("lbl_Renewal_Premium").ToString()
                        lblTotalPremium.Text = Replace(lblTotalPremium.Text, "#TotalPremium", New Money(oQuote.GrossTotal, Session(CNCurrenyCode)).Formatted)
                        lblTotalPremium.Visible = True
                        btnPrint.Visible = False
                        hdnfRenewalStatus.Value = "Awaiting_Update"
                        Dim oDocuments As Config.Documents = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork) _
                                .Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode) _
                                .Documents
                        If Not oDocuments.DocTemplate(Print_Renewaldocument.DocumentName) Is Nothing Then
                            Print_Renewaldocument.Visible = True
                        End If
                        'if the Policy is amended(using Edit\Requote) take the confirmation
                        'and if user directly click Buy button without editing the policy this message box should NOT display.
                        If Session(CNMode) = Mode.Edit Or Session(CNQuoteMode) = QuoteMode.ReQuote Then
                            btnBuy.Attributes.Add("onclick", "javascript:return ConfirmRenewalTermsAcceptence();")
                        End If
                    Else
                        btnBuy.Visible = False
                    End If

                End If
                'End Renewal Premium

                'Association of the javascript method with "Lapse" button
                btnLapse.Attributes.Add("OnClick", "javascript:return LapseConfirmation('" & oQuote.IsMigratedPolicy.ToString() & "');")
                'End

                If CBool(CType(ConfigurationManager.GetSection("NexusFrameWork"),
                                        Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID).AllowLapsePolicy) _
                                        And Session(CNRenewal) Then
                    'If AllowLapsePolicy="true" and Policy in Renewal
                    btnLapse.Visible = True
                    btnCancel.Visible = False ' Need NOT to show the Cancel button for Policy in Renewal
                End If

                Dim sInsuranceFileTypeCode As String = oQuote.InsuranceFileTypeCode.Trim().ToUpper()
                If (sInsuranceFileTypeCode = "MTAQUOTE" OrElse sInsuranceFileTypeCode = "MTAQTETEMP" OrElse
                    sInsuranceFileTypeCode = "MTAQCAN" OrElse sInsuranceFileTypeCode = "MTAQREINS") AndAlso (Session(CNMode) <> Mode.View) Then
                    'Then make visible the btnCancelMTAQuote button and set the attribute with ‘below java script
                    btnCancelMTAQuote.Visible = True
                    btnCancelMTAQuote.Attributes.Add("onclick", "javascript:return(MTACancellationConfirmation());")
                Else
                    btnCancelMTAQuote.Visible = False
                End If

                If Session(CNQuote) IsNot Nothing Then

                    'for Checking that the selected Policy has already been MTA
                    'Session(CNInsuranceFileKey) = oQuote.InsuranceFileKey 'Storing into Session for MTA
                    lblPageheader.Text = Replace(lblPageheader.Text, "#PolicyNo", oQuote.InsuranceFileRef)
                    If oPortal.UseCorePolicyHeader = True And CheckDebitOrderProcessing() Then
                        pnlPaymentMethods.Visible = False
                    End If


                    Session.Remove(CNOI)
                End If

                If Session(CNMode) = Mode.View Then
                    'in view mode user should NOT get these options
                    btnRequote.Visible = False
                    lblMessage.Visible = False
                End If
                If sInsFileTypeCode.ToUpper() = "RENEWAL" AndAlso
                    ((Trim(oQuote.PaymentMethod) <> "" AndAlso oQuote.PaymentMethod.ToUpper() <> "PAYNOW" AndAlso oQuote.PaymentMethod.ToUpper() <> "INVOICE") OrElse
                    oQuote.PaymentMethod.Trim().ToUpper() = "INSTALMENT" OrElse oQuote.PaymentMethod.Trim().ToUpper() = "PREMIUMFINANCE") Then

                    btnSaveQuote.Attributes.Add("onclick", "javascript:return(ConfirmPaymentChangeOnRenewalQuote());")
                End If

                ' AC: If a NEW risk was added after discount was applied, discount was already
                ' rolled back in AddRiskAndRedirect before AddRisk was called. Clear session state
                ' here so the control reinitialises fresh for the user to re-apply.
                ' Do NOT clear for edit-risk returns — the discount on other risks is still valid.
                If CBool(If(Session("POLICY_DISCOUNT_ROLLED_BACK_FOR_NEW_RISK"), False)) Then
                    Session("POLICY_DISCOUNT_ROLLED_BACK_FOR_NEW_RISK") = False
                    ' Fetch fresh total premium from DB (new risk added, total changed)
                    Dim dPreservedPctNew As Double = CDbl(If(Session(CNPolicyDiscountPercentage), 0.0))
                    Dim crFreshTotalNew As Decimal = 0D
                    Try
                        crFreshTotalNew = oWebService.GetPolicyDiscountTotalPremium(oQuote.InsuranceFileKey)
                    Catch
                    End Try
                    Dim crNewDiscountedNew As Decimal = Math.Round(crFreshTotalNew * CDec(1 + dPreservedPctNew / 100), 2)
                    Session("POLICY_DISCOUNT_ORIGINAL_TOTAL") = crFreshTotalNew
                    Session(CNPolicyDiscountTotalPremium) = crFreshTotalNew
                    Session(CNPolicyDiscountedPremium) = crNewDiscountedNew
                    ' Keep Applied=True so InitialiseControl shows the frame with session values.
                    ' Clear APPLIED_TO_RISKS so OnApplyDiscountClicked does fresh apply (no rollback).
                    Session(CNPolicyDiscountApplied) = True
                    Session("POLICY_DISCOUNT_APPLIED_TO_RISKS") = False
                    Dim sTransTypeNewRisk As String = GetDiscountTransactionType()
                    If GetPolicyDiscountCtrl() IsNot Nothing Then GetPolicyDiscountCtrl().InitialiseControl(sTransTypeNewRisk)
                    ViewState("DiscountRolledBack") = True
                End If

                ' Edit Risk: discount was rolled back before editing — preserve user's
                ' previous percentage/reason/recurring, recalculate against fresh premium.
                If CBool(If(Session("POLICY_DISCOUNT_ROLLED_BACK_FOR_EDIT_RISK"), False)) Then
                    Session("POLICY_DISCOUNT_ROLLED_BACK_FOR_EDIT_RISK") = False
                    ' Fetch fresh total premium from DB (risk was re-quoted with new values)
                    Dim dPreservedPct As Double = CDbl(If(Session(CNPolicyDiscountPercentage), 0.0))
                    Dim crFreshTotal As Decimal = 0D
                    Try
                        crFreshTotal = oWebService.GetPolicyDiscountTotalPremium(oQuote.InsuranceFileKey)
                    Catch
                    End Try
                    Dim crNewDiscounted As Decimal = Math.Round(crFreshTotal * CDec(1 + dPreservedPct / 100), 2)
                    Session("POLICY_DISCOUNT_ORIGINAL_TOTAL") = crFreshTotal
                    Session(CNPolicyDiscountTotalPremium) = crFreshTotal
                    Session(CNPolicyDiscountedPremium) = crNewDiscounted
                    ' Keep Applied=True so InitialiseControl shows the frame with session values.
                    ' Clear APPLIED_TO_RISKS so OnApplyDiscountClicked does fresh apply (no rollback).
                    Session(CNPolicyDiscountApplied) = True
                    Session("POLICY_DISCOUNT_APPLIED_TO_RISKS") = False
                    Dim sTransTypeEditRisk As String = GetDiscountTransactionType()
                    If GetPolicyDiscountCtrl() IsNot Nothing Then GetPolicyDiscountCtrl().InitialiseControl(sTransTypeEditRisk)
                    ViewState("DiscountRolledBack") = True
                End If

                ' AC: Initialise the PolicyDiscount control with the transaction type on first load
                ' The control no longer self-initialises — the host page must call InitialiseControl explicitly
                If Not CBool(If(ViewState("DiscountRolledBack"), False)) Then
                    Dim sTransType As String = GetDiscountTransactionType()
                    If GetPolicyDiscountCtrl() IsNot Nothing Then GetPolicyDiscountCtrl().InitialiseControl(sTransType)
                End If

                ' AC: Load existing discount on first load — but only for NB (MTA and REN are handled in InitialiseControl)
                If oQuote IsNot Nothing AndAlso oQuote.InsuranceFileKey > 0 _
                   AndAlso Not CBool(If(ViewState("DiscountRolledBack"), False)) _
                   AndAlso GetDiscountTransactionType() = "NB" Then
                    If GetPolicyDiscountCtrl() IsNot Nothing Then GetPolicyDiscountCtrl().LoadExistingDiscount(oQuote.InsuranceFileKey)
                End If
            Else
                If Session(CNCommissionWarning) IsNot Nothing Then
                    bCommAmendSessionHasValue = True
                    Page.ClientScript.GetPostBackEventReference(Me, "")
                End If
            End If
            ' AC: Disable control in View/Review mode
            If Session(CNMode) = Mode.View OrElse Session(CNMode) = Mode.Review Then
                If GetPolicyDiscountCtrl() IsNot Nothing Then GetPolicyDiscountCtrl().Enabled = False
            End If

            ' AC: If Apply Discount not yet pressed but frame is open with a reason selected,
            ' treat as Unquoted — Buy disabled, Payment Terms disabled, Print disabled, no make live.
            If CBool(If(Session(CNPolicyDiscountEnabled), False)) AndAlso
                   CInt(If(Session(CNPolicyDiscountReasonId), 0)) > 0 AndAlso
                   Not CBool(If(Session(CNPolicyDiscountApplied), False)) Then
                btnBuy.Enabled = False
                pnlPaymentMethods.Enabled = False
                btnPrint.Enabled = False
            End If

            'do this once you have marked the Quote for collection
            If (Session(CNMode) = Mode.Add Or Session(CNMode) = Mode.Edit Or Session(CNMode) = Mode.Buy) And UserCanDoTask("MarkQuote") _
                    And oQuote.MarkedQuoteForCollection = True Then
                btnRequote.OnClientClick = "return UnMarkedConfirmation()"
            End If

            'If Not bPartial Then
            '    Dim WebControlPath As String
            '    WebControlPath = "~/Products/" & oProductConfig.Name & "/SummaryCoverCntlr.ascx"
            '    If (System.IO.File.Exists(Request.MapPath(WebControlPath))) Then
            '        ucPolicyDetails.Visible = False
            '        Dim tempControl As Control = LoadControl(WebControlPath)
            '        SummaryCoverCntrl.Controls.Clear()
            '        SummaryCoverCntrl.Controls.Add(tempControl)
            '    Else
            '        ucPolicyDetails.Visible = True
            '    End If
            'End If

            'For Client Side Validation HiddenField1 value used in java script
            If Session(CNMTAType) = MTAType.CANCELLATION Then
                hdnIsCancelationMTA.Value = 1
            Else
                hdnIsCancelationMTA.Value = 0
            End If

            Dim oEnablPolicyValidationOnActiveplan As NexusProvider.OptionTypeSetting
            oEnablPolicyValidationOnActiveplan = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5076)

            If oQuote.ActivePlan > 0 AndAlso oEnablPolicyValidationOnActiveplan.OptionValue <> "1" AndAlso Session(CNMTAType) = MTAType.CANCELLATION Then
                btnBuy.OnClientClick = "return CancelLiveInstalmentPolicy()"
            End If

            If oQuote.AnniversaryCopy = True And CType(Session(CNIsTrueMonthlyPolicy), Boolean) = True And oQuote.IsValidAnniversaryToAccept = False And Session(CNRenewal) IsNot Nothing Then
                btnBuy.Attributes.Add("onclick", "alert('" & GetLocalResourceObject("lbl_MakeLiveValidation") & "'); return false;")
            End If
            If Session(CNRenewal) IsNot Nothing AndAlso bIsDuplicateRenewalExists = True Then
                btnBuy.Attributes.Add("onclick", "alert('" & GetLocalResourceObject("msg_DuplicateLivePolicyExists") & "'); return false;")
            End If
            ' Check if a backdated MTA version is in session.
            If bIsInBackDatedMode = True Then

                btnAddTask.Visible = False
                btnChangePolicy.Visible = False

                btnCancel.Visible = False
                btnLapse.Visible = False
                btnMarkQuoteForCollection.Visible = False
                btnPrint.Visible = False
                btnSaveQuote.Visible = True
                If Session(CNMode) = Mode.View Then
                    btnSaveQuote.Text = GetLocalResourceObject("btn_Back")
                End If
                pnlPaymentMethods.Visible = False
            End If

            'Write Policy
            If Request("__EVENTARGUMENT") = "WritePolicy" Then

                Dim oRiskCollection As NexusProvider.RiskCollection = oQuote.Risks
                Dim oRiskNotQuoted
                If Not IsNothing(oRiskCollection) AndAlso oRiskCollection.Count > 0 Then
                    oRiskNotQuoted = (From orisk As NexusProvider.Risk In oRiskCollection Where orisk.StatusCode IsNot Nothing AndAlso orisk.StatusCode.Trim <> "QUOTED" Select orisk).ToList()

                    If Not IsNothing(oRiskNotQuoted) AndAlso oRiskNotQuoted.Count = 0 Then
                        Dim oRiskSelected = (From orisk As NexusProvider.Risk In oRiskCollection Where orisk.StatusCode IsNot Nothing AndAlso orisk.StatusCode.Trim = "QUOTED" AndAlso orisk.IsRiskSelected Select orisk).ToList()
                        If Not IsNothing(oRiskSelected) AndAlso oRiskSelected.Count > 0 Then

                            oWebService = New NexusProvider.ProviderManager().Provider
                            Dim oPolicySummary As NexusProvider.PolicySummary
                            Dim oPayment As NexusProvider.Payment = Nothing

                            'Call the BindQuote with flag WritePolicy set to True
                            ' It will generate the Policy Number only
                            If Session(CNRenewal) IsNot Nothing Then
                                oPolicySummary = New NexusProvider.PolicySummary(oQuote.Reference)
                                oPolicySummary = oWebService.BindQuote(oQuote.InsuranceFileKey, oPayment, oQuote.TimeStamp, True, Nothing, "REN", Nothing, True, hPolicyNo.Value)
                            Else
                                oPolicySummary = oWebService.BindQuote(oQuote.InsuranceFileKey, oPayment, oQuote.TimeStamp, Nothing, Nothing, "NB", Nothing, True, hPolicyNo.Value)
                            End If
                            oQuote = oWebService.GetHeaderAndSummariesByKey(oQuote.InsuranceFileKey)
                            Session(CNQuote) = oQuote
                            Session.Item(CNPolicy_Summary) = oPolicySummary
                            Session(CNPaid) = False

                            'Move to the transaction confirmation page to display the document link there
                            Response.Redirect("~/secure/TransactionConfirmation.aspx", False)
                        End If
                    End If
                End If
            End If

            If btnMarkQuoteForCollection.Visible = True Then
                If CheckDebitOrderProcessing() Then
                    btnMarkQuoteForCollection.Visible = False
                End If
            End If

            If Session(CNMode) = Mode.View Or Session(CNMode) = Mode.Review Then
                pnlPaymentMethods.Enabled = False
                vldAgencyCancellation.Enabled = False
            End If
            Dim sIsPrepaymentOptionEnabled As String

            Dim sPaymentTypeValue() As String = rblPaymentMethods.SelectedValue.Split("=")
            sIsPrepaymentOptionEnabled = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsPrepaymentOptionEnabled, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
            If Session(CNAgentType) IsNot Nothing AndAlso sPaymentTypeValue IsNot Nothing AndAlso Session(CNAgentType).ToString.ToUpper = "INTERMEDIARY" AndAlso oQuote.TransactionType = NexusProvider.Quote.Enum_TransactionType.NB AndAlso
                        Session(CNMTAType) Is Nothing AndAlso (sPaymentTypeValue(0).Trim.ToUpper = "INVOICE" OrElse sPaymentTypeValue(0).Trim.ToUpper = "DIRECT DEBIT") Then
                If sIsPrepaymentOptionEnabled = "1" AndAlso sPaymentTypeValue(0).Trim.ToUpper = "INVOICE" Then
                Else
                    btnBuy.OnClientClick = "tb_show(null ,'../modal/SelectAccount.aspx?modal=true&KeepThis=true&TB_iframe=true&height=300&width=300' , null);return false;"
                End If
            End If

            If Session(CNMTAType) IsNot Nothing Then
                hdnTransactionType.Value = "MTA"
            ElseIf Session(CNRenewal) IsNot Nothing AndAlso Session(CNRenewal) Then
                hdnTransactionType.Value = "REN"
            End If

            If Session(CNMode) = Mode.View Then
                btnBacktoFindPolicy.Visible = True
                btnBacktoSummary.Visible = True
            Else
                btnBacktoSummary.Visible = False
            End If

        End Sub

        ''' <summary>
        ''' Add a new risk and redirect to first risk screen
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub AddRiskAndRedirect()

            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
            Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

            Session(CNMode) = Mode.Add

            Session(CNQuoteInSync) = False
            Session.Remove(CNOI)
            Session(CNQuoteInSync) = False
            Session(CNQuoteMode) = QuoteMode.FullQuote
            If Session(CNRiskType) IsNot Nothing Then
                Dim oRiskType As NexusProvider.RiskType = Session(CNRiskType)

                Dim sRiskFolder As String = sProductFolder & "/" & oRiskType.Path & "/"
                Dim sScreenCode As String = GetScreenCode(sRiskFolder & "/" & oProduct.FullQuoteConfig)

                'set up risk object and add a new risk to the quote
                Dim oRisk As New NexusProvider.Risk(sScreenCode, oRiskType.Name)
                oRisk.DataModelCode = oRiskType.DataModelCode
                oRisk.RiskCode = oRiskType.RiskCode
                oQuote.Risks.Add(oRisk)

                ' Req 12: Rollback any existing discount BEFORE adding the new risk, so the new
                ' risk is added on top of original (undiscounted) this_premium values.
                ' Only rollback if there are already quoted risks AND the insurance_file has a
                ' discount applied — checked from DB to avoid stale session state.
                Dim bHasQuotedRisks As Boolean = oQuote.Risks IsNot Nothing AndAlso
                    oQuote.Risks.Cast(Of NexusProvider.Risk)().Any(Function(r)
                                                                       Return r.StatusCode IsNot Nothing AndAlso
                               r.StatusCode.Trim().ToUpper() = "QUOTED"
                                                                   End Function)

                If bHasQuotedRisks Then
                    Dim oDiscount As NexusProvider.PolicyDiscount = oWebService.GetPolicyDiscountInfo(oQuote.InsuranceFileKey)
                    If oDiscount IsNot Nothing AndAlso oDiscount.IsDiscountApplied Then
                        Dim sRollbackTxType As String = GetDiscountTransactionType()
                        oWebService.RollbackPolicyDiscount(oQuote.InsuranceFileKey, 0, sRollbackTxType, 0)
                        Session(CNPolicyDiscountApplied) = False
                        Session("POLICY_DISCOUNT_APPLIED_TO_RISKS") = False
                        Session(CNPolicyDiscountPercentage) = 0.0
                        Session(CNPolicyDiscountedPremium) = 0D
                        Session(CNPolicyDiscountTotalPremium) = 0D
                        ' Signal PremiumDisplay.aspx to clear discount UI on return
                        Session("POLICY_DISCOUNT_ROLLED_BACK_FOR_NEW_RISK") = True
                    End If
                End If

                Session(CNCurrentRiskKey) = oQuote.Risks.Count - 1
                oWebService.AddRisk(oQuote, oQuote.Risks.Count - 1)
                Session(CNQuote) = oQuote
                Session.Remove(CNPolicyAllTaxesColl)
                'Redirect to correct risk screen
                Response.Redirect(sRiskFolder & GetFirstRiskScreen(sRiskFolder & oProduct.FullQuoteConfig), False)

            End If
        End Sub


        ''' <summary>
        ''' Redirect to client 360 page
        ''' </summary>
        ''' <remarks></remarks>
        Sub RedirectToClientDetailPage()
            If CType(Session(CNIsAnonymous), Boolean) = False Then
                'Clear session data for current quote
                ClearQuoteCollectionSessionValues()
                ClearQuote()
            End If

            'redirecting the user to Client details page if he clicks on Save Quote button
            ''need to check if the Login User is an Agent/Direct Registered Client
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
            If CType(Session.Item(CNLoginType), LoginType) = LoginType.Agent Then

                If CType(Session(CNIsAnonymous), Boolean) = False Then
                    Dim oParty As NexusProvider.BaseParty = Session(CNParty)
                    Select Case True
                        Case TypeOf oParty Is NexusProvider.PersonalParty
                            Response.Redirect("~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & oParty.Key.ToString() & "&Code=" & oParty.UserName, False)
                        Case TypeOf oParty Is NexusProvider.CorporateParty
                            Response.Redirect("~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oParty.Key.ToString() & "&Code=" & oParty.UserName, False)
                    End Select

                    oParty = Nothing
                Else
                    'If quote is anonymous then redirect to find client screen
                    Response.Redirect("~/secure/agent/FindClient.aspx")
                End If
            Else
                Response.Redirect(oPortal.ClientStartPage.Trim, False)
            End If

        End Sub
        Protected Sub btnSaveQuote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveQuote.Click

            ' Check if a backdated MTA version is in session.
            Dim bIsInBackDatedMode As Boolean = IIf(Session(CNBaseInsuranceFileKey) IsNot Nothing AndAlso Session(CNBaseInsuranceFileKey) <> Session(CNInsuranceFileKey), True, False)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote
            ' Check if a backdated MTA version is in session.
            If bIsInBackDatedMode = True Then
                'Remove exisitng sessions
                Session.Remove(CNIsInteractiveBackdatedMTA)
                Session.Remove(CNBackDatedVersions)
                Session(CNInsuranceFileKey) = Session(CNBaseInsuranceFileKey)

                Response.Redirect("~/secure/BackDatedMTA.aspx")
            Else
                oQuote = Session(CNQuote)
                Dim oExclusiveLocking As NexusProvider.OptionTypeSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ExclusiveLock)
                If oExclusiveLocking.OptionValue = "1" Then
                    'On Save Quote unlock the Policy
                    UnlockPolicy(oQuote.InsuranceFolderKey, Session(CNBranchCode).ToString)
                End If
                'Call UpdateQuoteV2 to update 
                If (rblPaymentMethods.SelectedValue IsNot Nothing AndAlso rblPaymentMethods.SelectedValue <> "") Then
                    Dim SelectedPaymentOption As String = rblPaymentMethods.SelectedValue
                    Dim SelectedPaymentIndex As String = Mid(Trim(SelectedPaymentOption), 1, SelectedPaymentOption.IndexOf("="))
                    Dim oPaymentOptions As Config.PaymentTypes = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).PaymentTypes
                    Dim oPaymentType As Config.PaymentType = oPaymentOptions.PaymentType(SelectedPaymentIndex)
                    Dim oFinancePlan As New NexusProvider.FinancePlan

                    oFinancePlan = CType(Session(CNFinancePlan), NexusProvider.FinancePlan)
                    oQuote.PaymentMethodCode = oPaymentType.Type
                    oQuote.PaymentMethod = oPaymentType.Type

                    Try
                        Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)

                        'if user has not supplied the Quote Expiry date in case of Renewal
                        If Session(CNRenewal) IsNot Nothing AndAlso oQuote.QuoteExpiryDate = Date.MinValue Then
                            oQuote.QuoteExpiryDate = oQuote.CoverStartDate.AddDays(1)
                        End If
                        If Session(CNMTAType) = MTAType.CANCELLATION Then
                            oQuote.LapseCancelDate = oQuote.LapseDate
                        End If
                        Dim grdInstalments As GridView = Instalments.FindControl("grdInstallmentQuotes")
                        If grdInstalments IsNot Nothing AndAlso grdInstalments.Rows.Count > 0 Then
                            Instalments.SaveInstallmentPlan()
                        End If
                        If Session(CNRenewal) IsNot Nothing AndAlso oQuote IsNot Nothing Then
                            oQuote.PaymentMethod = UCase(oPaymentOptions.PaymentType(Session(CNSelectedPaymentIndex)).Type)
                            If oUserDetails IsNot Nothing Then
                                oWebService.UpdateQuotev2(oQuote, oQuote.BranchCode, oQuote.SubBranchCode, oUserDetails.Key)
                            Else
                                oWebService.UpdateQuotev2(oQuote, oQuote.BranchCode, oQuote.SubBranchCode)
                            End If
                        End If
                        If (SelectedPaymentOption.ToUpper().Contains("INSTALMENTS") OrElse SelectedPaymentOption.ToUpper().Contains("DIRECT DEBIT") OrElse SelectedPaymentOption.ToUpper().Contains("PREMIUMFINANCE")) Then
                            Instalments.SaveFinancePlan()
                        End If
                    Finally
                        oWebService = Nothing
                    End Try
                End If
                'redirect to client detail page
                RedirectToClientDetailPage()
            End If
        End Sub

        Protected Sub btnBuy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuy.Click
            Session("sWarningmsg") = Nothing
            'if MTA is backdated then we need to call AddBackdatedMtaQuote and show backdated versions on a modal page
            'else it should work as per previous functionality

            If Page.IsValid Then
                Session.Remove(CNCommissionWarning)
                If CType(Session(CNIsBackDatedMTA), Boolean) = True OrElse Session(CNBackDatedReinstatement) = True Then
                    'Dim sURL As String
                    Dim oBackDatedVersions As NexusProvider.PolicyCollection
                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
                    Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                    Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
                    Dim oMtaQuote As New NexusProvider.MTA()

                    'Create a request for SAM Method "AddBackdatedMtaQuote"
                    oMtaQuote.InsuranceFileKey = CType(Session(CNInsuranceFileKey), Integer)
                    oMtaQuote.MTAType = CNMTATypeDesc 'as discessed MTA Type is fixed for both the cases either PERMANENT or CANCELLATION
                    oMtaQuote.TypeOfMta = "PERMANENT" 'CNMTATypeDesc
                    oMtaQuote.MtaEffectiveDate = oQuote.CoverStartDate
                    oMtaQuote.MtaExpiryDate = oQuote.ExpiryDate
                    Dim iGracePeriod As Integer = 0
                    With oQuote
                        oMtaQuote.AccountHandlerCnt = .AccountHandlerCnt
                        oMtaQuote.AnalysisCode = .AnalysisCode
                        oMtaQuote.BranchCode = .BranchCode
                        oMtaQuote.BusinessTypeCode = .BusinessTypeCode
                        oMtaQuote.IssueDate = .InceptionDate
                        oMtaQuote.InsuranceFileKey = .InsuranceFileKey
                        oMtaQuote.InsuredName = .InsuredName
                        oMtaQuote.LTUExpiryDate = .LTUExpiryDate
                        oMtaQuote.PolicyStatusCode = .PolicyStatusCode
                        oMtaQuote.PolicyStyleCode = .PolicyStyleCode
                        oMtaQuote.ProposalDate = .ProposalDate
                        iGracePeriod = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.GracePeriod, NexusProvider.RiskTypeOptions.Code, oProduct.ProductCode, oQuote.Risks(Session(CNCurrentRiskKey)).RiskCode)
                        oMtaQuote.QuoteExpiryDate = .QuoteExpiryDate
                        oMtaQuote.QuoteTimeStamp = .TimeStamp
                        oMtaQuote.ReferredAtRenewal = .ReferredAtRenewal
                        oMtaQuote.ReferredOnMTA = .ReferredAtMTA
                        oMtaQuote.Regarding = .Regarding
                        oMtaQuote.RenewalMethodCode = .RenewalMethodCode
                        oMtaQuote.StopReasonCode = .StopReasonCode
                        oMtaQuote.FrequencyCode = .FrequencyCode
                        oMtaQuote.PolicyKey = .InsuranceFileRef
                        If Session(CNMTAType) = MTAType.CANCELLATION Then
                            oMtaQuote.TranactionType = "MTC"
                            oMtaQuote.IsInteractive = False
                            Session(CNIsInteractiveBackdatedMTA) = False
                        ElseIf Session(CNMTAType) = MTAType.REINSTATEMENT Then
                            oMtaQuote.TranactionType = "MTR"
                            oMtaQuote.IsInteractive = False
                            Session(CNIsInteractiveBackdatedMTA) = False
                        Else
                            oMtaQuote.TranactionType = "MTA"
                            oMtaQuote.IsInteractive = True
                            Session(CNIsInteractiveBackdatedMTA) = True
                        End If
                        If Session(CNBaseInsuranceFileKey) Is Nothing Then
                            Session(CNBaseInsuranceFileKey) = .InsuranceFileKey
                        End If
                        oMtaQuote.InsuranceFolderKey = oQuote.InsuranceFolderKey
                        oMtaQuote.PartyKey = oQuote.PartyKey
                        Session.Remove(CNDeletedNode)
                    End With

                    Try
                        ' if some records exists in session CNBackDatedVersions then no need to make SAM call again
                        If Session(CNBackDatedVersions) Is Nothing Then
                            oBackDatedVersions = oWebService.GetBackdatedMTAPolicyVersions(oQuote.InsuranceFileKey)

                            If oBackDatedVersions IsNot Nothing AndAlso oBackDatedVersions.Count > 0 Then
                                Session(CNBackDatedVersions) = oBackDatedVersions
                            Else
                                Dim sFailureReason As String = ""
                                'Call SAM Method for Adding back dated versions. This will return backdated versions or failure reason 

                                'WPR 33 Web Method, Uncomment on SAM Update
                                oBackDatedVersions = oWebService.AddBackdatedMtaQuote(oMtaQuote, sFailureReason)
                                'oBackDatedVersions = Nothing

                                'Add backdated version in session.So that they can be used further
                                Session(CNBackDatedVersions) = oBackDatedVersions
                                'Add failure message in view state so that it can be used further
                                ViewState.Add("FailureReason", sFailureReason)
                            End If
                        Else
                            Session.Remove(CNBackDatedVersions)
                        End If
                    Finally
                        oWebService = Nothing
                        oMtaQuote = Nothing
                    End Try

                    'if failure reason returned then show failure reason as alert
                    If Not String.IsNullOrEmpty(ViewState("FailureReason")) Then
                        Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "AddBackdatedMTAQuoteError",
                            "<script language=""JavaScript"" type=""text/javascript"">function ShowError(){alert('" & ViewState("FailureReason").ToString() & "');}</script>")
                    Else ' Redirect to backdated versions page
                        Response.Redirect("~/secure/BackDatedMTA.aspx", False)

                    End If
                Else
                    'Do as per previous logic
                    RedirectOnBuyNowClick()
                End If
            End If
        End Sub
        ''' <summary>
        ''' AC 35177: Validates that the return premium (annual_premium) does not exceed
        ''' premium_this_year for any risk. Called before make-live for MTA and Cancellation.
        ''' Returns True if validation fails (invalid risks found) — caller must stop processing.
        ''' </summary>
        Private Function ValidateDiscountPremiumThisYear(ByVal oQuote As NexusProvider.Quote, ByVal bIsCancellation As Boolean) As Boolean
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oInvalidRisks As Object(,) = Nothing
            Try
                oInvalidRisks = oWebService.GetInvalidPolicyDiscountRisks(oQuote.InsuranceFileKey)
            Catch ex As Exception
                ' If the call fails, allow make-live to proceed
                Return False
            End Try

            If oInvalidRisks Is Nothing Then Return False

            Dim iRowCount As Integer = oInvalidRisks.GetLength(1)
            If iRowCount = 0 Then Return False

            ' Set affected risks to Unquoted
            ' Array layout from provider: vResults(columnIndex, riskIndex)
            '   0 = RiskKey, 1 = TotalBilledPremium, 2 = TotalReturnPremium, 3 = RiskDescription, 4 = RiskNumber
            For iRow As Integer = 0 To iRowCount - 1
                Dim iRiskKey As Integer = CInt(oInvalidRisks(0, iRow))
                For iRisk As Integer = 0 To oQuote.Risks.Count - 1
                    If oQuote.Risks(iRisk).Key = iRiskKey Then
                        oWebService.UpdateRiskStatus(0, iRiskKey, NexusProvider.RiskStatusType.UNQUOTED)
                        oQuote.Risks(iRisk).StatusCode = NexusProvider.RiskStatusType.UNQUOTED
                        oQuote.Risks(iRisk).StatusDescription = "Unquoted"
                        Exit For
                    End If
                Next
            Next
            Session(CNQuote) = oQuote

            If bIsCancellation Then
                ' AC: Cancellation — show message identifying each affected risk
                Dim sMsg As String = ""
                For iRow As Integer = 0 To iRowCount - 1
                    Dim sRiskNumber As String = CStr(oInvalidRisks(4, iRow))
                    Dim sRiskDesc As String = CStr(oInvalidRisks(3, iRow))
                    If sMsg.Length > 0 Then sMsg &= "\n"
                    sMsg &= String.Format(GetLocalResourceObject("msg_ReturnPremiumGreaterThanBilledForRisk").ToString(), sRiskNumber, sRiskDesc)
                Next
                ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "DiscountValidation",
                    "alert('" & sMsg.Replace("'", "\'") & "');", True)
            Else
                ' AC: MTA — show generic message, redirect to list risk screen (self)
                ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "DiscountValidation",
                    "alert('" & GetLocalResourceObject("msg_ReturnPremiumGreaterThanBilled").ToString().Replace("'", "\'") & "');", True)
            End If

            ' Re-initialise the discount control so the checkbox/frame remain visible
            ' after the validation alert renders on this postback.
            Dim sTransType As String = GetDiscountTransactionType()
            If GetPolicyDiscountCtrl() IsNot Nothing Then GetPolicyDiscountCtrl().InitialiseControl(sTransType)

            Return True
        End Function

        ''' <summary>
        ''' For Normal MTA(non backdated) this method should be called.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub RedirectOnBuyNowClick()

            Dim sSelectedPaymentOption As String = String.Empty
            Dim sSelectedPaymentIndex As Object
            Dim crSelectedPaymentValue As Decimal = Nothing
            Dim crOutstandingAmount As Decimal = Nothing
            Dim oWebService As NexusProvider.SAMForInsurance.ProviderSAMForInsuranceV2 = New NexusProvider.SAMForInsurance.ProviderSAMForInsuranceV2()

            ''First payment method will be defaulted when SkipPaymentSelect property is true
            If CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).SkipPaymentSelect = True Then
                sSelectedPaymentIndex = Session(CNSelectedPaymentIndex)
            Else
                sSelectedPaymentOption = rblPaymentMethods.SelectedValue
                If Not String.IsNullOrEmpty(sSelectedPaymentOption) Then
                    sSelectedPaymentIndex = Mid(Trim(sSelectedPaymentOption), 1, sSelectedPaymentOption.IndexOf("="))
                    crSelectedPaymentValue = Mid(Trim(sSelectedPaymentOption), (sSelectedPaymentOption.IndexOf("=") + 2))
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "NoPaymentMethodSelected", "alert('" + GetLocalResourceObject("msg_NoPaymentMethodAvailable") + "');", True)
                    Exit Sub
                End If
            End If

            Session(CNSelectedPaymentIndex) = sSelectedPaymentIndex

            'CHECK FOR SELECTED VALUE AND REDIRECT THE PAGE ACCORDINGLY.
            Dim oPaymentOptions As Config.PaymentTypes = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).PaymentTypes
            Dim oPaymentType As Config.PaymentType = oPaymentOptions.PaymentType(Session(CNSelectedPaymentIndex))

            If Page.IsValid AndAlso Session(CNMode) <> Mode.View AndAlso oPaymentOptions.PaymentType(Session(CNSelectedPaymentIndex)).Type = "PremiumFinance" Then
                Dim grdInstalments As GridView = Instalments.FindControl("grdInstallmentQuotes")
                If grdInstalments IsNot Nothing AndAlso grdInstalments.Rows.Count > 0 Then
                    If oPaymentType.Name.ToUpper() = "PUTMTAONNEXTINSTALMENT" Or (oPaymentType.Name.ToUpper() = "DIRECT DEBIT" And oQuote.GrossTotal < 0) Then
                        Instalments.SaveInstallmentPlan(True)
                    Else
                        Instalments.SaveInstallmentPlan(False)
                    End If

                Else
                    vldInstalmentSchemes.IsValid = False
                    Exit Sub
                End If
            End If

            If Session(CNCommissionGreaterThanPremium) IsNot Nothing AndAlso Session(CNCommissionGreaterThanPremium) = True Then
                vldCommissionGreaterThanPremium.Enabled = True
                vldCommissionGreaterThanPremium.IsValid = False
            End If
            If Page.IsValid Then
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim oProductConfig As Config.Product = oNexusConfig.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)

                Dim dTotalPremium As Decimal
                If oQuote.Risks.Count > 0 Then
                    dTotalPremium = oQuote.GrossTotal
                    Session(CNAmountToPay) = dTotalPremium
                    'if Payment method is PAYNOW and return premium
                    If oQuote.PaymentMethod IsNot Nothing AndAlso oQuote.PaymentMethod.ToUpper = "PAYNOW" AndAlso dTotalPremium < 0 Then
                        oWebService.GetPolicyOutstandingAmount(crOutstandingAmount, oQuote.InsuranceFileKey, oQuote.BranchCode)
                        dTotalPremium = crOutstandingAmount + oQuote.GrossTotal
                        Session(CNAmountToPay) = dTotalPremium
                    End If
                End If

                If CheckRefer() = True Then
                    Session(CNQuoteMode) = QuoteMode.FullQuote
                    Response.Redirect(AppSettings("WebRoot") & "referred.aspx")
                ElseIf CheckDecline() = True Then
                    Session(CNQuoteMode) = QuoteMode.FullQuote
                    Response.Redirect(AppSettings("WebRoot") & "declined.aspx")
                ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then
                    ' AC 35177: Cancellation validation — check annual_premium vs premium_this_year
                    If ValidateDiscountPremiumThisYear(oQuote, True) Then
                        Exit Sub
                    End If

                    Dim bRiskDeleted As Boolean = False
                    Dim iTotalRiskCount As Integer = oQuote.Risks.Count
                    For iTempVar As Integer = 0 To iTotalRiskCount - 1
                        If bRiskDeleted = True Then
                            bRiskDeleted = False
                            iTempVar -= 1
                        End If
                        If iTempVar < iTotalRiskCount AndAlso oQuote.Risks(iTempVar).IsRisk = False Then
                            oWebService.DeleteRisk(oQuote, iTempVar, oQuote.BranchCode)
                            oQuote.Risks.Remove(iTempVar)
                            bRiskDeleted = True
                            iTotalRiskCount = oQuote.Risks.Count
                        End If
                    Next
                    'Update Session(CNQuote) with only selected risk
                    Session(CNQuote) = oQuote
                    If oPaymentType IsNot Nothing Then
                        If oPaymentType.Name.Trim.ToUpper = "PAYNOW" Then
                            Session.Remove(CNCashListItem) 'Loads Cash List screen when PayNow option selection
                            Session.Remove(CNQuoteMode)
                            Response.Redirect(oPaymentType.Url, False)
                        Else
                            Session(CNPaid) = True
                            Response.Redirect("~/secure/TransactionConfirmation.aspx", False)
                        End If
                    Else
                        Response.Redirect("~/secure/TransactionConfirmation.aspx", False)
                    End If

                    If oQuote.Risks.Count > 0 Then
                        dTotalPremium = oQuote.GrossTotal
                    End If

                Else
                    ' AC 35177: MTA Reduced Premium / Delete Risk validation — check annual_premium vs premium_this_year
                    If Session(CNMTAType) IsNot Nothing AndAlso Session(CNMTAType) = MTAType.PERMANENT Then
                        If ValidateDiscountPremiumThisYear(oQuote, False) Then
                            Exit Sub
                        End If
                    End If

                    If Session(CNMTAType) IsNot Nothing AndAlso Session(CNMTAType) = MTAType.PERMANENT Then
                        If hdnReRunRenewalSelection.Value IsNot Nothing AndAlso hdnReRunRenewalSelection.Value.ToLower() = "true" Then
                            oQuote.DeleteRenQuoteReRunRenewal = True
                        ElseIf hdnReRunRenewalSelection.Value IsNot Nothing AndAlso hdnReRunRenewalSelection.Value.ToLower() = "false" Then
                            oQuote.DeleteRenQuoteReRunRenewal = False

                        End If
                    End If
                    If Session(CNRenewal) Is Nothing AndAlso Session(CNMTAType) IsNot Nothing AndAlso dTotalPremium < 0 Then
                        ' Begin - WPR VB 64 - Media Type Status 
                        Dim CheckMediatypeStatusAtPolicyRefund As String = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance,
                                                        NexusProvider.ProductRiskOptions.CheckMediatypeStatusAtPolicyRefund, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing, oQuote.BranchCode).Trim()

                        If CheckMediatypeStatusAtPolicyRefund.Contains("1") Then
                            Dim oMediaTypeStatus As New NexusProvider.MediaTypeStatus
                            With oMediaTypeStatus
                                .InsuranceFileKey = oQuote.InsuranceFileKey
                                .LossDateSpecified = False
                            End With
                            oWebService.GetPolicyStatusForMediaTypeStatus(oMediaTypeStatus)
                            'SAM Return the False intead of True, if unclear fund exist then it retirn False or else true
                            If Not oMediaTypeStatus.IsUnclearedCashListExists Then
                                vldMediaTypeStatus.IsValid = False
                                Exit Sub

                            End If

                        End If
                        ' End - WPR VB 64 - Media Type Status 
                    End If

                    If CType(Session(CNIsAnonymous), Boolean) = True Then
                        Session(CNRedirectedFor) = "BuyQuote"
                        'redirecting the user to Find Client Page if Quote is anonymous
                        If CType(Session.Item(CNLoginType), LoginType) = LoginType.Agent Then
                            Response.Redirect("~/secure/agent/FindClient.aspx")
                        End If
                    End If

                    If hSelectedAccount.Value <> "" Then
                        Session(CNSelectedAccount) = hSelectedAccount.Value
                    End If
                    If (dTotalPremium < 0.0) And (Session(CNMTAType) <> MTAType.CANCELLATION) Then
                        Session(CNMode) = Mode.Edit
                        Session(CNQuoteInSync) = False
                        Session.Remove(CNOI)
                        If hSelectedAccount.Value <> "" Then
                            Session(CNSelectedAccount) = hSelectedAccount.Value
                        End If
                        Dim TotalSettlementBalance As Decimal = 0

                        Dim dInstalmentSettelmentAmount As Decimal
                        Dim sTransactionType As String = ""
                        If Session(CNRenewal) IsNot Nothing Then
                            sTransactionType = "REN"
                        ElseIf Session(CNMTAType) IsNot Nothing Then
                            sTransactionType = "MTA"
                        Else
                            sTransactionType = "NB"
                        End If

                        oWebService.GetInstalmentSettlementAmount(oQuote.InsuranceFileKey, dInstalmentSettelmentAmount, sTransactionType, oQuote.BranchCode)
                        TotalSettlementBalance = dInstalmentSettelmentAmount + dTotalPremium

                        'this will check in case of MTA Return Premium exists
                        ' which will check statements is set to true in web.config and then will redirect to staements page
                        If oQuote.PaymentMethod.Trim.ToUpper = "DIRECT DEBIT" AndAlso TotalSettlementBalance < 0 Then
                            oQuote.PaymentMethod = "Invoice"
                            Session.Remove(CNSelectedPaymentIndex)
                            Session(CNPaid) = True
                            Response.Redirect("~/secure/TransactionConfirmation.aspx")
                        ElseIf CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).ShowStatements = True OrElse oQuote.PaymentMethod.Trim.ToUpper = "DIRECT DEBIT" Then
                            Response.Redirect("~/secure/Statements.aspx")
                            'else will redirect to transaction confirmation page directly
                        Else
                            Session(CNPaid) = True
                            Response.Redirect("~/secure/TransactionConfirmation.aspx")
                            'End If
                        End If
                    End If

                    '  End If

                    'This will allow Zero Premium to be transacted in Case of NB/MTA/Renewals
                    If (dTotalPremium = 0.0) Then
                        Session(CNMode) = Mode.Edit
                        Session(CNQuoteInSync) = False
                        Session.Remove(CNOI)
                        If CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).ShowStatements = True Then
                            Response.Redirect("~/secure/Statements.aspx")
                        Else
                            Session(CNPaid) = True
                            Response.Redirect("~/secure/TransactionConfirmation.aspx")
                        End If
                    End If

                    Response.Redirect("~/secure/Statements.aspx", False)
                End If
            End If

        End Sub

        Protected Sub btnChangePolicy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnChangePolicy.Click

            'Setting the mode as Edit,Quote mode as Mta Quote in case of Doing MTA on the Premium display page             '
            'Session(CNQuoteMode) = QuoteMode.MTAQuote
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Session.Remove(CNOI)
            Session(CNMode) = Mode.Edit
            Session(CNRenewal) = Nothing
            Session(CNInsuranceFileKey) = oQuote.InsuranceFileKey
            Session(CNQuoteMode) = QuoteMode.FullQuote
            Session(CNQuoteInSync) = False
            Session(CNMtaReasonSelected) = Nothing
            Response.Redirect("~/secure/MTAReason.aspx", False)

        End Sub

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            '' AddMtaQuote, UpdateMtaRisk has been already fired on the page load if CANCELLING the Policy
            Dim oWebService As NexusProvider.ProviderBase
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oMtaQuote As New NexusProvider.MTA()

            oWebService = New NexusProvider.ProviderManager().Provider

            Dim oPolicySummary As NexusProvider.PolicySummary
            oQuote = Session(CNQuote)
            Dim oPayment As NexusProvider.Payment = Nothing

            oPolicySummary = New NexusProvider.PolicySummary(oQuote.Reference)
            oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.Cash)
            oPolicySummary = oWebService.BindQuote(oQuote.InsuranceFileKey, oPayment, oQuote.TimeStamp, Nothing, oQuote.BranchCode, "MTC")
            '' now finally calling BindMtaQuote then redirecting to PersonalClientDetails page


            'Need to find the type of client whether personal or corporate and then redirect it to the right page
            Dim oParty As NexusProvider.BaseParty = Session(CNParty)
            Try
                'Removing the Sessions as the cancellation has been already done in this stage.
                Session.Remove(CNAmountToPay)
                Session.Remove(CNPayment)
                Session.Remove(CNOI)
                Session.Remove(CNMTAType)
                Session.Remove(CNMode)
                Session.Remove(CNQuote)
                Session.Remove(CNInsuranceFileKey)
                Session.Remove(CNCommissionWarning)

                Select Case True
                    Case TypeOf oParty Is NexusProvider.PersonalParty
                        Response.Redirect("~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & oParty.Key.ToString() & "&Code=" & oParty.UserName, False)
                    Case TypeOf oParty Is NexusProvider.CorporateParty
                        Response.Redirect("~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oParty.Key.ToString() & "&Code=" & oParty.UserName, False)
                End Select
            Finally
                'Removing the references of obejcts
                oWebService = Nothing
                oQuote = Nothing
                oPolicySummary = Nothing
                oPayment = Nothing
            End Try

        End Sub
        'Add a event for MarkedQuote entry as this is not been handle through SAM
        Private Sub AddEventForMarkedQuote()
            Dim oEventDetails As New NexusProvider.EventDetails
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            With oEventDetails
                .EventDate = Now()
                .InsuranceFileKey = oQuote.InsuranceFileKey
                .InsuranceFileKeySpecified = True
                .InsuranceFolderKey = oQuote.InsuranceFolderKey
                .InsuranceFolderKeySpecified = True
                .PartyKey = oQuote.PartyKey
                .RtfText = "Quote Marked For Collection"
                .UserName = Session(CNLoginName)
                .EventTypeKey = 5
                .EventLogSubjectKey = 1
            End With

            oWebService.AddEvent(oEventDetails)
        End Sub
        'This code will unmark the Quote if already marked 
        Private Sub CallUnmarkQuote()
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            If oQuote.MarkedQuoteForCollection = True Then
                oQuote.MarkedQuoteForCollection = False
                oWebService.UpdateQuotev2(oQuote, oQuote.BranchCode)
                Session(CNQuote) = oQuote
            End If
        End Sub
        'end

        Protected Sub btnRequote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRequote.Click
            Session("sWarningmsg") = Nothing
            ''UnMark the risk if it is already selected
            CallUnmarkQuote()

            ' AC: Clear the rollback flag before requoting — after requoting the rating sections
            ' will have new values and discount_original_this_premium backup will be stale.
            ' Clearing the flag ensures Apply fires directly (no rollback) on the fresh premium.
            Session("POLICY_DISCOUNT_APPLIED_TO_RISKS") = False
            Dim oEventDetails As New NexusProvider.EventDetails
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Session(CNQuoteMode) = QuoteMode.ReQuote
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
            Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name
            Dim oRiskType As Config.RiskType
            Dim oRiskT As New NexusProvider.RiskType

            'Call UnmarkQuote Process
            CallUnmarkQuote()

            If oQuote.Risks(0).RiskCode Is Nothing Then
                oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(0).RiskTypeCode.Trim)
            Else
                oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(0).RiskCode)
            End If
            oRiskT.DataModelCode = oRiskType.DataModelCode
            oRiskT.Name = oRiskType.Name
            oRiskT.Path = oRiskType.Path
            oRiskT.RiskCode = oRiskType.RiskCode
            Session(CNRiskType) = oRiskT

            Session(CNCoInsurancePage) = Nothing
            With oEventDetails
                .EventDate = Now()
                .InsuranceFileKey = oQuote.InsuranceFileKey
                .InsuranceFileKeySpecified = True
                .InsuranceFolderKey = oQuote.InsuranceFolderKey
                .InsuranceFolderKeySpecified = True
                .PartyKey = oQuote.PartyKey
                .RtfText = "Policy requoted"
                .UserName = Session(CNLoginName)
                .EventTypeKey = 5
                .EventLogSubjectKey = 1
            End With

            oWebService.AddEvent(oEventDetails)
            Dim sRiskFolder As String = sProductFolder & "/" & oRiskT.Path & "/"
            Response.Redirect(sProductFolder & "/" & GetFirstRiskScreen(sRiskFolder & oProduct.FullQuoteConfig), False)

        End Sub

        Protected Sub btnLapse_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLapse.Click
            Response.Redirect("~/secure/PolicyLapsed.aspx", False)
        End Sub


        Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click

            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim sDocument As String = String.Empty
            Dim oDocuments As Config.Documents = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork) _
                                .Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode) _
                                .Documents
            Dim sDocumentDirName As String = oDocuments.Location
            Dim oRenewalStatus As NexusProvider.RenewalStatus

            Dim sFileType As String = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).DocumentFormat.ToLower()

            Dim docType As NexusProvider.DocumentType

            Select Case sFileType.ToUpper.Trim
                Case "PDF"
                    docType = NexusProvider.DocumentType.PDF
                Case "DOCX"
                    docType = NexusProvider.DocumentType.DOCX
            End Select

            oRenewalStatus = oWebService.GetRenewalStatus(oQuote.InsuranceFileKey)

            Try
                If oRenewalStatus.RenewalStatusTypeDescription = sAwaiting_Manual_Preview Then
                    oWebService.UpdateRenewalStatus(oQuote, "AutoReview", oQuote.BranchCode)
                    oRenewalStatus = oWebService.GetRenewalStatus(oQuote.InsuranceFileKey)
                End If
                If oRenewalStatus.RenewalStatusTypeDescription = sAwaiting_Renewal_Notice Then
                    sDocument = oWebService.GenerateInvite(docType, True, oQuote, sDocumentDirName + "\" + Print_Renewaldocument.DocumentName + "." + sFileType, Nothing)
                End If
                If Not oDocuments.DocTemplate(Print_Renewaldocument.DocumentName) Is Nothing Then
                    Print_Renewaldocument.Visible = True
                End If


                btnPrint.Visible = False
                If Not UserCanDoTask("DisableBuyNow") Then 'EH023078
                    btnBuy.Visible = True
                Else
                    btnBuy.Visible = False
                End If

                If CheckDebitOrderProcessing() = False And UserCanDoTask("MarkQuote") Then
                    btnMarkQuoteForCollection.Visible = True
                End If

                'To Enable Write button only when status is Awaiting Update
                oRenewalStatus = oWebService.GetRenewalStatus(oQuote.InsuranceFileKey)
                If oRenewalStatus.RenewalStatusTypeDescription = sAwaiting_Update _
                Or oRenewalStatus.RenewalStatusTypeDescription = sWrittenAwaiting_Update Then
                    If Not UserCanDoTask("DisableBuyNow") Then 'EH023078
                        CheckWritePolicy()
                        MakeLiveVisible()
                    End If
                End If
                'to update the oQuote since Quote status has been updated
                oQuote = oWebService.GetHeaderAndSummariesByKey(oQuote.InsuranceFileKey)
                For iCount As Integer = 0 To oQuote.Risks.Count - 1
                    oWebService.GetRisk(oQuote.Risks(iCount).Key, iCount, oQuote, oQuote.BranchCode)
                Next
                oWebService.GetHeaderAndRisksByKey(oQuote)
                'TO retreive the selected status after btn Print
                For iCount As Integer = 0 To oQuote.Risks.Count - 1
                    If oQuote.Risks(iCount).IsRisk = True Then
                        oQuote.Risks(iCount).IsRisk = True
                    Else
                        oQuote.Risks(iCount).IsRisk = False
                    End If
                Next
            Catch ex As NexusProvider.NexusException
                '  If ex.Errors(0).Code <> "1000093" Then
                vldChkRenwalDoc.Enabled = True
                vldChkRenwalDoc.IsValid = False
                'End If
            Finally
                If Session(CNQuote) IsNot Nothing AndAlso CType(Session(CNQuote), NexusProvider.Quote).Risks IsNot Nothing Then
                    For iTempVar As Integer = 0 To CType(Session(CNQuote), NexusProvider.Quote).Risks.Count - 1
                        oQuote.Risks(iTempVar).IsRiskSelected = CType(Session(CNQuote), NexusProvider.Quote).Risks(iTempVar).IsRiskSelected
                    Next
                End If
                Session(CNQuote) = oQuote
            End Try

            Dim HTable As New Hashtable() 'to hold the document details
            Dim odocument As NexusProvider.DocumentCollection
            odocument = oWebService.GetDocumentList(oQuote.InsuranceFolderKey)

            Dim odocumentstr As New NexusProvider.Document

            'check if there is any object of document type returned
            If Not odocument Is Nothing Then
                If odocument.Count > 0 Then

                    'need to store the unique record into HashTable with the highest DocNum
                    Dim icount As Integer = 0
                    'run the loop till the count reaches to the total documents present in the policy
                    For icount = 0 To odocument.Count - 1
                        'if Exist, then update the data into Hash Table
                        If odocument.Item(icount).DocDescription.Contains("Renewal") Then
                            HTable.Item(odocument.Item(icount).DocDescription) = odocument.Item(icount).DocNum
                        End If
                    Next

                    'displaying the data from the Hash Table 
                    Dim HData As DictionaryEntry
                    For Each HData In HTable
                        Dim h1 As New System.Web.UI.WebControls.HyperLink
                        Session("SelectedDocId") = HData.Value
                        h1.NavigateUrl = "~/secure/document.aspx"
                        h1.Target = "_blank"
                        h1.Text = HData.Key
                        Dim tRow As New TableRow()
                        tblDocs.Rows.Add(tRow)
                        Dim tCell As New TableCell
                        tRow.Cells.Add(tCell)
                        tCell.Controls.Add(h1)
                    Next
                End If
            End If

        End Sub

        ''' <summary>
        ''' TO VALIDATE THE CURRENT POLICY STATUS
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="args"></param>
        ''' <remarks></remarks>
        Protected Sub vldChkStatus_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles vldChkStatus.ServerValidate

            Dim oQuote As NexusProvider.Quote = Session(CNQuote)

            For iTempVar As Integer = 0 To oQuote.Risks.Count - 1
                If oQuote.Risks IsNot Nothing Then
                    If oQuote.Risks(iTempVar).IsRisk = True Then
                        args.IsValid = True
                        Exit For
                    Else
                        args.IsValid = False
                    End If
                End If
            Next

            If args.IsValid = True Then
                For iTempVar As Integer = 0 To oQuote.Risks.Count - 1
                    'if any of the Risk is UNQUOTED then Buy Now should throw a message
                    If oQuote.Risks IsNot Nothing AndAlso oQuote.Risks(iTempVar).IsRisk Then
                        Dim bChkRiskQuoted As Boolean = False
                        If oQuote.Risks(iTempVar).StatusCode IsNot Nothing AndAlso (oQuote.Risks(iTempVar).StatusCode.Trim.ToUpper) <> "DELETED" Then

                            If (oQuote.Risks(iTempVar).StatusCode.Trim.ToUpper <> "QUOTED" AndAlso oQuote.Risks(iTempVar).StatusCode.Trim.ToUpper <> "WRITTEN" AndAlso oQuote.Risks(iTempVar).StatusCode.Trim.ToUpper <> "RIDEFERRED") Then
                                If oQuote.Risks(iTempVar).IsRiskSelected = False Then
                                    vldChkStatus.ErrorMessage = GetLocalResourceObject("Err_UnQuoted")
                                    args.IsValid = False
                                    Exit For
                                End If
                            ElseIf oQuote.Risks(iTempVar).StatusCode.Trim.ToUpper = "QUOTED" AndAlso oQuote.Risks(iTempVar).IsRiskSelected = False Then

                                vldChkStatus.ErrorMessage = GetLocalResourceObject("Err_UnQuoted")
                                args.IsValid = False
                            ElseIf (oQuote.Risks(iTempVar).StatusCode.Trim.ToUpper = "QUOTED" OrElse oQuote.Risks(iTempVar).StatusCode.Trim.ToUpper = "DELETED" OrElse oQuote.Risks(iTempVar).StatusCode.Trim.ToUpper = "RIDEFERRED") AndAlso oQuote.Risks(iTempVar).IsRiskSelected = True Then
                                args.IsValid = True
                                bChkRiskQuoted = True
                            End If
                            If oQuote.Risks(iTempVar).StatusCode.Trim.ToUpper = "PENDINGRI" AndAlso oQuote.Risks(iTempVar).IsRiskSelected = True Then
                                vldChkStatus.ErrorMessage = GetLocalResourceObject("Err_UnQuoted")
                                args.IsValid = False
                                Exit For
                            End If
                        End If
                        If oQuote.Risks(iTempVar).StatusCode IsNot Nothing AndAlso (oQuote.Risks(iTempVar).StatusCode.Trim.ToUpper) = "DELETED" AndAlso oQuote.Risks(iTempVar).IsRiskSelected = True Then
                            args.IsValid = True
                            bChkRiskQuoted = True
                        End If
                        If bChkRiskQuoted = False Then
                            vldChkStatus.ErrorMessage = GetLocalResourceObject("Err_UnQuoted")
                            args.IsValid = False
                            Exit For
                        End If
                    End If
                Next
            End If

            If args.IsValid = True Then
                If oQuote.Risks Is Nothing Then
                    vldChkStatus.ErrorMessage = GetLocalResourceObject("lbl_Please_Check")
                    args.IsValid = False
                End If
            End If

            If args.IsValid = True Then
                If oQuote.Risks IsNot Nothing Then
                    If oQuote.Risks.Count = 0 Then
                        vldChkStatus.ErrorMessage = GetLocalResourceObject("lbl_Please_Check")
                        args.IsValid = False
                    End If
                End If
            End If

            If args.IsValid = True Then
                If oQuote.Risks IsNot Nothing Then
                    Dim bAnyQuotedRiskExist As Boolean = False
                    For iTempVar As Integer = 0 To oQuote.Risks.Count - 1
                        If oQuote.Risks(iTempVar).IsRisk = True Then
                            If oQuote.Risks(iTempVar).StatusCode IsNot Nothing Then
                                If oQuote.Risks(iTempVar).StatusCode.Trim.ToUpper = RiskStatus.Quoted OrElse oQuote.Risks(iTempVar).StatusCode.Trim.ToUpper = "RIDEFERRED" Then
                                    bAnyQuotedRiskExist = True
                                    Exit For
                                End If
                            End If
                        End If
                    Next
                    If bAnyQuotedRiskExist = False Then
                        vldChkStatus.ErrorMessage = GetLocalResourceObject("lbl_Please_Check")
                        args.IsValid = False
                    End If
                End If
            End If

            If args.IsValid = True Then
                Dim bIsInBackDatedMode As Boolean 'To identify if Editing/Viewing backdated policy version from backdated screen
                Dim bAllRiskUnchanged As Boolean = True
                bIsInBackDatedMode = IIf(Session(CNBaseInsuranceFileKey) IsNot Nothing AndAlso Session(CNBaseInsuranceFileKey) <> Session(CNInsuranceFileKey), True, False)
                If Session(CNIsBackDatedMTA) = True And bIsInBackDatedMode = False Then
                    For Each oRisk As NexusProvider.Risk In oQuote.Risks
                        If oRisk.RiskLinkStatusFlag <> "U" Then
                            args.IsValid = True
                            bAllRiskUnchanged = False
                            Exit For
                        End If
                    Next

                    If bAllRiskUnchanged = True Then
                        vldChkStatus.ErrorMessage = GetLocalResourceObject("msg_BackDatedMakeLive")
                        args.IsValid = False
                    End If
                End If

            End If

            If args.IsValid = True Then
                If CType(Session(CNIsBackDatedMTA), Boolean) = True Or Session(CNBackDatedReinstatement) = True Then
                    For Each oRisk As NexusProvider.Risk In oQuote.Risks
                        If oRisk.IsRisk = False Then
                            vldChkStatus.ErrorMessage = GetLocalResourceObject("msg_AllRisksShouldBeSelectedForBackdatedMTA")
                            args.IsValid = False
                            Exit For
                        End If
                    Next
                End If
            End If

            If args.IsValid = True Then
                If (Session(CNMTAType) = MTAType.CANCELLATION OrElse oQuote.InsuranceFileTypeCode.Trim.ToUpper() = "MTAQCAN") AndAlso
                    rblPaymentMethods.SelectedIndex <> -1 Then
                    Dim sSelectedPayment As String = rblPaymentMethods.SelectedValue.Split("=")(0).Trim()
                    Dim oPaymentOptions As Config.PaymentTypes = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).PaymentTypes
                    Dim oPaymentType As Config.PaymentType = oPaymentOptions.PaymentType(sSelectedPayment)
                    If oPaymentType.Type.Trim().ToUpper() = "PREMIUMFINANCE" Then
                        vldChkStatus.ErrorMessage = GetLocalResourceObject("msg_CancellationPaymentMethod")
                        args.IsValid = False
                    End If
                End If
            End If

        End Sub


        Protected Sub Page_LoadComplete(sender As Object, e As System.EventArgs) Handles Me.LoadComplete
            If Session(CNQuote) IsNot Nothing Then
                SetupButtons(Session(CNQuote))
                If Not IsPostBack Then
                    PopulatePaymentMethods()
                End If
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim sIsPrepaymentOptionEnabled As String
                Dim sPaymentTypeValue() As String = rblPaymentMethods.SelectedValue.Split("=")
                sIsPrepaymentOptionEnabled = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsPrepaymentOptionEnabled, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
                If Session(CNAgentType) IsNot Nothing AndAlso sPaymentTypeValue IsNot Nothing AndAlso Session(CNAgentType).ToString.ToUpper = "INTERMEDIARY" AndAlso Session(CNMTAType) Is Nothing AndAlso Session(CNRenewal) Is Nothing AndAlso (sPaymentTypeValue(0).Trim.ToUpper = "INVOICE" OrElse sPaymentTypeValue(0).Trim.ToUpper = "DIRECT DEBIT") Then
                    If sIsPrepaymentOptionEnabled = "1" AndAlso sPaymentTypeValue(0).Trim.ToUpper = "INVOICE" Then
                        btnBuy.OnClientClick = ""
                    Else
                        btnBuy.OnClientClick = "tb_show(null ,'../modal/SelectAccount.aspx?modal=true&KeepThis=true&TB_iframe=true&height=300&width=300' , null);return false;"
                    End If
                End If
            End If
            Dim sPageIndexChanged As String = Request("__EVENTARGUMENT")
            If hdnRememberTab.Value = "1" Then
                ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "HideInstallmentsTab", ";ShowHideInstalmentTab(true);", True)
                If Not sPageIndexChanged.StartsWith("Page") Then
                    tabdetails.Attributes.Remove("class")
                    tabdetails.Attributes.Add("class", "tab-pane animated fadeIn")
                    tabInstalments.Attributes.Remove("class")
                    tabInstalments.Attributes.Add("class", "tab-pane animated fadeIn active")
                End If
            ElseIf hdnRememberTab.Value = "0" Then
                If Not rblPaymentMethods.SelectedValue.Contains("Direct") Then
                    ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "HideInstallmentsTab", ";ShowHideInstalmentTab(false);", True)
                End If
                tabInstalments.Attributes.Remove("class")
                tabInstalments.Attributes.Add("class", "tab-pane animated fadeIn")
                tabdetails.Attributes.Remove("class")
                tabdetails.Attributes.Add("class", "tab-pane animated fadeIn active")

            End If
        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

            If Request.QueryString("ViewPolicy") IsNot Nothing Then
                CMS.Library.Frontend.Functions.SetTheme(Page, ConfigurationManager.AppSettings("ModalPageTemplate"))
            End If

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "LapseConfirmation",
                "<script language=""JavaScript"" type=""text/javascript"">function LapseConfirmation(bIsMigratedPolicy){if(bIsMigratedPolicy=='True'){return confirm('" & GetLocalResourceObject("msg_ConfirmLapseMigratedPolicy").ToString() & "');} else {return confirm('" & GetLocalResourceObject("msg_ConfirmLapsePolicy").ToString() & "');}}</script>")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "MarkedConfirmation",
                "<script language=""JavaScript"" type=""text/javascript"">function MarkedConfirmation(){var IsConfirm; IsConfirm=confirm('" & GetLocalResourceObject("msg_MarkedConfirmation").ToString() & "');return IsConfirm;}</script>")


        End Sub

        Protected Sub btnMarkQuoteForCollection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMarkQuoteForCollection.Click
            'this will Mark the Quote if Button gets visible and MarkedQuoteForCollection is false 
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            If oQuote.MarkedQuoteForCollection = False Then
                oQuote.MarkedQuoteForCollection = True
                oQuote.MarkedDateforCollection = Date.Now.Date
                oWebService.UpdateQuotev2(oQuote, oQuote.BranchCode)
                'Add the Event for Updating the Marked Quote now as this is not handled in SAM
                AddEventForMarkedQuote()
                Session(CNQuote) = oQuote
                btnMarkQuoteForCollection.Visible = False
                SaveQuote()
            End If
        End Sub

        Function CheckMainDetails(ByVal v_sScreenConfigFile As String) As Boolean
            Dim bReturnResult As Boolean
            Dim sMainDetail As String = String.Empty
            Dim Navigator As XPathNavigator
            Dim Doc As XPathDocument = New XPathDocument(Server.MapPath(v_sScreenConfigFile))
            Navigator = Doc.CreateNavigator()
            Dim i, j As XPathNodeIterator
            Dim bStatus As Boolean = False
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            i = Navigator.Select("/screens/screen/tab[1]")
            While i.MoveNext()
                sMainDetail = i.Current.GetAttribute("maindetails", String.Empty)
            End While
            Boolean.TryParse(sMainDetail, bReturnResult)

            Return bReturnResult
        End Function

        ''' <summary>
        ''' This will check the config option from product before showing the Buy Now Button
        ''' </summary>
        ''' <remarks></remarks>
        Sub MakeLiveVisible()
            'Make visible the MakeLive button
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
            btnBuy.Visible = oProduct.MakeLiveVisible

        End Sub
        ''' <summary>
        ''' The method show/hode the write button
        ''' </summary>
        ''' <remarks></remarks>
        Sub CheckWritePolicy()
            If Session(CNMode) <> Mode.View And Session(CNMode) <> Mode.Review Then

                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                Dim sPolicyStatus As String = Nothing
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider


                If oQuote IsNot Nothing Then
                    'Funtionality is supported in New business and in Renewal only
                    'In case of Renewal only when policy renewal status is "Awaiting Update"
                    If Session(CNMTAType) Is Nothing Then
                        'Retreive the Policy Status
                        If oQuote.InsuranceFileTypeCode IsNot Nothing Then
                            sPolicyStatus = oQuote.InsuranceFileTypeCode.Trim.ToUpper
                        End If
                        'Check option from Product Risk Maintenance
                        Dim CheckWritePolicyStatus As String = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance,
                        NexusProvider.ProductRiskOptions.AllowWrittenStatus, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing, oQuote.BranchCode).Trim()
                        'Dim oRiskTypes As NexusProvider.RiskType = Session(CNRiskType)
                        'Dim CheckWritePolicyStatus As String = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, _
                        '                            NexusProvider.ProductRiskOptions.AllowWrittenStatus, NexusProvider.RiskTypeOptions.Code, oQuote.ProductCode, oRiskTypes.RiskCode, oQuote.BranchCode)


                        'Check status of the Quote
                        If sPolicyStatus <> "WRITTEN" And CheckWritePolicyStatus = "1" And Session(CNRenewal) Is Nothing Then
                            'For NB
                            'Make visible the btnWrite
                            If HttpContext.Current.Session.IsCookieless Then
                                btnWrite.OnClientClick = "tb_show(null , '" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/OverridePolicyNumber.aspx?PostbackTo=" & PanelButton.ClientID.ToString & "&modal=true&KeepThis=true&TB_iframe=true&height=450&width=650' , null);return false;"
                            Else
                                btnWrite.OnClientClick = "tb_show(null , '../Modal/OverridePolicyNumber.aspx?PostbackTo=" & PanelButton.ClientID.ToString & "&modal=true&KeepThis=true&TB_iframe=true&height=450&width=650' , null);return false;"
                            End If
                            btnWrite.Visible = True
                        ElseIf sPolicyStatus <> "WRITTEN" And CheckWritePolicyStatus = "1" And Session(CNRenewal) IsNot Nothing Then
                            'For Renewal
                            Dim oRenewalStatus As NexusProvider.RenewalStatus
                            oRenewalStatus = oWebService.GetRenewalStatus(oQuote.InsuranceFileKey)
                            If oRenewalStatus.RenewalStatusTypeDescription = sAwaiting_Update _
                                Or oRenewalStatus.RenewalStatusTypeDescription = sWrittenAwaiting_Update Then
                                'Make visible the btnWrite
                                btnWrite.Attributes.Add("onclick", "return WriteConfirmation()")
                                btnWrite.Visible = True
                            End If
                        Else
                            btnWrite.Visible = False
                        End If

                    End If

                End If
            End If
        End Sub
        Protected Sub btnWrite_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWrite.Click
            ' if all the condition are satisfied
            'Check whetehr user has confirm the choice 
            If Session(CNRenewal) IsNot Nothing Then
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oPolicySummary As NexusProvider.PolicySummary
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                Dim oPayment As NexusProvider.Payment = Nothing

                'Call the BindQuote with flag WritePolicy set to True
                ' It will generate the Policy Number only
                If Session(CNRenewal) IsNot Nothing Then
                    oPolicySummary = New NexusProvider.PolicySummary(oQuote.Reference)
                    oPolicySummary = oWebService.BindQuote(oQuote.InsuranceFileKey, oPayment, oQuote.TimeStamp, True, Nothing, "REN", Nothing, True, Nothing)

                End If

                Session.Item(CNPolicy_Summary) = oPolicySummary
                Session(CNPaid) = False
                'Move to the transaction confirmation page to display the document link there
                Response.Redirect("~/secure/TransactionConfirmation.aspx", False)

            End If
        End Sub

        Function FillContactedDropDown(ByVal iAgentKey As Integer) As Boolean

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oAgentSettings As NexusProvider.AgentSettings = Nothing
            Dim oUserCollection As New NexusProvider.UserCollection
            Dim oUserCollectionWithCorrectedUserName As NexusProvider.UserCollection


            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim sWhocontactedyou As String
            sWhocontactedyou = oQuote.ContactUserName

            GetAgentSettingsCall(oAgentSettings, iAgentKey)
            If (oAgentSettings IsNot Nothing AndAlso oAgentSettings.AssociatedUsers IsNot Nothing) Then
                oUserCollection = oAgentSettings.AssociatedUsers
            End If

            oUserCollectionWithCorrectedUserName = New NexusProvider.UserCollection
            Dim oCorrectedUser As NexusProvider.User
            For Each oUser As NexusProvider.User In oUserCollection
                oCorrectedUser = New NexusProvider.User
                oCorrectedUser = oUser
                oCorrectedUser.FullName = IIf(oUser.UserName.ToString = "", oUser.UserName.ToString(), oUser.FullName.ToString)
                oUserCollectionWithCorrectedUserName.Add(oCorrectedUser)
            Next

            Dim icnt, icount As Integer
            icount = 0
            If (oUserCollection.Count > 0) Then

                For icnt = 0 To oUserCollection.Count - 1
                    If (oUserCollection(icnt).UserName.ToString() = sWhocontactedyou) Then
                        icount = icount + 1
                    End If
                Next
                If icount > 0 Then
                    Return False
                Else
                    Return True
                End If
            End If
        End Function

        ''' <summary>
        ''' For Populate Payment Methods
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub PopulatePaymentMethods()
            Dim oPaymentOptions As Config.PaymentTypes = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).PaymentTypes

            If Not Page.IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "HideInstallmentsTab", ";ShowHideInstalmentTab(false);", True)
            Else

                If Session(CNSelectedPaymentIndex) IsNot Nothing AndAlso oPaymentOptions.PaymentType(Session(CNSelectedPaymentIndex)).Type = "PremiumFinance" AndAlso rblPaymentMethods.SelectedIndex <> -1 Then
                    pnlInstalmentsTab.Visible = True
                    ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "HideInstallmentsTab", "ShowHideInstalmentTab(true);", True)
                Else
                    pnlInstalmentsTab.Visible = False
                    ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "HideInstallmentsTab", "ShowHideInstalmentTab(false);", True)
                End If
            End If
            If Not Page.IsPostBack Then

                Dim oInstalmentQuotes As NexusProvider.InstallmentQuoteCollection
                Dim oInstalmentQuote As NexusProvider.InstalmentQuote
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                If Session(CNMTAType) = MTAType.PERMANENT AndAlso oQuote.GrossTotal < 0 AndAlso oQuote.DefaultPaymentMethod IsNot Nothing AndAlso (oQuote.DefaultPaymentMethod.ToUpper() = "INSTALMENT" OrElse oQuote.DefaultPaymentMethod.ToUpper() = "INSTALMENTS" OrElse oQuote.DefaultPaymentMethod.ToUpper() = "DIRECT DEBIT" OrElse oQuote.DefaultPaymentMethod.ToUpper() = "PREMIUMFINANCE") Then
                    Try


                        oInstalmentQuotes = oWebService.GetInstalmentQuotes(v_dAmountToFinance:=oQuote.GrossTotal,
                                                       v_dtStartDate:=oQuote.CoverStartDate, v_dtEndDate:=oQuote.CoverEndDate, v_dtPreferredDate:=Date.Today,
                                                       v_dtQuoteDate:=DateTime.Now, v_iWeekDay:=1, v_iMonthDay:=1, v_iInsuranceFileKey:=oQuote.InsuranceFileKey,
                                                       v_dOverrideInterestRate:=0, v_dOverrideRate:=0, v_bPaymentProtection:=True, v_sBranchCode:=oQuote.BranchCode,
                                                       v_iInstallmentType:=0, r_nPartyBankId:=0, bIsUseTransactionCurrency:=True,
                                                       v_OverrideDepositAmount:=0)
                        oInstalmentQuote = oInstalmentQuotes(0)


                    Catch ex As Exception

                    End Try
                End If

                CType(Session(CNQuote), NexusProvider.Quote).PaymentMethod = ""
                Dim oPortalConfig As Config.Portal = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID())

                oQuote = Session(CNQuote)

                SetPaymentAccessPermissions()

                Dim bStatementsAgreed As Boolean

                Boolean.TryParse(Session(CNStatementsAgreed), bStatementsAgreed)
                bStatementsAgreed = True
                If bStatementsAgreed Then
                    SetPageProgress(6)

                    Dim sRadioButtonLabel As String
                    Dim dPremiumGross As Decimal
                    Dim dPremiumNet As Decimal
                    Dim dPremiumIPT As Decimal
                    Dim dInstallmentAmount As Decimal
                    Dim nCount As Integer = 0
                    Dim bAvailable As Boolean = False 'for RequiredPaymentMethodForMTA option

                    Dim dt As New DataTable
                    Dim dr As DataRow
                    dt.Columns.Add(New DataColumn("Column1"))
                    dt.Columns.Add(New DataColumn("Column2"))
                    dt.Columns.Add(New DataColumn("Column3"))

                    For Each oPaymentType As Config.PaymentType In oPaymentOptions

                        'To Check the Availability of RequiredPaymentMethodForMTA option with Payment Method
                        If Not oPaymentType.RequiredPaymentMethodForMTA Is Nothing Then
                            If String.IsNullOrEmpty(oPaymentType.RequiredPaymentMethodForMTA.Trim()) = False Then
                                bAvailable = True
                                Exit For
                            End If
                        End If

                    Next

                    For Each oPaymentType As Config.PaymentType In oPaymentOptions
                        If Session(CNLoginType) = LoginType.Customer And oPaymentType.Name <> PaymentTypes.PayNow.ToString() _
                        And oPaymentType.Name <> PaymentTypes.BankGuarantee.ToString() And
                        oPaymentType.Name <> PaymentTypes.Invoice.ToString() And
                        oPaymentType.Name.Trim.ToUpper <> "DIRECT DEBIT" Then
                            If Not Session(CNMTAType) Is Nothing Then
                                'if user is doing MTA
                                sRadioButtonLabel = oPaymentType.MTADisplayName
                            Else
                                'if user is doing NB
                                sRadioButtonLabel = oPaymentType.DisplayName
                            End If
                            'Calculate Premium
                            If oQuote IsNot Nothing And Request.QueryString("quotecollection") <> "true" Then
                                If oQuote.Risks.Count > 0 Then
                                    dPremiumGross = CheckAndCalculateRoundOff()
                                    dPremiumNet = oQuote.NetTotal
                                    dPremiumIPT = oQuote.TaxTotal + oQuote.FeeTotal
                                End If
                            ElseIf Request.QueryString("quotecollection") = "true" Then
                                'quote collection, so set the total to pay from session
                                dPremiumGross = CType(Session(CNTotalForQuoteCollection), Decimal)
                            End If

                            If oPaymentType.FeePercent > 0 Then
                                dPremiumGross += dPremiumGross * (oPaymentType.FeePercent / 100)
                            End If
                            hdnNetPremium.Value = dPremiumNet

                            sRadioButtonLabel = sRadioButtonLabel.Replace("[!FeePercent!]", oPaymentType.FeePercent)
                            sRadioButtonLabel = sRadioButtonLabel.Replace("[!NetPremium!]", New Money(dPremiumNet, Session(CNCurrenyCode)).Formatted)
                            sRadioButtonLabel = sRadioButtonLabel.Replace("[!PremiumIPT!]", New Money(dPremiumIPT, Session(CNCurrenyCode)).Formatted)
                            sRadioButtonLabel = sRadioButtonLabel.Replace("[!Premium!]", New Money(dPremiumGross, Session(CNCurrenyCode)).Formatted)

                            'adding row to data table
                            dr = dt.NewRow
                            If Not Session(CNMTAType) Is Nothing Then 'MTA
                                If Not oPaymentType.RequiredPaymentMethodForMTA Is Nothing Then
                                    If String.IsNullOrEmpty(oPaymentType.RequiredPaymentMethodForMTA.Trim()) = False Then
                                        If oQuote.PaymentMethodCode.Trim().ToUpper() = oPaymentType.RequiredPaymentMethodForMTA.Trim().ToUpper() And oQuote.PaymentMethodCode.Trim().ToUpper() = oPaymentType.Name.Trim().ToUpper() Then
                                            'if RequiredPaymentMethodForMTA is set and matches with original policy payment method then  
                                            'show the matched payment option
                                            dr(0) = sRadioButtonLabel
                                            dr(1) = Trim(oPaymentType.Name & "=" & Convert.ToString(dPremiumGross))
                                            dr(2) = "PayOption" & dt.Rows.Count
                                            dt.Rows.Add(dr)
                                        End If
                                    End If
                                ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then
                                    If oPaymentType.Name = "PayNow" Then
                                        dr(0) = sRadioButtonLabel
                                        dr(1) = Trim(oPaymentType.Name & "=" & Convert.ToString(dPremiumGross))
                                        dr(2) = "PayOption" & dt.Rows.Count
                                        dt.Rows.Add(dr)
                                        dr = dt.NewRow
                                        dr(0) = "<ul><li> Invoice </li></ul>"
                                        dr(1) = Trim("Invoice" & "=" & Convert.ToString(dPremiumGross))
                                        dr(2) = "PayOption" & dt.Rows.Count
                                        dt.Rows.Add(dr)
                                    End If
                                ElseIf bAvailable = False Then
                                    'if RequiredPaymentMethodForMTA is not set then show all payment option
                                    dr(0) = sRadioButtonLabel
                                    dr(1) = Trim(oPaymentType.Name & "=" & Convert.ToString(dPremiumGross))
                                    dr(2) = "PayOption" & dt.Rows.Count
                                    dt.Rows.Add(dr)
                                End If
                            Else 'New Bussiness, need to show all the Payment Options from conig
                                dr(0) = sRadioButtonLabel
                                dr(1) = Trim(oPaymentType.Name & "=" & Convert.ToString(dPremiumGross))
                                dr(2) = "PayOption" & dt.Rows.Count
                                dt.Rows.Add(dr)
                            End If
                            nCount = nCount + 1

                        ElseIf Session(CNLoginType) <> LoginType.Customer Then

                            If GetPaymentAccess(oPaymentType) Then


                                'if SkipPaymentSelect is set to true then redirect to the first payment option (there should only be one option configured anyway)
                                If CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).SkipPaymentSelect = True Then
                                    Session.Add(CNSelectedPaymentIndex, 0)
                                    pnlPaymentMethods.Visible = False
                                Else
                                    pnlPaymentMethods.Visible = True
                                End If

                                If Not Session(CNMTAType) Is Nothing And oPaymentType.Name.Trim.ToUpper = "DIRECT DEBIT" Then
                                    sRadioButtonLabel = GetLocalResourceObject("lbl_displaymsg")
                                Else
                                    If Not Session(CNMTAType) Is Nothing Then
                                        'if user is doing MTA
                                        sRadioButtonLabel = oPaymentType.MTADisplayName
                                    Else
                                        'if user is doing NB
                                        sRadioButtonLabel = oPaymentType.DisplayName
                                    End If
                                End If



                                'Calculate Premium
                                If oQuote IsNot Nothing And Request.QueryString("quotecollection") <> "true" Then
                                    If oQuote.Risks.Count > 0 Then
                                        dPremiumGross = CheckAndCalculateRoundOff()
                                        dPremiumNet = oQuote.NetTotal
                                        dPremiumIPT = oQuote.TaxTotal + oQuote.FeeTotal
                                    End If
                                ElseIf Request.QueryString("quotecollection") = "true" Then
                                    'quote collection, so set the total to pay from session
                                    dPremiumGross = CType(Session(CNTotalForQuoteCollection), Decimal)
                                End If

                                If oPaymentType.FeePercent > 0 Then
                                    dPremiumGross += dPremiumGross * (oPaymentType.FeePercent / 100)
                                End If
                                hdnNetPremium.Value = dPremiumNet
                                If sRadioButtonLabel IsNot Nothing Then
                                    sRadioButtonLabel = sRadioButtonLabel.Replace("[!FeePercent!]", oPaymentType.FeePercent)
                                    sRadioButtonLabel = sRadioButtonLabel.Replace("[!NoOfInstallment!]", oPaymentType.NoOfInstalments)

                                    If oPaymentType.NoOfInstalments > 0 Then
                                        dInstallmentAmount = dPremiumGross / oPaymentType.NoOfInstalments
                                        sRadioButtonLabel = sRadioButtonLabel.Replace("[!InstallmentAmount!]", New Money(dInstallmentAmount, Session(CNCurrenyCode)).Formatted)
                                    End If

                                    sRadioButtonLabel = sRadioButtonLabel.Replace("[!NetPremium!]", New Money(dPremiumNet, Session(CNCurrenyCode)).Formatted)
                                    sRadioButtonLabel = sRadioButtonLabel.Replace("[!PremiumIPT!]", New Money(dPremiumIPT, Session(CNCurrenyCode)).Formatted)
                                    sRadioButtonLabel = sRadioButtonLabel.Replace("[!Premium!]", New Money(dPremiumGross, Session(CNCurrenyCode)).Formatted)
                                End If


                                'adding row to data table
                                dr = dt.NewRow
                                If Session(CNQuoteMode) = QuoteMode.MTAQuote Then 'MTA
                                    If Not oPaymentType.RequiredPaymentMethodForMTA Is Nothing Then
                                        If String.IsNullOrEmpty(oPaymentType.RequiredPaymentMethodForMTA.Trim()) = False Then
                                            If oQuote.PaymentMethodCode.Trim().ToUpper() = oPaymentType.RequiredPaymentMethodForMTA.Trim().ToUpper() And oQuote.PaymentMethodCode.Trim().ToUpper() = oPaymentType.Name.Trim().ToUpper() Then
                                                'if RequiredPaymentMethodForMTA is set and matches with original policy payment method then  
                                                'show the matched payment option
                                                dr(0) = sRadioButtonLabel
                                                dr(1) = Trim(oPaymentType.Name & "=" & Convert.ToString(dPremiumGross))
                                                dr(2) = "PayOption" & dt.Rows.Count
                                                dt.Rows.Add(dr)
                                            End If
                                        End If
                                    ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then
                                        dr(0) = sRadioButtonLabel
                                        dr(1) = Trim(oPaymentType.Name & "=" & Convert.ToString(dPremiumGross))
                                        dr(2) = "PayOption" & dt.Rows.Count
                                        dt.Rows.Add(dr)
                                    ElseIf bAvailable = False Then
                                        'if RequiredPaymentMethodForMTA is not set then show all payment option
                                        dr(0) = sRadioButtonLabel
                                        dr(1) = Trim(oPaymentType.Name & "=" & Convert.ToString(dPremiumGross))
                                        dr(2) = "PayOption" & dt.Rows.Count
                                        dt.Rows.Add(dr)
                                    End If
                                ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then
                                    dr(0) = sRadioButtonLabel
                                    dr(1) = Trim(oPaymentType.Name & "=" & Convert.ToString(dPremiumGross))
                                    dr(2) = "PayOption" & dt.Rows.Count
                                    dt.Rows.Add(dr)

                                Else 'New Bussiness, need to show all the Payment Options from conig
                                    dr(0) = sRadioButtonLabel
                                    dr(1) = Trim(oPaymentType.Name & "=" & Convert.ToString(dPremiumGross))
                                    dr(2) = "PayOption" & dt.Rows.Count
                                    dt.Rows.Add(dr)
                                End If
                                nCount = nCount + 1
                            End If
                        End If
                    Next

                    rblPaymentMethods.DataValueField = "Column2"
                    rblPaymentMethods.DataTextField = "Column1"
                    rblPaymentMethods.DataSource = dt
                    rblPaymentMethods.DataBind()


                End If

                If (oQuote.PaymentMethod <> "") Then
                    Dim oActivePaymentOptions = (From obj In oPaymentOptions Where obj.Enabled = True
                                                 Select obj).ToList()
                    For iSelectedIndex As Integer = 0 To oActivePaymentOptions.Count - 1

                        If iSelectedIndex > oActivePaymentOptions.Count - 1 Then
                            Exit For
                        End If


                        'Remove payment types based on user and product configuration
                        If ((Not bUserInvoice OrElse Not bProductInvoice) AndAlso oActivePaymentOptions(iSelectedIndex).Type.ToUpper() = "INVOICE") OrElse
                           ((Not bUserPayNow OrElse Not bProductPayNow) AndAlso oActivePaymentOptions(iSelectedIndex).Type.ToUpper() = "PAYNOW") OrElse
                           ((Not bUserCashDeposit OrElse Not bProductCashDeposit) AndAlso oActivePaymentOptions(iSelectedIndex).Type.ToUpper() = "CASHDEPOSIT") OrElse
                           ((Not bUserBankGuarantee OrElse Not bProductBankGuarantee) AndAlso oActivePaymentOptions(iSelectedIndex).Type.ToUpper() = "BANKGUARANTEE") OrElse
                           ((Not bUserDirectDebit OrElse Not bProductDirectDebit) AndAlso (oActivePaymentOptions(iSelectedIndex).Type.ToUpper() = "DIRECT DEBIT" OrElse
                           oActivePaymentOptions(iSelectedIndex).Type.ToUpper() = "PREMIUMFINANCE")) OrElse
                           ((Not bIsTrueMonthlypolicyandNextInstalmentRenewal) AndAlso oActivePaymentOptions(iSelectedIndex).Name.ToUpper() = "PUTMTAONNEXTINSTALMENT") Then
                            oActivePaymentOptions.RemoveAt(iSelectedIndex)
                            iSelectedIndex = iSelectedIndex - 1
                            Continue For
                        End If
                        If (oActivePaymentOptions(iSelectedIndex).Type.ToUpper() = oQuote.PaymentMethod.ToUpper() AndAlso Session(CNMTAType) <> MTAType.CANCELLATION) OrElse
                            (oActivePaymentOptions(iSelectedIndex).Type.ToUpper() = "INVOICE" OrElse
                                oActivePaymentOptions(iSelectedIndex).Type.ToUpper() = "PAYNOW" AndAlso
                                Session(CNMTAType) = MTAType.CANCELLATION AndAlso
                                rblPaymentMethods.SelectedIndex = -1) Then
                            If rblPaymentMethods.Items.Count > 0 Then
                                If rblPaymentMethods.Items.Count < oActivePaymentOptions.Count AndAlso rblPaymentMethods.SelectedIndex = -1 Then
                                    rblPaymentMethods.SelectedIndex = 0
                                Else
                                    rblPaymentMethods.SelectedIndex = iSelectedIndex
                                End If
                            End If
                        End If

                        'During cancellation its either invoice of paynow.
                        If (Session(CNMTAType) <> MTAType.CANCELLATION) AndAlso
                            (Not String.IsNullOrEmpty(oQuote.DefaultPaymentMethod) AndAlso
                             oActivePaymentOptions(iSelectedIndex).Type.ToUpper() = "INSTALMENT" OrElse
                             oActivePaymentOptions(iSelectedIndex).Type.ToUpper() = "INSTALMENTS" OrElse
                             oActivePaymentOptions(iSelectedIndex).Type.ToUpper() = "DIRECT DEBIT" OrElse
                             oActivePaymentOptions(iSelectedIndex).Type.ToUpper() = "PREMIUMFINANCE" OrElse
                             oActivePaymentOptions(iSelectedIndex).Name.ToUpper() = "PUTMTAONNEXTINSTALMENT") Then
                            If oQuote.DefaultPaymentMethod IsNot Nothing AndAlso oQuote.DefaultPaymentMethod.ToUpper() = "INSTALMENT" OrElse oQuote.DefaultPaymentMethod.ToUpper() = "INSTALMENTS" OrElse oQuote.DefaultPaymentMethod.ToUpper() = "DIRECT DEBIT" OrElse oQuote.DefaultPaymentMethod.ToUpper() = "PREMIUMFINANCE" Then
                                rblPaymentMethods.Items(iSelectedIndex).Selected = True
                                rblPaymentMethods.SelectedIndex = iSelectedIndex
                                If (Session(CNRenewal) IsNot Nothing) Then
                                    Session(CNInstalmentsPlan) = oQuote.DefaultSchemeNumber.ToString() + "," + oQuote.DefaultSchemeVersion.ToString()
                                End If
                            End If
                        End If


                        If Session(CNMTAType) = MTAType.CANCELLATION Then
                            If rblPaymentMethods IsNot Nothing AndAlso DirectCast(rblPaymentMethods, System.Web.UI.WebControls.RadioButtonList).Items(iSelectedIndex).Text <> "" Then
                                If (Not DirectCast(rblPaymentMethods, System.Web.UI.WebControls.RadioButtonList).Items(iSelectedIndex).Value.ToLower().Contains("invoice")) AndAlso (Not DirectCast(rblPaymentMethods, System.Web.UI.WebControls.RadioButtonList).Items(iSelectedIndex).Value.ToLower().Contains("paynow")) Then
                                    rblPaymentMethods.Items(iSelectedIndex).Enabled = False
                                End If
                            End If
                        End If
                    Next

                    If rblPaymentMethods.SelectedIndex <> -1 Then
                        SetPaymentOption()
                    End If
                Else
                    For iSelectedIndex As Integer = 0 To rblPaymentMethods.Items.Count - 1
                        If Session(CNMTAType) = MTAType.CANCELLATION Then
                            If rblPaymentMethods IsNot Nothing AndAlso DirectCast(rblPaymentMethods, System.Web.UI.WebControls.RadioButtonList).Items(iSelectedIndex).Text <> "" Then
                                If Not (DirectCast(rblPaymentMethods, System.Web.UI.WebControls.RadioButtonList).Items(iSelectedIndex).Value.ToLower().Contains("invoice") OrElse DirectCast(rblPaymentMethods, System.Web.UI.WebControls.RadioButtonList).Items(iSelectedIndex).Value.ToLower().Contains("agentcollection")) Then
                                    rblPaymentMethods.Items(iSelectedIndex).Enabled = False
                                    oQuote.DefaultPaymentMethod = "invoice"
                                End If
                                If DirectCast(rblPaymentMethods, System.Web.UI.WebControls.RadioButtonList).Items(iSelectedIndex).Value.ToLower().Contains("paynow") AndAlso ViewState("PaymentHubEnabled") = "1" AndAlso ViewState("RefundPremiumToInvoice") <> "1" Then
                                    rblPaymentMethods.Items(iSelectedIndex).Enabled = False
                                End If
                            End If
                        End If
                        If Session(CNMTAType) = MTAType.PERMANENT AndAlso oQuote.GrossTotal < 0 Then
                            If Not (DirectCast(rblPaymentMethods, System.Web.UI.WebControls.RadioButtonList).Items(iSelectedIndex).Value.ToLower().Contains("invoice") OrElse DirectCast(rblPaymentMethods, System.Web.UI.WebControls.RadioButtonList).Items(iSelectedIndex).Value.ToLower().Contains("agentcollection")) Then
                                rblPaymentMethods.Items(iSelectedIndex).Enabled = False
                            End If
                            If DirectCast(rblPaymentMethods, System.Web.UI.WebControls.RadioButtonList).Items(iSelectedIndex).Value.ToLower().Contains("paynow") AndAlso ViewState("PaymentHubEnabled") = "1" AndAlso ViewState("RefundPremiumToInvoice") <> "1" Then
                                rblPaymentMethods.Items(iSelectedIndex).Enabled = True
                            End If
                        End If
                        Dim sPaymentTypeValue() As String = rblPaymentMethods.Items(iSelectedIndex).Value.Split("#")
                        If oQuote.GrossTotal < 0 AndAlso ViewState("PaymentHubEnabled") = "1" AndAlso ViewState("RefundPremiumToInvoice") = "1" AndAlso oInstalmentQuote IsNot Nothing AndAlso oInstalmentQuote.MediaTypeDescription.Contains("Credit Card") Then
                            oQuote.DefaultPaymentMethod = "invoice"
                            If sPaymentTypeValue.Length >= 1 AndAlso oQuote.DefaultPaymentMethod IsNot Nothing AndAlso oQuote.DefaultPaymentMethod.ToLower() = "invoice" AndAlso (sPaymentTypeValue(0).ToLower().Contains("agentcollection") OrElse sPaymentTypeValue(0).ToLower().Contains("invoice")) Then
                                rblPaymentMethods.Items(iSelectedIndex).Selected = True
                                SetPaymentOption()
                            End If

                        Else
                            If (oQuote.DefaultPaymentMethod Is Nothing) Then
                                oQuote.DefaultPaymentMethod = "INVOICE"
                                oQuote.PaymentMethodCode = "INVOICE"
                            End If

                            If sPaymentTypeValue.Length >= 1 AndAlso oQuote.DefaultPaymentMethod IsNot Nothing AndAlso oQuote.DefaultPaymentMethod.ToLower() = "invoice" AndAlso sPaymentTypeValue(0).ToLower().Contains("agentcollection") Then
                                rblPaymentMethods.Items(iSelectedIndex).Selected = True
                                SetPaymentOption()
                            ElseIf sPaymentTypeValue.Length >= 1 AndAlso oQuote.DefaultPaymentMethod IsNot Nothing AndAlso oQuote.DefaultPaymentMethod.ToLower() = "instalment" AndAlso sPaymentTypeValue(0).ToLower().Contains("direct debit") Then
                                If oInstalmentQuote IsNot Nothing AndAlso oInstalmentQuote.TotalInstalmentsAmount < 0 AndAlso oQuote.GrossTotal < 0 AndAlso Not (DirectCast(rblPaymentMethods, System.Web.UI.WebControls.RadioButtonList).Items(iSelectedIndex).Value.ToLower().Contains("invoice") OrElse DirectCast(rblPaymentMethods, System.Web.UI.WebControls.RadioButtonList).Items(iSelectedIndex).Value.ToLower().Contains("agentcollection")) Then
                                    rblPaymentMethods.Items(iSelectedIndex).Selected = False
                                    rblPaymentMethods.Items(iSelectedIndex).Enabled = False
                                ElseIf (DirectCast(rblPaymentMethods, System.Web.UI.WebControls.RadioButtonList).Items(iSelectedIndex).Value.ToUpper().Contains("INSTALMENT") OrElse DirectCast(rblPaymentMethods, System.Web.UI.WebControls.RadioButtonList).Items(iSelectedIndex).Value.ToUpper().Contains("INSTALMENTS") OrElse DirectCast(rblPaymentMethods, System.Web.UI.WebControls.RadioButtonList).Items(iSelectedIndex).Value.ToUpper().Contains("DIRECT DEBIT") OrElse DirectCast(rblPaymentMethods, System.Web.UI.WebControls.RadioButtonList).Items(iSelectedIndex).Value.ToUpper().Contains("PREMIUMFINANCE")) Then
                                    rblPaymentMethods.Items(iSelectedIndex).Selected = True
                                    rblPaymentMethods.Items(iSelectedIndex).Enabled = True
                                    SetPaymentOption()
                                ElseIf oInstalmentQuote IsNot Nothing AndAlso oInstalmentQuote.TotalInstalmentsAmount < 0 AndAlso oQuote.GrossTotal < 0 AndAlso (DirectCast(rblPaymentMethods, System.Web.UI.WebControls.RadioButtonList).Items(iSelectedIndex).Value.ToLower().Contains("invoice") OrElse DirectCast(rblPaymentMethods, System.Web.UI.WebControls.RadioButtonList).Items(iSelectedIndex).Value.ToLower().Contains("agentcollection")) Then
                                    rblPaymentMethods.Items(iSelectedIndex).Selected = True
                                    rblPaymentMethods.Items(iSelectedIndex).Enabled = True
                                    SetPaymentOption()
                                End If


                                If (Session(CNRenewal) IsNot Nothing) Then
                                    Session(CNInstalmentsPlan) = oQuote.DefaultSchemeNumber.ToString() + "," + oQuote.DefaultSchemeVersion.ToString()
                                End If
                            ElseIf (sPaymentTypeValue.Length >= 1 AndAlso oQuote.DefaultPaymentMethod IsNot Nothing AndAlso sPaymentTypeValue(0).ToLower().Contains(oQuote.DefaultPaymentMethod.ToLower())) Then
                                rblPaymentMethods.Items(iSelectedIndex).Selected = True
                                SetPaymentOption()
                            End If
                        End If
                    Next
                End If
            Else
                'statements haven't been agreed so go to statements page before allowing payment.
                Response.Redirect("~/secure/Statements.aspx")
            End If
        End Sub

        ''' <summary>
        ''' Setting the payment options for Agent, Product and User 
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub SetPaymentAccessPermissions()

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oUserAuthority As New NexusProvider.UserAuthority
            Dim oAgentSetting As NexusProvider.AgentSettings
            Dim iAgentKey As Integer


            ' Obtaining and setting authority value for User AgentCollection/Invoice
            oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.CanMakeLiveInvoice
            oUserAuthority.UserCode = Session(CNLoginName) ' TO DO need to be made dynamic
            oWebService.GetUserAuthorityValue(oUserAuthority)
            If String.IsNullOrEmpty(oUserAuthority.UserAuthorityValue) = False AndAlso oUserAuthority.UserAuthorityValue.Trim = "1" Then
                bUserInvoice = True
            End If

            If Session(CNMTAType) = MTAType.CANCELLATION Then
                bUserInvoice = True
            End If

            ' Obtaining and setting authority value for User PayNow/Direct Debit   
            oUserAuthority.UserAuthorityOption = Nothing
            oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.CanMakeLivePayNow
            oUserAuthority.UserCode = Session(CNLoginName) ' TO DO need to be made dynamic
            oWebService.GetUserAuthorityValue(oUserAuthority)
            If String.IsNullOrEmpty(oUserAuthority.UserAuthorityValue) = False AndAlso oUserAuthority.UserAuthorityValue.Trim = "1" Then
                bUserPayNow = True
            End If

            ' Obtaining and setting authority value for User BankGuarantee
            oUserAuthority.UserAuthorityOption = Nothing
            oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.CanMakeLiveBankGuarantee
            oUserAuthority.UserCode = Session(CNLoginName) ' TO DO need to be made dynamic
            oWebService.GetUserAuthorityValue(oUserAuthority)
            If String.IsNullOrEmpty(oUserAuthority.UserAuthorityValue) = False AndAlso oUserAuthority.UserAuthorityValue.Trim = "1" Then
                bUserBankGuarantee = True
            End If

            'Check Agent level permission for Payment Type-Cash Deposit 
            oUserAuthority.UserAuthorityOption = Nothing
            oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.CanMakeLiveCashDeposit
            oUserAuthority.UserCode = Session(CNLoginName) ' TO DO need to be made dynamic
            oWebService.GetUserAuthorityValue(oUserAuthority)
            If String.IsNullOrEmpty(oUserAuthority.UserAuthorityValue) = False AndAlso oUserAuthority.UserAuthorityValue.Trim = "1" Then
                bUserCashDeposit = True
            End If

            'Check Agent level permission for Payment Type-Direct Debit 
            oUserAuthority.UserAuthorityOption = Nothing
            oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.CanMakeLiveInstalments
            oUserAuthority.UserCode = Session(CNLoginName) ' TO DO need to be made dynamic
            oWebService.GetUserAuthorityValue(oUserAuthority)
            If String.IsNullOrEmpty(oUserAuthority.UserAuthorityValue) = False AndAlso oUserAuthority.UserAuthorityValue.Trim = "1" Then
                bUserDirectDebit = True
            End If

            Dim oSysOptionNextInstalmentRenewal As NexusProvider.OptionTypeSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5075)
            If Session(CNIsTrueMonthlyPolicy) = True AndAlso oSysOptionNextInstalmentRenewal.OptionValue = "1" AndAlso Session(CNMTAType) IsNot Nothing Then
                bIsTrueMonthlypolicyandNextInstalmentRenewal = True
            End If
            'Only do the following if we're not in quote collection. 
            'We could be dealing with multiple products during quote 
            'collection so cannot set product related options
            If Request.QueryString("quotecollection") <> "true" Then
                sProductCode = CType(Session(CNQuote), NexusProvider.Quote).ProductCode

                ' Obtaining and setting authority value for Product AgentCollection
                Dim oOptionTypeSetting As New NexusProvider.OptionTypeSetting
                Dim ProductRiskOptionValue As String
                ProductRiskOptionValue = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.CanMakeLiveInvoice, NexusProvider.RiskTypeOptions.None, sProductCode, "")
                If String.IsNullOrEmpty(ProductRiskOptionValue) = False AndAlso ProductRiskOptionValue.Trim = "1" Then
                    bProductInvoice = True
                End If

                ' Obtaining and setting authority value for Product PayNow
                ProductRiskOptionValue = Nothing
                ProductRiskOptionValue = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.CanMakeLivePaynow, NexusProvider.RiskTypeOptions.None, sProductCode, "")
                If String.IsNullOrEmpty(ProductRiskOptionValue) = False AndAlso ProductRiskOptionValue.Trim = "1" Then
                    bProductPayNow = True
                End If

                ' Obtaining and setting authority value for Product BankGuarantee
                ProductRiskOptionValue = Nothing
                ProductRiskOptionValue = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.CanMakeBankGuarantee, NexusProvider.RiskTypeOptions.None, sProductCode, "")
                If String.IsNullOrEmpty(ProductRiskOptionValue) = False AndAlso ProductRiskOptionValue.Trim = "1" Then
                    bProductBankGuarantee = True
                End If

                'Check Product Risk Option permission for Payment Type-Cash Deposit
                ProductRiskOptionValue = Nothing
                ProductRiskOptionValue = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.CanMakeLiveCashDeposit, NexusProvider.RiskTypeOptions.None, sProductCode, "")
                If String.IsNullOrEmpty(ProductRiskOptionValue) = False AndAlso ProductRiskOptionValue.Trim = "1" Then
                    bProductCashDeposit = True
                End If

                'Check Product Risk Option permission for Payment Type-Direct Debit
                ProductRiskOptionValue = Nothing
                ProductRiskOptionValue = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.CanMakeLiveInstalments, NexusProvider.RiskTypeOptions.None, sProductCode, "")
                If String.IsNullOrEmpty(ProductRiskOptionValue) = False AndAlso ProductRiskOptionValue.Trim = "1" Then
                    bProductDirectDebit = True
                End If
            End If

            Integer.TryParse(oQuote.Agent, iAgentKey)

            If iAgentKey > 0 Then
                'Call agent setting for current agent
                GetAgentSettingsCall(oAgentSetting, iAgentKey)
                If oAgentSetting IsNot Nothing Then
                    ' Setting and setting authority value for Agent AgentCollection/AgentInvoice
                    bAgentInvoice = True
                    ' Setting and setting authority value for Agent PayNow
                    bAgentPayNow = True
                    ' Setting and setting authority value for Agent BankGuarantee
                    bAgentBankGuarantee = True
                    ' Setting and setting authority value for Agent CashDeposit
                    bAgentCashDeposit = True
                    ' Setting and setting authority value for Agent Direct Debit
                    bAgentDirectDebit = True
                End If
            End If
            oWebService = Nothing
            oUserAuthority = Nothing
            oAgentSetting = Nothing
            iAgentKey = Nothing

        End Sub
        ''' <summary>
        ''' Ge tPaymen tAccess
        ''' </summary>
        ''' <param name="oPaymentType"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetPaymentAccess(ByVal oPaymentType As Config.PaymentType) As Boolean
            Dim sPaymentType As String = oPaymentType.Type.ToString
            Dim bReturnvalue As Boolean = False

            If oQuote IsNot Nothing AndAlso Request.QueryString("quotecollection") <> "true" Then
                If (oQuote.BusinessTypeCode = "DIRECT") Or oQuote.AgentKey = 0 Then
                    If (sPaymentType = PaymentTypes.Invoice.ToString()) AndAlso bUserInvoice AndAlso bProductInvoice AndAlso oPaymentType.Enabled Then
                        bReturnvalue = True
                    ElseIf (sPaymentType = PaymentTypes.PayNow.ToString()) AndAlso bUserPayNow AndAlso bProductPayNow AndAlso oPaymentType.Enabled Then
                        bReturnvalue = True
                    ElseIf (sPaymentType = PaymentTypes.BankGuarantee.ToString()) AndAlso bUserBankGuarantee AndAlso bProductBankGuarantee AndAlso oPaymentType.Enabled Then
                        bReturnvalue = True
                    ElseIf (sPaymentType = PaymentTypes.CreditCard.ToString()) AndAlso oPaymentType.Enabled Then
                        bReturnvalue = True
                    ElseIf (sPaymentType = PaymentTypes.CashDeposit.ToString()) AndAlso bUserCashDeposit AndAlso bProductCashDeposit AndAlso oPaymentType.Enabled Then
                        bReturnvalue = True
                    ElseIf (oPaymentType.Name.Trim.ToUpper = "DIRECT DEBIT") AndAlso bUserDirectDebit AndAlso bProductDirectDebit AndAlso oPaymentType.Enabled Then
                        bReturnvalue = True
                    ElseIf (sPaymentType = PaymentTypes.PaymentHub.ToString()) Then
                        bReturnvalue = True
                    ElseIf oPaymentType.Name = PaymentTypes.PutMTAOnNextInstalment.ToString() AndAlso bIsTrueMonthlypolicyandNextInstalmentRenewal = True Then
                        bReturnvalue = True
                    End If
                Else
                    If (sPaymentType = PaymentTypes.Invoice.ToString()) AndAlso bUserInvoice AndAlso bProductInvoice AndAlso bAgentInvoice AndAlso oPaymentType.Enabled Then
                        bReturnvalue = True
                    ElseIf (sPaymentType = PaymentTypes.PayNow.ToString()) AndAlso bUserPayNow AndAlso bProductPayNow AndAlso bAgentPayNow AndAlso oPaymentType.Enabled Then
                        bReturnvalue = True
                    ElseIf (sPaymentType = PaymentTypes.BankGuarantee.ToString()) AndAlso bUserBankGuarantee AndAlso bProductBankGuarantee AndAlso bAgentBankGuarantee AndAlso oPaymentType.Enabled Then
                        bReturnvalue = True
                    ElseIf (sPaymentType = PaymentTypes.CreditCard.ToString()) AndAlso oPaymentType.Enabled Then
                        bReturnvalue = True
                    ElseIf (sPaymentType = PaymentTypes.CashDeposit.ToString()) AndAlso bUserCashDeposit AndAlso bProductCashDeposit AndAlso bAgentCashDeposit AndAlso oPaymentType.Enabled Then
                        bReturnvalue = True
                    ElseIf (oPaymentType.Name.Trim.ToUpper = "DIRECT DEBIT") AndAlso bUserDirectDebit AndAlso bProductDirectDebit AndAlso bAgentDirectDebit AndAlso oPaymentType.Enabled Then
                        bReturnvalue = True
                    ElseIf oPaymentType.Name = PaymentTypes.PutMTAOnNextInstalment.ToString() AndAlso bIsTrueMonthlypolicyandNextInstalmentRenewal = True Then
                        bReturnvalue = True
                    End If
                End If
            Else
                If Request.QueryString("quotecollection") = True Then
                    'we're in quote collection but this option is not enabled for quote collection 
                    'so show option according to whether or not it is enabled for quote collection
                    If oPaymentType.UseForQuoteCollection AndAlso oPaymentType.Enabled Then
                        bReturnvalue = True
                    Else
                        bReturnvalue = False
                    End If
                End If
            End If
            sPaymentType = Nothing
            Return bReturnvalue
        End Function
        ''' <summary>
        ''' Set Payment Option
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub SetPaymentOption()
            'If Page.IsValid Then
            Dim SelectedPaymentOption As String = rblPaymentMethods.SelectedValue
            Dim SelectedPaymentIndex As String = Mid(Trim(SelectedPaymentOption), 1, SelectedPaymentOption.IndexOf("="))
            Dim SelectedPaymentValue As Decimal = Mid(Trim(SelectedPaymentOption), (SelectedPaymentOption.IndexOf("=") + 2))
            Dim charges As Double
            'SET THE VALUE FOR PAID TO "FALSE". WILL GET "TRUE" AFTER SUCCESSFUL MONEY TRANSACTION
            Session(CNPaid) = False
            Session.Remove(CNSelectedAccount)
            'CHECK FOR SELECTED VALUE AND REDIRECT THE PAGE ACCORDINGLY.
            Dim oPaymentOptions As Config.PaymentTypes = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).PaymentTypes

            Dim oPaymentType As Config.PaymentType = oPaymentOptions.PaymentType(SelectedPaymentIndex)
            oQuote = Session(CNQuote)
            If oPaymentType IsNot Nothing Then
                'strip those unwanted characters which we have added in page_load event
                'PN41169
                If (oPaymentType.FeePercent > 0 AndAlso oQuote IsNot Nothing) Then
                    For iRiskCount As Integer = 0 To oQuote.Risks.Count - 1
                        charges += CType(Session(CNQuote), NexusProvider.Quote).Risks(iRiskCount).PremiumDueGross * (oPaymentType.FeePercent / 100)
                    Next
                End If
                Session.Add(CNChargetoPay, charges)


                UpdatePremiumWithAgentCommision()
                Dim i As Integer
                For i = 0 To oPaymentOptions.Count - 1 Step 1
                    If oPaymentOptions.PaymentType(i).Type = SelectedPaymentIndex Then
                        Session("SelectedItem") = i
                    End If
                Next
                Session.Add(CNSelectedPaymentIndex, SelectedPaymentIndex)

                CType(Session(CNQuote), NexusProvider.Quote).PaymentMethod = oPaymentOptions.PaymentType(Session(CNSelectedPaymentIndex)).Type
                If (oPaymentOptions.PaymentType(Session(CNSelectedPaymentIndex)).Type = "PremiumFinance" OrElse oPaymentOptions.PaymentType(Session(CNSelectedPaymentIndex)).Type = "Direct Debit") AndAlso Session(CNMode) <> Mode.View AndAlso Session(CNMode) <> Mode.Review Then
                    pnlInstalmentsTab.Visible = True
                    ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "ShowInstallmentsTabOnDD", ";ShowHideInstalmentTab(true);", True)
                Else
                    pnlInstalmentsTab.Visible = False
                    ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "ShowInstallmentsTab", ";ShowHideInstalmentTab(false);", True)
                    If HttpContext.Current.Session(CNRenewal) IsNot Nothing Then
                        Session(CNPayment) = Nothing
                        Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                        oQuote.DepositTransactasInstalment = False
                        oQuote.InstDepositAmount = 0
                        Session(CNQuote) = oQuote
                    End If
                End If
            End If
        End Sub
        ''' <summary>
        ''' PaymentMethods - rblPaymentMethods Selected Index Changed
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub rblPaymentMethods_SelectedIndexChanged(sender As Object, e As EventArgs)

            'Call UpdateQuoteV2 to update agent commission and fee etc
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim SelectedPaymentOption As String = rblPaymentMethods.SelectedValue
            Dim SelectedPaymentIndex As String = Mid(Trim(SelectedPaymentOption), 1, SelectedPaymentOption.IndexOf("="))
            Dim oPaymentOptions As Config.PaymentTypes = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).PaymentTypes
            Dim oPaymentType As Config.PaymentType = oPaymentOptions.PaymentType(SelectedPaymentIndex)

            oQuote.PaymentMethodCode = oPaymentType.Type
            oQuote.PaymentMethod = oPaymentType.Type

            SelectedPaymentOptionType = oPaymentType.Type
            Session(CNQuote) = oQuote

            SetPaymentOption()

        End Sub

        Protected Sub vldPaymentMethod_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles vldPaymentMethod.ServerValidate
            ''This validation is not required as first payment method will be defaulted when SkipPaymentSelect property is true
            If Not pnlPaymentMethods.Visible Then
                args.IsValid = True
            Else
                If rblPaymentMethods.SelectedValue IsNot Nothing AndAlso Not String.IsNullOrEmpty(rblPaymentMethods.SelectedValue) Then
                    args.IsValid = True
                Else
                    args.IsValid = False
                End If
            End If
        End Sub
        Protected Sub vldDebitOrderOptionType_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles vldDebitOrderOptionType.ServerValidate
            If CheckDebitOrderProcessing() = "1" Then
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                If oQuote.PaymentMethod.ToUpper() = "INVOICE" Then
                    If oQuote.CollectionFrequency = Nothing OrElse oQuote.PaymentTerm = Nothing Then
                        args.IsValid = False
                    Else
                        args.IsValid = True
                    End If
                End If
            End If
        End Sub
        Private Function CheckDebitOrderProcessing() As Boolean
            CheckDebitOrderProcessing = False
            If ViewState("DebitOrderOptionType") Is Nothing Then
                Dim oWebService As NexusProvider.ProviderBase = Nothing
                Dim oDebitOrderOptionType As New NexusProvider.OptionTypeSetting
                oWebService = New NexusProvider.ProviderManager().Provider
                oDebitOrderOptionType = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, 107)
                ViewState.Add("DebitOrderOptionType", oDebitOrderOptionType.OptionValue)
            End If
            If ViewState("DebitOrderOptionType") = "1" Then
                CheckDebitOrderProcessing = True

            End If
            Return CheckDebitOrderProcessing

        End Function


        ''' <summary>
        ''' ValidateAgencyCancellation
        ''' </summary>
        ''' <remarks></remarks>
        Sub ValidateAgencyCancellation()
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oAgentSettings As NexusProvider.AgentSettings = Nothing
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            If Not String.IsNullOrEmpty(oQuote.Agent) AndAlso oQuote.Agent <> 0 Then
                GetAgentSettingsCall(oAgentSettings, oQuote.Agent)
                If oAgentSettings IsNot Nothing Then
                    Dim dtAgencyCancellationDate As DateTime
                    Dim dtDateToValidateForAgencyCancellation As DateTime
                    Dim oValidateCancelledAgentOrBroker As New NexusProvider.OptionTypeSetting
                    Dim sInsuranceFileTypeCode As String = ""
                    If oQuote.InsuranceFileTypeCode IsNot Nothing Then
                        sInsuranceFileTypeCode = UCase(oQuote.InsuranceFileTypeCode.Trim())
                    End If
                    oValidateCancelledAgentOrBroker = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 1040)
                    If Not oValidateCancelledAgentOrBroker.OptionValue Is Nothing Then
                        If oValidateCancelledAgentOrBroker.OptionValue = "1" Then 'CoverStartDate
                            dtDateToValidateForAgencyCancellation = oQuote.CoverStartDate
                        ElseIf oValidateCancelledAgentOrBroker.OptionValue = "0" Then 'TransactionDate
                            dtDateToValidateForAgencyCancellation = DateTime.Now
                        End If
                    Else
                        dtDateToValidateForAgencyCancellation = DateTime.Now
                    End If

                    If oAgentSettings.AgencyCancellationDate <> DateTime.MinValue AndAlso oAgentSettings.AgencyCancellationDate.ToString("MM/dd/yyyy") <> "12/29/1899" Then
                        dtAgencyCancellationDate = oAgentSettings.AgencyCancellationDate
                        If dtAgencyCancellationDate <= dtDateToValidateForAgencyCancellation Then
                            If Session(CNAgentCancelled) Is Nothing OrElse Session(CNAgentCancelled) = False Then
                                Select Case sInsuranceFileTypeCode
                                    Case "QUOTE", "RENEWAL"
                                        btnBuy.Attributes.Add("onclick", "javascript:alert('" & GetLocalResourceObject("vldAgencyCancelledQuote") & "'); return false; ")
                                    Case Else
                                        btnBuy.Attributes.Add("onclick", "return confirm('" & GetLocalResourceObject("vldAgencyCancelled") & "'); return false; ")
                                End Select
                            End If
                        End If
                    End If
                End If
            End If
        End Sub

        Private Sub SetupButtons(ByRef oQuote As NexusProvider.Quote)
            If oQuote.IsMarketPlacePolicy Then
                btnBuy.Visible = False
                btnCancel.Visible = False
                btnLapse.Visible = False
                btnChangePolicy.Visible = False
                btnPrint.Visible = False
                btnWrite.Visible = False
                btnMarkQuoteForCollection.Visible = False
            End If
        End Sub

#Region "btnCancelMTAQuote_Click"
        ''' <summary>
        ''' This  Method is Used to cancel the MTA quote and call webservice method to be cancelled
        ''' Method Name=btnCancelMTAQuote_Click
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnCancelMTAQuote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelMTAQuote.Click
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
            Dim oMtaQuote As New NexusProvider.MTA()
            Dim nInsuranceFileKey As Integer
            Try
                If oQuote IsNot Nothing Then
                    nInsuranceFileKey = oQuote.InsuranceFileKey
                Else
                    nInsuranceFileKey = Convert.ToInt32(Session(CNInsuranceFileKey))
                End If

                oWebService.CancelMTAQuote(nInsuranceFileKey, Nothing)
            Finally
                oWebService = Nothing
            End Try
            Session(CNPaid) = False
            Session(CNIsCancelMTA) = True
            'Response.Redirect("~/secure/TransactionConfirmation.aspx")
            RedirectToClientDetailPage()
        End Sub
#End Region


        Protected Sub SaveQuoteDetails()
            If HttpContext.Current.Session(CNRenewal) IsNot Nothing Then
                Dim oQuote As NexusProvider.Quote = HttpContext.Current.Session(CNQuote)
                Dim oPaymentOptions As Nexus.Library.Config.PaymentTypes = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID()).PaymentTypes
                Dim nInsuranceFileKey As Integer = 0
                nInsuranceFileKey = oQuote.InsuranceFileKey
                If Session(CNMode) <> Nexus.Constants.Mode.View Then
                    If Not Session(CNSelectedPaymentIndex) Is Nothing Then
                        If oPaymentOptions.PaymentType(Session(CNSelectedPaymentIndex)).Type = "PremiumFinance" Then
                            Instalments.SaveInstallmentPlan()
                            If oQuote IsNot Nothing Then
                                Dim oWebService As NexusProvider.ProviderBase
                                Dim oPayment As NexusProvider.Payment = Nothing
                                oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.None, CDec(HttpContext.Current.Session(CNAmountToPay)))
                                oPayment = HttpContext.Current.Session(CNPayment)
                                oWebService = New NexusProvider.ProviderManager().Provider
                                oWebService.SavePremiumFinanceDetails(oPayment, nInsuranceFileKey, "REN")
                            End If
                        Else
                            Dim sPaymentTypeValue() As String = rblPaymentMethods.SelectedValue.Split("=")
                            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                            oQuote.PaymentMethod = sPaymentTypeValue(0)
                            oWebService.UpdatePolicyPaymentMethod(sPaymentTypeValue(0), nInsuranceFileKey, oQuote.TimeStamp)
                        End If
                    End If
                End If
            End If

        End Sub

        ''' <summary>
        ''' To validate the if instalment scheme is selected
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="args"></param>
        Protected Sub cvInstalmentScheme_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
            Dim oPaymentOptions As Config.PaymentTypes = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).PaymentTypes
            If Session(CNMode) <> Mode.View AndAlso Session(CNSelectedPaymentIndex) IsNot Nothing AndAlso oPaymentOptions.PaymentType(Session(CNSelectedPaymentIndex)).Type = "PremiumFinance" Then
                If Page.IsValid() Then
                    Dim grdInstalments As GridView = CType(Instalments.FindControl("grdInstallmentQuotes"), GridView)
                    If grdInstalments IsNot Nothing AndAlso grdInstalments.SelectedIndex < 0 Then
                        args.IsValid = False
                    End If
                End If
            End If
        End Sub

        Protected Sub btnBacktoFindPolicy_Click(sender As Object, e As EventArgs) Handles btnBacktoFindPolicy.Click
            If Session(CNMode) IsNot Nothing AndAlso Session(CNMode) = Mode.View Then
                If Session(CNPolicyBackButton) IsNot Nothing Then
                    If Session(CNPolicyBackButton) = "FindPolicy" Then
                        Response.Redirect("~/secure/FindPolicy.aspx", False)
                    Else
                        If Session(CNPolicyBackButton) = "BrokerView" Then
                            Response.Redirect("~/secure/BrokerView.aspx", False)
                        Else
                            Dim oParty As NexusProvider.BaseParty

                            If Session(CNParty) IsNot Nothing Then
                                Session(CNPolicyBackFlag) = "BackFlag"
                                Select Case True
                                    Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                                        oParty = CType(Session(CNParty), NexusProvider.CorporateParty)
                                        Response.Redirect("~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oParty.Key & "&Code=" & PureUrlEncode(oParty.UserName) & "#tab-policies")
                                    Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                                        oParty = CType(Session(CNParty), NexusProvider.PersonalParty)
                                        Response.Redirect("~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & oParty.Key & "&Code=" & PureUrlEncode(oParty.UserName) & "#tab-policies")
                                        'Case TypeOf Session(CNParty) Is NexusProvider.OtherParty
                                        '    oParty = CType(Session(CNParty), NexusProvider.OtherParty)
                                End Select
                            End If
                        End If
                    End If
                    Session(CNPolicyBackButton) = "BackButton"
                End If
            End If
        End Sub

        Private Sub btnBacktoSummary_Click(sender As Object, e As EventArgs) Handles btnBacktoSummary.Click
            If Session(CNParty) IsNot Nothing Then
                Dim oParty As NexusProvider.BaseParty
                Session(CNPolicyBackFlag) = "BackFlag"
                Select Case True
                    Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                        oParty = CType(Session(CNParty), NexusProvider.CorporateParty)
                        Response.Redirect("~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oParty.Key & "&Code=" & PureUrlEncode(oParty.UserName) & "#tab-policies")
                    Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                        oParty = CType(Session(CNParty), NexusProvider.PersonalParty)
                        Response.Redirect("~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & oParty.Key & "&Code=" & PureUrlEncode(oParty.UserName) & "#tab-policies")
                        'Case TypeOf Session(CNParty) Is NexusProvider.OtherParty
                        '    oParty = CType(Session(CNParty), NexusProvider.OtherParty)
                End Select
            End If
        End Sub
    End Class

End Namespace
