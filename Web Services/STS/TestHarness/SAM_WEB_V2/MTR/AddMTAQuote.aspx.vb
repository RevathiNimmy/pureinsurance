Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class MTC_AddMTAQuote
    Inherits System.Web.UI.Page

    Protected Sub btnAddMTAQuote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddMTAQuote.Click

        'create user token from credentials
        'normally the credentials would come from the log in
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")


        'create the request and response objects
        Dim oAddMTAQuoteRequestType As New AddMtaQuoteRequestType
        Dim oAddMTAQuoteResponseType As New AddMtaQuoteResponseType



        'set up request object with some values
        With oAddMTAQuoteRequestType
            .BranchCode = "HEADOFF"
            .EffectiveDate = Date.Now
            .ExpiryDate = Date.Now
            .InsuranceFileKey = 1539
            .MtaReason = "OTHER"
            .TypeOfMta = "PERMANENT"

            .InsuredName = txtInsuredName.Text
            .PolicyKey = txtPolicyKey.Text
            .Regarding = txtRegarding.Text
            .AlternateReference = txtAlternateReference.Text
            .PolicyStatusCode = txtPolicyStatusCode.Text
            .AnalysisCode = txtAnalysisCode.Text
            .BusinessTypeCode = txtBusinessTypeCode.Text
            .IssueDate = txtIssueDate.Text
            .ProposalDate = txtProposalDate.Text
            .FrequencyCode = txtFrequencyCode.Text
            .LTUExpiryDate = txtLTUExpiryDate.Text
            .StopReasonCode = txtStopReasonCode.Text
            .RenewalMethodCode = txtRenewalMethodCode.Text
            .LapseCancelReasonCode = txtLapseCancelReasonCode.Text
            .LapseCancelDate = txtLapseCancelDate.Text
            .ReferredAtRenewal = txtReferredAtRenewal.Text
            .ReferredOnMTA = txtReferredOnMTA.Text
            .PolicyStyleCode = txtPolicyStyleCode.Text
            .IsReinstatement = True

            '.InsuredName = "Mr p sriram"
            '.PolicyKey = "POLION/MOB/00787"
            '.Regarding = "unpaidpremium"
            '.AlternateReference = "premium"
            '.PolicyStatusCode = "CUR"
            '.AnalysisCode = "501"
            '.BusinessTypeCode = "DIRECT"
            '.IssueDateSpecified = True
            '.IssueDate = "1899-12-29"
            '.ProposalDateSpecified = True
            '.ProposalDate = "1899-12-29"
            '.FrequencyCode = "ANNUAL"
            '.LTUExpiryDateSpecified = True
            '.LTUExpiryDate = "1899-12-29"
            '.StopReasonCode = "9951"
            '.RenewalMethodCode = "AUTO"
            '.LapseCancelReasonCode = "AGY XFER"
            '.LapseCancelDateSpecified = True
            '.LapseCancelDate = "1900-12-29"
            '.ReferredAtRenewalSpecified = True
            '.ReferredOnMTASpecified = True
            '.ReferredAtRenewal = True
            '.ReferredOnMTA = True
            '.PolicyStyleCode = "SIMPLE"
        

        
        End With

        Try
            oAddMTAQuoteResponseType = oSAM.AddMtaQuote(oAddMTAQuoteRequestType)
            ' Dim sDataSet As String

            With oAddMTAQuoteResponseType
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Response.Write(GetMessageFromSamError(.Errors))
                End If
                'sDataSet = .Coinsurances.ToString
            End With

            'output dataset to the screen to show results
            txtInsuranceFileKey.Text = oAddMTAQuoteResponseType.InsuranceFileKey
            'aMTAOutput.DataSource
            'aMTAOutput.DataBind()

            lblOutput.Visible = True
            lblOutput.Text = "MTR Quote Added"
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

 
End Class
