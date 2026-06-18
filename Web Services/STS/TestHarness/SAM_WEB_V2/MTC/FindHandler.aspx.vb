Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2


Partial Class Lookup_Screens_FindAgent
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        
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
            .PartyType = PartyTypeType.AH
            .PartyTypeSpecified = True

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
        Session("SelectedHandler") = oFindPartyResponse.Parties(gvSearchResult.SelectedIndex)
        txtShortName.Text = oFindPartyResponse.Parties(gvSearchResult.SelectedIndex).ResolvedName
        hfHandlerCode.Value = oFindPartyResponse.Parties(gvSearchResult.SelectedIndex).ShortName
    End Sub

    Protected Sub btnNewSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewSearch.Click
        gvSearchResult.DataSource = Nothing

        gvSearchResult.DataBind()
    End Sub
End Class
