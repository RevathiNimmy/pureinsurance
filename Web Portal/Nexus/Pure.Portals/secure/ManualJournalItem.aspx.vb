Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports System.Web.Configuration
Imports Nexus.Library
Imports Nexus.Utils
Imports System.Web.HttpContext
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session

Partial Class secure_ManualJournalItem 
    Inherits Frontend.clsCMSPage

    Dim oAccountSearchResultCollection As NexusProvider.AccountSearchResultCollection
    Dim oManualJournalItemCollection, oEditManualJournalItemCollection As NexusProvider.ManualJournalItemCollection
    Dim oCurrency As NexusProvider.Currency
    Dim oManualJournalItem As NexusProvider.ManualJournalItem
    Dim oWebService As NexusProvider.ProviderBase
    Dim oBankAccounts As NexusProvider.AccountDetailsCollection
    Dim oSessionManualJournal As NexusProvider.ManualJournal
    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'To set the Focus
        Page.SetFocus(PMLookup_CurrencyType)

        '
        oSessionManualJournal = Session(CNManualJournal)
        'PMLookup_CurrencyType.Focus()
        txtCurrencyRate.Attributes.Add("readonly", "readonly")
        If Not IsPostBack Then
            txtAmount.Focus()
            txtBaseAmount.Attributes.Add("readonly", "readonly")
            oAccountSearchResultCollection = Nothing
            Session(CNResultSet) = Nothing
            hiddenAccountKey.Value = Session(CNAccountkey)

            'Setting of the currencies -Start
            oWebService = New NexusProvider.ProviderManager().Provider
            oBankAccounts = New NexusProvider.AccountDetailsCollection
            oBankAccounts = oWebService.GetBankAccounts()
            oCurrency = New NexusProvider.Currency




            If oBankAccounts.Count > 0 Then
                If oBankAccounts(0).CurrencyCode IsNot Nothing Then
                    'PMLookup_CurrencyType.Value = oBankAccounts(0).CurrencyCode.Trim
                    oCurrency.AccountCode = oBankAccounts(0).AccountCode
                    oCurrency.Mode = "All"
                    PMLookup_CurrencyType.Value = oBankAccounts(0).CurrencyCode.Trim
                    oCurrency.TransactionCurrencyCode = PMLookup_CurrencyType.Value.Trim()
                    'get currency exchange rates from SAM
                    oCurrency = oWebService.GetCurrencyExchangeRates(oCurrency)
                    txtCurrencyRate.Text = oCurrency.TransactionCurrencyRate.ToString()
                    'selecetd default currency on the basis of barch selected on MultiBranch page
                    PMLookup_CurrencyType.Value = Session("CurrencyCollections")
                    oCurrency.TransactionCurrencyCode = PMLookup_CurrencyType.Value.Trim()
                End If
            ElseIf Session(CNCurrenyCode) IsNot Nothing Then
                PMLookup_CurrencyType.Value = Session(CNCurrenyCode)

            End If
            'Setting of the currencies -End

            'check if this page in in edit mode, if so, set the control's values accordingly
            'from the Session(CNManualJournalItemCollection)

            If HttpContext.Current.Request.QueryString("Mode") IsNot Nothing AndAlso HttpContext.Current.Request.QueryString("MJItem") IsNot Nothing Then

                If HttpContext.Current.Request.QueryString("Mode").ToString = "Edit" Then
                    If Session(CNManualJournalItemCollection) IsNot Nothing Then
                        oEditManualJournalItemCollection = New NexusProvider.ManualJournalItemCollection
                        oEditManualJournalItemCollection = CType(Session(CNManualJournalItemCollection), NexusProvider.ManualJournalItemCollection)

                        Dim iCount As Integer
                        For iCount = 0 To oEditManualJournalItemCollection.Count - 1
                            If HttpContext.Current.Request.QueryString("MJItem").ToString() = iCount Then

                                txtAccount.Text = oEditManualJournalItemCollection(iCount).AccountKey.ToString()

                                'set the appropriate currency code here
                                PMLookup_CurrencyType.Value = oEditManualJournalItemCollection(iCount).CurrencyTypeCode

                                txtAmount.Text = oEditManualJournalItemCollection(iCount).Amount.ToString()
                                txtCurrencyRate.Text = oEditManualJournalItemCollection(iCount).CurrencyRate
                                txtBaseAmount.Text = oEditManualJournalItemCollection(iCount).BaseAmount.ToString()
                                txtAltReference.Text = oEditManualJournalItemCollection(iCount).AltReference
                                txtComment.Text = oEditManualJournalItemCollection(iCount).Comment

                                'set appropritae underwriting code here
                                PMLookup_UnderwritingYear.Value = oEditManualJournalItemCollection(iCount).UnderwritingYearCode

                                'set appropriate cost centre code here
                                PMLookup_CostCentre.Value = oEditManualJournalItemCollection(iCount).CostCentreCode

                                txtInsuranceRef.Text = oEditManualJournalItemCollection(iCount).InsuranceRef
                                txtPONumber.Text = oEditManualJournalItemCollection(iCount).PurchaseOrderNumber
                                txtPurchaseInvoiceNumber.Text = oEditManualJournalItemCollection(iCount).PurchaseInvoiceNumber

                            End If
                        Next
                    End If
                End If
            End If

            'cleaning
            oWebService = Nothing
            oBankAccounts = Nothing
            oCurrency = Nothing
            oEditManualJournalItemCollection = Nothing




        End If
    End Sub

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click

        If Page.IsValid Then
            'if account key is not set
            If String.IsNullOrEmpty(txtAccount.Text) = False Then
                Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
                Dim oAccountSearchCriteria As New NexusProvider.AccountSearchCriteria
                oAccountSearchCriteria.ShortCode = txtAccount.Text.Trim()
                oAccountSearchCriteria.ExcludeInsurerAgents = False
                oAccountSearchCriteria.IncludeInsurerAgents = False
                oAccountSearchCriteria.MaxRowsToFetch = oPortal.MaxSearchResults

                oWebService = New NexusProvider.ProviderManager().Provider
                oAccountSearchResultCollection = oWebService.FindAccounts(oAccountSearchCriteria)

                If oAccountSearchResultCollection IsNot Nothing AndAlso oAccountSearchResultCollection.Count > 0 Then
                    hiddenAccountKey.Value = oAccountSearchResultCollection(0).AccountKey
                    hiddenAccountName.Value = oAccountSearchResultCollection(0).AccountName
                Else
                    vldrqdAccount.IsValid = False
                    Exit Sub
                End If
            End If

            'check if the page is in edit mode, update the session item collection from the cotrol's values
            If HttpContext.Current.Request.QueryString("Mode") IsNot Nothing Then

                If txtAccount.Text.Length > 0 AndAlso txtAmount.Text.Length > 0 Then

                    If Session(CNManualJournalItemCollection) IsNot Nothing Then
                        oManualJournalItemCollection = New NexusProvider.ManualJournalItemCollection
                        oManualJournalItemCollection = CType(Session(CNManualJournalItemCollection), NexusProvider.ManualJournalItemCollection)

                        For iMJItemCount As Integer = 0 To oManualJournalItemCollection.Count - 1
                            If iMJItemCount = Convert.ToInt32(HttpContext.Current.Request.QueryString("MJItem")) Then

                                oManualJournalItemCollection(iMJItemCount).ManualJournalKey = Convert.ToInt32(HttpContext.Current.Request.QueryString("MJItem"))
                                oManualJournalItemCollection(iMJItemCount).AccountKey = txtAccount.Text
                                oManualJournalItemCollection(iMJItemCount).AltReference = txtAltReference.Text
                                oManualJournalItemCollection(iMJItemCount).CurrencyTypeCode = PMLookup_CurrencyType.Value
                                oManualJournalItemCollection(iMJItemCount).CurrencyTypeDescription = PMLookup_CurrencyType.Text
                                oManualJournalItemCollection(iMJItemCount).Amount = Convert.ToDecimal(txtAmount.Text)
                                oManualJournalItemCollection(iMJItemCount).BaseAmount = Convert.ToDecimal(txtBaseAmount.Text)
                                oManualJournalItemCollection(iMJItemCount).Comment = txtComment.Text.Trim()
                                oManualJournalItemCollection(iMJItemCount).CostCentreCode = PMLookup_CostCentre.Value
                                oManualJournalItemCollection(iMJItemCount).CostCentreDescription = PMLookup_CostCentre.Text

                                oManualJournalItemCollection(iMJItemCount).CurrencyRate = Convert.ToDecimal(txtCurrencyRate.Text.Trim())
                                oManualJournalItemCollection(iMJItemCount).InsuranceRef = txtInsuranceRef.Text.Trim()
                                oManualJournalItemCollection(iMJItemCount).PurchaseInvoiceNumber = txtPurchaseInvoiceNumber.Text.Trim()
                                oManualJournalItemCollection(iMJItemCount).PurchaseOrderNumber = txtPONumber.Text.Trim()
                                oManualJournalItemCollection(iMJItemCount).UnderwritingYearCode = PMLookup_UnderwritingYear.Value
                                oManualJournalItemCollection(iMJItemCount).UnderwritingYearDescription = PMLookup_UnderwritingYear.Text

                                Exit For
                            End If
                        Next

                    End If
                End If

            Else

                'if the page is not in edit mode, then create the session for manual journal item collection
                If txtAccount.Text.Length > 0 AndAlso txtAmount.Text.Length > 0 Then

                    oManualJournalItem = New NexusProvider.ManualJournalItem
                    oManualJournalItemCollection = New NexusProvider.ManualJournalItemCollection

                    'create a key to identify the manual journal item in the transaction grid
                    If oSessionManualJournal.JournalKey = 0 Then
                        oSessionManualJournal.JournalKey = "1"
                    ElseIf oSessionManualJournal.JournalKey > 0 Then
                        oSessionManualJournal.JournalKey = oSessionManualJournal.JournalKey + 1
                    End If

                    oManualJournalItem.ManualJournalKey = oSessionManualJournal.JournalKey
                    oManualJournalItem.AccountKey = txtAccount.Text
                    oManualJournalItem.AccountName = hiddenAccountName.Value
                    oManualJournalItem.AltReference = txtAltReference.Text
                    oManualJournalItem.CurrencyTypeCode = PMLookup_CurrencyType.Value
                    oManualJournalItem.CurrencyTypeDescription = PMLookup_CurrencyType.Text
                    oManualJournalItem.Amount = Convert.ToDecimal(txtAmount.Text)
                    oManualJournalItem.BaseAmount = Convert.ToDecimal(txtBaseAmount.Text)
                    oManualJournalItem.Comment = txtComment.Text.Trim()
                    oManualJournalItem.CostCentreCode = PMLookup_CostCentre.Value
                    oManualJournalItem.CostCentreDescription = PMLookup_CostCentre.Text

                    oManualJournalItem.CurrencyRate = Convert.ToDecimal(txtCurrencyRate.Text.Trim())
                    oManualJournalItem.InsuranceRef = txtInsuranceRef.Text.Trim()
                    oManualJournalItem.PurchaseInvoiceNumber = txtPurchaseInvoiceNumber.Text.Trim()
                    oManualJournalItem.PurchaseOrderNumber = txtPONumber.Text.Trim()
                    oManualJournalItem.UnderwritingYearCode = PMLookup_UnderwritingYear.Value
                    oManualJournalItem.UnderwritingYearDescription = PMLookup_UnderwritingYear.Text

                    'check here for existing collection in session
                    If Session(CNManualJournalItemCollection) IsNot Nothing Then
                        oManualJournalItemCollection = CType(Session(CNManualJournalItemCollection), NexusProvider.ManualJournalItemCollection)
                    End If

                    oManualJournalItemCollection.Add(oManualJournalItem)

                    'create session for manual journal item collection here and 
                    'populate the parent page as per this session
                    Session(CNManualJournal) = oSessionManualJournal
                End If
            End If

            Session(CNManualJournalItemCollection) = oManualJournalItemCollection

            'finally send the focus onto manual journal page where transactions grid will
            'be populated as per the session's item collection
            Response.Redirect("~/secure/manualjournal.aspx?Mode=Journal", False)

        End If
    End Sub
    
    Protected Sub CurrencyType_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles PMLookup_CurrencyType.SelectedIndexChange

        oWebService = New NexusProvider.ProviderManager().Provider
        oBankAccounts = oWebService.GetBankAccounts()
        oCurrency = New NexusProvider.Currency

        If oBankAccounts.Count > 0 Then
            If oBankAccounts(0).CurrencyCode IsNot Nothing Then
                oCurrency.AccountCode = oBankAccounts(0).AccountCode
                oCurrency.TransactionCurrencyCode = PMLookup_CurrencyType.Value.Trim()
                oCurrency.Mode = "All"
                'added oSessionManualJournal.JournalBranchCode parameter 
                oCurrency = oWebService.GetCurrencyExchangeRates(oCurrency, oSessionManualJournal.JournalBranchCode)
                txtCurrencyRate.Text = oCurrency.BaseCurrencyRate
                If txtAmount.Text.Length > 0 AndAlso IsNumeric(txtAmount.Text.Trim()) Then
                    txtBaseAmount.Text = Convert.ToDecimal(txtCurrencyRate.Text) * Convert.ToDecimal(txtAmount.Text)
                    txtBaseAmount.Text = Math.Round(Convert.ToDecimal(txtBaseAmount.Text), 2, System.MidpointRounding.AwayFromZero).ToString
                End If
            End If
        End If
       
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("~/secure/manualjournal.aspx?Mode=Journal", False)
    End Sub

    Protected Sub custvldAmount_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        If Not IsNumeric(txtAmount.Text) Then
            CType(source, WebControls.CustomValidator).ErrorMessage = GetLocalResourceObject("custvldAmount")
            args.IsValid = False
            Exit Sub
        ElseIf CDbl(txtAmount.Text) = 0.0 Then
            CType(source, WebControls.CustomValidator).ErrorMessage = GetLocalResourceObject("custvldAmount")
            args.IsValid = False
        End If
    End Sub
End Class
