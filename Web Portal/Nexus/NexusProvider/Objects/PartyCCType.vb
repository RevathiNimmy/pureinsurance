Public Class PartyCCType

    Private sCompanyName As String

    Private sBusinessCode As String

    Private sMainContact As String

    Private iNumberOfOffices As Integer

    Private bNumberOfEmployees As Boolean

    Private oClientDetail As BaseClientSharedDataType

    Private sCompanyReg As String

    Private sTradeCode As String

    Private sSICCode As String

    Private dTradingSince As Date

    Private dWageRoll As Decimal

    Private sTurnoverCode As String

    Private dFinancialYear As Date

    Private sSalutation As String

    Private bTPS As Boolean

    Private bMPS As Boolean

    Private beMPS As Boolean

    Private sSource As String

    Private sAlternativeId As String

    '''<remarks/>
    Public Property CompanyName() As String
        Get
            Return Me.sCompanyName
        End Get
        Set(ByVal value As String)
            Me.sCompanyName = value
        End Set
    End Property

    '''<remarks/>
    Public Property BusinessCode() As String
        Get
            Return Me.sBusinessCode
        End Get
        Set(ByVal value As String)
            Me.sBusinessCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property MainContact() As String
        Get
            Return Me.sMainContact
        End Get
        Set(ByVal value As String)
            Me.sMainContact = value
        End Set
    End Property

    '''<remarks/>
    Public Property NumberOfOffices() As Integer
        Get
            Return Me.iNumberOfOffices
        End Get
        Set(ByVal value As Integer)
            Me.iNumberOfOffices = value
        End Set
    End Property

   

    '''<remarks/>
    Public Property NumberOfEmployees() As Integer
        Get
            Return Me.bNumberOfEmployees
        End Get
        Set(ByVal value As Integer)
            Me.bNumberOfEmployees = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClientDetail() As BaseClientSharedDataType
        Get
            Return Me.clientDetailField
        End Get
        Set(ByVal value As BaseClientSharedDataType)
            Me.clientDetailField = value
        End Set
    End Property

    '''<remarks/>
    Public Property CompanyReg() As String
        Get
            Return Me.sCompanyReg
        End Get
        Set(ByVal value As String)
            Me.sCompanyReg = value
        End Set
    End Property

    '''<remarks/>
    Public Property TradeCode() As String
        Get
            Return Me.sTradeCode
        End Get
        Set(ByVal value As String)
            Me.sTradeCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property SICCode() As String
        Get
            Return Me.sSICCode
        End Get
        Set(ByVal value As String)
            Me.sSICCode = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
    Public Property TradingSince() As Date
        Get
            Return Me.dTradingSince
        End Get
        Set(ByVal value As Date)
            Me.dTradingSince = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlIgnoreAttribute()> _
    Public Property TradingSinceSpecified() As Boolean
        Get
            Return Me.tradingSinceFieldSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.tradingSinceFieldSpecified = value
        End Set
    End Property

    '''<remarks/>
    Public Property WageRoll() As Decimal
        Get
            Return Me.dWageRoll
        End Get
        Set(ByVal value As Decimal)
            Me.dWageRoll = value
        End Set
    End Property

    

    '''<remarks/>
    Public Property TurnoverCode() As String
        Get
            Return Me.sTurnoverCode
        End Get
        Set(ByVal value As String)
            Me.sTurnoverCode = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
    Public Property FinancialYear() As Date
        Get
            Return Me.dFinancialYear
        End Get
        Set(ByVal value As Date)
            Me.dFinancialYear = value
        End Set
    End Property

   

    '''<remarks/>
    Public Property Salutation() As String
        Get
            Return Me.sSalutation
        End Get
        Set(ByVal value As String)
            Me.sSalutation = value
        End Set
    End Property

    '''<remarks/>
    Public Property TPS() As Boolean
        Get
            Return Me.bTPS
        End Get
        Set(ByVal value As Boolean)
            Me.bTPS = value
        End Set
    End Property


    '''<remarks/>
    Public Property MPS() As Boolean
        Get
            Return Me.bMPS
        End Get
        Set(ByVal value As Boolean)
            Me.bMPS = value
        End Set
    End Property

    '''<remarks/>
    Public Property eMPS() As Boolean
        Get
            Return Me.beMPS
        End Get
        Set(ByVal value As Boolean)
            Me.beMPS = value
        End Set
    End Property


    '''<remarks/>
    Public Property Source() As String
        Get
            Return Me.sSource
        End Get
        Set(ByVal value As String)
            Me.sSource = value
        End Set
    End Property

    '''<remarks/>
    Public Property AlternativeId() As String
        Get
            Return Me.sAlternativeId
        End Get
        Set(ByVal value As String)
            Me.sAlternativeId = value
        End Set
    End Property
End Class
