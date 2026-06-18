
Partial Class OpenClaim_Peril
    Inherits System.Web.UI.Page
    Dim oOpenClaimRequestType As New OpenClaimRequestType
    Dim oGetClaimRiskLinksResponse As New GetClaimRiskLinksResponseType
    Dim oPerilType() As Peril


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblReserves.Visible = False
        oGetClaimRiskLinksResponse = DirectCast(Session("GetClaimRisksLinksResponse"), GetClaimRiskLinksResponseType)
        oPerilType = DirectCast(Session("oOpenClaimPerils"), Peril())
        If Not Page.IsPostBack Then
            ddlperils.DataSource = oGetClaimRiskLinksResponse.PerilType
            ddlperils.DataTextField = "Description"
            ddlperils.DataValueField = "Code"
            ddlperils.DataBind()
        End If

        gvPerils.DataSource = oPerilType
        gvPerils.DataBind()

    End Sub

    Protected Sub gvPerils_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPerils.SelectedIndexChanged
        lblReserves.Visible = True
        gvReserves.DataSource = oPerilType(gvPerils.SelectedIndex).Reserves
        gvReserves.DataBind()
    End Sub

    Protected Sub gvReserves_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvReserves.RowEditing
        gvReserves.EditIndex = e.NewEditIndex
        gvReserves.DataSource = oPerilType(gvPerils.SelectedIndex).Reserves
        gvReserves.DataBind()
    End Sub

    Protected Sub gvReserves_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvReserves.RowUpdating

        Dim txtEditBox As New TextBox
        txtEditBox = DirectCast(gvReserves.Rows(e.RowIndex).Controls(2).Controls(0), TextBox)
        'oPerilType(gvPerils.SelectedIndex).Reserves(gvReserves.EditIndex).ThisRevision = Convert.ToDecimal(txtEditBox.Text)
        oPerilType(gvPerils.SelectedIndex).Reserves(gvReserves.EditIndex).CurrentReserve = Convert.ToDecimal(txtEditBox.Text)
        oPerilType(gvPerils.SelectedIndex).Reserves(gvReserves.EditIndex).Incurred = Convert.ToDecimal(txtEditBox.Text)
        oPerilType(gvPerils.SelectedIndex).Reserves(gvReserves.EditIndex).InitialReserve = Convert.ToDecimal(txtEditBox.Text)
        Session("oOpenClaimPerils") = oPerilType
        gvReserves.EditIndex = -1
        gvReserves.DataSource = oPerilType(gvPerils.SelectedIndex).Reserves
        gvReserves.DataBind()

    End Sub

    Protected Sub gvReserves_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvReserves.RowCancelingEdit

        gvReserves.EditIndex = -1
        gvReserves.DataSource = oPerilType(gvPerils.SelectedIndex).Reserves
        gvReserves.DataBind()

    End Sub

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        'Response.Redirect("RiskReinsuranceArrangements.aspx")
        Response.Redirect("5_Recoveries.aspx")
    End Sub

    Protected Sub btnAddPeril_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddPeril.Click
        'Praveen
        If gvPerils IsNot Nothing Then
            btnAddPeril.Attributes.Add("Onclick", "alert('All perils specific to the Claim/Policy are already defined')")
        Else
            'Praveen
            pnlAddPeril.Visible = True
        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        pnlAddPeril.Visible = False
    End Sub

    Protected Sub btnPerilOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPerilOk.Click
        If oGetClaimRiskLinksResponse.PerilType IsNot Nothing Then
            For PerilCount As Integer = 0 To oGetClaimRiskLinksResponse.PerilType.Length - 1
                If oGetClaimRiskLinksResponse.PerilType(PerilCount).Code = ddlperils.SelectedValue Then
                    If oPerilType IsNot Nothing Then
                        ReDim Preserve oPerilType(oPerilType.Length)
                    Else
                        ReDim Preserve oPerilType(0)
                    End If

                    Dim PerilLength As Integer = oPerilType.Length - 1

                    oPerilType(PerilLength) = New Peril
                    oPerilType(PerilLength).Description = txtDescription.Text
                    oPerilType(PerilLength).PerilTypeCode = oGetClaimRiskLinksResponse.PerilType(PerilCount).Code
                    oPerilType(PerilLength).SumInsured = oGetClaimRiskLinksResponse.PerilType(PerilCount).SumInsured

                    If oGetClaimRiskLinksResponse.PerilType(PerilCount).ReserveType IsNot Nothing Then

                        ReDim Preserve oPerilType(PerilLength).Reserves(oGetClaimRiskLinksResponse.PerilType(PerilCount).ReserveType.Length - 1)
                        For ReserveCount As Integer = 0 To oGetClaimRiskLinksResponse.PerilType(PerilCount).ReserveType.Length - 1
                            oPerilType(PerilLength).Reserves(ReserveCount) = New ReserveDetails
                            oPerilType(PerilLength).Reserves(ReserveCount).RevisionAmount = 0
                            oPerilType(PerilLength).Reserves(ReserveCount).TypeCode = oGetClaimRiskLinksResponse.PerilType(PerilCount).ReserveType(ReserveCount).Code
                            oPerilType(PerilLength).Reserves(ReserveCount).Description = oGetClaimRiskLinksResponse.PerilType(PerilCount).ReserveType(ReserveCount).Description
                        Next
                    End If



                End If
            Next
        End If

        Session("oOpenClaimPerils") = oPerilType
        gvPerils.DataSource = oPerilType
        gvPerils.DataBind()
        pnlAddPeril.Visible = False
    End Sub
End Class
