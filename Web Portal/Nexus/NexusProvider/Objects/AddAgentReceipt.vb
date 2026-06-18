<Serializable()> Public Class AddAgentReceipt

    Private partyKeyField As Integer

    Private receiptTypeField As ReceiptType

    Public Sub New()
        receiptTypeField = New ReceiptType
    End Sub

    '''<remarks/>
    Public Property PartyKey() As Integer
        Get
            Return Me.partyKeyField
        End Get
        Set(ByVal value As Integer)
            Me.partyKeyField = value
        End Set
    End Property

    '''<remarks/>
    Public Property Receipt() As ReceiptType
        Get
            Return Me.receiptTypeField
        End Get
        Set(ByVal value As ReceiptType)
            Me.receiptTypeField = value
        End Set
    End Property



End Class
