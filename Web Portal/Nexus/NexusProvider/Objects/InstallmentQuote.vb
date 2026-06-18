<Serializable()> Public Class InstalmentQuote

    Private nAgentKey As Integer
    Private nClaimDebtID As Integer
    Private nCompanyNo As Integer
    Private nDaysDelay As Integer
    Private nSingleInstalmentPerMonth As Integer
    Private nFirstInstalmentAlignWithDayInMonth As Integer
    Private nUseTransCurrncy As Integer
    Private nFrequencyID As Integer
    Private nFrequencyPerYear As Integer
    Private nHighlightCell As Integer
    Private nInstalmentsToPay As Integer
    Private nMediaTypeID As Integer
    Private nMinMTA As Integer
    Private nPFRF_ID As Integer
    Private nRefundType As Integer
    Private nSchemeNo As Integer
    Private nSchemeVersion As Integer
    Private nTimeout As Integer
    Private sAgentRef As String
    Private sBrokerID As String
    Private sBrokerUrl As String
    Private sCompanyName As String
    Private sFrequencyDescription As String
    Private sFrequencyPeriod As String
    Private sMediaTypeDescription As String
    Private sPassword As String
    Private sProductClass As String
    Private sProductCode As String
    Private sProviderCode As String
    Private sRef As String
    Private sSchemeName As String
    Private sTerms As String
    Private sUserID As String
    Private sUserName As String
    Private sMediaTypeValidation As String
    Private sSchemeTypeCode As String
    Private dAprRate As Double
    Private dDepositAmount As Double
    Private dFinanceCharge As Double
    Private dFirstInstalmentAmount As Double
    Private dFrequencyAmount As Double
    Private dInterestAmount As Double
    Private dInterestRate As Double
    Private dLastInstalmentAmount As Double
    Private dOriginalAmount As Double
    Private dOriginalOtherInstalmentAmount As Double
    Private dOriginalRate As Double
    Private dOtherInstalmentAmount As Double
    Private dProtectionAmount As Double
    Private dTaxAmount As Double
    Private dTotalAmountInput As Double
    Private dTotalInstalmentsAmount, dAmount As Double
    Private dtFirstInstalmentDate As DateTime
    Private dtLastInstalmentDate As DateTime
    Private dtNextInstalmentDate As DateTime
    Private nAlignTo As Integer
    Private nStartLimit As Integer
    Private bBranchCodeMandatory As Boolean
    Private bBranchNameMandatory As Boolean
    Private bBankNameMandatory As Boolean
    Private bBankAddressMandatory As Boolean
    Private bDepositAsInstalment As Boolean
    Private dtNextInstalmentDueDate As DateTime
    Private iFinanceToNet As Integer
    ''' <summary>
    ''' Agent Cnt=True/False
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AgentCnt() As Integer
        Get
            Return nAgentKey
        End Get
        Set(ByVal value As Integer)
            nAgentKey = value
        End Set
    End Property
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Property FinanceToNet() As Integer
        Get
            Return iFinanceToNet
        End Get
        Set(ByVal value As Integer)
            iFinanceToNet = value
        End Set
    End Property

    ''' <summary>
    ''' SingleInstalmentPerMonth Cnt=True/False
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SingleInstalmentPerMonth() As Integer
        Get
            Return nSingleInstalmentPerMonth
        End Get
        Set(ByVal value As Integer)
            nSingleInstalmentPerMonth = value
        End Set
    End Property

    ''' <summary>
    ''' FirstInstalmentAlignWithDayInMonth Cnt=True/False
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FirstInstalmentAlignWithDayInMonth() As Integer
        Get
            Return nFirstInstalmentAlignWithDayInMonth
        End Get
        Set(ByVal value As Integer)
            nFirstInstalmentAlignWithDayInMonth = value
        End Set
    End Property

    Public Property UseTransCurrncy() As Integer
        Get
            Return nUseTransCurrncy
        End Get
        Set(ByVal value As Integer)
            nUseTransCurrncy = value
        End Set
    End Property

    ''' <summary>
    ''' Agent References
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AgentRef() As String
        Get
            Return sAgentRef
        End Get
        Set(ByVal value As String)
            sAgentRef = value
        End Set
    End Property

    ''' <summary>
    ''' Apr Rate
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AprRate() As Double
        Get
            Return dAprRate
        End Get
        Set(ByVal value As Double)
            dAprRate = value
        End Set
    End Property

    ''' <summary>
    ''' Broker ID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BrokerID() As String
        Get
            Return sBrokerID
        End Get
        Set(ByVal value As String)
            sBrokerID = value
        End Set
    End Property

    ''' <summary>
    ''' Broker URL
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BrokerURL() As String
        Get
            Return sBrokerUrl
        End Get
        Set(ByVal value As String)
            sBrokerUrl = value
        End Set
    End Property

    ''' <summary>
    ''' Claim Debt ID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ClaimDebtID() As Integer
        Get
            Return nClaimDebtID
        End Get
        Set(ByVal value As Integer)
            nClaimDebtID = value
        End Set
    End Property

    ''' <summary>
    ''' Company Name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CompanyName() As String
        Get
            Return sCompanyName
        End Get
        Set(ByVal value As String)
            sCompanyName = value
        End Set
    End Property

    ''' <summary>
    ''' Company No
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CompanyNo() As Integer
        Get
            Return nCompanyNo
        End Get
        Set(ByVal value As Integer)
            nCompanyNo = value
        End Set
    End Property

    ''' <summary>
    ''' N. Of Days Delay
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DaysDelay() As Integer
        Get
            Return nDaysDelay
        End Get
        Set(ByVal value As Integer)
            nDaysDelay = value
        End Set
    End Property

    ''' <summary>
    ''' Deposit Amount
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DepositAmount() As Double
        Get
            Return dDepositAmount
        End Get
        Set(ByVal value As Double)
            dDepositAmount = value
        End Set
    End Property

    ''' <summary>
    ''' FinanceCharge
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FinanceCharge() As Double
        Get
            Return dFinanceCharge
        End Get
        Set(ByVal value As Double)
            dFinanceCharge = value
        End Set
    End Property

    ''' <summary>
    ''' First Instalment Amount
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FirstInstalmentAmount() As Double
        Get
            Return dFirstInstalmentAmount
        End Get
        Set(ByVal value As Double)
            dFirstInstalmentAmount = value
        End Set
    End Property

    ''' <summary>
    ''' First Instalment Date
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FirstInstalmentDate() As Date
        Get
            Return dtFirstInstalmentDate
        End Get
        Set(ByVal value As Date)
            dtFirstInstalmentDate = value
        End Set
    End Property

    ''' <summary>
    ''' Frequency Amount
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FrequencyAmount() As Double
        Get
            Return dFrequencyAmount
        End Get
        Set(ByVal value As Double)
            dFrequencyAmount = value
        End Set
    End Property

    ''' <summary>
    ''' Frequency Description
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FrequencyDescription() As String
        Get
            Return sFrequencyDescription
        End Get
        Set(ByVal value As String)
            sFrequencyDescription = value
        End Set
    End Property

    ''' <summary>
    ''' Frequency ID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FrequencyID() As Integer
        Get
            Return nFrequencyID
        End Get
        Set(ByVal value As Integer)
            nFrequencyID = value
        End Set
    End Property

    ''' <summary>
    ''' Frequency Period
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FrequencyPeriod() As String
        Get
            Return sFrequencyPeriod
        End Get
        Set(ByVal value As String)
            sFrequencyPeriod = value
        End Set
    End Property

    ''' <summary>
    ''' Frequency Per Year
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FrequencyPerYear() As Integer
        Get
            Return nFrequencyPerYear
        End Get
        Set(ByVal value As Integer)
            nFrequencyPerYear = value
        End Set
    End Property

    ''' <summary>
    ''' High light Cell
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property HighlightCell() As Integer
        Get
            Return nHighlightCell
        End Get
        Set(ByVal value As Integer)
            nHighlightCell = value
        End Set
    End Property

    ''' <summary>
    ''' Instalments To Pay
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property InstalmentsToPay() As Integer
        Get
            Return nInstalmentsToPay
        End Get
        Set(ByVal value As Integer)
            nInstalmentsToPay = value
        End Set
    End Property

    ''' <summary>
    ''' Interest Amount
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property InterestAmount() As Double
        Get
            Return dInterestAmount
        End Get
        Set(ByVal value As Double)
            dInterestAmount = value
        End Set
    End Property

    ''' <summary>
    ''' Interest Rate
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property InterestRate() As Double
        Get
            Return dInterestRate
        End Get
        Set(ByVal value As Double)
            dInterestRate = value
        End Set
    End Property

    ''' <summary>
    ''' Last Instalment Amount
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LastInstalmentAmount() As Double
        Get
            Return dLastInstalmentAmount
        End Get
        Set(ByVal value As Double)
            dLastInstalmentAmount = value
        End Set
    End Property

    ''' <summary>
    ''' Last Instalment Date
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LastInstalmentDate() As Date
        Get
            Return dtLastInstalmentDate
        End Get
        Set(ByVal value As Date)
            dtLastInstalmentDate = value
        End Set
    End Property

    ''' <summary>
    ''' Media Type Description
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MediaTypeDescription() As String
        Get
            Return sMediaTypeDescription
        End Get
        Set(ByVal value As String)
            sMediaTypeDescription = value
        End Set
    End Property

    ''' <summary>
    ''' Media Type ID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MediaTypeID() As Integer
        Get
            Return nMediaTypeID
        End Get
        Set(ByVal value As Integer)
            nMediaTypeID = value
        End Set
    End Property

    ''' <summary>
    ''' Media Type Validation
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MediaTypeValidation() As String
        Get
            Return sMediaTypeValidation
        End Get
        Set(ByVal value As String)
            sMediaTypeValidation = value
        End Set
    End Property

    ''' <summary>
    ''' Minimum MTA
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MinMTA() As Integer
        Get
            Return nMinMTA
        End Get
        Set(ByVal value As Integer)
            nMinMTA = value
        End Set
    End Property

    ''' <summary>
    ''' Next Instalment Date
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NextInstalmentDate() As Date
        Get
            Return dtNextInstalmentDate
        End Get
        Set(ByVal value As Date)
            dtNextInstalmentDate = value
        End Set
    End Property

    ''' <summary>
    ''' Original Amount
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OriginalAmount() As Double
        Get
            Return dOriginalAmount
        End Get
        Set(ByVal value As Double)
            dOriginalAmount = value
        End Set
    End Property

    ''' <summary>
    ''' Original Other Instalment Amount
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OriginalOtherInstalmentAmount() As Double
        Get
            Return dOriginalOtherInstalmentAmount
        End Get
        Set(ByVal value As Double)
            dOriginalOtherInstalmentAmount = value
        End Set
    End Property

    ''' <summary>
    ''' Original Rate
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OriginalRate() As Double
        Get
            Return dOriginalRate
        End Get
        Set(ByVal value As Double)
            dOriginalRate = value
        End Set
    End Property

    ''' <summary>
    ''' Other Instalment Amount
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OtherInstalmentAmount() As Double
        Get
            Return dOtherInstalmentAmount
        End Get
        Set(ByVal value As Double)
            dOtherInstalmentAmount = value
        End Set
    End Property

    ''' <summary>
    ''' Password Values
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Password() As String
        Get
            Return sPassword
        End Get
        Set(ByVal value As String)
            sPassword = value
        End Set
    End Property

    ''' <summary>
    ''' PFRF ID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PFRF_ID() As Integer
        Get
            Return nPFRF_ID
        End Get
        Set(ByVal value As Integer)
            nPFRF_ID = value
        End Set
    End Property

    ''' <summary>
    ''' Product Class
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ProductClass() As String
        Get
            Return sProductClass
        End Get
        Set(ByVal value As String)
            sProductClass = value
        End Set
    End Property

    ''' <summary>
    ''' Product Code
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ProductCode() As String
        Get
            Return sProductCode
        End Get
        Set(ByVal value As String)
            sProductCode = value
        End Set
    End Property

    ''' <summary>
    ''' Protection Amount
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ProtectionAmount() As Double
        Get
            Return dProtectionAmount
        End Get
        Set(ByVal value As Double)
            dProtectionAmount = value
        End Set
    End Property

    ''' <summary>
    ''' Provider Code
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ProviderCode() As String
        Get
            Return sProviderCode
        End Get
        Set(ByVal value As String)
            sProviderCode = value
        End Set
    End Property

    ''' <summary>
    ''' Reference value
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Ref() As String
        Get
            Return sRef
        End Get
        Set(ByVal value As String)
            sRef = value
        End Set
    End Property

    ''' <summary>
    ''' Refund Type
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RefundType() As Integer
        Get
            Return nRefundType
        End Get
        Set(ByVal value As Integer)
            nRefundType = value
        End Set
    End Property

    ''' <summary>
    ''' Scheme Name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SchemeName() As String
        Get
            Return sSchemeName
        End Get
        Set(ByVal value As String)
            sSchemeName = value
        End Set
    End Property

    ''' <summary>
    ''' Scheme No
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SchemeNo() As Integer
        Get
            Return nSchemeNo
        End Get
        Set(ByVal value As Integer)
            nSchemeNo = value
        End Set
    End Property

    ''' <summary>
    ''' Scheme Type Code
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SchemeTypeCode() As String
        Get
            Return sSchemeTypeCode
        End Get
        Set(ByVal value As String)
            sSchemeTypeCode = value
        End Set
    End Property

    ''' <summary>
    ''' Scheme Version
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SchemeVersion() As Integer
        Get
            Return nSchemeVersion
        End Get
        Set(ByVal value As Integer)
            nSchemeVersion = value
        End Set
    End Property

    ''' <summary>
    ''' Tax Amount
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TaxAmount() As Double
        Get
            Return dTaxAmount
        End Get
        Set(ByVal value As Double)
            dTaxAmount = value
        End Set
    End Property

    ''' <summary>
    ''' Terms
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Terms() As String
        Get
            Return sTerms
        End Get
        Set(ByVal value As String)
            sTerms = value
        End Set
    End Property

    ''' <summary>
    ''' Time out
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Timeout() As Integer
        Get
            Return nTimeout
        End Get
        Set(ByVal value As Integer)
            nTimeout = value
        End Set
    End Property

    ''' <summary>
    ''' Total Amount Input
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TotalAmountInput() As Double
        Get
            Return dTotalAmountInput
        End Get
        Set(ByVal value As Double)
            dTotalAmountInput = value
        End Set
    End Property

    ''' <summary>
    ''' Total Instalments Amount
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TotalInstalmentsAmount() As Double
        Get
            Return dTotalInstalmentsAmount
        End Get
        Set(ByVal value As Double)
            dTotalInstalmentsAmount = value
        End Set
    End Property

    ''' <summary>
    ''' User ID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UserID() As String
        Get
            Return sUserID
        End Get
        Set(ByVal value As String)
            sUserID = value
        End Set
    End Property

    ''' <summary>
    ''' User name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Username() As String
        Get
            Return sUserName
        End Get
        Set(ByVal value As String)
            sUserName = value
        End Set
    End Property
    ''' <summary>
    ''' Amount for instalment quote
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Amount() As Double
        Get
            Return dAmount
        End Get
        Set(ByVal value As Double)
            dAmount = value
        End Set
    End Property
    ''' <summary>
    ''' It will contain instalment scheme option value of AlignTo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AlignTo() As Integer
        Get
            Return nAlignTo
        End Get
        Set(ByVal value As Integer)
            nAlignTo = value
        End Set
    End Property
    ''' <summary>
    ''' It will contain instalment scheme option value of DepositAsInstalment
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DepositAsInstalment() As Boolean
        Get
            Return bDepositAsInstalment
        End Get
        Set(ByVal value As Boolean)
            bDepositAsInstalment = value
        End Set
    End Property
    ''' <summary>
    ''' It will contain instalment scheme option value of BranchCodeMandatory
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BranchCodeMandatory() As Boolean
        Get
            Return bBranchCodeMandatory
        End Get
        Set(ByVal value As Boolean)
            bBranchCodeMandatory = value
        End Set
    End Property
    ''' <summary>
    ''' It will contain instalment scheme option value of BranchNameMandatory
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BranchNameMandatory() As Boolean
        Get
            Return bBranchNameMandatory
        End Get
        Set(ByVal value As Boolean)
            bBranchNameMandatory = value
        End Set
    End Property
    ''' <summary>
    ''' It will contain instalment scheme option value of BankNameMandatory
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BankNameMandatory() As Boolean
        Get
            Return bBankNameMandatory
        End Get
        Set(ByVal value As Boolean)
            bBankNameMandatory = value
        End Set
    End Property
    ''' <summary>
    ''' It will contain instalment scheme option value of BankAddressMandatory
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BankAddressMandatory() As Boolean
        Get
            Return bBankAddressMandatory
        End Get
        Set(ByVal value As Boolean)
            bBankAddressMandatory = value
        End Set
    End Property
    ''' <summary>
    ''' It will contain value of StartLimit of instalment scheme  
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property StartLimit() As Integer
        Get
            Return nStartLimit
        End Get
        Set(ByVal value As Integer)
            nStartLimit = value
        End Set
    End Property
    ''' <summary>
    ''' It will contains the next due date of previous active plan.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NextInstalmentDueDate() As Date
        Get
            Return dtNextInstalmentDueDate
        End Get
        Set(ByVal value As Date)
            dtNextInstalmentDueDate = value
        End Set
    End Property

    Public Overridable Function Print() As String
        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Agent Key : " & nAgentKey & "<br />")
        sbPrint.AppendLine("Claim Debt ID : " & nClaimDebtID.ToString() & "<br />")
        sbPrint.AppendLine("Company No : " & nCompanyNo.ToString() & "<br />")
        sbPrint.AppendLine("Scheme No : " & nSchemeNo.ToString() & "<br />")
        sbPrint.AppendLine("Scheme Version : " & nSchemeVersion.ToString() & "<br />")
        sbPrint.AppendLine("Days Delay" & nDaysDelay.ToString() & "<br />")
        sbPrint.AppendLine("Frequency ID : " & nFrequencyID.ToString() & "<br />")
        sbPrint.AppendLine("Frequency Per Year : " & nFrequencyPerYear.ToString() & "<br />")
        sbPrint.AppendLine("Highlight Cell : " & nHighlightCell.ToString() & "<br />")
        sbPrint.AppendLine("Instalments To Pay : " & nInstalmentsToPay.ToString() & "<br />")
        sbPrint.AppendLine("Media Type ID : " & nMediaTypeID.ToString() & "<br />")
        sbPrint.AppendLine("Min MTA" & nMinMTA.ToString() & "<br />")
        sbPrint.AppendLine("PFRF ID : " & nPFRF_ID.ToString() & "<br />")
        sbPrint.AppendLine("Timeout : " & nTimeout.ToString() & "<br />")
        sbPrint.AppendLine("Agent Ref : " & sAgentRef & "<br />")
        sbPrint.AppendLine("Broker ID : " & sBrokerID & "<br />")
        sbPrint.AppendLine("Broker Url : " & sBrokerUrl & "<br />")
        sbPrint.AppendLine("Company Name" & sCompanyName & "<br />")
        sbPrint.AppendLine("Frequency Description : " & sFrequencyDescription & "<br />")
        sbPrint.AppendLine("Frequency Period : " & sFrequencyPeriod & "<br />")
        sbPrint.AppendLine("Media Type Description" & sMediaTypeDescription & "<br />")
        sbPrint.AppendLine("Product Class : " & sProductClass & "<br />")
        sbPrint.AppendLine("Product Code : " & sProductCode & "<br />")
        sbPrint.AppendLine("Provider Code : " & sProviderCode & "<br />")
        sbPrint.AppendLine("Scheme Name : " & sSchemeName & "<br />")
        sbPrint.AppendLine("Media Type Validation : " & sMediaTypeValidation & "<br />")
        sbPrint.AppendLine("Scheme Type Code" & sSchemeTypeCode & "<br />")
        sbPrint.AppendLine("Apr Rate : " & dAprRate.ToString() & "<br />")
        sbPrint.AppendLine("Deposit Amount : " & dDepositAmount.ToString() & "<br />")
        sbPrint.AppendLine("Finance Charge : " & dFinanceCharge.ToString() & "<br />")
        sbPrint.AppendLine("First Instalment Amount : " & dFirstInstalmentAmount.ToString() & "<br />")
        sbPrint.AppendLine("Frequency Amount : " & dFrequencyAmount.ToString() & "<br />")
        sbPrint.AppendLine("Interest Amount" & dInterestAmount.ToString() & "<br />")
        sbPrint.AppendLine("Interest Rate : " & dInterestRate.ToString() & "<br />")
        sbPrint.AppendLine("Last Instalment Amount : " & dLastInstalmentAmount.ToString() & "<br />")
        sbPrint.AppendLine("Original Amount : " & dOriginalAmount.ToString() & "<br />")
        sbPrint.AppendLine("Original Other Instalment Amount : " & dOriginalOtherInstalmentAmount.ToString() & "<br />")
        sbPrint.AppendLine("Original Rate : " & dOriginalRate.ToString() & "<br />")
        sbPrint.AppendLine("Other Instalment Amount : " & dOtherInstalmentAmount.ToString() & "<br />")
        sbPrint.AppendLine("Protection Amount : " & dProtectionAmount.ToString() & "<br />")
        sbPrint.AppendLine("Tax Amount" & dTaxAmount.ToString() & "<br />")
        sbPrint.AppendLine("Total Amount Input : " & dTotalAmountInput.ToString() & "<br />")
        sbPrint.AppendLine("Total Instalment Amount : " & dTotalInstalmentsAmount.ToString() & "<br />")
        sbPrint.AppendLine("Amount : " & dAmount.ToString() & "<br />")
        sbPrint.AppendLine("First Instalment Date : " & dtFirstInstalmentDate.ToString() & "<br />")
        sbPrint.AppendLine("Last Instalment Date" & dtLastInstalmentDate.ToString() & "<br />")
        sbPrint.AppendLine("Next Instalment Date : " & dtNextInstalmentDate.ToString() & "<br />")

        Return sbPrint.ToString()

    End Function

End Class

'Make changes in this class to Convert it to sortable collection
<Serializable()> Public Class InstallmentQuoteCollection : Inherits SortableCollectionBase

    Public Sub New()
        MyBase.SortObjectType = GetType(InstalmentQuote)
    End Sub

    ''' <summary>
    ''' Print Instalment Quote
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oInstalmentQuote As InstalmentQuote In List
            sbPrint.AppendLine(oInstalmentQuote.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add Instalment Quote List
    ''' </summary>
    ''' <param name="v_oInstalmentQuote"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Add(ByVal v_oInstalmentQuote As InstalmentQuote) As Integer
        Return List.Add(v_oInstalmentQuote)
    End Function

    ''' <summary>
    ''' Remove Instalment Quote List
    ''' </summary>
    ''' <param name="v_oInstalmentQuote"></param>
    ''' <remarks></remarks>
    Public Sub Remove(ByVal v_oInstalmentQuote As InstalmentQuote)
        List.Remove(v_oInstalmentQuote)
    End Sub

    ''' <summary>
    ''' Remove Instalment Quote List at a particular Index
    ''' </summary>
    ''' <param name="index"></param>
    ''' <remarks></remarks>
    Public Shadows Sub RemoveAt(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Add Item Instalment Quote Item List
    ''' </summary>
    ''' <param name="i"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Default Public Property Item(ByVal i As Integer) As InstalmentQuote
        Get
            Return List(i)
        End Get
        Set(ByVal value As InstalmentQuote)
            List(i) = value
        End Set
    End Property

End Class

