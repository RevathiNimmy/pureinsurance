Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class View_Claim_1_2ClaimVersions
    Inherits System.Web.UI.Page
    Dim StartDate As Date
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        Dim oGetVersionsforClaimRequest As New GetVersionsForClaimRequestType
        Dim oGetVersionsforClaimResponse As New GetVersionsForClaimResponseType

        With oGetVersionsforClaimRequest
            .BranchCode = "HeadOff"
            .Claim_Number = DirectCast(Session("SelectedMainClaim"), BaseFindClaimResponseTypeRow).ClaimNumber
        End With

        Try
             StartDate = Date.Now
            oGetVersionsforClaimResponse = oSAM.GetVersionsForClaim(oGetVersionsforClaimRequest)
             WriteToLog(Session, "2_1ClaimVersions.aspx", "SAMForInsuranceV2", "GetVersionsForClaim", StartDate, Date.Now)
            Session("GetVersionsforClaim") = oGetVersionsforClaimResponse

            With oGetVersionsforClaimResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                Else
                    gvClaimVersions.DataSource = .Versions
                    gvClaimVersions.DataBind()
                End If

            End With



        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            Response.Write("An error occured calling SAM:<br>" & os.Message)
        Catch oe As Exception
            'should do some error handling here. Just output error for now
            Response.Write("An error occured:<br>" & oe.Message)
        Finally
            'clean
        End Try
    End Sub

    Protected Sub gvClaimVersions_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvClaimVersions.SelectedIndexChanged
        Dim oGetVersionForClaimResponse As New GetVersionsForClaimResponseType
        oGetVersionForClaimResponse = DirectCast(Session("GetVersionsForClaim"), GetVersionsForClaimResponseType)
        Session("SelectedClaim") = oGetVersionForClaimResponse.Versions(gvClaimVersions.SelectedIndex)
        Response.Redirect("2_Claim Details.aspx")
    End Sub
End Class
