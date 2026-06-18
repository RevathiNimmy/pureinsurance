Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2


Partial Class Lookup_Screens_FindAgent
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("AgentSearchType") = "SubAgent" Then
            ddlType.Items.Clear()
            ddlType.Items.Insert(0, New ListItem("SubAgents", "1"))
        End If

    End Sub

    Protected Sub mnuFindParty_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles mnuFindParty.MenuItemClick
        mvFindParty.ActiveViewIndex = Int32.Parse(e.Item.Value)
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
            .PartyType = PartyTypeType.AG
            .PartyTypeSpecified = True

            If Session("AgentSearchType") = "SubAgent" Then
                .SupressSubAgents = False
                .SupressSubAgentsSpecified = True
                .AgentType = PartyAgentType.SubAgent
                .AgentTypeSpecified = True
            Else
                If ddlType.SelectedValue <> "All" Then
                    .AgentType = Convert.ToInt32(ddlType.SelectedValue)
                    .AgentTypeSpecified = True
                Else
                    .AgentTypeSpecified = False
                End If


                .SupressSubAgents = True
                .SupressSubAgentsSpecified = True
            End If


            'Not Mandatory
            .Shortname = txtClientCode.Text
            .Name = txtName.Text
            .IncludeClosedBranches = chkClosedBranches.Checked
        End With

        Try
            oFindPartyResponse = oSAM.FindParty(oFindPartyRequest)
            Session("FindPartyResponse") = oFindPartyResponse
            With oFindPartyResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Response.Write(GetMessageFromSamError(.Errors))
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
        Dim oFindPartyResponse As New FindPartyResponseType
        oFindPartyResponse = DirectCast(Session("FindPartyResponse"), FindPartyResponseType)
        Session("SelectedAgent") = oFindPartyResponse.Parties(gvSearchResult.SelectedIndex)


        If Session("AgentSearchType") = "Agent" Then
            txtShortName.Text = oFindPartyResponse.Parties(gvSearchResult.SelectedIndex).ShortName
            hfAgentKey.Value = oFindPartyResponse.Parties(gvSearchResult.SelectedIndex).PartyKey
            Session("AgentType") = oFindPartyResponse.Parties(gvSearchResult.SelectedIndex).AgentType
        End If
        If Session("AgentSearchType") = "SubAgent" Then
            Dim oSubAgents As BaseGetSubAgentsResponseTypeRow() = DirectCast(Session("SubAgents"), BaseGetSubAgentsResponseTypeRow())

            If oSubAgents IsNot Nothing Then
                ReDim Preserve oSubAgents(oSubAgents.Length)
            Else
                ReDim Preserve oSubAgents(0)
            End If

            oSubAgents(oSubAgents.Length - 1) = New BaseGetSubAgentsResponseTypeRow
            oSubAgents(oSubAgents.Length - 1).Amount = 0
            oSubAgents(oSubAgents.Length - 1).Percentage = 0
            oSubAgents(oSubAgents.Length - 1).Code = oFindPartyResponse.Parties(gvSearchResult.SelectedIndex).ShortName
            oSubAgents(oSubAgents.Length - 1).PartyKey = oFindPartyResponse.Parties(gvSearchResult.SelectedIndex).PartyKey
            oSubAgents(oSubAgents.Length - 1).Name = oFindPartyResponse.Parties(gvSearchResult.SelectedIndex).ResolvedName
            Session("SubAgents") = oSubAgents
        End If
    End Sub

    Protected Sub btnNewSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewSearch.Click
        gvSearchResult.DataSource = Nothing

        gvSearchResult.DataBind()
        txtClientCode.Text = ""
        txtFileCode.Text = ""
        txtName.Text = ""


    End Sub
End Class
