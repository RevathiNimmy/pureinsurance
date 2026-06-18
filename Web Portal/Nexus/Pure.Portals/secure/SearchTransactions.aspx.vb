Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports System.Web.Configuration
Imports Nexus.Library
Imports Nexus.Utils
Imports System.Web.HttpContext
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Collections.Generic
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls


Namespace Nexus

    Partial Class SearchTransactions : Inherits Frontend.clsCMSPage
        Private oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Public Const CNDocref As String = "DocumentRef"
        Public Const CNTransDeatilsKeys As String = "TRANSDETAIL_KEY"
        Public Const sInterfaceName As String = "SearchTransaction"
        Dim oAccountdetails As New NexusProvider.AccountDetails
        Dim oAccountDetailsCollection As NexusProvider.AccountDetailsDefaults = Nothing
        Dim oAccountSearchResultCollection As NexusProvider.AccountSearchResultCollection
        Dim arrlistTransid As New ArrayList
        Dim documentTypeDictionary As Dictionary(Of String, Integer) = Nothing
        Dim nAgentCnt As Integer = 0
        Dim sAccountKey As String = String.Empty
        Dim sColumnList As String = String.Empty

        ''' <summary>
        ''' Set the Nexus Config informations.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oOptionSettings As NexusProvider.OptionTypeSetting

            'If System Option for "Single Cash List receipt/payment per allocation" is ON then we will not allow with multiple SPR/SPY
            oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.SingleCashListItemPerAllocation)
            'Save this system option in cache. So that it can be used furter
            If oOptionSettings.OptionValue <> "" Then
                ViewState.Add("IsSingleCashListPaymentOrReciept", oOptionSettings.OptionValue(0).ToString())
            End If
        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            If Request.QueryString("Mode") = "CashListAllocation" Then
                CMS.Library.Frontend.Functions.SetTheme(Page, ConfigurationManager.AppSettings("ModalPageTemplate"))
            End If

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oOptionSettings As NexusProvider.OptionTypeSetting

            'If System Option for "Single Cash List receipt/payment per allocation" is ON then we will not allow with multiple SPR/SPY
            'Pass correct value instead of 1009. To get correct value, contact to prabodh
            oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 1009)
            If Request.QueryString("modal") = "true" And Request.QueryString("Mode") = "CashListAllocation" Then
                btnReverse.Enabled = False
            End If
            If btnReverse IsNot Nothing Then
                btnReverse.Attributes.Add("onclick", "javascript:return ReverseConfirmation();")
            End If
            Dim sBranchCode As String = Session(CNBranchCode).ToString()
            ColumnSelectorExtender.BranchCode = sBranchCode
            ColumnSelectorExtender.Inetrface = sInterfaceName
            GetUserPreferredColumnList()
        End Sub

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Session(CNMode) = Nothing
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            lblInstallmentTransectionAllocationError.Visible = False
            Dim oUser As NexusProvider.UserCollection = Nothing
            lblInstallmentTransectionAllocationError.Visible = False
            txtAccountBalance.Attributes.Add("readonly", "readonly")

            If Not IsNothing(Current.Session(Nexus.Constants.CNAgentDetails)) Then
                nAgentCnt = CType(Current.Session(Nexus.Constants.CNAgentDetails), NexusProvider.UserDetails).Key
            End If

            If IsNothing(documentTypeDictionary) Then
                documentTypeDictionary = New Dictionary(Of String, Integer)
                documentTypeDictionary.Add("RECEIPT", 0)
                documentTypeDictionary.Add("INSTALMENT CASH", 1)
                documentTypeDictionary.Add("INSTALMENT CREDIT", 2)
                documentTypeDictionary.Add("INSTALMENT RENEWAL CREDIT", 3)
                documentTypeDictionary.Add("INSTALMENT REINSTATEMENT CREDIT", 4)
                documentTypeDictionary.Add("INSTALMENT NB CREDIT", 5)
                documentTypeDictionary.Add("INSTALMENT ENDORSEMENT CREDIT", 6)
            End If

            If Not IsPostBack Then

                ViewState("ShowDrillDoc") = True
                'Populate the Branch
                If CType(Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches.Count > 1 Then
                    Dim oBranchs As NexusProvider.BranchCollection = CType(Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches
                    ddlBranchCode.DataSource = oBranchs
                    ddlBranchCode.DataBind()
                    If GetLocalResourceObject("lbl_BranchCodedefaultText").ToString().Trim.Length <> 0 Then
                        'if client has give any default value than only add
                        ddlBranchCode.Items.Insert(0, New ListItem(GetLocalResourceObject("lbl_BranchCodedefaultText"), ""))
                    End If

                    If Session(CNBranchCode) IsNot Nothing Then
                        If ddlBranchCode.Items.FindByValue(Session(CNBranchCode).ToString()) IsNot Nothing Then
                            ddlBranchCode.SelectedValue = Session(CNBranchCode).ToString()
                        Else
                            ' Cross-branch: session branch not in user list, add it to dropdown
                            ddlBranchCode.Items.Add(New ListItem(Session(CNBranchCode).ToString(), Session(CNBranchCode).ToString()))
                            ddlBranchCode.SelectedValue = Session(CNBranchCode).ToString()
                        End If
                    End If
                Else
                    liBranch.Visible = False
                End If

                'create a unique key and add this to viewstate
                'this will be used to cache the results of the SAM call
                Dim DocRefpageCacheID, TransactionpageCacheID, AccountpageCacheID, RollUpDocRefpageCacheID As Guid
                DocRefpageCacheID = Guid.NewGuid()
                ViewState.Add("DocRefpageCacheID", DocRefpageCacheID.ToString)

                TransactionpageCacheID = Guid.NewGuid()
                ViewState.Add("TransactionpageCacheID", TransactionpageCacheID.ToString)

                AccountpageCacheID = Guid.NewGuid()
                ViewState.Add("AccountpageCacheID", AccountpageCacheID.ToString)

                RollUpDocRefpageCacheID = Guid.NewGuid()
                ViewState.Add("RollUpDocRefpageCacheID", RollUpDocRefpageCacheID.ToString)

                oAccountSearchResultCollection = Nothing
                Session(CNResultSet) = Nothing

                'To set the Focus
                Page.SetFocus(btnAccountCode)

                'Populate the Operator Name 
                oUser = oWebService.GetUserGroupUsers(Nothing, DateTime.Now, False, False)
                ddlOperatorName.DataSource = oUser

                btnAllocate.Visible = False
                btnAllocate.Visible = False
                ddlOperatorName.DataTextField = "UserName"
                ddlOperatorName.DataValueField = "UserName"
                ddlOperatorName.DataBind()

                'default selected value
                If GetLocalResourceObject("ddl_OperatorNamelst_defaulttext").ToString().Trim.Length <> 0 Then
                    ddlOperatorName.Items.Insert(0, New ListItem(GetLocalResourceObject("ddl_OperatorNamelst_defaulttext"), "0"))
                End If

                If gvGetTransactiondetails.Visible = True Then
                    btnCancel.Visible = False
                End If

                'Changed default selection and set as per BO
                rbtAmountColumn.Items(0).Selected = True 'Amount Column TC
                rbtOutStandingColumn.Items(0).Selected = True 'Outstanding Amount Column TC

                'Populate the period
                Dim olist As NexusProvider.LookupListCollection
                olist = oWebService.GetAccountingPeriod()
                ddlPeriod.DataSource = olist
                ddlPeriod.DataTextField = "Description"
                ddlPeriod.DataValueField = "Key"
                ddlPeriod.DataBind()
                ddlPeriod.Items.Insert(0, New ListItem("(all)", "0"))

                'Load the records based on condition
                LoadRecords()

            End If

            'check if the postback has been triggered by the modal dialog
            If Request("__EVENTARGUMENT") = "RefreshAllocation" OrElse Request("__EVENTARGUMENT") = "SelectColumns" OrElse Request("__EVENTARGUMENT") = "RefreshReverseAllocation" Then
                GetTransactionDetails()
                btnAllocate.Visible = False
            End If

            GetAccountDetails()

        End Sub
        Sub LoadRecords()
            If UserCanDoTask("SearchTransaction") Then
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

                If Request.QueryString("Mode") = "IP" Then

                    With oAccountdetails
                        .DocumentRef = Session(CNDocumentRef)
                        txtDocumentRef.Text = Session(CNDocumentRef)
                        btnFindNow.Visible = True
                        btnNewsearch.Visible = True
                        btnCancel.Visible = True
                        hiddenAccountCode.Value = Session(CNAccountkey)
                        chkShowOutStandingTransaction.Checked = False
                        If chkShowOutStandingTransaction.Checked = True Then
                            .OutstandingOnly = True
                        End If
                        If chkIncludeFuturebalanceTransaction.Checked = True Then
                            .IsNewPF = True
                        End If
                        If chkShowOnlyLatest500Transactions.Checked = True Then
                            .Display500 = True
                        End If
                        If chkIncludeFuturebalanceTransaction.Checked = True Then
                            .IncludeReversedTran = True
                        End If
                        .BranchCode = ddlBranchCode.SelectedValue
                    End With

                    oAccountDetailsCollection = oWebService.GetAccountDetails(oAccountdetails, ddlBranchCode.SelectedValue)
                    For iCount As Integer = 0 To oAccountDetailsCollection.AccountDetails.Count - 1
                        If Not IsDate(oAccountDetailsCollection.AccountDetails(iCount).DueDate) Then
                            Dim dueDate = oAccountDetailsCollection.AccountDetails(iCount).DueDate
                            If dueDate Is Nothing OrElse dueDate = "" Then
                                oAccountDetailsCollection.AccountDetails(iCount).DueDate = oAccountDetailsCollection.AccountDetails(iCount).EffectiveDate
                            End If
                        End If
                    Next
                    Cache.Insert(ViewState("TransactionpageCacheID"), oAccountDetailsCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

                    gvGetTransactiondetails.AllowPaging = True
                    gvGetTransactiondetails.PageIndex = 0
                    If (oNexusConfig.FinanceGridSize) = "" Then
                        gvGetTransactiondetails.PageSize = 10
                    Else
                        gvGetTransactiondetails.PageSize = CStr(oNexusConfig.FinanceGridSize)
                    End If
                    gvGetTransactiondetails.DataSource = oAccountDetailsCollection.AccountDetails
                    gvGetTransactiondetails.DataBind()

                    ColumnSelectorExtender.Visible = True
                    'Done Parallel with Etana 3.1(This line was not in vb code)
                    ScriptManager.RegisterStartupScript(Page, Me.GetType(), "id", "$get('" + ColumnSelectorExtender.ClientID + "').style.display='block';", True)
                    Cache.Insert(ViewState("TransactionpageCacheID"), oAccountDetailsCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))


                    'txtAccountName.Text = oAccountDetailsCollection.AccountName
                    'txtAccountBalance.Text = oAccountDetailsCollection.AccountBalance
                    'txtContactName.Text = oAccountDetailsCollection.ContactName
                    'txtActive.Text = oAccountDetailsCollection.AccountStatus
                    'txtTelePhoneNumber.Text = oAccountDetailsCollection.PhoneNumber
                    'txtAccountCode.Text = oAccountDetailsCollection.AccountDetails(0).Account
                    'btnAccountCode.Enabled = False

                ElseIf Request.QueryString("Mode") = "CashListAllocation" Then
                    'changes for cashlist allocation
                    With oAccountdetails
                        If Not String.IsNullOrEmpty(Request("AllocationAccountkey")) Then
                            .AccountKey = CInt(Request("AllocationAccountkey"))
                        End If
                        If chkShowOutStandingTransaction.Checked = True Then
                            .OutstandingOnly = True
                        End If
                        If chkIncludeFuturebalanceTransaction.Checked = True Then
                            .IsNewPF = True
                        End If
                        If chkShowOnlyLatest500Transactions.Checked = True Then
                            .Display500 = True
                        End If
                        If chkIncludeFuturebalanceTransaction.Checked = True Then
                            .IncludeReversedTran = True
                        End If
                    End With

                    oAccountDetailsCollection = oWebService.GetAccountDetails(oAccountdetails, ddlBranchCode.SelectedValue)
                    For iCount As Integer = 0 To oAccountDetailsCollection.AccountDetails.Count - 1
                        If Not IsDate(oAccountDetailsCollection.AccountDetails(iCount).DueDate) Then
                            Dim dueDate = oAccountDetailsCollection.AccountDetails(iCount).DueDate
                            If dueDate Is Nothing OrElse dueDate = "" Then
                                oAccountDetailsCollection.AccountDetails(iCount).DueDate = oAccountDetailsCollection.AccountDetails(iCount).EffectiveDate
                            End If
                        End If
                    Next
                    gvGetTransactiondetails.PageIndex = 0
                    If (oNexusConfig.FinanceGridSize) = "" Then
                        gvGetTransactiondetails.PageSize = 10
                    Else
                        gvGetTransactiondetails.PageSize = CStr(oNexusConfig.FinanceGridSize)
                    End If
                    gvGetTransactiondetails.DataSource = oAccountDetailsCollection.AccountDetails
                    gvGetTransactiondetails.DataBind()

                    ColumnSelectorExtender.Visible = True
                    'Done Parallel with Etana 3.1(This line was not in vb code)
                    ScriptManager.RegisterStartupScript(Page, Me.GetType(), "id", "$get('" + ColumnSelectorExtender.ClientID + "').style.display='block';", True)

                    Cache.Insert(ViewState("TransactionpageCacheID"), oAccountDetailsCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

                    If (nAgentCnt > 0 AndAlso txtAccountCode.Text <> String.Empty) OrElse nAgentCnt = 0 Then
                        txtAccountName.Text = oAccountDetailsCollection.AccountName
                        txtActive.Text = oAccountDetailsCollection.AccountStatus
                    End If

                    txtAccountBalance.Text = oAccountDetailsCollection.AccountBalance
                    txtContactName.Text = oAccountDetailsCollection.ContactName
                    txtTelePhoneNumber.Text = oAccountDetailsCollection.PhoneNumber
                    txtAccountCode.Text = oAccountDetailsCollection.AccountDetails(0).Account
                    btnAccountCode.Enabled = False
                    btnNewsearch.Visible = False
                    txtAccountCode.Enabled = False
                    ColumnSelectorExtender.Visible = True
                    'Done Parallel with Etana 3.1(This line was not in vb code)
                    ScriptManager.RegisterStartupScript(Page, Me.GetType(), "id", "$get('" + ColumnSelectorExtender.ClientID + "').style.display='block';", True)
                    Dim oUserDetails As NexusProvider.UserDetails = CType(Current.Session(CNAgentDetails), NexusProvider.UserDetails)
                    If Not String.IsNullOrEmpty(oUserDetails.PartyCode) AndAlso Not oUserDetails.PartyCode Is Nothing Then
                        txtAgentCode.Text = CStr(oUserDetails.PartyCode)
                        txtAgentCode.ReadOnly = True
                        txtAgentName.Text = CStr(oUserDetails.PartyName)
                        txtAgentName.ReadOnly = True
                        lblAccountCode.Text = GetLocalResourceObject("lbl_ClientCode")
                        lblAccountName.Text = GetLocalResourceObject("lbl_ClientName")
                        'txtAccountCode.Text = String.Empty
                        'txtAccountName.Text = String.Empty

                        txtInsuredAccountCode.Visible = False
                        lblbtnInsuredAccountCode.Visible = False
                        btnInsuredAccountCode.Visible = False
                        ddlOperatorName.Enabled = False
                    Else
                        UserAgentSection.Visible = False
                    End If
                Else
                    Dim oUserDetails As NexusProvider.UserDetails = CType(Current.Session(CNAgentDetails), NexusProvider.UserDetails)
                    If Not String.IsNullOrEmpty(oUserDetails.PartyCode) AndAlso Not oUserDetails.PartyCode Is Nothing Then
                        txtAgentCode.Text = CStr(oUserDetails.PartyCode)
                        txtAgentCode.ReadOnly = True

                        txtAgentName.Text = CStr(oUserDetails.PartyName)
                        lblAccountCode.Text = GetLocalResourceObject("lbl_ClientCode")
                        lblAccountName.Text = GetLocalResourceObject("lbl_ClientName")
                        txtInsuredAccountCode.Visible = False
                        lblbtnInsuredAccountCode.Visible = False
                        btnInsuredAccountCode.Visible = False
                        ddlOperatorName.Enabled = False
                    Else
                        UserAgentSection.Visible = False
                    End If
                End If
            End If
        End Sub
        Protected Sub btnFindNow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
            GridRefresh()
        End Sub

        Public Sub GridRefresh()
            If UserCanDoTask("SearchTransaction") Then
                btnReverse.Enabled = False
                If Request.QueryString("Mode") = "IP" Then
                    LoadRecords()
                    Exit Sub
                ElseIf txtAccountCode.Text.Trim() <> String.Empty Or txtInsuredAccountCode.Text.Trim() <> String.Empty OrElse (txtAgentCode.Text.Trim() <> String.Empty AndAlso nAgentCnt > 0) Then
                    If (txtAccountCode.Enabled AndAlso txtAccountCode.Text.Trim().Length > 0 AndAlso txtAccountCode.Text.Trim().Contains("%") AndAlso hdnIsAccountFound.Value <> "True") Then
                        hdnIsAccountFound.Value = "False"
                        CustVldDate.IsValid = False
                        CustVldDate.Enabled = False
                        'After getting the records in the grids, if user searched again with invalid AccountCode then need to hide the grids
                        gvGetTransactiondetails.Visible = False
                        gvDocRefTransactions.Visible = False
                        Exit Sub
                    End If
                    GetAccountDetails()
                Else
                    If txtAccountCode.Text.Trim() = String.Empty And txtDocumentRef.Text.Trim().Length = 0 _
                     And txtFrom.Text.Length = 0 And txtTo.Text.Length = 0 And txtAmount.Text.Trim().Length = 0 _
                     And txtBGRef.Text.Trim().Length = 0 And txtAlternateRef.Text.Trim().Length = 0 _
                     And txtMediaRef.Text.Trim().Length = 0 And txtPurChaseInvoice.Text.Trim().Length = 0 _
                     And txtPurchaseOrderNo.Text.Trim().Length = 0 And txtPolicyNumber.Text.Trim().Length = 0 _
                     And txtClaimNumber.Text.Length = 0 And ddlOperatorName.SelectedValue = "0" And DocumentType.Value = "0" Then
                        CustVldDate.IsValid = False
                        CustVldDate.ErrorMessage = GetLocalResourceObject("err_AccountCode")

                        'After getting the records in the grids, if user searched again with invalid AccountCode then need to hide the grids
                        gvGetTransactiondetails.Visible = False
                        gvDocRefTransactions.Visible = False
                        gvTransactionForAccount.Visible = False




                    ElseIf txtAccountCode.Text.Trim() = String.Empty And txtDocumentRef.Text.Trim().Length = 0 _
                     And txtFrom.Text.Length = 0 And txtTo.Text.Length = 0 And txtAmount.Text.Trim().Length = 0 _
                     And txtBGRef.Text.Trim().Length = 0 And txtAlternateRef.Text.Trim().Length = 0 _
                     And txtMediaRef.Text.Trim().Length = 0 And txtPurChaseInvoice.Text.Trim().Length = 0 _
                     And txtPurchaseOrderNo.Text.Trim().Length = 0 And txtPolicyNumber.Text.Trim().Length = 0 _
                     And txtClaimNumber.Text.Length = 0 And ddlOperatorName.SelectedValue = "0" And DocumentType.Value = "0" Then
                        CustVldDate.IsValid = False
                        CustVldDate.ErrorMessage = GetLocalResourceObject("err_InsurerAccountCode")

                        'After getting the records in the grids, if user searched again with invalid AccountCode then need to hide the grids
                        gvGetTransactiondetails.Visible = False
                        gvDocRefTransactions.Visible = False


                    End If
                End If

                If Page.IsValid Then
                    GetTransactionDetails()
                    ColumnSelectorExtender.Visible = True
                    If (gvDocRefTransactions.Visible = True AndAlso gvDocRefTransactions.Rows.Count > 0) Or (gvTransactionForAccount.Visible = True AndAlso gvTransactionForAccount.Rows.Count > 0) Or (gvGetTransactiondetails.Visible = True AndAlso gvGetTransactiondetails.Rows.Count > 0) Then
                        ScriptManager.RegisterStartupScript(Page, Me.GetType(), "id", "$get('" + ColumnSelectorExtender.ClientID + "').style.display='block';", True)
                    Else
                        ScriptManager.RegisterStartupScript(Page, Me.GetType(), "id", "$get('" + ColumnSelectorExtender.ClientID + "').style.display='none';", True)
                    End If
                End If
            End If
            GetUserPreferredColumnList()
        End Sub

        Protected Sub GetAccountDetails()
            If UserCanDoTask("SearchTransaction") Then
                ' Creating and Instantiating objects
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oAccountSearchCriteria As NexusProvider.AccountSearchCriteria

                If (txtAccountCode.Enabled AndAlso txtAccountCode.Text.Trim().Length > 0 AndAlso txtAccountCode.Text.Trim().Contains("%") AndAlso hdnIsAccountFound.Value <> "True") Then
                    hdnIsAccountFound.Value = "False"
                    CustVldDate.IsValid = False
                    CustVldDate.Enabled = False
                    'After getting the records in the grids, if user searched again with invalid AccountCode then need to hide the grids
                    gvGetTransactiondetails.Visible = False
                    gvDocRefTransactions.Visible = False
                    Exit Sub
                End If

                ' Obtaining data from controls if search criteria is entered 
                ' and assigning the data to objects                 
                If Not txtAccountCode.Text.Trim.Length() = 0 OrElse (Not txtAgentCode.Text.Trim.Length() = 0 AndAlso nAgentCnt > 0) Then
                    oAccountSearchCriteria = New NexusProvider.AccountSearchCriteria
                    If String.IsNullOrEmpty(txtAccountCode.Text) AndAlso Not String.IsNullOrEmpty(txtAgentCode.Text) Then
                        oAccountSearchCriteria.ShortCode = txtAgentCode.Text.Trim()
                    Else
                        If nAgentCnt > 0 Then
                            If txtAccountCode.Text.Trim <> String.Empty AndAlso txtAccountCode.Text.Trim = txtAgentCode.Text.Trim Then
                                oAccountSearchCriteria.LedgerCode = "AG"
                            Else
                                oAccountSearchCriteria.LedgerCode = "SA"
                            End If
                        End If
                        oAccountSearchCriteria.ShortCode = txtAccountCode.Text.Trim()
                    End If
                    ' calls the web method for obtaining data
                    oAccountSearchResultCollection = oWebService.FindAccounts(oAccountSearchCriteria)

                    ' checks if the collections returns any value and assigns the values to Grid
                    If oAccountSearchResultCollection IsNot Nothing Then
                        If (nAgentCnt > 0 AndAlso txtAccountCode.Text <> String.Empty) OrElse nAgentCnt = 0 Then
                            Me.txtAccountName.Text = oAccountSearchResultCollection(0).AccountName.Trim()
                        End If
                        Me.hiddenAccountCode.Value = oAccountSearchResultCollection(0).AccountKey
                        sAccountKey = oAccountSearchResultCollection(0).AccountKey.ToString
                        ViewState("PartyKey") = oAccountSearchResultCollection(0).PartyKey
                        ' Me.txtAccountCode.Focus()

                        ColumnSelectorExtender.Visible = True
                        ScriptManager.RegisterStartupScript(Page, Me.GetType(), "id", "$get('" + ColumnSelectorExtender.ClientID + "').style.display='block';", True)
                        ' Set the currency code
                        rbtOutStandingColumn.Items(2).Text = "Account"
                        rbtOutStandingColumn.Items(2).Text &= "(" & oAccountSearchResultCollection(0).CurrencyCode & ")"
                        rbtAmountColumn.Items(2).Text = "Account"
                        rbtAmountColumn.Items(2).Text &= "(" & oAccountSearchResultCollection(0).CurrencyCode & ")"
                        ViewState("AccountCurrencyCode") = oAccountSearchResultCollection(0).CurrencyCode
                        hdnIsAccountFound.Value = "True"
                    Else
                        If txtAccountCode.Enabled Then
                            hdnIsAccountFound.Value = "False"
                        End If
                        CustVldDate.IsValid = False
                        CustVldDate.ErrorMessage = GetLocalResourceObject("err_AccountCode")

                        'After getting the records in the grids, if user searched again with invalid AccountCode then need to hide the grids
                        gvGetTransactiondetails.Visible = False
                        gvDocRefTransactions.Visible = False

                        Exit Sub
                    End If
                End If

                If Not txtInsuredAccountCode.Text.Trim.Length() = 0 Then
                    oAccountSearchCriteria = New NexusProvider.AccountSearchCriteria
                    oAccountSearchCriteria.ShortCode = txtInsuredAccountCode.Text.Trim()
                    oAccountSearchResultCollection = oWebService.FindAccounts(oAccountSearchCriteria)
                    If oAccountSearchResultCollection IsNot Nothing Then
                        Me.hiddenInsuredAccountKey.Value = oAccountSearchResultCollection(0).AccountKey
                        Me.txtInsuredAccountCode.Focus()
                        ColumnSelectorExtender.Visible = True
                        ScriptManager.RegisterStartupScript(Page, Me.GetType(), "id", "$get('" + ColumnSelectorExtender.ClientID + "').style.display='block';", True)
                    Else
                        CustVldDate.IsValid = False
                        CustVldDate.ErrorMessage = GetLocalResourceObject("err_InsurerAccountCode")
                        Exit Sub
                    End If
                End If

            End If
        End Sub

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            ' If the user click the Cancel Button Taking him to the Next level
            If gvTransactionForAccount.Visible = True And gvGetTransactiondetails.Visible = False And gvDocRefTransactions.Visible = False Then
                ClearControls()
                gvDocRefTransactions.Visible = True
                ColumnSelectorExtender.Visible = True
                ScriptManager.RegisterStartupScript(Page, Me.GetType(), "id", "$get('" + ColumnSelectorExtender.ClientID + "').style.display='block';", True)
                'txtDocumentRef.Text = ViewState(CNDocref)
                gvTransactionForAccount.Visible = False
                btnAccountCode.Enabled = True
                txtAccountCode.Enabled = True
                btnNewsearch.Enabled = True
                Exit Sub
            ElseIf gvDocRefTransactions.Visible = True And gvGetTransactiondetails.Visible = False And gvTransactionForAccount.Visible = False Then
                Dim oOptionSetting As NexusProvider.OptionTypeSetting
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                oOptionSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 1031)
                Dim sShowDrillDoc As Boolean = True

                If ViewState("ShowDrillDoc") IsNot Nothing Then
                    sShowDrillDoc = Boolean.Parse(ViewState("ShowDrillDoc"))
                End If

                If oOptionSetting.OptionValue = "1" AndAlso sShowDrillDoc = False Then
                    oAccountDetailsCollection =
                                                          CType(Cache.Item(ViewState("RollUpDocRefpageCacheID")), NexusProvider.AccountDetailsDefaults)
                    ViewState("ShowDrillDoc") = True
                    gvGetTransactiondetails.Visible = False
                    gvDocRefTransactions.Visible = True
                    ColumnSelectorExtender.Visible = True
                    If (oNexusConfig.FinanceGridSize) = "" Then
                        gvDocRefTransactions.PageSize = 10
                    Else
                        gvDocRefTransactions.PageSize = CStr(oNexusConfig.FinanceGridSize)
                    End If
                    ScriptManager.RegisterStartupScript(Page, Me.GetType(), "id", "$get('" + ColumnSelectorExtender.ClientID + "').style.display='block';", True)
                    gvDocRefTransactions.DataSource = oAccountDetailsCollection.AccountDetails
                    gvDocRefTransactions.DataBind()
                    btnCancel.Visible = True
                Else
                    oAccountDetailsCollection =
                                       CType(Cache.Item(ViewState("TransactionpageCacheID")), NexusProvider.AccountDetailsDefaults)
                    If oAccountDetailsCollection Is Nothing Then
                        GetTransactionDetails()
                    End If
                    gvGetTransactiondetails.Visible = True
                    ColumnSelectorExtender.Visible = True
                    ScriptManager.RegisterStartupScript(Page, Me.GetType(), "id", "$get('" + ColumnSelectorExtender.ClientID + "').style.display='block';", True)
                    gvDocRefTransactions.Visible = False
                    btnCancel.Visible = False
                End If

                If Request.QueryString("Mode") IsNot Nothing AndAlso Request.QueryString("Mode") = "IP" Then
                    btnCancel.Visible = True
                    ClearControls()
                    txtDocumentRef.Text = ViewState(CNDocref)
                Else
                    ClearControls()
                    'txtDocumentRef.Text = String.Empty
                    oAccountDetailsCollection = oAccountDetailsCollection
                    If oAccountDetailsCollection IsNot Nothing Then
                        If (nAgentCnt > 0 AndAlso txtAccountCode.Text <> String.Empty) OrElse nAgentCnt = 0 Then
                            txtAccountName.Text = oAccountDetailsCollection.AccountName
                            txtActive.Text = oAccountDetailsCollection.AccountStatus
                        End If
                        If ViewState("AccountCurrencyCode") IsNot Nothing Then
                            txtAccountBalance.Text = New Money(oAccountDetailsCollection.AccountBalance, ViewState("AccountCurrencyCode")).Formatted
                        Else
                            txtAccountBalance.Text = oAccountDetailsCollection.AccountBalance
                        End If

                        txtContactName.Text = oAccountDetailsCollection.ContactName
                        txtTelePhoneNumber.Text = oAccountDetailsCollection.PhoneNumber
                        txtAccountCode.Text = oAccountDetailsCollection.AccountDetails(0).Account
                    End If
                End If
            ElseIf Request.QueryString("Mode") = "IP" Then
                Response.Redirect("~/secure/InsurerPayments.aspx?Mode=IP", False)
            End If
        End Sub

        Protected Sub btnNewsearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewsearch.Click
            ColumnSelectorExtender.Visible = False
            btnReverse.Enabled = False
            ScriptManager.RegisterStartupScript(Page, Me.GetType(), "id", "$get('" + ColumnSelectorExtender.ClientID + "').style.display='none';", True)
            Response.Redirect("~/secure/SearchTransactions.aspx", False)

        End Sub

        Protected Sub btnAddTrasaction_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddTrasaction.Click
            Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "RefreshAllocate") & ";"
            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "ParentPostBack", PostBackStr, True)
            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
        End Sub

        Protected Sub gvGetTransactiondetails_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvGetTransactiondetails.DataBound
            If gvGetTransactiondetails.Rows.Count = 0 Or gvGetTransactiondetails.PageCount = 1 Then
                gvGetTransactiondetails.AllowPaging = False
            End If
            If gvGetTransactiondetails.Rows.Count > 0 Then
                gvGetTransactiondetails.HeaderRow.Cells(1).Visible = False
            End If
        End Sub


        Protected Sub gvGetTransactiondetails_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvGetTransactiondetails.RowCommand
            If Not LCase(e.CommandName).Equals("page") Then
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

                Select Case e.CommandName
                    Case "DrillDoc" 'to find the transactions based on the document Ref

                        Try
                            oAccountdetails.DocumentRef = e.CommandArgument
                            ViewState.Add(CNDocref, e.CommandArgument)
                            chkShowOutStandingTransaction.Checked = False
                            oAccountdetails.OutstandingOnly = False
                            If chkIncludeFuturebalanceTransaction.Checked = True Then
                                oAccountdetails.IsNewPF = True
                            End If
                            If chkShowOnlyLatest500Transactions.Checked = True Then
                                oAccountdetails.Display500 = True
                            End If
                            If chkIncludeFuturebalanceTransaction.Checked = True Then
                                oAccountdetails.IncludeReversedTran = True
                            End If
                            ClearControls()
                            txtDocumentRef.Text = e.CommandArgument
                            txtAccountCode.Text = String.Empty

                            oAccountDetailsCollection = oWebService.GetAccountDetails(oAccountdetails)

                            For iCount As Integer = 0 To oAccountDetailsCollection.AccountDetails.Count - 1

                                If rbtAmountColumn.Items(0).Selected = True Then 'TC 
                                    oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount
                                    oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAccountAmount
                                ElseIf rbtAmountColumn.Items(1).Selected = True Then 'BC
                                    oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).Amount
                                    oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAmount
                                ElseIf rbtAmountColumn.Items(2).Selected = True Then 'AC
                                    oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).AccountAmount
                                    oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAccountAmount
                                End If

                                'If rbtAmountColumn.Items(0).Selected = True Then 'BC
                                '    oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).Amount
                                '    oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAmount
                                'ElseIf rbtAmountColumn.Items(1).Selected = True Then 'TC
                                '    oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount
                                '    oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAccountAmount
                                'ElseIf rbtAmountColumn.Items(2).Selected = True Then 'AC
                                '    oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).AccountAmount
                                '    oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAccountAmount
                                'End If

                                If rbtOutStandingColumn.Items(0).Selected = True Then 'BC
                                    oAccountDetailsCollection.AccountDetails(iCount).OutstandingAmount = oAccountDetailsCollection.AccountDetails(iCount).OutstandingAmount
                                ElseIf rbtOutStandingColumn.Items(1).Selected = True Then 'TC
                                    oAccountDetailsCollection.AccountDetails(iCount).OutstandingAmount = oAccountDetailsCollection.AccountDetails(iCount).OutStandingCurrencyAmount
                                ElseIf rbtOutStandingColumn.Items(2).Selected = True Then 'AC
                                    oAccountDetailsCollection.AccountDetails(iCount).OutstandingAmount = oAccountDetailsCollection.AccountDetails(iCount).AccountOutStandingAmount
                                End If
                            Next
                            If (oNexusConfig.FinanceGridSize) = "" Then
                                gvDocRefTransactions.PageSize = 10
                            Else
                                gvDocRefTransactions.PageSize = CStr(oNexusConfig.FinanceGridSize)
                            End If
                            gvDocRefTransactions.DataSource = oAccountDetailsCollection.AccountDetails
                            gvDocRefTransactions.DataBind()

                            Cache.Insert(ViewState("DocRefpageCacheID"), oAccountDetailsCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

                            gvGetTransactiondetails.Visible = False
                            gvTransactionForAccount.Visible = False
                            gvDocRefTransactions.Visible = True
                            ColumnSelectorExtender.Visible = True
                            ScriptManager.RegisterStartupScript(Page, Me.GetType(), "id", "$get('" + ColumnSelectorExtender.ClientID + "').style.display='block';", True)
                            btnCancel.Visible = True

                        Finally
                            oWebService = Nothing
                        End Try

                    Case "Expand"

                        Try
                            Dim sPartyKey As String = ViewState("PartyKey")
                            oAccountdetails.DocumentRef = e.CommandArgument

                            If String.IsNullOrEmpty(sPartyKey) = False Then
                                oAccountdetails.PartyCnt = Integer.Parse(sPartyKey)
                            End If

                            ViewState.Add(CNDocref, e.CommandArgument)
                            chkShowOutStandingTransaction.Checked = False
                            oAccountdetails.OutstandingOnly = False
                            If chkIncludeFuturebalanceTransaction.Checked = True Then
                                oAccountdetails.IsNewPF = True
                            End If
                            If chkShowOnlyLatest500Transactions.Checked = True Then
                                oAccountdetails.Display500 = True
                            End If
                            If chkIncludeFuturebalanceTransaction.Checked = True OrElse chkIncludeReversedReversalTransactions.Checked = True Then
                                oAccountdetails.IncludeReversedTran = True
                            End If
                            '  ClearControls()
                            txtDocumentRef.Text = e.CommandArgument
                            ' txtAccountCode.Text = String.Empty

                            oAccountDetailsCollection = oWebService.GetAccountDetails(oAccountdetails)

                            For iCount As Integer = 0 To oAccountDetailsCollection.AccountDetails.Count - 1
                                If rbtAmountColumn.Items(0).Selected = True Then 'TC 
                                    oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount
                                    oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAccountAmount
                                ElseIf rbtAmountColumn.Items(1).Selected = True Then 'BC
                                    oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).Amount
                                    oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAmount
                                ElseIf rbtAmountColumn.Items(2).Selected = True Then 'AC
                                    oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).AccountAmount
                                    oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAccountAmount
                                End If

                                'If rbtAmountColumn.Items(0).Selected = True Then 'BC
                                '    oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).Amount
                                '    oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAmount
                                'ElseIf rbtAmountColumn.Items(1).Selected = True Then 'TC
                                '    oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount
                                '    oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAccountAmount
                                'ElseIf rbtAmountColumn.Items(2).Selected = True Then 'AC
                                '    oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).AccountAmount
                                '    oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAccountAmount
                                'End If

                                If rbtOutStandingColumn.Items(0).Selected = True Then 'BC
                                    oAccountDetailsCollection.AccountDetails(iCount).OutstandingAmount = oAccountDetailsCollection.AccountDetails(iCount).OutstandingAmount
                                ElseIf rbtOutStandingColumn.Items(1).Selected = True Then 'TC
                                    oAccountDetailsCollection.AccountDetails(iCount).OutstandingAmount = oAccountDetailsCollection.AccountDetails(iCount).OutStandingCurrencyAmount
                                ElseIf rbtOutStandingColumn.Items(2).Selected = True Then 'AC
                                    oAccountDetailsCollection.AccountDetails(iCount).OutstandingAmount = oAccountDetailsCollection.AccountDetails(iCount).AccountOutStandingAmount
                                End If
                            Next
                            If (oNexusConfig.FinanceGridSize) = "" Then
                                gvDocRefTransactions.PageSize = 10
                            Else
                                gvDocRefTransactions.PageSize = CStr(oNexusConfig.FinanceGridSize)
                            End If
                            gvDocRefTransactions.DataSource = oAccountDetailsCollection.AccountDetails
                            gvDocRefTransactions.DataBind()

                            Cache.Insert(ViewState("RollUpDocRefpageCacheID"), oAccountDetailsCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

                            gvGetTransactiondetails.Visible = False
                            gvTransactionForAccount.Visible = False
                            gvDocRefTransactions.Visible = True
                            ColumnSelectorExtender.Visible = True
                            ScriptManager.RegisterStartupScript(Page, Me.GetType(), "id", "$get('" + ColumnSelectorExtender.ClientID + "').style.display='block';", True)
                            btnCancel.Visible = True

                        Finally
                            oWebService = Nothing
                        End Try
                    Case "GenerateDocument"
                        oWebService = New NexusProvider.ProviderManager().Provider

                        Dim documentDefaults As NexusProvider.DocumentDefaults = New NexusProvider.DocumentDefaults()
                        Dim fileLocation As String
                        Dim docType As String
                        Dim partyCnt As Integer
                        Dim accountCode As String
                        Dim documentRef As String

                        'Rowindex is stored as commandArgument
                        Dim rowIndex As Integer = e.CommandArgument

                        Dim accountDetailsCollection As NexusProvider.AccountDetailsDefaults =
                        CType(Cache.Item(ViewState("TransactionpageCacheID")), NexusProvider.AccountDetailsDefaults)

                        If IsNothing(accountDetailsCollection) Then
                            accountDetailsCollection = oWebService.GetAccountDetails(oAccountdetails, ddlBranchCode.SelectedValue)
                        End If

                        If Not IsNothing(accountDetailsCollection) Then
                            documentRef = accountDetailsCollection.AccountDetails(rowIndex).DocRef
                            docType = accountDetailsCollection.AccountDetails(rowIndex).DocumentTypeCode

                            accountCode = accountDetailsCollection.AccountDetails(rowIndex).Account

                            Dim accountSearchCriteria As New NexusProvider.AccountSearchCriteria
                            Dim accountSearchCollection As NexusProvider.AccountSearchResultCollection
                            accountSearchCriteria.ShortCode = accountCode.Trim
                            accountSearchCollection = oWebService.FindAccounts(accountSearchCriteria)

                            If Not IsNothing(accountSearchCollection) Then
                                partyCnt = accountSearchCollection.Item(0).PartyKey

                                Dim documentTemplateColl As NexusProvider.DocumentTemplateCollection = Nothing
                                Dim documentTemplate As New NexusProvider.DocumentTemplate
                                documentTemplate.TypeCode = 5
                                documentTemplateColl = oWebService.FindDocumentTemplates(documentTemplate, String.Empty)

                                Dim optionType As New NexusProvider.OptionTypeSetting

                                If documentTypeDictionary.ContainsKey(docType.ToUpper.Trim) Then
                                    optionType = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 61)
                                ElseIf docType.ToUpper.Trim = "PAYMENT" Then
                                    optionType = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 63)
                                End If

                                For Each docTemplate As NexusProvider.DocumentTemplate In documentTemplateColl
                                    If docTemplate.DocumentTemplateId = optionType.OptionValue Then
                                        documentDefaults.documentTemplateCode = docTemplate.Code
                                        Exit For
                                    End If
                                Next

                                If Not String.IsNullOrEmpty(documentDefaults.documentTemplateCode) Then
                                    'Get the option Value for option Number 5009 (archive as PDF)
                                    optionType = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5009)
                                    Dim documentType As NexusProvider.DocumentType

                                    If optionType.OptionValue = "1" Then
                                        documentType = NexusProvider.DocumentType.PDF
                                        documentDefaults.FileType = "PDF"
                                    Else
                                        documentType = NexusProvider.DocumentType.DOCX
                                        documentDefaults.FileType = "WORD"
                                    End If

                                    fileLocation = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork").Portals.Portal(CMS.Library.Portal.GetPortalID()), Nexus.Library.Config.Portal).TempFileLocation & "\" & Guid.NewGuid.ToString & "\" & documentDefaults.documentTemplateCode.Trim + "." + documentDefaults.FileType
                                    oWebService.GenerateDocument(partyCnt, 0, 0, documentDefaults.documentTemplateCode, documentType, fileLocation, 0, Nothing, Nothing, documentRef)

                                    If Not String.IsNullOrEmpty(documentRef) Then documentRef = documentRef.Trim Else documentRef = Session("ModeType")
                                    documentDefaults.FileLocation = fileLocation
                                    documentDefaults.DocumentName = documentRef.Trim + "." + documentDefaults.FileType

                                    Session(CNDocumentToDownload) = documentDefaults

                                    ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "RedirectToDownload", "RedirectToDownload();", True)
                                    ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "UnblockUI", "$.unblockUI();", True)
                                Else
                                    ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "DocumentNotFound", "alert('No document has been Configured for this process');", True)
                                End If
                            Else
                                ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "AccountDetailsNotFound", "alert('Account Details not found');", True)
                            End If
                        Else
                            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "AccountDetailsNotFound", "alert('Account Details not found');", True)
                        End If
                        oWebService = Nothing
                    Case "EMAILLETTER"

                        Dim accountDetailsCollection As NexusProvider.AccountDetailsDefaults =
                        CType(Cache.Item(ViewState("TransactionpageCacheID")), NexusProvider.AccountDetailsDefaults)
                        Dim sURL As String = String.Empty
                        Dim partyKey As Integer
                        Dim accountCode As String
                        Dim rowIndex As Integer = e.CommandArgument
                        Dim docType As String = ""
                        Dim sDocumentRef As String = String.Empty
                        If IsNothing(accountDetailsCollection) Then
                            accountDetailsCollection = oWebService.GetAccountDetails(oAccountdetails, ddlBranchCode.SelectedValue)
                        End If

                        If Not IsNothing(accountDetailsCollection) Then
                            sDocumentRef = accountDetailsCollection.AccountDetails(rowIndex).DocRef
                            docType = accountDetailsCollection.AccountDetails(rowIndex).DocumentTypeCode
                            partyKey = accountDetailsCollection.AccountDetails(rowIndex).PartyCnt
                            accountCode = accountDetailsCollection.AccountDetails(rowIndex).Account
                            Dim accountSearchCriteria As New NexusProvider.AccountSearchCriteria
                            Dim accountSearchCollection As NexusProvider.AccountSearchResultCollection
                            accountSearchCriteria.ShortCode = accountCode.Trim
                            accountSearchCollection = oWebService.FindAccounts(accountSearchCriteria)
                            If Not IsNothing(accountSearchCollection) Then
                                If partyKey = 0 Then
                                    partyKey = accountSearchCollection.Item(0).PartyKey
                                End If
                            End If

                        End If

                        If HttpContext.Current.Session.IsCookieless Then
                            sURL = System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "Modal/SendEmail.aspx?loc=manual&PartyKey=" & partyKey & "&Document_ref=" & sDocumentRef & "&InsuranceFileKey=0&modal=true&CalledFrom=CashList&ModeType=" & docType & "&KeepThis=true&TB_iframe=true&height=300&width=750"
                        Else
                            sURL = System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "Modal/SendEmail.aspx?loc=manual&PartyKey=" & partyKey & "&Document_ref=" & sDocumentRef & "&InsuranceFileKey=0&modal=true&CalledFrom=CashList&ModeType=" & docType & "&KeepThis=true&TB_iframe=true&height=300&width=750"
                        End If

                        ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "OpenModal", "tb_show(null , '" & sURL & "' , null);", True)
                        Exit Sub
                End Select
            End If
        End Sub

        Protected Sub gvGetTransactiondetails_RowDataBound1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvGetTransactiondetails.RowDataBound

            If e.Row.RowType = DataControlRowType.DataRow Then
                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("TransdetailId") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.AccountDetails).TransdetailId)
                e.Row.Cells(1).Text = CType(e.Row.DataItem, NexusProvider.AccountDetails).TransDetailKeys
                e.Row.Cells(1).Visible = False
                ' gvDocRefTransactions.Columns(9).Visible = True
                '  gvDocRefTransactions.Columns(11).Visible = True

                Dim oliDrillDoc As HtmlGenericControl = e.Row.Cells(30).FindControl("liDrillDoc")
                Dim oliExpand As HtmlGenericControl = e.Row.Cells(30).FindControl("liExpand")
                Dim oOptionSetting As NexusProvider.OptionTypeSetting
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                oOptionSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 1031)
                If oOptionSetting.OptionValue = "1" Then
                    oliDrillDoc.Visible = False
                    oliExpand.Visible = True
                Else
                    oliDrillDoc.Visible = True
                    oliExpand.Visible = False
                End If

                Dim amount As Double
                amount = CType(e.Row.DataItem, NexusProvider.AccountDetails).CurrencyAmount
                Dim outStandingamount As Double
                outStandingamount = CType(e.Row.DataItem, NexusProvider.AccountDetails).OutStandingCurrencyAmount
                Dim oliViewAllocation As HtmlGenericControl = e.Row.Cells(30).FindControl("liViewAllocation")
                Dim ohyplink As LinkButton = e.Row.Cells(30).FindControl("lblhypViewAllocation")
                Dim iTransdetailKey As Integer
                Dim bIsSplitReceipt As Boolean = False
                Dim bIsLeadAgent As Boolean = False

                bIsLeadAgent = CType(e.Row.DataItem, NexusProvider.AccountDetails).IsLeadAgent
                bIsSplitReceipt = CType(e.Row.DataItem, NexusProvider.AccountDetails).IsSplitReceipt

                iTransdetailKey = CType(e.Row.DataItem, NexusProvider.AccountDetails).TransDetailKeys

                sAccountKey = CType(e.Row.DataItem, NexusProvider.AccountDetails).AccountKey
                'To view the Allocation of the perticular Trascations
                If Math.Round(amount) <> Math.Round(outStandingamount) Then
                    If HttpContext.Current.Session.IsCookieless Then
                        ohyplink.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/Viewallocation.aspx?Transdetailkey=" & iTransdetailKey & "&IsLeadAgent=" & bIsLeadAgent & "&AccountKey=" & sAccountKey & "&IsSplitReceipt=" & bIsSplitReceipt & "&modal=true&KeepThis=true&TB_iframe=true&height=550&width=800' , null);return false;"
                    Else
                        ohyplink.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "Modal/Viewallocation.aspx?Transdetailkey=" & iTransdetailKey & "&IsLeadAgent=" & bIsLeadAgent & "&AccountKey=" & sAccountKey & "&IsSplitReceipt=" & bIsSplitReceipt & "&modal=true&KeepThis=true&TB_iframe=true&height=550&width=800' , null);return false;"
                    End If

                    ohyplink.Visible = True
                    oliViewAllocation.Visible = True
                Else
                    ohyplink.Visible = False
                    oliViewAllocation.Visible = False
                End If

                Dim chkTranskey As CheckBox = e.Row.FindControl("chkselectedTransaction")
                If outStandingamount = 0 Then
                    chkTranskey.Visible = False
                End If

                Dim olinkbuttonl As LinkButton = e.Row.Cells(30).FindControl("lnkbtnDrillDoc")
                Dim olinkExpand As LinkButton = e.Row.Cells(30).FindControl("lnkbtnExpand")
                olinkExpand.CommandArgument = CType(e.Row.DataItem, NexusProvider.AccountDetails).DocRef
                olinkbuttonl.CommandArgument = CType(e.Row.DataItem, NexusProvider.AccountDetails).DocRef
                olinkbuttonl.CommandName = "DrillDoc"
                If CType(e.Row.DataItem, NexusProvider.AccountDetails).PrimarySettled = True Then
                    e.Row.Cells(12).Text = "Yes"
                Else
                    e.Row.Cells(12).Text = "No"
                End If

                Dim lblDocument As LinkButton = e.Row.Cells(30).FindControl("lblDocument")
                lblDocument.CommandArgument = CType(e.Row.DataItem, NexusProvider.AccountDetails).DocumentTypeCode

                Dim lnkEmail As LinkButton = e.Row.Cells(30).FindControl("lnkEmail")
                If lnkEmail IsNot Nothing Then
                    lnkEmail.CommandArgument = CType(e.Row.DataItem, NexusProvider.AccountDetails).DocumentTypeCode
                End If
                Dim sDocType As String = CType(e.Row.DataItem, NexusProvider.AccountDetails).DocumentTypeCode

                If Not IsNothing(sDocType) Then
                    If sDocType = "Payment" Then
                        lblDocument.Visible = True
                        lblDocument.Text = GetLocalResourceObject("btnPaymentDocument")
                        lblDocument.CommandArgument = e.Row.DataItemIndex
                        If lnkEmail IsNot Nothing Then
                            lnkEmail.Visible = True
                            lnkEmail.Text = GetLocalResourceObject("btnEmailPayment")
                            lnkEmail.CommandArgument = e.Row.DataItemIndex
                        End If
                    ElseIf documentTypeDictionary.ContainsKey(sDocType.ToUpper.Trim) Then
                        lblDocument.Visible = True
                        lblDocument.Text = GetLocalResourceObject("btnReceiptDocument")
                        lblDocument.CommandArgument = e.Row.DataItemIndex
                        If lnkEmail IsNot Nothing Then
                            lnkEmail.Visible = True
                            lnkEmail.Text = GetLocalResourceObject("btnEmailReceipt")
                            lnkEmail.CommandArgument = e.Row.DataItemIndex
                        End If
                    Else
                        lblDocument.Visible = False
                        If lnkEmail IsNot Nothing Then
                            lnkEmail.Visible = False
                        End If
                    End If
                End If

                Dim TempGridRow As GridViewRow = e.Row
                    Dim oItem As NexusProvider.AccountDetails = CType(e.Row.DataItem, NexusProvider.AccountDetails)

                If rbtAmountColumn.Items(0).Selected = True Then 'TC 
                    TempGridRow.Cells(11).Text = New Money(oItem.CurrencyAmount, oItem.BaseCurrencyCode.Trim).Formatted
                ElseIf rbtAmountColumn.Items(1).Selected = True Then 'BC
                    TempGridRow.Cells(11).Text = New Money(oItem.CurrencyAmount, oItem.CurrencyCode.Trim).Formatted
                ElseIf rbtAmountColumn.Items(2).Selected = True Then 'AC
                    TempGridRow.Cells(11).Text = New Money(oItem.CurrencyAmount, oItem.AccountCurrencyCode.Trim).Formatted
                End If

                'If rbtAmountColumn.Items(0).Selected = True Then 'BC
                '    TempGridRow.Cells(10).Text = New Money(oItem.CurrencyAmount, oItem.BaseCurrencyCode.Trim).Formatted
                'ElseIf rbtAmountColumn.Items(1).Selected = True Then 'TC
                '    TempGridRow.Cells(10).Text = New Money(oItem.CurrencyAmount, oItem.CurrencyCode.Trim).Formatted
                'ElseIf rbtAmountColumn.Items(2).Selected = True Then 'AC
                '    TempGridRow.Cells(10).Text = New Money(oItem.CurrencyAmount, oItem.AccountCurrencyCode.Trim).Formatted
                'End If

                If rbtOutStandingColumn.Items(0).Selected = True Then 'BC
                    TempGridRow.Cells(13).Text = New Money(oItem.OutstandingAmount, oItem.CurrencyCode.Trim).Formatted
                ElseIf rbtOutStandingColumn.Items(1).Selected = True Then 'TC
                    TempGridRow.Cells(13).Text = New Money(oItem.OutstandingAmount, oItem.BaseCurrencyCode.Trim).Formatted
                ElseIf rbtOutStandingColumn.Items(2).Selected = True Then 'AC
                    TempGridRow.Cells(13).Text = New Money(oItem.OutstandingAmount, oItem.AccountCurrencyCode.Trim).Formatted
                End If
                TempGridRow.Cells(1).Text = e.Row.Cells(1).Text
            ElseIf e.Row.RowType = DataControlRowType.Header Then

            End If
        End Sub

        Protected Sub gvDocRefTransactions_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDocRefTransactions.PageIndexChanging
            Dim oOptionSetting As NexusProvider.OptionTypeSetting
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            oOptionSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 1031)
            Dim sShowDrillDoc As Boolean = Boolean.Parse(ViewState("ShowDrillDoc"))

            If oOptionSetting.OptionValue = "1" AndAlso sShowDrillDoc = False Then
                oAccountDetailsCollection = CType(Cache.Item(ViewState("DocRefpageCacheID")), NexusProvider.AccountDetailsDefaults)
            ElseIf oOptionSetting.OptionValue = "1" AndAlso sShowDrillDoc = True Then
                oAccountDetailsCollection = CType(Cache.Item(ViewState("RollUpDocRefpageCacheID")), NexusProvider.AccountDetailsDefaults)
            Else
                oAccountDetailsCollection = CType(Cache.Item(ViewState("DocRefpageCacheID")), NexusProvider.AccountDetailsDefaults)
            End If

            If oAccountDetailsCollection Is Nothing Then
                GetTransactionDetails()
            End If
            If (oNexusConfig.FinanceGridSize) = "" Then
                gvDocRefTransactions.PageSize = 10
            Else
                gvDocRefTransactions.PageSize = CStr(oNexusConfig.FinanceGridSize)
            End If
            gvDocRefTransactions.PageIndex = e.NewPageIndex
            gvDocRefTransactions.DataSource = oAccountDetailsCollection.AccountDetails
            gvDocRefTransactions.DataBind()

        End Sub

        Protected Sub gvDocRefTransactions_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDocRefTransactions.RowCommand
            If Not LCase(e.CommandName).Equals("page") Then
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Select Case e.CommandName
                    Case "DrillAccount" 'to find the transactions based on the document Ref
                        btnAccountCode.Enabled = False
                        txtAccountCode.Enabled = False
                        btnNewsearch.Enabled = False

                        Dim oOptionSetting As NexusProvider.OptionTypeSetting
                        Try
                            oAccountdetails.AccountKey = Convert.ToInt32(e.CommandArgument)
                            hiddenAccountCode.Value = Convert.ToInt32(e.CommandArgument)
                            If chkShowOutStandingTransaction.Checked = True Then
                                oAccountdetails.OutstandingOnly = True
                            End If

                            If chkIncludeFuturebalanceTransaction.Checked = True Then
                                oAccountdetails.IsNewPF = True
                            End If
                            If chkShowOnlyLatest500Transactions.Checked = True Then
                                oAccountdetails.Display500 = True
                            End If

                            If chkIncludeFuturebalanceTransaction.Checked = True Then
                                oAccountdetails.IncludeReversedTran = True
                            End If

                            oOptionSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 1031)
                            If oOptionSetting.OptionValue = "1" Then
                                oAccountdetails.Rollup = True
                            End If

                            oAccountDetailsCollection = oWebService.GetAccountDetails(oAccountdetails)

                            For iCount As Integer = 0 To oAccountDetailsCollection.AccountDetails.Count - 1
                                If rbtAmountColumn.Items(0).Selected = True Then 'TC 
                                    oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount
                                    oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAccountAmount
                                ElseIf rbtAmountColumn.Items(1).Selected = True Then 'BC
                                    oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).Amount
                                    oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAmount
                                ElseIf rbtAmountColumn.Items(2).Selected = True Then 'AC
                                    oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).AccountAmount
                                    oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAccountAmount
                                End If

                                'If rbtAmountColumn.Items(0).Selected = True Then 'BC
                                '    oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).Amount
                                '    oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAmount
                                'ElseIf rbtAmountColumn.Items(1).Selected = True Then 'TC
                                '    oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount
                                '    oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAccountAmount
                                'ElseIf rbtAmountColumn.Items(2).Selected = True Then 'AC
                                '    oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).AccountAmount
                                '    oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAccountAmount
                                'End If

                                If rbtOutStandingColumn.Items(0).Selected = True Then 'BC
                                    oAccountDetailsCollection.AccountDetails(iCount).OutstandingAmount = oAccountDetailsCollection.AccountDetails(iCount).OutstandingAmount
                                ElseIf rbtOutStandingColumn.Items(1).Selected = True Then 'TC
                                    oAccountDetailsCollection.AccountDetails(iCount).OutstandingAmount = oAccountDetailsCollection.AccountDetails(iCount).OutStandingCurrencyAmount
                                ElseIf rbtOutStandingColumn.Items(2).Selected = True Then 'AC
                                    oAccountDetailsCollection.AccountDetails(iCount).OutstandingAmount = oAccountDetailsCollection.AccountDetails(iCount).AccountOutStandingAmount
                                End If
                            Next
                            If (oNexusConfig.FinanceGridSize) = "" Then
                                gvTransactionForAccount.PageSize = 10
                            Else
                                gvTransactionForAccount.PageSize = CStr(oNexusConfig.FinanceGridSize)
                            End If
                            gvTransactionForAccount.DataSource = oAccountDetailsCollection.AccountDetails
                            gvTransactionForAccount.DataBind()

                            Cache.Insert(ViewState("AccountpageCacheID"), oAccountDetailsCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

                            txtAccountCode.Text = gvTransactionForAccount.Rows(0).Cells(3).Text
                            txtDocumentRef.Text = Nothing

                            If (nAgentCnt > 0 AndAlso txtAccountCode.Text <> String.Empty) OrElse nAgentCnt = 0 Then
                                txtAccountName.Text = oAccountDetailsCollection.AccountName
                                txtActive.Text = oAccountDetailsCollection.AccountStatus
                            End If
                            ' txtAccountBalance.Text = oAccountDetailsCollection.AccountBalance

                            If ViewState("AccountCurrencyCode") IsNot Nothing Then
                                txtAccountBalance.Text = New Money(oAccountDetailsCollection.AccountBalance, ViewState("AccountCurrencyCode")).Formatted
                            Else
                                txtAccountBalance.Text = oAccountDetailsCollection.AccountBalance
                            End If

                            txtContactName.Text = oAccountDetailsCollection.ContactName

                            txtTelePhoneNumber.Text = oAccountDetailsCollection.PhoneNumber

                            If oAccountDetailsCollection.TransactionCurrencyOutStandingBalance <> "0.00" Then
                                txtTransactionCurrencyaccountbalance.Text = New Money(oAccountDetailsCollection.TransactionCurrencyOutStandingBalance, oAccountDetailsCollection.AccountDetails(0).CurrencyCode).Formatted
                            Else
                                txtTransactionCurrencyaccountbalance.Text = String.Empty
                            End If

                            gvGetTransactiondetails.Visible = False
                            gvDocRefTransactions.Visible = False
                            gvTransactionForAccount.Visible = True
                            '  gvTransactionForAccount.Columns(9).Visible = True
                            '    gvTransactionForAccount.Columns(11).Visible = True
                            ColumnSelectorExtender.Visible = True
                            'Done Parallel with Etana 3.1(This line was not in vb code)
                            ScriptManager.RegisterStartupScript(Page, Me.GetType(), "id", "$get('" + ColumnSelectorExtender.ClientID + "').style.display='block';", True)
                        Finally
                            oWebService = Nothing
                        End Try

                    Case "DrillDoc" 'to find the transactions based on the document Ref

                        Try
                            ViewState("ShowDrillDoc") = False
                            oAccountdetails.DocumentRef = e.CommandArgument
                            ViewState.Add(CNDocref, e.CommandArgument)
                            chkShowOutStandingTransaction.Checked = False
                            oAccountdetails.OutstandingOnly = False
                            If chkIncludeFuturebalanceTransaction.Checked = True Then
                                oAccountdetails.IsNewPF = True
                            End If
                            If chkShowOnlyLatest500Transactions.Checked = True Then
                                oAccountdetails.Display500 = True
                            End If
                            If chkIncludeFuturebalanceTransaction.Checked = True OrElse chkIncludeReversedReversalTransactions.Checked = True Then
                                oAccountdetails.IncludeReversedTran = True
                            End If
                            ClearControls()
                            txtDocumentRef.Text = e.CommandArgument
                            txtAccountCode.Text = String.Empty

                            oAccountDetailsCollection = oWebService.GetAccountDetails(oAccountdetails)

                            For iCount As Integer = 0 To oAccountDetailsCollection.AccountDetails.Count - 1
                                If rbtAmountColumn.Items(0).Selected = True Then 'TC 
                                    oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount
                                    oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAccountAmount
                                ElseIf rbtAmountColumn.Items(1).Selected = True Then 'BC
                                    oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).Amount
                                    oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAmount
                                ElseIf rbtAmountColumn.Items(2).Selected = True Then 'AC
                                    oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).AccountAmount
                                    oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAccountAmount
                                End If

                                'If rbtAmountColumn.Items(0).Selected = True Then 'BC
                                '    oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).Amount
                                '    oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAmount
                                'ElseIf rbtAmountColumn.Items(1).Selected = True Then 'TC
                                '    oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount
                                '    oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAccountAmount
                                'ElseIf rbtAmountColumn.Items(2).Selected = True Then 'AC
                                '    oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).AccountAmount
                                '    oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAccountAmount
                                'End If

                                If rbtOutStandingColumn.Items(0).Selected = True Then 'BC
                                    oAccountDetailsCollection.AccountDetails(iCount).OutstandingAmount = oAccountDetailsCollection.AccountDetails(iCount).OutstandingAmount
                                ElseIf rbtOutStandingColumn.Items(1).Selected = True Then 'TC
                                    oAccountDetailsCollection.AccountDetails(iCount).OutstandingAmount = oAccountDetailsCollection.AccountDetails(iCount).OutStandingCurrencyAmount
                                ElseIf rbtOutStandingColumn.Items(2).Selected = True Then 'AC
                                    oAccountDetailsCollection.AccountDetails(iCount).OutstandingAmount = oAccountDetailsCollection.AccountDetails(iCount).AccountOutStandingAmount
                                End If
                            Next
                            If (oNexusConfig.FinanceGridSize) = "" Then
                                gvDocRefTransactions.PageSize = 10
                            Else
                                gvDocRefTransactions.PageSize = CStr(oNexusConfig.FinanceGridSize)
                            End If
                            gvDocRefTransactions.DataSource = oAccountDetailsCollection.AccountDetails
                            gvDocRefTransactions.DataBind()

                            Cache.Insert(ViewState("DocRefpageCacheID"), oAccountDetailsCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

                            gvGetTransactiondetails.Visible = False
                            gvTransactionForAccount.Visible = False
                            gvDocRefTransactions.Visible = True
                            btnCancel.Visible = True

                        Finally
                            oWebService = Nothing
                        End Try
                End Select
            End If
        End Sub

        Protected Sub gvDocRefTransactions_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDocRefTransactions.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("TransdetailId") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.AccountDetails).TransdetailId)
                e.Row.Cells(1).Text = CType(e.Row.DataItem, NexusProvider.AccountDetails).TransDetailKeys

                '
                'gvDocRefTransactions.Columns(9).Visible = True
                ' gvDocRefTransactions.Columns(11).Visible = True

                Dim oOptionSetting As NexusProvider.OptionTypeSetting
                Dim oliDrillDoc As HtmlGenericControl = e.Row.Cells(28).FindControl("liDrillDoc")
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                oOptionSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 1031)
                Dim sShowDrillDoc As Boolean = True

                If ViewState("ShowDrillDoc") IsNot Nothing Then
                    sShowDrillDoc = Boolean.Parse(ViewState("ShowDrillDoc"))
                End If

                If oOptionSetting.OptionValue = "1" AndAlso sShowDrillDoc = True Then
                    oliDrillDoc.Visible = True
                Else
                    oliDrillDoc.Visible = False
                End If

                Dim oliViewAllocation As HtmlGenericControl = e.Row.Cells(28).FindControl("liViewAllocation")
                Dim ohyplink As LinkButton = e.Row.Cells(28).FindControl("lblhypViewAllocation")
                Dim iTransdetailKey As Integer
                Dim bIsSplitReceipt As Boolean = False
                Dim bIsLeadAgent As Boolean = False
                iTransdetailKey = CType(e.Row.DataItem, NexusProvider.AccountDetails).TransDetailKeys
                bIsLeadAgent = CType(e.Row.DataItem, NexusProvider.AccountDetails).IsLeadAgent
                bIsSplitReceipt = CType(e.Row.DataItem, NexusProvider.AccountDetails).IsSplitReceipt

                Dim amount As Double
                amount = CType(e.Row.DataItem, NexusProvider.AccountDetails).CurrencyAmount
                Dim outStandingamount As Double
                outStandingamount = CType(e.Row.DataItem, NexusProvider.AccountDetails).OutStandingCurrencyAmount

                sAccountKey = CType(e.Row.DataItem, NexusProvider.AccountDetails).AccountKey

                If amount <> outStandingamount Then
                    If HttpContext.Current.Session.IsCookieless Then
                        ohyplink.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/Viewallocation.aspx?Transdetailkey=" & iTransdetailKey & "&IsLeadAgent=" & bIsLeadAgent & "&AccountKey=" & sAccountKey & "&IsSplitReceipt=" & bIsSplitReceipt & "&modal=true&KeepThis=true&TB_iframe=true&height=550&width=800' , null);return false;"
                    Else
                        ohyplink.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "Modal/Viewallocation.aspx?Transdetailkey=" & iTransdetailKey & "&IsLeadAgent=" & bIsLeadAgent & "&AccountKey=" & sAccountKey & "&IsSplitReceipt=" & bIsSplitReceipt & "&modal=true&KeepThis=true&TB_iframe=true&height=550&width=800' , null);return false;"
                    End If
                Else
                    ohyplink.Visible = False
                    oliViewAllocation.Visible = False
                End If

                'To find the Deatils based on the Accoount Code
                Dim olinkbuttonl As LinkButton = e.Row.Cells(28).FindControl("lnkbtnDrillDoc")
                olinkbuttonl.CommandArgument = CType(e.Row.DataItem, NexusProvider.AccountDetails).DocRef
                olinkbuttonl.CommandName = "DrillDoc"

                Dim olinkbutton2 As LinkButton = e.Row.Cells(28).FindControl("lnkbtnDrillAccount")
                olinkbutton2.CommandArgument = CType(e.Row.DataItem, NexusProvider.AccountDetails).AccountKey
                olinkbutton2.Text = GetLocalResourceObject("lbl_DrillAccount").ToString()
                olinkbutton2.CommandName = "DrillAccount"

                If CType(e.Row.DataItem, NexusProvider.AccountDetails).PrimarySettled = True Then
                    e.Row.Cells(12).Text = "Yes"
                Else
                    e.Row.Cells(12).Text = "No"
                End If

                Dim TempGridRow As GridViewRow = e.Row
                Dim oItem As NexusProvider.AccountDetails = CType(e.Row.DataItem, NexusProvider.AccountDetails)

                If rbtAmountColumn.Items(0).Selected = True Then 'TC 
                    TempGridRow.Cells(11).Text = New Money(oItem.CurrencyAmount, oItem.BaseCurrencyCode.Trim).Formatted
                ElseIf rbtAmountColumn.Items(1).Selected = True Then 'BC
                    TempGridRow.Cells(11).Text = New Money(oItem.CurrencyAmount, oItem.CurrencyCode.Trim).Formatted
                ElseIf rbtAmountColumn.Items(2).Selected = True Then 'AC
                    TempGridRow.Cells(11).Text = New Money(oItem.CurrencyAmount, oItem.AccountCurrencyCode.Trim).Formatted
                End If

                'If rbtAmountColumn.Items(0).Selected = True Then 'BC
                '    TempGridRow.Cells(8).Text = New Money(oItem.CurrencyAmount, oItem.BaseCurrencyCode.Trim).Formatted
                'ElseIf rbtAmountColumn.Items(1).Selected = True Then 'TC
                '    TempGridRow.Cells(8).Text = New Money(oItem.CurrencyAmount, oItem.CurrencyCode.Trim).Formatted
                'ElseIf rbtAmountColumn.Items(2).Selected = True Then 'AC
                '    TempGridRow.Cells(8).Text = New Money(oItem.CurrencyAmount, oItem.AccountCurrencyCode.Trim).Formatted
                'End If

                If rbtOutStandingColumn.Items(0).Selected = True Then 'BC
                    TempGridRow.Cells(13).Text = New Money(oItem.OutstandingAmount, oItem.CurrencyCode.Trim).Formatted
                ElseIf rbtOutStandingColumn.Items(1).Selected = True Then 'TC
                    TempGridRow.Cells(13).Text = New Money(oItem.OutstandingAmount, oItem.BaseCurrencyCode.Trim).Formatted
                ElseIf rbtOutStandingColumn.Items(2).Selected = True Then 'AC
                    TempGridRow.Cells(13).Text = New Money(oItem.OutstandingAmount, oItem.AccountCurrencyCode.Trim).Formatted
                End If
            ElseIf e.Row.RowType = DataControlRowType.Header Then

            End If
        End Sub


        Protected Sub gvGetTransactiondetails_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvGetTransactiondetails.PageIndexChanging
            oAccountDetailsCollection = CType(Cache.Item(ViewState("TransactionpageCacheID")), NexusProvider.AccountDetailsDefaults)
            If oAccountDetailsCollection Is Nothing Then
                GetTransactionDetails()
            End If
            gvGetTransactiondetails.PageIndex = e.NewPageIndex
            If (oNexusConfig.FinanceGridSize) = "" Then
                gvGetTransactiondetails.PageSize = 10
            Else
                gvGetTransactiondetails.PageSize = CStr(oNexusConfig.FinanceGridSize)
            End If
            gvGetTransactiondetails.DataSource = oAccountDetailsCollection.AccountDetails 'ViewState(CNGetTransactionDetails)
            gvGetTransactiondetails.DataBind()
        End Sub

        Protected Sub gvTransactionForAccount_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTransactionForAccount.PageIndexChanging
            oAccountDetailsCollection = CType(Cache.Item(ViewState("AccountpageCacheID")), NexusProvider.AccountDetailsDefaults)
            If oAccountDetailsCollection Is Nothing Then
                GetTransactionDetails()
            End If
            gvTransactionForAccount.PageIndex = e.NewPageIndex
            If (oNexusConfig.FinanceGridSize) = "" Then
                gvTransactionForAccount.PageSize = 10
            Else
                gvTransactionForAccount.PageSize = CStr(oNexusConfig.FinanceGridSize)
            End If
            gvTransactionForAccount.DataSource = oAccountDetailsCollection.AccountDetails 'ViewState(CNGetTransactionDetailsForAccount)
            gvTransactionForAccount.DataBind()
        End Sub
        Protected Sub gvDocRefTransactions_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDocRefTransactions.RowCreated
            'Hide the AccKey & TransDetailsKey column
            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(0).Visible = False
                e.Row.Cells(1).Visible = False
                ' e.Row.Cells(21).Visible = False
            End If
        End Sub
        Protected Sub gvTransactionForAccount_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTransactionForAccount.RowCreated
            'Hide the AccKey & TransDetailsKey column
            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(0).Visible = False
                e.Row.Cells(1).Visible = False
                ' e.Row.Cells(21).Visible = False
            End If
        End Sub
        Protected Sub gvGetTransactiondetails_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvGetTransactiondetails.RowCreated
            'Hide the AccKey & TransDetailsKey column
            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
                'e.Row.Cells(1).Visible = False
                e.Row.Cells(23).Visible = False
            End If
        End Sub


        Protected Sub gvTransactionForAccount_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTransactionForAccount.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("TransdetailId") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.AccountDetails).TransdetailId)
                e.Row.Cells(1).Text = CType(e.Row.DataItem, NexusProvider.AccountDetails).TransDetailKeys

                '  gvDocRefTransactions.Columns(9).Visible = True
                ' gvDocRefTransactions.Columns(11).Visible = True

                Dim oliViewAllocation As HtmlGenericControl = e.Row.Cells(28).FindControl("liViewAllocation")
                Dim ohyplink As LinkButton = e.Row.Cells(27).FindControl("lblhypViewAllocation")
                Dim iTransdetailKey As Integer
                Dim bIsSplitReceipt As Boolean = False
                Dim bIsLeadAgent As Boolean = False

                bIsLeadAgent = CType(e.Row.DataItem, NexusProvider.AccountDetails).IsLeadAgent
                bIsSplitReceipt = CType(e.Row.DataItem, NexusProvider.AccountDetails).IsSplitReceipt


                iTransdetailKey = CType(e.Row.DataItem, NexusProvider.AccountDetails).TransDetailKeys
                Dim amount As Double
                amount = CType(e.Row.DataItem, NexusProvider.AccountDetails).CurrencyAmount
                Dim outStandingamount As Double
                outStandingamount = CType(e.Row.DataItem, NexusProvider.AccountDetails).OutStandingCurrencyAmount
                sAccountKey = CType(e.Row.DataItem, NexusProvider.AccountDetails).AccountKey

                If amount <> outStandingamount Then
                    If HttpContext.Current.Session.IsCookieless Then
                        ohyplink.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/Viewallocation.aspx?Transdetailkey=" & iTransdetailKey & "&AccountKey=" & sAccountKey & "&modal=true&KeepThis=true&TB_iframe=true&height=550&width=700' , null);return false;"
                    Else
                        ohyplink.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "Modal/Viewallocation.aspx?Transdetailkey=" & iTransdetailKey & "&AccountKey=" & sAccountKey & "&modal=true&KeepThis=true&TB_iframe=true&height=550&width=700' , null);return false;"
                    End If
                Else
                    ohyplink.Visible = False
                    oliViewAllocation.Visible = False
                End If



                If CType(e.Row.DataItem, NexusProvider.AccountDetails).PrimarySettled = True Then
                    e.Row.Cells(12).Text = "Yes"
                Else
                    e.Row.Cells(12).Text = "No"
                End If

                Dim TempGridRow As GridViewRow = e.Row
                Dim oItem As NexusProvider.AccountDetails = CType(e.Row.DataItem, NexusProvider.AccountDetails)

                If rbtAmountColumn.Items(0).Selected = True Then 'TC 
                    TempGridRow.Cells(11).Text = New Money(oItem.CurrencyAmount, oItem.BaseCurrencyCode.Trim).Formatted
                ElseIf rbtAmountColumn.Items(1).Selected = True Then 'BC
                    TempGridRow.Cells(11).Text = New Money(oItem.CurrencyAmount, oItem.CurrencyCode.Trim).Formatted
                ElseIf rbtAmountColumn.Items(2).Selected = True Then 'AC
                    TempGridRow.Cells(11).Text = New Money(oItem.CurrencyAmount, oItem.AccountCurrencyCode.Trim).Formatted
                End If

                'If rbtAmountColumn.Items(0).Selected = True Then 'BC
                '    TempGridRow.Cells(8).Text = New Money(oItem.CurrencyAmount, oItem.BaseCurrencyCode.Trim).Formatted
                'ElseIf rbtAmountColumn.Items(1).Selected = True Then 'TC
                '    TempGridRow.Cells(8).Text = New Money(oItem.CurrencyAmount, oItem.CurrencyCode.Trim).Formatted
                'ElseIf rbtAmountColumn.Items(2).Selected = True Then 'AC
                '    TempGridRow.Cells(8).Text = New Money(oItem.CurrencyAmount, oItem.AccountCurrencyCode.Trim).Formatted
                'End If

                If rbtOutStandingColumn.Items(0).Selected = True Then 'BC
                    TempGridRow.Cells(13).Text = New Money(oItem.OutstandingAmount, oItem.CurrencyCode.Trim).Formatted
                ElseIf rbtOutStandingColumn.Items(1).Selected = True Then 'TC
                    TempGridRow.Cells(13).Text = New Money(oItem.OutstandingAmount, oItem.BaseCurrencyCode.Trim).Formatted
                ElseIf rbtOutStandingColumn.Items(2).Selected = True Then 'AC
                    TempGridRow.Cells(13).Text = New Money(oItem.OutstandingAmount, oItem.AccountCurrencyCode.Trim).Formatted
                End If
            End If

        End Sub

        Public Function PopulateTransdetailkeys() As Boolean
            ' Looping through the Grid and getting the TrasdetailKey
            Dim TempAccountDetailsCollection As NexusProvider.AccountDetailsDefaults
            Dim iAccountKey As Integer = 0
            Dim bReturn_Status As Boolean = True
            TempAccountDetailsCollection = CType(Cache.Item(ViewState("TransactionpageCacheID")), NexusProvider.AccountDetailsDefaults)

            Session(CNTransDeatilsKeys) = Nothing
            For iCount As Integer = 0 To TempAccountDetailsCollection.AccountDetails.Count - 1
                If TempAccountDetailsCollection.AccountDetails(iCount).IsSelected = True Then
                    If iAccountKey > 0 AndAlso iAccountKey <> TempAccountDetailsCollection.AccountDetails(iCount).AccountKey Then
                        '   DiffAccount.IsValid = False
                        bReturn_Status = False
                        Exit For
                    ElseIf iAccountKey = 0 Then
                        iAccountKey = TempAccountDetailsCollection.AccountDetails(iCount).AccountKey
                    End If
                    arrlistTransid.Add(TempAccountDetailsCollection.AccountDetails(iCount).TransDetailKeys)
                End If
            Next

            If (String.IsNullOrEmpty(hiddenAccountCode.Value) = True Or hiddenAccountCode.Value = "0") _
            And bReturn_Status = True AndAlso iAccountKey > 0 Then
                hiddenAccountCode.Value = iAccountKey
                '  DiffAccount.IsValid = True
            End If
            Session(CNTransDeatilsKeys) = arrlistTransid

            Return bReturn_Status
        End Function

        Public Sub GetTransactionDetails()
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oOptionSetting As NexusProvider.OptionTypeSetting

            With oAccountdetails
                If Request.QueryString("Mode") = "CashListAllocation" Then
                    .AccountKey = CInt(Request("AllocationAccountkey"))
                Else
                    If txtAccountCode.Text.Trim().Length = 0 AndAlso String.IsNullOrEmpty(txtAgentCode.Text) Then
                        .AccountKey = Nothing

                    Else
                        If hiddenAccountCode.Value.Trim.Length = 0 Or hiddenAccountCode.Value = "0" Then
                            Dim oAccountSearchCriteria As New NexusProvider.AccountSearchCriteria
                            Dim oAccountSearchCollection As NexusProvider.AccountSearchResultCollection
                            If txtAccountCode.Text.Trim().Length = 0 AndAlso Not String.IsNullOrEmpty(txtAgentCode.Text) Then
                                oAccountSearchCriteria.ShortCode = txtAgentCode.Text.Trim
                            Else
                                oAccountSearchCriteria.ShortCode = txtAccountCode.Text.Trim
                            End If

                            oAccountSearchCollection = oWebService.FindAccounts(oAccountSearchCriteria)
                            If oAccountSearchCollection IsNot Nothing Then
                                If oAccountSearchCollection.Count > 0 Then
                                    hiddenAccountCode.Value = oAccountSearchCollection(0).AccountKey
                                    ViewState("PartyKey") = oAccountSearchCollection(0).PartyKey
                                Else
                                    CustVldDate.IsValid = False
                                    CustVldDate.ErrorMessage = GetLocalResourceObject("err_AccountCode")
                                End If
                            Else
                                CustVldDate.IsValid = False
                                CustVldDate.ErrorMessage = GetLocalResourceObject("err_AccountCode")
                            End If
                        End If
                        If txtAccountName.Text.Trim().Length = 0 AndAlso Not String.IsNullOrEmpty(txtAccountCode.Text) Then
                            .AccountKey = Nothing
                        Else
                            .AccountKey = CInt(IIf(hiddenAccountCode.Value = "", 0, hiddenAccountCode.Value))
                        End If

                        '.AccountKey = CInt(IIf(hiddenAccountCode.Value = "", 0, hiddenAccountCode.Value))

                        ViewState("Accountkey") = .AccountKey
                        ViewState("AccountName") = txtAccountCode.Text
                    End If
                End If

                If Not txtDocumentRef.Text.Trim().Length = 0 Then
                    .DocumentRef = txtDocumentRef.Text
                End If
                If Not CurrencyType.Text.Trim().Length = 0 Then
                    .CurrencyCode = CurrencyType.Value
                End If
                If Not txtTransactionCurrencyaccountbalance.Text.Trim().Length = 0 Then
                    .CurrencyCode = CurrencyType.Value
                End If
                If Not txtAmount.Text.Trim().Length = 0 Then
                    .CurrencyAmount = Convert.ToDouble(txtAmount.Text.Trim())
                End If

                If Not txtTolerance.Text.Trim().Length = 0 Then
                    .Tolerance = Convert.ToDouble(txtTolerance.Text.Trim())
                End If

                If Not DocumentType.Text.Trim().Length = 0 Then
                    .DocumentTypeCode = DocumentType.Value
                End If

                If Not DocumentGroup.Text.Trim().Length = 0 Then
                    .DocTypeGroupCode = DocumentGroup.Value
                End If
                If Not ddlPeriod.SelectedValue <> 0 Then
                    .PeriodKey = ddlPeriod.SelectedValue
                Else
                    .PeriodKey = Nothing
                End If
                If IsDate(txtFrom.Text) Then
                    .DateFrom = txtFrom.Text
                End If
                If IsDate(txtTo.Text) Then
                    .DateTo = txtTo.Text
                End If

                If Not txtPolicyNumber.Text.Trim().Length = 0 Then
                    .InsuranceRef = txtPolicyNumber.Text.Trim()
                End If

                If ddlOperatorName.SelectedValue <> "0" Then ' if default seleted
                    .Username = ddlOperatorName.SelectedValue
                End If

                If Not ddlBranchCode.Text.Trim().Length = 0 Then
                    .BranchCode = ddlBranchCode.SelectedValue
                Else
                    Dim oUserDetails As NexusProvider.UserDetails = CType(Current.Session(CNAgentDetails), NexusProvider.UserDetails)
                    Dim sSourceIds As String = String.Empty
                    For iCount As Integer = 0 To oUserDetails.ListOfBranches.Count - 1
                        sSourceIds = sSourceIds & oUserDetails.ListOfBranches(iCount).BranchKey & ","
                    Next
                    If Not String.IsNullOrEmpty(sSourceIds) Then
                        sSourceIds = Left(sSourceIds, Len(sSourceIds) - 1)
                        oAccountdetails.SourceArray = sSourceIds
                    End If
                End If
                If Not txtPurChaseInvoice.Text.Trim().Length = 0 Then
                    .PurchaseInvoiceNo = txtPurChaseInvoice.Text.Trim()
                End If
                If Not txtPurchaseOrderNo.Text.Trim().Length = 0 Then
                    .PurchaseOrderNo = txtPurchaseOrderNo.Text.Trim()
                End If
                If Not Department.Text.Trim().Length = 0 Then
                    .Department = Department.Value
                End If

                If Not txtMediaRef.Text.Trim().Length = 0 Then
                    .Spare = txtMediaRef.Text.Trim()
                ElseIf Not txtClaimNumber.Text.Trim().Length = 0 Then
                    .Spare = txtClaimNumber.Text.Trim()
                End If

                If chkShowOutStandingTransaction.Checked = True Then
                    .OutstandingOnly = True
                End If

                If chkIncludeFuturebalanceTransaction.Checked = True Then
                    .IsNewPF = True
                End If
                If chkShowOnlyLatest500Transactions.Checked = True Then
                    .Display500 = True
                End If

                If chkIncludeFuturebalanceTransaction.Checked = True Then
                    .IncludeReversedTran = True
                End If
                If chkIncludeReversedReversalTransactions.Checked = True Then
                    .IncludeReversedTran = True
                End If
                If Not txtAlternateRef.Text.Trim().Length = 0 Then
                    .AltReference = txtAlternateRef.Text.Trim()
                End If
                If Not txtBGRef.Text.Trim().Length = 0 Then
                    .BGRef = txtBGRef.Text.Trim()
                End If

                oOptionSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 1031)
                If oOptionSetting.OptionValue = "1" And gvDocRefTransactions.Visible = False And gvTransactionForAccount.Visible = False Then
                    .Rollup = True
                End If

                Dim sShowDrillDoc As Boolean = Boolean.Parse(ViewState("ShowDrillDoc"))
                If ViewState("PartyKey") IsNot Nothing And sShowDrillDoc = True And gvDocRefTransactions.Visible = True Then
                    .PartyCnt = Integer.Parse(ViewState("PartyKey"))
                End If

                If Not txtInsuredAccountCode.Text.Trim().Length = 0 And hiddenInsuredAccountKey.Value <> "" Then
                    .InsuredAccountKey = CInt(hiddenInsuredAccountKey.Value)
                End If

                If IsDate(txtDueDateTo.Text) Then
                    .DueDateTo = txtDueDateTo.Text
                End If

                If IsDate(txtDueDateFrom.Text) Then
                    .DueDateFrom = txtDueDateFrom.Text
                End If

            End With

            'SAM Call to retreive the details
            oAccountDetailsCollection = oWebService.GetAccountDetails(oAccountdetails, ddlBranchCode.SelectedValue)

            If gvDocRefTransactions.Visible = True And gvGetTransactiondetails.Visible = False And gvTransactionForAccount.Visible = False Then
                If oAccountDetailsCollection IsNot Nothing Then
                    For iCount As Integer = 0 To oAccountDetailsCollection.AccountDetails.Count - 1
                        If rbtAmountColumn.Items(0).Selected = True Then 'TC 
                            oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount
                            oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAccountAmount
                        ElseIf rbtAmountColumn.Items(1).Selected = True Then 'BC
                            oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).Amount
                            oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAmount
                        ElseIf rbtAmountColumn.Items(2).Selected = True Then 'AC
                            oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).AccountAmount
                            oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAccountAmount
                        End If

                        If rbtOutStandingColumn.Items(0).Selected = True Then 'BC
                            oAccountDetailsCollection.AccountDetails(iCount).OutstandingAmount = oAccountDetailsCollection.AccountDetails(iCount).OutstandingAmount
                        ElseIf rbtOutStandingColumn.Items(1).Selected = True Then 'TC
                            oAccountDetailsCollection.AccountDetails(iCount).OutstandingAmount = oAccountDetailsCollection.AccountDetails(iCount).OutStandingCurrencyAmount
                        ElseIf rbtOutStandingColumn.Items(2).Selected = True Then 'AC
                            oAccountDetailsCollection.AccountDetails(iCount).OutstandingAmount = oAccountDetailsCollection.AccountDetails(iCount).AccountOutStandingAmount
                        End If
                    Next
                End If

                For iCount As Integer = 0 To oAccountDetailsCollection.AccountDetails.Count - 1
                    oAccountDetailsCollection.AccountDetails(iCount).IsSelected = False
                Next
                If (oNexusConfig.FinanceGridSize) = "" Then
                    gvDocRefTransactions.PageSize = 10
                Else
                    gvDocRefTransactions.PageSize = CStr(oNexusConfig.FinanceGridSize)
                End If
                gvDocRefTransactions.DataSource = oAccountDetailsCollection.AccountDetails
                sAccountKey = ViewState("Accountkey")
                gvDocRefTransactions.DataBind()

                'putting the values in cache
                Cache.Insert(ViewState("DocRefpageCacheID"), oAccountDetailsCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

                If (nAgentCnt > 0 AndAlso txtAccountCode.Text <> String.Empty) OrElse nAgentCnt = 0 Then
                    txtAccountName.Text = oAccountDetailsCollection.AccountName
                    txtActive.Text = oAccountDetailsCollection.AccountStatus
                End If

                If ViewState("AccountCurrencyCode") IsNot Nothing Then
                    txtAccountBalance.Text = New Money(oAccountDetailsCollection.AccountBalance, ViewState("AccountCurrencyCode")).Formatted
                Else
                    txtAccountBalance.Text = oAccountDetailsCollection.AccountBalance
                End If

                txtContactName.Text = oAccountDetailsCollection.ContactName

                txtTelePhoneNumber.Text = oAccountDetailsCollection.PhoneNumber

                If oAccountDetailsCollection.TransactionCurrencyOutStandingBalance <> "0.00" Then
                    txtTransactionCurrencyaccountbalance.Text = New Money(oAccountDetailsCollection.TransactionCurrencyOutStandingBalance, oAccountDetailsCollection.AccountDetails(0).CurrencyCode).Formatted
                Else
                    txtTransactionCurrencyaccountbalance.Text = String.Empty
                End If
            ElseIf gvTransactionForAccount.Visible = True And gvGetTransactiondetails.Visible = False And gvDocRefTransactions.Visible = False Then

                If oAccountDetailsCollection IsNot Nothing Then
                    For iCount As Integer = 0 To oAccountDetailsCollection.AccountDetails.Count - 1
                        If rbtAmountColumn.Items(0).Selected = True Then 'TC 
                            oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount
                            oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAccountAmount
                        ElseIf rbtAmountColumn.Items(1).Selected = True Then 'BC
                            oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).Amount
                            oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAmount
                        ElseIf rbtAmountColumn.Items(2).Selected = True Then 'AC
                            oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).AccountAmount
                            oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAccountAmount
                        End If

                        'If rbtAmountColumn.Items(0).Selected = True Then 'BC
                        '    oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).Amount
                        '    oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAmount
                        'ElseIf rbtAmountColumn.Items(1).Selected = True Then 'TC
                        '    oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount
                        '    oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAccountAmount
                        'ElseIf rbtAmountColumn.Items(2).Selected = True Then 'AC
                        '    oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).AccountAmount
                        '    oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAccountAmount
                        'End If

                        If rbtOutStandingColumn.Items(0).Selected = True Then 'BC
                            oAccountDetailsCollection.AccountDetails(iCount).OutstandingAmount = oAccountDetailsCollection.AccountDetails(iCount).OutstandingAmount
                        ElseIf rbtOutStandingColumn.Items(1).Selected = True Then 'TC
                            oAccountDetailsCollection.AccountDetails(iCount).OutstandingAmount = oAccountDetailsCollection.AccountDetails(iCount).OutStandingCurrencyAmount
                        ElseIf rbtOutStandingColumn.Items(2).Selected = True Then 'AC
                            oAccountDetailsCollection.AccountDetails(iCount).OutstandingAmount = oAccountDetailsCollection.AccountDetails(iCount).AccountOutStandingAmount
                        End If
                    Next
                End If

                For iCount As Integer = 0 To oAccountDetailsCollection.AccountDetails.Count - 1
                    oAccountDetailsCollection.AccountDetails(iCount).IsSelected = False
                Next
                If (oNexusConfig.FinanceGridSize) = "" Then
                    gvTransactionForAccount.PageSize = 10
                Else
                    gvTransactionForAccount.PageSize = CStr(oNexusConfig.FinanceGridSize)
                End If
                gvTransactionForAccount.DataSource = oAccountDetailsCollection.AccountDetails
                gvTransactionForAccount.DataBind()

                'putting the values in cache
                Cache.Insert(ViewState("AccountpageCacheID"), oAccountDetailsCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

                If (nAgentCnt > 0 AndAlso txtAccountCode.Text <> String.Empty) OrElse nAgentCnt = 0 Then
                    txtAccountName.Text = oAccountDetailsCollection.AccountName
                    txtActive.Text = oAccountDetailsCollection.AccountStatus
                End If

                If ViewState("AccountCurrencyCode") IsNot Nothing Then
                    txtAccountBalance.Text = New Money(oAccountDetailsCollection.AccountBalance, ViewState("AccountCurrencyCode")).Formatted
                Else
                    txtAccountBalance.Text = oAccountDetailsCollection.AccountBalance
                End If

                txtContactName.Text = oAccountDetailsCollection.ContactName

                txtTelePhoneNumber.Text = oAccountDetailsCollection.PhoneNumber

                If oAccountDetailsCollection.TransactionCurrencyOutStandingBalance <> "0.00" Then
                    txtTransactionCurrencyaccountbalance.Text = New Money(oAccountDetailsCollection.TransactionCurrencyOutStandingBalance, oAccountDetailsCollection.AccountDetails(0).CurrencyCode).Formatted
                Else
                    txtTransactionCurrencyaccountbalance.Text = String.Empty
                End If
            Else
                If oAccountDetailsCollection IsNot Nothing Then
                    For iCount As Integer = 0 To oAccountDetailsCollection.AccountDetails.Count - 1
                        If rbtAmountColumn.Items(0).Selected = True Then 'TC 
                            oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount
                            oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAccountAmount
                        ElseIf rbtAmountColumn.Items(1).Selected = True Then 'BC
                            oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).Amount
                            oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAmount
                        ElseIf rbtAmountColumn.Items(2).Selected = True Then 'AC
                            oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).AccountAmount
                            oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAccountAmount
                        End If

                        'If rbtAmountColumn.Items(0).Selected = True Then 'BC
                        '    oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).Amount
                        '    oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAmount
                        'ElseIf rbtAmountColumn.Items(1).Selected = True Then 'TC
                        '    oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount
                        '    oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAccountAmount
                        'ElseIf rbtAmountColumn.Items(2).Selected = True Then 'AC
                        '    oAccountDetailsCollection.AccountDetails(iCount).CurrencyAmount = oAccountDetailsCollection.AccountDetails(iCount).AccountAmount
                        '    oAccountDetailsCollection.AccountDetails(iCount).PaidAmount = oAccountDetailsCollection.AccountDetails(iCount).PaidAccountAmount
                        'End If

                        If rbtOutStandingColumn.Items(0).Selected = True Then 'BC
                            oAccountDetailsCollection.AccountDetails(iCount).OutstandingAmount = oAccountDetailsCollection.AccountDetails(iCount).OutstandingAmount
                        ElseIf rbtOutStandingColumn.Items(1).Selected = True Then 'TC
                            oAccountDetailsCollection.AccountDetails(iCount).OutstandingAmount = oAccountDetailsCollection.AccountDetails(iCount).OutStandingCurrencyAmount
                        ElseIf rbtOutStandingColumn.Items(2).Selected = True Then 'AC
                            oAccountDetailsCollection.AccountDetails(iCount).OutstandingAmount = oAccountDetailsCollection.AccountDetails(iCount).AccountOutStandingAmount
                        End If
                    Next
                End If

                For iCount As Integer = 0 To oAccountDetailsCollection.AccountDetails.Count - 1
                    oAccountDetailsCollection.AccountDetails(iCount).IsSelected = False
                    Dim dueDate = oAccountDetailsCollection.AccountDetails(iCount).DueDate
                    If Not IsDate(dueDate) Then
                        oAccountDetailsCollection.AccountDetails(iCount).DueDate = oAccountDetailsCollection.AccountDetails(iCount).EffectiveDate
                    End If
                Next

                gvGetTransactiondetails.AllowPaging = True
                gvGetTransactiondetails.PageIndex = 0
                If (oNexusConfig.FinanceGridSize) = "" Then
                    gvGetTransactiondetails.PageSize = 10
                Else
                    gvGetTransactiondetails.PageSize = CStr(oNexusConfig.FinanceGridSize)
                End If
                gvGetTransactiondetails.DataSource = oAccountDetailsCollection.AccountDetails
                sAccountKey = ViewState("Accountkey")
                gvGetTransactiondetails.DataBind()

                'putting the values in cache
                Cache.Insert(ViewState("TransactionpageCacheID"), oAccountDetailsCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

                If (nAgentCnt > 0 AndAlso txtAccountCode.Text <> String.Empty) OrElse nAgentCnt = 0 Then
                    txtAccountName.Text = oAccountDetailsCollection.AccountName
                    txtActive.Text = oAccountDetailsCollection.AccountStatus
                End If
                If ViewState("AccountCurrencyCode") IsNot Nothing Then
                    txtAccountBalance.Text = New Money(oAccountDetailsCollection.AccountBalance, ViewState("AccountCurrencyCode")).Formatted
                Else
                    txtAccountBalance.Text = oAccountDetailsCollection.AccountBalance
                End If

                txtContactName.Text = oAccountDetailsCollection.ContactName

                txtTelePhoneNumber.Text = oAccountDetailsCollection.PhoneNumber

                If oAccountDetailsCollection.TransactionCurrencyOutStandingBalance <> "0.00" Then
                    txtTransactionCurrencyaccountbalance.Text = New Money(oAccountDetailsCollection.TransactionCurrencyOutStandingBalance, oAccountDetailsCollection.AccountDetails(0).CurrencyCode).Formatted
                Else
                    txtTransactionCurrencyaccountbalance.Text = String.Empty
                End If

                gvDocRefTransactions.Visible = False
                gvTransactionForAccount.Visible = False
                gvGetTransactiondetails.Visible = True
                If gvGetTransactiondetails.Visible = True Then
                    btnCancel.Visible = False
                End If
            End If
            chkselectedTransaction_CheckedChanged(Nothing, Nothing)
        End Sub

        Protected Sub chkselectedTransaction_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

            Dim iCountVarDebitColl, iCountVarCreditColl, iCountvarFullyallocated As Integer
            Dim chkTranskey As New CheckBox
            Dim oTransactionDetails As NexusProvider.AccountDetailsDefaults =
                    CType(Cache.Item(ViewState("TransactionpageCacheID")), NexusProvider.AccountDetailsDefaults)

            If oTransactionDetails Is Nothing Then
                GetTransactionDetails()
                oTransactionDetails = CType(Cache.Item(ViewState("TransactionpageCacheID")), NexusProvider.AccountDetailsDefaults)
                ScriptManager.RegisterStartupScript(Page, Me.GetType(), "transactionRefreshed", "alert('" & GetLocalResourceObject("msg_transactionRefreshed") & "');", True)
            End If
            Dim iCurrentLocation As Integer
            Dim iSelectedSRPandSPYEntry As Integer
            Dim iSelectedOtherEntry As Integer
            Dim nIsInstalmentEntry As Integer = 0

            Dim bInsatlmentAllocation As Boolean = False
            For iCount As Integer = 0 To gvGetTransactiondetails.Rows.Count - 1
                chkTranskey = CType(gvGetTransactiondetails.Rows(iCount).FindControl("chkselectedTransaction"), CheckBox)
                If chkTranskey.Checked = True Then

                    For iCurrentLocation = 0 To oTransactionDetails.AccountDetails.Count - 1
                        If gvGetTransactiondetails.Rows(iCount).Cells(1).Text <> "" Then
                            If oTransactionDetails.AccountDetails(iCurrentLocation).TransDetailKeys = Convert.ToInt32(gvGetTransactiondetails.Rows(iCount).Cells(1).Text) Then
                                oTransactionDetails.AccountDetails(iCurrentLocation).IsSelected = True
                                Exit For
                            End If
                        End If
                    Next

                Else
                    For iCurrentLocation = 0 To oTransactionDetails.AccountDetails.Count - 1

                        If gvGetTransactiondetails.Rows(iCount).Cells(1).Text <> "" Then
                            If oTransactionDetails.AccountDetails(iCurrentLocation).TransDetailKeys = Convert.ToInt32(gvGetTransactiondetails.Rows(iCount).Cells(1).Text) Then

                                oTransactionDetails.AccountDetails(iCurrentLocation).IsSelected = False
                                Exit For
                            End If
                        End If
                    Next

                End If

                'Update the cache
                Cache.Insert(ViewState("TransactionpageCacheID"), oTransactionDetails, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

            Next

            'Checdk Debit and Credit Entry
            Dim outStandingamount As Double
            Dim isValidTransactions As Boolean = True
            For iCurrentLocation = 0 To oTransactionDetails.AccountDetails.Count - 1
                If oTransactionDetails.AccountDetails(iCurrentLocation).IsSelected = True Then
                    outStandingamount = oTransactionDetails.AccountDetails(iCurrentLocation).OutstandingAmount
                    If outStandingamount > 0 Then
                        iCountVarDebitColl = iCountVarDebitColl + 1
                    Else
                        iCountVarCreditColl = iCountVarCreditColl + 1
                    End If
                    If outStandingamount = 0 Then
                        iCountvarFullyallocated = iCountvarFullyallocated + 1
                    End If
                    If Left(oTransactionDetails.AccountDetails(iCurrentLocation).DocRef.ToUpper, 3) = "SRP" Or
                       Left(oTransactionDetails.AccountDetails(iCurrentLocation).DocRef.ToUpper, 3) = "SPY" Then
                        iSelectedSRPandSPYEntry = iSelectedSRPandSPYEntry + 1
                    Else
                        iSelectedOtherEntry = iSelectedOtherEntry + 1
                    End If
                    'Check for Instalment Entry
                    If oTransactionDetails.AccountDetails(iCurrentLocation).DocRef <> "" Then
                        If ChkDocTypeIsInstalments(oTransactionDetails.AccountDetails(iCurrentLocation).DocRef.ToString().Substring(0, 3)) Then
                            isValidTransactions = False
                            Exit For
                        End If
                    End If
                End If
            Next

            'Stop further allocation if the transactions selected is of IND, IED, IRD or INC type
            For iCount As Integer = 0 To oTransactionDetails.AccountDetails.Count - 1
                ''Check if selected Document is not of INC/IED/IND/IRD type
                If oTransactionDetails.AccountDetails(iCount).DocRef.ToUpper.StartsWith("INC") Or
                   oTransactionDetails.AccountDetails(iCount).DocRef.ToUpper.StartsWith("IED") Or
                   oTransactionDetails.AccountDetails(iCount).DocRef.ToUpper.StartsWith("IND") Or
                   oTransactionDetails.AccountDetails(iCount).DocRef.ToUpper.StartsWith("IRD") Then
                    If oTransactionDetails.AccountDetails(iCount).IsSelected Then
                        nIsInstalmentEntry = nIsInstalmentEntry + 1
                    End If
                End If
            Next

            If nIsInstalmentEntry > 0 Then
                bInsatlmentAllocation = True
            Else
                bInsatlmentAllocation = False
            End If
            'Added for Integration
            If Request("Mode") = "CashListAllocation" Then
                'Logic for Allocation of Cashlist items
                ' if the Mode= cashlistitem then it comes here
                If iCountvarFullyallocated > 0 Then
                Else
                    If (iCountVarDebitColl >= 1 Or iCountVarCreditColl >= 1) AndAlso isValidTransactions = True Then
                        If PopulateTransdetailkeys() Then
                            btnAddTrasaction.Visible = True
                            If bInsatlmentAllocation Then
                                btnAddTrasaction.OnClientClick = "alert('" &
                                                                 CStr(GetLocalResourceObject("err_InstalmentAllocation")) &
                                                                 "'); return false;"
                            Else
                                btnAddTrasaction.OnClientClick = "return true;"
                            End If
                        End If
                    Else
                        btnAddTrasaction.OnClientClick = "return true;"
                        btnAddTrasaction.Visible = False
                    End If
                End If
            Else
                'Pick system option value from cache
                Dim bIsSingleCashListPaymentOrReciept As Boolean = CType(ViewState("IsSingleCashListPaymentOrReciept"), Boolean)

                'Added for Integration

                If iCountvarFullyallocated > 0 Then

                Else
                    ' If Minimum of One Credit and One Debit is selected than the Allocate Buttun is made visible
                    If iCountVarDebitColl > 0 AndAlso iCountVarCreditColl > 0 AndAlso iCountvarFullyallocated = 0 AndAlso isValidTransactions = True Then

                        If PopulateTransdetailkeys() Then
                            Dim iAccountKey As Integer = CInt(hiddenAccountCode.Value)

                            btnAllocate.Visible = True
                            If (iSelectedSRPandSPYEntry > 1 And iSelectedOtherEntry >= 1) And bIsSingleCashListPaymentOrReciept = True Then
                                btnAllocate.OnClientClick = Nothing
                                btnAllocate.Attributes.Add("onclick", "alert('" & GetLocalResourceObject("lbl_SingleSRPSPYError") & "');return false;")
                            Else
                                If HttpContext.Current.Session.IsCookieless Then
                                    btnAllocate.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/Allocate.aspx?Accountkey=" & iAccountKey & "&TransMode=SearchTrans&modal=true&KeepThis=true&TB_iframe=true&height=600&width=700' , null);return false;"
                                Else
                                    btnAllocate.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "Modal/Allocate.aspx?Accountkey=" & iAccountKey & "&TransMode=SearchTrans&modal=true&KeepThis=true&TB_iframe=true&height=600&width=700' , null);return false;"
                                End If
                            End If
                        End If
                    Else
                        btnAllocate.Visible = False
                    End If

                End If
            End If
            Dim oUserAuthority As New NexusProvider.UserAuthority
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

            oUserAuthority.UserCode = Session(CNLoginName)
            oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.AllowReverseReceipt
            oWebService = New NexusProvider.ProviderManager().Provider
            oWebService.GetUserAuthorityValue(oUserAuthority)

            If oUserAuthority.UserAuthorityValue = "1" Then
                Dim nCount, nAccount_Key As Integer
                Dim sDocRef As String = String.Empty
                Dim sMediaRef As String = String.Empty
                Dim nOutstandingamount, nAmount As Double
                For iCurrentLocation = 0 To oTransactionDetails.AccountDetails.Count - 1
                    If oTransactionDetails.AccountDetails(iCurrentLocation).IsSelected = True Then
                        nCount = nCount + 1 ''check if only single row is selected
                    End If
                Next

                If nCount = 1 Then ''retrieve value for single selected row
                    For iCurrentLocation = 0 To oTransactionDetails.AccountDetails.Count - 1
                        If oTransactionDetails.AccountDetails(iCurrentLocation).IsSelected = True Then
                            sDocRef = Left(oTransactionDetails.AccountDetails(iCurrentLocation).DocRef.ToUpper, 3)
                            sMediaRef = oTransactionDetails.AccountDetails(iCurrentLocation).MediaRef.ToUpper
                            nOutstandingamount = oTransactionDetails.AccountDetails(iCurrentLocation).OutstandingAmount
                            nAmount = oTransactionDetails.AccountDetails(iCurrentLocation).Amount
                            nAccount_Key = oTransactionDetails.AccountDetails(iCurrentLocation).PartyCnt
                        End If
                    Next

                End If

                ''reverse btn enabled if single unallocated SRP selected
                If nCount = 1 AndAlso sDocRef = "SRP" AndAlso Not sMediaRef.Contains("REVERSED") AndAlso Not sMediaRef.Contains("REVERSAL") = -1 _
                    AndAlso nOutstandingamount = nAmount AndAlso nAccount_Key <> "0" Then
                    btnReverse.Enabled = True
                Else
                    btnReverse.Enabled = False
                End If
            Else
                btnReverse.Enabled = False
            End If

        End Sub


        Protected Sub ClearControls()
            CurrencyType.Value = Nothing
            CurrencyType.Value = Nothing
            txtAmount.Text = String.Empty
            txtTolerance.Text = String.Empty
            DocumentType.Value = Nothing
            DocumentGroup.Value = Nothing
            txtFrom.Text = String.Empty
            txtTo.Text = Nothing
            txtPolicyNumber.Text = String.Empty
            ddlOperatorName.SelectedValue = Nothing
            If ddlBranchCode.Items.Count > 0 Then
                ddlBranchCode.SelectedIndex = 0
            End If
            txtDocumentRef.Text = ""
            txtPurChaseInvoice.Text = String.Empty
            txtPurchaseOrderNo.Text = String.Empty
            Department.Value = Nothing
            txtMediaRef.Text = String.Empty
            txtClaimNumber.Text = String.Empty
            txtAlternateRef.Text = String.Empty
            txtBGRef.Text = String.Empty
            hiddenAccountCode.Value = Nothing
            txtAccountCode.Text = String.Empty
            txtAccountName.Text = String.Empty
            txtContactName.Text = String.Empty
            txtAccountBalance.Text = String.Empty
            txtActive.Text = String.Empty
            txtTelePhoneNumber.Text = String.Empty
            btnReverse.Enabled = False
        End Sub

        Protected Sub CustVldDate_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles CustVldDate.ServerValidate


            'Date Validation
            If txtFrom.Text.Trim.Length <> 0 And IsDate(txtFrom.Text.Trim) = False Then
                args.IsValid = False
                CustVldDate.ErrorMessage = GetLocalResourceObject("err_FromDate")
                Exit Sub
            ElseIf IsDate(txtFrom.Text.Trim) = True Then
                If CDate(txtFrom.Text.Trim).ToShortDateString() < "01/01/1900" Then
                    args.IsValid = False
                    CustVldDate.ErrorMessage = GetLocalResourceObject("err_FromDate")
                    Exit Sub
                End If
            End If

            If txtTo.Text.Trim.Length <> 0 And IsDate(txtTo.Text.Trim) = False Then
                args.IsValid = False
                CustVldDate.ErrorMessage = GetLocalResourceObject("err_ToDate")
                Exit Sub
            ElseIf IsDate(txtTo.Text.Trim) = True Then
                If CDate(txtTo.Text.Trim).ToShortDateString() < "01/01/1900" Then
                    args.IsValid = False
                    CustVldDate.ErrorMessage = GetLocalResourceObject("err_ToDate")
                    Exit Sub
                End If
            End If
            'Due Date Validation
            If txtDueDateFrom.Text.Trim.Length <> 0 And IsDate(txtDueDateFrom.Text.Trim) = False Then
                args.IsValid = False
                CustVldDate.ErrorMessage = GetLocalResourceObject("err_DueDateFrom")
                Exit Sub
            ElseIf IsDate(txtDueDateFrom.Text.Trim) = True Then
                If CDate(txtDueDateFrom.Text.Trim).ToShortDateString() < "01/01/1900" Then
                    args.IsValid = False
                    CustVldDate.ErrorMessage = GetLocalResourceObject("err_DueDateFrom")
                    Exit Sub
                End If
            End If

            If txtDueDateTo.Text.Trim.Length <> 0 And IsDate(txtDueDateTo.Text.Trim) = False Then
                args.IsValid = False
                CustVldDate.ErrorMessage = GetLocalResourceObject("err_DueDateTo")
                Exit Sub
            ElseIf IsDate(txtDueDateTo.Text.Trim) = True Then
                If CDate(txtDueDateTo.Text.Trim).ToShortDateString() < "01/01/1900" Then
                    args.IsValid = False
                    CustVldDate.ErrorMessage = GetLocalResourceObject("err_DueDateTo")
                    Exit Sub
                End If
            End If

            If txtDueDateTo.Text.Trim.Length <> 0 And IsDate(txtDueDateTo.Text.Trim) = True And txtDueDateFrom.Text.Trim.Length <> 0 And IsDate(txtDueDateFrom.Text.Trim) = True Then
                If CDate(txtDueDateFrom.Text.Trim) > CDate(txtDueDateTo.Text.Trim) Then
                    args.IsValid = False
                    CustVldDate.ErrorMessage = GetLocalResourceObject("err_DueDate")
                    Exit Sub
                End If
            End If


            'Amount Validation
            Dim dResult As Double
            If args.IsValid = True And txtAmount.Text.Trim.Length <> 0 And Double.TryParse(txtAmount.Text.Trim, dResult) = False Then
                args.IsValid = False
                CustVldDate.ErrorMessage = GetLocalResourceObject("err_Amount")
                Exit Sub
            ElseIf args.IsValid = True And txtTolerance.Text.Trim.Length <> 0 And Double.TryParse(txtTolerance.Text.Trim, dResult) = False Then
                args.IsValid = False
                CustVldDate.ErrorMessage = GetLocalResourceObject("err_Tolerance")
                Exit Sub
            End If

        End Sub


        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If HttpContext.Current.Session.IsCookieless Then
                btnAccountCode.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/FindAccount.aspx?modal=true&KeepThis=true&FromPage=ACC&TB_iframe=true&height=500&width=800' , null);return false;"
            Else
                btnAccountCode.OnClientClick = "tb_show(null ,'../Modal/FindAccount.aspx?modal=true&KeepThis=true&FromPage=ACC&TB_iframe=true&height=500&width=800' , null);return false;"

            End If
        End Sub
        ''' <summary>
        ''' sort Document Referance Transactions according to the column clicked.
        ''' we need to store the current sort order in viewstate, and reverse it each time
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvDocRefTransactions_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvDocRefTransactions.Sorting

            Dim oOptionSetting As NexusProvider.OptionTypeSetting
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            oOptionSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 1031)
            Dim sShowDrillDoc As Boolean = Boolean.Parse(ViewState("ShowDrillDoc"))

            Dim oAccountDetailsCollection As NexusProvider.AccountDetailsDefaults

            If oOptionSetting.OptionValue = "1" AndAlso sShowDrillDoc = False Then
                oAccountDetailsCollection = Cache.Item(ViewState("DocRefpageCacheID"))
            ElseIf oOptionSetting.OptionValue = "1" AndAlso sShowDrillDoc = True Then
                oAccountDetailsCollection = Cache.Item(ViewState("RollUpDocRefpageCacheID"))
            Else
                oAccountDetailsCollection = Cache.Item(ViewState("DocRefpageCacheID"))
            End If

            oAccountDetailsCollection.AccountDetails.SortColumn = e.SortExpression
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
            oAccountDetailsCollection.AccountDetails.SortingOrder = _sortDirection
            oAccountDetailsCollection.AccountDetails.Sort()
            CType(sender, GridView).DataSource = oAccountDetailsCollection.AccountDetails
            CType(sender, GridView).DataBind()
        End Sub
        ''' <summary>
        ''' sort Transaction Detail according to the column clicked
        ''' we need to store the current sort order in viewstate, and reverse it each time
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvGetTransactiondetails_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvGetTransactiondetails.Sorting

            Dim oAccountDetailsCollection As NexusProvider.AccountDetailsDefaults = Cache.Item(ViewState("TransactionpageCacheID"))

            oAccountDetailsCollection.AccountDetails.SortColumn = e.SortExpression
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
            oAccountDetailsCollection.AccountDetails.SortingOrder = _sortDirection
            oAccountDetailsCollection.AccountDetails.Sort()
            CType(sender, GridView).DataSource = oAccountDetailsCollection.AccountDetails
            CType(sender, GridView).DataBind()
        End Sub
        ''' <summary>
        ''' sort the Transaction for Account detail according to the column clicked
        ''' we need to store the current sort order in viewstate, and reverse it each time
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvTransactionForAccount_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvTransactionForAccount.Sorting

            Dim oAccountDetailsCollection As NexusProvider.AccountDetailsDefaults = Cache.Item(ViewState("AccountpageCacheID"))

            oAccountDetailsCollection.AccountDetails.SortColumn = e.SortExpression
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
            oAccountDetailsCollection.AccountDetails.SortingOrder = _sortDirection
            oAccountDetailsCollection.AccountDetails.Sort()
            CType(sender, GridView).DataSource = oAccountDetailsCollection.AccountDetails
            CType(sender, GridView).DataBind()
        End Sub

        Protected Sub btnReverse_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReverse.Click
            HttpContext.Current.Session.Remove(CNReverseReceipt)
            Dim oTransactionDetails As NexusProvider.AccountDetailsDefaults =
                    CType(Cache.Item(ViewState("TransactionpageCacheID")), NexusProvider.AccountDetailsDefaults)

            Dim sCashlistItemKey As String = String.Empty
            Dim sTransdetailKey As String = String.Empty
            Dim sBranchKey As String = String.Empty
            Dim nCurrencyCode As String = String.Empty
            Dim nBankAccountID As Integer = 0

            For iCurrentLocation = 0 To oTransactionDetails.AccountDetails.Count - 1
                If oTransactionDetails.AccountDetails(iCurrentLocation).IsSelected = True Then
                    sTransdetailKey = oTransactionDetails.AccountDetails(iCurrentLocation).TransDetailKeys
                    sCashlistItemKey = oTransactionDetails.AccountDetails(iCurrentLocation).CashListItemKey
                    sBranchKey = oTransactionDetails.AccountDetails(iCurrentLocation).BranchKey
                    nBankAccountID = oTransactionDetails.AccountDetails(iCurrentLocation).BankAccountID
                    nCurrencyCode = oTransactionDetails.AccountDetails(iCurrentLocation).BaseCurrencyCode
                End If
            Next

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Try
                oWebService.ReverseCashListItem(v_TransdetailKey:=sTransdetailKey, v_CashListItemKey:=sCashlistItemKey, v_ReverseReasonCode:="OTHER")
                GetTransactionDetails() ''refresh grid
                btnReverse.Enabled = False

                Session(CNReverseReceipt) = sBranchKey + ";" + nBankAccountID.ToString + ";" + nCurrencyCode
                Dim sSuccessMsg = "alert('" + GetLocalResourceObject("successMsg") + "');window.location ='../secure/payment/CashListNew.aspx?Mode=ReverseReceipt';"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "showsuccessmsg", sSuccessMsg, True)

                Catch ex As NexusProvider.NexusException
            If ex.Errors(0).Code = "1000160" Then
                Dim sErrorMessage As String = "alert('" + GetLocalResourceObject("errorMsg") + "')"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "showerrormsg", sErrorMessage, True)
            Else ex.Errors(0).Code = "1000201"
                Dim sErrorDesc As String = ex.Errors(0).Description
                If sErrorDesc IsNot Nothing AndAlso Not String.IsNullOrEmpty(sErrorDesc) Then
                    sErrorDesc = sErrorDesc.Replace(vbCrLf, " ")
                End If
                    Dim sErrorMessage As String = "alert('" + sErrorDesc + "')"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "showerrormsg", sErrorMessage, True)
            End If
         End Try
        End Sub
        Public Function ChkDocTypeIsInstalments(ByVal v_sDocType As String) As Boolean

            Dim result As Boolean = False
            Try

                Select Case v_sDocType

                    Case "IDR", "ICR", "ICA", "IND", "INC", "IED", "IEC", "IRD", "IRC"
                        result = True
                End Select

                Return result

            Catch excep As System.Exception

                Return result
            End Try
        End Function
        Public Sub GetUserPreferredColumnList()
            ColumnSelectorExtender.Visible = True

            Dim sBranchCode As String = Session(CNBranchCode).ToString()
            Dim oUserPreferredColumns As NexusProvider.UserPreferredColumnList
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            oUserPreferredColumns = New NexusProvider.UserPreferredColumnList

            oUserPreferredColumns = oWebService.GetUserPreferredColumnList(sBranchCode, sInterfaceName)
            sColumnList = oUserPreferredColumns.ColumnList
            If (sColumnList IsNot Nothing) Then
                Dim gridColumnList() As String = sColumnList.Split(",")
                For i As Integer = 0 To gvGetTransactiondetails.Columns.Count - 1
                    If gvGetTransactiondetails.Columns.Item(i).HeaderText.ToString <> "" Then
                        gvGetTransactiondetails.Columns.Item(i).Visible = False
                        For colCnt As Integer = 0 To gridColumnList.Length - 1
                            If gridColumnList(colCnt) = gvGetTransactiondetails.Columns.Item(i).HeaderText Then
                                gvGetTransactiondetails.Columns.Item(i).Visible = True
                            End If
                        Next
                    End If
                Next

                For i As Integer = 0 To gvDocRefTransactions.Columns.Count - 1
                    If gvDocRefTransactions.Columns.Item(i).HeaderText.ToString <> "" Then
                        gvDocRefTransactions.Columns.Item(i).Visible = False
                        For colCnt As Integer = 0 To gridColumnList.Length - 1
                            If gridColumnList(colCnt) = gvDocRefTransactions.Columns.Item(i).HeaderText Then
                                gvDocRefTransactions.Columns.Item(i).Visible = True
                            End If
                        Next
                    End If
                Next
                For i As Integer = 0 To gvTransactionForAccount.Columns.Count - 1
                    If gvTransactionForAccount.Columns.Item(i).HeaderText.ToString <> "" Then
                        gvTransactionForAccount.Columns.Item(i).Visible = False
                        For colCnt As Integer = 0 To gridColumnList.Length - 1
                            If gridColumnList(colCnt) = gvTransactionForAccount.Columns.Item(i).HeaderText Then
                                gvTransactionForAccount.Columns.Item(i).Visible = True
                            End If
                        Next
                    End If
                Next
                If (oNexusConfig.FinanceGridSize) = "" Then
                    gvGetTransactiondetails.PageSize = 10
                    gvDocRefTransactions.PageSize = 10
                    gvTransactionForAccount.PageSize = 10
                Else
                    gvGetTransactiondetails.PageSize = CStr(oNexusConfig.FinanceGridSize)
                    gvDocRefTransactions.PageSize = CStr(oNexusConfig.FinanceGridSize)
                    gvTransactionForAccount.PageSize = CStr(oNexusConfig.FinanceGridSize)
                End If
                For selectColumn As Integer = 0 To ColumnSelectorExtender.ColumnSelector.Items.Count - 1
                    For colCnt As Integer = 0 To gridColumnList.Length - 1
                        If gridColumnList(colCnt) = ColumnSelectorExtender.ColumnSelector.Items.Item(selectColumn).Text Then
                            ColumnSelectorExtender.ColumnSelector.Items.Item(selectColumn).Selected = True
                            Exit For
                        End If
                    Next
                Next
            End If

        End Sub

        Public Sub UpdateUserPreferredColumnList()
            Dim oSamProvider As NexusProvider.SAMForInsurance.ProviderSAMForInsuranceV2 = New NexusProvider.SAMForInsurance.ProviderSAMForInsuranceV2()
            Dim oUserPreferredColumns As NexusProvider.UserPreferredColumnList
            Dim sBranchCode As String = Session(CNBranchCode).ToString()
            oUserPreferredColumns = New NexusProvider.UserPreferredColumnList
            sColumnList = ""
            For i As Integer = 0 To ColumnSelectorExtender.ColumnSelector.Items.Count - 1
                If ColumnSelectorExtender.ColumnSelector.Items.Item(i).Selected Then
                    sColumnList = sColumnList & IIf(sColumnList = "", "", ",") & ColumnSelectorExtender.ColumnSelector.Items.Item(i).Value
                End If
            Next

            oUserPreferredColumns.ColumnList = sColumnList
            oUserPreferredColumns.InterfaceName = sInterfaceName
            'oSearchTransactionColumn.IsSplitReceipt = .Item(SearchTransactionColumns.IsSplitReceipt).Selected


            oSamProvider.UpdateUserPreferredColumnList(oUserPreferredColumns, sBranchCode)
        End Sub

    End Class
End Namespace
