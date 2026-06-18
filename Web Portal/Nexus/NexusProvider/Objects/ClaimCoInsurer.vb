<Serializable()> Public Class ClaimCoInsurer
    Private iPartyKey As Integer
    Private sName As String
    Private dShare As Decimal
    Private dShareValue As Decimal

    Private oClaimCoInsurer As ClaimCoInsurerCollection

    Public Sub New()
        oClaimCoInsurer = New ClaimCoInsurerCollection
    End Sub
    '''<remarks/>
    Public Property PartyKey() As Integer
        Get
            Return Me.iPartyKey
        End Get
        Set(ByVal value As Integer)
            Me.iPartyKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property Name() As String
        Get
            Return Me.sName
        End Get
        Set(ByVal value As String)
            Me.sName = value
        End Set
    End Property

    '''<remarks/>
    Public Property Share() As Decimal
        Get
            Return Me.dShare
        End Get
        Set(ByVal value As Decimal)
            Me.dShare = value
        End Set
    End Property

    '''<remarks/>
    Public Property ShareValue() As Decimal
        Get
            Return Me.dShareValue
        End Get
        Set(ByVal value As Decimal)
            Me.dShareValue = value
        End Set
    End Property
    Public Property ClaimCoInsurer() As ClaimCoInsurerCollection
        Get
            Return Me.oClaimCoInsurer
        End Get
        Set(ByVal value As ClaimCoInsurerCollection)
            Me.oClaimCoInsurer = value
        End Set
    End Property
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder

        sbPrint.AppendLine("PartyKey: " & iPartyKey & "<br />")
        sbPrint.AppendLine("Share : " & sName & "<br />")
        sbPrint.AppendLine("Share Value: " & dShareValue & "<br />")
        sbPrint.AppendLine("<br />")

        sbPrint.AppendLine("CoInsurers ---------------><br />")



        Return sbPrint.ToString()

    End Function
End Class
<Serializable()> Public Class ClaimCoInsurerCollection : Inherits CollectionBase
    Public Function Add(ByVal v_oClaimCoInsurer As ClaimCoInsurer) As Integer
        Return List.Add(v_oClaimCoInsurer)
    End Function

    Public Sub Remove(ByVal v_oClaimCoInsurer As ClaimCoInsurer)
        List.Remove(v_oClaimCoInsurer)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub
    Default Public Property Item(ByVal i As Integer) As ClaimCoInsurer
        Get
            Return List(i)
        End Get
        Set(ByVal value As ClaimCoInsurer)
            List(i) = value
        End Set
    End Property
End Class
