<Serializable()> Public Class PartySummary

    Private oClaims As Object
    Private oParty As BaseParty
    Private oPolicies As PolicyCollection
    Private oPartyType As NexusProvider.PartyType




    Public Sub New()
        oClaims = New String("Not yet implemented")
    End Sub

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Claims : NOT SUPPORTED YET<br />")
        sbPrint.AppendLine("Party ---------------><br />")
        sbPrint.AppendLine(Party.Print())

        sbPrint.AppendLine("Policies ---------------><br />")

        If oPolicies IsNot Nothing Then
            sbPrint.AppendLine(oPolicies.Print())
            sbPrint.AppendLine("<br />")
        End If

        Return sbPrint.ToString()

    End Function

    Public Property Party() As BaseParty
        Get
            Return oParty
        End Get
        Set(ByVal value As BaseParty)
            oParty = value
        End Set
    End Property

    Public Property Policies() As PolicyCollection
        Get
            Return oPolicies
        End Get
        Set(ByVal value As PolicyCollection)
            oPolicies = value
        End Set
    End Property

    Public Property Claims() As Object
        Get
            Return oClaims
        End Get
        Set(ByVal value As Object)
            oClaims = value
        End Set
    End Property
    Public Property PartyType() As PartyType

        Get
            Return oPartyType

        End Get
        Set(ByVal value As PartyType)
            oPartyType = value
        End Set
    End Property

End Class
