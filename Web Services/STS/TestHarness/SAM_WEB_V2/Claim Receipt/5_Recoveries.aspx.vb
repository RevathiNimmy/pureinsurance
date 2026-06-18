Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2

Partial Class OpenClaim_Recoveries
    Inherits System.Web.UI.Page

    Dim oPerilType() As Peril
    Dim oGetClaimRiskLinksResponse As New GetClaimRiskLinksResponseType

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        oGetClaimRiskLinksResponse = DirectCast(Session("GetClaimRisksLinksResponse"), GetClaimRiskLinksResponseType)
        oPerilType = DirectCast(Session("oMaintainClaimPerils"), Peril())
        gvPerils.DataSource = oPerilType
        gvPerils.DataBind()

    End Sub

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Response.Redirect("MaintainClaim.aspx")
    End Sub

    Protected Sub gvRecoveries_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvRecoveries.RowUpdating
        Dim txtEditBox As New TextBox
        txtEditBox = DirectCast(gvRecoveries.Rows(e.RowIndex).Controls(4).Controls(0), TextBox)
        oPerilType(gvPerils.SelectedIndex).Recoveries(gvRecoveries.EditIndex).ThisRevision = Convert.ToDecimal(txtEditBox.Text)
        oPerilType(gvPerils.SelectedIndex).Recoveries(gvRecoveries.EditIndex).TotalReserve = oPerilType(gvPerils.SelectedIndex).Recoveries(gvRecoveries.EditIndex).InitialReserve + Convert.ToDecimal(txtEditBox.Text)
        Session("oMaintainClaimPerils") = oPerilType
        gvRecoveries.EditIndex = -1
        gvRecoveries.DataSource = oPerilType(gvPerils.SelectedIndex).Recoveries
        gvRecoveries.DataBind()

    End Sub

    Protected Sub gvRecoveries_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvRecoveries.RowCancelingEdit
        gvRecoveries.EditIndex = -1
        gvRecoveries.DataSource = oPerilType(gvPerils.SelectedIndex).Recoveries
        gvRecoveries.DataBind()
    End Sub

    Protected Sub gvPerils_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPerils.SelectedIndexChanged
        gvRecoveries.DataSource = oPerilType(gvPerils.SelectedIndex).Recoveries
        gvRecoveries.DataBind()
       
        For PerilCount As Integer = 0 To oGetClaimRiskLinksResponse.PerilType.Length - 1
            If oGetClaimRiskLinksResponse.PerilType(PerilCount).Code = oPerilType(gvPerils.SelectedIndex).PerilTypeCode Then
                For RecoveryCount As Integer = 0 To oGetClaimRiskLinksResponse.PerilType(PerilCount).RecoveryType.Length - 1
                    If oGetClaimRiskLinksResponse.PerilType(PerilCount).RecoveryType(RecoveryCount).IsSalvage.ToString = rblSalvageTPRecovery.SelectedValue Then
                        ddlRecoveries.Items.Insert(0, New ListItem(oGetClaimRiskLinksResponse.PerilType(PerilCount).RecoveryType(RecoveryCount).Description, oGetClaimRiskLinksResponse.PerilType(PerilCount).RecoveryType(RecoveryCount).Code))
                    End If
                Next
            End If
        Next
    End Sub

    Protected Sub gvRecoveries_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvRecoveries.RowEditing
        gvRecoveries.EditIndex = e.NewEditIndex
        gvRecoveries.DataSource = oPerilType(gvPerils.SelectedIndex).Recoveries
        gvRecoveries.DataBind()

    End Sub

    Protected Sub rblSalvageTPRecovery_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblSalvageTPRecovery.SelectedIndexChanged
        gvRecoveries.EditIndex = -1
        ddlRecoveries.Items.Clear()

        If (gvPerils.SelectedIndex >= 0) Then
            For PerilCount As Integer = 0 To oGetClaimRiskLinksResponse.PerilType.Length - 1
                If oGetClaimRiskLinksResponse.PerilType(PerilCount).Code = oPerilType(gvPerils.SelectedIndex).PerilTypeCode Then
                    For RecoveryCount As Integer = 0 To oGetClaimRiskLinksResponse.PerilType(PerilCount).RecoveryType.Length - 1
                        If oGetClaimRiskLinksResponse.PerilType(PerilCount).RecoveryType(RecoveryCount).IsSalvage.ToString = rblSalvageTPRecovery.SelectedValue Then
                            ddlRecoveries.Items.Insert(0, New ListItem(oGetClaimRiskLinksResponse.PerilType(PerilCount).RecoveryType(RecoveryCount).Description, oGetClaimRiskLinksResponse.PerilType(PerilCount).RecoveryType(RecoveryCount).Code))
                        End If
                    Next
                End If
            Next

            gvRecoveries.DataSource = oPerilType(gvPerils.SelectedIndex).Recoveries
            gvRecoveries.DataBind()
        End If

    End Sub

    Protected Sub gvRecoveries_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvRecoveries.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            If DirectCast(e.Row.DataItem, RecoveryDetails).IsSalvage.ToString = rblSalvageTPRecovery.SelectedValue Then
                e.Row.Visible = True
            Else
                e.Row.Visible = False
            End If
        End If
    End Sub

    Protected Sub btnAddRecoverylOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddRecoverylOk.Click
        Dim RecoveryExists As New Boolean
        Dim isSalvage As New Integer
        isSalvage = 0
        RecoveryExists = False
        If oPerilType(gvPerils.SelectedIndex).Recoveries IsNot Nothing Then
            For RecoveryCount As Integer = 0 To oPerilType(gvPerils.SelectedIndex).Recoveries.Length - 1
                If oPerilType(gvPerils.SelectedIndex).Recoveries(RecoveryCount).TypeCode.Trim = ddlRecoveries.SelectedValue.Trim Then
                    RecoveryExists = True
                End If
            Next
        End If

        If RecoveryExists Then
            lblErrorMessage.Text = "Recovery Exists"
        Else
            For RecoveryCount As Integer = 0 To oGetClaimRiskLinksResponse.PerilType(gvPerils.SelectedIndex).RecoveryType.Length - 1
                If oGetClaimRiskLinksResponse.PerilType(gvPerils.SelectedIndex).RecoveryType(RecoveryCount).Code = ddlRecoveries.SelectedValue Then
                    isSalvage = oGetClaimRiskLinksResponse.PerilType(gvPerils.SelectedIndex).RecoveryType(RecoveryCount).IsSalvage
                End If
            Next
            If oPerilType(gvPerils.SelectedIndex).Recoveries IsNot Nothing Then
                ReDim Preserve oPerilType(gvPerils.SelectedIndex).Recoveries(oPerilType(gvPerils.SelectedIndex).Recoveries.Length)
            Else
                ReDim Preserve oPerilType(gvPerils.SelectedIndex).Recoveries(0)
            End If
            oPerilType(gvPerils.SelectedIndex).Recoveries(oPerilType(gvPerils.SelectedIndex).Recoveries.Length - 1) = New RecoveryDetails
            oPerilType(gvPerils.SelectedIndex).Recoveries(oPerilType(gvPerils.SelectedIndex).Recoveries.Length - 1).Description = ddlRecoveries.SelectedItem.Text
            oPerilType(gvPerils.SelectedIndex).Recoveries(oPerilType(gvPerils.SelectedIndex).Recoveries.Length - 1).TypeCode = ddlRecoveries.SelectedValue
            oPerilType(gvPerils.SelectedIndex).Recoveries(oPerilType(gvPerils.SelectedIndex).Recoveries.Length - 1).IsSalvage = isSalvage
            oPerilType(gvPerils.SelectedIndex).Recoveries(oPerilType(gvPerils.SelectedIndex).Recoveries.Length - 1).ThisRevision = Convert.ToDecimal(txtInitialReserve.Text)
            '#
            oPerilType(gvPerils.SelectedIndex).Recoveries(oPerilType(gvPerils.SelectedIndex).Recoveries.Length - 1).TotalReserve = Convert.ToDecimal(txtInitialReserve.Text)

        End If
        Session("oMaintainClaimPerils") = oPerilType
        gvRecoveries.DataSource = oPerilType(gvPerils.SelectedIndex).Recoveries
        gvRecoveries.DataBind()
        If RecoveryExists Then
            pnlAddRecovery.Visible = True
        Else
            pnlAddRecovery.Visible = False
        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        pnlAddRecovery.Visible = False
    End Sub

    Protected Sub btnAddRecovery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddRecovery.Click
        pnlAddRecovery.Visible = True
    End Sub
End Class

