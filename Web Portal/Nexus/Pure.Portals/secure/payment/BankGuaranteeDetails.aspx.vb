Imports CMS.Library
Imports Nexus.Utils
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session

Namespace Nexus
    Partial Class secure_BankGuaranteeDetails : Inherits BasePayment

        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Session(CNPaid) = True Then
                SetPaymentTakenAndRedirect()
            End If

            If Not IsPostBack Then
                Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                Dim oBankGuaranteeDetails As NexusProvider.BankGuaranteeCollection = Nothing
                Dim oPartyColl As NexusProvider.PartyCollection
                Dim oPartySearchCriteria As New NexusProvider.PartySearchCriteria

                If oQuote.BusinessTypeCode = "DIRECT" Then
                    oBankGuaranteeDetails = oWebservice.GetPolicyBankGuarantee(oQuote.InsuranceFileKey, NexusProvider.BGPartyTypeType.Client)
                    radioUserType.Items(1).Enabled = True
                    radioUserType.Items(1).Selected = True
                Else
                    oPartySearchCriteria.ShortName = oQuote.AgentCode.Trim
                    oPartySearchCriteria.AgentType = Nothing
                    oPartySearchCriteria.PartyType = "AG"
                    oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.AG)

                    oPartyColl = oWebservice.FindParty(oPartySearchCriteria)
                    If oPartyColl.Count > 0 Then
                        If oPartyColl(0).AgentType IsNot Nothing Then
                            If oPartyColl(0).AgentType.Trim.ToUpper = "INTERMEDIARY" Then
                                radioUserType.Items(0).Enabled = True
                                radioUserType.Items(1).Enabled = True
                                radioUserType.Items(0).Selected = True
                                oBankGuaranteeDetails = oWebservice.GetPolicyBankGuarantee(oQuote.InsuranceFileKey, NexusProvider.BGPartyTypeType.Agent)
                            ElseIf oPartyColl(0).AgentType.Trim.ToUpper = "BROKER" Then
                                radioUserType.Items(0).Enabled = True
                                radioUserType.Items(1).Enabled = False
                                radioUserType.Items(0).Selected = True
                                oBankGuaranteeDetails = oWebservice.GetPolicyBankGuarantee(oQuote.InsuranceFileKey, NexusProvider.BGPartyTypeType.Agent)
                            ElseIf oPartyColl(0).AgentType.Trim.ToUpper = "COMMISSION" Then
                                radioUserType.Items(0).Enabled = False
                                radioUserType.Items(1).Enabled = True
                                radioUserType.Items(1).Selected = True
                                oBankGuaranteeDetails = oWebservice.GetPolicyBankGuarantee(oQuote.InsuranceFileKey, NexusProvider.BGPartyTypeType.Client)
                            End If
                        End If
                    End If
                End If

                If oBankGuaranteeDetails IsNot Nothing Then
                    If oBankGuaranteeDetails.Count > 0 Then
                        Me.txtPartyCode.Text = oBankGuaranteeDetails(0).ClientShortName
                        Me.txtPartyName.Text = oBankGuaranteeDetails(0).ClientName
                        Me.grdvBGDebtDetails.AllowPaging = True
                        Me.grdvBGDebtDetails.DataSource = oBankGuaranteeDetails
                        Me.grdvBGDebtDetails.DataBind()
                        btnSubmit.Enabled = True
                    Else
                        Me.grdvBGDebtDetails.DataSource = Nothing
                        Me.grdvBGDebtDetails.DataBind()
                        btnSubmit.Enabled = False
                    End If
                Else
                    Me.grdvBGDebtDetails.DataSource = Nothing
                    Me.grdvBGDebtDetails.DataBind()
                    btnSubmit.Enabled = False
                End If
            End If
        End Sub

        Protected Sub chkAmtSelect_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim dPremiumAmount As Double = 0

            For iTempVar As Integer = 0 To grdvBGDebtDetails.Rows.Count - 1
                Dim chkSelected As CheckBox

                chkSelected = DirectCast(grdvBGDebtDetails.Rows(iTempVar).FindControl("chkAmtSelect"), CheckBox)
                If chkSelected.Checked Then
                    dPremiumAmount += Convert.ToDouble(grdvBGDebtDetails.Rows(iTempVar).Cells(3).Text)
                    Me.txtAmount.Text = New Money(dPremiumAmount, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString
                End If

            Next
        End Sub

        Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
            If Page.IsValid Then
                Dim oPayment As New NexusProvider.Payment(NexusProvider.PaymentTypes.BankGuarantee)

                For iTempVar As Integer = 0 To Me.grdvBGDebtDetails.Rows.Count - 1
                    If Me.grdvBGDebtDetails.Rows(iTempVar).RowType = DataControlRowType.DataRow Then
                        If DirectCast(Me.grdvBGDebtDetails.Rows(iTempVar).FindControl("chkAmtSelect"), CheckBox).Checked = True Then
                            With oPayment
                                .BankGuaranteeDetails.BGKey = Me.grdvBGDebtDetails.Rows(iTempVar).Cells(1).Text
                            End With
                        End If
                    End If
                Next
                oPayment.AmountPaid = Session(CNAmountToPay)
                Session(CNPayment) = oPayment
                SetPaymentTakenAndRedirect()
                'Response.Redirect("~/secure/TransactionConfirmation.aspx")
            End If
        End Sub

        Protected Sub radioUserType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radioUserType.SelectedIndexChanged
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oBankGuaranteeDetails As NexusProvider.BankGuaranteeCollection = Nothing
            If radioUserType.Items(0).Selected = True Then
                oBankGuaranteeDetails = oWebservice.GetPolicyBankGuarantee(oQuote.InsuranceFileKey, NexusProvider.BGPartyTypeType.Agent)
            ElseIf radioUserType.Items(1).Selected = True Then
                oBankGuaranteeDetails = oWebservice.GetPolicyBankGuarantee(oQuote.InsuranceFileKey, NexusProvider.BGPartyTypeType.Client)
            End If
            If oBankGuaranteeDetails IsNot Nothing Then
                If oBankGuaranteeDetails.Count > 0 Then
                    Me.txtPartyCode.Text = oBankGuaranteeDetails(0).ClientShortName
                    Me.txtPartyName.Text = oBankGuaranteeDetails(0).ClientName
                    Me.grdvBGDebtDetails.DataSource = oBankGuaranteeDetails
                    Me.grdvBGDebtDetails.DataBind()
                    btnSubmit.Enabled = True
                Else
                    Me.grdvBGDebtDetails.DataSource = Nothing
                    Me.grdvBGDebtDetails.DataBind()
                    btnSubmit.Enabled = False
                End If
            Else
                Me.grdvBGDebtDetails.DataSource = Nothing
                Me.grdvBGDebtDetails.DataBind()
                btnSubmit.Enabled = False
            End If
        End Sub

        Protected Sub CustAmountValidation_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles CustAmountValidation.ServerValidate
            Dim iCount As Integer = 0
            CustAmountValidation.Enabled = False
            For iTempVar As Integer = 0 To grdvBGDebtDetails.Rows.Count - 1
                Dim chkSelected As CheckBox
                chkSelected = DirectCast(grdvBGDebtDetails.Rows(iTempVar).FindControl("chkAmtSelect"), CheckBox)
                If chkSelected.Checked Then
                    iCount = iCount + 1
                End If
            Next
            If iCount > 1 Then
                CustAmountValidation.Enabled = True
                args.IsValid = False
            End If
        End Sub

        Protected Sub grdvBGDebtDetails_DataBound1(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvBGDebtDetails.DataBound
            If CType(sender, GridView).PageCount < 2 Then
                CType(sender, GridView).AllowPaging = False
            End If
        End Sub

        Protected Sub grdvBGDebtDetails_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvBGDebtDetails.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(1).Visible = False
            ElseIf e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(1).Visible = False
            End If
        End Sub

        Protected Sub grdvBGDebtDetails_DataBound(ByVal sender As Object, ByVal e As System.EventArgs)
            If CType(sender, GridView).Rows.Count = 0 Or CType(sender, GridView).PageCount = 1 Then
                CType(sender, GridView).AllowPaging = False
            End If
        End Sub
    End Class
End Namespace