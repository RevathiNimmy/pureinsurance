Imports System.IO
Imports System.Net
Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports CMS.Library
Imports Nexus.Utils
Imports System.Web.HttpContext
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class Modal_FindOtherParty
        Inherits BaseFindParty

        Dim oParty As NexusProvider.BaseParty = Nothing
        Private oPartyColl As New NexusProvider.PartyCollection
        Dim oWebService As NexusProvider.ProviderBase
        Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
            grdvSearchResults.Visible = True
            Dim sSearchType As String = ""
            Dim sValue As String
            If Request.QueryString("Type").Trim.ToUpper = "ALL" Then
                Session(CNSearchOtherPartyTypeCode) = Trim(ddlPartyType.SelectedValue).ToUpper
            Else
                If txtPartyType.Text <> String.Empty Then
                    Session(CNSearchOtherPartyTypeCode) = txtPartyType.Text
                End If
                OtherPartySearch()
            End If

            Session(CNSearchType) = PartyType.OTOTHERPARTY
            Session(CNSearchAgentType) = PartyAgentType.All
            sValue = Convert.ToString(Session(CNSearchOtherPartyTypeCode))
            If sValue.ToUpper = "" Then
                sSearchType = "OT"
            Else
                sSearchType = sValue.ToUpper 'Client Specific Party_Types
            End If

            If sValue = PartyType.OTREASSUR.ToString() Then
                FindParty()
            Else
                FindParty(sSearchType)
            End If


        End Sub

        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

             ' Preserve claim sessions when creating a new other party from within a claim context
            If Session(CNClaim) IsNot Nothing Then
                 Session(CNDoNotClearSession) = "true"
            End If

            If ViewState("PrevOI") Is Nothing Then
                ViewState("PrevOI") = Session(CNOI)
            End If
            If ViewState("PrevMode") Is Nothing Then
                 ViewState("PrevMode") = Session(CNMode)
            End If
            If Request("__EVENTARGUMENT") = "SearchNewParty" Then
                If ViewState("PrevOI") IsNot Nothing Then
                    Session(CNOI) = ViewState("PrevOI")
                    ViewState("PrevOI") = Nothing
                End If
                If ViewState("PrevMode") IsNot Nothing Then
                    Session(CNMode) = ViewState("PrevMode")
                    ViewState("PrevMode") = Nothing
                End If
                If Session(CNClaim) IsNot Nothing Then
                    Session.Remove(CNDoNotClearSession)
                End If
                txtClientCode.Text = hdnNewPartyCode.Value
                btnSearch_Click(Nothing, Nothing)
            End If

            'To set the Focus
            Page.SetFocus(txtName)

            If IsPostBack Then
                If Not Request("__EVENTTARGET").ToLower.Contains("ddlpartytype") Then
                    If Session(CNSearchData) IsNot Nothing Then
                        grdvSearchResults.DataSource = Session(CNSearchData)
                        grdvSearchResults.DataBind()
                        oPartyColl = Session(CNSearchData)
                    Else

                    End If
                    Dim oPartyCollection As NexusProvider.PartyCollection = Session(CNSearchData)
                    If oPartyCollection IsNot Nothing Then
                        If oPartyCollection.Count > 0 Then

                        Else
                            grdvSearchResults.Visible = False
                        End If
                    Else
                        grdvSearchResults.Visible = False
                    End If
                End If
            Else
                hdnPostBackClientID.Value = Request.QueryString("ClientID")
                txtType.Text = GetLocalResourceObject("lbl_OtherParty")
                txtType.Enabled = False
                If Request.QueryString("Type") IsNot Nothing Then
                    If Request.QueryString("Type").Trim.ToUpper = "ALL" Then
                        txtPartyType.Visible = False
                        ddlPartyType.Visible = True
                        FillPartyType()
                    Else
                        txtPartyType.Text = Request.QueryString("Type").Trim.ToUpper
                        txtPartyType.Attributes.Add("readonly", "readonly")
                        txtPartyType.Visible = True
                        ddlPartyType.Visible = False
                    End If
                End If
                If Not String.IsNullOrEmpty(txtPartyType.Text) Then
                    vldPartyType.Enabled = False
                    btnNewOtherParty.Attributes.Add("OnClick", "javascript: tb_show(null , '" + PortalReWriteForOT() + "' , null); return false;")
                End If
            End If

            If Not UserCanDoTask("AddOtherParty") Then
                btnNewOtherParty.Visible = False
                btnNewOtherParty.Enabled = False
            End If
        End Sub

        Sub RetreiveData()
            'Need to Retreive the Data from Session
            If Session(CNParty) IsNot Nothing Then
                Select Case True
                    Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                        oParty = CType(Session(CNParty), NexusProvider.CorporateParty)
                    Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                        oParty = CType(Session(CNParty), NexusProvider.PersonalParty)
                    Case TypeOf Session(CNParty) Is NexusProvider.OtherParty
                        oParty = CType(Session(CNParty), NexusProvider.OtherParty)
                End Select
            End If
        End Sub

        Sub OtherPartySearch()
            oWebService = New NexusProvider.ProviderManager().Provider
            Dim olist As NexusProvider.LookupListCollection
            olist = oWebService.GetList(NexusProvider.ListType.PMLookup, "party_type", True, False)
            For iCount As Integer = 0 To olist.Count - 1
                If olist(iCount).Code.ToUpper = Request.QueryString("Type").Trim.ToUpper OrElse olist(iCount).Description.ToUpper = Request.QueryString("Type").Trim.ToUpper Then
                    Session(CNSearchOtherPartyTypeCode) = olist(iCount).Code
                End If
            Next
            oWebService = Nothing
        End Sub
        Sub FillPartyType()
            oWebService = New NexusProvider.ProviderManager().Provider
            Dim olist As NexusProvider.LookupListCollection
            Dim oOTlist As New NexusProvider.LookupListCollection
            olist = oWebService.GetList(NexusProvider.ListType.PMLookup, "party_type", True, False)
            For iCount As Integer = 0 To olist.Count - 1
                If olist(iCount).Code.ToUpper.StartsWith("OT") Then
                    If Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                        oOTlist.Add(olist(iCount))
                    Else
                        If olist(iCount).Code.ToUpper <> "OTHERPARTY" Then
                            oOTlist.Add(olist(iCount))
                        End If
                    End If
                End If
            Next
            ddlPartyType.DataSource = oOTlist
            ddlPartyType.DataTextField = "Description"
            ddlPartyType.DataValueField = "Code"
            ddlPartyType.DataBind()
            ddlPartyType.Items.Insert(0, New ListItem("(All)", ""))
        End Sub
        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
            txtAddress.Text = String.Empty
            txtClaimNumber.Text = String.Empty
            txtClientCode.Text = String.Empty
            txtFileCode.Text = String.Empty
            txtName.Text = String.Empty
            txtPhone.Text = String.Empty
            txtPostcode.Text = String.Empty
            txtRiskIndex.Text = String.Empty
            chkIncludeClosedBranches.Checked = False
            If Request.QueryString("Type").Trim.ToUpper = "ALL" Then
                ddlPartyType.SelectedIndex = 0
            End If
            grdvSearchResults.DataSource = Nothing
            grdvSearchResults.Visible = False
        End Sub

        Protected Sub grdvSearchResults_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvSearchResults.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("UserName") %>")
                e.Row.Attributes.Add("id", Server.HtmlEncode(CType(e.Row.DataItem, NexusProvider.BaseParty).UserName))
                Dim oItem As NexusProvider.BaseParty = Nothing
                oItem = CType(e.Row.DataItem, NexusProvider.BaseParty)

                If oItem IsNot Nothing Then
                    Dim sResolvedName As String = oItem.ResolvedName.ToString.Replace("'", "\")
                    Dim sKey As String = oItem.Key.ToString.Trim
                    Dim sType As String = oItem.Type.ToString.Trim
                    Dim sUserName As String = oItem.UserName.ToString.Replace("'", "\")

                    CType(e.Row.Cells(e.Row.Cells.Count - 1).FindControl("btnSelect"), LinkButton).Attributes.Add("OnClick", "self.parent.set" + Request.QueryString("ClientID") + "OtherParty('" + sResolvedName + "','" + sKey + "','" + sUserName + "','" + sType + "');")
                End If
            End If
        End Sub

        Private Sub ddlPartyType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPartyType.SelectedIndexChanged
            btnNewOtherParty.Attributes.Add("OnClick", "javascript: tb_show(null , '" + PortalReWriteForOT() + "' , null); return false;")

        End Sub

        Function PortalReWriteForOT(Optional ByVal queryParams As String = "") As String

            Dim partyTypeCode As String = ""
            Dim partyTypeKey As Integer
            Dim url As String = String.Empty
            Dim modalParameters As String = "&modal=true&KeepThis=true&TB_iframe=true&height=550&width=800"
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortal As Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
            Dim searchDir As String
            Dim partyDesc As String = ""

            If txtPartyType.Text.Trim() <> "" Then
                partyDesc = GetDescriptionForCode(NexusProvider.ListType.PMLookup, txtPartyType.Text.Trim(), "Party_Type")

                If String.IsNullOrEmpty(partyDesc) Then
                    partyDesc = txtPartyType.Text
                End If
                partyTypeKey = GetKeyForDescription(NexusProvider.ListType.PMLookup, partyDesc.Trim(), "Party_Type", False)
                If partyTypeKey = 0 Then
                    Return String.Empty
                End If
                partyTypeCode = GetCodeForKey(NexusProvider.ListType.PMLookup, partyTypeKey, "Party_Type", True).Trim()

            ElseIf ddlPartyType.SelectedValue.Trim <> "" AndAlso Not ddlPartyType.SelectedValue.Trim.ToUpper.Contains("ALL") Then
                partyTypeCode = ddlPartyType.SelectedValue.Trim.ToUpper
            Else
                Return String.Empty
            End If
            searchDir = HttpContext.Current.Server.MapPath("~/portal/" & oPortal.Name)

            If File.Exists(String.Format("{0}/Modal/OtherPartyDetails_{1}.aspx", searchDir, partyTypeCode.Trim)) Then
                url = String.Format("Modal/OtherPartyDetails_{0}.aspx?PartyType={0}{1}{2}", partyTypeCode, IIf(queryParams = String.Empty, "&Mode=add", "&" + queryParams).ToString(), modalParameters)
            End If

            If url = String.Empty Then
                url = String.Format("Modal/OtherPartyDetails.aspx?PartyType={0}{1}{2}", partyTypeCode, IIf(queryParams = "", "&Mode=add", "&" + queryParams).ToString(), modalParameters)
            End If

            If HttpContext.Current.Session.IsCookieless Then
                url = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))/" & url
            Else
                url = AppSettings("WebRoot") & url
            End If

            Return url


        End Function
        Protected Function FormatDataItem(ByVal ResolvedName As Object) As String
            Dim sResolvedName As String = ResolvedName.ToString()
            sResolvedName = sResolvedName.Replace(Chr(39), String.Empty)
            Return sResolvedName

        End Function


    End Class

End Namespace
