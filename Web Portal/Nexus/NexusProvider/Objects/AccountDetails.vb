

<Serializable()> Public Class AccountDetails

    Private iPartyCnt As Integer
    Private iAccountKey As Integer
    Private sDocumentRef As String
    Private sCurrencyCode As String
    Private dCurrencyAmount As Double
    Private dTolerance As Double
    Private bToleranceSpecified As Boolean
    Private sDocTypeGroupCode As String
    Private sDocumentTypeCode As String
    Private iPeriodKey As Integer
    Private dDateFrom As Date
    Private dDateTo As Date
    Private sInsuranceRef As String
    Private sUsername As String
    Private sPurchaseInvoiceNo As String
    Private sPurchaseOrderNo As String
    Private sDepartment As String
    Private sSpare As String
    Private bOutstandingOnly As Boolean
    Private bOutstandingOnlySpecified As Boolean
    Private bIsNewPF As Boolean
    Private bIsNewPFSpecified As Boolean
    Private iInsuredAccountKey As Integer
    Private bRollup As Boolean
    Private iCashListKey As Integer
    Private bOrderBySpare As Boolean
    Private iDocumentKey As Integer
    Private iFinancePlanKey As Integer
    Private nFinancePlanVersion As Integer
    Private sFinancePlanStatus As String
    Private iUnderwritingYearKey As Integer
    Private sSourceArray As String
    Private sTransDetailKeys As String
    Private bDisplay500 As Boolean
    Private sAltReference As String
    Private bIncludeReversedTran As Boolean
    Private sBGRef As String

    'Specific for responsetype
    Private iBranchKey As Integer
    Private sAccount As String
    Private sAccountName As String
    Private sDocRef As String
    Private sAltRef As String
    Private dEffectiveDate As Date
    Private dTransDate As Date
    Private sMediaType As String
    Private dAmount As Double
    Private bPrimarySettled As Boolean
    Private dOutstandingAmount As Double
    Private dPaidDate As String
    Private iDocTypeId As Integer
    Private sReference As String
    Private sOperatorName As String
    Private sPeriod As String
    Private iDocumentGroupId As Integer
    Private sClient As String
    Private sClientCode As String
    Private sMediaRef As String
    Private sPayeeName As String
    Private sUnderwritingYear As String
    Private dAccountOutStandingAmount As Double
    Private dOutStandingCurrencyAmount As Double

    Private oDateByTransaction As InsurerPaymentsDateByType
    Private oMarkedStatus As InsurerPaymentsMarkedStatus
    Private oMonth As Month
    Private sAlternateReference As String
    Private sInsurerPaymentBranchCode As String
    Private bMarkedStatusSpecified As Boolean

    Private iDocumentId As Integer
    Private sInsurerRef As String
    Private dFullyPaidAmount As Decimal
    Private dClientOutstanding As Decimal
    Private iConsolidateBinder As Integer
    Private sShortName As String
    Private sResolvedName As String
    Private iTransdetailId As Integer
    Private iCompanyId As Integer
    Private dtAccountingDate As DateTime
    Private iCurrencyId As Integer
    Private dCurrencyBaseRate As Decimal
    Private dMarkedAmount As Decimal
    Private dPaidAmount As Decimal
    Private sPeriodName As String
    Private iAccountCurrencyId As Integer
    Private sAccountCurrencyCode As String
    Private dAccountBaseRate As Decimal
    Private dFullyPaidAccountAmount As Decimal
    Private dClientOutstandingAccountAmount As Decimal
    Private dAccountAmount As Decimal
    Private dMarkedAccountAmount As Decimal
    Private dPaidAccountAmount As Decimal
    Private sBranchCode As String
    Private sAccountCode As String
    Private bAllocationTimeStamp As Byte()
    Private bIsSelected As Boolean = False
    Private sCode, sDescription, sBaseCurrencyCode As String
    Private sBalanceType As String
    Private dTax, dComm, dSettled As Decimal
    Private dTotalMarkedAmount, dTotalPaidAmount, dTotalAmount, dTotalOutstandingAmount, dTotalClientOutstandingAmount As Decimal
    Private sYearName As String
    Private dDueDate, dDueDateTo, dDueDateFrom As Nullable(Of Date)
    Private bIsSplitReceipt As Boolean
    Private bIsLeadAgent As Boolean
    Private nCashListItemKey As Integer
    Private nBankAccountID As Integer
    Private sPolicyNumber As String
    Private bGrossAgent As Boolean
    Private Insurance_file_cnt As Integer
    Private Insurance_folder_cnt As Integer
    Private bExcludePendingAuth As Boolean
    Private bOnlyPendingAuth As Boolean

    Public Sub New()

        oDateByTransaction = New InsurerPaymentsDateByType
        oMarkedStatus = New InsurerPaymentsMarkedStatus
        oMonth = New Month

    End Sub
    Public Property TotalClientOutstandingAmount() As Decimal
        Get
            Return Me.dTotalClientOutstandingAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dTotalClientOutstandingAmount = value
        End Set
    End Property
    Public Property TotalOutstandingAmount() As Decimal
        Get
            Return Me.dTotalOutstandingAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dTotalOutstandingAmount = value
        End Set
    End Property
    Public Property TotalAmount() As Decimal
        Get
            Return Me.dTotalAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dTotalAmount = value
        End Set
    End Property
    Public Property TotalPaidAmount() As Decimal
        Get
            Return Me.dTotalPaidAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dTotalPaidAmount = value
        End Set
    End Property
    Public Property TotalMarkedAmount() As Decimal
        Get
            Return Me.dTotalMarkedAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dTotalMarkedAmount = value
        End Set
    End Property
    Public Property BaseCurrencyCode() As String
        Get
            Return Me.sBaseCurrencyCode
        End Get
        Set(ByVal value As String)
            Me.sBaseCurrencyCode = value
        End Set
    End Property
    Public Property Settled() As Decimal
        Get
            Return Me.dSettled
        End Get
        Set(ByVal value As Decimal)
            Me.dSettled = value
        End Set
    End Property
    Public Property Comm() As Decimal
        Get
            Return Me.dComm
        End Get
        Set(ByVal value As Decimal)
            Me.dComm = value
        End Set
    End Property
    Public Property Tax() As Decimal
        Get
            Return Me.dTax
        End Get
        Set(ByVal value As Decimal)
            Me.dTax = value
        End Set
    End Property
    Public Property Code() As String
        Get
            Return Me.sCode
        End Get
        Set(ByVal value As String)
            Me.sCode = value
        End Set
    End Property
    Public Property Description() As String
        Get
            Return Me.sDescription
        End Get
        Set(ByVal value As String)
            Me.sDescription = value
        End Set
    End Property
    Public Property MarkedStatusSpecified() As Boolean
        Get
            Return Me.bMarkedStatusSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bMarkedStatusSpecified = value
        End Set
    End Property

    Public Property AllocationTimeStamp() As Byte()
        Get
            Return Me.bAllocationTimeStamp
        End Get
        Set(ByVal value As Byte())
            Me.bAllocationTimeStamp = value
        End Set
    End Property
    Public Property IsSelected() As Boolean
        Get
            Return Me.bIsSelected
        End Get
        Set(ByVal value As Boolean)
            Me.bIsSelected = value
        End Set
    End Property

    Public Property AccountCode() As String
        Get
            Return Me.sAccountCode
        End Get
        Set(ByVal value As String)
            Me.sAccountCode = value
        End Set
    End Property
    Public Property BranchCode() As String
        Get
            Return Me.sBranchCode
        End Get
        Set(ByVal value As String)
            Me.sBranchCode = value
        End Set
    End Property
    Public Property PaidAccountAmount() As Decimal
        Get
            Return Me.dPaidAccountAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dPaidAccountAmount = value
        End Set
    End Property
    Public Property MarkedAccountAmount() As Decimal
        Get
            Return Me.dMarkedAccountAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dMarkedAccountAmount = value
        End Set
    End Property
    Public Property AccountAmount() As Decimal
        Get
            Return Me.dAccountAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dAccountAmount = value
        End Set
    End Property
    Public Property ClientOutstandingAccountAmount() As Decimal
        Get
            Return Me.dClientOutstandingAccountAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dClientOutstandingAccountAmount = value
        End Set
    End Property
    Public Property FullyPaidAccountAmount() As Decimal
        Get
            Return Me.dFullyPaidAccountAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dFullyPaidAccountAmount = value
        End Set
    End Property
    Public Property AccountBaseRate() As Decimal
        Get
            Return Me.dAccountBaseRate
        End Get
        Set(ByVal value As Decimal)
            Me.dAccountBaseRate = value
        End Set
    End Property
    Public Property AccountCurrencyCode() As String
        Get
            Return Me.sAccountCurrencyCode
        End Get
        Set(ByVal value As String)
            Me.sAccountCurrencyCode = value
        End Set
    End Property
    Public Property AccountCurrencyId() As Integer
        Get
            Return Me.iAccountCurrencyId
        End Get
        Set(ByVal value As Integer)
            Me.iAccountCurrencyId = value
        End Set
    End Property
    Public Property PeriodName() As String
        Get
            Return Me.sPeriodName
        End Get
        Set(ByVal value As String)
            Me.sPeriodName = value
        End Set
    End Property
    Public Property YearName() As String
        Get
            Return Me.sYearName
        End Get
        Set(ByVal value As String)
            Me.sYearName = value
        End Set
    End Property
    Public Property PaidAmount() As Decimal
        Get
            Return Me.dPaidAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dPaidAmount = value
        End Set
    End Property
    Public Property MarkedAmount() As Decimal
        Get
            Return Me.dMarkedAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dMarkedAmount = value
        End Set
    End Property
    Public Property CurrencyBaseRate() As Decimal
        Get
            Return Me.dCurrencyBaseRate
        End Get
        Set(ByVal value As Decimal)
            Me.dCurrencyBaseRate = value
        End Set
    End Property
    Public Property CurrencyId() As Integer
        Get
            Return Me.iCurrencyId
        End Get
        Set(ByVal value As Integer)
            Me.iCurrencyId = value
        End Set
    End Property
    Public Property AccountingDate() As DateTime
        Get
            Return Me.dtAccountingDate
        End Get
        Set(ByVal value As DateTime)
            Me.dtAccountingDate = value
        End Set
    End Property
    Public Property CompanyId() As Integer
        Get
            Return Me.iCompanyId
        End Get
        Set(ByVal value As Integer)
            Me.iCompanyId = value
        End Set
    End Property
    Public Property TransdetailId() As Integer
        Get
            Return Me.iTransdetailId
        End Get
        Set(ByVal value As Integer)
            Me.iTransdetailId = value
        End Set
    End Property
    Public Property ResolvedName() As String
        Get
            Return Me.sResolvedName
        End Get
        Set(ByVal value As String)
            Me.sResolvedName = value
        End Set
    End Property
    Public Property ShortName() As String
        Get
            Return Me.sShortName
        End Get
        Set(ByVal value As String)
            Me.sShortName = value
        End Set
    End Property
    Public Property ConsolidateBinder() As Integer
        Get
            Return Me.iConsolidateBinder
        End Get
        Set(ByVal value As Integer)
            Me.iConsolidateBinder = value
        End Set
    End Property
    Public Property ClientOutstanding() As Decimal
        Get
            Return Me.dClientOutstanding
        End Get
        Set(ByVal value As Decimal)
            Me.dClientOutstanding = value
        End Set
    End Property
    Public Property FullyPaidAmount() As Decimal
        Get
            Return Me.dFullyPaidAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dFullyPaidAmount = value
        End Set
    End Property
    Public Property InsurerRef() As String
        Get
            Return Me.sInsurerRef
        End Get
        Set(ByVal value As String)
            Me.sInsurerRef = value
        End Set
    End Property
    Public Property DocumentId() As Integer
        Get
            Return Me.iDocumentId
        End Get
        Set(ByVal value As Integer)
            Me.iDocumentId = value
        End Set
    End Property
    Public Property DateByTransaction() As InsurerPaymentsDateByType
        Get
            Return Me.oDateByTransaction
        End Get
        Set(ByVal value As InsurerPaymentsDateByType)
            Me.oDateByTransaction = value
        End Set
    End Property
    Public Property MarkedStatus() As InsurerPaymentsMarkedStatus
        Get
            Return Me.oMarkedStatus
        End Get
        Set(ByVal value As InsurerPaymentsMarkedStatus)
            Me.oMarkedStatus = value
        End Set
    End Property
    Public Property Month() As Month
        Get
            Return Me.oMonth
        End Get
        Set(ByVal value As Month)
            Me.oMonth = value
        End Set
    End Property
    Public Property AlternateReference() As String
        Get
            Return Me.sAlternateReference
        End Get
        Set(ByVal value As String)
            Me.sAlternateReference = value
        End Set
    End Property
    Public Property InsurerPaymentBranchCode() As String
        Get
            Return Me.sInsurerPaymentBranchCode
        End Get
        Set(ByVal value As String)
            Me.sInsurerPaymentBranchCode = value
        End Set
    End Property
    Public Property PartyCnt() As Integer
        Get
            Return Me.iPartyCnt
        End Get
        Set(ByVal value As Integer)
            Me.iPartyCnt = value
        End Set
    End Property

    Public Property AccountKey() As Integer
        Get
            Return Me.iAccountKey
        End Get
        Set(ByVal value As Integer)
            Me.iAccountKey = value
        End Set
    End Property
    Public Property BankAccountName() As String
        Get
            Return Me.sAccountName
        End Get
        Set(ByVal value As String)
            Me.sAccountName = value
        End Set
    End Property

    Public Property DocumentRef() As String
        Get
            Return Me.sDocumentRef
        End Get
        Set(ByVal value As String)
            Me.sDocumentRef = value
        End Set
    End Property
    Public Property CurrencyCode() As String
        Get
            Return Me.sCurrencyCode
        End Get
        Set(ByVal value As String)
            Me.sCurrencyCode = value
        End Set
    End Property
    Public Property CurrencyAmount() As Double
        Get
            Return Me.dCurrencyAmount
        End Get
        Set(ByVal value As Double)
            Me.dCurrencyAmount = value
        End Set
    End Property
    Public Property Tolerance() As Double
        Get
            Return Me.dTolerance
        End Get
        Set(ByVal value As Double)
            Me.dTolerance = value
        End Set
    End Property
    Public Property ToleranceSpecified() As Boolean
        Get
            Return Me.bToleranceSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bToleranceSpecified = value
        End Set
    End Property
    Public Property DocTypeGroupCode() As String
        Get
            Return Me.sDocTypeGroupCode
        End Get
        Set(ByVal value As String)
            Me.sDocTypeGroupCode = value
        End Set
    End Property
    Public Property DocumentTypeCode() As String
        Get
            Return Me.sDocumentTypeCode
        End Get
        Set(ByVal value As String)
            Me.sDocumentTypeCode = value
        End Set
    End Property
    Public Property PeriodKey() As Integer
        Get
            Return Me.iPeriodKey
        End Get
        Set(ByVal value As Integer)
            Me.iPeriodKey = value
        End Set
    End Property
    Public Property DateFrom() As Date
        Get
            Return Me.dDateFrom
        End Get
        Set(ByVal value As Date)
            Me.dDateFrom = value
        End Set
    End Property

    Public Property DateTo() As Date
        Get
            Return Me.dDateTo
        End Get
        Set(ByVal value As Date)
            Me.dDateTo = value
        End Set
    End Property

    Public Property InsuranceRef() As String
        Get
            Return Me.sInsuranceRef
        End Get
        Set(ByVal value As String)
            Me.sInsuranceRef = value
        End Set
    End Property
    Public Property Username() As String
        Get
            Return Me.sUsername
        End Get
        Set(ByVal value As String)
            Me.sUsername = value
        End Set
    End Property
    Public Property PurchaseInvoiceNo() As String
        Get
            Return Me.sPurchaseInvoiceNo
        End Get
        Set(ByVal value As String)
            Me.sPurchaseInvoiceNo = value
        End Set
    End Property
    Public Property PurchaseOrderNo() As String
        Get
            Return Me.sPurchaseOrderNo
        End Get
        Set(ByVal value As String)
            Me.sPurchaseOrderNo = value
        End Set
    End Property
    Public Property Department() As String
        Get
            Return Me.sDepartment
        End Get
        Set(ByVal value As String)
            Me.sDepartment = value
        End Set
    End Property
    Public Property Spare() As String
        Get
            Return Me.sSpare
        End Get
        Set(ByVal value As String)
            Me.sSpare = value
        End Set
    End Property
    Public Property OutstandingOnly() As Boolean
        Get
            Return Me.bOutstandingOnly
        End Get
        Set(ByVal value As Boolean)
            Me.bOutstandingOnly = value
        End Set
    End Property
    Public Property OutstandingOnlySpecified() As Boolean
        Get
            Return Me.bOutstandingOnlySpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bOutstandingOnlySpecified = value
        End Set
    End Property

    Public Property IsNewPF() As Boolean
        Get
            Return Me.bIsNewPF
        End Get
        Set(ByVal value As Boolean)
            Me.bIsNewPF = value
        End Set
    End Property
    Public Property IsNewPFSpecified() As Boolean
        Get
            Return Me.bIsNewPFSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bIsNewPFSpecified = value
        End Set
    End Property

    Public Property InsuredAccountKey() As Integer
        Get
            Return Me.iInsuredAccountKey
        End Get
        Set(ByVal value As Integer)
            Me.iInsuredAccountKey = value
        End Set
    End Property

    Public Property Rollup() As Boolean
        Get
            Return Me.bRollup
        End Get
        Set(ByVal value As Boolean)
            Me.bRollup = value
        End Set
    End Property

    Public Property CashListKey() As Integer
        Get
            Return Me.iCashListKey
        End Get
        Set(ByVal value As Integer)
            Me.iCashListKey = value
        End Set
    End Property

    Public Property OrderBySpare() As Boolean
        Get
            Return Me.bOrderBySpare
        End Get
        Set(ByVal value As Boolean)
            Me.bOrderBySpare = value
        End Set
    End Property

    Public Property DocumentKey() As Integer
        Get
            Return Me.iDocumentKey
        End Get
        Set(ByVal value As Integer)
            Me.iDocumentKey = value
        End Set
    End Property

    Public Property FinancePlanKey() As Integer
        Get
            Return Me.iFinancePlanKey
        End Get
        Set(ByVal value As Integer)
            Me.iFinancePlanKey = value
        End Set
    End Property

    Public Property FinancePlanVersion() As Integer
        Get
            Return Me.nFinancePlanVersion
        End Get
        Set(value As Integer)
            Me.nFinancePlanVersion = value
        End Set
    End Property

    Public Property FinancePlanStatus() As String
        Get
            Return Me.sFinancePlanStatus
        End Get
        Set(ByVal value As String)
            Me.sFinancePlanStatus = value
        End Set
    End Property

    Public Property UnderwritingYearKey() As Integer
        Get
            Return Me.iUnderwritingYearKey
        End Get
        Set(ByVal value As Integer)
            Me.iUnderwritingYearKey = value
        End Set
    End Property

    Public Property SourceArray() As String
        Get
            Return Me.sSourceArray
        End Get
        Set(ByVal value As String)
            Me.sSourceArray = value
        End Set
    End Property
    Public Property TransDetailKeys() As String
        Get
            Return Me.sTransDetailKeys
        End Get
        Set(ByVal value As String)
            Me.sTransDetailKeys = value
        End Set
    End Property
    Public Property Display500() As Boolean
        Get
            Return Me.bDisplay500
        End Get
        Set(ByVal value As Boolean)
            Me.bDisplay500 = value
        End Set
    End Property

    Public Property AltReference() As String
        Get
            Return Me.sAltReference
        End Get
        Set(ByVal value As String)
            Me.sAltReference = value
        End Set
    End Property
    Public Property IncludeReversedTran() As Boolean
        Get
            Return Me.bIncludeReversedTran
        End Get
        Set(ByVal value As Boolean)
            Me.bIncludeReversedTran = value
        End Set
    End Property

    Public Property BGRef() As String
        Get
            Return Me.sBGRef
        End Get
        Set(ByVal value As String)
            Me.sBGRef = value
        End Set
    End Property
    Public Property BranchKey() As Integer
        Get
            Return Me.iBranchKey
        End Get
        Set(ByVal value As Integer)
            Me.iBranchKey = value
        End Set
    End Property
    Public Property Account() As String
        Get
            Return Me.sAccount
        End Get
        Set(ByVal value As String)
            Me.sAccount = value
        End Set
    End Property
    Public Property DocRef() As String
        Get
            Return Me.sDocRef
        End Get
        Set(ByVal value As String)
            Me.sDocRef = value
        End Set
    End Property
    Public Property AltRef() As String
        Get
            Return Me.sAltRef
        End Get
        Set(ByVal value As String)
            Me.sAltRef = value
        End Set
    End Property
    Public Property EffectiveDate() As Date
        Get
            Return Me.dEffectiveDate
        End Get
        Set(ByVal value As Date)
            Me.dEffectiveDate = value
        End Set
    End Property
    Public Property TransDate() As Date
        Get
            Return Me.dTransDate
        End Get
        Set(ByVal value As Date)
            Me.dTransDate = value
        End Set
    End Property
    Public Property MediaType() As String
        Get
            Return Me.sMediaType
        End Get
        Set(ByVal value As String)
            Me.sMediaType = value
        End Set
    End Property
    Public Property Amount() As Double
        Get
            Return Me.dAmount
        End Get
        Set(ByVal value As Double)
            Me.dAmount = value
        End Set
    End Property
    Public Property PrimarySettled() As Boolean
        Get
            Return Me.bPrimarySettled
        End Get
        Set(ByVal value As Boolean)
            Me.bPrimarySettled = value
        End Set
    End Property
    Public Property OutstandingAmount() As Double
        Get
            Return Me.dOutstandingAmount
        End Get
        Set(ByVal value As Double)
            Me.dOutstandingAmount = value
        End Set
    End Property
    Public Property PaidDate() As string
        Get
            Return Me.dPaidDate
        End Get
        Set(ByVal value As string)
            Me.dPaidDate = value
        End Set
    End Property
    Public Property DocTypeId() As Integer
        Get
            Return Me.iDocTypeId
        End Get
        Set(ByVal value As Integer)
            Me.iDocTypeId = value
        End Set
    End Property
    Public Property Reference() As String
        Get
            Return Me.sReference
        End Get
        Set(ByVal value As String)
            Me.sReference = value
        End Set
    End Property
    Public Property OperatorName() As String
        Get
            Return Me.sOperatorName
        End Get
        Set(ByVal value As String)
            Me.sOperatorName = value
        End Set
    End Property
    Public Property Period() As String
        Get
            Return Me.sPeriod
        End Get
        Set(ByVal value As String)
            Me.sPeriod = value
        End Set
    End Property
    Public Property DocumentGroupId() As Integer
        Get
            Return Me.iDocumentGroupId
        End Get
        Set(ByVal value As Integer)
            Me.iDocumentGroupId = value
        End Set
    End Property
    Public Property Client() As String
        Get
            Return Me.sClient
        End Get
        Set(ByVal value As String)
            Me.sClient = value
        End Set
    End Property
    Public Property ClientCode() As String
        Get
            Return Me.sClientCode
        End Get
        Set(ByVal value As String)
            Me.sClientCode = value
        End Set
    End Property
    Public Property MediaRef() As String
        Get
            Return Me.sMediaRef
        End Get
        Set(ByVal value As String)
            Me.sMediaRef = value
        End Set
    End Property
    Public Property PayeeName() As String
        Get
            Return Me.sPayeeName
        End Get
        Set(ByVal value As String)
            Me.sPayeeName = value
        End Set
    End Property
    Public Property UnderwritingYear() As String
        Get
            Return Me.sUnderwritingYear
        End Get
        Set(ByVal value As String)
            Me.sUnderwritingYear = value
        End Set
    End Property
    Public Property AccountOutStandingAmount() As Double
        Get
            Return Me.dAccountOutStandingAmount
        End Get
        Set(ByVal value As Double)
            Me.dAccountOutStandingAmount = value
        End Set
    End Property
    Public Property OutStandingCurrencyAmount() As Double
        Get
            Return Me.dOutStandingCurrencyAmount
        End Get
        Set(ByVal value As Double)
            Me.dOutStandingCurrencyAmount = value
        End Set
    End Property

    Public Property BalanceType() As String
        Get
            Return Me.sBalanceType
        End Get
        Set(ByVal value As String)
            Me.sBalanceType = value
        End Set
    End Property
    Public Property DueDate() As Nullable(Of Date)
        Get
            If Me.dDueDate Is Nothing Then
                Return Nothing
            Else
                Return Me.dDueDate
            End If
        End Get
        Set(ByVal value As Nullable(Of Date))
            If value.HasValue Then Me.dDueDate = value
        End Set
    End Property

    Public Property DueDateFrom() As Nullable(Of Date)
        Get
            If Me.dDueDateFrom Is Nothing Then
                Return Nothing
            Else
                Return Me.dDueDateFrom
            End If
        End Get
        Set(ByVal value As Nullable(Of Date))
            If value.HasValue Then Me.dDueDateFrom = value
        End Set
    End Property
    Public Property DueDateTo() As Nullable(Of Date)
        Get
            If Me.dDueDateTo Is Nothing Then
                Return Nothing
            Else
                Return Me.dDueDateTo
            End If
        End Get
        Set(ByVal value As Nullable(Of Date))
            If value.HasValue Then Me.dDueDateTo = value
        End Set
    End Property

    Public Property IsSplitReceipt() As Boolean
        Get
            Return Me.bIsSplitReceipt
        End Get
        Set(ByVal value As Boolean)
            Me.bIsSplitReceipt = value
        End Set
    End Property

    Public Property IsLeadAgent() As Boolean
        Get
            Return Me.bIsLeadAgent
        End Get
        Set(ByVal value As Boolean)
            Me.bIsLeadAgent = value
        End Set
    End Property

    Public Property CashListItemKey() As Integer
        Get
            Return Me.nCashListItemKey
        End Get
        Set(value As Integer)
            Me.nCashListItemKey = value
        End Set
    End Property

    Public Property BankAccountID() As Integer
        Get
            Return Me.nBankAccountID
        End Get
        Set(value As Integer)
            Me.nBankAccountID = value
        End Set
    End Property

    Public Property PolicyNumber() As String
        Get
            Return Me.sPolicyNumber
        End Get
        Set(ByVal value As String)
            Me.sPolicyNumber = value
        End Set
    End Property
    Public Property ExcludePendingAuth() As Boolean
        Get
            Return bExcludePendingAuth
        End Get
        Set(ByVal value As Boolean)
            bExcludePendingAuth = value
        End Set
    End Property
    Public Property OnlyPendingAuth() As Boolean
        Get
            Return bOnlyPendingAuth
        End Get
        Set(ByVal value As Boolean)
            bOnlyPendingAuth = value
        End Set
    End Property
    Public Property GrossAgent() As Boolean
        Get
            Return bGrossAgent
        End Get
        Set(ByVal value As Boolean)
            bGrossAgent = value
        End Set
    End Property
    Public Property Insurance_filecnt() As Integer
        Get
            Return Me.Insurance_file_cnt
        End Get
        Set
            Me.Insurance_file_cnt = Value
        End Set
    End Property
    Public Property Insurance_foldercnt() As Integer
        Get
            Return Me.Insurance_folder_cnt
        End Get
        Set
            Me.Insurance_folder_cnt = Value
        End Set
    End Property
End Class

''' <summary>
''' "SortableCollectionBase" class internally inherits "CollectionBase" and gives additionaly "Sortable" feture in class.
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class AccountDetailsCollection : Inherits SortableCollectionBase

    Public Sub New()
        MyBase.SortObjectType = GetType(AccountDetails)
    End Sub

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()
        For Each oAccountDetails As AccountDetails In List
            sbPrint.AppendLine("<br />")
        Next
        Return sbPrint.ToString()

    End Function
    Public Function Add(ByVal v_oAccountDetails As AccountDetails) As Integer
        Return List.Add(v_oAccountDetails)
    End Function
    Public Sub Remove(ByVal v_oAccountDetails As AccountDetails)
        List.Remove(v_oAccountDetails)
    End Sub
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub
    Default Public Property Item(ByVal i As Integer) As AccountDetails
        Get
            Return List(i)
        End Get
        Set(ByVal value As AccountDetails)
            List(i) = value
        End Set
    End Property

    Public Function NumberOfRows() As Integer
        Return List.Count()
    End Function

End Class

Public Enum InsurerPaymentsDateByType

    '''<remarks/>

    EffectiveDate

    '''<remarks/>
    TransactionDate
End Enum


Public Enum InsurerPaymentsMarkedStatus

    '''<remarks/>
    No

    '''<remarks/>
    Yes

    '''<remarks/>

    Any
End Enum


Public Enum Month

    '''<remarks/>
    All

    '''<remarks/>
    January

    '''<remarks/>
    February

    '''<remarks/>
    March

    '''<remarks/>
    April

    '''<remarks/>
    May

    '''<remarks/>
    June

    '''<remarks/>
    July

    '''<remarks/>
    August

    '''<remarks/>
    September

    '''<remarks/>
    October

    '''<remarks/>
    November

    '''<remarks/>
    December
End Enum
