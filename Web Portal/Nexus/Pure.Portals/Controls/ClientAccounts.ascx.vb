Imports Nexus.Library
Imports CMS.Library
Imports System.Data
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Utils
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports NexusProvider
Imports System.Linq
Imports System.Resources
Imports System.Collections.Generic

Namespace Nexus

    Partial Class Controls_ClientAccounts : Inherits System.Web.UI.UserControl

        Dim oAccountdDetails As NexusProvider.AccountDetails
        Dim oAccountDetailsCollection As NexusProvider.AccountDetailsDefaults
        Private iPartyKey As Integer
        Private pdsAccountDetails As PagedDataSource

        'Private asExcludeDocTypeList() As String = {"ACCRUAL", "BAD DEBT", "BINDING JOURNAL", "CLAIM ADJUST", "CLAIM ADJUSTMENT",
        ' "CLAIM OPEN", "CLAIM PAYMENT", "CLAIM RECEIPT", "CREDIT NOTE",
        ' "CURRENCY EXCHANGE DIFFERENCE", "DEFERRED REINSURANCE", "DEPRECIATION JOURNAL",
        ' "PRE-PAYMENT", "PURCHASE CREDIT NOTE", "PURCHASE INVOICE", "SALES INVOICE"}

        Private asExcludeDocTypeList() As String = {"ACC", "SBD", "BJN", "CLA", "CLO", "CLP", "CLR", "SCN",
                                                    "SCD", "SDD", "DPJ", "PPT", "PCN", "PIN", "SIN"}

#Region "Private Constants"

        'Grid column index constants
        Const kDocRefCol As Integer = 1
        Const kDocTypeCol As Integer = 2
        Const kEffectiveDateCol As Integer = 3
        Const kTransDateCol As Integer = 4
        Const kMediaTypeCol As Integer = 5
        Const kAccountAmountCol As Integer = 6
        Const kPaidDateCol As Integer = 7
        Const kPolicyNumberCol As Integer = 8
        Const kAccountOSAmountCol As Integer = 9
        Const kCurrencyAmountCol As Integer = 10
        Const kOSCurrencyAmountCol As Integer = 11

        'Child Grid column index constants
        Const kEffectiveDateColChild As Integer = 0
        Const kTransDateColChild As Integer = 1
        Const kMediaTypeColChild As Integer = 2
        Const kMediaRefColChild As Integer = 3
        Const kAccountAmountColChild As Integer = 4
        Const kPaidDateColChild As Integer = 5
        Const kPolicyNumberColChild As Integer = 6
        Const kAccountOSAmountColChild As Integer = 7
        Const kCurrencyAmountColChild As Integer = 8
        Const kOSCurrencyAmountColChild As Integer = 9

        Const kDebtRollUpOptionNumber As Integer = 1031

#End Region

        Public Property PartyKey() As Integer
            Set(ByVal value As Integer)

                iPartyKey = value
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oAccountDetailsCollection As NexusProvider.AccountDetailsDefaults
                Dim oAccountDetailsCollectionWithoutRollUp As NexusProvider.AccountDetailsDefaults
                Dim oAccountdDetails As New NexusProvider.AccountDetails
                Dim oOptionSetting As NexusProvider.OptionTypeSetting

                Try
                    oWebService = New NexusProvider.ProviderManager().Provider
                    oAccountdDetails = New NexusProvider.AccountDetails
                    'setting the System Option configuration
                    oOptionSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 1031)
                    If oOptionSetting.OptionValue = "1" Then
                        oAccountdDetails.Rollup = True
                    End If
                    'setting the default values passed in Request Parameter
                    oAccountdDetails.PartyCnt = value
                    oAccountdDetails.OutstandingOnly = chkOutstandingTransaction.Checked
                    oAccountdDetails.OutstandingOnlySpecified = True
                    oAccountdDetails.Display500 = True

                    'Enable Debt Rollup if back office option is turned on
                    oOptionSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, kDebtRollUpOptionNumber)
                    If oOptionSetting.OptionValue = "1" Then
                        oAccountdDetails.Rollup = True
                    End If

                    oAccountDetailsCollection = oWebService.GetAccountDetails(oAccountdDetails)

                    ViewState("PartyKey") = oAccountdDetails.PartyCnt
                    If ViewState("AccountCollCacheID") Is Nothing Then
                        Dim AccountCollCacheID As Guid
                        AccountCollCacheID = Guid.NewGuid()
                        ViewState.Add("AccountCollCacheID", AccountCollCacheID.ToString)
                    End If

                    Cache.Insert(ViewState("AccountCollCacheID").ToString(), oAccountDetailsCollection, Nothing, DateTime.MaxValue,
                                 TimeSpan.FromMinutes(5))

                    If oAccountDetailsCollection.AccountDetails.Count > 0 Then
                        ViewState("AccountCurrencyCode") =
                            oAccountDetailsCollection.AccountDetails(0).AccountCurrencyCode.Trim
                    Else
                        Dim oCurrencyColl As NexusProvider.CurrencyCollection
                        oCurrencyColl = oWebService.GetCurrenciesByBranch(Session(CNBranchCode).ToString())
                        ViewState("BaseCurrency") = oCurrencyColl(0).BaseCurrencyCode

                    End If

                    If oOptionSetting.OptionValue = "1" Then
                        oAccountdDetails.Rollup = False
                        oAccountDetailsCollectionWithoutRollUp = oWebService.GetAccountDetails(oAccountdDetails)
                        If ViewState("AccountCollWithoutRollUpCacheID") Is Nothing Then
                            Dim AccountCollCacheID As Guid
                            AccountCollCacheID = Guid.NewGuid()
                            ViewState.Add("AccountCollWithoutRollUpCacheID", AccountCollCacheID.ToString)
                        End If

                        Cache.Insert(ViewState("AccountCollWithoutRollUpCacheID"), oAccountDetailsCollectionWithoutRollUp, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

                        'To prevent a GetAccountDetails() call with DebtRollUp True 
                        'as we already have all the infomration from earlier GetAccountDetails() call with DebtRollUp False
                        Dim oDocRefDictionary As Dictionary(Of String, Integer) = New Dictionary(Of String, Integer)
                        Dim oAccountDetails As AccountDetails
                        Dim nParentRecord As Integer = -1

                        With oAccountDetailsCollectionWithoutRollUp
                            oAccountDetailsCollection = New NexusProvider.AccountDetailsDefaults
                            oAccountDetailsCollection.AccountBalance = .AccountBalance
                            oAccountDetailsCollection.AccountName = .AccountName
                            oAccountDetailsCollection.ContactName = .ContactName
                            oAccountDetailsCollection.PhoneExtension = .PhoneExtension
                            oAccountDetailsCollection.PhoneAreaCode = .PhoneAreaCode
                            oAccountDetailsCollection.PhoneNumber = .PhoneNumber
                            oAccountDetailsCollection.AccountStatus = .AccountStatus
                            oAccountDetailsCollection.TransactionCurrencyOutStandingBalance = .TransactionCurrencyOutStandingBalance

                            For Each oAccountDetailsRow As NexusProvider.AccountDetails In oAccountDetailsCollectionWithoutRollUp.AccountDetails

                                If Not oDocRefDictionary.ContainsKey(oAccountDetailsRow.DocRef) Then 'If a DocRef does not exists in the Dictionary

                                    oAccountDetails = New AccountDetails() 'Create new object
                                    oAccountDetails.TransDetailKeys = oAccountDetailsRow.TransDetailKeys
                                    oAccountDetails.BranchKey = oAccountDetailsRow.BranchKey
                                    oAccountDetails.Account = oAccountDetailsRow.Account
                                    oAccountDetails.DocRef = oAccountDetailsRow.DocRef
                                    oAccountDetails.AltRef = oAccountDetailsRow.AltRef
                                    oAccountDetails.EffectiveDate = oAccountDetailsRow.EffectiveDate
                                    oAccountDetails.TransDate = oAccountDetailsRow.TransDate
                                    oAccountDetails.MediaType = oAccountDetailsRow.MediaType
                                    oAccountDetails.Amount = oAccountDetailsRow.Amount
                                    oAccountDetails.PrimarySettled = oAccountDetailsRow.PrimarySettled
                                    oAccountDetails.OutstandingAmount = oAccountDetailsRow.OutstandingAmount
                                    oAccountDetails.PaidDate = oAccountDetailsRow.PaidDate
                                    oAccountDetails.DocTypeId = oAccountDetailsRow.DocTypeId
                                    oAccountDetails.DocumentTypeCode = oAccountDetailsRow.DocumentTypeCode
                                    oAccountDetails.Reference = oAccountDetailsRow.Reference
                                    oAccountDetails.OperatorName = oAccountDetailsRow.OperatorName
                                    oAccountDetails.Period = oAccountDetailsRow.Period
                                    oAccountDetails.DocumentGroupId = oAccountDetailsRow.DocumentGroupId
                                    oAccountDetails.DocTypeGroupCode = oAccountDetailsRow.DocTypeGroupCode
                                    oAccountDetails.Client = oAccountDetailsRow.Client
                                    oAccountDetails.ClientCode = oAccountDetailsRow.ClientCode
                                    oAccountDetails.MediaRef = oAccountDetailsRow.MediaRef
                                    oAccountDetails.AccountKey = oAccountDetailsRow.AccountKey
                                    oAccountDetails.PayeeName = oAccountDetailsRow.PayeeName
                                    oAccountDetails.UnderwritingYear = oAccountDetailsRow.UnderwritingYear
                                    oAccountDetails.AccountOutStandingAmount = oAccountDetailsRow.AccountOutStandingAmount
                                    oAccountDetails.CurrencyAmount = oAccountDetailsRow.CurrencyAmount
                                    oAccountDetails.OutStandingCurrencyAmount = oAccountDetailsRow.OutStandingCurrencyAmount
                                    oAccountDetails.BGRef = oAccountDetailsRow.BGRef
                                    oAccountDetails.MediaRef = oAccountDetailsRow.MediaRef
                                    oAccountDetails.CurrencyCode = oAccountDetailsRow.CurrencyCode
                                    oAccountDetails.AccountCurrencyCode = oAccountDetailsRow.AccountCurrencyCode
                                    oAccountDetails.AccountAmount = oAccountDetailsRow.AccountAmount
                                    oAccountDetails.BalanceType = oAccountDetailsRow.BalanceType
                                    oAccountDetails.BaseCurrencyCode = oAccountDetailsRow.BaseCurrencyCode
                                    oAccountDetails.CashListKey = oAccountDetailsRow.CashListKey
                                    oAccountDetails.IsSplitReceipt = oAccountDetailsRow.IsSplitReceipt
                                    oAccountDetails.IsLeadAgent = oAccountDetailsRow.IsLeadAgent
                                    oAccountDetails.DueDate = oAccountDetailsRow.DueDate
                                    oAccountDetails.FinancePlanKey = oAccountDetailsRow.FinancePlanKey
                                    oAccountDetails.FinancePlanStatus = oAccountDetailsRow.FinancePlanStatus
                                    oAccountDetailsCollection.AccountDetails.Add(oAccountDetails) 'Add it collection
                                    oDocRefDictionary.Add(oAccountDetailsRow.DocRef, oAccountDetailsCollection.AccountDetails.Count - 1) 'Add it to Dictionary
                                Else
                                    'If it exists in Dictionary then update the AccountAmount etc .of the Parent record
                                    nParentRecord = oDocRefDictionary(oAccountDetailsRow.DocRef) 'Get the index at which parent exists.
                                    oAccountDetailsCollection.AccountDetails.Item(nParentRecord).AccountAmount += oAccountDetailsRow.AccountAmount
                                    oAccountDetailsCollection.AccountDetails.Item(nParentRecord).AccountOutStandingAmount += oAccountDetailsRow.AccountOutStandingAmount
                                    oAccountDetailsCollection.AccountDetails.Item(nParentRecord).CurrencyAmount += oAccountDetailsRow.CurrencyAmount
                                    oAccountDetailsCollection.AccountDetails.Item(nParentRecord).OutStandingCurrencyAmount += oAccountDetailsRow.OutStandingCurrencyAmount
                                    nParentRecord = -1
                                End If
                            Next
                        End With
                    Else
                        'When SystemOption DebtRollUp = False
                        oAccountDetailsCollection = oWebService.GetAccountDetails(oAccountdDetails)
                    End If


                    If ViewState("AccountCollCacheID") Is Nothing Then
                        Dim AccountCollCacheID As Guid
                        AccountCollCacheID = Guid.NewGuid()
                        ViewState.Add("AccountCollCacheID", AccountCollCacheID.ToString)
                    End If

                    Cache.Insert(ViewState("AccountCollCacheID"), oAccountDetailsCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

                    If oAccountDetailsCollection.AccountDetails.Count > 0 Then
                        ViewState("AccountCurrencyCode") = oAccountDetailsCollection.AccountDetails(0).AccountCurrencyCode.Trim
                    End If


                    BindParentDataGrid(gvClientAccountParent, oAccountDetailsCollection, True)

                Finally
                    oWebService = Nothing
                    oAccountDetailsCollection = Nothing
                    oAccountDetailsCollectionWithoutRollUp = Nothing
                    oAccountdDetails = Nothing
                End Try

            End Set
            Get
                Return iPartyKey
            End Get
        End Property

#Region "Private Methods"
        ''' <summary>
        ''' Bind Accounts grid with filtered dataset.
        ''' </summary>
        ''' <param name="gvToBind"></param>
        ''' <param name="oAccountDetailsCollection"></param>
        ''' <param name="bSetOutstandingAmount"></param>
        ''' <param name="nPageIndex"></param>
        ''' <remarks></remarks>
        Private Sub BindParentDataGrid(ByRef gvToBind As GridView,
                                       ByVal oAccountDetailsCollection As NexusProvider.AccountDetailsDefaults,
                                       ByVal bSetOutstandingAmount As Boolean,
                                       Optional ByVal nPageIndex As Integer = -1)

            Dim crAmountOutstanding As Double

            Try
                If chkExcludeInstalments.Checked Then
                    Dim oAccoundDetailForCurrentRow = From accountdetail In oAccountDetailsCollection.AccountDetails
                                                      Where (accountdetail.DocumentTypeCode <> "Instalment Cash") _
                                                            AndAlso (accountdetail.DocumentTypeCode <> "Instalment Credit") _
                                                            AndAlso (accountdetail.DocumentTypeCode <> "Instalment Debit") _
                                                            AndAlso (accountdetail.DocumentTypeCode <> "Instalment Endorsement Credit") _
                                                            AndAlso (accountdetail.DocumentTypeCode <> "Instalment Endorsement Debit") _
                                                            AndAlso (accountdetail.DocumentTypeCode <> "Instalment NB Credit") _
                                                            AndAlso (accountdetail.DocumentTypeCode <> "Instalment NB Debit") _
                                                            AndAlso (accountdetail.DocumentTypeCode <> "Instalment Reinstatement Credit") _
                                                            AndAlso (accountdetail.DocumentTypeCode <> "Instalment Reinstatement Debit") _
                                                            AndAlso (accountdetail.DocumentTypeCode <> "Instalment Renewal Credit") _
                                                            AndAlso (accountdetail.DocumentTypeCode <> "Instalment Renewal Debit")
                                                      Order By accountdetail.DocumentRef Select accountdetail

                    If bSetOutstandingAmount Then
                        crAmountOutstanding =
                            oAccoundDetailForCurrentRow.Sum(Function(item) Convert.ToDecimal(item.AccountOutStandingAmount))
                        If ViewState("AccountCurrencyCode") IsNot Nothing Then
                            txtAmountOutstanding.Text = New Money(crAmountOutstanding,
                                                                  ViewState("AccountCurrencyCode").ToString()).Formatted
                        Else
                            txtAmountOutstanding.Text = New Money(crAmountOutstanding,
                                                                  ViewState("BaseCurrency").ToString()).Formatted
                        End If
                    End If

                    If oAccoundDetailForCurrentRow.Count > 0 Then
                        gvToBind.DataSource = oAccoundDetailForCurrentRow.ToList()
                        If nPageIndex > -1 Then
                            gvToBind.PageIndex = nPageIndex
                        End If
                        gvToBind.DataBind()
                    Else
                        gvToBind.DataSource = Nothing
                        gvToBind.DataBind()
                    End If

                Else
                    Dim oAccoundDetailForCurrentRow = From accountdetail In oAccountDetailsCollection.AccountDetails
                                                      Where (accountdetail.DocumentTypeCode <> "Claim Receipt") _
                                                      AndAlso (accountdetail.DocumentTypeCode <> "Claim Payment") _
                                                      AndAlso (accountdetail.DocumentTypeCode <> "Claim Adjust") _
                                                      AndAlso (accountdetail.DocumentTypeCode <> "Claim Open")
                                                      Order By accountdetail.DocumentRef Select accountdetail

                    If bSetOutstandingAmount Then
                        crAmountOutstanding =
                            oAccoundDetailForCurrentRow.Sum(Function(item) Convert.ToDecimal(item.AccountOutStandingAmount))
                        If ViewState("AccountCurrencyCode") IsNot Nothing Then
                            txtAmountOutstanding.Text = New Money(crAmountOutstanding,
                                                                  ViewState("AccountCurrencyCode").ToString()).Formatted
                        Else
                            txtAmountOutstanding.Text = New Money(crAmountOutstanding,
                                                                  ViewState("BaseCurrency").ToString()).Formatted
                        End If
                    End If

                    If oAccoundDetailForCurrentRow.Count > 0 Then
                        gvToBind.DataSource = oAccoundDetailForCurrentRow.ToList()
                        If nPageIndex > -1 Then
                            gvToBind.PageIndex = nPageIndex
                        End If
                        gvToBind.DataBind()
                    Else
                        gvToBind.DataSource = Nothing
                        gvToBind.DataBind()
                    End If
                End If

            Catch ex As System.Exception

            End Try
        End Sub

#End Region

#Region "Protected Events"

        ''' <summary>
        ''' Grid View Databinding for setting Headertext on currency base.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvClientAccountParent_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvClientAccountParent.DataBinding
            If Not Page.IsPostBack Then
                Dim oCurrencies As Nexus.Library.Config.Currencies
                oCurrencies = CType(GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Currencies
                If ViewState("AccountCurrencyCode") IsNot Nothing Then
                    gvClientAccountParent.Columns(kAccountAmountCol).HeaderText = Replace(gvClientAccountParent.Columns(kAccountAmountCol).HeaderText, "[!AccountCurrency!]", oCurrencies.Currency(ViewState("AccountCurrencyCode")).Symbol)
                    gvClientAccountParent.Columns(kAccountOSAmountCol).HeaderText = Replace(gvClientAccountParent.Columns(kAccountOSAmountCol).HeaderText, "[!AccountCurrency!]", oCurrencies.Currency(ViewState("AccountCurrencyCode")).Symbol)
                Else
                    Dim oWebService As ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim oCurrencyColl As NexusProvider.CurrencyCollection
                    oCurrencyColl = oWebService.GetCurrenciesByBranch(Session(CNBranchCode))
                    ViewState("BaseCurrency") = oCurrencyColl(0).BaseCurrencyCode
                    gvClientAccountParent.Columns(kAccountAmountCol).HeaderText = Replace(gvClientAccountParent.Columns(kAccountAmountCol).HeaderText, "[!AccountCurrency!]", oCurrencies.Currency(oCurrencyColl(0).BaseCurrencyCode.Trim).Symbol)
                    gvClientAccountParent.Columns(kAccountOSAmountCol).HeaderText = Replace(gvClientAccountParent.Columns(kAccountOSAmountCol).HeaderText, "[!AccountCurrency!]", oCurrencies.Currency(oCurrencyColl(0).BaseCurrencyCode.Trim).Symbol)
                End If
            End If

        End Sub
        Protected Sub gvClientAccountParent_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvClientAccountParent.PageIndexChanging
            Dim oAccountDetailsCollection As NexusProvider.AccountDetailsDefaults
            oAccountDetailsCollection = CType(Cache.Item(ViewState("AccountCollCacheID")), NexusProvider.AccountDetailsDefaults)
            BindParentDataGrid(gvClientAccountParent, oAccountDetailsCollection, False, nPageIndex:=e.NewPageIndex)
        End Sub
#End Region

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "clientScript", "collapseAccountGrid();", True)
            If Not IsPostBack AndAlso Me.Visible = True Then
                If ViewState("AccountCollCacheID") Is Nothing Then
                    Dim AccountCollCacheID As Guid
                    AccountCollCacheID = Guid.NewGuid()
                    ViewState.Add("AccountCollCacheID", AccountCollCacheID.ToString)
                End If

                Dim oAccountDetailsCollection As NexusProvider.AccountDetailsDefaults = CType(Cache.Item(ViewState("AccountCollCacheID")), NexusProvider.AccountDetailsDefaults)
                If oAccountDetailsCollection Is Nothing Or
                    (oAccountDetailsCollection IsNot Nothing AndAlso oAccountDetailsCollection.AccountDetails IsNot Nothing AndAlso oAccountDetailsCollection.AccountDetails.Count = 0) Then
                    If Session(CNParty) IsNot Nothing AndAlso
                        Me.PartyKey = 0 AndAlso Session(CNClientMode) = Mode.View AndAlso
                        CType(Session(CNParty), NexusProvider.BaseParty).Key <> 0 AndAlso Session(CNIsNewClient) Is Nothing Then
                        Dim oParty As NexusProvider.BaseParty = Session(CNParty)
                        PartyKey = oParty.Key
                        btnSearch_Click(sender, e)

                    End If
                End If

                Dim lstLookupList As LookupListItem
                For Each sDocType In asExcludeDocTypeList
                    lstLookupList = DocumentType.Items.FindItemByCode(Trim(sDocType))
                    If lstLookupList IsNot Nothing Then
                        DocumentType.Items.Remove(lstLookupList)
                    End If
                Next

            End If

        End Sub

        Protected Sub gvClientAccountParent_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvClientAccountParent.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim oAccountDetailsCollection As NexusProvider.AccountDetails
                Dim bIsExpand As Boolean = False
                oAccountDetailsCollection = CType(e.Row.DataItem, NexusProvider.AccountDetails)
                e.Row.Cells(kAccountAmountCol).Text = New Money(oAccountDetailsCollection.AccountAmount, oAccountDetailsCollection.AccountCurrencyCode).Formatted 'Account Amount
                e.Row.Cells(kAccountOSAmountCol).Text = New Money(oAccountDetailsCollection.OutstandingAmount, oAccountDetailsCollection.CurrencyCode).Formatted 'Currency Amount
                e.Row.Cells(kCurrencyAmountCol).Text = New Money(oAccountDetailsCollection.CurrencyAmount, oAccountDetailsCollection.BaseCurrencyCode).Formatted 'Currency Amount
                e.Row.Cells(kOSCurrencyAmountCol).Text = New Money(oAccountDetailsCollection.OutStandingCurrencyAmount, oAccountDetailsCollection.BaseCurrencyCode).Formatted 'OutStanding Currency Amount

                ''Show ViewPlan if DocumentTypeCode is "Instalment Debit"
                If e.Row.FindControl("lnkbtnViewPlan") IsNot Nothing Then
                    Dim sDocRef As String
                    If oAccountDetailsCollection IsNot Nothing AndAlso
                        (oAccountDetailsCollection.DocumentTypeCode = "Instalment Debit" OrElse oAccountDetailsCollection.DocumentTypeCode = "Instalment Renewal Debit") AndAlso
                        (oAccountDetailsCollection.FinancePlanKey > 0) Then
                        e.Row.FindControl("lnkbtnViewPlan").Visible = True
                        sDocRef = e.Row.Cells(kDocRefCol).Text
                        Dim lnkbtnViewPlan As LinkButton = CType(e.Row.FindControl("lnkbtnViewPlan"), LinkButton)
                        If HttpContext.Current.Session.IsCookieless Then
                            lnkbtnViewPlan.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" &
                                                       "/Modal/InstalmentPlanView.aspx?DocRef=" & sDocRef &
                                                       "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=400' , null);return false;"
                        Else
                            lnkbtnViewPlan.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") &
                                                       "/Modal/InstalmentPlanView.aspx?DocRef=" & sDocRef &
                                                       "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=400' , null);return false;"
                        End If


                    Else
                        e.Row.FindControl("lnkbtnViewPlan").Visible = False
                    End If
                End If
                'Bind Child Grid if Debt RollUp option is ON
                If ViewState("AccountCollWithoutRollUpCacheID") IsNot Nothing Then
                    Dim oAccountDetailsCollectionWithoutRollUp As NexusProvider.AccountDetailsDefaults
                    oAccountDetailsCollectionWithoutRollUp = CType(Cache.Item(ViewState("AccountCollWithoutRollUpCacheID")), NexusProvider.AccountDetailsDefaults)
                    Dim AccoundDetailForCurrentRow = From accountdetail In oAccountDetailsCollectionWithoutRollUp.AccountDetails
                                                     Where Trim(accountdetail.DocRef.ToString()) = oAccountDetailsCollection.DocRef
                                                     Order By accountdetail.DocumentRef Select accountdetail
                    If e.Row.FindControl("gvClientAccountChild") IsNot Nothing Then
                        Dim gvClientAccountChild As GridView = CType(e.Row.FindControl("gvClientAccountChild"), GridView)

                        'Bind if we have more than one record
                        If AccoundDetailForCurrentRow.Count > 1 Then
                            pdsAccountDetails = New PagedDataSource()
                            pdsAccountDetails.DataSource = AccoundDetailForCurrentRow.ToList()
                            gvClientAccountChild.DataSource = pdsAccountDetails
                            gvClientAccountChild.DataBind()
                            pdsAccountDetails = Nothing
                            bIsExpand = True
                        Else
                            bIsExpand = False
                        End If
                    End If
                End If

                'Show/hide the expand icon based on whether we have at least a record in child grid
                Dim imgExpand As Image = e.Row.FindControl("imgExpand")
                If bIsExpand Then
                    imgExpand.Visible = True
                Else
                    imgExpand.Visible = False
                End If

                Dim accountAmountCell As TableCell = e.Row.Cells(6)
                Dim paidDateCell As TableCell = e.Row.Cells(7)
                Dim accountOutStandingAmountCell As TableCell = e.Row.Cells(9)
                If accountAmountCell.Text = accountOutStandingAmountCell.Text Then
                    e.Row.Cells(7).Text = ""
                End If
            End If
        End Sub
        ''' <summary>
        ''' Apply relevant Currency format to Amount and O/S Currency Amount columns for child grids
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Protected Sub gvClientAccountChild_RowDataBound(sender As Object, e As GridViewRowEventArgs)
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim oAccountDetails As NexusProvider.AccountDetails
                oAccountDetails = CType(e.Row.DataItem, NexusProvider.AccountDetails)
                e.Row.Cells(kAccountAmountColChild).Text = New Money(oAccountDetails.AccountAmount, oAccountDetails.AccountCurrencyCode).Formatted 'Account Amount
                e.Row.Cells(kAccountOSAmountColChild).Text = New Money(oAccountDetails.OutstandingAmount, oAccountDetails.CurrencyCode).Formatted
                e.Row.Cells(kCurrencyAmountColChild).Text = New Money(oAccountDetails.CurrencyAmount, oAccountDetails.BaseCurrencyCode).Formatted 'Currency Amount
                e.Row.Cells(kOSCurrencyAmountColChild).Text = New Money(oAccountDetails.OutStandingCurrencyAmount, oAccountDetails.BaseCurrencyCode).Formatted 'OutStanding Currency Amount
            End If
        End Sub
        ''' <summary>
        ''' Apply relevant Currency format to Amount and O/S Amount columns for child grids
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Protected Sub gvClientAccountChild_DataBinding(sender As Object, e As EventArgs)
            Dim oCurrencies As Nexus.Library.Config.Currencies
            Dim gvClientAccountChild As GridView = CType(sender, GridView)
            oCurrencies = CType(GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Currencies
            If ViewState("AccountCurrencyCode") IsNot Nothing Then
                gvClientAccountChild.Columns(kAccountAmountColChild).HeaderText = Replace(gvClientAccountChild.Columns(kAccountAmountColChild).HeaderText, "[!AccountCurrency!]", oCurrencies.Currency(ViewState("AccountCurrencyCode")).Symbol)
                gvClientAccountChild.Columns(kAccountOSAmountColChild).HeaderText = Replace(gvClientAccountChild.Columns(kAccountOSAmountColChild).HeaderText, "[!AccountCurrency!]", oCurrencies.Currency(ViewState("AccountCurrencyCode")).Symbol)
            Else
                If ViewState("BaseCurrency") Is Nothing Then
                    Dim oWebService As ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim oCurrencyColl As NexusProvider.CurrencyCollection
                    oCurrencyColl = oWebService.GetCurrenciesByBranch(Session(CNBranchCode))
                    ViewState("BaseCurrency") = oCurrencyColl(0).BaseCurrencyCode
                End If
                gvClientAccountChild.Columns(kAccountAmountColChild).HeaderText = Replace(gvClientAccountChild.Columns(kAccountAmountColChild).HeaderText, "[!AccountCurrency!]", oCurrencies.Currency(ViewState("BaseCurrency").Trim).Symbol)
                gvClientAccountChild.Columns(kAccountOSAmountColChild).HeaderText = Replace(gvClientAccountChild.Columns(kAccountOSAmountColChild).HeaderText, "[!AccountCurrency!]", oCurrencies.Currency(ViewState("BaseCurrency").Trim).Symbol)
            End If
        End Sub

        ''' <summary>
        ''' Search and display filtered content
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
            Dim oWebService As NexusProvider.ProviderBase
            Dim oAccountDetailsCollectionWithoutRollUp As NexusProvider.AccountDetailsDefaults
            Dim oOptionSetting As NexusProvider.OptionTypeSetting

            Try
                oWebService = New NexusProvider.ProviderManager().Provider
                oAccountdDetails = New NexusProvider.AccountDetails
                'setting the System Option configuration
                oOptionSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, kDebtRollUpOptionNumber)
                If oOptionSetting.OptionValue = "1" Then
                    oAccountdDetails.Rollup = True
                End If

                'setting the default values passed in Request Parameter
                oAccountdDetails.PartyCnt = IIf(ViewState("PartyKey") <> Nothing, CType(ViewState("PartyKey"), Integer), 0)
                If oAccountdDetails.PartyCnt = 0 Then
                    oAccountdDetails.PartyCnt = DirectCast(Session(CNParty), BaseParty).Key
                End If
                oAccountdDetails.OutstandingOnly = chkOutstandingTransaction.Checked
                oAccountdDetails.OutstandingOnlySpecified = True
                oAccountdDetails.Display500 = True

                If Not txtPolicyNumber.Text.Trim().Length = 0 Then
                    oAccountdDetails.InsuranceRef = txtPolicyNumber.Text.Trim()
                End If

                If Not DocumentType.Text.Trim().Length = 0 Then
                    oAccountdDetails.DocumentTypeCode = DocumentType.Value
                End If

                If Not txtDocumentRef.Text.Trim().Length = 0 Then
                    oAccountdDetails.DocumentRef = txtDocumentRef.Text.Trim()
                End If

                'Enable Debt Rollup if back office option is turned on
                oOptionSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, kDebtRollUpOptionNumber)
                If oOptionSetting.OptionValue = "1" Then
                    oAccountdDetails.Rollup = True
                End If

                oAccountDetailsCollection = oWebService.GetAccountDetails(oAccountdDetails)

                Dim AccountCollCacheID As Guid
                AccountCollCacheID = Guid.NewGuid()
                If ViewState("AccountCollCacheID") Is Nothing Then
                    ViewState.Add("AccountCollCacheID", AccountCollCacheID.ToString)
                Else
                    ViewState("AccountCollCacheID") = AccountCollCacheID.ToString
                End If

                Cache.Insert(ViewState("AccountCollCacheID"), oAccountDetailsCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

                If oAccountDetailsCollection.AccountDetails.Count > 0 Then
                    ViewState("AccountCurrencyCode") = oAccountDetailsCollection.AccountDetails(0).AccountCurrencyCode.Trim
                End If

                If oOptionSetting.OptionValue = "1" Then
                    oAccountdDetails.Rollup = False
                    oAccountDetailsCollectionWithoutRollUp = oWebService.GetAccountDetails(oAccountdDetails)
                    If ViewState("AccountCollWithoutRollUpCacheID") Is Nothing Then
                        AccountCollCacheID = Guid.NewGuid()
                        ViewState.Add("AccountCollWithoutRollUpCacheID", AccountCollCacheID.ToString)
                    Else
                        AccountCollCacheID = Guid.NewGuid()
                        ViewState("AccountCollWithoutRollUpCacheID") = AccountCollCacheID.ToString
                    End If

                    Cache.Insert(ViewState("AccountCollWithoutRollUpCacheID"), oAccountDetailsCollectionWithoutRollUp, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
                End If

                '' Exludes any transactions of following types and bind the grid and reset total amount outstanding
                BindParentDataGrid(gvClientAccountParent, oAccountDetailsCollection, True)

            Finally
                oWebService = Nothing
                oAccountDetailsCollection = Nothing
                oAccountDetailsCollectionWithoutRollUp = Nothing
                oAccountdDetails = Nothing
            End Try
        End Sub

        ''' <summary>
        ''' Sorting based on Document Ref, Document Type, Effective Date, Transaction Date, Media Type, Paid Date, Policy Number.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvClientAccountParent_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles gvClientAccountParent.Sorting

            Dim oAccountDetailsCollection As NexusProvider.AccountDetailsDefaults

            If (ViewState("AccountCollCacheID") Is Nothing OrElse Cache.Item(ViewState("AccountCollCacheID")) Is Nothing) AndAlso (Session(CNParty) IsNot Nothing AndAlso Me.PartyKey = 0 AndAlso Session(CNClientMode) = Mode.View _
                  AndAlso CType(Session(CNParty), NexusProvider.BaseParty).Key <> 0 AndAlso
                Session(CNIsNewClient) Is Nothing) Then

                Dim oParty As NexusProvider.BaseParty = Session(CNParty)
                PartyKey = oParty.Key

            End If
            oAccountDetailsCollection = CType(Cache.Item(ViewState("AccountCollCacheID")), NexusProvider.AccountDetailsDefaults)

            oAccountDetailsCollection.AccountDetails.SortColumn = e.SortExpression
            'check that the sort expression is the same as stored in viewstate as we should start again if reordering by a new column
            Dim oSortDirection As New SortDirection
            If ViewState("SortDirection") = SortDirection.Ascending And ViewState("SortExpression") = e.SortExpression _
                Then
                oSortDirection = SortDirection.Descending
            Else
                oSortDirection = SortDirection.Ascending
            End If
            'store the current sortdirection for comparison on the next sort
            ViewState("SortDirection") = oSortDirection
            'store the SortExpression in viewstate so that we can check if we are sorting by a new column on the next sort
            ViewState("SortExpression") = e.SortExpression
            oAccountDetailsCollection.AccountDetails.SortingOrder = oSortDirection
            oAccountDetailsCollection.AccountDetails.Sort()

            BindParentDataGrid(CType(sender, GridView), oAccountDetailsCollection, False)
        End Sub



    End Class

End Namespace
