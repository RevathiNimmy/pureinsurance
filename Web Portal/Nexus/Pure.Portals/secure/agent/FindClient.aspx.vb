Imports System.Web.Configuration
Imports Nexus.Library
Imports CMS.Library
Imports Nexus.Utils
Imports System.Web.HttpContext
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session

Namespace Nexus
    Partial Class secure_FindClient
        Inherits BaseFindParty

        Private sClaimNo As String = String.Empty
        Private sPolicyNo As String = String.Empty
        Private sReturnUrl As String = String.Empty
        Dim oPolicyAssociateCollection As NexusProvider.PolicyAssociateCollection

        Private bIsAnySelected As Boolean = False
        ''' <summary>
        ''' Obtains the search result from database and populates the datagrid
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
            If Page.IsValid Then
                SetClient()
                If Session(CNParty) IsNot Nothing Then
                    Session(CNParty) = Nothing
                End If
                grdvSearchResults.Visible = True
                grdvSearchResults.PageIndex = 0

                If Not String.IsNullOrEmpty(txtPolicyRiskIndex.Text) Then
                    Session(CNRiskIndex) = txtPolicyRiskIndex.Text
                Else
                    Session(CNRiskIndex) = Nothing
                End If
                If Request.QueryString("IncludeAgent") IsNot Nothing AndAlso Request.QueryString("IncludeAgent") <> "" Then
                    FindParty(v_bIncludeAgent:=True, bIsAnySelected:=bIsAnySelected)
                Else
                    FindParty(bIsAnySelected:=bIsAnySelected)
                End If
            End If
        End Sub

        Protected Sub SetClient()

            ' storing the type of client-search in the session
            Select Case ddlClientType.SelectedValue
                Case "PC"
                    Session(CNSearchType) = PartyType.PC
                Case "CC"
                    Session(CNSearchType) = PartyType.CC
                Case Else
                    Session(CNSearchType) = PartyType.GC
                    bIsAnySelected = True
            End Select

        End Sub

        Protected Sub grdvSearchResults_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvSearchResults.Load
            If grdvSearchResults.PageCount = 1 Then
                grdvSearchResults.AllowPaging = False
            End If
        End Sub

        Protected Shadows Sub grdvSearchResults_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdvSearchResults.RowCommand
            If Request.QueryString("modal") IsNot Nothing Then
                If Request.QueryString("modal") = True Then
                    If e.CommandName = "Select" Then
                        If Request.QueryString("RequestPage") = "BG" Or Request("ClaimFlag") = "1" Or Request.QueryString("Page") = "RS" Then
                            Dim oPartyCollection As NexusProvider.PartyCollection = CType(Session(CNSearchData), NexusProvider.PartyCollection)
                            For iTempVar As Integer = 0 To oPartyCollection.Count - 1
                                If grdvSearchResults.DataKeys(e.CommandArgument).Value.ToString = oPartyCollection(iTempVar).UserName Then
                                    Dim sResolvedName As String
                                    sResolvedName = oPartyCollection(iTempVar).ResolvedName.ToString.Replace("'", "\")
                                    ScriptManager.RegisterStartupScript(Me, GetType(String), "closeThickBox", "self.parent.setClient('" + PureEncode(oPartyCollection(iTempVar).UserName.ToString) + "','" + oPartyCollection(iTempVar).Key.ToString + "','" + sResolvedName + "');", True)
                                    Exit For
                                End If
                            Next
                        ElseIf Request.QueryString("Type").Trim.ToUpper = "CLIENT" Then
                            Dim oPartyCollection As NexusProvider.PartyCollection = CType(Session(CNSearchData), NexusProvider.PartyCollection)
                            For iTempVar As Integer = 0 To oPartyCollection.Count - 1
                                If grdvSearchResults.DataKeys(e.CommandArgument).Value.ToString = oPartyCollection(iTempVar).UserName Then
                                    ScriptManager.RegisterStartupScript(Me, GetType(String), "closeThickBox", "self.parent.set" & Request.QueryString("ClientID") & "OtherParty('" + PureEncode(oPartyCollection(iTempVar).ResolvedName.Trim) + "','" + oPartyCollection(iTempVar).Key.ToString + "','" + PureEncode(oPartyCollection(iTempVar).UserName.Trim) + "');", True)
                                    Exit For
                                End If
                            Next
                        End If
                    End If
                End If
            ElseIf Request.QueryString("RequestPage") = "Event" Then
                If e.CommandName = "Select" Then
                    Dim oPartyCollection As NexusProvider.PartyCollection = CType(Session(CNSearchData), NexusProvider.PartyCollection)
                    For iTempVar As Integer = 0 To oPartyCollection.Count - 1
                        If grdvSearchResults.DataKeys(e.CommandArgument).Value.ToString = oPartyCollection(iTempVar).UserName Then
                            Dim oNewParty As NexusProvider.BaseParty
                            Dim oWebservice As NexusProvider.ProviderBase
                            oWebservice = New NexusProvider.ProviderManager().Provider
                            oNewParty = oWebservice.GetParty(oPartyCollection(iTempVar).Key)
                            Session(CNParty) = oNewParty
                            Response.Redirect("~/secure/EventList.aspx", False)
                            Exit For
                        End If
                    Next
                End If
            ElseIf Request.QueryString("Type") IsNot Nothing Then

            End If
        End Sub
        Protected Sub grdvSearchResults_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvSearchResults.RowCreated
            If Request.QueryString("RequestPage") = "BG" Or Request("ClaimFlag") = "1" Or Request.QueryString("RequestPage") = "Event" Then
                If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
                    e.Row.Cells(8).Visible = False
                    e.Row.Cells(9).Visible = True
                End If
            ElseIf Request.QueryString("Type") IsNot Nothing Then
                If Request.QueryString("Type").Trim.ToUpper = "CLIENT" Then
                    If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
                        e.Row.Cells(8).Visible = False
                        e.Row.Cells(9).Visible = True
                    End If
                End If
            ElseIf e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(8).Visible = True
                e.Row.Cells(9).Visible = False
            End If
        End Sub
        Protected Sub grdvSearchResults_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvSearchResults.RowDataBound

            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
                Dim oHyperLink As LinkButton = CType(e.Row.FindControl("lnkDetails"), LinkButton)

                oHyperLink.Attributes.Add("OnClick", "javascript:return ContinueWithServiceLevel('" + CType(e.Row.DataItem, NexusProvider.BaseParty).ServiceLevelCode + "');")
                Session(CNClientMode) = Mode.View
                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("ClaimKey") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.BaseParty).UserName.Replace("""", "&quot;"))

                If Request("ClaimFlag") = "1" Then
                    Dim oPCItem As NexusProvider.PersonalParty = Nothing
                    Dim oCCItem As NexusProvider.CorporateParty = Nothing
                    Dim oAddress As NexusProvider.Address = Nothing

                    Select Case True
                        Case TypeOf e.Row.DataItem Is NexusProvider.PersonalParty

                            oPCItem = CType(e.Row.DataItem, NexusProvider.PersonalParty)
                            oAddress = oPCItem.Addresses(NexusProvider.AddressType.CorrespondenceAddress)
                            If oAddress IsNot Nothing Then
                                'Address 1
                                If oAddress.Address1.Trim().Length = 0 Then
                                    CType(e.Row.FindControl("ltAddress1"), Literal).Text = "&nbsp;"
                                Else
                                    CType(e.Row.FindControl("ltAddress1"), Literal).Text = oAddress.Address1
                                End If

                                'Post Code
                                If oAddress.PostCode.Trim().Length = 0 Then
                                    CType(e.Row.FindControl("ltPostcode"), Literal).Text = "&nbsp;"
                                Else
                                    CType(e.Row.FindControl("ltPostcode"), Literal).Text = oAddress.PostCode
                                End If
                            End If
                            If oPCItem.FileCode.Trim().Length = 0 Then
                                CType(e.Row.FindControl("ltFileCode"), Literal).Text = "&nbsp;"
                            Else
                                CType(e.Row.FindControl("ltFileCode"), Literal).Text = oPCItem.FileCode
                            End If

                            If oPCItem.DOB.ToString.Contains("1899") Then
                                CType(e.Row.FindControl("ltDOBirth"), Literal).Text = "&nbsp;"
                            Else
                                CType(e.Row.FindControl("ltDOBirth"), Literal).Text = oPCItem.DOB
                            End If
                            CType(e.Row.FindControl("ltCustomerType"), Literal).Text = "Personal"

                        Case TypeOf e.Row.DataItem Is NexusProvider.CorporateParty

                            oCCItem = CType(e.Row.DataItem, NexusProvider.CorporateParty)
                            oAddress = oCCItem.Addresses(NexusProvider.AddressType.CorrespondenceAddress)
                            CType(e.Row.FindControl("ltCustomerType"), Literal).Text = "Corporate"
                    End Select
                    If oAddress IsNot Nothing Then
                        'Address 1
                        If oAddress.Address1.Trim().Length = 0 Then
                            CType(e.Row.FindControl("ltAddress1"), Literal).Text = "&nbsp;"
                        Else
                            CType(e.Row.FindControl("ltAddress1"), Literal).Text = oAddress.Address1
                        End If

                        'Post Code
                        If oAddress.PostCode.Trim().Length = 0 Then
                            CType(e.Row.FindControl("ltPostcode"), Literal).Text = "&nbsp;"
                        Else
                            CType(e.Row.FindControl("ltPostcode"), Literal).Text = oAddress.PostCode
                        End If
                    End If
                    If oPCItem IsNot Nothing Then
                        If oPCItem.FileCode.Trim().Length = 0 Then
                            CType(e.Row.FindControl("ltFileCode"), Literal).Text = "&nbsp;"
                        Else
                            CType(e.Row.FindControl("ltFileCode"), Literal).Text = oPCItem.FileCode
                        End If
                    End If
                    If oCCItem IsNot Nothing Then
                        If oCCItem.FileCode.Trim().Length = 0 Then
                            CType(e.Row.FindControl("ltFileCode"), Literal).Text = "&nbsp;"
                        Else
                            CType(e.Row.FindControl("ltFileCode"), Literal).Text = oCCItem.FileCode
                        End If
                    End If
                    oHyperLink.Text = GetLocalResourceObject("lbl_select").ToString()
                    oHyperLink.CommandName = "Claim"
                    oHyperLink.CommandArgument = CType(e.Row.DataItem, NexusProvider.BaseParty).UserName
                    If TypeOf e.Row.DataItem Is NexusProvider.PersonalParty Then
                        CType(e.Row.FindControl("ltCustomerType"), Literal).Text = GetLocalResourceObject("lblCustomerTypePersonal").ToString()
                    ElseIf TypeOf e.Row.DataItem Is NexusProvider.CorporateParty Then
                        CType(e.Row.FindControl("ltCustomerType"), Literal).Text = GetLocalResourceObject("lblCustomerTypeCorporate").ToString()
                    End If
                Else
                    Select Case True
                        Case TypeOf e.Row.DataItem Is NexusProvider.PersonalParty

                            Dim oItem As NexusProvider.PersonalParty = CType(e.Row.DataItem, NexusProvider.PersonalParty)
                            Dim oAddress As NexusProvider.Address = oItem.Addresses(NexusProvider.AddressType.CorrespondenceAddress)

                            If oAddress IsNot Nothing Then
                                'Address 1
                                If oAddress.Address1.Trim().Length = 0 Then
                                    CType(e.Row.FindControl("ltAddress1"), Literal).Text = "&nbsp;"
                                Else
                                    CType(e.Row.FindControl("ltAddress1"), Literal).Text = oAddress.Address1
                                End If

                                'Post Code
                                If oAddress.PostCode.Trim().Length = 0 Then
                                    CType(e.Row.FindControl("ltPostcode"), Literal).Text = "&nbsp;"
                                Else
                                    CType(e.Row.FindControl("ltPostcode"), Literal).Text = oAddress.PostCode
                                End If
                                If oItem.FileCode.Trim().Length = 0 Then
                                    CType(e.Row.FindControl("ltFileCode"), Literal).Text = "&nbsp;"
                                Else
                                    CType(e.Row.FindControl("ltFileCode"), Literal).Text = oItem.FileCode
                                End If

                                If oItem.DOB.ToString.Contains("1899") Or oItem.DOB.ToString.Contains("00:00:00") Then
                                    CType(e.Row.FindControl("ltDOBirth"), Literal).Text = "&nbsp;"
                                Else
                                    CType(e.Row.FindControl("ltDOBirth"), Literal).Text = oItem.DOB
                                End If
                            End If
                            If oItem.Type.Trim() = "Personal Client" OrElse oItem.Type.Trim() = "PC" Then
                                CType(e.Row.FindControl("ltCustomerType"), Literal).Text = GetLocalResourceObject("lblCustomerTypePersonal").ToString()

                            ElseIf oItem.Type.Trim() = "Corporate Client" OrElse oItem.Type.Trim() = "CC" Then
                                CType(e.Row.FindControl("ltCustomerType"), Literal).Text = GetLocalResourceObject("lblCustomerTypeCorporate").ToString()
                            ElseIf oItem.Type.Trim() = "Group Client" OrElse oItem.Type.Trim() = "GC" Then
                                CType(e.Row.FindControl("ltCustomerType"), Literal).Text = "Group"
                            Else
                                CType(e.Row.FindControl("ltCustomerType"), Literal).Text = "Other"
                            End If
                            'CType(e.Row.FindControl("ltCustomerType"), Literal).Text = "Personal"
                            oHyperLink.Text = GetLocalResourceObject("lbl_select").ToString()
                            'To check if Quote is Anonymous Quote
                            If Session(CNIsAnonymous) = True Then
                                'For Personal Client
                                'To check if Primary client and Associated Client are same
                                If (oPolicyAssociateCollection IsNot Nothing AndAlso oPolicyAssociateCollection.Count > 0) Then
                                    For Each oAssociate As NexusProvider.PolicyAssociate In oPolicyAssociateCollection
                                        If (Trim(oItem.UserName) = Trim(oAssociate.PartyCode)) Then
                                            'To show validation error message if primary client and associated client are same
                                            oHyperLink.Attributes.Add("OnClick", "alert('" + GetLocalResourceObject("lblAssociateClientMsg").ToString() + "'); return false;")
                                            Exit For
                                        Else
                                            If (oItem.Type = "Personal Client") OrElse oItem.Type.Trim() = "PC" Then
                                                CType(e.Row.FindControl("ltCustomerType"), Literal).Text = GetLocalResourceObject("lblCustomerTypePersonal").ToString()
                                                oHyperLink.PostBackUrl = "~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & oItem.Key & "&Code=" & oItem.UserName & ""
                                            ElseIf (oItem.Type = "Corporate Client") OrElse oItem.Type.Trim() = "CC" Then
                                                CType(e.Row.FindControl("ltCustomerType"), Literal).Text = GetLocalResourceObject("lblCustomerTypeCorporate").ToString()
                                                oHyperLink.PostBackUrl = "~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oItem.Key & "&Code=" & oItem.UserName & ""
                                            End If

                                        End If
                                    Next
                                Else
                                    If (oItem.Type = "Personal Client") OrElse oItem.Type.Trim() = "PC" Then
                                        CType(e.Row.FindControl("ltCustomerType"), Literal).Text = GetLocalResourceObject("lblCustomerTypePersonal").ToString()
                                        oHyperLink.PostBackUrl = "~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & oItem.Key & "&Code=" & oItem.UserName & ""
                                    ElseIf (oItem.Type = "Corporate Client") OrElse oItem.Type.Trim() = "CC" Then
                                        CType(e.Row.FindControl("ltCustomerType"), Literal).Text = GetLocalResourceObject("lblCustomerTypeCorporate").ToString()
                                        oHyperLink.PostBackUrl = "~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oItem.Key & "&Code=" & oItem.UserName & ""
                                    End If
                                End If
                            Else
                                If Request.QueryString("Mode") IsNot Nothing AndAlso Request.QueryString("Mode") = "cardadd" AndAlso ViewState("PaymentHubEnabled") = "1" Then

                                    oHyperLink.PostBackUrl = "~/secure/Payment/OnlineCardPayment.aspx?PartyKey=" & oItem.Key & "&Code=" & oItem.UserName & "&PartyType=PC"
                                Else
                                    If (oItem.Type = "Personal Client") OrElse oItem.Type.Trim() = "PC" Then
                                        CType(e.Row.FindControl("ltCustomerType"), Literal).Text = GetLocalResourceObject("lblCustomerTypePersonal").ToString()
                                        oHyperLink.PostBackUrl = "~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & oItem.Key & "&Code=" & oItem.UserName & ""
                                    ElseIf (oItem.Type = "Corporate Client") OrElse oItem.Type.Trim() = "CC" Then
                                        CType(e.Row.FindControl("ltCustomerType"), Literal).Text = GetLocalResourceObject("lblCustomerTypeCorporate").ToString()
                                        oHyperLink.PostBackUrl = "~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oItem.Key & "&Code=" & oItem.UserName & ""
                                    End If
                                End If
                            End If
                        Case TypeOf e.Row.DataItem Is NexusProvider.CorporateParty

                            Dim oItem As NexusProvider.CorporateParty = CType(e.Row.DataItem, NexusProvider.CorporateParty)
                            Dim oAddress As NexusProvider.Address = oItem.Addresses(NexusProvider.AddressType.CorrespondenceAddress)

                            If oAddress IsNot Nothing Then
                                'Address 1
                                If oAddress.Address1.Trim().Length = 0 Then
                                    CType(e.Row.FindControl("ltAddress1"), Literal).Text = "&nbsp;"
                                Else
                                    CType(e.Row.FindControl("ltAddress1"), Literal).Text = oAddress.Address1
                                End If
                                'Post Code
                                If oAddress.PostCode.Trim().Length = 0 Then
                                    CType(e.Row.FindControl("ltPostCode"), Literal).Text = "&nbsp;"
                                Else
                                    CType(e.Row.FindControl("ltPostCode"), Literal).Text = oAddress.PostCode
                                End If

                                If oItem.FileCode.Trim().Length = 0 Then
                                    CType(e.Row.FindControl("ltFileCode"), Literal).Text = "&nbsp;"
                                Else
                                    CType(e.Row.FindControl("ltFileCode"), Literal).Text = oItem.FileCode
                                End If
                            End If

                            CType(e.Row.FindControl("ltCustomerType"), Literal).Text = GetLocalResourceObject("lblCustomerTypeCorporate").ToString()
                            oHyperLink.Text = GetLocalResourceObject("lbl_select").ToString()
                            'To check if Quote is Anonymous Quote
                            If Session(CNIsAnonymous) = True Then
                                'For Corporate Client
                                'To check if Primary client and Associated Client are same
                                If (oPolicyAssociateCollection IsNot Nothing AndAlso oPolicyAssociateCollection.Count > 0) Then
                                    For Each oAssociate As NexusProvider.PolicyAssociate In oPolicyAssociateCollection
                                        If (Trim(oItem.UserName) = Trim(oAssociate.PartyCode)) Then
                                            'To show validation error message if primary client and associated client are same
                                            oHyperLink.Attributes.Add("OnClick", "alert('" + GetLocalResourceObject("lblAssociateClientMsg").ToString() + "'); return false;")
                                            Exit For
                                        Else
                                            oHyperLink.PostBackUrl = "~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oItem.Key & "&Code=" & oItem.UserName & ""
                                        End If
                                    Next
                                Else
                                    oHyperLink.PostBackUrl = "~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oItem.Key & "&Code=" & oItem.UserName & ""
                                End If
                            Else
                                If Request.QueryString("Mode") IsNot Nothing AndAlso Request.QueryString("Mode") = "cardadd" AndAlso ViewState("PaymentHubEnabled") = "1" Then

                                    oHyperLink.PostBackUrl = "~/secure/Payment/OnlineCardPayment.aspx?PartyKey=" & oItem.Key & "&Code=" & oItem.UserName & "&PartyType=CC"
                                Else
                                    oHyperLink.PostBackUrl = "~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oItem.Key & "&Code=" & oItem.UserName & ""
                                End If
                            End If
                        Case Else
                    End Select
                End If
            End If

        End Sub
        ''' <summary>
        ''' Initlize control with system option
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oPaymentHubEnabled As NexusProvider.OptionTypeSetting
            Dim oOptionSettings As NexusProvider.OptionTypeSetting

            'If System Option for "Enhanced Case Search" is ON then we need to visible case related search criteria and grid column
            oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5099)

            If oOptionSettings IsNot Nothing AndAlso Not String.IsNullOrEmpty(oOptionSettings.OptionValue) Then
                If oOptionSettings.OptionValue(0) <> "0" Then
                    liCaseNumber.Visible = True
                Else
                    liCaseNumber.Visible = False
                End If
            Else
                liCaseNumber.Visible = False
            End If
            oPaymentHubEnabled = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.SystemOptionPaymentHubEnabled)
            ViewState("PaymentHubEnabled") = oPaymentHubEnabled.OptionValue
            Session(CNPaymentHubDetails) = Nothing
            Session(CNCardDetails) = Nothing
            Session(CNCashListItem) = Nothing
            Session(CNFinancePlanDetails) = Nothing
        End Sub

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack AndAlso Request.QueryString("modal") Is Nothing AndAlso Session(CNIsAnonymous) Is Nothing Then
                'Cleaning of the session values
                ClearQuote()
                ClearClaims()
                ClearHeader()
                HttpContext.Current.Session(CNAnonymous) = Nothing
            ElseIf Request.QueryString("modal") = "true" And Session(CNDoNotClearSession) Is Nothing Then
                Session(CNDoNotClearSession) = "true"
            End If

            If Request.QueryString("Mode") IsNot Nothing AndAlso Request.QueryString("Mode") = "ManualTransfer" Then
                Session(CNNoTrans) = "NB"
            Else
                Session(CNNoTrans) = Nothing
            End If

            ' Added to check the bisClientManagerViewOnly
            Dim bAddClient As Boolean
            Dim bIsClientManagerViewOnly As Boolean
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            'Checking of User Authority for Editing the Client using client manager
            Dim oUserAuthority As New NexusProvider.UserAuthority
            'Get the user name from session
            oUserAuthority.UserCode = Session(CNLoginName)
            'set the authority options for reverse allocation
            oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.IsClientManagerViewonly
            oWebService = New NexusProvider.ProviderManager().Provider
            'initiate the GetUserAuthority method
            oWebService.GetUserAuthorityValue(oUserAuthority)
            If oUserAuthority.UserAuthorityValue = "1" Then
                bIsClientManagerViewOnly = True
            Else
                bIsClientManagerViewOnly = False
            End If


            bAddClient = UserCanDoTask("AddParty")
            'If bEditClientViaClientManager = True and User Can has authority to edit a client

            If bAddClient AndAlso bIsClientManagerViewOnly = False Then
                btnCorporateCustomer.Enabled = True
                btnPersonalCustomer.Enabled = True

            ElseIf bIsClientManagerViewOnly Then
                btnCorporateCustomer.Enabled = False
                btnPersonalCustomer.Enabled = False
            End If

            If Session(CNLoginType) Is Nothing Then
                Response.Redirect("~/login.aspx", False)
            End If
            'Clear the session values on page_load
            If Not IsPostBack AndAlso Request.QueryString("Type") Is Nothing Then
                ClearSessionValues()
            End If

            'To set the Focus
            'Page.SetFocus(txtClientName)

            rvDOB.MaximumValue = Now.Date
            If Request("ClaimFlag") = "1" OrElse Request("RequestPage") = "BG" Then
                btnCorporateCustomer.Visible = False
                btnPersonalCustomer.Visible = False
            ElseIf Request.QueryString("Type") IsNot Nothing Then
                If Request.QueryString("Type").Trim.ToUpper = "CLIENT" Then
                    btnCorporateCustomer.Visible = False
                    btnPersonalCustomer.Visible = False
                End If
            End If

            If Not IsPostBack AndAlso Request.QueryString("dosearch") IsNot Nothing Then ' if redirected from QuickSearch control
                If CType(Request.QueryString("dosearch"), Boolean) = True Then ' if dosearch is true then search the items
                    Dim oMaster As ContentPlaceHolder
                    Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                    Dim txtControl As TextBox

                    oMaster = GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName)

                    'Assign the values retrieved from query string to corresponding control
                    For iCt As Integer = 0 To Request.QueryString.AllKeys.Length - 3
                        txtControl = CType(oMaster.FindControl(Request.QueryString.GetKey(iCt)), TextBox)
                        If txtControl IsNot Nothing Then
                            txtControl.Text = Request.QueryString(iCt)
                        End If
                    Next
                    'for GC client in Quciksearch bIsAnySelect is true
                    bIsAnySelected = True
                    'Set session variable to search all type of parties
                    Session(CNSearchType) = PartyType.GC
                    'Search the parties for given criteria
                    FindParty(bIsAnySelected:=bIsAnySelected)

                    'If jump to result is true and there is only single result for given criteria then redirect to client detail page
                    If CType(Request.QueryString("jumptoresult"), Boolean) = True Then
                        Dim oPartyCollection As NexusProvider.PartyCollection
                        oPartyCollection = Session(CNSearchData)
                        If oPartyCollection IsNot Nothing Then
                            If oPartyCollection.Count = 1 Then

                                If TypeOf oPartyCollection(0) Is NexusProvider.PersonalParty Then
                                    Dim oItem As NexusProvider.PersonalParty = CType(oPartyCollection(0), NexusProvider.PersonalParty)
                                    Response.Redirect("~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & oItem.Key & "&Code=" & PureUrlEncode(oItem.UserName) & "")
                                ElseIf TypeOf oPartyCollection(0) Is NexusProvider.CorporateParty Then
                                    Dim oItem As NexusProvider.CorporateParty = CType(oPartyCollection(0), NexusProvider.CorporateParty)
                                    Response.Redirect("~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oItem.Key & "&Code=" & PureUrlEncode(oItem.UserName) & "")
                                End If

                            End If
                        End If
                    End If
                End If
            End If
            If CType(Session(CNIsAnonymous), Boolean) = True Then
                litHeaderMessage.Visible = True
                If Session(CNRedirectedFor) = "SaveQuote" Then
                    litHeaderMessage.Text = GetLocalResourceObject("lbl_FindClient_headerMessage_SaveQuote")
                ElseIf Session(CNRedirectedFor) = "BuyQuote" Then
                    litHeaderMessage.Text = GetLocalResourceObject("lbl_FindClient_headerMessage_BuyQuote")
                Else
                    litHeaderMessage.Visible = False
                End If
            End If

            If Session(CNPolicyAssociateCollection) IsNot Nothing Then
                oPolicyAssociateCollection = Session(CNPolicyAssociateCollection)
            End If
            If Not IsPostBack Then
                txtPartyIndex.Attributes.Add("onblur", "javascript:ClearPartyIndexCall();")
                txtPartyIndex.Attributes.Add("onfocus", "javascript:DisableSearch();")
                txtPolicyRiskIndex.Attributes.Add("onblur", "javascript:ClearRiskIndexCall();")
                txtPolicyRiskIndex.Attributes.Add("onfocus", "javascript:DisableSearch();")
            End If
        End Sub

        Protected Sub btnPersonalCustomer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPersonalCustomer.Click
            Response.Redirect("~/secure/agent/PersonalClientDetails.aspx?mode=add", False)
        End Sub

        Protected Sub btnCorporateCustomer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCorporateCustomer.Click
            Response.Redirect("~/secure/agent/CorporateClientDetails.aspx?mode=add", False)
        End Sub

        Protected Shadows Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            If Request("ClaimFlag") = "1" Or Request.QueryString("Page") = "RS" Or Request.QueryString("RequestPage") = "BG" Then
                CMS.Library.Frontend.Functions.SetTheme(Page, ConfigurationManager.AppSettings("ModalPageTemplate"))
            ElseIf Request.QueryString("Type") IsNot Nothing Then
                If Request.QueryString("Type").Trim.ToUpper = "CLIENT" Then
                    CMS.Library.Frontend.Functions.SetTheme(Page, ConfigurationManager.AppSettings("ModalPageTemplate"))
                End If
            End If
        End Sub
        Protected Sub ClearSessionValues()
            Session.Remove(CNPartyDataModelCode)
            Session.Remove(CNOI)
            Session.Remove(CNClientMode)
            Session.Remove(CNIsNewClient)
        End Sub


        Protected Sub btnNewSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewSearch.Click
            grdvSearchResults.Visible = False
            grdvSearchResults.DataSource = Nothing
            grdvSearchResults.DataBind()
            txtAddress.Text = ""
            txtClaimNumber.Text = ""
            txtClaimRiskIndex.Text = ""
            txtClientCode.Text = ""
            txtClientName.Text = ""
            txtDOB.Text = ""
            txtFileCode.Text = ""
            txtPhone.Text = ""
            txtPolicyNumber.Text = ""
            txtPolicyRiskIndex.Text = ""
            txtPostcode.Text = ""
            txtPartyIndex.Text = ""
            ddlClientType.SelectedIndex = 0
            ddlStatus.SelectedIndex = 0
            chkIncludeClosedBranches.Checked = False
        End Sub
    End Class

End Namespace