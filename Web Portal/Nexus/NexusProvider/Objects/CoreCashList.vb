<Serializable()> Public Class CoreCashList
#Region "Private Fields"
    Private sTypeCode As String

    Private dtListDate As Date

    Private sBankAccountCode As String

    Private sCurrencyCode As String

    Private sReference As String

    Private sStatusCode As String
#End Region
    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()

        MyBase.New()

    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    ''' Public Overridable Function Print() As String
    Public Overridable Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("Type Code : " & sTypeCode & "<br />")
        sbPrint.AppendLine("List Date : " & dtListDate & "<br />")
        sbPrint.AppendLine("Bank Account Code : " & sBankAccountCode & "<br />")
        sbPrint.AppendLine("Currency Code : " & sCurrencyCode & "<br />")
        sbPrint.AppendLine("Reference : " & sReference & "<br />")
        sbPrint.AppendLine("Status Code : " & sStatusCode & "<br />")

        Return sbPrint.ToString

    End Function
#Region "Properties"
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
    <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
    Public Property ListDate() As Date
        Get
            Return Me.dtListDate
        End Get
        Set(ByVal value As Date)
            Me.dtListDate = value
        End Set
    End Property

    '''<remarks/>
    Public Property BankAccountCode() As String
        Get
            Return Me.sBankAccountCode
        End Get
        Set(ByVal value As String)
            Me.sBankAccountCode = value
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
    Public Property Reference() As String
        Get
            Return Me.sReference
        End Get
        Set(ByVal value As String)
            Me.sReference = value
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
#End Region
End Class
<Serializable()> Public Class PaymentCashList : Inherits CoreCashList
    Private oPaymentCashListItemCollection As PaymentCashListItemCollection
    Public Property PaymentItem() As PaymentCashListItemCollection
        Get
            Return Me.oPaymentCashListItemCollection
        End Get
        Set(ByVal value As PaymentCashListItemCollection)
            Me.oPaymentCashListItemCollection = value
        End Set
    End Property
End Class
