Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Utils
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Linq

Namespace Nexus
    Partial Class secure_FindPolicy
        Inherits Frontend.clsCMSPage

        Const CNPolicyCollection As String = "PolicyCollection"
        Const CNExistingQuotes As String = "ExistingQuotes"

        Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "UnMarkedConfirmation",
                       "function UnMarkedConfirmation(){var IsConfirm; IsConfirm=confirm('" & GetLocalResourceObject("msg_ConfirmUnMarkedCollection").ToString() & "');document.getElementById('" & hfAnswerForMarkedQuoteCollection.ClientID & "').value=IsConfirm;return IsConfirm;}", True)
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "UnReInstatementConfirmation",
                      "function UnReInstatementConfirmation(){var IsConfirm; IsConfirm=confirm('" & GetLocalResourceObject("msg_ConfirmReInstatement").ToString() & "');return IsConfirm;}", True)
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "MarketPlacePolicyConfirmation",
                        "function MarketPlacePolicyConfirmation(){var IsConfirm; IsConfirm=confirm('" & GetLocalResourceObject("msg_ConfirmMarketPlacePolicy1").ToString() & "'); if(IsConfirm==true) { IsConfirm=confirm('" & GetLocalResourceObject("msg_ConfirmMarketPlacePolicy2").ToString() & "'); if(IsConfirm==true) { document.getElementById('" & hfAnswerForMarketPlacePolicy.ClientID & "').value = false; } return IsConfirm; } else {return IsConfirm;} }", True)
						Session(CNFinancePlanDetails)=Nothing

        End Sub

        ''' <summary>
        ''' Bind products, enable/disable mandatory fields and different postback events from versions dialog
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Load1(sender As Object, e As EventArgs) Handles Me.Load
            Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
            If oUserDetails IsNot Nothing Then
                If oUserDetails.PartyKey <> 0 Then
                    btnAgent.Enabled = False
                    hfAgentKey.Value = oUserDetails.PartyKey
                End If
            End If

            Dim eventArgument As String = Request("__EVENTARGUMENT")
            If Not IsNothing(eventArgument) Then
                eventArgument = eventArgument.Trim.ToUpper()
            End If

            If Not IsPostBack orelse eventArgument = "SELECTBRANCH" Then
                BindProducts()
                btnNewSearch_Click(btnNewSearch,Nothing)
            End If

            If Not Page.IsPostBack Then
                txtAgentName.Attributes.Add("readonly", "readonly")
                EnableMandatoryFieldValidations()

                GetSystemOption()

                If Request.QueryString("Mode") IsNot Nothing AndAlso Request.QueryString("Mode") = "ManualTransfer" Then
                    Session(CNNoTrans) = "MTA"
                    lblPageHeader.Text = GetLocalResourceObject("lblNoTransHeader")
                    lblPolicyNumber.Text = GetLocalResourceObject("lbl_NoTransPolicyNumber")
                    ddlRecordType.SelectedValue = "POLICY"
                    ddlRecordType.Enabled = False
                Else
                    Session(CNNoTrans) = Nothing
                End If
            End If
            LoadPageFromSessionFilters()
            If (Not IsPostBack) Or (Request("__EVENTARGUMENT") IsNot Nothing) Then
                If hfPolicyVersionInsuranceFileKey.Value <> "" Then
                    Dim nInsuranceFileKey As Integer = Convert.ToInt32(hfPolicyVersionInsuranceFileKey.Value)
                    Select Case Request("__EVENTARGUMENT")
                        Case "View", "viewpolicy"
                            QuotePolicyActions.ViewQuoteAndPolicy("viewpolicy", nInsuranceFileKey)
                        Case "ViewMTA", "viewMTA"
                            QuotePolicyActions.ViewQuoteAndPolicy("viewMTA", nInsuranceFileKey)
                        Case "Reinstatement"
                            QuotePolicyActions.Reinstate("Reinstatement", nInsuranceFileKey, Convert.ToBoolean(hfAnswerForMarkedQuoteCollection.Value))
                        Case "EditQuote", "editmtaquote"
                            QuotePolicyActions.EditQuote(Request("__EVENTARGUMENT"), nInsuranceFileKey, Convert.ToBoolean(hfAnswerForMarketPlacePolicy.Value), Convert.ToBoolean(hfAnswerForMarkedQuoteCollection.Value))
                        Case "Change", "MTAquote"
                            QuotePolicyActions.MTA("MTAquote", nInsuranceFileKey, Convert.ToBoolean(hfAnswerForMarketPlacePolicy.Value), Convert.ToBoolean(hfAnswerForMarkedQuoteCollection.Value))
                        Case "Renew"
                            QuotePolicyActions.RenewQuote("viewDetails", nInsuranceFileKey)
                    End Select
                End If
            End If
        End Sub

        ''' <summary>
        ''' Set agent modal dialog
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If HttpContext.Current.Session.IsCookieless Then
                btnAgent.OnClientClick = "tb_show(null ,'../Modal/FindAgent.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            Else
                btnAgent.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "Modal/FindAgent.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            End If
        End Sub

        ''' <summary>
        ''' Search latest policy versions for given criteria
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
            Dim oNexusConfig As Config.NexusFrameWork
            Dim oPortal As Nexus.Library.Config.Portal
            Dim oInsuranceFiles As NexusProvider.InsuranceFileDetailsCollection = Nothing
            Dim oInsuranceFilteredFiles As NexusProvider.InsuranceFileDetailsCollection = Nothing
            Dim oWebService As NexusProvider.ProviderBase

            Try
                Dim oRecordType As NexusProvider.InsuranceQuoteType = NexusProvider.InsuranceQuoteType.ALL
                oNexusConfig = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                oPortal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
                oWebService = New NexusProvider.ProviderManager().Provider

                txtAgentName.Text = hfAgentName.Value

                If ddlRecordType.SelectedValue = "QUOTE" Then
                    oRecordType = NexusProvider.InsuranceQuoteType.QUOTE
                ElseIf ddlRecordType.SelectedValue = "POLICY" Then
                    oRecordType = NexusProvider.InsuranceQuoteType.POLICY
                End If

                oInsuranceFiles = oWebService.FindLatestPolicyVersions(txtPolicyNumber.Text.Trim, ddlProductType.SelectedValue, oRecordType, IIf(hfAgentKey.Value = "", 0, hfAgentKey.Value),
                                              txtInsuredName.Text, oPortal.MaxSearchResults, IIf(txtCoverStartDate.Text = "", Nothing, txtCoverStartDate.Text),
                                              IIf(txtQuoteDate.Text = "", Nothing, txtQuoteDate.Text), v_sRiskIndex:=IIf(txtRiskIndex.Text = "", Nothing, txtRiskIndex.Text))

                If (Session(CNNoTrans) IsNot Nothing AndAlso oInsuranceFiles IsNot Nothing AndAlso oInsuranceFiles.Count > 0) Then
                    oInsuranceFilteredFiles = New NexusProvider.InsuranceFileDetailsCollection
                    For Each oInsuranceFilesItem As NexusProvider.InsuranceFileDetails In oInsuranceFiles
                        If oInsuranceFilesItem.InsuranceFileTypeCode.Trim.ToUpper() <> "QUOTE" Then
                            oInsuranceFilteredFiles.Add(oInsuranceFilesItem)
                        End If
                    Next
                    oInsuranceFiles = oInsuranceFilteredFiles
                    lblPageHeader.Text = GetLocalResourceObject("lblNoTransHeader")
                    lblPolicyNumber.Text = GetLocalResourceObject("lbl_NoTransPolicyNumber")
                    lblQuoteDate.Text = GetLocalResourceObject("lblNoTransQuoteLiveDate")
                End If

                'Maintain policy search form data and result set
                Dim sPolicySearch As New NexusProvider.PolicySearchCriteria

                sPolicySearch.RecordType = ddlRecordType.SelectedValue
                sPolicySearch.PolicyNumber = txtPolicyNumber.Text.Trim()
                sPolicySearch.product = ddlProductType.SelectedValue
                sPolicySearch.AgentName = txtAgentName.Text
                sPolicySearch.QuoteDate = CDate(IIf(Trim(txtQuoteDate.Text) <> String.Empty, Trim(txtQuoteDate.Text), DateTime.MinValue))

                sPolicySearch.StartDate = CDate(IIf(Trim(txtCoverStartDate.Text) <> String.Empty, Trim(txtCoverStartDate.Text), DateTime.MinValue))
                sPolicySearch.InsuredName = txtInsuredName.Text
                If Session(CNPolicyBackButton) IsNot Nothing Then
                    Session.Remove(CNPolicyBackButton)
                End If
                Session(CNFindPolicySearchData) = sPolicySearch
                Session(CNPolicyBackButton) = "FindPolicy"

                ViewState.Add(CNPolicyCollection, oInsuranceFiles)

                If oInsuranceFiles IsNot Nothing AndAlso oInsuranceFiles.Count >= oPortal.MaxSearchResults Then
                    'create a custom validator
                    Dim cstMaxResults As New CustomValidator
                    cstMaxResults.IsValid = False
                    'look for a validation message in the page resources, but if there is not one defined add a default message
                    cstMaxResults.ErrorMessage = GetLocalResourceObject("lbl_warnignMaxResults")
                    cstMaxResults.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                    'add the validator to the page, this will have the effect of making the page invalid
                    Page.Validators.Add(cstMaxResults)
                End If

                pnlSearchResult.Visible = True
                grdSearchResult.AllowPaging = True
                grdSearchResult.PageIndex = 0
                grdSearchResult.DataSource = oInsuranceFiles
                grdSearchResult.DataBind()

                If oInsuranceFiles IsNot Nothing AndAlso oInsuranceFiles.Count = 1 AndAlso oInsuranceFiles(0).NoOfVersions > 1 Then
                    Dim sUrl As String
                    If HttpContext.Current.Session.IsCookieless Then
                        sUrl = "../Modal/PolicyVersions.aspx?PostbackTo=" & updSearchResults.ClientID & "&InsuranceFolderKey=" & oInsuranceFiles(0).InsuranceFolderKey.ToString()
                    Else
                        sUrl = AppSettings("WebRoot") & "Modal/PolicyVersions.aspx?PostbackTo=" & updSearchResults.ClientID & "&InsuranceFolderKey=" & oInsuranceFiles(0).InsuranceFolderKey.ToString()
                    End If
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "ShowVersions", "ShowPolicyVersionModal('" & sUrl & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750');", True)
                End If
            Finally
                oNexusConfig = Nothing
                oPortal = Nothing
                oWebService = Nothing
            End Try
        End Sub

        Private Sub LoadPageFromSessionFilters()
            If Session(CNPolicyBackButton) IsNot Nothing AndAlso Session(CNPolicyBackButton) = "BackButton" Then
                If Session(CNFindPolicySearchData) IsNot Nothing Then
                    Dim sPolicySearch As New NexusProvider.PolicySearchCriteria
                    sPolicySearch = CType(Session(CNFindPolicySearchData), NexusProvider.PolicySearchCriteria)
                    Dim QuoteDate As String = sPolicySearch.QuoteDate.ToString()
                    QuoteDate = IIf(QuoteDate = "00:00:00", "", QuoteDate.Remove(10))
                    Dim StartDate As String = sPolicySearch.StartDate
                    StartDate = IIf(StartDate = "00:00:00", "", StartDate)
                    txtPolicyNumber.Text = sPolicySearch.PolicyNumber
                    ddlProductType.SelectedValue = sPolicySearch.product
                    ddlRecordType.SelectedValue = sPolicySearch.RecordType
                    txtAgentName.Text = sPolicySearch.AgentName
                    txtQuoteDate.Text = QuoteDate
                    txtCoverStartDate.Text = StartDate
                    BindLatestPolicyFromSession()
                End If
            End If
        End Sub


        Private Sub BindLatestPolicyFromSession()
            Dim oNexusConfig As Config.NexusFrameWork
            Dim oPortal As Nexus.Library.Config.Portal
            Dim oInsuranceFiles As NexusProvider.InsuranceFileDetailsCollection = Nothing
            Dim oInsuranceFilteredFiles As NexusProvider.InsuranceFileDetailsCollection = Nothing
            Dim oWebService As NexusProvider.ProviderBase

            Try
                Dim oRecordType As NexusProvider.InsuranceQuoteType = NexusProvider.InsuranceQuoteType.ALL
                oNexusConfig = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                oPortal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
                oWebService = New NexusProvider.ProviderManager().Provider

                txtAgentName.Text = hfAgentName.Value

                If ddlRecordType.SelectedValue = "QUOTE" Then
                    oRecordType = NexusProvider.InsuranceQuoteType.QUOTE
                ElseIf ddlRecordType.SelectedValue = "POLICY" Then
                    oRecordType = NexusProvider.InsuranceQuoteType.POLICY
                End If

                oInsuranceFiles = oWebService.FindLatestPolicyVersions(txtPolicyNumber.Text.Trim, ddlProductType.SelectedValue, oRecordType, IIf(hfAgentKey.Value = "", 0, hfAgentKey.Value),
                                              txtInsuredName.Text, oPortal.MaxSearchResults, IIf(txtCoverStartDate.Text = "", Nothing, txtCoverStartDate.Text),
                                              IIf(txtQuoteDate.Text = "", Nothing, txtQuoteDate.Text))
                If (Session(CNNoTrans) IsNot Nothing AndAlso oInsuranceFiles IsNot Nothing AndAlso oInsuranceFiles.Count > 0) Then
                    oInsuranceFilteredFiles = New NexusProvider.InsuranceFileDetailsCollection
                    For Each oInsuranceFilesItem As NexusProvider.InsuranceFileDetails In oInsuranceFiles
                        If oInsuranceFilesItem.InsuranceFileTypeCode.Trim.ToUpper() <> "QUOTE" Then
                            oInsuranceFilteredFiles.Add(oInsuranceFilesItem)
                        End If
                    Next
                    oInsuranceFiles = oInsuranceFilteredFiles
                    lblPageHeader.Text = GetLocalResourceObject("lblNoTransHeader")
                    lblPolicyNumber.Text = GetLocalResourceObject("lbl_NoTransPolicyNumber")
                    lblQuoteDate.Text = GetLocalResourceObject("lblNoTransQuoteLiveDate")
                End If
                ViewState.Add(CNPolicyCollection, oInsuranceFiles)

                If oInsuranceFiles IsNot Nothing AndAlso oInsuranceFiles.Count >= oPortal.MaxSearchResults Then
                    'create a custom validator
                    Dim cstMaxResults As New CustomValidator
                    cstMaxResults.IsValid = False
                    'look for a validation message in the page resources, but if there is not one defined add a default message
                    cstMaxResults.ErrorMessage = GetLocalResourceObject("lbl_warnignMaxResults")
                    cstMaxResults.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                    'add the validator to the page, this will have the effect of making the page invalid
                    Page.Validators.Add(cstMaxResults)
                End If

                pnlSearchResult.Visible = True
                grdSearchResult.AllowPaging = True
                grdSearchResult.PageIndex = 0
                grdSearchResult.DataSource = oInsuranceFiles
                grdSearchResult.DataBind()
                Session(CNPolicyBackButton) = "FindPolicy"
                If oInsuranceFiles IsNot Nothing AndAlso oInsuranceFiles.Count = 1 AndAlso oInsuranceFiles(0).NoOfVersions > 1 Then
                    Dim sUrl As String
                    If HttpContext.Current.Session.IsCookieless Then
                        sUrl = "../Modal/PolicyVersions.aspx?PostbackTo=" & updSearchResults.ClientID & "&InsuranceFolderKey=" & oInsuranceFiles(0).InsuranceFolderKey.ToString()
                    Else
                        sUrl = AppSettings("WebRoot") & "Modal/PolicyVersions.aspx?PostbackTo=" & updSearchResults.ClientID & "&InsuranceFolderKey=" & oInsuranceFiles(0).InsuranceFolderKey.ToString()
                    End If
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "ShowVersions", "tb_show( null,'" & sUrl & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);", True)
                End If
            Finally
                oNexusConfig = Nothing
                oPortal = Nothing
                oWebService = Nothing
            End Try

        End Sub
        ''' <summary>
        ''' To bind available products
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindProducts()
            Dim sAllowedAgent() As String
            Dim bIsProductAllowedToAccess As Boolean = False
            Dim UserRoles As String

            Dim oProducts As Config.Products = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Products
            Dim oWebService As NexusProvider.ProviderBase
            Dim oUserDetails As NexusProvider.UserDetails
            Dim oAgentProducts As NexusProvider.ProductCollection
            Dim oAgentToProductLinkOptionSetting As NexusProvider.OptionTypeSetting

            Try
                oUserDetails = Session(CNAgentDetails)
                oWebService = New NexusProvider.ProviderManager().Provider
                oAgentProducts = New NexusProvider.ProductCollection

                'Get System Option value for AgentToProductLink
                oAgentToProductLinkOptionSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5088)
                'If System option is ON then GET All products for logged in agent
                If oAgentToProductLinkOptionSetting.OptionValue = "1" And oUserDetails.Key > 0 Then
                    oWebService = New NexusProvider.ProviderManager().Provider
                    oAgentProducts = oWebService.GetProductByAgent()
                End If

                'On refresh clear the dropdown before repopulating
                ddlProductType.Items.Clear()

                

                For Each oProduct As Config.Product In oProducts
                    'Retreive all the roles set for product in web.config
                    UserRoles = oProduct.AllowRole
                    'Roles is available
                    If UserRoles IsNot Nothing AndAlso UserIsInRoles(UserRoles) = True _
                       AndAlso FrameWorkFunctions.IsProductAssignedToUserBranch(oProduct, CType(Session(CNAgentDetails), NexusProvider.UserDetails).AvailableUserProductsByBranch) Then
                        'if logged user is agent
                        If CType(Session(CNLoginType), LoginType) = LoginType.Agent And oUserDetails.Key > 0 Then
                            If oAgentToProductLinkOptionSetting.OptionValue = "1" Then
                                'Check that product is assigned to agent or not
                                If FrameWorkFunctions.IsProductAssignedToAgent(oProduct, oAgentProducts) Then
                                    bIsProductAllowedToAccess = True
                                End If
                            Else
                                If String.IsNullOrEmpty(oProduct.AllowedAgent.Trim) Then
                                    bIsProductAllowedToAccess = True
                                Else
                                    sAllowedAgent = oProduct.AllowedAgent.Split(",")
                                    For nCnt As Integer = 0 To sAllowedAgent.Length - 1
                                        If sAllowedAgent(nCnt).ToUpper() = oUserDetails.PartyName.ToUpper() Then
                                            bIsProductAllowedToAccess = True
                                            Exit For
                                        End If
                                    Next
                                End If
                            End If
                        Else
                            'for Direct Customer
                            bIsProductAllowedToAccess = True
                        End If
                    Else
                        'Roles is not available
                        bIsProductAllowedToAccess = False
                    End If

                    'if bMatch is True means product will be added
                    If bIsProductAllowedToAccess = True Then
                        If ddlProductType.Items.Count = 0 Then
                            If rfvProduct.Enabled = True Then
                                ddlProductType.Items.Add(New ListItem(GetLocalResourceObject("li_PleaseSelectProduct"), "All"))
                            Else
                                ddlProductType.Items.Add(New ListItem(GetLocalResourceObject("li_All"), "All"))
                            End If
                        End If
                        ddlProductType.Items.Add(New ListItem(oProduct.Name, oProduct.ProductCode))
                    End If
                    bIsProductAllowedToAccess = False
                Next
            Finally
                oUserDetails = Nothing
                oWebService = Nothing
                oAgentProducts = Nothing
            End Try
        End Sub

        ''' <summary>
        ''' Enable / disable paging on basis of search results
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdSearchResult_DataBound(sender As Object, e As EventArgs) Handles grdSearchResult.DataBound
            If grdSearchResult.Rows.Count = 0 Or grdSearchResult.PageCount = 1 Then
                grdSearchResult.AllowPaging = False
            Else
                grdSearchResult.AllowPaging = True
                grdSearchResult.PageSize = 10
            End If
        End Sub

        ''' <summary>
        ''' To set current page index
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdSearchResult_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdSearchResult.PageIndexChanging
            grdSearchResult.PageIndex = e.NewPageIndex
            grdSearchResult.DataSource = ViewState(CNPolicyCollection)
            grdSearchResult.DataBind()
        End Sub

        ''' <summary>
        ''' To handle different policy actions
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdSearchResult_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdSearchResult.RowCommand
            If Not LCase(e.CommandName).Equals("page") And Not LCase(e.CommandName).Equals("sort") Then
                Dim nInsuranceFileKeyForAction As Integer = Convert.ToInt32(e.CommandArgument)

                Select Case e.CommandName
                    Case "View"

                    Case Else
                        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                        Dim oExclusiveLocking As NexusProvider.OptionTypeSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ExclusiveLock)
                        If oExclusiveLocking.OptionValue = "1" Then

                            Dim oInsuranceFiles As NexusProvider.InsuranceFileDetailsCollection = ViewState(CNPolicyCollection)
                            Dim oPolicyDetail = (From oInsuranceFile In oInsuranceFiles
                                                 Where oInsuranceFile.InsuranceFileKey = nInsuranceFileKeyForAction
                                                 Select oInsuranceFile).FirstOrDefault

                            Dim sUserName As String = CheckLock(oPolicyDetail.InsuranceFolderKey, Session(CNBranchCode).ToString)
                            If sUserName.Trim.Length > 0 Then
                                Dim sMessage As String = "alert('" + Replace(GetLocalResourceObject("lbl_policylocked_error"), "{1}", sUserName + ".") + "')"
                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", sMessage, True)
                                Return
                            End If

                            Dim oQuote As NexusProvider.Quote
                            oQuote = oWebService.GetHeaderAndSummariesByKey(nInsuranceFileKeyForAction, bExclusiveLock:=True)
                            oWebService = Nothing
                            oPolicyDetail = Nothing
                        End If
                End Select

                Select Case e.CommandName
                    Case "View", "viewpolicy"
                        QuotePolicyActions.ViewQuoteAndPolicy("viewpolicy", nInsuranceFileKeyForAction)
                    Case "ViewMTA", "viewMTA"
                        QuotePolicyActions.ViewQuoteAndPolicy("viewpolicy", nInsuranceFileKeyForAction)
                    Case "Reinstatement"
                        'MTAQREINS ,MTAQCAN    
                        Dim oInsuranceFiles As NexusProvider.InsuranceFileDetailsCollection = ViewState(CNPolicyCollection)
                        Dim oPolicyDetail = (From oInsuranceFile In oInsuranceFiles
                                             Where oInsuranceFile.InsuranceFileKey = nInsuranceFileKeyForAction
                                             Select oInsuranceFile).FirstOrDefault

                        If GetExistingQuotes(oPolicyDetail.InsuranceFolderKey, "MTAQREINS", nInsuranceFileKeyForAction) > 0 Then
                            Dim sUrl As String
                            If HttpContext.Current.Session.IsCookieless Then
                                sUrl = "../Modal/PolicyVersions.aspx?PostbackTo=" & updSearchResults.ClientID & "&InsuranceFolderKey=" & oPolicyDetail.InsuranceFolderKey.ToString() & "&InsuranceFileKey=" & nInsuranceFileKeyForAction.ToString() & "&ShowExistingQuote=true&StatusCode=MTAQREINS"
                            Else
                                sUrl = AppSettings("WebRoot") & "Modal/PolicyVersions.aspx?PostbackTo=" & updSearchResults.ClientID & "&InsuranceFolderKey=" & oPolicyDetail.InsuranceFolderKey.ToString() & "&InsuranceFileKey=" & nInsuranceFileKeyForAction.ToString() & "&ShowExistingQuote=true&StatusCode=MTAQREINS"
                            End If
                            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "ShowVersions", "$(document).ready(function(){tb_show( null,'" & sUrl & "' , null);});", True)
                        Else
                            QuotePolicyActions.Reinstate("Reinstatement", nInsuranceFileKeyForAction, Convert.ToBoolean(hfAnswerForMarkedQuoteCollection.Value))
                        End If
                    Case "EditQuote", "editmtaquote"
                        QuotePolicyActions.EditQuote("editquote", nInsuranceFileKeyForAction, Convert.ToBoolean(hfAnswerForMarketPlacePolicy.Value), Convert.ToBoolean(hfAnswerForMarkedQuoteCollection.Value))

                    Case "Change", "MTAquote"
                        Dim oInsuranceFiles As NexusProvider.InsuranceFileDetailsCollection = ViewState(CNPolicyCollection)
                        Dim oPolicyDetail = (From oInsuranceFile In oInsuranceFiles
                                             Where oInsuranceFile.InsuranceFileKey = nInsuranceFileKeyForAction
                                             Select oInsuranceFile).FirstOrDefault

                        If GetExistingQuotes(oPolicyDetail.InsuranceFolderKey, "MTAQUOTE,MTAQCAN", nInsuranceFileKeyForAction) > 0 Then
                            Dim sUrl As String
                            If HttpContext.Current.Session.IsCookieless Then
                                sUrl = "../Modal/PolicyVersions.aspx?PostbackTo=" & updSearchResults.ClientID & "&InsuranceFolderKey=" & oPolicyDetail.InsuranceFolderKey.ToString() & "&InsuranceFileKey=" & oPolicyDetail.InsuranceFileKey.ToString() & "&ShowExistingQuote=true&StatusCode=MTAQUOTE,MTAQCAN"
                            Else
                                sUrl = AppSettings("WebRoot") & "Modal/PolicyVersions.aspx?PostbackTo=" & updSearchResults.ClientID & "&InsuranceFolderKey=" & oPolicyDetail.InsuranceFolderKey.ToString() & "&InsuranceFileKey=" & oPolicyDetail.InsuranceFileKey.ToString() & "&ShowExistingQuote=true&StatusCode=MTAQUOTE,MTAQCAN"
                            End If
                            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "ShowVersions", "$(document).ready(function(){tb_show( null,'" & sUrl & "' , null);});", True)
                        Else
                            QuotePolicyActions.MTA("MTAquote", Convert.ToInt32(e.CommandArgument), Convert.ToBoolean(hfAnswerForMarketPlacePolicy.Value), Convert.ToBoolean(hfAnswerForMarkedQuoteCollection.Value))
                        End If
                    Case "Renew"
                        Dim oInsuranceFiles As NexusProvider.InsuranceFileDetailsCollection = ViewState(CNPolicyCollection)
                        Dim oPolicyDetail = (From oInsuranceFile In oInsuranceFiles
                                             Where oInsuranceFile.InsuranceFileKey = nInsuranceFileKeyForAction
                                             Select oInsuranceFile).FirstOrDefault

                        If GetExistingQuotes(oPolicyDetail.InsuranceFolderKey, "RENEWAL", nInsuranceFileKeyForAction) > 1 Then
                            Dim sUrl As String
                            If HttpContext.Current.Session.IsCookieless Then
                                sUrl = "../Modal/PolicyVersions.aspx?PostbackTo=" & updSearchResults.ClientID & "&InsuranceFolderKey=" & oPolicyDetail.InsuranceFolderKey.ToString() & "&InsuranceFileKey=" & nInsuranceFileKeyForAction.ToString() & "&ShowExistingQuote=true&StatusCode=RENEWAL"
                            Else
                                sUrl = AppSettings("WebRoot") & "Modal/PolicyVersions.aspx?PostbackTo=" & updSearchResults.ClientID & "&InsuranceFolderKey=" & oPolicyDetail.InsuranceFolderKey.ToString() & "&InsuranceFileKey=" & nInsuranceFileKeyForAction.ToString() & "&ShowExistingQuote=true&StatusCode=RENEWAL"
                            End If
                            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "ShowVersions", "$(document).ready(function(){tb_show( null,'" & sUrl & "' , null);});", True)
                        Else
                            QuotePolicyActions.RenewQuote("viewDetails", nInsuranceFileKeyForAction)
                        End If
                End Select

            End If
        End Sub

        ''' <summary>
        ''' To clear all search criteria
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnNewSearch_Click(sender As Object, e As EventArgs) Handles btnNewSearch.Click

            hfAgentKey.Value = ""
            hfAgentName.Value = ""
            txtAgentName.Text = ""
            txtCoverStartDate.Text = ""
            txtInsuredName.Text = ""
            txtPolicyNumber.Text = ""
            txtQuoteDate.Text = ""
            ddlProductType.SelectedIndex = 0
            ddlRecordType.SelectedIndex = 0

            pnlSearchResult.Visible = False
            grdSearchResult.DataSource = Nothing
            grdSearchResult.DataBind()
        End Sub

        ''' <summary>
        ''' To enable mandatory field validations
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub EnableMandatoryFieldValidations()
            Dim oNexusConfig As Config.NexusFrameWork
            Dim oPortal As Nexus.Library.Config.Portal

            Try
                oNexusConfig = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                oPortal = oNexusConfig.Portals.Portal(Portal.GetPortalID())

                If oPortal.FindPolicyMandatoryFields <> String.Empty Then
                    Dim sMandatoryFields() As String = oPortal.FindPolicyMandatoryFields.Split(",")
                    For Each sMandatoryField In sMandatoryFields
                        'RecordType,Product,PolicyReference,AgentName,InsuredName,QuoteOrLiveDate,CoverStartDate
                        Select Case sMandatoryField
                            Case "RecordType"
                                rfvRecordType.Enabled = True
                            Case "Product"
                                rfvProduct.Enabled = True
                            Case "PolicyReference"
                                rfvPolicyNumber.Enabled = True
                            Case "AgentName"
                                rfvAgentName.Enabled = True
                            Case "InsuredName"
                                rfvInsuredName.Enabled = True
                            Case "QuoteOrLiveDate"
                                rfvQuoteDate.Enabled = True
                            Case "CoverStartDate"
                                rfvCoverStartDate.Enabled = True
                        End Select
                    Next
                End If
            Finally
                oNexusConfig = Nothing
                oPortal = Nothing
            End Try

        End Sub

        Protected Sub grdSearchResult_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles grdSearchResult.RowCreated
            If (e.Row.RowType = DataControlRowType.Header OrElse e.Row.RowType = DataControlRowType.DataRow) Then
                Dim oAllowPolicyClientAssociationsOptionSettings As NexusProvider.OptionTypeSetting = CType(ViewState("AllowPolicyClientAssociationsOptionSettings"), NexusProvider.OptionTypeSetting)

                'Hide the PolicyAssociate Column if the Hidden option to show PolicyClientAssociate is False
                If oAllowPolicyClientAssociationsOptionSettings IsNot Nothing AndAlso oAllowPolicyClientAssociationsOptionSettings.OptionValue = "1" Then
                    grdSearchResult.Columns(7).Visible = True
                Else
                    grdSearchResult.Columns(7).Visible = False
                End If
            End If
        End Sub

        ''' <summary>
        ''' To show/hide or enable/disable policy actions
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdSearchResult_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdSearchResult.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim oPolicyDetail As NexusProvider.InsuranceFileDetails = CType(e.Row.DataItem, NexusProvider.InsuranceFileDetails)
                Dim lnkbtnVersions As LinkButton = e.Row.FindControl("lnkbtnVersions")
                Dim btnView As LinkButton = e.Row.FindControl("btnView")
                Dim lnkbtnAction As LinkButton = e.Row.FindControl("lnkbtnAction")
                Dim liRenew As HtmlControl = e.Row.FindControl("liRenew")
                Dim lnkbtnRenew As LinkButton = e.Row.FindControl("lnkbtnRenew")

                If lnkbtnVersions.Enabled Then
                    If HttpContext.Current.Session.IsCookieless Then
                        lnkbtnVersions.OnClientClick = "tb_show(null ,'../Modal/PolicyVersions.aspx?PostbackTo=" & updSearchResults.ClientID & "&InsuranceFolderKey=" & oPolicyDetail.InsuranceFolderKey.ToString() & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                    Else
                        lnkbtnVersions.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "Modal/PolicyVersions.aspx?PostbackTo=" & updSearchResults.ClientID & "&InsuranceFolderKey=" & oPolicyDetail.InsuranceFolderKey.ToString() & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                    End If
                End If

                If Not UserCanDoTask("ViewQuote") Then
                    btnView.Enabled = False
                End If

                If oPolicyDetail.IsReadOnly Then
                    lnkbtnAction.Enabled = False
                Else
                    If oPolicyDetail.IsCancelled Then
                        If UserCanDoTask("MidTermReinstatement") Then
                            lnkbtnAction.CommandName = "Reinstatement"
                            lnkbtnAction.Text = GetLocalResourceObject("lbl_Reinstate")
                            lnkbtnAction.Attributes.Add("OnClick", "javascript:return UnReInstatementConfirmation();")
                        Else
                            lnkbtnAction.Enabled = False
                        End If
                    ElseIf oPolicyDetail.InsuranceFileTypeCode.Trim = "QUOTE" Or oPolicyDetail.InsuranceFileTypeCode.Trim = "WRITTEN" Then
                        'Check if any non-quote policy version is in LIVE state
                        If HasLivePolicyVersion(oPolicyDetail.InsuranceFolderKey) Then
                            lnkbtnAction.Enabled = False
                        ElseIf UserCanDoTask("NewBusiness") Then
                            lnkbtnAction.CommandName = "EditQuote"
                            lnkbtnAction.Text = GetLocalResourceObject("lbl_Edit")
                            If (oPolicyDetail.QuoteExpiryDate < DateTime.Now) Then
                                lnkbtnAction.Attributes.Add("OnClick", "alert('" + GetLocalResourceObject("msg_QuoteExpired") + "');")
                            End If
                        Else
                            lnkbtnAction.Enabled = False
                        End If
                    ElseIf oPolicyDetail.InsuranceFileTypeCode.Trim = "POLICY" Then
                        If UserCanDoTask("MidTermAdjustment") Then
                            lnkbtnAction.CommandName = "Change"
                            lnkbtnAction.Text = GetLocalResourceObject("lbl_Change")
                        Else
                            lnkbtnAction.Enabled = False
                        End If

                    End If
                    If oPolicyDetail.NoOfRenewalVersions > 0 Then
                        If UserCanDoTask("Renewals") Then
                            liRenew.Visible = True
                            If IsRenewalQuoteMarkedForCollection(oPolicyDetail.InsuranceFolderKey) = True Then
                                lnkbtnRenew.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                            End If
                        Else
                            liRenew.Visible = False
                        End If
                    End If

                        If lnkbtnAction.Enabled = True Then
                        If oPolicyDetail.MarkedQuoteForCollection Then
                            lnkbtnAction.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                        End If
                        If oPolicyDetail.IsMarketPlacePolicy Then
                            lnkbtnAction.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                        End If
                    End If
                End If

                Dim oAllowPolicyClientAssociations As NexusProvider.OptionTypeSetting = ViewState("AllowPolicyClientAssociationsOptionSettings")
                If oAllowPolicyClientAssociations IsNot Nothing AndAlso oAllowPolicyClientAssociations.OptionValue = "1" Then

                    If e.Row.RowType = DataControlRowType.DataRow Then
                        Dim xmldoc As New System.Xml.XmlDocument
                        If e.Row.DataItem IsNot Nothing Then
                            If ((CType(e.Row.DataItem, NexusProvider.InsuranceFileDetails).AssociatedClients IsNot Nothing) AndAlso Not (String.IsNullOrEmpty(CType(e.Row.DataItem, NexusProvider.InsuranceFileDetails).AssociatedClients))) Then
                                Try
                                    Dim associatedClientsXml As String = CType(e.Row.DataItem, NexusProvider.InsuranceFileDetails).AssociatedClients.Trim()
                                    If associatedClientsXml.StartsWith("<") Then
                                        xmldoc.LoadXml(associatedClientsXml)
                                        Dim rptrFolderNavigation As Repeater = e.Row.FindControl("rptrAssociateClient")
                                        If rptrFolderNavigation IsNot Nothing Then
                                            rptrFolderNavigation.DataSource = xmldoc.SelectNodes("Associates/Associate")
                                            rptrFolderNavigation.DataBind()
                                        End If
                                    End If
                                Catch ex As System.Xml.XmlException
                                    ' Invalid XML - skip binding
                                End Try
                            End If
                        End If
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        '''  To get existing quotes for given status
        ''' </summary>
        ''' <param name="v_nInsuranceFolderKey"></param>
        ''' <param name="v_sStatusCode">status for quote version to filter</param>
        ''' <param name="v_nInsuranceFileKeyForAction"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetExistingQuotes(ByVal v_nInsuranceFolderKey As Integer, ByVal v_sStatusCode As String, ByRef v_nInsuranceFileKeyForAction As Integer) As Integer
            Dim oPolicyVersions As NexusProvider.PolicyCollection
            Dim oWebService As NexusProvider.ProviderBase
            Dim nExistingVersions As Integer = 0
            Try
                oWebService = New NexusProvider.ProviderManager().Provider
                If v_nInsuranceFolderKey <> 0 Then
                    Dim aStatusToFilter() = v_sStatusCode.Split(",")

                    oPolicyVersions = oWebService.GetAllPolicyVersions(v_nInsuranceFolderKey)
                    Session(CNExistingQuotes) = oPolicyVersions

                    Dim oFilteredQuotes = (From oPolicyDetail In oPolicyVersions
                                           Where aStatusToFilter.Contains(oPolicyDetail.InsuranceFileTypeCode.ToUpper().Trim()) And oPolicyDetail.IsReadOnly = False
                                           Select oPolicyDetail).ToList()

                    If oFilteredQuotes IsNot Nothing AndAlso oFilteredQuotes.Count > 0 Then
                        nExistingVersions = oFilteredQuotes.Count
                    End If

                    If v_sStatusCode = "MTAQREINS" Then
                        Dim oVersionToReinstate = (From oPolicyDetail In oPolicyVersions
                                                   Where oPolicyDetail.InsuranceFileTypeCode.ToUpper().Trim() = "MTACAN" And oPolicyDetail.IsReadOnly = False Order By oPolicyDetail.InsuranceFileKey Descending
                                                   Select oPolicyDetail).FirstOrDefault()
                        If oVersionToReinstate IsNot Nothing Then
                            v_nInsuranceFileKeyForAction = oVersionToReinstate.InsuranceFileKey
                        End If
                    ElseIf oFilteredQuotes IsNot Nothing AndAlso oFilteredQuotes.Count > 0 Then
                        v_nInsuranceFileKeyForAction = oFilteredQuotes(0).InsuranceFileKey
                    End If
                End If
            Finally
                oWebService = Nothing
                oPolicyVersions = Nothing
            End Try
            Return nExistingVersions
        End Function

        ''' <summary>
        ''' To check if insurance folder cnt has a single renewal quote which is marked for collection
        ''' </summary>
        ''' <param name="v_nInsuranceFolderKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function IsRenewalQuoteMarkedForCollection(ByVal v_nInsuranceFolderKey As Integer) As Boolean
            Dim oPolicyVersions As NexusProvider.PolicyCollection
            Dim oWebService As NexusProvider.ProviderBase
            Try
                oWebService = New NexusProvider.ProviderManager().Provider
                If v_nInsuranceFolderKey <> 0 Then
                    oPolicyVersions = oWebService.GetAllPolicyVersions(v_nInsuranceFolderKey)

                    Dim oFilteredQuotes = (From oPolicyDetail In oPolicyVersions
                                           Where oPolicyDetail.InsuranceFileTypeCode.ToUpper().Trim() = "RENEWAL" And oPolicyDetail.IsReadOnly = False
                                           Select oPolicyDetail).ToList()
                    If oFilteredQuotes IsNot Nothing AndAlso oFilteredQuotes.Count = 1 Then
                        Return oFilteredQuotes(0).MarkedQuoteForCollection
                    Else
                        Return False
                    End If
                End If
            Finally
                oWebService = Nothing
                oPolicyVersions = Nothing
            End Try
        End Function

        ''' Check if any non-quote policy version is in LIVE state for the given insurance folder
        ''' </summary>
        ''' <param name="v_nInsuranceFolderKey"></param>
        ''' <returns></returns>
        Private Function HasLivePolicyVersion(ByVal v_nInsuranceFolderKey As Integer) As Boolean
            Dim oWebService As NexusProvider.ProviderBase = Nothing
            Try
                oWebService = New NexusProvider.ProviderManager().Provider
                If v_nInsuranceFolderKey <> 0 Then
                    Dim oPolicyVersions As NexusProvider.PolicyCollection = oWebService.GetAllPolicyVersions(v_nInsuranceFolderKey)
                    Dim sQuoteTypeCodes As String = "QUOTE,MTAQUOTE,MTAQTETEMP,MTAQREINS,MTAQCAN,RENEWAL,WRITTEN"
                    If oPolicyVersions IsNot Nothing Then
                        For Each oPolVersion As NexusProvider.Policy In oPolicyVersions
                            If oPolVersion.PolicyStatusCode IsNot Nothing AndAlso oPolVersion.PolicyStatusCode.Trim().ToUpper() = "LIVE" _
                                AndAlso oPolVersion.InsuranceFileTypeCode IsNot Nothing _
                                AndAlso Not sQuoteTypeCodes.Contains(oPolVersion.InsuranceFileTypeCode.Trim().ToUpper()) Then
                                Return True
                            End If
                        Next
                    End If
                End If
            Finally
                oWebService = Nothing
            End Try
            Return False
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdSearchResult_Sorting(sender As Object, e As GridViewSortEventArgs) Handles grdSearchResult.Sorting
            Dim oInsuranceFiles As NexusProvider.InsuranceFileDetailsCollection = ViewState(CNPolicyCollection)
            oInsuranceFiles.SortColumn = e.SortExpression
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
            oInsuranceFiles.SortingOrder = _sortDirection
            oInsuranceFiles.Sort()
            grdSearchResult.DataSource = oInsuranceFiles
            grdSearchResult.DataBind()
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

