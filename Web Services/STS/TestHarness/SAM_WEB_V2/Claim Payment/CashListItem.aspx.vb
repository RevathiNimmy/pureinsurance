Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class Claim_Payment_CashListItem
    Inherits System.Web.UI.Page
    Dim StartDate As Date
    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        mvCashListItem.ActiveViewIndex = Int32.Parse(e.Item.Value)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        Dim oPayClaimRequest As New PayClaimRequestType
        oPayClaimRequest = DirectCast(Session("PayClaimRequest"), PayClaimRequestType)

        txtAccount.Text = Session("AccountShortCode")
        If Session("TransactionDate") IsNot Nothing Then
            txtTransactionDate.Text = Convert.ToDateTime(Session("TransactionDate").ToString).Date
        End If
        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        If Not Page.IsPostBack Then
            BuildLists(oSAM, ddlMediaType, STSListType.PMLookup, "MediaType", "")
            BuildLists(oSAM, ddlCountry, STSListType.PMLookup, "Country", "")
            BuildLists(oSAM, ddlPaymentType, STSListType.PMLookup, "cashlistitem_payment_type", "")
            BuildLists(oSAM, ddlStatus, STSListType.PMLookup, "cashlistitem_payment_status", "")
            'Praveen
            ddlStatus.SelectedIndex = 3
            'Praveen
        End If
        Dim TotalAmount As Integer
        TotalAmount = 0
        'PraveenGora
        If oPayClaimRequest.ClaimPayment.ClaimPaymentItem IsNot Nothing Then
            'PraveenGora
            For Count As Integer = 0 To oPayClaimRequest.ClaimPayment.ClaimPaymentItem.Length - 1
                TotalAmount = TotalAmount + oPayClaimRequest.ClaimPayment.ClaimPaymentItem(Count).PaymentAmount
            Next
            'PraveenGora
        End If
        'PraveenGora
        txtAmount.Text = TotalAmount.ToString()
        'Praveen
        txtMediaReference.Text = Session("MediaReference")
        txtMediaReference.Enabled = False
        'Praveen

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
            WriteToLog(Session, "CashListItem.aspx", "SAMForInsuranceV2", "GetList", StartDate, Date.Now)
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
        Dim oPayClaimRequest As New PayClaimRequestType
        oPayClaimRequest = DirectCast(Session("PayClaimRequest"), PayClaimRequestType)


        ReDim Preserve oPayClaimRequest.ClaimPayment.CashList.PaymentItem(0)
        Dim CashListitem As New BasePaymentCashListItemType
        CashListitem.AccountShortCode = txtAccount.Text
        CashListitem.AllocationStatusCode = ddlStatus.SelectedValue

        CashListitem.Amount = Convert.ToDecimal(txtAmount.Text)
        CashListitem.Bank = New BaseBankPaymentType
        CashListitem.Bank.AccountCode = txtAccountCode.Text
        CashListitem.Bank.BranchCode = txtBranchCode.Text
        CashListitem.Bank.PayeeName = txtPayeeName.Text
        CashListitem.Bank.Reference1 = txtRef1.Text
        CashListitem.Bank.Reference2 = txtref2.Text

        If txtPaymentExpDate.Text <> "" Then
            CashListitem.Bank.ExpiryDate = txtPaymentExpDate.Text
            CashListitem.Bank.ExpiryDateSpecified = True
        Else
            CashListitem.Bank.ExpiryDateSpecified = False
        End If
        CashListitem.ContactAddress = New BaseSimpleAddressType
        CashListitem.ContactAddress.AddressLine1 = txtStreetName.Text
        CashListitem.ContactAddress.AddressLine2 = txtLocality.Text
        CashListitem.ContactAddress.AddressLine3 = txtpostTown.Text
        CashListitem.ContactAddress.AddressLine4 = txtCountry.Text
        CashListitem.ContactAddress.CountryCode = ddlCountry.SelectedValue
        CashListitem.ContactAddress.PostCode = txtAddressPostCode.Text
        CashListitem.ContactName = txtContactName.Text
        CashListitem.AllocationStatusCode = "U" 'txtAllocationStatus.Text
        CashListitem.FurtherDetails = txtFurtherDetails.Text
        CashListitem.MediaReference = txtMediaReference.Text
        CashListitem.MediaTypeCode = ddlMediaType.SelectedValue
        CashListitem.OurReference = txtOurReference.Text
        CashListitem.StatusCode = ddlStatus.SelectedValue
        CashListitem.TheirReference = txtTheirReference.Text
        CashListitem.TransactionDate = txtTransactionDate.Text
        CashListitem.TypeCode = ddlPaymentType.SelectedValue
        oPayClaimRequest.ClaimPayment.CashList.PaymentItem(0) = New BasePaymentCashListItemType


        If ddlMediaType.SelectedValue = "CC" Then
            Dim oCreditCard As New BaseCreditCardType
            Dim oCardHolder As New BaseCreditCardTypeCardHolder
            With oCardHolder
                .AddressLine1 = "Street 1"
                .AddressLine2 = "Street 2"
                .CountryCode = "GBR"
                .Name = "John Trevolta"
                .PostCode = "B73 6BY"
            End With
            With oCreditCard
                .AuthCode = txtAuthCode.Text
                .CustomerPresent = chkCustomerPresent.Checked
                .ExpiryDate = txtExpiryDate.Text
                .Issue = txtIssueNumber.Text
                .ManualAuthCode = txtManualAuth.Text
                .NameOnCreditCard = txtNameOnCard.Text
                .Number = txtCardNumber.Text
                .Pin = txtCSVPIN.Text
                .StartDate = txtStartDate.Text
                If (chkCustomerPresent.Checked) Then
                    .CustomerPresent = True
                Else
                    .CustomerPresent = False
                End If

                .CardHolder = New BaseCreditCardTypeCardHolder
                .CardHolder = oCardHolder
            End With
            CashListitem.CreditCard = New BaseCreditCardType
            CashListitem.CreditCard = oCreditCard

        End If
        oPayClaimRequest.ClaimPayment.CashList.PaymentItem(0) = CashListitem

        Session("PayClaimRequest") = oPayClaimRequest
        Response.Redirect("PayClaim.aspx")

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("PayClaim.aspx")
    End Sub

    Protected Sub ddlMediaType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMediaType.SelectedIndexChanged
        If ddlMediaType.SelectedValue = "CC" Then
            pnlCreditCard.Visible = True
            pnlBank.Visible = False
            pnlPayee.Visible = False
        Else
            pnlCreditCard.Visible = False
            pnlBank.Visible = True
            pnlPayee.Visible = True
        End If
    End Sub

    
End Class
