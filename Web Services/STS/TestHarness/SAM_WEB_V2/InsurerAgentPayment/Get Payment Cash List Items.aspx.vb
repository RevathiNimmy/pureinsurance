Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class CASHLIST_Get_Receipt_Cash_List_Details
    Inherits System.Web.UI.Page
    Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
    'set up the proxy object
    Dim oSAM As New SAMForInsuranceV2
    Dim totalCount As Double

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        oSAM.SetClientCredential(UserToken)
        'Session("CashListKey") = 726
        'Session("accountkey") = Session("accountkey")
        'Session("CashListItemKey") = Session("CashListItemKey")

        Dim oRequest As New GetPaymentCashListItemsRequestType
        Dim oResponse As GetPaymentCashListItemsResponseType
        oSAM.SetPolicy("SamClientPolicy")
        oRequest.BranchCode = "HeadOff"
        oRequest.CashListKey = Session("CashListKey")


        oResponse = oSAM.GetPaymentCashListItems(oRequest)

        If Not (oResponse.Errors) Is Nothing Then
            'errors returned, so throw an exception
            Throw New SamResponseException(oResponse.Errors)
        Else

            Try
                With oResponse

                    gvResult.DataSource = .PaymentCashListItems
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
    End Sub

    Protected Sub gvResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvResult.RowDataBound
        Dim oDataItem As New BaseGetPaymentCashListItemsResponseTypeRow
        Dim lbl As New Label

        If (e.Row.RowType = DataControlRowType.DataRow) Then
            oDataItem = DirectCast(e.Row.DataItem, BaseGetPaymentCashListItemsResponseTypeRow)

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
        ' Response.Redirect("CreateCashListItem.aspx")
    End Sub

    Protected Sub btnview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnview.Click
        'Response.Redirect("GetPaymentCashListItemDetails.aspx")
    End Sub

    Protected Sub gvResult_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvResult.SelectedIndexChanged
        'Session("paymentamount") = gvResult.SelectedRow.Cells(3).Text
        'Session("selectedrownumber") = gvResult.SelectedIndex

    End Sub

    Protected Sub Btnpost_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btnpost.Click

    End Sub

    Protected Sub Btnallocate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btnallocate.Click
        'Session("accountkey") = Session("accountkey")
        'Session("CashListItemKey") = Session("CashListItemKey")
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

        orequest.AccountKey = Session("AccountKey")
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
        'If Session("totalwriteoffamount") = Nothing Then
        '    oUpdateAllocationRequest.WriteOffAmountSpecified = False
        'End If


        oUpdateAllocationRequest.CurrencyDiffSpecified = False

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

        'If oUpdateAllocationRequest.WriteOffAmountSpecified = True Then
        '    oUpdateAllocationRequest.WriteOffReasonSpecified = True
        '    oUpdateAllocationRequest.WriteOffReason = 1
        '    oUpdateAllocationRequest.WriteOffAmount = -(Convert.ToDouble(Session("totalwriteoffamount")))
        'End If

        Session("totalwriteoffamount") = Nothing
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
            'Dim result As String
            Dim i As Boolean
            i = oResponseUpdateAllocation.AllocationStatus


            Session("status") = i
            If i = True Then
                Response.Redirect("../UIIC_demo/HomePage.aspx?id=" + Session("AccountCode") + "&name=Account")
            Else
                Response.Redirect("../UIIC_demo/HomePage.aspx?id=" + Session("AccountCode") + "&name=Account1")
            End If
            'Response.Redirect("Get Payment Cash List Items.aspx")

            'Response.Write("<script>self.close();</script>")
        End If

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Response.Write("<script>self.close();</script>")
    End Sub
End Class
