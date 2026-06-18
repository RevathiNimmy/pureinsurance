<Serializable()> Public Class CaseDetails : Inherits Claim

    Private iCaseKey As Integer
    Private sCaseNumber As String
    Private dtCaseOpenDate As Date
    Private sAssistant As String
    Private sAnalyst As String
    Private sEventDescription As String
    Private iBaseCaseKey As Integer
    Private bIsLinked As Boolean
    Private sRiskIndex, sCaseProgressDescription As String
    Private sRiskTypex, sCurrencyCode As String
    Private iMaxRowsToFetch As Integer
    Private oLinkedClaims As LinkedClaimCollection
    Private bMaxRowsToFetchFieldSpecified As Boolean
    Private bCaseOpenDateFieldSpecified As Boolean
    Private dTotalIndemnity, dTotalExpense, dTotalExcess As Decimal
    ''' <summary>
    ''' TotalExcess
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TotalExcess() As Decimal
        Get
            Return Me.dTotalExcess
        End Get
        Set(ByVal value As Decimal)
            Me.dTotalExcess = value
        End Set
    End Property

    ''' <summary>
    ''' TotalExpense
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TotalExpense() As Decimal
        Get
            Return Me.dTotalExpense
        End Get
        Set(ByVal value As Decimal)
            Me.dTotalExpense = value
        End Set
    End Property
    ''' <summary>
    ''' TotalIndemnity
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TotalIndemnity() As Decimal
        Get
            Return Me.dTotalIndemnity
        End Get
        Set(ByVal value As Decimal)
            Me.dTotalIndemnity = value
        End Set
    End Property
    ''' <summary>
    ''' CaseProgressDescription
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CaseProgressDescription() As String
        Get
            Return Me.sCaseProgressDescription
        End Get
        Set(ByVal value As String)
            Me.sCaseProgressDescription = value
        End Set
    End Property

    ''' <summary>
    ''' CaseKey
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CaseKey() As Integer
        Get
            Return Me.iCaseKey
        End Get
        Set(ByVal value As Integer)
            Me.iCaseKey = value
        End Set
    End Property

    ''' <summary>
    ''' CaseNumber
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CaseNumber() As String
        Get
            Return Me.sCaseNumber
        End Get
        Set(ByVal value As String)
            Me.sCaseNumber = value
        End Set
    End Property

    ''' <summary>
    ''' CaseOpenDate
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CaseOpenDate() As Date
        Get
            Return Me.dtCaseOpenDate
        End Get
        Set(ByVal value As Date)
            Me.dtCaseOpenDate = value
        End Set
    End Property

    ''' <summary>
    ''' Assistant
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Assistant() As String
        Get
            Return Me.sAssistant
        End Get
        Set(ByVal value As String)
            Me.sAssistant = value
        End Set
    End Property

    ''' <summary>
    ''' Analyst
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Analyst() As String
        Get
            Return Me.sAnalyst
        End Get
        Set(ByVal value As String)
            Me.sAnalyst = value
        End Set
    End Property

    ''' <summary>
    ''' EventDescription
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property EventDescription() As String
        Get
            Return Me.sEventDescription
        End Get
        Set(ByVal value As String)
            Me.sEventDescription = value
        End Set
    End Property

    ''' <summary>
    ''' MaxRowsToFetch
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MaxRowsToFetch() As Integer
        Get
            Return Me.iMaxRowsToFetch
        End Get
        Set(ByVal value As Integer)
            Me.iMaxRowsToFetch = value
        End Set
    End Property
    ''' <summary>
    ''' MaxRowsToFetchSpecified
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Xml.Serialization.XmlIgnoreAttribute()> _
    Public Property MaxRowsToFetchSpecified() As Boolean
        Get
            Return Me.bmaxRowsToFetchFieldSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bmaxRowsToFetchFieldSpecified = value
        End Set
    End Property

    ''' <summary>
    ''' CaseOpenDateSpecified
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Xml.Serialization.XmlIgnoreAttribute()> _
    Public Property CaseOpenDateSpecified() As Boolean
        Get
            Return Me.bCaseOpenDateFieldSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bCaseOpenDateFieldSpecified = value
        End Set
    End Property

    ''' <summary>
    ''' RiskTypex
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RiskTypex() As String
        Get
            Return Me.sRiskTypex
        End Get
        Set(ByVal value As String)
            Me.sRiskTypex = value
        End Set
    End Property

    ''' <summary>
    ''' RiskIndex
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RiskIndex() As String
        Get
            Return Me.sRiskIndex
        End Get
        Set(ByVal value As String)
            Me.sRiskIndex = value
        End Set
    End Property

    ''' <summary>
    ''' IsLinked
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsLinked() As Boolean
        Get
            Return Me.bIsLinked
        End Get
        Set(ByVal value As Boolean)
            Me.bIsLinked = value
        End Set
    End Property

    ''' <summary>
    ''' LinkedClaims
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shadows Property LinkedClaims() As LinkedClaimCollection
        Get
            Return Me.oLinkedClaims
        End Get
        Set(ByVal value As LinkedClaimCollection)
            Me.oLinkedClaims = value
        End Set
    End Property

    Public Sub New()
        oLinkedClaims = New LinkedClaimCollection
    End Sub

    ''' <summary>
    ''' CurrencyCode
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CurrencyCode() As String
        Get
            Return Me.sCurrencyCode
        End Get
        Set(ByVal value As String)
            Me.sCurrencyCode = value
        End Set
    End Property

End Class

<Serializable()> Public Class CaseCollection : Inherits SortableCollectionBase
    Public Sub New()
        MyBase.SortObjectType = GetType(CaseDetails)
    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oCase As CaseDetails In List
            sbPrint.AppendLine(oCase.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function
    Public Function Add(ByVal v_oCase As CaseDetails) As Integer
        Return List.Add(v_oCase)
    End Function

    Public Sub Remove(ByVal v_oCase As CaseDetails)
        List.Remove(v_oCase)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As CaseDetails
        Get
            Return List(i)
        End Get
        Set(ByVal value As CaseDetails)
            List(i) = value
        End Set
    End Property
End Class

<Serializable()> Public Class LinkedClaimCollection : Inherits SortableCollectionBase
    Public Sub New()
        MyBase.SortObjectType = GetType(CaseDetails)
    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oCase As CaseDetails In List
            sbPrint.AppendLine(oCase.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function
    Public Function Add(ByVal v_oCase As CaseDetails) As Integer
        Return List.Add(v_oCase)
    End Function

    Public Sub Remove(ByVal v_oCase As CaseDetails)
        List.Remove(v_oCase)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As CaseDetails
        Get
            Return List(i)
        End Get
        Set(ByVal value As CaseDetails)
            List(i) = value
        End Set
    End Property
End Class


