Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Imports System.Data
Partial Class OpenClaim_Peril
    Inherits System.Web.UI.Page
    Dim oGetClaimDetailsResponse As New GetClaimDetailsResponseType
    Dim oClaimReceiptRequestType As New ClaimReceiptRequestType
    Dim oClaimReceiptItem() As BaseClaimReceiptItemType
    Dim oSAM As New SAMForInsuranceV2
    Dim totalReserve, TotalCurrentReserve, totalReceivedToDate, TotalReceiptTax As Double
    Dim TotalPerilPaid, TotalPerilCurrentReserve, TotalPerilIncurred As Double
    Dim oPerilType() As Peril


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ''''Claim Receipt----------Saurabh
        oGetClaimDetailsResponse = DirectCast(Session("GetClaimDetailsResponse"), GetClaimDetailsResponseType)
        If Not Page.IsPostBack Then
            oPerilType = DirectCast(Session("oClaimPerils"), Peril())
            gvPerils.DataSource = oPerilType
            gvPerils.DataBind()
            lblLossDate.Text = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.LossFromDate.ToString()
        End If

        'PraveenGora
        If Not Page.IsPostBack Then
            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

            'set up the proxy object
            Session("Receipts") = Nothing
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")

            lblRisk.Text = Session("RiskTypeDesc").ToString()
            lblRiskType.Text = Session("RiskTypeDesc").ToString()
            lblLossCurrency.Text = Session("Currency").ToString()
            lblCurrency.Text = Session("Currency").ToString()

            BuildLists(oSAM, ddlTaxGroup, STSListType.PMLookup, "tax_group", "")
            BuildLists(oSAM, ddlMediaType, STSListType.PMLookup, "MediaType", "")
            BuildLists(oSAM, ddlCurrency, STSListType.PMLookup, "Currency", "")

        End If




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

    Protected Sub gvPerils_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPerils.RowDataBound

    End Sub
    Protected Sub gvPerils_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPerils.SelectedIndexChanged
        'Dim oGetClaimReceiptTaxesRequest As New GetClaimReceiptTaxesRequestType
        'Dim oGetClaimReceiptTaxesResponse As New GetClaimReceiptTaxesResponseType
        'Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        ''set up the proxy object

        'oSAM.SetClientCredential(UserToken)
        'oSAM.SetPolicy("SamClientPolicy")
        rblReceiptType.Enabled = False
        MultiView1.ActiveViewIndex = 0

        'With oGetClaimReceiptTaxesRequest
        '    .BranchCode = "HeadOff"
        '    .ClaimReceipt = DirectCast(Session("ClaimReceiptRequest"), ClaimReceiptRequestType).ClaimReceipt
        '    .ClaimReceipt.BaseClaimPerilKey = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).BaseClaimPerilKey
        '    .TimeStamp = Session("TimeStamp")

        'End With
        'Try
        '    oGetClaimReceiptTaxesResponse = oSAM.GetClaimReceiptTaxes(oGetClaimReceiptTaxesRequest)
        '    With oGetClaimReceiptTaxesResponse
        '        If Not (.Errors) Is Nothing Then
        '            'errors returned, so throw an exception
        '            Throw New SamResponseException(.Errors)
        '        End If

        '    End With

        'Catch os As SamResponseException
        '    'should do some error handling here. Just output error for now
        '    Response.Write("An error occured calling SAM:<br>" & os.Message)
        'Catch oe As Exception
        '    'should do some error handling here. Just output error for now
        '    Response.Write("An error occured:<br>" & oe.Message)

        'Finally
        '    'clean up any objects here
        'End Try

        Dim Receipts() As ClaimReceipt
        Dim RecoveryLength As New Integer
        If Not oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Recovery Is Nothing Then
            RecoveryLength = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Recovery.Length
            ReDim Preserve Receipts(RecoveryLength - 1)

            For RecoveryCount As Integer = 0 To RecoveryLength - 1
                Receipts(RecoveryCount) = New ClaimReceipt
                Receipts(RecoveryCount).RecoveryDescription = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Recovery(RecoveryCount).TypeCode
                Receipts(RecoveryCount).CurrentReserve = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Recovery(RecoveryCount).InitialRecovery + oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Recovery(RecoveryCount).RevisedRecovery
                Receipts(RecoveryCount).BaseRecoveryKey = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Recovery(RecoveryCount).BaseRecoveryKey
                Receipts(RecoveryCount).ThisReceiptInclTax = 0
                'JP Receipts(RecoveryCount).RecoveryParty = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Recovery(RecoveryCount).RecoveryPartyCode
                Receipts(RecoveryCount).ReceivedToDate = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Recovery(RecoveryCount).ReceiptedAmount - Receipts(RecoveryCount).ReceivedToDate
                Receipts(RecoveryCount).TotalRecovery = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Recovery(RecoveryCount).InitialRecovery + oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Recovery(RecoveryCount).RevisedRecovery - Receipts(RecoveryCount).ReceivedToDate
                Receipts(RecoveryCount).IsSalvage = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Recovery(RecoveryCount).IsSalvage


            Next


            'Receipts(RecoveryLength) = New ClaimReceipt
            'Receipts(RecoveryLength).TotalRecovery = 0
            'Receipts(RecoveryLength).CurrentReserve = 0
            'For RecoveryCount As Integer = 0 To RecoveryLength - 1
            '    If oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Recovery(RecoveryCount).IsSalvage.ToString = rblReceiptType.SelectedValue Then
            '        Receipts(RecoveryLength).TotalRecovery = Receipts(RecoveryLength).TotalRecovery + (oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Recovery(RecoveryCount).InitialRecovery + oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Reserve(RecoveryCount).RevisedReserve) - oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Recovery(RecoveryCount).ReceiptedAmount
            '        Receipts(RecoveryLength).CurrentReserve = Receipts(RecoveryLength).CurrentReserve + oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Recovery(RecoveryCount).InitialRecovery + oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Recovery(RecoveryCount).RevisedRecovery
            '    End If

            'Next

            'Receipts(RecoveryLength).RecoveryDescription = "Total"
            'Receipts(RecoveryLength).IsSalvage = Convert.ToInt32(rblReceiptType.SelectedValue)
            'If Not oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).ClaimReceipts Is Nothing Then
            '    Receipts(RecoveryLength).ReceivedToDate = 0
            '    Receipts(RecoveryLength).ReceivedToDateTax = 0

            '    For ReceiptCount As Integer = 0 To oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).ClaimReceipts.Length - 1
            '        If Receipts(RecoveryLength).IsSalvage.ToString = rblReceiptType.SelectedValue Then
            '            Receipts(RecoveryLength).ReceivedToDate = Receipts(RecoveryLength).ReceivedToDate + oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).ClaimReceipts(ReceiptCount).ReceiptAmount
            '            Receipts(RecoveryLength).ReceivedToDateTax = Receipts(RecoveryLength).ReceivedToDateTax + oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).ClaimReceipts(ReceiptCount).TaxAmount
            '        End If

            '    Next


            'For ReceiptCount As Integer = 0 To oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).ClaimReceipts.Length - 2
            '    For RecoveryItemCount As Integer = 0 To oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).ClaimReceipts(ReceiptCount).ReceiptItem.Length - 1
            '        For RecoveryCount As Integer = 0 To oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Recovery.Length - 1
            '            If oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).ClaimReceipts(ReceiptCount).ReceiptItem(RecoveryItemCount).BaseRecoveryKey = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Recovery(RecoveryCount).BaseRecoveryKey Then
            '                'Receipts(RecoveryCount).PaidToDateTax = Receipts(RecoveryCount).PaidToDateTax + oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).ClaimReceipts(PaymentCount).ClaimReceiptItems(PaymentItemCount).TaxAmount
            '                'Receipts(RecoveryCount).PaidToDate = Receipts(RecoveryCount).PaidToDate + oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).ClaimReceipts(PaymentCount).ClaimReceiptItems(PaymentItemCount).PaymentAmount
            '            End If
            '        Next
            '    Next
            'Next


            'End If

            ''Receipts(RecoveryLength).PaidToDate = Receipts(RecoveryLength).PaidToDate - Receipts(RecoveryLength).PaidToDateTax
            Session("Receipts") = Receipts
            gvReceiptDetails.DataSource = Receipts
            gvReceiptDetails.DataBind()

            'gvReceiptDetails.Rows(Receipts.Length - 1).Cells(3).Text = Session("totalReserve ")
            'gvReceiptDetails.Rows(Receipts.Length - 1).Cells(4).Text = TotalCurrentReserve
            '' gvReceiptDetails.Rows(Receipts.Length - 1).Cells(8).Text = totalPaidToDate
            '' gvReceiptDetails.Rows(Receipts.Length - 1).Cells(6).Text = TotalPaymentTax
        End If


    End Sub



    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click

        oClaimReceiptRequestType = DirectCast(Session("ClaimReceiptRequest"), ClaimReceiptRequestType)
        oClaimReceiptRequestType.ClaimReceipt.Payee = New BaseClaimPayeeType
        oClaimReceiptRequestType.ClaimReceipt.Payee.MediaReference = txtMediaReference.Text
        oClaimReceiptRequestType.ClaimReceipt.Payee.BankCode = txtBankCode.Text
        oClaimReceiptRequestType.ClaimReceipt.Payee.BankName = txtBankName.Text
        oClaimReceiptRequestType.ClaimReceipt.Payee.MediaTypeCode = "CA"
        oClaimReceiptRequestType.ClaimReceipt.Payee.Name = txtPayeeName.Text
        oClaimReceiptRequestType.ClaimReceipt.Payee.BankNumber = txtBankAccountNo.Text
        oClaimReceiptRequestType.ClaimReceipt.Payee.TheirReference = txtTheirReference.Text
        oClaimReceiptRequestType.ClaimReceipt.CurrencyCode = ddlCurrency.SelectedValue
        oClaimReceiptRequestType.ClaimReceipt.ReceiptPartyType = ClaimReceiptPartyTypeType.CLMRECEIVABLE
        oClaimReceiptRequestType.ClaimReceipt.PartyKey = 0
        oClaimReceiptRequestType.ClaimReceipt.IsSalvageRecovery = (rblReceiptType.SelectedValue = "1")

        Session("ClaimReceiptRequest") = oClaimReceiptRequestType
        Session("AccountShortCode") = txtShortName.Text
        'Praveen
        Session("MediaReference") = txtMediaReference.Text
        'Praveen

        Response.Redirect("ClaimReceipt.aspx")


    End Sub

    Protected Sub gvPaymentDetails_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvReceiptDetails.SelectedIndexChanged
        'JP If oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Recovery(gvReceiptDetails.SelectedIndex).RecoveryPartyCode = "" Then
        rblPaymentPartyType.SelectedIndex = 0
        txtShortName.Text = "CLMReceivable"
        rblPaymentPartyType.Enabled = True
        'JP Else
        'JP rblPaymentPartyType.SelectedValue = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Recovery(gvReceiptDetails.SelectedIndex).RecoveryPartyTypeCode.Trim
        'JP txtShortName.Text = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Recovery(gvReceiptDetails.SelectedIndex).RecoveryPartyCode
        rblPaymentPartyType.Enabled = False
        'JP End If

        If gvReceiptDetails.SelectedIndex = -1 Or rblPaymentPartyType.SelectedIndex = -1 Then
            btnEditPayment.Enabled = False
        Else
            btnEditPayment.Enabled = True
        End If
    End Sub

    Protected Sub btnEditPayment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditPayment.Click
        pnlPaymentDetails.Visible = True
        Dim Receipts() As ClaimReceipt
        Receipts = DirectCast(Session("Receipts"), ClaimReceipt())
        'lblRiskType.Text = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.RiskKey
        lblRecovery.Text = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Recovery(gvReceiptDetails.SelectedIndex).TypeCode
        lblTotalReserve.Text = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Recovery(gvReceiptDetails.SelectedIndex).InitialRecovery - oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Reserve(gvReceiptDetails.SelectedIndex).RevisedReserve
        lblReceivedToDate.Text = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Recovery(gvReceiptDetails.SelectedIndex).ReceiptedAmount
        'lblLossCurrency.Text = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.CurrencyCode
    End Sub

    Protected Sub btnPDCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPDCancel.Click
        pnlPaymentDetails.Visible = False
    End Sub

    Protected Sub btnPaymentDetailOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPaymentDetailOk.Click
        pnlPaymentDetails.Visible = False
        Dim ItemIndex As New Integer
        ItemIndex = -1
        oClaimReceiptRequestType = DirectCast(Session("ClaimReceiptRequest"), ClaimReceiptRequestType)

        oClaimReceiptItem = oClaimReceiptRequestType.ClaimReceipt.ReceiptItem

        Dim Receipts() As ClaimReceipt
        Receipts = DirectCast(Session("Receipts"), ClaimReceipt())

        If Not oClaimReceiptItem Is Nothing Then
            For ReceiptItemCount As Integer = 0 To oClaimReceiptItem.Length - 1
                If oClaimReceiptItem(ReceiptItemCount).BaseRecoveryKey = Receipts(gvReceiptDetails.SelectedIndex).BaseRecoveryKey Then
                    ItemIndex = ReceiptItemCount
                End If

            Next
            If ItemIndex = -1 Then
                ReDim Preserve oClaimReceiptItem(oClaimReceiptItem.Length)
                ItemIndex = oClaimReceiptItem.Length - 1
            End If

        Else
            ReDim Preserve oClaimReceiptItem(0)
            ItemIndex = oClaimReceiptItem.Length - 1
        End If
        Session("totalReserve ") = Convert.ToDecimal(Session("totalReserve ")) - Convert.ToDecimal(txtReceiptAmount.Text)

        Receipts(gvReceiptDetails.SelectedIndex).TotalRecovery = Receipts(gvReceiptDetails.SelectedIndex).TotalRecovery - Convert.ToDecimal(txtReceiptAmount.Text)
        '' Receipts(gvReceiptDetails.SelectedIndex).ReceivedToDate = Receipts(gvReceiptDetails.SelectedIndex).ReceivedToDate - Receipts(gvReceiptDetails.SelectedIndex).ThisReceiptInclTax + Convert.ToDecimal(txtReceiptAmount.Text)
        Receipts(gvReceiptDetails.SelectedIndex).ThisReceiptInclTax = Convert.ToDecimal(txtReceiptAmount.Text)

        'Receipts(Receipts.Length - 1).CurrentReserve = Receipts(Receipts.Length - 1).CurrentReserve - Convert.ToDecimal(txtReceiptAmount.Text)
        'Receipts(Receipts.Length - 1).ReceivedToDate = Receipts(Receipts.Length - 1).ReceivedToDate - Receipts(Receipts.Length - 1).ThisReceiptInclTax + Convert.ToDecimal(txtReceiptAmount.Text)
        'Receipts(Receipts.Length - 1).ThisReceiptInclTax = Convert.ToDecimal(txtReceiptAmount.Text)

        oClaimReceiptItem(ItemIndex) = New BaseClaimReceiptItemType
        oClaimReceiptItem(ItemIndex).BaseRecoveryKey = Receipts(gvReceiptDetails.SelectedIndex).BaseRecoveryKey
        oClaimReceiptItem(ItemIndex).ReceiptAmount = Convert.ToDecimal(txtReceiptAmount.Text)
        'JP oClaimReceiptItem(ItemIndex).RecoveryPartyCode = txtShortName.Text
        'JP oClaimReceiptItem(ItemIndex).RecoveryPartyTypeCode = rblPaymentPartyType.SelectedValue



        If ddlTaxGroup.SelectedValue = "None" Then
            oClaimReceiptItem(ItemIndex).TaxGroupCode = Nothing
        Else
            oClaimReceiptItem(ItemIndex).TaxGroupCode = ddlTaxGroup.SelectedValue
        End If
        oClaimReceiptRequestType.ClaimReceipt.ReceiptItem = oClaimReceiptItem
        oClaimReceiptRequestType.ClaimReceipt.BaseClaimPerilKey = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).BaseClaimPerilKey



        Session("ClaimReceiptRequest") = oClaimReceiptRequestType
        Session("Receipts") = Receipts
        gvReceiptDetails.DataSource = Receipts
        gvReceiptDetails.DataBind()
    End Sub

    'Protected Sub Menu3_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu3.MenuItemClick
    '    MvPayment.ActiveViewIndex = Int32.Parse(e.Item.Value)
    'End Sub

    Protected Sub TextBox3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBankName.TextChanged

    End Sub

    Protected Sub TextBox6_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTheirReference.TextChanged

    End Sub

    Protected Sub RadioButtonList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblPaymentPartyType.SelectedIndexChanged
        txtShortName.Text = ""
        If rblPaymentPartyType.SelectedValue = "" Then
            txtShortName.Enabled = True
            btnFindParty.Enabled = True
        Else
            txtShortName.Enabled = False
            btnFindParty.Enabled = False
            txtShortName.Text = "CLMReceiveable"
        End If
        If rblPaymentPartyType.SelectedValue = "CL" Then
            txtShortName.Text = DirectCast(Session("SelectedClaim"), BaseFindClaimResponseTypeRow).ClientShortName
        End If
        If rblPaymentPartyType.SelectedValue = "AG" Then
            txtShortName.Text = DirectCast(Session("SelectedClaim"), BaseFindClaimResponseTypeRow).ClientShortName
        End If
        If rblPaymentPartyType.SelectedValue = "IN" Then
            txtShortName.Text = ""
        End If
        If rblPaymentPartyType.SelectedValue = "OT" Then
            txtShortName.Text = ""
        End If
        If gvReceiptDetails.SelectedIndex = -1 Or rblPaymentPartyType.SelectedIndex = -1 Then
            btnEditPayment.Enabled = False
        Else
            btnEditPayment.Enabled = True
        End If
    End Sub



    Protected Sub btnFindParty_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindParty.Click
        Session("OpenerPage") = "CP"
    End Sub

    Protected Sub gvPaymentDetails_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvReceiptDetails.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            If DirectCast(e.Row.DataItem, ClaimReceipt).IsSalvage.ToString = rblReceiptType.SelectedValue Then
                e.Row.Visible = True
            Else
                e.Row.Visible = False
            End If
        End If

    End Sub

    Protected Sub rblReceiptType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblReceiptType.SelectedIndexChanged
        rblReceiptType.Enabled = False
    End Sub
End Class

