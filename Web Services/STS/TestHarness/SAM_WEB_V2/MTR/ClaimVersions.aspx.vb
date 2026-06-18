Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2

Partial Class View_Claim_ClaimVersions
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Response.Write(Session("ClaimKey"))
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oGetVersionsForClaimRequestType As New GetVersionsForClaimRequestType
        Dim oGetVersionsForClaimResponseType As New GetVersionsForClaimResponseType


        With oGetVersionsForClaimRequestType
            .Claim_Number = Session("ClaimNumber").ToString
            .BranchCode = "HeadOff"
        End With

        Try
            oGetVersionsForClaimResponseType = oSAM.GetVersionsForClaim(oGetVersionsForClaimRequestType)

            With oGetVersionsForClaimResponseType
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Response.Write(GetMessageFromSamError(.Errors))

                Else
                    gvResult.DataSource = .Versions
                    gvResult.DataBind()
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
End Class
