<Serializable()> Public Class BankSearchCriteria

    Private sShortCode As String
    Private sBankName As String
    Private iMaxRowsToFetch As Integer

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Type : " & Me.GetType.Name & "<br />")
        sbPrint.AppendLine("Short Code : " & sShortCode & "<br />")
        sbPrint.AppendLine("Bank Name : " & sBankName & "<br />")
        sbPrint.AppendLine("Max Rows ToFetch : " & iMaxRowsToFetch & "<br />")

        Return sbPrint.ToString

    End Function

    '''<remarks/>
    Public Property ShortCode() As String
        Get
            Return Me.sShortCode
        End Get
        Set(ByVal value As String)
            Me.sShortCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property BankName() As String
        Get
            Return Me.sBankName
        End Get
        Set(ByVal value As String)
            Me.sBankName = value
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

End Class
