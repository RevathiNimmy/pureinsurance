Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class OpenClaim_CoinsuranceBreakdown
    Inherits System.Web.UI.Page
    Dim oOpenClaimRequestType As New OpenClaimRequestType

    Protected Sub btnGetCoinsuranceRecoveries_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetCoinsuranceRecoveries.Click
        'create user token from credentials
        'normally the credentials would come from the log in
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")


        'create the request and response objects
        Dim oGetCoinsuranceRecoveriesRequestType As New GetRecoveryCoinsuranceRequestType
        Dim oGetCoinsuranceRecoveriesResponseType As New GetRecoveryCoinsuranceResponseType


        Dim oGetClaimDetailsRequest As New GetClaimDetailsRequestType
        Dim oGetClaimDetailsResponse As New GetClaimDetailsResponseType

        With oGetClaimDetailsRequest
            .BranchCode = "HeadOff"
            .ClaimKey = Session("ClaimKey")

        End With

        Try
            oGetClaimDetailsResponse = oSAM.GetClaimDetails(oGetClaimDetailsRequest)

        Catch ex As Exception

        End Try

        'set up request object with some values
        With oGetCoinsuranceRecoveriesRequestType

            .ClaimPerilKey = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).ClaimPerilKey
            .BranchCode = "HEADOFF"
            .IsSalvage = False

        End With

        Try
            oGetCoinsuranceRecoveriesResponseType = oSAM.GetRecoveryCoinsurance(oGetCoinsuranceRecoveriesRequestType)
            ' Dim sDataSet As String

            With oGetCoinsuranceRecoveriesResponseType
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Response.Write(GetMessageFromSamError(.Errors))
                End If
                'sDataSet = .Coinsurances.ToString
            End With

            'output dataset to the screen to show results
            CROutput.DataSource = oGetCoinsuranceRecoveriesResponseType.Coinsurances
            CROutput.DataBind()

            lblOutput.Visible = True
            lblOutput.Text = "Coinsurance Recoveries Details. "
            btnReinsuranceRecoveries.Enabled = True

        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            lblOutput.Visible = True
            lblOutput.Text = "An error occured calling SAM:<br>" & os.Message
        Catch oe As Exception
            'should do some error handling here. Just output error for now
            lblOutput.Visible = True
            lblOutput.Text = "An error occured:<br>" & oe.Message
        Finally
            'clean up any objects here
        End Try
    End Sub

    Protected Sub btnReinsuranceRecoveries_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReinsuranceRecoveries.Click

        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")


        'create the request and response objects
        Dim oGetReinsuranceRecoveriesRequestType As New GetRecoveryReinsuranceRequestType
        Dim oGetReinsuranceRecoveriesResponseType As New GetRecoveryReinsuranceResponseType


        Dim oGetClaimDetailsRequest As New GetClaimDetailsRequestType
        Dim oGetClaimDetailsResponse As New GetClaimDetailsResponseType

        With oGetClaimDetailsRequest
            .BranchCode = "HeadOff"
            .ClaimKey = Session("ClaimKey")

        End With

        Try
            oGetClaimDetailsResponse = oSAM.GetClaimDetails(oGetClaimDetailsRequest)

        Catch ex As Exception

        End Try

        'set up request object with some values
        With oGetReinsuranceRecoveriesRequestType

            .ClaimPerilKey = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).ClaimPerilKey
            .BranchCode = "HEADOFF"
            .IsSalvage = False

        End With

        Try
            oGetReinsuranceRecoveriesResponseType = oSAM.GetRecoveryReinsurance(oGetReinsuranceRecoveriesRequestType)
            ' Dim sDataSet As String

            With oGetReinsuranceRecoveriesResponseType
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Response.Write(GetMessageFromSamError(.Errors))
                End If
                'sDataSet = .Coinsurances.ToString
            End With

            'output dataset to the screen to show results
            CROutput.DataSource = oGetReinsuranceRecoveriesResponseType.Reinsurances
            CROutput.DataBind()

            lblOutput.Visible = True
            lblOutput.Text = "Reinsurance Recoveries Details. "
            btnReinsuranceRecoveries.Enabled = True

        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            lblOutput.Visible = True
            lblOutput.Text = "An error occured calling SAM:<br>" & os.Message
        Catch oe As Exception
            'should do some error handling here. Just output error for now
            lblOutput.Visible = True
            lblOutput.Text = "An error occured:<br>" & oe.Message
        Finally
            'clean up any objects here
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        oOpenClaimRequestType = DirectCast(Session("OpenClaimRequestType"), OpenClaimRequestType)
        gvPerils.DataSource = oOpenClaimRequestType.Claim.ClaimPeril
        gvPerils.DataBind()

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Response.Redirect("6_CoinsuranceBreakDown.aspx")
    End Sub
End Class
