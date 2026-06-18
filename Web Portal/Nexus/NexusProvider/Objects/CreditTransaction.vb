<Serializable()> Public Class CreditTransaction
    Private iAccountKey As Integer

    Private iTransDetailKey As Integer

    Private dAmount As Double

    Private dtcollectionDate As Date


    Public Property AccountKey() As Integer
        Get
            Return iAccountKey
        End Get
        Set(ByVal value As Integer)
            iAccountKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property TransDetailKey() As Integer
        Get
            Return iTransDetailKey
        End Get
        Set(ByVal value As Integer)
            iTransDetailKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property Amount() As Double
        Get
            Return dAmount
        End Get
        Set(ByVal value As Double)
            dAmount = value
        End Set
    End Property

    '''<remarks/>
    Public Property CollectionDate() As Date
        Get
            Return dtcollectionDate
        End Get
        Set(ByVal value As Date)
            dtcollectionDate = value
        End Set
    End Property
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder

        sbPrint.AppendLine("Sum Amount : " & iAccountKey.ToString() & "<br />")
        sbPrint.AppendLine("Currency Code : " & iTransDetailKey & "<br />")
        sbPrint.AppendLine("Float Balance : " & dAmount.ToString() & "<br />")
        sbPrint.AppendLine("Overdraft : " & dtcollectionDate.ToString() & "<br />")
        sbPrint.AppendLine("<br />")

        Return sbPrint.ToString()

    End Function
End Class

<Serializable()> Public Class CreditTransactionCollection : Inherits CollectionBase
    Public Function Add(ByVal v_oCreditTransaction As CreditTransaction) As Integer
        Return List.Add(v_oCreditTransaction)
    End Function

    Public Sub Remove(ByVal v_oCreditTransaction As CreditTransaction)
        List.Remove(v_oCreditTransaction)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As CreditTransaction
        Get
            Return List(i)
        End Get
        Set(ByVal value As CreditTransaction)
            List(i) = value
        End Set
    End Property
End Class
