Imports CMS.Library
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports Nexus.Constants.Session
Imports Nexus.Library
Imports Nexus.Utils
Imports System.Web.Services

Namespace Nexus
    Partial Class AuthorizeManualJournal : Inherits Frontend.clsCMSPage
        Dim oWebservice As NexusProvider.ProviderBase
        Private oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Public Const sInterfaceName As String = "AuthorizeManualJournalPayment"
        Dim sColumnList As String = String.Empty
#Region "Page Events"
        Private Sub AuthorizeManualJournal_Init(sender As Object, e As EventArgs) Handles Me.Init
            txtDateTo.Text = DateTime.Now.ToString("d")
            '------------------document type ddl----------------------
            Dim oLookupCollection As NexusProvider.LookupListCollection
            oLookupCollection = PMLookup_DocumentType.Items
            ddlDocumentType.DataSource = oLookupCollection
            ddlDocumentType.DataTextField = "Description"
            ddlDocumentType.DataValueField = "Code"
            ddlDocumentType.DataBind()
            ddlDocumentType.Items.Insert(0, "All")
            '------------------Payment type ddl----------------------
        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If HttpContext.Current.Session.IsCookieless Then
                btnAccountCode.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/FindAccount.aspx?modal=true&KeepThis=true&FromPage=ACC&TB_iframe=true&height=500&width=800' , null);return false;"
            Else
                btnAccountCode.OnClientClick = "tb_show(null ,'../Modal/FindAccount.aspx?modal=true&KeepThis=true&FromPage=ACC&TB_iframe=true&height=500&width=800' , null);return false;"
            End If
        End Sub
        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                Dim AuthoriseManualJournalTransactionspageCacheID As Guid
                AuthoriseManualJournalTransactionspageCacheID = Guid.NewGuid()
                rvtxtdatefrom.MaximumValue = Now.Date
                rvtxtDateTo.MaximumValue = DateTime.Now.Date.AddYears(100).ToString("dd/MM/yyyy")
                ViewState.Add("AuthoriseManualJournalTransactionspageCacheID", AuthoriseManualJournalTransactionspageCacheID.ToString)
                ' PMLookup_DocumentType.Value = "JN"

                oWebservice = New NexusProvider.ProviderManager().Provider
                Dim oUserAuthority As New NexusProvider.UserAuthority With {
                    .UserCode = CType(Session(CNLoginName), String),
                    .UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.HasManualJournalAuthority
                }
                oWebservice.GetUserAuthorityValue(oUserAuthority)
                hdnJournalAuthoriser.Value = oUserAuthority.UserAuthorityValue
                hdnApprovalAuthorityLimit.Value = Convert.ToDecimal(oUserAuthority.UserAuthorityOptionalValue2)
                Try
                    If hdnJournalAuthoriser.Value = "0" Then
                        divHeader.Visible = False
                        divHidden.Visible = True
                        divSubmit.Visible = False
                    Else
                        Dim oSearchcritaria As Collection = CType(Session(CNSearchManualJournalTransactions), Collection)
                        If oSearchcritaria IsNot Nothing Then
                            txtDateFrom.Text = oSearchcritaria.Item(txtDateFrom.ID)
                            txtDateTo.Text = oSearchcritaria.Item(txtDateTo.ID)
                            ddlDocumentType.SelectedValue = oSearchcritaria.Item(ddlDocumentType.ID)
                            txtAccountCode.Text = oSearchcritaria.Item(txtAccountCode.ID)
                            GridRefresh()
                        End If
                    End If

                Catch ex As System.Exception
                Finally
                    oWebservice = Nothing
                    oUserAuthority = Nothing
                End Try
            End If

        End Sub
#End Region

#Region "Button Events"

        Public Sub BtnFindNow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
            If Page.IsValid Then
                grdManualJournalTransactions.PageIndex = 0
                GridRefresh()
                'ClearSearch()
                Dim oSearchcritaria As New Collection From {
                    {txtDateFrom.Text.Trim, txtDateFrom.ID},
                    {txtDateTo.Text.Trim, txtDateTo.ID},
                    {ddlDocumentType.SelectedValue.Trim, ddlDocumentType.ID},
                    {txtAccountCode.Text.Trim, txtAccountCode.ID}
                }
                Session(CNSearchManualJournalTransactions) = oSearchcritaria
            End If
        End Sub

        Protected Sub BtnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
            Session.Remove(CNSearchManualJournalTransactions)
            Session.Remove(CNAuthoriseManualJournalTransactions)
            txtAccountCode.Text = String.Empty
            ddlDocumentType.SelectedIndex = "0"
            txtDateFrom.Text = String.Empty
            txtDateTo.Text = DateTime.Now.ToString("d")

            grdManualJournalTransactions.Visible = False
        End Sub

        Sub GridRefresh()
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oManualJournalTransactionRequestType = New NexusProvider.AuthorisedManualJournalTransactionRequestType
            Try
                If txtDateFrom.Text.Trim.Length <> 0 And IsDate(txtDateFrom.Text.Trim) = True Then
                    oManualJournalTransactionRequestType.DateFrom = CDate(txtDateFrom.Text.Trim)
                End If
                If txtDateFrom.Text.Trim.Length <> 0 Then
                    oManualJournalTransactionRequestType.DateFrom = CDate(txtDateFrom.Text.Trim)
                Else
                    oManualJournalTransactionRequestType.DateFrom = Nothing
                End If
                If txtDateTo.Text.Trim.Length <> 0 Then
                    oManualJournalTransactionRequestType.DateTo = CDate(txtDateTo.Text.Trim)
                Else
                    oManualJournalTransactionRequestType.DateTo = Nothing
                End If
                If txtAccountCode.Text.Trim.Length <> 0 Then
                    oManualJournalTransactionRequestType.AccountCode = txtAccountCode.Text.Trim
                Else
                    oManualJournalTransactionRequestType.AccountCode = Nothing
                End If
                If ddlDocumentType.SelectedValue.Trim.Length <> 0 Then
                    oManualJournalTransactionRequestType.JournalTypeCode = ddlDocumentType.SelectedValue.Trim
                Else
                    oManualJournalTransactionRequestType.JournalTypeCode = Nothing
                End If

                Dim oManualJournalTransactionsCollection As NexusProvider.ManualJournalTransactionsCollection = oWebService.GetListofManualJournalTransactions(oManualJournalTransactionRequestType)


                grdManualJournalTransactions.Visible = True
                grdManualJournalTransactions.AllowPaging = True
                If (oNexusConfig.FinanceGridSize) = "" Then
                    grdManualJournalTransactions.PageSize = 10
                Else
                    grdManualJournalTransactions.PageSize = CStr(oNexusConfig.FinanceGridSize)
                End If
                grdManualJournalTransactions.DataSource = oManualJournalTransactionsCollection
                grdManualJournalTransactions.DataBind()
                ColumnSelectorExtender1.Visible = True
                ScriptManager.RegisterStartupScript(Page, Me.GetType(), "id", "$get('" + ColumnSelectorExtender1.ClientID + "').style.display='block';", True)
                Cache.Insert(ViewState("AuthoriseManualJournalTransactionspageCacheID"), oManualJournalTransactionsCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
            Catch ex As System.Exception
                Throw
            Finally

            End Try

        End Sub

#End Region

#Region "Grid Events"

        ''' <summary>
        ''' Authorise Claim Payment Grid Data Bound
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub GrdManualJournalTransactions_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdManualJournalTransactions.DataBound
            If grdManualJournalTransactions.Rows.Count = 0 Or grdManualJournalTransactions.PageCount = 1 Then
                grdManualJournalTransactions.AllowPaging = False

            End If
            If (grdManualJournalTransactions.Rows.Count > 0) Then
                'grdManualJournalTransactions.HeaderRow.Cells(0).Visible = False
                ' grdManualJournalTransactions.Columns(0).Visible = False



            End If
        End Sub

        Protected Sub GrdManualJournalTransactions_RowDataBound1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdManualJournalTransactions.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim oItem As NexusProvider.AuthorisedManualJournalTransactionsList = CType(e.Row.DataItem, NexusProvider.AuthorisedManualJournalTransactionsList)
                e.Row.Cells(1).Text = New Money(oItem.Amount, oItem.CurrencyCode.Trim).Formatted
                e.Row.Cells(2).Text = New Money(oItem.CurrencyRate, oItem.CurrencyCode.Trim).Formatted
                e.Row.Cells(3).Text = New Money(oItem.BaseAmount, oItem.CurrencyCode.Trim).Formatted
                If e.Row.Cells(6).Text.Trim() = "0" Then
                    e.Row.Cells(6).Text = String.Empty
                End If
            End If
        End Sub

        Private Sub GrdManualJournalTransactions_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdManualJournalTransactions.RowCommand
            Dim manualJournalMasterKey As Integer
            Dim mode As String = String.Empty
            Dim commandArgs As String()
            Dim dApproverLimitValue As Decimal
            If e.CommandName <> "Page" AndAlso e.CommandName <> "Sort" Then
                commandArgs = e.CommandArgument.ToString().Split(New Char() {","c})
                dApproverLimitValue = Convert.ToDecimal(hdnApprovalAuthorityLimit.Value.Trim)

                manualJournalMasterKey = CInt(commandArgs(0))

                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

                Dim oManualJournal = New NexusProvider.ManualJournal
                oManualJournal.ManualJournalId = manualJournalMasterKey


                Dim sCreatedBy As String = commandArgs(2)

                If e.CommandName = "Authorise" Then
                    oManualJournal.Approved = 1
                    mode = "A"
                ElseIf e.CommandName = "View" Then
                    mode = "V"
                ElseIf e.CommandName = "Decline" Then
                    mode = "D"
                    oManualJournal.Approved = 0
                End If

                If mode = "A" OrElse mode = "D" Then
                    Dim oManualJournalCollection = New NexusProvider.ManualJournalCollection
                    oManualJournalCollection = oWebService.ValidateAuthorizationSteps(oManualJournal)
                    If (oManualJournalCollection.Item(0).ValidationMessage <> "") Then
                        Dim sErrorMessage = GetLocalResourceObject(oManualJournalCollection.Item(0).ValidationMessage.ToString)
                        sErrorMessage = sErrorMessage.ToString.Replace("{amount}", oManualJournalCollection.Item(0).JournalAmount)
                        sErrorMessage = sErrorMessage.ToString.Replace("{Authlimit}", dApproverLimitValue.ToString("N2"))
                        'look for a validation message in the page resources, but if there is not one defined add a default message
                        Dim cstUserAuthorisation As New CustomValidator With {
                            .IsValid = False,
                            .ErrorMessage = sErrorMessage,
                            .Display = ValidatorDisplay.None
                            }
                        'add the validator to the page, this will have the effect of making the page invalid
                        Page.Validators.Add(cstUserAuthorisation)
                        Exit Sub
                    End If
                End If

                Response.Redirect("~/secure/ManualJournal.aspx?Type=Task&ManualJournalKey=" & manualJournalMasterKey & "&Mode=" & mode & "")
            End If
        End Sub

        ''' <summary>
        ''' Page Index Change Event For Grid View
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub GrdManualJournalTransactions_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdManualJournalTransactions.PageIndexChanging
            grdManualJournalTransactions.PageIndex = e.NewPageIndex
            If (oNexusConfig.FinanceGridSize) = "" Then
                grdManualJournalTransactions.PageSize = 10
            Else
                grdManualJournalTransactions.PageSize = CStr(oNexusConfig.FinanceGridSize)
            End If
            grdManualJournalTransactions.DataSource = CType(Cache.Item(ViewState("AuthoriseManualJournalTransactionspageCacheID")), NexusProvider.ManualJournalTransactionsCollection)
            grdManualJournalTransactions.DataBind()
        End Sub
        ''' <summary>
        ''' sort the Authorise manual journal transactions grid according to column click.
        ''' we need to store the current sort order in viewstate, and reverse it each time.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub GrdManualJournalTransactions_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdManualJournalTransactions.Sorting

            Dim oTransactionItems As NexusProvider.ManualJournalTransactionsCollection = Cache.Item(ViewState("AuthoriseManualJournalTransactionspageCacheID"))
            oTransactionItems.SortColumn = e.SortExpression
            'check that the sort expression is the same as stored in viewstate as we should start again if reordering by a new column
            Dim _sortDirection As SortDirection
            If ViewState("SortDirection") = SortDirection.Ascending And ViewState("SortExpression") = e.SortExpression Then
                _sortDirection = SortDirection.Descending
            Else
                _sortDirection = SortDirection.Ascending
            End If
            'store the current sortdirection for comparison on the next sort
            ViewState("SortDirection") = _sortDirection
            'store the SortExpression in viewstate so that we can check if we are sorting by a new column on the next sort
            ViewState("SortExpression") = e.SortExpression
            oTransactionItems.SortingOrder = _sortDirection
            oTransactionItems.Sort()
            If (oNexusConfig.FinanceGridSize) = "" Then
                CType(sender, GridView).PageSize = 10
            Else
                CType(sender, GridView).PageSize = CStr(oNexusConfig.FinanceGridSize)
            End If
            CType(sender, GridView).DataSource = oTransactionItems
            CType(sender, GridView).DataBind()
        End Sub

#End Region

        Public Sub GetUserPreferredColumnList()
            ColumnSelectorExtender1.Visible = True

            Dim sBranchCode As String = Session(CNBranchCode).ToString()
            Dim oUserPreferredColumns As NexusProvider.UserPreferredColumnList
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            oUserPreferredColumns = New NexusProvider.UserPreferredColumnList

            oUserPreferredColumns = oWebService.GetUserPreferredColumnList(sBranchCode, sInterfaceName)
            sColumnList = oUserPreferredColumns.ColumnList


            If (sColumnList IsNot Nothing) Then
                Dim sColumnListCollection() As String = sColumnList.Split(",")
                For i As Integer = 0 To grdManualJournalTransactions.Columns.Count - 1
                    If grdManualJournalTransactions.Columns.Item(i).HeaderText.ToString <> "" Then
                        grdManualJournalTransactions.Columns.Item(i).Visible = False
                        For colCnt As Integer = 0 To sColumnListCollection.Length - 1
                            If sColumnListCollection(colCnt) = grdManualJournalTransactions.Columns.Item(i).HeaderText Then
                                grdManualJournalTransactions.Columns.Item(i).Visible = True
                            End If
                        Next
                    End If
                Next
            End If

        End Sub
        Public Sub UpdateUserPreferredColumnList()
            Dim oSamProvider As NexusProvider.SAMForInsurance.ProviderSAMForInsuranceV2 = New NexusProvider.SAMForInsurance.ProviderSAMForInsuranceV2()
            Dim oUserPreferredColumns As NexusProvider.UserPreferredColumnList
            Dim sBranchCode As String = Session(CNBranchCode).ToString()
            oUserPreferredColumns = New NexusProvider.UserPreferredColumnList
            sColumnList = ""
            For i As Integer = 0 To ColumnSelectorExtender1.ColumnSelector.Items.Count - 1
                If ColumnSelectorExtender1.ColumnSelector.Items.Item(i).Selected Then
                    sColumnList = sColumnList & IIf(sColumnList = "", "", ",") & ColumnSelectorExtender1.ColumnSelector.Items.Item(i).Value
                End If
            Next

            oUserPreferredColumns.ColumnList = sColumnList
            oUserPreferredColumns.InterfaceName = sInterfaceName
            'oSearchTransactionColumn.IsSplitReceipt = .Item(SearchTransactionColumns.IsSplitReceipt).Selected


            oSamProvider.UpdateUserPreferredColumnList(oUserPreferredColumns, sBranchCode)
        End Sub
        Protected Sub CustVldDate_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles custVldDate.ServerValidate
            'Validation of Force from and Force to date
            If txtDateFrom.Text.Trim.Length <> 0 And IsDate(txtDateFrom.Text.Trim) = False Then
                args.IsValid = False
                custVldDate.ErrorMessage = GetLocalResourceObject("ErrMessage_InvalidDate")
            ElseIf txtDateTo.Text.Trim.Length <> 0 And IsDate(txtDateTo.Text.Trim) = False Then
                args.IsValid = False
                custVldDate.ErrorMessage = GetLocalResourceObject("ErrMessage_InvalidDate")
            ElseIf txtDateFrom.Text.Trim.Length <> 0 And txtDateTo.Text.Trim.Length <> 0 Then
                If CDate(txtDateFrom.Text.Trim) > CDate(txtDateTo.Text.Trim) Then
                    args.IsValid = False
                    custVldDate.ErrorMessage = GetLocalResourceObject("ErrMessage_InvalidDate")
                End If
            End If
        End Sub

        Private Sub AuthorizeManualJournal_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
            Dim sBranchCode As String = Session(CNBranchCode).ToString()
            ColumnSelectorExtender1.BranchCode = sBranchCode
            ColumnSelectorExtender1.Inetrface = sInterfaceName
            GetUserPreferredColumnList()
        End Sub
    End Class
End Namespace

