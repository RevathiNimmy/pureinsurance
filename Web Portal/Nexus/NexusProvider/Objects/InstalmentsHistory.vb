<Serializable()> Public Class InstalmentsHistory
    Private dtPostedDateField As Date
    Private sPFIStatusDescriptionField As String
    Private sPFIResultDescriptionField As String
    Private sPFIResultCodeField As String

    '''<remarks/>
    Public Property PostedDate() As Date
        Get
            Return Me.dtPostedDateField
        End Get
        Set(ByVal value As Date)
            Me.dtPostedDateField = value
        End Set
    End Property
    '''<remarks/>
    Public Property PFIStatusDescription() As String
        Get
            Return Me.sPFIStatusDescriptionField
        End Get
        Set(ByVal value As String)
            Me.sPFIStatusDescriptionField = value
        End Set
    End Property

    '''<remarks/>
    Public Property PFIResultDescription() As String
        Get
            Return Me.sPFIResultDescriptionField
        End Get
        Set(ByVal value As String)
            Me.sPFIResultDescriptionField = value
        End Set
    End Property

    '''<remarks/>
    Public Property PFIResultCode() As String
        Get
            Return Me.sPFIResultCodeField
        End Get
        Set(ByVal value As String)
            Me.sPFIResultCodeField = value
        End Set
    End Property
End Class

<Serializable()> Public Class InstalmentsHistoryCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oInstalments As Instalment In List
            'sbPrint.AppendLine(oInstalments.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oHistory As InstalmentsHistory) As Integer
        Return List.Add(v_oHistory)
    End Function

    Public Sub Remove(ByVal v_oHistory As InstalmentsHistory)
        List.Remove(v_oHistory)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As InstalmentsHistory
        Get
            Return List(i)
        End Get
        Set(ByVal value As InstalmentsHistory)
            List(i) = value
        End Set
    End Property

End Class
