Public NotInheritable Class ValueDescriptionPair 
    Private sName As String
    Private iID As Integer
    Private iIndex As Integer

    Public Sub New(ByVal Name As String, ByVal ID As Integer)
        sName = Name
        iID = ID

    End Sub
    Public Sub New(ByVal Name As String, ByVal ID As Integer, ByVal Index As Integer)
        sName = Name
        iID = ID
        iIndex = Index
    End Sub

    Public Property Name() As String
        Get
            Return sName
        End Get

        Set(ByVal sValue As String)
            sName = sValue
        End Set
    End Property
    Public Property Index() As Integer
        Get
            Return iIndex
        End Get
        Set(ByVal Value As Integer)
            iIndex = Value
        End Set
    End Property


    Public Property ItemData() As Integer
        Get
            Return iID
        End Get

        Set(ByVal iValue As Integer)
            iID = iValue
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return sName
    End Function

End Class
