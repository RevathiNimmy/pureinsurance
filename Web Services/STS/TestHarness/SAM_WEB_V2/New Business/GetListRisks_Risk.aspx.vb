Imports SAMForInsuranceV2
Imports Microsoft.Web.Services3.Security.Tokens
Partial Class GetListRisksRisk
    Inherits System.Web.UI.Page
    Dim StartDate As Date
    Dim dLeadCommission As New Double
    Dim dSubAgentCommission As New Double
    Dim dLeadTax As New Double
    Dim dSubAgentTax As New Double
    Dim dLeadTotalNetPremium As New Double
    Dim dSubAgnetTotalNetPremium As New Double
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sCoverNoteNumberingId As String
        Dim sCoverNoteDefaultPeriod As String
        Dim sCoverNoteDocumentTemplate As String
        If Not Page.IsPostBack Then
            GetHeaderAndRisksByKey()
            GetHeaderAndRisksTaxByKey()
            GetHeaderAndAgentCommissionByKey()
            sCoverNoteNumberingId = GetProductRiskOptionValue(ProductRiskOptions.CoverNoteNumberingId)

            If sCoverNoteNumberingId <> "" Then
                sCoverNoteDefaultPeriod = GetProductRiskOptionValue(ProductRiskOptions.CoverNoteDefaultPeriod)
                Session("CoverNoteDefaultPeriod") = sCoverNoteDefaultPeriod
                Session("AttachCoverNote") = True

                sCoverNoteDocumentTemplate = GetProductRiskOptionValue(ProductRiskOptions.CoverNoteDocTemplate)
                If sCoverNoteDocumentTemplate <> "" Then
                    Session("GenerateCoverNoteDocument") = True
                Else
                    Session("GenerateCoverNoteDocument") = False
                End If



            Else
                grdLiskRisk.Columns(3).Visible = False
                Session("AttachCoverNote") = False
            End If

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
        Dim oAddRiskRequestType As New AddRiskRequestType
        Panel1.Visible = True
        'Response.Redirect("AddRisk.aspx")

    End Sub

    Protected Sub btnEditRisk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditRisk.Click
        Response.Redirect("MotorEditRisk.aspx")
    End Sub

    Protected Sub btnSaveQuote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveQuote.Click
        'JP 18/03/2010
        If rblPaymentTerms.Items(2).Selected Then
            Dim oSAM As New SAMForInsuranceV2
            Dim oUpdateQuoteV2RequestType As New UpdateQuoteV2RequestType
            Dim oUpdateQuoteV2ResponseType As New UpdateQuoteV2ResponseType
            Dim objAddUpdateQuoteRequest As Object

            If rblPaymentTerms.Items(2).Selected Then

                With oUpdateQuoteV2RequestType
                    .Timestamp = Session("TimeStamp")
                    .AgentKey = Int32.Parse(objAddUpdateQuoteRequest.AgentKey)
                    If .AgentKey = 0 Then
                        .AgentKeySpecified = False
                    Else
                        .AgentKeySpecified = True
                    End If
                    .AlternativeRef = objAddUpdateQuoteRequest.AlternativeRef
                    .AnalysisCode = objAddUpdateQuoteRequest.AnalysisCode
                    .BranchCode = objAddUpdateQuoteRequest.BranchCode
                    .SubBranchCode = objAddUpdateQuoteRequest.SubBranchCode
                    .CoverEndDate = objAddUpdateQuoteRequest.CoverEndDate
                    .CoverNoteSheetNumberSpecified = objAddUpdateQuoteRequest.CoverNoteSheetNumberSpecified
                    .CoverStartDate = objAddUpdateQuoteRequest.CoverStartDate
                    .CurrencyCode = objAddUpdateQuoteRequest.CurrencyCode
                    .Description = objAddUpdateQuoteRequest.Description
                    .InsuredName = objAddUpdateQuoteRequest.InsuredName
                    .PartyKey = Session("PartyKey")
                    .ProductCode = objAddUpdateQuoteRequest.ProductCode
                    .QuoteRef = objAddUpdateQuoteRequest.QuoteRef
                    .SubBranchCode = "HeadOff"
                    .CoverNoteBookNumber = objAddUpdateQuoteRequest.CoverNoteBookNumber
                    .FrequencyCode = objAddUpdateQuoteRequest.FrequencyCode
                    .HandlerCode = objAddUpdateQuoteRequest.HandlerCode
                    .BusinessTypeCode = objAddUpdateQuoteRequest.BusinessTypeCode
                    If .CoverNoteSheetNumberSpecified Then
                        .CoverNoteSheetNumber = objAddUpdateQuoteRequest.CoverNoteSheetNumber
                        .CoverNoteSheetNumberSpecified = True
                    Else
                        .CoverNoteSheetNumberSpecified = False
                    End If

                    .InceptionDate = objAddUpdateQuoteRequest.InceptionDate
                    .InceptionTPI = objAddUpdateQuoteRequest.InceptionTPI

                    If .IssuedDateSpecified Then
                        .IssuedDate = objAddUpdateQuoteRequest.IssuedDate
                        .IssuedDateSpecified = True
                    Else
                        .IssuedDateSpecified = False
                    End If

                    If .LapseCancelDateSpecified Then
                        .LapseCancelDate = objAddUpdateQuoteRequest.LapseCancelDate
                        .LapseCancelDateSpecified = True
                    Else
                        .LapseCancelDateSpecified = False
                    End If

                    .LapseCancelReasonCode = objAddUpdateQuoteRequest.LapseCancelReasonCode
                    If .LTUExpiryDateSpecified Then
                        .LTUExpiryDate = objAddUpdateQuoteRequest.LTUExpiryDate
                        .LTUExpiryDateSpecified = True
                    Else
                        .LTUExpiryDateSpecified = False
                    End If

                    ''.ProposalDate
                    .QuoteExpiryDate = objAddUpdateQuoteRequest.QuoteExpiryDate
                    .QuoteRef = objAddUpdateQuoteRequest.QuoteRef

                    .ReferredAtMTA = objAddUpdateQuoteRequest.ReferredAtMTA
                    .ReferredAtMTASpecified = objAddUpdateQuoteRequest.ReferredAtMTASpecified
                    .ReferredAtRenewal = objAddUpdateQuoteRequest.ReferredAtRenewal
                    .ReferredAtRenewalSpecified = objAddUpdateQuoteRequest.ReferredAtRenewalSpecified
                    .Regarding = objAddUpdateQuoteRequest.Regarding
                    .RenewalDate = objAddUpdateQuoteRequest.RenewalDate
                    .RenewalMethodCode = objAddUpdateQuoteRequest.RenewalMethodCode
                    .StopReasonCode = objAddUpdateQuoteRequest.StopReasonCode
                    If Session("SelectedPolicy") Is Nothing Then
                        'JP - For New Business
                        .InsuranceFileKey = Session("InsuranceFileKey")
                        .InsuranceFolderKey = Session("InsuranceFolderKey")
                    Else
                        .InsuranceFileKey = objAddUpdateQuoteRequest.InsuranceFileKey
                        .InsuranceFolderKey = objAddUpdateQuoteRequest.InsuranceFolderKey
                    End If

                    'JP Mark this quote for Quote Collection
                    '.Quotecollection = true

                End With
                Try
                    StartDate = Date.Now

                    oUpdateQuoteV2ResponseType = oSAM.UpdateQuoteV2(oUpdateQuoteV2RequestType)
                    WriteToLog(Session, "GetListRisks_Risk.aspx", "SAMForInsuranceV2", "UpdateQuoteV2", StartDate, Date.Now)

                    With oUpdateQuoteV2ResponseType

                        If Not (.Errors) Is Nothing Then
                            'errors returned, so throw an exception
                            lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                        Else
                            Session("TimeStamp") = .TimeStamp
                            Response.Write("<script>var t = confirm('Quote is sucessfully saved'); if(t)window.location='../UIIC_demo/HomePage.aspx'  </script>")
                        End If
                    End With

                Catch os As SamResponseException
                    'should do some error handling here. Just output error for now
                    'Response.Write("An error occured calling SAM:<br>" & os.Message)
                Catch oe As Exception
                    'should do some error handling here. Just output error for now
                    'Response.Write("An error occured:<br>" & oe.Message)
                Finally
                    'clean
                End Try
            End If

        Else
            Response.Write("<script>var t = confirm('Quote is sucessfully saved'); if(t)window.location='../UIIC_demo/HomePage.aspx'  </script>")
        End If

    End Sub

    Protected Sub grdLiskRisk_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdLiskRisk.RowCommand
        If e.CommandName = "Edit" Then
            If (e.CommandName.Equals("Delete")) Then
                Session("RiskAction") = "Delete"
            Else
                Session("RiskAction") = ""
            End If
            Session("RiskKey") = e.CommandArgument
            Response.Redirect("MotorEditRiskQQ.aspx")
        End If


    End Sub

   

    Protected Sub btnMakeLive_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMakeLive.Click
        'PraveenGora
        Session("Amount") = Convert.ToDecimal(lblLeadAgentTotalNetPremium.Text)

       

        'PraveenGora
        Dim oSAM As New SAMForInsuranceV2
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        SaveCoverNote()
        Dim oGetOptionsettingsRequest As New GetOptionSettingRequestType
        Dim oGetOptionsettingsResponse As New GetOptionSettingResponseType

        With oGetOptionsettingsRequest
            .BranchCode = "HeadOff"
            .OptionNumber = 87
            .OptionType = OptionType.ProductOption
        End With

        Try
            StartDate = Date.Now
            oGetOptionsettingsResponse = oSAM.GetOptionSetting(oGetOptionsettingsRequest)
            WriteToLog(Session, "GetListRisks_Risk.aspx", "SAMForInsuranceV2", "GetOptionSetting",StartDate, Date.Now)

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
                            .TransactionType = "NB"

                        End With

                        Try
                            StartDate = Date.Now
                            oBindQuoteResponse = oSAM.BindQuote(oBindQuoteRequest)
                            WriteToLog(Session, "GetListRisks_Risk.aspx", "SAMForInsuranceV2", "BindQuote", StartDate, Date.Now)
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
            If System.AppDomain.CurrentDomain.GetData("BUSINESSTYPE").ToString() = "Agency Business" Then
                lblAgent.Visible = True
                lblAgent1.Visible = True

            Else
                lblAgent.Visible = False
                lblAgent1.Visible = False

            End If
        Catch ex As Exception
            lblError.Text = "Error occured: " + ex.Message
        End Try

        lblError.Text = ""

    End Sub

    Protected Sub grdLiskRisk_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdLiskRisk.RowDataBound


        If e.Row.RowType = DataControlRowType.DataRow Then
            DirectCast(e.Row.FindControl("chkSelectRisk"), CheckBox).Checked = DirectCast(e.Row.DataItem, BaseGetHeaderAndRisksByKeyResponseTypeRow).IsRisk
            If DirectCast(e.Row.DataItem, BaseGetHeaderAndRisksByKeyResponseTypeRow).CoverNote IsNot Nothing Then

                DirectCast(e.Row.FindControl("chkAttachCoverNote"), CheckBox).Checked = True


            End If
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

        Session("GrossTotal") = lblGrossTotal.Text
    End Sub

    ''End (Saurabh)

    Protected Sub RadioButtonList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonList1.SelectedIndexChanged

        Panel1.Visible = False
        If RadioButtonList1.SelectedValue = 1 Then
            Response.Redirect("MOTORAddRisk.aspx")
        Else
            Response.Redirect("MOTORAddRiskQQ.aspx")
        End If



    End Sub

    Protected Sub rbtnCopyRisk_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnCopyRisk.SelectedIndexChanged
        If grdLiskRisk.SelectedIndex <> -1 Then


            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
            Dim oSAM As New SAMForInsuranceV2
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")
            Dim oRequest As New CopyRiskRequestType
            Dim oResponse As New CopyRiskResponseType
            oRequest.BranchCode = "HeadOff"
            If rbtnCopyRisk.SelectedItem.Text = "Comprative" Then
                oRequest.CopyType = CopyRiskType.Comparative
            Else
                oRequest.CopyType = CopyRiskType.Duplicate
            End If
            oRequest.InsuranceFileKey = Session("InsuranceFileKey")
            oRequest.InsuranceFolderKey = Session("InsuranceFolderKey")
            oRequest.RiskNumber = Convert.ToInt32(grdLiskRisk.SelectedRow.Cells(8).Text)
            oRequest.RiskKey = Convert.ToInt32(grdLiskRisk.SelectedRow.Cells(10).Text)
            StartDate = Date.Now
            oResponse = oSAM.CopyRisk(oRequest)
            WriteToLog(Session, "GetListRisks_Risk.aspx", "SAMForInsuranceV2", "CopyRisk",StartDate, Date.Now)
            GetHeaderAndRisksByKey()
            rbtnCopyRisk.Visible = False
            rbtnCopyRisk.ClearSelection()

        End If
    End Sub

    Protected Sub btncopyrisk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncopyrisk.Click
        If rbtnCopyRisk.Visible = True Then
            rbtnCopyRisk.Visible = False
        Else
            rbtnCopyRisk.Visible = True
        End If

    End Sub

    Protected Sub btnDeleteRisk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteRisk.Click

        If grdLiskRisk.SelectedIndex <> -1 Then

            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
            Dim oSAM As New SAMForInsuranceV2
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")
            Dim oRequest As New DeleteRiskRequestType
            Dim oResponse As New DeleteRiskResponseType
            oRequest.BranchCode = "HeadOff"
            oRequest.InsuranceFileKey = Session("InsuranceFileKey")
            oRequest.InsuranceFolderKey = Session("InsuranceFolderKey")
            oRequest.RiskKey = Convert.ToInt32(grdLiskRisk.SelectedRow.Cells(10).Text)
            oRequest.QuoteTimeStamp = Session("TimeStamp")
            StartDate = Date.Now
            oResponse = oSAM.DeleteRisk(oRequest)
            WriteToLog(Session, "GetListRisks_Risk.aspx", "SAMForInsuranceV2", "DeleteRisk",StartDate, Date.Now)
            With oResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                Else
                    Session("TimeStamp") = .QuoteTimeStamp
                End If
            End With

            GetHeaderAndRisksByKey()
        End If

    End Sub

    Protected Sub grdLiskRisk_SelectedIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSelectEventArgs) Handles grdLiskRisk.SelectedIndexChanging

    End Sub
    Protected Sub chkAttachCoverNote_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim chkTemp As CheckBox = CType(sender, CheckBox)
        Dim gvr As GridViewRow
        gvr = CType(chkTemp.Parent.Parent, GridViewRow)

        'JP bypassing this function AttachDettachCoverNote(chkTemp, gvr.RowIndex)

    End Sub

    Private Function GetProductRiskOptionValue(ByVal sOption As ProductRiskOptions) As String
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oRequest As New ProductRiskOptionValueRequestType
        Dim oResponse As New ProductRiskOptionValueResponseType

        oRequest.ActionType = ProductConfigActionType.ProductRiskMaintenance
        oRequest.BranchCode = "HeadOff"
        oRequest.ProducRiskOption = sOption
        oRequest.ProducRiskOptionSpecified = True
        oRequest.ProductCode = Session("ProductCode")
        StartDate = Date.Now
        oResponse = oSAM.GetProductRiskOptionValue(oRequest)
        WriteToLog(Session, "GetListRisks_Risk.aspx", "SAMForInsuranceV2", "GetProductRiskOptionValue", StartDate, Date.Now)
        Return oResponse.ProductRiskOptionValue

    End Function

    Private Sub AttachDettachCoverNote(ByVal chkBox As CheckBox, ByVal iIndex As Integer)
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        Dim oSAM As New SAMForInsuranceV2

        Dim oRisks As BaseGetHeaderAndRisksByKeyResponseTypeRow()

        Dim oRequest As New AttachCoverNoteRequestType
        Dim oResponse As New AttachCoverNoteResponseType

        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        oRisks = DirectCast(Session("Risks"), BaseGetHeaderAndRisksByKeyResponseTypeRow())


        If Not chkBox.Checked Then
            oRequest.ProcessType = CoverNoteProcessType.Dettach
        Else
            oRequest.ProcessType = CoverNoteProcessType.Attach
        End If

        'Dim oGetNumberingSchemeRequest As New GetNumberingSchemeNoRequestType
        'Dim oGetNumberingSchemeResponse As New GetNumberingSchemeNoResponseType

        'oGetNumberingSchemeRequest.BranchCode = Session("BranchCode")
        'oGetNumberingSchemeRequest.SchemeType = NumberingSchemeType.CoverNote
        'oGetNumberingSchemeRequest.ProductCode = Session("ProductCode")
        'oGetNumberingSchemeRequest.AgentKey = Session("LeadAgentKey")

        'oGetNumberingSchemeResponse = oSAM.GetNumberingSchemeNo(oGetNumberingSchemeRequest)

        'With oGetNumberingSchemeResponse
        '    If .Errors IsNot Nothing Then
        '        lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
        '    Else
        '        oRequest.BranchCode = Session("BranchCode")
        '        oRequest.CoverNote.CoverNoteNumber = oGetNumberingSchemeResponse.GeneratedCode
        '        oRequest.CoverNote.CoverNoteFrom = Date.Now
        '        oRequest.CoverNote.CoverNoteFromSpecified = False
        '        oRequest.CoverNote.CoverNoteTo = Date.Today.AddDays(Convert.ToDouble(Session("CoverNoteDefaultPeriod")))
        '        oRequest.CoverNote.RiskKey = oRisks(iIndex).RiskKey
        '        oRequest.CoverNote.RiskDesc = oRisks(iIndex).RiskTypeDescription
        '        oRequest.CoverNote.TImeStamp = Session("TimeStamp")

        '    End If
        'End With

        oRequest.BranchCode = Session("BranchCode")
        oRequest.CoverNote = New BaseCoverNoteRiskItemType
        'JP oRequest.CoverNote.CheckMandatory = False
        oRequest.CoverNote.RiskKey = oRisks(iIndex).RiskKey
        oRequest.CoverNote.TImeStamp = Session("timestamp") ' oRisks(iIndex).CoverNote.TImeStamp
        'JP oRequest.InsuranceFolderKey = Session("InsuranceFolderKey")
        StartDate = Date.Now
        oResponse = oSAM.AttachCoverNote(oRequest)
        WriteToLog(Session, "GetListRisks_Risk.aspx", "SAMForInsuranceV2", "AttachCoverNote", StartDate, Date.Now)

        With oResponse
            If Not .Errors Is Nothing Then
                lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
            Else
                GetHeaderAndRisksByKey()
                Session("TimeStamp") = oResponse.TimeStamp

            End If


        End With







    End Sub


    Private Sub SaveCoverNote()
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        Dim oSAM As New SAMForInsuranceV2

        Dim oRisks As BaseGetHeaderAndRisksByKeyResponseTypeRow()

        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        oRisks = DirectCast(Session("Risks"), BaseGetHeaderAndRisksByKeyResponseTypeRow())

        For Each oRisk As BaseGetHeaderAndRisksByKeyResponseTypeRow In oRisks
            If oRisk.IsRisk = True And oRisk.CoverNote IsNot Nothing Then

                Dim oRequest As New AttachCoverNoteRequestType
                Dim oResponse As New AttachCoverNoteResponseType

                oSAM.SetClientCredential(UserToken)
                oSAM.SetPolicy("SamClientPolicy")

                Dim oGetNumberingSchemeRequest As New GetNumberingSchemeNoRequestType
                Dim oGetNumberingSchemeResponse As New GetNumberingSchemeNoResponseType

                oGetNumberingSchemeRequest.BranchCode = Session("BranchCode")
                oGetNumberingSchemeRequest.SchemeType = NumberingSchemeType.CoverNote
                'JP oGetNumberingSchemeRequest.ProductCode = Session("ProductCode")
                oGetNumberingSchemeRequest.AgentKey = Session("LeadAgentKey")
                StartDate = Date.Now
                oGetNumberingSchemeResponse = oSAM.GetNumberingSchemeNo(oGetNumberingSchemeRequest)
                WriteToLog(Session, "GetListRisks_Risk.aspx", "SAMForInsuranceV2", "GetNumberingSchemeNo", StartDate, Date.Now)

                With oGetNumberingSchemeResponse
                    If .Errors IsNot Nothing Then
                        lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                    Else
                        oRequest.BranchCode = Session("BranchCode")
                        oRequest.CoverNote = New BaseCoverNoteRiskItemType
                        'JP oRequest.CoverNote.CheckMandatory = True
                        oRequest.CoverNote.RiskKey = oRisk.RiskKey
                        oRequest.CoverNote.CoverNoteNumber = .GeneratedCode
                        'JP oRequest.TimeStamp = Session("TimeStamp")
                        oRequest.CoverNote.CoverNoteFrom = Date.Now.AddMinutes(2)
                        oRequest.CoverNote.CoverNoteFromSpecified = True
                        oRequest.CoverNote.CoverNoteTo = Date.Now.AddDays(Convert.ToDouble(Session("CoverNoteDefaultPeriod")))
                        oRequest.CoverNote.CoverNoteToSpecified = True
                        'JP oRequest.InsuranceFolderKey = Session("InsuranceFolderKey")
                        oRequest.ProcessType = CoverNoteProcessType.Attach
                        oRequest.GenerateCoverNoteDocs = Session("GenerateCoverNoteDocument")
                        oRequest.CoverNote.RiskDesc = oRisk.Description
                        StartDate = Date.Now
                        oResponse = oSAM.AttachCoverNote(oRequest)
                        WriteToLog(Session, "GetListRisks_Risk.aspx", "SAMForInsuranceV2", "AttachCoverNote", StartDate, Date.Now)

                        With oResponse
                            If Not .Errors Is Nothing Then
                                lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                            Else
                                Session("TimeStamp") = oResponse.TimeStamp
                            End If


                        End With
                    End If
                End With

            End If
        Next

        







    End Sub
End Class
