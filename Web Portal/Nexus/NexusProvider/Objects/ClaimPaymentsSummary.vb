<Serializable()> Public Class ClaimPaymentsSummary
    ''' <summary>
    ''' to store MediaTypeCode
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MediaTypeCode As String = String.Empty
    ''' <summary>
    ''' to store Amount
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Amount As Decimal = CDec(0.0)
    ''' <summary>
    ''' to store StatusOfTransaction
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property StatusOfTransaction As String = String.Empty
    ''' <summary>
    ''' to store NoOfTransactions
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NoOfTransactions As Integer = 0
    
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("<br />")

        Return sbPrint.ToString()

    End Function


End Class

<Serializable()> Public Class SettleAllClaimPaymentsResults
    ''' <summary>
    ''' to store Summary collection
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Summary As ClaimPaymentsSummaryCollection
    ''' <summary>
    ''' to store Warning collection
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Warning As WarningCollection

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("<br />")

        Return sbPrint.ToString()

    End Function
End Class

''' <summary>
''' Collection Class to hold ClaimPaymentsSummary objects.
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class ClaimPaymentsSummaryCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oClaimPaymentsSummary As ClaimPaymentsSummary In List
            sbPrint.AppendLine(oClaimPaymentsSummary.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oClaimPaymentsSummary As ClaimPaymentsSummary) As Integer
        Return List.Add(v_oClaimPaymentsSummary)
    End Function

    Public Sub Remove(ByVal v_oClaimPaymentsSummary As ClaimPaymentsSummary)
        List.Remove(v_oClaimPaymentsSummary)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As ClaimPaymentsSummary
        Get
            Return List(i)
        End Get
        Set(ByVal value As ClaimPaymentsSummary)
            List(i) = value
        End Set
    End Property

End Class
