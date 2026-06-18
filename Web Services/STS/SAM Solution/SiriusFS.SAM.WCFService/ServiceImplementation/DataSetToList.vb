Imports SiriusFS.SAM.Structure
Imports SiriusFS.SAM.Structure.SFI.SAMForInsuranceV2.WCF
Imports System.Reflection
Imports System.IO
Partial Public Class PureService


    ''' <summary>
    ''' Data table to List conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetClaimPerilSummaryReserveType(dtResultSet As DataTable) As List(Of BaseGetClaimPerilSummaryResponseTypeReserveTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetClaimPerilSummaryResponseTypeReserveTypeRow With {
        .Description = If(IsDBNull(row("Description")), Nothing, row("Description")),
        .InitialReserve = If(IsDBNull(row("InitialReserve")), Nothing, row("InitialReserve")),
        .PaidAmount = If(IsDBNull(row("PaidAmount")), Nothing, row("PaidAmount")),
        .RevisedReserve = If(IsDBNull(row("RevisedReserve")), Nothing, row("RevisedReserve")),
        .CurrentReserve = If(IsDBNull(row("CurrentReserve")), Nothing, row("CurrentReserve")),
        .SumInsured = If(IsDBNull(row("SumInsured")), Nothing, row("SumInsured")),
        .Average = If(IsDBNull(row("Average")), Nothing, row("Average"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetRatingSectionTypes(dtResultSet As DataTable) As List(Of BaseGetRatingSectionTypesResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetRatingSectionTypesResponseTypeRow With {
        .RatingSectionTypeId = If(IsDBNull(row("RatingSectionTypeId")), Nothing, row("RatingSectionTypeId")),
        .RatingSectionTypeCode = If(IsDBNull(row("RatingSectionTypeCode")), Nothing, row("RatingSectionTypeCode")),
        .Description = If(IsDBNull(row("Description")), Nothing, row("Description")),
        .RateTypeId = If(IsDBNull(row("RateTypeId")), Nothing, row("RateTypeId")),
        .RateTypeCode = If(IsDBNull(row("RateTypeCode")), Nothing, row("RateTypeCode")),
        .Rate = If(IsDBNull(row("Rate")), Nothing, row("Rate")),
        .CurrencyId = If(IsDBNull(row("CurrencyId")), Nothing, row("CurrencyId")),
        .CurrencyCode = If(IsDBNull(row("CurrencyCode")), Nothing, row("CurrencyCode")),
        .CountryId = If(IsDBNull(row("CountryId")), Nothing, row("CountryId")),
        .CountryCode = If(IsDBNull(row("CountryCode")), Nothing, row("CountryCode")),
        .StateId = If(IsDBNull(row("StateId")), Nothing, row("StateId")),
        .StateCode = If(IsDBNull(row("StateCode")), Nothing, row("StateCode")),
        .EarningPatternCode = If(IsDBNull(row("EarningPatternCode")), Nothing, row("EarningPatternCode"))})
        Return myData.ToList()
    End Function

    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_CheckPendingOOSVersions(dtResultSet As DataTable) As List(Of BaseCheckPendingOOSVersionsResponseTypePolicies)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseCheckPendingOOSVersionsResponseTypePolicies With {
         .BaseInsuranceFileKey = If(IsDBNull(row("BaseInsuranceFileKey")), Nothing, row("BaseInsuranceFileKey"))})
        Return myData.ToList()
    End Function


    ''' <summary>
    ''' Data table to List conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_FindAccounts(dtResultSet As DataTable) As List(Of BaseFindAccountsResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseFindAccountsResponseTypeRow With {
        .FullKey = If(IsDBNull(row("FullKey")), Nothing, row("FullKey")),
        .ShortCode = If(IsDBNull(row("ShortCode")), Nothing, row("ShortCode")),
        .AccountName = If(IsDBNull(row("AccountName")), Nothing, row("AccountName")),
        .LedgerKey = If(IsDBNull(row("LedgerKey")), Nothing, row("LedgerKey")),
        .AccountTypeKey = If(IsDBNull(row("AccountTypeKey")), Nothing, row("AccountTypeKey")),
        .AccountKey = If(IsDBNull(row("AccountKey")), Nothing, row("AccountKey")),
        .NominalAccountKey = If(IsDBNull(row("NominalAccountKey")), Nothing, row("NominalAccountKey")),
        .AccountStatus = If(IsDBNull(row("AccountStatus")), Nothing, row("AccountStatus")),
        .AccountStatusKey = If(IsDBNull(row("AccountStatusKey")), Nothing, row("AccountStatusKey")),
        .CompanyKey = If(IsDBNull(row("CompanyKey")), Nothing, row("CompanyKey")),
        .ContactName = If(IsDBNull(row("ContactName")), Nothing, row("ContactName")),
        .AddressLine1 = If(IsDBNull(row("AddressLine1")), Nothing, row("AddressLine1")),
        .PersonalClientForename = If(IsDBNull(row("PersonalClientForename")), Nothing, row("PersonalClientForename")),
        .LedgerCode = If(IsDBNull(row("LedgerCode")), Nothing, row("LedgerCode")),
        .AccountTypeCode = If(IsDBNull(row("AccountTypeCode")), Nothing, row("AccountTypeCode")),
        .AccountBalance = If(IsDBNull(row("AccountBalance")), Nothing, row("AccountBalance")),
        .PartyKey = If(IsDBNull(row("PartyKey")), Nothing, row("PartyKey")),
        .CurrencyId = If(IsDBNull(row("CurrencyId")), Nothing, row("CurrencyId")),
        .CurrencyCode = If(IsDBNull(row("CurrencyCode")), Nothing, row("CurrencyCode")),
        .SourceId = If(IsDBNull(row("SourceId")), Nothing, row("SourceId")),
        .SourceCode = If(IsDBNull(row("SourceCode")), Nothing, row("SourceCode")),
        .IsGrossAgent = If(IsDBNull(row("IsGrossAgent")), Nothing, row("IsGrossAgent"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_FindCashListReceipts(dtResultSet As DataTable) As List(Of BaseFindCashListReceiptsResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseFindCashListReceiptsResponseTypeRow With {
        .CashListItemKey = If(IsDBNull(row("CashListItemKey")), Nothing, row("CashListItemKey")),
        .InsuranceFileKey = If(IsDBNull(row("InsuranceFileKey")), Nothing, row("InsuranceFileKey")),
        .MediaTypeKey = If(IsDBNull(row("MediaTypeKey")), Nothing, row("MediaTypeKey")),
        .MediaTypeDescription = If(IsDBNull(row("MediaTypeDescription")), Nothing, row("MediaTypeDescription")),
        .MediaTypeStatusKey = If(IsDBNull(row("MediaTypeStatusKey")), Nothing, row("MediaTypeStatusKey")),
        .MediaTypeStatusDescription = If(IsDBNull(row("MediaTypeStatusDescription")), Nothing, row("MediaTypeStatusDescription")),
        .DocumentRef = If(IsDBNull(row("DocumentRef")), Nothing, row("DocumentRef")),
        .BranchDescription = If(IsDBNull(row("BranchDescription")), Nothing, row("BranchDescription")),
        .ClientCode = If(IsDBNull(row("ClientCode")), Nothing, row("ClientCode")),
        .ClientName = If(IsDBNull(row("ClientName")), Nothing, row("ClientName")),
        .PolicyNumber = If(IsDBNull(row("PolicyNumber")), Nothing, row("PolicyNumber")),
        .DrawnBankName = If(IsDBNull(row("DrawnBankName")), Nothing, row("DrawnBankName")),
        .MediaReference = If(IsDBNull(row("MediaReference")), Nothing, row("MediaReference")),
        .MediaTypeCode = If(IsDBNull(row("MediaTypeCode")), Nothing, row("MediaTypeCode")),
        .MediaTypeStatusCode = If(IsDBNull(row("MediaTypeStatusCode")), Nothing, row("MediaTypeStatusCode")),
        .CurrentStatus = If(IsDBNull(row("CurrentStatus")), Nothing, row("CurrentStatus"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetAccountBalance(dtResultSet As DataTable) As List(Of BaseGetAccountBalanceResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetAccountBalanceResponseTypeRow With {
        .SumAmount = If(IsDBNull(row("SumAmount")), Nothing, row("SumAmount")),
        .CurrencyCode = If(IsDBNull(row("CurrencyCode")), Nothing, row("CurrencyCode")),
        .FloatBalance = If(IsDBNull(row("FloatBalance")), Nothing, row("FloatBalance")),
        .Overdraft = If(IsDBNull(row("Overdraft")), Nothing, row("Overdraft"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetAccountingPeriod(dtResultSet As DataTable) As List(Of BaseGetAccountingPeriodResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetAccountingPeriodResponseTypeRow With {
        .PeriodKey = If(IsDBNull(row("PeriodKey")), Nothing, row("PeriodKey")),
        .YearName = If(IsDBNull(row("YearName")), Nothing, row("YearName")),
        .PeriodName = If(IsDBNull(row("PeriodName")), Nothing, row("PeriodName")),
        .PeriodEndDate = If(IsDBNull(row("PeriodEndDate")), Nothing, row("PeriodEndDate"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetAllocationDetails(dtResultSet As DataTable) As List(Of BaseGetAllocationDetailsResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetAllocationDetailsResponseTypeRow With {
        .DocRef = If(IsDBNull(row("DocRef")), Nothing, row("DocRef")),
        .TransDate = If(IsDBNull(row("TransDate")), Nothing, row("TransDate")),
        .AllocatedDate = If(IsDBNull(row("AllocatedDate")), Nothing, row("AllocatedDate")),
        .AllocatedAmount = If(IsDBNull(row("AllocatedAmount")), Nothing, row("AllocatedAmount")),
        .OriginalAmount = If(IsDBNull(row("OriginalAmount")), Nothing, row("OriginalAmount")),
        .WriteOffAmount = If(IsDBNull(row("WriteOffAmount")), Nothing, row("WriteOffAmount")),
        .DocType = If(IsDBNull(row("DocType")), Nothing, row("DocType")),
        .InsuranceRef = If(IsDBNull(row("InsuranceRef")), Nothing, row("InsuranceRef")),
        .Account = If(IsDBNull(row("Account")), Nothing, row("Account")),
        .User = If(IsDBNull(row("User")), Nothing, row("User")),
        .TransDetailKey = If(IsDBNull(row("TransDetailKey")), Nothing, row("TransDetailKey")),
        .AllocationKey = If(IsDBNull(row("AllocationKey")), Nothing, row("AllocationKey")),
        .Spare = If(IsDBNull(row("Spare")), Nothing, row("Spare")),
        .CurrencyCode = If(IsDBNull(row("CurrencyCode")), Nothing, row("CurrencyCode"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetBalancesAndUnallocatedCredits(dtResultSet As DataTable) As List(Of BaseGetBalancesAndUnallocatedCreditsResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetBalancesAndUnallocatedCreditsResponseTypeRow With {
        .TransDetailKey = If(IsDBNull(row("TransDetailKey")), Nothing, row("TransDetailKey")),
        .DocumentRef = If(IsDBNull(row("DocumentRef")), Nothing, row("DocumentRef")),
        .MediaType = If(IsDBNull(row("MediaType")), Nothing, row("MediaType")),
        .Reference = If(IsDBNull(row("Reference")), Nothing, row("Reference")),
        .Amount = If(IsDBNull(row("Amount")), Nothing, row("Amount")),
        .AccountKey = If(IsDBNull(row("AccountKey")), Nothing, row("AccountKey")),
        .CollectionDate = If(IsDBNull(row("CollectionDate")), Nothing, row("CollectionDate"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetBalancesAndUnallocatedCredits1(dtResultSet As DataTable) As List(Of BaseGetBalancesAndUnallocatedCreditsResponseTypeRow1)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetBalancesAndUnallocatedCreditsResponseTypeRow1 With {
        .TransDetailKey = If(IsDBNull(row("TransDetailKey")), Nothing, row("TransDetailKey")),
        .DocumentRef = If(IsDBNull(row("DocumentRef")), Nothing, row("DocumentRef")),
        .MediaType = If(IsDBNull(row("MediaType")), Nothing, row("MediaType")),
        .Reference = If(IsDBNull(row("Reference")), Nothing, row("Reference")),
        .Amount = If(IsDBNull(row("Amount")), Nothing, row("Amount")),
        .AccountKey = If(IsDBNull(row("AccountKey")), Nothing, row("AccountKey")),
        .CollectionDate = If(IsDBNull(row("CollectionDate")), Nothing, row("CollectionDate"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetPaymentCashListItems(dtResultSet As DataTable) As List(Of BaseGetPaymentCashListItemsResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetPaymentCashListItemsResponseTypeRow With {
        .CashListItemKey = If(IsDBNull(row("CashListItemKey")), Nothing, row("CashListItemKey")),
        .MediaReference = If(IsDBNull(row("MediaReference")), Nothing, row("MediaReference")),
        .MediaType = If(IsDBNull(row("MediaType")), Nothing, row("MediaType")),
        .Amount = If(IsDBNull(row("Amount")), Nothing, row("Amount")),
        .AccountShortCode = If(IsDBNull(row("AccountShortCode")), Nothing, row("AccountShortCode")),
        .Status = If(IsDBNull(row("Status")), Nothing, row("Status")),
        .Letter = If(IsDBNull(row("Letter")), Nothing, row("Letter"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetReceiptCashListItems(dtResultSet As DataTable) As List(Of BaseGetReceiptCashListItemsResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetReceiptCashListItemsResponseTypeRow With {
        .CashListItemKey = If(IsDBNull(row("CashListItemKey")), Nothing, row("CashListItemKey")),
        .MediaReference = If(IsDBNull(row("MediaReference")), Nothing, row("MediaReference")),
        .MediaType = If(IsDBNull(row("MediaType")), Nothing, row("MediaType")),
        .Amount = If(IsDBNull(row("Amount")), Nothing, row("Amount")),
        .AccountShortCode = If(IsDBNull(row("AccountShortCode")), Nothing, row("AccountShortCode")),
        .Status = If(IsDBNull(row("Status")), Nothing, row("Status")),
        .Letter = If(IsDBNull(row("Letter")), Nothing, row("Letter"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetTransactionDetails(dtResultSet As DataTable) As List(Of BaseGetTransactionDetailsResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetTransactionDetailsResponseTypeRow With {
        .TransDetailKey = If(IsDBNull(row("TransDetailKey")), Nothing, row("TransDetailKey")),
        .DocRef = If(IsDBNull(row("DocRef")), Nothing, row("DocRef")),
        .AltRef = If(IsDBNull(row("AltRef")), Nothing, row("AltRef")),
        .EffectiveDate = If(IsDBNull(row("EffectiveDate")), Nothing, row("EffectiveDate")),
        .TransDate = If(IsDBNull(row("TransDate")), Nothing, row("TransDate")),
        .MediaType = If(IsDBNull(row("MediaType")), Nothing, row("MediaType")),
        .Amount = If(IsDBNull(row("Amount")), Nothing, row("Amount")),
        .OutstandingAmount = If(IsDBNull(row("OutstandingAmount")), Nothing, row("OutstandingAmount")),
        .MediaRef = If(IsDBNull(row("MediaRef")), Nothing, row("MediaRef")),
        .Accountkey = If(IsDBNull(row("Accountkey")), Nothing, row("Accountkey")),
        .AccountCode = If(IsDBNull(row("AccountCode")), Nothing, row("AccountCode")),
        .Currency = If(IsDBNull(row("Currency")), Nothing, row("Currency")),
        .CurrencyCode = If(IsDBNull(row("CurrencyCode")), Nothing, row("CurrencyCode")),
        .TaxBand = If(IsDBNull(row("TaxBand")), Nothing, row("TaxBand")),
        .TransactionCurrenciesAmount = If(IsDBNull(row("TransactionCurrenciesAmount")), Nothing, row("TransactionCurrenciesAmount")),
        .TransactionCurrency = If(IsDBNull(row("TransactionCurrency")), Nothing, row("TransactionCurrency")),
        .CurrencyDiff = If(IsDBNull(row("CurrencyDiff")), Nothing, row("CurrencyDiff")),
        .AllocationTimeStamp = If(IsDBNull(row("AllocationTimeStamp")), Nothing, row("AllocationTimeStamp")),
        .TransactionCurrencyCode = If(IsDBNull(row("TransactionCurrencyCode")), Nothing, row("TransactionCurrencyCode")),
        .SourceID = If(IsDBNull(row("SourceID")), Nothing, row("SourceID")),
        .InsuranceRef = If(IsDBNull(row("InsuranceRef")), Nothing, row("InsuranceRef")),
        .PeriodName = If(IsDBNull(row("PeriodName")), Nothing, row("PeriodName")),
        .DocType = If(IsDBNull(row("DocType")), Nothing, row("DocType")),
        .DoctypeGroup = If(IsDBNull(row("DoctypeGroup")), Nothing, row("DoctypeGroup")),
        .PrimarySettled = If(IsDBNull(row("PrimarySettled")), Nothing, row("PrimarySettled")),
        .Spare = If(IsDBNull(row("Spare")), Nothing, row("Spare")),
        .DocTypeID = If(IsDBNull(row("DocTypeID")), Nothing, row("DocTypeID")),
        .InsuranceFileCnt = If(IsDBNull(row("InsuranceFileCnt")), Nothing, row("InsuranceFileCnt"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetInsurerPayments(dtResultSet As DataTable) As List(Of BaseGetInsurerPaymentsResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetInsurerPaymentsResponseTypeRow With {
        .YearName = If(IsDBNull(row("YearName")), Nothing, row("YearName")),
        .DocumentId = If(IsDBNull(row("DocumentId")), Nothing, row("DocumentId")),
        .DocumentRef = If(IsDBNull(row("DocumentRef")), Nothing, row("DocumentRef")),
        .InsurerRef = If(IsDBNull(row("InsurerRef")), Nothing, row("InsurerRef")),
        .FullyPaidAmount = If(IsDBNull(row("FullyPaidAmount")), Nothing, row("FullyPaidAmount")),
        .ClientOutstanding = If(IsDBNull(row("ClientOutstanding")), Nothing, row("ClientOutstanding")),
        .ConsolidateBinder = If(IsDBNull(row("ConsolidateBinder")), Nothing, row("ConsolidateBinder")),
        .ShortName = If(IsDBNull(row("ShortName")), Nothing, row("ShortName")),
        .ResolvedName = If(IsDBNull(row("ResolvedName")), Nothing, row("ResolvedName")),
        .TransdetailId = If(IsDBNull(row("TransdetailId")), Nothing, row("TransdetailId")),
        .CompanyId = If(IsDBNull(row("CompanyId")), Nothing, row("CompanyId")),
        .AccountingDate = If(IsDBNull(row("AccountingDate")), Nothing, row("AccountingDate")),
        .CurrencyAmount = If(IsDBNull(row("CurrencyAmount")), Nothing, row("CurrencyAmount")),
        .CurrencyId = If(IsDBNull(row("CurrencyId")), Nothing, row("CurrencyId")),
        .CurrencyCode = If(IsDBNull(row("CurrencyCode")), Nothing, row("CurrencyCode")),
        .CurrencyBaseRate = If(IsDBNull(row("CurrencyBaseRate")), Nothing, row("CurrencyBaseRate")),
        .MarkedAmount = If(IsDBNull(row("MarkedAmount")), Nothing, row("MarkedAmount")),
        .PaidAmount = If(IsDBNull(row("PaidAmount")), Nothing, row("PaidAmount")),
        .Spare = If(IsDBNull(row("Spare")), Nothing, row("Spare")),
        .PeriodName = If(IsDBNull(row("PeriodName")), Nothing, row("PeriodName")),
        .Month = If(IsDBNull(row("Month")), Nothing, row("Month")),
        .AccountCurrencyId = If(IsDBNull(row("AccountCurrencyId")), Nothing, row("AccountCurrencyId")),
        .AccountCurrencyCode = If(IsDBNull(row("AccountCurrencyCode")), Nothing, row("AccountCurrencyCode")),
        .AccountBaseRate = If(IsDBNull(row("AccountBaseRate")), Nothing, row("AccountBaseRate")),
        .FullyPaidAccountAmount = If(IsDBNull(row("FullyPaidAccountAmount")), Nothing, row("FullyPaidAccountAmount")),
        .ClientOutstandingAccountAmount = If(IsDBNull(row("ClientOutstandingAccountAmount")), Nothing, row("ClientOutstandingAccountAmount")),
        .AccountAmount = If(IsDBNull(row("AccountAmount")), Nothing, row("AccountAmount")),
        .MarkedAccountAmount = If(IsDBNull(row("MarkedAccountAmount")), Nothing, row("MarkedAccountAmount")),
        .PaidAccountAmount = If(IsDBNull(row("PaidAccountAmount")), Nothing, row("PaidAccountAmount")),
        .AlternateReference = If(IsDBNull(row("AlternateReference")), Nothing, row("AlternateReference")),
        .EffectiveDate = If(IsDBNull(row("EffectiveDate")), Nothing, row("EffectiveDate")),
        .BranchCode = If(IsDBNull(row("BranchCode")), Nothing, row("BranchCode")),
        .DueDate = If(IsDBNull(row("DueDate")), Nothing, row("DueDate"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetBankAccounts(dtResultSet As DataTable) As List(Of BaseGetBankAccountsResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetBankAccountsResponseTypeRow With {
        .BankAccountKey = If(IsDBNull(row("BankAccountKey")), Nothing, row("BankAccountKey")),
        .Code = If(IsDBNull(row("Code")), Nothing, row("Code")),
        .BankAccountNumber = If(IsDBNull(row("BankAccountNumber")), Nothing, row("BankAccountNumber")),
        .Description = If(IsDBNull(row("Description")), Nothing, row("Description")),
        .EffectiveDate = If(IsDBNull(row("EffectiveDate")), Nothing, row("EffectiveDate")),
        .IsDeleted = If(IsDBNull(row("IsDeleted")), Nothing, row("IsDeleted")),
        .CurrencyKey = If(IsDBNull(row("CurrencyKey")), Nothing, row("CurrencyKey")),
        .CurrencyCode = If(IsDBNull(row("CurrencyCode")), Nothing, row("CurrencyCode")),
        .BankAccountName = If(IsDBNull(row("BankAccountName")), Nothing, row("BankAccountName"))})
        Return myData.ToList()
    End Function
    Private Function DataTabletoList_GetTaxeTypesAndBands(dtResultSet As DataTable) As List(Of BaseGetTaxTypesAndBandsResponseRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetTaxTypesAndBandsResponseRow With {
        .TaxTypeId = If(IsDBNull(row("tax_type_id")), Nothing, row("tax_type_id")),
        .TaxTypeDescription = If(IsDBNull(row("description")), Nothing, row("description")),
        .TaxTypeCode = If(IsDBNull(row("code")), Nothing, row("code")),
        .TaxBandId = If(IsDBNull(row("tax_band_id")), Nothing, row("tax_band_id")),
        .TaxBandDescription = If(IsDBNull(row("description")), Nothing, row("description")),
        .IsValue = If(IsDBNull(row("is_value")), Nothing, row("is_value")),
        .Rate = If(IsDBNull(row("rate")), Nothing, row("rate")),
        .CurrencyId = If(IsDBNull(row("currency_id")), Nothing, row("currency_id")),
        .Sequence = If(IsDBNull(row("sequence")), Nothing, row("sequence")),
        .AllowTaxCredit = If(IsDBNull(row("allow_tax_credit")), Nothing, row("allow_tax_credit")),
        .TaxBandRateId = If(IsDBNull(row("tax_band_rate_id")), Nothing, row("tax_band_rate_id"))})
        Return myData.ToList()
    End Function

    ''' <summary>
    ''' Data table to List conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_CheckUnpaidPremium(dtResultSet As DataTable) As List(Of BaseCheckUnpaidPremiumResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseCheckUnpaidPremiumResponseTypeRow With {
        .short_code = If(IsDBNull(row("short_code")), Nothing, row("short_code")),
        .document_type = If(IsDBNull(row("document_type")), Nothing, row("document_type")),
        .document_ref = If(IsDBNull(row("document_ref")), Nothing, row("document_ref")),
        .document_date = If(IsDBNull(row("document_date")), Nothing, row("document_date")),
        .amount = If(IsDBNull(row("amount")), Nothing, row("amount")),
        .outstanding = If(IsDBNull(row("outstanding")), Nothing, row("outstanding")),
        .BranchCode = If(IsDBNull(row("BranchCode")), Nothing, row("BranchCode")),
        .BranchDescription = If(IsDBNull(row("BranchDescription")), Nothing, row("BranchDescription"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_FindClaim(dtResultSet As DataTable) As List(Of BaseFindClaimResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseFindClaimResponseTypeRow With {
               .InsuranceFileKey = If(IsDBNull(row("InsuranceFileKey")), Nothing, row("InsuranceFileKey")),
               .ClaimKey = If(IsDBNull(row("ClaimKey")), Nothing, row("ClaimKey")),
               .ClaimDescription = If(IsDBNull(row("ClaimDescription")), Nothing, row("ClaimDescription")),
               .ClaimNumber = If(IsDBNull(row("ClaimNumber")), Nothing, row("ClaimNumber")),
               .InsuranceRef = If(IsDBNull(row("InsuranceRef")), Nothing, row("InsuranceRef")),
               .ClientShortName = If(IsDBNull(row("ClientShortName")), Nothing, row("ClientShortName")),
               .ProductDescription = If(IsDBNull(row("ProductDescription")), Nothing, row("ProductDescription")),
               .LossDateFrom = If(IsDBNull(row("LossDateFrom")), Nothing, row("LossDateFrom")),
               .ClientName = If(IsDBNull(row("ClientName")), Nothing, row("ClientName")),
               .ClaimStatusID = If(IsDBNull(row("ClaimStatusID")), Nothing, row("ClaimStatusID")),
               .ClaimHandlerDescription = If(IsDBNull(row("ClaimHandlerDescription")), Nothing, row("ClaimHandlerDescription")),
               .InsurerClaimNumber = If(IsDBNull(row("InsurerClaimNumber")), Nothing, row("InsurerClaimNumber")),
               .ClientClaimNumber = If(IsDBNull(row("ClientClaimNumber")), Nothing, row("ClientClaimNumber")),
               .ClientTelephoneNumber = If(IsDBNull(row("ClientTelephoneNumber")), Nothing, row("ClientTelephoneNumber")),
               .ClientTelephoneNumberOffice = If(IsDBNull(row("ClientTelephoneNumberOffice")), Nothing, row("ClientTelephoneNumberOffice")),
               .PrimaryCauseDescription = If(IsDBNull(row("PrimaryCauseDescription")), Nothing, row("PrimaryCauseDescription")),
               .SecondaryCauseDescription = If(IsDBNull(row("SecondaryCauseDescription")), Nothing, row("SecondaryCauseDescription")),
               .ProgressStatusDescription = If(IsDBNull(row("ProgressStatusDescription")), Nothing, row("ProgressStatusDescription")),
               .Payments = If(IsDBNull(row("Payments")), Nothing, row("Payments")),
               .Reserve = If(IsDBNull(row("Reserve")), Nothing, row("Reserve")),
               .CurrencyISOCode = If(IsDBNull(row("CurrencyISOCode")), Nothing, row("CurrencyISOCode")),
               .IsDeleted = If(IsDBNull(row("IsDeleted")), Nothing, row("IsDeleted")),
               .IsAllowedClosedClaims = If(IsDBNull(row("IsAllowedClosedClaims")), Nothing, row("IsAllowedClosedClaims")),
               .RiskKey = If(dtResultSet.Columns.Contains("RiskKey"), If(IsDBNull(row("RiskKey")), Nothing, row("RiskKey")), Nothing),
               .RiskDescription = If(dtResultSet.Columns.Contains("RiskDescription"), If(IsDBNull(row("RiskDescription")), Nothing, row("RiskDescription")), Nothing),
               .ReportedDate = If(IsDBNull(row("ReportedDate")), Nothing, row("ReportedDate")),
               .LastModifiedDate = If(IsDBNull(row("LastModifiedDate")), Nothing, row("LastModifiedDate")),
               .BaseClaimKey = If(IsDBNull(row("BaseClaimKey")), Nothing, row("BaseClaimKey")),
               .CoverFrom = If(IsDBNull(row("CoverFrom")), Nothing, row("CoverFrom")),
               .CoverTo = If(IsDBNull(row("CoverTo")), Nothing, row("CoverTo")),
               .LeadAgentName = If(IsDBNull(row("LeadAgentName")), Nothing, row("LeadAgentName")),
               .NotificationDate = If(dtResultSet.Columns.Contains("NotificationDate"), If(IsDBNull(row("NotificationDate")), Nothing, row("NotificationDate")), Nothing),
               .CatastropheCode = If(dtResultSet.Columns.Contains("CatastropheCode"), If(IsDBNull(row("CatastropheCode")), Nothing, row("CatastropheCode")), Nothing),
               .CaseNumber = If(IsDBNull(row("CaseNumber")), Nothing, row("CaseNumber")),
               .ClaimStatus = If(IsDBNull(row("ClaimStatus")), Nothing, row("ClaimStatus")),
               .SearchResultsCol1 = If(IsDBNull(row("SearchResultsCol1")), Nothing, row("SearchResultsCol1")),
               .AssociatedClients = If(IsDBNull(row("AssociatedClients")), Nothing, row("AssociatedClients")),
               .ClaimRiskField = If(IsDBNull(row("ClaimRiskField")), Nothing, row("ClaimRiskField"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_FindInsuranceFile(dtResultSet As DataTable) As List(Of BaseFindInsuranceFileResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseFindInsuranceFileResponseTypeRow With {
        .LapseDate = If(IsDBNull(row("LapseDate")), Nothing, row("LapseDate")),
        .InsuranceFileTypeCode = If(IsDBNull(row("InsuranceFileTypeCode")), Nothing, row("InsuranceFileTypeCode")),
        .InceptionDate = If(IsDBNull(row("InceptionDate")), Nothing, row("InceptionDate")),
        .LeadAgentKey = If(IsDBNull(row("LeadAgentKey")), Nothing, row("LeadAgentKey")),
        .InsuranceFileKey = If(IsDBNull(row("InsuranceFileKey")), Nothing, row("InsuranceFileKey")),
        .StatusId = If(IsDBNull(row("StatusId")), Nothing, row("StatusId")),
        .IsSourceClosed = If(IsDBNull(row("IsSourceClosed")), Nothing, row("IsSourceClosed")),
        .InsuranceRef = If(IsDBNull(row("InsuranceRef")), Nothing, row("InsuranceRef")),
        .ProductCode = If(IsDBNull(row("ProductCode")), Nothing, row("ProductCode")),
        .RenewalDate = If(IsDBNull(row("RenewalDate")), Nothing, row("RenewalDate")),
        .RiskIndex = If(IsDBNull(row("RiskIndex")), Nothing, row("RiskIndex")),
        .ClientShortName = If(IsDBNull(row("ClientShortName")), Nothing, row("ClientShortName")),
        .ClientName = If(IsDBNull(row("ClientName")), Nothing, row("ClientName")),
        .ResolvedName = If(IsDBNull(row("ResolvedName")), Nothing, row("ResolvedName")),
        .ClientAddressLine1 = If(IsDBNull(row("ClientAddressLine1")), Nothing, row("ClientAddressLine1")),
        .ClientPostCode = If(IsDBNull(row("ClientPostCode")), Nothing, row("ClientPostCode")),
        .InsuranceFileStatusCode = If(IsDBNull(row("InsuranceFileStatusCode")), Nothing, row("InsuranceFileStatusCode")),
        .CoverFrom = If(IsDBNull(row("CoverFrom")), Nothing, row("CoverFrom")),
        .CoverTo = If(IsDBNull(row("CoverTo")), Nothing, row("CoverTo")),
        .LeadAgentName = If(IsDBNull(row("LeadAgentName")), Nothing, row("LeadAgentName")),
        .AllowedClosedBranchClaims = If(IsDBNull(row("AllowedCLosedBranchClaims")), Nothing, row("AllowedCLosedBranchClaims")),
        .AssociatedClients = If(IsDBNull(row("AssociatedClients")), Nothing, row("AssociatedClients")),
        .FileCode = If(IsDBNull(row("FileCode")), Nothing, row("FileCode")),
        .DOB = If(IsDBNull(row("DOBirth")), Nothing, row("DOBirth"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetClaimCoinsurer(dtResultSet As DataTable) As List(Of BaseGetClaimCoinsurerResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetClaimCoinsurerResponseTypeRow With {
        .PartyKey = If(IsDBNull(row("PartyKey")), Nothing, row("PartyKey")),
        .Name = If(IsDBNull(row("Name")), Nothing, row("Name")),
        .Share = If(IsDBNull(row("Share")), Nothing, row("Share")),
        .ShareValue = If(IsDBNull(row("ShareValue")), Nothing, row("ShareValue"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetClaimPartyDetails(dtResultSet As DataTable) As List(Of BaseGetClaimPartyDetailsResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetClaimPartyDetailsResponseTypeRow With {
        .ResolvedName = If(IsDBNull(row("ResolvedName")), Nothing, row("ResolvedName")),
        .ShortName = If(IsDBNull(row("ShortName")), Nothing, row("ShortName")),
        .Address1 = If(IsDBNull(row("Address1")), Nothing, row("Address1")),
        .Address2 = If(IsDBNull(row("Address2")), Nothing, row("Address2")),
        .Address3 = If(IsDBNull(row("Address3")), Nothing, row("Address3")),
        .Address4 = If(IsDBNull(row("Address4")), Nothing, row("Address4")),
        .PostalCode = If(IsDBNull(row("PostalCode")), Nothing, row("PostalCode")),
        .TelHome = If(IsDBNull(row("TelHome")), Nothing, row("TelHome")),
        .TelOff = If(IsDBNull(row("TelOff")), Nothing, row("TelOff")),
        .Fax = If(IsDBNull(row("Fax")), Nothing, row("Fax")),
        .Mobile = If(IsDBNull(row("Mobile")), Nothing, row("Mobile")),
        .EMail = If(IsDBNull(row("EMail")), Nothing, row("EMail")),
        .PartyKey = If(IsDBNull(row("PartyKey")), Nothing, row("PartyKey")),
        .CountryKey = If(IsDBNull(row("CountryKey")), Nothing, row("CountryKey")),
        .AddressKey = If(IsDBNull(row("AddressKey")), Nothing, row("AddressKey"))})
        Return myData.ToList()
    End Function

    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetClaimPerilSummary1(dtResultSet As DataTable) As List(Of BaseGetClaimPerilSummaryResponseTypeRow1)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetClaimPerilSummaryResponseTypeRow1 With {
        .Description = If(IsDBNull(row("Description")), Nothing, row("Description")),
        .InitialRecovery = If(IsDBNull(row("InitialRecovery")), Nothing, row("InitialRecovery")),
        .ReceiptedAmount = If(IsDBNull(row("ReceiptedAmount")), Nothing, row("ReceiptedAmount")),
        .RevisedRecovery = If(IsDBNull(row("RevisedRecovery")), Nothing, row("RevisedRecovery")),
        .CurrentRecovery = If(IsDBNull(row("CurrentRecovery")), Nothing, row("CurrentRecovery"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetClaimPerilSummary2(dtResultSet As DataTable) As List(Of BaseGetClaimPerilSummaryResponseTypeRow2)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetClaimPerilSummaryResponseTypeRow2 With {
        .Description = If(IsDBNull(row("Description")), Nothing, row("Description")),
        .InitialRecovery = If(IsDBNull(row("InitialRecovery")), Nothing, row("InitialRecovery")),
        .ReceiptedAmount = If(IsDBNull(row("ReceiptedAmount")), Nothing, row("ReceiptedAmount")),
        .RevisedRecovery = If(IsDBNull(row("RevisedRecovery")), Nothing, row("RevisedRecovery")),
        .CurrentRecovery = If(IsDBNull(row("CurrentRecovery")), Nothing, row("CurrentRecovery"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetClaimPaymentTaxGroups(dtResultSet As DataTable) As List(Of BaseGetClaimPaymentTaxGroupsResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetClaimPaymentTaxGroupsResponseTypeRow With {
        .Code = If(IsDBNull(row("Code")), Nothing, row("Code")),
        .Description = If(IsDBNull(row("Description")), Nothing, row("Description")),
        .IsWithholdingTax = If(IsDBNull(row("IsWithholdingTax")), Nothing, row("IsWithholdingTax"))})
        Return myData.ToList()
    End Function

    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetRecoveryCoinsuranceCoinsurances(dtResultSet As DataTable) As List(Of BaseGetRecoveryCoinsuranceResponseTypeCoinsurancesRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetRecoveryCoinsuranceResponseTypeCoinsurancesRow With {
        .RecoveryKey = If(IsDBNull(row("RecoveryKey")), Nothing, row("RecoveryKey")),
        .RecoveryType = If(IsDBNull(row("RecoveryType")), Nothing, row("RecoveryType")),
        .PartyKey = If(IsDBNull(row("PartyKey")), Nothing, row("PartyKey")),
        .Coinsurer = If(IsDBNull(row("Coinsurer")), Nothing, row("Coinsurer")),
        .SharePercent = If(IsDBNull(row("SharePercent")), Nothing, row("SharePercent")),
        .RecoveryToDate = If(IsDBNull(row("RecoveryToDate")), Nothing, row("RecoveryToDate"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetReferredPayments(dtResultSet As DataTable) As List(Of BaseGetReferredPaymentsResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetReferredPaymentsResponseTypeRow With {
        .ClaimPaymentKey = If(IsDBNull(row("ClaimPaymentKey")), Nothing, row("ClaimPaymentKey")),
        .ClaimKey = If(IsDBNull(row("ClaimKey")), Nothing, row("ClaimKey")),
        .ClaimNumber = If(IsDBNull(row("ClaimNumber")), Nothing, row("ClaimNumber")),
        .PolicyNumber = If(IsDBNull(row("PolicyNumber")), Nothing, row("PolicyNumber")),
        .ClientName = If(IsDBNull(row("ClientName")), Nothing, row("ClientName")),
        .PaymentAmount = If(IsDBNull(row("PaymentAmount")), Nothing, row("PaymentAmount")),
        .PaymentDate = If(IsDBNull(row("PaymentDate")), Nothing, row("PaymentDate")),
        .CreatedBy = If(IsDBNull(row("CreatedBy")), Nothing, row("CreatedBy")),
        .Status = If(IsDBNull(row("Status")), Nothing, row("Status")),
        .PaymentAmountBaseCurrency = If(IsDBNull(row("PaymentAmountBaseCurrency")), Nothing, row("PaymentAmountBaseCurrency")),
        .PaymentAmountBaseCurrencySpecified = True,
        .CaseNumber = If(IsDBNull(row("CaseNumber")), Nothing, row("CaseNumber")),
        .PayeeName = If(IsDBNull(row("PayeeName")), Nothing, row("PayeeName")),
        .IsReferredForRecommendation = If(IsDBNull(row("IsReferredForRecommendation")), Nothing, row("IsReferredForRecommendation")),
        .RecommendedBy = If(IsDBNull(row("RecommendedBy")), Nothing, row("RecommendedBy")),
        .CurrencyCode = If(IsDBNull(row("CurrencyCode")), Nothing, row("CurrencyCode")),
        .PayeeType = If(IsDBNull(row("PayeeType")), Nothing, row("PayeeType")),
        .CurrencyId = If(IsDBNull(row("CurrencyId")), Nothing, row("CurrencyId"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetTaxGroupsForClaims(dtResultSet As DataTable) As List(Of BaseGetTaxGroupsForClaimsResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetTaxGroupsForClaimsResponseTypeRow With {
        .TaxGroupKey = If(IsDBNull(row("TaxGroupKey")), Nothing, row("TaxGroupKey")),
        .Description = If(IsDBNull(row("Description")), Nothing, row("Description")),
        .Code = If(IsDBNull(row("Code")), Nothing, row("Code")),
        .IsWithHoldingTax = If(IsDBNull(row("IsWithHoldingTax")), Nothing, row("IsWithHoldingTax")),
        .AdvanceTaxScript = If(IsDBNull(row("AdvanceTaxScript")), Nothing, row("AdvanceTaxScript")),
        .IsTaxAmountEditable = If(IsDBNull(row("IsTaxAmountEditable")), Nothing, row("IsTaxAmountEditable"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetUnallocatedClaimPayments(dtResultSet As DataTable) As List(Of BaseGetUnallocatedClaimPaymentsResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetUnallocatedClaimPaymentsResponseTypeRow With {
        .Status = If(IsDBNull(row("Status")), Nothing, row("Status")),
        .MediaTypeCode = If(IsDBNull(row("MediaTypeCode")), Nothing, row("MediaTypeCode")),
        .CurrencyCode = If(IsDBNull(row("CurrencyCode")), Nothing, row("CurrencyCode")),
        .BankAccountCode = If(IsDBNull(row("BankAccountCode")), Nothing, row("BankAccountCode")),
        .CashListItemKey = If(IsDBNull(row("CashListItemKey")), Nothing, row("CashListItemKey")),
        .AccountCode = If(IsDBNull(row("AccountCode")), Nothing, row("AccountCode")),
        .ClaimPaymentBranchCode = If(IsDBNull(row("ClaimPaymentBranchCode")), Nothing, row("ClaimPaymentBranchCode")),
        .PayeeName = If(IsDBNull(row("PayeeName")), Nothing, row("PayeeName")),
        .OurRef = If(IsDBNull(row("OurRef")), Nothing, row("OurRef")),
        .ClaimPaymentKey = If(IsDBNull(row("ClaimPaymentKey")), Nothing, row("ClaimPaymentKey")),
        .MediaTypeDesc = If(IsDBNull(row("MediaTypeDesc")), Nothing, row("MediaTypeDesc")),
        .DocumentKey = If(IsDBNull(row("DocumentKey")), Nothing, row("DocumentKey")),
        .DocumentRef = If(IsDBNull(row("DocumentRef")), Nothing, row("DocumentRef")),
        .CurrencyAmount = If(IsDBNull(row("CurrencyAmount")), Nothing, row("CurrencyAmount")),
        .CurrencyKey = If(IsDBNull(row("CurrencyKey")), Nothing, row("CurrencyKey")),
        .Amount = If(IsDBNull(row("Amount")), Nothing, row("Amount")),
        .AmountCurrencyKey = If(IsDBNull(row("AmountCurrencyKey")), Nothing, row("AmountCurrencyKey")),
        .AccountAmount = If(IsDBNull(row("AccountAmount")), Nothing, row("AccountAmount")),
        .AccountCurrencyKey = If(IsDBNull(row("AccountCurrencyKey")), Nothing, row("AccountCurrencyKey")),
        .ClaimNumber = If(IsDBNull(row("ClaimNumber")), Nothing, row("ClaimNumber")),
        .DocumentComment = If(IsDBNull(row("DocumentComment")), Nothing, row("DocumentComment")),
        .CurrencyDescription = If(IsDBNull(row("CurrencyDescription")), Nothing, row("CurrencyDescription")),
        .CurrencyFormatString = If(IsDBNull(row("CurrencyFormatString")), Nothing, row("CurrencyFormatString")),
        .DateOfPayment = If(IsDBNull(row("DateOfPayment")), Nothing, row("DateOfPayment")),
        .PayeeMediaTypeKey = If(IsDBNull(row("PayeeMediaTypeKey")), Nothing, row("PayeeMediaTypeKey")),
        .BaseCurrencyDescription = If(IsDBNull(row("BaseCurrencyDescription")), Nothing, row("BaseCurrencyDescription")),
        .BaseCurrencyFormatString = If(IsDBNull(row("BaseCurrencyFormatString")), Nothing, row("BaseCurrencyFormatString")),
        .DocumentDate = If(IsDBNull(row("DocumentDate")), Nothing, row("DocumentDate")),
        .AccountKey = If(IsDBNull(row("AccountKey")), Nothing, row("AccountKey")),
        .BaseClaimPaymentKey = If(IsDBNull(row("BaseClaimPaymentKey")), Nothing, row("BaseClaimPaymentKey")),
        .AccountName = If(IsDBNull(row("AccountName")), Nothing, row("AccountName")),
        .TheirRef = If(IsDBNull(row("TheirRef")), Nothing, row("TheirRef")),
        .PartyBankId = If(IsDBNull(row("PartyBankId")), Nothing, row("PartyBankId")),
        .PayeeAccountNo = If(IsDBNull(row("PayeeAccountNo")), Nothing, row("PayeeAccountNo")),
        .PayeeShortCode = If(IsDBNull(row("PayeeSortCode")), Nothing, row("PayeeSortCode")),
        .MediaRef = If(IsDBNull(row("MediaRef")), Nothing, row("MediaRef"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetValidPrimaryCauses(dtResultSet As DataTable) As List(Of BaseGetValidPrimaryCausesResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetValidPrimaryCausesResponseTypeRow With {
        .code = If(IsDBNull(row("code")), Nothing, row("code")),
        .description = If(IsDBNull(row("description")), Nothing, row("description")),
        .primary_cause_id = If(IsDBNull(row("primary_cause_id")), Nothing, row("primary_cause_id"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetVersionsForClaim(dtResultSet As DataTable) As List(Of BaseGetVersionsForClaimResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetVersionsForClaimResponseTypeRow With {
        .ClaimKey = If(IsDBNull(row("ClaimKey")), Nothing, row("ClaimKey")),
        .Version = If(IsDBNull(row("Version")), Nothing, row("Version")),
        .TransactionDate = If(IsDBNull(row("TransactionDate")), Nothing, row("TransactionDate")),
        .TransactionType = If(IsDBNull(row("TransactionType")), Nothing, row("TransactionType")),
        .VersionDescription = If(IsDBNull(row("VersionDescription")), Nothing, row("VersionDescription")),
        .TotalIncurred = If(IsDBNull(row("TotalIncurred")), Nothing, row("TotalIncurred")),
        .TotalPaid = If(IsDBNull(row("TotalPaid")), Nothing, row("TotalPaid")),
        .ThisRevision = If(IsDBNull(row("ThisRevision")), Nothing, row("ThisRevision")),
        .ThisPayment = If(IsDBNull(row("ThisPayment")), Nothing, row("ThisPayment")),
        .ThisSalvageRecovery = If(IsDBNull(row("ThisSalvageRecovery")), Nothing, row("ThisSalvageRecovery")),
        .ThisThirdPartyRecovery = If(IsDBNull(row("ThisThirdPartyRecovery")), Nothing, row("ThisThirdPartyRecovery")),
        .CurrentReserve = If(IsDBNull(row("CurrentReserve")), Nothing, row("CurrentReserve")),
        .PolicyCurrency = If(IsDBNull(row("PolicyCurrency")), Nothing, row("PolicyCurrency")),
        .LossCurrency = If(IsDBNull(row("LossCurrency")), Nothing, row("LossCurrency")),
        .User = If(IsDBNull(row("User")), Nothing, row("User")),
        .ClaimDescription = If(IsDBNull(row("ClaimDescription")), Nothing, row("ClaimDescription")),
        .InsuranceRef = If(IsDBNull(row("InsuranceRef")), Nothing, row("InsuranceRef")),
        .InsuranceFileKey = If(IsDBNull(row("InsuranceFileKey")), Nothing, row("InsuranceFileKey")),
        .claim_number = If(IsDBNull(row("claim_number")), Nothing, row("claim_number")),
        .RiskKey = If(IsDBNull(row("RiskKey")), Nothing, row("RiskKey")),
        .client_short_name = If(IsDBNull(row("client_short_name")), Nothing, row("client_short_name")),
        .loss_from_date = If(IsDBNull(row("loss_from_date")), Nothing, row("loss_from_date")),
        .InsuranceHolderShortName = If(IsDBNull(row("InsuranceHolderShortName")), Nothing, row("InsuranceHolderShortName")),
        .InsuranceFolderKey = If(IsDBNull(row("InsuranceFolderKey")), Nothing, row("InsuranceFolderKey"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_FindCaseCaseDetails(dtResultSet As DataTable) As List(Of BaseFindCaseResponseTypeCaseDetailsRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseFindCaseResponseTypeCaseDetailsRow With {
        .CaseKey = If(IsDBNull(row("CaseKey")), Nothing, row("CaseKey")),
        .CaseNumber = If(IsDBNull(row("CaseNumber")), Nothing, row("CaseNumber")),
        .CaseOpenDate = If(IsDBNull(row("CaseOpenDate")), Nothing, row("CaseOpenDate")),
        .Analyst = If(IsDBNull(row("Analyst")), Nothing, row("Analyst")),
        .Assistant = If(IsDBNull(row("Assistant")), Nothing, row("Assistant")),
        .CaseProgressDescription = If(IsDBNull(row("CaseProgressDescription")), Nothing, row("CaseProgressDescription")),
        .TotalIndemnity = If(IsDBNull(row("TotalIndemnity")), Nothing, row("TotalIndemnity")),
        .TotalExpense = If(IsDBNull(row("TotalExpense")), Nothing, row("TotalExpense")),
        .TotalExcess = If(IsDBNull(row("TotalExcess")), Nothing, row("TotalExcess")),
        .BaseCaseKey = If(IsDBNull(row("BaseCaseKey")), Nothing, row("BaseCaseKey")),
        .CurrencyCode = If(IsDBNull(row("CurrencyCode")), Nothing, row("CurrencyCode"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetClaimReinsuranceArrangementLines(dtResultSet As DataTable) As List(Of BaseGetClaimReinsuranceArrangementLinesResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetClaimReinsuranceArrangementLinesResponseTypeRow With {
        .Name = If(IsDBNull(row("Name")), Nothing, row("Name")),
        .DefaultPerc = If(IsDBNull(row("DefaultPerc")), Nothing, row("DefaultPerc")),
        .ThisPerc = If(IsDBNull(row("ThisPerc")), Nothing, row("ThisPerc")),
        .SumInsured = If(IsDBNull(row("SumInsured")), Nothing, row("SumInsured")),
        .ReserveToDate = If(IsDBNull(row("ReserveToDate")), Nothing, row("ReserveToDate")),
        .ThisReserve = If(IsDBNull(row("ThisReserve")), Nothing, row("ThisReserve")),
        .PaymentToDate = If(IsDBNull(row("PaymentToDate")), Nothing, row("PaymentToDate")),
        .ThisPayment = If(IsDBNull(row("ThisPayment")), Nothing, row("ThisPayment")),
        .Balance = If(IsDBNull(row("Balance")), Nothing, row("Balance")),
        .Agreement = If(IsDBNull(row("Agreement")), Nothing, row("Agreement")),
        .IsObligatory = If(IsDBNull(row("IsObligatory")), Nothing, row("IsObligatory")),
        .Type = If(IsDBNull(row("LineType")), Nothing, row("LineType"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetClaimReinsuranceArrangements(dtResultSet As DataTable) As List(Of BaseGetClaimReinsuranceArrangementsResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetClaimReinsuranceArrangementsResponseTypeRow With {
        .ArrangementId = If(IsDBNull(row("ArrangementId")), Nothing, row("ArrangementId")),
        .BandId = If(IsDBNull(row("BandId")), Nothing, row("BandId")),
        .SumInsured = If(IsDBNull(row("SumInsured")), Nothing, row("SumInsured")),
        .ReserveToDate = If(IsDBNull(row("ReserveToDate")), Nothing, row("ReserveToDate")),
        .ThisReserve = If(IsDBNull(row("ThisReserve")), Nothing, row("ThisReserve")),
        .PaymentToDate = If(IsDBNull(row("PaymentToDate")), Nothing, row("PaymentToDate")),
        .ThisPayment = If(IsDBNull(row("ThisPayment")), Nothing, row("ThisPayment")),
        .Balance = If(IsDBNull(row("Balance")), Nothing, row("Balance")),
        .RecoveryToDate = If(IsDBNull(row("RecoveryToDate")), Nothing, row("RecoveryToDate"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetClaimReinsuranceBands(dtResultSet As DataTable) As List(Of BaseGetClaimReinsuranceBandsResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetClaimReinsuranceBandsResponseTypeRow With {
        .BandId = If(IsDBNull(row("BandId")), Nothing, row("BandId")),
        .Band = If(IsDBNull(row("Band")), Nothing, row("Band"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetRecoveryReinsurance(dtResultSet As DataTable) As List(Of BaseGetRecoveryReinsuranceResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetRecoveryReinsuranceResponseTypeRow With {
        .RecoveryKey = If(IsDBNull(row("RecoveryKey")), Nothing, row("RecoveryKey")),
        .RecoveryType = If(IsDBNull(row("RecoveryType")), Nothing, row("RecoveryType")),
        .PartyKey = If(IsDBNull(row("PartyKey")), Nothing, row("PartyKey")),
        .Reinsurer = If(IsDBNull(row("Reinsurer")), Nothing, row("Reinsurer")),
        .SharePercent = If(IsDBNull(row("SharePercent")), Nothing, row("SharePercent")),
        .RecoveryToDate = If(IsDBNull(row("RecoveryToDate")), Nothing, row("RecoveryToDate")),
        .Salvage = If(IsDBNull(row("Salvage")), Nothing, row("Salvage")),
        .ThisSalvage = If(IsDBNull(row("ThisSalvage")), Nothing, row("ThisSalvage")),
        .Recovery = If(IsDBNull(row("Recovery")), Nothing, row("Recovery")),
        .ThisRecovery = If(IsDBNull(row("ThisRecovery")), Nothing, row("ThisRecovery"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_AddressLookupType(dtResultSet As DataTable) As List(Of BaseAddressLookupType)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseAddressLookupType With {
        .AddressLine1 = If(IsDBNull(row("AddressLine1")), Nothing, row("AddressLine1")),
        .AddressLine2 = If(IsDBNull(row("AddressLine2")), Nothing, row("AddressLine2")),
        .AddressLine3 = If(IsDBNull(row("AddressLine3")), Nothing, row("AddressLine3")),
        .AddressLine4 = If(IsDBNull(row("AddressLine4")), Nothing, row("AddressLine4")),
        .CountryCode = If(IsDBNull(row("CountryCode")), Nothing, row("CountryCode")),
        .PostCode = If(IsDBNull(row("PostCode")), Nothing, row("PostCode"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_FindBank(dtResultSet As DataTable) As List(Of BaseFindBankResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseFindBankResponseTypeRow With {
        .BankKey = If(IsDBNull(row("BankKey")), Nothing, row("BankKey")),
        .BankName = If(IsDBNull(row("BankName")), Nothing, row("BankName")),
        .Code = If(IsDBNull(row("Code")), Nothing, row("Code")),
        .BarnchCode = If(IsDBNull(row("BarnchCode")), Nothing, row("BarnchCode")),
        .HeadOffice = If(IsDBNull(row("HeadOffice")), Nothing, row("HeadOffice")),
        .BankAddress = If(IsDBNull(row("BankAddress")), Nothing, row("BankAddress"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_FindBankGuaranteeBankGuarantee(dtResultSet As DataTable) As List(Of BaseFindBankGuaranteeResponseTypeBankGuaranteeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseFindBankGuaranteeResponseTypeBankGuaranteeRow With {
        .BGKey = If(IsDBNull(row("BGKey")), Nothing, row("BGKey")),
        .BankNameKey = If(IsDBNull(row("BankNameKey")), Nothing, row("BankNameKey")),
        .BankName = If(IsDBNull(row("BankName")), Nothing, row("BankName")),
        .BankGuaranteeRef = If(IsDBNull(row("BankGuaranteeRef")), Nothing, row("BankGuaranteeRef")),
        .BGLimit = If(IsDBNull(row("BGLimit")), Nothing, row("BGLimit")),
        .AvailableBalance = If(IsDBNull(row("AvailableBalance")), Nothing, row("AvailableBalance")),
        .ExpiryDate = If(IsDBNull(row("ExpiryDate")), Nothing, row("ExpiryDate")),
        .PartyKey = If(IsDBNull(row("PartyKey")), Nothing, row("PartyKey")),
        .ClientResolvedName = If(IsDBNull(row("ClientResolvedName")), Nothing, row("ClientResolvedName")),
        .Branches = If(IsDBNull(row("Branches")), Nothing, row("Branches")),
        .Products = If(IsDBNull(row("Products")), Nothing, row("Products")),
        .StatusDescription = If(IsDBNull(row("StatusDescription")), Nothing, row("StatusDescription")),
        .ClientShortName = If(IsDBNull(row("ClientShortName")), Nothing, row("ClientShortName"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_FindCoverNoteBooks(dtResultSet As DataTable) As List(Of BaseFindCoverNoteBooksResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseFindCoverNoteBooksResponseTypeRow With {
        .CoverNoteBookKey = If(IsDBNull(row("CoverNoteBookKey")), Nothing, row("CoverNoteBookKey")),
        .BookNumber = If(IsDBNull(row("BookNumber")), Nothing, row("BookNumber")),
        .StartNumber = If(IsDBNull(row("StartNumber")), Nothing, row("StartNumber")),
        .EndNumber = If(IsDBNull(row("EndNumber")), Nothing, row("EndNumber")),
        .AgentKey = If(IsDBNull(row("AgentKey")), Nothing, row("AgentKey")),
        .AgentName = If(IsDBNull(row("AgentName")), Nothing, row("AgentName")),
        .CoverNoteStatusKey = If(IsDBNull(row("CoverNoteStatusKey")), Nothing, row("CoverNoteStatusKey")),
        .CoverNoteStatusDescription = If(IsDBNull(row("CoverNoteStatusDescription")), Nothing, row("CoverNoteStatusDescription")),
        .CoverNoteBranchKey = If(IsDBNull(row("CoverNoteBranchKey")), Nothing, row("CoverNoteBranchKey")),
        .CoverNoteBranchDescription = If(IsDBNull(row("CoverNoteBranchDescription")), Nothing, row("CoverNoteBranchDescription")),
        .LastUpdated = If(IsDBNull(row("LastUpdated")), Nothing, row("LastUpdated")),
        .DateCreated = If(IsDBNull(row("DateCreated")), Nothing, row("DateCreated")),
        .EffectiveDate = If(IsDBNull(row("EffectiveDate")), Nothing, row("EffectiveDate"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_FindDocumentTemplates(dtResultSet As DataTable) As List(Of BaseFindDocumentTemplatesResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseFindDocumentTemplatesResponseTypeRow With {
        .DocumentTemplateKey = If(IsDBNull(row("DocumentTemplateKey")), Nothing, row("DocumentTemplateKey")),
        .Code = If(IsDBNull(row("Code")), Nothing, row("Code")),
        .Description = If(IsDBNull(row("Description")), Nothing, row("Description")),
        .Type = If(IsDBNull(row("Type")), Nothing, row("Type")),
        .EffectiveDate = If(IsDBNull(row("EffectiveDate")), Nothing, row("EffectiveDate"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_FindUsers(dtResultSet As DataTable) As List(Of BaseFindUsersResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseFindUsersResponseTypeRow With {
        .UserId = If(IsDBNull(row("UserId")), Nothing, row("UserId")),
        .UserName = If(IsDBNull(row("UserName")), Nothing, row("UserName")),
        .FullName = If(IsDBNull(row("FullName")), Nothing, row("FullName")),
        .EffectiveDate = If(IsDBNull(row("EffectiveDate")), Nothing, row("EffectiveDate"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetCurrenciesByBranch(dtResultSet As DataTable) As List(Of BaseGetCurrenciesByBranchResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetCurrenciesByBranchResponseTypeRow With {
        .CurrencyCode = If(IsDBNull(row("CurrencyCode")), Nothing, row("CurrencyCode")),
        .Description = If(IsDBNull(row("Description")), Nothing, row("Description"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetList(dtResultSet As DataTable) As List(Of BaseGetListResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetListResponseTypeRow With {
        .Key = row("Key"),
        .Description = If(IsDBNull(row("Description")), Nothing, row("Description")),
        .Code = row("Code"),
        .EffectiveDate = If(IsDBNull(row("EffectiveDate")), Nothing, row("EffectiveDate")),
        .IsDeleted = If(IsDBNull(row("IsDeleted")), Nothing, row("IsDeleted")),
        .ParentKey = If(IsDBNull(row("ParentKey")), Nothing, row("ParentKey"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetProductByAgent(dtResultSet As DataTable) As List(Of BaseGetProductByAgentResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetProductByAgentResponseTypeRow With {
        .ProductKey = If(IsDBNull(row("ProductKey")), Nothing, row("ProductKey")),
        .ProductCode = If(IsDBNull(row("ProductCode")), Nothing, row("ProductCode")),
        .Description = If(IsDBNull(row("Description")), Nothing, row("Description")),
        .SchemeAgencyRef = If(IsDBNull(row("SchemeAgencyRef")), Nothing, row("SchemeAgencyRef")),
        .BlockNumber = If(IsDBNull(row("BlockNumber")), Nothing, row("BlockNumber")),
        .ConsolidatedLeadAgentCommission = If(IsDBNull(row("ConsolidatedLeadAgentCommission")), Nothing, row("ConsolidatedLeadAgentCommission")),
        .ConsolidatedSubAgentCommission = If(IsDBNull(row("ConsolidatedSubAgentCommission")), Nothing, row("ConsolidatedSubAgentCommission"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetRatingSectionByRiskType(dtResultSet As DataTable) As List(Of BaseGetRatingSectionByRiskTypeResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetRatingSectionByRiskTypeResponseTypeRow With {
        .RatingSectionId = If(IsDBNull(row("RatingSectionId")), Nothing, row("RatingSectionId")),
        .RatingSectionCode = If(IsDBNull(row("RatingSectionCode")), Nothing, row("RatingSectionCode")),
        .Description = If(IsDBNull(row("Description")), Nothing, row("Description")),
        .RateTypeID = If(IsDBNull(row("rate_type_id")), Nothing, row("rate_type_id")),
        .Rate = If(IsDBNull(row("Rate")), Nothing, row("Rate")),
        .CurrencyID = If(IsDBNull(row("currency_id")), Nothing, row("currency_id")),
        .CountryID = If(IsDBNull(row("country_id")), Nothing, row("country_id")),
        .StateID = If(IsDBNull(row("state_id")), Nothing, row("state_id")),
        .EarningPatternID = If(IsDBNull(row("Earning_Pattern_id")), Nothing, row("Earning_Pattern_id"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetTaskGroupTasks(dtResultSet As DataTable) As List(Of BaseGetTaskGroupTasksResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetTaskGroupTasksResponseTypeRow With {
        .TaskKey = If(IsDBNull(row("TaskKey")), Nothing, row("TaskKey")),
        .Name = If(IsDBNull(row("Name")), Nothing, row("Name")),
        .EffectiveDate = If(IsDBNull(row("EffectiveDate")), Nothing, row("EffectiveDate")),
        .Description = If(IsDBNull(row("Description")), Nothing, row("Description")),
        .IsDeleted = If(IsDBNull(row("IsDeleted")), Nothing, row("IsDeleted")),
        .IsIncluded = If(IsDBNull(row("IsIncluded")), Nothing, row("IsIncluded")),
        .IsViewOnly = If(IsDBNull(row("IsViewOnly")), Nothing, row("IsViewOnly")),
        .IsAvailable = If(IsDBNull(row("IsAvailable")), Nothing, row("IsAvailable")),
        .TaskCategoryKey = If(IsDBNull(row("TaskCategoryKey")), Nothing, row("TaskCategoryKey")),
        .DisplayIcon = If(IsDBNull(row("DisplayIcon")), Nothing, row("DisplayIcon"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetTaskGroups(dtResultSet As DataTable) As List(Of BaseGetTaskGroupsResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetTaskGroupsResponseTypeRow With {
        .TaskGroupKey = If(IsDBNull(row("TaskGroupKey")), Nothing, row("TaskGroupKey")),
        .Code = If(IsDBNull(row("Code")), Nothing, row("Code")),
        .Description = If(IsDBNull(row("Description")), Nothing, row("Description")),
        .IsDeleted = If(IsDBNull(row("IsDeleted")), Nothing, row("IsDeleted")),
        .TaskGroupCategoryKey = If(IsDBNull(row("TaskGroupCategoryKey")), Nothing, row("TaskGroupCategoryKey")),
        .EffectiveDate = If(IsDBNull(row("EffectiveDate")), Nothing, row("EffectiveDate")),
        .CaptionID = If(IsDBNull(row("CaptionID")), Nothing, row("CaptionID"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetUserDetails(dtResultSet As DataTable) As List(Of BaseGetUserDetailsResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetUserDetailsResponseTypeRow With {
        .IsAssociated = If(IsDBNull(row("IsAssociated")), Nothing, row("IsAssociated")),
        .UserGroupKey = If(IsDBNull(row("UserGroupKey")), Nothing, row("UserGroupKey")),
        .Code = If(IsDBNull(row("Code")), Nothing, row("Code")),
        .Description = If(IsDBNull(row("Description")), Nothing, row("Description")),
        .IsSupervisor = If(IsDBNull(row("IsSupervisor")), Nothing, row("IsSupervisor")),
        .IsSystemAdmin = If(IsDBNull(row("IsSystemAdmin")), Nothing, row("IsSystemAdmin"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetUserGroupTaskGroups(dtResultSet As DataTable) As List(Of BaseGetUserGroupTaskGroupsResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetUserGroupTaskGroupsResponseTypeRow With {
        .TaskGroupKey = If(IsDBNull(row("TaskGroupKey")), Nothing, row("TaskGroupKey")),
        .Code = If(IsDBNull(row("Code")), Nothing, row("Code")),
        .EffectiveDate = If(IsDBNull(row("EffectiveDate")), Nothing, row("EffectiveDate")),
        .Description = If(IsDBNull(row("Description")), Nothing, row("Description")),
        .IsDeleted = If(IsDBNull(row("IsDeleted")), Nothing, row("IsDeleted")),
        .IsIncluded = If(IsDBNull(row("IsIncluded")), Nothing, row("IsIncluded")),
        .DisplaySequence = If(IsDBNull(row("DisplaySequence")), Nothing, row("DisplaySequence"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetUserGroupUsers(dtResultSet As DataTable) As List(Of BaseGetUserGroupUsersResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetUserGroupUsersResponseTypeRow With {
        .UserKey = If(IsDBNull(row("UserKey")), Nothing, row("UserKey")),
        .Name = If(IsDBNull(row("Name")), Nothing, row("Name")),
        .EmailAddress = If(IsDBNull(row("EmailAddress")), Nothing, row("EmailAddress"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetUserGroups(dtResultSet As DataTable) As List(Of BaseGetUserGroupsResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetUserGroupsResponseTypeRow With {
        .UserGroupKey = If(IsDBNull(row("UserGroupKey")), Nothing, row("UserGroupKey")),
        .Code = If(IsDBNull(row("Code")), Nothing, row("Code")),
        .Description = If(IsDBNull(row("Description")), Nothing, row("Description")),
        .IsDeleted = If(IsDBNull(row("IsDeleted")), Nothing, row("IsDeleted")),
        .IsSystemAdmin = If(IsDBNull(row("IsSystemAdmin")), Nothing, row("IsSystemAdmin")),
        .EffectiveDate = If(IsDBNull(row("EffectiveDate")), Nothing, row("EffectiveDate")),
        .IsDebtorPMUserGroup = If(IsDBNull(row("IsDebtorPmuserGroup")), Nothing, row("IsDebtorPmuserGroup"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetUserGroupsbyTask(dtResultSet As DataTable) As List(Of BaseGetUserGroupsbyTaskResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetUserGroupsbyTaskResponseTypeRow With {
        .UserGroupKey = If(IsDBNull(row("UserGroupKey")), Nothing, row("UserGroupKey")),
        .UserGroupCode = If(IsDBNull(row("UserGroupCode")), Nothing, row("UserGroupCode")),
        .UserGroupDescription = If(IsDBNull(row("UserGroupDescription")), Nothing, row("UserGroupDescription"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetWmTask(dtResultSet As DataTable) As List(Of BaseGetWmTaskResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetWmTaskResponseTypeRow With {
        .KeyName = If(IsDBNull(row("KeyName")), Nothing, row("KeyName")),
        .KeyValue = If(IsDBNull(row("KeyValue")), Nothing, row("KeyValue"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetWmTaskLogTaskLog(dtResultSet As DataTable) As List(Of BaseGetWmTaskLogResponseTypeTaskLogRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetWmTaskLogResponseTypeTaskLogRow With {
        .TaskInstanceKey = If(IsDBNull(row("TaskInstanceKey")), Nothing, row("TaskInstanceKey")),
        .DateCreated = If(IsDBNull(row("DateCreated")), Nothing, row("DateCreated")),
        .LogText = If(IsDBNull(row("LogText")), Nothing, row("LogText")),
        .CreatedByKey = If(IsDBNull(row("CreatedByKey")), Nothing, row("CreatedByKey")),
        .UserName = If(IsDBNull(row("UserName")), Nothing, row("UserName"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetWorkManagerScheduledTasks(dtResultSet As DataTable) As List(Of BaseGetWorkManagerScheduledTasksResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetWorkManagerScheduledTasksResponseTypeRow With {
        .Urgent = If(IsDBNull(row("Urgent")), Nothing, row("Urgent")),
        .TaskStatusKey = If(IsDBNull(row("TaskStatusKey")), Nothing, row("TaskStatusKey")),
        .DueDate = If(IsDBNull(row("DueDate")), Nothing, row("DueDate")),
        .Description = If(IsDBNull(row("Description")), Nothing, row("Description")),
        .Customer = If(IsDBNull(row("Customer")), Nothing, row("Customer")),
        .Branch = If(IsDBNull(row("Branch")), Nothing, row("Branch")),
        .Type = If(IsDBNull(row("Type")), Nothing, row("Type")),
        .UserGroupKey = If(IsDBNull(row("UserGroupKey")), Nothing, row("UserGroupKey")),
        .UserKey = If(IsDBNull(row("UserKey")), Nothing, row("UserKey")),
        .TaskInstanceKey = If(IsDBNull(row("TaskInstanceKey")), Nothing, row("TaskInstanceKey")),
        .UserGroupCode = If(IsDBNull(row("UserGroupCode")), Nothing, row("UserGroupCode")),
        .UserGroupDescription = If(IsDBNull(row("UserGroupDescription")), Nothing, row("UserGroupDescription")),
        .UserCode = If(IsDBNull(row("UserCode")), Nothing, row("UserCode")),
        .TaskGroupKey = If(IsDBNull(row("TaskGroupKey")), Nothing, row("TaskGroupKey")),
        .TaskKey = If(IsDBNull(row("TaskKey")), Nothing, row("TaskKey")),
        .PartyKey = If(IsDBNull(row("PartyCnt")), Nothing, row("PartyCnt")),
        .PartyName = If(IsDBNull(row("PartyName")), Nothing, row("PartyName"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_ValidateBankAccountNumber(dtResultSet As DataTable) As List(Of BaseValidateBankAccountNumberResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseValidateBankAccountNumberResponseTypeRow With {
        .BankName = If(IsDBNull(row("BankName")), Nothing, row("BankName")),
        .IsValid = If(IsDBNull(row("IsValid")), Nothing, row("IsValid")),
        .IsValidSpecified = If(IsDBNull(row("IsValidSpecified")), Nothing, row("IsValidSpecified")),
        .AddressLine1 = If(IsDBNull(row("AddressLine1")), Nothing, row("AddressLine1")),
        .AddressLine2 = If(IsDBNull(row("AddressLine2")), Nothing, row("AddressLine2")),
        .AddressLine3 = If(IsDBNull(row("AddressLine3")), Nothing, row("AddressLine3")),
        .AddressLine4 = If(IsDBNull(row("AddressLine4")), Nothing, row("AddressLine4")),
        .PostalCode = If(IsDBNull(row("PostalCode")), Nothing, row("PostalCode")),
        .ValidationMessageDataset = If(IsDBNull(row("ValidationMessageDataset")), Nothing, row("ValidationMessageDataset")),
        .IsValidationOverridable = If(IsDBNull(row("IsValidationOverridable")), Nothing, row("IsValidationOverridable")),
        .IsValidationOverridableSpecified = If(IsDBNull(row("IsValidationOverridableSpecified")), Nothing, row("IsValidationOverridableSpecified"))})
        Return myData.ToList()
    End Function

    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetEventDetails(dtResultSet As DataTable) As List(Of BaseGetEventDetailsResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetEventDetailsResponseTypeRow With {
        .CaseNumber = If(IsDBNull(row("CaseNumber")), Nothing, row("CaseNumber")),
        .BGKey = If(IsDBNull(row("BGKey")), Nothing, row("BGKey")),
        .EventKey = If(IsDBNull(row("EventKey")), Nothing, row("EventKey")),
        .InsuranceFolderKey = If(IsDBNull(row("InsuranceFolderKey")), Nothing, row("InsuranceFolderKey")),
        .InsuranceFileKey = If(IsDBNull(row("InsuranceFileKey")), Nothing, row("InsuranceFileKey")),
        .DocumentKey = If(IsDBNull(row("DocumentKey")), Nothing, row("DocumentKey")),
        .EventDate = If(IsDBNull(row("EventDate")), Nothing, row("EventDate")),
        .TypeKey = If(IsDBNull(row("TypeKey")), Nothing, row("TypeKey")),
        .PolicyCode = If(IsDBNull(row("PolicyCode")), Nothing, row("PolicyCode")),
        .ClaimNumber = If(IsDBNull(row("ClaimNumber")), Nothing, row("ClaimNumber")),
        .ClaimKey = If(IsDBNull(row("ClaimKey")), Nothing, row("ClaimKey")),
        .Description = If(IsDBNull(row("Description")), Nothing, row("Description")),
        .EventDescription = If(IsDBNull(row("EventDescription")), Nothing, row("EventDescription")),
        .UserName = If(IsDBNull(row("UserName")), Nothing, row("UserName")),
        .Priority = If(IsDBNull(row("Priority")), Nothing, row("Priority")),
        .StatusKey = If(IsDBNull(row("StatusKey")), Nothing, row("StatusKey")),
        .EventNoteExist = If(IsDBNull(row("EventNoteExist")), Nothing, row("EventNoteExist")),
        .EventType = If(IsDBNull(row("EventType")), Nothing, row("EventType")),
        .EventTypeCode = If(IsDBNull(row("EventTypeCode")), Nothing, row("EventTypeCode")),
        .Document_Path = If(IsDBNull(row("Document_Path")), Nothing, row("Document_Path"))})
        Return myData.ToList()
    End Function


    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetAuditTrailUser(dtResultSet As DataTable) As List(Of BaseGetAuditTrailUserResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetAuditTrailUserResponseTypeRow With {
        .UserId = If(IsDBNull(row("UserId")), Nothing, row("UserId")),
        .UserName = If(IsDBNull(row("UserName")), Nothing, row("UserName"))})
        Return myData.ToList()
    End Function


    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetAuditTrailModule(dtResultSet As DataTable) As List(Of BaseGetAuditTrailModuleResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetAuditTrailModuleResponseTypeRow With {
        .ModuleId = If(IsDBNull(row("ModuleId")), Nothing, row("ModuleId")),
        .ModuleDesc = If(IsDBNull(row("ModuleName")), Nothing, row("ModuleName"))})
        Return myData.ToList()
    End Function

    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_ValidateAuthorizationSteps(dtResultSet As DataTable) As List(Of BaseValidateAuthorizationStepsResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseValidateAuthorizationStepsResponseTypeRow With {
        .CurrentStep = If(IsDBNull(row("CurrentStep")), Nothing, row("CurrentStep")),
        .IsLastStep = If(IsDBNull(row("IsLastStep")), Nothing, row("IsLastStep")),
        .PMUserGroup = If(IsDBNull(row("PMUserGroup")), Nothing, row("PMUserGroup")),
        .JournalAmount = If(IsDBNull(row("JournalAmount")), Nothing, row("JournalAmount")),
        .ValidationMessage = If(IsDBNull(row("ValidationMessage")), Nothing, row("ValidationMessage"))})
        Return myData.ToList()
    End Function

    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetAuditTrail(dtResultSet As DataTable) As List(Of BaseGetAuditTrailResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetAuditTrailResponseTypeRow With {
        .ConfigurationAuditdetailId = If(IsDBNull(row("ConfigurationAuditdetailid")), Nothing, row("ConfigurationAuditdetailid")),
        .ScreenDescription = If(IsDBNull(row("ScreenDescription")), Nothing, row("ScreenDescription")),
        .FieldDescription = If(IsDBNull(row("FieldDisplayName")), Nothing, row("FieldDisplayName")),
        .UserName = If(IsDBNull(row("UserName")), Nothing, row("UserName")),
        .ModifiedOn = If(IsDBNull(row("ModifiedOn")), Nothing, row("ModifiedOn")),
        .OldValue = If(IsDBNull(row("OldValue")), Nothing, row("OldValue")),
        .NewValue = If(IsDBNull(row("NewValue")), Nothing, row("NewValue"))})
        Return myData.ToList()
    End Function

    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetEventNote(dtResultSet As DataTable) As List(Of BaseGetEventNoteResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetEventNoteResponseTypeRow With {
        .EventKey = If(IsDBNull(row("EventKey")), Nothing, row("EventKey")),
        .EventPublicTextKey = If(IsDBNull(row("EventPublicTextKey")), Nothing, row("EventPublicTextKey")),
        .EventText = If(IsDBNull(row("EventText")), Nothing, row("EventText"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetEventNoteType(dtResultSet As DataTable) As List(Of BaseGetEventNoteTypeResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetEventNoteTypeResponseTypeRow With {
        .EventTypeKey = If(IsDBNull(row("EventTypeKey")), Nothing, row("EventTypeKey")),
        .EventTypeCode = If(IsDBNull(row("EventTypeCode")), Nothing, row("EventTypeCode")),
        .EventTypeDescription = If(IsDBNull(row("EventTypeDescription")), Nothing, row("EventTypeDescription"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetPeriod(dtResultSet As DataTable) As List(Of BaseGetPeriodResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetPeriodResponseTypeRow With {
        .PeriodName = If(IsDBNull(row("PeriodName")), Nothing, row("PeriodName")),
        .YearName = If(IsDBNull(row("YearName")), Nothing, row("YearName")),
        .PeriodID = If(IsDBNull(row("PeriodID")), Nothing, row("PeriodID")),
        .AllocationIndicator = If(IsDBNull(row("AllocationIndicator")), Nothing, row("AllocationIndicator"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_CaseItemsLinkedClaims(dtResultSet As DataTable) As List(Of BaseCaseItemsResponseTypeLinkedClaimsRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseCaseItemsResponseTypeLinkedClaimsRow With {
        .ClaimKey = If(IsDBNull(row("ClaimKey")), Nothing, row("ClaimKey")),
        .ClaimNumber = If(IsDBNull(row("ClaimNumber")), Nothing, row("ClaimNumber")),
        .LossDate = If(IsDBNull(row("LossDate")), Nothing, row("LossDate")),
        .ClaimHandler = If(IsDBNull(row("ClaimHandler")), Nothing, row("ClaimHandler")),
        .RiskType = If(IsDBNull(row("RiskType")), Nothing, row("RiskType")),
        .Status = If(IsDBNull(row("Status")), Nothing, row("Status")),
        .TotalIdmenity = If(IsDBNull(row("TotalIndemnity")), Nothing, row("TotalIndemnity")),
        .TotalExpense = If(IsDBNull(row("TotalExpense")), Nothing, row("TotalExpense")),
        .TotalExcess = If(IsDBNull(row("TotalExcess")), Nothing, row("TotalExcess")),
        .InsuranceFileKey = If(IsDBNull(row("InsuranceFileKey")), Nothing, row("InsuranceFileKey"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetPolicyDetailsForBouncedReceipt(dtResultSet As DataTable) As List(Of BaseGetPolicyDetailsForBouncedReceiptResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetPolicyDetailsForBouncedReceiptResponseTypeRow With {
        .DocumentRef = If(IsDBNull(row("DocumentRef")), Nothing, row("DocumentRef")),
        .InsuranceFileRef = If(IsDBNull(row("InsuranceFileRef")), Nothing, row("InsuranceFileRef")),
        .AccountShortcode = If(IsDBNull(row("AccountShortcode")), Nothing, row("AccountShortcode")),
        .PartyShortcode = If(IsDBNull(row("PartyShortcode")), Nothing, row("PartyShortcode")),
        .PartyName = If(IsDBNull(row("PartyName")), Nothing, row("PartyName")),
        .PartyType = If(IsDBNull(row("PartyType")), Nothing, row("PartyType")),
        .InsuredShortcode = If(IsDBNull(row("InsuredShortcode")), Nothing, row("InsuredShortcode")),
        .InsuredName = If(IsDBNull(row("InsuredName")), Nothing, row("InsuredName")),
        .InsuredKey = If(IsDBNull(row("InsuredKey")), Nothing, row("InsuredKey")),
        .InsuranceFileKey = If(IsDBNull(row("InsuranceFileKey")), Nothing, row("InsuranceFileKey")),
        .GrossPremium = If(IsDBNull(row("GrossPremium")), Nothing, row("GrossPremium")),
        .InceptionDate = If(IsDBNull(row("InceptionDate")), Nothing, row("InceptionDate")),
        .CoverStartDate = If(IsDBNull(row("CoverStartDate")), Nothing, row("CoverStartDate")),
        .CoverEndDate = If(IsDBNull(row("CoverEndDate")), Nothing, row("CoverEndDate"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_FindParty(dtResultSet As DataTable) As List(Of BaseFindPartyResponseTypeRow)

        Dim ResponseList = New List(Of BaseFindPartyResponseTypeRow)(dtResultSet.Rows.Count)
        Dim bSourceIdColumnExists As Boolean = False
        Dim bPartyKeyColumnExists As Boolean = False
        Dim bShortNameColumnExists As Boolean = False
        Dim bResolvedNameColumnExists As Boolean = False
        Dim bAddressLine1ColumnExists As Boolean = False
        Dim bAddressLine2ColumnExists As Boolean = False
        Dim bPostCodeColumnExists As Boolean = False

        Dim bFileCodeColumnExists As Boolean = False
        Dim bContactTelephoneNumberColumnExists As Boolean = False
        Dim bTypeColumnExists As Boolean = False
        Dim bStatusColumnExists As Boolean = False
        Dim bDateOfBirthColumnExists As Boolean = False
        Dim bDateOfBirthSpecifiedColumnExists As Boolean = False
        Dim bAgentKeyColumnExists As Boolean = False
        Dim bAgentKeySpecifiedColumnExists As Boolean = False
        Dim bSwiftLinkColumnExists As Boolean = False
        Dim bAgentTypeColumnExists As Boolean = False
        Dim bPartySourceIdColumnExists As Boolean = False
        Dim bPartySourceDescriptionColumnExists As Boolean = False
        Dim bReinsuranceTypeColumnExists As Boolean = False
        Dim bAllowConsolidatedCommissionColumnExists As Boolean = False
        Dim bIsProspectColumnExists As Boolean = False
        Dim bIsRIBrokerColumnExists As Boolean = False
        Dim bNameColumnExists As Boolean = False
        Dim bDateCancelledColumnExists As Boolean = False
        Dim bCountryCodeColumnExists As Boolean = False
        Dim bServiceLevelColumnsExists As Boolean = False
        Dim bClientCodeColumnExists As Boolean = False
        Dim bPartyTypeIdColumnExists As Boolean = False
        Dim oColumns As DataColumnCollection = dtResultSet.Columns

        If oColumns.Contains("Source_id") Then
            bSourceIdColumnExists = True
        End If
        If oColumns.Contains("PartyKey") Then
            bPartyKeyColumnExists = True
        End If
        If oColumns.Contains("ShortName") Then
            bShortNameColumnExists = True
        End If
        If oColumns.Contains("ResolvedName") Then
            bResolvedNameColumnExists = True
        End If
        If oColumns.Contains("AddressLine1") Then
            bAddressLine1ColumnExists = True
        End If
        If oColumns.Contains("AddressLine2") Then
            bAddressLine2ColumnExists = True
        End If
        If oColumns.Contains("PostCode") Then
            bPostCodeColumnExists = True
        End If

        If oColumns.Contains("CountryCode") Then
            bCountryCodeColumnExists = True
        End If

        If oColumns.Contains("FileCode") Then
            bFileCodeColumnExists = True
        End If
        If oColumns.Contains("ContactTelephoneNumber") Then
            bContactTelephoneNumberColumnExists = True
        End If
        If oColumns.Contains("Type") Then
            bTypeColumnExists = True
        End If
        If oColumns.Contains("Status") Then
            bStatusColumnExists = True
        End If
        If oColumns.Contains("DateOfBirth") Then
            bDateOfBirthColumnExists = True
        End If
        If oColumns.Contains("DateOfBirthSpecified") Then
            bDateOfBirthSpecifiedColumnExists = True
        End If
        If oColumns.Contains("AgentKey") Then
            bAgentKeyColumnExists = True
        End If
        If oColumns.Contains("AgentKeySpecified") Then
            bAgentKeySpecifiedColumnExists = True
        End If
        If oColumns.Contains("SwiftLink") Then
            bSwiftLinkColumnExists = True
        End If
        If oColumns.Contains("AgentType") Then
            bAgentTypeColumnExists = True
        End If


        If oColumns.Contains("PartySourceId") Then
            bPartySourceIdColumnExists = True
        End If
        If oColumns.Contains("PartySourceDescription") Then
            bPartySourceDescriptionColumnExists = True
        End If
        If oColumns.Contains("ReinsuranceType") Then
            bReinsuranceTypeColumnExists = True
        End If
        If oColumns.Contains("AllowConsolidatedCommission") Then
            bAllowConsolidatedCommissionColumnExists = True
        End If
        If oColumns.Contains("IsProspect") Then
            bIsProspectColumnExists = True
        End If
        If oColumns.Contains("IsRIBroker") Then
            bIsRIBrokerColumnExists = True
        End If
        If oColumns.Contains("Name") Then
            bNameColumnExists = True
        End If
        If oColumns.Contains("DateCancelled") Then
            bDateCancelledColumnExists = True
        End If
        If oColumns.Contains("ServiceLevelCode") Then
            bServiceLevelColumnsExists = True
        End If

        If oColumns.Contains("ClientCode") Then
            bClientCodeColumnExists = True
        End If

        If oColumns.Contains("PartyTypeId") Then
            bPartyTypeIdColumnExists = True
        End If

        For Each row As DataRow In dtResultSet.Rows
            Dim oFindParty = New BaseFindPartyResponseTypeRow()

            With oFindParty
                If bSourceIdColumnExists Then
                    .PartySourceId = If(IsDBNull(row("Source_id")), Nothing, row("Source_id"))
                End If
                If bPartyKeyColumnExists Then
                    .PartyKey = If(IsDBNull(row("PartyKey")), Nothing, row("PartyKey"))
                End If
                If bShortNameColumnExists Then
                    .ShortName = If(IsDBNull(row("ShortName")), Nothing, row("ShortName"))
                End If
                If bResolvedNameColumnExists Then
                    .ResolvedName = If(IsDBNull(row("ResolvedName")), Nothing, row("ResolvedName"))
                End If
                If bAddressLine1ColumnExists Then
                    .AddressLine1 = If(IsDBNull(row("AddressLine1")), Nothing, row("AddressLine1"))
                End If
                If bAddressLine2ColumnExists Then
                    .AddressLine2 = If(IsDBNull(row("AddressLine2")), Nothing, row("AddressLine2"))
                End If
                If bPostCodeColumnExists Then
                    .PostCode = If(IsDBNull(row("PostCode")), Nothing, row("PostCode"))
                End If
                If bFileCodeColumnExists Then
                    .FileCode = If(IsDBNull(row("FileCode")), Nothing, row("FileCode"))
                End If
                If bContactTelephoneNumberColumnExists Then
                    .ContactTelephoneNumber = If(IsDBNull(row("ContactTelephoneNumber")), Nothing, row("ContactTelephoneNumber"))
                End If
                If bTypeColumnExists Then
                    .Type = If(IsDBNull(row("Type")), Nothing, row("Type"))
                End If
                If bStatusColumnExists Then
                    .Status = If(IsDBNull(row("Status")), Nothing, row("Status"))
                End If
                If bDateOfBirthColumnExists Then
                    .DateOfBirth = If(IsDBNull(row("DateOfBirth")), Nothing, row("DateOfBirth"))
                End If
                If bDateOfBirthSpecifiedColumnExists Then
                    .DateOfBirthSpecified = If(IsDBNull(row("DateOfBirthSpecified")), Nothing, row("DateOfBirthSpecified"))
                End If
                If bAgentKeyColumnExists Then
                    .AgentKey = If(IsDBNull(row("AgentKey")), Nothing, row("AgentKey"))
                End If
                If bAgentKeySpecifiedColumnExists Then
                    .AgentKeySpecified = If(IsDBNull(row("AgentKeySpecified")), Nothing, row("AgentKeySpecified"))
                End If
                If bSwiftLinkColumnExists Then
                    .SwiftLink = If(IsDBNull(row("SwiftLink")), Nothing, row("SwiftLink"))
                End If
                If bAgentTypeColumnExists Then
                    .AgentType = If(IsDBNull(row("AgentType")), Nothing, row("AgentType"))
                End If
                If bPartySourceIdColumnExists Then
                    .PartySourceId = If(IsDBNull(row("PartySourceId")), Nothing, row("PartySourceId"))
                End If
                If bPartySourceDescriptionColumnExists Then
                    .PartySourceDescription = If(IsDBNull(row("PartySourceDescription")), Nothing, row("PartySourceDescription"))
                End If
                If bReinsuranceTypeColumnExists Then
                    .ReinsuranceType = If(IsDBNull(row("ReinsuranceType")), Nothing, row("ReinsuranceType"))
                End If
                If bAllowConsolidatedCommissionColumnExists Then
                    .AllowConsolidatedCommission = If(IsDBNull(row("AllowConsolidatedCommission")), Nothing, row("AllowConsolidatedCommission"))
                End If
                If bIsProspectColumnExists Then
                    .IsProspect = If(IsDBNull(row("IsProspect")), Nothing, row("IsProspect"))
                End If
                If bIsRIBrokerColumnExists Then
                    .IsRIBroker = If(IsDBNull(row("IsRIBroker")), Nothing, row("IsRIBroker"))
                End If

                If bNameColumnExists Then
                    .Name = If(IsDBNull(row("Name")), Nothing, row("Name"))
                End If
                If bDateCancelledColumnExists Then
                    .DateCancelled = If(IsDBNull(row("DateCancelled")) Or String.IsNullOrEmpty(row("DateCancelled")), Nothing, row("DateCancelled"))
                End If
                If bClientCodeColumnExists Then
                    .ClientCode = If(IsDBNull(row("ClientCode")), Nothing, row("ClientCode"))
                End If

                If bCountryCodeColumnExists Then
                    .CountryCode = If(IsDBNull(row("CountryCode")), Nothing, row("CountryCode"))
                End If
                If bServiceLevelColumnsExists Then
                    .ServiceLevelCode = If(IsDBNull(row("ServiceLevelCode")), Nothing, row("ServiceLevelCode"))
                    .ServiceLevelDescription = If(IsDBNull(row("ServiceLevelDescription")), Nothing, row("ServiceLevelDescription"))
                End If

                If bPartyTypeIdColumnExists Then
                    .PartyTypeId = If(IsDBNull(row("PartyTypeId")), Nothing, row("PartyTypeId"))
                End If
            End With
            ResponseList.Add(oFindParty)
        Next
        Return ResponseList

    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetBrokerSummary(dtResultSet As DataTable) As List(Of BaseGetBrokerSummaryResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetBrokerSummaryResponseTypeRow With {
        .RiskStatus = If(IsDBNull(row("RiskStatus")), Nothing, row("RiskStatus")),
        .InsuranceFileKey = If(IsDBNull(row("InsuranceFileKey")), Nothing, row("InsuranceFileKey")),
        .InsuranceFolderKey = If(IsDBNull(row("InsuranceFolderKey")), Nothing, row("InsuranceFolderKey")),
        .PartyKey = If(IsDBNull(row("PartyKey")), Nothing, row("PartyKey")),
        .InsuranceRef = If(IsDBNull(row("InsuranceRef")), Nothing, row("InsuranceRef")),
        .ProductCode = If(IsDBNull(row("ProductCode")), Nothing, row("ProductCode")),
        .ProductDescription = If(IsDBNull(row("ProductDescription")), Nothing, row("ProductDescription")),
        .InsuranceFileTypeDescription = If(IsDBNull(row("InsuranceFileTypeDescription")), Nothing, row("InsuranceFileTypeDescription")),
        .ClientShortName = If(IsDBNull(row("ClientShortName")), Nothing, row("ClientShortName")),
        .ClientName = If(IsDBNull(row("ClientName")), Nothing, row("ClientName")),
        .IssuedDate = If(IsDBNull(row("IssuedDate")), Nothing, row("IssuedDate")),
        .StartDate = If(IsDBNull(row("StartDate")), Nothing, row("StartDate")),
        .ExpiryDate = If(IsDBNull(row("ExpiryDate")), Nothing, row("ExpiryDate")),
        .InsuranceFileTypeCode = If(IsDBNull(row("InsuranceFileTypeCode")), Nothing, row("InsuranceFileTypeCode")),
        .PolicyStatusCode = If(IsDBNull(row("PolicyStatusCode")), Nothing, row("PolicyStatusCode")),
        .PolicyStatusDescription = If(IsDBNull(row("PolicyStatusDescription")), Nothing, row("PolicyStatusDescription")),
        .IsCurrent = If(IsDBNull(row("IsCurrent")), Nothing, row("IsCurrent")),
        .BaseInsuranceFolderKey = If(IsDBNull(row("BaseInsuranceFolderKey")), Nothing, row("BaseInsuranceFolderKey")),
        .QuoteStatusKey = If(IsDBNull(row("QuoteStatusKey")), Nothing, row("QuoteStatusKey")),
        .QuoteVersion = If(IsDBNull(row("QuoteVersion")), Nothing, row("QuoteVersion")),
        .AgentKey = If(IsDBNull(row("AgentKey")), Nothing, row("AgentKey")),
        .AgentName = If(IsDBNull(row("AgentName")), Nothing, row("AgentName")),
        .QuoteExpiryDate = If(IsDBNull(row("QuoteExpiryDate")), Nothing, row("QuoteExpiryDate")),
        .RenewedVersion = If(IsDBNull(row("RenewedVersion")), Nothing, row("RenewedVersion")),
        .IsMarketPlacePolicy = If(IsDBNull(row("IsMarketPlacePolicy")), Nothing, row("IsMarketPlacePolicy")),
        .IsReinstateLinkVersion = If(IsDBNull(row("IsReinstateLink")), Nothing, row("IsReinstateLink")),
        .BaseInsuranceFileKey = If(IsDBNull(row("BaseInsuranceFileKey")), Nothing, row("BaseInsuranceFileKey")),
        .RiskNumber = If(IsDBNull(row("RiskNumber")), Nothing, row("RiskNumber")),
        .RiskDescription = If(IsDBNull(row("RiskDescription")), Nothing, row("RiskDescription")),
        .AssociatedClients = If(IsDBNull(row("AssociatedClients")), Nothing, row("AssociatedClients"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetPartyPolicies(dtResultSet As DataTable) As List(Of BaseGetPartyPoliciesResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetPartyPoliciesResponseTypeRow With {
        .InsuranceFileKey = If(IsDBNull(row("InsuranceFileKey")), Nothing, row("InsuranceFileKey")),
        .InsuranceFileSourceKey = If(IsDBNull(row("InsuranceFileSourceKey")), Nothing, row("InsuranceFileSourceKey")),
        .InsuranceRef = If(IsDBNull(row("InsuranceRef")), Nothing, row("InsuranceRef")),
        .LastTransDesc = If(IsDBNull(row("LastTransDesc")), Nothing, row("LastTransDesc")),
        .TypeCode = If(IsDBNull(row("TypeCode")), Nothing, row("TypeCode")),
        .RenewalDate = If(IsDBNull(row("RenewalDate")), Nothing, row("RenewalDate")),
        .InsuranceHolderKey = If(IsDBNull(row("InsuranceHolderKey")), Nothing, row("InsuranceHolderKey")),
        .InsuranceFolderKey = If(IsDBNull(row("InsuranceFolderKey")), Nothing, row("InsuranceFolderKey")),
        .ProductKey = If(IsDBNull(row("ProductKey")), Nothing, row("ProductKey")),
        .ProductCode = If(IsDBNull(row("ProductCode")), Nothing, row("ProductCode")),
        .LeadAgentKey = If(IsDBNull(row("LeadAgentKey")), Nothing, row("LeadAgentKey")),
        .LeadAgentCode = If(IsDBNull(row("LeadAgentCode")), Nothing, row("LeadAgentCode")),
        .DateCreated = If(IsDBNull(row("DateCreated")), Nothing, row("DateCreated")),
        .StatusCode = If(IsDBNull(row("StatusCode")), Nothing, row("StatusCode")),
        .ThisPremium = If(IsDBNull(row("ThisPremium")), Nothing, row("ThisPremium")),
        .PolicyTypeKey = If(IsDBNull(row("PolicyTypeKey")), Nothing, row("PolicyTypeKey")),
        .PolicyTypeCode = If(IsDBNull(row("PolicyTypeCode")), Nothing, row("PolicyTypeCode")),
        .PolicyTypeDesc = If(IsDBNull(row("PolicyTypeDesc")), Nothing, row("PolicyTypeDesc")),
        .InsuranceDesc = If(IsDBNull(row("InsuranceDesc")), Nothing, row("InsuranceDesc")),
        .OpenPolicyClaims = If(IsDBNull(row("OpenPolicyClaims")), Nothing, row("OpenPolicyClaims")),
        .ClosePolicyClaims = If(IsDBNull(row("ClosePolicyClaims")), Nothing, row("ClosePolicyClaims")),
        .CoverStartDate = If(IsDBNull(row("CoverStartDate")), Nothing, row("CoverStartDate")),
        .ExpiryDate = If(IsDBNull(row("ExpiryDate")), Nothing, row("ExpiryDate")),
        .MarkedForCollection = If(IsDBNull(row("MarkedForCollection")), Nothing, row("MarkedForCollection")),
        .BaseInsuranceFolderKey = If(IsDBNull(row("BaseInsuranceFolderKey")), Nothing, row("BaseInsuranceFolderKey")),
        .QuoteStatusKey = If(IsDBNull(row("QuoteStatusKey")), Nothing, row("QuoteStatusKey")),
        .QuoteVersion = If(IsDBNull(row("QuoteVersion")), Nothing, row("QuoteVersion")),
        .PolicyStatus = If(IsDBNull(row("PolicyStatus")), Nothing, row("PolicyStatus")),
        .IsMarketPlacePolicy = If(IsDBNull(row("IsMarketPlacePolicy")), Nothing, row("IsMarketPlacePolicy")),
        .CorrespondenceType = If(IsDBNull(row("CorrespondenceType")), Nothing, row("CorrespondenceType")),
        .DefaultPreferredCorrespondence = If(IsDBNull(row("DefaultPreferredCorrespondence")), Nothing, row("DefaultPreferredCorrespondence")),
        .IsAgentReceiveCorrespondence = If(IsDBNull(row("IsAgentCorrespondence")), Nothing, row("IsAgentCorrespondence"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetPartySummary(dtResultSet As DataTable) As List(Of BaseGetPartySummaryResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetPartySummaryResponseTypeRow With {
        .RiskStatus = If(dtResultSet.Columns.Contains("RiskStatus"), If(IsDBNull(row("RiskStatus")), Nothing, row("RiskStatus")), Nothing),
        .InsuranceFileId = If(IsDBNull(row("InsuranceFileId")), Nothing, row("InsuranceFileId")),
        .BranchKey = If(IsDBNull(row("BranchKey")), Nothing, row("BranchKey")),
        .BranchCode = If(IsDBNull(row("BranchCode")), Nothing, row("BranchCode")),
        .InsuranceFileKey = If(IsDBNull(row("InsuranceFileKey")), Nothing, row("InsuranceFileKey")),
        .PolicyRef = If(IsDBNull(row("PolicyRef")), Nothing, row("PolicyRef")),
        .InsuranceFolderKey = If(IsDBNull(row("InsuranceFolderKey")), Nothing, row("InsuranceFolderKey")),
        .PolicyTypeId = If(IsDBNull(row("PolicyTypeId")), Nothing, row("PolicyTypeId")),
        .LeadInsurerKey = If(IsDBNull(row("LeadInsurerKey")), Nothing, row("LeadInsurerKey")),
        .DateIssued = If(IsDBNull(row("DateIssued")), Nothing, row("DateIssued")),
        .CoverStartDate = If(IsDBNull(row("CoverStartDate")), Nothing, row("CoverStartDate")),
        .ExpiryDate = If(IsDBNull(row("ExpiryDate")), Nothing, row("ExpiryDate")),
        .RenewalDate = If(IsDBNull(row("RenewalDate")), Nothing, row("RenewalDate")),
        .InsuredKey = If(IsDBNull(row("InsuredKey")), Nothing, row("InsuredKey")),
        .ProductKey = If(IsDBNull(row("ProductKey")), Nothing, row("ProductKey")),
        .LeadAgentKey = If(IsDBNull(row("LeadAgentKey")), Nothing, row("LeadAgentKey")),
        .ThisPremium = If(IsDBNull(row("ThisPremium")), Nothing, row("ThisPremium")),
        .AnnualPremium = If(IsDBNull(row("AnnualPremium")), Nothing, row("AnnualPremium")),
        .NetPremium = If(IsDBNull(row("NetPremium")), Nothing, row("NetPremium")),
        .TaxAmount = If(IsDBNull(row("TaxAmount")), Nothing, row("TaxAmount")),
        .GeminiPolicyStatus = If(IsDBNull(row("GeminiPolicyStatus")), Nothing, row("GeminiPolicyStatus")),
        .PartyShortName = If(IsDBNull(row("PartyShortName")), Nothing, row("PartyShortName")),
        .ProductCode = If(IsDBNull(row("ProductCode")), Nothing, row("ProductCode")),
        .ProductDesc = If(IsDBNull(row("ProductDesc")), Nothing, row("ProductDesc")),
        .InsuranceFileTypeCode = If(IsDBNull(row("InsuranceFileTypeCode")), Nothing, row("InsuranceFileTypeCode")),
        .PolicyStatusCode = If(IsDBNull(row("PolicyStatusCode")), Nothing, row("PolicyStatusCode")),
        .InsurerShortName = If(IsDBNull(row("InsurerShortName")), Nothing, row("InsurerShortName")),
        .AgentShortName = If(IsDBNull(row("AgentShortName")), Nothing, row("AgentShortName")),
        .PolicyTypeCode = If(IsDBNull(row("PolicyTypeCode")), Nothing, row("PolicyTypeCode")),
        .PolicyTypeDesc = If(IsDBNull(row("PolicyTypeDesc")), Nothing, row("PolicyTypeDesc")),
        .CurrencyCode = If(IsDBNull(row("CurrencyCode")), Nothing, row("CurrencyCode")),
        .AlternativeRef = If(IsDBNull(row("AlternativeRef")), Nothing, row("AlternativeRef")),
        .Regarding = If(IsDBNull(row("Regarding")), Nothing, row("Regarding")),
        .PolicyStatus = If(IsDBNull(row("PolicyStatus")), Nothing, row("PolicyStatus")),
        .RiskTypeDescription = If(IsDBNull(row("RiskTypeDescription")), Nothing, row("RiskTypeDescription")),
        .EventDescription = If(IsDBNull(row("EventDescription")), Nothing, row("EventDescription")),
        .IsCurrent = If(IsDBNull(row("IsCurrent")), Nothing, row("IsCurrent")),
        .MarkedForCollection = If(IsDBNull(row("MarkedForCollection")), Nothing, row("MarkedForCollection")),
        .BaseInsuranceFolderKey = If(IsDBNull(row("BaseInsuranceFolderKey")), Nothing, row("BaseInsuranceFolderKey")),
        .QuoteStatusKey = If(IsDBNull(row("QuoteStatusKey")), Nothing, row("QuoteStatusKey")),
        .QuoteVersion = If(IsDBNull(row("QuoteVersion")), Nothing, row("QuoteVersion")),
        .QuoteExpiryDate = If(IsDBNull(row("QuoteExpiryDate")), Nothing, row("QuoteExpiryDate")),
        .IsMarketPlacePolicy = If(IsDBNull(row("IsMarketPlacePolicy")), Nothing, row("IsMarketPlacePolicy")),
        .IsReadOnly = If(IsDBNull(row("IsReadOnly")), Nothing, row("IsReadOnly"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_AddBackDatedMTAQuote(dtResultSet As DataTable) As List(Of BaseAddBackDatedMTAQuoteResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseAddBackDatedMTAQuoteResponseTypeRow With {
         .InsuranceFileCnt = If(IsDBNull(row("InsuranceFileCnt")), Nothing, row("InsuranceFileCnt")),
         .PolicyType = If(IsDBNull(row("PolicyType")), Nothing, row("PolicyType")),
         .CoverStartDate = If(IsDBNull(row("CoverStartDate")), Nothing, row("CoverStartDate")),
         .CoverEndDate = If(IsDBNull(row("CoverEndDate")), Nothing, row("CoverEndDate")),
         .MTAPremium = If(IsDBNull(row("MTAPremium")), Nothing, row("MTAPremium")),
         .OriginalMTAPremium = If(IsDBNull(row("OriginalMTAPremium")), Nothing, row("OriginalMTAPremium")),
         .PolicyStatus = If(IsDBNull(row("PolicyStatus")), Nothing, row("PolicyStatus")),
         .ReversedInsuranceFileCnt = If(IsDBNull(row("ReversedInsuranceFileCnt")), Nothing, row("ReversedInsuranceFileCnt")),
         .QuoteStatus = If(IsDBNull(row("QuoteStatus")), Nothing, row("QuoteStatus")),
         .OriginalCommission = If(IsDBNull(row("OriginalCommission")), Nothing, row("OriginalCommission")),
         .MTACommission = If(IsDBNull(row("MTACommission")), Nothing, row("MTACommission")),
         .OriginalFee = If(IsDBNull(row("OriginalFee")), Nothing, row("OriginalFee")),
         .MTAFee = If(IsDBNull(row("MTAFee")), Nothing, row("MTAFee"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_FindPolicy(dtResultSet As DataTable) As List(Of BaseFindPolicyResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseFindPolicyResponseTypeRow With {
        .InsuranceFileKey = If(IsDBNull(row("InsuranceFileKey")), Nothing, row("InsuranceFileKey")),
        .InsuranceRef = If(IsDBNull(row("InsuranceRef")), Nothing, row("InsuranceRef")),
        .InsuranceFileType = If(IsDBNull(row("InsuranceFileType")), Nothing, row("InsuranceFileType")),
        .ClientName = If(IsDBNull(row("ClientName")), Nothing, row("ClientName")),
        .ClientShortName = If(IsDBNull(row("ClientShortName")), Nothing, row("ClientShortName")),
        .PartyKey = If(IsDBNull(row("PartyKey")), Nothing, row("PartyKey")),
        .CreatedDate = If(IsDBNull(row("CreatedDate")), Nothing, row("CreatedDate")),
        .LastModifiedDate = If(IsDBNull(row("LastModifiedDate")), Nothing, row("LastModifiedDate")),
        .InsuranceFolderKey = If(IsDBNull(row("InsuranceFolderKey")), Nothing, row("InsuranceFolderKey")),
        .ProductCode = If(IsDBNull(row("ProductCode")), Nothing, row("ProductCode")),
        .Status = If(IsDBNull(row("Status")), Nothing, row("Status")),
        .ProductDescription = If(IsDBNull(row("ProductDescription")), Nothing, row("ProductDescription")),
        .AssociatedClients = If(IsDBNull(row("AssociatedClients")), Nothing, row("AssociatedClients"))})
        Return myData.ToList()
    End Function

    ''' <summary>
    ''' To covert dataset to list
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_FindLatestPolicyVersions(dtResultSet As DataTable) As List(Of BaseFindLatestPolicyVersionsResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseFindLatestPolicyVersionsResponseTypeRow With {
        .InsuranceFileKey = row("InsuranceFileKey"),
        .InsuranceFolderKey = row("InsuranceFolderKey"),
        .InsuranceRef = row("InsuranceRef"),
        .PolicyStatusCode = If(IsDBNull(row("PolicyStatusCode")), Nothing, row("PolicyStatusCode")),
        .PolicyStatusDescription = If(IsDBNull(row("PolicyStatusDescription")), Nothing, row("PolicyStatusDescription")),
        .InsuranceFileTypeCode = If(IsDBNull(row("InsuranceFileTypeCode")), Nothing, row("InsuranceFileTypeCode")),
        .InsuranceFileTypeDescription = If(IsDBNull(row("InsuranceFileTypeDescription")), Nothing, row("InsuranceFileTypeDescription")),
        .ProductCode = If(IsDBNull(row("ProductCode")), Nothing, row("ProductCode")),
        .ProductDescription = If(IsDBNull(row("ProductDescription")), Nothing, row("ProductDescription")),
        .IssuedDate = If(IsDBNull(row("IssueDate")), Nothing, row("IssueDate")),
        .AgentKey = If(IsDBNull(row("AgentKey")), Nothing, row("AgentKey")),
        .AgentName = If(IsDBNull(row("AgentName")), Nothing, row("AgentName")),
        .PartyKey = row("PartyKey"),
        .ClientShortName = If(IsDBNull(row("ClientName")), Nothing, row("ClientName")),
        .ClientName = If(IsDBNull(row("ClientName")), Nothing, row("ClientName")),
        .CoverStartDate = If(IsDBNull(row("CoverStartDate")), Nothing, row("CoverStartDate")),
        .RenewalDate = If(IsDBNull(row("RenewalDate")), Nothing, row("RenewalDate")),
        .TransactionDate = If(IsDBNull(row("TransactionDate")), Nothing, row("TransactionDate")),
        .QuoteExpiryDate = If(IsDBNull(row("QuoteExpiryDate")), Nothing, row("QuoteExpiryDate")),
        .IsMarketPlacePolicy = row("IsMarketPlacePolicy"),
        .IsReadOnly = row("IsReadOnly"),
        .IsCancelled = row("IsCancelled"),
        .NoOfVersions = row("NoOfVersions"),
        .NoOfRenewalVersions = row("NoOfRenewalVersions"),
        .MarkedQuoteForCollection = row("MarkedQuoteForCollection"),
        .AssociatedClients = If(IsDBNull(row("AssociatedClients")), Nothing, row("AssociatedClients"))})
        Return myData.ToList()
    End Function

    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetAllPolicyVersions(dtResultSet As DataTable) As List(Of BaseGetAllPolicyVersionsResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetAllPolicyVersionsResponseTypeRow With {
        .InsuranceFolderKey = If(IsDBNull(row("InsuranceFolderKey")), Nothing, row("InsuranceFolderKey")),
        .insuranceFileKey = If(IsDBNull(row("insuranceFileKey")), Nothing, row("insuranceFileKey")),
        .InsuranceHolderKey = If(IsDBNull(row("InsuranceHolderKey")), Nothing, row("InsuranceHolderKey")),
        .PolicyTypeCode = If(IsDBNull(row("PolicyTypeCode")), Nothing, row("PolicyTypeCode")),
        .PolicyRef = If(IsDBNull(row("PolicyRef")), Nothing, row("PolicyRef")),
        .InsuranceFileTypeDesc = If(IsDBNull(row("InsuranceFileTypeDesc")), Nothing, row("InsuranceFileTypeDesc")),
        .ProductDesc = If(IsDBNull(row("ProductDesc")), Nothing, row("ProductDesc")),
        .RenewalDate = If(IsDBNull(row("RenewalDate")), Nothing, row("RenewalDate")),
        .PartyShortName = If(IsDBNull(row("PartyShortName")), Nothing, row("PartyShortName")),
        .Premium = If(IsDBNull(row("Premium")), Nothing, row("Premium")),
        .InsuranceFileTypeCode = If(IsDBNull(row("InsuranceFileTypeCode")), Nothing, row("InsuranceFileTypeCode")),
        .InsuranceFileTypeKey = If(IsDBNull(row("InsuranceFileTypeKey")), Nothing, row("InsuranceFileTypeKey")),
        .CoverStartDate = If(IsDBNull(row("CoverStartDate")), Nothing, row("CoverStartDate")),
        .ExpiryDate = If(IsDBNull(row("ExpiryDate")), Nothing, row("ExpiryDate")),
        .QuoteExpiryDate = If(IsDBNull(row("QuoteExpiryDate")), Nothing, row("QuoteExpiryDate")),
        .DateIssued = If(IsDBNull(row("DateIssued")), Nothing, row("DateIssued")),
        .EventDesc = If(IsDBNull(row("EventDesc")), Nothing, row("EventDesc")),
        .TaxAmount = If(IsDBNull(row("TaxAmount")), Nothing, row("TaxAmount")),
        .GracePeriod = If(IsDBNull(row("GracePeriod")), Nothing, row("GracePeriod")),
        .ProductCode = If(IsDBNull(row("ProductCode")), Nothing, row("ProductCode")),
        .PolicyVersion = If(IsDBNull(row("PolicyVersion")), Nothing, row("PolicyVersion")),
        .PaymentMethod = If(IsDBNull(row("PaymentMethod")), Nothing, row("PaymentMethod")),
        .Regarding = If(IsDBNull(row("Regarding")), Nothing, row("Regarding")),
        .InstalmentPlanStatus = If(IsDBNull(row("InstalmentPlanStatus")), Nothing, row("InstalmentPlanStatus")),
        .PreviousVersionInstalmentPlanStatus = If(IsDBNull(row("PreviousVersionInstalmentPlanStatus")), Nothing, row("PreviousVersionInstalmentPlanStatus")),
        .AlternativeRef = If(IsDBNull(row("AlternativeRef")), Nothing, row("AlternativeRef")),
        .PolicyStatus = If(IsDBNull(row("PolicyStatus")), Nothing, row("PolicyStatus")),
        .LapseCancelDate = If(IsDBNull(row("LapseCancelDate")), Nothing, row("LapseCancelDate")),
        .InsuredPersons = If(IsDBNull(row("InsuredPersons")), Nothing, row("InsuredPersons")),
        .Intermediary = If(IsDBNull(row("Intermediary")), Nothing, row("Intermediary")),
        .Currency = If(IsDBNull(row("Currency")), Nothing, row("Currency")),
        .TransactionDate = If(IsDBNull(row("TransactionDate")), Nothing, row("TransactionDate")),
        .PolicyStatusCode = If(IsDBNull(row("PolicyStatusCode")), Nothing, row("PolicyStatusCode")),
        .IsCurrent = If(IsDBNull(row("IsCurrent")), Nothing, row("IsCurrent")),
        .IsOutOfSequenceReplaced = If(IsDBNull(row("IsOutOfSequenceReplaced")), Nothing, row("IsOutOfSequenceReplaced")),
        .IsMarketPlacePolicy = If(IsDBNull(row("IsMarketPlacePolicy")), Nothing, row("IsMarketPlacePolicy")),
        .IsReadOnly = If(IsDBNull(row("IsReadOnly")), Nothing, row("IsReadOnly")),
        .BaseInsuranceFileKey = If(IsDBNull(row("BaseInsuranceFileKey")), Nothing, row("BaseInsuranceFileKey")),
        .AgentKey = If(IsDBNull(row("AgentKey")), Nothing, row("AgentKey")),
        .AgentName = If(IsDBNull(row("AgentName")), Nothing, row("AgentName")),
        .MarkedQuoteForCollection = row("MarkedQuoteForCollection"),
        .AssociatedClients = If(IsDBNull(row("AssociatedClients")), Nothing, row("AssociatedClients"))})
        Return myData.ToList()

        ' cOT CODE
        ' .Insurer = IF(IsDBNull(row("Insurer")), Nothing, row("Insurer")), _
        ' .Handler = IF(IsDBNull(row("Handler")), Nothing, row("Handler")), _
        ' .Branch = IF(IsDBNull(row("Branch")), Nothing, row("Branch")), _
        '.SchemeName = IF(IsDBNull(row("SchemeName")), Nothing, row("SchemeName")), _
        '.PaymentFrequency = IF(IsDBNull(row("PaymentFrequency")), Nothing, row("PaymentFrequency")), _
        '.LegalExpenseProvider = IF(IsDBNull(row("LegalExpenseProvider")), Nothing, row("LegalExpenseProvider")), _
        '.Regarding1 = IF(IsDBNull(row("Regarding1")), Nothing, row("Regarding1")), _
        '.party_cnt = IF(IsDBNull(row("party_cnt")), Nothing, row("party_cnt")), _

    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetCashDepositsForPolicyCashDepositPolicies(dtResultSet As DataTable) As List(Of BaseGetCashDepositsForPolicyResponseTypeCashDepositPoliciesRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetCashDepositsForPolicyResponseTypeCashDepositPoliciesRow With {
        .CashDepositKey = If(IsDBNull(row("CashDepositKey")), Nothing, row("CashDepositKey")),
        .AccountKey = If(IsDBNull(row("AccountKey")), Nothing, row("AccountKey")),
        .PartyKey = If(IsDBNull(row("PartyKey")), Nothing, row("PartyKey")),
        .CashDepositRef = If(IsDBNull(row("CashDepositRef")), Nothing, row("CashDepositRef")),
        .Amount = If(IsDBNull(row("Amount")), Nothing, row("Amount")),
        .AvailableBalance = If(IsDBNull(row("AvailableBalance")), Nothing, row("AvailableBalance")),
        .DateCreated = If(IsDBNull(row("DateCreated")), Nothing, row("DateCreated"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetHeaderAndAgentCommissionByKey(dtResultSet As DataTable) As List(Of BaseGetHeaderAndAgentCommissionByKeyResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetHeaderAndAgentCommissionByKeyResponseTypeRow With {
        .Agent = If(IsDBNull(row("Agent")), Nothing, row("Agent")),
        .AgentType = If(IsDBNull(row("AgentType")), Nothing, row("AgentType")),
        .RiskType = If(IsDBNull(row("RiskType")), Nothing, row("RiskType")),
        .CommissionBand = If(IsDBNull(row("CommissionBand")), Nothing, row("CommissionBand")),
        .Premium = If(IsDBNull(row("Premium")), Nothing, row("Premium")),
        .CommissionRate = If(IsDBNull(row("CommissionRate")), Nothing, row("CommissionRate")),
        .CommissionValue = If(IsDBNull(row("CommissionValue")), Nothing, row("CommissionValue")),
        .IsLeadAgent = If(IsDBNull(row("IsLeadAgent")), Nothing, row("IsLeadAgent")),
        .TaxGroup = If(IsDBNull(row("TaxGroup")), Nothing, row("TaxGroup")),
        .TaxValue = If(IsDBNull(row("TaxValue")), Nothing, row("TaxValue")),
        .IsValue = If(IsDBNull(row("IsValue")), Nothing, row("IsValue"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetHeaderAndPolicyFeesByKey(dtResultSet As DataTable) As List(Of BaseGetHeaderAndPolicyFeesByKeyResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetHeaderAndPolicyFeesByKeyResponseTypeRow With {
        .FeeName = If(IsDBNull(row("FeeName")), Nothing, row("FeeName")),
        .CurrencyCode = If(IsDBNull(row("CurrencyCode")), Nothing, row("CurrencyCode")),
        .AppliedTo = If(IsDBNull(row("AppliedTo")), Nothing, row("AppliedTo")),
        .Premium = If(IsDBNull(row("Premium")), Nothing, row("Premium")),
        .Rate = If(IsDBNull(row("Rate")), Nothing, row("Rate")),
        .FeeAmount = If(IsDBNull(row("FeeAmount")), Nothing, row("FeeAmount")),
        .TaxAmount = If(IsDBNull(row("TaxAmount")), Nothing, row("TaxAmount")),
        .TotalAmount = If(IsDBNull(row("TotalAmount")), Nothing, row("TotalAmount")),
        .TaxGroup = If(IsDBNull(row("TaxGroup")), Nothing, row("TaxGroup")),
        .IncludeInInstallment = If(IsDBNull(row("IncludeInInstallment")), Nothing, row("IncludeInInstallment")),
        .SpreadAcrossInstallment = If(IsDBNull(row("SpreadAcrossInstallment")), Nothing, row("SpreadAcrossInstallment")),
        .IsValue = If(IsDBNull(row("IsValue")), Nothing, row("IsValue")),
        .PolicyFeeKey = If(IsDBNull(row("PolicyFeeKey")), Nothing, row("PolicyFeeKey"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetHeaderAndPolicyTaxByKey(dtResultSet As DataTable) As List(Of BaseGetHeaderAndPolicyTaxByKeyResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetHeaderAndPolicyTaxByKeyResponseTypeRow With {
        .TaxGroup = If(IsDBNull(row("TaxGroup")), Nothing, row("TaxGroup")),
        .Sequence = If(IsDBNull(row("Sequence")), Nothing, row("Sequence")),
        .TaxBand = If(IsDBNull(row("TaxBand")), Nothing, row("TaxBand")),
        .TaxAmount = If(IsDBNull(row("TaxAmount")), Nothing, row("TaxAmount")),
        .CalculationBasis = If(IsDBNull(row("CalculationBasis")), Nothing, row("CalculationBasis")),
        .Rate = If(IsDBNull(row("Rate")), Nothing, row("Rate")),
        .ClassOfBusiness = If(IsDBNull(row("ClassOfBusiness")), Nothing, row("ClassOfBusiness")),
        .Country = If(IsDBNull(row("Country")), Nothing, row("Country")),
        .State = If(IsDBNull(row("State")), Nothing, row("State")),
        .IsNotAppliedToClient = If(IsDBNull(row("IsNotAppliedToClient")), Nothing, row("IsNotAppliedToClient")),
        .IncludeInInstallment = If(IsDBNull(row("IncludeInInstallment")), Nothing, row("IncludeInInstallment")),
        .SpreadAcrossInstallment = If(IsDBNull(row("SpreadAcrossInstallment")), Nothing, row("SpreadAcrossInstallment")),
        .ApplyTaxBy = If(IsDBNull(row("ApplyTaxBy")), Nothing, row("ApplyTaxBy")),
        .IsValue = If(IsDBNull(row("IsValue")), Nothing, row("IsValue"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetHeaderAndRiskFeesByKey(dtResultSet As DataTable) As List(Of BaseGetHeaderAndRiskFeesByKeyResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetHeaderAndRiskFeesByKeyResponseTypeRow With {
        .FeeName = If(IsDBNull(row("FeeName")), Nothing, row("FeeName")),
        .CurrencyCode = If(IsDBNull(row("CurrencyCode")), Nothing, row("CurrencyCode")),
        .AppliedTo = If(IsDBNull(row("AppliedTo")), Nothing, row("AppliedTo")),
        .Premium = If(IsDBNull(row("Premium")), Nothing, row("Premium")),
        .Rate = If(IsDBNull(row("Rate")), Nothing, row("Rate")),
        .FeeAmount = If(IsDBNull(row("FeeAmount")), Nothing, row("FeeAmount")),
        .TaxAmount = If(IsDBNull(row("TaxAmount")), Nothing, row("TaxAmount")),
        .TotalAmount = If(IsDBNull(row("TotalAmount")), Nothing, row("TotalAmount")),
        .TaxGroup = If(IsDBNull(row("TaxGroup")), Nothing, row("TaxGroup")),
        .IncludeInInstallment = If(IsDBNull(row("IncludeInInstallment")), Nothing, row("IncludeInInstallment")),
        .SpreadAcrossInstallment = If(IsDBNull(row("SpreadAcrossInstallment")), Nothing, row("SpreadAcrossInstallment")),
        .IsValue = If(IsDBNull(row("IsValue")), Nothing, row("IsValue")),
        .RiskFeeKey = If(IsDBNull(row("RiskFeeKey")), Nothing, row("RiskFeeKey"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetHeaderAndRiskTaxByKey(dtResultSet As DataTable) As List(Of BaseGetHeaderAndRiskTaxByKeyResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetHeaderAndRiskTaxByKeyResponseTypeRow With {
        .TaxGroup = If(IsDBNull(row("TaxGroup")), Nothing, row("TaxGroup")),
        .Sequence = If(IsDBNull(row("Sequence")), Nothing, row("Sequence")),
        .TaxBand = If(IsDBNull(row("TaxBand")), Nothing, row("TaxBand")),
        .TaxAmount = If(IsDBNull(row("TaxAmount")), Nothing, row("TaxAmount")),
        .CalculationBasis = If(IsDBNull(row("CalculationBasis")), Nothing, row("CalculationBasis")),
        .Rate = If(IsDBNull(row("Rate")), Nothing, row("Rate")),
        .ClassOfBusiness = If(IsDBNull(row("ClassOfBusiness")), Nothing, row("ClassOfBusiness")),
        .Country = If(IsDBNull(row("Country")), Nothing, row("Country")),
        .State = If(IsDBNull(row("State")), Nothing, row("State")),
        .IsNotAppliedToClient = If(IsDBNull(row("IsNotAppliedToClient")), Nothing, row("IsNotAppliedToClient")),
        .IncludeInInstallment = If(IsDBNull(row("IncludeInInstallment")), Nothing, row("IncludeInInstallment")),
        .SpreadAcrossInstallment = If(IsDBNull(row("SpreadAcrossInstallment")), Nothing, row("SpreadAcrossInstallment")),
        .ApplyTaxBy = If(IsDBNull(row("ApplyTaxBy")), Nothing, row("ApplyTaxBy")),
        .CurrencyCode = If(IsDBNull(row("CurrencyCode")), Nothing, row("CurrencyCode")),
        .IsValue = If(IsDBNull(row("IsValue")), False, row("IsValue"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetHeaderAndSummariesByKey(dtResultSet As DataTable) As List(Of GetHeaderAndSummariesByKeyResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New GetHeaderAndSummariesByKeyResponseTypeRow With {
        .PartyKey = If(IsDBNull(row("PartyKey")), Nothing, row("PartyKey")),
        .IsLead = If(IsDBNull(row("IsLead")), Nothing, row("IsLead")),
        .Correspondence = If(IsDBNull(row("Correspondence")), Nothing, row("Correspondence"))})
        Return myData.ToList()

        ' Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New GetHeaderAndSummariesByKeyResponseTypeRow With { _
        '.PartyKey = IF(IsDBNull(row("PartyKey")), Nothing, row("PartyKey")), _
        '.IsLead = IF(IsDBNull(row("IsLead")), Nothing, row("IsLead")), _
        '.IsLeadSpecified = IF(IsDBNull(row("IsLeadSpecified")), Nothing, row("IsLeadSpecified")), _
        '.Correspondence = IF(IsDBNull(row("Correspondence")), Nothing, row("Correspondence")), _
        '.CorrespondenceSpecified = IF(IsDBNull(row("CorrespondenceSpecified")), Nothing, row("CorrespondenceSpecified"))})
        ' Return myData.ToList()

    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetHeaderAndSummaries(dtResultSet As DataTable) As List(Of BaseGetHeaderAndSummariesResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetHeaderAndSummariesResponseTypeRow With {
        .HasClaimLink = If(IsDBNull(row("HasClaimLink")), Nothing, row("HasClaimLink")),
        .RiskKey = If(IsDBNull(row("RiskKey")), Nothing, row("RiskKey")),
        .RiskFolderKey = If(IsDBNull(row("RiskFolderKey")), Nothing, row("RiskFolderKey")),
        .RiskTypeCode = If(IsDBNull(row("RiskTypeCode")), Nothing, row("RiskTypeCode")),
        .Description = If(IsDBNull(row("Description")), Nothing, row("Description")),
        .TotalSumInsured = If(IsDBNull(row("TotalSumInsured")), Nothing, row("TotalSumInsured")),
        .Premium = If(IsDBNull(row("Premium")), Nothing, row("Premium")),
        .StatusCode = If(IsDBNull(row("StatusCode")), Nothing, row("StatusCode")),
        .GISRetroactiveDate = If(IsDBNull(row("GISRetroactiveDate")), Nothing, row("GISRetroactiveDate")),
        .RiskInceptionDate = If(IsDBNull(row("RiskInceptionDate")), Nothing, row("RiskInceptionDate")),
        .RiskNumber = If(IsDBNull(row("RiskNumber")), Nothing, row("RiskNumber")),
        .RiskLinkStatusFlag = If(IsDBNull(row("RiskLinkStatusFlag")), Nothing, row("RiskLinkStatusFlag")),
        .RiskLinkChangeDate = If(IsDBNull(row("RiskLinkChangeDate")), Nothing, row("RiskLinkChangeDate")),
        .OriginalRiskKey = If(IsDBNull(row("OriginalRiskKey")), Nothing, row("OriginalRiskKey")),
        .IsEditable = If(IsDBNull(row("IsEditable")), Nothing, row("IsEditable")),
        .HasFacProp = If(IsDBNull(row("HasFacProp")), Nothing, row("HasFacProp")),
        .IsAutoRated = If(IsDBNull(row("IsAutoRated")), Nothing, row("IsAutoRated"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetHeaderAndSummariesByRef(dtResultSet As DataTable) As List(Of GetHeaderAndSummariesByRefResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New GetHeaderAndSummariesByRefResponseTypeRow With {
        .PartyKey = If(IsDBNull(row("PartyKey")), Nothing, row("PartyKey")),
        .IsLead = If(IsDBNull(row("IsLead")), Nothing, row("IsLead")),
        .IsLeadSpecified = If(row.Table.Columns.Contains("IsLeadSpecified"), If(IsDBNull(row("IsLeadSpecified")), Nothing, row("IsLeadSpecified")), Nothing),
        .Correspondence = If(IsDBNull(row("Correspondence")), Nothing, row("Correspondence")),
        .CorrespondenceSpecified = If(row.Table.Columns.Contains("CorrespondenceSpecified"), If(IsDBNull(row("CorrespondenceSpecified")), Nothing, row("CorrespondenceSpecified")), Nothing)})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetInstalmentQuotes(dtResultSet As DataTable) As List(Of BaseGetInstalmentQuotesResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetInstalmentQuotesResponseTypeRow With {
        .AlignTo = If(IsDBNull(row("AlignTo")), Nothing, row("AlignTo")),
        .DepositAsInstalment = If(IsDBNull(row("DepositAsInstalment")), Nothing, row("DepositAsInstalment")),
        .BranchCodeMandatory = If(IsDBNull(row("BranchCodeMandatory")), Nothing, row("BranchCodeMandatory")),
        .BranchNameMandatory = If(IsDBNull(row("BranchNameMandatory")), Nothing, row("BranchNameMandatory")),
        .BankNameMandatory = If(IsDBNull(row("BankNameMandatory")), Nothing, row("BankNameMandatory")),
        .BankAddressMandatory = If(IsDBNull(row("BankAddressMandatory")), Nothing, row("BankAddressMandatory")),
        .StartLimit = If(IsDBNull(row("StartLimit")), Nothing, row("StartLimit")),
        .CompanyNo = If(IsDBNull(row("CompanyNo")), Nothing, row("CompanyNo")),
        .CompanyName = If(IsDBNull(row("CompanyName")), Nothing, row("CompanyName")),
        .SchemeNo = If(IsDBNull(row("SchemeNo")), Nothing, row("SchemeNo")),
        .SchemeVersion = If(IsDBNull(row("SchemeVersion")), Nothing, row("SchemeVersion")),
        .SchemeName = If(IsDBNull(row("SchemeName")), Nothing, row("SchemeName")),
        .FrequencyID = If(IsDBNull(row("FrequencyID")), Nothing, row("FrequencyID")),
        .FrequencyDescription = If(IsDBNull(row("FrequencyDescription")), Nothing, row("FrequencyDescription")),
        .MediaTypeID = If(IsDBNull(row("MediaTypeID")), Nothing, row("MediaTypeID")),
        .MediaTypeDescription = If(IsDBNull(row("MediaTypeDescription")), Nothing, row("MediaTypeDescription")),
        .ProductClass = If(IsDBNull(row("ProductClass")), Nothing, row("ProductClass")),
        .ProductCode = If(IsDBNull(row("ProductCode")), Nothing, row("ProductCode")),
        .TotalAmountInput = If(IsDBNull(row("TotalAmountInput")), Nothing, row("TotalAmountInput")),
        .InstalmentsToPay = If(IsDBNull(row("InstalmentsToPay")), Nothing, row("InstalmentsToPay")),
        .FirstInstalmentDate = If(IsDBNull(row("FirstInstalmentDate")), Nothing, row("FirstInstalmentDate")),
        .NextInstalmentDate = If(IsDBNull(row("NextInstalmentDate")), Nothing, row("NextInstalmentDate")),
        .LastInstalmentDate = If(IsDBNull(row("LastInstalmentDate")), Nothing, row("LastInstalmentDate")),
        .FirstInstalmentAmount = If(IsDBNull(row("FirstInstalmentAmount")), Nothing, row("FirstInstalmentAmount")),
        .OtherInstalmentAmount = If(IsDBNull(row("OtherInstalmentAmount")), Nothing, row("OtherInstalmentAmount")),
        .TotalInstalmentsAmount = If(IsDBNull(row("TotalInstalmentsAmount")), Nothing, row("TotalInstalmentsAmount")),
        .AprRate = If(IsDBNull(row("AprRate")), Nothing, row("AprRate")),
        .InterestRate = If(IsDBNull(row("InterestRate")), Nothing, row("InterestRate")),
        .DaysDelay = If(IsDBNull(row("DaysDelay")), Nothing, row("DaysDelay")),
        .DepositAmount = If(IsDBNull(row("DepositAmount")), Nothing, row("DepositAmount")),
        .InterestAmount = If(IsDBNull(row("InterestAmount")), Nothing, row("InterestAmount")),
        .TaxAmount = If(IsDBNull(row("TaxAmount")), Nothing, row("TaxAmount")),
        .FinanceCharge = If(IsDBNull(row("FinanceCharge")), Nothing, row("FinanceCharge")),
        .ProtectionAmount = If(IsDBNull(row("ProtectionAmount")), Nothing, row("ProtectionAmount")),
        .OriginalOtherInstalmentAmount = If(IsDBNull(row("OriginalOtherInstalmentAmount")), Nothing, row("OriginalOtherInstalmentAmount")),
        .HighlightCell = If(IsDBNull(row("HighlightCell")), Nothing, row("HighlightCell")),
        .SchemeTypeCode = If(IsDBNull(row("SchemeTypeCode")), Nothing, row("SchemeTypeCode")),
        .MediaTypeValidation = If(IsDBNull(row("MediaTypeValidation")), Nothing, row("MediaTypeValidation")),
        .FrequencyPerYear = If(IsDBNull(row("FrequencyPerYear")), Nothing, row("FrequencyPerYear")),
        .PFRF_ID = If(IsDBNull(row("PFRF_ID")), Nothing, row("PFRF_ID")),
        .FrequencyPeriod = If(IsDBNull(row("FrequencyPeriod")), Nothing, row("FrequencyPeriod")),
        .FrequencyAmount = If(IsDBNull(row("FrequencyAmount")), Nothing, row("FrequencyAmount")),
        .OriginalAmount = If(IsDBNull(row("OriginalAmount")), Nothing, row("OriginalAmount")),
        .ClaimDebtID = If(IsDBNull(row("ClaimDebtID")), Nothing, row("ClaimDebtID")),
        .UserID = If(IsDBNull(row("UserID")), Nothing, row("UserID")),
        .AgentCnt = If(IsDBNull(row("AgentCnt")), Nothing, row("AgentCnt")),
        .AgentRef = If(IsDBNull(row("AgentRef")), Nothing, row("AgentRef")),
        .LastInstalmentAmount = If(IsDBNull(row("LastInstalmentAmount")), Nothing, row("LastInstalmentAmount")),
        .Username = If(IsDBNull(row("Username")), Nothing, row("Username")),
        .Password = If(IsDBNull(row("Password")), Nothing, row("Password")),
        .BrokerID = If(IsDBNull(row("BrokerID")), Nothing, row("BrokerID")),
        .BrokerURL = If(IsDBNull(row("BrokerURL")), Nothing, row("BrokerURL")),
        .Timeout = If(IsDBNull(row("Timeout")), Nothing, row("Timeout")),
        .ProviderCode = If(IsDBNull(row("ProviderCode")), Nothing, row("ProviderCode")),
        .Terms = If(IsDBNull(row("Terms")), Nothing, row("Terms")),
        .Ref = If(IsDBNull(row("Ref")), Nothing, row("Ref")),
        .OriginalRate = If(IsDBNull(row("OriginalRate")), Nothing, row("OriginalRate")),
        .RefundType = If(IsDBNull(row("RefundType")), Nothing, row("RefundType")),
        .MinMTA = If(IsDBNull(row("MinMTA")), Nothing, row("MinMTA")),
        .SingleInstalmentPerMonth = If(IsDBNull(row("SingleInstalmentPerMonth")), Nothing, row("SingleInstalmentPerMonth")),
        .FirstInstalmentAlignWithDayInMonth = If(IsDBNull(row("FirstInstalmentAlignWithDayInMonth")), Nothing, row("FirstInstalmentAlignWithDayInMonth")),
        .UseTransCurrncy = If(IsDBNull(row("UseTransCurrncy")), Nothing, row("UseTransCurrncy")),
        .FinanceToNet = If(IsDBNull(row("FinanceToNet")), Nothing, row("FinanceToNet"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetPoliciesForRenewalSelection(dtResultSet As DataTable) As List(Of BaseGetPoliciesForRenewalSelectionResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetPoliciesForRenewalSelectionResponseTypeRow With {
        .InsuranceFileKey = If(IsDBNull(row("InsuranceFileKey")), Nothing, row("InsuranceFileKey")),
        .InsuranceFolderKey = If(IsDBNull(row("InsuranceFolderKey")), Nothing, row("InsuranceFolderKey")),
        .PartyKey = If(IsDBNull(row("PartyKey")), Nothing, row("PartyKey")),
        .ProductKey = If(IsDBNull(row("ProductKey")), Nothing, row("ProductKey")),
        .LeadAgentKey = If(IsDBNull(row("LeadAgentKey")), Nothing, row("LeadAgentKey")),
        .InsuranceFileRef = If(IsDBNull(row("InsuranceFileRef")), Nothing, row("InsuranceFileRef")),
        .CoverStartDate = If(IsDBNull(row("CoverStartDate")), Nothing, row("CoverStartDate")),
        .CoverEndDate = If(IsDBNull(row("CoverEndDate")), Nothing, row("CoverEndDate")),
        .ClientCode = If(IsDBNull(row("ClientCode")), Nothing, row("ClientCode")),
        .Client = If(IsDBNull(row("Client")), Nothing, row("Client")),
        .LeadAgent = If(IsDBNull(row("LeadAgent")), Nothing, row("LeadAgent")),
        .IsAutoRenewable = If(IsDBNull(row("IsAutoRenewable")), Nothing, row("IsAutoRenewable")),
        .ProductDescription = If(IsDBNull(row("ProductDescription")), Nothing, row("ProductDescription")),
        .RenewalDate = If(IsDBNull(row("RenewalDate")), Nothing, row("RenewalDate")),
        .IsClosed = If(IsDBNull(row("IsClosed")), Nothing, row("IsClosed")),
        .IsInTransferMode = If(IsDBNull(row("IsInTransferMode")), Nothing, row("IsInTransferMode")),
        .IsTrueMonthlyPolicy = If(IsDBNull(row("IsTrueMonthlyPolicy")), Nothing, row("IsTrueMonthlyPolicy")),
        .AnniversaryCopy = If(IsDBNull(row("AnniversaryCopy")), Nothing, row("AnniversaryCopy")),
        .RenewalCount = If(IsDBNull(row("RenewalCount")), Nothing, row("RenewalCount"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetPoliciesInRenewal(dtResultSet As DataTable) As List(Of BaseGetPoliciesInRenewalResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetPoliciesInRenewalResponseTypeRow With {
        .RenewalStatusKey = If(IsDBNull(row("RenewalStatusKey")), Nothing, row("RenewalStatusKey")),
        .PartyKey = If(IsDBNull(row("PartyKey")), Nothing, row("PartyKey")),
        .BranchCode = If(IsDBNull(row("BranchCode")), Nothing, row("BranchCode")),
        .PartyName = If(IsDBNull(row("PartyName")), Nothing, row("PartyName")),
        .InsuranceFileRef = If(IsDBNull(row("InsuranceFileRef")), Nothing, row("InsuranceFileRef")),
        .InsuranceFileKey = If(IsDBNull(row("InsuranceFileKey")), Nothing, row("InsuranceFileKey")),
        .InsuranceFolderKey = If(IsDBNull(row("InsuranceFolderKey")), Nothing, row("InsuranceFolderKey")),
        .InsuranceFileStatusDescription = If(IsDBNull(row("InsuranceFileStatusDescription")), Nothing, row("InsuranceFileStatusDescription")),
        .InsuranceFileTypeDescription = If(IsDBNull(row("InsuranceFileTypeDescription")), Nothing, row("InsuranceFileTypeDescription")),
        .RenewalStatusTypeCode = If(IsDBNull(row("RenewalStatusTypeCode")), Nothing, row("RenewalStatusTypeCode")),
        .RenewalStatusTypeDescription = If(IsDBNull(row("RenewalStatusTypeDescription")), Nothing, row("RenewalStatusTypeDescription")),
        .CoverStartDate = If(IsDBNull(row("CoverStartDate")), Nothing, row("CoverStartDate")),
        .CoverEndDate = If(IsDBNull(row("CoverEndDate")), Nothing, row("CoverEndDate")),
        .RenewalDate = If(IsDBNull(row("RenewalDate")), Nothing, row("RenewalDate")),
        .RenewalPremium = If(IsDBNull(row("RenewalPremium")), Nothing, row("RenewalPremium")),
        .ProductCode = If(IsDBNull(row("ProductCode")), Nothing, row("ProductCode")),
        .ProductDescription = If(IsDBNull(row("ProductDescription")), Nothing, row("ProductDescription")),
        .LeadAgentKey = If(IsDBNull(row("LeadAgentKey")), Nothing, row("LeadAgentKey")),
        .LeadAgent = If(IsDBNull(row("LeadAgent")), Nothing, row("LeadAgent")),
        .AccHandler = If(IsDBNull(row("AccHandler")), Nothing, row("AccHandler")),
        .ClaimIndicator = If(IsDBNull(row("ClaimIndicator")), Nothing, row("ClaimIndicator")),
        .IsClosed = If(IsDBNull(row("IsClosed")), Nothing, row("IsClosed")),
        .IsTrueMonthlyPolicy = If(IsDBNull(row("IsTrueMonthlyPolicy")), Nothing, row("IsTrueMonthlyPolicy")),
        .AnniversaryCopy = If(IsDBNull(row("AnniversaryCopy")), Nothing, row("AnniversaryCopy")),
        .IsMigratedPolicy = If(IsDBNull(row("IsMigratedPolicy")), Nothing, row("IsMigratedPolicy")),
        .IsMarketPlacePolicy = If(IsDBNull(row("IsMarketPlacePolicy")), Nothing, row("IsMarketPlacePolicy")),
        .AssociatedClients = If(IsDBNull(row("AssociatedClients")), Nothing, row("AssociatedClients"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetPoliciesOnBankGuaranteeByKey(dtResultSet As DataTable) As List(Of BaseGetPoliciesOnBankGuaranteeByKeyResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetPoliciesOnBankGuaranteeByKeyResponseTypeRow With {
        .InsuranceFileKey = If(IsDBNull(row("InsuranceFileKey")), Nothing, row("InsuranceFileKey")),
        .ClientCode = If(IsDBNull(row("ClientCode")), Nothing, row("ClientCode")),
        .ClientName = If(IsDBNull(row("ClientName")), Nothing, row("ClientName")),
        .PolicyRef = If(IsDBNull(row("PolicyRef")), Nothing, row("PolicyRef")),
        .AgentCode = If(IsDBNull(row("AgentCode")), Nothing, row("AgentCode")),
        .BranchDesc = If(IsDBNull(row("BranchDesc")), Nothing, row("BranchDesc")),
        .ProductDesc = If(IsDBNull(row("ProductDesc")), Nothing, row("ProductDesc")),
        .PremiumAmount = If(IsDBNull(row("PremiumAmount")), Nothing, row("PremiumAmount")),
        .CoverStartDate = If(IsDBNull(row("CoverStartDate")), Nothing, row("CoverStartDate")),
        .CoverEndDate = If(IsDBNull(row("CoverEndDate")), Nothing, row("CoverEndDate"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetPoliciesOnBankGuaranteeForReceipt(dtResultSet As DataTable) As List(Of BaseGetPoliciesOnBankGuaranteeForReceiptResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetPoliciesOnBankGuaranteeForReceiptResponseTypeRow With {
        .BGKey = If(IsDBNull(row("BGKey")), Nothing, row("BGKey")),
        .BankNameKey = If(IsDBNull(row("BankNameKey")), Nothing, row("BankNameKey")),
        .BankName = If(IsDBNull(row("BankName")), Nothing, row("BankName")),
        .BankGuaranteeRef = If(IsDBNull(row("BankGuaranteeRef")), Nothing, row("BankGuaranteeRef")),
        .BGDueDate = If(IsDBNull(row("BGDueDate")), Nothing, row("BGDueDate")),
        .PolicyKey = If(IsDBNull(row("PolicyKey")), Nothing, row("PolicyKey")),
        .PolicyRef = If(IsDBNull(row("PolicyRef")), Nothing, row("PolicyRef")),
        .PremiumAmount = If(IsDBNull(row("PremiumAmount")), Nothing, row("PremiumAmount")),
        .BranchCode = If(IsDBNull(row("BranchCode")), Nothing, row("BranchCode")),
        .BranchDesc = If(IsDBNull(row("BranchDesc")), Nothing, row("BranchDesc")),
        .ProductCode = If(IsDBNull(row("ProductCode")), Nothing, row("ProductCode")),
        .ProductDesc = If(IsDBNull(row("ProductDesc")), Nothing, row("ProductDesc")),
        .OutstandingPolicyAmt = If(IsDBNull(row("OutstandingPolicyAmt")), Nothing, row("OutstandingPolicyAmt")),
        .CoverStartDate = If(IsDBNull(row("CoverStartDate")), Nothing, row("CoverStartDate")),
        .CoverEndDate = If(IsDBNull(row("CoverEndDate")), Nothing, row("CoverEndDate"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetPolicyBankGuarantee(dtResultSet As DataTable) As List(Of BaseGetPolicyBankGuaranteeResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetPolicyBankGuaranteeResponseTypeRow With {
        .BGKey = If(IsDBNull(row("BGKey")), Nothing, row("BGKey")),
        .BankNameKey = If(IsDBNull(row("BankNameKey")), Nothing, row("BankNameKey")),
        .BankName = If(IsDBNull(row("BankName")), Nothing, row("BankName")),
        .BankGuaranteeRef = If(IsDBNull(row("BankGuaranteeRef")), Nothing, row("BankGuaranteeRef")),
        .BGLimit = If(IsDBNull(row("BGLimit")), Nothing, row("BGLimit")),
        .AvailableBalance = If(IsDBNull(row("AvailableBalance")), Nothing, row("AvailableBalance")),
        .ExpiryDate = If(IsDBNull(row("ExpiryDate")), Nothing, row("ExpiryDate")),
        .ClientShortName = If(IsDBNull(row("ClientShortName")), Nothing, row("ClientShortName")),
        .ClientName = If(IsDBNull(row("ClientName")), Nothing, row("ClientName")),
        .DueDate = If(IsDBNull(row("DueDate")), Nothing, row("DueDate"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetQuotesMarkedForCollection(dtResultSet As DataTable) As List(Of BaseGetQuotesMarkedForCollectionResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetQuotesMarkedForCollectionResponseTypeRow With {
        .InsuranceFileKey = If(IsDBNull(row("InsuranceFileKey")), Nothing, row("InsuranceFileKey")),
        .InsuranceFileRef = If(IsDBNull(row("InsuranceFileRef")), Nothing, row("InsuranceFileRef")),
        .PartyKey = If(IsDBNull(row("PartyKey")), Nothing, row("PartyKey")),
        .PartyName = If(IsDBNull(row("PartyName")), Nothing, row("PartyName")),
        .AgentKey = If(IsDBNull(row("AgentKey")), Nothing, row("AgentKey")),
        .AgentName = If(IsDBNull(row("AgentName")), Nothing, row("AgentName")),
        .ProductKey = If(IsDBNull(row("ProductKey")), Nothing, row("ProductKey")),
        .ProductCode = If(IsDBNull(row("ProductCode")), Nothing, row("ProductCode")),
        .BranchKey = If(IsDBNull(row("BranchKey")), Nothing, row("BranchKey")),
        .BranchCode = If(IsDBNull(row("BranchCode")), Nothing, row("BranchCode")),
        .CurrencyKey = If(IsDBNull(row("CurrencyKey")), Nothing, row("CurrencyKey")),
        .CurrencyCode = If(IsDBNull(row("CurrencyCode")), Nothing, row("CurrencyCode")),
        .Premium = If(IsDBNull(row("Premium")), Nothing, row("Premium")),
        .InsuranceFileTypeCode = If(IsDBNull(row("InsuranceFileTypeCode")), Nothing, row("InsuranceFileTypeCode")),
        .AgentTypeCode = If(IsDBNull(row("AgentTypeCode")), Nothing, row("AgentTypeCode")),
        .AgentCommission = If(IsDBNull(row("AgentCommission")), Nothing, row("AgentCommission"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetStandardPolicyWordings(dtResultSet As DataTable) As List(Of BaseGetStandardPolicyWordingsResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetStandardPolicyWordingsResponseTypeRow With {
        .DocumentTemplateId = If(IsDBNull(row("DocumentTemplateId")), Nothing, row("DocumentTemplateId")),
        .Code = If(IsDBNull(row("Code")), Nothing, row("Code")),
        .Description = If(IsDBNull(row("Description")), Nothing, row("Description"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetTaxes(dtResultSet As DataTable) As List(Of BaseGetTaxesResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetTaxesResponseTypeRow With {
        .TaxCalculationKey = If(IsDBNull(row("TaxCalculationKey")), Nothing, row("TaxCalculationKey")),
        .RiskKey = If(IsDBNull(row("RiskKey")), Nothing, row("RiskKey")),
        .TaxBandKey = If(IsDBNull(row("TaxBandKey")), Nothing, row("TaxBandKey")),
        .Premium = If(IsDBNull(row("Premium")), Nothing, row("Premium")),
        .IsValue = If(IsDBNull(row("IsValue")), Nothing, row("IsValue")),
        .TaxPercentage = If(IsDBNull(row("TaxPercentage")), Nothing, row("TaxPercentage")),
        .TaxValue = If(IsDBNull(row("TaxValue")), Nothing, row("TaxValue")),
        .IsManuallyChanged = If(IsDBNull(row("IsManuallyChanged")), Nothing, row("IsManuallyChanged")),
        .CalculationBasis = If(IsDBNull(row("CalculationBasis")), Nothing, row("CalculationBasis")),
        .BasisValue = If(IsDBNull(row("BasisValue")), Nothing, row("BasisValue")),
        .SumInsured = If(IsDBNull(row("SumInsured")), Nothing, row("SumInsured")),
        .SumInsuredRounded = If(IsDBNull(row("SumInsuredRounded")), Nothing, row("SumInsuredRounded")),
        .CurrencyKey = If(IsDBNull(row("CurrencyKey")), Nothing, row("CurrencyKey")),
        .AllowTaxCredit = If(IsDBNull(row("AllowTaxCredit")), Nothing, row("AllowTaxCredit")),
        .OriginalSumInsured = If(IsDBNull(row("OriginalSumInsured")), Nothing, row("OriginalSumInsured")),
        .CountryKey = If(IsDBNull(row("CountryKey")), Nothing, row("CountryKey")),
        .StateKey = If(IsDBNull(row("StateKey")), Nothing, row("StateKey")),
        .ClassOfBusinessKey = If(IsDBNull(row("ClassOfBusinessKey")), Nothing, row("ClassOfBusinessKey")),
        .TaxGroupKey = If(IsDBNull(row("TaxGroupKey")), Nothing, row("TaxGroupKey")),
        .Sequence = If(IsDBNull(row("Sequence")), Nothing, row("Sequence")),
        .PolicyFeeUKey = If(IsDBNull(row("PolicyFeeUKey")), Nothing, row("PolicyFeeUKey")),
        .AgentCommissionKey = If(IsDBNull(row("AgentCommissionKey")), Nothing, row("AgentCommissionKey")),
        .RIPartyKey = If(IsDBNull(row("RIPartyKey")), Nothing, row("RIPartyKey")),
        .RIArrangementLineKey = If(IsDBNull(row("RIArrangementLineKey")), Nothing, row("RIArrangementLineKey")),
        .InsuranceSectionKey = If(IsDBNull(row("InsuranceSectionKey")), Nothing, row("InsuranceSectionKey")),
        .PolicyFeeKey = If(IsDBNull(row("PolicyFeeKey")), Nothing, row("PolicyFeeKey")),
        .PolicyAgentsKey = If(IsDBNull(row("PolicyAgentsKey")), Nothing, row("PolicyAgentsKey")),
        .InsurerPartyKey = If(IsDBNull(row("InsurerPartyKey")), Nothing, row("InsurerPartyKey")),
        .ClaimPerilKey = If(IsDBNull(row("ClaimPerilKey")), Nothing, row("ClaimPerilKey")),
        .ClaimPaymentKey = If(IsDBNull(row("ClaimPaymentKey")), Nothing, row("ClaimPaymentKey")),
        .ClaimReceiptKey = If(IsDBNull(row("ClaimReceiptKey")), Nothing, row("ClaimReceiptKey")),
        .ClaimPaymentItemKey = If(IsDBNull(row("ClaimPaymentItemKey")), Nothing, row("ClaimPaymentItemKey")),
        .ClaimReceiptItemKey = If(IsDBNull(row("ClaimReceiptItemKey")), Nothing, row("ClaimReceiptItemKey")),
        .IsNotAppliedToClient = If(IsDBNull(row("IsNotAppliedToClient")), Nothing, row("IsNotAppliedToClient")),
        .IncludeTaxInInstalments = If(IsDBNull(row("IncludeTaxInInstalments")), Nothing, row("IncludeTaxInInstalments")),
        .SpreadTaxAcrossInstalments = If(IsDBNull(row("SpreadTaxAcrossInstalments")), Nothing, row("SpreadTaxAcrossInstalments")),
        .BaseTaxCalculationKey = If(IsDBNull(row("BaseTaxCalculationKey")), Nothing, row("BaseTaxCalculationKey")),
        .VersionKey = If(IsDBNull(row("VersionKey")), Nothing, row("VersionKey")),
        .PfPremFinanceKey = If(IsDBNull(row("PfPremFinanceKey")), Nothing, row("PfPremFinanceKey")),
        .PfPremFinanceVersion = If(IsDBNull(row("PfPremFinanceVersion")), Nothing, row("PfPremFinanceVersion")),
        .PolicyCoinsurersSectionKey = If(IsDBNull(row("PolicyCoinsurersSectionKey")), Nothing, row("PolicyCoinsurersSectionKey")),
        .IsCommissionTax = If(IsDBNull(row("IsCommissionTax")), Nothing, row("IsCommissionTax")),
        .ApplyTaxBy = If(IsDBNull(row("ApplyTaxBy")), Nothing, row("ApplyTaxBy")),
        .TaxBandRateKey = If(IsDBNull(row("TaxBandRateKey")), Nothing, row("TaxBandRateKey")),
        .IsSuspended = If(IsDBNull(row("IsSuspended")), Nothing, row("IsSuspended")),
        .TransType = If(IsDBNull(row("TransType")), Nothing, row("TransType")),
        .TaxBandCode = If(IsDBNull(row("TaxBandCode")), Nothing, row("TaxBandCode")),
        .TaxBandDescription = If(IsDBNull(row("TaxBandDescription")), Nothing, row("TaxBandDescription")),
        .TaxGroupCode = If(IsDBNull(row("TaxGroupCode")), Nothing, row("TaxGroupCode")),
        .TaxGroupDescription = If(IsDBNull(row("TaxGroupDescription")), Nothing, row("TaxGroupDescription")),
        .TaxBandRateDescription = If(IsDBNull(row("TaxBandRateDescription")), Nothing, row("TaxBandRateDescription"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetCoinsuranceDefaults(dtResultSet As DataTable) As List(Of BaseGetCoinsuranceDefaultsResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetCoinsuranceDefaultsResponseTypeRow With {
        .CoinsuranceDefaultId = If(IsDBNull(row("CoinsuranceDefaultId")), Nothing, row("CoinsuranceDefaultId")),
        .CoinsuranceDefault = If(IsDBNull(row("CoinsuranceDefault")), Nothing, row("CoinsuranceDefault")),
        .Code = If(IsDBNull(row("Code")), Nothing, row("Code")),
        .IsRecovered = If(IsDBNull(row("IsRecovered")), Nothing, row("IsRecovered")),
        .IsSurcharged = If(IsDBNull(row("IsSurcharged")), Nothing, row("IsSurcharged"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetCoinsuranceValues(dtResultSet As DataTable) As List(Of BaseGetCoinsuranceValuesResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetCoinsuranceValuesResponseTypeRow With {
        .CoInsurerKey = If(IsDBNull(row("CoInsurerKey")), Nothing, row("CoInsurerKey")),
        .CoInsurer = If(IsDBNull(row("CoInsurer")), Nothing, row("CoInsurer")),
        .ArrangementRef = If(IsDBNull(row("ArrangementRef")), Nothing, row("ArrangementRef")),
        .SharePerc = If(IsDBNull(row("SharePerc")), Nothing, row("SharePerc")),
        .CommissionPerc = If(IsDBNull(row("CommissionPerc")), Nothing, row("CommissionPerc"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetProductRiskEvents(dtResultSet As DataTable) As List(Of BaseGetProductRiskEventsResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetProductRiskEventsResponseTypeRow With {
        .EventKey = If(IsDBNull(row("EventKey")), Nothing, row("EventKey")),
        .EventCode = If(IsDBNull(row("EventCode")), Nothing, row("EventCode")),
        .EventDescription = If(IsDBNull(row("EventDescription")), Nothing, row("EventDescription")),
        .IsDefault = If(IsDBNull(row("IsDefault")), Nothing, row("IsDefault"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetRiskReinsuranceArrangementLines(dtResultSet As DataTable) As List(Of BaseGetRiskReinsuranceArrangementLinesResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetRiskReinsuranceArrangementLinesResponseTypeRow With {
        .PartyKey = If(IsDBNull(row("PartyKey")), 0, row("PartyKey")),
        .RIArrangementLineKey = If(IsDBNull(row("RIArrangementLineKey")), 0, row("RIArrangementLineKey")),
        .TreatyCode = If(IsDBNull(row("TreatyCode")), 0, row("TreatyCode")),
        .Name = If(IsDBNull(row("Name")), Nothing, row("Name")),
        .DefaultPerc = If(IsDBNull(row("DefaultPerc")), Nothing, row("DefaultPerc")),
        .ThisPerc = If(IsDBNull(row("ThisPerc")), Nothing, row("ThisPerc")),
        .SumInsured = If(IsDBNull(row("SumInsured")), Nothing, row("SumInsured")),
        .Premium = If(IsDBNull(row("Premium")), Nothing, row("Premium")),
        .Tax = If(IsDBNull(row("Tax")), Nothing, row("Tax")),
        .CommissionPerc = If(IsDBNull(row("CommissionPerc")), Nothing, row("CommissionPerc")),
        .Commission = If(IsDBNull(row("Commission")), Nothing, row("Commission")),
        .CommissionTax = If(IsDBNull(row("CommissionTax")), Nothing, row("CommissionTax")),
        .Agreement = If(IsDBNull(row("Agreement")), Nothing, row("Agreement")),
        .Type = If(IsDBNull(row("Type")), Nothing, row("Type")),
        .IsObligatory = If(IsDBNull(row("IsObligatory")), Nothing, row("IsObligatory")),
        .DefaultLine = If(IsDBNull(row("DefaultLine")), Nothing, row("DefaultLine"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetRiskReinsuranceArrangements(dtResultSet As DataTable) As List(Of BaseGetRiskReinsuranceArrangementsResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetRiskReinsuranceArrangementsResponseTypeRow With {
        .ArrangementId = If(IsDBNull(row("ArrangementId")), Nothing, row("ArrangementId")),
        .BandId = If(IsDBNull(row("BandId")), Nothing, row("BandId")),
        .ModelId = If(IsDBNull(row("ModelId")), Nothing, row("ModelId")),
        .SumInsured = If(IsDBNull(row("SumInsured")), Nothing, row("SumInsured")),
        .Premium = If(IsDBNull(row("Premium")), Nothing, row("Premium")),
        .IsOriginal = If(IsDBNull(row("IsOriginal")), Nothing, row("IsOriginal")),
        .IsModified = If(IsDBNull(row("IsModified")), Nothing, row("IsModified")),
        .FACPremiumType = If(IsDBNull(row("FACPremiumType")), Nothing, row("FACPremiumType")),
        .RIModelCode = If(IsDBNull(row("RIModelCode")), Nothing, row("RIModelCode")),
        .ExtendedLimitAmount = If(IsDBNull(row("ExtendedLimitAmount")), Nothing, row("ExtendedLimitAmount")),
        .XOLRIModelID = If(IsDBNull(row("XOLRIModelID")), Nothing, row("XOLRIModelID")),
        .XOLRIModelCode = If(IsDBNull(row("XOLRIModelCode")), Nothing, row("XOLRIModelCode")),
        .IsExtendedLimitApplied = If(IsDBNull(row("IsExtendedLimitApplied")), Nothing, row("IsExtendedLimitApplied")),
        .RiOverrideReasonId = If(IsDBNull(row("RiOverrideReasonId")), Nothing, row("RiOverrideReasonId"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetRiskReinsuranceBands(dtResultSet As DataTable) As List(Of BaseGetRiskReinsuranceBandsResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetRiskReinsuranceBandsResponseTypeRow With {
        .BandKey = If(IsDBNull(row("BandKey")), Nothing, row("BandKey")),
        .Band = If(IsDBNull(row("Band")), Nothing, row("Band"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetRIModelLineDetailsLines(dtResultSet As DataTable) As List(Of BaseGetRIModelLineDetailsResponseTypeLinesRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetRIModelLineDetailsResponseTypeLinesRow With {
        .RIModelLineKey = If(IsDBNull(row("RIModelLineKey")), Nothing, row("RIModelLineKey")),
        .RIModelKey = If(IsDBNull(row("RIModelKey")), Nothing, row("RIModelKey")),
        .Priority = If(IsDBNull(row("Priority")), Nothing, row("Priority")),
        .NoOfLines = If(IsDBNull(row("NoOfLines")), Nothing, row("NoOfLines")),
        .LineLimit = If(IsDBNull(row("LineLimit")), Nothing, row("LineLimit")),
        .TreatyCode = If(IsDBNull(row("TreatyCode")), Nothing, row("TreatyCode")),
        .TreatyKey = If(IsDBNull(row("TreatyKey")), Nothing, row("TreatyKey")),
        .Description = If(IsDBNull(row("Description")), Nothing, row("Description")),
        .SharePercent = If(IsDBNull(row("SharePercent")), Nothing, row("SharePercent")),
        .LowerLimit = If(IsDBNull(row("LowerLimit")), Nothing, row("LowerLimit")),
        .CedingRate = If(IsDBNull(row("CedingRate")), Nothing, row("CedingRate")),
        .TreatyTypeKey = If(IsDBNull(row("TreatyTypeKey")), Nothing, row("TreatyTypeKey")),
        .TreatyTypeCode = If(IsDBNull(row("TreatyTypeCode")), Nothing, row("TreatyTypeCode")),
        .ReinsuranceTypeKey = If(IsDBNull(row("ReinsuranceTypeKey")), Nothing, row("ReinsuranceTypeKey")),
        .ReinsuranceTypeCode = If(IsDBNull(row("ReinsuranceTypeCode")), Nothing, row("ReinsuranceTypeCode")),
        .CedePremiumOnly = If(IsDBNull(row("CedePremiumOnly")), Nothing, row("CedePremiumOnly")),
        .EffectiveDate = If(IsDBNull(row("EffectiveDate")), Nothing, row("EffectiveDate")),
        .ExpiryDate = If(IsDBNull(row("ExpiryDate")), Nothing, row("ExpiryDate"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_FindReinsurerReinsurers(dtResultSet As DataTable) As List(Of BaseFindReinsurerResponseTypeReinsurersRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseFindReinsurerResponseTypeReinsurersRow With {
        .ReinsurerKey = If(IsDBNull(row("ReinsurerKey")), Nothing, row("ReinsurerKey")),
        .ReinsurerCode = If(IsDBNull(row("ReinsurerCode")), Nothing, row("ReinsurerCode")),
        .RIName = If(IsDBNull(row("RIName")), Nothing, row("RIName")),
        .BranchCode = If(IsDBNull(row("BranchCode")), Nothing, row("BranchCode")),
        .BranchName = If(IsDBNull(row("BranchName")), Nothing, row("BranchName")),
        .AccountType = If(IsDBNull(row("AccountType")), Nothing, row("AccountType")),
        .ReinsuranceTypeCode = If(IsDBNull(row("ReinsuranceTypeCode")), Nothing, row("ReinsuranceTypeCode")),
        .IsRetained = If(IsDBNull(row("IsRetained")), Nothing, row("IsRetained")),
        .IsBroker = If(IsDBNull(row("IsBroker")), Nothing, row("IsBroker")),
        .TaxNumber = If(IsDBNull(row("TaxNumber")), Nothing, row("TaxNumber")),
        .IsDomiciledForTax = If(IsDBNull(row("IsDomiciledForTax")), Nothing, row("IsDomiciledForTax")),
        .IsTaxExempt = If(IsDBNull(row("IsTaxExempt")), Nothing, row("IsTaxExempt")),
        .TaxPercentage = If(IsDBNull(row("TaxPercentage")), Nothing, row("TaxPercentage")),
        .TaxGroupCode = If(IsDBNull(row("TaxGroupCode")), Nothing, row("TaxGroupCode")),
        .Address1 = If(IsDBNull(row("Address1")), Nothing, row("Address1")),
        .Address2 = If(IsDBNull(row("Address2")), Nothing, row("Address2")),
        .PostalCode = If(IsDBNull(row("PostalCode")), Nothing, row("PostalCode")),
        .DefaultCommissionPercentage = If(IsDBNull(row("DefaultCommissionPercentage")), Nothing, row("DefaultCommissionPercentage"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetTreatyPartyDetailsParties(dtResultSet As DataTable) As List(Of BaseGetTreatyPartyDetailsResponseTypePartiesRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetTreatyPartyDetailsResponseTypePartiesRow With {
        .TreatyPartyKey = If(IsDBNull(row("TreatyPartyKey")), Nothing, row("TreatyPartyKey")),
        .PartyKey = If(IsDBNull(row("PartyKey")), Nothing, row("PartyKey")),
        .ResolvedName = If(IsDBNull(row("ResolvedName")), Nothing, row("ResolvedName")),
        .TreatyKey = If(IsDBNull(row("TreatyKey")), Nothing, row("TreatyKey")),
        .SharePercent = If(IsDBNull(row("SharePercent")), Nothing, row("SharePercent")),
        .CommissionPercent = If(IsDBNull(row("CommissionPercent")), Nothing, row("CommissionPercent")),
        .IsDomiciledForTax = If(IsDBNull(row("IsDomiciledForTax")), Nothing, row("IsDomiciledForTax")),
        .TaxGroupKey = If(IsDBNull(row("TaxGroupKey")), Nothing, row("TaxGroupKey")),
        .Description = If(IsDBNull(row("Description")), Nothing, row("Description")),
        .IsReinsurerApproved = If(IsDBNull(row("IsReinsurerApproved")), Nothing, row("IsReinsurerApproved"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetRiskByProduct(dtResultSet As DataTable) As List(Of BaseGetRiskByProductResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetRiskByProductResponseTypeRow With {
        .RiskTypeKey = If(IsDBNull(row("RiskTypeKey")), Nothing, row("RiskTypeKey")),
        .RiskTypeCode = If(IsDBNull(row("RiskTypeCode")), Nothing, row("RiskTypeCode")),
        .Description = If(IsDBNull(row("Description")), Nothing, row("Description")),
        .ScreenKey = If(IsDBNull(row("ScreenKey")), Nothing, row("ScreenKey")),
        .DataModelCode = If(IsDBNull(row("DataModelCode")), Nothing, row("DataModelCode")),
        .ScreenCode = If(IsDBNull(row("ScreenCode")), Nothing, row("ScreenCode"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetSubAgents(dtResultSet As DataTable) As List(Of BaseGetSubAgentsResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetSubAgentsResponseTypeRow With {
        .PartyKey = If(IsDBNull(row("PartyKey")), Nothing, row("PartyKey")),
        .Code = If(IsDBNull(row("Code")), Nothing, row("Code")),
        .Name = If(IsDBNull(row("Name")), Nothing, row("Name")),
        .Percentage = If(IsDBNull(row("Percentage")), Nothing, row("Percentage")),
        .Amount = If(IsDBNull(row("Amount")), Nothing, row("Amount"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetRatingDetails(dtResultSet As DataTable) As List(Of BaseGetRatingDetailsResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetRatingDetailsResponseTypeRow With {
        .RatingSectionType = If(IsDBNull(row("RatingSectionType")), Nothing, row("RatingSectionType")),
        .PolicySectionType = If(IsDBNull(row("PolicySectionType")), Nothing, row("PolicySectionType")),
        .RateType = If(IsDBNull(row("RateType")), Nothing, row("RateType")),
        .AnnualRate = If(IsDBNull(row("AnnualRate")), Nothing, row("AnnualRate")),
        .SumInsured = If(IsDBNull(row("SumInsured")), Nothing, row("SumInsured")),
        .ThisPremium = If(IsDBNull(row("ThisPremium")), Nothing, row("ThisPremium")),
        .AnnualPremium = If(IsDBNull(row("AnnualPremium")), Nothing, row("AnnualPremium")),
        .Country = If(IsDBNull(row("Country")), Nothing, row("Country")),
        .State = If(IsDBNull(row("State")), Nothing, row("State")),
        .RatingSectionId = If(IsDBNull(row("RatingSectionId")), Nothing, row("RatingSectionId")),
        .RatingSectionTypeId = If(IsDBNull(row("RatingSectionTypeId")), Nothing, row("RatingSectionTypeId")),
        .PolicySectionTypeId = If(IsDBNull(row("PolicySectionTypeId")), Nothing, row("PolicySectionTypeId")),
        .RateTypeId = If(IsDBNull(row("RateTypeId")), Nothing, row("RateTypeId")),
        .OriginalFlag = If(IsDBNull(row("OriginalFlag")), Nothing, row("OriginalFlag")),
        .CurrencyId = If(IsDBNull(row("CurrencyId")), Nothing, row("CurrencyId")),
        .CountryId = If(IsDBNull(row("CountryId")), Nothing, row("CountryId")),
        .StateId = If(IsDBNull(row("StateId")), Nothing, row("StateId")),
        .IsAmended = If(IsDBNull(row("IsAmended")), Nothing, row("IsAmended")),
        .CalculatedPremium = If(IsDBNull(row("CalculatedPremium")), Nothing, row("CalculatedPremium")),
        .OverrideReason = If(IsDBNull(row("OverrideReason")), Nothing, row("OverrideReason")),
        .EarningPattern = If(IsDBNull(row("EarningPattern")), Nothing, row("EarningPattern")),
        .EarningPatternId = If(IsDBNull(row("EarningPatternId")), Nothing, row("EarningPatternId")),
        .StateCode = If(IsDBNull(row("StateCode")), Nothing, row("StateCode")),
        .CountryCode = If(IsDBNull(row("CountryCode")), Nothing, row("CountryCode")),
        .RatingTypeCode = If(IsDBNull(row("RatingTypeCode")), Nothing, row("RatingTypeCode")),
        .RatingSectionTypeCode = If(IsDBNull(row("RatingSectionTypeCode")), Nothing, row("RatingSectionTypeCode")),
        .EarningPatternCode = If(IsDBNull(row("EarningPatternCode")), Nothing, row("EarningPatternCode")),
        .CurrencyCode = If(IsDBNull(row("CurrencyCode")), Nothing, row("CurrencyCode"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetCoverNoteBook(dtResultSet As DataTable) As List(Of BaseGetCoverNoteBookResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetCoverNoteBookResponseTypeRow With {
        .ProductKey = If(IsDBNull(row("ProductKey")), Nothing, row("ProductKey")),
        .ProductCode = If(IsDBNull(row("ProductCode")), Nothing, row("ProductCode")),
        .Description = If(IsDBNull(row("Description")), Nothing, row("Description")),
        .Chosen = If(IsDBNull(row("Chosen")), Nothing, row("Chosen"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetCoverNoteBook1(dtResultSet As DataTable) As List(Of BaseGetCoverNoteBookResponseTypeRow1)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetCoverNoteBookResponseTypeRow1 With {
        .CoverNoteSheetKey = If(IsDBNull(row("CoverNoteSheetKey")), Nothing, row("CoverNoteSheetKey")),
        .CoverNoteSheetNumber = If(IsDBNull(row("CoverNoteSheetNumber")), Nothing, row("CoverNoteSheetNumber")),
        .CustomerName = If(IsDBNull(row("CustomerName")), Nothing, row("CustomerName")),
        .CoverNoteSheetStatusKey = If(IsDBNull(row("CoverNoteSheetStatusKey")), Nothing, row("CoverNoteSheetStatusKey")),
                .CoverNoteSheetStatusCode = If(IsDBNull(row("CoverNoteSheetStatusCode")), Nothing, row("CoverNoteSheetStatusCode")),
        .CoverNoteSheetStatusDescription = If(IsDBNull(row("CoverNoteSheetStatusDescription")), Nothing, row("CoverNoteSheetStatusDescription")),
        .PolicyNumber = If(IsDBNull(row("PolicyNumber")), Nothing, row("PolicyNumber")),
        .BranchName = If(IsDBNull(row("BranchName")), Nothing, row("BranchName")),
        .AgentName = If(IsDBNull(row("AgentName")), Nothing, row("AgentName")),
        .DateImported = If(IsDBNull(row("DateImported")), Nothing, row("DateImported"))})
        Return myData.ToList()
    End Function
    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetClaimPerilSummary(dtResultSet As DataTable) As List(Of BaseGetClaimPerilSummaryResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetClaimPerilSummaryResponseTypeRow With {
        .Description = If(IsDBNull(row("Description")), Nothing, row("Description")),
        .InitialReserve = If(IsDBNull(row("InitialReserve")), Nothing, row("InitialReserve")),
        .PaidAmount = If(IsDBNull(row("PaidAmount")), Nothing, row("PaidAmount")),
        .RevisedReserve = If(IsDBNull(row("RevisedReserve")), Nothing, row("RevisedReserve")),
        .CurrentReserve = If(IsDBNull(row("CurrentReserve")), Nothing, row("CurrentReserve")),
        .SumInsured = If(IsDBNull(row("SumInsured")), Nothing, row("SumInsured")),
        .Average = If(IsDBNull(row("Average")), Nothing, row("Average"))})
        Return myData.ToList()
    End Function

    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_GetLockDetails(dtResultSet As DataTable) As List(Of BaseGetLockDetailsResponseTypeDetailsRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetLockDetailsResponseTypeDetailsRow With {
        .IsExclusiveLock = If(IsDBNull(row("IsExclusiveLock")), Nothing, row("IsExclusiveLock")),
        .IsSystemLock = If(IsDBNull(row("IsSystemLock")), Nothing, row("IsSystemLock")),
        .Lock2Value = If(IsDBNull(row("Lock2Value")), Nothing, row("Lock2Value")),
        .LockedByID = If(IsDBNull(row("LockedByID")), Nothing, row("LockedByID")),
        .LockedTime = If(IsDBNull(row("LockedTime")), Nothing, row("LockedTime")),
        .LockName = If(IsDBNull(row("LockName")), Nothing, row("LockName")),
        .LockValue = If(IsDBNull(row("LockValue")), Nothing, row("LockValue")),
        .UserName = If(IsDBNull(row("UserName")), Nothing, row("UserName"))})
        Return myData.ToList()
    End Function

    Private Function DataTabletoList_GetTaskOnKeys(dtResultSet As DataTable) As List(Of BaseGetWorkManagerScheduledTasksResponseTypeTasksRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetWorkManagerScheduledTasksResponseTypeTasksRow With {
        .Urgent = If(IsDBNull(row("Urgent")), Nothing, row("Urgent")),
        .TaskStatusKey = If(IsDBNull(row("TaskStatusKey")), Nothing, row("TaskStatusKey")),
        .Type = If(IsDBNull(row("Type")), Nothing, row("Type")),
        .DueDate = If(IsDBNull(row("DueDate")), Nothing, row("DueDate")),
        .Customer = If(IsDBNull(row("Customer")), Nothing, row("Customer")),
        .Description = If(IsDBNull(row("Description")), Nothing, row("Description")),
        .UserGroupKey = If(IsDBNull(row("UserGroupKey")), Nothing, row("UserGroupKey")),
        .UserKey = If(IsDBNull(row("UserKey")), Nothing, row("UserKey")),
        .TaskInstanceKey = If(IsDBNull(row("TaskInstanceKey")), Nothing, row("TaskInstanceKey")),
        .UserCode = If(IsDBNull(row("UserCode")), Nothing, row("UserCode")),
        .UserGroupDescription = If(IsDBNull(row("UserGroupDescription")), Nothing, row("UserGroupDescription")),
        .UserGroupCode = If(IsDBNull(row("UserGroupCode")), Nothing, row("UserGroupCode")),
        .TaskGroupKey = If(IsDBNull(row("TaskGroupKey")), Nothing, row("TaskGroupKey")),
        .TaskKey = If(IsDBNull(row("TaskKey")), Nothing, row("TaskKey")),
        .PartyKey = If(IsDBNull(row("PartyCnt")), Nothing, row("PartyCnt")),
        .IsExternalItem = If(IsDBNull(row("IsExternalItem")), Nothing, row("IsExternalItem")),
        .GuidPMExternalItem = If(IsDBNull(row("GuidPMExternalItem")), Nothing, row("GuidPMExternalItem")),
        .ParentTaskKey = If(IsDBNull(row("ParentTaskKey")), Nothing, row("ParentTaskKey")),
        .PartyName = If(IsDBNull(row("PartyName")), Nothing, row("PartyName"))})
        Return myData.ToList()

    End Function

    ''' <summary>
    ''' Data table to List Conversion
    ''' </summary>
    ''' <param name="dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataTabletoList_AddBackDatedMTCQuote(dtResultSet As DataTable) As List(Of BaseAddBackDatedMTAQuoteResponseTypeRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseAddBackDatedMTAQuoteResponseTypeRow With {
         .InsuranceFileCnt = If(IsDBNull(row("InsuranceFileCnt")), Nothing, row("InsuranceFileCnt")),
         .PolicyType = If(IsDBNull(row("PolicyType")), Nothing, row("PolicyType")),
         .CoverStartDate = If(IsDBNull(row("CoverStartDate")), Nothing, row("CoverStartDate")),
         .CoverEndDate = If(IsDBNull(row("CoverEndDate")), Nothing, row("CoverEndDate")),
         .MTAPremium = If(IsDBNull(row("MTAPremium")), Nothing, row("MTAPremium")),
         .OriginalMTAPremium = If(IsDBNull(row("OriginalMTAPremium")), Nothing, row("OriginalMTAPremium")),
         .PolicyStatus = If(IsDBNull(row("PolicyStatus")), Nothing, row("PolicyStatus")),
         .ReversedInsuranceFileCnt = If(IsDBNull(row("ReversedInsuranceFileCnt")), Nothing, row("ReversedInsuranceFileCnt")),
         .QuoteStatus = If(IsDBNull(row("QuoteStatus")), Nothing, row("QuoteStatus")),
         .OriginalCommission = If(IsDBNull(row("OriginalCommission")), Nothing, row("OriginalCommission")),
         .MTACommission = If(IsDBNull(row("MTACommission")), Nothing, row("MTACommission")),
         .OriginalFee = If(IsDBNull(row("OriginalFee")), Nothing, row("OriginalFee")),
         .MTAFee = If(IsDBNull(row("MTAFee")), Nothing, row("MTAFee"))})
        Return myData.ToList()
    End Function

    Private Function DataTabletoList_GetRIPropTreaties(dtResultSet As DataTable) As List(Of BaseGetRIPropTreatiesResponseTypeTreatiesRow)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetRIPropTreatiesResponseTypeTreatiesRow With {
        .TreatyId = If(IsDBNull(row("TreatyId")), Nothing, row("TreatyId")),
        .TreatyCode = If(IsDBNull(row("TreatyCode")), Nothing, row("TreatyCode")),
        .TreatyDescription = If(IsDBNull(row("TreatyDescription")), Nothing, row("TreatyDescription"))})
        Return myData.ToList()
    End Function

    Private Function DataTabletoList_ListofUnapprovedPayment(dtResultSet As DataTable) As List(Of BaseGetListofUnapprovedPaymentResponseRowType)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetListofUnapprovedPaymentResponseRowType With {
       .Amount = If(IsDBNull(row("Amount")), Nothing, row("Amount")),
       .Assignedto = If(IsDBNull(row("Assignedto")), Nothing, row("Assignedto")),
       .BankAccount = If(IsDBNull(row("BankAccount")), Nothing, row("BankAccount")),
       .BaseCurrencyAmount = If(IsDBNull(row("BaseCurrencyAmount")), Nothing, row("BaseCurrencyAmount")),
       .Branch = If(IsDBNull(row("Branch")), Nothing, row("Branch")),
       .ClaimRef = If(IsDBNull(row("ClaimRef")), Nothing, row("ClaimRef")),
       .CreatedBy = If(IsDBNull(row("CreatedBy")), Nothing, row("CreatedBy")),
       .Currency = If(IsDBNull(row("Currency")), Nothing, row("Currency")),
       .DateAssigned = If(IsDBNull(row("DateAssigned")), Nothing, row("DateAssigned")),
       .MediaRef = If(IsDBNull(row("MediaRef")), Nothing, row("MediaRef")),
       .MediaType = If(IsDBNull(row("MediaType")), Nothing, row("MediaType")),
       .PaymentType = If(IsDBNull(row("PaymentType")), Nothing, row("PaymentType")),
       .PolicyRef = If(IsDBNull(row("PolicyRef")), Nothing, row("PolicyRef")),
       .Status = If(IsDBNull(row("Status")), Nothing, row("Status")),
       .PayeeAccountName = If(IsDBNull(row("PayeeAccountName")), Nothing, row("PayeeAccountName")),
       .CashListId = If(IsDBNull(row("CashListId")), Nothing, row("CashListId")),
       .CashListItemId = If(IsDBNull(row("CashListItemId")), Nothing, row("CashListItemId")),
       .TransactionDate = If(IsDBNull(row("TransactionDate")), Nothing, row("TransactionDate"))})
        Return myData.ToList()
    End Function

    Private Function DataTabletoList_ListofManualJournalTransactions(dtResultSet As DataTable) As List(Of BaseGetListofManualJournalTransactionsResponseRowType)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetListofManualJournalTransactionsResponseRowType With {
       .Amount = If(IsDBNull(row("Amount")), Nothing, row("Amount")),
       .AccountCode = If(IsDBNull(row("AccountCode")), Nothing, row("AccountCode")),
       .Currency = If(IsDBNull(row("Currency")), Nothing, row("Currency")),
       .CurrencyRate = If(IsDBNull(row("CurrencyRate")), Nothing, row("CurrencyRate")),
       .BaseAmountRate = If(IsDBNull(row("BaseCurrencyAmount")), Nothing, row("BaseCurrencyAmount")),
       .AlternateRef = If(IsDBNull(row("AlternateRef")), Nothing, row("AlternateRef")),
       .Comment = If(IsDBNull(row("Comment")), Nothing, row("Comment")),
       .UnderwritingYearId = If(IsDBNull(row("UnderwritingYearId")), Nothing, row("UnderwritingYearId")),
       .CostCenterId = If(IsDBNull(row("CostCenterId")), Nothing, row("CostCenterId")),
       .InsuranceRef = If(IsDBNull(row("InsuranceRef")), Nothing, row("InsuranceRef")),
       .PurchaseOrderNumber = If(IsDBNull(row("PurchaseOrderNumber")), Nothing, row("PurchaseOrderNumber")),
       .PurchaseInvoiceNumber = If(IsDBNull(row("PurchaseInvoiceNumber")), Nothing, row("PurchaseInvoiceNumber")),
       .ManualJournalId = If(IsDBNull(row("ManualJournalId")), Nothing, row("ManualJournalId")),
       .CurrencyCode = If(IsDBNull(row("CurrencyCode")), Nothing, row("CurrencyCode")),
              .Status = If(IsDBNull(row("Status")), Nothing, row("Status")),
              .CreatedDate = If(IsDBNull(row("CreatedDate")), Nothing, row("CreatedDate")),
       .CreatedBy = If(IsDBNull(row("CreatedBy")), Nothing, row("CreatedBy"))})

        Return myData.ToList()
    End Function
  
    Private Function DataTabletoList_ListofManualJournalTransactionsMaster(dtResultSet As DataTable) As List(Of BaseGetListOfManualJournalTransactionMasterResponseRowType)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetListOfManualJournalTransactionMasterResponseRowType With {
       .CreatedDate = If(IsDBNull(row("CreatedDate")), Nothing, row("CreatedDate")),
       .DocumentType = If(IsDBNull(row("DocumentType")), Nothing, row("DocumentType")),
       .Branch = If(IsDBNull(row("Branch")), Nothing, row("Branch")),
       .Comment = If(IsDBNull(row("Comment")), Nothing, row("Comment")),
       .AuthorisationComment = If(IsDBNull(row("AuthorisationComment")), Nothing, row("AuthorisationComment")),
       .IsReferred = If(IsDBNull(row("IsReferred")), Nothing, row("IsReferred")),
       .PerMonthOnDay = If(IsDBNull(row("PerMonthOnDay")), Nothing, row("PerMonthOnDay")),
       .PerPeriodOnDay = If(IsDBNull(row("PerPeriodOnDay")), Nothing, row("PerPeriodOnDay")),
       .PerQuarterOnDay = If(IsDBNull(row("PerQuarterOnDay")), Nothing, row("PerQuarterOnDay")),
       .RecurringOccurs = If(IsDBNull(row("RecurringOccurs")), Nothing, row("RecurringOccurs")),
       .ReversesOn = If(IsDBNull(row("ReversesOn")), Nothing, row("ReversesOn"))
        })

        Return myData.ToList()
    End Function
    
    Private Function DataTabletoList_ListofManualJournalTransactionsDetails(dtResultSet As DataTable) As List(Of BaseGetListOfManualJournalTransactionDetailsResponseRowType)
        Dim myData = dtResultSet.AsEnumerable().Select(Function(row As DataRow) New BaseGetListOfManualJournalTransactionDetailsResponseRowType With {
        .AccountCode = If(IsDBNull(row("AccountCode")), Nothing, row("AccountCode")),
        .ManualJournalDetailId = If(IsDBNull(row("ManualJournalDetailId")), Nothing, row("ManualJournalDetailId")),
        .Amount = If(IsDBNull(row("Amount")), Nothing, row("Amount")),
       .CurrencyCode = If(IsDBNull(row("CurrencyCode")), Nothing, row("CurrencyCode")),
       .CurrencyTypeDescription = If(IsDBNull(row("CurrencyTypeDescription")), Nothing, row("CurrencyTypeDescription")),
       .CurrencyRate = If(IsDBNull(row("CurrencyRate")), Nothing, row("CurrencyRate")),
       .BaseAmount = If(IsDBNull(row("BaseAmount")), Nothing, row("BaseAmount")),
       .AlternateRef = If(IsDBNull(row("AlternateRef")), Nothing, row("AlternateRef")),
       .Comment = If(IsDBNull(row("Comment")), Nothing, row("Comment")),
       .UnderwritingYearDescription = If(IsDBNull(row("UnderwritingYearDescription")), Nothing, row("UnderwritingYearDescription")),
       .CostCentreDescription = If(IsDBNull(row("CostCentreDescription")), Nothing, row("CostCentreDescription")),
       .InsuranceRef = If(IsDBNull(row("InsuranceRef")), Nothing, row("InsuranceRef")),
       .PurchaseOrderNumber = If(IsDBNull(row("PurchaseOrderNumber")), Nothing, row("PurchaseOrderNumber")),
       .PurchaseInvoiceNumber = If(IsDBNull(row("PurchaseInvoiceNumber")), Nothing, row("PurchaseInvoiceNumber")),
       .TransDetailId = If(IsDBNull(row("TransDetailId")), Nothing, row("TransDetailId"))})

        Return myData.ToList()
    End Function
                   
End Class
