<Serializable()> Public Class UnallocatedClaimPayments

    Private iDocumentKey As Integer
    Private sDocumentRef As String
    Private dCurrencyAmount As Double
    Private iCurrencyKey As Integer
    Private dAmount As Double
    Private iAmountCurrencyKey As Integer
    Private dAccountAmount As Double
    Private iAccountCurrencyKey As Integer
    Private sClaimNumber As String
    Private sDocumentComment As String
    Private sCurrencyDescription As String
    Private sCurrencyFormatString As String
    Private dDateOfPayment As Date
    Private iPayeeMediaTypeKey As Integer
    Private sBaseCurrencyDescription As String
    Private sBaseCurrencyFormatString As String
    Private iMaxClaimPaymentKey As Integer
    Private dDocumentDate As Date
    Private iAccountKey As Integer
    Private iBaseClaimPaymentKey As Integer
    Private sAccountName As String
    Private nPartyBankId As Integer
    Private sPayeeAccountNo As String
    Private sPayeeShortCode As String

    '''<remarks/>
    Public Property DocumentKey() As Integer
        Get
            Return Me.iDocumentKey
        End Get
        Set(ByVal value As Integer)
            Me.iDocumentKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property DocumentRef() As String
        Get
            Return Me.sDocumentRef
        End Get
        Set(ByVal value As String)
            Me.sDocumentRef = value
        End Set
    End Property

    '''<remarks/>
    Public Property CurrencyAmount() As Double
        Get
            Return Me.dCurrencyAmount
        End Get
        Set(ByVal value As Double)
            Me.dCurrencyAmount = value
        End Set
    End Property

    '''<remarks/>
    Public Property CurrencyKey() As Integer
        Get
            Return Me.iCurrencyKey
        End Get
        Set(ByVal value As Integer)
            Me.iCurrencyKey = value
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
    Public Property AmountCurrencyKey() As Integer
        Get
            Return Me.iAmountCurrencyKey
        End Get
        Set(ByVal value As Integer)
            Me.iAmountCurrencyKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property AccountAmount() As Double
        Get
            Return Me.dAccountAmount
        End Get
        Set(ByVal value As Double)
            Me.dAccountAmount = value
        End Set
    End Property

    '''<remarks/>
    Public Property AccountCurrencyKey() As Integer
        Get
            Return Me.iAccountCurrencyKey
        End Get
        Set(ByVal value As Integer)
            Me.iAccountCurrencyKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClaimNumber() As String
        Get
            Return Me.sClaimNumber
        End Get
        Set(ByVal value As String)
            Me.sClaimNumber = value
        End Set
    End Property

    '''<remarks/>
    Public Property DocumentComment() As String
        Get
            Return Me.sDocumentComment
        End Get
        Set(ByVal value As String)
            Me.sDocumentComment = value
        End Set
    End Property

    '''<remarks/>
    Public Property CurrencyDescription() As String
        Get
            Return Me.sCurrencyDescription
        End Get
        Set(ByVal value As String)
            Me.sCurrencyDescription = value
        End Set
    End Property

    '''<remarks/>
    Public Property CurrencyFormatString() As String
        Get
            Return Me.sCurrencyFormatString
        End Get
        Set(ByVal value As String)
            Me.sCurrencyFormatString = value
        End Set
    End Property

    '''<remarks/>
    Public Property DateOfPayment() As Date
        Get
            Return Me.dDateOfPayment
        End Get
        Set(ByVal value As Date)
            Me.dDateOfPayment = value
        End Set
    End Property

    '''<remarks/>
    Public Property PayeeMediaTypeKey() As Integer
        Get
            Return Me.iPayeeMediaTypeKey
        End Get
        Set(ByVal value As Integer)
            Me.iPayeeMediaTypeKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property BaseCurrencyDescription() As String
        Get
            Return Me.sBaseCurrencyDescription
        End Get
        Set(ByVal value As String)
            Me.sBaseCurrencyDescription = value
        End Set
    End Property

    '''<remarks/>
    Public Property BaseCurrencyFormatString() As String
        Get
            Return Me.sBaseCurrencyFormatString
        End Get
        Set(ByVal value As String)
            Me.sBaseCurrencyFormatString = value
        End Set
    End Property

    '''<remarks/>
    Public Property MaxClaimPaymentKey() As Integer
        Get
            Return Me.iMaxClaimPaymentKey
        End Get
        Set(ByVal value As Integer)
            Me.iMaxClaimPaymentKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property DocumentDate() As Date
        Get
            Return Me.dDocumentDate
        End Get
        Set(ByVal value As Date)
            Me.dDocumentDate = value
        End Set
    End Property

    '''<remarks/>
    Public Property AccountKey() As Integer
        Get
            Return Me.iAccountKey
        End Get
        Set(ByVal value As Integer)
            Me.iAccountKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property BaseClaimPaymentKey() As Integer
        Get
            Return Me.iBaseClaimPaymentKey
        End Get
        Set(ByVal value As Integer)
            Me.iBaseClaimPaymentKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property AccountName() As String
        Get
            Return Me.sAccountName
        End Get
        Set(ByVal value As String)
            Me.sAccountName = value
        End Set
    End Property
    '''<remarks/>
    Public Property PartyBankId() As Integer
        Get
            Return Me.nPartyBankId
        End Get
        Set(ByVal value As Integer)
            Me.nPartyBankId = value
        End Set
    End Property
    '''<remarks/>
    Public Property PayeeAccountNo() As String
        Get
            Return Me.sPayeeAccountNo
        End Get
        Set(ByVal value As String)
            Me.sPayeeAccountNo = value
        End Set
    End Property
    '''<remarks/>
    Public Property PayeeShortCode() As String
        Get
            Return Me.sPayeeShortCode
        End Get
        Set(ByVal value As String)
            Me.sPayeeShortCode = value
        End Set
    End Property

    ''' <summary>
    ''' to store Status
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Status As String = String.Empty
    ''' <summary>
    ''' to store MediaTypeCode
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MediaTypeCode As String = String.Empty
    ''' <summary>
    ''' to store CurrencyCode
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CurrencyCode As String = String.Empty
    ''' <summary>
    ''' to store BankAccountCode
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BankAccountCode As String = String.Empty
    ''' <summary>
    ''' to store CashListItemKey
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CashListItemKey As Integer = 0
    ''' <summary>
    ''' to store AccountCode
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AccountCode As String = String.Empty
    ''' <summary>
    ''' to store ClaimPaymentBranchCode
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ClaimPaymentBranchCode As String = String.Empty
    ''' <summary>
    ''' to store PayeeName
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PayeeName As String = String.Empty
    ''' <summary>
    ''' to store OurRef
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OurRef As String = String.Empty
    ''' <summary>
    ''' to store ClaimPaymentKey
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ClaimPaymentKey As Integer = 0
    ''' <summary>
    ''' to store IsSelected
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsSelected As Boolean = False
    ''' <summary>
    ''' to store MediaTypeDesc
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MediaTypeDesc As String = String.Empty
    ''' <summary>
    ''' hold Third Party reference
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TheirRef As String = String.Empty

    Public Property AmountCurrencyCode As String = String.Empty
    ''' <summary>
    ''' To store Media Reference
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MediaReference As String = String.Empty

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("<br />")

        Return sbPrint.ToString()

    End Function


End Class

''' <summary>
''' Collection Class to hold UnallocatedCredit objects.
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class UnallocatedClaimPaymentsCollection : Inherits SortableCollectionBase

    Public Sub New()
        MyBase.SortObjectType = GetType(UnallocatedClaimPayments)
    End Sub

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oUnallocatedClaimPayments As UnallocatedClaimPayments In List
            sbPrint.AppendLine(oUnallocatedClaimPayments.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oUnallocatedClaimPayments As UnallocatedClaimPayments) As Integer
        Return List.Add(v_oUnallocatedClaimPayments)
    End Function

    Public Sub Remove(ByVal v_oUnallocatedClaimPayments As UnallocatedClaimPayments)
        List.Remove(v_oUnallocatedClaimPayments)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As UnallocatedClaimPayments
        Get
            Return List(i)
        End Get
        Set(ByVal value As UnallocatedClaimPayments)
            List(i) = value
        End Set
    End Property

End Class
