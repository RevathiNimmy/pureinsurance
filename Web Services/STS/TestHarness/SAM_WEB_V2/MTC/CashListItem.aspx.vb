Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class Claim_Payment_CashListItem
    Inherits System.Web.UI.Page

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        mvCashListItem.ActiveViewIndex = Int32.Parse(e.Item.Value)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        Dim oPayClaimRequest As New PayClaimRequestType


      

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        If Not Page.IsPostBack Then
            If (Session("AgentName") = "") Or Session("AgentType") = "Commission Account" Then
                txtAccount.Text = Session("PartyName")
            Else
                txtAccount.Text = Session("AgentName")
            End If



            txtAmount.Text = Session("Amount").ToString
            BuildLists(oSAM, ddlMediaType, STSListType.PMLookup, "MediaType", "CA")
            BuildLists(oSAM, ddlCountry, STSListType.PMLookup, "Country", "")
            BuildLists(oSAM, ddlPaymentType, STSListType.PMLookup, "cashlistItem_receipt_type", "")
            BuildLists(oSAM, ddlStatus, STSListType.PMLookup, "cashlistitem_payment_status", "")
            chkProduceDocument.Checked = True
            txtTransactionDate.Text = Today.Date
        End If
            'Dim TotalAmount As Integer
            'TotalAmount = 0
            'For Count As Integer = 0 To oPayClaimRequest.ClaimPayment.ClaimPaymentItem.Length - 1
            '    TotalAmount = TotalAmount + oPayClaimRequest.ClaimPayment.ClaimPaymentItem(Count).PaymentAmount
            'Next
            'txtAmount.Text = TotalAmount.ToString()



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
                    If (BindValue <> "") Then
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

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        'Praveen
        Dim oSAM As New SAMForInsuranceV2
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        Dim oBaseRecipetType As New BaseReceiptType

        oBaseRecipetType = DirectCast(Session("BaseReciptType"), BaseReceiptType)

        With oBaseRecipetType

            .Address1 = txtStreetName.Text
            .Address2 = txtLocality.Text
            .Address3 = txtpostTown.Text
            .Address4 = txtCountry.Text
            .CountryCode = ddlCountry.SelectedValue
            .PostalCode = txtAddressPostCode.Text
            .ContactName = txtContactName.Text
            .MediaReference = txtMediaReference.Text
            .MediaTypeCode = ddlMediaType.SelectedValue
            .OurReference = txtOurReference.Text
            .TheirReference = txtTheirReference.Text
            .TransactionDate = txtTransactionDate.Text
            .ReceiptTypeCode = ddlPaymentType.SelectedValue
            .Comments = txtComments.Text
            .Amount = Convert.ToDecimal(txtAmount.Text)
            .CollectionDateSpecified = True
            .CCAuthCode = txtAuthCode.Text
            .CCCustomer = txtContactName.Text
            .CCExpiryDate = txtExpiryDate.Text
            .CCIssue = txtIssueNumber.Text
            .CCManualAuthCode = txtManualAuth.Text
            .CCName = txtNameOnCard.Text
            .CCNumber = txtCardNumber.Text
            .CCPin = txtCSVPIN.Text
            .CCStartDate = txtStartDate.Text

            If txtChequeDate.Text <> "" Then
                .ChequeDate = txtChequeDate.Text
                .ChequeDateSpecified = True
            Else
                .ChequeDateSpecified = False
            End If
            .ChequeName = txtName.Text
            If txtCollectionDate.Text <> "" Then
                If CDate(txtCollectionDate.Text) <= Date.Today Then
                    .CollectionDate = txtCollectionDate.Text
                    .CollectionDateSpecified = True
                Else
                    lblSamErrorMessage.Text = "Collection date can not be future date"
                End If
            Else
                .CollectionDateSpecified = False
                .CollectionDate = Date.Today
            End If
        End With

        Dim oGetOptionsettingsRequest As New GetOptionSettingRequestType
        Dim oGetOptionsettingsResponse As New GetOptionSettingResponseType

        With oGetOptionsettingsRequest
            .BranchCode = "HeadOff"
            .OptionNumber = 87
            .OptionType = OptionType.ProductOption
        End With

        Try
            oGetOptionsettingsResponse = oSAM.GetOptionSetting(oGetOptionsettingsRequest)

            With oGetOptionsettingsResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                Else
                    If .OptionValue = "1" Then
                        Dim oAddPayNowRecipetRequest As New AddPayNowReceiptRequestType
                        Dim oAddPayNowRecipetResponse As New AddPayNowReceiptResponseType

                        With oAddPayNowRecipetRequest
                            If (Session("AgentName") = "") Or Session("AgentType") = "Commission Account" Then
                                .PartyKey = Session("PartyKey")
                            Else
                                .PartyKey = Session("AgentKey")

                            End If
                          
                            .Receipt = oBaseRecipetType
                            .BranchCode = "HeadOff"
                        End With
                        Try
                            oAddPayNowRecipetResponse = oSAM.AddPayNowReceipt(oAddPayNowRecipetRequest)
                            With oAddPayNowRecipetResponse
                                If .Errors IsNot Nothing Then
                                    lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                                Else
                                    Response.Redirect("PrePayment.aspx")
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
                        Dim oBindQuoteRequest As New BindQuoteRequestType
                        Dim oBindQuoteResponse As New BindQuoteResponseType
                        With oBindQuoteRequest
                            .BranchCode = "HeadOff"
                            .CreditTransactions = Nothing
                            .DebitAgainst = DebitAgainstType.DebitAgainstCashListItem
                            .DebitAgainstSpecified = True
                            .InsuranceFileKey = Session("InsuranceFileKey")
                            .PayNowDetails = oBaseRecipetType
                            .PaymentMethodSpecified = True
                            If (Session("Process") IsNot Nothing AndAlso Session("Process") = "REN") Then
                                .TransactionType = "REN"
                                .AcceptRenewal = True
                                .AcceptRenewalSpecified = True
                            Else
                                .TransactionType = "MTC"
                            End If

                        End With

                        Try
                            oBindQuoteResponse = oSAM.BindQuote(oBindQuoteRequest)
                            With oBindQuoteResponse
                                If Not (.Errors) Is Nothing Then
                                    'errors returned, so throw an exception
                                    lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                                    If (lblSamErrorMessage.Text.Contains("documentTemplateCode")) Then
                                        If (Session("Process") IsNot Nothing AndAlso Session("Process") = "REN") Then
                                            Session("StatusMessage") = "Process completed successfully without document generation"
                                            Response.Redirect(Session("ReturnPage").ToString)
                                        Else
                                            Response.Redirect("../UIIC_demo/HomePage.aspx?id=" + .Policy.PolicyRef + "&name=Policy")
                                        End If
                                    End If
                                Else
                                    If (Session("Process") IsNot Nothing AndAlso Session("Process") = "REN") Then
                                        Session("StatusMessage") = "Process completed successfully"
                                        Response.Redirect(Session("ReturnPage").ToString)
                                    Else
                                        lblPolicyNum.Text = .Policy.PolicyRef
                                        lblPolicyNum.Visible = True
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

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("PayClaim.aspx")
    End Sub

    Protected Sub ddlMediaType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMediaType.SelectedIndexChanged
        If ddlMediaType.SelectedValue = "CC" Then
            'pnlCreditCard.Visible = True
            pnlBank.Visible = False
            pnlPayee.Visible = False
        Else
            'pnlCreditCard.Visible = False
            pnlBank.Visible = True
            pnlPayee.Visible = True
        End If
    End Sub

    
End Class
