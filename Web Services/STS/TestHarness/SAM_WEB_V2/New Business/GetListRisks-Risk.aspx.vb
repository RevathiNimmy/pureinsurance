Imports SAMForInsuranceV2
Imports Microsoft.Web.Services3.Security.Tokens
Partial Class GetListRisksRisk
    Inherits System.Web.UI.Page
    Dim StartDate As Date
    Dim InsuranceFileKey As Integer = DirectCast(Session("SelectedPolicy"), BaseGetAllPolicyVersionsResponseTypeRow).insuranceFileKey

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If (Not IsPostBack) Then
            Dim oGetHeaderAndRisksByKeyRequestType As New GetHeaderAndRisksByKeyRequestType
            Dim oGetHeaderAndRisksByKeyResponseType As New GetHeaderAndRisksByKeyResponseType
            Try

                oGetHeaderAndRisksByKeyRequestType.BranchCode = "HeadOff"
                oGetHeaderAndRisksByKeyRequestType.InsuranceFileKey = DirectCast(Session("SelectedPolicy"), BaseGetAllPolicyVersionsResponseTypeRow).insuranceFileKey


                Dim oSAM As New SAMForInsuranceV2

                Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
                oSAM.SetClientCredential(UserToken)
                oSAM.SetPolicy("SamClientPolicy")
                StartDate = Date.Now
                oGetHeaderAndRisksByKeyResponseType = oSAM.GetHeaderAndRisksByKey(oGetHeaderAndRisksByKeyRequestType)
                WriteToLog(Session, "GetListRisks-Risk.aspx", "SAMForInsuranceV2", "GetHeaderAndRisksByKey",StartDate, Date.Now)
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

            oGetHeaderAndRisksByKeyRequestType.BranchCode = "HeadOff" 'Session("BRANCHCODE").ToString()
            oGetHeaderAndRisksByKeyRequestType.InsuranceFileKey = InsuranceFileKey

            Dim iSelectedIndex As Integer
            iSelectedIndex = Menu1.Items.IndexOf(Menu1.SelectedItem)
            MultiView1.ActiveViewIndex = iSelectedIndex

            If (iSelectedIndex = 0) Then
                Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
                oSAM.SetClientCredential(UserToken)
                oSAM.SetPolicy("SamClientPolicy")
                StartDate = Date.Now
                oGetHeaderAndRisksByKeyResponseType = oSAM.GetHeaderAndRisksByKey(oGetHeaderAndRisksByKeyRequestType)
                WriteToLog(Session, "GetListRisks-Risk.aspx", "SAMForInsuranceV2", "GetHeaderAndRisksByKey",StartDate, Date.Now)
                grdLiskRisk.DataSource = oGetHeaderAndRisksByKeyResponseType.Risks
                grdLiskRisk.DataBind()

            ElseIf (iSelectedIndex = 1) Then
                Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
                oSAM.SetClientCredential(UserToken)
                oSAM.SetPolicy("SamClientPolicy")

                Dim oGetHeaderAndPolicyFeesByKeyRequest As New GetHeaderAndPolicyFeesByKeyRequestType
                Dim oGetHeaderAndPolicyFeesByKeyResponseType As New GetHeaderAndPolicyFeesByKeyResponseType

                oGetHeaderAndPolicyFeesByKeyRequest.BranchCode = "HeadOff" ''Session("BRANCHCODE").ToString()
                oGetHeaderAndPolicyFeesByKeyRequest.InsuranceFileKey = InsuranceFileKey 'Convert.ToInt32(Session("INSURANCEFILEKEY"))
                StartDate = Date.Now
                oGetHeaderAndPolicyFeesByKeyResponseType = oSAM.GetHeaderAndPolicyFeesByKey(oGetHeaderAndPolicyFeesByKeyRequest)
                WriteToLog(Session, "GetListRisks-Risk.aspx", "SAMForInsuranceV2", "GetHeaderAndPolicyFeesByKey",StartDate, Date.Now)
                grdPolicyFees.DataSource = oGetHeaderAndPolicyFeesByKeyResponseType.PolicyFees
                grdPolicyFees.DataBind()
            ElseIf (iSelectedIndex = 2) Then
                Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
                oSAM.SetClientCredential(UserToken)
                oSAM.SetPolicy("SamClientPolicy")

                Dim oGetHeaderAndPolicyTaxByKeyRequest As New GetHeaderAndPolicyTaxByKeyRequestType
                Dim oGetHeaderAndPolicyTaxByKeyResponse As New GetHeaderAndPolicyTaxByKeyResponseType

                oGetHeaderAndPolicyTaxByKeyRequest.BranchCode = "HeadOff" ''Session("BRANCHCODE").ToString()
                oGetHeaderAndPolicyTaxByKeyRequest.InsuranceFileKey = InsuranceFileKey ''.ToInt32(Session("INSURANCEFILEKEY"))
                StartDate = Date.Now
                oGetHeaderAndPolicyTaxByKeyResponse = oSAM.GetHeaderAndPolicyTaxByKey(oGetHeaderAndPolicyTaxByKeyRequest)
                WriteToLog(Session, "GetListRisks-Risk.aspx", "SAMForInsuranceV2", "GetHeaderAndPolicyTaxByKey",StartDate, Date.Now)
                grdPolicyTaxes.DataSource = oGetHeaderAndPolicyTaxByKeyResponse.PolicyTaxes
                grdPolicyTaxes.DataBind()
            ElseIf (iSelectedIndex = 3) Then
                Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
                oSAM.SetClientCredential(UserToken)
                oSAM.SetPolicy("SamClientPolicy")

                Dim oGetHeaderAndAgentCommissionByKeyRequest As New GetHeaderAndAgentCommissionByKeyRequestType
                Dim oGetHeaderAndAgentCommissionByKeyResponse As New GetHeaderAndAgentCommissionByKeyResponseType

                oGetHeaderAndAgentCommissionByKeyRequest.BranchCode = "HeadOff" ''Session("BRANCHCODE").ToString()
                oGetHeaderAndAgentCommissionByKeyRequest.InsuranceFileKey = InsuranceFileKey ''Convert.ToInt32(Session("INSURANCEFILEKEY"))
                StartDate = Date.Now
                oGetHeaderAndAgentCommissionByKeyResponse = oSAM.GetHeaderAndAgentCommissionByKey(oGetHeaderAndAgentCommissionByKeyRequest)
                WriteToLog(Session, "GetListRisks-Risk.aspx", "SAMForInsuranceV2", "GetHeaderAndAgentCommissionByKey",StartDate, Date.Now)
                grdAgentCommission.DataSource = oGetHeaderAndAgentCommissionByKeyResponse.AgentCommission
                grdAgentCommission.DataBind()
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

            .InsuranceFolderKey = DirectCast(Session("SelectedPolicy"), BaseGetAllPolicyVersionsResponseTypeRow).InsuranceFolderKey
            .InsuranceFileKey = InsuranceFileKey
            .QuoteTimeStamp = Session("QuoteTimeStamp")
            .ProductCode = "MOBILE"
        End With

        Session("AddRiskRequestType") = oAddRiskRequestType
        'Response.Redirect("AddRisk.aspx")
        Response.Redirect("MOTORAddRisk.aspx")

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
        StartDate = Date.Now
        oHeaderAndSummariesByKeyResponse = oSAM.GetHeaderAndSummariesByKey(oHeaderAndSummariesByKeyRequest)
        WriteToLog(Session, "GetListRisks-Risk.aspx", "SAMForInsuranceV2", "GetHeaderAndSummariesByKey",StartDate, Date.Now)

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
            StartDate = Date.Now
            oUpdateQuoteResponse = oSAM.UpdateQuote(oUpdateQuoteRequest)
            WriteToLog(Session, "GetListRisks-Risk.aspx", "SAMForInsuranceV2", "UpdateQuote",StartDate, Date.Now)
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
        StartDate = Date.Now
        oHeaderAndSummariesByKeyResponse = oSAM.GetHeaderAndSummariesByKey(oHeaderAndSummariesByKeyRequest)
        WriteToLog(Session, "GetListRisks-Risk.aspx", "SAMForInsuranceV2", "GetHeaderAndSummariesByKey", StartDate, Date.Now)

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
            StartDate = Date.Now
            oBindQuoteResponse = oSAM.BindQuote(oBindQuoteRequest)
            WriteToLog(Session, "GetListRisks-Risk.aspx", "SAMForInsuranceV2", "BindQuote",StartDate, Date.Now)
        End With

    End Sub
End Class
