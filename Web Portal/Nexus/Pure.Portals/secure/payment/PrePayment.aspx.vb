Imports CMS.library
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports Nexus.Utils
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session


Namespace Nexus

    Partial Class secure_payment_PrePayment : Inherits BasePayment

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not IsPostBack Then

                If Session(CNPaid) = True Then
                    SetPaymentTakenAndRedirect()
                End If

                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)


                'Getting Installment Quotes
                Try

                    Dim dPremiumGross As Decimal
                    If oQuote IsNot Nothing And Session(CNTotalForQuoteCollection) Is Nothing Then
                        If oQuote.Risks.Count > 0 Then
                            'Check if Agent is Broker then Agent Commission should be deducted from Total AMount
                            If Session(CNLoginType) = LoginType.Agent Then
                                Dim bFound As Boolean = False

                                If Session(CNAgentType) IsNot Nothing And Session(CNAgentComm) IsNot Nothing Then
                                    If Session(CNAgentType).ToString.Trim.ToUpper = "BROKER" Then
                                        Dim dAgentComm As Decimal = Session(CNAgentComm)
                                        dPremiumGross = CheckAndCalculateRoundOff()
                                        dPremiumGross = dPremiumGross - dAgentComm
                                        Session.Add(CNAmountToPay, dPremiumGross)
                                        bFound = True
                                    End If
                                Else
                                    'Find The AgentType through SAM Call
                                    Dim oTempParty As NexusProvider.PartyCollection
                                    Dim oTempSearchCriteria As New NexusProvider.PartySearchCriteria

                                    oTempSearchCriteria.AgentType = Nothing
                                    oTempSearchCriteria.ShortName = CType(Session(CNQuote), NexusProvider.Quote).AgentCode
                                    oTempSearchCriteria.PartyType = CType(Session(CNAgentDetails), NexusProvider.UserDetails).PartyType
                                    oTempSearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.AG)

                                    oTempParty = oWebService.FindParty(oTempSearchCriteria)

                                    If oTempParty IsNot Nothing Then
                                        If oTempParty.Count > 0 Then
                                            Session(CNAgentType) = oTempParty(0).AgentType
                                            'Check if Agent is Broker then Agent Commission should be deducted from Total AMount
                                            If Session(CNAgentType).ToString.Trim.ToUpper = "BROKER" Then
                                                Dim dAgentComm As Decimal = Session(CNAgentComm)
                                                dPremiumGross = CheckAndCalculateRoundOff()
                                                dPremiumGross = dPremiumGross - dAgentComm
                                                Session.Add(CNAmountToPay, dPremiumGross)
                                                bFound = True
                                            End If
                                        End If
                                    End If
                                End If
                                'if bFound is False it means that Agnet is Not Broker so that Full AMount will move further
                                If bFound = False Then
                                    dPremiumGross = CheckAndCalculateRoundOff()
                                    Session.Add(CNAmountToPay, dPremiumGross)
                                End If
                            End If
                            'End

                        End If
                    ElseIf Session(CNTotalForQuoteCollection) IsNot Nothing Then
                        'Quote Collection
                        dPremiumGross = Session(CNTotalForQuoteCollection)
                    End If

                    txtTotalDue.Text = dPremiumGross 'need this hidden text to validate in Live Process
                    litTotalDueheading.Text = New Money(dPremiumGross, Session(CNCurrenyCode)).Formatted

                    Dim oBalancesAndUnallocatedCredits As NexusProvider.BalancesAndUnallocatedCredits
                    Dim oUnallocatedCreditCollection As NexusProvider.UnallocatedCreditCollection
                    ' Code  For Quote Collection
                    If Session(CNQuoteCollectionFiles) IsNot Nothing Then
                        Dim arrQuoteCollectionFiles As New ArrayList
                        Dim iInsuranceFileKey As Integer
                        arrQuoteCollectionFiles = Session(CNQuoteCollectionFiles)
                        iInsuranceFileKey = arrQuoteCollectionFiles(0)
                        oBalancesAndUnallocatedCredits = oWebService.GetBalancesAndUnallocatedCredits(iInsuranceFileKey)
                        oQuote = oWebService.GetHeaderAndSummariesByKey(iInsuranceFileKey)
                    Else
                        oBalancesAndUnallocatedCredits = oWebService.GetBalancesAndUnallocatedCredits(oQuote.InsuranceFileKey)
                    End If


                    With oBalancesAndUnallocatedCredits
                        'storing the data in ViewState to use again in "radioUserType_SelectedIndexChanged" and to avoid SAM call

                        ViewState.Add(CNUnallocatedCreditsForAgents, .UnallocatedCreditsForAgents)
                        ViewState.Add(CNUnallocatedCreditsForClients, .UnallocatedCreditsForClients)

                        If .AgentType = Nothing And oQuote.BusinessTypeCode = "DIRECT" Then
                            radioUserType.Items(1).Enabled = True
                            radioUserType.Items(1).Selected = True
                        Else
                            If .AgentType = "Intermed" And (oQuote.BusinessTypeCode <> "DIRECT" Or oQuote.BusinessTypeCode <> "COIN FOLL" Or oQuote.BusinessTypeCode <> "IN FAC") Then
                                radioUserType.Items(0).Enabled = True
                                radioUserType.Items(0).Selected = True
                                radioUserType.Items(1).Enabled = True
                            ElseIf .AgentType = "Broker" And (oQuote.BusinessTypeCode <> "DIRECT" Or oQuote.BusinessTypeCode <> "COIN FOLL" Or oQuote.BusinessTypeCode <> "IN FAC") Then
                                radioUserType.Items(0).Enabled = True
                                radioUserType.Items(0).Selected = True
                                radioUserType.Items(1).Enabled = False
                            ElseIf .AgentType = "Comm Acc" And (oQuote.BusinessTypeCode <> "DIRECT" Or oQuote.BusinessTypeCode <> "COIN FOLL" Or oQuote.BusinessTypeCode <> "IN FAC") Then
                                radioUserType.Items(0).Enabled = False
                                radioUserType.Items(1).Enabled = True
                                radioUserType.Items(1).Selected = True
                            End If
                        End If

                        litPolicyRefheading.Text = .InsuranceRef

                        'add the Option 'OverDraftLimit' and make it disable if not available
                        ViewState("OverDraftLimit") = .OverDraftLimit
                        radioDebitAgainst.Items.Add(GetLocalResourceObject("lbl_radioDB_OverDraft") & New Money(.OverDraftLimit, Session(CNCurrenyCode)).Formatted)
                        If Not .IsOverDraftAccount Then
                            radioDebitAgainst.Items(0).Enabled = False
                        End If

                        'add the Option 'FloatBalanceLimit' and make it disable if not available
                        ViewState("FloatBalance") = .FloatBalanceLimit
                        radioDebitAgainst.Items.Add(GetLocalResourceObject("lbl_radioDB_FloatBalance") & New Money(.FloatBalanceLimit, Session(CNCurrenyCode)).Formatted)
                        If Not .IsFloatBalanceAccount Then
                            radioDebitAgainst.Items(1).Enabled = False
                        End If

                        'add the Option 'AccountBalance' and make it disable if not available
                        'litAccountBalance.Text = .AccountBalance.ToString 'need this hidden text to validate in Live Process
                        'radioDebitAgainst.Items.Add(GetLocalResourceObject("lbl_radioDB_Amount") & New Money(.AccountBalance, Session(CNCurrenyCode)).Formatted)

                        'add the Option 'UnallocatedCredit' and make it disable if not available
                        radioDebitAgainst.Items.Add(GetLocalResourceObject("lbl_radioDB_UnallocatedCredit"))

                        If radioUserType.Items(0).Selected = True Then
                            oUnallocatedCreditCollection = ViewState(CNUnallocatedCreditsForAgents)
                            oUnallocatedCreditCollection.Sort(NexusProvider.UnallocatedCreditSort.CollectionDate, NexusProvider.Direction.Desc)
                            ViewState(CNUnallocatedCreditsForAgents) = oUnallocatedCreditCollection
                        Else
                            oUnallocatedCreditCollection = ViewState(CNUnallocatedCreditsForClients)
                            oUnallocatedCreditCollection.Sort(NexusProvider.UnallocatedCreditSort.CollectionDate, NexusProvider.Direction.Desc)
                            ViewState(CNUnallocatedCreditsForClients) = oUnallocatedCreditCollection
                        End If

                        ' If dPremiumGross is Negative Value (i.e Refund premium) then hide the Unallocated grid (i.e grdvPrePayment).
                        If oUnallocatedCreditCollection IsNot Nothing And dPremiumGross > 0 Then
                            grdvPrePayment.AllowPaging = True
                            grdvPrePayment.DataSource = oUnallocatedCreditCollection
                            grdvPrePayment.DataBind()
                        Else
                            Dim dAccountBalance As Decimal
                            For iCount As Integer = 0 To oUnallocatedCreditCollection.Count - 1
                                dAccountBalance = dAccountBalance + Math.Abs(oUnallocatedCreditCollection(iCount).Amount)
                            Next

                            radioDebitAgainst.Items(2).Text = GetLocalResourceObject("lbl_radioDB_UnallocatedCredit") & New Money(dAccountBalance, Session(CNCurrenyCode)).Formatted
                            cvUnAllocatedCreditchk.ValidationGroup = String.Empty
                        End If

                    End With

                Finally
                    oWebService = Nothing

                End Try
            End If



        End Sub

        Protected Sub cvCollectionDatechk_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvCollectionDatechk.ServerValidate
            Dim dtCollectionDate As Date
            Dim oQuote As NexusProvider.Quote
            oQuote = Session(CNQuote)

            For iTempVar As Integer = 0 To grdvPrePayment.Rows.Count - 1
                Dim chkUnAllocated As CheckBox
                chkUnAllocated = DirectCast(grdvPrePayment.Rows(iTempVar).FindControl("ChkBoxUnallocatedAmount"), CheckBox)
                If chkUnAllocated.Checked AndAlso grdvPrePayment.Rows(iTempVar).Cells(5).Text.Trim.Length <> 0 Then
                    dtCollectionDate = CDate(grdvPrePayment.Rows(iTempVar).Cells(5).Text)
                    'CollectionDate should be <= CoverFromDate
                    'check if this is Quote Collection then bypass
                    If Session(CNQuoteCollectionFiles) Is Nothing Then
                        If dtCollectionDate <= oQuote.CoverStartDate Then
                            args.IsValid = True
                        Else
                            args.IsValid = False
                            Exit For
                        End If
                    End If
                End If
            Next

        End Sub

        Protected Sub ChkBoxUnallocatedAmount_Selected(ByVal sender As Object, ByVal e As System.EventArgs)

            If radioDebitAgainst.Items(2).Selected Then 'UnAllocated
                Dim iUnAllocatedAmount As Double = 0
                Dim oUnAllocatedCredit As NexusProvider.UnallocatedCreditCollection = Nothing
                If radioUserType.Items(0).Selected Then
                    oUnAllocatedCredit = ViewState(CNUnallocatedCreditsForAgents)
                ElseIf radioUserType.Items(1).Selected Then
                    oUnAllocatedCredit = ViewState(CNUnallocatedCreditsForClients)
                End If

                For iTempVar As Integer = 0 To grdvPrePayment.Rows.Count - 1
                    Dim chkUnAllocated As CheckBox
                    chkUnAllocated = DirectCast(grdvPrePayment.Rows(iTempVar).FindControl("ChkBoxUnallocatedAmount"), CheckBox)
                    If chkUnAllocated.Checked Then
                        For jTempVar As Integer = 0 To oUnAllocatedCredit.Count - 1
                            If grdvPrePayment.Rows(iTempVar).Cells(7).Text.Trim = oUnAllocatedCredit(jTempVar).TransDetailKey.ToString Then
                                oUnAllocatedCredit(jTempVar).IsSelected = True
                                Exit For
                            End If
                        Next
                    Else
                        For jTempVar As Integer = 0 To oUnAllocatedCredit.Count - 1
                            If grdvPrePayment.Rows(iTempVar).Cells(7).Text.Trim = oUnAllocatedCredit(jTempVar).TransDetailKey.ToString Then
                                oUnAllocatedCredit(jTempVar).IsSelected = False
                                Exit For
                                'iUnAllocatedAmount += Convert.ToDouble(grdvPrePayment.Rows(iTempVar).Cells(4).Text)
                            End If
                        Next
                       
                    End If
                Next
                If radioUserType.Items(0).Selected Then
                    ViewState(CNUnallocatedCreditsForAgents) = oUnAllocatedCredit

                ElseIf radioUserType.Items(1).Selected Then
                    ViewState(CNUnallocatedCreditsForClients) = oUnAllocatedCredit
                End If

                For iTempVar As Integer = 0 To oUnAllocatedCredit.Count - 1
                    If oUnAllocatedCredit(iTempVar).IsSelected = True Then
                        iUnAllocatedAmount += oUnAllocatedCredit(iTempVar).Amount
                    End If
                Next

                litUnAllocatedCredit.Value = iUnAllocatedAmount 'need this hidden text to validate in Live Process
                'convert the total amount to Money Format and show in the "Unallocated Credit" Option
                radioDebitAgainst.Items(2).Text = GetLocalResourceObject("lbl_radioDB_UnallocatedCredit") & New Money(iUnAllocatedAmount, Session(CNCurrenyCode)).Formatted
            End If

        End Sub

        Protected Sub radioUserType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radioUserType.SelectedIndexChanged
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oUnallocatedCreditCollection As NexusProvider.UnallocatedCreditCollection = Nothing
            Dim iUnAllocatedAmount As Double = 0

            If radioUserType.Items(0).Selected Then
                'if Agent is selectd
                radioDebitAgainst.Items(0).Enabled = True
                oUnallocatedCreditCollection = ViewState(CNUnallocatedCreditsForAgents)
            ElseIf radioUserType.Items(1).Selected Then
                'if Client is selectd
                radioDebitAgainst.Items(0).Enabled = False
                radioDebitAgainst.Items(0).Selected = False
                oUnallocatedCreditCollection = ViewState(CNUnallocatedCreditsForClients)
            End If

            If oQuote.GrossTotal < 0 Then

                Dim dAccountBalance As Decimal
                For iCount As Integer = 0 To oUnallocatedCreditCollection.Count - 1
                    dAccountBalance = dAccountBalance + Math.Abs(oUnallocatedCreditCollection(iCount).Amount)
                Next
                radioDebitAgainst.Items(2).Text = GetLocalResourceObject("lbl_radioDB_UnallocatedCredit") & New Money(dAccountBalance, Session(CNCurrenyCode)).Formatted
                cvUnAllocatedCreditchk.ValidationGroup = String.Empty

            Else
                'clear the selection if user type is changed
                For iTempVar As Integer = 0 To oUnallocatedCreditCollection.Count - 1
                    If oUnallocatedCreditCollection(iTempVar).IsSelected = True Then
                        oUnallocatedCreditCollection(iTempVar).IsSelected = False
                    End If
                Next

                litUnAllocatedCredit.Value = iUnAllocatedAmount 'need this hidden text to validate in Live Process
                'convert the total amount to Money Format and show in the "Unallocated Credit" Option
                radioDebitAgainst.Items(2).Text = GetLocalResourceObject("lbl_radioDB_UnallocatedCredit") & New Money(iUnAllocatedAmount, Session(CNCurrenyCode)).Formatted
                grdvPrePayment.PageIndex = 0
            End If

            If oUnallocatedCreditCollection IsNot Nothing AndAlso oQuote.GrossTotal > 0 Then
                grdvPrePayment.DataSource = oUnallocatedCreditCollection
                grdvPrePayment.DataBind()
            End If

        End Sub

        Protected Sub radioDebitAgainst_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radioDebitAgainst.SelectedIndexChanged
            If radioDebitAgainst.Items(2).Selected Then 'Make Grid Enable if only "Unallocated Credit" option is selected
                Dim totalDue As Decimal
                totalDue = txtTotalDue.Text.Trim()

                If totalDue > 0 Then
                    grdvPrePayment.Enabled = True
                Else
                    grdvPrePayment.Visible = False
                End If
            ElseIf radioDebitAgainst.Items(1).Selected Then
                litUnAllocatedCredit.Value = ViewState("FloatBalance")
                grdvPrePayment.Enabled = False
            ElseIf radioDebitAgainst.Items(0).Selected Then
                litUnAllocatedCredit.Value = ViewState("OverDraftLimit")
                grdvPrePayment.Enabled = False
            Else
                grdvPrePayment.Enabled = False
            End If
        End Sub

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            If Session(CNQuoteCollectionFiles) IsNot Nothing Then
                Response.Redirect("~/secure/QuoteCollection.aspx", True)
            Else
                Response.Redirect("~/secure/PremiumDisplay.aspx", True)
            End If
        End Sub

        Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click

            If Page.IsValid Then
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                Dim oCreditTransaction As NexusProvider.CreditTransaction = Nothing
                Dim oPayment As NexusProvider.Payment = Session(CNPayment)
                If Session(CNPayment) IsNot Nothing Then
                    oPayment = Session(CNPayment)
                Else
                    oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.AgentCollection)
                End If
                Dim total As Decimal
                total = txtTotalDue.Text
                '
                Dim oUnallocatedCreditCollection As NexusProvider.UnallocatedCreditCollection = Nothing
                If radioUserType.Items(0).Selected Then 'if "Agent" is selected
                    oUnallocatedCreditCollection = ViewState(CNUnallocatedCreditsForAgents)
                    For iCount As Integer = 0 To oUnallocatedCreditCollection.Count - 1
                        If oUnallocatedCreditCollection(iCount).IsSelected = True Then
                            oCreditTransaction = New NexusProvider.CreditTransaction

                            oCreditTransaction.Amount = oUnallocatedCreditCollection(iCount).Amount * -1
                            oCreditTransaction.AccountKey = oUnallocatedCreditCollection(iCount).AccountKey
                            oCreditTransaction.TransDetailKey = oUnallocatedCreditCollection(iCount).TransDetailKey

                            If oUnallocatedCreditCollection(iCount).CollectionDate = Date.MinValue Or _
                                          oUnallocatedCreditCollection(iCount).CollectionDate.ToShortDateString = "01/01/0001" Or _
                                          oUnallocatedCreditCollection(iCount).CollectionDate = "01/01/1900" Then
                                'Do not read the collection date
                            Else
                                oCreditTransaction.CollectionDate = oUnallocatedCreditCollection(iCount).CollectionDate
                            End If

                            oPayment.CreditTransaction.Add(oCreditTransaction)
                        End If
                    Next

                    oPayment.PayNowDetails = Nothing
                ElseIf radioUserType.Items(1).Selected Then
                    'if "Client" is selected
                    'We have Amount in Negative but SAM expects the value in +ve
                    oUnallocatedCreditCollection = ViewState(CNUnallocatedCreditsForClients)
                    For iCount As Integer = 0 To oUnallocatedCreditCollection.Count - 1
                        If oUnallocatedCreditCollection(iCount).IsSelected = True Then
                            oCreditTransaction = New NexusProvider.CreditTransaction

                            oCreditTransaction.Amount = oUnallocatedCreditCollection(iCount).Amount * -1
                            oCreditTransaction.AccountKey = oUnallocatedCreditCollection(iCount).AccountKey
                            oCreditTransaction.TransDetailKey = oUnallocatedCreditCollection(iCount).TransDetailKey

                            If oUnallocatedCreditCollection(iCount).CollectionDate = Date.MinValue Or _
                               oUnallocatedCreditCollection(iCount).CollectionDate.ToShortDateString = "01/01/0001" Or _
                               oUnallocatedCreditCollection(iCount).CollectionDate = "01/01/1900" Then
                                'Do not read the collection date
                            Else
                                oCreditTransaction.CollectionDate = oUnallocatedCreditCollection(iCount).CollectionDate
                            End If

                            oPayment.CreditTransaction.Add(oCreditTransaction)
                        End If
                    Next

                    oPayment.PayNowDetails = Nothing
                End If

                oPayment.AmountPaid = txtTotalDue.Text

                If radioDebitAgainst.Items(0).Selected Then
                    oPayment.DebitAgainst = NexusProvider.DebitAgainstType.DebitAgainstOverDraft
                ElseIf radioDebitAgainst.Items(1).Selected Then
                    oPayment.DebitAgainst = NexusProvider.DebitAgainstType.DebitAgainstFloatBalance
                ElseIf radioDebitAgainst.Items(2).Selected Then
                    oPayment.DebitAgainst = NexusProvider.DebitAgainstType.DebitAgainstUnallocatedCredit
                End If

                'Intermediary
                If Session(CNAgentType) IsNot Nothing AndAlso Session(CNAgentType).ToString.ToUpper = "INTERMEDIARY" Then
                    If oQuote.BusinessTypeCode <> "DIRECT" Then
                        If radioUserType.Items(0).Selected Then
                            'Agent
                            oPayment.DebitAgainstAccount = "Agent"
                        ElseIf radioUserType.Items(1).Selected Then
                            'Cleint
                            oPayment.DebitAgainstAccount = "Client"
                        End If
                    End If
                End If

                Session(CNPayment) = oPayment

                'Code For Quote Colleciton
                If Session(CNQuoteCollectionFiles) IsNot Nothing Then
                    'set appropriate session values here to indicate payment taken and then redirect to end page
                    Session(CNPaid) = True
                    Response.Redirect("~/secure/QuoteCollectionConfirmation.aspx", False)
                Else
                    SetPaymentTakenAndRedirect()
                End If
            End If
        End Sub

        Protected Sub grdvPrePayment_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvPrePayment.DataBound
            If CType(sender, GridView).PageCount < 2 Then
                CType(sender, GridView).AllowPaging = False
            End If
        End Sub

        Protected Sub grdvPrePayment_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvPrePayment.PageIndexChanging
            Dim oUnallocatedCreditCollection As NexusProvider.UnallocatedCreditCollection = Nothing
            grdvPrePayment.PageIndex = e.NewPageIndex
            If radioUserType.Items(0).Selected Then
                oUnallocatedCreditCollection = ViewState(CNUnallocatedCreditsForAgents)
            ElseIf radioUserType.Items(1).Selected Then
                oUnallocatedCreditCollection = ViewState(CNUnallocatedCreditsForClients)
            End If

            grdvPrePayment.DataSource = oUnallocatedCreditCollection
            grdvPrePayment.DataBind()

        End Sub

        Protected Sub CustVldDebitAgainst_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles CustVldDebitAgainst.ServerValidate
            For iTempVar As Integer = 0 To radioDebitAgainst.Items.Count - 1
                If radioDebitAgainst.Items(iTempVar).Selected Then
                    args.IsValid = True ' if any one selected exit don't show the error message and exit sub
                    Exit Sub
                End If
            Next
            args.IsValid = False ' if No one selected show error
        End Sub

        Protected Sub grdvPrePayment_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvPrePayment.RowCreated
            'Hide the AccKey  & TransDetailKeys column

            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(6).Visible = False
                e.Row.Cells(7).Visible = False
            End If
           
        End Sub

        Protected Sub grdvPrePayment_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvPrePayment.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                If CType(e.Row.DataItem, NexusProvider.UnallocatedCredit).CollectionDate = Date.MinValue Or _
                CType(e.Row.DataItem, NexusProvider.UnallocatedCredit).CollectionDate.ToShortDateString = "01/01/0001" Or _
                CType(e.Row.DataItem, NexusProvider.UnallocatedCredit).CollectionDate < "01/01/1900" Then
                    e.Row.Cells(5).Text = String.Empty
                End If
            End If
        End Sub
    End Class


End Namespace

