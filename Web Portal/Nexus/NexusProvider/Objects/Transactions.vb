''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class Transactions

#Region "Private Variables"

    Private sAccountCode As String
    Private dAmount As Decimal
    Private sUnderwritingYearCode As String
    Private sComment As String
    Private dtTransactionDate As DateTime
    Private iPeriodID As Integer
    Private sUserName As String
    Private sReference As String

#End Region

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Account Code : " & sAccountCode.ToString() & "<br />")
        sbPrint.AppendLine("Amount : " & dAmount.ToString() & "<br />")
        sbPrint.AppendLine("Underwriting Year Code : " & sUnderwritingYearCode.ToString() & "<br />")
        sbPrint.AppendLine("Comment : " & sComment.ToString() & "<br />")
        sbPrint.AppendLine("Transaction Date : " & dtTransactionDate.ToString() & "<br />")
        sbPrint.AppendLine("Period ID : " & iPeriodID.ToString() & "<br />")
        sbPrint.AppendLine("User Name : " & sUserName.ToString() & "<br />")
        sbPrint.AppendLine("sReference : " & sReference.ToString() & "<br />")

        Return sbPrint.ToString()

    End Function

#Region "Public Property"

    Public Property AccountCode() As String
        Get
            Return Me.sAccountCode
        End Get
        Set(ByVal value As String)
            Me.sAccountCode = value
        End Set
    End Property

    Public Property Amount() As Decimal
        Get
            Return Me.dAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dAmount = value
        End Set
    End Property

    Public Property UnderwritingYearCode() As String
        Get
            Return Me.sUnderwritingYearCode
        End Get
        Set(ByVal value As String)
            Me.sUnderwritingYearCode = value
        End Set
    End Property

    Public Property Comment() As String
        Get
            Return Me.sComment
        End Get
        Set(ByVal value As String)
            Me.sComment = value
        End Set
    End Property

    Public Property TransactionDate() As DateTime
        Get
            Return Me.dtTransactionDate
        End Get
        Set(ByVal value As DateTime)
            Me.dtTransactionDate = value
        End Set
    End Property

    Public Property PeriodID() As Integer
        Get
            Return Me.iPeriodID
        End Get
        Set(ByVal value As Integer)
            Me.iPeriodID = value
        End Set
    End Property

    Public Property UserName() As String
        Get
            Return Me.sUserName
        End Get
        Set(ByVal value As String)
            Me.sUserName = value
        End Set
    End Property

    Public Property Reference() As String
        Get
            Return Me.sReference
        End Get
        Set(ByVal value As String)
            Me.sReference = value
        End Set
    End Property

#End Region

End Class

''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class TransactionsCollections : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oTransactions As Transactions In List
            sbPrint.AppendLine(oTransactions.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oTransactions As Transactions) As Integer
        Return List.Add(v_oTransactions)
    End Function

    Public Sub Remove(ByVal v_oTransactions As Transactions)
        List.Remove(v_oTransactions)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As Transactions
        Get
            Return List(i)
        End Get
        Set(ByVal value As Transactions)
            List(i) = value
        End Set
    End Property

End Class