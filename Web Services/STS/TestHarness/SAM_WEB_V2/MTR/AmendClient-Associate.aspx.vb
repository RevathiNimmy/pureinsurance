Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2


Partial Class Find_Party_Find_Party
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If ddlType.SelectedValue = "3" Then
            txtDateOfBirth.Text = ""
            txtDateOfBirth.Enabled = False
        Else
            txtDateOfBirth.Enabled = True

        End If
        mvFindParty.ActiveViewIndex = 0
        getParty()
    End Sub

    Protected Sub mnuFindParty_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles mnuFindParty.MenuItemClick
        mvFindParty.ActiveViewIndex = Int32.Parse(e.Item.Value)
    End Sub

    Protected Sub btnFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFind.Click
        getParty()
    End Sub

    Private Sub getParty()
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oFindPartyRequest As New FindPartyRequestType
        Dim oFindPartyResponse As New FindPartyResponseType

        With oFindPartyRequest
            .BranchCode = "HeadOff"

            .PartyType = Int32.Parse(ddlType.SelectedValue)

            If txtDateOfBirth.Text <> "" Then
                .DateOfBirth = Convert.ToDateTime(txtDateOfBirth.Text)
                .DateOfBirthSpecified = True
            Else
                .DateOfBirthSpecified = False
            End If
            .Status = Int32.Parse(ddlStatus.SelectedValue)
            .PartyTypeSpecified = True
            Dim Request As New GetListRequestType
            Request.ListType = STSListType.PMLookup

            'Not Mandatory
            .Shortname = txtClientCode.Text
            .Name = txtName.Text
            .IncludeClosedBranches = chkClosedBranches.Checked
            .AddressLine1 = txtAddrLine1.Text
            .AreaCode = txtAreaCode.Text
            .PostCode = txtPostCode.Text
            .TelephoneNumber = txtTelephoneNumber.Text
            .PolicyRef = txtPolicyNumber.Text
            .ClaimNumber = txtClaimNumber.Text
            .RiskIndex = txtRiskIndex.Text
        End With

        Try
            oFindPartyResponse = oSAM.FindParty(oFindPartyRequest)
            Session("FindPartyResponse") = oFindPartyResponse

            With oFindPartyResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                Else
                    Session("PARTIES") = oFindPartyResponse.Parties
                    gvSearchResult.DataSource = oFindPartyResponse.Parties
                    gvSearchResult.DataBind()
                End If
            End With

        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            Response.Write("An error occured calling SAM:<br>" & os.Message)

        Catch oe As Exception
            'should do some error handling here. Just output error for now
            Response.Write("An error occured:<br>" & oe.Message)

        Finally
            'clean up any objects here
        End Try
    End Sub

    Protected Sub gvSearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSearchResult.PageIndexChanging
        gvSearchResult.PageIndex = e.NewPageIndex
        gvSearchResult.DataSource = Session("PARTIES")
        gvSearchResult.DataBind()
    End Sub

    Protected Sub gvSearchResult_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSearchResult.SelectedIndexChanged
        Dim oFindPartyResponse As New FindPartyResponseType
        oFindPartyResponse = DirectCast(Session("FindPartyResponse"), FindPartyResponseType)
        txtShortName.Text = oFindPartyResponse.Parties(gvSearchResult.SelectedIndex).ResolvedName
        hfAgentKey.Value = oFindPartyResponse.Parties(gvSearchResult.SelectedIndex).PartyKey
    End Sub
End Class
