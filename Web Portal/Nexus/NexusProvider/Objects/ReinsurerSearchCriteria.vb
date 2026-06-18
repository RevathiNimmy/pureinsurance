<Serializable()> Public Class ReinsurerSearchCriteria

    Private sRICode As String
    Private sRIName As String
    Private sRITypeCode As String
    Private bIsRetained As Boolean
    Private sFileCode As String
    Private bIsBroker, bIsBrokerSpecified As Boolean
    Private iMaxRowsToFetch As Integer
    Private bIsFAX, bIsFAXSpecified, bMaxRowsToFetchSpecified As Boolean


    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder

        sbPrint.AppendLine("RI Code : " & sRICode & "<br />")
        sbPrint.AppendLine("RI Name : " & sRIName & "<br />")
       
        sbPrint.AppendLine("RI Type Code : " & sRITypeCode & "<br />")
        sbPrint.AppendLine("IsRetained : " & bIsRetained & "<br />")
        sbPrint.AppendLine("FileCode : " & sFileCode & "<br />")
        sbPrint.AppendLine("IsBroker : " & bIsBroker & "<br />")
        sbPrint.AppendLine("MaxRowsToFetch : " & iMaxRowsToFetch & "<br />")

        Return sbPrint.ToString()

    End Function
    Public Property RICode() As String
        Get
            Return sRICode
        End Get
        Set(ByVal value As String)
            sRICode = value
        End Set
    End Property

    Public Property RIName() As String
        Get
            Return sRIName
        End Get
        Set(ByVal value As String)
            sRIName = value
        End Set
    End Property

    Public Property RITypeCode() As String
        Get
            Return sRITypeCode
        End Get
        Set(ByVal value As String)
            sRITypeCode = value
        End Set
    End Property

    Public Property IsFAX() As Boolean
        Get
            Return bIsFAX
        End Get
        Set(ByVal value As Boolean)
            bIsFAX = value
        End Set
    End Property

    Public Property IsFAXSpecified() As Boolean
        Get
            Return bIsFAxSpecified
        End Get
        Set(ByVal value As Boolean)
            bIsFAxSpecified = value
        End Set
    End Property

    Public Property IsRetained() As Boolean
        Get
            Return bIsRetained
        End Get
        Set(ByVal value As Boolean)
            bIsRetained = value
        End Set
    End Property

    Public Property FileCode() As String
        Get
            Return sFileCode
        End Get
        Set(ByVal value As String)
            sFileCode = value
        End Set
    End Property

    Public Property IsBroker() As Boolean
        Get
            Return bIsBroker
        End Get
        Set(ByVal value As Boolean)
            bIsBroker = value
        End Set
    End Property
    Public Property IsBrokerSpecified() As Boolean
        Get
            Return bIsBrokerSpecified
        End Get
        Set(ByVal value As Boolean)
            bIsBrokerSpecified = value
        End Set
    End Property

    Public Property MaxRowsToFetch() As Integer
        Get
            Return iMaxRowsToFetch
        End Get
        Set(ByVal value As Integer)
            iMaxRowsToFetch = value
        End Set
    End Property
    Public Property MaxRowsToFetchSpecified() As Boolean
        Get
            Return bMaxRowsToFetchSpecified
        End Get
        Set(ByVal value As Boolean)
            bMaxRowsToFetchSpecified = value
        End Set
    End Property
   
End Class

