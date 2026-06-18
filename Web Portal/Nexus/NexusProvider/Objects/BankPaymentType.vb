
<Serializable()> Public Class BankPaymentType


    Private sPayeeName As String
    Private sAccountCode As String
    Private sBranchCode As String
    Private dExpiryDate As Date
    Private sReference1 As String
    Private sReference2 As String
    Private sBIC As String
    Private sIBAN As String

    Public Property PayeeName() As String
        Get
            Return Me.sPayeeName
        End Get
        Set(ByVal value As String)
            Me.sPayeeName = value
        End Set
    End Property

    Public Property AccountCode() As String
        Get
            Return Me.sAccountCode
        End Get
        Set(ByVal value As String)
            Me.sAccountCode = value
        End Set
    End Property

    Public Property BranchCode() As String
        Get
            Return Me.sBranchCode
        End Get
        Set(ByVal value As String)
            Me.sBranchCode = value
        End Set
    End Property

    Public Property ExpiryDate() As Date
        Get
            Return Me.dExpiryDate
        End Get
        Set(ByVal value As Date)
            Me.dExpiryDate = value
        End Set
    End Property

    Public Property Reference1() As String
        Get
            Return Me.sReference1
        End Get
        Set(ByVal value As String)
            Me.sReference1 = value
        End Set
    End Property

    Public Property Reference2() As String
        Get
            Return Me.sReference2
        End Get
        Set(ByVal value As String)
            Me.sReference2 = value
        End Set
    End Property

    ''' <summary>
    ''' Business Identifier Codes(BIC) used in Party,Instalments,Claim,Cash/Cheque Payment and Receipt
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BIC() As String
        Get
            Return Me.sBIC
        End Get
        Set(ByVal value As String)
            Me.sBIC = value
        End Set
    End Property

    ''' <summary>
    ''' International Bank Account Number(IBAN) used in Party,Instalments,Claim,Cash/Cheque Payment and Receipt
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IBAN() As String
        Get
            Return Me.sIBAN
        End Get
        Set(ByVal value As String)
            Me.sIBAN = value
        End Set
    End Property

End Class
