Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2


Partial Class Lookup_Screens_FindAgent
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            getAgents()
        End If
    End Sub

    Protected Sub btnFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFind.Click
        getAgents()
    End Sub
    Private Sub getAgents()

        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oFindPartyRequest As New FindPartyRequestType
        Dim oFindPartyResponse As New FindPartyResponseType

        With oFindPartyRequest
            'Mandatory
            .BranchCode = "HeadOff"
            .PartyType = PartyTypeType.IN
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
                    Response.Write(GetMessageFromSamError(.Errors))
                Else
                    gvSearchResult.DataSource = oFindPartyResponse.Parties
                    gvSearchResult.DataBind()
                End If
            End With

        Catch os As SamResponseException
            Response.Write("An error occured calling SAM:<br>" & os.Message)
        Catch oe As Exception
            Response.Write("An error occured:<br>" & oe.Message)

        Finally
            'clean up any objects here
        End Try
    End Sub

    Protected Sub gvSearchResult_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSearchResult.SelectedIndexChanged
        Dim oFindPartyResponse As New FindPartyResponseType
        oFindPartyResponse = DirectCast(Session("FindPartyResponse"), FindPartyResponseType)
        txtShortName.Text = oFindPartyResponse.Parties(gvSearchResult.SelectedIndex).ResolvedName
        hfAgentKey.Value = oFindPartyResponse.Parties(gvSearchResult.SelectedIndex).PartyKey
        hfCode.Value = oFindPartyResponse.Parties(gvSearchResult.SelectedIndex).ShortName
    End Sub
End Class
