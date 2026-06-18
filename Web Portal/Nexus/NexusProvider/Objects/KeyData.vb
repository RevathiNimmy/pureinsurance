''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class KeyDataCollection : Inherits CollectionBase
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oKeyField As KeyData In List
            sbPrint.AppendLine(oKeyField.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oKeyField As KeyData) As Integer
        Return List.Add(v_oKeyField)
    End Function

    Public Sub Remove(ByVal v_oKeyField As KeyData)
        List.Remove(v_oKeyField)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As KeyData
        Get
            Return List(i)
        End Get
        Set(ByVal value As KeyData)
            List(i) = value
        End Set
    End Property
End Class


''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class KeyData

    Private skeyNameField As String

    Private skeyValueField As String

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder

        sbPrint.AppendLine("Key Name Field : " & skeyNameField.ToString() & "<br />")
        sbPrint.AppendLine("Total Annual Tax : " & skeyValueField.ToString() & "<br />")

        Return sbPrint.ToString()

    End Function

    Public Property KeyName() As String
        Get
            Return Me.skeyNameField
        End Get
        Set(ByVal value As String)
            Me.skeyNameField = value
        End Set
    End Property

    Public Property KeyValue() As String
        Get
            Return Me.skeyValueField
        End Get
        Set(ByVal value As String)
            Me.skeyValueField = value
        End Set
    End Property
End Class
