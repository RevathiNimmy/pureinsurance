
Partial Class OpenClaim_Peril
    Inherits System.Web.UI.Page
    Dim oGetClaimDetailsResponse As New GetClaimDetailsResponseType
    Dim oMaintainClaimRequestType As New MaintainClaimRequestType




    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        oGetClaimDetailsResponse = DirectCast(Session("GetClaimDetailsResponse"), GetClaimDetailsResponseType)
        gvPerils.DataSource = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril
        gvPerils.DataBind()


    End Sub

    Protected Sub gvPerils_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPerils.SelectedIndexChanged
        oGetClaimDetailsResponse = DirectCast(Session("GetClaimDetailsResponse"), GetClaimDetailsResponseType)
        gvReserves.DataSource = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Reserve
        gvReserves.DataBind()
    End Sub

    Protected Sub gvReserves_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvReserves.RowEditing
        oMaintainClaimRequestType = DirectCast(Session("MaintainClaimRequestType"), MaintainClaimRequestType)
        gvReserves.EditIndex = e.NewEditIndex
        gvReserves.DataSource = oMaintainClaimRequestType.Claim.ClaimPeril(gvPerils.SelectedIndex).Reserve
        gvReserves.DataBind()
    End Sub

    Protected Sub gvReserves_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvReserves.RowUpdating

        oMaintainClaimRequestType = DirectCast(Session("MaintainClaimRequestType"), MaintainClaimRequestType)
        Dim txtEditBox As New TextBox
        txtEditBox = DirectCast(gvReserves.Rows(e.RowIndex).Controls(1).Controls(0), TextBox)
        oMaintainClaimRequestType.Claim.ClaimPeril(gvPerils.SelectedIndex).Reserve(gvReserves.EditIndex).RevisionAmount = Convert.ToDecimal(txtEditBox.Text)
        Session("MaintainClaimRequestType") = oMaintainClaimRequestType

        oGetClaimDetailsResponse = DirectCast(Session("GetClaimDetailsResponse"), GetClaimDetailsResponseType)
        oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Reserve(gvReserves.EditIndex).RevisedReserve = Convert.ToDecimal(txtEditBox.Text)
        Session("GetClaimDetailsResponse") = oGetClaimDetailsResponse
        gvReserves.EditIndex = -1
        gvReserves.DataSource = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Reserve
        gvReserves.DataBind()

    End Sub

    Protected Sub gvReserves_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvReserves.RowCancelingEdit
        oMaintainClaimRequestType = DirectCast(Session("MaintainClaimRequestType"), MaintainClaimRequestType)
        gvReserves.EditIndex = -1
        gvReserves.DataSource = oMaintainClaimRequestType.Claim.ClaimPeril(gvPerils.SelectedIndex).Reserve
        gvReserves.DataBind()

    End Sub

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Response.Redirect("5_Recoveries.aspx")
    End Sub

    Protected Sub gvReserves_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvReserves.DataBinding

    End Sub

    Protected Sub gvReserves_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvReserves.RowDataBound
        'Dim lblRevisionAmount As New Label
        'lblRevisionAmount = DirectCast(e.Row.Cells(1).FindControl("lblRevisedAmount"), Label)
        'lblRevisionAmount.Text = "New Revision Amount"

    End Sub
End Class
