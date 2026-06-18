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

    End Sub

    Protected Sub mnuFindParty_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles mnuFindParty.MenuItemClick
        mvFindParty.ActiveViewIndex = Int32.Parse(e.Item.Value)
    End Sub

    Protected Sub TextBox2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPostCode.TextChanged

    End Sub

    Protected Sub btnFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFind.Click

        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oFindPartyRequest As New FindPartyRequestType
        Dim oFindPartyResponse As New FindPartyResponseType

        With oFindPartyRequest
            'Mandatory
            .BranchCode = "HeadOff"

            If Session("openerWindow") = "CP" Then
                .PartyType = PartyTypeType.OTTHIRD
                .PartyTypeSpecified = True
            Else
                If ddlType.SelectedValue <> "All" Then
                    .PartyType = Int32.Parse(ddlType.SelectedValue)
                    .PartyTypeSpecified = True
                Else
                    .PartyTypeSpecified = False
                End If

            End If

                If txtDateOfBirth.Text <> "" Then
                    .DateOfBirth = Convert.ToDateTime(txtDateOfBirth.Text)
                    .DateOfBirthSpecified = True
                Else
                    .DateOfBirthSpecified = False
                End If

                .Status = Int32.Parse(ddlStatus.SelectedValue)



                Dim Request As New GetListRequestType
                Request.ListType = STSListType.PMLookup

                'Not Mandatory
                .Shortname = txtClientCode.Text
                .FileCode = txtFileCode.Text
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
            Dim StartDate As Date
            StartDate = Date.Now
            oFindPartyResponse = oSAM.FindParty(oFindPartyRequest)
            WriteToLog(Session, "FindParty.aspx", "SAMForInsuranceV2", "FindParty", StartDate, Date.Now)
            Session("PARTIES") = oFindPartyResponse.Parties
            With oFindPartyResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                Else
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

    Protected Sub gvSearchResult_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSearchResult.SelectedIndexChanged
        'Dim oFindPartyResponse As New FindPartyResponseType
        'oFindPartyResponse = DirectCast(Session("FindPartyResponse"), FindPartyResponseType)
        'Session("SelectedParty") = oFindPartyResponse.Parties(gvSearchResult.SelectedIndex)
        'Session("PartyKey") = oFindPartyResponse.Parties(gvSearchResult.SelectedIndex).PartyKey
        'Session("AccountKey") = oFindPartyResponse.Parties(gvSearchResult.SelectedIndex).PartyKey
        ' Session("PartyName") = oFindPartyResponse.Parties(gvSearchResult.SelectedIndex).ShortName
        ' txtShortName.Text = oFindPartyResponse.Parties(gvSearchResult.SelectedIndex).ShortName


        Session("BRANCHCODE") = "HEADOFF"
        Session("CLIENTTYPE") = ddlType.SelectedItem.Text
        '' Code added by Jigar Pandya
        Session("FindPartyResponse") = DirectCast(Session("PARTIES"), BaseFindPartyResponseTypeRow())(gvSearchResult.SelectedIndex)
        Session("PARTYKEY") = DirectCast(Session("PARTIES"), BaseFindPartyResponseTypeRow())(gvSearchResult.SelectedIndex).PartyKey
        Session("clientcode") = DirectCast(Session("PARTIES"), BaseFindPartyResponseTypeRow())(gvSearchResult.SelectedIndex).ShortName
        Response.Redirect("ClientManager.aspx")

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Write("<script>self.close();</script>")
    End Sub
    Protected Sub gvSearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSearchResult.PageIndexChanging
        gvSearchResult.PageIndex = e.NewPageIndex
        gvSearchResult.DataSource = Session("PARTIES")
        gvSearchResult.DataBind()
    End Sub

    Protected Sub txtNewClient_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNewClient.Click
        Response.Redirect("NewClient.aspx")
    End Sub
End Class
