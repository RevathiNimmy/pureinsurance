Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports NexusProvider
Imports System.Data
Imports Nexus.Utils
Imports System.Configuration.ConfigurationManager
Imports CMS.Library.Portal
Imports Nexus.Library
Imports System.Linq
Imports System.Linq.Enumerable

Namespace Nexus

    Partial Class Claims_FinancialDetails
        Inherits CMS.Library.Frontend.clsCMSPage
        Private sReturnURL As String = String.Empty
        ''' <summary>
        ''' Page_Load Event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim oWebService As ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
            Dim oPerilSummary As New PerilSummary
            Dim oRequestPerilSummary As New PerilSummary
            Dim oClaimDetails As New ClaimDetails
            Dim iClaimKey As Integer = 0

            Dim oOptionSettingsRecoveryExcludeTaxes As NexusProvider.OptionTypeSetting
            Dim bReceiptExcludeTax As Boolean = False

            oOptionSettingsRecoveryExcludeTaxes = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5067)

            If oOptionSettingsRecoveryExcludeTaxes IsNot Nothing AndAlso (oOptionSettingsRecoveryExcludeTaxes.OptionValue IsNot Nothing AndAlso oOptionSettingsRecoveryExcludeTaxes.OptionValue <> "0") Then
                bReceiptExcludeTax = True
            End If

            Dim sOption As String
            Dim sIsGrossClaimPaymentAmount As String
            sOption = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsGrossClaimPaymentAmount, NexusProvider.RiskTypeOptions.None, Session(CNProductCode), Nothing)
            If String.IsNullOrEmpty(sOption) Then
                sIsGrossClaimPaymentAmount = "0"
            Else
                sIsGrossClaimPaymentAmount = sOption
            End If

            'Storing of the Return URL value to return the parent page
            If Request.QueryString("ReturnUrl") IsNot Nothing Then
                sReturnURL = Request.QueryString("ReturnUrl")
            End If
            'Retreiving the claim details 
            If Not IsPostBack Then

                'Find the Base Currency
                Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
                Dim oCurrencyColl As NexusProvider.CurrencyCollection
                oCurrencyColl = oWebService.GetCurrenciesByBranch(oQuote.BranchCode)
                If oCurrencyColl IsNot Nothing AndAlso oCurrencyColl.Count > 0 Then
                    ViewState("BaseCurerency") = oCurrencyColl(0).BaseCurrencyCode
                End If

                iClaimKey = CType(Session(CNClaim), ClaimOpen).ClaimKey

                oRequestPerilSummary.ClaimKey = iClaimKey
                oRequestPerilSummary.IncludeReserveTypes = True
                oRequestPerilSummary.IncludeSalvageRecovery = True
                oRequestPerilSummary.IncludeTotals = True
                oRequestPerilSummary.IncludeTPRecovery = True

                oPerilSummary = oWebService.GetClaimPerilSummary(oRequestPerilSummary, oQuote.BranchCode)
                GetClaimDetails(oClaimOpen.ClaimKey, Nothing, 1)
                oClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)

                'For Total Tab
                Session("Total") = oPerilSummary.PerilTotals
                Me.gvTotal.DataSource = oPerilSummary.PerilTotals
                Me.gvTotal.DataBind()
                'For TP Recovery Tab
                Session("TPRecovery") = oPerilSummary.TPRecovery
                Me.gvTPRecovery.DataSource = oPerilSummary.TPRecovery
                Me.gvTPRecovery.DataBind()
                'For Salvage Tab
                Session("SalvageRecovery") = oPerilSummary.SalvageRecovery
                Me.gvSalvage.DataSource = oPerilSummary.SalvageRecovery
                Me.gvSalvage.DataBind()

                'Payments Tab
                Dim oClaimPayment As New NexusProvider.ClaimPaymentCollection
                For Each oClaimPeril As NexusProvider.PerilSummary In oClaimOpen.ClaimPeril
                    For Each oPayment As NexusProvider.ClaimPayment In oClaimPeril.ClaimPayment
                        For Each oPaymentItem As NexusProvider.ClaimPaymentItem In oPayment.PaymentItems
                            Dim oBasePayment As New NexusProvider.ClaimPayment
                            If String.IsNullOrEmpty(oPayment.PartyPaidName) = False Then
                                oBasePayment.PartyPaidName = oPayment.PartyPaidName
                            Else
                                oBasePayment.PartyPaidName = GetLocalResourceObject("lbl_ClaimPayable")
                            End If
                            oBasePayment.PaymentDate = oPayment.PaymentDate
                            oBasePayment.Payee = oPayment.Payee
                            oBasePayment.MediaType = oPayment.Payee.MediaTypeDesc
                            oBasePayment.MediaRefrenece = oPayment.Payee.MediaReference
                            If sIsGrossClaimPaymentAmount <> "0" Then
                                oBasePayment.PaymentAmount = oPaymentItem.PaymentAmount
                            Else
                                oBasePayment.PaymentAmount = oPaymentItem.PaymentAmount + oPaymentItem.TaxAmount
                            End If
                            oBasePayment.TaxAmount = oPaymentItem.TaxAmount
                            oBasePayment.CurrencyDescription = oPayment.CurrencyDescription
                            oBasePayment.CurrencyCode = oPayment.CurrencyCode
                            oBasePayment.LossCurrencyCode = oPayment.LossCurrencyCode
                            'Calculate the Loss Amount and Base Amount

                            Dim dPaymentAmount As Decimal = 0D
                            dPaymentAmount = oPaymentItem.PaymentAmount + oPaymentItem.TaxAmount

                            Dim oCurrency As New NexusProvider.Currency
                            Dim dBaseAmount As Decimal
                            Dim dLossAmount As Decimal
                            oCurrency.AccountCode = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen).ClientShortName
                            oCurrency.TransactionCurrencyCode = oPayment.CurrencyCode
                            oCurrency.Mode = "ALL"
                            oCurrency = oWebService.GetCurrencyExchangeRates(oCurrency)
                            dBaseAmount = Math.Round((dPaymentAmount * oCurrency.BaseCurrencyRate), 2)
                            oCurrency.TransactionCurrencyCode = oPayment.LossCurrencyCode
                            oCurrency = oWebService.GetCurrencyExchangeRates(oCurrency)
                            dLossAmount = Math.Round((dPaymentAmount * oCurrency.BaseCurrencyRate), 2)
                            oBasePayment.LossAmount = dLossAmount
                            oBasePayment.BaseAmount = dBaseAmount
                            If (oBasePayment.LossAmount > 0 OrElse oBasePayment.BaseAmount OrElse oBasePayment.PaymentAmount > 0) Then
                                oClaimPayment.Add(oBasePayment)
                            End If

                        Next
                    Next
                Next
                If oClaimPayment IsNot Nothing AndAlso oClaimPayment.Count > 0 Then
                    ViewState("LossCurerency") = oClaimPayment(0).LossCurrencyCode.Trim
                End If
                Session(CNPayment) = oClaimPayment
                Me.gvPayment.DataSource = oClaimPayment
                Me.gvPayment.DataBind()

                'Receipt Tab
                Dim iClaimReceiptItem As Integer = 0
                Dim oClaimReceipt As New NexusProvider.ClaimReceiptItemTypeCollection
                For Each oClaimPeril As NexusProvider.PerilSummary In oClaimOpen.ClaimPeril
                    Dim nItemCount As Integer = 0
                    For Each oReceipt As NexusProvider.ClaimReceipt In oClaimPeril.ClaimReceipt

                        Dim oBaseReceipt As New NexusProvider.ClaimReceiptItemType
                        If String.IsNullOrEmpty(oReceipt.PartyReceiptCode) = False Then
                            Dim oAccountSearchCriteria As New NexusProvider.AccountSearchCriteria
                            oAccountSearchCriteria.ShortCode = oReceipt.PartyReceiptCode
                            Dim oAccountSearchResultCollection As NexusProvider.AccountSearchResultCollection = oWebService.FindAccounts(oAccountSearchCriteria)
                            If oAccountSearchResultCollection(0).ContactName.Trim.Length = 0 Then
                                oBaseReceipt.PartyReceiptName = oAccountSearchResultCollection(0).AccountName
                            Else
                                oBaseReceipt.PartyReceiptName = oAccountSearchResultCollection(0).ContactName
                            End If
                        Else
                            oBaseReceipt.PartyReceiptName = GetLocalResourceObject("lbl_ClaimReceivable")
                        End If
                        oBaseReceipt.ReceiptDate = oReceipt.ReceiptDate
                        oBaseReceipt.Payee = oReceipt.Payee
                        If oReceipt.ClaimReceiptItem IsNot Nothing AndAlso oReceipt.ClaimReceiptItem.Count > 0 Then
                            For Each oClaimReceiptItem As ClaimReceiptItemType In oReceipt.ClaimReceiptItem
                                oBaseReceipt.TaxAmount = oBaseReceipt.TaxAmount + oClaimReceiptItem.TaxAmount
                                oBaseReceipt.LossAmount = oBaseReceipt.LossAmount + oClaimReceiptItem.LossAmount
                                oBaseReceipt.BaseAmount = oBaseReceipt.BaseAmount + oClaimReceiptItem.BaseAmount
                            Next
                            oBaseReceipt.TaxAmount = oReceipt.ClaimReceiptItem(oClaimReceipt.Count).TaxAmount
                        Else
                            oBaseReceipt.TaxAmount = oReceipt.TaxAmount
                        End If


                        oBaseReceipt.ReceiptAmount = oReceipt.ReceiptAmount


                        oBaseReceipt.CurrencyDescription = GetDescriptionForCode(ListType.PMLookup, oReceipt.CurrencyCode, "Currency")
                        If oReceipt.ClaimReceiptItem IsNot Nothing AndAlso oReceipt.ClaimReceiptItem.Count > 0 Then
                            oBaseReceipt.LossAmount = oReceipt.ClaimReceiptItem(nItemCount).LossAmount
                            oBaseReceipt.BaseAmount = oReceipt.ClaimReceiptItem(nItemCount).BaseAmount
                        End If
                        oClaimReceipt.Add(oBaseReceipt)
                        nItemCount = nItemCount + 1
                    Next
                Next
                Session("CNReceipt") = oClaimReceipt
                Me.gvReceipt.DataSource = oClaimReceipt
                Me.gvReceipt.DataBind()

                If Session(CNMode) = Mode.PayClaim Then
                    ''''''Get Current payment paid amount
                    oClaimDetails = oWebService.GetClaimDetails(iClaimKey, oQuote.BranchCode)
                    oClaimOpen.ClaimPeril = oClaimDetails.ClaimPeril
                End If

                'Dynamic Reserve Type
                Dim dtMain As New DataSet

                Dim oFormatString As String = CType(GetSection("NexusFrameWork"), Library.Config.NexusFrameWork).Portals.Portal(GetPortalID()).FormatStrings.FormatString("Currency").DataFormatString
                Dim oReserveCollection As New ReserveCollection
                Dim oReserve As Reserve
                For iCount As Integer = 0 To oClaimOpen.ClaimPeril.Count - 1
                    Dim dt(oClaimOpen.ClaimPeril(iCount).Reserve.Count) As DataTable
                    For pCount As Integer = 0 To oClaimOpen.ClaimPeril(iCount).Reserve.Count - 1
                        For jCount As Integer = 0 To oClaimOpen.ClaimPeril(iCount).Reserve.Count - 1
                            If oClaimOpen.ClaimPeril(iCount).Reserve(pCount).TypeCode.Trim.ToUpper = oClaimOpen.ClaimPeril(iCount).Reserve(jCount).TypeCode.ToUpper Then
                                oReserve = New Reserve
                                oReserve.TypeCode = oClaimOpen.ClaimPeril(iCount).Reserve(pCount).TypeDescription.Trim
                                oReserve.Description = oClaimOpen.ClaimPeril(iCount).Description
                                oReserve.InitialReserve = oClaimOpen.ClaimPeril(iCount).Reserve(jCount).InitialReserve
                                oReserve.PaidAmount = oClaimOpen.ClaimPeril(iCount).Reserve(jCount).PaidAmount
                                oReserve.RevisedReserve = oClaimOpen.ClaimPeril(iCount).Reserve(jCount).RevisedReserve
                                oReserve.CurrentReserve = oClaimOpen.ClaimPeril(iCount).Reserve(jCount).InitialReserve + oClaimOpen.ClaimPeril(iCount).Reserve(jCount).RevisedReserve - oClaimOpen.ClaimPeril(iCount).Reserve(jCount).PaidAmount
                                oReserve.SumInsured = oClaimOpen.ClaimPeril(iCount).Reserve(jCount).SumInsured
                                oReserve.Average = oClaimOpen.ClaimPeril(iCount).Reserve(jCount).Average
                                If (oReserve.CurrentReserve > 0 OrElse oReserve.InitialReserve > 0 OrElse oReserve.PaidAmount OrElse oReserve.RevisedReserve > 0 OrElse oReserve.SumInsured > 0) Then
                                    oReserveCollection.Add(oReserve)
                                End If
                            End If
                        Next
                    Next
                Next
                Session("CNReserve") = oReserveCollection

                gvReserveTypes.DataSource = oReserveCollection
                gvReserveTypes.DataBind()
            End If

        End Sub
        ''' <summary>
        ''' Dynamically adding the column name
        ''' </summary>
        ''' <param name="oDt"></param>
        ''' <remarks></remarks>
        Function AddCoumn(ByRef oDt As DataTable) As String
            Dim ReturnString As String
            ReturnString = "<tr class='" & GetLocalResourceObject("CssTRReserve") & "'>"
            ReturnString += "<th class='" & GetLocalResourceObject("CssTHReserveTypeCode") & "'>" & GetLocalResourceObject("Main_ReserveType") & "</th>"
            ReturnString += "<th class='" & GetLocalResourceObject("CssTHReserveDescription") & "'>" & GetLocalResourceObject("Main_Description") & "</th>"
            ReturnString += "<th class='" & GetLocalResourceObject("CssTHReserveInitialReserve") & "'>" & GetLocalResourceObject("Main_InitialReserve") & "</th>"
            ReturnString += "<th class='" & GetLocalResourceObject("CssTHReservePaidToDate") & "'>" & GetLocalResourceObject("Main_PaidToDate") & "</th>"
            ReturnString += "<th class='" & GetLocalResourceObject("CssTHReserveRevisedReserve") & "'>" & GetLocalResourceObject("Main_RevisedReserve") & "</th>"
            ReturnString += "<th class='" & GetLocalResourceObject("CssTHReserveCurrentReserve") & "'>" & GetLocalResourceObject("Main_CurrentReserve") & "</th>"
            ReturnString += "<th class='" & GetLocalResourceObject("CssTHReserveSumInsured") & "'>" & GetLocalResourceObject("Main_SumInsured") & "</th>"
            ReturnString += "<th class='" & GetLocalResourceObject("CssTHReserveAverage") & "'>" & GetLocalResourceObject("Main_Average") & "</th>"
            ReturnString += "</tr>"
            Return ReturnString
        End Function
        Protected Sub gvPayment_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPayment.Load
            If gvPayment.PageCount = 1 Then
                gvPayment.AllowPaging = False
            End If
        End Sub
        ''' <summary>
        ''' Payment Grid DataBound Fucntion
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvPayment_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPayment.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                'To Display the amount with Currency 
                Dim oItem As NexusProvider.ClaimPayment = CType(e.Row.DataItem, NexusProvider.ClaimPayment)
                Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                Dim sLossCurrency As String = Session(CNCurrenyCode)
                Dim sPaymentCurrency As String = GetCurrencyForDescription(oItem.CurrencyDescription.Trim)
                If sPaymentCurrency IsNot Nothing Then
                    e.Row.Cells(5).Text = New Money(oItem.PaymentAmount, sPaymentCurrency).Formatted 'Payment Amount
                    e.Row.Cells(6).Text = New Money(oItem.TaxAmount, sPaymentCurrency).Formatted 'Tax Amount
                Else
                    e.Row.Cells(5).Text = New Money(oItem.PaymentAmount, ViewState("BaseCurerency")).Formatted 'Payment Amount
                    e.Row.Cells(6).Text = New Money(oItem.TaxAmount, ViewState("BaseCurerency")).Formatted 'Tax Amount
                End If
                e.Row.Cells(8).Text = New Money(oItem.LossAmount, sLossCurrency).Formatted 'Loss AMount
                e.Row.Cells(9).Text = New Money(oItem.BaseAmount, ViewState("BaseCurerency")).Formatted 'Base AMount
                e.Row.Cells(2).Text = If(oItem.Payee IsNot Nothing, oItem.Payee.Name, String.Empty)
            End If
        End Sub

        Protected Sub gvReceipt_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvReceipt.Load
            If gvReceipt.PageCount = 1 Then
                gvReceipt.AllowPaging = False
            End If
        End Sub
        ''' <summary>
        ''' Receipt Grid DataBound Function
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvReceipt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvReceipt.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then

                'To Display the amount with Currency 
                Dim oItem As NexusProvider.ClaimReceiptItemType = CType(e.Row.DataItem, NexusProvider.ClaimReceiptItemType)
                Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                Dim sLossCurrency As String = Session(CNCurrenyCode)
                Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
                Dim sReceiptCurrency As String = GetCurrencyForDescription(oItem.CurrencyDescription.Trim, oQuote.BranchCode)
                e.Row.Cells(2).Text = If(oItem.Payee IsNot Nothing, oItem.Payee.Name, String.Empty)
                e.Row.Cells(3).Text = New Money(oItem.ReceiptAmount, sReceiptCurrency).Formatted 'Receipt Amount
                e.Row.Cells(4).Text = New Money(oItem.TaxAmount, sReceiptCurrency).Formatted 'Tax Amount
                e.Row.Cells(6).Text = New Money(oItem.LossAmount, sLossCurrency).Formatted 'Loss AMount
                e.Row.Cells(7).Text = New Money(oItem.BaseAmount, ViewState("BaseCurerency")).Formatted 'Base AMount
                e.Row.Cells(2).Text = oItem.Payee.Name
            End If
        End Sub

        Protected Sub gvSalvage_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSalvage.Load
            If gvSalvage.PageCount = 1 Then
                gvSalvage.AllowPaging = False
            End If
        End Sub

        Protected Sub gvTotal_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvTotal.Load
            If gvTotal.PageCount = 1 Then
                gvTotal.AllowPaging = False
            End If

        End Sub

        Protected Sub gvTPRecovery_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvTPRecovery.Load
            If gvTPRecovery.PageCount = 1 Then
                gvTPRecovery.AllowPaging = False
            End If
        End Sub

        Protected Sub Page_PreInit1(sender As Object, e As EventArgs) Handles Me.PreInit
            If Request.QueryString("modal") = "true" Then
                CMS.Library.Frontend.Functions.SetTheme(Page, ConfigurationManager.AppSettings("ModalPageTemplate"))
                btnOk.Attributes.Add("onclick", "self.parent.tb_remove();")
            End If
        End Sub

        Protected Sub gvReserveSorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvReserveTypes.Sorting
            Dim oReserveCollection As NexusProvider.ReserveCollection = CType(Session("CNReserve"), NexusProvider.ReserveCollection)
            oReserveCollection.SortColumn = e.SortExpression
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
            oReserveCollection.SortingOrder = _sortDirection
            oReserveCollection.SortObjectType = GetType(NexusProvider.Reserve)
            oReserveCollection.Sort()
            CType(sender, GridView).DataSource = oReserveCollection
            CType(sender, GridView).DataBind()
        End Sub
        Protected Sub gvPaymentSorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvPayment.Sorting
            Dim oPaymentCollection As NexusProvider.ClaimPaymentCollection = CType(Session(CNPayment), NexusProvider.ClaimPaymentCollection)
            oPaymentCollection.SortColumn = e.SortExpression
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
            oPaymentCollection.SortingOrder = _sortDirection
            oPaymentCollection.SortObjectType = GetType(NexusProvider.ClaimPayment)
            oPaymentCollection.Sort()
            CType(sender, GridView).DataSource = oPaymentCollection
            CType(sender, GridView).DataBind()
        End Sub
        Protected Sub gvReceiptSorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvReceipt.Sorting
            Dim oReceiptCollection As NexusProvider.ClaimReceiptItemTypeCollection = CType(Session("CNReceipt"), NexusProvider.ClaimReceiptItemTypeCollection)
            oReceiptCollection.SortColumn = e.SortExpression
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
            oReceiptCollection.SortingOrder = _sortDirection
            oReceiptCollection.SortObjectType = GetType(NexusProvider.ClaimReceiptItemType)
            oReceiptCollection.Sort()
            CType(sender, GridView).DataSource = oReceiptCollection
            CType(sender, GridView).DataBind()
        End Sub

        Protected Sub gvTPRecoverySorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvTPRecovery.Sorting
            Dim oTPRecoveryCollection As NexusProvider.PerilRecoveryCollection = CType(Session("TPRecovery"), NexusProvider.PerilRecoveryCollection)
            oTPRecoveryCollection.SortColumn = e.SortExpression
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
            oTPRecoveryCollection.SortingOrder = _sortDirection
            oTPRecoveryCollection.SortObjectType = GetType(NexusProvider.PerilRecovery)
            oTPRecoveryCollection.Sort()
            CType(sender, GridView).DataSource = oTPRecoveryCollection
            CType(sender, GridView).DataBind()
        End Sub
        Protected Sub gvSalvageSorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvSalvage.Sorting
            Dim oSalvageCollection As NexusProvider.PerilRecoveryCollection = CType(Session("SalvageRecovery"), NexusProvider.PerilRecoveryCollection)
            oSalvageCollection.SortColumn = e.SortExpression
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
            oSalvageCollection.SortingOrder = _sortDirection
            oSalvageCollection.SortObjectType = GetType(NexusProvider.PerilRecovery)
            oSalvageCollection.Sort()
            CType(sender, GridView).DataSource = oSalvageCollection
            CType(sender, GridView).DataBind()
        End Sub

        Protected Sub gvTotalSorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvTotal.Sorting
            Dim oTotalCollection As NexusProvider.PerilCollection = CType(Session("Total"), NexusProvider.PerilCollection)
            oTotalCollection.SortColumn = e.SortExpression
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
            oTotalCollection.SortingOrder = _sortDirection
            oTotalCollection.SortObjectType = GetType(NexusProvider.PerilSummary)
            oTotalCollection.Sort()
            CType(sender, GridView).DataSource = oTotalCollection
            CType(sender, GridView).DataBind()
        End Sub
    End Class

End Namespace
