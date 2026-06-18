Imports Microsoft.Web.Services3.Security.Tokens
Partial Class PolicyRenewal_wfrmAcceptRenewal
    Inherits System.Web.UI.Page

      Dim StartDate As Date
    Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Session("ReturnPage") = "~/PolicyRenewal/wfrmAcceptRenewal.aspx"
        If GetRenewalStatus(Convert.ToInt32(Session("InsuranceFileKey"))) = "Update" Then
            MakeLive()
        Else
            lblOutput.Text = "Renewal status of selected policy is not set for acceptance"
        End If
    End Sub
    Private Function GetRenewalStatus(ByVal iInsuranceFileKey As Integer) As String
        Dim strStatus As String = String.Empty
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        'create the request and response objects
        Dim oGetRenewalStatusRequest As New GetRenewalStatusRequestType
        Dim oGetRenewalStatusResponse As New GetRenewalStatusResponseType
        oGetRenewalStatusRequest.BranchCode = "HeadOff"
        oGetRenewalStatusRequest.InsuranceFileKey = iInsuranceFileKey
        Try
            StartDate = Date.Now
            oGetRenewalStatusResponse = oSAM.GetRenewalStatus(oGetRenewalStatusRequest)
            WriteToLog(Session, "wfrmAcceptRenewal.aspx", "SAMForInsuranceV2", "GetRenewalStatus", StartDate, Date.Now)

            If Not (oGetRenewalStatusResponse.Errors) Is Nothing Then
                'errors returned, so throw an exception
                Throw New SamResponseException(oGetRenewalStatusResponse.Errors)
            End If
            strStatus = oGetRenewalStatusResponse.RenewalStatusTypeCode.Trim
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
        Return strStatus
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblOutput.Text = ""
        If (Session("StatusMessage") IsNot Nothing) Then
            lblOutput.Text = Session("StatusMessage")
            Session("SatatusMessage") = Nothing
        End If
        If Not IsPostBack Then
            Session("Process") = "REN"
        End If
        txtInsuranceRef.Attributes.Add("ReadOnly", "True")
    End Sub

    Private Sub MakeLive()
        Session("Amount") = CalculateAmount()
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oGetHeaderAndSummariesByKeyRequest As New GetHeaderAndSummariesByKeyRequestType
        Dim oGetHeaderAndSummariesByKeResponse As New GetHeaderAndSummariesByKeyResponseType

        oGetHeaderAndSummariesByKeyRequest.BranchCode = "HeadOff"
        oGetHeaderAndSummariesByKeyRequest.InsuranceFileKey = Convert.ToInt32(Session("InsuranceFileKey").ToString)
        StartDate = Date.Now
        oGetHeaderAndSummariesByKeResponse = oSAM.GetHeaderAndSummariesByKey(oGetHeaderAndSummariesByKeyRequest)
        WriteToLog(Session, "wfrmAcceptRenewal.aspx", "SAMForInsuranceV2", "GetHeaderAndSummariesByKey(", StartDate, Date.Now)

        If Not (oGetHeaderAndSummariesByKeResponse.Errors) Is Nothing Then
            'errors returned, so throw an exception
            Throw New SamResponseException(oGetHeaderAndSummariesByKeResponse.Errors)
        End If
        Session("QuoteTimeStamp") = oGetHeaderAndSummariesByKeResponse.QuoteTimeStamp
        Session("InsuranceFileKey") = oGetHeaderAndSummariesByKeResponse.InsuranceFileKey
        Session("PartyKey") = oGetHeaderAndSummariesByKeResponse.PartyKey
        Session("PartyName") = oGetHeaderAndSummariesByKeResponse.InsuredName
        '#Prakash: Check whether prepayment is on
        Dim oGetOptionsettingsRequest As New GetOptionSettingRequestType
        Dim oGetOptionsettingsResponse As New GetOptionSettingResponseType

        With oGetOptionsettingsRequest
            .BranchCode = "HeadOff"
            .OptionNumber = 87
            .OptionType = OptionType.ProductOption
        End With
        StartDate = Date.Now
        oGetOptionsettingsResponse = oSAM.GetOptionSetting(oGetOptionsettingsRequest)
        WriteToLog(Session, "wfrmAcceptRenewal.aspx", "SAMForInsuranceV2", "GetOptionSetting", StartDate, Date.Now)
        With oGetOptionsettingsResponse
            If Not (.Errors) Is Nothing Then
                'errors returned, so throw an exception
                Throw New SamResponseException(GetMessageFromSamError(.Errors))
            Else
                If .OptionValue = "1" Then
                    If rblPaymentMethod.SelectedValue.Trim = "PayNow" Then
                        Response.Redirect("~/MTA/CashList.aspx")
                    Else
                        Response.Redirect("~/MTA/PrePayment.aspx")
                    End If
                Else
                    If rblPaymentMethod.SelectedValue.Trim = "PayNow" Then
                        Response.Redirect("~/MTA/CashList.aspx")
                    End If

                    Dim oBindQuoteRequest As New BindQuoteRequestType
                    Dim oBindQuoteResponse As New BindQuoteResponseType
                    With oBindQuoteRequest

                        .BranchCode = "HeadOff"
                        .CreditTransactions = Nothing
                        .DebitAgainstSpecified = False
                        .InsuranceFileKey = Session("InsuranceFileKey")
                        .PaymentMethodSpecified = False
                        .TransactionType = "REN"
                        .AcceptRenewal = True
                        .AcceptRenewalSpecified = True
                        .QuoteTimeStamp = oGetHeaderAndSummariesByKeResponse.QuoteTimeStamp
                    End With
                    StartDate = Date.Now
                    oBindQuoteResponse = oSAM.BindQuote(oBindQuoteRequest)
                     WriteToLog(Session, "wfrmAcceptRenewal.aspx", "SAMForInsuranceV2", "BindQuote", StartDate, Date.Now)
                    
                    With oBindQuoteResponse
                    
                        If Not (.Errors) Is Nothing Then
                            'errors returned, so throw an exception
                            Dim strMessage As String = GetMessageFromSamError(.Errors)
                            If (strMessage.Contains("DocumentId")) Then
                                lblOutput.Text = "Process completed successfully"
                            End If
                            Throw New SamResponseException(.Errors)


                        End If
                        lblOutput.Text = "Process completed successfully"
                    End With
                End If
            End If

        End With
    End Sub
    Private Function CalculateAmount() As Decimal
        Dim dAmount As Double = 0
        Dim dGrossTotal As Double
        Dim dLeadCommission As Double
        Dim dLeadTax As Double

        'Get Gross Total
        Dim iInsuranceFileKey As Integer = Session("InsuranceFileKey")
        Dim oGetHeaderAndRisksByKeyRequestType As New GetHeaderAndRisksByKeyRequestType
        Dim oGetHeaderAndRisksByKeyResponseType As New GetHeaderAndRisksByKeyResponseType

        oGetHeaderAndRisksByKeyRequestType.BranchCode = "HeadOff"
        oGetHeaderAndRisksByKeyRequestType.InsuranceFileKey = iInsuranceFileKey

        Dim oSAM As New SAMForInsuranceV2

        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        'logging method call
        Dim StartDate As Date
        StartDate = Date.Now
        oGetHeaderAndRisksByKeyResponseType = oSAM.GetHeaderAndRisksByKey(oGetHeaderAndRisksByKeyRequestType)
        WriteToLog(Session, "wfrmPolicyRenewals_accept.aspx", "SAMForInsuranceV2", "GetHeaderAndRisksByKey", StartDate, Date.Now)

        If Not (oGetHeaderAndRisksByKeyResponseType.Errors) Is Nothing Then
            Throw New SamResponseException(oGetHeaderAndRisksByKeyResponseType.Errors)
        End If
        With oGetHeaderAndRisksByKeyResponseType
            dGrossTotal = .GrossTotal
        End With

        'Get Lead Agent commission, Lead agent tax if any
        Dim oGetHeaderAndAgentCommissionByKeyRequest As New GetHeaderAndAgentCommissionByKeyRequestType
        Dim oGetHeaderAndAgentCommissionByKeyResponse As New GetHeaderAndAgentCommissionByKeyResponseType

        oGetHeaderAndAgentCommissionByKeyRequest.BranchCode = "HeadOff" ''Session("BRANCHCODE").ToString()
        oGetHeaderAndAgentCommissionByKeyRequest.InsuranceFileKey = iInsuranceFileKey

        'logging method call
        StartDate = Date.Now
        oGetHeaderAndAgentCommissionByKeyResponse = oSAM.GetHeaderAndAgentCommissionByKey(oGetHeaderAndAgentCommissionByKeyRequest)
        WriteToLog(Session, "GetListRisks_Risk.aspx", "SAMForInsuranceV2", "GetHeaderAndAgentCommissionByKey", StartDate, Date.Now)
        If Not (oGetHeaderAndAgentCommissionByKeyResponse.Errors) Is Nothing Then
            Throw New SamResponseException(oGetHeaderAndAgentCommissionByKeyResponse.Errors)
        End If
        If (oGetHeaderAndAgentCommissionByKeyResponse.AgentCommission IsNot Nothing AndAlso oGetHeaderAndAgentCommissionByKeyResponse.AgentCommission.Length > 0) Then
            For Each oCommissionRow As BaseGetHeaderAndAgentCommissionByKeyResponseTypeRow In oGetHeaderAndAgentCommissionByKeyResponse.AgentCommission
                dLeadCommission = dLeadCommission + oCommissionRow.CommissionValue
                dLeadTax = dLeadTax + oCommissionRow.TaxValue
            Next
        End If

        'Calculate the amount tobe payed
        dAmount = dGrossTotal - dLeadCommission - dLeadTax
        Return dAmount

    End Function

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("~/UIIC_DEMO/HomePage.aspx")
    End Sub
End Class
