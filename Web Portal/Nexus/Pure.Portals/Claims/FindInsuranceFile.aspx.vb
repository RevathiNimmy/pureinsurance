Imports System.Web.Configuration.WebConfigurationManager
Imports System.Web.Configuration
Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Utils

Namespace Nexus
    Partial Class Claims_FindInsuranceFile
        Inherits CMS.Library.Frontend.clsCMSPage

        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                'Cleaning of the session values
                ClearQuote()
                ClearClaims()
                ClearHeader()
                ClearCase()
                GetSystemOption()
                ClearTemporarySessionValues()
                'Update the session for case key
                If Request.QueryString("BaseCaseKey") IsNot Nothing Then
                    Session(CNBaseCaseKey) = Request.QueryString("BaseCaseKey")
                End If

                If Request.QueryString("Mode") IsNot Nothing AndAlso Request.QueryString("Mode") = "ManualTransfer" Then
                    Session(CNNoTrans) = "Claim"
                Else
                    Session(CNNoTrans) = Nothing
                End If

                'Dynamic javascript
                'To set the Focus
                Page.SetFocus(txtPolicyNumber)

                Claims_FindInsuranceFile__Claim_Date.Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper()
                rngClaimDate.MaximumValue = DateTime.MaxValue.ToShortDateString

                'create a unique key and add this to viewstate
                'this will be used to cache the results of the SAM call
                Dim InsuranceFileDetailspageCacheID As Guid
                InsuranceFileDetailspageCacheID = Guid.NewGuid()
                ViewState.Add("InsuranceFileDetailspageCacheID", InsuranceFileDetailspageCacheID.ToString)

                'This page is called from various pages ( although these would not be required in future)
                If Request.QueryString("Page") = "RS" Or Request.QueryString("Page") = "EL" Or Request.QueryString("Page") = "Renewal" Then
                    rqdClaimDate.Enabled = False
                    rngClaimDate.Enabled = False
                    'emClaimDate.Visible = False
                    Claims_FindInsuranceFile__Claim_Date.CssClass = "field-medium"
                    liCoverNote.Visible = False
                    liClaimDate.Visible = False
                    liForcedFrom.Visible = False
                    liForcedTo.Visible = False
                    'Event List
                    If Request.QueryString("Page") = "EL" Then
                        If Session(CNParty) IsNot Nothing Then
                            Select Case True
                                Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                                    With CType(Session(CNParty), NexusProvider.CorporateParty)
                                        txtShortName.Text = .ClientSharedData.ShortName
                                    End With
                                Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                                    With CType(Session(CNParty), NexusProvider.PersonalParty)
                                        txtShortName.Text = .ClientSharedData.ShortName
                                    End With
                            End Select
                        End If
                        txtShortName.Enabled = False
                    End If
                End If
            End If

            txtCoverNote.Attributes.Add("onkeypress", "javascript:return isInteger(event);")
            Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
            If oUserDetails IsNot Nothing Then
                If (oUserDetails.Key <> 0 AndAlso Request.QueryString("PolicyNo") <> "" AndAlso Request.QueryString("submit") <> "" AndAlso Request.QueryString("submit") = "false") Then
                    hvMakePolicyNumberReadOnly.Value = 1
                End If
            End If

        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            'To change the master page on condition
            If Request("FindClaim") = "1" Or Request.QueryString("Page") = "AP" Or Request.QueryString("Page") = "RS" _
            Or Request.QueryString("Page") = "EL" Or Request.QueryString("Page") = "Renewal" Then
                CMS.Library.Frontend.Functions.SetTheme(Page, ConfigurationManager.AppSettings("ModalPageTemplate"))
            End If

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "LapseConfirmation", _
              "<script language=""JavaScript"" type=""text/javascript"">function RenewalConfirmation(){ alert('" & GetLocalResourceObject("msg_ConfirmRenewalPolicy").ToString() & "'); return true;}</script>")
        End Sub

#Region " Find Insurance - Button Click Events "

        Protected Sub btnNewSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewSearch.Click
            'Cleaning of the search citeria values
            ClearInsuranceFileSearchCriteria()
            grdvInsuranceFile.Visible = False
            grdvInsuranceFile.DataSource = Nothing
            grdvInsuranceFile.DataBind()
        End Sub
        Sub BindGrid()
            'try to get the search results from the cache
            Dim oInsuranceFileDetails As NexusProvider.InsuranceFileDetailsCollection = _
                    CType(Cache.Item(ViewState("InsuranceFileDetailspageCacheID")), NexusProvider.InsuranceFileDetailsCollection)

            If oInsuranceFileDetails Is Nothing Then
                'Get search results by calling FindInsuranceFileForClaims
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oInsuranceFileSearchCriteria As New NexusProvider.InsuranceFileDetails

                Dim oGrdInsuranceFileDetails As New NexusProvider.InsuranceFileDetailsCollection
                Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
                Dim sBranchCode As String = oUserDetails.ListOfBranches(0).Code

                With oInsuranceFileSearchCriteria
                    .InsuranceRef = Trim(txtPolicyNumber.Text)
                    .CoverNoteSheetNumber = CInt(IIf(String.IsNullOrEmpty(Trim(txtCoverNote.Text)), 0, txtCoverNote.Text))
                    .RiskIndex = Trim(txtRiskIndex.Text)
                    If rqdClaimDate.Enabled = True Then
                        .LossDate = CDate(Trim(Claims_FindInsuranceFile__Claim_Date.Text))
                        .SearchDate = CDate(Trim(Claims_FindInsuranceFile__Claim_Date.Text))
                    Else
                        .SearchDate = Date.Now
                        .LossDate = Date.Now
                    End If

                    .ClientShortName = Trim(txtShortName.Text)
                    .PostCode = Trim(txtPostCode.Text)
                    .InForceFrom = CDate(IIf(String.IsNullOrEmpty(Trim(Claims_FindInsuranceFile__ForceFrom.Text)), DateTime.MinValue, Claims_FindInsuranceFile__ForceFrom.Text))
                    .InForceTo = CDate(IIf(String.IsNullOrEmpty(Trim(Claims_FindInsuranceFile_ForceTo.Text)), DateTime.MinValue, Claims_FindInsuranceFile_ForceTo.Text))

                    'to limit the search return from SAM
                    oInsuranceFileSearchCriteria.MaxRowsToFetch = oPortal.MaxSearchResults

                End With


                oInsuranceFileDetails = oWebService.FindInsuranceFileForClaims(oInsuranceFileSearchCriteria, sBranchCode)

                Cache.Insert(ViewState("InsuranceFileDetailspageCacheID"), oInsuranceFileDetails, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

                'validate size of dataset. if 500(configured at portal level) or more results are returned then add a validation message to the screen
                If oInsuranceFileDetails.Count >= oPortal.MaxSearchResults Then
                    'create a custom validator
                    Dim cstMaxResults As New CustomValidator
                    cstMaxResults.IsValid = False
                    'look for a validation message in the page resources, but if there is not one defined add a default message
                    cstMaxResults.ErrorMessage = IIf(GetLocalResourceObject("cstMaxResults") Is Nothing, "Maximum number of search results exceeded, please refine your search criteria", GetLocalResourceObject("cstMaxResults"))
                    cstMaxResults.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                    'add the validator to the page, this will have the effect of making the page invalid
                    Page.Validators.Add(cstMaxResults)
                End If
            End If

            grdvInsuranceFile.Visible = True
            grdvInsuranceFile.AllowPaging = True
            grdvInsuranceFile.DataSource = oInsuranceFileDetails
            grdvInsuranceFile.DataBind()
        End Sub
        Protected Sub btnFindNow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
            If Page.IsValid Then
                Cache.Remove(ViewState("InsuranceFileDetailspageCacheID"))
                BindGrid()
            End If
        End Sub

        Protected Sub btnRefreshShortName_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRefreshShortName.Click
            txtShortName.Text = CStr(Session(CNClaimShortName))
        End Sub

#End Region

#Region " Find Insurance - GridView Events "

        Protected Sub grdvInsuranceFile_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvInsuranceFile.DataBound
            If grdvInsuranceFile.Rows.Count = 0 Or grdvInsuranceFile.PageCount = 1 Then
                grdvInsuranceFile.AllowPaging = False
            End If
        End Sub
        Protected Sub grdvInsuranceFile_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvInsuranceFile.PageIndexChanging
            grdvInsuranceFile.PageIndex = e.NewPageIndex
            BindGrid()
        End Sub
        Protected Sub grdvInsuranceFile_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdvInsuranceFile.RowCommand
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
            Dim iInsuranceFileKey As Integer
            Dim sInsuranceRef As String, sTypeCode As String
            Dim dLossDate As Date
            Dim dtStartDate As Date, dtEndDate As Date, dtInceptionDate As Date
            Dim iReturnCode As Integer
            Dim oInsuranceFileDetails As NexusProvider.InsuranceFileDetailsCollection = _
            CType(Cache.Item(ViewState("InsuranceFileDetailspageCacheID")), NexusProvider.InsuranceFileDetailsCollection)
            Dim iRowIndex As Integer

            For iTemp = 0 To grdvInsuranceFile.Rows.Count - 1
                If e.CommandArgument.ToString().Trim() = oInsuranceFileDetails.Item(iTemp).InsuranceFileKey.ToString() Then
                    iRowIndex = iTemp
                    Exit For
                End If
            Next

            If iRowIndex > -1 Then
                If (oInsuranceFileDetails(iRowIndex).IsSourceClosed = 1) Then
                    If (oInsuranceFileDetails(iRowIndex).AllowedClosedBranchClaims = 0) Then
                        ScriptManager.RegisterStartupScript(Me, GetType(String), "alert", "alert('Claims cannot be opened against this policy because it is associated to a closed branch.');", True)
                        Exit Sub
                    End If
                End If
            End If

            If Not String.IsNullOrEmpty(Request.QueryString("Page")) And Request.QueryString("Page") = "AP" Then
                Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.setPolicy('" + e.CommandArgument.ToString + "');", True)
                'RS -Renewal Selection Page and EL- Event List Page
                ' WPR VB 64 - Media Type Status
            ElseIf Not String.IsNullOrEmpty(Request.QueryString("Page")) And (Request.QueryString("Page") = "RS" Or Request.QueryString("Page") = "EL" _
            Or Request.QueryString("Page") = "CR" Or Request.QueryString("Page") = "Renewal") Then
                'Dim oInsuranceFileDetails As NexusProvider.InsuranceFileDetailsCollection = _
                For iTempVar As Integer = 0 To oInsuranceFileDetails.Count - 1
                    If e.CommandArgument = oInsuranceFileDetails(iTempVar).InsuranceRef Then
                        iInsuranceFileKey = oInsuranceFileDetails(iTempVar).InsuranceFileKey
                        Dim sClientCode As String = oInsuranceFileDetails(iTempVar).ClientShortName
                        Session(CNPolicyNumber) = oInsuranceFileDetails(iTempVar).InsuranceRef.Trim
                        'Session(CNInsuranceFileKey) = iInsuranceFileKey
                        If Request.QueryString("FromPage") = "BG" Then
                            ScriptManager.RegisterStartupScript(Me, GetType(String), "closeThickBox", "self.parent.setPolicy('" + e.CommandArgument.ToString + "','" + iInsuranceFileKey.ToString + "','" + sClientCode.ToString + "');", True)
                            Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "RefreshBGsLoad") & ";"
                            ScriptManager.RegisterStartupScript(Me, GetType(String), "ParentPostBack", PostBackStr, True)
                        Else
                            ScriptManager.RegisterStartupScript(Me, GetType(String), "closeThickBox", "self.parent.setPolicy('" + e.CommandArgument.ToString + "','" + iInsuranceFileKey.ToString + "');", True)
                        End If
                        Exit For
                    End If
                Next

            ElseIf e.CommandName = "Maintain" Or e.CommandName = "Open" Then
                If Page.IsValid Then
                    Dim lbtn As LinkButton = e.CommandSource
                    Dim CurrentRow As GridViewRow = lbtn.NamingContainer
                    Dim nInsuranceFilKey As Integer = Convert.ToInt32(grdvInsuranceFile.DataKeys(CurrentRow.RowIndex).Values("InsuranceFileKey"))
                    Dim bIsPendingPortfolioTransfer, bIsPendingCloneTransfer As Boolean

                    oWebService.IsPendingTransfer(nInsuranceFilKey, bIsPendingCloneTransfer, bIsPendingPortfolioTransfer, Nothing)
                    Dim sMessage As String = ""
                    If bIsPendingCloneTransfer = True Or bIsPendingPortfolioTransfer = True Then
                        If bIsPendingPortfolioTransfer = True Then
                            sMessage = GetLocalResourceObject("msg_PendingPortfolioTransfer")
                        ElseIf bIsPendingCloneTransfer = True Then
                            sMessage = GetLocalResourceObject("msg_PendingClonedTransfer")
                        End If
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "PendingPortfolioTransfer", "alert('" + sMessage + "')", True)
                        Exit Sub
                    End If


                    If e.CommandName = "Open" Then
                        If e.CommandArgument <> 0 Then

                            '  CType(Cache.Item(ViewState("InsuranceFileDetailspageCacheID")), NexusProvider.InsuranceFileDetailsCollection)

                            For i As Integer = 0 To oInsuranceFileDetails.Count - 1
                                If oInsuranceFileDetails(i).InsuranceFileKey = e.CommandArgument Then
                                    Dim oOpenClaim As New NexusProvider.ClaimOpen
                                    Dim oAgentDetailsPolicy As NexusProvider.AgentDetailsForPolicy = Nothing
                                    Dim oPartyDetails As NexusProvider.InsuranceFileDetails = Nothing

                                    Dim oInsuranceFileCollection As NexusProvider.PolicyCollection

                                    iInsuranceFileKey = oInsuranceFileDetails(i).InsuranceFileKey
                                    sInsuranceRef = oInsuranceFileDetails(i).InsuranceFileRef
                                    dLossDate = CDate(Trim(Claims_FindInsuranceFile__Claim_Date.Text))

                                    'btnNext.Attributes.Remove("onclick")
                                    Dim oQuote1 As NexusProvider.Quote = oWebService.GetHeaderAndSummariesByKey(iInsuranceFileKey, , True)
                                    oInsuranceFileCollection = oWebService.GetAllPolicyVersions(oQuote1.InsuranceFolderKey)
                                    GetPolicyForClaimDate(oInsuranceFileCollection, dLossDate, iInsuranceFileKey, sInsuranceRef, dtStartDate, dtEndDate, iReturnCode, dtInceptionDate, sTypeCode)

                                    If iInsuranceFileKey = 0 Then
                                        iInsuranceFileKey = oInsuranceFileDetails(i).InsuranceFileKey
                                    End If

                                    Try
                                        'Populating the Agent(insurer) Details if Policy is associated with agent
                                        If oInsuranceFileDetails(i).LeadAgentKey > 0 Then
                                            oAgentDetailsPolicy = oWebService.GetAgentDetailsForPolicy(iInsuranceFileKey)
                                        End If
                                        'Populating the Client details
                                        oPartyDetails = oWebService.GetClaimPartyDetails(iInsuranceFileKey)
                                    Finally

                                    End Try

                                    With oOpenClaim
                                        If oPartyDetails IsNot Nothing Then
                                            .ClientName = oPartyDetails.ClientName
                                            .Client.ClientName = oPartyDetails.ClientName
                                            .ClientShortName = oPartyDetails.ClientShortName
                                            .Client.ShortName = oPartyDetails.ClientShortName
                                            .Client.Address = oPartyDetails.Address
                                            .Client.PartyKey = oPartyDetails.PartyKey
                                            If .Client.PartyKey > 0 Then
                                                Dim oBaseParty As NexusProvider.BaseParty
                                                oBaseParty = oWebService.GetParty(.Client.PartyKey)
                                                Session.Item(CNParty) = oBaseParty
                                            End If
                                            .Client.Address.CountryCode = GetCodeForKey(NexusProvider.ListType.PMLookup, oPartyDetails.CountryKey, "Country", True)
                                            .ClientEmail = oPartyDetails.Email
                                            .ClientFaxNo = oPartyDetails.Fax
                                            .ClientMobileNo = oPartyDetails.Mobile
                                            .ClientTelNo = oPartyDetails.TelHome
                                            .ClientTelNoOff = oPartyDetails.TelOff
                                        Else
                                            .Client = Nothing
                                        End If

                                        If oAgentDetailsPolicy IsNot Nothing Then
                                            .Insurer.ContactName = oAgentDetailsPolicy.Name
                                            .Insurer.InsurerName = oAgentDetailsPolicy.Name
                                            .Insurer.ShortName = oAgentDetailsPolicy.Shortname
                                            .Insurer.Address.Address1 = oAgentDetailsPolicy.Address1
                                            .Insurer.Address.Address2 = oAgentDetailsPolicy.Address2
                                            .Insurer.Address.Address3 = oAgentDetailsPolicy.Address3
                                            .Insurer.Address.Address4 = oAgentDetailsPolicy.Address4
                                            .Insurer.Address.CountryCode = GetCodeForKey(NexusProvider.ListType.PMLookup, oAgentDetailsPolicy.CountryKey, "Country", True)
                                            .Insurer.Address.CountryDescription = GetDescriptionForCode(NexusProvider.ListType.PMLookup, .Insurer.Address.CountryCode, "COUNTRY")
                                            .Insurer.Address.PostCode = oAgentDetailsPolicy.PostalCode
                                            .Insurer.Address.CountryCode = .Insurer.Address.CountryCode
                                            .Insurer.Address.CountryDescription = .Insurer.Address.CountryDescription

                                            If oAgentDetailsPolicy.Contacts IsNot Nothing Then
                                                .Insurer.Contact = oAgentDetailsPolicy.Contacts
                                            End If
                                        Else
                                            .Insurer = Nothing
                                        End If
                                    End With
                                    Session(CNLapseDate) = oInsuranceFileDetails(i).LapseDate
                                    Session(CNClaim) = oOpenClaim
                                    Exit For
                                End If
                            Next

                            Session(CNInsuranceFileKey) = iInsuranceFileKey
                            Dim oQuote As NexusProvider.Quote = Nothing
                            Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
                            Dim sBranchCode As String = oUserDetails.ListOfBranches(0).Code

                            Try
                                'Retreival of the quote information and storing in session
                                oQuote = oWebService.GetHeaderAndSummariesByKey(iInsuranceFileKey, sBranchCode, True)
                                'If policy is CAN and MTACAN then cover start date is less than equal to current date then claim can not be processed
                                If oQuote.InsuranceFileStatusCode IsNot Nothing Then
                                    If oQuote.InsuranceFileStatusCode.Trim.ToUpper = "CAN" And CDate(oQuote.CoverStartDate.ToShortDateString) <= CDate(Date.Now.ToShortDateString) Then
                                        'IsPolicyCancelled.IsValid = False

                                    End If
                                ElseIf oQuote.InsuranceFileTypeCode IsNot Nothing Then
                                    If oQuote.InsuranceFileTypeCode.Trim.ToUpper = "MTACAN" And CDate(oQuote.CoverStartDate.ToShortDateString) <= CDate(Date.Now.ToShortDateString) Then
                                        'IsPolicyCancelled.IsValid = False
                                        'Exit Sub
                                    End If
                                ElseIf oQuote.PolicyStatusCode IsNot Nothing Then
                                    'on Policy status LAP, claim can not be processed
                                    If oQuote.PolicyStatusCode.Trim.ToUpper = "LAP" Then
                                        IsPolicyCancelled.IsValid = False
                                        Exit Sub
                                    End If
                                Else
                                    IsPolicyCancelled.IsValid = True
                                End If
                                'Updation of the session variables

                                Session(CNLossDate) = FormatDateTime(Claims_FindInsuranceFile__Claim_Date.Text, DateFormat.LongDate) & " " & Date.Now.ToShortTimeString
                                Session(CNLossToDate) = FormatDateTime(Claims_FindInsuranceFile__Claim_Date.Text, DateFormat.LongDate) & " " & Date.Now.ToShortTimeString
                                Session(CNRisks) = oQuote.Risks
                                Session.Item(CNPartyKey) = oQuote.PartyKey
                                Session.Item(CNDate_Header) = oQuote.CoverStartDate.ToShortDateString & " - " & oQuote.CoverEndDate.ToShortDateString
                                Session.Item(CNPolicyNumber) = oQuote.InsuranceFileRef
                                Session.Item(CNCurrenyCode) = oQuote.CurrencyCode
                                Session.Item(CNClaimNumber) = GetLocalResourceObject("lbl_ClaimNumber").ToString()
                                Session.Item(CNStatus) = oPortal.Claims.NewClaimStatus
                                Session(CNInsurer_Header) = oQuote.InsuredName
                                Session(CNProductCode) = oQuote.ProductCode
                                Session(CNPolicyNumber) = oQuote.InsuranceFileRef
                                Session(CNMode) = Mode.NewClaim
                                Session(CNClaimQuote) = oQuote
                                Session(CNInsuranceFileStatus) = oQuote.InsuranceFileStatusCode

                                Response.Redirect("~/claims/overview.aspx", False)

                            Finally
                                oWebService = Nothing
                                oUserDetails = Nothing
                            End Try
                        Else
                            IsPolicyExist.IsValid = False
                        End If
                    ElseIf e.CommandName = "Maintain" Then
                        Dim sClaimType As String = Request("ClientType")
                        Session(CNClaimPolicy) = CStr(e.CommandArgument).Trim
                        'Sending back the data to the parent screen
                        ScriptManager.RegisterStartupScript(Me, GetType(String), "closeThickBox", "self.parent.setPolicy('" + e.CommandArgument.ToString + "');", True)
                    End If
                End If
            End If
        End Sub

        Protected Sub grdvInsuranceFile_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles grdvInsuranceFile.RowCreated
            If (e.Row.RowType = DataControlRowType.Header OrElse e.Row.RowType = DataControlRowType.DataRow) Then
                Dim oAllowPolicyClientAssociationsOptionSettings As NexusProvider.OptionTypeSetting = CType(ViewState("AllowPolicyClientAssociationsOptionSettings"), NexusProvider.OptionTypeSetting)

                'Hide the PolicyAssociate Column if the Hidden option to show PolicyClientAssociate is False
                If oAllowPolicyClientAssociationsOptionSettings IsNot Nothing AndAlso oAllowPolicyClientAssociationsOptionSettings.OptionValue = "1" Then
                    grdvInsuranceFile.Columns(10).Visible = True
                Else
                    grdvInsuranceFile.Columns(10).Visible = False
                    End If
                End If
        End Sub

        Protected Sub grdvInsuranceFile_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvInsuranceFile.RowDataBound

            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim lnkbtnSelect As LinkButton = e.Row.FindControl("lnkDetails")

                If UserCanDoTask("OpenClaim") Then
                    lnkbtnSelect.Visible = True
                Else
                    lnkbtnSelect.Visible = False
                End If

                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("InsuranceFileKey") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.InsuranceFileDetails).InsuranceFileKey)

                If Request("FindClaim") = "1" Then 'call from Claims/FindClaim.aspx page

                    lnkbtnSelect.CommandArgument = CType(e.Row.DataItem, NexusProvider.InsuranceFileDetails).InsuranceRef
                    lnkbtnSelect.CommandName = "Maintain"

                ElseIf Request.QueryString("Page") = "RS" Or Request.QueryString("Page") = "EL" _
                Or Request.QueryString("Page") = "CR" Or Request.QueryString("Page") = "Renewal" _
                Or Request.QueryString("Page") = "AP" Then

                    lnkbtnSelect.CommandArgument = CType(e.Row.DataItem, NexusProvider.InsuranceFileDetails).InsuranceRef

                Else 'call from Claims/FindInsuranceFile.aspx page

                    lnkbtnSelect.CommandArgument = CType(e.Row.DataItem, NexusProvider.InsuranceFileDetails).InsuranceFileKey
                    lnkbtnSelect.CommandName = "Open"

                    If CType(e.Row.DataItem, NexusProvider.InsuranceFileDetails).CoverTo < CDate(Claims_FindInsuranceFile__Claim_Date.Text) Then
                        If CType(e.Row.DataItem, NexusProvider.InsuranceFileDetails).InsuranceFileStatusCode.Trim().ToUpper = "REN" Then
                            lnkbtnSelect.OnClientClick = "var ret = confirm('" & GetLocalResourceObject("msg_ConfirmExpiry").ToString() & "'); if(ret==true){alert('" & GetLocalResourceObject("msg_ConfirmRenewalPolicy").ToString() & "');} return ret;"
                        Else
                            lnkbtnSelect.OnClientClick = "return confirm('" & GetLocalResourceObject("msg_ConfirmExpiry").ToString() & "');"
                        End If

                    Else

                        If CType(e.Row.DataItem, NexusProvider.InsuranceFileDetails).InsuranceFileStatusCode.Trim().ToUpper = "REN" Then
                            lnkbtnSelect.OnClientClick = "return RenewalConfirmation();"
                        End If

                    End If

                    If CType(e.Row.DataItem, NexusProvider.InsuranceFileDetails).InceptionDate > CDate(Claims_FindInsuranceFile__Claim_Date.Text) Then
                        lnkbtnSelect.OnClientClick = "return confirm('" & GetLocalResourceObject("msg_ConfirmInception").ToString() & "');"
                    End If

                    If CType(e.Row.DataItem, NexusProvider.InsuranceFileDetails).InsuranceFileStatusCode.Trim().ToUpper = "CAN" _
                    And CType(e.Row.DataItem, NexusProvider.InsuranceFileDetails).LapseDate <= CDate(Claims_FindInsuranceFile__Claim_Date.Text) Then
                        lnkbtnSelect.OnClientClick = "return confirm('" & GetLocalResourceObject("msg_ConfirmCancel").ToString().Replace("#CoverFrom#", CType(e.Row.DataItem, NexusProvider.InsuranceFileDetails).LapseDate.ToShortDateString()) & "');"
                    End If

                    Dim oAllowPolicyClientAssociationsOptionSettings As NexusProvider.OptionTypeSetting

                    oAllowPolicyClientAssociationsOptionSettings = ViewState("AllowPolicyClientAssociationsOptionSettings")

                    If e.Row.Cells(9).Text = Date.MinValue Then
                        e.Row.Cells(9).Text = ""
                    Else
                        Dim dt As DateTime = DateTime.Parse(e.Row.Cells(9).Text)
                        Dim dtYear As Int32 = dt.Year
                        If dtYear <= 1900 Then
                            e.Row.Cells(9).Text = ""
                        End If
                    End If

                    If oAllowPolicyClientAssociationsOptionSettings IsNot Nothing AndAlso oAllowPolicyClientAssociationsOptionSettings.OptionValue = "1" Then
                            If oPortal.EnableMasterClientAssociate = True Then
                                Dim xmldoc As New System.Xml.XmlDocument
                                If e.Row.DataItem IsNot Nothing Then
                                    If (CType(e.Row.DataItem, NexusProvider.InsuranceFileDetails).AssociatedClients IsNot Nothing AndAlso Not (String.IsNullOrEmpty(CType(e.Row.DataItem, NexusProvider.InsuranceFileDetails).AssociatedClients))) Then
                                        xmldoc.InnerXml = CType(e.Row.DataItem, NexusProvider.InsuranceFileDetails).AssociatedClients

                                    Dim rptrFolderNavigation As Repeater = e.Row.FindControl("rptrAssociateClient")
                                    If rptrFolderNavigation IsNot Nothing Then
                                            rptrFolderNavigation.DataSource = xmldoc.SelectNodes("/Associates/Associate")
                                            rptrFolderNavigation.DataBind()
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
            End If
        End Sub

#End Region

#Region " Find Insurance - Private Methods "
        Private Sub ClearInsuranceFileSearchCriteria()
            'Reset the values
            txtPolicyNumber.Text = String.Empty
            txtCoverNote.Text = String.Empty
            txtRiskIndex.Text = String.Empty
            txtPostCode.Text = String.Empty
            txtShortName.Text = String.Empty
            Claims_FindInsuranceFile__Claim_Date.Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper()
            Claims_FindInsuranceFile__ForceFrom.Text = String.Empty
            Claims_FindInsuranceFile_ForceTo.Text = String.Empty
            Session.Remove(CNClaimFlag)
        End Sub
#End Region

        Protected Sub custVldForceDate_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles custVldForceDate.ServerValidate
            'Validation of Force from and Force to date
            If Claims_FindInsuranceFile__ForceFrom.Text.Trim.Length <> 0 And IsDate(Claims_FindInsuranceFile__ForceFrom.Text.Trim) = False Then
                args.IsValid = False
                custVldForceDate.ErrorMessage = GetLocalResourceObject("Err_InvalidForceFromDate")
            ElseIf Claims_FindInsuranceFile_ForceTo.Text.Trim.Length <> 0 And IsDate(Claims_FindInsuranceFile_ForceTo.Text.Trim) = False Then
                args.IsValid = False
                custVldForceDate.ErrorMessage = GetLocalResourceObject("Err_InvalidForceToDate")
            ElseIf Claims_FindInsuranceFile__Claim_Date.Text.ToUpper.Trim = "DD/MM/YYYY" Or IsDate(Claims_FindInsuranceFile__Claim_Date.Text.Trim) = False Then
                args.IsValid = False
                custVldForceDate.ErrorMessage = GetLocalResourceObject("lbl_ClaimDate_Range_err")
            ElseIf Claims_FindInsuranceFile__ForceFrom.Text.Trim.Length <> 0 And Claims_FindInsuranceFile_ForceTo.Text.Trim.Length <> 0 Then
                If CDate(Claims_FindInsuranceFile__ForceFrom.Text.Trim) > CDate(Claims_FindInsuranceFile_ForceTo.Text.Trim) Then
                    args.IsValid = False
                    custVldForceDate.ErrorMessage = GetLocalResourceObject("Err_InvalidForceFromDate")
                End If
            ElseIf Claims_FindInsuranceFile__ForceFrom.Text.Trim.Length <> 0 And Claims_FindInsuranceFile_ForceTo.Text.Trim.Length <> 0 Then
                If CDate(Claims_FindInsuranceFile_ForceTo.Text.Trim) < CDate(Claims_FindInsuranceFile__ForceFrom.Text.Trim) Then
                    args.IsValid = False
                    custVldForceDate.ErrorMessage = GetLocalResourceObject("Err_InvalidForceToDate")
                End If
            End If
        End Sub

        Protected Sub vldRiskIndex_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles vldRiskIndex.ServerValidate
            If txtRiskIndex.Text.Length <> 0 AndAlso txtRiskIndex.Text.Contains("%") Then
                args.IsValid = False
            Else
                args.IsValid = True
            End If
        End Sub
        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If HttpContext.Current.Session.IsCookieless Then
                btnShortName.OnClientClick = "tb_show(null ,'../secure/agent/FindClient.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Open&TB_iframe=true&height=500&width=750' , null);return false;"
            Else
                btnShortName.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "secure/agent/FindClient.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Open&TB_iframe=true&height=500&width=750' , null);return false;"
            End If
        End Sub
        Protected Sub GetSystemOption()
            Dim oAllowPolicyClientAssociationsOptionSettings As NexusProvider.OptionTypeSetting
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

            oAllowPolicyClientAssociationsOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.AllowPolicyClientAssociations)
            ViewState("AllowPolicyClientAssociationsOptionSettings") = oAllowPolicyClientAssociationsOptionSettings

            oWebService = Nothing
        End Sub
        Public Function GetPolicyForClaimDate(ByVal v_oPolicies As NexusProvider.PolicyCollection, ByVal v_dtClaimDate As Date, ByRef r_lInsuranceFileCnt As Integer, ByRef r_sPolicyNumber As String, ByRef r_dtStartDate As Date, ByRef r_dtEndDate As Date, Optional ByRef r_lReturnCode As Integer = 0, Optional ByRef r_dtInceptionDate As Date = #12:00:00 PM#, Optional ByRef r_sTypeCode As String = "") As Integer

            Dim result As Integer = 0
            Dim lCurrentPosition As Integer

            Dim dtFirstStartDate, dtLastExpiryDate As Date
            Dim bFoundDate As Boolean

            Const ReturnCode_Error As Integer = 0
            Const ReturnCode_Ok As Integer = 1
            Const ReturnCode_TooEarly As Integer = 2
            Const ReturnCode_TooLate As Integer = 3



            For lCount As Integer = 0 To v_oPolicies.Count - 1

                If r_lInsuranceFileCnt = v_oPolicies.Item(lCount).InsuranceFileKey Then

                    r_dtStartDate = CDate(v_oPolicies.Item(lCount).CoverStartDate)

                    r_dtEndDate = CDate(v_oPolicies.Item(lCount).ExpiryDate)
                    bFoundDate = True
                    Exit For
                End If
            Next lCount

            ' if we havent found a date yet then
            If Not bFoundDate Then

                For lCount As Integer = 0 To v_oPolicies.Count - 1
                    If (v_oPolicies.Item(lCount).InsuranceFileTypeKey = 2 Or v_oPolicies.Item(lCount).InsuranceFileTypeKey = 5 Or v_oPolicies.Item(lCount).InsuranceFileTypeKey = 6 Or v_oPolicies.Item(lCount).InsuranceFileTypeKey = 8 Or v_oPolicies.Item(lCount).InsuranceFileTypeKey = 9) Then

                        r_dtStartDate = CDate(v_oPolicies.Item(lCount).CoverStartDate)

                        r_dtEndDate = CDate(v_oPolicies.Item(lCount).ExpiryDate)
                        bFoundDate = True
                        Exit For
                    End If
                Next lCount
            End If

            ' Find a version of the policy which encompasses the claim date
            lCurrentPosition = -1
            If v_oPolicies IsNot Nothing AndAlso v_oPolicies.Count > 0 Then
                v_oPolicies.SortColumn = "CoverStartDate"
                v_oPolicies.SortObjectType = v_oPolicies.Item(0).GetType()
                v_oPolicies.Sort()
            End If

            For lCount As Integer = v_oPolicies.Count - 1 To 0 Step -1

                If (v_oPolicies.Item(lCount).InsuranceFileTypeKey = 2 Or v_oPolicies.Item(lCount).InsuranceFileTypeKey = 5 Or v_oPolicies.Item(lCount).InsuranceFileTypeKey = 6 Or v_oPolicies.Item(lCount).InsuranceFileTypeKey = 8 Or v_oPolicies.Item(lCount).InsuranceFileTypeKey = 9) Then
                    If (v_oPolicies.Item(lCount).CoverStartDate <= v_dtClaimDate) And (v_dtClaimDate <= v_oPolicies.Item(lCount).ExpiryDate) Then

                        ' We found a valid one, note its position and exit!
                        lCurrentPosition = lCount
                        Exit For
                    End If

                    ' Save the earliest start date and the latest expiry date

                    If CDate(v_oPolicies.Item(lCount).CoverStartDate) < dtFirstStartDate Or lCount = 0 Then

                        dtFirstStartDate = CDate(v_oPolicies.Item(lCount).CoverStartDate)
                    End If

                    If CDate(v_oPolicies.Item(lCount).ExpiryDate) > dtLastExpiryDate Or lCount = 0 Then

                        dtLastExpiryDate = CDate(v_oPolicies.Item(lCount).ExpiryDate)
                    End If
                End If
            Next lCount

            r_lInsuranceFileCnt = 0
            r_dtInceptionDate = dtFirstStartDate

            If lCurrentPosition <> -1 Then

                ' Found one, we returns its details

                r_sPolicyNumber = v_oPolicies.Item(lCurrentPosition).InsuranceFileRef

                r_lInsuranceFileCnt = v_oPolicies.Item(lCurrentPosition).InsuranceFileKey

                r_sTypeCode = v_oPolicies.Item(lCurrentPosition).InsuranceFileTypeCode

                r_lReturnCode = ReturnCode_Ok

            Else

                ' Else, we check why we haven't found one, to report to user
                If v_dtClaimDate > dtLastExpiryDate Then
                    ' The claim date if after the latest expiry date
                    r_lReturnCode = ReturnCode_TooLate
                ElseIf v_dtClaimDate < dtFirstStartDate Then
                    ' The claim date is before the earliest start date
                    r_lReturnCode = ReturnCode_TooEarly
                Else
                    ' We can't find any policy, probably because the claim date is in between
                    ' two valid policy versions
                    r_lReturnCode = ReturnCode_Error
                End If

            End If

            Return result

        End Function
        Private Function GetInsuranceFileDetails() As NexusProvider.InsuranceFileDetailsCollection

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oInsuranceFileDetails As NexusProvider.InsuranceFileDetailsCollection = _
                            CType(Cache.Item(ViewState("InsuranceFileDetailspageCacheID")), NexusProvider.InsuranceFileDetailsCollection)

            If oInsuranceFileDetails Is Nothing Then
                'Get search results by calling FindInsuranceFileForClaims
                Dim oInsuranceFileSearchCriteria As New NexusProvider.InsuranceFileDetails
                Dim oGrdInsuranceFileDetails As New NexusProvider.InsuranceFileDetailsCollection
                Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
                Dim sBranchCode As String = oUserDetails.ListOfBranches(0).Code

                With oInsuranceFileSearchCriteria
                    .InsuranceRef = Trim(txtPolicyNumber.Text)
                    .CoverNoteSheetNumber = CInt(IIf(String.IsNullOrEmpty(Trim(txtCoverNote.Text)), 0, txtCoverNote.Text))
                    .RiskIndex = Trim(txtRiskIndex.Text)
                    If rqdClaimDate.Enabled = True Then
                        .LossDate = CDate(Trim(Claims_FindInsuranceFile__Claim_Date.Text))
                        .SearchDate = CDate(Trim(Claims_FindInsuranceFile__Claim_Date.Text))
                    Else
                        .SearchDate = Date.Now
                        .LossDate = Date.Now
                    End If

                    .ClientShortName = Trim(txtShortName.Text)
                    .PostCode = Trim(txtPostCode.Text)
                    .InForceFrom = CDate(IIf(String.IsNullOrEmpty(Trim(Claims_FindInsuranceFile__ForceFrom.Text)), DateTime.MinValue, Claims_FindInsuranceFile__ForceFrom.Text))
                    .InForceTo = CDate(IIf(String.IsNullOrEmpty(Trim(Claims_FindInsuranceFile_ForceTo.Text)), DateTime.MinValue, Claims_FindInsuranceFile_ForceTo.Text))

                    'to limit the search return from SAM
                    oInsuranceFileSearchCriteria.MaxRowsToFetch = oPortal.MaxSearchResults
                End With

                oInsuranceFileDetails = oWebService.FindInsuranceFileForClaims(oInsuranceFileSearchCriteria, sBranchCode)
                Cache.Insert(ViewState("InsuranceFileDetailspageCacheID"), oInsuranceFileDetails, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
            End If

            Return oInsuranceFileDetails
        End Function

        Protected Sub grdvInsuranceFile_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdvInsuranceFile.Sorting


            Dim oInsuranceFileDetails As NexusProvider.InsuranceFileDetailsCollection =
            CType(Cache.Item(ViewState("InsuranceFileDetailspageCacheID")), NexusProvider.InsuranceFileDetailsCollection)

            oInsuranceFileDetails.SortColumn = e.SortExpression
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
            oInsuranceFileDetails.SortingOrder = _sortDirection
            oInsuranceFileDetails.Sort()
            CType(sender, GridView).DataSource = oInsuranceFileDetails
            CType(sender, GridView).DataBind()
        End Sub
        
    End Class
End Namespace

