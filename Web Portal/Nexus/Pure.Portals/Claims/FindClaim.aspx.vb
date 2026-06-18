Imports System
Imports System.Globalization
Imports System.Threading
Imports NexusProvider.SAMForInsurance
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Utils
Imports CMS.Library
Imports Nexus.Library
Imports System.Web.Configuration.WebConfigurationManager

Namespace Nexus

    Partial Class Framework_FindClaim
        Inherits CMS.Library.Frontend.clsCMSPage
        Dim bDisplayCaseOption As Boolean = False
        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())

        Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oOptionSettings As NexusProvider.OptionTypeSetting

            'If System Option for "Enhanced Case Search" is ON then we need to visible case related search criteria and grid column
            oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5099)
            If oOptionSettings IsNot Nothing AndAlso oOptionSettings.OptionValue IsNot Nothing Then
                If oOptionSettings.OptionValue <> "0" Then
                    liCaseNumber.Visible = True
                    bDisplayCaseOption = True
                    vldWildCard.ControlsToValidate = "txtClaimReference,txtPolicy,txtClient,txtCaseNumber,TxtDescription,txtRiskIndex"
                Else
                    liCaseNumber.Visible = False
                    bDisplayCaseOption = False
                    vldWildCard.ControlsToValidate = "txtClaimReference,txtPolicy,txtClient,TxtDescription,txtRiskIndex"
                End If
            Else
                liCaseNumber.Visible = False
                bDisplayCaseOption = False
            End If

        End Sub

        ''' <summary>
        ''' This is fired on Page PreInit
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            If Request.QueryString("FindClaim") = "1" Or Request.QueryString("Page") = "AP" Or Request.QueryString("Page") = "CC" Then
                CMS.Library.Frontend.Functions.SetTheme(Page, ConfigurationManager.AppSettings("ModalPageTemplate"))
            End If
        End Sub

        ''' <summary>
        ''' This event is fired on Page Load
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'if user is trying to access this page directly
            If Session(CNLoginType) Is Nothing Then
                Response.Redirect("~/login.aspx", False)
            End If

            'Setting the max and min values in range validator
            rngLossDateEndLimit.MaximumValue = DateTime.MaxValue.ToShortDateString
            rngLossDateStartLimit.MaximumValue = DateTime.MaxValue.ToShortDateString
            GetSystemOption()
            'Setting of dynamic javascript
            'To set the Focus
            Page.SetFocus(txtClaimReference)
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            'if user move to this page during claim edit process
            If Request.QueryString("ReDirectPage") IsNot Nothing Then
                Dim oClaimVersions As NexusProvider.VersionsCollections = Session(CNClaimVersion)
                If oClaimVersions IsNot Nothing Then

                    Dim oClaims As NexusProvider.ClaimCollection
                    Dim oClaimSearchCriteria As New NexusProvider.ClaimSearchCriteria
                    oClaimSearchCriteria.ClaimNumber = Session(CNClaimNumber)
                    txtClaimReference.Text = Session(CNClaimNumber)

                    ClearQuote()
                    ClearClaims()

                    FindClaims()
                    ClearTemporarySessionValues()
                End If
                Dim oLoggedInUser As NexusProvider.UserDetails = Session(CNAgentDetails)
                If oLoggedInUser IsNot Nothing AndAlso oLoggedInUser.Key <> 0 Then
                    txtClaimReference.Enabled = False
                    hvMakeClaimNumberReadOnly.Value = 1
                End If
            Else
                ClearQuote()
                ClearClaims()
            End If

            Dim oUserDetails As NexusProvider.UserDetails
            'During Event List
            If Not IsPostBack Then
                If Request.QueryString("Page") = "EL" Then
                    txtPolicy.Text = Session(CNPolicyNumber)
                End If
                If Request.QueryString("Page") = "CEL" AndAlso Request.QueryString("CaseNumber") <> "" Then
                    txtCaseNumber.Text = Request.QueryString("CaseNumber")
                    txtCaseNumber.Enabled = False
                    btnCaseNumber.Enabled = False
                End If

                If Request.QueryString("Claimno") IsNot Nothing Then
                    txtClaimReference.Text = Request.QueryString("Claimno")
                    FindClaims()
                End If

                If Request.QueryString("Policyno") IsNot Nothing Then
                    txtPolicy.Text = Request.QueryString("Policyno")
                    If Request.QueryString("LossStartDate") IsNot Nothing Then
                        Claims__LossDateStartLimit.Text = Request.QueryString("LossStartDate").ToString().Replace("%2f", "/")
                    End If
                    If Request.QueryString("LossEndDate") IsNot Nothing Then
                        Claims__LossDateEndLimit.Text = Request.QueryString("LossEndDate").ToString().Replace("%2f", "/")
                    End If
                    FindClaims()
                End If

            End If

            oUserDetails = oWebservice.GetUserDetails(HttpContext.Current.User.Identity.Name)
            'check if TPA is attached with the logged in user.
            'If so then the field will be prepopulated with the TPA code and will be read only
            'update the find party control properties as well
            If oUserDetails.PartyKey <> 0 Then
                If Trim(oUserDetails.PartyType) = OtherPartySearch(PartyName.Type) Then
                    hdnAttachedPartyName.Value = Trim(oUserDetails.PartyCode)
                    PartyName.PartyCode = Trim(oUserDetails.PartyCode)
                    PartyName.PartyKey = oUserDetails.PartyKey
                Else
                    hdnAttachedPartyName.Value = ""
                    PartyName.PartyCode = ""
                    PartyName.PartyKey = 0
                End If
            End If


            ' if request came from quick search control and dosearch is true
            If Not IsPostBack AndAlso Request.QueryString("dosearch") IsNot Nothing Then
                If CType(Request.QueryString("dosearch"), Boolean) = True Then

                    Dim oMaster As ContentPlaceHolder
                    Dim txtControl As TextBox
                    Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)

                    oMaster = GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName)
                    'Assign values to controls from query string. these values will be used to search the claims
                    For iCt As Integer = 0 To Request.QueryString.AllKeys.Length - 3
                        txtControl = CType(oMaster.FindControl(Request.QueryString.GetKey(iCt)), TextBox)
                        If txtControl IsNot Nothing Then
                            txtControl.Text = Request.QueryString(iCt)
                        End If
                    Next
                    'find the claims for given filter
                    FindClaims()
                End If
            End If
            If Session(CNClaimBackButton) IsNot Nothing AndAlso Session(CNClaimBackButton) = "BackButton" Then
                FindClaims()
            End If
        End Sub

        ''' <summary>
        ''' This event is Fired on Find Now Button Click
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnFindNow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindNow.Click

            If Page.IsValid Then
                FindClaims()
            Else
                grdvSearchResults.DataSource = Nothing
                grdvSearchResults.DataBind()
            End If
        End Sub

        Private Sub FindClaims()

            'Cleaning of the session values
            ClearHeader()
            ClearQuote()
            ClearClaims()
            ClearCase()
            ClearSearch()

            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oClaimSearchCriteria As New NexusProvider.ClaimSearchCriteria
            Dim oClaims As NexusProvider.ClaimCollection

            Dim htCriteria As New Hashtable()

            Dim sInsuranceFileRef As String = UCase(txtPolicy.Text)
            Dim dLossStartDate As Date = IIf(String.IsNullOrEmpty(Claims__LossDateStartLimit.Text), Nothing, Claims__LossDateStartLimit.Text)
            Dim dLossEndDate As Date = IIf(String.IsNullOrEmpty(Claims__LossDateEndLimit.Text), Nothing, Claims__LossDateEndLimit.Text)

            Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
            Dim sBranchCode As String = oUserDetails.ListOfBranches(0).Code
            Dim sURL As String

            If Session(CNClaimBackButton) IsNot Nothing AndAlso Session(CNClaimBackButton) = "BackButton" Then
                Dim formFilds As NexusProvider.ClaimSearchCriteria
                If Session(CNFindClaimSearchData) IsNot Nothing Then
                    formFilds = CType(Session(CNFindClaimSearchData), NexusProvider.ClaimSearchCriteria)

                    Dim lossStartDate As Date = IIf(String.IsNullOrEmpty(formFilds.LossDateFrom), "", formFilds.LossDateFrom)
                    Dim lossEndDate As Date = IIf(String.IsNullOrEmpty(formFilds.LossDateTo), "", formFilds.LossDateTo)
                    'Initializing the values
                    oClaimSearchCriteria.ClaimNumber = UCase(Trim(formFilds.ClaimNumber))
                    oClaimSearchCriteria.InsuranceFileRef = Trim(sInsuranceFileRef)
                    oClaimSearchCriteria.IncludeClosedClaim = formFilds.IncludeClosedClaim
                    oClaimSearchCriteria.LossDateFrom = lossStartDate
                    oClaimSearchCriteria.LossDateTo = lossEndDate
                    oClaimSearchCriteria.ClientShortName = Trim(formFilds.ClientShortName)
                    oClaimSearchCriteria.RiskIndex = Trim(formFilds.RiskIndex)

                    Dim LossDateTo As String = formFilds.LossDateTo
                    LossDateTo = IIf(LossDateTo = "00:00:00", "", LossDateTo)
                    Dim LossDateFrom As String = formFilds.LossDateFrom
                    LossDateFrom = IIf(LossDateFrom = "00:00:00", "", LossDateFrom)

                    txtClaimReference.Text = formFilds.ClaimNumber
                    Claims__LossDateStartLimit.Text = LossDateFrom
                    Claims__LossDateEndLimit.Text = LossDateTo
                    txtClient.Text = formFilds.ClientShortName
                    chkIncludeClosedClaims.Checked = formFilds.IncludeClosedClaim
                    txtRiskIndex.Text = formFilds.RiskIndex
                    txtCaseNumber.Text = formFilds.CaseNumber
                    TxtDescription.Text = formFilds.Description
                    'Implimented wildcard search
                    If txtCaseNumber.Text.Length <> 0 Then
                        oClaimSearchCriteria.CaseNumber = formFilds.CaseNumber.Trim
                    Else
                        oClaimSearchCriteria.CaseNumber = Nothing
                    End If
                    'TPA code is passed so that search will show only the claims associated with the TPA.
                    If PartyName.PartyCode <> "" Then
                        oClaimSearchCriteria.TPACode = Trim(PartyName.PartyCode)
                    End If
                    'to limit the search return from SAM
                    oClaimSearchCriteria.MaxRowsToFetch = oPortal.MaxSearchResults
                    oClaimSearchCriteria.Description = formFilds.Description.Trim()

                    'Sam Call with serch criteria
                    oClaims = oWebservice.FindClaim(formFilds, sBranchCode)
                    Session.Remove(CNFindClaimSearchData)
                End If
            Else

                'Initializing the values
                oClaimSearchCriteria.ClaimNumber = UCase(Trim(txtClaimReference.Text))
                oClaimSearchCriteria.InsuranceFileRef = Trim(sInsuranceFileRef)
                oClaimSearchCriteria.IncludeClosedClaim = chkIncludeClosedClaims.Checked
                oClaimSearchCriteria.LossDateFrom = dLossStartDate
                oClaimSearchCriteria.LossDateTo = dLossEndDate
                oClaimSearchCriteria.ClientShortName = Trim(txtClient.Text)
                oClaimSearchCriteria.RiskIndex = Trim(txtRiskIndex.Text)
                'Implimented wildcard search
                If txtCaseNumber.Text.Length <> 0 Then
                    oClaimSearchCriteria.CaseNumber = txtCaseNumber.Text.Trim
                Else
                    oClaimSearchCriteria.CaseNumber = Nothing
                End If
                'TPA code is passed so that search will show only the claims associated with the TPA.
                If PartyName.PartyCode <> "" Then
                    oClaimSearchCriteria.TPACode = Trim(PartyName.PartyCode)
                End If
                'to limit the search return from SAM
                oClaimSearchCriteria.MaxRowsToFetch = oPortal.MaxSearchResults
                oClaimSearchCriteria.Description = TxtDescription.Text.Trim()

                'Sam Call with serch criteria
                oClaims = oWebservice.FindClaim(oClaimSearchCriteria, sBranchCode)
            End If

            'Populating the session with search results
            Session(CNFindClaimSearchData) = oClaimSearchCriteria
            Session(CNClaimBackButton) = "ClaimView"

            Session(CNClaimsSearchData) = oClaims
            grdvSearchResults.Visible = True
            grdvSearchResults.AllowPaging = True
            grdvSearchResults.PageIndex = 0
            grdvSearchResults.DataSource = oClaims
            grdvSearchResults.DataBind()

            'validate size of dataset. if 500(configured at portal level) or more results are returned then add a validation message to the screen
            If oClaims.Count >= oPortal.MaxSearchResults Then
                'create a custom validator
                Dim cstMaxResults As New CustomValidator
                cstMaxResults.IsValid = False
                'look for a validation message in the page resources, but if there is not one defined add a default message
                cstMaxResults.ErrorMessage = IIf(GetLocalResourceObject("cstMaxResults") Is Nothing, "Maximum number of search results exceeded, please refine your search criteria", GetLocalResourceObject("cstMaxResults"))
                cstMaxResults.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                'add the validator to the page, this will have the effect of making the page invalid
                Page.Validators.Add(cstMaxResults)
            End If

        End Sub
        ''' <summary>
        ''' This event is Fired on New Search Button Click
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnNewSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewSearch.Click
            Session.Remove(CNClaimsSearchData)
            Session.Remove(CNClaimBackButton)
            txtClaimReference.Text = String.Empty
            Claims__LossDateStartLimit.Text = String.Empty
            txtPolicy.Text = String.Empty
            Claims__LossDateEndLimit.Text = String.Empty
            txtClient.Text = String.Empty
            chkIncludeClosedClaims.Checked = False
            txtRiskIndex.Text = String.Empty
            txtCaseNumber.Text = String.Empty
            TxtDescription.Text = String.Empty
            Dim txtPartyName As TextBox = Me.PartyName.FindControl("txtPartyName")
            txtPartyName.Text = String.Empty
            grdvSearchResults.Visible = False
            grdvSearchResults.DataSource = Nothing
            grdvSearchResults.DataBind()
        End Sub

        Protected Sub grdvSearchResults_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvSearchResults.DataBound
            If grdvSearchResults.Rows.Count = 0 Or grdvSearchResults.PageCount = 1 Then
                grdvSearchResults.AllowPaging = False
            End If
            Dim Portal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
            grdvSearchResults.Columns(5).Visible = Portal.Claims.ShowClaimStatusOnFindGrid
            If bDisplayCaseOption Then
                grdvSearchResults.Columns(0).Visible = True
            Else
                grdvSearchResults.Columns(0).Visible = False
            End If
        End Sub

        ''' <summary>
        ''' This is fired on Page Index Change of the Grid View
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvSearchResults_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvSearchResults.PageIndexChanging
            grdvSearchResults.PageIndex = e.NewPageIndex
            grdvSearchResults.DataSource = CType(Session(CNClaimsSearchData), NexusProvider.ClaimCollection)
            grdvSearchResults.DataBind()
        End Sub

        ''' <summary>
        ''' This is fired on Row Command of the Grid View
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvSearchResults_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdvSearchResults.RowCommand

            Dim bExclusiveLock As Boolean = True

            Dim oClaims1 As NexusProvider.ClaimCollection
            oClaims1 = CType(Session(CNClaimsSearchData), NexusProvider.ClaimCollection)
            Dim iRowIndex As Integer
            Dim iTemp As Integer
            Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
            Dim sBranchCode As String = oUserDetails.ListOfBranches(0).Code

            For iTemp = 0 To grdvSearchResults.Rows.Count - 1
                If e.CommandArgument() = oClaims1(iTemp).ClaimNumber Then
                    iRowIndex = iTemp
                    Exit For
                End If
            Next
            If iRowIndex > -1 Then
                If (oClaims1(iRowIndex).IsDeleted = True) Then
                    If (oClaims1(iRowIndex).IsAllowedClosedClaims = False) Then
                        ScriptManager.RegisterStartupScript(Me, Page.GetType(), "alert", "alert('Claims cannot be opened against this policy because it is associated to a closed branch.');", True)
                        Exit Sub
                    End If
                End If
            End If

            Select Case e.CommandName
                Case "Select"
                    If Request.QueryString("Page") = "EL" Or Request.QueryString("Page") = "CC" Or Request.QueryString("Page") = "CEL" Then
                        Dim oClaims As NexusProvider.ClaimCollection
                        oClaims = CType(Session(CNClaimsSearchData), NexusProvider.ClaimCollection)

                        For iTempVar As Integer = 0 To oClaims.Count - 1
                            If e.CommandArgument = oClaims(iTempVar).ClaimNumber Then
                                Dim iClaimKey As Integer = oClaims(iTempVar).ClaimKey
                                Dim iFileKey As Integer = oClaims(iTempVar).InsuranceFileKey
                                Dim PolicyRef As String = Session(CNPolicyNumber)
                                If PolicyRef IsNot Nothing Then
                                    If txtPolicy.Text.Length <> 0 And PolicyRef.Trim.ToUpper <> oClaims(iTempVar).InsuranceRef.Trim.ToUpper Then
                                        Session(CNPolicyNumber) = Nothing
                                    ElseIf txtPolicy.Text.Length = 0 Then
                                        Session(CNPolicyNumber) = Nothing
                                    End If
                                End If

                                If Request.QueryString("Page") = "EL" Or Request.QueryString("Page") = "CC" Then
                                    'Sending back the data to the parent screen
                                    ScriptManager.RegisterStartupScript(Me, Page.GetType(), "closeThickBox", "self.parent.setClaimReference('" + e.CommandArgument.ToString + "','" + iClaimKey.ToString + "');", True)
                                ElseIf Request.QueryString("Page") = "CEL" Then
                                    'Sending back the data to the parent screen
                                    ScriptManager.RegisterStartupScript(Me, Page.GetType(), "closeThickBox", "self.parent.setClaimReference('" + e.CommandArgument.ToString + "','" + iClaimKey.ToString + "','CC');", True)
                                End If

                                'Sending back the data to the parent screen
                                ScriptManager.RegisterStartupScript(Me, Page.GetType(), "closeThickBox", "self.parent.setClaimReference('" + e.CommandArgument.ToString() + "','" + iClaimKey.ToString + "','" + iFileKey.ToString + "');", True)
                                'Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.setClaimReference('" + e.CommandArgument.ToString() + "','" + iClaimKey.ToString + "','" + iFileKey.ToString + "');", True)
                                Exit For
                            End If
                        Next
                    Else
                        'Sending back the data to the parent screen
                        ' Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.setClaimReference('" & e.CommandArgument & "');", True)
                        ScriptManager.RegisterStartupScript(Me, GetType(String), "closeThickBox", "self.parent.setClaimReference('" & e.CommandArgument & "');", True)
                    End If


                Case "EditClaim", "View", "Pay", "Salvage", "TPRecovery"
                    Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    If e.CommandName = "EditClaim" Then
                        Session(CNMode) = Mode.EditClaim
                    ElseIf e.CommandName = "Pay" Then
                        Session(CNMode) = Mode.PayClaim
                    ElseIf e.CommandName = "Salvage" Then
                        Session(CNMode) = Mode.SalvageClaim
                    ElseIf e.CommandName = "TPRecovery" Then
                        Session(CNMode) = Mode.TPRecovery
                    Else
                        bExclusiveLock = False
                        Session(CNMode) = Mode.ViewClaim
                    End If

                    ClearClaims()
                    'Find Latest Claim Version
                    If Not String.IsNullOrEmpty(e.CommandArgument) Then
                        Dim oClaimVersions As NexusProvider.VersionsCollections = Nothing
                        Dim oQuote As NexusProvider.Quote = Nothing
                        Dim oBaseParty As NexusProvider.BaseParty = Nothing
                        Dim sClaimNumber As String = CStr(e.CommandArgument)
                        Dim iHighest As Integer = 0
                        Dim oOpenClaim As New NexusProvider.ClaimOpen
                        Dim oClaimDetails As NexusProvider.ClaimDetails = Nothing
                        Dim oCashListItem As NexusProvider.CashListItemsCollection = Nothing
                        Dim oClaimRisk As NexusProvider.ClaimRisk = Nothing
                        Dim bUnlockRequired As Boolean = False
                        Dim sInsuranceFileTypeCode As String = String.Empty

                        Try
                            If Session(CNMode) = Mode.EditClaim OrElse Session(CNMode) = Mode.SalvageClaim OrElse Session(CNMode) = Mode.TPRecovery OrElse Session(CNMode) = Mode.PayClaim Then
                                'Check if portfolio/clone transfer is pending on the policy associated with claim
                                Dim lbtn As LinkButton = CType(e.CommandSource, LinkButton)
                                Dim gvwCurrentRow As GridViewRow = CType(lbtn.NamingContainer, GridViewRow)
                                Dim iInsuranceFilKey As Integer = Convert.ToInt32(grdvSearchResults.DataKeys(gvwCurrentRow.RowIndex).Values("InsuranceFileKey"))
                                Dim bIsPendingPortfolioTransfer, bIsPendingCloneTransfer As Boolean

                                oWebservice.IsPendingTransfer(iInsuranceFilKey, bIsPendingCloneTransfer, bIsPendingPortfolioTransfer, Nothing)
                                Dim sMessage As String = ""
                                If bIsPendingCloneTransfer OrElse bIsPendingPortfolioTransfer Then
                                    If bIsPendingPortfolioTransfer Then
                                        sMessage = Convert.ToString(GetLocalResourceObject("msg_PendingPortfolioTransfer"))
                                    ElseIf bIsPendingCloneTransfer Then
                                        sMessage = Convert.ToString(GetLocalResourceObject("msg_PendingClonedTransfer"))
                                    End If
                                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "PendingPortfolioTransfer", "alert('" + sMessage + "')", True)
                                    Exit Sub
                                End If
                            End If


                            oClaimVersions = oWebservice.GetVersionsForClaim(sClaimNumber)
                            If oClaimVersions IsNot Nothing AndAlso oClaimVersions.Count > 0 Then
                                'Check if Policy Locked
                                If DirectCast(oClaimVersions(0), NexusProvider.Versions).IsPreviouslyLocked AndAlso Session(CNMode) <> Mode.ViewClaim Then
                                    lbl_policyLock.Text = "Policy Is locked"
                                    Exit Sub
                                End If

                                'Find Highest Version
                                For iCount As Integer = 0 To oClaimVersions.Count - 1
                                    If oClaimVersions(iCount).Version > iHighest Then
                                        iHighest = oClaimVersions(iCount).Version
                                    End If
                                Next

                                'Unlock claim of same user
                                If e.CommandName = "Edit" OrElse e.CommandName = "Pay" OrElse e.CommandName = "Salvage" OrElse e.CommandName = "TPRecovery" Then
                                    For iCount As Integer = 0 To oClaimVersions.Count - 1
                                        UnlockClaim(oClaimVersions(iCount).ClaimKey)
                                    Next
                                End If

                                'Updating of claim quote oQuote
                                oQuote = oWebservice.GetHeaderAndSummariesByKey(oClaimVersions(0).InsuranceFileKey, , True)
                                If oQuote IsNot Nothing Then
                                    oBaseParty = oWebservice.GetParty(oQuote.PartyKey)
                                    Session.Item(CNParty) = oBaseParty
                                    Session.Item(CNRisks) = oQuote.Risks
                                    Session.Item(CNRenewalDate) = oQuote.RenewalDate
                                    Session.Item(CNAddress) = oBaseParty.Addresses(0).Address1 & ", " & oBaseParty.Addresses(0).Address4
                                    Session.Item(CNDate_Header) = oQuote.CoverStartDate.ToShortDateString & " - " & oQuote.CoverEndDate.ToShortDateString
                                    Session(CNInsurer_Header) = oQuote.InsuredName
                                    Session(CNProductCode) = oQuote.ProductCode
                                    Session("ProductCode") = oQuote.ProductCode
                                    Session(CNClaimQuote) = oQuote
                                    If oQuote.InsuranceFileTypeCode IsNot Nothing Then
                                        sInsuranceFileTypeCode = oQuote.InsuranceFileTypeCode.Trim.ToUpper()
                                    End If
                                End If
                                Session(CNClaimVersion) = oClaimVersions
                                Session.Item(CNInsuranceFileKey) = oClaimVersions(0).InsuranceFileKey
                                Session.Item(CNPolicyNumber) = oClaimVersions(0).InsuranceRef
                                'Response.Redirect("FindClaim.aspx")
                            End If
                            For iCount As Integer = 0 To oClaimVersions.Count - 1
                                If oClaimVersions(iCount).Version = iHighest Then

                                    ' Begin - WPR VB 64 - Media Type Status 

                                    If e.CommandName = "Pay" Then
                                        If sInsuranceFileTypeCode = "WRITTEN" Then
                                            cvWrittenTypePolicy.Enabled = True
                                            cvWrittenTypePolicy.IsValid = False
                                            Exit Sub
                                        Else
                                            Dim CheckMediatypeStatusAtPolicyRefund As String = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance,
                                                                   NexusProvider.ProductRiskOptions.CheckMediatypeStatusAtClaimPayment, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing, oQuote.BranchCode).Trim()

                                            If CheckMediatypeStatusAtPolicyRefund.Contains("1") Then
                                                Dim oMediaTypeStatus As New NexusProvider.MediaTypeStatus
                                                With oMediaTypeStatus
                                                    .InsuranceFileKey = oQuote.InsuranceFileKey
                                                    .LossDateSpecified = False
                                                End With
                                                oWebservice.GetPolicyStatusForMediaTypeStatus(oMediaTypeStatus)
                                                'SAM Return the False intead of True, if unclear fund exist then it retirn False or else true
                                                If Not oMediaTypeStatus.IsUnclearedCashListExists Then
                                                    vldMediaTypeStatus.IsValid = False
                                                    bUnlockRequired = True
                                                    Exit Sub
                                                End If
                                            End If
                                        End If
                                    End If
                                    ' End - WPR VB 64 - Media Type Status 

                                    If e.CommandName = "Pay" Then
                                        Dim sMultipleClaimsPayments As String
                                        Dim dMaxUnauthorisedClaimValue As Double
                                        Dim iMaxUnauthorisedNoClaimPayments As Integer
                                        Dim CashListItem As New NexusProvider.CashListItems
                                        Dim iCountOfClaims As Integer
                                        'get the Product Risk option setting named Multiple Claims Payments(MultipleClaimsPayments)
                                        sMultipleClaimsPayments = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.MultipleClaimsPayments, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
                                        'get the Product Risk option setting named max unauthorised claim Value(MaxUnauthorisedClaimValue)
                                        dMaxUnauthorisedClaimValue = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.MaxUnauthorisedClaimValue, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
                                        'get the Product Risk option setting named max unauthorised number of claim payamnt(MaxUnauthorisedNoClaimPayments)
                                        iMaxUnauthorisedNoClaimPayments = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.MaxUnauthorisedNoClaimPayments, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)

                                        With CashListItem
                                            .ClaimNumber = oClaimVersions(iCount).ClaimNumber
                                        End With
                                        'get the Payment Referred details
                                        oCashListItem = oWebservice.GetReferredPayments(CashListItem)

                                        For Each oCashList As NexusProvider.CashListItems In oCashListItem
                                            If sMultipleClaimsPayments = "1" Then
                                                If oClaimVersions(iCount).ClaimNumber = oCashList.ClaimNumber Then
                                                    ' Count the Number of oCashList and add it up in iCountOfClaims and check the same against MaxUnauthorisedNoClaimPayments
                                                    iCountOfClaims = iCountOfClaims + 1
                                                    If iMaxUnauthorisedNoClaimPayments <= iCountOfClaims Or dMaxUnauthorisedClaimValue <= oCashList.PaymentAmount Then
                                                        cvAllowMultipleClaimPayment.IsValid = False
                                                        Exit Sub
                                                    End If
                                                End If
                                            Else
                                                If oClaimVersions(iCount).ClaimNumber = oCashList.ClaimNumber Then
                                                    AllowClaimPayment.IsValid = False
                                                    Exit Sub
                                                End If
                                            End If
                                        Next
                                    End If
                                    'To Check whether Payment is pending for Authorization
                                    If e.CommandName = "Salvage" OrElse e.CommandName = "TPRecovery" Then

                                        Dim CashListItem As New NexusProvider.CashListItems
                                        With CashListItem
                                            .ClaimNumber = oClaimVersions(iCount).ClaimNumber
                                        End With
                                        oCashListItem = oWebservice.GetReferredPayments(CashListItem)
                                        If oCashListItem IsNot Nothing AndAlso oCashListItem.Count > 0 Then
                                            Dim sMessage As String = "alert('" + Replace(GetLocalResourceObject("msg_PendingReferredPayments"), "(COUNT)", oCashListItem.Count().ToString()) + "')"
                                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "claimlocked", sMessage, True)
                                            Exit Sub
                                        End If
                                        For Each oCashList As NexusProvider.CashListItems In oCashListItem
                                            If oClaimVersions(iCount).ClaimNumber = oCashList.ClaimNumber Then
                                                AllowClaimPayment.IsValid = False
                                                bUnlockRequired = True
                                                Session(CNMode) = Nothing
                                                Session(CNClaimNumber) = sClaimNumber
                                                Exit Sub
                                            End If
                                        Next
                                    End If
                                    'Retreival of claim details
                                    'Dim sBranchCode As String = oQuote.BranchCode
                                    Try
                                        'This is expected to through an error if claim is locked in BO
                                        'oClaimDetails = GetClaimDetailsCall(oClaimVersions(iCount).ClaimKey, sBranchCode)
                                        If e.CommandName = "View" Then
                                            oClaimDetails = oWebservice.GetClaimDetails(v_iClaimKey:=oClaimVersions(iCount).ClaimKey, v_sBranchCode:=sBranchCode, v_iFetchAllVersionAmounts:=1)
                                        Else
                                            oClaimDetails = oWebservice.GetClaimDetails(v_iClaimKey:=oClaimVersions(iCount).ClaimKey, v_sBranchCode:=sBranchCode, bExclusiveLock:=bExclusiveLock)
                                        End If

                                    Catch ex As NexusProvider.NexusException
                                        'Claim locking error
                                        Select Case CType(ex.Errors(0), NexusProvider.NexusError).Code
                                            Case "1000148", "1000158" 'Claim is locked for modification as already in use
                                                'Show Claim locking error as alert
                                                Dim sMessage As String = "alert('" + Replace(GetLocalResourceObject("lbl_ClaimLock_error"), "{1}", (ex.Errors(0).Detail.Split(":"))(2) + ".") + "')"
                                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "claimlocked", sMessage, True)
                                                bUnlockRequired = False
                                                Server.ClearError()
                                                'Clear all claim related sessions and throw the error
                                                ClearQuote()
                                                ClearClaims()
                                                Exit Sub
                                            Case "200" 'Claim Locking
                                                'Show Claim locking error as alert
                                                Dim sMessage As String = "alert('" + ex.Errors(0).Description + ".\n" + ex.Errors(0).Detail + "')"
                                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "claimlocked", sMessage, True)
                                                Server.ClearError()
                                                'Clear all claim related sessions and throw the error
                                                ClearQuote()
                                                ClearClaims()
                                                bUnlockRequired = True
                                            Case Else
                                                Throw ex
                                        End Select
                                    End Try
                                    'check for closed claim
                                    If e.CommandName = "Pay" AndAlso oClaimDetails IsNot Nothing Then
                                        If Not String.IsNullOrEmpty(oClaimDetails.ClaimStatus) AndAlso oClaimDetails.ClaimStatus.Trim.ToUpper = "CLOSED" Then
                                            ChkClosedClaim.IsValid = False
                                            ChkClosedClaim.Display = ValidatorDisplay.Dynamic
                                            bUnlockRequired = True
                                            Exit Sub
                                        End If
                                    End If

                                    'Updation of session with claim details
                                    With oClaimDetails
                                        oOpenClaim.BaseCaseKey = .BaseCaseKey
                                        oOpenClaim.CatastropheCode = .CatastropheCode
                                        oOpenClaim.BaseClaimKey = .BaseClaimKey
                                        oOpenClaim.Claim = .Claim
                                        oOpenClaim.ClaimCoInsurer = .ClaimCoInsurer
                                        oOpenClaim.ClaimDescription = .ClaimDescription
                                        oOpenClaim.ClaimHandlerDescription = .ClaimHandlerDescription
                                        oOpenClaim.ClaimKey = .ClaimKey
                                        oOpenClaim.ClaimNumber = .ClaimNumber
                                        oOpenClaim.ClaimPeril = .ClaimPeril
                                        oOpenClaim.ClaimStatus = .ClaimStatus
                                        oOpenClaim.ClaimStatusDate = .ClaimStatusDate
                                        oOpenClaim.ClaimStatusID = .ClaimStatusID
                                        oOpenClaim.ClaimVersion = .ClaimVersion
                                        oOpenClaim.ClaimVersionDescription = .ClaimVersionDescription
                                        oOpenClaim.ClientClaimNumber = .ClientClaimNumber
                                        oOpenClaim.CoinsuranceTreatmentCode = .CoinsuranceTreatmentCode
                                        If Not Session(CNMode) = Mode.ViewClaim Then
                                            If .Client.Contact.Count > 0 Then
                                                For Each ocontact As NexusProvider.Contact In .Client.Contact
                                                    Select Case ocontact.ContactType
                                                        Case NexusProvider.ContactType.Email
                                                            oOpenClaim.ClientEmail = ocontact.Number
                                                        Case NexusProvider.ContactType.Fax
                                                            oOpenClaim.ClientFaxNo = ocontact.Number
                                                        Case NexusProvider.ContactType.HomePhone
                                                            oOpenClaim.ClientTelNo = ocontact.Number
                                                        Case NexusProvider.ContactType.Main, NexusProvider.ContactType.WorkPhone
                                                            oOpenClaim.ClientTelNoOff = ocontact.Number
                                                        Case NexusProvider.ContactType.Mobile
                                                            oOpenClaim.ClientMobileNo = ocontact.Number
                                                    End Select
                                                Next
                                            Else
                                                oOpenClaim.ClientEmail = .ClientEmail
                                                oOpenClaim.ClientFaxNo = .ClientFaxNo
                                                oOpenClaim.ClientMobileNo = .ClientMobileNo
                                                oOpenClaim.ClientTelNo = .ClientTelNo
                                                oOpenClaim.ClientTelNoOff = .ClientTelNoOff
                                            End If
                                        Else
                                            oOpenClaim.ClientEmail = .ClientEmail
                                            oOpenClaim.ClientFaxNo = .ClientFaxNo
                                            oOpenClaim.ClientMobileNo = .ClientMobileNo
                                            oOpenClaim.ClientTelNo = .ClientTelNo
                                            oOpenClaim.ClientTelNoOff = .ClientTelNoOff
                                        End If

                                        oOpenClaim.ClientName = .ClientName
                                        oOpenClaim.ClientShortName = oClaimVersions(0).ClientShortName 'IIf(.ClientShortName <> String.Empty, .ClientShortName, Trim(lblClientCode.Text))
                                        oOpenClaim.CloseClaimOnZeroReserveRecoveryBalance = .CloseClaimOnZeroReserveRecoveryBalance
                                        oOpenClaim.Comments = .Comments
                                        oOpenClaim.Contact = .Contact
                                        oOpenClaim.CurrencyISOCode = .CurrencyCode
                                        oOpenClaim.Description = .Description
                                        oOpenClaim.ExternalHandler = .ExternalHandler
                                        oOpenClaim.HandlerCode = .HandlerCode
                                        oOpenClaim.IgnoreClaimMaintain = .IgnoreClaimMaintain
                                        oOpenClaim.InfoOnly = .InfoOnly
                                        oOpenClaim.InsuranceFileKey = .InsuranceFileKey
                                        oOpenClaim.InsuranceRef = .InsuranceRef
                                        oOpenClaim.InsurerClaimNumber = .InsurerClaimNumber
                                        oOpenClaim.IsAllowedClosedClaims = .IsAllowedClosedClaims
                                        oOpenClaim.IsDeleted = .IsDeleted
                                        oOpenClaim.LastModifiedDate = .LastModifiedDate
                                        oOpenClaim.LikelyClaim = .LikelyClaim
                                        oOpenClaim.Location = .Location
                                        oOpenClaim.LossDate = .LossDate
                                        oOpenClaim.LossDateFrom = .LossDateFrom
                                        oOpenClaim.LossFromDate = .LossFromDate
                                        oOpenClaim.LossToDate = .LossToDate
                                        oOpenClaim.LossToDateSpecified = .LossToDateSpecified
                                        oOpenClaim.Payments = .Payments
                                        oOpenClaim.PolicyNumber = .PolicyNumber
                                        oOpenClaim.PolicyType = .PolicyType
                                        oOpenClaim.PrimaryCause = .PrimaryCause
                                        oOpenClaim.PrimaryCauseCode = .PrimaryCauseCode
                                        oOpenClaim.PrimaryCauseDescription = .PrimaryCauseDescription
                                        oOpenClaim.ProductDescription = .ProductDescription
                                        oOpenClaim.ProgressStatusCode = .ProgressStatusCode
                                        oOpenClaim.ProgressStatusDescription = .ProgressStatusDescription
                                        oOpenClaim.ReportedDate = .ReportedDate
                                        oOpenClaim.Reserve = .Reserve
                                        oOpenClaim.RiskKey = .RiskKey
                                        oOpenClaim.RiskType = CType(Session(CNClaimQuote), NexusProvider.Quote).Risks.FindItemByRiskKey(.RiskKey).RiskTypeCode
                                        oOpenClaim.RiskTypeDescription = CType(Session(CNClaimQuote), NexusProvider.Quote).Risks.FindItemByRiskKey(.RiskKey).Description
                                        oOpenClaim.SecondaryCause = .SecondaryCause
                                        oOpenClaim.SecondaryCauseCode = .SecondaryCauseCode
                                        oOpenClaim.SecondaryCauseDescription = .SecondaryCauseDescription
                                        oOpenClaim.TotalCurrentShareValue = .TotalCurrentShareValue
                                        oOpenClaim.TotalShare = .TotalShare
                                        oOpenClaim.Town = .Town
                                        oOpenClaim.TownCode = .TownCode
                                        oOpenClaim.UnderwritingYearCode = .UnderwritingYearCode
                                        oOpenClaim.UserDefFldACode = .UserDefFldACode
                                        oOpenClaim.UserDefFldBCode = .UserDefFldBCode
                                        oOpenClaim.UserDefFldCCode = .UserDefFldCCode
                                        oOpenClaim.UserDefFldDCode = .UserDefFldDCode
                                        oOpenClaim.UserDefFldECode = .UserDefFldECode
                                        oOpenClaim.IsPolicyOutstanding = .IsPolicyOutstanding

                                        If Session(CNMode) = Mode.ViewClaim Then
                                            oOpenClaim.IsRecovery = .IsRecovery
                                        End If
                                        oOpenClaim.TPA = .TPA
                                        'Added for Insurer
                                        oOpenClaim.Insurer = .Insurer
                                        Session.Item(CNClaimTimeStamp) = .TimeStamp
                                        oOpenClaim.CurrencyISOCode = .CurrencyCode
                                        Session.Item(CNCurrenyCode) = Trim(.CurrencyCode) 'Changed
                                        oOpenClaim.Client = .Client
                                        Session.Item(CNBaseCaseKey) = .BaseCaseKey
                                        'this needs to be removed after SAM issue is resolved
                                        If oOpenClaim.Client.PartyKey = 0 Then
                                            oOpenClaim.Client.PartyKey = oQuote.PartyKey
                                        End If
                                        'Session(CNInsurer_Header) = .ClientName
                                        Session(CNClaimNumber) = .ClaimNumber
                                        Session(CNStatus) = .ClaimStatus

                                        'Check Recovery Reserve
                                        Dim bAvailableReserve As Boolean = False
                                        If e.CommandName = "Salvage" Then
                                            If sInsuranceFileTypeCode = "WRITTEN" Then
                                                cvWrittenTypePolicy.Enabled = True
                                                cvWrittenTypePolicy.IsValid = False
                                                Exit Sub
                                            Else
                                                'Check the availability of salvage reserve
                                                If oOpenClaim.ClaimPeril IsNot Nothing Then
                                                    For iCounter As Integer = 0 To oOpenClaim.ClaimPeril.Count - 1
                                                        If oOpenClaim.ClaimPeril(iCounter).SalvageRecovery IsNot Nothing Then
                                                            If oOpenClaim.ClaimPeril(iCounter).SalvageRecovery.Count > 0 Then
                                                                For jCounter As Integer = 0 To oOpenClaim.ClaimPeril(iCounter).SalvageRecovery.Count - 1
                                                                    If oOpenClaim.ClaimPeril(iCounter).SalvageRecovery(jCounter).TotalRecovery > 0 Then
                                                                        bAvailableReserve = True
                                                                        Exit For
                                                                    End If
                                                                Next
                                                            End If
                                                        End If
                                                        If oOpenClaim.ClaimPeril(iCounter).Receipt IsNot Nothing Then
                                                            oOpenClaim.ClaimPeril(iCounter).Receipt.TimeStamp = .TimeStamp
                                                        End If
                                                        If bAvailableReserve = True Then
                                                            Exit For
                                                        End If
                                                    Next
                                                    'if reserve is available then link will be shown
                                                    If bAvailableReserve = False Then
                                                        ChkRecoveryReserver.Enabled = True
                                                        ChkRecoveryReserver.IsValid = False
                                                        bUnlockRequired = True
                                                        Exit Sub
                                                    Else
                                                        ChkRecoveryReserver.Enabled = False
                                                        ChkRecoveryReserver.IsValid = True
                                                    End If
                                                End If
                                            End If

                                        ElseIf e.CommandName = "TPRecovery" Then
                                            If sInsuranceFileTypeCode = "WRITTEN" Then
                                                cvWrittenTypePolicy.Enabled = True
                                                cvWrittenTypePolicy.IsValid = False
                                                Exit Sub
                                            Else
                                                'Check the availability of TPRecovery reserve
                                                If oOpenClaim.ClaimPeril IsNot Nothing Then
                                                    For iCounter As Integer = 0 To oOpenClaim.ClaimPeril.Count - 1
                                                        If oOpenClaim.ClaimPeril(iCounter).TPRecovery IsNot Nothing Then
                                                            If oOpenClaim.ClaimPeril(iCounter).TPRecovery.Count > 0 Then
                                                                For jCounter As Integer = 0 To oOpenClaim.ClaimPeril(iCounter).TPRecovery.Count - 1
                                                                    If oOpenClaim.ClaimPeril(iCounter).TPRecovery(jCounter).TotalRecovery > 0 Then
                                                                        bAvailableReserve = True
                                                                        Exit For
                                                                    End If
                                                                Next
                                                            End If
                                                        End If
                                                        If oOpenClaim.ClaimPeril(iCounter).Receipt IsNot Nothing Then
                                                            oOpenClaim.ClaimPeril(iCounter).Receipt.TimeStamp = .TimeStamp
                                                        End If
                                                        If bAvailableReserve = True Then
                                                            Exit For
                                                        End If
                                                    Next
                                                    'if reserve is available then link will be shown
                                                    If bAvailableReserve = False Then
                                                        ChkRecoveryReserver.Enabled = True
                                                        ChkRecoveryReserver.IsValid = False
                                                        bUnlockRequired = True
                                                        Exit Sub
                                                    Else
                                                        ChkRecoveryReserver.Enabled = False
                                                        ChkRecoveryReserver.IsValid = True
                                                    End If
                                                End If
                                            End If
                                        End If
                                        If e.CommandName = "Pay" Or e.CommandName = "View" Or e.CommandName = "Salvage" Or e.CommandName = "TPRecovery" Then
                                            'Retreival of the risk related values 
                                            'Arch issue 268

                                            Dim oOptionType As New NexusProvider.OptionTypeSetting
                                            oOptionType = oWebservice.GetOptionSetting(NexusProvider.OptionType.ProductOption, 12)
                                            If (oOptionType IsNot Nothing AndAlso String.IsNullOrEmpty(oOptionType.OptionValue) = False) _
                                               AndAlso oOptionType.OptionValue = "1" Then
                                                oClaimRisk = GetClaimRiskCall(.BaseClaimKey, .ClaimKey, sBranchCode)
                                                Session(CNDataSet) = oClaimRisk.XMLDataSet
                                            End If
                                        End If
                                    End With
                                End If
                            Next

                            Session(CNClaim) = oOpenClaim
                            bUnlockRequired = False
                            Response.Redirect("~/Claims/Overview.aspx", False)
                        Finally
                            If bUnlockRequired Then
                                Dim oLock As New NexusProvider.Locks
                                Dim oLockCollection As New NexusProvider.LockCollection

                                oLock.LockName = "claim_id"                    ''It is equivalent to lockname for locking claims in SAM
                                oLock.LockValue = oOpenClaim.BaseClaimKey

                                oLockCollection.Add(oLock)
                                oWebservice.MaintainLock(oLockCollection, False, False, Session(CNBranchCode).ToString())
                            End If
                            oClaimDetails = Nothing
                            oWebservice = Nothing
                            oClaimRisk = Nothing
                        End Try
                    End If
                    ' End of Find Latest Claim Version
            End Select
        End Sub

        Protected Sub grdvSearchResults_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles grdvSearchResults.RowCreated
            If (e.Row.RowType = DataControlRowType.Header OrElse e.Row.RowType = DataControlRowType.DataRow) Then
                Dim oAllowPolicyClientAssociationsOptionSettings As NexusProvider.OptionTypeSetting = CType(ViewState("AllowPolicyClientAssociationsOptionSettings"), NexusProvider.OptionTypeSetting)

                'Hide the PolicyAssociate Column if the Hidden option to show PolicyClientAssociate is False
                If oAllowPolicyClientAssociationsOptionSettings IsNot Nothing AndAlso oAllowPolicyClientAssociationsOptionSettings.OptionValue = "1" Then
                    grdvSearchResults.Columns(4).Visible = True
                Else
                    grdvSearchResults.Columns(4).Visible = False
                End If
            End If
        End Sub

        ''' <summary>
        ''' This is fired on the row data bound of the Grid View.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvSearchResults_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvSearchResults.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim btnSelectButton As LinkButton = CType(e.Row.FindControl("btnSelect"), LinkButton)
                Dim btnEditButton As LinkButton = CType(e.Row.FindControl("btnEdit"), LinkButton)
                Dim btnViewButton As LinkButton = CType(e.Row.FindControl("btnView"), LinkButton)
                Dim btnPayButton As LinkButton = CType(e.Row.FindControl("btnPay"), LinkButton)
                Dim btnSalvageButton As LinkButton = CType(e.Row.FindControl("btnSalvage"), LinkButton)
                Dim btnTPRecovery As LinkButton = CType(e.Row.FindControl("btnTPRecovery"), LinkButton)


                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("ClaimKey") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.Claim).ClaimKey)

                If (UCase(CType(e.Row.DataItem, NexusProvider.Claim).ProgressStatusDescription.Trim()) = "CLOSED") Then
                    btnEditButton.Visible = False
                End If

                'check Maintain Claim role exists, if it doesn't then hide the Edit button

                If Not UserCanDoTask("MaintainClaim") Then
                    btnEditButton.Visible = False
                Else
                    btnEditButton.Visible = True
                End If


                'check ViewClaim role exists, if it doesn't then hide the View button

                If Not UserCanDoTask("ViewClaim") Then
                    btnViewButton.Visible = False
                Else
                    btnViewButton.Visible = True
                End If

                'check PaymentClaim role exists, if it doesn't then hide the Pay button

                If Not UserCanDoTask("PaymentClaim") Then
                    btnPayButton.Visible = False
                Else
                    btnPayButton.Visible = True
                End If

                If Not UserCanDoTask("SalvageClaim") Then
                    btnSalvageButton.Visible = False
                Else
                    btnSalvageButton.Visible = True
                End If

                If Not UserCanDoTask("TPRecoveryClaim") Then
                    btnTPRecovery.Visible = False
                Else
                    btnTPRecovery.Visible = True
                End If

                If Request.QueryString("Page") = "AP" Or Request.QueryString("Page") = "EL" _
                Or Request.QueryString("Page") = "CC" Or Request.QueryString("Page") = "CEL" Then
                    If Request.QueryString("Page") = "CC" Then
                        Dim sMessage As String = GetLocalResourceObject("msg_LinkCaseConfirm")
                        sMessage = sMessage.Replace("#ClaimNumber", CType(e.Row.DataItem, NexusProvider.Claim).ClaimNumber)

                        If Request.QueryString("CaseNumber") IsNot Nothing Then
                            sMessage = sMessage.Replace("#CaseNumber", Convert.ToString(Request.QueryString("CaseNumber")))
                        End If

                        btnSelectButton.OnClientClick = "javascript:return LinkCaseConfirmation('" & sMessage & "');"
                    End If
                    btnEditButton.Visible = False
                    btnViewButton.Visible = False
                    btnPayButton.Visible = False
                    btnSalvageButton.Visible = False
                    btnTPRecovery.Visible = False
                Else
                    btnSelectButton.Visible = False
                End If
                Dim oAllowPolicyClientAssociations As NexusProvider.OptionTypeSetting = Nothing
                oAllowPolicyClientAssociations = ViewState("AllowPolicyClientAssociationsOptionSettings")
                If oAllowPolicyClientAssociations IsNot Nothing AndAlso oAllowPolicyClientAssociations.OptionValue = "1" Then
                    Dim xmldoc As New System.Xml.XmlDocument
                    If (CType(e.Row.DataItem, NexusProvider.Claim).AssociatedClients IsNot Nothing AndAlso Not (String.IsNullOrEmpty(CType(e.Row.DataItem, NexusProvider.Claim).AssociatedClients))) Then
                        xmldoc.InnerXml = CType(e.Row.DataItem, NexusProvider.Claim).AssociatedClients

                        Dim rptrFolderNavigation As Repeater = e.Row.FindControl("rptrAssociateClient")
                        If rptrFolderNavigation IsNot Nothing Then
                            rptrFolderNavigation.DataSource = xmldoc.SelectNodes("/Associates/Associate")
                            rptrFolderNavigation.DataBind()
                        End If
                    End If
                    xmldoc = Nothing
                End If
                If e.Row.FindControl("lblDescription") IsNot Nothing Then

                    Dim oLabelDesc As Label = e.Row.FindControl("lblDescription")
                    Dim sdesccription As String = oLabelDesc.Text
                    e.Row.Attributes.Add("title", sdesccription)
                End If
            End If

        End Sub

        Protected Sub grdvSearchResults_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles grdvSearchResults.RowEditing
            'This Event need blank body in order to "Edit" the claim
        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If HttpContext.Current.Session.IsCookieless Then
                btnClient.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/secure/agent/FindClient.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700' , null);return false;"
                btnPolicy.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Claims/FindInsuranceFile.aspx?modal=true&KeepThis=true&FindClaim=1&ClientType=Claim&TB_iframe=true&height=500&width=700' , null);return false;"
                btnCaseNumber.OnClientClick = "tb_show(null ,'../Claims/FindCase.aspx?modal=true&KeepThis=true&Page=EL&FindCase=1&TB_iframe=true&height=550&width=750' , null);return false;"
            Else
                btnClient.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/secure/agent/FindClient.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700' , null);return false;"
                btnPolicy.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/Claims/FindInsuranceFile.aspx?modal=true&KeepThis=true&FindClaim=1&ClientType=Claim&TB_iframe=true&height=500&width=700' , null);return false;"
                btnCaseNumber.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "Claims/FindCase.aspx?modal=true&KeepThis=true&Page=EL&FindCase=1&TB_iframe=true&height=550&width=750' , null);return false;"
            End If
        End Sub
        ''' <summary>
        ''' sort the Claim according to the column clicked
        ''' we need to store the current sort order in viewstate, and reverse it each time
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvSearchResults_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdvSearchResults.Sorting

            Dim oClaimsCollection As NexusProvider.ClaimCollection = CType(Session(CNClaimsSearchData), NexusProvider.ClaimCollection)
            oClaimsCollection.SortColumn = e.SortExpression
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
            oClaimsCollection.SortingOrder = _sortDirection
            oClaimsCollection.Sort()
            CType(sender, GridView).DataSource = oClaimsCollection
            CType(sender, GridView).DataBind()
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs) Handles Me.Unload

        End Sub

        Private Sub UnlockClaim(ByVal nClaimKey As Integer)
            Dim oLockCollection As NexusProvider.LockCollection
            Dim oWebService As NexusProvider.ProviderBase = Nothing
            Dim sUserName As String = String.Empty
            Dim bMaintainedSuccess As Boolean = False
            Dim bLogout As Boolean = False
            Dim bAllClear As Boolean = False
            Dim oLock As New NexusProvider.Locks
            oWebService = New NexusProvider.ProviderManager().Provider
            oLockCollection = oWebService.GetLockDetails(Session(CNBranchCode).ToString())

            For Each oLockItem As NexusProvider.Locks In oLockCollection
                If HttpContext.Current.User.Identity.Name.Trim().ToLower().ToUpper = oLockItem.LockUserName.Trim().ToLower().ToUpper AndAlso oLockItem.LockName.Trim() = "claim_id" AndAlso oLockItem.LockValue = nClaimKey Then
                    oLock.LockName = oLockItem.LockName
                    oLock.LockValue = oLockItem.LockValue
                    oLockCollection.Add(oLock)
                    bMaintainedSuccess = oWebService.MaintainLock(oLockCollection, bAllClear, bLogout, Session(CNBranchCode).ToString())
                    Exit For
                End If
            Next
        End Sub
        Protected Sub GetSystemOption()
            Dim oAllowPolicyClientAssociationsOptionSettings As NexusProvider.OptionTypeSetting
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

            oAllowPolicyClientAssociationsOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.AllowPolicyClientAssociations)
            ViewState("AllowPolicyClientAssociationsOptionSettings") = oAllowPolicyClientAssociationsOptionSettings

            oWebService = Nothing
        End Sub
    End Class
End Namespace
