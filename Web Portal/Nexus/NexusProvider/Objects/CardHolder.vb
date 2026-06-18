
<Serializable()> Public Class CardHolder : Inherits Address

    Private sName As String

    '''<remarks/>
    Public Property Name() As String
        Get
            Return Me.sName
        End Get
        Set(ByVal value As String)
            Me.sName = value
        End Set
    End Property
End Class
