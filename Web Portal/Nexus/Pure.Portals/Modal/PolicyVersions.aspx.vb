Imports Nexus.Utils
Imports NexusProvider.SAMForInsurance
Imports System.Data
Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports System.Linq

Namespace Nexus
    Partial Class Modal_PolicyVersions
        Inherits CMS.Library.Frontend.clsCMSPage

        Const CNPolicyVersions As String = "PolicyVersions"
        Const CNIsReinstateLink As String = "IsReinstateLink"
        Const CNExistingQuotes As String = "ExistingQuotes"

        Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "UnMarkedConfirmation", _
                       "<script language=""JavaScript"" type=""text/javascript"">function UnMarkedConfirmation(){var IsConfirm; IsConfirm=confirm('" & GetLocalResourceObject("msg_ConfirmUnMarkedCollection").ToString() & "');document.getElementById('" & hfAnswerForMarkedQuoteCollection.ClientID & "').value=IsConfirm;return IsConfirm;}</script>")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "UnReInstatementConfirmation", _
                      "<script language=""JavaScript"" type=""text/javascript"">function UnReInstatementConfirmation(){var IsConfirm; IsConfirm=confirm('" & GetLocalResourceObject("msg_ConfirmReInstatement").ToString() & "');return IsConfirm;}</script>")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "MarketPlacePolicyConfirmation",
                        "<script language=""JavaScript"" type=""text/javascript"">function MarketPlacePolicyConfirmation(){var IsConfirm; IsConfirm=confirm('" & GetLocalResourceObject("msg_ConfirmMarketPlacePolicy1").ToString() & "'); if(IsConfirm==true) { IsConfirm=confirm('" & GetLocalResourceObject("msg_ConfirmMarketPlacePolicy2").ToString() & "'); if(IsConfirm==true) { document.getElementById('" & hfAnswerForMarketPlacePolicy.ClientID & "').value = false; } return IsConfirm; } else {return IsConfirm;} }</script>")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "VoidPolicyVersionConfirmation",
                      "<script language=""JavaScript"" type=""text/javascript"">function VoidPolicyVersionConfirmation(){ var sVoidConfirmationMessage = document.getElementById('" & hvVoidMessage.ClientID & "').value; if (confirm(sVoidConfirmationMessage) == true) {document.getElementById('" & hvVoidConfirm.ClientID & "').value=true; return true;} else {document.getElementById('" & hvVoidConfirm.ClientID & "').value=false;} }</script>")
        End Sub

        ''' <summary>
        ''' To attach event for "Add Quote" button
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Load1(sender As Object, e As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                Dim bShowExistingQuote As Boolean = False

                If Request.QueryString("ShowExistingQuote") IsNot Nothing Then
                    bShowExistingQuote = Convert.ToBoolean(Request.QueryString("ShowExistingQuote"))
                End If

                BindPolicyVersions(Convert.ToInt32(Request.QueryString("InsuranceFolderKey")), bShowExistingQuote)

                If bShowExistingQuote Then
                    If Request.QueryString("StatusCode") = "MTAQUOTE,MTAQCAN" Then
                        btnNewQuote.OnClientClick = "self.parent.selectPolicyVersion('" & Request.QueryString("PostbackTo") & "','MTAquote','" & Request.QueryString("InsuranceFileKey") & "','" & hfAnswerForMarkedQuoteCollection.Value & "','" & hfAnswerForMarketPlacePolicy.Value & "');"
                        btnNewQuote.Visible = True
                    ElseIf Request.QueryString("StatusCode") = "MTAQREINS" Then
                        btnNewQuote.OnClientClick = "self.parent.selectPolicyVersion('" & Request.QueryString("PostbackTo") & "','Reinstatement','" & Request.QueryString("InsuranceFileKey") & "','" & hfAnswerForMarkedQuoteCollection.Value & "','" & hfAnswerForMarketPlacePolicy.Value & "');"
                        btnNewQuote.Visible = True
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' To initialize modal theme
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        ''' <summary>
        ''' To show/hide paging
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdPolicyVersions_Load(sender As Object, e As EventArgs) Handles grdPolicyVersions.Load
            If grdPolicyVersions.PageCount = 1 Then
                grdPolicyVersions.AllowPaging = False
            End If
        End Sub

        ''' <summary>
        ''' To handle page index changing 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdPolicyVersions_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdPolicyVersions.PageIndexChanging
            grdPolicyVersions.DataSource = ViewState(CNPolicyVersions)
            grdPolicyVersions.PageIndex = e.NewPageIndex
            grdPolicyVersions.DataBind()
        End Sub

        ''' <summary>
        ''' To attach a postback event for NewQuote
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdPolicyVersions_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdPolicyVersions.RowCommand
            Select Case e.CommandName
                Case "View", "viewpolicy"
                    Session(CNMode) = "View"
                Case Else
                    If e.CommandName = "Void" Then
                        SetPolicyVersionVoid(e.CommandArgument)
                    End If
                    Dim nInsuranceFileKey As Integer
                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim oExclusiveLocking As NexusProvider.OptionTypeSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ExclusiveLock)
                    If oExclusiveLocking.OptionValue = "1" Then

                        Dim nInsuranceFolderKey As Integer = Convert.ToInt32(Request.QueryString("InsuranceFolderKey"))

                        'Dim oInsuranceFiles As NexusProvider.PolicyCollection = ViewState(CNPolicyVersions)
                        'Dim oPolicyDetail = (From oInsuranceFile In oInsuranceFiles
                        '             Select oInsuranceFile).FirstOrDefault
                        nInsuranceFileKey = CType(e.CommandArgument, Int32)
                        Dim sUserName As String = CheckLock(nInsuranceFolderKey, Session(CNBranchCode).ToString)
                        If sUserName.Trim.Length > 0 Then
                            Dim sMessage As String = "alert('" + Replace(GetLocalResourceObject("lbl_policylocked_error"), "{1}", sUserName + ".") + "')"
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", sMessage, True)
                            Return
                        End If

                        Dim oQuote As NexusProvider.Quote
                        oQuote = oWebService.GetHeaderAndSummariesByKey(nInsuranceFileKey, bExclusiveLock:=True)
                        oWebService = Nothing

                    End If
            End Select
            If Not LCase(e.CommandName).Equals("page") And Not LCase(e.CommandName).Equals("sort") Then
                ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "closeThickBox", "self.parent.selectPolicyVersion('" & Request.QueryString("PostbackTo") & "','" & e.CommandName & "','" & e.CommandArgument & "','" & hfAnswerForMarkedQuoteCollection.Value & "','" & hfAnswerForMarketPlacePolicy.Value & "');", True)
            End If
        End Sub

        ''' <summary>
        ''' To bind policy versions
        ''' </summary>
        ''' <param name="v_nInsuranceFolderKey"></param>
        ''' <param name="v_bShowExistingVersions"></param>
        ''' <remarks></remarks>
        Private Sub BindPolicyVersions(v_nInsuranceFolderKey As Integer, v_bShowExistingVersions As Boolean)
            Dim oPolicyVersions As NexusProvider.PolicyCollection
            Dim oWebService As NexusProvider.ProviderBase

            Try
                oWebService = New NexusProvider.ProviderManager().Provider

                If v_bShowExistingVersions = True Then
                    oPolicyVersions = Session(CNExistingQuotes)
                    Dim aStatusToFilter() = Request.QueryString("StatusCode").Split(",")
                    Dim oFilteredQuotes = (From oPolicyDetail In oPolicyVersions
                              Where aStatusToFilter.Contains(oPolicyDetail.InsuranceFileTypeCode.ToUpper().Trim()) And oPolicyDetail.IsReadOnly = False
                              Select oPolicyDetail).ToList()

                    If oFilteredQuotes.Count > 0 Then
                        oPolicyVersions = New NexusProvider.PolicyCollection
                    End If
                    For Each oPolicyVersion As NexusProvider.Policy In oFilteredQuotes
                        oPolicyVersions.Add(oPolicyVersion)
                    Next

                    ViewState.Add(CNPolicyVersions, oPolicyVersions)
                    ltPageHeading.Text = GetLocalResourceObject("lbl_ModalTitle_" & aStatusToFilter(0)).ToString()
                Else
                    oPolicyVersions = oWebService.GetAllPolicyVersions(v_nInsuranceFolderKey)
                    ViewState.Add(CNPolicyVersions, oPolicyVersions)
                End If

                IsReinstateLink()

                If oPolicyVersions.Count > 0 Then
                    ltPageHeading.Text = ltPageHeading.Text & " (" & oPolicyVersions(0).Reference.Trim & ")"
                End If

                grdPolicyVersions.AllowPaging = True
                grdPolicyVersions.PageIndex = 0
                grdPolicyVersions.DataSource = ViewState(CNPolicyVersions)
                grdPolicyVersions.DataBind()
            Finally
                oWebService = Nothing
            End Try

        End Sub

        ''' <summary>
        ''' To enable grid 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdPolicyVersions_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdPolicyVersions.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim oWebService As NexusProvider.ProviderBase
                Dim oPolicyDetail As NexusProvider.Policy = CType(e.Row.DataItem, NexusProvider.Policy)
                Dim oPortalConfig As Config.Portal = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID())
                Dim btnView As LinkButton = e.Row.Cells(8).FindControl("btnView")
                Dim lnkbtnAction As LinkButton = e.Row.Cells(8).FindControl("lnkbtnAction")
                Dim lnkbtnRenew As LinkButton = e.Row.Cells(8).FindControl("lnkbtnRenew")
                Dim liRenew As HtmlControl = e.Row.Cells(8).FindControl("liRenew")
                Dim liVoid As HtmlControl = e.Row.Cells(8).FindControl("liVoid")
                Dim lnkbtnVoid As LinkButton = e.Row.Cells(8).FindControl("lnkbtnVoid")
                Dim nInsuranceFileKey As Integer = oPolicyDetail.InsuranceFileKey
                Dim bShowVoidButton As Boolean = False
                Select Case UCase(oPolicyDetail.InsuranceFileTypeCode.Trim)
                    Case "POLICY"                       
                        '' need to check if the Policy has been CANCELLED then can't allow POLICY CHANGE again.
                        If oPolicyDetail.IsCurrent Then
                            bShowVoidButton = ShowHideVoidButton(nInsuranceFileKey)
                            If bShowVoidButton AndAlso liVoid IsNot Nothing AndAlso lnkbtnVoid IsNot Nothing Then
                                liVoid.Visible = True
                                lnkbtnVoid.Visible = True
                                lnkbtnVoid.CommandArgument = nInsuranceFileKey
                                lnkbtnVoid.Attributes.Add("OnClick", "javascript:return VoidPolicyVersionConfirmation();")
                                lnkbtnVoid.CommandName = "Void"
                            End If
                            If UserCanDoTask("MidTermAdjustment") Then
                                If oPolicyDetail.PolicyStatusCode.Trim.ToUpper() = "LAP" Then
                                    'Disable Temporary MTA system option
                                    Dim oDisableTemporaryMTAOptionSettings As NexusProvider.OptionTypeSetting
                                    Try
                                        oWebService = New NexusProvider.ProviderManager().Provider
                                        oDisableTemporaryMTAOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.DisableTempMTAs)
                                    Finally
                                        oWebService = Nothing
                                    End Try
                                    'Allow Permanent MTA on lapsed quote if "DISABLE TEMP MTA" system option is ON
                                    If oDisableTemporaryMTAOptionSettings.OptionValue = "1" Then
                                        lnkbtnAction.CommandArgument = nInsuranceFileKey
                                        lnkbtnAction.Text = GetLocalResourceObject("lbl_Change").ToString() '"edit"
                                        lnkbtnAction.CommandName = "MTAquote"
                                        lnkbtnAction.Visible = True
                                    End If
                                    'replace status from "Policy" to "Lapsed"
                                    'grdPolicyVersions.Rows(grdPolicyVersions.SelectedRow.RowIndex).Cells(8).Text = GetLocalResourceObject("lbl_Lapsed")
                                Else
                                    'if AllowMTA then only user will be able to see option CHANGE                                
                                    'Only Allow MTA if User Has Authority to do that
                                    If IsRenewed(oPolicyDetail.Reference.Trim, oPolicyDetail.CoverStartDate, oPolicyDetail.InsuranceFileKey) = False Then
                                        If UserCanDoTask("MidTermAdjustment") Or UserCanDoTask("MidTermReinstatement") Or UserCanDoTask("MidTermCancellation") Then
                                            lnkbtnAction.CommandArgument = nInsuranceFileKey
                                            lnkbtnAction.CommandName = "MTAquote"
                                            lnkbtnAction.Visible = True
                                            'This code is added for unmarking the quote for collection
                                            If oPolicyDetail.MarkedQuoteForCollection Then
                                                lnkbtnAction.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                                            End If
                                            If oPolicyDetail.IsMarketPlacePolicy Then
                                                lnkbtnAction.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                            btnView.CommandArgument = nInsuranceFileKey
                            btnView.CommandName = "viewpolicy"
                            btnView.Visible = True
                        ElseIf oPolicyDetail.PolicyStatusCode.ToUpper() = "CAN" _
                                Or oPolicyDetail.PolicyStatusCode.ToUpper() = "REN" Then
                            'if the plocy has been cancelled then only one link i.e VIEW
                            If oPolicyDetail.PolicyStatusCode.ToUpper() = "REN" Then
                                If IsInRenewal(oPolicyDetail.Reference) = True Then 'reference
                                    If UserCanDoTask("MidTermAdjustment") Or UserCanDoTask("MidTermReinstatement") Or UserCanDoTask("MidTermCancellation") Then
                                        lnkbtnAction.CommandArgument = nInsuranceFileKey
                                        lnkbtnAction.CommandName = "MTAquote"
                                        lnkbtnAction.Visible = True
                                        'This code is added for unmarking the quote for collection
                                        If oPolicyDetail.MarkedQuoteForCollection Then
                                            lnkbtnAction.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                                        End If
                                        If oPolicyDetail.IsMarketPlacePolicy Then
                                            lnkbtnAction.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                                        End If
                                    End If
                                End If
                            End If

                            btnView.CommandArgument = nInsuranceFileKey
                            btnView.CommandName = "viewMTA"
                            btnView.Visible = True
                            'Various cases copied from clientquotes.ascx.vb for safe coding as future perspective
                        ElseIf oPolicyDetail.InsuranceFileTypeCode.ToUpper() = "LAP" Then

                            btnView.CommandArgument = nInsuranceFileKey
                            btnView.CommandName = "viewpolicy"
                            btnView.Visible = True
                            'grdPolicyVersions.Rows(grdPolicyVersions.SelectedRow.RowIndex).Cells(8).Text = GetLocalResourceObject("lbl_Lapsed")

                        ElseIf Convert.ToBoolean(IsRenewed(oPolicyDetail.Reference.Trim, oPolicyDetail.CoverStartDate, oPolicyDetail.InsuranceFileKey)) = True Then
                            btnView.CommandArgument = nInsuranceFileKey
                            btnView.CommandName = "viewpolicy"
                            btnView.Visible = True
                        Else
                            btnView.CommandArgument = nInsuranceFileKey
                            btnView.CommandName = "viewpolicy"
                            btnView.Visible = True
                        End If

                    Case "QUOTE"

                        If HasLivePolicyVersion() Then
                            'When any non-quote version is LIVE, only show View button for quotation
                            btnView.CommandArgument = nInsuranceFileKey
                            btnView.CommandName = "viewpolicy"
                            btnView.Visible = True
                            lnkbtnAction.Visible = False
                        Else
                        Dim dExpiryDate As Date = oPolicyDetail.QuoteExpiryDate
                        If UserCanDoTask("NewBusiness") AndAlso oPolicyDetail.PolicyStatusCode.Trim.ToUpper() <> "LAP" Then

                            If (dExpiryDate < DateTime.Today) Then
                                btnView.CommandArgument = nInsuranceFileKey
                                btnView.CommandName = "viewpolicy"
                                btnView.Visible = True
                            Else
                                lnkbtnAction.CommandArgument = nInsuranceFileKey
                                lnkbtnAction.CommandName = "EditQuote"
                                lnkbtnAction.Text = GetLocalResourceObject("lbl_Edit").ToString() 'Edit
                                lnkbtnAction.Visible = True
                                If (oPolicyDetail.QuoteExpiryDate < DateTime.Now) Then
                                    lnkbtnAction.Attributes.Add("OnClick", "alert('" + GetLocalResourceObject("lbl_QuoteExpired") + "');")
                                End If
                                If oPolicyDetail.IsMarketPlacePolicy Then
                                    lnkbtnAction.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                                End If
                            End If
                            'This code is added for unmarking the quote for collection
                            If oPolicyDetail.MarkedQuoteForCollection Then
                                lnkbtnAction.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                            Else
                                If (oPolicyDetail.QuoteExpiryDate < DateTime.Now) Then
                                    lnkbtnAction.Attributes.Add("OnClick", "alert('" + GetLocalResourceObject("lbl_QuoteExpired") + "');")
                                End If
                            End If
                        ElseIf UserCanDoTask("ViewQuote") Then
                            btnView.CommandArgument = nInsuranceFileKey
                            btnView.CommandName = "View"
                            btnView.Visible = True
                        End If
                        End If

                    Case "WRITTEN"
                        If oPolicyDetail.EventDesc IsNot Nothing Then 'Event Desc
                            'Edit/Buy options will be available only if user has Authority
                            If UserCanDoTask("NewBusiness") And Not oPolicyDetail.EventDesc.Contains("Written Renewal") Then
                                'code commented to hide the edit button for Written Policy status

                                'This code is added for unmarking the quote for collection
                                If oPolicyDetail.MarkedQuoteForCollection Then
                                    lnkbtnAction.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                                    lnkbtnAction.Visible = True
                                    If oPolicyDetail.IsMarketPlacePolicy Then
                                        lnkbtnAction.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                                    End If
                                End If
                            End If
                        End If

                    Case "MTAQUOTE", "MTAQTETEMP", "MTAQCAN"

                        If oPolicyDetail.PolicyStatusCode.Trim().ToUpper() = "LIVE" Then

                            If UserCanDoTask("MidTermAdjustment") Then
                                lnkbtnAction.CommandArgument = nInsuranceFileKey
                                lnkbtnAction.CommandName = "editmtaquote"
                                lnkbtnAction.Text = GetLocalResourceObject("lbl_Edit").ToString() '"Edit"
                                lnkbtnAction.Visible = True
                                If (oPolicyDetail.QuoteExpiryDate < DateTime.Now) Then
                                    lnkbtnAction.Attributes.Add("OnClick", "alert('" + GetLocalResourceObject("lbl_QuoteExpired") + "');")
                                End If
                                'This code is added for unmarking the quote for collection
                                If oPolicyDetail.MarkedQuoteForCollection Then
                                    lnkbtnAction.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                                Else
                                    If (oPolicyDetail.QuoteExpiryDate < DateTime.Now) Then
                                        lnkbtnAction.Attributes.Add("OnClick", "alert('" + GetLocalResourceObject("lbl_QuoteExpired") + "');")
                                    End If
                                End If
                            End If

                        ElseIf oPolicyDetail.PolicyStatusCode.Trim().ToUpper() = "CAN" _
                            Or oPolicyDetail.PolicyStatusCode.Trim().ToUpper() = "LAP" _
                            Or oPolicyDetail.PolicyStatusCode.Trim().ToUpper() = "REP" Then
                            'if the plocy has been cancelled then only one link i.e VIEW
                            If UserCanDoTask("ViewQuote") Then
                                btnView.CommandArgument = nInsuranceFileKey
                                btnView.CommandName = "View"
                                btnView.Visible = True
                            End If
                        End If

                    Case "MTAQREINS" '

                        'Only Allow MTA if User Has Authority to do that
                        '' need to check if the Policy has been CANCELLED then can't allow POLICY CHANGE again.
                        If oPolicyDetail.PolicyStatusCode.Trim().ToUpper() = "LIVE" Then

                            If UserCanDoTask("MidTermReinstatement") Then
                                lnkbtnAction.CommandArgument = nInsuranceFileKey
                                lnkbtnAction.CommandName = "editmtaquote"
                                lnkbtnAction.Visible = True
                                lnkbtnAction.Text = GetLocalResourceObject("lbl_Edit").ToString() '"Edit"
                                If (oPolicyDetail.QuoteExpiryDate < DateTime.Now) Then
                                    lnkbtnAction.Attributes.Add("OnClick", "alert('" + GetLocalResourceObject("lbl_QuoteExpired") + "');")
                                End If
                                'This code is added for unmarking the quote for collection
                                If oPolicyDetail.MarkedQuoteForCollection Then
                                    lnkbtnAction.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                                Else
                                    If (oPolicyDetail.QuoteExpiryDate < DateTime.Now) Then
                                        lnkbtnAction.Attributes.Add("OnClick", "alert('" + GetLocalResourceObject("lbl_QuoteExpired") + "');")
                                    End If
                                End If
                            End If
                        Else
                            'if the plocy has been cancelled then only one link i.e VIEW
                            If UserCanDoTask("ViewQuote") Then
                                btnView.CommandArgument = nInsuranceFileKey
                                btnView.CommandName = "View"
                                btnView.Visible = True
                            End If
                        End If

                    Case "MTA PERM", "MTA TEMP"                        
                        Select Case oPolicyDetail.InsuranceFileTypeCode.Trim().ToUpper() 'InsuranceFileTypeCode
                            Case "MTA PERM"
                                If oPolicyDetail.IsCurrent Then
                                    bShowVoidButton = ShowHideVoidButton(nInsuranceFileKey)
                                    If bShowVoidButton AndAlso liVoid IsNot Nothing AndAlso lnkbtnVoid IsNot Nothing Then
                                        liVoid.Visible = True
                                        lnkbtnVoid.Visible = True
                                        lnkbtnVoid.CommandArgument = nInsuranceFileKey
                                        lnkbtnVoid.Attributes.Add("OnClick", "javascript:return VoidPolicyVersionConfirmation();")
                                        lnkbtnVoid.CommandName = "Void"
                                    End If
                                    If UserCanDoTask("MidTermAdjustment") Or UserCanDoTask("MidTermReinstatement") Or UserCanDoTask("MidTermCancellation") Then
                                        If oPolicyDetail.PolicyStatusCode.Trim().ToUpper() = "LAP" Then
                                            'Disable Temporary MTA system option
                                            Dim oDisableTemporaryMTAOptionSettings As NexusProvider.OptionTypeSetting
                                            Try
                                                oWebService = New NexusProvider.ProviderManager().Provider
                                                oDisableTemporaryMTAOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.DisableTempMTAs)
                                            Finally
                                                oWebService = Nothing
                                            End Try
                                            'WPR 59 - Allow Permanent MTA on lapsed quote if "DISABLE TEMP MTA" system option is ON
                                            If oDisableTemporaryMTAOptionSettings.OptionValue = "1" Then
                                                lnkbtnAction.CommandArgument = nInsuranceFileKey
                                                lnkbtnAction.Text = GetLocalResourceObject("lbl_Change").ToString() '"edit"
                                                lnkbtnAction.CommandName = "MTAquote"
                                                lnkbtnAction.Visible = True
                                            End If
                                        Else
                                            lnkbtnAction.CommandArgument = nInsuranceFileKey
                                            lnkbtnAction.CommandName = "MTAquote"
                                            lnkbtnAction.Visible = True
                                        End If
                                        If oPolicyDetail.IsMarketPlacePolicy Then
                                            lnkbtnAction.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                                        End If
                                    End If

                                ElseIf oPolicyDetail.PolicyStatusCode.Trim.ToUpper() = "CAN" _
                                Or oPolicyDetail.PolicyStatusCode.Trim.ToUpper() = "REN" Then 'PolicystatusCdoe
                                    'if the plocy has been cancelled then only one link i.e VIEW
                                    If oPolicyDetail.PolicyStatusCode.Trim().ToUpper() = "REN" Then
                                        If IsInRenewal(oPolicyDetail.Reference) = True Then 'reference
                                            If UserCanDoTask("MidTermAdjustment") Or UserCanDoTask("MidTermReinstatement") Or UserCanDoTask("MidTermCancellation") Then
                                                lnkbtnAction.CommandArgument = nInsuranceFileKey
                                                lnkbtnAction.Text = GetLocalResourceObject("lbl_Change").ToString() '"edit"
                                                lnkbtnAction.CommandName = "MTAquote"
                                                lnkbtnAction.Visible = True
                                                'This code is added for unmarking the quote for collection
                                                If Convert.ToBoolean(grdPolicyVersions.DataKeys(grdPolicyVersions.SelectedRow.RowIndex).Values("MarkedQuoteForCollection")) Then
                                                    lnkbtnAction.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                                                End If
                                                If Convert.ToBoolean(grdPolicyVersions.DataKeys(grdPolicyVersions.SelectedRow.RowIndex).Values("IsMarketPlacePolicy")) Then
                                                    lnkbtnAction.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                                                End If
                                                'end
                                            End If
                                        End If
                                    End If
                                ElseIf oPolicyDetail.PolicyStatusCode.Trim().ToUpper() = "LAP" Then

                                    btnView.CommandArgument = nInsuranceFileKey
                                    btnView.CommandName = "View"
                                    btnView.Visible = True
                                    'grdPolicyVersions.Rows(grdPolicyVersions.SelectedRow.RowIndex).Cells(8).Text = GetLocalResourceObject("lbl_Lapsed")
                                End If

                                'If oPolicyDetail.PolicyStatusCode.Trim().ToUpper() = "LAP" Then
                                '    grdPolicyVersions.Rows(grdPolicyVersions.SelectedRow.RowIndex).Cells(8).Text = GetLocalResourceObject("lbl_Lapsed")
                                'End If

                            Case "MTA TEMP"
                                bShowVoidButton = ShowHideVoidButton(nInsuranceFileKey)
                                If bShowVoidButton AndAlso liVoid IsNot Nothing AndAlso lnkbtnVoid IsNot Nothing Then
                                    liVoid.Visible = True
                                    lnkbtnVoid.Visible = True
                                    lnkbtnVoid.CommandArgument = nInsuranceFileKey
                                    lnkbtnVoid.Attributes.Add("OnClick", "javascript:return VoidPolicyVersionConfirmation();")
                                    lnkbtnVoid.CommandName = "Void"
                                End If
                        End Select

                        btnView.CommandArgument = nInsuranceFileKey
                        btnView.CommandName = "View"
                        btnView.Visible = True

                    Case "MTAREINS"                       
                        '' need to check if the Policy has been CANCELLED then can't allow POLICY CHANGE again.
                        If oPolicyDetail.IsCurrent Then
                                bShowVoidButton = ShowHideVoidButton(nInsuranceFileKey)
                            If bShowVoidButton AndAlso liVoid IsNot Nothing AndAlso lnkbtnVoid IsNot Nothing Then
                                liVoid.Visible = True
                                lnkbtnVoid.Visible = True
                                lnkbtnVoid.CommandArgument = nInsuranceFileKey
                                lnkbtnVoid.Attributes.Add("OnClick", "javascript:return VoidPolicyVersionConfirmation();")
                                lnkbtnVoid.CommandName = "Void"
                            End If
                            If UserCanDoTask("MidTermAdjustment") Then
                                If IsInRenewal(oPolicyDetail.Reference) = True _
                                    Or Convert.ToBoolean(IsRenewed(oPolicyDetail.Reference.Trim, oPolicyDetail.CoverStartDate, oPolicyDetail.InsuranceFileKey)) = False _
                                    Or IsReinstated(oPolicyDetail.Reference) = True Then
                                    If UserCanDoTask("MidTermAdjustment") Or UserCanDoTask("MidTermReinstatement") Or UserCanDoTask("MidTermCancellation") Then
                                        lnkbtnAction.CommandArgument = nInsuranceFileKey
                                        lnkbtnAction.Text = GetLocalResourceObject("lbl_Change").ToString() '"edit"
                                        lnkbtnAction.CommandName = "MTAquote"
                                        lnkbtnAction.Visible = True
                                        'This code is added for unmarking the quote for collection
                                        If oPolicyDetail.MarkedQuoteForCollection Then
                                            lnkbtnAction.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                                        End If
                                        If oPolicyDetail.IsMarketPlacePolicy Then
                                            lnkbtnAction.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                                        End If
                                        'end
                                    End If
                                End If
                            End If
                            btnView.CommandArgument = nInsuranceFileKey
                            btnView.CommandName = "View"
                            btnView.Visible = True

                        ElseIf oPolicyDetail.PolicyStatusCode.Trim().ToUpper() = "CAN" _
                          Or oPolicyDetail.PolicyStatusCode.Trim().ToUpper() = "LAP" _
                          Or oPolicyDetail.PolicyStatusCode.Trim().ToUpper() = "REN" _
                          Or oPolicyDetail.PolicyStatusCode.Trim().ToUpper() = "REP" Then
                            'if the plocy has been cancelled then only one link i.e VIEW

                            If oPolicyDetail.PolicyStatusCode.Trim().ToUpper() = "LAP" Then
                                btnView.CommandArgument = nInsuranceFileKey
                                btnView.CommandName = "View"
                                btnView.Visible = True

                            ElseIf oPolicyDetail.PolicyStatusCode.Trim().ToUpper() = "REN" Then
                                If Convert.ToBoolean(IsInRenewal(oPolicyDetail.Reference)) = True Then
                                    If UserCanDoTask("MidTermAdjustment") Or UserCanDoTask("MidTermReinstatement") Or UserCanDoTask("MidTermCancellation") Then
                                        lnkbtnAction.CommandArgument = nInsuranceFileKey
                                        lnkbtnAction.CommandName = "MTAquote"
                                        lnkbtnAction.Visible = True
                                        'This code is added for unmarking the quote for collection
                                        If oPolicyDetail.MarkedQuoteForCollection Then
                                            lnkbtnAction.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                                        End If
                                        If oPolicyDetail.IsMarketPlacePolicy Then
                                            lnkbtnAction.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                                        End If
                                    End If

                                    btnView.CommandArgument = nInsuranceFileKey
                                    btnView.CommandName = "View"
                                    btnView.Visible = True
                                End If
                            Else
                                btnView.CommandArgument = nInsuranceFileKey
                                btnView.CommandName = "View"
                                btnView.Visible = True
                            End If
                        ElseIf oPolicyDetail.PolicyStatusCode.Trim().ToUpper() = "LIVE" Then
                            btnView.CommandArgument = nInsuranceFileKey
                            btnView.CommandName = "View"
                            btnView.Visible = True
                        End If

                    Case "RENEWAL"
                        bShowVoidButton = ShowHideVoidButton(nInsuranceFileKey)
                        If bShowVoidButton AndAlso liVoid IsNot Nothing AndAlso lnkbtnVoid IsNot Nothing Then
                            liVoid.Visible = True
                            lnkbtnVoid.Visible = True
                            lnkbtnVoid.CommandArgument = nInsuranceFileKey
                            lnkbtnVoid.Attributes.Add("OnClick", "javascript:return VoidPolicyVersionConfirmation();")
                            lnkbtnVoid.CommandName = "Void"
                        End If
                        If oPolicyDetail.PolicyStatusCode.Trim().ToUpper() = "LAP" Then 'Migrated LAPSED Policy
                            btnView.CommandArgument = nInsuranceFileKey
                            btnView.Text = GetLocalResourceObject("lbl_view").ToString()
                            btnView.CommandName = "View"
                            btnView.Visible = True
                            'grdPolicyVersions.Rows(grdPolicyVersions.SelectedRow.RowIndex).Cells(8).Text = GetLocalResourceObject("lbl_Lapsed")
                        Else
                            'need to show only one link i.e. "Details"
                            'Check the roles before displaying the "Details" link
                            If UserCanDoTask("Renewals") Then
                                liRenew.Visible = True
                                lnkbtnRenew.CommandArgument = nInsuranceFileKey
                                lnkbtnRenew.CommandName = "Renew"
                                lnkbtnRenew.Visible = True
                                lnkbtnAction.Visible = False
                                'This code is added for unmarking the quote for collection
                                If oPolicyDetail.MarkedQuoteForCollection Then
                                    lnkbtnRenew.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                                End If
                                CType(e.Row.FindControl("lbl_Status"), Label).Text = GetLocalResourceObject("lbl_RenewalQuote").ToString() '"RENEWAL QUOTE"
                            End If
                        End If
                    Case "MTACAN"
                        If oPolicyDetail.PolicyStatusCode.Trim().ToUpper() = "CAN" Then

                            'Now the Reinstatement button will only be shown if user has access to MTR/MTC
                            If IsRenewed(oPolicyDetail.Reference.Trim, _
                               oPolicyDetail.CoverStartDate, _
                                oPolicyDetail.InsuranceFileKey) = False _
                            And Convert.ToBoolean(IsInRenewal(oPolicyDetail.Reference)) = False Then
                                If ViewState(CNIsReinstateLink) IsNot Nothing AndAlso ViewState(CNIsReinstateLink) = oPolicyDetail.InsuranceFileKey Then
                                    If UserCanDoTask("MidTermReinstatement") Then
                                        lnkbtnAction.CommandArgument = nInsuranceFileKey
                                        lnkbtnAction.CommandName = "Reinstatement"
                                        lnkbtnAction.Text = GetLocalResourceObject("lbl_Reinstate").ToString() '"Reinstate"
                                        lnkbtnAction.Attributes.Add("OnClick", "javascript:return UnReInstatementConfirmation();")
                                        lnkbtnAction.Visible = True
                                    End If
                                End If
                            End If
                        End If

                        btnView.CommandArgument = nInsuranceFileKey
                        btnView.CommandName = "ViewMTA"
                        btnView.Visible = True
                    Case "VOID", "VOIDREP", "VOIDRENREP"
                        btnView.CommandArgument = nInsuranceFileKey
                        btnView.CommandName = "View"
                        btnView.Visible = True
                    Case Else

                        lnkbtnAction.Visible = False

                End Select

                Select Case UCase(oPolicyDetail.InsuranceFileTypeCode.Trim())
                    Case "MTA PERM"
                        btnView.CommandName = "ViewMTA"
                        btnView.Visible = True
                End Select

                If (ViewState("PageIndex") IsNot Nothing) Then
                    grdPolicyVersions.PageIndex = CInt(ViewState("PageIndex"))
                End If

                If oPolicyDetail.IsMigratedPolicy Then 'Migrated policies are ulways under renewal
                    'need to show only one link i.e. "Details"
                    'Check the roles before displaying the "Details" link
                    If UserCanDoTask("Renewals") Then
                        'Do'nt show view link.
                        btnView.Visible = False
                        lnkbtnAction.CommandArgument = nInsuranceFileKey
                        lnkbtnAction.Text = GetLocalResourceObject("lbl_Details").ToString() '"details"
                        lnkbtnAction.CommandName = "Renew"
                    Else
                        'Do,nt show change link.Only view link should be displayed
                        lnkbtnAction.Visible = False
                    End If
                ElseIf oPolicyDetail.IsReadOnly Then
                    btnView.Visible = True
                    lnkbtnAction.Visible = False
                    liRenew.Visible = False
                    lnkbtnAction.Visible = False
                End If
            End If
        End Sub


        ''' <summary>
        ''' To check if the policies are renewed
        ''' </summary>
        ''' <param name="v_sPolicyRef"></param>
        ''' <param name="v_dtCoverStartDate"></param>
        ''' <param name="v_nInsuranceFileKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function IsRenewed(ByVal v_sPolicyRef As String, ByVal v_dtCoverStartDate As Date, ByVal v_nInsuranceFileKey As Integer) As Boolean
            Dim oPolicyCollection As NexusProvider.PolicyCollection = ViewState(CNPolicyVersions)
            Dim bStatus As Boolean = False
            Dim TempVar As Integer
            For TempVar = 0 To oPolicyCollection.Count - 1
                If oPolicyCollection(TempVar).InsuranceFileTypeCode IsNot Nothing Then
                    If oPolicyCollection(TempVar).Reference.Trim = v_sPolicyRef.Trim And oPolicyCollection(TempVar).CoverStartDate > v_dtCoverStartDate _
                And (oPolicyCollection(TempVar).InsuranceFileTypeCode.Trim.ToUpper = "POLICY" Or _
                oPolicyCollection(TempVar).InsuranceFileTypeCode.Trim.ToUpper = "MTAREINS") _
                And oPolicyCollection(TempVar).InsuranceFileKey <> v_nInsuranceFileKey _
                And oPolicyCollection(TempVar).PolicyStatusCode.Trim.ToUpper <> "CAN" Then
                        bStatus = True
                        'Yes Policy Has been Renewed
                    End If
                End If
            Next
            Return bStatus
        End Function

        ''' <summary>
        ''' To check if the policies are in renewal
        ''' </summary>
        ''' <param name="v_sPolicyRef"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function IsInRenewal(ByVal v_sPolicyRef As String) As Boolean
            Dim oPolicyCollection As NexusProvider.PolicyCollection = ViewState(CNPolicyVersions)
            Dim bStatus As Boolean = False
            Dim TempVar As Integer
            For TempVar = 0 To oPolicyCollection.Count - 1
                If oPolicyCollection(TempVar).InsuranceFileTypeCode IsNot Nothing Then
                    If oPolicyCollection(TempVar).Reference.Trim = v_sPolicyRef.Trim _
                    And oPolicyCollection(TempVar).InsuranceFileTypeCode.Trim.ToUpper = "RENEWAL" And oPolicyCollection(TempVar).IsOutOfSequenceReplaced <> True Then
                        bStatus = True
                        'Yes Policy Has been Renewed
                    End If
                End If
            Next
            Return bStatus
        End Function


        ''' <summary>
        ''' To check if the policies are re-instated
        ''' </summary>
        ''' <param name="v_sPolicyRef"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function IsReinstated(ByVal v_sPolicyRef As String) As Boolean
            Dim oPolicyCollection As NexusProvider.PolicyCollection = ViewState(CNPolicyVersions)
            Dim bStatus As Boolean = False
            Dim TempVar As Integer
            For TempVar = 0 To oPolicyCollection.Count - 1
                If oPolicyCollection(TempVar).InsuranceFileTypeCode IsNot Nothing Then
                    If oPolicyCollection(TempVar).Reference.Trim = v_sPolicyRef.Trim _
                    And oPolicyCollection(TempVar).InsuranceFileTypeCode.Trim.ToUpper = "MTAREINS" Then
                        bStatus = True
                        'Yes Policy Has been Cancelled/Lapsed
                        'Check whether it has been reinstated or not
                    End If
                End If
            Next
            Return bStatus
        End Function

        ''' <summary>
        ''' To Set the reinstate insurance file key in view state
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub IsReinstateLink()
            Dim oPolicyDetail As NexusProvider.Policy
            Dim oPolicyVersions As NexusProvider.PolicyCollection = ViewState(CNPolicyVersions)
            For nCnt As Integer = oPolicyVersions.Count - 1 To 0 Step -1
                oPolicyDetail = oPolicyVersions(nCnt)
                If oPolicyDetail.InsuranceFileTypeCode.Trim().ToUpper() = "MTACAN" And oPolicyDetail.BaseInsuranceFileKey = 0 Or oPolicyDetail.BaseInsuranceFileKey = oPolicyDetail.InsuranceFileKey Then
                    ViewState(CNIsReinstateLink) = oPolicyDetail.InsuranceFileKey
                    Exit For
                End If
            Next
        End Sub

        ''' <summary>
        ''' Check if any non-quote policy version is in LIVE state
        ''' </summary>
        ''' <returns></returns>
        Private Function HasLivePolicyVersion() As Boolean
            Dim oPolicyVersions As NexusProvider.PolicyCollection = ViewState(CNPolicyVersions)
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
            Return False
        End Function

        Private Function ShowHideVoidButton(ByVal v_lInsuranceFileKey As Integer) As Boolean
            Dim bShowVoidButton As Boolean = False
            Dim bInstalmentExists As Boolean = False
            Dim bQuoteExists As Boolean = False
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim sb As New StringBuilder
            sb.Append(GetLocalResourceObject("msg_ConfirmVoidPolicyVersion").ToString.Replace("\n", Environment.NewLine))

            If v_lInsuranceFileKey = 0 Then
                v_lInsuranceFileKey = Session(CNInsuranceFileKey)
            End If
            If v_lInsuranceFileKey > 0 Then
                oWebService.IsVoidPolicyVersion(v_lInsuranceFileKey, bShowVoidButton, bInstalmentExists, bQuoteExists)
            End If

            If bShowVoidButton Then
                If bInstalmentExists OrElse bQuoteExists Then
                    sb.Append("WARNING: ")
                End If
                If bInstalmentExists Then
                    sb.Append(GetLocalResourceObject("msg_InsPlanExists").ToString.Replace("\n", Environment.NewLine))
                End If
                If bQuoteExists Then
                    sb.Append(GetLocalResourceObject("msg_QuoteExists").ToString.Replace("\n", Environment.NewLine))
                End If
                hvVoidMessage.Value = sb.ToString()
                End If
                Return bShowVoidButton
        End Function

        Protected Sub SetPolicyVersionVoid(ByVal v_lInsuranceFileKey As Integer)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim nInsuranceFolderKey As Integer = Convert.ToInt32(Request.QueryString("InsuranceFolderKey"))
            Dim bCreated As Boolean
            Dim sMessage As String
            If hvVoidConfirm.Value.ToUpper = "TRUE" Then
                If v_lInsuranceFileKey > 0 Then
                    ' Make the latest live version void
                    oWebService.CreateVoidPolicyVersion(v_lInsuranceFileKey, nInsuranceFolderKey, bCreated, sMessage)
                    If bCreated Then
                        BindPolicyVersions(nInsuranceFolderKey, False)
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "CallRenewalConfirmation", "alert('" & GetLocalResourceObject("msg_VoidPolicyFailure").ToString & "');", True)
                    End If

                End If
            End If
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_updated('','text');", True)
        End Sub

    End Class

End Namespace

