Imports SAMForInsuranceV2
Imports Microsoft.Web.Services3.Security.Tokens
Partial Class GetReserveReinsuranceRecoveries
    Inherits System.Web.UI.Page
    Dim StartDate As Date
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblError.Text = ""
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click

        Dim oGetRecoveriesRequestType As New GetRecoveryReinsuranceRequestType
        Dim oGetRecoveriesResponseType As New GetRecoveryReinsuranceResponseType
        Try

            oGetRecoveriesRequestType.BranchCode = txtBranchCode.Text
            oGetRecoveriesRequestType.ClaimPerilKey = txtClaimPerilKey.Text
            oGetRecoveriesRequestType.IsSalvage = chkIsSalvage.Checked

            Dim oSAM As New SAMForInsuranceV2

            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")
     
            StartDate = Date.Now
            oGetRecoveriesResponseType = oSAM.GetRecoveryReinsurance(oGetRecoveriesRequestType)
            WriteToLog(Session, "7_GetReserveReinsuranceRecoveries.aspx", "SAMForInsuranceV2", "GetRecoveryReinsurance", StartDate, Date.Now)


            If Not (oGetRecoveriesResponseType.Errors) Is Nothing Then
                Throw New SamResponseException(oGetRecoveriesResponseType.Errors)
            End If

            grdReserveRIRecoveries.DataSource = oGetRecoveriesResponseType.Reinsurances
            grdReserveRIRecoveries.DataBind()
        Catch ex As Exception
            lblError.Text = "Error occured: " + ex.Message
        End Try
    End Sub
End Class
