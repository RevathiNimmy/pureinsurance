Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Data
Partial Class Insurer_Payment_InsureragentPayment
    Inherits System.Web.UI.Page
    Dim UserToken As UsernameToken
    Dim oSAM As New SAMForInsuranceV2
    Dim COUNT As Integer
    Dim totalMarkedAmount As Double
    Dim Transid As New ArrayList
    Dim Amount As New ArrayList
    Dim currencycode As New ArrayList
    Dim markamount As New ArrayList
    Dim markKeys As New ArrayList
    Dim writeoffamount As New ArrayList
    Dim totalwriteoffamount As Double
   
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        UserToken = GetUserToken("sirius", "sirius")
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        If Not Page.IsPostBack Then
            BuildLists(oSAM, ddlPaymentGroup, STSListType.PMLookup, "source", "")
            rbtCurrecy.Items(0).Enabled = True
            rbtDAte.Items(0).Enabled = True
            'PopulatePeriod()
            txtDateTo.Text = Date.Today
            txtDateTo.Enabled = False
            Session("totalMarkedAmount") = txtTotalMarked.Text
            Session("totalwriteoffamount") = txtTotalwriteoff.Text
            txtShortName.Text = String.Empty
        End If
        pnlWritee.Visible = False
        pnlPartPay.Visible = False
        txtShortName.Enabled = False

        txtShortName.Text = Session("AccountCode")
        Session("totalMarkedAmount") = txtTotalMarked.Text
        Session("totalwriteoffamount") = txtTotalwriteoff.Text

    End Sub
    Private Sub BuildLists(ByVal oSAM As SAMForInsuranceV2, ByRef objControl As DropDownList, ByVal ESTSLookup As STSListType, ByVal ListCode As String, ByVal BindValue As String)
        Dim oRequest As New GetListRequestType
        Dim oResponse As New GetListResponseType


        oRequest.BranchCode = "HeadOff"
        oRequest.ListType = STSListType.PMLookup
        oRequest.ListCode = ListCode
        oRequest.ExcludeDeletedRecords = True

        ''rk modifies to provides the following few missing params for SAM SFI Interop testing
        'oRequest.ExcludeDeletedRecords = False 'GetListRequest.ExcludeDeletedRecords
        'oRequest.ExcludeEffectiveDate = False 'GetListRequest.ExcludeEffectiveDate
        ''oRequest.ParentFieldName = "" 'GetListRequest.ParentFieldName
        ''oRequest.ParentFieldValue = 0 'GetListRequest.ParentFieldValue
        ''oRequest.ParentFieldValueSpecified = False 'GetListRequest.ParentFieldValueSpecified
        ' ''oRequest.FilterType = CType([Enum].ToObject(GetType(GetListFilterType), GetListRequest.FilterType), BaseImplementationTypes.GetListFilterType)
        ''oRequest.FilterValue = "" 'GetListRequest.FilterValue


        Try
            Dim StartDate As Date
            StartDate = Date.Now
            oResponse = oSAM.GetList(oRequest)
            WriteToLog(Session, "InsurerAgentpayment.aspx", "SAMForInsuranceV2", "GetList", StartDate, Date.Now)
            With oResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception

                Else

                    objControl.DataSource = oResponse.List
                    objControl.DataTextField = "Description"
                    objControl.DataValueField = "code"
                    objControl.DataBind()
                    If (BindValue = "") Then
                        objControl.Items.Insert(0, New ListItem("", ""))
                    Else
                        objControl.SelectedValue = BindValue
                    End If
                End If
            End With

        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            Response.Write("An error occured calling SAM:<br>" & os.Message)

            ''Catch oe As Exception
            ''    'should do some error handling here. Just output error for now
            ''    Response.Write("An error occured:<br>" & oe.Message)

        Finally
            'clean up any objects here
        End Try

    End Sub

    Dim gvFinal As New GridView
    'Public Sub FilterAll(ByVal oRes As GetInsurerPaymentsResponseType)

    '    Dim fnd As System.Collections.Generic.List(Of GetInsurerPaymentsResponseType)
    '    fnd = New System.Collections.Generic.List(Of GetInsurerPaymentsResponseType)(oRes).FindAll(AddressOf TestLogic)
    'End Sub
    'Public Function TestLogic(ByVal obj As GetInsurerPaymentsResponseType) As Boolean
    '    Return obj.InsurerPayments(0).DocumentRef.Equals(Session("DocRef"))

    'End Function
    Protected Sub btnFindNow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindNow.Click

        UserToken = GetUserToken("sirius", "sirius")
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        

        Dim oInsurerpaymentRequest As New GetInsurerPaymentsRequestType

        Dim oInsurerpaymentResponse As New GetInsurerPaymentsResponseType
        With oInsurerpaymentRequest
            .AccountKey = Convert.ToInt32(hfAccountKey.Value)
            Session("AccountKey") = Convert.ToInt32(hfAccountKey.Value)
            Session("AccountCode") = txtShortName.Text
            .BranchCode = "HeadOff"
            .AlternateReference = txtAlternateRef.Text
            .InsurerPaymentBranchCode = ddlPaymentGroup.SelectedValue
            If chkDateTo.Checked = True Then
                .DateTo = txtDateTo.Text
                .DateToSpecified = True

            End If
            If rbtDAte.Items(0).Selected = True Then
                .DateByTransaction = InsurerPaymentsDateByType.EffectiveDate
                .DateByTransactionSpecified = False
            Else
                .DateByTransaction = InsurerPaymentsDateByType.TransactionDate
                .DateByTransactionSpecified = True
            End If
            .MarkedStatusSpecified = True

            .MarkedStatus = ddlMarkedStatus.SelectedItem.Value



            .MonthSpecified = True
            .Month = ddlMonth.SelectedItem.Value




            oInsurerpaymentResponse = oSAM.GetInsurerPayments(oInsurerpaymentRequest)
            Session("Response") = oInsurerpaymentResponse
            gvTransactions.DataSource = oInsurerpaymentResponse.InsurerPayments

            gvTest.DataSource = oInsurerpaymentResponse.InsurerPayments
            gvTransactions.DataBind()
            gvTest.DataBind()
            FillGrid()

            populatetranskey()

            txtTotalMarked.Text = totalMarkedAmount
            txtTotalwriteoff.Text = totalwriteoffamount
            gvTransactions.Visible = True
        End With
      
    End Sub

    Public Sub FillGrid()
      
        Dim CurrencyAmount As Double
        Dim PaidAmount As Double
        Dim MarkedAmount As Double

        For iCount As Integer = 0 To gvTransactions.Rows.Count - 1
            For iCnt As Integer = 0 To gvTest.Rows.Count - 1
                If gvTransactions.Rows(iCount).Cells(4).Text = gvTest.Rows(iCnt).Cells(4).Text Then
                    CurrencyAmount += Convert.ToDouble(gvTest.Rows(iCnt).Cells(8).Text)
                    PaidAmount += Convert.ToDouble(gvTest.Rows(iCnt).Cells(9).Text)

                    MarkedAmount += Convert.ToDouble(gvTest.Rows(iCnt).Cells(14).Text)
                    gvTransactions.Rows(iCount).Cells(8).Text = CurrencyAmount
                    gvTransactions.Rows(iCount).Cells(9).Text = PaidAmount

                    gvTransactions.Rows(iCount).Cells(14).Text = MarkedAmount

                End If
            Next
            For idel As Integer = iCount + 1 To gvTransactions.Rows.Count - 1
                If gvTransactions.Rows(iCount).Cells(4).Text = gvTransactions.Rows(idel).Cells(4).Text Then
                    gvTransactions.DeleteRow(idel)
                    gvTransactions.Rows(idel).Visible = False
                End If
                CurrencyAmount = 0.0
                PaidAmount = 0.0

                MarkedAmount = 0.0
            Next
        Next
    End Sub
    Public Sub populatetranskey()
        For iCount As Integer = 0 To gvTest.Rows.Count - 1
            If gvTest.Rows(iCount).Cells(14).Text <> "0" Then

                Transid.Add(Convert.ToInt32(gvTest.Rows(iCount).Cells(13).Text))
                Amount.Add(Convert.ToDouble(gvTest.Rows(iCount).Cells(14).Text))

            End If
            If gvTest.Rows(iCount).Cells(21).Text = "WRITEOFF" Then
                totalwriteoffamount += Convert.ToDouble(gvTest.Rows(iCount).Cells(14).Text)
                writeoffamount.Add(gvTest.Rows(iCount).Cells(14).Text)
            End If

        Next
        Session("Transid") = Transid
        Session("Amount") = Amount
        Session("writeoffamount") = writeoffamount




    End Sub

  
    Protected Sub ddlMarkedStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMarkedStatus.SelectedIndexChanged
        Dim oInsurerpaymentRequest As New GetInsurerPaymentsRequestType

        Dim oInsurerpaymentResponse As New GetInsurerPaymentsResponseType
        oInsurerpaymentRequest.MarkedStatus = ddlMarkedStatus.SelectedValue
    End Sub

    Protected Sub ddlPaymentGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPaymentGroup.SelectedIndexChanged
        Dim oInsurerpaymentRequest As New GetInsurerPaymentsRequestType

        Dim oInsurerpaymentResponse As New GetInsurerPaymentsResponseType
        oInsurerpaymentRequest.BranchCode = ddlPaymentGroup.SelectedValue

    End Sub

    'Protected Sub gvTransactionDetails_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvTransactionDetails.SelectedIndexChanged
    '    'UserToken = GetUserToken("sirius", "sirius")
    '    'oSAM.SetClientCredential(UserToken)
    '    'oSAM.SetPolicy("SamClientPolicy")
    '    'Dim arr As New ArrayList
    '    'Dim arr1 As New ArrayList
    '    'Dim arr2 As New ArrayList

    '    'If Not Session("Arr") Is Nothing Then
    '    '    arr = Session("Arr")
    '    '    arr1 = Session("Arr1")
    '    '    arr2 = Session("Arr2")
    '    '    arr1.Add(gvTransactionDetails.SelectedRow.Cells(10).Text)
    '    '    arr2.Add(gvTransactionDetails.SelectedRow.Cells(3).Text)
    '    '    arr.Add(gvTransactionDetails.SelectedRow.DataItemIndex)
    '    'Else
    '    '    arr.Add(gvTransactionDetails.SelectedRow.DataItemIndex)
    '    '    arr1.Add(gvTransactionDetails.SelectedRow.Cells(10).Text)
    '    '    arr2.Add(gvTransactionDetails.SelectedRow.Cells(3).Text)
    '    '    Session("Arr") = arr
    '    '    Session("Arr1") = arr1
    '    '    Session("Arr2") = arr2
    '    'End If


    '    'Dim oFindAccountDetailsRequest As New GetAccountDetailsRequestType
    '    'Dim oFindAccountDetailsResponse As New GetAccountDetailsResponseType
    '    'With oFindAccountDetailsRequest



    '    '    'Mandatory Input Fields

    '    '    .PartyCntSpecified = False
    '    '    .AccountKeySpecified = True
    '    '    .AccountKey = Convert.ToInt32(hfAccountKey.Value) '3554
    '    '    .BranchCode = ddlPaymentGroup.SelectedValue

    '    '    .DocumentRef = (gvTransactionDetails.SelectedRow.Cells(3).Text)

    '    '    oFindAccountDetailsResponse = oSAM.GetAccountDetails(oFindAccountDetailsRequest)
    '    '    gvOutStandingTransactionDetails.DataSource = oFindAccountDetailsResponse.Transactions
    '    '    gvOutStandingTransactionDetails.DataBind()

    '    '    Session("docref") = (gvTransactionDetails.SelectedRow.Cells(3).Text)


    '    '    'lblTranscount.Text = gvOutStandingTransactionDetails.Rows.Count.ToString
    '    '    gvOutStandingTransactionDetails.Visible = True
    '    'End With
    '    'For i As Integer = 0 To arr.Count - 1
    '    '    gvTransactionDetails.Rows(arr(i)).BackColor = Drawing.Color.Cornsilk
    '    'Next
    'End Sub


    'Protected Sub gvOutStandingTransactionDetails_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvOutStandingTransactionDetails.SelectedIndexChanged

    '    Response.Redirect("FindAccountDetails.aspx")
    'End Sub

    'Protected Sub btnDrill_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDrill.Click
    '    If gvTransactionDetails.SelectedIndex <> -1 Then
    '        Response.Redirect("FindAccountDetails.aspx")
    '    Else

    '    End If

    'End Sub

    Protected Sub txtTotalMarked_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTotalMarked.TextChanged

    End Sub

    'Protected Sub gvTransactionDetails_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTransactionDetails.RowDataBound
    '    'If (e.Row.RowType = DataControlRowType.DataRow) Then
    '    '    If DirectCast(e.Row.DataItem, BaseGetAccountDetailsResponseTypeRow).OutStandingCurrencyAmount = 0 Then
    '    '        e.Row.Visible = False
    '    '    Else
    '    '        e.Row.Visible = True
    '    '        If e.Row.Visible = True Then
    '    '            'Dim COUNT As Integer = 0
    '    '            COUNT = COUNT + 1

    '    '        End If



    '    '    End If

    '    'End If

    '    'If (e.Row.RowType = DataControlRowType.DataRow) Then
    '    '    totalCount += Convert.ToDouble(e.Row.Cells(14).Text)
    '    'End If
    'End Sub

    Protected Sub Calendar1_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Calendar1.SelectionChanged
        ' txtDateTo.Text = Calendar1.SelectedDate.Date
    End Sub
    'Public Sub PopulatePeriod()
    '    Dim cn As New SqlConnection("Data Source=localhost;Initial Catalog=Sirius_UIIC;Integrated Security=True")
    '    Dim cntI As Integer
    '    Dim objDataReader As SqlDataReader
    '    Dim objCmdAllGroups As SqlCommand

    '    If (cn.State = ConnectionState.Closed) Then
    '        cn.Open()
    '    End If

    '    cntI = 0
    '    objCmdAllGroups = New SqlCommand("select year_name + ':' + period_name as description,period_id as code from period where year_name=@yearname", cn)
    '    objCmdAllGroups.CommandType = CommandType.Text
    '    objCmdAllGroups.Parameters.Add("yearname", SqlDbType.VarChar)
    '    objCmdAllGroups.Parameters("yearname").Value = Year(System.DateTime.Now)
    '    objDataReader = objCmdAllGroups.ExecuteReader()
    '    ddlMonth.Items.Clear()
    '    While (objDataReader.Read())
    '        ddlMonth.Items.Insert(++cntI, New ListItem(objDataReader("description"), objDataReader("code")))
    '    End While
    '    ddlMonth.Items.Insert(0, New ListItem("(all)", "0"))
    '    objDataReader.Close()
    'End Sub

    'Protected Sub btnPay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPay.Click
    '    'Session("ACCOUNTCODE") = txtShortName.Text
    '    'Response.Redirect("CashList.aspx")

    '    Session("Arr") = DirectCast(Session("Arr"), ArrayList)
    '    Session("Arr1") = DirectCast(Session("Arr1"), ArrayList)
    '    Session("Arr2") = DirectCast(Session("Arr2"), ArrayList)
    '    Dim traskeyCount As Integer = DirectCast(Session("Arr1"), ArrayList).Count
    '    Dim al As ArrayList = DirectCast(Session("Arr1"), ArrayList)


    '    Dim oSAM As New SAMForInsuranceV2
    '    Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

    '    oSAM.SetClientCredential(UserToken)

    '    oSAM.SetPolicy("SamClientPolicy")

    '    Dim orequest As New GetTransactionDetailsRequestType
    '    Dim oresponse As New GetTransactionDetailsResponseType
    '    orequest.AccountKeySpecified = True
    '    orequest.AccountKey = Session("accountkey")
    '    orequest.BranchCode = "HeadOff"

    '    For i As Integer = 0 To traskeyCount - 1

    '        ReDim Preserve orequest.Allocation(i)
    '        orequest.Allocation(i) = New BaseGetTransactionDetailsRequestTypeRow
    '        orequest.Allocation(i).AllocationTransDetailKey = Convert.ToInt32(al.Item(i).ToString())
    '    Next

    '    oresponse = oSAM.GetTransactionDetails(orequest)




    'End Sub

    'Protected Sub gvTransactionDetails_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvTransactionDetails.RowDeleting

    'End Sub


  

    Protected Sub gvTransactions_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTransactions.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then

            If e.Row.Cells(6).Text = "01-Jan-0001 00:00:00" Then
                e.Row.Cells(6).Text = String.Empty
            End If

            totalMarkedAmount += Convert.ToDouble(e.Row.Cells(14).Text)
            Dim chk As New CheckBox
            chk = DirectCast(e.Row.FindControl("chk"), CheckBox)
            Dim MarkedAmount As Double = Convert.ToDouble(e.Row.Cells(14).Text)
            If MarkedAmount <> 0.0 Then
                chk.Checked = True
                'Amount.Add(Convert.ToDouble(e.Row.Cells(14).Text))
            End If
            'Session("Amount") = Amount

        End If

    End Sub

    Protected Sub gvTransactions_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvTransactions.SelectedIndexChanged
        If gvTransactions.SelectedIndex <> -1 Then



            If chk = False Then
                UserToken = GetUserToken("sirius", "sirius")
                oSAM.SetClientCredential(UserToken)
                oSAM.SetPolicy("SamClientPolicy")

                Session("DocRef") = gvTransactions.SelectedRow.Cells(4).Text
                Dim oInsurerpaymentRequest As New GetInsurerPaymentsRequestType

                Dim oInsurerpaymentResponse As New GetInsurerPaymentsResponseType

                'FilterAll(Session("Response"))
                With oInsurerpaymentRequest
                    .AccountKey = Convert.ToInt32(hfAccountKey.Value)
                    .BranchCode = "HeadOff"
                    .AlternateReference = txtAlternateRef.Text
                    .InsurerPaymentBranchCode = ddlPaymentGroup.SelectedValue
                    Session("Branch") = .InsurerPaymentBranchCode

                    If chkDateTo.Checked = True Then
                        .DateTo = txtDateTo.Text
                        .DateToSpecified = True

                    End If
                    If rbtDAte.Items(0).Selected = True Then
                        .DateByTransaction = InsurerPaymentsDateByType.EffectiveDate
                        .DateByTransactionSpecified = False
                    Else
                        .DateByTransaction = InsurerPaymentsDateByType.TransactionDate
                        .DateByTransactionSpecified = True
                    End If
                    .MarkedStatusSpecified = True

                    .MarkedStatus = ddlMarkedStatus.SelectedItem.Value



                    .MonthSpecified = True
                    .Month = ddlMonth.SelectedItem.Value
                    'lstDEST.Add(lst.Item(index))
                    oInsurerpaymentResponse = oSAM.GetInsurerPayments(oInsurerpaymentRequest)
                    gvTransactions.DataSource = oInsurerpaymentResponse.InsurerPayments
                    gvTransactions.DataBind()
                    gvOutstandingTrans.DataSource = oInsurerpaymentResponse.InsurerPayments
                    gvOutstandingTrans.DataBind()
                End With

            Else

                'Dim dr As GridViewRow
                'Dim CC As String = gvTransactions.Rows(dr.RowIndex).Cells(2).ToString()
                Dim oRequestMarkunmark As New MarkUnmarkTransactionRequestType
                Dim oResponseMarkunmark As New MarkUnmarkTransactionResponseType
                Dim MarkKeyCount As Integer = Convert.ToInt32(DirectCast(Session("markKeys"), ArrayList).Count)

                For icnt As Integer = 0 To gvOutstandingTrans.Rows.Count - 1

                    Dim ChkgvOutstandingTrans As New CheckBox
                    ChkgvOutstandingTrans = gvOutstandingTrans.Rows(icnt).FindControl("chkeachout")
                  

                    If gvOutstandingTrans.Rows(icnt).Visible = True Then

                        oRequestMarkunmark.BranchCode = gvOutstandingTrans.Rows(icnt).Cells(32).Text '("Branch") 'ddlPaymentGroup.SelectedValue
                        oRequestMarkunmark.CurrencyCode = gvOutstandingTrans.Rows(icnt).Cells(19).Text
                        Dim chk1 As New CheckBox
                        chk1 = DirectCast(gvTransactions.SelectedRow.FindControl("chk"), CheckBox)
                        If chk1.Checked = True Then
                            oRequestMarkunmark.MarkStatus = MarkStatusType.Mark
                            oRequestMarkunmark.PaymentAmount = Convert.ToDouble(gvOutstandingTrans.Rows(icnt).Cells(8).Text) - Convert.ToDouble(gvOutstandingTrans.Rows(icnt).Cells(9).Text)
                            ChkgvOutstandingTrans.Checked = True
                        Else
                            oRequestMarkunmark.MarkStatus = MarkStatusType.UnMark
                            oRequestMarkunmark.PaymentAmount = 0
                            ChkgvOutstandingTrans.Checked = False
                        End If


                        oRequestMarkunmark.TransactionKey = Convert.ToInt32(gvOutstandingTrans.Rows(icnt).Cells(13).Text)
                        oResponseMarkunmark = oSAM.MarkUnmarkTransaction(oRequestMarkunmark)
                    End If

                Next

            End If
            gvTransactions.DataSource = Nothing
            gvOutstandingTrans.DataSource = Nothing

            'gvTransactions.DataBind()
            'gvOutstandingTrans.DataBind()
            totalMarkedAmount = 0.0
            enablewriteoff()
            getinsurerPayment()
        End If
    End Sub

    Protected Sub gvOutstandingTrans_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvOutstandingTrans.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            If DirectCast(e.Row.DataItem, BaseGetInsurerPaymentsResponseTypeRow).DocumentRef = Session("DocRef") Then
                e.Row.Visible = True
                Session("DocumentId") = DirectCast(e.Row.DataItem, BaseGetInsurerPaymentsResponseTypeRow).DocumentId
                markamount.Add(DirectCast(e.Row.DataItem, BaseGetInsurerPaymentsResponseTypeRow).CurrencyAmount - DirectCast(e.Row.DataItem, BaseGetInsurerPaymentsResponseTypeRow).PaidAmount)
                Session("markamount") = markamount
                markKeys.Add(DirectCast(e.Row.DataItem, BaseGetInsurerPaymentsResponseTypeRow).TransdetailId)

                currencycode.Add(DirectCast(e.Row.DataItem, BaseGetInsurerPaymentsResponseTypeRow).CurrencyCode)

                Session("currencyCode") = currencycode

            Else

                e.Row.Visible = False


            End If
            Dim chk As New CheckBox
            chk = DirectCast(e.Row.FindControl("chkeachout"), CheckBox)
            Dim MarkedAmount As Double = Convert.ToDouble(e.Row.Cells(14).Text)
            If MarkedAmount <> 0.0 Then
                chk.Checked = True

                'Session("Transid") = Transid.Add(e.Row.Cells(13).Text)

            End If
        End If
       
        Session("markKeys") = markKeys
    End Sub
    
    Protected Sub gvTransactions_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvTransactions.RowDeleting

    End Sub

    Protected Sub btnPay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPay.Click
        If txtTotalMarked.Text > 0 Then
            Response.Redirect("CashListReceipt.aspx")
        Else
            Response.Redirect("CashListPayment.aspx")
        End If

    End Sub

    Protected Sub gvOutstandingTrans_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvOutstandingTrans.RowDeleting

    End Sub

    

    Protected Sub btnWRITEOFF_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWRITEOFF.Click
        pnlWritee.Visible = True
       

    End Sub

    Protected Sub Chk_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        chk = True

        gvTransactions_SelectedIndexChanged(sender, e)


    End Sub
    Public chk As Boolean = False
   
    Protected Sub gvTransactions_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvTransactions.RowEditing

    End Sub

    Protected Sub gvTransactions_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTransactions.RowCommand
       
    End Sub

    Public Sub getinsurerPayment()
        UserToken = GetUserToken("sirius", "sirius")
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")


        Dim oInsurerpaymentRequest As New GetInsurerPaymentsRequestType

        Dim oInsurerpaymentResponse As New GetInsurerPaymentsResponseType
        With oInsurerpaymentRequest
            .AccountKey = Convert.ToInt32(hfAccountKey.Value)
            Session("AccountKey") = Convert.ToInt32(hfAccountKey.Value)
            Session("AccountCode") = txtShortName.Text
            .BranchCode = "HeadOff"
            .AlternateReference = txtAlternateRef.Text
            .InsurerPaymentBranchCode = ddlPaymentGroup.SelectedValue
            If chkDateTo.Checked = True Then
                .DateTo = txtDateTo.Text
                .DateToSpecified = True

            End If
            If rbtDAte.Items(0).Selected = True Then
                .DateByTransaction = InsurerPaymentsDateByType.EffectiveDate
                .DateByTransactionSpecified = False
            Else
                .DateByTransaction = InsurerPaymentsDateByType.TransactionDate
                .DateByTransactionSpecified = True
            End If
            .MarkedStatusSpecified = True

            .MarkedStatus = ddlMarkedStatus.SelectedItem.Value 'InsurerPaymentsMarkedStatus.Any '



            .MonthSpecified = True
            .Month = ddlMonth.SelectedItem.Value

            oInsurerpaymentResponse = oSAM.GetInsurerPayments(oInsurerpaymentRequest)
            gvTransactions.DataSource = oInsurerpaymentResponse.InsurerPayments
            gvTest.DataSource = oInsurerpaymentResponse.InsurerPayments
            gvTransactions.DataBind()
            gvTest.DataBind()
            FillGrid()
            populatetranskey()

            gvOutstandingTrans.DataSource = oInsurerpaymentResponse.InsurerPayments
            gvOutstandingTrans.DataBind()
            enablewriteoff()
            txtTotalMarked.Text = totalMarkedAmount

        End With

    End Sub

    Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        UserToken = GetUserToken("sirius", "sirius")
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oRequestWriteOff As New AddWriteOffRequestType
        Dim oResponseWriteOff As New AddWriteOffResponseType


        oRequestWriteOff.AccountKey = Session("AccountKey")
        oRequestWriteOff.BranchCode = Session("Branch")
        oRequestWriteOff.DocumentKey = Session("DocumentId")
        oRequestWriteOff.WriteOffAmount = Convert.ToDouble(txtWriteoffAmount.Text)

        oResponseWriteOff = oSAM.AddWriteOff(oRequestWriteOff)
        pnlWritee.Visible = False
        getinsurerPayment()
    End Sub
    Public chkeach As Boolean = False
    Protected Sub btnDrill_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDrill.Click
        If gvTransactions.SelectedIndex <> -1 Then
            'Response.Redirect("FindAccountDetails.aspx")
        Else

        End If
    End Sub

    Protected Sub gvOutstandingTrans_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvOutstandingTrans.SelectedIndexChanged
       

    End Sub
    Protected Sub chkeach_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If gvOutstandingTrans.SelectedIndex <> -1 Then


            chkeach = True
            'gvOutstandingTrans_SelectedIndexChanged(sender, e)
            If chkeach = True Then
                Dim oRequestMarkunmark As New MarkUnmarkTransactionRequestType
                Dim oResponseMarkunmark As New MarkUnmarkTransactionResponseType

                oRequestMarkunmark.BranchCode = gvOutstandingTrans.SelectedRow.Cells(32).Text
                oRequestMarkunmark.CurrencyCode = gvOutstandingTrans.SelectedRow.Cells(19).Text
                Dim chk2 As CheckBox
                chk2 = DirectCast(gvOutstandingTrans.SelectedRow.FindControl("chkeachout"), CheckBox)
                If chk2.Checked = True Then
                    oRequestMarkunmark.MarkStatus = MarkStatusType.Mark
                    oRequestMarkunmark.PaymentAmount = Convert.ToDouble(gvOutstandingTrans.SelectedRow.Cells(8).Text) - Convert.ToDouble(gvOutstandingTrans.SelectedRow.Cells(9).Text)

                Else
                    oRequestMarkunmark.MarkStatus = MarkStatusType.UnMark
                    oRequestMarkunmark.PaymentAmount = 0.0

                End If
                oRequestMarkunmark.TransactionKey = Convert.ToInt32(gvOutstandingTrans.SelectedRow.Cells(13).Text)
                oResponseMarkunmark = oSAM.MarkUnmarkTransaction(oRequestMarkunmark)

            End If

            gvTransactions.DataSource = Nothing
            gvOutstandingTrans.DataSource = Nothing


            totalMarkedAmount = 0.0
            getinsurerPayment()
        End If
    End Sub

    Protected Sub btnPartPay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPartPay.Click


        If gvOutstandingTrans.SelectedIndex <> -1 Then

            pnlPartPay.Visible = True

           
           
        End If
    End Sub

    Protected Sub btnpartpayOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnpartpayOk.Click

        Dim chk2 As CheckBox
        chk2 = DirectCast(gvOutstandingTrans.SelectedRow.FindControl("chkeachout"), CheckBox)
        If chk2.Checked = False Then
            mark()
            
        Else
            unmark()
            mark()

        End If


        gvTransactions.DataSource = Nothing
        gvOutstandingTrans.DataSource = Nothing


        totalMarkedAmount = 0.0
        getinsurerPayment()
        pnlPartPay.Visible = False
    End Sub
    Public Sub unmark()
        Dim chk2 As CheckBox
        chk2 = DirectCast(gvOutstandingTrans.SelectedRow.FindControl("chkeachout"), CheckBox)
        If chk2.Checked = True Then

            Dim oRequestMarkunmark As New MarkUnmarkTransactionRequestType
            Dim oResponseMarkunmark As New MarkUnmarkTransactionResponseType

            oRequestMarkunmark.BranchCode = gvOutstandingTrans.SelectedRow.Cells(32).Text
            oRequestMarkunmark.CurrencyCode = gvOutstandingTrans.SelectedRow.Cells(19).Text

            oRequestMarkunmark.MarkStatus = MarkStatusType.UnMark
            oRequestMarkunmark.PaymentAmount = 0 'Convert.ToDouble(txtpartPay.Text)

            oRequestMarkunmark.TransactionKey = Convert.ToInt32(gvOutstandingTrans.SelectedRow.Cells(13).Text)
            oResponseMarkunmark = oSAM.MarkUnmarkTransaction(oRequestMarkunmark)
        End If
    End Sub
    Public Sub mark()
        Dim oRequestMarkunmark As New MarkUnmarkTransactionRequestType
        Dim oResponseMarkunmark As New MarkUnmarkTransactionResponseType

        oRequestMarkunmark.BranchCode = gvOutstandingTrans.SelectedRow.Cells(32).Text
        oRequestMarkunmark.CurrencyCode = gvOutstandingTrans.SelectedRow.Cells(19).Text

        oRequestMarkunmark.MarkStatus = MarkStatusType.Mark
        oRequestMarkunmark.PaymentAmount = Convert.ToDouble(txtpartPay.Text)

        oRequestMarkunmark.TransactionKey = Convert.ToInt32(gvOutstandingTrans.SelectedRow.Cells(13).Text)
        oResponseMarkunmark = oSAM.MarkUnmarkTransaction(oRequestMarkunmark)
    End Sub
    Public Sub enablewriteoff()
        Dim visiblecount As Integer = 0
        Dim checkcount As Integer = 0

        For icnt As Integer = 0 To gvOutstandingTrans.Rows.Count - 1
            Dim ChkgvOutstandingTrans As New CheckBox
            ChkgvOutstandingTrans = gvOutstandingTrans.Rows(icnt).FindControl("chkeachout")
            If gvOutstandingTrans.Rows(icnt).Visible = True Then
                visiblecount = visiblecount + 1
            End If
            If gvOutstandingTrans.Rows(icnt).Visible = True And ChkgvOutstandingTrans.Checked = True Then
                checkcount = checkcount + 1
            End If
        Next

        If visiblecount = checkcount Then
            btnWRITEOFF.Enabled = True
        Else
            btnWRITEOFF.Enabled = False
        End If
    End Sub
   
    Protected Sub btnNewsearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewsearch.Click
        gvTransactions.DataSource = Nothing
        gvTransactions.Visible = False
        gvOutstandingTrans.DataSource = Nothing
        gvTest.DataSource = Nothing
        gvFinal.DataSource = Nothing
        txtShortName.Text = String.Empty
        Session("AccountCode") = Nothing
        txtTotalMarked.Text = String.Empty
        txtTotalwriteoff.Text = String.Empty
        'txtDateTo.Text = String.Empty
    End Sub

    Protected Sub btnMark_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMark.Click
        For i As Integer = 0 To gvTransactions.Rows.Count - 1
            Dim oRequestMarkunmark As New MarkUnmarkTransactionRequestType
            Dim oResponseMarkunmark As New MarkUnmarkTransactionResponseType
            oRequestMarkunmark.BranchCode = gvTransactions.Rows(i).Cells(32).Text
            oRequestMarkunmark.CurrencyCode = gvTransactions.Rows(i).Cells(19).Text
            Dim chk3 As CheckBox
            chk3 = DirectCast(gvTransactions.Rows(i).FindControl("chk"), CheckBox)
            If chk3.Checked = True Then
                
                
                oRequestMarkunmark.MarkStatus = MarkStatusType.UnMark
                oRequestMarkunmark.PaymentAmount = 0.0
                oRequestMarkunmark.TransactionKey = Convert.ToDouble(gvTransactions.Rows(i).Cells(13).Text)
                oResponseMarkunmark = oSAM.MarkUnmarkTransaction(oRequestMarkunmark)

                
                oRequestMarkunmark.MarkStatus = MarkStatusType.Mark 'gvTransactions.Rows(i).Cells(0).Text
                oRequestMarkunmark.PaymentAmount = Convert.ToDouble(gvTransactions.Rows(i).Cells(8).Text) - Convert.ToDouble(gvTransactions.Rows(i).Cells(9).Text)
                oRequestMarkunmark.TransactionKey = Convert.ToDouble(gvTransactions.Rows(i).Cells(13).Text)
                oResponseMarkunmark = oSAM.MarkUnmarkTransaction(oRequestMarkunmark)
            Else
                oRequestMarkunmark.MarkStatus = MarkStatusType.Mark 'gvTransactions.Rows(i).Cells(0).Text
                oRequestMarkunmark.PaymentAmount = Convert.ToDouble(gvTransactions.Rows(i).Cells(8).Text) - Convert.ToDouble(gvTransactions.Rows(i).Cells(9).Text)
                oRequestMarkunmark.TransactionKey = Convert.ToDouble(gvTransactions.Rows(i).Cells(13).Text)
                oResponseMarkunmark = oSAM.MarkUnmarkTransaction(oRequestMarkunmark)
            End If
            
        Next
        getinsurerPayment()
    End Sub

    Protected Sub chkDateTo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDateTo.CheckedChanged
        If chkDateTo.Checked = True Then
            txtDateTo.Enabled = True
        Else
            txtDateTo.Enabled = False
        End If
    End Sub
End Class
