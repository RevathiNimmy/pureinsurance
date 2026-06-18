Imports CMS.Library
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports Nexus.Utils
Imports System.Web.Services

Imports SiriusFS.SAM.Client
Imports NexusProvider.Quote
Imports System.Linq
Imports System.Text
Imports System.Xml.Linq
Imports System.IO
Imports Nexus.Constants
Namespace Nexus
    Partial Class secure_agent_RenewalManager : Inherits Frontend.clsCMSPage
        Protected sMsgMigratedLapseConfirmation As String

        Private _InsuranceFileKey As Integer
        Private _ClaimKey As Integer
        Private _PartyKey As Integer
        Private oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())

        Protected Sub grdvRenQuotes_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvRenQuotes.DataBound
            If grdvRenQuotes.Rows.Count = 0 Or grdvRenQuotes.PageCount = 1 Then
                grdvRenQuotes.AllowPaging = False
            End If
        End Sub 'Inherits System.Web.UI.Page '

        Protected Sub grdvRenQuotes_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvRenQuotes.PageIndexChanging
            grdvRenQuotes.PageIndex = e.NewPageIndex
            grdvRenQuotes.DataSource = Session(CNSearchResults)
            grdvRenQuotes.DataBind()
        End Sub
        Protected Sub grdvRenQuotes_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdvRenQuotes.RowCommand
            Dim sMessage As String
            If Not LCase(e.CommandName).Equals("page") Then
                If e.CommandName = "Details" Then

                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim oQuote As NexusProvider.Quote
                    Dim oParty As NexusProvider.BaseParty = Session(CNParty)
                    Dim nInsuranceFolderKey As Integer
                    Dim oExclusiveLocking As NexusProvider.OptionTypeSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ExclusiveLock)
                    If Not LCase(e.CommandName).Equals("page") And Not LCase(e.CommandName).Equals("sort") Then
                        'Check for Exclusive lock
                        Dim GridRow As GridViewRow = CType((e.CommandSource).NamingContainer, GridViewRow)
                        Dim lblInsuranceFolderKey As Label = GridRow.FindControl("lblInsuranceFolderKey")
                        nInsuranceFolderKey = CInt(lblInsuranceFolderKey.Text)
                    End If
                    If oExclusiveLocking.OptionValue = "1" Then
                        Dim sUserName As String = UnlockPolicy(nInsuranceFolderKey)
                        If sUserName.Trim.Length > 0 Then
                            sMessage = "alert('" + Replace(GetLocalResourceObject("lbl_policylocked_error"), "{1}", sUserName + ".") + "')"
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", sMessage, True)
                            Exit Sub
                        End If
                    End If
                    Try
                        oQuote = oWebService.GetHeaderAndSummariesByKey(e.CommandArgument, bExclusiveLock:=True)

                        If Session(CNParty) Is Nothing Then
                            oParty = oWebService.GetParty(oQuote.PartyKey)
                            Session(CNParty) = oParty
                        End If

                        'Locking message is required for details Mode
                        Dim bIgnoreLocking As Boolean = False
                        ' Put highest risk key into Session
                        If Not oQuote.Risks Is Nothing AndAlso oQuote.Risks.Count > 0 Then
                            'Populate XML dataset atleast for first risk as it will help to get datamodal code and quick quote flag
                            For i As Integer = 0 To oQuote.Risks.Count - 1
                                oWebService.GetRisk(oQuote.Risks(i).Key, i, oQuote, oQuote.BranchCode, v_bIgnoreLocking:=bIgnoreLocking)
                            Next
                        End If

                        oWebService.GetHeaderAndRisksByKey(oQuote)
                        Session(CNCurrenyCode) = oQuote.CurrencyCode
                        Session(CNQuote) = oQuote

                        'Use the GetDataSetDefinition to interogate the dataset to get the datamodelcode into session
                        GetDataSetDefinition()


                    Catch ex As NexusProvider.NexusException
                        'Policy locking error
                        Select Case CType(ex.Errors(0), NexusProvider.NexusError).Code
                            Case "200", "1000158" 'Policy Locking
                                'Show policy locking error as alert
                                Dim sLockingMessage As String = "alert('" + ex.Errors(0).Description + ".\n" + ex.Errors(0).Detail + "')"
                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", sLockingMessage, True)
                                Server.ClearError()
                                ClearQuote()
                                Exit Sub
                            Case Else
                                Throw
                        End Select
                    Finally

                    End Try

                    Dim bIsPendingPortfolioTransfer, bIsPendingCloneTransfer As Boolean
                    oWebService.IsPendingTransfer(oQuote.InsuranceFileKey, bIsPendingCloneTransfer, bIsPendingPortfolioTransfer, oQuote.InsuranceFileRef)
                    sMessage = ""
                    If bIsPendingCloneTransfer OrElse bIsPendingPortfolioTransfer Then
                        If bIsPendingPortfolioTransfer Then
                            sMessage = Convert.ToString(GetLocalResourceObject("msg_PendingPortfolioTransfer"))
                        ElseIf bIsPendingCloneTransfer Then
                            sMessage = Convert.ToString(GetLocalResourceObject("msg_PendingClonedTransfer"))
                        End If
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "PendingPortfolioTransfer", "alert('" + sMessage + "')", True)
                        Exit Sub
                    End If
                    Session.Remove(CNMTAType)
                    Session.Remove(CNMTATypeDesc)
                    Session.Remove(CNRenewal)
                    Session.Remove(CNRenewalShowPremium)
                    Session.Remove(CNRiskType)
                    Session.Remove(CNMode)
                    Session(CNMode) = Mode.Buy
                    Session.Remove(CNOI)
                    Session(CNQuoteInSync) = False
                    Session.Remove(CNStatus)
                    Session(CNRenewal) = True
                    Session.Remove(CNQuoteMode)
                    Session(CNQuoteMode) = QuoteMode.FullQuote

                    ' --- Renewal discount auto-apply for Recurring = "Policy" (3) ---
                    ' NOTE: The BO (bSIRRenSelectionBusiness) already applies the discount to the
                    ' renewal quote during renewal selection. We only need to read the discount
                    ' info and populate session so the UI displays correctly.
                    Try
                        Dim nOriginalKey As Integer = If(oQuote.OriginalInsuranceFileKey > 0, oQuote.OriginalInsuranceFileKey, oQuote.InsuranceFileKey)
                        Dim oOriginalDiscount As NexusProvider.PolicyDiscount = oWebService.GetPolicyDiscountInfo(nOriginalKey)
                        If oOriginalDiscount IsNot Nothing AndAlso oOriginalDiscount.IsDiscountApplied Then
                            If oOriginalDiscount.RecurringTypeId = 3 Then
                                ' AC: Recurring = "Policy" — BO already applied discount to renewal risks.
                                ' Fetch the actual renewal total to populate session with correct values.
                                ' GetPolicyDiscountTotalPremium returns the already-discounted this_premium
                                ' (BO applied discount during renewal selection). Back-calculate the original.
                                Dim crRenewalDiscounted As Decimal = oWebService.GetPolicyDiscountTotalPremium(oQuote.InsuranceFileKey)
                                Dim crOriginalTotal As Decimal = Math.Round(crRenewalDiscounted / CDec(1 + oOriginalDiscount.DiscountPercentage / 100), 2)

                                Session(CNPolicyDiscountReasonId) = oOriginalDiscount.DiscountReasonId
                                Session(CNPolicyDiscountPercentage) = oOriginalDiscount.DiscountPercentage
                                Session(CNPolicyDiscountedPremium) = crRenewalDiscounted
                                Session(CNPolicyDiscountTotalPremium) = crOriginalTotal
                                Session(CNPolicyDiscountRecurringTypeId) = oOriginalDiscount.RecurringTypeId
                                Session(CNPolicyDiscountApplied) = True
                                Session("POLICY_DISCOUNT_APPLIED_TO_RISKS") = True
                            Else
                                ' AC: Recurring = "This Transaction" (1) or "This Term" (2) — no carry on renewal
                                Session(CNPolicyDiscountApplied) = False
                                Session(CNPolicyDiscountRecurringTypeId) = oOriginalDiscount.RecurringTypeId
                                Session.Remove(CNPolicyDiscountReasonId)
                                Session.Remove(CNPolicyDiscountPercentage)
                                Session.Remove(CNPolicyDiscountedPremium)
                                Session.Remove(CNPolicyDiscountTotalPremium)
                            End If
                        End If
                    Catch ex As Exception
                        ' Log error and continue — user can apply discount manually on PremiumDisplay
                    End Try
                    ' --- End renewal discount ---

                    DataSetFunctions.GetScreens()
                    If DataSetFunctions.sSummaryOfCover.ToLower = "true" Then
                        oWebService.GetHeaderAndRisksByKey(oQuote)
                        Session(CNQuote) = oQuote
                        Response.Redirect(DataSetFunctions.sSummaryOfCoverURL, True)
                    Else
                        Response.Redirect("~/secure/PremiumDisplay.aspx", True)
                    End If
                End If
                If e.CommandName = "viewunderRenewalpolicy" Then

                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim oQuote As NexusProvider.Quote
                    Dim oParty As NexusProvider.BaseParty = Session(CNParty)
                    Dim nInsuranceFolderKey As Integer
                    If Not LCase(e.CommandName).Equals("page") And Not LCase(e.CommandName).Equals("sort") Then
                        'Check for Exclusive lock
                        Dim GridRow As GridViewRow = CType((e.CommandSource).NamingContainer, GridViewRow)
                        Dim lblInsuranceFolderKey As Label = GridRow.FindControl("lblInsuranceFolderKey")
                        nInsuranceFolderKey = CInt(lblInsuranceFolderKey.Text)
                    End If
                    Dim sUserName As String = UnlockPolicy(nInsuranceFolderKey)
                    If sUserName.Trim.Length > 0 Then
                        Dim sMessagePolicylocked As String = "alert('" + Replace(GetLocalResourceObject("lbl_policylocked_error"), "{1}", sUserName + ".") + "')"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", sMessagePolicylocked, True)
                        Exit Sub
                    End If

                    Try
                        oQuote = oWebService.GetHeaderAndSummariesByKey(e.CommandArgument, bExclusiveLock:=True)

                        If Session(CNParty) Is Nothing Then
                            oParty = oWebService.GetParty(oQuote.PartyKey)
                            Session(CNParty) = oParty
                        End If

                        'Put highest risk key into Session
                        For i As Integer = 0 To oQuote.Risks.Count - 1
                            oWebService.GetRisk(oQuote.Risks(i).Key, i, oQuote, oQuote.BranchCode)
                        Next

                        Session(CNCurrenyCode) = oQuote.CurrencyCode
                        Session(CNQuote) = oQuote

                        'Use the GetDataSetDefinition to interogate the dataset to get the datamodelcode into session
                        GetDataSetDefinition()
                    Finally

                    End Try

                    Session.Remove(CNMTAType)
                    Session.Remove(CNMTATypeDesc)
                    Session.Remove(CNRenewal)
                    Session.Remove(CNRenewalShowPremium)
                    Session.Remove(CNRiskType)
                    Session.Remove(CNMode)
                    Session(CNMode) = Mode.View
                    Session.Remove(CNOI)
                    Session(CNQuoteInSync) = False
                    ' Session(CNInsuranceFileKey) = e.CommandArgument
                    Session.Remove(CNStatus)
                    Session(CNRenewal) = True
                    Session.Remove(CNQuoteMode)
                    Session(CNQuoteMode) = QuoteMode.FullQuote
                    DataSetFunctions.GetScreens()
                    If DataSetFunctions.sSummaryOfCover.ToLower = "true" Then
                        oWebService.GetHeaderAndRisksByKey(oQuote)
                        Session(CNQuote) = oQuote
                        Response.Redirect(DataSetFunctions.sSummaryOfCoverURL, True)
                    Else
                        Response.Redirect("~/secure/PremiumDisplay.aspx", True)
                    End If
                    'Response.Redirect("~/secure/PremiumDisplay.aspx", True)
                End If
            End If

        End Sub

        Protected Sub grdvRenQuotes_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles grdvRenQuotes.RowCreated
            If (e.Row.RowType = DataControlRowType.Header OrElse e.Row.RowType = DataControlRowType.DataRow) Then
                Dim oAllowPolicyClientAssociationsOptionSettings As NexusProvider.OptionTypeSetting = CType(ViewState("AllowPolicyClientAssociationsOptionSettings"), NexusProvider.OptionTypeSetting)

                'Hide the PolicyAssociate Column if the Hidden option to show PolicyClientAssociate is False
                If oAllowPolicyClientAssociationsOptionSettings IsNot Nothing AndAlso oAllowPolicyClientAssociationsOptionSettings.OptionValue = "1" Then
                    grdvRenQuotes.Columns(8).Visible = True
                Else
                    grdvRenQuotes.Columns(8).Visible = False
                End If
            End If

        End Sub
        Protected Sub grdvRenQuotes_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvRenQuotes.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("InsuranceFileKey") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey)

                e.Row.Cells(0).Visible = False
                Dim lnkbtnSelect As LinkButton = e.Row.Cells(8).FindControl("lnkbtnSelect")
                lnkbtnSelect.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                Dim cb As CheckBox = CType(e.Row.FindControl("chkSelection"), CheckBox)

                'Dim ChkSelect As CheckBox = e.Row.FindControl("chkSelection")
                'ChkSelect.Checked = CType(e.Row.DataItem, NexusProvider.Policy).IsSelected
                ClientScript.RegisterArrayDeclaration("CheckBoxIDs", String.Concat("'", cb.ClientID, "'"))
                cb.Attributes.Add("onclick", "CallMe('" & cb.ClientID & "','" & CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey & "','" & CType(e.Row.DataItem, NexusProvider.Policy).IsMigratedPolicy & "','" & CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFolderKey & "')")
                If CType(e.Row.DataItem, NexusProvider.Policy).IsMarketPlacePolicy Then
                    cb.Attributes.Add("Onclick", "javascript:return MarketPlacePolicyConfirmation();")
                    lnkbtnSelect.Attributes.Add("Onclick", "javascript:return MarketPlacePolicyConfirmation();")
                End If

                Dim oAllowPolicyClientAssociationsOptionSettings As NexusProvider.OptionTypeSetting
                oAllowPolicyClientAssociationsOptionSettings = CType(ViewState("AllowPolicyClientAssociationsOptionSettings"), NexusProvider.OptionTypeSetting)
                If oAllowPolicyClientAssociationsOptionSettings IsNot Nothing AndAlso oAllowPolicyClientAssociationsOptionSettings.OptionValue = "1" Then
                    If e.Row.RowType = DataControlRowType.DataRow Then
                        Dim xmldoc As New System.Xml.XmlDocument
                        If e.Row.DataItem IsNot Nothing Then
                            If (CType(e.Row.DataItem, NexusProvider.Policy).AssociatedClients IsNot Nothing AndAlso Not (String.IsNullOrEmpty(CType(e.Row.DataItem, NexusProvider.Policy).AssociatedClients))) Then
                                xmldoc.InnerXml = CType(e.Row.DataItem, NexusProvider.Policy).AssociatedClients

                                Dim rptrFolderNavigation As Repeater = e.Row.FindControl("rptrAssociateClient")
                                If rptrFolderNavigation IsNot Nothing Then
                                    rptrFolderNavigation.DataSource = xmldoc.SelectNodes("/Associates/Associate")
                                    rptrFolderNavigation.DataBind()
                                End If
                            End If
                        End If
                    End If
                End If
            ElseIf e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(0).Visible = False
            End If

        End Sub
        Public Function GetInsuranceFileKeys() As StringBuilder
            Dim i As Integer
            Dim isChecked As Boolean
            Dim StrInsuranceFiles As New StringBuilder
            'This will Loop through all the Rows inside a GridView
            For i = 0 To grdvRenQuotes.Rows.Count - 1

                Dim row As GridViewRow
                row = grdvRenQuotes.Rows(i)
                'Find the checkbox control if it is checked
                isChecked = CType(row.FindControl("chkSelection"), CheckBox).Checked

                'if checked then append the Policy to the String Builder
                If (isChecked) Then
                    Dim iInsurancefilekey As Integer
                    iInsurancefilekey = grdvRenQuotes.DataKeys(i).Values(1).ToString
                    StrInsuranceFiles.Append(iInsurancefilekey & " ")
                    CType(row.FindControl("chkSelection"), CheckBox).Checked = False
                End If

            Next
            Return StrInsuranceFiles
        End Function


        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            sMsgMigratedLapseConfirmation = GetLocalResourceObject("msg_MigratedLapseConfirmation")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "DeleteConfirmation", _
                "<script language=""JavaScript"" type=""text/javascript"">function DeleteConfirmation(){return confirm('" & GetLocalResourceObject("msg_Delete").ToString() & "');}</script>")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "MarketPlacePolicyConfirmation", _
                        "<script language=""JavaScript"" type=""text/javascript"">function MarketPlacePolicyConfirmation(){var IsConfirm; IsConfirm=confirm('" & GetLocalResourceObject("msg_ConfirmMarketPlacePolicy1").ToString() & "'); if(IsConfirm==true) { IsConfirm=confirm('" & GetLocalResourceObject("msg_ConfirmMarketPlacePolicy2").ToString() & "'); return IsConfirm; } else {return IsConfirm;} }</script>")
        End Sub


        Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
            Dim sLockedItems As String = ""
            Dim StrInsuranceFilesGroup As StringBuilder
            StrInsuranceFilesGroup = GetInsuranceFileKeys()
            If StrInsuranceFilesGroup.Length = 0 Then
                lblNoPoliciesSelected.Visible = True
            Else
                Dim oPolCol As NexusProvider.PolicyCollection = Session(CNSearchResults)
                Dim oLockCollection As NexusProvider.LockCollection
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oExclusiveLocking As NexusProvider.OptionTypeSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ExclusiveLock)
                If oExclusiveLocking.OptionValue = "1" Then
                    oLockCollection = oWebService.GetLockDetails(0)

                    For iCount As Integer = 0 To oPolCol.Count - 1
                        Dim nInsuranceFolderKey As Integer = oPolCol(iCount).InsuranceFolderKey
                        Dim sInsuranceFileRef As String = oPolCol(iCount).InsuranceFileRef

                        For Each oLockItem As NexusProvider.Locks In oLockCollection
                            If HttpContext.Current.Session(CNLoginName).Trim().ToUpper <> oLockItem.LockUserName.Trim().ToUpper AndAlso oLockItem.LockName.Trim() = "insurance_folder_cnt" _
                               AndAlso oLockItem.LockValue = nInsuranceFolderKey Then
                                'Return oLockItem.LockUserName & "," & iCount.ToString()
                                sLockedItems += "Policy " & sInsuranceFileRef.Trim & " Locked by : " & oLockItem.LockUserName & "\n"
                            End If
                        Next
                    Next

                    If sLockedItems <> "" Then
                        sLockedItems += "Please try later."
                        Dim sMessage As String = "alert('" + sLockedItems + " ')"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylockedLapse44", sMessage, True)
                        Exit Sub
                    End If
                End If
                For iCount As Integer = 0 To oPolCol.Count - 1
                    If oPolCol(iCount).IsSelected = True Then
                        Dim oQuote As NexusProvider.Quote
                        oQuote = oWebService.GetHeaderAndSummariesByKey(oPolCol(iCount).InsuranceFileKey)
                        oWebService.DeleteRenewal(oQuote, oQuote.BranchCode)
                    End If
                Next
                FillRenewalRecords()
            End If
        End Sub
        Protected Sub PopulateDropDown()
            Dim oLookUP, oStatusLookup As New NexusProvider.LookupListCollection
            Dim oWebService = New NexusProvider.ProviderManager().Provider
            oLookUP = oWebService.GetList(NexusProvider.ListType.PMLookup, "Product", True, False, "is_renewable", "1")
            oStatusLookup = oWebService.GetList(NexusProvider.ListType.PMLookup, "Renewal_Status_Type", True, False)
            ddlProductType.Items.Clear()
            RenewalStatusType.Items.Clear()

            Dim sAllowedAgent() As String
            Dim oUserDetails As NexusProvider.UserDetails = Session.Item(CNAgentDetails)
            Dim iCounter As Integer = 0
            Dim bMatched As Boolean = False
            Dim UserRoles As String
            Dim oProducts As Config.Products = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Products
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim f_flagProduct As NexusProvider.LookupListItem

            ddlProductType.Items.Clear()

            For Each oProduct As Config.Product In oProducts
                'Retreive all the roles set for product in web.config
                UserRoles = oProduct.AllowRole

                'Roles is  available

                'Issue # 2191
                If oUserDetails IsNot Nothing AndAlso oUserDetails.Key = 0 Then ' In case of UW
                    txtAgentCode.Enabled = True
                    btnAgentCode.Enabled = True
                Else ' In case of Broker Login
                    If oQuote Is Nothing Then
                        txtAgentCode.Text = CType(Session(CNAgentDetails), NexusProvider.UserDetails).PartyName 'ResolvedName
                        txtAgentKey.Value = CType(Session(CNAgentDetails), NexusProvider.UserDetails).Key
                    Else
                        txtAgentCode.Text = oQuote.AgentCode
                        txtAgentKey.Value = oQuote.Agent
                    End If
                    txtAgentCode.Enabled = False
                    btnAgentCode.Enabled = False
                End If

                If UserRoles IsNot Nothing AndAlso UserIsInRoles(UserRoles) = True _
                       AndAlso FrameWorkFunctions.IsProductAssignedToUserBranch(oProduct, CType(Session(CNAgentDetails), NexusProvider.UserDetails).AvailableUserProductsByBranch) Then
                    'if logged user is agent
                    If CType(Session(CNLoginType), LoginType) = LoginType.Agent Then
                        If String.IsNullOrEmpty(oProduct.AllowedAgent.Trim) Then
                            bMatched = True
                            'txtAgentCode.Text = oQuote.AgentCode
                        Else
                            sAllowedAgent = oProduct.AllowedAgent.Split(",")
                            For iCounter = 0 To sAllowedAgent.Length - 1
                                If sAllowedAgent(iCounter).ToUpper() = oUserDetails.PartyName.ToUpper() Then
                                    bMatched = True
                                    Exit For
                                End If
                            Next

                        End If
                    Else
                        'for Direct Customer
                        bMatched = True
                    End If
                Else
                    'Roles is not available
                    bMatched = False
                End If
                'if bMatch is True means product will be added
                If bMatched = True Then
                    f_flagProduct = oLookUP.FindItemByCode(oProduct.ProductCode)
                    If f_flagProduct IsNot Nothing Then
                        ddlProductType.Items.Add(New ListItem(oProduct.Name + "-" + oProduct.ProductCode, oProduct.ProductCode))
                    End If
                End If
                f_flagProduct = Nothing
                bMatched = False
            Next

            For iStatusCount As Integer = 0 To oStatusLookup.Count - 1
                Dim lstStatusCount As New ListItem
                lstStatusCount.Text = oStatusLookup(iStatusCount).Description
                lstStatusCount.Value = Trim(oStatusLookup(iStatusCount).Code)
                RenewalStatusType.Items.Add(lstStatusCount)
                RenewalStatusType.DataBind()
            Next
            RenewalStatusType.Items.Insert(0, New ListItem("(all)", "0"))
            If Session(CNAgentDetails) IsNot Nothing Then
                Dim oUserDetailsNew As NexusProvider.UserDetails
                oUserDetailsNew = Session(CNAgentDetails)
                BranchCode.DataSource = oUserDetailsNew.ListOfBranches
                BranchCode.DataTextField = "Description"
                BranchCode.DataValueField = "Code"
                BranchCode.DataBind()
                BranchCode.Items.Insert(0, New ListItem("(all)", "0"))
            End If
            If oUserDetails.Key <> 0 Then
                If RenewalStatusType.Items.Count > 0 Then
                    RenewalStatusType.SelectedValue = "Update"
                End If
            End If
        End Sub
        Protected Sub clearPanel(ByVal oPolCol As NexusProvider.PolicyCollection)
            If oPolCol IsNot Nothing Then
                If oPolCol.Count > 0 Then
                    pnlButtons.Visible = True
                Else
                    pnlButtons.Visible = False
                End If
            Else
                pnlButtons.Visible = False
            End If
        End Sub
        Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
            txtAgentCode.Text = hdnAgentCode.Value
            FillRenewalRecords()
        End Sub
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            'To set the Focus
            Page.SetFocus(RenewalStatusType)



            If Not Session(CNIsLapsed) Is Nothing AndAlso CBool(Session(CNIsLapsed)) = True Then
                FillRenewalRecords()
                Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim oPortalConfig As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
                'check the web config at portal to show email model page
                If oPortalConfig.PolicyLapseEmail = True Then
                    SendMail()
                End If
                Session(CNIsLapsed) = Nothing
                Session.Remove(CNIsLapsed)
            End If
            If HttpContext.Current.Session.IsCookieless Then
                'Lapse Link
                btnLapse.OnClientClick = "if(vInsuranceFileRef.length > 0){publishMessage('POLICY',vInsuranceFileRef,'LAPSE')};tb_show(null , '" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/RenewalLapseReason.aspx?PostbackTo=" & UpdRenewal.ClientID.ToString & "&modal=true&Type=All&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            Else
                'Status Link
                btnStatus.OnClientClick = "tb_show(null , '../Modal/RenewalStatusType.aspx?PostbackTo=" & UpdRenewal.ClientID.ToString & "&modal=true&Type=All&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                'Lapse Link
                btnLapse.OnClientClick = "if(vInsuranceFileRef.length > 0){publishMessage('POLICY',vInsuranceFileRef,'LAPSE')};tb_show(null , '../Modal/RenewalLapseReason.aspx?PostbackTo=" & UpdRenewal.ClientID.ToString & "&modal=true&Type=All&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            End If

            If Not IsPostBack Then
                ClearClaims()
                ClearHeader()
                ClearQuote()
                GetSystemOption()
                'Reset The values
                txtAgentCode.Text = String.Empty
                txtAgentKey.Value = String.Empty
                txtClient.Text = String.Empty
                txtClientKey.Value = String.Empty
                txtPolicyNo.Text = String.Empty
                txtAgentCode.Attributes.Add("readonly", "readonly")
                txtClient.Attributes.Add("readonly", "readonly")

                If Session(CNRiskViewStartPoint) IsNot Nothing AndAlso Session(CNRiskViewStartPoint) = "ClientManager" Then
                    Session.Remove(CNRiskViewStartPoint)
                End If
            Else
                If chkRenewalDate.Checked = False Then
                    txtRenewalDate.Text = DateTime.Now.ToShortDateString
                End If
                HttpContext.Current.Session("SelectedPolicies") = Nothing
            End If

            'On Change of the status of the policy
            If Request("__EVENTARGUMENT") = "RefreshPolicy" Then
                'Reset numeber of selected migrated policies to zero on postback from LapseReason page
                hfSelectedMigratedPolicy.Value = 0
                FillRenewalRecords()
            End If

            If Request("__EVENTARGUMENT") = "RefreshPolicy_lps" Then
                Session(CNIsLapsed) = True
                Response.Redirect(ConfigurationManager.AppSettings("WebRoot") & "/secure/RenewalManager.aspx")
            End If

            'Will check the user's Authority to perform different Tasks such as Transfer, Lapse, Delete and Status Updation
            CheckUserTasks()
        End Sub
        Protected Sub GetSystemOption()
            Dim oAllowPolicyClientAssociationsOptionSettings As NexusProvider.OptionTypeSetting
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

            oAllowPolicyClientAssociationsOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.AllowPolicyClientAssociations)
            ViewState("AllowPolicyClientAssociationsOptionSettings") = oAllowPolicyClientAssociationsOptionSettings

            oWebService = Nothing
        End Sub
        Protected Sub CheckUserTasks()
            'Check if user has the Authority to perform Lapse Task, then make visible the lapse button
            If UserCanDoTask("ShowRenManagerLapseButton") Then
                btnLapse.Visible = True
            Else
                btnLapse.Visible = False
            End If
            'Check if user has the Authority to perform Delete Task, then make visible the delete button
            If UserCanDoTask("ShowRenManagerDeleteButton") Then
                btnDelete.Visible = True
            Else
                btnDelete.Visible = False
            End If
            'Check if user has the Authority to change the Status, then make the status button visible
            If UserCanDoTask("ShowRenManagerStatusButton") Then
                btnStatus.Visible = True
            Else
                btnStatus.Visible = False
            End If
            'Check if user has the Authority to select the Agent Code, then make the Agent Code Button Enabled
            If UserCanDoTask("EnableRenManagerAgentCode") Then
                btnAgentCode.Enabled = True
            Else
                btnAgentCode.Enabled = False
            End If
            'Check if user has the Authority to change the Renewal Status, then make the Renewal Status Dropdown Enabled
            If UserCanDoTask("EnableRenManagerStatus") Then
                RenewalStatusType.Enabled = True
            Else
                RenewalStatusType.Enabled = False
            End If
        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender


            Dim eventArgument As String = Request("__EVENTARGUMENT")
            If Not IsNothing(eventArgument) Then
                eventArgument = eventArgument.Trim.ToUpper()
            End If

            If Not IsPostBack OrElse eventArgument = "SELECTBRANCH" Then
                PopulateDropDown()
                btnNewSearch_Click(btnNewSearch, Nothing)
            End If

            If Not IsPostBack Then
                Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
                Session(CNParty) = Nothing
                txtRenewalDate.Text = Date.Today.ToShortDateString
                'This will populate the dropdowns from the NexusLookups

                If oUserDetails IsNot Nothing Then
                    If oUserDetails.Key > 0 Then
                        'As per WPR 73_74, Agent Panel should be enabled or disable on the basis of User Authority and not logged in User Type.
                        'So disbaled the code for that purpose.
                        'AgentPanel.Visible = False

                        hvIsAgent.Value = 1 'To check if logged in user is an Agent
                    Else
                        'As per WPR 73_74, Agent Panel should be enabled or disable on the basis of User Authority and not logged in User Type.
                        'So disbaled the code for that purpose.
                        'AgentPanel.Visible = True

                        hvIsAgent.Value = 0 'To check if logged in user is an Underwriter
                    End If
                Else
                    AgentPanel.Visible = True
                End If
            End If

            'This will populate search modals
            If HttpContext.Current.Session.IsCookieless Then
                'btnStatus.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/RenewalStatusType.aspx?PostbackTo=" & UpdRenewal.ClientID.ToString & "&modal=true&Type=All&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                btnAgentCode.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/FindAgent.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                btnClient.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/secure/agent/FindClient.aspx?RequestPage=BG&modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=500&width=800' , null);return false;"
            Else
                'btnStatus.OnClientClick = "tb_show(null , '../Modal/RenewalStatusType.aspx?PostbackTo=" & UpdRenewal.ClientID.ToString & "&modal=true&Type=All&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                btnClient.OnClientClick = "tb_show(null ,'../secure/agent/FindClient.aspx?RequestPage=BG&modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=500&width=800' , null);return false;"
                btnAgentCode.OnClientClick = "tb_show(null ,'../Modal/FindAgent.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            End If
        End Sub
        Sub FillRenewalRecords()
            Dim oPortalConfig As Config.Portal = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID())
            Dim oFilterPolCol As NexusProvider.PolicyCollection = New NexusProvider.PolicyCollection
            If UserCanDoTask("RenewalManager") Then
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
                Dim oPolCol As New NexusProvider.PolicyCollection
                Dim oParty As NexusProvider.BaseParty = Session(CNParty)
                Dim sBranchCode As String
                Dim iAgentKey, iPartyKey As Integer
                Dim dRenewalDate As Date

                'Set the Branch Code
                If BranchCode.SelectedValue IsNot Nothing Then
                    If BranchCode.SelectedValue.Trim.Length = 0 Then
                        sBranchCode = String.Empty
                    Else
                        sBranchCode = BranchCode.SelectedValue
                    End If
                Else
                    sBranchCode = BranchCode.SelectedValue
                End If

                'Set the Agent Key
                If txtAgentCode.Text.Trim.Length <> 0 Then
                    iAgentKey = txtAgentKey.Value
                End If

                'Set the Party Key
                If txtClient.Text.Trim.Length <> 0 Then
                    iPartyKey = txtClientKey.Value
                End If

                'Renewal Date
                If chkRenewalDate.Checked And txtRenewalDate.Text.Length <> 0 Then
                    dRenewalDate = CDate(txtRenewalDate.Text)
                End If

                If oUserDetails IsNot Nothing Then
                    If oUserDetails.Key = 0 AndAlso iAgentKey = 0 Then
                        ' -	Note that if the logged in user is an agent, then the agent code drop down should not be displayed.
                        'AgentPanel.Visible = False
                        'Get all Policies in Renewals for all the client against Agent key
                        oPolCol = oWebService.GetPoliciesInRenewal(iPartyKey, Nothing, ddlProductType.SelectedValue, dRenewalDate, Nothing, Nothing, sBranchCode, v_sInsuranceRef:=IIf(txtPolicyNo.Text = "", Nothing, txtPolicyNo.Text))
                    Else
                        If (ddlResults.SelectedValue = "UserOnly") Then
                            oPolCol = oWebService.GetPoliciesInRenewal(iPartyKey, iAgentKey, ddlProductType.SelectedValue, dRenewalDate, Nothing, Nothing, sBranchCode, v_sInsuranceRef:=IIf(txtPolicyNo.Text = "", Nothing, txtPolicyNo.Text), v_bShowUserOnly:=True)
                        Else
                            oPolCol = oWebService.GetPoliciesInRenewal(iPartyKey, iAgentKey, ddlProductType.SelectedValue, dRenewalDate, Nothing, Nothing, sBranchCode, v_sInsuranceRef:=IIf(txtPolicyNo.Text = "", Nothing, txtPolicyNo.Text))
                        End If

                    End If
                End If
                If oPolCol IsNot Nothing Then
                    'Filter Renewals with Status
                    For Each oPol As NexusProvider.Policy In oPolCol
                        If RenewalStatusType.SelectedValue <> "0" AndAlso oPol.RenewalStatusTypeCode.Trim = RenewalStatusType.SelectedValue Then
                            oFilterPolCol.Add(oPol)
                        ElseIf RenewalStatusType.SelectedValue = "0" Then
                            oFilterPolCol.Add(oPol)
                        End If
                    Next
                    'Bind The Filtered Collection
                    grdvRenQuotes.Visible = True

                    grdvRenQuotes.AllowPaging = True
                    If oFilterPolCol IsNot Nothing AndAlso oFilterPolCol.Count > 0 Then
                        grdvRenQuotes.PageIndex = 0
                        grdvRenQuotes.DataSource = oFilterPolCol
                        grdvRenQuotes.DataBind()
                        ltRenewalMessage.Visible = True
                        ltRenewalMessage.Text = hvRenewalTitle.Value.Replace("XXX", Convert.ToString(oFilterPolCol.Count))
                        GridViewRenewals.Visible = True
                    Else
                        grdvRenQuotes.DataSource = Nothing
                        grdvRenQuotes.DataBind()
                        ltRenewalMessage.Visible = False
                        GridViewRenewals.Visible = False
                    End If
                    HttpContext.Current.Session(CNSearchResults) = oFilterPolCol
                    clearPanel(oFilterPolCol)
                Else
                    grdvRenQuotes.DataSource = Nothing
                    grdvRenQuotes.DataBind()
                    ltRenewalMessage.Visible = False
                    GridViewRenewals.Visible = False
                End If
                HttpContext.Current.Session(CNSearchResults) = oFilterPolCol
                clearPanel(oFilterPolCol)
            End If
        End Sub

        Protected Sub btnNewSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewSearch.Click
            'txtRenewalDate.ReadOnly = True
            'RenewalDate_CalendarLookup.Enabled = False
            txtRenewalDate.Text = Date.Today.ToShortDateString
            chkRenewalDate.Checked = False

            If ddlProductType.Items.Count > 0 Then
                ddlProductType.SelectedIndex = 0
            End If

            If RenewalStatusType.Items.Count > 0 Then
                RenewalStatusType.SelectedIndex = 0
            End If

            If BranchCode.Items.Count > 0 Then
                BranchCode.SelectedIndex = 0
            End If

            txtAgentCode.Text = String.Empty
            txtAgentKey.Value = String.Empty
            txtClient.Text = String.Empty
            txtClientKey.Value = String.Empty
            txtPolicyNo.Text = String.Empty
            pnlButtons.Visible = False
            grdvRenQuotes.DataSource = Nothing
            grdvRenQuotes.DataBind()
            grdvRenQuotes.AllowPaging = True
            grdvRenQuotes.Visible = False
            ltRenewalMessage.Visible = False
            HttpContext.Current.Session(CNSearchResults) = Nothing
            HttpContext.Current.Session("SelectedPolicies") = Nothing
            hdnAgentCode.Value = Nothing
            GridViewRenewals.Visible = False
        End Sub
        <WebMethod()> _
        Public Shared Function SelectRecord(ByVal ChkStatus As String, ByVal v_iInsuranceFileKey As String, ByVal sInsuranceFolderKey As String) As String
            Dim oPolCol As NexusProvider.PolicyCollection = HttpContext.Current.Session(CNSearchResults)
            Dim oSelectedPolicies As Hashtable = New Hashtable
            If HttpContext.Current.Session("SelectedPolicies") IsNot Nothing AndAlso HttpContext.Current.Session("SelectedPolicies").Count > 0 Then
                oSelectedPolicies = HttpContext.Current.Session("SelectedPolicies")
            End If

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = HttpContext.Current.Session(CNQuote)
            Dim bIsReferred As Boolean = False

            If CBool(ChkStatus) Then
                For iCount As Integer = 0 To oPolCol.Count - 1
                    If oPolCol(iCount).InsuranceFileKey = CInt(v_iInsuranceFileKey) Then
                        Dim oLockCollection As NexusProvider.LockCollection
                        Dim nInsuranceFolderKey As Integer = CInt(sInsuranceFolderKey)

                        oLockCollection = oWebService.GetLockDetails(nInsuranceFolderKey)
                        For Each oLockItem As NexusProvider.Locks In oLockCollection
                            If HttpContext.Current.Session(CNLoginName).Trim().ToUpper <> oLockItem.LockUserName.Trim().ToUpper AndAlso
                            oLockItem.LockName.Trim() = "insurance_folder_cnt" AndAlso oLockItem.LockValue = nInsuranceFolderKey Then
                                Return oLockItem.LockUserName & "," & iCount.ToString()
                            End If
                        Next
                        oPolCol(iCount).IsSelected = True
                        oQuote = oWebService.GetHeaderAndSummariesByKey(oPolCol(iCount).InsuranceFileKey)
                        bIsReferred = CBool((From oRisk As NexusProvider.Risk In oQuote.Risks Where oRisk.StatusCode.Trim.ToUpper() = "REFERRED").ToList().Count)
                        oSelectedPolicies.Add(oQuote.InsuranceFileRef, bIsReferred)
                        Exit For
                    End If
                Next
            End If

            'Deselect unchecked policies
            If CBool(ChkStatus) = False Then
                For iCount As Integer = 0 To oPolCol.Count - 1
                    If oPolCol(iCount).InsuranceFileKey = CInt(v_iInsuranceFileKey) Then
                        oPolCol(iCount).IsSelected = False
                        oQuote = oWebService.GetHeaderAndSummariesByKey(oPolCol(iCount).InsuranceFileKey)
                        oSelectedPolicies.Remove(oQuote.InsuranceFileRef)
                        Exit For
                    End If
                Next
            End If

            HttpContext.Current.Session(CNSearchResults) = oPolCol
            HttpContext.Current.Session("SelectedPolicies") = oSelectedPolicies

            Dim sRefrredPolicies As StringBuilder = New StringBuilder
            For Each item In oSelectedPolicies
                Dim sIsReferred As String = CStr(item.value)
                If (sIsReferred = "True") Then
                    If sRefrredPolicies.Length > 0 Then sRefrredPolicies = sRefrredPolicies.Append(",")
                    sRefrredPolicies = sRefrredPolicies.Append(item.key.Trim)
                End If
            Next

            If sRefrredPolicies.Length > 0 Then
                Return "IsReferred;" + sRefrredPolicies.ToString()
            ElseIf oSelectedPolicies.Count = 0 Then
                Return ""
            Else
                Return "Success"
            End If

            oQuote = Nothing
            oWebService = Nothing
        End Function


        Protected Sub SendMail()
            Dim sURL As String
            Dim oParty As NexusProvider.BaseParty = Session(CNParty)
            Dim oQuote As NexusProvider.Quote
            Dim oClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            If Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Or Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                oQuote = Session(CNClaimQuote)
            Else
                oQuote = Session(CNQuote)
            End If

            If oParty Is Nothing AndAlso oQuote IsNot Nothing Then
                Try
                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    If Session(CNParty) Is Nothing Then
                        oParty = oWebService.GetParty(oQuote.PartyKey)
                        Session(CNParty) = oParty
                    End If
                Finally
                End Try
            End If
            If oParty IsNot Nothing AndAlso oQuote IsNot Nothing Then
                If HttpContext.Current.Session.IsCookieless Then
                    sURL = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/SendEmail.aspx?PartyKey=" & oParty.Key & "&key=" & "&InsuranceFileKey=" & oQuote.InsuranceFileKey & "&modal=true&loc=docm&n=p&Riskcheck=true&KeepThis=true&TB_iframe=true&height=300&width=750"
                Else
                    sURL = AppSettings("WebRoot") & "/Modal/SendEmail.aspx?PartyKey=" & oParty.Key & "&key=" & "&InsuranceFileKey=" & oQuote.InsuranceFileKey & "&modal=true&loc=docm&n=p&Riskcheck=true&KeepThis=true&TB_iframe=true&height=300&width=750"
                End If

                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show", "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sURL & "' , null);});</script>")
                Exit Sub
            End If

        End Sub

        Protected Sub btnLapse_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLapse.Click
            'SendMail()
            Dim oPolCol As NexusProvider.PolicyCollection = HttpContext.Current.Session(CNSearchResults)
            Dim oLockCollection As NexusProvider.LockCollection
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oExclusiveLocking As NexusProvider.OptionTypeSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ExclusiveLock)
            If oExclusiveLocking.OptionValue = "1" Then
                oLockCollection = oWebService.GetLockDetails(0)

                Dim sLockedItems As String = ""
                For iCount As Integer = 0 To oPolCol.Count - 1
                    Dim nInsuranceFolderKey As Integer = oPolCol(iCount).InsuranceFolderKey
                    Dim sInsuranceFileRef As String = oPolCol(iCount).InsuranceFileRef

                    For Each oLockItem As NexusProvider.Locks In oLockCollection
                        If HttpContext.Current.Session(CNLoginName).Trim().ToUpper <> oLockItem.LockUserName.Trim().ToUpper AndAlso oLockItem.LockName.Trim() = "insurance_folder_cnt" _
                           AndAlso oLockItem.LockValue = nInsuranceFolderKey Then
                            'Return oLockItem.LockUserName & "," & iCount.ToString()
                            sLockedItems += "Policy " & sInsuranceFileRef.Trim & " Locked by : " & oLockItem.LockUserName & "\n"
                        End If
                    Next
                Next

                If sLockedItems <> "" Then
                    sLockedItems += "Please try later."
                    Dim sMessage As String = "alert('" + sLockedItems + " ')"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylockedLapse44", sMessage, True)
                    Exit Sub
                End If
            End If
            Dim sURL As String = ""
            If HttpContext.Current.Session.IsCookieless Then
                sURL = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/RenewalLapseReason.aspx?PostbackTo=" & UpdRenewal.ClientID.ToString & "&modal=true&Type=All&KeepThis=true&TB_iframe=true&height=500&width=750"
            Else
                sURL = AppSettings("WebRoot") & "/Modal/RenewalLapseReason.aspx?PostbackTo=" & UpdRenewal.ClientID.ToString & "&modal=true&Type=All&KeepThis=true&TB_iframe=true&height=500&width=750"
            End If
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show", "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sURL & "' , null);});</script>")
            Exit Sub

        End Sub



        Protected Sub btnStatus_Click(sender As Object, e As EventArgs) Handles btnStatus.Click
            Dim oPolCol As NexusProvider.PolicyCollection = HttpContext.Current.Session(CNSearchResults)
            Dim oLockCollection As NexusProvider.LockCollection
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oExclusiveLocking As NexusProvider.OptionTypeSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ExclusiveLock)
            hvChecked.Value = "true"
            If oExclusiveLocking.OptionValue = "1" Then

                oLockCollection = oWebService.GetLockDetails(0)

                Dim sLockedItems As String = ""
                For iCount As Integer = 0 To oPolCol.Count - 1
                    Dim nInsuranceFolderKey As Integer = oPolCol(iCount).InsuranceFolderKey
                    Dim sInsuranceFileRef As String = oPolCol(iCount).InsuranceFileRef

                    For Each oLockItem As NexusProvider.Locks In oLockCollection
                        If HttpContext.Current.Session(CNLoginName).Trim().ToUpper <> oLockItem.LockUserName.Trim().ToUpper AndAlso oLockItem.LockName.Trim() = "insurance_folder_cnt" _
                           AndAlso oLockItem.LockValue = nInsuranceFolderKey Then

                            sLockedItems += "Policy " & sInsuranceFileRef.Trim & " Locked by : " & oLockItem.LockUserName & "\n"
                        End If
                    Next
                Next

                If sLockedItems <> "" Then
                    sLockedItems += "Please try later."
                    Dim sMessage As String = "alert('" + sLockedItems + " ')"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylockedstatus1", sMessage, True)
                    Exit Sub
                End If
            End If
            Dim sURL As String = ""
            If HttpContext.Current.Session.IsCookieless Then
                sURL = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/RenewalStatusType.aspx?PostbackTo=" & UpdRenewal.ClientID.ToString & "&modal=true&Type=All&KeepThis=true&TB_iframe=true&height=500&width=750"
            Else
                sURL = "../Modal/RenewalStatusType.aspx?PostbackTo=" & UpdRenewal.ClientID.ToString & "&modal=true&Type=All&KeepThis=true&TB_iframe=true&height=500&width=750"
            End If
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show", "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sURL & "' , null);});</script>")
            Exit Sub
        End Sub
        Private Function UnlockPolicy(ByVal nInsuranceFileKey As Integer) As String
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
                If oLockItem.LockName.Trim() = "insurance_folder_cnt" AndAlso oLockItem.LockValue = nInsuranceFileKey Then
                    oLock.LockName = oLockItem.LockName
                    oLock.LockValue = oLockItem.LockValue
                    oLockCollection.Add(oLock)
                    If HttpContext.Current.Session(CNLoginName).ToLower().Trim() = oLockItem.LockUserName.ToLower().Trim() Then
                        bMaintainedSuccess = oWebService.MaintainLock(oLockCollection, bAllClear, bLogout, Session(CNBranchCode).ToString())
                        sUserName = String.Empty
                    Else
                        sUserName = oLockItem.LockUserName.Trim
                    End If
                    Exit For
                End If
            Next
            Return sUserName
        End Function
    End Class
End Namespace
