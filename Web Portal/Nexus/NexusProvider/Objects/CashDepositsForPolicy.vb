<Serializable()> Public Class CashDepositsForPolicy

    Private sAgentType As String
    Private oCashDepositPolicies As CashDepositPoliciesCollection

    Public Sub New()
        oCashDepositPolicies = New CashDepositPoliciesCollection
    End Sub

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Agent Type : " & sAgentType & "<br />")
        sbPrint.AppendLine(oCashDepositPolicies.Print() & "<br />")

        Return sbPrint.ToString()

    End Function

    Public Property AgentType() As String
        Get
            Return sAgentType
        End Get
        Set(ByVal value As String)
            sAgentType = value
        End Set
    End Property

    Public Property CashDepositPolicies() As CashDepositPoliciesCollection
        Get
            Return oCashDepositPolicies
        End Get
        Set(ByVal value As CashDepositPoliciesCollection)
            oCashDepositPolicies = value
        End Set
    End Property

End Class

