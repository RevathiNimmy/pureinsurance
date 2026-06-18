<Serializable()> Public Class AccountBalance

    Private dSumAmount As Double
    Private sCurrencyCode As String
    Private aFloatBalance As Double
    Private dOverdraft As Double

    Public Sub New()

    End Sub
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder

        sbPrint.AppendLine("Sum Amount : " & dSumAmount.ToString() & "<br />")
        sbPrint.AppendLine("Currency Code  : " & sCurrencyCode & "<br />")
        sbPrint.AppendLine("Float Balance  : " & aFloatBalance.ToString() & "<br />")
        sbPrint.AppendLine("Overdraft  : " & dOverdraft.ToString() & "<br />")

        sbPrint.AppendLine("<br />")

        Return sbPrint.ToString()

    End Function
    Public Property SumAmount() As Double
        Get
            Return dSumAmount
        End Get
        Set(ByVal value As Double)
            dSumAmount = value
        End Set
    End Property

    ''' <summary>
    ''' Currency Code
    ''' </summary>
    ''' <value>Currency Code</value>
    ''' <returns>Currency Code</returns>
    Public Property CurrencyCode() As String
        Get
            Return sCurrencyCode
        End Get
        Set(ByVal value As String)
            sCurrencyCode = value
        End Set
    End Property

    ''' <summary>
    ''' Float Balance
    ''' </summary>
    ''' <value>Float Balance</value>
    ''' <returns>Float Balance</returns>
    Public Property FloatBalance() As Double
        Get
            Return aFloatBalance
        End Get
        Set(ByVal value As Double)
            aFloatBalance = value
        End Set
    End Property

    ''' <summary>
    ''' Overdraft
    ''' </summary>
    ''' <value>Overdraft</value>
    ''' <returns>Overdraft</returns>
    Public Property Overdraft() As Double
        Get
            Return dOverdraft
        End Get
        Set(ByVal value As Double)
            dOverdraft = value
        End Set
    End Property
End Class

<Serializable()> Public Class AccountBalancecollection : Inherits CollectionBase



    Public Function Add(ByVal v_oAccountBalance As AccountBalance) As Integer

        Return List.Add(v_oAccountBalance)

    End Function

    Public Sub Remove(ByVal v_oAccountBalance As AccountBalance)
        List.Remove(v_oAccountBalance)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As AccountBalance
        Get
            Return List(i)
        End Get
        Set(ByVal value As AccountBalance)
            List(i) = value
        End Set
    End Property

End Class
