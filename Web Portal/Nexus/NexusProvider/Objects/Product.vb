
<Serializable()> Public Class Product

    Private sBranchCode As String
    Private iProductKey As Integer
    Private sProductCode As String
    Private sDescription As String
    Private sSchemeAgencyRef As String
    Private sBlockNumber As String
    Private bConsolidatedLeadAgentCommission As Boolean
    Private iChosen As Integer
    Private bConsolidatedSubAgentCommission As Boolean





    Public Property ProductKey() As Integer
        Get
            Return iProductKey
        End Get
        Set(ByVal value As Integer)
            iProductKey = value
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

    Public Property Description() As String
        Get
            Return sDescription
        End Get
        Set(ByVal value As String)
            sDescription = value
        End Set
    End Property

    Public Property SchemeAgencyRef() As String
        Get
            Return sSchemeAgencyRef
        End Get
        Set(ByVal value As String)
            sSchemeAgencyRef = value
        End Set
    End Property

    Public Property BlockNumber() As String
        Get
            Return sBlockNumber
        End Get
        Set(ByVal value As String)
            sBlockNumber = value
        End Set
    End Property


    Public Property ConsolidatedLeadAgentCommission() As Boolean
        Get
            Return bConsolidatedLeadAgentCommission
        End Get
        Set(ByVal value As Boolean)
            bConsolidatedLeadAgentCommission = value
        End Set
    End Property
    Public Property Chosen() As Integer
        Get
            Return iChosen
        End Get
        Set(ByVal value As Integer)
            iChosen = value
        End Set
    End Property

    Public Property ConsolidatedSubAgentCommission() As Boolean
        Get
            Return bConsolidatedSubAgentCommission
        End Get
        Set(ByVal value As Boolean)
            bConsolidatedSubAgentCommission = value
        End Set
    End Property

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder

        sbPrint.AppendLine("Branch Code : " & sBranchCode & "<br />")
        sbPrint.AppendLine("ProductKey : " & iProductKey.ToString & "<br />")
        sbPrint.AppendLine("ProductCode : " & sProductCode & "<br />")
        sbPrint.AppendLine("Description : " & sDescription & "<br />")
        sbPrint.AppendLine("SchemeAgencyRef : " & sSchemeAgencyRef & "<br />")
        sbPrint.AppendLine("BlockNumber : " & sBlockNumber & "<br />")
        sbPrint.AppendLine("ConsolidatedLeadAgentCommission : " & IIf(bConsolidatedLeadAgentCommission, "true", "false") & "<br />")
        sbPrint.AppendLine("ConsolidatedSubAgentCommission : " & IIf(bConsolidatedSubAgentCommission, "true", "false") & "<br />")

        sbPrint.AppendLine("<br />")

        Return sbPrint.ToString()

    End Function


End Class
<Serializable()> Public Class ProductCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oProduct As Product In List
            sbPrint.AppendLine(oProduct.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oProduct As Product) As Integer
        Return List.Add(v_oProduct)
    End Function

    Public Sub Remove(ByVal v_oProduct As Product)
        List.Remove(v_oProduct)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As Product
        Get
            Return List(i)
        End Get
        Set(ByVal value As Product)
            List(i) = value
        End Set
    End Property

End Class
