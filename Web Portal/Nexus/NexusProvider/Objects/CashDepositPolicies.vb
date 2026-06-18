<Serializable()> Public Class CashDepositPolicies

    Private iCashDepositKey, iAccountKey, iPartyKey As Integer
    Private dAmount, dAvailableBalance As Decimal
    Private sCashDepositRef, sCurrencyCode As String
    Private dtDateCreated As DateTime

    Public Sub New()
    End Sub

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder

        sbPrint.AppendLine("Cash Deposit Key : " & iCashDepositKey.ToString & "<br />")
        sbPrint.AppendLine("Account Key : " & iAccountKey.ToString & "<br />")
        sbPrint.AppendLine("Party Key : " & iPartyKey.ToString & "<br />")
        sbPrint.AppendLine("Amount : " & dAmount.ToString & "<br />")
        sbPrint.AppendLine("Available Balance : " & dAvailableBalance.ToString & "<br />")
        sbPrint.AppendLine("Cash Deposit Ref : " & sCashDepositRef.ToString & "<br />")
        sbPrint.AppendLine("Date Created : " & dtDateCreated.ToString & "<br />")

        Return sbPrint.ToString()

    End Function

    Public Property CashDepositKey() As Integer
        Get
            Return iCashDepositKey
        End Get
        Set(ByVal value As Integer)
            iCashDepositKey = value
        End Set
    End Property

    Public Property AccountKey() As Integer
        Get
            Return iAccountKey
        End Get
        Set(ByVal value As Integer)
            iAccountKey = value
        End Set
    End Property

    Public Property PartyKey() As Integer
        Get
            Return iPartyKey
        End Get
        Set(ByVal value As Integer)
            iPartyKey = value
        End Set
    End Property

    Public Property Amount() As Decimal
        Get
            Return dAmount
        End Get
        Set(ByVal value As Decimal)
            dAmount = value
        End Set
    End Property

    Public Property AvailableBalance() As Decimal
        Get
            Return dAvailableBalance
        End Get
        Set(ByVal value As Decimal)
            dAvailableBalance = value
        End Set
    End Property

    Public Property CashDepositRef() As String
        Get
            Return sCashDepositRef
        End Get
        Set(ByVal value As String)
            sCashDepositRef = value
        End Set
    End Property

    Public Property DateCreated() As DateTime
        Get
            Return dtDateCreated
        End Get
        Set(ByVal value As DateTime)
            dtDateCreated = value
        End Set
    End Property

    Public Property CurrencyCode() As String
        Get
            Return sCurrencyCode
        End Get
        Set(ByVal value As String)
            sCurrencyCode = value
        End Set
    End Property

End Class

<Serializable()> Public Class CashDepositPoliciesCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oCashDepositPolicies As CashDepositPolicies In List
            sbPrint.AppendLine(oCashDepositPolicies.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oCashDepositPolicies As CashDepositPolicies) As Integer
        Return List.Add(v_oCashDepositPolicies)
    End Function

    Public Sub Remove(ByVal v_oCashDepositPolicies As CashDepositPolicies)
        List.Remove(v_oCashDepositPolicies)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As CashDepositPolicies
        Get
            Return List(i)
        End Get
        Set(ByVal value As CashDepositPolicies)
            List(i) = value
        End Set
    End Property

End Class