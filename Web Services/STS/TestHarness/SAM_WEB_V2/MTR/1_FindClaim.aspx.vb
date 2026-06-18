Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2

Partial Class View_Claim_FindClaim
    Inherits System.Web.UI.Page

    Protected Sub btnFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFind.Click
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oFindClaimRequestType As New FindClaimRequestType
        Dim oFindClaimResponseType As New FindClaimResponseType


        With oFindClaimRequestType
            .BranchCode = "HeadOff"
            .ClaimNumber = txtClaim.Text
            .ClientShortName = txtShortName.Text
            .IncludeClosedClaim = chkIncludeCloseClaim.Checked
            .RiskIndex = txtRiskIndex.Text
            If txtInForceFrom.Text <> "" Then
                .LossDateFrom = txtInForceFrom.Text
                .LossDateFromSpecified = True
            End If
            If txtInForceTo.Text <> "" Then
                .LossDateTo = txtInForceTo.Text
                .LossDateToSpecified = True
            End If
        End With

        Try
            oFindClaimResponseType = oSAM.FindClaim(oFindClaimRequestType)

            With oFindClaimResponseType
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Response.Write(GetMessageFromSamError(.Errors))

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

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Response.Redirect("2_Claim Details.aspx")
    End Sub

    Protected Sub gvResult_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvResult.RowEditing

    End Sub

    
    
    Protected Sub gvResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvResult.RowCommand
       



    End Sub

    Protected Sub gvResult_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvResult.SelectedIndexChanged
        Dim oFindClaimResponse As New FindClaimResponseType
        oFindClaimResponse.Claims = DirectCast(Session("FindClaimResponse"), FindClaimResponseType).Claims
        Session("SelectedClaim") = oFindClaimResponse.Claims(gvResult.SelectedIndex)
    End Sub
End Class
