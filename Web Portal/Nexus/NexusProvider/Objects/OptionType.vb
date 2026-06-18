
<Serializable()> Public Class OptionTypeSetting

    Private sOptionValue As String
    Private sUnderwritingType As String
    Private sAccountType As String


    Public Property OptionValue() As String
        Get
            Return sOptionValue
        End Get
        Set(ByVal value As String)
            sOptionValue = value
        End Set
    End Property

    Public Property UnderwritingType() As String
        Get
            Return sUnderwritingType
        End Get
        Set(ByVal value As String)
            sUnderwritingType = value
        End Set
    End Property

    Public Property AccountType() As String
        Get
            Return sAccountType
        End Get
        Set(ByVal value As String)
            sAccountType = value
        End Set
    End Property


End Class
