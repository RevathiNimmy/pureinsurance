Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2

Partial Class OpenClaim_Recoveries
    Inherits System.Web.UI.Page

    Dim oOpenClaimRequestType As New OpenClaimRequestType
    Dim oGetClaimRiskLinksResponse As New GetClaimRiskLinksResponseType
    Dim oPerilType() As Peril

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        oGetClaimRiskLinksResponse = DirectCast(Session("GetClaimRisksLinksResponse"), GetClaimRiskLinksResponseType)
        oPerilType = DirectCast(Session("oOpenClaimPerils"), Peril())
        gvPerils.DataSource = oPerilType
        gvPerils.DataBind()
        gvPerils.Columns(1).Visible = False
    End Sub

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        oOpenClaimRequestType = DirectCast(Session("OpenClaimRequestType"), OpenClaimRequestType)
        With oOpenClaimRequestType
            .Claim.DuplicateClaimOverrideUserName = txtUserName.Text
            .Claim.DuplicateClaimOverrideUserPassword = txtPassword.Text
        End With
        Session("OpenClaimRequestType") = oOpenClaimRequestType
        Response.Redirect("OpenClaim.aspx")
    End Sub

    Protected Sub gvRecoveries_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvRecoveries.RowEditing

        gvRecoveries.EditIndex = e.NewEditIndex
        gvRecoveries.DataSource = oPerilType(gvPerils.SelectedIndex).Recoveries
        gvRecoveries.DataBind()
    End Sub

    Protected Sub gvRecoveries_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvRecoveries.RowUpdating
        Dim txtEditBox As New TextBox
        txtEditBox = DirectCast(gvRecoveries.Rows(e.RowIndex).Controls(4).Controls(0), TextBox)
        oPerilType(gvPerils.SelectedIndex).Recoveries(gvRecoveries.EditIndex).ThisRevision = Convert.ToDecimal(txtEditBox.Text)
        oPerilType(gvPerils.SelectedIndex).Recoveries(gvRecoveries.EditIndex).TotalReserve = oPerilType(gvPerils.SelectedIndex).Recoveries(gvRecoveries.EditIndex).InitialReserve + Convert.ToDecimal(txtEditBox.Text)
        Session("oOpenClaimPerils") = oPerilType
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

        ddlRecoveries.Items.Clear()
        gvPerils.Columns(1).Visible = False

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

    Protected Sub chkOverrideCLaim_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkOverrideCLaim.CheckedChanged
        If chkOverrideCLaim.Checked Then
            lblUser.Visible = True
            lblPassword.Visible = True
            txtUserName.Visible = True
            txtPassword.Visible = True
        Else
            lblUser.Visible = False
            lblPassword.Visible = False
            txtUserName.Visible = False
            txtPassword.Visible = False
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

    Protected Sub rblSalvageTPRecovery_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblSalvageTPRecovery.SelectedIndexChanged
        gvRecoveries.EditIndex = -1
        ddlRecoveries.Items.Clear()
        If gvPerils.SelectedIndex <> -1 Then
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
            For PerilCount As Integer = 0 To oGetClaimRiskLinksResponse.PerilType.Length - 1

                If oPerilType(gvPerils.SelectedIndex).PerilTypeCode = oGetClaimRiskLinksResponse.PerilType(PerilCount).Code Then

                    For RecoveryCount As Integer = 0 To oGetClaimRiskLinksResponse.PerilType(PerilCount).RecoveryType.Length - 1
                        If oGetClaimRiskLinksResponse.PerilType(PerilCount).RecoveryType(RecoveryCount).Code = ddlRecoveries.SelectedValue Then
                            isSalvage = oGetClaimRiskLinksResponse.PerilType(PerilCount).RecoveryType(RecoveryCount).IsSalvage
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
                    oPerilType(gvPerils.SelectedIndex).Recoveries(oPerilType(gvPerils.SelectedIndex).Recoveries.Length - 1).InitialReserve = Convert.ToDecimal(txtInitialReserve.Text)
                    oPerilType(gvPerils.SelectedIndex).Recoveries(oPerilType(gvPerils.SelectedIndex).Recoveries.Length - 1).TotalReserve = Convert.ToDecimal(txtInitialReserve.Text)
                    oPerilType(gvPerils.SelectedIndex).Recoveries(oPerilType(gvPerils.SelectedIndex).Recoveries.Length - 1).PartyCode = txtParty.Text
                    oPerilType(gvPerils.SelectedIndex).Recoveries(oPerilType(gvPerils.SelectedIndex).Recoveries.Length - 1).PartyTypeCode = ddlRecoveryPartyType.SelectedValue

                End If

            Next

        End If
        Session("oOpenClaimPerils") = oPerilType
        gvRecoveries.DataSource = oPerilType(gvPerils.SelectedIndex).Recoveries
        gvRecoveries.DataBind()
        If RecoveryExists Then
            pnlAddRecovery.Visible = True
        Else
            pnlAddRecovery.Visible = False
        End If
        ' pnlAddRecovery.Visible = False
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        pnlAddRecovery.Visible = False
    End Sub

    Protected Sub btnAddRecovery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddRecovery.Click
        pnlAddRecovery.Visible = True

    End Sub
    Protected Sub ddlRecoveryPartyType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRecoveryPartyType.SelectedIndexChanged
        Dim UserToken As UsernameToken
        Dim oSAM As New SAMForInsuranceV2

        If ddlRecoveryPartyType.SelectedValue = "AG" Then
            btnShortName.Enabled = False

            UserToken = GetUserToken("sirius", "sirius")

            'set up the proxy object
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")


            Dim oRequest As New GetAgentDetailsForPolicyRequestType
            Dim oResponse As New GetAgentDetailsForPolicyResponseType

            oRequest.BranchCode = "HeadOff"
            oRequest.InsuranceFileKey = Session("InsuranceFileKey")

            oResponse = oSAM.GetAgentDetailsForPolicy(oRequest)

            With oResponse
                If .Errors IsNot Nothing Then
                    Throw New SamResponseException(.Errors)
                Else
                    If .Shortname <> "" Then
                        txtParty.Text = .Shortname
                    Else
                        Response.Write("<script>alert('There is no agent associated with the policy record. Pleasse make other selection');</script>")
                        ddlRecoveryPartyType.SelectedIndex = 0
                    End If
                End If
            End With


        ElseIf ddlRecoveryPartyType.SelectedValue = "OT" Then
            btnShortName.Enabled = True
            btnShortName.OnClientClick = "LoadWindows('FindOtherParty.aspx',400,400);return false"
        ElseIf ddlRecoveryPartyType.SelectedValue = "IN" Then
            btnShortName.Enabled = True
            btnShortName.OnClientClick = "LoadWindows('FindReinsurer.aspx',400,400);return false"
        ElseIf ddlRecoveryPartyType.SelectedValue = "CL" Then
            btnShortName.Enabled = False
            txtParty.Text = Session("ClientShortCode")
        End If
    End Sub
End Class
