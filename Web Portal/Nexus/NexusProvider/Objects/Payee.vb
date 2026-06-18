<Serializable()> Public Class Payee
    Private sName As String
    Private sBankName As String
    Private sBankNumber As String
    Private sBankCode As String
    Private sMediaTypeCode As String
    Private sMediaReference As String
    Private sTheirReference As String
    Private sComments As String
    Private oAddress As Address
    Private iPartyBankKey As Integer
    Private sBIC As String
    Private sIBAN As String

    Public Property PartyBankKey() As Integer
        Get
            Return Me.iPartyBankKey
        End Get
        Set(ByVal value As Integer)
            Me.iPartyBankKey = value
        End Set
    End Property

    Public Property Name() As String
        Get
            Return Me.sName
        End Get
        Set(ByVal value As String)
            Me.sName = value
        End Set
    End Property

    Public Property BankName() As String
        Get
            Return Me.sBankName
        End Get
        Set(ByVal value As String)
            Me.sBankName = value
        End Set
    End Property

    Public Property BankNumber() As String
        Get
            Return Me.sBankNumber
        End Get
        Set(ByVal value As String)
            Me.sBankNumber = value
        End Set
    End Property

    Public Property BankCode() As String
        Get
            Return Me.sBankCode
        End Get
        Set(ByVal value As String)
            Me.sBankCode = value
        End Set
    End Property

    Public Property MediaTypeCode() As String
        Get
            Return Me.sMediaTypeCode
        End Get
        Set(ByVal value As String)
            Me.sMediaTypeCode = value
        End Set
    End Property

    Public Property MediaReference() As String
        Get
            Return Me.sMediaReference
        End Get
        Set(ByVal value As String)
            Me.sMediaReference = value
        End Set
    End Property

    Public Property TheirReference() As String
        Get
            Return Me.sTheirReference
        End Get
        Set(ByVal value As String)
            Me.sTheirReference = value
        End Set
    End Property

    Public Property Comments() As String
        Get
            Return Me.sComments
        End Get
        Set(ByVal value As String)
            Me.sComments = value
        End Set
    End Property

    Public Property Address() As Address
        Get
            Return Me.oAddress
        End Get
        Set(ByVal value As Address)
            Me.oAddress = value
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

    Public Property MediaTypeDesc() As String

    Public Property AccountType() As String
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Name : " & sName & "<br />")
        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("BankName : " & sBankName & "<br />")
        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("BankCode : " & sBankCode & "<br />")
        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("MediaTypeCode : " & sMediaTypeCode & "<br />")
        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("MediaReference : " & sMediaReference & "<br />")
        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("TheirReference : " & sTheirReference & "<br />")
        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("Comments : " & sComments & "<br />")
        sbPrint.AppendLine("<br />")
        Return sbPrint.ToString()

    End Function

    Public Sub New()
        oAddress = New Address()
    End Sub
End Class
