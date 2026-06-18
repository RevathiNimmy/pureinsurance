<Serializable()> Public Class MarkUnmarkTransaction

    Private iTransactionKey As Integer

    Private sCurrencyCode As String

    Private dPaymentAmount As Decimal

    Private oMarkStatus As MarkStatusType

    '''<remarks/>
    Public Property TransactionKey() As Integer
        Get
            Return Me.iTransactionKey
        End Get
        Set(ByVal value As Integer)
            Me.iTransactionKey = value
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
    Public Property PaymentAmount() As Decimal
        Get
            Return Me.dPaymentAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dPaymentAmount = value
        End Set
    End Property

    '''<remarks/>
    Public Property MarkStatus() As MarkStatusType
        Get
            Return Me.oMarkStatus
        End Get
        Set(ByVal value As MarkStatusType)
            Me.oMarkStatus = value
        End Set
    End Property

End Class
Public Enum MarkStatusType

    '''<remarks/>
    Mark

    '''<remarks/>
    UnMark
End Enum
