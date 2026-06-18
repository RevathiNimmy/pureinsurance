''' <summary>
''' NF 1.4 get the Claims for the Seleced Client
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class Transaction

    Private iDocumentKey As Integer
    Private dtTransactionDate As DateTime
    Private dtEffectiveDate As DateTime
    Private sDocRef As String
    Private sReference As String
    Private dAmount As Double
    Private sISO As String
    Private dtPaidDate As DateTime
    Private dOSAmount As Double
    Private sReason As String
    Private dBalance As Double

    Private sAccountCode As String
    Private sUnderwritingYearCode As String
    Private sComment As String
    'Private dtTransactionDate As DateTime
    Private iPeriodID As Integer
    Private sUserName As String
    'Private sReference As String

    'Properties for GetTransactionDetails method

    Private iTransDetailKey As Integer
    Private sAltRef As String
    Private dTransDate As Date
    Private sMediaType As String
    Private dOutstandingAmount As Decimal
    Private sMediaRef As String
    Private iAccountkey As Integer
    Private bAllocationTimeStamp() As Byte
    'Newly added Properties
    Private sShortCode As String
    Private dtDocumentDate As DateTime
    Private sBranchDescription As String
    Private sDocumentType As String
    Private sBranchcode As String


    Public Sub New()
    End Sub

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder

        sbPrint.AppendLine("Document Key : " & iDocumentKey & "<br />")
        sbPrint.AppendLine("Transaction Date : " & dtTransactionDate.ToString & "<br />")
        sbPrint.AppendLine("Effective Date : " & dtEffectiveDate.ToString & "<br />")
        sbPrint.AppendLine("Doc Ref : " & sDocRef & "<br />")
        sbPrint.AppendLine("Reference : " & sReference & "<br />")
        sbPrint.AppendLine("Amount : " & dAmount.ToString & "<br />")
        sbPrint.AppendLine("ISO : " & sISO & "<br />")
        sbPrint.AppendLine("Paid Date : " & dtPaidDate.ToString & "<br />")
        sbPrint.AppendLine("Reason : " & sReason & "<br />")
        sbPrint.AppendLine("Balance : " & dBalance.ToString & "<br />")


        sbPrint.AppendLine("Account Code : " & sAccountCode.ToString() & "<br />")
        'sbPrint.AppendLine("Amount : " & dAmount.ToString() & "<br />")
        sbPrint.AppendLine("Underwriting Year Code : " & sUnderwritingYearCode.ToString() & "<br />")
        sbPrint.AppendLine("Comment : " & sComment.ToString() & "<br />")
        'sbPrint.AppendLine("Transaction Date : " & dtTransactionDate.ToString() & "<br />")
        sbPrint.AppendLine("Period ID : " & iPeriodID.ToString() & "<br />")
        sbPrint.AppendLine("User Name : " & sUserName.ToString() & "<br />")
        'sbPrint.AppendLine("sReference : " & sReference.ToString() & "<br />")

        Return sbPrint.ToString()

    End Function

    '''<remarks/>
    Public Property Branchcode() As String
        Get
            Return Me.sBranchcode
        End Get
        Set(ByVal value As String)
            Me.sBranchcode = value
        End Set
    End Property
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
    Public Property TransactionDate() As DateTime
        Get
            Return Me.dtTransactionDate
        End Get
        Set(ByVal value As DateTime)
            Me.dtTransactionDate = value
        End Set
    End Property

    '''<remarks/>
    Public Property EffectiveDate() As DateTime
        Get
            Return Me.dtEffectiveDate
        End Get
        Set(ByVal value As DateTime)
            Me.dtEffectiveDate = value
        End Set
    End Property

    '''<remarks/>
    Public Property DocRef() As String
        Get
            Return Me.sDocRef
        End Get
        Set(ByVal value As String)
            Me.sDocRef = value
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
    Public Property Amount() As Double
        Get
            Return Me.dAmount
        End Get
        Set(ByVal value As Double)
            Me.dAmount = value
        End Set
    End Property

    '''<remarks/>
    Public Property ISO() As String
        Get
            Return Me.sISO
        End Get
        Set(ByVal value As String)
            Me.sISO = value
        End Set
    End Property

    '''<remarks/>
    Public Property PaidDate() As DateTime
        Get
            Return Me.dtPaidDate
        End Get
        Set(ByVal value As DateTime)
            Me.dtPaidDate = value
        End Set
    End Property

    ''' <remarks></remarks>
    Public Property OSAmount() As Double
        Get
            Return Me.dOSAmount
        End Get
        Set(ByVal value As Double)
            Me.dOSAmount = value
        End Set
    End Property

    '''<remarks/>
    Public Property Reason() As String
        Get
            Return Me.sReason
        End Get
        Set(ByVal value As String)
            Me.sReason = value
        End Set
    End Property

    Public Property Balance() As Double
        Get
            Return Me.dBalance
        End Get
        Set(ByVal value As Double)
            Me.dBalance = value
        End Set
    End Property

    'Addded By Sanjib
    Public Property AccountCode() As String
        Get
            Return Me.sAccountCode
        End Get
        Set(ByVal value As String)
            Me.sAccountCode = value
        End Set
    End Property

    'Public Property Amount() As Decimal
    '    Get
    '        Return Me.dAmount
    '    End Get
    '    Set(ByVal value As Decimal)
    '        Me.dAmount = value
    '    End Set
    'End Property

    Public Property UnderwritingYearCode() As String
        Get
            Return Me.sUnderwritingYearCode
        End Get
        Set(ByVal value As String)
            Me.sUnderwritingYearCode = value
        End Set
    End Property

    Public Property Comment() As String
        Get
            Return Me.sComment
        End Get
        Set(ByVal value As String)
            Me.sComment = value
        End Set
    End Property

    'Public Property TransactionDate() As DateTime
    '    Get
    '        Return Me.dtTransactionDate
    '    End Get
    '    Set(ByVal value As DateTime)
    '        Me.dtTransactionDate = value
    '    End Set
    'End Property

    Public Property PeriodID() As Integer
        Get
            Return Me.iPeriodID
        End Get
        Set(ByVal value As Integer)
            Me.iPeriodID = value
        End Set
    End Property

    Public Property UserName() As String
        Get
            Return Me.sUserName
        End Get
        Set(ByVal value As String)
            Me.sUserName = value
        End Set
    End Property

    'Public Property Reference() As String
    '    Get
    '        Return Me.sReference
    '    End Get
    '    Set(ByVal value As String)
    '        Me.sReference = value
    '    End Set
    'End Property


    '''<remarks/>
    Public Property TransDetailKey() As Integer
        Get
            Return Me.iTransDetailKey
        End Get
        Set(ByVal value As Integer)
            Me.iTransDetailKey = value
        End Set
    End Property
    '''<remarks/>
    Public Property AltRef() As String
        Get
            Return Me.sAltRef
        End Get
        Set(ByVal value As String)
            Me.sAltRef = value
        End Set
    End Property
    '''<remarks/>
    Public Property TransDate() As Date
        Get
            Return Me.dTransDate
        End Get
        Set(ByVal value As Date)
            Me.dTransDate = value
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
    Public Property OutstandingAmount() As Decimal
        Get
            Return Me.dOutstandingAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dOutstandingAmount = value
        End Set
    End Property
    '''<remarks/>
    Public Property MediaRef() As String
        Get
            Return Me.sMediaRef
        End Get
        Set(ByVal value As String)
            Me.sMediaRef = value
        End Set
    End Property
    '''<remarks/>
    Public Property Accountkey() As Integer
        Get
            Return Me.iAccountkey
        End Get
        Set(ByVal value As Integer)
            Me.iAccountkey = value
        End Set
    End Property
    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute(DataType:="base64Binary")> _
    Public Property AllocationTimeStamp() As Byte()
        Get
            Return Me.bAllocationTimeStamp
        End Get
        Set(ByVal value As Byte())
            Me.bAllocationTimeStamp = value
        End Set
    End Property
    '''<remarks/>
    Public Property ShortCode() As String
        Get
            Return Me.sShortCode
        End Get
        Set(ByVal value As String)
            Me.sShortCode = value
        End Set
    End Property
    '''<remarks/>
    Public Property DocumentDate() As DateTime
        Get
            Return Me.dtDocumentDate
        End Get
        Set(ByVal value As DateTime)
            Me.dtDocumentDate = value
        End Set
    End Property
    '''<remarks/>
    Public Property BranchDescription() As String
        Get
            Return Me.sBranchDescription
        End Get
        Set(ByVal value As String)
            Me.sBranchDescription = value
        End Set
    End Property
    '''<remarks/>
    Public Property DocumentType() As String
        Get
            Return Me.sDocumentType
        End Get
        Set(ByVal value As String)
            Me.sDocumentType = value
        End Set

    End Property


End Class

<Serializable()> Public Class TransactionCollection :  Inherits SortableCollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oTransaction As Transaction In List
            sbPrint.AppendLine(oTransaction.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oTransaction As Transaction) As Integer
        Return List.Add(v_oTransaction)
    End Function

    Public Sub Remove(ByVal v_oTransaction As Transaction)
        List.Remove(v_oTransaction)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As Transaction
        Get
            Return List(i)
        End Get
        Set(ByVal value As Transaction)
            List(i) = value
        End Set
    End Property

End Class
