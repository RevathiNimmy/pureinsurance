<Serializable()> Public Class CashDeposit

    Private bIsSinglePolicy As Boolean
    Private bCDTimeStamp() As Byte
    Private iAccountKey, iCashDepositKey, iIsDeleted, iPartyKey, iUserId, iNextCashDepositNumber As Integer
    Private oBranchColl As NexusProvider.BranchCollection
    Private oProductColl As NexusProvider.ProductCollection
    Private sCashDepositRef, sPartyCode, sUserName, sBankName, sBranchName, sProductName, sCurrencyCode As String
    Private sClientType As ClientAgentType

    Private dAmount, dAvailableBalance As Decimal
    Private dDateCreated As DateTime
    Private bDateCreatedSpecified, bUserIdSpecified As Boolean
    Private sPartyName As String
    Public Sub New()
        oBranchColl = New NexusProvider.BranchCollection
        oProductColl = New NexusProvider.ProductCollection
    End Sub

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder

        sbPrint.AppendLine("Account Key : " & iAccountKey.ToString & "<br />")
        sbPrint.AppendLine("Cash Deposit Key : " & iCashDepositKey.ToString & "<br />")
        sbPrint.AppendLine("IsDeleted : " & iIsDeleted.ToString & "<br />")
        sbPrint.AppendLine("Party Key : " & iPartyKey.ToString & "<br />")
        sbPrint.AppendLine("bIsSinglePolicy : " & bIsSinglePolicy.ToString & "<br />")

        Return sbPrint.ToString()

    End Function
    Public Property NextCashDepositNumber() As Integer
        Get
            Return iNextCashDepositNumber
        End Get
        Set(ByVal value As Integer)
            iNextCashDepositNumber = value
        End Set
    End Property
    Public Property UserIdSpecified() As Boolean
        Get
            Return bUserIdSpecified
        End Get
        Set(ByVal value As Boolean)
            bUserIdSpecified = value
        End Set
    End Property
    Public Property UserId() As Integer
        Get
            Return iUserId
        End Get
        Set(ByVal value As Integer)
            iUserId = value
        End Set
    End Property
    Public Property PartyName() As String
        Get
            Return sPartyName
        End Get
        Set(ByVal value As String)
            sPartyName = value
        End Set
    End Property
    Public Property DateCreatedSpecified() As Boolean
        Get
            Return bDateCreatedSpecified
        End Get
        Set(ByVal value As Boolean)
            bDateCreatedSpecified = value
        End Set
    End Property
    Public Property DateCreated() As DateTime
        Get
            Return dDateCreated
        End Get
        Set(ByVal value As DateTime)
            dDateCreated = value
        End Set
    End Property
    Public Property ProductName() As String
        Get
            Return sProductName
        End Get
        Set(ByVal value As String)
            sProductName = value
        End Set
    End Property
    Public Property BranchName() As String
        Get
            Return sBranchName
        End Get
        Set(ByVal value As String)
            sBranchName = value
        End Set
    End Property
    Public Property BankName() As String
        Get
            Return sBankName
        End Get
        Set(ByVal value As String)
            sBankName = value
        End Set
    End Property
    Public Property AvailableBalance() As Decimal
        Get
            Return dAvailableBalance
        End Get
        Set(ByVal value As Decimal)
            dAvailableBalance = value
        End Set
    End Property
    Public Property Amount() As Decimal
        Get
            Return dAmount
        End Get
        Set(ByVal value As Decimal)
            dAmount = value
        End Set
    End Property
    Public Property AccountKey() As Integer
        Get
            Return iAccountKey
        End Get
        Set(ByVal value As Integer)
            iAccountKey = value
        End Set
    End Property
    Public Property CashDepositKey() As Integer
        Get
            Return iCashDepositKey
        End Get
        Set(ByVal value As Integer)
            iCashDepositKey = value
        End Set
    End Property
    Public Property IsDeleted() As Integer
        Get
            Return iIsDeleted
        End Get
        Set(ByVal value As Integer)
            iIsDeleted = value
        End Set
    End Property
    Public Property PartyKey() As Integer
        Get
            Return iPartyKey
        End Get
        Set(ByVal value As Integer)
            iPartyKey = value
        End Set
    End Property
    Public Property CDTimeStamp() As Byte()
        Get
            Return bCDTimeStamp
        End Get
        Set(ByVal value As Byte())
            bCDTimeStamp = value
        End Set
    End Property
    Public Property IsSinglePolicy() As Boolean
        Get
            Return bIsSinglePolicy
        End Get
        Set(ByVal value As Boolean)
            bIsSinglePolicy = value
        End Set
    End Property
    Public Property Branches() As NexusProvider.BranchCollection
        Get
            Return oBranchColl
        End Get
        Set(ByVal value As NexusProvider.BranchCollection)
            oBranchColl = value
        End Set
    End Property
    Public Property Products() As NexusProvider.ProductCollection
        Get
            Return oProductColl
        End Get
        Set(ByVal value As NexusProvider.ProductCollection)
            oProductColl = value
        End Set
    End Property
    Public Property CashDepositRef() As String
        Get
            Return sCashDepositRef
        End Get
        Set(ByVal value As String)
            sCashDepositRef = value
        End Set
    End Property
    Public Property PartyCode() As String
        Get
            Return sPartyCode
        End Get
        Set(ByVal value As String)
            sPartyCode = value
        End Set
    End Property
    Public Property PartyType() As ClientAgentType
        Get
            Return sClientType
        End Get
        Set(ByVal value As ClientAgentType)
            sClientType = value
        End Set
    End Property
    Public Property UserName() As String
        Get
            Return sUserName
        End Get
        Set(ByVal value As String)
            sUserName = value
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
End Class

<Serializable()> Public Class CashDepositCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oCashDeposit As CashDeposit In List
            sbPrint.AppendLine(oCashDeposit.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oCashDeposit As CashDeposit) As Integer
        Return List.Add(v_oCashDeposit)
    End Function

    Public Sub Remove(ByVal v_oCashDeposit As CashDeposit)
        List.Remove(v_oCashDeposit)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As CashDeposit
        Get
            Return List(i)
        End Get
        Set(ByVal value As CashDeposit)
            List(i) = value
        End Set
    End Property

End Class