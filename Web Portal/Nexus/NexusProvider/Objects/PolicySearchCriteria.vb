<Serializable()> Public Class PolicySearchCriteria
    Private sPolicyNumber As String
    Private sRecordType As String
    Private sproduct As String
    Private sAgentName As String
    Private sQuoteDate As Date
    Private sStartDate As Date
    Private sInsuredName As String

    Public Property PolicyNumber() As String
        Get
            Return sPolicyNumber
        End Get
        Set(ByVal value As String)
            sPolicyNumber = value
        End Set
    End Property

    Public Property RecordType() As String
        Get
            Return sRecordType
        End Get
        Set(ByVal value As String)
            sRecordType = value
        End Set
    End Property

    Public Property product() As String
        Get
            Return sproduct
        End Get
        Set(ByVal value As String)
            sproduct = value
        End Set
    End Property

    Public Property AgentName() As String
        Get
            Return sAgentName
        End Get
        Set(ByVal value As String)
            sAgentName = value
        End Set
    End Property

    Public Property QuoteDate() As Date
        Get
            Return sQuoteDate
        End Get
        Set(ByVal value As Date)
            sQuoteDate = value
        End Set
    End Property
    Public Property StartDate() As Date
        Get
            Return sStartDate
        End Get
        Set(ByVal value As Date)
            sStartDate = value
        End Set
    End Property
    Public Property InsuredName() As String
        Get
            Return sInsuredName
        End Get
        Set(ByVal value As String)
            sInsuredName = value
        End Set
    End Property
End Class
