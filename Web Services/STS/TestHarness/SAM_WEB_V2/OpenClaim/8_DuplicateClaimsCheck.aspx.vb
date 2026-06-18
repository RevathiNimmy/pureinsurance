Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class OpenClaim_3_DuplicateClaimsCheck
    Inherits System.Web.UI.Page
    Protected Sub btnOverRideClaims_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOverRideClaims.Click

        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oOpenClaimRequestType As New OpenClaimRequestType
        Dim oOpenClaimResponseType As New OpenClaimResponseType
        oOpenClaimRequestType = DirectCast(Session("oOpenClaimRequestType"), OpenClaimRequestType)

     

        Try
            With oOpenClaimRequestType
                .Claim.DuplicateClaimOverrideUserName = txtUserName.Text
                .Claim.DuplicateClaimOverrideUserPassword = txtPassword.Text
            End With

            Session("oOpenClaimRequestType") = oOpenClaimRequestType
            Response.Redirect("4_CheckUnPaidPremium.aspx")
            'oOpenClaimResponseType = oSAM.OpenClaim(oOpenClaimRequestType)

            'With oOpenClaimResponseType
            '    If Not (.Errors) Is Nothing Then
            '        'errors returned, so throw an exception
            '        Throw New SamResponseException(.Errors)

            '    Else
            '        lblOutput.Text = oOpenClaimResponseType.BaseClaimKey
            '        'gvResult.DataSource = oFindInsurenceFileForClaimResponse
            '        'gvResult.DataBind()
            '    End If
            'End With
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
End Class
