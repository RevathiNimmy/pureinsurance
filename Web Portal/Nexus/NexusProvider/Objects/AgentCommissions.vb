''' <summary>
''' Nexus currency object
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class AgentCommissions

    Private dTaxValue As Decimal
    Private dCommissionValue As Decimal
    Private sTaxGroup As String
    Private dCommissionRate As Decimal
    Private sAgentCode As String

    ''' <summary>
    ''' Desfault constructor
    ''' </summary>
    Public Sub New()

    End Sub

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder

        sbPrint.AppendLine("<br />")

        Return sbPrint.ToString()

    End Function
    Public Property TaxValue() As Decimal
        Get
            Return dTaxValue
        End Get
        Set(ByVal value As Decimal)
            dTaxValue = value
        End Set
    End Property
    Public Property CommissionValue() As Decimal
        Get
            Return dCommissionValue
        End Get
        Set(ByVal value As Decimal)
            dCommissionValue = value
        End Set
    End Property

    Public Property TaxGroup() As String
        Get
            Return sTaxGroup
        End Get
        Set(ByVal value As String)
            sTaxGroup = value
        End Set
    End Property

    Public Property CommissionRate() As Decimal
        Get
            Return dCommissionRate
        End Get
        Set(ByVal value As Decimal)
            dCommissionRate = value
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

End Class

''' <summary>
''' Collection of currency objects
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class AgentCommissionsCollection : Inherits SortableCollectionBase
    Public Sub New()
        MyBase.SortObjectType = GetType(AgentCommissions)
    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string of the objects contents</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oAgentCommissions As AgentCommissions In List
            sbPrint.AppendLine(oAgentCommissions.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add a Currency object to the collection
    ''' </summary>
    ''' <param name="v_oCurrency">The Currency object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oAgentCommissions As AgentCommissions) As Integer
        Return List.Add(v_oAgentCommissions)
    End Function

    ''' <summary>
    ''' Remove an Currency object from the collection
    ''' </summary>
    ''' <param name="v_oCurrency">The Currency object to be removed</param>
    Public Sub Remove(ByVal v_oAgentCommissions As AgentCommissions)
        List.Remove(v_oAgentCommissions)
    End Sub

    ''' <summary>
    ''' Remove an Currency object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the Currency object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an Currency object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the Currency object</param>
    ''' <value>The replacement Currency object</value>
    ''' <returns>The Currency object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As AgentCommissions
        Get
            Return List(i)
        End Get
        Set(ByVal value As AgentCommissions)
            List(i) = value
        End Set
    End Property

End Class