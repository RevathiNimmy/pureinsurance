<Serializable()> Public Class AccountSearchCriteria

    Private sLedgerCode As String
    Private sAccountName As String
    Private sAccountTypeCode As String
    Private sShortCode As String
    Private sInsuranceRef As String
    Private iOperatorKey As Integer
    Private bOperatorKeySpecified As Boolean
    Private sPurchaseOrderNo As String
    Private sPurchaseInvoiceNo As String
    Private sSpare As String
    Private bShowDeleted As Boolean
    Private bShowDeletedSpecified As Boolean
    Private bShowBalance As Boolean
    Private bShowBalanceSpecified As Boolean
    Private bOnlyUpdatableAccounts As Boolean
    Private bOnlyUpdatableAccountsSpecified As Boolean
    Private bIncludeInsurerAgents As Boolean
    Private bExcludeInsurerAgents As Boolean
    Private iMaxRowsToFetch As Integer

    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()

        MyBase.New()
        bOperatorKeySpecified = False
        bShowDeleted = False
        bShowDeletedSpecified = False
        bShowBalance = False
        bShowDeletedSpecified = False
        bShowBalanceSpecified = False
        bOnlyUpdatableAccounts = False
        bOnlyUpdatableAccountsSpecified = False

    End Sub

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    ''' Public Overridable Function Print() As String
    Public Overridable Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("sLedgerCode : " & sLedgerCode & "<br />")
        sbPrint.AppendLine("sAccountName : " & sAccountName & "<br />")
        sbPrint.AppendLine("sAccountTypeCode : " & sAccountTypeCode & "<br />")
        sbPrint.AppendLine("sShortCode : " & sShortCode & "<br />")
        sbPrint.AppendLine("sInsuranceRef : " & sInsuranceRef & "<br />")
        sbPrint.AppendLine("iOperatorKey : " & iOperatorKey & "<br />")
        sbPrint.AppendLine("sPurchaseOrderNo : " & sPurchaseOrderNo & "<br />")
        sbPrint.AppendLine("sPurchaseInvoiceNo : " & sPurchaseInvoiceNo & "<br />")
        sbPrint.AppendLine("sSpare : " & sSpare & "<br />")
        sbPrint.AppendLine("bOperatorKeySpecified: " & IIf(bOperatorKeySpecified, "true", "false") & "<br />")
        sbPrint.AppendLine("bShowDeleted: " & IIf(bShowDeleted, "true", "false") & "<br />")
        sbPrint.AppendLine("bShowDeletedSpecified: " & IIf(bShowDeletedSpecified, "true", "false") & "<br />")
        sbPrint.AppendLine("bShowBalance: " & IIf(bShowBalance, "true", "false") & "<br />")
        sbPrint.AppendLine("bShowBalanceSpecified: " & IIf(bShowBalanceSpecified, "true", "false") & "<br />")
        sbPrint.AppendLine("bOnlyUpdatableAccounts: " & IIf(bOnlyUpdatableAccounts, "true", "false") & "<br />")
        sbPrint.AppendLine("bOnlyUpdatableAccountsSpecified: " & IIf(bOnlyUpdatableAccountsSpecified, "true", "false") & "<br />")
        sbPrint.AppendLine("bIncludeInsurerAgents: " & IIf(bIncludeInsurerAgents, "true", "false") & "<br />")
        sbPrint.AppendLine("bExcludeInsurerAgents: " & IIf(bExcludeInsurerAgents, "true", "false") & "<br />")
        sbPrint.AppendLine("Max Rows ToFetch: " & iMaxRowsToFetch.ToString & "<br />")

        Return sbPrint.ToString

    End Function

    '''<remarks/>
    Public Property LedgerCode() As String
        Get
            Return Me.sLedgerCode
        End Get
        Set(ByVal value As String)
            Me.sLedgerCode = value
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
    Public Property AccountTypeCode() As String
        Get
            Return Me.sAccountTypeCode
        End Get
        Set(ByVal value As String)
            Me.sAccountTypeCode = value
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
    Public Property InsuranceRef() As String
        Get
            Return Me.sInsuranceRef
        End Get
        Set(ByVal value As String)
            Me.sInsuranceRef = value
        End Set
    End Property

    '''<remarks/>
    Public Property OperatorKey() As Integer
        Get
            Return Me.iOperatorKey
        End Get
        Set(ByVal value As Integer)
            Me.iOperatorKey = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlIgnoreAttribute()> _
    Public Property OperatorKeySpecified() As Boolean
        Get
            Return Me.bOperatorKeySpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bOperatorKeySpecified = value
        End Set
    End Property

    '''<remarks/>
    Public Property PurchaseOrderNo() As String
        Get
            Return Me.sPurchaseOrderNo
        End Get
        Set(ByVal value As String)
            Me.sPurchaseOrderNo = value
        End Set
    End Property

    '''<remarks/>
    Public Property PurchaseInvoiceNo() As String
        Get
            Return Me.sPurchaseInvoiceNo
        End Get
        Set(ByVal value As String)
            Me.sPurchaseInvoiceNo = value
        End Set
    End Property

    '''<remarks/>
    Public Property Spare() As String
        Get
            Return Me.sSpare
        End Get
        Set(ByVal value As String)
            Me.sSpare = value
        End Set
    End Property

    '''<remarks/>
    Public Property ShowDeleted() As Boolean
        Get
            Return Me.bShowDeleted
        End Get
        Set(ByVal value As Boolean)
            Me.bShowDeleted = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlIgnoreAttribute()> _
    Public Property ShowDeletedSpecified() As Boolean
        Get
            Return Me.bShowDeletedSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bShowDeletedSpecified = value
        End Set
    End Property

    '''<remarks/>
    Public Property ShowBalance() As Boolean
        Get
            Return Me.bShowBalance
        End Get
        Set(ByVal value As Boolean)
            Me.bShowBalance = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlIgnoreAttribute()> _
    Public Property ShowBalanceSpecified() As Boolean
        Get
            Return Me.bShowBalanceSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bShowBalanceSpecified = value
        End Set
    End Property

    '''<remarks/>
    Public Property OnlyUpdatableAccounts() As Boolean
        Get
            Return Me.bOnlyUpdatableAccounts
        End Get
        Set(ByVal value As Boolean)
            Me.bOnlyUpdatableAccounts = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlIgnoreAttribute()> _
    Public Property OnlyUpdatableAccountsSpecified() As Boolean
        Get
            Return Me.bOnlyUpdatableAccountsSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bOnlyUpdatableAccountsSpecified = value
        End Set
    End Property

    '''<remarks/>
    Public Property IncludeInsurerAgents() As Boolean
        Get
            Return Me.bIncludeInsurerAgents
        End Get
        Set(ByVal value As Boolean)
            Me.bIncludeInsurerAgents = value
        End Set
    End Property

    '''<remarks/>
    Public Property ExcludeInsurerAgents() As Boolean
        Get
            Return Me.bExcludeInsurerAgents
        End Get
        Set(ByVal value As Boolean)
            Me.bExcludeInsurerAgents = value
        End Set
    End Property

    Public Property MaxRowsToFetch() As Integer
        Get
            Return iMaxRowsToFetch
        End Get
        Set(ByVal value As Integer)
            iMaxRowsToFetch = value
        End Set
    End Property

End Class
