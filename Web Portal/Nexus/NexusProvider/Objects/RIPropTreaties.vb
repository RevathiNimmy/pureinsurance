<Serializable()> Public Class RIPropTreaties
    Public Property TreatyId() As Integer
    Public Property TreatyCode() As String
    Public Property TreatyDescription() As String
    Public Property ReinsuranceCode() As String

    Public Overridable Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("TreatyId : " & TreatyId & "<br />")
        sbPrint.AppendLine("TreatyCode : " & TreatyCode & "<br />")
        sbPrint.AppendLine("TreatyDescription : " & TreatyDescription & "<br />")
        sbPrint.AppendLine("ReinsuranceCode : " & ReinsuranceCode & "<br />")
        Return sbPrint.ToString

    End Function
End Class

<Serializable()> Public Class RIPropTreatiesCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oRITreatyParty As RIPropTreaties In List
            sbPrint.AppendLine(oRITreatyParty.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function
    Public Function Add(ByVal v_oRITreatyParty As RIPropTreaties) As Integer
        Return List.Add(v_oRITreatyParty)
    End Function

    Public Sub Remove(ByVal v_oRITreatyParty As RIPropTreaties)
        List.Remove(v_oRITreatyParty)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As RIPropTreaties
        Get
            Return List(i)
        End Get
        Set(ByVal value As RIPropTreaties)
            List(i) = value
        End Set
    End Property

End Class



