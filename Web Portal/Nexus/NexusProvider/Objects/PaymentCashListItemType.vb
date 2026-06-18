<Serializable()> Public Class PaymentCashList : Inherits PaymentCashListItemType
    Private sTypeCode, sBankAccountCode, sCurrencyCode, sReference As String
    Private sStatusCode As String
    Private sSubbranchCode As String
    Private dListDate As Date
    Private oPaymentCashListItemType As PaymentCashListItemTypeCollection

    Public Sub New()
        oPaymentCashListItemType = New PaymentCashListItemTypeCollection
    End Sub
    Public Property PaymentCashListItemType() As PaymentCashListItemTypeCollection
        Get
            Return oPaymentCashListItemType
        End Get
        Set(ByVal value As PaymentCashListItemTypeCollection)
            oPaymentCashListItemType = value
        End Set
    End Property
    Public Property ListDate() As Date
        Get
            Return dListDate
        End Get
        Set(ByVal value As Date)
            dListDate = value
        End Set
    End Property
    Public Property Reference() As String
        Get
            Return sReference
        End Get
        Set(ByVal value As String)
            sReference = value
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
    Public Property BankAccountCode() As String
        Get
            Return sBankAccountCode
        End Get
        Set(ByVal value As String)
            sBankAccountCode = value
        End Set
    End Property
    Public Property TypeCode() As String
        Get
            Return sTypeCode
        End Get
        Set(ByVal value As String)
            sTypeCode = value
        End Set
    End Property
    Public Property StatusCode() As String
        Get
            Return sStatusCode
        End Get
        Set(ByVal value As String)
            sStatusCode = value
        End Set
    End Property
    Public Property SubbranchCode() As String
        Get
            Return sSubbranchCode
        End Get
        Set(ByVal value As String)
            sSubbranchCode = value
        End Set
    End Property
End Class

<Serializable()> Public Class PaymentCashListItemType : Inherits CoreCashListItemType
    Private sTypeCode, sUserName As String
    Private sStatusCode As String
    Private iCashListKey As Integer
    Private bLetter As Boolean

    Private sComments As String
    Private bTimeStamp As Byte()
    Private iCashListItemKey As Integer
    Private iTransDetailKey As Integer
    Private iTransDetailsKey As Integer
    Private bDeclined As Boolean
    Private oCoreCashList As CoreCashListType
    Private oCreditCard As CreditCard
    Private oBankPaymentType As BankPaymentType
    Private oBank As Bank
    Private eAllocationType As AllocationType
    Private oPaymentItems As PaymentItemsCollection
    Private oPaymentCashList As PaymentCashListCollection
    Private oInstalmentPlanDetails As InstalmentPlanDetails
    Private bAutoAllocateIfAble As Boolean
    Private bAutoAllocatePaymentSuccessful As Boolean
    Private bCheckValidationOnly As Boolean



    Public Sub New()
        oBank = New Bank
        oCreditCard = New CreditCard
        oBankPaymentType = New BankPaymentType
        oCoreCashList = New CoreCashListType
        oPaymentItems = New PaymentItemsCollection
        oPaymentCashList = New PaymentCashListCollection
    End Sub

    Public Overridable Function Print() As String

        Dim sbPrint As New Text.StringBuilder()
        Return sbPrint.ToString

    End Function
    Public Property UserName() As String
        Get
            Return sUserName
        End Get
        Set(ByVal value As String)
            sUserName = value
        End Set
    End Property
    Public Property Comments() As String
        Get
            Return sComments
        End Get
        Set(ByVal value As String)
            sComments = value
        End Set
    End Property
    Public Property TimeStamp() As Byte()
        Get
            Return bTimeStamp
        End Get
        Set(ByVal value As Byte())
            bTimeStamp = value
        End Set
    End Property
    Public Property Declined() As Boolean
        Get
            Return bDeclined
        End Get
        Set(ByVal value As Boolean)
            bDeclined = value
        End Set
    End Property
    Public Property CashListKey() As Integer
        Get
            Return iCashListKey
        End Get
        Set(ByVal value As Integer)
            iCashListKey = value
        End Set
    End Property
    Public Property Letter() As Boolean
        Get
            Return bLetter
        End Get
        Set(ByVal value As Boolean)
            bLetter = value
        End Set
    End Property
    Public Property TypeCode() As String
        Get
            Return sTypeCode
        End Get
        Set(ByVal value As String)
            sTypeCode = value
        End Set
    End Property
    Public Property StatusCode() As String
        Get
            Return sStatusCode
        End Get
        Set(ByVal value As String)
            sStatusCode = value
        End Set
    End Property
    Public Property CashListItemKey() As Integer
        Get
            Return Me.iCashListItemKey
        End Get
        Set(ByVal value As Integer)
            Me.iCashListItemKey = value
        End Set
    End Property
    Public Property TransDetailsKey() As Integer
        Get
            Return Me.iTransDetailsKey
        End Get
        Set(ByVal value As Integer)
            Me.iTransDetailsKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property TransDetailKey() As Integer
        Get
            Return Me.iTransDetailKey
        End Get
        Set(ByVal value As Integer)
            Me.iTransDetailKey = value
        End Set
    End Property
    Public Property Bank() As Bank
        Get
            Return oBank
        End Get
        Set(ByVal value As Bank)
            oBank = value
        End Set
    End Property
    Public Property CoreCashList() As CoreCashListType
        Get
            Return oCoreCashList
        End Get
        Set(ByVal value As CoreCashListType)
            oCoreCashList = value
        End Set
    End Property
    Public Property CreditCard() As CreditCard
        Get
            Return oCreditCard
        End Get
        Set(ByVal value As CreditCard)
            oCreditCard = value
        End Set
    End Property
    Public Property BankPaymentType() As BankPaymentType
        Get
            Return oBankPaymentType
        End Get
        Set(ByVal value As BankPaymentType)
            oBankPaymentType = value
        End Set
    End Property
    Public Property AllocationTypes() As AllocationType
        Get
            Return eAllocationType
        End Get
        Set(ByVal value As AllocationType)
            eAllocationType = value
        End Set
    End Property
    Public Property PaymentItems() As PaymentItemsCollection
        Get
            Return oPaymentItems
        End Get
        Set(ByVal value As PaymentItemsCollection)
            oPaymentItems = value
        End Set
    End Property
    Public Property PaymentCashList() As PaymentCashListCollection
        Get
            Return oPaymentCashList
        End Get
        Set(ByVal value As PaymentCashListCollection)
            oPaymentCashList = value
        End Set
    End Property

    Public Property InstalmentPlanDetails() As InstalmentPlanDetails
        Get
            Return oInstalmentPlanDetails
        End Get
        Set(ByVal value As InstalmentPlanDetails)
            oInstalmentPlanDetails = value
        End Set
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AutoAllocateIfAble() As Boolean
        Get
            Return bAutoAllocateIfAble
        End Get
        Set(ByVal value As Boolean)
            bAutoAllocateIfAble = value
        End Set
    End Property

    Public Property AutoAllocatePaymentSuccessful() As Boolean
        Get
            Return bAutoAllocatePaymentSuccessful
        End Get
        Set(ByVal value As Boolean)
            bAutoAllocatePaymentSuccessful = value
        End Set
    End Property
    Public Property CheckValidationOnly() As Boolean
        Get
            Return bCheckValidationOnly
        End Get
        Set(value As Boolean)
            bCheckValidationOnly = value
        End Set
    End Property
    Public Property DocumentRef As String

    Public Property DocumentCode As String

    Public Enum AllocationType
        '''<remarks/>
        BankGuarantee
    End Enum


End Class

<Serializable()> Public Class PaymentCashListItemTypeCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oPaymentCashListItemType As PaymentCashListItemType In List
            sbPrint.AppendLine(oPaymentCashListItemType.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oPaymentCashListItemType As PaymentCashListItemType) As Integer
        Return List.Add(v_oPaymentCashListItemType)
    End Function

    Public Sub Remove(ByVal v_oPaymentCashListItemType As PaymentCashListItemType)
        List.Remove(v_oPaymentCashListItemType)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As PaymentCashListItemType
        Get
            Return List(i)
        End Get
        Set(ByVal value As PaymentCashListItemType)
            List(i) = value
        End Set
    End Property

End Class
<Serializable()> Public Class PaymentCashListCollection : Inherits CollectionBase
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oPaymentCashList As PaymentCashList In List
            sbPrint.AppendLine(oPaymentCashList.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oPaymentCashList As PaymentCashList) As Integer
        Return List.Add(v_oPaymentCashList)
    End Function

    Public Sub Remove(ByVal v_oPaymentCashList As PaymentCashList)
        List.Remove(v_oPaymentCashList)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As PaymentCashList
        Get
            Return List(i)
        End Get
        Set(ByVal value As PaymentCashList)
            List(i) = value
        End Set
    End Property

End Class