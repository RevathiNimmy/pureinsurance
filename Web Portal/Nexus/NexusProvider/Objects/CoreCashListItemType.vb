'added by radha
Public Class CoreCashListItemType

    Private bIsProduceDocument As Boolean
    Private sBankReference As String
    Private sMediaTypeCode As String
    Private dTransactionDate As Date
    Private sAccountShortCode As String
    Private dAmount As Decimal
    Private sAllocationStatusCode As String
    Private sMediaReference As String
    Private sOurReference As String
    Private sTheirReference As String
    Private sContactName As String
    Private oContactAddress As Address
    Private sFurtherDetails As String


    Public Property IsProduceDocument() As Boolean
        Get
            Return bIsProduceDocument
        End Get
        Set(ByVal value As Boolean)
            bIsProduceDocument = value
        End Set
    End Property

    Public Property BankReference() As String
        Get
            Return sBankReference
        End Get
        Set(ByVal value As String)
            sBankReference = value
        End Set
    End Property
    Public Property MediaTypeCode() As String
        Get
            Return sMediaTypeCode
        End Get
        Set(ByVal value As String)
            sMediaTypeCode = value
        End Set
    End Property
    Public Property TransactionDate() As Date
        Get
            Return dTransactionDate
        End Get
        Set(ByVal value As Date)
            dTransactionDate = value
        End Set
    End Property

    Public Property AccountShortCode() As String
        Get
            Return sAccountShortCode
        End Get
        Set(ByVal value As String)
            sAccountShortCode = value
        End Set
    End Property
    Public Property Amount() As Decimal
        Get
            Return dAmount
        End Get
        Set(ByVal value As Decimal)
            dAmount = value
        End Set
    End Property

    Public Property AllocationStatusCode() As String
        Get
            Return sAllocationStatusCode
        End Get
        Set(ByVal value As String)
            sAllocationStatusCode = value
        End Set
    End Property

    Public Property MediaReference() As String
        Get
            Return sMediaReference
        End Get
        Set(ByVal value As String)
            sMediaReference = value
        End Set
    End Property

    Public Property OurReference() As String
        Get
            Return sOurReference
        End Get
        Set(ByVal value As String)
            sOurReference = value
        End Set
    End Property

    Public Property TheirReference() As String
        Get
            Return sTheirReference
        End Get
        Set(ByVal value As String)
            sTheirReference = value
        End Set
    End Property

    Public Property ContactName() As String
        Get
            Return sContactName
        End Get
        Set(ByVal value As String)
            sContactName = value
        End Set
    End Property

    Public Property ContactAddress() As Address
        Get
            Return oContactAddress
        End Get
        Set(ByVal value As Address)
            oContactAddress = value
        End Set
    End Property

    Public Property FurtherDetails() As String
        Get
            Return sFurtherDetails
        End Get
        Set(ByVal value As String)
            sFurtherDetails = value
        End Set
    End Property

End Class
