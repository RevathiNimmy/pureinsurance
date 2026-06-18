Imports System.IO
Imports System.Net
Imports CMS.Library
Imports Nexus.Constants
Imports Nexus.Library
Imports Nexus
Imports System.Web.Configuration.WebConfigurationManager

Partial Class Secure_FindOtherParty
    Inherits Nexus.BaseFindParty

    Private oParty As New NexusProvider.PartyCollection
    Dim oWebService As NexusProvider.ProviderBase

    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Page.SetFocus(txtPartyCode)

        If IsPostBack Then
            If Not Session(CNSearchData) IsNot Nothing Then
                Dim oPartyCollection As NexusProvider.PartyCollection = Session(CNSearchData)
                If oPartyCollection IsNot Nothing AndAlso oPartyCollection.Count > 0 Then
                    grdvSearchResults.DataSource = oPartyCollection 'Session(CNSearchData)
                    grdvSearchResults.DataBind()
                    oParty = Session(CNSearchData)
                End If
            End If
        Else
            txtType.Text = GetLocalResourceObject("lbl_OtherParty")
            txtType.Enabled = False
            FillPartyType()
        End If

        If Not UserCanDoTask("AddOtherParty") Then
            btnNewOtherParty.Visible = False
            btnNewOtherParty.Enabled = False
        End If
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

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        grdvSearchResults.Visible = True
        Dim sSearchType As String = ""
        Dim sValue As String
        If ddlPartyType.SelectedValue <> String.Empty AndAlso ddlPartyType.SelectedValue.Trim.ToUpper = "ALL" Then
            OtherPartySearch()
        Else
            Session(CNSearchOtherPartyTypeCode) = Trim(ddlPartyType.SelectedValue).ToUpper
        End If

        Session(CNSearchType) = PartyType.OTOTHERPARTY
        Session(CNSearchAgentType) = PartyAgentType.All
        sValue = Convert.ToString(Trim(ddlPartyType.SelectedValue).ToUpper)

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

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        txtAddress.Text = String.Empty
        txtClaimNumber.Text = String.Empty
        txtPartyCode.Text = String.Empty
        txtFileCode.Text = String.Empty
        txtPartyCode.Text = String.Empty
        txtPartyName.Text = String.Empty
        txtPhone.Text = String.Empty
        txtPostcode.Text = String.Empty
        txtRiskIndex.Text = String.Empty
        chkIncludeClosedBranches.Checked = False
        ddlPartyType.SelectedIndex = 0
        grdvSearchResults.DataSource = Nothing
        grdvSearchResults.Visible = False
    End Sub

    Protected Sub grdvSearchResults_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvSearchResults.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim oHyperLink As LinkButton = CType(e.Row.FindControl("lnkDetails"), LinkButton)
            Dim oOPItem As NexusProvider.BaseParty = Nothing
            Dim partyTypeCode As String = ""

            Dim oParty As NexusProvider.BaseParty = Session(CNParty)
            If Session(CNSearchOtherPartyTypeCode) Is Nothing OrElse Session(CNSearchOtherPartyTypeCode) = String.Empty Then
                partyTypeCode = GetCodeForKey(NexusProvider.ListType.PMLookup, e.Row.Cells(8).Text.Trim(), "party_type", True, False)
            Else
                partyTypeCode = Session(CNSearchOtherPartyTypeCode)
            End If

            oOPItem = CType(e.Row.DataItem, NexusProvider.BaseParty)
            Session(CNPartyType) = oOPItem.Type
            e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.BaseParty).UserName.Replace("""", "&quot;"))
            oHyperLink.PostBackUrl = PortalReWriteForOT(partyTypeCode, String.Format("PartyKey={0}&Code={1}&PartyType={2}&Mode=view", oOPItem.Key, oOPItem.Type, partyTypeCode))

            e.Row.Cells(8).Visible = False
            grdvSearchResults.HeaderRow.Cells(8).Visible = False
        End If

    End Sub
    Protected Sub btnNewOtherParty_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewOtherParty.Click
        Response.Redirect(PortalReWriteForOT(ddlPartyType.SelectedValue.Trim, "mode=add&PartyType=" + ddlPartyType.SelectedValue.Trim), False)

    End Sub

    Function PortalReWriteForOT(ByVal partyCode As String, Optional ByVal queryParams As String = "") As String

        Dim url As String = String.Empty
        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim oPortal As Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
        Dim searchDir As String
        searchDir = HttpContext.Current.Server.MapPath("~/portal/" & oPortal.Name)

        If File.Exists(String.Format("{0}/secure/agent/OtherPartyDetails_{1}.aspx", searchDir, partyCode.Trim)) Then
            url = String.Format("~/secure/agent/OtherPartyDetails_{0}.aspx{1}", partyCode, IIf(queryParams = String.Empty, "?Mode=add", "?" + queryParams).ToString())
        End If

        If url = String.Empty Then
            url = String.Format("~/secure/agent/OtherPartyDetails.aspx{0}", IIf(queryParams = "", "?Mode=add", "?" + queryParams).ToString())
        End If

        If HttpContext.Current.Session.IsCookieless Then
            url = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & url.TrimStart(New Char() {"~"})
        Else
            url = AppSettings("WebRoot") & "/" & url.TrimStart(New Char() {"~"})
        End If
        Return url

    End Function

End Class

