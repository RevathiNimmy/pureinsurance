Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Linq

Namespace Nexus
    Partial Class secure_payment_ClaimPaymentProcessing : Inherits CMS.Library.Frontend.clsCMSPage

        Dim oWebService As NexusProvider.ProviderBase
        Dim oAccountSearchResultCollection As NexusProvider.AccountSearchResultCollection
        Dim bSelectAll As Boolean

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                'clear the session variable
                ClearQuote()
                ClearClaims()
                btnAccountCode.Attributes.Add("onkeypress", "javascript:return DisplayControl();")
                txtAccountCode.Attributes.Add("autocomplete", "off")
                txtAccountCode.Focus()

            End If
        End Sub

        Protected Sub btnFindNow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
            If UserCanDoTask("ClaimPaymentProcessing") Then
                If Page.IsValid Then
                    Dim iResult, iAccountKey As Integer
                    Dim dPaymentDate As Date = IIf(String.IsNullOrEmpty(txtPaymentDate.Text), Nothing, txtPaymentDate.Text)
                    Dim dPaymentDateTo As Date = IIf(String.IsNullOrEmpty(txtPaymentDateTo.Text), Nothing, txtPaymentDateTo.Text)
                    Dim sShortCode As String = Nothing
                    Dim oUnallocatedClaimPaymentsCollection As NexusProvider.UnallocatedClaimPaymentsCollection = Nothing

                    Dim sBtnAccountCode_OnClientClick As String = String.Empty
                    sBtnAccountCode_OnClientClick = "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){" + btnAccountCode.OnClientClick.Replace("?", "?shortCode=" + txtAccountCode.Text.Trim() + "&") + "});</script>"

                    If (txtAccountCode.Text.Trim().Length > 0 AndAlso txtAccountCode.Text.Trim().Contains("%")) Then
                        Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show", sBtnAccountCode_OnClientClick)
                        Exit Sub
                    End If

                    If GetAccountDetails() = False Then
                        Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show", sBtnAccountCode_OnClientClick)
                        Exit Sub
                    End If

                    oWebService = New NexusProvider.ProviderManager().Provider

                    If String.IsNullOrEmpty(txtPaymentDate.Text) = False Or String.IsNullOrEmpty(txtPaymentDateTo.Text) = False Then
                        If String.IsNullOrEmpty(txtPaymentDate.Text) = True Or String.IsNullOrEmpty(txtPaymentDateTo.Text) = True Then
                            IsUnAllocatedPaymentExist.IsValid = False
                            IsUnAllocatedPaymentExist.ErrorMessage = GetLocalResourceObject("lbl_PaymentDate_err").ToString()
                        Else
                            oUnallocatedClaimPaymentsCollection = oWebService.GetUnallocatedClaimPayment(iAccountKey, dPaymentDate, dPaymentDateTo)

                            ViewState("UnAllocatedClaimPayments") = oUnallocatedClaimPaymentsCollection
                            grdUnAllocatedClaimPayment.Visible = True
                            grdUnAllocatedClaimPayment.AllowPaging = True
                            grdUnAllocatedClaimPayment.DataSource = oUnallocatedClaimPaymentsCollection
                            grdUnAllocatedClaimPayment.DataBind()
                        End If

                    ElseIf String.IsNullOrEmpty(txtAccountCode.Text) = False Then

                        sShortCode = txtAccountCode.Text
                        oUnallocatedClaimPaymentsCollection = oWebService.GetUnallocatedClaimPayment(iAccountKey, dPaymentDate, dPaymentDateTo, sShortCode)

                        ViewState("UnAllocatedClaimPayments") = oUnallocatedClaimPaymentsCollection
                        grdUnAllocatedClaimPayment.AllowPaging = True
                        grdUnAllocatedClaimPayment.Visible = True
                        grdUnAllocatedClaimPayment.DataSource = oUnallocatedClaimPaymentsCollection
                        grdUnAllocatedClaimPayment.DataBind()

                    ElseIf String.IsNullOrEmpty(hiddenAccountKey.Value) = False Then
                        If Integer.TryParse(hiddenAccountKey.Value, iResult) = True _
                        And String.IsNullOrEmpty(txtPaymentDate.Text) = True And String.IsNullOrEmpty(txtPaymentDateTo.Text) = True Then

                            If hiddenAccountKey.Value <> "" And hiddenAccountKey.Value <> "0" Then
                                iAccountKey = hiddenAccountKey.Value
                            Else
                                IsUnAllocatedPaymentExist.IsValid = False
                                IsUnAllocatedPaymentExist.ErrorMessage = GetLocalResourceObject("lbl_UnAllocatedPaymentExist_error").ToString()
                                Exit Sub
                            End If
                            oUnallocatedClaimPaymentsCollection = oWebService.GetUnallocatedClaimPayment(iAccountKey, dPaymentDate, dPaymentDateTo)

                            ViewState("UnAllocatedClaimPayments") = oUnallocatedClaimPaymentsCollection
                            grdUnAllocatedClaimPayment.AllowPaging = True
                            grdUnAllocatedClaimPayment.Visible = True
                            grdUnAllocatedClaimPayment.DataSource = oUnallocatedClaimPaymentsCollection
                            grdUnAllocatedClaimPayment.DataBind()

                        Else
                            IsUnAllocatedPaymentExist.IsValid = False
                            IsUnAllocatedPaymentExist.ErrorMessage = GetLocalResourceObject("lbl_UnAllocatedPayment_error").ToString()
                        End If
                    Else
                        IsUnAllocatedPaymentExist.IsValid = False
                        IsUnAllocatedPaymentExist.ErrorMessage = GetLocalResourceObject("lbl_UnAllocatedPayment_error").ToString()
                        Exit Sub
                    End If
                End If

                'Enable/Disable the fields
                If txtAccountCode.Text.Trim.Length <> 0 Then
                    txtPaymentDate.Enabled = False
                    PaymentDate_CalendarLookup.Enabled = False
                ElseIf txtPaymentDate.Text.Trim.Length <> 0 Then
                    txtAccountCode.Enabled = False
                    btnAccountCode.Enabled = False
                End If
            End If
        End Sub

        Protected Function GetAccountDetails() As Boolean
            Dim bStatus As Boolean = True
            Dim oAccountSearchCriteria As New NexusProvider.AccountSearchCriteria
            oWebService = New NexusProvider.ProviderManager().Provider

            If Not txtAccountCode.Text.Trim.Length() = 0 Then

                oAccountSearchCriteria.ShortCode = txtAccountCode.Text.Trim()

                oAccountSearchResultCollection = oWebService.FindAccounts(oAccountSearchCriteria)

                'check if oAccountSearchResultCollection is NOT Nothing then assign the values to controls
                If oAccountSearchResultCollection IsNot Nothing Then
                    txtAccountName.Text = oAccountSearchResultCollection(0).AccountName.Trim()
                    hiddenAccountKey.Value = oAccountSearchResultCollection(0).PartyKey
                    txtAccountCode.Focus()
                Else
                    IsUnAllocatedPaymentExist.IsValid = False
                    bStatus = False
                End If
            End If

            'cleaning up
            oWebService = Nothing
            oAccountSearchCriteria = Nothing
            oAccountSearchResultCollection = Nothing

            Return bStatus
        End Function

        Protected Sub grdUnAllocatedClaimPayment_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdUnAllocatedClaimPayment.DataBound
            If CType(sender, GridView).PageCount < 2 Then
                CType(sender, GridView).AllowPaging = False
            End If
        End Sub

        Protected Sub grdUnAllocatedClaimPayment_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdUnAllocatedClaimPayment.PageIndexChanging
            grdUnAllocatedClaimPayment.PageIndex = e.NewPageIndex
            Dim oUnallocatedClaimPaymentsCollection As NexusProvider.UnallocatedClaimPaymentsCollection = CType(ViewState("UnAllocatedClaimPayments"), NexusProvider.UnallocatedClaimPaymentsCollection)
            grdUnAllocatedClaimPayment.DataSource = oUnallocatedClaimPaymentsCollection
            grdUnAllocatedClaimPayment.DataBind()
            Dim oSelectedAll = From oSelectedAllPayments In oUnallocatedClaimPaymentsCollection Where oSelectedAllPayments.IsSelected = False
            If oSelectedAll.Count = 0 Then
                CType(grdUnAllocatedClaimPayment.HeaderRow.FindControl("chkHeader"), CheckBox).Checked = True
            End If
        End Sub

        Protected Sub btnNewSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewSearch.Click
            ' clearing all the values for entering new search criteria
            txtAccountCode.Text = String.Empty
            txtAccountName.Text = String.Empty
            txtPaymentDate.Text = String.Empty
            txtPaymentDateTo.Text = String.Empty
            hiddenAccountKey.Value = Nothing
            grdUnAllocatedClaimPayment.DataSource = Nothing
            grdUnAllocatedClaimPayment.DataBind()
            grdUnAllocatedClaimPayment.Visible = False
            ViewState("UnAllocatedClaimPayments") = Nothing
            txtPaymentDate.Enabled = True
            PaymentDate_CalendarLookup.Enabled = True
            btnAccountCode.Enabled = True
            txtAccountCode.Enabled = True
        End Sub

        Protected Sub grdUnAllocatedClaimPayment_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdUnAllocatedClaimPayment.RowCommand
            If e.CommandName = "Pay" Then
                Dim oUnallocatedClaimPaymentsCollection As NexusProvider.UnallocatedClaimPaymentsCollection = CType(ViewState("UnAllocatedClaimPayments"), NexusProvider.UnallocatedClaimPaymentsCollection)
                Dim oSelectedPayment = From oSelectedPayments In oUnallocatedClaimPaymentsCollection Where oSelectedPayments.ClaimPaymentKey = e.CommandArgument
                If oSelectedPayment.Count > 0 Then
                    Session(CNUnAllocatedClaimPayment) = oSelectedPayment.ElementAt(0)
                    Session(CNCurrenyCode) = GetCodeForKey(NexusProvider.ListType.PMLookup, oSelectedPayment.ElementAt(0).CurrencyKey, "Currency", True)
                End If
                Session("CPP") = "true"
                Response.Redirect("~/secure/payment/CashListNew.aspx", False)
            End If
        End Sub

        Protected Sub grdUnAllocatedClaimPayment_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdUnAllocatedClaimPayment.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("BaseClaimPaymentKey") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.UnallocatedClaimPayments).BaseClaimPaymentKey)

                Dim dCurrencyAmount As Decimal = CType(e.Row.DataItem, NexusProvider.UnallocatedClaimPayments).CurrencyAmount
                Dim dBaseAmount As Decimal = CType(e.Row.DataItem, NexusProvider.UnallocatedClaimPayments).AccountAmount

                e.Row.Cells(5).Text = FormatNumber((dCurrencyAmount * -1), 2)
                e.Row.Cells(7).Text = FormatNumber((dBaseAmount * -1), 2)
                CType(e.Row.Cells(0).FindControl("chkRow"), CheckBox).Checked = CType(e.Row.DataItem, NexusProvider.UnallocatedClaimPayments).IsSelected
            ElseIf e.Row.RowType = DataControlRowType.Header Then
                CType(e.Row.Cells(0).FindControl("chkHeader"), CheckBox).Checked = bSelectAll
            End If
        End Sub

        ''' <summary>
        ''' Handles button SettleAll click event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnSettleAll_Click(sender As Object, e As EventArgs) Handles btnSettleAll.Click
            If Page.IsValid Then
                Dim oUnallocatedClaimPaymentsCollection As NexusProvider.UnallocatedClaimPaymentsCollection = CType(ViewState("UnAllocatedClaimPayments"), NexusProvider.UnallocatedClaimPaymentsCollection)
                Dim oSelectedPayment = From oSelected In oUnallocatedClaimPaymentsCollection Where oSelected.IsSelected = True

                If oSelectedPayment.Count > 0 Then
                    Dim oRequestCollection As New NexusProvider.UnallocatedClaimPaymentsCollection
                    Dim oRequestItem As NexusProvider.UnallocatedClaimPayments

                    For Each row In oSelectedPayment
                        oRequestItem = New NexusProvider.UnallocatedClaimPayments
                        With oRequestItem
                            .AccountAmount = row.AccountAmount
                            .AccountCode = row.AccountCode.ToString.Trim
                            .AccountCurrencyKey = row.AccountCurrencyKey
                            .AccountKey = row.AccountKey
                            .AccountName = row.AccountName.ToString.Trim
                            .Amount = row.Amount
                            .AmountCurrencyKey = row.AmountCurrencyKey
                            .BankAccountCode = row.BankAccountCode.ToString.Trim
                            .BaseClaimPaymentKey = row.BaseClaimPaymentKey
                            .BaseCurrencyDescription = row.BaseCurrencyDescription
                            .BaseCurrencyFormatString = row.BaseCurrencyFormatString
                            .ClaimPaymentBranchCode = row.ClaimPaymentBranchCode.ToString.Trim
                            .ClaimPaymentKey = row.ClaimPaymentKey
                            .CashListItemKey = row.CashListItemKey
                            .ClaimNumber = row.ClaimNumber
                            .CurrencyAmount = row.CurrencyAmount
                            .CurrencyCode = row.CurrencyCode.ToString.Trim
                            .CurrencyDescription = row.CurrencyDescription
                            .CurrencyFormatString = row.CurrencyFormatString
                            .CurrencyKey = row.CurrencyKey
                            .DateOfPayment = row.DateOfPayment
                            .DocumentComment = row.DocumentComment
                            .DocumentDate = row.DocumentDate
                            .DocumentKey = row.DocumentKey
                            .DocumentRef = row.DocumentRef
                            .IsSelected = row.IsSelected
                            .MaxClaimPaymentKey = row.MaxClaimPaymentKey
                            .MediaTypeCode = row.MediaTypeCode.ToString.Trim
                            .OurRef = row.OurRef
                            .PayeeMediaTypeKey = row.PayeeMediaTypeKey
                            .PayeeName = row.PayeeName
                            .Status = row.Status
                            .AmountCurrencyCode = GetCodeForKey(NexusProvider.ListType.PMLookup, row.AmountCurrencyKey, "Currency", True)
                        End With
                        oRequestCollection.Add(oRequestItem)
                    Next
                    Dim oSettleAllClaimPayments As NexusProvider.SettleAllClaimPaymentsResults
                    oWebService = New NexusProvider.ProviderManager().Provider
                    oSettleAllClaimPayments = oWebService.SettleAllClaimPayments(oRequestCollection)
                    Session(CNClaimPaymentSummary) = oSettleAllClaimPayments
                    If oSettleAllClaimPayments.Summary IsNot Nothing Then
                        Dim sUrl As String
                        If HttpContext.Current.Session.IsCookieless Then
                            sUrl = "tb_show( null,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/ClaimPaymentSummary.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=700' , null);"
                        Else
                            sUrl = "tb_show( null,'" & AppSettings("WebRoot") & "/Modal/ClaimPaymentSummary.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=700' , null);"
                        End If
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Summary", sUrl, True)
                    End If
                    btnSettleAll.Enabled = False
                    btnFindNow_Click(sender, e)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Handle grid header row checked change event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub chkHeader_OnCheckedChanged(sender As Object, e As EventArgs)
            bSelectAll = DirectCast(sender, CheckBox).Checked
            Dim oUnallocatedClaimPaymentsCollection As NexusProvider.UnallocatedClaimPaymentsCollection = CType(ViewState("UnAllocatedClaimPayments"), NexusProvider.UnallocatedClaimPaymentsCollection)
            For Each oPayment As NexusProvider.UnallocatedClaimPayments In oUnallocatedClaimPaymentsCollection
                oPayment.IsSelected = bSelectAll
            Next
            grdUnAllocatedClaimPayment.DataSource = oUnallocatedClaimPaymentsCollection
            grdUnAllocatedClaimPayment.DataBind()
            If oUnallocatedClaimPaymentsCollection IsNot Nothing AndAlso oUnallocatedClaimPaymentsCollection.Count > 0 Then
                Dim oSelectedPayment = From oSelected In oUnallocatedClaimPaymentsCollection Where oSelected.IsSelected = True
                If oSelectedPayment.Count > 0 Then
                    btnSettleAll.Enabled = True
                Else
                    btnSettleAll.Enabled = False
                End If
            End If
        End Sub

        ''' <summary>
        ''' Handle grid row checked change event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub chkRow_OnCheckedChanged(sender As Object, e As EventArgs)
            Dim bSelect As Boolean = DirectCast(sender, CheckBox).Checked
            Dim iRecordNumber As Integer = DirectCast(DirectCast(sender, System.Web.UI.WebControls.CheckBox).BindingContainer, System.Web.UI.WebControls.GridViewRow).RowIndex + (grdUnAllocatedClaimPayment.PageIndex * grdUnAllocatedClaimPayment.PageSize)
            Dim oUnallocatedClaimPaymentsCollection As NexusProvider.UnallocatedClaimPaymentsCollection = CType(ViewState("UnAllocatedClaimPayments"), NexusProvider.UnallocatedClaimPaymentsCollection)
            oUnallocatedClaimPaymentsCollection.Item(iRecordNumber).IsSelected = bSelect
            Dim oSelectedPaymentAll = From oSelected In oUnallocatedClaimPaymentsCollection Where oSelected.IsSelected = True
            If oSelectedPaymentAll.Count > 0 Then
                btnSettleAll.Enabled = True
            Else
                btnSettleAll.Enabled = False
            End If
        End Sub

        ''' <summary>
        ''' Validate the payments against status, MediaType and BankAccount
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="args"></param>
        ''' <remarks></remarks>
        Protected Sub cvSettleAll_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cvSettleAll.ServerValidate
            Dim oUnallocatedClaimPaymentsCollection As NexusProvider.UnallocatedClaimPaymentsCollection = CType(ViewState("UnAllocatedClaimPayments"), NexusProvider.UnallocatedClaimPaymentsCollection)
            If oUnallocatedClaimPaymentsCollection IsNot Nothing Then
                Dim oSelectedPayment = From oSelected In oUnallocatedClaimPaymentsCollection Where oSelected.IsSelected = True

                If oSelectedPayment.Count > 0 Then
                    Dim strStatus As String = "Awaiting Settlement"
                    Dim oSelectedPaymentFinal = From oSelected2 In oSelectedPayment Where (oSelected2.status <> strStatus Or oSelected2.MediaTypeCode = "" Or oSelected2.BankAccountCode = "")
                    If oSelectedPaymentFinal.Count > 0 Then
                        args.IsValid = False
                        cvSettleAll.ErrorMessage = GetLocalResourceObject("lbl_SettleAll_error").ToString()
                    End If
                Else
                    args.IsValid = False
                    cvSettleAll.ErrorMessage = GetLocalResourceObject("lbl_SettleAll_error").ToString()
                End If
            End If

        End Sub

        Protected Sub grdUnAllocatedClaimPayment_Sorting(sender As Object, e As GridViewSortEventArgs) Handles grdUnAllocatedClaimPayment.Sorting
            Dim oUnallocatedClaimPaymentsCollection As NexusProvider.UnallocatedClaimPaymentsCollection = CType(ViewState("UnAllocatedClaimPayments"), NexusProvider.UnallocatedClaimPaymentsCollection)
            oUnallocatedClaimPaymentsCollection.SortColumn = e.SortExpression
            'check that the sort expression is the same as stored in viewstate as we should start again if reordering by a new column
            Dim _sortDirection As New SortDirection
            If ViewState("SortDirection") = SortDirection.Ascending And ViewState("SortExpression") = e.SortExpression Then
                _sortDirection = SortDirection.Descending
            Else
                _sortDirection = SortDirection.Ascending
            End If
            'store the current sortdirection for comparison on the next sort
            ViewState("SortDirection") = _sortDirection
            'store the SortExpression in viewstate so that we can check if we are sorting by a new column on the next sort
            ViewState("SortExpression") = e.SortExpression
            oUnallocatedClaimPaymentsCollection.SortingOrder = _sortDirection
            oUnallocatedClaimPaymentsCollection.Sort()
            
            CType(sender, GridView).DataSource = oUnallocatedClaimPaymentsCollection
            CType(sender, GridView).DataBind()
            
        End Sub
    End Class
End Namespace
