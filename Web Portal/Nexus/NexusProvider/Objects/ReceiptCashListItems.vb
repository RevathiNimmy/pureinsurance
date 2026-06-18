<Serializable()> Public Class ReceiptCashListItems
    Private sKey As String
    Private iCashListItemKey As Integer

    Private sMediaReference As String

    Private sMediaType As String

    Private dAmount As Double

    Private sAccountShortCode As String

    Private sStatus As String

    Private bLetter As String

    '''<remarks/>
    Public Property CashListItemKey() As Integer
        Get
            Return Me.iCashListItemKey
        End Get
        Set(ByVal value As Integer)
            Me.iCashListItemKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property MediaReference() As String
        Get
            Return Me.sMediaReference
        End Get
        Set(ByVal value As String)
            Me.sMediaReference = value
        End Set
    End Property

    '''<remarks/>
    Public Property MediaType() As String
        Get
            Return Me.sMediaType
        End Get
        Set(ByVal value As String)
            Me.sMediaType = value
        End Set
    End Property

    '''<remarks/>
    Public Property Amount() As Double
        Get
            Return Me.dAmount
        End Get
        Set(ByVal value As Double)
            Me.dAmount = value
        End Set
    End Property

    '''<remarks/>
    Public Property AccountShortCode() As String
        Get
            Return Me.sAccountShortCode
        End Get
        Set(ByVal value As String)
            Me.sAccountShortCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property Status() As String
        Get
            Return Me.sStatus
        End Get
        Set(ByVal value As String)
            Me.sStatus = value
        End Set
    End Property

    '''<remarks/>
    Public Property Letter() As String
        Get
            Return Me.bLetter
        End Get
        Set(ByVal value As String)
            Me.bLetter = value
        End Set
    End Property
    Public Property Key() As String
        Get
            Return sKey
        End Get
        Set(ByVal value As String)
            sKey = value
        End Set
    End Property
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder


        sbPrint.AppendLine("<br />")

        Return sbPrint.ToString()

    End Function

End Class
<Serializable()> Public Class ReceiptCashListItemsCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oReceiptCashListItems As ReceiptCashListItems In List
            sbPrint.AppendLine(oReceiptCashListItems.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oReceiptCashListItems As ReceiptCashListItems) As Integer
        Return List.Add(v_oReceiptCashListItems)
    End Function

    Public Sub Remove(ByVal v_oReceiptCashListItems As ReceiptCashListItems)
        List.Remove(v_oReceiptCashListItems)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As ReceiptCashListItems
        Get
            Return List(i)
        End Get
        Set(ByVal value As ReceiptCashListItems)
            List(i) = value
        End Set
    End Property

    Public Sub Update(ByVal v_oReceiptCashListItems As ReceiptCashListItems)
        List.Item(v_oReceiptCashListItems.Key) = v_oReceiptCashListItems
    End Sub

    Public Sub Update(ByVal v_oReceiptCashListItems As ReceiptCashListItems, ByVal index As Integer)
        List.Item(index) = v_oReceiptCashListItems
    End Sub

End Class