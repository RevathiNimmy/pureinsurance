Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2

Partial Class MTA_PolicyHeader
    Inherits System.Web.UI.Page

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
                oRequest.InsuranceFileKey = 2589 'Session("INSURANCEFILEKEY") 2447
                oRequest.BranchCode = "HeadOff" 'Session("BRANCHCODE") 
                oResponse = oSAM.GetHeaderAndSummariesByKey(oRequest)

                bindColumns(oResponse)
                txtInsuredName.Text = oResponse.InsuredName
                'BuildLists(oSAM, ddlPolictStatus, STSListType.PMLookup, "Policy_Status")
                'BuildLists(oSAM, ddlAnalysisCode, STSListType.PMLookup, "Analysis_code")
                'BuildLists(oSAM, ddlBranchCode, STSListType.PMLookup, "source")
                'BuildLists(oSAM, ddlBusinessType, STSListType.PMLookup, "Business_Type")
                'BuildLists(oSAM, ddlCurrency, STSListType.PMLookup, "Currency")
                'BuildLists(oSAM, ddlSubBranch, STSListType.PMLookup, "Sub_Branch")
                'BuildLists(oSAM, ddlFrequencyCode, STSListType.PMLookup, "Renewal_Frequency")
                'BuildLists(oSAM, ddlRenewalmethodCode, STSListType.PMLookup, "Renewal_Method")
                'BuildLists(oSAM, ddlStopReasoCode, STSListType.PMLookup, "Renewal_stop_code")
                'BuildLists(oSAM, ddlLapseCancellation, STSListType.PMLookup, "Lapsed_Reason")
                BuildLists(oSAM, ddlPolictStatus, STSListType.PMLookup, "Policy_Status", oResponse.PolicyStatusCode)
                BuildLists(oSAM, ddlAnalysisCode, STSListType.PMLookup, "Analysis_code", oResponse.AnalysisCode)
                BuildLists(oSAM, ddlBranchCode, STSListType.PMLookup, "source", oResponse.BranchCode)
                BuildLists(oSAM, ddlBusinessType, STSListType.PMLookup, "Business_Type", oResponse.BusinessTypeCode)
                BuildLists(oSAM, ddlCurrency, STSListType.PMLookup, "Currency", oResponse.CurrencyCode)
                BuildLists(oSAM, ddlSubBranch, STSListType.PMLookup, "Sub_Branch", oResponse.SubBranchCode)
                BuildLists(oSAM, ddlFrequencyCode, STSListType.PMLookup, "Renewal_Frequency", oResponse.RenewalFrequencyCode)
                BuildLists(oSAM, ddlRenewalmethodCode, STSListType.PMLookup, "Renewal_Method", oResponse.RenewalMethodCode)
                BuildLists(oSAM, ddlStopReasoCode, STSListType.PMLookup, "Renewal_stop_code", oResponse.StopReasonCode)
                BuildLists(oSAM, ddlLapseCancellation, STSListType.PMLookup, "Lapsed_Reason", oResponse.LapsedReasonCode)





                Session("InsuranceFolderKey") = oResponse.InsuranceFolderKey
                Session("InsuranceFileKey") = oResponse.InsuranceFileKey
                Session("QuoteTimeStamp") = oResponse.QuoteTimeStamp
                mvPolicyHeaders.ActiveViewIndex = 0
            Catch ex As Exception

            End Try

        End If

    End Sub
    Private Sub bindColumns(ByVal oResponse As GetHeaderAndSummariesByKeyResponseType)
        txtInsuredName.Text = oResponse.InsuredName
        txtPolicyNo.Text = oResponse.InsuranceFileRef
        txtProduct.Text = oResponse.ProductName
        txtStatus.Text = oResponse.PolicyStatusCode
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
        txtcoveredTo.Text = oResponse.CoverEndDate.ToString
        'txtHCExpiry.Text =oResponse.Risks
        If (oResponse.InceptionDate.Year > 1773) Then
            txtinceptionDate.Text = oResponse.InceptionDate.ToString
        Else
            txtinceptionDate.Text = Now.Date
        End If
        txtInceptionTPI.Text = oResponse.InceptionTPI.ToString
        If (oResponse.IssueDate.Year > 1773) Then
            txtIssuedDate.Text = oResponse.IssueDate.ToString
        Else
            txtIssuedDate.Text = Now.Date
        End If
        If (oResponse.QuoteExpiryDate.Year > 1773) Then
            txtQuoteexpiry.Text = oResponse.QuoteExpiryDate.ToString
        Else
            txtQuoteexpiry.Text = Now.Date
        End If
        If (IsDate(oResponse.RenewalDate)) Then
            txtRenewalDate.Text = oResponse.RenewalDate.ToString
        Else
            txtRenewalDate.Text = Now.Date
        End If

        txtInsuredName.Text = oResponse.InsuredName
        txtRegarding.Text = oResponse.Regarding
        txtAlternateRef.Text = oResponse.AlternativeRef
        txtstdPolicyDesc.Text = oResponse.StandardPolicyDescription
        txtStPolicyWording.Text = oResponse.StandardPolicyWordingCode

    End Sub
    Private Sub BuildLists(ByVal oSAM As SAMForInsuranceV2, ByRef objControl As DropDownList, ByVal ESTSLookup As STSListType, ByVal ListCode As String, ByVal BindValue As String)
        Dim oRequest As New GetListRequestType
        Dim oResponse As New GetListResponseType


        oRequest.BranchCode = "HeadOff"
        oRequest.ListType = STSListType.PMLookup
        oRequest.ListCode = ListCode


        Try
            oResponse = oSAM.GetList(oRequest)

            With oResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
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

        If (Session("PROCESS").ToString = "MTA") Then
            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
            Dim oSAM As New SAMForInsuranceV2
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")

            Dim oRequest As New AddMtaQuoteRequestType
            Dim oResponse As New AddMtaQuoteResponseType

            With oRequest

                .AlternateReference = txtAlternateRef.Text
                .AnalysisCode = ddlAnalysisCode.Text
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
                Else
                    .IssueDate = Nothing
                End If
                .IssueDateSpecified = True
                .LapseCancelDate = Now.Date
                .LapseCancelDateSpecified = True

                If (IsDate(txtHCExpiry.Text)) Then
                    .LTUExpiryDate = CDate(txtHCExpiry.Text)
                Else
                    .LTUExpiryDate = Now.Date
                End If
                .LTUExpiryDateSpecified = True
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
                .PolicyStyleCode = "SIMPLE" ''' HARD-CODED AS IF NOW
                .RenewalMethodCode = ddlRenewalmethodCode.Text
                .StopReasonCode = ddlStopReasoCode.Text
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
                .LapseCancelReasonCode = ddlLapseCancellation.Text

                .TypeOfMta = Session("MTAISPERMANENT").ToString 'check with rahul

            End With
            'oResponse = oSAM.AddMtaQuote(oRequest)
        Else
            UpDateQuote()
        End If
        Session("AlternateRef") = txtAlternateRef.Text
        Session("AnalysisCode") = ddlAnalysisCode.Text

        Response.Redirect("GetListRisks-Risk.aspx")
    End Sub
    Private Sub UpDateQuote()
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oRequest As New UpdateQuoteRequestType
        Dim oResponse As New UpdateQuoteResponseType

        With oRequest
            .AlternativeRef = txtAlternateRef.Text
            .AnalysisCode = ddlAnalysisCode.Text
            .BranchCode = Session("BranchCode")
            '.ConsolidatedLeadAgentCommission=
            '.ConsolidatedLeadAgentCommissionSpecified
            '.ConsolidatedSubAgentCommission
            '.ConsolidatedSubAgentCommissionSpecified
            If (IsDate(txtcoveredTo.Text)) Then
                .CoverEndDate = txtcoveredTo.Text
            End If
            .CoverNoteBookNumber = txtCoverBook.Text
            If (IsNothing(txtCoverSheet.Text)) Then
                .CoverNoteSheetNumber = Convert.ToInt32(txtCoverSheet.Text)
            End If
            .CoverNoteSheetNumberSpecified = True
            If (IsDate(txtcoveredFrom.Text)) Then
                .CoverStartDate = CDate(txtcoveredFrom.Text)
            End If
            .CurrencyCode = ddlCurrency.SelectedValue
            .Description = "OTHER"
            .InsuranceFileKey = Convert.ToInt32(Session("InsuranceFileKey"))
            .InsuranceFolderKey = Convert.ToInt32(Session("InsuranceFolderKey"))
            '.InsuredParties
            .QuoteTimeStamp = Session("QuoteTimeStamp")
        End With
        oResponse = oSAM.UpdateQuote(oRequest)
        Session("QuoteTimeStamp") = oResponse.QuoteTimeStamp
    End Sub
End Class
