''' <summary>
''' Property Class for BalancesAndUnallocatedCredits
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class BalancesAndUnallocatedCredits

    Private sInsuranceRefField As String

    Private iClientKeyField As Integer

    Private iAgentKeyField As Integer

    Private sAgentTypeField As String

    Private bIsFloatBalanceAccountField As Boolean

    Private bIsOverDraftAccountField As Boolean

    Private dFloatBalanceLimitField As Double

    Private dOverDraftLimitField As Double

    Private dtOverDraftExpiryField As DateTime

    Private dAccountBalanceField As Double

    Private oUnallocatedCreditsForAgentsField As UnallocatedCreditCollection

    Private oUnallocatedCreditsForClientsField As UnallocatedCreditCollection

    Public Sub New()
        oUnallocatedCreditsForAgentsField = New UnallocatedCreditCollection()
        oUnallocatedCreditsForClientsField = New UnallocatedCreditCollection()
    End Sub

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Insurance Ref : " & sInsuranceRefField & "<br />")
        sbPrint.AppendLine("Cleint Key : " & iClientKeyField.ToString() & "<br />")
        sbPrint.AppendLine("Agent Key : " & iAgentKeyField.ToString() & "<br />")
        sbPrint.AppendLine("Agent Type : " & sAgentTypeField & "<br />")
        sbPrint.AppendLine("Float Balance Account : " & bIsFloatBalanceAccountField.ToString() & "<br />")
        sbPrint.AppendLine("Over Draft Account : " & bIsOverDraftAccountField.ToString() & "<br />")
        sbPrint.AppendLine("Float Balance Limit : " & dFloatBalanceLimitField.ToString() & "<br />")
        sbPrint.AppendLine("Over Draft Limit : " & dOverDraftLimitField.ToString() & "<br />")
        sbPrint.AppendLine("Over Draft Expiry : " & dtOverDraftExpiryField.ToString() & "<br />")
        sbPrint.AppendLine("Account Balance : " & dAccountBalanceField.ToString() & "<br />")
        sbPrint.AppendLine(oUnallocatedCreditsForAgentsField.Print() & "<br />")
        sbPrint.AppendLine(oUnallocatedCreditsForClientsField.Print() & "<br />")

        Return sbPrint.ToString()

    End Function

    Public Property InsuranceRef() As String
        Get
            Return Me.sInsuranceRefField
        End Get
        Set(ByVal value As String)
            Me.sInsuranceRefField = value
        End Set
    End Property

    Public Property ClientKey() As Integer
        Get
            Return Me.iClientKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iClientKeyField = value
        End Set
    End Property

    Public Property AgentKey() As Integer
        Get
            Return Me.iAgentKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iAgentKeyField = value
        End Set
    End Property

    Public Property AgentType() As String
        Get
            Return Me.sAgentTypeField
        End Get
        Set(ByVal value As String)
            Me.sAgentTypeField = value
        End Set
    End Property

    Public Property IsFloatBalanceAccount() As Boolean
        Get
            Return Me.bIsFloatBalanceAccountField
        End Get
        Set(ByVal value As Boolean)
            Me.bIsFloatBalanceAccountField = value
        End Set
    End Property

    Public Property IsOverDraftAccount() As Boolean
        Get
            Return Me.bIsOverDraftAccountField
        End Get
        Set(ByVal value As Boolean)
            Me.bIsOverDraftAccountField = value
        End Set
    End Property

    Public Property FloatBalanceLimit() As Double
        Get
            Return Me.dFloatBalanceLimitField
        End Get
        Set(ByVal value As Double)
            Me.dFloatBalanceLimitField = value
        End Set
    End Property

    Public Property OverDraftLimit() As Double
        Get
            Return Me.dOverDraftLimitField
        End Get
        Set(ByVal value As Double)
            Me.dOverDraftLimitField = value
        End Set
    End Property

    Public Property OverDraftExpiry() As DateTime
        Get
            Return Me.dtOverDraftExpiryField
        End Get
        Set(ByVal value As Date)
            Me.dtOverDraftExpiryField = value
        End Set
    End Property

    Public Property AccountBalance() As Double
        Get
            Return Me.dAccountBalanceField
        End Get
        Set(ByVal value As Double)
            Me.dAccountBalanceField = value
        End Set
    End Property

  
    Public Property UnallocatedCreditsForAgents() As UnallocatedCreditCollection
        Get
            Return Me.oUnallocatedCreditsForAgentsField
        End Get
        Set(ByVal value As UnallocatedCreditCollection)
            Me.oUnallocatedCreditsForAgentsField = value
        End Set
    End Property

    Public Property UnallocatedCreditsForClients() As UnallocatedCreditCollection
        Get
            Return Me.oUnallocatedCreditsForClientsField
        End Get
        Set(ByVal value As UnallocatedCreditCollection)
            Me.oUnallocatedCreditsForClientsField = value
        End Set
    End Property
End Class
