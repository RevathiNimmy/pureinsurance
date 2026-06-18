<Serializable()> Public Class BankPayment
#Region "PrivateFields"
    Private sPayeeName As String

    Private sAccountCode As String

    Private sBranchCode As String

    Private dtExpiryDate As Date

    Private bExpiryDateFieldSpecified As Boolean

    Private sReference1 As String

    Private sReference2 As String
#End Region

    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()

        MyBase.New()
        bExpiryDateFieldSpecified = False

    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    ''' Public Overridable Function Print() As String
    Public Overridable Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("Payee Name : " & sPayeeName & "<br />")
        sbPrint.AppendLine("Account Code : " & sAccountCode & "<br />")
        sbPrint.AppendLine("Branch Code: " & sBranchCode & "<br />")
        sbPrint.AppendLine("Expiry Date : " & dtExpiryDate & "<br />")
        sbPrint.AppendLine("Expiry Date Field Specified: " & IIf(bExpiryDateFieldSpecified, "true", "false") & "<br />")
        sbPrint.AppendLine("Reference1 : " & sReference1 & "<br />")
        sbPrint.AppendLine("Reference2 : " & sReference2 & "<br />")
        Return sbPrint.ToString

    End Function
#Region "Properties"
    '''<remarks/>
    Public Property PayeeName() As String
        Get
            Return Me.sPayeeName
        End Get
        Set(ByVal value As String)
            Me.sPayeeName = value
        End Set
    End Property

    '''<remarks/>
    Public Property AccountCode() As String
        Get
            Return Me.sAccountCode
        End Get
        Set(ByVal value As String)
            Me.sAccountCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property BranchCode() As String
        Get
            Return Me.sBranchCode
        End Get
        Set(ByVal value As String)
            Me.sBranchCode = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
    Public Property ExpiryDate() As Date
        Get
            Return Me.dtExpiryDate
        End Get
        Set(ByVal value As Date)
            Me.dtExpiryDate = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlIgnoreAttribute()> _
    Public Property ExpiryDateSpecified() As Boolean
        Get
            Return Me.bExpiryDateFieldSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bExpiryDateFieldSpecified = value
        End Set
    End Property

    '''<remarks/>
    Public Property Reference1() As String
        Get
            Return Me.sReference1
        End Get
        Set(ByVal value As String)
            Me.sReference1 = value
        End Set
    End Property

    '''<remarks/>
    Public Property Reference2() As String
        Get
            Return Me.sReference2
        End Get
        Set(ByVal value As String)
            Me.sReference2 = value
        End Set
    End Property
#End Region
End Class
