<Serializable()> Public Class AccountDetailsDefaults
    Private sAccountName As String
    Private sContactName As String
    Private sPhoneAreaCode As String
    Private sPhoneNumber As String
    Private sPhoneExtension As String
    Private sAccountStatus As String
    Private sAccountBalance As Double
    Private sTransactionCurrencyOutStandingBalance As Double
    Dim oAccountDetails As AccountDetailsCollection

    Public Sub New()
        oAccountDetails = New AccountDetailsCollection
    End Sub

    Public Property AccountDetails() As AccountDetailsCollection
        Get
            Return Me.oAccountDetails
        End Get
        Set(ByVal value As AccountDetailsCollection)
            Me.oAccountDetails = value
        End Set
    End Property

    Public Property AccountName() As String
        Get
            Return Me.sAccountName
        End Get
        Set(ByVal value As String)
            Me.sAccountName = value
        End Set
    End Property


    Public Property ContactName() As String
        Get
            Return Me.sContactName
        End Get
        Set(ByVal value As String)
            Me.sContactName = value
        End Set
    End Property

    Public Property PhoneAreaCode() As String
        Get
            Return Me.sPhoneAreaCode
        End Get
        Set(ByVal value As String)
            Me.sPhoneAreaCode = value
        End Set
    End Property


    Public Property PhoneNumber() As String
        Get
            Return Me.sPhoneNumber
        End Get
        Set(ByVal value As String)
            Me.sPhoneNumber = value
        End Set
    End Property


    Public Property PhoneExtension() As String
        Get
            Return Me.sPhoneExtension
        End Get
        Set(ByVal value As String)
            Me.sPhoneExtension = value
        End Set
    End Property

    Public Property TransactionCurrencyOutStandingBalance() As Double
        Get
            Return Me.sTransactionCurrencyOutStandingBalance
        End Get
        Set(ByVal value As Double)
            Me.sTransactionCurrencyOutStandingBalance = value
        End Set
    End Property

    Public Property AccountStatus() As String
        Get
            Return Me.sAccountStatus
        End Get
        Set(ByVal value As String)
            Me.sAccountStatus = value
        End Set
    End Property
    Public Property AccountBalance() As Double
        Get
            Return Me.sAccountBalance
        End Get
        Set(ByVal value As Double)
            Me.sAccountBalance = value
        End Set
    End Property
End Class
<Serializable()> Public Class AccountDetailsDefaultsCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oAccountDetailsDefaults As AccountDetailsDefaults In List
            'sbPrint.AppendLine(oCoinsuranceDefaults.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oAccountDetailsDefaults As AccountDetailsDefaults) As Integer
        Return List.Add(v_oAccountDetailsDefaults)
    End Function

    Public Sub Remove(ByVal v_oAccountDetailsDefaults As AccountDetailsDefaults)
        List.Remove(v_oAccountDetailsDefaults)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As AccountDetailsDefaults
        Get
            Return List(i)
        End Get
        Set(ByVal value As AccountDetailsDefaults)
            List(i) = value
        End Set
    End Property

End Class