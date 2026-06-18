''' <summary>
''' Property Class for Find User
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class FindUsersSearchCriteria

    Private sUserNameField As String
    Private sFullNameField As String
    Private iMaxRowsToFetch As Integer


    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("User Name : " & sUserNameField & "<br />")
        sbPrint.AppendLine("Full Name : " & sFullNameField & "<br />")
        sbPrint.AppendLine("Max Rows ToFetch: " & iMaxRowsToFetch.ToString & "<br />")

        Return sbPrint.ToString()

    End Function

    Public Property UserName() As String
        Get
            Return Me.sUserNameField
        End Get
        Set(ByVal value As String)
            Me.sUserNameField = value
        End Set
    End Property

    Public Property FullName() As String
        Get
            Return Me.sFullNameField
        End Get
        Set(ByVal value As String)
            Me.sFullNameField = value
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
