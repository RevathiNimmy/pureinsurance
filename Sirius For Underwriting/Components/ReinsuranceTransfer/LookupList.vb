Public NotInheritable Class LookupList 
    Private iID As Integer
    Private sDescription As String

    Public Sub New(ByVal id As Integer, ByVal Description As String)
        iID = id
        sDescription = Description
    End Sub


    Public Property ID() As Integer
        Get
            Return iID
        End Get
        Set(ByVal value As Integer)
            iID = value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return sDescription
        End Get
        Set(ByVal value As String)
            sDescription = value
        End Set
    End Property

End Class
