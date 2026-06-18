<Serializable()> Public Class FinancePlanInformation
    Private sProductCode As String
    Private nOriginalInsuranceFileKey As Integer
    Private nPremiumFinanceKey As Integer
    Private nPremiumFinanceVersion As Integer

    ''' <summary>
    ''' 
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
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OriginalInsuranceFileKey() As Integer
        Get
            Return nOriginalInsuranceFileKey
        End Get
        Set(ByVal value As Integer)
            nOriginalInsuranceFileKey = value
        End Set
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PremiumFinanceKey() As Integer
        Get
            Return nPremiumFinanceKey
        End Get
        Set(ByVal value As Integer)
            nPremiumFinanceKey = value
        End Set
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PremiumFinanceVersion() As Integer
        Get
            Return nPremiumFinanceVersion
        End Get
        Set(ByVal value As Integer)
            nPremiumFinanceVersion = value
        End Set
    End Property

End Class
