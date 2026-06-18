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

        If (Not IsPostBack) Then
            Dim oGetHeaderAndRisksByKeyRequestType As New GetHeaderAndRisksByKeyRequestType
            Dim oGetHeaderAndRisksByKeyResponseType As New GetHeaderAndRisksByKeyResponseType
            Try


                'Session("BRANCHCODE") = "HeadOff"
                'Session("INSURANCEFILEKEY") = 2446
                'Session("InsuranceFolderKey") = 1607
                oGetHeaderAndRisksByKeyRequestType.BranchCode = Session("BRANCHCODE").ToString()
                oGetHeaderAndRisksByKeyRequestType.InsuranceFileKey = Session("INSURANCEFILEKEY").ToString()


                Dim oSAM As New SAMForInsuranceV2

                Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
                oSAM.SetClientCredential(UserToken)
                oSAM.SetPolicy("SamClientPolicy")

                oGetHeaderAndRisksByKeyResponseType = oSAM.GetHeaderAndRisksByKey(oGetHeaderAndRisksByKeyRequestType)

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
            Catch ex As Exception
                lblError.Text = "Error occured: " + ex.Message
            End Try
        End If
        lblError.Text = ""
    End Sub

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick

        Dim oGetHeaderAndRisksByKeyRequestType As New GetHeaderAndRisksByKeyRequestType
        Dim oGetHeaderAndRisksByKeyResponseType As New GetHeaderAndRisksByKeyResponseType
        Dim oSAM As New SAMForInsuranceV2
        Try

            oGetHeaderAndRisksByKeyRequestType.BranchCode = Session("BRANCHCODE").ToString()
            oGetHeaderAndRisksByKeyRequestType.InsuranceFileKey = Convert.ToInt32(Session("INSURANCEFILEKEY"))

            Dim iSelectedIndex As Integer
            iSelectedIndex = Menu1.Items.IndexOf(Menu1.SelectedItem)
            MultiView1.ActiveViewIndex = iSelectedIndex

            If (iSelectedIndex = 0) Then
                Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
                oSAM.SetClientCredential(UserToken)
                oSAM.SetPolicy("SamClientPolicy")

                oGetHeaderAndRisksByKeyResponseType = oSAM.GetHeaderAndRisksByKey(oGetHeaderAndRisksByKeyRequestType)
                grdLiskRisk.DataSource = oGetHeaderAndRisksByKeyResponseType.Risks
                grdLiskRisk.DataBind()

            ElseIf (iSelectedIndex = 1) Then
                Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
                oSAM.SetClientCredential(UserToken)
                oSAM.SetPolicy("SamClientPolicy")

                Dim oGetHeaderAndPolicyFeesByKeyRequest As New GetHeaderAndPolicyFeesByKeyRequestType
                Dim oGetHeaderAndPolicyFeesByKeyResponseType As New GetHeaderAndPolicyFeesByKeyResponseType

                oGetHeaderAndPolicyFeesByKeyRequest.BranchCode = Session("BRANCHCODE").ToString()
                oGetHeaderAndPolicyFeesByKeyRequest.InsuranceFileKey = Convert.ToInt32(Session("INSURANCEFILEKEY"))
                oGetHeaderAndPolicyFeesByKeyResponseType = oSAM.GetHeaderAndPolicyFeesByKey(oGetHeaderAndPolicyFeesByKeyRequest)

                With oGetHeaderAndPolicyFeesByKeyResponseType
                    lblTotalPolicyFees.Text = .TotalPolicyFees.ToString
                    lblTotalRiskFees.Text = .TotalRiskFees.ToString
                    lblPolicyTotlafeeEligibleForFinancing.Text = .TotalPolicyFeesEligibleForFinancing.ToString
                    lblPolicyTotlafeeExcludedFromFinancing.Text = .TotalPolicyFeesExcludedFromFinancing.ToString
                    lblTotlafeeEligibleForFinancing.Text = .TotalRiskFeesEligibleForFinancing.ToString
                    lblTotlafeeExcludedFromFinancing.Text = .TotalRiskFeesExcludedFromFinancing.ToString
                End With
                grdPolicyFees.DataSource = oGetHeaderAndPolicyFeesByKeyResponseType.PolicyFees
                grdPolicyFees.DataBind()
            ElseIf (iSelectedIndex = 2) Then
                Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
                oSAM.SetClientCredential(UserToken)
                oSAM.SetPolicy("SamClientPolicy")

                Dim oGetHeaderAndPolicyTaxByKeyRequest As New GetHeaderAndPolicyTaxByKeyRequestType
                Dim oGetHeaderAndPolicyTaxByKeyResponse As New GetHeaderAndPolicyTaxByKeyResponseType

                oGetHeaderAndPolicyTaxByKeyRequest.BranchCode = Session("BRANCHCODE").ToString()
                oGetHeaderAndPolicyTaxByKeyRequest.InsuranceFileKey = Convert.ToInt32(Session("INSURANCEFILEKEY"))
                oGetHeaderAndPolicyTaxByKeyResponse = oSAM.GetHeaderAndPolicyTaxByKey(oGetHeaderAndPolicyTaxByKeyRequest)

                With oGetHeaderAndPolicyTaxByKeyResponse
                    lblTotalPolicyTaxs.Text = .TotalPolicyTax.ToString
                    lblTotalRiskTaxs.Text = .TotalRiskTax.ToString
                    lblPolicyTotlaTaxEligibleForFinancing.Text = .TotalPolicyTaxEligibleForFinancing.ToString
                    lblPolicyTotlaTaxExcludedFromFinancing.Text = .TotalPolicyTaxExcludedFromFinancing.ToString
                    lblTotlaTaxEligibleForFinancing.Text = .TotalRiskTaxEligibleForFinancing.ToString
                    lblTotlaTaxExcludedFromFinancing.Text = .TotalRiskTaxExcludedFromFinancing.ToString
                End With
                grdPolicyTaxes.DataSource = oGetHeaderAndPolicyTaxByKeyResponse.PolicyTaxes
                grdPolicyTaxes.DataBind()
            ElseIf (iSelectedIndex = 3) Then
                Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
                oSAM.SetClientCredential(UserToken)
                oSAM.SetPolicy("SamClientPolicy")

                Dim oGetHeaderAndAgentCommissionByKeyRequest As New GetHeaderAndAgentCommissionByKeyRequestType
                Dim oGetHeaderAndAgentCommissionByKeyResponse As New GetHeaderAndAgentCommissionByKeyResponseType

                oGetHeaderAndAgentCommissionByKeyRequest.BranchCode = Session("BRANCHCODE").ToString()
                oGetHeaderAndAgentCommissionByKeyRequest.InsuranceFileKey = Convert.ToInt32(Session("INSURANCEFILEKEY"))
                oGetHeaderAndAgentCommissionByKeyResponse = oSAM.GetHeaderAndAgentCommissionByKey(oGetHeaderAndAgentCommissionByKeyRequest)

                grdAgentCommission.DataSource = oGetHeaderAndAgentCommissionByKeyResponse.AgentCommission
                grdAgentCommission.DataBind()

                lblLeadAgentCommission.Text = dLeadCommission.ToString
                lblSubAgentCommission.Text = dSubAgentCommission.ToString
                lblTotalTax.Text = dLeadTax.ToString
                lblSubAgentTotalTax.Text = dSubAgentTax.ToString
                lblLeadAgentTotalNetPremium.Text = (dLeadTotalNetPremium - dLeadCommission - dLeadTax).ToString
                lblSubagentPremium.Text = (dSubAgnetTotalNetPremium - dSubAgentCommission - dSubAgentTax).ToString
            End If

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
        
        Dim a(10) As Byte
        a(0) = Convert.ToByte(1)
        With oAddRiskRequestType
            .BranchCode = "Headoff"
            .RiskTypeCode = "MOBILE"
            .ScreenCode = "MOBILE"
            .DataModelCode = "MOBILECLM"
            .RunDefaultRules = True
            .RiskDescription = "Mobile Phone Cover"

            .InsuranceFolderKey = Session("InsuranceFolderKey")
            .InsuranceFileKey = Session("InsuranceFileKey")
            .QuoteTimeStamp = Session("QuoteTimeStamp")
            .ProductCode = "MOBILE"
        End With

        Session("AddRiskRequestType") = oAddRiskRequestType
        Response.Redirect("AddRisk.aspx")
    End Sub

    Protected Sub btnEditRisk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditRisk.Click
        'Response.Redirect("UpdateRisk.aspx")
    End Sub

    Protected Sub btnSaveQuote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveQuote.Click
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        Dim oSAM As New SAMForInsuranceV2
        Dim oUpdateQuoteRequest As New UpdateQuoteRequestType
        Dim oUpdateQuoteResponse As New UpdateQuoteResponseType

        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oHeaderAndSummariesByKeyResponse As New GetHeaderAndSummariesByKeyResponseType
        Dim oHeaderAndSummariesByKeyRequest As New GetHeaderAndSummariesByKeyRequestType

        oHeaderAndSummariesByKeyRequest.InsuranceFileKey = Session("INSURANCEFILEKEY") '2447
        oHeaderAndSummariesByKeyRequest.BranchCode = Session("BRANCHCODE") '"HeadOff"
        oHeaderAndSummariesByKeyResponse = oSAM.GetHeaderAndSummariesByKey(oHeaderAndSummariesByKeyRequest)

        With oUpdateQuoteRequest
            .AlternativeRef = oHeaderAndSummariesByKeyResponse.AlternativeRef
            .AnalysisCode = oHeaderAndSummariesByKeyResponse.AnalysisCode
            .BranchCode = Session("BranchCode")
            .ConsolidatedLeadAgentCommission = oHeaderAndSummariesByKeyResponse.ConsolidatedLeadAgentCommission
            .ConsolidatedLeadAgentCommissionSpecified = oHeaderAndSummariesByKeyResponse.ConsolidatedLeadAgentCommissionSpecified
            .ConsolidatedSubAgentCommission = oHeaderAndSummariesByKeyResponse.ConsolidatedSubAgentCommission
            .ConsolidatedSubAgentCommissionSpecified = oHeaderAndSummariesByKeyResponse.ConsolidatedSubAgentCommissionSpecified
            .CoverEndDate = lblExpiryDate.Text
            '.CoverNoteBookNumber =
            '.CoverNoteSheetNumber =
            '.CoverNoteSheetNumberSpecified =
            .CoverStartDate = lblCoverStartDate.Text
            .CurrencyCode = lblCurrency.Text
            .Description = Session("MTAREASON").ToString
            .InsuranceFileKey = Session("InsuranceFileKey")
            .InsuranceFolderKey = Session("InsuranceFolderKey")
            '.InsuredParties =
            .QuoteTimeStamp = Session("QuoteTimeStamp")
            oUpdateQuoteResponse = oSAM.UpdateQuote(oUpdateQuoteRequest)
        End With
    End Sub

    Protected Sub grdLiskRisk_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdLiskRisk.RowCommand
        If (e.CommandName.Equals("Select")) Then
            Session("RiskKey") = e.CommandArgument
            Response.Redirect("EditRisk.aspx")
        End If
    End Sub

    Protected Sub grdLiskRisk_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdLiskRisk.SelectedIndexChanged

    End Sub

    Protected Sub btnMakeLive_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMakeLive.Click
        Dim oSAM As New SAMForInsuranceV2
        Dim oBindQuoteRequest As New BindQuoteRequestType
        Dim oBindQuoteResponse As New BindQuoteResponseType

        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oHeaderAndSummariesByKeyResponse As New GetHeaderAndSummariesByKeyResponseType
        Dim oHeaderAndSummariesByKeyRequest As New GetHeaderAndSummariesByKeyRequestType

        oHeaderAndSummariesByKeyRequest.InsuranceFileKey = Session("INSURANCEFILEKEY")
        oHeaderAndSummariesByKeyRequest.BranchCode = Session("BRANCHCODE")
        oHeaderAndSummariesByKeyResponse = oSAM.GetHeaderAndSummariesByKey(oHeaderAndSummariesByKeyRequest)

        With oBindQuoteRequest
            .BranchCode = Session("BranchCode")
            .InsuranceFileKey = Session("InsuranceFileKey")
            .PaymentMethod = 0 'oHeaderAndSummariesByKeyResponse.PaymentMethodCode
            .PaymentMethodSpecified = True
            '.PayNowDetails = '
            '.PayTrueMonthlyPolicyMTAPremiumOnRenewal=
            '.PayTrueMonthlyPolicyMTAPremiumOnRenewalSpecified=
            '.SelectedInstalmentQuote = 
            '.TransactionType=
            oBindQuoteResponse = oSAM.BindQuote(oBindQuoteRequest)
        End With

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
End Class
