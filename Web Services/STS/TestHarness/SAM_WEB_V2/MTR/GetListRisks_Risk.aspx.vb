Imports SAMForInsuranceV2
Imports Microsoft.Web.Services3.Security.Tokens
Partial Class GetListRisksRisk
    Inherits System.Web.UI.Page

    Dim dLeadCommission As New Double
    Dim dSubAgentCommission As New Double
    Dim dLeadTax As New Double
    Dim dSubAgentTax As New Double
    Dim dLeadTotalNetPremium As New Double
    Dim dSubAgnetTotalNetPremium As New Double
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            GetHeaderAndRisksByKey()
            GetHeaderAndRisksTaxByKey()
            GetHeaderAndAgentCommissionByKey()
        End If

    End Sub

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick

        Dim oGetHeaderAndRisksByKeyRequestType As New GetHeaderAndRisksByKeyRequestType
        Dim oGetHeaderAndRisksByKeyResponseType As New GetHeaderAndRisksByKeyResponseType
        Dim oSAM As New SAMForInsuranceV2
        Try

            oGetHeaderAndRisksByKeyRequestType.BranchCode = "HeadOff" 'Session("BRANCHCODE").ToString()
            oGetHeaderAndRisksByKeyRequestType.InsuranceFileKey = Session("InsuranceFileKey")

            Dim iSelectedIndex As Integer
            iSelectedIndex = Menu1.Items.IndexOf(Menu1.SelectedItem)
            MultiView1.ActiveViewIndex = iSelectedIndex



        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Private Sub UserValidation()
        Dim oSAM As New SAMForInsuranceV2
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
    End Sub

    Protected Sub btnAddRisk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddRisk.Click
        Response.Redirect("MotorAddRisk.aspx")
    End Sub

    Protected Sub btnEditRisk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditRisk.Click
        'Response.Redirect("UpdateRisk.aspx")
    End Sub

    Protected Sub btnSaveQuote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveQuote.Click
       
    End Sub

    Protected Sub grdLiskRisk_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdLiskRisk.RowCommand
        If (e.CommandName.Equals("Delete")) Then
            Session("RiskAction") = "Delete"
        Else
            Session("RiskAction") = ""
        End If
        Session("RiskKey") = e.CommandArgument
        Response.Redirect("MotorEditRisk.aspx")


    End Sub

   

    Protected Sub btnMakeLive_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMakeLive.Click
        'PraveenGora
        Session("Amount") = Convert.ToDecimal(lblLeadAgentTotalNetPremium.Text)

       

        'PraveenGora
        Dim oSAM As New SAMForInsuranceV2
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        '#Prakash: If process is renewal, do nothing else bind quote
        If (Session("Process") = "REN") Then
            If (Session("Action") = "Accept") Then
                Try
                    Dim iInsuranceFileKey = Convert.ToInt32(Session("InsuranceFileKey"))
                    SetStatus(iInsuranceFileKey)
                    Dim oGetOptionsettingsRequest As New GetOptionSettingRequestType
                    Dim oGetOptionsettingsResponse As New GetOptionSettingResponseType

                    With oGetOptionsettingsRequest
                        .BranchCode = "HeadOff"
                        .OptionNumber = 87
                        .OptionType = OptionType.ProductOption
                    End With


                    oGetOptionsettingsResponse = oSAM.GetOptionSetting(oGetOptionsettingsRequest)

                    With oGetOptionsettingsResponse
                        If Not (.Errors) Is Nothing Then
                            'errors returned, so throw an exception
                            lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                        Else
                            If .OptionValue = "1" Then
                                If rblPaymentTerms.SelectedValue = "PayNow" Then
                                    Response.Redirect("CashList.aspx")
                                Else
                                    Response.Redirect("PrePayment.aspx")
                                End If
                            Else
                                If rblPaymentTerms.SelectedValue = "PayNow" Then
                                    Response.Redirect("CashList.aspx")
                                End If

                                Dim oBindQuoteRequest As New BindQuoteRequestType
                                Dim oBindQuoteResponse As New BindQuoteResponseType
                                With oBindQuoteRequest
                                    .BranchCode = "HeadOff"
                                    .CreditTransactions = Nothing
                                    .DebitAgainstSpecified = False
                                    .InsuranceFileKey = iInsuranceFileKey
                                    .PaymentMethodSpecified = False
                                    .TransactionType = "REN"
                                    .AcceptRenewal = True
                                    .AcceptRenewalSpecified = True

                                End With

                                Try
                                    oBindQuoteResponse = oSAM.BindQuote(oBindQuoteRequest)
                                    With oBindQuoteResponse
                                        If Not (.Errors) Is Nothing Then
                                            'errors returned, so throw an exception
                                            lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                                        Else
                                            lblPolicyNum.Text = .Policy.PolicyRef
                                            lblPolicyNum.Visible = True
                                            Session("StatusMessage") = "Process completed successfully"
                                            Response.Redirect(Session("ReturnPage").ToString)
                                        End If
                                    End With

                                Catch os As SamResponseException
                                    'should do some error handling here. Just output error for now
                                    Response.Write("An error occured calling SAM:<br>" & os.Message)
                                Catch oe As Exception
                                    'should do some error handling here. Just output error for now
                                    Response.Write("An error occured:<br>" & oe.Message)
                                Finally
                                    'clean
                                End Try


                            End If
                        End If

                    End With

                Catch os As SamResponseException
                    'should do some error handling here. Just output error for now
                    Response.Write("An error occured calling SAM:<br>" & os.Message)
                Catch oe As Exception
                    'should do some error handling here. Just output error for now
                    Response.Write("An error occured:<br>" & oe.Message)
                Finally
                    'clean
                End Try
            Else
                Session("StatusMessage") = "Process completed successfully"
                Response.Redirect(Session("ReturnPage").ToString)
            End If

        Else

            Dim oGetOptionsettingsRequest As New GetOptionSettingRequestType
            Dim oGetOptionsettingsResponse As New GetOptionSettingResponseType

            With oGetOptionsettingsRequest
                .BranchCode = "HeadOff"
                .OptionNumber = 87
                .OptionType = OptionType.ProductOption
            End With

            Try
                oGetOptionsettingsResponse = oSAM.GetOptionSetting(oGetOptionsettingsRequest)

                With oGetOptionsettingsResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                    Else
                        If .OptionValue = "1" Then
                            If rblPaymentTerms.SelectedValue = "PayNow" Then
                                Response.Redirect("CashList.aspx")
                            Else
                                Response.Redirect("PrePayment.aspx")
                            End If
                        Else
                            If rblPaymentTerms.SelectedValue = "PayNow" Then
                                Response.Redirect("CashList.aspx")
                            End If

                            Dim oBindQuoteRequest As New BindQuoteRequestType
                            Dim oBindQuoteResponse As New BindQuoteResponseType
                            With oBindQuoteRequest
                                .BranchCode = "HeadOff"
                                .CreditTransactions = Nothing
                                .DebitAgainstSpecified = False
                                .InsuranceFileKey = Session("InsuranceFileKey")
                                .PaymentMethodSpecified = False
                                .TransactionType = "MTR"

                            End With

                            Try
                                oBindQuoteResponse = oSAM.BindQuote(oBindQuoteRequest)
                                With oBindQuoteResponse
                                    If Not (.Errors) Is Nothing Then
                                        'errors returned, so throw an exception
                                        lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                                    Else
                                        lblPolicyNum.Text = .Policy.PolicyRef
                                        lblPolicyNum.Visible = True
                                    End If
                                End With

                            Catch os As SamResponseException
                                'should do some error handling here. Just output error for now
                                Response.Write("An error occured calling SAM:<br>" & os.Message)
                            Catch oe As Exception
                                'should do some error handling here. Just output error for now
                                Response.Write("An error occured:<br>" & oe.Message)
                            Finally
                                'clean
                            End Try


                        End If
                    End If

                End With

            Catch os As SamResponseException
                'should do some error handling here. Just output error for now
                Response.Write("An error occured calling SAM:<br>" & os.Message)
            Catch oe As Exception
                'should do some error handling here. Just output error for now
                Response.Write("An error occured:<br>" & oe.Message)
            Finally
                'clean
            End Try
        End If

    End Sub

   
    Protected Sub grdAgentCommission_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdAgentCommission.RowDataBound

        Dim oAgentCommisssion As New BaseGetHeaderAndAgentCommissionByKeyResponseTypeRow
        If e.Row.RowType = DataControlRowType.DataRow Then
            oAgentCommisssion = DirectCast(e.Row.DataItem, BaseGetHeaderAndAgentCommissionByKeyResponseTypeRow)
            If oAgentCommisssion.AgentType = "Sub-Agent" Then
                dSubAgentTax = dSubAgentTax + oAgentCommisssion.TaxValue
                dSubAgentCommission = dSubAgentCommission + oAgentCommisssion.CommissionValue
                dSubAgnetTotalNetPremium = dSubAgnetTotalNetPremium + oAgentCommisssion.Premium
            Else
                dLeadTax = dLeadTax + oAgentCommisssion.TaxValue
                dLeadCommission = dLeadCommission + oAgentCommisssion.CommissionValue
                dLeadTotalNetPremium = dLeadTotalNetPremium + oAgentCommisssion.Premium
            End If

        End If
    End Sub
    'PraveenGora
    
    'PraveenGora

    ''Start (Saurabh)
    Protected Sub chkSelectRisk_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        'Dim oSAM As New SAMForInsuranceV2
        'Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        'oSAM.SetClientCredential(UserToken)
        'oSAM.SetPolicy("SamClientPolicy")


        'Dim oUpdateRiskSelectionRequest As New UpdateRiskSelectionRequestType
        'Dim oUpdateRiskSelectionResponse As New UpdateRiskSelectionResponseType


        'For RowCount As Integer = 0 To grdLiskRisk.Rows.Count - 1
        '    If grdLiskRisk.Rows(RowCount).RowType = DataControlRowType.DataRow Then
        '        With oUpdateRiskSelectionRequest
        '            .BranchCode = "HeadOff"
        '            .InsuranceFileKey = Session("InsuranceFileKey")
        '            .InsuranceFolderKey = Session("InsuranceFolderKey")

        '            If DirectCast(grdLiskRisk.Rows(RowCount).FindControl("chkSelectRisk"), CheckBox).Checked Then
        '                .IsSelected = 1
        '            Else
        '                .IsSelected = 0
        '            End If

        '            .RiskKey = DirectCast(Session("Risks"), BaseGetHeaderAndRisksByKeyResponseTypeRow())(RowCount).RiskKey
        '            .TransactionType = TransactionType.NB
        '            .TimeStamp = Session("TimeStamp")

        '        End With
        '        Try
        '            oUpdateRiskSelectionResponse = oSAM.UpdateRiskSelection(oUpdateRiskSelectionRequest)

        '            With oUpdateRiskSelectionResponse
        '                If Not (.Errors) Is Nothing Then
        '                    'errors returned, so throw an exception
        '                    lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
        '                Else
        '                    Session("TimeStamp") = .TimeStamp
        '                End If
        '            End With

        '        Catch os As SamResponseException
        '            'should do some error handling here. Just output error for now
        '            Response.Write("An error occured calling SAM:<br>" & os.Message)
        '        Catch oe As Exception
        '            'should do some error handling here. Just output error for now
        '            Response.Write("An error occured:<br>" & oe.Message)
        '        Finally
        '            'clean
        '        End Try

        '    End If

        'Next

        'GetHeaderAndRisksByKey()
        'GetHeaderAndRisksByKey()
        'GetHeaderAndRisksTaxByKey()
        'GetHeaderAndAgentCommissionByKey()






    End Sub
    Private Sub GetHeaderAndRisksByKey()


        Dim InsuranceFileKey As Integer = Session("InsuranceFileKey")
        Dim oGetHeaderAndRisksByKeyRequestType As New GetHeaderAndRisksByKeyRequestType
        Dim oGetHeaderAndRisksByKeyResponseType As New GetHeaderAndRisksByKeyResponseType
        Try

            oGetHeaderAndRisksByKeyRequestType.BranchCode = "HeadOff"
            oGetHeaderAndRisksByKeyRequestType.InsuranceFileKey = Session("InsuranceFileKey")


            Dim oSAM As New SAMForInsuranceV2

            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")
            Dim StartDate As Date
            StartDate = Date.Now
            oGetHeaderAndRisksByKeyResponseType = oSAM.GetHeaderAndRisksByKey(oGetHeaderAndRisksByKeyRequestType)
            WriteToLog(Session, "GetListRisks_Risk.aspx", "SAMForInsuranceV2", "GetHeaderAndRisksByKey", StartDate, Date.Now)
            If Not (oGetHeaderAndRisksByKeyResponseType.Errors) Is Nothing Then
                Throw New SamResponseException(oGetHeaderAndRisksByKeyResponseType.Errors)
            End If
            With oGetHeaderAndRisksByKeyResponseType
                lblInsuranceFileKey.Text = .InsuranceFileKey
                lblClientCode.Text = .ClientCode
                lblInsuranceFileRef.Text = .InsuranceFileRef
                lblAgent.Text = .Agent
                lblInceptionDate.Text = .InceptionDate
                lblCoverStartDate.Text = .CoverStartDate
                lblExpiryDate.Text = .ExpiryDate
                lblCurrency.Text = .Currency
                lblNetTotal.Text = .NetTotal
                lblTaxTotal.Text = .TaxTotal
                lblFeeTotal.Text = .FeeTotal
                lblGrossTotal.Text = .GrossTotal

            End With
            MultiView1.ActiveViewIndex = 0
            grdLiskRisk.DataSource = oGetHeaderAndRisksByKeyResponseType.Risks
            grdLiskRisk.DataBind()

            Session("Risks") = oGetHeaderAndRisksByKeyResponseType.Risks

        Catch ex As Exception
            lblError.Text = "Error occured: " + ex.Message
        End Try

        lblError.Text = ""

    End Sub

    Protected Sub grdLiskRisk_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdLiskRisk.RowDataBound


        If e.Row.RowType = DataControlRowType.DataRow Then
            DirectCast(e.Row.FindControl("chkSelectRisk"), CheckBox).Checked = DirectCast(e.Row.DataItem, BaseGetHeaderAndRisksByKeyResponseTypeRow).IsRisk
        End If

    End Sub
    Private Sub GetHeaderAndRisksFeesByKey()
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        Dim oSAM As New SAMForInsuranceV2

        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oGetHeaderAndPolicyFeesByKeyRequest As New GetHeaderAndPolicyFeesByKeyRequestType
        Dim oGetHeaderAndPolicyFeesByKeyResponseType As New GetHeaderAndPolicyFeesByKeyResponseType

        oGetHeaderAndPolicyFeesByKeyRequest.BranchCode = "HeadOff" ''Session("BRANCHCODE").ToString()
        oGetHeaderAndPolicyFeesByKeyRequest.InsuranceFileKey = Session("InsuranceFileKey") 'Convert.ToInt32(Session("INSURANCEFILEKEY"))
        Dim StartDate As Date
        StartDate = Date.Now
        oGetHeaderAndPolicyFeesByKeyResponseType = oSAM.GetHeaderAndPolicyFeesByKey(oGetHeaderAndPolicyFeesByKeyRequest)
        WriteToLog(Session, "GetListRisks_Risk.aspx", "SAMForInsuranceV2", "GetHeaderAndPolicyFeesByKey", StartDate, Date.Now)
        grdPolicyFees.DataSource = oGetHeaderAndPolicyFeesByKeyResponseType.PolicyFees
        grdPolicyFees.DataBind()
        With oGetHeaderAndPolicyFeesByKeyResponseType
            lblTotalPolicyFees.Text = .TotalPolicyFees.ToString
            lblTotalRiskFees.Text = .TotalRiskFees.ToString
            lblPolicyTotlafeeEligibleForFinancing.Text = .TotalPolicyFeesEligibleForFinancing.ToString
            lblPolicyTotlafeeExcludedFromFinancing.Text = .TotalPolicyFeesExcludedFromFinancing.ToString
            lblTotlafeeEligibleForFinancing.Text = .TotalRiskFeesEligibleForFinancing.ToString
            lblTotlafeeExcludedFromFinancing.Text = .TotalRiskFeesExcludedFromFinancing.ToString
        End With
    End Sub
    Private Sub GetHeaderAndRisksTaxByKey()
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oGetHeaderAndPolicyTaxByKeyRequest As New GetHeaderAndPolicyTaxByKeyRequestType
        Dim oGetHeaderAndPolicyTaxByKeyResponse As New GetHeaderAndPolicyTaxByKeyResponseType

        oGetHeaderAndPolicyTaxByKeyRequest.BranchCode = "HeadOff" ''Session("BRANCHCODE").ToString()
        oGetHeaderAndPolicyTaxByKeyRequest.InsuranceFileKey = Session("InsuranceFileKey") ''.ToInt32(Session("INSURANCEFILEKEY"))

        Dim StartDate As Date
        StartDate = Date.Now
        oGetHeaderAndPolicyTaxByKeyResponse = oSAM.GetHeaderAndPolicyTaxByKey(oGetHeaderAndPolicyTaxByKeyRequest)
        WriteToLog(Session, "GetListRisks_Risk.aspx", "SAMForInsuranceV2", "GetHeaderAndPolicyTaxByKey", StartDate, Date.Now)
        grdPolicyTaxes.DataSource = oGetHeaderAndPolicyTaxByKeyResponse.PolicyTaxes
        grdPolicyTaxes.DataBind()
        With oGetHeaderAndPolicyTaxByKeyResponse
            lblTotalPolicyTaxs.Text = .TotalPolicyTax.ToString
            lblTotalRiskTaxs.Text = .TotalRiskTax.ToString
            lblPolicyTotlaTaxEligibleForFinancing.Text = .TotalPolicyTaxEligibleForFinancing.ToString
            lblPolicyTotlaTaxExcludedFromFinancing.Text = .TotalPolicyTaxExcludedFromFinancing.ToString
            lblTotlaTaxEligibleForFinancing.Text = .TotalRiskTaxEligibleForFinancing.ToString
            lblTotlaTaxExcludedFromFinancing.Text = .TotalRiskTaxExcludedFromFinancing.ToString
        End With
    End Sub
    Private Sub GetHeaderAndAgentCommissionByKey()
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oGetHeaderAndAgentCommissionByKeyRequest As New GetHeaderAndAgentCommissionByKeyRequestType
        Dim oGetHeaderAndAgentCommissionByKeyResponse As New GetHeaderAndAgentCommissionByKeyResponseType

        oGetHeaderAndAgentCommissionByKeyRequest.BranchCode = "HeadOff" ''Session("BRANCHCODE").ToString()
        oGetHeaderAndAgentCommissionByKeyRequest.InsuranceFileKey = Session("InsuranceFileKey") ''Convert.ToInt32(Session("INSURANCEFILEKEY"))

        Dim StartDate As Date
        StartDate = Date.Now
        oGetHeaderAndAgentCommissionByKeyResponse = oSAM.GetHeaderAndAgentCommissionByKey(oGetHeaderAndAgentCommissionByKeyRequest)
        WriteToLog(Session, "GetListRisks_Risk.aspx", "SAMForInsuranceV2", "GetHeaderAndAgentCommissionByKey", StartDate, Date.Now)

        grdAgentCommission.DataSource = oGetHeaderAndAgentCommissionByKeyResponse.AgentCommission
        grdAgentCommission.DataBind()

        lblLeadAgentCommission.Text = dLeadCommission.ToString
        lblSubAgentCommission.Text = dSubAgentCommission.ToString
        lblTotalTax.Text = dLeadTax.ToString
        lblSubAgentTotalTax.Text = dSubAgentTax.ToString
        lblLeadAgentTotalNetPremium.Text = (Convert.ToDecimal(lblGrossTotal.Text) - dLeadCommission - dLeadTax).ToString
        lblSubagentPremium.Text = (dSubAgnetTotalNetPremium - dSubAgentCommission - dSubAgentTax).ToString

    End Sub

    ''End (Saurabh)
    Private Sub SetStatus(ByVal iInsuranceFileKey As Integer)
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        'create the request and response objects
        Dim oUpdateRenewalStatusRequest As New UpdateRenewalStatusRequestType
        Dim oUpdateRenewalStatusResponse As New UpdateRenewalStatusResponseType
        'Use GetHeaderAndSummariesByKey to get timestamp on InsuranceFolderKey that has to be passed on request of GenerateInvite
        Dim oGetHeaderAndSummariesByKeyRequest As New GetHeaderAndSummariesByKeyRequestType
        Dim oGetHeaderAndSummariesByKeResponse As New GetHeaderAndSummariesByKeyResponseType

        oGetHeaderAndSummariesByKeyRequest.BranchCode = "HeadOff"
        oGetHeaderAndSummariesByKeyRequest.InsuranceFileKey = iInsuranceFileKey
        oGetHeaderAndSummariesByKeResponse = oSAM.GetHeaderAndSummariesByKey(oGetHeaderAndSummariesByKeyRequest)

        If Not (oGetHeaderAndSummariesByKeResponse.Errors) Is Nothing Then
            'errors returned, so throw an exception
            Throw New SamResponseException(oGetHeaderAndSummariesByKeResponse.Errors)
        End If

        'set up request object with some values
        With oUpdateRenewalStatusRequest
            If Session("BranchCode") IsNot Nothing Then
                .BranchCode = Convert.ToString(Session("BranchCode"))
            Else
                .BranchCode = "HeadOff"
            End If

            .InsuranceFileKey = iInsuranceFileKey
            .QuoteTimeStamp = oGetHeaderAndSummariesByKeResponse.QuoteTimeStamp
            .RenewalStatusCode = "Update"
        End With


        oUpdateRenewalStatusResponse = oSAM.UpdateRenewalStatus(oUpdateRenewalStatusRequest)
        'oGenerateInviteResponseType.

        With oUpdateRenewalStatusResponse
            If Not (.Errors) Is Nothing Then
                'errors returned, so throw an exception
                Throw New SamResponseException(.Errors)
            End If
        End With

    End Sub
End Class
