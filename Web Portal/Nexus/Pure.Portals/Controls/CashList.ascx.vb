Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports System.Web.Configuration.WebConfigurationManager

Namespace Nexus

    Partial Class Controls_CashListtab
        Inherits System.Web.UI.UserControl
        Dim BankAccountsCacheID As Guid
        Dim sMode As String
        Dim sCurrency As String = Nothing
        Dim hdnTabName As HiddenField

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'If Session("hfActiveTab") IsNot Nothing Or Session("hfPreviousTab") IsNot Nothing Then
            'If Session("hfActiveTab") = 0 Or Session("hfPreviousTab") = 0 Then
            Dim oNexusFrameWork As Nexus.Library.Config.NexusFrameWork = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork)
            hdnTabName = CType(CType(Nexus.Utils.GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("hdnTabName"), HiddenField)

            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            If Visible And Not IsPostBack Then

                sMode = Session("ModeValue")
                If Request.QueryString.HasKeys() Then
                    Session("ModeType") = sMode
                Else
                    Session("ModeType") = Nothing
                End If


                BindBankAccountsAndCurrency() ' this will bind the bank accounts and currency control
                PopulateCurrencyByBranch() ' Fill the currency as per Branch Selection
                Dim sSetFocusOnControl As String = Nothing
                Session(CNTransBranchCode) = Nothing

                Cash_List__Date.Text = DateTime.Now.ToShortDateString()

                If sMode = "Payment" Or sMode = "Receipt" Or sMode = "IP" Or sMode = "INS" Or sMode = "INSDEPOSIT" Then 'Cash Cheque Payment
                    If (Request.QueryString.HasKeys())Then
                        If sMode <> "INSDEPOSIT" Then
                            'Clear all the previous session variabls
                            ClearQuote()
                            ClearClaims()
                            Session(CNUnAllocatedClaimPayment) = Nothing
                        End If
                    End If
                    Dim oBranchCollection As NexusProvider.BranchCollection = CType(Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches
                    If oBranchCollection IsNot Nothing Then
                        'Branch Code Panel will be visible only if agent is assiciated with more then one branch
                        liBranchCode.Visible = True

                        'Sort collection before binding
                        oBranchCollection.SortColumn = "Description"
                        oBranchCollection.SortingOrder = NexusProvider.GenericComparer.SortOrder.Ascending
                        oBranchCollection.Sort()

                        If FindControl("ddlBranchCode") IsNot Nothing Then
                            'bind the control only if available
                            Dim dllBranchCode As DropDownList = CType(FindControl("ddlBranchCode"), DropDownList)
                            If ddlBranchCode IsNot Nothing Then
                                dllBranchCode.DataSource = oBranchCollection
                                dllBranchCode.DataBind()
                                dllBranchCode.SelectedValue = Session(CNBranchCode)
                            End If

                            If sMode = "ReverseReceipt" Then
                                If Session(CNReverseReceipt) IsNot Nothing Then
                                    Dim sReverseReceiptData As String = Session(CNReverseReceipt)
                                    For i = 0 To oBranchCollection.Count - 1
                                        If oBranchCollection(i).BranchKey = sReverseReceiptData.Split(";")(0) Then
                                            dllBranchCode.SelectedValue = oBranchCollection(i).Code
                                        End If
                                    Next
                                End If
                            End If
                        End If


                    End If
                End If

                If Request.QueryString.HasKeys() AndAlso sMode <> "" AndAlso sMode <> "PayNow"   Then 'Cash/Cheque Payment,Cash/Cheque Receipt,Insurer Payment
                    If sMode = "Payment" Then 'Cash/Cheque Payment
                        GISLookup_Type.Value = "P"
                        CashList_Currencies.SelectedValue = Session(CNCurrency)
                    ElseIf sMode = "Receipt" Then 'Cash/Cheque Receipt
                        GISLookup_Type.Value = "R"
                        CashList_Currencies.SelectedValue = Session(CNCurrency)
                    ElseIf sMode = "IP" Then 'Insurer Payment
                        Session(CNCashListItem) = Nothing
                        If Session(CNMarkedAmountSignForCashList) > 0 Then
                            GISLookup_Type.Value = "R"
                        Else
                            GISLookup_Type.Value = "P"
                        End If

                        'WPR48 
                        If Session(CNTransCurr) IsNot Nothing Then
                            If Not String.IsNullOrEmpty(Convert.ToString(Session(CNTransCurr))) Then
                                CashList_Currencies.SelectedValue = Session(CNTransCurr)
                            Else
                                CashList_Currencies.SelectedValue = Session(CNCurrency)
                            End If
                        Else
                            CashList_Currencies.SelectedValue = Session(CNCurrency)
                        End If
                    ElseIf sMode = "INS" OrElse sMode = "INSDEPOSIT" Then
                        GISLookup_Type.Value = "R"
                        sSetFocusOnControl = GISLookup_BankAccount.ClientID
                    End If

                    sSetFocusOnControl = GISLookup_BankAccount.ClientID

                ElseIf Session(CNQuoteMode) = QuoteMode.FullQuote Then 'New Business
                    If Session(CNAmountToPay) > 0 Then
                        GISLookup_Type.Value = "R"
                    Else
                        GISLookup_Type.Value = "P"
                    End If
                    CashList_Currencies.SelectedValue = Session(CNCurrenyCode)
                    sSetFocusOnControl = GISLookup_BankAccount.ClientID
                    GISLookup_Type.Enabled = False
                    Dim oBranchCollection As NexusProvider.BranchCollection = CType(Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches
                    If oBranchCollection IsNot Nothing Then
                        'Branch Code Panel will be visible only if agent is assiciated with more then one branch
                        liBranchCode.Visible = True

                        'Sort collection before binding
                        oBranchCollection.SortColumn = "Description"
                        oBranchCollection.SortingOrder = NexusProvider.GenericComparer.SortOrder.Ascending
                        oBranchCollection.Sort()

                        If FindControl("ddlBranchCode") IsNot Nothing Then
                            'bind the control only if available
                            Dim dllBranchCode As DropDownList = CType(FindControl("ddlBranchCode"), DropDownList)
                            If ddlBranchCode IsNot Nothing Then
                                dllBranchCode.DataSource = oBranchCollection
                                dllBranchCode.DataBind()
                                dllBranchCode.SelectedValue = Session(CNBranchCode)
                            End If
                        End If
                    End If


                ElseIf CType(Session(CNMode), Mode) = Mode.PayClaim Then
                    Dim oClaimReserve As NexusProvider.ClaimPerilReservePaymentTypeCollection = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(Session(CNClaimPerilIndex)).ClaimReserve
                    GISLookup_Type.Value = "CP"
                    'btnCancel.Visible = False

                    GISLookup_Type.Enabled = False

                    'By Default Payment currency should be set
                    If oClaimReserve IsNot Nothing Then
                        For Each oPaymentItem As NexusProvider.ClaimPerilReservePaymentType In oClaimReserve
                            If oPaymentItem.PayQueue > 0 Then
                                CashList_Currencies.SelectedValue = oPaymentItem.CurrencyCode.Trim
                            End If
                        Next
                    End If

                    sSetFocusOnControl = GISLookup_BankAccount.ClientID
                    BindBranchDropdown()
                    ddlBranchCode.Enabled = False
                ElseIf Session(CNUnAllocatedClaimPayment) IsNot Nothing Or Session(CNMode) = Mode.Recommend Then 'Claim Payment Processing

                    GISLookup_Type.Value = "CP"
                    GISLookup_Type.Enabled = False
                    CashList_Currencies.SelectedValue = Session(CNCurrenyCode)
                    sSetFocusOnControl = GISLookup_BankAccount.ClientID
                    BindBranchDropdown()
                    ddlBranchCode.Enabled = False

                ElseIf Session(CNQuoteCollectionFiles) IsNot Nothing Then
                    'Mark For Collection
                    GISLookup_Type.Value = "R"
                    CashList_Currencies.SelectedValue = Session(CNCurrenyCode)
                    sSetFocusOnControl = GISLookup_BankAccount.ClientID
                    GISLookup_Type.Enabled = False
                ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then
                    If Session(CNAmountToPay) > 0 Then
                        GISLookup_Type.Value = "R"
                    Else
                        GISLookup_Type.Value = "P"
                    End If
                    CashList_Currencies.SelectedValue = Session(CNCurrency)
                    sSetFocusOnControl = GISLookup_BankAccount.ClientID
                    GISLookup_Type.Enabled = False
                    BindBranchDropdown()
                    ddlBranchCode.Enabled = False
                Else
                    'Rest of the conditions
                    GISLookup_Type.Value = "R"
                    sSetFocusOnControl = GISLookup_BankAccount.ClientID
                End If

                If sMode = "ReverseReceipt" Then
                    'retrieve values from session and set to controls
                    Dim sReverseReceiptData As String = Session(CNReverseReceipt)
                    Dim oBankAccountsDetails As NexusProvider.AccountDetailsCollection
                    oBankAccountsDetails = Cache.Item(ViewState("BankAccountsCacheID"))
                    'For loop for checking bank seleted 
                    For i = 0 To oBankAccountsDetails.Count - 1
                        If Trim(oBankAccountsDetails(i).Code) = Trim(sReverseReceiptData.Split(";")(1)) Then
                            GISLookup_BankAccount.SelectedValue = oBankAccountsDetails(i).AccountKey
                            Exit For
                        End If
                    Next
                    CashList_Currencies.SelectedValue = sReverseReceiptData.Split(";")(2)
                    GISLookup_Type.Value = "R"
                End If
                'set the focus on the appropriate control depends on the condition applied
                'To set the Focus
                Page.SetFocus(sSetFocusOnControl)
                'added following event as it was not setting default currency on page load
                If sMode <> "ReverseReceipt" AndAlso Session(CNUnAllocatedClaimPayment) Is Nothing Then
                    Call GISLookup_BankAccount_SelectedIndexChanged(sender, e)
                End If
                Dim bShowSubBranchForPosting As Boolean = CType(GetSection("NexusFrameWork").Portals.Portal(CMS.Library.Portal.GetPortalID()), Nexus.Library.Config.Portal).ShowSubBranchForPosting

                If bShowSubBranchForPosting Then
                    liSubBranchCode.Visible = True
                    If ddlBranchCode IsNot Nothing Then
                        Dim sBranchCode As String = ddlBranchCode.SelectedValue.ToString
                        FillSubBranches(sBranchCode)
                    Else
                        liSubBranchCode.Visible = False
                    End If
                Else
                    liSubBranchCode.Visible = False
                End If

            End If
            '    End If
            'End If
        End Sub

        Public Sub FillSubBranches(Optional ByVal oBranchCode As String = Nothing)

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

            Dim oLookUP As New NexusProvider.LookupListCollection

            'sam call to retreive the list of branch from table source
            oLookUP = oWebService.GetList(NexusProvider.ListType.PMLookup, "Source", False, False, "Source_ID")

            If String.IsNullOrEmpty(oBranchCode) Then
                oBranchCode = Session(CNBranchCode)
            End If

            'Retreival of the Branch Key, which will latet identify the sub-branch
            'sam need barnch key to find the respective sub-branches of the selected branches
            Dim iBranchKey As Integer = 0
            For iBranchCount As Integer = 0 To oLookUP.Count - 1
                If oLookUP(iBranchCount).Code = oBranchCode Then
                    iBranchKey = oLookUP(iBranchCount).Key
                    Exit For
                End If
            Next

            'sam call to retreive the list of sub-branch from table source
            oLookUP = Nothing
            oLookUP = oWebService.GetList(NexusProvider.ListType.PMLookup, "Sub_Branch", False, False, "Source_ID", iBranchKey, oBranchCode)

            'Populating the sub-branch control with the retreived values
            If ddlSubBranchCode IsNot Nothing Then
                'existing items cleared
                ddlSubBranchCode.Items.Clear()
                ddlSubBranchCode.Items.Add(New ListItem("(Please Select)", ""))
                For iSubBranchCount As Integer = 0 To oLookUP.Count - 1
                    Dim lstSubBranch As New ListItem
                    lstSubBranch.Text = oLookUP(iSubBranchCount).Description
                    lstSubBranch.Value = Trim(oLookUP(iSubBranchCount).Code)
                    ddlSubBranchCode.Items.Add(lstSubBranch)
                    ddlSubBranchCode.DataBind()
                Next
            End If
        End Sub

        Private Sub BindBranchDropdown()
            Dim oBranchCollection As NexusProvider.BranchCollection = CType(Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches
            If oBranchCollection IsNot Nothing Then
                'Branch Code Panel will be visible only if agent is assiciated with more then one branch
                liBranchCode.Visible = True

                'Sort collection before binding
                oBranchCollection.SortColumn = "Description"
                oBranchCollection.SortingOrder = NexusProvider.GenericComparer.SortOrder.Ascending
                oBranchCollection.Sort()

                If FindControl("ddlBranchCode") IsNot Nothing Then
                    'bind the control only if available
                    Dim dllBranchCode As DropDownList = CType(FindControl("ddlBranchCode"), DropDownList)
                    If ddlBranchCode IsNot Nothing Then
                        dllBranchCode.DataSource = oBranchCollection
                        dllBranchCode.DataBind()
                        dllBranchCode.SelectedValue = Session(CNBranchCode)
                    End If
                End If
            End If
        End Sub

        Private Sub BindBankAccountsAndCurrency()
            'bind the bank accounts and currency control

            Dim oWebservice As NexusProvider.ProviderBase

            Dim oBankAccounts As NexusProvider.AccountDetailsCollection
            oWebservice = New NexusProvider.ProviderManager().Provider

            Dim sBranchCode As String
            If Not (Session("ModeValue") Is Nothing And CType(Session(CNMode), Mode) = Mode.PayClaim And Session(CNUnAllocatedClaimPayment) Is Nothing) Then
                If ddlBranchCode.SelectedIndex > -1 Then
                    sBranchCode = ddlBranchCode.SelectedItem.Value
                Else
                    sBranchCode = Session(CNBranchCode)
                End If
            End If
            'check and assign cache to oBankAccountsDetails variable
            oBankAccounts = oWebservice.GetBankAccounts(v_sBranchCode:=sBranchCode)
            'Sort collection before binding
            oBankAccounts.SortColumn = "BankAccountName"
            oBankAccounts.SortingOrder = NexusProvider.GenericComparer.SortOrder.Ascending
            oBankAccounts.Sort()

            'Generata unique cache id for storing different values and collections in cache
            BankAccountsCacheID = Guid.NewGuid()
            ViewState.Add("BankAccountsCacheID", BankAccountsCacheID.ToString)
            Cache.Insert(ViewState("BankAccountsCacheID"), oBankAccounts, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

            'Bind Bank Accounts with the control
            GISLookup_BankAccount.DataSource = oBankAccounts
            GISLookup_BankAccount.DataTextField = "Description"
            GISLookup_BankAccount.DataValueField = "AccountKey"

            GISLookup_BankAccount.DataBind()

            'Set Default Value for Bank Account 
            'added check for cMode =Reciept in folowing line
            If Session(CNMode) = Mode.PayClaim OrElse Session(CNMode) = Mode.Authorise OrElse Session(CNMode) = Mode.Recommend OrElse Session(CNUnAllocatedClaimPayment) IsNot Nothing OrElse sMode = "Receipt" OrElse sMode = "Payment" OrElse sMode = "IP" OrElse Session("ModeType") = "Payment" OrElse Session("ModeType") = "Receipt" OrElse sMode = "ReverseReceipt" Then
                Dim iMediaTypeId As Integer
                Dim iBankAccountId As Integer
                sBranchCode = ddlBranchCode.SelectedValue
                If sMode <> "ReverseReceipt" Then
                    If sMode = "Payment" OrElse Session("ModeType") = "Payment" Then
                        GetBankAccountDefault(iMediaTypeId, iBankAccountId, 1, sBranchCode:=sBranchCode)
                    ElseIf sMode = "Receipt" OrElse Session("ModeType") = "Receipt" Then
                        GetBankAccountDefault(iMediaTypeId, iBankAccountId, 2, sBranchCode:=sBranchCode)
                    Else
                        GetBankAccountDefault(iMediaTypeId, iBankAccountId, 3, sBranchCode:=sBranchCode)
                    End If
                End If

                'select bank in dropdown
                If iBankAccountId > 0 Then
                    GISLookup_BankAccount.SelectedValue = iBankAccountId
                End If
                Dim sBankAccountCode As String = ""
                'get bank code on the basis of code
                sBankAccountCode = GetCodeForKey(NexusProvider.ListType.PMLookup, GISLookup_BankAccount.SelectedValue, "BankAccount", True)
                If Not IsNothing(sBankAccountCode) Then
                    If sBankAccountCode.Length > 0 And sBankAccountCode.Length <= 10 Then
                        Dim nLength As Integer = 10 - sBankAccountCode.Length
                        sBankAccountCode = sBankAccountCode + Space(nLength)
                    End If
                End If
                hdnBankAccountCode.Value = sBankAccountCode
            End If
        End Sub

        Private Sub PopulateBankAccountsCache()
            Dim oBankAccounts As NexusProvider.AccountDetailsCollection
            Dim oWebservice As NexusProvider.ProviderBase

            oWebservice = New NexusProvider.ProviderManager().Provider
            oBankAccounts = oWebservice.GetBankAccounts()

            'Sort collection before binding
            oBankAccounts.SortColumn = "BankAccountName"
            oBankAccounts.SortingOrder = NexusProvider.GenericComparer.SortOrder.Ascending
            oBankAccounts.Sort()

            'Generata unique cache id for storing different values and collections in cache
            BankAccountsCacheID = Guid.NewGuid()
            ViewState.Add("BankAccountsCacheID", BankAccountsCacheID.ToString)
            Cache.Insert(ViewState("BankAccountsCacheID"), oBankAccounts, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
        End Sub

        'Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click

        '    If Page.IsValid Then

        '        Dim sMode As String = Nothing
        '        If (GISLookup_Type.Value = "R" AndAlso (Session("ModeValue") IsNot Nothing _
        '        AndAlso Session("ModeValue") <> "IP")) OrElse Session("ModeValue") = "INS" OrElse Session("ModeValue") = "INSDEPOSIT" Then
        '            sMode = "Receipt"
        '        ElseIf (GISLookup_Type.Value = "P" Or GISLookup_Type.Value = "CP") AndAlso (Session("ModeValue") IsNot Nothing _
        '        And Session("ModeValue") <> "IP") And Session(CNUnAllocatedClaimPayment) Is Nothing Then
        '            sMode = "Payment"
        '        End If
        '        Session("ModeType") = sMode
        '        Session(CNCurrenyCode) = CashList_Currencies.SelectedValue
        '        'New Business
        '        If Session("ModeValue") Is Nothing And (Session(CNQuoteMode) = QuoteMode.FullQuote _
        '                    Or Session(CNQuoteMode) = QuoteMode.MTAQuote) _
        '                    Or Session(CNQuoteCollectionFiles) IsNot Nothing Then

        '            Dim oPayNowReceipt As New NexusProvider.AddPayNowReceipt

        '            With oPayNowReceipt
        '                .Receipt.BankAccountName = GISLookup_BankAccount.SelectedItem.Text.Trim
        '                .Receipt.CurrencyCode = CashList_Currencies.SelectedValue
        '                .Receipt.ListDate = Cash_List__Date.Text
        '                .Receipt.Type = GISLookup_Type.Value
        '                .Receipt.CashListRef = ""
        '                .Receipt.SubbranchCode = ddlSubBranchCode.SelectedValue
        '            End With

        '            Session(CNPayNowReceipt) = oPayNowReceipt

        '            If Session(CNQuoteCollectionFiles) IsNot Nothing Then
        '                If GISLookup_Type.Value.Trim() = PaymentType.R.ToString() Then
        '                    Dim oReceiptCashListItem As New NexusProvider.ReceiptCashListItemType
        '                    With oReceiptCashListItem.CoreCashList
        '                        .BankAccountCode = hdnBankAccountCode.Value
        '                        .CurrencyCode = CashList_Currencies.SelectedValue
        '                        .ListDate = Cash_List__Date.Text
        '                        .TypeCode = GISLookup_Type.Value
        '                        .StatusCode = "E"
        '                        .BankAccountKey = GISLookup_BankAccount.SelectedValue
        '                        .SubBranchCode = ddlSubBranchCode.SelectedValue
        '                    End With
        '                    Session(CNCurrenyCode) = CashList_Currencies.SelectedValue
        '                    Session(CNCashListItem) = oReceiptCashListItem
        '                End If
        '            Else
        '                Session(CNCashListItem) = "CashListItem"
        '            End If
        '            Response.Redirect("~/secure/payment/PayNow.aspx", False)

        '            'Claim Payments, Claim Payment Processing, Authorise Claim Payments
        '        ElseIf Session("ModeValue") Is Nothing And CType(Session(CNMode), Mode) = Mode.PayClaim And Session(CNUnAllocatedClaimPayment) Is Nothing Then
        '            ' Pay Claim - Should redirect to CashListItem page.
        '            Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
        '            Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
        '            With oClaimOpen.ClaimPeril(Session(CNClaimPerilIndex)).Payment.CashList
        '                .ListDate = Cash_List__Date.Text
        '                .BankAccountCode = hdnBankAccountCode.Value
        '                .CurrencyCode = CashList_Currencies.SelectedValue
        '                .TypeCode = GISLookup_Type.Value
        '                .Reference = " "
        '                .StatusCode = "E"
        '                .SubbranchCode = ddlSubBranchCode.SelectedValue
        '            End With

        '            'Reset The Loss Currency Code, original values had lost due to the currency control
        '            Session(CNCurrenyCode) = oQuote.CurrencyCode
        '            Session(CNClaim) = oClaimOpen
        '            Response.Redirect("~/secure/payment/CashListItem.aspx", False)
        '        ElseIf Session("ModeValue") = "IP" Then 'Insurer Payments

        '            If GISLookup_Type.Value.Trim() = PaymentType.P.ToString() Or GISLookup_Type.Value.Trim() = PaymentType.CP.ToString() Then
        '                Dim oPaymentCashListItem As New NexusProvider.PaymentCashListItemType
        '                With oPaymentCashListItem.CoreCashList
        '                    .BankAccountCode = hdnBankAccountCode.Value
        '                    .CurrencyCode = CashList_Currencies.SelectedValue
        '                    .ListDate = Cash_List__Date.Text
        '                    .TypeCode = GISLookup_Type.Value
        '                    .StatusCode = "E"
        '                    .BankAccountKey = GISLookup_BankAccount.SelectedValue
        '                    .SubBranchCode = ddlSubBranchCode.SelectedValue
        '                End With
        '                Session(CNCashListItem) = oPaymentCashListItem
        '            ElseIf GISLookup_Type.Value.Trim() = PaymentType.R.ToString() Then
        '                'User is doing Insurer Payment - Receipts
        '                Dim oReceiptCashListItem As New NexusProvider.ReceiptCashListItemType
        '                With oReceiptCashListItem.CoreCashList
        '                    .BankAccountCode = hdnBankAccountCode.Value
        '                    .CurrencyCode = CashList_Currencies.SelectedValue
        '                    .ListDate = Cash_List__Date.Text
        '                    .TypeCode = GISLookup_Type.Value
        '                    .StatusCode = "E"
        '                    .BankAccountKey = GISLookup_BankAccount.SelectedValue
        '                    .SubBranchCode = ddlSubBranchCode.SelectedValue
        '                End With
        '                Session(CNCashListItem) = oReceiptCashListItem
        '            End If
        '            Session(CNTransBranchCode) = ddlBranchCode.SelectedValue
        '            'Server.Transfer("~/secure/payment/CashListItems.aspx?Mode=IP&Type=" + GISLookup_Type.Value + "&KeepThis=true&TB_iframe=true&PartyKey=" + Session("PartyKey"))

        '            Session("ModeValue") = "IP"
        '            Session("Type") = GISLookup_Type.Value
        '            Session("KeepThis") = True
        '            Session("TB_iframe") = True

        '            'CashListItems
        '            Dim changeTab2 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(2) a').tab('show')});"
        '            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab2", changeTab2, True)
        '            'Session("hfActiveTab") = 2
        '            Session("hfPreviousTab") = 2
        '        ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then 'MTA Cancellation Payment

        '            Dim oPayNowReceipt As New NexusProvider.AddPayNowReceipt

        '            With oPayNowReceipt
        '                .Receipt.BankAccountName = GISLookup_BankAccount.SelectedItem.Text.Trim
        '                .Receipt.CurrencyCode = CashList_Currencies.SelectedValue
        '                .Receipt.ListDate = Cash_List__Date.Text
        '                .Receipt.Type = GISLookup_Type.Value
        '                .Receipt.CashListRef = ""
        '                .Receipt.SubbranchCode = ddlSubBranchCode.SelectedValue
        '            End With

        '            Session(CNPayNowReceipt) = oPayNowReceipt

        '            Dim oPaymentCashListItem As New NexusProvider.PaymentCashListItemType
        '            With oPaymentCashListItem.CoreCashList
        '                .BankAccountCode = hdnBankAccountCode.Value
        '                .CurrencyCode = CashList_Currencies.SelectedValue
        '                .ListDate = Cash_List__Date.Text
        '                .TypeCode = GISLookup_Type.Value
        '                .StatusCode = "E"
        '                .BankAccountKey = GISLookup_BankAccount.SelectedValue
        '            End With
        '            Session(CNTransBranchCode) = ddlBranchCode.SelectedValue
        '            Session(CNCashListItem) = oPaymentCashListItem
        '            Response.Redirect("~/secure/payment/CashListItem.aspx", False)

        '        ElseIf Session("ModeType") = "Payment" Then 'Cash/Cheque Payments

        '            Dim oPaymentCashListItem As New NexusProvider.PaymentCashListItemType
        '            With oPaymentCashListItem.CoreCashList
        '                .BankAccountCode = hdnBankAccountCode.Value
        '                .CurrencyCode = CashList_Currencies.SelectedValue
        '                .ListDate = Cash_List__Date.Text
        '                .TypeCode = GISLookup_Type.Value
        '                .StatusCode = "E"
        '                .BankAccountKey = GISLookup_BankAccount.SelectedValue
        '                .SubBranchCode = ddlSubBranchCode.SelectedValue
        '            End With
        '            Session(CNTransBranchCode) = ddlBranchCode.SelectedValue
        '            Session(CNCashListItem) = oPaymentCashListItem
        '            Response.Redirect("~/secure/payment/CashListItems.aspx")
        '        ElseIf Session(CNUnAllocatedClaimPayment) IsNot Nothing Or Session(CNMode) = Mode.Recommend Then 'Claim Payment Processing
        '            Dim oPaymentCashListItem As New NexusProvider.PaymentCashListItemType

        '            With oPaymentCashListItem.CoreCashList
        '                .BankAccountCode = hdnBankAccountCode.Value
        '                .CurrencyCode = CashList_Currencies.SelectedValue
        '                .ListDate = Cash_List__Date.Text
        '                .TypeCode = GISLookup_Type.Value
        '                .StatusCode = "E"
        '                .BankAccountKey = GISLookup_BankAccount.SelectedValue
        '                .SubBranchCode = ddlSubBranchCode.SelectedValue
        '            End With

        '            Session(CNCashListItem) = oPaymentCashListItem
        '            Response.Redirect("~/secure/payment/CashListItem.aspx", False)
        '        ElseIf Session("ModeValue") = "INS" Then
        '            Dim oReceiptCashListItem As New NexusProvider.ReceiptCashListItemType
        '            With oReceiptCashListItem.CoreCashList
        '                .BankAccountCode = hdnBankAccountCode.Value
        '                .CurrencyCode = CashList_Currencies.SelectedValue
        '                .ListDate = Cash_List__Date.Text
        '                .TypeCode = GISLookup_Type.Value
        '                .StatusCode = "E"
        '                .BankAccountKey = GISLookup_BankAccount.SelectedValue
        '                .SubBranchCode = ddlSubBranchCode.SelectedValue
        '            End With
        '            Session(CNTransBranchCode) = ddlBranchCode.SelectedValue
        '            Session(CNCashListItem) = oReceiptCashListItem

        '            'Response.Redirect("~/secure/payment/CashListItem.aspx?Mode=INS&Type=" + GISLookup_Type.Value)
        '            Session("ModeValue") = "INS"
        '            Session("Type") = GISLookup_Type.Value
        '            'CashListItem
        '            Dim changeTab1 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(1) a').tab('show')});"
        '            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab1", changeTab1, True)
        '            'Session("hfActiveTab") = 1
        '            Session("hfPreviousTab") = 1
        '        ElseIf Session("ModeValue") = "INSDEPOSIT" Then
        '            Dim oReceiptCashListItem As New NexusProvider.ReceiptCashListItemType
        '            With oReceiptCashListItem.CoreCashList
        '                .BankAccountCode = hdnBankAccountCode.Value
        '                .CurrencyCode = CashList_Currencies.SelectedValue
        '                .ListDate = Today.Date
        '                .TypeCode = "R"
        '                .StatusCode = "E"
        '                .BankAccountKey = GISLookup_BankAccount.SelectedValue
        '                .SubBranchCode = ddlSubBranchCode.SelectedValue
        '            End With
        '            Session(CNTransBranchCode) = ddlBranchCode.SelectedValue
        '            Session(CNCashListItem) = oReceiptCashListItem

        '            'Response.Redirect("~/secure/payment/CashListItem.aspx?Mode=INSDEPOSIT&Type=" + GISLookup_Type.Value)
        '            Session("ModeValue") = "INSDEPOSIT"
        '            Session("Type") = GISLookup_Type.Value

        '            'CashListItem
        '            Dim changeTab1 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(1) a').tab('show')});"
        '            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab1", changeTab1, True)
        '            'Session("hfActiveTab") = 1
        '            Session("hfPreviousTab") = 1
        '        Else 'Cash/Cheque Receipts

        '            Dim oReceiptCashListItem As New NexusProvider.ReceiptCashListItemType
        '            With oReceiptCashListItem.CoreCashList
        '                .BankAccountCode = hdnBankAccountCode.Value
        '                .CurrencyCode = CashList_Currencies.SelectedValue
        '                .ListDate = Cash_List__Date.Text
        '                .TypeCode = GISLookup_Type.Value
        '                .StatusCode = "E"
        '                .BankAccountKey = GISLookup_BankAccount.SelectedValue
        '                .SubBranchCode = ddlSubBranchCode.SelectedValue
        '            End With
        '            Session(CNTransBranchCode) = ddlBranchCode.SelectedValue
        '            Session(CNCashListItem) = oReceiptCashListItem
        '            If Session(CNQuoteCollectionFiles) IsNot Nothing Then
        '                Response.Redirect("~/secure/payment/CashListItem.aspx", False)
        '            Else
        '                Response.Redirect("~/secure/payment/CashListItems.aspx", False)
        '            End If

        '        End If
        '    End If
        'End Sub

        'Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        '    If Session(CNUnAllocatedClaimPayment) IsNot Nothing Then  'Claim payment Processing
        '        Response.Redirect("~/secure/payment/claimpaymentprocessing.aspx", False)
        '    ElseIf Session("ModeType") = "Payment" Or Session("ModeType") = "Receipt" Then
        '        Dim sAgentStartPage As String = CType(GetSection("NexusFrameWork").Portals.Portal(CMS.Library.Portal.GetPortalID()), Nexus.Library.Config.Portal).AgentStartPage
        '        Response.Redirect(sAgentStartPage, False)
        '    ElseIf Session(CNQuoteCollectionFiles) IsNot Nothing Then
        '        Response.Redirect("~/secure/QuoteCollection.aspx", True)
        '    ElseIf Session("ModeValue") = "IP" Then
        '        'Insurer Payments
        '        Response.Redirect("~/secure/InsurerPayments.aspx?Mode=IP", False)
        '    ElseIf Session("ModeValue") = "INS" Then 'Instalments - WPR005
        '        Response.Redirect("~/PremiumFinance/PremiumFinancePlan.aspx?Type=EditPlan", True)
        '    ElseIf Session("ModeValue") = "ReverseReceipt" Then
        '        Response.Redirect("~/secure/SearchTransactions.aspx", False)
        '    Else 'New Business
        '        Response.Redirect("~/secure/PremiumDisplay.aspx", True)
        '    End If
        'End Sub

        Protected Sub cvCurrency_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvCurrency.ServerValidate

            If CashList_Currencies.SelectedValue Is Nothing Then
                args.IsValid = False
            End If

        End Sub
        Sub PopulateCurrencyByBranch()
            'Fill Currency
            Dim oCurrencyCollection As NexusProvider.CurrencyCollection
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim sBranchCode As String
            Dim oQuote As NexusProvider.Quote
            If Session("ModeValue") Is Nothing And CType(Session(CNMode), Mode) = Mode.PayClaim And Session(CNUnAllocatedClaimPayment) Is Nothing Then
                oQuote = Session(CNClaimQuote)
                sBranchCode = oQuote.BranchCode
            Else
                sBranchCode = Session(CNBranchCode)
            End If

            oCurrencyCollection = oWebService.GetCurrenciesByBranch(sBranchCode)
            'Sort the collection
            oCurrencyCollection.SortColumn = "Description"
            oCurrencyCollection.SortingOrder = NexusProvider.GenericComparer.SortOrder.Ascending
            oCurrencyCollection.Sort()

            CashList_Currencies.Items.Clear()
            For i As Integer = 0 To oCurrencyCollection.Count - 1
                Dim lstCurrency As New ListItem
                lstCurrency.Text = oCurrencyCollection.Item(i).Description.ToString.Trim
                lstCurrency.Value = Trim(oCurrencyCollection.Item(i).CurrencyCode.ToString)
                CashList_Currencies.Items.Add(lstCurrency)
            Next


            'need to update the quote if user has selected currency using Currencies Control
            If CashList_Currencies.Items.Count > 0 Then
                CashList_Currencies.SelectedValue = Session(CNCurrenyCode)
            End If
        End Sub

        'Added following event to change cuuency on the basis of bank selected
        Protected Sub GISLookup_BankAccount_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GISLookup_BankAccount.SelectedIndexChanged
            Dim i As Integer
            Dim oBankAccountsDetails As NexusProvider.AccountDetailsCollection
            'check and assign cache to oBankAccountsDetails variable
            If ViewState("BankAccountsCacheID") Is Nothing OrElse Cache.Item(ViewState("BankAccountsCacheID")) Is Nothing Then
                PopulateBankAccountsCache() 'Fill the BankAccountsCacheID cache
            End If
            oBankAccountsDetails = Cache.Item(ViewState("BankAccountsCacheID"))
            'For loop for checking bank seleted 
            For i = 0 To oBankAccountsDetails.Count - 1
                If oBankAccountsDetails(i).AccountKey = GISLookup_BankAccount.SelectedValue Then
                    sCurrency = oBankAccountsDetails(i).CurrencyCode
                    hdnBankAccountCode.Value = oBankAccountsDetails(i).Code
                    Exit For
                End If
            Next
            ' on basis of bank selected in GISLookup_BankAccount dropdown select currency in currency drop down
            If sMode = "IP" Then
                If Session(CNTransCurr) IsNot Nothing Then
                    CashList_Currencies.SelectedValue = Session(CNTransCurr).ToString()
                End If
            Else
                If sCurrency IsNot Nothing Then
                    CashList_Currencies.SelectedValue = sCurrency.Trim()
                End If
            End If
        End Sub

        Protected Sub ddlBranchCode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlBranchCode.SelectedIndexChanged
            BindBankAccountsAndCurrency()
            FillSubBranches(ddlBranchCode.SelectedValue)
        End Sub

        Public Sub okBtnClick()
            ' If Page.IsValid Then
            If (hdnTabName.Value = "tab-CashList") Then
                Dim sMode As String = Nothing
                If (GISLookup_Type.Value = "R" AndAlso (Session("ModeValue") IsNot Nothing _
                AndAlso Session("ModeValue") <> "IP")) OrElse Session("ModeValue") = "INS" OrElse Session("ModeValue") = "INSDEPOSIT" Then
                    sMode = "Receipt"
                ElseIf (GISLookup_Type.Value = "P" Or GISLookup_Type.Value = "CP") AndAlso (Session("ModeValue") IsNot Nothing _
                And Session("ModeValue") <> "IP") And Session(CNUnAllocatedClaimPayment) Is Nothing Then
                    sMode = "Payment"
                End If
                If Request.QueryString.HasKeys() Then
                    Session("ModeType") = sMode
                Else
                    Session("ModeType") = Nothing
                End If
                Session(CNCurrenyCode) = CashList_Currencies.SelectedValue
                'New Business
                If (Session("ModeValue") Is Nothing OrElse Session("ModeValue") = "PayNow") And (Session(CNQuoteMode) = QuoteMode.FullQuote _
                            Or Session(CNQuoteMode) = QuoteMode.MTAQuote) _
                            Or Session(CNQuoteCollectionFiles) IsNot Nothing Then

                    Dim oPayNowReceipt As New NexusProvider.AddPayNowReceipt

                    With oPayNowReceipt
                        .Receipt.BankAccountName = GISLookup_BankAccount.SelectedItem.Text.Trim
                        .Receipt.CurrencyCode = CashList_Currencies.SelectedValue
                        .Receipt.ListDate = Cash_List__Date.Text
                        .Receipt.Type = GISLookup_Type.Value
                        .Receipt.CashListRef = ""
                        .Receipt.SubbranchCode = ddlSubBranchCode.SelectedValue
                    End With

                    Session(CNPayNowReceipt) = oPayNowReceipt

                    If Session(CNQuoteCollectionFiles) IsNot Nothing Then
                        If GISLookup_Type.Value.Trim() = PaymentType.R.ToString() Then
                            Dim oReceiptCashListItem As New NexusProvider.ReceiptCashListItemType
                            With oReceiptCashListItem.CoreCashList
                                .BankAccountCode = hdnBankAccountCode.Value
                                .CurrencyCode = CashList_Currencies.SelectedValue
                                .ListDate = Cash_List__Date.Text
                                .TypeCode = GISLookup_Type.Value
                                .StatusCode = "E"
                                .BankAccountKey = GISLookup_BankAccount.SelectedValue
                                .SubBranchCode = ddlSubBranchCode.SelectedValue
                            End With
                            Session(CNCurrenyCode) = CashList_Currencies.SelectedValue
                            Session(CNCashListItem) = oReceiptCashListItem
                        End If
                    ElseIf Session("ModeType") = "Payment" Then 'Cash/Cheque Payments

                        Dim oPaymentCashListItem As New NexusProvider.PaymentCashListItemType
                        With oPaymentCashListItem.CoreCashList
                            .BankAccountCode = hdnBankAccountCode.Value
                            .CurrencyCode = CashList_Currencies.SelectedValue
                            .ListDate = Cash_List__Date.Text
                            .TypeCode = GISLookup_Type.Value
                            .StatusCode = "E"
                            .BankAccountKey = GISLookup_BankAccount.SelectedValue
                            .SubBranchCode = ddlSubBranchCode.SelectedValue
                        End With
                        Session(CNTransBranchCode) = ddlBranchCode.SelectedValue
                        Session(CNCashListItem) = oPaymentCashListItem
                    Else
                        Session(CNCashListItem) = "CashListItem"
                    End If
                    'Response.Redirect("~/secure/payment/PayNow.aspx", False)

                    Dim changeTab1 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(1) a').tab('show')});"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab1", changeTab1, True)
                    Session("hfPreviousTab") = 1

                    'Claim Payments, Claim Payment Processing, Authorise Claim Payments
                ElseIf Request.QueryString.HasKeys = False And CType(Session(CNMode), Mode) = Mode.PayClaim And Session(CNUnAllocatedClaimPayment) Is Nothing Then
                    ' Pay Claim - Should redirect to CashListItem page.
                    Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                    Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
                    With oClaimOpen.ClaimPeril(Session(CNClaimPerilIndex)).Payment.CashList
                        .ListDate = Cash_List__Date.Text
                        .BankAccountCode = hdnBankAccountCode.Value
                        .CurrencyCode = CashList_Currencies.SelectedValue
                        .TypeCode = GISLookup_Type.Value
                        .Reference = " "
                        .StatusCode = "E"
                        .SubbranchCode = ddlSubBranchCode.SelectedValue
                    End With

                    'Reset The Loss Currency Code, original values had lost due to the currency control
                    Session(CNCurrenyCode) = oQuote.CurrencyCode
                    Session(CNClaim) = oClaimOpen
                    'Response.Redirect("~/secure/payment/CashListItem.aspx", False)
                    'CashListItem
                    Dim changeTab1 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(1) a').tab('show')});"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab1", changeTab1, True)
                    'Session("hfActiveTab") = 1
                    Session("hfPreviousTab") = 1
                ElseIf Session("ModeValue") = "IP" Then 'Insurer Payments

                    If GISLookup_Type.Value.Trim() = PaymentType.P.ToString() Or GISLookup_Type.Value.Trim() = PaymentType.CP.ToString() Then
                        Dim oPaymentCashListItem As New NexusProvider.PaymentCashListItemType
                        With oPaymentCashListItem.CoreCashList
                            .BankAccountCode = hdnBankAccountCode.Value
                            .CurrencyCode = CashList_Currencies.SelectedValue
                            .ListDate = Cash_List__Date.Text
                            .TypeCode = GISLookup_Type.Value
                            .StatusCode = "E"
                            .BankAccountKey = GISLookup_BankAccount.SelectedValue
                            .SubBranchCode = ddlSubBranchCode.SelectedValue
                        End With
                        Session(CNCashListItem) = oPaymentCashListItem
                    ElseIf GISLookup_Type.Value.Trim() = PaymentType.R.ToString() Then
                        'User is doing Insurer Payment - Receipts
                        Dim oReceiptCashListItem As New NexusProvider.ReceiptCashListItemType
                        With oReceiptCashListItem.CoreCashList
                            .BankAccountCode = hdnBankAccountCode.Value
                            .CurrencyCode = CashList_Currencies.SelectedValue
                            .ListDate = Cash_List__Date.Text
                            .TypeCode = GISLookup_Type.Value
                            .StatusCode = "E"
                            .BankAccountKey = GISLookup_BankAccount.SelectedValue
                            .SubBranchCode = ddlSubBranchCode.SelectedValue
                        End With
                        Session(CNCashListItem) = oReceiptCashListItem
                    End If
                    Session(CNTransBranchCode) = ddlBranchCode.SelectedValue
                    'Server.Transfer("~/secure/payment/CashListItems.aspx?Mode=IP&Type=" + GISLookup_Type.Value + "&KeepThis=true&TB_iframe=true&PartyKey=" + Session("PartyKey"))

                    Session("ModeValue") = "IP"
                    Session("Type") = GISLookup_Type.Value
                    Session("KeepThis") = True
                    Session("TB_iframe") = True

                    'CashListItems
                    Dim changeTab2 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(2) a').tab('show')});"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab2", changeTab2, True)
                    'Session("hfActiveTab") = 2
                    Session("hfPreviousTab") = 2

                ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then 'MTA Cancellation Payment

                    Dim oPayNowReceipt As New NexusProvider.AddPayNowReceipt

                    With oPayNowReceipt
                        .Receipt.BankAccountName = GISLookup_BankAccount.SelectedItem.Text.Trim
                        .Receipt.CurrencyCode = CashList_Currencies.SelectedValue
                        .Receipt.ListDate = Cash_List__Date.Text
                        .Receipt.Type = GISLookup_Type.Value
                        .Receipt.CashListRef = ""
                        .Receipt.SubbranchCode = ddlSubBranchCode.SelectedValue
                    End With

                    Session(CNPayNowReceipt) = oPayNowReceipt

                    Dim oPaymentCashListItem As New NexusProvider.PaymentCashListItemType
                    With oPaymentCashListItem.CoreCashList
                        .BankAccountCode = hdnBankAccountCode.Value
                        .CurrencyCode = CashList_Currencies.SelectedValue
                        .ListDate = Cash_List__Date.Text
                        .TypeCode = GISLookup_Type.Value
                        .StatusCode = "E"
                        .BankAccountKey = GISLookup_BankAccount.SelectedValue
                    End With
                    Session(CNTransBranchCode) = ddlBranchCode.SelectedValue
                    Session(CNCashListItem) = oPaymentCashListItem
                    'Response.Redirect("~/secure/payment/CashListItem.aspx", False)
                    'CashListItem
                    Dim changeTab1 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(1) a').tab('show')});"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab1", changeTab1, True)
                    'Session("hfActiveTab") = 1
                    Session("hfPreviousTab") = 1
                ElseIf Session("ModeType") = "Payment" Then 'Cash/Cheque Payments

                    Dim oPaymentCashListItem As New NexusProvider.PaymentCashListItemType
                    With oPaymentCashListItem.CoreCashList
                        .BankAccountCode = hdnBankAccountCode.Value
                        .CurrencyCode = CashList_Currencies.SelectedValue
                        .ListDate = Cash_List__Date.Text
                        .TypeCode = GISLookup_Type.Value
                        .StatusCode = "E"
                        .BankAccountKey = GISLookup_BankAccount.SelectedValue
                        .SubBranchCode = ddlSubBranchCode.SelectedValue
                    End With
                    Session(CNTransBranchCode) = ddlBranchCode.SelectedValue
                    Session(CNCashListItem) = oPaymentCashListItem
                    'Response.Redirect("~/secure/payment/CashListItems.aspx")
                    'CashListItems
                    Dim changeTab2 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(2) a').tab('show')});"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab2", changeTab2, True)
                    'Session("hfActiveTab") = 2
                    Session("hfPreviousTab") = 2
                ElseIf Session(CNUnAllocatedClaimPayment) IsNot Nothing Or Session(CNMode) = Mode.Recommend Then 'Claim Payment Processing
                    Dim oPaymentCashListItem As New NexusProvider.PaymentCashListItemType

                    With oPaymentCashListItem.CoreCashList
                        .BankAccountCode = hdnBankAccountCode.Value
                        .CurrencyCode = CashList_Currencies.SelectedValue
                        .ListDate = Cash_List__Date.Text
                        .TypeCode = GISLookup_Type.Value
                        .StatusCode = "E"
                        .BankAccountKey = GISLookup_BankAccount.SelectedValue
                        .SubBranchCode = ddlSubBranchCode.SelectedValue
                    End With

                    Session(CNCashListItem) = oPaymentCashListItem
                    'Response.Redirect("~/secure/payment/CashListItem.aspx", False)
                    'CashListItem
                    Dim changeTab1 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(1) a').tab('show')});"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab1", changeTab1, True)
                    'Session("hfActiveTab") = 1
                    Session("hfPreviousTab") = 1
                ElseIf Session("ModeValue") = "INS" Then
                    Dim oReceiptCashListItem As New NexusProvider.ReceiptCashListItemType
                    With oReceiptCashListItem.CoreCashList
                        .BankAccountCode = hdnBankAccountCode.Value
                        .CurrencyCode = CashList_Currencies.SelectedValue
                        .ListDate = Cash_List__Date.Text
                        .TypeCode = GISLookup_Type.Value
                        .StatusCode = "E"
                        .BankAccountKey = GISLookup_BankAccount.SelectedValue
                        .SubBranchCode = ddlSubBranchCode.SelectedValue
                    End With
                    Session(CNTransBranchCode) = ddlBranchCode.SelectedValue
                    Session(CNCashListItem) = oReceiptCashListItem

                    'Response.Redirect("~/secure/payment/CashListItem.aspx?Mode=INS&Type=" + GISLookup_Type.Value)
                    Session("ModeValue") = "INS"
                    Session("Type") = GISLookup_Type.Value
                    'CashListItem
                    Dim changeTab1 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(1) a').tab('show')});"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab1", changeTab1, True)
                    'Session("hfActiveTab") = 1
                    Session("hfPreviousTab") = 1
                ElseIf Session("ModeValue") = "INSDEPOSIT" Then
                    Dim oReceiptCashListItem As New NexusProvider.ReceiptCashListItemType
                    With oReceiptCashListItem.CoreCashList
                        .BankAccountCode = hdnBankAccountCode.Value
                        .CurrencyCode = CashList_Currencies.SelectedValue
                        .ListDate = Today.Date
                        .TypeCode = "R"
                        .StatusCode = "E"
                        .BankAccountKey = GISLookup_BankAccount.SelectedValue
                        .SubBranchCode = ddlSubBranchCode.SelectedValue
                    End With
                    Session(CNTransBranchCode) = ddlBranchCode.SelectedValue
                    Session(CNCashListItem) = oReceiptCashListItem

                    'Response.Redirect("~/secure/payment/CashListItem.aspx?Mode=INSDEPOSIT&Type=" + GISLookup_Type.Value)
                    Session("ModeValue") = "INSDEPOSIT"
                    Session("Type") = GISLookup_Type.Value

                    'CashListItem
                    Dim changeTab1 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(1) a').tab('show')});"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab1", changeTab1, True)
                    'Session("hfActiveTab") = 1
                    Session("hfPreviousTab") = 1
                Else 'Cash/Cheque Receipts

                    Dim oReceiptCashListItem As New NexusProvider.ReceiptCashListItemType
                    With oReceiptCashListItem.CoreCashList
                        .BankAccountCode = hdnBankAccountCode.Value
                        .CurrencyCode = CashList_Currencies.SelectedValue
                        .ListDate = Cash_List__Date.Text
                        .TypeCode = GISLookup_Type.Value
                        .StatusCode = "E"
                        .BankAccountKey = GISLookup_BankAccount.SelectedValue
                        .SubBranchCode = ddlSubBranchCode.SelectedValue
                    End With
                    Session(CNTransBranchCode) = ddlBranchCode.SelectedValue
                    Session(CNCashListItem) = oReceiptCashListItem
                    If Session(CNQuoteCollectionFiles) IsNot Nothing Then
                        'Response.Redirect("~/secure/payment/CashListItem.aspx", False)
                        'CashListItem
                        Dim changeTab1 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(1) a').tab('show')});"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab1", changeTab1, True)
                        'Session("hfActiveTab") = 1
                        Session("hfPreviousTab") = 1
                    Else
                        'Response.Redirect("~/secure/payment/CashListItems.aspx", False)
                        'CashListItems
                        ' Dim changeTab2 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(2) a').tab('show');});"
                        'Dim oNexusFrameWork As Nexus.Library.Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork)
                        'Dim hdnTabName As HiddenField = CType(CType(Nexus.Utils.GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("hdnTabName"), HiddenField)
                        'hdnTabName.Value = "tab-CashList_Items"
                        'Dim changeTab2 As String = " $(document).ready(function () {alert($('#Tabs a[href=#tab-CashList_Items]'));$('#Tabs a[href=#tab-CashList_Items]').tab('show');__doPostBack('<%=btnCashListItemNext.UniqueID%>', 'CRCashListItem');});"
                        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab2", changeTab2, True)
                        'Session("hfActiveTab") = 2
                        ' Session("hfPreviousTab") = 2
                    End If
                End If
            End If
            'End If
        End Sub
        Public Sub cancelBtnClick()
            If Session(CNUnAllocatedClaimPayment) IsNot Nothing Then  'Claim payment Processing
                Response.Redirect("~/secure/payment/claimpaymentprocessing.aspx", False)
            ElseIf Session("ModeValue") <> "PayNow" AndAlso (Session("ModeType") = "Payment" Or Session("ModeType") = "Receipt") Then
                Dim sAgentStartPage As String = CType(GetSection("NexusFrameWork").Portals.Portal(CMS.Library.Portal.GetPortalID()), Nexus.Library.Config.Portal).AgentStartPage
                Response.Redirect(sAgentStartPage, False)
            ElseIf Session(CNQuoteCollectionFiles) IsNot Nothing Then
                Response.Redirect("~/secure/QuoteCollection.aspx", True)
            ElseIf Session("ModeValue") = "IP" Then
                'Insurer Payments
                Response.Redirect("~/secure/InsurerPayments.aspx?Mode=IP", False)
            ElseIf Session("ModeValue") = "INS" Then 'Instalments - WPR005
                Response.Redirect("~/PremiumFinance/PremiumFinancePlan.aspx?Type=EditPlan", True)
            ElseIf Session("ModeValue") = "ReverseReceipt" Then
                Response.Redirect("~/secure/SearchTransactions.aspx", False)
            Else 'New Business
                Response.Redirect("~/secure/PremiumDisplay.aspx", True)
            End If
        End Sub

    End Class
End Namespace

