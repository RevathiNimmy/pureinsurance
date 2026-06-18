Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2

Partial Class New_Business_PrePayment
    Inherits System.Web.UI.Page
    Dim StartDate As Date
    Dim oSAM As New SAMForInsuranceV2

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        If Session("POPUP") = "SHOW" Then
            btnMakeLive.Attributes.Add("OnClick", "Showmodalpopup()")
        End If
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        If Session("AgentType") = Nothing Then
            rblType.Items(0).Enabled = False
            rblType.Items(1).Enabled = True
            rblType.Items(1).Selected = True
        Else
            If Session("AgentType") = "Intermediary" Then
                rblType.Items(0).Enabled = True
                rblType.Items(1).Enabled = True
            ElseIf (Session("AgentType") = "Broker") Then
                rblType.Items(0).Enabled = True
                rblType.Items(0).Selected = True
                rblType.Items(1).Enabled = False
            Else
                rblType.Items(0).Enabled = False
                rblType.Items(1).Enabled = True
                rblType.Items(1).Selected = True

            End If

        End If


        If Not Page.IsPostBack Then
            Dim oGetBalancesAndUnallocatedCreditsRequest As New GetBalancesAndUnallocatedCreditsRequestType
            Dim oGetBalancesAndUnallocatedCreditsResponse As New GetBalancesAndUnallocatedCreditsResponseType

            Dim totalDue As Decimal
            totalDue = Session("Amount")

            lblTotalDue.Text = totalDue.ToString

            With oGetBalancesAndUnallocatedCreditsRequest
                .BranchCode = "HeadOff"
                .InsuranceFileKey = Session("InsuranceFileKey")
            End With
            Try
                StartDate = Date.Now
                oGetBalancesAndUnallocatedCreditsResponse = oSAM.GetBalancesAndUnallocatedCredits(oGetBalancesAndUnallocatedCreditsRequest)
                WriteToLog(Session, "PrePayment.aspx", "SAMForInsuranceV2", "GetBalancesAndUnallocatedCredits",StartDate, Date.Now)
                With oGetBalancesAndUnallocatedCreditsResponse
                    If .Errors IsNot Nothing Then
                        'errors returned, so throw an exception
                        lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                    Else
                        lblPolicyRef.Text = .InsuranceRef


                        txtFloatBalance.Text = .FloatBalanceLimit.ToString
                        txtOverDraft.Text = .OverDraftLimit.ToString
                        txtAccount.Text = .AccountBalance.ToString
                        Session("UnallocatedCreditsForAgents") = .UnallocatedCreditsForAgents
                        Session("UnallocatedCreditsForClients") = .UnallocatedCreditsForClients

                        If Session("AgentType") IsNot Nothing Then
                            gvPrePayment.DataSource = .UnallocatedCreditsForAgents
                            gvPrePayment.DataBind()
                        Else
                            gvPrePayment.DataSource = .UnallocatedCreditsForClients
                            gvPrePayment.DataBind()
                        End If

                        If .IsFloatBalanceAccount Then
                            rblDebitAgainst.Items(1).Enabled = True
                            txtFloatBalance.Enabled = True
                        Else
                            rblDebitAgainst.Items(1).Enabled = False
                            txtFloatBalance.Enabled = False
                        End If

                        If .IsOverDraftAccount Then
                            rblDebitAgainst.Items(0).Enabled = True
                            txtOverDraft.Enabled = True
                        Else
                            rblDebitAgainst.Items(0).Enabled = False
                            txtOverDraft.Enabled = False
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
        End If




    End Sub

    Protected Sub btnMakeLive_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMakeLive.Click

        If rblDebitAgainst.SelectedItem.Text = "Account" Then
            If Math.Abs(CDbl(txtAccount.Text)) < CDbl(lblTotalDue.Text) Then
                Response.Write("<script> alert('Sufficient amount balance in not available in your account')</script>")
                Exit Sub
            End If
        ElseIf rblDebitAgainst.SelectedItem.Text = "UnAllocated" Then
            If Math.Abs(CDbl(txtUnAllocatedCredit.Text)) < CDbl(lblTotalDue.Text) Then
                Response.Write("<script> alert('Sufficient amount balance in not available in your UnAllocatedCredit')</script>")
                Exit Sub
            End If
        End If

        If Cancel.Value = "CANCELLED" Then
            Cancel.Value = ""
            Exit Sub
        End If
        Dim oBaseGetBalancesAndUnallocatedCreditsResponseTypeRow() As BaseGetBalancesAndUnallocatedCreditsResponseTypeRow
        oBaseGetBalancesAndUnallocatedCreditsResponseTypeRow = DirectCast(Session("UnallocatedCreditsForAgents"), BaseGetBalancesAndUnallocatedCreditsResponseTypeRow())

        Dim oBaseGetBalancesAndUnallocatedCreditsResponseTypeRow1() As BaseGetBalancesAndUnallocatedCreditsResponseTypeRow1
        oBaseGetBalancesAndUnallocatedCreditsResponseTypeRow1 = DirectCast(Session("UnallocatedCreditsForClients"), BaseGetBalancesAndUnallocatedCreditsResponseTypeRow1())

        Dim oBaseBindQuoteRequestTypeCreditTransactionsRow() As BaseBindQuoteRequestTypeCreditTransactionsRow




        Dim total As Double
        total = Session("Amount")
        For Count As Integer = 0 To gvPrePayment.Rows.Count - 1

            If gvPrePayment.Rows(Count).RowType = DataControlRowType.DataRow Then

                If DirectCast(gvPrePayment.Rows(Count).Controls(0).Controls(1), CheckBox).Checked Then
                    If oBaseBindQuoteRequestTypeCreditTransactionsRow Is Nothing Then
                        ReDim Preserve oBaseBindQuoteRequestTypeCreditTransactionsRow(0)
                    Else
                        ReDim Preserve oBaseBindQuoteRequestTypeCreditTransactionsRow(oBaseBindQuoteRequestTypeCreditTransactionsRow.Length)
                    End If

                    If rblType.SelectedValue = "Agent" Then

                        oBaseBindQuoteRequestTypeCreditTransactionsRow(oBaseBindQuoteRequestTypeCreditTransactionsRow.Length - 1) = New BaseBindQuoteRequestTypeCreditTransactionsRow
                        oBaseBindQuoteRequestTypeCreditTransactionsRow(oBaseBindQuoteRequestTypeCreditTransactionsRow.Length - 1).AccountKey = oBaseGetBalancesAndUnallocatedCreditsResponseTypeRow(Count).AccountKey
                        If (total + oBaseGetBalancesAndUnallocatedCreditsResponseTypeRow(Count).Amount) > 0 Then
                            total = total + oBaseGetBalancesAndUnallocatedCreditsResponseTypeRow(Count).Amount
                            oBaseBindQuoteRequestTypeCreditTransactionsRow(oBaseBindQuoteRequestTypeCreditTransactionsRow.Length - 1).Amount = Math.Abs(oBaseGetBalancesAndUnallocatedCreditsResponseTypeRow(Count).Amount)
                        Else

                            oBaseBindQuoteRequestTypeCreditTransactionsRow(oBaseBindQuoteRequestTypeCreditTransactionsRow.Length - 1).Amount = total
                            total = 0
                        End If

                        oBaseBindQuoteRequestTypeCreditTransactionsRow(oBaseBindQuoteRequestTypeCreditTransactionsRow.Length - 1).TransDetailKey = oBaseGetBalancesAndUnallocatedCreditsResponseTypeRow(Count).TransDetailKey
                        If oBaseGetBalancesAndUnallocatedCreditsResponseTypeRow(Count).CollectionDateSpecified Then
                            oBaseBindQuoteRequestTypeCreditTransactionsRow(oBaseBindQuoteRequestTypeCreditTransactionsRow.Length - 1).CollectionDate = oBaseGetBalancesAndUnallocatedCreditsResponseTypeRow(Count).CollectionDate
                        End If


                    Else
                        oBaseBindQuoteRequestTypeCreditTransactionsRow(oBaseBindQuoteRequestTypeCreditTransactionsRow.Length - 1) = New BaseBindQuoteRequestTypeCreditTransactionsRow
                        oBaseBindQuoteRequestTypeCreditTransactionsRow(oBaseBindQuoteRequestTypeCreditTransactionsRow.Length - 1).AccountKey = oBaseGetBalancesAndUnallocatedCreditsResponseTypeRow1(Count).AccountKey
                        If (total + oBaseGetBalancesAndUnallocatedCreditsResponseTypeRow1(Count).Amount) > 0 Then
                            total = total + oBaseGetBalancesAndUnallocatedCreditsResponseTypeRow1(Count).Amount
                            oBaseBindQuoteRequestTypeCreditTransactionsRow(oBaseBindQuoteRequestTypeCreditTransactionsRow.Length - 1).Amount = Math.Abs(oBaseGetBalancesAndUnallocatedCreditsResponseTypeRow1(Count).Amount)
                        Else

                            oBaseBindQuoteRequestTypeCreditTransactionsRow(oBaseBindQuoteRequestTypeCreditTransactionsRow.Length - 1).Amount = total
                            total = 0
                        End If

                        oBaseBindQuoteRequestTypeCreditTransactionsRow(oBaseBindQuoteRequestTypeCreditTransactionsRow.Length - 1).TransDetailKey = oBaseGetBalancesAndUnallocatedCreditsResponseTypeRow1(Count).TransDetailKey
                        If oBaseGetBalancesAndUnallocatedCreditsResponseTypeRow1(Count).CollectionDateSpecified Then
                            oBaseBindQuoteRequestTypeCreditTransactionsRow(oBaseBindQuoteRequestTypeCreditTransactionsRow.Length - 1).CollectionDate = oBaseGetBalancesAndUnallocatedCreditsResponseTypeRow1(Count).CollectionDate
                        End If
                    End If

                End If
            End If
        Next
        Dim oBindQuoteRequest As New BindQuoteRequestType
        Dim oBindQuoteResponse As New BindQuoteResponseType
        With oBindQuoteRequest
            .BranchCode = "HeadOff"
            .CreditTransactions = oBaseBindQuoteRequestTypeCreditTransactionsRow

            If rblDebitAgainst.SelectedValue = "Account" Then
                .DebitAgainstSpecified = False
            Else
                If rblDebitAgainst.SelectedValue <> "" Then
                    .DebitAgainst = Convert.ToInt32(rblDebitAgainst.SelectedValue)
                    .DebitAgainstSpecified = True
                End If

            End If


            .InsuranceFileKey = Session("InsuranceFileKey")
            .PaymentMethodSpecified = False
            .TransactionType = "NB"


        End With

        Try
            StartDate = Date.Now
            oBindQuoteResponse = oSAM.BindQuote(oBindQuoteRequest)
            WriteToLog(Session, "PrePayment.aspx", "SAMForInsuranceV2", "BindQuote",StartDate, Date.Now)
            With oBindQuoteResponse

                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                Else
                    lblPolicyNum.Text = .Policy.PolicyRef
                    lblPolicyNum.Visible = True
                    Response.Redirect("../UIIC_demo/HomePage.aspx?id=" + .Policy.PolicyRef + "&name=Policy")
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

   
    Protected Sub rblType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblType.SelectedIndexChanged

        Dim oBaseGetBalancesAndUnallocatedCreditsResponseTypeRow() As BaseGetBalancesAndUnallocatedCreditsResponseTypeRow
        oBaseGetBalancesAndUnallocatedCreditsResponseTypeRow = DirectCast(Session("UnallocatedCreditsForAgents"), BaseGetBalancesAndUnallocatedCreditsResponseTypeRow())

        Dim oBaseGetBalancesAndUnallocatedCreditsResponseTypeRow1() As BaseGetBalancesAndUnallocatedCreditsResponseTypeRow1
        oBaseGetBalancesAndUnallocatedCreditsResponseTypeRow1 = DirectCast(Session("UnallocatedCreditsForClients"), BaseGetBalancesAndUnallocatedCreditsResponseTypeRow1())

        If rblType.SelectedValue = "Agent" Then
            gvPrePayment.DataSource = oBaseGetBalancesAndUnallocatedCreditsResponseTypeRow
            gvPrePayment.DataBind()

        Else
            gvPrePayment.DataSource = oBaseGetBalancesAndUnallocatedCreditsResponseTypeRow1
            gvPrePayment.DataBind()
        End If

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("GetListRisks_Risk.aspx")
    End Sub

    Protected Sub gvPrePayment_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPrePayment.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim chk As New CheckBox
            chk = DirectCast(e.Row.FindControl("CheckBox1"), CheckBox)
            chk.ToolTip = e.Row.RowIndex

        End If
    End Sub






    Protected Sub Checkboxchanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'Dim chk As New CheckBox
        'chk = DirectCast(sender, CheckBox)
        'Dim index As Integer = Convert.ToInt32(chk.ToolTip)

        'If rblDebitAgainst.SelectedItem.Text = "UnAllocated" Then
        '    For i As Integer = 0 To gvPrePayment.Rows.Count - 1
        '        'If index = i Then

        '        '    txtUnAllocatedCredit.Text = ""

        '        'End If
        '        If chk.Checked Then

        '        End If
        '    Next
        'End If


        If rblDebitAgainst.SelectedItem IsNot Nothing Then 'JP 04/03/2010
            If rblDebitAgainst.SelectedItem.Text = "UnAllocated" AndAlso gvPrePayment.Rows.Count > 0 Then
                Dim UnallocatedAmt As Integer = 0
                For i As Integer = 0 To gvPrePayment.Rows.Count - 1
                    Dim chk As New CheckBox
                    chk = DirectCast(gvPrePayment.Rows(i).FindControl("CheckBox1"), CheckBox)
                    If chk.Checked Then
                        UnallocatedAmt += Convert.ToDouble(gvPrePayment.Rows(i).Cells(2).Text)
                    End If
                Next

                txtUnAllocatedCredit.Text = UnallocatedAmt
            End If
        End If
    End Sub

End Class
