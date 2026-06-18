<Serializable()> Public Class AccountSearchResult
#Region "PrivateFields"
    Private sFullKey As String

    Private sShortCode As String

    Private sAccountName, sAccountTypeCode, sLedgerCode As String

    Private iLedgerKey As Integer

    Private iAccountTypeKey As Integer

    Private iAccountKey As Integer

    Private iNominalAccountKey As Integer

    Private sAccountStatus As String

    Private iAccountStatusKey As Integer

    Private iCompanyKey, iSourceID As Integer

    Private sContactName As String

    Private sAddressLine1 As String

    Private sPersonalClientForename, sCurrencyCode As String
    Private iPartyKey As Integer
    Private dAccountBalance As Double
    Private sSourceCode As String
    Private sIsGrossAgent As String
#End Region
    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()
       
    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    ''' Public Overridable Function Print() As String
    Public Overridable Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("FullKey : " & sFullKey & "<br />")
        sbPrint.AppendLine("ShortCode : " & sShortCode & "<br />")
        sbPrint.AppendLine("AccountName : " & sAccountName & "<br />")
        sbPrint.AppendLine("AccountTypeKey : " & iAccountTypeKey & "<br />")
        sbPrint.AppendLine("LedgerKey : " & iLedgerKey & "<br />")
        sbPrint.AppendLine("AccountKey : " & iAccountKey & "<br />")
        sbPrint.AppendLine("NominalAccountKey : " & iNominalAccountKey & "<br />")
        sbPrint.AppendLine("AccountStatus : " & sAccountStatus & "<br />")
        sbPrint.AppendLine("AccountStatusKey : " & iAccountStatusKey & "<br />")
        sbPrint.AppendLine("NominalAccountKey : " & iNominalAccountKey & "<br />")
        sbPrint.AppendLine("AccountStatus : " & sAccountStatus & "<br />")
        sbPrint.AppendLine("AccountStatusKey : " & iAccountStatusKey & "<br />")
        sbPrint.AppendLine("CompanyKey : " & iCompanyKey & "<br />")
        sbPrint.AppendLine("PartyFey : " & iPartyKey & "<br />")
        sbPrint.AppendLine("ContactName : " & sContactName & "<br />")
        sbPrint.AppendLine("AddressLine1 : " & sAddressLine1 & "<br />")
        sbPrint.AppendLine("PersonalClientForename : " & sPersonalClientForename & "<br />")
        sbPrint.AppendLine("SourceCode : " & sSourceCode & "<br />")
        sbPrint.AppendLine("IsGrossAgent : " & sIsGrossAgent & "<br />")
        Return sbPrint.ToString

    End Function
#Region "Properties"
    Public Property CurrencyCode() As String
        Get
            Return Me.sCurrencyCode
        End Get
        Set(ByVal value As String)
            Me.sCurrencyCode = value
        End Set
    End Property
    Public Property SourceID() As Integer
        Get
            Return Me.iSourceID
        End Get
        Set(ByVal value As Integer)
            Me.iSourceID = value
        End Set
    End Property
    Public Property AccountBalance() As Double
        Get
            Return Me.dAccountBalance
        End Get
        Set(ByVal value As Double)
            Me.dAccountBalance = value
        End Set
    End Property
    Public Property LedgerCode() As String
        Get
            Return Me.sLedgerCode
        End Get
        Set(ByVal value As String)
            Me.sLedgerCode = value
        End Set
    End Property
    Public Property AccountTypeCode() As String
        Get
            Return Me.sAccountTypeCode
        End Get
        Set(ByVal value As String)
            Me.sAccountTypeCode = value
        End Set
    End Property
    '''<remarks/>
    Public Property FullKey() As String
        Get
            Return Me.sFullKey
        End Get
        Set(ByVal value As String)
            Me.sFullKey = value
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
    Public Property AccountName() As String
        Get
            Return Me.sAccountName
        End Get
        Set(ByVal value As String)
            Me.sAccountName = value
        End Set
    End Property

    '''<remarks/>
    Public Property LedgerKey() As Integer
        Get
            Return Me.iLedgerKey
        End Get
        Set(ByVal value As Integer)
            Me.iLedgerKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property AccountTypeKey() As Integer
        Get
            Return Me.iAccountTypeKey
        End Get
        Set(ByVal value As Integer)
            Me.iAccountTypeKey = value
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
    Public Property NominalAccountKey() As Integer
        Get
            Return Me.iNominalAccountKey
        End Get
        Set(ByVal value As Integer)
            Me.iNominalAccountKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property AccountStatus() As String
        Get
            Return Me.sAccountStatus
        End Get
        Set(ByVal value As String)
            Me.sAccountStatus = value
        End Set
    End Property

    '''<remarks/>
    Public Property AccountStatusKey() As Integer
        Get
            Return Me.iAccountStatusKey
        End Get
        Set(ByVal value As Integer)
            Me.iAccountStatusKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property CompanyKey() As Integer
        Get
            Return Me.iCompanyKey
        End Get
        Set(ByVal value As Integer)
            Me.iCompanyKey = value
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
    Public Property AddressLine1() As String
        Get
            Return Me.sAddressLine1
        End Get
        Set(ByVal value As String)
            Me.sAddressLine1 = value
        End Set
    End Property

    '''<remarks/>
    Public Property PersonalClientForename() As String
        Get
            Return Me.sPersonalClientForename
        End Get
        Set(ByVal value As String)
            Me.sPersonalClientForename = value
        End Set
    End Property

    '''<remarks/>
    Public Property PartyKey() As Integer
        Get
            Return Me.iPartyKey
        End Get
        Set(ByVal value As Integer)
            Me.iPartyKey = value
        End Set
    End Property
    Public Property SourceCode() As String
        Get
            Return Me.sSourceCode
        End Get
        Set(ByVal value As String)
            Me.sSourceCode = value
        End Set
    End Property

    Public Property IsGrossAgent() As String
        Get
            Return Me.sIsGrossAgent
        End Get
        Set(ByVal value As String)
            Me.sIsGrossAgent = value
        End Set
    End Property
#End Region
End Class
<Serializable()> Public Class AccountSearchResultCollection : Inherits CollectionBase
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oAccountSearchResult As AccountSearchResult In List
            sbPrint.AppendLine(oAccountSearchResult.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add a BaseParty object to the collection
    ''' </summary>
    ''' <param name="v_oAccountSearchResult">The BaseParty object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oAccountSearchResult As AccountSearchResult) As Integer
        Return List.Add(v_oAccountSearchResult)
    End Function

    ''' <summary>
    ''' Remove an BaseParty object from the collection
    ''' </summary>
    ''' <param name="v_oAccountSearchResult">The BaseParty object to be removed</param>
    Public Sub Remove(ByVal v_oAccountSearchResult As AccountSearchResult)
        List.Remove(v_oAccountSearchResult)
    End Sub
    Default Public Property Item(ByVal i As Integer) As AccountSearchResult
        Get
            Return List(i)
        End Get
        Set(ByVal value As AccountSearchResult)
            List(i) = value
        End Set
    End Property
End Class
