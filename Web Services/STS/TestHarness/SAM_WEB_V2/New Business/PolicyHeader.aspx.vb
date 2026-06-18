Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2

Partial Class MTA_PolicyHeader
    Inherits System.Web.UI.Page
    Dim StartDate As Date
    Dim UserToken As UsernameToken
    Dim oSAM As New SAMForInsuranceV2

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        mvPolicyHeaders.ActiveViewIndex = Int32.Parse(e.Item.Value)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        UserToken = GetUserToken("sirius", "sirius")
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")


        Dim oResponse As New GetHeaderAndSummariesByKeyResponseType
        Dim oRequest As New GetHeaderAndSummariesByKeyRequestType
        If Not Page.IsPostBack Then
            Try
                BuildLists(oSAM, ddlPolictStatus, STSListType.PMLookup, "Policy_Status")
                BuildLists(oSAM, ddlAnalysisCode, STSListType.PMLookup, "Analysis_code")
                BuildLists(oSAM, ddlBranchCode, STSListType.PMLookup, "source")
                BuildLists(oSAM, ddlBusinessType, STSListType.PMLookup, "Business_Type")
                BuildLists(oSAM, ddlSubBranch, STSListType.PMLookup, "Sub_Branch")
                BuildLists(oSAM, ddlCurrency, STSListType.PMLookup, "Currency")
                BuildLists(oSAM, ddlFrequencyCode, STSListType.PMLookup, "Renewal_Frequency")
                BuildLists(oSAM, ddlRenewalmethodCode, STSListType.PMLookup, "Renewal_Method")
                BuildLists(oSAM, ddlStopReasoCode, STSListType.PMLookup, "Renewal_stop_code")
                BuildLists(oSAM, ddlLapseCancellation, STSListType.PMLookup, "Lapsed_Reason")
                BuildLists(oSAM, ddlPolicyStyle, STSListType.PMLookup, "Policy_Style")
                ddlStopReasoCode.Items.Insert(0, "")
                ddlLapseCancellation.Items.Insert(0, "")
                ddlRenewalmethodCode.Items.Insert(0, "")
                ddlAnalysisCode.Items.Insert(0, "")
                ddlPolicyStyle.Items.Insert(0, New ListItem("None", ""))

                ddlPolictStatus.SelectedValue = "CUR"
                txtInsuredName.Text = Session("PartyName")

                If Not Session("SelectedPolicy") Is Nothing Then
                    btnLapseQuote.Visible = True
                    btnPolicyTax.Visible = True
                    btnPolicyFee.Visible = True
                    btnCommission.Visible = True

                    oRequest.InsuranceFileKey = Session("InsuranceFileKey")
                    oRequest.BranchCode = "HeadOff" '"HeadOff"
                    StartDate = Date.Now
                    oResponse = oSAM.GetHeaderAndSummariesByKey(oRequest)
                    WriteToLog(Session, "PolicyHeader.aspx", "SAMForInsuranceV2", "GetHeaderAndSummariesByKey",StartDate, Date.Now)
                    Session("AgentName") = txtShortName.Text
                    txtShortName.Text = DirectCast(Session("SelectedPolicy"), BaseGetPartySummaryResponseTypeRow).AgentShortName
                    hfPartyKey.Value = DirectCast(Session("SelectedPolicy"), BaseGetPartySummaryResponseTypeRow).LeadAgentKey
                    'Session("AgentType") = DirectCast(Session("SelectedPolicy"), BaseGetPartySummaryResponseTypeRow).AgentShortName
                    lblPolicyStatus.Text = oResponse.PolicyTypeCode
                    bindColumns(oResponse)
                    Session("LeadAgentKey") = oResponse.LeadAgentKey
                    Session("BranchCode") = oResponse.BranchCode
                    Session("ProductCode") = oResponse.ProductCode
                    Session("TimeStamp") = oResponse.QuoteTimeStamp
                    mvPolicyHeaders.ActiveViewIndex = 0

                    Dim oGetSubAgentsRequest As New GetSubAgentsRequestType
                    Dim oGetSubAgentResponse As New GetSubAgentsResponseType

                    With oGetSubAgentsRequest
                        .InsuranceFileKey = Session("InsuranceFileKey")
                        .BranchCode = "HeadOff"

                    End With
                    StartDate = Date.Now
                    oGetSubAgentResponse = oSAM.GetSubAgents(oGetSubAgentsRequest)
                    WriteToLog(Session, "PolicyHeader.aspx", "SAMForInsuranceV2", "GetSubAgents",StartDate, Date.Now)
                    With oGetSubAgentResponse
                        If Not .Errors Is Nothing Then
                            ''lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
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
                    WriteToLog(Session, "PolicyHeader.aspx", "SAMForInsuranceV2", "GetStandardPolicyWordings",StartDate, Date.Now)
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

                Else

                    btnLapseQuote.Visible = False
                    btnPolicyTax.Visible = False
                    btnPolicyFee.Visible = False
                    btnCommission.Visible = False

                    txtcoveredFrom.Text = Date.Now().Date
                    txtcoveredTo.Text = Date.Now().Add(New System.TimeSpan(365, 0, 0, 0)).Date
                    txtRenewalDate.Text = txtcoveredTo.Text
                    txtinceptionDate.Text = Date.Now().Date
                    txtInceptionTPI.Text = Date.Now().Date
                    txtQuoteexpiry.Text = Date.Now().Add(New System.TimeSpan(20, 0, 0, 0)).Date


                    ddlProduct.Items.Add(DirectCast(Session("SelectedProduct"), ListItem))
                    GetdefaultClauses()
                End If
                If ddlBusinessType.SelectedValue <> "DIRECT" Then
                    btnAgentCode.Enabled = True
                Else
                    btnAgentCode.Enabled = False

                End If
                '' btnAgentCode.Attributes.Add("onCLick", "LoadWindows()")

            Catch ex As Exception

            End Try

        End If
        If Not Session("SubAgents") Is Nothing Then
            gvSubAgents.DataSource = DirectCast(Session("SubAgents"), BaseGetSubAgentsResponseTypeRow())
            gvSubAgents.DataBind()
        End If
        If Not Session("PolicyWording") Is Nothing Then
            gvStandardWording.DataSource = DirectCast(Session("PolicyWording"), DocTemplate())
            gvStandardWording.DataBind()
        End If
        If ddlBusinessType.Items.Count > 0 Then
            If ddlBusinessType.SelectedValue.Trim() = "DIRECT" Then
                txtCoverBook.ReadOnly = True
                txtCoverSheet.ReadOnly = True
            Else
                txtCoverBook.ReadOnly = False
                txtCoverSheet.ReadOnly = False
            End If
        End If
        If ddlBranchCode.Items.Count > 0 Then
            ddlBranchCode.SelectedValue = "HeadOff"
        End If
    End Sub
    Private Sub bindColumns(ByVal oResponse As GetHeaderAndSummariesByKeyResponseType)
        txtInsuredName.Text = oResponse.InsuredName
        txtPolicyNo.Text = oResponse.InsuranceFileRef


        txtcoveredFrom.Text = oResponse.CoverStartDate
        txtcoveredFrom.ReadOnly = True
        txtcoveredTo.Text = oResponse.CoverEndDate.ToString
        If (oResponse.ProposalDate.Year > 1899) Then
        End If
        'txtHCExpiry.Text = oResponse.ProposalDate.ToString()
        If (oResponse.InceptionDate.Year > 1899) Then
            txtinceptionDate.Text = oResponse.InceptionDate.ToString
        Else
            txtinceptionDate.Text = Now.Date
        End If
        txtInceptionTPI.Text = oResponse.InceptionTPI.ToString
        If (oResponse.IssueDate.Year > 1899) Then
            txtIssuedDate.Text = oResponse.IssueDate.ToString

        End If
        If (oResponse.QuoteExpiryDate.Year > 1899) Then
            txtQuoteexpiry.Text = oResponse.QuoteExpiryDate.ToString
        Else
            txtQuoteexpiry.Text = Now.Date
        End If
        If (IsDate(oResponse.RenewalDate)) Then
            txtRenewalDate.Text = oResponse.RenewalDate.ToString
        Else
            txtRenewalDate.Text = Now.Date
        End If


        ddlProduct.Items.Insert(0, New ListItem(oResponse.ProductName, oResponse.ProductCode))
        Session("SelectedProduct") = ddlProduct.Items(0)
        txtInsuredName.Text = oResponse.InsuredName
        txtRegarding.Text = oResponse.Regarding
        txtAlternateRef.Text = oResponse.AlternativeRef

        ddlBusinessType.SelectedIndex = ddlBusinessType.Items.IndexOf(ddlBusinessType.Items.FindByValue(oResponse.BusinessTypeCode.Trim))
        ddlCurrency.SelectedIndex = ddlCurrency.Items.IndexOf(ddlCurrency.Items.FindByValue(oResponse.CurrencyCode.Trim))
        ddlAnalysisCode.SelectedIndex = ddlAnalysisCode.Items.IndexOf(ddlAnalysisCode.Items.FindByValue(oResponse.AnalysisCode.Trim))
        ddlBranchCode.SelectedIndex = ddlBranchCode.Items.IndexOf(ddlBranchCode.Items.FindByValue(oResponse.BranchCode.Trim))
        ddlSubBranch.SelectedIndex = ddlSubBranch.Items.IndexOf(ddlSubBranch.Items.FindByValue(oResponse.SubBranchCode.Trim))
        If oResponse.PolicyStatusCode = "" Then
            ddlPolictStatus.SelectedValue = "CUR"
            txtStatus.Text = "Current"
        Else
            ddlPolictStatus.SelectedValue = oResponse.PolicyStatusCode.Trim
            txtStatus.Text = oResponse.PolicyStatusCode
        End If

        ddlFrequencyCode.SelectedIndex = ddlFrequencyCode.Items.IndexOf(ddlFrequencyCode.Items.FindByValue(oResponse.RenewalFrequencyCode))
        ddlSubBranch.SelectedValue = oResponse.SubBranchCode
    End Sub
    Private Sub BuildLists(ByVal oSAM As SAMForInsuranceV2, ByRef objControl As DropDownList, ByVal ESTSLookup As STSListType, ByVal ListCode As String)
        Dim oRequest As New GetListRequestType
        Dim oResponse As New GetListResponseType


        oRequest.BranchCode = "HeadOff"
        oRequest.ListType = STSListType.PMLookup
        oRequest.ListCode = ListCode


        Try
            
            StartDate = Date.Now
            oResponse = oSAM.GetList(oRequest)
            WriteToLog(Session, "PolicyHeader.aspx", "SAMForInsuranceV2", "GetList",StartDate, Date.Now)
            With oResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    ''lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                Else

                    objControl.DataSource = oResponse.List
                    objControl.DataTextField = "Description"
                    objControl.DataValueField = "Code"
                    objControl.DataBind()
                End If
                If ListCode = "source" Then
                    Session("Branch") = oResponse.List
                End If
            End With

        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            'Response.Write("An error occured calling SAM:<br>" & os.Message)

        Catch oe As Exception
            'should do some error handling here. Just output error for now
            'Response.Write("An error occured:<br>" & oe.Message)

        Finally
            'clean up any objects here
        End Try

    End Sub



    Protected Sub cmdOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdOk.Click
        If ddlBusinessType.SelectedItem.Text = "Agency Business" And txtShortName.Text = String.Empty Then
            lblAgentKey.Visible = True
            Exit Sub
        End If
        If Cancel.Value = "CANCELLED" Then
            Cancel.Value = ""
            Exit Sub
        End If
        System.AppDomain.CurrentDomain.SetData("BUSINESSTYPE", ddlBusinessType.SelectedItem.Text)
        Session("AgentName") = txtShortName.Text
        If Session("SelectedPolicy") Is Nothing Then

            Dim oAddQuoteRequestType As New AddQuoteV2RequestType
            Dim oAddQuoteResponseType As New AddQuoteV2ResponseType


            With oAddQuoteRequestType

                If hfPartyKey.Value <> "" Then
                    .AgentKey = Int32.Parse(hfPartyKey.Value)
                    Session("AgentKey") = .AgentKey
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
                If txtCoverSheet.Text <> "" Then
                    .CoverNoteSheetNumberSpecified = False
                    .CoverNoteSheetNumber = Convert.ToInt32(txtCoverSheet.Text)
                Else
                    .CoverNoteSheetNumberSpecified = False
                End If

                .CoverStartDate = txtcoveredFrom.Text
                .CurrencyCode = ddlCurrency.SelectedValue
                .Description = "Quote Created"
                .InsuredName = txtInsuredName.Text
                .PartyKey = Session("PartyKey")
                .ProductCode = ddlProduct.SelectedValue
                .QuoteRef = txtPolicyNo.Text
                .SubBranchCode = "HeadOff"
                .CoverNoteBookNumber = txtCoverBook.Text
                .FrequencyCode = ddlFrequencyCode.SelectedValue
                .HandlerCode = hfHandlerCode.Value
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

                .RenewalCount = 0
                .RenewalCountSpecified = False

                .RenewalDate = txtRenewalDate.Text
                .RenewalMethodCode = ddlRenewalmethodCode.SelectedValue
                .StopReasonCode = ddlStopReasoCode.SelectedValue


            End With
            Try
                StartDate = Date.Now
                oAddQuoteResponseType = oSAM.AddQuoteV2(oAddQuoteRequestType)
                WriteToLog(Session, "PolicyHeader.aspx", "SAMForInsuranceV2", "AddQuoteV2", StartDate, Date.Now)

                With oAddQuoteResponseType

                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                    Else

                        Session("InsuranceFileKey") = .InsuranceFileKey
                        Session("InsuranceFolderKey") = .InsuranceFolderKey
                        Session("TimeStamp") = oAddQuoteResponseType.QuoteTimeStamp
                        UpdateSubagentsAndPolcyWordings()
                        'JP 18/03/2010
                        Session("AddUpdateQuoteRequest") = oAddQuoteRequestType
                        If ddlBusinessType.SelectedValue.Trim = "COIN LEAD" Then

                            Response.Redirect("Coinsurance.aspx")
                        Else
                            Response.Redirect("GetListRisks_Risk.aspx")
                        End If


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
        Else
            Dim oUpdateQuoteV2RequestType As New UpdateQuoteV2RequestType
            Dim oUpdateQuoteV2ResponseType As New UpdateQuoteV2ResponseType

            With oUpdateQuoteV2RequestType
                .InsuranceFileKey = Session("InsuranceFileKey")
                .InsuranceFolderKey = Session("InsuranceFolderKey")
                If hfPartyKey.Value <> "" Then
                    .AgentKey = Int32.Parse(hfPartyKey.Value)
                    Session("AgentKey") = .AgentKey
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
                .HandlerCode = hfHandlerCode.Value
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
                .Timestamp = Session("TimeStamp")


            End With
            Try
                StartDate = Date.Now
                oUpdateQuoteV2ResponseType = oSAM.UpdateQuoteV2(oUpdateQuoteV2RequestType)
                WriteToLog(Session, "PolicyHeader.aspx", "SAMForInsuranceV2", "UpdateQuoteV2", StartDate, Date.Now)

                With oUpdateQuoteV2ResponseType

                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)

                    Else
                        Session("TimeStamp") = .TimeStamp
                        UpdateSubagentsAndPolcyWordings()
                        'JP 18/03/2010
                        Session("AddUpdateQuoteRequest") = oUpdateQuoteV2RequestType
                        If ddlBusinessType.SelectedValue.Trim = "COIN LEAD" Then

                            Response.Redirect("Coinsurance.aspx")
                        Else
                            Response.Redirect("GetListRisks_Risk.aspx")
                        End If
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


    End Sub

    Protected Sub ddlBranchCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlBranchCode.SelectedIndexChanged

        Dim oGetListResponseTypeRow() As BaseGetListResponseTypeRow
        oGetListResponseTypeRow = DirectCast(Session("Branch"), BaseGetListResponseTypeRow())

        Dim BranchId As Integer = 0
        For BranchCount As Integer = 0 To oGetListResponseTypeRow.Length - 1
            If oGetListResponseTypeRow(BranchCount).Code = ddlBranchCode.SelectedValue Then
                BranchId = oGetListResponseTypeRow(BranchCount).Key
            End If
        Next

        ' ddlSubBranch.Items.Clear()


        Dim oRequest As New GetListRequestType
        Dim oResponse As New GetListResponseType


        oRequest.BranchCode = "HeadOff"
        oRequest.ListType = STSListType.PMLookup
        oRequest.ListCode = "Sub_Branch"


        Try
            StartDate = Date.Now
            oResponse = oSAM.GetList(oRequest)
            WriteToLog(Session, "PolicyHeader.aspx", "SAMForInsuranceV2", "GetList",StartDate, Date.Now)
            With oResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    ''lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                Else
                    For SubBranchCount As Integer = 0 To .List.Length - 1
                        If .List(SubBranchCount).ParentKey = BranchId Then
                            Dim item As New ListItem
                            item.Text = .List(SubBranchCount).Description
                            item.Value = .List(SubBranchCount).Code
                            ddlSubBranch.Items.Add(item)
                        End If
                    Next
                End If
            End With

        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            'Response.Write("An error occured calling SAM:<br>" & os.Message)

        Catch oe As Exception
            'should do some error handling here. Just output error for now
            'Response.Write("An error occured:<br>" & oe.Message)

        Finally
            'clean up any objects here
        End Try




    End Sub

    Protected Sub ddlBusinessType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlBusinessType.SelectedIndexChanged
        If ddlBusinessType.SelectedValue <> "DIRECT" Then
            btnAgentCode.Enabled = True
            txtShortName.Enabled = True
            btnCommission.Enabled = True
        Else
            btnAgentCode.Enabled = False
            txtShortName.Text = ""
            txtShortName.Enabled = False
            btnCommission.Enabled = False


        End If
        If ddlBusinessType.SelectedItem.Text = "Co-Insurance Lead" Then
            btnCoinsurers.Enabled = True
        End If
    End Sub


    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Session("AgentSearchType") = "SubAgent"
        Response.Write("<script>window.open('FindAgent.aspx','_new');</script>")


    End Sub

    Protected Sub gvSubAgents_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSubAgents.SelectedIndexChanged

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

    Protected Sub btnAgentCode_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAgentCode.Click
        Session("AgentSearchType") = "Agent"
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
                        ''lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                    Else
                        Session("TimeStamp") = .TimeStamp
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
                'Response.Write("An error occured calling SAM:<br>" & os.Message)
            Catch oe As Exception
                'should do some error handling here. Just output error for now
                'Response.Write("An error occured:<br>" & oe.Message)
            Finally
                'clean


            End Try
        End If

    End Sub


    Protected Sub btnCoinsurers_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCoinsurers.Click
        Response.Write("<script>window.open('CoInsurance.aspx');</script>")
    End Sub
    Protected Sub ddlCurrency_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurrency.SelectedIndexChanged
        cmdOk.Attributes.Add("OnClick", "Showmodalpopup()")
        Session("TRANSACTIONCURRENCYCODE") = ddlCurrency.SelectedValue
    End Sub

   
    Protected Sub txtShortName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtShortName.TextChanged
        lblAgentKey.Visible = False
    End Sub
    Protected Sub txtcoveredFrom_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtcoveredFrom.TextChanged
        'txtcoveredFrom.Text = 
        Dim dt As Date
        dt = Convert.ToDateTime(txtcoveredFrom.Text)

        'txtcoveredTo.Text = Date.Now().Add(New System.TimeSpan(365, 0, 0, 0)).Date
        txtcoveredTo.Text = dt.Add(New System.TimeSpan(365, 0, 0, 0)).Date
        txtRenewalDate.Text = txtcoveredTo.Text
        'txtinceptionDate.Text = dt.Date
        txtInceptionTPI.Text = dt.Date
        txtQuoteexpiry.Text = dt.Add(New System.TimeSpan(20, 0, 0, 0)).Date
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
        'JP StartDate = Date.Now
        'JP oResponse = oSAM.GetProductOrRiskClauses(oRequest)
        'JP WriteToLog(Session, "PolicyHeader.aspx", "SAMForInsuranceV2", "GetProductOrRiskClauses", StartDate, Date.Now)

        'JP With oResponse
        'JP If .Errors IsNot Nothing Then
        'JP lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
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

