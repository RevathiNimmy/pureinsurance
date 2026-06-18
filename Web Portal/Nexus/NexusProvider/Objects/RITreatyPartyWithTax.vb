<Serializable()> Public Class RITreatyPartyWithTax
    Public Property InsuranceFileID() As Integer
    Public Property RiskID() As Integer
    Public Property RIArrangementLineID() As Integer
    Public Property TreatyID() As Integer
    Public Property Premium() As Decimal
    Public Property Commission() As Decimal
    Public Property PremiumTransType() As String
    Public Property CommissionTransType() As String
    Public Property TreatyCode() As String
    Public Property IgnoreDetails() As Boolean
    Public Property IgnoreTax() As Boolean
    Public Property PremiumTax() As Decimal
    Public Property CommissionTax() As Decimal
    Public Property CommissionPercent() As Decimal
    Public Property IsRetained() As Integer
    Public Property AgreementCode() As String
    Public Overridable Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("InsuranceFileID : " & InsuranceFileID & "<br />")
        sbPrint.AppendLine("RiskID : " & RiskID & "<br />")
        sbPrint.AppendLine("RIArrangementLineID : " & RIArrangementLineID & "<br />")
        sbPrint.AppendLine("TreatyID : " & TreatyID & "<br />")
        sbPrint.AppendLine("Premium : " & Premium & "<br />")
        sbPrint.AppendLine("Commission : " & Commission & "<br />")
        sbPrint.AppendLine("PremiumTransType : " & PremiumTransType & "<br />")
        sbPrint.AppendLine("CommissionTransType : " & CommissionTransType & "<br />")
        sbPrint.AppendLine("TreatyCode : " & TreatyCode & "<br />")
        sbPrint.AppendLine("IgnoreDetails : " & IgnoreDetails & "<br />")
        sbPrint.AppendLine("IgnoreTax : " & IgnoreTax & "<br />")
        sbPrint.AppendLine("PremiumTax : " & PremiumTax & "<br />")
        sbPrint.AppendLine("CommissionTax : " & CommissionTax & "<br />")
        sbPrint.AppendLine("CommissionPercent : " & CommissionPercent & "<br />")
        sbPrint.AppendLine("IsRetained : " & IsRetained & "<br />")
        sbPrint.AppendLine("AgreementCode : " & AgreementCode & "<br />")
        Return sbPrint.ToString

    End Function
End Class

<Serializable()> Public Class RITreatyPartyWithTaxCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oRITreatyParty As RITreatyPartyWithTax In List
            sbPrint.AppendLine(oRITreatyParty.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function
    Public Function Add(ByVal v_oRITreatyPartyWithTax As RITreatyPartyWithTax) As Integer
        Return List.Add(v_oRITreatyPartyWithTax)
    End Function

    Public Sub Remove(ByVal v_oRITreatyPartyWithTax As RITreatyPartyWithTax)
        List.Remove(v_oRITreatyPartyWithTax)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As RITreatyPartyWithTax
        Get
            Return List(i)
        End Get
        Set(ByVal value As RITreatyPartyWithTax)
            List(i) = value
        End Set
    End Property

End Class



