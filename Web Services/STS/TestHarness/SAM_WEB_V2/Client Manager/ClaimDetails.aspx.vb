Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2

Partial Class View_Claim_FindClaim
    Inherits System.Web.UI.Page

    

   

    Protected Sub gvResult_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvResult.RowEditing

    End Sub

    
    
    Protected Sub gvResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvResult.RowCommand
       



    End Sub


    Protected Sub gvResult_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvResult.SelectedIndexChanged
        Dim oFindClaimResponse As New FindClaimResponseType
        oFindClaimResponse.Claims = DirectCast(Session("FindClaimResponse"), FindClaimResponseType).Claims
        Session("SelectedMainClaim") = oFindClaimResponse.Claims(gvResult.SelectedIndex)
        Response.Redirect("2_1ClaimVersions.aspx")
    End Sub

  
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")


        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oFindClaimRequestType As New FindClaimRequestType
        Dim oFindClaimResponseType As New FindClaimResponseType


        With oFindClaimRequestType

            .BranchCode = "HeadOff"

            .ClientShortName = Session("clientcode")
           
        End With

        Try
            oFindClaimResponseType = oSAM.FindClaim(oFindClaimRequestType)

            With oFindClaimResponseType
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)

                Else
                    gvResult.DataSource = oFindClaimResponseType.Claims
                    gvResult.DataBind()
                    Session("FindClaimResponse") = oFindClaimResponseType
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
