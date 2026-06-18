<Serializable()> Public Class PaymentType
#Region "PrivateFields"

    Private iInsuranceFileRef As String

    Private iPaymentAccountID As Integer

    Private sPaymentTypeCode As String

    Private sMediaTypeCode As String

    Private sMediaReference As String

    Private sOurReference As String

    Private sTheirReference As String

    Private iCashListKey As Integer

    Private iCashListItemKey As Integer

    Private iTransDetailKey As Integer

    Private sSubbranchCode As String

#End Region


    Public Sub New()

    End Sub

    '''<remarks/>
    Public Property InsuranceFileRef() As String
        Get
            Return Me.iInsuranceFileRef
        End Get
        Set(ByVal value As String)
            Me.iInsuranceFileRef = value
        End Set
    End Property
    '''<remarks/>
    Public Property PaymentAccountID() As Integer
        Get
            Return Me.iPaymentAccountID
        End Get
        Set(ByVal value As Integer)
            Me.iPaymentAccountID = value
        End Set
    End Property
    '''<remarks/>
    Public Property PaymentTypeCode() As String
        Get
            Return Me.sPaymentTypeCode
        End Get
        Set(ByVal value As String)
            Me.sPaymentTypeCode = value
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
    Public Property CashListKey() As Integer
        Get
            Return Me.iCashListKey
        End Get
        Set(ByVal value As Integer)
            Me.iCashListKey = value
        End Set
    End Property
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
    Public Property TransDetailKey() As Integer
        Get
            Return Me.iTransDetailKey
        End Get
        Set(ByVal value As Integer)
            Me.iTransDetailKey = value
        End Set
    End Property

    Public Property SubbranchCode() As String
        Get
            Return Me.sSubbranchCode
        End Get
        Set(ByVal value As String)
            Me.sSubbranchCode = value
        End Set
    End Property
End Class
