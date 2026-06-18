Imports CMS.Library
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports System.Web.Configuration
Imports Nexus.Utils
Imports System.Web.HttpContext
Imports Nexus.Constants.Session
Imports Nexus.Constants.Constant
Imports System.Xml.XmlReader
Imports System.Xml

Namespace Nexus
    Partial Class secure_MainDetails : Inherits Frontend.clsCMSPage

        Protected iMode As Integer
        Protected sMsgAnniversaryDayEquallRenewalDay As String
        Protected sMsgAnniversaryDayGreaterRenewalDay As String
        Protected sMsgInvalidAnniversaryDate As String
        Dim oWebService As NexusProvider.ProviderBase = Nothing
        Protected iGracePeriod As Integer = 0
        Protected oMonthsInFutureAllowedForCoverFromDate, oMonthsInFutureAllowedForCoverToDate As NexusProvider.OptionTypeSetting
        Protected sMidnightRenewalSetting, sDefaultCoverToDateToLastDay As String
        Private oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Protected sMsgAgencyCancellation As String
        Protected sMsg_CoverFromDateInFutureValidationMessage As String
        Protected sMsg_CoverToDateInFutureValidationMessage As String
        Protected sServerDate As String

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "DeleteConfirmation",
                "<script language=""JavaScript"" type=""text/javascript"">function DeleteConfirmation(){return confirm('" & GetLocalResourceObject("msgCancelMTA").ToString() & "');}</script>")
            btnCancelMTA.OnClientClick = "return DeleteConfirmation();"

            'Fill all dropdowns
            If Not Page.IsPostBack Then
                FillProduct()
                FillBranches()
                FillSubBranches(POLICYHEADER__BRANCH.SelectedValue)
                FillCurrency()
                FillUnifiedRenewalDays()
                Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
                If oUserDetails IsNot Nothing AndAlso oUserDetails.Key = 0 Then
                    hdnIsBroker.Value = 0
                Else
                    hdnIsBroker.Value = 1
                End If
            End If

        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

            If Request.QueryString("ViewPolicy") IsNot Nothing Then
                CMS.Library.Frontend.Functions.SetTheme(Page, ConfigurationManager.AppSettings("ModalPageTemplate"))
            End If

        End Sub

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oRiskTypes As NexusProvider.RiskType = New NexusProvider.RiskType
            hfNeedToUpdateOnStartChange.Value = "True"

            sMsgAnniversaryDayEquallRenewalDay = GetLocalResourceObject("lbl_vldAnniversaryDayEqualRenewalDay")
            sMsgAnniversaryDayGreaterRenewalDay = GetLocalResourceObject("lbl_vldAnniversaryDayGreaterRenewalDay")
            sMsgInvalidAnniversaryDate = GetLocalResourceObject("lbl_vldInvalidAnniversaryDate")
            sMsgAgencyCancellation = GetLocalResourceObject("msgAgencyCancelled")
            sServerDate = DateTime.Now.ToShortDateString()
            If oQuote.BusinessTypeCode.Trim.ToUpper() = "AGENCY" AndAlso POLICYHEADER__COVERNOTEBOOKNO.SelectedIndex = -1 Then
                If oQuote.InsuranceFileTypeCode Is Nothing OrElse Trim(UCase(oQuote.InsuranceFileTypeCode)) = "QUOTE" Then

                    If Not String.IsNullOrEmpty(oQuote.CoverNoteBookNumber) Then

                        POLICYHEADER__AGENTCODE.Text = oQuote.AgentCode
                        POLICYHEADER__AGENT.Value = oQuote.Agent
                        FillCoverNoteBook() 'Population of covernotbook
                        chkIsCoverNoteUsed.Checked = True
                        POLICYHEADER__COVERNOTEBOOKNO.SelectedIndex = POLICYHEADER__COVERNOTEBOOKNO.Items.IndexOf(POLICYHEADER__COVERNOTEBOOKNO.Items.FindByText(oQuote.CoverNoteBookNumber))
                        FillCoverNoteSheets(oQuote.CoverNoteSheetNumber)
                        POLICYHEADER__COVERNOTESHEETNO.SelectedIndex = POLICYHEADER__COVERNOTESHEETNO.Items.IndexOf(POLICYHEADER__COVERNOTESHEETNO.Items.FindByText(oQuote.CoverNoteSheetNumber))

                    Else
                        hAgentCode.Value = oQuote.AgentCode
                        POLICYHEADER__AGENTCODE.CssClass = "form-control field-mandatory"
                        chkIsCoverNoteUsed.Enabled = True
                        DisableControls(POLICYHEADER__COVERNOTEPANEL)
                        FillCoverNoteBook() 'Population of covernotbook
                    End If
                    txtCoverNoteBookNo.Text = oQuote.CoverNoteBookNumber
                Else
                    If Not String.IsNullOrEmpty(oQuote.CoverNoteBookNumber) Then

                        POLICYHEADER__AGENTCODE.Text = oQuote.AgentCode
                        POLICYHEADER__AGENT.Value = oQuote.Agent
                        FillCoverNoteBook() 'Population of covernotbook
                        POLICYHEADER__COVERNOTEBOOKNO.SelectedIndex = POLICYHEADER__COVERNOTEBOOKNO.Items.IndexOf(POLICYHEADER__COVERNOTEBOOKNO.Items.FindByText(oQuote.CoverNoteBookNumber))
                        FillCoverNoteSheets(oQuote.CoverNoteSheetNumber)
                        POLICYHEADER__COVERNOTESHEETNO.SelectedIndex = POLICYHEADER__COVERNOTESHEETNO.Items.IndexOf(POLICYHEADER__COVERNOTESHEETNO.Items.FindByText(oQuote.CoverNoteSheetNumber))
                        DisableControls(POLICYHEADER__COVERNOTEPANEL)
                    End If
                End If
            End If
            If Not oQuote.BusinessTypeCode = Nothing AndAlso oQuote.BusinessTypeCode.Trim.ToUpper() = "AGENCY" AndAlso Not Session(CNMode) = Mode.View Then
                AgencyCancelled()
            End If

            If chkIsCoverNoteUsed.Checked Then
                EnableControls(POLICYHEADER__COVERNOTEPANEL)

            End If
            If Not Page.IsPostBack Then
                If oQuote IsNot Nothing Then

                    Dim oAgentSettings As NexusProvider.AgentSettings = Nothing
                    Dim oSubAgents As NexusProvider.SubAgentCollection = Nothing
                    'Initializing this as required for product risk options
                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim oUserDetails As NexusProvider.UserDetails
                    oUserDetails = oWebService.GetUserDetails(HttpContext.Current.User.Identity.Name)

                    Dim oEnableUnderwritingYearLabelling As NexusProvider.OptionTypeSetting = Nothing
                    oEnableUnderwritingYearLabelling = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, 68)
                    If oEnableUnderwritingYearLabelling.OptionValue = "1" Then
                        liUnderwritingYear.Visible = True
                        rfvUnderwritingYear.Enabled = True
                    Else
                        liUnderwritingYear.Visible = False
                    End If

                    Dim bChangePolicyNoAtRenewal As Boolean
                    bChangePolicyNoAtRenewal = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.ChangePolicyNumberAtRenewal, NexusProvider.RiskTypeOptions.Code, oQuote.ProductCode, oRiskTypes.RiskCode)
                    If bChangePolicyNoAtRenewal = "1" Then
                        POLICYHEADER__POLICYNUMBER.ReadOnly = False
                    Else
                        POLICYHEADER__POLICYNUMBER.ReadOnly = True
                    End If

                    If oQuote.InsuranceFileTypeCode.Trim().ToUpper().Equals("MTAQUOTE") Then
                        hfIsPolicyInRenewal.Value = oQuote.IsPolicyInRenewal
                        hfDoNotDeleteRenewalQuoteOnMTA.Value = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.DoNotDeleteRenQuoteOnMTA, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
                        Dim oPolicy As NexusProvider.PolicyCollection
                        oPolicy = oWebService.GetAllPolicyVersions(oQuote.InsuranceFolderKey)

                        For iTempVar = oPolicy.Count - 1 To 0 Step -1
                            If oPolicy.Item(iTempVar).InsuranceFileTypeCode.Trim = "RENEWAL" Then ' Find insurancefilecnt for renewal version
                                hfRenewalVersionStartDate.Value = oPolicy.Item(iTempVar).CoverStartDate
                                hfRenewalInsuranceFileKey.Value = oPolicy.Item(iTempVar).InsuranceFileKey
                                Exit For
                            End If
                        Next
                    End If

                    'Option Setting from BO to Re Calculate the date
                    iGracePeriod = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.GracePeriod, NexusProvider.RiskTypeOptions.Code, oQuote.ProductCode, oRiskTypes.RiskCode)
                    hfDefaultCoverToDateToLastDay.Value = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.DefaultCoverToDateToLastDay, NexusProvider.RiskTypeOptions.Code, oQuote.ProductCode, oRiskTypes.RiskCode)
                    oMonthsInFutureAllowedForCoverToDate = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 1009)
                    oMonthsInFutureAllowedForCoverFromDate = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 1008)
                    hfMonthInFutureAllowedForCoverFromDate.Value = oMonthsInFutureAllowedForCoverFromDate.OptionValue
                    sMidnightRenewalSetting = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsMidnightRenewal, NexusProvider.RiskTypeOptions.Code, oQuote.ProductCode, oRiskTypes.RiskCode)
                    Dim iMidNightRenewalOptionValue As Integer = Val(sMidnightRenewalSetting.ToString)
                    hfGracePeriod.Value = iGracePeriod
                    hfMonthInFutureAllowedForCoverToDate.Value = oMonthsInFutureAllowedForCoverToDate.OptionValue
                    hfMidnightRenewalSettings.Value = iMidNightRenewalOptionValue

                    If (hfRenewalVersionStartDate.Value <> String.Empty) Then
                        hfRequiredCoverEndDate.Value = If(iMidNightRenewalOptionValue, DateAdd(DateInterval.Day, -1, CDate(hfRenewalVersionStartDate.Value)), CDate(hfRenewalVersionStartDate.Value))
                    End If
                    POLICYHEADER__POLICYNUMBER.Text = oQuote.InsuranceFileRef
                    POLICYHEADER__PRODUCT.SelectedValue = oQuote.ProductCode
                    POLICYHEADER__INSUREDNAME.Text = oQuote.InsuredName
                    POLICYHEADER__BRANCH.SelectedValue = oQuote.BranchCode
                    POLICYHEADER__SUBBRANCH.SelectedValue = oQuote.SubBranchCode

                    POLICYHEADER__CURRENCY.SelectedValue = oQuote.CurrencyCode
                    POLICYHEADER__FREQUENCY.Value = oQuote.FrequencyCode
                    POLICYHEADER__HANDLER.Text = oQuote.HandlerCode
                    POLICYHEADER__OldPolicyNo.Text = oQuote.OldPolicyNumber

                    If oQuote.BusinessTypeCode.Trim.ToUpper() = "DIRECT" Then
                        btnAgentCode.Enabled = False
                    Else
                        If oUserDetails.PartyKey <> 0 And oUserDetails.PartyType = "AG" Then
                            btnAgentCode.Enabled = False
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "DisableBtnAgentCode", "DisableBusinessType_AgentCodeBtn();", True)
                        Else
                            btnAgentCode.Enabled = True
                        End If
                        POLICYHEADER__AGENTCODE.Text = oQuote.AgentCode
                        POLICYHEADER__AGENT.Value = oQuote.Agent
                        hAgentCode.Value = oQuote.AgentCode
                        POLICYHEADER__AGENTCODE.CssClass = "form-control field-mandatory"

                        chkIsCoverNoteUsed.Enabled = True

                        If oQuote.Agent IsNot Nothing AndAlso CInt(oQuote.Agent) <> 0 Then
                            FillContactPerson(oQuote.Agent)
                        End If
                        POLICYHEADER__CONTACT_NAME.Enabled = True
                        POLICYHEADER__CONTACT_NAME.SelectedValue = oQuote.ContactUserKey
                    End If

                    'Fill payment methods as per selected business type and agency
                    FillPaymentMethods()

                    POLICYHEADER__ALTERNATEREF.Text = oQuote.AlternativeRef
                    POLICYHEADER__POLICYSTATUSCODE.Value = oQuote.PolicyStatusCode
                    If oQuote.AnalysisCode IsNot Nothing Then
                        POLICYHEADER__ANALYSISCODE.Value = oQuote.AnalysisCode.Trim()
                    End If


                    POLICYHEADER__COVERSTARTDATE.Text = oQuote.CoverStartDate
                    hfOriginalCoverStartDate.Value = oQuote.CoverStartDate
                    POLICYHEADER__COVERENDDATE.Text = oQuote.CoverEndDate
                    hfOriginalCoverEndDate.Value = oQuote.CoverEndDate
                    POLICYHEADER__INCEPTION.Text = oQuote.InceptionDate
                    POLICYHEADER__INCEPTIONTPI.Text = oQuote.InceptionTPI
                    POLICYHEADER__RENEWAL.Text = oQuote.RenewalDate
                    POLICYHEADER__QUOTEEXPIRYDATE.Text = oQuote.QuoteExpiryDate.ToShortDateString()
                    If oQuote.ProposalDate <> Date.MinValue Then
                        POLICYHEADER__PROPOSALDATE.Text = oQuote.ProposalDate.ToShortDateString()
                    End If
                    POLICYHEADER__REGARDING.Text = oQuote.Regarding
                    lblRenewedTimes.Text = oQuote.RenewalCount
                    POLICYHEADER__REFERREDATRENEWAL.Checked = oQuote.ReferredAtRenewal
                    POLICYHEADER__REFERREDATMTA.Checked = oQuote.ReferredAtMTA
                    POLICYHEADER__CONSOLIDATEDLEADAGENTCOMMISSION.Checked = oQuote.ConsolidatedLeadAgentCommission
                    POLICYHEADER__RENEWALMETHOD.Value = oQuote.RenewalMethodCode
                    If oQuote.LTUExpiryDate <> Date.MinValue Then
                        POLICYHEADER__LTUEXPIRYDATE.Text = oQuote.LTUExpiryDate
                    End If
                    POLICYHEADER__STOPREASON.Value = oQuote.StopReasonCode

                    If oQuote.LapseCancelDate <> Date.MinValue Then
                        POLICYHEADER__LAPSECANCELDATE.Text = oQuote.LapseCancelDate
                    End If
                    POLICYHEADER__LAPSECANCELREASON.Value = oQuote.LapseCancelReasonCode
                    POLICYHEADER__UNIFIEDRENEWALDAY.SelectedValue = oQuote.RenewalDayNo
                    If oQuote.AnniversaryDate <> Date.MinValue Then
                        POLICYHEADER__ANNIVERSARY.Text = oQuote.AnniversaryDate
                    Else
                        'Temporary arrangement as currently not being returned by SAM. This code need to be removed when corresponding WPR "True Monthly enahncement" is paralelled
                        If Session(CNIsTrueMonthlyPolicy) = True Then
                            Dim dAnniversaryDate As Date
                            If IsDate(CDate(oQuote.RenewalDayNo & "/" & oQuote.CoverStartDate.Month & "/" & oQuote.CoverStartDate.AddYears(1).Year).ToShortDateString()) Then
                                dAnniversaryDate = CDate(oQuote.RenewalDayNo & "/" & oQuote.CoverStartDate.Month & "/" & oQuote.CoverStartDate.AddYears(1).Year).ToShortDateString()
                            End If
                            oQuote.AnniversaryDate = dAnniversaryDate
                            POLICYHEADER__ANNIVERSARY.Text = dAnniversaryDate
                        End If
                    End If
                    hfOriginalAnniversaryDate.Value = POLICYHEADER__ANNIVERSARY.Text
                    POLICYHEADER__BUSINESSTYPE.Value = oQuote.BusinessTypeCode
                    POLICYHEADER__UNDERWRITINGYEAR.Value = oQuote.UnderwritingYearId
                    POLICYHEADER__PAYMENTTERM.Value = oQuote.PaymentTerm
                    POLICYHEADER__COLLECTIONFREQUENCY.Value = oQuote.CollectionFrequency
                    POLICYHEADER__PAYMENTMETHOD.SelectedValue = oQuote.PaymentMethod

                    If (oQuote.CoinsurancePlacement = "GROSS") OrElse (oQuote.CoinsurancePlacement = "NETT") Then
                        POLICYHEADER__COINSURANCEPLACEMENT.SelectedIndex = POLICYHEADER__COINSURANCEPLACEMENT.Items.IndexOf(POLICYHEADER__COINSURANCEPLACEMENT.Items.FindByValue(oQuote.CoinsurancePlacement))
                        POLICYHEADER__COINSURANCEPLACEMENT.Enabled = False
                    Else
                        'POLICYHEADER__COINSURANCEPLACEMENT.SelectedIndex = -1
                    End If
                    'Enable/Disable of the Cover Note control
                    'OnBusinessTypeChange()
                    If Session(CNIsTrueMonthlyPolicy) Is Nothing Then
                        Session(CNIsTrueMonthlyPolicy) = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsTrueMonthlyPolicy, NexusProvider.RiskTypeOptions.Code, oQuote.ProductCode, oRiskTypes.RiskCode)
                        hfIsTrueMonthlyProduct.Value = "1"
                    End If
                    If Session(CNIsUnifiedRenewalDayReadOnly) Is Nothing Then
                        Session(CNIsUnifiedRenewalDayReadOnly) = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.UnifiedRenewalDateIsReadOnly, NexusProvider.RiskTypeOptions.Code, oQuote.ProductCode, oRiskTypes.RiskCode)
                        hfIsUnifiedRenewalDayReadOnly.Value = "1"
                    End If
                End If

                If Not Session(CNMode) = Mode.View AndAlso
                        Not Session(CNMTAType) Is Nothing AndAlso (
                        Session(CNMTAType) = MTAType.CANCELLATION OrElse
                        Session(CNMTAType) = MTAType.PERMANENT OrElse
                        Session(CNMTAType) = MTAType.REINSTATEMENT OrElse
                        Session(CNMTAType) = MTAType.TEMPORARY) AndAlso
                        Session(CNRenewal) Is Nothing Then
                    btnCancelMTA.Visible = True
                Else
                    btnCancelMTA.Visible = False
                End If

                'Show/Hide Policy Associates
                Dim oAllowPolicyClientAssociations As NexusProvider.OptionTypeSetting
                oWebService = New NexusProvider.ProviderManager().Provider
                oAllowPolicyClientAssociations = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.AllowPolicyClientAssociations)
                If oAllowPolicyClientAssociations.OptionValue = "1" Then
                    ctrlPolicyAssociates.Visible = True
                Else
                    ctrlPolicyAssociates.Visible = False
                End If
                oWebService = Nothing

            End If

            If Session(CNIsTrueMonthlyPolicy) = True Then
                POLICYHEADER__FREQUENCY.Enabled = False
                hfIsTrueMonthlyProduct.Value = 1
                'Show true monthly policy related controls
                liAnniversaryDate.Attributes.Remove("style")
                liUnifiedRenewalDay.Attributes.Remove("style")
                If Session(CNIsUnifiedRenewalDayReadOnly) = True Then
                    POLICYHEADER__UNIFIEDRENEWALDAY.Enabled = False
                    calCoverEndDate.Enabled = False
                Else
                    calRenewalDate.Enabled = True
                End If
                rfvAnniversaryDate.Enabled = True
                cvAnniversaryDate.Enabled = True

            Else
                hfIsTrueMonthlyProduct.Value = 0
                'hide true monthly policy related controls
                liAnniversaryDate.Attributes.Add("style", "display:none")
                liUnifiedRenewalDay.Attributes.Add("style", "display:none")
                If Session(CNIsUnifiedRenewalDayReadOnly) IsNot Nothing AndAlso Session(CNIsUnifiedRenewalDayReadOnly) = True Then
                    POLICYHEADER__UNIFIEDRENEWALDAY.Attributes.Remove("readonly")
                End If
            End If

            'Used in Javascript for cover date -Start
            If Session(CNRenewal) Then
                iMode = 1
            Else
                iMode = 0
            End If

            'WPR 105
            If Session(CNMode) = Mode.View Then
                POLICYHEADER__PAYMENTTERM.Enabled = False
                POLICYHEADER__COLLECTIONFREQUENCY.Enabled = False
                POLICYHEADER__PAYMENTMETHOD.Enabled = False
            ElseIf Session(CNMTAType) Is Nothing Then ' NB and REN
                POLICYHEADER__PAYMENTTERM.Enabled = True
                POLICYHEADER__COLLECTIONFREQUENCY.Enabled = True
                POLICYHEADER__PAYMENTMETHOD.Enabled = True
            ElseIf (Session(CNMTAType) = MTAType.REINSTATEMENT) And (oQuote.CoverStartDate = oQuote.InceptionTPI) Then ' reinstatement from inception
                POLICYHEADER__PAYMENTTERM.Enabled = True
                POLICYHEADER__COLLECTIONFREQUENCY.Enabled = True
                POLICYHEADER__PAYMENTMETHOD.Enabled = True
            Else
                POLICYHEADER__PAYMENTTERM.Enabled = False
                POLICYHEADER__COLLECTIONFREQUENCY.Enabled = False
                POLICYHEADER__PAYMENTMETHOD.Enabled = False
            End If

            If Session(CNMode) = Mode.View Then
                'Disable all cotrols 
                btnSaveQuote.Visible = False
                Dim oMaster As ContentPlaceHolder
                oMaster = GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName)
                DisableControls(oMaster)
                DisableControls(POLICYHEADER__COVERNOTEPANEL)
                DisableControls(POLICYHEADER__COVERDATESPANEL)
                DisableControls(liUnifiedRenewalDay)
                DisableControls(liAnniversaryDate)
                POLICYHEADER__BUSINESSTYPE.Enabled = False
                btnAgentCode.Enabled = False
                btnHandler.Enabled = False
            Else
                'Attach change tracker to all controls. So that we can ask for saving changes before leaving the page
                Dim oMaster As ContentPlaceHolder
                oMaster = GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName)
                AttachJavascripAttributeWithControls(oMaster, "onchange", "TrackChanges();")
            End If
            If Request("__EVENTARGUMENT") = "RefreshBusinessType" Then
                POLICYHEADER__CORRESPONDENCETYPE_SelectedIndexChange(sender, e)
            End If
            'Refreshing of agent 
            If Request("__EVENTARGUMENT") = "RefreshAgent" Then
                chkIsCoverNoteUsed.Enabled = True 'Enable teh cover not book checkbox
                FillCoverNoteBook()
                EnableAlternateRef() 'Check the AlternateRef settings
                FillContactPerson(POLICYHEADER__AGENT.Value)
                POLICYHEADER__CORRESPONDENCETYPE_SelectedIndexChange(sender, e)
                FillPaymentMethods()
                AgencyCancelled()
                btnNext.OnClientClick = "return AgencyCancellation();"

                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "TrackChanges", "TrackChanges();", True)
            End If

            CheckDebitOrderProcessing()

            'In case of NB and Renewal “Cover Start Date” will be enabled else disabled.
            If Session(CNMTAType) Is Nothing And Session(CNMode) <> Mode.View Then
                calCoverFromDate.Enabled = True
            Else
                calCoverFromDate.Enabled = False
            End If

            'In case of MTC and Backdated MTA , "Cover End Date" will be disabled.
            If Session(CNMTAType) = MTAType.CANCELLATION Or CType(Session(CNIsBackDatedMTA), Boolean) = True Then
                calCoverEndDate.Enabled = False
                calAnniversary.Enabled = False
                POLICYHEADER__UNIFIEDRENEWALDAY.Enabled = False
                POLICYHEADER__INSUREDNAME.Enabled = False
                POLICYHEADER__ALTERNATEREF.Enabled = False
                POLICYHEADER__ANALYSISCODE.Enabled = False
                POLICYHEADER__SUBBRANCH.Enabled = False
                POLICYHEADER__BUSINESSTYPE.Enabled = False
                btnHandler.Enabled = False
                POLICYHEADER__POLICYNUMBER.Enabled = False
            End If

            If Session(CNMTAType) = MTAType.TEMPORARY Then
                calCoverEndDate.Enabled = False
            End If

            'CR26
            If Session(CNRenewal) IsNot Nothing Then
                Dim sDisableCoverStartDateOnREN As String = ""
                oWebService = New NexusProvider.ProviderManager().Provider
                sDisableCoverStartDateOnREN = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.DisableCoverStartDateOnREN, NexusProvider.RiskTypeOptions.Code, oQuote.ProductCode, "")
                If sDisableCoverStartDateOnREN.Trim = "1" Then
                    POLICYHEADER__COVERSTARTDATE.Enabled = False
                    calCoverFromDate.Enabled = False
                End If
            End If
            'End 

            'For true monthly product, frequency should be defaulted to Monthly and user should not be allowed to change 
            If Session(CNIsTrueMonthlyPolicy) = True Then
                Dim nCt As Integer = 0
                Dim nCollectionFrequencyForMonthly As Integer = 0
                For nCt = 0 To POLICYHEADER__COLLECTIONFREQUENCY.Items.Count - 1
                    If POLICYHEADER__COLLECTIONFREQUENCY.Items(nCt).Description.ToUpper() = "MONTHLY" Then
                        nCollectionFrequencyForMonthly = POLICYHEADER__COLLECTIONFREQUENCY.Items(nCt).Key
                        Exit For
                    End If
                Next

                POLICYHEADER__COLLECTIONFREQUENCY.Value = nCollectionFrequencyForMonthly
                POLICYHEADER__COLLECTIONFREQUENCY.Enabled = False

                'CR0026 MTA Change 
                If oQuote IsNot Nothing Then
                    If Session(CNMode) <> Mode.View AndAlso Session(CNIsBackDatedMTA) = False AndAlso Session(CNMTAType) IsNot Nothing AndAlso (Session(CNMTAType) = MTAType.TEMPORARY OrElse Session(CNMTAType) = MTAType.PERMANENT) Then
                        Dim bIsAnniversaryDateEditable As Boolean = False
                        oWebService.IsAnniversaryDateEditable(oQuote.InsuranceFileKey, bIsAnniversaryDateEditable)
                        If bIsAnniversaryDateEditable Then
                            POLICYHEADER__ANNIVERSARY.Enabled = True
                            calAnniversary.Enabled = True
                        Else
                            POLICYHEADER__ANNIVERSARY.Enabled = False
                            calAnniversary.Enabled = False
                        End If
                    ElseIf CType(Session(CNRenewal), Boolean) = True Then
                        POLICYHEADER__ANNIVERSARY.Enabled = False
                        calAnniversary.Enabled = False
                    End If
                End If
                'End 
            End If

            If (Session(CNMTAType) = MTAType.REINSTATEMENT) AndAlso (oQuote.CoverStartDate = oQuote.InceptionTPI) Then
                POLICYHEADER__ANNIVERSARY.Enabled = True
                calAnniversary.Enabled = True
            End If

            ' Check if a backdated MTA version is in session.
            If Session(CNBaseInsuranceFileKey) IsNot Nothing AndAlso Session(CNBaseInsuranceFileKey) <> Session(CNInsuranceFileKey) Then
                pageContainerDiv.Enabled = False
                btnSaveQuote.Visible = False
            End If

            btnNext.Attributes.Add("onclick", "javascript:return ValidateCoverDates();")
            btnSaveQuote.Attributes.Add("onclick", "javascript:return ValidateCoverDates();")

            sMsg_CoverToDateInFutureValidationMessage = GetLocalResourceObject("sMsg_CoverToDateInFutureValidationMessage")
            sMsg_CoverToDateInFutureValidationMessage = sMsg_CoverToDateInFutureValidationMessage.Replace("#MonthForCoverToDateInFuture", hfMonthInFutureAllowedForCoverToDate.Value)
            sMsg_CoverFromDateInFutureValidationMessage = GetLocalResourceObject("sMsg_CoverFromDateInFutureValidationMessage")
            sMsg_CoverFromDateInFutureValidationMessage = sMsg_CoverFromDateInFutureValidationMessage.Replace("#MonthForCoverFromDateInFuture", hfMonthInFutureAllowedForCoverFromDate.Value)

            'Required for Co-Insurance-Lead
            Session(CNCoInsurancePage) = Nothing
            If Not Page.IsPostBack AndAlso Session(CNMode) Is Nothing AndAlso Request("newquote") IsNot Nothing Then
                Dim oBranchs As NexusProvider.BranchCollection = CType(Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches
                If oBranchs IsNot Nothing Then
                    For Each oBranch As NexusProvider.Branch In oBranchs
                        If oBranch.Code = POLICYHEADER__BRANCH.SelectedValue Then
                            If oBranch.BusinessType.Trim.ToUpper() = "AGENCY" OrElse oBranch.BusinessType.Trim.ToUpper() = "" AndAlso
                             oQuote IsNot Nothing AndAlso oQuote.AgentCode Is Nothing Then

                                POLICYHEADER__BUSINESSTYPE.Value = "AGENCY"
                                POLICYHEADER__AGENTCODE.Text = oBranch.AgentCode
                                If String.IsNullOrEmpty(POLICYHEADER__AGENTCODE.Text) Then
                                    POLICYHEADER__AGENTCODE.Text = oQuote.AgentCode
                                    POLICYHEADER__AGENT.Value = oQuote.AgentKey
                                    hAgentCode.Value = oQuote.AgentCode
                                Else
                                    POLICYHEADER__AGENTCODE.Text = oBranch.AgentCode
                                    POLICYHEADER__AGENT.Value = oBranch.AgentKey
                                    hAgentCode.Value = oBranch.AgentCode
                                End If
                                If Current.Session(CNLoginType) = LoginType.Agent Then
                                    btnAgentCode.Enabled = True
                                Else
                                    btnAgentCode.Enabled = False
                                End If

                                Exit For
                            Else
                                If String.IsNullOrEmpty(POLICYHEADER__AGENTCODE.Text) Then
                                    POLICYHEADER__BUSINESSTYPE.Value = "DIRECT"
                                    POLICYHEADER__AGENTCODE.Text = ""
                                    hAgentCode.Value = ""
                                    POLICYHEADER__AGENT.Value = ""
                                    btnAgentCode.Enabled = False
                                End If
                                Exit For
                            End If
                        End If
                    Next
                End If
            End If
            If (Session(CNMTAType) Is Nothing OrElse Session(CNRenewal) IsNot Nothing OrElse Session(CNMTAType) = MTAType.REINSTATEMENT) AndAlso ((POLICYHEADER__BUSINESSTYPE.Value.Trim() = "COIN LEAD") OrElse (POLICYHEADER__BUSINESSTYPE.Value.Trim() = "COIN FOLL")) Then
                POLICYHEADER__COINSURANCEPLACEMENT.Enabled = True
            Else
                POLICYHEADER__COINSURANCEPLACEMENT.Enabled = False
            End If
            If Session(CNSubAgents) Is Nothing Then
                Try
                    Dim oSubAgentResponse As New NexusProvider.SubAgentCollection
                    oWebService = New NexusProvider.ProviderManager().Provider
                    oSubAgentResponse = oWebService.GetSubAgents(oQuote.InsuranceFileKey)
                    Session.Add(CNSubAgents, oSubAgentResponse)
                Finally
                    oWebService = Nothing
                End Try
            End If
            If Request("__EVENTARGUMENT") = "ContinueAfterMulticurrency" Then
                ' Continue with normal btnNext flow
                btnNext_Click(btnNext, EventArgs.Empty)
            End If
        End Sub


        ''' <summary>
        ''' Cancel MTA Quote
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnCancelMTA_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelMTA.Click

            Dim oWebService As NexusProvider.ProviderBase
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            oWebService = New NexusProvider.ProviderManager().Provider

            oWebService.CancelQuote(oQuote, oQuote.BranchCode)

            ' Redirect to the client 360 view.
            Dim oParty As NexusProvider.BaseParty = Session(CNParty)
            Select Case True
                Case TypeOf oParty Is NexusProvider.PersonalParty
                    Response.Redirect("~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & oParty.Key.ToString() & "&Code=" & oParty.UserName, False)
                Case TypeOf oParty Is NexusProvider.CorporateParty
                    Response.Redirect("~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oParty.Key.ToString() & "&Code=" & oParty.UserName, False)
            End Select

        End Sub


        ''' <summary>
        ''' TO Get Number of months for given frequency. 
        ''' </summary>
        ''' <param name="v_oListType"></param>
        ''' <param name="v_sListCode"></param>
        ''' <param name="v_sCode"></param>
        ''' <param name="v_sBranchCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetNumberOfMonthsForGivenFrequency(ByVal v_oListType As NexusProvider.ListType,
                                             ByVal v_sListCode As String, ByVal v_sCode As String,
                                             Optional ByVal v_sBranchCode As String = Nothing) As String

            Dim sCode As String = Nothing
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim r_oList As NexusProvider.LookupListCollection
            Dim v_sXML As System.Xml.XmlElement = Nothing
            Dim sNumberOfMonths As String = Nothing

            r_oList = oWebService.GetList(v_oListType, v_sListCode, False, False, , , v_sBranchCode, v_sXML)

            If Not v_sXML Is Nothing Then
                Dim sXML As String = v_sXML.OuterXml
                Dim xmlDoc As New System.Xml.XmlDocument
                xmlDoc.PreserveWhitespace = False
                xmlDoc.LoadXml(sXML)

                Dim oNodeList As XmlNodeList
                oNodeList = xmlDoc.SelectNodes("/AdditionalDetails/" & v_sListCode)
                For Each oNode As XmlNode In oNodeList

                    If oNode.ChildNodes(3) IsNot Nothing Then
                        sCode = oNode.ChildNodes(2).InnerText
                        sNumberOfMonths = oNode.ChildNodes(6).InnerText
                    End If
                    If sCode.Trim().ToUpper() = v_sCode.Trim().ToUpper() Then
                        Exit For
                    End If
                Next
            End If
            Return sNumberOfMonths
        End Function

        ''' <summary>
        ''' This will attach a javascript function for given event for all controls within a container
        ''' </summary>
        ''' <param name="oContainer"></param>
        ''' <param name="sJavascriptEvent">javascript event name</param>
        ''' <param name="sJavascriptFunction">javascript function</param>
        ''' <remarks></remarks>
        Sub AttachJavascripAttributeWithControls(ByVal oContainer As Control, ByVal sJavascriptEvent As String, ByVal sJavascriptFunction As String)

            Dim oControl As Object
            For Each oControl In oContainer.Controls
                Select Case oControl.GetType.Name.ToUpper
                    Case "TEXTBOX", "HTMLINPUTTEXT", "DROPDOWNLIST", "HTMLINPUTHIDDEN", "LOOKUPLIST"
                        oControl.Attributes.Add(sJavascriptEvent, sJavascriptFunction)

                    Case "PANEL"
                        AttachJavascripAttributeWithControls(CType(oControl, Panel), sJavascriptEvent, sJavascriptFunction)

                    Case "PLACEHOLDER"
                        AttachJavascripAttributeWithControls(CType(oControl, PlaceHolder), sJavascriptEvent, sJavascriptFunction)

                    Case "UPDATEPANEL"

                        Dim oUpdPanel As UpdatePanel = CType(oControl, UpdatePanel)
                        If oUpdPanel.HasControls Then
                            For Each oCtrl As Object In oUpdPanel.Controls
                                AttachJavascripAttributeWithControls(oCtrl, sJavascriptEvent, sJavascriptFunction)
                            Next
                        End If

                    Case "HTMLGENERICCONTROL"
                        Dim oGenericCtrl As HtmlGenericControl = CType(oControl, HtmlGenericControl)
                        If oGenericCtrl.HasControls Then
                            For Each oCtrl As Object In oGenericCtrl.Controls
                                AttachJavascripAttributeWithControls(oCtrl, sJavascriptEvent, sJavascriptFunction)
                            Next
                        End If
                    Case "CHECKBOX"
                        'CheckBox Onchange event is not working, hence OnClick is added.
                        oControl.Attributes.Add("OnClick", sJavascriptFunction)

                End Select
            Next

        End Sub

        ''' <summary>
        ''' validating Anniversary Date
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="args"></param>
        ''' <remarks></remarks>
        Protected Sub ValidateAnniversaryDate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvAnniversaryDate.ServerValidate
            If (hfIsTrueMonthlyProduct.Value = "1") Then
                Dim iUnifiedRenewalDay As Integer = POLICYHEADER__UNIFIEDRENEWALDAY.SelectedValue
                Dim dtAnniversaryDate As Date = CDate(POLICYHEADER__ANNIVERSARY.Text)
                Dim dRenewalDate As Date = POLICYHEADER__RENEWAL.Text
                Dim iAnniversaryDay As Integer = dtAnniversaryDate.Day

                If IsDate(dtAnniversaryDate) Then
                    If (iAnniversaryDay <> POLICYHEADER__UNIFIEDRENEWALDAY.SelectedValue) Then
                        cvAnniversaryDate.ErrorMessage = sMsgAnniversaryDayEquallRenewalDay
                        args.IsValid = False
                    Else
                        If (dtAnniversaryDate < dRenewalDate) Then
                            cvAnniversaryDate.ErrorMessage = sMsgAnniversaryDayGreaterRenewalDay
                            args.IsValid = False
                        Else
                            args.IsValid = True
                        End If
                    End If
                Else
                    cvAnniversaryDate.ErrorMessage = sMsgInvalidAnniversaryDate
                    args.IsValid = False
                End If
            End If

        End Sub

        ''' <summary>
        ''' Enable the Alternate Ref based on the agnet selection
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub EnableAlternateRef()
            If Session(CNLoginType) = LoginType.Agent Then

                Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
                Dim oAgentSettings As NexusProvider.AgentSettings = Nothing
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

                If POLICYHEADER__AGENT.Value.Trim.Length <> 0 AndAlso POLICYHEADER__AGENT.Value.Trim <> "0" Then
                    GetAgentSettingsCall(oAgentSettings, POLICYHEADER__AGENT.Value)
                End If
                If oAgentSettings IsNot Nothing Then
                    If oAgentSettings.IsAlternateReferenceMandatory Then
                        POLICYHEADER__ALTERNATEREF.CssClass = "form-control field-mandatory"
                        vldrqdAlternateRef.Enabled = True
                    Else
                        POLICYHEADER__ALTERNATEREF.CssClass = "form-control"
                        vldrqdAlternateRef.Enabled = False
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' Logic for cover dates will be changed for the selected frequency
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub POLICYHEADER__FREQUENCY_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles POLICYHEADER__FREQUENCY.SelectedIndexChange
            Dim sSelectedVal As String = POLICYHEADER__FREQUENCY.Value
            Dim iMonth As Integer
            Dim dtResult As Date

            'Attach confirm save during postback
            If Page.IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "TrackChanges", "TrackChanges();", True)
            End If
            iMonth = CInt(GetNumberOfMonthsForGivenFrequency(NexusProvider.ListType.PMLookup, "Renewal_Frequency", sSelectedVal))
            hfRenewalFrequency.Value = iMonth
            If Date.TryParse(POLICYHEADER__COVERSTARTDATE.Text, dtResult) Then
                POLICYHEADER__COVERENDDATE.Text = CDate(POLICYHEADER__COVERSTARTDATE.Text).AddMonths(iMonth)
                POLICYHEADER__PROPOSALDATE.Text = POLICYHEADER__COVERSTARTDATE.Text

                'Checkhing the Value of Midnight Renewal Settings
                If hfMidnightRenewalSettings.Value = "1" Then
                    'Adding 366 days to Renewal Date and cover to date
                    POLICYHEADER__COVERENDDATE.Text = CDate(POLICYHEADER__COVERENDDATE.Text).AddDays(-1).ToShortDateString()
                    POLICYHEADER__RENEWAL.Text = CDate(POLICYHEADER__COVERENDDATE.Text).AddDays(1).ToShortDateString()
                Else
                    'Adding 365 days to Renewal Date
                    POLICYHEADER__RENEWAL.Text = CDate(POLICYHEADER__COVERENDDATE.Text).ToShortDateString()
                End If
            End If
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function CheckDuplicateClient() As Boolean
            If Session(CNRiskDuplicateClientCheck) IsNot Nothing Then
                If CType(Session(CNRiskDuplicateClientCheck), Boolean) = True Then
                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim oParty As NexusProvider.BaseParty = Session(CNParty)
                    Session.Add(CNIsTransferQuoteRequired, "True")
                    Session.Add(CNRiskDuplicateClientCheck, "False")
                    Return True
                Else
                    Session.Remove(CNRiskDuplicateClientCheck)
                    Return False
                End If
            End If
        End Function

        ''' <summary>
        ''' On next button button click, we need to save the quote object and need to unquote all risks
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNext.Click
            Dim bIsInBackDatedMode As Boolean = False
            Dim bDuplicateClientCheck As Boolean = False

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuoteSession As NexusProvider.Quote = Session(CNQuote)
            Dim oHeaderAndAgentCommission As NexusProvider.HeaderAndAgentCommission
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim sInsuranceFileTypeCode As String = ""
            Dim txtCoverstrDate As TextBox = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__COVERSTARTDATE"), TextBox)
            Dim dNBCoverStartDate As Date

            oHeaderAndAgentCommission = oWebService.GetHeaderAndAgentCommissionByKey(oQuoteSession.InsuranceFileKey)

            If oQuoteSession.InsuranceFileTypeCode IsNot Nothing Then
                sInsuranceFileTypeCode = UCase(oQuoteSession.InsuranceFileTypeCode.Trim())
            End If
            dNBCoverStartDate = oHeaderAndAgentCommission.InceptionDate.AddYears(-1)
            If sInsuranceFileTypeCode = "RENEWAL" And txtCoverstrDate.Text < dNBCoverStartDate Then
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ValidateCoverDatesForRenewal();", True)
            Else
                If Session(CNIsBackDatedMTA) = True Then
                    If Session(CNBaseInsuranceFileKey) IsNot Nothing AndAlso Session(CNInsuranceFileKey) IsNot Nothing Then
                        If Session(CNBaseInsuranceFileKey) <> Session(CNInsuranceFileKey) Then
                            bIsInBackDatedMode = True
                        End If
                    End If
                End If
                oWebService = New NexusProvider.ProviderManager().Provider

                If (hfDeletePolicyFromRenewal.Value <> String.Empty AndAlso hfDeletePolicyFromRenewal.Value = "True" AndAlso hfRenewalInsuranceFileKey.Value <> "") Then
                    Dim oQuote As NexusProvider.Quote
                    oQuote = oWebService.GetHeaderAndSummariesByKey(Convert.ToString(hfRenewalInsuranceFileKey.Value))
                    oWebService.DeleteRenewal(oQuote)
                    hfDeletePolicyFromRenewal.Value = "False"
                End If
                ' Check if a backdated MTA version is in session.
                If Session(CNMode) <> Mode.View And CType(Session(CNMode), Mode) <> Mode.Review And bIsInBackDatedMode = False Then
                    If Request("__EVENTARGUMENT") = "ContinueAfterMulticurrency" Then
                        Page.Validate()
                    End If
                    If Page.IsValid Then
                        Dim oQuote As NexusProvider.Quote = oWebService.GetHeaderAndSummariesByKey(DirectCast(Session(CNQuote), NexusProvider.Quote).InsuranceFileKey)

                        Session(CNQuote) = oQuote
                        SaveQuoteAndUnquoteAllRisks()
                        SavePolicyStandardWordings(sender)
                        'Retrieve quote object again

                        Try

                            oQuote = oWebService.GetHeaderAndSummariesByKey(oQuote.InsuranceFileKey)

                            For i As Integer = 0 To oQuote.Risks.Count - 1
                                If oQuote.Risks(i).OriginalRiskKey = 0 AndAlso oQuote.InsuranceFileTypeCode = "QUOTE" Then
                                    oWebService.UpdateRiskStatus(oQuote.InsuranceFileKey, oQuote.Risks(i).Key, NexusProvider.RiskStatusType.UNQUOTED, oQuote.BranchCode, oQuote.CoverStartDate)
                                End If
                                oWebService.GetRisk(oQuote.Risks(i).Key, i, oQuote, oQuote.BranchCode)
                            Next

                            oWebService.GetHeaderAndRisksByKey(oQuote)

                            Session(CNQuote) = oQuote
                            Session(CNCurrenyCode) = oQuote.CurrencyCode

                            If Session("MulticurrencyHandled") Is Nothing AndAlso Request("__EVENTARGUMENT") <> "ContinueAfterMulticurrency" AndAlso
                                    oQuote IsNot Nothing AndAlso oQuote.BaseCurrencyID <> oQuote.TransCurrencyID Then

                                ' Show multicurrency modal
                                Dim sUrl As String = AppSettings("WebRoot") & "Modal/Multicurrency.aspx?ProcessName=Policy&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750"
                                If HttpContext.Current.Session.IsCookieless Then
                                    sUrl = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() & "))/Modal/Multicurrency.aspx?ProcessName=Policy&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750"
                                End If

                                ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "ShowMulticurrency",
                                    "$(document).ready(function(){tb_show(null,'" & sUrl & "', null);});", True)
                                Session("MulticurrencyHandled") = True
                                Return ' Stop here
                            End If



                        Finally
                            oWebService = Nothing
                        End Try


                        If Session(CNMode) <> Mode.View And CType(Session(CNMode), Mode) <> Mode.Review Then

                            'Check DuplicateClient Response
                            bDuplicateClientCheck = CheckDuplicateClient()
                        End If
                        If (bDuplicateClientCheck = False Or (CType(Session(CNIsTransferQuoteRequired), Boolean) = True And Session(CNPartyKey) IsNot Nothing)) Then
                            If (oQuote.BusinessTypeCode.Trim.ToUpper = "COIN LEAD" OrElse oQuote.BusinessTypeCode.Trim.ToUpper = "COIN FOLL") AndAlso Session(CNCoInsurancePage) Is Nothing Then

                                Session("NextPage") = "~\secure\PremiumDisplay.aspx"
                                Response.Redirect("~/secure/CoinsuranceDetails.aspx")
                            End If
                        End If
                        Response.Redirect("~\secure\PremiumDisplay.aspx", False)
                    End If
                Else
                    Response.Redirect("~\secure\PremiumDisplay.aspx", False)
                End If
            End If
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub SaveQuoteAndUnquoteAllRisks()
            Dim bUnquoteRequired As Boolean = False
            Dim oProductConfiguration As Nexus.Library.Config.Product
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oNexus As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortalConfig As Nexus.Library.Config.Portal = oNexus.Portals.Portal(Portal.GetPortalID())
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            oProductConfiguration = oPortalConfig.Products.Product(oQuote.ProductCode)

            'Check if any value for requotefields has been changed
            bUnquoteRequired = UnquoteRiskRequired()

            'set all changed values to oQuote and update session
            oQuote.InsuranceFileRef = POLICYHEADER__POLICYNUMBER.Text
            oQuote.ProductCode = POLICYHEADER__PRODUCT.SelectedValue
            oQuote.InsuredName = POLICYHEADER__INSUREDNAME.Text
            oQuote.BranchCode = POLICYHEADER__BRANCH.SelectedValue
            Session(CNTransBranchCode) = POLICYHEADER__BRANCH.SelectedValue
            oQuote.SubBranchCode = POLICYHEADER__SUBBRANCH.SelectedValue
            oQuote.CurrencyCode = POLICYHEADER__CURRENCY.SelectedValue
            oQuote.FrequencyCode = POLICYHEADER__FREQUENCY.Value
            oQuote.HandlerCode = POLICYHEADER__HANDLER.Text
            oQuote.BusinessTypeCode = POLICYHEADER__BUSINESSTYPE.Value
            If oQuote.BusinessTypeCode.Trim.ToUpper() <> "DIRECT" Then
                oQuote.AgentCode = POLICYHEADER__AGENTCODE.Text
                oQuote.Agent = POLICYHEADER__AGENT.Value
                If POLICYHEADER__CONTACT_NAME.SelectedValue <> "" Then
                    oQuote.ContactUserKey = POLICYHEADER__CONTACT_NAME.SelectedValue
                    oQuote.ContactUserName = POLICYHEADER__CONTACT_NAME.SelectedItem.Text
                Else
                    oQuote.ContactUserKey = Nothing
                    oQuote.ContactUserName = ""
                End If
            Else
                oQuote.AgentCode = ""
                oQuote.Agent = ""
            End If

            Session(CNAgentType) = hAgentType.Value

            oQuote.AlternativeRef = POLICYHEADER__ALTERNATEREF.Text
            oQuote.PolicyStatusCode = POLICYHEADER__POLICYSTATUSCODE.Value
            oQuote.AnalysisCode = POLICYHEADER__ANALYSISCODE.Value

            oQuote.CoverStartDate = POLICYHEADER__COVERSTARTDATE.Text
            oQuote.CoverEndDate = POLICYHEADER__COVERENDDATE.Text
            oQuote.InceptionDate = POLICYHEADER__INCEPTION.Text
            oQuote.InceptionTPI = POLICYHEADER__INCEPTIONTPI.Text
            oQuote.RenewalDate = POLICYHEADER__RENEWAL.Text
            If Not String.IsNullOrEmpty(POLICYHEADER__QUOTEEXPIRYDATE.Text) Then
                oQuote.QuoteExpiryDate = POLICYHEADER__QUOTEEXPIRYDATE.Text
            End If

            If Not String.IsNullOrEmpty(POLICYHEADER__PROPOSALDATE.Text) Then
                oQuote.ProposalDate = POLICYHEADER__PROPOSALDATE.Text
            End If

            oQuote.Regarding = POLICYHEADER__REGARDING.Text

            oQuote.ReferredAtRenewal = POLICYHEADER__REFERREDATRENEWAL.Checked
            oQuote.ReferredAtMTA = POLICYHEADER__REFERREDATMTA.Checked
            oQuote.ConsolidatedLeadAgentCommission = POLICYHEADER__CONSOLIDATEDLEADAGENTCOMMISSION.Checked
            oQuote.RenewalMethodCode = POLICYHEADER__RENEWALMETHOD.Value
            If Not String.IsNullOrEmpty(POLICYHEADER__LTUEXPIRYDATE.Text) Then
                oQuote.LTUExpiryDate = POLICYHEADER__LTUEXPIRYDATE.Text
            End If
            oQuote.StopReasonCode = POLICYHEADER__STOPREASON.Value

            If Not String.IsNullOrEmpty(POLICYHEADER__LAPSECANCELDATE.Text) Then
                oQuote.LapseCancelDate = POLICYHEADER__LAPSECANCELDATE.Text
                oQuote.LapseCancelReasonCode = POLICYHEADER__LAPSECANCELREASON.Value
            End If

            If Session(CNIsTrueMonthlyPolicy) = True Then
                oQuote.RenewalDayNo = POLICYHEADER__UNIFIEDRENEWALDAY.SelectedValue
                oQuote.AnniversaryDate = POLICYHEADER__ANNIVERSARY.Text
            End If

            If liUnderwritingYear.Visible = True Then
                oQuote.UnderwritingYearId = POLICYHEADER__UNDERWRITINGYEAR.Value
            End If


            If Not String.IsNullOrEmpty(POLICYHEADER__COLLECTIONFREQUENCY.Value) Then
                oQuote.CollectionFrequency = POLICYHEADER__COLLECTIONFREQUENCY.Value
            Else
                oQuote.CollectionFrequency = 0
            End If

            If Not String.IsNullOrEmpty(POLICYHEADER__PAYMENTTERM.Value) Then
                oQuote.PaymentTerm = Convert.ToInt32(POLICYHEADER__PAYMENTTERM.Value)
            Else
                oQuote.PaymentTerm = 0
            End If

            oQuote.PaymentMethodCode = POLICYHEADER__PAYMENTMETHOD.SelectedValue
            oQuote.PaymentMethod = POLICYHEADER__PAYMENTMETHOD.SelectedValue
            If POLICYHEADER__COVERNOTEBOOKNO.SelectedIndex > 0 Then
                oQuote.CoverNoteBookNumber = POLICYHEADER__COVERNOTEBOOKNO.SelectedItem.Text

                If Trim(Convert.ToString(POLICYHEADER__COVERNOTESHEETNO.SelectedItem.Text)) <> "" Then
                    oQuote.CoverNoteSheetNumber = POLICYHEADER__COVERNOTESHEETNO.SelectedItem.Text
                    oQuote.CoverNoteSheetNumberSpecified = True
                End If
            End If
            If (oQuote.BusinessTypeCode.Trim.ToUpper() = "COIN LEAD") OrElse (oQuote.BusinessTypeCode.Trim.ToUpper() = "COIN FOLL") Then
                oQuote.CoinsurancePlacement = POLICYHEADER__COINSURANCEPLACEMENT.SelectedValue
            End If
            oQuote.OldPolicyNumber = POLICYHEADER__OldPolicyNo.Text
            oQuote.CorrespondenceType = POLICYHEADER__CORRESPONDENCETYPE.Value.Trim
            oQuote.DefaultPreferredCorrespondence = POLICYHEADER__DEFAULTCORRESPONDENCECODE.Value.Trim
            oQuote.IsAgentReceiveCorrespondence = If(POLICYHEADER__RECEIVESCLIENTCORRESPONDENCE.Value = 1, True, False)
            Session(CNQuote) = oQuote

            Try
                Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)

                'if user has not supplied the Quote Expiry date in case of Renewal
                If Session(CNRenewal) IsNot Nothing AndAlso oQuote.QuoteExpiryDate = Date.MinValue Then
                    oQuote.QuoteExpiryDate = oQuote.CoverStartDate.AddDays(1)
                End If
                If Session(CNMTAType) = MTAType.CANCELLATION Then
                    oQuote.LapseCancelDate = oQuote.LapseDate
                End If

                If oUserDetails IsNot Nothing Then
                    oWebService.UpdateQuotev2(oQuote, oQuote.BranchCode, oQuote.SubBranchCode, oUserDetails.Key)
                Else
                    oWebService.UpdateQuotev2(oQuote, oQuote.BranchCode, oQuote.SubBranchCode)
                End If

            Finally
                oWebService = Nothing
            End Try

            Session(CNQuote) = oQuote
            Dim oPaymentOptions As Config.PaymentTypes = oNexusConfig.Portals.Portal(Portal.GetPortalID()).PaymentTypes
            Dim nSelectedPaymentIndex As Integer = 0
            For Each oPaymentType1 As Config.PaymentType In oPaymentOptions
                If oPaymentType1.Type.Trim() = oQuote.PaymentMethodCode Then
                    Session(CNSelectedPaymentIndex) = nSelectedPaymentIndex
                    Exit For
                End If
                nSelectedPaymentIndex = nSelectedPaymentIndex + 1
            Next

            If bUnquoteRequired = True Then
                'Write a code to unquote the risks that are not already unquoted
                For Each oRisk As NexusProvider.Risk In oQuote.Risks
                    If oRisk.StatusCode.Trim().ToUpper() <> "UNQUOTED" Then
                        Try
                            oWebService = New NexusProvider.ProviderManager().Provider
                            oWebService.UpdateRiskStatus(oQuote.InsuranceFileKey, oRisk.Key, NexusProvider.RiskStatusType.UNQUOTED, oQuote.BranchCode)
                        Finally
                            oWebService = Nothing
                        End Try
                    End If
                Next
            End If
            If Session(CNSubAgents) IsNot Nothing Then
                Try
                    oWebService = New NexusProvider.ProviderManager().Provider
                    oWebService.UpdateSubAgents(oQuote, Session(CNSubAgents))
                Finally
                    oWebService = Nothing
                End Try
            End If

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function UnquoteRiskRequired() As Boolean
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oProductConfiguration As Nexus.Library.Config.Product
            Dim oNexus As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortalConfig As Nexus.Library.Config.Portal = oNexus.Portals.Portal(Portal.GetPortalID())
            Dim sRequoteFields As String = ""
            Dim bUnquoteRequired As Boolean = False
            Dim sControlId As String = ""

            oProductConfiguration = oPortalConfig.Products.Product(oQuote.ProductCode)

            sRequoteFields = oProductConfiguration.RequoteFields
            If Not String.IsNullOrEmpty(sRequoteFields) Then
                'set all changed values to oQuote
                Dim aRequoteFields() As String = sRequoteFields.Split(",")


                For iCt As Int16 = 0 To aRequoteFields.GetUpperBound(0)
                    sControlId = "POLICYHEADER__" + aRequoteFields.GetValue(iCt).ToString().Trim().ToUpper()
                    Select Case sControlId
                        Case "POLICYHEADER__ANNIVERSARY"
                            If Session(CNIsTrueMonthlyPolicy) = True Then
                                If oQuote.AnniversaryDate <> POLICYHEADER__ANNIVERSARY.Text Then
                                    bUnquoteRequired = True
                                    Exit For
                                End If
                            End If
                        Case "POLICYHEADER__ANALYSISCODE"
                            If oQuote.AnalysisCode <> POLICYHEADER__ANALYSISCODE.Value Then
                                bUnquoteRequired = True
                                Exit For
                            End If
                        Case "POLICYHEADER__AGENTCODE"
                            If oQuote.AgentCode <> POLICYHEADER__AGENTCODE.Text Then
                                bUnquoteRequired = True
                                Exit For
                            End If
                        Case "POLICYHEADER__AGENT"
                            If oQuote.Agent <> POLICYHEADER__AGENT.Value Then
                                bUnquoteRequired = True
                                Exit For
                            End If
                        Case "POLICYHEADER__BRANCH"
                            If oQuote.BranchCode <> POLICYHEADER__BRANCH.SelectedValue Then
                                bUnquoteRequired = True
                                Exit For
                            End If
                        Case "POLICYHEADER__CURRENCY"
                            If oQuote.CurrencyCode <> POLICYHEADER__CURRENCY.SelectedValue Then
                                bUnquoteRequired = True
                                Exit For
                            End If
                        Case "POLICYHEADER__BUSINESSTYPE"
                            If oQuote.BusinessTypeCode <> POLICYHEADER__BUSINESSTYPE.Value Then
                                bUnquoteRequired = True
                                Exit For
                            End If
                    End Select
                Next
            End If

            Return bUnquoteRequired

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnSaveQuote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveQuote.Click
            If Page.IsValid Then
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                If (hfDeletePolicyFromRenewal.Value <> String.Empty AndAlso hfDeletePolicyFromRenewal.Value = "True" AndAlso hfRenewalInsuranceFileKey.Value <> "") Then
                    Dim oQuoteRenewal As NexusProvider.Quote = oWebService.GetHeaderAndSummariesByKey(Convert.ToString(hfRenewalInsuranceFileKey.Value))
                    oWebService.DeleteRenewal(oQuoteRenewal)
                    hfDeletePolicyFromRenewal.Value = "False"
                End If

                Dim oQuote As NexusProvider.Quote = oWebService.GetHeaderAndSummariesByKey(DirectCast(Session(CNQuote), NexusProvider.Quote).InsuranceFileKey)
                Session(CNQuote) = oQuote
                SaveQuoteAndUnquoteAllRisks()
                SavePolicyStandardWordings(sender)
                'Unlock the Quote if Locked.
                UnlockQuote()
                'redirecting the user to Client details page if he clicks on Save Quote button
                ''need to check if the Login User is an Agent/Direct Registered Client
                Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
                If CType(Session.Item(CNLoginType), LoginType) = LoginType.Agent Then

                    Dim oParty As NexusProvider.BaseParty = Session(CNParty)
                    Select Case True
                        Case TypeOf oParty Is NexusProvider.PersonalParty
                            Response.Redirect("~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & oParty.Key.ToString() & "&Code=" & oParty.UserName, False)
                        Case TypeOf oParty Is NexusProvider.CorporateParty
                            Response.Redirect("~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oParty.Key.ToString() & "&Code=" & oParty.UserName, False)
                    End Select

                    oParty = Nothing
                Else
                    Response.Redirect(oPortal.ClientStartPage.Trim, False)
                End If
            End If
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        ''' 
        Protected Sub POLICYHEADER__BRANCH_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles POLICYHEADER__BRANCH.SelectedIndexChanged
            If POLICYHEADER__BRANCH.SelectedValue.Trim.Length > 0 Then
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oUserAuthority As New NexusProvider.UserAuthority
                'Attach confirm save during postback
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "TrackChanges", "TrackChanges();", True)

                oUserAuthority.UserCode = Session(CNLoginName)
                ''Pass Allow Edit Agent during MTA/MTC option
                oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.AgentEditableDuringMTAMTC
                ''Get User Authority Value
                oWebService.GetUserAuthorityValue(oUserAuthority)

                'Get all the branches for an agent 
                Dim oAgentSettings As NexusProvider.AgentSettings = Nothing

                If POLICYHEADER__AGENT.Value.Trim.Length <> 0 AndAlso POLICYHEADER__AGENT.Value.Trim <> "0" Then
                    GetAgentSettingsCall(oAgentSettings, POLICYHEADER__AGENT.Value)
                End If
                Dim bIsAgentBranch As Boolean = False
                'Loop through all the branches returned from above SAM Method
                If oAgentSettings IsNot Nothing AndAlso oAgentSettings.AgentBranchCollection IsNot Nothing Then
                    For i = 0 To oAgentSettings.AgentBranchCollection.Count - 1
                        ' Check Agent branchs(Allowed branches) with selected branch
                        If POLICYHEADER__BRANCH.SelectedValue.Trim.ToUpper = oAgentSettings.AgentBranchCollection(i).Code.Trim.ToUpper Then
                            bIsAgentBranch = True
                            Exit For
                        End If
                    Next
                End If

                'Agent does not have permission to changed branch
                If bIsAgentBranch = False Then
                    'if selected branch not exist in allowed branches for the agent, then show a validation message using javascript. 
                    'and reset selected branch value for setting previous value
                    If oAgentSettings IsNot Nothing AndAlso (Session(CNMTAType) = MTAType.PERMANENT Or Session(CNMTAType) = MTAType.TEMPORARY Or Session(CNMTAType) = MTAType.CANCELLATION) Then
                        Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                        'Show Alert Message for not to changing branch
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowValidation", "ShowValidationAndResetBranch('" + Session(CNTransBranchCode) + "');", True)
                        'Reset Branch to be previous one in oQuote object
                        POLICYHEADER__BRANCH.SelectedValue = Session(CNTransBranchCode)
                        Exit Sub
                    Else
                        'We need to clear agent code on change of branch code as agent will be searched for the selected branch only
                        POLICYHEADER__AGENTCODE.Text = ""
                        POLICYHEADER__AGENT.Value = ""
                        hAgentCode.Value = ""
                        hAgentType.Value = ""
                    End If
                End If

                'Populating of the sub-branches based on the currenct selection
                FillSubBranches(POLICYHEADER__BRANCH.SelectedValue)

                'Session updation with the latest values, this session is used to find the user selected branch
                Session(CNTransBranchCode) = POLICYHEADER__BRANCH.SelectedValue

                'Populating of the currency based on the current selection
                FillCurrency()

                'For Agent/Employee login Population of the cover note book
                If Session(CNLoginType) = LoginType.Agent Then
                    Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
                    If oUserDetails IsNot Nothing Then
                        FillCoverNoteBook()
                    End If
                End If
            End If
            If Session(CNMode) Is Nothing Then
                Dim oBranchs As NexusProvider.BranchCollection = CType(Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches
                If oBranchs IsNot Nothing Then
                    For Each oBranch As NexusProvider.Branch In oBranchs
                        If oBranch.Code = POLICYHEADER__BRANCH.SelectedValue Then
                            If oBranch.BusinessType.Trim.ToUpper() = "AGENCY" OrElse oBranch.BusinessType.Trim.ToUpper() = "" Then
                                POLICYHEADER__BUSINESSTYPE.Value = "AGENCY"
                                POLICYHEADER__AGENTCODE.Text = oBranch.AgentCode
                                POLICYHEADER__AGENT.Value = oBranch.AgentKey
                                hAgentCode.Value = oBranch.AgentCode
                                btnAgentCode.Enabled = True
                                Exit For
                            Else
                                POLICYHEADER__BUSINESSTYPE.Value = "DIRECT"
                                POLICYHEADER__AGENTCODE.Text = ""
                                hAgentCode.Value = ""
                                POLICYHEADER__AGENT.Value = ""
                                btnAgentCode.Enabled = False
                                Exit For
                            End If
                        End If
                    Next
                End If
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "DisableTextboxForFindControls", "DisableTextboxForFindControls();", True)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub POLICYHEADER__BUSINESSTYPE_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles POLICYHEADER__BUSINESSTYPE.SelectedIndexChange
            'Attach confirm save during postback
            If Page.IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "TrackChanges", "TrackChanges();", True)
            End If
            OnBusinessTypeChange()
            POLICYHEADER__CORRESPONDENCETYPE_SelectedIndexChange(sender, e)
            CheckDebitOrderProcessing()
            If (Session(CNMTAType) Is Nothing OrElse Session(CNRenewal) IsNot Nothing OrElse Session(CNMTAType) = MTAType.REINSTATEMENT) AndAlso ((POLICYHEADER__BUSINESSTYPE.Value.Trim() = "COIN LEAD") OrElse (POLICYHEADER__BUSINESSTYPE.Value.Trim() = "COIN FOLL")) Then
                POLICYHEADER__COINSURANCEPLACEMENT.Enabled = True
            Else
                POLICYHEADER__COINSURANCEPLACEMENT.Enabled = False
                If (Session(CNMTAType) Is Nothing OrElse Session(CNMTAType) = MTAType.REINSTATEMENT OrElse Session(CNRenewal) IsNot Nothing) Then
                    POLICYHEADER__COINSURANCEPLACEMENT.SelectedIndex = -1
                End If

            End If
            If POLICYHEADER__BUSINESSTYPE.Value <> "AGENCY" Then

                If POLICYHEADER__COVERNOTEBOOKNO.Items.Count > 0 Then
                    POLICYHEADER__COVERNOTEBOOKNO.ClearSelection()
                End If
                If POLICYHEADER__COVERNOTESHEETNO.Items.Count > 0 Then
                    POLICYHEADER__COVERNOTESHEETNO.ClearSelection()
                End If
                chkIsCoverNoteUsed.Checked = False
                POLICYHEADER__COVERNOTEPANEL.Enabled = False

            End If
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub OnBusinessTypeChange()

            Dim bAllowEditAgentDuringMTAMTC As Boolean = True
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oUserAuthority As New NexusProvider.UserAuthority
            oUserAuthority.UserCode = Session(CNLoginName)
            'Pass Allow Edit Agent during MTA/MTC option
            oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.AgentEditableDuringMTAMTC
            oWebService.GetUserAuthorityValue(oUserAuthority)
            bAllowEditAgentDuringMTAMTC = oUserAuthority.UserAuthorityValue

            If POLICYHEADER__BUSINESSTYPE.Value.Trim.Length <> 0 Then
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                If POLICYHEADER__BUSINESSTYPE.Value.Trim.ToUpper = "DIRECT" Then
                    'Check if "Allow agent change" is enabled for user
                    'Check if user is doing MTA/MTC
                    'Check if existing quote is agency business
                    If Session(CNMTAType) IsNot Nothing And bAllowEditAgentDuringMTAMTC = False And oQuote.BusinessTypeCode.ToUpper <> "DIRECT" Then
                        'Show alert message
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowValidation", "alert('Cannot change to Direct Business this will remove the Agent.\nInsufficient user authority to remove agent');", True)
                        'Revert the value as before
                        POLICYHEADER__BUSINESSTYPE.Value = oQuote.BusinessTypeCode
                    Else
                        'in case of the DIRECT business cover note panel should be disable
                        'agent panel should also be disable
                        btnAgentCode.Enabled = False
                        POLICYHEADER__COVERNOTEPANEL.Enabled = False
                        POLICYHEADER__AGENTCODE.Text = String.Empty
                        POLICYHEADER__AGENT.Value = String.Empty
                        POLICYHEADER__AGENTCODE.CssClass = "form-control"
                        DisableControls(POLICYHEADER__COVERNOTEPANEL)
                        FillPaymentMethods()
                        rfvAgentCode.Enabled = False
                        vldrqdCoinsurancePlacement.Enabled = False
                    End If
                Else
                    If Session(CNMTAType) IsNot Nothing And bAllowEditAgentDuringMTAMTC = False AndAlso Session(CNMTAType) <> MTAType.REINSTATEMENT Then
                        btnAgentCode.Enabled = False
                    Else
                        btnAgentCode.Enabled = True
                        POLICYHEADER__COVERNOTEPANEL.Enabled = True
                        POLICYHEADER__AGENTCODE.CssClass = "form-control field-mandatory"
                    End If
                    FillPaymentMethods()
                    rfvAgentCode.Enabled = True
                    If (POLICYHEADER__BUSINESSTYPE.Value.Trim.ToUpper = "COIN LEAD" Or POLICYHEADER__BUSINESSTYPE.Value.Trim.ToUpper = "COIN FOLL") Then
                        Dim oCoInsuranceAgentPerPolicyBasis As NexusProvider.OptionTypeSetting
                        oCoInsuranceAgentPerPolicyBasis = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5026)
                        If oCoInsuranceAgentPerPolicyBasis.OptionValue = "1" Then
                            rfvAgentCode.Enabled = False
                            POLICYHEADER__AGENTCODE.CssClass = "form-control"
                        End If
                        vldrqdCoinsurancePlacement.Enabled = True
                    Else
                        vldrqdCoinsurancePlacement.Enabled = False
                    End If
                End If
            End If

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "DisableTextboxForFindControls", "DisableTextboxForFindControls();", True)
        End Sub

        ''' <summary>
        ''' This will check debit order processing and will mark the mandatory fields
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub CheckDebitOrderProcessing()

            If ViewState("DebitOrderOptionType") Is Nothing Then
                Dim oDebitOrderOptionType As New NexusProvider.OptionTypeSetting
                oWebService = New NexusProvider.ProviderManager().Provider
                oDebitOrderOptionType = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, 107)
                ViewState.Add("DebitOrderOptionType", oDebitOrderOptionType.OptionValue)
            End If
            If ViewState("DebitOrderOptionType") = "1" Then
                If POLICYHEADER__PAYMENTMETHOD.SelectedValue = "Invoice" Then
                    rfvCollectionFrequency.Enabled = True
                    POLICYHEADER__COLLECTIONFREQUENCY.CssClass = "form-control field-mandatory"

                    rfvPaymentTerm.Enabled = True
                    POLICYHEADER__PAYMENTTERM.CssClass = "form-control field-mandatory"
                Else
                    rfvCollectionFrequency.Enabled = False
                    POLICYHEADER__COLLECTIONFREQUENCY.CssClass = "form-control"

                    rfvPaymentTerm.Enabled = False
                    POLICYHEADER__PAYMENTTERM.CssClass = "form-control"
                End If
            Else
                updPaymentMethod.Visible = False
            End If


        End Sub

#Region "Fill DropDowns"

        ''' <summary>
        ''' This method fill the cover note book
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub FillCoverNoteBook()
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            'Dim txtAgent As HiddenField = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__AGENT"), HiddenField)
            'Dim ddlCoverNoteBookNo As DropDownList = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__COVERNOTEBOOKNO"), DropDownList)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

            'if valid agent key is selected/entered
            If POLICYHEADER__AGENT.Value.Trim.Length > 0 Then
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                Dim oCoverNote As New NexusProvider.CoverNote
                Dim oCoverNoteCollection As New NexusProvider.CoverNoteCollection
                Dim oTempCoverNoteCollection As New NexusProvider.CoverNoteCollection
                Dim iCounter As Integer = 0

                EnableControls(POLICYHEADER__COVERNOTEPANEL)
                chkIsCoverNoteUsed.Enabled = True
                chkIsCoverNoteUsed.Checked = False
                'Populating of the valid cover not book
                'based on the valid agent key and branch selected by user
                'Only Issued status cover not book should be populated
                oCoverNote.CoverNoteBranchCode = Session(CNTransBranchCode)
                oCoverNote.AgentKey = POLICYHEADER__AGENT.Value
                oCoverNote.CoverNoteBookStatusCode = "ISSUED"
                oTempCoverNoteCollection = oWebService.FindCoverNoteBooks(oCoverNote)
                For iCounter = 0 To oTempCoverNoteCollection.Count - 1
                    If oTempCoverNoteCollection(iCounter).CoverNoteStatusDescription IsNot Nothing And
                    oTempCoverNoteCollection(iCounter).CoverNoteStatusDescription.Trim.ToUpper = "ISSUED" _
                    And CDate(oTempCoverNoteCollection(iCounter).EffectiveDate).ToShortDateString <= CDate(Date.Now.ToShortDateString) Then
                        oCoverNoteCollection.Add(oTempCoverNoteCollection(iCounter))
                    End If
                Next

                'Set "Please select" as default for both CoverNote book and sheet No
                POLICYHEADER__COVERNOTEBOOKNO.Items.Clear()
                POLICYHEADER__COVERNOTEBOOKNO.SelectedIndex = -1
                POLICYHEADER__COVERNOTEBOOKNO.DataSource = oCoverNoteCollection
                POLICYHEADER__COVERNOTEBOOKNO.DataTextField = "BookNumber"
                POLICYHEADER__COVERNOTEBOOKNO.DataValueField = "CoverNoteBookKey"
                POLICYHEADER__COVERNOTEBOOKNO.DataBind()
                POLICYHEADER__COVERNOTEBOOKNO.Items.Insert(0, (New ListItem("(Please Select)", "")))
                POLICYHEADER__COVERNOTEBOOKNO.SelectedIndex = 0
                POLICYHEADER__COVERNOTEBOOKNO.Enabled = True
            End If
        End Sub

        ''' <summary>
        ''' This method fill the product 
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub FillProduct()
            Dim oProducts As Config.Products = CType(WebConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Products
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)

            For Each oProduct As Config.Product In oProducts
                POLICYHEADER__PRODUCT.Items.Add(New ListItem(oProduct.Name, oProduct.ProductCode))
            Next
            POLICYHEADER__PRODUCT.SelectedValue = oQuote.ProductCode
            POLICYHEADER__PRODUCT.Enabled = False

        End Sub

        ''' <summary>
        ''' This method fill the Branches
        ''' </summary>
        ''' <param name="oBranchCode"></param>
        ''' <remarks></remarks>
        Private Sub FillBranches(Optional ByVal oBranchCode As String = Nothing)
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            'If branch code is available in oQuote object
            If oBranchCode Is Nothing Then
                oBranchCode = oQuote.BranchCode
            End If

            'Fill Branch for Agent/Employee login
            If Session(CNLoginType) = LoginType.Agent Then
                If oUserDetails IsNot Nothing Then
                    For i As Integer = 0 To oUserDetails.ListOfBranches.Count - 1
                        Dim lstBranch As New ListItem
                        lstBranch.Text = oUserDetails.ListOfBranches(i).Description.ToString
                        lstBranch.Value = oUserDetails.ListOfBranches(i).Code.ToString
                        POLICYHEADER__BRANCH.Items.Add(lstBranch)
                        POLICYHEADER__BRANCH.DataBind()
                    Next
                End If
            Else 'For Customer login
                Dim lstBranch As New ListItem
                lstBranch.Text = oNexusConfig.BranchCode
                lstBranch.Value = oNexusConfig.BranchCode
                POLICYHEADER__BRANCH.Items.Add(lstBranch)
                POLICYHEADER__BRANCH.DataBind()
            End If
            If Session(CNMode) = Mode.View Then
                If (POLICYHEADER__BRANCH.Items.IndexOf(POLICYHEADER__BRANCH.Items.FindByValue(oQuote.BranchCode))) < 0 Then
                    Dim lstQuoteBranch As New ListItem
                    lstQuoteBranch.Text = GetDescriptionForCode(NexusProvider.ListType.PMLookup, oQuote.BranchCode, "Source")
                    lstQuoteBranch.Value = oQuote.BranchCode
                    POLICYHEADER__BRANCH.Items.Add(lstQuoteBranch)
                    POLICYHEADER__BRANCH.DataBind()
                    lstQuoteBranch = Nothing
                End If
            End If

            'Setting the values in Session based on the value from oQuote object
            If oQuote.BranchCode IsNot Nothing Then
                Session(CNTransBranchCode) = oQuote.BranchCode
                POLICYHEADER__BRANCH.SelectedValue = Session(CNTransBranchCode)
            End If

        End Sub

        ''' <summary>
        ''' This methods fill the sub branches by default or based on the value passed in it.
        ''' </summary>
        ''' <param name="oBranchCode"></param>
        ''' <remarks></remarks>
        Private Sub FillSubBranches(Optional ByVal oBranchCode As String = Nothing)
            'Fill Sub Branch
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oLookUP As New NexusProvider.LookupListCollection
            'sam call to retreive the list of branch from table source
            oLookUP = oWebService.GetList(NexusProvider.ListType.PMLookup, "Source", False, False, "Source_ID")
            'Retreival of the Branch Key, which will latet identify the sub-branch
            'sam need barnch key to find the respective sub-branches of the selected branches
            Dim iBranchKey As Integer = 0
            For iBranchCount As Integer = 0 To oLookUP.Count - 1
                If oLookUP(iBranchCount).Code = oBranchCode Then
                    iBranchKey = oLookUP(iBranchCount).Key
                    Exit For
                End If
            Next
            'sam call to retreive the list of sub-branch from table source
            oLookUP = Nothing
            oLookUP = oWebService.GetList(NexusProvider.ListType.PMLookup, "Sub_Branch", True, False, "Source_ID", iBranchKey, Session(CNTransBranchCode))

            'Populating the sub-branch control with the retreived values
            'If ddlSubBranch IsNot Nothing Then
            'existing items cleared
            POLICYHEADER__SUBBRANCH.Items.Clear()
            For iSubBranchCount As Integer = 0 To oLookUP.Count - 1
                Dim lstSubBranch As New ListItem
                lstSubBranch.Text = oLookUP(iSubBranchCount).Description
                lstSubBranch.Value = Trim(oLookUP(iSubBranchCount).Code)
                POLICYHEADER__SUBBRANCH.Items.Add(lstSubBranch)
                POLICYHEADER__SUBBRANCH.DataBind()
            Next
        End Sub
        ''' <summary>
        ''' This method fill the currency based on the Branch in session
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub FillCurrency()
            'Fill Currency
            Dim oCurrencyCollection As NexusProvider.CurrencyCollection
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim bIsCurrencyExistForBranch As Boolean = False
            'Sam call to find the currency based on the branch in session
            oCurrencyCollection = oWebService.GetCurrenciesByBranch(POLICYHEADER__BRANCH.SelectedValue)

            'Populating the currency control bansed on results returned
            'Making collection empty
            POLICYHEADER__CURRENCY.Items.Clear()
            For i As Integer = 0 To oCurrencyCollection.Count - 1
                Dim lstCurrency As New ListItem
                lstCurrency.Text = oCurrencyCollection.Item(i).Description.ToString
                lstCurrency.Value = Trim(oCurrencyCollection.Item(i).CurrencyCode.ToString)
                If oQuote.CurrencyCode = Trim(oCurrencyCollection.Item(i).CurrencyCode.ToString) Then
                    bIsCurrencyExistForBranch = True
                End If
                POLICYHEADER__CURRENCY.Items.Add(lstCurrency)
            Next
            POLICYHEADER__CURRENCY.DataBind()

            'Setting of the values based on the oQuote.CurrencyCode
            If oQuote IsNot Nothing Then
                If oQuote.CurrencyCode IsNot Nothing Then
                    If oQuote.CurrencyCode.Trim.Length > 0 And bIsCurrencyExistForBranch = True Then
                        POLICYHEADER__CURRENCY.SelectedValue = oQuote.CurrencyCode
                    Else
                        POLICYHEADER__CURRENCY.SelectedValue = oCurrencyCollection(0).CurrencyCode.Trim()
                    End If
                Else
                    POLICYHEADER__CURRENCY.SelectedIndex = 0
                End If
            End If

        End Sub

        Private Sub FillUnifiedRenewalDays()
            'existing items cleared
            POLICYHEADER__UNIFIEDRENEWALDAY.Items.Clear()
            'Fill items from 1 to 31
            For iRenewalDay As Integer = 1 To 31
                Dim lstRenewalDay As New ListItem
                lstRenewalDay.Text = iRenewalDay
                lstRenewalDay.Value = iRenewalDay
                POLICYHEADER__UNIFIEDRENEWALDAY.Items.Add(lstRenewalDay)
                POLICYHEADER__UNIFIEDRENEWALDAY.DataBind()
            Next
        End Sub


        ' declaring local variables for getting UserAuthority
        Dim bUserAgentCollection As Boolean = False
        Dim bUserPayNow As Boolean = False
        Dim bUserBankGuarantee As Boolean = False
        Dim bUserCashDeposit As Boolean = False
        Dim bUserDirectDebit As Boolean = False

        ' declaring local variables for getting Product Payment options
        Dim bProductAgentCollection As Boolean = False
        Dim bProductPayNow As Boolean = False
        Dim bProductBankGuarantee As Boolean = False
        Dim bProductCashDeposit As Boolean = False
        Dim bProductDirectDebit As Boolean = False

        ' declaring local variables for getting Agent Payment options
        Dim bAgentAgentCollection As Boolean = False
        Dim bAgentPayNow As Boolean = False
        Dim bAgentBankGuarantee As Boolean = False
        Dim bAgentCashDeposit As Boolean = False
        Dim bAgentDirectDebit As Boolean = False

        Dim sProductCode As String = Nothing
        Dim bIsTrueMonthlypolicy As Integer = 0
        Dim ScanMakeLiveTMP As String
        Dim oQuote As NexusProvider.Quote
        Private Sub FillPaymentMethods()

            Dim oPortalConfig As Config.Portal = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID())
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            oQuote = Session(CNQuote)

            If oQuote IsNot Nothing Then
                bIsTrueMonthlypolicy = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsTrueMonthlyPolicy, NexusProvider.RiskTypeOptions.Code, oQuote.ProductCode, "")
                If (bIsTrueMonthlypolicy) Then
                    ScanMakeLiveTMP = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.DefaultPaymentMethod, NexusProvider.RiskTypeOptions.Code, oQuote.ProductCode, "")
                End If

                SetPaymentAccessPermissions()
            End If

            Dim oPaymentOptions As Config.PaymentTypes = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).PaymentTypes

            Dim nCnt As Int32 = 0
            Dim bAvailable As Boolean = False 'for RequiredPaymentMethodForMTA option

            For Each oPaymentType As Config.PaymentType In oPaymentOptions
                'To Check the Availability of RequiredPaymentMethodForMTA option with Payment Method
                If Not oPaymentType.RequiredPaymentMethodForMTA Is Nothing Then
                    If String.IsNullOrEmpty(oPaymentType.RequiredPaymentMethodForMTA.Trim()) = False Then
                        bAvailable = True
                        Exit For
                    End If
                End If
            Next
            Dim oPaymentMethods As New NexusProvider.PaymentMethodCollection
            For Each oPaymentType As Config.PaymentType In oPaymentOptions
                Dim oPaymentMethod As NexusProvider.PaymentMethod
                Dim sPaymentType As String = oPaymentType.Type.ToString

                If Session(CNLoginType) = LoginType.Customer And oPaymentType.Name <> PaymentTypes.PayNow.ToString() _
                And oPaymentType.Name <> PaymentTypes.BankGuarantee.ToString() And
                oPaymentType.Name <> PaymentTypes.Invoice.ToString() And
                oPaymentType.Name.Trim.ToUpper <> "DIRECT DEBIT" Then

                    oPaymentMethod = New NexusProvider.PaymentMethod
                    oPaymentMethod.PaymentType = oPaymentType.Type
                    oPaymentMethod.PaymentName = oPaymentType.Name
                    oPaymentMethod.DisplayName = oPaymentType.DisplayName
                    oPaymentMethod.MTADisplayName = oPaymentType.MTADisplayName
                    oPaymentMethod.RedirectURL = oPaymentType.Url
                    nCnt = nCnt + 1
                    oPaymentMethods.Add(oPaymentMethod)
                ElseIf Session(CNLoginType) <> LoginType.Customer Then
                    If GetPaymentAccess(oPaymentType) Then
                        If Session.Item(CNQuoteMode) = QuoteMode.MTAQuote Then 'MTA
                            If Not oPaymentType.RequiredPaymentMethodForMTA Is Nothing Then
                                If String.IsNullOrEmpty(oPaymentType.RequiredPaymentMethodForMTA.Trim()) = False Then
                                    If oQuote.PaymentMethodCode.Trim().ToUpper() = oPaymentType.RequiredPaymentMethodForMTA.Trim().ToUpper() And oQuote.PaymentMethodCode.Trim().ToUpper() = oPaymentType.Name.Trim().ToUpper() Then
                                        'if RequiredPaymentMethodForMTA is set and matches with original policy payment method then  
                                        'show the matched payment option
                                        oPaymentMethod = New NexusProvider.PaymentMethod
                                        oPaymentMethod.PaymentType = oPaymentType.Type
                                        oPaymentMethod.PaymentName = oPaymentType.Name
                                        oPaymentMethod.DisplayName = oPaymentType.DisplayName
                                        oPaymentMethod.MTADisplayName = oPaymentType.MTADisplayName
                                        oPaymentMethod.RedirectURL = oPaymentType.Url

                                        oPaymentMethods.Add(oPaymentMethod)
                                    End If
                                End If
                            ElseIf bAvailable = False Then
                                'if RequiredPaymentMethodForMTA is not set then show all payment option
                                oPaymentMethod = New NexusProvider.PaymentMethod
                                oPaymentMethod.PaymentType = oPaymentType.Type
                                oPaymentMethod.PaymentName = oPaymentType.Name
                                oPaymentMethod.DisplayName = oPaymentType.DisplayName
                                oPaymentMethod.MTADisplayName = oPaymentType.MTADisplayName
                                oPaymentMethod.RedirectURL = oPaymentType.Url

                                oPaymentMethods.Add(oPaymentMethod)
                            End If
                        Else 'New Bussiness, need to show all the Payment Options from conig
                            oPaymentMethod = New NexusProvider.PaymentMethod
                            oPaymentMethod.PaymentType = oPaymentType.Type
                            oPaymentMethod.PaymentName = oPaymentType.Name
                            oPaymentMethod.DisplayName = oPaymentType.DisplayName
                            oPaymentMethod.MTADisplayName = oPaymentType.MTADisplayName
                            oPaymentMethod.RedirectURL = oPaymentType.Url

                            oPaymentMethods.Add(oPaymentMethod)
                        End If
                        nCnt = nCnt + 1
                    End If
                End If
            Next
            If oPaymentMethods.Count > 0 Then
                POLICYHEADER__PAYMENTMETHOD.Items.Clear()
                POLICYHEADER__PAYMENTMETHOD.SelectedValue = Nothing
                POLICYHEADER__PAYMENTMETHOD.DataSource = oPaymentMethods
                POLICYHEADER__PAYMENTMETHOD.DataValueField = "PaymentType"
                POLICYHEADER__PAYMENTMETHOD.DataTextField = "PaymentName"
                POLICYHEADER__PAYMENTMETHOD.DataBind()
            End If
        End Sub

        Private Function GetPaymentAccess(ByVal oPaymentType As Config.PaymentType) As Boolean
            Dim sPaymentType As String = oPaymentType.Type.ToString
            Dim bReturnvalue As Boolean = False

            If oQuote IsNot Nothing And Request.QueryString("quotecollection") <> "true" Then
                If (POLICYHEADER__BUSINESSTYPE.Value = "DIRECT") Then
                    'WHEN PRODUCT HAVE TRUE MONTHLY POLICY 
                    If (bIsTrueMonthlypolicy) Then
                        If ((sPaymentType = PaymentTypes.Invoice.ToString()) And bUserAgentCollection And bProductAgentCollection) Then
                            bReturnvalue = True
                        ElseIf ((oPaymentType.Name.Trim.ToUpper = "DIRECT DEBIT") And bUserDirectDebit And bProductDirectDebit) Then
                            bReturnvalue = True
                        End If
                    Else
                        If (sPaymentType = PaymentTypes.Invoice.ToString()) And bUserAgentCollection And bProductAgentCollection Then
                            bReturnvalue = True
                        ElseIf (sPaymentType = PaymentTypes.PayNow.ToString()) And bUserPayNow And bProductPayNow Then
                            bReturnvalue = True
                        ElseIf (sPaymentType = PaymentTypes.BankGuarantee.ToString()) And bUserBankGuarantee And bProductBankGuarantee Then
                            bReturnvalue = True
                        ElseIf (sPaymentType = PaymentTypes.CreditCard.ToString()) Then
                            bReturnvalue = True
                        ElseIf (sPaymentType = PaymentTypes.CashDeposit.ToString()) And bUserCashDeposit And bProductCashDeposit Then
                            bReturnvalue = True
                        ElseIf (oPaymentType.Name.Trim.ToUpper = "DIRECT DEBIT") And bUserDirectDebit And bProductDirectDebit Then
                            bReturnvalue = True
                        End If
                    End If


                Else
                    'WHEN PRODUCT HAVE TRUE MONTHLY POLICY 
                    If (bIsTrueMonthlypolicy) Then
                        If ((sPaymentType = PaymentTypes.Invoice.ToString()) And bUserAgentCollection And bProductAgentCollection And bAgentAgentCollection) Then
                            bReturnvalue = True
                        ElseIf ((oPaymentType.Name.Trim.ToUpper = "DIRECT DEBIT") And bUserDirectDebit And bProductDirectDebit And bAgentDirectDebit) Then
                            bReturnvalue = True
                        End If
                    Else
                        If (sPaymentType = PaymentTypes.Invoice.ToString()) And bUserAgentCollection And bProductAgentCollection And bAgentAgentCollection Then
                            bReturnvalue = True
                        ElseIf (sPaymentType = PaymentTypes.PayNow.ToString()) And bUserPayNow And bProductPayNow And bAgentPayNow Then
                            bReturnvalue = True
                        ElseIf (sPaymentType = PaymentTypes.BankGuarantee.ToString()) And bUserBankGuarantee And bProductBankGuarantee And bAgentBankGuarantee Then
                            bReturnvalue = True
                        ElseIf (sPaymentType = PaymentTypes.CreditCard.ToString()) Then
                            bReturnvalue = True
                        ElseIf (sPaymentType = PaymentTypes.CashDeposit.ToString()) And bUserCashDeposit And bProductCashDeposit And bAgentCashDeposit Then
                            bReturnvalue = True
                        ElseIf (oPaymentType.Name.Trim.ToUpper = "DIRECT DEBIT") And bUserDirectDebit And bProductDirectDebit And bAgentDirectDebit Then
                            bReturnvalue = True
                        End If
                    End If

                End If
            Else
                If Request.QueryString("quotecollection") = True Then
                    'we're in quote collection but this option is not enabled for quote collection 
                    'so show option according to whether or not it is enabled for quote collection
                    If oPaymentType.UseForQuoteCollection Then
                        bReturnvalue = True
                    Else
                        bReturnvalue = False
                    End If
                End If
            End If

            Return bReturnvalue
        End Function

        ''' <summary>
        ''' Setting the payment options for Agent, Product and User 
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub SetPaymentAccessPermissions()

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oUserAuthority As New NexusProvider.UserAuthority
            Dim oAgentSetting As NexusProvider.AgentSettings
            Dim nAgentKey As Integer
            ' Obtaining and setting authority value for User AgentCollection/Invoice
            oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.CanMakeLiveInvoice
            oUserAuthority.UserCode = Session(CNLoginName) ' TO DO need to be made dynamic
            oWebService.GetUserAuthorityValue(oUserAuthority)
            If String.IsNullOrEmpty(oUserAuthority.UserAuthorityValue) = False AndAlso oUserAuthority.UserAuthorityValue.Trim = "1" Then
                bUserAgentCollection = True
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
                    bProductAgentCollection = True
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

            Integer.TryParse(POLICYHEADER__AGENT.Value, nAgentKey)

            If nAgentKey > 0 Then
                'Call agent setting for current agent
                GetAgentSettingsCall(oAgentSetting, nAgentKey)
                If oAgentSetting IsNot Nothing Then
                    ' Setting and setting authority value for Agent AgentCollection
                    bAgentAgentCollection = oAgentSetting.CanMakeLiveInvoice
                    ' Setting and setting authority value for Agent PayNow
                    bAgentPayNow = oAgentSetting.CanMakeLivePaynow
                    ' Setting and setting authority value for Agent BankGuarantee
                    bAgentBankGuarantee = oAgentSetting.CanMakeLiveBankGuarantee
                    ' Setting and setting authority value for Agent CashDeposit
                    bAgentCashDeposit = oAgentSetting.CanMakeLiveCashDeposit
                    ' Setting and setting authority value for Agent Direct Debit
                    bAgentDirectDebit = oAgentSetting.CanMakeLiveInstalments
                End If
            End If

        End Sub

#End Region

        ''' <summary>
        ''' POLICYHEADER__PAYMENTMETHOD Selected Index Changed
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub POLICYHEADER__PAYMENTMETHOD_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles POLICYHEADER__PAYMENTMETHOD.SelectedIndexChanged

            If ViewState("DebitOrderOptionType") Is Nothing Then
                Dim oDebitOrderOptionType As New NexusProvider.OptionTypeSetting
                oWebService = New NexusProvider.ProviderManager().Provider
                oDebitOrderOptionType = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, 105)
                ViewState.Add("DebitOrderOptionType", oDebitOrderOptionType.OptionValue)
            End If

            If ViewState("DebitOrderOptionType") = "1" And POLICYHEADER__PAYMENTMETHOD.SelectedValue = "Invoice" Then
                rfvCollectionFrequency.Enabled = True
                POLICYHEADER__COLLECTIONFREQUENCY.CssClass = "form-control field-mandatory"

                rfvPaymentTerm.Enabled = True
                POLICYHEADER__PAYMENTTERM.CssClass = "form-control field-mandatory"
            Else
                rfvCollectionFrequency.Enabled = False
                POLICYHEADER__COLLECTIONFREQUENCY.CssClass = "form-control"

                rfvPaymentTerm.Enabled = False
                POLICYHEADER__PAYMENTTERM.CssClass = "form-control"
            End If

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            Dim sScriptBuilder As New StringBuilder
            sScriptBuilder.Append("function GetDateAndRedirect(){")
            sScriptBuilder.Append("var dtCoverStartDate = document.getElementById('" & POLICYHEADER__COVERSTARTDATE.UniqueID.Replace("$", "_") & "').value;")
            If HttpContext.Current.Session.IsCookieless Then
                sScriptBuilder.Append("tb_show(null , ' " & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/FindAgent.aspx?FromPage=MainDetails&COVERSTARTDATE=' + dtCoverStartDate + '&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;")
                btnHandler.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/FindAccountHandler.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            Else
                sScriptBuilder.Append("tb_show(null , ' " & AppSettings("WebRoot") & "Modal/FindAgent.aspx?FromPage=MainDetails&COVERSTARTDATE=' + dtCoverStartDate + '&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;")
                btnHandler.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "Modal/FindAccountHandler.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            End If

            sScriptBuilder.Append("}")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "GetDateAndRedirect",
            "<script language=""javascript"" type=""text/javascript"">" & sScriptBuilder.ToString() & "</script>")
            btnAgentCode.OnClientClick = "return GetDateAndRedirect();"
        End Sub

        Protected Sub FillContactPerson(ByVal iAgentKey As Integer)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oAgentSettings As NexusProvider.AgentSettings = Nothing
            Dim oUserCollection As New NexusProvider.UserCollection
            Dim oUserCollectionWithCorrectedUserName As NexusProvider.UserCollection

            GetAgentSettingsCall(oAgentSettings, iAgentKey)
            If (oAgentSettings IsNot Nothing AndAlso oAgentSettings.AssociatedUsers IsNot Nothing) Then
                oUserCollection = oAgentSettings.AssociatedUsers
            End If

            oUserCollectionWithCorrectedUserName = New NexusProvider.UserCollection
            Dim oCorrectedUser As NexusProvider.User
            For Each oUser As NexusProvider.User In oUserCollection
                oCorrectedUser = New NexusProvider.User
                oCorrectedUser = oUser
                oCorrectedUser.FullName = IIf(oUser.FullName.ToString = "", oUser.UserName.ToString(), oUser.FullName.ToString)
                oUserCollectionWithCorrectedUserName.Add(oCorrectedUser)
            Next
            oUserCollectionWithCorrectedUserName.SortColumn = "FullName"
            oUserCollectionWithCorrectedUserName.SortingOrder = NexusProvider.GenericComparer.SortOrder.Ascending
            oUserCollectionWithCorrectedUserName.Sort()
            POLICYHEADER__CONTACT_NAME.DataValueField = "UserKey"
            POLICYHEADER__CONTACT_NAME.DataTextField = "FullName"
            POLICYHEADER__CONTACT_NAME.DataSource = oUserCollectionWithCorrectedUserName
            POLICYHEADER__CONTACT_NAME.DataBind()
            POLICYHEADER__CONTACT_NAME.Enabled = True

            If (oUserCollection.Count = 0) Then
                POLICYHEADER__CONTACT_NAME.Items.Clear()
                POLICYHEADER__CONTACT_NAME.Enabled = False
            End If
            POLICYHEADER__CONTACT_NAME.Items.Insert(0, New ListItem(GetLocalResourceObject("lblDefaultText"), ""))
        End Sub


        ''' <summary>
        ''' AgencyCancelled
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub AgencyCancelled()
            Dim oAgentSettings As NexusProvider.AgentSettings = Nothing
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim hdnAgent As HiddenField = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__AGENT"), HiddenField)
            Dim txtCoverstrDate As TextBox = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("POLICYHEADER__COVERSTARTDATE"), TextBox)
            Dim sInsuranceFileTypeCode As String = ""

            If hdnAgent Is Nothing Then
                Exit Sub
            End If

            If String.IsNullOrEmpty(hdnAgent.Value) Then
                If oQuote.Agent Is Nothing OrElse CInt(oQuote.Agent) = 0 Then
                    Exit Sub
                End If
                hdnAgent.Value = Trim(oQuote.Agent)
            End If
            If hdnAgent.Value Is Nothing OrElse CInt(hdnAgent.Value) = 0 Then
                Exit Sub
            End If
            If oQuote.InsuranceFileTypeCode IsNot Nothing Then
                sInsuranceFileTypeCode = UCase(oQuote.InsuranceFileTypeCode.Trim())
            End If
            GetAgentSettingsCall(oAgentSettings, hdnAgent.Value)

            If oAgentSettings IsNot Nothing Then
                Dim dtAgencyCancellationDate As DateTime
                Dim dtDateToValidateForAgencyCancellation As DateTime
                Dim oValidateCancelledAgentOrBroker As New NexusProvider.OptionTypeSetting
                oValidateCancelledAgentOrBroker = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 1040)

                If Not oValidateCancelledAgentOrBroker.OptionValue Is Nothing Then
                    If oValidateCancelledAgentOrBroker.OptionValue = "1" Then 'CoverStartDate
                        If Not txtCoverstrDate Is Nothing AndAlso Not String.IsNullOrEmpty(txtCoverstrDate.Text) Then
                            dtDateToValidateForAgencyCancellation = CDate(txtCoverstrDate.Text)
                        Else
                            dtDateToValidateForAgencyCancellation = oQuote.CoverStartDate
                        End If
                    ElseIf oValidateCancelledAgentOrBroker.OptionValue = "0" Then 'TransactionDate
                        dtDateToValidateForAgencyCancellation = DateTime.Now
                    End If
                Else
                    dtDateToValidateForAgencyCancellation = DateTime.Now
                End If
                Dim sScriptBuilder As New StringBuilder

                sScriptBuilder.Append("function AgencyCancellation(){")
                If oAgentSettings.AgencyCancellationDate <> DateTime.MinValue AndAlso oAgentSettings.AgencyCancellationDate.ToString("MM/dd/yyyy") <> "12/29/1899" Then
                    dtAgencyCancellationDate = oAgentSettings.AgencyCancellationDate

                    sScriptBuilder.Append("var modifiedAgent = document.getElementById('" & hdnAgent.UniqueID.Replace("$", "_") & "').value;")
                    If oValidateCancelledAgentOrBroker.OptionValue = "0" Then
                        sScriptBuilder.Append("var modifiedcovertdate = ('" & dtDateToValidateForAgencyCancellation.ToString("dd/MM/yyyy") & "');")
                    Else
                        sScriptBuilder.Append("var modifiedcovertdate = (document.getElementById('" & txtCoverstrDate.UniqueID.Replace("$", "_") & "').value);")
                    End If
                    sScriptBuilder.Append("var agentcancelleddate = ('" & dtAgencyCancellationDate.ToString("dd/MM/yyyy") & "');")
                    sScriptBuilder.Append("var businesstype = (document.getElementById('" & POLICYHEADER__BUSINESSTYPE.UniqueID.Replace("$", "_") & "').value);")

                    sScriptBuilder.Append("var datecompare=fn_DateDiff(agentcancelleddate,modifiedcovertdate);")
                    Select Case sInsuranceFileTypeCode
                        Case Nothing, "QUOTE", "RENEWAL"
                            sScriptBuilder.Append("if ((datecompare>=0) && (businesstype=='AGENCY')) {")
                            sScriptBuilder.Append("alert('" & GetLocalResourceObject("msgAgencyCancelledQuote") & "');return false;}")
                        Case Else
                            sScriptBuilder.Append("if ((datecompare>=0) && (businesstype=='AGENCY')) {")
                            sScriptBuilder.Append("return confirm('" & GetLocalResourceObject("msgAgencyCancelled") & "');}")
                    End Select
                    btnNext.OnClientClick = "return AgencyCancellation();"
                Else
                    sScriptBuilder.Append("return true;")
                End If
                sScriptBuilder.Append("}")
                Session(CNAgentCancelled) = True
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "AgencyCancellation",
"<script language=""javascript"" type=""text/javascript"">" & sScriptBuilder.ToString() & "</script>")
            End If
        End Sub
        Private Sub FillCoverNoteSheets(Optional ByVal nSheetNumber As Integer = 0)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

            'if selection is valid then only Not Issue status sheet should be populated
            If POLICYHEADER__COVERNOTEBOOKNO.SelectedIndex > 0 Then
                Dim oCoverNote As New NexusProvider.CoverNote
                Dim oCoverNoteSheetColl As New NexusProvider.CoverNoteSheetTypeCollection
                Dim iCounter As Integer = 0

                oCoverNote.BookNumber = POLICYHEADER__COVERNOTEBOOKNO.SelectedItem.Text
                oCoverNote.CoverNoteBookKey = POLICYHEADER__COVERNOTEBOOKNO.SelectedValue
                oCoverNote.CoverNoteStatusCode = "NOTISS"

                oWebService.GetCoverNoteBook(oCoverNote)
                For iCounter = 0 To oCoverNote.CoverNoteSheets.Count - 1
                    If oCoverNote.CoverNoteSheets(iCounter).CoverNoteSheetStatusCode.Trim = "NOTISS" OrElse (nSheetNumber <> 0 AndAlso oCoverNote.CoverNoteSheets(iCounter).CoverNoteSheetNumber = nSheetNumber) Then
                        oCoverNoteSheetColl.Add(oCoverNote.CoverNoteSheets(iCounter))
                    End If
                Next

                POLICYHEADER__COVERNOTESHEETNO.DataSource = oCoverNoteSheetColl
                POLICYHEADER__COVERNOTESHEETNO.DataTextField = "CoverNoteSheetNumber"
                POLICYHEADER__COVERNOTESHEETNO.DataValueField = "CoverNoteSheetNumber"
                POLICYHEADER__COVERNOTESHEETNO.DataBind()
                POLICYHEADER__COVERNOTESHEETNO.Items.Insert(0, New ListItem("(Please Select)", ""))
                POLICYHEADER__COVERNOTESHEETNO.SelectedIndex = 0
            Else
                POLICYHEADER__COVERNOTESHEETNO.Items.Clear()
                POLICYHEADER__COVERNOTESHEETNO.Items.Insert(0, New ListItem("(Please Select)", ""))
                POLICYHEADER__COVERNOTESHEETNO.SelectedIndex = 0
            End If
        End Sub
        Protected Sub POLICYHEADER__COVERNOTEBOOKNO_SelectedIndexChanged(sender As Object, e As EventArgs) Handles POLICYHEADER__COVERNOTEBOOKNO.SelectedIndexChanged

            FillCoverNoteSheets()
        End Sub

        Protected Sub POLICYHEADER__CORRESPONDENCETYPE_SelectedIndexChange(sender As Object, e As EventArgs) Handles POLICYHEADER__CORRESPONDENCETYPE.SelectedIndexChange
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oAgentSettings As NexusProvider.AgentSettings = Nothing
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)

            If POLICYHEADER__RECEIVESCLIENTCORRESPONDENCE IsNot Nothing Then
                POLICYHEADER__RECEIVESCLIENTCORRESPONDENCE.Value = 0
            End If

            If POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE IsNot Nothing AndAlso POLICYHEADER__CORRESPONDENCETYPE IsNot Nothing AndAlso POLICYHEADER__CORRESPONDENCETYPE.Value IsNot Nothing Then
                If POLICYHEADER__BUSINESSTYPE.Value.Trim.ToUpper = "DIRECT" OrElse POLICYHEADER__BUSINESSTYPE.Value.Trim.ToUpper = "" Then
                    lblPOLICYHEADER_CORRESPONDENCETYPE.Text = "Client Correspondence"
                    If POLICYHEADER__CORRESPONDENCETYPE.Value.Trim.ToUpper = "DEFAULT" Then
                        POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Visible = True
                        POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Text = GetClientDefaultPreferredCorrespondence()
                    Else
                        POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Visible = False
                        POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Text = String.Empty
                    End If
                Else
                    If Not String.IsNullOrEmpty(POLICYHEADER__AGENT.Value.ToString) OrElse POLICYHEADER__AGENT.Value.Trim <> "" Then
                        If POLICYHEADER__AGENT.Value.Trim.Length <> 0 AndAlso POLICYHEADER__AGENT.Value.Trim <> "0" Then
                            oAgentSettings = oWebService.GetAgentSettings(POLICYHEADER__AGENT.Value)
                        End If
                        If oAgentSettings IsNot Nothing Then
                            If oAgentSettings.IsReceiveClientCorrespondence Then
                                lblPOLICYHEADER_CORRESPONDENCETYPE.Text = "Agent Correspondence"
                                POLICYHEADER__RECEIVESCLIENTCORRESPONDENCE.Value = 1
                                If POLICYHEADER__CORRESPONDENCETYPE.Value.Trim.ToUpper = "DEFAULT" Then
                                    POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Visible = True
                                    POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Text = oAgentSettings.CorrespondenceType.Trim.ToUpper
                                Else
                                    POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Visible = False
                                    POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Text = String.Empty
                                End If
                            Else
                                lblPOLICYHEADER_CORRESPONDENCETYPE.Text = "Client Correspondence"
                                If POLICYHEADER__CORRESPONDENCETYPE.Value.Trim.ToUpper = "DEFAULT" Then
                                    POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Visible = True
                                    POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Text = GetClientDefaultPreferredCorrespondence()
                                Else
                                    POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Visible = False
                                    POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Text = String.Empty
                                End If
                            End If
                        End If
                    Else
                        lblPOLICYHEADER_CORRESPONDENCETYPE.Text = "Client Correspondence"
                        If POLICYHEADER__CORRESPONDENCETYPE.Value.Trim.ToUpper = "DEFAULT" Then
                            POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Visible = True
                            POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Text = GetClientDefaultPreferredCorrespondence()
                        Else
                            POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Visible = False
                            POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Text = String.Empty
                        End If
                    End If
                End If
                If Not String.IsNullOrEmpty(POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Text) Then
                    POLICYHEADER__DEFAULTCORRESPONDENCECODE.Value = POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Text
                    POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Text = GetListItemDescriptionfromCode("PMLookUp", "Contact_Type", POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Text)
                End If
                Dim sScript As String = "policyBusinessType();"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "thickbox", sScript, True)
            End If
        End Sub

        Private Function GetClientDefaultPreferredCorrespondence() As String
            Dim oParty As NexusProvider.BaseParty
            Dim sDefaultPreferredCorrespondence As String = String.Empty
            If Session(CNParty) IsNot Nothing Then
                Select Case True
                    Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                        oParty = CType(Session(CNParty), NexusProvider.CorporateParty)
                        With CType(oParty, NexusProvider.CorporateParty)
                            sDefaultPreferredCorrespondence = .ClientSharedData.CorrespondenceCode
                        End With
                    Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                        oParty = CType(Session(CNParty), NexusProvider.PersonalParty)
                        With CType(oParty, NexusProvider.PersonalParty)
                            sDefaultPreferredCorrespondence = .ClientSharedData.CorrespondenceCode
                        End With
                End Select
            End If
            Return sDefaultPreferredCorrespondence
        End Function

        Protected Function GetListItemDescriptionfromCode(ByVal sListType As String, ByVal sListCode As String, ByVal sItemCode As String) As String
            Dim sItemDescription As String = String.Empty

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oList As New NexusProvider.LookupListCollection

            ' sam call to retreive the list of items from user defined list
            If sListType = "UserDefined" Then
                oList = oWebService.GetList(NexusProvider.ListType.UserDefined, sListCode, False, False)
            Else
                oList = oWebService.GetList(NexusProvider.ListType.PMLookup, sListCode, False, False)
            End If

            ' Get code for ID
            For iListCount As Integer = 0 To oList.Count - 1
                If oList(iListCount).Code = sItemCode Then
                    sItemDescription = oList(iListCount).Description
                    Exit For
                End If
            Next
            Return sItemDescription
        End Function

        Private Sub UnlockQuote()
            'Lock the claim 
            Dim oExclusiveLocking As NexusProvider.OptionTypeSetting
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            oWebService = New NexusProvider.ProviderManager().Provider
            oExclusiveLocking = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ExclusiveLock)
            If oExclusiveLocking.OptionValue = "1" Then
                UnlockPolicy(nInsuranceFolderCnt:=oQuote.InsuranceFolderKey, sBranchCode:=Session(CNTransBranchCode).ToString)
            End If
            oWebService = Nothing
        End Sub


        ''' <summary>
        ''' Save Policy level Standard Wordings
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <remarks></remarks>
        Private Sub SavePolicyStandardWordings(ByVal sender As Object)
            Dim oSWContorl As Control = sender.Parent
            If oSWContorl Is Nothing Then
                Exit Sub
            End If

            For Each oControl In oSWContorl.Controls
                If oControl.GetType.Name.Contains("controls_standardwordings_ascx") Then
                    oControl.SubmitSelections(True)
                    oControl = Nothing
                    Exit For
                End If
            Next

            oSWContorl = Nothing
        End Sub
    End Class
End Namespace
