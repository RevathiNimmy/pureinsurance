Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class Claim_Payment_CashListItem
    Inherits System.Web.UI.Page

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        mvCashListItem.ActiveViewIndex = Int32.Parse(e.Item.Value)
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        If Not Page.IsPostBack Then
            BuildLists(oSAM, ddlMediaType, STSListType.PMLookup, "MediaType", "")
            BuildLists(oSAM, ddlCountry, STSListType.PMLookup, "Country", "")
            BuildLists(oSAM, ddlPaymentType, STSListType.PMLookup, "cashlistitem_receipt_type", "")
            'BuildLists(oSAM, ddlBank, STSListType.PMLookup, "bank", "")
            txtTransactionDate.Text = Today.Date
            txtAllocationStatus.Text = "UNALLOCATED"
            txtCollectionDate.Text = Today.Date
            Dim orequest As New FindBankRequestType
            Dim oresponse As New FindBankResponseType
            orequest.BranchCode = "HEADOFF"
            oresponse = oSAM.FindBank(orequest)
            ddlBank.DataSource = oresponse.Bank
            ddlBank.DataTextField = "BankName"
            ddlBank.DataValueField = "Code"
            ddlBank.DataBind()
            ddlBank.Items.Insert(0, New ListItem("", ""))
            ' ddlBank.Items.Insert(new ListItem("","")
            ddlBank.Enabled = True
            txtName.Enabled = True
            txtChequeDate.Text = Today.Date

        End If
        txtAccount.Text = Session("AccountCode")
        txtAmount.Text = Session("totalMarkedAmount")

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
      
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

       

        Dim oCreateCashList As New CreateReceiptCashListItemRequestType
        Dim oCreateCashListResposne As CreateReceiptCashListItemResponseType = Nothing
        ReDim Preserve oCreateCashList.ReceiptCashListItem(0)
        
        oCreateCashList.ReceiptCashListItem(0) = New BaseReceiptCashListItemType
        With oCreateCashList.ReceiptCashListItem(0)
            .AccountShortCode = txtAccount.Text
            .AllocationStatusCode = "U"

            .Amount = (Convert.ToDecimal(txtAmount.Text)) * -1
            If ddlMediaType.SelectedValue <> "CA" And ddlMediaType.SelectedValue <> "CC" Then
                .Bank = New BaseBankReceiptType
                .Bank.BankCode = ddlBank.SelectedItem.Text
                If (txtChequeDate.Text = "") Then
                    .Bank.ChequeDate = Today.Date
                Else
                    .Bank.ChequeDate = Convert.ToDateTime(txtChequeDate.Text)

                End If


                .Bank.PayerName = txtName.Text

            End If




            .ContactAddress = New BaseSimpleAddressType
            .ContactAddress.AddressLine1 = txtStreetName.Text
            .ContactAddress.AddressLine2 = txtLocality.Text
            .ContactAddress.AddressLine3 = txtpostTown.Text
            .ContactAddress.AddressLine4 = txtCountry.Text
            .ContactAddress.CountryCode = ddlCountry.SelectedValue
            .ContactAddress.PostCode = txtAddressPostCode.Text
            .ContactName = txtContactName.Text
            .AllocationStatusCode = "U" 'txtAllocationStatus.Text
            .FurtherDetails = txtFurtherDetails.Text
            .MediaReference = txtMediaReference.Text
            .MediaTypeCode = ddlMediaType.SelectedValue
            .OurReference = txtOurReference.Text
            .StatusCode = "ADD" 'ddlPaymentType.SelectedValue
            .TheirReference = txtTheirReference.Text
            .TransactionDate = txtTransactionDate.Text
            .TypeCode = ddlPaymentType.SelectedValue

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
                .CreditCard = New BaseCreditCardType
                .CreditCard = oCreditCard

            End If
        End With
   
        Try
            'Session("CreateReceiptCashListWithItemsRequest") = oCreateCashList
            oCreateCashList.BranchCode = "HeadOff"
            oCreateCashList.CashListKey = Session("CashListKey")

            oCreateCashListResposne = oSAM.CreateReceiptCashListItem(oCreateCashList)
            Session("CashListItem") = oCreateCashListResposne.CashListItem
            Dim int1 As Integer
            int1 = oCreateCashListResposne.CashListItem(0).CashListItemKey
            Session("CashListItemKey") = int1
        

            Dim inttranskey1 As Integer
            inttranskey1 = oCreateCashListResposne.CashListItem(0).TransDetailKey
            Session("TransItemKey") = inttranskey1
          
            With oCreateCashListResposne
                If Not (.Errors) Is Nothing Then

                    'errors returned, so throw an exception
                    Response.Write(GetMessageFromSamError(.Errors))
                Else
                    Response.Redirect("Get Receipt Cash List Items.aspx")
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

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("PayClaim.aspx")
    End Sub

    Protected Sub ddlMediaType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMediaType.SelectedIndexChanged
        If ddlMediaType.SelectedValue = "BD" Then
            txtMediaReference.Enabled = True
            txtTheirReference.Enabled = True
            txtOurReference.Enabled = True
            txtBankAccntName.Enabled = False
            txtComments.Enabled = False
            chkProduceDocument.Checked = False
            pnlcreditcard.Visible = True
            pnlcreditcard.Enabled = False
            pnlcheque.Visible = True
            pnlcheque.Enabled = True
            txtChequeDate.Text = Today.Date

        ElseIf ddlMediaType.SelectedValue = "CA" Then
            txtMediaReference.Enabled = True
            txtTheirReference.Enabled = True
            txtOurReference.Enabled = True
            txtBankAccntName.Enabled = True
            txtComments.Enabled = False
            chkProduceDocument.Checked = True

            pnlcreditcard.Visible = True
            pnlcreditcard.Enabled = False
            pnlcheque.Visible = True
            pnlcheque.Enabled = False
            txtChequeDate.Text = ""

            lbltendered.Visible = True
            txttendered.Visible = True

            lblchange.Visible = True
            txtchange.Visible = True
            txtchange.Enabled = False

        ElseIf ddlMediaType.SelectedValue = "CHAPS" Then
            txtMediaReference.Enabled = True
            txtTheirReference.Enabled = True
            txtOurReference.Enabled = True
            txtBankAccntName.Enabled = False
            txtComments.Enabled = False
            chkProduceDocument.Checked = False

            pnlcreditcard.Visible = True
            pnlcreditcard.Enabled = False
            pnlcheque.Visible = True
            pnlcheque.Enabled = False
            txtChequeDate.Text = ""
        ElseIf ddlMediaType.SelectedValue = "CQ" Then
            txtMediaReference.Enabled = True
            txtTheirReference.Enabled = True
            txtOurReference.Enabled = True
            txtBankAccntName.Enabled = False
            txtComments.Enabled = False
            chkProduceDocument.Checked = True
            pnlcreditcard.Visible = True
            pnlcreditcard.Enabled = False
            pnlcheque.Visible = True
            pnlcheque.Enabled = False
            txtChequeDate.Text = ""
        ElseIf ddlMediaType.SelectedValue = "CC" Then
            txtMediaReference.Enabled = True
            txtTheirReference.Enabled = True
            txtOurReference.Enabled = True
            txtBankAccntName.Enabled = False
            txtComments.Enabled = False
            chkProduceDocument.Checked = False
            pnlcreditcard.Visible = False
            pnlcreditcard.Enabled = True
            pnlcheque.Visible = True
            pnlcheque.Enabled = True
            txtChequeDate.Text = ""
        ElseIf ddlMediaType.SelectedValue = "DDM" Then
            txtMediaReference.Enabled = True
            txtTheirReference.Enabled = True
            txtOurReference.Enabled = True
            txtBankAccntName.Enabled = False
            txtComments.Enabled = False
            chkProduceDocument.Checked = False
            pnlcreditcard.Visible = True
            pnlcreditcard.Enabled = False
            pnlcheque.Visible = True
            pnlcheque.Enabled = False
            txtChequeDate.Text = ""
        ElseIf ddlMediaType.SelectedValue = "DD" Then
            txtMediaReference.Enabled = True
            txtTheirReference.Enabled = True
            txtOurReference.Enabled = True
            txtBankAccntName.Enabled = False
            txtComments.Enabled = False
            chkProduceDocument.Checked = False

            pnlcreditcard.Visible = True
            pnlcreditcard.Enabled = False
            pnlcheque.Visible = True
            pnlcheque.Enabled = True
            txtChequeDate.Text = Today.Date
        ElseIf ddlMediaType.SelectedValue = "DI" Then
            txtMediaReference.Enabled = True
            txtTheirReference.Enabled = True
            txtOurReference.Enabled = True
            txtBankAccntName.Enabled = True
            txtComments.Enabled = False
            chkProduceDocument.Checked = False
            pnlcreditcard.Visible = True
            pnlcreditcard.Enabled = False
            pnlcheque.Visible = True
            pnlcheque.Enabled = True
            txtChequeDate.Text = Today.Date
        ElseIf ddlMediaType.SelectedValue = "EFT" Then
            txtMediaReference.Enabled = True
            txtTheirReference.Enabled = True
            txtOurReference.Enabled = True
            txtBankAccntName.Enabled = False
            txtComments.Enabled = False
            chkProduceDocument.Checked = False
            pnlcreditcard.Visible = True
            pnlcreditcard.Enabled = False
            pnlcheque.Visible = True
            pnlcheque.Enabled = True
            txtChequeDate.Text = ""

        ElseIf ddlMediaType.SelectedValue = "SWIFT" Then
            txtMediaReference.Enabled = True
            txtTheirReference.Enabled = True
            txtOurReference.Enabled = True
            txtBankAccntName.Enabled = False
            txtComments.Enabled = False
            chkProduceDocument.Checked = False
            pnlcreditcard.Visible = True
            pnlcreditcard.Enabled = False
            pnlcheque.Visible = True
            pnlcheque.Enabled = True
            txtChequeDate.Text = ""

        ElseIf ddlMediaType.SelectedValue = "PDQ" Then
            txtMediaReference.Enabled = True
            txtTheirReference.Enabled = True
            txtOurReference.Enabled = True
            txtBankAccntName.Enabled = False
            txtComments.Enabled = False
            chkProduceDocument.Checked = False
            pnlcreditcard.Visible = True
            pnlcreditcard.Enabled = False
            pnlcheque.Visible = True
            pnlcheque.Enabled = True
            txtChequeDate.Text = ""

        ElseIf ddlMediaType.SelectedValue = "SC" Then
            txtMediaReference.Enabled = True
            txtTheirReference.Enabled = True
            txtOurReference.Enabled = True
            txtBankAccntName.Enabled = True
            txtComments.Enabled = False
            chkProduceDocument.Checked = False
            pnlcreditcard.Visible = True
            pnlcreditcard.Enabled = False
            pnlcheque.Visible = True
            pnlcheque.Enabled = True
            txtChequeDate.Text = Today.Date
        Else
            txtMediaReference.Enabled = True
            txtTheirReference.Enabled = True
            txtOurReference.Enabled = True
            txtBankAccntName.Enabled = False
            txtComments.Enabled = False
            chkProduceDocument.Checked = False
            pnlcreditcard.Visible = True
            pnlcreditcard.Enabled = False
            pnlcheque.Visible = True
            pnlcheque.Enabled = True
            txtChequeDate.Text = Today.Date

        End If
    End Sub

    Protected Sub txttendered_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txttendered.TextChanged
        txtchange.Text = Convert.ToDecimal(txttendered.Text) - Convert.ToDecimal(txtAmount.Text)
    End Sub
End Class
