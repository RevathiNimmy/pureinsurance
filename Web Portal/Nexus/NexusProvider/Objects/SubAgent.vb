''' <summary>
''' Property Class for SubAgent
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class SubAgent

    Private iPartyKeyField As Integer

    Private sCodeField As String

    Private sNameField As String

    Private dPercentageField As Double

    Private dAmountField As Double


    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Party Key : " & iPartyKeyField.ToString() & "<br />")
        sbPrint.AppendLine("Code : " & sCodeField & "<br />")
        sbPrint.AppendLine("Name : " & sNameField & "<br />")
        sbPrint.AppendLine("Percentage : " & dPercentageField.ToString() & "<br />")
        sbPrint.AppendLine("Amount : " & dAmountField.ToString() & "<br />")

        Return sbPrint.ToString()

    End Function

    Public Property PartyKey() As Integer
        Get
            Return Me.iPartyKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iPartyKeyField = value
        End Set
    End Property


    Public Property Code() As String
        Get
            Return Me.sCodeField
        End Get
        Set(ByVal value As String)
            Me.sCodeField = value
        End Set
    End Property


    Public Property Name() As String
        Get
            Return Me.sNameField
        End Get
        Set(ByVal value As String)
            Me.sNameField = value
        End Set
    End Property


    Public Property Percentage() As Double
        Get
            Return Me.dPercentageField
        End Get
        Set(ByVal value As Double)
            Me.dPercentageField = value
        End Set
    End Property


    Public Property Amount() As Double
        Get
            Return Me.dAmountField
        End Get
        Set(ByVal value As Double)
            Me.dAmountField = value
        End Set
    End Property

End Class

''' <summary>
''' Collection Class to hold subagent objects.
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class SubAgentCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oSubAgent As SubAgent In List
            sbPrint.AppendLine(oSubAgent.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oSubAgent As SubAgent) As Integer
        'v_oSubAgent.PartyKey = List.Add(v_oSubAgent)
        'Return v_oSubAgent.PartyKey
        List.Add(v_oSubAgent)
    End Function

    Public Sub Remove(ByVal v_oSubAgent As SubAgent)
        List.Remove(v_oSubAgent)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As SubAgent
        Get
            Return List(i)
        End Get
        Set(ByVal value As SubAgent)
            List(i) = value
        End Set
    End Property

    Public Function FindItemByKey(ByVal v_iKey As Integer) As SubAgent
        For Each oItem As SubAgent In List
            If oItem.PartyKey = v_iKey Then
                Return oItem
            End If
        Next

        Return Nothing
    End Function
End Class