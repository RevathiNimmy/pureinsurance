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

    Protected Sub gvSearchResult_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSearchResult.DataBound

    End Sub

    Protected Sub gvSearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSearchResult.PageIndexChanging
        gvSearchResult.PageIndex = e.NewPageIndex
        gvSearchResult.DataSource = Session("PARTIES")
        gvSearchResult.DataBind()
    End Sub

    Protected Sub lnkSelect_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim str As String = "<script language=javascript>var a=confirm('Do You want to amend Client?');if(a==true) { window.location = 'http://localhost/SAM_Test/MTA/AmendClient.aspx?PartyKey=';  } else {window.location = 'http://localhost/SAM_Test/MTA/List Policy.aspx';  }</script>"
        Me.ClientScript.RegisterStartupScript(Me.GetType(), "Startup", str)
    End Sub

    Protected Sub gvSearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSearchResult.RowCommand


        Session("BRANCHCODE") = "HEADOFF"
        Session("PARTYKEY") = e.CommandArgument
        Session("CLIENTTYPE") = ddlType.SelectedItem.Text
        Dim index As Integer
        If (e.CommandName.Equals("ListPolicy")) Then
            Dim oFindPartyResponse As New FindPartyResponseType
            oFindPartyResponse = DirectCast(Session("FindPartyResponse"), FindPartyResponseType)
            Session("PartyName") = gvSearchResult.Rows(index).Cells(2).Text.ToString()
            Session("PARTYKEY") = gvSearchResult.Rows(index).Cells(13).Text.ToString()
            Response.Redirect("List Policy.aspx")
        End If


    End Sub

    
    Protected Sub btnNewSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewSearch.Click
        txtClientCode.Text = ""
        txtName.Text = ""
        chkClosedBranches.Checked = False
        gvSearchResult.DataSource = Nothing
        gvSearchResult.DataBind()
    End Sub
End Class
