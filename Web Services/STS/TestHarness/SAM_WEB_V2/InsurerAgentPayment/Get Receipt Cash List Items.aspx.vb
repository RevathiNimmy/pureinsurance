Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class CASHLIST_Get_Receipt_Cash_List_Details
    Inherits System.Web.UI.Page
    Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
    'set up the proxy object
    Dim oSAM As New SAMForInsuranceV2
    Dim totalCount As Double
    Dim allocationtimestamp As ArrayList

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then


            oSAM.SetClientCredential(UserToken)
            'Session("CashListKey") = 726

            Dim oRequest As New GetReceiptCashListItemsRequestType
            Dim oResponse As GetReceiptCashListItemsResponseType
            oSAM.SetPolicy("SamClientPolicy")
            oRequest.BranchCode = "HeadOff"
            oRequest.CashListKey = Session("CashListKey")

            oResponse = oSAM.GetReceiptCashListItems(oRequest)

            If Not (oResponse.Errors) Is Nothing Then
                'errors returned, so throw an exception
                Throw New SamResponseException(oResponse.Errors)
            Else

                Try
                    With oResponse

                        gvResult.DataSource = .ReceiptCashListItems
                        gvResult.DataBind()


                    End With
                    txttotal.Text = totalCount

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
        End If
    End Sub

    Protected Sub gvResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvResult.RowDataBound
        Dim oDataItem As New BaseGetReceiptCashListItemsResponseTypeRow
        Dim lbl As New Label

        If (e.Row.RowType = DataControlRowType.DataRow) Then
            oDataItem = DirectCast(e.Row.DataItem, BaseGetReceiptCashListItemsResponseTypeRow)

            lbl = DirectCast(e.Row.Cells(6).FindControl("Letter"), Label)
            If oDataItem.Letter = "True" Then

                lbl.Visible = True
                lbl.Text = "Y"


            Else

                lbl.Visible = True
                lbl.Text = "F"


            End If
            totalCount += Convert.ToDouble(e.Row.Cells(3).Text)
        End If

    End Sub

    Protected Sub btnadd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnadd.Click
        'Response.Redirect("CreateCashListItem.aspx")
    End Sub

    'Protected Sub Btnallocate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btnallocate.Click
    '    Response.Redirect("FindAccountDetails.aspx")
    'End Sub
    'Protected Sub gvResult_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvResult.SelectedIndexChanged
    '    Session("paymentamount") = gvResult.SelectedRow.Cells(3).Text
    '    Session("selectedrownumber") = gvResult.SelectedIndex

    'End Sub
    Protected Sub Btnallocate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btnallocate.Click
        'Session("accountkey") = Session("accountkey")
        'Session("CashListItemKey") = Session("CashListItemKey")
        'Response.Redirect("FindAccountDetails.aspx")
        UserToken = GetUserToken("sirius", "sirius")
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        Dim orequest As New GetTransactionDetailsRequestType
        Dim oresponse As New GetTransactionDetailsResponseType
        Session("Transid") = DirectCast(Session("Transid"), ArrayList)
        Dim traskeyCount As Integer = Convert.ToInt32(DirectCast(Session("Transid"), ArrayList).Count)
        Session("Amount") = DirectCast(Session("Amount"), ArrayList)
        Dim amountCount As Integer = Convert.ToInt32(DirectCast(Session("Amount"), ArrayList).Count)
        Dim al As ArrayList = DirectCast(Session("Transid"), ArrayList)
        Dim a2 As ArrayList = DirectCast(Session("Amount"), ArrayList)
        orequest.AccountKeySpecified = True
       
        orequest.AccountKey = Convert.ToInt32(Session("AccountKey"))
        orequest.BranchCode = "HeadOff"
        For i As Integer = 0 To traskeyCount - 1

            ReDim Preserve orequest.Allocation(i)
            orequest.Allocation(i) = New BaseGetTransactionDetailsRequestTypeRow
            orequest.Allocation(i).AllocationTransDetailKey = Convert.ToInt32(al.Item(i).ToString())

        Next

        oresponse = oSAM.GetTransactionDetails(orequest)
        'For itimestamp As Integer = 0 To traskeyCount - 1
        '    allocationtimestamp.Add(oresponse.Transactions(itimestamp).AllocationTimeStamp)
        'Next
        'Session("allocationtimestamp") = allocationtimestamp
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        Dim oUpdateAllocationRequest As New UpdateAllocationRequestType
        Dim oResponseUpdateAllocation As New UpdateAllocationResponseType
        Dim ipaymentamount As Double
        ipaymentamount = Session("totalMarkedAmount")


        oUpdateAllocationRequest.BranchCode = "Headoff"
        oUpdateAllocationRequest.AccountKey = Convert.ToInt32(Session("AccountKey"))
        oUpdateAllocationRequest.TransdetailKey = Convert.ToInt32(Session("TransItemKey"))
        oUpdateAllocationRequest.Amount = -(ipaymentamount) 'Session("paymentamount")
        oUpdateAllocationRequest.CashListItemKey = Convert.ToInt32(Session("CashListItemKey"))
        'oUpdateAllocationRequest.WriteOffAmountSpecified = True

        'oUpdateAllocationRequest.CurrencyDiffSpecified = False
        'If oUpdateAllocationRequest.WriteOffAmountSpecified = True Then
        '    oUpdateAllocationRequest.WriteOffReasonSpecified = True
        '    oUpdateAllocationRequest.WriteOffReason = 1
        '    oUpdateAllocationRequest.WriteOffAmount = (Convert.ToDouble(Session("totalwriteoffamount")))
        'End If

        '        If oUpdateAllocationRequest.WriteOffAmountSpecified = True Then
        'oUpdateAllocationRequest.WriteOffAmount=
        '        End If
        'If (String.IsNullOrEmpty(txtWriteOffAmount.Text)) Then
        '    oUpdateAllocationRequest.WriteOffReasonSpecified = False
        'Else
        '    oUpdateAllocationRequest.WriteOffReason = txtWriteOffReason.Text
        '    oUpdateAllocationRequest.WriteOffReasonSpecified = True
        'End If
        'If (String.IsNullOrEmpty(txtWriteOffAmount.Text)) Then
        '    oUpdateAllocationRequest.CurrencyDiffSpecified = False
        'Else
        '    oUpdateAllocationRequest.CurrencyDiff = txtCurrencyDiff.Text
        '    oUpdateAllocationRequest.CurrencyDiffSpecified = True
        'End If
        Dim iloop1 As Integer
        For iloop1 = 0 To traskeyCount - 1
            ReDim Preserve oUpdateAllocationRequest.Allocation(iloop1)
            'oUpdateAllocationRequest.Allocation(0) = New BaseUpdateAllocationRequestTypeAllocation
            ''oUpdateAllocationRequest.Allocation(0).AllocationAmount = 100
            ''oUpdateAllocationRequest.Allocation(0).AllocationAmountSpecified = True
            'oUpdateAllocationRequest.Allocation(0).AllocationTransdetailKey = 1
            'Dim txtbox As New TextBox
            'txtbox = gvoutput.Rows(iloop1 + 1).FindControl("Allocated")
            'Dim txtbox1 As New TextBox
            'txtbox1 = gvoutput.Rows(iloop1 + 1).FindControl("WriteOff")
            'If txtbox1.Text <= 0 Then
            '    oUpdateAllocationRequest.WriteOffAmountSpecified = False
            'Else
            '    oUpdateAllocationRequest.WriteOffAmountSpecified = True
            '    oUpdateAllocationRequest.WriteOffAmount = Convert.ToDouble(txtbox1.Text)
            'End If
           

            oUpdateAllocationRequest.Allocation(iloop1) = New BaseUpdateAllocationRequestTypeAllocation



            oUpdateAllocationRequest.Allocation(iloop1).AllocationAmountSpecified = True
            oUpdateAllocationRequest.Allocation(iloop1).AllocationTransdetailKey = oresponse.Transactions(iloop1).TransDetailKey
            oUpdateAllocationRequest.Allocation(iloop1).AllocationTimeStamp = oresponse.Transactions(iloop1).AllocationTimeStamp

            oUpdateAllocationRequest.Allocation(iloop1).AllocationAmount = Convert.ToDouble(a2.Item(iloop1).ToString())


        Next
        oResponseUpdateAllocation = oSAM.UpdateAllocation(oUpdateAllocationRequest)
        If Not (oresponse.Errors Is Nothing) Then
            Throw New SamResponseException(oresponse.Errors)
        Else
            ' Dim result As String
            ' result = oResponse.AllocationStatus
            'Dim i As Boolean
            'i = True
            'Session("status") = i
            'Response.Redirect("Get Receipt Cash List Items.aspx")
            Dim i As Boolean

            i = oResponseUpdateAllocation.AllocationStatus


            Session("status") = i
            If i = True Then
                Response.Redirect("../UIIC_demo/HomePage.aspx?id=" + Session("AccountCode") + "&name=Account")
            Else
                Response.Redirect("../UIIC_demo/HomePage.aspx?id=" + Session("AccountCode") + "&name=Account1")
            End If
            'Response.Write("<script>self.close();</script>")
        End If



    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Response.Write("<script>self.close();</script>")
    End Sub
End Class
