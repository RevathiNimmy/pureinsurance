Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2

Partial Class MTA_PolicyHeader
    Inherits System.Web.UI.Page
     Dim StartDate As Date
    Dim oSAM As New SAMForInsuranceV2



    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        mvPolicyHeaders.ActiveViewIndex = Int32.Parse(e.Item.Value)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oResponse As New GetHeaderAndSummariesByKeyResponseType
        Dim oRequest As New GetHeaderAndSummariesByKeyRequestType
        If Not Page.IsPostBack Then
            Try
                oRequest.InsuranceFileKey = Session("InsuranceFilekey") '2447
                oRequest.BranchCode = Session("BRANCHCODE") '"HeadOff"
                StartDate = Date.Now
                oResponse = oSAM.GetHeaderAndSummariesByKey(oRequest)
                WriteToLog(Session, "PolicyHeader.aspx", "SAMForInsuranceV2", "GetHeaderAndSummariesByKey", StartDate, Date.Now)

                txtInsuredName.Text = oResponse.InsuredName
                
                BuildLists(oSAM, ddlPolictStatus, STSListType.PMLookup, "Policy_Status", oResponse.PolicyStatusCode)
                ddlPolictStatus.SelectedIndex = ddlPolictStatus.Items.IndexOf(ddlPolictStatus.Items.FindByValue(oResponse.PolicyStatusCode.Trim))
                BuildLists(oSAM, ddlAnalysisCode, STSListType.PMLookup, "Analysis_code", oResponse.AnalysisCode)
                ddlAnalysisCode.SelectedIndex = ddlAnalysisCode.Items.IndexOf(ddlAnalysisCode.Items.FindByValue(oResponse.AnalysisCode.Trim))
                BuildLists(oSAM, ddlBranchCode, STSListType.PMLookup, "source", oResponse.BranchCode)
                ddlBranchCode.SelectedIndex = ddlBranchCode.Items.IndexOf(ddlBranchCode.Items.FindByValue(oResponse.BranchCode.Trim))
                BuildLists(oSAM, ddlBusinessType, STSListType.PMLookup, "Business_Type", oResponse.BusinessTypeCode)
                ddlBusinessType.SelectedIndex = ddlBusinessType.Items.IndexOf(ddlBusinessType.Items.FindByValue(oResponse.BusinessTypeCode.Trim))
                BuildLists(oSAM, ddlCurrency, STSListType.PMLookup, "Currency", oResponse.CurrencyCode)
                ddlCurrency.SelectedIndex = ddlCurrency.Items.IndexOf(ddlCurrency.Items.FindByValue(oResponse.CurrencyCode.Trim))
                BuildLists(oSAM, ddlSubBranch, STSListType.PMLookup, "Sub_Branch", oResponse.SubBranchCode)
                ddlSubBranch.SelectedIndex = ddlSubBranch.Items.IndexOf(ddlSubBranch.Items.FindByValue(oResponse.SubBranchCode.Trim))
                BuildLists(oSAM, ddlFrequencyCode, STSListType.PMLookup, "Renewal_Frequency", oResponse.RenewalFrequencyCode)
                ddlFrequencyCode.SelectedIndex = ddlFrequencyCode.Items.IndexOf(ddlFrequencyCode.Items.FindByValue(oResponse.RenewalFrequencyCode.Trim))
                BuildLists(oSAM, ddlRenewalmethodCode, STSListType.PMLookup, "Renewal_Method", oResponse.RenewalMethodCode)
                ddlRenewalmethodCode.SelectedIndex = ddlRenewalmethodCode.Items.IndexOf(ddlRenewalmethodCode.Items.FindByValue(oResponse.RenewalMethodCode.Trim))
                BuildLists(oSAM, ddlStopReasoCode, STSListType.PMLookup, "Renewal_stop_code", oResponse.StopReasonCode)
                ddlStopReasoCode.SelectedIndex = ddlStopReasoCode.Items.IndexOf(ddlStopReasoCode.Items.FindByValue(oResponse.StopReasonCode.Trim))
                BuildLists(oSAM, ddlLapseCancellation, STSListType.PMLookup, "Lapsed_Reason", oResponse.LapsedReasonCode)
                ddlLapseCancellation.SelectedIndex = ddlLapseCancellation.Items.IndexOf(ddlLapseCancellation.Items.FindByValue(oResponse.LapsedReasonCode.Trim))
                BuildLists(oSAM, ddlProduct, STSListType.PMLookup, "Product", oResponse.ProductCode)
                ddlProduct.SelectedIndex = ddlProduct.Items.IndexOf(ddlProduct.Items.FindByValue(oResponse.ProductCode.Trim))
                'ddlPolictStatus.SelectedValue = "CUR"
                bindColumns(oResponse)
                BuildLists(oSAM, ddlPolicyStyle, STSListType.PMLookup, "Policy_Style", oResponse.PolicyStyleCode)
                Session("InsuranceFolderKey") = oResponse.InsuranceFolderKey
                Session("InsuranceFileKey") = oResponse.InsuranceFileKey
                Session("oldkey") = oResponse.InsuranceFileKey
                Session("oldInsuranceFolderKey") = oResponse.InsuranceFolderKey
                Session("QuoteTimeStamp") = oResponse.QuoteTimeStamp
                '#Prakash: Commenting this line because at this point, txtShortName.text is empty
                'Session("AgentName") = txtShortName.Text

                ddlPolicyStyle.Items.Insert(0, New ListItem("None", ""))

                '#Prakash: GetHeaderAndSummaryByKey returns the agent information also. so we can use that instead of the following lines
                'txtHandler.Text = DirectCast(Session("SelectedPolicy"), BaseGetPartySummaryResponseTypeRow)
                'txtShortName.Text = DirectCast(Session("SelectedPolicy"), BaseGetPartySummaryResponseTypeRow).AgentShortName
                'hfAgentKey.Value = DirectCast(Session("SelectedPolicy"), BaseGetPartySummaryResponseTypeRow).LeadAgentKey

                txtShortName.Text = oResponse.LeadAgent
                hfAgentKey.Value = oResponse.LeadAgentKey
                Session("AgentName") = oResponse.LeadAgent
                Session("AgentKey") = oResponse.LeadAgentKey

                lblPolicyStatus.Text = oResponse.PolicyTypeCode

                Dim oGetSubAgentsRequest As New GetSubAgentsRequestType
                Dim oGetSubAgentResponse As New GetSubAgentsResponseType

                With oGetSubAgentsRequest
                    .InsuranceFileKey = Session("InsuranceFileKey")
                    
                    .BranchCode = "HeadOff"

                End With


                StartDate = Date.Now
                oGetSubAgentResponse = oSAM.GetSubAgents(oGetSubAgentsRequest)
                WriteToLog(Session, "PolicyHeader.aspx", "SAMForInsuranceV2", "GetSubAgents", StartDate, Date.Now)

                With oGetSubAgentResponse
                    If Not .Errors Is Nothing Then
                        Response.Write(GetMessageFromSamError(.Errors))
                    Else
                        If Session("SubAgents") Is Nothing Then
                            gvSubAgents.DataSource = .SubAgents
                            gvSubAgents.DataBind()
                            Session("SubAgents") = .SubAgents

                        End If


                    End If
                End With

                Dim oGetStandardPolicyWordingRequest As New GetStandardPolicyWordingsRequestType
                Dim oGetStandardPolicyWordingResponse As New GetStandardPolicyWordingsResponseType


                With oGetStandardPolicyWordingRequest
                    .InsuranceFileKey = Session("InsuranceFileKey")

                    .BranchCode = "HeadOff"
                End With
                StartDate = Date.Now

                oGetStandardPolicyWordingResponse = oSAM.GetStandardPolicyWordings(oGetStandardPolicyWordingRequest)
                WriteToLog(Session, "PolicyHeader.aspx", "SAMForInsuranceV2", "GetStandardPolicyWordings", StartDate, Date.Now)

                With oGetStandardPolicyWordingResponse
                    If Not .Errors Is Nothing Then
                            ''lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                    Else
                        If Session("PolicyWording") Is Nothing Then
                                Dim oDocTemplates As DocTemplate()
                                ReDim oDocTemplates(.DocumentTemplates.Length)

                                For iDocCount As Integer = 0 To .DocumentTemplates.Length - 1
                                    oDocTemplates(iDocCount) = New DocTemplate
                                    oDocTemplates(iDocCount).Code = .DocumentTemplates(iDocCount).Code
                                    oDocTemplates(iDocCount).Description = .DocumentTemplates(iDocCount).Description
                                Next
                                gvStandardWording.DataSource = oDocTemplates
                                gvStandardWording.DataBind()
                                Session("PolicyWording") = oDocTemplates
                        End If



                    End If
                End With



                mvPolicyHeaders.ActiveViewIndex = 0

                If ddlBusinessType.SelectedValue <> "DIRECT" Then
                    btnAgentCode.Enabled = True
                Else
                    btnAgentCode.Enabled = False

                End If
            Catch ex As Exception

            End Try

        End If
        If ddlBusinessType.SelectedValue <> "DIRECT" Then
            btnAgentCode.Enabled = True
        Else
            btnAgentCode.Enabled = False

        End If

        If Not Session("SubAgents") Is Nothing Then
            gvSubAgents.DataSource = DirectCast(Session("SubAgents"), BaseGetSubAgentsResponseTypeRow())
            gvSubAgents.DataBind()
        End If
        If Not Session("PolicyWording") Is Nothing Then
            gvStandardWording.DataSource = DirectCast(Session("PolicyWording"), DocTemplate())
            gvStandardWording.DataBind()
        End If
    End Sub
    Private Sub bindColumns(ByVal oResponse As GetHeaderAndSummariesByKeyResponseType)
        txtInsuredName.Text = oResponse.InsuredName
        txtPolicyNo.Text = oResponse.InsuranceFileRef
        If oResponse.PolicyStatusCode = "" Then
            ddlPolictStatus.SelectedValue = "CUR"
            txtStatus.Text = "Current"
        Else
            ddlPolictStatus.SelectedValue = oResponse.PolicyStatusCode.Trim
            txtStatus.Text = "Current" 'oResponse.PolicyStatusCode
        End If
        If (Session("PROCESS").ToString = "MTA") Then
            If (IsDate(Session("MTADATE"))) Then
                txtcoveredFrom.Text = Session("MTADATE").ToString
            Else
                txtcoveredFrom.Text = Now.Date
            End If
            txtcoveredFrom.Text = CDate(Session("MTADATE"))
        Else
            txtcoveredFrom.Text = oResponse.CoverStartDate
        End If
        txtcoveredFrom.ReadOnly = True
        txtcoveredTo.Text = oResponse.CoverEndDate.ToString   '("COVERTO")
        If (oResponse.ProposalDate.Year > 1899) Then
            txtHCExpiry.Text = oResponse.ProposalDate.ToString
        End If

        'Session("COVERTO")
        If (oResponse.InceptionDate.Year > 1899) Then
            txtinceptionDate.Text = oResponse.InceptionDate 'Session("COVERFROM")
        Else
            txtinceptionDate.Text = oResponse.InceptionDate 'Now.Date
        End If
        txtInceptionTPI.Text = oResponse.InceptionTPI 'Session("COVERFROM")

        If (oResponse.QuoteExpiryDate.Year > 1899) Then
            txtQuoteexpiry.Text = oResponse.QuoteExpiryDate.ToString ' Date.Now().Add(New System.TimeSpan(20, 0, 0, 0)).Date
        Else
            txtQuoteexpiry.Text = Date.Now().Add(New System.TimeSpan(20, 0, 0, 0)).Date
        End If
        txtRenewalDate.Text = oResponse.RenewalDate.ToString
        'If (IsDate(oResponse.RenewalDate)) Then
        '    txtRenewalDate.Text = Session("COVERTO")
        'Else
        'txtRenewalDate.Text = Now.Date
        If (oResponse.IssueDate.Year > 1899) Then
            txtIssuedDate.Text = oResponse.IssueDate.ToString
        End If

        txtInsuredName.Text = oResponse.InsuredName
        txtRegarding.Text = oResponse.Regarding
        txtAlternateRef.Text = oResponse.AlternativeRef
        ddlBusinessType.SelectedIndex = ddlBusinessType.Items.IndexOf(ddlBusinessType.Items.FindByValue(oResponse.BusinessTypeCode.Trim))
        ddlCurrency.SelectedIndex = ddlCurrency.Items.IndexOf(ddlCurrency.Items.FindByValue(oResponse.CurrencyCode.Trim))
        ddlAnalysisCode.SelectedIndex = ddlAnalysisCode.Items.IndexOf(ddlAnalysisCode.Items.FindByValue(oResponse.AnalysisCode.Trim))
        ddlBranchCode.SelectedIndex = ddlBranchCode.Items.IndexOf(ddlBranchCode.Items.FindByValue(oResponse.BranchCode.Trim))
        ddlSubBranch.SelectedIndex = ddlSubBranch.Items.IndexOf(ddlSubBranch.Items.FindByValue(oResponse.SubBranchCode.Trim))

    End Sub
    Private Sub BuildLists(ByVal oSAM As SAMForInsuranceV2, ByRef objControl As DropDownList, ByVal ESTSLookup As STSListType, ByVal ListCode As String, ByVal BindValue As String)
        Dim oRequest As New GetListRequestType
        Dim oResponse As New GetListResponseType


        oRequest.BranchCode = "HeadOff"
        oRequest.ListType = STSListType.PMLookup
        oRequest.ListCode = ListCode


        Try
                StartDate = Date.Now

            oResponse = oSAM.GetList(oRequest)
                WriteToLog(Session, "PolicyHeader.aspx", "SAMForInsuranceV2", "GetList", StartDate, Date.Now)
            With oResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Response.Write(GetMessageFromSamError(.Errors))
                Else

                    objControl.DataSource = oResponse.List
                    objControl.DataTextField = "Description"
                    objControl.DataValueField = "Code"
                    objControl.DataBind()
                    If (BindValue = "") Then
                        objControl.Items.Insert(0, New ListItem("", ""))
                    Else
                        objControl.SelectedValue = BindValue
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
            'clean up any objects here
        End Try

    End Sub

    Protected Sub cmdOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdOk.Click
        Session("AgentName") = txtShortName.Text
        If (Session("PROCESS").ToString = "MTA") Then

            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
            Dim oSAM As New SAMForInsuranceV2
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")

            Dim oRequest As New AddMtaQuoteRequestType
            Dim oResponse As New AddMtaQuoteResponseType

            With oRequest

                .AlternateReference = txtAlternateRef.Text
                .AnalysisCode = ddlAnalysisCode.SelectedValue
                .BranchCode = Session("BranchCode")
                .BusinessTypeCode = ddlBusinessType.Text
                .EffectiveDate = CDate(Session("MTADATE"))
                If (txtQuoteexpiry.Text <> "") Then
                    .ExpiryDate = CDate(txtQuoteexpiry.Text)

                Else
                    .ExpiryDate = Nothing
                End If




                .InsuranceFileKey = Session("InsuranceFileKey")

                .InsuredName = txtInsuredName.Text
                If (txtIssuedDate.Text <> "") Then
                    .IssueDate = txtIssuedDate.Text
                    .IssueDateSpecified = True
                Else
                    .IssueDate = Nothing
                    .IssueDateSpecified = False
                End If
                If txtLapseCancelDate.Text <> "" Then
                    .LapseCancelDate = txtLapseCancelDate.Text
                    .LapseCancelDateSpecified = True
                Else
                    .LapseCancelDateSpecified = False

                End If


                If (IsDate(txtHCExpiry.Text)) Then
                    .LTUExpiryDate = CDate(txtHCExpiry.Text)
                    .LTUExpiryDateSpecified = True
                Else
                    .LTUExpiryDate = Now.Date
                    .LTUExpiryDateSpecified = False
                End If

                .MtaReason = Session("MTAREASON").ToString
                .PolicyKey = txtPolicyNo.Text
                .PolicyStatusCode = ddlPolictStatus.Text

                If (txtinceptionDate.Text <> "") Then
                    .ProposalDate = CDate(txtinceptionDate.Text)
                Else
                    .ProposalDate = Now
                End If

                .ProposalDateSpecified = True




                ''' Added on 02062008
                .FrequencyCode = ddlFrequencyCode.Text
                .Regarding = txtRegarding.Text
                .PolicyStyleCode = ddlPolicyStyle.SelectedValue ''' HARD-CODED AS IF NOW
                .RenewalMethodCode = ddlRenewalmethodCode.SelectedValue
                .StopReasonCode = ddlStopReasoCode.SelectedValue
                If (CkBxReferrredatrenewal.Checked = True) Then
                    .ReferredAtRenewalSpecified = True
                    .ReferredAtRenewal = True
                Else
                    .ReferredAtRenewal = False

                End If
                If (CkBxReferrredonMTA.Checked = True) Then
                    .ReferredOnMTA = True
                    .ReferredOnMTASpecified = True
                Else
                    .ReferredOnMTA = False

                End If
                .LapseCancelReasonCode = ddlLapseCancellation.SelectedValue

                .TypeOfMta = Session("MTAISPERMANENT").ToString 'check with rahul

            End With
            Try
                StartDate = Date.Now

                oResponse = oSAM.AddMtaQuote(oRequest)
                WriteToLog(Session, "PolicyHeader.aspx", "SAMForInsuranceV2", "AddMtaQuote", StartDate, Date.Now)

                Session("InsuranceFileKey") = oResponse.InsuranceFileKey
                Session("QuoteTimeStamp") = oResponse.QuoteTimeStamp
                With oResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Session("ErrorMsg") = GetMessageFromSamError(.Errors)
                        Response.Write(GetMessageFromSamError(.Errors))
                        ' Response.Redirect("ErrorForMTA.aspx")
                    Else
                        Session("AlternateRef") = txtAlternateRef.Text
                        Session("AnalysisCode") = ddlAnalysisCode.Text

                        If ddlBusinessType.SelectedValue.Trim = "COIN LEAD" Then

                            Response.Redirect("Coinsurance.aspx")
                        Else
                            Response.Redirect("GetListRisks_Risk.aspx")
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
            UpDateQuote()

            Session("AlternateRef") = txtAlternateRef.Text
            Session("AnalysisCode") = ddlAnalysisCode.Text

            Response.Redirect("GetListRisks_Risk.aspx")
        End If
        
    End Sub
    Private Sub UpDateQuote()
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        'Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        'Dim oSAM As New SAMForInsuranceV2
        'oSAM.SetClientCredential(UserToken)
        'oSAM.SetPolicy("SamClientPolicy")

        'Dim oRequest As New UpdateQuoteRequestType
        'Dim oResponse As New UpdateQuoteResponseType

        'With oRequest
        '    .AlternativeRef = txtAlternateRef.Text
        '    .AnalysisCode = ddlAnalysisCode.Text
        '    .BranchCode = Session("BranchCode")
        '    '.ConsolidatedLeadAgentCommission=
        '    '.ConsolidatedLeadAgentCommissionSpecified
        '    '.ConsolidatedSubAgentCommission
        '    '.ConsolidatedSubAgentCommissionSpecified
        '    If (IsDate(txtcoveredTo.Text)) Then
        '        .CoverEndDate = txtcoveredTo.Text
        '    End If
        '    .CoverNoteBookNumber = txtCoverBook.Text
        '    If (IsNothing(txtCoverSheet.Text)) Then
        '        .CoverNoteSheetNumber = Convert.ToInt32(txtCoverSheet.Text)
        '    End If
        '    .CoverNoteSheetNumberSpecified = True
        '    If (IsDate(txtcoveredFrom.Text)) Then
        '        .CoverStartDate = CDate(txtcoveredFrom.Text)
        '    End If
        '    .CurrencyCode = ddlCurrency.SelectedValue
        '    .Description = "OTHER"
        '    .InsuranceFileKey = Convert.ToInt32(Session("InsuranceFileKey"))
        '    .InsuranceFolderKey = Convert.ToInt32(Session("InsuranceFolderKey"))
        '    '.InsuredParties
        '    .QuoteTimeStamp = Session("QuoteTimeStamp")
        'End With
        'oResponse = oSAM.UpdateQuote(oRequest)
        'Session("QuoteTimeStamp") = oResponse.QuoteTimeStamp


        Dim oUpdateQuoteV2RequestType As New UpdateQuoteV2RequestType
        Dim oUpdateQuoteV2ResponseType As New UpdateQuoteV2ResponseType

        With oUpdateQuoteV2RequestType
            .InsuranceFileKey = Session("InsuranceFileKey")
            .InsuranceFolderKey = Session("InsuranceFolderKey")
            If hfAgentKey.Value <> "" Then
                .AgentKey = Int32.Parse(hfAgentKey.Value)

                If .AgentKey = 0 Then
                    .AgentKeySpecified = False
                Else
                    .AgentKeySpecified = True
                End If
            End If
            .AlternativeRef = txtAlternateRef.Text
            .AnalysisCode = ddlAnalysisCode.SelectedValue
            .BranchCode = ddlBranchCode.SelectedValue
            .SubBranchCode = ddlSubBranch.SelectedValue
            .CoverEndDate = txtcoveredTo.Text
            .CoverNoteSheetNumberSpecified = False
            .CoverStartDate = txtcoveredFrom.Text
            .CurrencyCode = ddlCurrency.SelectedValue
            .Description = "Quote Updated"
            .InsuredName = txtInsuredName.Text
            .PartyKey = Session("PartyKey")
            .ProductCode = ddlProduct.SelectedValue
            .QuoteRef = txtPolicyNo.Text
            .SubBranchCode = "HeadOff"
            .CoverNoteBookNumber = txtCoverBook.Text
            .FrequencyCode = ddlFrequencyCode.SelectedValue

            .BusinessTypeCode = ddlBusinessType.SelectedValue

            If txtCoverSheet.Text <> "" Then
                .CoverNoteSheetNumber = Convert.ToInt32(txtCoverSheet.Text)
                .CoverNoteSheetNumberSpecified = True
            Else
                .CoverNoteSheetNumberSpecified = False
            End If

            .InceptionDate = txtinceptionDate.Text
            .InceptionTPI = txtInceptionTPI.Text

            If txtIssuedDate.Text <> "" Then
                .IssuedDate = txtIssuedDate.Text
                .IssuedDateSpecified = True
            Else
                .IssuedDateSpecified = False
            End If

            If txtLapseCancelDate.Text <> "" Then
                .LapseCancelDate = txtLapseCancelDate.Text
                .LapseCancelDateSpecified = True
            Else
                .LapseCancelDateSpecified = False
            End If
            .LapseCancelReasonCode = ddlLapseCancellation.SelectedValue
            If txtLTUExpiry.Text <> "" Then
                .LTUExpiryDate = txtLTUExpiry.Text
                .LTUExpiryDateSpecified = True
            Else
                .LTUExpiryDateSpecified = False
            End If

            ''.ProposalDate
            .QuoteExpiryDate = txtQuoteexpiry.Text
            .QuoteRef = txtPolicyNo.Text

            .ReferredAtMTA = CkBxReferrredonMTA.Checked
            If CkBxReferrredonMTA.Checked Then
                .ReferredAtMTASpecified = True
            Else
                .ReferredAtMTASpecified = False
            End If

            .ReferredAtRenewal = CkBxReferrredatrenewal.Checked
            If CkBxReferrredatrenewal.Checked Then
                .ReferredAtRenewalSpecified = True
            Else
                .ReferredAtRenewalSpecified = False
            End If

            .Regarding = txtRegarding.Text

            .RenewalDate = txtRenewalDate.Text
            .RenewalMethodCode = ddlRenewalmethodCode.SelectedValue
            .StopReasonCode = ddlStopReasoCode.SelectedValue
            .Timestamp = Session("QuoteTimeStamp")
                StartDate = Date.Now

            oUpdateQuoteV2ResponseType = oSAM.UpdateQuoteV2(oUpdateQuoteV2RequestType)
                WriteToLog(Session, "PolicyHeader.aspx", "SAMForInsuranceV2", "UpdateQuoteV2", StartDate, Date.Now)

            With oUpdateQuoteV2ResponseType

                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception


                Else
                    Session("QuoteTimeStamp") = .TimeStamp
                    UpdateSubagentsAndPolcyWordings()
                    If ddlBusinessType.SelectedValue.Trim = "COIN LEAD" Then

                        Response.Redirect("Coinsurance.aspx")
                    Else
                        Response.Redirect("GetListRisks_Risk.aspx")
                    End If
                End If
            End With
        End With

    End Sub

    Protected Sub btnAgentCode_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAgentCode.Click
        Session("AgentSearchType") = "Agent"
    End Sub

    Protected Sub btnAddPolicywording_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddPolicywording.Click

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If (Session("PROCESS").ToString = "MTA") Then
            Response.Redirect("MTACaptureDate.aspx")
        ElseIf (Session("PROCESS").ToString = "REN") Then
            Response.Redirect(Session("ReturnPage").ToString)
        Else
            Response.Redirect("ListPolicyVersions.aspx")
        End If



    End Sub
    Protected Sub txtcoveredTo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtcoveredTo.TextChanged
        txtRenewalDate.Text = txtcoveredTo.Text
    End Sub
    'Protected Sub txtcoveredTo_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtcoveredTo.Disposed
    '    txtRenewalDate.Text = txtcoveredTo.Text
    'End Sub
    'Protected Sub gvSubAgents_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSubAgents.SelectedIndexChanged
    '    Session("AgentSearchType") = "SubAgent"
    '    Response.Write("<script>window.open('FindAgent.aspx','_new');</script>")
    'End Sub
    Protected Sub btnCoinsurers_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCoinsurers.Click
        Response.Write("<script>window.open('CoInsurance.aspx');</script>")
    End Sub
    Private Sub UpdateSubagentsAndPolcyWordings()
        Dim oUpdateSubAgentsRequest As New UpdateSubAgentsRequestType
        Dim oUpdateSubAgentsResponse As New UpdateSubAgentsResponseType

        Dim oUpdateSubAgents As BaseUpdateSubAgentsRequestTypeSubAgentsRow()
        Dim oSubAgents As BaseGetSubAgentsResponseTypeRow()
        oSubAgents = DirectCast(Session("SubAgents"), BaseGetSubAgentsResponseTypeRow())
        If oSubAgents IsNot Nothing Then
            ReDim Preserve oUpdateSubAgents(oSubAgents.Length - 1)
            For Count As Integer = 0 To oUpdateSubAgents.Length - 1
                oUpdateSubAgents(Count) = New BaseUpdateSubAgentsRequestTypeSubAgentsRow
                oUpdateSubAgents(Count).Amount = oSubAgents(Count).Amount
                oUpdateSubAgents(Count).PartyKey = oSubAgents(Count).PartyKey
                oUpdateSubAgents(Count).Percentage = oSubAgents(Count).Percentage
            Next
            With oUpdateSubAgentsRequest
                .BranchCode = "HeadOff"
                .InsuranceFileKey = Session("InsuranceFileKey")
                .SubAgents = oUpdateSubAgents
                .TimeStamp = Session("TimeStamp")
            End With
            Try
                StartDate = Date.Now

                oUpdateSubAgentsResponse = oSAM.UpdateSubAgents(oUpdateSubAgentsRequest)
                WriteToLog(Session, "PolicyHeader.aspx", "SAMForInsuranceV2", "UpdateSubAgents", StartDate, Date.Now)

                With oUpdateSubAgentsResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        'lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                    Else
                        Session("TimeStamp") = .TimeStamp
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
        Dim oUpdateStandardPolicyWordingRequest As New UpdateStandardPolicyWordingsRequestType
        Dim oUpdateStandardPolicyWordingResponse As New UpdateStandardPolicyWordingsResponseType

        Dim oDocTemplates As DocTemplate()
        oDocTemplates = DirectCast(Session("PolicyWording"), DocTemplate())


        If oDocTemplates IsNot Nothing Then
            Dim oUpdateStandardWording As BaseUpdateStandardPolicyWordingsRequestTypePolicyStandardWordingsRow()
            If oDocTemplates IsNot Nothing Then
                ReDim Preserve oUpdateStandardWording(oDocTemplates.Length - 1)
            Else
                ReDim Preserve oUpdateStandardWording(0)
            End If


            For Count As Integer = 0 To oUpdateStandardWording.Length - 1
                oUpdateStandardWording(Count) = New BaseUpdateStandardPolicyWordingsRequestTypePolicyStandardWordingsRow
                oUpdateStandardWording(Count).Code = oDocTemplates(Count).Code
            Next

            With oUpdateStandardPolicyWordingRequest
                .BranchCode = "HeadOff"
                .InsuranceFileKey = Session("InsuranceFileKey")
                .PolicyStandardWordings = oUpdateStandardWording
                .TimeStamp = Session("TimeStamp")
            End With

            Try
                StartDate = Date.Now
                oUpdateStandardPolicyWordingResponse = oSAM.UpdateStandardPolicyWordings(oUpdateStandardPolicyWordingRequest)
                WriteToLog(Session, "PolicyHeader.aspx", "SAMForInsuranceV2", "UpdateStandardPolicyWordings",StartDate, Date.Now)
                With oUpdateStandardPolicyWordingResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        'lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                    Else
                        Session("TimeStamp") = .TimeStamp
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
    Protected Sub gvSubAgents_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvSubAgents.RowDeleting
        Dim oSubAgents As BaseGetSubAgentsResponseTypeRow()
        Dim oTempSubAgents As BaseGetSubAgentsResponseTypeRow()
        oSubAgents = DirectCast(Session("SubAgents"), BaseGetSubAgentsResponseTypeRow())

        For Count As Integer = 0 To oSubAgents.Length - 1
            If Count <> e.RowIndex Then
                If oTempSubAgents Is Nothing Then
                    ReDim Preserve oTempSubAgents(0)
                Else
                    ReDim Preserve oTempSubAgents(oTempSubAgents.Length)
                End If
                oTempSubAgents(oTempSubAgents.Length - 1) = oSubAgents(Count)
            Else
            End If
        Next
        Session("SubAgents") = oTempSubAgents
        gvSubAgents.DataSource = oTempSubAgents
        gvSubAgents.DataBind()
    End Sub
    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Session("AgentSearchType") = "SubAgent"
        Response.Write("<script>window.open('FindAgent.aspx','_new');</script>")
    End Sub
    Private Sub GetdefaultClauses()
        'JP Dim oRequest As New GetProductOrRiskClausesRequestType
        'JP Dim oResponse As New GetProductOrRiskClausesResponseType

        'JP With oRequest
        'JP .BranchCode = Session("BranchCode")
        'JP .ClauseSelType = ClauseSelectionType.Product
        'JP .CurrentBranchCode = Session("BranchCode")
        'JP .IsDefault = True
        'JP .ProductOrRiskTypeCode = Session("ProductCode")
        'JP End With

        'JP oResponse = oSAM.GetProductOrRiskClauses(oRequest)

        'JP With oResponse
        'JP If .Errors IsNot Nothing Then
        ''lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
        'JP Else
        'JP Dim oDocTemplates As DocTemplate()
        'JP ReDim oDocTemplates(.Documents.Length - 1)

        'JP For iDocCount As Integer = 0 To .Documents.Length - 1
        'JP oDocTemplates(iDocCount) = New DocTemplate
        'JP oDocTemplates(iDocCount).Code = .Documents(iDocCount).Code
        'JP oDocTemplates(iDocCount).Description = .Documents(iDocCount).Description
        'JP Next
        'JP gvStandardWording.DataSource = oDocTemplates
        'JP gvStandardWording.DataBind()
        'JP Session("PolicyWording") = oDocTemplates
        'JP End If
        'JP End With

    End Sub

    Protected Sub gvStandardWording_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvStandardWording.RowDeleting
        Dim oDocTemplates As DocTemplate()
        Dim oTempDocTemplates As DocTemplate()
        oDocTemplates = DirectCast(Session("PolicyWording"), DocTemplate())

        For Count As Integer = 0 To oDocTemplates.Length - 1
            If Count <> e.RowIndex Then
                If oTempDocTemplates Is Nothing Then
                    ReDim Preserve oTempDocTemplates(0)
                Else
                    ReDim Preserve oTempDocTemplates(oTempDocTemplates.Length)
                End If
                oTempDocTemplates(oTempDocTemplates.Length - 1) = oDocTemplates(Count)
            Else
            End If

        Next
        Session("PolicyWording") = oTempDocTemplates
        gvStandardWording.DataSource = oTempDocTemplates
        gvStandardWording.DataBind()
    End Sub

End Class
