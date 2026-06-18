<Serializable()> Public Class BankGuarantee

    Private iKey As Integer
    Private iBankNameKey As Integer
    Private sBankNameDescription As String
    Private sBankBranch As String
    Private iPartyKey As Integer
    Private sCustodyBranchCode As String
    Private sCurrencyCode As String
    Private sBankGuaranteeReference As String
    Private dLimit As Decimal
    Private dLimitAvailable As Decimal
    Private dtIssueDate As DateTime
    Private dtExpiryDate As DateTime
    Private bPolicyLock As Boolean
    Private bDeleted As Boolean
    Private sStatusCode As String
    Private bTimeStamp() As Byte

    Private oBranches As BranchCollection
    Private oProducts As ProductCollection
    Private oBranch As String
    Private oProduct As String

    ' for FindBankGuarantee
    Private sPartyCode As String
    Private sAgentCode As String
    Private sInsuranceRef As String
    Private sBankGuaranteeRef As String
    Private sBankNameCode As String
    Private sBGStatusCode As String
    Private iBGKey As Integer
    Private sBankName As String
    Private dBGLimit As Decimal
    Private dAvailableBalance As Decimal
    Private sClientResolvedName As String
    Private sStatusDescription As String
    Private sClientShortName As String

    Private oBankGuaranteeField As BankGuaranteeCollection

    'UpdateConditionally
    Private iKeyField As Integer
    Private sRefField As String
    Private dtInvokedDateField As Date
    Private bInvokedDateFieldSpecified As Boolean
    Private bIsDeletedFieldSpecified As Boolean
    Private eActionTypeField As ActionType

    'GetPolicyBankGuarantee
    Private iInsuranceFileKey As Integer
    'Private GetBGsOf As 
    Private sClientName As String
    Private dDueDate As Date
    Private eGetBgs As GetBgs
    Private iMaxRowsToFetch As Integer

    Public Sub New()
        oBranches = New BranchCollection
        oProducts = New ProductCollection
    End Sub

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Overridable Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Type : " & Me.GetType.Name & "<br />")
        sbPrint.AppendLine("Key  : " & iKey & "<br />")
        sbPrint.AppendLine("BankNameKey  : " & iBankNameKey.ToString() & "<br />")
        sbPrint.AppendLine("Bank Name Description  : " & sBankNameDescription & "<br />")
        sbPrint.AppendLine("Bank Branch   : " & sBankBranch & "<br />")
        sbPrint.AppendLine("Party Key   : " & iPartyKey & "<br />")
        sbPrint.AppendLine("Custody Branch Code   : " & sCustodyBranchCode & "<br />")
        sbPrint.AppendLine("Currency Code   : " & sCurrencyCode & "<br />")
        sbPrint.AppendLine("Bank Guarantee Reference   : " & sBankGuaranteeReference & "<br />")
        sbPrint.AppendLine("Limit   : " & dLimit & "<br />")
        sbPrint.AppendLine("Limit Available   : " & dLimitAvailable & "<br />")
        sbPrint.AppendLine("Issue Date   : " & dtIssueDate & "<br />")
        sbPrint.AppendLine("Expiry Date   : " & dtExpiryDate & "<br />")
        sbPrint.AppendLine("Policy Lock   : " & bPolicyLock & "<br />")
        sbPrint.AppendLine("Deleted   : " & bDeleted & "<br />")
        sbPrint.AppendLine("Status Code   : " & sStatusCode & "<br />")
        sbPrint.Append("TimeStamp : ")

        If bTimeStamp IsNot Nothing Then

            For Each oByte As Byte In bTimeStamp
                sbPrint.Append(oByte.ToString & " | ")
            Next

        End If

        sbPrint.AppendLine("<br />")

        sbPrint.AppendLine("Branches ---------------><br />")

        If oBranches IsNot Nothing Then
            sbPrint.AppendLine(oBranches.Print())
        End If

        sbPrint.AppendLine("Products ---------------><br />")

        If oProducts IsNot Nothing Then
            sbPrint.AppendLine(oProducts.Print())
        End If

        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("Max Rows ToFetch: " & iMaxRowsToFetch.ToString & "<br />")

        Return sbPrint.ToString

    End Function

    Public Property DueDate() As Date
        Get
            Return dDueDate
        End Get
        Set(ByVal value As Date)
            dDueDate = value
        End Set
    End Property
    Public Property InsuranceFileKey() As Integer
        Get
            Return iInsuranceFileKey
        End Get
        Set(ByVal value As Integer)
            iInsuranceFileKey = value
        End Set
    End Property
    Public Property ClientName() As String
        Get
            Return Me.sClientName
        End Get
        Set(ByVal value As String)
            Me.sClientName = value
        End Set
    End Property
    Public Property ClientShortName() As String
        Get
            Return Me.sClientShortName
        End Get
        Set(ByVal value As String)
            Me.sClientShortName = value
        End Set
    End Property
    Public Property StatusDescription() As String
        Get
            Return Me.sStatusDescription
        End Get
        Set(ByVal value As String)
            Me.sStatusDescription = value
        End Set
    End Property
    Public Property AvailableBalance() As Decimal
        Get
            Return Me.dAvailableBalance
        End Get
        Set(ByVal value As Decimal)
            Me.dAvailableBalance = value
        End Set
    End Property
    Public Property BGLimit() As Decimal
        Get
            Return Me.dBGLimit
        End Get
        Set(ByVal value As Decimal)
            Me.dBGLimit = value
        End Set
    End Property
    Public Property BGKey() As Integer
        Get
            Return Me.iBGKey
        End Get
        Set(ByVal value As Integer)
            Me.iBGKey = value
        End Set
    End Property

    Public Property ClientResolvedName() As String
        Get
            Return Me.sClientResolvedName
        End Get
        Set(ByVal value As String)
            Me.sClientResolvedName = value
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
    Public Property PartyCode() As String
        Get
            Return Me.sPartyCode
        End Get
        Set(ByVal value As String)
            Me.sPartyCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property AgentCode() As String
        Get
            Return Me.sAgentCode
        End Get
        Set(ByVal value As String)
            Me.sAgentCode = value
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
    Public Property BankGuaranteeRef() As String
        Get
            Return Me.sBankGuaranteeRef
        End Get
        Set(ByVal value As String)
            Me.sBankGuaranteeRef = value
        End Set
    End Property

    '''<remarks/>
    Public Property BankNameCode() As String
        Get
            Return Me.sBankNameCode
        End Get
        Set(ByVal value As String)
            Me.sBankNameCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property BGStatusCode() As String
        Get
            Return Me.sBGStatusCode
        End Get
        Set(ByVal value As String)
            Me.sBGStatusCode = value
        End Set
    End Property


    ''' <summary>
    ''' BGKey
    ''' </summary>
    ''' <value>BGKey</value>
    ''' <returns>BGKey</returns>
    Public Property Key() As Integer
        Get
            Return iKey
        End Get
        Set(ByVal value As Integer)
            iKey = value
        End Set
    End Property
    ''' <summary>
    ''' BankNameKey
    ''' </summary>
    ''' <value>BankNameKey</value>
    ''' <returns>BankNameKey</returns>
    Public Property BankNameKey() As Integer
        Get
            Return iBankNameKey
        End Get
        Set(ByVal value As Integer)
            iBankNameKey = value
        End Set
    End Property
    ''' <summary>
    ''' BankName Description
    ''' </summary>
    ''' <value>BankName Description</value>
    ''' <returns>BankName Description </returns>
    Public Property BankNameDescription() As String
        Get
            Return sBankNameDescription
        End Get
        Set(ByVal value As String)
            sBankNameDescription = value
        End Set
    End Property


    ''' <summary>
    ''' BankBranch
    ''' </summary>
    ''' <value>BankBranch</value>
    ''' <returns>BankBranch</returns>
    Public Property BankBranch() As String
        Get
            Return sBankBranch
        End Get
        Set(ByVal value As String)
            sBankBranch = value
        End Set
    End Property


    ''' <summary>
    ''' Party Key
    ''' </summary>
    ''' <value>Party Key</value>
    ''' <returns>Party Key</returns>
    Public Property PartyKey() As Integer
        Get
            Return iPartyKey
        End Get
        Set(ByVal value As Integer)
            iPartyKey = value
        End Set
    End Property
    ''' <summary>
    ''' Custody Branch Code
    ''' </summary>
    ''' <value>Custody Branch Code</value>
    ''' <returns>Custody Branch Code</returns>
    Public Property CustodyBranchCode() As String
        Get
            Return sCustodyBranchCode
        End Get
        Set(ByVal value As String)
            sCustodyBranchCode = value
        End Set
    End Property




    ''' <summary>
    ''' Custody Branch Code
    ''' </summary>
    ''' <value>Custody Branch Code</value>
    ''' <returns>Custody Branch Code</returns>
    Public Property CurrencyCode() As String
        Get
            Return sCurrencyCode
        End Get
        Set(ByVal value As String)
            sCurrencyCode = value
        End Set
    End Property


    ''' <summary>
    ''' BankGuarantee Reference
    ''' </summary>
    ''' <value>BankGuarantee Reference</value>
    ''' <returns>BankGuarantee Reference</returns>
    Public Property BankGuaranteeReference() As String
        Get
            Return sBankGuaranteeReference
        End Get
        Set(ByVal value As String)
            sBankGuaranteeReference = value
        End Set
    End Property

    ''' <summary>
    ''' Limit
    ''' </summary>
    ''' <value>Limit</value>
    ''' <returns>Limit</returns>
    Public Property Limit() As Decimal
        Get
            Return dLimit
        End Get
        Set(ByVal value As Decimal)
            dLimit = value
        End Set
    End Property

    ''' <summary>
    ''' LimitAvailable
    ''' </summary>
    ''' <value>Limit Available</value>
    ''' <returns>Limit Available</returns>
    Public Property LimitAvailable() As Decimal
        Get
            Return dLimitAvailable
        End Get
        Set(ByVal value As Decimal)
            dLimitAvailable = value
        End Set
    End Property

    ''' <summary>
    ''' Issue Date
    ''' </summary>
    ''' <value>Issue Date</value>
    ''' <returns>Issue Date</returns>
    Public Property IssueDate() As DateTime
        Get
            Return dtIssueDate
        End Get
        Set(ByVal value As DateTime)
            dtIssueDate = value
        End Set
    End Property

    ''' <summary>
    ''' Expiry Date
    ''' </summary>
    ''' <value>Expiry Date</value>
    ''' <returns>Expiry Date</returns>
    Public Property ExpiryDate() As DateTime
        Get
            Return dtExpiryDate
        End Get
        Set(ByVal value As DateTime)
            dtExpiryDate = value
        End Set
    End Property

    ''' <summary>
    ''' Policy Lock
    ''' </summary>
    ''' <value>Policy Lock</value>
    ''' <returns>Policy Lock</returns>
    Public Property PolicyLock() As Boolean
        Get
            Return bPolicyLock
        End Get
        Set(ByVal value As Boolean)
            bPolicyLock = value
        End Set
    End Property

    ''' <summary>
    ''' Deleted
    ''' </summary>
    ''' <value>Deleted</value>
    ''' <returns>Deleted</returns>
    Public Property Deleted() As Boolean
        Get
            Return bDeleted
        End Get
        Set(ByVal value As Boolean)
            bDeleted = value
        End Set
    End Property

    ''' <summary>
    ''' StatusCode
    ''' </summary>
    ''' <value>StatusCode</value>
    ''' <returns>StatusCode</returns>
    Public Property StatusCode() As String
        Get
            Return sStatusCode
        End Get
        Set(ByVal value As String)
            sStatusCode = value
        End Set
    End Property

    ''' <summary>
    ''' StatusCode
    ''' </summary>
    ''' <value>StatusCode</value>
    ''' <returns>StatusCode</returns>
    Public Property TimeStamp() As Byte()
        Get
            Return bTimeStamp
        End Get
        Set(ByVal value As Byte())
            bTimeStamp = value
        End Set
    End Property

    ''' <summary>
    ''' Branches
    ''' </summary>
    ''' <value>Branches</value>
    ''' <returns>Branches</returns>
    Public Property Branches() As BranchCollection
        Get
            Return oBranches
        End Get
        Set(ByVal value As BranchCollection)
            oBranches = value
        End Set
    End Property

    ''' <summary>
    ''' Branches
    ''' </summary>
    ''' <value>Branches</value>
    ''' <returns>Branches</returns>
    Public Property Products() As ProductCollection
        Get
            Return oProducts
        End Get
        Set(ByVal value As ProductCollection)
            oProducts = value
        End Set
    End Property
    Public Property GetBg() As GetBgs
        Get
            Return eGetBgs
        End Get
        Set(ByVal value As GetBgs)
            eGetBgs = value
        End Set
    End Property
    Public Property ActionType() As ActionType
        Get
            Return eActionTypeField
        End Get
        Set(ByVal value As ActionType)
            eActionTypeField = value
        End Set
    End Property

    Public Property InvokedDate() As Date
        Get
            Return dtInvokedDateField
        End Get
        Set(ByVal value As Date)
            dtInvokedDateField = value
        End Set
    End Property

    Public Property InvokedDateFieldSpecified() As Boolean
        Get
            Return bInvokedDateFieldSpecified
        End Get
        Set(ByVal value As Boolean)
            bInvokedDateFieldSpecified = value
        End Set
    End Property

    Public Property DeletedFieldSpecified() As Boolean
        Get
            Return bIsDeletedFieldSpecified
        End Get
        Set(ByVal value As Boolean)
            bIsDeletedFieldSpecified = value
        End Set
    End Property

    Public Property Product() As String
        Get
            Return oProduct
        End Get
        Set(ByVal value As String)
            oProduct = value
        End Set
    End Property

    Public Property Branch() As String
        Get
            Return oBranch
        End Get
        Set(ByVal value As String)
            oBranch = value
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

<Serializable()> Public Class BankGuaranteeCollection : Inherits Collections.CollectionBase
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder
        sbPrint.AppendLine()
        For Each oBankGuarantee As BankGuarantee In List
            sbPrint.AppendLine(oBankGuarantee.Print())
            sbPrint.AppendLine("<Br/>")
        Next
        Return sbPrint.ToString()

    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_oBankGuarantee"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Add(ByVal v_oBankGuarantee As BankGuarantee) As Integer

        Return List.Add(v_oBankGuarantee)

    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_oBankGuarantee"></param>
    ''' <remarks></remarks>
    Public Sub Remove(ByVal v_oBankGuarantee As BankGuarantee)
        List.Remove(v_oBankGuarantee)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="index"></param>
    ''' <remarks></remarks>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="i"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Default Public Property Item(ByVal i As Integer) As BankGuarantee

        Get
            Return List(i)
        End Get
        Set(ByVal value As BankGuarantee)
            List(i) = value
        End Set

    End Property

End Class

Public Enum ActionType
    Invoked
    DelUnDel
End Enum

Public Enum GetBgs
    Agent
    Client
End Enum

