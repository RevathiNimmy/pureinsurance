Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class MTC_GetHeaderandSummaries
    Inherits System.Web.UI.Page

    Protected Sub btnGetPolicyHeaderDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetPolicyHeaderDetails.Click
        'create user token from credentials
        'normally the credentials would come from the log in
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        'create the request and response objects
        Dim oGetPolicyHeaderRequestType As New GetHeaderAndSummariesByKeyRequestType
        Dim oGetPolicyHeaderResponseType As New GetHeaderAndSummariesByKeyResponseType

        'set up request object with some values
        With oGetPolicyHeaderRequestType

            .InsuranceFileKey = txtInsuranceFilelKey.Text
            .BranchCode = "HEADOFF"

        End With

        Try
            oGetPolicyHeaderResponseType = oSAM.GetHeaderAndSummariesByKey(oGetPolicyHeaderRequestType)
            ' Dim sDataSet As String
            With oGetPolicyHeaderResponseType
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                End If

            End With
            
            'output dataset to the screen to show results
            Label10.Text = oGetPolicyHeaderResponseType.CoverEndDate
            Label11.Text = oGetPolicyHeaderResponseType.CoverStartDate
            Label12.Text = oGetPolicyHeaderResponseType.Description
            Label13.Text = oGetPolicyHeaderResponseType.InceptionDate
            Label14.Text = oGetPolicyHeaderResponseType.InsuranceFileRef
            Label15.Text = oGetPolicyHeaderResponseType.InsuranceFileStatusCode
            Label16.Text = oGetPolicyHeaderResponseType.InsuranceFileTypeCode
            Label18.Text = oGetPolicyHeaderResponseType.InsuranceFileVersion
            Label20.Text = oGetPolicyHeaderResponseType.InsuranceFolderKey
            Label22.Text = oGetPolicyHeaderResponseType.PartyKey
            Label24.Text = oGetPolicyHeaderResponseType.PaymentMethodCode
            Label26.Text = oGetPolicyHeaderResponseType.ProductCode
            Label28.Text = oGetPolicyHeaderResponseType.QuoteExpiryDate
            Label30.Text = oGetPolicyHeaderResponseType.QuoteIsLocked
            Label34.Text = oGetPolicyHeaderResponseType.SubBranchCode
            Label36.Text = oGetPolicyHeaderResponseType.ConsolidatedLeadAgentCommission
            Label38.Text = oGetPolicyHeaderResponseType.ConsolidatedSubAgentCommission
            'Label40.Text = ToServiceTaxesAndFeesType(oGetPolicyHeaderResponseType.PolicyLevelTaxesAndFees)
            PHDOutput.DataSource = oGetPolicyHeaderResponseType.InsuredParties
            PHDOutput.DataBind()

            PHDOUTPUT1.DataSource = oGetPolicyHeaderResponseType.Risks
            PHDOUTPUT1.DataBind()

            lblOutput.Visible = True
            lblOutput.Text = "Policy header Details. "
            btnNextScreen.Enabled = True

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


    Protected Sub btnNextScreen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNextScreen.Click

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
End Class
