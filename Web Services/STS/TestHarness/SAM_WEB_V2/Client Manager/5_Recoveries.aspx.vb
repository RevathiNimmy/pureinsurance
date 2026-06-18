Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Imports SAMHelper

Partial Class OpenClaim_Recoveries
    Inherits System.Web.UI.Page

    Dim oGetClaimDetailsResponse As New GetClaimDetailsResponseType
    Dim Recoveries() As RecoveryDetails

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        oGetClaimDetailsResponse = DirectCast(Session("GetClaimDetailsResponse"), GetClaimDetailsResponseType)
        If Not oGetClaimDetailsResponse.ClaimDetails.ClaimPeril Is Nothing Then
            gvPerils.DataSource = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril
            gvPerils.DataBind()
        End If
    End Sub

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        oGetClaimDetailsResponse = DirectCast(Session("GetClaimDetailsResponse"), GetClaimDetailsResponseType)
        Response.Redirect("../UIIC_demo/HomePage.aspx?id=" + oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.ClaimNumber + "&name=ViewClaim")
    End Sub

    Protected Sub gvPerils_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPerils.SelectedIndexChanged
        oGetClaimDetailsResponse = DirectCast(Session("GetClaimDetailsResponse"), GetClaimDetailsResponseType)
        If Not oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Recovery Is Nothing Then
            ReDim Preserve Recoveries(oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Recovery.Length - 1)
            For RecoveryCount As Integer = 0 To Recoveries.Length - 1
                Recoveries(RecoveryCount) = New RecoveryDetails
                Recoveries(RecoveryCount).Description = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Recovery(RecoveryCount).TypeCode
                Recoveries(RecoveryCount).RevisionAmount = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Recovery(RecoveryCount).RevisedRecovery
                Recoveries(RecoveryCount).InitialReserve = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Recovery(RecoveryCount).InitialRecovery
            Next
        End If

        Session("Recoveries") = Recoveries

        gvRecoveries.DataSource = Recoveries
        gvRecoveries.DataBind()
    End Sub

    
End Class

