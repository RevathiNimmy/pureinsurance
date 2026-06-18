<Serializable()<Public Class CorelistItem
#Region "PrivateFields"
    Private bIsProduceDocument As Boolean

    Private sBankReference As String

    Private sMediaTypeCode As String

    Private dtTransactionDate As Date

    Private sAccountShortCode As String

    Private dAmount As Decimal

    Private sAllocationStatusCode As String

    Private sMediaReference As String

    Private sOurReference As String

    Private sTheirReference As String

    Private sContactName As String

    Private oaddress As Address

    Private sFurtherDetails As String

#End Region

    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()

        MyBase.New()
        bIsProduceDocument = False

    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    ''' Public Overridable Function Print() As String
    Public Overridable Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("Is Produce Document: " & IIf(bIsProduceDocument, "true", "false") & "<br />")
        sbPrint.AppendLine("Bank Reference : " & sBankReference & "<br />")
        sbPrint.AppendLine("Media Type Code : " & sMediaTypeCode & "<br />")
        sbPrint.AppendLine("Transaction Date : " & dtTransactionDate & "<br />")
        sbPrint.AppendLine("Account Short Code : " & sAccountShortCode & "<br />")
        sbPrint.AppendLine("Amount : " & dAmount & "<br />")
        sbPrint.AppendLine("Allocation Status Code : " & sAllocationStatusCode & "<br />")
        sbPrint.AppendLine("Media Reference : " & sMediaReference & "<br />")
        sbPrint.AppendLine("Our Reference : " & sOurReference & "<br />")
        sbPrint.AppendLine("Their Reference : " & sTheirReference & "<br />")
        sbPrint.AppendLine("Contact Name : " & sContactName & "<br />")
        sbPrint.AppendLine("Addresses ---------------><br />")

        If oAddresses IsNot Nothing Then
            sbPrint.AppendLine(oaddress.Print())
        End If
        sbPrint.AppendLine("Further Details : " & sFurtherDetails & "<br />")
        Return sbPrint.ToString

    End Function
#Region "Properties"
    '''<remarks/>
    Public Property IsProduceDocument() As Boolean
        Get
            Return Me.bIsProduceDocument
        End Get
        Set(ByVal value As Boolean)
            Me.bIsProduceDocument = value
        End Set
    End Property

    '''<remarks/>
    Public Property BankReference() As String
        Get
            Return Me.sBankReference
        End Get
        Set(ByVal value As String)
            Me.sBankReference = value
        End Set
    End Property

    '''<remarks/>
    Public Property MediaTypeCode() As String
        Get
            Return Me.sMediaTypeCode
        End Get
        Set(ByVal value As String)
            Me.sMediaTypeCode = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
    Public Property TransactionDate() As Date
        Get
            Return Me.dtTransactionDate
        End Get
        Set(ByVal value As Date)
            Me.dtTransactionDate = value
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
    Public Property Amount() As Decimal
        Get
            Return Me.dAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dAmount = value
        End Set
    End Property

    '''<remarks/>
    Public Property AllocationStatusCode() As String
        Get
            Return Me.sAllocationStatusCode
        End Get
        Set(ByVal value As String)
            Me.sAllocationStatusCode = value
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
    Public Property OurReference() As String
        Get
            Return Me.sOurReference
        End Get
        Set(ByVal value As String)
            Me.sOurReference = value
        End Set
    End Property

    '''<remarks/>
    Public Property TheirReference() As String
        Get
            Return Me.sTheirReference
        End Get
        Set(ByVal value As String)
            Me.sTheirReference = value
        End Set
    End Property

    '''<remarks/>
    Public Property ContactName() As String
        Get
            Return Me.sContactName
        End Get
        Set(ByVal value As String)
            Me.sContactName = value
        End Set
    End Property

    '''<remarks/>
    Public Property ContactAddress() As Address
        Get
            Return Me.oaddress
        End Get
        Set(ByVal value As BaseSimpleAddressType)
            Me.oaddress = value
        End Set
    End Property

    '''<remarks/>
    Public Property FurtherDetails() As String
        Get
            Return Me.sFurtherDetails
        End Get
        Set(ByVal value As String)
            Me.sFurtherDetails = value
        End Set
    End Property
#End Region
End Class
<Serializable()> Public Class PaymentCashListItem : Inherits CorelistItem
#Region "PrivateFields"
    Private sTypeCode As String

    Private sStatusCode As String

    Private oCreditCard As CreditCardType

    Private oBankPayment As BankPayment
#End Region
    Public Sub New()

        MyBase.New()
        bIsProduceDocument = False

    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    ''' Public Overridable Function Print() As String
    Public Overridable Function Print() As String
        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Type Code : " & sTypeCode & "<br />")
        sbPrint.AppendLine("Status Code : " & sStatusCode & "<br />")
        sbPrint.AppendLine("CreditCard ---------------><br />")

        If oCreditCard IsNot Nothing Then
            sbPrint.AppendLine(oCreditCard.Print())
        End If
        sbPrint.AppendLine("BankPayment ---------------><br />")

        If oBankPayment IsNot Nothing Then
            sbPrint.AppendLine(oBankPayment.Print())
        End If

        Return sbPrint.ToString

    End Function
#Region "properties"
    '''<remarks/>
    Public Property TypeCode() As String
        Get
            Return Me.sTypeCode
        End Get
        Set(ByVal value As String)
            Me.sTypeCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property StatusCode() As String
        Get
            Return Me.sStatusCode
        End Get
        Set(ByVal value As String)
            Me.sStatusCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property CreditCard() As CreditCardType
        Get
            Return Me.oCreditCard
        End Get
        Set(ByVal value As CreditCardType)
            Me.oCreditCard = value
        End Set
    End Property

    '''<remarks/>
    Public Property Bank() As BankPayment
        Get
            Return Me.oBankPayment
        End Get
        Set(ByVal value As BankPayment)
            Me.oBankPayment = value
        End Set
    End Property
#End Region
   
End Class
<Serializable()> Public Class PaymentCashListItemCollection : Inherits CollectionBase

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oPaymentCashListItem As PaymentCashListItem In List
            sbPrint.AppendLine(oPaymentCashListItem.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function
    Public Function Add(ByVal v_oPaymentCashListItem As PaymentCashListItem) As Integer
        Return List.Add(v_oPaymentCashListItem)
    End Function

    Public Sub Remove(ByVal v_oPaymentCashListItem As PaymentCashListItem)
        List.Remove(v_oPaymentCashListItem)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As PaymentCashListItem
        Get
            Return List(i)
        End Get
        Set(ByVal value As PaymentCashListItem)
            List(i) = value
        End Set
    End Property


End Class
