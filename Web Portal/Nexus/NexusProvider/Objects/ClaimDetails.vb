<Serializable()> Public Class ClaimDetails : Inherits Claim

    Private iClaimKey As Integer
    Private iBaseClaimKey As Integer
    Private iInsuranceFileKey As Integer
    Private iRiskKey As Integer
    Private sCurrencyCode As String
    Private sUnderwritingYearCode As String
    Private sGisScreenCode As String
    Private sClaimNumber As String
    Private bTimeStamp() As Byte
    Private oClaimPeril As PerilCollection



    Public Property TimeStamp() As Byte()
        Get
            Return Me.bTimeStamp
        End Get
        Set(ByVal value As Byte())
            Me.bTimeStamp = value
        End Set
    End Property

    '''<remarks/>
    Public Property CurrencyCode() As String
        Get
            Return Me.sCurrencyCode
        End Get
        Set(ByVal value As String)
            Me.sCurrencyCode = value
        End Set
    End Property


    '''<remarks/>
    Public Property GisScreenCode() As String
        Get
            Return Me.sGisScreenCode
        End Get
        Set(ByVal value As String)
            Me.sGisScreenCode = value
        End Set
    End Property

    Public Shadows Property ClaimPeril() As PerilCollection
        Get
            Return Me.oClaimPeril
        End Get
        Set(ByVal value As PerilCollection)
            Me.oClaimPeril = value
        End Set
    End Property

    Public Sub New()
        oClaimPeril = New PerilCollection
    End Sub
End Class
<Serializable()> Public Class ClaimContact : Inherits ClaimDetails

    Public Shadows Function Print() As String

        Dim sbPrint As New Text.StringBuilder

        sbPrint.AppendLine("<br />")

        Return sbPrint.ToString()

    End Function
End Class
<Serializable()> Public Class ClaimContactCollection : Inherits CollectionBase

    Public Function Add(ByVal v_oClaimContact As ClaimContact) As Integer
        Return List.Add(v_oClaimContact)
    End Function

    Public Sub Remove(ByVal v_oClaimContact As ClaimContact)
        List.Remove(v_oClaimContact)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As ClaimContact
        Get
            Return List(i)
        End Get
        Set(ByVal value As ClaimContact)
            List(i) = value
        End Set
    End Property


End Class

