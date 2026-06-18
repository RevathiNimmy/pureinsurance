Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class MTC_Risk_Premium_Details
    Inherits System.Web.UI.Page

 

    Protected Sub btnRiskPremiumDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRiskPremiumDetails.Click
        'create user token from credentials
        'normally the credentials would come from the log in
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")


        'create the request and response objects
        Dim oGetRiskPremiumRequestType As New GetRatingDetailsRequestType
        Dim oGetRiskPremiumResponseType As New GetRatingDetailsResponseType



        'set up request object with some values
        With oGetRiskPremiumRequestType

            .InsuranceFileKey = txtInsuranceFileKey.Text
            .RiskKey = txtRiskKey.Text
            .BranchCode = "HEADOFF"
        End With

        Try
            oGetRiskPremiumResponseType = oSAM.GetRatingDetails(oGetRiskPremiumRequestType)
            ' Dim sDataSet As String

            With oGetRiskPremiumResponseType
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Response.Write(GetMessageFromSamError(.Errors))
                End If
                'sDataSet = .Coinsurances.ToString
            End With

            'output dataset to the screen to show results
            GRDOutput.DataSource = oGetRiskPremiumResponseType.RatingDetails
            GRDOutput.DataBind()

            lblOutput.Visible = True
            lblOutput.Text = "RatingD Details. "
            btnnextscreen.Enabled = True

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

    Protected Sub btnnextscreen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnnextscreen.Click

    End Sub

    
End Class
