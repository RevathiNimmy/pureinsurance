<Serializable()> Public Class MidFile
    Dim iMIDFileKey As Integer
    Dim sFileSequenceNumber As String
    Dim dDateGenerated As DateTime
    Dim sStatusDescription As String
    Dim sFileName As String
    Sub New()

    End Sub
    Public Overridable Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("MIDFileKey : " & iMIDFileKey.ToString & "<br />")
        sbPrint.AppendLine("DateGenerated : " & dDateGenerated & "<br />")
        sbPrint.AppendLine("FileSequenceNumber : " & FileSequenceNumber & "<br />")
        sbPrint.AppendLine("StatusDescription: " & StatusDescription & "<br />")
        Return sbPrint.ToString

    End Function
    Public Property MIDFileKey() As Integer
        Get
            Return iMIDFileKey
        End Get
        Set(ByVal value As Integer)
            iMIDFileKey = value
        End Set
    End Property
    Public Property FileSequenceNumber() As String
        Get
            Return sFileSequenceNumber
        End Get
        Set(ByVal value As String)
            sFileSequenceNumber = value
        End Set
    End Property
    Public Property DateGenerated() As DateTime
        Get
            Return dDateGenerated
        End Get
        Set(ByVal value As DateTime)
            dDateGenerated = value
        End Set
    End Property
    Public Property StatusDescription() As String
        Get
            Return sStatusDescription
        End Get
        Set(ByVal value As String)
            sStatusDescription = value
        End Set
    End Property
    Public Property FileName() As String
        Get
            Return sFileName
        End Get
        Set(ByVal value As String)
            sFileName = value
        End Set
    End Property
End Class
<Serializable()> Public Class MidFileCollection : Inherits SortableCollectionBase

    Public Sub New()
        MyBase.SortObjectType = GetType(MidFile)
    End Sub

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()
        For Each oMidFile As MidFile In List
            sbPrint.AppendLine("<br />")
        Next
        Return sbPrint.ToString()

    End Function
    Public Function Add(ByVal v_oMidFile As MidFile) As Integer
        Return List.Add(v_oMidFile)
    End Function
    Public Sub Remove(ByVal v_oMidFile As MidFile)
        List.Remove(v_oMidFile)
    End Sub
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub
    Default Public Property Item(ByVal i As Integer) As MidFile
        Get
            Return List(i)
        End Get
        Set(ByVal value As MidFile)
            List(i) = value
        End Set
    End Property

    Public Function NumberOfRows() As Integer
        Return List.Count()
    End Function

End Class
