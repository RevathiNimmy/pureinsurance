<Serializable()> Public Class BankGuaranteePolicy
    Private sPartyCode As String
    Private sResolvedName As String

    Private oPartyBGPolicyDetails As PartyBGPolicyDetailsCollection

    Public Sub New()

        oPartyBGPolicyDetails = New PartyBGPolicyDetailsCollection

    End Sub
    Public Property PartyCode() As String
        Get
            Return sPartyCode
        End Get
        Set(ByVal value As String)
            sPartyCode = value
        End Set
    End Property
    Public Property ResolvedName() As String
        Get
            Return sResolvedName
        End Get
        Set(ByVal value As String)
            sResolvedName = value
        End Set
    End Property
    Public Property PartyBGPolicyDetails() As PartyBGPolicyDetailsCollection
        Get
            Return oPartyBGPolicyDetails
        End Get
        Set(ByVal value As PartyBGPolicyDetailsCollection)
            oPartyBGPolicyDetails = value
        End Set
    End Property


End Class
<Serializable()> Public Class PartyBGPolicyDetails

    'these are for GetPoliciesOnBankGuaranteeForReceipt method

    Private iInsuranceFileKey As Integer
    Private sClientCode As String
    Private sClientName As String
    Private sPolicyRef As String
    Private sAgentCode As String
    Private sBranchDesc As String
    Private sProductDesc As String
    Private dPremiumAmount As Decimal
    Private dtCoverStartDate As DateTime
    Private dtCoverEndDate As DateTime

    'these are for GetPoliciesOnBankGuaranteeForReceipt method



    Private iBGKey As Integer
    Private iBankNameKey As Integer
    Private sBankName As String
    Private sBankGuaranteeRef As String
    Private dtBGDueDate As DateTime
    Private iPolicyKey As Integer
    Private sBranchCode As String
    Private sProductCode As String
    Private dOutstandingPolicyAmt As Decimal

    'these are for GetPoliciesOnBankGuaranteeForReceipt method

    Private dBGLimit As Decimal
    Private dAvailableBalance As Decimal
    Private dtExpiryDate As DateTime
    Private sClientShortName As String
    Private dtDueDate As DateTime

    Public Overridable Function Print() As String
        Dim sbPrint As New Text.StringBuilder()

        Return sbPrint.ToString
    End Function



    Public Property BGKey() As Integer
        Get
            Return iBGKey
        End Get
        Set(ByVal value As Integer)
            iBGKey = value
        End Set
    End Property
    Public Property BankNameKey() As Integer
        Get
            Return iBankNameKey
        End Get
        Set(ByVal value As Integer)
            iBankNameKey = value
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
    Public Property BankGuaranteeRef() As String
        Get
            Return sBankGuaranteeRef
        End Get
        Set(ByVal value As String)
            sBankGuaranteeRef = value
        End Set
    End Property

    Public Property BGDueDate() As DateTime
        Get
            Return dtBGDueDate
        End Get
        Set(ByVal value As DateTime)
            dtBGDueDate = value
        End Set
    End Property

    Public Property PolicyKey() As Integer
        Get
            Return iPolicyKey
        End Get
        Set(ByVal value As Integer)
            iPolicyKey = value
        End Set
    End Property

    Public Property BranchCode() As String
        Get
            Return sBranchCode
        End Get
        Set(ByVal value As String)
            sBranchCode = value
        End Set
    End Property

    Public Property ProductCode() As String
        Get
            Return sProductCode
        End Get
        Set(ByVal value As String)
            sProductCode = value
        End Set
    End Property

    Public Property OutstandingPolicyAmt() As Decimal
        Get
            Return dOutstandingPolicyAmt
        End Get
        Set(ByVal value As Decimal)
            dOutstandingPolicyAmt = value
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

    Public Property ClientCode() As String
        Get
            Return sClientCode
        End Get
        Set(ByVal value As String)
            sClientCode = value
        End Set
    End Property

    Public Property ClientName() As String
        Get
            Return sClientName
        End Get
        Set(ByVal value As String)
            sClientName = value
        End Set
    End Property

    Public Property PolicyRef() As String
        Get
            Return sPolicyRef
        End Get
        Set(ByVal value As String)
            sPolicyRef = value
        End Set
    End Property

    Public Property AgentCode() As String
        Get
            Return sAgentCode
        End Get
        Set(ByVal value As String)
            sAgentCode = value
        End Set
    End Property
    Public Property BranchDesc() As String
        Get
            Return sBranchDesc
        End Get
        Set(ByVal value As String)
            sBranchDesc = value
        End Set
    End Property
    Public Property ProductDesc() As String
        Get
            Return sProductDesc
        End Get
        Set(ByVal value As String)
            sProductDesc = value
        End Set
    End Property


    Public Property PremiumAmount() As Decimal
        Get
            Return dPremiumAmount
        End Get
        Set(ByVal value As Decimal)
            dPremiumAmount = value
        End Set
    End Property

    Public Property CoverStartDate() As DateTime
        Get
            Return dtCoverStartDate
        End Get
        Set(ByVal value As DateTime)
            dtCoverStartDate = value
        End Set
    End Property

    Public Property CoverEndDate() As DateTime
        Get
            Return dtCoverEndDate
        End Get
        Set(ByVal value As DateTime)
            dtCoverEndDate = value
        End Set
    End Property

    Public Property BGLimit() As Decimal
        Get
            Return dBGLimit
        End Get
        Set(ByVal value As Decimal)
            dBGLimit = value
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

    Public Property ExpiryDate() As DateTime
        Get
            Return dtExpiryDate
        End Get
        Set(ByVal value As DateTime)
            dtExpiryDate = value
        End Set
    End Property
    Public Property ClientShortName() As String
        Get
            Return sClientShortName
        End Get
        Set(ByVal value As String)
            sClientShortName = value
        End Set
    End Property

    Public Property DueDate() As DateTime
        Get
            Return dtDueDate
        End Get
        Set(ByVal value As DateTime)
            dtDueDate = value
        End Set
    End Property

End Class



<Serializable()> Public Class PartyBGPolicyDetailsCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oPartyBGPolicyDetails As PartyBGPolicyDetails In List
            sbPrint.AppendLine(oPartyBGPolicyDetails.print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oPartyBGPolicyDetails As PartyBGPolicyDetails) As Integer
        Return List.Add(v_oPartyBGPolicyDetails)
    End Function

    Public Sub Remove(ByVal v_oPartyBGPolicyDetails As PartyBGPolicyDetails)
        List.Remove(v_oPartyBGPolicyDetails)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As PartyBGPolicyDetails
        Get
            Return List(i)
        End Get
        Set(ByVal value As PartyBGPolicyDetails)
            List(i) = value
        End Set
    End Property

End Class
