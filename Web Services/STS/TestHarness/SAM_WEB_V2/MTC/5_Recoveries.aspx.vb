Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2

Partial Class OpenClaim_Recoveries
    Inherits System.Web.UI.Page

    Dim oGetClaimDetailsResponse As New GetClaimDetailsResponseType
    Dim oMaintainClaimRequestType As New MaintainClaimRequestType
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        oMaintainClaimRequestType = DirectCast(Session("MaintainClaimRequestType"), MaintainClaimRequestType)
        gvPerils.DataSource = oMaintainClaimRequestType.Claim.ClaimPeril
        gvPerils.DataBind()


    End Sub

    Protected Sub btnSalvage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalvage.Click
        If gvPerils.SelectedIndex <> -1 Then
            gvRecoveries.DataSource = oMaintainClaimRequestType.Claim.ClaimPeril(gvPerils.SelectedIndex).Recovery
            gvRecoveries.DataBind()
        End If
    End Sub



    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oMaintainClaimResponseType As New MaintainClaimResponseType

        oMaintainClaimRequestType.TimeStamp = Session("TimeStamp")


        
        Try
            oMaintainClaimResponseType = oSAM.MaintainClaim(oMaintainClaimRequestType)


            With oMaintainClaimResponseType
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Response.Write(GetMessageFromSamError(.Errors))
                Else
                    lblClaimNumber.Text = .ClaimNumber.ToString()
                    lblClaimKey.Text = .ClaimKey.ToString()
                    Session("ClaimKey") = .ClaimKey
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

    Protected Sub gvRecoveries_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvRecoveries.RowEditing

        oMaintainClaimRequestType = DirectCast(Session("MaintainClaimRequestType"), MaintainClaimRequestType)
        gvRecoveries.EditIndex = e.NewEditIndex
        gvRecoveries.DataSource = oMaintainClaimRequestType.Claim.ClaimPeril(gvPerils.SelectedIndex).Recovery
        gvRecoveries.DataBind()
    End Sub

    Protected Sub gvRecoveries_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvRecoveries.RowUpdating
        oMaintainClaimRequestType = DirectCast(Session("MaintainClaimRequestType"), MaintainClaimRequestType)
        Dim txtEditBox As New TextBox
        txtEditBox = DirectCast(gvRecoveries.Rows(e.RowIndex).Controls(1).Controls(0), TextBox)
        oMaintainClaimRequestType.Claim.ClaimPeril(gvPerils.SelectedIndex).Recovery(gvRecoveries.EditIndex).RevisionAmount = Convert.ToDecimal(txtEditBox.Text)
        Session("MaintainClaimRequestType") = oMaintainClaimRequestType
        gvRecoveries.EditIndex = -1
        gvRecoveries.DataSource = oMaintainClaimRequestType.Claim.ClaimPeril(gvPerils.SelectedIndex).Recovery
        gvRecoveries.DataBind()

    End Sub

    Protected Sub gvRecoveries_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvRecoveries.RowCancelingEdit
        oMaintainClaimRequestType = DirectCast(Session("MaintainClaimRequestType"), MaintainClaimRequestType)
        gvRecoveries.EditIndex = -1
        gvRecoveries.DataSource = oMaintainClaimRequestType.Claim.ClaimPeril(gvPerils.SelectedIndex).Recovery
        gvRecoveries.DataBind()
    End Sub

    Protected Sub btnCoInsuranceBreakDown_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCoInsuranceBreakDown.Click
        Response.Redirect("6_CoinsuranceRecoveries.aspx")
    End Sub

    Protected Sub gvPerils_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPerils.SelectedIndexChanged
        oGetClaimDetailsResponse = DirectCast(Session("GetClaimDetailsResponse"), GetClaimDetailsResponseType)
        gvRecoveries.DataSource = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Recovery
        gvRecoveries.DataBind()
    End Sub
End Class
