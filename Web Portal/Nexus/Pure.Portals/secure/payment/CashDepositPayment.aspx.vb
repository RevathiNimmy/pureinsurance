Imports System.Data
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports Nexus.Utils

Namespace Nexus
    Partial Class secure_payment_CashDepositPayment : Inherits BasePayment

        Dim oWebService As NexusProvider.ProviderBase
        Dim oQuote As NexusProvider.Quote
        Dim iInsuranceFileKey As Integer
        Dim dGrossTotal As Double

        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            oQuote = Session(CNQuote) 'Use to check the BusinessTypeCode
            dGrossTotal = oQuote.GrossTotal ' Gross Total will be used for the validation on Amount 

            If Not IsPostBack Then
                SearchCDAccountForPolicy()

                If ViewState("AgentType") Is Nothing And oQuote.BusinessTypeCode = "DIRECT" Then
                    radioUserType.Items(0).Enabled = True
                    radioUserType.Items(0).Selected = True
                    radioUserType.Items(1).Enabled = False
                Else
                    If ViewState("AgentType").ToString.ToUpper = "INTERMED" And (oQuote.BusinessTypeCode <> "DIRECT") Then
                        radioUserType.Items(0).Enabled = True
                        radioUserType.Items(1).Enabled = True
                        radioUserType.Items(1).Selected = True
                    ElseIf ViewState("AgentType").ToString.ToUpper = "BROKER" And (oQuote.BusinessTypeCode <> "DIRECT") Then
                        radioUserType.Items(0).Enabled = False
                        radioUserType.Items(1).Enabled = True
                        radioUserType.Items(1).Selected = True
                    ElseIf ViewState("AgentType").ToString.ToUpper = "COMM ACC" And (oQuote.BusinessTypeCode <> "DIRECT") Then
                        radioUserType.Items(1).Enabled = False
                        radioUserType.Items(0).Enabled = True
                        radioUserType.Items(0).Selected = True
                    End If
                End If
                SetPartyDetails() 'set the Party Name and Party Code for the Current Policy

            End If

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "Confirmation", _
                    "<script language=""JavaScript"" type=""text/javascript"">function InsufficientCDAmount(CDBalance){if(CDBalance<=" & dGrossTotal & "){alert('" & GetLocalResourceObject("err_SufficientBalance").ToString() & "');return false;}}</script>")

        End Sub

        Sub SearchCDAccountForPolicy()
            Dim oCDPoliciesCollForAgent, oCDPoliciesCollForClient As NexusProvider.CashDepositsForPolicy

            oWebService = New NexusProvider.ProviderManager().Provider

            iInsuranceFileKey = oQuote.InsuranceFileKey

            If Not String.IsNullOrEmpty(oQuote.Agent) Then
                oCDPoliciesCollForAgent = oWebService.GetCashDepositsForPolicy(iInsuranceFileKey, NexusProvider.CDPartyType.Agent)

                With oCDPoliciesCollForAgent
                    ViewState("AgentType") = .AgentType
                    Session("TempCDPoliciesForAgent") = oCDPoliciesCollForAgent.CashDepositPolicies
                    grdvCDDetailsForAgents.AllowPaging = True
                    grdvCDDetailsForAgents.DataSource = oCDPoliciesCollForAgent.CashDepositPolicies
                    grdvCDDetailsForAgents.DataBind()
                    If .AgentType IsNot Nothing AndAlso .AgentType.ToString.ToUpper = "INTERMED" Then
                        oCDPoliciesCollForClient = oWebService.GetCashDepositsForPolicy(iInsuranceFileKey, NexusProvider.CDPartyType.Client)
                        With oCDPoliciesCollForClient
                            'ToDo: Need to change the value from Session to any other best way
                            Session("TempCDPoliciesForClient") = oCDPoliciesCollForClient.CashDepositPolicies
                            grdvCDDetailsForClients.DataSource = oCDPoliciesCollForClient.CashDepositPolicies
                            grdvCDDetailsForClients.DataBind()
                            'If agentType=INTERMED, then Client grid should NOT be visible
                            grdvCDDetailsForClients.Visible = False
                        End With
                    End If
                End With
            Else
                'getCdsOff = NexusProvider.CDPartyType.Client
                oCDPoliciesCollForClient = oWebService.GetCashDepositsForPolicy(iInsuranceFileKey, NexusProvider.CDPartyType.Client)
                With oCDPoliciesCollForClient
                    'ToDo: Need to change the value from Session to any other best way
                    Session("TempCDPoliciesForClient") = oCDPoliciesCollForClient.CashDepositPolicies
                    grdvCDDetailsForClients.AllowPaging = True
                    grdvCDDetailsForClients.DataSource = oCDPoliciesCollForClient.CashDepositPolicies
                    grdvCDDetailsForClients.DataBind()
                End With
            End If

            oWebService = Nothing
        End Sub

        Protected Sub grdvCDDetailsForAgents_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvCDDetailsForAgents.DataBound
            If CType(sender, GridView).PageCount < 2 Then
                CType(sender, GridView).AllowPaging = False
            End If
        End Sub

        Protected Sub grdvCDDetailsForAgents_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvCDDetailsForAgents.PageIndexChanging
            grdvCDDetailsForAgents.PageIndex = e.NewPageIndex
            'ToDo: Need to change the value from Session to any other best way
            grdvCDDetailsForAgents.DataSource = Session("TempCDPoliciesForAgent")
            grdvCDDetailsForAgents.DataBind()
        End Sub

        Protected Sub grdvCDDetailsForClients_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvCDDetailsForClients.DataBound
            If CType(sender, GridView).PageCount < 2 Then
                CType(sender, GridView).AllowPaging = False
            End If
        End Sub

        Protected Sub grdvCDDetailsForClients_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvCDDetailsForClients.PageIndexChanging
            grdvCDDetailsForClients.PageIndex = e.NewPageIndex
            'ToDo: Need to change the value from Session to any other best way
            grdvCDDetailsForClients.DataSource = Session("TempCDPoliciesForClient")
            grdvCDDetailsForClients.DataBind()
        End Sub

        Protected Sub grdvCDDetailsForAgents_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdvCDDetailsForAgents.RowCommand
            'If Page.IsValid Then
            If e.CommandName = "Select" Then
                oQuote = Session(CNQuote)

                'Making the Payment Object for the selected policy
                Dim oPayment As New NexusProvider.Payment(NexusProvider.PaymentTypes.None, CDec(Session(CNAmountToPay)))
                oPayment.PreferredDate = oQuote.InceptionDate
                oPayment.PaymentMethod = NexusProvider.PaymentTypes.CashDeposit
                oPayment.DebitAgainst = NexusProvider.DebitAgainstType.DebitAgainstCashDeposit

                Dim oCashDeposit As New NexusProvider.CashDeposit
                oCashDeposit.CashDepositRef = e.CommandArgument ' selected cashdeposit ref
                oPayment.SelectedCashDeposit = oCashDeposit ' Adding Cash Deposit to Payment object

                Session(CNPayment) = oPayment
                SetPaymentTakenAndRedirect()
                'End If
            End If
        End Sub

        Protected Sub grdvCDDetailsForClients_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdvCDDetailsForClients.RowCommand
            If e.CommandName = "Select" Then
                oQuote = Session(CNQuote)

                'Making the Payment Object for the selected policy
                Dim oPayment As New NexusProvider.Payment(NexusProvider.PaymentTypes.None, CDec(Session(CNAmountToPay)))
                oPayment.PreferredDate = oQuote.InceptionDate
                oPayment.PaymentMethod = NexusProvider.PaymentTypes.CashDeposit
                oPayment.DebitAgainst = NexusProvider.DebitAgainstType.DebitAgainstCashDeposit

                Dim oCashDeposit As New NexusProvider.CashDeposit
                oCashDeposit.CashDepositRef = e.CommandArgument ' selected cashdeposit ref
                oPayment.SelectedCashDeposit = oCashDeposit ' Adding Cash Deposit to Payment object

                Session(CNPayment) = oPayment
                SetPaymentTakenAndRedirect()
            End If
        End Sub

        Protected Sub grdvCDDetailsForAgents_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvCDDetailsForAgents.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("CashDepositKey") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.CashDepositPolicies).CashDepositKey)
                Dim oItem As NexusProvider.CashDepositPolicies = CType(e.Row.DataItem, NexusProvider.CashDepositPolicies)
                e.Row.Cells(1).Text = New Money(oItem.AvailableBalance, oItem.CurrencyCode).Formatted 'AvailableBalance
                Dim oHyperLinkSelect As LinkButton = CType(e.Row.FindControl("lnkSelect"), LinkButton)
                ' Need confiration before proceeding
                oHyperLinkSelect.Attributes.Add("OnClick", "javascript:return InsufficientCDAmount('" + e.Row.Cells(1).Text + "');")
            End If
        End Sub

        Protected Sub grdvCDDetailsForClients_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvCDDetailsForClients.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("CashDepositKey") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.CashDepositPolicies).CashDepositKey)
                Dim oItem As NexusProvider.CashDepositPolicies = CType(e.Row.DataItem, NexusProvider.CashDepositPolicies)
                e.Row.Cells(1).Text = New Money(oItem.AvailableBalance, oItem.CurrencyCode).Formatted 'AvailableBalance
                Dim oHyperLinkSelect As LinkButton = CType(e.Row.FindControl("lnkSelect"), LinkButton)
                ' Need confiration before proceeding
                oHyperLinkSelect.Attributes.Add("OnClick", "javascript:return InsufficientCDAmount('" + e.Row.Cells(1).Text + "');")
            End If
        End Sub

        Protected Sub radioUserType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radioUserType.SelectedIndexChanged
            PopulateGridwithPolicies()
            SetPartyDetails()
        End Sub

        Private Sub PopulateGridwithPolicies()
            oQuote = Session(CNQuote)
            If radioUserType.SelectedValue = "Client" Then
                grdvCDDetailsForClients.Visible = True
                grdvCDDetailsForAgents.Visible = False
            ElseIf radioUserType.SelectedValue = "Agent" Then
                grdvCDDetailsForAgents.Visible = True
                grdvCDDetailsForClients.Visible = False
            End If
        End Sub

        Private Sub SetPartyDetails()
            'set the Party Name and Party Code for the Current Policy

            Dim oAgentDetailsForPolicy As NexusProvider.AgentDetailsForPolicy

            If radioUserType.SelectedValue = "Client" Then
                Dim oParty As NexusProvider.BaseParty = Session(CNParty)
                Select Case True
                    Case TypeOf oParty Is NexusProvider.PersonalParty
                        With CType(oParty, NexusProvider.PersonalParty)
                            'If  selected Client if Personal
                            lblPartyNameValue.Text = .Title & " " & .Initials & " " & .Lastname
                        End With
                    Case TypeOf oParty Is NexusProvider.CorporateParty
                        With CType(oParty, NexusProvider.CorporateParty)
                            'If  selected Client if Corporate
                            lblPartyNameValue.Text = .CompanyName
                        End With
                End Select

                lblPartyCodeValue.Text = oParty.UserName

            ElseIf radioUserType.SelectedValue = "Agent" Then

                'Call the SAM method GetAgentDetailsForPolicy for getting the details for an Agent attached with Policy
                oWebService = New NexusProvider.ProviderManager().Provider

                iInsuranceFileKey = CType(Session(CNQuote), NexusProvider.Quote).InsuranceFileKey

                oAgentDetailsForPolicy = oWebService.GetAgentDetailsForPolicy(iInsuranceFileKey)

                lblPartyNameValue.Text = oAgentDetailsForPolicy.Name
                lblPartyCodeValue.Text = oAgentDetailsForPolicy.Shortname
            End If

            'cleaning up
            oAgentDetailsForPolicy = Nothing
            oWebService = Nothing
        End Sub

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            Response.Redirect("PaymentSelect.aspx", False)
        End Sub

    End Class
End Namespace
