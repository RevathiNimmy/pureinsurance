Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports CMS.Library
Imports System.Xml
Imports Nexus
Imports System.Data
Imports System
Imports System.Globalization
Imports System.Threading
Imports Nexus.Utils
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class secure_mtareason : Inherits Frontend.clsCMSPage
        Const ClientMode As String = "CLIENT_MODE"
        Dim PolicyStartDate, FD_MTADate As Date
        Dim oQuote As NexusProvider.Quote
        Dim oWebService As NexusProvider.ProviderBase

        'Protected variable declared, so that they can be used in java script written in .aspx file
        Protected dt_lastRenewalDate As Date
        Protected dt_secondLastRenewalDate As Date
        Protected dt_coverStartDate As Date
        Protected dt_PolicyStartDate As Date
        Protected dt_PolicyExpiryDate As Date
        Protected dt_MinMTADate As Date

        Protected sBackDatedMTAnMTCAuthority As String
        Protected sMsg_BackDatedMTAnMTC_NotAuthorised As String
        Protected sMsg_BackDatedMTAnMTC_WithNoClaim As String
        Protected sMsg_BackDatedConfirmation As String
        Protected sTempMTAMessage As String

        Protected bAllowBackDatedMTAForProduct As Boolean
        Protected bAllowBackDatedMTCForProduct As Boolean
        Protected sDateAllowedForOutOfSyncMTAForProduct As String 'Can be 1,2,3,4
        Protected sMsg_BackDatedMTAForProduct As String
        Protected sMsg_BackDatedMTCForProduct As String
        Protected sMsg_PolicyUnderRenewal As String
        Protected sMsg_BackDateBeforeCover As String
        Protected sMsg_DateAllowedForOutOfSyncMTAForProduct_NA As String
        Protected sMsg_DateAllowedForOutOfSyncMTAForProduct_CP As String 'Current period only
        Protected sMsg_DateAllowedForOutOfSyncMTAForProduct_CPPlusOne As String 'Current period + 1
        Protected iNoOfClaimsForPolicy As Integer
        Protected sTransType As String

        Protected sMTAReasonForCancelation As String = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).MTAReasonForCancellation
        Protected sPolicyStatus As String
        Protected sMsg_MTAonLapsedConfirmation As String
        Protected sConfirmPolicyUnderRenewal As String
        Protected dt_LastClaimDate As Date
        Protected sMsg_ConfirmClaimMessage, sMsg_ConfirmClaimMessageForBackdatedMTA As String
        Protected sConfirmPolicyUnderRenewalToDelete As String
        Protected bIsPolicyInAnnualRenewal, bIsPolicyInRenewal, bDoNotDeleteRenewalQuoteOnMTA, bDeleteRenQuoteReRunRenewal As Boolean
        Protected dt_LastCancelledDate As Date?
        Protected sMsg_ConfirmSaveOOSMTAQuoteMessage As String
        Protected bHasOOSPendingVersions As Boolean
        Protected bAskForRenewalConfirmation As Boolean
        Protected bMidTermAdjustmentTemp As Boolean
        Dim oXmlElement As System.Xml.XmlElement = Nothing
        Dim oListColl As NexusProvider.LookupListCollection = Nothing
        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Dim SelectedPaymentOption As String
            SelectedPaymentOption = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).MTAReasonForCancellation
            Me.ClientScript.RegisterStartupScript(GetType(String), "StartupScripts", "pageload('" & SelectedPaymentOption & "');", True)
            If Not IsPostBack Then
                If Not IsPostBack Then
                    'Do not default MTA date - system option
                    Dim oDoNotDefaultMTADateOptionSettings As NexusProvider.OptionTypeSetting
                    Try
                        oWebService = New NexusProvider.ProviderManager().Provider
                        oDoNotDefaultMTADateOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.DoNotDefaultMTADate)
                    Finally
                        oWebService = Nothing
                    End Try

                    If oDoNotDefaultMTADateOptionSettings IsNot Nothing AndAlso oDoNotDefaultMTADateOptionSettings.OptionValue = "1" Then
                        txtEffectiveDate.Text = ""
                        If Session(CNMTAType) IsNot Nothing AndAlso Session(CNMTAType) = MTAType.REINSTATEMENT Then
                            txtEffectiveDate.Text = Date.Now.ToShortDateString()
                        End If
                    Else
                        'Effective date of endorsement to default to current date.
                        txtEffectiveDate.Text = Date.Now.ToShortDateString()
                    End If
                    txtHiddenShortDateFormat.Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper()
                End If
            End If
        End Sub

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If UserCanDoTask("MidTermAdjustment") = False Then
                'Not CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).AllowMTA Then
                'if AllowMTA=false then blocking access to page so that user can't access it by typing the URL
                Response.Redirect("~/Secure/Agent/FindClient.aspx", False)
            End If

            If Session(CNTempOriginalMTAQuote) IsNot Nothing AndAlso Session(CNMTAType) <> MTAType.REINSTATEMENT Then
                Session(CNQuote) = Session(CNTempOriginalMTAQuote)
                Session.Remove(CNTempOriginalMTAQuote)
            End If
            If UserCanDoTask("MidTermAdjustmentTemp") = True Then
                bMidTermAdjustmentTemp = True
            End If

            oQuote = CType(Session(CNQuote), NexusProvider.Quote)
            dt_coverStartDate = oQuote.CoverStartDate
            dt_PolicyExpiryDate = oQuote.CoverEndDate

            Dim oPortalConfig As Config.Portal = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID())
            Dim oPolicy As NexusProvider.PolicyCollection
            Dim oOOSMTAVersions As NexusProvider.BaseInsuranceFileKeyCollection

            'If Not IsPostBack Then
            'We need to set all the required variables to check all the validations 
            'for backdated MTA for user and product.These variables will be use in javascript
            oWebService = New NexusProvider.ProviderManager().Provider
            oPolicy = oWebService.GetAllPolicyVersions(oQuote.InsuranceFolderKey)

            oOOSMTAVersions = oWebService.CheckPendingOOSVersions(oQuote.InsuranceFileKey, oQuote.InsuranceFolderKey)
            If oOOSMTAVersions IsNot Nothing AndAlso oOOSMTAVersions.Count > 0 Then
                bHasOOSPendingVersions = True
            End If

            Session("OOS_MTA_SAVED_VERSIONS") = oOOSMTAVersions


            'Checking the Backdated MTA/MTC Authority
            Dim oUserAuthority As New NexusProvider.UserAuthority
            oUserAuthority.UserCode = Session(CNLoginName)
            oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.BackDatedMtaAndMtcAuthorityType

            oWebService.GetUserAuthorityValue(oUserAuthority)
            sBackDatedMTAnMTCAuthority = oUserAuthority.UserAuthorityValue  'Possible values are 1,2 or 3

            If sBackDatedMTAnMTCAuthority = "2" Then 'Call if BackDatedMTAnMTCAuthority = 2 as thats when we use iNoOfClaimsForPolicy
                'Get Claim Details on a policy
                Dim oClaimSearchCriteria As New NexusProvider.ClaimSearchCriteria
                Dim oClaims As New NexusProvider.ClaimCollection

                oClaimSearchCriteria.ClientShortName = oQuote.ClientCode
                oClaimSearchCriteria.InsuranceFileRef = oQuote.InsuranceFileRef
                oClaimSearchCriteria.IncludeClosedClaim = True

                oClaims = oWebService.FindClaim(oClaimSearchCriteria)
                iNoOfClaimsForPolicy = oClaims.Count 'Get number of claims for a policy

                For i = 0 To oClaims.Count - 1
                    If dt_LastClaimDate < (oClaims.Item(i).LossDateFrom) Then
                        dt_LastClaimDate = (oClaims.Item(i).LossDateFrom)
                    End If
                Next

            End If

            'Set all the messages in protected valiables from resource file.So that they can be used in javascript 
            sMsg_BackDatedMTAnMTC_NotAuthorised = GetLocalResourceObject("lbl_BanckDatedMTAnMTC_NotAuthorised")
            sMsg_BackDatedMTAnMTC_WithNoClaim = GetLocalResourceObject("lbl_BanckDatedMTAnMTC_WithNoClaim")
            sMsg_BackDatedConfirmation = GetLocalResourceObject("lbl_BackDatedConfirmation")
            sMsg_BackDatedMTAForProduct = GetLocalResourceObject("lbl_BackDatedMTAForProduct")
            sMsg_BackDatedMTCForProduct = GetLocalResourceObject("lbl_BackDatedMTCForProduct")
            sMsg_DateAllowedForOutOfSyncMTAForProduct_NA = GetLocalResourceObject("lbl_DateAllowedForOutOfSyncMTAForProduct_NA") 'Not allowed
            sMsg_DateAllowedForOutOfSyncMTAForProduct_CP = GetLocalResourceObject("lbl_DateAllowedForOutOfSyncMTAForProduct_CP") 'Current period only
            sMsg_DateAllowedForOutOfSyncMTAForProduct_CPPlusOne = GetLocalResourceObject("lbl_DateAllowedForOutOfSyncMTAForProduct_CPPlusOne") 'Current period + 1
            sTempMTAMessage = GetLocalResourceObject("lbl_WarningMsg") 'Temp MTA Confirmation message
            sConfirmPolicyUnderRenewal = GetLocalResourceObject("msg_ConfirmPolicyUnderRenewal")
            sConfirmPolicyUnderRenewalToDelete = GetLocalResourceObject("msg_ConfirmPolicyUnderRenewalToDelete")
            sMsg_ConfirmClaimMessageForBackdatedMTA = GetLocalResourceObject("msg_ConfirmClaimMessageForPolicy")
            sMsg_ConfirmClaimMessage = GetLocalResourceObject("msg_ConfirmClaimMessage")
            sMsg_ConfirmSaveOOSMTAQuoteMessage = GetLocalResourceObject("msg_ConfirmSaveOOSMTAQuoteMessage")
            bIsPolicyInAnnualRenewal = oQuote.IsPolicyInAnnualRenewal
            bIsPolicyInRenewal = oQuote.IsPolicyInRenewal
            bDoNotDeleteRenewalQuoteOnMTA = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.DoNotDeleteRenQuoteOnMTA, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
            bDeleteRenQuoteReRunRenewal = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.DeleteRenQuoteReRunRenewal, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
            sMsg_BackDateBeforeCover = GetLocalResourceObject("lbl_DateBeforeCover")
            sConfirmPolicyUnderRenewal = GetLocalResourceObject("msg_ConfirmPolicyUnderRenewal")
            sMsg_ConfirmClaimMessage = GetLocalResourceObject("msg_ConfirmClaimMessage")
            sPolicyStatus = Trim(oQuote.InsuranceFileStatusCode)
            sMsg_MTAonLapsedConfirmation = GetLocalResourceObject("lbl_MtaOnLapsedPolicy")
            'Get All user authority and product lable settings for back dated MTA/MTC and out of sync MTA and set variables
            bAllowBackDatedMTAForProduct = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.AllowBackdatedMTAs, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)

            If oQuote.InsuranceFileStatusCode.Trim().ToUpper() = "REN" Then
                bAskForRenewalConfirmation = True
            Else
                bAskForRenewalConfirmation = False
            End If

            'if backdated mta is allowed then MTC automatically should be allowed
            If bAllowBackDatedMTAForProduct = True Then
                bAllowBackDatedMTCForProduct = True
            Else
                'get actual vlaue for backdated MTC option
                bAllowBackDatedMTCForProduct = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.AllowBackdatedMTCs, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
            End If

            sDateAllowedForOutOfSyncMTAForProduct = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.OutOfSequenceMTADates, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)

            'Identify current and current-1 live renewal version's period
            Dim iCount As Integer = 0
            Dim iTempVar As Integer = 0
            For iTempVar = oPolicy.Count - 1 To 0 Step -1
                If oPolicy.Item(iTempVar).InsuranceFileTypeCode.Trim = "POLICY" AndAlso oPolicy.Item(iTempVar).PolicyStatusCode.Trim <> "REPBDMTA" Then
                    iCount = iCount + 1
                    If iCount = 1 Then
                        dt_lastRenewalDate = oPolicy.Item(iTempVar).CoverStartDate
                    ElseIf iCount = 2 Then
                        dt_secondLastRenewalDate = oPolicy.Item(iTempVar).CoverStartDate
                    End If
                ElseIf oPolicy.Item(iTempVar).InsuranceFileTypeCode.Trim = "RENEWAL" Then ' Find insurancefilecnt for renewal version
                    sMsg_PolicyUnderRenewal = GetLocalResourceObject("lbl_vldMsg_PolicyUnderRenewal")
                    Dim oRenewalStatus As NexusProvider.RenewalStatus
                    ViewState.Add("RenewalVersionInsuranceFileCnt", oPolicy.Item(iTempVar).InsuranceFileKey)
                    oWebService = New NexusProvider.ProviderManager().Provider
                    If Not IsPostBack Then
                        oRenewalStatus = oWebService.GetRenewalStatus(CType(ViewState("RenewalVersionInsuranceFileCnt"), Integer))
                        If Not oRenewalStatus Is Nothing AndAlso Trim(oRenewalStatus.RenewalStatusTypeDescription) <> "" Then
                            Hdnrenwalflag.Value = 1
                        End If
                    End If
                End If
            Next

            'Identify policy start date.
            For iTempVar = 0 To oPolicy.Count - 1
                If oPolicy.Item(iTempVar).InsuranceFileTypeCode.Trim = "POLICY" Then
                    dt_PolicyStartDate = oPolicy.Item(iTempVar).CoverStartDate
                    Exit For
                End If
            Next

            Dim sInvalidEffectiveDate As String = GetLocalResourceObject("lbl_InvalidEffectiveDate")
            sInvalidEffectiveDate = sInvalidEffectiveDate.Replace("#StartDate", dt_PolicyStartDate)
            sInvalidEffectiveDate = sInvalidEffectiveDate.Replace("#EndDate", dt_PolicyExpiryDate)

            'Set validation message, min date and max date for range validator as per product 
            'option selected in BO for out of sync MTA
            Select Case sBackDatedMTAnMTCAuthority
                Case "1" 'Not authorised
                    If CType(Session.Item(CNLoginType), LoginType) = LoginType.Agent And UserCanDoTask("BackDatedMTA") = True Then
                        rangevldDateFrom.MinimumValue = oQuote.CoverStartDate
                    Else
                        rangevldDateFrom.MinimumValue = Date.Today.Date
                    End If
                    rangevldDateFrom.ErrorMessage = sMsg_BackDatedMTAnMTC_NotAuthorised
                Case "2", "3" '2- With No Claims and 3- With Claims
                    If iNoOfClaimsForPolicy > 0 And sBackDatedMTAnMTCAuthority = "2" Then
                        If CType(Session.Item(CNLoginType), LoginType) = LoginType.Agent And UserCanDoTask("BackDatedMTA") = True Then
                            rangevldDateFrom.MinimumValue = oQuote.CoverStartDate.Date.ToShortDateString()
                        Else
                            rangevldDateFrom.MinimumValue = Date.Today.Date.ToShortDateString()
                        End If
                        rangevldDateFrom.ErrorMessage = sMsg_BackDatedMTAnMTC_WithNoClaim
                    Else
                        'Same as user authority with Claims
                        If sDateAllowedForOutOfSyncMTAForProduct = "0" Then ' Not Allowed
                            If CType(Session.Item(CNLoginType), LoginType) = LoginType.Agent And UserCanDoTask("BackDatedMTA") = True Then
                                rangevldDateFrom.MinimumValue = oQuote.CoverStartDate
                            Else
                                rangevldDateFrom.MinimumValue = Date.Today.Date
                            End If
                            rangevldDateFrom.ErrorMessage = sMsg_DateAllowedForOutOfSyncMTAForProduct_NA
                        ElseIf sDateAllowedForOutOfSyncMTAForProduct = "1" Then ' Current period only
                            If CType(Session.Item(CNLoginType), LoginType) = LoginType.Agent And UserCanDoTask("BackDatedMTA") = True Then
                                rangevldDateFrom.MinimumValue = dt_lastRenewalDate
                            Else
                                rangevldDateFrom.MinimumValue = Date.Today.Date
                            End If
                            rangevldDateFrom.ErrorMessage = sMsg_DateAllowedForOutOfSyncMTAForProduct_CP
                        ElseIf sDateAllowedForOutOfSyncMTAForProduct = "2" Then ' Current period + 1
                            If CType(Session.Item(CNLoginType), LoginType) = LoginType.Agent And UserCanDoTask("BackDatedMTA") = True Then
                                If dt_secondLastRenewalDate <> Date.MinValue Then
                                    rangevldDateFrom.MinimumValue = dt_secondLastRenewalDate
                                Else
                                    rangevldDateFrom.MinimumValue = dt_lastRenewalDate
                                End If
                            Else
                                rangevldDateFrom.MinimumValue = Date.Today.Date
                            End If
                            rangevldDateFrom.ErrorMessage = sMsg_DateAllowedForOutOfSyncMTAForProduct_CPPlusOne
                        ElseIf sDateAllowedForOutOfSyncMTAForProduct = "3" Then ' Unrestricted
                            If CType(Session.Item(CNLoginType), LoginType) = LoginType.Agent And UserCanDoTask("BackDatedMTA") = True Then
                                rangevldDateFrom.MinimumValue = dt_PolicyStartDate
                            Else
                                rangevldDateFrom.MinimumValue = Date.Today.Date
                            End If
                            rangevldDateFrom.ErrorMessage = sInvalidEffectiveDate
                        End If
                    End If
            End Select

            rangevldDateFrom.MaximumValue = dt_PolicyExpiryDate.Date
            dt_MinMTADate = rangevldDateFrom.MinimumValue

            'For Editing the Contact Details
            If UserCanDoTask("EditClientDetails") Then


                lnkEditDetails.Visible = True

                'To set the session if user is directly going back from Browoser Back Button
                Session(CNIsSummaryVisited) = True

                Dim oParty As NexusProvider.BaseParty = Session(CNParty)

                Select Case True
                    Case TypeOf oParty Is NexusProvider.PersonalParty
                        If HttpContext.Current.Session.IsCookieless Then
                            lnkEditDetails.OnClientClick = "tb_show(null , '" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))/Secure/Agent/PersonalClientDetails.aspx?mode=edit" _
                                                           & "&partykey=" & oParty.Key.ToString() & "&Code=" & oParty.UserName & "&RequestType=MTA" & "&modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=600&width=750' , null);return false;"
                        Else
                            lnkEditDetails.OnClientClick = "tb_show(null , '../Secure/Agent/PersonalClientDetails.aspx?mode=edit" _
                                                           & "&partykey=" & oParty.Key.ToString() & "&Code=" & oParty.UserName & "&RequestType=MTA" & "&modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=600&width=750' , null);return false;"
                        End If
                        'lnkEditDetails.NavigateUrl = "~/Secure/Agent/PersonalClientDetails.aspx?mode=edit" _
                        '        & "&partykey=" & oParty.Key.ToString() & "&Code=" & oParty.UserName & "&RequestType=MTA" & "&modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=600&width=750"
                    Case TypeOf oParty Is NexusProvider.CorporateParty
                        If HttpContext.Current.Session.IsCookieless Then
                            lnkEditDetails.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))/Secure/Agent/CorporateClientDetails.aspx?mode=edit" _
                                & "&partykey=" & oParty.Key.ToString() & "&Code=" & oParty.UserName & "&RequestType=MTA" & "&modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=600&width=750' , null);return false;"
                        Else
                            lnkEditDetails.OnClientClick = "tb_show(null , '../Secure/Agent/CorporateClientDetails.aspx?mode=edit" _
                                    & "&partykey=" & oParty.Key.ToString() & "&Code=" & oParty.UserName & "&RequestType=MTA" & "&modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=600&width=750' , null);return false;"
                        End If
                        'lnkEditDetails.NavigateUrl = "~/Secure/Agent/CorporateClientDetails.aspx?mode=edit" _
                        '        & "&partykey=" & oParty.Key.ToString() & "&Code=" & oParty.UserName & "&RequestType=MTA" & "&modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=600&width=750"
                End Select
                ViewState.Add(ClientMode, Mode.Edit)
                Session(CNMode) = Mode.Edit
                'For passing the mode of opening  Personal Client Details Page.
                Session("IsThickBox") = True
            End If

            If Not IsPostBack Then
                Try
                    'Initialize it first time
                    Session(CNCurrentRiskKey) = 0
                    '' MTA Reasons from mta_event_description
                    Dim olist As NexusProvider.LookupListCollection = oWebService.GetProductRiskEvents(oQuote.InsuranceFileKey, oQuote.ProductCode, "MTA")
                    'Fetch MTA Event Description Table Data
                    oListColl = oWebService.GetList(NexusProvider.ListType.PMLookup, "MTA_Event_Description", True, False, Nothing, Nothing, Nothing, oXmlElement)

                    Dim CountVar As Integer
                    Dim dt As New DataTable
                    Dim dr As DataRow
                    dt.Columns.Add(New DataColumn("Column1"))
                    dt.Columns.Add(New DataColumn("Column2"))
                    dt.Columns.Add(New DataColumn("Column3"))

                    Dim MTAReasonCodeForCancellation As String = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).MTAReasonForCancellation
                    Dim MTAReasonCodeForReinstatement As String = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode).MTAReasonForReinstatement
                    For CountVar = 0 To olist.Count - 1
                        If olist.Item(CountVar).IsDeleted = False Then 'to filter out the deleted records
                            'Cancellation is allowed only for an agent and AllowAgentCancellation=true
                            If Not ((CType(Session.Item(CNLoginType), LoginType) = LoginType.Agent) And UserCanDoTask("Cancellation")) Then
                                If Not MTAReasonCodeForCancellation.ToUpper.Contains(olist.Item(CountVar).Code.ToUpper) Then
                                    If ((MTAReasonCodeForReinstatement = olist.Item(CountVar).Code And UserCanDoTask("MidTermReinstatement")) Or
                                    (Not MTAReasonCodeForReinstatement = olist.Item(CountVar).Code) And UserCanDoTask("MidTermAdjustment")) Then
                                        dr = dt.NewRow
                                        dr(0) = olist.Item(CountVar).Description
                                        dr(1) = olist.Item(CountVar).Code

                                        'If Page is revisited or browser back button is clicked then to hold the selected value
                                        If Not Session(CNMtaReasonSelected) Is Nothing Then
                                            If Session(CNMtaReasonSelected).ToString().ToUpper().Trim() = olist.Item(CountVar).Code.ToUpper().Trim() Then
                                                dr(2) = "checked=checked"
                                            End If
                                        Else
                                            dr(2) = ""
                                        End If
                                        dt.Rows.Add(dr)
                                    Else

                                    End If
                                End If
                            Else
                                If Not MTAReasonCodeForReinstatement = olist.Item(CountVar).Code Then

                                    If (MTAReasonCodeForCancellation.ToUpper.Contains(olist.Item(CountVar).Code.ToUpper) And UserCanDoTask("MidTermCancellation")) Or
                                    (Not MTAReasonCodeForCancellation.ToUpper.Contains(olist.Item(CountVar).Code.ToUpper) And UserCanDoTask("MidTermAdjustment")) Then

                                        dr = dt.NewRow
                                        dr(0) = olist.Item(CountVar).Description
                                        dr(1) = olist.Item(CountVar).Code

                                        'If Page is revisited or browser back button is clicked then to hold the selected value
                                        If Not Session(CNMtaReasonSelected) Is Nothing Then
                                            If Session(CNMtaReasonSelected).ToString().ToUpper().Trim() = olist.Item(CountVar).Code.ToUpper().Trim() Then
                                                dr(2) = "checked=checked"
                                            End If
                                        Else
                                            dr(2) = ""
                                        End If
                                        dt.Rows.Add(dr)
                                    End If
                                End If
                            End If
                        End If
                    Next
                    GridMTAReasons.DataSource = dt
                    GridMTAReasons.DataBind()

                Finally
                    oWebService = Nothing
                End Try

                'Disable Temporary MTA system option
                Dim oDisableTemporaryMTAOptionSettings As NexusProvider.OptionTypeSetting
                Try
                    oWebService = New NexusProvider.ProviderManager().Provider
                    oDisableTemporaryMTAOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.DisableTempMTAs)
                Finally
                    oWebService = Nothing
                End Try

                'Allow permanent MTA on LAPSED Policy
                If oQuote.InsuranceFileStatusCode = "LAP" OrElse UserCanDoTask("MidTermAdjustmentTemp") = False Then
                    ' Making PnlMTADateTo hide if AllowTempMTA=false
                    PnlMTADateTo.Visible = False
                    RangevldExpiryDate.Enabled = False
                Else
                    RangevldExpiryDate.MinimumValue = dt_PolicyStartDate
                    'RangevldExpiryDate: date can't be grater than PolicyExpiryDate
                    RangevldExpiryDate.MaximumValue = dt_PolicyExpiryDate.Date
                End If

                If Session(CNMTAType) = MTAType.REINSTATEMENT Then
                    PnlMTADateFrom.Visible = False
                    PnlMTADateTo.Visible = False
                    GridMTAReasons.Visible = False
                    pnlReinstatementDate.Visible = True
                    CustVldMTAReasonRequired.Enabled = False
                    txtReinstatementDate.Text = oQuote.CoverStartDate.ToShortDateString
                    reqReinstatementDate.Enabled = True
                    cmpVldReinstatementDate.Enabled = True
                    custvldReinstatementDate.Enabled = True
                    vldDateFrom.Enabled = False

                    'Reinstatement date will be disabled as it should be defaulted to cancelled date
                    txtReinstatementDate_uctCal.Enabled = False

                    ' Check for OOS Reinstatement. Get the last cancelled record cover start date.
                    ' NOTE: Temporary logic for reinstatement only. New logic to be implemented for all to cater for complex cases.
                    For iTempVar = oPolicy.Count - 1 To 0 Step -1
                        If oPolicy(iTempVar).InsuranceFileTypeCode.Trim().ToUpper() = "MTACAN" Then
                            dt_LastCancelledDate = oPolicy(iTempVar).CoverStartDate
                            Exit For
                        End If
                    Next
                    If dt_LastCancelledDate IsNot Nothing Then
                        btnSubmit.OnClientClick = "javascript:return validateBackDatedReinstatement();"
                    End If
                Else
                    pnlReinstatementDate.Visible = False
                    btnSubmit.OnClientClick = "javascript:return ValidateMTADateAndRenewal();"
                    vldDateFrom.Enabled = True
                End If

                Dim sXML As String = oXmlElement.OuterXml
                Dim xmlDoc As New System.Xml.XmlDocument()

                If oXmlElement IsNot Nothing Then
                    xmlDoc.LoadXml(sXML)
                End If
                Dim oMtaDescriptionList As Hashtable = New Hashtable()

                If oListColl IsNot Nothing Then
                    If xmlDoc.ChildNodes IsNot Nothing Then
                        Dim iIfOther As Integer = 2
                        Dim sCode As String = String.Empty
                        For icount1 As Integer = 0 To oListColl.Count - 1
                            For iChildCount As Integer = 0 To xmlDoc.ChildNodes(0).ChildNodes(icount1).ChildNodes.Count - 1
                                If xmlDoc.ChildNodes(0).ChildNodes(icount1).ChildNodes(iChildCount).Name.Trim.ToUpper = "CODE" Then
                                    sCode = xmlDoc.ChildNodes(0).ChildNodes(icount1).ChildNodes(iChildCount).InnerText
                                End If

                                If xmlDoc.ChildNodes(0).ChildNodes(icount1).ChildNodes(iChildCount).Name.Trim.ToUpper = "IS_OTHER" Then
                                    iIfOther = CInt(xmlDoc.ChildNodes(0).ChildNodes(icount1).ChildNodes(iChildCount).InnerText)
                                End If
                            Next
                            oMtaDescriptionList.Add(sCode, iIfOther)
                        Next
                    End If
                End If
                Dim jSearializer As New System.Web.Script.Serialization.JavaScriptSerializer()
                hdnMtaReasonList.Value = jSearializer.Serialize(oMtaDescriptionList)

            End If

            If IsDate(txtExpiryDate.Text) Then
                rangevldDateFrom.MinimumValue = dt_PolicyStartDate
            End If

            If Request("__EVENTARGUMENT") = "GetMTADescription" Then
                btnSubmit_Click(Nothing, Nothing)
            End If
        End Sub

        Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
            If Not IsloggedInBranchAssignedToProduct(oQuote.ProductCode) Then
                Exit Sub
            End If
            Page.Validate()
            If Page.IsValid And Not String.IsNullOrEmpty(txtEffectiveDate.Text) Then
                Dim oRiskType As Config.RiskType
                Dim sUserGroupCodeForBackdatedMTA As String = ""
                Dim sTaskGroupCodeForBackdatedMTA As String = ""
                Dim sUserGroupIdForBackdatedMTA As String = ""
                Dim sTaskGroupIdForBackdatedMTA As String = ""
                Dim bIsInBackDatedMode As Boolean = IIf(Session(CNBaseInsuranceFileKey) IsNot Nothing AndAlso Session(CNBaseInsuranceFileKey) <> Session(CNInsuranceFileKey), True, False)
                Dim oOOSMTAVersions As NexusProvider.BaseInsuranceFileKeyCollection

                oQuote = CType(Session(CNQuote), NexusProvider.Quote)
                Dim PreviousCoverStartDate As DateTime = oQuote.CoverStartDate


                oOOSMTAVersions = CType(Session("OOS_MTA_SAVED_VERSIONS"), NexusProvider.BaseInsuranceFileKeyCollection)
                If oOOSMTAVersions IsNot Nothing And oOOSMTAVersions.Count > 0 Then
                    For Each oVersion As NexusProvider.BaseInsuranceFileKey In oOOSMTAVersions
                        oWebService = New NexusProvider.ProviderManager().Provider
                        oWebService.DeleteBackDatedVersions(oVersion.BaseInsuranceFileKey)
                    Next
                End If

                If Not String.IsNullOrEmpty(txtEffectiveDate.Text) Then
                    Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
                    Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
                    Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name

                    Dim SelectedPaymentOption As String = Request("rdoMTAReasonList")
                    Session(CNMtaReasonSelected) = SelectedPaymentOption 'To hold the selected mta reason

                    'Session Values are set for MTAQuote
                    Session(CNMode) = Mode.Edit
                    Session(CNQuoteMode) = QuoteMode.MTAQuote
                    Session(CNBackDatedReinstatement) = CBool(hfReinstatementBackdatedIndicator.Value)

                    If Session(CNMTAType) = MTAType.REINSTATEMENT Then
                        Dim oMtaQuote As New NexusProvider.MTA()

                        oWebService = New NexusProvider.ProviderManager().Provider

                        Session(CNMtaReasonSelected) = oProduct.MTAReasonForReinstatement 'To hold the selected mta reason
                        oMtaQuote.InsuranceFileKey = CType(Session(CNInsuranceFileKey), Integer)
                        oMtaQuote.MTAType = MTAType.REINSTATEMENT
                        oMtaQuote.TypeOfMta = "PERMANENT"
                        oMtaQuote.MtaReason = oProduct.MTAReasonForReinstatement
                        Dim iGracePeriod As Integer = 0
                        With oQuote
                            oMtaQuote.MtaEffectiveDate = CType(txtReinstatementDate.Text, Date)
                            oMtaQuote.MtaExpiryDate = .CoverEndDate
                            oMtaQuote.AccountHandlerCnt = .AccountHandlerCnt
                            oMtaQuote.AlternateReference = .AlternativeRef
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
                            If Session(CNMTAType) <> MTAType.REINSTATEMENT Then
                                oMtaQuote.LapseCancelDate = .LapseCancelDate
                            End If
                            oMtaQuote.AlternateReference = .AlternativeRef
                            oMtaQuote.LapseCancelReasonCode = .LapseCancelReasonCode

                            If oQuote.Risks(0).RiskCode IsNot Nothing Then
                                iGracePeriod = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.GracePeriod, NexusProvider.RiskTypeOptions.Code, oProduct.ProductCode, oQuote.Risks(0).RiskCode)
                            Else
                                iGracePeriod = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.GracePeriod, NexusProvider.RiskTypeOptions.Code, oProduct.ProductCode, oQuote.Risks(0).RiskTypeCode)
                            End If

                            oMtaQuote.QuoteExpiryDate = CDate(Date.Now).AddDays(iGracePeriod).ToShortDateString()
                            oMtaQuote.QuoteTimeStamp = .TimeStamp
                            oMtaQuote.ReferredAtRenewal = .ReferredAtRenewal
                            oMtaQuote.ReferredOnMTA = .ReferredAtMTA
                            oMtaQuote.Regarding = .Regarding
                            oMtaQuote.RenewalMethodCode = .RenewalMethodCode
                            oMtaQuote.StopReasonCode = .StopReasonCode
                            oMtaQuote.FrequencyCode = .FrequencyCode
                            oMtaQuote.PolicyKey = .InsuranceFileRef
                            oMtaQuote.IsReinstatement = True
                            oMtaQuote.TranactionType = "MTR"
                            oMtaQuote.CoInsurancePlacement = .CoinsurancePlacement
                            If .AnniversaryDate <> Date.MinValue Then
                                oMtaQuote.AnniversaryDate = .AnniversaryDate
                            End If

                            oMtaQuote.OldPolicyNumber = .OldPolicyNumber
                            oMtaQuote.AlternateReference = .AlternativeRef
                        End With
                        Try
                            Session(CNTempOriginalMTAQuote) = Session(CNQuote)
                            oWebService.AddMtaQuote(oMtaQuote, oMtaQuote.BranchCode)
                            oQuote = oWebService.GetHeaderAndSummariesByKey(oMtaQuote.InsuranceFileKey)
                            Dim oCopyMTARiskOption As NexusProvider.OptionTypeSetting
                            oCopyMTARiskOption = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, 116)
                            If oCopyMTARiskOption IsNot Nothing AndAlso oCopyMTARiskOption.OptionValue = "1" Then
                                oWebService.GetHeaderAndRisksByKey(oQuote)
                                For iCount As Integer = 0 To oQuote.Risks.Count - 1
                                    oWebService.GetRisk(oQuote.Risks(iCount).Key, iCount, oQuote, oQuote.BranchCode, v_bIgnoreLocking:=True)
                                Next
                            End If
                            Session(CNQuoteInSync) = True

                            ' --- Discount auto-reapply after risk copy (reinstatement) ---
                            If Session(CNMTAType) IsNot Nothing AndAlso CInt(Session(CNMTAType)) <> MTAType.CANCELLATION Then
                                Dim oOriginalPolicyKey As Integer = CType(Session(CNInsuranceFileKey), Integer)
                                Try
                                    Dim oOriginalDiscount As NexusProvider.PolicyDiscount = oWebService.GetPolicyDiscountInfo(oOriginalPolicyKey)
                                    If oOriginalDiscount IsNot Nothing AndAlso oOriginalDiscount.IsDiscountApplied Then
                                        If oOriginalDiscount.RecurringTypeId = 2 OrElse oOriginalDiscount.RecurringTypeId = 3 Then
                                            Dim sFailure As String = ""
                                            Dim sTransType As String = "MTA"
                                            oWebService.ApplyPolicyDiscount(
                                                oQuote.InsuranceFileKey, 0, sTransType, 0, sFailure,
                                                oOriginalDiscount.DiscountedPremium,
                                                oOriginalDiscount.DiscountPercentage,
                                                0,
                                                oOriginalDiscount.DiscountReasonId,
                                                oOriginalDiscount.RecurringTypeId)

                                            ' Fetch actual MTA discounted total from new quote (not NB values)
                                            Dim crMtaDiscounted As Decimal = 0D
                                            Try
                                                crMtaDiscounted = oWebService.GetPolicyDiscountTotalPremium(oQuote.InsuranceFileKey)
                                            Catch
                                                crMtaDiscounted = CDec(oOriginalDiscount.DiscountedPremium)
                                            End Try
                                            Dim crMtaOriginal As Decimal = Math.Round(crMtaDiscounted / CDec(1 + oOriginalDiscount.DiscountPercentage / 100), 2)

                                            Session(CNPolicyDiscountReasonId) = oOriginalDiscount.DiscountReasonId
                                            Session(CNPolicyDiscountPercentage) = oOriginalDiscount.DiscountPercentage
                                            Session(CNPolicyDiscountedPremium) = crMtaDiscounted
                                            Session(CNPolicyDiscountTotalPremium) = crMtaOriginal
                                            Session(CNPolicyDiscountRecurringTypeId) = oOriginalDiscount.RecurringTypeId
                                            Session(CNPolicyDiscountApplied) = True
                                            ' Do NOT set POLICY_DISCOUNT_APPLIED_TO_RISKS = True here.
                                            ' mtareason auto-applies discount but the backup (discount_original_this_premium)
                                            ' copied from NB is stale. Setting the flag True would cause rollback
                                            ' to fire on the user's first Apply, restoring the wrong NB value.
                                            ' The flag is set True only after the user explicitly clicks Apply.
                                            Session("POLICY_DISCOUNT_APPLIED_TO_RISKS") = False
                                        Else
                                            Session(CNPolicyDiscountApplied) = False
                                            Session.Remove(CNPolicyDiscountReasonId)
                                            Session.Remove(CNPolicyDiscountPercentage)
                                            Session.Remove(CNPolicyDiscountedPremium)
                                            Session.Remove(CNPolicyDiscountTotalPremium)
                                            Session.Remove(CNPolicyDiscountRecurringTypeId)
                                        End If
                                    End If
                                Catch ex As Exception
                                    ' Log error and continue
                                End Try
                            End If
                            ' --- End discount auto-reapply ---

                            oQuote.PreviousLiveVersionCoverStartDate = PreviousCoverStartDate
                            Session(CNQuote) = oQuote
                            Session(CNQuoteInSync) = False
                            Session.Remove(CNOI)
                            Session(CNInsuranceFileKey) = oQuote.InsuranceFileKey
                        Finally
                            oWebService = Nothing
                            oMtaQuote = Nothing
                        End Try

                        DoRedirect()
                    ElseIf CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).MTAReasonForCancellation.ToUpper.Contains(SelectedPaymentOption.ToUpper) Then
                        'If selected MTAReason is CANCELLATION then
                        Session(CNMTAType) = MTAType.CANCELLATION

                        Dim oWebService As NexusProvider.ProviderBase
                        Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                        Dim oMtaQuote As New NexusProvider.MTA()

                        oWebService = New NexusProvider.ProviderManager().Provider

                        oMtaQuote.InsuranceFileKey = CType(Session(CNInsuranceFileKey), Integer)
                        oMtaQuote.MTAType = CNMTATypeDesc 'as discessed MTA Type is fixed for both the cases either PERMANENT or CANCELLATION
                        oMtaQuote.TypeOfMta = "PERMANENT" 'CNMTATypeDesc
                        oMtaQuote.MtaReason = SelectedPaymentOption
                        If (Not String.IsNullOrWhiteSpace(MTADescription.Value)) AndAlso MTADescription.Value.Length > 0 Then
                            SelectedPaymentOption += " - " + MTADescription.Value.Trim
                        End If
                        oMtaQuote.MtaReason = SelectedPaymentOption

                        oMtaQuote.MtaEffectiveDate = txtEffectiveDate.Text
                        oMtaQuote.MtaExpiryDate = dt_PolicyExpiryDate
                        oMtaQuote.LapseCancelDate = txtEffectiveDate.Text
                        Dim iGracePeriod As Integer = 0
                        With oQuote
                            oMtaQuote.AccountHandlerCnt = .AccountHandlerCnt
                            oMtaQuote.AnalysisCode = .AnalysisCode
                            oMtaQuote.AlternateReference = .AlternativeRef
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
                            oMtaQuote.QuoteExpiryDate = CDate(Date.Now).AddDays(iGracePeriod).ToShortDateString() ' .QuoteExpiryDate
                            oMtaQuote.QuoteTimeStamp = .TimeStamp
                            oMtaQuote.ReferredAtRenewal = .ReferredAtRenewal
                            oMtaQuote.ReferredOnMTA = .ReferredAtMTA
                            oMtaQuote.Regarding = .Regarding
                            oMtaQuote.RenewalMethodCode = .RenewalMethodCode
                            oMtaQuote.StopReasonCode = .StopReasonCode
                            oMtaQuote.FrequencyCode = .FrequencyCode '"ANNUAL" '.FrequencyCode
                            oMtaQuote.PolicyKey = .InsuranceFileRef
                            oMtaQuote.TranactionType = "MTC"
                            oMtaQuote.OldPolicyNumber = .OldPolicyNumber
                            oMtaQuote.CorrespondenceType = .CorrespondenceType
                            oMtaQuote.DefaultPreferredCorrespondence = .DefaultPreferredCorrespondence
                            oMtaQuote.IsAgentReceiveCorrespondence = .IsAgentReceiveCorrespondence
                            oMtaQuote.AlternateReference = .AlternativeRef
                            If .AnniversaryDate <> Date.MinValue Then
                                oMtaQuote.AnniversaryDate = .AnniversaryDate
                            End If
                            oMtaQuote.CoInsurancePlacement = .CoinsurancePlacement
                        End With

                        If CType(txtEffectiveDate.Text, Date) < oQuote.CoverStartDate AndAlso
                            Not GetCurrentMTAType = MTAType.TEMPORARY Then ' this should be backdated mta
                            Session(CNIsBackDatedMTA) = True
                        End If
                        Try
                            Session(CNTempOriginalMTAQuote) = Session(CNQuote)
                            oWebService.AddMtaQuote(oMtaQuote, oMtaQuote.BranchCode)
                            oQuote = oWebService.GetHeaderAndSummariesByKey(oMtaQuote.InsuranceFileKey)
                            Dim oCopyMTARiskOption As NexusProvider.OptionTypeSetting
                            oCopyMTARiskOption = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, 116)
                            If oCopyMTARiskOption IsNot Nothing AndAlso oCopyMTARiskOption.OptionValue = "1" Then
                                oWebService.GetHeaderAndRisksByKey(oQuote)
                                For iCount As Integer = 0 To oQuote.Risks.Count - 1
                                    oWebService.GetRisk(oQuote.Risks(iCount).Key, iCount, oQuote, oQuote.BranchCode, v_bIgnoreLocking:=True)
                                Next
                            End If
                            Session(CNQuoteInSync) = True

                            oQuote.PreviousLiveVersionCoverStartDate = PreviousCoverStartDate
                            Session(CNQuote) = oQuote
                            If Session(CNIsBackDatedMTA) Then
                                Session(CNCurrentRiskKey) = Nothing
                            End If
                        Finally
                            oWebService = Nothing
                            oMtaQuote = Nothing
                        End Try

                        Session(CNQuoteInSync) = True
                        DoRedirect()
                    Else
                        'If Not CANCELLATION then its TEMPORARY OR PERMANENT
                        Dim oWebService As NexusProvider.ProviderBase
                        Dim oMtaQuote As New NexusProvider.MTA()

                        oWebService = New NexusProvider.ProviderManager().Provider

                        oMtaQuote.InsuranceFileKey = CType(Session(CNInsuranceFileKey), Integer)
                        oMtaQuote.MTAType = CNMTATypeDesc  'as discessed MTA Type is fixed for both the cases either PERMANENT or CANCELLATION
                        oMtaQuote.TypeOfMta = CNMTATypeDesc

                        If (Not String.IsNullOrWhiteSpace(MTADescription.Value)) AndAlso MTADescription.Value.Length > 0 Then
                            SelectedPaymentOption += " - " + MTADescription.Value.Trim
                        End If

                        oMtaQuote.MtaReason = SelectedPaymentOption

                        oMtaQuote.MtaEffectiveDate = txtEffectiveDate.Text.Trim()

                        If txtExpiryDate.Text = Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper() Or txtExpiryDate.Text = "" Then
                            'its a PERMANENT MTA, 
                            Session(CNMTAType) = MTAType.PERMANENT
                            oMtaQuote.MTAType = CNMTATypeDesc
                            oMtaQuote.TypeOfMta = CNMTATypeDesc
                            oMtaQuote.MtaExpiryDate = oQuote.CoverEndDate 'MTAExpiryDate must be equal to the PolicyExpiryDate

                        Else 'its a TEMPORARY MTA
                            Session(CNMTAType) = MTAType.TEMPORARY
                            oMtaQuote.MTAType = "TEMPORARY" 'MTAType.TEMPORARY
                            oMtaQuote.TypeOfMta = "TEMPORARY" 'MTAType.TEMPORARY
                            oMtaQuote.MtaExpiryDate = txtExpiryDate.Text.Trim()
                            Session(CNIsBackDatedMTA) = False
                        End If
                        Dim iGracePeriod As Integer = 0
                        With oQuote
                            oMtaQuote.AccountHandlerCnt = .AccountHandlerCnt
                            oMtaQuote.AnalysisCode = .AnalysisCode
                            oMtaQuote.AlternateReference = .AlternativeRef
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
                            oMtaQuote.QuoteExpiryDate = CDate(DateTime.Now).AddDays(iGracePeriod).ToShortDateString() ' .QuoteExpiryDate
                            oMtaQuote.QuoteTimeStamp = .TimeStamp
                            oMtaQuote.ReferredAtRenewal = .ReferredAtRenewal
                            oMtaQuote.ReferredOnMTA = .ReferredAtMTA
                            oMtaQuote.Regarding = .Regarding
                            oMtaQuote.RenewalMethodCode = .RenewalMethodCode
                            oMtaQuote.StopReasonCode = .StopReasonCode
                            oMtaQuote.FrequencyCode = .FrequencyCode '"ANNUAL" '.FrequencyCode
                            oMtaQuote.PolicyKey = .InsuranceFileRef
                            oMtaQuote.TranactionType = "MTA"
                            If .AnniversaryDate <> Date.MinValue Then
                                oMtaQuote.AnniversaryDate = .AnniversaryDate
                            End If
                            oMtaQuote.CoInsurancePlacement = .CoinsurancePlacement
                            oMtaQuote.OldPolicyNumber = .OldPolicyNumber
                            oMtaQuote.CorrespondenceType = .CorrespondenceType
                            oMtaQuote.DefaultPreferredCorrespondence = .DefaultPreferredCorrespondence
                            oMtaQuote.IsAgentReceiveCorrespondence = .IsAgentReceiveCorrespondence
                            oMtaQuote.AlternateReference = .AlternativeRef
                        End With
                        'set session CNIsBackDatedMTA as true.So that it can be used further
                        If CType(txtEffectiveDate.Text, Date) < oQuote.CoverStartDate And Session(CNMTAType) <> MTAType.TEMPORARY Then
                            Session(CNIsBackDatedMTA) = True
                        End If

                        'PM042978 START
                        Dim oRenQuote As New NexusProvider.Quote

                        Try
                            oRenQuote.InsuranceFileKey = oQuote.InsuranceFileKey
                            Dim oPreRenStatus As NexusProvider.RenewalStatus
                            oPreRenStatus = oWebService.GetRenewalStatus(oRenQuote.InsuranceFileKey)
                            If Not oPreRenStatus Is Nothing And Not oPreRenStatus.RenewalStatusTypeDescription Is Nothing Then
                                If oPreRenStatus.RenewalStatusTypeDescription.ToString <> "" Then
                                    oRenQuote = oWebService.GetHeaderAndSummariesByKey(oRenQuote.InsuranceFileKey)
                                    oWebService.DeleteRenewal(oRenQuote)
                                    Dim oInsuranceFileDetailsCollection As NexusProvider.InsuranceFileDetailsCollection
                                    oInsuranceFileDetailsCollection = oWebService.FindPolicy(oRenQuote.InsuranceFileRef, "", "", NexusProvider.InsuranceFileTypes.POLICY, False)
                                    For Each oInsuranceFileDetails As NexusProvider.InsuranceFileDetails In oInsuranceFileDetailsCollection
                                        oRenQuote.InsuranceFileKey = oInsuranceFileDetails.InsuranceFileKey
                                        Exit For
                                    Next
                                End If
                            Else
                                'Deletion of all previous version of the Renewal
                                oRenQuote = oWebService.GetHeaderAndSummariesByKey(oRenQuote.InsuranceFileKey)
                                Dim oInsuranceFileDetailsCollection As NexusProvider.InsuranceFileDetailsCollection
                                oInsuranceFileDetailsCollection = oWebService.FindPolicy(oRenQuote.InsuranceFileRef, "", "", NexusProvider.InsuranceFileTypes.RENEWAL, False)
                                If oInsuranceFileDetailsCollection IsNot Nothing Then
                                    For Each oInsuranceFileDetails As NexusProvider.InsuranceFileDetails In oInsuranceFileDetailsCollection
                                        Dim oTempQuote As New NexusProvider.Quote
                                        oTempQuote = oWebService.GetHeaderAndSummariesByKey(oInsuranceFileDetails.InsuranceFileKey)
                                        oWebService.DeleteRenewal(oTempQuote)
                                    Next
                                End If
                            End If

                        Catch ex As NexusProvider.NexusException
                            'Policy locking error
                            Select Case CType(ex.Errors(0), NexusProvider.NexusError).Code
                                Case "200" 'Policy Locking
                                    'Show policy locking error as alert
                                    Dim sMessage As String = "alert('" + ex.Errors(0).Description + "')"
                                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", sMessage, True)
                                    Server.ClearError()
                                    ClearQuote()
                                    Exit Sub
                                Case Else
                                    Throw ex
                            End Select
                        Finally
                            oQuote = Nothing
                        End Try

                        'PM042978 END

                        Try
                            If IsPostBack Then
                                oWebService = New NexusProvider.ProviderManager().Provider
                                Session(CNTempOriginalMTAQuote) = Session(CNQuote)
                                oWebService.AddMtaQuote(oMtaQuote, oMtaQuote.BranchCode)
                                oQuote = oWebService.GetHeaderAndSummariesByKey(oMtaQuote.InsuranceFileKey)
                                Dim oCopyMTARiskOption As NexusProvider.OptionTypeSetting
                                oCopyMTARiskOption = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, 116)
                                If oCopyMTARiskOption IsNot Nothing AndAlso oCopyMTARiskOption.OptionValue = "1" Then
                                    oWebService.GetHeaderAndRisksByKey(oQuote)
                                    For iCount As Integer = 0 To oQuote.Risks.Count - 1
                                        oWebService.GetRisk(oQuote.Risks(iCount).Key, iCount, oQuote, oQuote.BranchCode, v_bIgnoreLocking:=True)
                                    Next
                                End If

                                ' --- Discount auto-reapply after risk copy ---
                                If Session(CNMTAType) IsNot Nothing AndAlso CInt(Session(CNMTAType)) <> MTAType.CANCELLATION Then
                                    Dim oOriginalPolicyKey As Integer = CType(Session(CNInsuranceFileKey), Integer)
                                    Try
                                        Dim oOriginalDiscount As NexusProvider.PolicyDiscount = oWebService.GetPolicyDiscountInfo(oOriginalPolicyKey)
                                        If oOriginalDiscount IsNot Nothing AndAlso oOriginalDiscount.IsDiscountApplied Then
                                            If oOriginalDiscount.RecurringTypeId = 2 OrElse oOriginalDiscount.RecurringTypeId = 3 Then
                                                ' Auto-reapply the carried discount to the new MTA quote
                                                Dim sFailure As String = ""
                                                Dim sTransType As String = "MTA"
                                                oWebService.ApplyPolicyDiscount(
                                                    oQuote.InsuranceFileKey, 0, sTransType, 0, sFailure,
                                                    oOriginalDiscount.DiscountedPremium,
                                                    oOriginalDiscount.DiscountPercentage,
                                                    0,
                                                    oOriginalDiscount.DiscountReasonId,
                                                    oOriginalDiscount.RecurringTypeId)

                                                ' Fetch actual MTA discounted total from new quote (not NB values)
                                                Dim crMtaDiscounted As Decimal = 0D
                                                Try
                                                    crMtaDiscounted = oWebService.GetPolicyDiscountTotalPremium(oQuote.InsuranceFileKey)
                                                Catch
                                                    crMtaDiscounted = CDec(oOriginalDiscount.DiscountedPremium)
                                                End Try
                                                Dim crMtaOriginal As Decimal = Math.Round(crMtaDiscounted / CDec(1 + oOriginalDiscount.DiscountPercentage / 100), 2)

                                                Session(CNPolicyDiscountReasonId) = oOriginalDiscount.DiscountReasonId
                                                Session(CNPolicyDiscountPercentage) = oOriginalDiscount.DiscountPercentage
                                                Session(CNPolicyDiscountedPremium) = crMtaDiscounted
                                                Session(CNPolicyDiscountTotalPremium) = crMtaOriginal
                                                Session(CNPolicyDiscountRecurringTypeId) = oOriginalDiscount.RecurringTypeId
                                                Session(CNPolicyDiscountApplied) = True
                                                ' Do NOT set POLICY_DISCOUNT_APPLIED_TO_RISKS = True here.
                                                ' mtareason auto-applies discount but the backup (discount_original_this_premium)
                                                ' copied from NB is stale. Setting the flag True would cause rollback
                                                ' to fire on the user's first Apply, restoring the wrong NB value.
                                                ' The flag is set True only after the user explicitly clicks Apply.
                                                Session("POLICY_DISCOUNT_APPLIED_TO_RISKS") = False
                                            Else
                                                ' AC: Recurring type 1 ("This Transaction") or 0 — clear discount, MTA starts clean
                                                Session(CNPolicyDiscountApplied) = False
                                                Session.Remove(CNPolicyDiscountReasonId)
                                                Session.Remove(CNPolicyDiscountPercentage)
                                                Session.Remove(CNPolicyDiscountedPremium)
                                                Session.Remove(CNPolicyDiscountTotalPremium)
                                                Session(CNPolicyDiscountRecurringTypeId) = oOriginalDiscount.RecurringTypeId
                                            End If
                                        End If
                                    Catch ex As Exception
                                        ' Log error and continue — user can apply discount manually on PremiumDisplay
                                    End Try
                                End If
                                ' --- End discount auto-reapply ---

                                oQuote.TransactionType = NexusProvider.Quote.Enum_TransactionType.MTA
                                oQuote.PreviousLiveVersionCoverStartDate = PreviousCoverStartDate
                                Session(CNQuote) = oQuote
                                Session(CNQuoteInSync) = False
                                Session.Remove(CNOI)
                                Session(CNInsuranceFileKey) = oQuote.InsuranceFileKey
                                If Session(CNIsBackDatedMTA) Then
                                    Session(CNCurrentRiskKey) = Nothing
                                End If
                            End If
                        Finally
                            oWebService = Nothing
                            oMtaQuote = Nothing
                        End Try
                        DoRedirect()
                    End If
                End If
            End If
        End Sub
        ''' <summary>
        ''' This Mehod is to redirect from this page
        ''' </summary>
        ''' <remarks></remarks>
        Sub DoRedirect()
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            If oQuote.Risks.Count = 1 Then
                'This will work only when risk count is 1
                Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
                Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name
                Dim doc As New XmlDocument
                Dim nodes As XmlNodeList
                Dim oRiskType As Config.RiskType
                'this case is for the scenario when oQuote.Risks.Count is 1(mentioned in the starting of Sub), that means we can pass hardcoded 0 value for the Risks counter - otherwise will give issue in case of backdated MTA when then current version of Policy has higher number of risks.
                If oQuote.Risks(0).RiskCode Is Nothing Then
                    oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(0).RiskTypeCode.Trim)
                Else
                    oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(0).RiskCode.Trim)
                End If
                DataSetFunctions.GetScreens()
                Dim oRisk As New NexusProvider.RiskType
                oRisk.DataModelCode = oRiskType.DataModelCode
                oRisk.Name = oRiskType.Name
                oRisk.Path = oRiskType.Path
                oRisk.RiskCode = oRiskType.RiskCode
                Session(CNRiskType) = oRisk
                Dim sRiskFolder As String = sProductFolder & "/" & oRisk.Path & "/"

                If System.IO.File.Exists(Server.MapPath(sRiskFolder & "MTAReason.config")) Then
                    doc.Load(Server.MapPath(sRiskFolder & "MTAReason.config"))
                    nodes = doc.SelectNodes("/MTAReasonMapping/MTAReason")
                    If Session(CNMtaReasonSelected) IsNot Nothing Then
                        Dim bFound As Boolean = False
                        For Each node As XmlNode In nodes
                            If node.Attributes("name").Value = Session(CNMtaReasonSelected) Then
                                'Run Default Rules Edit is called to load the data from Sub Main
                                EditDefaultRule()

                                bFound = True
                                Response.Redirect(node.Attributes("page").Value, False)
                            End If
                        Next
                        If bFound = False Then
                            'if reason is not mapped in the mtareason page
                            If DataSetFunctions.sSummaryOfCover.ToLower = "true" Then
                                Response.Redirect(DataSetFunctions.sSummaryOfCoverURL, False)
                            Else
                                Response.Redirect("~/secure/PremiumDisplay.aspx", False)
                            End If
                        End If
                    End If
                Else
                    'if mtareason page is not available
                    If DataSetFunctions.sSummaryOfCover.ToLower = "true" Then
                        Response.Redirect(DataSetFunctions.sSummaryOfCoverURL, False)
                    Else
                        Response.Redirect("~/secure/PremiumDisplay.aspx", False)
                    End If
                End If
            Else
                'In case of more than one risk
                If DataSetFunctions.sSummaryOfCover.ToLower = "true" Then
                    Response.Redirect(DataSetFunctions.sSummaryOfCoverURL, False)
                Else
                    Response.Redirect("~/secure/PremiumDisplay.aspx", False)
                End If
            End If
        End Sub
        Sub EditDefaultRule()
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
            Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name & "/"
            Dim sScreenCode As String = Nothing
            Dim oRiskType As Config.RiskType
            oWebService = New NexusProvider.ProviderManager().Provider

            If oQuote.Risks(Session(CNCurrentRiskKey)).RiskCode Is Nothing Then
                oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(Session(CNCurrentRiskKey)).RiskTypeCode.Trim)
            Else
                oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(Session(CNCurrentRiskKey)).RiskCode.Trim)
            End If
            If oQuote.Risks(Session(CNCurrentRiskKey)).ScreenCode IsNot Nothing Then
                If oQuote.Risks(Session(CNCurrentRiskKey)).ScreenCode.Trim.Length <> 0 Then
                    sScreenCode = oQuote.Risks(Session(CNCurrentRiskKey)).ScreenCode
                Else
                    sScreenCode = GetScreenCode(sProductFolder & oRiskType.Path & "\" & oProduct.FullQuoteConfig)
                End If
            Else
                sScreenCode = GetScreenCode(sProductFolder & oRiskType.Path & "\" & oProduct.FullQuoteConfig)
            End If
            
            If oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset Is Nothing OrElse oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset = "" Then
                oWebService.GetRisk(oQuote.Risks(Session(CNCurrentRiskKey)).Key, Session(CNCurrentRiskKey), oQuote, oQuote.BranchCode, False, "U", True)
            End If
            If Session(CNMTAType) IsNot Nothing And Session(CNRenewal) Is Nothing Then
                If Session(CNMTAType) = MTAType.PERMANENT Or Session(CNMTAType) = MTAType.TEMPORARY Then
                    oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset = oWebService.RunDefaultRulesEdit(sScreenCode, oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset, Nothing, Nothing, "MTA")
                ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then
                    oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset = oWebService.RunDefaultRulesEdit(sScreenCode, oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset, Nothing, Nothing, "MTC")
                ElseIf (Session(CNMTAType) = MTAType.REINSTATEMENT) Then
                    oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset = oWebService.RunDefaultRulesEdit(sScreenCode, oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset, Nothing, Nothing, "MTR")
                End If
            End If
        End Sub
        Protected Sub CustVldMTAReasonRequired_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles CustVldMTAReasonRequired.ServerValidate
            If Not Request("rdoMTAReasonList") Is Nothing Then
                args.IsValid = True
            Else
                args.IsValid = False
            End If
        End Sub

        Protected Sub custvldDateFrom_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles custvldDateFrom.ServerValidate

            If Not String.IsNullOrEmpty(txtEffectiveDate.Text) And IsDate(txtEffectiveDate.Text) Then
                If Not (txtEffectiveDate.Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper()) Then
                    If Not (rangevldDateFrom.IsValid) Then
                        args.IsValid = False
                        custvldDateFrom.ErrorMessage = String.Empty
                        cmpVldEffectiveDate.ErrorMessage = String.Empty
                        Exit Sub
                    End If
                    If CType(txtEffectiveDate.Text, Date) < FD_MTADate.Date Then
                        args.IsValid = False
                        custvldDateFrom.ErrorMessage = "MTA Effective Date Cannot be Prior to the Permanent MTA Date"
                        rangevldDateFrom.ErrorMessage = String.Empty
                        cmpVldEffectiveDate.ErrorMessage = String.Empty
                        Exit Sub
                    End If
                End If
            Else
                args.IsValid = False
            End If

        End Sub

        Protected Sub CustvldExpiryDate_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles CustvldExpiryDate.ServerValidate
            If Not String.IsNullOrEmpty(txtExpiryDate.Text) AndAlso Not String.IsNullOrEmpty(txtEffectiveDate.Text) Then
                If Not (txtExpiryDate.Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper()) _
                And txtExpiryDate.Text.Trim.Length <> 0 Then
                    If IsDate(txtExpiryDate.Text.Trim) = False AndAlso (txtEffectiveDate.Text.Trim.Length <> 0 And IsDate(txtEffectiveDate.Text.Trim) = False) Then
                        args.IsValid = False
                        CustvldExpiryDate.ErrorMessage = GetLocalResourceObject("lbl_InvalidExpiryDateFormat")
                    ElseIf CType(txtExpiryDate.Text, Date) < CType(txtEffectiveDate.Text, Date) Then
                        args.IsValid = False
                        CustvldExpiryDate.ErrorMessage = GetLocalResourceObject("lbl_ValidExpriryDate")
                    Else
                        args.IsValid = True
                    End If
                    Exit Sub
                End If

            End If
        End Sub

        Protected Sub custvldReinstatementDate_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles custvldReinstatementDate.ServerValidate
            If String.IsNullOrEmpty(txtReinstatementDate.Text) = True Or txtReinstatementDate.Text.Trim = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper() Then
                args.IsValid = False
                custvldReinstatementDate.ErrorMessage = GetLocalResourceObject("lbl_MTAReinstatementMandatoryDate")
                'Reinstatement Date is Mandatory
            ElseIf Not String.IsNullOrEmpty(txtReinstatementDate.Text) AndAlso IsDate(txtReinstatementDate.Text) Then
                If CDate(txtReinstatementDate.Text) < CDate(oQuote.CoverStartDate) Or CDate(txtReinstatementDate.Text) > CDate(oQuote.CoverEndDate) Then
                    args.IsValid = False
                    custvldReinstatementDate.ErrorMessage = GetLocalResourceObject("lbl_MTAReinstatementMessage")
                    'Reinstatement Date can not be prior to the cancellation date and can not be after cover end date
                Else
                    args.IsValid = True
                End If
            End If
        End Sub

        ''' <summary>
        ''' If policy is under renewal and renewal status is "Awaiting Manual Renewal" 
        ''' then this method should be called to create a WM Task
        ''' </summary>
        ''' <param name="sUserGroup">User Group for task</param>
        ''' <param name="sTaskGroup">Task froup for task</param>
        ''' <remarks></remarks>

        Private Sub CreateWMTaskForBackdatedMTA(ByVal sUserGroup As String, ByVal sTaskGroup As String)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oWorkManagerCollection As New NexusProvider.WorkManagerCollection
            Dim oWorkManager As New NexusProvider.WorkManager

            oWorkManager.DueDate = DateTime.Now()

            If Session(CNParty) IsNot Nothing Then
                Dim oParty As NexusProvider.BaseParty = Session(CNParty)
                If TypeOf oParty Is NexusProvider.PersonalParty Then
                    Dim oPersonalParty As NexusProvider.PersonalParty
                    oPersonalParty = CType(oParty, NexusProvider.PersonalParty)
                    oWorkManager.Client = oPersonalParty.Title & " " & oPersonalParty.Forename & " " & oPersonalParty.Forename
                ElseIf TypeOf oParty Is NexusProvider.CorporateParty Then
                    Dim oCorporateParty As NexusProvider.CorporateParty
                    oCorporateParty = CType(oParty, NexusProvider.CorporateParty)
                    oWorkManager.Client = oCorporateParty.CompanyName
                End If
            End If

            oWorkManager.Task = "MEMO"
            oWorkManager.TaskGroup = sTaskGroup
            oWorkManager.AllocationUser = Session(CNLoginName)
            oWorkManager.AllocationUserGroup = sUserGroup
            oWorkManager.Description = GetLocalResourceObject("lbl_Msg_RenewalTask")

            If Session(CNMTAType) IsNot Nothing Then
                Dim oWmrk As New NexusProvider.KeyData
                oWmrk.KeyName = "MtaType"
                oWmrk.KeyValue = Session(CNMTAType)
                oWorkManager.KeyData.Add(oWmrk)
            Else
                Dim oWmrk As New NexusProvider.KeyData
                oWmrk.KeyName = "MtaType"
                oWmrk.KeyValue = "None"
                oWorkManager.KeyData.Add(oWmrk)
            End If
            If Session(CNQuote) IsNot Nothing Then
                Dim oWmrk As New NexusProvider.KeyData
                oWmrk.KeyName = "InsuranceFileKey"
                oWmrk.KeyValue = CType(Session(CNQuote), NexusProvider.Quote).InsuranceFileKey
                oWorkManager.KeyData.Add(oWmrk)
            End If
            If Session(CNParty) IsNot Nothing Then
                Dim oWmrk As New NexusProvider.KeyData
                oWmrk.KeyName = "PartyKey"
                oWmrk.KeyValue = CType(Session(CNParty), NexusProvider.BaseParty).Key
                oWorkManager.KeyData.Add(oWmrk)
            End If
            If oWorkManager.TaskGroup IsNot Nothing Then
                oWebService.CreateWmTask(oWorkManager)
            End If
        End Sub
        ''' <summary>
        ''' If policy is under renewal and renewal status is "Awaiting Manual Renewal" 
        ''' then this method should be called to create a event
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub CreateEventForBackdatedMTA()
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oEventDetails As New NexusProvider.EventDetails
            If Session(CNParty) IsNot Nothing Then
                Dim oParty As NexusProvider.BaseParty = Session(CNParty)
                With oEventDetails
                    .EventDate = Now()
                    .PartyKey = oParty.Key
                    .RtfText = GetLocalResourceObject("lbl_Msg_RenewalEvent")
                    .UserName = Session(CNLoginName)
                    .EventTypeKey = 20 'Notes-Client
                    .EventLogSubjectKey = 1 'Default
                End With

                oWebService.AddEvent(oEventDetails)
            End If
        End Sub

        ''' <summary>
        ''' This is used to validate the Cancellation Process if policy have any live plan and System option "CancelPlanOnPolcyCancellation" is ticked off.
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="args"></param>
        ''' <remarks></remarks>
        Protected Sub custvldPolicyCancellationOnLivePlan_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles custvldPolicyCancellationOnLivePlan.ServerValidate
            Dim sSelectedPaymentOption As String = Request("rdoMTAReasonList")
            Dim oEnablPolicyValidationOnActiveplan As NexusProvider.OptionTypeSetting
            oWebService = New NexusProvider.ProviderManager().Provider
            oEnablPolicyValidationOnActiveplan = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5076)
            If sSelectedPaymentOption IsNot Nothing Then
                If CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).
                              MTAReasonForCancellation.ToUpper.Contains(sSelectedPaymentOption.ToUpper) _
                              AndAlso oQuote.ActivePlans > 0 _
                              AndAlso oEnablPolicyValidationOnActiveplan.OptionValue <> "1" Then
                    args.IsValid = False
                    Dim oFinancePlanDetail As New NexusProvider.FinancePlan
                    oFinancePlanDetail = oWebService.GetFinancePlanDetails(oQuote.InsuranceFileKey, nPremiumFinanceCnt:=oQuote.DefaultInstalmentPlan, nPremiumFinanceVersion:=oQuote.DefaultInstalmentPlanVersion)

                    If oFinancePlanDetail IsNot Nothing AndAlso
                        (oFinancePlanDetail.Status.Trim() = "140" OrElse oFinancePlanDetail.StatusDescription.Trim().ToUpper() = "ONHOLD") Then
                        custvldPolicyCancellationOnLivePlan.ErrorMessage = GetLocalResourceObject("msg_PolicyCancellationValidationOnLivePlan").Replace("""Live""", """On Hold""")
                    End If
                Else
                    args.IsValid = True
                End If
            End If
        End Sub


    End Class
End Namespace


