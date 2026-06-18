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
            BuildLists(oSAM, ddlPaymentType, STSListType.PMLookup, "cashlistitem_payment_type", "")
            BuildLists(oSAM, ddlStatus, STSListType.PMLookup, "cashlistitem_payment_status", "ISS")
            txtTransactionDate.Text = Today.Date
            txtAllocationStatus.Text = "UNALLOCATED"
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
      
        Dim oResponse As New CreatePaymentCashListItemResponseType
        Dim oRequest As New CreatePaymentCashListItemRequestType

        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        

        Dim CashListitem As New BasePaymentCashListItemType
        CashListitem.AccountShortCode = txtAccount.Text
        CashListitem.StatusCode = ddlStatus.SelectedValue
        CashListitem.AllocationStatusCode = "U"


        CashListitem.Amount = Convert.ToDecimal(txtAmount.Text)
        If ddlMediaType.SelectedValue <> "CA" And ddlMediaType.SelectedValue <> "CC" Then
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

        CashListitem.TheirReference = txtTheirReference.Text
        CashListitem.TransactionDate = txtTransactionDate.Text
        CashListitem.TypeCode = ddlPaymentType.SelectedValue

        ReDim oRequest.PaymentItem(0)
        oRequest.PaymentItem(0) = New BasePaymentCashListItemType


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
        'If Trim(hfAccountKey.Value) <> "" Then

        '    Dim a() As String
        '    Dim ub As Integer

        '    If Session("accountkey") Is Nothing Then
        '        ReDim a(0)
        '        a(0) = hfAccountKey.Value
        '    Else
        '        a = Session("accountkey")
        '        ub = UBound(a)
        '        ReDim Preserve a(ub + 1)
        '        a(ub + 1) = hfAccountKey.Value
        '        'Session("accountkey") = Join(a, "|")
        '    End If

        '    Session("accountkey") = a
        '    hfAccountKey.Value = ""
        'End If

        Session("accountkey") = Session("AccountKey") 'Convert.ToInt32(hfAccountKey.Value)
        oRequest.PaymentItem(0) = CashListitem
        oRequest.CashListKey = Session("CashListKey")
        oRequest.BranchCode = "HeadOff"


        oResponse = oSAM.CreatePaymentCashListItem(oRequest)



        Dim int1 As Integer
        'Vikas Exception raised for oResponse.CashListItemKey is nothing in old and new environment
        int1 = oResponse.CashListItemKey(0)
        Session("CashListItemKey") = int1
        'Dim a1() As String
        'Dim ub1 As Integer

        'If Session("CashListItemKey") Is Nothing Then
        '    ReDim a1(0)
        '    a1(0) = oResponse.CashListItemKey(0)
        'Else
        '    a1 = Session("CashListItemKey")
        '    ub1 = UBound(a1)
        '    ReDim Preserve a1(ub1 + 1)
        '    a1(ub1 + 1) = oResponse.CashListItemKey(0)
        '    'Session("accountkey") = Join(a, "|")
        'End If

        'Session("CashListItemKey") = a1

        Dim inttranskey1 As Integer
        inttranskey1 = oResponse.TransDetailKey(0)
        Session("TransItemKey") = inttranskey1
        ''Dim a2() As String
        ''Dim ub2 As Integer

        ''If Session("TransItemKey") Is Nothing Then
        ''    ReDim a2(0)
        ''    a2(0) = oResponse.TransDetailKey(0)
        ''Else
        ''    a2 = Session("TransItemKey")
        ''    ub2 = UBound(a2)
        ''    ReDim Preserve a2(ub2 + 1)
        ''    a2(ub2 + 1) = oResponse.TransDetailKey(0)
        ''    'Session("accountkey") = Join(a, "|")
        ''End If

        'Session("TransItemKey") = a2



        With oResponse
            If .Errors IsNot Nothing Then
                Response.Write(GetMessageFromSamError(.Errors))
            Else
                Response.Redirect("Get Payment Cash List Items.aspx")
            End If
        End With





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
            chkProduceDocument.Checked = False
            pnlcreditcard.Visible = False
            pnlPayee.Visible = True
            pnlBank.Visible = True
            pnlBank.Enabled = False




          

        ElseIf ddlMediaType.SelectedValue = "CA" Then
            txtMediaReference.Enabled = True
            txtTheirReference.Enabled = True
            txtOurReference.Enabled = True
            txtBankAccntName.Enabled = True
            chkProduceDocument.Checked = True

            pnlcreditcard.Visible = False
            pnlPayee.Visible = True
            pnlBank.Visible = True
            pnlBank.Enabled = False

        ElseIf ddlMediaType.SelectedValue = "CHAPS" Then
            txtMediaReference.Enabled = True
            txtTheirReference.Enabled = True
            txtOurReference.Enabled = True
            txtBankAccntName.Enabled = False
            chkProduceDocument.Checked = False

            pnlcreditcard.Visible = False
            pnlPayee.Visible = True
            pnlBank.Visible = True
            pnlBank.Enabled = False
        ElseIf ddlMediaType.SelectedValue = "CQ" Then
            txtMediaReference.Enabled = True
            txtTheirReference.Enabled = True
            txtOurReference.Enabled = True
            txtBankAccntName.Enabled = False
            chkProduceDocument.Checked = True

            pnlcreditcard.Visible = False
            pnlPayee.Visible = True
            pnlBank.Visible = True
            pnlBank.Enabled = False
        ElseIf ddlMediaType.SelectedValue = "CC" Then
            txtMediaReference.Enabled = True
            txtTheirReference.Enabled = True
            txtOurReference.Enabled = True
            txtBankAccntName.Enabled = False
            chkProduceDocument.Checked = False

            pnlcreditcard.Visible = True
            pnlPayee.Visible = False
            pnlBank.Visible = False
            'pnlBank.Enabled = False
        ElseIf ddlMediaType.SelectedValue = "DDM" Then
            txtMediaReference.Enabled = True
            txtTheirReference.Enabled = True
            txtOurReference.Enabled = True
            txtBankAccntName.Enabled = False
            chkProduceDocument.Checked = False
            pnlcreditcard.Visible = False
            pnlPayee.Visible = True
            pnlBank.Visible = True
            pnlBank.Enabled = False
        ElseIf ddlMediaType.SelectedValue = "DD" Then
            txtMediaReference.Enabled = True
            txtTheirReference.Enabled = True
            txtOurReference.Enabled = True
            txtBankAccntName.Enabled = False
            chkProduceDocument.Checked = False

            pnlcreditcard.Visible = False
            pnlPayee.Visible = True
            pnlBank.Visible = True
            pnlBank.Enabled = False
        ElseIf ddlMediaType.SelectedValue = "DI" Then
            txtMediaReference.Enabled = True
            txtTheirReference.Enabled = True
            txtOurReference.Enabled = True
            txtBankAccntName.Enabled = False
            chkProduceDocument.Checked = False

            pnlcreditcard.Visible = False
            pnlPayee.Visible = True
            pnlBank.Visible = True
            pnlBank.Enabled = False
        ElseIf ddlMediaType.SelectedValue = "EFT" Then
            txtMediaReference.Enabled = True
            txtTheirReference.Enabled = True
            txtOurReference.Enabled = True
            txtBankAccntName.Enabled = False
            chkProduceDocument.Checked = False
            pnlcreditcard.Visible = False
            pnlPayee.Visible = True
            pnlBank.Visible = True
            pnlBank.Enabled = False
        ElseIf ddlMediaType.SelectedValue = "SWIFT" Then
            txtMediaReference.Enabled = True
            txtTheirReference.Enabled = True
            txtOurReference.Enabled = True
            txtBankAccntName.Enabled = False
            chkProduceDocument.Checked = False
            pnlcreditcard.Visible = False
            pnlPayee.Visible = True
            pnlBank.Visible = True
            pnlBank.Enabled = False
        ElseIf ddlMediaType.SelectedValue = "PDQ" Then
            txtMediaReference.Enabled = True
            txtTheirReference.Enabled = True
            txtOurReference.Enabled = True
            txtBankAccntName.Enabled = False
            chkProduceDocument.Checked = False
            pnlcreditcard.Visible = False
            pnlPayee.Visible = True
            pnlBank.Visible = True
            pnlBank.Enabled = False
        Else : ddlMediaType.SelectedValue = "SC"
            txtMediaReference.Enabled = True
            txtTheirReference.Enabled = True
            txtOurReference.Enabled = True
            txtBankAccntName.Enabled = True
            chkProduceDocument.Checked = False
            pnlcreditcard.Visible = False
            pnlPayee.Visible = True
            pnlBank.Visible = True
            pnlBank.Enabled = False
        End If
    End Sub

    
End Class
