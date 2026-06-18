Imports System.Web.Configuration
Imports Nexus.Library
Imports CMS.Library
Imports Nexus.Utils
Imports Nexus.Constants
Imports System.Web.HttpContext
Imports System.Web.Configuration.WebConfigurationManager

Namespace Nexus
    Partial Class secure_RenewalSelection : Inherits Frontend.clsCMSPage
        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                'Cleaning of the session values
                ClearQuote()
                ClearClaims()
                ClearHeader()
                GetSytemOption()
                'create a unique key and add this to viewstate
                'this will be used to cache the results of the SAM call
                Dim pageCacheID As Guid
                pageCacheID = Guid.NewGuid
                ViewState.Add("pageCacheID", pageCacheID.ToString)
                txtReference.Focus()
                If (Request.QueryString("ref") IsNot Nothing) Then
                    txtReference.Text = Request.QueryString("ref")
                End If

            End If
        End Sub
        ''' <summary>
        ''' Call BindGrid to get search results. Clear any previous results from cache first
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>Stores search results in session to be used when rebinding on page index change</remarks>
        Protected Sub btnFindNow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
            If Page.IsValid Then
                Cache.Remove(ViewState("pageCacheID"))
                grdvSearchResults.PageIndex = 0
                lblMessage.Text = String.Empty
                BindGrid()
            End If
        End Sub

        Protected Sub grdvSearchResults_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvSearchResults.DataBound
            If CType(sender, GridView).PageCount < 2 Then
                CType(sender, GridView).AllowPaging = False
            End If
        End Sub

        ''' <summary>
        ''' Sets the new page index and rebinds data
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvSearchResults_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvSearchResults.PageIndexChanging
            grdvSearchResults.PageIndex = e.NewPageIndex
            BindGrid()
        End Sub

        ''' <summary>
        ''' Calls FindPolicy passing in search parameters and populates grid with results
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub BindGrid()
            If UserCanDoTask("RenewalSelection") Then
                'try to get the search results from the cache
                Dim oInsuranceFileDetailsCollection As NexusProvider.InsuranceFileDetailsCollection = _
                    CType(Cache.Item(ViewState("pageCacheID")), NexusProvider.InsuranceFileDetailsCollection)
                oInsuranceFileDetailsCollection = Nothing
                If oInsuranceFileDetailsCollection Is Nothing Then
                    'Get search results by calling FindPolicy
                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim sInsuranceRef As String = IIf(String.IsNullOrEmpty(txtReference.Text), Nothing, txtReference.Text)
                    Dim sRiskIndex As String = IIf(String.IsNullOrEmpty(txtRiskIndex.Text), Nothing, txtRiskIndex.Text)
                    Dim sClientShortName As String = IIf(String.IsNullOrEmpty(txtClient.Text), Nothing, txtClient.Text)
                    Dim sQuoteType As NexusProvider.InsuranceFileType
                    Dim iMaxRowsToFetch As Integer = oPortal.MaxSearchResults

                    sQuoteType = NexusProvider.InsuranceFileTypes.POLICY

                    oInsuranceFileDetailsCollection = oWebService.FindPolicy(sInsuranceRef, sRiskIndex, sClientShortName, sQuoteType, False, iMaxRowsToFetch, Nothing)

                    If oInsuranceFileDetailsCollection IsNot Nothing Then
                        'add the results to the cache so that we don't need to call FindPolicy again
                        'todo - cache length should be taken from config
                        Cache.Insert(ViewState("pageCacheID"), oInsuranceFileDetailsCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

                        'validate size of dataset. if 500(configured at portal level) or more results are returned then add a validation message to the screen
                        If oInsuranceFileDetailsCollection.Count >= oPortal.MaxSearchResults Then
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
                End If

                'Bind the grid to the search results and make sure that it is visible
                grdvSearchResults.Visible = True
                grdvSearchResults.AllowPaging = True
                grdvSearchResults.DataSource = oInsuranceFileDetailsCollection
                grdvSearchResults.DataBind()
            End If


        End Sub

        ''' <summary>
        ''' Clears form, hides the results grid and also clears search results from cache
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnNewSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewSearch.Click
            grdvSearchResults.Visible = False
            txtClient.Text = String.Empty
            txtReference.Text = String.Empty
            txtRiskIndex.Text = String.Empty
            lblMessage.Text = String.Empty
            PnlMessage.Visible = False
            Cache.Remove(ViewState("pageCacheID"))
        End Sub

        Protected Sub grdvSearchResults_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdvSearchResults.RowCommand
            If e.CommandName = "select" Then
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oQuote As New NexusProvider.Quote
                Dim oPortalConfig As Config.Portal = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID())
                Dim oPolCol As New NexusProvider.PolicyCollection
                PnlMessage.Visible = False
                Dim bExclusiveLock As Boolean = True
                Try
                    Dim nInsuranceFolderKey As Integer
                    Dim nInsuranceFileKey As Integer
                    Dim sUserName As String

                    Dim GridRow As GridViewRow
                    If Not LCase(e.CommandName).Equals("page") And Not LCase(e.CommandName).Equals("sort") Then
                        GridRow = CType((e.CommandSource).NamingContainer, GridViewRow)
                        Dim lblInsuranceFolderKey As Label = GridRow.FindControl("lblInsuranceFolderKey")
                        nInsuranceFolderKey = CInt(lblInsuranceFolderKey.Text)
                        Dim lblInsuranceFileKey As Label = GridRow.FindControl("lblInsuranceFileKey")
                        nInsuranceFileKey = CInt(lblInsuranceFileKey.Text)

                    End If

                    oQuote.InsuranceFileKey = e.CommandArgument
                    oQuote = oWebService.GetHeaderAndSummariesByKey(v_iInsuranceFileKey:=nInsuranceFileKey, bExclusiveLock:=True)

                    Dim oOOSMTAVersions As NexusProvider.BaseInsuranceFileKeyCollection
                    oOOSMTAVersions = oWebService.CheckPendingOOSVersions(oQuote.InsuranceFileKey, oQuote.InsuranceFolderKey)
                    If oOOSMTAVersions IsNot Nothing AndAlso oOOSMTAVersions.Count > 0 Then
                        For Each oVersion As NexusProvider.BaseInsuranceFileKey In oOOSMTAVersions
                            oWebService.DeleteBackDatedVersions(oVersion.BaseInsuranceFileKey)
                        Next
                    End If
                    Dim oPreRenStatus As NexusProvider.RenewalStatus
                    oPreRenStatus = oWebService.GetRenewalStatus(nInsuranceFileKey)
                    If Not oPreRenStatus Is Nothing And Not oPreRenStatus.RenewalStatusTypeDescription Is Nothing Then
                        If oPreRenStatus.RenewalStatusTypeDescription.ToString <> "" Then
                            If oQuote Is Nothing Then
                                oQuote = oWebService.GetHeaderAndSummariesByKey(v_iInsuranceFileKey:=oQuote.InsuranceFileKey)
                            End If

                            oWebService.DeleteRenewal(oQuote, oQuote.BranchCode)
                            Dim oInsuranceFileDetailsCollection As NexusProvider.InsuranceFileDetailsCollection
                            oInsuranceFileDetailsCollection = oWebService.FindPolicy(oQuote.InsuranceFileRef, "", "", NexusProvider.InsuranceFileTypes.POLICY, False)
                            For Each oInsuranceFileDetails As NexusProvider.InsuranceFileDetails In oInsuranceFileDetailsCollection
                                oQuote.InsuranceFileKey = oInsuranceFileDetails.InsuranceFileKey
                                Exit For
                            Next
                        End If
                    Else
                        'Deletion of all previous version of the Renewal
                        oQuote.InsuranceFileKey = e.CommandArgument
                        If oQuote Is Nothing Then
                            oQuote = oWebService.GetHeaderAndSummariesByKey(v_iInsuranceFileKey:=oQuote.InsuranceFileKey)
                        End If
                        Dim oInsuranceFileDetailsCollection As NexusProvider.InsuranceFileDetailsCollection
                        oInsuranceFileDetailsCollection = oWebService.FindPolicy(oQuote.InsuranceFileRef, "", "", NexusProvider.InsuranceFileTypes.RENEWAL, False)
                        If oInsuranceFileDetailsCollection IsNot Nothing Then
                            For Each oInsuranceFileDetails As NexusProvider.InsuranceFileDetails In oInsuranceFileDetailsCollection
                                Dim oTempQuote As New NexusProvider.Quote
                                oTempQuote = oWebService.GetHeaderAndSummariesByKey(oInsuranceFileDetails.InsuranceFileKey, , bExclusiveLock:=True)
                                If oTempQuote.CoverStartDate = oQuote.RenewalDate Then
                                    oWebService.DeleteRenewal(oTempQuote, oTempQuote.BranchCode)
                                End If
                            Next
                        End If
                        oInsuranceFileDetailsCollection = oWebService.FindPolicy(oQuote.InsuranceFileRef, "", "", NexusProvider.InsuranceFileTypes.POLICY, False)
                        For Each oInsuranceFileDetails As NexusProvider.InsuranceFileDetails In oInsuranceFileDetailsCollection
                            oQuote.InsuranceFileKey = oInsuranceFileDetails.InsuranceFileKey
                            oQuote.Reference = oInsuranceFileDetails.InsuranceRef
                            Exit For
                        Next
                    End If

                    oWebService.RunRenewalSelectionByPolicy(oQuote, oQuote.BranchCode)
                    Dim oStatus As NexusProvider.RenewalStatus
                    oStatus = oWebService.GetRenewalStatus(oQuote.InsuranceFileKey)
                    PnlMessage.Visible = True
                    lblMessage.Text = GetLocalResourceObject("lbl_Message").ToString()
                    lblMessage.Text = Replace(lblMessage.Text, "#PolicyRef", oQuote.Reference.Trim)
                    lblMessage.Text = Replace(lblMessage.Text, "#RenewalStatusDescription", oStatus.RenewalStatusTypeDescription.ToString)
                    BindGrid()
                    'unlock the Policy ( if locked)
                    Dim oOptionSettings As NexusProvider.OptionTypeSetting
                    oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ExclusiveLock) 'Exclusive Lock
                    If oOptionSettings.OptionValue = "1" Then
                        UnlockPolicy(oQuote.InsuranceFolderKey, Session(CNBranchCode).ToString)
                    End If

                Catch ex As NexusProvider.NexusException
                    'Policy locking error
                    Select Case CType(ex.Errors(0), NexusProvider.NexusError).Code
                        Case "200", "1000158" 'Policy Locking
                            'Show policy locking error as alert
                            Dim sMessage As String = "alert('" + Replace(GetLocalResourceObject("lbl_policylocked_error"), "{1}", (ex.Errors(0).Detail.Split(":"))(2) + ".") + "')"
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", sMessage, True)
                            Server.ClearError()
                            ClearQuote()
                            Exit Sub
                        Case Else
                            Throw
                    End Select
                Finally
                    oWebService = Nothing
                    oQuote = Nothing
                End Try
            End If
        End Sub

        Protected Sub grdvSearchResults_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles grdvSearchResults.RowCreated

            If (e.Row.RowType = DataControlRowType.Header OrElse e.Row.RowType = DataControlRowType.DataRow) Then
                Dim oAllowPolicyClientAssociations As NexusProvider.OptionTypeSetting = Nothing
                oAllowPolicyClientAssociations = ViewState("AllowPolicyClientAssociationsOptionSettings")

                If oAllowPolicyClientAssociations.OptionValue = "1" Then
                    grdvSearchResults.Columns(4).Visible = True
                Else
                    grdvSearchResults.Columns(4).Visible = False
                End If
            End If
        End Sub

        Protected Sub grdvSearchResults_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvSearchResults.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("ClaimKey") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.InsuranceFileDetails).InsuranceFileKey)

                Dim sInsuranceFileStatus As String = CType(e.Row.DataItem, NexusProvider.InsuranceFileDetails).Status.Trim()
                Dim lnkSelect As LinkButton = CType(e.Row.FindControl("lnkSelect"), LinkButton)
                lnkSelect.CommandArgument = CType(e.Row.DataItem, NexusProvider.InsuranceFileDetails).InsuranceFileKey
                If CType(e.Row.DataItem, NexusProvider.InsuranceFileDetails).IsMarketPlacePolicy Then
                    lnkSelect.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                End If

                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim bIsPendingPortfolioTransfer As Boolean
                Dim bIsPendingCloneTransfer As Boolean
                Dim nInsuranceFileKey As Integer = CType(e.Row.DataItem, NexusProvider.InsuranceFileDetails).InsuranceFileKey
                Dim sInsuranceRef As String = CType(e.Row.DataItem, NexusProvider.InsuranceFileDetails).InsuranceFileRef
                oWebService.IsPendingTransfer(nInsuranceFileKey, bIsPendingCloneTransfer, bIsPendingPortfolioTransfer, "")
                Dim sMessage As String = ""
                If bIsPendingCloneTransfer OrElse bIsPendingPortfolioTransfer Then
                    If bIsPendingPortfolioTransfer Then
                        sMessage = GetLocalResourceObject("msg_PendingPortfolioTransfer")
                    ElseIf bIsPendingCloneTransfer Then
                        sMessage = GetLocalResourceObject("msg_PendingClonedTransfer")
                    End If
                    lnkSelect.Attributes.Add("onclick", "alert('" + sMessage + "');return false;")
                Else
                    If sInsuranceFileStatus = "Replaced: Backdated Endorsement" Then
                        sMessage = GetLocalResourceObject("msg_SavedOOSVersions")
                        lnkSelect.Attributes.Add("onclick", "return confirm('" + sMessage + "');")
                    End If
                End If

                Dim oAllowPolicyClientAssociations As NexusProvider.OptionTypeSetting = ViewState("AllowPolicyClientAssociationsOptionSettings")
                If oAllowPolicyClientAssociations IsNot Nothing AndAlso oAllowPolicyClientAssociations.OptionValue = "1" Then
                    Dim xmldoc As New System.Xml.XmlDocument
                    If e.Row.DataItem IsNot Nothing Then
                        If ((CType(e.Row.DataItem, NexusProvider.InsuranceFileDetails).AssociatedClients IsNot Nothing) AndAlso Not (String.IsNullOrEmpty(CType(e.Row.DataItem, NexusProvider.InsuranceFileDetails).AssociatedClients))) Then
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
        End Sub

        Protected Sub custValidate_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles custValidate.ServerValidate
            If args.IsValid = True AndAlso String.IsNullOrEmpty(txtRiskIndex.Text) = False Then
                If txtRiskIndex.Text.Trim.Contains("%") Then
                    args.IsValid = False
                    custValidate.ErrorMessage = GetLocalResourceObject("lbl_RiskIndex_Error")
                End If
            End If
        End Sub

        Protected Sub Page_PreInit1(sender As Object, e As EventArgs) Handles Me.PreInit
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "MarketPlacePolicyConfirmation", _
                        "<script language=""JavaScript"" type=""text/javascript"">function MarketPlacePolicyConfirmation(){var IsConfirm; IsConfirm=confirm('" & GetLocalResourceObject("msg_ConfirmMarketPlacePolicy1").ToString() & "'); if(IsConfirm==true) { IsConfirm=confirm('" & GetLocalResourceObject("msg_ConfirmMarketPlacePolicy2").ToString() & "'); return IsConfirm; } else {return IsConfirm;} }</script>")
        End Sub

        ''' <summary>
        ''' Register client script for the button Client Code to open "FindClient.aspx" page.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

            If HttpContext.Current.Session.IsCookieless Then
                btnClient.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/secure/agent/FindClient.aspx?RequestPage=BG&modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=500&width=800' , null);return false;"
            Else
                btnClient.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/secure/agent/FindClient.aspx?RequestPage=BG&modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=500&width=800' , null);return false;"
            End If

        End Sub
        Protected Sub GetSytemOption()
            Dim oAllowPolicyClientAssociationsOptionSettings As NexusProvider.OptionTypeSetting
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

            oAllowPolicyClientAssociationsOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.AllowPolicyClientAssociations)
            ViewState("AllowPolicyClientAssociationsOptionSettings") = oAllowPolicyClientAssociationsOptionSettings

            oWebService = Nothing
        End Sub
        Private Function CheckLock(ByVal nInsuranceFolderCnt As Integer) As String
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
                If HttpContext.Current.User.Identity.Name.Trim() <> oLockItem.LockUserName.Trim() _
                AndAlso oLockItem.LockName.Trim() = "insurance_folder_cnt" AndAlso oLockItem.LockValue = nInsuranceFolderCnt Then
                    sUserName = oLockItem.LockUserName.Trim()
                End If
            Next
            Return sUserName
        End Function
    End Class
End Namespace

