<Serializable()> Public Class CoinsuranceDefaults

    Private iCoinsuranceDefaultId As Integer

    Private sCoinsuranceDefault As String

    Private sCode As String

    Private bIsRecovered As Boolean

    Private bIsSurcharged As Boolean

    Dim oCoInsurer As CoInsurersCollections

    Public Sub New()
        oCoInsurer = New CoInsurersCollections
    End Sub
    Public Property CoInsurer() As CoInsurersCollections
        Get
            Return Me.oCoInsurer
        End Get
        Set(ByVal value As CoInsurersCollections)
            Me.oCoInsurer = value
        End Set
    End Property

    Public Property CoinsuranceDefaultId() As Integer
        Get
            Return Me.iCoinsuranceDefaultId
        End Get
        Set(ByVal value As Integer)
            Me.iCoinsuranceDefaultId = value
        End Set
    End Property


    Public Property CoinsuranceDefault() As String
        Get
            Return Me.sCoinsuranceDefault
        End Get
        Set(ByVal value As String)
            Me.sCoinsuranceDefault = value
        End Set
    End Property


    Public Property Code() As String
        Get
            Return Me.sCode
        End Get
        Set(ByVal value As String)
            Me.sCode = value
        End Set
    End Property


    Public Property IsRecovered() As Boolean
        Get
            Return Me.bIsRecovered
        End Get
        Set(ByVal value As Boolean)
            Me.bIsRecovered = value
        End Set
    End Property


    Public Property IsSurcharged() As Boolean
        Get
            Return Me.bIsSurcharged
        End Get
        Set(ByVal value As Boolean)
            Me.bIsSurcharged = value
        End Set
    End Property

End Class

<Serializable()> Public Class CoinsuranceDefaultsCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oCoinsuranceDefaults As CoinsuranceDefaults In List
            'sbPrint.AppendLine(oCoinsuranceDefaults.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oCoinsuranceDefaults As CoinsuranceDefaults) As Integer
        Return List.Add(v_oCoinsuranceDefaults)
    End Function

    Public Sub Remove(ByVal v_oCoinsuranceDefaults As CoinsuranceDefaults)
        List.Remove(v_oCoinsuranceDefaults)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As CoinsuranceDefaults
        Get
            Return List(i)
        End Get
        Set(ByVal value As CoinsuranceDefaults)
            List(i) = value
        End Set
    End Property

End Class
