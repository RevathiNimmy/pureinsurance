<Serializable()> Public Class StoredProcedureResponseType

    Private sResults As String

    Public Sub New()

    End Sub
    Public Property Results() As String
        Get
            Return Me.sResults
        End Get
        Set
            Me.sResults = Value
        End Set
    End Property
End Class
